Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Drawing.Printing
Partial Friend Class frmChildTIF
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmChildTIF
    '
    ' Date: 14/05/1998
    '
    ' Description: TIF Viewer Child Form
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant to identify which class this is.
    Const ACClass As String = "frmChildTIF"

    ' PRIVATE Data Members (Begin)
    Dim m_sDocumentKey As String = ""
    Dim m_bMultiFileDocument As Boolean

    ' Local copy of file array starting at 1 to sync with page numbers
    Dim m_vFileArray() As Object

    ' Total number of pages
    Dim m_iTotalPages As Integer

    ' Currently displayed page number
    Dim m_iCurrentPage As Integer

    ' Stores the return value for a function call.
    Private m_lReturn As Integer
    ' Private objfrmParentMDI As New frmParentMDI
    Private Const m_lThumbWidth As Integer = 2175
    Private m_bThumbnails As Boolean

    Dim m_iFitStatus As Integer
    Dim m_bBoldStatus As Boolean
    Dim m_iMouseStatus As Integer
    Dim m_sParentsPath As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)


    Public Property ParentsPath() As String
        Get

            Return m_sParentsPath

        End Get
        Set(ByVal Value As String)

            m_sParentsPath = Value

        End Set
    End Property

    Public ReadOnly Property DocumentKey() As String
        Get

            Return m_sDocumentKey

        End Get
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


    Public Property FitStatus() As Integer
        Get

            Return m_iFitStatus

        End Get
        Set(ByVal Value As Integer)

            m_iFitStatus = Value

        End Set
    End Property

    Public ReadOnly Property ViewerType() As Integer
        Get

            ' Return the viewer type
            Return ACViewerTypeTIF

        End Get
    End Property

    Public ReadOnly Property MultiFileDocument() As Boolean
        Get

            Return m_bMultiFileDocument

        End Get
    End Property

    Public ReadOnly Property PageTotal() As Integer
        Get

            ' Return the total number of pages in current doc
            Return m_iTotalPages

        End Get
    End Property

    Public ReadOnly Property PageDisplayed() As Integer
        Get

            ' Return the currently displayed page number
            Return m_iCurrentPage

        End Get
    End Property

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    Public Function GetPageFilenames(ByRef vFileNames() As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the array
            vFileNames = VB6.CopyArray(m_vFileArray)


            If vFileNames Is Nothing Then
                ReDim vFileNames(0)

                'TODO
                'vFileNames(0) = ImageKit1.FileName
                vFileNames(0) = ImageKit1.File.FileName
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get page filenames", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPageFilenames", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         DocumentOpen
    '
    ' Description:  DocumentOpen
    '
    ' ***************************************************************** '
    Public Function DocumentOpen(ByVal v_sDocumentKey As String, ByVal v_sDocumentName As String, ByVal v_sParents As String, ByVal v_vFileArray As Object) As Integer

        Dim result As Integer = 0
        Dim lFirstFileIndex, lLastFileIndex As Integer
        Dim sPath As String = ""
        Dim sFileExt As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the Doc number to identify the form instance
            m_sDocumentKey = v_sDocumentKey

            ' Store the parents path
            m_sParentsPath = v_sParents

            ' Set form caption to name of Document
            Me.Text = "'" & v_sDocumentName & "'" & v_sParents


            lFirstFileIndex = v_vFileArray.GetLowerBound(0)

            lLastFileIndex = v_vFileArray.GetUpperBound(0)

            ' File Array will contain either
            ' several single page TIFs
            ' or a single multi or single page TIF.
            ' Find out which, set flag and determine page count.

            m_bMultiFileDocument = lFirstFileIndex <> lLastFileIndex


            'Set current page index to 1
            'TODO
            m_iCurrentPage = 1

            ' Setup m_vFileArray to start at 1 ie same as page num

            m_lReturn = SetupBase1Array(v_vFileArray:=v_vFileArray)

            sPath = CStr(m_vFileArray(m_iCurrentPage)).Trim()
            sPath = sPath.Substring(0, IIf(sPath = "" And "\" = "", 0, (sPath.LastIndexOf("\") + 1)))

            'Set the thumbnails
            IkThumb1.FilePath = sPath

            sFileExt = New StringBuilder("")
            m_iTotalPages = 0
            For Each m_vFileArray_item As Object In m_vFileArray

                If Not Information.IsNothing(m_vFileArray_item) Then
                    'Total the number of pages in each document we are using.
                    'TODO 
                    'Starts
                    'ImageKit1.FileName = CStr(m_vFileArray_item).Trim()
                    'ImageKit1.LoadPage = 0
                    'ImageKit1.GetImagefileType()
                    'm_iTotalPages += ImageKit1.FileMaxPage
                    ImageKit1.File.FileName = CStr(m_vFileArray_item).Trim()
                    ImageKit1.File.LoadPage = 0
                    ImageKit1.File.GetImageFileType()
                    m_iTotalPages += ImageKit1.File.FileMaxPage
                    'IkCommon1.ImgHandle = ImageKit1.ImgHandle
                    'IkCommon1.FreeMemory()
                    'ImageKit1.ImgHandle = 0
                    'Ends

                    'Add file to list
                    If sFileExt.ToString() <> "" Then
                        sFileExt.Append(";")
                    End If
                    sFileExt.Append(Mid(CStr(m_vFileArray_item), (IIf(CStr(m_vFileArray_item) = "" And "\" = "", 0, (CStr(m_vFileArray_item).LastIndexOf("\") + 1))) + 1))
                End If
            Next m_vFileArray_item

            'TODO
            'Starts
            'IkThumb1.FileExt = sFileExt.ToString()
            IkThumb1.FileExtension = sFileExt.ToString()
            'IkThumb1.ReadMode = IMGKIT6Lib.ThumbnailReadModeConstants.ikThumbRdMultiImg
            'Ends

            IkThumb1.GetFiles()
            IkThumb1.Display()

            'Open the first page of the first file
            m_lReturn = FileOpen(v_iFileIndex:=m_iCurrentPage)

            Me.Tag = "TIF" & v_sDocumentKey

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Open Document process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DocumentOpen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function AdjustThumbnail() As Integer

        Try

            If m_bThumbnails Then

                IkThumb1.SelectImage(CShort(m_iCurrentPage))

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AdjustThumbnail failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AdjustThumbnail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: DisplayNextPage
    '
    ' Description: DisplayNextPage
    '
    ' ***************************************************************** '
    Public Function DisplayNextPage() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_iCurrentPage < m_iTotalPages Then
                m_iCurrentPage += 1
            End If

            m_lReturn = PageDisplay()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Adjust the thumbnail document Accordingly
            AdjustThumbnail()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayNextPage process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayNextPage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayPreviousPage
    '
    ' Description: DisplayPreviousPage
    '
    ' ***************************************************************** '
    Public Function DisplayPreviousPage() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_iCurrentPage > 1 Then
                m_iCurrentPage -= 1
            End If

            m_lReturn = PageDisplay()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Adjust the thumbnail document Accordingly
            AdjustThumbnail()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayPreviousPage process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayPreviousPage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayFirstPage
    '
    ' Description: DisplayFirstPage
    '
    ' ***************************************************************** '
    Public Function DisplayFirstPage() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_iCurrentPage = 1

            m_lReturn = PageDisplay()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Adjust the thumbnail document Accordingly
            AdjustThumbnail()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayFirstPage process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayFirstPage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayPage
    '
    ' Description: DisplayPage
    '
    ' ***************************************************************** '
    Public Function DisplayPage(ByRef iPageNum As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If a strange page number is passed then...

            ' If its less than 1, set to first page
            If iPageNum < 1 Then
                iPageNum = 1
            End If

            ' If its more than the available pages, set to last page
            If iPageNum > m_iTotalPages Then
                iPageNum = m_iTotalPages
            End If

            m_iCurrentPage = iPageNum

            m_lReturn = PageDisplay()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bDontUpdate = True
            objfrmParentMDI.cboPages.SelectedIndex = m_iCurrentPage - 1
            bDontUpdate = False

            ' Adjust the thumbnail document Accordingly
            AdjustThumbnail()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayPage process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayPage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLastPage
    '
    ' Description: DisplayLastPage
    '
    ' ***************************************************************** '
    Public Function DisplayLastPage() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_iCurrentPage = m_iTotalPages

            m_lReturn = PageDisplay()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Adjust the thumbnail document Accordingly
            AdjustThumbnail()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayLastPage process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLastPage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: PageDisplay
    '
    ' Description: PageDisplay
    '
    ' ***************************************************************** '
    Private Function PageDisplay() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = FileOpen(v_iFileIndex:=m_iCurrentPage)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PageDisplay process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PageDisplay", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FileOpen
    '
    ' Description: FileOpen
    '
    ' ***************************************************************** '
    Private Function FileOpen(ByVal v_iFileIndex As Integer, Optional ByVal v_bOpenOnly As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sFilePath As String = ""
        Dim lCurrentPageTotal As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TODO
            'Starts
            'If ImageKit1.ImgHandle <> 0 Then
            '    IkCommon1.ImgHandle = ImageKit1.ImgHandle
            '    IkCommon1.FreeMemory()
            '    ImageKit1.ImgHandle = 0
            'End If
            'If ImageKit1.ImgHandle <> 0 Then
            '    IkCommon1.ImgHandle = ImageKit1.ImgHandle
            '    IkCommon1.FreeMemory()
            '    ImageKit1.ImgHandle = 0
            'End If
            'Ends
            'Open the required TIF file

            'TODO
            'If m_bMultiFileDocument Then

            '    lCurrentPageTotal = 0
            '    For Each m_vFileArray_item As Object In m_vFileArray

            '        'Total the number of pages in each document we are using.
            '        ImageKit1.FileName = CStr(m_vFileArray_item).Trim()
            '        ImageKit1.LoadPage = 0
            '        ImageKit1.GetImagefileType()

            '        If v_iFileIndex - lCurrentPageTotal <= ImageKit1.FileMaxPage Then
            '            ImageKit1.LoadPage = CShort(v_iFileIndex - lCurrentPageTotal - 1)
            '            Exit For
            '        Else
            '            lCurrentPageTotal += ImageKit1.FileMaxPage
            '        End If
            '    Next m_vFileArray_item

            'Else

            '    sFilePath = CStr(m_vFileArray(1)).Trim()
            '    ImageKit1.FileName = sFilePath
            '    ImageKit1.LoadPage = CShort(v_iFileIndex - 1)

            'End If

            If m_bMultiFileDocument Then

                lCurrentPageTotal = 0
                For Each m_vFileArray_item As Object In m_vFileArray

                    If Not Information.IsNothing(m_vFileArray_item) Then

                        'Total the number of pages in each document we are using.
                        ImageKit1.File.FileName = CStr(m_vFileArray_item).Trim()
                        ImageKit1.File.LoadPage = 0
                        ImageKit1.File.GetImageFileType()

                        If v_iFileIndex - lCurrentPageTotal <= ImageKit1.File.FileMaxPage Then
                            ImageKit1.File.LoadPage = CShort(v_iFileIndex - lCurrentPageTotal - 1)
                            Exit For
                        Else
                            lCurrentPageTotal += ImageKit1.File.FileMaxPage
                        End If
                    End If
                Next m_vFileArray_item

            Else

                sFilePath = CStr(m_vFileArray(1)).Trim()
                ImageKit1.File.FileName = sFilePath
                ImageKit1.File.LoadPage = CShort(v_iFileIndex - 1)

            End If

            'TODO
            'ImageKit1.LoadFile(IMGKIT6Lib.LoadFileConstants.ikLoad)
            ImageKit1.File.LoadImageFromFile(Newtone.ImageKit.LoadFileType.LoadTIFF)
            ImageKit1.Display(Newtone.ImageKit.Win.DisplayMode.ActualSize)
            If Not v_bOpenOnly Then
                'TODO
                'ImageKit1.ImgHandle = ImageKit1.ImgHandle

                objfrmParentMDI.SetFitStatus(m_iFitStatus)
                objfrmParentMDI.SetMouseStatus(m_iMouseStatus)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FileOpen process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FileOpen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetupBase1Array
    '
    ' Description: Setup a module level array starting from 1 so
    '              that Page number matches index
    '
    ' ***************************************************************** '
    Private Function SetupBase1Array(ByVal v_vFileArray() As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have a zero based array shift up by 1
            If v_vFileArray.GetLowerBound(0) = 0 Then

                ReDim m_vFileArray(v_vFileArray.GetUpperBound(0) + 1)

                For lIndex As Integer = 0 To v_vFileArray.GetUpperBound(0)

                    m_vFileArray(lIndex + 1) = v_vFileArray(lIndex)

                Next

                ' Otherwise we already have a base one array so pass it on
            Else

                ReDim m_vFileArray(v_vFileArray.GetUpperBound(0))

                m_vFileArray = v_vFileArray

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetupBase1Array process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetupBase1Array", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmChildTIF_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Dim bEnabled As Boolean

            ' enable the common controls (both to TIF and RTF)
            m_lReturn = objfrmParentMDI.EnableCommon()

            ' Add the number of pages to the combo box
            objfrmParentMDI.cboPages.Items.Clear()
            For iLoop1 As Integer = 1 To m_iTotalPages
                objfrmParentMDI.cboPages.Items.Add(CStr(iLoop1))
            Next

            ' PW180603 - CQ1155 - check if any pages before setting number
            If Me.PageTotal > 0 Then
                objfrmParentMDI.cboPages.SelectedIndex = m_iCurrentPage
            End If

            ' Check to see if the thumbnails are activated. If not, disable the control
            m_bThumbnails = objfrmParentMDI.mnuShowThumbnails.Checked

            m_lReturn = DisplayThumbs(v_bDisplay:=m_bThumbnails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Not important
            End If

            ' Show the Thumbnail menu and toolbar
            objfrmParentMDI.Toolbar.Items.Item("Thumbnails").Enabled = True
            objfrmParentMDI.mnuShowThumbnails.Enabled = True

            objfrmParentMDI.mnuFont.Enabled = False
            objfrmParentMDI.mnuImage.Enabled = True

            objfrmParentMDI.mnuNormalDisplay.Enabled = True

            ' Multi page? If not then we dont want the nagivate buttons
            bEnabled = (Me.PageTotal > 1)

            ' Disable/Enable the page movement menu options
            objfrmParentMDI.mnuFirstPg.Enabled = bEnabled
            objfrmParentMDI.mnuPrevpg.Enabled = bEnabled
            objfrmParentMDI.mnuNextpg.Enabled = bEnabled
            objfrmParentMDI.mnuLastpg.Enabled = bEnabled
            objfrmParentMDI.mnuMove.Enabled = bEnabled

            ' Disable/Enable the page movement toolbar buttons
            objfrmParentMDI.Toolbar.Items.Item("FirstPage").Enabled = bEnabled
            objfrmParentMDI.Toolbar.Items.Item("PreviousPage").Enabled = bEnabled
            objfrmParentMDI.Toolbar.Items.Item("NextPage").Enabled = bEnabled
            objfrmParentMDI.Toolbar.Items.Item("LastPage").Enabled = bEnabled

            ' Enable the page combo box
            objfrmParentMDI.cboPages.Enabled = bEnabled

            ' Disable the copy button
            objfrmParentMDI.mnuCopy.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("Copy").Enabled = False

            ' Enable the move button/menu
            objfrmParentMDI.mnuMove.Enabled = True
            objfrmParentMDI.Toolbar.Items.Item("Move").Enabled = True

            ' Enable the zoom button/menu
            objfrmParentMDI.mnuZoom.Enabled = True
            objfrmParentMDI.Toolbar.Items.Item("Zoom").Enabled = True

            ' Enable the bold button/menu
            objfrmParentMDI.mnuBold.Enabled = False
            objfrmParentMDI.Toolbar.Items.Item("Bold").Enabled = False

            ' Disable the rotation options
            objfrmParentMDI.mnuRotateLeft.Enabled = True
            objfrmParentMDI.Toolbar.Items.Item("RotateLeft").Enabled = True
            objfrmParentMDI.mnuRotateRight.Enabled = True
            objfrmParentMDI.Toolbar.Items.Item("RotateRight").Enabled = True

            ' Enable the Fit To... buttons
            objfrmParentMDI.mnuFitHeight.Enabled = True
            objfrmParentMDI.Toolbar.Items.Item("FitHeight").Enabled = True

            objfrmParentMDI.mnuFitWidth.Enabled = True
            objfrmParentMDI.Toolbar.Items.Item("FitWidth").Enabled = True

            objfrmParentMDI.mnuFitScreen.Enabled = True
            objfrmParentMDI.Toolbar.Items.Item("FitScreen").Enabled = True

            objfrmParentMDI.mnuNormalDisplay.Enabled = True
            objfrmParentMDI.Toolbar.Items.Item("Normal").Enabled = True

            objfrmParentMDI.SetFitStatus(m_iFitStatus, True)
            objfrmParentMDI.SetMouseStatus(m_iMouseStatus)

            objfrmParentMDI.StbInfo.Items.Item(0).Text = "Page " & _
                                                      Me.PageDisplayed & " of " & _
                                                      Me.PageTotal

            If Me.BoldStatus Then
                objfrmParentMDI.mnuBold.Checked = True
                CType(objfrmParentMDI.Toolbar.Items.Item("Bold"), ToolStripButton).Checked = True
            Else
                objfrmParentMDI.mnuBold.Checked = False
                CType(objfrmParentMDI.Toolbar.Items.Item("Bold"), ToolStripButton).Checked = False
            End If

            objfrmParentMDI.Toolbar.Items.Item("ToolBar").Enabled = False

            bEnabled = SetWinPos(objfrmParentMDI.Handle.ToInt32())

        End If
    End Sub

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    ' ***************************************************************** '
    ' Name: Form_Load
    '
    ' Description: Form_Load
    '
    ' ***************************************************************** '

    Private Sub frmChildTIF_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            m_bBoldStatus = False

            m_iFitStatus = FIT_WIDTH
            m_iMouseStatus = MOUSE_MOVE

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
    Private Sub frmChildTIF_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Const c_lImageHeight As Integer = 2165

        Dim lHeight, lRows As Integer

        Try

            If Me.WindowState <> FormWindowState.Minimized Then

                'Check for minimum height
                If VB6.PixelsToTwipsY(Me.Height) < 3300 Then
                    Me.Height = VB6.TwipsToPixelsY(3300)
                End If

                'Check for minimum width
                If VB6.PixelsToTwipsX(Me.Width) < 2300 Then
                    Me.Width = VB6.TwipsToPixelsX(2300)
                End If

                If m_bThumbnails Then
                    ImageKit1.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.Width) - 120) - m_lThumbWidth)
                Else
                    ImageKit1.Width = Me.Width - VB6.TwipsToPixelsX(120)
                End If
                ImageKit1.Height = Me.Height - VB6.TwipsToPixelsY(400)

                IkThumb1.Height = ImageKit1.Height

                'TODO
                'lHeight = CInt(VB6.PixelsToTwipsY(IkThumb1.Height) - (IkThumb1.EdgeSize * 2) + IkThumb1.GapSize)

                lRows = CInt(Math.Floor(lHeight / c_lImageHeight))

                'TODO
                'IkThumb1.RowCount = CShort(lRows)
                IkThumb1.RowNumber = CShort(lRows)

            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Resize process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Resize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub




        End Try

    End Sub

    Private Sub frmChildTIF_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

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

        MemoryHelper.ReleaseMemory()
    End Sub

    'TODO
    'Private Sub ImageKit1_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ImageKit1.ClickEvent
    Private Sub ImageKit1_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ImageKit1.Click
        ' Check the handle on the window clicked on. If its a different one, then select that window,
        ' else, it could be a zoom
        If Me.Handle.ToInt32() <> objfrmParentMDI.ActiveMdiChild.Handle.ToInt32() Then

            m_lReturn = Me.DisplayPage(iPageNum:=Me.PageDisplayed)

            'Set the focus to the form

            frmChildTIF_Activated(Me, New EventArgs())


            ImageKit1.FindForm().Activate()

        End If
    End Sub

    ' PRIVATE Events (End)

    Public Function DisplayThumbs(ByVal v_bDisplay As Boolean) As Integer

        Try

            If v_bDisplay Then

                IkThumb1.Visible = True
                IkThumb1.Width = VB6.TwipsToPixelsX(m_lThumbWidth)
                ImageKit1.Left = VB6.TwipsToPixelsX(m_lThumbWidth)
                ImageKit1.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.Width) - 120) - m_lThumbWidth)
                m_bThumbnails = True

                AdjustThumbnail()

            Else

                IkThumb1.Visible = False
                ImageKit1.Left = 0
                ImageKit1.Width = Me.Width - VB6.TwipsToPixelsX(120)
                m_bThumbnails = False

            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayThumbs failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayThumbs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try

    End Function

    'TODO
    'Private Sub ImageKit1_MouseUpImage(ByVal eventSender As Object, ByVal eventArgs As AxIMGKIT6Lib._IIkDispEvents_MouseUpImageEvent) Handles ImageKit1.MouseUpImage
    Private Sub ImageKit1_MouseUpImage(ByVal eventSender As Object, ByVal eventArgs As Newtone.ImageKit.Win.MouseUpDownEventArgs) Handles ImageKit1.MouseUpImage
        Try

            If ImageKit1.RectDraw Then
                'TODO
                'ImageKit1.Zoom(ImageKit1.RectLeft, ImageKit1.RectTop, ImageKit1.RectRight, ImageKit1.RectBottom)
                ImageKit1.Zoom(ImageKit1.Rect.Left, ImageKit1.Rect.Top, ImageKit1.Rect.Right, ImageKit1.Rect.Bottom)
                'Clear the rectangle
                ImageKit1.RectDraw = False
                ImageKit1.RectDraw = True
            End If

        Catch
        End Try


    End Sub

    'TODO
    'Private Sub IkThumb1_SelectFile(ByVal eventSender As Object, ByVal eventArgs As AxIMGKIT6Lib._IIkThumbEvents_SelectFileEvent) Handles IkThumb1.SelectFile
    Private Sub IkThumb1_SelectFile_SelectFile(ByVal eventSender As Object, ByVal eventArgs As Newtone.ImageKit.Win.ThumbnailEventArgs) Handles IkThumb1.SelectFile
        If m_iCurrentPage <> eventArgs.ImageNumber Then
            m_iCurrentPage = eventArgs.ImageNumber
            m_lReturn = DisplayPage(iPageNum:=m_iCurrentPage)
        End If

    End Sub

    Public Function PrintSetup() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PrintSetup"

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        'TODO
        'ImageKit1.Copies = 1
        'ImageKit1.PrintToFile = False
        'ImageKit1.MinPage = 1
        'ImageKit1.MaxPage = CShort(m_iTotalPages)
        'ImageKit1.FromPage = 1
        'ImageKit1.ToPage = CShort(m_iTotalPages)
        'ImageKit1.Options = &H100000 Or &H4S
        'm_lReturn = ImageKit1.PrintDlg()
        'Dim psd As PageSetupDialog = New PageSetupDialog
        Dim mPrint As PrintDocument = New PrintDocument
        'psd.Document = mPrint
        ' psd.ShowDialog()


        Dim pd As PrintDialog = New PrintDialog
        pd.Document = mPrint

        If ImageKit1.Image.Width > ImageKit1.Image.Height Then
            mPrint.DefaultPageSettings.Landscape = True
        Else
            mPrint.DefaultPageSettings.Landscape = False
        End If

        If pd.ShowDialog() = DialogResult.OK Then
            mPrint.Print()
        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        ' Do any tidy up, e.g. Set x = Nothing here

'        Return result

        ' This is for debugging only
'        Resume

'        Return result
        End Try
        Return result
    End Function


    Public Function PrintPages() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PrintPages"

        Dim lLeft, lTop, lRight, lBottom, lWidth, lHeight As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        'Retrieves paper size or available printing area
        'TODO
        'm_lReturn = ImageKit1.GetPaperSize(ImageKit1.hDC, lLeft, lTop, lRight, lBottom, lWidth, lHeight, CShort(IMGKIT6Lib.OutPutDeviceModeConstants.ikPrinter))

        'TODO
        'ImageKit1.DocName = Me.Text
        Dim mPrint As PrintDocument = New PrintDocument
        mPrint.DocumentName = Me.Text
        'm_lReturn = ImageKit1.PrintStartDoc()
        'For lPage As Integer = 1 To m_iTotalPages

        '    If lPage >= ImageKit1.FromPage And lPage <= ImageKit1.ToPage Then
        '        FileOpen(v_iFileIndex:=lPage, v_bOpenOnly:=True)

        '        m_lReturn = ImageKit1.PrintStartPage()
        '        'TODO
        '        'm_lReturn = ImageKit1.ImageOut(ImageKit1.hDC, ImageKit1.ImgHandle, 0, 0, lRight - lLeft, lBottom - lTop, True, True, IMGKIT6Lib.OutPutDeviceModeConstants.ikPrinter)
        '        m_lReturn = ImageKit1.PrintEndPage()
        '    End If

        'Next

        'm_lReturn = ImageKit1.PrintEndDoc()
        'ImageKit1.PrintDeleteDC()

        AddHandler mPrint.PrintPage, AddressOf Me.DoPrintPage
        Dim p As PrintDialog = New PrintDialog
        p.Document = mPrint

        If p.ShowDialog() = DialogResult.OK Then
            mPrint.Print()
        End If

        'Reset the open file to be the currently displayed one
        PageDisplay()


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        ' Do any tidy up, e.g. Set x = Nothing here

'        Return result

        ' This is for debugging only
'        Resume

'        Return result
        End Try
        Return result
    End Function

    Public Function RotatePage(ByVal v_lDegrees As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RotatePage"

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        'TODO
        'If ImageKit1.ImgHandle = 0 Then
        '    GoTo Finally_Renamed
        'End If

        'TODO
        'ImageKit1.InImgHandle = ImageKit1.ImgHandle
        'ImageKit1.InMskHandle = 0

        'Convert into hundredths of a degree
        v_lDegrees *= 100

        m_lReturn = ImageKit1.Effect.Rotation(v_lDegrees, False, False, True, 255, 255, 255, False)
        If m_lReturn = 0 Then
	    Return result
        End If

        'Frees the effect control's input image handle from the memory
        'TODO
        'IkCommon1.ImgHandle = ImageKit1.InImgHandle
        'IkCommon1.FreeMemory()

        'Passes the effect control's output image handle to the display control and displays the image...
        'TODO
        'ImageKit1.ImgHandle = ImageKit1.Effect.OutImgHandle
        ImageKit1.Refresh()


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        ' Do any tidy up, e.g. Set x = Nothing here

'        Return result

        ' This is for debugging only
'        Resume

'        Return result
        End Try
        Return result
    End Function
    'TODO
    Private Sub DoPrintPage(ByVal sender As Object, ByVal e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics

        Dim wd As Integer = 0
        Dim ht As Integer = 0

        SetImageDimension((e.PageBounds.Width * 0.9), (e.PageBounds.Height * 0.9), ImageKit1.Image.Width, ImageKit1.Image.Height, wd, ht)

        g.DrawImage(ImageKit1.Image, New Rectangle((e.PageBounds.Width * 0.05), (e.PageBounds.Height * 0.05), wd, ht))
    End Sub
    'TODO
    Private Sub SetImageDimension(ByVal SetWidth As Integer, ByVal SetHeight As Integer, ByVal OrgWidth As Integer, ByVal OrgHeight As Integer, ByRef retX As Integer, ByRef retY As Integer)
        If SetHeight < 1 Or OrgWidth < 1 Or OrgHeight < 1 Then
            retY = 0
            retX = 0
        End If

        If (SetWidth / SetHeight) <= (OrgWidth / OrgHeight) Then
            retX = SetWidth
            retY = retX * (OrgHeight / OrgWidth)
        Else
            retY = SetHeight
            retX = (retY * (OrgWidth / OrgHeight))
        End If

    End Sub
End Class
