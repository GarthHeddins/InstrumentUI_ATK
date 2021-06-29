using System.Drawing;
using System.Windows.Forms;

namespace InstrumentUI_ATK.Controls
{
    /// <summary>
    /// Displays a TableLayout control
    /// </summary>
    public partial class TableLayout : UserControl
    {
        /// <summary>
        /// Text Font
        /// </summary>
        public Font TextFont { get; set; }


        /// <summary>
        /// Text Color
        /// </summary>
        public Color TextColor { get; set; }


        /// <summary>
        /// Text Alignment within a Cell
        /// </summary>
        public ContentAlignment TextAlign
        {
            get { return _textAlign; } 
            set { _textAlign = value; }
        }
        private ContentAlignment _textAlign = ContentAlignment.MiddleCenter;


        /// <summary>
        /// Back color for alternate rows
        /// </summary>
        public Color AlternateRowColor { get; set; }


        /// <summary>
        /// Row Height
        /// </summary>
        public int RowHeight
        {
            get { return _rowHeight; }
            set { _rowHeight = value; }
        }
        private int _rowHeight = 25;



        /// <summary>
        /// Create a new instance of the TableLayout  class
        /// </summary>
        private TableLayout()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Create the control based on the column count and width of each column
        /// </summary>
        /// <param name="columns">Number of Columns</param>
        /// <param name="columnWidth">Array of Column Width values</param>
        public TableLayout(int columns, params int[] columnWidth) : this()
        {
            // create required no of columns
            for (var cnt = 2; cnt < columns; cnt++) // 2 columns are already there so start from 3rd.
            {
                var cs = new ColumnStyle(SizeType.Absolute);
                tlpTable.ColumnStyles.Add(cs);
                tlpTable.ColumnCount++;
            }

            // set the width of all the columns
            for (var cnt = 0; cnt < tlpTable.ColumnCount; cnt++)
            {
                if (columnWidth.Length > cnt)
                    tlpTable.ColumnStyles[cnt].Width = columnWidth[cnt];
            }

            // remove all the rows
            tlpTable.RowStyles.Clear();
            tlpTable.RowCount = 0;

            // add cellpaint event to change the color of alternate row
            tlpTable.CellPaint += tlpTable_CellPaint;
        }


        /// <summary>
        /// Change the color of alternate row to grey
        /// </summary>
        private void tlpTable_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Row % 2 == 0) 
            { 
                e.Graphics.FillRectangle(new SolidBrush(AlternateRowColor), e.CellBounds); 
            } 
        }


        /// <summary>
        /// Add a new row in the table with specified column values
        /// </summary>
        /// <param name="columnValues">Array of Column values</param>
        public void AddRow(params string[] columnValues)
        {
            // add a new row
            tlpTable.RowStyles.Add(new RowStyle(SizeType.Absolute, RowHeight));
            tlpTable.RowCount++;

            // set the value for each columns of added row
            for (var cnt = 0; cnt < tlpTable.ColumnCount; cnt++)
            {
                var lbl = new Label
                              {
                                  UseMnemonic = false,
                                  BackColor = Color.Transparent,
                                  Font = TextFont,
                                  ForeColor = TextColor,
                                  Text = (columnValues.Length > cnt) ? columnValues[cnt] : string.Empty,
                                  Dock = DockStyle.Fill,
                                  TextAlign = TextAlign
                              };

                tlpTable.Controls.Add(lbl, cnt, tlpTable.RowCount - 1);
            }
        }
    }
}
