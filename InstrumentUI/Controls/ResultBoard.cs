using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InstrumentUI_ATK.Controls
{
    /// <summary>
    /// This will display results in a tabular form, no of columns could be provided by the user
    /// </summary>
    public partial class ResultBoard : UserControl
    {
        private int _rowCounter = 0;
        private CustomFlowLayoutPanel _lastRow;
        private int[] _columnsWidth;

        public Font TextFont { get; set; }

        public Color TextColor { get; set; }

        public Color AlternateRowColor { get; set; }

        public Color BorderColor { get; set; }

        private ResultBoard()
        {
            InitializeComponent();

            _lastRow = customFlpFirstRow; // initialize the last row as first row
        }

        public ResultBoard(int columns, params int[] columnWidth)
            : this()
        {
            // intialize the variabel for further use
            _columnsWidth = columnWidth;

            // create required no of columns
            for (int cnt = 2; cnt < columns; cnt++) // 2 columns are already there so start from 3rd.
            {
                Label lbl = CreateLabel(cnt);
                customFlpFirstRow.Controls.Add(lbl);
            }

            // set the width of all the columns
            int counter = 0;
            foreach (var lbl in customFlpFirstRow.Controls.OfType<Label>())
            {
                if (columnWidth.Length > counter)
                    lbl.Width = columnWidth[counter];

                counter++;
            }

            // calculate the total width and set it as row width
            int totalwidth = columnWidth.Sum();
            customFlpFirstRow.Width = totalwidth + 15; // 15 px margin

            customFlpFirstRow.Visible = false; // initially hide the row
        }

        /// <summary>
        /// Create a row with required columns and add it in the control
        /// </summary>
        /// <param name="columnValues"></param>
        public void AddRow(params string[] columnValues)
        {
            if (_rowCounter == 0) // first record, so the exiting row will work
            {
                customFlpFirstRow.Visible = true; // make it visible
                customFlpFirstRow.BorderColor = this.BorderColor; // set border color
                customFlpFirstRow.BackColor = this.AlternateRowColor; // set the back color

                // set the values of the columns
                int counter = 0;
                foreach (var lbl in customFlpFirstRow.Controls.OfType<Label>())
                {
                    lbl.Font = this.TextFont;
                    lbl.ForeColor = this.TextColor;

                    if (columnValues.Length > counter)
                        lbl.Text = columnValues[counter];
                    switch (lbl.Text)
                    {
                        case "Pass":
                            lbl.ForeColor = Color.Green;
                            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                            break;
                        case "Fail":
                            lbl.ForeColor = Color.Red;
                            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                            break;
                        case "Low":
                            lbl.ForeColor = Color.Blue;
                            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                            break;
                        case "High":
                            lbl.ForeColor = Color.Orange;
                            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                            break;
                    }
                    counter++;
                }
            }
            else // need to create a new row
            {
                CustomFlowLayoutPanel row = CreateRow(); // create a new row

                // create columns and set columns values
                for (int cnt = 0; cnt < columnValues.Length; cnt++)
                {
                    Label lbl = CreateLabel(cnt);
                    row.Controls.Add(lbl);

                    lbl.Text = columnValues[cnt];
                    switch (lbl.Text)
                    {
                        case "Pass":
                            lbl.ForeColor = Color.Green;
                            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                            break;
                        case "Fail":
                            lbl.ForeColor = Color.Red;
                            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                            break;
                        case "Low":
                            lbl.ForeColor = Color.Blue;
                            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                            break;
                        case "High":
                            lbl.ForeColor = Color.Orange;
                            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                            break;
                    }
                }

                // set the location of this row, it must be overlap the upper 
                // row by 1 px to hide the border otherwise dual border will appear
                row.Location = new Point(_lastRow.Left, _lastRow.Bottom - 1);

                // set the alternate row color
                if (_rowCounter % 2 == 0)
                {
                    row.BackColor = this.AlternateRowColor;
                }

                // finally add the row in the control
                this.Controls.Add(row);

                // set the current row as last row for the next record
                _lastRow = row;
            }

            _rowCounter++;
        }

        /// <summary>
        /// creates a label with same properties as reuired
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private Label CreateLabel(int columnIndex)
        {
            Label lbl = new Label();
            lbl.AutoSize = lblFirst.AutoSize;
            lbl.TextAlign = lblFirst.TextAlign;
            lbl.Font = this.TextFont;
            lbl.ForeColor = this.TextColor;
            lbl.Margin = lblFirst.Margin;
            lbl.Height = lblFirst.Height;
            lbl.Width = _columnsWidth[columnIndex];

            return lbl;
        }

        /// <summary>
        /// creates a row with same properties as required
        /// </summary>
        /// <returns></returns>
        private CustomFlowLayoutPanel CreateRow()
        {
            CustomFlowLayoutPanel row = new CustomFlowLayoutPanel();
            row.HasColorBorder = customFlpFirstRow.HasColorBorder;
            row.Size = customFlpFirstRow.Size;
            row.FlowDirection = customFlpFirstRow.FlowDirection;
            row.WrapContents = customFlpFirstRow.WrapContents;
            row.BorderColor = this.BorderColor;

            return row;
        }
    }
}
