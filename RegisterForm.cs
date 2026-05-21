using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace safetyapp
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button was clicked!");
            // Inside the button click:
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=safetyDB;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "INSERT INTO Users (FullName, Username, Password, Phone) VALUES (@name, @user, @pass, @phone)";
                SqlCommand cmd = new SqlCommand(query, con);

                // Linking the textboxes to the SQL command
                cmd.Parameters.AddWithValue("@name", txtFullName.Text);
                cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registration Successful! You can now log in.");
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Hide();
        }

        private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
