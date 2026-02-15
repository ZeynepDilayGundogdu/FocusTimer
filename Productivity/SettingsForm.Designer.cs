namespace Productivity
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.numPomodoro = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numLongBreak = new System.Windows.Forms.Label();
            this.numShortBreak = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPomodoro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShortBreak)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pomodoro Duration (minutes):";
            // 
            // numPomodoro
            // 
            this.numPomodoro.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numPomodoro.Location = new System.Drawing.Point(211, 39);
            this.numPomodoro.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numPomodoro.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPomodoro.Name = "numPomodoro";
            this.numPomodoro.Size = new System.Drawing.Size(120, 22);
            this.numPomodoro.TabIndex = 1;
            this.numPomodoro.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Short Break (minutes):";
            // 
            // numLongBreak
            // 
            this.numLongBreak.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numLongBreak.AutoSize = true;
            this.numLongBreak.Location = new System.Drawing.Point(18, 116);
            this.numLongBreak.Name = "numLongBreak";
            this.numLongBreak.Size = new System.Drawing.Size(136, 16);
            this.numLongBreak.TabIndex = 3;
            this.numLongBreak.Text = "Long Break (minutes):";
            // 
            // numShortBreak
            // 
            this.numShortBreak.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numShortBreak.Location = new System.Drawing.Point(211, 71);
            this.numShortBreak.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numShortBreak.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numShortBreak.Name = "numShortBreak";
            this.numShortBreak.Size = new System.Drawing.Size(120, 22);
            this.numShortBreak.TabIndex = 4;
            this.numShortBreak.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDown2.Location = new System.Drawing.Point(211, 110);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 22);
            this.numericUpDown2.TabIndex = 5;
            this.numericUpDown2.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(53, 186);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(179, 20);
            this.chkAutoStart.TabIndex = 6;
            this.chkAutoStart.Text = "Auto-start next Pomodoro";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSaveSettings.Location = new System.Drawing.Point(68, 230);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSettings.TabIndex = 7;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnCancelSettings
            // 
            this.btnCancelSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancelSettings.Location = new System.Drawing.Point(225, 230);
            this.btnCancelSettings.Name = "btnCancelSettings";
            this.btnCancelSettings.Size = new System.Drawing.Size(75, 23);
            this.btnCancelSettings.TabIndex = 8;
            this.btnCancelSettings.Text = "Cancel";
            this.btnCancelSettings.UseVisualStyleBackColor = true;
            this.btnCancelSettings.Click += new System.EventHandler(this.btnCancelSettings_Click_1);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Productivity.Properties.Resources.coolbackgrounds_fractalize_clear_lagoon1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(382, 303);
            this.Controls.Add(this.btnCancelSettings);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.chkAutoStart);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numShortBreak);
            this.Controls.Add(this.numLongBreak);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numPomodoro);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numPomodoro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShortBreak)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numPomodoro;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label numLongBreak;
        private System.Windows.Forms.NumericUpDown numShortBreak;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnCancelSettings;
    }
}