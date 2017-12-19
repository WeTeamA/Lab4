
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
            switch ((sender as Button).Text)
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
                    textBox1.Text = textBox1.Text.Insert(start, (sender as Button).Text);
                    start += (sender as Button).Text.Length;
                    char c = (sender as Button).Text[0];
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
            double n = new double();
            if (radioButtonDeg.Checked) n = Math.PI/180;
            if (radioButtonRad.Checked) n = 1;
            try
            {
                Tokens t = new Tokens(textBox1.Text);
                TokenTree st = new TokenTree(t.TokensQueue, n);
                textBox2.Text = st.Result.ToString().Replace(",", ".");
            }
            catch { MessageBox.Show("Неверное выражение"); }

        }
    }
}
