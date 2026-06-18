Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Partial Friend Class frmSelectFolders
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Form Name: frmSelectFolders
    '
    ' Date: 5/11/1998
    '
    ' Description:  To enable the user to expand only those folders
    '               chosen rather than the whole lot
    '
    ' Edit History:
    '
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmSelectFolders"
    Private frmInterface As frmInterface

    ' PUBLIC Data Members (Begin)

    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    'to store the folders returned property
    Private m_lFoldersReturned As Integer

    'to store the selected folders
    Private m_vFolderArray(,) As Object

    ' Stores the return value for function calls.
    Private m_lReturn As Integer

    Private Const MAXFORLIST As Integer = 500
    '
    '
    ' stores the properties of folders
    Private Structure Folder_Properties_Type
        'Name
        Dim sName As String
        'number
        Dim lNumber As Integer
        'date
        'sDate As String
        'children
        Dim lChildren As Integer
        Public Shared Function CreateInstance() As Folder_Properties_Type
            Dim result As New Folder_Properties_Type
            result.sName = String.Empty
            Return result
        End Function
    End Structure

    Private m_FolderProperties As Folder_Properties_Type = Folder_Properties_Type.CreateInstance()
    ' PRIVATE Data Members (End)
    '
    Private m_oTNSelectedFolder As System.Windows.Forms.TreeNode = Nothing
    Private m_nTotalFolderItems As Integer = Nothing
    Private Sub cboCompany_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCompany.SelectionChangeCommitted

        Dim lFoldNum, lChildren As Integer

        Try

            ' set folder to search on i.e. currently selected company number
            m_FolderProperties.lNumber = VB6.GetItemData(cboCompany, cboCompany.SelectedIndex)
            'Debug.Print m_FolderProperties.lNumber

            lFoldNum = m_FolderProperties.lNumber

            'get total of clients in company
            m_lReturn = g_oBusiness.CountChildren(lFoldNum, lChildren)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cboCompany_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cboCompany_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

            m_FolderProperties.lChildren = lChildren
            m_FolderProperties.sName = VB6.GetItemString(cboCompany, cboCompany.SelectedIndex)

            'reset everything and run new search on currenty selected company
            SetTextBoxes()

        Catch ex As System.Exception



            If cboCompany.SelectedIndex < 0 Then
                cboCompany.SelectedIndex = 0
                Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to re-select company details", vApp:=ACApp, vClass:=ACClass, vMethod:="cboCompany_Click", excep:=ex)
            Exit Sub
        End Try

    End Sub


    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        Try
            'Populate main treeview on Interface with selected folders
            m_lReturn = ReturnSelectedFolders()

            cmdFind.Enabled = False
            VB6.SetDefault(cmdFind, True)

            cmdApply.Enabled = False
            cmdOK.Enabled = False

        Catch



            MessageBox.Show("Nothing Selected!", "Select Folders", MessageBoxButtons.OK)
            Exit Sub
        End Try


    End Sub

    ' PUBLIC Property Procedures (Begin)
    '***VarDataEnd***

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click
        m_oTNSelectedFolder = Nothing
        'hide with no changes
        ' RemoveHandler Me.FormClosing, AddressOf frmSelectFolders_FormClosing
        ' Me.Close()
        Me.Hide()
        'cmdNewSearch_Click
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
        'MS 01/06/01
        ' if it is an archive then expand all client folders in the main treeview
        ' in order single click and archive
        If g_lArchiveDocNum > 0 Then
            For iCount As Integer = 0 To frmInterface.tvwMain.Nodes.Count - 1
                m_lReturn = frmInterface.ExpandFolder(tvw:=frmInterface.tvwMain, sTempKey:=frmInterface.tvwMain.Nodes.Item(iCount).Name.ToString())
            Next iCount

        End If

        'Me.Close()
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

    End Sub

    Private Sub cmdFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFind.Click

        'do the search based on the parameters in the textboxes
        'and populate the listview with the returned array

        Dim vFolders As Object
        Dim lNumber As Integer
        Dim sCaption As String = ""

        Try

            If txtNumber.Text.Trim() = "" Then
                lNumber = 0
            Else
                lNumber = CInt(txtNumber.Text.Trim())
            End If
            sCaption = txtCaption.Text.Trim()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            If sCaption <> "*" Then

                m_lReturn = SelectFolderList(sCaption:=sCaption, lMaxFoldersReturned:=lNumber, vResultArray:=vFolders)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select Folder List", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFind_Click")
                    Exit Sub
                End If
            Else
                'Get folderlist for supplied parent
                m_lReturn = g_oBusiness.GetFolderList(lParentNum:=m_FolderProperties.lNumber, sFilter:="", lMaxFoldersReturned:=lNumber, vResultArray:=vFolders)
            End If

            'populate the list
            'decide whether to show listview or listbox depending on
            'number of folders in array.  listbox has a lower max.
            'actually no, we need the listview

            If Information.IsArray(vFolders) Then
                m_lReturn = frmInterface.PopulateListView(lvw:=lvwFolders, vFolderArray:=vFolders, bDetails:=False, bExtraDate:=False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select Folder List", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFind_Click")
                    Exit Sub

                End If
                'debveloper guide no. 275
                'ssbFolders.Text = lvwFolders.Items.Count & " of " & CStr(m_FolderProperties.lChildren)
                _ssbFolders_Panel1.Text = lvwFolders.Items.Count & " of " & CStr(m_FolderProperties.lChildren)

            Else
                MessageBox.Show("Nothing to Return", "Select Folders", MessageBoxButtons.OK)
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            VB6.SetDefault(cmdApply, True)

        Catch ex As System.Exception



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select Folder List", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFind_Click", excep:=ex)
            Exit Sub
        End Try


    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        NewSearch()
        cmdApply.Enabled = False
        cmdOK.Enabled = False
        txtCaption.Focus()

    End Sub

    ' ***************************************************************** '
    ' Name: NewSearch
    '
    ' Description: clear the view and boxes and disable cmdFind
    '               separate sub so can call before the form is visible
    '
    '
    ' ***************************************************************** '
    Private Sub NewSearch()

        Try


            txtCaption.Text = ""
            txtNumber.Text = CStr(g_lMaxAutoExpand)
            ssbFolders.Text = "0 of " & m_FolderProperties.lChildren
            lvwFolders.Items.Clear()
            cmdFind.Enabled = False
            Me.AcceptButton = cmdFind
            'VB6.SetDefault(cmdFind, True)

        Catch excep As System.Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewSearchFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewSearch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try
    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        'cmdApply_Click

        m_lReturn = ReturnSelectedFolders()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        cmdClose_Click(cmdClose, New EventArgs())

    End Sub

    ' ***************************************************************** '
    ' Name: SelectFolderList
    '
    ' Description:  Search for folders with sCaption in title
    '               Max returned Is lNumber, returns vFolders
    '               an array of the folder names and numbers
    '
    ' ***************************************************************** '
    Public Function SelectFolderList(ByRef sCaption As String, ByRef lMaxFoldersReturned As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL_Text As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = g_oBusiness.GetMatchedFolderList(sFolderName:=sCaption, lParentNum:=m_FolderProperties.lNumber, lMaxFoldersReturned:=lMaxFoldersReturned, vResultArray:=vResultArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Business Failed.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectFolderList")

                Return result
            End If

            Return result

        Catch excep As System.Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectFolderList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ReturnSelectedFolders
    '
    ' Description:  Should populate the main treeview with only the
    '               folders that are in the array passed - taken
    '               from the listview on m_oSelectFolders
    '
    '
    ' ***************************************************************** '
    Public Function ReturnSelectedFolders() As Integer

        Dim result As Integer = 0
        Dim vTempArray(,) As Object

        Dim lCount, lInnerCount, lNodeNum As Integer
        Dim iIndex As Integer
        Dim sKey As String = ""


        Dim bSplash, bNodeSelected As Boolean
        Dim sNodesToDelete() As String
        Try

            If m_oTNSelectedFolder Is Nothing Then
                m_oTNSelectedFolder = frmInterface.tvwMain.SelectedNode
                m_nTotalFolderItems = lvwFolders.Items.Count
            End If

            frmInterface.tvwMain.SelectedNode = m_oTNSelectedFolder

            result = gPMConstants.PMEReturnCode.PMTrue
            bNodeSelected = False

            'have to do this again because sometimes the node
            If frmInterface.tvwMain.SelectedNode Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            'gets reselected
            If frmInterface.tvwMain.SelectedNode.Text.Length > 3 AndAlso frmInterface.tvwMain.SelectedNode.Text.Substring(0, 3).ToUpper = "ADD" Then
                sKey = frmInterface.tvwMain.SelectedNode.Parent.Text
                frmInterface.tvwMain.SelectedNode = frmInterface.tvwMain.Nodes(sKey)
            End If

            'in case it gets changed
            iIndex = frmInterface.tvwMain.SelectedNode.Index
            ' MS 06/01/01
            ' if more than one company can be selected, change index to correct co.
            If cboCompany.Enabled Then
                For iCount As Integer = 0 To frmInterface.tvwMain.Nodes.Count - 1
                    ' parent will be nothing for a co. node
                    If frmInterface.tvwMain.Nodes.Item(iCount).Parent Is Nothing Then
                        m_lReturn = ExtractNumFromKey(frmInterface.tvwMain.Nodes(iCount).Name, lNodeNum)
                        If lNodeNum = m_FolderProperties.lNumber Then
                            iIndex = frmInterface.tvwMain.Nodes(iCount).Index
                            Exit For

                        End If

                    End If

                Next iCount

            End If

            'create a new array based on selected items from the list
            'so basically the opposite of populatetreechildren
            'while doing this, delete node from tree
            lInnerCount = 0

            'if we are doing quite a few, splash
            'developer guide no. 275
            If lvwFolders.Items.Count > 500 Then
                bSplash = True
                m_lReturn = g_oSplash.Show(DOCSplash_Retrieving)
            End If
            'For lCount = 1 To lvwFolders.Items.Count
            For lCount = 0 To lvwFolders.Items.Count - 1
                If lvwFolders.Items(lCount).Selected Then
                    'insert the bits into the array
                    'm_lReturn = ExtractNumFromKey(lvwFolders.Items(lCount).Text, lNodeNum)
                    m_lReturn = ExtractNumFromKey(lvwFolders.Items(lCount).Name, lNodeNum)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        ReDim Preserve vTempArray(3, lInnerCount)

                        ReDim Preserve sNodesToDelete(lInnerCount)
                        'sNodesToDelete(lInnerCount) = lvwFolders.Items(lCount).Text
                        sNodesToDelete(lInnerCount) = lvwFolders.Items(lCount).Name
                        vTempArray(0, lInnerCount) = lNodeNum
                        'name
                        vTempArray(1, lInnerCount) = lvwFolders.Items(lCount).Text
                        'password
                        'vTempArray(2, lInnerCount) = CStr(lvwFolders.Items(lCount).Name).Substring(ACPasswordStart - 1, Math.Min(lvwFolders.Items(lCount).Text.Length, ACPasswordLen))
                        vTempArray(2, lInnerCount) = CStr(lvwFolders.Items(lCount).Name).Substring(ACPasswordStart - 1, ACPasswordLen)
                        'date
                        'vTempArray(3, lInnerCount) = lvwFolders.Items(lCount).SubItems(1)
                        vTempArray(3, lInnerCount) = lvwFolders.Items(lCount).SubItems(1).Text
                        lInnerCount += 1
                        bNodeSelected = True
                    End If
                End If
            Next lCount

            'delete them now
            If Not Information.IsArray(sNodesToDelete) Or Not bNodeSelected Then
                MessageBox.Show("Nothing Selected!", "Select Folders", MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMError
            Else
                For Each sNodesToDelete_item As String In sNodesToDelete
                    'lvwFolders.Items.Remove(sNodesToDelete_item)
                    lvwFolders.Items.RemoveByKey(sNodesToDelete_item)
                Next sNodesToDelete_item
            End If

            ssbFolders.Text = lvwFolders.Items.Count & " of " & CStr(m_FolderProperties.lChildren)

            'filltree
            If Not Information.IsArray(vTempArray) Then
                'do nothing because user pressed cancel or selected nowt
                Return result
            Else
                m_vFolderArray = vTempArray
                'If children exist, this node has been previouly expanded

                'hide splash
                If bSplash Then
                    m_lReturn = g_oSplash.Hide()
                End If

            End If



            Dim sAddNumberKey As String
            sAddNumberKey = "ADD" & m_oTNSelectedFolder.Name.Replace(" ", "")
            If frmInterface.tvwMain.SelectedNode.Nodes.ContainsKey(sAddNumberKey) Then
                frmInterface.tvwMain.SelectedNode.Nodes.RemoveByKey(sAddNumberKey)
            End If


            m_lReturn = frmInterface.PopulateTreeChildren(frmInterface.tvwMain, iIndex, m_vFolderArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnSelectedFolders", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            End If


            sKey = m_oTNSelectedFolder.FirstNode.Name

            frmInterface.tvwMain.SelectedNode = frmInterface.tvwMain.Nodes.Find(sKey, True)(0)
            frmInterface.tvwMain.SelectedNode.Expand()


            If m_oTNSelectedFolder.Nodes.Count < m_nTotalFolderItems Then
                m_lReturn = frmInterface.AddToViewNode(tvw:=frmInterface.tvwMain, sKey:=sKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Return result
                End If
            End If


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnSelectedFolders", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowMe
    '
    ' Description: displays the form, default is modally
    '
    '
    ' ***************************************************************** '
    Private Function ShowMe(Optional ByRef Modal_Renamed As Object = Nothing) As Integer

        Dim result As Integer = 0



        If Information.IsNothing(Modal_Renamed) Then
            Modal_Renamed = 1
        End If

        Dim lModal As FormShowConstants = Modal_Renamed

        iPMFunc.CenterForm(Me)

        VB6.ShowForm(Me, lModal)

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMe", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetTextBoxes
    '
    ' Description: put the folder values into the textboxes
    '
    '
    ' ***************************************************************** '
    Private Function SetTextBoxes() As Integer

        Dim result As Integer = 0
        Dim sDocName, sDocExCode As String
        Dim lFolderNum As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cboCompany.Enabled = False
            Me.Text = "Select Folders to Display for " & m_FolderProperties.sName
            cboCompany.Text = m_FolderProperties.sName
            txtTotalChild.Text = CStr(m_FolderProperties.lChildren)
            ToolTip1.SetToolTip(txtTotalChild, "Total Children for " & m_FolderProperties.sName)

            ' if more than 1 company then it is an Archive document request
            ' overwrite with relevant details
            If cboCompany.Items.Count > 0 And g_lArchiveDocNum > 0 Then

                'get the document name

                m_lReturn = g_oBusiness.GetDocInfo(g_lArchiveDocNum, sDocName, lFolderNum, sDocExCode)

                cboCompany.Enabled = True

                Me.Text = "Archive '" & sDocName & "', Select a Destination Folder"

            End If

            NewSearch()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetTextBoxesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetTextBoxes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef frm As frmInterface) As Integer

        Dim result As Integer = 0
        Try


            'set defaults or whatever
            frmInterface = frm

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                Me.Close()
                g_lArchiveDocNum = 0
                
            End If
        End If
        Me.disposedValue = True
    End Sub


    Private Sub cmdSelectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectAll.Click
        'select everything in the list view
        'developer guide no. 275
        For lCount As Integer = 0 To lvwFolders.Items.Count - 1
            lvwFolders.Items(lCount).Selected = True

        Next lCount

        cmdApply.Enabled = True
        cmdOK.Enabled = True
        'give it focus
        lvwFolders.Focus()

    End Sub


    Private Sub frmSelectFolders_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            Dim pmeCROSizeOnly, pmeCRTRelativeToBottomRight, pmeCROPositionOnly As Object

            If cboCompany.Enabled Then
                cboCompany.Focus()
            End If

            ' Tell Resizer which controls to resize and HOW to resize them.
            With uctPMResizer1

                .SetControlResizeOption("cmdOK", pmeCROPositionOnly, pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("cmdClose", pmeCROPositionOnly, pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("cmdApply", pmeCROPositionOnly, pmeCRTRelativeToBottomRight)


                .SetControlResizeOption("cmdSelectAll", pmeCROPositionOnly, pmeCRTRelativeToBottomRight)

                '.SetControlResizeOption "cmdFind", pmeCROTopOnly, pmeCRTRelativeToBottomRight
                '.SetControlResizeOption "cmdNewSearch", pmeCROTopOnly, pmeCRTRelativeToBottomRight



                .SetControlResizeOption("lvwFolders", pmeCROSizeOnly, pmeCRTRelativeToBottomRight)

                '.SetControlResizeOption "tabMain", pmeCROPositionOnly, pmeCRTRelativeToBottomRight
            End With

        End If
    End Sub


    Private Sub frmSelectFolders_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        With uctPMResizer1
            ' Tell the control to only resize the controls that I tell it to.

            .NoResizeByDefault = True
            ' Form Min Height/Width

            .FormMinHeight = 6240

            .FormMinWidth = 6165
        End With

    End Sub

    Private Sub frmSelectFolders_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Hide()
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        If Not eventArgs.CloseReason = CloseReason.None Then
            g_lArchiveDocNum = 0
        End If
        eventArgs.Cancel = True
    End Sub








    Private isInitializingComponent As Boolean
    Private Sub txtCaption_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCaption.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        cmdFind.Enabled = txtCaption.Text.Trim() <> ""

    End Sub


    ' ***************************************************************** '
    ' Name: SetFolderValues
    '
    ' Description: used to set values of m_FolderProperties when
    '              accessing SelectFolder form
    '
    '
    ' ***************************************************************** '
    Public Function SetFolderValues(ByRef lFolderNum As Integer, ByRef lChildren As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'reset the folder array
            m_vFolderArray = vResultArray

            'if archive document
            If lFolderNum = 0 And lChildren = 0 Then
                ' get all company cabinets
                ' lMaxFoldersReturned set to 50... surely will not be using more than that!
                m_lReturn = g_oBusiness.GetFolderList(lParentNum:=0, sFilter:="", lMaxFoldersReturned:=50, vResultArray:=vResultArray)

                ' Populate the company lsit i.e combo box in frmSelectFolders
                If Information.IsArray(vResultArray) Then
                    Debug.WriteLine("")
                    Debug.WriteLine(DateTime.Now)
                    Debug.WriteLine("Adding Company [SetFolderValues]")

                    cboCompany.Items.Clear()
                    For iRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                        'populate co. name
                        cboCompany.Items.Add(CStr(vResultArray(1, iRow)))
                        'populate co. folder number
                        VB6.SetItemData(cboCompany, iRow, CInt(vResultArray(0, iRow)))
                        Debug.WriteLine(VB6.GetItemString(cboCompany, iRow) & " " & CStr(VB6.GetItemData(cboCompany, iRow)))

                    Next iRow

                Else
                    ' oh really!

                    MessageBox.Show("Failed to get Company folders", "Archive Document Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result

                End If

                'set the company to the most recent
                lFolderNum = VB6.GetItemData(cboCompany, 0)

                'get total children i.e clients in company
                m_lReturn = g_oBusiness.CountChildren(lFolderNum, lChildren)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFolderValuesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFolderValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                m_FolderProperties.lNumber = lFolderNum
                m_FolderProperties.lChildren = lChildren
                m_FolderProperties.sName = VB6.GetItemString(cboCompany, 0)

            Else

                ' normal folder select
                m_lReturn = g_oBusiness.GetFolderValues(lFolderNum, vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get folder values", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFolderValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                If Not Information.IsArray(vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMError

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid folder", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFolderValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If


                m_FolderProperties.lNumber = lFolderNum
                m_FolderProperties.lChildren = lChildren
                m_FolderProperties.sName = CStr(vResultArray(1, 0)).Trim()

            End If
            m_lReturn = SetTextBoxes()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set folder values", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFolderValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFolderValuesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFolderValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectFolders
    '
    ' Description: Called by the interface to set up form and
    '              return array of folders
    '
    '
    ' ***************************************************************** '
    Public Function SelectFolders(ByRef lFolderNum As Integer, ByRef lChildren As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'initialise properties

            m_lReturn = SetFolderValues(lFolderNum, lChildren)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            'and show
            ShowMe()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select Folders", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectFolders", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub txtNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNumber.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If Conversion.Val(txtNumber.Text) > DOCMaxMaxAutoExpand Then
            txtNumber.Text = CStr(DOCMaxMaxAutoExpand)
        End If

    End Sub


    Private Sub lvwFolders_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwFolders.DoubleClick
        cmdApply_Click(cmdApply, New EventArgs())
    End Sub

    Private Sub lvwFolders_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvwFolders.MouseDown
        cmdApply.Enabled = True
        cmdOK.Enabled = True
    End Sub
End Class
