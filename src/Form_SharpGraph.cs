using SharpGraph.Cartesian;

namespace SharpGraph
{
    public partial class FormSharpGraph : Form
    {
        public FormSharpGraph()
        {
            InitializeComponent();
            Text = "SharpGraph";
        }

        private void FormSharpGraph_Load(object sender, EventArgs e)
        {
            _ = new GraphRender(pbGraphScreen);
        }
    }
}