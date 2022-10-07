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
    public partial class New_entry : Form
    {
        readonly DataBase dataBase = new DataBase();

        public New_entry()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataBase.openConection();

            var name = textBoxName.Text;
            var descrip = textBoxDes.Text;
            var manuf = textBoxManuf.Text;
            var price = textBoxPrice.Text;
            var remain = textBoxRemain.Text;

            if (int.TryParse(textBoxProv.Text, out int id))
            {
                var addQvery = $"insert into Склад_Комплектующие(Наименование, Описание, Производитель," +
                    $" Стоимость, Остаток, ID_Поставщика) values ('{name}', '{descrip}', '{manuf}'," +
                    $" '{price}', '{remain}', '{id}')";

                var command = new SqlCommand(addQvery, dataBase.getConection());
                command.ExecuteNonQuery();

                MessageBox.Show("Запись создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Отклонено!!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            dataBase.closeConection();
        }
    }
}
