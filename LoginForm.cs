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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace safetyapp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
       
        {
            // 1. Connection String - Points to your database
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=safetyDB;Integrated Security=True";

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    // 2. The Query - Checks if Username and Password match a row in the table
                    // Make sure these match your TextBox names (txtLoginUser and txtLoginPass)
                    string query = "SELECT COUNT(*) FROM Users WHERE Username=@user AND Password=@pass";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user", txtLoginUser.Text); // Change 'textBox1' if you renamed it
                    cmd.Parameters.AddWithValue("@pass", txtLoginPass.Text); // Change 'textBox2' if you renamed it

                    con.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();

                    // 3. The Logic - If count is 1, the user exists
                    if (count > 0)
                    {
                        MessageBox.Show("Login Successful!");

                        // Open the Dashboard
                        Dashboard dash = new Dashboard();
                        dash.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }
       

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 1. Create an instance of the Register Form
            RegisterForm register = new RegisterForm();

            // 2. Show the Register Form
            register.Show();

            // 3. Hide the current Login Form
            this.Hide();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void txtLoginUser_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
