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
    public partial class Form1 : Form
    {
       // SqlConnection con = new SqlConnection("Data Source=projectdb;Initial Catalog=Sample;Integrated Security=true;");
        SqlConnection con = new SqlConnection("Integrated Security=true;server=desktop-6rrc7gb;" +
                                               "trusted_connection=yes;" +
                                                "database=projectdb; " +
                                                 "connection timeout=30");
        SqlCommand cmd;

        public Form1()
        {
            InitializeComponent();
        }
        Login login = new Login("admin", "12345");

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string user = username.Text;
            string pass = password.Text;
            
            cmd = new SqlCommand("SELECT * FROM Employee WHERE Name = @name AND Password = @password", con);
            con.Open();
            cmd.Parameters.AddWithValue("@name", username.Text);
            cmd.Parameters.AddWithValue("@password", password.Text);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read()) {

                object userIDobj = reader["id"];
                string userID = userIDobj.ToString();
                con.Close();
                MessageBox.Show("You are logged in successfully");

                new Form2(userID).Show();
                this.Hide();
            } else {

                con.Close();
                MessageBox.Show("You entered a wrong username or password");
            }
        }
        

        private void registerButton_Click(object sender, EventArgs e)
        {
            if (username.Text != "" && password.Text != "") {
                cmd = new SqlCommand("SELECT * FROM Employee WHERE Name = @name OR Password = @password", con);
                con.Open();
                cmd.Parameters.AddWithValue("@name", username.Text);
                cmd.Parameters.AddWithValue("@password", password.Text);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) {

                    con.Close();
                    MessageBox.Show("This username or password has already been taken.");
                } else {

                    con.Close();
                    cmd = new SqlCommand("insert into Employee(Name,Password) values(@name,@password)", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@name", username.Text);
                    cmd.Parameters.AddWithValue("@password", password.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Successfully registered");
                }
            } else {

                MessageBox.Show("Please Provide Details!");
            }
        }
    }
}
