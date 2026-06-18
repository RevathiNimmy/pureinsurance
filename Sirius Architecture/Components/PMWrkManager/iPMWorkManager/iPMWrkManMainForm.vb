Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles
Imports SListBar.ListBarControl
Friend Partial Class frmMain
    Inherits System.Windows.Forms.Form
    '* Amendment History
    '*
    '* DAK080999 - Refresh Scheduled Tasks on a regular basis
    '* DAK090999 - Allow drag to "Favourites" group only
    '* DAK090999 - Prevent dragging of non-QuickView tasks
    '* DAK100999 - Prevent dropping onto other tasks
    '* DAK130999 - Function to add a task to the Favourites Group
    '* DAK160999 - Set/unset PMNews enablement when address is changed
    '* DAK210999 - Further amendments for dragging & dropping
    '* DAK220999 - Refresh available tasks
    '* DAK091299 - Prevent crash when leaving Active list Bar
    '* DAK221299 - Add Option to remove graphics
    '* DAK030100 - Reset refresh timer when manual refresh
    '* DAK110100 - Refresh options
    '* DAK210100 - Separate User Options from System Options
    '* DAK240100 - Memo tasks can only create new scheduled task
    '* DAK190600 - Name of "PM News" tab is now held on server registry
    '******************************************************************************

    Private Declare Function OSWinHelp Lib "user32"  Alias "WinHelpA"(ByVal hwnd As Integer, ByVal HelpFile As String, ByVal wCommand As Short, ByVal dwData As Integer) As Short

    ' RDC 16052002 for return codes
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private s_HelpFile As String = ""
    Private i_HelpContext As Integer = 1
    Private m_bMoving As Boolean
    Private m_sHGap As Single
    Private m_sVGap As Single

    ' Parerent Control Class
    Private m_oParent As PMWorkManager.ControlClass

    Private m_bFormDisplayed As Boolean

    Private m_sPMNewsWebAddress As String = ""
    Private m_bViewSplash As Boolean
    Private m_bViewQuickStart As Boolean
    Private m_bViewAvailableTasks As Boolean
    'DAK231299
    ' ViewToolbar
    Private m_bViewToolbar As Boolean
    ' ViewStatusBar
    Private m_bViewStatusBar As Boolean
    ' ViewGridLines
    Private m_bViewGridLines As Boolean
    ' ViewGraphics
    Private m_bViewGraphics As Boolean
    ' ListBarPicture
    Private m_oListBarPicture As Image
    'DAK110100
    ' IsAutoRefresh
    Private m_bIsAutoRefresh As Boolean
    ' RefreshRate
    Private m_iRefreshRate As Integer

    'DAK190600
    ' WebTabCaption
    Private m_sWebTabCaption As String = ""

    'DAK110700
    ' FormCaption
    Private m_sFormCaption As String = ""
    ' PMSupportWebAddress
    Private m_sPMSupportWebAddress As String = ""

    ' RDC 16052002 toolbar website buttons
    Private m_vButtonData( ,  ) As Object


    'RFC160399 - Available Task/Quick Start Bar now uses Sheriden Active List Bar
    Private m_sCurrentAvailableTaskKey As String = ""

    'DAK210999 - Index value of list item in Favourites group
    Private m_iCurrentIndex As Integer

    'DAK130999 - Need to know if a task is in the Favourites Group
    Private m_bInFavouritesGroup As Boolean

    ' RDC 22112000 listview column widths
    Private m_sColumnWidths As String = ""

    ' RDC 17022002 combobox settings
    Private m_sComboSettings As String = ""

    Private m_fCurrentYPos As Single

    Private m_lEffect As DragDropEffects

    'RAM20020715 : Variable to hold the Selected Multiple Tasks
    Private m_vSelectedTasks As Object
    'For incresed Drop down width
    Private i As Integer
    Private lmax_wid As Integer
    Private lcur_wid As Integer
    Private m_bIsTextFilterSelected As Boolean
    'TODO
    Private lsavemode As Integer

    Private Const ACClass As String = "frmMain"

    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.2)
    Private m_sFavouritesCaption As String = ""
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.2)
    ' RDC 16052002 for website buttons
    Private Const PMWRK_WEBSITE_CODE As Integer = 0
    Private Const PMWRK_WEBSITE_DESC As Integer = 1
    Private Const PMWRK_WEBSITE_ICON As Integer = 2
    Private Const PMWRK_WEBSITE_URL As Integer = 3
    Private Const PMWRK_WEBSITE_TIP As Integer = 4

    ' Events
    Public Event ScheduledTaskAction(ByVal eAction As MainModule.ACESchedTaskAction)
    Public Event ScheduledTaskClick(ByVal v_sScheduledTaskKey As String)
    Public Event ScheduledTaskRightClick(ByVal v_sScheduledTaskKey As String, ByVal v_lSelectedUserGroupId As Integer)
    Public Event RefreshScheduledTasks(ByVal v_bForceRefresh As Boolean)
    Public Event RefreshBatchTasks(ByVal v_bForceRefresh As Boolean)
    'DAK220999
    Public Event RefreshAvailableTasks()
    Public Event NewTaskKnownType(ByVal v_sAvailableTaskKey As String)
    Public Event NewTaskUnknownType()
    Public Event DoTaskNow(ByVal v_sAvailableTaskKey As String)
    Public Event AddTaskToQSBar(ByVal v_sAvailableTaskKey As String)
    Public Event RemoveFromQSBar(ByVal v_sAvailableTaskKey As String)
    Public Event CheckForDueSystemTasks()
    Public Event FormClose()
    Public Event ChangePMNewsAddress()
    'DAK190600
    Public Event ChangeWebTabCaption()
    'DAK110700
    Public Event ChangeMainFormCaption()
    Public Event ChangePMSupportAddress()
    'DAK130999
    Public Event AddTaskToFavourites(ByVal v_sAvailableTaskKey As String, ByVal v_iIndex As Integer)
    Public Event MoveFavouriteTask(ByVal v_sAvailableTaskKey As String, ByVal v_iIndex As Integer)
    Public Event RemoveTaskFromFavourites(ByVal v_sAvailableTaskKey As String, ByVal v_iIndex As Integer)
    Public Event IncrementFavouritesIndex(ByVal v_iIndex As Integer)
    Public Event DecrementFavouritesIndex(ByVal v_iIndex As Integer)
    Public Event PMSiriusSupport()
    ' RDC 16052002
    Public Event ShowWebsite(ByVal sURL As String)
    'RAM20020712 : Multiple Tasks Assignment
    Public Event ScheduledMultipleTaskRightClick(ByVal v_vScheduledTaskKey As Object)
    Public Event ScheduledMultipleTaskAction(ByVal eAction As MainModule.ACESchedTaskAction, ByVal v_vScheduledTaskKey As Object)
    Private m_vAllocationData(,) As Object

    Public Const kBatchProcessListProcessIndex As Integer = 0
    Public Const kBatchProcessListDescriptionIndex As Integer = 1
    Public Const kBatchProcessListStartDateTimeIndex As Integer = 2
    Public Const kBatchProcessListEndDateTimeIndex As Integer = 3
    Public Const kBatchProcessListFileNameIndex As Integer = 4
    Public Const kBatchProcessListPassedRecordsIndex As Integer = 5
    Public Const kBatchProcessListFailedRecordsIndex As Integer = 6
    Public Const kBatchProcessListTotalRecordsIndex As Integer = 7
    Public Const kBatchProcessListBatchJobStatusIndex As Integer = 8
    Public Const kBatchProcessListBatchIdIndex As Integer = 9

    'RFC160399 - Available Task/Quick Start Bar now uses Sheriden Active List Bar
    Private Property CurrentAvailableTaskKey() As String
        Get
            Return m_sCurrentAvailableTaskKey.Trim()
        End Get
        Set(ByVal Value As String)
            m_sCurrentAvailableTaskKey = Value.Trim()
        End Set
    End Property

    'DAK210999 - Index value of list item in Favourites group
    Private Property CurrentIndex() As Integer
        Get
            Return m_iCurrentIndex
        End Get
        Set(ByVal Value As Integer)
            m_iCurrentIndex = Value
        End Set
    End Property
    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.2)

    Public Property FavouritesCaption() As String
        Get
            Return m_sFavouritesCaption.Trim()
        End Get
        Set(ByVal Value As String)
            m_sFavouritesCaption = Value.Trim()
        End Set
    End Property
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.2)

    Public Property FormDisplayed() As Boolean
        Get
            Return m_bFormDisplayed
        End Get
        Set(ByVal Value As Boolean)
            m_bFormDisplayed = Value
        End Set
    End Property

    Public Property Parent_Renamed() As PMWorkManager.ControlClass
        Get
            Return m_oParent
        End Get
        Set(ByVal Value As PMWorkManager.ControlClass)
            'TODO
            'If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is string) Then
            '	m_oParent = Value
            'Else
            m_oParent = Value
            'End If
        End Set
    End Property

    Public Property PMNewsWebAddress() As String
        Get
            Return m_sPMNewsWebAddress.Trim()
        End Get
        Set(ByVal Value As String)
            m_sPMNewsWebAddress = Value.Trim()
        End Set
    End Property

    Public Property ViewSplash() As Boolean
        Get
            Return m_bViewSplash
        End Get
        Set(ByVal Value As Boolean)
            m_bViewSplash = Value
        End Set
    End Property

    Public Property ViewQuickStart() As Boolean
        Get
            Return m_bViewQuickStart
        End Get
        Set(ByVal Value As Boolean)
            m_bViewQuickStart = Value
        End Set
    End Property

    Public Property ViewAvailableTasks() As Boolean
        Get
            Return m_bViewAvailableTasks
        End Get
        Set(ByVal Value As Boolean)
            m_bViewAvailableTasks = Value
        End Set
    End Property

    'DAK231299
    Public Property ViewToolbar() As Boolean
        Get
            Return m_bViewToolbar
        End Get
        Set(ByVal Value As Boolean)
            m_bViewToolbar = Value
        End Set
    End Property

    Public Property ViewStatusBar() As Boolean
        Get
            Return m_bViewStatusBar
        End Get
        Set(ByVal Value As Boolean)
            m_bViewStatusBar = Value
        End Set
    End Property

    Public Property ViewGridLines() As Boolean
        Get
            Return m_bViewGridLines
        End Get
        Set(ByVal Value As Boolean)
            m_bViewGridLines = Value
        End Set
    End Property

    Public Property ViewGraphics() As Boolean
        Get
            Return m_bViewGraphics
        End Get
        Set(ByVal Value As Boolean)
            m_bViewGraphics = Value
        End Set
    End Property

    Public Property ListBarPicture() As Object
        Get
            Return m_oListBarPicture
        End Get
        Set(ByVal Value As Object)
            If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is string) Then
                m_oListBarPicture = Value
            Else
                m_oListBarPicture = Value
            End If
        End Set
    End Property

    Public Property InFavouritesGroup() As Boolean
        Get
            Return m_bInFavouritesGroup
        End Get
        Set(ByVal Value As Boolean)
            m_bInFavouritesGroup = Value
        End Set
    End Property

    'DAK110100
    Public Property IsAutoRefresh() As Boolean
        Get
            Return m_bIsAutoRefresh
        End Get
        Set(ByVal Value As Boolean)
            m_bIsAutoRefresh = Value
        End Set
    End Property

    Public Property RefreshRate() As Integer
        Get
            Return m_iRefreshRate
        End Get
        Set(ByVal Value As Integer)
            m_iRefreshRate = Value
        End Set
    End Property

    'DAK190600
    Public Property WebTabCaption() As String
        Get
            Return m_sWebTabCaption
        End Get
        Set(ByVal Value As String)
            m_sWebTabCaption = Value
        End Set
    End Property

    'DAK110700
    Public Property FormCaption() As String
        Get
            Return m_sFormCaption
        End Get
        Set(ByVal Value As String)
            m_sFormCaption = Value
        End Set
    End Property

    Public Property PMSupportWebAddress() As String
        Get
            Return m_sPMSupportWebAddress.Trim()
        End Get
        Set(ByVal Value As String)
            m_sPMSupportWebAddress = Value.Trim()
        End Set
    End Property

    ' RDC 22112000 list view column widths

    Public Property ColumnWidths() As String
        Get
            Return m_sColumnWidths.Trim()
        End Get
        Set(ByVal Value As String)
            m_sColumnWidths = Value
        End Set
    End Property

    ' RDC 17022002 record values in comboboxes for later retrieval

    Public Property ComboSettings() As String
        Get
            Return m_sComboSettings
        End Get
        Set(ByVal Value As String)
            m_sComboSettings = Value
        End Set
    End Property
    Private Sub TabControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabMain.SelectedIndexChanged
        Dim indexOfSelectedTab As Integer = tabMain.SelectedIndex
        If indexOfSelectedTab = 0 Then
            tmrRefreshTimer.Enabled = True
        Else
            tmrRefreshTimer.Enabled = False
            RaiseEvent RefreshBatchTasks(True)
        End If
        UpdateStatusBar(v_vActivity:="")
    End Sub

    '  Private Sub albAvailableTasks_GroupClick(ByVal eventSender As Object, ByVal eventArgs As AxListbar.DSSListbarEvents_GroupClickEvent) Handles albAvailableTasks.GroupClick

    '      InFavouritesGroup = albAvailableTasks.CurrentGroupCaption = FavouritesCaption

    '  End Sub
    Private Sub albAvailableTasks_GroupClicked(sender As Object, eventSender As GroupClickedEventArgs) Handles albAvailableTasks.GroupClicked
        InFavouritesGroup = albAvailableTasks.SelectedGroup.Caption = FavouritesCaption
        albAvailableTasks.SelectedGroup.Items.Refresh()
    End Sub
    Private Sub albAvailableTasks_ItemClicked(ByVal eventSender As Object, ByVal eventArgs As ItemClickedEventArgs) Handles albAvailableTasks.ItemClicked
        'Context Menu
        CurrentAvailableTaskKey = eventArgs.Item.Key
        If (eventArgs.MouseButton = Windows.Forms.MouseButtons.Right) Then
            'Developer Guide No 103 Modified By Mohit Uniyal on 28/01/2015 
            'Ctx_mnuAvailTask.Show(albAvailableTasks, eventArgs.Location)
            Ctx_mnuAvailTask.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
            'CurrentAvailableTaskKey = eventArgs.Item.Key
        Else


            Static bAlreadyClicked As Boolean
            Dim dtTime As Date
            If Not bAlreadyClicked Then
                bAlreadyClicked = True
                'Memo tasks can only create new scheduled task
                'If EventArgs.itemClicked.TagVariant(1) = gPMConstants.PMEWrkManTaskType.pmeWMTTMemo Then
                Try
                    If eventArgs.Item.Tag(1) = gPMConstants.PMEWrkManTaskType.pmeWMTTMemo Then
                        'RaiseEvent NewTaskKnownType(Me, New NewTaskKnownTypeEventArgs(CurrentAvailableTaskKey))
                        RaiseEvent NewTaskKnownType(CurrentAvailableTaskKey)
                    Else
                        'RaiseEvent DoTaskNow(New DoTaskNowEventArgs(DirectCast(eventSender, SListBar.ListBarControl.ListBar).SelectedGroup.SelectedItem.Key))
                        RaiseEvent DoTaskNow(CurrentAvailableTaskKey)
                    End If
                Catch

                End Try
                dtTime = DateTime.Now.AddSeconds(1)

                Do While dtTime > DateTime.Now
                    Application.DoEvents()
                Loop
            End If
            bAlreadyClicked = False


        End If
    End Sub
    'Private Sub albAvailableTasks_ListItemClick(ByVal eventSender As Object, ByVal eventArgs As AxListbar.DSSListbarEvents_ListItemClickEvent) Handles albAvailableTasks.ListItemClick

    '      Static bAlreadyClicked As Boolean
    '      Dim dtTime As Date

    '      If Not bAlreadyClicked Then
    '          albAvailableTasks.Enabled = False
    '          bAlreadyClicked = True

    '              'Memo tasks can only create new scheduled task
    '              If eventArgs.ItemClicked.TagVariant(1) = gPMConstants.PMEWrkManTaskType.pmeWMTTMemo Then
    '                  RaiseEvent NewTaskKnownType(CurrentAvailableTaskKey)
    '              Else
    '                  RaiseEvent DoTaskNow(CurrentAvailableTaskKey)
    '              End If

    '              dtTime = DateTime.Now.AddSeconds(1)

    '              Do While dtTime > DateTime.Now
    '                  Application.DoEvents()
    '              Loop

    '      End If
    '            albAvailableTasks.Enabled = True
    '            bAlreadyClicked = False

    '  End Sub

    'Private Sub albAvailableTasks_ListItemEnter(ByVal ItemEntered As Listbar.SSListItem)
    '    'RFC160399 - Available Task/Quick Start Bar now uses Sheriden Active List Bar
    '    CurrentAvailableTaskKey = ItemEntered.Key
    '    'DAK090999 - Prevent dragging of non-QuickView tasks
    '    If ItemEntered.TagVariant = PMTrue Then
    '        albAvailableTasks.OLEDragMode = ssOLEDragAutomatic
    '    Else
    '        albAvailableTasks.OLEDragMode = ssOLEDragManual
    '    End If
    '
    'End Sub

    'Private Sub albAvailableTasks_ListItemExit(ByVal ItemExited As Listbar.SSListItem)
    '    'RFC160399 - Available Task/Quick Start Bar now uses Sheriden Active List Bar
    '    CurrentAvailableTaskKey = ""
    '
    'End Sub

    'Private Sub albAvailableTasks_MouseMoveEvent(ByVal eventSender As Object, ByVal eventArgs As AxListbar.DSSListbarEvents_MouseMoveEvent) Handles albAvailableTasks.MouseMoveEvent
    '    Dim oListItem As Listbar.SSListItem

    '    With albAvailableTasks
    '        If .WhereIs(eventArgs.x, eventArgs.y) <> Listbar.Constants_WhereIs.ssHitListItem Then
    '            CurrentAvailableTaskKey = ""

    '            .OLEDragMode = Listbar.Constants_OLEDrag.ssOLEDragManual
    '        Else
    '            oListItem = .ListItemFromPosition(eventArgs.x, eventArgs.y)
    '            If Not InFavouritesGroup Then
    '                CurrentAvailableTaskKey = Replace(oListItem.Key, "SEAR_", "")
    '                CurrentIndex = 0
    '            Else
    '                CurrentAvailableTaskKey = oListItem.Key.Substring(oListItem.Key.Length - (Strings.Len(oListItem.Key) - 1))
    '                CurrentIndex = oListItem.Index
    '            End If
    '            If oListItem.TagVariant(2) = gPMConstants.PMEReturnCode.PMTrue Then

    '                .OLEDragMode = Listbar.Constants_OLEDrag.ssOLEDragAutomatic
    '            Else

    '                .OLEDragMode = Listbar.Constants_OLEDrag.ssOLEDragManual
    '            End If
    '        End If
    '    End With

    'End Sub

    'Private Sub albAvailableTasks_MouseUpEvent(ByVal eventSender As Object, ByVal eventArgs As AxListbar.DSSListbarEvents_MouseUpEvent) Handles albAvailableTasks.MouseUpEvent


    '    'DAK130999 - The task key is prefixed by "F" if in the
    '    '            Favourites group
    '    If eventArgs.Button <> MouseButtonConstants.RightButton Then
    '        Exit Sub
    '    End If

    '    If albAvailableTasks.WhereIs(eventArgs.x, eventArgs.y) <> Listbar.Constants_WhereIs.ssHitListItem Then
    '        Exit Sub
    '    End If

    '    Dim oListItem As Listbar.SSListItem = albAvailableTasks.ListItemFromPosition(eventArgs.x, eventArgs.y)

    '    Dim sKey As String = oListItem.Key

    '    mnuAvailTaskScheduleNew.Enabled = True

    '    If InFavouritesGroup Then
    '        sKey = sKey.Substring(sKey.Length - (sKey.Length - 1))
    '        mnuAvailTaskAddToFavourites.Enabled = False
    '        mnuAvailTaskAddToFavourites.Available = False
    '        mnuAvailTaskRemoveFromFavourites.Enabled = True
    '        mnuAvailTaskRemoveFromFavourites.Available = True
    '        ' RDC 04102002 memo task should not have 'do now'
    '        mnuAvailTaskDoNow.Enabled = Not (oListItem.Text = "Memo Task")
    '    Else
    '        mnuAvailTaskAddToFavourites.Available = True
    '        mnuAvailTaskRemoveFromFavourites.Enabled = False
    '        mnuAvailTaskRemoveFromFavourites.Available = False
    '        ' TagVariant(2) determines if a task can be added to the
    '        ' favourites group
    '        ' RDC 04102002 can now add memos to the favourites group
    '        mnuAvailTaskAddToFavourites.Enabled = oListItem.TagVariant(2) = gPMConstants.PMEReturnCode.PMTrue Or oListItem.Text = "Memo Task"
    '        ' TagVariant(1) contains the type of task
    '        ' Memo tasks cannot be run from the available task bar
    '        mnuAvailTaskDoNow.Enabled = Not (oListItem.TagVariant(1) = gPMConstants.PMEWrkManTaskType.pmeWMTTMemo)
    '    End If

    '    CurrentAvailableTaskKey = sKey

    '    Ctx_mnuAvailTask.Show(Me, PointToClient(Cursor.Position).x, PointToClient(Cursor.Position).y)

    '    'RFC160399 - Available Task/Quick Start Bar now uses Sheriden Active List Bar
    '    '    If (Button = vbRightButton) Then
    '    '        If (sKey <> "") Then
    '    '            RaiseEvent NewTaskKnownType(sKey)
    '    '        End If
    '    '    End If
    'End Sub
    Private Sub albAvailableTasks_MouseUpEvent(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles albAvailableTasks.MouseUp


        'DAK130999 - The task key is prefixed by "F" if in the
        '            Favourites group
        If eventArgs.Button <> MouseButtons.Right Then
            Exit Sub
        End If

        mnuAvailTaskScheduleNew.Enabled = True

        If InFavouritesGroup Then
            'sKey = sKey.Substring(sKey.Length - (sKey.Length - 1))
            mnuAvailTaskAddToFavourites.Enabled = False
            mnuAvailTaskAddToFavourites.Available = False
            mnuAvailTaskRemoveFromFavourites.Enabled = True
            mnuAvailTaskRemoveFromFavourites.Available = True
            ' RDC 04102002 memo task should not have 'do now'
            mnuAvailTaskDoNow.Enabled = Not (albAvailableTasks.SelectedRightClickItemCaption = "Memo Task")

        Else
            mnuAvailTaskAddToFavourites.Available = True
             mnuAvailTaskAddToFavourites.Enabled = True
            mnuAvailTaskRemoveFromFavourites.Enabled = False
            mnuAvailTaskRemoveFromFavourites.Available = False
            ' TagVariant(2) determines if a task can be added to the
            ' favourites group
            ' RDC 04102002 can now add memos to the favourites group
            mnuAvailTaskAddToFavourites.Enabled = Not (albAvailableTasks.SelectedRightClickItemCaption = "Memo Task")
            'oListItem.TagVariant(2) = gPMConstants.PMEReturnCode.PMTrue Or oListItem.Text = "Memo Task"
            ' TagVariant(1) contains the type of task
            ' Memo tasks cannot be run from the available task bar
            mnuAvailTaskDoNow.Enabled = Not (albAvailableTasks.SelectedRightClickItemCaption = "Memo Task")
        End If


    End Sub

    'Public Sub albAvailableTasks_OLECompleteDrag(ByRef Effect As Listbar.SSReturnLong) 'Handles albAvailableTasks.OLECompleteDrag

    '    Dim oListItem As Listbar.SSListItem
    '    Dim iIndex As Integer
    '    Dim fYPos, fXMid As Single
    '    Dim iHit As Listbar.Constants_WhereIs


    '    'RFC160399 - Available Task/Quick Start Bar now uses Sheriden Active List Bar
    '    'DAK100999 - Prevent dropping onto other tasks
    '    If m_lEffect <> DragDropEffects.Move And m_lEffect <> DragDropEffects.Copy Then
    '        Exit Sub
    '    End If

    '    InFavouritesGroup = True

    '    If CurrentAvailableTaskKey = "" Then
    '        Exit Sub
    '    End If

    '    With albAvailableTasks

    '        fXMid = VB6.PixelsToTwipsX(.Width) / 2 + VB6.PixelsToTwipsX(.Left)

    '        fYPos = m_fCurrentYPos
    '        Do
    '            fYPos -= 1

    '            If fYPos < 0 Then
    '                Exit Do
    '            End If

    '            iHit = .WhereIs(fXMid, fYPos)

    '        Loop While iHit = Listbar.Constants_WhereIs.ssHitGroup

    '        If fYPos < 0 Or iHit = Listbar.Constants_WhereIs.ssHitGroupCaption Then
    '            iIndex = 1
    '        ElseIf iHit <> Listbar.Constants_WhereIs.ssHitListItem Then
    '            Exit Sub
    '        Else
    '            oListItem = .ListItemFromPosition(fXMid, fYPos)

    '            If oListItem.Index = CurrentIndex Then
    '                Exit Sub
    '            End If

    '            If CurrentIndex = 0 Or oListItem.Index < CurrentIndex Then
    '                iIndex = oListItem.Index + 1
    '            Else
    '                iIndex = oListItem.Index
    '            End If

    '        End If

    '    End With

    '    If m_lEffect = DragDropEffects.Move Then
    '        RaiseEvent MoveFavouriteTask(CurrentAvailableTaskKey, iIndex)
    '    Else
    '        RaiseEvent AddTaskToFavourites(CurrentAvailableTaskKey, iIndex)
    '    End If

    'End Sub


    'Private Sub albAvailableTasks_OLEDragOver(ByRef Data As Listbar.SSDataObject, ByRef Effect As Listbar.SSReturnLong, ByRef Button As Short, ByRef Shift As Short, ByRef x As Single, ByRef y As Single, ByRef State As Listbar.SSReturnShort)
    '    'DAK090999
    '    Dim fXMid As Single


    '    m_fCurrentYPos = y

    '    With albAvailableTasks

    '        fXMid = VB6.PixelsToTwipsX(.Width) / 2 + VB6.PixelsToTwipsX(.Left)

    '        ' Moving tasks within the Favourites Group
    '        If InFavouritesGroup Then
    '            Select Case .WhereIs(fXMid, y)
    '                Case Listbar.Constants_WhereIs.ssHitGroupCaption
    '                    Effect.Value = DragDropEffects.Scroll
    '                Case Listbar.Constants_WhereIs.ssHitGroup
    '                    Effect.Value = DragDropEffects.Move
    '                Case Else
    '                    Effect.Value = DragDropEffects.None
    '            End Select
    '            m_lEffect = Effect.Value
    '            Exit Sub
    '        End If


    '        'DAK091299
    '        If .GroupFromPosition(x, y) Is Nothing Then
    '            Effect.Value = DragDropEffects.None
    '            m_lEffect = Effect.Value
    '            Exit Sub
    '        End If

    '        ' Copying a task to the Favourites Group
    '        If .CurrentGroupCaption <> FavouritesCaption Then
    '            If .WhereIs(x, y) = Listbar.Constants_WhereIs.ssHitGroupCaption And .GroupFromPosition(x, y).Caption = FavouritesCaption Then
    '                .CurrentGroup = .GroupFromPosition(x, y)
    '                Effect.Value = DragDropEffects.Copy
    '            Else
    '                Effect.Value = DragDropEffects.None
    '            End If
    '            m_lEffect = Effect.Value
    '            Exit Sub
    '        End If

    '        ' Pasting a task in the Favourites Group
    '        Select Case .WhereIs(fXMid, y)
    '            Case Listbar.Constants_WhereIs.ssHitGroupCaption
    '                Effect.Value = DragDropEffects.Scroll
    '            Case Listbar.Constants_WhereIs.ssHitGroup
    '                Effect.Value = DragDropEffects.Copy
    '            Case Else
    '                Effect.Value = DragDropEffects.None
    '        End Select

    '        m_lEffect = Effect.Value

    '    End With

    'End Sub


    'Private Sub albAvailableTasks_OLEStartDrag(ByRef Data As Listbar.SSDataObject, ByRef AllowedEffects As Listbar.SSReturnLong)
    '    'RFC160399 - Available Task/Quick Start Bar now uses Sheriden Active List Bar
    '    'DAK090999 - Allow Copy and No drop effects

    '    AllowedEffects.Value = DragDropEffects.None Or DragDropEffects.Scroll

    '    If InFavouritesGroup Then
    '        AllowedEffects.Value = AllowedEffects.Value Or DragDropEffects.Move
    '    Else
    '        AllowedEffects.Value = AllowedEffects.Value Or DragDropEffects.Copy
    '    End If

    'End Sub

    Private Sub cboDateRange_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDateRange.SelectedIndexChanged
        ' Refresh the Scheduled Tasks List
        RaiseEvent RefreshScheduledTasks(False)
    End Sub

    Private Sub cboShowSystem_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboShowSystem.SelectedIndexChanged

        ' If the user has selected System Tasks only,
        ' then apart from the date filter the others do not apply so disable them.
        If VB6.GetItemString(cboShowSystem, cboShowSystem.SelectedIndex) = ACListShowSystemOnly Then
            cboUserGroup.Enabled = False
            cboUser.Enabled = False
            cboTaskStatus.Enabled = False
        Else
            cboUserGroup.Enabled = True
            cboUser.Enabled = True And cboUser.Enabled
            cboTaskStatus.Enabled = True
        End If

        ' Refresh the Scheduled Tasks List
        RaiseEvent RefreshScheduledTasks(False)
    End Sub

    Private Sub cboTaskStatus_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaskStatus.SelectedIndexChanged
        ' Refresh the Scheduled Tasks List
        RaiseEvent RefreshScheduledTasks(False)
    End Sub

    Private Sub cboUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUser.Click
        ' Refresh the Scheduled Tasks List
        RaiseEvent RefreshScheduledTasks(False)
    End Sub

    Private Sub cboUserGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUserGroup.Click
        ' Refresh the List of Users based on the Group selected.
        RefreshUserList()

        ' Note we do not need to raise the RefreshScheduledTasks Event
        ' as the RefreshUserList call will raise the event for us.
    End Sub

    ' RDC 23092002 browser buttons END

    Private Sub frmMain_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            ' Reset the Control Start Positions and Resize Options
            ResetControlPositions()

            AutoSizeDropDownUserControlWidth()
        End If
    End Sub


    Private Sub frmMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        With uctPMResizer1
            ' Tell the Resizer to only resize ones I tell it to.
            .NoResizeByDefault = True
            .KeepRatio = False
            .FormMinHeight = 6645
            .FormMinWidth = 9405
        End With
        'picSplitter.DoDragDrop(imgSplitter, DragDropEffects.Scroll)
        imgSplitter.AllowDrop = True
        picSplitter.AllowDrop = True
        'Me.cboUser.FirstItem = "()"
        'Me.cboTaskGroups.FirstItem = ""
        'Me.cboUserGroup.FirstItem = "(All Groups)"
        'Me.cboAllUsers.FirstItem = "()"

        'MainModule.Main()
        ' Work Out the Initial Horizontal & Vertical Gaps between
        ' the controls.
        m_sHGap = VB6.PixelsToTwipsY(albAvailableTasks.Top) - (VB6.PixelsToTwipsY(picTitles.Top) + VB6.PixelsToTwipsY(picTitles.Height))
        m_sVGap = VB6.PixelsToTwipsX(lblTitle(2).Left) - (VB6.PixelsToTwipsX(lblTitle(1).Left) + VB6.PixelsToTwipsX(lblTitle(1).Width))

        ' RDC 23112000 set column widths in listview
        SetColumnWidths()

        ' RDC 16052002 add buttons containing links to websites
        'AddWebsiteButtons()
        Dim nResult As Integer
        Dim nHasBatchProcessAuth As Integer
        nResult = m_oParent.GetBatchProcessTabUserAuthority(nHasViewBatchProcessStatus:=nHasBatchProcessAuth)
        If (nHasBatchProcessAuth = 0) Then
            tabMain.TabPages.Remove(tabMain.TabPages(1))
        End If

        MainModule.m_frm = Me

    End Sub

    Private Sub frmMain_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing

        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If Not (Parent_Renamed Is Nothing) Then
            If Parent_Renamed.NumOfTasksInProgress > 0 Then
                MessageBox.Show(ACInProgressWarning, ACInProgressTitle, MessageBoxButtons.OK)
                Cancel = True
            End If
        End If
        eventArgs.Cancel = Cancel <> 0

        ' RDC 23112000 get current column widths in listview
        m_lReturn = CType(SaveColumnWidths(), gPMConstants.PMEReturnCode)

        ' RDC 17072002 get current combo box settings
        m_lReturn = CType(SaveComboSettings(), gPMConstants.PMEReturnCode)

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmMain_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        'MRH 21/05/2004

        'PN490

        ResetControlPositions()


    End Sub

    Private Sub frmMain_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        ' Raise the Form Close Event
        RaiseEvent FormClose()

        MemoryHelper.ReleaseMemory()
    End Sub

    Private Sub lstScheduledTasks_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lstScheduledTasks.ColumnClick
        Dim ColumnHeader As ColumnHeader = lstScheduledTasks.Columns(eventArgs.Column)

        If (ColumnHeader.Index + 1 - 1) = ACSTDueDateSortableCol Then
            ' We have used a hidden column for a sortable version of the
            ' due date. This code resets the column width to zero, if the
            ' user has found the column, expanded it and clicked in it.
            lstScheduledTasks.Columns.Item(ACSTDueDateSortableCol).Width = CInt(0)
            mnuViewSortBy_Click(mnuViewSortBy(ACSTDueDateCol + 1), New EventArgs())
            'mkw100204 PN9978 START
        ElseIf ((ColumnHeader.Index + 1 - 1) = ACSTCustomerCol) Then
            lstScheduledTasks.Columns.Item(ACSTCustomerSortableCol).Width = CInt(0)
            mnuViewSortBy_Click(mnuViewSortBy(ACSTCustomerCol + 1), New EventArgs())
            'mkw100204 PN9978 END
        ElseIf ((ColumnHeader.Index + 1 - 1) = ACSTCustomerSortableCol) Then
            lstScheduledTasks.Columns.Item(ACSTCustomerSortableCol).Width = CInt(0)
            mnuViewSortBy_Click(mnuViewSortBy(ACSTCustomerCol + 1), New EventArgs())
        Else
            mnuViewSortBy_Click(mnuViewSortBy((ColumnHeader.Index + 1)), New EventArgs())
        End If

    End Sub

    Private Sub lstScheduledTasks_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstScheduledTasks.DoubleClick

        Dim vSelectedTask As Object

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20020712 : Check if the multiple lists are selected
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Go through all the listview nodes noting which are selected
        Dim iNoOfItemsSelected As Integer = 0
        For iCounter As Integer = 1 To lstScheduledTasks.Items.Count
            If lstScheduledTasks.Items.Item(iCounter - 1).Selected Then
                iNoOfItemsSelected += 1
                If iNoOfItemsSelected > 1 Then
                    ' We have multiple list selected
                    Exit For
                End If
                m_oParent.ScheduledTaskKey = lstScheduledTasks.Items.Item(iCounter - 1).Name()
            End If
        Next

        If iNoOfItemsSelected <= 1 Then


            m_vSelectedTasks = ""

            ' Do as normal
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Raise the Start Task Event
            RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAStart)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20020712 : Display the Multiple Task Assign Dialog
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Else

            ' RDC 30082002

            'TODO
            If cboUserGroup.ListIndex > 1 Then
                ' not set to a specific user group
                MessageBox.Show("Tasks cannot be selected unless User Group dropdown is" & Strings.Chr(13) & Strings.Chr(10) & _
                                "Set to a single group. Select a group from the highlighted control.", "Sirius WorkManager", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Exit Sub
            End If

            ' Get all the selected ListItem Key in an Array
            For iCounter As Integer = 1 To lstScheduledTasks.Items.Count
                If lstScheduledTasks.Items.Item(iCounter - 1).Selected Then
                    If Information.IsArray(vSelectedTask) Then

                        ReDim Preserve vSelectedTask(vSelectedTask.GetUpperBound(0) + 1)
                    Else
                        ReDim vSelectedTask(0)
                    End If


                    vSelectedTask(vSelectedTask.GetUpperBound(0)) = lstScheduledTasks.Items.Item(iCounter - 1).Name
                End If
            Next

            ' Store it at the module level variable


            m_vSelectedTasks = vSelectedTask

            ' So we have to display Multiple Task assign dialog for all the keys
            RaiseEvent ScheduledMultipleTaskAction(MainModule.ACESchedTaskAction.aceSTAStart, vSelectedTask)

        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Private Sub lstScheduledTasks_ItemClick(ByVal Item As ListViewItem)

        ' Belt and Braces Check
        If Item Is Nothing Then
            Exit Sub
        End If

        ' Raise the Scheduled Task Click Event
        RaiseEvent ScheduledTaskClick(Item.Name)

    End Sub

    'changes as the signature was not compatible
    'Private Sub lstScheduledTasks_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lstScheduledTasks.MouseUp
    Private Sub lstScheduledTasks_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstScheduledTasks.MouseUp
        'Dim Button As Integer = CInt(EventArgs.button)
        Dim Button As Integer = CInt(e.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(EventArgs.x)
        'Dim y As Single = VB6.PixelsToTwipsY(EventArgs.y)
        Dim x As Single = VB6.PixelsToTwipsX(e.X)
        Dim y As Single = VB6.PixelsToTwipsY(e.Y)
        Dim iNoOfItemsSelected As Integer
        Dim vSelectedTask As Object


        ' If Right Mouse Click
        'changes as MouseButtonConstants.RightButton not gving the desired result
        'If Button = MouseButtonConstants.RightButton Then
        iNoOfItemsSelected = 0
        For iCounter As Integer = 1 To lstScheduledTasks.Items.Count
            If lstScheduledTasks.Items.Item(iCounter - 1).Selected Then
                iNoOfItemsSelected += 1
                If iNoOfItemsSelected > 1 Then
                    ' We have multiple list selected
                    Exit For
                End If
                m_oParent.ScheduledTaskKey = lstScheduledTasks.Items.Item(iCounter - 1).Name
            End If
        Next
        If Button = MouseButtons.Right Then

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20020712 : Check if the multiple lists are selected
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Go through all the listview nodes noting which are selected
            'iNoOfItemsSelected = 0
            'For iCounter As Integer = 1 To lstScheduledTasks.Items.Count
            '    If lstScheduledTasks.Items.Item(iCounter - 1).Selected Then
            '        iNoOfItemsSelected += 1
            '        If iNoOfItemsSelected > 1 Then
            '            ' We have multiple list selected
            '            Exit For
            '        End If
            '    End If
            'Next

            If iNoOfItemsSelected <= 1 Then


                m_vSelectedTasks = ""

                'DJM 11/02/2004 PN9029 : Get the key of the one that has just been clicked on.
                If iNoOfItemsSelected = 1 Then
                    For iCounter As Integer = 1 To lstScheduledTasks.Items.Count
                        If lstScheduledTasks.Items.Item(iCounter - 1).Selected Then
                            RaiseEvent ScheduledTaskRightClick(lstScheduledTasks.Items.Item(iCounter - 1).Name, cboUserGroup.UserGroupID)
                            Exit For
                        End If
                    Next
                Else
                    RaiseEvent ScheduledTaskRightClick("", -1)
                End If

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20020712 : Display the Menu with Assign Option only
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Else

                ' RDC 30082002
                If cboUserGroup.ListIndex < 1 Then
                    ' not set to a specific user group
                    MessageBox.Show("Tasks cannot be selected unless User Group dropdown is" & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Set to a single group. Select a group from the highlighted control.", "Sirius WorkManager", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Exit Sub
                End If

                ' Get all the selected ListItem Key in an Array
                For iCounter As Integer = 1 To lstScheduledTasks.Items.Count
                    If lstScheduledTasks.Items.Item(iCounter - 1).Selected Then
                        If Information.IsArray(vSelectedTask) Then

                            ReDim Preserve vSelectedTask(vSelectedTask.GetUpperBound(0) + 1)
                        Else
                            ReDim vSelectedTask(0)
                        End If


                        vSelectedTask(vSelectedTask.GetUpperBound(0)) = lstScheduledTasks.Items.Item(iCounter - 1).Name
                    End If
                Next

                ' Store it at the module level variable


                m_vSelectedTasks = vSelectedTask

                ' So we have to display only the Assign Menu for all the keys
                RaiseEvent ScheduledMultipleTaskRightClick(vSelectedTask)

            End If

        else
            If iNoOfItemsSelected = 1 Then
                For iCounter As Integer = 1 To lstScheduledTasks.Items.Count
                    If lstScheduledTasks.Items.Item(iCounter - 1).Selected Then
                        lstScheduledTasks_ItemClick(lstScheduledTasks.Items.Item(iCounter - 1))
                        Exit For
                    End If
                Next

            End If
        End If
    End Sub


    'Private Sub mnuAvailTaskAddToQSBar_Click()
    'Dim oNode As TreeNode
    '
    ' more to do here
    '
    '    ' Get the Current Node
    '    Set oNode = treAvailableTasks.SelectedItem
    ''
    '    ' If there is no Selected Node then just exit.
    '    If (oNode Is Nothing = True) Then
    '        Exit Sub
    '    End If
    ''
    '    ' If the Selected Node is a Task Group then just exit.
    '    If (oNode.Parent Is Nothing = True) Then
    '        Exit Sub
    '    End If
    ''
    '    ' Raise the Add Task To Quick Start Bar Event
    '    RaiseEvent AddTaskToQSBar(oNode.Key)
    '
    'End Sub

    Public Sub mnuAvailTaskAddToFavourites_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuAvailTaskAddToFavourites.Click

        If CurrentAvailableTaskKey = "" Then
            Exit Sub
        End If
        If CurrentAvailableTaskKey.Substring(0, 5) = "SEAR_" Then
            CurrentAvailableTaskKey = CurrentAvailableTaskKey.Substring(5)
        End If

        RaiseEvent AddTaskToFavourites(CurrentAvailableTaskKey, 0)

    End Sub

    Public Sub mnuAvailTaskDoNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuAvailTaskDoNow.Click

        ' more to do here

        '    ' Get the Current Node
        '    Set oNode = treAvailableTasks.SelectedItem
        '
        '    ' If there is no Selected Node then just exit.
        '    If (oNode Is Nothing = True) Then
        '        Exit Sub
        '    End If
        '
        '    ' If the Selected Node is a Task Group then just exit.
        '    If (oNode.Parent Is Nothing = True) Then
        '        Exit Sub
        '    End If
        '
        '    ' The Tag tell us whether the Quick Start Functionality is available
        '    If oNode.Tag = PMTrue Then
        '        ' Raise the Do Task Now Event
        '        RaiseEvent DoTaskNow(oNode.Key)
        '    End If

        If CurrentAvailableTaskKey = "" Then
            Exit Sub
        End If
        If CurrentAvailableTaskKey.Substring(0, 5) = "SEAR_" Then
            CurrentAvailableTaskKey = CurrentAvailableTaskKey.Substring(5)
        End If
        RaiseEvent DoTaskNow(CurrentAvailableTaskKey)

    End Sub


    'Private Sub mnuAvailTaskRemoveFromQSBar_Click()
    'Dim oNode As TreeNode
    '
    ' more to do here
    '
    '    ' Get the Current Node
    '    Set oNode = treAvailableTasks.SelectedItem
    ''
    '    ' If there is no Selected Node then just exit.
    '    If (oNode Is Nothing = True) Then
    '        Exit Sub
    '    End If
    ''
    '    ' If the Selected Node is a Task Group then just exit.
    '    If (oNode.Parent Is Nothing = True) Then
    '        Exit Sub
    '    End If
    ''
    '    ' Raise the Remove Task From Quick Start Bar Event
    '    RaiseEvent RemoveFromQSBar(oNode.Key)
    '
    'End Sub

    Public Sub mnuAvailTaskRemoveFromFavourites_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuAvailTaskRemoveFromFavourites.Click

        If CurrentAvailableTaskKey = "" Then
            Exit Sub
        End If

        RaiseEvent RemoveTaskFromFavourites(CurrentAvailableTaskKey, 0)

    End Sub

    Public Sub mnuAvailTaskScheduleNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuAvailTaskScheduleNew.Click

        ' more to do here

        '    ' Get the Current Node
        '    Set oNode = treAvailableTasks.SelectedItem
        '
        '    ' If there is no Selected Node then just exit.
        '    If (oNode Is Nothing = True) Then
        '        Exit Sub
        '    End If
        '
        '    ' If the Selected Node is a Task Group then just exit.
        '    If (oNode.Parent Is Nothing = True) Then
        '        Exit Sub
        '    End If
        '
        '    ' Raise the New Event Task
        '    RaiseEvent NewTaskKnownType(oNode.Key)

        If CurrentAvailableTaskKey = "" Then
            Exit Sub
        End If
        If CurrentAvailableTaskKey.Substring(0, 5) = "SEAR_" Then
            CurrentAvailableTaskKey = CurrentAvailableTaskKey.Substring(5)
        End If
        RaiseEvent NewTaskKnownType(CurrentAvailableTaskKey)

    End Sub

    Public Sub mnuFileExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExit.Click
        Me.Close()
    End Sub

    Public Sub mnuHelpPMProductUpdateHistory_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpPMProductUpdateHistory.Click

        Dim lReturn As Integer
        Dim oHistory As iPMProductUpdateHistory.Interface_Renamed
        Try
            oHistory = New iPMProductUpdateHistory.Interface_Renamed()

            'lReturn = CType(oHistory, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            lReturn = oHistory.Initialise()

            lReturn = oHistory.Start()

            oHistory.Dispose()

            oHistory = Nothing

        Catch excep As System.Exception



            oHistory = Nothing

            MessageBox.Show("Failed to start the Product Update History program:" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                            "Error no: " & CStr(Information.Err().Number) & Strings.Chr(13) & Strings.Chr(10) & _
                            "Description: " & excep.Message, "WorkManager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try

    End Sub

    Public Sub mnuHelpPMSiriusSupport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpPMSiriusSupport.Click


        Dim sActivity As String = "Launching Internet Explorer to display the Sirius Support Web Page"
        UpdateStatusBar(v_vActivity:=sActivity)

        RaiseEvent PMSiriusSupport()

    End Sub

    ' RDC 16052002
    Public Sub mnuLinksWebsite_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuLinksWebsite_0.Click
        Dim Index As Integer = Array.IndexOf(mnuLinksWebsite, eventSender)


        Dim iTag As Integer = CInt(Convert.ToString(mnuLinksWebsite(Index).Tag))

        Dim sURL As String = CStr(m_vButtonData(PMWRK_WEBSITE_URL, iTag))

        RaiseEvent ShowWebsite(sURL)

    End Sub

    Public Sub mnuTaskAssign_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskAssign.Click

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20020715 : Check if the multiple lists are selected
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Information.IsArray(m_vSelectedTasks) Then

            ' So we have to display Multiple Task assign dialog for all the keys
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            RaiseEvent ScheduledMultipleTaskAction(MainModule.ACESchedTaskAction.aceSTAStart, m_vSelectedTasks)
            RaiseEvent RefreshScheduledTasks(True)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        Else
            ' Do as Normal

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Assign/ReAssign Currently Selected Task
            RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAAssign)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20020715 : Check if the multiple lists are selected
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Public Sub mnuTaskComplete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskComplete.Click
        ' Complete the Currently Selected Task
        RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAComplete)
    End Sub

    Public Sub mnuTaskDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskDelete.Click
        ' Delete the Currently Selected Task
        RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTADelete)
    End Sub

    Public Sub mnuTaskEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskEdit.Click
        ' Edit the Currently Selected Task
        RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAEdit)
    End Sub

    Public Sub mnuTaskIncomplete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskIncomplete.Click
        ' Incomplete the Currently Selected Task
        RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAIncomplete)
    End Sub

    Public Sub mnuTaskLog_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskLog.Click
        ' View/Edit the Task Log for the Currently Selected Task
        RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTATaskLog)
    End Sub

    Public Sub mnuTaskNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskNew.Click
        ' New Task
        RaiseEvent NewTaskUnknownType()
    End Sub

    Public Sub mnuTaskStart_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskStart.Click
        ' Start the Currently Selected Task
        RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAStart)
    End Sub

    Public Sub mnuTaskView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskView.Click
        ' View Currently Selected Task
        RaiseEvent ScheduledTaskAction(MainModule.ACESchedTaskAction.aceSTAView)
    End Sub

    Public Sub mnuViewAvailableTasks_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewAvailableTasks.Click

        If mnuViewAvailableTasks.Checked Then
            lblTitle(1).Visible = False
            albAvailableTasks.Visible = False
            mnuViewAvailableTasks.Checked = False
            imgSplitter.Visible = False
            txtFilter.Visible = False

            'Modified by Vijay Pal on 6/7/2010 4:12:42 PM ViewAvailableTasks is nt giving any result so index is passed.
            'CType(tbToolBar.Items.Item("ViewAvailableTasks"), ToolStripButton).Checked = False
            CType(tbToolBar.Items.Item(1), ToolStripButton).Checked = False
        Else
            lblTitle(1).Visible = True
            albAvailableTasks.Visible = True
            mnuViewAvailableTasks.Checked = True
            imgSplitter.Visible = True
            txtFilter.Visible = True
            'Modified by Vijay Pal on 6/7/2010 4:12:42 PM ViewAvailableTasks is nt giving any result so index is passed.
            'CType(tbToolBar.Items.Item("ViewAvailableTasks"), ToolStripButton).Checked = True
            CType(tbToolBar.Items.Item(1), ToolStripButton).Checked = True
        End If

        ViewAvailableTasks = mnuViewAvailableTasks.Checked

        ResetControlPositions()

    End Sub

    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        'TODO
        Dim oPMAbout As iPMAbout.Interface_Renamed


        Dim sTitle, sVersionNumber, sVersionDate, sComponent As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sVersion, sRelease, sSiriusType, sInstallDate As String

        Try

            ' Set the application title
            sTitle = "Sirius Architecture Tools"
            sComponent = "Work Manager"

            ' get Sirius version info
            lReturn = CType(gPMFunctions.GetSiriusVersion(sVersion, sRelease, sSiriusType, sInstallDate), gPMConstants.PMEReturnCode)

            ' Set the version number and date
            If sSiriusType = "" Then
                sVersionNumber = "Sirius Architecture v" & sVersion & ", sr" & sRelease
            Else
                sVersionNumber = "SSP Pure " & sSiriusType & " v" & sVersion & ", sr" & sRelease
            End If

            sVersionDate = sInstallDate

            ' Create the object
            'TODO
            oPMAbout = New iPMAbout.Interface_Renamed()


            ' Initialise it. No parameters
            lReturn = oPMAbout.Initialise()

            ' Display the about screen modally
            lReturn = oPMAbout.Show(sTitle:=sTitle, sVersionNumber:=sVersionNumber, sVersionDate:=sVersionDate, sComponent:=sComponent)

            ' Terminate it, and...
            oPMAbout.Dispose()

            ' ...remove it from memory
            oPMAbout = Nothing

        Catch



            Exit Sub
        End Try


    End Sub


    ' RDC 23112000 set default column widths in listview
    Public Sub mnuViewColumns_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewColumns.Click

        'm_sColumnWidths = "400;800;600;1440;1440;1600;1440;600;0;"
        m_sColumnWidths = "40;80;60;80;60;160;144;60;0;"

        SetColumnWidths()

    End Sub

    'DAK210100
    Public Sub mnuViewSystemOptions_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewSystemOptions.Click
        Dim fOptions As frmSystemOptions


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            fOptions = New frmSystemOptions()

            'Developer Guide No. 68(Guide)
            'Load(fOptions)

            With fOptions
                .PMNewsAddress = PMNewsWebAddress
                'DAK190600
                .WebTabCaption = WebTabCaption
                'DAK110700
                .MainFormCaption = FormCaption
                .PMSupportAddress = PMSupportWebAddress

                .ShowDialog()

                If Not .Cancelled Then
                    If PMNewsWebAddress <> .PMNewsAddress Then
                        PMNewsWebAddress = .PMNewsAddress
                        RaiseEvent ChangePMNewsAddress()
                    End If
                    'DAK190600
                    If WebTabCaption <> .WebTabCaption Then
                        WebTabCaption = .WebTabCaption
                        RaiseEvent ChangeWebTabCaption()
                    End If

                    'DAK110700
                    If FormCaption <> .MainFormCaption Then
                        FormCaption = .MainFormCaption
                        RaiseEvent ChangeMainFormCaption()
                    End If

                    If PMSupportWebAddress <> .PMSupportAddress Then

                        PMSupportWebAddress = .PMSupportAddress
                        RaiseEvent ChangePMSupportAddress()

                        If PMSupportWebAddress = "" Then
                            tbToolBar.Items.Item(5).Enabled = False
                            mnuHelpPMSiriusSupport.Enabled = False
                        Else
                            tbToolBar.Items.Item(5).Enabled = True
                            mnuHelpPMSiriusSupport.Enabled = True
                        End If

                    End If

                End If

            End With

            fOptions.Close()
            fOptions = Nothing

            'DAK150999
            If PMNewsWebAddress = "" Then
                SSTabHelper.SetTabEnabled(tabMain, 1, False)
            Else
                SSTabHelper.SetTabEnabled(tabMain, 1, True)
            End If

            'DAK190600
            If WebTabCaption = "" Then
                SSTabHelper.SetTabCaption(tabMain, 1, "&News")
            Else
                SSTabHelper.SetTabCaption(tabMain, 1, WebTabCaption)
            End If

        Catch



            fOptions = Nothing

            Exit Sub
        End Try


    End Sub

    Public Sub mnuViewGridLines_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewGridLines.Click

        ' m_lReturn isnt declared for some reason!!!!

        Dim lReturn As Integer

        ' CF 110599 - Toggle grid lines on list view

        If mnuViewGridLines.Checked Then

            ' Turn them off
            mnuViewGridLines.Checked = False
            lReturn = SetExtraListViewProperties(v_hWndList:=lstScheduledTasks.Handle.ToInt32(), v_vShowGridLines:=False)
            lstScheduledTasks.GridLines = False
            'ViewGridLines = False

        Else

            ' Display them
            mnuViewGridLines.Checked = True
            lReturn = SetExtraListViewProperties(v_hWndList:=lstScheduledTasks.Handle.ToInt32(), v_vShowGridLines:=True)
            lstScheduledTasks.GridLines = True
            'ViewGridLines = True
        End If

        'DAK241299
        ViewGridLines = mnuViewGridLines.Checked
        lstScheduledTasks.CheckBoxes = False

    End Sub

    Private Sub mnuViewQuickStartBar_Click()

        ' more to do here

        '    If mnuViewQuickStartBar.Checked Then
        '        lblTitle(0).Visible = False
        '        panQuickStart.Visible = False
        '        tbQuickStart.Visible = False
        '        mnuViewQuickStartBar.Checked = False
        '        tbToolBar.Buttons("ViewQuickStartBar").Value = tbrUnpressed
        '    Else
        '        lblTitle(0).Visible = True
        '        panQuickStart.Visible = True
        '        tbQuickStart.Visible = True
        '        mnuViewQuickStartBar.Checked = True
        '        tbToolBar.Buttons("ViewQuickStartBar").Value = tbrPressed
        '    End If
        '
        '    ViewQuickStart = mnuViewQuickStartBar.Checked
        '
        '    ResetControlPositions

    End Sub

    Public Sub mnuViewSortBy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuViewSortBy_1.Click, _mnuViewSortBy_2.Click, _mnuViewSortBy_3.Click, _mnuViewSortBy_4.Click, _mnuViewSortBy_5.Click, _mnuViewSortBy_6.Click, _mnuViewSortBy_7.Click, _mnuViewSortBy_8.Click, _mnuViewSortBy_11.Click
        Dim Index As Integer = Array.IndexOf(mnuViewSortBy, eventSender)

        ' Set the List View Sort Column to be the
        ' Menu Index. The two must match for this to work.

        With lstScheduledTasks

            ' Turn off sorting so that the list is not sorted twice
            ListViewHelper.SetSortedProperty(lstScheduledTasks, False)

            ' Reset key so that re-click is recognised see below
            If ListViewHelper.GetSortKeyProperty(lstScheduledTasks) = ACSTDueDateSortableCol Then
                ListViewHelper.SetSortKeyProperty(lstScheduledTasks, ACSTDueDateCol)
            End If

            'mkw100204 PN9978
            If ListViewHelper.GetSortKeyProperty(lstScheduledTasks) = ACSTCustomerSortableCol Then
                ListViewHelper.SetSortKeyProperty(lstScheduledTasks, ACSTCustomerCol)
            End If

            ' If we are already sorted by this Column
            If Index - 1 = ListViewHelper.GetSortKeyProperty(lstScheduledTasks) Then
                ' Set sort order opposite of current direction
                If ListViewHelper.GetSortOrderProperty(lstScheduledTasks) = SortOrder.Ascending Then
                    ListViewHelper.SetSortOrderProperty(lstScheduledTasks, SortOrder.Descending)
                Else
                    ListViewHelper.SetSortOrderProperty(lstScheduledTasks, SortOrder.Ascending)
                End If
            Else

                ' Uncheck the current Selection
                mnuViewSortBy(ListViewHelper.GetSortKeyProperty(lstScheduledTasks) + 1).Checked = False

                ' Sort by this column
                ListViewHelper.SetSortKeyProperty(lstScheduledTasks, Index - 1)
                ' Ascending
                ListViewHelper.SetSortOrderProperty(lstScheduledTasks, SortOrder.Ascending)


                ' Check the new selection
                mnuViewSortBy(Index).Checked = True

            End If

            ' If we're sorting the date use the hidden yyyymmdd date col
            ' so that we get the dates in cronological order
            If ListViewHelper.GetSortKeyProperty(lstScheduledTasks) = ACSTDueDateCol Then
                ListViewHelper.SetSortKeyProperty(lstScheduledTasks, ACSTDueDateSortableCol)
            End If

            'mkw100204 PN9978
            If ListViewHelper.GetSortKeyProperty(lstScheduledTasks) = ACSTCustomerCol Then
                ListViewHelper.SetSortKeyProperty(lstScheduledTasks, ACSTCustomerSortableCol)
            End If

            ' Turn on sorting
            ListViewHelper.SetSortedProperty(lstScheduledTasks, True)

        End With
        ' ListViewFunc.SortListView(lstScheduledTasks, eventArgs)

    End Sub

    Public Sub mnuViewSplashScreen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewSplashScreen.Click
        mnuViewSplashScreen.Checked = Not mnuViewSplashScreen.Checked
        ViewSplash = mnuViewSplashScreen.Checked
    End Sub

    Public Sub mnuViewStatusBar_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewStatusBar.Click
        If mnuViewStatusBar.Checked Then
            sbStatusBar.Visible = False
            mnuViewStatusBar.Checked = False
            '        tbToolBar.Buttons("ViewStatusBar").Value = tbrUnpressed
        Else
            sbStatusBar.Visible = True
            mnuViewStatusBar.Checked = True
            '        tbToolBar.Buttons("ViewStatusBar").Value = tbrPressed
        End If

        ResetControlPositions()

        'DAK241299
        ViewStatusBar = mnuViewStatusBar.Checked

    End Sub


    Public Sub mnuViewToolbar_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewToolbar.Click
        If mnuViewToolbar.Checked Then
            tbToolBar.Visible = False
            mnuViewToolbar.Checked = False
            '        tbToolBar.Buttons("ViewToolBar").Value = tbrUnpressed
        Else
            tbToolBar.Visible = True
            mnuViewToolbar.Checked = True
            '        tbToolBar.Buttons("ViewToolBar").Value = tbrPressed
        End If

        ResetControlPositions()

        'DAK241299
        ViewToolbar = mnuViewToolbar.Checked

    End Sub

    Private Sub imgSplitter_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitter.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        imgSplitter.Height = albAvailableTasks.Height
        With imgSplitter
            picSplitter.SetBounds(.Left, .Top, VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(.Width) \ 2), VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(.Height) - 20))
        End With
        picSplitter.Visible = True
        m_bMoving = True
    End Sub


    Private Sub imgSplitter_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitter.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim sglPos As Single

        Dim sglSplitLimit As Single = 1000
        If m_bMoving Then
            sglPos = x + VB6.PixelsToTwipsX(imgSplitter.Left)
            If sglPos < sglSplitLimit Then
                picSplitter.Left = VB6.TwipsToPixelsX(sglSplitLimit)
            ElseIf sglPos > VB6.PixelsToTwipsX(Me.Width) - sglSplitLimit Then
                picSplitter.Left = Me.Width - VB6.TwipsToPixelsX(sglSplitLimit)
            Else
                picSplitter.Left = VB6.TwipsToPixelsX(sglPos)
            End If
        End If
    End Sub


    Private Sub imgSplitter_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitter.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        imgSplitter.Height = albAvailableTasks.Height
        albAvailableTasks.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(picSplitter.Left) - VB6.PixelsToTwipsX(albAvailableTasks.Left) - 50)

        panMainTab.Left = picSplitter.Left + VB6.TwipsToPixelsX(50)
        panMainTab.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(panMainTab.Left) - 20)
        lblTitle(2).Left = panMainTab.Left
        lblTitle(2).Width = panMainTab.Width
        albAvailableTasks.Width = picSplitter.Left - albAvailableTasks.Left
        txtFilter.Width = picSplitter.Left - albAvailableTasks.Left
        lblTitle(1).Width = albAvailableTasks.Width

        picSplitter.Visible = False
        m_bMoving = False
        ResetControlPositions()
    End Sub

    Public Sub mnuViewUserOptions_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewUserOptions.Click
        Dim fOptions As frmUserOptions
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim iViewToolbar As CheckState
        Dim iViewAvailableTasks As CheckState
        Dim iViewStatusBar As CheckState
        Dim iViewSplashScreen As CheckState
        Dim iViewGridLines As CheckState
        Dim iViewGraphics As CheckState


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If mnuViewToolbar.Checked Then
                iViewToolbar = CheckState.Checked
            Else
                iViewToolbar = CheckState.Unchecked
            End If

            If mnuViewToolbar.Enabled Then
                iViewToolbar += ACChkEnabled
            End If

            If mnuViewAvailableTasks.Checked Then
                iViewAvailableTasks = CheckState.Checked
            Else
                iViewAvailableTasks = CheckState.Unchecked
            End If

            If mnuViewAvailableTasks.Enabled Then
                iViewAvailableTasks += ACChkEnabled
            End If

            If mnuViewStatusBar.Checked Then
                iViewStatusBar = CheckState.Checked
            Else
                iViewStatusBar = CheckState.Unchecked
            End If

            If mnuViewStatusBar.Enabled Then
                iViewStatusBar += ACChkEnabled
            End If

            If mnuViewSplashScreen.Checked Then
                iViewSplashScreen = CheckState.Checked
            Else
                iViewSplashScreen = CheckState.Unchecked
            End If

            If mnuViewSplashScreen.Enabled Then
                iViewSplashScreen += ACChkEnabled
            End If

            If mnuViewGridLines.Checked Then
                iViewGridLines = CheckState.Checked
            Else
                iViewGridLines = CheckState.Unchecked
            End If

            If mnuViewGridLines.Enabled Then
                iViewGridLines += ACChkEnabled
            End If

            If ViewGraphics Then
                iViewGraphics = CheckState.Checked
            Else
                iViewGraphics = CheckState.Unchecked
            End If

            iViewGraphics += ACChkEnabled

            fOptions = New frmUserOptions()

            'Developer Guide No. 68(Guide)
            'Load(fOptions)

            With fOptions
                .ViewToolbar = iViewToolbar
                .ViewAvailableTasks = iViewAvailableTasks
                .ViewStatusBar = iViewStatusBar
                .ViewSplashScreen = iViewSplashScreen
                .ViewGridLines = iViewGridLines
                .ViewGraphics = iViewGraphics
                'DAK110100
                .IsAutoRefresh = IsAutoRefresh
                .RefreshRate = RefreshRate

                .ShowDialog()

                If Not .Cancelled Then
                    If .ViewGraphics <> iViewGraphics Then

                        If .ViewGraphics And CheckState.Checked Then
                            lReturn = CType(SetGraphics(True), gPMConstants.PMEReturnCode)
                        Else
                            lReturn = CType(SetGraphics(False), gPMConstants.PMEReturnCode)
                        End If

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception()
                        End If

                        ' If ViewGraphics has changed, reset ViewSplashScreen
                        If mnuViewSplashScreen.Checked Then
                            iViewSplashScreen = CheckState.Checked
                        Else
                            iViewSplashScreen = CheckState.Unchecked
                        End If

                        If mnuViewSplashScreen.Enabled Then
                            iViewSplashScreen += ACChkEnabled
                        End If

                    End If

                    If .ViewToolbar <> iViewToolbar Then
                        mnuViewToolbar_Click(mnuViewToolbar, New EventArgs())
                    End If

                    If .ViewAvailableTasks <> iViewAvailableTasks Then
                        mnuViewAvailableTasks_Click(mnuViewAvailableTasks, New EventArgs())
                    End If

                    If .ViewStatusBar <> iViewStatusBar Then
                        mnuViewStatusBar_Click(mnuViewStatusBar, New EventArgs())
                    End If

                    If .ViewSplashScreen <> iViewSplashScreen Then
                        mnuViewSplashScreen_Click(mnuViewSplashScreen, New EventArgs())
                    End If

                    If .ViewGridLines <> iViewGridLines Then
                        mnuViewGridLines_Click(mnuViewGridLines, New EventArgs())
                    End If
                    'DAK110100
                    If .IsAutoRefresh <> IsAutoRefresh Then
                        IsAutoRefresh = .IsAutoRefresh
                        'DAK240700 - Use new Refresh Timer
                        tmrRefreshTimer.Enabled = IsAutoRefresh
                    End If

                    If .RefreshRate <> RefreshRate Then
                        RefreshRate = .RefreshRate
                    End If

                End If

            End With

            fOptions.Close()
            fOptions = Nothing

        Catch



            fOptions = Nothing

            Exit Sub
        End Try


    End Sub

    Private Sub tabMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMain.KeyDown
        Dim strKey As String = tabMain.TabPages(0).Text
        strKey = strKey.Substring(1, 1)
        'strKey = "Keys." & strKey

        'If e.Alt And e.KeyCode = Keys.I Then
        If e.Alt And e.KeyCode = Asc(strKey) Then
            tabMain.SelectedIndex = 1
            tabMain.Focus()
        End If
        If e.Alt And e.KeyCode = Keys.S Then
            tabMain.SelectedIndex = 0
            tabMain.Focus()
        End If
    End Sub


    'Private Sub tbQuickStart_ButtonClick(ByVal Button As ToolStripButton)
    '
    ' Raise the Do Task Now Event.
    ' The Button Key is the Available Task key
    'RaiseEvent DoTaskNow(Button.Name)
    '
    'End Sub


    Private Sub tbToolBar_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _tbToolBar_Button2.Click, _tbToolBar_Button3.Click, _tbToolBar_Button4.Click, _tbToolBar_Button5.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Dim sWeb As String = ""

        ' RDC 16052002
        If Button.Name.ToUpper().Substring(0, 3) = "WEB" Then
            DisplayWebsite(Button.Name)
            Exit Sub
        End If

        Select Case Button.Name
            Case "_tbToolBar_Button4"
                mnuViewRefresh_Click(mnuViewRefresh, New EventArgs())
            Case "_tbToolBar_Button1"
                mnuViewQuickStartBar_Click()
            Case "_tbToolBar_Button2"
                mnuViewAvailableTasks_Click(mnuViewAvailableTasks, New EventArgs())
            Case "_tbToolBar_Button6"
                mnuHelpPMSiriusSupport_Click(mnuHelpPMSiriusSupport, New EventArgs())
        End Select

    End Sub

    ' RDC 16052002 display website
    Private Sub DisplayWebsite(ByVal sKey As String)


        ' search for the key and get the URL
        Dim sURL As String = ""
        Dim iLoop As Integer = m_vButtonData.GetLowerBound(1)

        Do Until iLoop > m_vButtonData.GetUpperBound(1) Or sURL <> ""
            If CStr(m_vButtonData(PMWRK_WEBSITE_CODE, iLoop)) = sKey Then
                sURL = CStr(m_vButtonData(PMWRK_WEBSITE_URL, iLoop))
            End If

            iLoop += 1
        Loop

        If sURL = "" Then
            ' not found
            Exit Sub
        End If

        RaiseEvent ShowWebsite(sURL)

    End Sub

    Public Sub mnuHelpContents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpContents.Click

        Try


            PMHelpFunc.g_sProductFamily = g_sProductFamily

            PMHelpFunc.ShowHelp(Me, 1)



        Catch



            Exit Sub
        End Try


    End Sub


    Public Sub mnuHelpSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpSearch.Click

        Try




            PMHelpFunc.g_sProductFamily = g_sProductFamily
            PMHelpFunc.ShowHelp(Me)


        Catch



            Exit Sub
        End Try


    End Sub


    Public Sub mnuViewRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewRefresh.Click

        'RaiseEvent RefreshAvailableTasks()
        m_oParent.m_fMainForm_RefreshAvailableTasks()
        Select Case SSTabHelper.GetSelectedIndex(tabMain)
            Case 0
                RaiseEvent RefreshScheduledTasks(True)
            Case 1
                brwWebBrowser.Refresh()
            Case Else
                ' Do Nothing
        End Select
        ' RaiseEvent RefreshScheduledTasks(True)
        RaiseEvent RefreshScheduledTasks(Nothing)
        ' RDC 05112003 only reset timer if user option enabled
        If m_bIsAutoRefresh Then
            tmrRefreshTimer.Enabled = False
            tmrRefreshTimer.Enabled = True
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: ResetControlPositions
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Sub ResetControlPositions()

        Try

            albAvailableTasks.Top = txtFilter.Top + txtFilter.Height + 5

            If mnuViewStatusBar.Checked Then
                albAvailableTasks.Height = Me.ClientRectangle.Height - txtFilter.Height - albAvailableTasks.Top
            Else
                albAvailableTasks.Height = Me.ClientRectangle.Height - txtFilter.Height - albAvailableTasks.Top + sbStatusBar.Height
            End If

            panMainTab.Top = txtFilter.Top
            panMainTab.Height = albAvailableTasks.Height + m_sHGap + (txtFilter.Height + 2)

            If mnuViewAvailableTasks.Checked Then
                panMainTab.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(albAvailableTasks.Left) + VB6.PixelsToTwipsX(albAvailableTasks.Width)) + 5
            Else
                panMainTab.Left = 0
            End If

            panMainTab.Width = Me.ClientRectangle.Width - panMainTab.Left

            lblTitle(1).Width = albAvailableTasks.Width + 5
            lblTitle(2).Left = panMainTab.Left
            lblTitle(2).Width = panMainTab.Width

            tabMain.Width = panMainTab.Width
            tabMain.Height = panMainTab.Height
            tabMain.Left = 0

            lstScheduledTasks.Width = tabMain.ClientRectangle.Width - lstScheduledTasks.Left - 15
            lstScheduledTasks.Height = albAvailableTasks.Height - txtFilter.Height - 20
            txtFilter.Width = albAvailableTasks.Width - albAvailableTasks.Left + 3

            picSplitter.Left = albAvailableTasks.Left + albAvailableTasks.Width
            picSplitter.Height = albAvailableTasks.Height
            picSplitter.Width = 5
            imgSplitter.Left = picSplitter.Left
            imgSplitter.Height = picSplitter.Height
            imgSplitter.Width = 5

            lstBatchTasksStatus.Width = tabMain.ClientRectangle.Width - lstScheduledTasks.Left - 15
            lstBatchTasksStatus.Height = albAvailableTasks.Height - txtFilter.Height - 20
            btnBatchTaskRefresh.Location = New System.Drawing.Point(lstBatchTasksStatus.Width - 78, 7)
        Catch excep As System.Exception



            'MRH 21/05/2004
            'PN490
            ' need to cope with the fact that this will be being called during a resize, in which case scaleheight e.t.c. will be zero.
            If Information.Err().Number = 380 Then


            Else
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetControlPositionsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetControlPositions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Sub
            End If
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: AddAvailableTaskGroup
    '
    ' Description: Adds a Task Group to the Active List Bar.
    '
    ' ***************************************************************** '
    'Private Function AddAvailableTaskGroup(ByVal v_lTaskGroupID As Integer, ByRef r_oGroup As Listbar.SSGroup) As Integer
    Private Function AddAvailableTaskGroup(ByVal v_lTaskGroupID As Integer, ByRef r_oGroup As SListBar.ListBarControl.ListBarGroup) As Integer
        Dim result As Integer = 0
        Dim sTaskGroupKey, sTaskGroupCaption As String




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Caption for this Task Group
        sTaskGroupCaption = cboTaskGroups.ItemCaption(v_lTaskGroupID).Trim()

        ' Derive the Key
        sTaskGroupKey = (ACTaskGroupPrefix & v_lTaskGroupID).Trim()

        ' Check to see if we already have this Task Group
        r_oGroup = Nothing
        'Try
        '    r_oGroup = albAvailableTasks.Groups.Item(sTaskGroupKey)

        'Catch
        'End Try
        Try
            If albAvailableTasks.ContainsGroupInhash("Fav", FavouritesCaption) Then
                albAvailableTasks.Groups.Add(FavouritesCaption)
            End If
            If albAvailableTasks.ContainsGroupInhash("sear", "Search") Then
                albAvailableTasks.Groups.Add("Search")
            End If
            If albAvailableTasks.ContainsGroupInhash(sTaskGroupKey, sTaskGroupCaption) Then
                albAvailableTasks.Groups.Add(sTaskGroupCaption)
            End If

            r_oGroup = albAvailableTasks.Groups.ItemCaption(sTaskGroupCaption)
        Catch
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try


        ' If Not then Add it
        'If r_oGroup Is Nothing Then
        '    r_oGroup = albAvailableTasks.Groups.Add(Key:=sTaskGroupKey, Caption:=sTaskGroupCaption)

        'End If

        '' If we still do not have it then error.
        'If r_oGroup Is Nothing Then
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If

        Return result

Err_AddAvailableTaskGroup:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAvailableTaskGroupFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAvailableTaskGroup", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: AddAvailableTask
    '
    ' Description: Adds the Available Task supplied to the
    '              Active List Bar.
    ' ***************************************************************** '
    Public Function AddAvailableTask(ByVal v_lTaskGroupID As Object, ByVal v_lTaskID As Object, ByVal v_sTaskKey As Object, ByVal v_sTaskCaption As Object, ByVal v_iIsSystemTask As Object, ByVal v_iTypeOfTask As Object, ByVal v_lQuickStartAvailable As Object, ByVal v_lDisplayIcon As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        'Dim oGroup As Listbar.SSGroup
        'Dim oListItem As Listbar.SSListItem
        Dim oGroup As SListBar.ListBarControl.ListBarGroup
        Dim oListItem As SListBar.ListBarControl.ListBarItem
        Dim vTag(2) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the Task Group.
            ' Note: If it already exists a ref to it will be returned
            lReturn = CType(AddAvailableTaskGroup(v_lTaskGroupID:=v_lTaskGroupID, r_oGroup:=oGroup), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If v_sTaskCaption.IndexOf("&"c) >= 0 Then
                v_sTaskCaption = v_sTaskCaption.Replace("&", "&&")
            End If
            ' Add the Task
            If oGroup Is Nothing Then
                oGroup = albAvailableTasks.Groups.Add(v_sTaskCaption.Trim())
                oListItem = oGroup.Items.Add(v_sTaskCaption.Trim())
            Else
                oListItem = oGroup.Items.Add(v_sTaskCaption.Trim())
            End If

            If oListItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            oListItem.Key = v_sTaskKey
            oListItem.IconIndex = v_lDisplayIcon

            'oListItem.IconLarge = v_lDisplayIcon

            'oListItem.IconSmall = v_lDisplayIcon

            ' Use the TagVariant to denote whether Quick Start is available or not.

            vTag(0) = v_iIsSystemTask

            vTag(1) = v_iTypeOfTask

            vTag(2) = v_lQuickStartAvailable
            oListItem.Tag = vTag
            oListItem = Nothing
            'oListItem.let_TagVariant(vTag)

            'oListItem = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add the Available Tasks", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAvailableTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddScheduledTaskToList
    '
    ' Description: Builds the Scheduled Task List View from the Array
    '              Supplied.
    ' ***************************************************************** '
    Public Function AddScheduledTaskToList(ByVal v_sKey As String, ByVal v_iIsUrgent As Integer, ByVal v_sTaskStatusDesc As String, ByVal v_sTaskTypedesc As String, ByVal v_dtTaskDueDate As Date, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_sUserGroup As String, ByVal v_sUser As String, ByVal v_sClientCode As String, Optional ByVal v_sPartyName As String = "") As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_iIsUrgent = gPMConstants.PMEReturnCode.PMTrue Then
                oListItem = lstScheduledTasks.Items.Add(v_sKey, "Yes", "Urgent")

                'ListViewHelper.SetSmallIconsProperty(lstScheduledTasks, "Urgent")
            Else
                oListItem = lstScheduledTasks.Items.Add(v_sKey, "No", "")
            End If


            oListItem.SubItems.Add(v_sTaskStatusDesc)
            oListItem.SubItems.Add(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=v_dtTaskDueDate))
            oListItem.SubItems.Add(v_sDescription)
            oListItem.SubItems.Add(v_sCustomer)
            oListItem.SubItems.Add(v_sTaskTypedesc)
            oListItem.SubItems.Add(v_sUserGroup)
            oListItem.SubItems.Add(v_sUser)
            oListItem.SubItems.Add(v_dtTaskDueDate.ToString("yyyyMMdd"))


            'ListViewHelper.GetListViewSubItem(oListItem, ACSTStatusCol).Text = v_sTaskStatusDesc.Trim()
            ''TODO
            'ListViewHelper.GetListViewSubItem(oListItem, ACSTDueDateCol).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=v_dtTaskDueDate)
            'ListViewHelper.GetListViewSubItem(oListItem, ACSTDescriptionCol).Text = v_sDescription.Trim()
            'ListViewHelper.GetListViewSubItem(oListItem, ACSTCustomerCol).Text = v_sCustomer.Trim()
            'ListViewHelper.GetListViewSubItem(oListItem, ACSTTaskTypeCol).Text = v_sTaskTypedesc.Trim()
            'ListViewHelper.GetListViewSubItem(oListItem, ACSTUserGroupCol).Text = v_sUserGroup.Trim()
            'ListViewHelper.GetListViewSubItem(oListItem, ACSTUserCol).Text = v_sUser.Trim()
            'ListViewHelper.GetListViewSubItem(oListItem, ACSTDueDateSortableCol).Text = v_dtTaskDueDate.ToString("yyyyMMdd")

            'mkw100204 PN9978
            'If v_sClientCode.Trim() <> "" Then
            '    ListViewHelper.GetListViewSubItem(oListItem, ACSTCustomerSortableCol).Text = v_sClientCode.Trim()
            'Else
            '    ListViewHelper.GetListViewSubItem(oListItem, ACSTCustomerSortableCol).Text = v_sCustomer.Trim()
            'End If
            'ListViewHelper.GetListViewSubItem(oListItem, ACSTAgentSortableCol).Text = v_sPartyName.Trim()

            If v_sClientCode.Trim <> "" Then
                oListItem.SubItems.Add(v_sClientCode)
            Else
                oListItem.SubItems.Add(v_sCustomer)
            End If
            oListItem.SubItems.Add(v_sPartyName)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddScheduledTaskToListFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddScheduledTaskToList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Builds the Batch Task List View from the Array Supplied.
    ''' </summary>
    ''' <param name="sProcess"></param>
    ''' <param name="sDescription"></param>
    ''' <param name="dtStartDateTime"></param>
    ''' <param name="dtEndDateTime"></param>
    ''' <param name="sFileName"></param>
    ''' <param name="nTotalRecordCount"></param>
    ''' <param name="nPassedRecordCount"></param>
    ''' <param name="nFailedRecordCount"></param>
    ''' <param name="sBatchStatus"></param>
    ''' <returns></returns>
    Public Function AddBatchTaskToList(ByVal sProcess As String, ByVal sDescription As String,
                                       ByVal dtStartDateTime As Date,
                                       ByVal dtEndDateTime As Date, ByVal sFileName As String,
                                       ByVal nTotalRecordCount As Integer, ByVal nPassedRecordCount As Integer,
                                       ByVal nFailedRecordCount As Integer, ByVal sBatchStatus As String) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oListItem As ListViewItem

        oListItem = lstBatchTasksStatus.Items.Add(sProcess, sProcess, "")
        oListItem.SubItems.Add(sDescription)
        oListItem.SubItems.Add(dtStartDateTime)
        If Convert.ToDateTime(dtEndDateTime) = Convert.ToDateTime("0001/01/01 00:00") Then
            oListItem.SubItems.Add("")
        Else
            oListItem.SubItems.Add(dtEndDateTime)
        End If
        'oListItem.SubItems.Add(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, vFieldValue:=dtStartDateTime))
        'oListItem.SubItems.Add(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateTimeLong, vFieldValue:=dtEndDateTime))
        Dim nlength As Integer = sFileName.Split("\").Length
        Dim sFileNameWithExtn As Array = sFileName.Split("\")
        oListItem.SubItems.Add(sFileNameWithExtn(nlength - 1))
        If nPassedRecordCount = 0 Then
            oListItem.SubItems.Add("")
        Else
            oListItem.SubItems.Add(nPassedRecordCount)
        End If

        If nFailedRecordCount = 0 Then
            oListItem.SubItems.Add("")
        Else
            oListItem.SubItems.Add(nFailedRecordCount)
        End If
        If nTotalRecordCount = 0 Then
            oListItem.SubItems.Add("")
        Else
            oListItem.SubItems.Add(nTotalRecordCount)
        End If
        oListItem.SubItems.Add(sBatchStatus)

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: UpdateScheduledTask
    '
    ' Description: Updates a Scheduled Task.
    '
    ' ***************************************************************** '
    'Developer Guide No. 33(Latest Guide)
    Public Sub UpdateScheduledTask(ByVal v_sKey As String, Optional ByVal v_vTaskStatusDesc As String = "", Optional ByVal v_vTaskDueDate As Object = Nothing, Optional ByVal v_vDescription As String = "", Optional ByVal v_vCustomer As String = "", Optional ByVal v_vTaskTypedesc As Object = Nothing, Optional ByVal v_vUserGroup As String = "", Optional ByVal v_vUser As String = "", Optional ByVal v_IsUrgernt As Object = 0)

        Dim oListItem As ListViewItem

        Try

            ' Get the Scheduled Task From the List
            oListItem = lstScheduledTasks.Items.Item(v_sKey)

            ' If we have got it, update it
            If oListItem Is Nothing Then
            Else
                ' Update the value for each column that we have been supplied.
                If v_IsUrgernt = 1 Then
                    ListViewHelper.GetListViewSubItem(oListItem, 0).Text = "Yes"
                    oListItem.ImageKey = "Urgent"
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, 0).Text = "No"
                End If

                If Not Information.IsNothing(v_vTaskStatusDesc) Then
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = v_vTaskStatusDesc.Trim()
                End If
                If Not Information.IsNothing(v_vTaskDueDate) Then
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=v_vTaskDueDate)
                End If

                If Not Information.IsNothing(v_vDescription) Then
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = v_vDescription.Trim()
                End If

                If Not Information.IsNothing(v_vCustomer) Then
                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = v_vCustomer.Trim()
                End If

                If Not Information.IsNothing(v_vTaskTypedesc) Then
                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = v_vTaskTypedesc.Trim()
                End If

                If Not Information.IsNothing(v_vUserGroup) Then
                    ListViewHelper.GetListViewSubItem(oListItem, 6).Text = v_vUserGroup.Trim()
                End If
                ' Note: This needs to be a variant so we can use the IsMissing function
                '       as a user of "" is valid.

                If Not Information.IsNothing(v_vUser) Then
                    ListViewHelper.GetListViewSubItem(oListItem, 7).Text = v_vUser.Trim()
                End If
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateScheduledTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateScheduledTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DeleteScheduledTask
    '
    ' Description: Deletes a Scheduled Task.
    '
    ' ***************************************************************** '
    Public Sub DeleteScheduledTask(ByVal v_sKey As String)

        Try

            ' Get the Scheduled Task From the List
            'Modified by Vijay Pal on 6/7/2010 12:42:55 PM declare a integer index which is taking the index value of v_sKey
            Dim index As Integer = lstScheduledTasks.Items.IndexOfKey(v_sKey)
            'Modified by Vijay Pal on 6/7/2010 1:13:20 PM RemoveAt(CInt(v_sKey) - 1) is replaced by  RemoveAt(index),index taking the index value of v_sKey
            'lstScheduledTasks.Items.RemoveAt(CInt(v_sKey) - 1)
            lstScheduledTasks.Items.RemoveAt(index)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteScheduledTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteScheduledTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SetForDisplay
    '
    ' Description: Sets the Form defaults for initial display.
    '
    '
    ' ***************************************************************** '
    Public Function SetForDisplay() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If this User a System Administrator or Normal User
            lReturn = CType(Parent_Renamed.GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                ' Add an All Groups Entry in the UserGroups List
                ' and Refresh the List of Users
                cboUserGroup.FirstItem = ACUserGroupAllGroups
                RefreshUserList()
            Else
                ' Add a Your Groups entry and tell the control
                ' to only list the groups that the user is a member of.
                cboUserGroup.FirstItem = ACUserGroupYourGroups

                'PN 24659
                cboUserGroup.PMUserID = Parent_Renamed.UserID

                ' RDC 06112002
                cboUserGroup.RefreshList()

                RefreshUserList()
            End If

            UpdateStatusBar(v_vPMAuthorityLevel:=lPMAuthorityLevel)

            FormDisplayed = True

            ' Initially Sort Scheduled Tasks by Due Date Ascending
            With lstScheduledTasks
                ' Initially Sort by Due Date, oldest first.
                ListViewHelper.SetSortedProperty(lstScheduledTasks, True)
                mnuViewSortBy_Click(mnuViewSortBy(ACSTDueDateCol + 1), New EventArgs())
                ListViewHelper.SetSortOrderProperty(lstScheduledTasks, SortOrder.Ascending)
                ' Set the sortable due date column to zer width
                ' so the user cannot see it.
                .Columns.Item(ACSTDueDateSortableCol).Width = CInt(0)
            End With

            ' Are we showing the Quick Start Bar
            If Not ViewQuickStart Then
                mnuViewQuickStartBar_Click()
            End If

            ' Are we showing the Available Tasks
            If Not ViewAvailableTasks Then
                mnuViewAvailableTasks_Click(mnuViewAvailableTasks, New EventArgs())
            End If

            ' Have we shown the Splash Screen
            If Not ViewSplash Then
                mnuViewSplashScreen.Checked = False
            End If

            'DAK231299
            ' Are we showing the Toolbar
            If Not ViewToolbar Then
                mnuViewToolbar_Click(mnuViewToolbar, New EventArgs())
            End If

            ' Are we showing the Status Bar
            If Not ViewStatusBar Then
                mnuViewStatusBar_Click(mnuViewStatusBar, New EventArgs())
            End If

            ' Are we showing the Grid Lines
            If Not ViewGridLines Then
                mnuViewGridLines_Click(mnuViewGridLines, New EventArgs())
            End If

            ' Are we showing the graphics
            'ListBarPicture = albAvailableTasks.PictureBackground
            lReturn = CType(SetGraphics(ViewGraphics), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Maximise the Form
            Me.WindowState = FormWindowState.Maximized
            ResetControlPositions()

            ' Add the Username to the Form Caption
            'DAK110700 - change to form caption
            'Me.Caption = Me.Caption & "(" & Parent.Username & ")"
            If FormCaption = "" Then
                Text = ACMainFormCaption & " (" & Parent_Renamed.Username & ")"
            Else
                Text = FormCaption & " (" & Parent_Renamed.Username & ")"
            End If

            ' If the User is a Systems Administrator
            If lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                ' Allow them to change the News Home Page
                'DAK210100
                mnuViewSystemOptions.Available = True
                mnuViewBar5.Available = True
            Else
                'DAK210100
                mnuViewSystemOptions.Available = False
                mnuViewBar5.Available = False
            End If

            'RFC150399 - Add Full Row Select & Grid Lines to List View
            'DAK241299 - Set grid lines to ViewGridLines property
            lReturn = CType(SetExtraListViewProperties(v_hWndList:=lstScheduledTasks.Handle.ToInt32(), v_vShowRowSelect:=True, v_vShowGridLines:=ViewGridLines), gPMConstants.PMEReturnCode)

            'DAK130999
            'If albAvailableTasks.CurrentGroupCaption = FavouritesCaption Then
            '    InFavouritesGroup = True
            'End If

            'DAK110700
            If PMSupportWebAddress = "" Then
                If tbToolBar.Items.Count > 2 Then
                    tbToolBar.Items.Item(2).Enabled = False
                End If
                mnuHelpPMSiriusSupport.Enabled = False
            End If


            ' RDC 17072002 set comboboxes to previous settings

            m_lReturn = SetComboSettings()

            cboUser.UserID = m_oParent.UserID

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetForDisplayFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetForDisplay", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RefreshUserList
    '
    ' Description: Refreshes the List of Users, based on the selected
    '              User Group.
    '
    ' ***************************************************************** '
    Private Sub RefreshUserList()
        Dim lPMUserGroupID As Integer
        Dim lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            ' Get the Currently Selected User GroupID
            lPMUserGroupID = cboUserGroup.UserGroupID

            ' If this User a System Administrator,
            ' Group Supervisor or Normal User
            'lReturn = CType(Parent_Renamed.GetUserAuthority(lPMAuthorityLevel, lPMUserGroupID), gPMConstants.PMEReturnCode)
            If (Not Parent_Renamed Is Nothing) Then


                lReturn = Parent_Renamed.GetUserAuthority(lPMAuthorityLevel, lPMUserGroupID)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                ' Set the User List based on the Authority Level
                ' and group selection.

                Select Case lPMAuthorityLevel
                    ' Administrators and Group Supervisors can see All Users
                    Case gPMConstants.PMEAuthorityLevel.pmeALSysAdmin, gPMConstants.PMEAuthorityLevel.pmeALSupervisor

                        cboUser.SingleUserID = 0
                        cboUser.Enabled = True
                        If lPMUserGroupID > 0 Then
                            cboUser.FirstItem = "All Group Users"
                            cboUser.PMUserGroupID = lPMUserGroupID
                        Else
                            cboUser.FirstItem = "All Users"
                            cboUser.PMUserGroupID = 0
                        End If

                        ' Normal Users can see only themselves.
                    Case gPMConstants.PMEAuthorityLevel.pmeALUser
                        cboUser.SingleUserID = Parent_Renamed.UserID
                        cboUser.Enabled = False
                        cboUser.FirstItem = ""
                        cboUser.UserID = Parent_Renamed.UserID

                End Select

                ' Refresh the List.
                cboUser.RefreshList()

                UpdateStatusBar(v_vPMAuthorityLevel:=lPMAuthorityLevel)
            End If
        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshUserListFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshUserList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SetTaskMenuOptions
    '
    ' Description: Sets the Task Menu options which are available.
    '
    ' ***************************************************************** '
    Public Sub SetTaskMenuOptions(ByVal v_bNewEnabled As Boolean, ByVal v_bEditEnabled As Boolean, ByVal v_bAssignEnabled As Boolean, ByVal v_bViewEnabled As Boolean, ByVal v_bStartEnabled As Boolean, ByVal v_bCompleteEnabled As Boolean, ByVal v_bIncompleteEnabled As Boolean, ByVal v_bDeleteEnabled As Boolean, ByVal v_bTaskLogEnabled As Boolean)

        Try

            mnuTaskNew.Enabled = v_bNewEnabled
            mnuTaskEdit.Enabled = v_bEditEnabled
            mnuTaskAssign.Enabled = v_bAssignEnabled
            mnuTaskView.Enabled = v_bViewEnabled
            mnuTaskStart.Enabled = v_bStartEnabled
            mnuTaskComplete.Enabled = v_bCompleteEnabled
            mnuTaskIncomplete.Enabled = v_bIncompleteEnabled
            mnuTaskDelete.Enabled = v_bDeleteEnabled
            mnuTaskLog.Enabled = v_bTaskLogEnabled

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetTaskMenuOptionsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetTaskMenuOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayTaskMenu
    '
    ' Description: Displays the Task Menu
    '
    '
    ' ***************************************************************** '
    Public Sub DisplayTaskMenu()

        Try

            Ctx_mnuTask.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayTaskMenuFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayTaskMenu", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: AddQuickStartButton
    '
    ' Description:
    ' ***************************************************************** '
    Public Function AddQuickStartButton(ByVal v_sKey As String, ByVal v_sCaption As String, ByVal v_sToolTipText As String, ByVal v_lDisplayIcon As Integer) As Integer

        Dim result As Integer = 0
        Dim oButton As ToolStripButton = Nothing
        Dim sCaption As String = ""

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

            ' more to do here

            '    ' Work out the Caption from the Display Icon
            '    Select Case v_lDisplayIcon
            '      Case ACTaskGroupIconIndexClient
            '        sCaption = ACTaskGroupIconDescClient
            '      Case ACTaskGroupIconIndexPolicy
            '        sCaption = ACTaskGroupIconDescPolicy
            '      Case ACTaskGroupIconIndexQuote
            '        sCaption = ACTaskGroupIconDescQuote
            '      Case ACTaskGroupIconIndexClaim
            '        sCaption = ACTaskGroupIconDescClaim
            '      Case ACTaskGroupIconIndexAccount
            '        sCaption = ACTaskGroupIconDescAccount
            '      Case ACTaskGroupIconIndexReport
            '        sCaption = ACTaskGroupIconDescReport
            '      Case ACTaskGroupIconIndexAgent
            '        sCaption = ACTaskGroupIconDescAgent
            '      Case ACTaskGroupIconIndexAdmin
            '        sCaption = ACTaskGroupIconDescAdmin
            '      Case ACTaskGroupIconIndexRenewals
            '        sCaption = ACTaskGroupIconDescRenewals
            '      Case ACTaskGroupIconIndexStatistics
            '        sCaption = ACTaskGroupIconDescStatistics
            '      Case Else
            '        sCaption = ACTaskGroupIconDescGeneral
            '    End Select

            If v_sCaption.Length > 13 Then
                sCaption = v_sCaption.Substring(0, 10) & "..."
            Else
                sCaption = v_sCaption
            End If

            'Set oButton = tbQuickStart.Buttons.Add(, v_sKey, sCaption, , v_lDisplayIcon)

            oButton.ToolTipText = v_sToolTipText

            oButton = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQuickStartButtonFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddQuickStartButton", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' DAK130999
    ' Name: AddToFavourites
    '
    ' Description:
    ' ***************************************************************** '
    Public Function AddToFavourites(ByVal v_sKey As String, ByVal v_sCaption As String, ByRef r_iIndex As Integer, ByVal v_lDisplayIcon As Integer) As Integer

        Dim result As Integer = 0
        Dim oGroup As New SListBar.ListBarControl.ListBarGroup
        Dim oRealTask, oFavouritesTask As SListBar.ListBarControl.ListBarItem
        Dim oListItem As SListBar.ListBarControl.ListBarItem

        '        Dim oGroup As Listbar.SSGroup
        '       Dim oRealTask, oFavouritesTask As Listbar.SSListItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            oGroup = albAvailableTasks.Groups.ItemCaption(FavouritesCaption)

            oListItem = oGroup.Items.Add(v_sCaption.Trim())
            oListItem.Key = v_sKey
            oListItem.IconIndex = v_lDisplayIcon
            'Developer Guide No 103 Modified By Mohit Uniyal on 14/05/2015 | setting tag property of favorite menu during Page Load
            If albAvailableTasks.SelectedGroup.Caption = FavouritesCaption Then
                For lLoop As Integer = 2 To albAvailableTasks.Groups.Count - 1
                    If Not (albAvailableTasks.Groups.Item(lLoop).Items(v_sKey) Is Nothing) Then
                        oListItem.Tag = albAvailableTasks.Groups.Item(lLoop).Items(v_sKey).Tag
                        Exit For
                    End If
                Next
            Else
                'Developer Guide No 103 Modified By Mohit Uniyal on 28/01/2015 | setting tag property and index of favorite menu when clicked on add favorite from selected group
                oListItem.Tag = albAvailableTasks.SelectedGroup.Items(v_sKey).Tag
            End If
            r_iIndex = oGroup.Items.IndexOf(oListItem) + 1
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddToFavouritesFailed", vApp:=MainModule.ACApp, vClass:=ACClass, vMethod:="AddToFavourites", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' DAK130999
    ' Name: RemoveFromFavourites
    '
    ' Description:
    ' ***************************************************************** '
    Public Function RemoveFromFavourites(ByVal v_sKey As String, ByVal v_sCaption As String, ByRef r_iIndex As Integer, ByVal v_lDisplayIcon As Integer) As Integer

        Dim result As Integer = 0
        Dim oGroup As New SListBar.ListBarControl.ListBarGroup
        Dim oListItem As SListBar.ListBarControl.ListBarItem

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Favourites group
            oGroup = albAvailableTasks.Groups.ItemCaption(FavouritesCaption)

            ' Find the ListBarItem using the Key
            For Each oListItem In oGroup.Items
                If oListItem.Key = v_sKey Then
                    ' Remove the item from the Favourites group
            oGroup.Items.Remove(oListItem)

                    ' Update the index (after removal)
                    r_iIndex = oGroup.Items.IndexOf(oListItem) + 1
                    Exit For
                End If
            Next

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveFromFavouritesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveFromFavourites", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: RemoveQuickStartButton
    '
    ' Description:
    ' ***************************************************************** '
    Public Function RemoveQuickStartButton(ByVal v_sKey As String) As Integer

        Dim result As Integer = 0
        Try


            ' If there aren't any Buttons on the Quick Start then Exit.
            '    If (tbQuickStart.Buttons.Count < 1) Then
            '        Exit Function
            '    End If
            '
            '    ' Remove the Button
            '    tbQuickStart.Buttons.Remove (v_sKey)

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveQuickStartButtonFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveQuickStartButton", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateStatusBar
    '
    ' Description: Updates the Status Bar with the Values Supplied.
    ' ***************************************************************** '
    Public Sub UpdateStatusBar(Optional ByVal v_vPMAuthorityLevel As gPMConstants.PMEAuthorityLevel = 0, Optional ByVal v_vActivity As String = "", Optional ByVal v_vErrorMsg As String = "")

        Try

            If sbStatusBar.Items.Count = 0 Then
                Exit Sub
            End If

            If Not Information.IsNothing(v_vPMAuthorityLevel) Then

                Select Case v_vPMAuthorityLevel
                    Case gPMConstants.PMEAuthorityLevel.pmeALSysAdmin
                        sbStatusBar.Items.Item(0).Text = ACStatusAuthSysAdmin
                    Case gPMConstants.PMEAuthorityLevel.pmeALSupervisor
                        sbStatusBar.Items.Item(0).Text = ACStatusAuthSupervisor
                    Case Else
                        'sbStatusBar.Items.Item(0).Text = ACStatusAuthUser
                        sbStatusBar.Items.Item(0).Text = ACStatusAuthSysAdmin

                End Select

            End If


            sbStatusBar.Items.Item(1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, DateTime.Now)

            If Not lstScheduledTasks.IsDisposed And tabMain.SelectedIndex = 0 Then
                sbStatusBar.Items.Item(2).Text = CStr(lstScheduledTasks.Items.Count) & ACStatusItems
            ElseIf Not lstBatchTasksStatus.IsDisposed And tabMain.SelectedIndex = 1 Then
                sbStatusBar.Items.Item(2).Text = CStr(lstBatchTasksStatus.Items.Count) & ACStatusItems
            End If

            If Not Information.IsNothing(v_vActivity) Then
                sbStatusBar.Items.Item(3).Text = v_vActivity
                If CBool(CStr(v_vActivity = "").Trim()) Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Else
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                End If
            Else
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If


            If Not Information.IsNothing(v_vErrorMsg) Then
                sbStatusBar.Items.Item(3).Text = v_vErrorMsg
            End If

            ' Refresh the Status Bar
            'Developer Guide No. 99 (Guide)
            If sbStatusBar.InvokeRequired Then
                sbStatusBar.Invoke(New DlgRefreshStatusBar(AddressOf sbStatusBar.Refresh))
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateStatusBarFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStatusBar", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    'Developer Guide No. 99 (Guide)
    'Deepak Added the code start'
    Private Delegate Sub DlgRefreshStatusBar()
    'End'

    Private Sub tmrRefreshTimer_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrRefreshTimer.Tick
        Static iCounter As Integer

        If iCounter = 0 Then
            iCounter = 1
        End If

        If iCounter < RefreshRate Then
            iCounter += 1
            Exit Sub
        End If

        'DAK080999
        ' Refresh Scheduled Tasks on a regular basis
        ' RDC 09012003 change parameter from False
        RaiseEvent RefreshScheduledTasks(True)

        'DAK220999
        ' RDC 09012003 remove this - not required for auto refresh.
        ' Available tasks can still be refreshed by hitting F5
        'RaiseEvent RefreshAvailableTasks

        iCounter = 1

    End Sub

    Private Sub tmrSystemTasks_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrSystemTasks.Tick
        Static iCounter As Integer


        'DAK110100
        If iCounter = 0 Then
            iCounter = 1
        End If

        If iCounter < ACSystemTasksTimerInterval Then
            iCounter += 1
            Exit Sub
        End If

        ' Check For Due System Tasks
        RaiseEvent CheckForDueSystemTasks()

        ' Update the Date/Time on the Status Bar
        UpdateStatusBar()

        iCounter = 1

    End Sub


    'Private Sub treAvailableTasks_DblClick()
    '    ' Double Click Means do this Task Now.
    '    mnuAvailTaskDoNow_Click
    'End Sub


    'Private Sub treAvailableTasks_MouseUp(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
    'Dim oNode As Node
    ''
    '    ' Has the User Right Clicked on the Available Tasks
    '    If (Button = vbRightButton) Then
    ''
    '        ' Get the selected Available Task
    '        Set oNode = treAvailableTasks.HitTest(X, Y)
    '        If (oNode Is Nothing = True) Then
    '            Exit Sub
    '        End If
    ''
    '        ' If Node is a Task Group then exit.
    '        If (oNode.Parent Is Nothing = True) Then
    '            Exit Sub
    '        End If
    ''
    '        ' The Tag tell us whether the Quick Start Functionality is available
    '        If oNode.Tag = PMTrue Then
    '            mnuAvailTaskAddToQSBar.Enabled = True
    '            mnuAvailTaskRemoveFromQSBar.Enabled = True
    '            mnuAvailTaskDoNow.Enabled = True
    '        Else
    '            mnuAvailTaskAddToQSBar.Enabled = False
    '            mnuAvailTaskRemoveFromQSBar.Enabled = False
    '            mnuAvailTaskDoNow.Enabled = False
    '        End If
    '        PopupMenu mnuAvailTask, vbPopupMenuLeftButton
    '    End If
    '
    'End Sub

    ' ***************************************************************** '
    '
    ' Name: SetGraphics
    '
    ' Description:
    '
    ' History: 23/12/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function SetGraphics(ByRef bViewGraphics As Boolean) As Integer
        'DAK130100
        Dim result As Integer = 0
        Dim sViewGraphics As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bViewGraphics Then
                ViewGraphics = True
                'albAvailableTasks.PictureBackground = ListBarPicture
                ' RDC 16052002 picture removed
                ' picToolbar.Visible = True
                mnuViewSplashScreen.Enabled = True
                'DAK130100
                sViewGraphics = CStr(gPMConstants.PMEReturnCode.PMTrue)
            Else
                ViewGraphics = False
                'albAvailableTasks.PictureBackground = Nothing
                ' RDC 16052002 picture removed
                ' picToolbar.Visible = False
                If ViewSplash Then
                    mnuViewSplashScreen_Click(mnuViewSplashScreen, New EventArgs())
                End If
                mnuViewSplashScreen.Enabled = False
                'DAK130100
                sViewGraphics = CStr(gPMConstants.PMEReturnCode.PMFalse)
            End If

            'DAK130100
            ' Set the Registry Setting
            lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACRegKeyViewGraphics, v_sSettingValue:=sViewGraphics), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetGraphics Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetGraphics", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' RDC 23112000 set column widths in listview control
    Private Sub SetColumnWidths()



        Try

            Dim sCL As String = m_sColumnWidths

            Dim iCount As Integer = 1
            Dim iPos As Integer = (sCL.IndexOf(";"c) + 1)

            Do Until sCL = "" Or lstScheduledTasks.Columns.Count < iCount

                'lstScheduledTasks.Columns.Item(iCount - 1).Width = VB6.TwipsToPixelsX(CInt(sCL.Substring(0, iPos - 1)))
                lstScheduledTasks.Columns.Item(iCount - 1).Width = sCL.Substring(0, iPos - 1)

                iCount += 1

                sCL = Mid(sCL, iPos + 1)
                iPos = (sCL.IndexOf(";"c) + 1)
            Loop

        Catch exc As System.Exception
            'Modified,comment the line
        End Try

    End Sub

    ' RDC 17072002 combobox settings
    Private Function SetComboSettings() As Integer

        Dim result As Integer = 0
        Dim vCombo As Object
        Dim nPMUserGroupID As Integer
        Dim nPMAuthorityLevel As Integer
        Dim nReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sComboSettings = "" Then
                Return result
            End If

            ' Get the Currently Selected User GroupID
            nPMUserGroupID = cboUserGroup.UserGroupID

            ' If this User a System Administrator,
            ' Group Supervisor or Normal User
            nReturn = Parent_Renamed.GetUserAuthority( _
                r_lPMAuthorityLevel:=nPMAuthorityLevel, _
                v_lUserGroupID:=nPMUserGroupID)
            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Throw New ApplicationException
            End If

            vCombo = m_sComboSettings.Split(";"c)

            FormDisplayed = False
            cboTaskStatus.SelectedIndex = CInt(vCombo(0))
            cboUserGroup.ListIndex = CInt(vCombo(1))
            If nPMAuthorityLevel = SharedFiles.PMEAuthorityLevel.pmeALSysAdmin Or nPMAuthorityLevel = SharedFiles.PMEAuthorityLevel.pmeALSupervisor Then
                cboUser.ListIndex = vCombo(2)
            End If
            cboDateRange.SelectedIndex = CInt(vCombo(3))
            cboShowSystem.SelectedIndex = CInt(vCombo(4))
            FormDisplayed = True

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    ' RDC 23112000 get column widths in listview control
    Private Function SaveColumnWidths() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_sColumnWidths = ""

            For Each col As ColumnHeader In lstScheduledTasks.Columns
                m_sColumnWidths = m_sColumnWidths & CStr(col.Width) & ";"
            Next


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' RDC 17072002
    Private Function SaveComboSettings() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_sComboSettings = ""

            '    m_sComboSettings = m_sComboSettings & cboTaskStatus.Text & ";"
            '    m_sComboSettings = m_sComboSettings & cboUserGroup.ListIndex & ";"
            '    m_sComboSettings = m_sComboSettings & cboUser.ItemUsername & ";"
            '    m_sComboSettings = m_sComboSettings & cboDateRange.Text & ";"
            '    m_sComboSettings = m_sComboSettings & cboShowSystem.Text & ";"

            m_sComboSettings = m_sComboSettings & CStr(cboTaskStatus.SelectedIndex) & ";"
            m_sComboSettings = m_sComboSettings & CStr(cboUserGroup.ListIndex) & ";"
            m_sComboSettings = m_sComboSettings & CStr(cboUser.ListIndex) & ";"
            m_sComboSettings = m_sComboSettings & CStr(cboDateRange.SelectedIndex) & ";"
            m_sComboSettings = m_sComboSettings & CStr(cboShowSystem.SelectedIndex) & ";"


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' RDC 16052002 add toolbar buttons that provide links to websites
    Public Sub AddWebsiteButtons()

        Dim oButton As ToolStripButton
        Dim lIconId As Integer

        Try

            If Parent_Renamed Is Nothing Then
                Exit Sub
            End If

            m_lReturn = CType(Parent_Renamed.GetToolBarWebsiteData(m_vButtonData), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' none defined
                mnuLinks.Available = False
                Exit Sub
            End If

            ' add the buttons
            For iLoop As Integer = 0 To m_vButtonData.GetUpperBound(1)

                'AR20050225 - PN14142 Get the Icon Id
                lIconId = gPMFunctions.ToSafeLong(CStr(m_vButtonData(PMWRK_WEBSITE_ICON, iLoop)), -1)

                If lIconId <> -1 Then
                    If (UCase(RTrim(LTrim(m_vButtonData(PMWRK_WEBSITE_CODE, iLoop))).Substring(0, 3)) = "WEB") Then
                        ' add button to toolbar
                        oButton = New ToolStripButton()
                        tbToolBar.Items.Add(oButton)
                        oButton.Name = m_vButtonData(PMWRK_WEBSITE_CODE, iLoop)
                        'oButton.Name = "_tbToolBar_Button6"
                        oButton.ImageIndex = lIconId - 1
                        oButton.ToolTipText = "Website: " & CStr(m_vButtonData(PMWRK_WEBSITE_TIP, iLoop))
                        AddHandler oButton.Click, AddressOf tbToolBar_ButtonClick
                    End If

                End If

                ' add submenu to 'Links' menu
                If iLoop > 0 Then
                    ContainerHelper.LoadControl(Me, "mnuLinksWebsite", iLoop)
                End If

                ' set menu item properties
                With mnuLinksWebsite(iLoop)
                    .Text = CStr(m_vButtonData(PMWRK_WEBSITE_DESC, iLoop))
                    .Tag = CStr(iLoop)
                    .Available = True
                    .Enabled = True
                End With

            Next

        Catch
        End Try



    End Sub


    '**************************************************************
    'PURPOSE: Automatically size the user control drop down width
    '         based on the width of the longest item in the combo box

    'JT       31/04/2005
    '**************************************************************
    Public Function AutoSizeDropDownUserControlWidth() As Integer

        Dim result As Integer = 0
        Try

            With cboUserGroup


                'TODO
                'lsavemode = .FindForm().ScaleMode
                'Reset to vbPixels for TextWidth to work properly



                'TODO
                '.FindForm().ScaleMode = vbPixels
                'Find longest member
                'developer guide no. 74
                For i As Integer = 0 To .ListCount - 1

                    lcur_wid = CInt(.FindForm().CreateGraphics().MeasureString(.List(i), .FindForm().Font).Width)
                    If lmax_wid < lcur_wid Then
                        lmax_wid = lcur_wid
                    End If
                Next i

                'restore form's ScaleMode


                'TODO
                '.FindForm().ScaleMode = lsavemode
            End With
            ' Set the width for the dropdown list, adding a margin.
            m_lReturn = cboUserGroup.AutoSizeDropDownUserControlWidth(lmax_wid)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoSizeDropDownUserControlWidth Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoSizeDropDownUserControlWidth", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    'Private Sub albAvailableTasks_OLECompleteDrag(ByVal eventSender As System.Object, ByVal eventArgs As AxListbar.DSSListbarEvents_OLECompleteDragEvent) Handles albAvailableTasks.OLECompleteDrag

    '    Dim oListItem As Listbar.SSListItem
    '    Dim iIndex As Short
    '    Dim fYPos As Single
    '    Dim fXMid As Single
    '    Dim iHit As Short


    '    'RFC160399 - Available Task/Quick Start Bar now uses Sheriden Active List Bar
    '    'DAK100999 - Prevent dropping onto other tasks
    '    If (m_lEffect <> System.Windows.Forms.DragDropEffects.Move And m_lEffect <> System.Windows.Forms.DragDropEffects.Copy) Then
    '        Exit Sub
    '    End If

    '    InFavouritesGroup = True

    '    If CurrentAvailableTaskKey = "" Then
    '        Exit Sub
    '    End If

    '    With albAvailableTasks

    '        'fXMid = VB6.PixelsToTwipsX(.Width) / 2 + VB6.PixelsToTwipsX(.Left)
    '        fXMid = .Width / 2 + .Left

    '        fYPos = m_fCurrentYPos
    '        Do
    '            fYPos = fYPos - 1

    '            If fYPos < 0 Then
    '                Exit Do
    '            End If

    '            iHit = .WhereIs(fXMid, fYPos)

    '        Loop While iHit = Listbar.Constants_WhereIs.ssHitGroup

    '        If fYPos < 0 Or iHit = Listbar.Constants_WhereIs.ssHitGroupCaption Then
    '            iIndex = 1
    '        ElseIf iHit <> Listbar.Constants_WhereIs.ssHitListItem Then
    '            Exit Sub
    '        Else
    '            oListItem = .ListItemFromPosition(fXMid, fYPos)

    '            If oListItem.Index = CurrentIndex Then
    '                Exit Sub
    '            End If

    '            If CurrentIndex = 0 Or oListItem.Index < CurrentIndex Then
    '                iIndex = oListItem.Index + 1
    '            Else
    '                iIndex = oListItem.Index
    '            End If

    '        End If

    '    End With

    '    If m_lEffect = System.Windows.Forms.DragDropEffects.Move Then
    '        RaiseEvent MoveFavouriteTask(CurrentAvailableTaskKey, iIndex)
    '    Else
    '        RaiseEvent AddTaskToFavourites(CurrentAvailableTaskKey, iIndex)
    '    End If


    'End Sub




    'Private Sub albAvailableTasks_OLEStartDrag(ByVal eventSender As System.Object, ByVal eventArgs As AxListbar.DSSListbarEvents_OLEStartDragEvent) Handles albAvailableTasks.OLEStartDrag
    '    'RFC160399 - Available Task/Quick Start Bar now uses Sheriden Active List Bar
    '    'DAK090999 - Allow Copy and No drop effects

    '    eventArgs.AllowedEffects.Value = System.Windows.Forms.DragDropEffects.None Or System.Windows.Forms.DragDropEffects.Scroll

    '    If InFavouritesGroup = True Then
    '        eventArgs.AllowedEffects.Value = eventArgs.AllowedEffects.Value Or System.Windows.Forms.DragDropEffects.Move
    '    Else
    '        eventArgs.AllowedEffects.Value = eventArgs.AllowedEffects.Value Or System.Windows.Forms.DragDropEffects.Copy
    '    End If

    'End Sub

    'Private Sub albAvailableTasks_OLEDragOver(ByVal eventSender As System.Object, ByVal eventArgs As AxListbar.DSSListbarEvents_OLEDragOverEvent) Handles albAvailableTasks.OLEDragOver
    '    'DAK090999
    '    Dim fXMid As Single


    '    m_fCurrentYPos = eventArgs.y

    '    With albAvailableTasks

    '        'fXMid = VB6.PixelsToTwipsX(.Width) / 2 + VB6.PixelsToTwipsX(.Left)
    '        fXMid = .Width / 2 + .Left

    '        ' Moving tasks within the Favourites Group
    '        If InFavouritesGroup = True Then
    '            Select Case .WhereIs(fXMid, eventArgs.y)
    '                Case Listbar.Constants_WhereIs.ssHitGroupCaption
    '                    eventArgs.Effect.Value = System.Windows.Forms.DragDropEffects.Scroll
    '                Case Listbar.Constants_WhereIs.ssHitGroup
    '                    eventArgs.Effect.Value = System.Windows.Forms.DragDropEffects.Move
    '                Case Else
    '                    eventArgs.Effect.Value = System.Windows.Forms.DragDropEffects.None
    '            End Select
    '            m_lEffect = eventArgs.Effect.Value
    '            Exit Sub
    '        End If


    '        'DAK091299
    '        If .GroupFromPosition(eventArgs.x, eventArgs.y) Is Nothing Then
    '            'eventArgs.effect.Value = System.Windows.Forms.DragDropEffects.None
    '            eventArgs.effect.Value = System.Windows.Forms.DragDropEffects.Copy
    '            m_lEffect = eventArgs.effect.Value
    '            Exit Sub
    '        End If

    '        ' Copying a task to the Favourites Group
    '        If (.CurrentGroupCaption <> FavouritesCaption) Then
    '            If (.WhereIs(eventArgs.x, eventArgs.y) = Listbar.Constants_WhereIs.ssHitGroupCaption And .GroupFromPosition(eventArgs.x, eventArgs.y).Caption = FavouritesCaption) Then
    '                .CurrentGroup = .GroupFromPosition(eventArgs.x, eventArgs.y)
    '                eventArgs.Effect.Value = System.Windows.Forms.DragDropEffects.Copy
    '            Else
    '                eventArgs.Effect.Value = System.Windows.Forms.DragDropEffects.None
    '            End If
    '            m_lEffect = eventArgs.Effect.Value
    '            Exit Sub
    '        End If

    '        ' Pasting a task in the Favourites Group
    '        Select Case .WhereIs(fXMid, eventArgs.y)
    '            Case Listbar.Constants_WhereIs.ssHitGroupCaption
    '                eventArgs.Effect.Value = System.Windows.Forms.DragDropEffects.Scroll
    '            Case Listbar.Constants_WhereIs.ssHitGroup
    '                eventArgs.Effect.Value = System.Windows.Forms.DragDropEffects.Copy
    '            Case Else
    '                eventArgs.Effect.Value = System.Windows.Forms.DragDropEffects.None
    '        End Select

    '        m_lEffect = eventArgs.Effect.Value

    '    End With

    'End Sub


    Private Sub txtFilter_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilter.GotFocus
        If txtFilter.Text = "Search..." Then
            txtFilter.Text = ""
        End If
        txtFilter.SelectAll()
    End Sub
    'start added by shipali
    Private Sub txtFilter_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFilter.GotFocus
        If MouseButtons = Windows.Forms.MouseButtons.None Then
            If txtFilter.Text = "Search..." Then
                txtFilter.Text = ""
            End If
            'txtFilter.SelectionStart = 0
            'txtFilter.SelectionLength = Strings.Len(txtFilter.Text)
            txtFilter.SelectAll()
            m_bIsTextFilterSelected = True
        End If
    End Sub
    Private Sub txtFilter_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtFilter.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        'Ctrl + A
        If KeyCode = 65 And Shift = ShiftConstants.CtrlMask Then '
            txtFilter.SelectionStart = 0
            txtFilter.SelectionLength = Strings.Len(txtFilter.Text)
        End If
        'Ctrl + C

        If KeyCode = 67 And Shift = ShiftConstants.CtrlMask Then
            My.Computer.Clipboard.Clear()
            'UPGRADE_WARNING: (2081) Clipboard.SetText has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2081.aspx
            My.Computer.Clipboard.SetText(txtFilter.SelectedText)
        End If
        'Ctrl + V

        If KeyCode = 86 And Shift = ShiftConstants.CtrlMask Then
            txtFilter.SelectedText = My.Computer.Clipboard.GetText()
        End If
    End Sub

    Private Sub txtFilter_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFilter.Leave
        If txtFilter.Text = "" Then
            txtFilter.Text = "Search..."
            'Developer guide no 103 Parveen Sachdeva on 10/10/2014
            'albAvailableTasks.Groups.Item(2).ListItems.Clear()
            albAvailableTasks.Groups.Item(1).Items.Clear()
        End If
        m_bIsTextFilterSelected = False
    End Sub
    'end added by shipali

    Private Sub txtFilter_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFilter.KeyUp
        Dim KeyCode As Integer = e.KeyCode
        Dim Shift As Integer = e.KeyData \ &H10000
        If e.KeyCode = Keys.Return Then
            DoSearch()
        End If
    End Sub

    Private Sub txtFilter_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilter.LostFocus
        If txtFilter.Text = "" Then
            txtFilter.Text = "Search..."
            albAvailableTasks.Groups.Item(2).Items.Clear()
        End If
    End Sub

    Private Sub DoSearch()
        Try

            Dim bExists As Boolean
            If txtFilter.Text <> "" Then
                ' albAvailableTasks.CurrentGroup = albAvailableTasks.Groups.Item(2)
                albAvailableTasks.SelectedGroup = albAvailableTasks.Groups.Item(1)
                albAvailableTasks.CurrentSelected = albAvailableTasks.Groups.Item(1)
                albAvailableTasks.SelectGroup(albAvailableTasks.Groups.Item(1))
                albAvailableTasks.CurrentSelected = Nothing
                InFavouritesGroup = False

                'Remove any previous results
                albAvailableTasks.Groups.Item(1).Items.Clear()


                For lLoop As Integer = 2 To albAvailableTasks.Groups.Count - 1
                    For j As Integer = 0 To albAvailableTasks.Groups.Item(lLoop).Items.Count - 1
                        If albAvailableTasks.Groups.Item(lLoop).Items.Item(j).Caption.ToUpper().IndexOf(txtFilter.Text.ToUpper()) + 1 Then
                            'Check if task already in list
                            bExists = False
                            For k As Integer = 0 To albAvailableTasks.Groups.Item(1).Items.Count - 1
                                If albAvailableTasks.Groups.Item(1).Items.Item(k).Caption = albAvailableTasks.Groups.Item(lLoop).Items.Item(j).Caption Then
                                    bExists = True
                                End If
                            Next

                            If Not bExists Then
                                Dim oListItem As SListBar.ListBarControl.ListBarItem

                                oListItem = albAvailableTasks.Groups.Item(1).Items.Add(albAvailableTasks.Groups.Item(lLoop).Items.Item(j).Caption)
                                oListItem.Key = albAvailableTasks.Groups.Item(lLoop).Items.Item(j).Key
                                oListItem.Tag = albAvailableTasks.Groups.Item(lLoop).Items.Item(j).Tag
                                oListItem.IconIndex = albAvailableTasks.Groups.Item(lLoop).Items.Item(j).IconIndex
                                oListItem = Nothing
                            End If
                        End If
                    Next
                Next


            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Search Failed", vApp:=MainModule.ACApp, vClass:=ACClass, vMethod:="DoSearch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try
    End Sub

    Private Sub cboParty_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboParty.SelectedIndexChanged
        RaiseEvent RefreshScheduledTasks(False)
    End Sub

    Private Sub btnBatchTaskRefresh_Click(sender As Object, e As EventArgs) Handles btnBatchTaskRefresh.Click
        RaiseEvent RefreshBatchTasks(True)
    End Sub

    Private Sub cboBatchTaskDateRange_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBatchTaskDateRange.SelectedIndexChanged
        RaiseEvent RefreshBatchTasks(True)
    End Sub
    Private Sub cboBatchTaskStatus1_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBatchTaskStatus.Click
        RaiseEvent RefreshBatchTasks(True)
    End Sub

    Private Sub lstBatchTasksStatus_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lstBatchTasksStatus.ColumnClick
        Dim ColumnHeader As ColumnHeader = lstBatchTasksStatus.Columns(eventArgs.Column)
        Try
            Dim nDirection As SortOrder
            Dim nColumnHeaderIndex As Integer = ColumnHeader.Index
            Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue

            With lstBatchTasksStatus
                If ListViewHelper.GetSortOrderProperty(lstBatchTasksStatus) = 1 Then
                    nDirection = SortOrder.Descending
                Else
                    nDirection = SortOrder.Ascending
                End If
                Select Case nColumnHeaderIndex
                    Case kBatchProcessListPassedRecordsIndex, kBatchProcessListFailedRecordsIndex, kBatchProcessListTotalRecordsIndex, kBatchProcessListBatchIdIndex
                        ListViewHelper.SetSortedProperty(lstBatchTasksStatus, False)
                        ListViewHelper.SetSortOrderProperty(lstBatchTasksStatus, nDirection)
                        ListViewFunc.ListViewSortByValue(lstBatchTasksStatus, nColumnHeaderIndex, ListViewHelper.GetSortOrderProperty(lstBatchTasksStatus))
                    Case kBatchProcessListStartDateTimeIndex, kBatchProcessListEndDateTimeIndex
                        nReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=lstBatchTasksStatus, v_iSourceColumn:=nColumnHeaderIndex, v_iDirection:=nDirection), gPMConstants.PMEReturnCode)
                    Case Else
                        ListViewHelper.SetSortKeyProperty(lstBatchTasksStatus, ColumnHeader.Index)
                        ListViewHelper.SetSortOrderProperty(lstBatchTasksStatus, nDirection)
                        ListViewHelper.SetSortedProperty(lstBatchTasksStatus, True)
                End Select
            End With

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="OnColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub


End Class
