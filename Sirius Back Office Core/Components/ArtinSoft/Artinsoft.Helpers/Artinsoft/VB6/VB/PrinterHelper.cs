using System;
using Microsoft.VisualBasic.Compatibility.VB6;
using System.Reflection;
using System.Collections.Generic;
using System.Management;
using System.Drawing;
using System.Drawing.Printing;

namespace Artinsoft.VB6.VB
{
    /// <summary>
    /// Class to emulate the VB6 object printer.
    /// All internal lengths and sizes will be stored in the current unit of measurement defined by either
    /// ScaleHeight, ScaleWidth, ScaleLeft, ScaleTop, ScaleMode
    /// </summary>
    public class PrinterHelper
    {
        /// <summary>
        /// Private static property DISPLAY_DPI
        /// </summary>
        private static float DISPLAY_DPI = 0;

        /// <summary>
        /// Printer static property.
        /// </summary>
        public static PrinterHelper Printer = new PrinterHelper();

        /// <summary>
        /// _Printers private member, it manages printers list (VB6 collection)
        /// </summary>
        private static List<PrinterHelper> _Printers = null;
        /// <summary>
        /// Printers list object will emulate VB6 printers collection
        /// </summary>
        public static List<PrinterHelper> Printers
        {
            get
            {
                if (_Printers == null)
                {
                    _Printers = new List<PrinterHelper>();
                }

                foreach (string printerName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    _Printers.Add(new PrinterHelper(printerName));
                }

                return _Printers;
            }
        }

        /// <summary>
        /// FillStyle Constants Enum.
        /// </summary>
        private enum FillStyleConstants : short
        {
            vbFSSolid = 0,
            vbFSTransparent = 1
        }

        /// <summary>
        /// ScaleMode Constants Enum.
        /// </summary>
        private enum ScaleModeConstants : short
        {
            vbCentimeters = 7,
            vbCharacters = 4,
            vbContainerPosition = 9,
            vbContainerSize = 10,
            vbHimetric = 8,
            vbInches = 5,
            vbMilimeters = 6,
            vbPixels = 3,
            vbPoints = 2,
            vbTwips = 1,
            vbUser = 0
        }

        /// <summary>
        /// DrawMode Constants Enum.
        /// </summary>
        private enum DrawModeConstants : int
        {
            vbBlackness = 1,
            vbNotMergePen = 2,
            vbMaskNotPen = 3,
            vbNotCopyPen = 4,
            vbMaskPenNot = 5,
            vbInvert = 6,
            vbXorPen = 7,
            vbNotMaskPen = 8,
            vbMaskPen = 9,
            vbNotXorPen = 10,
            vbNop = 11,
            vbMergeNotPen = 12,
            vbCopyPen = 13,
            vbMergePenNot = 14,
            vbMergePen = 15,
            vbWhiteness = 16
        }

        /// <summary>
        /// PrinterObject Constants Class.
        /// </summary>
        public static class PrinterObjectConstants
        {
            /// <summary>
            /// Upper
            /// </summary>
            public static readonly short vbPRBNUpper = 1;
            /// <summary>
            /// Lower
            /// </summary>
            public static readonly short vbPRBNLower = 2;
            /// <summary>
            /// Middle
            /// </summary>
            public static readonly short vbPRBNMiddle = 3;
            /// <summary>
            /// Manual
            /// </summary>
            public static readonly short vbPRBNManual = 4;
            /// <summary>
            /// Envelope
            /// </summary>
            public static readonly short vbPRBNEnvelope = 5;
            /// <summary>
            /// Envelope Manual
            /// </summary>
            public static readonly short vbPRBNEnvManual = 6;
            /// <summary>
            /// Auto
            /// </summary>
            public static readonly short vbPRBNAuto = 7;
            /// <summary>
            /// Tractor
            /// </summary>
            public static readonly short vbPRBNTractor = 8;
            /// <summary>
            /// Small Format
            /// </summary>
            public static readonly short vbPRBNSmallFmt = 9;
            /// <summary>
            /// Large Format
            /// </summary>
            public static readonly short vbPRBNLargeFmt = 10;
            /// <summary>
            /// Large Capacity
            /// </summary>
            public static readonly short vbPRBNLargeCapacity = 11;
            /// <summary>
            /// Cassette
            /// </summary>
            public static readonly short vbPRBNCassette = 14;
            /// <summary>
            /// Monochrome
            /// </summary>
            public static readonly short vbPRCMMonochrome = 1;
            /// <summary>
            /// Color
            /// </summary>
            public static readonly short vbPRCMColor = 2;
            /// <summary>
            /// Simplex
            /// </summary>
            public static readonly short vbPRDPSimplex = 1;
            /// <summary>
            /// Horizontal
            /// </summary>
            public static readonly short vbPRDPHorizontal = 2;
            /// <summary>
            /// Vertical
            /// </summary>
            public static readonly short vbPRDPVertical = 3;
            /// <summary>
            /// Portrait
            /// </summary>
            public static readonly short vbPRORPortrait = 1;
            /// <summary>
            /// Landscape
            /// </summary>
            public static readonly short vbPRORLandscape = 2;
            /// <summary>
            /// Draft
            /// </summary>
            public static readonly short vbPRPQDraft = -1;
            /// <summary>
            /// Low
            /// </summary>
            public static readonly short vbPRPQLow = -2;
            /// <summary>
            /// Medium
            /// </summary>
            public static readonly short vbPRPQMedium = -3;
            /// <summary>
            /// High
            /// </summary>
            public static readonly short vbPRPQHigh = -4;
            /// <summary>
            /// Letter
            /// </summary>
            public static readonly short vbPRPSLetter = 1;
            /// <summary>
            /// LetterSmall
            /// </summary>
            public static readonly short vbPRPSLetterSmall = 2;
            /// <summary>
            /// Tabloid
            /// </summary>
            public static readonly short vbPRPSTabloid = 3;
            /// <summary>
            /// Ledger
            /// </summary>
            public static readonly short vbPRPSLedger = 4;
            /// <summary>
            /// Legal
            /// </summary>
            public static readonly short vbPRPSLegal = 5;
            /// <summary>
            /// Statement
            /// </summary>
            public static readonly short vbPRPSStatement = 6;
            /// <summary>
            /// Executive
            /// </summary>
            public static readonly short vbPRPSExecutive = 7;
            /// <summary>
            /// A3
            /// </summary>
            public static readonly short vbPRPSA3 = 8;
            /// <summary>
            /// A4
            /// </summary>
            public static readonly short vbPRPSA4 = 9;
            /// <summary>
            /// A4 Small
            /// </summary>
            public static readonly short vbPRPSA4Small = 10;
            /// <summary>
            /// A5
            /// </summary>
            public static readonly short vbPRPSA5 = 11;
            /// <summary>
            /// B4
            /// </summary>
            public static readonly short vbPRPSB4 = 12;
            /// <summary>
            /// B5
            /// </summary>
            public static readonly short vbPRPSB5 = 13;
            /// <summary>
            /// Folio
            /// </summary>
            public static readonly short vbPRPSFolio = 14;
            /// <summary>
            /// Quarto
            /// </summary>
            public static readonly short vbPRPSQuarto = 15;
            /// <summary>
            /// 10x14
            /// </summary>
            public static readonly short vbPRPS10x14 = 16;
            /// <summary>
            /// 11x17
            /// </summary>
            public static readonly short vbPRPS11x17 = 17;
            /// <summary>
            /// Note
            /// </summary>
            public static readonly short vbPRPSNote = 18;
            /// <summary>
            /// Envelope 9
            /// </summary>
            public static readonly short vbPRPSEnv9 = 19;
            /// <summary>
            /// Envelope 10
            /// </summary>
            public static readonly short vbPRPSEnv10 = 20;
            /// <summary>
            /// Envelope 11
            /// </summary>
            public static readonly short vbPRPSEnv11 = 21;
            /// <summary>
            /// Envelope 12
            /// </summary>
            public static readonly short vbPRPSEnv12 = 22;
            /// <summary>
            /// Envelope 14
            /// </summary>
            public static readonly short vbPRPSEnv14 = 23;
            /// <summary>
            /// SC Sheet
            /// </summary>
            public static readonly short vbPRPSCSheet = 24;
            /// <summary>
            /// SD Sheet
            /// </summary>
            public static readonly short vbPRPSDSheet = 25;
            /// <summary>
            /// SE Sheet
            /// </summary>
            public static readonly short vbPRPSESheet = 26;
            /// <summary>
            /// Envelope DL
            /// </summary>
            public static readonly short vbPRPSEnvDL = 27;
            /// <summary>
            /// Envelope C5
            /// </summary>
            public static readonly short vbPRPSEnvC5 = 28;
            /// <summary>
            /// Envelope C3
            /// </summary>
            public static readonly short vbPRPSEnvC3 = 29;
            /// <summary>
            /// Envelope C4
            /// </summary>
            public static readonly short vbPRPSEnvC4 = 30;
            /// <summary>
            /// Envelope C6
            /// </summary>
            public static readonly short vbPRPSEnvC6 = 31;
            /// <summary>
            /// Envelope C65
            /// </summary>
            public static readonly short vbPRPSEnvC65 = 32;
            /// <summary>
            /// Envelope B4
            /// </summary>
            public static readonly short vbPRPSEnvB4 = 33;
            /// <summary>
            /// Envelope B5
            /// </summary>
            public static readonly short vbPRPSEnvB5 = 34;
            /// <summary>
            /// Envelope B6
            /// </summary>
            public static readonly short vbPRPSEnvB6 = 35;
            /// <summary>
            /// Envelope Italy
            /// </summary>
            public static readonly short vbPRPSEnvItaly = 36;
            /// <summary>
            /// Envelope Monarch
            /// </summary>
            public static readonly short vbPRPSEnvMonarch = 37;
            /// <summary>
            /// Envelope Personal
            /// </summary>
            public static readonly short vbPRPSEnvPersonal = 38;
            /// <summary>
            /// Fanfold US
            /// </summary>
            public static readonly short vbPRPSFanfoldUS = 39;
            /// <summary>
            /// Fanfold Standard German
            /// </summary>
            public static readonly short vbPRPSFanfoldStdGerman = 40;
            /// <summary>
            /// Fanfold Legal German
            /// </summary>
            public static readonly short vbPRPSFanfoldLglGerman = 41;
            /// <summary>
            /// User
            /// </summary>
            public static readonly short vbPRPSUser = 256;
        }

        /// <summary>
        /// TabSize : Constants for internal use (pixels units).
        /// </summary>
        private const int TabSize = 66;  //Mesured in Pixels

        /// <summary>
        /// The column size for the function Tab given in the internal unit given by ScaleMode.
        /// </summary>
        private float ColumnSize
        {
            get
            {
                System.Drawing.SizeF size;
                //El tamaño de una columna para ser utilizado por la funcion Tab deberia ser el promedio de todos los caracteres
                //para el font actual, pero no se ha podido emular el mismo valor que utiliza VB6, asi que se utiliza
                //el string de abajo para hacer un calculo aproximado
                size = System.Windows.Forms.TextRenderer.MeasureText(PrinterGraphics, "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", Font);
                return (float)ConvertFromPixelsX(size.Width / 100);
            }
        }

        /// <summary>
        /// The maximum amount of available columns which can be printer in a page.
        /// </summary>
        private int MaxColumnAvailables
        {
            get
            {
                return (int)(ConvertFromPixelsY(ConvertToPixelsY(Width, ScaleModeConstants.vbTwips)) / ColumnSize);
            }
        }

        // Misc struture definitions for internal use purposes

        /// <summary>
        /// DataInfo structure.
        /// </summary>
        private struct DataInfo
        {
            public string value;
            public System.Drawing.Font font;
            public float X;
            public float Y;
        }

        /// <summary>
        /// LineInfo structure.
        /// </summary>
        private struct LineInfo
        {
            public System.Drawing.Pen pen;
            public System.Drawing.PointF p1;
            public System.Drawing.PointF p2;
        }

        /// <summary>
        /// RectangleInfo structure.
        /// </summary>
        private struct RectangleInfo
        {
            public System.Drawing.Pen pen;
            public System.Drawing.RectangleF rec;
            public FillStyleConstants FillStyle;
            public System.Drawing.Color FillColor;
        }

        /// <summary>
        /// ImageInfo structure.
        /// </summary>
        private struct ImageInfo
        {
            public System.Drawing.PointF p;
            public System.Drawing.Image picture;
        }

        /// <summary>
        /// PageInfo structure.
        /// </summary>
        private struct PageInfo
        {
            private bool _Dirty;
            public bool Dirty
            {
                get { return _Dirty; }
                set
                {
                    _Dirty = value;
                }
            }

            /// <summary>
            /// _Data private member.
            /// </summary>
            private DataInfo[] _Data;

            /// <summary>
            /// Data list object.
            /// </summary>
            public DataInfo[] Data
            {
                get
                {
                    return _Data;
                }
                set
                {
                    Dirty = true;
                    _Data = value;
                }
            }

            /// <summary>
            /// _Lines private member.
            /// </summary>
            private LineInfo[] _Lines;

            /// <summary>
            /// Lines list object.
            /// </summary>
            public LineInfo[] Lines
            {
                get { return _Lines; }
                set
                {
                    Dirty = true;
                    _Lines = value;
                }
            }

            /// <summary>
            /// _Circles private member.
            /// </summary>
            private RectangleInfo[] _Circles;

            /// <summary>
            /// Circles list object.
            /// </summary>
            public RectangleInfo[] Circles
            {
                get { return _Circles; }
                set
                {
                    Dirty = true;
                    _Circles = value;
                }
            }

            /// <summary>
            /// _Rectangles private member.
            /// </summary>
            private RectangleInfo[] _Rectangles;

            /// <summary>
            /// Rectangles list object.
            /// </summary>
            public RectangleInfo[] Rectangles
            {
                get { return _Rectangles; }
                set
                {
                    Dirty = true;
                    _Rectangles = value;
                }
            }

            /// <summary>
            /// _Images private member.
            /// </summary>
            private ImageInfo[] _Images;

            /// <summary>
            /// Images list object.
            /// </summary>
            public ImageInfo[] Images
            {
                get { return _Images; }
                set
                {
                    Dirty = true;
                    _Images = value;
                }
            }
        }

        /// <summary>
        /// Pages private member, list of PageInfo.
        /// </summary>
        private PageInfo[] Pages;

        /// <summary>
        /// objPen private member, Pen used to draw.
        /// </summary>
        private System.Drawing.Pen objPen = new System.Drawing.Pen(System.Drawing.Brushes.Black);

        /// <summary>
        /// brush private member, brush used to draw.
        /// </summary>
        private System.Drawing.Brush brush = null;

        private int PageIndex = 0;

        /// <summary>
        /// Utility function to indicate if a printer is local or networked. This function will fail in Windows 2000
        /// and Windows NT 4.0 as the properties Local, Network are not supported in those systems.
        /// </summary>
        /// <param name="DeviceName">The name of the printer</param>
        /// <returns>true if the printer is local</returns>
        public static bool isLocalPrinter(string DeviceName)
        {
            bool local, network;

            try
            {
                local = network = false;
                ManagementObjectSearcher query;
                ManagementObjectCollection queryCollection;
                string queryString = "SELECT Local, Network FROM Win32_Printer WHERE Name=\"" + DeviceName.Replace("\\", "\\\\") + "\"";
                query = new ManagementObjectSearcher(queryString);
                queryCollection = query.Get();
                foreach (ManagementObject mo in queryCollection)
                {
                    local = Convert.ToBoolean(mo["Local"]);
                    network = Convert.ToBoolean(mo["Network"]);

                    return (local && !network);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception thrown while getting information for the printer. " +
                    "The current implementation is not supported in Windows 2000 and Windows NT. " + e.Message);
            }
            return false;
        }

        /// <summary>
        /// InnerPrinter private member.
        /// </summary>
        private System.Drawing.Printing.PrintDocument InnerPrinter = new System.Drawing.Printing.PrintDocument();

        /// <summary>
        /// _Font private member.
        /// </summary>
        private System.Drawing.Font _Font = new System.Drawing.Font("Arial", 8.28f);

        /// <summary>
        /// Font property.
        /// </summary>
        public System.Drawing.Font Font
        {
            get { return _Font; }
            set
            {
                _Font = value;
                _InternalTextHeight = -1;
            }
        }

        /// <summary>
        /// FillColor property.
        /// </summary>
        public System.Drawing.Color FillColor;

        /// <summary>
        /// _FillStyle private member.
        /// </summary>
        private FillStyleConstants _FillStyle = FillStyleConstants.vbFSTransparent;

        /// <summary>
        /// FillStyle property (FillStyleConstants enum).
        /// </summary>
        public int FillStyle
        {
            get
            {
                return (int)_FillStyle;
            }
            set
            {
                _FillStyle = (value == 0) ? FillStyleConstants.vbFSSolid : FillStyleConstants.vbFSTransparent;
            }
        }

        /// <summary>
        /// _ScaleMode private member.
        /// </summary>
        private ScaleModeConstants _ScaleMode = ScaleModeConstants.vbTwips;

        /// <summary>
        /// ScaleMode property (ScaleModeConstants enum).
        /// </summary>
        public int ScaleMode
        {
            get
            {
                return (int)_ScaleMode;
            }
            set
            {
                switch (value)
                {
                    case 0:
                        _ScaleMode = ScaleModeConstants.vbUser;
                        break;
                    case 2:
                        _ScaleMode = ScaleModeConstants.vbPoints;
                        break;
                    case 3:
                        _ScaleMode = ScaleModeConstants.vbPixels;
                        break;
                    case 4:
                        _ScaleMode = ScaleModeConstants.vbCharacters;
                        break;
                    case 5:
                        _ScaleMode = ScaleModeConstants.vbInches;
                        break;
                    case 6:
                        _ScaleMode = ScaleModeConstants.vbMilimeters;
                        break;
                    case 7:
                        _ScaleMode = ScaleModeConstants.vbCentimeters;
                        break;
                    case 8:
                        _ScaleMode = ScaleModeConstants.vbHimetric;
                        break;
                    case 9:
                        _ScaleMode = ScaleModeConstants.vbContainerPosition;
                        break;
                    case 10:
                        _ScaleMode = ScaleModeConstants.vbContainerSize;
                        break;
                    default:
                        _ScaleMode = ScaleModeConstants.vbTwips;
                        break;
                }
            }
        }

        /// <summary>
        /// _DriverName private member.
        /// </summary>
        private string _DriverName = string.Empty;

        /// <summary>
        /// DriverName property.
        /// </summary>
        public string DriverName
        {
            get { return _DriverName; }
        }

        /// <summary>
        /// Private misc function which returns driver name it is used to set DriverName property value.
        /// </summary>
        /// <returns>Driver Name</returns>
        private string getDriverName()
        {
            string driverName = string.Empty;
            try
            {
                ManagementObjectSearcher query;
                ManagementObjectCollection queryCollection;
                string queryString = "SELECT DriverName FROM Win32_Printer WHERE Name=\"" + DeviceName.Replace("\\", "\\\\") + "\"";
                query = new ManagementObjectSearcher(queryString);
                queryCollection = query.Get();
                foreach (ManagementObject mo in queryCollection)
                {
                    driverName = mo["DriverName"] as string;
                }

            }
            catch (Exception)
            {
            }
            return driverName;
        }

        /// <summary>
        /// _Port private member.
        /// </summary>
        private string _Port = string.Empty;

        /// <summary>
        /// Port property
        /// </summary>
        public string Port
        {
            get
            {
                return _Port;
            }
        }

        /// <summary>
        /// Function to return the port of the current printer.
        /// </summary>
        /// <returns>Port Name</returns>
        private string getPort()
        {
            string portName = string.Empty;
            try
            {
                ManagementObjectSearcher query;
                ManagementObjectCollection queryCollection;
                string queryString = "SELECT PortName FROM Win32_Printer WHERE Name=\"" + DeviceName.Replace("\\", "\\\\") + "\"";
                query = new ManagementObjectSearcher(queryString);
                queryCollection = query.Get();
                foreach (ManagementObject mo in queryCollection)
                {
                    if (!string.IsNullOrEmpty(portName))
                        portName += ", ";

                    portName += mo["PortName"] as string;
                }

            }
            catch (Exception)
            {
            }
            return portName;
        }

        /// <summary>
        /// Utility property to indicate if the current printer is local or networked.
        /// </summary>
        public bool isLocal
        {
            get
            {
                return isLocalPrinter(DeviceName);
            }
        }

        /// <summary>
        /// FontTransparent property.
        /// </summary>
        public bool FontTransparent = false;

        /// <summary>
        /// PrinterHelper Constructor.
        /// </summary>
        public PrinterHelper()
        {
            Pages = new PageInfo[1];
            Pages[0] = new PageInfo();
            _Port = getPort();
            _DriverName = getDriverName();
            InnerPrinter.PrintController = new System.Drawing.Printing.StandardPrintController();
            InnerPrinter.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(Printer_PrintPage);
        }

        /// <summary>
        /// PrinterHelper static Constructor.
        /// </summary>
        static PrinterHelper()
        {
            System.Windows.Forms.Control ctrl = new System.Windows.Forms.Control();
            Graphics g = Graphics.FromHwnd(ctrl.Handle);
            DISPLAY_DPI = g.DpiX;
        }

        /// <summary>
        /// PrinterHelper private Constructor.
        /// </summary>
        private PrinterHelper(string PrinterName)
        {
            Pages = new PageInfo[1];
            Pages[0] = new PageInfo();
            _Port = getPort();
            _DriverName = getDriverName();
            InnerPrinter.PrinterSettings.PrinterName = PrinterName;
            InnerPrinter.PrintController = new System.Drawing.Printing.StandardPrintController();
            InnerPrinter.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(Printer_PrintPage);
        }

        /// <summary>
        /// _DrawMode private property.
        /// </summary>
        private DrawModeConstants _DrawMode = DrawModeConstants.vbBlackness;

        /// <summary>
        /// DrawMode: Sets the appearance for output from graphics methods, or for a Shape or a Line control. (DrawModeConstants).
        /// </summary>
        public int DrawMode
        {
            get
            {
                return (int)_DrawMode;
            }
            set
            {
                switch (value)
                {
                    case 1:
                        _DrawMode = DrawModeConstants.vbBlackness;
                        break;
                    case 2:
                        _DrawMode = DrawModeConstants.vbNotMergePen;
                        break;
                    case 3:
                        _DrawMode = DrawModeConstants.vbMaskNotPen;
                        break;
                    case 4:
                        _DrawMode = DrawModeConstants.vbNotCopyPen;
                        break;
                    case 5:
                        _DrawMode = DrawModeConstants.vbMaskPenNot;
                        break;
                    case 6:
                        _DrawMode = DrawModeConstants.vbInvert;
                        break;
                    case 7:
                        _DrawMode = DrawModeConstants.vbXorPen;
                        break;
                    case 8:
                        _DrawMode = DrawModeConstants.vbNotMaskPen;
                        break;
                    case 9:
                        _DrawMode = DrawModeConstants.vbMaskPen;
                        break;
                    case 10:
                        _DrawMode = DrawModeConstants.vbNotXorPen;
                        break;
                    case 11:
                        _DrawMode = DrawModeConstants.vbNop;
                        break;
                    case 12:
                        _DrawMode = DrawModeConstants.vbMergeNotPen;
                        break;
                    case 13:
                        _DrawMode = DrawModeConstants.vbCopyPen;
                        break;
                    case 14:
                        _DrawMode = DrawModeConstants.vbMergePenNot;
                        break;
                    case 15:
                        _DrawMode = DrawModeConstants.vbMergePen;
                        break;
                    case 16:
                        _DrawMode = DrawModeConstants.vbWhiteness;
                        break;
                    default:
                        _DrawMode = DrawModeConstants.vbBlackness;
                        break;
                }
            }
        }

        //  CIRCLE METHODS ///

        /// <summary>
        /// Circle method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Circle(System.Drawing.Point P, int radius)
        {
            Circle(P, radius, false);
        }

        /// <summary>
        /// Circle method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Circle(System.Drawing.Point P, int radius, bool Step)
        {
            Circle(P, radius, objPen.Color, Step);
        }

        /// <summary>
        /// Circle method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Circle(System.Drawing.Point P, int radius, System.Drawing.Color Color)
        {
            Circle(P, radius, Color, false);
        }

        /// <summary>
        /// Circle method.
        /// </summary>
        /// <param name="P">PoingF</param>
        /// <param name="radius">int radius</param>
        /// <param name="Color">Circle color</param>
        /// <param name="Step"></param>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Circle(System.Drawing.Point P, int radius, System.Drawing.Color Color, bool Step)
        {
            PointF newP = new PointF(P.X, P.Y);
            Circle(newP, radius, Color, Step);
        }

        /// <summary>
        /// Circle method.
        /// P and radius are given in the unit specified by Scalemode, step is false.
        /// </summary>
        /// <param name="P">PointF</param>
        /// <param name="radius">double</param>
        public void Circle(System.Drawing.PointF P, double radius)
        {
            Circle(P, radius, false);
        }

        /// <summary>
        /// Circle method.
        /// P, radius and step are given in the unit specified by Scalemode.
        /// </summary>
        /// <param name="P"></param>
        /// <param name="radius"></param>
        /// <param name="Step"></param>
        public void Circle(System.Drawing.PointF P, double radius, bool Step)
        {
            Circle(P, radius, objPen.Color, Step);
        }

        /// <summary>
        /// Circle method.
        /// P, radius and step are given in the unit specified by Scalemode.
        /// </summary>
        /// <param name="P"></param>
        /// <param name="radius"></param>
        /// <param name="Color"></param>
        public void Circle(System.Drawing.PointF P, double radius, System.Drawing.Color Color)
        {
            Circle(P, radius, Color, false);
        }

        /// <summary>
        /// Circle method
        /// P and radius are given in the unit specified by Scalemode
        /// </summary>
        public void Circle(System.Drawing.PointF P, double radius, System.Drawing.Color Color, bool Step)
        {
            double diameter = 0;

            if (Step == true)
            {
                P.X = P.X + (float)CurrentX;
                P.Y = P.Y + (float)CurrentY;
            }
            diameter = radius * 2;

            // Moves the CurrentX and CurrentY properties
            CurrentX = P.X + radius;
            CurrentY = P.Y + radius;

            RectangleInfo[] tmpCircles = Pages[PageIndex].Circles;
            if ((Pages[PageIndex].Circles == null))
                Array.Resize(ref tmpCircles, 1);
            else
                Array.Resize(ref tmpCircles, Pages[PageIndex].Circles.Length + 1);
            Pages[PageIndex].Circles = tmpCircles;

            //The final values are stored in pixels
            RectangleF circleRec = new RectangleF((float)ConvertToPrinterUnitsX(P.X), (float)ConvertToPrinterUnitsY(P.Y), (float)ConvertToPrinterUnitsX(diameter), (float)ConvertToPrinterUnitsY(diameter));
            Pages[PageIndex].Circles[Pages[PageIndex].Circles.Length - 1].rec = circleRec;
            Pages[PageIndex].Circles[Pages[PageIndex].Circles.Length - 1].pen = objPen;
            Pages[PageIndex].Circles[Pages[PageIndex].Circles.Length - 1].FillColor = FillColor;
            Pages[PageIndex].Circles[Pages[PageIndex].Circles.Length - 1].FillStyle = _FillStyle;

        }


        //  CIRCLE METHODS



        //vbPRCMMonochrome = 1, vbPRCMColor = 2
        /// <summary>
        /// ColorMode property
        /// Values are monochrome or color
        /// </summary>
        public int ColorMode
        {
            get
            {
                return (InnerPrinter.PrinterSettings.DefaultPageSettings.Color) ? 2 : 1;
            }
            set
            {
                InnerPrinter.PrinterSettings.DefaultPageSettings.Color = (value != 1);
            }
        }

        /// <summary>
        /// Copies property. Number of copies to be printed.
        /// </summary>
        public int Copies
        {
            get
            {
                return InnerPrinter.PrinterSettings.Copies;
            }
            set
            {
                InnerPrinter.PrinterSettings.Copies = (short)value;
            }
        }

        /// <summary>
        /// _CurrentX private member
        /// </summary>
        private double _CurrentX = 0;

        /// <summary>
        /// CurrentX property.
        /// Returns or set the horizontal coordinate for the next printing or drawing method.
        /// Coordinates are expressed in the current unit of measurement defined by ScaleHeight, ScaleWidth, ScaleLeft,
        /// ScaleTop and ScaleMode.
        /// </summary>
        public double CurrentX
        {
            get
            {
                return _CurrentX;
            }
            set
            {
                _CurrentX = value;
            }
        }

        /// <summary>
        /// _CurrentY private member
        /// </summary>
        /// 
        private double _CurrentY = 0;
        /// <summary>
        /// CurrentY property.
        /// Returns or set the vertical coordinate for the next printing or drawing method.
        /// Coordinates are expressed in the current unit of measurement defined by ScaleHeight, ScaleWidth, ScaleLeft,
        /// ScaleTop and ScaleMode.
        /// </summary>
        public double CurrentY
        {
            get
            {
                return _CurrentY;
            }
            set
            {
                _CurrentY = value;
            }
        }

        /// <summary>
        /// DeviceName property (printer name).
        /// </summary>
        public string DeviceName
        {
            get
            {
                return InnerPrinter.PrinterSettings.PrinterName;
            }
        }

        /// <summary>
        /// _DrawStyle private member.
        /// </summary>
        private int _DrawStyle = 0;

        /// <summary>
        /// DrawStyle property (System.Drawing.Drawing2D.DashStyle).
        /// </summary>
        public int DrawStyle
        {
            get
            {
                return _DrawStyle;
            }
            set
            {
                _DrawStyle = value;
                switch (value)
                {
                    case 0:
                        objPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        break;
                    case 1:
                        objPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        break;
                    case 2:
                        objPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        break;
                    case 3:
                        objPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                        break;
                    case 4:
                        objPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                        break;
                    case 5:
                        objPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
                        break;
                    default:
                        objPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        _DrawStyle = 0;
                        break;
                }

            }
        }

        /// <summary>
        /// DrawWidth property
        /// the pencil width value.
        /// </summary>
        public int DrawWidth
        {
            get
            {
                return (int)objPen.Width;
            }
            set
            {
                objPen.Width = (float)value;
            }
        }
        
        /***********************************************************
         * properties/methods pending to be implemented
        ************************************************************/

        /// <summary>
        /// KillDoc method -- pending to be implemented
        /// cancels a print.
        /// </summary>
        public void KillDoc()
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// PaperBin property -- pending to be implemented.
        /// Returns/sets the paper size for the current printer.
        /// </summary>
        public int PaperBin
        {
            get
            {
                throw new System.Exception("Method or Property not implemented yet!");
            }
            set
            {
                throw new System.Exception("Method or Property not implemented yet!");
            }
        }

        /// <summary>
        /// PSet method -- pending to be implemented.
        /// Sets a point on an object to a specified color.
        /// </summary>
        public void PSet(System.Drawing.Point p, System.Drawing.Color color, int step)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// PSet method -- pending to be implemented
        /// Sets a point on an object to a specified color.
        /// </summary>
        public void PSet(System.Drawing.Point p, System.Drawing.Color color)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// PSet method -- pending to be implemented.
        /// Sets a point on an object to a specified color.
        /// </summary>
        public void PSet(System.Drawing.Point p)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// Scale method -- pending to be implemented
        /// Defines the coordinate system for a Form, PictureBox, or Printer.
        /// </summary>
        public void Scale(System.Drawing.Point p1, System.Drawing.Point p2, int flags)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// Scale method -- pending to be implemented.
        /// Defines the coordinate system for a Form, PictureBox, or Printer.
        /// </summary>
        public void Scale(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// Scale method -- pending to be implemented.
        /// Defines the coordinate system for a Form, PictureBox, or Printer.
        /// </summary>
        public void Scale()
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// RightToLeft property -- pending to be implemented.
        /// Returns a boolean value indicating text display direction and control visual appearance on a bidirectional system.
        /// </summary>
        public bool RightToLeft
        {
            get
            {
                throw new System.Exception("Method or Property not implemented yet!");
            }
            set
            {
                throw new System.Exception("Method or Property not implemented yet!");
            }
        }

        /// <summary>
        /// ScaleX method -- pending to be implemented.
        /// Converts the value for the width of a Form, PictureBox, or Printer from one unit of measure to another.
        /// </summary>
        public float ScaleX(float width, object fromScale, object toScale)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// ScaleX method -- pending to be implemented.
        /// Converts the value for the width of a Form, PictureBox, or Printer from one unit of measure to another.
        /// </summary>
        public float ScaleX(float width, object fromScale)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// ScaleX method -- pending to be implemented.
        /// Converts the value for the width of a Form, PictureBox, or Printer from one unit of measure to another.
        /// </summary>
        public float ScaleX(float width)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// ScaleY method -- pending to be implemented.
        /// Converts the value for the width of a Form, PictureBox, or Printer from one unit of measure to another.
        /// </summary>
        public float ScaleY(float height, object fromScale, object toScale)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// ScaleY method -- pending to be implemented.
        /// Converts the value for the width of a Form, PictureBox, or Printer from one unit of measure to another.
        /// </summary>
        public float ScaleY(float height, object fromScale)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// ScaleY method -- pending to be implemented.
        /// Converts the value for the width of a Form, PictureBox, or Printer from one unit of measure to another.
        /// </summary>
        public float ScaleY(float height)
        {
            throw new System.Exception("Method or Property not implemented yet!");
        }

        /// <summary>
        /// TrackDefault property -- pending to be implemented.
        /// Returns/sets a value that determines if the Printer object considers the default printer setting in the Control Panel.
        /// </summary>
        public bool TrackDefault
        {
            get
            {
                throw new System.Exception("Method or Property not implemented yet!");
            }
            set
            {
                throw new System.Exception("Method or Property not implemented yet!");
            }
        }

        /// <summary>
        /// Zoom property -- pending to be implemented.
        /// Returns/sets the percentage by which printed output is to be scaled up or down.
        /// </summary>
        public int Zoom
        {
            get
            {
                throw new System.Exception("Method or Property not implemented yet!");
            }
            set
            {
                throw new System.Exception("Method or Property not implemented yet!");
            }
        }

        //*********************************************************************************************************************************

        /// <summary>
        /// _Duplex private member.
        /// </summary>
        private int _Duplex = 0;

        /// <summary>
        /// Duplex property (System.Drawing.Printing.Duplex enum)
        /// Returns or sets a value that determines whether a page is printed on both sides (if the printer supports this feature).
        /// Not available at design time.
        /// </summary>
        public int Duplex
        {
            get
            {
                return _Duplex;
            }
            set
            {
                _Duplex = value;
                switch (value)
                {
                    case 1:
                        InnerPrinter.PrinterSettings.Duplex = System.Drawing.Printing.Duplex.Simplex;
                        break;
                    case 2:
                        InnerPrinter.PrinterSettings.Duplex = System.Drawing.Printing.Duplex.Vertical;
                        break;
                    case 3:
                        InnerPrinter.PrinterSettings.Duplex = System.Drawing.Printing.Duplex.Horizontal;
                        break;
                    default:
                        InnerPrinter.PrinterSettings.Duplex = System.Drawing.Printing.Duplex.Default;
                        _Duplex = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// EndDoc method.
        /// Terminates a print operation sent to the Printer object, releasing the document to the print device or spooler.
        /// </summary>
        public void EndDoc()
        {
            PageIndex = GetNextDirtyPage(-1);
            if (PageIndex > -1)
                InnerPrinter.Print();

            ClearCollections();

            //Returns the PaperSize to the default value after printing
            lastCustomHeight = lastCustomWidth = 0;
            PaperSize = (int)InnerPrinter.PrinterSettings.DefaultPageSettings.PaperSize.Kind;
        }

        /// <summary>
        /// Given the current page index returns the index of the next page which is dirty
        /// PageInfo[] Pages.
        /// </summary>
        /// <param name="index">Current page index</param>
        /// <returns>-1 if there is no more dirty pages or the index of the next dirty page</returns>
        private int GetNextDirtyPage(int index)
        {
            index++;
            while (index < Pages.Length)
            {
                if (Pages[index].Dirty)
                    return index;

                index++;
            }
            return -1;
        }

        /// <summary>
        /// DocumentName property.
        /// Gets or sets the document name to display (for example, in a print status dialog box or printer queue) while printing the document.
        /// The document name to display while printing the document. The default is "document".
        /// </summary>
        public String DocumentName
        {
            get
            {
                return InnerPrinter.DocumentName;
            }
            set
            {
                InnerPrinter.DocumentName = value;
            }
        }

        /// <summary>
        /// FontBold property.
        /// </summary>
        public bool FontBold
        {
            get
            {
                return Font.Bold;
            }
            set
            {
                if (value)
                    Font = new System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style | System.Drawing.FontStyle.Bold);
                else
                    Font = new System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style & ~System.Drawing.FontStyle.Bold);
            }
        }

        /// <summary>
        /// FontCount property
        /// </summary>
        public int FontCount
        {
            get
            {
                // Get an array of the available font families.
                System.Drawing.FontFamily[] families = System.Drawing.FontFamily.GetFamilies(PrinterGraphics);
                return families.Length;
            }
        }

        /// <summary>
        /// FontItalic property.
        /// </summary>
        public bool FontItalic
        {
            get
            {
                return Font.Italic;
            }
            set
            {
                if (value)
                    Font = new System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style | System.Drawing.FontStyle.Italic);
                else
                    Font = new System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style & ~System.Drawing.FontStyle.Italic);
            }
        }

        /// <summary>
        /// FontName property.
        /// </summary>
        public string FontName
        {
            get
            {
                return Font.Name;
            }
            set
            {
                if (!value.Equals(Font.Name))
                {
                    Font = new System.Drawing.Font(value, Font.Size, Font.Style);
                    //FSQSABORIO 20080901. By default use Arial if the FontName is not accepted
                    if (!value.Equals(Font.Name))
                        Font = new System.Drawing.Font("Arial", Font.Size, Font.Style);
                }
            }
        }

        //ORIGINAL LINE: Public ReadOnly Property Fonts(ByVal index As Integer) As String
        //INSTANT C# NOTE: C# does not support parameterized properties - the following property has been rewritten as a function:
        /// <summary>
        /// GetFonts function, index access FontFamily array to return select item.Name
        /// </summary>
        public string GetFonts(int index)
        {
            // Get an array of the available font families.
            System.Drawing.FontFamily[] families = System.Drawing.FontFamily.GetFamilies(PrinterGraphics);
            return families[index].Name;
        }

        /// <summary>
        /// FontSize property.
        /// </summary>
        public float FontSize
        {
            get
            {
                return Font.Size;
            }
            set
            {
                if (value != Font.Size)
                {
                    Font = new System.Drawing.Font(Font.FontFamily, value, Font.Style);
                }
            }
        }

        /// <summary>
        /// FontStrikethru property.
        /// </summary>
        public bool FontStrikethru
        {
            get
            {
                return Font.Strikeout;
            }
            set
            {
                if (value)
                    Font = new System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style | System.Drawing.FontStyle.Strikeout);
                else
                    Font = new System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style & ~System.Drawing.FontStyle.Strikeout);
            }
        }

        /// <summary>
        /// FontUnderline property.
        /// </summary>
        public bool FontUnderline
        {
            get
            {
                return Font.Underline;
            }
            set
            {
                if (value)
                    Font = new System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style | System.Drawing.FontStyle.Underline);
                else
                    Font = new System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style & ~System.Drawing.FontStyle.Underline);
            }
        }

        /// <summary>
        /// ForeColor property.
        /// Pencil color.
        /// </summary>
        public System.Drawing.Color ForeColor
        {
            get
            {
                return objPen.Color;
            }
            set
            {
                objPen.Color = value;
            }
        }

        /// <summary>
        /// hDC property.
        /// </summary>
        public int hDC
        {
            get
            {
                return InnerPrinter.PrinterSettings.GetHdevmode().ToInt32();
            }
        }

        /// <summary>
        /// lastCustomHeight private member.
        /// The physical dimensions of the paper set up for the printing device; not available at design time. 
        /// If set at run time, values in these properties are used instead of the setting of the PaperSize property.
        /// </summary>
        private int lastCustomHeight = 0;

        /// <summary>
        /// _cacheHeight private member
        /// The physical Height of the paper for the printer. 
        /// According to Microsoft's documentation this value is given in Twips units
        /// Default value is set to -1, to address performance issues
        /// it improves performance
        /// </summary>
        private int _cacheHeight = -1;

        /// <summary>
        /// Height property.
        /// </summary>
        public int Height
        {
            get
            {
                if (_cacheHeight < 0)
                {

                    if (InnerPrinter.DefaultPageSettings.Landscape)
                        _cacheHeight = (int)ConvertFromPrinterUnitsY(InnerPrinter.DefaultPageSettings.PaperSize.Width, ScaleModeConstants.vbTwips);
                    else
                        _cacheHeight = (int)ConvertFromPrinterUnitsY(InnerPrinter.DefaultPageSettings.PaperSize.Height, ScaleModeConstants.vbTwips);
                }

                return _cacheHeight;
            }
            set
            {
                //In VB6, If you set the Height and Width properties for a printer driver that doesn't 
                //allow these properties to be set, no error occurs and the size 
                //of the paper remains as it was.
                try
                {
                    lastCustomHeight = Convert.ToInt32(ConvertToPrinterUnitsY(value, ScaleModeConstants.vbTwips));
                    PaperSize = PrinterObjectConstants.vbPRPSUser;
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// lastCustomWidth private member
        /// The physical dimensions of the paper set up for the printing device
        /// If set at run time, values in these properties are used instead of the setting of the PaperSize property.
        /// Not available at design time
        /// </summary>
        private int lastCustomWidth = 0;

        /// <summary>
        /// Width property
        /// The physical Width of the paper for the printer. 
        /// According to Microsoft's documentation this value is given in Twips units
        /// </summary>
        public int Width
        {
            get
            {
                if (InnerPrinter.DefaultPageSettings.Landscape)
                    return (int)ConvertFromPrinterUnitsX(InnerPrinter.DefaultPageSettings.PaperSize.Height, ScaleModeConstants.vbTwips);
                else
                    return (int)ConvertFromPrinterUnitsX(InnerPrinter.DefaultPageSettings.PaperSize.Width, ScaleModeConstants.vbTwips);
            }
            set
            {
                //In VB6, If you set the Height and Width properties for a printer driver that doesn't 
                //allow these properties to be set, no error occurs and the size 
                //of the paper remains as it was.
                try
                {
                    lastCustomWidth = Convert.ToInt32(ConvertToPrinterUnitsX(value, ScaleModeConstants.vbTwips));
                    PaperSize = PrinterObjectConstants.vbPRPSUser;
                }
                catch
                {
                }
            }
        }

        //The Line method was developed using a combination of optional parameters and overloading because:
        //   -  VB.NET doesn’t accepts Structures (i.e Color, Point) as optional parameters
        //   -  In VB.NET the optional parameters have to be located at the end of the method declaration

        //  LINE METHODS ///

        /// <summary>
        /// Line method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Line(System.Drawing.Point p1, System.Drawing.Point p2, bool Step1, bool Step2, bool Box)
        {
            Line(p1, p2, Step1, Step2, Box, false);
        }

        /// <summary>
        /// Line method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Line(System.Drawing.Point p1, System.Drawing.Point p2, bool Step1, bool Step2)
        {
            Line(p1, p2, Step1, Step2, false, false);
        }

        /// <summary>
        /// Line method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Line(System.Drawing.Point p1, System.Drawing.Point p2, bool Step1)
        {
            Line(p1, p2, Step1, false, false, false);
        }

        /// <summary>
        /// Line method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Line(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            Line(p1, p2, false, false, false, false);
        }

        //INSTANT C# NOTE: C# does not support optional parameters. Overloaded method(s) are created above.
        //ORIGINAL LINE: Public Sub Line(ByVal p1 As Drawing.Point, ByVal p2 As Drawing.Point, Optional ByVal Step1 As Boolean = false, Optional ByVal Step2 As Boolean = false, Optional ByVal Box As Boolean = false, Optional ByVal Fill As Boolean = false)
        /// <summary>
        /// Line method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Line(System.Drawing.Point p1, System.Drawing.Point p2, bool Step1, bool Step2, bool Box, bool Fill)
        {
            Line(p1, p2, objPen.Color, Step1, Step2, Box, Fill);
        }

        /// <summary>
        /// Line method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Line(System.Drawing.Point p1, System.Drawing.Point p2, System.Drawing.Color Color, bool Step1, bool Step2, bool Box)
        {
            Line(p1, p2, Color, Step1, Step2, Box, false);
        }

        /// <summary>
        /// Line method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Line(System.Drawing.Point p1, System.Drawing.Point p2, System.Drawing.Color Color, bool Step1, bool Step2)
        {
            Line(p1, p2, Color, Step1, Step2, false, false);
        }

        /// <summary>
        /// Line method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Line(System.Drawing.Point p1, System.Drawing.Point p2, System.Drawing.Color Color, bool Step1)
        {
            Line(p1, p2, Color, Step1, false, false, false);
        }

        /// <summary>
        /// Line method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Line(System.Drawing.Point p1, System.Drawing.Point p2, System.Drawing.Color Color)
        {
            Line(p1, p2, Color, false, false, false, false);
        }

        //INSTANT C# NOTE: C# does not support optional parameters. Overloaded method(s) are created above.
        //ORIGINAL LINE: Public Sub Line(ByVal p1 As Drawing.Point, ByVal p2 As Drawing.Point, ByVal Color As Drawing.Color, Optional ByVal Step1 As Boolean = false, Optional ByVal Step2 As Boolean = false, Optional ByVal Box As Boolean = false, Optional ByVal Fill As Boolean = false)
        /// <summary>
        /// Line method.
        /// </summary>
        [Obsolete("Use the methods receiving PointF in order to prevent lost of precision")]
        public void Line(System.Drawing.Point p1, System.Drawing.Point p2, System.Drawing.Color Color, bool Step1, bool Step2, bool Box, bool Fill)
        {
            System.Drawing.PointF newP1 = new System.Drawing.PointF(p1.X, p1.Y);
            System.Drawing.PointF newP2 = new System.Drawing.PointF(p2.X, p2.Y);
            Line(newP1, newP2, Color, Step1, Step2, Box, Fill);
        }

        /// <summary>
        /// Line method, P1 and P2 are given in the unit specified by Scalemode.
        /// </summary>
        public void Line(System.Drawing.PointF p1, System.Drawing.PointF p2, bool Step1, bool Step2, bool Box)
        {
            Line(p1, p2, Step1, Step2, Box, false);
        }

        /// <summary>
        /// Line method, P1 and P2 are given in the unit specified by Scalemode.
        /// </summary>
        public void Line(System.Drawing.PointF p1, System.Drawing.PointF p2, bool Step1, bool Step2)
        {
            Line(p1, p2, Step1, Step2, false, false);
        }

        /// <summary>
        /// Line method, P1 and P2 are given in the unit specified by Scalemode.
        /// </summary>
        public void Line(System.Drawing.PointF p1, System.Drawing.PointF p2, bool Step1)
        {
            Line(p1, p2, Step1, false, false, false);
        }

        /// <summary>
        /// Line method, P1 and P2 are given in the unit specified by Scalemode
        /// </summary>
        public void Line(System.Drawing.PointF p1, System.Drawing.PointF p2)
        {
            Line(p1, p2, false, false, false, false);
        }

        /// <summary>
        /// Line method, P1 and P2 are given in the unit specified by Scalemode.
        /// </summary>
        public void Line(System.Drawing.PointF p1, System.Drawing.PointF p2, bool Step1, bool Step2, bool Box, bool Fill)
        {
            Line(p1, p2, objPen.Color, Step1, Step2, Box, Fill);
        }

        /// <summary>
        /// Line method, P1 and P2 are given in the unit specified by Scalemode.
        /// </summary>
        public void Line(System.Drawing.PointF p1, System.Drawing.PointF p2, System.Drawing.Color Color, bool Step1, bool Step2, bool Box)
        {
            Line(p1, p2, Color, Step1, Step2, Box, false);
        }

        /// <summary>
        /// Line method, P1 and P2 are given in the unit specified by Scalemode.
        /// </summary>
        public void Line(System.Drawing.PointF p1, System.Drawing.PointF p2, System.Drawing.Color Color, bool Step1, bool Step2)
        {
            Line(p1, p2, Color, Step1, Step2, false, false);
        }

        /// <summary>
        /// Line method, P1 and P2 are given in the unit specified by Scalemode.
        /// </summary>
        public void Line(System.Drawing.PointF p1, System.Drawing.PointF p2, System.Drawing.Color Color, bool Step1)
        {
            Line(p1, p2, Color, Step1, false, false, false);
        }

        /// <summary>
        /// Line method, P1 and P2 are given in the unit specified by Scalemode.
        /// </summary>
        public void Line(System.Drawing.PointF p1, System.Drawing.PointF p2, System.Drawing.Color Color)
        {
            Line(p1, p2, Color, false, false, false, false);
        }

        /// <summary>
        /// Line method, P1 and P2 are given in the unit specified by Scalemode.
        /// </summary>
        public void Line(System.Drawing.PointF p1, System.Drawing.PointF p2, System.Drawing.Color Color, bool Step1, bool Step2, bool Box, bool Fill)
        {
            if (Step1 == true)
            {
                p1.X = p1.X + (float)CurrentX;
                p1.Y = p1.Y + (float)CurrentY;
            }

            if (Step2 == true)
            {
                p2.X = p2.X + (float)CurrentX;
                p2.Y = p2.Y + (float)CurrentY;
            }

            // Moves the CurrentX and CurrentY properties
            CurrentX = p2.X;
            CurrentY = p2.Y;

            if (Box == true) // Draw a Box
            {
                DrawRectangle(new System.Drawing.RectangleF(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y), Color, Fill);
            }
            else // Draw a single line
            {
                DrawLine(p1, p2, Color);
            }
        }


        //  LINE METHODS


        /// <summary>
        /// DrawLine method, P1 and P2 are given in the unit specified by Scalemode.
        /// </summary>
        private void DrawLine(System.Drawing.PointF p1, System.Drawing.PointF p2, System.Drawing.Color Color)
        {
            //Stores the position in pixels within the structure Line
            PointF newP1 = new PointF((float)ConvertToPrinterUnitsX(p1.X), (float)ConvertToPrinterUnitsY(p1.Y));
            PointF newP2 = new PointF((float)ConvertToPrinterUnitsX(p2.X), (float)ConvertToPrinterUnitsY(p2.Y));

            LineInfo[] tmpLines = Pages[PageIndex].Lines;
            if ((Pages[PageIndex].Lines == null))
                Array.Resize(ref tmpLines, 1);
            else
                Array.Resize(ref tmpLines, Pages[PageIndex].Lines.Length + 1);

            Pages[PageIndex].Lines = tmpLines;

            Pages[PageIndex].Lines[Pages[PageIndex].Lines.Length - 1].p1 = newP1;
            Pages[PageIndex].Lines[Pages[PageIndex].Lines.Length - 1].p2 = newP2;
            Pages[PageIndex].Lines[Pages[PageIndex].Lines.Length - 1].pen = (System.Drawing.Pen)objPen.Clone();
            Pages[PageIndex].Lines[Pages[PageIndex].Lines.Length - 1].pen.Color = Color;
        }

        /// <summary>
        /// DrawRectangle method, rec is given in the unit specified by Scalemode.
        /// </summary>
        private void DrawRectangle(System.Drawing.RectangleF rec, System.Drawing.Color Color, bool Fill)
        {
            //Stores the position in pixels within the structure Rectangle
            RectangleF newRec = new RectangleF((float)ConvertToPrinterUnitsX(rec.X), (float)ConvertToPrinterUnitsY(rec.Y),
                (float)ConvertToPrinterUnitsX(rec.Width), (float)ConvertToPrinterUnitsY(rec.Height));

            RectangleInfo[] tmpRectangles = Pages[PageIndex].Rectangles;
            if ((Pages[PageIndex].Rectangles == null))
                Array.Resize(ref tmpRectangles, 1);
            else
                Array.Resize(ref tmpRectangles, Pages[PageIndex].Rectangles.Length + 1);

            Pages[PageIndex].Rectangles = tmpRectangles;

            Pages[PageIndex].Rectangles[Pages[PageIndex].Rectangles.Length - 1].rec = newRec;
            Pages[PageIndex].Rectangles[Pages[PageIndex].Rectangles.Length - 1].pen = (System.Drawing.Pen)objPen.Clone();
            Pages[PageIndex].Rectangles[Pages[PageIndex].Rectangles.Length - 1].pen.Color = Color;
            if (Fill)
            {
                Pages[PageIndex].Rectangles[Pages[PageIndex].Rectangles.Length - 1].FillStyle = FillStyleConstants.vbFSSolid;
                Pages[PageIndex].Rectangles[Pages[PageIndex].Rectangles.Length - 1].FillColor = Color;
            }
            else
            {
                Pages[PageIndex].Rectangles[Pages[PageIndex].Rectangles.Length - 1].FillStyle = _FillStyle;
                Pages[PageIndex].Rectangles[Pages[PageIndex].Rectangles.Length - 1].FillColor = FillColor;
            }
        }

        /// <summary>
        /// Adds a new page.
        /// </summary>
        public void NewPage()
        {
            Pages[Pages.Length - 1].Dirty = true;
            Array.Resize(ref Pages, Pages.Length + 1);
            PageIndex = Pages.Length - 1;
            Pages[PageIndex] = new PageInfo();
            _CurrentX = 0;
            _CurrentY = 0;
        }

        /// <summary>
        /// Orientation property gets or sets a value indicating whether the page is printed in landscape or portrait orientation.
        /// vbPRORPortrait = 1, vbPRORLandscape = 2.
        /// </summary>
        public int Orientation
        {
            get
            {
                return (InnerPrinter.DefaultPageSettings.Landscape) ? 2 : 1;
            }
            set
            {
                InnerPrinter.DefaultPageSettings.Landscape = (value == 2);
                _cacheHeight = -1;
            }
        }

        /// <summary>
        /// Page property, current page index.
        /// </summary>
        public int Page
        {
            get
            {
                return PageIndex + 1;
            }
        }

        //  PAINTPICTURE METHODS ///

        /// <summary>
        /// PaintPicture method draws an image in the current page.
        /// <param name="Picture">The picture to draw</param>
        /// <param name="p">The upper left corner</param>
        /// </summary>
        [Obsolete("Use the method receiving PointF in order to prevent lost of precision")]
        public void PaintPicture(System.Drawing.Image Picture, System.Drawing.Point p)
        {
            PointF newP = new PointF(p.X, p.Y);
            PaintPicture(Picture, newP);
        }

        /// <summary>
        /// PaintPicture method draws an image in the current page, P is given in the unit specified by Scalemode.
        /// <param name="Picture">The picture to draw</param>
        /// <param name="P">The upper left corner</param>
        /// </summary>
        public void PaintPicture(System.Drawing.Image Picture, System.Drawing.PointF P)
        {
            ImageInfo[] tmpImages = Pages[PageIndex].Images;
            if ((Pages[PageIndex].Images == null))
                Array.Resize(ref tmpImages, 1);
            else
                Array.Resize(ref tmpImages, Pages[PageIndex].Images.Length + 1);

            Pages[PageIndex].Images = tmpImages;

            //P is stored in pixels
            PointF newP = new PointF((float)ConvertToPrinterUnitsX(P.X), (float)ConvertToPrinterUnitsY(P.Y));

            Pages[PageIndex].Images[Pages[PageIndex].Images.Length - 1].p = newP;
            Pages[PageIndex].Images[Pages[PageIndex].Images.Length - 1].picture = (System.Drawing.Image)Picture.Clone();
        }

        /// <summary>
        /// PaintPicture method draws an image in the current page, P is given in the unit specified by Scalemode.
        /// </summary>
        /// <param name="Picture">The picture to draw</param>
        /// <param name="P1">The upper left corner</param>
        /// <param name="P2">The lower right corner</param>
        public void PaintPicture(System.Drawing.Image Picture, System.Drawing.PointF P1, System.Drawing.PointF P2)
        {
            int newWidth = (int)ConvertToPixelsX(P2.X - P1.X);
            int newHeight = (int)ConvertToPixelsY(P2.Y - P1.Y);

            System.Drawing.Bitmap tmpPicture = new Bitmap(Picture, newWidth, newHeight);
            PaintPicture(tmpPicture, P1);
        }

        //  PAINTPICTURE METHODS ///

        /// <summary>
        /// PaperSize property returns or sets a value indicating the paper size for the current printer
        /// Not available at design time.
        /// </summary>
        public int PaperSize
        {
            get
            {
                if (InnerPrinter.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
                    return PrinterObjectConstants.vbPRPSUser;
                else
                    return InnerPrinter.DefaultPageSettings.PaperSize.RawKind;
            }
            set
            {
                if ((value != PaperSize) || (value == PrinterObjectConstants.vbPRPSUser))
                {
                    InnerPrinter.DefaultPageSettings.PaperSize = GetPaperSize(value);
                    _cacheHeight = -1;
                }
            }
        }

        /// <summary>
        /// Private GetPaperSize method returns a paper size suitable for the current printer.
        /// </summary>
        /// <param name="value">The enum value given the desired paper size</param>
        /// <returns>The paper size that can be used or an exception if the paper size desired is not available</returns>
        private PaperSize GetPaperSize(int value)
        {
            if (value == PrinterObjectConstants.vbPRPSUser)
            {
                if (ListOfAvailableSizes.ContainsKey(PaperKind.Custom))
                {
                    PaperSize pSize = new PaperSize(string.Empty, lastCustomWidth, lastCustomHeight);
                    pSize.PaperName = ListOfAvailableSizes[PaperKind.Custom].PaperName;
                    pSize.RawKind = ListOfAvailableSizes[PaperKind.Custom].RawKind;
                    return pSize;
                }
            }
            else if ((Enum.IsDefined(typeof(PaperKind), value)) && (ListOfAvailableSizes.ContainsKey((PaperKind)value)))
            {
                return ListOfAvailableSizes[(PaperKind)value];
            }

            throw new InvalidOperationException("Invalid property value");
        }

        /// <summary>
        /// Private PrinterName property for current property.
        /// </summary>
        private string CurrentPrinterName = string.Empty;

        /// <summary>
        /// Private member list of available sizes for the current printer.
        /// </summary>
        Dictionary<PaperKind, PaperSize> _ListOfAvailableSizes = null;

        /// <summary>
        /// Returns the list of available sizes for the current printer.
        /// </summary>
        private Dictionary<PaperKind, PaperSize> ListOfAvailableSizes
        {
            get
            {
                if (!CurrentPrinterName.Equals(InnerPrinter.PrinterSettings.PrinterName) || (_ListOfAvailableSizes == null))
                {
                    CurrentPrinterName = InnerPrinter.PrinterSettings.PrinterName;
                    _ListOfAvailableSizes = new Dictionary<PaperKind, PaperSize>();
                    foreach (PaperSize pSize in InnerPrinter.PrinterSettings.PaperSizes)
                    {
                        _ListOfAvailableSizes.Add(pSize.Kind, pSize);
                    }
                }

                return _ListOfAvailableSizes;
            }
        }

        /// <summary>
        /// Returns the height of a text string as it would be printed in the current font of the printer.
        /// The height is expressed in terms of the ScaleMode property setting.
        /// </summary>
        /// <param name="str">The string text to use in the calculation</param>
        /// <returns>The text height expressed in terms of the ScaleMode units</returns>
        public double TextWidth(string str)
        {
            System.Drawing.SizeF size = new System.Drawing.SizeF();
            size = System.Windows.Forms.TextRenderer.MeasureText(PrinterGraphics, str, Font);
            return ConvertFromPixelsX(size.Width);
        }

        /// <summary>
        /// Returns the width of a text string as it would be printed in the current font of the printer.
        /// The width is expressed in terms of the ScaleMode property setting.
        /// </summary>
        /// <param name="str">The string text to use in the calculation</param>
        /// <returns>The text width expressed in terms of the ScaleMode units</returns>
        public double TextHeight(string str)
        {
            System.Drawing.SizeF size = new System.Drawing.SizeF();
            size = System.Windows.Forms.TextRenderer.MeasureText(PrinterGraphics, str, Font);
            return ConvertFromPixelsY(size.Height);
        }

        //AIS-FSABORIO To improve performance
        /// <summary>
        /// Private member, System.Drawing.Graphics associated with the specified page settings,
        /// and optionally specifying the origin at the margins
        /// member will be created untill it is requested
        /// it improves performance.
        /// </summary>
        private Graphics _PrinterGraphics = null;

        /// <summary>
        /// Private member, accessor for _PrinterGraphics private member
        /// it improves performance.
        /// </summary>
        private Graphics PrinterGraphics
        {
            get
            {
                if (_PrinterGraphics == null)
                    _PrinterGraphics = InnerPrinter.PrinterSettings.CreateMeasurementGraphics();

                return _PrinterGraphics;
            }
        }

        /// <summary>
        /// Private property to store the average height of a text
        /// it improves performance.
        /// </summary>
        private double _InternalTextHeight = -1;
        /// <summary>
        /// Private member, accessor for _InternalTextHeight private member
        /// it improves performance.
        /// </summary>
        private double InternalTextHeight
        {
            get
            {
                if (_InternalTextHeight < 0)
                    _InternalTextHeight = TextHeight("Text Height");

                return _InternalTextHeight;
            }
        }

        /// <summary>
        /// Prints an empty line.
        /// </summary>
        public void Print()
        {
            this.CurrentY += this.InternalTextHeight;
            this.CurrentX = 0;

            //If no character can be printed in the next line then a new page will be inserted
            if ((CurrentY + this.InternalTextHeight) > ConvertFromPixelsY(ConvertToPixelsY(Height, ScaleModeConstants.vbTwips)))
                NewPage();
        }

        /// <summary>
        /// Prints
        /// NoNewLine if parameter is set to true, it won't print.
        /// <param name="NoNewLine">boolean flag to stop printing</param>
        /// </summary>
        public void Print(bool NoNewLine)
        {
            if (!NoNewLine)
                Print();
        }

        /// <summary>
        /// Prints.
        /// <param name="str">array of objects which will be printed</param>
        /// </summary>
        public void Print(params object[] str)
        {
            Print(false, str);
        }

        /// <summary>
        /// Prints.
        /// <param name="NoNewLine">boolean flag to stop printing</param>
        /// <param name="str">str are the values which will be printed</param>
        /// </summary>
        public void Print(bool NoNewLine, params object[] str)
        {
            //note:
            // 
            //     All positions and sizes are calculated in the unit measurement given by ScaleMode,
            //     when the final numbers are stored in the Data structures then they are converted to
            //     pixels
            //     

            int j = 0;
            string myStr = null;
            double tabPos = 0;

            for (j = 0; j < str.Length; j++)
            {
                myStr = str[j].ToString();
                if ((myStr == System.Environment.NewLine) | (myStr == "\r") | (myStr == "\n") | (myStr == System.Environment.NewLine))
                {
                    this.CurrentY += this.InternalTextHeight;
                    CurrentX = 0;
                }
                else if (myStr == "\t")
                {
                    CurrentX += ConvertFromPixelsX(TabSize);
                }
                else if (str[j] is Microsoft.VisualBasic.TabInfo)
                {
                    Microsoft.VisualBasic.TabInfo tInfo = ((Microsoft.VisualBasic.TabInfo)str[j]);
                    if (tInfo.Column > MaxColumnAvailables)
                        tInfo.Column = (short)(tInfo.Column % MaxColumnAvailables);

                    if (tInfo.Column < 1)
                        tInfo.Column = 1;

                    tInfo.Column--;

                    tabPos = tInfo.Column * ColumnSize;
                    if (CurrentX > tabPos)
                        this.CurrentY += this.InternalTextHeight;

                    CurrentX = tabPos;
                }
                else
                {
                    DataInfo[] tmpData = Pages[PageIndex].Data;
                    if ((Pages[PageIndex].Data == null))
                        Array.Resize(ref tmpData, 1);
                    else
                        Array.Resize(ref tmpData, Pages[PageIndex].Data.Length + 1);

                    Pages[PageIndex].Data = tmpData;

                    Pages[PageIndex].Data[Pages[PageIndex].Data.Length - 1].value = myStr;
                    Pages[PageIndex].Data[Pages[PageIndex].Data.Length - 1].font = (System.Drawing.Font)Font.Clone();
                    Pages[PageIndex].Data[Pages[PageIndex].Data.Length - 1].X = (float)ConvertToPrinterUnitsX(CurrentX);
                    Pages[PageIndex].Data[Pages[PageIndex].Data.Length - 1].Y = (float)ConvertToPrinterUnitsY(CurrentY);
                    CurrentX += this.TextWidth(myStr);
                }
            }

            // Adds a Carriage return–linefeed 
            if (!NoNewLine)
            {
                this.CurrentY += this.InternalTextHeight;
                this.CurrentX = 0;
            }


            //If no caracter can be printed in the next line then a new page is inserted
            if ((CurrentY + this.InternalTextHeight) > ConvertFromPixelsY(ConvertToPixelsY(Height, ScaleModeConstants.vbTwips)))
                NewPage();
        }

        /// <summary>
        /// Clear all Collections.
        /// </summary>
        private void ClearCollections()
        {
            int j = 0;
            for (j = 0; j < Pages.Length; j++)
            {
                if (Pages[j].Data != null)
                {
                    Array.Clear(Pages[j].Data, 0, Pages[j].Data.Length - 1);
                }
                if (Pages[j].Circles != null)
                {
                    Array.Clear(Pages[j].Circles, 0, Pages[j].Circles.Length);
                }
                if (Pages[j].Images != null)
                {
                    Array.Clear(Pages[j].Images, 0, Pages[j].Images.Length);
                }
                if (Pages[j].Lines != null)
                {
                    Array.Clear(Pages[j].Lines, 0, Pages[j].Lines.Length);
                }
                if (Pages[j].Rectangles != null)
                {
                    Array.Clear(Pages[j].Rectangles, 0, Pages[j].Rectangles.Length);
                }
            }
            Array.Clear(Pages, 0, Pages.Length);
            Pages = new PageInfo[1];
            PageIndex = 0;
            Pages[PageIndex] = new PageInfo();
            this._CurrentX = 0.0;
            this._CurrentY = 0.0;

            Utils.MemoryHelper.ReleaseMemory();
        }

        /// <summary>
        /// Print the pages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Printer_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int i = 0;

            // Draw all Lines
            if ((Pages[PageIndex].Lines == null) == false)
            {
                for (i = 0; i < Pages[PageIndex].Lines.Length; i++)
                {
                    e.Graphics.DrawLine(Pages[PageIndex].Lines[i].pen, Pages[PageIndex].Lines[i].p1.X, Pages[PageIndex].Lines[i].p1.Y, Pages[PageIndex].Lines[i].p2.X, Pages[PageIndex].Lines[i].p2.Y);
                }
            }
            // Draw all Rectangles
            if ((Pages[PageIndex].Rectangles == null) == false)
            {
                for (i = 0; i < Pages[PageIndex].Rectangles.Length; i++)
                {
                    e.Graphics.DrawRectangle(Pages[PageIndex].Rectangles[i].pen, Pages[PageIndex].Rectangles[i].rec.X,
                        Pages[PageIndex].Rectangles[i].rec.Y, Pages[PageIndex].Rectangles[i].rec.Width, Pages[PageIndex].Rectangles[i].rec.Height);
                    if (Pages[PageIndex].Rectangles[i].FillStyle == FillStyleConstants.vbFSSolid)
                    {
                        e.Graphics.FillRectangle(new System.Drawing.SolidBrush(Pages[PageIndex].Rectangles[i].FillColor), Pages[PageIndex].Rectangles[i].rec);
                    }
                }
            }
            // Draw all Images
            if ((Pages[PageIndex].Images == null) == false)
            {
                for (i = 0; i < Pages[PageIndex].Images.Length; i++)
                {
                    e.Graphics.DrawImage(Pages[PageIndex].Images[i].picture, Pages[PageIndex].Images[i].p);
                }
            } // Draw all circles
            if ((Pages[PageIndex].Circles == null) == false)
            {
                for (i = 0; i < Pages[PageIndex].Circles.Length; i++)
                {
                    //int x = 0;
                    //int y = 0;
                    e.Graphics.DrawEllipse(Pages[PageIndex].Circles[i].pen, Pages[PageIndex].Circles[i].rec);
                    if (Pages[PageIndex].Circles[i].FillStyle == FillStyleConstants.vbFSSolid)
                    {
                        e.Graphics.FillEllipse(new System.Drawing.SolidBrush(Pages[PageIndex].Circles[i].FillColor), Pages[PageIndex].Circles[i].rec);
                    }
                }
            }
            if ((Pages[PageIndex].Data == null) == false)
            {
                for (i = 0; i < Pages[PageIndex].Data.Length; i++)
                {
                    if (brush == null)
                    {
                        e.Graphics.DrawString(Pages[PageIndex].Data[i].value, Pages[PageIndex].Data[i].font, System.Drawing.Brushes.Black, Pages[PageIndex].Data[i].X, Pages[PageIndex].Data[i].Y);
                    }
                    else
                    {
                        e.Graphics.DrawString(Pages[PageIndex].Data[i].value, Pages[PageIndex].Data[i].font, brush, Pages[PageIndex].Data[i].X, Pages[PageIndex].Data[i].Y);
                    }
                }
            }

            PageIndex = GetNextDirtyPage(PageIndex);
            e.HasMorePages = (PageIndex > -1);
        }

        /// <summary>
        /// Private member, stores Print Quality value.
        /// </summary>
        private int _PrintQuality = -3;

        /// <summary>
        /// Returns or sets a value indicating the printer resolution
        /// Not available at design time
        /// System.Drawing.Printing.PrinterResolutionKind PrintQuality
        /// InnerPrinter.DefaultPageSettings.PrinterResolution.Kind.
        /// </summary>
        public int PrintQuality
        {
            get
            {
                return _PrintQuality;  //InnerPrinter.DefaultPageSettings.PrinterResolution.Kind;
            }
            set
            {
                _PrintQuality = value;
                switch (value)
                {
                    case -1:
                        //Draft
                        InnerPrinter.DefaultPageSettings.PrinterResolution = InnerPrinter.PrinterSettings.PrinterResolutions[3];
                        break;
                    case -3:
                        //Medium
                        InnerPrinter.DefaultPageSettings.PrinterResolution = InnerPrinter.PrinterSettings.PrinterResolutions[2];
                        break;
                    case -2:
                        //Low
                        InnerPrinter.DefaultPageSettings.PrinterResolution = InnerPrinter.PrinterSettings.PrinterResolutions[1];
                        break;
                    case -4:
                        //High
                        InnerPrinter.DefaultPageSettings.PrinterResolution = InnerPrinter.PrinterSettings.PrinterResolutions[0];
                        break;
                    default:
                        //Default value is Medium Resolution
                        InnerPrinter.DefaultPageSettings.PrinterResolution = InnerPrinter.PrinterSettings.PrinterResolutions[1];
                        _PrintQuality = -3;
                        break;
                }
            }
        }

        /// <summary>
        /// Returns or sets a value indicating the printer resolution
        /// Not available at design time
        /// System.Drawing.Printing.PrinterResolutionKind PrintQuality
        /// InnerPrinter.DefaultPageSettings.PrinterResolution.Kind.
        /// </summary>
        public int PrintQuality2
        {

            get
            {
                return (int)InnerPrinter.DefaultPageSettings.PrinterResolution.Kind;
            }
            set
            {
                switch (value)
                {
                    case -1:
                        //Draft
                        InnerPrinter.DefaultPageSettings.PrinterResolution = InnerPrinter.PrinterSettings.PrinterResolutions[3];
                        break;
                    case -2:
                        //Low
                        InnerPrinter.DefaultPageSettings.PrinterResolution = InnerPrinter.PrinterSettings.PrinterResolutions[2];
                        break;
                    case -3:
                        //Medium
                        InnerPrinter.DefaultPageSettings.PrinterResolution = InnerPrinter.PrinterSettings.PrinterResolutions[1];
                        break;
                    case -4:
                        //High
                        InnerPrinter.DefaultPageSettings.PrinterResolution = InnerPrinter.PrinterSettings.PrinterResolutions[0];
                        break;
                    default:
                        //Default value is Medium Resolution
                        InnerPrinter.DefaultPageSettings.PrinterResolution = InnerPrinter.PrinterSettings.PrinterResolutions[1];
                        break;
                }
            }
        }

        /// <summary>
        /// Converts the parameter to printer units X. 
        /// It is assumed that the parameter is given in the units specified by ScaleMode
        /// </summary>
        /// <param name="num">The number to convert to printer units X</param>
        /// <returns>The parameter converted to printer units given the value of ScaleMode</returns>
        public double ConvertToPrinterUnitsX(double num)
        {
            return ConvertToPrinterUnitsX(num, _ScaleMode);
        }

        /// <summary>
        /// Converts the parameter to printer units X. 
        /// It is assumed that the parameter is given in the units specified by ScaleMode
        /// </summary>
        /// <param name="num">The number to convert to printer units X</param>
        /// <param name="ScaleMode">The ScaleMode to use</param>
        /// <returns>The parameter converted to printer units given the value of ScaleMode</returns>
        private double ConvertToPrinterUnitsX(double num, ScaleModeConstants ScaleMode)
        {
            //The parameter is converted to pixels then to the printer units
            return ConvertToPixelsX(num, ScaleMode) / (DISPLAY_DPI / 100);
        }

        /// <summary>
        /// Converts the parameter to printer units Y. 
        /// It is assumed that the parameter is given in the units specified by ScaleMode
        /// </summary>
        /// <param name="num">The number to convert to printer units Y</param>
        /// <returns>The parameter converted to printer units given the value of ScaleMode</returns>
        public double ConvertToPrinterUnitsY(double num)
        {
            return ConvertToPrinterUnitsY(num, _ScaleMode);
        }

        /// <summary>
        /// Converts the parameter to printer units Y. 
        /// It is assumed that the parameter is given in the units specified by ScaleMode
        /// </summary>
        /// <param name="num">The number to convert to printer units Y</param>
        /// <param name="ScaleMode">The ScaleMode to use</param>
        /// <returns>The parameter converted to printer units given the value of ScaleMode</returns>
        private double ConvertToPrinterUnitsY(double num, ScaleModeConstants ScaleMode)
        {
            //The parameter is converted to pixels then to the printer units
            return ConvertToPixelsY(num, ScaleMode) / (DISPLAY_DPI / 100);
        }

        /// <summary>
        /// Converts the paratemer to the new numeric system given by ScaleMode.
        /// It is assumed that the parameter is represented in the internal units of the printer
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public double ConvertFromPrinterUnitsX(double num)
        {
            return ConvertFromPrinterUnitsX(num, _ScaleMode);
        }

        /// <summary>
        /// Converts the paratemer to the new numeric system given by ScaleMode.
        /// It is assumed that the parameter is represented in the internal units of the printer
        /// </summary>
        /// <param name="num"></param>
        /// <param name="ScaleMode"></param>
        /// <returns></returns>
        private double ConvertFromPrinterUnitsX(double num, ScaleModeConstants ScaleMode)
        {
            //The parameter is converted to pixels then to the internal units given by ScaleMode
            return ConvertFromPixelsX(num * (DISPLAY_DPI / 100), ScaleMode);
        }

        /// <summary>
        /// Converts the paratemer to the new numeric system given by ScaleMode.
        /// It is assumed that the parameter is represented in the internal units of the printer
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public double ConvertFromPrinterUnitsY(double num)
        {
            //The parameter is converted to pixels then to the internal units given by ScaleMode
            return ConvertFromPrinterUnitsY(num, _ScaleMode);
        }

        /// <summary>
        /// Converts the paratemer to the new numeric system given by ScaleMode.
        /// It is assumed that the parameter is represented in the internal units of the printer
        /// </summary>
        /// <param name="num"></param>
        /// <param name="ScaleMode"></param>
        /// <returns></returns>
        private double ConvertFromPrinterUnitsY(double num, ScaleModeConstants ScaleMode)
        {
            return ConvertFromPixelsY(num * (DISPLAY_DPI / 100), ScaleMode);
        }


        /// <summary>
        /// Converts the parameter to the new numeric system given by ScaleMode.
        /// It is assumed that the parameter represents pixels X
        /// </summary>
        /// <param name="num">The pixels to convert</param>
        /// <returns>The pixels converted according to the ScaleMode property</returns>
        public double ConvertFromPixelsX(double num)
        {
            return ConvertFromPixelsX(num, _ScaleMode);
        }

        /// <summary>
        /// Converts the parameter to the new numeric system given by ScaleMode.
        /// It is assumed that the parameter represents pixels X
        /// </summary>
        /// <param name="num">The pixels to convert</param>
        /// <param name="ScaleMode">The ScaleMode to use</param>
        /// <returns>The pixels converted according to the ScaleMode parameter</returns>
        private double ConvertFromPixelsX(double num, ScaleModeConstants ScaleMode)
        {
            double res = 0;
            switch (ScaleMode)
            {
                case ScaleModeConstants.vbTwips:
                    res = Support.PixelsToTwipsX(num);
                    break;
                case ScaleModeConstants.vbPoints:
                    res = Support.FromPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Points);
                    break;
                case ScaleModeConstants.vbCentimeters:
                    res = Support.FromPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Centimeters);
                    break;
                case ScaleModeConstants.vbCharacters:
                    res = Support.FromPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Characters);
                    break;
                case ScaleModeConstants.vbHimetric:
                    res = Support.FromPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Himetric);
                    break;
                case ScaleModeConstants.vbInches:
                    res = Support.FromPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Inches);
                    break;
                case ScaleModeConstants.vbMilimeters:
                    res = Support.FromPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Millimeters);
                    break;
                default:
                    res = num;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Converts the parameter to the new numeric system given by ScaleMode.
        /// It is assumed that the parameter represents pixels Y
        /// </summary>
        /// <param name="num">The pixels to convert</param>
        /// <returns>The pixels converted according to the ScaleMode property</returns>
        public double ConvertFromPixelsY(double num)
        {
            return ConvertFromPixelsY(num, _ScaleMode);
        }

        /// <summary>
        /// Converts the parameter to the new numeric system given by ScaleMode.
        /// It is assumed that the parameter represents pixels Y
        /// </summary>
        /// <param name="num">The pixels to convert</param>
        /// <param name="ScaleMode">The ScaleMode to use</param>
        /// <returns>The pixels converted according to the ScaleMode parameter</returns>
        private double ConvertFromPixelsY(double num, ScaleModeConstants ScaleMode)
        {
            double res = 0;
            switch (ScaleMode)
            {
                case ScaleModeConstants.vbTwips:
                    res = Support.PixelsToTwipsY(num);
                    break;
                case ScaleModeConstants.vbPoints:
                    res = Support.FromPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Points);
                    break;
                case ScaleModeConstants.vbCentimeters:
                    res = Support.FromPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Centimeters);
                    break;
                case ScaleModeConstants.vbCharacters:
                    res = Support.FromPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Characters);
                    break;
                case ScaleModeConstants.vbHimetric:
                    res = Support.FromPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Himetric);
                    break;
                case ScaleModeConstants.vbInches:
                    res = Support.FromPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Inches);
                    break;
                case ScaleModeConstants.vbMilimeters:
                    res = Support.FromPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Millimeters);
                    break;
                default:
                    res = num;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Converts the parameter to pixels X. 
        /// It is assumed that the parameter is given in the units specified by ScaleMode
        /// </summary>
        /// <param name="num">The number to convert to pixels X</param>
        /// <returns>The parameter converted to pixels given the value of ScaleMode</returns>
        public double ConvertToPixelsX(double num)
        {
            return ConvertToPixelsX(num, _ScaleMode);
        }

        /// <summary>
        /// Converts the parameter to pixels X. 
        /// It is assumed that the parameter is given in the units specified by ScaleMode
        /// </summary>
        /// <param name="num">The number to convert to pixels X</param>
        /// <param name="ScaleMode">The ScaleMode to use</param>
        /// <returns>The parameter converted to pixels given the value of ScaleMode</returns>
        private double ConvertToPixelsX(double num, ScaleModeConstants ScaleMode)
        {
            double res = 0;
            switch (ScaleMode)
            {
                case ScaleModeConstants.vbTwips:
                    res = Support.TwipsToPixelsX(num);
                    break;
                case ScaleModeConstants.vbPoints:
                    res = Support.ToPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Points);
                    break;
                case ScaleModeConstants.vbCentimeters:
                    res = Support.ToPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Centimeters);
                    break;
                case ScaleModeConstants.vbCharacters:
                    res = Support.ToPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Characters);
                    break;
                case ScaleModeConstants.vbHimetric:
                    res = Support.ToPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Himetric);
                    break;
                case ScaleModeConstants.vbInches:
                    res = Support.ToPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Inches);
                    break;
                case ScaleModeConstants.vbMilimeters:
                    res = Support.ToPixelsX(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Millimeters);
                    break;
                default:
                    res = num;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Converts the parameter to pixels Y. 
        /// It is assumed that the parameter is given in the units specified by ScaleMode
        /// </summary>
        /// <param name="num">The number to convert to pixels Y</param>
        /// <returns>The parameter converted to pixels given the value of ScaleMode</returns>
        public double ConvertToPixelsY(double num)
        {
            return ConvertToPixelsY(num, _ScaleMode);
        }

        /// <summary>
        /// Converts the parameter to pixels Y. 
        /// It is assumed that the parameter is given in the units specified by ScaleMode
        /// </summary>
        /// <param name="num">The number to convert to pixels Y</param>
        /// <param name="ScaleMode">The ScaleMode to use</param>
        /// <returns>The parameter converted to pixels given the value of ScaleMode</returns>
        private double ConvertToPixelsY(double num, ScaleModeConstants ScaleMode)
        {

            double res = 0;
            switch (ScaleMode)
            {
                case ScaleModeConstants.vbTwips:
                    res = Support.TwipsToPixelsY(num);
                    break;
                case ScaleModeConstants.vbPoints:
                    res = Support.ToPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Points);
                    break;
                case ScaleModeConstants.vbCentimeters:
                    res = Support.ToPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Centimeters);
                    break;
                case ScaleModeConstants.vbCharacters:
                    res = Support.ToPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Characters);
                    break;
                case ScaleModeConstants.vbHimetric:
                    res = Support.ToPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Himetric);
                    break;
                case ScaleModeConstants.vbInches:
                    res = Support.ToPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Inches);
                    break;
                case ScaleModeConstants.vbMilimeters:
                    res = Support.ToPixelsY(num, Microsoft.VisualBasic.Compatibility.VB6.ScaleMode.Millimeters);
                    break;
                default:
                    res = num;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Gets a value that is used to convert twips to pixels based on screen settings.
        /// <returns>The conversion factor.</returns>
        /// </summary>
        public float TwipsPerPixelX
        {
            get
            {
                return Support.TwipsPerPixelX();
            }
        }

        /// <summary>
        /// Gets a value that is used to convert twips to pixels based on screen settings.
        /// <returns>The conversion factor.</returns>
        /// </summary>
        public float TwipsPerPixelY
        {
            get
            {
                return Support.TwipsPerPixelY();
            }
        }

        //******************************************************************************************************
        //These are the variables if the font class of VB6

        /// <summary>
        /// Class compatibility for VB6 StdFont
        /// </summary>
        public class StdFont
        {
            /// <summary>
            /// Font Bold
            /// </summary>
            public bool Bold = false;
            /// <summary>
            /// Font CharSet
            /// </summary>
            public int CharSet = 0;
            /// <summary>
            /// Font Italic
            /// </summary>
            public bool Italic = false;
            /// <summary>
            /// Font Name
            /// </summary>
            public string Name = "Arial";
            /// <summary>
            /// Font Size
            /// </summary>
            public double Size = 8.28;
            /// <summary>
            /// Font StrikeThrough
            /// </summary>
            public bool StrikeThrough = false;
            /// <summary>
            /// Font Underline
            /// </summary>
            public bool UnderLine = false;
            /// <summary>
            /// Font Weight
            /// </summary>
            public int Weight = 400;

            /// <summary>
            /// Convert the Font Type of VB6 to FontStyle in .NET
            /// </summary>
            private System.Drawing.FontStyle GetStyle
            {
                get
                {
                    System.Drawing.FontStyle auxStyle = 0;
                    if (Bold)
                    {
                        auxStyle = auxStyle | System.Drawing.FontStyle.Bold;
                    }
                    if (Italic)
                    {
                        auxStyle = auxStyle | System.Drawing.FontStyle.Italic;
                    }
                    if (StrikeThrough)
                    {
                        auxStyle = auxStyle | System.Drawing.FontStyle.Strikeout;
                    }
                    if (UnderLine)
                    {
                        auxStyle = auxStyle | System.Drawing.FontStyle.Underline;
                    }
                    return auxStyle;
                }
            }

            /// <summary>
            /// Clones a StdFont
            /// </summary>
            public StdFont Clone()
            {
                StdFont tempFont = new StdFont();

                tempFont.Bold = this.Bold;
                tempFont.Italic = this.Italic;
                tempFont.Name = this.Name;
                tempFont.Size = this.Size;
                tempFont.StrikeThrough = this.StrikeThrough;
                tempFont.UnderLine = this.UnderLine;
                tempFont.CharSet = this.CharSet;
                tempFont.Weight = this.Weight;

                return tempFont;
            }

            /// <summary>
            /// Converts this Font to a .NET Font
            /// </summary>
            public System.Drawing.Font NetFont
            {
                get
                {
                    return new System.Drawing.Font(this.Name, (float)(this.Size), this.GetStyle);
                }
            }
        }
    }
}
