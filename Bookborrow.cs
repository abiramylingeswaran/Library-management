using System.Data.SqlClient;
using System.Data;

namespace Library01
{
    public partial class Bookborrow : Form
    {
        public Bookborrow()
        {
            InitializeComponent();
        }

        private void Bookborrow_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            LoadBooks();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadBooks();
            string connectionString = "Data Source=DESKTOP-KRP578P;Initial Catalog=Library;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string bookId = textBox1.Text;
                string username = textBox2.Text;
                DateTime borrowDate = dateTimePicker1.Value;

                connection.Open();

                string checkUserQuery = "SELECT COUNT(*) FROM Membership WHERE UserName = @UserName";
                SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection);
                checkUserCommand.Parameters.AddWithValue("@UserName", username);

                int userCount = (int)checkUserCommand.ExecuteScalar();
                if (userCount == 0)
                {
                    MessageBox.Show("The username does not exist.");
                    return;
                }

                string checkBookQuery = "SELECT NoofBook FROM Book1 WHERE BookId = @BookId";
                SqlCommand checkBookCommand = new SqlCommand(checkBookQuery, connection);
                checkBookCommand.Parameters.AddWithValue("@BookId", bookId);

                int noOfBooks = (int)checkBookCommand.ExecuteScalar();
                if (noOfBooks == 0)
                {
                    MessageBox.Show("The book is not available.");
                    return;
                }

                string insertQuery = "INSERT INTO Borrow (BookId, UserName, DateandTime) VALUES (@BookId, @UserName, @DateandTime)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@BookId", bookId);
                insertCommand.Parameters.AddWithValue("@UserName", username);
                insertCommand.Parameters.AddWithValue("@DateandTime", borrowDate);

                try
                {
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        string updateBookQuery = "UPDATE Book1 SET NoofBook = NoofBook - 1 WHERE BookId = @BookId";
                        SqlCommand updateBookCommand = new SqlCommand(updateBookQuery, connection);
                        updateBookCommand.Parameters.AddWithValue("@BookId", bookId);
                        updateBookCommand.ExecuteNonQuery();

                        MessageBox.Show("Borrowed successfully!");
                        LoadBooks();
                    }
                    else
                    {
                        MessageBox.Show("Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }

                this.Close();
            }
        }

        public void LoadBooks()
        {
            string connectionString = "Data Source=DESKTOP-KRP578P;Initial Catalog=Library;Integrated Security=True";
            string selectQuery = "SELECT  BookId, BookName,NoofBook,  Author FROM Book1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Clear existing rows
                            dataGridView1.Rows.Clear();
                            // Add columns if not already added
                            if (dataGridView1.Columns.Count == 0)
                            {
                                dataGridView1.Columns.Add("BookId", "Book ID");
                                dataGridView1.Columns.Add("BookName", "Book Name");
                                dataGridView1.Columns.Add("NoofBook", "Number of Books");
                                dataGridView1.Columns.Add("Author", "Author");
                            }
                            // Read each record and add to the DataGridView
                            while (reader.Read())
                            {
                                dataGridView1.Rows.Add(reader["BookId"], reader["BookName"], reader["NoofBook"], reader["Author"]);
                            }
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
