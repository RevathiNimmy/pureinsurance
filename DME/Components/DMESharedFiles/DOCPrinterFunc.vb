Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.VB
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports SharedFiles
Imports Microsoft.Office.Interop

Public Module PrinterFunc

    ' ***************************************************************** '
    ' Module Name: PrinterFunc
    '
    ' Date: 09/06/1998
    '
    ' Description: Contains all the functions needed to print RTF, TXT, and
    '              TIF images. Needs the Kofax controls installed, and
    '              referenced.
    '
    ' Edit History: 9/6/98  Created - C.Field
    '
    ' ***************************************************************** '

    ' Constant for the methods to identify
    ' which class this is.
    Private Const ACClass As String = "PrinterFunc"

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_iStartPage As Integer
    Private m_iEndPage As Integer

    Private m_iCurrentPrintPage As Integer
    Private m_iPagesToPrint As Integer
    Private m_vFileNames As Object

    ' RAM20021218 :  Late binding

    Private m_oWord As Word.Application ' Word.Application
    Private m_oDocument As Object ' Word.Document

    Private m_lWordHwnd As Integer


    '******************************
    ' This development been done due to excel printing
    ' feature regarding PN 45500
    '*****************************

    Private Xl As Excel.Application ' Excel.Application


    ' Constants for the printing
    Private Const WM_USER As Integer = &H400S
    Private Const EM_FORMATRANGE As Integer = WM_USER + 57
    Private Const EM_SETTARGETDEVICE As Integer = WM_USER + 72
    Private Const PHYSICALOFFSETX As Integer = 112
    Private Const PHYSICALOFFSETY As Integer = 113

    Private Structure Rect
        Dim Left As Integer
        Dim Top As Integer
        Dim Right As Integer
        Dim Bottom As Integer
    End Structure

    Private Structure CharRange
        Dim cpMin As Integer ' First character of range (0 for start of doc)
        Dim cpMax As Integer ' Last character of range (-1 for end of doc)
    End Structure

    Private Structure FormatRange
        Dim hdc As Integer ' Actual DC to draw on
        Dim hdcTarget As Integer ' Target DC for determining text formatting
        Dim rc As Rect ' Region of the DC to draw to (in twips)
        Dim rcPage As Rect ' Region of the entire DC (page size) (in twips)
        Dim chrg As CharRange ' Range of text to draw (see above declaration)
        Public Shared Function CreateInstance() As FormatRange
            Dim result As New FormatRange
            Return result
        End Function
    End Structure

    ' Win32 API Calls required by PrintTIF
    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal msg As Integer, ByVal wp As Integer, ByVal lp As Integer) As Integer

    ' Win32 API Calls required by PrintTIF
    Private Declare Function GetDeviceCaps Lib "gdi32" (ByVal hdc As Integer, ByVal nIndex As Integer) As Integer

    ' Win32 API Calls required by PrintTIF
    Private Declare Function CreateDC Lib "gdi32" Alias "CreateDCA" (ByVal lpDriverName As String, ByVal lpDeviceName As String, ByVal lpOutput As Integer, ByVal lpInitData As Integer) As Integer

    Public Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Declare Function IsWindow Lib "user32.dll" (ByVal hwnd As Integer) As Integer

    ' ********************************************************************
    '
    ' PrintRTF - Prints the contents of a RichTextBox control using the
    '            provided margins
    '
    ' RTF - A RichTextBox control to print
    '
    ' LeftMarginWidth - Width of desired left margin in twips
    '
    ' TopMarginHeight - Height of desired top margin in twips
    '
    ' RightMarginWidth - Width of desired right margin in twips
    '
    ' BottomMarginHeight - Height of desired bottom margin in twips
    '
    ' Notes - If you are also using WYSIWYG_RTF() on the provided RTF
    '         parameter you should specify the same LeftMarginWidth and
    '         RightMarginWidth that you used to call WYSIWYG_RTF()
    ' ********************************************************************



    '******************************
    ' This development been done due to excel printing
    ' feature regarding PN 45500
    '*****************************

    Private sFileTypeEx As String = ""
    Private sFileCacheName As String = ""

    Public Property sGetFileType() As String
        Get
            Return sFileTypeEx
        End Get
        Set(ByVal Value As String)
            sFileTypeEx = Value
        End Set
    End Property

    Public Property sGetFileCacheName() As String
        Get
            Return sFileCacheName
        End Get
        Set(ByVal Value As String)
            sFileCacheName = Value
        End Set
    End Property





    Public Function PrintRTF(ByRef RTF As RichTextBox, ByRef LeftMarginWidth As Integer, ByRef TopMarginHeight As Double, ByRef RightMarginWidth As Double, ByRef BottomMarginHeight As Double) As Integer

        Dim result As Integer = 0
        Dim LeftOffset, TopOffset, LeftMargin, TopMargin, RightMargin, BottomMargin As Integer
        Dim fr As FormatRange = FormatRange.CreateInstance()
        Dim rcDrawTo As New Rect
        Dim rcPage As New Rect
        Dim TextLength, NextCharPosition, r As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start a print job to get a valid Printer.hDC
            PrinterHelper.Printer.Print(New String(" "c, 1))

            'developer guide no. 217
            'PrinterHelper.Printer.ScaleMode = vbTwips
            PrinterHelper.Printer.ScaleMode = 1

            ' Get the offsett to the printable area on the page in twips


            'LeftOffset = CInt(PrinterHelper.Printer.ScaleX(GetDeviceCaps(PrinterHelper.Printer.hDC, PHYSICALOFFSETX), vbPixels, vbTwips))
            'developer guide no. 217
            LeftOffset = CInt(PrinterHelper.Printer.ScaleX(GetDeviceCaps(PrinterHelper.Printer.hDC, PHYSICALOFFSETX), 3, 1))


            'TopOffset = CInt(PrinterHelper.Printer.ScaleY(GetDeviceCaps(PrinterHelper.Printer.hDC, PHYSICALOFFSETY), vbPixels, vbTwips))
            'developer guide no. 217
            TopOffset = CInt(PrinterHelper.Printer.ScaleY(GetDeviceCaps(PrinterHelper.Printer.hDC, PHYSICALOFFSETY), 3, 1))

            ' Calculate the Left, Top, Right, and Bottom margins
            LeftMargin = LeftMarginWidth - LeftOffset
            TopMargin = CInt(TopMarginHeight - TopOffset)
            RightMargin = CInt((PrinterHelper.Printer.Width - RightMarginWidth) - LeftOffset)
            BottomMargin = CInt((PrinterHelper.Printer.Height - BottomMarginHeight) - TopOffset)

            ' Set printable area rect
            rcPage.Left = 0
            rcPage.Top = 0

            'TODO:Needs to be done
            'rcPage.Right = CInt(Printer.ScaleWidth)

            ''TODO:Needs to be done
            'rcPage.Bottom = CInt(Printer.ScaleHeight)

            ' Set rect in which to print (relative to printable area)
            rcDrawTo.Left = LeftMargin
            rcDrawTo.Top = TopMargin
            rcDrawTo.Right = RightMargin
            rcDrawTo.Bottom = BottomMargin

            ' Set up the print instructions
            fr.hdc = PrinterHelper.Printer.hDC ' Use the same DC for measuring and rendering
            fr.hdcTarget = PrinterHelper.Printer.hDC ' Point at printer hDC
            fr.rc = rcDrawTo ' Indicate the area on page to draw to
            fr.rcPage = rcPage ' Indicate entire size of page
            fr.chrg.cpMin = 0 ' Indicate start of text through
            fr.chrg.cpMax = -1 ' end of the text
            ' Get length of text in RTF

            TextLength = Strings.Len(RTF.Text)


            ' Loop printing each page until done
            Do
                ' Print the page by sending EM_FORMATRANGE message
                Dim handle As GCHandle = GCHandle.Alloc(fr, GCHandleType.Pinned)
                Try
                    Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()


                    NextCharPosition = SendMessage(tmpPtr, EM_FORMATRANGE, True, tmpPtr)
                Finally
                    handle.Free()
                End Try
                If NextCharPosition >= TextLength Then Exit Do 'If done then exit
                fr.chrg.cpMin = NextCharPosition ' Starting position for next page
                PrinterHelper.Printer.NewPage() ' Move on to next page
                PrinterHelper.Printer.Print(New String(" "c, 1)) ' Re-initialize hDC
                fr.hdc = PrinterHelper.Printer.hDC
                fr.hdcTarget = PrinterHelper.Printer.hDC
            Loop


            ' Commit the print job
            PrinterHelper.Printer.EndDoc()

            ' Allow the RTF to free up memory
            Dim handle2 As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
            Try
                Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()

                r = SendMessage(tmpPtr2, EM_FORMATRANGE, False, tmpPtr2)
            Finally
                handle2.Free()
            End Try

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.

            'TODO: Needs to checked
            'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to print RTF document '" & RTF. & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRTF", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: OpenDocument
    '
    ' Description: Open the selected document
    '
    ' ***************************************************************** '
    Private Function OpenDocument(ByRef sDocument As String) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDocument = m_oWord.Documents.Open(sDocument, ConfirmConversions:=False)
            Application.DoEvents()

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: LaunchOurDoc
    '
    ' Description:  Runs word and sets required document as current.
    '
    '
    ' ***************************************************************** '
    Private Function LaunchOurDoc(ByRef sDocument As String) As Integer
        Dim result As Integer = 0
        Dim sWindowText As String = ""

        

            result = gPMConstants.PMEReturnCode.PMTrue

            'Launch Word.
            m_oWord = New Word.Application()


            sWindowText = m_oWord.Caption

            m_oWord.Caption = "Tinny Boy"

            m_lWordHwnd = FindWindow(Nothing, "Tinny Boy")


            m_oWord.Caption = sWindowText

            If m_lWordHwnd = 0 Then
                MessageBox.Show("Failed To Get Word Handle", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Open current document.
            m_lReturn = CType(OpenDocument(sDocument), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: OurDocIsRunning
    '
    ' Description: Checks to see if our document is still open. Does this by
    '               trying to access ActiveDocument property of module level
    '               Word object. If an error occurs, we assume document has
    '               already been closed down.
    '
    ' ***************************************************************** '
    Private Function OurDocIsRunning(ByRef sDocument As String) As Integer

        Dim result As Integer = 0
        Dim sTest As String = ""
        Dim iDocNum As Integer

        

            result = gPMConstants.PMEReturnCode.PMFalse

            If Not (m_oWord Is Nothing) Then


                iDocNum = m_oWord.Documents.Count

                For iCount As Integer = 1 To iDocNum

                    sTest = m_oWord.Documents.Item(iCount).FullName
                    If sTest.ToLower() = sDocument.ToLower() Then
                        result = gPMConstants.PMEReturnCode.PMTrue
                    End If
                Next

            End If

            Return result


    End Function

    ' ***************************************************************** '
    ' Name: PrintDocumentSilent
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function PrintDocumentSilent(ByRef sDocument As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If OurDocIsRunning(sDocument) = gPMConstants.PMEReturnCode.PMFalse Then
                If OurInstanceOfWordIsRunning() = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(OpenDocument(sDocument), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    m_lReturn = CType(LaunchOurDoc(sDocument), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            'Run Document Manager Macro to print document.

            m_oWord.Run("Normal.PMDocumentManager.PMBPrintDocumentSilent")

            m_lReturn = CType(ShutItDown(sDocument), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocumentSilent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: OurInstanceOfWordIsRunning
    '
    ' Description: Checks to see if the instance of word we created to
    '               edit or print a document is still running.
    '
    '
    ' ***************************************************************** '
    Private Function OurInstanceOfWordIsRunning() As Integer

        Dim sTest As String = ""

        Try

            'is our word still running?
            m_lReturn = CType(IsWindow(m_lWordHwnd), gPMConstants.PMEReturnCode)


            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try




        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' ***************************************************************** '
    '
    ' Name: ShutItDown
    '
    ' Description: Shuts down the instance of word that has been open
    '
    '
    ' ***************************************************************** '
    Public Function ShutItDown(ByRef sDocument As String) As Integer

        Dim result As Integer = 0

        Dim sTemp As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'is our word session still open
            m_lReturn = CType(IsWindow(m_lWordHwnd), gPMConstants.PMEReturnCode)

            'yeap its still open
            If m_lReturn <> gPMConstants.PMEReturnCode.PMFalse Then
                'do we have any document open
                If OurDocIsRunning(sDocument) = gPMConstants.PMEReturnCode.PMTrue Then

                    m_oWord.Documents.Close(SaveChanges:=True)
                End If


                m_oWord.Quit()

                m_oWord = Nothing
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to shut it down", vApp:=ACApp, vClass:=ACClass, vMethod:="ShutItDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' *****************************************************************
    '
    ' Name: eXcelPrint
    '
    ' Description: Making Printout for excel worksheet from Docmanager
    ' Date : 11-09-2008
    ' Author : Saurabh Singh
    ' PN : 45500
    ' *****************************************************************


    Public Function eXcelPrint(ByRef sDocument As String) As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            Xl = New Excel.Application() ' For Excel

            Xl.Workbooks.Open(sDocument)

            Dim SheetCount As Integer
            Dim CurrentSheet As Object 'Worksheet


            For i1 As Integer = 1 To Xl.ActiveWorkbook.Worksheets.Count

                CurrentSheet = Xl.ActiveWorkbook.Worksheets(i1)
                '  Skip empty sheets and hidden sheets



                If Xl.Application.CountA(CurrentSheet.Cells) <> 0 And CurrentSheet.Visible Then
                    SheetCount += 1
                End If
            Next i1

            If SheetCount <> 0 Then


                For i1 As Integer = 1 To Xl.ActiveWorkbook.Worksheets.Count

                    CurrentSheet = Xl.ActiveWorkbook.Worksheets(i1)
                    ' Skip empty sheets and hidden sheets



                    If Xl.Application.CountA(CurrentSheet.Cells) <> 0 And CurrentSheet.Visible Then


                        CurrentSheet.PrintOut()
                        'Xl.Worksheets(i1).Activate
                        'Xl.ActiveSheet.PrintPreview 'for debugging
                        'Xl.ActiveSheet.PrintOut

                    End If
                Next i1
            Else
                MessageBox.Show("All worksheets are empty.", Application.ProductName)
            End If

            '   Reactivate original sheet

            CurrentSheet.Activate()

            Xl.Quit()
            Xl = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            Xl = Nothing
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to print the worksheet", vApp:=ACApp, vClass:=ACClass, vMethod:="eXcelPrint", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: GetFileExtension for Excel printing
    '
    ' Description: GetFileExtension4Excl
    '
    ' ***************************************************************** '
    Public Function GetFileExtension4Excl(ByVal v_sFileName As String) As Integer

        Dim result As Integer = 0
        Dim sFilename, sFileExtension As String
        Dim lExtLen As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sFilename = v_sFileName.Trim()

            lExtLen = 0

            ' Find the position of the last dot in the filename
            ' and get the length of the file extension from that
            For lPos As Integer = sFilename.Length To 1 Step -1
                If Mid(sFilename, lPos, 1) = "." Then
                    lExtLen = sFilename.Length - lPos
                    Exit For
                End If
            Next

            ' Get trailing chars up to dot and convert to uppercase
            If lExtLen > 0 Then
                sFileExtension = sFilename.Substring(sFilename.Length - lExtLen).ToUpper()
            Else
                sFileExtension = ""
            End If

            ' Set returned file type
            Select Case sFileExtension
                Case "TIF", "TIFF"
                    sGetFileType = "TIF"
                Case "RTF"
                    sGetFileType = "RTF"
                Case "TXT", "TEXT", "ASCI"
                    sGetFileType = "TXT"
                Case "DOC", "DOCX", "DOT", "DOTX", "ASC", "ANS", "MCW", "WPS" ' WORD FILES
                    sGetFileType = "DOC"
                Case "XLS", "XLSX", "XLT", "XLS", "CSV", "WK1", "WK2", "WK3", "WK4", "WQ1", "PRN", "DIF", "SLK", "XLA", "TAB" 'SOB 01/06/99 EXCEL Files
                    sGetFileType = "XLS"
                Case "PPT", "PPTX", "POT", "POTX", "PPS", "PPSX", "PPA" ' Power Point Files
                    sGetFileType = "PPT"
                Case "MDB", "ADP", "MDW", "MDA", "MDE", "ADE", "DBF", "DB" ' Ms Access Files
                    sGetFileType = "MDB"
                Case "HTM", "HTML", "SHTM", "SHTML", "STM", "ASP", "HTT", "CSS", "CFML", "XML" 'IE, Netscape Files
                    sGetFileType = "HTML"
                Case "GIF", "GIFF"
                    sGetFileType = "GIF" ' GIF Files
                Case "JPEG", "JPG"
                    sGetFileType = "JPG"
                Case "EML", "OFT", "MSG", "EML" ' E-Mail Doc
                    sGetFileType = "EML"
                Case "PDF"
                    sGetFileType = "PDF" ' Adobe Accrobat Files
                Case "HLP"
                    sGetFileType = "HLP" ' Help Files
                Case "ZIP", "GZ"
                    sGetFileType = "ZIP" 'ZIP Files
                Case "BMP"
                    sGetFileType = "BMP" ' Bitmap files
                Case Else
                    sGetFileType = ""
            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFileExtension process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFileExtension", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
