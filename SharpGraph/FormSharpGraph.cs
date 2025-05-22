using SharpGraph.Cartesian;

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
            GraphRender Graph = new(pbGraphScreen);
            Graph.Start();
        }
    }
}
