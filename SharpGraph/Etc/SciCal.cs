namespace SharpGraph.Etc
{
    public partial class SciCal : Form
    {
        double dFirstVal, dSecondVal;
        string? op;
        public SciCal()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void btnerase_Click(object sender, EventArgs e)
        {
            if (txtResult.Text.Length > 0)
            {
                txtResult.Text = txtResult.Text.Remove(txtResult.Text.Length - 1, 1);
            }
            if (txtResult.Text == "")
            {
                txtResult.Text = "0";
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtResult.Text = "0";
        }

        private void btnpm_Click(object sender, EventArgs e)
        {
            double q = Convert.ToDouble(txtResult.Text);
            txtResult.Text = Convert.ToString(-1 * q);
        }

        private void EnterNumber(object sender, EventArgs e)
        {
            Button num = (Button)sender;
            if (txtResult.Text == "0")
                txtResult.Text = "";
            {
                if (num.Text == ".")
                {
                    if (!txtResult.Text.Contains("."))
                        txtResult.Text = txtResult.Text + num.Text;
                }
                else
                {
                    txtResult.Text = txtResult.Text + num.Text;
                }
            }
        }

        private void Numberop(object sender, EventArgs e)
        {
            Button num = (Button)sender;
            dFirstVal = Convert.ToDouble(txtResult.Text);
            op = num.Text;
            txtResult.Text = "";
        }

        private void btnequal_Click(object sender, EventArgs e)
        {
            dSecondVal = Convert.ToDouble(txtResult.Text);
            switch (op)
            {
                case "+":
                    txtResult.Text = (dFirstVal + dSecondVal).ToString();
                    break;
                case "-":
                    txtResult.Text = (dFirstVal - dSecondVal).ToString();
                    break;
                case "*":
                    txtResult.Text = (dFirstVal * dSecondVal).ToString();
                    break;
                case "/":
                    txtResult.Text = (dFirstVal / dSecondVal).ToString();
                    break;
                case "mod":
                    txtResult.Text = (dFirstVal % dSecondVal).ToString();
                    break;
                case "exp":
                    double i = Convert.ToDouble(txtResult.Text);
                    double j;
                    j = dSecondVal;
                    txtResult.Text = Math.Sqrt(i * Math.Log(j * 4)).ToString();
                    break;
                default:
                    break;

                }
            }

        private void btnpi_Click(object sender, EventArgs e)
        {
            txtResult.Text = "3.141592653589976323";
        }

        private void btnlog_Click(object sender, EventArgs e)
        {
            double logg = Convert.ToDouble(txtResult.Text);
            logg = Math.Log10(logg);
            txtResult.Text = Convert.ToString(logg);
        }

        private void btnsqrt_Click(object sender, EventArgs e)
        {
            double sq = Convert.ToDouble(txtResult.Text);
            sq = Math.Sqrt(sq);
            txtResult.Text = Convert.ToString(sq);
        }

        private void btnsinh_Click(object sender, EventArgs e)
        {
            double sh = Convert.ToDouble(txtResult.Text);
            sh = Math.Sinh(sh);
            txtResult.Text = Convert.ToString(sh);
        }

        private void btnsin_Click(object sender, EventArgs e)
        {
            double sin = Convert.ToDouble(txtResult.Text);
            sin = Math.Sin(sin);
            txtResult.Text = Convert.ToString(sin);
        }

        private void btndec_Click(object sender, EventArgs e)
        {
            double dec = Convert.ToDouble(txtResult.Text);
            int i1 = Convert.ToInt32(dec);
            int i2 = (int)dec;
            txtResult.Text = Convert.ToString(i2);
        }

        private void btncosh_Click(object sender, EventArgs e)
        {
            double cosh = Convert.ToDouble(txtResult.Text);
            cosh = Math.Cosh(cosh);
            txtResult.Text = Convert.ToString(cosh);
        }

        private void btncos_Click(object sender, EventArgs e)
        {
            double cos = Convert.ToDouble(txtResult.Text);
            cos = Math.Cos(cos);
            txtResult.Text = Convert.ToString(cos);
        }

        private void btnbin_Click(object sender, EventArgs e)
        {
            int a = int.Parse(txtResult.Text);
            txtResult.Text = Convert.ToString(a, 2);
        }

        private void btntanh_Click(object sender, EventArgs e)
        {
            double tanh = Convert.ToDouble(txtResult.Text);
            tanh = Math.Tanh(tanh);
            txtResult.Text = Convert.ToString(tanh);
        }

        private void btntan_Click(object sender, EventArgs e)
        {
            double tan = Convert.ToDouble(txtResult.Text);
            tan = Math.Tan(tan);
            txtResult.Text = Convert.ToString(tan);
        }

        private void btnhex_Click(object sender, EventArgs e)
        {
            int a = int.Parse(txtResult.Text);
            txtResult.Text = Convert.ToString(a, 16);
        }

        private void btnexp_Click(object sender, EventArgs e)
        {
            Button num = (Button)sender;
            dFirstVal = Convert.ToDouble(txtResult.Text);
            op = num.Text;
            txtResult.Text = "";
        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            Button num = (Button)sender;
            dFirstVal = Convert.ToDouble(txtResult.Text);
            op = num.Text;
            txtResult.Text = "";
        }

        private void btnoct_Click(object sender, EventArgs e)
        {
            int a = int.Parse(txtResult.Text);
            txtResult.Text = Convert.ToString(a, 8);
        }

        private void btnpercentage_Click(object sender, EventArgs e)
        {
            double a;
            a = Convert.ToDouble(txtResult.Text) / Convert.ToDouble(100);
            txtResult.Text = Convert.ToString(a);
        }

        private void btninx_Click(object sender, EventArgs e)
        {
            double inx = Convert.ToDouble(txtResult.Text);
            inx = Math.Log(inx);
            txtResult.Text = Convert.ToString(inx);
        }

        private void btn1divx_Click(object sender, EventArgs e)
        {
            double a;
            a = Convert.ToDouble(1.0 / Convert.ToDouble(txtResult.Text));
            txtResult.Text = Convert.ToString(a);
        }

        private void btnpower3_Click(object sender, EventArgs e)
        {
            double x, q, p, m;
            q = Convert.ToDouble(txtResult.Text);
            p = Convert.ToDouble(txtResult.Text);
            m = Convert.ToDouble(txtResult.Text);

            x = (q * p * m);
            txtResult.Text = Convert.ToString(x);
        }

        private void buttonunit_Click(object sender, EventArgs e)
        {
            timerscientific.Start();
        }

        private void btnclearenter_Click(object sender, EventArgs e)
        {
            txtResult.Text = "0";
            string f, s;

            f = Convert.ToString(dFirstVal);
            s = Convert.ToString(dSecondVal);
            f = "";
            s = "";
        }
    }
}
