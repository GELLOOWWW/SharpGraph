using SharpGraph.Expressions;

namespace SharpGraph.UI
{
    public class InputPanel
    {
        private readonly Panel container;
        private readonly RichTextBox txtbxInput = new();
        private readonly Button btnAdd = new();
        private readonly Button inputColorButton = new();
        private readonly TableLayoutPanel tblExprList = new();
        private readonly int expressionLimit;

        public event Func<string, Color, Task>? ExpressionSubmitted;
        public event Action<int>? ExpressionRemoved;
        public event Action<int, ParsedExpression>? ExpressionModified;

        private readonly int RowHeight = Settings.Scale(45);
        private Color currentInputColor = Color.Blue;

        // Keep track of expression rows with expressions and UI controls
        private readonly List<(RichTextBox TextBox, Button ColorBtn, Button RemoveBtn, Color SelectedColor, string ExpressionText)> expressionRows = [];

        public InputPanel(Panel panel, int limit = 8)
        {
            container = panel ?? throw new ArgumentNullException(nameof(panel));
            expressionLimit = limit;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            container.Controls.Clear();
            container.BackColor = Color.LightGray;
            container.AutoScroll = true;

            tblExprList.Dock = DockStyle.Fill;
            tblExprList.ColumnCount = 3;
            tblExprList.RowCount = 0;
            tblExprList.ColumnStyles.Clear();
            tblExprList.RowStyles.Clear();
            tblExprList.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tblExprList.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblExprList.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            container.Controls.Add(tblExprList);

            AddInputRow();
            AddFillerRow();
        }


        private void AddInputRow()
        {
            int idx = tblExprList.RowCount++;
            tblExprList.RowStyles.Add(new RowStyle(SizeType.Absolute, RowHeight));

            inputColorButton.BackColor = currentInputColor;
            inputColorButton.Dock = DockStyle.Fill;
            inputColorButton.Margin = new Padding(5);
            inputColorButton.FlatStyle = FlatStyle.Flat;
            inputColorButton.FlatAppearance.BorderSize = 0;
            inputColorButton.TabStop = false;
            inputColorButton.Click += (s, e) => ChangeInputColor();

            tblExprList.Controls.Add(inputColorButton, 0, idx);

            txtbxInput.Font = Settings.ExpressionFont;
            txtbxInput.BorderStyle = BorderStyle.FixedSingle;
            txtbxInput.Dock = DockStyle.Fill;
            txtbxInput.Margin = new Padding(3, 5, 3, 5);
            txtbxInput.KeyDown += InputBox_KeyDown;
            tblExprList.Controls.Add(txtbxInput, 1, idx);

            btnAdd.Text = "➕";
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.BackColor = Color.LightGreen;
            btnAdd.Margin = new Padding(3, 5, 3, 5);
            btnAdd.Click += async (_, _) => await SubmitExpressionAsync();
            tblExprList.Controls.Add(btnAdd, 2, idx);
        }

        private void ChangeInputColor()
        {
            using var dlg = new ColorDialog { Color = inputColorButton.BackColor };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                currentInputColor = dlg.Color;
                inputColorButton.BackColor = dlg.Color;
            }
        }

        private async Task SubmitExpressionAsync()
        {
            string expr = txtbxInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(expr) || expressionRows.Count >= expressionLimit)
                return;

            var parseResult = await ExpressionParser.TryParseAsync(expr, GetColorForExpression(expr));
            if (!parseResult.IsValid)
            {
                txtbxInput.BackColor = Color.MistyRose;
                return;
            }
            txtbxInput.BackColor = SystemColors.Window;
            txtbxInput.Clear();

            RemoveFillerRow();
            AddExpressionRow(expr, currentInputColor);
            AddFillerRow();

            if (ExpressionSubmitted != null)
                await ExpressionSubmitted(expr, currentInputColor);

            // Set inputColorButton to a new random color from the settings
            SetRandomInputColor();
        }

        private void AddExpressionRow(string expr, Color color)
        {
            int rowIndex = HasFillerRow() ? tblExprList.RowCount - 1 : tblExprList.RowCount;

            tblExprList.RowCount++;
            tblExprList.RowStyles.Insert(rowIndex, new RowStyle(SizeType.Absolute, RowHeight));

            foreach (Control c in tblExprList.Controls)
                if (tblExprList.GetRow(c) >= rowIndex)
                    tblExprList.SetRow(c, tblExprList.GetRow(c) + 1);

            var btnColor = new Button
            {
                BackColor = color,
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                TabStop = false,
            };
            btnColor.FlatAppearance.BorderSize = 0;
            btnColor.Click += (s, e) => ChangeColor_Click(s, e, btnColor);
            tblExprList.Controls.Add(btnColor, 0, rowIndex);

            var rtxbxExpr = new RichTextBox
            {
                Text = expr,
                Font = Settings.ExpressionFont,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Margin = new Padding(3, 5, 3, 5),
                Multiline = false,
                AcceptsTab = false,
                Tag = rowIndex // store row index in Tag
            };
            rtxbxExpr.TextChanged += (s, e) =>
            {
                if (s is RichTextBox tb && tb.Tag is int idx)
                {
                    UpdateExpression(idx, tb.Text);
                }
            };
            tblExprList.Controls.Add(rtxbxExpr, 1, rowIndex);

            var btnRemove = new Button
            {
                Text = "❌",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Red,
                Dock = DockStyle.Fill,
                Margin = new Padding(3, 5, 3, 5),
                TabStop = false,
                Tag = rowIndex,
            };
            tblExprList.Controls.Add(btnRemove, 2, rowIndex);

            expressionRows.Add((rtxbxExpr, btnColor, btnRemove, color, expr));
            btnRemove.Click += (s, e) =>
            {
                if (s is Button btn && btn.Tag is int idx)
                {
                    RemoveExpressionRow(expressionRows[idx]);
                }
            };

            UpdateBoxTags();
        }

        private async void UpdateExpression(int i, string newExpression)
        {
            if (i < 0 || i >= expressionRows.Count) return;

            var (textBox, colorBtn, removeBtn, selectedColor, _) = expressionRows[i];
            expressionRows[i] = (textBox, colorBtn, removeBtn, selectedColor, newExpression);

            try
            {
                await Task.Delay(50); // slight delay for when user is typing

                var parsedNewExpr = await ExpressionParser.TryParseAsync(newExpression, GetColorForExpression(newExpression));
                if (!parsedNewExpr.IsValid)
                {
                    textBox.BackColor = Settings.ErrorColor;
                }
                else
                {
                    ExpressionModified?.Invoke(i, parsedNewExpr);
                    textBox.BackColor = Settings.BgColor;
                }
            } catch {  } // ignore cancellation, because most likely user is still typing
        }

        private void RemoveExpressionRow((RichTextBox TextBox, Button ColorBtn, Button RemoveBtn, Color SelectedColor, string ExpressionText) row)
        {
            int index = expressionRows.IndexOf(row);
            if (index == -1) return;
            int rIdx = tblExprList.GetRow(row.TextBox);

            tblExprList.Controls.Remove(row.TextBox);
            tblExprList.Controls.Remove(row.ColorBtn);
            tblExprList.Controls.Remove(row.RemoveBtn);

            ExpressionRemoved?.Invoke(index);
            expressionRows.RemoveAt(index);

            for (int i = 0; i < tblExprList.Controls.Count; i++)
            {
                var ctrl = tblExprList.Controls[i];
                int r = tblExprList.GetRow(ctrl);
                if (r > rIdx)
                    tblExprList.SetRow(ctrl, r - 1);
            }

            if (tblExprList.RowStyles.Count > rIdx)
            {
                tblExprList.RowStyles.RemoveAt(rIdx);
                tblExprList.RowCount--;
            }

            UpdateBoxTags();
        }

        private void UpdateBoxTags()
        {
            for (int i = 0; i < expressionRows.Count; i++)
            {
                expressionRows[i].TextBox.Tag = i;
                expressionRows[i].RemoveBtn.Tag = i;
            }
        }

        private void SetRandomInputColor()
        {
            var random = new Random();
            if (Settings.ExpressionColors == null || Settings.ExpressionColors.Count == 0)
                return;
            int index = random.Next(Settings.ExpressionColors.Count);
            currentInputColor = Settings.ExpressionColors[index];
            inputColorButton.BackColor = currentInputColor;
        }

        private async void ChangeColor_Click(object? sender, EventArgs e, Button colorBtn)
        {
            using var dlg = new ColorDialog { Color = colorBtn.BackColor };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                colorBtn.BackColor = dlg.Color;
                for (int i = 0; i < expressionRows.Count; i++)
                {
                    if (expressionRows[i].ColorBtn == colorBtn)
                    {
                        expressionRows[i] = (expressionRows[i].TextBox, colorBtn, expressionRows[i].RemoveBtn, dlg.Color, expressionRows[i].ExpressionText);
                        var parsedExpr = await ExpressionParser.TryParseAsync(expressionRows[i].ExpressionText, dlg.Color);
                        ExpressionModified?.Invoke(i, parsedExpr);
                        break;
                    }
                }
            }
        }

        private bool HasFillerRow()
        {
            if (tblExprList.RowCount == 0) return false;
            var style = tblExprList.RowStyles[tblExprList.RowCount - 1];
            return style.SizeType == SizeType.Percent && Math.Abs(style.Height - 100f) < 0.1f;
        }

        private void AddFillerRow()
        {
            if (HasFillerRow()) return;
            tblExprList.RowCount++;
            tblExprList.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            var p = new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty, BackColor = Color.Transparent };
            tblExprList.Controls.Add(p, 0, tblExprList.RowCount - 1);
            tblExprList.SetColumnSpan(p, 3);
        }

        private void RemoveFillerRow()
        {
            if (!HasFillerRow()) return;
            int idx = tblExprList.RowCount - 1;
            Control? filler = null;
            foreach (Control c in tblExprList.Controls)
                if (tblExprList.GetRow(c) == idx && tblExprList.GetColumnSpan(c) == 3)
                    filler = c;
            if (filler != null)
                tblExprList.Controls.Remove(filler);
            tblExprList.RowStyles.RemoveAt(idx);
            tblExprList.RowCount--;
        }

        private async void InputBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await SubmitExpressionAsync();
            }
        }

        public Color GetColorForExpression(string expression)
        {
            foreach (var row in expressionRows)
            {
                if (row.TextBox.Text == expression)
                    return row.SelectedColor;
            }
            return Color.Empty;
        }
    }
}