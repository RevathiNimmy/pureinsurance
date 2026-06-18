Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports Microsoft.Office.Interop
Imports Aspose.Words.Settings
Partial Friend Class frmChildOFF
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name     : frmChildOFF
    '
    ' Created on    : 17/12/2002
    '
    ' Description   : MDI Child form to display Office Documents.
    '
    ' Notes         : This uses a Microsoft's Office Document Viewer ActiveX Control
    '                   to display the office documents.
    '                 Ref. http://support.microsoft.com/default.aspx?scid=KB;en-us;q311765
    ' Edit History  :
    ' KR20021218    : Created.
    ' ***************************************************************** '

    ' Constant to identify which class this is.
    Const ACClass As String = "frmChildOFF"

    ' PRIVATE Data Members (Begin)
    Dim m_sDocumentKey As String = ""

    Dim m_bBoldStatus As Boolean
    Dim m_iMouseStatus As Integer
    Dim m_sParentsPath As String = ""

    ' RDC 22062005
    Private m_bAllowCopyPaste As Boolean
    'Private objfrmParentMDI As New frmParentMDI
    ' Stores the return value for a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Dim m_oWord As Word.Application = New Word.Application()
    Private m_sWordVersion As String = "15"
    Private m_lWordHwnd As Integer
    Private m_iFileType As Integer
    Private m_sClientDocument As String = ""
    Private Declare Function OpenClipboard Lib "user32" (ByVal hwnd As Integer) As Integer
    Private Declare Function CloseClipboard Lib "user32" () As Integer

    Private oMSApp As Object 'refers to Word Application object
    Private oCurrentDoc As Object 'refers to Word Document object
    Private mOCRetVal As Integer 'return value of calling the OpenClipboard api function

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property ViewerType() As Integer
        Get

            ' Return the viewer type
            Return ACViewerTypeWRD

        End Get
    End Property


    Public Property ParentsPath() As String
        Get

            Return m_sParentsPath

        End Get
        Set(ByVal Value As String)

            m_sParentsPath = Value

        End Set
    End Property


    Public Property MouseStatus() As Integer
        Get

            Return m_iMouseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iMouseStatus = Value

        End Set
    End Property

    Public Property BoldStatus() As Boolean
        Get

            Return m_bBoldStatus

        End Get
        Set(ByVal Value As Boolean)

            m_bBoldStatus = Value

        End Set
    End Property

    Public ReadOnly Property DocumentKey() As String
        Get

            Return m_sDocumentKey

        End Get
    End Property

    ' RDC 22062005
    Public WriteOnly Property AllowCopyPaste() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllowCopyPaste = Value
        End Set
    End Property

    Public Function DocumentOpen(ByVal v_sDocumentKey As String, ByVal v_sDocumentName As String, ByVal v_sParents As String, ByVal v_iFileType As Integer, ByVal v_sFilePath As Object) As Integer

        Dim result As Integer = 0

        Const wdPrintView As Integer = 3

        Dim sWindowText As String = ""
        Dim m_lWordHwnd As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the Doc number to identify the form instance
            m_sDocumentKey = v_sDocumentKey

            ' Store the parents path
            m_sParentsPath = v_sParents

            ' Set form caption to name of Document
            Me.Text = "'" & v_sDocumentName & "'" & v_sParents
            Me.Tag = "IMG" & v_sDocumentKey
            Me.WindowState = FormWindowState.Normal
            m_iFileType = v_iFileType

            'If FCOffice.ActiveDocument Is Nothing Then
            ' Process.Start(v_sFilePath)
            'Else
            m_sClientDocument = v_sFilePath
            OpenDocument()
            ' Replace the template text with real data:
            ' FCOffice.Open(v_sFilePath)
            'FCOffice.Open(document:=v_sFilePath, ReadOnly:=True)

            '  Obtain objects for automation:
            'oCurrentDoc = FCOffice.ActiveDocument 'returns Word.Document object

            'oMSApp = oCurrentDoc.Application 'returns Word.Application object
            'oCurrentDoc = FCOffice.ActiveDocument       'returns Word.Document object
            'oMSApp = oCurrentDoc.Application            'returns Word.Application object

            'If v_iFileType = ACFileTypeWRD Then

            '    ''''''''''''''''''''''''''''''''''''''''''
            '    ' Note : This code is to diable toolbar blank area,
            '    '       if we are resizing the form
            '    '''''''''''''''''''''''''''''''''''''''''''
            '    'Make Toolbar invisible

            '    For Each cmd As Object In oMSApp.CommandBars
            '        cmd.Enabled = False
            '    Next cmd
            '    '''''''''''''''''''''''''''''''''''''''''''

            '    ' Optional: Display in Print View:

            '    oMSApp.ActiveWindow.View.Type = wdPrintView

            '    ' Optional: Turn off the display of the rulers:

            '    oMSApp.ActiveWindow.DisplayRulers = False

            '    ' RDC 22062005
            '    If Not m_bAllowCopyPaste Then
            '        ' Optional: Disable the popup menu when right-clicking in a document:
            '        oCurrentDoc.CommandBars("Text").Enabled = False
            '    Else
            '        CloseClipboard()
            '    End If

            '    ' Optional: If we open a HTML document as word then, web toolbar will appear. To stop it displaying

            '    oCurrentDoc.CommandBars("Web").Enabled = False

            '    ' Prevent the document from being edited:
            '    Try
            '        'On Error Resume Next 'in case it is already protected
            '        'oCurrentDoc.Protect 1, , "mypassword"      ' This is one way to protect the document.
            '        ' Disadvantage : What will happen if the document itself
            '        '                 is password protected !!!

            '        'oCurrentDoc.Protect Type:=1 'wdAllowOnlyComments '2         ' This is the call which stop typing in while, print preview

            '        oCurrentDoc.Protect(Type:=1) ' This is the call which stop typing in while, print preview
            '    Catch
            '    End Try
            'ElseIf v_iFileType = ACFileTypeEXL Then

            '    ' Prevent the document from being edited:
            '    Try
            '        'On Error Resume Next 'in case it is already protected
            '        oMSApp.ActiveSheet.Protect(DrawingObjects:=True, Contents:=True, Scenarios:=True)
            '    Catch
            '    End Try
            'End If

            'End If

            Return result
        Catch ex As Exception
            ' Handles the error while opening the document in word format
            ' Store error number and return the execution from here - in the parent method on the basis of this error number, open this document in pdf format
            If Information.Err().Number = 5 Then
                result = Information.Err().Number
                Return result
            End If
            result = gPMConstants.PMEReturnCode.PMError

            ' Re-enable the Clipboard so other apps can now use the clipboard!
            If mOCRetVal <> 0 Then CloseClipboard()

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Open Document process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DocumentOpen", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Return result

        End Try
    End Function

    Private Sub frmChildOFF_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            ' enable the common controls (both to TIF and RTF)
            m_lReturn = CType(objfrmParentMDI.EnableCommon(), gPMConstants.PMEReturnCode)

            ' RDC 08122004 BriefcaseViewer should not do this
            If g_iUserID = USER_IS_BRIEFCASE Then
                Exit Sub
            End If

            With objfrmParentMDI

                ' Disable the image
                .mnuImage.Enabled = False

                'disable menus not needed when viewing office docs!
                .mnuView.Enabled = False
                .mnuImage.Enabled = False
                .mnuWindow.Enabled = True
                .mnuArchive.Enabled = False

                ' Disable the page movement menu options
                .mnuFirstPg.Enabled = False
                .mnuPrevpg.Enabled = False
                .mnuNextpg.Enabled = False
                .mnuLastpg.Enabled = False
                .mnuMove.Enabled = False

                ' Disable the page movement toolbar buttons
                '.Toolbar.Items.Item("FirstPage").Enabled = False
                '.Toolbar.Items.Item("PreviousPage").Enabled = False
                '.Toolbar.Items.Item("NextPage").Enabled = False
                '.Toolbar.Items.Item("LastPage").Enabled = False
                .Toolbar.Items.Item("FirstPage").Enabled = False
                .Toolbar.Items.Item("PreviousPage").Enabled = False
                .Toolbar.Items.Item("NextPage").Enabled = False
                .Toolbar.Items.Item("LastPage").Enabled = False


                ' Hide the Thumbnail menu and toolbar
                '.Toolbar.Items.Item("Thumbnails").Enabled = False
                .Toolbar.Items.Item("Thumbnails").Enabled = False
                .mnuShowThumbnails.Enabled = False

                ' Disable the page combo box
                .cboPages.Items.Clear()
                .cboPages.Items.Add("1")
                .cboPages.Text = "1"
                .cboPages.Enabled = False

                ' Disable the rotation options
                .mnuRotateLeft.Enabled = False
                '.Toolbar.Items.Item("RotateLeft").Enabled = False
                .Toolbar.Items.Item("RotateLeft").Enabled = False
                .mnuRotateRight.Enabled = False
                '.Toolbar.Items.Item("RotateRight").Enabled = False
                .Toolbar.Items.Item("RotateRight").Enabled = False

                ' Enable the copy menu option
                .mnuCopy.Enabled = False
                '.Toolbar.Items.Item("Copy").Enabled = False
                .Toolbar.Items.Item("Copy").Enabled = False

                ' Disable the move option
                objfrmParentMDI.mnuMove.Enabled = False
                'objfrmParentMDI.Toolbar.Items.Item("Move").Enabled = False
                objfrmParentMDI.Toolbar.Items.Item("Move").Enabled = False

                ' Disable the Fit To... buttons
                .mnuFitHeight.Enabled = False
                '.Toolbar.Items.Item("FitHeight").Enabled = False
                .Toolbar.Items.Item("FitHeight").Enabled = False

                .mnuFitWidth.Enabled = False
                '.Toolbar.Items.Item("FitWidth").Enabled = False
                .Toolbar.Items.Item("FitWidth").Enabled = False

                .mnuFitScreen.Enabled = False
                '.Toolbar.Items.Item("FitScreen").Enabled = False
                .Toolbar.Items.Item("FitScreen").Enabled = False

                .mnuZoom.Enabled = False
                '.Toolbar.Items.Item("Zoom").Enabled = False
                .Toolbar.Items.Item("Zoom").Enabled = False

                .mnuBold.Enabled = False
                '.Toolbar.Items.Item("Bold").Enabled = False
                .Toolbar.Items.Item("Bold").Enabled = False

                .mnuNormalDisplay.Enabled = False
                '.Toolbar.Items.Item("Normal").Enabled = False
                .Toolbar.Items.Item("Normal").Enabled = False

                .mnuFont.Enabled = False

                ' Set the page number
                .StbInfo.Items.Item(0).Text = ""

                ' Set the zoom/move
                Select Case Me.MouseStatus
                    Case MOUSE_NONE
                        .StbInfo.Items.Item(1).Text = ""
                    Case MOUSE_ZOOM
                        .StbInfo.Items.Item(1).Text = "Zoom"
                End Select

                .StbInfo.Items.Item(2).Text = ""
                .StbInfo.Refresh()

                .mnuBold.Checked = False
                'CType(.Toolbar.Items.Item("_Toolbar_Button4"), ToolStripButton).Checked = False
                '.Toolbar.Items.Item("_Toolbar_Button4").Enabled = False

                '.mnuMove.Checked = False
                'CType(.Toolbar.Items.Item("_Toolbar_Button7"), ToolStripButton).Checked = False
                '.Toolbar.Items.Item("_Toolbar_Button7").Enabled = False

                '.mnuZoom.Checked = False
                'CType(.Toolbar.Items.Item("_Toolbar_Button6"), ToolStripButton).Checked = False
                '.Toolbar.Items.Item("_Toolbar_Button6").Enabled = False

                CType(.Toolbar.Items.Item("Bold"), ToolStripButton).Checked = False
                .Toolbar.Items.Item("Bold").Enabled = False

                .mnuMove.Checked = False
                CType(.Toolbar.Items.Item("Move"), ToolStripButton).Checked = False
                .Toolbar.Items.Item("Move").Enabled = False

                .mnuZoom.Checked = False
                CType(.Toolbar.Items.Item("Zoom"), ToolStripButton).Checked = False
                .Toolbar.Items.Item("Zoom").Enabled = False
                ' CTAF 20031104 - Changed from False to True so that we can now print
                .mnuPrint.Enabled = True
                '.Toolbar.Items.Item("_Toolbar_Button28").Enabled = True
                .Toolbar.Items.Item("Print").Enabled = True

            End With

            'FCOffice.Activate()
            Application.DoEvents()

        End If
    End Sub


    Private isInitializingComponent As Boolean
    Private Sub frmChildOFF_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            ' If not minimised resize Kofax control to match
            If Me.WindowState <> FormWindowState.Minimized Then
                'FCOffice.Width = Me.Width - VB6.TwipsToPixelsX(120)
                'FCOffice.Width = Me.Width - 8
                'FCOffice.Height = Me.Height - VB6.TwipsToPixelsY(400)
                'FCOffice.Height = Me.Height - 26

            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Resize process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Resize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub frmChildOFF_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        Try
            Dim bEnable As Boolean

            If Not m_bAllowCopyPaste Then
                ' Re-enable the Clipboard when we close this form,
                '  so other apps can now use the clipboard!
                If mOCRetVal <> 0 Then CloseClipboard()
            End If

            ' Re-enable the DisplayRulers Property,
            '  since this is a permanent setting:
            If m_iFileType = ACFileTypeWRD Then

                m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)

            End If


            'FCOffice = Nothing
            Application.DoEvents()

            oMSApp = Nothing
            oCurrentDoc = Nothing

            ' Check to see if theres anything loaded
            'bEnable = Not (Application.OpenForms.Count = 2)
            If Me.MdiChildren.Length > 0 Then
                bEnable = True
            Else
                bEnable = False
            End If

            ' reflect the values on the toolbar
            For iLoop1 As Integer = 1 To objfrmParentMDI.Toolbar.Items.Count
                If objfrmParentMDI.Toolbar.Items.Item(iLoop1 - 1).Name <> "ReturnManager" Then
                    objfrmParentMDI.Toolbar.Items.Item(iLoop1 - 1).Enabled = bEnable
                End If
            Next iLoop1

            ' and the combo box
            objfrmParentMDI.cboPages.Enabled = bEnable

            ' and the menu's
            objfrmParentMDI.mnuView.Enabled = bEnable
            objfrmParentMDI.mnuImage.Enabled = bEnable
            objfrmParentMDI.mnuWindow.Enabled = bEnable
            objfrmParentMDI.mnuPrint.Enabled = bEnable
            objfrmParentMDI.mnuInfo.Enabled = bEnable
            objfrmParentMDI.mnuFileCloseAll.Enabled = bEnable
            objfrmParentMDI.mnuArchive.Enabled = bEnable 'MS 15/05/01

            m_lReturn = CType(RefreshFormControl(v_sExceptionDocumentKey:=m_sDocumentKey), gPMConstants.PMEReturnCode)
            MemoryHelper.ReleaseMemory()
        Catch excep As Exception

            bPMFunc.LogMessage(g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Child form already closed", vApp:=ACApp, vClass:=ACClass, vMethod:="frmChildOFF_Closed", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        End Try

    End Sub

    'Private Sub FCOffice_OnDocumentClosed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FCOffice.OnDocumentClosed
    '    If TypeOf (CType(sender, AxDSOFramer.AxFramerControl).ActiveDocument) Is Microsoft.Office.Interop.Word.Application Then
    '        Dim obj As Microsoft.Office.Interop.Word.Application = CType(CType(sender, AxDSOFramer.AxFramerControl).ActiveDocument, Microsoft.Office.Interop.Word.Application)
    '        obj.Documents.Close()
    '        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj)
    '    ElseIf TypeOf (CType(sender, AxDSOFramer.AxFramerControl).ActiveDocument) Is Microsoft.Office.Interop.Excel.Application Then
    '        Dim obj As Microsoft.Office.Interop.Excel.Application = CType(CType(sender, AxDSOFramer.AxFramerControl).ActiveDocument, Microsoft.Office.Interop.Excel.Application)
    '        For Each wrk As Microsoft.Office.Interop.Excel.Workbook In obj.Workbooks
    '            wrk.Close()
    '        Next
    '        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj)

    '    End If
    '    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(sender)
    'End Sub

    Public Function Initialise() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            ' RDC 22062005
            If Not m_bAllowCopyPaste Then
                ' Disable the Clipboard while this form is open:
                mOCRetVal = OpenClipboard(0)
                If mOCRetVal = 0 Then 'OpenClipboard failed. Can't disable clipboard.
                    MessageBox.Show("The Clipboard is locked by another application. Unable to view this form.", Application.ProductName)
                    ' Close this form, which will end the application if no other forms are open:
                    Me.Close()
                End If
            End If


            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Initialise process failed",
                               vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try
    End Function

    Private Function OpenDocument() As Integer
        Dim result As Integer = 0
        Dim dtPause As Date
        Dim sOptionValue As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Alix - 31/10/2002
        If m_sWordVersion.Substring(0, 1) = "8" Then

            ' Avoid message when saving document

            m_oWord.DefaultSaveFormat = "HTML"

            ' Open document, specifying HTML as default opening format

            m_oWord.Documents.Open(FileName:=m_sClientDocument, Format:=11)

        Else


            m_oWord.Documents.Open(m_sClientDocument, ConfirmConversions:=False)

        End If

        'Some documents in Word 2000 or less require a pause after opening in order to allow them to fully open
        ' Maximize window for word application and make it visible
        m_oWord.WindowState = Word.WdWindowState.wdWindowStateMaximize
        m_oWord.ActiveWindow.ActivePane.View.Zoom.Percentage = 100
        m_oWord.Visible = True
        activateDocumentWindow(m_oWord.Name.Split(".")(0))
        m_oWord.Activate()


        'PSL 24/09/2003 Issue 6085


        'm_oWord.CommandBars("Standard").Visible = True

        'm_oWord.CommandBars("Formatting").Visible = True

        Application.DoEvents()

        Return result

    End Function
    Private Function LaunchOurDoc() As Integer
        Dim result As Integer = 0
        'TN20010711
        Dim sWindowText As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' SET 18/10/2004 ISS13245 - launch word            
        m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)


        'Open current document.
        m_lReturn = OpenDocument()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'For Debug.
        'm_oWord.Visible = True            
        Return result

    End Function
    Public Sub activateDocumentWindow(ByVal docname As String) ' code to activate the document window through loop(by catching all running process's main window title name one by one)
        Try
            Dim alllocal As Process() = Process.GetProcesses()
            Dim item As Process

            For Each item In alllocal
                If item.MainWindowTitle.ToString <> "" Then
                    If InStr(item.MainWindowTitle.ToString, docname) > 0 Then
                        Dim intourdoc As Integer = item.Id
                        AppActivate(intourdoc)
                        Exit For
                    End If
                End If
            Next
        Catch
            'Do Nothing
        End Try

    End Sub

End Class
