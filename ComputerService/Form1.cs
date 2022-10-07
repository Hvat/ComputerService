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
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }

    public partial class Form1 : Form
    {
        readonly DataBase dataBase = new DataBase();

        int selectedRow;

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void CreateColumns()
        {
            dataGridViewStock.Columns.Add("ID_Товара", "ID");
            dataGridViewStock.Columns.Add("Наименование", "Наименование");
            dataGridViewStock.Columns.Add("Описание", "Описание");
            dataGridViewStock.Columns.Add("Производитель", "Производитель");
            dataGridViewStock.Columns.Add("Стоимость", "Стоимость");
            dataGridViewStock.Columns.Add("Остаток", "Остаток");
            dataGridViewStock.Columns.Add("ID_Поставщика", "ID_П");
            dataGridViewStock.Columns.Add("Поставщик", "Поставщик");
            dataGridViewStock.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSigleRows(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt64(0),
                         record.GetString(1),
                         record.GetString(2),
                         record.GetString(3),
                         record.GetValue(4),
                         record.GetInt64(5),
                         record.GetInt64(6),
                         record.GetString(7),
                         RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string querystring = $"select Склад_Комплектующие.ID_Товара, Склад_Комплектующие.Наименование," +
                $" Склад_Комплектующие.Описание, Склад_Комплектующие.Производитель," +
                $" Склад_Комплектующие.Стоимость, Склад_Комплектующие.Остаток, Склад_Комплектующие.ID_Поставщика, Поставщики.Поставщик" +
                $" from Склад_Комплектующие, Поставщики" +
                $" where Склад_Комплектующие.ID_Поставщика = Поставщики.ID_Поставщика";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConection());

            dataBase.openConection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSigleRows(dgw, reader);
            }

            reader.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridViewStock);
        }

        private void dataGridViewStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewStock.Rows[selectedRow];

                textBoxId.Text = row.Cells[0].Value.ToString();
                textBoxName.Text = row.Cells[1].Value.ToString();
                textBoxDes.Text = row.Cells[2].Value.ToString();
                textBoxManuf.Text = row.Cells[3].Value.ToString();
                textBoxPrice.Text = row.Cells[4].Value.ToString();
                textBoxRemain.Text = row.Cells[5].Value.ToString();
                textBoxId_p.Text = row.Cells[6].Value.ToString();
                textBoxProv.Text = row.Cells[7].Value.ToString();

            }
        }

        private void ClearFields()
        {
            textBoxId.Text = "";
            textBoxName.Text = "";
            textBoxDes.Text = "";
            textBoxManuf.Text = "";
            textBoxPrice.Text = "";
            textBoxRemain.Text = "";
            textBoxId_p.Text = "";
            textBoxProv.Text = "";
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridViewStock);
            ClearFields();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            New_entry new_Entry = new New_entry();
            new_Entry.Show();
        }

        private void Search (DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchstring = $"select Склад_Комплектующие.ID_Товара, Склад_Комплектующие.Наименование," +
                $" Склад_Комплектующие.Описание, Склад_Комплектующие.Производитель," +
                $" Склад_Комплектующие.Стоимость, Склад_Комплектующие.Остаток, Склад_Комплектующие.ID_Поставщика, Поставщики.Поставщик" +
                $" from Склад_Комплектующие, Поставщики" +
                $" where concat (Склад_Комплектующие.ID_Товара, Склад_Комплектующие.Наименование, Склад_Комплектующие.Описание, Склад_Комплектующие.Производитель," +
                $" Склад_Комплектующие.Стоимость, Склад_Комплектующие.Остаток, Склад_Комплектующие.ID_Поставщика, Поставщики.Поставщик) like '%" + textBox1.Text + "%'";

            SqlCommand command = new SqlCommand(searchstring, dataBase.getConection());

            dataBase.openConection();

            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                ReadSigleRows(dgw, reader);
            }

            reader.Close();
        }

        private void deleteRow()
        {
            int index = dataGridViewStock.CurrentCell.RowIndex;

            dataGridViewStock.Rows[index].Visible = false;

            if (dataGridViewStock.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridViewStock.Rows[index].Cells[8].Value = RowState.Deleted;
                return;
            }

            dataGridViewStock.Rows[index].Cells[8].Value = RowState.Deleted;
        }

        private void Updates()
        {
            dataBase.openConection();

            for(int index = 0; index < dataGridViewStock.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridViewStock.Rows[index].Cells[8].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToUInt64(dataGridViewStock.Rows[index].Cells[0].Value);

                    var deleteQwery = $"delete from Склад_Комплектующие where Склад_Комплектующие.ID_Товара = '{id}'";

                    var command = new SqlCommand(deleteQwery, dataBase.getConection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridViewStock.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridViewStock.Rows[index].Cells[1].Value.ToString();
                    var descrip = dataGridViewStock.Rows[index].Cells[2].Value.ToString();
                    var manuf = dataGridViewStock.Rows[index].Cells[3].Value.ToString();
                    var price = dataGridViewStock.Rows[index].Cells[4].Value.ToString();
                    var remain = dataGridViewStock.Rows[index].Cells[5].Value.ToString();
                    var id_prov = dataGridViewStock.Rows[index].Cells[6].Value.ToString();

                    var changeQwery = $"update Склад_Комплектующие set Склад_Комплектующие.Наименование = '{name}'," +
                        $" Склад_Комплектующие.Описание = '{descrip}', Склад_Комплектующие.Производитель = '{manuf}'," +
                        $" Склад_Комплектующие.Стоимость = '{price}', Склад_Комплектующие.Остаток = '{remain}'," +
                        $" Склад_Комплектующие.ID_Поставщика = '{id_prov}' where Склад_Комплектующие.ID_Товара = '{id}'";

                    var command = new SqlCommand(changeQwery, dataBase.getConection());
                    command.ExecuteNonQuery();
                }
            }

            dataBase.closeConection();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search (dataGridViewStock);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Updates();
            ClearFields();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            deleteRow();
            ClearFields();
        }

        private void Change()
        {
            var selectRowIndex = dataGridViewStock.CurrentCell.RowIndex;
            
            var id = textBoxId.Text;
            var name = textBoxName.Text;
            var descrip = textBoxDes.Text;
            var manuf = textBoxManuf.Text;
            var price = textBoxPrice.Text;
            var id_prov = textBoxId_p.Text;
            var prov = textBoxProv.Text;

            if (dataGridViewStock.Rows[selectRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(textBoxRemain.Text, out int remain))
                {
                    dataGridViewStock.Rows[selectRowIndex].SetValues(id, name, descrip, manuf, price, remain, id_prov, prov);
                    dataGridViewStock.Rows[selectRowIndex].Cells[8].Value = RowState.Modified;
                }
                else
                    MessageBox.Show("Неверный формат!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Change();
        }
    }
}
