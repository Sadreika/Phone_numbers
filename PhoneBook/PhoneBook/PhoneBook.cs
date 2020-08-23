using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PhoneBook
{
    public partial class PhoneBook : Form
    {
        private string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=PhoneBookDatabase;Integrated Security=True";
        private string IdOfColumn = "";
        public void fillingTable()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("GetAllPhoneNumbers", con);
            DataTable data = new DataTable();
            sqlData.Fill(data);
            phoneNumberDataGridView.DataSource = data;
            con.Close();
        }
        public void setIdOfColumn(string id)
        {
            this.IdOfColumn = id;
        }
        public string returnIdOfColumn()
        {
            return this.IdOfColumn;
        }
        public PhoneBook()
        {
            InitializeComponent();
            fillingTable();
        }
        private void Add_Click(object sender, EventArgs e)
        {
            int numberId = searchingForMaxId();
            Add addingNumber = new Add(numberId);
            addingNumber.Show();
        }
        public int searchingForMaxId()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand command = new SqlCommand("MaxId", con);
            command.CommandType = CommandType.StoredProcedure;
            int id = 0;
            try
            {
                string stringId = command.ExecuteScalar().ToString();
                id = Int32.Parse(stringId);
            }
            catch (Exception)
            {
                id = 0;
            }
            con.Close();
            return id;
        }
        private void Delete_Click(object sender, EventArgs e)
        {
            string columnId = returnIdOfColumn();
            if (columnId == "")
            {
                MessageBox.Show("Select a column", "");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to remove this contact?", "Deleting contact", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    SqlCommand command = new SqlCommand("deletePhoneNumber", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = columnId;
                    command.ExecuteNonQuery();
                    con.Close();
                    fillingTable();
                }
            }
        }
        private void Update_Click(object sender, EventArgs e)
        {
            string columnId = returnIdOfColumn();
            if(columnId == "")
            {
                MessageBox.Show("Select a column", "");
            }
            else
            {
                Update updateObject = new Update(columnId);
                updateObject.Show();
            }
        }
        private void phoneNumberDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.phoneNumberDataGridView.Rows[e.RowIndex];
                try
                {
                    var id = row.Cells["Id"].Value.ToString();
                    setIdOfColumn(id);
                }
                catch (Exception)
                {
                }
            }
        }
        private void PhoneBook_Activated(object sender, EventArgs e)
        {
            fillingTable();
        }
    }
}
