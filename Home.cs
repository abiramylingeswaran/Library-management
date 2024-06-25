using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library01
{
    public partial class Member : Form
    {
        // Connection string should ideally be stored in a configuration file.
        string Data = "Data Source=DESKTOP-KRP578P;Initial Catalog=Library;Integrated Security=True";

        public Member()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // This can be used for input validation or other logic as needed
        }

        private void label10_Click(object sender, EventArgs e)
        {
            // This can be used for label click event logic as needed
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate input before attempting to insert into the database
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(Data))
            {
                try
                {
                    connection.Open();

                    // Step 1: Check if the username already exists
                    string checkUserQuery = "SELECT COUNT(*) FROM Membership WHERE Username = @Username";
                    using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        checkUserCommand.Parameters.AddWithValue("@Username", textBox6.Text);
                        int userCount = (int)checkUserCommand.ExecuteScalar();

                        if (userCount > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.");
                        }
                        else
                        {
                            // Step 2: Insert the new user if the username does not exist
                            string insertQuery = "INSERT INTO Membership (Name, Fathername, Nicnumber, Occupation, Place, Birthday, Username, Password) VALUES (@Name, @Fathername, @Nicnumber, @Occupation," +
                                " @Place, @Birthday, @Username, @Password)";
                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Name", textBox1.Text);
                                insertCommand.Parameters.AddWithValue("@Fathername", textBox2.Text);
                                insertCommand.Parameters.AddWithValue("@Nicnumber", textBox3.Text);
                                insertCommand.Parameters.AddWithValue("@Occupation", textBox4.Text);
                                insertCommand.Parameters.AddWithValue("@Place", textBox5.Text);
                                insertCommand.Parameters.AddWithValue("@Birthday", dateTimePicker1.Value); // Changed to DateTimePicker.Value
                                insertCommand.Parameters.AddWithValue("@Username", textBox6.Text);
                                insertCommand.Parameters.AddWithValue("@Password", textBox7.Text);

                                int rowsAffected = insertCommand.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("User signed up successfully!");
                                }
                                else
                                {
                                    MessageBox.Show("Failed to sign up. Please try again.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            // Optionally, close the form after successful sign-up
            this.Close();
        }
    }
}
