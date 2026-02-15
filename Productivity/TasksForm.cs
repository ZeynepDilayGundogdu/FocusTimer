using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Productivity
{
    public partial class TasksForm : Form
    {
        DataTable tasksTable = new DataTable();
        public TasksForm()
        {
            InitializeComponent();
            this.Load += TasksForm_Load;

            this.btnAddTask.Click += new System.EventHandler(this.btnAddTask_Click);
            this.btnEditTask.Click += new System.EventHandler(this.btnEditTask_Click);
            this.btnDeleteTask.Click += new System.EventHandler(this.btnDeleteTask_Click);
        }

        private void TasksForm_Load(object sender, EventArgs e)
        {
            LoadTasks();
        }

        // ✔ TEK DB
        private string GetConnectionString()
        {
            return @"Data Source=" + Application.StartupPath +
                   @"\ProductivityDatabase_new.db;Version=3;";
        }


        private void dgvTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var idCell = dgvTasks.Rows[e.RowIndex].Cells["Id"];

                if (idCell != null && idCell.Value != null && idCell.Value != DBNull.Value)
                {
                    int taskId = Convert.ToInt32(idCell.Value);

                    AddEditTaskForm editForm = new AddEditTaskForm(taskId);
                    editForm.ShowDialog();

                    LoadTasks();
                }
                else
                {
                    MessageBox.Show("Seçili görevin ID bilgisi eksik veya geçersiz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadTasks()
        {
            using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                string query = @"SELECT Id, Title, Category, EstimatedPomodoros, Completed 
                         FROM Tasks 
                         WHERE UserId=@uid";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

                    tasksTable = new DataTable();
                    da.Fill(tasksTable);

                    dgvTasks.DataSource = tasksTable;
                }

                if (dgvTasks.Columns.Contains("Id"))
                    dgvTasks.Columns["Id"].Visible = false;

                if (dgvTasks.Columns.Contains("EstimatedPomodoros"))
                    dgvTasks.Columns["EstimatedPomodoros"].HeaderText = "Estimated";
            }
        }


        private void btnAddTask_Click(object sender, EventArgs e)
        {
            AddEditTaskForm addForm = new AddEditTaskForm();

            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadTasks();
                MessageBox.Show("Yeni görev başarıyla eklendi.", "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEditTask_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count > 0)
            {
                int rowIndex = dgvTasks.SelectedRows[0].Index;
                var idCell = dgvTasks.Rows[rowIndex].Cells["Id"];

                if (idCell != null && idCell.Value != null && idCell.Value != DBNull.Value)
                {
                    int taskId = Convert.ToInt32(idCell.Value);

                    AddEditTaskForm editForm = new AddEditTaskForm(taskId);

                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadTasks();
                        MessageBox.Show("Görev başarıyla güncellendi.", "Başarılı",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Seçili görevde düzenlenecek geçerli bir ID bulunamadı.",
                        "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek için bir görev seçin.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ✔ SADECE BU KISMı değiştirdim
        private void btnDeleteTask_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();
                    string query = @"SELECT Id, Title, Category, EstimatedPomodoros, Completed 
                                     FROM Tasks 
                                     WHERE UserId=@uid";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                        SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

                        tasksTable = new DataTable();
                        da.Fill(tasksTable);

                        dgvTasks.DataSource = tasksTable;
                    }
                }

                if (dgvTasks.Columns.Contains("Id"))
                    dgvTasks.Columns["Id"].Visible = false;

                if (dgvTasks.Columns.Contains("EstimatedPomodoros"))
                    dgvTasks.Columns["EstimatedPomodoros"].HeaderText = "Estimated";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Görevler yüklenirken bir hata oluştu: {ex.Message}", "Veritabanı Hatası");
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (tasksTable == null || tasksTable.Rows.Count == 0)
                return;

            string aranan = txtSearch.Text.Trim().Replace("'", "''");

            try
            {
                if (string.IsNullOrEmpty(aranan))
                {
                    tasksTable.DefaultView.RowFilter = "";
                }
                else
                {
                    tasksTable.DefaultView.RowFilter =
                        $"Title LIKE '%{aranan}%' OR Category LIKE '%{aranan}%'";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Filtreleme hatası: " + ex.Message);
            }
        }

        private void btnDeleteTask_Click_1(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count > 0)
            {
                int rowIndex = dgvTasks.SelectedRows[0].Index;
                var idCell = dgvTasks.Rows[rowIndex].Cells["Id"];
                var titleCell = dgvTasks.Rows[rowIndex].Cells["Title"];

                if (idCell == null || idCell.Value == null || idCell.Value == DBNull.Value)
                {
                    MessageBox.Show("Seçili görevde silme işlemi için geçerli bir ID bulunamadı.",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int taskId = Convert.ToInt32(idCell.Value);
                string taskTitle = (titleCell != null && titleCell.Value != null && titleCell.Value != DBNull.Value)
                    ? titleCell.Value.ToString() : "Seçili Görev";

                var confirmResult = MessageBox.Show(
                    $"'{taskTitle}' başlıklı görevi silmek istediğinizden emin misiniz?",
                    "Silme Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                        {
                            conn.Open();
                            string query = "DELETE FROM Tasks WHERE Id = @Id";
                            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@Id", taskId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        LoadTasks();
                        MessageBox.Show("Görev başarıyla silindi.", "Başarılı",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Görev silinirken bir veritabanı hatası oluştu: {ex.Message}",
                            "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir görev seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
