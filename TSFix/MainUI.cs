using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSFix
{
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
    using CheckResType = Tuple<UInt16, ulong, ulong, long>;
    using MatchResType = Tuple<long, long, long>;
    public partial class MainUI : Form
    {

        class PatchResult
        {
            public long StartPos;
            public long SecondStartPos;
            public long EndPos;
            public long SecondEndPos;
            public PatchResult(long StartPos, long EndPos, long SecondStartPos,long SecondEndPos)
            {
                this.StartPos = StartPos;
                this.EndPos = EndPos;
                this.SecondStartPos = SecondStartPos;
                this.SecondEndPos = SecondEndPos;
            }

            public override string ToString()
            {
                return $"MainStartPos:{StartPos}({(StartPos / 1024.0 / 1024).ToString("F3")} MB) MainEndPos:{EndPos}({(EndPos / 1024.0 / 1024).ToString("F3")} MB){Environment.NewLine}" +
                    $"SecondStartPos:{SecondStartPos}({(SecondStartPos / 1024.0 / 1024).ToString("F3")} MB) SecondEndPos:{SecondEndPos}({(SecondEndPos / 1024.0 / 1024).ToString("F3")} MB)";
            }

        }

        class CheckResultDetail
        {
            long FileSize;
            double TSTimeSeconds;
            decimal Limit;
            string LastDetail = "";
            //PID TEI CC POS
            public List<CheckResType> Result = new List<CheckResType>();
            public CheckResultDetail(long FileSize, double TSTimeSeconds, decimal Limit)
            {
                this.FileSize = FileSize;
                this.TSTimeSeconds = TSTimeSeconds;
                this.Limit = Limit;
            }

            public void Add(UInt16 PID, ulong TEI, ulong CC, long POS)
            {
                LastDetail = ToStringInternal(PID, TEI, CC, POS);
                if (Result.Count < Limit)
                {
                    Result.Add(new CheckResType(PID, TEI, CC, POS));
                }
            }

            static string Seconds2FormatTime(double Seconds)
            {
                TimeSpan time = TimeSpan.FromSeconds(Seconds);
                return time.ToString(@"hh\:mm\:ss");
            }

            private string ToStringInternal(UInt16 PID, ulong TEI, ulong CC, long POS)
            {
                string Result = "";
                if (TSTimeSeconds > 0) Result += $"[{Seconds2FormatTime(TSTimeSeconds * ((double)POS / (double)FileSize))}] ";
                if (PID > 8191)
                {
                    //Sync Result
                    return $"{Result}TS Sync at pos: {POS} ({(POS / 1024 / 1024)} MB {POS * 100 / FileSize}%)";
                }
                return $"{Result}PID:{PID}  TEI:{TEI}  CC:{CC}  POS:{POS} ({(POS / 1024 / 1024)} MB {POS * 100 / FileSize}%)";
            }
            public string ToString(int Index = -1)
            {
                if (Index == -1) return LastDetail;
                if (Result.Count == 0) return "";
                var _Result = Result[Index];
                var PID = _Result.Item1;
                var TEI = _Result.Item2;
                var CC = _Result.Item3;
                var POS = _Result.Item4;
                return ToStringInternal(PID, TEI, CC, POS);
            }

            public string ToStringAll(string ExtraText = "-------Detail-------")
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ExtraText);
                for (int i = 0; i < Result.Count; i++)
                {
                    sb.AppendLine(ToString(i));
                }
                return sb.ToString();
            }

        }
        BackgroundWorker worker = new BackgroundWorker();
        BackgroundWorker ComputePatchWorker = new BackgroundWorker();
        BackgroundWorker OutputWorker = new BackgroundWorker();
        List<MatchResType> MatchResultList = new List<MatchResType>();
        List<PatchResult> PatchResultList = new List<PatchResult>();
        const int TS_PACKET_SIZE = 188;

        public MainUI()
        {
            InitializeComponent();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += ScanWorker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            ComputePatchWorker.WorkerReportsProgress = true;
            ComputePatchWorker.WorkerSupportsCancellation = true;
            ComputePatchWorker.DoWork += ComputeWorker_DoWork;
            ComputePatchWorker.ProgressChanged += Worker_ProgressChanged;
            ComputePatchWorker.RunWorkerCompleted += ComputeWorker_RunWorkerCompleted;


            OutputWorker.WorkerReportsProgress = true;
            OutputWorker.WorkerSupportsCancellation = true;
            OutputWorker.DoWork += OutputWorker_DoWork;
            OutputWorker.ProgressChanged += Worker_ProgressChanged;
            OutputWorker.RunWorkerCompleted += OutputWorker_RunWorkerCompleted;
        }

        private void GetFileName(TextBox TBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false; //是否可以多选true=ok/false=no
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                TBox.Text = openFileDialog.FileName;
            }
        }

        private void OpenMainBtn_Click(object sender, EventArgs e)
        {
            GetFileName(MainTBox);
        }

        private void OpenSecondaryBtn_Click(object sender, EventArgs e)
        {
            GetFileName(SecondTBox);
        }
        private void StartScan(String fileName)
        {
            Dictionary<string, object> Args = new Dictionary<string, object>();
            Args["FileName"] = fileName;
            Args["Padding"] = (long)(Math.Round(PatchPaddingNum.Value / TS_PACKET_SIZE) * TS_PACKET_SIZE);
            Args["MatchLength"] = (long)(Math.Round(MatchLengthNum.Value / TS_PACKET_SIZE) * TS_PACKET_SIZE);
            string[] TSTime = DurationTBox.Text.Replace("：", ":").Split(':');
            double TSTimeSeconds = 0;
            for (int i = 0; i < TSTime.Length; i++)
            {
                if (!Regex.IsMatch(TSTime[i], @"^(\d+\.\d+|\d+)$")) continue;
                TSTimeSeconds += double.Parse(TSTime[i]) * Math.Pow(60, TSTime.Length - 1 - i);
            }
            Args["TSTimeSeconds"] = TSTimeSeconds;
            Args["DetailLimit"] = (int)DetailLimitNum.Value;
            LogTBox.AppendText($"StartScan -> Main File: {fileName}{Environment.NewLine}");
            worker.RunWorkerAsync(Args);
        }

        private void StartCompute(String fileName,String FileNameSecond)
        {
            Dictionary<string, object> Args = new Dictionary<string, object>();
            Args["FileName"] = fileName;
            Args["FileNameSecond"] = FileNameSecond;
            Args["MatchResult"] = MatchResultList;
            Args["PatchOnlyGEQ"] = PatchOnlySIzeGEQCBox.Checked;
            Args["PatchOnlyNoCorrupt"] = PatchNoCorrupt.Checked;
            LogTBox.AppendText($"StartCompute -> Main File: {fileName} Secondary File: {FileNameSecond}{Environment.NewLine}");
            ComputePatchWorker.RunWorkerAsync(Args);
        }

        private void StartOutput(String fileName, String FileNameSecond,String OutputFileName)
        {
            Dictionary<string, object> Args = new Dictionary<string, object>();
            Args["FileName"] = fileName;
            Args["FileNameSecond"] = FileNameSecond;
            Args["FileNameOutput"] = OutputFileName;
            Args["PatchResult"] = PatchResultList;
            LogTBox.AppendText($"StartOutput -> Main File: {fileName} Secondary File: {FileNameSecond} Output File: {OutputFileName}{Environment.NewLine}");
            OutputWorker.RunWorkerAsync(Args);
        }
        private void MainScanBtn_Click(object sender, EventArgs e)
        {
            if (!File.Exists(MainTBox.Text))
            {
                MessageBox.Show("file does not exists.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (worker.IsBusy)
            {
                if (worker.WorkerSupportsCancellation == true)
                {
                    worker.CancelAsync();
                }
                return;
            }
            MainScanBtn.Text = "Stop Scan";
            OutputBtn.Enabled = false;
            ComputeBtn.Enabled = false;
            MatchResultList.Clear();
            PatchResultList.Clear();
            StartScan(MainTBox.Text);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var Result = e.Result as Dictionary<string, object>;
                    var PIDCountDic = new Dictionary<int, ulong>();

                    var PIDCount = Result["PIDCount"] as ulong[];
                    var PIDCCErrorCount = Result["PIDCCErrorCount"] as ulong[];
                    var PIDTEICount = Result["PIDTEICount"] as ulong[];
                    var CheckResult = Result["CheckResult"] as CheckResultDetail;
                    var MatchResult = Result["MatchResult"] as List<MatchResType>;
                    this.MatchResultList = MatchResult;

                    for (int i = 0; i < PIDCount.Length; i++)
                    {
                        if (PIDCount[i] > 0)
                        {
                            PIDCountDic[i] = PIDCount[i];
                        }
                    }

                    ulong TotalError = 0;
                    var ResultSB = new StringBuilder();
                    var MatchResultSB = new StringBuilder();
                    ulong TotalCount = 0;
                    foreach (var Item in PIDCount)
                        TotalCount += Item;

                    foreach (var Item in PIDCountDic.OrderByDescending(x => x.Value))
                    {
                        string Info = $"PID:{Item.Key.ToString("D4")}\tCount:{Item.Value}\t({Item.Value * 100 / TotalCount}%)\tTEI:{PIDTEICount[Item.Key]}\tCC:{PIDCCErrorCount[Item.Key]}";
                        ResultSB.AppendLine(Info);
                        TotalError += (PIDTEICount[Item.Key] + PIDCCErrorCount[Item.Key]);
                    }

                    ResultSB.Append(CheckResult.ToStringAll());

                    foreach (var match in MatchResult)
                    {
                        string Info = $"Corrupt Match: Start {match.Item1}({(match.Item1 / 1024.0 / 1024).ToString("F3")} MB)\t" +
                            $"End: {match.Item2}({(match.Item2 / 1024.0 / 1024).ToString("F3")} MB)\t" +
                            $"Lnegth:{match.Item3}({(match.Item3 / 1024.0 / 1024).ToString("F0")} MB)";
                        MatchResultSB.AppendLine(Info);
                    }


                    LogTBox.Text += $"Total Corrupt: {TotalError}{Environment.NewLine}{ResultSB}{MatchResultSB}";
                }
            }
            catch (Exception _e)
            {
                MessageBox.Show(_e.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                MainScanBtn.Text = "Scan";
                MainScanBtn.Enabled = true;
                OutputBtn.Enabled = true;
                ComputeBtn.Enabled = true;
            }
        }

        private void ComputeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var Result = e.Result as Dictionary<string, object>;

                    var PatchResult = Result["PatchResult"] as List<PatchResult>;
                    PatchResultList = PatchResult;
                    var PatchResultSB = new StringBuilder();
                    foreach (var patch in PatchResultList) {
                        PatchResultSB.AppendLine(patch.ToString());
                    }

                    LogTBox.Text += $"Patch result (total: {PatchResultList.Count}){Environment.NewLine}{PatchResultSB}";
                }
            }
            catch (Exception _e)
            {
                MessageBox.Show(_e.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                ComputeBtn.Text = "Compute Main TS Patch";
                MainScanBtn.Enabled = true;
                OutputBtn.Enabled = true;
            }
        }

        private void OutputWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var Result = e.Result as Dictionary<string, object>;

                    var PatchSuccessResult = Result["PatchSuccessResult"] as List<PatchResult>;
                    var PatchResultSB = new StringBuilder();
                    foreach (var patch in PatchSuccessResult)
                    {
                        PatchResultSB.AppendLine(patch.ToString());
                    }

                    LogTBox.Text += $"Patch success result (total: {PatchSuccessResult.Count}){Environment.NewLine}{PatchResultSB}";
                }
            }
            catch (Exception _e)
            {
                MessageBox.Show(_e.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                OutputBtn.Text = "Output Patched FIle";
                MainScanBtn.Enabled = true;
                ComputeBtn.Enabled = true;
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string UserState = e.UserState as string;
            if(e.ProgressPercentage == -1)
            {
                LogTBox.AppendText(UserState + Environment.NewLine);
            }
            else LogTBox.AppendText($"[{e.ProgressPercentage}%] " + UserState + Environment.NewLine);
            LogTBox.SelectionStart = LogTBox.Text.Length;
            LogTBox.ScrollToCaret();
        }

        private void ScanWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var Args = e.Argument as Dictionary<string, object>;
            BackgroundWorker worker = sender as BackgroundWorker;
            var FileName = Args["FileName"] as string;
            var Padding = (long)Args["Padding"];
            var MatchLength = (long)Args["MatchLength"];
            var TSTimeSeconds = (double)Args["TSTimeSeconds"];
            var DetailLimit = (int)Args["DetailLimit"];
            byte[] TSBuffer = new byte[TS_PACKET_SIZE];
            bool TSSync = false;
            bool Exit = false;
            Int64 LastSyncPos = -1;
            Int64 LastPacketPos = -1;
            Int64 LastFilePos = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            var PIDCount = new ulong[8192];
            var PIDCCErrorCount = new ulong[8192];
            var PIDTEICount = new ulong[8192];
            var LastPIDCC = new sbyte[8192];
            long LastCorruptPos = 0;
            for (int i = 0; i < LastPIDCC.Length; i++)
                LastPIDCC[i] = -1;

            var FileSize = new FileInfo(FileName).Length;
            CheckResultDetail CheckResult = new CheckResultDetail(FileSize, TSTimeSeconds, DetailLimit);
            List<MatchResType> MatchResult = new List<MatchResType>();
            var Result = new Dictionary<string, object>();

            Result["PIDCount"] = PIDCount;
            Result["PIDCCErrorCount"] = PIDCCErrorCount;
            Result["PIDTEICount"] = PIDTEICount;

            Result["CheckResult"] = CheckResult;
            Result["MatchResult"] = MatchResult;

            e.Result = Result;

            long CorruptStartMatchPos = -1;

            using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1 << 17)) // 1 << 17 == 128K
            {
                long EndPos = FileSize;
                while (true)
                {
                    if (stopwatch.ElapsedMilliseconds > 1000)
                    {
                        long CurrentPos = fs.Position;
                        int Progress = (int)((CurrentPos * 100 / EndPos));
                        string Speed = $"{((CurrentPos - LastFilePos) / 1024 / 1024).ToString("F0")} MB/s";
                        worker.ReportProgress(Progress, $"[{Speed}] " + CheckResult.ToString());
                        LastFilePos = CurrentPos;
                        stopwatch.Restart();

                        if (worker.CancellationPending == true)
                        {
                            Exit = true; break;
                        }
                    }
                    while (true)
                    {
                        if (fs.Read(TSBuffer, 0, 1) == 0) { Exit = true; break; }

                        if (TSBuffer[0] == 0x47)
                        {
                            if (!TSSync)
                            {
                                LastSyncPos = fs.Position - 1;
                                CheckResult.Add(8192, 0, 0, LastSyncPos);
                                TSSync = true;
                            }

                            if (fs.Read(TSBuffer, 1, TS_PACKET_SIZE - 1) < TS_PACKET_SIZE - 1) Exit = true;
                            LastPacketPos = fs.Position - TS_PACKET_SIZE;
                            break;
                        }
                        if (worker.CancellationPending == true)
                        {
                            Exit = true; break;
                        }
                        TSSync = false;
                    }

                    if (Exit) break;

                    byte TEI = (byte)(TSBuffer[1] >> 7);
                    UInt16 PID = (UInt16)(((TSBuffer[1] & 0x1f) << 8) | TSBuffer[2]);
                    byte AFC = (byte)((TSBuffer[3] & 0x30) >> 4);
                    sbyte CC = (sbyte)((TSBuffer[3] & 0x0f));

                    PIDCount[PID]++;

                    if (TEI == 1)
                    {
                        PIDTEICount[PID]++;
                        CheckResult.Add(PID, PIDTEICount[PID], PIDCCErrorCount[PID], LastPacketPos);
                        if(CorruptStartMatchPos == -1)
                        {
                            if (LastPacketPos - LastCorruptPos >= Padding + MatchLength)
                            {
                                CorruptStartMatchPos = LastPacketPos - Padding - MatchLength;

                            }
                        }
                        LastCorruptPos = LastPacketPos;
                    }

                    if (AFC == 1 || AFC == 3)
                    {
                        if (LastPIDCC[PID] == -1)
                            LastPIDCC[PID] = CC;
                        else
                        {
                            if (!((((LastPIDCC[PID] + 1) % 16) == CC) ||
                                (LastPIDCC[PID] == CC)))
                            {
                                PIDCCErrorCount[PID]++;
                                CheckResult.Add(PID, PIDTEICount[PID], PIDCCErrorCount[PID], LastPacketPos);

                                if (CorruptStartMatchPos == -1)
                                {
                                    if (LastPacketPos - LastCorruptPos >= Padding + MatchLength)
                                    {
                                        CorruptStartMatchPos = LastPacketPos - Padding - MatchLength;
                                    }
                                }
                                LastCorruptPos = LastPacketPos;
                            }
                            LastPIDCC[PID] = CC;
                        }

                    }

                    if (CorruptStartMatchPos != -1 && LastPacketPos >= LastCorruptPos + Padding + MatchLength)
                    {
                        MatchResult.Add(new MatchResType(CorruptStartMatchPos, LastCorruptPos + TS_PACKET_SIZE + Padding, MatchLength));
                        CorruptStartMatchPos = -1;
                    }
                }
            }

        }

        private (List<byte[]>,long) ReadTSList(FileStream fs,long StartPos,int ReadCount)
        {
            var result = new List<byte[]>();
            fs.Seek(StartPos, SeekOrigin.Begin);
            long SyncPos = -1;
            while (result.Count < ReadCount)
            {
                byte[] TSBuffer = new byte[TS_PACKET_SIZE];
                if (fs.Read(TSBuffer, 0, 1) == 0) break;

                if (TSBuffer[0] == 0x47)
                {
                    if (SyncPos == -1)
                    {
                        SyncPos = fs.Position - 1;
                    }
                    if (fs.Read(TSBuffer, 1, TS_PACKET_SIZE - 1) < TS_PACKET_SIZE - 1) break;
                    result.Add(TSBuffer);

                }else {
                    if (result.Count > 0)
                        result.Clear();
                    SyncPos = -1;
                }
            }
            return (result, SyncPos);
        }

        private void ComputeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var Args = e.Argument as Dictionary<string, object>;
            BackgroundWorker worker = sender as BackgroundWorker;
            var FileName = Args["FileName"] as string;
            var FileNameSecond = Args["FileNameSecond"] as string;
            var MatchResult = Args["MatchResult"] as List<MatchResType>;
            var PatchOnlyGEQ = (bool)Args["PatchOnlyGEQ"];
            var PatchOnlyNoCorrupt = (bool)Args["PatchOnlyNoCorrupt"];
            byte[] TSBuffer = new byte[TS_PACKET_SIZE];
            bool Exit = false;
            Int64 LastPacketPos = -1;
            long LastFilePos = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var LastPIDCC = new sbyte[8192];
            long LastCorruptPos = 0;
            for (int i = 0; i < LastPIDCC.Length; i++)
                LastPIDCC[i] = -1;

            var FileSize = new FileInfo(FileNameSecond).Length;
            var Result = new Dictionary<string, object>();
            List<PatchResult> PatchResultList = new List<PatchResult>();

            Result["PatchResult"] = PatchResultList;

            e.Result = Result;
            using(var fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1 << 17))
            {
                List <(List<byte[]>,long)> AllMatchStart = new List<(List<byte[]>, long)>();
                List <(List<byte[]>, long)> AllMatchEnd = new List<(List<byte[]>, long)>();
                worker.ReportProgress(0, $"reading main TS segment");
                for(int i = 0;i < MatchResult.Count; i++)
                {
                    var match = MatchResult[i];
                    worker.ReportProgress(0, $"[{i + 1}/{MatchResult.Count}] reading main TS segment");
                    var (MatchStartList, FileMatchStartPos) = ReadTSList(fs, match.Item1, (int)match.Item3 / TS_PACKET_SIZE);
                    var (MatchEndList, FileMatchEndPos) = ReadTSList(fs, match.Item2, (int)match.Item3 / TS_PACKET_SIZE);

                    if (MatchStartList.Count * TS_PACKET_SIZE != match.Item3 || MatchEndList.Count * TS_PACKET_SIZE != match.Item3)
                    {
                        throw new Exception("main TS read error");
                    }
                    AllMatchStart.Add((MatchStartList, FileMatchStartPos));
                    AllMatchEnd.Add((MatchEndList, FileMatchEndPos));
                }

                using (var fs2 = new FileStream(FileNameSecond, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1 << 17)) // 1 << 17 == 128K
                {
                    long EndPos = FileSize;
                    int LastNotAllMatchIndex = -1;
                    long LastNotAllMatchStartPos = -1;
                    int LastMatchStartSuccessIndex = -1;
                    long FileSecondMatchSuccessStartPos = -1;
                    long FileSecondMatchSuccessEndPos = -1;

                    int NextMatchIndex = 0;
                    int FileSecondMatchStartIndex = 0;
                    int FileSecondMatchEndIndex = 0;

                    while (true)
                    {
                        if (stopwatch.ElapsedMilliseconds > 1000)
                        {
                            long CurrentPos = fs2.Position;
                            int Progress = (int)((CurrentPos * 100 / EndPos));
                            string Speed = $"{((CurrentPos - LastFilePos) / 1024 / 1024).ToString("F0")} MB/s";
                            worker.ReportProgress(Progress, $"[{Speed}] [{(NextMatchIndex >= MatchResult.Count ? MatchResult.Count : NextMatchIndex + 1)}/{MatchResult.Count}] searching secondary TS segment");
                            LastFilePos = CurrentPos;
                            stopwatch.Restart();
                            if (worker.CancellationPending == true)
                            {
                                Exit = true; break;
                            }
                        }
                        bool TSReadOK = false;
                        while (true)
                        {
                            if (fs2.Read(TSBuffer, 0, 1) == 0) break;

                            if (TSBuffer[0] == 0x47)
                            {
                                if (fs2.Read(TSBuffer, 1, TS_PACKET_SIZE - 1) == TS_PACKET_SIZE - 1) TSReadOK = true;
                                LastPacketPos = fs2.Position - TS_PACKET_SIZE;
                                break;
                            }
                            if (worker.CancellationPending == true)
                            {
                                Exit = true; break;
                            }
                        }
                        if (Exit) break;
                        if (!TSReadOK) break;

                        byte TEI = (byte)(TSBuffer[1] >> 7);
                        UInt16 PID = (UInt16)(((TSBuffer[1] & 0x1f) << 8) | TSBuffer[2]);
                        byte AFC = (byte)((TSBuffer[3] & 0x30) >> 4);
                        sbyte CC = (sbyte)((TSBuffer[3] & 0x0f));

                        if (TEI == 1)
                        {
                            LastCorruptPos = LastPacketPos;
                        }

                        if (AFC == 1 || AFC == 3)
                        {
                            if (LastPIDCC[PID] == -1)
                                LastPIDCC[PID] = CC;
                            else
                            {
                                if (!((((LastPIDCC[PID] + 1) % 16) == CC) ||
                                    (LastPIDCC[PID] == CC)))
                                {
                                    LastCorruptPos = LastPacketPos;
                                }
                                LastPIDCC[PID] = CC;
                            }

                        }
                        int i = LastNotAllMatchIndex != -1 ? LastNotAllMatchIndex : NextMatchIndex;
                        while (i < AllMatchStart.Count)
                        {
                            var (MatchStartList, FileMatchStartPos) = AllMatchStart[i];

                            if (TSBuffer.SequenceEqual(MatchStartList[FileSecondMatchStartIndex]))
                            {
                                if(FileSecondMatchStartIndex == 0)
                                {
                                    LastNotAllMatchIndex = i;
                                    LastNotAllMatchStartPos = LastPacketPos;
                                }
                                FileSecondMatchStartIndex++;

                                if (FileSecondMatchStartIndex >= MatchStartList.Count)
                                {
                                    LastMatchStartSuccessIndex = i;
                                    FileSecondMatchSuccessStartPos = LastNotAllMatchStartPos;

                                    LastNotAllMatchIndex = -1;
                                    LastNotAllMatchStartPos = -1;
                                    NextMatchIndex = i + 1;

                                    FileSecondMatchStartIndex = 0;
                                    FileSecondMatchEndIndex = 0;
                                }
                                break;
                            }
                            else
                            {
                                if (LastNotAllMatchIndex != -1)
                                {
                                    i = NextMatchIndex;
                                    LastNotAllMatchIndex = -1;
                                    LastNotAllMatchStartPos = -1;
                                }
                                else
                                {
                                    i++;
                                }
                                FileSecondMatchStartIndex = 0;
                            }
                        }

                        if(LastMatchStartSuccessIndex != -1)
                        {
                            var (MatchEndList, FileMatchEndPos) = AllMatchEnd[LastMatchStartSuccessIndex];

                            if (TSBuffer.SequenceEqual(MatchEndList[FileSecondMatchEndIndex]))
                            {
                                if(FileSecondMatchEndIndex == 0)
                                {
                                    FileSecondMatchSuccessEndPos = LastPacketPos;
                                }
                                FileSecondMatchEndIndex++;

                                if (FileSecondMatchEndIndex >= MatchEndList.Count)
                                {
                                    var (MatchStartList2, FileMatchStartPos2) = AllMatchStart[LastMatchStartSuccessIndex];
                                    var match = MatchResult[LastMatchStartSuccessIndex];
                                    var diff = (FileSecondMatchSuccessEndPos - (FileSecondMatchSuccessStartPos + match.Item3)) - (FileMatchEndPos - (FileMatchStartPos2 + match.Item3));
                                    if (((PatchOnlyGEQ && diff >= 0) || !PatchOnlyGEQ) && 
                                        ( !PatchOnlyNoCorrupt || !(LastCorruptPos >= FileSecondMatchSuccessStartPos + match.Item3 && LastCorruptPos < FileSecondMatchSuccessEndPos) ))
                                    {
                                        PatchResultList.Add(new PatchResult(FileMatchStartPos2 + match.Item3, FileMatchEndPos,
                                        FileSecondMatchSuccessStartPos + match.Item3, FileSecondMatchSuccessEndPos));
                                    }

                                    LastMatchStartSuccessIndex = -1;
                                }
                            }
                            else
                            {
                                FileSecondMatchEndIndex = 0;
                                FileSecondMatchSuccessEndPos = -1;
                            }
                        }
                    }
                }
            }
        }

        private void OutputWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var Args = e.Argument as Dictionary<string, object>;
            BackgroundWorker worker = sender as BackgroundWorker;
            var FileName = Args["FileName"] as string;
            var FileNameSecond = Args["FileNameSecond"] as string;
            var FileNameOutput = Args["FileNameOutput"] as string;
            var PatchResult = Args["PatchResult"] as List<PatchResult>;
            var PatchSuccessResult = new List<PatchResult>();
            byte[] buffer = new byte[2048];
            bool Exit = false;
            long LastFilePos = 0;
            var Result = new Dictionary<string, object>();
            Result["PatchSuccessResult"] = PatchSuccessResult;
            e.Result = Result;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var outfs = new FileStream(FileNameOutput, FileMode.Create, FileAccess.Write, FileShare.Read, 1 << 17))
            {
                using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1 << 17))
                {
                    long EndPos = fs.Length;
                    string EndPosStr = $"{(EndPos / 1024 / 1024).ToString("F0")}";
                    int ReadSize;

                    using (var fs2 = new FileStream(FileNameSecond, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1 << 17))
                    {
                        for (int i = 0; i < PatchResult.Count; i++)
                        {
                            var patch = PatchResult[i];

                            while (patch.StartPos - fs.Position > 0)
                            {
                                ReadSize = (int)(patch.StartPos - fs.Position > buffer.Length ? buffer.Length : patch.StartPos - fs.Position);
                                ReadSize = fs.Read(buffer, 0, ReadSize);
                                outfs.Write(buffer, 0, (int)ReadSize);
                                if (stopwatch.ElapsedMilliseconds > 1000)
                                {
                                    long CurrentPos = fs.Position;
                                    int Progress = (int)((CurrentPos * 100 / EndPos));
                                    string Speed = $"{((CurrentPos - LastFilePos) / 1024 / 1024).ToString("F0")} MB/s";
                                    string CurrentPosStr = $"{(CurrentPos / 1024 / 1024).ToString("F0")}";
                                    worker.ReportProgress(Progress, $"[{Speed}] {CurrentPosStr}/{EndPosStr} MB output to {FileNameOutput}");
                                    LastFilePos = CurrentPos;
                                    stopwatch.Restart();
                                    if (worker.CancellationPending == true)
                                    {
                                        Exit = true; break;
                                    }
                                }
                            }
                            if (Exit) break;
                            worker.ReportProgress(-1, $"now patching: {patch.ToString()}");
                            fs2.Seek(patch.SecondStartPos, SeekOrigin.Begin);
                            while (patch.SecondEndPos - fs2.Position > 0)
                            {
                                ReadSize = (int)(patch.SecondEndPos - fs2.Position > buffer.Length ? buffer.Length : patch.SecondEndPos - fs2.Position);
                                ReadSize = fs2.Read(buffer, 0, ReadSize);
                                outfs.Write(buffer, 0, (int)ReadSize);
                                if (worker.CancellationPending == true)
                                {
                                    Exit = true; break;
                                }
                            }
                            if (Exit) break;
                            PatchSuccessResult.Add(patch);
                            fs.Seek(patch.EndPos, SeekOrigin.Begin);
                        }

                    }
                    while ((ReadSize = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outfs.Write(buffer, 0, ReadSize);
                        if (stopwatch.ElapsedMilliseconds > 1000)
                        {
                            long CurrentPos = fs.Position;
                            int Progress = (int)((CurrentPos * 100 / EndPos));
                            string Speed = $"{((CurrentPos - LastFilePos) / 1024 / 1024).ToString("F0")} MB/s";
                            string CurrentPosStr = $"{(CurrentPos / 1024 / 1024).ToString("F0")}";
                            worker.ReportProgress(Progress, $"[{Speed}] {CurrentPosStr}/{EndPosStr} MB output to {FileNameOutput}");
                            LastFilePos = CurrentPos;
                            stopwatch.Restart();
                            if (worker.CancellationPending == true)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void ComputeBtn_Click(object sender, EventArgs e)
        {
            if (!File.Exists(MainTBox.Text))
            {
                MessageBox.Show("main file does not exists.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!File.Exists(SecondTBox.Text))
            {
                MessageBox.Show("second file does not exists.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MatchResultList.Count == 0)
            {
                MessageBox.Show("not segment to compute.", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (ComputePatchWorker.IsBusy)
            {
                if (ComputePatchWorker.WorkerSupportsCancellation == true)
                {
                    ComputePatchWorker.CancelAsync();
                }
                return;
            }
            ComputeBtn.Text = "Stop Compute";
            OutputBtn.Enabled = false;
            MainScanBtn.Enabled = false;
            StartCompute(MainTBox.Text,SecondTBox.Text);
        }

        private void OutputBtn_Click(object sender, EventArgs e)
        {
            if (OutputWorker.IsBusy)
            {
                if (OutputWorker.WorkerSupportsCancellation == true)
                {
                    OutputWorker.CancelAsync();
                }
                return;
            }
            if (!File.Exists(MainTBox.Text))
            {
                MessageBox.Show("main file does not exists.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!File.Exists(SecondTBox.Text))
            {
                MessageBox.Show("second file does not exists.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PatchResultList.Count == 0)
            {
                MessageBox.Show("not segment to patch.", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string OutputFileName = saveFileDialog.FileName;
                OutputBtn.Text = "Stop Output";
                ComputeBtn.Enabled = false;
                MainScanBtn.Enabled = false;
                StartOutput(MainTBox.Text, SecondTBox.Text,OutputFileName);
            }
        }

        private void TBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void MainTBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                MainTBox.Text = files[0];
            }
        }

        private void SecondTBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                SecondTBox.Text = files[0];
            }
        }

        private void ClearLogBtn_Click(object sender, EventArgs e)
        {
            LogTBox.Clear();
        }
    }
}
