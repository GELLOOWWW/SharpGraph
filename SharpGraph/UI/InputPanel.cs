using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGraph.Expressions;

namespace SharpGraph.UI
{
    /// <summary>
    /// Represents a panel for inputting and displaying mathematical expressions.
    /// </summary>
    public class InputPanel
    {
        private readonly Panel container;
        private readonly RichTextBox txtbxInput = new();
        private readonly Button btnAdd = new();
        private readonly TableLayoutPanel tblExprList = new();
        private readonly int expressionLimit;

        /// <summary>
        /// Event triggered when an expression is submitted.
        /// </summary>
        public event Func<string, Task>? ExpressionSubmitted;

        /// <summary>
        /// Event triggered when an expression is removed.
        /// </summary>
        public event Action<string>? ExpressionRemoved;

        /// <summary>
        /// Initializes a new instance of the InputPanel class.
        /// </summary>
        /// <param name="panel">The parent panel to contain the input controls.</param>
        /// <param name="limit">The maximum number of expressions allowed.</param>
        public InputPanel(Panel panel, int limit = 8)
        {
            this.container = panel ?? throw new ArgumentNullException(nameof(panel));
            this.expressionLimit = limit;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            container.Controls.Clear();
            container.AutoScroll = true;

            // TableLayoutPanel for expressions
            tblExprList.Dock = DockStyle.Fill;
            tblExprList.AutoScroll = true;
            tblExprList.ColumnCount = 2;
            tblExprList.RowCount = 0;
            // Set fixed width for button column (30px) and fixed or percentage width for label column
            tblExprList.ColumnStyles.Clear();
            // Label column fixed width, e.g., 250 px (adjust as needed)
            tblExprList.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            // Button column fixed width
            tblExprList.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
            container.Controls.Add(tblExprList);
            // Input box
            txtbxInput.Height = 28;
            txtbxInput.Font = Settings.FontDefault;
            txtbxInput.Dock = DockStyle.Top;
            txtbxInput.KeyDown += InputBox_KeyDown;
            tblExprList.Controls.Add(txtbxInput);
            // Add button
            btnAdd.Text = "+";
            btnAdd.Width = 32;
            btnAdd.Height = 28;
            btnAdd.Dock = DockStyle.Top;
            btnAdd.Click += async (_, _) => await SubmitExpressionAsync();
            tblExprList.Controls.Add(btnAdd);
        }
        private async Task SubmitExpressionAsync()
        {
            string expression = txtbxInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(expression) || tblExprList.RowCount >= expressionLimit)
                return;

            var result = await ExpressionParser.TryParseAsync(expression);
            if (!result.IsValid)
            {
                txtbxInput.BackColor = Color.MistyRose;
                return;
            }

            txtbxInput.BackColor = SystemColors.Window;
            txtbxInput.Clear();

            AddExpressionRow(expression);
            if (ExpressionSubmitted != null)
                await ExpressionSubmitted.Invoke(expression);
        }

        private void AddExpressionRow(string expression)
        {
            // Add a new row
            int rowIndex = tblExprList.RowCount++;
            tblExprList.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            // Create label
            var label = new Label
            {
                Text = expression,
                AutoEllipsis = true,
                MaximumSize = new Size(245, 0), // Slightly less than column width to avoid clipping
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(3, 5, 3, 5)
            };
            tblExprList.Controls.Add(label, 0, rowIndex);
            // Create remove button
            var removeButton = new Button
            {
                Text = "X",
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };
            removeButton.Click += (_, _) =>
            {
                RemoveExpressionRow(label, removeButton);
                ExpressionRemoved?.Invoke(expression);
            };
            tblExprList.Controls.Add(removeButton, 1, rowIndex);
        }
        private void RemoveExpressionRow(Control label, Control button)
        {
            int rowIndex = tblExprList.GetRow(label);

            // Remove controls for the row
            tblExprList.Controls.Remove(label);
            tblExprList.Controls.Remove(button);

            // Shift rows above up
            for (int i = rowIndex + 1; i < tblExprList.RowCount; i++)
            {
                foreach (Control ctrl in tblExprList.Controls)
                {
                    if (tblExprList.GetRow(ctrl) == i)
                    {
                        tblExprList.SetRow(ctrl, i - 1);
                    }
                }
            }

            // Remove last row style and decrease count
            if (tblExprList.RowCount > 0)
            {
                tblExprList.RowStyles.RemoveAt(tblExprList.RowCount - 1);
                tblExprList.RowCount--;
            }
        }

        private async void InputBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await SubmitExpressionAsync();
            }
        }
    }
}
