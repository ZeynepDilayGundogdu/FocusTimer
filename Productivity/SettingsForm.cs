using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Productivity
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.Load += SettingsForm_Load;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            EnsureSettingsTable();
            LoadSettings();
        }

        private string GetConnectionString()
        {
            return @"Data Source=" + Application.StartupPath +
                   @"\ProductivityDatabase_new.db;Version=3;";
        }

        private void EnsureSettingsTable()
        {
            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                // 1) Tablo yoksa oluştur
                string createTable = @"
CREATE TABLE IF NOT EXISTS Settings (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER,
    PomodoroDuration INTEGER,
    ShortBreak INTEGER,
    LongBreak INTEGER,
    AutoStart INTEGER
);";
                using (var cmd = new SQLiteCommand(createTable, conn))
                    cmd.ExecuteNonQuery();

                // 2) Eski tablon UNIQUE değilse: UserId için UNIQUE INDEX oluştur (ON CONFLICT bununla çalışır)
                // Not: Eğer DB'de aynı UserId'den birden fazla satır varsa index oluştururken hata verir.
                // O yüzden önce duplicates temizliyoruz.
                string deleteDuplicates = @"
DELETE FROM Settings
WHERE rowid NOT IN (
    SELECT MIN(rowid)
    FROM Settings
    GROUP BY UserId
);";
                using (var cmd = new SQLiteCommand(deleteDuplicates, conn))
                    cmd.ExecuteNonQuery();

                string createUniqueIndex = @"
CREATE UNIQUE INDEX IF NOT EXISTS UX_Settings_UserId
ON Settings(UserId);";
                using (var cmd = new SQLiteCommand(createUniqueIndex, conn))
                    cmd.ExecuteNonQuery();
            }
        }

        private void LoadSettings()
        {
            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                string q = @"SELECT PomodoroDuration, ShortBreak, LongBreak, AutoStart
                             FROM Settings WHERE UserId=@uid";

                using (var cmd = new SQLiteCommand(q, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            numPomodoro.Value = Convert.ToInt32(r["PomodoroDuration"]);
                            numShortBreak.Value = Convert.ToInt32(r["ShortBreak"]);
                            numericUpDown2.Value = Convert.ToInt32(r["LongBreak"]);
                            chkAutoStart.Checked = Convert.ToInt32(r["AutoStart"]) == 1;
                        }
                    }
                }
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                string q = @"
INSERT INTO Settings (UserId, PomodoroDuration, ShortBreak, LongBreak, AutoStart)
VALUES (@uid, @pd, @sb, @lb, @auto)
ON CONFLICT(UserId) DO UPDATE SET
    PomodoroDuration=@pd,
    ShortBreak=@sb,
    LongBreak=@lb,
    AutoStart=@auto;
";

                using (var cmd = new SQLiteCommand(q, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);
                    cmd.Parameters.AddWithValue("@pd", (int)numPomodoro.Value);
                    cmd.Parameters.AddWithValue("@sb", (int)numShortBreak.Value);
                    cmd.Parameters.AddWithValue("@lb", (int)numericUpDown2.Value);
                    cmd.Parameters.AddWithValue("@auto", chkAutoStart.Checked ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }

            // 🔥 Form1'e HABER VERİYORUZ
            Form1 main = Application.OpenForms["Form1"] as Form1;
            main?.StopAndReloadTimer();

            MessageBox.Show("Ayarlar kaydedildi ✔");
            this.Close();
        }

        private void btnSaveSettings_Click_1(object sender, EventArgs e) { }
        private void btnCancelSettings_Click(object sender, EventArgs e) { }
        private void btnCancelSettings_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
