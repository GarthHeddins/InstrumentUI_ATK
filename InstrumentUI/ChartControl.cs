using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using InstrumentUI_ATK.Common;
using com.quinncurtis.chart2dnet;

namespace InstrumentUI_ATK
{
    public partial class ChartControl : ChartView
    {
        public TimeSimpleDataset Dataset1;
        public TimeCoordinates pTransform1;
        ChartPrint printobj = null;

        public ChartControl()
        {
            InitializeComponent();
        }

        public void InitializeChart()
        {

        }

        private void ChartControl_Load(object sender, EventArgs e)
        {
            InitializeChart();
        }

        public void UpdateControlChart(ChartCalendar[] xData, double[] yData)
        {
            Dataset1 = new TimeSimpleDataset("Samples", xData, yData);
            ChartView chartVu = this;

            Font theFont;
            theFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);

            chartVu.ResetChartObjectList();


            pTransform1 = new TimeCoordinates();
            pTransform1.AutoScale(Dataset1);
            pTransform1.SetGraphBorderDiagonal(0.15, .15, .90, 0.7);
            Background background = new Background(pTransform1, ChartObj.PLOT_BACKGROUND, Color.White);
            chartVu.AddChartObject(background);
            TimeAxis xAxis = new TimeAxis(pTransform1);
            chartVu.AddChartObject(xAxis);
            LinearAxis yAxis = new LinearAxis(pTransform1, ChartObj.Y_AXIS);
            chartVu.AddChartObject(yAxis);
            TimeAxisLabels xAxisLab = new TimeAxisLabels(xAxis);
            xAxisLab.SetTextFont(theFont);
            chartVu.AddChartObject(xAxisLab);
            NumericAxisLabels yAxisLab = new NumericAxisLabels(yAxis);
            yAxisLab.SetTextFont(theFont);
            chartVu.AddChartObject(yAxisLab);
            Font titleFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            AxisTitle yaxistitle = new AxisTitle(yAxis, titleFont, "Measurement");
            chartVu.AddChartObject(yaxistitle);
            AxisTitle xaxistitle = new AxisTitle(xAxis, titleFont, "Sample Date/Time");
            chartVu.AddChartObject(xaxistitle);
            Grid xgrid = new Grid(xAxis, yAxis, ChartObj.X_AXIS, ChartObj.GRID_MAJOR);
            chartVu.AddChartObject(xgrid);
            Grid ygrid = new Grid(xAxis, yAxis, ChartObj.Y_AXIS, ChartObj.GRID_MAJOR);
            chartVu.AddChartObject(ygrid);
            ChartAttribute attrib1 = new ChartAttribute(Color.Blue, 1, DashStyle.Solid);
            attrib1.SetFillColor(Color.Blue);
            attrib1.SetFillFlag(true);
            attrib1.SetSymbolSize(10);

            ChartAttribute attrib2 = new ChartAttribute(Color.Green, 3, DashStyle.Solid);

            SimpleScatterPlot thePlot2 = new SimpleScatterPlot(pTransform1, Dataset1, ChartObj.CROSS, attrib2);
            chartVu.AddChartObject(thePlot2);

            Font theTitleFont = new Font("Microsoft Sans Serif", 14, FontStyle.Bold);
            ChartTitle mainTitle = new ChartTitle(pTransform1, theTitleFont, "Eurofins QTA");
            mainTitle.SetTitleType(ChartObj.CHART_HEADER);
            mainTitle.SetTitlePosition(ChartObj.CENTER_GRAPH);
            chartVu.AddChartObject(mainTitle);
            GraphicsPath titleLine = new GraphicsPath();
            titleLine.AddLine(0.1f, 0.1f, 0.9f, 0.1f);
            ChartShape titleLineShape = new ChartShape(pTransform1, titleLine, ChartObj.NORM_GRAPH_POS, 0.0, 0.0, ChartObj.NORM_GRAPH_POS, 0);
            titleLineShape.SetLineWidth(3);
            chartVu.AddChartObject(titleLineShape);
            Font theFooterFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            ChartTitle footer = new ChartTitle(pTransform1, theFooterFont, "Trend Graph");
            footer.SetTitleType(ChartObj.CHART_FOOTER);
            footer.SetTitlePosition(ChartObj.CENTER_GRAPH);
            footer.SetTitleOffset(8);
            chartVu.AddChartObject(footer);
            chartVu.SetResizeMode(ChartObj.AUTO_RESIZE_OBJECTS);

            Font legendFont = new Font("SansSerif", 12, FontStyle.Bold);
            ChartAttribute legendAttributes = new ChartAttribute(Color.Black, 1, 0);
            legendAttributes.SetFillFlag(false);
            legendAttributes.SetLineFlag(true);
            StandardLegend legend = new StandardLegend(0.1, .90, 0.3, 0.4, legendAttributes, StandardLegend.HORIZ_DIR);
            String szTrait = Helper.szMaterial + " " + Helper.szTrait;
            legend.AddLegendItem(szTrait, ChartObj.CROSS, thePlot2, legendFont);
            chartVu.AddChartObject(legend);

            ChartZoom zoomObj = new ChartZoom(chartVu, pTransform1, true);
            zoomObj.SetButtonMask(MouseButtons.Left);
            zoomObj.SetZoomYEnable(true);
            zoomObj.SetZoomXEnable(true);
            zoomObj.SetZoomXRoundMode(ChartObj.AUTOAXES_FAR);
            zoomObj.SetZoomYRoundMode(ChartObj.AUTOAXES_FAR);
            zoomObj.SetEnable(true);
            zoomObj.SetZoomStackEnable(true);
            // set range limits to 1000 ms, 1 degree
            //zoomObj.SetZoomRangeLimitsRatio(new Dimension(1.0, 1.0));
            zoomObj.InternalZoomStackProcesssing = true;

            Font toolTipFont = new Font("SansSerif", 10, FontStyle.Regular);
            DataToolTip datatooltip = new DataToolTip(chartVu);
            TimeLabel xValueTemplate = new TimeLabel(ChartObj.TIMEDATEFORMAT_MDY_HM);
            NumericLabel yValueTemplate = new NumericLabel(ChartObj.DECIMALFORMAT, 2);
            ChartText textTemplate = new ChartText(toolTipFont, "");
            textTemplate.SetTextBgColor(Color.FromArgb(255, 255, 204));
            textTemplate.SetTextBgMode(true);
            ChartSymbol toolTipSymbol = new ChartSymbol(null, ChartObj.SQUARE, new ChartAttribute(Color.Black));
            toolTipSymbol.SetSymbolSize(5.0);
            datatooltip.SetTextTemplate(textTemplate);
            datatooltip.SetXValueTemplate(xValueTemplate);
            datatooltip.SetYValueTemplate(yValueTemplate);
            datatooltip.SetDataToolTipFormat(ChartObj.DATA_TOOLTIP_XY_ONELINE);
            datatooltip.SetToolTipSymbol(toolTipSymbol);
            datatooltip.SetEnable(true);


            chartVu.SetCurrentMouseListener(datatooltip);
            chartVu.SetCurrentMouseListener(zoomObj);

            chartVu.UpdateDraw();
        }

        public void PageSetup(object sender, System.EventArgs e)
        {
            ChartView chartVu = this;
            if (chartVu != null)
            {
                if (printobj == null)
                {
                    printobj = new ChartPrint(chartVu);
                }
                else
                    printobj.PrintChartView = chartVu;
                printobj.PageSetupItem(sender, e);
            }
        }

        // This routine invokes the chart objects printer setup dialog method
        public void PrinterSetup(object sender, System.EventArgs e)
        {
            ChartView chartVu = this;
            if (chartVu != null)
            {
                if (printobj == null)
                {
                    printobj = new ChartPrint(chartVu);
                }
                else
                    printobj.PrintChartView = chartVu;
                printobj.DoPrintDialog();
            }
        }

        // This routine invokes the chart objects PrintPreviewItem method
        public void PrintPreview(object sender, System.EventArgs e)
        {
            ChartView chartVu = this;
            if (chartVu != null)
            {
                if (printobj == null)
                {
                    printobj = new ChartPrint(chartVu);
                }
                else
                    printobj.PrintChartView = chartVu;
                printobj.PrintPreviewItem(sender, e);
            }
        }

        // This routine prints a chart by invoking the chart objects DocPrintPage method
        public void PrintPage(object sender, System.EventArgs e)
        {
            ChartView chartVu = this;
            if (chartVu != null)
            {
                if (printobj == null)
                {
                    printobj = new ChartPrint(chartVu);
                    printobj.DoPrintDialog();
                }
                else
                    printobj.PrintChartView = chartVu;
                printobj.DocPrintPage(sender, e);
            }
        }

        public void SaveAsFile(object sender, System.EventArgs e)
        {
            ChartView chartview = this;
            String filename = this.Name;
            SaveFileDialog imagefilechooser = new SaveFileDialog();
            imagefilechooser.Filter =
                "Image Files(*.BMP;*.JPG;*.GIF;*.TIFF;*.PNG)|*.BMP;*.JPG;*.GIF;*.TIFF;*.PNG|All files (*.*)|*.*";
            imagefilechooser.FileName = filename;
            if (imagefilechooser.ShowDialog() == DialogResult.OK)
            {
                filename = imagefilechooser.FileName;
                FileInfo fileinformation = new FileInfo(filename);
                String fileext = fileinformation.Extension;
                fileext = fileext.ToUpper();
                ImageFormat fileimageformat;
                if (fileext == ".BMP")
                    fileimageformat = ImageFormat.Bmp;
                else if ((fileext == ".JPG") || (fileext == ".JPEG"))
                    fileimageformat = ImageFormat.Jpeg;
                else if ((fileext == ".GIF"))
                    fileimageformat = ImageFormat.Gif;
                else if ((fileext == ".TIF") || (fileext == ".TIFF"))
                    fileimageformat = ImageFormat.Tiff;
                else if ((fileext == ".PNG"))
                    fileimageformat = ImageFormat.Png;
                else
                    fileimageformat = ImageFormat.Bmp;

                BufferedImage savegraph = new BufferedImage(chartview, fileimageformat);
                savegraph.Render();
                savegraph.SaveImage(filename);
            }
        }

    }
}
