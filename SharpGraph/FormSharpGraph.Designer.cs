namespace SharpGraph
{
    partial class FormSharpGraph
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSharpGraph));
            spltPanels = new SplitContainer();
            pbGraphScreen = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)spltPanels).BeginInit();
            spltPanels.Panel2.SuspendLayout();
            spltPanels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbGraphScreen).BeginInit();
            SuspendLayout();
            // 
            // spltPanels
            // 
            spltPanels.Dock = DockStyle.Fill;
            spltPanels.Location = new Point(0, 0);
            spltPanels.Name = "spltPanels";
            // 
            // spltPanels.Panel2
            // 
            spltPanels.Panel2.Controls.Add(pbGraphScreen);
            spltPanels.Size = new Size(1084, 661);
            spltPanels.SplitterDistance = 360;
            spltPanels.TabIndex = 0;
            // 
            // pbGraphScreen
            // 
            pbGraphScreen.Dock = DockStyle.Fill;
            pbGraphScreen.Location = new Point(0, 0);
            pbGraphScreen.Name = "pbGraphScreen";
            pbGraphScreen.Size = new Size(720, 661);
            pbGraphScreen.TabIndex = 0;
            pbGraphScreen.TabStop = false;
            // 
            // FormSharpGraph
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 661);
            Controls.Add(spltPanels);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormSharpGraph";
            Text = "Sharp Graph";
            Load += FormSharpGraph_Load;
            spltPanels.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spltPanels).EndInit();
            spltPanels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbGraphScreen).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer spltPanels;
        private PictureBox pbGraphScreen;
    }
}
