
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace safetyapp
{
    public partial class ManageContacts : Form
    {
        // Connection string stays at the top class level
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SafetyDB;Integrated Security=True";
        public ManageContacts()
        {
            InitializeComponent();
        }

        // Action for the BACK button
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // Closes this window and returns to Dashboard
        }

        // Action for the SAVE button
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Check if the textboxes have data
            if (string.IsNullOrWhiteSpace(txtContactName.Text) || string.IsNullOrWhiteSpace(txtContactPhone.Text))
            {
                MessageBox.Show("Please fill in both the Name and Phone Number.");
                return;
            }

            // 2. Prevent the short-number crash
            if (txtContactPhone.Text.Length < 10)
            {
                MessageBox.Show("Please enter a valid 10-digit phone number.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string query = "INSERT INTO EmergencyContacts (UserId, ContactName, ContactPhone) VALUES (@id, @name, @phone)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@id", 1); // Fixed user ID for now
                    cmd.Parameters.AddWithValue("@name", txtContactName.Text);
                    cmd.Parameters.AddWithValue("@phone", txtContactPhone.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Contact Saved Successfully!");

                    // Clear fields and refresh the view
                    txtContactName.Clear();
                    txtContactPhone.Clear();
                    LoadContacts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        // Logic to refresh the list of contacts
        private void LoadContacts()
       
      
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                // Change 'Id' to 'ContactId'
                string query = "SELECT ContactId, ContactName, ContactPhone FROM EmergencyContacts";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                // Hide it so the user only sees names and numbers
                if (dataGridView1.Columns.Contains("ContactId"))
                {
                    dataGridView1.Columns["ContactId"].Visible = false;
                }
            }
        }
            

        private void ManageContacts_Load(object sender, EventArgs e)
        {
            LoadContacts();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        
        
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Option A: Use the exact name "ContactId"
                string contactId = dataGridView1.SelectedRows[0].Cells["ContactId"].Value.ToString();

                // OR Option B: Use index [0] which is safer
                // string contactId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    // IMPORTANT: Change 'Id' to 'ContactId' here too!
                    string query = "DELETE FROM EmergencyContacts WHERE ContactId = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", contactId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Contact Deleted!");
                    LoadContacts();
                }
            }
        }
           


        private void dgvContacts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ManageContacts_Load_1(object sender, EventArgs e)
        {

        }

        private void txtContactPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void phonenumber_Click(object sender, EventArgs e)
        {

        }
    }
    }
