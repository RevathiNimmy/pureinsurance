Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmBrowser
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmBrowser
    '
    ' Date: 01/06/1999
    '
    ' Description: OLE Container Doc Viewer using Web Control
    '               The sutable viewer needs to be installed on client machine
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant to identify which class this is.
    Const ACClass As String = "frmBrowser"

    ' PRIVATE Data Members (Begin)
    Dim m_sDocumentKey As String = ""

    Dim m_bBoldStatus As Boolean
    Dim m_iMouseStatus As Integer
    Dim m_sParentsPath As String = ""

    Dim miViewerType As Integer
    ' Stores the return value for a function call.
    Private m_lReturn As Integer
    'Private objfrmParentMDI As New frmParentMDI
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property ViewerType() As Integer
        Get

            ' Return the viewer type
            Return miViewerType

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

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: DocumentOpen
    '
    ' Description: DocumentOpen
    '
    ' ***************************************************************** '
    Public Function DocumentOpen(ByVal v_sDocumentKey As String, ByVal v_sDocumentName As String, ByVal v_sParents As String, ByVal v_iFileType As Integer, ByVal v_sFilePath As String) As Integer

        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Store the Doc number to identify the form instance
            m_sDocumentKey = v_sDocumentKey
            ' Store the parents path
            m_sParentsPath = v_sParents
            ' Set form caption to name of Document
            Me.Text = "'" & v_sDocumentName & "'" & v_sParents
            Dim fileInfo As FileInfo = New FileInfo(v_sFilePath)
            fileInfo.Attributes = FileAttribute.ReadOnly ' Set Read Only attribute.
            brwWebBrowser.Navigate(New Uri(v_sFilePath))
            Me.Tag = "OLE" & v_sDocumentKey
            Return result

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Function

    Private Sub frmBrowser_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            'Enable the show OLE Toolbar button
            objfrmParentMDI.Toolbar.Items.Item("ToolBar").Enabled = True

            ' Disable the image
            objfrmParentMDI.mnuImage.Enabled = False

            ' Disable the page movement menu options
            objfrmParentMDI.mnuFirstPg.Enabled = False
            objfrmParentMDI.mnuPrevpg.Enabled = False
            objfrmParentMDI.mnuNextpg.Enabled = False
            objfrmParentMDI.mnuLastpg.Enabled = False
            objfrmParentMDI.mnuMove.Enabled = False

            ' Disable the page movement toolbar buttons
            objfrmParentMDI.Toolbar.Items.Item("FirstPage").Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("PreviousPage").Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("NextPage").Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("LastPage").Enabled = False

            ' Disable the page combo box
            objfrmParentMDI.cboPages.Items.Clear()
            objfrmParentMDI.cboPages.Enabled = False

            ' Hide the Thumbnail menu and toolbar
            objfrmParentMDI.Toolbar.Items.Item("Thumbnails").Enabled = False
            objfrmParentMDI.mnuShowThumbnails.Enabled = False

            ' Disable the rotation options
            objfrmParentMDI.mnuRotateLeft.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("RotateLeft").Enabled = False
            objfrmParentMDI.mnuRotateRight.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("RotateRight").Enabled = False

            ' Enable the copy menu option
            objfrmParentMDI.mnuCopy.Enabled = True
            objfrmParentMDI.Toolbar.Items.Item("Copy").Enabled = True

            ' Disable the move option
            objfrmParentMDI.mnuMove.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("Move").Enabled = False

            ' Disable the Fit To... buttons
            objfrmParentMDI.mnuFitHeight.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("FitHeight").Enabled = False

            objfrmParentMDI.mnuFitWidth.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("FitWidth").Enabled = False

            objfrmParentMDI.mnuFitScreen.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("FitScreen").Enabled = False


            ' For an RTF, disable the zoom, bold and font selection
            objfrmParentMDI.mnuZoom.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("Zoom").Enabled = False

            objfrmParentMDI.mnuBold.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("Bold").Enabled = False

            objfrmParentMDI.mnuNormalDisplay.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("Normal").Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("Copy").Enabled = False

            objfrmParentMDI.mnuFont.Enabled = False
            objfrmParentMDI.mnuWindow.Enabled = True

            ' Set the page number
            objfrmParentMDI.StbInfo.Items.Item(0).Text = ""

            ' Set the zoom/move
            Select Case Me.MouseStatus
                Case MOUSE_NONE
                    objfrmParentMDI.StbInfo.Items.Item(1).Text = ""
                Case MOUSE_ZOOM
                    objfrmParentMDI.StbInfo.Items.Item(1).Text = "Zoom"
            End Select

            ' Set the fit
            objfrmParentMDI.StbInfo.Items.Item(2).Text = ""

            If Me.BoldStatus Then
                objfrmParentMDI.mnuBold.Checked = True
                CType(objfrmParentMDI.Toolbar.Items.Item("Bold"), ToolStripButton).Checked = True
            Else
                objfrmParentMDI.mnuBold.Checked = False
                CType(objfrmParentMDI.Toolbar.Items.Item("Bold"), ToolStripButton).Checked = False
            End If

            If Me.MouseStatus = MOUSE_MOVE Then
                objfrmParentMDI.mnuMove.Checked = True
                CType(objfrmParentMDI.Toolbar.Items.Item("Move"), ToolStripButton).Checked = True

                objfrmParentMDI.mnuZoom.Checked = False
                CType(objfrmParentMDI.Toolbar.Items.Item("Zoom"), ToolStripButton).Checked = False
            ElseIf (Me.MouseStatus = MOUSE_ZOOM) Then
                objfrmParentMDI.mnuMove.Checked = False
                CType(objfrmParentMDI.Toolbar.Items.Item("Move"), ToolStripButton).Checked = False

                objfrmParentMDI.mnuZoom.Checked = True
                CType(objfrmParentMDI.Toolbar.Items.Item("Zoom"), ToolStripButton).Checked = True
            Else
                objfrmParentMDI.mnuMove.Checked = False
                CType(objfrmParentMDI.Toolbar.Items.Item("Move"), ToolStripButton).Checked = False

                objfrmParentMDI.mnuZoom.Checked = False
                CType(objfrmParentMDI.Toolbar.Items.Item("Zoom"), ToolStripButton).Checked = False
            End If

            Dim bEnabled As Boolean = SetWinPos(objfrmParentMDI.Handle.ToInt32())

        End If
    End Sub

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)
    ' ***************************************************************** '
    ' Name: Form_Load
    '
    ' Description: Form_Load
    '
    ' ***************************************************************** '

    Private Sub frmBrowser_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try


            m_bBoldStatus = False
            m_iMouseStatus = MOUSE_NONE

            frmBrowser_Activated(Me, New EventArgs())

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Form_Resize
    '
    ' Description: Form_Resize
    '
    ' ***************************************************************** '

    Private isInitializingComponent As Boolean
    Private Sub frmBrowser_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If


        Try

            'brwWebBrowser.Top = CInt(VB6.TwipsToPixelsY(Me.ScaleTop + 10))
            brwWebBrowser.Width = CInt(Me.ClientRectangle.Width - VB6.TwipsToPixelsX(10))
            brwWebBrowser.Height = CInt(Me.ClientRectangle.Height - VB6.TwipsToPixelsY(10))

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    Private Sub frmBrowser_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        Dim bEnable As Boolean

        ' Check to see if theres anything loaded
        bEnable = Not (Application.OpenForms.Count = 2)

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

        m_lReturn = RefreshFormControl(v_sExceptionDocumentKey:=m_sDocumentKey)

    End Sub

    ' PRIVATE Events (End)

    'UPGRADE_NOTE: (7001) The following declaration (rtbView_MouseDown) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub rtbView_MouseDown(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
    '
    ' Show the edit menu
    'If Button = MouseButtonConstants.RightButton Then
    'Ctx_frmParentMDI_mnuView.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
    'End If
    '
    'End Sub
End Class
