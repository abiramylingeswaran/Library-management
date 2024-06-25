using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library01
{
    public partial class Newbook : Form
    {
        private Bookborrow _bookborrowForm;

        public Newbook(Bookborrow bookborrowForm)
        {
            InitializeComponent();
            _bookborrowForm = bookborrowForm;
        }


        private void button1_Click(object sender, EventArgs e)

        {
            string connectionString = "Data Source=DESKTOP-KRP578P;Initial Catalog=Library;Integrated Security=True";

            string bookTitle = textBox1.Text;
            int noOfBooks;
            if (!int.TryParse(textBox2.Text, out noOfBooks))
            {
                MessageBox.Show("Please enter a valid number for the number of books.");
                return;
            }
            string author = textBox3.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Book1 (BookName, NoofBook, Author) VALUES (@BookName, @NoofBook, @Author)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@BookName", bookTitle);
                        command.Parameters.AddWithValue("@NoofBook", noOfBooks);
                        command.Parameters.AddWithValue("@Author", author);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            this.Close();
                            MessageBox.Show("New book added successfully!");
                            //_bookborrowForm.LoadBooks(); // Refresh the DataGridView in Bookborrow form
                        }
                        else
                        {
                            MessageBox.Show("Failed to add the book. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
