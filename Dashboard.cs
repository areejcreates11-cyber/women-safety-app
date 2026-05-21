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
using System.Device.Location;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace safetyapp
{
    public partial class Dashboard : Form
    {
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SafetyDB;Integrated Security=True";

        // 1. FIXED HERE: Globally declare the watcher variable inside the class context
        GeoCoordinateWatcher watcher;

        public Dashboard()
        {
            InitializeComponent();

            // 2. FIXED HERE: Instantiate and fire up the background location listener 
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.Start();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSOS_Click(object sender, EventArgs e)
        {
            // The compiler now successfully recognizes the global 'watcher' field!
            GeoCoordinate coord = watcher.Position.Location;
            string locationLink = "Location Unavailable";

            if (!coord.IsUnknown)
            {
                locationLink = $"https://www.google.com/maps?q={coord.Latitude},{coord.Longitude}";
            }

            string finalMessage = $"EMERGENCY ALERT! I need help. My current tracking link is: {locationLink}";

            // 3. NOTE: Remember to swap these placeholders out with your true online console credentials later!
            string accountSid = "AC19d1dbc92bd1ca743102f5854b31193a";
            string authToken = "902757f70a13455732d41a45f95bbb42";
            string twilioSenderNumber = "+19788012645"; // Format example: "+1XXXXXXXXXX"

            try
            {
                TwilioClient.Init(accountSid, authToken);

                // Connect to your local SQL database to fetch the emergency contacts
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string query = "SELECT ContactPhone FROM EmergencyContacts";
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int messagesSentCount = 0;

                    // Loop through every contact phone number in your database grid
                    while (reader.Read())
                    {
                        string destinationPhone = reader["ContactPhone"].ToString().Trim();

                        // Format number if it starts with a local '0' (e.g., 0322... becomes +92322...)
                        if (destinationPhone.StartsWith("0"))
                        {
                            destinationPhone = "+92" + destinationPhone.Substring(1);
                        }

                        // Fire the live network SMS request
                        var message = MessageResource.Create(
                            body: finalMessage,
                            from: new PhoneNumber(twilioSenderNumber),
                            to: new PhoneNumber(destinationPhone)
                        );

                        messagesSentCount++;
                    }
                    reader.Close();

                    if (messagesSentCount > 0)
                    {
                        MessageBox.Show($"SOS Broadcast sent immediately to {messagesSentCount} emergency contacts!",
                                        "Alert Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No active contacts found in your database to text.", "Alert Status");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SMS Gateway Dispatch Error: {ex.Message}", "System Failure",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnManageContacts_Click(object sender, EventArgs e)
        {
            ManageContacts contacts = new ManageContacts();
            contacts.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            try
            {
                this.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing home: " + ex.Message);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}