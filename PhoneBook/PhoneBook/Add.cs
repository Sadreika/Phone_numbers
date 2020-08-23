using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PhoneBook
{
    public partial class Add : Form
    {
        private int id = 0;
        public void settingId(int id)
        {
            this.id = id;
        }
        public Add(int id)
        {
            InitializeComponent();
            settingId(id);
        }
        private string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=PhoneBookDatabase;Integrated Security=True";
        private void Submit_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string name = NameTextBox.Text.ToString();
            string surname = SurnameTextBox.Text.ToString();
            string phoneNumber = PhoneNumberTextBox.Text.ToString();
            string birth = BirthTextBox.Text.ToString();
            PhoneBook phoneBookObject = new PhoneBook();

            if (name != "" || surname != "" || phoneNumber != "")
            {
                if (birth == "")
                {
                    SqlCommand command = new SqlCommand("AddPhoneNumberWithOutDate", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = this.id + 1;
                    command.Parameters.AddWithValue("@Name", SqlDbType.NVarChar).Value = name;
                    command.Parameters.AddWithValue("@Surname", SqlDbType.NVarChar).Value = surname;
                    command.Parameters.AddWithValue("@PhoneNumber", SqlDbType.NVarChar).Value = phoneNumber;
                    command.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand command = new SqlCommand("AddPhoneNumber", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = this.id + 1;
                    command.Parameters.AddWithValue("@Name", SqlDbType.NVarChar).Value = name;
                    command.Parameters.AddWithValue("@Surname", SqlDbType.NVarChar).Value = surname;
                    command.Parameters.AddWithValue("@PhoneNumber", SqlDbType.NVarChar).Value = phoneNumber;
                    command.Parameters.AddWithValue("@BornDate", SqlDbType.Date).Value = DateTime.Parse(birth);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                MessageBox.Show("Cant create empty contact", "Cant create empty contact");
            }

            con.Close();
            this.Hide();
        }
    }
}
