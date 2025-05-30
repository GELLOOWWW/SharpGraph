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
        }
    }
}
