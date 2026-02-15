using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Productivity
{
    public partial class StatisticsForm : Form
    {
        public StatisticsForm()
        {
            InitializeComponent();
            this.Load += StatisticsForm_Load;
        }

        private void StatisticsForm_Load(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            string connectionString = @"Data Source=" + Application.StartupPath +
                                      @"\" + LoginForm.CurrentUserDB + ";Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // === DAILY ===
                string dailyQuery = @"
SELECT DateCompleted, COUNT(*) as CompletedCount
FROM PomodoroHistory
WHERE UserId=@uid
GROUP BY DateCompleted
ORDER BY DateCompleted ASC";

                DataTable dtDaily = new DataTable();
                using (var cmd = new SQLiteCommand(dailyQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        dtDaily.Load(reader);
                    }
                }
                SetupChart(chartDaily, dtDaily, "Pomodoros", "DateCompleted", "CompletedCount");

                // === WEEKLY ===
                int[] weeklyCounts = new int[7]; // 0=Sun ... 6=Sat

                string weeklyQuery = @"
SELECT 
    strftime('%w', DateCompleted) AS WeekDay,
    COUNT(*) AS CompletedCount
FROM PomodoroHistory
WHERE UserId=@uid
  AND DateCompleted >= DATE('now','localtime','-6 days')
GROUP BY WeekDay";

                using (var cmd = new SQLiteCommand(weeklyQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int day = Convert.ToInt32(reader["WeekDay"]); // 0..6
                            weeklyCounts[day] = Convert.ToInt32(reader["CompletedCount"]);
                        }
                    }
                }

                chartWeekly.Series.Clear();
                chartWeekly.ChartAreas.Clear();

                ChartArea areaWeekly = new ChartArea();
                chartWeekly.ChartAreas.Add(areaWeekly);

                Series weeklySeries = new Series("Pomodoros")
                {
                    ChartType = SeriesChartType.Column
                };

                string[] labels = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

                for (int i = 0; i < 7; i++)
                {
                    weeklySeries.Points.AddXY(labels[i], weeklyCounts[i]);
                }

                chartWeekly.Series.Add(weeklySeries);

                // ✅ TASK CATEGORY DISTRIBUTION (BURASI using(conn) İÇİNDE!)
                string taskCategoryQuery = @"
SELECT Category, COUNT(*) AS TaskCount
FROM Tasks
WHERE UserId=@uid
  AND Category IS NOT NULL
  AND TRIM(Category) <> ''
GROUP BY Category";

                DataTable dtTaskCategory = new DataTable();

                using (var cmd = new SQLiteCommand(taskCategoryQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", LoginForm.CurrentUserId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        dtTaskCategory.Load(reader);
                    }
                }

                chartTaskDistribution.Series.Clear();
                chartTaskDistribution.ChartAreas.Clear();

                ChartArea taskArea = new ChartArea();
                chartTaskDistribution.ChartAreas.Add(taskArea);

                Series taskSeries = new Series("Tasks")
                {
                    ChartType = SeriesChartType.Pie
                };

                chartTaskDistribution.Series.Add(taskSeries);
                chartTaskDistribution.DataSource = dtTaskCategory;
                taskSeries.XValueMember = "Category";
                taskSeries.YValueMembers = "TaskCount";
                chartTaskDistribution.DataBind();
            }
        }



        private void SetupChart(Chart chart, DataTable dt, string seriesName, string xMember, string yMember, SeriesChartType chartType = SeriesChartType.Column)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            ChartArea area = new ChartArea();
            chart.ChartAreas.Add(area);

            Series series = new Series
            {
                Name = seriesName,
                ChartType = chartType,
                XValueMember = xMember,
                YValueMembers = yMember
            };

            chart.Series.Add(series);
            chart.DataSource = dt;
            chart.DataBind();
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chartTaskDistribution_Click(object sender, EventArgs e)
        {
        }

        private void chartDaily_Click(object sender, EventArgs e)
        {
        }
    }
}
