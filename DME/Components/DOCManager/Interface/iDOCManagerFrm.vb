Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections.Specialized
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Imports Sspi.Common.Aws.S3

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 3/12/1997
    '
    ' Description: Main interface for the DocuMaster Enterprise rather
    '              fine Document Manager.
    '
    ' Edit History:
    '
    ' SP040898 - Add 2 spaces to end of form caption ie "DocuMaster
    ' Enterprise  " so when dmslink does a FindWindow, caption not
    ' confused with explorer windows of same name.
    '
    ' JH271098 ConstructView - if the parameter for document has
    ' not been passed then just skip the bit where the doc
    ' is accessed and display folder only, likewise for the folders
    ' at other levels
    '
    ' JH051198 Select Folders stuff added and changed loads of routines
    '
    ' JH071298 NodeClick - folders shown in DocView match Select Folders
    '
    ' JH231298 LocateDocument - ignore error caused by info form DR 4248
    ' Enable folder node to respond to enter on right-hand-side DR 4253
    '
    ' JH040199 AddKeyword pass isuseradmin variable so non-admin cannot add
    ' and delete default keywords: DR 4250
    '
    ' JH040199 added form resizer to form to set minimum: DR 4273
    '
    ' JH040199 cannot rename any node to a blank string: DR 4384
    '
    ' JH040199 lvwDocList_DragDrop check whether destination is
    ' a folder before allowing move/copy and whether trying to
    ' move/copy to itself. DR 4247
    '
    ' JH050199 lvwDocList_ColumnClick and Populate_List altered so
    ' sort by hidden date column where dates are formatted as
    ' yyyymmddhhmmss DR 4290 + 4291
    '
    '
    '
    ' MS250900  Option to set auto fire-up of keywords and
    '           annotations windows after importing of a file (RSAIB doc 302)
    '
    ' DN 12/12/00 Added Email functionality
    '
    ' MS 22/05/01  Menu option added to clear down cache directory
    '
    ' MS 06/04/01  Save and load option added for favorite folders
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private keyComb As String = ""
    Public oDOCManagerInterface As iDOCManager.Interface_Renamed
    Public vPastedDocsArray As Object(,)
    Public bReloadmode As Boolean

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Form re-sizing Constants

    'Left alignment of left most control
    Const ACLeftAlign As Integer = 0

    'Closest Horizontal and vertical splitters can go to edges of Form
    Const ACHorizSplitLimit As Integer = 328 '1500
    Const ACVertSplitLimit As Integer = 100 '1500

    'Thickness of splitter bars
    Const ACSplitterVWidth As Integer = 4 '40
    Const ACSplitterHHeight As Integer = 3 '30

    'Off sets to allow for thickness of forms edges and stuff
    Const ACHorizOffset As Integer = 10 '150
    Const ACVertOffset As Integer = 52 '770 '670

    'Minimum dimensions when resizing Form
    Const ACHorizMinFormSize As Integer = 267 '4000
    Const ACVertMinFormSize As Integer = 200 '3000

    'Height of title labels
    Const ACLabelHeight As Integer = 16 '240
    ' PRIVATE Data Members (End)


    'Tool Bar Button.Key values
    Const ACViewDoc As String = "VIEWDOC"
    Const ACScan As String = "SCAN"
    Const ACCut As String = "CUT"
    Const ACCopy As String = "COPY"
    Const ACPaste As String = "PASTE"
    Const ACDelete As String = "DELETE"
    Const ACDocInfo As String = "DOCINFO"
    Const ACLargeIcon As String = "LARGE"
    Const ACSmallIcon As String = "SMALL"
    Const ACList As String = "LIST"
    Const ACListDetails As String = "LISTDET"
    Const ACHotKey As String = "HOTKEY"
    Const ACGoHome As String = "HOME"
    Const ACKeyword As String = "KEYWORD"
    Const ACAnnotation As String = "ANNOTATION"
    Const ACViewMain As String = "MAIN"
    Const ACViewFavourites As String = "FAV"
    Const ACViewBC As String = "BC"
    Const ACViewFindResults As String = "FIND"
    Const ACPrint As String = "PRINT"
    Const ACEmail As String = "EMAIL"
    Const ACArchive As String = "ARCHIVE"
    Const ACExpand As String = "EXPAND"

    'Other constants
    'JH051198 moved to main module so select folders can use
    ''Constants used in node keys (values, positions in key etc)
    '' Key structure is <F/D><P/' '><99999><Node Number>
    'Const ACFolder = "F"
    'Const ACDocument = "D"
    'Const ACPassword = "P"
    'Const ACPasswordStart = 2
    'Const ACPasswordLen = 1
    'Const ACDateStart = 3
    'Const ACDateLen = 5

    'Constants that determine when pasting whether the contents of the cliboard
    'are from a cut,a copy or it is empty.
    Const ACPasteEmpty As Integer = 0
    Const ACPasteCut As Integer = 1
    Const ACPasteCopy As Integer = 2

    'Virtual Key Codes
    Const VK_CONTROL As Integer = &H11S
    Const VK_SHIFT As Integer = &H10S

    'WR77 Documaster Enhancements START
    'Briefcase button Constants
    Const ACBCEmail As String = "EMAIL"
    Const ACBCArchive As String = "ARCHIVE"
    Const ACBCExport As String = "EXPORT"
    Const ACBCREMOVE As String = "REMOVE"
    Const ACBCDocsFrameHeight As Integer = 214 '3200
    Private m_BCDocsFrameHeight As Integer
    'WR77 Documaster Enhancements END

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_dtEffectiveDate As Date

    ' Set to true when controls are being resized
    Private m_bResizing As Boolean

    ' Set to true during a dragdrop operation
    Private m_bDragging As Boolean

    'What view mode we are in
    Private m_iViewMode As Integer

    'Which extra views are currently visible
    Private m_iViewKeywords As Integer
    Private m_iViewAnnotations As Integer
    Private m_iViewPages As Integer

    'Stores current X, Y pos of mouse on current control
    Private m_lX As Integer
    Private m_lY As Integer

    'Store results of query to business to get child folders and docs
    'for a parent
    Private m_vFolderArray(,) As Object
    Private m_vDocArray As Object

    'Store key of last open folder for each of the tree views
    Private m_sMainLastOpenFolder As String = ""
    Private m_sFindLastOpenFolder As String = ""

    'Options Values

    'Whether or not we only display documents in the doc listview
    Private m_bDocsOnly As Boolean
    'Whether or not we go home on start up
    Private m_bStartInHome As Boolean
    'Max Folders returned
    Private m_lMaxFolders As Integer
    'Max Folders returned from a filter
    Private m_lMaxFilterFolders As Integer
    'Warn if scanning to non version 2 folder
    Private m_bWarnScanToExternal As Boolean
    'Warn if moving docs to non version 2 folder
    Private m_bWarnMoveToNonFolder As Boolean

    'JH181198 RTF options
    Private m_bPrintWord As Boolean
    Private m_bViewWord As Boolean

    'MS250900 fire-up keywords/annotations automatically after import of a file?
    Private m_bAutoKeyword As Boolean

    'When right clicking a node in the treeview, in some cases we want to leave
    'the right clicked node selected. Set this to true in these cases
    Private m_bLeaveNodeSelected As Boolean

    'Stores the keys of the source nodes when drag/dropping or cut and
    'pasting nodes
    Private m_sDragNodes() As DOCConst.DOCNodes = Nothing
    Private m_sPasteNodes() As DOCConst.DOCNodes = Nothing
    Private m_iPasteFlag As Integer

    'store the current control and key for which a rename is taking place
    'Private m_cntRename As ListView
    Private m_cntRename As Object
    Private m_sRenameNode As DOCConst.DOCNodes = DOCConst.DOCNodes.CreateInstance()

    'hotkey array (ie frequently visited nodes)
    Dim m_sHotKey(9) As String
    'Store where we are in the hot key array
    Dim m_iHotKeyPos As Integer

    'Store the current control that a transaction applies to
    'Dim m_cntCurrent As ListView
    Dim m_cntCurrent As Object

    ' {* USER DEFINED CODE (End) *}
    '
    '' Declare an instance of the splash object.
    'Private m_oSplash As Object
    'JH051198 this should be global so SelectFolders can access

    ' Declare an instance of the document information object

    Private m_oInformation As iDOCInformation.Interface_Renamed

    ' Declare an instance of the password object

    Private m_oPassword As iDOCPassword.Interface_Renamed

    ' Declare an instance of the access level object

    Private m_oSetAccessLevel As iDOCSetAccessLevel.Interface_Renamed

    ' Declare an instance of the keyword admin object

    Private m_oKeywordAdmin As iDOCKeywordAdmin.Interface_Renamed

    ' Declare an instance of the Business object.
    'Private m_oBusiness As Object
    'JH051198 this should be global so SelectFolders can access

    ' Declare an instance of the Viewer object.

    Private m_oViewer As iDOCViewer.Interface_Renamed 'iDOCViewer.Interface


    ' Declare an instance of the Scan object.
    Private m_oScan As Object

    ' Declare an instance of the Zipper object.

    ' Private m_oZipper As bSIRZipper.Zipper
    Private m_oZipper As bPMZipper.Business


    'declare an instance of the selectfolders object
    Private m_oSelectFolders As iDOCManager.frmSelectFolders

    ' Stores the return value for function calls.
    Private m_lReturn As Integer

    ' Stores the location of the cache
    Private m_sCachePath As String = ""

    ' Set to true when form has been activated
    Private m_bFormLoaded As Boolean

    'JH051198 Set to true select folders class is initialised
    Public m_bSelectFoldersInitialised As Boolean

    'MS 09/05/01
    Private m_bArchive As Boolean

    Private m_bIsTopLevelFolder As Boolean
    'PN-52096
    Private m_bIsMoveDocList As Boolean

    Private m_bRefresh As Boolean
    Private Declare Function GetKeyState Lib "user32" (ByVal nVirtKey As Integer) As Short

    Private m_oFrmManager As Object
    Private Delegate Sub SetTreeView(ByVal sKey As String, ByVal sText As String, ByVal sImageKey As String, ByVal lIndex As Integer, ByRef trv As TreeView)
    Private Delegate Sub AddTreeNode(ByVal sKey As String, ByVal sText As String, ByVal sImageKey As String, ByRef ParentNode As TreeNode)
    Private Delegate Sub GetTreeNodeCount(ByVal bIncludeSubTrees As Boolean, ByRef lNodeCount As Integer, ByRef ParentNode As TreeNode)
    Private Delegate Sub SortTreeView(ByRef trvw As TreeView)

    Public Property FrmManager() As Object
        Get
            ' Return the task.
            Return m_oFrmManager
        End Get

        Set(ByVal value As Object)
            ' Return the task.
            m_oFrmManager = value
        End Set
    End Property

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            If Not m_bRefresh Then
                'Set the view mode to main
                SetViewModeMain()

                ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))
                'indicate we are running now
                m_bFormLoaded = True
            End If

            m_bRefresh = False

        End If
    End Sub

    Private Sub Form_Initialize_Renamed()
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' create splash object
            g_oSplash = New iDOCSplash.Interface_Renamed()

            ' keep user informed

            m_lReturn = g_oSplash.Show(DOCSplash_Message, "DocuMaster Enterprise is Loading ... Please Wait ...")

            'DN 29/11/01 - Instance the viewer if not already done so
            If m_oViewer Is Nothing Then

                'DN 15/02/02 - Create the Viewer Object
                m_oViewer = New iDOCViewer.Interface_Renamed()

                'initialise and pass instance of myself

                m_lReturn = m_oViewer.Initialise(Me)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'DN 13/12/00 - Don't display error message when cache path not setup
                    If m_lReturn = gPMConstants.PMEReturnCode.PMInvalidRequest Then


                        m_lReturn = g_oSplash.Hide()

                        'Fire up options screen to populate cache directory
                        mnuViewOptions_Click(mnuViewOptions, New EventArgs())

                    Else


                        m_oViewer.Dispose()
                        m_oViewer = Nothing

                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise viewer", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                        Exit Sub
                    End If
                End If
            End If

            'set boolean to false
            m_bSelectFoldersInitialised = False
            'm_bSelectFoldersInitialised = False

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bDOCManager.Form", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Failed to get an instance of the business object.
                ' Big problem
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                m_lReturn = g_oSplash.Hide()

                ' MS 21/05/01 A
                MessageBox.Show("Sirius Briefcase Database is empty OR " & Strings.Chr(13) & Strings.Chr(10) & _
                                "Failed to get business object", "Quitting DocuMaster", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                ' MS 21/05/01 D
                '        iPMFunc.LogMessage _
                ''            iType:=PMLogOnError, _
                ''            sMsg:="Failed to get business object", _
                ''            vApp:=ACApp, _
                ''            vClass:=ACClass, _
                ''            vMethod:="Form_Initialise"


                g_oObjectManager.Dispose()
                g_oObjectManager = Nothing

                ' MS 21/05/01

                'trash the other objects if they were used
                If Not (m_oViewer Is Nothing) Then

                    m_oViewer.Dispose()
                    m_oViewer = Nothing
                End If

                If Not (g_oSplash Is Nothing) Then

                    g_oSplash.Dispose()
                    g_oSplash = Nothing
                End If
                '

                Environment.Exit(0)

            End If

            'if successful can get rid of object manager now

            g_oObjectManager.Dispose()
            ' Destroy the instance of the object manager
            ' from memory.
            g_oObjectManager = Nothing

            'Store the access levels

            g_iAccessLevel = g_oBusiness.AccessLevel

            g_iAdminLevel = g_oBusiness.AdminLevel

            'ND 081100 - Set Delete and move levels

            g_iFileCopyLevel = g_oBusiness.FileCopyLevel

            g_iFolderCopyLevel = g_oBusiness.folderCopyLevel

            g_iFileMoveLevel = g_oBusiness.FileMoveLevel

            g_iFolderMoveLevel = g_oBusiness.folderMoveLevel

            g_iFileDeleteLevel = g_oBusiness.FileDeleteLevel

            g_iFolderDeleteLevel = g_oBusiness.folderDeleteLevel

            'Is user administrator ?
            g_bUserIsAdministrator = (g_iAccessLevel <= g_iAdminLevel)

            'Get the options values
            GetOptions()

            'Get start up values (form size etc)
            GetStartUpValues()

            'Set up all the controls
            m_lReturn = InitializeControls()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                m_lReturn = g_oSplash.Hide()

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed to Initialize all Controls", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Environment.Exit(0)
            End If

            ' Create an instance of the zipper class
            'm_oZipper = New bSIRZipper.Zipper()
            m_oZipper = New bPMZipper.Business()


            m_lReturn = g_oSplash.Hide()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub
    Public Sub SetModes()
        Dim command As String
        ' request to display a document triggered by SBO
        If oDOCManagerInterface.m_lDocNum > 0 Then

            oDOCManagerInterface.SBOViewDocument()
            Exit Sub
            'Return result

        End If



        If Information.IsNothing(oDOCManagerInterface.sCommand) Then
            oDOCManagerInterface.sCommand = ""
        Else

            command = CStr(oDOCManagerInterface.sCommand)
        End If

        'uppercase and trim command
        command = command.ToUpper()
        command = command.Trim()


        'Briefcase command lines detected
        If command.StartsWith("BC") Then

            ' Process Briefcase download or run command line for viewing
            m_lReturn = CType(oDOCManagerInterface.BriefCaseProcess(command), gPMConstants.PMEReturnCode)


            'if briefcase download was performed, attach database back
            If command.StartsWith("BC ") Then
                ' Attach PMBriefcase database
                m_lReturn = CType(oDOCManagerInterface.AttachDB(), gPMConstants.PMEReturnCode)

            End If

            If oDOCManagerInterface.m_sCurrentUser = g_sUserName Then
                'reset briefcase user in registry as download has finished

                m_lReturn = g_oBusiness.BriefcaseUser(sMode:="SET", sUser:="")

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to re-set Briefcase user details in server registry", "Terminating Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    '  Return gPMConstants.PMEReturnCode.PMFalse

                End If
            End If

            'Return result

        End If


        'SBO request to display folder via command line prompt
        If command.StartsWith("SBO") Then

            m_lReturn = CType(oDOCManagerInterface.SBODisplayClient(command, True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Cannot run iDocManager.SBODisplayClient", "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            ' don't want to run anything else after this..

            ' Return result
        Else
            'anything else..

            If command <> "" Then

                'splice the command
                m_lReturn = CType(oDOCManagerInterface.GetExternalCodes(command, oDOCManagerInterface.sExCodes), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Return result
                End If

                ConstructView(sCabExCode:=oDOCManagerInterface.sExCodes(1), sDrawExCode:=oDOCManagerInterface.sExCodes(2), sFoldExCode:=oDOCManagerInterface.sExCodes(3), sDocExCode:=oDOCManagerInterface.sExCodes(4))
            End If


        End If




        ' Return result
    End Sub

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load


        uctPMResizer1 = New PMResizerControl.uctPMResizer
        'JH040199
        With uctPMResizer1
            ' Tell the control to only resize the controls that I tell it to.
            '(not going to tell it to resize any because this is already done
            ' I just want to set the minimums)

            .NoResizeByDefault = True
            ' Form Min Height/Width

            .FormMinHeight = 334 '5000

            .FormMinWidth = 534 '8000
        End With
        staContents.Width = Me.ClientRectangle.Width
        _staContents_Panel1.Width = (staContents.Width) * (10 / 100)
        _staContents_Panel3.Width = (staContents.Width) * (5 / 100)
        _staContents_Panel2.Width = (staContents.Width) - _staContents_Panel1.Width - _staContents_Panel2.Width
        _staContents_Panel3.Text = DateTime.Now.ToShortTimeString
        staContents.Refresh()
        lblTitleFind(1).Left = 340
        lblTitleMain(1).Left = 340
        lblTitleFind(1).Width = lvwDocList.Width
        lblTitleMain(1).Width = lvwDocList.Width

        Dim sOption As String
        'Get the Sharepoint Server from the System Options
        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOption)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get document archive settings.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If
        If sOption.Length > 0 AndAlso sOption = "2" Then
            mnuPopToSharePoint.Enabled = True
        Else
            mnuPopToSharePoint.Enabled = False
        End If

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)


        ' Forms query unload event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Terminate the business object

            If Not (g_oBusiness Is Nothing) Then

                g_oBusiness.Dispose()
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to terminate the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload")
            End If

            ' Destroy the instance of the business object
            ' from memory.
            g_oBusiness = Nothing

            ' Destroy the instance of the zipper control
            m_oZipper = Nothing

            'trash the other objects if they were used
            If Not (m_oViewer Is Nothing) Then
                ' RDC 27072005 quick and dirty fix to resolve 462 errors
                Try

                    m_oViewer.Dispose()
                    m_oViewer = Nothing
                Catch ex As Exception

                End Try



            End If

            If Not (g_oSplash Is Nothing) Then

                g_oSplash.Dispose()

                g_oSplash = Nothing
            End If

            If Not (m_oInformation Is Nothing) Then

                m_oInformation.Dispose()
                m_oInformation = Nothing
            End If

            If Not (m_oPassword Is Nothing) Then

                m_oPassword.Dispose()
                m_oPassword = Nothing
            End If

            If Not (m_oSetAccessLevel Is Nothing) Then

                m_oSetAccessLevel.Dispose()
                m_oSetAccessLevel = Nothing
            End If

            If Not (m_oKeywordAdmin Is Nothing) Then

                m_oKeywordAdmin.Dispose()
                m_oKeywordAdmin = Nothing
            End If

            If Not (m_oSelectFolders Is Nothing) Then
                m_oSelectFolders.Dispose()
                m_oSelectFolders = Nothing
            End If

            'save the current form position, size etc as start up
            'values for when we run next (as long as not minimized)
            If Me.WindowState <> FormWindowState.Minimized Then
                SaveStartUpValues()
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            Me.Dispose()


        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            'Restrict minimum dimensions of the form
            If Me.WindowState <> FormWindowState.Minimized Then

                If (Me.Width) < ACVertMinFormSize Then
                    Me.Width = (ACVertMinFormSize)
                End If

                If (Me.Height) < ACHorizMinFormSize Then
                    Me.Height = (ACHorizMinFormSize)
                End If
            End If

            'Set up the controls positions, according to the two splitter bars
            ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

            'save the current form position, size etc as start up
            'values for when we run next (as long as not minimized,
            ' and form has been activated)
            If (Me.WindowState <> FormWindowState.Minimized) And (m_bFormLoaded) Then
                SaveStartUpValues()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Resize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: ResizeControls
    '
    ' Description: Resizes and repositions all controls according to the
    ' position of the two splitter bars and the current view
    '
    '       r_lX = imgSplitterV.Left
    '       r_lY = imgSplitterH.Top
    '
    ' ***************************************************************** '
    Private Sub ResizeControls(ByRef r_lX As Integer, ByRef r_lY As Integer, ByRef r_lZ As Integer)

        Dim iExtrasNum As Integer
        Dim lConstantControlsHeight, lFormAvailableScaleHeight As Integer


        Try

            lConstantControlsHeight = CInt((tlbMain.Height) + (picTitles.Height)) + 15
            lFormAvailableScaleHeight = CInt((Me.ClientRectangle.Height) - lConstantControlsHeight - (staContents.Height))

            'If we are minimised, dont resize as it will make no sense
            If Me.WindowState = FormWindowState.Minimized Then
                Exit Sub
            End If

            'check the width
            If r_lX < ACHorizSplitLimit Then
                r_lX = ACHorizSplitLimit
            End If

            If r_lX > ((Me.Width) - ACHorizSplitLimit) Then
                r_lX = CInt((Me.Width) - ACHorizSplitLimit)
            End If

            'check the height
            If r_lY < ACVertSplitLimit Then
                r_lY = ACVertSplitLimit
            End If

            If r_lY > ((Me.Height) - ACVertSplitLimit) Then
                r_lY = CInt((Me.Height) - ACVertSplitLimit)
            End If

            'check how many of the optional extra windows (ie keywords, annotations and
            'pages) are to be displayed, and set up accordingly. If this zero, then
            ' will have effect on heights of all other windows, obviously.
            iExtrasNum = Math.Abs(m_iViewKeywords + m_iViewAnnotations + m_iViewPages)

            'Resize individual controls

            'Main Tree View
            tvwMain.Left = (ACLeftAlign)
            tvwMain.Width = (r_lX) + 8
            tvwMain.Top = (lConstantControlsHeight) + 25

            'height of main tree different if in briefcase
            If m_iViewMode = DOCViewModeBC Then
                tvwMain.Height = (r_lY - ((picTitles.Top) + (picTitles.Height)))
            Else
                tvwMain.Height = (r_lZ) - tvwMain.Top
            End If

            'Favourites Tree View
            tvwFav.Left = tvwMain.Left
            tvwFav.Width = tvwMain.Width
            tvwFav.Height = tvwMain.Height
            tvwFav.Top = tvwMain.Top

            'Find Results Tree View
            tvwFind.Left = tvwMain.Left
            tvwFind.Width = tvwMain.Width
            tvwFind.Height = tvwMain.Height
            tvwFind.Top = tvwMain.Top

            'Main Folder Contents ListView
            lvwDocList.Left = (r_lX + ACSplitterVWidth) + 10
            lvwDocList.Width = Me.Width - (tvwMain.Width + (ACHorizOffset)) - 8
            lvwDocList.Top = (lConstantControlsHeight) + 25

            If iExtrasNum = 0 Then
                lvwDocList.Height = (lFormAvailableScaleHeight) - 26
            Else
                lvwDocList.Height = (r_lY) - lvwDocList.Top
            End If

            imgSplitterH.Width = lvwDocList.Width
            imgSplitterH.Left = lvwDocList.Left


            'Keywords Listview
            If m_iViewKeywords Then
                lvwKeyWords.Left = lvwDocList.Left
                lvwKeyWords.Width = (((Me.Width) - ((tvwMain.Width) + ACHorizOffset)) / iExtrasNum) - 8
                lvwKeyWords.Top = (r_lY + ACSplitterHHeight + 1)
                lvwKeyWords.Height = (lFormAvailableScaleHeight - (lvwDocList.Height) - ACSplitterHHeight) - 26
            End If

            'Annotations Listview
            If m_iViewAnnotations Then
                lvwAnnotations.Left = lvwDocList.Left + ((lvwKeyWords.Width) * Math.Abs(m_iViewKeywords))
                lvwAnnotations.Width = (((Me.Width) - ((tvwMain.Width) + ACHorizOffset)) / iExtrasNum) - 8
                lvwAnnotations.Top = (r_lY + ACSplitterHHeight + 1)
                lvwAnnotations.Height = (lFormAvailableScaleHeight - (lvwDocList.Height) - ACSplitterHHeight) - 26
            End If


            imgBCSplitterH.Width = tvwMain.Width
            imgBCSplitterH.Left = tvwMain.Left
            imgBCSplitterH.Top = (r_lZ)

            frBCDocs.Top = (r_lZ + 3)
            frBCDocs.Left = tvwMain.Left
            frBCDocs.Width = tvwMain.Width
            frBCDocs.Height = (lFormAvailableScaleHeight) - tvwMain.Height - imgBCSplitterH.Height - 20

            If (frBCDocs.Height) - (lblBCDocs.Height) - (tlbBCDocsButtons.Height) - 10 > 0 Then
                lvwBCDocs.Height = ((frBCDocs.Height) - (lblBCDocs.Height) - (tlbBCDocsButtons.Height) - 10)
            End If

            lblBCDocs.Width = frBCDocs.Width
            tlbBCDocsButtons.Width = frBCDocs.Width
            lvwBCDocs.Width = frBCDocs.Width - (3)

            'Pages Listview
            If m_iViewPages Then
                lvwPages.Left = ((lvwDocList.Left) + ((lvwKeyWords.Width) * Math.Abs(m_iViewKeywords)) + ((lvwAnnotations.Width) * Math.Abs(m_iViewAnnotations)))
                lvwPages.Width = (((Me.Width) - ((tvwMain.Width) + ACHorizOffset)) / iExtrasNum)
                lvwPages.Top = (r_lY + ACSplitterHHeight)
                lvwPages.Height = Me.Height - ((lvwPages.Top) + (staContents.Height) + ACVertOffset)
            End If

            'Briefcase Contents Treeview
            tvwBCMain.Left = lvwDocList.Left
            tvwBCMain.Width = lvwDocList.Width + 20
            tvwBCMain.Top = lvwDocList.Top
            tvwBCMain.Height = lvwDocList.Height - 12

            'Main Folder Contents (docs only) Listview
            lvwDocsOnly.Left = tvwMain.Left
            lvwDocsOnly.Width = tvwMain.Width
            lvwDocsOnly.Top = (r_lY + ACSplitterHHeight)
            lvwDocsOnly.Height = Me.Height - ((lvwDocsOnly.Top) + (staContents.Height) + ACVertOffset) - 20

            '    'Briefcase Document Contents Listview
            lvwBCDocsOnly.Left = tvwBCMain.Left
            lvwBCDocsOnly.Width = tvwBCMain.Width + 8
            lvwBCDocsOnly.Top = (r_lY + ACSplitterHHeight)
            lvwBCDocsOnly.Height = Me.Height - ((lvwBCDocsOnly.Top) + (staContents.Height) + ACVertOffset) - 20

            'Title Labels

            'For Each cnt As Control In ContainerHelper.Controls(Me)
            '    If TypeOf cnt Is Label Then
            '        If cnt.Text.ToUpper() <> "BRIEFCASE" Then 'WR77 Documaster Enhancements
            '            cnt.Top = 0
            '            cnt.Height = (ACLabelHeight)

            '            If ContainerHelper.GetControlIndex(cnt) = 0 Then
            '                'ie the label on the left
            '                cnt.Left = (ACLeftAlign)
            '                cnt.Width = tvwMain.Width

            '            ElseIf ContainerHelper.GetControlIndex(cnt) = 1 Then
            '                'ie the label on the right
            '                cnt.Left = lvwDocList.Left + (ACSplitterHHeight)
            '                cnt.Width = lvwDocList.Width - (ACSplitterVWidth)
            '            End If
            '        End If 'WR77 Documaster Enhancements END
            '    End If
            'Next cnt
            'Status Bar
            '    staContents.Width = Me.Width

            'Vertical Splitter
            imgSplitterV.Left = (r_lX)
            imgSplitterV.Top = tvwMain.Top
            imgSplitterV.Height = Me.ClientRectangle.Height - (picTitles.Top + picTitles.Height + staContents.Height)

            'Horizontal Splitter
            If m_iViewMode = DOCViewModeBC Then
                imgSplitterH.Left = (ACLeftAlign)
                imgSplitterH.Width = Me.Width
                imgSplitterH.Top = (r_lY)
            Else
                imgSplitterH.Left = lvwDocList.Left
                imgSplitterH.Width = Me.Width - (tvwMain.Width + (ACHorizOffset))
                imgSplitterH.Top = (r_lY)
            End If

        Catch excep As System.Exception
            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Failed to resize/reposition a control", vApp:=ACApp, vClass:=ACClass, vMethod:="ResizeControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Sub

    Private Sub imgSplitterV_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterV.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)


        Try

            'Size the picture splitter to same sixe as the image splitter being moved
            With imgSplitterV
                picSplitter.SetBounds(.Left, .Top, ((.Width) \ 2), ((.Height) - 20))
            End With

            picSplitter.Visible = True
            m_bResizing = True

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="imgSplitterV_MouseDown", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub imgSplitterV_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterV.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        Dim lPos As Integer

        Try

            'Move the picture splitter to its new location
            If m_bResizing Then

                lPos = CInt(X + (imgSplitterV.Left))

                If lPos < ACHorizSplitLimit Then
                    picSplitter.Left = ACHorizSplitLimit
                Else

                    If lPos > (Me.Width) - ACHorizSplitLimit Then
                        picSplitter.Left = Me.Width - ACHorizSplitLimit
                    Else
                        picSplitter.Left = lPos
                    End If
                End If
            End If

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="imgSplitterV_MouseMove", excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub imgSplitterV_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterV.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = eventArgs.X
        Dim Y As Single = eventArgs.Y


        'Resize all the forms controls
        ResizeControls(picSplitter.Left, imgSplitterH.Top, imgBCSplitterH.Top)

        picSplitter.Visible = False
        m_bResizing = False

    End Sub

    ' ***************************************************************** '
    ' Name: InitializeControls
    '
    ' Description: Sets the start positions and properties of the image
    '  splitters, upon which the position and size of all other controls
    ' depend etc.
    '
    ' ***************************************************************** '
    Private Function InitializeControls() As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the start positions of the image splitters, upon
            'which the postion and size of all other controls depend.
            imgSplitterV.Top = tlbMain.Height + picTitles.Height
            imgSplitterV.Height = Me.ClientRectangle.Height - (picTitles.Top + picTitles.Height + staContents.Height)

            imgSplitterH.Left = imgSplitterV.Left
            imgSplitterH.Width = Me.Width - imgSplitterV.Left

            imgBCSplitterH.Left = 0

            tvwMain.Left = (ACLeftAlign)

            'Setup the menu items
            m_lReturn = SetUpMenu()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed in SetUpMenu", vApp:=ACApp, vClass:=ACClass, vMethod:="InitializeControls")

                Return result

            End If

            'Set the extra views

            'keywords
            If m_iViewKeywords Then
                mnuViewExtrasKeywords.Checked = True
                If Not CType(tlbMain.Items.Find("_tlbMain_Button21", True)(0), ToolStripButton) Is Nothing Then
                    CType(tlbMain.Items.Find("_tlbMain_Button21", True)(0), ToolStripButton).Checked = True
                    lvwKeyWords.Visible = m_iViewKeywords
                End If

            Else
                mnuViewExtrasKeywords.Checked = False
                'If Not CType(tlbMain.Items.Item(ACKeyword), ToolStripButton) Is Nothing Then
                If Not CType(tlbMain.Items.Find("_tlbMain_Button21", True)(0), ToolStripButton) Is Nothing Then
                    CType(tlbMain.Items.Find("_tlbMain_Button21", True)(0), ToolStripButton).Checked = False
                    lvwKeyWords.Visible = m_iViewKeywords
                End If
            End If

            'annotations
            If m_iViewAnnotations Then
                mnuViewExtrasAnnotations.Checked = True
                If Not CType(tlbMain.Items.Find("_tlbMain_Button22", True)(0), ToolStripButton) Is Nothing Then
                    CType(tlbMain.Items.Find("_tlbMain_Button22", True)(0), ToolStripButton).Checked = True
                    lvwAnnotations.Visible = m_iViewAnnotations
                End If

            Else
                mnuViewExtrasAnnotations.Checked = False
                If Not CType(tlbMain.Items.Find("_tlbMain_Button22", True)(0), ToolStripButton) Is Nothing Then
                    CType(tlbMain.Items.Find("_tlbMain_Button22", True)(0), ToolStripButton).Checked = False
                    lvwAnnotations.Visible = m_iViewAnnotations
                End If
            End If

            'pages
            mnuViewExtrasPages.Checked = False
            m_iViewPages = False
            lvwPages.Visible = m_iViewPages

            'Get the root folder list
            m_lReturn = GetFolderList(0, "", m_vFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed in GetFolderList", vApp:=ACApp, vClass:=ACClass, vMethod:="InitializeControls")

                Return result

            End If

            'populate root nodes of main view
            tvwMain.Nodes.Clear()

            m_lReturn = PopulateTreeRoots(tvwMain, m_vFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed in PopulateTreeRoots", vApp:=ACApp, vClass:=ACClass, vMethod:="InitializeControls")

                Return result

            End If

            'go home on start up if option set
            If m_bStartInHome Then

                LocateFolder(g_oBusiness.HomeFolder, True)
            End If

            lvwBCDocs.LabelEdit = False
            lvwBCDocs.View = View.Details
            lvwBCDocs.HideSelection = False
            lvwBCDocs.MultiSelect = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the controls", vApp:=ACApp, vClass:=ACClass, vMethod:="InitializeControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub imgSplitterH_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterH.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        Try

            'Size the picture splitter to same sixe as the splitter bar being moved
            With imgSplitterH
                picSplitter.SetBounds(.Left, .Top, .Width - 20, .Height / 2)
            End With

            picSplitter.Visible = True
            m_bResizing = True

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="imgSplitterH_MouseDown", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub imgSplitterH_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterH.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        Dim lPos As Integer

        Try

            'Move the picture splitter to its new location
            If m_bResizing Then

                lPos = CInt(Y + (imgSplitterH.Top))

                If lPos < ACVertSplitLimit Then
                    picSplitter.Top = ACVertSplitLimit
                Else

                    If lPos > (Me.Height) - ACVertSplitLimit Then
                        picSplitter.Top = Me.Height - ACVertSplitLimit
                    Else
                        picSplitter.Top = lPos
                    End If
                End If
            End If

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="imgSplitterH_MouseMove", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub imgSplitterH_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitterH.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)


        'Resize all the forms Controls
        ResizeControls(CInt((imgSplitterV.Left)), CInt((picSplitter.Top)), CInt((imgBCSplitterH.Top)))

        picSplitter.Visible = False
        m_bResizing = False

    End Sub

    Private Sub lvwAnnotations_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAnnotations.Click

        EnableMenuItems(lvwAnnotations)

    End Sub


    'Private Sub lvwAnnotations_DragOver(ByRef Source As Control, ByRef X As Single, ByRef Y As Single, ByRef State As Integer)

    '	DragOverCheck(lvwAnnotations, Source)

    'End Sub

    Private Sub lvwAnnotations_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAnnotations.Enter

        EnableMenuItems(lvwAnnotations)

    End Sub

    Private Sub lvwAnnotations_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwAnnotations.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        'delete pressed ?
        If eventArgs.KeyCode = Keys.Delete Then

            'save current control
            m_cntCurrent = lvwAnnotations

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            DeleteAnnotation()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            m_cntCurrent = Nothing

        End If

    End Sub

    Private Sub lvwAnnotations_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAnnotations.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        Try

            'Bring Popup menu if right click on a node
            If Button = MouseButtons.Right Then
                If lvwAnnotations.Items.Count > 0 Then
                    m_lReturn = NodeClicked(lvwAnnotations)


                    Select Case m_lReturn
                        Case gPMConstants.PMEReturnCode.PMFalse

                            'Node not clicked on, continue
                            Exit Sub

                        Case gPMConstants.PMEReturnCode.PMTrue
                            'Fine, continue
                            EnableMenuItems(lvwAnnotations)

                            lvwAnnotations.FocusedItem = lvwAnnotations.Items.Item(lvwAnnotations.GetItemAt(X, Y).Name)

                            'save current control
                            m_cntCurrent = lvwAnnotations
                            Ctx_mnuPop.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                            m_cntCurrent = lvwAnnotations

                        Case Else
                            'problem, so go.
                            Exit Sub

                    End Select
                End If
            Else

                'may have changed permissable menu items
                EnableMenuItems(lvwAnnotations)

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwAnnotations_MouseDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwAnnotations_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAnnotations.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = eventArgs.X
        Dim Y As Single = eventArgs.Y

        'Always keep track of where mouse is on a control, so can check if its over a
        'node when clicked
        m_lX = CInt(X)
        m_lY = CInt(Y)

    End Sub

    Private Sub lvwBCDocsOnly_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwBCDocsOnly.Enter

        EnableMenuItems(lvwBCDocsOnly)

    End Sub

    Private Sub lvwBCDocsOnly_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwBCDocsOnly.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        'Bring Popup menu if right click on a node
        If Button = 2 Then
            m_lReturn = NodeClicked(lvwBCDocsOnly)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Node not clicked on, so exit
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Fine, continue
                    EnableMenuItems(lvwBCDocsOnly)

                    'save current control
                    m_cntCurrent = lvwBCDocsOnly
                    Ctx_mnuPop.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                    m_cntCurrent = Nothing

                Case Else
                    'problem, so go.
                    Exit Sub

            End Select
        End If

    End Sub

    Private Sub lvwBCDocsOnly_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwBCDocsOnly.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = eventArgs.X
        Dim Y As Single = eventArgs.Y

        'Always keep track of where mouse is on a control, so can check if its over a
        'node when clicked
        m_lX = CInt(X)
        m_lY = CInt(Y)

    End Sub

    Private Sub lvwDocList_AfterLabelEdit(ByVal eventSender As Object, ByVal eventArgs As LabelEditEventArgs) Handles lvwDocList.AfterLabelEdit
        Dim Cancel As Boolean = eventArgs.CancelEdit
        Dim NewString As String = eventArgs.Label

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            If Not String.IsNullOrEmpty(NewString) Then
                m_lReturn = RenameNode(NewString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                'abort rename
                Cancel = 1
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_AfterLabelEdit", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwDocList_BeforeLabelEdit(ByVal eventSender As Object, ByVal eventArgs As LabelEditEventArgs) Handles lvwDocList.BeforeLabelEdit
        Dim Cancel As Boolean = eventArgs.CancelEdit

        'store the control, node key and name before edit
        m_cntRename = lvwDocList
        m_sRenameNode.Key = lvwDocList.FocusedItem.Name
        m_sRenameNode.Text = lvwDocList.FocusedItem.Text

    End Sub

    Private Sub lvwDocList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDocList.Click


        Try

            m_lReturn = NodeClicked(lvwDocList)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Node not clicked on, so exit
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Fine, continue
                Case Else
                    'Anything else, continue
            End Select

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Do keywords processing for the selected node.
            CheckKeywords(lvwDocList.FocusedItem.Name)

            ' Do Annotations processing for the selected node.
            CheckAnnotations(lvwDocList.FocusedItem.Name)

            'menu items available may have been affected - check
            EnableMenuItems(lvwDocList)

            UpdateStatusBar(lvwDocList)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwDocList_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwDocList.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwDocList.Columns(eventArgs.Column)


        Try

            ' Flip sort way around
            If ListViewHelper.GetSortOrderProperty(lvwDocList) = SortOrder.Ascending Then
                ListViewHelper.SetSortOrderProperty(lvwDocList, SortOrder.Descending)
                ' Set the value for the sorting of the date
                bSortAccending = False
            Else
                ListViewHelper.SetSortOrderProperty(lvwDocList, SortOrder.Ascending)
                ' date sorting order
                bSortAccending = True
            End If

            ' Set Sorted to True to sort the list.
            ListViewHelper.SetSortedProperty(lvwDocList, True)

            ' if it was the date column, then it needs to be handled
            ' differently.
            If ColumnHeader.Text = "Create Date" Then

                'JH050199 don't do this anymore, just sort by the
                'the hidden date column where they are formatted yyyymmdd

                '        ' We dont want windows to sort the list, so we set it to false
                '        lvwDocList.Sorted = False
                '        ' Now we sort it
                '        SendMessage lvwDocList.hWnd, _
                ''                  LVM_SORTITEMS, _
                ''                  lvwDocList.hWnd, _
                ''                  AddressOf CompareDates

                ListViewHelper.SetSortKeyProperty(lvwDocList, DOCHiddenSortColumn - 1)

            Else

                ListViewHelper.SetSortKeyProperty(lvwDocList, ColumnHeader.Index + 1 - 1)

            End If

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_ColumnClick", excep:=excep)

            Exit Sub

        End Try


    End Sub
    Private Sub lvwDocList_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDocList.DoubleClick
        Dim oMessage As frmMessage

        Dim sNodeKey() As DOCConst.DOCNodes = Nothing
        Dim sParentNodeKey As String = ""

        Dim tvw As TreeView

        Try

            m_lReturn = NodeClicked(lvwDocList)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Node not clicked on, so exit
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Fine, continue
                Case Else
                    'Anything else, continue
            End Select

            'check the passwords of the node - unless you are adminstrator
            If Not g_bUserIsAdministrator Then

                ReDim sNodeKey(0)
                sNodeKey(0).Key = lvwDocList.FocusedItem.Name
                sNodeKey(0).Text = lvwDocList.FocusedItem.Text

                m_lReturn = VerifyPasswords(sNodeKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Exit Sub
                End If
            End If

            'if we have a document, then call the document viewer
            If lvwDocList.FocusedItem.Name.Substring(0, 1) = ACDocument Then


                ' If lvwDocList.listViewHelper1.GetListViewSubItem(lvwDocList.FocusedItem, 2).Text = DOCEML Then
                If ListViewHelper.GetListViewSubItem(lvwDocList.FocusedItem, 2).Text = DOCEML Then
                    oMessage = New frmMessage()
                    oMessage.GetMessage(sMessage:="Loading Document " & lvwDocList.FocusedItem.Text, lSeconds:=10)
                    Application.DoEvents()
                    m_bRefresh = True
                End If
                ViewDocument()
                If m_bRefresh Then
                    If Not (m_oViewer Is Nothing) Then

                        m_oViewer.Dispose()
                        m_oViewer = Nothing
                    End If
                End If
                Exit Sub
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'check what view we're in and store the key of the selected folders parent

            Select Case m_iViewMode
                Case DOCViewModeMain

                    sParentNodeKey = Convert.ToString(lblTitleMain(1).Tag)
                    tvw = tvwMain

                Case DOCViewModeFavourites, DOCViewModeBC

                Case DOCViewModeFindResults

                    sParentNodeKey = Convert.ToString(lblTitleFind(1).Tag)
                    tvw = tvwFind

            End Select

            'get the key of the nodes parent


            'Check to see if the selected node exists in the tree view (ie its parent
            'has children). If so can just select it in the treeview, else we need to
            'populate the treeview from the listview, as the selected node needs to
            'appear as selected in the tree
            'If (tvw.SelectedItem.Children > 0) Then
            'If tvw.Nodes.Item(sParentNodeKey).GetNodeCount(False) > 0 Then
            If tvw.Nodes.Find(sParentNodeKey, True)(0).GetNodeCount(False) > 0 Then

                'OK, node already exists in the tree, so just select it so it appears
                'tvw.SelectedNode = tvw.Nodes.Item(lvwDocList.FocusedItem.Name)
                tvw.SelectedNode = tvw.Nodes.Find(lvwDocList.FocusedItem.Name, True)(0)

            Else

                'Get the list of folders in the listview
                GetFolderListFromListView(lvwDocList, m_vFolderArray)

                'display the selected folder in the tree
                'm_lReturn = PopulateTreeChildren(tvw, tvw.Nodes.Item(sParentNodeKey).Index, m_vFolderArray)
                m_lReturn = PopulateTreeChildren(tvw, tvw.Nodes.Find(sParentNodeKey, True)(0).Index, m_vFolderArray)

                'tvw.SelectedNode = tvw.Nodes.Item(lvwDocList.FocusedItem.Name)
                tvw.SelectedNode = tvw.Nodes.Find(lvwDocList.FocusedItem.Name, True)(0)

            End If


            'update the open folders ImageKey

            Select Case m_iViewMode
                Case DOCViewModeMain

                    'update the label
                    lblTitleMain(1).Text = "Contents of '" & lvwDocList.FocusedItem.Text & "'"
                    lblTitleMain(1).Tag = lvwDocList.FocusedItem.Name

                    'Swap icons of newly opened folder and last open folder
                    If m_sMainLastOpenFolder <> "" Then
                        'tvw.Nodes.Item(m_sMainLastOpenFolder).ImageKey = "IMGCLOSEDFOLDER"
                        tvw.Nodes.Find(m_sMainLastOpenFolder, True)(0).ImageKey = "IMGCLOSEDFOLDER"
                    End If


                    tvw.SelectedNode.ImageKey = "IMGOPENFOLDER"
                    m_sMainLastOpenFolder = tvw.SelectedNode.Name

                    NodeClick(tvw, lvwDocList, lvwDocList.FocusedItem.Name, "")

                Case DOCViewModeFavourites, DOCViewModeBC

                Case DOCViewModeFindResults

                    'Swap icons of newly opened folder and last open folder
                    If m_sFindLastOpenFolder <> "" Then
                        'tvw.Nodes.Item(m_sFindLastOpenFolder).ImageKey = "IMGCLOSEDFOLDER"
                        tvw.Nodes.Find(m_sFindLastOpenFolder, True)(0).ImageKey = "IMGCLOSEDFOLDER"
                    End If


                    tvw.SelectedNode.ImageKey = "IMGOPENFOLDER"
                    m_sFindLastOpenFolder = tvw.SelectedNode.Name

                    'update the label
                    lblTitleFind(1).Text = "Contents of '" & lvwDocList.FocusedItem.Text & "'"
                    lblTitleFind(1).Tag = lvwDocList.FocusedItem.Name

                    NodeClick(tvw, lvwDocList, lvwDocList.FocusedItem.Name, "")

            End Select


            'menu items available may have been affected - check
            EnableMenuItems(lvwDocList)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_DblClick", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwDocList_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwDocList.DragDrop
        Dim iCntl As Integer
        Dim bCopy As Boolean
        Dim sDestNode As DOCConst.DOCNodes = DOCConst.DOCNodes.CreateInstance()


        Try

            'See if over a node

            If lvwDocList.FocusedItem Is Nothing Then
                ' lvwDocList.DropHighlight = Nothing
                m_bDragging = False
                Exit Sub

            Else

                'JH040199 check whether it's trying to copy to itself

                'If lvwDocList.DropHighlight.Name = m_sDragNodes(0).Key Then
                '    
                '    lvwDocList.DropHighlight = Nothing
                '    m_bDragging = False
                '    Exit Sub
                'End If

                ''JH040199 check whether the destination is a folder by ImageKey
                '
                'If lvwDocList.DropHighlight.ImageKey <> "IMGCLOSEDFOLDER" Then
                '    MessageBox.Show("Can Only Move or Copy to a Folder", Application.ProductName)
                '    
                '    lvwDocList.DropHighlight = Nothing
                '    m_bDragging = False
                '    Exit Sub
                'End If

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


                ' lvwDocList.DropHighlight = Nothing
                m_bDragging = False

                'Check if copying or not by seeing if Control Key pressed
                iCntl = GetKeyState(VK_CONTROL)
                bCopy = (iCntl And &H8000S) <> 0

                'Store the destination node key and name
                sDestNode.Key = lvwDocList.GetItemAt(e.X, e.Y).Name
                sDestNode.Text = lvwDocList.GetItemAt(e.X, e.Y).Text

                'Update the actual database with the moves
                If bCopy Then
                    m_lReturn = CopyNodes(sDestNode, m_sDragNodes)
                Else
                    m_lReturn = MoveNodes(sDestNode, m_sDragNodes)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                'Update the control views
                m_lReturn = UpdateViews(tvwMain, lvwDocList, bCopy, sDestNode, m_sDragNodes)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed to update views. Please refresh your view of the data.", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_DragDrop", excep:=New Exception(Information.Err().Description))
                End If

                'menu items available may have been affected - check
                EnableMenuItems(lvwDocList)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            End If

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="HitTest failed to determine ToNode", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_DragDrop", excep:=excep)

            Exit Sub

        End Try

    End Sub





    '
    'Private Sub lvwDocList_DragDrop(ByRef Source As Control, ByRef X As Single, ByRef Y As Single)

    '	Dim iCntl As Integer
    '	Dim bCopy As Boolean
    '	Dim sDestNode As DOCConst.DOCNodes = DOCConst.DOCNodes.CreateInstance()


    '	Try 

    '		'See if over a node

    '		
    '		If lvwDocList.DropHighlight Is Nothing Then

    '			
    '			lvwDocList.DropHighlight = Nothing
    '			m_bDragging = False
    '			Exit Sub

    '		Else

    '			'JH040199 check whether it's trying to copy to itself
    '			
    '			If lvwDocList.DropHighlight.Name = m_sDragNodes(0).Key Then
    '				
    '				lvwDocList.DropHighlight = Nothing
    '				m_bDragging = False
    '				Exit Sub
    '			End If

    '			'JH040199 check whether the destination is a folder by ImageKey
    '			
    '			If lvwDocList.DropHighlight.ImageKey <> "IMGCLOSEDFOLDER" Then
    '				MessageBox.Show("Can Only Move or Copy to a Folder", Application.ProductName)
    '				
    '				lvwDocList.DropHighlight = Nothing
    '				m_bDragging = False
    '				Exit Sub
    '			End If

    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

    '			
    '			lvwDocList.DropHighlight = Nothing
    '			m_bDragging = False

    '			'Check if copying or not by seeing if Control Key pressed
    '			iCntl = GetKeyState(VK_CONTROL)
    '			bCopy = (iCntl And &H8000s) <> 0

    '			'Store the destination node key and name
    '			sDestNode.Key = lvwDocList.GetItemAt(X, Y).Name
    '			sDestNode.Text = lvwDocList.GetItemAt(X, Y).Text

    '			'Update the actual database with the moves
    '			If bCopy Then
    '				m_lReturn = CopyNodes(sDestNode, m_sDragNodes)
    '			Else
    '				m_lReturn = MoveNodes(sDestNode, m_sDragNodes)
    '			End If

    '			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
    '				Exit Sub
    '			End If

    '			'Update the control views
    '			m_lReturn = UpdateViews(tvwMain, lvwDocList, bCopy, sDestNode, m_sDragNodes)

    '			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '				LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed to update views. Please refresh your view of the data.", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_DragDrop", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
    '			End If

    '			'menu items available may have been affected - check
    '			EnableMenuItems(lvwDocList)

    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

    '		End If

    '	Catch excep As System.Exception



    '		LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="HitTest failed to determine ToNode", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_DragDrop", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

    '		Exit Sub

    '	End Try

    'End Sub
    '
    'Private Sub lvwDocList_DragOver(ByRef Source As Control, ByRef X As Single, ByRef Y As Single, ByRef State As Integer)


    '	Try 

    '		If m_bDragging Then

    '			' Set DropHighlight to the mouse's coordinates.
    '			
    '			lvwDocList.DropHighlight = lvwDocList.GetItemAt(X, Y)

    '			DragOverCheck(lvwDocList, Source, X, Y)

    '		End If

    '	Catch excep As System.Exception



    '		LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_DragOver", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

    '		Exit Sub

    '	End Try

    'End Sub


    Private Sub lvwDocList_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDocList.Enter


        Try

            EnableMenuItems(lvwDocList)

            UpdateStatusBar(lvwDocList)

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_GotFocus", excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub lvwDocList_ItemClick(ByVal Item As ListViewItem)
        m_bIsMoveDocList = False
    End Sub

    Private Sub lvwDocList_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles lvwDocList.ItemDrag
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim X As Single = e.X
        'Dim Y As Single = e.Y


        Try

            'Always keep track of where mouse is on a control, so can check if its over a
            'node when clicked
            'm_lX = CInt(X)
            'm_lY = CInt(Y)
            If e.Button = MouseButtons.Left Then  ' Signal a Drag operation.

                lvwDocList.MultiSelect = True
                'Store the nodes selected for moving
                m_lReturn = StoreSelectedNodes(m_sDragNodes, lvwDocList)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                'if nodes selected do nowt
                If m_sDragNodes(0).Key = "" Then
                    Exit Sub
                End If

                m_bDragging = True ' Set the flag to true.


                'lvwDocList.Drag(vbBeginDrag) ' Drag operation.
                lvwDocList.DoDragDrop(lvwDocList.FocusedItem.Name, DragDropEffects.Move)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            End If

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_MouseMove", excep:=excep)

            Exit Sub

        End Try
    End Sub


    Private Sub lvwDocList_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwDocList.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Dim oMessage As frmMessage

        Dim lNodeNum As Integer
        Dim sMsg, sKey As String

        Dim lFolderNum As Integer
        Dim sExCode, sDocType As String
        Dim iAccessLevel As Integer
        Dim sPassword As String = ""
        Dim dCreateDate As Date
        Dim lLink As Integer
        Dim sZipped As String = ""

        Dim dExpiryDate As Date
        Dim sScanUser As String = ""
        Dim dDocDate As Date
        Dim sLastUser As String = ""
        Dim dLastDate As Date
        Dim vPageList() As Object
        Dim iVolumeID As Integer
        Static iLastKeyPressed As Keys

        Try
            'Helpful information about a node
            If (eventArgs.KeyCode = Keys.F12) And (Shift = ShiftConstants.CtrlMask) Then

                m_lReturn = ExtractNumFromKey(lvwDocList.FocusedItem.Name, lNodeNum)

                Try
                    If lvwDocList.FocusedItem.Name.Substring(0, 1) = ACDocument Then

                        sMsg = "Document Name : " & Strings.Chr(9).ToString() & "'" & lvwDocList.FocusedItem.Text & "'" & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Document Num : " & Strings.Chr(9).ToString() & CStr(lNodeNum) & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Node Key : " & Strings.Chr(9).ToString() & lvwDocList.FocusedItem.Name & Strings.Chr(10).ToString() & Strings.Chr(10).ToString()

                        ' For a document
                        ' Get all the information

                        m_lReturn = g_oBusiness.GetDocumentInformation(lNodeNum:=lNodeNum, lFolderNum:=lFolderNum, sExCode:=sExCode, sDocType:=sDocType, iAccessLevel:=iAccessLevel, sPassword:=sPassword, dCreateDate:=dCreateDate, lLink:=lLink, sZipped:=sZipped, dExpiryDate:=dExpiryDate, sScanUser:=sScanUser, dDocDate:=dDocDate, sLastUser:=sLastUser, dLastDate:=dLastDate, vPageList:=vPageList, iVolumeID:=iVolumeID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_KeyDown", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Exit Sub
                        End If

                        ' add the info from the document table
                        sMsg = sMsg & "Folder Number : " & Strings.Chr(9).ToString() & CStr(lFolderNum) & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Ex Code : " & Strings.Chr(9).ToString() & "'" & sExCode & "'" & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Document Type : " & Strings.Chr(9).ToString() & "'" & sDocType & "'" & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Access Level : " & Strings.Chr(9).ToString() & CStr(iAccessLevel) & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Password : " & Strings.Chr(9).ToString() & "'" & sPassword & "'" & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Create Date : " & Strings.Chr(9).ToString() & DateTimeHelper.ToString(dCreateDate) & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Link : " & Strings.Chr(9).ToString() & Strings.Chr(9).ToString() & CStr(lLink) & Strings.Chr(10).ToString() & Strings.Chr(10).ToString()
                        'sMsg = sMsg & "Zipped : " & Chr$(9) & Chr$(9) & "'" & sZipped & "'" & Chr$(10) & Chr(10)

                        ' add the info from the doc_info table
                        sMsg = sMsg & "Expiry Date : " & Strings.Chr(9).ToString() & DateTimeHelper.ToString(dExpiryDate) & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Scan User : " & Strings.Chr(9).ToString() & "'" & sScanUser & "'" & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Document Date : " & Strings.Chr(9).ToString() & DateTimeHelper.ToString(dDocDate) & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Last User : " & Strings.Chr(9).ToString() & "'" & sLastUser & "'" & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Last Date : " & Strings.Chr(9).ToString() & DateTimeHelper.ToString(dLastDate) & Strings.Chr(10).ToString()
                        sMsg = sMsg & "Volume ID : " & Strings.Chr(9).ToString() & CStr(iVolumeID) & Strings.Chr(10).ToString()

                        ' Add all the info from the page table
                        For Each vPageList_item As Object In vPageList
                            sMsg = sMsg & Strings.Chr(10).ToString() & "'" & CStr(vPageList_item) & "'"
                        Next vPageList_item

                        MessageBox.Show(sMsg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

                    End If
                Catch ex As Exception

                End Try
            End If

            'delete pressed? Then delete doc
            If eventArgs.KeyCode = Keys.Delete Then

                'save current control
                m_cntCurrent = lvwDocList

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                DeleteNodes()

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

                m_cntCurrent = Nothing

            End If

            'return pressed? Then view doc
            'JH231298 or open folder if it is a folder
            If eventArgs.KeyCode = Keys.Return Then
                If lvwDocList.FocusedItem.Name.Substring(0, 1) = ACDocument Then
                    'If lvwDocList.listViewHelper1.GetListViewSubItem(lvwDocList.FocusedItem, 2).Text = DOCEML Then
                    If ListViewHelper.GetListViewSubItem(lvwDocList.FocusedItem, 2).Text = DOCEML Then
                        oMessage = New frmMessage()
                        oMessage.GetMessage(sMessage:="Loading Document " & lvwDocList.FocusedItem.Text, lSeconds:=10)
                        Application.DoEvents()
                        m_bRefresh = True
                    End If

                    ViewDocument()
                    If m_bRefresh Then
                        If Not (m_oViewer Is Nothing) Then

                            m_oViewer.Dispose()
                            m_oViewer = Nothing
                        End If
                    End If
                Else
                    sKey = lvwDocList.FocusedItem.Name

                    'add the node to tvw first
                    m_lReturn = ExtractNumFromKey(sKey, lFolderNum)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    LocateFolder(lFolderNum, False)

                    m_lReturn = GetMyFolders(tvw:=tvwMain, bAddToView:=False, sTempKey:=sKey)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                End If
            End If

            'Secret admin keys pressed ?
            '(ie ALT, G, O, D)
            'This will temporarily make you an administrator. Intended for engineer
            'when they first set an administrator
            If eventArgs.Shift Then
                Select Case eventArgs.KeyCode
                    Case Keys.G
                        keyComb = "G"
                    Case Keys.O
                        keyComb = keyComb + "O"
                    Case Keys.D
                        keyComb = keyComb + "D"
                    Case Else
                        keyComb = ""
                End Select
            Else
                keyComb = ""
            End If
            If keyComb = "GOD" Then
                MessageBox.Show("You now have administrator access untill you exit.", DOCAppName)
                g_bUserIsAdministrator = True
            End If

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_KeyDown", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End Try
    End Sub

    Private Sub lvwDocList_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwDocList.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        Dim sKey As String = ""
        Dim bNodesSelected, bCurrentNodeSelected, bMulti As Boolean


        Try

            'have we right clicked on a node
            'If Button = 2 Then
            If eventArgs.Button = MouseButtons.Right Then
                m_lReturn = NodeClicked(lvwDocList)

                Select Case m_lReturn
                    Case gPMConstants.PMEReturnCode.PMFalse
                        'Node not clicked on, so exit
                        Exit Sub
                    Case gPMConstants.PMEReturnCode.PMTrue
                        'Fine, continue
                    Case Else
                        'problem, so go.
                        Exit Sub

                End Select
            Else
                'menu items available may have been affected - check
                EnableMenuItems(lvwDocList)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If

            'enable all menu items possibly applicable to this control
            EnableMenuItems(lvwDocList)

            'Remaining logic adjusts what menu items are available depending on whether
            'ctrl/shift is pressed, what type of listitem, is selected, how many etc...

            If lvwDocList.GetItemAt(X, Y) Is Nothing Then
                Exit Sub
            End If
            'store key of node clicked
            sKey = lvwDocList.GetItemAt(X, Y).Name

            'determine if any items currently selected
            bNodesSelected = False
            For i As Integer = 1 To lvwDocList.Items.Count
                If lvwDocList.Items.Item(i - 1).Selected Then
                    bNodesSelected = True
                    Exit For
                End If
            Next i

            'determine if ctrl or shift pressed
            bMulti = False
            If (Shift = ShiftConstants.ShiftMask) Or (Shift = ShiftConstants.CtrlMask) Or (Shift = ShiftConstants.ShiftMask + ShiftConstants.CtrlMask) Then
                bMulti = True
            End If

            'Is clicked node currently selected?
            bCurrentNodeSelected = False
            If lvwDocList.Items.Item(sKey).Selected Then
                bCurrentNodeSelected = True
            End If

            'Affect menu items accordingly

            'in following cases just select the node that has been right clicked
            If ((Not bCurrentNodeSelected) And (bNodesSelected) And (Not bMulti)) Or ((Not bCurrentNodeSelected) And (Not bNodesSelected) And (Not bMulti)) Or ((bCurrentNodeSelected) And (bNodesSelected) And (Not bMulti)) Then

                'remove options,according to whether doc or folder

                Select Case sKey.Substring(0, 1)
                    Case ACDocument
                        'document - leave all popup menu items

                    Case ACFolder
                        'folder - cant so some stuff
                        mnuPopOpenDocument.Enabled = False
                        'mnuPopRename.Enabled = False
                        mnuPopAddKeyword.Enabled = False
                        mnuPopAddAnn.Enabled = False
                        mnuPopInformation.Enabled = False

                        'some stuff only want to do when in main mode
                        If m_iViewMode <> DOCViewModeMain Then
                            mnuPopPassword.Enabled = False
                            mnuPopAccess.Enabled = False
                            mnuPopCut.Enabled = False
                            mnuPopCopy.Enabled = False
                            mnuPopDelete.Enabled = False
                        End If

                    Case Else
                        'Hmm. best go
                        Exit Sub

                End Select

                'unselect all items
                lvwDocList.MultiSelect = False

                'select the right clicked item
                lvwDocList.FocusedItem = lvwDocList.Items.Item(lvwDocList.GetItemAt(X, Y).Name)

                lvwDocList.MultiSelect = True

                UpdateStatusBar(lvwDocList)

                'save current control
                m_cntCurrent = lvwDocList
                Ctx_mnuPop.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                'm_cntCurrent = Nothing

            End If

            'in following cases multi nodes have been selected, so reduce options available
            If ((bCurrentNodeSelected) And (bNodesSelected) And (bMulti)) Or ((bCurrentNodeSelected) And (Not bNodesSelected) And (bMulti)) Or ((Not bCurrentNodeSelected) And (bNodesSelected) And (bMulti)) Then

                mnuPopOpenDocument.Enabled = False
                mnuPopAddKeyword.Enabled = False
                mnuPopAddAnn.Enabled = False
                mnuPopPassword.Enabled = False
                mnuPopAccess.Enabled = False
                mnuPopRename.Enabled = False
                mnuPopInformation.Enabled = False

                'if we are not in main view, then lets not even think about
                'this devilish combination
                If m_iViewMode <> DOCViewModeMain Then
                    Exit Sub
                End If

                'save current control
                m_cntCurrent = lvwDocList
                Ctx_mnuPop.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)

            End If

        Catch excep As System.Exception
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_MouseDown", excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub lvwDocsOnly_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDocsOnly.Enter

        EnableMenuItems(lvwDocsOnly)

    End Sub


    Private Sub lvwDocsOnly_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwDocsOnly.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        'Bring Popup menu if right click on a node
        If Button = 2 Then
            m_lReturn = NodeClicked(lvwDocsOnly)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Node not clicked on, so exit
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Fine, continue
                    EnableMenuItems(lvwDocsOnly)

                    'save current control
                    m_cntCurrent = lvwDocsOnly
                    Ctx_mnuPop.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                    m_cntCurrent = lvwDocsOnly

                Case Else
                    'problem, so go.
                    Exit Sub

            End Select
        End If

    End Sub

    Private Sub lvwDocsOnly_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwDocsOnly.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = eventArgs.X
        Dim Y As Single = eventArgs.Y

        'Always keep track of where mouse is on a control, so can check if its over a
        'node when clicked
        m_lX = CInt(X)
        m_lY = CInt(Y)

    End Sub

    Private Sub lvwKeyWords_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwKeyWords.Click

        'this may affect menu items available
        EnableMenuItems(lvwKeyWords)

    End Sub

    Private Sub lvwKeyWords_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwKeyWords.Enter

        'this may affect menu items available
        EnableMenuItems(lvwKeyWords)

    End Sub


    Private Sub lvwKeyWords_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwKeyWords.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        'delete pressed ?
        If eventArgs.KeyCode = Keys.Delete Then

            'save current control
            m_cntCurrent = lvwKeyWords

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            DeleteDocKeyword()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            m_cntCurrent = Nothing

        End If

    End Sub

    Private Sub lvwKeyWords_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwKeyWords.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = eventArgs.X
        Dim Y As Single = eventArgs.Y


        Try

            'Bring Popup menu if right click on a node
            If Button = MouseButtons.Right Then
                If lvwKeyWords.Items.Count > 0 Then

                    m_lReturn = NodeClicked(lvwKeyWords)

                    Select Case m_lReturn
                        Case gPMConstants.PMEReturnCode.PMFalse
                            'Node not clicked on, continue
                            Exit Sub

                        Case gPMConstants.PMEReturnCode.PMTrue
                            'Fine, continue
                            EnableMenuItems(lvwKeyWords)

                            lvwKeyWords.FocusedItem = lvwKeyWords.Items.Item(lvwKeyWords.GetItemAt(X, Y).Name)

                            'save current control
                            m_cntCurrent = lvwKeyWords
                            Ctx_mnuPop.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                            m_cntCurrent = lvwKeyWords

                        Case Else
                            'problem, so go.
                            Exit Sub

                    End Select
                End If
            Else

                'may have changed permissable menu items
                EnableMenuItems(lvwKeyWords)

            End If

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="lvwKeyWords_MouseDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwKeyWords_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwKeyWords.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = eventArgs.X
        Dim Y As Single = eventArgs.Y

        'Always keep track of where mouse is on a control, so can check if its over a
        'node when clicked
        m_lX = CInt(X)
        m_lY = CInt(Y)

    End Sub

    Private Sub lvwPages_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwPages.Enter

        EnableMenuItems(lvwPages)

    End Sub

    Private Sub lvwPages_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPages.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        'Bring Popup menu if right click on a node
        If Button = 2 Then
            m_lReturn = NodeClicked(lvwPages)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Node not clicked on, so exit
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Fine, continue
                    EnableMenuItems(lvwPages)

                    'save current control
                    m_cntCurrent = lvwPages
                    Ctx_mnuPop.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                    m_cntCurrent = lvwPages

                Case Else
                    'problem, so go.
                    Exit Sub

            End Select
        End If

    End Sub

    Private Sub lvwPages_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPages.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        'Always keep track of where mouse is on a control, so can check if its over a
        'node when clicked
        m_lX = CInt(X)
        m_lY = CInt(Y)

    End Sub


    Public Sub mnuFileArchive_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileArchive.Click

        '    If MsgBox("Proceed with document archive?", _
        ''        vbYesNo, "Archive Document") <> vbYes Then
        '        Exit Sub
        '    End If

        m_lReturn = ArchiveDocument()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Cannot run frmInterface.ArchiveDocument", "DocuMaster file Archive Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

    End Sub

    ' MS 22/05/01
    ' Deletes the Cache directory (i.e. dir everything inside <..>\Documaster Cache\Data )
    ' also closes the Viewer and Refreshes the TreeView

    Public Sub mnuToolsClearCache_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuToolsClearCache.Click

        Dim sCacheDir As String = ""

        Try


            ' Read cache dir from the registry
            m_lReturn = GetRegistryValue(sKey:=DOCCacheLocationKey, sSubKey:="Cache", sValue:=sCacheDir, iLocation:=REGISTRY_SYSTEM)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' cache not set so exit
            If sCacheDir = "" Then
                MessageBox.Show("You don't have a local cache directory!", "DocuMaster Cache", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            Else
                ' construct the cache dir in the correct format i.e start at the <..path>\00 tree ..
                sCacheDir = sCacheDir.Trim()
                If Not sCacheDir.EndsWith("\") Then
                    sCacheDir = sCacheDir & "\"
                End If
                ' CTAF 20040106 - Not needed anymore
                'sCacheDir = sCacheDir & "Data"
            End If

            'Cache already cleared so exit
            If Not Directory.Exists(sCacheDir) Then
                MessageBox.Show("Cache is already cleared", "Clear DocuMaster Cache", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If MessageBox.Show(sCacheDir & Environment.NewLine & Environment.NewLine & "Delete contents of this directory?" & Environment.NewLine, "Clear DocuMaster Cache", MessageBoxButtons.YesNoCancel) <> System.Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If

            'close down the Viewer if open
            If Not (m_oViewer Is Nothing) Then

                m_oViewer.Dispose()
                m_oViewer = Nothing
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = DestroyFolderAndAllContents(sCacheDir, 1)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("'Refresh' and re-try 'Clear Cache'" & Environment.NewLine & _
                                "If still unsuccessful please contact your Software Vendor", "Clear DocuMaster Cache Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            Else
                MessageBox.Show("Sucessfully completed", "Clear DocuMaster Cache", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            'refresh main treeview
            mnuViewRefresh_Click(mnuViewRefresh, New EventArgs())

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsClearCache_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Public Sub mnuEditCopy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditCopy.Click


        Try

            m_cntCurrent = Me.ActiveControl

            'Store the selected nodes, with their descriptions
            If TypeOf m_cntCurrent Is ListView Then

                m_lReturn = StoreSelectedNodes(m_sPasteNodes, m_cntCurrent)

                'store that this was a copy
                m_iPasteFlag = ACPasteCopy
            End If

            'If TypeOf m_cntCurrent Is TreeView Then
            If m_cntCurrent.GetType.Name = "TreeView" Then

                ReDim m_sPasteNodes(0)
                m_sPasteNodes(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                m_sPasteNodes(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text

                'store that this was a copy
                m_iPasteFlag = ACPasteCopy
            End If


            m_cntCurrent = Nothing

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuEditCopy_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuEditCut_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditCut.Click


        Try

            m_cntCurrent = Me.ActiveControl

            'Store the selected nodes, with their descriptions
            If TypeOf m_cntCurrent Is ListView Then

                m_lReturn = StoreSelectedNodes(m_sPasteNodes, m_cntCurrent)

                GhostSelectedNodes(m_sPasteNodes, m_cntCurrent)

            End If

            'If TypeOf m_cntCurrent Is TreeView Then
            If m_cntCurrent.GetType.Name = "TreeView" Then

                ReDim m_sPasteNodes(0)
                m_sPasteNodes(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                m_sPasteNodes(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text

            End If

            'store that this was a Cut
            m_iPasteFlag = ACPasteCut

            m_cntCurrent = Nothing

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuEditCut_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Public Sub mnuEditPaste_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditPaste.Click


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_cntCurrent = Me.ActiveControl

            PasteNodes(vPastedDocsArray)

            m_cntCurrent = Me.ActiveControl

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuEditPaste_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuEditSelectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditSelectAll.Click

        Try

            'select all docs in the doc list view
            For i As Integer = 1 To lvwDocList.Items.Count

                lvwDocList.Items.Item(i - 1).Selected = True

            Next i

            'give it focus
            lvwDocList.Focus()

            'update with number of objects selected
            UpdateStatusBar(lvwDocList)

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuEditSelectAll_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'MS 01/06/01

    ' Archive the document into DME and also create an Event record in SBO

    Function ArchiveIt() As Integer

        Dim lFolderNum, lDrawerNum, lPartyCnt As Integer
        Dim sFolderCode, sDrawerExCode, sDescription As String
        Dim vClaimCnt As String = ""
        Dim vInsuranceFolderCnt As Integer

        Try

            'only run archive if SBO is installed - TO DO?

            'set flag to disable msgboxes when performing move in paste function
            m_bArchive = True

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'cut command was activated from Viewer, now just paste it into destination folder
            If m_iPasteFlag <> ACPasteCut Then
                m_iPasteFlag = ACPasteCut
            End If

            mnuEditPaste_Click(mnuEditPaste, New EventArgs())

            'it is now in the new destination folder, get document's folder num
            If Not (vPastedDocsArray Is Nothing) Then
                For I As Integer = vPastedDocsArray.GetLowerBound(1) To vPastedDocsArray.GetUpperBound(1)
                    g_lArchiveDocNum = vPastedDocsArray(0, I)
                    m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Document, lNodeNum:=g_lArchiveDocNum, lParentNum:=lFolderNum)
                    '.. and it's ex code

                    m_lReturn = g_oBusiness.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, sExCode:=sFolderCode)

                    'now get folder's parent i.e. drawer num

                    m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, lParentNum:=lDrawerNum)
                    ' .. and it's ex code

                    m_lReturn = g_oBusiness.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lDrawerNum, sExCode:=sDrawerExCode)


                    'check if external code exist
                    If (sDrawerExCode = "") Or (sFolderCode = "") Then
                        'MsgBox "Destination folder does not have an external code", _
                        'vbOKOnly + vbExclamation, "Archive Document - Event not logged"

                        m_bArchive = False
                        'force re-select docment
                        g_lArchiveDocNum = 0
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Function
                    Else
                        sDrawerExCode = sDrawerExCode.Trim()
                        sFolderCode = sFolderCode.Trim()
                    End If


                    'if it's a claims folder, then truncate the C prefix
                    If sFolderCode.Substring(0, 1) = "C" Then
                        vClaimCnt = Mid(sFolderCode, 2)
                    Else

                        vClaimCnt = Nothing
                    End If


                    'the client ex_code will actually be used for the lPartyCnt
                    'if no ex_code is supplied then default to 0
                    If sDrawerExCode = "" Then
                        lPartyCnt = 0
                    Else
                        lPartyCnt = CInt(sDrawerExCode)

                        'if  general folder then show in Event log as it's the client folder
                        If sFolderCode = "GENERAL" Then
                            sFolderCode = ""
                        End If
                    End If


                    'create an event for this in SBO

                    ' I'm afraid I had to hard code it in as there was no other way
                    ' lEventcnt = 0 as adding a new event will generate the count automatically in SBO
                    ' DocumentTypeId = 5 i.e Standard Letter
                    ' EventTypeId =  PMBEventDocument i.e. 10

                    sDescription = "Archived file - " & Interaction.InputBox("Enter a description for this Event", "Archive file")

                    If sFolderCode = "" Then

                        vInsuranceFolderCnt = Nothing
                    Else
                        'PN 14151 added an extra check for the Claim folder
                        If sFolderCode.Substring(0, 1) = "C" Then
                            'vInsuranceFolderCnt = CLng(Mid(sFolderCode, 2))
                            'PN 32440

                            m_lReturn = g_oBusiness.GetInsuranceFolderCnt(vClaimCnt:=vClaimCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt)
                        Else
                            vInsuranceFolderCnt = CInt(sFolderCode)
                        End If
                    End If



                    m_lReturn = g_oBusiness.CreateEventInSBO(lEventCnt:=0, lPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=DBNull.Value, vClaimCnt:=vClaimCnt, lDocNum:=g_lArchiveDocNum, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentTypeId:=5, vReportTypeId:=DBNull.Value, lEventTypeId:=10, dtEventDate:=DateTime.Today, sDescription:=sDescription)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to create Event record in Sirius", "File Archive", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                Next

            End If

            'create event succeeded, reset settings
            m_bArchive = False
            g_lArchiveDocNum = 0

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            m_bArchive = False
            g_lArchiveDocNum = 0
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveIt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function
        End Try

    End Function

    Public Sub mnuFileDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileDelete.Click

        Try

            'delete nodes
            m_cntCurrent = Me.ActiveControl

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Select Case m_cntCurrent.Name
                Case "lvwDocList"
                    DeleteNodes()

                Case "tvwMain"

                    'JH051198
                    If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                        Exit Sub
                    End If

                    DeleteNodes()

                Case "lvwAnnotations"
                    DeleteAnnotation()

                Case "lvwKeyWords"
                    DeleteDocKeyword()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuFileExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExit.Click

        'We've come to the end
        Me.Close()

    End Sub

    Public Sub mnuFileFilter_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileFilter.Click

        Try

            m_cntCurrent = Me.ActiveControl

            Select Case m_cntCurrent.Name
                Case "tvwMain"

                    'JH051198
                    If Not tvwMain.SelectedNode Is Nothing Then
                        If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                            MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                            Exit Sub
                        End If
                        'filter
                        Filter()
                    End If



                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control - " & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileFilter_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileFilter_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    'DN 12/12/00
    'Public Sub mnuFileEmail_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileEmail.Click

    '	Dim vPageArray() As Object
    '	Dim lDocNum As Integer
    '	Dim sFilename As String = ""
    '	Dim sSendTo, sUnZipPath, sSendFile, sTempDir As String
    '	Dim bZipped As Boolean

    '	' CTAF 20040106 Start
    '	Dim lPages As Integer
    '	Dim sNewFilename As String = ""
    '	Dim lEventCnt As Integer

    '	Dim ofrmEmail As New frmEmail

    '	'Get the doc num
    '	m_lReturn = ExtractNumFromKey(lvwDocList.FocusedItem.Name, lDocNum)

    '	' save the subject text
    '	Dim sSubject As String = lvwDocList.FocusedItem.Text

    '	'Get the page file path
    '	
    '	m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

    '	If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in send email of '" & lvwDocList.FocusedItem.Text &  _
    '		           "'. Failed to get pages.", vApp:=ACApp, vClass:=ACClass, vMethod:="EmailDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

    '	Else
    '		'Get the first filename
    '		sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))

    '	End If

    '	' CTAF 20040106 Start
    '	' CTAF 20040106 Detect multi page TIF files and count the correct page number

    '	'Populate page files and
    '	For i As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0) - 1
    '		ofrmEmail.txtFile.Text = ofrmEmail.txtFile.Text & CStr(vPageArray(i)).Substring(CStr(vPageArray(i)).Length - 6) & ", "
    '	Next i

    '	'single page or last page
    '	ofrmEmail.txtFile.Text = ofrmEmail.txtFile.Text & CStr(vPageArray(vPageArray.GetUpperBound(0))).Substring(CStr(vPageArray(vPageArray.GetUpperBound(0))).Length - 6)


    '	If vPageArray.GetLowerBound(0) = vPageArray.GetUpperBound(0) Then

    '		' 1 page, or its a multipage tif
    '		sFilename = CStr(vPageArray(0))

    '		'check to see if file is zipped. Presume that if first page is zipped then all are
    '		'm_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZipFile:=bZipped)
    '		m_lReturn = ZipCheck(CStr(vPageArray(0)), bZipped)

    '		If Not m_lReturn Then
    '			'error - assume unzipped
    '			bZipped = False
    '		End If

    '		' Unzip (and cache) the image to be loaded into the viewer temporarily
    '		m_lReturn = CacheFile(m_oZipper, sFilename, sNewFilename, m_sCachePath, bZipped)
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			' Error already displayed by CacheFile
    '			Exit Sub
    '		End If

    '		'Total the number of pages in each document we are using.
    '		If sNewFilename.Trim().EndsWith("TIF") Then
    '			IkFile1.FileName = sNewFilename
    '			IkFile1.LoadPage = 0
    '			IkFile1.GetImagefileType()
    '			lPages = IkFile1.FileMaxPage - 1

    '			IkCommon1.ImgHandle = IkFile1.ImgHandle
    '			IkCommon1.FreeMemory()
    '			IkFile1.ImgHandle = 0
    '		Else
    '			lPages = -1
    '		End If

    '	Else

    '		lPages = vPageArray.GetUpperBound(0)

    '	End If

    '	'Populate subject field
    '	'if more than 1 page
    '	If lPages > 0 Then
    '		ofrmEmail.txtSubject.Text = sSubject & " (" & CStr(lPages + 1) & " pages)"
    '	ElseIf lPages = 0 Then 
    '		ofrmEmail.txtSubject.Text = sSubject & " (" & CStr(lPages + 1) & " page)"
    '	Else
    '		ofrmEmail.txtSubject.Text = sSubject
    '	End If

    '	' CTAF 20040106 End

    '	'Show the screen
    '	
    '	ofrmEmail.ShowDialog()

    '	'Pass back the entered info
    '	'sSendTo = ofrmEmail.SendTo
    '	Dim sAddress() As Object = VB6.CopyArray(ofrmEmail.Addresses)

    '	'add a message if required, also inform recipient total pages attached
    '	Dim sMessage As String = ofrmEmail.SendNote & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &  _
    '	                         " (" & CStr(lPages + 1) & " page file attached)"

    '	'Exit Send Email if cancelled from screen
    '	If ofrmEmail.Status <> gPMConstants.PMEReturnCode.PMOk Then
    '		ofrmEmail.Close()
    '		Exit Sub
    '	End If

    '	ofrmEmail.Close()

    '	'Create Email object
    '	
    '	Dim m_oPMMAPI As iPMMapi.PMMAPI = New iPMMapi.PMMAPI()

    '	
    '	If m_oPMMAPI.Session() Is Nothing Then
    '		
    '		m_lReturn = m_oPMMAPI.Initialise
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			m_oPMMAPI = Nothing
    '			Exit Sub
    '		End If
    '	End If


    '	'Add a message to the Messages collection
    '	
    '	m_lReturn = m_oPMMAPI.messages.AddMessage(v_vSubject:=sSubject, v_vNoteText:=sMessage)

    '	If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '		m_oPMMAPI = Nothing
    '		Exit Sub
    '	End If


    '	'The LastItem is a Message
    '	
    '	With m_oPMMAPI.messages.LastItem
    '		For	Each sAddress_item As Object In sAddress
    '			'Add a Recipient
    '			
    '			m_lReturn = .Recipients.AddRecipient(v_vName:=sAddress_item, v_vRecipientType:=gPMConstants.PMEMapiRecipientTypes.pmeMapiToList, v_vAddressBook:=True)

    '			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '				m_oPMMAPI = Nothing
    '				Exit Sub
    '			End If
    '		Next sAddress_item
    '		'in order for the user to view documents outside DME, if the files are zipped
    '		'then we need to unzip them attach it to the email.

    '		'check to see if file is zipped. Presume that if first page is zipped then all are
    '		
    '		m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZipFile:=bZipped)

    '		If Not m_lReturn Then
    '			'error - assume unzipped
    '			bZipped = False
    '		End If

    '		'get the system temp dir  "<Drive>:\Temp" and extract the zip files there
    '		sTempDir = Interaction.Environ("TEMP") & "\"

    '		'attach each page of the document
    '		For	Each vPageArray_item As Object In vPageArray
    '			If bZipped Then
    '				'get the filename and unzip into the temp dir
    '				sFilename = sTempDir & CStr(vPageArray_item).Substring(CStr(vPageArray_item).Length - 6)
    '				
    '				m_lReturn = m_oZipper.UnZipFile(CStr(vPageArray_item), sFilename)
    '			Else
    '				'get file from server unc
    '				sFilename = CStr(vPageArray_item)
    '			End If

    '			'Call the Attachment the same as the document
    '			
    '			m_lReturn = .Attachments.AddAttachment(v_vName:=sFilename, v_vPath:=sFilename)

    '			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '				m_oPMMAPI = Nothing
    '				Exit Sub
    '			End If

    '		Next vPageArray_item

    '		
    '		m_lReturn = .Send

    '	End With

    '	'now delete all the unzipped files from system temp dir
    '	If bZipped Then
    '		For	Each vPageArray_item_2 As Object In vPageArray
    '			sFilename = sTempDir & CStr(vPageArray_item_2).Substring(CStr(vPageArray_item_2).Length - 6)
    '			If FileSystem.Dir(sFilename, FileAttribute.Normal) > "" Then
    '				'reset read-only file attribute in order to delete
    '				Dim fileInfo As FileInfo = New FileInfo(sFilename)
    '				fileInfo.Attributes = FileAttribute.Normal
    '				' boom, you're  a dead file
    '				m_lReturn = KillFile(sFilename)
    '			End If
    '		Next vPageArray_item_2
    '	End If

    '	'Destroy the Email object
    '	m_oPMMAPI = Nothing

    '	' Now we've finished the email process create an event for it
    '	
    '	m_lReturn = g_oBusiness.CopyEventInSBO(lEventCnt:=lEventCnt, lDocNum:=lDocNum, dtEventDate:=DateTime.Now, sDescriptionPrefix:="Emailed:")

    '	Exit Sub



    '	iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileEmail_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

    '	Exit Sub

    'End Sub
    Public Sub mnuFileEmail_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileEmail.Click

        Dim vPageArray, vAttachments As Object
        Dim lDocNum, lPages, lEventCnt As Integer
        Dim sFilename As String = ""
        Dim sTempDir As String
        Dim bZipped As Boolean
        Dim oOutlook As Outlook
        ' CTAF 20040106 Start
        Dim result As Integer = 0
        Dim sNewFilename As String = ""
        Dim sEmailSubject As String = ""

        Try

            If lvwDocList.SelectedItems.Count = 0 Then
                Exit Sub
            End If
            'Get the doc num
            m_lReturn = ExtractNumFromKey(lvwDocList.SelectedItems(0).Name, lDocNum)

            ' save the subject text
            Dim sSubject As String = lvwDocList.SelectedItems(0).Text

            'Get the page file path
            m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in send email of '" & lvwDocList.SelectedItems(0).Text & _
                           "'. Failed to get pages.", vApp:=ACApp, vClass:=ACClass, vMethod:="EmailDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Else
                'Get the first filename
                sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))

            End If

            oOutlook = New Outlook()
            m_lReturn = oOutlook.Initialise()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Dim tempFileName As String = String.Empty
                If sFilename <> "" Then
                    ReDim vAttachments(0)
                    Dim cloudHostingOptionValue As String = ""
                    m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)

                    If (gPMFunctions.NullToString(cloudHostingOptionValue) = "1") Then

                        Dim sDataPath As String = String.Empty
                        m_lReturn = g_oBusiness.GetDataPath(lVolumeID:=DOCHD1_ID, sDataPath:=sDataPath)
                        Dim s3FileName As String = sFilename.Substring(sDataPath.Length).Replace("\", "/").TrimStart("/")
                        Dim repository As IS3Repository = New S3Repository(Environment.GetEnvironmentVariable("AWS_DME_BUCKET_NAME"),
                            Environment.GetEnvironmentVariable("AWS_REGION"), g_sUserName)
                        'Get Temporary Location
                        Dim tmpFolder As String = sDataPath & "\tmp\"
                        tempFileName = tmpFolder & sFilename.Substring(sFilename.LastIndexOf("\") + 1)

                        m_lReturn = MakePath(tempFileName)

                        m_lReturn = repository.DownloadFileAsync(s3FileName, tmpFolder).Result

                        vAttachments(0) = tempFileName
                    Else
                        vAttachments(0) = sFilename
                    End If
                End If

                lPages = vPageArray.GetUpperBound(0)

                If lPages > 0 Then
                    sEmailSubject = sSubject & " (" & CStr(lPages + 1) & " pages)"
                ElseIf lPages = 0 Then
                    sEmailSubject = sSubject & " (" & CStr(lPages + 1) & " page)"
                Else
                    sEmailSubject = sSubject
                End If

                m_lReturn = oOutlook.NewEmail("", sEmailSubject, , vAttachments, Nothing, "", False)

                oOutlook.Dispose()
                oOutlook = Nothing
                KillFile(tempFileName)
            Else

                ' CTAF 20040106 Start
                ' CTAF 20040106 Detect multi page TIF files and count the correct page number
                Dim ofrmEmail As New frmEmail
                'Populate page files and

                For i As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0) - 1


                    ofrmEmail.txtFile.Text = ofrmEmail.txtFile.Text & CStr(vPageArray(i)).Substring(CStr(vPageArray(i)).Length - 6) & ", "
                Next i

                'single page or last page



                ofrmEmail.txtFile.Text = ofrmEmail.txtFile.Text & CStr(vPageArray(vPageArray.GetUpperBound(0))).Substring(CStr(vPageArray(vPageArray.GetUpperBound(0))).Length - 6)



                If vPageArray.GetLowerBound(0) = vPageArray.GetUpperBound(0) Then

                    ' 1 page, or its a multipage tif

                    sFilename = CStr(vPageArray(0))

                    'check to see if file is zipped. Presume that if first page is zipped then all are
                    'm_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZipFile:=bZipped)

                    m_lReturn = ZipCheck(CStr(vPageArray(0)), bZipped)

                    If Not m_lReturn Then
                        'error - assume unzipped
                        bZipped = False
                    End If

                    ' Unzip (and cache) the image to be loaded into the viewer temporarily
                    m_lReturn = CacheFile(m_oZipper, sFilename, sNewFilename, m_sCachePath, bZipped)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Error already displayed by CacheFile
                        Exit Sub
                    End If

                    'Total the number of pages in each document we are using.
                    If sNewFilename.Trim().EndsWith("TIF") Then
                        'TODO: Need to find solution

                        'IkFile1.FileName = sNewFilename
                        '
                        'IkFile1.LoadPage = 0
                        '
                        'IkFile1.GetImagefileType()
                        '
                        'lPages = CInt(IkFile1.FileMaxPage - 1)

                        '
                        '
                        'IkCommon1.ImgHandle = IkFile1.ImgHandle
                        '
                        'IkCommon1.FreeMemory()
                        '
                        'IkFile1.ImgHandle = 0
                    Else
                        lPages = -1
                    End If

                Else


                    lPages = vPageArray.GetUpperBound(0)

                End If

                'Populate subject field
                'if more than 1 page
                If lPages > 0 Then
                    ofrmEmail.txtSubject.Text = sSubject & " (" & CStr(lPages + 1) & " pages)"
                ElseIf lPages = 0 Then
                    ofrmEmail.txtSubject.Text = sSubject & " (" & CStr(lPages + 1) & " page)"
                Else
                    ofrmEmail.txtSubject.Text = sSubject
                End If

                ' CTAF 20040106 End

                'Show the screen
                ofrmEmail.ShowDialog()

                'Pass back the entered info
                'sSendTo = ofrmEmail.SendTo
                Dim sAddress As Object = ofrmEmail.Addresses

                'add a message if required, also inform recipient total pages attached
                Dim sMessage As String = ofrmEmail.SendNote & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                         " (" & CStr(lPages + 1) & " page file attached)"

                'Exit Send Email if cancelled from screen
                If ofrmEmail.Status <> gPMConstants.PMEReturnCode.PMOK Then
                    ofrmEmail.Close()
                    Exit Sub
                End If

                ofrmEmail.Close()

                'Create Email object
                Dim m_oPMMAPI As Object = New iPMMapi.PMMAPI()
                If m_oPMMAPI.Session() Is Nothing Then
                    m_lReturn = m_oPMMAPI.Initialise()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_oPMMAPI = Nothing
                        Exit Sub
                    End If
                End If


                'Add a message to the Messages collection
                m_lReturn = m_oPMMAPI.Messages.AddMessage(v_vSubject:=sSubject, v_vNoteText:=sMessage)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_oPMMAPI = Nothing
                    Exit Sub
                End If


                'The LastItem is a Message
                With m_oPMMAPI.Messages.LastItem
                    For iCnt As Integer = sAddress.GetLowerBound(0) To sAddress.GetUpperBound(0)
                        'Add a Recipient
                        m_lReturn = .Recipients.AddRecipient(v_vName:=sAddress(iCnt), v_vRecipientType:=gPMConstants.PMEMapiRecipientTypes.pmeMapiToList, v_vAddressBook:=True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_oPMMAPI = Nothing
                            Exit Sub
                        End If
                    Next iCnt
                    'in order for the user to view documents outside DME, if the files are zipped
                    'then we need to unzip them attach it to the email.

                    'check to see if file is zipped. Presume that if first page is zipped then all are
                    m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZIPFile:=bZipped)

                    If Not m_lReturn Then
                        'error - assume unzipped
                        bZipped = False
                    End If

                    'get the system temp dir  "<Drive>:\Temp" and extract the zip files there
                    sTempDir = Interaction.Environ("TEMP") & "\"

                    'attach each page of the document
                    For i As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0)
                        If bZipped Then
                            'get the filename and unzip into the temp dir
                            sFilename = sTempDir & CStr(vPageArray(i)).Substring(CStr(vPageArray(i)).Length - 6)
                            m_lReturn = m_oZipper.UnZipFile(CStr(vPageArray(i)), sFilename)
                        Else
                            'get file from server unc
                            sFilename = CStr(vPageArray(i))
                        End If

                        'Call the Attachment the same as the document
                        m_lReturn = .Attachments.AddAttachment(v_vName:=sFilename, v_vPath:=sFilename)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_oPMMAPI = Nothing
                            Exit Sub
                        End If

                    Next i

                    m_lReturn = .Send

                End With

                'now delete all the unzipped files from system temp dir
                If bZipped Then
                    For i As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0)
                        sFilename = sTempDir & CStr(vPageArray(i)).Substring(CStr(vPageArray(i)).Length - 6)
                        If FileSystem.Dir(sFilename, FileAttribute.Normal) > "" Then
                            'reset read-only file attribute in order to delete
                            Dim fileInfo As FileInfo = New FileInfo(sFilename)
                            fileInfo.Attributes = FileAttribute.Normal
                            ' boom, you're  a dead file
                            m_lReturn = KillFile(sFilename)
                        End If
                    Next i
                End If

                'Destroy the Email object
                m_oPMMAPI = Nothing

            End If

            ' Now we've finished the email process create an event for it
            m_lReturn = g_oBusiness.CopyEventInSBO(lEventCnt:=lEventCnt, lDocNum:=lDocNum, dtEventDate:=DateTime.Now, sDescriptionPrefix:="Emailed:")

        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileEmail_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        End Try

    End Sub
    Public Sub mnuFileImport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileImport.Click

        Dim sFolderName As String = ""  'MS 250900
        Dim sFolderKey As String = ""  '
        Dim sDocName As String = ""  '

        Try

            m_cntCurrent = Me.ActiveControl


            Select Case m_cntCurrent.Name
                Case "tvwMain"

                    'JH051198
                    If Not tvwMain.SelectedNode Is Nothing Then
                        If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                            MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                            Exit Sub
                        End If

                        'Import a document and get folder name
                        ImportDocument()
                    End If



                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control - " & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileImport_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select


            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileImport_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Public Sub mnuFileInformation_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileInformation.Click

        Try

            m_cntCurrent = Me.ActiveControl

            'display doc info we've clicked on doclist

            Select Case m_cntCurrent.Name
                Case "lvwDocList"
                    DisplayDocumentInformation()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileInformation_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileInformation_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuFileNewFolder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileNewFolder.Click

        Dim sKey, sNewKey As String
        Dim Node As TreeNode
        Dim lParentNum, lFolderNum As Integer
        Dim dDate As Date
        Dim itmX As ListViewItem


        Try

            'store selected node key



            'sKey = eventSender.ActiveControl.SelectedItem.Key
            sKey = tvwMain.SelectedNode.Name

            If sKey.Substring(0, 3) = "ADD" Then
                MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                Exit Sub
            End If

            m_lReturn = ExtractNumFromKey(sKey, lParentNum)

            If m_lReturn < gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            dDate = DateTime.Now

            'save a new folder in selected folder

            m_lReturn = g_oBusiness.NewFolder(lParentNum:=lParentNum, sFolderName:="New Folder", dCreateDate:=dDate, lFolderNum:=lFolderNum)

            If m_lReturn < gPMConstants.PMEReturnCode.PMTrue Then
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add new folder", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileNewFolder_Click", excep:=New Exception(Information.Err().Description))

                Exit Sub
            End If

            'now add the new nodes key and add it to the tree view
            sNewKey = ACFolder & " " & CStr(CInt(dDate.ToOADate)) & CStr(lFolderNum)
            Node = tvwMain.Nodes.Find(sKey, True)(0).Nodes.Add(sNewKey, "New Folder", "IMGCLOSEDFOLDER")

            'add it to list view, if appropriate
            If (Convert.ToString(lblTitleMain(1).Tag) = sKey) And (Not m_bDocsOnly) Then

                itmX = lvwDocList.Items.Add(sNewKey, CStr(1), "")
                itmX.Text = "New Folder"
                ListViewHelper.GetListViewSubItem(itmX, 1).Text = DateTimeHelper.ToString(DateTime.FromOADate(CInt(dDate.ToOADate)))

                itmX.ImageKey = "IMGCLOSEDFOLDER"

            End If

            'now select it and a offer a rename, then reselect original

            'tvwMain.Nodes.Item(sNewKey).Selected = True
            'tvwMain.Nodes.Item(sNewKey).Selected = True
            tvwMain.SelectedNode = tvwMain.Nodes.Find(tvwMain.Nodes.Find(sNewKey, True)(0).Name, True)(0)
            If Not (tvwMain.SelectedNode Is Nothing) Then
                tvwMain.SelectedNode.BeginEdit()
            End If


            'tvwMain.Nodes.Item(sKey).Selected = True
            tvwMain.SelectedNode = tvwMain.Nodes.Find(tvwMain.Nodes.Find(sKey, True)(0).Name, True)(0)
        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileNewFolder_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuFileOpenDocument_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileOpenDocument.Click
        Dim oMessage As frmMessage


        Try

            m_cntCurrent = Me.ActiveControl

            If m_cntCurrent.Name = "lvwDocList" Then
                If lvwDocList.FocusedItem.Name.Substring(0, 1) = ACDocument Then
                    'If lvwDocList.listViewHelper1.GetListViewSubItem(lvwDocList.FocusedItem, 2).Text = DOCEML Then
                    If ListViewHelper.GetListViewSubItem(lvwDocList.FocusedItem, 2).Text = DOCEML Then
                        oMessage = New frmMessage()
                        oMessage.GetMessage(sMessage:="Loading Document " & lvwDocList.FocusedItem.Text, lSeconds:=10)
                        Application.DoEvents()
                        m_bRefresh = True
                    End If
                End If

                'view the document
                ViewDocument()
                If m_bRefresh Then
                    If Not (m_oViewer Is Nothing) Then

                        m_oViewer.Dispose()
                        m_oViewer = Nothing
                    End If
                End If
            End If

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileOpenDocument_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuFilePrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFilePrint.Click
        Try
            ' OK, printer's all set, lets print selected documents
            PrintDocument(True)

            'Exit Sub

        Catch ex As System.Exception
            Select Case Information.Err().Number
                ' This is generated when the user clicks on the Cancel button
                Case DialogResult.Cancel
                    ' Exit ok, no errors
                    Exit Sub

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process mnuPrint_Click.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPrint_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            End Select
        End Try
    End Sub

    Public Sub mnuFileRename_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileRename.Click
        Dim name As String
        Try

            'rename node
            m_cntCurrent = Me.ActiveControl

            'store the control, node key and name before edit
            ' m_cntRename = m_cntCurrent
            If TypeOf (m_cntCurrent) Is TreeView Then
                m_cntCurrent = CType(m_cntCurrent, TreeView)
                m_cntRename = CType(m_cntCurrent, TreeView)
                m_sRenameNode.Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                m_sRenameNode.Text = CType(m_cntCurrent, TreeView).SelectedNode.Text
                name = CType(m_cntCurrent, TreeView).Name
            ElseIf TypeOf (m_cntCurrent) Is ListView Then
                m_cntCurrent = CType(m_cntCurrent, ListView)
                m_cntRename = CType(m_cntCurrent, ListView)
                m_sRenameNode.Key = CType(m_cntCurrent, ListView).SelectedItems(0).Name
                m_sRenameNode.Text = CType(m_cntCurrent, ListView).SelectedItems(0).Text
                name = CType(m_cntCurrent, ListView).Name

            End If
            'm_sRenameNode.Key = m_cntCurrent.FocusedItem.Name
            'm_sRenameNode.Text = m_cntCurrent.FocusedItem.Text

            'note bug in listview - if using startlabeledit method, have
            'to explictly name the control
            Select Case name
                Case "lvwDocList"
                    If Not (lvwDocList.FocusedItem Is Nothing) Then
                        lvwDocList.FocusedItem.BeginEdit()
                    End If

                Case "tvwMain"

                    'JH051198
                    If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                        MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                        Exit Sub
                    End If

                    If Not (tvwMain.SelectedNode Is Nothing) Then
                        tvwMain.SelectedNode.BeginEdit()
                    End If
                    'WR77 Documaster Enhancements START
                Case "lvwBCDocs"
                    'WR77 Documaster Enhancements END
                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileRename_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select


            Exit Sub

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileRename_Click", excep:=excep)

            Exit Sub


        End Try

    End Sub



    Public Sub mnuFileSaveAs_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileSaveAs.Click

        Try

            m_cntCurrent = Me.ActiveControl


            Select Case m_cntCurrent.Name
                Case "lvwDocList"

                    'export a document
                    ExportDocument()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control - " & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileSaveAs_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileSaveAs_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuFileScan_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileScan.Click

        Try

            'cal the scan app
            m_cntCurrent = Me.ActiveControl

            Select Case m_cntCurrent.Name
                Case "lvwDocList"
                    Scan()

                Case "tvwMain"

                    'JH051198
                    If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                        Exit Sub
                    End If

                    Scan()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileScan_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileScan_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuFileSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileSelect.Click

        'show the selectfolders form

        m_lReturn = GetMyFolders(tvw:=tvwMain, bAddToView:=True, sTempKey:=tvwMain.SelectedNode.Name, bMenu:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileSelect_Click", excep:=New Exception(Information.Err().Description))
            Exit Sub
        End If

    End Sub

    ' **********************************************************************
    '
    ' Function    : mnuHelpAbout
    '
    ' Description : Displays the standard Policy Master about screen modally
    '
    ' **********************************************************************
    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        ' Code needed to display standard PM About screen


        Dim oSirAbout As iPMAbout.Interface_Renamed

        Dim sTitle, sVersionNumber, sVersionDate, sComponent As String

        Try

            ' Set the application title
            sTitle = "DocuMaster Enterprise"

            ' Set the version number and date
            'DN 16/02/01 - Get the Version from the Registry
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=PMRegKeyVersion, r_sSettingValue:=sVersionNumber)


            sVersionDate = DateTimeHelper.ToString((New FileInfo(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe")).LastWriteTime)
            sVersionDate = sVersionDate.Substring(0, 8)

            ' Create the object
            oSirAbout = New iPMAbout.Interface_Renamed()

            ' Initialise it. No parameters

            m_lReturn = CType(oSirAbout, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            ' Display the about screen modally

            m_lReturn = oSirAbout.Show(sTitle:=sTitle, sVersionNumber:=sVersionNumber, sVersionDate:=sVersionDate, sComponent:=sComponent)

            ' Terminate it, and...

            oSirAbout.Dispose()

            ' ...remove it from memory
            oSirAbout = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuHelpAbout_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuPopAccess_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopAccess.Click

        Try


            Select Case m_cntCurrent.Name
                'set access level of the selected node
                Case "lvwDocList", "tvwMain"
                    SetNodeAccessLevel()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopAccess_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopAccess_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Public Sub mnuPopAddAnn_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopAddAnn.Click


        Try

            If m_cntCurrent.Name = "lvwDocList" Then
                'add an annotation
                AddAnnotation()
            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopAddAnn_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Public Sub mnuPopAddKeyword_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopAddKeyword.Click


        Try

            If m_cntCurrent.Name = "lvwDocList" Then
                'add a keyword
                AddKeyword()
            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopAddKeyword_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'MS 10/05/01
    Public Sub mnuPopArchive_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopArchive.Click

        mnuFileArchive_Click(mnuFileArchive, New EventArgs())

    End Sub

    Public Sub mnuPopCopy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopCopy.Click


        Try

            'Store the selected nodes, with their descriptions
            If TypeOf m_cntCurrent Is ListView Then

                m_lReturn = StoreSelectedNodes(m_sPasteNodes, m_cntCurrent)
                'store that this was a copy
                m_iPasteFlag = ACPasteCopy

            End If

            'If TypeOf m_cntCurrent Is TreeView Then
            If m_cntCurrent.GetType.Name = "TreeView" Then

                ReDim m_sPasteNodes(0)
                m_sPasteNodes(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                m_sPasteNodes(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text

                'store that this was a copy
                m_iPasteFlag = ACPasteCopy

            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopCopy_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Public Sub mnuPopCut_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopCut.Click


        Try

            'Store the selected nodes, with their descriptions
            If TypeOf m_cntCurrent Is ListView Then

                m_lReturn = StoreSelectedNodes(m_sPasteNodes, m_cntCurrent)

                GhostSelectedNodes(m_sPasteNodes, m_cntCurrent)

                'store that this was a Cut
                m_iPasteFlag = ACPasteCut
            End If

            'If TypeOf m_cntCurrent Is TreeView Then
            If m_cntCurrent.GetType.Name = "TreeView" Then

                ReDim m_sPasteNodes(0)
                m_sPasteNodes(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                m_sPasteNodes(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text

                'store that this was a Cut
                m_iPasteFlag = ACPasteCut
            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopCut_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Public Sub mnuPopDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopDelete.Click


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Select Case m_cntCurrent.Name
                Case "lvwDocList", "tvwMain"
                    DeleteNodes()

                Case "lvwAnnotations"
                    DeleteAnnotation()

                Case "lvwKeyWords"
                    DeleteDocKeyword()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Public Sub mnuPopFilter_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopFilter.Click


        Try


            Select Case m_cntCurrent.Name
                Case "tvwMain"
                    'expand node by filter
                    Filter()

                Case Else
                    'Mmm.

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopDelete_Click", vErrNo:=Information.Err(), vErrDesc:=Information.Err().Description)

            End Select

            'this ensures this right clicked node remains selected
            m_bLeaveNodeSelected = True

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopFilter_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Public Sub mnuPopFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopFind.Click

        Dim lNum As Integer


        Try

            If TypeOf m_cntCurrent Is TreeView Then
                If CType(m_cntCurrent, TreeView).SelectedNode.Name.Substring(0, 3) = "ADD" Then
                    MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                    Exit Sub

                End If

                m_lReturn = ExtractNumFromKey(CType(m_cntCurrent, TreeView).SelectedNode.Name, lNum)

                Find(lNum, CType(m_cntCurrent, TreeView).SelectedNode.Text)
            End If
        Catch excep As System.Exception
            '   End If



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopFind_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Public Sub mnuPopInformation_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopInformation.Click


        Try

            'display doc info we've clicked on doclist

            Select Case m_cntCurrent.Name
                Case "lvwDocList"
                    DisplayDocumentInformation()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopInformation_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuPopNewFolder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopNewFolder.Click

        Dim sNewKey As String
        Dim Node As TreeNode
        Dim lFolderNum As Integer
        Dim dDate As Date



        Try

            'store current selected node key
            'sKey = m_cntCurrent.FocusedItem.Name
            'If Not CType(m_cntCurrent, TreeView).SelectedNode Is Nothing Then
            '    sKey = CType(m_cntCurrent, TreeView).SelectedNode.Name
            'End If


            dDate = DateTime.Now

            'save a new root folder in selected folder

            m_lReturn = g_oBusiness.NewFolder(lParentNum:=0, sFolderName:="New Folder", dCreateDate:=dDate, lFolderNum:=lFolderNum)

            If m_lReturn < gPMConstants.PMEReturnCode.PMTrue Then
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add new folder", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileNewFolder_Click", excep:=New Exception(Information.Err().Description))

                Exit Sub
            End If

            'now add the new root node's key and add it to the tree view
            sNewKey = ACFolder & " " & CStr(CInt(dDate.ToOADate)) & CStr(lFolderNum)
            Node = tvwMain.Nodes.Add(sNewKey, "New Folder", "IMGCLOSEDFOLDER")


            'now select it and a offer a rename, then reselect original

            'tvwMain.Nodes.Item(sNewKey).Selected = True
            tvwMain.SelectedNode = tvwMain.Nodes.Find(tvwMain.Nodes.Item(sNewKey).Name, True)(0)
            If Not (tvwMain.SelectedNode Is Nothing) Then
                tvwMain.SelectedNode.BeginEdit()
            End If


            'tvwMain.Nodes.Item(sKey).Selected = True
            'tvwMain.SelectedNode = tvwMain.Nodes.Find(tvwMain.Nodes.Find(sKey, True)(0).Name, True)(0)

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileNewFolder_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuPopOpenDocument_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopOpenDocument.Click
        Dim oMessage As frmMessage


        Try

            If m_cntCurrent.Name = "lvwDocList" Then
                If lvwDocList.FocusedItem.Name.Substring(0, 1) = ACDocument Then
                    'If lvwDocList.listViewHelper1.GetListViewSubItem(lvwDocList.FocusedItem, 2).Text = DOCEML Then
                    If ListViewHelper.GetListViewSubItem(lvwDocList.FocusedItem, 2).Text = DOCEML Then
                        oMessage = New frmMessage()
                        oMessage.GetMessage(sMessage:="Loading Document " & lvwDocList.FocusedItem.Text, lSeconds:=10)
                        Application.DoEvents()
                        m_bRefresh = True
                    End If
                End If

                'view the document
                ViewDocument()

                If m_bRefresh Then
                    If Not (m_oViewer Is Nothing) Then

                        m_oViewer.Dispose()
                        m_oViewer = Nothing
                    End If
                End If

            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopOpenDocument_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuPopPassword_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopPassword.Click

        Try


            Select Case m_cntCurrent.Name
                'password the selected node
                Case "lvwDocList", "tvwMain"
                    SetNodePassword()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopPassword_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopPassword_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Public Sub mnuPopPaste_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopPaste.Click


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            PasteNodes()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopPaste_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'DN 12/12/00
    Public Sub mnuPopEmail_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopEmail.Click

        mnuFileEmail_Click(mnuFileEmail, New EventArgs())

    End Sub

    Public Sub mnuPopRename_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopRename.Click

        Try

            'store the control, node key and before name
            m_cntRename = m_cntCurrent
            If m_cntCurrent.GetType.Name = "TreeView" Then
                m_sRenameNode.Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                m_sRenameNode.Text = CType(m_cntCurrent, TreeView).SelectedNode.Text
            End If
            If TypeOf m_cntCurrent Is ListView Then
                m_sRenameNode.Key = CType(m_cntCurrent, ListView).SelectedItems(0).Name
                m_sRenameNode.Text = CType(m_cntCurrent, ListView).SelectedItems(0).Text
            End If
            'note bug in listview - if using startlabeledit method, have
            'to explictly name the control
            Select Case m_cntCurrent.Name
                Case "lvwDocList"
                    If Not (lvwDocList.FocusedItem Is Nothing) Then
                        lvwDocList.FocusedItem.BeginEdit()
                    End If

                Case "tvwMain"
                    If Not (tvwMain.SelectedNode Is Nothing) Then
                        tvwMain.SelectedNode.BeginEdit()
                    End If

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopRename_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopRename_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Public Sub mnuPopScan_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopScan.Click

        Try

            Select Case m_cntCurrent.Name
                Case "lvwDocList", "tvwMain"
                    Scan()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopScan_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopScan_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuPopSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopSelect.Click

        'do the select folders routine
        mnuFileSelect_Click(mnuFileSelect, New EventArgs())

        'this ensures this right clicked node remains selected
        m_bLeaveNodeSelected = True

    End Sub

    Public Sub mnuPopSetHome_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopSetHome.Click

        Try

            ' Set Home folder position
            Select Case m_cntCurrent.Name
                Case "lvwDocList", "tvwMain"
                    SetHomeFolder()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopSetHome_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPopSetHome_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Public Sub mnuPopSubFolder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopSubFolder.Click

        mnuFileNewFolder_Click(mnuFileNewFolder, New EventArgs())

    End Sub

    Public Sub mnuToolsAccess_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuToolsAccess.Click


        Try

            'set access level of selected node
            m_cntCurrent = Me.ActiveControl

            Select Case m_cntCurrent.Name
                Case "lvwDocList", "tvwMain"
                    SetNodeAccessLevel()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsAccess_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsAccess_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuToolsAddAnn_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuToolsAddAnn.Click


        Try

            m_cntCurrent = Me.ActiveControl

            If m_cntCurrent.Name = "lvwDocList" Then
                'add an annotation
                AddAnnotation()
            End If

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsAddAnn_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuToolsAddKeyword_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuToolsAddKeyword.Click


        Try

            m_cntCurrent = Me.ActiveControl

            If m_cntCurrent.Name = "lvwDocList" Then
                'add a Keyword
                AddKeyword()
            End If

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsAddKeyword_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub




    Public Sub mnuToolsFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuToolsFind.Click


        Try

            'find in all folders
            Find(0, "All Folders")

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsFind_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuToolsFindClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuToolsFindClear.Click

        'remove find results
        tvwFind.Nodes.Clear()

        'if we are in find view, then clear doc listview contents too
        If m_iViewMode = DOCViewModeFindResults Then
            lvwDocList.Items.Clear()
        End If

        'update the label
        lblTitleFind(1).Text = ""
        lblTitleFind(1).Tag = ""

    End Sub

    Public Sub mnuToolsGoHome_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuToolsGoHome.Click

        Try


            LocateFolder(g_oBusiness.HomeFolder, False)

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsGoHome_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuToolsPassword_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuToolsPassword.Click

        Try

            'password selected node
            m_cntCurrent = Me.ActiveControl

            Select Case m_cntCurrent.Name
                Case "lvwDocList", "tvwMain"
                    SetNodePassword()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsPassword_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsPassword_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuToolsSetHome_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuToolsSetHome.Click


        Try

            ' Set Home Folder
            m_cntCurrent = Me.ActiveControl

            Select Case m_cntCurrent.Name
                Case "lvwDocList"
                    SetHomeFolder()

                Case "tvwMain"

                    'JH051198
                    If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                        MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                        Exit Sub
                    End If

                    SetHomeFolder()

                Case Else
                    'Mmm.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Control" & m_cntCurrent.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsSetHome_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

            m_cntCurrent = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuToolsSetHome_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuUtilitiesDocNames_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuUtilitiesDocNames.Click

        Dim oDocNameAdmin As iDOCDocNameAdmin.Interface_Renamed


        Try

            'only administrator can maintain document names
            If Not g_bUserIsAdministrator Then
                MessageBox.Show("Only the DocuMaster Administrator can maintain document names.", DOCAppName)
                Exit Sub
            End If

            If oDocNameAdmin Is Nothing Then

                oDocNameAdmin = New iDOCDocNameAdmin.Interface_Renamed()

                'DoEvents

                m_lReturn = oDocNameAdmin.Initialise

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise iDOCDocNameAdmin.Interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuUtilitiesDocNames_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

            End If

            'start it

            m_lReturn = oDocNameAdmin.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start iDOCDocNameAdmin.Interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuUtilitiesDocNames_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub

            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuUtilitiesDocNames_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuUtilitiesEditAccess_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuUtilitiesEditAccess.Click

        Dim frmEdit As frmEditAdmin

        Try

            'only administrator can maintain document names
            If Not g_bUserIsAdministrator Then
                MessageBox.Show("Only the DocuMaster Administrator can set edit access levels.", DOCAppName)
                Exit Sub
            End If

            'show the form modally
            frmEdit = New frmEditAdmin()

            frmEdit.ShowDialog()

            ' if user clicked okay to update access levels, save to database
            If frmEdit.UpdateAccess Then

                'set file access levels

                g_oBusiness.FileCopyLevel = g_iFileCopyLevel

                g_oBusiness.FileDeleteLevel = g_iFileDeleteLevel

                g_oBusiness.FileMoveLevel = g_iFileMoveLevel
                'and now folder

                g_oBusiness.folderCopyLevel = g_iFolderCopyLevel

                g_oBusiness.folderDeleteLevel = g_iFolderDeleteLevel

                g_oBusiness.folderMoveLevel = g_iFolderMoveLevel

                'update access levels to database

                m_lReturn = g_oBusiness.UpdateFileAndFolderAccess()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update edit access levels", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuUtilitiesEditAccess_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                End If

            End If

            frmEdit = Nothing

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mmnuUtilitiesEditAccess_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuUtilitiesKeywords_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuUtilitiesKeywords.Click

        Try

            'only administrator can maintain keywords
            If Not g_bUserIsAdministrator Then
                MessageBox.Show("Only the DocuMaster Administrator can maintain keywords names.", DOCAppName)
                Exit Sub
            End If

            'get the keyword object and initialise it if not already done so
            If m_oKeywordAdmin Is Nothing Then

                m_oKeywordAdmin = New iDOCKeywordAdmin.Interface_Renamed()


                'develpoer guide no. 9
                'm_lReturn = CType(m_oKeywordAdmin, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                m_lReturn = m_oKeywordAdmin.Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise iDOCKeywordAdmin.Interface class", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuUtilitiesKeywords_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If


            m_oKeywordAdmin.UserIsAdministrator = g_bUserIsAdministrator

            'call the admin method

            m_lReturn = m_oKeywordAdmin.AdministerKeywords()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuUtilitiesKeywords_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub


    Public Sub mnuUtilitiesUserAccess_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuUtilitiesUserAccess.Click

        Dim oUserAdmin As iDOCUserAdmin.Interface_Renamed


        Try

            'only administrator can maintain document names
            If Not g_bUserIsAdministrator Then
                MessageBox.Show("Only the DocuMaster Administrator can set user access levels.", DOCAppName)
                Exit Sub
            End If

            If oUserAdmin Is Nothing Then

                oUserAdmin = New iDOCUserAdmin.Interface_Renamed()

                'DoEvents

                m_lReturn = oUserAdmin.Initialise

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise iDOCUserAdmin.Interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuUtilitiesUserAccess_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

            End If

            'start it


            m_lReturn = oUserAdmin.UserAdmin()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start iDOCUserAdmin.Interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuUtilitiesUserAccess_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub

            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuUtilitiesUserAccess_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuViewBC_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewBC.Click

        'Change the view to briefcase mode
        SetViewModeBC()

        'Set up the controls positions, according to the two splitter bars
        ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

    End Sub

    Public Sub mnuViewDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewDetails.Click

        ' Change the list view to details
        lvwDocList.View = View.Details
        'CType(tlbMain.Items.Item(ACLargeIcon), ToolStripButton).Checked = False
        CType(tlbMain.Items.Find("_tlbMain_Button11", True)(0), ToolStripButton).Checked = False
        'CType(tlbMain.Items.Item(ACSmallIcon), ToolStripButton).Checked = False
        CType(tlbMain.Items.Find("_tlbMain_Button12", True)(0), ToolStripButton).Checked = False
        'CType(tlbMain.Items.Item(ACList), ToolStripButton).Checked = False
        CType(tlbMain.Items.Find("_tlbMain_Button13", True)(0), ToolStripButton).Checked = False
        'CType(tlbMain.Items.Item(ACListDetails), ToolStripButton).Checked = True
        CType(tlbMain.Items.Find("_tlbMain_Button14", True)(0), ToolStripButton).Checked = False

        mnuViewLargeIcons.Checked = False
        mnuViewSmallIcons.Checked = False
        mnuViewList.Checked = False
        mnuViewDetails.Checked = True

    End Sub



    'expands all client folders in the main view
    Public Sub mnuViewExpand_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewExpand.Click
        Dim clientNodeToExpand As TreeNode = Nothing
        Dim tn As TreeNode = Nothing
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        If Not tvwMain.SelectedNode Is Nothing Then
            clientNodeToExpand = tvwMain.SelectedNode
            For counter As Integer = 0 To clientNodeToExpand.Nodes.Count - 1
                tn = clientNodeToExpand.Nodes(counter)
                If Not tn Is Nothing Then
                    m_lReturn = ExpandFolderAll(tvw:=tvwMain, sTempKey:=tn.Name)
                End If
            Next
        End If
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

    End Sub

    Public Sub mnuViewExtrasAnnotations_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewExtrasAnnotations.Click

        Dim sNodeKeys() As DOCConst.DOCNodes = Nothing


        Try

            'toggle annotations view
            m_iViewAnnotations = Not m_iViewAnnotations
            lvwAnnotations.Visible = m_iViewAnnotations
            mnuViewExtrasAnnotations.Checked = m_iViewAnnotations

            'switch the button
            'CType(tlbMain.Items.Item(ACAnnotation), ToolStripButton).Checked = (m_iViewAnnotations)
            CType(tlbMain.Items.Find("_tlbMain_Button22", True)(0), ToolStripButton).Checked = (m_iViewAnnotations)

            'if turning on this view, check if there is one selected
            'node and populate for that - else clear it
            If lvwAnnotations.Visible Then

                'get the selected nodes
                m_lReturn = StoreSelectedNodes(sNodeKeys, lvwDocList)

                'if only one selected, then populate annotations for this doc
                If (sNodeKeys.GetUpperBound(0) - sNodeKeys.GetLowerBound(0) + 1) = 1 Then
                    If Not String.IsNullOrEmpty(sNodeKeys(sNodeKeys.GetLowerBound(0)).Key) Then
                        CheckAnnotations(sNodeKeys(sNodeKeys.GetLowerBound(0)).Key)
                    End If
                Else
                    lvwAnnotations.Items.Clear()
                End If

            End If

            'Resize all the forms controls
            ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuViewExtrasAnnotations_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuViewExtrasKeywords_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewExtrasKeywords.Click

        Dim sNodeKeys() As DOCConst.DOCNodes = Nothing


        Try

            'toggle keywords view
            m_iViewKeywords = Not m_iViewKeywords
            lvwKeyWords.Visible = m_iViewKeywords
            mnuViewExtrasKeywords.Checked = m_iViewKeywords

            'switch the button
            'CType(tlbMain.Items.Item(ACKeyword), ToolStripButton).Checked = (m_iViewKeywords)
            If Not CType(tlbMain.Items.Find("_tlbMain_Button21", True)(0), ToolStripButton) Is Nothing Then
                CType(tlbMain.Items.Find("_tlbMain_Button21", True)(0), ToolStripButton).Checked = (m_iViewKeywords)
            End If

            'if turning on this view, check if there is one selected
            'node and populate for that - else clear it
            If lvwKeyWords.Visible Then

                'get the selected nodes
                m_lReturn = StoreSelectedNodes(sNodeKeys, lvwDocList)

                'if only one selected, then populate keywords for this doc
                If (sNodeKeys.GetUpperBound(0) - sNodeKeys.GetLowerBound(0) + 1) = 1 Then
                    If Not String.IsNullOrEmpty(sNodeKeys(sNodeKeys.GetLowerBound(0)).Key) Then
                        CheckKeywords(sNodeKeys(sNodeKeys.GetLowerBound(0)).Key)
                    End If
                Else
                    lvwKeyWords.Items.Clear()
                End If

            End If

            'Resize all the forms controls
            ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuViewExtrasKeywords_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub

    Public Sub mnuViewExtrasPages_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewExtrasPages.Click

        'toggle pages view
        m_iViewPages = Not m_iViewPages
        lvwPages.Visible = m_iViewPages
        mnuViewExtrasPages.Checked = m_iViewPages

        'Resize all the forms controls
        ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

    End Sub

    Public Sub mnuViewFavourites_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewFavourites.Click

        'Change the view to selected favourites
        SetViewModeFavourites()

        'Set up the controls positions, according to the two splitter bars
        ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

    End Sub

    Public Sub mnuViewFindResults_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewFindResults.Click

        'Change the view to find results
        SetViewModeFindResults()

        'Set up the controls positions, according to the two splitter bars
        ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

    End Sub

    Public Sub mnuViewLargeIcons_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewLargeIcons.Click

        ' Change the list view to large icons
        lvwDocList.View = View.LargeIcon
        CType(tlbMain.Items.Find("_tlbMain_Button11", True)(0), ToolStripButton).Checked = True
        CType(tlbMain.Items.Find("_tlbMain_Button12", True)(0), ToolStripButton).Checked = False
        CType(tlbMain.Items.Find("_tlbMain_Button13", True)(0), ToolStripButton).Checked = False
        CType(tlbMain.Items.Find("_tlbMain_Button14", True)(0), ToolStripButton).Checked = False

        mnuViewLargeIcons.Checked = True
        mnuViewSmallIcons.Checked = False
        mnuViewList.Checked = False
        mnuViewDetails.Checked = False


    End Sub

    Public Sub mnuViewList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewList.Click

        ' Change the list view to a list
        lvwDocList.View = View.List
        CType(tlbMain.Items.Find("_tlbMain_Button11", True)(0), ToolStripButton).Checked = False
        CType(tlbMain.Items.Find("_tlbMain_Button12", True)(0), ToolStripButton).Checked = False
        CType(tlbMain.Items.Find("_tlbMain_Button13", True)(0), ToolStripButton).Checked = True
        CType(tlbMain.Items.Find("_tlbMain_Button14", True)(0), ToolStripButton).Checked = False

        mnuViewLargeIcons.Checked = False
        mnuViewSmallIcons.Checked = False
        mnuViewList.Checked = True
        mnuViewDetails.Checked = False



    End Sub

    Public Sub mnuViewMain_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewMain.Click

        'Change the view to the main one
        SetViewModeMain()

        'Set up the controls positions, according to the two splitter bars
        ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

    End Sub



    Public Sub mnuViewOptions_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewOptions.Click

        Dim oOptions As iDOCOptions.Interface_Renamed
        Dim iStatus As Integer

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'get instance of options object
            'Set oOptions = New iDOCOptions.Interface
            oOptions = New iDOCOptions.Interface_Renamed()

            'initialise it

            m_lReturn = oOptions.Initialise(bUserIsAdministrator:=g_bUserIsAdministrator)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise options object", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuViewOptions_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                oOptions.Dispose()
                oOptions = Nothing
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            'start it

            m_lReturn = oOptions.Start(iStatus:=iStatus)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in Options object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Find", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                oOptions.Dispose()
                oOptions = Nothing
                Exit Sub
            End If

            'terminate object

            oOptions.Dispose()
            oOptions = Nothing

            'Check status, if not cancelled, some options may have changed,
            'so check them all
            If iStatus <> gPMConstants.PMEReturnCode.PMCancel Then
                GetOptions()
            End If

        Catch excep As System.Exception



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuViewOptions_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuViewRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewRefresh.Click

        'Clear all views and reload tree roots

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Clear all tree views
            tvwMain.Nodes.Clear()
            tvwFind.Nodes.Clear()
            tvwFav.Nodes.Clear()
            tvwBCMain.Nodes.Clear()

            'clear all listviews
            lvwDocList.Items.Clear()
            lvwPages.Items.Clear()
            lvwAnnotations.Items.Clear()
            lvwKeyWords.Items.Clear()
            lvwDocsOnly.Items.Clear()
            lvwBCDocsOnly.Items.Clear()

            'Get the root folder list
            m_lReturn = GetFolderList(0, "", m_vFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If

            'repopulate tree
            m_lReturn = PopulateTreeRoots(tvwMain, m_vFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If

            'clear the labels
            lblTitleMain(1).Text = ""
            lblTitleMain(1).Tag = ""
            lblTitleFind(1).Text = ""
            lblTitleFind(1).Tag = ""
            lblTitleFav(1).Text = ""
            lblTitleFav(1).Tag = ""
            lblTitleBC(1).Text = ""
            lblTitleBC(1).Tag = ""

            'MS 01/06/01
            ' reset document archive
            g_lArchiveDocNum = 0

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuViewRefresh_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Public Sub mnuViewSmallIcons_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewSmallIcons.Click

        ' Change the list view to small icons
        'lvwDocList.View = View.ImageKey
        lvwDocList.View = View.SmallIcon
        CType(tlbMain.Items.Find("_tlbMain_Button11", True)(0), ToolStripButton).Checked = False
        CType(tlbMain.Items.Find("_tlbMain_Button12", True)(0), ToolStripButton).Checked = True
        CType(tlbMain.Items.Find("_tlbMain_Button13", True)(0), ToolStripButton).Checked = False
        CType(tlbMain.Items.Find("_tlbMain_Button14", True)(0), ToolStripButton).Checked = False

        mnuViewLargeIcons.Checked = False
        mnuViewSmallIcons.Checked = True
        mnuViewList.Checked = False
        mnuViewDetails.Checked = False



    End Sub


    'Private Sub picTitles_DragOver(ByRef Source As Control, ByRef X As Single, ByRef Y As Single, ByRef State As Integer)

    '	DragOverCheck(picTitles, Source)

    'End Sub



    Private Sub staContents_DragOver(ByRef Source As Control, ByRef X As Single, ByRef Y As Single, ByRef State As Integer)

        'DragOverCheck(staContents, Source)

    End Sub

    Public Sub tlbMain_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _tlbMain_Button3.Click, _tlbMain_Button7.Click, _tlbMain_Button10.Click, _tlbMain_Button15.Click, _tlbMain_Button17.Click, _tlbMain_Button20.Click, _tlbMain_Button23.Click, _tlbMain_Button28.Click, _tlbMain_Button9.Click, _tlbMain_Button8.Click, _tlbMain_Button6.Click, _tlbMain_Button5.Click, _tlbMain_Button4.Click, _tlbMain_Button31.Click, _tlbMain_Button30.Click, _tlbMain_Button29.Click, _tlbMain_Button27.Click, _tlbMain_Button26.Click, _tlbMain_Button25.Click, _tlbMain_Button24.Click, _tlbMain_Button22.Click, _tlbMain_Button21.Click, _tlbMain_Button2.Click, _tlbMain_Button19.Click, _tlbMain_Button18.Click, _tlbMain_Button16.Click, _tlbMain_Button14.Click, _tlbMain_Button13.Click, _tlbMain_Button12.Click, _tlbMain_Button11.Click, _tlbMain_Button1.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        ' History
        ' ~~~~~~~
        ' CFJK19051998  : Added tbrPressed and tbrUnPressed values for
        '                 the list view.
        ' CF17071998    : Changed so that Scan button stays depressed while
        '                 ScanStation is active.
        '

        Try


            Select Case Button.Tag.ToString()
                Case ACViewDoc

                    'view the document(s)
                    ViewDocument()

                Case ACScan

                    m_cntCurrent = Me.ActiveControl

                    'call scan app
                    Select Case m_cntCurrent.Name
                        Case "lvwDocList", "tvwMain"
                            'CType(tlbMain.Items.Item(ACScan), ToolStripButton).Checked = True
                            CType(tlbMain.Items.Find("_tlbMain_Button2", True)(0), ToolStripButton).Checked = True
                            Scan()
                            'CType(tlbMain.Items.Item(ACScan), ToolStripButton).Checked = False
                            CType(tlbMain.Items.Find("_tlbMain_Button2", True)(0), ToolStripButton).Checked = False

                        Case Else
                            'nothing to do

                    End Select

                    m_cntCurrent = Nothing

                Case ACCut

                    m_cntCurrent = Me.ActiveControl

                    'cut the nodes

                    Select Case m_cntCurrent.Name
                        Case "lvwDocList"
                            m_lReturn = StoreSelectedNodes(m_sPasteNodes, m_cntCurrent)

                            GhostSelectedNodes(m_sPasteNodes, m_cntCurrent)

                            'store that this was a Cut
                            m_iPasteFlag = ACPasteCut

                        Case "tvwMain"

                            ReDim m_sPasteNodes(0)
                            m_sPasteNodes(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                            m_sPasteNodes(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text

                            'store that this was a Cut
                            m_iPasteFlag = ACPasteCut

                        Case Else
                            'nothing to do

                    End Select

                    m_cntCurrent = Nothing

                Case ACCopy

                    m_cntCurrent = Me.ActiveControl

                    'copy the nodes

                    Select Case m_cntCurrent.Name
                        Case "lvwDocList"

                            m_lReturn = StoreSelectedNodes(m_sPasteNodes, m_cntCurrent)

                            'store that this was a copy
                            m_iPasteFlag = ACPasteCopy

                        Case "tvwMain"

                            ReDim m_sPasteNodes(0)

                            m_sPasteNodes(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                            m_sPasteNodes(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text

                            'store that this was a copy
                            m_iPasteFlag = ACPasteCopy

                        Case Else
                            'nothing to do

                    End Select

                    m_cntCurrent = Nothing

                Case ACPaste

                    m_cntCurrent = Me.ActiveControl

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                    'paste the nodes
                    Select Case m_cntCurrent.Name
                        Case "lvwDocList", "tvwMain"
                            PasteNodes()

                        Case Else
                            'nothing to do

                    End Select

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

                    m_cntCurrent = Nothing

                Case ACDelete

                    m_cntCurrent = Me.ActiveControl

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                    'delete the nodes
                    Select Case m_cntCurrent.Name
                        Case "lvwDocList"
                            DeleteNodes()

                        Case "tvwMain"

                            'JH051198
                            If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                                MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                                Exit Sub
                            End If

                            DeleteNodes()

                        Case "lvwAnnotations"
                            DeleteAnnotation()

                        Case "lvwKeyWords"
                            DeleteDocKeyword()

                        Case Else
                            'nothing to do

                    End Select

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

                    m_cntCurrent = Nothing

                Case ACDocInfo

                    m_cntCurrent = Me.ActiveControl

                    'display doc info
                    Select Case m_cntCurrent.Name
                        Case "lvwDocList"
                            DisplayDocumentInformation()

                        Case Else
                            'nothing to do

                    End Select

                    m_cntCurrent = Nothing

                Case ACLargeIcon

                    lvwDocList.View = View.LargeIcon
                    CType(tlbMain.Items.Find("_tlbMain_Button11", True)(0), ToolStripButton).Checked = True
                    CType(tlbMain.Items.Find("_tlbMain_Button12", True)(0), ToolStripButton).Checked = False
                    CType(tlbMain.Items.Find("_tlbMain_Button13", True)(0), ToolStripButton).Checked = False
                    CType(tlbMain.Items.Find("_tlbMain_Button14", True)(0), ToolStripButton).Checked = False

                    mnuViewLargeIcons.Checked = True
                    mnuViewSmallIcons.Checked = False
                    mnuViewList.Checked = False
                    mnuViewDetails.Checked = False

                Case ACSmallIcon

                    'lvwDocList.View = View.ImageKey
                    lvwDocList.View = View.SmallIcon
                    CType(tlbMain.Items.Find("_tlbMain_Button11", True)(0), ToolStripButton).Checked = False
                    CType(tlbMain.Items.Find("_tlbMain_Button12", True)(0), ToolStripButton).Checked = True
                    CType(tlbMain.Items.Find("_tlbMain_Button13", True)(0), ToolStripButton).Checked = False
                    CType(tlbMain.Items.Find("_tlbMain_Button14", True)(0), ToolStripButton).Checked = False

                    mnuViewLargeIcons.Checked = False
                    mnuViewSmallIcons.Checked = True
                    mnuViewList.Checked = False
                    mnuViewDetails.Checked = False

                Case ACList

                    lvwDocList.View = View.List
                    CType(tlbMain.Items.Find("_tlbMain_Button11", True)(0), ToolStripButton).Checked = False
                    CType(tlbMain.Items.Find("_tlbMain_Button12", True)(0), ToolStripButton).Checked = False
                    CType(tlbMain.Items.Find("_tlbMain_Button13", True)(0), ToolStripButton).Checked = True
                    CType(tlbMain.Items.Find("_tlbMain_Button14", True)(0), ToolStripButton).Checked = False

                    mnuViewLargeIcons.Checked = False
                    mnuViewSmallIcons.Checked = False
                    mnuViewList.Checked = True
                    mnuViewDetails.Checked = False

                Case ACListDetails

                    lvwDocList.View = View.Details
                    CType(tlbMain.Items.Find("_tlbMain_Button11", True)(0), ToolStripButton).Checked = False
                    CType(tlbMain.Items.Find("_tlbMain_Button12", True)(0), ToolStripButton).Checked = False
                    CType(tlbMain.Items.Find("_tlbMain_Button13", True)(0), ToolStripButton).Checked = False
                    CType(tlbMain.Items.Find("_tlbMain_Button14", True)(0), ToolStripButton).Checked = True

                    mnuViewLargeIcons.Checked = False
                    mnuViewSmallIcons.Checked = False
                    mnuViewList.Checked = False
                    mnuViewDetails.Checked = True

                Case ACGoHome

                    'Go to users home position

                    LocateFolder(g_oBusiness.HomeFolder, False)

                Case ACKeyword

                    'toggle the keywords view
                    mnuViewExtrasKeywords_Click(mnuViewExtrasKeywords, New EventArgs())

                Case ACAnnotation

                    'toggle the Annotations view
                    mnuViewExtrasAnnotations_Click(mnuViewExtrasAnnotations, New EventArgs())

                Case ACViewMain

                    'main view
                    SetViewModeMain()

                Case ACViewFavourites

                    'favourites view
                    SetViewModeFavourites()

                Case ACViewFindResults

                    'find results view
                    SetViewModeFindResults()

                Case ACViewBC

                    'brief case view
                    SetViewModeBC()

                Case ACHotKey

                    HotKeyAdvance()

                Case ACPrint

                    ' Print the document with default settings
                    m_lReturn = PrintDocument(False)

                Case ACEmail

                    m_cntCurrent = Me.ActiveControl

                    'display doc info
                    Select Case m_cntCurrent.Name
                        Case "lvwDocList"
                            mnuFileEmail_Click(mnuFileEmail, New EventArgs())

                        Case Else
                            'nothing to do

                    End Select

                    m_cntCurrent = Nothing

                Case ACArchive

                    m_cntCurrent = Me.ActiveControl

                    'display doc info
                    Select Case m_cntCurrent.Name
                        Case "lvwDocList"
                            mnuFileArchive_Click(mnuFileArchive, New EventArgs())

                        Case Else
                            'nothing to do

                    End Select

                    m_cntCurrent = Nothing


                Case ACExpand

                    mnuViewExpand_Click(mnuViewExpand, New EventArgs())


                Case Else

                    MessageBox.Show("What?", Application.ProductName)


            End Select

            'Set up the controls positions, according to the two splitter bars
            ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tlbMain_ButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayCaptions) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisplayCaptions() As gPMConstants.PMEReturnCode
    '
    '
    'Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Display all language specific captions.
    '
    'Me.Text = CStr(GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
    '
    ' Check for an error.
    'If Me.Text = "" Then
    ' Failed to get data from the resource file.
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
    '           "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
    '
    'Return result
    'End If
    '
    '    cmdOK.Caption = GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACOKButton, _
    ''        iDataType:=PMResString)
    ''
    '    cmdCancel.Caption = GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACCancelButton, _
    ''        iDataType:=PMResString)
    ''
    '    cmdHelp.Caption = GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACHelpButton, _
    ''        iDataType:=PMResString)
    ''
    '    cmdNavigate.Caption = GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACNavigateButton, _
    ''        iDataType:=PMResString)
    ''
    '    tabMainTab.TabCaption(0) = GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACTabTitle1, _
    ''        iDataType:=PMResString)
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' ************************************************************
    ' Enter your code here to display all language specific
    ' captions.
    ' The GetResData function will allow you to do this.
    ''
    ' Example:-
    ''
    '    lblDesc.Caption = GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACDesc, _
    ''        iDataType:=PMResString)
    ''
    ' NOTE: Replace this section with your new code.
    ' ************************************************************
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    Private Function SetUpMenu() As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try




            Return gPMConstants.PMEReturnCode.PMTrue


            '    mnuFile.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACFile, _
            ''        iDataType:=PMResString)

            ' Check for an error.
            If mnuFile.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                           "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUpMenu")

                Return result
            End If

            mnuHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="SetupMenu", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub tlbMain_DragOver(ByRef Source As Control, ByRef X As Single, ByRef Y As Single, ByRef State As Integer)

        'DragOverCheck(tlbMain, Source)

    End Sub


    Private Sub tvwBCMain_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwBCMain.Enter

        EnableMenuItems(tvwBCMain)

    End Sub

    Private Sub tvwBCMain_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwBCMain.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        'Bring Popup menu if right click on a node
        If Button = 2 Then
            m_lReturn = NodeClicked(tvwBCMain)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Node not clicked on, so exit
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Fine, continue
                    EnableMenuItems(tvwBCMain)

                    'save current control
                    m_cntCurrent = tvwBCMain
                    Ctx_mnuPop.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                    m_cntCurrent = tvwBCMain

                Case Else
                    'problem, so go.
                    Exit Sub

            End Select
        End If

    End Sub

    Private Sub tvwBCMain_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwBCMain.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        'Always keep track of where mouse is on a control, so can check if its over a
        'node when clicked
        m_lX = CInt(X)
        m_lY = CInt(Y)

    End Sub


    Private Sub tvwFav_DragOver(ByRef Source As Control, ByRef X As Single, ByRef Y As Single, ByRef State As Integer)

        'DragOverCheck(tvwFav, Source)

    End Sub

    Private Sub tvwFav_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwFav.Enter

        EnableMenuItems(tvwFav)


    End Sub

    Private Sub tvwFav_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwFav.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        'Bring Popup menu if right click on a node
        If Button = 2 Then
            m_lReturn = NodeClicked(tvwFav)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Node not clicked on, so exit
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Fine, continue
                    EnableMenuItems(tvwFav)

                    'save current control
                    m_cntCurrent = tvwFav
                    Ctx_mnuPop.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                    m_cntCurrent = tvwFav

                Case Else
                    'problem, so go.
                    Exit Sub

            End Select
        End If

    End Sub

    Private Sub tvwFav_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwFav.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        'Always keep track of where mouse is on a control, so can check if its over a
        'node when clicked
        m_lX = CInt(X)
        m_lY = CInt(Y)

    End Sub



    Private Sub tvwFind_AfterCollapse(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwFind.AfterCollapse
        Dim Node As TreeNode = eventArgs.Node

        Try

            'Check if contents of listview already matches that of collapsed node, in
            'which case we dont need to repopulate the list view.
            If Convert.ToString(lblTitleFind(1).Tag) = Node.Name Then
                Exit Sub
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            NodeClick(tvwFind, lvwDocList, Node.Name, "")

            'update the label
            lblTitleFind(1).Text = "Contents of '" & Node.Text & "'"
            lblTitleFind(1).Tag = Node.Name

            'Swap icons of newly opened folder and last open folder
            If m_sFindLastOpenFolder <> "" Then
                tvwFind.Nodes.Find(m_sFindLastOpenFolder, True)(0).ImageKey = "IMGCLOSEDFOLDER"
            End If
            Node.ImageKey = "IMGOPENFOLDER"
            m_sFindLastOpenFolder = tvwFind.SelectedNode.Name

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwFind_Collapse", excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub tvwFind_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwFind.DoubleClick

        Dim sNodeKey() As DOCConst.DOCNodes = Nothing


        Try

            m_lReturn = NodeClicked(tvwFind)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Node not clicked on, so exit
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Fine, continue
                Case Else
                    'Anything else, continue
            End Select

            'check the passwords of the node - unless you are adminstrator
            If Not g_bUserIsAdministrator Then

                ReDim sNodeKey(0)
                sNodeKey(0).Key = tvwFind.SelectedNode.Name
                sNodeKey(0).Text = tvwFind.SelectedNode.Text

                m_lReturn = VerifyPasswords(sNodeKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Exit Sub
                End If
            End If

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwFind_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub tvwFind_DragOver(ByRef Source As Control, ByRef X As Single, ByRef Y As Single, ByRef State As Integer)

        'DragOverCheck(tvwFind, Source)

    End Sub

    Private Sub tvwFind_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwFind.Enter

        EnableMenuItems(tvwFind)

    End Sub

    Private Sub tvwFind_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwFind.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        'Bring Popup menu if right click on a node
        'disable this for now
        '    If (Button = 2) Then
        '        m_lReturn = NodeClicked(tvwFind)
        '
        '        Select Case m_lReturn
        '            Case PMFalse
        '                'Node not clicked on, so exit
        '                Exit Sub
        '            Case PMTrue
        '                'Fine, continue
        '                EnableMenuItems tvwFind
        '
        '                'save current control
        '                Set m_cntCurrent = tvwFind
        '                PopupMenu mnuPop
        '                Set m_cntCurrent = tvwFind
        '
        '            Case Else
        '                'problem, so go.
        '                Exit Sub
        '
        '        End Select
        '    End If

    End Sub

    Private Sub tvwFind_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwFind.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        'Always keep track of where mouse is on a control, so can check if its over a
        'node when clicked
        m_lX = CInt(X)
        m_lY = CInt(Y)

    End Sub

    Private Sub tvwMain_AfterLabelEdit(ByVal eventSender As Object, ByVal eventArgs As NodeLabelEditEventArgs) Handles tvwMain.AfterLabelEdit
        'Dim Cancel As Boolean = eventArgs.CancelEdit
        Dim NewString As String = eventArgs.Label


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If Not String.IsNullOrEmpty(NewString) Then
                m_lReturn = RenameNode(NewString)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                'abort rename
                ' Cancel = 1
                eventArgs.CancelEdit = True
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_AfterLabelEdit", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub tvwMain_BeforeLabelEdit(ByVal eventSender As Object, ByVal eventArgs As NodeLabelEditEventArgs) Handles tvwMain.BeforeLabelEdit
        Dim Cancel As Boolean = eventArgs.CancelEdit

        'store the control, node key and name before edit
        'can't rename 'Add Folders to View' - sorted in the rename function

        m_cntRename = tvwMain
        m_sRenameNode.Key = tvwMain.SelectedNode.Name
        m_sRenameNode.Text = tvwMain.SelectedNode.Text

    End Sub

    Private Sub tvwMain_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwMain.Click

        Dim lFoldNum As Integer
        Dim sNodeKey() As DOCConst.DOCNodes = Nothing
        Dim iCount As Integer
        Dim iAnswer As DialogResult
        Dim sText, sDocName, sDocExCode As String

        On Error GoTo Err_tvwMain_Click

        m_lReturn = NodeClicked(tvwMain)

        Select Case m_lReturn
            Case gPMConstants.PMEReturnCode.PMFalse
                'Node not clicked on, so exit
                Exit Sub
            Case gPMConstants.PMEReturnCode.PMTrue
                'Fine, continue
            Case Else
                'Anything else, continue
        End Select

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


        'check the passwords of the node - unless you are adminstrator
        If Not g_bUserIsAdministrator Then

            ReDim sNodeKey(0)
            sNodeKey(0).Key = tvwMain.SelectedNode.Name
            sNodeKey(0).Text = tvwMain.SelectedNode.Text

            m_lReturn = VerifyPasswords(sNodeKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'user could n't be bothered to supply the password - or got it wrong
                Exit Sub
            End If
        End If

        If Not tvwMain.SelectedNode Is Nothing Then
            If tvwMain.SelectedNode.Name.Substring(0, 3) <> "ADD" Then
                NodeClick(tvwMain, lvwDocList, tvwMain.SelectedNode.Name, "")
            End If
        Else
            lvwDocList.Items.Clear()
        End If


        'Swap icons of newly opened folder and last open folder

        If m_sMainLastOpenFolder <> "" Then
            'this may not exist
            On Error Resume Next
            tvwMain.Nodes.Item(m_sMainLastOpenFolder).ImageKey = "IMGCLOSEDFOLDER"
        End If

        If tvwMain.SelectedNode.Name.Substring(0, 3) <> "ADD" Then

            tvwMain.SelectedNode.ImageKey = "IMGOPENFOLDER"
            m_sMainLastOpenFolder = tvwMain.SelectedNode.Name
            'update the label
            lblTitleMain(1).Text = "Contents of '" & tvwMain.SelectedNode.Text & "'"
        Else
            m_sMainLastOpenFolder = ""
            'update the label
            lblTitleMain(1).Text = "Add Folders to View"
        End If

        'update the label
        'lblTitleMain(1).Caption = "Contents of '" & tvwMain.SelectedItem.Text & "'"
        lblTitleMain(1).Tag = tvwMain.SelectedNode.Name


        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)



        ' MS 01/06/01
        ' if a document archive request.
        ' Option to archive, re-select anotehr folder or quit completely

        While g_lArchiveDocNum > 0

            'get the document name

            m_lReturn = g_oBusiness.GetDocInfo(g_lArchiveDocNum, sDocName, lFoldNum, sDocExCode)

            iAnswer = MessageBox.Show("Yes     - archive into highlighted folder" & Environment.NewLine & _
                      "No       - to select a different folder" & Environment.NewLine & _
                      "Cancel - to quit archive altogether" & Environment.NewLine, "Archive '" & sDocName & "' into selected folder?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information)


            Select Case iAnswer
                Case Is = System.Windows.Forms.DialogResult.Yes
                    'go for it..
                    m_lReturn = ArchiveIt()
                    g_lArchiveDocNum = 0

                Case Is = System.Windows.Forms.DialogResult.No
                    'just simply exit in order for user to re-select
                    Exit Sub

                Case Is = System.Windows.Forms.DialogResult.Cancel
                    'reset global flag
                    g_lArchiveDocNum = 0

            End Select

        End While

        Exit Sub

Err_tvwMain_Click:


        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_Click", excep:=New Exception(Information.Err().Description))

        Exit Sub

    End Sub

    Private Sub tvwMain_AfterCollapse(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwMain.AfterCollapse
        Dim Node As TreeNode = eventArgs.Node


        Try
            If tvwMain.SelectedNode Is Nothing Then
                Exit Sub
            End If
            'as long as it's not an Add to view node
            If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                Exit Sub
            End If

            'Check if contents of listview already matches that of collapsed node, in
            'which case we dont need to repopulate the list view.
            If Convert.ToString(lblTitleMain(1).Tag) = Node.Name Then
                Exit Sub
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            NodeClick(tvwMain, lvwDocList, Node.Name, "")

            'update the label
            lblTitleMain(1).Text = "Contents of '" & Node.Text & "'"
            lblTitleMain(1).Tag = Node.Name

            'Swap icons of newly opened folder and last open folder
            If m_sMainLastOpenFolder <> "" Then
                'tvwMain.Nodes.Item(m_sMainLastOpenFolder).ImageKey = "IMGCLOSEDFOLDER"
                tvwMain.Nodes.Find(m_sMainLastOpenFolder, True)(0).ImageKey = "IMGCLOSEDFOLDER"
            End If
            Node.ImageKey = "IMGOPENFOLDER"
            m_sMainLastOpenFolder = tvwMain.SelectedNode.Name


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_AfterCollapse", excep:=excep)
            Exit Sub
        End Try

    End Sub

    Private Sub tvwMain_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwMain.DoubleClick

        '*****************************************************
        '
        ' Edit History:
        ' JH051198 changed this to use the GetMyFolders routine
        ' for Select Folders use
        '
        '
        '*****************************************************

        Dim sTempKey As String = ""

        Try

            m_lReturn = NodeClicked(tvwMain)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Node not clicked on, so exit
                    Exit Sub
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Fine, continue
                Case Else
                    'Anything else, continue
            End Select

            'save currently selected node
            sTempKey = tvwMain.SelectedNode.Name

            m_lReturn = GetMyFolders(tvw:=tvwMain, bAddToView:=False, sTempKey:=sTempKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'to stop it from going back to 'Add Folders' node

            'tvwMain.Nodes.Item(sTempKey).Selected = True
            tvwMain.SelectedNode = tvwMain.Nodes.Find(tvwMain.Nodes.Find(sTempKey, True)(0).Name, True)(0)

        Catch excep As System.Exception



            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_DblClick", excep:=excep)

            Exit Sub

        End Try

    End Sub


    'Private Sub tvwMain_DragDrop(ByRef Source As Control, ByRef X As Single, ByRef Y As Single)

    '	Dim iCntl As Integer
    '	Dim bCopy As Boolean
    '	Dim sDestNode As DOCConst.DOCNodes = DOCConst.DOCNodes.CreateInstance()


    '	Try 

    '		m_bIsTopLevelFolder = False
    '		'as long as it's not an Add to view node
    '		If tvwMain.GetNodeAt(X, Y).Name.Substring(0, 3) = "ADD" Then
    '			MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
    '			Exit Sub
    '		End If

    '		'Check if copying or not by seeing if Control Key pressed
    '		iCntl = GetKeyState(VK_CONTROL)
    '		bCopy = (iCntl And &H8000s) <> 0


    '		'See if over a node
    '		
    '		If tvwMain.DropHighlight Is Nothing Then

    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

    '			
    '			tvwMain.DropHighlight = Nothing
    '			m_bDragging = False

    '			'Update the actual database with the moves
    '			If bCopy Then
    '				m_lReturn = CopyNodesToRoot(m_sDragNodes)
    '			Else
    '				m_lReturn = MoveNodesToRoot(m_sDragNodes)
    '			End If

    '			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
    '				Exit Sub
    '			End If

    '			'Update the controls
    '			m_lReturn = UpdateRootViews(tvwMain, lvwDocList, bCopy, m_sDragNodes)

    '			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
    '				LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed to update views. Please refresh your view of the data.", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_DragDrop", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
    '				Exit Sub
    '			End If

    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

    '			Exit Sub

    '		Else

    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

    '			
    '			tvwMain.DropHighlight = Nothing
    '			m_bDragging = False

    '			'Store the destination node key and name
    '			sDestNode.Key = tvwMain.GetNodeAt(X, Y).Name
    '			sDestNode.Text = tvwMain.GetNodeAt(X, Y).Text

    '			'check we haven't just done a crap double click, and dragged a
    '			'node to itself
    '			If TypeOf Source Is TreeView Then
    '				If sDestNode.Key = m_sDragNodes(0).Key Then
    '					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
    '					Exit Sub
    '				End If
    '			End If

    '			If m_bIsMoveDocList Then
    '				m_bIsTopLevelFolder = tvwMain.Nodes.Item(m_sDragNodes(0).Key).Parent Is Nothing
    '			End If
    '			'Update the actual database with the moves
    '			If bCopy Then
    '				m_lReturn = CopyNodes(sDestNode, m_sDragNodes)
    '			Else
    '				m_lReturn = MoveNodes(sDestNode, m_sDragNodes)
    '			End If

    '			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
    '				Exit Sub
    '			End If

    '			'Update the controls
    '			m_lReturn = UpdateViews(tvwMain, lvwDocList, bCopy, sDestNode, m_sDragNodes)

    '			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
    '				LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed to update views. Please refresh your view of the data.", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_DragDrop", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
    '			End If

    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

    '		End If

    '	Catch excep As System.Exception



    '		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

    '		'to get rid of error messages when user drags by accident

    '		If Information.Err().Number <> 0 Then

    '			LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="HitTest failed to determine To Node", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_DragDrop", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

    '		End If

    '		Exit Sub

    '	End Try

    'End Sub

    '
    'Private Sub tvwMain_DragOver(ByRef Source As Control, ByRef X As Single, ByRef Y As Single, ByRef State As Integer)


    '	Try 

    '		'as long as it's not an Add to view node
    '		If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
    '			Exit Sub
    '		End If

    '		If m_bDragging Then
    '			' Set DropHighlight to the mouse's coordinates.
    '			
    '			tvwMain.DropHighlight = tvwMain.GetNodeAt(X, Y)
    '			DragOverCheck(tvwMain, Source)
    '		End If

    '	Catch excep As System.Exception



    '		LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_DragOver", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

    '		Exit Sub

    '	End Try

    'End Sub


    Private Sub tvwMain_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwMain.Enter

        EnableMenuItems(tvwMain)

    End Sub

    Private Sub tvwMain_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles tvwMain.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iKey As Integer


        On Error GoTo Err_tvwMain_KeyDown


        'Helpfull information
        Dim lNodeNum, lParentNum As Integer
        Dim sNodeName, sMsg, sKey, sExCode As String
        Dim iFolderLevel, iAccessLevel As Integer
        Dim sPassword As String = ""
        Dim dCreateDate As Date
        If (eventArgs.KeyCode = Keys.F12) And (Shift = ShiftConstants.CtrlMask) Then


            'as long as it's not an Add to view node
            If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                Exit Sub
            End If

            m_lReturn = ExtractNumFromKey(tvwMain.SelectedNode.Name, lNodeNum)
            On Error Resume Next
            If Not (tvwMain.SelectedNode.Parent Is Nothing) Then
                sKey = tvwMain.SelectedNode.Parent.Name
                m_lReturn = ExtractNumFromKey(sKey, lParentNum)
            End If

            sMsg = "Folder Name : " & Strings.Chr(9).ToString() & "'" & tvwMain.SelectedNode.Text & "'" & Strings.Chr(10).ToString()
            sMsg = sMsg & "Folder Num : " & Strings.Chr(9).ToString() & CStr(lNodeNum) & Strings.Chr(10).ToString()
            sMsg = sMsg & "Node Key : " & Strings.Chr(9).ToString() & tvwMain.SelectedNode.Name & Strings.Chr(10).ToString() & Strings.Chr(10).ToString()

            ' ex code
            ' folder_level
            ' access_level
            ' password
            ' create_date


            m_lReturn = g_oBusiness.GetFolderInformation(lNodeNum:=lNodeNum, sExCode:=sExCode, iFolderLevel:=iFolderLevel, iAccessLevel:=iAccessLevel, sPassword:=sPassword, dCreateDate:=dCreateDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_KeyDown", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            sMsg = sMsg & "Ex Code : " & Strings.Chr(9).ToString() & "'" & sExCode & "'" & Strings.Chr(10).ToString()
            sMsg = sMsg & "Folder Level : " & Strings.Chr(9).ToString() & CStr(iFolderLevel) & Strings.Chr(10).ToString()
            sMsg = sMsg & "Access Level : " & Strings.Chr(9).ToString() & CStr(iAccessLevel) & Strings.Chr(10).ToString()
            sMsg = sMsg & "Password : " & Strings.Chr(9).ToString() & "'" & sPassword & "'" & Strings.Chr(10).ToString()
            sMsg = sMsg & "Create Date : " & Strings.Chr(9).ToString() & DateTimeHelper.ToString(dCreateDate) & Strings.Chr(10).ToString() & Strings.Chr(10).ToString()

            If Not (tvwMain.SelectedNode.Parent Is Nothing) Then
                sMsg = sMsg & "Parent Name : " & Strings.Chr(9).ToString() & "'" & tvwMain.SelectedNode.Parent.Text & "'" & Strings.Chr(10).ToString()
                sMsg = sMsg & "Parent Num  : " & Strings.Chr(9).ToString() & CStr(lParentNum) & Strings.Chr(10).ToString()
                sMsg = sMsg & "Parent Key  : " & Strings.Chr(9).ToString() & tvwMain.SelectedNode.Parent.Name & Strings.Chr(10).ToString() & Strings.Chr(10).ToString()
            End If

            MessageBox.Show(sMsg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If

        ' Delete pressed ?
        If eventArgs.KeyCode = Keys.Delete Then

            'save current control
            m_cntCurrent = tvwMain

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            DeleteNodes()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            m_cntCurrent = Nothing

        End If

        'What key was pressed
        Select Case KeyCode
            Case Keys.D0
                iKey = 0
            Case Keys.D1
                iKey = 1
            Case Keys.D2
                iKey = 2
            Case Keys.D3
                iKey = 3
            Case Keys.D4
                iKey = 4
            Case Keys.D5
                iKey = 5
            Case Keys.D6
                iKey = 6
            Case Keys.D7
                iKey = 7
            Case Keys.D8
                iKey = 8
            Case Keys.D9
                iKey = 9
            Case Else
                'No otherkeys are hot keys, so go.
                Exit Sub
        End Select

        'if control pressed, save the hot key
        If Shift = ShiftConstants.CtrlMask Then

            m_sHotKey(iKey) = tvwMain.SelectedNode.Name

            'save position in array
            m_iHotKeyPos = iKey

            Exit Sub

        End If

        'if alt pressed, go to hot key node
        If Shift = ShiftConstants.AltMask Then

            'the hot key node may no longer be in the current tree,
            'so ignore errors
            On Error Resume Next

            If m_sHotKey(iKey) <> "" Then
                tvwMain.Nodes.Item(m_sHotKey(iKey)).EnsureVisible()

                'tvwMain.Node.Item(m_sHotKey(iKey)).Selected = True
                tvwMain.SelectedNode = tvwMain.Nodes.Find(tvwMain.Nodes.Item(m_sHotKey(iKey)).Name, True)(0)
                'save position in array
                m_iHotKeyPos = iKey
                Exit Sub
            End If

        End If

        Exit Sub

Err_tvwMain_KeyDown:

        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_KeyDown", excep:=New Exception(Information.Err().Description))

        Exit Sub

    End Sub

    Private Sub tvwMain_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles tvwMain.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Try

            If KeyAscii <> CInt(Keys.Return) Then
                'ddi n't hit enter
                If KeyAscii = 0 Then
                    eventArgs.Handled = True
                End If
                Exit Sub
            End If

            m_lReturn = GetMyFolders(tvw:=tvwMain, bAddToView:=False, sTempKey:=tvwMain.SelectedNode.Name)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If KeyAscii = 0 Then
                    eventArgs.Handled = True
                End If
                Exit Sub
            End If

            'JH051198 moved all this lot to GetMyFolders function

            '    'check the passwords of the node - unless you are adminstrator
            '    If (g_bUserIsAdministrator = False) Then
            '
            '        ReDim sNodeKey(0)
            '        sNodeKey(0).Key = tvwMain.SelectedItem.Key
            '        sNodeKey(0).Text = tvwMain.SelectedItem.Text
            '
            '        m_lReturn& = VerifyPasswords(sNodeKey())
            '
            '        If (m_lReturn <> PMTrue) Then
            '            'user could n't be bothered to supply the password - or got it wrong
            '            Exit Sub
            '        End If
            '
            '    End If
            '
            '    'single click
            '    iPMFunc.SetMousePointer (PMMouseBusy)
            '
            '    NodeClick tvwMain, lvwDocList, tvwMain.SelectedItem.Key, ""
            '
            '    'Swap icons of newly opened folder and last open folder
            '    If (m_sMainLastOpenFolder <> "") Then
            '        tvwMain.Nodes(m_sMainLastOpenFolder).Image = "IMGCLOSEDFOLDER"
            '    End If
            '
            '    tvwMain.SelectedItem.Image = "IMGOPENFOLDER"
            '    m_sMainLastOpenFolder = tvwMain.SelectedItem.Key
            '
            '    'update the label
            '    lblTitleMain(1).Caption = "Contents of '" & tvwMain.SelectedItem.Text & "'"
            '    lblTitleMain(1).Tag = tvwMain.SelectedItem.Key
            '
            '
            '    'If children exist, this node has been previouly expanded so can leave.
            '    If (tvwMain.SelectedItem.Children > 0) Then
            '        iPMFunc.SetMousePointer (PMMouseReset)
            '        Exit Sub
            '    End If
            '
            '    'if we are not displaying folders in the document list view, then preceding
            '    'click event will not have gotten the folder list, so we'd best get it now
            '    If (m_bDocsOnly = True) Then
            '
            '        'Get the folder num from the selected node key
            '        m_lReturn = ExtractNumFromKey(tvwMain.SelectedItem.Key, lFoldNum&)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            iPMFunc.SetMousePointer (PMMouseReset)
            '            Exit Sub
            '        End If
            '
            '        'Get the folders in the selected folder
            '        m_lReturn = GetFolderList(lFoldNum, "", m_vFolderArray)
            '
            '    End If
            '
            '    m_lReturn = PopulateTreeChildren(tvwMain, tvwMain.SelectedItem.Index, m_vFolderArray)
            '
            '    'expand
            '    tvwMain.SelectedItem.Expanded = True
            '
            '    iPMFunc.SetMousePointer (PMMouseReset)

            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub

        Catch
        End Try



        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_KeyPress", excep:=New Exception(Information.Err().Description))

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        Exit Sub

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub tvwMain_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwMain.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        Dim sKey As String = ""



        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_tvwMain_MouseDown)")


        'Bring Popup menu if right click on a node
        If eventArgs.Button = MouseButtons.Right Then
            ' CTAF 20040604 - Bug fix by HSG. Merged from DME 1.6x
            m_lX = CInt(X)
            m_lY = CInt(Y)
            'm_lReturn = NodeClicked(tvwMain)
            m_cntCurrent = tvwMain
            Ctx_mnuPop2.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
            m_cntCurrent = Nothing

        Else
            Exit Sub

        End If




        Exit Sub

Err_tvwMain_MouseDown:

        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_MouseDown", excep:=New Exception(Information.Err().Description))
        Exit Sub

    End Sub

    Public ReadOnly Property ViewMode() As Integer
        Get

            Return m_iViewMode

        End Get
    End Property
    ' ***************************************************************** '
    ' Name: SetViewModeMain
    '
    ' Description: Sets the visibility status of appropriate controls
    ' when viewing in normal, ie 'Main' mode.
    '
    ' *****************************************************************
    Private Sub SetViewModeMain()


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'property value
            m_iViewMode = DOCViewModeMain

            'check appropriate menu item
            mnuViewMain.Checked = True
            mnuViewFavourites.Checked = False
            mnuViewBC.Checked = False
            mnuViewFindResults.Checked = False

            'press appropriate button
            'CType(tlbMain.Items.Item(ACViewMain), ToolStripButton).Checked = True
            If Not CType(tlbMain.Items.Find("_tlbMain_Button24", True)(0), ToolStripButton) Is Nothing Then
                CType(tlbMain.Items.Find("_tlbMain_Button24", True)(0), ToolStripButton).Checked = True
            End If


            tvwMain.Visible = True
            tvwFav.Visible = False
            tvwFind.Visible = False
            tvwBCMain.Visible = False
            lvwDocsOnly.Visible = False
            lvwBCDocsOnly.Visible = False
            lvwDocList.Visible = True
            If m_iViewKeywords = 0 Then
                lvwKeyWords.Visible = False
            Else
                lvwKeyWords.Visible = True
            End If
            If m_iViewAnnotations = 0 Then
                lvwAnnotations.Visible = False
            Else
                lvwAnnotations.Visible = True
            End If
            If m_iViewPages = 0 Then
                lvwPages.Visible = False
            Else
                lvwPages.Visible = True
            End If
            lblTitleMain(0).Visible = True
            lblTitleMain(1).Visible = True
            lblTitleFav(0).Visible = False
            lblTitleFav(1).Visible = False
            lblTitleFind(0).Visible = False
            lblTitleFind(1).Visible = False
            lblTitleBC(0).Visible = False
            lblTitleBC(1).Visible = False

            'Give the main tree focus
            tvwMain.Focus()

            'Populate doc list for currently open folder in main tree view
            ' (if there is one)
            If Not String.IsNullOrEmpty(lblTitleMain(1).Tag) Then
                NodeClick(tvwMain, lvwDocList, Convert.ToString(lblTitleMain(1).Tag), "")
            Else
                lvwDocList.Items.Clear()
                lvwAnnotations.Items.Clear()
                lvwKeyWords.Items.Clear()
                lvwPages.Items.Clear()
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="SetViewModeMain", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: SetViewModeBC
    '
    ' Description: Sets the visibility status of appropriate controls
    ' when in Briefcase mode.
    '
    ' *****************************************************************
    Private Sub SetViewModeBC()


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'set property value
            m_iViewMode = DOCViewModeBC

            'check appropriate menu item
            mnuViewMain.Checked = False
            mnuViewFavourites.Checked = False
            mnuViewBC.Checked = True
            mnuViewFindResults.Checked = False

            'press appropriate button
            'CType(tlbMain.Items.Item(ACViewBC), ToolStripButton).Checked = True
            CType(tlbMain.Items.Find("_tlbMain_Button26", True)(0), ToolStripButton).Checked = True

            tvwMain.Visible = True
            tvwFav.Visible = False
            tvwFind.Visible = False
            tvwBCMain.Visible = True
            lvwDocsOnly.Visible = True
            lvwBCDocsOnly.Visible = True
            lvwDocList.Visible = False
            lvwKeyWords.Visible = False
            lvwAnnotations.Visible = False
            lvwPages.Visible = False
            lblTitleMain(0).Visible = False
            lblTitleMain(1).Visible = False
            lblTitleFav(0).Visible = False
            lblTitleFav(1).Visible = False
            lblTitleFind(0).Visible = False
            lblTitleFind(1).Visible = False
            lblTitleBC(0).Visible = True
            lblTitleBC(1).Visible = True

            'Give the main tree focus
            tvwMain.Focus()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            'Log error
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="SetViewModeBC", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: SetViewModeFavourites
    '
    ' Description: Sets the visibility status of appropriate controls
    ' when viewing selected favourites.
    '
    ' *****************************************************************
    Private Sub SetViewModeFavourites()


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'set property value
            m_iViewMode = DOCViewModeFavourites

            'check appropriate menu item
            mnuViewMain.Checked = False
            mnuViewFavourites.Checked = True
            mnuViewBC.Checked = False
            mnuViewFindResults.Checked = False

            'press appropriate button
            'CType(tlbMain.Items.Item(ACViewFavourites), ToolStripButton).Checked = True
            CType(tlbMain.Items.Find("_tlbMain_Button25", True)(0), ToolStripButton).Checked = True

            tvwMain.Visible = False
            tvwFav.Visible = True
            tvwFind.Visible = False
            tvwBCMain.Visible = False
            lvwDocsOnly.Visible = False
            lvwBCDocsOnly.Visible = False
            lvwDocList.Visible = True
            lvwKeyWords.Visible = m_iViewKeywords
            lvwAnnotations.Visible = m_iViewAnnotations
            lvwPages.Visible = m_iViewPages
            lblTitleMain(0).Visible = False
            lblTitleMain(1).Visible = False
            lblTitleFav(0).Visible = True
            lblTitleFav(1).Visible = True
            lblTitleFind(0).Visible = False
            lblTitleFind(1).Visible = False
            lblTitleBC(0).Visible = False
            lblTitleBC(1).Visible = False

            'Give the fav tree focus
            tvwFav.Focus()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="SetViewModeFavourites", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: SetViewFindResults
    '
    ' Description: Sets the visibility status of appropriate controls
    ' when viewing results of a find.
    '
    ' *****************************************************************
    Public Sub SetViewModeFindResults()


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'set property value
            m_iViewMode = DOCViewModeFindResults

            'check appropriate menu item
            mnuViewMain.Checked = False
            mnuViewFavourites.Checked = False
            mnuViewBC.Checked = False
            mnuViewFindResults.Checked = True
            _lblTitleFind_0.BringToFront()
            'press appropriate button
            'CType(tlbMain.Items.Item(ACViewFindResults), ToolStripButton).Checked = True
            CType(tlbMain.Items.Find("_tlbMain_Button27", True)(0), ToolStripButton).Checked = True

            tvwMain.Visible = False
            tvwFav.Visible = False
            tvwFind.Visible = True
            tvwBCMain.Visible = False
            lvwDocsOnly.Visible = False
            lvwBCDocsOnly.Visible = False
            lvwDocList.Visible = True
            lvwKeyWords.Visible = m_iViewKeywords
            lvwAnnotations.Visible = m_iViewAnnotations
            lvwPages.Visible = m_iViewPages
            lblTitleMain(0).Visible = False
            lblTitleMain(1).Visible = False
            lblTitleFav(0).Visible = False
            lblTitleFav(1).Visible = False
            lblTitleFind(0).Visible = True
            lblTitleFind(1).Visible = True
            lblTitleBC(0).Visible = False
            lblTitleBC(1).Visible = False

            'Give the find tree focus
            tvwFind.Focus()

            'Populate doc list for currently open folder in find results tree view
            ' (if there is one)
            If Convert.ToString(lblTitleFind(1).Tag) <> "" Then
                NodeClick(tvwFind, lvwDocList, Convert.ToString(lblTitleFind(1).Tag), "")
            Else
                lvwDocList.Items.Clear()
                lvwAnnotations.Items.Clear()
                lvwKeyWords.Items.Clear()
                lvwPages.Items.Clear()
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="SetViewModeFindResults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ConstructView
    '
    ' Description: This section is called when DocuMaster was called
    ' with a current document to view. It constructs the treeview to
    ' to this document and shows the document
    '
    ' Edit History:

    ' JH271098 ConstructView - if the parameter for document has
    ' not been passed then just skip the bit where the doc
    ' is accessed and display folder only, likewise for the folders
    ' at other levels
    '
    ' JH051198 change this so it will only retrieve the folder that
    ' is required rather than the whole lot.  Ref: Select Folders
    '
    ' *****************************************************************
    Public Sub ConstructView(ByRef sCabExCode As String, ByRef sDrawExCode As String, ByRef sFoldExCode As String, ByRef sDocExCode As String)

        Dim sCabKey, sDrawKey, sFoldKey, sTempKey As String  'JH271098 to hold the current key
        Dim sSiblingKey As String = ""  'JH051198 to hold the 'add to view' key sibling

        Dim bPopFolds, bPopDraws, bPopCabs As Boolean

        Dim lNum, lDocNum, lParentNum, lChildren As Integer

        Dim lNodeCount As Integer = 0
        Dim sFolderName, sPassword As String

        Dim dDate As Date

        Dim bDocFound, bNoAccess As Boolean

        Dim sNodeKey() As DOCConst.DOCNodes = Nothing


        On Error GoTo Err_ConstructView

        SetViewModeMain()

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        'construct keys for these nodes

        'cabinet first

        m_lReturn = g_oBusiness.GetNodeKey(sExCode:=sCabExCode, iFolderLevel:=DOCCabinet, lParentNum:=0, sFolderNum:=lNum, sPassword:=sPassword, dCreateDate:=dDate, bNoAccess:=bNoAccess)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Unable To Locate Cabinet", vApp:=ACApp, vClass:=ACClass, vMethod:="ConstructView", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Exit Sub

        End If

        'notify user of lack of access
        If bNoAccess Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            MessageBox.Show("You cannot access this cabinet.", DOCAppName)
            Exit Sub
        End If

        'construct cabinet key
        If sPassword.Trim() <> "" Then

            'ie passworded
            sCabKey = ACFolder & ACPassword & CStr(CInt(dDate.ToOADate)) & CStr(lNum)

        Else
            sCabKey = ACFolder & " " & CStr(CInt(dDate.ToOADate)) & CStr(lNum)
        End If

        sTempKey = sCabKey

        'now drawer

        If sDrawExCode.Trim() <> "" Then

            lParentNum = lNum

            m_lReturn = g_oBusiness.GetNodeKey(sExCode:=sDrawExCode, iFolderLevel:=DOCDrawer, lParentNum:=lParentNum, sFolderNum:=lNum, sPassword:=sPassword, dCreateDate:=dDate, bNoAccess:=bNoAccess)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Unable To Locate Drawer", vApp:=ACApp, vClass:=ACClass, vMethod:="ConstructView", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'notify user of lack of access
            If bNoAccess Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                MessageBox.Show("You cannot access this drawer.", DOCAppName)
                Exit Sub
            End If

            'construct drawer key
            If sPassword.Trim() <> "" Then
                'ie passworded
                sDrawKey = ACFolder & ACPassword & CStr(CInt(dDate.ToOADate)) & CStr(lNum)
            Else
                sDrawKey = ACFolder & " " & CStr(CInt(dDate.ToOADate)) & CStr(lNum)
            End If

            sTempKey = sDrawKey
        End If

        'now folder

        If sFoldExCode.Trim() <> "" Then

            lParentNum = lNum

            m_lReturn = g_oBusiness.GetNodeKey(sExCode:=sFoldExCode, iFolderLevel:=DOCFolder, lParentNum:=lParentNum, sFolderNum:=lNum, sPassword:=sPassword, dCreateDate:=dDate, bNoAccess:=bNoAccess)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Unable To Locate Folder", vApp:=ACApp, vClass:=ACClass, vMethod:="ConstructView", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'notify user of lack of access
            If bNoAccess Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                MessageBox.Show("You cannot access this folder.", DOCAppName)
                Exit Sub
            End If

            'construct folder key
            If sPassword.Trim() <> "" Then
                'ie passworded
                sFoldKey = ACFolder & ACPassword & CStr(CInt(dDate.ToOADate)) & CStr(lNum)
            Else
                sFoldKey = ACFolder & " " & CStr(CInt(dDate.ToOADate)) & CStr(lNum)
            End If

            sTempKey = sFoldKey
        End If

        ' first expand cabinet - this will always be present
        'tvwMain.Nodes.Item(sCabKey).Expand()
        If tvwMain.Nodes.Find(sCabKey, True).Length > 0 Then
            tvwMain.Nodes.Find(sCabKey, True)(0).Expand()
            'tvwMain.SelectedNode = tvwMain.Nodes.Find(sCabKey, True)(0)
            'tvwMain_DoubleClick(tvwMain, New EventArgs)
        End If


        ' now expand draw - may not exist in which case it will go
        ' to error section,

        'JH271098 only if the parameter has been passed

        If sDrawExCode.Trim() <> "" Then

            On Error GoTo Err_Drawer
            'tvwMain.Nodes.Item(sDrawKey).Expand()
            If tvwMain.Nodes.Find(sDrawKey, True).Length > 0 Then
                tvwMain.Nodes.Find(sDrawKey, True)(0).Expand()
            Else
                bPopDraws = True
            End If

            If bPopDraws Then

                'draw not in view, so get draw list for cab and populate
                'JH051198 only get the draw we need and add this


                'check for cabinet password
                If Not g_bUserIsAdministrator Then

                    ReDim sNodeKey(0)
                    If tvwMain.Nodes.Find(sDrawKey, True).Length > 0 Then
                        sNodeKey(0).Key = tvwMain.Nodes.Find(sDrawKey, True)(0).Name
                        sNodeKey(0).Text = tvwMain.Nodes.Find(sDrawKey, True)(0).Text

                    End If
                    If sNodeKey.Length > 0 Then
                        If Not sNodeKey(0).Key Is Nothing Then
                            m_lReturn = VerifyPasswords(sNodeKey)
                        End If

                    End If


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'user couldn't be bothered to supply the password - or got it wrong
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Exit Sub
                    End If
                End If

                'get just one draw

                'count cabinet children before getting the folder number

                m_lReturn = ExtractNumFromKey(sCabKey, lNum)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                m_lReturn = CountChildren(lNum, lChildren)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                m_lReturn = ExtractNumFromKey(sDrawKey, lNum)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If


                m_lReturn = g_oBusiness.GetFolderValues(lNum, m_vFolderArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                '            'get all draws for this cabinet
                '            m_lReturn& = GetFolderList(lNum, "", m_vFolderArray)
                '
                'JH051198
                'if there is an 'Add to Veiw' node in the folder
                'then we need to delete it or it will get sorted with the rest
                'it will always be first

                If Me.tvwMain.Nodes.Item(sCabKey).GetNodeCount(False) > 0 Then


                    'If frmInterface.tvwMain.Nodes(sCabKey).Child.FirstSibling.Name.Substring(0, 3) = "ADD" Then
                    If tvwMain.Nodes.Find(sCabKey, True).Length > 0 Then
                        If tvwMain.Nodes.Find(sCabKey, True)(0).FirstNode.Name.Substring(0, 3) = "ADD" Then
                            Dim key As Integer = tvwMain.Nodes.Find(sCabKey, True)(0).FirstNode.Index
                            Me.tvwMain.Nodes.RemoveAt(key - 1)
                        End If
                    End If


                End If

                'fill tree
                If Not tvwMain.Nodes.Item(sCabKey).Parent Is Nothing Then
                    m_lReturn = PopulateTreeChildren(tvwMain, tvwMain.Nodes.Find(sCabKey, True)(0).Index, m_vFolderArray, tvwMain.Nodes.Find(sCabKey, True)(0).Parent.Name)
                Else
                    m_lReturn = PopulateTreeChildren(tvwMain, tvwMain.Nodes.Find(sCabKey, True)(0).Index, m_vFolderArray)

                End If


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                'JH051198
                'if more children available then add an 'add to view' node
                If Me.tvwMain.Nodes.Item(sCabKey).GetNodeCount(False) < lChildren Then


                    'sSiblingKey = frmInterface.tvwMain.Nodes(sCabKey).Child.FirstSibling.Name
                    sSiblingKey = tvwMain.Nodes(sCabKey).FirstNode.Name

                    m_lReturn = AddToViewNode(tvw:=Me.tvwMain, sKey:=sSiblingKey)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Exit Sub
                    End If

                End If

            End If

        End If

        'expand fold
        'JH271098 only if the parameter has been passed

        If sFoldExCode.Trim() <> "" Then

            On Error GoTo Err_Folder
            If tvwMain.Nodes.Find(sFoldKey, True).Length > 0 Then
                tvwMain.Nodes.Find(sFoldKey, True)(0).Expand()
            Else
                bPopFolds = True
            End If
            'tvwMain.Nodes.Item(sFoldKey).Expand()

            If bPopFolds Then

                'folder not in view, so get draw list for cab and pop

                'check for drawer password
                If Not g_bUserIsAdministrator Then

                    ReDim sNodeKey(0)
                    If tvwMain.Nodes.Find(sDrawKey, True).Length > 0 Then
                        sNodeKey(0).Key = tvwMain.Nodes.Find(sDrawKey, True)(0).Name
                        sNodeKey(0).Text = tvwMain.Nodes.Find(sDrawKey, True)(0).Text

                    End If


                    m_lReturn = VerifyPasswords(sNodeKey)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'user couldn't be bothered to supply the password - or got it wrong
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Exit Sub
                    End If
                End If

                'get just one folder

                'count siblings before getting the folder number
                m_lReturn = CountChildren(lNum, lChildren)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                m_lReturn = ExtractNumFromKey(sFoldKey, lNum)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If


                m_lReturn = g_oBusiness.GetFolderValues(lNum, m_vFolderArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                '            m_lReturn& = ExtractNumFromKey(sDrawKey, lNum)
                '            m_lReturn& = GetFolderList(lNum, "", m_vFolderArray)

                'JH051198
                'if there is an 'Add to Veiw' node in the folder
                'then we need to delete it or it will get sorted with the rest
                'it will always be first

                'If Me.tvwMain.Nodes.Item(sDrawKey).GetNodeCount(False) > 0 Then
                If Not Information.IsNothing(Me.tvwMain.Nodes.Item(sDrawKey)) Then
                    If Me.IsHandleCreated Then
                        Me.Invoke(New GetTreeNodeCount(AddressOf GetNodeCount), False, lNodeCount, Me.tvwMain.Nodes.Item(sDrawKey))
                        If lNodeCount > 0 Then
                            If tvwMain.Nodes(sDrawKey).FirstNode.Name.Substring(0, 3) = "ADD" Then
                                Me.tvwMain.Nodes.RemoveAt(CInt(tvwMain.Nodes(sDrawKey).FirstNode.Name) - 1)
                            End If
                        End If
                    ElseIf Me.tvwMain.Nodes.Item(sDrawKey).GetNodeCount(False) > 0 Then
                        If tvwMain.Nodes(sDrawKey).FirstNode.Name.Substring(0, 3) = "ADD" Then
                            Me.tvwMain.Nodes.RemoveAt(CInt(tvwMain.Nodes(sDrawKey).FirstNode.Name) - 1)
                        End If
                    End If


                End If

                'fill tree
                'm_lReturn = PopulateTreeChildren(tvwMain, tvwMain.Nodes.Item(sDrawKey).Index, m_vFolderArray)
                If Not tvwMain.Nodes.Find(sDrawKey, True)(0).Parent Is Nothing Then
                    m_lReturn = PopulateTreeChildren(tvwMain, tvwMain.Nodes.Find(sDrawKey, True)(0).Index, m_vFolderArray, tvwMain.Nodes.Find(sDrawKey, True)(0).Parent.Name)
                Else
                    m_lReturn = PopulateTreeChildren(tvwMain, tvwMain.Nodes.Find(sDrawKey, True)(0).Index, m_vFolderArray)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                'JH051198
                'if more children available then add an 'add to view' node

                'If Me.tvwMain.Nodes.Item(sDrawKey).GetNodeCount(False) < lChildren Then
                If Not Information.IsNothing(Me.tvwMain.Nodes.Item(sDrawKey)) Then
                    lNodeCount = 0
                    If Me.IsHandleCreated Then
                        Me.Invoke(New GetTreeNodeCount(AddressOf GetNodeCount), False, lNodeCount, Me.tvwMain.Nodes.Item(sDrawKey))
                        If lNodeCount < lChildren Then
                            sSiblingKey = tvwMain.Nodes.Find(sDrawKey, True)(0).FirstNode.Name
                            m_lReturn = AddToViewNode(tvw:=Me.tvwMain, sKey:=sSiblingKey)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                                Exit Sub
                            End If
                        End If
                    ElseIf Me.tvwMain.Nodes.Item(sDrawKey).GetNodeCount(False) < lChildren Then
                        sSiblingKey = tvwMain.Nodes.Find(sDrawKey, True)(0).FirstNode.Name
                        m_lReturn = AddToViewNode(tvw:=Me.tvwMain, sKey:=sSiblingKey)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                            Exit Sub
                        End If
                    End If
                End If


            End If

            'check for folder password
            If Not g_bUserIsAdministrator Then

                ReDim sNodeKey(0)
                'sNodeKey(0).Key = tvwMain.Nodes.Item(sFoldKey).Name
                'sNodeKey(0).Text = tvwMain.Nodes.Item(sFoldKey).Text
                If tvwMain.Nodes.Find(sFoldKey, True).Length > 0 Then
                    sNodeKey(0).Key = tvwMain.Nodes.Find(sFoldKey, True)(0).Name
                    sNodeKey(0).Text = tvwMain.Nodes.Find(sFoldKey, True)(0).Text

                End If

                If sNodeKey.Length > 0 Then
                    m_lReturn = VerifyPasswords(sNodeKey)
                End If


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user couldn't be bothered to supply the password - or got it wrong
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If
            End If
        End If


        'ensure is visible and selected
        If tvwMain.Nodes.Find(sFoldKey, True).Length > 0 Then
            tvwMain.Nodes.Find(sFoldKey, True)(0).EnsureVisible()
            tvwMain.SelectedNode = tvwMain.Nodes.Find(sFoldKey, True)(0)

        End If

        'populate doc
        NodeClick(tvwMain, lvwDocList, sTempKey, "")


        If m_sMainLastOpenFolder <> "" Then
            'this may not exist
            On Error Resume Next
            tvwMain.Nodes.Item(m_sMainLastOpenFolder).ImageKey = "IMGCLOSEDFOLDER"
        End If
        If tvwMain.SelectedNode Is Nothing Then
            tvwMain.SelectedNode = tvwMain.Nodes.Find(sTempKey, True)(0)
        End If

        tvwMain.SelectedNode.ImageKey = "IMGOPENFOLDER"
        m_sMainLastOpenFolder = tvwMain.SelectedNode.Name

        'update the label
        lblTitleMain(1).Text = "Contents of '" & tvwMain.SelectedNode.Text & "'"
        lblTitleMain(1).Tag = tvwMain.SelectedNode.Name

        'actually view document now
        'JH271098 exit sub if the parameter has not been passed

        If sDocExCode.Trim() = "" Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            Exit Sub
        End If

        'loop thru doc list till we find our doc, select it, then view it
        For i As Integer = 1 To lvwDocList.Items.Count

            m_lReturn = ExtractNumFromKey(lvwDocList.Items.Item(i - 1).Name, lDocNum)

            If lDocNum = CInt(sDocExCode) Then

                'select the doc
                lvwDocList.Items.Item(i - 1).Selected = True
                lvwDocList.FocusedItem = lvwDocList.Items.Item(i - 1)

                m_cntCurrent = lvwDocList

                If m_cntCurrent.Name = "lvwDocList" Then
                    'view the document
                    ViewDocument()
                End If

                m_cntCurrent = Nothing
                If Not lvwDocList.FocusedItem Is Nothing Then
                    ' Do keywords processing for the selected node.
                    CheckKeywords(lvwDocList.FocusedItem.Name)

                    ' Do Annotations processing for the selected node.
                    CheckAnnotations(lvwDocList.FocusedItem.Name)

                End If

                Exit For
            End If
        Next i

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Exit Sub


        'Could not expanad the cabinet because not preset,
        'so set flag to indicate we need to populate the cabinets
        'and carry on
        bPopCabs = True
        Resume Next

Err_Drawer:
        'Could not expand the Drawer because not preset,
        'so set flag to indicate we need to populate the Drawers
        'and carry on
        bPopDraws = True
        Resume Next

Err_Folder:
        'Could not expanad the Folder because not preset,
        'so set flag to indicate we need to populate the Folders
        'and carry on
        bPopFolds = True
        Resume Next


Err_ConstructView:
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ConstructView", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub

    Private Sub tvwMain_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwMain.MouseMove
        Dim Err_Check_Node As Boolean = False
        Dim Err_tvwMain_MouseMove As Boolean = False
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)


        Try
            Err_tvwMain_MouseMove = True
            Err_Check_Node = False

            'Always keep track of where mouse is on a control, so can check if its over a
            'node when clicked
            m_lX = CInt(X)
            m_lY = CInt(Y)

            'was a node clicked ?
            'm_lReturn = NodeClicked(tvwMain)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Node not clicked on, so exit
                    Exit Sub

                Case gPMConstants.PMEReturnCode.PMTrue

                    'as long as it's not an Add to view node

                    Err_Check_Node = True
                    Err_tvwMain_MouseMove = False

                    If Not tvwMain.SelectedNode Is Nothing Then
                        If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                            Exit Sub
                        End If
                    End If



                    Err_tvwMain_MouseMove = True
                    Err_Check_Node = False

                    'Fine, continue
                    If Button = MouseButtonConstants.LeftButton Then

                        'Save the node being dragged
                        ReDim m_sDragNodes(0)
                        m_sDragNodes(0).Key = tvwMain.GetNodeAt(X, Y).Name
                        m_sDragNodes(0).Text = tvwMain.GetNodeAt(X, Y).Text

                        ' Signal a Drag operation.
                        m_bDragging = True  ' Set the flag to true.


                        'tvwMain.Drag(vbBeginDrag) ' Drag operation.

                    End If

                Case Else
                    'problem, so go.
                    Exit Sub

            End Select

        Catch excep As System.Exception
            If Not Err_Check_Node And Not Err_tvwMain_MouseMove Then
                Throw excep
            End If

            If Err_Check_Node Then

                'on first mousemove after refresh get error checking node
                'because no selected item

                'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")

            End If
            If Err_tvwMain_MouseMove Or Err_Check_Node Then


                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_MouseMove", excep:=excep)

                Exit Sub

            End If
        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: NodeClicked
    '
    ' Description: Using m_lX, m_lY (ie the current position of the mouse
    ' on the current control) check if the event calling this proc is being
    ' done on a node.
    '
    ' ***************************************************************** '
    Private Function NodeClicked(ByRef cnt As Control) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            'Check if mouse currently over a node

            'TODO: No Fix for this
            'If cnt.HitTest(m_lX, m_lY) Is Nothing Then

            'Return gPMConstants.PMEReturnCode.PMFalse
            'Else
            'Return gPMConstants.PMEReturnCode.PMTrue
            'End If


            Return gPMConstants.PMEReturnCode.PMTrue
        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        'Log to File
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Cannot tell if node selected.", vApp:=ACApp, vClass:=ACClass, vMethod:="NodeClicked", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetFolderList
    '
    ' Description: Hit business to get list of folder details for a parent.
    ' Filter is for folder name.
    '
    ' ***************************************************************** '
    Private Function GetFolderList(ByRef lParentNum As Integer, ByRef sFilter As String, ByRef vResultArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lTmp As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the maximum folders retuned depending if filtered
            If sFilter.Trim() = "" Then
                lTmp = m_lMaxFolders
            Else
                lTmp = m_lMaxFilterFolders
            End If

            'Get folderlist for supplied parent

            m_lReturn = g_oBusiness.GetFolderList(lParentNum:=lParentNum, sFilter:=sFilter, lMaxFoldersReturned:=lTmp, vResultArray:=vResultArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Business Failed.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderList")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log to File
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderList", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetDocKeywordList
    '
    ' Description: Hit business to get list of keywords for a document.
    '
    ' ***************************************************************** '
    Private Function GetDocKeywordList(ByRef lDocNum As Integer, ByRef vResultArray(,) As Object) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get keyword list for supplied document

            m_lReturn = g_oBusiness.GetDocKeywordList(lDocNum:=lDocNum, vResultArray:=vResultArray)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Business Failed.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get keyword list from business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocKeywordList")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocKeywordList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAnnotationList
    '
    ' Description: Hit business to get list of annotations for a document.
    '
    ' ***************************************************************** '
    Private Function GetAnnotationList(ByRef lDocNum As Integer, ByRef vResultArray(,) As Object) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get annotation list for supplied doc

            m_lReturn = g_oBusiness.GetAnnotationList(lDocNum:=lDocNum, vResultArray:=vResultArray)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Business Failed.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get annotation list from business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAnnotationList")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAnnotationList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocList
    '
    ' Description: Hit business to get list of documents for a parent
    ' folder. Filter is for doc name.
    '
    ' ***************************************************************** '
    Private Function GetDocList(ByRef lParentNum As Integer, ByRef sFilter As String, ByRef vResultArray As Object) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get doc list for parent folder

            m_lReturn = g_oBusiness.GetDocList(lParentNum:=lParentNum, sFilter:=sFilter, vResultArray:=vResultArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Business Failed.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocList")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateTreeChildren
    '
    ' Description: Populate specific node of treeview with folder list
    ' from the supplied array of folders array.
    '
    ' Edit History: JH051198 Change this so that it will Add Folders to View
    ' ie. if the node already exists then don't add, and sort into
    ' order - public so select folders can use - also incorporate
    ' 'Add Folders to View' node
    '
    ' ***************************************************************** '
    Public Function PopulateTreeChildren(ByRef tvw As TreeView, ByRef iIndex As Integer, ByVal vArray(,) As Object, Optional ByVal iParentKey As String = "") As gPMConstants.PMEReturnCode
        Dim Err_AddingNode As Boolean = False
        Dim Err_PopulateTreeChildren As Boolean = False

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim Node As TreeNode
        Dim sKey As String = ""
        Dim bSplash As Boolean

        Try
            Err_PopulateTreeChildren = True
            Err_AddingNode = False

            result = gPMConstants.PMEReturnCode.PMTrue

            'loop thru array and add each child node
            If Information.IsArray(vArray) Then

                'if weare doing quite a few, splash
                If vArray.GetUpperBound(1) > 200 Then
                    bSplash = True

                    m_lReturn = g_oSplash.Show(DOCSplash_Retrieving)
                End If

                For l As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    'Construct node key

                    If CStr(vArray(2, l)).Trim() <> "" Then
                        'ie passworded


                        sKey = ACFolder & ACPassword & CStr(CInt(CDate(vArray(3, l)).ToOADate)) & CStr(vArray(0, l))
                    Else


                        sKey = ACFolder & " " & CStr(CInt(CDate(vArray(3, l)).ToOADate)) & CStr(vArray(0, l))
                    End If

                    'if node already exists then leave it
                    'only way to check is try adding and error
                    'Add the node

                    Err_AddingNode = True
                    Err_PopulateTreeChildren = False

                    If iParentKey.Trim() <> "" Then
                        If Not tvw.Nodes.Find(iParentKey, True)(0).Nodes(iIndex) Is Nothing Then
                            If Me.IsHandleCreated Then
                                Me.Invoke(New AddTreeNode(AddressOf AddNode), sKey, CStr(vArray(1, l)).Trim(), "IMGCLOSEDFOLDER", tvw.Nodes.Find(iParentKey, True)(0).Nodes(iIndex))
                            Else
                                If Not tvw.Nodes.Find(iParentKey, True)(0).Nodes(iIndex).Nodes.ContainsKey(sKey) Then
                                    Node = tvw.Nodes.Find(iParentKey, True)(0).Nodes(iIndex).Nodes.Add(sKey, CStr(vArray(1, l)).Trim(), "IMGCLOSEDFOLDER")
                                End If
                            End If
                        End If

                    Else
                        If Me.IsHandleCreated Then
                            Me.Invoke(New SetTreeView(AddressOf SetTreeVw), sKey, CStr(vArray(1, l)).Trim(), "IMGCLOSEDFOLDER", iIndex, tvw)
                        Else
                            If Not tvw.Nodes(iIndex).Nodes.ContainsKey(sKey) Then
                                Node = tvw.Nodes(iIndex).Nodes.Add(sKey, CStr(vArray(1, l)).Trim(), "IMGCLOSEDFOLDER")
                            End If
                        End If
                    End If


                    Err_PopulateTreeChildren = True
                    Err_AddingNode = False
                Next l

                If Me.IsHandleCreated Then
                    Me.Invoke(New SortTreeView(AddressOf SortTreeVw), tvw)
                Else
                    tvw.Sort()
                End If



            End If

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            Return result

        Catch excep As System.Exception
            If Not Err_AddingNode And Not Err_PopulateTreeChildren Then
                Throw excep
            End If

            If Err_AddingNode Then


                'node is already there so leave it

                ' Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")

            End If
            If Err_PopulateTreeChildren Or Err_AddingNode Then


                result = gPMConstants.PMEReturnCode.PMError

                'hide splash
                If bSplash Then

                    m_lReturn = g_oSplash.Hide()
                End If

                'Log to File
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateTreeChildren", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End If
        End Try
    End Function

    '***************************************************************************************************************
    'Delegates to handle Documaster through Client Manager 
    '***************************************************************************************************************
    Private Sub SetTreeVw(ByVal sKey As String, ByVal sText As String, ByVal sImageKey As String, ByVal lIndex As Integer, ByRef trv As TreeView)
        If bReloadmode Then
            bReloadmode = False
            trv.Nodes(lIndex).Nodes.Add(sKey, sText, sImageKey)  'Commented by Tariq
        Else
            Dim oTNode As TreeNode = GetSeletedNode(trv:=trv)
            If Not oTNode.Nodes.ContainsKey(sKey) Then
                oTNode.Nodes.Add(sKey, sText, sImageKey)
            End If
        End If
    End Sub


    Private Sub AddNode(ByVal sKey As String, ByVal sText As String, ByVal sImageKey As String, ByRef ParentNode As TreeNode)
        If Not ParentNode.Nodes.ContainsKey(sKey) Then
            ParentNode.Nodes.Add(sKey, sText, sImageKey)
        End If
    End Sub
    Private Sub GetNodeCount(ByVal bincludeSubTrees As Boolean, ByRef lNodeCount As Integer, ByRef ParentNode As TreeNode)
        lNodeCount = ParentNode.GetNodeCount(bincludeSubTrees)
    End Sub
    Private Sub SortTreeVw(ByRef trv As TreeView)
        trv.Sort()
    End Sub
    '***************************************************************************************************************
    '
    '***************************************************************************************************************

    ' ***************************************************************** '
    ' Name: PopulateTreeRoots
    '
    ' Description: Populate root nodes of supplied tree control from array.
    '
    ' ***************************************************************** '

    Private Function PopulateTreeRoots(ByRef tvw As TreeView, ByRef vArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim Node As TreeNode
        Dim sKey As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add each root node.
            If Information.IsArray(vArray) Then
                For i As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    'Construct node key

                    If CStr(vArray(2, i)).Trim() <> "" Then
                        'ie passworded


                        sKey = ACFolder & ACPassword & CStr(CInt(CDate(vArray(3, i)).ToOADate)) & CStr(vArray(0, i))
                    Else


                        sKey = ACFolder & " " & CStr(CInt(CDate(vArray(3, i)).ToOADate)) & CStr(vArray(0, i))
                    End If

                    'Add the node

                    Node = tvw.Nodes.Add(sKey, CStr(vArray(1, i)).Trim(), "IMGCLOSEDFOLDER")

                Next i
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateTreeRoots", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: PopulateListView
    '
    ' Description: Populate listview control from supplied arrays (or
    ' treeview if supplied)
    '
    ' Edit History: JH051198 use this function for SelectFolders
    '
    ' JH050199 put long formatted dates in hidden column so we
    ' can sort properly. Need bExtraDate variable to distinguish
    ' from Select folders calling. DR 4290
    '
    ' ***************************************************************** '
    'Public Function PopulateListView(ByRef lvw As ListView, ByRef vFolderArray(,) As Object, ByRef bDetails As Boolean, ByRef bExtraDate As Boolean, ByRef vDocArray_optional As Object) As gPMConstants.PMEReturnCode
    '    Dim vDocArray(,) As Object = Nothing
    '    If vDocArray_optional Is Nothing Or Not vDocArray_optional.Equals(Type.Missing) Then vDocArray = TryCast(vDocArray_optional, Object())
    '    Try
    '        'vDocArray As Variant)

    '        'To reduce network traffic, use these parmeters and popualte the
    '        'the view directly from the treeview when the folders are already
    '        'there, much slower than getting afresh, though. See NodeCLick
    '        '                                    Optional tvw As TreeView, _
    '        ''                                    Optional nod As node) As Long

    '        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
    '        Dim itmX As ListViewItem
    '        Dim sKey As String = ""
    '        Dim bSplash As Boolean


    '        Try

    '            result = gPMConstants.PMEReturnCode.PMTrue

    '            lvw.Items.Clear()

    '            'See Above comment
    '            '    Dim childnod As node
    '            '    Dim iIndex As Integer
    '            '
    '            '    If (tvw Is Nothing <> True) Then
    '            '
    '            '        'get the first child node
    '            '        Set childnod = nod.Child.FirstSibling
    '            '        iIndex% = childnod.Index
    '            '
    '            '        'add all the other folder nodes to the array.
    '            '        While iIndex <> childnod.LastSibling.Index
    '            '
    '            '            'Add the node and stuff
    '            '            Set itmX = lvw.ListItems.Add(, tvw.Nodes(iIndex%).Key, 1)
    '            '            itmX.Text = tvw.Nodes(iIndex%).Text
    '            '            itmX.SubItems(1) = CDate(Mid$(tvw.Nodes(iIndex%).Key, 3, 5))
    '            '            itmX.ImageKey = "IMGCLOSEDFOLDER"
    '            '            itmX.ImageKey = "IMGCLOSEDFOLDER"
    '            '
    '            '            iIndex = tvw.Nodes(iIndex).Next.Index
    '            '
    '            '        Wend
    '            '
    '            '        Set itmX = lvw.ListItems.Add(, tvw.Nodes(iIndex%).Key, 1)
    '            '        itmX.Text = tvw.Nodes(iIndex%).Text
    '            '        itmX.SubItems(1) = CDate(Mid$(tvw.Nodes(iIndex%).Key, 3, 5))
    '            '        itmX.ImageKey = "IMGCLOSEDFOLDER"
    '            '        itmX.ImageKey = "IMGCLOSEDFOLDER"
    '            '
    '            '    End If


    '            'First add the folder nodes from the folder array
    '            If Information.IsArray(vFolderArray) Then

    '                'if we're doing quite a few, splash
    '                If vFolderArray.GetUpperBound(1) > 200 Then
    '                    bSplash = True
    '                    
    '                    m_lReturn = g_oSplash.Show(DOCSplash_Retrieving)
    '                End If

    '                If bDetails Then

    '                    For lCount As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

    '                        'Check if passworded
    '                        
    '                        If CStr(vFolderArray(2, lCount)).Trim() <> "" Then
    '                            'ie passworded
    '                            
    '                            
    '                            sKey = ACFolder & ACPassword & CStr(CInt(CDate(vFolderArray(3, lCount)).ToOADate)) & CStr(vFolderArray(0, lCount))
    '                        Else
    '                            
    '                            
    '                            sKey = ACFolder & " " & CStr(CInt(CDate(vFolderArray(3, lCount)).ToOADate)) & CStr(vFolderArray(0, lCount))
    '                        End If

    '                        'Add the node and stuff
    '                        itmX = lvw.Items.Add(sKey, CStr(1), "")
    '                        
    '                        itmX.Text = CStr(vFolderArray(1, lCount))
    '                        
    '                        ListViewHelper.GetListViewSubItem(itmX, 1).Text = CStr(vFolderArray(3, lCount)).Trim()

    '                        'JH050199 add another date to sort by
    '                        If bExtraDate Then
    '                            
    '                            ListViewHelper.GetListViewSubItem(itmX, 3).Text = DateTime.FromOADate(CInt(CDate(vFolderArray(3, lCount)).ToOADate)).ToString("yyyyMMddHHMMss")
    '                        End If
    '                        
    '                        itmX.ImageKey = "IMGCLOSEDFOLDER"
    '                        
    '                        itmX.ImageKey = "IMGCLOSEDFOLDER"
    '                    Next lCount
    '                Else
    '                    'add everything without the images

    '                    For lCount As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

    '                        'Check if passworded
    '                        
    '                        If CStr(vFolderArray(2, lCount)).Trim() <> "" Then
    '                            'ie passworded
    '                            
    '                            
    '                            sKey = ACFolder & ACPassword & CStr(CInt(CDate(vFolderArray(3, lCount)).ToOADate)) & CStr(vFolderArray(0, lCount))
    '                        Else
    '                            
    '                            
    '                            sKey = ACFolder & " " & CStr(CInt(CDate(vFolderArray(3, lCount)).ToOADate)) & CStr(vFolderArray(0, lCount))
    '                        End If

    '                        'Add the node and stuff
    '                        itmX = lvw.Items.Add(sKey, CStr(1), "")
    '                        
    '                        itmX.Text = CStr(vFolderArray(1, lCount))
    '                        
    '                        ListViewHelper.GetListViewSubItem(itmX, 1).Text = CStr(vFolderArray(3, lCount)).Trim()

    '                        'JH050199 add another date to sort by
    '                        If bExtraDate Then
    '                            
    '                            ListViewHelper.GetListViewSubItem(itmX, 3).Text = DateTime.FromOADate(CInt(CDate(vFolderArray(3, lCount)).ToOADate)).ToString("yyyyMMddHHMMss")
    '                        End If

    '                    Next lCount
    '                End If
    '            End If

    '            'Now add the document nodes from the document array
    '            If Not Not (vDocArray_optional Is Nothing) AndAlso vDocArray_optional.Equals(Type.Missing) Then
    '                If Information.IsArray(vDocArray) Then

    '                    'if we're doing quite a few, splash
    '                    If (vDocArray.GetUpperBound(1) > 200) And (Not bSplash) Then
    '                        bSplash = True
    '                        
    '                        m_lReturn = g_oSplash.Show(DOCSplash_Retrieving)
    '                    End If

    '                    For lCount As Integer = vDocArray.GetLowerBound(1) To vDocArray.GetUpperBound(1)

    '                        'Check if passworded
    '                        
    '                        If CStr(vDocArray(2, lCount)).Trim() <> "" Then
    '                            'ie passworded
    '                            
    '                            
    '                            sKey = ACDocument & ACPassword & CStr(CInt(CDate(vDocArray(4, lCount)).ToOADate)) & CStr(vDocArray(0, lCount))
    '                        Else
    '                            
    '                            
    '                            sKey = ACDocument & " " & CStr(CInt(CDate(vDocArray(4, lCount)).ToOADate)) & CStr(vDocArray(0, lCount))
    '                        End If

    '                        'Add node and stuff
    '                        itmX = lvw.Items.Add(sKey, CStr(1), "")
    '                        
    '                        itmX.Text = CStr(vDocArray(1, lCount)).Trim()
    '                        
    '                        ListViewHelper.GetListViewSubItem(itmX, 1).Text = CStr(vDocArray(4, lCount)).Trim()
    '                        '090699 Use constants instead of charaters more descriptive easier to maintain
    '                        
    '                        Select Case CStr(vDocArray(3, lCount))
    '                            Case kDocFileTypeTIF
    '                                
    '                                itmX.ImageKey = "IMGTIFF"
    '                                
    '                                itmX.ImageKey = "IMGTIFF"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCImage
    '                            Case kDocFileTypeRTF, "L", "R"
    '                                
    '                                itmX.ImageKey = "IMGRTF"
    '                                
    '                                itmX.ImageKey = "IMGRTF"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCRTF
    '                            Case kDocFileTypeTXT, "N"
    '                                
    '                                itmX.ImageKey = "IMGTEXT"
    '                                
    '                                itmX.ImageKey = "IMGTEXT"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCText
    '                            Case kDocFileTypeWRD ' Ms Word
    '                                
    '                                itmX.ImageKey = "IMGWORD"
    '                                
    '                                itmX.ImageKey = "IMGWORD"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCWRD
    '                            Case kDocFileTypeEXL
    '                                
    '                                itmX.ImageKey = "IMGEXCEL" 'Ms Excel
    '                                
    '                                itmX.ImageKey = "IMGEXCEL"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCEXL
    '                            Case kDocFileTypePWP 'Ms Powerpoint
    '                                
    '                                itmX.ImageKey = "IMGPOWERPNT"
    '                                
    '                                itmX.ImageKey = "IMGPOWERPNT"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCPWP
    '                            Case kDocFileTypeACC 'Ms Access
    '                                
    '                                itmX.ImageKey = "IMGACCESS"
    '                                
    '                                itmX.ImageKey = "IMGACCESS"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCACC
    '                            Case kDocFileTypeHTM 'Ms Internet Exployer
    '                                
    '                                itmX.ImageKey = "IMGIEXPLORER"
    '                                
    '                                itmX.ImageKey = "IMGIEXPLORER"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCHTM
    '                            Case kDocFileTypeGIF 'Gif
    '                                
    '                                itmX.ImageKey = "IMGGIF"
    '                                
    '                                itmX.ImageKey = "IMGGIF"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCGIF
    '                            Case kDocFileTypeJPG 'Jpeg
    '                                
    '                                itmX.ImageKey = "IMGJPEG"
    '                                
    '                                itmX.ImageKey = "IMGJPEG"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCJPG
    '                            Case kDocFileTypeEML 'E-mail Doc
    '                                
    '                                itmX.ImageKey = "IMGOUTLOOK"
    '                                
    '                                itmX.ImageKey = "IMGOUTLOOK"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCEML
    '                            Case kDocFileTypePDF 'Adobe Pdf Files
    '                                
    '                                itmX.ImageKey = "IMGADOBE"
    '                                
    '                                itmX.ImageKey = "IMGADOBE"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCPDF
    '                            Case kDocFileTypeHLP 'Help Files
    '                                
    '                                itmX.ImageKey = "IMGHELP"
    '                                
    '                                itmX.ImageKey = "IMGHELP"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCHLP
    '                            Case kDocFileTypeZIP 'Zip Files
    '                                
    '                                itmX.ImageKey = "IMGZIP"
    '                                
    '                                itmX.ImageKey = "IMGZIP"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCZIP
    '                            Case Else
    '                                'General Document
    '                                
    '                                itmX.ImageKey = "IMGUNKNOWN"
    '                                
    '                                itmX.ImageKey = "IMGUNKNOWN"
    '                                ListViewHelper.GetListViewSubItem(itmX, 2).Text = DOCUnknown
    '                        End Select

    '                        'JH050199 add another date to sort by
    '                        If bExtraDate Then
    '                            
    '                            ListViewHelper.GetListViewSubItem(itmX, 3).Text = CDate(vDocArray(4, lCount)).ToString("yyyyMMddHHMMss")
    '                        End If

    '                    Next lCount
    '                End If

    '                'JH051198 this is only if we want details
    '                If bDetails Then
    '                    'If we have repopulated the document listview, then the keywords will no longer
    '                    'be applicable - so clear them out
    '                    If lvwKeyWords.Visible Then
    '                        lvwKeyWords.Items.Clear()
    '                        lvwKeyWords.Tag = ""
    '                    End If

    '                    'likewise annotations
    '                    If lvwAnnotations.Visible Then
    '                        lvwAnnotations.Items.Clear()
    '                        lvwAnnotations.Tag = ""
    '                    End If

    '                    UpdateStatusBar(lvwDocList)
    '                End If
    '            End If

    '            'hide splash
    '            If bSplash Then
    '                
    '                m_lReturn = g_oSplash.Hide()
    '            End If

    '            Return result

    '        Catch excep As System.Exception



    '            result = gPMConstants.PMEReturnCode.PMError

    '            'hide splash
    '            If bSplash Then
    '                
    '                m_lReturn = g_oSplash.Hide()
    '            End If

    '            'Log to File
    '            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '            Return result

    '        End Try
    '    Finally
    '        vDocArray_optional = vDocArray
    '    End Try
    'End Function
    Public Function PopulateListView(ByRef lvw As ListView, ByRef vFolderArray(,) As Object, ByRef bDetails As Boolean, ByRef bExtraDate As Boolean, Optional ByRef vDocArray(,) As Object = Nothing) As gPMConstants.PMEReturnCode
        'vDocArray As Variant)

        'To reduce network traffic, use these parmeters and popualte the
        'the view directly from the treeview when the folders are already
        'there, much slower than getting afresh, though. See NodeCLick
        '                                    Optional tvw As TreeView, _
        ''                                    Optional nod As node) As Long

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Dim itmX As ListViewItem
        Dim sKey As String = ""
        Dim bSplash As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            lvw.Items.Clear()

            'See Above comment
            '    Dim childnod As node
            '    Dim iIndex As Integer
            '
            '    If (tvw Is Nothing <> True) Then
            '
            '        'get the first child node
            '        Set childnod = nod.Child.FirstSibling
            '        iIndex% = childnod.Index
            '
            '        'add all the other folder nodes to the array.
            '        While iIndex <> childnod.LastSibling.Index
            '
            '            'Add the node and stuff
            '            Set itmX = lvw.Items.Add(, tvw.Nodes(iIndex%).Text, 1)
            '            itmX.Text = tvw.Nodes(iIndex%).Text
            '            itmX.SubItems(1) = CDate(Mid$(tvw.Nodes(iIndex%).Text, 3, 5))
            '            itmX.ImageKey = "IMGCLOSEDFOLDER"
            '            itmX.ImageKey = "IMGCLOSEDFOLDER"
            '
            '            iIndex = tvw.Nodes(iIndex).Next.Index
            '
            '        Wend
            '
            '        Set itmX = lvw.Items.Add(, tvw.Nodes(iIndex%).Text, 1)
            '        itmX.Text = tvw.Nodes(iIndex%).Text
            '        itmX.SubItems(1) = CDate(Mid$(tvw.Nodes(iIndex%).Text, 3, 5))
            '        itmX.SmallIcon = "IMGCLOSEDFOLDER"
            '        itmX.ImageKey = "IMGCLOSEDFOLDER"
            '
            '    End If


            'First add the folder nodes from the folder array
            If Information.IsArray(vFolderArray) Then

                'if we're doing quite a few, splash
                If vFolderArray.GetUpperBound(1) > 200 Then
                    bSplash = True

                    m_lReturn = g_oSplash.Show(DOCSplash_Retrieving)
                End If

                If bDetails Then

                    For lCount As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

                        'Check if passworded

                        If CStr(vFolderArray(2, lCount)).Trim() <> "" Then
                            'ie passworded


                            sKey = ACFolder & ACPassword & CStr(CInt(CDate(vFolderArray(3, lCount)).ToOADate)) & CStr(vFolderArray(0, lCount))
                        Else


                            sKey = ACFolder & " " & CStr(CInt(CDate(vFolderArray(3, lCount)).ToOADate)) & CStr(vFolderArray(0, lCount))
                        End If

                        'Add the node and stuff

                        itmX = lvw.Items.Add(sKey, 1)
                        itmX.Name = sKey


                        itmX.Text = vFolderArray(1, lCount)


                        'itmX.SubItems(1).Text = CStr(vFolderArray(3, lCount)).Trim()
                        'itmX.SubItems(0).Text = CStr(vFolderArray(3, lCount)).Trim()
                        itmX.SubItems.Add(CStr(vFolderArray(3, lCount)).Trim())
                        itmX.SubItems.Add("")
                        'JH050199 add another date to sort by
                        If bExtraDate Then


                            'itmX.SubItems(3).Text = DateTime.FromOADate(CInt(CDate(vFolderArray(3, lCount)).ToOADate)).ToString("yyyyMMddHHMMss")
                            itmX.SubItems.Add(DateTime.FromOADate(CInt(CDate(vFolderArray(3, lCount)).ToOADate)).ToString("yyyyMMddHHMMss"))
                            'itmX.SubItems(2).Text = DateTime.FromOADate(CInt(CDate(vFolderArray(3, lCount)).ToOADate)).ToString("yyyyMMddHHMMss")
                        End If

                        itmX.ImageKey = "IMGCLOSEDFOLDER"

                        itmX.ImageKey = "IMGCLOSEDFOLDER"
                    Next lCount
                Else
                    'add everything without the images

                    For lCount As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

                        'Check if passworded

                        If CStr(vFolderArray(2, lCount)).Trim() <> "" Then
                            'ie passworded


                            sKey = ACFolder & ACPassword & CStr(CInt(CDate(vFolderArray(3, lCount)).ToOADate)) & CStr(vFolderArray(0, lCount))
                        Else


                            sKey = ACFolder & " " & CStr(CInt(CDate(vFolderArray(3, lCount)).ToOADate)) & CStr(vFolderArray(0, lCount))
                        End If

                        'Add the node and stuff

                        itmX = lvw.Items.Add(sKey, 1)
                        itmX.Name = sKey


                        itmX.Text = vFolderArray(1, lCount)


                        'itmX.SubItems(1).Text = CStr(vFolderArray(3, lCount)).Trim()
                        'itmX.SubItems(0).Text = CStr(vFolderArray(3, lCount)).Trim()
                        itmX.SubItems.Add(CStr(vFolderArray(3, lCount)).Trim())

                        'JH050199 add another date to sort by
                        If bExtraDate Then


                            'itmX.SubItems(3).Text = DateTime.FromOADate(CInt(CDate(vFolderArray(3, lCount)).ToOADate)).ToString("yyyyMMddHHMMss")
                            itmX.SubItems.Add(DateTime.FromOADate(CInt(CDate(vFolderArray(3, lCount)).ToOADate)).ToString("yyyyMMddHHMMss"))

                        End If

                    Next lCount
                End If
            End If

            'Now add the document nodes from the document array

            If Not Information.IsNothing(vDocArray) Then
                If Information.IsArray(vDocArray) Then

                    'if we're doing quite a few, splash
                    If (vDocArray.GetUpperBound(1) > 200) And (Not bSplash) Then
                        bSplash = True

                        m_lReturn = g_oSplash.Show(DOCSplash_Retrieving)
                    End If

                    For lCount As Integer = vDocArray.GetLowerBound(1) To vDocArray.GetUpperBound(1)

                        'Check if passworded

                        If CStr(vDocArray(2, lCount)).Trim() <> "" Then
                            'ie passworded


                            sKey = ACDocument & ACPassword & CStr(CInt(CDate(vDocArray(4, lCount)).ToOADate)) & CStr(vDocArray(0, lCount))
                        Else


                            sKey = ACDocument & " " & CStr(CInt(CDate(vDocArray(4, lCount)).ToOADate)) & CStr(vDocArray(0, lCount))
                        End If

                        'Add node and stuff

                        itmX = lvw.Items.Add(sKey, 1)
                        itmX.Name = sKey


                        itmX.Text = CStr(vDocArray(1, lCount)).Trim()


                        'itmX.SubItems(1).Text = CStr(vDocArray(4, lCount)).Trim()
                        itmX.SubItems.Add(CStr(vDocArray(4, lCount)).Trim())
                        'itmX.SubItems(0).Text = CStr(vDocArray(4, lCount)).Trim()
                        '090699 Use constants instead of charaters more descriptive easier to maintain
                        If vDocArray(5, lCount).ToString = "" Then
                            Select Case CStr(vDocArray(3, lCount))
                                Case kDocFileTypeTIF
                                    itmX.ImageIndex = 1
                                    itmX.SubItems.Add(DOCImage)
                                Case kDocFileTypeBMP
                                    itmX.ImageIndex = 1
                                    itmX.SubItems.Add(DOCBMP)
                                Case kDocFileTypeRTF, "L", "R"
                                    itmX.ImageIndex = 2
                                    itmX.SubItems.Add(DOCRTF)
                                Case kDocFileTypeTXT, "N"
                                    itmX.ImageIndex = 4
                                    itmX.SubItems.Add(DOCText)
                                Case kDocFileTypeXML
                                    itmX.ImageIndex = 10
                                    itmX.SubItems.Add(DOCXML)
                                Case kDocFileTypeWRD  ' Ms Word
                                    itmX.ImageIndex = 6
                                    itmX.SubItems.Add(DOCWRD)
                                Case kDocFileTypeEXL

                                    itmX.ImageIndex = 7
                                    itmX.SubItems.Add(DOCEXL)
                                Case kDocFileTypePWP  'Ms Powerpoint
                                    itmX.ImageIndex = 8
                                    itmX.SubItems.Add(DOCPWP)
                                Case kDocFileTypeACC  'Ms Access
                                    itmX.ImageIndex = 9
                                    itmX.SubItems.Add(DOCACC)
                                Case kDocFileTypeHTM  'Ms Internet Exployer
                                    itmX.ImageIndex = 10
                                    itmX.SubItems.Add(DOCHTM)
                                Case kDocFileTypeGIF  'Gif
                                    itmX.ImageIndex = 11
                                    itmX.SubItems.Add(DOCGIF)
                                Case kDocFileTypeJPG  'Jpeg
                                    itmX.ImageIndex = 12
                                    itmX.SubItems.Add(DOCJPG)
                                Case kDocFileTypeEML  'E-mail Doc
                                    itmX.ImageIndex = 13
                                    itmX.SubItems.Add(DOCEML)
                                Case kDocFileTypePDF  'Adobe Pdf Files
                                    itmX.ImageIndex = 14
                                    itmX.SubItems.Add(DOCPDF)
                                Case kDocFileTypeHLP  'Help Files
                                    itmX.ImageIndex = 16
                                    itmX.SubItems.Add(DOCHLP)
                                Case kDocFileTypeZIP  'Zip Files
                                    itmX.ImageIndex = 16
                                    itmX.SubItems.Add(DOCZIP)
                                Case Else
                                    itmX.ImageIndex = 0
                                    itmX.SubItems.Add(DOCUnknown)
                            End Select
                        Else
                            'Add new icons for migrated docs
                            Select Case CStr(vDocArray(5, lCount))
                                Case "FAIL"
                                    itmX.ImageIndex = 17
                                Case "WIP"
                                    itmX.ImageIndex = 19
                                Case "COMPLETE"
                                    itmX.ImageIndex = 18
                                Case Else
                                    itmX.ImageIndex = 0
                                    itmX.SubItems.Add(DOCUnknown)
                            End Select
                        End If

                        'JH050199 add another date to sort by
                        If bExtraDate Then


                            'itmX.SubItems(3).Text = CDate(vDocArray(4, lCount)).ToString("yyyyMMddHHMMss")
                            itmX.SubItems.Add(CDate(vDocArray(4, lCount)).ToString("yyyyMMddHHMMss"))
                        End If

                    Next lCount
                End If

                'JH051198 this is only if we want details
                If bDetails Then
                    'If we have repopulated the document listview, then the keywords will no longer
                    'be applicable - so clear them out

                    If lvwKeyWords.Visible Then

                        lvwKeyWords.Items.Clear()

                        lvwKeyWords.Tag = ""
                    End If

                    'likewise annotations

                    If lvwAnnotations.Visible Then

                        lvwAnnotations.Items.Clear()

                        lvwAnnotations.Tag = ""
                    End If

                    UpdateStatusBar(lvwDocList)
                End If
            End If

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'Public Function PopulateListView(ByRef lvw As ListView, ByRef vFolderArray(,) As Object, ByRef bDetails As Boolean, ByRef bExtraDate As Boolean) As gPMConstants.PMEReturnCode
    '    Dim tempRefParam As Object = Type.Missing
    '    Return PopulateListView(lvw, vFolderArray, bDetails, bExtraDate, tempRefParam)
    'End Function
    ' ***************************************************************** '
    ' Name: PopulateKeywordsListView
    '
    ' Description: Populate keyword listview from supplied array.
    '
    ' ***************************************************************** '
    Private Function PopulateKeywordsListView(ByRef lvw As ListView, ByRef vKeywordArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim itmX As ListViewItem
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvw.Items.Clear()

            'Now add the keyword nodes from the keyword array
            If Information.IsArray(vKeywordArray) Then
                For i As Integer = vKeywordArray.GetLowerBound(1) To vKeywordArray.GetUpperBound(1)

                    'Add node and stuff

                    itmX = lvw.Items.Add("K" & CStr(vKeywordArray(0, i)), CStr(1), "")

                    itmX.Text = CStr(vKeywordArray(1, i))

                    itmX.ImageKey = "KEYWORD"

                    ListViewHelper.GetListViewSubItem(itmX, 1).Text = CStr(vKeywordArray(2, i))

                    ListViewHelper.GetListViewSubItem(itmX, 2).Text = CStr(vKeywordArray(3, i))

                Next i
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateKeywordsListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateAnnotationsListView
    '
    ' Description: Populate annotation listview from supplied array.
    '
    ' ***************************************************************** '
    Private Function PopulateAnnotationsListView(ByRef lvw As ListView, ByRef vAnnotationArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim itmX As ListViewItem


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvw.Items.Clear()

            'Now add the keyword nodes from the keyword array
            If Information.IsArray(vAnnotationArray) Then
                For i As Integer = vAnnotationArray.GetLowerBound(1) To vAnnotationArray.GetUpperBound(1)

                    'Add node and stuff

                    itmX = lvw.Items.Add("A" & CStr(vAnnotationArray(0, i)), CStr(1), "")

                    itmX.Text = CStr(vAnnotationArray(1, i))

                    itmX.ImageKey = "ANNOTATION"

                    ListViewHelper.GetListViewSubItem(itmX, 1).Text = CStr(vAnnotationArray(2, i))

                    ListViewHelper.GetListViewSubItem(itmX, 2).Text = CStr(vAnnotationArray(3, i))

                Next i
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAnnotationsListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RemoveTreeChildren
    '
    ' Description: Removes all child nodes for a given treeview control
    ' and parent node.
    '
    ' ***************************************************************** '
    Private Function RemoveTreeChildren(ByRef tvw As TreeView, ByRef nod As TreeNode) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'remove the first child node till none left
            For Each tn As TreeNode In nod.Nodes
                tn.Remove()
            Next
            'While nod.GetNodeCount(False)
            '    tvw.Nodes.RemoveAt(nod.FirstNode.Index - 1)
            'End While

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveTreeChildren", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetFolderListFromListView
    '
    ' Description: Populate array of folders for a parent, but from current
    ' listview. Saves having to go to the database when the listview
    ' requires a folder list. Get it from the tree if its there.
    '
    ' ***************************************************************** '
    Private Function GetFolderListFromListView(ByRef lvw As ListView, ByRef vArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim i As Integer
        Dim bSplash As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'initialize array
            ReDim vArray(3, 0)

            'splash if quite a few
            If lvwDocList.Items.Count > 200 Then
                bSplash = True

                m_lReturn = g_oSplash.Show(DOCSplash_Processing)
            End If

            'For each folder item, stick the data from the key into the array
            For iIndex As Integer = 1 To lvwDocList.Items.Count

                If lvw.Items.Item(iIndex - 1).Name.Substring(0, 1) <> ACDocument Then

                    ReDim Preserve vArray(vArray.GetUpperBound(0), i)


                    vArray(0, i) = lvw.Items.Item(iIndex - 1).Name.Substring(lvw.Items.Item(iIndex - 1).Name.Length - (Strings.Len(lvw.Items.Item(iIndex - 1).Name) - DOCNodeKeyOffSet))

                    vArray(1, i) = lvw.Items.Item(iIndex - 1).Text
                    If lvw.Items.Item(iIndex - 1).Name.Substring(ACPasswordStart - 1, Math.Min(lvw.Items.Item(iIndex - 1).Name.Length, ACPasswordLen)) = ACPassword Then

                        vArray(2, i) = ACPassword
                    End If

                    vArray(3, i) = lvw.Items.Item(iIndex - 1).Name.Substring(ACDateStart - 1, Math.Min(lvw.Items.Item(iIndex - 1).Name.Length, ACDateLen))

                    i += 1

                End If

            Next iIndex

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderListFromListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: LocateDocument
    '
    ' Description: This method is called from the document viewer when
    ' user wishes to return to the manager, with the folder for the
    ' currently selected document expanded.
    '
    ' It basically locates the parent folder and highlights the doc.
    '
    ' ***************************************************************** '
    Public Sub LocateDocument(ByRef sKey As String)
        Dim Err_InfoShowing As Boolean = False
        Dim Err_LocateDocument As Boolean = False

        Dim bDocFound As Boolean
        Dim lFolderNum, lDocNum, lTmp As Integer


        Try
            Err_LocateDocument = True
            Err_InfoShowing = False

            ' if form currently minimised, make it maximised
            If WindowState = FormWindowState.Minimized Then
                WindowState = FormWindowState.Maximized
            End If

            'if no node to find, just show
            If sKey = "" Then
                lvwDocList.Focus()
                Exit Sub
            End If

            'Go thru doc list and select the document
            'select all docs in the doc list view
            For i As Integer = 1 To lvwDocList.Items.Count

                If lvwDocList.Items.Item(i - 1).Name = sKey Then
                    lvwDocList.Items.Item(i - 1).Selected = True
                    lvwDocList.FocusedItem = lvwDocList.Items.Item(i - 1)
                    bDocFound = True
                Else
                    lvwDocList.Items.Item(i - 1).Selected = False
                End If

            Next i

            Err_InfoShowing = True
            Err_LocateDocument = False  'JH231298

            'did we find it
            If bDocFound Then
                lvwDocList.Focus()
                Exit Sub
            End If

            Err_LocateDocument = True
            Err_InfoShowing = False

            If tvwMain.SelectedNode.Name.Substring(0, 3) = "ADD" Then
                Exit Sub
            End If


            'we didn't, so get parent folder and locate that
            m_lReturn = ExtractNumFromKey(sKey, lDocNum)


            m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Document, lNodeNum:=lDocNum, lParentNum:=lFolderNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get parent folder number", vApp:=ACApp, vClass:=ACClass, vMethod:="LocateDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            LocateFolder(lFolderNum, False)


            'select appropriate doc
            'the key may have changed though if user passworded the doc in
            'the viewer, so have to compare actual document number

            For i As Integer = 1 To lvwDocList.Items.Count

                m_lReturn = ExtractNumFromKey(lvwDocList.Items.Item(i - 1).Name, lTmp)

                If lTmp = lDocNum Then
                    lvwDocList.FocusedItem = lvwDocList.Items.Item(i - 1)
                    Exit For
                End If

            Next i

            ' Do keywords processing for the selected node.
            CheckKeywords(lvwDocList.FocusedItem.Name)

            ' Do Annotations processing for the selected node.
            CheckAnnotations(lvwDocList.FocusedItem.Name)

            lvwDocList.Focus()

        Catch excep As System.Exception
            If Not Err_InfoShowing And Not Err_LocateDocument Then
                Throw excep
            End If

            If Err_InfoShowing Then


                'JH231298
                'get error 5 if information form is showing (as it is modal)

                If Information.Err().Number = 5 Then

                    ' Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")
                Else
                    'TODO
                    'GoTo Err_LocateDocument
                End If


            End If
            If Err_LocateDocument Or Err_InfoShowing Then


                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="LocateDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Sub

            End If
        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: UpdateDocumentName
    '
    ' Description: This section is called from the viewer is the user
    ' changes the document name. It updates the text description of
    ' the document
    '
    ' ***************************************************************** '
    Public Sub UpdateDocumentName(ByRef sKey As String, ByRef sNewName As String)


        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_UpdateDocumentName)")

        'The node may (quite reasonably) not exist so turn off error trapping
        Try
            lvwDocList.Items.Item(sKey).Text = sNewName

        Catch
        End Try

        Exit Sub

Err_UpdateDocumentName:

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocumentName", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub

    ' ***************************************************************** '
    ' Name: EnableMenuItems
    '
    ' Description: This section enables the correct menu items according
    ' to the control with focus (and which node selected if appropriate)
    ' The constants indicate which controls that menu item is enabled
    ' for
    '
    '   Position 1  - tvwmain
    '   Position 2  - tvwfav
    '   Position 3  - tvwfind
    '   Position 4  - tvwbcmain
    '   Position 5  - lvwdoclist
    '   Position 6  - lvwdocsonly
    '   Position 7  - lvwbcdocsonly
    '   Position 8  - lvwkeywords
    '   Position 9  - lvwannotations
    '   Position 10 - lvwpages
    '
    '   eg FileSaveAs = "0000100001" - Save As menu item only available
    '   when lvwDocList or lvwPages have focus
    '
    '   This part will enable the max possible menu items according to
    '   the control with focus  -the adjustmenuitems bit at the end then
    '   disables any that are not applicable according to what nodes
    '   are currently selected in the control with focus.
    '
    '   Any menu items that do not appear here (such as utilities) are
    '   always available regardless of the control with focus
    '
    ' ***************************************************************** '
    Private Sub EnableMenuItems(ByRef cnt As Control)

        Dim iTmp As String = ""
        Dim iPosition As Integer

        'File Menu Items
        Const FileOpenDocument As String = "0000100000"
        Const FileFilter As String = "1100000000"
        Const FileNew As String = "1100000000"
        Const FileImport As String = "1100000000"
        ' Const FileEmail As String = "0000100000" 'DN 12/12/00
        ' Const FileArchive As String = "0000100000" 'MS 01/06/00   enable when on doclist
        Const FileSaveAs As String = "0000100001"
        Const FileDelete As String = "1100100111"
        Const FileRename As String = "1100100000"
        Const FileInformation As String = "0000100000"
        Const FilePrint As String = "0000100001"
        Const FileScan As String = "1000000000"
        ' Const FileSelect As String = "1000000000"

        'Edit Menu Items
        Const EditCut As String = "1000100000"
        Const EditCopy As String = "1000100000"
        Const EditPaste As String = "1000100000"
        'Const EditSelectAll = "0000100111"
        Const EditSelectAll As String = "1110100111"

        'Tools Menu Items
        Const ToolsFind As String = "1111111111"
        Const ToolsGoHome As String = "1111111111"
        Const ToolsPassword As String = "1000100000"
        Const ToolsAccess As String = "1000100000"
        Const ToolsAddAnn As String = "0000100000"
        Const ToolsAddKeyword As String = "0000100000"
        Const ToolsSetHome As String = "1000000000"

        'Popup Menu Items
        Const PopFilter As String = "1110000000"
        Const PopOpenDocument As String = "0000100001"
        Const PopFind As String = "1110000000"
        Const PopSetHome As String = "1000000000"
        ' Const PopSubFolder As String = "1100000000" 'DN 12/12/00
        Const PopAddKeyword As String = "0000100000"
        Const PopAddAnn As String = "0000100000"
        Const PopPassword As String = "1110100000"
        Const PopAccess As String = "1000100000"
        Const PopCut As String = "1000100000"
        Const PopCopy As String = "1000100000"
        Const PopPaste As String = "1000100000"
        Const PopDelete As String = "1000100111"
        'Bug in listview - startlabeledit wont work when invoked from a pop
        'up menu, so disable rename on pop menu for listview
        'Const PopRename = "1000000000"
        Const PopRename As String = "1000100000"  'GP 11/02/02 changed in order to rename documents
        Const PopInformation As String = "0000100000"
        Const PopScan As String = "1000000000"
        Const PopEmail As String = "0000100000"  'DN 12/12/00
        Const PopArchive As String = "0000100000"  'MS 01/06/00   enable when on doclist

        Try

            'Check what control has focus and set the appropriate menu
            Select Case cnt.Name
                Case "tvwMain"
                    iPosition = 1
                Case "tvwFav"
                    iPosition = 2
                Case "tvwFind"
                    iPosition = 3
                Case "tvwBCMain"
                    iPosition = 4
                Case "lvwDocList"
                    iPosition = 5
                Case "lvwDocsOnly"
                    iPosition = 6
                Case "lvwBCDocsOnly"
                    iPosition = 7
                Case "lvwKeyWords"
                    iPosition = 8
                Case "lvwAnnotations"
                    iPosition = 9
                Case "lvwPages"
                    iPosition = 10
                Case Else
                    'What else could there be ?

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="EnableMenuItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub
            End Select

            'File Items
            mnuFileOpenDocument.Enabled = CBool(FileOpenDocument.Substring(iPosition - 1, 1))
            mnuFileFilter.Enabled = CBool(FileFilter.Substring(iPosition - 1, 1))
            mnuFileNew.Enabled = CBool(FileNew.Substring(iPosition - 1, 1))
            mnuFileImport.Enabled = CBool(FileImport.Substring(iPosition - 1, 1))
            mnuFileEmail.Enabled = CBool(PopEmail.Substring(iPosition - 1, 1))  'DN 12/12/00
            mnuFileArchive.Enabled = CBool(PopArchive.Substring(iPosition - 1, 1))  'MS 01/06/00
            mnuFileSaveAs.Enabled = CBool(FileSaveAs.Substring(iPosition - 1, 1))
            mnuFileDelete.Enabled = CBool(FileDelete.Substring(iPosition - 1, 1))
            mnuFileRename.Enabled = CBool(FileRename.Substring(iPosition - 1, 1))
            mnuFileInformation.Enabled = CBool(FileInformation.Substring(iPosition - 1, 1))
            mnuFilePrint.Enabled = CBool(FilePrint.Substring(iPosition - 1, 1))
            mnuFileScan.Enabled = CBool(FileScan.Substring(iPosition - 1, 1))

            'Edit Items
            mnuEditCut.Enabled = CBool(EditCut.Substring(iPosition - 1, 1))
            mnuEditCopy.Enabled = CBool(EditCopy.Substring(iPosition - 1, 1))
            mnuEditPaste.Enabled = CBool(EditPaste.Substring(iPosition - 1, 1))
            'disable the paste regardless, if clipboard is empty
            If m_iPasteFlag = ACPasteEmpty Then
                mnuEditPaste.Enabled = False
            End If
            mnuEditSelectAll.Enabled = CBool(EditSelectAll.Substring(iPosition - 1, 1))

            'Tools Items
            mnuToolsFind.Enabled = CBool(ToolsFind.Substring(iPosition - 1, 1))
            mnuToolsGoHome.Enabled = CBool(ToolsGoHome.Substring(iPosition - 1, 1))
            mnuToolsPassword.Enabled = CBool(ToolsPassword.Substring(iPosition - 1, 1))
            mnuToolsAccess.Enabled = CBool(ToolsAccess.Substring(iPosition - 1, 1))
            mnuToolsAddAnn.Enabled = CBool(ToolsAddAnn.Substring(iPosition - 1, 1))
            mnuToolsAddKeyword.Enabled = CBool(ToolsAddKeyword.Substring(iPosition - 1, 1))
            mnuToolsSetHome.Enabled = CBool(ToolsSetHome.Substring(iPosition - 1, 1))

            'Popup items
            mnuPopFilter.Enabled = CBool(PopFilter.Substring(iPosition - 1, 1))
            mnuPopOpenDocument.Enabled = CBool(PopOpenDocument.Substring(iPosition - 1, 1))
            mnuPopFind.Enabled = CBool(PopFind.Substring(iPosition - 1, 1))
            mnuPopSetHome.Enabled = CBool(PopSetHome.Substring(iPosition - 1, 1))
            mnuPopSubFolder.Enabled = CBool(FileNew.Substring(iPosition - 1, 1))  'DN 12/12/00
            mnuPopAddKeyword.Enabled = CBool(PopAddKeyword.Substring(iPosition - 1, 1))
            mnuPopAddAnn.Enabled = CBool(PopAddAnn.Substring(iPosition - 1, 1))
            mnuPopPassword.Enabled = CBool(PopPassword.Substring(iPosition - 1, 1))
            mnuPopAccess.Enabled = CBool(PopAccess.Substring(iPosition - 1, 1))
            mnuPopCut.Enabled = CBool(PopCut.Substring(iPosition - 1, 1))
            mnuPopCopy.Enabled = CBool(PopCopy.Substring(iPosition - 1, 1))
            mnuPopPaste.Enabled = CBool(PopPaste.Substring(iPosition - 1, 1))
            'disable the paste regardless, if clipboard is empty
            If m_iPasteFlag = ACPasteEmpty Then
                mnuPopPaste.Enabled = False
            End If
            mnuPopDelete.Enabled = CBool(PopDelete.Substring(iPosition - 1, 1))
            mnuPopRename.Enabled = CBool(PopRename.Substring(iPosition - 1, 1))
            mnuPopInformation.Enabled = CBool(PopInformation.Substring(iPosition - 1, 1))
            mnuPopScan.Enabled = CBool(PopScan.Substring(iPosition - 1, 1))
            mnuPopEmail.Enabled = CBool(PopEmail.Substring(iPosition - 1, 1))  'DN 12/12/00
            mnuPopArchive.Enabled = CBool(PopArchive.Substring(iPosition - 1, 1))  'MS 01/06/00

            'Do some tweaking now here according to the control and whether
            'any nodes have benn selected (and if they are docs or folders)
            AdjustMenuItems(cnt)

        Catch excep As System.Exception



            'Log error
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="EnableMenuItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Sub DragOverCheck()
        Dim iCntl As Integer
        Dim bCopy As Boolean
        iCntl = GetKeyState(VK_CONTROL)
        bCopy = (iCntl And &H8000S) <> 0
        If bCopy Then

        Else

        End If
    End Sub
    ' ***************************************************************** '
    ' Name: DragOverCheck
    '
    ' Description: This section is performed for the dragover event of
    ' every control. According to the control being dragged over
    ' (cntTarget) and the control being dragged (cntSource) the
    ' appropriate ImageKey is displayed. Also the state of the control key
    ' is checked for multiple moves.
    '
    ' ***************************************************************** '
    'Private Sub DragOverCheck(ByRef cntTarget As Control, ByRef cntSource As Control, Optional ByRef X As Single = 0, Optional ByRef Y As Single = 0)

    '    Dim iCntl As Integer
    '    Dim bCopy As Boolean


    '    Try

    '        No point doing this if we are not even doing a drag operation
    '        If Not m_bDragging Then
    '            Exit Sub
    '        End If

    '        Check if copying or not by seeing if Control Key pressed
    '        iCntl = GetKeyState(VK_CONTROL)
    '        bCopy = (iCntl And &H8000S) <> 0

    '        Check the destination control, then action according to the source

    '        Select Case cntTarget.Name
    '            Case "tvwMain"

    '                Select Case cntSource.Name
    '                    Case "lvwDocList"

    '                        If bCopy Then
    '                            UPGRADE_ISSUE: (2064) ComctlLib.IListItem property lvwDocList.SelectedItem.ImageKey was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            Select Case lvwDocList.SelectedItems(0).ImageKey
    '                                Case "IMGCLOSEDFOLDER"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGCLOSEDFOLDERMULTI")
    '                                Case "IMGRTF"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGRTFMULTI")
    '                                Case "IMGTIFF"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGTIFFMULTI")
    '                                Case "IMGTEXT"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGTEXTMULTI")
    '                            End Select
    '                        Else
    '                            UPGRADE_ISSUE: (2064) ComctlLib.IListItem property lvwDocList.SelectedItem.ImageKey was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            Select Case lvwDocList.SelectedItems(0).ImageKey
    '                                Case "IMGCLOSEDFOLDER"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGCLOSEDFOLDER")
    '                                Case "IMGRTF"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGRTF")
    '                                Case "IMGTIFF"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGTIFF")
    '                                Case "IMGTEXT"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGTEXT")
    '                            End Select

    '                        End If


    '                    Case "tvwMain"
    '                        If bCopy Then
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGCLOSEDFOLDERMULTI")
    '                        Else
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGCLOSEDFOLDER")
    '                        End If

    '                    Case Else
    '                        UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                        cntSource.DragIcon = imgMain.Images.Item("IMGNODROP")

    '                End Select

    '            Case "lvwDocList"

    '                Select Case cntSource.Name
    '                    Case "lvwDocList"
    '                        If bCopy Then
    '                            UPGRADE_ISSUE: (2064) ComctlLib.IListItem property lvwDocList.SelectedItem.ImageKey was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            Select Case lvwDocList.SelectedItems(0).ImageKey
    '                                Case "IMGCLOSEDFOLDER"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGCLOSEDFOLDERMULTI")
    '                                Case "IMGRTF"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGRTFMULTI")
    '                                Case "IMGTIFF"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGTIFFMULTI")
    '                                Case "IMGTEXT"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGTEXTMULTI")
    '                            End Select
    '                        Else
    '                            UPGRADE_ISSUE: (2064) ComctlLib.IListItem property lvwDocList.SelectedItem.ImageKey was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            Select Case lvwDocList.SelectedItems(0).ImageKey
    '                                Case "IMGCLOSEDFOLDER"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGCLOSEDFOLDER")
    '                                Case "IMGRTF"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGRTF")
    '                                Case "IMGTIFF"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGTIFF")
    '                                Case "IMGTEXT"
    '                                    UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                    cntSource.DragIcon = imgMain.Images.Item("IMGTEXT")
    '                            End Select

    '                        End If


    '                        Cant drop to a document
    '                        UPGRADE_TODO: (1067) Member HitTest is not defined in type VB.Control. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
    '                        If Not (cntSource.HitTest(X, Y) Is Nothing) Then

    '                            UPGRADE_TODO: (1067) Member HitTest is not defined in type VB.Control. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
    '                            If cntSource.HitTest(X, Y).Key.Substring(0, 1) = ACDocument Then
    '                                UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                cntSource.DragIcon = imgMain.Images.Item("IMGNODROP")

    '                            End If
    '                        End If

    '                    Case "tvwMain"

    '                        If bCopy Then
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGCLOSEDFOLDERMULTI")
    '                        Else
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGCLOSEDFOLDER")
    '                        End If

    '                        Cant drop to a document
    '                        UPGRADE_TODO: (1067) Member HitTest is not defined in type VB.Control. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
    '                        If Not (cntTarget.HitTest(X, Y) Is Nothing) Then

    '                            UPGRADE_TODO: (1067) Member HitTest is not defined in type VB.Control. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
    '                            If cntTarget.HitTest(X, Y).Key.Substring(0, 1) = ACDocument Then
    '                                UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                                cntSource.DragIcon = imgMain.Images.Item("IMGNODROP")

    '                            End If
    '                        End If

    '                    Case Else
    '                        UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                        cntSource.DragIcon = imgMain.Images.Item("IMGNODROP")

    '                End Select
    '            Case "lvwBCDocs"
    '                If bCopy Then
    '                    UPGRADE_ISSUE: (2064) ComctlLib.IListItem property lvwDocList.SelectedItem.ImageKey was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                    Select Case lvwDocList.SelectedItems(0).ImageKey
    '                        Case "IMGCLOSEFOLDER"
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGCLOSEDFOLDERMULTI")
    '                        Case "IMGRTF"
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGRTFMULTI")
    '                        Case "IMGTIFF"
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGTIFFMULTI")
    '                        Case "IMGTEXT"
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGTEXTMULTI")
    '                    End Select
    '                Else
    '                    UPGRADE_ISSUE: (2064) ComctlLib.IListItem property lvwDocList.SelectedItem.ImageKey was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                    Select Case lvwDocList.SelectedItems(0).ImageKey
    '                        Case "IMGCLOSEFOLDER"
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGCLOSEDFOLDER")
    '                        Case "IMGRTF"
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGRTF")
    '                        Case "IMGTIFF"
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGTIFF")
    '                        Case "IMGTEXT"
    '                            UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                            cntSource.DragIcon = imgMain.Images.Item("IMGTEXT")
    '                    End Select
    '                End If
    '            Case Else
    '                No other other controls accept a drag
    '                UPGRADE_ISSUE: (2064) Control property cntSource.DragIcon was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
    '                cntSource.DragIcon = imgMain.Images.Item("IMGNODROP")

    '        End Select

    '    Catch excep As System.Exception




    '        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText & ": " & cntSource.Name & " dragged over " & cntSource.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="DragOverCheck", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

    '        Exit Sub

    '    End Try

    'End Sub
    ' ***************************************************************** '
    ' Name: CheckDestinationValid
    '
    ' Description: This section is performed when nodes are being moved.
    ' It checks the destination folder is not the same as the source and
    ' that you are not moving a folder to one of its subfolders.
    '
    ' ***************************************************************** '
    Private Function CheckDestinationValid(ByRef sDestNode As DOCConst.DOCNodes, ByRef sSourceNodes() As DOCConst.DOCNodes) As Integer

        Dim result As Integer = 0
        Dim lParentNum, lFolderNum As Integer
        Dim lParentArray() As Integer
        Dim lTmpParentNum As Integer

        'sp todo - note - briefcase destination will need special treatment (or noe at all, as we move from one structure
        'to a completeley different one.
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'we first need to build an array of the destination folders' ancestors
            ReDim lParentArray(0)

            m_lReturn = ExtractNumFromKey(sDestNode.Key, lFolderNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return False
            End If

            lParentArray(0) = lFolderNum

            'Get folders parent

            m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, lParentNum:=lParentNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            While lParentNum <> 0

                ReDim Preserve lParentArray(lParentArray.GetUpperBound(0) + 1)

                lParentArray(lParentArray.GetUpperBound(0)) = lParentNum

                'Get folders parent

                m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lParentNum, lParentNum:=lTmpParentNum)

                lParentNum = lTmpParentNum

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Business Failed.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End While


            'Now, for each source folder being moved, check it is not the same
            'as the destination, or any of the destinations ancestors
            For i As Integer = sSourceNodes.GetLowerBound(0) To sSourceNodes.GetUpperBound(0)

                'Only need to check for folders
                If sSourceNodes(i).Key.Substring(0, 1) <> ACDocument Then

                    m_lReturn = ExtractNumFromKey(sSourceNodes(i).Key, lFolderNum)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return False
                    End If

                    'Go thru array of ancestors
                    For j As Integer = lParentArray.GetLowerBound(0) To lParentArray.GetUpperBound(0)

                        If lParentArray(j) = lFolderNum Then

                            MessageBox.Show("Cannot move a folder to itself, or one of its subfolders", "Document Manager")
                            Return False
                        End If

                    Next j
                End If

            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDestinationValid", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateViews
    '
    ' Description: After moving or copying nodes, this procedure is
    ' passed the current treeview and listview. It then updates the
    ' treeview with the changed nodes, and if the source is a listview
    ' then it removes the nodes from that if they were not copied.
    '
    ' ***************************************************************** '
    Private Function UpdateViews(ByRef tvw As TreeView, ByRef lvw As ListView, ByRef bCopy As Boolean, ByRef sDestNode As DOCConst.DOCNodes, ByRef sSourceNodes() As DOCConst.DOCNodes) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lFoldNum As Integer


        On Error GoTo Err_UpdateViews

        result = gPMConstants.PMEReturnCode.PMTrue

        If Not bCopy Then

            'If moving, remove the nodes from the 2 views - however they may not
            'exist (quite feasibly) so ignore errors
            On Error Resume Next

            For Each sSourceNodes_item As DOCConst.DOCNodes In sSourceNodes
                lvw.Items.RemoveByKey(sSourceNodes_item.Key)
                'lvw.Items.RemoveAt(CInt(sSourceNodes_item.Key) - 1)
                'tvw.Nodes.RemoveAt(CInt(sSourceNodes_item.Key) - 1)
                tvw.Nodes.RemoveByKey(sSourceNodes_item.Key)

            Next sSourceNodes_item

        End If

        'Get the folder num from the selected node key
        m_lReturn = ExtractNumFromKey(sDestNode.Key, lFoldNum)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'We now need to amend the treeview by adding the new nodes to the
        'destination.

        'First we need to check if the destination node has any children.
        On Error GoTo Err_ExitFunction

        'If this test errors, then the destination node does not exist in the
        'tree, so we dont need to populate it
        'If tvw.Nodes.Item(sDestNode.Key).GetNodeCount(False) > 0 Then
        If tvw.Nodes.Find(sDestNode.Key, True)(0).GetNodeCount(False) > 0 Then

            On Error GoTo Err_UpdateViews
            'children exist, so remove them and repopulate node with fresh data

            'm_lReturn = RemoveTreeChildren(tvw, tvw.Nodes.Item(sDestNode.Key))
            m_lReturn = RemoveTreeChildren(tvw, tvw.Nodes.Find(sDestNode.Key, True)(0))

            'Get the folders in the selected folder
            m_lReturn = GetFolderList(lFoldNum, "", m_vFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'populate the node
            'm_lReturn = PopulateTreeChildren(tvw, tvw.Nodes.Item(sDestNode.Key).Index, m_vFolderArray)
            m_lReturn = PopulateTreeChildren(tvw, tvw.Nodes.Find(sDestNode.Key, True)(0).Index, m_vFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If


        'Finally, we check if the listview currently contains the contents of the destination
        'node, in which case it should be repopulated
        If Convert.ToString(lblTitleMain(1).Tag) = sDestNode.Key Then

            NodeClick(tvw, lvw, sDestNode.Key, "")

        End If

        'Swap icons of newly opened folder and last open folder
        If m_sMainLastOpenFolder <> "" Then
            'tvw.Nodes.Item(m_sMainLastOpenFolder).ImageKey = "IMGCLOSEDFOLDER"
            tvw.Nodes.Find(m_sMainLastOpenFolder, True)(0).ImageKey = "IMGCLOSEDFOLDER"
        End If

        If tvw.Nodes.Find(sDestNode.Key, True).Length > 0 Then
            tvw.SelectedNode = tvw.Nodes.Find(sDestNode.Key, True)(0)
            tvw.SelectedNode.ImageKey = "IMGOPENFOLDER"
            m_sMainLastOpenFolder = tvw.SelectedNode.Name

            'update the label
            lblTitleMain(1).Text = "Contents of '" & tvw.SelectedNode.Text & "'"
            lblTitleMain(1).Tag = tvw.SelectedNode.Name
        End If


        Return result

Err_ExitFunction:

        Return result

Err_UpdateViews:

        result = gPMConstants.PMEReturnCode.PMError

        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Failed to update the current views. Please refresh your data.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateViews", excep:=New Exception(Information.Err().Description))

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: UpdateRootViews
    '
    ' Description: After moving or copying nodes to the root, this
    ' procedure is passed the current treeview and listview. It then
    ' updates the treeview with the changed nodes, and if the source is
    ' a listview then it removes the nodes from that if they were not copied.
    '
    ' ***************************************************************** '
    Private Function UpdateRootViews(ByRef tvw As TreeView, ByRef lvw As ListView, ByRef bCopy As Boolean, ByRef sSourceNodes() As DOCConst.DOCNodes) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Dim vArray(,) As Object
        Dim sKey As String = ""
        Dim Node As TreeNode



        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_UpdateRootViews)")

        result = gPMConstants.PMEReturnCode.PMTrue

        If Not bCopy Then

            'If moving, remove the nodes from the 2 views - however they may not
            'exist (quite feasibly) so ignore errors, also add them in as root nodes

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Resume Next")

            For Each sSourceNodes_item As DOCConst.DOCNodes In sSourceNodes

                lvw.Items.RemoveAt(CInt(sSourceNodes_item.Key) - 1)
                tvw.Nodes.RemoveAt(CInt(sSourceNodes_item.Key) - 1)
                Node = tvw.Nodes.Add(sSourceNodes_item.Key, sSourceNodes_item.Text.Trim(), "IMGCLOSEDFOLDER")

            Next sSourceNodes_item

        End If

        If bCopy Then

            'if copying, we need to get root folder list and add in again, ignoring '
            'errors as thes will be root folders tahtare already present

            'Get the folders in the selected folder
            m_lReturn = GetFolderList(0, "", vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Information.IsArray(vArray) Then

                'loop round adding each folder
                For i As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    'Construct node key
                    If CStr(vArray(2, i)).Trim() <> "" Then
                        'ie passworded
                        sKey = ACFolder & ACPassword & CStr(CInt(CDate(vArray(3, i)).ToOADate)) & CStr(vArray(0, i))
                    Else
                        sKey = ACFolder & " " & CStr(CInt(CDate(vArray(3, i)).ToOADate)) & CStr(vArray(0, i))
                    End If

                    'Add the node
                    Try
                        Node = tvw.Nodes.Add(sKey, CStr(vArray(1, i)).Trim(), "IMGCLOSEDFOLDER")

                    Catch
                    End Try

                    'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_UpdateRootViews)")

                Next i
            End If

        End If


        'Swap icons of newly opened folder and last open folder
        If m_sMainLastOpenFolder <> "" Then
            'tvw.Nodes.Item(m_sMainLastOpenFolder).ImageKey = "IMGCLOSEDFOLDER"
            tvw.Nodes.Find(m_sMainLastOpenFolder, True)(0).ImageKey = "IMGCLOSEDFOLDER"
        End If


        tvw.SelectedNode.ImageKey = "IMGOPENFOLDER"
        m_sMainLastOpenFolder = tvw.SelectedNode.Name

        'update the label
        lblTitleMain(1).Text = "Contents of '" & tvw.SelectedNode.Text & "'"
        lblTitleMain(1).Tag = tvw.SelectedNode.Name


        Return result



        Return result

Err_UpdateRootViews:

        result = gPMConstants.PMEReturnCode.PMError

        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Failed to update the current views. Please refresh your data.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRootViews", excep:=New Exception(Information.Err().Description))

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: StoreSelectedNodes
    '
    ' Description: This section goes thru a listview and stores the key of
    ' each selected node.
    '
    ' ***************************************************************** '
    Private Function StoreSelectedNodes(ByRef sNodeKeys() As DOCConst.DOCNodes, ByRef lvw As ListView) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim bFirstElement As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Initialise the array where you store the selected nodes
            ReDim sNodeKeys(0)

            'Add each selected nodes key to array
            bFirstElement = True
            For i As Integer = 0 To lvw.Items.Count - 1
                If lvw.Items.Item(i).Selected Then
                    If bFirstElement Then
                        bFirstElement = False
                    Else
                        ReDim Preserve sNodeKeys(sNodeKeys.GetUpperBound(0) + 1)
                    End If

                    sNodeKeys(sNodeKeys.GetUpperBound(0)).Key = lvw.Items.Item(i).Name
                    sNodeKeys(sNodeKeys.GetUpperBound(0)).Text = lvw.Items.Item(i).Text

                End If

            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="StoreSelectedNodes", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GhostSelectedNodes
    '
    ' Description: This section goes thru a listview and sets the ghosted
    ' property of each selected list view item to true.
    '
    ' ***************************************************************** '
    Private Sub GhostSelectedNodes(ByRef sNodeKeys() As DOCConst.DOCNodes, ByRef lvw As ListView)



        Try

            'ghost each selected nodes
            For i As Integer = 1 To lvw.Items.Count




                ' lvw.ListItems(i).Ghosted = (lvw.Items.Item(i - 1).Selected)


            Next i

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="GhostSelectedNodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: MoveNodes
    '
    ' Description: This procedure confirms a node move, checks passwords,
    ' external codes etc and calls the business to actually do the moves.
    '
    ' ***************************************************************** '
    Private Function MoveNodes(ByRef sDestNode As DOCConst.DOCNodes, ByRef sSourceNodes() As DOCConst.DOCNodes, Optional ByRef vPastedDocs(,) As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim iTmp As Integer
        Dim lDestFolderNum As Integer
        Dim vFolderArray(,) As Object
        Dim vDocArray(,) As Object
        Dim bExternal, bV2Folder, bSplash As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            ' ND 081100 - Check if user has permission to perform move
            If Not g_bUserIsAdministrator Then

                If sSourceNodes(0).Key.Substring(0, 1) = "F" Then

                    ' check folder move access
                    If g_iAccessLevel > g_iFolderMoveLevel Then
                        MessageBox.Show("You're current access level does not permit folder moving", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else

                    ' check document move access
                    If g_iAccessLevel > g_iFileMoveLevel Then
                        MessageBox.Show("You're current access level does not permit file moving", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            End If

            'Ensure source and destination are compatible for a move
            m_lReturn = CheckDestinationValid(sDestNode, sSourceNodes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get the node numbers of nodes being moved
            m_lReturn = GetNodeNumsFromKeys(sSourceNodes, vFolderArray, vDocArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Get the number of the destination node
            m_lReturn = ExtractNumFromKey(sDestNode.Key, lDestFolderNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get user to confirm
            iTmp = sSourceNodes.GetUpperBound(0) - sSourceNodes.GetLowerBound(0) + 1

            If iTmp = 1 Then
                'confirm single delete
                If Not m_bArchive Then  'MS 09/05/01
                    If m_bIsTopLevelFolder Then
                        m_lReturn = MessageBox.Show("You can't move top level folders back. Are you SURE you wish to move '" & _
                                    sSourceNodes(0).Text.Trim() & "' to '" & _
                                    sDestNode.Text.Trim() & "' ?", "Confirm Move", MessageBoxButtons.YesNo)
                    Else
                        m_lReturn = MessageBox.Show("Are you SURE you wish to move '" & _
                                    sSourceNodes(0).Text.Trim() & "' to '" & _
                                    sDestNode.Text.Trim() & "' ?", "Confirm Move", MessageBoxButtons.YesNo)
                    End If

                    If m_lReturn = System.Windows.Forms.DialogResult.No Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Else
                If Not m_bArchive Then  'MS 09/05/01
                    'confirm multi delete
                    m_lReturn = MessageBox.Show("Are you SURE you wish to move these " & _
                                iTmp & " items to '" & _
                                sDestNode.Text & "' ", "Confirm Move", MessageBoxButtons.YesNo)

                    If m_lReturn = System.Windows.Forms.DialogResult.No Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            'Check if any external folders, if so you cannot move

            If (Not (vFolderArray Is Nothing)) And (Not m_bArchive) Then  'MS 09/05/01Then

                For i As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)


                    m_lReturn = g_oBusiness.AmIExternal(iNodeType:=DOCNode_Folder, lNodeNum:=CInt(vFolderArray(0, i)), bExternal:=bExternal)


                    If bExternal Then

                        MessageBox.Show("The folder(s) you are attempting to move is/are external." & _
                                        Strings.Chr(10).ToString() & "They are linked to an external application and cannot be moved.", "Folder Move")
                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                Next i

            End If

            'Warn if warn option set and folder is non V2Folder
            If (m_bWarnMoveToNonFolder) And (Information.IsArray(vDocArray)) Then


                m_lReturn = g_oBusiness.AmIV2Folder(lFolderNum:=lDestFolderNum, bV2Folder:=bV2Folder)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'business will have logged this
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not bV2Folder Then
                    If Not m_bArchive Then  'MS 09/05/01
                        m_lReturn = MessageBox.Show("'" & sDestNode.Text.Trim() & "' is not a" & _
                                    " DocuMaster Version 2 Folder. " & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() & "Are " & _
                                    "you SURE you wish to move documents to it?", "Confirm Move", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)

                        If m_lReturn = System.Windows.Forms.DialogResult.No Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If

            End If

            'check the passwords of all the nodes - unless you are adminstrator
            If Not g_bUserIsAdministrator Then
                m_lReturn = VerifyPasswords(sSourceNodes)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'if moving more than 4, pop splash
            If iTmp > 4 Then
                bSplash = True

                m_lReturn = g_oSplash.Show(DOCSplash_Moving)
            End If


            'Now call appropiate business to do work.
            m_lReturn = CheckDocumentsBeforeMove()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'user could n't be bothered to supply the password - or got it wrong
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vFolderArray) Then

                'move the list of folders to new destination

                m_lReturn = g_oBusiness.MoveFolders(lDestFolder:=lDestFolderNum, vFolderArray:=vFolderArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    'hide splash
                    If bSplash Then

                        m_lReturn = g_oSplash.Hide()
                    End If

                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to move folders. Transaction abandoned.", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveNodes", excep:=New Exception(Information.Err().Description))

                    Return result
                End If

            End If

            If Information.IsArray(vDocArray) Then

                'move the list of documents to new destination

                m_lReturn = g_oBusiness.MoveDocs(lDestFolder:=lDestFolderNum, vDocArray:=vDocArray, vPastedDocs:=vPastedDocs)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    'hide splash
                    If bSplash Then

                        m_lReturn = g_oSplash.Hide()
                    End If

                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to move documents. Transaction abandoned.", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveNodes", excep:=New Exception(Information.Err().Description))

                    Return result
                End If

            End If

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="MoveNodes", excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: MoveNodesToRoot
    '
    ' Description: This procedure confirms a node move, checks passwords,
    ' external codes etc and calls the business to actually do the moves.
    ' It is hoever only for when we move nodes to the root
    '
    ' ***************************************************************** '
    Private Function MoveNodesToRoot(ByRef sSourceNodes() As DOCConst.DOCNodes) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim iTmp As Integer

        Dim vFolderArray(,) As Object
        Dim vDocArray(,) As Object
        Dim bExternal As Boolean
        Dim lParentNum As Integer
        Dim bSplash As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' ND 081100 - Check if user has permission to perform move
            If Not g_bUserIsAdministrator Then
                If sSourceNodes(0).Key.Substring(0, 1) = "F" Then

                    ' check folder move access
                    If g_iAccessLevel > g_iFolderMoveLevel Then
                        MessageBox.Show("You're current access level does not permit folder moving", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If

                Else

                    ' check document move access
                    If g_iAccessLevel > g_iFileMoveLevel Then
                        MessageBox.Show("You're current access level does not permit file moving", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If

                End If
            End If

            'get the node numbers of nodes being moved
            m_lReturn = GetNodeNumsFromKeys(sSourceNodes, vFolderArray, vDocArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'check if any docs to be move as can only move these to a folder
            If Information.IsArray(vDocArray) Then

                MessageBox.Show("Documents can only be moved to another folder." & Strings.Chr(10).ToString() & _
                                "Transaction abandoned.", "Folder Move")
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'ensure we are not moving a root folder, as this is already in the root

            'Get folders parent

            m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=CInt(vFolderArray(0, 0)), lParentNum:=lParentNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'check it is not the root
            If lParentNum = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get user to confirm
            iTmp = sSourceNodes.GetUpperBound(0) - sSourceNodes.GetLowerBound(0) + 1

            If iTmp = 1 Then
                'confirm single delete
                m_lReturn = MessageBox.Show("Are you SURE you wish to move '" & _
                            sSourceNodes(0).Text & "' to the DocuMaster Root ?", "Confirm Move", MessageBoxButtons.YesNo)

                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                'confirm multi delete
                m_lReturn = MessageBox.Show("Are you SURE you wish to move these " & _
                            iTmp & " folders to the DocuMaster root ?", "Confirm Move", MessageBoxButtons.YesNo)

                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'Check if any external folders, if so you cannot move

            If Not (vFolderArray Is Nothing) Then

                For i As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)


                    m_lReturn = g_oBusiness.AmIExternal(iNodeType:=DOCNode_Folder, lNodeNum:=CInt(vFolderArray(0, i)), bExternal:=bExternal)

                    If bExternal Then

                        MessageBox.Show("The folder(s) you are attempting to move is/are external." & _
                                        Strings.Chr(10).ToString() & "They are linked to an external application and cannot be moved.", "Folder Move")
                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                Next i

            End If


            'check the passwords of all the nodes - unless you are adminstrator
            If Not g_bUserIsAdministrator Then
                m_lReturn = VerifyPasswords(sSourceNodes)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'if copying more than 2, pop splash
            If iTmp > 2 Then
                bSplash = True

                m_lReturn = g_oSplash.Show(DOCSplash_Moving)
            End If

            'Now call appropiate business to do work.

            If Information.IsArray(vFolderArray) Then

                'move the list of folders to new destination

                m_lReturn = g_oBusiness.MoveFolders(lDestFolder:=0, vFolderArray:=vFolderArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    'hide splash
                    If bSplash Then

                        m_lReturn = g_oSplash.Hide()
                    End If

                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to move folders. Transaction abandoned.", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveNodesToRoot", excep:=New Exception(Information.Err().Description))

                    Return result
                End If

            End If

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="MoveNodesToRoot", excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyNodes
    '
    ' Description: This procedure confirms a node copy, checks passwords,
    ' external codes etc and calls the business to actually do the moves.
    '
    ' ***************************************************************** '
    Private Function CopyNodes(ByRef sDestNode As DOCConst.DOCNodes, ByRef sSourceNodes() As DOCConst.DOCNodes, Optional ByRef vPastedDocs(,) As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim iTmp As Integer
        Dim lDestFolderNum As Integer
        Dim vFolderArray(,) As Object
        Dim vDocArray(,) As Object
        Dim bV2Folder, bSplash As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' ND 081100 - Check if user has permission to perform copy
            If Not g_bUserIsAdministrator Then
                If sSourceNodes(0).Key.Substring(0, 1) = "F" Then

                    ' check folder copy access
                    If g_iAccessLevel > g_iFolderCopyLevel Then
                        MessageBox.Show("You're current access level does not permit folder copying", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Return result
                    End If

                Else

                    ' check document copy access
                    If g_iAccessLevel > g_iFileCopyLevel Then
                        MessageBox.Show("You're current access level does not permit file copying", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Return result
                    End If

                End If
            End If

            'Ensure source and destination are compatible for a move
            m_lReturn = CheckDestinationValid(sDestNode, sSourceNodes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Return result
            End If

            'get the node numbers of nodes being moved
            m_lReturn = GetNodeNumsFromKeys(sSourceNodes, vFolderArray, vDocArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Get the number of the destination node
            m_lReturn = ExtractNumFromKey(sDestNode.Key, lDestFolderNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'get user to confirm
            iTmp = sSourceNodes.GetUpperBound(0) - sSourceNodes.GetLowerBound(0) + 1

            If iTmp = 1 Then
                'confirm single delete
                m_lReturn = MessageBox.Show("Are you SURE you wish to copy '" & _
                            sSourceNodes(0).Text & "' to '" & _
                            sDestNode.Text & "' ?", "Confirm Copy", MessageBoxButtons.YesNo)

                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                'confirm multi delete
                m_lReturn = MessageBox.Show("Are you SURE you wish to copy these " & _
                            iTmp & " items to '" & _
                            sDestNode.Text & "' ", "Confirm Copy", MessageBoxButtons.YesNo)

                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'Warn if warn option set and folder is non V2Folder
            If (m_bWarnMoveToNonFolder) And (Information.IsArray(vDocArray)) Then


                m_lReturn = g_oBusiness.AmIV2Folder(lFolderNum:=lDestFolderNum, bV2Folder:=bV2Folder)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'business will have logged this
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not bV2Folder Then
                    m_lReturn = MessageBox.Show("'" & sDestNode.Text.Trim() & "' is not a" & _
                                " DocuMaster Version 2 Folder. " & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() & "Are " & _
                                "you SURE you wish to copy documents to it?", "Confirm Copy", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)

                    If m_lReturn = System.Windows.Forms.DialogResult.No Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If


            'check the passwords of all the nodes - unless you are adminstrator
            If Not g_bUserIsAdministrator Then
                m_lReturn = VerifyPasswords(sSourceNodes)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user couldn't even be bothered to supply the password (or got it wrong)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'if copying more than 2 nodes, or any number of folder nodes,
            'pop up splash
            If (iTmp > 2) Or (Information.IsArray(vFolderArray)) Then
                bSplash = True

                m_lReturn = g_oSplash.Show(DOCSplash_Copying)
            End If

            'Now call appropiate business to do work.

            If Information.IsArray(vFolderArray) Then

                'copy the list of folders to new destination

                m_lReturn = g_oBusiness.CopyFolders(lDestFolder:=lDestFolderNum, vFolderArray:=vFolderArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    If bSplash Then

                        m_lReturn = g_oSplash.Hide()
                    End If

                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to copy folders. Transaction abandoned.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyNodes", excep:=New Exception(Information.Err().Description))

                    Return result
                End If

            End If

            If Information.IsArray(vDocArray) Then

                'copy the list of documents to new destination

                m_lReturn = g_oBusiness.CopyDocs(lDestFolder:=lDestFolderNum, vDocArray:=vDocArray, vPastedDocs:=vPastedDocs)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    If bSplash Then

                        m_lReturn = g_oSplash.Hide()
                    End If

                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to copy documents. Transaction abandoned.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyNodes", excep:=New Exception(Information.Err().Description))

                    Return result
                End If

            End If

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyNodes", excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyNodesToRoot
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CopyNodesToRoot(ByRef sSourceNodes() As DOCConst.DOCNodes) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim iTmp As Integer
        Dim vFolderArray(,) As Object
        Dim vDocArray(,) As Object
        Dim bSplash As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' ND 081100 - Check if user has permission to perform copy
            If Not g_bUserIsAdministrator Then
                If sSourceNodes(0).Key.Substring(0, 1) = "F" Then

                    ' check folder copy access
                    If g_iAccessLevel > g_iFolderCopyLevel Then
                        MessageBox.Show("You're current access level does not permit folder copying", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Return result
                    End If

                Else

                    ' check document copy access
                    If g_iAccessLevel > g_iFileCopyLevel Then
                        MessageBox.Show("You're current access level does not permit file copying", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Return result
                    End If

                End If
            End If

            'get the node numbers of nodes being moved
            m_lReturn = GetNodeNumsFromKeys(sSourceNodes, vFolderArray, vDocArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'check if any docs to be move as can only move these to a folder
            If Information.IsArray(vDocArray) Then

                MessageBox.Show("Documents can only be copied to another folder." & Strings.Chr(10).ToString() & _
                                "Transaction abandoned.", "Folder Copy")
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'get user to confirm
            iTmp = sSourceNodes.GetUpperBound(0) - sSourceNodes.GetLowerBound(0) + 1

            If iTmp = 1 Then
                'confirm single delete
                m_lReturn = MessageBox.Show("Are you SURE you wish to copy '" & _
                            sSourceNodes(0).Text & "' to the DocuMaster root ?", "Confirm Copy", MessageBoxButtons.YesNo)

                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                'confirm multi delete
                m_lReturn = MessageBox.Show("Are you SURE you wish to copy these " & _
                            iTmp & " folders to the DocuMaster root ?", "Confirm Copy", MessageBoxButtons.YesNo)

                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'check the passwords of all the nodes - unless you are adminstrator
            If Not g_bUserIsAdministrator Then
                m_lReturn = VerifyPasswords(sSourceNodes)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user couldn't even be bothered to supply the password (or got it wrong)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            'Now call appropiate business to do work.

            If Information.IsArray(vFolderArray) Then

                bSplash = True

                m_lReturn = g_oSplash.Show(DOCSplash_Copying)

                'copy the list of folders to new destination

                m_lReturn = g_oBusiness.CopyFolders(lDestFolder:=0, vFolderArray:=vFolderArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    'hide splash
                    If bSplash Then

                        m_lReturn = g_oSplash.Hide()
                    End If

                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to copy folders. Transaction abandoned.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyNodesToRoot", excep:=New Exception(Information.Err().Description))

                    Return result
                End If

            End If

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyNodesToRoot", excep:=excep)

            Return result


        End Try
    End Function

    Private Function GetNodeNumsFromKeys(ByRef sNodeKeys() As DOCConst.DOCNodes, ByRef vFolderArray(,) As Object, ByRef vDocArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim bFirstDoc, bFirstFolder As Boolean
        Dim lTmpNum As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bFirstDoc = True
            bFirstFolder = True

            'Go thru array of nodes, and add to folder or doc array, accordingly
            For i As Integer = sNodeKeys.GetLowerBound(0) To sNodeKeys.GetUpperBound(0)

                If sNodeKeys(i).Key.Substring(0, 1) = ACDocument Then

                    'ie document node
                    m_lReturn = ExtractNumFromKey(sNodeKeys(i).Key, lTmpNum)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If bFirstDoc Then
                        ReDim vDocArray(0, 0)

                        vDocArray(0, vDocArray.GetUpperBound(1)) = lTmpNum
                        bFirstDoc = False
                    Else
                        ReDim Preserve vDocArray(0, vDocArray.GetUpperBound(1) + 1)

                        vDocArray(0, vDocArray.GetUpperBound(1)) = lTmpNum
                    End If

                Else

                    'ie folder node
                    If sNodeKeys(i).Key.Substring(0, 3) <> "ADD" Then

                        m_lReturn = ExtractNumFromKey(sNodeKeys(i).Key, lTmpNum)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If bFirstFolder Then
                            ReDim vFolderArray(0, 0)

                            vFolderArray(0, vFolderArray.GetUpperBound(1)) = lTmpNum
                            bFirstFolder = False
                        Else
                            ReDim Preserve vFolderArray(0, vFolderArray.GetUpperBound(1) + 1)

                            vFolderArray(0, vFolderArray.GetUpperBound(1)) = lTmpNum
                        End If
                    End If
                End If

            Next i


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeNumsFromKeys", excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CheckAnnotations
    '
    ' Description: This function sees if the annotation view is turned on
    ' and if so proceess accordingly for the supplied document.
    '
    ' ***************************************************************** '
    Private Sub CheckAnnotations(ByRef sKey As String)

        Dim lDocNum As Integer
        Dim vAnnotationArray(,) As Object


        Try

            'First see if we are actually displaying annotations, cause if we are we need
            ' to get them.
            If Not lvwAnnotations.Visible Then
                Exit Sub
            End If

            'Check if document clicked
            If sKey.Substring(0, 1) <> ACDocument Then
                Exit Sub
            End If

            'Get the doc num from the selected node key
            m_lReturn = ExtractNumFromKey(sKey, lDocNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'check tag to see if correct anns already displayed
            If Convert.ToString(lvwAnnotations.Tag) <> "" Then
                If CInt(Convert.ToString(lvwAnnotations.Tag)) = lDocNum Then
                    Exit Sub
                End If
            End If

            'Get the anns for this doc
            m_lReturn = GetAnnotationList(lDocNum, vAnnotationArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Populate the keyword display
            m_lReturn = PopulateAnnotationsListView(lvwAnnotations, vAnnotationArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Tag the control so it knows which document its current contents apply to
            lvwAnnotations.Tag = CStr(lDocNum)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAnnotations", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: CheckKeywords
    '
    ' Description: This function sees if the keyword view is turned on
    ' and if so proceess accordingly for the supplied document.
    '
    ' ***************************************************************** '
    Private Sub CheckKeywords(ByRef sKey As String)

        Dim lDocNum As Integer
        Dim vDocKeywordArray(,) As Object


        Try

            'First see if we are actually displaying keywords, cause if we are we need
            'to get them.
            If lvwKeyWords.Visible Then

                'Check if document clicked
                If sKey.Substring(0, 1) = ACDocument Then

                    'Get the doc num from the selected node key
                    m_lReturn = ExtractNumFromKey(sKey, lDocNum)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    'check tag to see if correct keywords already displayed
                    If Convert.ToString(lvwKeyWords.Tag) <> "" Then
                        If CInt(Convert.ToString(lvwKeyWords.Tag)) = lDocNum Then
                            Exit Sub
                        End If
                    End If

                    'Get the keywords for this doc
                    m_lReturn = GetDocKeywordList(lDocNum, vDocKeywordArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    'Populate the keyword display
                    m_lReturn = PopulateKeywordsListView(lvwKeyWords, vDocKeywordArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    'Tag the control so it knows which document its current keywords apply to
                    lvwKeyWords.Tag = CStr(lDocNum)

                End If

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckKeywords", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: HotKeyAdvance
    '
    ' Description: This procedure goes to the node for the next hotkey
    ' in the hot key array
    '
    '
    ' ***************************************************************** '
    Private Sub HotKeyAdvance()

        Dim iKey As Integer


        Try

            'advance position in hot key array
            iKey = m_iHotKeyPos + 1
            If iKey > m_sHotKey.GetUpperBound(0) Then
                iKey = 0
            End If

            'find next hot key node
            Do Until m_sHotKey(iKey) <> ""

                iKey += 1
                If iKey > m_sHotKey.GetUpperBound(0) Then
                    iKey = 0
                End If

                'No hotkeys set up
                If iKey = m_iHotKeyPos + 1 Then
                    Exit Sub
                End If
            Loop

            'Go to hot key node
            tvwMain.Nodes.Item(m_sHotKey(iKey)).EnsureVisible()

            'tvwMain.Nodes.Item(m_sHotKey(iKey)).Selected = True
            tvwMain.SelectedNode = tvwMain.Nodes.Find(tvwMain.Nodes.Item(m_sHotKey(iKey)).Name, True)(0)

            m_iHotKeyPos = iKey

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="HotKeyAdvance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try




    End Sub

    ' ***************************************************************** '
    ' Name: AdjustMenuItems
    '
    ' Description: This procedure is called to check a control for
    ' which/how many nodes are selected etc and disables menu items
    ' acordingly. It may be affected by the view we are in, too.
    '
    ' ***************************************************************** '
    Private Sub AdjustMenuItems(ByRef cnt As Control)

        Dim iNodesSelected As Integer


        Try

            'count the num of nodes selected
            CountSelectedNodes(cnt, iNodesSelected)

            'Adjust for the main document list view
            If cnt.Name = "lvwDocList" Then


                Select Case iNodesSelected
                    Case 0
                        'Nothing selected, so disable items accordingly
                        mnuFileOpenDocument.Enabled = False
                        mnuFileSaveAs.Enabled = False
                        mnuFileDelete.Enabled = False
                        mnuFileRename.Enabled = False
                        mnuFileInformation.Enabled = False
                        mnuFilePrint.Enabled = False

                        mnuEditCut.Enabled = False
                        mnuEditCopy.Enabled = False

                        mnuToolsPassword.Enabled = False
                        mnuToolsAccess.Enabled = False
                        mnuToolsAddAnn.Enabled = False
                        mnuToolsAddKeyword.Enabled = False

                    Case 1
                        'one item - is it folder or doc ?
                        Select Case lvwDocList.SelectedItems(0).Name.Substring(0, 1)
                            Case ACDocument
                                'doc - everything is permitted

                            Case ACFolder
                                'folder - cant do some stuff
                                mnuFileOpenDocument.Enabled = False

                                mnuFileSaveAs.Enabled = False
                                mnuFileInformation.Enabled = False
                                mnuFilePrint.Enabled = False

                                mnuToolsAddAnn.Enabled = False
                                mnuToolsAddKeyword.Enabled = False

                                '                        'some stuff only want to do when in main mode
                                '                        If (m_iViewMode% <> DOCViewModeMain) Then
                                '                            mnuPopPassword.Enabled = False
                                '                            mnuPopAccess.Enabled = False
                                '                            mnuPopCut.Enabled = False
                                '                            mnuPopCopy.Enabled = False
                                '                            mnuPopDelete.Enabled = False
                                '                        End If

                            Case Else
                                'no, no.
                                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                oDict.Add("cnt", cnt)
                                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Node not folder nor document", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDocList_GotFocus", excep:=New Exception(Information.Err().Description), oDicParms:=oDict)

                        End Select


                    Case Else
                        'loads selected, so disable some stuff
                        mnuFileOpenDocument.Enabled = False
                        mnuFileSaveAs.Enabled = False
                        mnuFileRename.Enabled = False
                        mnuFileInformation.Enabled = False
                        mnuFilePrint.Enabled = False

                        mnuToolsPassword.Enabled = False
                        mnuToolsAccess.Enabled = False
                        mnuToolsAddAnn.Enabled = False
                        mnuToolsAddKeyword.Enabled = False

                End Select

            End If

            'Adjust for the keywords list view
            If cnt.Name = "lvwKeyWords" Then


                Select Case iNodesSelected
                    Case 0
                        'Nothing selected, so disable items accordingly
                        mnuFileDelete.Enabled = False

                    Case Else
                        'at least one selected, so nothing to disable

                End Select

            End If

            'Adjust for the annotations list view
            If cnt.Name = "lvwAnnotations" Then


                Select Case iNodesSelected
                    Case 0
                        'Nothing selected, so disable items accordingly
                        mnuFileDelete.Enabled = False

                    Case Else
                        'at least one selected, so nothing to disable

                End Select

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AdjustMenuItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub
    ' ***************************************************************** '
    ' Name: CountSelectedNodes
    '
    ' Description: This procedure counts the num of currently selected
    ' nodes in a listview - ie 0, 1 or many
    '
    ' ***************************************************************** '
    Private Sub CountSelectedNodes(ByRef cnt As Control, ByRef iNodesSelected As Integer)



        Try

            If TypeOf cnt Is TreeView Then
                iNodesSelected = 1
                Exit Sub
            End If

            'see how many nodes seletected ie, 0, 1 or many
            iNodesSelected = 0


            For i As Integer = 0 To CType(cnt, ListView).Items.Count - 1


                If CType(cnt, ListView).Items(i).Selected Then

                    iNodesSelected += 1
                    If iNodesSelected > 1 Then
                        Exit For
                    End If

                End If

            Next i

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="CountSelectedNodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: UpdateStatusBar
    '
    ' Description: Update the status bar with the number of list view
    ' items selected
    '
    ' ***************************************************************** '
    Private Sub UpdateStatusBar(ByRef lvw As ListView)

        Dim iNodesSelected As Integer


        Try

            'ND 081100 - we don't want to change text if we are selecting a new folder to scan to
            If staContents.Items.Item(0).Text = "Please select a new folder" Then Exit Sub

            iNodesSelected = 0

            For i As Integer = 1 To lvw.Items.Count

                If lvw.Items.Item(i - 1).Selected Then

                    iNodesSelected += 1

                End If

            Next i

            If iNodesSelected > 0 Then
                staContents.Items.Item(0).Text = CStr(iNodesSelected) & " object(s) selected"
            Else
                staContents.Items.Item(0).Text = CStr(lvw.Items.Count) & " object(s)"
            End If
            staContents.Refresh()
        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStatusBar", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: DeleteNodes
    '
    ' Description: This section is performed whenever a users has opted
    ' to delete either folders or documents.
    '
    ' m_cntCurrent contains the control from which the user has opted to
    ' delete, and the items to delete will be the selected ones in that
    ' control
    '
    ' Edit History: JH051198 using this routine to delete nodes
    ' before re-adding in the select folders routines
    '
    ' ***************************************************************** '
    Private Sub DeleteNodes()

        Dim sNodeKeys() As DOCConst.DOCNodes = Nothing
        Dim iTmp As Integer
        Dim vFolderArray(,) As Object
        Dim vDocArray(,) As Object
        Dim bExternal, bFoldersChecked As Boolean
        Dim sNoAccessName As String = ""
        Dim bSplash As Boolean


        Try
            'Store the selected nodes, with their descriptions
            If TypeOf m_cntCurrent Is ListView Then
                m_lReturn = StoreSelectedNodes(sNodeKeys, m_cntCurrent)
            End If

            'If TypeOf m_cntCurrent Is TreeView Then
            If m_cntCurrent.GetType.Name = "TreeView" Then
                ReDim sNodeKeys(0)

                'sNodeKeys(0).Key = m_cntCurrent.FocusedItem.Name
                'sNodeKeys(0).Text = m_cntCurrent.FocusedItem.Text
                sNodeKeys(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                sNodeKeys(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text
            End If

            'Check we actually have some nodes to delete
            If sNodeKeys(0).Key = "" Then
                Exit Sub
            End If

            ' ND 081100 - Check if user has permission to perform Delete
            If Not g_bUserIsAdministrator Then
                If sNodeKeys(0).Key.Substring(0, 1) = "F" Then

                    ' check folder Delete access
                    If g_iAccessLevel > g_iFolderDeleteLevel Then
                        MessageBox.Show("You're current access level does not permit folder deleting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If

                Else

                    ' check document Delete access
                    If g_iAccessLevel > g_iFileDeleteLevel Then
                        MessageBox.Show("You're current access level does not permit file deleting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If

                End If
            End If

            'get the actual node numbers
            m_lReturn = GetNodeNumsFromKeys(sNodeKeys, vFolderArray, vDocArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'get user to confirm
            iTmp = sNodeKeys.GetUpperBound(0) - sNodeKeys.GetLowerBound(0) + 1

            If iTmp = 1 Then
                'confirm single delete
                m_lReturn = MessageBox.Show("Are you SURE you wish to delete '" & _
                            sNodeKeys(0).Text & "' ?", "Confirm Delete", MessageBoxButtons.YesNo)

                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If

            Else
                'confirm multi delete
                m_lReturn = MessageBox.Show("Are you SURE you wish to delete these " & _
                            iTmp & " items ?", "Confirm Delete", MessageBoxButtons.YesNo)

                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If

            End If

            'Check if any external folders

            If Not (vFolderArray Is Nothing) Then

                For i As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)


                    m_lReturn = g_oBusiness.AmIExternal(iNodeType:=DOCNode_Folder, lNodeNum:=CInt(vFolderArray(0, i)), bExternal:=bExternal)

                    If bExternal Then

                        If g_bUserIsAdministrator Then
                            'double check
                            m_lReturn = MessageBox.Show("The folder(s) you are attempting to delete is/are external." & _
                                        Strings.Chr(10).ToString() & "Are you REALLY quite sure you wish to delete them?.", "Folder Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

                            If m_lReturn = System.Windows.Forms.DialogResult.Yes Then

                                bFoldersChecked = True
                                Exit For

                            Else
                                Exit Sub
                            End If
                        Else
                            'only administrator can delete
                            MessageBox.Show("The folder(s) you are attempting to delete is/are external." & _
                                            Strings.Chr(10).ToString() & "Only an administrator can delete these.", "Folder Delete")
                            Exit Sub
                        End If

                    End If

                Next i

            End If

            'Check if any external docs

            If (Not (vDocArray Is Nothing)) And (Not bFoldersChecked) Then

                'check if docs (can just check first one) are extrenal - ie belong
                'to an external folder

                m_lReturn = g_oBusiness.AmIExternal(iNodeType:=DOCNode_Document, lNodeNum:=CInt(vDocArray(0, 0)), bExternal:=bExternal)

                If bExternal Then
                    'double check
                    m_lReturn = MessageBox.Show("The document(s) you are attempting to delete is/are external." & _
                                Strings.Chr(10).ToString() & "Are you REALLY quite sure you wish to delete them?.", "Document Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

                    If m_lReturn = System.Windows.Forms.DialogResult.No Then
                        Exit Sub
                    End If
                End If

            End If


            'check the passwords of all the nodes - unless you are adminstrator
            If Not g_bUserIsAdministrator Then
                m_lReturn = VerifyPasswords(sNodeKeys)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Exit Sub
                End If
            End If

            'if deleting  more than 2 nodes, or any number of folder nodes,
            'pop up splash
            If (iTmp > 2) Or (Information.IsArray(vFolderArray)) Then
                bSplash = True

                m_lReturn = g_oSplash.Show(DOCSplash_Deleting)
            End If


            'now delete the documents if we have any

            If Not (vDocArray Is Nothing) Then


                m_lReturn = g_oBusiness.DeleteDocuments(vDocArray, bExternal, sNoAccessName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'hide splash
                    If bSplash Then

                        m_lReturn = g_oSplash.Hide()
                    End If

                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete documents.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteNodes", excep:=New Exception(Information.Err().Description & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                                                                                                                                                                                 "NOTE: Delete fails if documents are read-only"))


                    Exit Sub

                End If

            End If

            'remove the deleted docs from the listview (if any)
            If TypeOf m_cntCurrent Is ListView Then
                For Each sNodeKeys_item As DOCConst.DOCNodes In sNodeKeys

                    If sNodeKeys_item.Key.Substring(0, 1) = ACDocument Then
                        'm_cntCurrent.Items.RemoveAt(CInt(sNodeKeys_item.Key) - 1)
                        CType(m_cntCurrent, ListView).Items.RemoveByKey(sNodeKeys_item.Key)
                    End If

                Next sNodeKeys_item
            End If

            'now delete the folders if we have any

            If Not (vFolderArray Is Nothing) Then


                m_lReturn = g_oBusiness.DeleteFolders(vFolderArray, sNoAccessName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'hide splash
                    If bSplash Then

                        m_lReturn = g_oSplash.Hide()
                    End If

                    'if no access name returned (ie doc user did not have access for a certain
                    'doc) then inform so.
                    If sNoAccessName <> "" Then

                        MessageBox.Show("You do not have access permissions to delete the child " & _
                                        "folder '" & sNoAccessName & "'." & Strings.Chr(10).ToString() & _
                                        "Delete transaction abandoned.", "Folder Delete")
                    Else

                        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete folders.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteNodes", excep:=New Exception(Information.Err().Description))

                    End If

                    Exit Sub

                End If

            End If

            'Now remove the deleted folders from the control views

            'If source is list view, remove the folder nodes from the controls
            If TypeOf m_cntCurrent Is ListView Then

                'remove folder nodes from listview
                For i As Integer = sNodeKeys.GetLowerBound(0) To sNodeKeys.GetUpperBound(0)

                    If sNodeKeys(i).Key.Substring(0, 1) = ACFolder Then
                        m_cntCurrent.Items.RemoveAt(CInt(sNodeKeys(i).Key) - 1)
                    End If

                Next i

                'remove folder nodes from tree view - note, the node may not actually
                'be in the tree view, which is fine, so ignore errors
                Try



                    For i As Integer = sNodeKeys.GetLowerBound(0) To sNodeKeys.GetUpperBound(0)

                        If sNodeKeys(i).Key.Substring(0, 1) = ACFolder Then
                            tvwMain.Nodes.RemoveAt(CInt(sNodeKeys(i).Key) - 1)
                        End If

                    Next i
                Catch ex As Exception

                End Try


            End If

            'If source is treeview, remove the deleted folder node from the controls
            'If TypeOf m_cntCurrent Is TreeView Then
            If m_cntCurrent.GetType.Name = "TreeView" Then

                'if the folder we are removing has a parent for which the listview is
                'populated, and we are displaying folders in the listview, then node must
                'be deleted from the listview too
                ' (must ignore errors for this as a root node wont have a parent)
                Try


                    If (Convert.ToString(lblTitleMain(1).Tag) = tvwMain.Nodes.Item(sNodeKeys(0).Key).Parent.Name) And (Not m_bDocsOnly) Then
                        lvwDocList.Items.RemoveAt(CInt(sNodeKeys(0).Key) - 1)
                    End If
                Catch ex As Exception

                End Try

                'there will only be one selected node

                If m_cntCurrent.GetType.Name = "TreeView" Then
                    CType(m_cntCurrent, TreeView).Nodes.Remove(CType(m_cntCurrent, TreeView).Nodes.Find(sNodeKeys(0).Key, True)(0))
                End If



                'check if listview currently populated for deleted node, in which case
                'it must be cleared, and repopulated for the new selected folder
                If Convert.ToString(lblTitleMain(1).Tag) = sNodeKeys(0).Key Then

                    lvwDocList.Items.Clear()

                    'we'd best populate for the new selected folder, which by default
                    'will be the next in the tree
                    tvwMain_Click(tvwMain, New EventArgs())


                    tvwMain.SelectedNode.ImageKey = "IMGOPENFOLDER"
                    m_sMainLastOpenFolder = tvwMain.SelectedNode.Name

                    'update the label
                    lblTitleMain(1).Text = "Contents of '" & tvwMain.SelectedNode.Text & "'"
                    lblTitleMain(1).Tag = tvwMain.SelectedNode.Name

                Else

                    'set selected folder back to the one for which the listview is
                    'currentlt populated
                    If Convert.ToString(lblTitleMain(1).Tag) <> "" Then

                        'tvwMain.Nodes.Item(Convert.ToString(lblTitleMain(1).Tag)).Selected = True
                        tvwMain.SelectedNode = tvwMain.Nodes.Find(Convert.ToString(lblTitleMain(1).Tag), True)(0)

                    End If
                End If

            End If

            'remove the deleted nodes from the hot key array - if it's in there that is
            For i As Integer = sNodeKeys.GetLowerBound(0) To sNodeKeys.GetUpperBound(0)

                For j As Integer = m_sHotKey.GetLowerBound(0) To m_sHotKey.GetUpperBound(0)

                    If m_sHotKey(j) = sNodeKeys(i).Key Then

                        m_sHotKey(j) = ""
                        Exit For
                    End If

                Next j

            Next i

            'hide splash
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            Exit Sub

        Catch ex As Exception
            If bSplash Then

                m_lReturn = g_oSplash.Hide()
            End If

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteNodes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: RenameNode
    '
    ' Description: This section is performed whenever a users has opted
    ' to rename either folders or documents.
    '
    ' m_cntCurrent contains the control from which the user has opted to
    ' rename, and the item to rename will be the selected one in that
    ' control
    '
    ' Edit History:
    ' JH051198 cannot rename Add to folders node
    '
    ' JH040199 cannot rename to a blank string DR 4384
    '
    ' ***************************************************************** '


    Private Function RenameNode(ByRef sNewName As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lNodeNum As Integer
        Dim bExternal As Boolean
        Dim sNodeKey() As DOCConst.DOCNodes = Nothing



        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_RenameNode)")

        result = gPMConstants.PMEReturnCode.PMTrue

        'JH051198
        If m_sRenameNode.Key.Substring(0, 3) = "ADD" Then
            If sNewName <> "Add Folders to View..." Then
                'got to rename it back to 'Add Folders to View...'
                MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                'm_sRenameNode.Text = "Add Folders to View..." (don't need this if already cancelling)
            End If
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'SOB150699
        'This format of adding a blank to a trimed string
        'checks for blank and Null
        If sNewName.Trim() & "" = "" Then
            MessageBox.Show("Please Enter a Document or Folder name", "Error Renaming", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            'abort rename
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'get the actual node num
        m_lReturn = ExtractNumFromKey(m_sRenameNode.Key, lNodeNum)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'if node is folder, check if external as these cant be renamed
        If m_sRenameNode.Key.Substring(0, 1) = ACFolder Then


            m_lReturn = g_oBusiness.AmIExternal(iNodeType:=DOCNode_Folder, lNodeNum:=lNodeNum, bExternal:=bExternal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bExternal Then

                'you cannot delete an external folder
                MessageBox.Show("'" & m_sRenameNode.Text & "' is an external folder." & _
                                Strings.Chr(10).ToString() & "These cannot be renamed.", "Folder Rename")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'check the passwords of the node - unless you are adminstrator
        If Not g_bUserIsAdministrator Then

            ReDim sNodeKey(0)
            sNodeKey(0).Key = m_sRenameNode.Key
            sNodeKey(0).Text = m_sRenameNode.Text

            m_lReturn = VerifyPasswords(sNodeKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'user could n't be bothered to supply the password - or got it wrong
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        'now rename the node

        If m_sRenameNode.Key.Substring(0, 1) = ACFolder Then
            'folder rename

            m_lReturn = g_oBusiness.RenameFolder(lNodeNum, sNewName)

        Else
            'document rename

            m_lReturn = g_oBusiness.RenameDoc(lNodeNum, sNewName)

        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Everything was OK, so update the other view
        'ie if we just renamed a node in the list view, then rename the same node in
        'in the tree view, and vice versa. Can ignore errors as the node may well not
        'exit in the other control.

        Try

            If m_cntRename.Name = "lvwDocList" Then
                If Not tvwMain.Nodes.Item(m_sRenameNode.Key) Is Nothing Then
                    tvwMain.Nodes.Item(m_sRenameNode.Key).Text = sNewName
                End If
            Else
                If m_cntRename.Name = "tvwMain" Then
                    If Not lvwDocList.Items.Item(m_sRenameNode.Key) Is Nothing Then
                        lvwDocList.Items.Item(m_sRenameNode.Key).Text = sNewName
                    End If
                End If
            End If

            m_cntRename = Nothing

            Return result


Err_RenameNode:

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="RenameNode", excep:=New Exception(Information.Err().Description))

            Return result

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: VerifyPasswords
    '
    ' Description: Given an array of node keys, this procedure prompts
    ' user to verify the password for any that are passworded
    '
    ' ***************************************************************** '
    Private Function VerifyPasswords(ByRef sNodeKeys() As DOCConst.DOCNodes) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Dim iNodeType As Integer
        Dim lNodeNum As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'see if we alrewady have the password object
            If m_oPassword Is Nothing Then

                'create it
                m_oPassword = New iDOCPassword.Interface_Renamed()


                m_lReturn = m_oPassword.Initialise(False, g_sUserName, "", 0, g_iSourceID, g_iLanguageID, 0, 4, ACApp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="VerifyPasswords", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If


            For i As Integer = sNodeKeys.GetLowerBound(0) To sNodeKeys.GetUpperBound(0)

                'Check if node passworded
                If Not sNodeKeys(i).Key Is Nothing Then
                    If sNodeKeys(i).Key.Substring(1, 1) = ACPassword Then

                        'get node type
                        Select Case sNodeKeys(i).Key.Substring(0, 1)
                            Case ACFolder
                                'folder
                                iNodeType = DOCNode_Folder

                            Case ACDocument
                                'document
                                iNodeType = DOCNode_Document

                            Case Else

                                result = gPMConstants.PMEReturnCode.PMFalse

                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid Node Type", vApp:=ACApp, vClass:=ACClass, vMethod:="VerifyPasswords", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                Return result
                        End Select

                        'get node number
                        m_lReturn = ExtractNumFromKey(sNodeKeys(i).Key, lNodeNum)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'verify password

                        m_lReturn = m_oPassword.VerifyPassword(lNodeNum:=lNodeNum, iNodeLevel:=iNodeType, sNodeName:=sNodeKeys(i).Text)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMOK Then
                            'user either cancelled or error occurred.
                            ' Proceed NO further.
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If
                End If


            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="VerifyPasswords", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NodeClick
    '
    ' Description: This procedure is called with a treeview and listview
    ' and selected node, and preforms the code to populate the views
    ' according to the node clicked.
    '
    ' Edit History: JH071298 folders need to only appear in listview
    ' after select folders has done.
    '
    ' ***************************************************************** '
    Private Sub NodeClick(ByRef tvw As TreeView, ByRef lvw As ListView, ByRef sKey As String, ByRef sFilter As String)

        Dim vTempArray(,) As Object

        Dim lCount, lEndCount, lNodeNum, lFoldNum As Integer

        Dim ChildNode As TreeNode

        Try


            If sKey.Substring(0, 3) = "ADD" Then
                Exit Sub
            End If

            'Get the folder num from the selected node key
            m_lReturn = ExtractNumFromKey(sKey, lFoldNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            If tvw.Nodes.Find(sKey, True).Length > 0 Then


                'If tvw.Nodes.Item(sKey).GetNodeCount(False) > 0 Then
                If tvw.Nodes.Find(sKey, True)(0).GetNodeCount(False) > 0 Then

                    'dont displays folders in listview (ever) if in find results view
                    If (m_bDocsOnly) Or (m_iViewMode = DOCViewModeFindResults) Then

                        m_vFolderArray = Nothing

                    Else
                        'Get the folders in the selected folder
                        'm_lReturn = GetFolderList(lFoldNum, "", m_vFolderArray)

                        'JH071298 Get only the folders that have been
                        'displayed in treeview up to maximum of g_lMaxAutoExpand
                        '(not using MaxFolders as this may be 'All')
                        'so build a new array


                        'ChildNode = tvw.Nodes(sKey).Child.FirstSibling
                        ChildNode = tvw.Nodes.Find(sKey, True)(0).FirstNode
                        If ChildNode.Name.Substring(0, 3) = "ADD" Then
                            ChildNode = ChildNode.NextNode
                            'lEndCount = tvw.Nodes.Item(sKey).GetNodeCount(False) - 2
                            lEndCount = tvw.Nodes.Find(sKey, True)(0).GetNodeCount(False) - 2
                        Else
                            'lEndCount = tvw.Nodes.Item(sKey).GetNodeCount(False) - 1
                            lEndCount = tvw.Nodes.Find(sKey, True)(0).GetNodeCount(False) - 1
                        End If

                        m_lReturn = ExtractNumFromKey(ChildNode.Name, lNodeNum)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            ReDim Preserve vTempArray(3, 0)


                            vTempArray(0, 0) = lNodeNum
                            'name

                            vTempArray(1, 0) = ChildNode.Text
                            'password

                            vTempArray(2, 0) = ChildNode.Name.Substring(ACPasswordStart - 1, Math.Min(ChildNode.Name.Length, ACPasswordLen))
                            'date

                            vTempArray(3, 0) = ChildNode.Name.Substring(ACDateStart - 1, Math.Min(ChildNode.Name.Length, ACDateLen))

                            For lCount = 1 To lEndCount

                                ChildNode = ChildNode.NextNode

                                'insert the bits into the array

                                m_lReturn = ExtractNumFromKey(ChildNode.Name, lNodeNum)
                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    ReDim Preserve vTempArray(3, lCount)


                                    vTempArray(0, lCount) = lNodeNum
                                    'name

                                    vTempArray(1, lCount) = ChildNode.Text
                                    'password

                                    vTempArray(2, lCount) = ChildNode.Name.Substring(ACPasswordStart - 1, Math.Min(ChildNode.Name.Length, ACPasswordLen))
                                    'date

                                    vTempArray(3, lCount) = ChildNode.Name.Substring(ACDateStart - 1, Math.Min(ChildNode.Name.Length, ACDateLen))

                                End If

                                If lCount >= g_lMaxAutoExpand Then
                                    Exit For
                                End If

                            Next lCount
                        End If

                        m_vFolderArray = vTempArray

                    End If

                    'Get the docs in the selected folder
                    m_lReturn = GetDocList(lFoldNum, "", m_vDocArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    'display the child folders and docs on right
                    ' (swap these line to reduce newtork traffic,c.f PopulateListView)

                    m_lReturn = PopulateListView(lvw:=lvw, vFolderArray:=m_vFolderArray, bDetails:=True, bExtraDate:=True, vDocArray:=m_vDocArray)
                    'm_lReturn = PopulateListView(lvw, m_vFolderArray, m_vDocArray, tvw, tvw.Nodes(sKey$))

                Else

                    'JH071298 if child folders more than g_lMaxAutoExpand
                    'then don't display folders until Select Folders done

                    If (m_bDocsOnly) Or (m_iViewMode = DOCViewModeFindResults) Then

                        m_vFolderArray = Nothing
                    Else

                        m_lReturn = CountChildren(lFoldNum, lCount)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If

                        If lCount > g_lMaxAutoExpand Then
                            m_vFolderArray = Nothing
                        Else

                            'Get the folders in the selected folder
                            m_lReturn = GetFolderList(lFoldNum, "", m_vFolderArray)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Exit Sub
                            End If

                        End If

                    End If

                    'Get the folder num from the selected node key
                    '        m_lReturn = ExtractNumFromKey(sKey$, lFoldNum&)
                    '
                    '        If (m_lReturn& <> PMTrue) Then
                    '            Exit Sub
                    '        End If

                    'dont displays folders in listview (ever) if in find results view
                    '        If ((m_bDocsOnly = True) Or _
                    ''            (m_iViewMode% = DOCViewModeFindResults)) Then
                    '
                    '            Set m_vFolderArray = Nothing
                    '        Else
                    '            'Get the folders in the selected folder
                    '            m_lReturn = GetFolderList(lFoldNum, sFilter$, m_vFolderArray)
                    '
                    '            If (m_lReturn& <> PMTrue) Then
                    '                Exit Sub
                    '            End If
                    '
                    '        End If

                    'Get the docs in the selected folder
                    m_lReturn = GetDocList(lFoldNum, "", m_vDocArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    'display the child folders and docs on right

                    m_lReturn = PopulateListView(lvw:=lvw, vFolderArray:=m_vFolderArray, bDetails:=True, bExtraDate:=True, vDocArray:=m_vDocArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                End If
            End If

        Catch excep As System.Exception



            'Log to File
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="NodeClick", excep:=excep)

            Exit Sub

        End Try


    End Sub


    ' ***************************************************************** '
    ' Name: PasteNodes
    '
    ' Description: Move or copy any pasted nodes according to whether
    ' they were cut or copied.
    '
    ' ***************************************************************** '
    Private Sub PasteNodes(Optional ByRef vPastedDocs(,) As Object = Nothing)

        Dim sDestNode As DOCConst.DOCNodes = DOCConst.DOCNodes.CreateInstance()


        Try

            'get the destination node to which we will paste
            If TypeOf m_cntCurrent Is ListView Then
                'if paste to list view, then we are pasting to the node for
                'which the listview is currently populated

                sDestNode.Key = tvwMain.Nodes.Find(Convert.ToString(lblTitleMain(1).Tag), True)(0).Name
                sDestNode.Text = tvwMain.Nodes.Find(Convert.ToString(lblTitleMain(1).Tag), True)(0).Text

            End If

            'If TypeOf m_cntCurrent Is TreeView Then
            If m_cntCurrent.GetType.Name = "TreeView" Then
                'its the treeview
                sDestNode.Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                sDestNode.Text = CType(m_cntCurrent, TreeView).SelectedNode.Text
            End If

            'Paste in the nodes according to whether cut or copy



            Select Case m_iPasteFlag
                Case ACPasteCut

                    'move the nodes
                    m_lReturn = MoveNodes(sDestNode, m_sPasteNodes, vPastedDocs)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    m_lReturn = UpdateViews(tvw:=tvwMain, lvw:=lvwDocList, bCopy:=False, sDestNode:=sDestNode, sSourceNodes:=m_sPasteNodes)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If


                Case ACPasteCopy

                    'copy the nodes
                    m_lReturn = CopyNodes(sDestNode, m_sPasteNodes, vPastedDocs)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    m_lReturn = UpdateViews(tvw:=tvwMain, lvw:=lvwDocList, bCopy:=True, sDestNode:=sDestNode, sSourceNodes:=m_sPasteNodes)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                Case ACPasteEmpty

                    'nothing to paste
                    Exit Sub

                Case Else

                    'surely not ?
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unrecognised paste flag", vApp:=ACApp, vClass:=ACClass, vMethod:="PasteNodes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

            End Select

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PasteNodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PopulateFindResults
    '
    ' Description: Given a list of found documents, this function
    ' gets the folder tree for each and adds it to the find results
    ' tree view.
    '
    ' ***************************************************************** '
    Private Function PopulateFindResults(ByRef vDocArray As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim Node As TreeNode
        Dim sKey, sTmpKey As String
        Dim vFolderArray(,) As Object

        tvwFind.Nodes.Clear()

        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_PopulateFindResults)")

        result = gPMConstants.PMEReturnCode.PMTrue

        'loop thru each found document

        For i As Integer = vDocArray.GetLowerBound(0) To vDocArray.GetUpperBound(0)

            'get the parent tree for this document


            m_lReturn = g_oBusiness.GetFullFolderTree(lNodeNum:=CInt(vDocArray(i)), iNodeType:=DOCNode_Document, vFolderArray:=vFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vFolderArray) Then

                'add root folder for this doc (ie highest in array)

                'Construct node key
                If CStr(vFolderArray(2, vFolderArray.GetUpperBound(1))).Trim() <> "" Then
                    'ie passworded
                    sKey = ACFolder & ACPassword & CStr(CInt(CDate(vFolderArray(3, vFolderArray.GetUpperBound(1))).ToOADate)) & _
                           CStr(vFolderArray(0, vFolderArray.GetUpperBound(1)))
                Else
                    sKey = ACFolder & " " & CStr(CInt(CDate(vFolderArray(3, vFolderArray.GetUpperBound(1))).ToOADate)) & _
                           CStr(vFolderArray(0, vFolderArray.GetUpperBound(1)))
                End If

                'now add the root folder - can ignore errors as may well already exist
                Try
                    If tvwFind.Nodes.Find(sKey, False).Length = 0 Then
                        Node = tvwFind.Nodes.Add(sKey, CStr(vFolderArray(1, vFolderArray.GetUpperBound(1))), "IMGCLOSEDFOLDER")
                    End If



                Catch
                End Try

                'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_PopulateFindResults)")

                'store key so can add its child folder
                sTmpKey = sKey

                'now loop down through the array, adding each remaining child folder in
                'turn - again must ignore errors as folder may already have been added
                For j As Integer = vFolderArray.GetUpperBound(1) - 1 To vFolderArray.GetLowerBound(1) Step -1

                    'add child

                    'Construct node key
                    If CStr(vFolderArray(2, j)).Trim() <> "" Then
                        'ie passworded
                        sKey = ACFolder & ACPassword & CStr(CInt(CDate(vFolderArray(3, j)).ToOADate)) & _
                               CStr(vFolderArray(0, j))
                    Else
                        sKey = ACFolder & " " & CStr(CInt(CDate(vFolderArray(3, j)).ToOADate)) & _
                               CStr(vFolderArray(0, j))
                    End If

                    'now add the child to its parent - again can ignore errors as may well
                    'already exist
                    Try
                        If tvwFind.Nodes.Find(sKey, True).Length = 0 Then
                            Node = tvwFind.Nodes.Find(sTmpKey, True)(0).Nodes.Add(sKey, vFolderArray(1, j), "IMGCLOSEDFOLDER")
                        Else
                            If (tvwFind.Nodes.Find(sTmpKey, True).Length = 0) Then
                                Node = tvwFind.Nodes.Find(sTmpKey, True)(0).Nodes.Add(sKey, vFolderArray(1, j), "IMGCLOSEDFOLDER")
                            End If
                        End If

                    Catch
                    End Try

                    'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_PopulateFindResults)")

                    tvwFind.Nodes.Find(sTmpKey, True)(0).EnsureVisible()

                    sTmpKey = sKey

                Next j

            End If

        Next i

        Return result

Err_PopulateFindResults:

        result = gPMConstants.PMEReturnCode.PMError

        'Log to File
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:=gPMConstants.PMWarningText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateFindResults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Find
    '
    ' Description: Create find object and use it, then fill the results
    ' window.
    '
    ' ***************************************************************** '
    Private Sub Find(ByRef lStartFoldNum As Integer, ByRef sStartFoldName As String)

        Dim vDocArray As Object
        Dim oFind As iDOCFind.Interface_Renamed
        Dim sMinTxt As String = ""


        Try

            'get instance of find object
            oFind = New iDOCFind.Interface_Renamed()

            'initialise it

            m_lReturn = oFind.Initialise(g_iAccessLevel)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise Find object", vApp:=ACApp, vClass:=ACClass, vMethod:="Find", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'call find a doc

            m_lReturn = oFind.Find(lStartFoldNum:=lStartFoldNum, sStartFoldName:=sStartFoldName, vDocsFound:=vDocArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in Find object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Find", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            If Information.IsArray(vDocArray) Then
                'some docs were found !!!

                'if more than 200 docs found advise user how long it will take to display
                '        lNumFound& = UBound(vDocArray) - LBound(vDocArray) + 1
                '
                '        lTmp& = lNumFound& / 30
                '        If (lTmp& = 0) Then
                '            lTmp& = 1
                '            sMinTxt = " minute"
                '        Else
                '            If (lTmp& = 1) Then
                '                sMinTxt = " minute"
                '            Else
                '                sMinTxt = " minutes"
                '            End If
                '        End If

                '        If (lNumFound& >= 10) Then
                '            m_lReturn& = MsgBox(lNumFound& & " Matching documents were found." & Chr(10) & _
                ''                        "It may take up to " & lTmp & sMinTxt & " to display your search " & _
                ''                        "results." & Chr(10) & Chr(10) & _
                ''                        "Do you wish to continue ?", vbYesNo, "Find Document")
                '
                '            If (m_lReturn& = vbNo) Then
                '                Exit Sub
                '            End If
                '
                '        End If

                'switch view to find results
                mnuViewFindResults_Click(mnuViewFindResults, New EventArgs())

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                'fill the find results tree view with find results
                m_lReturn = PopulateFindResults(vDocArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            End If

        Catch excep As System.Exception



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Find", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: AddAnnotation
    '
    ' Description: Adds an annotation to currently selected document.
    '
    ' ***************************************************************** '
    Private Sub AddAnnotation()


        Dim sDocName As String = ""
        Dim lDocNum As Integer
        Dim sAnnText As String = ""
        Dim vAnnotationArray(,) As Object


        Try

            'Capture annotation
            sDocName = m_cntCurrent.FocusedItem.Text.Trim()

            sAnnText = Interaction.InputBox("Please Enter Annotation", "Current Document - '" & _
                       sDocName & "'")

            If sAnnText = "" Then
                Exit Sub
            End If

            'Make sure it isn't too long
            'While sAnnText.Length > 50

            '	sAnnText = Interaction.InputBox("Please Enter Annotation" & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() &  _
            '	           "Maximum 50 characters", "Current Document - '" & sDocName &  _
            '	           "'", sAnnText.Substring(0, 50))

            '	If sAnnText = "" Then
            '		Exit Sub
            '	End If

            'End While

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' get doc num
            m_lReturn = ExtractNumFromKey(m_cntCurrent.FocusedItem.Name, lDocNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If


            'call business to add ann

            m_lReturn = g_oBusiness.AddAnnotation(lDocNum:=lDocNum, sAnnText:=sAnnText)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to add annotation.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAnnotation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'if we are currently displaying anns, refresh display
            If lvwAnnotations.Visible Then

                'Get the anns for this doc
                m_lReturn = GetAnnotationList(lDocNum, vAnnotationArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                'Populate the keyword display
                m_lReturn = PopulateAnnotationsListView(lvwAnnotations, vAnnotationArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AddAnnotation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: AddKeyword
    '
    ' Description: Call keyord admin object to attach keywords then
    ' refresh view.
    '
    ' Edit History:
    '
    ' JH040199 pass isuseradmin variable so non-admin cannot add
    ' and delete default keywords ref DR 4250
    '
    ' ***************************************************************** '
    Private Sub AddKeyword()

        Dim lDocNum As Integer
        Dim vKeywordArray(,) As Object
        Dim vKeywordID As Object


        Try

            'get the keyword object and initialise it if not already done so
            If m_oKeywordAdmin Is Nothing Then

                m_oKeywordAdmin = New iDOCKeywordAdmin.Interface_Renamed()


                'developer guide no.9
                'm_lReturn = CType(m_oKeywordAdmin, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                m_lReturn = m_oKeywordAdmin.Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise iDOCKeywordAdmin.Interface class", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyword", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            ' get doc num
            m_lReturn = ExtractNumFromKey(m_cntCurrent.FocusedItem.Name, lDocNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If

            'JH040199

            m_oKeywordAdmin.UserIsAdministrator = g_bUserIsAdministrator

            'call the attach method for this document

            m_lReturn = m_oKeywordAdmin.AttachKeywords(lDocNum:=lDocNum, vKeywordID:=vKeywordID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'notify user, but refresh list so they know which failed
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to attach all keywords.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyword", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End If

            'if we are currently displaying keywords, refresh display
            If lvwKeyWords.Visible Then

                'Get the anns for this doc
                m_lReturn = GetDocKeywordList(lDocNum, vKeywordArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

                'Populate the keyword display
                m_lReturn = PopulateKeywordsListView(lvwKeyWords, vKeywordArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If

            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyword", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: ViewDocument
    '
    ' Description: Call the Document Viewer
    '
    ' ***************************************************************** '
    Private Sub ViewDocument()

        Dim lDocNum As Integer
        Dim sNodeKeys() As DOCConst.DOCNodes = Nothing
        Dim vPageArray() As Object

        Dim bZipped As Boolean
        Dim sParents, sFilename As String
        'Dim oZipper As New bSIRZipper.Zipper
        Dim bAllowCopyPaste As Boolean
        Dim fs As FileInfo
        Try

            'instance the viewer if not already done so
            'If m_oViewer Is Nothing Then

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_oViewer = New iDOCViewer.Interface_Renamed()

            'initialise and pass instance of myself

            m_lReturn = m_oViewer.Initialise(Me)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'DN 13/12/00 - Don't display error message when cache path not setup
                If m_lReturn = gPMConstants.PMEReturnCode.PMInvalidRequest Then


                    m_oViewer.Dispose()
                    m_oViewer = Nothing
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

                    'Fire up options screen to populate cache directory
                    mnuViewOptions_Click(mnuViewOptions, New EventArgs())

                    Exit Sub

                Else


                    m_oViewer.Dispose()
                    m_oViewer = Nothing
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise viewer", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument")

                    Exit Sub
                End If
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            'End If

            'store selected node
            m_lReturn = StoreSelectedNodes(sNodeKeys, lvwDocList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' RDC 23062005
            m_lReturn = GetAllowCopyPasteOption(bAllowCopyPaste:=bAllowCopyPaste)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'if no nodes selected, returned, just show the viewer
            If sNodeKeys(sNodeKeys.GetLowerBound(0)).Key = "" Then

                'call the doc viewer

                m_lReturn = m_oViewer.ViewDocument(v_sDocumentKey:="", v_sDocumentName:="", v_sParents:="", v_vFileArray:=Nothing, v_bZipped:=False, v_bShowOnly:=True, v_bAllowCopyPaste:=bAllowCopyPaste)

                Exit Sub

            End If

            'check the passwords of all the nodes - unless you are administrator
            If Not g_bUserIsAdministrator Then
                m_lReturn = VerifyPasswords(sNodeKeys)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Exit Sub
                End If
            End If

            'get the parent folders names

            Select Case m_iViewMode
                Case DOCViewModeMain
                    'sFolderName$ = tvwMain.Nodes(lblTitleMain(1).Tag).Text
                    m_lReturn = GetParentNamesFromTree(tvwMain, Convert.ToString(lblTitleMain(1).Tag), sParents)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                Case DOCViewModeFindResults
                    'sFolderName$ = tvwFind.Nodes(lblTitleFind(1).Tag).Text
                    m_lReturn = GetParentNamesFromTree(tvwFind, Convert.ToString(lblTitleFind(1).Tag), sParents)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                Case Else

            End Select

            'Go thru all select docs, calling the viewer for each
            For Each sNodeKeys_item As DOCConst.DOCNodes In sNodeKeys

                If sNodeKeys_item.Key.Substring(0, 1) = ACDocument Then

                    'get the doc num
                    m_lReturn = ExtractNumFromKey(sNodeKeys_item.Key, lDocNum)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    'get the page file paths

                    m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot view '" & sNodeKeys_item.Text & _
                                   "'. Failed to get pages.", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Else
                        ' Get the first filename
                        sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))

                        ' Check to see if zip file...
                        Dim cloudHostingOptionValue As String = ""

                        m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)

                        If Not (gPMFunctions.NullToString(cloudHostingOptionValue) = "1") Then


                            m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZIPFile:=bZipped)
                            fs = New FileInfo(sFilename)
                            If Not fs.Exists Then
                                m_lReturn = gPMConstants.PMEReturnCode.PMError
                            End If
                            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                                MsgBox("File: " & sFilename & " does not exist", vbOKOnly, "ValidZIPFile")
                                Exit Sub
                            End If
                            If Not m_lReturn Then
                                'error - assume unzipped
                                bZipped = False
                            End If
                        Else
                            bZipped = False
                        End If

                        'call the doc viewer

                        m_lReturn = m_oViewer.ViewDocument(v_sDocumentKey:=sNodeKeys_item.Key, v_sDocumentName:=sNodeKeys_item.Text, v_sParents:=sParents, v_vFileArray:=vPageArray, v_bZipped:=bZipped, v_bAllowCopyPaste:=bAllowCopyPaste)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If

                    End If

                End If

            Next sNodeKeys_item

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub
    ' ***************************************************************** '
    ' Name: Scan
    '
    ' Description: Call the Scan Application
    '
    ' ***************************************************************** '
    Private Sub Scan()

        Dim lFoldNum As Integer
        Dim vFolderArray As Object
        Dim bExternal As Boolean


        Try

            'if no nodes then begone
            If TypeOf m_cntCurrent Is ListView Then
                If m_cntCurrent.Items.Count < 1 Then
                    Exit Sub
                End If
            End If

            'ensure we have folder
            If TypeOf (m_cntCurrent) Is TreeView Then
                If CType(m_cntCurrent, TreeView).SelectedNode.Name.Substring(0, 1) <> ACFolder Then
                    Exit Sub
                End If
            End If
            If TypeOf (m_cntCurrent) Is ListView Then
                If CType(m_cntCurrent, ListView).SelectedItems(0).Name <> ACFolder Then
                    Exit Sub
                End If
            End If


            'Store the selected folder
            Dim sNodeKeys() As DOCConst.DOCNodes = ArraysHelper.InitializeArray(Of DOCConst.DOCNodes)(1)
            If TypeOf (m_cntCurrent) Is TreeView Then
                sNodeKeys(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                sNodeKeys(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text

            End If
            If TypeOf (m_cntCurrent) Is ListView Then
                sNodeKeys(0).Key = CType(m_cntCurrent, ListView).SelectedItems(0).Name
                sNodeKeys(0).Text = CType(m_cntCurrent, ListView).SelectedItems(0).Text
            End If
            'get folder number
            m_lReturn = ExtractNumFromKey(sNodeKeys(0).Key, lFoldNum)

            'Warn if warn option set and folder external
            If m_bWarnScanToExternal Then


                m_lReturn = g_oBusiness.AmIExternal(iNodeType:=DOCNode_Folder, lNodeNum:=lFoldNum, bExternal:=bExternal)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'business will ahve logged this
                    Exit Sub
                End If

                If bExternal Then
                    m_lReturn = MessageBox.Show("'" & sNodeKeys(0).Text.Trim() & "' is an " & _
                                "external folder. " & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() & "Are " & _
                                "you SURE you wish to scan directly to it?", "Confirm Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)

                    If m_lReturn = System.Windows.Forms.DialogResult.No Then
                        Exit Sub
                    End If

                End If

            End If


            'check the password of the folder - unless you are administrator
            If Not g_bUserIsAdministrator Then
                m_lReturn = VerifyPasswords(sNodeKeys)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Exit Sub
                End If
            End If

            'get the ancestry

            m_lReturn = g_oBusiness.GetFolderTree(lFoldNum, vFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            'instance the scanner if not already done so
            If m_oScan Is Nothing Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                m_oScan = New iDOCScan.interface_Renamed()


                m_lReturn = m_oScan.Initialise(False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then


                    m_oScan.Dispose()
                    m_oScan = Nothing
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise scanner.", vApp:=ACApp, vClass:=ACClass, vMethod:="Scan")

                    Exit Sub
                End If

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            End If


            'call the scan method

            m_lReturn = m_oScan.Scan(vFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'ND 071100 - check scan object to see if it has just been hidden so
            '            user can select another folder, if so show in status bar

            If m_oScan.HiddenForFolderSelect Then
                staContents.Items.Item(0).Text = "Please select a new folder"
            Else
                staContents.Items.Item(0).Text = ""
                UpdateStatusBar(lvwDocList)
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Scan", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SetNodeAccessLevel
    '
    ' Description: This function calls the access level object to set
    ' the access level for selected node
    '
    ' ***************************************************************** '
    Private Sub SetNodeAccessLevel()

        Dim lNodeNum As Integer
        Dim sNodeName As String = ""
        Dim iNodeType As Integer
        Dim sNodeKey() As DOCConst.DOCNodes = Nothing


        Try
            If TypeOf (m_cntCurrent) Is TreeView Then
                sNodeName = CType(m_cntCurrent, TreeView).SelectedNode.Name.Trim()
            ElseIf TypeOf (m_cntCurrent) Is ListView Then
                sNodeName = CType(m_cntCurrent, ListView).FocusedItem.Name.Trim()
            End If

            If Not g_bUserIsAdministrator Then

                ReDim sNodeKey(0)
                If TypeOf (m_cntCurrent) Is TreeView Then
                    sNodeKey(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name.Trim()
                    sNodeKey(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text
                ElseIf TypeOf (m_cntCurrent) Is ListView Then
                    sNodeKey(0).Key = CType(m_cntCurrent, ListView).FocusedItem.Name.Trim()
                    sNodeKey(0).Text = CType(m_cntCurrent, ListView).FocusedItem.Text
                End If
                'sNodeKey(0).Key = m_cntCurrent.FocusedItem.Name
                'sNodeKey(0).Text = m_cntCurrent.FocusedItem.Text

                m_lReturn = VerifyPasswords(sNodeKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user couldn't be bothered to supply the password - or got it wrong
                    Exit Sub
                End If
            End If

            'get the access level object and initialise it if not already done so
            If m_oSetAccessLevel Is Nothing Then

                m_oSetAccessLevel = New iDOCSetAccessLevel.Interface_Renamed()


                'developer guide no. 9
                'm_lReturn = CType(m_oSetAccessLevel, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                m_lReturn = m_oSetAccessLevel.Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise iDOCSetAccessLevel.Interface class", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNodeAccessLevel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            ' get node num
            m_lReturn = ExtractNumFromKey(sNodeName, lNodeNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' get the node name
            'sNodeName = m_cntCurrent.FocusedItem.Text.Trim()

            ' get the node type
            Select Case sNodeName.Substring(0, 1)
                Case ACFolder
                    ' folder
                    iNodeType = DOCNode_Folder

                Case ACDocument
                    ' document
                    iNodeType = DOCNode_Document

                Case Else

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid Node Type", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNodeAccessLevel")

                    Exit Sub

            End Select

            ' call the set access level method for this node

            m_lReturn = m_oSetAccessLevel.SetAccessLevel(iNodeType:=iNodeType, lNodeNum:=lNodeNum, sNodeName:=sNodeName, iUserAccessLevel:=g_iAccessLevel)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'error
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set access level.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNodeAccessLevel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="SetNodeAccessLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DeleteAnnotation
    '
    ' Description: Delete currently selected annotation from DB and
    ' from display.
    '
    ' ***************************************************************** '
    Private Sub DeleteAnnotation()


        Dim sKey As String = ""
        Dim lAnnId As Integer

        Try

            'if no annotations to delete, then exit
            If lvwAnnotations.Items.Count < 1 Then
                Exit Sub
            End If

            ' extract annotation id from key of selected item
            ' ie the key minus the first char
            sKey = lvwAnnotations.FocusedItem.Name
            lAnnId = CInt(sKey.Substring(sKey.Length - (sKey.Length - 1)))

            'call business to delete ann

            m_lReturn = g_oBusiness.DeleteAnnotation(lAnnId:=lAnnId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete annotation.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAnnotation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            lvwAnnotations.Items.RemoveByKey(sKey)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAnnotation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: DeleteDocKeyword
    '
    ' Description: Delete currently selected Keyword from DB and
    ' from display.
    '
    ' ***************************************************************** '
    Private Sub DeleteDocKeyword()

        Dim sKey As String = ""
        Dim lDocKeywordID As Integer


        Try

            'if no keywords to delete, then exit
            If lvwKeyWords.Items.Count < 1 Then
                Exit Sub
            End If

            ' extract DocKeyword id from key of selected item
            ' ie the key minus the first char
            sKey = lvwKeyWords.FocusedItem.Name
            lDocKeywordID = CInt(sKey.Substring(sKey.Length - (sKey.Length - 1)))

            'call business to delete ann

            m_lReturn = g_oBusiness.DeleteDocKeyword(lDocKeywordID:=lDocKeywordID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete keyword.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocKeyword", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            lvwKeyWords.Items.RemoveByKey(sKey)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocKeyword", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub ImportDocument()

        Dim sKey As String = ""
        Dim lFoldNum As Integer
        Dim dDocDate As Date
        Dim lFileSize, lDocNum As Integer
        Dim sTmpPageName, sDataPath As String

        Dim itmX As ListViewItem
        Dim sDocName, sPageType As String
        Dim sNodeKey() As DOCConst.DOCNodes = Nothing
        'Dim oZipper As bSIRZipper.Zipper
        Dim sFolderKey, sFolderName As String
        Dim vFileList() As Object
        Dim sFilePath As String = ""


        Try

            'instance the zipper
            'Set oZipper = New bSIRZipper.Zipper

            'check the passwords of the node - unless you are adminstrator
            If Not g_bUserIsAdministrator Then

                ReDim sNodeKey(0)

                'sNodeKey(0).Text = m_cntCurrent.SelectedItems(0).Tag
                'sNodeKey(0).Text = m_cntCurrent.SelectedItems(0).Text
                If (TypeOf m_cntCurrent Is TreeView) Then
                    sNodeKey(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                    sNodeKey(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text
                End If





                m_lReturn = VerifyPasswords(sNodeKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Exit Sub
                End If
            End If

            'get the import document

            'dlgMain.FileName = ""
            dlgMainOpen.FileName = ""
            'SP231101 - Multi select support

            'dlgMain.MaxFileSize = 32000

            'dlgMain.flags = cdlOFNHideReadOnly + cdlOFNAllowMultiselect + cdlOFNExplorer

            dlgMainOpen.Title = "Select Import Document"

            dlgMainOpen.Filter = "All Files (*.*)|*.*|" & _
                             "DocuMaster Files (*.txt;*.tif;*.rtf)|*.txt;*.tif;*.rtf|" & _
                             "Office Files (*.doc*;*.xl*;*.ppt*;*.odb;*.mdb;*.htm;*.html)|*.doc*;*.xl*;*.ppt*;*.odb;*.mdb;*.htm;*.html|" & _
                             "Plain Text Files (*.txt)|*.txt|" & _
                             "Tagged Image Files (*.tif)|*.tif"
            '09/06/1999 Sob Added Office files and all files to Import Dialog and set Filter Index

            dlgMainOpen.FilterIndex = 2

            'dlgMain.ShowOpen()
            dlgMainOpen.ShowDialog()

            'check if user cancelled

            If Strings.Len(dlgMainOpen.FileName) = 0 Then
                Exit Sub
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'SP231101 - convert the file list to an array

            m_lReturn = ConvertFilesToArray(dlgMainOpen.FileName, sFilePath, vFileList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to ConvertFilesToArray", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'get destination folder num

            'sKey = m_cntCurrent.SelectedItems(0).Tag
            If (TypeOf m_cntCurrent Is TreeView) Then
                sKey = CType(m_cntCurrent, TreeView).SelectedNode.Name
            End If

            m_lReturn = ExtractNumFromKey(sKey, lFoldNum)


            'SP231101 - Start the loop for multiple file import
            Dim sFilename As String = ""
            Dim lExtLen As Integer
            For Each vFileList_item As Object In vFileList

                'get the file size in bytes

                lFileSize = CInt((New FileInfo(sFilePath & CStr(vFileList_item))).Length)  'SP231101

                'zip the file to a temp area on the server

                m_lReturn = g_oBusiness.GetDataPath(lVolumeID:=DOCHD1_ID, sDataPath:=sDataPath)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If


                sTmpPageName = sDataPath & "\tmp\" & CStr(vFileList_item)  'SP231110

                m_lReturn = MakePath(sTmpPageName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to MakePath", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                FileSystem.FileCopy(sFilePath & CStr(vFileList_item), sTmpPageName)

                'm_lReturn = m_oZipper.ZipFile(sFilePath & CStr(vFileList_item), sTmpPageName) 'SP231101

                'If Not m_lReturn Then
                '    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to ZipFile", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                '    Exit Sub
                'End If

                'get the doc date
                dDocDate = DateTime.Now

                'separate doc name and extension
                'This does not work as it assumes 3 charter extesion we don't use DOS anymore
                'sDocName$ = Left$(dlgMain.FileTitle, Len(dlgMain.FileTitle) - 4)
                'sPageType$ = UCase$(Right$(dlgMain.FileTitle, 3))

                ' Find the position of the last dot in the filename
                ' and get the length of the file extension from that


                sFilename = CStr(vFileList_item)  'SP231101
                lExtLen = 0

                For lPos As Integer = sFilename.Length To 1 Step -1
                    If Mid(sFilename, lPos, 1) = "." Then
                        lExtLen = sFilename.Length - lPos
                        Exit For
                    End If
                Next

                ' Get trailing chars up to dot and convert to uppercase
                If lExtLen > 0 Then
                    sPageType = sFilename.Substring(sFilename.Length - lExtLen).ToUpper()
                Else
                    sPageType = ""
                End If


                sDocName = CStr(vFileList_item).Substring(0, Strings.Len(CStr(vFileList_item)) - (lExtLen + 1))

                'call business to do write new document to database

                m_lReturn = g_oBusiness.ImportDocument(sDocName:=sDocName, sPageType:=sPageType, lFoldNum:=lFoldNum, lPageSize:=lFileSize, sTmpPageName:=sTmpPageName, lDocNum:=lDocNum, dDocDate:=dDocDate, sZipped:="Y")


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Import Document.", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub
                End If

                'all is fine, so update the doc list if populated for import folder
                If Convert.ToString(lblTitleMain(1).Tag) = sKey Then

                    'SP231101 - why is this done?
                    'sKey$ = "D " & CStr(CLng(dDocDate)) & lDocNum&

                    'Add node and stuff

                    itmX = lvwDocList.Items.Add("D " & CInt(dDocDate.ToOADate) & CStr(lDocNum), 1)
                    itmX.Name = "D " & CInt(dDocDate.ToOADate) & CStr(lDocNum)
                    itmX.Text = sDocName

                    itmX.SubItems.Add(dDocDate.ToString())
                    Select Case sPageType
                        Case "TIF", "TIFF"

                            itmX.ImageKey = "IMGTIFF"

                            itmX.SubItems.Add(DOCImage)
                        Case "RTF"

                            itmX.ImageKey = "IMGRTF"

                            itmX.SubItems.Add(DOCRTF)
                        Case "TXT", "TEXT", "ASCI"

                            itmX.ImageKey = "IMGTEXT"

                            itmX.SubItems.Add(DOCText)
                        Case "DOC", "DOCX", "DOT", "DOTX", "ASC", "ANS", "MCW", "WPS"  'SOB 01/06/99 WORD FILES

                            itmX.ImageKey = "IMGWORD"

                            itmX.ImageKey = "IMGWORD"

                            itmX.SubItems.Add(DOCWRD)
                        Case "XLS", "XLSX", "XLT", "XLS", "CSV", "WK1", "WK2", "WK3", "WK4", "WQ1", "PRN", "DIF", "SLK", "XLA", "TAB"  'SOB 01/06/99 EXCEL Files

                            itmX.ImageKey = "IMGEXCEL"  'Ms Excel

                            itmX.ImageKey = "IMGEXCEL"

                            itmX.SubItems.Add(DOCEXL)
                        Case "PPT", "PPTX", "POT", "POTX", "PPS", "PPSX", "PPA"  'SOB 01/06/99 Power Point Files

                            itmX.ImageKey = "IMGPOWERPNT"

                            itmX.ImageKey = "IMGPOWERPNT"

                            itmX.SubItems.Add(DOCPWP)
                        Case "MDB", "ADP", "MDW", "MDA", "MDE", "ADE", "DBF", "DB"  'SOB 01/06/99 Ms Access Files

                            itmX.ImageKey = "IMGACCESS"

                            itmX.ImageKey = "IMGACCESS"

                            itmX.SubItems.Add(DOCACC)
                        Case "HTM", "HTML", "SHTM", "SHTML", "STM", "ASP", "HTT", "CSS", "CFML", "XML"  'SOB 01/06/99 IE, Netscape Files

                            itmX.ImageKey = "IMGIEXPLORER"

                            itmX.ImageKey = "IMGIEXPLORER"

                            itmX.SubItems.Add(DOCHTM)
                        Case "GIF", "GIFF"

                            itmX.ImageKey = "IMGGIF"

                            itmX.ImageKey = "IMGGIF"

                            itmX.SubItems.Add(DOCGIF)
                        Case "JPEG", "JPG"

                            itmX.ImageKey = "IMGJPEG"

                            itmX.ImageKey = "IMGJPEG"

                            itmX.SubItems.Add(DOCJPG)
                        Case "EML", "OFT", "MSG", "EML"  'SOB 01/06/99 E-Mail Doc

                            itmX.ImageKey = "IMGOUTLOOK"

                            itmX.ImageKey = "IMGOUTLOOK"

                            itmX.SubItems.Add(DOCEML)
                        Case "PDF"

                            itmX.ImageKey = "IMGADOBE"

                            itmX.ImageKey = "IMGADOBE"

                            itmX.SubItems.Add(DOCPDF)
                        Case "HLP"

                            itmX.ImageKey = "IMGHELP"

                            itmX.ImageKey = "IMGHELP"

                            itmX.SubItems.Add(DOCHLP)
                        Case "ZIP", "GZ"

                            itmX.ImageKey = "IMGZIP"

                            itmX.ImageKey = "IMGZIP"

                            itmX.SubItems.Add(DOCZIP)
                        Case Else
                            'General Document

                            itmX.ImageKey = "IMGUNKNOWN"

                            itmX.ImageKey = "IMGUNKNOWN"

                            itmX.SubItems.Add(DOCUnknown)
                    End Select

                End If


                'SP231101 - end of multiple file import
            Next vFileList_item

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            'MS250900 Added everything inside if statement

            ' auto fire-up keywords/annotations windows straight after an import of a file                  ' MS250900 >
            'SP231101 Will only want to do this if one file imported else makes no sense
            If (m_bAutoKeyword) And (vFileList.GetUpperBound(0) = 0) Then

                ' the imported document defaults to last one in the document listview

                lvwDocList.Focus()

                lvwDocList.Items(lvwDocList.Items.Count).Selected = True

                lvwDocList.Refresh()

                ' Have to use the object var save the current folder's  key & name

                'sFolderKey = m_cntCurrent.SelectedItems(0).Tag
                '
                'sFolderName = m_cntCurrent.SelectedItems(0).Text

                If (TypeOf m_cntCurrent Is TreeView) Then
                    sFolderKey = CType(m_cntCurrent, TreeView).SelectedNode.Name
                    sFolderName = CType(m_cntCurrent, TreeView).SelectedNode.Text
                End If

                ' need to pass the document key and document name via the folder object reference
                ' as the Addkeyword and AddAnnotation functiions require that


                'm_cntCurrent.SelectedItems(0).Tag = lvwDocList.Items(lvwDocList.Items.Count).Text
                '
                'm_cntCurrent.SelectedItems(0).Text = sDocName

                If (TypeOf m_cntCurrent Is TreeView) Then
                    CType(m_cntCurrent, TreeView).SelectedNode.Name = lvwDocList.Items(lvwDocList.Items.Count).Text
                    CType(m_cntCurrent, TreeView).SelectedNode.Tag = lvwDocList.Items(lvwDocList.Items.Count).Text
                    CType(m_cntCurrent, TreeView).SelectedNode.Text = sDocName
                End If

                AddKeyword()

                Do While MessageBox.Show("Add Annotation?", "Documaster Enterprise", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes
                    AddAnnotation()
                Loop

                ' copy back folder key & name to original state of folder's object var

                'm_cntCurrent.SelectedItems(0).Tag = sFolderKey
                '
                'm_cntCurrent.SelectedItems(0).Text = sFolderName

                If (TypeOf m_cntCurrent Is TreeView) Then
                    CType(m_cntCurrent, TreeView).SelectedNode.Name = sFolderKey
                    CType(m_cntCurrent, TreeView).SelectedNode.Tag = sFolderKey
                    CType(m_cntCurrent, TreeView).SelectedNode.Text = sFolderName
                End If

            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            ' CTAF 20031205 - Moved to after the iPMFunc.LogMessage so that the Err object isnt reset
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)


            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: ConvertFilesToArray
    '
    ' Description:
    '
    ' History: 23/11/2001 SP - Created.
    '
    ' SP231101 - Converts the output of a dialog file open to an arrays of files
    ' and their parent folder
    '
    ' ***************************************************************** '
    Private Function ConvertFilesToArray(ByRef sFileString As String, ByRef sFilePath As String, ByRef vFileList() As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sTmp As New StringBuilder
        Dim iFileNumber As Integer

        Dim bMultipleFiles As Boolean
        Dim iTmp As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iFileNumber = 0
            bMultipleFiles = False

            'See if multiple files returned
            For i As Integer = 1 To sFileString.Length
                If Strings.Asc(Mid(sFileString, i, 1)(0)) = 0 Then
                    bMultipleFiles = True
                End If
            Next i

            Select Case bMultipleFiles
                Case True

                    'loop thru - file paths are delimited by null
                    For i As Integer = 1 To sFileString.Length

                        'is it a null
                        If Strings.Asc(Mid(sFileString, i, 1)(0)) = 0 Then
                            If iFileNumber = 0 Then
                                'this is the folder
                                sFilePath = sTmp.ToString()
                                sTmp = New StringBuilder("")
                                iFileNumber += 1
                            Else
                                'It's the first file name to be stored
                                If iFileNumber = 1 Then
                                    ReDim vFileList(0)
                                    iFileNumber += 1
                                Else
                                    ReDim Preserve vFileList(vFileList.GetUpperBound(0) + 1)
                                End If


                                vFileList(vFileList.GetUpperBound(0)) = sTmp.ToString()
                                sTmp = New StringBuilder("")
                            End If
                        Else
                            sTmp.Append(Mid(sFileString, i, 1))
                        End If

                    Next i

                    'Catch the last one
                    If sTmp.ToString() <> "" Then
                        If iFileNumber = 1 Then
                            ReDim vFileList(0)
                        Else
                            ReDim Preserve vFileList(vFileList.GetUpperBound(0) + 1)
                        End If

                        vFileList(vFileList.GetUpperBound(0)) = sTmp.ToString()
                    End If

                Case False

                    'just one file so get the name by looping back
                    For i As Integer = sFileString.Length To 1 Step -1
                        If Mid(sFileString, i, 1) = "\" Then
                            iTmp = i
                            Exit For
                        End If
                    Next i

                    sFilePath = sFileString.Substring(0, iTmp)
                    ReDim vFileList(0)

                    vFileList(0) = sFileString.Substring(sFileString.Length - (sFileString.Length - iTmp))

            End Select

            'Check the file path has a \ on the end else add one
            If Not sFilePath.EndsWith("\") Then
                sFilePath = sFilePath & "\"
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ConvertFilesToArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertFilesToArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ExportDocument
    '
    ' Description: Let user save a file out of DocuMaster.
    '
    ' ***************************************************************** '
    Private Sub ExportDocument()

        Dim lDocNum As Integer
        Dim vPageArray() As Object
        Dim sTmpName, sTmpExt As String
        Dim iPageCnt As Integer
        Dim sNodeKey() As DOCConst.DOCNodes = Nothing
        'Dim oZipper As bSIRZipper.Zipper
        Dim bZipped As Boolean
        Dim sFilename As String = ""
        Dim iTemp As Integer

        Try


            'check the passwords of the node - unless you are adminstrator
            If Not g_bUserIsAdministrator Then

                ReDim sNodeKey(0)
                sNodeKey(0).Key = m_cntCurrent.FocusedItem.Name
                sNodeKey(0).Text = m_cntCurrent.FocusedItem.Text

                m_lReturn = VerifyPasswords(sNodeKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Exit Sub
                End If
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'get doc num
            m_lReturn = ExtractNumFromKey(m_cntCurrent.FocusedItem.Name, lDocNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If

            '    'get list of file paths for this doc
            '    m_lReturn& = g_oBusiness.AmIZippedUp(lDocNum:=lDocNum&, _
            ''                                         bZipped:=bZipped)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '
            '        iPMFunc.SetMousePointer PMMouseReset
            '        iPMFunc.LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed in AmIZippedUp", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="ExportDocument", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '
            '        Exit Sub
            '    End If

            'get list of file paths for this doc

            m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Export Document", vApp:=ACApp, vClass:=ACClass, vMethod:="ExportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

            'Check if File name contains a special character
            Dim sSpecialCharacter(8) As String

            sSpecialCharacter(0) = "/"
            sSpecialCharacter(1) = "\"
            sSpecialCharacter(2) = ":"
            sSpecialCharacter(3) = "*"
            sSpecialCharacter(4) = "?"
            sSpecialCharacter(5) = """"
            sSpecialCharacter(6) = "<"
            sSpecialCharacter(7) = ">"
            sSpecialCharacter(8) = "|"

            For i As Integer = 0 To sSpecialCharacter.GetUpperBound(0)
                iTemp = (lvwDocList.FocusedItem.Text.IndexOf(sSpecialCharacter(i)) + 1)
                If iTemp > 0 Then
                    MessageBox.Show("Document name contains a special character, rename it before saving.", DOCAppName)
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Exit Sub
                End If
            Next i

            'get the export file name via com dialog box

            'what type is it ? Check the extension of the first page
            dlgMainOpen.FileName = lvwDocList.FocusedItem.Text
            dlgMainSave.FileName = lvwDocList.FocusedItem.Text

            'dlgMain.Flags = CInt(CStr(MSComDlg.FileOpenConstants.cdlOFNHideReadOnly) & CStr(MSComDlg.FileOpenConstants.cdlOFNOverwritePrompt))
            dlgMainOpen.Title = "Save Document As"
            dlgMainSave.Title = "Save Document As"

            dlgMainOpen.DefaultExt = CStr(vPageArray(vPageArray.GetLowerBound(0))).Substring(CStr(vPageArray(vPageArray.GetLowerBound(0))).Length - 3).ToUpper()
            dlgMainSave.DefaultExt = CStr(vPageArray(vPageArray.GetLowerBound(0))).Substring(CStr(vPageArray(vPageArray.GetLowerBound(0))).Length - 3).ToUpper()

            dlgMainOpen.Filter = "(*." & CStr(vPageArray(vPageArray.GetLowerBound(0))).Substring(CStr(vPageArray(vPageArray.GetLowerBound(0))).Length - 3).ToLower() & ")|*." & CStr(vPageArray(vPageArray.GetLowerBound(0))).Substring(CStr(vPageArray(vPageArray.GetLowerBound(0))).Length - 3).ToLower()
            dlgMainSave.Filter = "(*." & CStr(vPageArray(vPageArray.GetLowerBound(0))).Substring(CStr(vPageArray(vPageArray.GetLowerBound(0))).Length - 3).ToLower() & ")|*." & CStr(vPageArray(vPageArray.GetLowerBound(0))).Substring(CStr(vPageArray(vPageArray.GetLowerBound(0))).Length - 3).ToLower()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            dlgMainSave.ShowDialog()
            dlgMainOpen.FileName = dlgMainSave.FileName

            'check if user cancelled - ie no change in filename
            If dlgMainOpen.FileName = lvwDocList.FocusedItem.Text Then
                Exit Sub
            End If

            'Delete if destination file already present
            If FileExists(dlgMainOpen.FileName) Then
                File.Delete(dlgMainOpen.FileName)
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'instance the zipper
            '    Set oZipper = New bSIRZipper.Zipper

            sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))

            ' Check to see if zip file...

            m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZIPFile:=bZipped)

            If Not m_lReturn Then
                'error - assume unzipped
                bZipped = False
            End If

            'separate extension from file title
            sTmpName = dlgMainOpen.FileName.Substring(0, Strings.Len(dlgMainOpen.FileName) - 4)
            sTmpExt = dlgMainOpen.FileName.Substring(dlgMainOpen.FileName.Length - 4)

            'Copy doc to new location - if we have more than one page for the doc
            '(ie multi page tiff) , number each page
            iPageCnt = 1

            Dim cloudHostingOptionValue As String = ""
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)
            Dim cloudHostingEnabled As Boolean = (gPMFunctions.NullToString(cloudHostingOptionValue) = "1")
            Dim sDataPath As String = String.Empty
            m_lReturn = g_oBusiness.GetDataPath(lVolumeID:=DOCHD1_ID, sDataPath:=sDataPath)
            Dim tmpFolder As String = sDataPath & "\tmp\"
            Dim s3FileName As String = String.Empty
            Dim repository As IS3Repository
            Dim tempFileName As String
            If cloudHostingEnabled Then
                repository = New S3Repository(Environment.GetEnvironmentVariable("AWS_DME_BUCKET_NAME"),
                    Environment.GetEnvironmentVariable("AWS_REGION"), g_sUserName)
            End If

            For i As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0)

                If vPageArray.GetLowerBound(0) = vPageArray.GetUpperBound(0) Then

                    'only one page
                    If bZipped Then

                        m_lReturn = m_oZipper.UnZipFile(CStr(vPageArray(i)), dlgMainOpen.FileName)
                    Else
                        If cloudHostingEnabled Then
                            s3FileName = CStr(vPageArray(i)).Substring(sDataPath.Length).Replace("\", "/").TrimStart("/")
                            tempFileName = tmpFolder & CStr(vPageArray(i)).Substring(CStr(vPageArray(i)).LastIndexOf("\") + 1)
                            MakePath(tempFileName)
                            m_lReturn = repository.DownloadFileAsync(s3FileName, tmpFolder).Result
                            m_lReturn = DOCGeneralFunc.CopyFile(tempFileName, dlgMainOpen.FileName)
                            KillFile(tempFileName)
                        Else
                            m_lReturn = DOCGeneralFunc.CopyFile(CStr(vPageArray(i)), dlgMainOpen.FileName)
                        End If

                    End If

                Else

                    'several pages, so add in page counter
                    If bZipped Then

                        m_lReturn = m_oZipper.UnZipFile(CStr(vPageArray(i)), sTmpName & CStr(iPageCnt) & sTmpExt)
                    Else
                        If cloudHostingEnabled Then
                            s3FileName = CStr(vPageArray(i)).Substring(sDataPath.Length).Replace("\", "/").TrimStart("/")
                            tempFileName = tmpFolder & CStr(vPageArray(i)).Substring(CStr(vPageArray(i)).LastIndexOf("\") + 1)
                            MakePath(tempFileName)
                            m_lReturn = repository.DownloadFileAsync(s3FileName, tmpFolder).Result

                            m_lReturn = DOCGeneralFunc.CopyFile(tempFileName, sTmpName & CStr(iPageCnt) & sTmpExt)
                            KillFile(tempFileName)
                        Else
                            m_lReturn = DOCGeneralFunc.CopyFile(CStr(vPageArray(i)), sTmpName & CStr(iPageCnt) & sTmpExt)
                        End If
                    End If

                    iPageCnt += 1

                End If

            Next i

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ExportDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub



    ' ***************************************************************** '
    ' Name: Filter
    '
    ' Description: Expand the folder tree, returning only folders that
    ' start with the filter
    '
    ' ***************************************************************** '
    Private Sub Filter()

        Dim sFilter As String = ""
        Dim lFoldNum As Integer


        On Error GoTo Err_Filter

        sFilter = Interaction.InputBox("Please enter the inital letters of the folder " & _
                  "you require.", "Folder Select")

        If sFilter = "" Then
            Exit Sub
        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        'm_lReturn = RemoveTreeChildren(tvwMain, tvwMain.SelectedNode.Text)
        m_lReturn = RemoveTreeChildren(tvwMain, tvwMain.SelectedNode)

        'single click
        NodeClick(tvwMain, lvwDocList, tvwMain.SelectedNode.Name, sFilter)

        'Swap icons of newly opened folder and last open folder
        If m_sMainLastOpenFolder <> "" Then
            'this may not exist
            On Error Resume Next
            tvwMain.Nodes.Item(m_sMainLastOpenFolder).ImageKey = "IMGCLOSEDFOLDER"
        End If


        tvwMain.SelectedNode.ImageKey = "IMGOPENFOLDER"
        m_sMainLastOpenFolder = tvwMain.SelectedNode.Name

        'update the label
        lblTitleMain(1).Text = "Contents of '" & tvwMain.SelectedNode.Text & "'"
        lblTitleMain(1).Tag = tvwMain.SelectedNode.Name


        'if we are not displaying folders in the document list view, then preceding
        'click event will not have gotten the folder list, so we'd best get it now
        If m_bDocsOnly Then

            'Get the folder num from the selected node key
            m_lReturn = ExtractNumFromKey(tvwMain.SelectedNode.Name, lFoldNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If

            'Get the folders in the selected folder
            m_lReturn = GetFolderList(lFoldNum, sFilter, m_vFolderArray)

        End If

        m_lReturn = PopulateTreeChildren(tvwMain, tvwMain.SelectedNode.Index, m_vFolderArray)

        'expand
        tvwMain.SelectedNode.Expand()

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Exit Sub
        'Remove kids
        '    m_lReturn = RemoveTreeChildren(tvwMain, tvwMain.SelectedItem)
        '
        '    m_lReturn = ExtractNumFromKey(tvwMain.SelectedItem.Key, lFoldNum)
        '
        '    m_lReturn = GetFolderList(lFoldNum, sFilter, m_vFolderArray)
        '
        '    m_lReturn = GetDocList(lFoldNum, "", m_vDocArray)
        '
        '    m_lReturn = PopulateListView(lvwDocList, m_vFolderArray, m_vDocArray)
        '
        '    m_lReturn = PopulateTreeChildren(tvwMain, tvwMain.SelectedItem.Index, m_vFolderArray)
        '
        '    'expand
        '    tvwMain.SelectedItem.Expanded = True

Err_Filter:

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Filter", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayDocumentInformation
    '
    ' Description: Create instance of doc information object and
    ' start it for selected doc.
    '
    ' ***************************************************************** '
    Private Sub DisplayDocumentInformation()
        Dim iDOCInformation As Object = Nothing

        Dim sKey As String = ""
        Dim lDocNum As Integer
        Dim sNewName As String = ""


        Try

            'get doc num from key
            sKey = lvwDocList.FocusedItem.Name

            'ensure node is not a folder
            If sKey.Substring(0, 1) <> ACDocument Then
                Exit Sub
            End If

            m_lReturn = ExtractNumFromKey(sKey, lDocNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'create document information object, if not already done so
            If m_oInformation Is Nothing Then

                m_oInformation = New iDOCInformation.Interface_Renamed()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Create Object", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayDocumentInformation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub
                End If


                'developer guide no. 9
                'm_lReturn = CType(m_oInformation, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                m_lReturn = m_oInformation.Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayDocumentInformation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub
                End If

            End If


            m_lReturn = m_oInformation.Start(lDocNum, sNewName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to start object", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayDocumentInformation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If
            'added to set the find mode again
            SetViewModeFindResults()
            'If new name has been returned, update list view
            If sNewName <> "" Then
                'lvwDocList.FocusedItem.Text = sNewName
                If lvwDocList.Items.Find(sKey, True).Length > 0 Then
                    lvwDocList.Items.Find(sKey, True)(0).Text = sNewName
                End If
            End If

            ' Update the keyword list
            lvwKeyWords.Tag = ""
            CheckKeywords(sKey:=sKey)

            ' Update the annotations list
            lvwAnnotations.Tag = ""
            CheckAnnotations(sKey:=sKey)
            SetViewModeMain()
        Catch excep As System.Exception



            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayDocumentInformation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: SetNodePassword
    '
    ' Description: Set a password for the currently selected node.
    ' ie first verify current password then call password object
    ' to set a new one.
    '
    ' ***************************************************************** '
    Private Sub SetNodePassword()
        Dim lNodeNum As Integer
        Dim iNodeType As Integer
        Dim sKeyIn, sKeyOut, sPassword As String

        Try

            'get the key info
            Dim sNodeKeys() As DOCConst.DOCNodes = ArraysHelper.InitializeArray(Of DOCConst.DOCNodes)(1)
            If TypeOf (m_cntCurrent) Is TreeView Then
                sNodeKeys(0).Key = CType(m_cntCurrent, TreeView).SelectedNode.Name
                sNodeKeys(0).Text = CType(m_cntCurrent, TreeView).SelectedNode.Text
            End If
            If TypeOf (m_cntCurrent) Is ListView Then
                sNodeKeys(0).Key = CType(m_cntCurrent, ListView).SelectedItems(0).Name
                sNodeKeys(0).Text = CType(m_cntCurrent, ListView).SelectedItems(0).Text
            End If
            'get node type
            Select Case sNodeKeys(0).Key.Substring(0, 1)
                Case ACFolder
                    iNodeType = DOCNode_Folder

                Case ACDocument
                    iNodeType = DOCNode_Document

                Case Else

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid Node Type", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNodePassword", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub
            End Select

            'get node number
            m_lReturn = ExtractNumFromKey(sNodeKeys(0).Key, lNodeNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'check the passwords of all the nodes - unless you are adminstrator
            If Not g_bUserIsAdministrator Then
                m_lReturn = VerifyPasswords(sNodeKeys)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Exit Sub
                End If
            End If

            'create password object if not already in existence
            If m_oPassword Is Nothing Then

                m_oPassword = New iDOCPassword.Interface_Renamed()


                m_lReturn = m_oPassword.Initialise(False, g_sUserName, "", 0, g_iSourceID, g_iLanguageID, 0, 4, ACApp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNodePassword", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'add a password to this node

            m_lReturn = m_oPassword.AddPassword(lNodeNum:=lNodeNum, iNodeLevel:=iNodeType, sEncryptedPassword:=sPassword)


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMOK, gPMConstants.PMEReturnCode.PMCancel  'fine
                Case Else
                    'no, no
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set password", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNodePassword", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

            End Select

            ' Change the node key to show it is passworded (or not). Note we change in both
            ' treeview and listview as node may appear in both. Must ignore errors
            ' as node is probably only in one
            If sPassword.Trim() <> "" Then
                'indicate node is passworded in key
                sKeyIn = sNodeKeys(0).Key
                sKeyOut = sNodeKeys(0).Key
                Mid(sKeyOut, 2, 1) = ACPassword

                Try
                    tvwMain.Nodes.Item(sKeyIn).Name = sKeyOut
                    lvwDocList.Items.Item(sKeyIn).Name = sKeyOut
                Catch ex As Exception

                End Try

            Else
                'indicate node is not passworded in key
                sKeyIn = sNodeKeys(0).Key
                sKeyOut = sNodeKeys(0).Key
                Mid(sKeyOut, 2, 1) = " "

                Try
                    tvwMain.Nodes.Item(sKeyIn).Name = sKeyOut
                    lvwDocList.Items.Item(sKeyIn).Name = sKeyOut

                Catch ex As Exception

                End Try

            End If

            Exit Sub

        Catch ex As Exception
            'Log to File
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="SetNodePassword", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: LocateFolder
    '
    '
    ' Description: Locate and expand given folder
    '
    ' Edit History: JH071298 Revamped so only the selected folder
    ' is retrieved if it is not already there.
    ' See also ConstructView
    '
    ' ***************************************************************** '
    Private Sub LocateFolder(ByRef lFolderNum As Integer, ByRef bCalledAtStartUp As Boolean)

        Dim vFolderArray(,) As Object
        Dim SortNode As TreeNode

        Dim bPopulateChildren, bNodeExists As Boolean

        Dim lNum, lChildren As Integer

        Dim sSiblingKey, sKey As String


        On Error GoTo Err_LocateFolder

        'message user, unless called at start up
        If (lFolderNum = 0) And (Not bCalledAtStartUp) Then
            MessageBox.Show("You do not have a home folder currently set.", DOCAppName)
            Exit Sub
        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        'set the view to main
        mnuViewMain_Click(mnuViewMain, New EventArgs())

        'get the parent tree

        m_lReturn = g_oBusiness.GetFullFolderTree(lNodeNum:=lFolderNum, iNodeType:=DOCNode_Folder, vFolderArray:=vFolderArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            Exit Sub
        End If

        If Not Information.IsArray(vFolderArray) Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            Exit Sub
        End If

        Dim sNodeKeys() As DOCConst.DOCNodes = ArraysHelper.InitializeArray(Of DOCConst.DOCNodes)(vFolderArray.GetUpperBound(1) + 1)

        'Construct node keys
        For i As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

            If CStr(vFolderArray(2, i)).Trim() <> "" Then
                'ie passworded
                sKey = ACFolder & ACPassword & CStr(CInt(CDate(vFolderArray(3, i)).ToOADate)) & _
                       CStr(vFolderArray(0, i))
            Else
                sKey = ACFolder & " " & CStr(CInt(CDate(vFolderArray(3, i)).ToOADate)) & _
                       CStr(vFolderArray(0, i))
            End If

            sNodeKeys(i).Key = sKey
            sNodeKeys(i).Text = CStr(vFolderArray(1, i))
        Next i

        'check for passwords
        If Not g_bUserIsAdministrator Then

            m_lReturn = VerifyPasswords(sNodeKeys)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'user couldn't be bothered to supply the password - or got it wrong
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If
        End If

        ' first expand root level - this will always be present
        tvwMain.Nodes.Item(sNodeKeys(sNodeKeys.GetUpperBound(0)).Key).Expand()

        'see if home folder more than one level deep

        If sNodeKeys.GetUpperBound(0) > sNodeKeys.GetLowerBound(0) Then

            ' now expand remaining -  some may not exist in which case it will go
            ' to error section which will set a flag indicating population required
            For i As Integer = sNodeKeys.GetUpperBound(0) - 1 To sNodeKeys.GetLowerBound(0) Step -1

                On Error GoTo Err_Populate

                'I% + 1 is the parent
                'I% is the child

                tvwMain.Nodes.Item(sNodeKeys(i).Key).Expand()

                On Error GoTo Err_LocateFolder

                If bPopulateChildren Then

                    'JH071298 don't delete any nodes, just add the new one and sort
                    '(have to delete and add the 'Add to view' first and add it after)

                    On Error GoTo Err_AddNode

                    bNodeExists = True

                    If tvwMain.Nodes.Item(sNodeKeys(i + 1).Key).GetNodeCount(False) > 0 Then
                        If tvwMain.Nodes.Item(sNodeKeys(i + 1).Key).FirstNode.Name.Substring(0, 3) = "ADD" Then
                            If bNodeExists Then
                                tvwMain.Nodes.RemoveAt(CInt(tvwMain.Nodes.Item(sNodeKeys(i + 1).Key).FirstNode.Name) - 1)
                            End If
                        End If
                    End If

                    On Error GoTo Err_LocateFolder

                    'get the folder number
                    m_lReturn = ExtractNumFromKey(sNodeKeys(i).Key, lNum)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Exit Sub
                    End If

                    'then retrieve just this one folder

                    m_lReturn = g_oBusiness.GetFolderValues(lNum, m_vFolderArray)

                    '                'folder not in view, so get folder list for parent and populate
                    '                m_lReturn& = RemoveTreeChildren(tvwMain, tvwMain.Nodes(sNodeKeys(I% + 1).Key))
                    '
                    '                If (m_lReturn& <> PMTrue) Then
                    '                    iPMFunc.SetMousePointer PMMouseReset
                    '                    Exit Sub
                    '                End If
                    '
                    '                m_lReturn& = ExtractNumFromKey(sNodeKeys(I% + 1).Key, lNum&)
                    '
                    '                If (m_lReturn& <> PMTrue) Then
                    '                    iPMFunc.SetMousePointer PMMouseReset
                    '                    Exit Sub
                    '                End If
                    '
                    '                'get all folders for this parent
                    '                m_lReturn& = GetFolderList(lNum, "", m_vFolderArray)
                    '
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Exit Sub
                    End If

                    'fill tree
                    m_lReturn = PopulateTreeChildren(tvw:=tvwMain, iIndex:=tvwMain.Nodes.Item(sNodeKeys(i + 1).Key).Index, vArray:=m_vFolderArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Exit Sub
                    End If

                    'then sort so it is alphabetical

                    'tvwMain.Nodes.Item(sNodeKeys(i + 1).Key).Sorted = True
                    tvwMain.Sort()

                    'then unsort so the 'Add to view' node can go in

                    'tvwMain.Nodes.Item(sNodeKeys(i + 1).Key).Sorted = False
                    tvwMain.Sort()


                    'then add the 'Add to view' node

                    'check if more children available

                    'get the folder number
                    m_lReturn = ExtractNumFromKey(sNodeKeys(i + 1).Key, lNum)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Exit Sub
                    End If

                    m_lReturn = CountChildren(lNum, lChildren)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Exit Sub
                    End If

                    If tvwMain.Nodes.Item(sNodeKeys(i + 1).Key).GetNodeCount(False) < lChildren Then


                        'sSiblingKey = tvwMain.Nodes(sNodeKeys(i).Key).FirstSibling.Name
                        sSiblingKey = tvwMain.Nodes(sNodeKeys(i).Key).Nodes(0).Name

                        m_lReturn = AddToViewNode(tvw:=Me.tvwMain, sKey:=sSiblingKey)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                            Exit Sub
                        End If

                    End If

                    bPopulateChildren = False

                End If

            Next i

        End If

        'ensure is visible and selected
        tvwMain.Nodes.Item(sNodeKeys(sNodeKeys.GetLowerBound(0)).Key).EnsureVisible()

        'tvwMain.Nodes.Item(sNodeKeys(sNodeKeys.GetLowerBound(0)).Key).Selected = True
        tvwMain.SelectedNode = tvwMain.Nodes.Find(sNodeKeys(sNodeKeys.GetLowerBound(0)).Key, True)(0)
        'populate doc
        NodeClick(tvwMain, lvwDocList, sNodeKeys(sNodeKeys.GetLowerBound(0)).Key, "")

        If m_sMainLastOpenFolder <> "" Then
            'this may not exist
            On Error Resume Next
            tvwMain.Nodes.Item(m_sMainLastOpenFolder).ImageKey = "IMGCLOSEDFOLDER"
        End If


        tvwMain.SelectedNode.ImageKey = "IMGOPENFOLDER"
        m_sMainLastOpenFolder = tvwMain.SelectedNode.Name

        'update the label
        lblTitleMain(1).Text = "Contents of '" & tvwMain.SelectedNode.Text & "'"
        lblTitleMain(1).Tag = tvwMain.SelectedNode.Name

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Exit Sub

Err_AddNode:
        bNodeExists = False
        Resume Next

Err_Populate:
        'called if we error when expanding a node( ie node not present).
        'Set flag so we know the nodes parent needs populating
        bPopulateChildren = True
        Resume Next

Err_LocateFolder:

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to locate your home folder." & Strings.Chr(10).ToString() & _
                   "Please ensure you have sufficient access for each level.", vApp:=ACApp, vClass:=ACClass, vMethod:="LocateFolder", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub
    ' ***************************************************************** '
    ' Name: SetHomeFolder
    '
    ' Description: Set users home folder
    '
    ' ***************************************************************** '
    Private Sub SetHomeFolder()

        Dim lFolderNum As Integer
        ' Get the selected node
        Dim node As TreeNode

        Try

            'ensure we have folder
            If m_cntCurrent.GetType.Name = "TreeView" Then
                node = CType(m_cntCurrent, TreeView).SelectedNode
                If node.Name.Substring(0, 1) <> ACFolder Then
                    Exit Sub
                End If
            End If


            'check the folder password - unless you are adminstrator
            Dim sNodeKeys() As DOCConst.DOCNodes = ArraysHelper.InitializeArray(Of DOCConst.DOCNodes)(1)

            sNodeKeys(0).Key = node.Name

            sNodeKeys(0).Text = node.Text
            If Not g_bUserIsAdministrator Then
                m_lReturn = VerifyPasswords(sNodeKeys)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Exit Sub
                End If
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = ExtractNumFromKey(node.Name, lFolderNum)

            'set it in business

            m_lReturn = g_oBusiness.SetHomeFolder(lFolderNum:=lFolderNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to set home folder", vApp:=ACApp, vClass:=ACClass, vMethod:="SetHomeFolder", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="SetHomeFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub




    Private Function GetParentNamesFromTree(ByRef tvw As TreeView, ByRef sKey As String, ByRef sParents As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sParentKey As String = ""
        Dim sNames() As String


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim sNames(0)
            'sNames(0) = tvw.Nodes.Item(sKey).Text
            sNames(0) = tvw.Nodes.Find(sKey, True)(0).Text
            sParentKey = sKey

            Do
                If tvw.Nodes.Find(sParentKey, True)(0).Parent Is Nothing Then
                    Exit Do
                Else
                    sParentKey = tvw.Nodes.Find(sParentKey, True)(0).Parent.Name
                    ReDim Preserve sNames(sNames.GetUpperBound(0) + 1)
                    sNames(sNames.GetUpperBound(0)) = tvw.Nodes.Find(sParentKey, True)(0).Text
                End If
            Loop

            sParents = " in '" & sNames(0) & "'"

            For i As Integer = 1 To sNames.GetUpperBound(0)
                sParents = sParents & ", '" & sNames(i) & "'"
            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetParentNamesFromTree", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ****************************************************************************
    '
    ' Function : PrintDocument
    '
    ' Description : Prints the currently selected nodes, using default settings
    '
    ' ****************************************************************************

    Private Function PrintDocument(ByRef bShowSetup As Boolean) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lDocNum As Integer
        Dim sNodeKeys() As DOCConst.DOCNodes = Nothing
        Dim vPageArray As Object
        Dim sFolderName, sTmp As String
        Dim bZipped As Boolean
        Dim sNewFilename As String = ""
        Dim bIsImage As Boolean
        Dim sFilename As String = ""
        Dim iStartPage, iEndPage As Integer
        Dim lEventCnt As Integer
        Dim iTotalPages As Integer
        Dim lLeft, lTop, lRight, lBottom, lWidth, lHeight As Integer
        Dim bMultiFileDocument As Boolean
        Dim oDOCManagerInterface As iDOCManager.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a cache path first
            If m_sCachePath.Trim() = "" Then
                MessageBox.Show("You must first set a cache location." & _
                                Environment.NewLine & "Please set one in View | Options.", "Cache location", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            'store selected node
            m_lReturn = StoreSelectedNodes(sNodeKeys, lvwDocList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'check the passwords of all the nodes - unless you are administrator
            If Not g_bUserIsAdministrator Then
                m_lReturn = VerifyPasswords(sNodeKeys)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    Return result
                End If
            End If

            'Go thru all select docs, calling the viewer for each
            For i As Integer = sNodeKeys.GetLowerBound(0) To sNodeKeys.GetUpperBound(0)
                If String.IsNullOrEmpty(sNodeKeys(i).Key) Then
 
                    MessageBox.Show("No document selected for printing.","Print Document", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sNodeKeys(i).Key.Substring(0, 1) = ACDocument Then

                    'get the doc num
                    m_lReturn = ExtractNumFromKey(sNodeKeys(i).Key, lDocNum)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    'get the page file paths

                    m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Log an error message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot print '" & sNodeKeys(i).Text & _
                                   "'. Failed to get pages.", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Else

                        If Information.IsArray(vPageArray) Then



                            sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))

                            ' Check to see if zip file...

                            m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZIPFile:=bZipped)

                            If Not CBool(m_lReturn) Then
                                'error - assume unzipped
                                bZipped = False
                            End If

                            ' cache each file

                            For iLoop1 As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0)


                                sFilename = CStr(vPageArray(iLoop1))
                                m_lReturn = CacheFile(oZipper:=m_oZipper, sFilename:=sFilename, sNewFilename:=sNewFilename, sCachePath:=m_sCachePath, bZipped:=bZipped)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                ' update the array of filenames

                                vPageArray(iLoop1) = sNewFilename

                            Next iLoop1

                        Else

                            ' Cache just the single file


                            sFilename = CStr(vPageArray)

                            ' Check to see if zip file...

                            m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZIPFile:=bZipped)

                            If Not m_lReturn Then
                                'error - assume unzipped
                                bZipped = False
                            End If

                            m_lReturn = CacheFile(oZipper:=m_oZipper, sFilename:=sFilename, sNewFilename:=sNewFilename, sCachePath:=m_sCachePath, bZipped:=bZipped)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' update the filename

                            vPageArray = sNewFilename

                        End If

                    End If

                    ' Work out if the files selected are TIF's or otherwise
                    If Information.IsArray(vPageArray) Then

                        bIsImage = (CBool(CStr(vPageArray(vPageArray.GetLowerBound(0)).EndsWith("TIF")).ToUpper()))
                    Else
                        bIsImage = (CBool(CStr(vPageArray.EndsWith("TIF")).ToUpper()))
                    End If

                    ' Its an image...
                    If bIsImage Then

                        '' Just one copy
                        'iTotalPages = 0
                        '
                        'For lIndex As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0)

                        '    'Total the number of pages in each document we are using.
                        '    
                        '    IkFile1.FileName = CStr(vPageArray.GetValue(lIndex)).Trim()
                        '    IkFile1.LoadPage = 0
                        '    IkFile1.GetImagefileType()
                        '    iTotalPages += IkFile1.FileMaxPage

                        '    IkCommon1.ImgHandle = IkFile1.ImgHandle
                        '    IkCommon1.FreeMemory()
                        '    IkFile1.ImgHandle = 0

                        'Next

                        '' File Array will contain either
                        '' several single page TIFs
                        '' or a single multi or single page TIF.
                        '' Find out which, set flag and determine page count.
                        '
                        'bMultiFileDocument = vPageArray.GetLowerBound(0) <> vPageArray.GetUpperBound(0)

                        'If bShowSetup Then

                        '    IkPrint1.Copies = 1
                        '    IkPrint1.PrintToFile = False
                        '    IkPrint1.MinPage = 1
                        '    IkPrint1.MaxPage = CShort(iTotalPages)
                        '    IkPrint1.FromPage = 1
                        '    IkPrint1.ToPage = CShort(iTotalPages)
                        '    IkPrint1.Options = &H100000 Or &H4S

                        '    m_lReturn = IkPrint1.PrintDlg()
                        '    If m_lReturn = 0 Then
                        '        GoTo Finally_Renamed
                        '    End If

                        'Else
                        '    IkPrint1.PrintFileName = "Default"
                        '    IkPrint1.PrintCreateDC(IMGKIT6Lib.PrintModeConstants.ikPrintFileName)
                        '    IkPrint1.FromPage = 1
                        '    IkPrint1.ToPage = CShort(iTotalPages)
                        'End If

                        '
                        'm_lReturn = g_oSplash.Show(DOCSplash_Message, "Printing document. Please wait...")

                        ''Retrieves paper size or available printing area
                        'm_lReturn = IkPrint1.GetPaperSize(IkPrint1.hDC, lLeft, lTop, lRight, lBottom, lWidth, lHeight, CShort(IMGKIT6Lib.OutPutDeviceModeConstants.ikPrinter))

                        'IkPrint1.DocName = sNodeKeys(i).Text
                        'm_lReturn = IkPrint1.PrintStartDoc()

                        'For lPage As Integer = 1 To iTotalPages

                        '    If lPage >= IkPrint1.FromPage And lPage <= IkPrint1.ToPage Then

                        '        'Open the required TIF file
                        '        If bMultiFileDocument Then

                        '            
                        '            IkFile1.FileName = CStr(vPageArray.GetValue(lPage - 1)).Trim()
                        '            IkFile1.LoadPage = 0

                        '        Else

                        '            
                        '            IkFile1.FileName = CStr(vPageArray.GetValue(0)).Trim()
                        '            IkFile1.LoadPage = CShort(lPage - 1)

                        '        End If

                        '        IkFile1.LoadFile(IMGKIT6Lib.LoadFileConstants.ikLoadTIFF)

                        '        m_lReturn = IkPrint1.PrintStartPage()
                        '        m_lReturn = IkPrint1.ImageOut(IkPrint1.hDC, IkFile1.ImgHandle, 0, 0, lRight - lLeft, lBottom - lTop, True, True, IMGKIT6Lib.OutPutDeviceModeConstants.ikPrinter)
                        '        m_lReturn = IkPrint1.PrintEndPage()

                        '        If IkFile1.ImgHandle <> 0 Then
                        '            IkCommon1.ImgHandle = IkFile1.ImgHandle
                        '            IkCommon1.FreeMemory()
                        '            IkFile1.ImgHandle = 0
                        '        End If
                        '    End If

                        'Next

                        'm_lReturn = IkPrint1.PrintEndDoc()
                        'IkPrint1.PrintDeleteDC()

                        '' Now we've printed create an event
                        '
                        'm_lReturn = g_oBusiness.CopyEventInSBO(lEventCnt:=lEventCnt, lDocNum:=lDocNum, dtEventDate:=DateTime.Now, sDescriptionPrefix:="Printed:")

                        '
                        'm_lReturn = g_oSplash.Hide()

                    Else
                        ' Print other document types
                        If bShowSetup Then

                            ' disable the range, file and selection option

                            'dlgMain.Flags = MSComDlg.PrinterConstants.cdlPDHidePrintToFile

                            dlgMainPrint.AllowSelection = False

                            'dlgMain.Flags = MSComDlg.PrinterConstants.cdlPDNoPageNums

                            ' Detect if the user presses cancel

                            ' dlgMain.CancelError = True

                            dlgMainPrint.PrinterSettings.Copies = 1

                            dlgMainPrint.ShowDialog()

                        End If



                        'rtbHidden.LoadFile(CStr(vPageArray.GetValue(vPageArray.GetLowerBound(0))))


                        m_lReturn = g_oSplash.Show(DOCSplash_Message, "Printing document. Please wait...")
                        ' Leave the return value, as this should not affect printing.

                        For iLoop1 As Integer = 1 To dlgMainPrint.PrinterSettings.Copies

                            If CBool(CStr(vPageArray(vPageArray.GetLowerBound(0)).EndsWith("PDF")).ToUpper()) Then
                                oDOCManagerInterface = New iDOCManager.Interface_Renamed
                                m_lReturn = oDOCManagerInterface.PrintPDFs(sNewFilename)
                                oDOCManagerInterface = Nothing
                            Else
                                m_lReturn = GetFileExtension4Excl(sNewFilename)
                                If sGetFileType = "XLS" Then
                                    m_lReturn = eXcelPrint(sNewFilename)
                                Else
                                    'can we try and unzip it here?
                                    Dim bZipSuccess As Boolean = False
                                    Dim unzipTargetFolder As String = ""
                                    Dim fInfo As New FileInfo(sNewFilename)
                                    unzipTargetFolder = fInfo.DirectoryName
                                    Dim sNewFileZip As String
                                    Dim sBackupFile As String
                                    sNewFileZip = Strings.Left(sNewFilename, InStrRev(sNewFilename, ".") - 1)
                                    'now rename it to a zip file
                                    sNewFileZip = sNewFileZip + "_copy.zip"
                                    fInfo.CopyTo(sNewFileZip, True)
                                    'rename to a backup file so we can delete it later
                                    sBackupFile = (unzipTargetFolder + "\" + fInfo.Name + ".backup")
                                    fInfo = New FileInfo(sBackupFile)
                                    If fInfo.Exists Then
                                        fInfo.Delete()
                                    End If
                                    fInfo = New FileInfo(sNewFilename)
                                    fInfo.MoveTo(sBackupFile)

                                    Dim sTarget As String = ""
                                    Dim sExt As String = ""
                                    sExt = Path.GetExtension(sNewFilename).ToUpper()

                                    'sNewFileZip is the file to unzip, unzipTargetFolder is the target directory for unzipping
                                    If sExt <> ".DOCX" And sExt <> ".XLSX" Then '(Unzip not required for .docx files as this scenario was occuring only for .doc)
                                        bZipSuccess = m_oZipper.UnZipFile(sNewFileZip, unzipTargetFolder)
                                    End If
                                    If Not bZipSuccess Then
                                        'If file is not unziped then file may be doc file without zipped, Recover original file from backup file 
                                        sBackupFile = Replace(sBackupFile, ".backup", "")
                                        fInfo = New FileInfo(sBackupFile)
                                        If fInfo.Exists Then
                                            fInfo.Delete()
                                        End If
                                        'Move file from backup to Original
                                        fInfo = New FileInfo(sNewFilename + ".backup")
                                        fInfo.MoveTo(sBackupFile)
                                        sTarget = sNewFilename
                                        fInfo = New FileInfo(sNewFileZip)
                                        fInfo.Delete()
                                        fInfo = Nothing
                                    Else
                                        Dim ofiles As String() = Directory.GetFiles(unzipTargetFolder)
                                        If ofiles IsNot Nothing AndAlso ofiles.GetUpperBound(0) > 0 Then
                                            If String.IsNullOrEmpty(sExt) Then
                                                sTarget = ofiles(0).ToString
                                            Else
                                                For iCounter As Integer = 0 To ofiles.Length
                                                    If Mid(ofiles(iCounter).ToString, InStrRev(ofiles(iCounter).ToString, "."), Len(ofiles(iCounter).ToString)).ToUpper = sExt.ToUpper Then
                                                        sTarget = ofiles(iCounter).ToString
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                    m_lReturn = PrintDocumentSilent(sTarget)
                                    fInfo = New FileInfo(sNewFileZip)
                                    fInfo.Delete()
                                    fInfo = New FileInfo(sBackupFile)
                                    fInfo.Delete()
                                    fInfo = New FileInfo(sTarget)
                                    fInfo.Delete()
                                    fInfo = Nothing
                                End If
                            End If
                        Next iLoop1

                        ' Now we've printed create an event

                        m_lReturn = g_oBusiness.CopyEventInSBO(lEventCnt:=lEventCnt, lDocNum:=lDocNum, dtEventDate:=DateTime.Now, sDescriptionPrefix:="Printed:")


                        m_lReturn = g_oSplash.Hide()
                        ' again, ignore the return value
                    End If

                End If

            Next i

            Return result

        Catch ex As Exception

            ' If cancel was pressed, then this is OK

            If Information.Err().Number = DialogResult.Cancel Then
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed print document(s).", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetOptions
    '
    ' Description: This section gets applicable options values from
    ' the registry.
    '
    ' ***************************************************************** '
    Private Sub GetOptions()

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sTmp As String = ""


        Try

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon

            'get whether to display folders in listview
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCDisplayFoldersOnRightKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCDisplayFoldersOnRightKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "Y"
                    m_bDocsOnly = False
                Case "N"
                    m_bDocsOnly = True
                Case Else
                    m_bDocsOnly = True
            End Select

            'get whether we start in home folder
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCStartHome, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCStartHome & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "Y"
                    m_bStartInHome = True
                Case "N"
                    m_bStartInHome = False
                Case Else
                    m_bStartInHome = False
            End Select

            'JH301198
            'get whether we show Keywords
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCExtrasKeywordsKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCExtrasKeywordsKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "N"
                    If m_iViewKeywords Then
                        m_iViewKeywords = False

                        'turn off keywords view
                        lvwKeyWords.Visible = False

                        mnuViewExtrasKeywords.Checked = False

                        'switch the button
                        'CType(tlbMain.Items.Item(ACKeyword), ToolStripButton).Checked = False
                        CType(tlbMain.Items.Find("_tlbMain_Button21", True)(0), ToolStripButton).Checked = False

                        'Resize all the forms controls
                        ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

                    End If
                Case Else
                    'forget it because the WAN options will only turn them off
            End Select

            'get whether we show Annotations
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCExtrasAnnotationsKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCExtrasAnnotationsKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "N"
                    If m_iViewAnnotations Then
                        m_iViewAnnotations = False

                        'turn off Annotations view
                        lvwAnnotations.Visible = False

                        mnuViewExtrasAnnotations.Checked = False

                        'switch the button
                        'CType(tlbMain.Items.Item(ACAnnotation), ToolStripButton).Checked = False
                        CType(tlbMain.Items.Find("_tlbMain_Button22", True)(0), ToolStripButton).Checked = False

                        'Resize all the forms controls
                        ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))

                    End If
                Case Else
                    'forget it because the WAN options will only turn them off
            End Select

            'JH051198
            'get whether we print in word
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCPrintWordKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCPrintWordKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "Y"
                    m_bPrintWord = True
                Case "N"
                    m_bPrintWord = False
                Case Else
                    m_bPrintWord = False
            End Select

            'get whether we view in word
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCViewWordKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCViewWordKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "Y"
                    m_bViewWord = True
                Case "N"
                    m_bViewWord = False
                Case Else
                    m_bViewWord = False
            End Select

            ' MS250900 get whether to fire-up keywords/annotations window
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCVAutoKeyword, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCVAutoKeyword & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "Y"
                    m_bAutoKeyword = True
                Case "N"
                    m_bAutoKeyword = False
                Case Else
                    m_bAutoKeyword = False
            End Select


            'get maximum number of folders returned
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMaxFoldersKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCMaxFoldersKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Dim dbNumericTemp As Double
            If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                m_lMaxFolders = CInt(sTmp)
            Else
                m_lMaxFolders = 0
            End If

            'get maximum number of folders returned from filter
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMaxFilterFoldersKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCMaxFilterFoldersKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Dim dbNumericTemp2 As Double
            If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                m_lMaxFilterFolders = CInt(sTmp)
            Else
                m_lMaxFilterFolders = 0
            End If

            'get maximum number of folders auto returned
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMaxAutoExpandKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCMaxAutoExpandKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Dim dbNumericTemp3 As Double
            If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                g_lMaxAutoExpand = CInt(sTmp)
            Else
                g_lMaxAutoExpand = DOCDefaultMaxAutoExpand
            End If

            'get whether to warn if scanning to external folder
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCScanToExternalWarning, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCScanToExternalWarning & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "Y"
                    m_bWarnScanToExternal = True
                Case "N"
                    m_bWarnScanToExternal = False
                Case Else
                    m_bWarnScanToExternal = True
            End Select

            'get whether to warn if moving/copying docs to non version 2 folder
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMoveToNonFolderWarning, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCMoveToNonFolderWarning & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "Y"
                    m_bWarnMoveToNonFolder = True
                Case "N"
                    m_bWarnMoveToNonFolder = False
                Case Else
                    m_bWarnMoveToNonFolder = True
            End Select

            'get cache location
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCCacheLocationKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCCacheLocationKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            If sTmp.Trim() = "" Then
                m_sCachePath = "c:\" & DOCCacheName
            Else
                m_sCachePath = sTmp.Trim()
            End If

            '    If (Right$(m_sCachePath, 1) <> "\") Then
            '        m_sCachePath = m_sCachePath & "\"
            '    End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: GetStartUpValues
    '
    ' Description: This section gets start up values for the app
    '
    ' ***************************************************************** '
    'Private Sub GetStartUpValues()
    '	Dim l2 As FormWindowState

    '	Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
    '	Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
    '	Dim eProductFamily As gPMConstants.PMEProductFamily
    '	Dim sTmp, sHotKey As String

    '	Try 

    '		eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser
    '		eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster

    '		' COMMON SETTINGS

    '		eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon 'pmeRSLCommon
    '		m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCWindowStateKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCWindowStateKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		Dim dbNumericTemp As Double
    '		If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
    '			l2 = CInt(sTmp)
    '			Me.WindowState = l2
    '		End If
    '		m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCFormWidthKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCFormWidthKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		Dim l As Integer

    '		Dim dbNumericTemp2 As Double
    '		If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
    '			l = CInt(sTmp)
    '			Me.Width = (l)
    '		End If
    '		m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCFormHeightKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCFormHeightKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		Dim dbNumericTemp3 As Double
    '		If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
    '			Me.Height = (CInt(sTmp))
    '		End If
    '		m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterHTopKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCSplitterHTopKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		Dim dbNumericTemp4 As Double
    '		If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
    '			imgSplitterH.Top = (CInt(sTmp))
    '		End If
    '		m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterVLeftKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCSplitterVLeftKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		Dim dbNumericTemp5 As Double
    '		If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
    '			imgSplitterV.Left = (CInt(sTmp))
    '		End If

    '		'WR77 Documaster Enhancements START
    '		m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterBCHTopKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCSplitterBCHTopKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		Dim dbNumericTemp6 As Double
    '		If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
    '			imgBCSplitterH.Top = (CInt(sTmp))
    '		End If

    '		'WR77 Documaster Enhancements END



    '		m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCExtrasAnnotationsKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCExtrasAnnotationsKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		Select Case sTmp
    '			Case "Y"
    '				m_iViewAnnotations = True
    '			Case "N"
    '				m_iViewAnnotations = False
    '			Case Else
    '				m_iViewAnnotations = True
    '		End Select

    '		m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCExtrasKeywordsKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCExtrasKeywordsKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		Select Case sTmp
    '			Case "Y"
    '				m_iViewKeywords = True
    '			Case "N"
    '				m_iViewKeywords = False
    '			Case Else
    '				m_iViewKeywords = True
    '		End Select

    '		'get the hotkey values
    '		For i As Integer = m_sHotKey.GetLowerBound(0) To m_sHotKey.GetUpperBound(0)


    '			sHotKey = "HotKey" & i
    '			m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sHotKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)
    '			m_sHotKey(i) = sTmp

    '		Next i

    '		' LOCAL MACHINE SETTINGS

    '		eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine

    '		m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCScanStationKey, r_sSettingValue:=sTmp)
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCScanStationKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If
    '		Select Case sTmp
    '			Case "Y", "y"
    '				Me.Visible = False
    '				mnuFileScan.Available = True
    '				fsep5.Available = True
    '				mnuPopScan.Available = True
    '				psep6.Available = True
    '				tlbMain.Items.Item("SCAN").Visible = True
    '			Case Else
    '				mnuFileScan.Available = False
    '				fsep5.Available = False
    '				mnuPopScan.Available = False
    '				psep6.Available = False
    '				tlbMain.Items.Item("SCAN").Visible = False
    '		End Select

    '	Catch excep As System.Exception




    '		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '		Exit Sub

    '	End Try

    'End Sub
    Private Sub GetStartUpValues()
        Dim l2 As FormWindowState

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sTmp, sHotKey As String

        Try

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster

            ' COMMON SETTINGS

            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon 'pmeRSLCommon
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCWindowStateKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCWindowStateKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Dim dbNumericTemp As Double
            If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                l2 = CInt(sTmp)
                Me.WindowState = l2
            End If
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCFormWidthKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCFormWidthKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Dim l As Integer

            Dim dbNumericTemp2 As Double
            If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                l = CInt(sTmp)
                Me.Width = (l)
            End If
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCFormHeightKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCFormHeightKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Dim dbNumericTemp3 As Double
            If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Me.Height = (CInt(sTmp))
            End If
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterHTopKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCSplitterHTopKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Dim dbNumericTemp4 As Double
            If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                imgSplitterH.Top = (CInt(sTmp))
            End If
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterVLeftKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCSplitterVLeftKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Dim dbNumericTemp5 As Double
            If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                imgSplitterV.Left = (CInt(sTmp))
            End If

            'WR77 Documaster Enhancements START
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterHTopKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCSplitterHTopKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Dim dbNumericTemp6 As Double
            If Double.TryParse(sTmp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                imgBCSplitterH.Top = (CInt(sTmp))
            End If

            'WR77 Documaster Enhancements END



            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCExtrasAnnotationsKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCExtrasAnnotationsKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "Y"
                    m_iViewAnnotations = True
                Case "N"
                    m_iViewAnnotations = False
                Case Else
                    m_iViewAnnotations = True
            End Select

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCExtrasKeywordsKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCExtrasKeywordsKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Select Case sTmp
                Case "Y"
                    m_iViewKeywords = True
                Case "N"
                    m_iViewKeywords = False
                Case Else
                    m_iViewKeywords = True
            End Select

            'get the hotkey values
            For i As Integer = m_sHotKey.GetLowerBound(0) To m_sHotKey.GetUpperBound(0)


                sHotKey = "HotKey" & i
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sHotKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)
                m_sHotKey(i) = sTmp

            Next i

            ' LOCAL MACHINE SETTINGS

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCScanStationKey, r_sSettingValue:=sTmp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCScanStationKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If
            Select Case sTmp
                Case "Y", "y"
                    Me.Visible = False
                    mnuFileScan.Available = True
                    fsep5.Available = True
                    mnuPopScan.Available = True
                    psep6.Available = True

                    'tlbMain.Buttons("SCAN").Visible = True
                Case Else
                    mnuFileScan.Available = False
                    fsep5.Available = False
                    mnuPopScan.Available = False
                    psep6.Available = False

                    'tlbMain.Buttons("SCAN").Visible = False
            End Select

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: SaveStartUpValues
    '
    ' Description: This section gets start up values for the app
    '
    ' ***************************************************************** '
    'Private Sub SaveStartUpValues()

    '	Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
    '	Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
    '	Dim eProductFamily As gPMConstants.PMEProductFamily
    '	Dim sTmp, sHotKey As String

    '	Try 

    '		eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser
    '		eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
    '		eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon

    '		'save current form width
    '		m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCFormWidthKey, v_sSettingValue:=CStr((Me.Width)), v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCFormWidthKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		'save current form height
    '		m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCFormHeightKey, v_sSettingValue:=CStr((Me.Height)), v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCFormHeightKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		'save current window state
    '		m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCWindowStateKey, v_sSettingValue:=CStr(Me.WindowState), v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCWindowStateKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If


    '		'save current horizontal splitter position
    '		m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterHTopKey, v_sSettingValue:=CStr((imgSplitterH.Top)), v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCSplitterHTopKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		'save current vertical splitter position
    '		m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterVLeftKey, v_sSettingValue:=CStr((imgSplitterV.Left)), v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCSplitterVLeftKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		'WR77 Documaster Enhancements START
    '		'save BC horizontal splitter position
    '		m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterBCHTopKey, v_sSettingValue:=CStr((imgBCSplitterH.Top)), v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCSplitterBCHTopKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If
    '		'WR77 Documaster Enhancements END

    '		'save whether annotations displayed
    '		If m_iViewAnnotations Then
    '			sTmp = "Y"
    '		Else
    '			sTmp = "N"
    '		End If

    '		m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCExtrasAnnotationsKey, v_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCExtrasAnnotationsKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If

    '		'save whether keywords displayed
    '		If m_iViewKeywords Then
    '			sTmp = "Y"
    '		Else
    '			sTmp = "N"
    '		End If

    '		m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCExtrasKeywordsKey, v_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCExtrasKeywordsKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '		End If


    '		'save the hotkey values
    '		For i As Integer = m_sHotKey.GetLowerBound(0) To m_sHotKey.GetUpperBound(0)

    '			sHotKey = "HotKey" & i
    '			m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sHotKey, v_sSettingValue:=m_sHotKey(i), v_sSubKey:=DOCStartUpSection)

    '		Next i

    '	Catch excep As System.Exception



    '		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '		Exit Sub

    '	End Try

    'End Sub
    Private Sub SaveStartUpValues()

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sTmp, sHotKey As String

        Try

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon

            'save current form width
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCFormWidthKey, v_sSettingValue:=CStr((Me.Width)), v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCFormWidthKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            'save current form height
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCFormHeightKey, v_sSettingValue:=CStr((Me.Height)), v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCFormHeightKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            'save current window state
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCWindowStateKey, v_sSettingValue:=CStr(Me.WindowState), v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCWindowStateKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


            'save current horizontal splitter position
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterHTopKey, v_sSettingValue:=CStr((imgSplitterH.Top)), v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCSplitterHTopKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            'save current vertical splitter position
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterVLeftKey, v_sSettingValue:=CStr((imgSplitterV.Left)), v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCSplitterVLeftKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            'WR77 Documaster Enhancements START
            'save BC horizontal splitter position
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCSplitterHTopKey, v_sSettingValue:=CStr((imgBCSplitterH.Top)), v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCSplitterHTopKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If
            'WR77 Documaster Enhancements END

            'save whether annotations displayed
            If m_iViewAnnotations Then
                sTmp = "Y"
            Else
                sTmp = "N"
            End If

            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCExtrasAnnotationsKey, v_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCExtrasAnnotationsKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            'save whether keywords displayed
            If m_iViewKeywords Then
                sTmp = "Y"
            Else
                sTmp = "N"
            End If

            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCExtrasKeywordsKey, v_sSettingValue:=sTmp, v_sSubKey:=DOCStartUpSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCExtrasKeywordsKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


            'save the hotkey values
            For i As Integer = m_sHotKey.GetLowerBound(0) To m_sHotKey.GetUpperBound(0)

                sHotKey = "HotKey" & i
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sHotKey, v_sSettingValue:=m_sHotKey(i), v_sSubKey:=DOCStartUpSection)

            Next i

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="SaveStartUpValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: GetSelectFolders
    '
    'JH051198
    ' Description: gets the select folders form, a different function
    ' so it can be accessed from a number of routines
    '
    '
    ' ***************************************************************** '
    Private Function GetSelectFolders(ByRef lFolderNum As Integer, ByRef lChildren As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Dim sKey As String = ""
        'Dim vTempArray As Variant
        'Dim bSplash As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Get the select folders class if it isn't initialised
            If Not m_bSelectFoldersInitialised Then
                m_oSelectFolders = New iDOCManager.frmSelectFolders()

                'm_lReturn = CType(m_oSelectFolders, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                m_lReturn = m_oSelectFolders.Initialise(Me)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    m_oSelectFolders.Dispose()
                    m_oSelectFolders = Nothing
                    Return result

                End If

                m_bSelectFoldersInitialised = True
            End If

            m_lReturn = m_oSelectFolders.SelectFolders(lFolderNum:=lFolderNum, lChildren:=lChildren) ', |                                    'vFolderArray:=vTempArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSelectFoldersFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSelectFolders", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            '    frmInterface.Refresh
            '
            '    'filltree
            '
            '    If IsArray(vTempArray) <> True Then
            '        'do nothing because user pressed cancel or selected nowt
            '        Exit Function
            '    Else
            '        m_vFolderArray = vTempArray
            '        'If children exist, this node has been previouly expanded
            '
            '        'if we are doing quite a few, splash
            '        If (tvwMain.SelectedItem.Children > 200) Then
            '            bSplash = True
            '            m_lReturn = g_oSplash.Show(DOCSplash_Retrieving)
            '        End If
            '
            '            'no, we need to add to whatever is there
            ''        If tvwMain.SelectedItem.Children > 0 Then
            ''            'need to delete whatever is in the tree first
            ''            m_lReturn = RemoveTreeChildren(tvw:=tvwMain, _
            '''                                    nod:=tvwMain.SelectedItem)
            ''
            ''            If m_lReturn <> PMTrue Then
            ''                iPMFunc.SetMousePointer (PMMouseReset)
            ''                iPMFunc.LogMessage _
            '''                    iType:=PMLogOnError, _
            '''                    sMsg:=gPMConstants.PMErrorText, _
            '''                    vApp:=ACApp, _
            '''                    vClass:=ACClass, _
            '''                    vMethod:="GetSelectFolders", _
            '''                    vErrNo:=Err.Number, _
            '''                    vErrDesc:=Err.Description
            ''                Exit Function
            ''
            ''            End If
            ''        End If
            '
            '        'hide splash
            '        If (bSplash = True) Then
            '            m_lReturn = g_oSplash.Hide()
            '        End If
            '
            '    End If
            '
            '    m_lReturn = PopulateTreeChildren(tvwMain, tvwMain.SelectedItem.Index, m_vFolderArray)
            '
            '    If m_lReturn <> PMTrue Then
            '        iPMFunc.SetMousePointer (PMMouseReset)
            '        iPMFunc.LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:=gPMConstants.PMErrorText, _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetSelectFolders", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '
            '    End If
            '    'expand
            '    tvwMain.SelectedItem.Expanded = True
            '
            '    iPMFunc.SetMousePointer (PMMouseReset)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'hide splash
            '    If (bSplash = True) Then
            '        m_lReturn = g_oSplash.Hide()
            '    End If

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSelectFoldersFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSelectFolders", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CountChildren
    '
    'JH051198
    ' Description: Count the children folders of a given folder
    ' returns number which is then compared to MaxAutoExpand
    '
    '
    ' ***************************************************************** '
    Public Function CountChildren(ByRef lFoldNum As Integer, ByRef lChildren As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.CountChildren(lFoldNum, lChildren)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CountChildrenFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CountChildren", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CountChildrenFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CountChildren", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetMyFolders
    '
    ' JH051198 added because 'Add Folders to View...' node makes more code
    '
    ' Description: Do the stuff for double-click or hitting 'enter'
    '               on a treeview or menu clicked, passed tvw control
    '               and boolean value bAddToView for whether to compare
    '               maxautoexpand with counted children
    '
    '
    ' ***************************************************************** '
    Private Function GetMyFolders(ByRef tvw As TreeView, ByRef bAddToView As Boolean, ByRef sTempKey As String, Optional ByRef bMenu As Boolean = False) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lFoldNum As Integer
        Dim sNodeKey() As DOCConst.DOCNodes = Nothing
        Dim lChildren As Integer
        'Dim sTempKey As String

        Dim iIndex As Integer
        Dim iParentKey As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'to stop the user selecting the 'add to view' node again
            'by accident - or the function will add folders as
            'children of the 'add to view' node, which we don't want!
            'tvw.Enabled = False
            'no I don't like the way that looks, so just reselect
            'before showing

            'JH051198 if it is 'Add Folders to View...' node that was clicked
            'select the parent

            If tvw.Nodes.Find(sTempKey, True)(0).Name.Substring(0, 3) = "ADD" Then
                sTempKey = tvw.Nodes.Find(sTempKey, True)(0).Parent.Name
                'move this further on  
                'tvw.Nodes(sTempKey$).Selected = True
                bAddToView = True
            Else
                'If children exist, this node has been previouly expanded so can leave.
                If (tvw.Nodes.Find(sTempKey, True)(0).GetNodeCount(False) > 0) And Not bAddToView Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    tvw.Enabled = True
                    Return result
                End If
            End If

            'check the passwords of the node - unless you are adminstrator
            If Not g_bUserIsAdministrator Then

                ReDim sNodeKey(0)
                sNodeKey(0).Key = sTempKey
                sNodeKey(0).Text = tvw.Nodes.Find(sTempKey, True)(0).Text

                m_lReturn = VerifyPasswords(sNodeKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    tvw.Enabled = True
                    Return result
                End If
            End If

            'JH051198 this is where we decide whether to show select folders dialogue

            'Get the folder num from the selected node key
            m_lReturn = ExtractNumFromKey(tvw.Nodes.Find(sTempKey, True)(0).Name, lFoldNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                tvw.Enabled = True
                Return result
            End If

            m_lReturn = CountChildren(lFoldNum, lChildren)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                tvw.Enabled = True
                Return result
            End If
            '
            'do this in the menu option - not on double-click
            If bMenu Then
                If tvw.Nodes.Find(sTempKey, True)(0).GetNodeCount(False) >= lChildren Then
                    'they've already got them all
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    MessageBox.Show("No More Folders to Add", "Add Folders to View", MessageBoxButtons.OK)
                    tvw.Enabled = True
                    Return result
                End If
            End If

            'just to check
            tvw.SelectedNode = tvw.Nodes.Find(sTempKey, True)(0)

            If lChildren > g_lMaxAutoExpand Or bAddToView Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

                m_lReturn = GetSelectFolders(lFoldNum, lChildren)

                tvw.Enabled = True
                Return result

            End If

            'if we are not displaying folders in the document list view, then preceding
            'click event will not have gotten the folder list, so we'd best get it now
            If m_bDocsOnly Then
                '
                '   doing this outside the if statement now
                '        'Get the folder num from the selected node key
                '        m_lReturn = ExtractNumFromKey(tvw.SelectedItem.Key, lFoldNum&)
                '
                '        If (m_lReturn& <> PMTrue) Then
                '            iPMFunc.SetMousePointer (PMMouseReset)
                '            Exit Sub
                '        End If

                'Get the folders in the selected folder
                m_lReturn = GetFolderList(lFoldNum, "", m_vFolderArray)

            End If

            iIndex = tvw.Nodes.Find(sTempKey, True)(0).Index
            If Not tvw.Nodes.Find(sTempKey, True)(0).Parent Is Nothing Then
                iParentKey = tvw.Nodes.Find(sTempKey, True)(0).Parent.Name
                m_lReturn = PopulateTreeChildren(tvw, iIndex, m_vFolderArray, iParentKey)
                tvw.Nodes.Find(sTempKey, True)(0).Parent.Nodes(iIndex).Expand()
            Else
                m_lReturn = PopulateTreeChildren(tvw, iIndex, m_vFolderArray)
                tvw.Nodes(iIndex).Expand()
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            tvw.Enabled = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetMyFolders", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: AddToViewNode
    '
    ' Description: finds the next ADD number for the key and adds
    '               the special node to the parent node
    '
    '
    ' ***************************************************************** '
    Public Function AddToViewNode(ByRef tvw As TreeView, ByRef sKey As String) As gPMConstants.PMEReturnCode
        Dim Err_FindAddNumber As Boolean = False
        Dim Err_AddToViewNode As Boolean = False  'iIndex As Integer) As Long

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sAddNumber As String = ""
        Dim Node As TreeNode
        Dim bFoundIt As Boolean

        Try
            Err_AddToViewNode = True
            Err_FindAddNumber = False

            result = gPMConstants.PMEReturnCode.PMTrue

            'find the next ADD number

            For lAddNumber As Integer = 0 To (tvw.Nodes.Count - 1)

                'sAddNumber = "ADD" & lAddNumber  'Commented by Tariq
                sAddNumber = "ADD" & tvw.Nodes.Find(sKey, True)(0).Parent.Name.Replace(" ", "")
                bFoundIt = True
                'if it's already there it will error
                Err_FindAddNumber = True
                Err_AddToViewNode = False

                'add the 'Add Folders to View' node
                'Node = tvw.Nodes.Find(sKey, True)(0).Parent.Nodes.Insert(tvw.Nodes.Find(sKey, True)(0).Index - 1, sAddNumber, "Add Folders to View...", "IMGADDFOLDER")
                If tvw.Nodes.Find(sKey, True)(0).Parent.Nodes.Find(sAddNumber, False).Length = 0 Then
                    'Node = tvw.Nodes.Find(sKey, True)(0).Parent.Nodes.Insert(tvw.Nodes.Find(sKey, True)(0).Index + 1, sAddNumber, "Add Folders to View...", "IMGADDFOLDER")
                    If Me.IsHandleCreated Then
                        Me.Invoke(New AddTreeNode(AddressOf AddNode), sAddNumber, "Add Folders to View...", "IMGADDFOLDER", tvw.Nodes.Find(sKey, True)(0).Parent)
                    Else
                        Node = tvw.Nodes.Find(sKey, True)(0).Parent.Nodes.Add(sAddNumber, "Add Folders to View...", "IMGADDFOLDER")
                    End If
                End If

                'tvwLast, _
                ''tvwFirst, _
                '
                Err_AddToViewNode = True
                Err_FindAddNumber = False

                If bFoundIt Then Exit For

            Next lAddNumber

            tvw.Nodes.Find(sAddNumber, True)(0).EnsureVisible()

            Return result

        Catch excep As System.Exception
            If Not Err_FindAddNumber And Not Err_AddToViewNode Then
                Throw excep
            End If

            If Err_FindAddNumber Then


                'the node exists
                bFoundIt = False

                Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")

            End If
            If Err_AddToViewNode Or Err_FindAddNumber Then


                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddToViewNodeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToViewNode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End If
        End Try
    End Function

    ' MS 22/05/01

    ' ***************************************************************** '
    ' Name: DestroyFolderAndAllContents
    '
    ' Description: Clears down the cache directory by deleting
    '              all the documents and folders in cache dir
    '
    '
    ' ***************************************************************** '

    Private Function DestroyFolderAndAllContents(ByVal sFolder As String, Optional ByVal v_lDepth As Integer = 0) As Integer
        Dim result As Integer = 1
        Dim rootDir As System.IO.DirectoryInfo

        Try
            rootDir = New DirectoryInfo(sFolder)
            If Not rootDir Is Nothing Then
                For Each subDir As DirectoryInfo In rootDir.GetDirectories()
                    If Not subDir Is Nothing Then
                        subDir.Delete(True)
                        rootDir.Refresh()
                        Application.DoEvents()  ' allow other processes to happen
                    End If
                Next

            End If
            Return result

        Catch ex As Exception
            result = 0
            ' path/access error or permission denied error
            If (Information.Err().Number = 75) Or (Information.Err().Number = 70) Then
                MessageBox.Show("The Cache directory maybe in use. Close down any" & Strings.Chr(13) & Strings.Chr(10) & _
                                "applications which maybe accessing it, 'Refresh' and try again" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                "Error Number: " & CStr(Information.Err().Number) & Strings.Chr(13) & Strings.Chr(10) & "Description: " & ex.Message & Strings.Chr(13) & Strings.Chr(10) & _
                                "App: " & ACApp & "." & ACClass & Strings.Chr(13) & Strings.Chr(10) & "Method: DestroyFolderAndAllContents", "Clear DocuMaster Cache Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error attempting to delete folder or files in." & sFolder, vApp:=ACApp, vClass:=ACClass, vMethod:="DestroyFolderAndAllContents", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return result
        End Try



    End Function

    Public Function ArchiveDocument() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' clear down current doc in viewwer as move will put it in new folder and info will be wrong
            'Unload m_oViewer.ActiveForm
            ' get doc num as global
            'm_lReturn = ExtractNumFromKey(lvwDocList.FocusedItem.Name, g_lArchiveDocNum)
            m_lReturn = ExtractNumFromKey(lvwDocList.SelectedItems(0).Name, g_lArchiveDocNum)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' failed somehow
                MessageBox.Show("Failed to get document number", "Archive Document Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            'trigger the cut event
            tlbMain_ButtonClick(tlbMain.Items.Item(3), New EventArgs())

            'Get the select folders class if it isn't initialised

            If Not m_bSelectFoldersInitialised Then
                m_oSelectFolders = New iDOCManager.frmSelectFolders()

                m_lReturn = m_oSelectFolders.Initialise(Me)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    m_oSelectFolders.Dispose()
                    m_oSelectFolders = Nothing
                    Return result

                End If

                m_bSelectFoldersInitialised = True
            End If

            ' send as zeros when Archiving.
            ' The correct folder will be determined in function below
            m_lReturn = m_oSelectFolders.SelectFolders(lFolderNum:=0, lChildren:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ArchiveDocumentFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If g_lArchiveDocNum > 0 Then
                MessageBox.Show("Click on a folder to archive", "Archive Document", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Focus()
            End If



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ArchiveDocumentFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ExpandFolder
    '
    '       Function which expands a folder
    '       Used to expand any clients in current treeview
    '
    ' ***************************************************************** '
    Public Function ExpandFolder(ByRef tvw As TreeView, ByRef sTempKey As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lFoldNum As Integer
        Dim sNodeKey() As DOCConst.DOCNodes = Nothing
        Dim lChildren As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            'cannot expand on 'ADD to folders' folder so exit
            'If tvw.Nodes.Item(sTempKey).Name.Substring(0, 3) = "ADD" Then
            If tvw.Nodes.Find(sTempKey, True)(0).Name.Substring(0, 3) = "ADD" Then
                Return result
            End If

            'check the passwords of the node - unless you are adminstrator
            If Not g_bUserIsAdministrator Then

                ReDim sNodeKey(0)
                sNodeKey(0).Key = sTempKey
                sNodeKey(0).Text = tvw.Nodes.Find(sTempKey, True)(0).Text

                m_lReturn = VerifyPasswords(sNodeKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    tvw.Enabled = True
                    Return result
                End If
            End If

            If tvw.Nodes.Find(sTempKey, True)(0).Parent Is Nothing Then
                ' don't want to expand a top-level company folder as it will expand everything!
                ' just expand from client level folders
                Return result
            End If

            'Get the folder num from the selected node key
            m_lReturn = ExtractNumFromKey(tvw.Nodes.Find(sTempKey, True)(0).Name, lFoldNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                tvw.Enabled = True
                Return result
            End If

            m_lReturn = CountChildren(lFoldNum, lChildren)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                tvw.Enabled = True
                Return result
            End If

            'if we are not displaying folders in the document list view, then preceding
            'click event will not have gotten the folder list, so we'd best get it now
            If m_bDocsOnly Then


                'Get the folders in the selected folder
                m_lReturn = GetFolderList(lFoldNum, "", m_vFolderArray)

            End If

            m_lReturn = PopulateTreeChildren(tvw, tvw.Nodes.Find(sTempKey, True)(0).Index, m_vFolderArray)

            'expand it - hooray!
            tvw.Nodes.Find(sTempKey, True)(0).Expand()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            tvw.Enabled = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ExpandFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function ExpandFolderAll(ByRef tvw As TreeView, ByRef sTempKey As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lFoldNum As Integer
        Dim sNodeKey() As DOCConst.DOCNodes = Nothing
        Dim lChildren As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            'cannot expand on 'ADD to folders' folder so exit
            'If tvw.Nodes.Item(sTempKey).Name.Substring(0, 3) = "ADD" Then
            If tvw.Nodes.Find(sTempKey, True)(0).Name.Substring(0, 3) = "ADD" Then
                Return result
            End If

            'check the passwords of the node - unless you are adminstrator
            If Not g_bUserIsAdministrator Then

                ReDim sNodeKey(0)
                sNodeKey(0).Key = sTempKey
                sNodeKey(0).Text = tvw.Nodes.Find(sTempKey, True)(0).Text

                m_lReturn = VerifyPasswords(sNodeKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'user could n't be bothered to supply the password - or got it wrong
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    tvw.Enabled = True
                    Return result
                End If
            End If


            'Get the folder num from the selected node key
            m_lReturn = ExtractNumFromKey(tvw.Nodes.Find(sTempKey, True)(0).Name, lFoldNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                tvw.Enabled = True
                Return result
            End If

            m_lReturn = CountChildren(lFoldNum, lChildren)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                tvw.Enabled = True
                Return result
            End If

            'if we are not displaying folders in the document list view, then preceding
            'click event will not have gotten the folder list, so we'd best get it now
            If m_bDocsOnly Then


                'Get the folders in the selected folder
                m_lReturn = GetFolderList(lFoldNum, "", m_vFolderArray)

            End If

            m_lReturn = PopulateTreeChildren(tvw, tvw.Nodes.Find(sTempKey, True)(0).Index, m_vFolderArray, tvw.Nodes.Find(sTempKey, True)(0).Parent.Name)

            'expand it - hooray!
            If Not tvw.Nodes.Find(sTempKey, True)(0) Is Nothing Then
                tvw.Nodes.Find(sTempKey, True)(0).ExpandAll()
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            tvw.Enabled = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ExpandFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetAllowCopyPasteOption(ByRef bAllowCopyPaste As Boolean) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Dim sOptionValue As String = ""

        Dim oBusinessBusiness As bDOCOptions.Business

        Dim oObjMgr As bObjectManager.ObjectManager

        Try


#If PD_EARLYBOUND = 1 Then

			Set oObjMgr = New bObjectManager.ObjectManager
#Else
            oObjMgr = New bObjectManager.ObjectManager()
#End If


            m_lReturn = oObjMgr.Initialise(sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            Dim temp_oBusinessBusiness As Object
            m_lReturn = oObjMgr.GetInstance(temp_oBusinessBusiness, "bDOCOptions.Business", vInstanceManager:="ClientManager")
            oBusinessBusiness = temp_oBusinessBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            m_lReturn = oBusinessBusiness.GetOption(sOptionName:=OPTIONS_VIEWER_ALLOW_CUT_PASTE, sOptionValue:=sOptionValue)

            oBusinessBusiness = Nothing
            oObjMgr = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            bAllowCopyPaste = sOptionValue = "1"


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowCopyPasteOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub tvwMain_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwMain.AfterSelect
        Dim Node As TreeNode = eventArgs.Node
        m_bIsMoveDocList = True
    End Sub
    'WR77 Documaster Enhancements START
    Private Sub tlbBCDocsButtons_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _tlbBCDocsButtons_Button1.Click, _tlbBCDocsButtons_Button2.Click, _tlbBCDocsButtons_Button3.Click, _tlbBCDocsButtons_Button4.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Const kMethodName As String = "tlbBCDocsButtons_ButtonClick"
        Try
            Dim m_lReturn As gPMConstants.PMEReturnCode


            Select Case Button.Tag
                Case ACBCEmail
                    'Email the document(s)
                    m_lReturn = EmailBCDocuments()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Function tlbBCDocsButtons_ButtonClick Failed")
                    End If
                Case ACBCArchive
                    'Archive the document(s)
                    m_lReturn = ArchiveBCDocuments()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Function tlbBCDocsButtons_ButtonClick Failed")
                    End If
                Case ACBCExport
                    'Export the document(s)
                    'TODO
                    m_lReturn = ExportBCDocuments()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Function tlbBCDocsButtons_ButtonClick Failed")
                    End If
                Case ACBCREMOVE
                    'Remove the selected item(s) from the ListView
                    m_lReturn = RemoveItems()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Function tlbBCDocsButtons_ButtonClick Failed")
                    End If
            End Select

            'Set up the controls positions, according to the two splitter bars
            ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((imgBCSplitterH.Top)))


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

        Finally

        End Try
        Exit Sub
    End Sub

    Private Sub lvwBCDocs_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwBCDocs.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwBCDocs.Columns(eventArgs.Column)

        Const kMethodName As String = "lvwBCDocs_ColumnClick"

        Try


            ' Flip sort way around
            If ListViewHelper.GetSortOrderProperty(lvwBCDocs) = SortOrder.Ascending Then
                ListViewHelper.SetSortOrderProperty(lvwBCDocs, SortOrder.Descending)
                ' Set the value for the sorting of the date
                bSortAccending = False
            Else
                ListViewHelper.SetSortOrderProperty(lvwBCDocs, SortOrder.Ascending)
                ' date sorting order
                bSortAccending = True
            End If

            ' Set Sorted to True to sort the list.
            ListViewHelper.SetSortedProperty(lvwBCDocs, True)
            ListViewHelper.SetSortKeyProperty(lvwBCDocs, ColumnHeader.Index + 1 - 1)



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Exit Sub
    End Sub



    Private Sub imgBCSplitterH_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgBCSplitterH.MouseDown
        Dim Catch_Renamed As Boolean = False
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        Const kMethodName As String = "imgBCSplitterH_MouseDown"
        Try
            Catch_Renamed = True



            'Size the picture splitter to same sixe as the splitter bar being moved
            With imgBCSplitterH
                picSplitter.SetBounds(.Left, .Top, ((.Width) - 20), .Height / 2)
            End With

            picSplitter.Visible = True

            m_bResizing = True

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed
            If Catch_Renamed Then

                ' DO Not Call any functions before here or the error will be lost
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=excep)
            End If
Finally_Renamed:
        End Try

    End Sub

    Private Sub imgBCSplitterH_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgBCSplitterH.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        Const kMethodName As String = "imgBCSplitterH_MouseMove"
        Dim lPos As Integer
        Try



            'Move the picture splitter to its new location
            If m_bResizing Then

                lPos = CInt(Y + (imgBCSplitterH.Top))

                m_BCDocsFrameHeight = lPos

                If lPos < ACVertSplitLimit Then
                    picSplitter.Top = (ACVertSplitLimit)
                Else
                    If lPos > (Me.Height) - ACVertSplitLimit Then
                        picSplitter.Top = Me.Height - (ACVertSplitLimit)
                    Else
                        picSplitter.Top = (lPos)
                    End If
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

        Finally

        End Try
        Exit Sub
    End Sub

    Private Sub imgBCSplitterH_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgBCSplitterH.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)

        Const kMethodName As String = "imgBCSplitterH_MouseUp"
        Try


            'Resize all the forms Controls
            ResizeControls(CInt((imgSplitterV.Left)), CInt((imgSplitterH.Top)), CInt((picSplitter.Top)))

            picSplitter.Visible = False
            m_bResizing = False


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

        Finally

        End Try
        Exit Sub
    End Sub

    Private Function CheckBCPartiesMatch(ByRef cnt As ListView) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "CheckBCPartiesMatch"

        Dim lDocNum, lFolderNum, lDrawerNum, lPartyCnt As Integer
        Dim sFolderCode, sDrawerExCode As String
        Dim bPartyExist As Boolean
        Dim vPartyCnt() As Object

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            lPartyCnt = 0

            If cnt.Items.Count = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            For Each liBCDoc As ListViewItem In cnt.Items
                lDocNum = 0
                lFolderNum = 0
                sFolderCode = ""
                lDrawerNum = 0
                sDrawerExCode = ""
                lPartyCnt = 0
                m_lReturn = ExtractNumFromKey(liBCDoc.Name, lDocNum)


                m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Document, lNodeNum:=lDocNum, lParentNum:=lFolderNum)
                '.. and it's ex code

                m_lReturn = g_oBusiness.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, sExCode:=sFolderCode)

                'now get folder's parent i.e. drawer num

                m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, lParentNum:=lDrawerNum)
                ' .. and it's ex code

                m_lReturn = g_oBusiness.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lDrawerNum, sExCode:=sDrawerExCode)
                sDrawerExCode = sDrawerExCode.Trim()
                sFolderCode = sFolderCode.Trim()

                If sDrawerExCode = "" Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Exit For
                Else
                    If Information.IsArray(vPartyCnt) Then
                        For lPartyCnt = 0 To vPartyCnt.GetUpperBound(0)
                            If sDrawerExCode <> CStr(vPartyCnt(lPartyCnt)) Then
                                bPartyExist = True
                                Exit For
                            End If
                        Next
                        If bPartyExist Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        Else
                            If lPartyCnt = 0 Then
                                ReDim vPartyCnt(lPartyCnt)
                            Else
                                ReDim Preserve vPartyCnt(lPartyCnt)
                            End If
                            vPartyCnt(lPartyCnt) = sDrawerExCode

                            lPartyCnt += 1
                        End If
                    Else
                        ReDim vPartyCnt(lPartyCnt)
                        vPartyCnt(lPartyCnt) = sDrawerExCode
                        lPartyCnt += 1
                    End If
                End If
            Next liBCDoc



        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Private Function EmailBCDocuments() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oPMMAPI As Object
        Dim ofrmEmail As frmEmail
        Dim bZipped As Boolean

        Dim sAddress() As Object
        Dim vPageArray() As Object

        Dim sNewFilename, sFolderCode, sDrawerExCode, sDescription As String
        Dim sSubject As New StringBuilder

        Dim sMessage, sUnZipPath, sSendFile, sTempDir, sFilename As String

        Dim lPages, lEventCnt, lDocNum, lPartyCnt, lFolderNum, lDrawerNum As Integer
        Dim sNodeKeys() As DOCConst.DOCNodes = Nothing
        Dim bFirstElement As Boolean
        Dim vInsuranceFolderCnt As Integer
        Dim vClaimCnt, sInputReceived As String
        Dim dFilesSize As Double
        Dim bAllFilesEMailed As Boolean
        Dim iValidListCnt As Integer
        Dim sTempFolderCode, sTempDrawerExCode As String
        Dim lTempDocNum As Integer
        Dim bValidMultipledoc As Boolean

        Const kMethodName As String = "EmailBCDocuments"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ofrmEmail = New frmEmail()
            ReDim sNodeKeys(0)
            ' First we loop round to get all the document in the list and build a list of attachment

            For Each liBCDoc As ListViewItem In lvwBCDocs.Items
                'Get the doc num
                m_lReturn = ExtractNumFromKey(liBCDoc.Name, lDocNum)

                'Get the page file path
                m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Function GetPageList Failed")
                Else
                    'Get the first filename
                    sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))
                End If

                ' Check to see if zip file...
                m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZIPFile:=bZipped)
                If Not CBool(m_lReturn) Then
                    MessageBox.Show("File " & sFilename & " does not exist physically. Please remove this file to proceed further.", " Email Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result
                End If
            Next liBCDoc

            For Each liBCDoc As ListViewItem In lvwBCDocs.Items
                'Get the doc num
                m_lReturn = ExtractNumFromKey(liBCDoc.Name, lDocNum)

                ' save the subject text
                If sSubject.ToString().Trim().Length = 0 Then
                    sSubject = New StringBuilder(ListViewHelper.GetListViewSubItem(liBCDoc, k_BCDocFileName).Text)
                Else
                    sSubject.Append("; " & ListViewHelper.GetListViewSubItem(liBCDoc, k_BCDocFileName).Text)
                End If

                'Get the page file path
                m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Function GetPageList Failed")
                Else
                    'Get the first filename
                    sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))
                End If

                'Populate page files and
                For lCnt As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0) - 1
                    ofrmEmail.txtFile.Text = ofrmEmail.txtFile.Text & CStr(vPageArray(lCnt)).Substring(CStr(vPageArray(lCnt)).Length - 6) & ", "
                Next lCnt

                'single page or last page
                If ofrmEmail.txtFile.Text.Trim().Length > 0 Then
                    ofrmEmail.txtFile.Text = ofrmEmail.txtFile.Text & "; " & CStr(vPageArray(vPageArray.GetUpperBound(0))).Substring(CStr(vPageArray(vPageArray.GetUpperBound(0))).Length - 6)
                Else
                    ofrmEmail.txtFile.Text = CStr(vPageArray(vPageArray.GetUpperBound(0))).Substring(CStr(vPageArray(vPageArray.GetUpperBound(0))).Length - 6)
                End If
            Next liBCDoc

            ofrmEmail.txtSubject.Text = sSubject.ToString()
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'show the screen
            ofrmEmail.ShowDialog()

            'Pass back the entered info
            'sSendTo = ofrmEmail.SendTo

            sAddress = VB6.CopyArray(ofrmEmail.Addresses)
            sSubject = New StringBuilder(ofrmEmail.SendSubject)
            'add a message if required, also inform recipient total pages attached
            sMessage = ofrmEmail.SendNote & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                       " (" & CStr(lPages + 1) & " page document attached)"

            If ofrmEmail.Status <> gPMConstants.PMEReturnCode.PMOK Then
                ofrmEmail.Close()
                Return result
            End If

            ofrmEmail.Close()

            oPMMAPI = New iPMMapi.PMMAPI()

            If oPMMAPI.Session() Is Nothing Then
                m_lReturn = oPMMAPI.Initialise
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            End If

            'add a message to the Message collection
            m_lReturn = oPMMAPI.messages.AddMessage(v_vSubject:=sSubject.ToString(), v_vNoteText:=sMessage)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'The LastItem is a Message

            For Each sAddress_item As Object In sAddress
                'Add a Recipient
                m_lReturn = oPMMAPI.messages.LastItem.Recipients.AddRecipient(v_vName:=sAddress_item, v_vRecipientType:=gPMConstants.PMEMapiRecipientTypes.pmeMapiToList, v_vAddressBook:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            Next sAddress_item

            For Each liBCDoc As ListViewItem In lvwBCDocs.Items
                'Get the doc num
                m_lReturn = ExtractNumFromKey(liBCDoc.Name, lDocNum)

                'Get the page file path
                m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Function GetPageList Failed")
                Else
                    'Get the first filename
                    sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))
                End If

                ' Check to see if zip file...
                m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZIPFile:=bZipped)

                If m_lReturn Then
                    'get the system temp dir "<Drive>:\Temp" and extract the zip tfiles there
                    sTempDir = Interaction.Environ("Temp") & "\"
                    'attach each page of the document
                    For Each vPageArray_item As Object In vPageArray
                        If bZipped Then
                            'get the filename and unzip into the temp dir
                            sFilename = sTempDir & CStr(vPageArray_item).Substring(CStr(vPageArray_item).Length - 6)
                            m_lReturn = m_oZipper.UnZipFile(CStr(vPageArray_item), sFilename)
                        Else
                            'get file from server unc
                            sFilename = CStr(vPageArray_item)
                        End If
                        If FileSystem.Dir(sFilename, FileAttribute.Normal) <> Nothing Then
                            'get the file size in bytes
                            dFilesSize += CInt((New FileInfo(sFilename)).Length)
                        End If

                        If dFilesSize >= (10485760) Then
                            If (MessageBox.Show("Attached files(s) size is increasing to the prescribed limit. Do you want to continue?", "Document Email", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) = System.Windows.Forms.DialogResult.Yes Then
                                Exit For
                            Else
                                Return result
                            End If
                        Else
                            'call the Attachment the same as the document
                            m_lReturn = oPMMAPI.messages.LastItem.Attachments.AddAttachment(v_vName:=sFilename, v_vPath:=sFilename)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return result
                            End If
                        End If
                    Next vPageArray_item

                    If bFirstElement Then
                        bFirstElement = False
                    Else
                        ReDim Preserve sNodeKeys(sNodeKeys.GetUpperBound(0) + 1)
                    End If

                    sNodeKeys(sNodeKeys.GetUpperBound(0)).Key = liBCDoc.Name
                    sNodeKeys(sNodeKeys.GetUpperBound(0)).Text = liBCDoc.Text
                    bAllFilesEMailed = True
                End If
            Next liBCDoc

            If bAllFilesEMailed Then
                m_lReturn = oPMMAPI.messages.LastItem.Send
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to send email ", "E-mail Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)


                    m_lReturn = DeleteUnzipFiles(lvwBCDocs)

                    Return result
                End If
                bValidMultipledoc = True
                For Each liBCDoc As ListViewItem In lvwBCDocs.Items
                    sTempDrawerExCode = ""
                    sTempFolderCode = ""
                    lTempDocNum = 0

                    'Get the doc num
                    m_lReturn = ExtractNumFromKey(liBCDoc.Name, lTempDocNum)

                    'it is now in the new destination folder, get document's folder num
                    m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Document, lNodeNum:=lTempDocNum, lParentNum:=lFolderNum)
                    '.. and it's ex code
                    m_lReturn = g_oBusiness.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, sExCode:=sTempFolderCode)

                    'now get folder's parent i.e. drawer num
                    m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, lParentNum:=lDrawerNum)
                    ' .. and it's ex code
                    m_lReturn = g_oBusiness.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lDrawerNum, sExCode:=sTempDrawerExCode)

                    If String.IsNullOrEmpty(sDrawerExCode) Then
                        sDrawerExCode = sTempDrawerExCode.Trim()
                    End If

                    If String.IsNullOrEmpty(sFolderCode) Then
                        sFolderCode = sTempFolderCode.Trim()
                    End If

                    'check if external code exist
                    If (sTempDrawerExCode <> "") Or (sTempFolderCode <> "") Then
                        lDocNum = lTempDocNum
                        If Not (sTempDrawerExCode = sDrawerExCode And sTempFolderCode = sFolderCode) Then
                            bValidMultipledoc = False
                        End If
                        iValidListCnt += 1
                        '                If iValidListCnt > 1 Then
                        '                    Exit For
                        '                End If
                    End If
                Next liBCDoc

                If sDrawerExCode = "" Then
                    lPartyCnt = 0
                Else
                    lPartyCnt = ToSafeLong(sDrawerExCode)
                    If sFolderCode = "GENERAL" Then
                        sFolderCode = ""
                    End If
                End If

                'if it's a claims folder, then truncate the C prefix
                If sFolderCode.Substring(0, 1) = "C" Then
                    vClaimCnt = Mid(sFolderCode, 2)
                Else

                    vClaimCnt = Nothing
                End If

                If sFolderCode = "" Then

                    vInsuranceFolderCnt = Nothing
                Else
                    If sFolderCode.Substring(0, 1) = "C" Then

                        m_lReturn = g_oBusiness.GetInsuranceFolderCnt(vClaimCnt:=vClaimCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt)
                    Else
                        vInsuranceFolderCnt = CInt(sFolderCode)
                    End If
                End If

                If lEventCnt = 0 Then
                    sInputReceived = Interaction.InputBox("Enter a description for this Event", " Email files")
                    lEventCnt += 1
                End If

                If iValidListCnt = 1 Then
                    sDescription = "Emailed file - " & sInputReceived

                    m_lReturn = g_oBusiness.CreateEventInSBO(lEventCnt:=0, lPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=DBNull.Value, vClaimCnt:=vClaimCnt, lDocNum:=lDocNum, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentTypeId:=5, vReportTypeId:=DBNull.Value, lEventTypeId:=10, dtEventDate:=DateTime.Today, sDescription:=sDescription)
                Else
                    sDescription = "Emailed multiple files - " & sInputReceived

                    If bValidMultipledoc Then
                        m_lReturn = g_oBusiness.CreateEventInSBO(lEventCnt:=0, lPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=DBNull.Value, vClaimCnt:=vClaimCnt, lDocNum:=0, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentTypeId:=5, vReportTypeId:=DBNull.Value, lEventTypeId:=10, dtEventDate:=DateTime.Today, sDescription:=sDescription)

                    Else
                        m_lReturn = g_oBusiness.CreateEventInSBO(lEventCnt:=0, lPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=DBNull.Value, vInsuranceFileCnt:=DBNull.Value, vClaimCnt:=DBNull.Value, lDocNum:=0, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentTypeId:=5, vReportTypeId:=DBNull.Value, lEventTypeId:=10, dtEventDate:=DateTime.Today, sDescription:=sDescription)
                    End If
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to create Event record in Sirius.", " Email Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                If bZipped Then
                    'TODO
                    m_lReturn = DeleteUnzipFiles(lvwBCDocs)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to Unzip zipped files ", "E-mail Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If
                For lCnt As Integer = sNodeKeys.GetLowerBound(0) To sNodeKeys.GetUpperBound(0)
                    If (Not sNodeKeys(lCnt).Key Is Nothing) AndAlso (sNodeKeys(lCnt).Key.Substring(0, 1) = ACDocument) Then
                        lvwBCDocs.Items.RemoveByKey(sNodeKeys(lCnt).Key)
                    End If
                Next lCnt


                If bAllFilesEMailed Then
                    MessageBox.Show("All file(s) Emailed successfully.", " Email Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If

                If CheckBCPartiesMatch(lvwBCDocs) = gPMConstants.PMEReturnCode.PMTrue Then
                    EnableBCDocButtons(True)
                Else
                    EnableBCDocButtons(False)
                End If
            End If


        Catch ex As Exception
            If bZipped Then
                'TODO
                m_lReturn = DeleteUnzipFiles(lvwBCDocs)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to Unzip zipped files ", "E-mail Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            oPMMAPI = Nothing
        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    Private Function ArchiveBCDocuments() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "ArchiveBCDocuments"

        Dim sFolderCode, sDrawerExCode, sDescription As String
        Dim lDocNum, lFolderNum, lDrawerNum, lPartyCnt As Integer
        Dim bAllFilesArchived As Boolean
        Dim vClaimCnt As String = ""
        Dim vInsuranceFolderCnt As Integer
        Dim sNodeKeys() As DOCConst.DOCNodes = Nothing
        Dim bFirstElement As Boolean

        Try


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            result = gPMConstants.PMEReturnCode.PMTrue

            bAllFilesArchived = True

            'Initialise the array where you store the selected nodes
            ReDim sNodeKeys(0)
            'Add each selected nodes key to array
            bFirstElement = True

            ' First we loop round to get all the document in the lsit and build a list of attachment
            For Each liBCDoc As ListViewItem In lvwBCDocs.Items
                lDocNum = 0
                lFolderNum = 0
                sFolderCode = ""
                lDrawerNum = 0
                sDrawerExCode = ""
                lPartyCnt = 0

                vInsuranceFolderCnt = Nothing

                'Get the doc num
                m_lReturn = ExtractNumFromKey(liBCDoc.Name, lDocNum)
                m_bArchive = True

                'it is now in the new destination folder, get document's folder num

                m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Document, lNodeNum:=lDocNum, lParentNum:=lFolderNum)

                '.. and it's ex code

                m_lReturn = g_oBusiness.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, sExCode:=sFolderCode)

                'now get folder's parent i.e. drawer num

                m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, lParentNum:=lDrawerNum)
                ' .. and it's ex code

                m_lReturn = g_oBusiness.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lDrawerNum, sExCode:=sDrawerExCode)

                sDrawerExCode = sDrawerExCode.Trim()
                sFolderCode = sFolderCode.Trim()

                'check if external code exist

                If (sDrawerExCode.Trim().Length > 0) And (sFolderCode.Trim().Length > 0) Then
                    'if it's a claims folder, then truncate the C prefix
                    If sFolderCode.Substring(0, 1) = "C" Then
                        vClaimCnt = Mid(sFolderCode, 2)
                    Else

                        vClaimCnt = Nothing
                    End If

                    'the client ex_code will actually be used for the lPartyCnt
                    'if no ex_code is supplied then default to 0
                    If sDrawerExCode = "" Then
                        lPartyCnt = 0
                    Else
                        lPartyCnt = CInt(sDrawerExCode)

                        'if  general folder then show in Event log as it's the client folder
                        If sFolderCode = "GENERAL" Then
                            sFolderCode = ""
                        End If
                    End If

                    sDescription = "Archived file - " & ListViewHelper.GetListViewSubItem(liBCDoc, 1).Text

                    If sFolderCode = "" Then

                        vInsuranceFolderCnt = Nothing
                    Else
                        If sFolderCode.Substring(0, 1) = "C" Then

                            m_lReturn = g_oBusiness.GetInsuranceFolderCnt(vClaimCnt:=vClaimCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt)
                        Else
                            vInsuranceFolderCnt = CInt(sFolderCode)
                        End If
                    End If


                    m_lReturn = g_oBusiness.CreateEventInSBO(lEventCnt:=0, lPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=DBNull.Value, vClaimCnt:=vClaimCnt, lDocNum:=lDocNum, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentTypeId:=5, vReportTypeId:=DBNull.Value, lEventTypeId:=10, dtEventDate:=DateTime.Today, sDescription:=sDescription)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to create Event record in Sirius.", "Archive Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        If bFirstElement Then
                            bFirstElement = False
                        Else
                            ReDim Preserve sNodeKeys(sNodeKeys.GetUpperBound(0) + 1)
                        End If

                        sNodeKeys(sNodeKeys.GetUpperBound(0)).Key = liBCDoc.Name
                        sNodeKeys(sNodeKeys.GetUpperBound(0)).Text = liBCDoc.Text

                    End If
                Else
                    bAllFilesArchived = False
                End If

            Next liBCDoc
            For lCnt As Integer = sNodeKeys.GetLowerBound(0) To sNodeKeys.GetUpperBound(0)
                If sNodeKeys(lCnt).Key.Substring(0, 1) = ACDocument Then
                    lvwBCDocs.Items.RemoveByKey(sNodeKeys(lCnt).Key)
                End If
            Next lCnt
            'For	Each sNodeKeys_item As DOCConst.DOCNodes In sNodeKeys
            '	If sNodeKeys_item.Key.Substring(0, 1) = ACDocument Then
            '              lvwBCDocs.Items.RemoveAt(CInt(sNodeKeys_item.Key) - 1)

            '	End If
            'Next sNodeKeys_item

            If lvwBCDocs.Items.Count > 0 Then
                If Not bAllFilesArchived Then
                    MessageBox.Show("All file(s) could not be archived.", "Archive Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    MessageBox.Show("All file(s) Archived successfully.", "Archive Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If

            If CheckBCPartiesMatch(lvwBCDocs) = gPMConstants.PMEReturnCode.PMTrue Then
                EnableBCDocButtons(True)
            Else
                tlbBCDocsButtons.Items.Item(0).Enabled = False
                tlbBCDocsButtons.Items.Item(2).Enabled = False
            End If


            If lvwBCDocs.Items.Count = 0 Then
                tlbBCDocsButtons.Items.Item(1).Enabled = False
                tlbBCDocsButtons.Items.Item(3).Enabled = False
            End If

            'create event succeeded, reset settings
            m_bArchive = False
            g_lArchiveDocNum = 0



        Catch ex As Exception
            m_bArchive = False
            g_lArchiveDocNum = 0

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    Private Function ExportBCDocuments() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "ExportBCDocuments"

        Dim ofrmExportFolder As frmExportFolder
        Dim iPageCnt, iTemp As Integer
        Dim vPageArray() As Object
        Dim sFolderName As String = ""
        Dim sFile() As String
        Dim sTmpExt, sFilename As String
        Dim sSpecialCharacter() As Object
        Dim sFolderCode, sDrawerExCode, sTempDir, sDescription, sTempFileName As String
        Dim sNodeKey() As DOCConst.DOCNodes = Nothing
        Dim bZipped As Boolean
        Dim lPartyCnt, lFolderNum, lDrawerNum, lDocNum As Integer
        Dim sResolvedPath, sFileExt As String
        Dim bAllFilesExported As Boolean
        Dim sNodeKeys() As DOCConst.DOCNodes = Nothing
        Dim bFirstElement As Boolean
        Dim vInsuranceFolderCnt As Integer
        Dim vClaimCnt As String = ""
        Dim iValidListCnt As Integer
        Dim sTempFolderCode, sTempDrawerExCode As String
        Dim lTempDocNum As Integer
        Dim bValidMultipledoc As Boolean

        ' Dim oFSO As FileSystem
        Dim ofso As System.IO.FileInfo
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim sNodeKeys(0)
            bFirstElement = True

            For Each liBCDoc As ListViewItem In lvwBCDocs.Items
                'Get the doc num
                m_lReturn = ExtractNumFromKey(liBCDoc.Name, lDocNum)

                'Get the page file path

                m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Function GetPageList Failed")
                Else
                    'Get the first filename
                    sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))
                End If

                ' Check to see if zip file...

                m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZIPFile:=bZipped)
                If Not CBool(m_lReturn) Then
                    MessageBox.Show("File " & sFilename & " does not exist physically. Please remove this file to proceed further.", " Email Documents", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result
                End If
            Next liBCDoc


            'ofso = New FileInfo

            ofrmExportFolder = New frmExportFolder()
            'Show the screen
            ofrmExportFolder.ShowDialog()

            'Pass back the entered info
            sFolderName = ofrmExportFolder.FolderName
            If ofrmExportFolder.Status <> gPMConstants.PMEReturnCode.PMOK Then
                ofrmExportFolder.Close()
                Return result
            End If

            ofrmExportFolder.Close()
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            ' First we loop round to get all the document in the lsit and build a list of attachment
            For Each liBCDoc As ListViewItem In lvwBCDocs.Items
                'Get the doc num
                m_lReturn = ExtractNumFromKey(liBCDoc.Name, lDocNum)

                'get list of file paths for this doc

                m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Function GetPageList Failed")
                End If
                'Check if File name contains a special character
                ReDim sSpecialCharacter(8)

                sSpecialCharacter(0) = "/"
                sSpecialCharacter(1) = "\"
                sSpecialCharacter(2) = ":"
                sSpecialCharacter(3) = "*"
                sSpecialCharacter(4) = "?"
                sSpecialCharacter(5) = """"
                sSpecialCharacter(6) = "<"
                sSpecialCharacter(7) = ">"
                sSpecialCharacter(8) = "|"

                For lCnt As Integer = 0 To sSpecialCharacter.GetUpperBound(0)
                    iTemp = (ListViewHelper.GetListViewSubItem(liBCDoc, 1).Text.IndexOf(CStr(sSpecialCharacter(lCnt))) + 1)
                    If iTemp > 0 Then
                        MessageBox.Show("Document name contains a special character, rename it before saving.", DOCAppName)
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Return result
                    End If
                Next lCnt

                'check if user cancelled - ie no change in filename
                If sFolderName = liBCDoc.Text Then
                    Return result
                End If

                sTempFileName = sFolderName.Trim() & "\" & Mid(CStr(vPageArray(vPageArray.GetLowerBound(0))), (IIf(CStr(vPageArray(vPageArray.GetLowerBound(0))) = "" And "\" = "", 0, (CStr(vPageArray(vPageArray.GetLowerBound(0))).LastIndexOf("\") + 1))) + 1, Strings.Len(CStr(vPageArray(vPageArray.GetLowerBound(0)))))

                sTempFileName = sTempFileName.Trim()
                'Delete if destination file already present

                ofso = New FileInfo(sTempFileName)

                If ofso.Exists Then

                    ofso.Delete()
                End If

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))

                ' Check to see if zip file...

                m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZIPFile:=bZipped)

                If Not CBool(m_lReturn) Then
                    'error - assume unzipped
                    bZipped = False
                End If

                'separate extension from file title

                sResolvedPath = Mid(CStr(vPageArray(vPageArray.GetLowerBound(0))), (IIf(CStr(vPageArray(vPageArray.GetLowerBound(0))) = "" And "\" = "", 0, (CStr(vPageArray(vPageArray.GetLowerBound(0))).LastIndexOf("\") + 1))) + 1, Strings.Len(CStr(vPageArray(vPageArray.GetLowerBound(0)))))

                sFile = sResolvedPath.Split("."c)

                sFilename = sFile(0)

                If sFile.GetUpperBound(0) > 0 Then
                    sFileExt = sFile(1)
                End If

                iPageCnt = 1
                For lCnt As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0)
                    If vPageArray.GetLowerBound(0) = vPageArray.GetUpperBound(0) Then
                        'only one page
                        If bZipped Then

                            m_lReturn = m_oZipper.UnZipFile(CStr(vPageArray(lCnt)), sTempFileName)
                        Else
                            m_lReturn = DOCGeneralFunc.CopyFile(CStr(vPageArray(lCnt)), sTempFileName)
                        End If
                    Else
                        'several pages, so add in page counter
                        If bZipped Then

                            m_lReturn = m_oZipper.UnZipFile(CStr(vPageArray(lCnt)), sFilename & CStr(iPageCnt) & sFileExt)
                        Else
                            m_lReturn = DOCGeneralFunc.CopyFile(CStr(vPageArray(lCnt)), sFilename & CStr(iPageCnt) & sFileExt)
                        End If
                        iPageCnt += 1
                    End If
                Next lCnt
                bAllFilesExported = True
                If bFirstElement Then
                    bFirstElement = False
                Else
                    ReDim Preserve sNodeKeys(sNodeKeys.GetUpperBound(0) + 1)
                End If

                sNodeKeys(sNodeKeys.GetUpperBound(0)).Key = liBCDoc.Name
                sNodeKeys(sNodeKeys.GetUpperBound(0)).Text = liBCDoc.Text

            Next liBCDoc

            'now delete all the unzipped files from system temp dir
            If bZipped Then
                For Each vPageArray_item_2 As Object In vPageArray
                    sFilename = sTempDir & CStr(vPageArray_item_2).Substring(CStr(vPageArray_item_2).Length - 6)
                    If FileSystem.Dir(sFilename, FileAttribute.Normal) > "" Then
                        'reset read-only file attribute in order to delete
                        Dim fileInfo As FileInfo = New FileInfo(sFilename)
                        fileInfo.Attributes = FileAttribute.Normal
                        ' boom, you're  a dead file
                        m_lReturn = KillFile(sFilename)
                    End If
                Next vPageArray_item_2
            End If
            bValidMultipledoc = True
            For Each liBCDoc As ListViewItem In lvwBCDocs.Items
                sTempDrawerExCode = ""
                sTempFolderCode = ""
                lTempDocNum = 0

                'Get the doc num
                m_lReturn = ExtractNumFromKey(liBCDoc.Name, lTempDocNum)

                'it is now in the new destination folder, get document's folder num

                m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Document, lNodeNum:=lTempDocNum, lParentNum:=lFolderNum)
                '.. and it's ex code

                m_lReturn = g_oBusiness.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, sExCode:=sTempFolderCode)

                'now get folder's parent i.e. drawer num

                m_lReturn = g_oBusiness.GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, lParentNum:=lDrawerNum)
                ' .. and it's ex code

                m_lReturn = g_oBusiness.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lDrawerNum, sExCode:=sTempDrawerExCode)

                If String.IsNullOrEmpty(sDrawerExCode) Then
                    ' If sDrawerExCode.Trim().Length = 0 Then
                    sDrawerExCode = sTempDrawerExCode.Trim()
                    'End If
                End If


                If String.IsNullOrEmpty(sFolderCode) Then
                    sFolderCode = sTempFolderCode.Trim()
                End If

                'check if external code exist
                If (sTempDrawerExCode <> "") Or (sTempFolderCode <> "") Then
                    lDocNum = lTempDocNum
                    If Not (sTempDrawerExCode = sDrawerExCode And sTempFolderCode = sFolderCode) Then
                        bValidMultipledoc = False
                    End If

                    iValidListCnt += 1
                    '            If iValidListCnt > 1 Then
                    '                Exit For
                    '            End If
                End If
            Next liBCDoc

            If sDrawerExCode = "" Then
                lPartyCnt = 0
            Else
                lPartyCnt = ToSafeLong(sDrawerExCode)
                If sFolderCode = "GENERAL" Then
                    sFolderCode = ""
                End If
            End If

            'if it's a claims folder, then truncate the C prefix
            If sFolderCode.Substring(0, 1) = "C" Then
                vClaimCnt = Mid(sFolderCode, 2)
            Else

                vClaimCnt = Nothing
            End If

            If sFolderCode = "" Then

                vInsuranceFolderCnt = Nothing
            Else
                If sFolderCode.Substring(0, 1) = "C" Then

                    m_lReturn = g_oBusiness.GetInsuranceFolderCnt(vClaimCnt:=vClaimCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt)
                Else
                    vInsuranceFolderCnt = CInt(sFolderCode)
                End If
            End If

            If iValidListCnt = 1 Then
                sDescription = "Exported file - " & Interaction.InputBox("Enter a description for this Event", " Export file")



                m_lReturn = g_oBusiness.CreateEventInSBO(lEventCnt:=0, lPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=DBNull.Value, vClaimCnt:=vClaimCnt, lDocNum:=lDocNum, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentTypeId:=5, vReportTypeId:=DBNull.Value, lEventTypeId:=10, dtEventDate:=DateTime.Today, sDescription:=sDescription)

            Else
                sDescription = "Exported multiple files - " & Interaction.InputBox("Enter a description for this Event", " Export files")

                If bValidMultipledoc Then


                    m_lReturn = g_oBusiness.CreateEventInSBO(lEventCnt:=0, lPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=DBNull.Value, vClaimCnt:=vClaimCnt, lDocNum:=0, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentTypeId:=5, vReportTypeId:=DBNull.Value, lEventTypeId:=10, dtEventDate:=DateTime.Today, sDescription:=sDescription)
                Else


                    m_lReturn = g_oBusiness.CreateEventInSBO(lEventCnt:=0, lPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=DBNull.Value, vInsuranceFileCnt:=DBNull.Value, vClaimCnt:=DBNull.Value, lDocNum:=0, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentTypeId:=5, vReportTypeId:=DBNull.Value, lEventTypeId:=10, dtEventDate:=DateTime.Today, sDescription:=sDescription)
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to create Event record in Sirius.", "Export Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            For lCnt As Integer = sNodeKeys.GetLowerBound(0) To sNodeKeys.GetUpperBound(0)
                If sNodeKeys(lCnt).Key.Substring(0, 1) = ACDocument Then
                    lvwBCDocs.Items.RemoveByKey(sNodeKeys(lCnt).Key)
                End If
            Next lCnt

            If bAllFilesExported Then
                MessageBox.Show("All file(s) Exported successfully.", "Export Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            If CheckBCPartiesMatch(lvwBCDocs) = gPMConstants.PMEReturnCode.PMTrue Then
                EnableBCDocButtons(True)
            Else
                '        tlbBCDocsButtons.Buttons(ACBCEmail).Enabled = False
                '        tlbBCDocsButtons.Buttons(ACBCExport).Enabled = False
                EnableBCDocButtons(False)
            End If



        Catch ex As Exception
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ofso = Nothing
            Me.Cursor = Cursors.Default

        End Try
        Return result
    End Function

    Private Function RemoveItems() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "RemoveItems"

        Dim liBCDoc As ListViewItem
        Dim m_lReturn As Integer
        Dim sNodeKeys() As DOCConst.DOCNodes = Nothing
        Try


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = StoreSelectedNodes(sNodeKeys, lvwBCDocs)
            For i As Integer = sNodeKeys.GetLowerBound(0) To sNodeKeys.GetUpperBound(0)
                If sNodeKeys(i).Key.Substring(0, 1) = ACDocument Then
                    lvwBCDocs.Items.RemoveByKey(sNodeKeys(i).Key)
                End If
            Next


            If lvwBCDocs.Items.Count = 0 Then
                EnableBCDocButtons(False)
                Return result
            End If

            If CheckBCPartiesMatch(lvwBCDocs) = gPMConstants.PMEReturnCode.PMTrue Then
                EnableBCDocButtons(True)
            Else
                tlbBCDocsButtons.Items.Item(0).Enabled = False
                tlbBCDocsButtons.Items.Item(1).Enabled = False
            End If




        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result

    End Function

    Private Function SetBytes(ByRef bytes As Integer, ByRef sReturnString As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "SetBytes"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            If bytes >= 1073741824 Then
                sReturnString = StringsHelper.Format(bytes / 1024 / 1024 / 1024, "#0.00") & " GB"
            ElseIf bytes >= 71048576 Then
                sReturnString = StringsHelper.Format(bytes / 1024 / 1024, "#0.00") & " MB"
            ElseIf bytes >= 1024 Then
                sReturnString = StringsHelper.Format(bytes / 1024, "#0.00") & " KB"
            ElseIf bytes < 1024 Then
                sReturnString = CStr(CInt(IIf(CDbl(bytes) > 0, Math.Floor(CDbl(bytes)), Math.Ceiling(CDbl(bytes))))) & " Bytes"
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try
        Return result
    End Function
    Private Function DeleteUnzipFiles(ByRef lvw As ListView) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "DeleteUnzipFiles"
        Dim lDocNum As Integer
        Dim vPageArray() As Object
        Dim sFilename, sTempDir As String
        ' Dim oFSO As FileSystemObject
        Dim oFSO As System.IO.DirectoryInfo
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            ' oFSO = New FileSystemObject()

            'now delete all the unzipped files from system temp dir

            For Each liBCDoc As ListViewItem In lvwBCDocs.Items
                'Get the doc num
                m_lReturn = ExtractNumFromKey(liBCDoc.Name, lDocNum)

                'Get the page file path
                m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Function GetPageList Failed")
                Else
                    'Get the first filename
                    sFilename = CStr(vPageArray(vPageArray.GetLowerBound(0)))
                End If
                oFSO = New System.IO.DirectoryInfo(sFilename)
                If Not oFSO Is Nothing Then
                    ' Check to see if zip file...
                    m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZIPFile:=True)

                    If Not m_lReturn Then
                        'error - assume unzipped
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'get the system temp dir "<Drive>:\Temp" and extract the zip tfiles there
                    sTempDir = Interaction.Environ("Temp") & "\"
                    'attach each page of the document
                    For Each vPageArray_item As Object In vPageArray
                        'get the filename and unzip into the temp dir
                        sFilename = sTempDir & CStr(vPageArray_item).Substring(CStr(vPageArray_item).Length - 6)
                        'call the Attachment the same as the document
                        If FileSystem.Dir(sFilename, FileAttribute.Normal) > "" Then
                            'reset read-only file attribute in order to delete
                            Dim fileInfo As FileInfo = New FileInfo(sFilename)
                            fileInfo.Attributes = FileAttribute.Normal
                            ' boom, you're  a dead file
                            m_lReturn = KillFile(sFilename)
                        End If
                    Next vPageArray_item
                End If
            Next liBCDoc



        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result

    End Function

    Private Sub EnableBCDocButtons(ByVal v_bValue As Boolean)
        tlbBCDocsButtons.Items(0).Enabled = v_bValue
        tlbBCDocsButtons.Items(1).Enabled = v_bValue
        tlbBCDocsButtons.Items(2).Enabled = v_bValue
        tlbBCDocsButtons.Items(3).Enabled = v_bValue
        'tlbBCDocsButtons.Items.Item(ACBCEmail).Enabled = v_bValue
        'tlbBCDocsButtons.Items.Item(ACBCExport).Enabled = v_bValue
        'tlbBCDocsButtons.Items.Item(ACBCArchive).Enabled = v_bValue
        'tlbBCDocsButtons.Items.Item(ACBCREMOVE).Enabled = v_bValue
    End Sub

    Private Function CheckDocumentsBeforeMove() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "CheckDocumentsBeforeMove"

        Dim lCntDragNodes As Integer
        Dim sKeyDragNodes As String = ""

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the Nodes codes that need to be dragged and droped
            If Not m_sDragNodes Is Nothing Then

                lCntDragNodes = ToSafeLong(CStr(m_sDragNodes.GetUpperBound(0)))

                'Make a loop to dragged nodes
                For lCnt As Integer = 0 To lCntDragNodes
                    sKeyDragNodes = ToSafeString(m_sDragNodes(lCnt).Key)
                    If lvwBCDocs.Items.Count >= 0 Then
                        'Check wheter this item already exist in Briefcase List View
                        For lListCnt As Integer = 1 To lvwBCDocs.Items.Count
                            ' if this node is not added earlier then only add this node to the list
                            If sKeyDragNodes = lvwBCDocs.Items.Item(lListCnt - 1).Name Then
                                MessageBox.Show("These files are already in briefcase. Remove them from briefcase before moving to another folder.", DOCAppName, MessageBoxButtons.OK)
                                result = gPMConstants.PMEReturnCode.PMFalse
                                Return result
                            End If
                        Next
                    End If  'sKeyDragNodes
                Next
            End If



        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    'WR77 Documaster Enhancements END
    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()

    End Sub




    Private Sub tvwFind_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvwFind.MouseClick

        Dim lFoldNum As Integer
        Dim sNodeKey() As DOCConst.DOCNodes = Nothing


        On Error GoTo Err_tvwFind_Click

        m_lReturn = NodeClicked(tvwFind)

        Select Case m_lReturn
            Case gPMConstants.PMEReturnCode.PMFalse
                'Node not clicked on, so exit
                Exit Sub
            Case gPMConstants.PMEReturnCode.PMTrue
                'Fine, continue
            Case Else
                'Anything else, continue
        End Select

        'check the passwords of the node - unless you are adminstrator
        If Not g_bUserIsAdministrator Then

            ReDim sNodeKey(0)
            sNodeKey(0).Key = tvwFind.GetNodeAt(e.Location.X, e.Location.Y).Name
            sNodeKey(0).Text = tvwFind.GetNodeAt(e.Location.X, e.Location.Y).Text

            m_lReturn = VerifyPasswords(sNodeKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'user could n't be bothered to supply the password - or got it wrong
                Exit Sub
            End If
        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Application.DoEvents()

        NodeClick(tvwFind, lvwDocList, tvwFind.GetNodeAt(e.Location.X, e.Location.Y).Name, "")

        Application.DoEvents()

        'Swap icons of newly opened folder and last open folder

        If m_sFindLastOpenFolder <> "" Then
            'this may not exist
            On Error Resume Next
            tvwFind.Nodes.Find(m_sFindLastOpenFolder, True)(0).ImageKey = "IMGCLOSEDFOLDER"
        End If


        tvwFind.GetNodeAt(e.Location.X, e.Location.Y).ImageKey = "IMGOPENFOLDER"
        'm_sFindLastOpenFolder = tvwFind.SelectedNode.Name
        m_sFindLastOpenFolder = tvwFind.GetNodeAt(e.Location.X, e.Location.Y).Name

        'update the label
        lblTitleFind(1).Text = "Contents of '" & tvwFind.GetNodeAt(e.Location.X, e.Location.Y).Text & "'"
        'lblTitleFind(1).Tag = tvwFind.SelectedNode.Name
        lblTitleFind(1).Tag = tvwFind.GetNodeAt(e.Location.X, e.Location.Y).Name

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Exit Sub

Err_tvwFind_Click:

        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwFind_Click", excep:=New Exception(Information.Err().Description))

        Exit Sub
    End Sub



    Private Sub tvwMain_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwMain.NodeMouseClick
        Dim Button As Integer = CInt(e.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = (e.X)
        Dim Y As Single = (e.Y)

        Dim sKey As String = ""
        Dim lFoldNum, lChildren As Integer
        'as long as it's not an Add to view node
        If tvwMain.GetNodeAt(X, Y).Name.Substring(0, 3) = "ADD" Then
            Exit Sub
        Else
            tvwMain.SelectedNode = tvwMain.GetNodeAt(X, Y)
        End If


        'JH051198 this is where we decide whether to disable select folders option

        'Get the folder num from the selected node key
        'm_lReturn = ExtractNumFromKey(tvwMain.Nodes.Item(tvwMain.GetNodeAt(X, Y).Name).Name, lFoldNum)
        m_lReturn = ExtractNumFromKey(tvwMain.Nodes.Find(tvwMain.GetNodeAt(X, Y).Name, True)(0).Name, lFoldNum)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_lReturn = CountChildren(lFoldNum, lChildren)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        'If tvwMain.Nodes.Item(tvwMain.GetNodeAt(X, Y).Name).GetNodeCount(False) >= lChildren Then
        If tvwMain.Nodes.Find(tvwMain.GetNodeAt(X, Y).Name, True)(0).GetNodeCount(False) >= lChildren Then
            'they've already got them all
            mnuFileSelect.Enabled = False
            mnuPopSelect.Enabled = False
        Else
            mnuFileSelect.Enabled = True
            mnuPopSelect.Enabled = True
        End If


        'If Button = 2 Then
        If e.Button = MouseButtons.Right Then

            'Fine, continue
            EnableMenuItems(tvwMain)

            'temporarily select rightclicked node

            'tvwMain.Nodes.Item(tvwMain.GetNodeAt(X, Y).Name).Selected = True
            tvwMain.SelectedNode = tvwMain.Nodes.Find(tvwMain.GetNodeAt(X, Y).Name, True)(0)

            'save currently selected node
            sKey = tvwMain.SelectedNode.Name

            'save current control
            m_cntCurrent = tvwMain


            'Ctx_mnuPop.Show(Me.tvwMain, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
            Ctx_mnuPop.Show(Me.tvwMain, X, Y)

            ' m_cntCurrent = Nothing

            'restore old position (unless we earlier set a flag
            'to not restore it)
            If Not m_bLeaveNodeSelected Then

                ' RDC 30032005 This'll fail if the node was deleted
                Try

                    'tvwMain.Nodes.Item(sKey).Selected = True
                    tvwMain.SelectedNode = tvwMain.Nodes.Find(tvwMain.GetNodeAt(X, Y).Name, True)(0)

                Catch
                End Try

                'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_tvwMain_MouseDown)")

                'Swap icons of newly opened folder and last open folder
                If m_sMainLastOpenFolder <> "" Then
                    ' tvwMain.Nodes.Item(m_sMainLastOpenFolder).ImageKey = "IMGCLOSEDFOLDER"
                    tvwMain.Nodes.Find(m_sMainLastOpenFolder, True)(0).ImageKey = "IMGCLOSEDFOLDER"
                End If


                'tvwMain.SelectedNode.ImageKey = "IMGOPENFOLDER"
                m_sMainLastOpenFolder = tvwMain.SelectedNode.Name

                'update the label
                lblTitleMain(1).Text = "Contents of '" & tvwMain.SelectedNode.Text & "'"
                lblTitleMain(1).Tag = tvwMain.SelectedNode.Name
            Else
                m_bLeaveNodeSelected = False
            End If
        End If



        Exit Sub

Err_tvwMain_MouseDown:

        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tvwMain_MouseDown", excep:=New Exception(Information.Err().Description))
        Exit Sub


    End Sub



    Private Sub lvwBCDocs_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwBCDocs.DragDrop
        Const kMethodName As String = "lvwBCDocs_DragDrop"
        Try

            Dim lDocNum, lCntDragNodes As Integer
            Dim sKeyDragNodes, sParents As String
            Dim bDragNodeExist As Boolean
            Dim vPageArray() As Object
            Dim sFilePath, sFilename As String
            Dim lFileSize As Integer
            Dim oListItem As ListViewItem
            Dim vParentArray() As Object
            Dim iParentCnt As Integer
            Dim sReturnString As String = ""




            'If sender.Name.Trim() <> "lvwDocList" Then
            '    Exit Sub
            'End If

            bDragNodeExist = False

            'Get the Nodes codes that need to be dragged and droped
            lCntDragNodes = ToSafeLong(CStr(m_sDragNodes.GetUpperBound(0)))

            'Make a loop to dragged nodes
            For lCnt As Integer = 0 To lCntDragNodes
                sKeyDragNodes = ToSafeString(m_sDragNodes(lCnt).Key)
                If lvwBCDocs.Items.Count >= 0 Then
                    'Check wheter this item already exist in Briefcase List View
                    For lListCnt As Integer = 1 To lvwBCDocs.Items.Count
                        ' if this node is not added earlier then only add this node to the list
                        If sKeyDragNodes = lvwBCDocs.Items.Item(lListCnt - 1).Name Then
                            bDragNodeExist = True
                            Exit For
                        Else
                            bDragNodeExist = False
                        End If
                    Next
                End If  'sKeyDragNodes
                'If Dragged Node is not already exist
                If Not bDragNodeExist Then
                    'Most important thing First need to check what is the View Mode
                    'because Main Mode have lblTitleMain and Find Mode have lblTitleFind
                    Select Case m_iViewMode
                        Case DOCViewModeMain
                            m_lReturn = GetParentNamesFromTree(tvwMain, Convert.ToString(lblTitleMain(1).Tag), sParents)
                        Case DOCViewModeFindResults
                            m_lReturn = GetParentNamesFromTree(tvwFind, Convert.ToString(lblTitleFind(1).Tag), sParents)
                        Case Else
                    End Select

                    vParentArray = sParents.Split(","c)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Function GetParentNamesFromTree Failed")
                    End If

                    m_lReturn = ExtractNumFromKey(m_sDragNodes(lCnt).Key, lDocNum)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Function ExtractNumFromKey Failed")
                    End If

                    'get the page file paths

                    m_lReturn = g_oBusiness.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Function GetPageList Failed")
                    Else
                        ' Get the file Path
                        sFilePath = CStr(vPageArray(vPageArray.GetLowerBound(0)))
                    End If

                    For i As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0)
                        If FileSystem.Dir(sFilePath, FileAttribute.Normal) <> Nothing Then
                            'get the file size in bytes
                            lFileSize = CInt((New FileInfo(sFilePath)).Length)
                        End If
                        'Show Original Name of the File
                        sFilename = ToSafeString(m_sDragNodes(lCnt).Text)
                        ' Exclude System Generated File Name from the path

                        'sFilePath = Mid(sFilePath, 1, InStrRev(sFilePath, "\"))
                    Next

                    iParentCnt = vParentArray.GetUpperBound(0)
                    For i As Integer = vParentArray.GetLowerBound(0) To vParentArray.GetUpperBound(0)
                        If i = 0 Then
                            If i = iParentCnt Then  ' to remove In
                                sFilePath = Mid(CStr(vParentArray(iParentCnt - i)).Trim(), 4).Replace("'", "")
                            Else
                                sFilePath = CStr(vParentArray(iParentCnt - i)).Trim().Replace("'", "")
                            End If
                        Else
                            If i = iParentCnt Then  ' to remove In
                                sFilePath = (sFilePath & "\" & Mid(CStr(vParentArray(iParentCnt - i)).Trim(), 3).Trim()).Replace("'", "")
                            Else
                                sFilePath = (sFilePath & "\" & CStr(vParentArray(iParentCnt - i)).Trim()).Replace("'", "")
                            End If
                        End If
                    Next

                    'NOW ADD THIS TO LIST
                    oListItem = lvwBCDocs.Items.Add(sKeyDragNodes, sFilePath.Trim(), "")

                    ListViewHelper.GetListViewSubItem(oListItem, k_BCDocFileName).Text = sFilename.Trim()

                    'Do we need to convert then in KB??
                    m_lReturn = SetBytes(CInt(CStr(lFileSize).Trim()), sReturnString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Function GetPageList Failed")
                    End If
                    ListViewHelper.GetListViewSubItem(oListItem, k_BCDocFileSize).Text = sReturnString
                End If
            Next
            If lvwBCDocs.Items.Count > 0 Then
                lvwBCDocs.Items.Item(0).Selected = True
            End If

            'Check that all the list items are of Same Party & Branch
            If CheckBCPartiesMatch(lvwBCDocs) = gPMConstants.PMEReturnCode.PMTrue Then
                EnableBCDocButtons(True)
            Else
                tlbBCDocsButtons.Items.Item(0).Enabled = False
                tlbBCDocsButtons.Items.Item(1).Enabled = False
                tlbBCDocsButtons.Items.Item(2).Enabled = True
                tlbBCDocsButtons.Items.Item(3).Enabled = True

            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
        Finally

        End Try
        Exit Sub
    End Sub

    Private Sub lvwBCDocs_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwBCDocs.DragEnter
        e.Effect = DragDropEffects.Move

    End Sub

    Private Sub lvwBCDocs_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwBCDocs.DragOver
        Const kMethodName As String = "lvwBCDocs_DragOver"
        Try


            If m_bDragging Then

                ' Set DropHighlight to the mouse's coordinates.

                lvwBCDocs.FocusedItem = lvwBCDocs.GetItemAt(e.X, e.Y)

                'DragOverCheck(lvwBCDocs, Source, X, Y)

            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
        Finally

        End Try
        Exit Sub
    End Sub



    Private Sub mnuPopToSharePoint_Click(sender As Object, e As EventArgs) Handles mnuPopToSharePoint.Click
        Dim oProcess As New Process
        oProcess.StartInfo.UseShellExecute = False
        oProcess.StartInfo.FileName = "DMEMigrationController.exe"

        If lvwDocList.SelectedItems.Count > 0 Then
            Dim sParam As String = String.Empty
            Dim iDocNum As Integer

            For Each itmDocName As ListViewItem In lvwDocList.SelectedItems
                m_lReturn = ExtractNumFromKey(itmDocName.Name, iDocNum)
                sParam = sParam & " -D=" & iDocNum.ToString
            Next

            oProcess.StartInfo.Arguments = sParam
            oProcess.Start()
        Else
            Dim iFolderNum As Integer
            m_lReturn = ExtractNumFromKey(tvwMain.SelectedNode.Name, iFolderNum)
            oProcess.StartInfo.Arguments = "-F=" & iFolderNum.ToString
            oProcess.Start()
        End If

    End Sub

End Class
