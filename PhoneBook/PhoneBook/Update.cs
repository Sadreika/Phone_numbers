using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System;

namespace PhoneBook
{
    public partial class Update : Form
    {
        private string columnId = "";
        public Update(string columnId)
        {
            InitializeComponent();
            settingColumnId(columnId);
            updatingUpdateForm();
        }

        public void settingColumnId(string columnId)
        {
            this.columnId = columnId;
        }
        private string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=PhoneBookDatabase;Integrated Security=True";
        public void updatingUpdateForm()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string Name = "";
            string Surname = "";
            string PhoneNumber = "";
            string Date = "";
            
            SqlCommand command = new SqlCommand("SelectingContact", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", Int32.Parse(columnId));
            SqlDataReader dataReader = command.ExecuteReader();
            
            if (dataReader.Read())
            {
                Name = (dataReader["Name"].ToString());
                Surname = (dataReader["Surname"].ToString());
                PhoneNumber = (dataReader["PhoneNumber"].ToString());
                Date = (dataReader["BornData"].ToString());
            }

            NameTextBox.Text = Name;
            SurnameTextBox.Text = Surname;
            PhoneNumberTextBox.Text = PhoneNumber;
            DateOfBirthTextBox.Text = Date;
            con.Close();
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string name = NameTextBox.Text.ToString();
            string surname = SurnameTextBox.Text.ToString();
            string phoneNumber = PhoneNumberTextBox.Text.ToString();
            string birth = DateOfBirthTextBox.Text.ToString();
            if (birth == "")
            {
                SqlCommand command = new SqlCommand("UpdatePhoneNumberWithOutDate", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Int32.Parse(columnId);
                command.Parameters.AddWithValue("@Name", SqlDbType.NVarChar).Value = name;
                command.Parameters.AddWithValue("@Surname", SqlDbType.NVarChar).Value = surname;
                command.Parameters.AddWithValue("@PhoneNumber", SqlDbType.Date).Value = phoneNumber;
                command.ExecuteNonQuery();
            }
            else
            {
                SqlCommand command = new SqlCommand("UpdatePhoneNumber", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Int32.Parse(columnId);
                command.Parameters.AddWithValue("@Name", SqlDbType.NVarChar).Value = name;
                command.Parameters.AddWithValue("@Surname", SqlDbType.NVarChar).Value = surname;
                command.Parameters.AddWithValue("@PhoneNumber", SqlDbType.Date).Value = phoneNumber;
                command.Parameters.AddWithValue("@BornDate", SqlDbType.Date).Value = DateTime.Parse(birth);
                command.ExecuteNonQuery();
            }
               
            con.Close();
            this.Hide();
        }
    }
}
