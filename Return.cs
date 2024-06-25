using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library01
{
    public partial class Return : Form
    {
        public DateTimePicker dateTimePicker2;


        public Return()
        {
            InitializeComponent();
            dateTimePicker2 = new DateTimePicker(); 
            this.Controls.Add(dateTimePicker2);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string connectionString = "Data Source=DESKTOP-KRP578P;Initial Catalog=Library;Integrated Security=True";

            string bookId = textBox1.Text;
            string userName = textBox2.Text;
            DateTime returnDate = dateTimePicker1.Value;

            using (DbConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Validate the username in Addnew table
                    string validateUserQuery = "SELECT COUNT(*) FROM Membership WHERE UserName = @userName";
                    using (DbCommand validateUserCommand = connection.CreateCommand())
                    {
                        validateUserCommand.CommandText = validateUserQuery;
                        validateUserCommand.Parameters.Add(CreateParameter(validateUserCommand, "@userName", userName));
                        int userCount = (int)validateUserCommand.ExecuteScalar();

                        if (userCount == 0)
                        {
                            MessageBox.Show("Username does not exist in the Membership table.");
                            return;
                        }
                    }

                    // Validate the borrow record and fetch the borrow date
                    string fetchBorrowDateQuery = "SELECT * FROM Borrow WHERE BookId = @bookId AND UserName = @userName";
                    using (DbCommand fetchBorrowDateCommand = connection.CreateCommand())
                    {
                        fetchBorrowDateCommand.CommandText = fetchBorrowDateQuery;
                        fetchBorrowDateCommand.Parameters.Add(CreateParameter(fetchBorrowDateCommand, "@bookId", bookId));
                        fetchBorrowDateCommand.Parameters.Add(CreateParameter(fetchBorrowDateCommand, "@userName", userName));

                        using (DbDataReader reader = fetchBorrowDateCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Convert the borrow date from string to DateTime
                                DateTime borrowDate;
                                if (DateTime.TryParse(reader["DateandTime"].ToString(), out borrowDate))
                                {                    
                                    // Calculate the difference in days between borrow date and return date
                                    TimeSpan difference = returnDate - borrowDate;
                                    int daysDifference = difference.Days;


                                    // Display the difference in days in a label
                                    if (daysDifference > 7)
                                     {
                                        int lateDays = daysDifference - 7;
                                        float fee = lateDays * 5;
                                        label4.Text = $"Fine fee for {lateDays} days late: ${fee}";
                                     }
                                    else
                                     {
                                        label4.Text = "No fine incurred.";
                                     }
                                }
                                else
                                {
                                    MessageBox.Show("Invalid borrow date format.");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("The book ID and username combination does not exist in the Borrow table.");
                                return;
                            }
                        }
                    }


                    string returnQuery = "INSERT INTO Return1 (BookId, UserName, Date) VALUES (@bookId, @userName, @date)";
                    using (DbCommand returnCommand = connection.CreateCommand())
                    {
                        returnCommand.CommandText = returnQuery;
                        returnCommand.Parameters.Add(CreateParameter(returnCommand, "@bookId", bookId));
                        returnCommand.Parameters.Add(CreateParameter(returnCommand, "@userName", userName));
                        returnCommand.Parameters.Add(CreateParameter(returnCommand, "@date", returnDate)); // Get return date from DateTimePicker2

                        int rowsAffected = returnCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Return successful!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to return. Please try again.");
                            return;
                        }
                    }

                    // Delete from Borrow table
                    string deleteQuery = "DELETE FROM Borrow WHERE BookId = @bookId AND UserName = @userName";
                    using (DbCommand deleteCommand = connection.CreateCommand())
                    {
                        deleteCommand.CommandText = deleteQuery;
                        deleteCommand.Parameters.Add(CreateParameter(deleteCommand, "@bookId", bookId));
                        deleteCommand.Parameters.Add(CreateParameter(deleteCommand, "@userName", userName));

                        int rowsAffected = deleteCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            this.Close();
                            MessageBox.Show("Borrow record deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete borrow record. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private DbParameter CreateParameter(DbCommand command, string name, object value)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }
    }
}
