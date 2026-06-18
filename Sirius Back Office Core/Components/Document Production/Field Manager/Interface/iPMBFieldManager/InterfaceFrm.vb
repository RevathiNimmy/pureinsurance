Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports Word = Microsoft.Office.Interop.Word
Imports SharedFiles
Imports System.Collections.Generic

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' *****************************************************************
    ' Form Name: frmInterface
    '
    ' Date: ???
    '
    ' Description: Interface Form
    '
    ' Edit History:
    '
    ' DJM 23/07/2002 : Change so that code works with sp_ and spu_.
    ' DJM 23/07/2002 : Stopped putting in table name as it isn't used (Allows for shorter merge codes).
    ' *****************************************************************


    Private m_vFieldArray(,) As Object

    'RWH(24/08/2000) RSAIB Process 12
    Private m_vClauseArray(,) As Object
    Private m_vLoopsInDocument() As Object

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private Const ACClass As String = "frmInterface"

    Private m_oBusiness As bSIRFieldManager.Business

    Dim oDictMainGroup As New Dictionary(Of String, Object)
    Dim oDictSubGroup As New Dictionary(Of String, Object)

    Private m_bCalledFromSwift As Boolean ' RAM20050104 - Added for Swift
    Private m_vSubDocumentsArray(,) As Object ' RAM20050104 - Added to supprt Sub-Documents
    'developer guide no.7
    Private Const vbFormControlMenu As Integer = 0
    ' Show or hides the form
    Public WriteOnly Property FormVisible() As Boolean
        Set(ByVal Value As Boolean)

            Dim bReturn As Boolean

            If Value Then

                ' Show the form
                Me.Show()

                ' Restore the form
                Me.WindowState = FormWindowState.Normal
                'Added TopMost.vb from shared files explicitly
                ' Set the form to Always on top
                'bReturn = TopMost.SetTopmost(Me)
                bReturn = SetTopmost(Me)
            Else

                ' Clear the always on top
                'bReturn = TopMost.ClearTopmost(Me)
                bReturn = ClearTopmost(Me)

                ' Minimise the form
                Me.WindowState = FormWindowState.Minimized

                ' Hide the form
                Me.Hide()

            End If

        End Set
    End Property

    Public WriteOnly Property CalledFromSwift() As Boolean
        Set(ByVal Value As Boolean)
            m_bCalledFromSwift = Value
        End Set
    End Property


    Private Sub cmdBrowse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrowse.Click

        Try



            dlgFileOpen.ShowReadOnly = False
            dlgFileOpen.ShowDialog()

            txtFileName.Text = dlgFileOpen.FileName

        Catch



            Exit Sub
        End Try


    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Me.FormVisible = False

    End Sub


   

    Private Sub InsertField(ByRef iInsertType As Integer)

        Dim sField As String = ""
        Dim sStartOfLoopMarker, sEndOfLoopMarker, sBookmarkName As String
        Dim oLoopBookmark As Word.Bookmark
        Dim vLoopArray() As Object
        Dim MyRange As Word.Range
        Dim iLoopStart, iLoopEnd As Integer

        Dim vbookmarkarray As Object
        Dim sBookmark As String = ""
        Dim iArrayCount As Integer
        Dim bCreateLoop As Boolean

        If tvwFields.SelectedNode.GetNodeCount(False) <> 0 Then
            Exit Sub
        End If

        Dim sNodeKey As String = tvwFields.SelectedNode.Name

        tvwFields.Focus()

        Dim iTag As Integer = Convert.ToString(tvwFields.SelectedNode.Tag)
        Dim sLoop1 As String = CStr(m_vFieldArray(ACFields_Loop1, iTag))
        Dim sLoop2 As String = CStr(m_vFieldArray(ACFields_Loop2, iTag))
        Dim sLoop3 As String = CStr(m_vFieldArray(ACFields_Loop3, iTag))
        Dim sLoop4 As String = CStr(m_vFieldArray(ACFields_Loop4, iTag))

        'Is this field part of a loop.
        If sLoop1 <> "" Then

            'Compile array of loop levels.
            ReDim vLoopArray(0)

            vLoopArray(0) = sLoop1

            If sLoop2 <> "" Then
                ReDim Preserve vLoopArray(1)

                vLoopArray(1) = sLoop2
            End If

            If sLoop3 <> "" Then
                ReDim Preserve vLoopArray(2)

                vLoopArray(2) = sLoop3
            End If

            If sLoop4 <> "" Then
                ReDim Preserve vLoopArray(3)

                vLoopArray(3) = sLoop4
            End If

            'Loop through array hierarchy starting with highest level,
            'creating loop tags as necessary.

            For iLoopCount As Integer = 0 To vLoopArray.GetUpperBound(0)

                Try
                    sBookmarkName = LoopTag & Separator & CStr(vLoopArray(iLoopCount))
                    sStartOfLoopMarker = g_sFIELD_START_MARKER & LoopTag & Separator & CStr(vLoopArray(iLoopCount)) & g_sFIELD_END_MARKER & Strings.Chr(13) & Strings.Chr(10)
                    sEndOfLoopMarker = Strings.Chr(13) & Strings.Chr(10) & g_sFIELD_START_MARKER & EndLoopTag & Separator & CStr(vLoopArray(iLoopCount)) & g_sFIELD_END_MARKER

                Catch ex As Exception

                End Try
                'AJM 10/10/2001 - need to be cleverer on checking if loop exists because
                'more than one of the same loop is allowed

                'Initialize some stuff
                iArrayCount = 0
                bCreateLoop = False

                vbookmarkarray = Nothing

                'Are there any bookmarks?

                If g_oCallingApp.ActiveDocument.Bookmarks.Count > 0 Then

                    For iCount As Integer = 1 To g_oCallingApp.ActiveDocument.Bookmarks.Count
                        'Does the bookmark match our criteria?

                        If Strings.Left(g_oCallingApp.ActiveDocument.Bookmarks(iCount).Name, Len(sBookmarkName)) = sBookmarkName Then

                            sBookmark = g_oCallingApp.ActiveDocument.Bookmarks(iCount).Name
                            If Not Information.IsArray(vbookmarkarray) Then
                                ReDim vbookmarkarray(0)
                            Else
                                'Bookmark collection starts at 1 but need array to start at 0
                                ReDim Preserve vbookmarkarray(iArrayCount)
                            End If

                            vbookmarkarray(iArrayCount) = sBookmark
                            iArrayCount += 1
                        End If
                    Next iCount
                End If

                'Have we got any existing loops?
                If Not Information.IsArray(vbookmarkarray) Then
                    bCreateLoop = True
                Else
                    'We have existing loops so are we inside of an existing one?

                    For iCount As Integer = 0 To vbookmarkarray.GetUpperBound(0)

                        oLoopBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Item(vbookmarkarray(iCount))

                        If (g_oCallingApp.ActiveWindow.Selection.Start >= oLoopBookmark.Start) And (g_oCallingApp.ActiveWindow.Selection.Start <= oLoopBookmark.End) Then
                            'We are inside an existing valid loop so get out
                            bCreateLoop = False
                            Exit For
                        Else
                            bCreateLoop = True
                        End If
                    Next iCount
                End If

                sField = " "

                'Does this loop already exist.
                'If oLoopBookmark Is Nothing Then
                'AJM 10/10/2001 - change in the way we check loops
                If bCreateLoop Then

                    'Store beginning of loop position.

                    iLoopStart = g_oCallingApp.ActiveWindow.Selection.Start
                    'Debug.Print "Start = " & iLoopStart

                    'Insert loop start marker.
                    InsertText(sStartOfLoopMarker)

                    'AJM 09/10/2001 - ADD start position of bookmark to end of bookmark name to make unique
                    sBookmarkName = sBookmarkName & CStr(iLoopStart)

                    'Insert field within bookmark.

                    g_oCallingApp.ActiveWindow.Selection.InsertAfter(sField)

                    oLoopBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Add(sBookmarkName)

                    'Insert loop end marker.
                    InsertText(sEndOfLoopMarker)

                    'Store end of loop position.
                    iLoopEnd = g_oCallingApp.ActiveWindow.Selection.End

                    'Set loop beginning & end markers to be bold for clarity.
                    MyRange = Word_Global_definst.ActiveDocument.Range(Start:=iLoopStart, End:=oLoopBookmark.Start)
                    MyRange.Bold = True
                    MyRange = Word_Global_definst.ActiveDocument.Range(Start:=oLoopBookmark.End, End:=iLoopEnd)
                    MyRange.Bold = True

                    'Set current cursor position inside bookmark to enable entering
                    'child loops automatically.

                    g_oCallingApp.ActiveWindow.Selection.Start = oLoopBookmark.Start

                    g_oCallingApp.ActiveWindow.Selection.End = oLoopBookmark.Start


                End If

                oLoopBookmark = Nothing

            Next iLoopCount

        End If

        'Include field itself.
        sField = g_sFIELD_START_MARKER & sNodeKey & g_sFIELD_END_MARKER

        Select Case iInsertType
            Case 0
                ' Field inserted into current line with no CR.

            Case 1
                ' Field should be followed by CR.
                sField = sField & Strings.Chr(13) & Strings.Chr(10)

            Case 2
                ' Field should be preceded by CR.
                sField = Strings.Chr(13) & Strings.Chr(10) & sField

        End Select

        'Insert field into document.
        InsertText(sField)

        g_oCallingApp.Selection.Font.Bold = False

    End Sub

    ' ***************************************************************** '
    '
    ' Name: InsertClause
    '
    ' Description:  Creates merge field string to pass to InsertText.
    '
    ' History: 08/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function InsertClause(ByRef iClauseTCD As Integer) As Integer
        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim sMergeField As String = ""

        Dim sTCD As String = "" ' JJ 15/08/2003 IR 5011

        Try
            Dim sSelective As String = ""
            result = gPMConstants.PMEReturnCode.PMTrue

            oListItem = lvwClauses.FocusedItem

            '+++ JJ 15/08/2003 IR 5011
            sTCD = ""
            sSelective = ""
            If iClauseTCD = 1 Then
                sTCD = "C" & Separator
            End If

            If iClauseTCD = 2 Then
                sTCD = "D" & Separator
            End If


            If iClauseTCD = 3 Then
                sTCD = ""
                sSelective = StandardWordingsTag & Separator
            End If


            sMergeField = g_sFIELD_START_MARKER & sSelective & ClauseTag & Separator & sTCD & Convert.ToString(oListItem.Tag).Trim() & g_sFIELD_END_MARKER

            '--- JJ 15/08/2003 IR 5011

            'Insert field into document.
            InsertText(sMergeField)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertClause Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertClause", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        'developer guide no.184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        End If

    End Sub

    Private Sub cmdInsertFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsertFile.Click

        If txtFileName.Text.Trim() > "" Then
            InsertBookmark(FileTag, txtFileName.Text, "", "", "", "")
        End If

    End Sub

    Private Sub cmdInsertHeaders_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsertHeaders.Click
        Dim sIndicator As String = ""

        If chkIncludeHeaders.CheckState = CheckState.Checked Then
            sIndicator = "Y"
        Else
            sIndicator = "N"
        End If

        Dim sField As String = g_sFIELD_START_MARKER & RiskHeaderTag & Separator & sIndicator & g_sFIELD_END_MARKER
        InsertText(sField)


    End Sub

    Private Sub cmdInsertQuestion_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsertQuestion.Click
        Dim sField As String = ""

        If txtQuestion.Text.Trim() > "" Then
            'Thinh Nguyen 10/10/2002 start - this is question mandatory
            If chkMandatoryQuestion.CheckState = CheckState.Checked Then
                sField = g_sFIELD_START_MARKER & MandQuestionTag & Separator & txtQuestion.Text & g_sFIELD_END_MARKER

            Else
                '        InsertBookmark QuestionTag, txtQuestion.Text, "", "", ""
                sField = g_sFIELD_START_MARKER & QuestionTag & Separator & txtQuestion.Text & g_sFIELD_END_MARKER
            End If

            InsertText(sField)

            'Thinh Nguyen 10/10/2002 end - this is question mandatory
        End If

    End Sub

    Private Sub cmdInsertRiskLoop_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsertRiskLoop.Click
        Dim sField As String = ""
        Dim lDocId, lDocTypeId As Integer
        Dim sTemplateToInsert As String = ""
        Dim bRiskTypeSelected As Boolean
        Dim sRiskTypePrefix As String = ""


        If txtRiskLoopDoc.Text.Trim() > "" Then
            bRiskTypeSelected = False
            For iCount As Integer = 0 To optRiskType.GetUpperBound(0)
                If optRiskType(iCount).Checked Then
                    bRiskTypeSelected = True
                    Select Case iCount
                        Case 0
                            sRiskTypePrefix = "A"
                        Case 1
                            sRiskTypePrefix = "C"
                        Case 2
                            sRiskTypePrefix = "N"
                        Case 3
                            sRiskTypePrefix = "D"
                        Case Else

                    End Select

                    Exit For
                End If
            Next iCount
            If Not bRiskTypeSelected Then
                MessageBox.Show("Please select a Risk Type", "Field Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            sTemplateToInsert = "RK" & txtRiskLoopDoc.Text.Trim()

            m_lReturn = m_oBusiness.GetTemplateFromCode(sCode:=sTemplateToInsert, lDocId:=lDocId, lDocType:=lDocTypeId, v_dtEffectiveDate:=DateTime.Today)

            If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And (lDocId <> 0) Then
                sField = g_sFIELD_START_MARKER & RiskLoopTag & Separator & sRiskTypePrefix & txtRiskLoopDoc.Text.Trim() & g_sFIELD_END_MARKER

                InsertText(sField)
            Else
                MessageBox.Show("Template """ & sTemplateToInsert & """ not found.", "Field Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If

    End Sub

    Private Sub cmdInsertStandardWordings_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsertStandardWordings.Click
        Dim sField As String = ""

        If optStandardWordingText.Checked = False AndAlso optStandardWordingCodes.Checked = False AndAlso optStandardWordingDescription.Checked = False Then
            MessageBox.Show("Please select one of the" & vbCrLf & "standard wording option first.", "Select Option", MessageBoxButtons.OK)
            Exit Sub
        End If

        If chkStandardWordingPageBreak.CheckState = CheckState.Checked Then
            If optStandardWordingText.Checked Then
                sField = g_sFIELD_START_MARKER & StandardWordingNPTag & g_sFIELD_END_MARKER
            End If
        Else
            If optStandardWordingText.Checked Then
                sField = g_sFIELD_START_MARKER & StandardWordingsTag & g_sFIELD_END_MARKER
            ElseIf optStandardWordingCodes.Checked Then
                sField = g_sFIELD_START_MARKER & StandardWordingsCodeTag & g_sFIELD_END_MARKER
            ElseIf optStandardWordingDescription.Checked Then
                sField = g_sFIELD_START_MARKER & StandardWordingsDescTag & g_sFIELD_END_MARKER
            End If
        End If

        InsertText(sField)


    End Sub

    Public Sub frmInterfaceLoad()

        ' Position the form so that is not off the edge of the screen
        With Me

            If VB6.PixelsToTwipsX(.Left) + VB6.PixelsToTwipsX(.Width) > VB6.PixelsToTwipsX(Screen.PrimaryScreen.Bounds.Width) Then
                .Left = Screen.PrimaryScreen.Bounds.Width - .Width
            End If

            If VB6.PixelsToTwipsY(.Top) + VB6.PixelsToTwipsY(.Height) > VB6.PixelsToTwipsY(Screen.PrimaryScreen.Bounds.Height) Then
               .Top = Screen.PrimaryScreen.Bounds.Height - .Height
           End If

        End With

        SSTabHelper.SetTabVisible(tabFields, 2, False)
        'SD 19/07/2002 make sure the first tab is shown on loading
        SSTabHelper.SetSelectedIndex(tabFields, 0)


    End Sub
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        '	' Position the form so that is not off the edge of the screen
        '	With Me

        '		If VB6.PixelsToTwipsX(.Left) + VB6.PixelsToTwipsX(.Width) > VB6.PixelsToTwipsX(Screen.PrimaryScreen.Bounds.Width) Then
        '			.Left = Screen.PrimaryScreen.Bounds.Width - .Width
        '		End If

        '		If VB6.PixelsToTwipsY(.Top) + VB6.PixelsToTwipsY(.Height) > VB6.PixelsToTwipsY(Screen.PrimaryScreen.Bounds.Height) Then
        '			.Top = Screen.PrimaryScreen.Bounds.Height - .Height
        '		End If

        '	End With

        '	SSTabHelper.SetTabVisible(tabFields, 2, False)
        '	'SD 19/07/2002 make sure the first tab is shown on loading
        '	SSTabHelper.SetSelectedIndex(tabFields, 0)


    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' If user tries to close, don't unload

        If UnloadMode = vbFormControlMenu Then
            Cancel = True
            Exit Sub
        End If

        m_vFieldArray = Nothing

        eventArgs.Cancel = Cancel <> 0
    End Sub



    'UPGRADE_NOTE: (7001) The following declaration (UpdateBookMarkList) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub UpdateBookMarkList()
    '
    'Dim sBMArray() As String
    'Dim lReturn As Long
    'Dim lCount As Long
    ''
    ''
    '    lReturn& = GetBookMarks(sBMArray())
    ''
    '    lstBookmarks.Clear
    ''
    '    If (lReturn& > 0) Then
    ''
    '        If IsArray(sBMArray) Then
    '            For lCount = LBound(sBMArray) To UBound(sBMArray)
    '                lstBookmarks.AddItem sBMArray(lCount&)
    '            Next
    '        End If
    ''
    '    End If
    '
    'End Sub

    '+++ JJ 15/08/2003 IR 5011

    Private Sub lvwClauses_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwClauses.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Select Case Button
            Case MouseButtons.Right

                If Not (lvwClauses.FocusedItem Is Nothing) Then
                    Ctx_mnuClauses.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                End If

            Case Else

        End Select
    End Sub

    Public Sub mnuClause_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuClause_0.Click, _mnuClause_1.Click, _mnuClause_2.Click, _mnuClause_3.Click
        Dim Index As Integer = Array.IndexOf(mnuClause, eventSender)
        If lvwClauses.Items.Count > 0 Then
            If lvwClauses.FocusedItem.Index >= 0 Then
                InsertClause(Index)
            End If
        End If

    End Sub
    '--- JJ 15/08/2003 IR 5011

    Public Sub mnuInsert_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuInsert_0.Click, _mnuInsert_1.Click, _mnuInsert_2.Click
        Dim Index As Integer = Array.IndexOf(mnuInsert, eventSender)

        InsertField(Index)

    End Sub

    Private Sub tvwFields_AfterCollapse(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwFields.AfterCollapse
        Dim Node As TreeNode = e.Node
        Node.ImageKey = "closed"
    End Sub

    Private Sub tvwFields_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwFields.AfterExpand
        Dim Node As TreeNode = e.Node
        Node.ImageKey = "open"

        If Node.Nodes.Count > 0 AndAlso Node.Nodes(0).Text = "" Then
            Node.Nodes.Clear()
        Else
            Exit Sub
        End If

        Dim iIndex As Integer
        Dim sTableKey, sFieldName, sFieldKey As String
        Dim sSubGroup As String = ""
        Dim sLastSubGroup As String = ""
        Dim sDisplayName As String = ""
        Dim sTableName As String = ""
        Dim vSubGroup As Object
        Dim sChildKey As New StringBuilder
        Dim sParentKey As New StringBuilder
        Dim nNode As TreeNode

        If oDictMainGroup.ContainsKey(Node.Name) Then
            vSubGroup = oDictMainGroup.Item(Node.Name)

            Dim iFrom As Integer = vSubGroup(0)
            Dim iTo As Integer = vSubGroup(1)

            For lRow As Integer = iFrom To iTo
                sSubGroup = CStr(m_vFieldArray(ACFields_SubGroup, lRow))

                If sSubGroup.ToUpper() <> sLastSubGroup.ToUpper() Then
                    nNode = tvwFields.Nodes.Find(Node.Name, True)(0).Nodes.Add(Node.Name & sSubGroup.ToUpper(), sSubGroup, "closed")
                    sLastSubGroup = sSubGroup
                    nNode.SelectedImageKey = "open"
                    nNode.Nodes.Add("")
                End If
            Next

        ElseIf oDictSubGroup.ContainsKey(Node.Name) Then
            vSubGroup = oDictSubGroup.Item(Node.Name)

            Dim iFrom As Integer = vSubGroup(0)
            Dim iTo As Integer = vSubGroup(1)
            iIndex = ToSafeInteger(vSubGroup(2))

            For lRow As Integer = iFrom To iTo
                If String.IsNullOrEmpty(vSubGroup(2)) Then
                    sSubGroup = String.Empty
                Else
                    sSubGroup = CStr(m_vFieldArray(iIndex, lRow))
                End If

                If sSubGroup <> "" Then
                    If sSubGroup.ToUpper() <> sLastSubGroup.ToUpper() Then
                        nNode = tvwFields.Nodes.Find(Node.Name, True)(0).Nodes.Add(Node.Name & sSubGroup.ToUpper(), sSubGroup, "closed")
                        sLastSubGroup = sSubGroup
                        nNode.SelectedImageKey = "open"
                        nNode.Nodes.Add("")
                    End If
                Else
                    sTableName = CStr(m_vFieldArray(ACFields_SQL, lRow))

                    sTableName = sTableName.Substring(sTableName.IndexOf("_wp_") + 4)

                    If sTableName.EndsWith("_all") Then
                        sTableName = sTableName.Substring(0, sTableName.Length - 4)
                    End If

                    sTableKey = DbTag & Separator & sTableName

                    sFieldName = CStr(m_vFieldArray(ACFields_FieldName, lRow))
                    sFieldKey = sTableKey & Separator & sFieldName

                    sDisplayName = CStr(m_vFieldArray(ACFields_DisplayName, lRow))

                    nNode = tvwFields.Nodes.Find(Node.Name, True)(0).Nodes.Add(sFieldKey.ToUpper(), sDisplayName, "leaf")

                    nNode.Tag = CStr(lRow)
                End If
            Next
        End If

    End Sub


    Private Sub tvwFields_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwFields.DoubleClick
        If Not IsNothing(tvwFields.SelectedNode) Then
            InsertField(0)
        End If

    End Sub


    Public Function FillFieldTree(ByRef oBusiness As bSIRFieldManager.Business) As Integer

        Dim oNode As TreeNode
        Dim sSubGroup1Key, sSubGroup2Key, sSubGroup3Key, sSubGroup4Key As String
        Dim sMainGroup As String = ""
        Dim sLastMainGroup As String = ""
        Dim sMainGroupKey As String = ""
        Dim vSubGroup As Object

        Dim oStatus As New frmStatus
        oStatus.ShowStatus("Retrieving database fields ...")
        oStatus.Refresh()
        ' Get the field list from the Database

        m_lReturn = oBusiness.GetFieldList(m_vFieldArray)

        ' Clear tree
        tvwFields.Nodes.Clear()

        Dim sTableName As String = ""
        Dim sLastTable As String = ""

        Dim lRow As Integer = m_vFieldArray.GetLowerBound(1)

        Do

            'Only if display selected

            If CDbl(m_vFieldArray(ACFields_IsDisplayed, lRow)) = 1 Then

                sMainGroup = CStr(m_vFieldArray(ACFields_MainGroup, lRow))

                'Construct array of sub groups.
                'Make sure old array destroyed first in case we have no
                'subgroups.

                ' If main group has changed, insert a new Table Node
                If sMainGroup.ToUpper() <> sLastMainGroup.ToUpper() Then

                    sMainGroupKey = DbTag & sMainGroup.ToUpper()

                    oNode = tvwFields.Nodes.Add(sMainGroupKey, sMainGroup, "closed")

                    sLastMainGroup = sMainGroup
                    'Reset Last sub group array as sub groups are allowed to have
                    'same name now we have changed parent.
                    'ReDim vLastSubGroup(ACMaxRiskLoops - 1)

                    oNode.SelectedImageKey = "open"

                    oNode.Nodes.Add("")

                    oDictMainGroup.Add(sMainGroupKey, New Object() {lRow, lRow})
                Else
                    oDictMainGroup.Item(sMainGroupKey)(1) = lRow
                End If

                vSubGroup = Nothing
                If CStr(m_vFieldArray(ACFields_SubGroup, lRow)) <> "" Then
                    ReDim vSubGroup(0)

                    vSubGroup(0) = m_vFieldArray(ACFields_SubGroup, lRow)
                    sSubGroup1Key = sMainGroupKey & vSubGroup(0).ToString().ToUpper()

                    If Not oDictSubGroup.ContainsKey(sSubGroup1Key) Then
                        oDictSubGroup.Add(sSubGroup1Key, New Object() {lRow, lRow, ACFields_SubGroup2})
                    Else
                        oDictSubGroup.Item(sSubGroup1Key)(1) = lRow
                    End If
                End If

                If CStr(m_vFieldArray(ACFields_SubGroup2, lRow)) <> "" Then
                    ReDim Preserve vSubGroup(1)

                    vSubGroup(1) = m_vFieldArray(ACFields_SubGroup2, lRow)
                    sSubGroup2Key = sSubGroup1Key & vSubGroup(1).ToString().ToUpper()

                    If Not oDictSubGroup.ContainsKey(sSubGroup2Key) Then
                        oDictSubGroup.Add(sSubGroup2Key, New Object() {lRow, lRow, ACFields_SubGroup3})
                    Else
                        oDictSubGroup.Item(sSubGroup2Key)(1) = lRow
                    End If
                End If

                If CStr(m_vFieldArray(ACFields_SubGroup3, lRow)) <> "" Then
                    ReDim Preserve vSubGroup(2)

                    vSubGroup(2) = m_vFieldArray(ACFields_SubGroup3, lRow)
                    sSubGroup3Key = sSubGroup2Key & vSubGroup(2).ToString().ToUpper()
                   
                    If Not oDictSubGroup.ContainsKey(sSubGroup3Key) Then
                        oDictSubGroup.Add(sSubGroup3Key, New Object() {lRow, lRow, ACFields_SubGroup4})
                    Else
                        oDictSubGroup.Item(sSubGroup3Key)(1) = lRow
                    End If
                End If

                If CStr(m_vFieldArray(ACFields_SubGroup4, lRow)) <> "" Then
                    ReDim Preserve vSubGroup(3)

                    vSubGroup(3) = m_vFieldArray(ACFields_SubGroup4, lRow)
                    sSubGroup4Key = sSubGroup3Key & vSubGroup(3).ToString().ToUpper()

                    If Not oDictSubGroup.ContainsKey(sSubGroup4Key) Then
                        oDictSubGroup.Add(sSubGroup4Key, New Object() {lRow, lRow, ""})
                    Else
                        oDictSubGroup.Item(sSubGroup4Key)(1) = lRow
                    End If
                End If

            End If

            lRow += 1

        Loop Until lRow > m_vFieldArray.GetUpperBound(1)

        oStatus.ClearStatus()

    End Function


    Public Function FillFieldTreeOLD(ByRef oBusiness As Object) As Integer

        Dim oNode As TreeNode
        Dim sFieldName, sFieldKey As String
        Dim sParent, sMainGroup, sSubGroup, sLastMainGroup, sLastSubGroup, sMainGroupKey, sSubGroupKey, sDisplayName As String

        Dim oStatus As New frmStatus
        oStatus.ShowStatus("Retrieving database fields ...")
        oStatus.Refresh()

        ' Get the field list from the Database

        m_lReturn = oBusiness.GetfieldList(m_vFieldArray)

        ' Clear tree
        tvwFields.Nodes.Clear()

        '    Set oNode = tvwFields.Nodes.Add(, , DbTag, "Database", "closed")
        '    oNode.ExpandedImage = "open"

        '    Set oNode = tvwFields.Nodes.Add(, , FileTag, "File", "closed")
        '    oNode.ExpandedImage = "open"

        '    Set oNode = tvwFields.Nodes.Add(FileTag, tvwChild, FileTag & "Insert", "Insert File", "leaf")


        Dim sTableName As String = ""
        Dim sLastTable As String = ""

        Dim lRow As Integer = m_vFieldArray.GetLowerBound(1)

        Do
            '        ' Get table name
            '        sTableName$ = vFieldArray(1, lRow&)
            '
            '        'Remove the leading "sp_wp_" or "spu_wp_"
            '        sTableName$ = Mid$(sTableName, InStr(sTableName, "_wp_") + 4)
            '
            '        'Remove the trailing "_all"
            '        If (Right$(sTableName, 4) = "_all") Then
            '            sTableName = Left$(sTableName, Len(sTableName) - 4)
            '        End If
            '
            '        ' If table name has changed, insert a new Table Node
            '        If (sTableName$ <> sLastTable$) Then
            '
            '            sTableKey$ = DbTag & Separator & sTableName$
            '
            '            sParent = DbTag
            '
            '            'Set oNode = tvwFields.Nodes.Add(sParent$, tvwChild, sTableKey$, sTableName$, "closed")
            '            Set oNode = tvwFields.Nodes.Add(, , sTableKey$, sTableName$, "closed")
            '            oNode.ExpandedImage = "open"
            '
            '            sLastTable$ = sTableName$
            '        End If
            '
            '        ' Get field name
            '        sFieldName$ = vFieldArray(0, lRow&)
            '        sFieldKey$ = sTableKey$ & Separator & sFieldName$
            '
            '        ' Insert field node as child of table node
            '        Set oNode = tvwFields.Nodes.Add(sTableKey$, tvwChild, sFieldKey$, InsertSpaces(sFieldName$), "leaf")
            '
            '        lRow& = lRow& + 1

            'Only if display selected

            If CDbl(m_vFieldArray(ACFields_IsDisplayed, lRow)) = 1 Then

                'DJM 23/07/2002 : Stopped putting in table name as it isn't used (Allows for shorter merge codes).
                '            ' Get table name
                '            sTableName$ = m_vFieldArray(1, lRow&)
                '
                '            'Remove the leading "spu_wp_"
                '            sTableName$ = Mid$(sTableName, 8)
                '
                '            'Remove the trailing "_all"
                '            If (Right$(sTableName, 4) = "_all") Then
                '                sTableName = Left$(sTableName, Len(sTableName) - 4)
                '            End If
                '
                '            sTableKey = DbTag & Separator & sTableName$

                ' Get field name
                sFieldName = CStr(m_vFieldArray(ACFields_FieldName, lRow))
                sFieldKey = DbTag & Separator & sFieldName

                sMainGroup = CStr(m_vFieldArray(ACFields_MainGroup, lRow))
                sSubGroup = CStr(m_vFieldArray(ACFields_SubGroup, lRow))

                ' If main group has changed, insert a new Table Node
                If sMainGroup <> sLastMainGroup Then

                    sMainGroupKey = DbTag & sMainGroup

                    sParent = DbTag

                    'Set oNode = tvwFields.Nodes.Add(sParent$, tvwChild, sTableKey$, sTableName$, "closed")
                    oNode = tvwFields.Nodes.Add(sMainGroupKey, sMainGroup, "closed")

                    'developer guide no.210
                    oNode.SelectedImageKey = "open"

                    sLastMainGroup = sMainGroup

                    If sSubGroup = "" Then
                        sSubGroupKey = ""
                    Else
                        sSubGroupKey = DbTag & sMainGroup & sSubGroup
                        oNode = tvwFields.Nodes.Find(sMainGroupKey, True)(0).Nodes.Add(sSubGroupKey, sSubGroup, "closed")

                        'developer guide no.210
                        oNode.SelectedImageKey = "open"
                    End If

                    sLastSubGroup = sSubGroup

                End If

                If sSubGroup <> sLastSubGroup Then

                    If sSubGroup = "" Then
                        sSubGroupKey = ""
                    Else
                        sSubGroupKey = DbTag & sMainGroup & sSubGroup
                        oNode = tvwFields.Nodes.Find(sMainGroupKey, True)(0).Nodes.Add(sSubGroupKey, sSubGroup, "closed")

                        'developer guide no.210
                        oNode.SelectedImageKey = "open"
                    End If

                    sLastSubGroup = sSubGroup
                End If

                If sSubGroupKey = "" Then
                    sParent = sMainGroupKey
                Else
                    sParent = sSubGroupKey
                End If

                sDisplayName = CStr(m_vFieldArray(ACFields_DisplayName, lRow))

                ' Insert field node as child of table node
                oNode = tvwFields.Nodes.Find(sParent, True)(0).Nodes.Add(sFieldKey, sDisplayName, "leaf")

                oNode.Tag = CStr(lRow)

            End If

            lRow += 1

        Loop Until lRow > m_vFieldArray.GetUpperBound(1)

        oStatus.ClearStatus()

    End Function

    
    ''' <summary>
    ''' FillClauseList : Calls function to retrieve clauses from database and then populates the Clauses ListView.
    ''' </summary>
    ''' <param name="oBusiness"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FillClauseList(ByRef oBusiness As bSIRFieldManager.Business) As Integer
        Dim nResult As Integer = 0
        Dim oStatus As frmStatus
        Dim oListItem As ListViewItem
        Dim nDocId As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oBusiness = oBusiness

            oStatus = New frmStatus()
            oStatus.ShowStatus("Retrieving clauses ...")
            oStatus.Refresh()

            nDocId = CLng(Trim$(Mid(g_oCallingApp.ActiveWindow.Document.Name, InStr(g_oCallingApp.ActiveWindow.Document.Name, " ") + 1, 2)))

            ' Get the clause list from the Database
            m_lReturn = oBusiness.GetClauseList(nDocId, m_vClauseArray)

            ' Clear list
            lvwClauses.Items.Clear()

            If Information.IsArray(m_vClauseArray) Then
                For iClauseCount As Integer = 0 To m_vClauseArray.GetUpperBound(1)
                    oListItem = lvwClauses.Items.Add(CStr(m_vClauseArray(1, iClauseCount)))
                    oListItem.SubItems.Add(CStr(m_vClauseArray(2, iClauseCount)))
                    oListItem.Tag = CStr(m_vClauseArray(0, iClauseCount))
                Next iClauseCount
            End If

            oStatus.ClearStatus()
            oStatus = Nothing

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillClauseList Failed", vApp:=ACApp, vClass:=ACClass,vMethod:="FillClauseList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    Public Function FillFieldList(ByRef oBusiness As Object) As Integer

        Dim oNode As TreeNode
        Dim sFieldName, sFieldKey As String
        Dim lRow As Integer
        Dim oStatus As frmStatus
        Dim sParent As String = ""
        Dim vFieldArray(,) As Object

        Try

            oStatus = New frmStatus()
            oStatus.ShowStatus("Retrieving database fields ...")
            oStatus.Refresh()
            ' Get the field list from the Database

            m_lReturn = oBusiness.GetfieldList(vFieldArray)

            ' Clear tree
            tvwFields.Nodes.Clear()

            oNode = tvwFields.Nodes.Add(DbTag, "Database Fields", "closed")

            'developer guide no.210
            'oNode.ExpandedImage = "open"
            oNode.SelectedImageKey = "open"
            oNode.Expand()

            'Set oNode = tvwFields.Nodes.Add(, , FileTag, "File", "closed")
            'oNode.ExpandedImage = "open"

            'Set oNode = tvwFields.Nodes.Add(FileTag, tvwChild, FileTag & "Insert", "Insert File", "leaf")



            lRow = vFieldArray.GetLowerBound(1)


            Do
                ' Get field name from array

                sFieldName = CStr(vFieldArray(ACFields_FieldName, lRow))

                ' Insert a new node in the tree
                sFieldKey = DbTag & Separator & sFieldName
                sParent = DbTag

                oNode = tvwFields.Nodes.Find(sParent, True)(0).Nodes.Add(sFieldKey, InsertSpaces(sFieldName), "leaf")

                'developer guide no.210
                oNode.SelectedImageKey = "leaf"

                lRow += 1

            Loop Until lRow > vFieldArray.GetUpperBound(1)

            oStatus.ClearStatus()
            oStatus = Nothing

        Catch



            ' Error

            oStatus.ClearStatus()




        End Try


    End Function

    Private Sub tvwFields_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwFields.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70
        'start
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'end

        Select Case Button
            'developer guide no.64
            Case MouseButtons.Right

                If Not (tvwFields.SelectedNode Is Nothing) Then
                    Ctx_mnuField.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                End If

            Case Else

        End Select

    End Sub

    ' ***************************************************************** '
    '
    ' Name: GetAllLoopsInTemplate
    '
    ' Description: Loops through template retrieving all loops and
    '               stores their names in an array.
    '
    ' History: 24/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetAllLoopsInTemplate() As Integer
        Dim result As Integer = 0
        Dim iFileNum As Integer
        Dim sDocName, sCurrentLine, sLoopMarkerStart, sLoopMarkerEnd, sLoopName As String
        Dim iLoopNameStart As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            sDocName = g_oCallingApp.ActiveWindow.Document.FullName

            sLoopMarkerStart = g_sFIELD_START_MARKER & LoopTag
            sLoopMarkerEnd = g_sFIELD_END_MARKER & EndLoopTag

            ' Open the chosen template document
            iFileNum = FileSystem.FreeFile()
            'developer guide no.(To open the file in shared mode)
            FileSystem.FileOpen(iFileNum, sDocName, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)

            Do While Not FileSystem.EOF(iFileNum)

                sCurrentLine = FileSystem.LineInput(iFileNum)
                'Debug.Print sCurrentLine

                If sCurrentLine.IndexOf(sLoopMarkerStart) >= 0 Then
                    iLoopNameStart = (sCurrentLine.IndexOf(sLoopMarkerStart) + 1) + sLoopMarkerStart.Length + 1
                    sLoopName = sCurrentLine.Substring(iLoopNameStart - 1)
                    sLoopName = sLoopName.Substring(0, sLoopName.IndexOf(g_sFIELD_END_MARKER))

                    If Information.IsArray(m_vLoopsInDocument) Then
                        ReDim m_vLoopsInDocument(m_vLoopsInDocument.GetUpperBound(0) + 1)
                    Else
                        ReDim m_vLoopsInDocument(0)
                    End If
                    m_vLoopsInDocument(m_vLoopsInDocument.GetUpperBound(0)) = sLoopName

                End If

            Loop

            FileSystem.FileClose(iFileNum)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllLoopsInTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllLoopsInTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub txtRiskLoopDoc_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRiskLoopDoc.Enter
        txtRiskLoopDoc.SelectionStart = 0
        txtRiskLoopDoc.SelectionLength = Strings.Len(txtRiskLoopDoc.Text)
    End Sub

    Private Sub txtRiskLoopDoc_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtRiskLoopDoc.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)


        KeyAscii = Strings.Asc(Strings.Chr(KeyAscii).ToString().ToUpper()(0))

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: InsertLoopMarkers
    '
    ' Description:
    '
    ' History: 22/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (InsertLoopMarkers) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function InsertLoopMarkers(ByRef sLoopName As String) As Integer
    'Dim result As Integer = 0
    'Dim sStartMarker, sEndMarker As String
    'Dim oLoopBookmark, oEndLoopBookmark, oBookmark As Word.Bookmark
    'Dim iCounter As Integer
    'Dim sName, sBMName As String
    'Dim IsBookmark As Boolean
    '


    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'sStartMarker = LoopTag & Separator & sLoopName
    '

    'Try 
    '

    'oLoopBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Item(sStartMarker)
    '
    'sEndMarker = EndLoopTag & Separator & sLoopName
    '

    'oEndLoopBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Item(sEndMarker)
    '
    'Back to normal


    '
    'Also for now, worry only about one of each type of loop...
    'Possibly prevent insertion if outside the real loop?
    '
    'If Not (oLoopBookmark Is Nothing) Then

    'If oLoopBookmark.Start > g_oCallingApp.ActiveWindow.Selection.Start Then
    'Return result
    'End If
    'End If
    '
    'If Not (oEndLoopBookmark Is Nothing) Then

    'If oEndLoopBookmark.Start < g_oCallingApp.ActiveWindow.Selection.Start Then
    'Return result
    'End If
    'End If
    '
    'If oLoopBookmark Is Nothing Then
    '        sTemp = LoopTag & Separator & sLoopName

    'g_oCallingApp.ActiveWindow.Selection.InsertAfter(sStartMarker)

    'oLoopBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Add(sStartMarker)
    'm_lReturn = CType(MSWordInsertText(Strings.Chr(13) & Strings.Chr(10)), gPMConstants.PMEReturnCode)
    '
    'End If
    '
    ' Increment counter until the bookmark is unique
    'Do 
    'iCounter += 1
    '
    'sBMName = sName
    '
    'If iCounter% > 1 Then
    ' Set the name of bookmark to include counter at end
    'sBMName = sName & "_" & CStr(iCounter)
    'End If
    '
    ' See if the Bookmark already exists :
    ' If not, an error will occur (See error trap)
    'IsBookmark = True

    'oBookmark = g_oCallingApp.ActiveDocument.Bookmarks(sBMName)
    '
    'Loop Until Not IsBookmark
    '
    ' Insert the bookmark

    'oBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Add(sBMName)
    '
    ' Set insertion point to after bookmark

    'g_oCallingApp.ActiveWindow.Selection.Start = g_oCallingApp.ActiveWindow.Selection.End
    '
    'If sLoopName <> "" Then
    'If oEndLoopBookmark Is Nothing Then
    '                m_lReturn = MSWordInsertText(vbCrLf)
    '                sTemp = EndLoopTag & Separator & sLoopName

    'g_oCallingApp.ActiveWindow.Selection.InsertAfter(sEndMarker)

    'oLoopBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Add(sEndMarker)
    '                g_oCallingApp.ActiveWindow.Selection.Start = g_oCallingApp.ActiveWindow.Selection.End
    'End If
    'End If
    'Dim active As Word.Document

    'active = g_oCallingApp.ActiveDocument
    '
    'Dim window As Word.Window

    'window = g_oCallingApp.ActiveWindow
    '
    ' Set insertion point to after bookmark
    'window.Selection.Start = active.Bookmarks.Item(sStartMarker).End
    'window.Selection.End = active.Bookmarks.Item(sStartMarker).End
    'window.Selection.InsertAfter(Strings.Chr(13) & Strings.Chr(10))
    ' Re-Activate the application

    'g_oCallingApp.Activate()
    '
    '    End With
    '
    ' Return Successful
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Err_InsertLoopMarkers: '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' If error is caused by Bookmark not found in collection,
    ' continue to say bookmark name is not yet used.
    'If Information.Err().Number = 5941 Then
    'IsBookmark = False


    'End If
    '
    'DisplayError(Information.Err().Number, Information.Err().Description, "MSWordInsertBookmark")
    '
    'Return result
    '
    'Catch exc As System.Exception
    'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
    'End Try
    '
    'End Function

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Timer1.Enabled = False
        Timer1.Enabled = True

        If Me.WindowState <> FormWindowState.Normal Or VB6.PixelsToTwipsY(Me.Height) < 5820 Or VB6.PixelsToTwipsX(Me.Width) < 4140 Then Exit Sub

        cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1000)
        cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(2800)

        cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1000)
        cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(1495)

        tvwFields.Height = Me.Height - VB6.TwipsToPixelsY(1820)
        tvwFields.Width = Me.Width - VB6.TwipsToPixelsX(495)

        tabFields.Height = Me.Height - VB6.TwipsToPixelsY(1105)
        tabFields.Width = Me.Width - VB6.TwipsToPixelsX(195)

        If SSTabHelper.GetTabVisible(tabFields, 3) Then
            lvwClauses.Height = Me.Height - VB6.TwipsToPixelsY(1800)
            lvwClauses.Width = Me.Width - VB6.TwipsToPixelsX(435)
        End If

        If SSTabHelper.GetTabVisible(tabFields, 4) Then
            lvwSubDocuments.Height = Me.Height - VB6.TwipsToPixelsY(1800)
            lvwSubDocuments.Width = Me.Width - VB6.TwipsToPixelsX(435)
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Timer1.Tick

        If Me.WindowState <> FormWindowState.Normal Then Exit Sub

        'set minimum size
        If VB6.PixelsToTwipsY(Me.Height) < 5820 Then Me.Height = VB6.TwipsToPixelsY(5820)
        If VB6.PixelsToTwipsX(Me.Width) < 4140 Then Me.Width = VB6.TwipsToPixelsX(4140)

        Timer1.Enabled = False

    End Sub
        
    Public Function FillFieldTreeCool(ByRef oBusiness As bSIRFieldManager.Business) As Integer
        Dim result As Integer = 0


        Dim oStatus As frmStatus
        Dim lRow As Integer
        Dim sCurrentPath, sPath As String
        Dim sCurrentPathParts() As String, sPathParts() As String
        Dim sCreatePath, sCreateParentPath As String
        Dim oNode As TreeNode
        Dim sFieldKey, sDisplayName As String
        Dim lFrom, lTo As Integer
        Dim bExists As Boolean

        Try

            oStatus = New frmStatus()
            oStatus.ShowStatus("Retrieving database fields ...")
            oStatus.Refresh()
            ' Get the field list from the Database
            If m_bCalledFromSwift Then
                ' RAM20050104 : Check if we are called from SWIFT, if so, Filter the fields

                m_lReturn = oBusiness.GetFieldList(m_vFieldArray, gPMConstants.PMEProductFamily.pmePFSwift)
            Else

                m_lReturn = oBusiness.GetFieldList(m_vFieldArray)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error and exit
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oBusiness.GetfieldList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillFieldTreeCool", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                oStatus.ClearStatus()
                oStatus = Nothing
                Return result
            End If

            ' Clear tree
            tvwFields.Nodes.Clear()

            If Information.IsArray(m_vFieldArray) Then
                ' Do only if we have an array
                lFrom = m_vFieldArray.GetLowerBound(1)
                lTo = m_vFieldArray.GetUpperBound(1)

                For lPass As Integer = 1 To 2 'two passes, first for folders, second to add nodes

                    If lPass = 1 Then
                        oStatus.ShowStatus("Generating field structure ...")
                    Else
                        oStatus.ShowStatus("Adding fields ...")
                    End If

                    lRow = m_vFieldArray.GetLowerBound(1)

                    sCurrentPath = ""

                    Do
                        If gPMFunctions.ToSafeDouble(m_vFieldArray(ACFields_IsDisplayed, lRow)) = 1 Then ' check "is_displayed" field = 1

                            'build up path of node

                            sPath = CStr(m_vFieldArray(ACFields_MainGroup, lRow))

                            If sPath = "Risk" And CStr(m_vFieldArray(ACFields_DataModel, lRow)) <> "" Then 'add the data model as a level
                                sPath = sPath & "\" & CStr(m_vFieldArray(ACFields_DataModel, lRow))
                            End If

                            If CStr(m_vFieldArray(ACFields_SubGroup, lRow)) <> "" Then
                                sPath = sPath & "\" & CStr(m_vFieldArray(ACFields_SubGroup, lRow))
                            End If

                            If CStr(m_vFieldArray(ACFields_SubGroup2, lRow)) <> "" Then
                                sPath = sPath & "\" & CStr(m_vFieldArray(ACFields_SubGroup2, lRow))
                            End If

                            If CStr(m_vFieldArray(ACFields_SubGroup3, lRow)) <> "" Then
                                sPath = sPath & "\" & CStr(m_vFieldArray(ACFields_SubGroup3, lRow))
                            End If

                            If CStr(m_vFieldArray(ACFields_SubGroup4, lRow)) <> "" Then
                                sPath = sPath & "\" & CStr(m_vFieldArray(ACFields_SubGroup4, lRow))
                            End If

                            'if folder pass and path has changed then create new groups

                            If lPass = 1 And (sCurrentPath <> sPath) Then

                                sCurrentPathParts = sCurrentPath.Split("\"c)
                                sPathParts = sPath.Split("\"c)

                                sCreatePath = ""
                                sCreateParentPath = sCreatePath

                                For lDepth As Integer = sPathParts.GetLowerBound(0) To sPathParts.GetUpperBound(0)

                                    sCreateParentPath = sCreatePath

                                    If lDepth > sPathParts.GetLowerBound(0) Then
                                        sCreatePath = sCreatePath & "\"
                                    End If

                                    sCreatePath = sCreatePath & sPathParts(lDepth)

                                    'If any part of the path so far is different then we need to add it
                                    bExists = True
                                    If lDepth <= sCurrentPathParts.GetUpperBound(0) Then
                                        For lLoop As Integer = sPathParts.GetLowerBound(0) To lDepth
                                            If sCurrentPathParts(lLoop) <> sPathParts(lLoop) Then
                                                bExists = False
                                            End If
                                        Next
                                    Else
                                        bExists = False
                                    End If

                                    If Not bExists Then
                                        If sCreateParentPath = "" Then
                                            oNode = tvwFields.Nodes.Add(sCreatePath, sPathParts(lDepth), "closed")
                                        Else
                                            oNode = tvwFields.Nodes.Find(sCreateParentPath, True)(0).Nodes.Add(sCreatePath, sPathParts(lDepth), "closed")
                                        End If
                                    End If

                                Next
                                sCurrentPath = sPath
                            End If

                            If lPass = 2 Then
                                ' Get field key
                                If gPMFunctions.ToSafeDouble(m_vFieldArray(ACFields_SpecialsType, lRow)) = 5 Then
                                    sFieldKey = "STANDARDWORDINGS:" & CStr(m_vFieldArray(ACFields_ColumnName, lRow))
                                Else
                                    sFieldKey = DbTag & Separator & CStr(m_vFieldArray(ACFields_FieldName, lRow))
                                End If
                                sDisplayName = CStr(m_vFieldArray(ACFields_DisplayName, lRow))

                                ' Insert field node as child of table node
                                oNode = tvwFields.Nodes.Find(sPath, True)(0).Nodes.Add(sFieldKey, sDisplayName, "leaf")
                            End If

                            oNode.Tag = CStr(lRow)

                        End If

                        lRow += 1

                    Loop Until lRow > lTo
                Next

            End If

            oStatus.ClearStatus()
            oStatus = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            oStatus.ClearStatus()

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillFieldTreeCool Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillFieldTreeCool", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ''' <summary>
    ''' FillSubDocumentsList : Calls function to retrieve Sub Documents from database and then populates the SubDocuments ListView.
    ''' </summary>
    ''' <param name="oBusiness"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FillSubDocumentsList(ByVal oBusiness As bSIRFieldManager.Business) As Integer
        Dim nResult As Integer = 0
        Dim oStatus As frmStatus
        Dim lFrom, lTo As Integer
        Dim oListItem As ListViewItem

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oBusiness = oBusiness

            oStatus = New frmStatus()
            oStatus.ShowStatus("Retrieving Sub-Documents ...")
            oStatus.Refresh()
            ' Get the SubDocuments list from the Database

            m_lReturn = oBusiness.GetSubDocumentsList(m_vSubDocumentsArray)

            ' Clear list of Sub-Documentes
            lvwSubDocuments.Items.Clear()
            lvwSubDocuments.Columns.Item(2).Width = CInt(0) ' Hide the ID Column


            If Information.IsArray(m_vSubDocumentsArray) Then
                lFrom = 0
                lTo = m_vSubDocumentsArray.GetUpperBound(1)
                For lCounter As Integer = lFrom To lTo
                    oListItem = lvwSubDocuments.Items.Add(CStr(m_vSubDocumentsArray(1, lCounter)).Trim())
                    oListItem.SubItems.Add(CStr(m_vSubDocumentsArray(2, lCounter)))
                    oListItem.SubItems.Add(CStr(m_vSubDocumentsArray(0, lCounter)))
                    ' Store the Document Template information in the Tag
                    oListItem.Tag = CStr(m_vSubDocumentsArray(1, lCounter)).Trim() ' DocumentTemplateCode
                Next lCounter
            End If

            oStatus.ClearStatus()
            oStatus = Nothing

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillSubDocumentsList Failed",vApp:=ACApp, vClass:=ACClass, vMethod:="FillSubDocumentsList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name          : lvwSubDocuments_MouseDown
    '
    ' Description   : Event Handler Sub to handle the lvwSubDocuments MouseDown Event
    '
    ' Edit History  :
    ' RAM20050104   : Created
    ' ***************************************************************** '
    Private Sub lvwSubDocuments_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSubDocuments.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Select Case Button
            Case MouseButtons.Right

                If Not (lvwSubDocuments.FocusedItem Is Nothing) Then
                    Ctx_mnuSubDocuments.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
                End If

            Case Else

        End Select
    End Sub


    ' ***************************************************************** '
    '
    ' Name          : mnuSubDocumentInsert_Click
    '
    ' Description   : Event Handler Sub to handle the SubDocuments Menu Click
    '
    ' Edit History  :
    ' RAM20050104   : Created
    ' ***************************************************************** '
    Public Sub mnuSubDocumentInsert_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSubDocumentInsert.Click
        Dim iIndex As Integer
        If lvwSubDocuments.Items.Count > 0 Then
            iIndex = lvwSubDocuments.FocusedItem.Index + 1
            If iIndex > 0 Then
                InsertSubDocument(iIndex)
            End If
        End If
    End Sub


    ' ***************************************************************** '
    '
    ' Name          : InsertSubDocument
    '
    ' Description   : Calls function to Insert the SubDocument Placeholder
    '                   mergecode with in the Template
    '
    ' Edit History  :
    ' RAM20050104   : Created
    ' ***************************************************************** '
    Private Function InsertSubDocument(ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim sMergeField As String = ""
        Dim lDocId, lFrom, lTo As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oListItem = lvwSubDocuments.FocusedItem


            lFrom = InStr(g_oCallingApp.ActiveWindow.Document.Name, " ") + 1

            lTo = InStr(g_oCallingApp.ActiveWindow.Document.Name, Strings.Right(g_oCallingApp.ActiveWindow.Document.Name, 4))

            lDocId = CInt(Mid(g_oCallingApp.ActiveWindow.Document.Name, lFrom, lTo - lFrom).Trim())

            If lDocId = CInt(ListViewHelper.GetListViewSubItem(oListItem, 2).Text) Then
                ' We are trying to create a circular reference (include the same document within the same document)
                ' Warn the user
                MessageBox.Show("You can't insert this Sub-Document" & Strings.Chr(13) & Strings.Chr(10) & "This will create a circular reference", "Invalid Sub-Document", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            sMergeField = g_sFIELD_START_MARKER & SubDocumentTag & Separator & Convert.ToString(oListItem.Tag).Trim() & g_sFIELD_END_MARKER

            'Insert field into document.
            InsertText(sMergeField)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertSubDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertSubDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabFields.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabFields.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            tabFields.SelectedIndex = 2
        End If
        If e.Alt And e.KeyCode = Keys.D4 Then
            tabFields.SelectedIndex = 3
        End If
    End Sub

    Private Sub tvwFields_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwFields.NodeMouseClick
        Dim Node As TreeNode = e.Node
        If Node.GetNodeCount(True) = 0 Then
            Node.SelectedImageKey = "leaf"
        ElseIf Node.GetNodeCount(True) > 0 And Node.IsExpanded Then
            Node.SelectedImageKey = "open"
        Else
            Node.SelectedImageKey = "closed"
        End If
    End Sub

    Private Sub tabFields_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabFields.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabFields.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabFields.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            tabFields.SelectedIndex = 2
        End If
        If e.Alt And e.KeyCode = Keys.D4 Then
            tabFields.SelectedIndex = 3
        End If
    End Sub

    Private Sub optStandardWordingCodes_CheckedChanged(sender As Object, e As EventArgs) Handles optStandardWordingCodes.CheckedChanged
        If optStandardWordingCodes.Checked Then
            chkStandardWordingPageBreak.Enabled = False
            chkStandardWordingPageBreak.Checked = CheckState.Unchecked
        Else
            chkStandardWordingPageBreak.Enabled = True
        End If
    End Sub

    Private Sub optStandardWordingDescription_CheckedChanged(sender As Object, e As EventArgs) Handles optStandardWordingDescription.CheckedChanged
        If optStandardWordingDescription.Checked Then
            chkStandardWordingPageBreak.Enabled = False
            chkStandardWordingPageBreak.Checked = CheckState.Unchecked
        Else
            chkStandardWordingPageBreak.Enabled = True
        End If
    End Sub

    Private Sub optStandardWordingText_CheckedChanged(sender As Object, e As EventArgs) Handles optStandardWordingText.CheckedChanged
        If optStandardWordingText.Checked Then
            chkStandardWordingPageBreak.Enabled = True
        End If
    End Sub

    Private Sub cmdInsertDocBreak_Click(sender As Object, e As EventArgs) Handles cmdInsertDocBreak.Click
        Dim sTemplateToInsert As String = ""
        Dim sField As String = ""
        Dim lDocId, lDocTypeId As Integer

        If txtDocumentSplit.Text.Trim() > "" Then

            sTemplateToInsert = txtDocumentSplit.Text.Trim()

            m_lReturn = m_oBusiness.GetTemplateFromCode(sCode:=sTemplateToInsert, lDocId:=lDocId, lDocType:=lDocTypeId, v_dtEffectiveDate:=DateTime.Today)

            If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And (lDocId <> 0) Then
                sField = g_sFIELD_START_MARKER & DocumentSplitTag & Separator & sTemplateToInsert & g_sFIELD_END_MARKER

                InsertText(sField)
            Else
                MessageBox.Show("Template """ & sTemplateToInsert & """ not found.", "Field Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub
End Class
