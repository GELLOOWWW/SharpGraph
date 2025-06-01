using System.Drawing.Imaging;
using SharpGraph.UI;

namespace SharpGraph
{
    public partial class FormSharpGraph : Form
    {
        public FormSharpGraph()
        {
            InitializeComponent();
        }

        private void FormSharpGraph_Load(object sender, EventArgs e)
        {
            InitSharpGraph.Start(spltPanels.Panel1, pbGraphScreen);

            var menuPanel = new MenuPanel();
            this.Controls.Add(menuPanel);
            menuPanel.DarkModeToggle += MenuPanel_DarkModeToggle;
            menuPanel.SaveGraph += Menupanel_SaveGraph;
        }

        private void MenuPanel_DarkModeToggle()
        {
            this.Invalidate();
            pbGraphScreen.Invalidate();
        }

        private void Menupanel_SaveGraph()
        {
            SaveFileDialog sfd = new()
            {
                Filter = "Choose Image(*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp",
                FileName =$"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Second}_SharpGraph",
            };
            ImageFormat format;

            if (sfd.ShowDialog() is DialogResult.OK)
            {
                string ext = Path.GetExtension(sfd.FileName);
                format = ext switch
                {
                    ".jpg" => ImageFormat.Jpeg,
                    ".bmp" => ImageFormat.Bmp,
                    ".png" => ImageFormat.Png,
                    _ => ImageFormat.Png,
                };
                using var bitmap = new Bitmap(pbGraphScreen.Width, pbGraphScreen.Height);
                pbGraphScreen.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                bitmap.Save(sfd.FileName, format);

                string msg = $"Image saved in: {Path.GetFullPath(sfd.FileName)}";
                MessageBox.Show(msg, "SharpGraph", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
