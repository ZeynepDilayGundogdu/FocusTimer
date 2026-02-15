using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace Productivity
{
    public partial class Form1 : Form
    {
        private int totalSeconds = 0;
        private bool isRunning = false;
        private Timer timerPomodoro;
        bool menuOpen = true;
        Timer menuTimer = new Timer();
        int menuMaxWidth = 156;
        private int pomodoroCount = 0;
        private enum TimerMode { Pomodoro, ShortBreak, LongBreak }
        private TimerMode currentMode = TimerMode.Pomodoro;




        public Form1()
        {
            InitializeComponent();

            timerPomodoro = new Timer();
            timerPomodoro.Interval = 1000;
            timerPomodoro.Tick += TimerPomodoro_Tick;

            EnsureSettingsTable();
            EnsureUserColumns();

            LoadPomodoroFromSettings();
            UpdateCompletedToday();
            menuTimer.Interval = 10;
            menuTimer.Tick += MenuTimer_Tick;

           




        }

        private string GetConnectionString()
        {
            return @"Data Source=" + Application.StartupPath +
                   @"\ProductivityDatabase_new.db;Version=3;";
            
        }

        private void LoadPomodoroFromSettings()
        {
            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                var cmd = new SQLiteCommand(
                    "SELECT PomodoroDuration FROM Settings WHERE UserId=@uid LIMIT 1",
                    conn);

                cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                var result = cmd.ExecuteScalar();
                totalSeconds = result != null ? Convert.ToInt32(result) * 60 : 25 * 60;
            }

            lblTimer.Text = FormatTime(totalSeconds);

            progressBar1.Maximum = totalSeconds;
            progressBar1.Value = 0;
        }

        private void nightButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void EnsureSettingsTable()
        {
            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                string sql = @"
        CREATE TABLE IF NOT EXISTS Settings (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER,
            PomodoroDuration INTEGER,
            ShortBreak INTEGER,
            LongBreak INTEGER,
            AutoStart INTEGER
        );";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                string insertDefault = @"
INSERT INTO Settings (UserId, PomodoroDuration, ShortBreak, LongBreak, AutoStart)
SELECT @uid, 25, 5, 15, 0
WHERE NOT EXISTS(SELECT 1 FROM Settings WHERE UserId=@uid);
";

                using (var cmd = new SQLiteCommand(insertDefault, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void TimerPomodoro_Tick(object sender, EventArgs e)
        {
            if (totalSeconds > 0)
            {
                totalSeconds--;
                lblTimer.Text = FormatTime(totalSeconds);

                if (progressBar1.Value < progressBar1.Maximum)
                    progressBar1.Value++;
                return;
            }

            timerPomodoro.Stop();
            isRunning = false;

            var s = GetSettings();

            if (currentMode == TimerMode.Pomodoro)
            {
                SavePomodoroCompletion();
                UpdateCompletedToday();
                pomodoroCount++;

                if (pomodoroCount % 4 == 0)
                {
                    currentMode = TimerMode.LongBreak;
                    totalSeconds = s.longBreak * 60;
                    MessageBox.Show("Long Break zamanı 💤");
                }
                else
                {
                    currentMode = TimerMode.ShortBreak;
                    totalSeconds = s.shortBreak * 60;
                    MessageBox.Show("Short Break zamanı ☕");
                }
            }
            else
            {
                currentMode = TimerMode.Pomodoro;
                totalSeconds = s.pomodoro * 60;
                MessageBox.Show("Yeni Pomodoro 🔥");
            }

            lblTimer.Text = FormatTime(totalSeconds);
            progressBar1.Value = 0;
            progressBar1.Maximum = totalSeconds;

            if (s.autoStart == 1)
            {
                timerPomodoro.Start();
                isRunning = true;
            }
        }
        private (int pomodoro, int shortBreak, int longBreak, int autoStart) GetSettings()
        {
            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                var cmd = new SQLiteCommand(
                    "SELECT PomodoroDuration, ShortBreak, LongBreak, AutoStart FROM Settings WHERE UserId=@uid",
                    conn);

                cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        return (
                            Convert.ToInt32(r["PomodoroDuration"]),
                            Convert.ToInt32(r["ShortBreak"]),
                            Convert.ToInt32(r["LongBreak"]),
                            Convert.ToInt32(r["AutoStart"])
                        );
                    }
                }
            }

            return (25, 5, 15, 0);
        }

        private void EnsureUserColumns()
        {
            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                string[] sqls =
                {
            "ALTER TABLE PomodoroHistory ADD COLUMN UserId INTEGER;",
            "ALTER TABLE Tasks ADD COLUMN UserId INTEGER;"
        };

                foreach (string sql in sqls)
                {
                    try
                    {
                        using (var cmd = new SQLiteCommand(sql, conn))
                            cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        // zaten varsa sessiz geçsin
                    }
                }
            }
        }
        private void SavePomodoroCompletion()
        {
            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                var cmd = new SQLiteCommand(
                    "INSERT INTO PomodoroHistory (UserId, TaskId, DateCompleted) VALUES (@uid, 0, DATE('now','localtime'))",
                    conn);

                cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateCompletedToday()
        {
            int count = 0;

            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                string q = @"
            SELECT COUNT(*) 
            FROM PomodoroHistory 
            WHERE UserId=@uid
              AND DateCompleted = DATE('now','localtime')";

                var cmd = new SQLiteCommand(q, conn);
                cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                var result = cmd.ExecuteScalar();
                count = result != null ? Convert.ToInt32(result) : 0;
            }

            lblCompleted.Text = "Completed Today: " + count;
        }

        private string FormatTime(int seconds)
        {
            int m = seconds / 60;
            int s = seconds % 60;
            return $"{m:D2}:{s:D2}";
        }

        // -------- NAVİGASYON --------

        private void btnTasks_Click(object sender, EventArgs e)
        {
            new TasksForm().ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            new StatisticsForm().ShowDialog();
        }

        private void btnTasks_Click_1(object sender, EventArgs e)
        {
            new TasksForm().ShowDialog();
        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            new StatisticsForm().ShowDialog();
        }

        private void btnSettings_Click_1(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        // -------- POMODORO --------

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isRunning) return;

            timerPomodoro.Start();
            isRunning = true;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                timerPomodoro.Stop();
                isRunning = false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            timerPomodoro.Stop();
            isRunning = false;

            LoadPomodoroFromSettings();
            progressBar1.Value = 0;
        }

        public void StopAndReloadTimer()
        {
            timerPomodoro.Stop();
            isRunning = false;
            currentMode = TimerMode.Pomodoro;


            int duration = 25;
            int autoStart = 0;

            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                var cmd = new SQLiteCommand(
                    "SELECT PomodoroDuration, AutoStart FROM Settings WHERE UserId=@uid LIMIT 1",
                    conn);

                cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        duration = Convert.ToInt32(r["PomodoroDuration"]);
                        autoStart = Convert.ToInt32(r["AutoStart"]);
                    }
                }
            }

            // 🔴 EN KRİTİK 3 SATIR
            totalSeconds = duration * 60;
            lblTimer.Text = FormatTime(totalSeconds);

            progressBar1.Value = 0;
            progressBar1.Maximum = totalSeconds;

            if (autoStart == 1)
            {
                timerPomodoro.Start();
                isRunning = true;
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void lblTimer_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            StyleButton(btnStart, Color.White, Color.FromArgb(220, 255, 255, 255));
            StyleButton(btnPause, Color.White, Color.FromArgb(220, 255, 255, 255));
            StyleButton(btnReset, Color.White, Color.FromArgb(220, 255, 255, 255));

            StyleButton(btnTasks, Color.FromArgb(230, 255, 255, 255), Color.White);
            StyleButton(btnStats, Color.FromArgb(230, 255, 255, 255), Color.White);
            StyleButton(btnSettings, Color.FromArgb(230, 255, 255, 255), Color.White);
        }

        

        private void btnMenu_Click(object sender, EventArgs e)
        {
          
            menuTimer.Start();
        }
        private void MenuTimer_Tick(object sender, EventArgs e)
        {
            if (menuOpen)
            {
                panel1.Width -= 20;
                if (panel1.Width <= 0)
                {
                    panel1.Width = 0;
                    menuOpen = false;
                    menuTimer.Stop();
                }
            }
            else
            {
                panel1.Width += 156;
                if (panel1.Width >= menuMaxWidth)
                {
                    panel1.Width = menuMaxWidth;
                    menuOpen = true;
                    menuTimer.Stop();
                }
            }
        }


        private void StyleButton(Button btn, Color normal, Color hover)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = normal;
            btn.ForeColor = Color.Black;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Height = 36;

            // Yuvarlak köşe
            btn.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 20, 20));

            // Hover efekti
            btn.MouseEnter += (s, e) => btn.BackColor = hover;
            btn.MouseLeave += (s, e) => btn.BackColor = normal;
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
    int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
    int nWidthEllipse, int nHeightEllipse);

        private void btnSettings_Click_2(object sender, EventArgs e)
        {
            var set = new SettingsForm();
            set.ShowDialog();
        }
    }
}

    
