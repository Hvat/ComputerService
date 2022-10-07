using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ComputerService
{
    public partial class Log_in : Form
    {
        DataBase dataBase = new DataBase();
        public Log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Log_in_Load(object sender, EventArgs e)
        {
            textBox_password.PasswordChar = '*';
            textBox_login.MaxLength = 50;
            textBox_password.MaxLength = 50;
        }

        private void button_log_Click(object sender, EventArgs e)
        {
            var loginUser = textBox_login.Text;
            var passUser = textBox_password.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            string querystring = $"select ID_Пользователя, ID_Сотрудника, Имя_Пользователя, Пароль_Пользователя" +
                $" from Регистрация where Имя_Пользователя = '{loginUser}' and Пароль_Пользователя = '{passUser}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConection());

            adapter.SelectCommand = command;
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Вход выполнен!","Успешно!",MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();
            }
            else
                MessageBox.Show("Пользователь не найден!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
