Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    Private m_lReturn As Integer

    Private m_strServerName As String = ""
    Private m_blnFilterInDll As Boolean
    Private m_objEventLogs As cPMEventLogViewer.cEventLogs

    Private Const kDateColumn As Integer = 1

    Private Sub chkSiriusOnly_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSiriusOnly.CheckStateChanged

        m_lReturn = ReadEventLog()

    End Sub

    Private Sub cmdGo_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGo.Click

        m_lReturn = MessageBox.Show("This operation can take some time if the target machine has" & Strings.Chr(13) & Strings.Chr(10) & _
                    "a lot of messages and/or the network is slow. Proceed?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

        If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If

        ' get the contents of the event log
        ReadEventLog()

    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        m_objEventLogs = Nothing

        Me.Close()

    End Sub

    Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click

        ' get the contents of the event log
        ReadEventLog()

    End Sub

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Dim oEvent As New frmEvent

        If Not IsNothing(lvwEvents.FocusedItem) Then

            oEvent.EventIndex = CInt(Mid(lvwEvents.FocusedItem.Name, 2))
            oEvent.Parent_Renamed = Me

            oEvent.ShowDialog()

            oEvent.Close()

            oEvent = Nothing
        Else
            MessageBox.Show("Select an Item to view", "Message", MessageBoxButtons.OK)

        End If

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        m_objEventLogs = New cPMEventLogViewer.cEventLogs()

        chkSiriusOnly.CheckState = CheckState.Checked
        txtMachine.Text = ""
        'm_strServerName = "VN-007.sspdev.local"

        ' initialse the listview control
        lvwEvents.View = View.Details
        ListViewHelper.SetSortedProperty(lvwEvents, True)

        ' column headers
        lvwEvents.Columns.Clear()
        lvwEvents.Columns.Add("Type", CInt(VB6.TwipsToPixelsX(980)))
        lvwEvents.Columns.Add("Timestamp", CInt(VB6.TwipsToPixelsX(1750)))
        lvwEvents.Columns.Add("Source", CInt(VB6.TwipsToPixelsX(1500)))
        lvwEvents.Columns.Add("Category", CInt(VB6.TwipsToPixelsX(860)))
        lvwEvents.Columns.Add("Computer", CInt(VB6.TwipsToPixelsX(1650)))

        ' get the contents of the event log
        m_lReturn = ReadEventLog()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMNotFound
                    MessageBox.Show("The application event log is empty", "PMEventLogViewer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Case Else
                    MessageBox.Show("Failed to retrieve message from the application event log", "PMEventLogViewer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Select

        End If

    End Sub

    Private Function ReadEventLog() As Integer

        Dim result As Integer = 0
        Dim bSiriusOnly As Boolean

        Dim p_lngEventCount, p_lngNumActualRecs As Integer
        Dim p_strNumRecs As String = ""
        Dim p_lngNumRecs As Integer




        result = gPMConstants.PMEReturnCode.PMFalse

        ' Sirius only filter
        bSiriusOnly = chkSiriusOnly.CheckState

        lvwEvents.Items.Clear()

        ' Check the number of records to read
        p_lngNumRecs = -1

        ' Set data over 256 bytes *NOT* to be converted to Hex
        m_objEventLogs.EventDataReturnHex = False
        m_objEventLogs.EventTypeLog = "Application"

        ' Get the settings from the form

        Try
            m_objEventLogs.OpenAnyEventLog(m_strServerName)

            'developer guide no. 165
            If Information.Err().Number <> 0 Then
                MessageBox.Show("Error: " & Information.Err().Description, "Error opening Log", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            ' Set the filter
            If m_blnFilterInDll Then
                m_objEventLogs.EventFilter = g_typUserFilterData
            End If

            ' Read the event log
            Information.Err().Clear()
            Try

                p_lngNumActualRecs = m_objEventLogs.ReadEventEntries(xi_blnFilterEvents:=m_blnFilterInDll, xi_lngNumRecsToRead:=p_lngNumRecs)

            Catch excep As System.Exception
                MessageBox.Show("Error: " & excep.Message, "Error reading log", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End Try

            ' Get a count of the number of records, and display it on the main form
            p_lngEventCount = m_objEventLogs.CountEventRecords
            If p_lngEventCount = 0 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Redim the global type array to receive the data from the OLE properties.
            If p_lngEventCount > 0 Then
                ReDim g_atypEventRecords(p_lngEventCount - 1)
            End If

            ' Cycle thru the properties in the collection in the OLE server
            If p_lngNumActualRecs > 0 Then
                For p_lngLoop As Integer = 1 To p_lngNumActualRecs - 1

                    'g_atypEventRecords = New 

                    With g_atypEventRecords(p_lngLoop)
                        ' Fill in the type array
                        .EventTimeWritten = m_objEventLogs.EventTimeWritten(p_lngLoop)
                        .EventTimeCreated = m_objEventLogs.EventTimeCreated(p_lngLoop)
                        .EventSourceName = m_objEventLogs.EventSourceName(p_lngLoop)
                        '.EventUserName = m_objEventLogs.EventUserName(p_lngLoop)
                        .EventUserSID = m_objEventLogs.EventUserSID(p_lngLoop)
                        .EventComputerName = m_objEventLogs.EventComputerName(p_lngLoop)
                        .EventType = m_objEventLogs.EventType(p_lngLoop)
                        .EventDescription = m_objEventLogs.EventDescription(p_lngLoop)
                        .EventData = m_objEventLogs.EventData(p_lngLoop)
                        .EventDataText = m_objEventLogs.EventDataText(p_lngLoop)
                        .EventCategory = m_objEventLogs.EventCategory(p_lngLoop)
                        .EventCategoryString = m_objEventLogs.EventCategoryString(p_lngLoop)
                        .EventRecordNum = m_objEventLogs.EventRecordNum(p_lngLoop)
                        .EventID = m_objEventLogs.EventID(p_lngLoop)
                    End With

                    If (bSiriusOnly And g_atypEventRecords(p_lngLoop).EventSourceName.ToUpper = "PUREINSURANCE") Or Not (bSiriusOnly) Then

                        ' Add to Listview
                        AddToListView(xi_lngIndex:=p_lngLoop)
                    End If

                Next p_lngLoop
            End If

            'developer guide no. code added
            If lvwEvents.Items.Count > 0 Then
                lvwEvents.Items.Item(0).Selected = True
            End If
            Return gPMConstants.PMEReturnCode.PMTrue

Err_ReadEventLog:

            Return gPMConstants.PMEReturnCode.PMError

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function

    Private Sub AddToListView(ByVal xi_lngIndex As Integer)

        Dim sIcon As String = ""
        Dim oItem As ListViewItem

        With g_atypEventRecords(xi_lngIndex)

            Select Case .EventType
                Case "Information" : sIcon = "Info"
                Case "Warning" : sIcon = "Warning"
                Case "Error" : sIcon = "Error"
            End Select

            oItem = lvwEvents.Items.Add("k" & xi_lngIndex, sIcon, sIcon)

            oItem.SubItems.Add(Convert.ToString(DateTimeHelper.ToString(.EventTimeCreated)).Trim())
            oItem.SubItems.Add(Convert.ToString(.EventSourceName).Trim())
            oItem.SubItems.Add(Convert.ToString(.EventCategory).Trim())
            oItem.SubItems.Add(Convert.ToString(.EventComputerName).Trim())

        End With

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If VB6.PixelsToTwipsX(Me.Width) < 8745 Then
            Me.Width = VB6.TwipsToPixelsX(8745)
        End If

        If VB6.PixelsToTwipsY(Me.Height) < 5370 Then
            Me.Height = VB6.TwipsToPixelsY(5370)
        End If

        tabMain.Height = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(720)
        tabMain.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(240)

        cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdOk.Height) - 120)
        cmdOk.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(cmdOk.Width) - 120)

        lvwEvents.Width = tabMain.Width - VB6.TwipsToPixelsX(1560)
        lvwEvents.Height = tabMain.Height - VB6.TwipsToPixelsY(960)

        'cmdRefresh.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(tabMain.Width) - VB6.PixelsToTwipsX(cmdRefresh.Width) - 180)
        'cmdRefresh.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(tabMain.Height) - VB6.PixelsToTwipsY(cmdRefresh.Height) - 240)

        cmdView.Left = cmdRefresh.Left


    End Sub

    Private Sub lvwEvents_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwEvents.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwEvents.Columns(eventArgs.Column)


        ' subitem value
        Dim iIndex As Integer = ColumnHeader.Index + 1 - 1
        Dim nDirection As SortOrder

        ''Check if the index is date column index 
        If ColumnHeader.Index + 1 - 1 = kDateColumn Then

            If ListViewHelper.GetSortKeyProperty(lvwEvents) = kDateColumn Then
                nDirection = SortOrder.Descending
                lvwEvents.Sorting = SortOrder.Descending
                m_lReturn = CType(ListView6Func.ListViewSortByDate(lvwEvents, kDateColumn, nDirection), gPMConstants.PMEReturnCode)
            Else
                nDirection = SortOrder.Ascending
                lvwEvents.Sorting = SortOrder.Ascending
                m_lReturn = CType(ListView6Func.ListViewSortByDate(lvwEvents, kDateColumn, nDirection, True), gPMConstants.PMEReturnCode)
            End If

        ElseIf iIndex = ListViewHelper.GetSortKeyProperty(lvwEvents) Then
            ' change sort order of currently sorted column
            If ListViewHelper.GetSortOrderProperty(lvwEvents) = SortOrder.Ascending Then
                ListViewHelper.SetSortOrderProperty(lvwEvents, SortOrder.Descending)
            Else
                ListViewHelper.SetSortOrderProperty(lvwEvents, SortOrder.Ascending)
            End If
        Else
            ' new column to sort on
            ListViewHelper.SetSortKeyProperty(lvwEvents, iIndex)
            ListViewHelper.SetSortOrderProperty(lvwEvents, SortOrder.Ascending)
        End If

    End Sub

    Private Sub lvwEvents_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwEvents.DoubleClick

        cmdView_Click(cmdView, New EventArgs())

    End Sub

    Private Sub txtMachine_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMachine.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        m_strServerName = txtMachine.Text

    End Sub

    Private Sub txtMachine_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtMachine.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If KeyCode = 13 Then
            ' get the contents of the event log
            ReadEventLog()
        End If
    End Sub

    Private Sub chkSiriusOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSiriusOnly.CheckedChanged

    End Sub


End Class