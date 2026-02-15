using System;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Productivity
{
    public partial class LoginForm : Form
    {
        public static int CurrentUserId;
        public static string CurrentUsername;
        public static string CurrentUserDB = "ProductivityDatabase_new.db";

        private readonly string usernamePlaceholder = "Username";
        private readonly string passwordPlaceholder = "Password";
        private Timer fadeTimer;

        public LoginForm()
        {
            InitializeComponent();
            this.Load += LoginForm_Load;

            this.Opacity = 0;

            fadeTimer = new Timer();
            fadeTimer.Interval = 10;
            fadeTimer.Tick += FadeInEffect;
        }

        private string GetConnectionString()
        {
            return @"Data Source=" + Application.StartupPath +
                   @"\ProductivityDatabase_new.db;Version=3;";
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            fadeTimer.Start();

            EnsureUsersTable();

            // 🔹 Placeholder'lar ÖNCE
            textBox1.Text = usernamePlaceholder;
            textBox1.ForeColor = Color.Gray;

            maskedTextBox1.Text = passwordPlaceholder;
            maskedTextBox1.ForeColor = Color.Gray;
            maskedTextBox1.UseSystemPasswordChar = false;

            // 🔥 Hatırlanan kullanıcıyı EN SON yükle
            LoadRememberedUser();

            this.BeginInvoke(new Action(() =>
            {
                MakeRounded(textBox1, 12);
                MakeRounded(maskedTextBox1, 12);
                MakeRounded(button1, 18);
                MakeRounded(button2, 18);
                MakeRounded(btnTogglePassword, 10);
            }));
        }
        private void EnsureUsersTable()
        {
            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();
                string sql = @"
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT,
    Password TEXT
);";
                using (var cmd = new SQLiteCommand(sql, conn))
                    cmd.ExecuteNonQuery();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == usernamePlaceholder ||
                maskedTextBox1.Text == passwordPlaceholder)
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz!");
                return;
            }

            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                string query = "SELECT Id FROM Users WHERE Username=@u AND Password=@p LIMIT 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", maskedTextBox1.Text.Trim());

                    var result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Kullanıcı bulunamadı!");
                        return;
                    }

                    CurrentUserId = Convert.ToInt32(result);
                }

                // 🔐 SETTINGS SATIRI YOKSA OLUŞTUR
                string ensureSettings = @"
INSERT INTO Settings (UserId, PomodoroDuration, ShortBreak, LongBreak, AutoStart, RememberUsername, RememberMe)
SELECT @uid, 25, 5, 15, 0, '', 0
WHERE NOT EXISTS (SELECT 1 FROM Settings WHERE UserId=@uid)";
                using (var cmd = new SQLiteCommand(ensureSettings, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", CurrentUserId);
                    cmd.ExecuteNonQuery();
                }

                // 💾 REMEMBER ME KAYDET
                string update = @"
UPDATE Settings 
SET RememberUsername=@u, RememberMe=@r 
WHERE UserId=@uid";
                using (var cmd = new SQLiteCommand(update, conn))
                {
                    cmd.Parameters.AddWithValue("@u", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@r", checkBox1.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@uid", CurrentUserId);
                    cmd.ExecuteNonQuery();
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == usernamePlaceholder)
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = usernamePlaceholder;
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void maskedTextBox1_Enter(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text == passwordPlaceholder)
            {
                maskedTextBox1.Text = "";
                maskedTextBox1.ForeColor = Color.Black;
                maskedTextBox1.UseSystemPasswordChar = true;
            }
        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(maskedTextBox1.Text))
            {
                maskedTextBox1.UseSystemPasswordChar = false;
                maskedTextBox1.Text = passwordPlaceholder;
                maskedTextBox1.ForeColor = Color.Gray;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();
                    string insertSql = "INSERT INTO Users (Username, Password) VALUES (@u, @p)";
                    using (var cmd = new SQLiteCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@p", maskedTextBox1.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Kayıt tamam!");
            }
            catch (SQLiteException ex)
            {
                // 👇 Kullanıcı adı zaten varsa
                if (ex.Message.Contains("UNIQUE"))
                {
                    MessageBox.Show("Bu kullanıcı adı zaten mevcut ❌");
                }
                else
                {
                    MessageBox.Show("Kayıt sırasında bir hata oluştu.");
                }
            }
        }


        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.UseSystemPasswordChar)
            {
                maskedTextBox1.UseSystemPasswordChar = false;
                btnTogglePassword.Text = "🙈";
            }
            else
            {
                maskedTextBox1.UseSystemPasswordChar = true;
                btnTogglePassword.Text = "👁";
            }
        }

        private void FadeInEffect(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
                this.Opacity += 0.15;
            else
                fadeTimer.Stop();
        }

        private void MakeRounded(Control c, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(c.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(c.Width - radius, c.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, c.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            c.Region = new Region(path);
        }
        private void LoadRememberedUser()
        {
            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                string q = @"
SELECT RememberUsername, RememberMe
FROM Settings
WHERE RememberMe = 1
ORDER BY Id DESC
LIMIT 1";

                using (var cmd = new SQLiteCommand(q, conn))
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        textBox1.Text = r["RememberUsername"].ToString();
                        textBox1.ForeColor = Color.Black;
                        checkBox1.Checked = true;
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void LoginForm_Load_1(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
    }
}
