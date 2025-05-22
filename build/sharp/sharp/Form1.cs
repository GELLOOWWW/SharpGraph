using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sharp
{
    public partial class Form1 : Form
    {
        double enterFirstValue, enterSecondValue;
        bool sideBarexpond;
        string op;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (sideBarexpond)
            {
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sideBarexpond = false;
                    timer1.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sideBarexpond = true;
                    timer1.Stop();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            unitconverter unitsharp = new unitconverter();
            unitsharp.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            scientificcalculator wow = new scientificcalculator();
            wow.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
