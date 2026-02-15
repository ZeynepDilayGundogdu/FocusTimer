namespace Productivity
{
    partial class AddEditTaskForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.taskTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.Category = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.estimatedPomodoros = new System.Windows.Forms.Label();
            this.numEstimated = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numEstimated)).BeginInit();
            this.SuspendLayout();
            // 
            // taskTitle
            // 
            this.taskTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.taskTitle.AutoSize = true;
            this.taskTitle.Location = new System.Drawing.Point(-2, 25);
            this.taskTitle.Name = "taskTitle";
            this.taskTitle.Size = new System.Drawing.Size(67, 16);
            this.taskTitle.TabIndex = 0;
            this.taskTitle.Text = "Task Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtTitle.Location = new System.Drawing.Point(84, 12);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(236, 34);
            this.txtTitle.TabIndex = 1;
            // 
            // Category
            // 
            this.Category.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Category.AutoSize = true;
            this.Category.Location = new System.Drawing.Point(3, 66);
            this.Category.Name = "Category";
            this.Category.Size = new System.Drawing.Size(62, 16);
            this.Category.TabIndex = 2;
            this.Category.Text = "Category";
            // 
            // cmbCategory
            // 
            this.cmbCategory.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Items.AddRange(new object[] {
            "Study",
            "Work",
            "Exercise",
            "Coding",
            "Reading",
            "Self-Care"});
            this.cmbCategory.Location = new System.Drawing.Point(84, 66);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(123, 24);
            this.cmbCategory.TabIndex = 3;
            // 
            // estimatedPomodoros
            // 
            this.estimatedPomodoros.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.estimatedPomodoros.AutoSize = true;
            this.estimatedPomodoros.Location = new System.Drawing.Point(3, 106);
            this.estimatedPomodoros.Name = "estimatedPomodoros";
            this.estimatedPomodoros.Size = new System.Drawing.Size(144, 16);
            this.estimatedPomodoros.TabIndex = 4;
            this.estimatedPomodoros.Text = "Estimated Pomodoros:";
            // 
            // numEstimated
            // 
            this.numEstimated.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numEstimated.Location = new System.Drawing.Point(153, 104);
            this.numEstimated.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numEstimated.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEstimated.Name = "numEstimated";
            this.numEstimated.Size = new System.Drawing.Size(120, 22);
            this.numEstimated.TabIndex = 5;
            this.numEstimated.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtDescription.Location = new System.Drawing.Point(84, 150);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(300, 100);
            this.txtDescription.TabIndex = 7;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.Location = new System.Drawing.Point(72, 288);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.Location = new System.Drawing.Point(231, 288);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddEditTaskForm
            // 
            this.BackgroundImage = global::Productivity.Properties.Resources.coolbackgrounds_fractalize_persian_lounge;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(382, 353);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numEstimated);
            this.Controls.Add(this.estimatedPomodoros);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.Category);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.taskTitle);
            this.DoubleBuffered = true;
            this.Name = "AddEditTaskForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add/Edit Task";
            ((System.ComponentModel.ISupportInitialize)(this.numEstimated)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label taskTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label Category;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label estimatedPomodoros;
        private System.Windows.Forms.NumericUpDown numEstimated;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
