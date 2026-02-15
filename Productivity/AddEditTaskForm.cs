using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Productivity
{
    public partial class AddEditTaskForm : Form
    {
        private int editTaskId = 0;
        private bool isEdit = false;

        public AddEditTaskForm(int taskId = 0)
        {
            InitializeComponent();

            if (taskId != 0)
            {
                editTaskId = taskId;
                isEdit = true;
                LoadTaskData(taskId);
            }

            numEstimated.Minimum = 1;
        }

        private string GetConnectionString()
        {
            return @"Data Source=" + Application.StartupPath + @"\ProductivityDatabase_new.db;Version=3;";
        }

        private void LoadTaskData(int taskId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();
                string query = @"SELECT * FROM Tasks 
                                 WHERE Id=@id AND UserId=@uid";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtTitle.Text = reader["Title"].ToString();
                            cmbCategory.Text = reader["Category"].ToString();
                            numEstimated.Value = Convert.ToInt32(reader["EstimatedPomodoros"]);
                            txtDescription.Text = reader["Description"]?.ToString();
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a task title");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                string query = isEdit
                    ? @"UPDATE Tasks SET 
                            Title=@title, 
                            Category=@category, 
                            EstimatedPomodoros=@est,
                            Description=@desc
                        WHERE Id=@id AND UserId=@uid"
                    :  @"INSERT INTO Tasks 
        (Title, Category, EstimatedPomodoros, Description, Completed, UserId) 
        VALUES (@title, @category, @est, @desc, 0, @uid)";



                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@category", cmbCategory.Text.Trim());
                    cmd.Parameters.AddWithValue("@est", (int)numEstimated.Value);
                    cmd.Parameters.AddWithValue("@desc", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                    if (isEdit)
                        cmd.Parameters.AddWithValue("@id", editTaskId);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Saved");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
