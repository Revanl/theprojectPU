using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Login
{
    public partial class Form2 : Form
    {
        SqlConnection con = new SqlConnection("Integrated Security=true;server=desktop-6rrc7gb;" +
                                               "trusted_connection=yes;" +
                                                "database=projectdb; " +
                                                 "connection timeout=30");
        SqlCommand cmd;

        int seconds;
        int minutes;
        int hours;
        Task currentTask = new Task();
        bool isPaused = true;
        DateTime startTime;
        DateTime endTime;
        DateTime pauseTime;
        DateTime resumeTime;
        double workingTime;
        double nonWorkingTime;

        string userIDvalue;

        public Form2(string userID)
        {
            InitializeComponent();
            seconds = minutes = hours = 0;
            userIDvalue = userID;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            loadHoursTable();
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds++;

            if(seconds > 59)
            {
                minutes++;
                seconds = 0;
            }

            if(minutes > 59)
            {
                hours++;
                minutes = 0;
            }

            xHours.Text = appendZero(hours);
            xMinutes.Text = appendZero(minutes);
            xSeconds.Text = appendZero(seconds);

        }

        private string appendZero(double str)
        {
            if (str <= 9)
                return "0" + str;
            else
                return str.ToString();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (taskName.Text != string.Empty) {

                startTime = DateTime.Now;
                currentTask = new Task(taskName.Text);
                timer1.Start();
                startButton.Enabled = false;
                pauseButton.Enabled = true;
                stopButton.Enabled = true;
            } else {

                MessageBox.Show("Please enter a task name");
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (isPaused) {

                timer1.Stop();
                pauseTime = DateTime.Now;
                pauseButton.Text = "Resume";
                stopButton.Enabled = false;
                isPaused = false;
            } else {

                resumeTime = DateTime.Now;
                pauseButton.Text = "Pause";
                stopButton.Enabled = true;
                timer1.Start();
                isPaused = true;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            endTime = DateTime.Now;
           
            TimeSpan breakHours = (resumeTime - pauseTime);
            var breakTime = breakHours.ToString(@"hh\:mm\:ss");

            TimeSpan hoursWorked = ((endTime - startTime) - (resumeTime - pauseTime));
            var workedTime = hoursWorked.ToString(@"hh\:mm\:ss");

            cmd = new SqlCommand("insert into Hours(workingHours, nonWorkingHours, task, employeeID) values(@workingHours, @nonWorkingHours, @task, @employeeID)", con);
            con.Open();
            cmd.Parameters.AddWithValue("@workingHours", workedTime);
            cmd.Parameters.AddWithValue("@nonWorkingHours", breakTime);
            cmd.Parameters.AddWithValue("@task", taskName.Text);
            cmd.Parameters.AddWithValue("@employeeID", userIDvalue);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Successfully added record");

            seconds = minutes = hours = 0;
            xHours.Text = appendZero(hours);
            xMinutes.Text = appendZero(minutes);
            xSeconds.Text = appendZero(seconds);
            taskName.Text = string.Empty;
            startButton.Enabled = true;
            pauseButton.Enabled = false;
            stopButton.Enabled = false;

            loadHoursTable();
        }

        private void loadHoursTable() {

            cmd = new SqlCommand("SELECT * FROM Hours", con);
            con.Open();
            // SqlDataReader reader = cmd.ExecuteReader();

            SqlDataAdapter data = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            data.Fill(table);
            dataGridView1.DataSource = new BindingSource(table, null);
            // dataGridView1.DataSource = dt;
            con.Close();
        }
    }
}
