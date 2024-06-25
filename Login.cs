using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace Library01
{

    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        string Data = "Data Source=DESKTOP-KRP578P;Initial Catalog=Library;Integrated Security=True";
        private void btnLog_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Data;
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT * FROM Membership WHERE Username=@Username AND Password=@Password";
            cmd.Parameters.AddWithValue("@Username", textBox1.Text);
            cmd.Parameters.AddWithValue("@Password", textBox2.Text);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                this.Close();
                Propoties propoties = new Propoties();
                propoties.Show();

            }


            else
            {
                MessageBox.Show("Wrong Username OR Password");
            }

        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            Member home = new Member(); 
            home.Show();
        }
    }
}
