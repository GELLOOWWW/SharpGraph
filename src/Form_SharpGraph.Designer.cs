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
        ///  But i edited it and will continue to do so :)
        /// </summary>
        private void InitializeComponent()
        {
            splitContainerScreen = new SplitContainer();
            pbGraphScreen = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)splitContainerScreen).BeginInit();
            splitContainerScreen.Panel2.SuspendLayout();
            splitContainerScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbGraphScreen).BeginInit();
            SuspendLayout();
            // 
            // splitContainerScreen
            // 
            splitContainerScreen.Dock = DockStyle.Fill;
            splitContainerScreen.Location = new Point(0, 0);
            splitContainerScreen.Margin = new Padding(3, 2, 3, 2);
            splitContainerScreen.Name = "splitContainerScreen";
            // 
            // splitContainerScreen.Panel1
            // 
            splitContainerScreen.Panel1.BackColor = Color.Silver;
            // 
            // splitContainerScreen.Panel2
            // 
            splitContainerScreen.Panel2.Controls.Add(pbGraphScreen);
            splitContainerScreen.Size = new Size(944, 581);
            splitContainerScreen.SplitterDistance = 354;
            splitContainerScreen.TabIndex = 4;
            // 
            // pbGraphScreen
            // 
            pbGraphScreen.Dock = DockStyle.Fill;
            pbGraphScreen.Location = new Point(0, 0);
            pbGraphScreen.Name = "pbGraphScreen";
            pbGraphScreen.Size = new Size(586, 581);
            pbGraphScreen.TabIndex = 0;
            pbGraphScreen.TabStop = false;
            // 
            // FormSharpGraph
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 580);
            Controls.Add(splitContainerScreen);
            Name = "FormSharpGraph";
            Text = "SharpGraph";
            Load += FormSharpGraph_Load;
            splitContainerScreen.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerScreen).EndInit();
            splitContainerScreen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbGraphScreen).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainerScreen;
        private PictureBox pbGraphScreen;
    }
}
