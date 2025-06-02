using SharpGraph.Etc;

namespace SharpGraph.UI
{
    /// <summary>
    /// A UserControl containing a ToolStrip with panels for opening forms, settings, and app description.
    /// </summary>
    public class MenuPanel : UserControl
    {
        private ToolStrip? toolStrip;
        private ToolStripDropDownButton? tsbtnApp;
        private ToolStripDropDownButton? tsbtnActions;
        private ToolStripDropDownButton? tsbtnAbout;

        public event Action? DarkModeToggle;
        public event Action? SaveGraph;

        public MenuPanel()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.toolStrip = new ToolStrip();
            this.tsbtnApp = new ToolStripDropDownButton("App");
            this.tsbtnActions = new ToolStripDropDownButton("Actions");
            this.tsbtnAbout = new ToolStripDropDownButton("About");

            this.toolStrip.Dock = DockStyle.Top;
            this.toolStrip.GripStyle = ToolStripGripStyle.Hidden;

            // Setup Forms DropDownButton items (example forms)
            var app1Item = new ToolStripMenuItem("Open Unit Converter");
            app1Item.Click += (s, e) => OpenForm(new UnitConverter());

            var app2Item = new ToolStripMenuItem("Open Calculator");
            app2Item.Click += (s, e) => OpenForm(new SciCal());

            this.tsbtnApp.DropDownItems.Add(app1Item);
            this.tsbtnApp.DropDownItems.Add(app2Item);
            this.tsbtnApp.ToolTipText = "Open other apps";

            // Dark Mode Toggle
            var actions1Item = new ToolStripMenuItem("[BETA] Toggle Dark Mode");
            actions1Item.Click += (_, _) => DarkMode();

            this.tsbtnActions.DropDownItems.Add(actions1Item);
            this.tsbtnActions.ToolTipText = "SharpGraph Actions";

            // Save Graph
            var actions2item = new ToolStripMenuItem("[BETA] Save Graph");
            actions2item.Click += (_, _) => SaveGraph?.Invoke();

            this.tsbtnActions.DropDownItems.Add(actions2item);

            // Setup About DropDownButton shows description label
            var about1Item = new ToolStripMenuItem("About");
            about1Item.Click += (_, _) => ShowMessage("SharpGraph is a Graphing Calculator inspired by Desmos and the TI-84 Calculator, fully written in C#, using no other libraries or frameworks outside the core .NET framework itself. This app is capable of graphing most, if not all implicit functions (f(x,y) = 0) in a Cartesian Coordinate Plane.");

            this.tsbtnAbout.DropDownItems.Add(about1Item);
            this.tsbtnAbout.ToolTipText = "About this application";

            // Add drop down buttons to the ToolStrip
            this.toolStrip.Items.Add(this.tsbtnApp);
            this.toolStrip.Items.Add(new ToolStripSeparator());
            this.toolStrip.Items.Add(this.tsbtnActions);
            this.toolStrip.Items.Add(new ToolStripSeparator());
            this.toolStrip.Items.Add(this.tsbtnAbout);

            // Add the ToolStrip to the UserControl
            this.Controls.Add(this.toolStrip);

            // Set UserControl size defaults
            this.Height = toolStrip.Height;
            this.Dock = DockStyle.Top;
        }

        static bool bDarkMode = false;
        private void DarkMode()
        {
            bDarkMode = !bDarkMode;
            Settings.BgColor = bDarkMode ? Color.Black : Color.White;
            Settings.AxisColor = bDarkMode ? Color.White : Color.Black;
            Settings.GridNumColor = bDarkMode ? Color.White : Color.Black;

            DarkModeToggle?.Invoke();
        }

        private static void OpenForm(Form form)
        {
            if (form == null) return;
            form.StartPosition = FormStartPosition.CenterParent;
            form.Show();
        }

        private static void ShowMessage(string message)
        {
            MessageBox.Show(message, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
