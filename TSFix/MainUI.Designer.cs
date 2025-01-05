namespace TSFix
{
    partial class MainUI
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainUI));
            this.MainTBox = new System.Windows.Forms.TextBox();
            this.MainScanBtn = new System.Windows.Forms.Button();
            this.OpenMainBtn = new System.Windows.Forms.Button();
            this.SecondTBox = new System.Windows.Forms.TextBox();
            this.OpenSecondaryBtn = new System.Windows.Forms.Button();
            this.ComputeBtn = new System.Windows.Forms.Button();
            this.PatchPaddingNum = new System.Windows.Forms.NumericUpDown();
            this.PaddingLbl = new System.Windows.Forms.Label();
            this.PatchBytesLbl = new System.Windows.Forms.Label();
            this.OutputBtn = new System.Windows.Forms.Button();
            this.LogTBox = new System.Windows.Forms.RichTextBox();
            this.MatchLengthNum = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.MatchOffsetLbl = new System.Windows.Forms.Label();
            this.DurationTBox = new System.Windows.Forms.TextBox();
            this.DurationLbl = new System.Windows.Forms.Label();
            this.DetailLimitNum = new System.Windows.Forms.NumericUpDown();
            this.DetailLimitLbl = new System.Windows.Forms.Label();
            this.PatchOnlySIzeGEQCBox = new System.Windows.Forms.CheckBox();
            this.PatchNoCorrupt = new System.Windows.Forms.CheckBox();
            this.ClearLogBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PatchPaddingNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatchLengthNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetailLimitNum)).BeginInit();
            this.SuspendLayout();
            // 
            // MainTBox
            // 
            this.MainTBox.AllowDrop = true;
            this.MainTBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTBox.Font = new System.Drawing.Font("Arial", 9F);
            this.MainTBox.Location = new System.Drawing.Point(136, 12);
            this.MainTBox.Name = "MainTBox";
            this.MainTBox.Size = new System.Drawing.Size(132, 21);
            this.MainTBox.TabIndex = 0;
            this.MainTBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainTBox_DragDrop);
            this.MainTBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.TBox_DragEnter);
            // 
            // MainScanBtn
            // 
            this.MainScanBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MainScanBtn.Font = new System.Drawing.Font("Arial", 9F);
            this.MainScanBtn.Location = new System.Drawing.Point(458, 11);
            this.MainScanBtn.Name = "MainScanBtn";
            this.MainScanBtn.Size = new System.Drawing.Size(78, 23);
            this.MainScanBtn.TabIndex = 1;
            this.MainScanBtn.Text = "Scan";
            this.MainScanBtn.UseVisualStyleBackColor = true;
            this.MainScanBtn.Click += new System.EventHandler(this.MainScanBtn_Click);
            // 
            // OpenMainBtn
            // 
            this.OpenMainBtn.Font = new System.Drawing.Font("Arial", 9F);
            this.OpenMainBtn.Location = new System.Drawing.Point(12, 10);
            this.OpenMainBtn.Name = "OpenMainBtn";
            this.OpenMainBtn.Size = new System.Drawing.Size(119, 23);
            this.OpenMainBtn.TabIndex = 1;
            this.OpenMainBtn.Text = "Open Main TS File";
            this.OpenMainBtn.UseVisualStyleBackColor = true;
            this.OpenMainBtn.Click += new System.EventHandler(this.OpenMainBtn_Click);
            // 
            // SecondTBox
            // 
            this.SecondTBox.AllowDrop = true;
            this.SecondTBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SecondTBox.Font = new System.Drawing.Font("Arial", 9F);
            this.SecondTBox.Location = new System.Drawing.Point(153, 41);
            this.SecondTBox.Name = "SecondTBox";
            this.SecondTBox.Size = new System.Drawing.Size(216, 21);
            this.SecondTBox.TabIndex = 0;
            this.SecondTBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.SecondTBox_DragDrop);
            this.SecondTBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.TBox_DragEnter);
            // 
            // OpenSecondaryBtn
            // 
            this.OpenSecondaryBtn.Font = new System.Drawing.Font("Arial", 9F);
            this.OpenSecondaryBtn.Location = new System.Drawing.Point(12, 39);
            this.OpenSecondaryBtn.Name = "OpenSecondaryBtn";
            this.OpenSecondaryBtn.Size = new System.Drawing.Size(135, 23);
            this.OpenSecondaryBtn.TabIndex = 1;
            this.OpenSecondaryBtn.Text = "Open Secondary TS";
            this.OpenSecondaryBtn.UseVisualStyleBackColor = true;
            this.OpenSecondaryBtn.Click += new System.EventHandler(this.OpenSecondaryBtn_Click);
            // 
            // ComputeBtn
            // 
            this.ComputeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ComputeBtn.Font = new System.Drawing.Font("Arial", 9F);
            this.ComputeBtn.Location = new System.Drawing.Point(375, 41);
            this.ComputeBtn.Name = "ComputeBtn";
            this.ComputeBtn.Size = new System.Drawing.Size(160, 23);
            this.ComputeBtn.TabIndex = 1;
            this.ComputeBtn.Text = "Compute Main TS Patch ";
            this.ComputeBtn.UseVisualStyleBackColor = true;
            this.ComputeBtn.Click += new System.EventHandler(this.ComputeBtn_Click);
            // 
            // PatchPaddingNum
            // 
            this.PatchPaddingNum.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PatchPaddingNum.Location = new System.Drawing.Point(105, 72);
            this.PatchPaddingNum.Maximum = new decimal(new int[] {
            104857600,
            0,
            0,
            0});
            this.PatchPaddingNum.Minimum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.PatchPaddingNum.Name = "PatchPaddingNum";
            this.PatchPaddingNum.Size = new System.Drawing.Size(88, 21);
            this.PatchPaddingNum.TabIndex = 2;
            this.PatchPaddingNum.Value = new decimal(new int[] {
            5242880,
            0,
            0,
            0});
            // 
            // PaddingLbl
            // 
            this.PaddingLbl.AutoSize = true;
            this.PaddingLbl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PaddingLbl.Location = new System.Drawing.Point(12, 74);
            this.PaddingLbl.Name = "PaddingLbl";
            this.PaddingLbl.Size = new System.Drawing.Size(87, 15);
            this.PaddingLbl.TabIndex = 3;
            this.PaddingLbl.Text = "Patch Padding";
            // 
            // PatchBytesLbl
            // 
            this.PatchBytesLbl.AutoSize = true;
            this.PatchBytesLbl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PatchBytesLbl.Location = new System.Drawing.Point(199, 74);
            this.PatchBytesLbl.Name = "PatchBytesLbl";
            this.PatchBytesLbl.Size = new System.Drawing.Size(37, 15);
            this.PatchBytesLbl.TabIndex = 3;
            this.PatchBytesLbl.Text = "Bytes";
            // 
            // OutputBtn
            // 
            this.OutputBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputBtn.Font = new System.Drawing.Font("Arial", 9F);
            this.OutputBtn.Location = new System.Drawing.Point(376, 99);
            this.OutputBtn.Name = "OutputBtn";
            this.OutputBtn.Size = new System.Drawing.Size(160, 23);
            this.OutputBtn.TabIndex = 1;
            this.OutputBtn.Text = "Output Patched FIle";
            this.OutputBtn.UseVisualStyleBackColor = true;
            this.OutputBtn.Click += new System.EventHandler(this.OutputBtn_Click);
            // 
            // LogTBox
            // 
            this.LogTBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogTBox.Location = new System.Drawing.Point(12, 171);
            this.LogTBox.Name = "LogTBox";
            this.LogTBox.Size = new System.Drawing.Size(524, 175);
            this.LogTBox.TabIndex = 4;
            this.LogTBox.Text = "";
            // 
            // MatchLengthNum
            // 
            this.MatchLengthNum.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MatchLengthNum.Location = new System.Drawing.Point(105, 99);
            this.MatchLengthNum.Maximum = new decimal(new int[] {
            104857600,
            0,
            0,
            0});
            this.MatchLengthNum.Minimum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.MatchLengthNum.Name = "MatchLengthNum";
            this.MatchLengthNum.Size = new System.Drawing.Size(88, 21);
            this.MatchLengthNum.TabIndex = 2;
            this.MatchLengthNum.Value = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(199, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Bytes";
            // 
            // MatchOffsetLbl
            // 
            this.MatchOffsetLbl.AutoSize = true;
            this.MatchOffsetLbl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MatchOffsetLbl.Location = new System.Drawing.Point(19, 102);
            this.MatchOffsetLbl.Name = "MatchOffsetLbl";
            this.MatchOffsetLbl.Size = new System.Drawing.Size(80, 15);
            this.MatchOffsetLbl.TabIndex = 3;
            this.MatchOffsetLbl.Text = "Match Length";
            // 
            // DurationTBox
            // 
            this.DurationTBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DurationTBox.Font = new System.Drawing.Font("Arial", 9F);
            this.DurationTBox.Location = new System.Drawing.Point(394, 12);
            this.DurationTBox.Name = "DurationTBox";
            this.DurationTBox.Size = new System.Drawing.Size(58, 21);
            this.DurationTBox.TabIndex = 0;
            // 
            // DurationLbl
            // 
            this.DurationLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DurationLbl.AutoSize = true;
            this.DurationLbl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DurationLbl.Location = new System.Drawing.Point(270, 15);
            this.DurationLbl.Name = "DurationLbl";
            this.DurationLbl.Size = new System.Drawing.Size(121, 15);
            this.DurationLbl.TabIndex = 3;
            this.DurationLbl.Text = "Duration (hh:mm:ss)";
            // 
            // DetailLimitNum
            // 
            this.DetailLimitNum.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DetailLimitNum.Location = new System.Drawing.Point(341, 72);
            this.DetailLimitNum.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.DetailLimitNum.Name = "DetailLimitNum";
            this.DetailLimitNum.Size = new System.Drawing.Size(86, 21);
            this.DetailLimitNum.TabIndex = 2;
            this.DetailLimitNum.Value = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            // 
            // DetailLimitLbl
            // 
            this.DetailLimitLbl.AutoSize = true;
            this.DetailLimitLbl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DetailLimitLbl.Location = new System.Drawing.Point(242, 74);
            this.DetailLimitLbl.Name = "DetailLimitLbl";
            this.DetailLimitLbl.Size = new System.Drawing.Size(93, 15);
            this.DetailLimitLbl.TabIndex = 3;
            this.DetailLimitLbl.Text = "Log Detail Limit";
            // 
            // PatchOnlySIzeGEQCBox
            // 
            this.PatchOnlySIzeGEQCBox.AutoSize = true;
            this.PatchOnlySIzeGEQCBox.Checked = true;
            this.PatchOnlySIzeGEQCBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PatchOnlySIzeGEQCBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PatchOnlySIzeGEQCBox.Location = new System.Drawing.Point(15, 126);
            this.PatchOnlySIzeGEQCBox.Name = "PatchOnlySIzeGEQCBox";
            this.PatchOnlySIzeGEQCBox.Size = new System.Drawing.Size(403, 19);
            this.PatchOnlySIzeGEQCBox.TabIndex = 5;
            this.PatchOnlySIzeGEQCBox.Text = "Patch only secondary TS segment size greater or equal than main TS";
            this.PatchOnlySIzeGEQCBox.UseVisualStyleBackColor = true;
            // 
            // PatchNoCorrupt
            // 
            this.PatchNoCorrupt.AutoSize = true;
            this.PatchNoCorrupt.Checked = true;
            this.PatchNoCorrupt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PatchNoCorrupt.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PatchNoCorrupt.Location = new System.Drawing.Point(15, 146);
            this.PatchNoCorrupt.Name = "PatchNoCorrupt";
            this.PatchNoCorrupt.Size = new System.Drawing.Size(294, 19);
            this.PatchNoCorrupt.TabIndex = 5;
            this.PatchNoCorrupt.Text = "Patch only secondary TS segment has no corrupt";
            this.PatchNoCorrupt.UseVisualStyleBackColor = true;
            // 
            // ClearLogBtn
            // 
            this.ClearLogBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearLogBtn.Font = new System.Drawing.Font("Arial", 9F);
            this.ClearLogBtn.Location = new System.Drawing.Point(457, 142);
            this.ClearLogBtn.Name = "ClearLogBtn";
            this.ClearLogBtn.Size = new System.Drawing.Size(78, 23);
            this.ClearLogBtn.TabIndex = 1;
            this.ClearLogBtn.Text = "Clear Log";
            this.ClearLogBtn.UseVisualStyleBackColor = true;
            this.ClearLogBtn.Click += new System.EventHandler(this.ClearLogBtn_Click);
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 355);
            this.Controls.Add(this.PatchNoCorrupt);
            this.Controls.Add(this.PatchOnlySIzeGEQCBox);
            this.Controls.Add(this.LogTBox);
            this.Controls.Add(this.MatchOffsetLbl);
            this.Controls.Add(this.DurationLbl);
            this.Controls.Add(this.DetailLimitLbl);
            this.Controls.Add(this.PaddingLbl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PatchBytesLbl);
            this.Controls.Add(this.MatchLengthNum);
            this.Controls.Add(this.DetailLimitNum);
            this.Controls.Add(this.PatchPaddingNum);
            this.Controls.Add(this.OpenSecondaryBtn);
            this.Controls.Add(this.ComputeBtn);
            this.Controls.Add(this.OutputBtn);
            this.Controls.Add(this.OpenMainBtn);
            this.Controls.Add(this.SecondTBox);
            this.Controls.Add(this.ClearLogBtn);
            this.Controls.Add(this.MainScanBtn);
            this.Controls.Add(this.DurationTBox);
            this.Controls.Add(this.MainTBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainUI";
            this.Text = "TSFix";
            ((System.ComponentModel.ISupportInitialize)(this.PatchPaddingNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatchLengthNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetailLimitNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MainTBox;
        private System.Windows.Forms.Button MainScanBtn;
        private System.Windows.Forms.Button OpenMainBtn;
        private System.Windows.Forms.TextBox SecondTBox;
        private System.Windows.Forms.Button OpenSecondaryBtn;
        private System.Windows.Forms.Button ComputeBtn;
        private System.Windows.Forms.NumericUpDown PatchPaddingNum;
        private System.Windows.Forms.Label PaddingLbl;
        private System.Windows.Forms.Label PatchBytesLbl;
        private System.Windows.Forms.Button OutputBtn;
        private System.Windows.Forms.RichTextBox LogTBox;
        private System.Windows.Forms.NumericUpDown MatchLengthNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label MatchOffsetLbl;
        private System.Windows.Forms.TextBox DurationTBox;
        private System.Windows.Forms.Label DurationLbl;
        private System.Windows.Forms.NumericUpDown DetailLimitNum;
        private System.Windows.Forms.Label DetailLimitLbl;
        private System.Windows.Forms.CheckBox PatchOnlySIzeGEQCBox;
        private System.Windows.Forms.CheckBox PatchNoCorrupt;
        private System.Windows.Forms.Button ClearLogBtn;
    }
}

