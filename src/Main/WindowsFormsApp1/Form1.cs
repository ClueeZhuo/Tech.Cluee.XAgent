using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //webBrowser1.Navigate("http://localhost:8080/OpenMES/AutoUpdate/ANNOUNCE.txt");

            var str =Application.StartupPath+ @"/ANNOUNCE.txt";

            webBrowser1.Navigate(str);
        }
    }
}
