using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login
{
    public partial class Form2 : Form
    {
        int seconds;
        int minutes;
        int hours;
        Task currentTask = new Task();
        bool isPaused = true;
        DateTime pauseTime;
        DateTime resumeTime;

        public Form2()
        {
            InitializeComponent();
            seconds = minutes = hours = 0;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

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

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != string.Empty)
            {
                currentTask = new Task(textBox1.Text);
                timer1.Start();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            currentTask.EndTime = DateTime.Now;
            currentTask.WorkingTime = currentTask.EndTime.Subtract(currentTask.StartTime);
            currentTask.WorkingTime -= currentTask.NonWorkingTime;
            seconds = minutes = hours = 0;
            xHours.Text = appendZero(hours);
            xMinutes.Text = appendZero(minutes);
            xSeconds.Text = appendZero(seconds);
            textBox1.Text = string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isPaused)
            {
                timer1.Stop();
                pauseTime = DateTime.Now;
                button2.Text = "Resume";
                isPaused = false;
            }
            else
            {
                resumeTime = DateTime.Now;
                button2.Text = "Pause";
                timer1.Start();
                currentTask.NonWorkingTime += resumeTime.Subtract(pauseTime);
                isPaused = true;
            }
        }

        private void newTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fr2 = new Form2();
            fr2.Show();
        }

    }
}
