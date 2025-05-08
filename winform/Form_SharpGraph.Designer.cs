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
            txtboxTest = new RichTextBox();
            GraphScreen = new PictureBox();
            lblCoords = new Label();
            ((System.ComponentModel.ISupportInitialize)GraphScreen).BeginInit();
            SuspendLayout();
            // 
            // txtboxTest
            // 
            txtboxTest.Location = new Point(15, 15);
            txtboxTest.Margin = new Padding(3, 2, 3, 2);
            txtboxTest.Name = "txtboxTest";
            txtboxTest.Size = new Size(283, 31);
            txtboxTest.TabIndex = 1;
            txtboxTest.Text = "test";
            // 
            // GraphScreen
            // 
            GraphScreen.Anchor = AnchorStyles.None;
            GraphScreen.Location = new Point(385, 15);
            GraphScreen.Margin = new Padding(0);
            GraphScreen.Name = "GraphScreen";
            GraphScreen.Size = new Size(550, 550);
            GraphScreen.TabIndex = 2;
            GraphScreen.TabStop = false;
            GraphScreen.Paint += GraphScreen_Paint;
            GraphScreen.MouseMove += GraphScreen_MouseMove;
            // 
            // lblCoords
            // 
            lblCoords.AutoSize = true;
            lblCoords.Location = new Point(15, 525);
            lblCoords.Name = "lblCoords";
            lblCoords.Size = new Size(66, 15);
            lblCoords.TabIndex = 3;
            lblCoords.Text = "Coordinate";
            // 
            // FormSharpGraph
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(944, 581);
            Controls.Add(lblCoords);
            Controls.Add(GraphScreen);
            Controls.Add(txtboxTest);
            Name = "FormSharpGraph";
            Text = "SharpGraph";
            ((System.ComponentModel.ISupportInitialize)GraphScreen).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox txtboxTest;
        private PictureBox GraphScreen;
        private Label lblCoords;
    }
}
