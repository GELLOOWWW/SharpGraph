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
            InitSharpGraph init = new(pbGraphScreen);
            init.StartScreen();
            init.StartPanel(spltPanels.Panel1);
        }
    }
}
