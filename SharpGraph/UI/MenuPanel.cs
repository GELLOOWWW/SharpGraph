using SharpGraph.Etc;

namespace SharpGraph.UI
{
    /// <summary>
    /// A UserControl containing a ToolStrip with panels for opening forms, settings, and app description.
    /// </summary>
    public class MenuPanel : UserControl
    {
        private ToolStrip? toolStrip;
        private ToolStripDropDownButton? formsDropDownButton;
        private ToolStripDropDownButton? settingsDropDownButton;
        private ToolStripDropDownButton? aboutDropDownButton;

        public MenuPanel()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.toolStrip = new ToolStrip();
            this.formsDropDownButton = new ToolStripDropDownButton("App");
            this.settingsDropDownButton = new ToolStripDropDownButton("Settings");
            this.aboutDropDownButton = new ToolStripDropDownButton("About");

            this.toolStrip.Dock = DockStyle.Top;
            this.toolStrip.GripStyle = ToolStripGripStyle.Hidden;

            // Setup Forms DropDownButton items (example forms)
            var form1Item = new ToolStripMenuItem("Open Unit Converter");
            form1Item.Click += (s, e) => OpenForm(new UnitConverter());

            var form2Item = new ToolStripMenuItem("Open Scientific Calculator");
            form2Item.Click += (s, e) => OpenForm(new SciCal());

            this.formsDropDownButton.DropDownItems.Add(form1Item);
            this.formsDropDownButton.DropDownItems.Add(form2Item);
            this.formsDropDownButton.ToolTipText = "Open other apps";

            // Setup Settings DropDownButton items (example settings)
            var setting1Item = new ToolStripMenuItem("Settings");
            setting1Item.Click += (s, e) => ShowMessage("Implementation Soon!");

            this.settingsDropDownButton.DropDownItems.Add(setting1Item);
            this.settingsDropDownButton.ToolTipText = "Settings options";

            // Setup About DropDownButton shows description label
            var about1Item = new ToolStripMenuItem("About");
            about1Item.Click += (_, _) => ShowMessage("idk");

            this.aboutDropDownButton.DropDownItems.Add(about1Item);
            this.aboutDropDownButton.ToolTipText = "About this application";

            // Add drop down buttons to the ToolStrip
            this.toolStrip.Items.Add(this.formsDropDownButton);
            this.toolStrip.Items.Add(new ToolStripSeparator());
            this.toolStrip.Items.Add(this.settingsDropDownButton);
            this.toolStrip.Items.Add(new ToolStripSeparator());
            this.toolStrip.Items.Add(this.aboutDropDownButton);

            // Add the ToolStrip to the UserControl
            this.Controls.Add(this.toolStrip);

            // Set UserControl size defaults
            this.Height = toolStrip.Height;
            this.Dock = DockStyle.Top;
        }

        private static void OpenForm(Form form)
        {
            if (form == null) return;
            form.StartPosition = FormStartPosition.CenterParent;
            form.Show();
        }

        private static void ShowMessage(string message)
        {
            MessageBox.Show(message, "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
