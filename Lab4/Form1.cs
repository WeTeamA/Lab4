
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace Lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int start = textBox1.SelectionStart;
            switch ((sender as System.Windows.Forms.Button).Text)
            {
                case "C": textBox1.Clear(); textBox2.Clear(); break;
                case "←": 
                    if (textBox1.Text!="")
                    {
                        textBox1.Text = textBox1.Text.Remove(start - 1, 1);
                        start--;
                    }
                    break;
                default:
                    textBox1.Text = textBox1.Text.Insert(start, (sender as System.Windows.Forms.Button).Text);
                    start += (sender as System.Windows.Forms.Button).Text.Length;
                    char c = (sender as System.Windows.Forms.Button).Text[0];
                    if (Char.IsLetter(c) && c != 'p' && c != 'e' && (start == textBox1.Text.Length || textBox1.Text[start] != '('))
                    {
                        textBox1.Text = textBox1.Text.Insert(start, "()");
                        start += 1;
                    }
                    break;
            }
            textBox1.Focus();
            textBox1.SelectionStart = start;
            textBox1.SelectionLength = 0;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            List<List<string>> Constants = new List<List<string>>();
            string con = "";
            for (int i = 0; i < listView.Items.Count; i++)
            {
                Constants.Add(new List<string> { listView.Items[i].SubItems[0].Text, listView.Items[i].SubItems[1].Text });
                con += listView.Items[i].SubItems[0].Text +"|";
            }
            con = con.Remove(con.Length - 1,1);
            string constant = @"^(?<sign>-?)(?<const>(" + con + "))";
            double n = new double();
            if (radioButtonDeg.Checked) n = Math.PI/180;
            if (radioButtonRad.Checked) n = 1;
            try
            {
                Tokens t = new Tokens(textBox1.Text, constant);
                TokenTree st = new TokenTree(t.TokensQueue, n, Constants);
                textBox2.Text = st.Result.ToString().Replace(",", ".");
            }
            catch { MessageBox.Show("Неверное выражение"); }

        }

        private void buttonAdd_const_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workBook;
            Microsoft.Office.Interop.Excel.Worksheet workSheet;
            workBook = excelApp.Workbooks.Open(@"C:\Users\Михаил\Google Диск\Учеба\Третий семестр\ООП\Лабораторная работа №4\Lab4\Lab4\bin\Debug\Constants.xlsx");
            workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets.get_Item(1);
            int count = 1;
            while (workSheet.Cells[count, 1].Text != "")             
                count++;
            workSheet.Cells[count, 1] = textBoxName.Text;
            workSheet.Cells[count, 2] = textBoxValue.Text;
            ListViewItem c = new ListViewItem(textBoxName.Text);
            c.SubItems.Add(textBoxValue.Text);
            listView.Items.Add(c);
            listView.Items[count-1].SubItems[0].Text = textBoxName.Text;
            listView.Items[count-1].SubItems[1].Text = textBoxValue.Text;
            textBoxName.Clear();
            textBoxValue.Clear();
            workBook.Save();
            workBook.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workBook;
            Microsoft.Office.Interop.Excel.Worksheet workSheet;
            workBook = excelApp.Workbooks.Open(@"C:\Users\Михаил\Google Диск\Учеба\Третий семестр\ООП\Лабораторная работа №4\Lab4\Lab4\bin\Debug\Constants.xlsx");//Свой путь прописать (и может быть создать excel таблицу Constants.xlsx)
            workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets.get_Item(1);
            int count = 1;
            while (workSheet.Cells[count, 1].Text != "")
            {
                ListViewItem c = new ListViewItem(workSheet.Cells[count, 1].Text);
                c.SubItems.Add(workSheet.Cells[count, 2].Text);
                listView.Items.Add(c);
                count++;
            }
            workBook.Save();
            workBook.Close();
            buttonAdd_const.Enabled = false;
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar)
                || (e.KeyChar == '.')
                || (e.KeyChar == ',')
                || (e.KeyChar == ',')
                || (e.KeyChar == '/')
                || (e.KeyChar == ';')
                || (e.KeyChar == ':')
                || (e.KeyChar == '}')
                || (e.KeyChar == ']')
                || (e.KeyChar == '[')
                || (e.KeyChar == '{')
                || (e.KeyChar == '=')
                || (e.KeyChar == '+')
                || (e.KeyChar == '-')
                || (e.KeyChar == '_')
                || (e.KeyChar == ')')
                || (e.KeyChar == '(')
                || (e.KeyChar == '*')
                || (e.KeyChar == '&')
                || (e.KeyChar == '?')
                || (e.KeyChar == '^')
                || (e.KeyChar == '%')
                || (e.KeyChar == '$')
                || (e.KeyChar == '#')
                || (e.KeyChar == '№')
                || (e.KeyChar == '"')
                || (e.KeyChar == '@')
                || (e.KeyChar == '!')
                || (e.KeyChar == '~')
                || (e.KeyChar == '>')
                || (e.KeyChar == '<')
                || (e.KeyChar == ' '))
            { 
                e.Handled = true;
            }                  
        }

        private void textBoxValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)
            || (e.KeyChar == '/')
            || (e.KeyChar == ';')
            || (e.KeyChar == ':')
            || (e.KeyChar == '}')
            || (e.KeyChar == ']')
            || (e.KeyChar == '[')
            || (e.KeyChar == '{')
            || (e.KeyChar == '=')
            || (e.KeyChar == '+')
            || (e.KeyChar == '-')
            || (e.KeyChar == '_')
            || (e.KeyChar == ')')
            || (e.KeyChar == '(')
            || (e.KeyChar == '*')
            || (e.KeyChar == '&')
            || (e.KeyChar == '?')
            || (e.KeyChar == '^')
            || (e.KeyChar == '%')
            || (e.KeyChar == '$')
            || (e.KeyChar == '#')
            || (e.KeyChar == '№')
            || (e.KeyChar == '"')
            || (e.KeyChar == '@')
            || (e.KeyChar == '!')
            || (e.KeyChar == '~')
            || (e.KeyChar == '>')
            || (e.KeyChar == '<')
            || (e.KeyChar == ' '))
            { e.Handled = true; }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxName.Text == "" || textBoxValue.Text == "")
                buttonAdd_const.Enabled = false;
            else buttonAdd_const.Enabled = true;
        }

        private void textBoxValue_TextChanged(object sender, EventArgs e)
        {
            if (textBoxName.Text == "" || textBoxValue.Text == "")
                buttonAdd_const.Enabled = false;
            else buttonAdd_const.Enabled = true;
        }
    }
}
