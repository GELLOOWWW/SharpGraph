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
            InitSharpGraph init = new();
            init.StartScreen(pbGraphScreen);
            init.StartPanel(spltPanels.Panel1);
        }
    }
}
