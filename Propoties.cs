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

namespace Library01
{
    public partial class Propoties : Form
    {
        private Bookborrow _bookborrowForm;

        public Propoties()
        {
            InitializeComponent();
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Return ret = new Return();
            ret.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Bookborrow Bookborrow1 = new Bookborrow();
            // bookDetail.Show();

            // Connection string to your SQL Server database
            string connectionString = "Data Source=DESKTOP-KRP578P;Initial Catalog=Library;Integrated Security=True";
            //"Data Source=DESKTOP-KRP578P;Initial Catalog=Library;Integrated Security=True"
            //string connectionString = "Data Source=YourServer;Initial Catalog=YourDatabase;Integrated Security=True";

            // SQL query to select all columns from the Book table
            string query = "SELECT * FROM Book1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                Bookborrow1.dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                   

                    Bookborrow1.dataGridView1.Rows.Add(reader["BookId"], reader["BookName"], reader["NoofBook"]);
                }

                reader.Close();
            }

            Bookborrow1.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Newbook Newbook1 = new Newbook(_bookborrowForm);
            Newbook1.Show();
        }
    }
}
