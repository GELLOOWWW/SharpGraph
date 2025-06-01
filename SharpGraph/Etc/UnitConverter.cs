namespace SharpGraph.Etc
{
    public partial class UnitConverter : Form
    {
        readonly double meter;
        string? op;
        public UnitConverter()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "CF";
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "FC";
        }

        private void radioButton3_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "K";
        }

        private void radioButton6_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "KG";
        }

        private void radioButton5_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "GK";
        }
        private void radioButton4_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "LM";
        }
        private void radioButton10_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "ML";
        }

        private void radioButton9_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "KM";
        }

        private void radioButton8_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "MK";
        }

        private void radioButton7_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "MF";
        }

        private void radioButton11_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "FI";
        }

        private void radioButton12_CheckedChanged_1(object sender, EventArgs e)
        {
            op = "CM";
        }

        private void btnConvert_Click_1(object sender, EventArgs e)
        {
            switch (op)
            {
                case "KM":
                    double kilometer = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((1000 * kilometer).ToString());
                    break;
                case "MK":
                    double Meter = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((meter / 1000).ToString());
                    break;
                case "MF":
                    double Inches = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((3.280 * Inches).ToString());
                    break;
                case "FI":
                    double Feet = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((Feet * 12).ToString());
                    break;
                case "KG":
                    double Kilo = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((1000 * Kilo).ToString());
                    break;
                case "GK":
                    double Gram = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((Gram / 1000).ToString());
                    break;
                case "LM":
                    double Liter = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((1000 * Liter).ToString());
                    break;
                case "ML":
                    double Milliter = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((Milliter / 1000).ToString());
                    break;
                case "CM":
                    double Centemiter = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((Centemiter / 100).ToString());
                    break;
                case "CF":
                    double Celsius = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((((9 * Celsius) / 5) + 32).ToString());
                    break;
                case "FC":
                    double Fahrenheit = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((((Fahrenheit - 32) * 5) / 9).ToString());
                    break;
                case "K":
                    double Kelvin = Convert.ToDouble(txtConvert.Text);
                    lblConverter.Text = ((((9 * Kelvin) / 5) + 273.15).ToString());
                    break;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtConvert.Clear();
            lblConverter.Text = "0";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            radioButton7.Checked = false;
            radioButton8.Checked = false;
            radioButton9.Checked = false;
            radioButton10.Checked = false;
            radioButton11.Checked = false;
            radioButton12.Checked = false;

        }
    }
}
