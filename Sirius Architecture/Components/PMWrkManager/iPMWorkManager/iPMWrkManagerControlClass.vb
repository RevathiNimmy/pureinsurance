Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No.129
Imports SharedFiles
Imports System.Data
Imports SListBar.ListBarControl
Friend NotInheritable Class ControlClass
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: ControlClass
    '
    ' Date: 26/10/1998
    '
    ' Description: Main Controlling Class.
    '              Handles the display of the Form etc.
    '
    ' Edit History:
    '
    ' DAK130999 - New functions to replace Quick Start buttons
    '             with the Favourites Group in Sheridan
    '             Active list bar.
    ' DAK150999 - Add Task Icons in place of group icons.
    ' DAK220999 - Add RefreshAvailableTasks event for frmMain
    ' DAK300999 - Save Favourites before refreshing available tasks
    ' DAK071299 - Changes to messages displayed when licence limit is
    '             exceeded
    ' DAK071299 - Allow edit in Task Log
    ' DAK141299 - Tasks started from Available tasks should not be visisble
    ' DAK030100 - Refresh tasks for any user - not just admin.
    ' DAK080200 - Prevent refresh when starting Available Task.
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ControlClass"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    Private m_oObjectManager As bObjectManager.ObjectManager
    Private m_sUsername As String = ""
    Private m_iUserID As Integer
    ' RDC 16082002
    Private m_lUserGroupID As Integer

    ' Main Form
    'Private WithEvents m_fMainForm As frmMain
    Private WithEvents m_fMainForm As frmMain

    ' Available Task Collection (i.e. Tasks which the User can do.)
    Private m_oAvailableTasks As PMWorkManager.AvailableTasks

    ' Scheduled Task Collection
    Private m_oSchedTasks As PMWorkManager.ScheduledTasks

    ' Currently Running Tasks (Both Navigators & Single Components)
    Private WithEvents m_oInProgTasks As PMWorkManager.InProgTasks



    Private m_oBusiness As bPMWrkManager.FormClass
    'Private m_oBusiness As bPMWrkManager.FormClass

    'DAK120700

    Private m_oServerRegistry As bPMServerRegistry.PMServerRegistry

    ' The currently selected Scheduled Task
    Private m_sScheduledTaskKey As String = ""

    ' Whether the QS Bar has been amended or not.
    Private m_bQSBarDirty As Boolean

    'DAK130999
    ' Whether the Favourites Group has been amended or not.
    Private m_bFavouritesChanged As Boolean
    Private bLoadFavorite As Boolean = False
    ' When we are due to Check for Due System Tasks again
    Private m_dtNextCheckSystemTasks As Date

    ' RDC 23032001
    Dim m_sSplashAppTitle As String = ""

    'MKW110803 PN6022 1.8.5 to 1.8.6 Catchup START
    'SET 08/04/2003 ISS3299
    Private m_oUserGroup As Object
    ' GIRIJA
    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.3.2)
    'Added by Gaurav ( To make it accessible in AddTaskToFavourites function)
    Private m_sFavouritesCaption As Object
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.3.2)

    Private m_lSelectedUserGroupId As Integer
    Public Const kACHasViewBatchProcessStatusArrPos As Integer = 79

    Private Function GetGroups(ByRef r_vGroups(,) As Object, ByVal dtEffectiveDate As Date, Optional ByVal lUserId As Integer = -1, Optional ByVal lGroupId As Integer = -1) As Integer
        Dim result As Integer = 0

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vAllGroups(,), vUserGroup, vTempGroup(,) As Object
        Dim iPos As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' get all the groups for the 'logged in' user


        vAllGroups = m_fMainForm.cboUserGroup.UserGroups

        If lUserId > -1 Then
            ' get all the groups for the selected user
            'Developer Guide No. 41 (Guide)
            If m_oUserGroup Is Nothing Then
                lReturn = m_oObjectManager.GetInstance(m_oUserGroup, "bPMUserGroup.LookUp", PMGetViaClientManager)
            End If

            lReturn = m_oUserGroup.GetAllEffectiveGroupsByUser(v_lPMUserID:=lUserId, v_dtEffectiveDate:=dtEffectiveDate, r_vUserGroupsArray:=vUserGroup)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get effective groups for user " & m_fMainForm.cboUser.ItemUsername, vApp:=ACApp, vClass:=ACClass, vMethod:="GetGroups", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return lReturn
            End If

            If Not Information.IsArray(vUserGroup) Then
                ' this user is not a member of a group
                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=m_fMainForm.cboUser.ItemUsername & " is not a member of any user group.", vApp:=ACApp, vClass:=ACClass)

                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

        End If

        ReDim vTempGroup(vAllGroups.GetUpperBound(0), vAllGroups.GetUpperBound(1))
        If Information.IsArray(vAllGroups) And Information.IsArray(vUserGroup) Then
            ' the list of groups is the cartesian product of the 2 arrays
            iPos = 0

            For i1 As Integer = vUserGroup.GetLowerBound(1) To vUserGroup.GetUpperBound(1)

                For i2 As Integer = vAllGroups.GetLowerBound(1) To vAllGroups.GetUpperBound(1)


                    If Conversion.Val(CStr(vAllGroups(0, i2))) = Conversion.Val(CStr(vUserGroup(0, i1))) Then
                        ' found a match so save it


                        'vTempGroup(0, iPos) = vAllGroups(0, i2)
                        'vTempGroup(1, iPos) = vAllGroups(1, i2)
                        'vTempGroup(2, iPos) = vAllGroups(2, i2)

                        vTempGroup(0, iPos) = vAllGroups(0, i2)
                        vTempGroup(1, iPos) = vAllGroups(1, i2)
                        vTempGroup(2, iPos) = vAllGroups(2, i2)



                        iPos += 1
                        Exit For
                    End If
                Next i2
            Next i1

            ' resize the array to remove unused fields

            ReDim Preserve vTempGroup(vAllGroups.GetUpperBound(0), iPos - 1)

            r_vGroups = vTempGroup
        Else

            r_vGroups = vAllGroups
        End If

        Return result

    End Function
    'MKW110803 PN6022 1.8.5 to 1.8.6 Catchup END

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    Public Property ScheduledTaskKey() As String
        Get
            Return m_sScheduledTaskKey.Trim()
        End Get
        Set(ByVal Value As String)
            m_sScheduledTaskKey = Value.Trim()
        End Set
    End Property

    Private Property QSBarDirty() As Boolean
        Get
            Return m_bQSBarDirty
        End Get
        Set(ByVal Value As Boolean)
            m_bQSBarDirty = Value
        End Set
    End Property

    Private Property FavouritesChanged() As Boolean
        Get
            Return m_bFavouritesChanged
        End Get
        Set(ByVal Value As Boolean)
            m_bFavouritesChanged = Value
        End Set
    End Property

    Public ReadOnly Property NumOfTasksInProgress() As Integer
        Get

            If m_oInProgTasks Is Nothing Then
                Return 0
            Else
                Return m_oInProgTasks.Count()
            End If

        End Get
    End Property

    Public ReadOnly Property Username() As String
        Get
            Return m_sUsername.Trim()
        End Get
    End Property
    Public ReadOnly Property UserID() As Integer
        Get
            Return m_iUserID
        End Get
    End Property

    ' RDC 16082002

    Public Property UserGroupID() As Integer
        Get
            Return m_lUserGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lUserGroupID = Value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_fMainForm = New frmMain

            m_oObjectManager = New bObjectManager.ObjectManager()

            lReturn = m_oObjectManager.Initialise(ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            With m_oObjectManager
                m_sUsername = .UserName
                m_iUserID = .UserID
            End With

            ' Get the Business
            m_oBusiness = New bPMWrkManager.FormClass
            lReturn = m_oObjectManager.GetInstance(m_oBusiness, "bPMWrkManager.FormClass", PMGetViaClientManager)
            '         Dim temp_m_oBusiness As Object
            'lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bPMWrkManager.FormClass", gPMConstants.PMGetViaClientManager)
            'm_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return lReturn
            End If

            'DAK120700
            ' Get the Server Registry tools
            m_oServerRegistry = New bPMServerRegistry.PMServerRegistry
            lReturn = m_oObjectManager.GetInstance(m_oServerRegistry, "bPMServerRegistry.PMServerRegistry", gPMConstants.PMGetViaClientManager)
            '         Dim temp_m_oServerRegistry As Object
            'lReturn = m_oObjectManager.GetInstance(temp_m_oServerRegistry, "bPMServerRegistry.PMServerRegistry", gPMConstants.PMGetViaClientManager)
            'm_oServerRegistry = temp_m_oServerRegistry
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return lReturn
            End If

            ' Create the Form
            'Modified by Tarun on 5/11/2010 4:22:54 PM unnecessary code removed.
            'm_fMainForm = New frmMain()

            ' Set the Form Parent
            m_fMainForm.Parent_Renamed = Me

            ' Create New Available Tasks Collection
            m_oAvailableTasks = New PMWorkManager.AvailableTasks()
            'Developer Guide No.9
            lReturn = m_oAvailableTasks.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return lReturn
            End If

            ' Create New Scheduled Tasks Collection
            m_oSchedTasks = New PMWorkManager.ScheduledTasks()
            'Developer Guide No.9
            lReturn = m_oSchedTasks.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return lReturn
            End If

            ' Create New Navigator Instances Collection
            m_oInProgTasks = New PMWorkManager.InProgTasks()
            'Developer Guide No.9
            lReturn = m_oInProgTasks.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return lReturn
            End If

            'MKW110803 PN6022 1.8.5 to 1.8.6 Catchup START
            ' SET 08/04/2003 ISS3299
            ' Get the usergroup Business object
            'Modified by Tarun on 5/11/2010 4:22:54 PM unnecessary code removed.
            'Dim temp_m_oUserGroup As Object
            'lReturn = m_oObjectManager.GetInstance(temp_m_oUserGroup, "bPMUserGroup.LookUp", gPMConstants.PMGetViaClientManager)
            'm_oUserGroup = temp_m_oUserGroup
            lReturn = m_oObjectManager.GetInstance(m_oUserGroup, "bPMUserGroup.LookUp", gPMConstants.PMGetViaClientManager)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return lReturn
            End If
            'MKW110803 PN6022 1.8.5 to 1.8.6 Catchup END

            m_dtNextCheckSystemTasks = DateTime.Now

            ' Set the Help File
            'TODO
            'App.HelpFile = My.Application.Info.DirectoryPath & ACHelpFileLocation

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            m_fMainForm.FavouritesCaption = "Favourites"

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
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
                If m_oAvailableTasks IsNot Nothing Then
                    m_oAvailableTasks.Dispose()
                    m_oAvailableTasks = Nothing
                End If
                If m_oSchedTasks IsNot Nothing Then
                    m_oSchedTasks.Dispose()
                    m_oSchedTasks = Nothing
                End If
                If m_oInProgTasks IsNot Nothing Then
                    m_oInProgTasks.Dispose()
                    m_oInProgTasks = Nothing
                End If
                If m_oServerRegistry IsNot Nothing Then
                    m_oServerRegistry.Dispose()
                    m_oServerRegistry = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If

                If Not (m_fMainForm Is Nothing) Then
                    ' Terminate Form
                    m_fMainForm.Parent_Renamed = Nothing
                    m_fMainForm.Close()
                    m_fMainForm = Nothing
                End If

                If m_oUserGroup IsNot Nothing Then
                    m_oUserGroup.Dispose()
                    m_oUserGroup = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Start Work Manager.
    '
    '
    ' ***************************************************************** '
    Public Function Start() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            m_fMainForm.cboUser.FirstItem = "()"
            m_fMainForm.cboTaskGroups.FirstItem = ""
            m_fMainForm.cboUserGroup.FirstItem = "()"
            m_fMainForm.cboAllUsers.FirstItem = "()"
            m_fMainForm.AddWebsiteButtons()
            ' Get the Registry Settings
            GetRegistrySettings()

            If m_fMainForm.ViewSplash Then
                ' Display the Splash Screen
                SplashScreen(True)
            End If

            ' Setup the Form
            lReturn = SetFormForDisplay()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Show the Form
            'Developer Guide No.14
            ' Close the Splash Screen
            SplashScreen(False)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'Developer Guide No.14
            If Not m_fMainForm Is Nothing And Not m_fMainForm.IsDisposed Then
                Application.Run(m_fMainForm)
            End If



            Return result

        Catch exDisposed As ObjectDisposedException

            Initialise()
            Start()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Returns whether the User is a Sys Admin or Supervisor
    '              or Normal User.
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_lPMAuthorityLevel As Integer, Optional ByVal v_lUserGroupID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBusiness As bPMWrkManager.FormClass
        Static bIsAdministrator As Boolean
        Static vSupervisedGroups As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default the Authority to User
            r_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALUser

            ' If we have NOT previously got the User Authority details
            ' then get them.

            If Object.Equals(vSupervisedGroups, Nothing) Then

                ' Get the Business
                'Modified by Tarun on 5/11/2010 4:52:46 PM unnecessary code commented.
                'Dim temp_oBusiness As Object
                'lReturn = m_oObjectManager.GetInstance(temp_oBusiness, "bPMWrkManager.FormClass", gPMConstants.PMGetViaClientManager)
                'oBusiness = temp_oBusiness
                lReturn = m_oObjectManager.GetInstance(oBusiness, "bPMWrkManager.FormClass", gPMConstants.PMGetViaClientManager)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If


                lReturn = oBusiness.GetUserAuthority(r_bIsAdministrator:=bIsAdministrator, r_vSupervisedGroups:=vSupervisedGroups)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    oBusiness.Dispose()
                    oBusiness = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                oBusiness.Dispose()
                oBusiness = Nothing

            End If

            ' Is the User an Administrator
            If bIsAdministrator Then
                ' Yes, so set the Level and exit.
                ' Note: There is no need to check if they are a Group
                '       Supervisor as SysAdmin is a higher level authority.
                r_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin
                Return result
            End If

            ' Has A GroupID been supplied ?
            If v_lUserGroupID > 0 Then
                ' Do they supervise any Groups ?
                If Information.IsArray(vSupervisedGroups) Then
                    ' Yes, so check to see if they Supervise the Group supplied

                    For lRow As Integer = vSupervisedGroups.GetLowerBound(1) To vSupervisedGroups.GetUpperBound(1)
                        ' If the GroupID's match

                        If (v_lUserGroupID) = CInt(vSupervisedGroups(0, lRow)) Then
                            ' Set Authority Level to Supervisor
                            r_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSupervisor
                            Exit For
                        End If
                    Next lRow
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetFormForDisplay
    '
    ' Description: Gets the Form ready for display.
    '
    ' ***************************************************************** '
    Private Function SetFormForDisplay() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel
        'DAK110100
        'Dim iFavouritesCaptionResource      As Long




        result = gPMConstants.PMEReturnCode.PMTrue

        With m_fMainForm
            ' Populate the Task Status
            .cboTaskStatus.Items.Add(ACListTaskTypeAllButComplete)
            .cboTaskStatus.Items.Add(ACListTaskTypeAll)
            .cboTaskStatus.Items.Add(ACListTaskTypeNew)
            .cboTaskStatus.Items.Add(ACListTaskTypeInProgress)
            .cboTaskStatus.Items.Add(ACListTaskTypeInComplete)
            .cboTaskStatus.Items.Add(ACListTaskTypeComplete)
            .cboTaskStatus.SelectedIndex = 0

            'Populate Batch Task Status Date
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescAll)
            Dim indxBatchDate As Integer
            indxBatchDate = m_fMainForm.cboBatchTaskDateRange.Items.Count - 1

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexAll)
            indxBatchDate = indxBatchDate + 1
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescToday)

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexToday)
            indxBatchDate = indxBatchDate + 1
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescPrev1)

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexPrev1)
            indxBatchDate = indxBatchDate + 1
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescPrev2)

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexPrev2)
            indxBatchDate = indxBatchDate + 1
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescPrev3)

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexPrev3)
            indxBatchDate = indxBatchDate + 1
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescPrev4)

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexPrev4)
            indxBatchDate = indxBatchDate + 1
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescPrev5)

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexPrev5)
            indxBatchDate = indxBatchDate + 1
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescPrev6)

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexPrev6)
            indxBatchDate = indxBatchDate + 1
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescPrev7)

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexPrev7)
            indxBatchDate = indxBatchDate + 1
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescPrev14)

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexPrev14)
            indxBatchDate = indxBatchDate + 1
            .cboBatchTaskDateRange.Items.Add(kACBatchDateRangeDescPrev28)

            VB6.SetItemData(.cboBatchTaskDateRange, indxBatchDate, kACBatchDateRangeIndexPrev28)
            ' Default is Past 7 Days
            .cboBatchTaskDateRange.SelectedIndex = kACBatchDateRangeIndexPrev7

            'TODO
            .cboDateRange.Items.Add(ACDateRangeDescAll)
            Dim indx As Integer
            indx = m_fMainForm.cboDateRange.Items.Count - 1

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexAll)
            indx = indx + 1
            .cboDateRange.Items.Add(ACDateRangeDescToday)

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexToday)
            indx = indx + 1
            .cboDateRange.Items.Add(ACDateRangeDescNext1)

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexNext1)
            indx = indx + 1
            .cboDateRange.Items.Add(ACDateRangeDescNext2)

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexNext2)
            indx = indx + 1
            .cboDateRange.Items.Add(ACDateRangeDescNext3)

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexNext3)
            indx = indx + 1
            .cboDateRange.Items.Add(ACDateRangeDescNext4)

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexNext4)
            indx = indx + 1
            .cboDateRange.Items.Add(ACDateRangeDescNext5)

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexNext5)
            indx = indx + 1
            .cboDateRange.Items.Add(ACDateRangeDescNext6)

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexNext6)
            indx = indx + 1
            .cboDateRange.Items.Add(ACDateRangeDescNext7)

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexNext7)
            indx = indx + 1
            .cboDateRange.Items.Add(ACDateRangeDescNext14)

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexNext14)
            indx = indx + 1
            .cboDateRange.Items.Add(ACDateRangeDescNext28)

            VB6.SetItemData(.cboDateRange, indx, ACDateRangeIndexNext28)
            ' Default is All Dates
            .cboDateRange.SelectedIndex = ACDateRangeIndexAll


            ' What Type of Tasks to look at.
            .cboShowSystem.Items.Add(ACListShowSystemAll)
            .cboShowSystem.Items.Add(ACListShowSystemUser)
            .cboShowSystem.Items.Add(ACListShowSystemOnly)

            'DAK040100
            ' Enable the Timer
            .tmrSystemTasks.Enabled = True
            If ACSystemTasksTimerInterval = 0 Then
                .tmrSystemTasks.Enabled = False
            Else
                .tmrSystemTasks.Interval = ACSystemTasksTimerInterval
                .tmrSystemTasks.Enabled = True
            End If
            'DAK240700 - Revert to previous system timer settings and use new Refresh timer
            '        If .IsAutoRefresh Then
            '            .tmrSystemTasks.Enabled = True
            '            .tmrSystemTasks.Interval = ACSystemTasksTimerInterval
            '        Else
            '            .tmrSystemTasks.Enabled = False
            '        End If
            .tmrRefreshTimer.Enabled = .IsAutoRefresh

            ' Get the User Authority
            'Developer Guide No.9
            lReturn = GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel)

            ' Is the User a System Administrator
            If lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                ' Yes, then default to all & allow them to change
                .cboShowSystem.SelectedIndex = 0

                ' Enable the Timer
                'DAK040100
                '            .tmrSystemTasks.Enabled = True
                '            .tmrSystemTasks.Interval = ACSystemTasksTimerInterval
            Else
                ' No, so default to User and do not allow them to change
                .cboShowSystem.SelectedIndex = 1
                .cboShowSystem.Visible = False

                ' Disable the Timer
                'DAK040100
                '            .tmrSystemTasks.Enabled = False
            End If

        End With
        Dim vArray(,) As Object
        Dim iCurrentAgent As Object
        lReturn = m_oBusiness.GetAgents(r_vArray:=vArray, r_iCurrentAgent:=iCurrentAgent)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve Agents.")
            Return result
        End If
        m_fMainForm.cboParty.Items.Add(ACListShowAgentAll)
        m_fMainForm.cboParty.SelectedIndex = ACAgentIndexAll
        If Not vArray Is Nothing Then
            For iLoop As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                Dim cboParty_NewIndex As Integer = 0

                cboParty_NewIndex = m_fMainForm.cboParty.Items.Add(CStr(vArray(1, iLoop)))

                VB6.SetItemData(m_fMainForm.cboParty, cboParty_NewIndex, CInt(vArray(0, iLoop)))


            Next
        End If
        If iCurrentAgent <> 0 Then
            'Set Selected Value to iCurrentAgent
            For nCount As Integer = 0 To m_fMainForm.cboParty.Items.Count - 1

                If iCurrentAgent = VB6.GetItemData(m_fMainForm.cboParty, nCount) Then
                    m_fMainForm.cboParty.SelectedIndex = nCount
                    cboPartySelectedValue = iCurrentAgent
                    Exit For
                End If

            Next
            m_fMainForm.cboParty.Enabled = False
        Else
            m_fMainForm.cboParty.Enabled = True
        End If
        'Add a manual Item (All) to cboParty with ItemData set to 0
        'Add Items to cboParty from the vArray
        ' User VB6.SetItemData to set Item Data 	
        'If iCurrentAgent <> 0 Then
        'Set Selected Value to iCurrentAgent
        'And Set CboParty.Enabled = False
        'Else
        'Set Selected Value to (All)
        'And Set CboParty.Enabled = True
        'End If


        ' Get the Form Ready for initial display
        lReturn = m_fMainForm.SetForDisplay()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.3.2)
        Dim iLanguageID As Integer
        'tarun modified below line
        'lReturn = CType(gPMFunctions.GetUserIsAmericanLanguageID(iLanguageID), gPMConstants.PMEReturnCode)
        lReturn = gPMFunctions.GetUserIsAmericanLanguageID(iLanguageID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'iFavouritesCaptionResource = ACFavouritiesCaption


        'Modified by Archana Tokas on 5/21/2010 4:09:55 PM function added in the same class 
        'm_sFavouritesCaption = CStr(iPMFunc.GetResData(iLangID:=iLanguageID, lId:=ACFavouritiesCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
        'm_sFavouritesCaption = CStr(iPMFunc.GetResData(iLangID:=iLanguageID, lId:=ACFavouritiesCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
        'If iLanguageID = kUSLangId Then
        '    m_fMainForm.albAvailableTasks.Groups.Item(1).Key = m_sFavouritesCaption
        '    m_fMainForm.albAvailableTasks.CurrentGroupCaption = m_sFavouritesCaption
        'End If
        'tarun modified below line
        'lReturn = CType(PopulateAvailableTasks(m_sFavouritesCaption), gPMConstants.PMEReturnCode)
        lReturn = PopulateAvailableTasks(m_sFavouritesCaption)
        'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.3.2)

        ' Populate Users Available Tasks
        ' lReturn = PopulateAvailableTasks()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Populate Scheduled Tasks
        lReturn = CType(PopulateSchedTasks(v_bForceRefresh:=True), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Load Quick Start Tasks
        'DAK130999 - Replaced by Favourites group on Sheridan
        '            Active List bar.
        'lReturn = LoadQSBarTasks
        lReturn = LoadFavourites()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' DEMO CF CF CF CF CF CF CF CF CF CF CF CF CF CF DEM '
        ' CF DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO CF '
        ' DEMO CF CF CF CF CF CF CF CF CF CF CF CF CF CF DEM '

        ' Flip to first tab (not favorites)
        'm_fMainForm.albAvailableTasks.CurrentGroup = 2

        ' DEMO CF CF CF CF CF CF CF CF CF CF CF CF CF CF DEM '
        ' CF DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO DEMO CF '
        ' DEMO CF CF CF CF CF CF CF CF CF CF CF CF CF CF DEM '
        m_fMainForm.lstScheduledTasks.CheckBoxes = False
        m_fMainForm.lstScheduledTasks.FullRowSelect = True

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: PopulateAvailableTasks
    '
    ' Description: Get the list of Tasks that the current user has
    '              access to and populates the Form Tree View.
    '
    'DAK150999 - Icons are now related to the task
    ' ***************************************************************** '
    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.3.1)
    'Added the optional parameter sFavouritesCaption
    Private Function PopulateAvailableTasks(Optional ByRef sFavouritesCaption As String = "") As Integer
        'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.3.1)

        Dim result As Integer = 0
        Dim vAvailableTaskArray(,) As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim lPMWrkTaskGroupID As Integer
        Dim lPMWrkTaskID As Integer
        'Dim sTaskCaption As Integer
        Dim sTaskCaption As String
        Dim iIsSupervisor As Integer
        Dim iTypeOfTask As Integer
        Dim iIsSystemTask As Integer
        Dim lPMNavprocessID As Integer
        'Developer Guide No. 101
        Dim sObjectName As String
        Dim sClassName As String
        Dim lDeleteAfterDays As Integer
        Dim sKey As String = ""
        Dim lQuickStartAvailable As gPMConstants.PMEReturnCode
        Dim sTaskImage As String = ""
        Dim lTaskIcon As Integer
        Dim iIsViewOnlyTask As Integer
        Dim sLinkedObjectName, sLinkedClassName, sLinkedCaption As String
        Dim iCurrentGroupIndex As Integer
        ' RDC 02122002
        Dim sNavXMLfile As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Tasks from the Business

        lReturn = m_oBusiness.GetAvailableTasks(r_vAvailableTasksArray:=vAvailableTaskArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve Available Tasks.")
            Return result
        End If

        ' Clear Collection and Form.
        m_oAvailableTasks.Clear()

        'DAK220999 - clear Avaliable Tasks Bar
        'With m_fMainForm.albAvailableTasks

        '    If iCurrentGroupIndex <> 1 Then
        '        iCurrentGroupIndex = .CurrentGroup.Index
        '    End If

        '    .ListItems.Clear()
        '    'Vijaypal
        '    Do While .Groups.Count > 2
        '        .Groups.Remove(.Groups.Item(.Groups.Count))
        '    Loop

        'End With

        m_fMainForm.cboTaskGroups.RefreshList()

        ' If there are no Available Tasks then Exit.
        If Not Information.IsArray(vAvailableTaskArray) Then
            Return result
        End If


        For lRow As Integer = vAvailableTaskArray.GetLowerBound(1) To vAvailableTaskArray.GetUpperBound(1)


            lPMWrkTaskGroupID = CInt(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskGroupIDCol, lRow))

            lPMWrkTaskID = CInt(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskIDCol, lRow))

            sTaskCaption = vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskCaptionCol, lRow)

            iIsSupervisor = CInt(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskIsSupervisorCol, lRow))

            iTypeOfTask = CInt(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskTypeOfTaskCol, lRow))

            iIsSystemTask = CInt(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskIsSystemTaskCol, lRow))

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskPMNavProcessIDCol, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                lPMNavprocessID = CInt(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskPMNavProcessIDCol, lRow))
            Else
                lPMNavprocessID = 0
            End If

            sObjectName = vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskObjectNameCol, lRow)

            sClassName = vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskClassNameCol, lRow)

            lDeleteAfterDays = CInt(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskDeleteAfterDaysCol, lRow))

            lTaskIcon = CInt(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskDisplayIconCol, lRow))

            iIsViewOnlyTask = CInt(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskIsViewOnlyTaskCol, lRow))

            sLinkedObjectName = CStr(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskLinkedObjectNameCol, lRow))
            'sLinkedObjectName = vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskLinkedObjectNameCol, lRow)

            sLinkedClassName = CStr(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskLinkedClassNameCol, lRow))

            sLinkedCaption = CStr(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskLinkedCaptionCol, lRow))

            sNavXMLfile = CStr(vAvailableTaskArray(gPMConstants.PMEACAvailTaskCol.ACAvailTaskNavXMLFileCol, lRow))

            ' RDC 10012003 add v_asNavXMLfile parameter
            lReturn = CType(m_oAvailableTasks.Add(v_lPMWrkTaskGroupID:=lPMWrkTaskGroupID, v_lPMWrkTaskID:=lPMWrkTaskID, v_sTaskCaption:=sTaskCaption, v_iIsSupervisor:=iIsSupervisor, v_iTypeOfTask:=iTypeOfTask, v_iIsSystemTask:=iIsSystemTask, v_lPMNavProcessID:=lPMNavprocessID, v_sObjectName:=CStr(sObjectName), v_sClassName:=CStr(sClassName), v_lDeleteAfterDays:=lDeleteAfterDays, v_lDisplayIcon:=lTaskIcon, v_iIsViewOnlyTask:=iIsViewOnlyTask, v_sLinkedObjectName:=sLinkedObjectName, v_sLinkedClassName:=sLinkedClassName, v_sLinkedCaption:=sLinkedCaption, r_sKey:=sKey, v_sNavXMLfile:=sNavXMLfile), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Work out whether Quick Start should be available or not
            ' Note: Quick Start is not available for Memo's or System Tasks
            If (iTypeOfTask = gPMConstants.PMEWrkManTaskType.pmeWMTTMemo) Or (iIsSystemTask = gPMConstants.PMEReturnCode.PMTrue) Then
                lQuickStartAvailable = gPMConstants.PMEReturnCode.PMFalse
            Else
                lQuickStartAvailable = gPMConstants.PMEReturnCode.PMTrue
            End If

            ' Select the Icon Based on the TypeOfTask
            Select Case iTypeOfTask
                Case gPMConstants.PMEWrkManTaskType.pmeWMTTMemo
                    sTaskImage = ACTaskMemoImage
                Case gPMConstants.PMEWrkManTaskType.pmeWMTTSingleComponent
                    sTaskImage = ACTaskSingleComponentImage
                Case gPMConstants.PMEWrkManTaskType.pmeWMTTNavigatorProcess
                    sTaskImage = ACTaskNavProcessImage
            End Select

            ' Use the System icon if applicable.
            If iIsSystemTask = gPMConstants.PMEReturnCode.PMTrue Then
                sTaskImage = ACTaskSystemImage
            End If

            ' Populate the Form.
            lReturn = CType(m_fMainForm.AddAvailableTask(v_lTaskGroupID:=lPMWrkTaskGroupID, v_lTaskID:=lPMWrkTaskID, v_sTaskKey:=sKey, v_sTaskCaption:=sTaskCaption, v_iIsSystemTask:=iIsSystemTask, v_iTypeOfTask:=iTypeOfTask, v_lQuickStartAvailable:=lQuickStartAvailable, v_lDisplayIcon:=lTaskIcon), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve Available Tasks.")
                Return result
            End If

        Next lRow



        'With m_fMainForm.albAvailableTasks
        '    If iCurrentGroupIndex > .Groups.Count Then
        '        .CurrentGroup = .Groups.Item(.Groups.Count)
        '    Else
        '        .CurrentGroup = .Groups.Item(iCurrentGroupIndex)
        '    End If

        '    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2)
        '    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2)
        '    If .CurrentGroupCaption = m_sFavouritesCaption Then
        '        'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.3.1)
        '        m_fMainForm.InFavouritesGroup = True
        '    Else
        '        m_fMainForm.InFavouritesGroup = False
        '    End If


        'End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PopulateSchedTasks
    '
    ' Description: Populate the Scheduled Tasks List View
    '
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function PopulateSchedTasks(ByVal v_bForceRefresh As Object) As Integer

        Dim result As Integer = 0
        'Developer Guide No. 15(Guide)
        Dim lReturn As Long
        'Developer Guide No. 17(Guide)
        Dim vSchedTaskArray(,) As Object
        Dim vSystemTaskArray(,) As Object
        Dim lCurrentTaskStatus As gPMConstants.PMEWrkManTaskStatus
        Dim lCurrentPMUserGroupID, lCurrentUserID As Integer
        Dim dtEndDate As Date
        Dim bCurrentOmitCompleted As Boolean
        Dim sCurrentShowSystem As String = ""

        Static lPreviousTaskStatus As gPMConstants.PMEWrkManTaskStatus
        Static lPreviousPMUserGroupID, lPreviousUserID As Integer
        Static dtPreviousEndDate As Date
        Static bPreviousOmitCompleted As Boolean
        Static sPreviousShowSystem As String = ""

        Dim dtSystemEndDate As Date

        Dim bShowUserTasks, bShowSystemTasks As Boolean
        Dim lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel

        ' RDC 04092002
        Dim vGroups As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        '    MKW110803 PN6022 1.8.5 to 1.8.6 Catchup START
        '    ' RDC 04092002
        '    vGroups = m_fMainForm.cboUserGroup.UserGroups
        '    MKW110803 PN6022 1.8.5 to 1.8.6 Catchup END

        ' Get the Users Authority Level
        lReturn = CType(GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel), gPMConstants.PMEReturnCode)

        ' Refresh User and System Tasks by default.
        bShowUserTasks = True
        bShowSystemTasks = True

        ' Default the Current Selection Criteria
        lCurrentTaskStatus = -1
        lCurrentPMUserGroupID = -1
        lCurrentUserID = -1
        dtEndDate = #12/29/1899#
        bCurrentOmitCompleted = False
        sCurrentShowSystem = ""

        ' Set the Specific Selection Criteria
        Select Case VB6.GetItemString(m_fMainForm.cboTaskStatus, m_fMainForm.cboTaskStatus.SelectedIndex)
            ' All Tasks
            Case ACListTaskTypeAll

                ' New Only
            Case ACListTaskTypeNew
                lCurrentTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew

            Case ACListTaskTypeInProgress
                lCurrentTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress

            Case ACListTaskTypeComplete
                lCurrentTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete

            Case ACListTaskTypeInComplete
                lCurrentTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete

            Case ACListTaskTypeAllButComplete
                bCurrentOmitCompleted = True
        End Select

        lCurrentPMUserGroupID = m_fMainForm.cboUserGroup.UserGroupID
        If lCurrentPMUserGroupID < 1 Then
            lCurrentPMUserGroupID = -1
        End If
        lCurrentUserID = m_fMainForm.cboUser.UserID
        If lCurrentUserID < 1 Then
            lCurrentUserID = -1
        End If

        ' Default the End Date to End of Today
        dtEndDate = DateTime.Today.AddDays(DateAndTime.TimeSerial(23, 59, 59).ToOADate())

        ' Work out the Date Range End Date
        Select Case (VB6.GetItemData(m_fMainForm.cboDateRange, m_fMainForm.cboDateRange.SelectedIndex))
            Case ACDateRangeIndexAll
                ' Set end date to include all dates
                dtEndDate = #12/29/1899#
            Case ACDateRangeIndexToday
                ' Already Defaulted to this
            Case ACDateRangeIndexNext1
                ' Add 1 Day
                dtEndDate = dtEndDate.AddDays(1)
            Case ACDateRangeIndexNext2
                ' Add 2 Day
                dtEndDate = dtEndDate.AddDays(2)
            Case ACDateRangeIndexNext3
                ' Add 3 Day
                dtEndDate = dtEndDate.AddDays(3)
            Case ACDateRangeIndexNext4
                ' Add 4 Day
                dtEndDate = dtEndDate.AddDays(4)
            Case ACDateRangeIndexNext5
                ' Add 5 Day
                dtEndDate = dtEndDate.AddDays(5)
            Case ACDateRangeIndexNext6
                ' Add 6 Day
                dtEndDate = dtEndDate.AddDays(6)
            Case ACDateRangeIndexNext7
                ' Add 7 Day
                dtEndDate = dtEndDate.AddDays(7)
            Case ACDateRangeIndexNext14
                ' Add 14 Day
                dtEndDate = dtEndDate.AddDays(14)
            Case ACDateRangeIndexNext28
                ' Add 28 Day
                dtEndDate = dtEndDate.AddDays(28)
            Case Else
                ' Set end date to include all dates
                dtEndDate = #12/29/1899#
        End Select

        'MKW110803 PN6022 1.8.5 to 1.8.6 catchup START
        ' SET 08/04/2003 ISS3299 -

        lReturn = CType(GetGroups(r_vGroups:=vGroups, dtEffectiveDate:=dtEndDate, lUserId:=lCurrentUserID, lGroupId:=lCurrentPMUserGroupID), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Clear the any existing tasks
            m_fMainForm.lstScheduledTasks.Items.Clear()
            m_fMainForm.UpdateStatusBar()

            ' save the selection criteria
            lPreviousTaskStatus = lCurrentTaskStatus
            lPreviousPMUserGroupID = lCurrentPMUserGroupID
            lPreviousUserID = lCurrentUserID
            dtPreviousEndDate = dtEndDate
            bPreviousOmitCompleted = bCurrentOmitCompleted
            sPreviousShowSystem = sCurrentShowSystem
            Return result
        End If
        'MKW110803 PN6022 1.8.5 to 1.8.6 catchup END

        ' Work out which Tasks to Show
        cboPartySelectedValue = VB6.GetItemData(m_fMainForm.cboParty, m_fMainForm.cboParty.SelectedIndex)
        sCurrentShowSystem = VB6.GetItemString(m_fMainForm.cboShowSystem, m_fMainForm.cboShowSystem.SelectedIndex)

        Select Case (sCurrentShowSystem)
            ' Show System Tasks Only
            Case ACListShowSystemOnly
                bShowUserTasks = False

                ' Show User Tasks Only
            Case ACListShowSystemUser
                bShowSystemTasks = False

                ' Show All Tasks
            Case ACListShowSystemAll
                bShowUserTasks = True
                bShowSystemTasks = True

        End Select

        ' If we are NOT Forcing a Refresh AND
        ' we have we already filled the list with this Selection then EXIT
        If (Not v_bForceRefresh) And (lPreviousTaskStatus = lCurrentTaskStatus) And (lPreviousPMUserGroupID = lCurrentPMUserGroupID) And (lPreviousUserID = lCurrentUserID) And (dtPreviousEndDate = dtEndDate) And (bPreviousOmitCompleted = bCurrentOmitCompleted) And (sPreviousShowSystem = sCurrentShowSystem) And (cboPartySelectedValue < 0) Then
            Return result
        End If

        m_fMainForm.UpdateStatusBar(v_vActivity:=ACStatusActSearching)

        ' Clear the any existing tasks
        m_fMainForm.lstScheduledTasks.Items.Clear()

        ' Clear the Collection
        m_oSchedTasks.Clear()

        ' Do we need to Get & Show User Tasks
        If bShowUserTasks Then

            ' Get the Scheduled Tasks from business

            lReturn = m_oBusiness.GetScheduledTasks(r_vScheduledTaskArray:=vSchedTaskArray, v_lTaskStatus:=lCurrentTaskStatus, v_lPmuserGroupID:=lCurrentPMUserGroupID, v_iUserID:=lCurrentUserID, v_dtDueDateLimit:=dtEndDate, v_bOmitCompleted:=bCurrentOmitCompleted, vGroups:=vGroups, PartyKey:=cboPartySelectedValue)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the Scheduled Tasks.", vApp:=ACApp, vClass:=ACClass)
                Return result
            End If

            ' Add the Scheduled Tasks
            lReturn = AddScheduledTasks(r_vSchedTaskArray:=vSchedTaskArray, v_bDisplayOnForm:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to populate the Scheduled Tasks.", vApp:=ACApp, vClass:=ACClass)
                Return result
            End If

        End If

        ' IF the User is a System Adminstrator then we always need to GET
        ' the System Tasks (Regardless of whether we are Showing them.)
        If lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then

            ' Get ALL System Tasks which are due within the Next 3 days.
            dtSystemEndDate = DateTime.Today.AddDays(DateAndTime.TimeSerial(23, 59, 59).ToOADate())
            dtSystemEndDate = dtSystemEndDate.AddDays(ACSystemTasksWithinDays)

            ' Get the System Tasks from business

            lReturn = m_oBusiness.GetScheduledSystemTasks(r_vSystemTaskArray:=vSystemTaskArray, v_dtDueDateLimit:=dtSystemEndDate, PartyKey:=cboPartySelectedValue)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the System Tasks.", vApp:=ACApp, vClass:=ACClass)
                Return result
            End If

        End If

        ' If we are Showing System Tasks then Add them to the Form.
        If bShowSystemTasks Then

            ' Add the Scheduled Tasks
            'Tarun modified the belwo code
            'lReturn = CType(AddScheduledTasks(r_vSchedTaskArray:=vSystemTaskArray, v_bDisplayOnForm:=bShowSystemTasks), gPMConstants.PMEReturnCode)
            lReturn = AddScheduledTasks(r_vSchedTaskArray:=vSystemTaskArray, v_bDisplayOnForm:=bShowSystemTasks)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to populate the System Tasks.", vApp:=ACApp, vClass:=ACClass)
                Return result
            End If

        End If

        ' Turn Sorting Back On & Refresh
        ' Tarun modfified for below line.
        'm_fMainForm.listViewHelper1.SetSorted(m_fMainForm.lstScheduledTasks, True)
        m_fMainForm.lstScheduledTasks.Sort()
        m_fMainForm.lstScheduledTasks.Refresh()
        m_fMainForm.lstScheduledTasks.FullRowSelect = True


        ' All OK so save the selection criteria
        lPreviousTaskStatus = lCurrentTaskStatus
        lPreviousPMUserGroupID = lCurrentPMUserGroupID
        lPreviousUserID = lCurrentUserID
        dtPreviousEndDate = dtEndDate
        bPreviousOmitCompleted = bCurrentOmitCompleted
        sPreviousShowSystem = sCurrentShowSystem

        ' Release any Memory used by the Arrays

        vSchedTaskArray = Nothing

        vSystemTaskArray = Nothing

        m_fMainForm.UpdateStatusBar(v_vActivity:="")

        Return result

    End Function


    ''' <summary>
    ''' Populate the Batch Tasks List View
    ''' </summary>
    ''' <param name="bForceRefresh"></param>
    ''' <returns></returns>
    Private Function PopulateBatchTasks(ByVal bForceRefresh As Boolean) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oBatchTaskArray(,) As Object
        Dim dtBatchTask As DataTable

        Dim sCurrentTaskStatus As String = "All"
        Dim dtEndDate As Date
        Dim sCurrentShowSystem As String = ""

        Static sPreviousTaskStatus As String
        Static dtPreviousEndDate As Date
        Static sPreviousShowSystem As String = ""

        dtEndDate = #12/29/1899#
        sCurrentShowSystem = ""

        If m_fMainForm.cboBatchTaskStatus.ListIndex <> 0 Then
            sCurrentTaskStatus = m_fMainForm.cboBatchTaskStatus.ItemCode.Trim()
        End If

        dtEndDate = DateTime.Today.AddDays(DateAndTime.TimeSerial(0, 0, 0).ToOADate())

        Select Case (VB6.GetItemData(m_fMainForm.cboBatchTaskDateRange, m_fMainForm.cboBatchTaskDateRange.SelectedIndex))
            Case kACBatchDateRangeIndexAll
                dtEndDate = #12/29/1899#
            Case kACBatchDateRangeIndexToday
            Case kACBatchDateRangeIndexPrev1
                dtEndDate = dtEndDate.AddDays(-1)
            Case kACBatchDateRangeIndexPrev2
                dtEndDate = dtEndDate.AddDays(-2)
            Case kACBatchDateRangeIndexPrev3
                dtEndDate = dtEndDate.AddDays(-3)
            Case kACBatchDateRangeIndexPrev4
                dtEndDate = dtEndDate.AddDays(-4)
            Case kACBatchDateRangeIndexPrev5
                dtEndDate = dtEndDate.AddDays(-5)
            Case kACBatchDateRangeIndexPrev6
                dtEndDate = dtEndDate.AddDays(-6)
            Case kACBatchDateRangeIndexPrev7
                dtEndDate = dtEndDate.AddDays(-7)
            Case kACBatchDateRangeIndexPrev14
                dtEndDate = dtEndDate.AddDays(-14)
            Case kACBatchDateRangeIndexPrev28
                dtEndDate = dtEndDate.AddDays(-28)
            Case Else
                dtEndDate = #12/29/1899#
        End Select

        If (Not bForceRefresh) And (sPreviousTaskStatus = sCurrentTaskStatus) And (dtPreviousEndDate = dtEndDate) Then
            Return nResult
        End If

        m_fMainForm.UpdateStatusBar(v_vActivity:=ACStatusActSearching)

        m_fMainForm.lstBatchTasksStatus.Items.Clear()
        m_fMainForm.lstBatchTasksStatus.Sorting = SortOrder.None

        nResult = m_oBusiness.GetBatchTasks(r_dtBatchTask:=dtBatchTask, sTaskStatus:=sCurrentTaskStatus, v_dtDueDateLimit:=dtEndDate)

        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the Batch Tasks.", vApp:=ACApp, vClass:=ACClass)
            Return nResult
        End If

        If (dtBatchTask.Rows.Count > 0) Then
            nResult = AddBatchTasks(r_dtBatchTask:=dtBatchTask, v_bDisplayOnForm:=True)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to populate the Batch Tasks.", vApp:=ACApp, vClass:=ACClass)
                Return nResult
            End If
        End If

        m_fMainForm.lstBatchTasksStatus.Sort()
        m_fMainForm.lstBatchTasksStatus.Refresh()
        m_fMainForm.lstBatchTasksStatus.FullRowSelect = True

        sPreviousTaskStatus = sCurrentTaskStatus
        dtPreviousEndDate = dtEndDate
        sPreviousShowSystem = sCurrentShowSystem

        oBatchTaskArray = Nothing
        m_fMainForm.UpdateStatusBar(v_vActivity:="")

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: AddScheduledTasks
    '
    ' Description: Adds the Scheduled Tasks from the Array.
    '
    '

    'Private Function AddScheduledTasks(ByRef r_vSchedTaskArray( ,  ) As Object, ByVal v_bDisplayOnForm As Boolean) As Integer
    Private Function AddScheduledTasks(ByRef r_vSchedTaskArray(,) As Object, ByVal v_bDisplayOnForm As Boolean) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim iIsUrgent, iTaskType, iIsSystem, iTaskStatus As Integer
        Dim lTaskInstanceCnt As Integer
        Dim dtTaskDueDate As Date
        Dim sCustomer, sDescription As String
        Dim lUserGroupID As Integer
        Dim vUserID As String = ""
        Dim vPMNavProcessID As String = ""
        Dim lPMNavprocessID As Integer
        Dim sComponentObjectName, sComponentClassName As String
        Dim lDisplayIcon As Integer
        Dim iIsViewOnlyTask As Integer
        Dim sLinkedObjectName, sLinkedClassName, sLinkedCaption As String
        'DAK141299
        Dim iIsVisible As Integer
        ' RDC 14012003
        Dim sNavXMLfile, sClientCode As String 'mkw100204 PN9978
        Dim sPartyName As String
        Dim iPartyCnt As Integer
        Dim iReadOnly As Integer
        

        result = gPMConstants.PMEReturnCode.PMTrue

        ' If there is no Summary to add, just exit.
        If Not Information.IsArray(r_vSchedTaskArray) Then
            Return result
        End If

        ' Add Each Row in the Array to the List
        For lRow As Integer = r_vSchedTaskArray.GetLowerBound(1) To r_vSchedTaskArray.GetUpperBound(1)

            ' Get the values from the Array

            iIsUrgent = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskUrgentCol, lRow))

            iTaskType = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskTypeCol, lRow))

            iIsSystem = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskIsSystemCol, lRow))

            iTaskStatus = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskStatusCol, lRow))

            lTaskInstanceCnt = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskInstanceCntCol, lRow))

            dtTaskDueDate = CDate(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskDueDateCol, lRow))

            sCustomer = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskCustomerCol, lRow))

            sDescription = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskDescriptionCol, lRow))

            lUserGroupID = CInt(CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskUserGroupIDCol, lRow)))

            vUserID = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskUserIDCol, lRow))

            vPMNavProcessID = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskNavProcessIDCol, lRow))
            Dim dbNumericTemp As Double
            If Double.TryParse(vPMNavProcessID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                lPMNavprocessID = CInt(vPMNavProcessID)
            Else
                lPMNavprocessID = 0
            End If

            sComponentObjectName = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskObjectNameCol, lRow))

            sComponentClassName = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskClassNameCol, lRow))

            lDisplayIcon = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskDisplayIconCol, lRow))

            iIsViewOnlyTask = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskIsViewOnlyTaskCol, lRow))

            sLinkedObjectName = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskLinkedObjectNameCol, lRow))

            sLinkedClassName = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskLinkedClassNameCol, lRow))

            sLinkedCaption = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskLinkedCaptionCol, lRow)).Trim()
            'DAK141299

            iIsVisible = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskIsVisibleCol, lRow))
            ' RDC 14012003

            sNavXMLfile = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskNavXMLfile, lRow))

            sClientCode = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskClientCode, lRow)) 'mkw100204 PN9978
            iReadOnly = ToSafeInteger(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskReadOnly, lRow))
            If r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskPartyCnt, lRow) = "" Then
                iPartyCnt = 0
            Else
                iPartyCnt = Convert.ToInt32(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskPartyCnt, lRow))
            End If

            sPartyName = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskPartyName, lRow))

            
            
            
            
            ' Add the Scheduled Task
            'DAK141299
            'mkw100204 PN9978 Added Client code Parameter

            lReturn = AddScheduledTask(v_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, _
                                       v_iIsUrgent:=iIsUrgent, _
                                       v_iTaskStatus:=iTaskStatus, _
                                       v_iTypeOfTask:=iTaskType, _
                                       v_iIsSystem:=iIsSystem, _
                                       v_dtTaskDueDate:=dtTaskDueDate, _
                                       v_sCustomer:=sCustomer, _
                                       v_sDescription:=sDescription, _
                                       v_lUserGroupID:=lUserGroupID, _
                                       v_vUserID:=vUserID, _
                                       v_lNavProcessID:=lPMNavprocessID, _
                                       v_sComponentObjectName:=sComponentObjectName, _
                                       v_sComponentClassName:=sComponentClassName, _
                                       v_lDisplayIcon:=lDisplayIcon, _
                                       v_iIsViewOnlyTask:=iIsViewOnlyTask, _
                                       v_sLinkedObjectName:=sLinkedObjectName, _
                                       v_sLinkedClassName:=sLinkedClassName, _
                                       v_sLinkedCaption:=sLinkedCaption, _
                                       v_iIsVisible:=iIsVisible, _
                                       v_bDisplayOnForm:=v_bDisplayOnForm, _
                                       v_sNavXMLfile:=sNavXMLfile, _
                                       v_sClientCode:=sClientCode, _
                                       v_sPartyName:=sPartyName, v_iPartyCnt:=iPartyCnt)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next lRow

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddScheduledTask
    '
    ' Description: Adds Scheduled Task to the List View and the Collection
    '
    ' ***************************************************************** '
    'DAK141299
    'mkw100204 PN9978 Added Client Code Parameter
    'Developer Guide No. 101
    Private Function AddScheduledTask(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, ByVal v_iIsUrgent As Integer, ByVal v_iTypeOfTask As Integer, ByVal v_iIsSystem As Integer, ByVal v_iTaskStatus As Integer, ByVal v_lUserGroupID As Integer, ByVal v_vUserID As Object, ByVal v_lNavProcessID As Integer, ByVal v_sComponentObjectName As String, ByVal v_sComponentClassName As String, ByVal v_lDisplayIcon As Integer, ByVal v_iIsViewOnlyTask As Integer, ByVal v_sLinkedObjectName As String, ByVal v_sLinkedClassName As String, ByVal v_sLinkedCaption As String, ByVal v_iIsVisible As Integer, ByVal v_bDisplayOnForm As Boolean, ByVal v_sNavXMLfile As String, ByVal v_sClientCode As String, Optional ByVal v_iIsReadOnly As Integer = 0, Optional ByVal v_sPartyName As String = "", Optional ByVal v_iPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sTaskTypeDesc, sTaskStatusDesc, sUserGroup, sUser, sKey As String
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Work out the Task Type desciption
        Select Case v_iTypeOfTask
            Case gPMConstants.PMEWrkManTaskType.pmeWMTTMemo
                sTaskTypeDesc = ACTaskTypeDescMemo
            Case gPMConstants.PMEWrkManTaskType.pmeWMTTSingleComponent
                sTaskTypeDesc = ACTaskTypeDescSingle
            Case gPMConstants.PMEWrkManTaskType.pmeWMTTNavigatorProcess
                sTaskTypeDesc = ACTaskTypeDescNavigator
        End Select

        If v_iIsSystem = gPMConstants.PMEReturnCode.PMTrue Then
            sTaskTypeDesc = ACTaskTypeDescSystem
        End If

        ' Work Out the Task Status Description
        sTaskStatusDesc = TaskStatusDescription(v_iTaskStatus)

        ' Get the User Group Caption
        sUserGroup = m_fMainForm.cboUserGroup.ItemUserGroupname(v_lUserGroupID).Trim

        ' Get the User Caption
        Dim dbNumericTemp As Double
        If Double.TryParse(CStr(v_vUserID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            If v_vUserID < 1 Then
                sUser = ""
            Else
                'sUser = m_fMainForm.cboUser.ItemUsername(v_vUserID)                 
                sUser = m_fMainForm.cboAllUsers.ItemUsername(v_vUserID:=v_vUserID,v_bRefresh:=False).Trim                    

            End If
        Else
            sUser = ""
        End If

        'DAK141299
        'mkw100204 PN9978 Added Client Code Parameter
        'Tarun modified 
        'lReturn = CType(m_oSchedTasks.Add(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_iIsUrgent:=v_iIsUrgent, v_iTaskStatus:=v_iTaskStatus, v_iTypeOfTask:=v_iTypeOfTask, v_iIsSystemTask:=v_iIsSystem, v_dtTaskDueDate:=v_dtTaskDueDate, v_sCustomer:=v_sCustomer, v_sDescription:=v_sDescription, v_lPmuserGroupID:=v_lUserGroupID, v_vUserID:=CStr(v_vUserID), v_sUserGroup:=sUserGroup, v_sUser:=sUser, v_lNavProcessID:=v_lNavProcessID, v_sComponentObjectName:=v_sComponentObjectName, v_sComponentClassName:=v_sComponentClassName, v_lDisplayIcon:=v_lDisplayIcon, v_iIsViewOnlyTask:=v_iIsViewOnlyTask, v_sLinkedObjectName:=v_sLinkedObjectName, v_sLinkedClassName:=v_sLinkedClassName, v_sLinkedCaption:=v_sLinkedCaption, v_iIsVisible:=v_iIsVisible, r_sKey:=sKey, v_sNavXMLfile:=v_sNavXMLfile, v_sClientCode:=v_sClientCode), gPMConstants.PMEReturnCode)
        lReturn = m_oSchedTasks.Add(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_iIsUrgent:=v_iIsUrgent, v_iTaskStatus:=v_iTaskStatus, v_iTypeOfTask:=v_iTypeOfTask, v_iIsSystemTask:=v_iIsSystem, v_dtTaskDueDate:=v_dtTaskDueDate, v_sCustomer:=v_sCustomer, v_sDescription:=v_sDescription, v_lPmuserGroupID:=v_lUserGroupID, v_vUserID:=CStr(v_vUserID), v_sUserGroup:=sUserGroup, v_sUser:=sUser, v_lNavProcessID:=v_lNavProcessID, v_sComponentObjectName:=v_sComponentObjectName, v_sComponentClassName:=v_sComponentClassName, v_lDisplayIcon:=v_lDisplayIcon, v_iIsViewOnlyTask:=v_iIsViewOnlyTask, v_sLinkedObjectName:=v_sLinkedObjectName, v_sLinkedClassName:=v_sLinkedClassName, v_sLinkedCaption:=v_sLinkedCaption, v_iIsVisible:=v_iIsVisible, r_sKey:=sKey, v_sNavXMLfile:=v_sNavXMLfile, v_sClientCode:=v_sClientCode, v_iIsReadOnly:=v_iIsReadOnly, PartyName:=v_sPartyName, PartyKey:=v_iPartyCnt)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If we are to Display this Task, then Add on Form
        'DAK141299
        If v_bDisplayOnForm And v_iIsVisible = gPMConstants.PMEReturnCode.PMTrue Then

            'mkw100204 PN9978 Added Client Code Parameter
            'Tarun modified
            'lReturn = CType(m_fMainForm.AddScheduledTaskToList(v_sKey:=sKey, v_iIsUrgent:=v_iIsUrgent, v_sTaskStatusDesc:=sTaskStatusDesc, v_sTaskTypedesc:=sTaskTypeDesc, v_dtTaskDueDate:=v_dtTaskDueDate, v_sCustomer:=v_sCustomer, v_sDescription:=v_sDescription, v_sUserGroup:=sUserGroup, v_sUser:=sUser, v_sClientCode:=v_sClientCode), gPMConstants.PMEReturnCode)
            lReturn = m_fMainForm.AddScheduledTaskToList(v_sKey:=sKey, _
                                                         v_iIsUrgent:=v_iIsUrgent, _
                                                         v_sTaskStatusDesc:=sTaskStatusDesc, _
                                                         v_sTaskTypedesc:=sTaskTypeDesc, _
                                                         v_dtTaskDueDate:=v_dtTaskDueDate, _
                                                         v_sCustomer:=v_sCustomer, _
                                                         v_sDescription:=v_sDescription, _
                                                         v_sUserGroup:=sUserGroup, _
                                                         v_sUser:=sUser, _
                                                         v_sClientCode:=v_sClientCode, _
                                                         v_sPartyName:=v_sPartyName)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateScheduledTask
    '
    ' Description: Updates the details for a Scheduled Task in the
    '              List View and Collection.
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    Private Sub UpdateScheduledTask(ByVal v_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_vCustomer As Object = Nothing, Optional ByVal v_vDescription As Object = Nothing, Optional ByVal v_vTaskDueDate As Date = #12/30/1899#, Optional ByVal v_vIsUrgent As Object = Nothing, Optional ByVal v_vTaskStatus As Object = Nothing, Optional ByVal v_vUserGroupID As Object = Nothing, Optional ByVal v_vUserID As Object = Nothing, Optional ByVal v_vNavProcessID As Object = Nothing, Optional ByVal v_vComponentObjectName As Object = Nothing, Optional ByVal v_vComponentClassName As Object = Nothing, Optional ByVal v_vDisplayIcon As Object = Nothing, Optional ByVal v_vIsViewOnlyTask As Object = Nothing, Optional ByVal v_vLinkedObjectName As Object = Nothing, Optional ByVal v_vLinkedClassName As Object = Nothing, Optional ByVal v_vLinkedCaption As Object = Nothing)

        Dim sScheduledTaskKey As String = ""
        Dim oSchedTask As PMWorkManager.ScheduledTask
        Dim sStatusDesc, sUserGroup, sUser As String
        'DAK141299
        Dim iIsVisible As gPMConstants.PMEReturnCode




        ' Generate the Collection Key for this Task Instance
        sScheduledTaskKey = m_oSchedTasks.GenerateKey(v_lScheduledTaskCnt:=v_lPMWrkTaskInstanceCnt)

        ' Get a Reference to the Scheduled Task
        oSchedTask = m_oSchedTasks.Item(sScheduledTaskKey)

        ' Is it loaded
        If oSchedTask Is Nothing Then
            ' No, so exit.
            Exit Sub
        Else
            ' Yes, so lets update it.

            ' UPDATE the Schedule Task with the values supplied
            With oSchedTask

                If Not Information.IsNothing(v_vCustomer) Then
                    .Customer = v_vCustomer
                End If

                If Not Information.IsNothing(v_vDescription) Then
                    .Description = v_vDescription
                End If

                If Not Information.IsNothing(v_vTaskDueDate) Then
                    .TaskDueDate = v_vTaskDueDate
                End If

                If Not Information.IsNothing(v_vIsUrgent) Then
                    .IsUrgent = v_vIsUrgent
                End If

                If Not Information.IsNothing(v_vTaskStatus) Then
                    .TaskStatus = v_vTaskStatus
                End If

                If Not Information.IsNothing(v_vUserGroupID) Then
                    .PmuserGroupID = CInt(v_vUserGroupID)
                    ' Get the User Group Caption
                    sUserGroup = m_fMainForm.cboUserGroup.ItemUserGroupname(v_vUserGroupID)
                    .UserGroup = sUserGroup
                    v_vUserGroupID = sUserGroup
                End If

                If Not Information.IsNothing(v_vUserID) Then
                    .UserID = v_vUserID
                    Dim dbNumericTemp As Double
                    If Double.TryParse(v_vUserID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        If StringsHelper.ToDoubleSafe(v_vUserID) < 1 Then
                            sUser = ""
                        Else
                            sUser = m_fMainForm.cboUser.ItemUsername(v_vUserID)
                        End If
                    Else
                        sUser = ""
                    End If
                    v_vUserID = sUser
                End If

                If Not Information.IsNothing(v_vNavProcessID) Then
                    .PMNavProcessId = v_vNavProcessID
                End If

                If Not Information.IsNothing(v_vComponentObjectName) Then
                    .ComponentObjectName = v_vComponentObjectName
                End If

                If Not Information.IsNothing(v_vComponentClassName) Then
                    .ComponentClassName = v_vComponentClassName
                End If

                If Not Information.IsNothing(v_vDisplayIcon) Then
                    .DisplayIcon = v_vDisplayIcon
                End If

                If Not Information.IsNothing(v_vIsViewOnlyTask) Then
                    .IsViewOnlyTask = v_vIsViewOnlyTask
                End If

                If Not Information.IsNothing(v_vLinkedObjectName) Then
                    .LinkedObjectName = v_vLinkedObjectName
                End If

                If Not Information.IsNothing(v_vLinkedClassName) Then
                    .LinkedClassName = v_vLinkedClassName
                End If

                If Not Information.IsNothing(v_vLinkedCaption) Then
                    .LinkedCaption = v_vLinkedCaption
                End If
                'DAK141299
                iIsVisible = .IsVisible
            End With

            ' Release the reference
            oSchedTask = Nothing

            If iIsVisible = gPMConstants.PMEReturnCode.PMTrue Then
                ' Get the Description for this Status
                sStatusDesc = TaskStatusDescription(v_vTaskStatus)

                If v_vUserGroupID = "" Then
                    'If task has been assigned to user group that this user can't see then remove it.
                    m_fMainForm.DeleteScheduledTask(sScheduledTaskKey)
                Else
                    ' Update the Task Status in the List View
                    m_fMainForm.UpdateScheduledTask(v_sKey:=sScheduledTaskKey, v_vTaskStatusDesc:=sStatusDesc, v_vTaskDueDate:=v_vTaskDueDate, v_vDescription:=v_vDescription, v_vCustomer:=v_vCustomer, v_vUserGroup:=v_vUserGroupID, v_vUser:=v_vUserID, v_IsUrgernt:=v_vIsUrgent)
                End If

            End If

        End If


    End Sub

    ' ***************************************************************** '
    ' Name: DeleteScheduledTask
    '
    ' Description: Deletes the details for a Scheduled Task in the
    '              List View and Collection.
    '
    ' ***************************************************************** '
    Private Sub DeleteScheduledTask(ByVal v_lPMWrkTaskInstanceCnt As Integer)
        Dim sScheduledTaskKey As String = ""
        'DAK141299
        Dim oSchedTask As ScheduledTask
        Dim iIsVisible As gPMConstants.PMEReturnCode




        ' Generate the Collection Key for this Task Instance
        sScheduledTaskKey = m_oSchedTasks.GenerateKey(v_lScheduledTaskCnt:=v_lPMWrkTaskInstanceCnt)

        'DAK141299
        oSchedTask = m_oSchedTasks.Item(sScheduledTaskKey)
        iIsVisible = oSchedTask.IsVisible

        ' Delete the Scheduled Task from the Collection
        m_oSchedTasks.Delete(sScheduledTaskKey)
        ' Delete the Scheduled Task From the List
        'DAK141299
        If iIsVisible = gPMConstants.PMEReturnCode.PMTrue Then
            m_fMainForm.DeleteScheduledTask(v_sKey:=sScheduledTaskKey)
        End If


    End Sub

    ''' <summary>
    ''' Add Batch from datatable to listview
    ''' </summary>
    ''' <param name="r_dtBatchTask"></param>
    ''' <param name="v_bDisplayOnForm"></param>
    ''' <returns></returns>
    Private Function AddBatchTasks(ByRef r_dtBatchTask As DataTable, ByVal v_bDisplayOnForm As Boolean) As Integer

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim iTotalRecordCount, iPassedRecordCount, iFailedRecordCount As Integer
        Dim sBatchStatus As String
        Dim dtStartDateTime, dtEndDateTime As Date
        Dim sProcess, sDescription, sFileName As String

        For lRow As Integer = 0 To r_dtBatchTask.Rows.Count - 1
            Dim drBatchTask As DataRow = r_dtBatchTask.Rows(lRow)

            sProcess = ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskProcessCol))

            sDescription = ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskDescriptionCol))

            If ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskStartDateTimeCol)) Is "" Then
                dtStartDateTime = Nothing
            Else
                dtStartDateTime = CDate(ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskStartDateTimeCol)))
            End If
            If ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskEndDateTimeCol)) Is "" Then
                dtEndDateTime = Nothing
            Else
                dtEndDateTime = CDate(ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskEndDateTimeCol)))
            End If

            sFileName = ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskFileNameCol))

            If ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskTotalRecordsCountCol)) = "" Then
                iTotalRecordCount = Nothing
            Else
                iTotalRecordCount = ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskTotalRecordsCountCol))
            End If

            If ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskPassedRecordsCountCol)) = "" Then
                iPassedRecordCount = Nothing
            Else
                iPassedRecordCount = ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskPassedRecordsCountCol))
            End If

            If ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskFailedRecordsCountCol)) = "" Then
                iFailedRecordCount = Nothing
            Else
                iFailedRecordCount = ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskFailedRecordsCountCol))
            End If

            sBatchStatus = ToSafeString(drBatchTask(gPMConstants.PMEACBatchTaskCol.ACBatchTaskStatusDescCol))


            lReturn = AddBatchTask(sProcess:=sProcess,
                                       sDescription:=sDescription,
                                       dtStartDateTime:=dtStartDateTime,
                                       dtEndDateTime:=dtEndDateTime,
                                       sFileName:=sFileName,
                                       nTotalRecordCount:=iTotalRecordCount,
                                       nPassedRecordCount:=iPassedRecordCount,
                                       nFailedRecordCount:=iFailedRecordCount,
                                       sBatchStatus:=sBatchStatus)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next lRow

        Return lReturn

    End Function
    ' ***************************************************************** '
    ' Name: AddBatchTask
    '
    ' Description: Adds Batch Task to the List View and the Collection
    '
    ' ***************************************************************** '
    Private Function AddBatchTask(ByVal sProcess As String,
                                  ByVal sDescription As String,
                                  ByVal dtStartDateTime As Date,
                                  ByVal dtEndDateTime As Date,
                                  ByVal sFileName As String,
                                  ByVal nTotalRecordCount As Integer,
                                  ByVal nPassedRecordCount As Integer,
                                  ByVal nFailedRecordCount As Integer,
                                  ByVal sBatchStatus As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        result = gPMConstants.PMEReturnCode.PMTrue

        lReturn = m_fMainForm.AddBatchTaskToList(sProcess:=sProcess,
                                                         sDescription:=sDescription,
                                                         dtStartDateTime:=dtStartDateTime,
                                                         dtEndDateTime:=dtEndDateTime,
                                                         sFileName:=sFileName,
                                                         nTotalRecordCount:=nTotalRecordCount,
                                                         nPassedRecordCount:=nPassedRecordCount,
                                                         nFailedRecordCount:=nFailedRecordCount,
                                                         sBatchStatus:=sBatchStatus)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    '///////////////////////////////

    ' ***************************************************************** '
    ' Name: SetTaskMenuOptions
    '
    ' Description: Set the Task Menu options dependent on the currently
    '              selected Scheduled Task.
    ' ***************************************************************** '
    Private Sub SetTaskMenuOptions()

        Dim bNewEnabled, bEditEnabled, bAssignEnabled, bViewEnabled, bStartEnabled, bCompleteEnabled, bIncompleteEnabled, bDeleteEnabled, bTaskLogEnabled As Boolean

        Dim oScheduledTask As ScheduledTask



        bNewEnabled = False
        bEditEnabled = False
        bAssignEnabled = False
        bViewEnabled = False
        bStartEnabled = False
        bCompleteEnabled = False
        bIncompleteEnabled = False
        bDeleteEnabled = False
        bTaskLogEnabled = False

        If ScheduledTaskKey = "" Then
            bNewEnabled = True
        Else
            oScheduledTask = m_oSchedTasks.Item(ScheduledTaskKey)
            If oScheduledTask Is Nothing Then
                bNewEnabled = True
            Else
                CalcTaskMenuOptions(v_oScheduledTask:=oScheduledTask, r_bNewEnabled:=bNewEnabled, r_bEditEnabled:=bEditEnabled, r_bAssignEnabled:=bAssignEnabled, r_bViewEnabled:=bViewEnabled, r_bStartEnabled:=bStartEnabled, r_bCompleteEnabled:=bCompleteEnabled, r_bIncompleteEnabled:=bIncompleteEnabled, r_bDeleteEnabled:=bDeleteEnabled, r_bTaskLogEnabled:=bTaskLogEnabled)
            End If
        End If

        If (Not oScheduledTask Is Nothing) AndAlso m_lSelectedUserGroupId <> oScheduledTask.PmuserGroupID And m_lSelectedUserGroupId <> 0 Then
            m_fMainForm.SetTaskMenuOptions(v_bNewEnabled:=False, v_bEditEnabled:=False, v_bAssignEnabled:=False, v_bViewEnabled:=bViewEnabled, v_bStartEnabled:=False, v_bCompleteEnabled:=False, v_bIncompleteEnabled:=False, v_bDeleteEnabled:=False, v_bTaskLogEnabled:=False)
        Else
            m_fMainForm.SetTaskMenuOptions(v_bNewEnabled:=bNewEnabled, v_bEditEnabled:=bEditEnabled, v_bAssignEnabled:=bAssignEnabled, v_bViewEnabled:=bViewEnabled, v_bStartEnabled:=bStartEnabled, v_bCompleteEnabled:=bCompleteEnabled, v_bIncompleteEnabled:=bIncompleteEnabled, v_bDeleteEnabled:=bDeleteEnabled, v_bTaskLogEnabled:=bTaskLogEnabled)
        End If


    End Sub

    ' ***************************************************************** '
    ' Name: CalcTaskMenuOptions
    '
    ' Description: Set the Task Menu options dependent on the currently
    '              selected Scheduled Task.
    ' ***************************************************************** '
    Private Sub CalcTaskMenuOptions(ByVal v_oScheduledTask As ScheduledTask, ByRef r_bNewEnabled As Boolean, ByRef r_bEditEnabled As Boolean, ByRef r_bAssignEnabled As Boolean, ByRef r_bViewEnabled As Boolean, ByRef r_bStartEnabled As Boolean, ByRef r_bCompleteEnabled As Boolean, ByRef r_bIncompleteEnabled As Boolean, ByRef r_bDeleteEnabled As Boolean, ByRef r_bTaskLogEnabled As Boolean)

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel
        Dim lPMUserGroupID As Integer



        ' Get the User Group ID for this Task
        lPMUserGroupID = v_oScheduledTask.PmuserGroupID

        ' If this User a System Administrator or Normal User
        lReturn = CType(GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel, v_lUserGroupID:=lPMUserGroupID), gPMConstants.PMEReturnCode)


        'You don't want to have to scroll all the way down to the bottom of the list,
        'just so that you can add a new task.
        'Right-Click - New should be available anywhere on the list
        r_bNewEnabled = True

        ' Set the Default Options dependent on the Task Status

        Select Case v_oScheduledTask.TaskStatus
            ' New or Incomplete Tasks
            Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete

                r_bEditEnabled = True
                r_bAssignEnabled = True
                r_bViewEnabled = True
                r_bStartEnabled = True
                r_bTaskLogEnabled = True

                ' If this is a Memo Task then Complete is available
                If v_oScheduledTask.TypeOfTask = gPMConstants.PMEWrkManTaskType.pmeWMTTMemo Then
                    r_bCompleteEnabled = True
                    r_bStartEnabled = True
                End If

                'DJM 11/02/2004 PN9029 : Allow Complete if the task belongs to the user regardless of authority level.
                ' If the User is a Systems Administrator
                ' then Complete is available for ANY Task.
                If lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Or v_oScheduledTask.User.ToUpper() = m_sUsername.ToUpper() Then
                    r_bCompleteEnabled = True
                End If

                ' In Progress
            Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress
                r_bViewEnabled = True

                'DJM 11/02/2004 PN9029 : Allow InComplete if the task belongs to the user regardless of authority level.
                ' If the User is a Systems Administrator
                ' then Incomplete is available for InProgress Tasks.
                ' This is to allow them to reset Tasks which have crashed.
                If lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Or v_oScheduledTask.User.ToUpper() = m_sUsername.ToUpper() Then
                    r_bIncompleteEnabled = True
                End If

                ' Complete
            Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete
                r_bDeleteEnabled = True
                r_bViewEnabled = True

                'DJM 11/02/2004 PN9029 : Allow InComplete if the task belongs to the user regardless of authority level.
                ' If the User is a Systems Administrator
                ' then Incomplete is available for Complete Tasks.
                If lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Or v_oScheduledTask.User.ToUpper() = m_sUsername.ToUpper() Then
                    r_bIncompleteEnabled = True
                End If

        End Select


    End Sub

    ' ***************************************************************** '
    ' Name: StartQuickStartTask
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function StartQuickStartTask(ByVal v_sAvailableTaskKey As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oTask As PMWorkManager.AvailableTask
        Dim sComponent As String = ""
        Dim lPMWrkTaskInstanceCnt As Integer
        Dim dtTaskDueDate As Date
        Dim lUserGroupID As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get a Reference to the AvailableTask
        oTask = m_oAvailableTasks.Item(v_sAvailableTaskKey)
        If oTask Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If a task is a memo task or is linked to an object, we need more
        ' information which is added in the task instance screen.
        With oTask
            If .LinkedCaption <> "" Or .TypeOfTask = gPMConstants.PMEWrkManTaskType.pmeWMTTMemo Then

                lReturn = CType(NewSchedTask(r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_lPMWrkTaskGroupID:=.TaskGroupID, v_lPMWrkTaskID:=.TaskID), gPMConstants.PMEReturnCode)

            Else
                'Otherwise we need to create a task instance without displaying the form


                lReturn = m_oBusiness.GetDefaultUserGroupForTaskGroup(m_iUserID, .TaskGroupID, lUserGroupID)

                dtTaskDueDate = DateTime.Now

                lReturn = m_oBusiness.CreateTaskInstance(v_lPMWrkTaskGroupID:=.TaskGroupID, v_lPMWrkTaskID:=.TaskID, v_sCustomer:="None", v_dtTaskDueDate:=dtTaskDueDate, v_lPmuserGroupID:=lUserGroupID, v_sDescription:=.TaskCaption, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=gPMConstants.PMEReturnCode.PMFalse, r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_iIsVisible:=gPMConstants.PMEReturnCode.PMFalse)


                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    lReturn = CType(AddScheduledTask(v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_sCustomer:="None", v_sDescription:=.TaskCaption, v_dtTaskDueDate:=dtTaskDueDate, v_iIsUrgent:=gPMConstants.PMEReturnCode.PMFalse, v_iTypeOfTask:=.TypeOfTask, v_iIsSystem:=.IsSystemTask, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_lUserGroupID:=lUserGroupID, v_vUserID:=m_iUserID, v_lNavProcessID:=.PMNavProcessId, v_sComponentObjectName:=.ComponentObjectName, v_sComponentClassName:=.ComponentClassName, v_lDisplayIcon:=.DisplayIcon, v_iIsViewOnlyTask:=.IsViewOnlyTask, v_sLinkedObjectName:=.LinkedObjectName, v_sLinkedClassName:=.LinkedClassName, v_sLinkedCaption:=.LinkedCaption, v_iIsVisible:=Not (Convert.IsDBNull(.NavXMLfile) Or IsNothing(.NavXMLfile)), v_bDisplayOnForm:=False, v_sNavXMLfile:=.NavXMLfile, v_sClientCode:=""), gPMConstants.PMEReturnCode)
                End If
            End If
        End With

        oTask = Nothing

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Available Task.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If lPMWrkTaskInstanceCnt = 0 Then
            Return result
        End If

        ' We now Start a Scheduled task
        lReturn = CType(StartScheduledTask(lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Task.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: StartScheduledTask
    '
    ' Description: Starts a Scheduled Task
    '
    '
    ' ***************************************************************** '
    Private Function StartScheduledTask(ByVal v_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_bFromSchedule As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sKey As String = ""
        Dim lPMAuthorityLevel, lPMUserGroupID As Integer
        Dim sComponent As String = ""
        Dim oScheduledTask As PMWorkManager.ScheduledTask
        Dim vSetKeyArray As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        sKey = m_oSchedTasks.GenerateKey(v_lPMWrkTaskInstanceCnt)

        oScheduledTask = m_oSchedTasks.Item(sKey)
        If oScheduledTask Is Nothing Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Start Task - Cannot Find Task In Collection")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_fMainForm.UpdateStatusBar(v_vActivity:=ACStatusActStartingTask)

        ' Start the Task

        lReturn = m_oBusiness.SetStatusInProgress(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
        If lReturn = gPMConstants.PMEReturnCode.PMWarnLicenceExceeded Then
            MessageBox.Show("Licence limit exceeded - Please contact Sirius Support: " & _
                            Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                            "The task will continue", Application.ProductName)
            lReturn = gPMConstants.PMEReturnCode.PMTrue
        End If

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Select Case lReturn
                Case gPMConstants.PMEReturnCode.PMMAlreadyInUse
                    DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Start Task - Task is Already In Progress")
                Case gPMConstants.PMEReturnCode.PMMNoAccess
                    DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Start Task - Task Is Assigned To Someone else.")
                Case gPMConstants.PMEReturnCode.PMInvalidRequest
                    DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Start Task - Task Is Complete.")
                Case gPMConstants.PMEReturnCode.PMBlockLicenceExceeded
                    'DAK071299
                    DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Start Task - Licence Limit Would Be Exceeded." & _
                                        " Please Try Later")
                    'DAK070100
                Case gPMConstants.PMEReturnCode.PMInvalidLicenceKey
                    DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Start Task - Licence Key is Invalid.")
                Case Else
                    DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Start Task.")
            End Select
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the User GroupID for this Task
        lPMUserGroupID = oScheduledTask.PmuserGroupID

        ' Update the Status in the Collection and Form List View
        UpdateScheduledTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_vTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress, v_vUserID:=CStr(UserID), v_vUserGroupID:=CStr(lPMUserGroupID))


        ' Work out the User's Authority Level for this Task
        lReturn = GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel, v_lUserGroupID:=lPMUserGroupID)

        ' Get the Set Keys for this Task Instance

        lReturn = m_oBusiness.GetTaskInstKeys(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_vKeyArray:=vSetKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        With oScheduledTask


            Select Case .TypeOfTask
                Case gPMConstants.PMEWrkManTaskType.pmeWMTTMemo

lReturn = MessageBox.Show("Customer: " & .Customer & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                              Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                              "Description: " & .Description & _
                              Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                              "Has This Task Been Completed?", Application.ProductName, MessageBoxButtons.YesNo)
                    If lReturn = System.Windows.Forms.DialogResult.Yes Then
                        ' Mark as complete
                        lReturn = CompleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    ElseIf lReturn = System.Windows.Forms.DialogResult.No Then
                        ' Mark as incomplete
                        lReturn = IncompleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Else

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                    ' Single Component
                Case gPMConstants.PMEWrkManTaskType.pmeWMTTSingleComponent
                    sComponent = .ComponentObjectName & "." & .ComponentClassName
                    'AR20050428 - PN7388 Pass FromSchedule flag
                    lReturn = m_oInProgTasks.StartComponentTask(v_sComponent:=sComponent, v_lPMAuthorityLevel:=lPMAuthorityLevel, v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_vSetKeyArray:=vSetKeyArray, v_bFromSchedule:=v_bFromSchedule)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Navigator Process
                Case gPMConstants.PMEWrkManTaskType.pmeWMTTNavigatorProcess
                    ' Start the Navigator Process
                    If vSetKeyArray IsNot Nothing AndAlso Information.IsArray(vSetKeyArray) AndAlso vSetKeyArray.GetUpperBound(0) > 0 Then
                        ReDim Preserve vSetKeyArray(vSetKeyArray.GetUpperBound(0), vSetKeyArray.GetUpperBound(1) + 1)
                        vSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vSetKeyArray.GetUpperBound(1)) = "WorkManagerStartupTask"
                        vSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vSetKeyArray.GetUpperBound(1)) = "True"
                    End If

                    lReturn = m_oInProgTasks.StartNavigatorTask(v_lPMNavProcessID:= .PMNavProcessId, v_lPMAuthorityLevel:=lPMAuthorityLevel, v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_vSetKeyArray:=vSetKeyArray, v_sNavXMLfile:= .NavXMLfile)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

            End Select
        End With

        oScheduledTask = Nothing

        m_fMainForm.UpdateStatusBar(v_vActivity:="")

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateSchedTaskStatus
    '
    ' Description: Called when a Navigator Instance or a Single Component
    '              has finished. Updates the Task Status of the
    '              associated Task.
    ' ***************************************************************** '
    Private Function UpdateSchedTaskStatus(ByVal v_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_vComplete As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim sScheduledTaskKey As String = ""

        Dim lPMWrkTaskGroupID, lPMWrkTaskID As Integer
        Dim sCustomer As String = ""
        Dim dtTaskDueDate As Date
        Dim lPMUserGroupID As Integer
        Dim iUserID As Integer
        Dim sDescription As String = ""
        Dim iTaskStatus As gPMConstants.PMEWrkManTaskStatus
        Dim iIsUrgent As Integer
        Dim dtDateCreated As Date
        Dim iCreatedByID As Integer
        Dim dtLastModified As Date
        Dim iModifiedbyID As Integer
        Dim sStatusDesc As String = ""
        'DAK141299
        Dim sSchedTaskKey As String = ""
        Dim oSchedTask As ScheduledTask




        result = gPMConstants.PMEReturnCode.PMTrue


        ' If we do not know what the Status is

        If Information.IsNothing(v_vComplete) Then

            ' Work it out for ourselves.

            ' Refresh From Database
            ' RDC 22032001 drop the vIsVisible parm as this
            ' does not exist on the business function. This function was failing to
            ' release product licences due to this problem!
            'lReturn = m_oBusiness.GetDetails( _
            'v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, _
            'r_lPMWrkTaskGroupID:=lPMWrkTaskGroupID, _
            'r_lPMWrkTaskID:=lPMWrkTaskID, _
            'r_sCustomer:=sCustomer, _
            'r_dtTaskDueDate:=dtTaskDueDate, _
            'r_lPMUserGroupID:=lPMUserGroupID, _
            'r_iUserID:=iUserID, _
            'r_sDescription:=sDescription, _
            'r_iTaskStatus:=iTaskStatus, _
            'r_iIsUrgent:=iIsUrgent, _
            'r_dtDateCreated:=dtDateCreated, _
            'r_iCreatedByID:=iCreatedByID, _
            'r_dtLastModified:=dtLastModified, _
            'r_iModifiedbyID:=iModifiedbyID, _
            'r_vIsVisible:=vIsVisible)

            lReturn = m_oBusiness.GetDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_lPMWrkTaskGroupID:=lPMWrkTaskGroupID, r_lPMWrkTaskID:=lPMWrkTaskID, r_sCustomer:=sCustomer, r_dtTaskDueDate:=dtTaskDueDate, r_lPMUserGroupID:=lPMUserGroupID, r_iUserID:=iUserID, r_sDescription:=sDescription, r_iTaskStatus:=iTaskStatus, r_iIsUrgent:=iIsUrgent, r_dtDateCreated:=dtDateCreated, r_iCreatedByID:=iCreatedByID, r_dtLastModified:=dtLastModified, r_iModifiedByID:=iModifiedbyID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If the Task Status is still 'In Progress',
            ' i.e. The Nav process or Component
            ' has not set it Complete/Incomplete, then set it to Incomplete
            If iTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress Then
                iTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete
            Else
                Return result
            End If

        Else

            ' We know what the Status is
            If v_vComplete Then
                iTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete
            Else
                iTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete
            End If

        End If

        'DAK141299 - invisible tasks need to be deleted right away.
        'DAK141299 - First mark as complete
        ' Generate the Collection Key for this Task Instance
        sScheduledTaskKey = m_oSchedTasks.GenerateKey(v_lScheduledTaskCnt:=v_lPMWrkTaskInstanceCnt)

        ' Get a Reference to the Scheduled Task
        oSchedTask = m_oSchedTasks.Item(sScheduledTaskKey)
        If Not oSchedTask Is Nothing Then
            If oSchedTask.IsVisible = gPMConstants.PMEReturnCode.PMFalse Then
                iTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete
            End If

            If iTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                lReturn = CType(CompleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
            Else
                lReturn = CType(IncompleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
            End If

            'DAK141299
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oSchedTask = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK141299 - now delete the task instance

            If oSchedTask.IsVisible = gPMConstants.PMEReturnCode.PMFalse Then
                lReturn = CType(DeleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
            End If




            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oSchedTask = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSchedTask = Nothing
        Else
            If iTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                lReturn = CType(CompleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
            Else
                lReturn = CType(IncompleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
            End If
        End If
        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CompleteSchedTask
    '
    ' Description: Marks the Task As Completed.
    '
    ' ***************************************************************** '
    Private Function CompleteSchedTask(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sStatusDesc As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sScheduledTaskKey As String = ""
        'DC010404 PN11369 failed if not used
        Dim lPMUserGroupID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Complete the Task

        lReturn = m_oBusiness.SetStatusComplete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Complete Task")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DC010404 PN11369 failed if not used
        lPMUserGroupID = 0

        ' Update the Status in the Collection and Form List View
        UpdateScheduledTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_vTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete, v_vUserGroupID:=CStr(lPMUserGroupID))

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: IncompleteSchedTask
    '
    ' Description: Marks the Task As InCompleted.
    '
    ' ***************************************************************** '
    Private Function IncompleteSchedTask(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        'DC010404 PN11360 -set UserGroup As failed if not there
        Dim lPMUserGroupID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' InComplete the Task

        lReturn = m_oBusiness.SetStatusInComplete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Complete Task")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DC010404 PN11360 -set UserGroup As failed if not there
        lPMUserGroupID = 0

        ' Update the Status in the Collection and Form List View
        UpdateScheduledTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_vTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete, v_vUserGroupID:=CStr(lPMUserGroupID))

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteSchedTask
    '
    ' Description: Deletes the Scheduled Task.
    '
    ' ***************************************************************** '
    Private Function DeleteSchedTask(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim iResponse As DialogResult
        Dim oSchedTask As ScheduledTask
        Dim sScheduledTaskKey As String = ""
        Dim iIsVisible As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        'ISS2277 SJP(CMG) Warn the user if they about to inadvertently delete a completed visible task

        ' start
        ' Generate the Collection Key for this Task Instance
        sScheduledTaskKey = m_oSchedTasks.GenerateKey(v_lScheduledTaskCnt:=v_lPMWrkTaskInstanceCnt)

        oSchedTask = m_oSchedTasks.Item(sScheduledTaskKey)

        iIsVisible = oSchedTask.IsVisible

        oSchedTask = Nothing

        If iIsVisible = gPMConstants.PMEReturnCode.PMTrue Then
            iResponse = MessageBox.Show("Do you wish to delete the selected task?", "Delete Task", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        Else
            iResponse = System.Windows.Forms.DialogResult.Yes
        End If
        ' end

        If iResponse = System.Windows.Forms.DialogResult.Yes Then
            ' Delete the Task

            lReturn = m_oBusiness.Delete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Select Case lReturn
                    Case gPMConstants.PMEReturnCode.PMInvalidRequest
                        DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Delete Task - Task is NOT Complete.")
                    Case Else
                        DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Delete Task.")
                End Select
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Delete the Sched Task.
            DeleteScheduledTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: TaskStatusDescription
    '
    ' Description: Returns the Task Status Description for the Supplied
    '              Task Status.
    '
    ' ***************************************************************** '
    Private Function TaskStatusDescription(ByVal eTaskStatus As gPMConstants.PMEWrkManTaskStatus) As String

        Dim result As String = String.Empty


        result = "Unknown"

        Select Case eTaskStatus
            Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew
                result = ACTaskStatusDescNew
            Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress
                result = ACTaskStatusDescInProgress
            Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete
                result = ACTaskStatusDescInComplete
            Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete
                result = ACTaskStatusDescComplete
        End Select

        Return result


    End Function

    ' ***************************************************************** '
    ' Name: NewSchedTask
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function NewSchedTask(ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_lPMWrkTaskGroupID As Integer = 0, Optional ByVal v_lPMWrkTaskID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the Form
        lReturn = DisplayTaskInstanceForm(v_lAction:=gPMConstants.PMEComponentAction.PMAdd, r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_lPMWrkTaskID:=v_lPMWrkTaskID)
        m_fMainForm_RefreshScheduledTasks(True)
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: EditViewAssignSchedTask
    '
    ' Description: Displays the Task Instance form for a Scheduled Task
    '
    ' ***************************************************************** '
    Private Function EditViewAssignSchedTask(ByVal v_lAction As Integer, ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sCurrentlyLockedBy As String = ""
        Dim lPMWrkTaskInstanceCnt As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        If v_lAction = gPMConstants.PMEComponentAction.PMAdd Then
            Return result
        End If

        ' Lock the Task Instance

        lReturn = m_oBusiness.LockTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_sCurrentlyLockedBy:=sCurrentlyLockedBy)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Scheduled Task is Locked by user " & sCurrentlyLockedBy.Trim())
            Return result
        End If

        'DJM 14/10/2003 : Use backup of PMWrkTaskInstanceCnt as if cancel clicked then
        '                 zero is returned and you can't unlock it.
        lPMWrkTaskInstanceCnt = v_lPMWrkTaskInstanceCnt

        ' Display the Form
        lReturn = CType(DisplayTaskInstanceForm(v_lAction:=v_lAction, r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to display the Task Instance Form.")
            Return result
        End If

        ' Lock the Task Instance

        lReturn = m_oBusiness.UnlockTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to UnLock Scheduled Task. Contact your system Administrator.")
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DisplayTaskInstanceForm
    '
    ' Description: Displays the Task Instance Form in the Mode required.
    ' ***************************************************************** '
    Private Function DisplayTaskInstanceForm(ByVal v_lAction As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_lPMWrkTaskGroupID As Integer = 0, Optional ByVal v_lPMWrkTaskID As Integer = 0) As Integer

        Dim result As Integer = 0

        Dim oTaskInstance As iPMWrkTaskInstance.Interface_Renamed
        Dim lReturn As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create the Component
        Dim temp_oTaskInstance As Object
        lReturn = m_oObjectManager.GetInstance(temp_oTaskInstance, sClassName:="iPMWrkTaskInstance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oTaskInstance = temp_oTaskInstance
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Set Process Modes

        lReturn = oTaskInstance.SetProcessModes(vTask:=v_lAction, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If Add
        If v_lAction = gPMConstants.PMEComponentAction.PMAdd Then
            ' Do we Know what Task Group/Task to create
            If (v_lPMWrkTaskGroupID > 0) And (v_lPMWrkTaskID > 0) Then
                ' Yes, so set the Properties.

                oTaskInstance.PMWrkTaskGroupId = v_lPMWrkTaskGroupID

                oTaskInstance.PMWrkTaskId = v_lPMWrkTaskID
            End If
        Else
            ' Edit, View, ReAssign Mode so set the Key

            oTaskInstance.PMWrkTaskInstanceCnt = r_lPMWrkTaskInstanceCnt
        End If

        ' Start the Form

        lReturn = oTaskInstance.Start
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Task Instance Form:- iPMWrkTaskInstance.Interface")

            oTaskInstance.Dispose()
            oTaskInstance = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If the User Canceled then exit as we do not need
        ' to Refresh the Form details.

        If oTaskInstance.Status = gPMConstants.PMEReturnCode.PMCancel Then
            r_lPMWrkTaskInstanceCnt = 0

            oTaskInstance.Dispose()
            oTaskInstance = Nothing
            Return result
        End If

        ' Refreh the Form/Collection if we need to.
        Select Case v_lAction
            ' View
            Case gPMConstants.PMEComponentAction.PMView
                ' No Refresh Required.

                ' Add
            Case gPMConstants.PMEComponentAction.PMAdd
                With oTaskInstance

                    r_lPMWrkTaskInstanceCnt = .PMWrkTaskInstanceCnt

                    'Modified by Archana Tokas on 6/1/2010 3:07:21 PM duedate was not suppoerted by oTaskInstance as it is case sencitive
                    'lReturn = AddScheduledTask(v_lPMWrkTaskInstanceCnt:=.PMWrkTaskInstanceCnt, v_sCustomer:=.Customer, v_sDescription:=.Description, v_dtTaskDueDate:=.duedate, v_iIsUrgent:=.IsUrgent, v_iTypeOfTask:=.TypeOfTask, v_iIsSystem:=.IsSystemTask, v_iTaskStatus:=.TaskStatus, v_lUserGroupID:=.PmuserGroupID, v_vUserID:=.UserID, v_lNavProcessID:=.PMNavProcessId, v_sComponentObjectName:=.ComponentObjectName, v_sComponentClassName:=.ComponentClassName, v_lDisplayIcon:=.DisplayIcon, v_iIsViewOnlyTask:=.IsViewOnlyTask, v_sLinkedObjectName:=.LinkedObjectName, v_sLinkedClassName:=.LinkedClassName, v_sLinkedCaption:=.LinkedCaption, v_iIsVisible:=gPMConstants.PMEReturnCode.PMTrue, v_bDisplayOnForm:=True, v_sNavXMLfile:=.NavXMLfile, v_sClientCode:="")
                    lReturn = AddScheduledTask(v_lPMWrkTaskInstanceCnt:=.PMWrkTaskInstanceCnt, v_sCustomer:=.Customer, v_sDescription:=.Description, v_dtTaskDueDate:=.DueDate, v_iIsUrgent:=.IsUrgent, v_iTypeOfTask:=.TypeOfTask, v_iIsSystem:=.IsSystemTask, v_iTaskStatus:=.TaskStatus, v_lUserGroupID:=.PMUserGroupID, v_vUserID:=.UserID, v_lNavProcessID:=.PMNavProcessId, v_sComponentObjectName:=.ComponentObjectName, v_sComponentClassName:=.ComponentClassName, v_lDisplayIcon:=.DisplayIcon, v_iIsViewOnlyTask:=.IsViewOnlyTask, v_sLinkedObjectName:=.LinkedObjectName, v_sLinkedClassName:=.LinkedClassName, v_sLinkedCaption:=.LinkedCaption, v_iIsVisible:=gPMConstants.PMEReturnCode.PMTrue, v_bDisplayOnForm:=True, v_sNavXMLfile:=.NavXMLfile, v_sClientCode:="")
                End With

                ' Anything Else i.e. Edit, Assign
            Case Else

                With oTaskInstance

                    'Developer Guide No. 65
                    UpdateScheduledTask(v_lPMWrkTaskInstanceCnt:=.PMWrkTaskInstanceCnt, v_vCustomer:=.Customer, v_vDescription:=.Description, v_vTaskDueDate:=.DueDate, v_vIsUrgent:=.IsUrgent, v_vTaskStatus:=.TaskStatus, v_vUserGroupID:=.PMUserGroupID, v_vUserID:=.UserID, v_vNavProcessID:=.PMNavProcessId, v_vComponentObjectName:=.ComponentObjectName, v_vComponentClassName:=.ComponentClassName, v_vDisplayIcon:=.DisplayIcon, v_vIsViewOnlyTask:=.IsViewOnlyTask, v_vLinkedObjectName:=.LinkedObjectName, v_vLinkedClassName:=.LinkedClassName, v_vLinkedCaption:=.LinkedCaption)
                End With

        End Select


        oTaskInstance.Dispose()
        oTaskInstance = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddViewSchedTaskLog
    '
    ' Description: Displays the Task Instance Log form for a Scheduled Task
    '
    ' ***************************************************************** '
    Private Function AddViewSchedTaskLog(ByVal v_lAction As Integer, ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0

        'Developer Guide No. 88
        Dim oTaskLog As Object
        Dim lReturn As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create the Component
        Dim temp_oTaskLog As Object
        lReturn = m_oObjectManager.GetInstance(temp_oTaskLog, sClassName:="iPMWrkTaskInstLog.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oTaskLog = temp_oTaskLog
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Set Process Modes

        lReturn = oTaskLog.SetProcessModes(vTask:=v_lAction, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Set the key.

        oTaskLog.PMWrkTaskInstanceCnt = v_lPMWrkTaskInstanceCnt

        ' Start the Form

        lReturn = oTaskLog.Start
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Task Log Form:- iPMWrkTaskInstLog.Interface")

            oTaskLog.Dispose()
            oTaskLog = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        oTaskLog.Dispose()
        oTaskLog = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadQSBarTasks
    '
    ' Description: Load the Quick Start Bar Buttons from the Database
    '
    ' ***************************************************************** '

    'Private Function LoadQSBarTasks() As Integer
    'Dim result As Integer = 0
    'Dim lReturn As gPMConstants.PMEReturnCode
    'Dim vButtonArray As Object
    'Dim lTaskGroupID, lTaskID As Integer
    'Dim sTaskKey As String = ""
    'Dim bDirty As Boolean
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Load the Quick Start Buttons

    'lReturn = m_oBusiness.GetQuickStartTasks(r_vQuickStartArray:=vButtonArray)
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' If there are no Buttons then exit.
    'If Not Information.IsArray(vButtonArray) Then
    'Return result
    'End If
    '
    'bDirty = False
    '
    ' For Each Button

    'For 'lRow As Integer = vButtonArray.GetLowerBound(1) To vButtonArray.GetUpperBound(1)
    ' Get the Task Group ID

    'lTaskGroupID = CInt(vButtonArray(gPMConstants.PMEACQuickStartCol.ACQSTaskGroupIDCol, lRow))
    ' Get the Task ID

    'lTaskID = CInt(vButtonArray(gPMConstants.PMEACQuickStartCol.ACQSTaskIDCol, lRow))
    ' Generate the Task Key
    'sTaskKey = m_oAvailableTasks.GenerateKey(v_lTaskGroupID:=lTaskGroupID, v_lTaskID:=lTaskID)
    ' Add it to the QS Bar.
    'lReturn = CType(AddTaskToQSBar(sTaskKey), gPMConstants.PMEReturnCode)
    ' If we could not add the Task as it is no longer available.
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'If Not bDirty Then
    'MessageBox.Show(ACQuickStartErroMsg, Application.ProductName)
    'bDirty = True
    'End If
    'End If
    'Next lRow
    '
    ' Indicate whether the QS Bar Tasks need to be saved.
    'QSBarDirty = bDirty
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadQSBarTasksFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadQSBarTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' DAK130999
    ' Name: LoadFavourites
    '
    ' Description: Load the Quick Start Bar Buttons from the Database
    '
    ' ***************************************************************** '
    Private Function LoadFavourites() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vButtonArray(,) As Object
        Dim lTaskGroupID, lTaskID As Integer
        Dim sTaskKey As String = ""
        Dim bDirty As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the Favourites Buttons

        lReturn = m_oBusiness.GetQuickStartTasks(r_vQuickStartArray:=vButtonArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If there are no Buttons then exit.
        If Not Information.IsArray(vButtonArray) Then
            Return result
        End If

        bDirty = False

        ' For Each Button

        For lRow As Integer = vButtonArray.GetLowerBound(1) To vButtonArray.GetUpperBound(1)
            ' Get the Task Group ID

            lTaskGroupID = gPMFunctions.ToSafeInteger(vButtonArray(gPMConstants.PMEACQuickStartCol.ACQSTaskGroupIDCol, lRow))
            ' Get the Task ID

            lTaskID = gPMFunctions.ToSafeInteger(vButtonArray(gPMConstants.PMEACQuickStartCol.ACQSTaskIDCol, lRow))
            ' Generate the Task Key
            sTaskKey = m_oAvailableTasks.GenerateKey(v_lTaskGroupID:=lTaskGroupID, v_lTaskID:=lTaskID)
            bLoadFavorite = True
            ' Add it to the Favourites Group.
            lReturn = CType(AddTaskToFavourites(sTaskKey, lRow + 1), gPMConstants.PMEReturnCode)
            bLoadFavorite = False
            ' If we could not add the Task as it is no longer available.
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If Not bDirty Then
                    MessageBox.Show(ACFavouritesErrorMsg, Application.ProductName)
                    bDirty = True
                End If
            End If
        Next lRow

        ' Indicate whether the Favourites Group Tasks need to be saved.
        FavouritesChanged = bDirty

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SaveQSBarTasks
    '
    ' Description: Saves the QS Bar Tasks in the Database.
    '
    '
    ' ***************************************************************** '

    'Private Function SaveQSBarTasks() As Integer
    '
    'Dim result As Integer = 0
    'Dim lReturn As gPMConstants.PMEReturnCode
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' If the QS Bar does not need saving then exit.
    'If Not QSBarDirty Then
    'Return result
    'End If
    '
    ' Delete Existing Quick Start Tasks

    'lReturn = m_oBusiness.DeleteQuickStartTasks
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Delete Existing Quick Start Tasks.")
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' For Each Available Task
    'For	Each oTask As PMWorkManager.AvailableTask In m_oAvailableTasks
    'With oTask
    ' If its on the QS Bar
    'If .QuickStartBarPos > 0 Then
    ' Add it to the Database

    'lReturn = m_oBusiness.AddQuickStartTask(v_lPMWrkTaskGroupID:=.TaskGroupID, v_lPMWrkTaskID:=.TaskID, v_ldisplaysequencenum:=.QuickStartBarPos)
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Add Quick Start Tasks.")
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    'End With
    'Next oTask
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveQSBarTasksFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveQSBarTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' DAK130999
    ' Name: SaveFavouritesTasks
    '
    ' Description: Saves the Favourites Tasks in the Database.
    '
    '
    ' ***************************************************************** '
    Private Function SaveFavouritesTasks(Optional ByVal v_sAvailableTaskKey As String = "",
                                      Optional ByVal v_iIndex As Integer = 0,
                                      Optional ByVal sFlag As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sScheduledTaskKey As String = ""
        result = gPMConstants.PMEReturnCode.PMTrue

        ' If the Favourites Group does not need saving then exit.
        If Not FavouritesChanged Then
            Return result
        End If

        ' If the flag is False, delete the existing tasks
        If Not sFlag Then
            ' Delete Existing Quick Start Tasks
            For Each oTask As PMWorkManager.AvailableTask In m_oAvailableTasks
                With oTask
                    sScheduledTaskKey = GenerateKey(.TaskGroupID, .TaskID)
                    ' If the task matches the available task key, delete it
                    If (v_sAvailableTaskKey = sScheduledTaskKey) Then
                        ' Call the DeleteQuickStartTask function to delete the existing task
                        lReturn = m_oBusiness.DeleteSingleQuickStartTask(.TaskGroupID, .TaskID, m_iUserID)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Delete Existing Favourites Tasks.")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End With
            Next oTask
        Else
            ' If the flag is True, add new tasks
            ' For Each Available Task, add the new Quick Start Tasks
            For Each oTask As PMWorkManager.AvailableTask In m_oAvailableTasks
                With oTask
                    sScheduledTaskKey = GenerateKey(.TaskGroupID, .TaskID)
                    ' If it's on the QS Bar (based on the key)
                    If (v_sAvailableTaskKey = sScheduledTaskKey) Then
                        ' Add it to the Database
                        lReturn = m_oBusiness.AddQuickStartTask(v_lPMWrkTaskGroupID:= .TaskGroupID, v_lPMWrkTaskID:= .TaskID, v_lDisplaySequenceNum:= .FavouritesIndex)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Add Favourites Tasks.")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End With
            Next oTask
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GenerateKey
    '
    ' Description:
    ' ***************************************************************** '
    Public Function GenerateKey(ByVal v_lTaskGroupID As Integer, ByVal v_lTaskID As Integer) As String

        Dim result As String = String.Empty
        Dim sKey As String = ""

        Try
            ' Derive the Key
            sKey = (ACTaskGroupPrefix & Conversion.Str(v_lTaskGroupID).Trim()).Trim()
            sKey = sKey & (ACTaskPrefix & Conversion.Str(v_lTaskID).Trim()).Trim()

            ' Return the Key
            Return sKey
        Catch excep As System.Exception
            ' Error.
            result = ""

            ' Log Error Message
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GenerateKey", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateKey", vErrNo:=gPMFunctions.ToSafeString(Information.Err().Number), vErrDesc:=excep.Message)
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddTaskToQSBar
    '
    ' Description:
    ' ***************************************************************** '
    Private Function AddTaskToQSBar(ByVal v_sAvailableTaskKey As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oAvailableTask As PMWorkManager.AvailableTask
        Static lButtonPos As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get a Reference to the Available Task using the Key
        oAvailableTask = m_oAvailableTasks.Item(v_sAvailableTaskKey)

        If oAvailableTask Is Nothing Then
            Return result
        End If

        With oAvailableTask

            ' If the Task is already on the Quick Start Bar then exit.
            If .QuickStartBarPos > 0 Then
                Return result
            End If

            lReturn = CType(m_fMainForm.AddQuickStartButton(v_sKey:=v_sAvailableTaskKey, v_sCaption:=.TaskCaption, v_sToolTipText:=.TaskCaption, v_lDisplayIcon:=.DisplayIcon), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            lButtonPos += 1

            ' Update the Available Task with its Position
            .QuickStartBarPos = lButtonPos

            ' Indicate that the QS Bar has been amended.
            QSBarDirty = True

        End With

        oAvailableTask = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' DAK130999
    ' Name: AddTaskToFavourites
    '
    ' Description:
    ' ***************************************************************** '
    Private Function AddTaskToFavourites(ByVal v_sAvailableTaskKey As String, ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oAvailableTask As PMWorkManager.AvailableTask



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get a Reference to the Available Task using the Key
        oAvailableTask = m_oAvailableTasks.Item(v_sAvailableTaskKey)

        If oAvailableTask Is Nothing Then
            Return result
        End If

        With oAvailableTask

            ' If the task already exists in the favourites
            ' group there is no need to add it
            If .FavouritesIndex <> 0 Then
                Return result
            End If
            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.3.2)
            'Added by Gaurav
            'm_fMainForm.FavouritesCaption = m_sFavouritesCaption
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.2.3.2)
            lReturn = CType(m_fMainForm.AddToFavourites(v_sKey:=v_sAvailableTaskKey, v_sCaption:= .TaskCaption, r_iIndex:=v_iIndex, v_lDisplayIcon:= .DisplayIcon), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Update the Available Task with its Position
            .FavouritesIndex = v_iIndex

            ' Indicate that the Favourites Group has been amended.
            FavouritesChanged = True

        End With
        If Not bLoadFavorite Then
            SaveFavouritesTasks(v_sAvailableTaskKey, v_iIndex, True)
        End If
        oAvailableTask = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' DAK130999
    ' Name: MoveFavouriteTask
    '
    ' Description:
    ' ***************************************************************** '
    Private Function MoveFavouriteTask(ByVal v_sAvailableTaskKey As String, ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oAvailableTask As PMWorkManager.AvailableTask



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get a Reference to the Available Task using the Key
        oAvailableTask = m_oAvailableTasks.Item(v_sAvailableTaskKey)

        If oAvailableTask Is Nothing Then
            Return result
        End If

        With oAvailableTask

            If .FavouritesIndex = v_iIndex Then
                Return result
            End If

        End With

        lReturn = CType(RemoveTaskFromFavourites(v_sAvailableTaskKey, v_iIndex), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        lReturn = CType(AddTaskToFavourites(v_sAvailableTaskKey, v_iIndex), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        oAvailableTask = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: RemoveTaskFromQSBar
    '
    ' Description:
    ' ***************************************************************** '
    Private Function RemoveTaskFromQSBar(ByVal v_sAvailableTaskKey As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oAvailableTask As PMWorkManager.AvailableTask



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get a Reference to the Available Task using the Key
        oAvailableTask = m_oAvailableTasks.Item(v_sAvailableTaskKey)

        If oAvailableTask Is Nothing Then
            Return result
        End If

        With oAvailableTask

            ' If the Task is Not on the Quick Start Bar then exit.
            If .QuickStartBarPos < 1 Then
                Return result
            End If

            lReturn = CType(m_fMainForm.RemoveQuickStartButton(v_sKey:=v_sAvailableTaskKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Update the Availabl Task with a Button Position of Zero,
            ' to denote that it is not on the Quick Start Bar.
            .QuickStartBarPos = 0

            ' Indicate that the QS Bar has been amended.
            QSBarDirty = True

        End With

        oAvailableTask = Nothing


        Return result

    End Function

    ' ***************************************************************** '
    ' DAK130999
    ' Name: RemoveTaskFromFavourites
    '
    ' Description:
    ' ***************************************************************** '
    Private Function RemoveTaskFromFavourites(ByVal v_sAvailableTaskKey As String, ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oAvailableTask As PMWorkManager.AvailableTask

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get a Reference to the Available Task using the Key
        oAvailableTask = m_oAvailableTasks.Item(v_sAvailableTaskKey)

        If oAvailableTask Is Nothing Then
            Return result
        End If

        With oAvailableTask

            ' If the Task is Not in the Favourites Group then exit.
            If .FavouritesIndex < 1 Then
                Return result
            End If

            ' Update the Available Task with a Favourites index of Zero,
            ' to denote that it is not in the Group.
            .FavouritesIndex = 0

            ' Remove the Task from the Favourites
            lReturn = CType(m_fMainForm.RemoveFromFavourites(v_sKey:=v_sAvailableTaskKey, v_sCaption:= .TaskCaption, r_iIndex:=v_iIndex, v_lDisplayIcon:= .DisplayIcon), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Indicate that the Favourites Group has been amended.
            FavouritesChanged = True

            ' Save the change (like in the AddTaskToFavourites function)
            If Not bLoadFavorite Then
                SaveFavouritesTasks(v_sAvailableTaskKey, v_iIndex, False) ' Here we pass 0 to signify removal
            End If

        End With

        oAvailableTask = Nothing

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CheckForDueSystemTasks
    '
    ' Description: Checks the Scheduled Task collection for System
    '              Tasks which are due and Starts them.
    '
    ' ***************************************************************** '
    Private Function CheckForDueSystemTasks() As Integer

        Dim result As Integer = 0
        Dim dtDueDate As Date
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Are we due to check for System Tasks
        If m_dtNextCheckSystemTasks <= DateTime.Now Then

            ' Yes.
            dtDueDate = DateTime.Now
            ' Check for System Tasks again in x minutes.
            m_dtNextCheckSystemTasks = DateTime.Now.AddMinutes(ACSystemTasksCheckEveryMins)

        Else

            ' No, so Exit.
            Return result

        End If

        m_fMainForm.UpdateStatusBar(v_vActivity:=ACStatusActCheckingForSystem)

        ' For Each Scheduled Task
        For Each oSchedTask As PMWorkManager.ScheduledTask In m_oSchedTasks
            With oSchedTask

                ' If it is a Scheduled System Task
                If (.IsSystemTask = gPMConstants.PMEReturnCode.PMTrue) And (.TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew) Then

                    ' Is the Task Due
                    If .TaskDueDate <= dtDueDate Then

                        ' Yes, so start it
                        lReturn = CType(StartScheduledTask(v_lPMWrkTaskInstanceCnt:=.PMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return lReturn
                        End If

                        m_fMainForm.UpdateStatusBar(v_vActivity:=ACStatusActCheckingForSystem)

                    Else

                        ' No, it is NOT Due

                        ' Is the Task Due before our Next Check
                        If .TaskDueDate < m_dtNextCheckSystemTasks Then
                            ' Yes, so Next Check equals the Due Date
                            m_dtNextCheckSystemTasks = .TaskDueDate
                        End If

                    End If

                End If
            End With
        Next oSchedTask

        m_fMainForm.UpdateStatusBar(v_vActivity:="")

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CloseWorkManager
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function CloseWorkManager() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Save the Users Quick Start Tasks
        'DAK130999 - Replaced by Favourites group on Sheridan
        '            Active List bar.
        'lReturn = SaveQSBarTasks()
        lReturn = CType(SaveFavouritesTasks(), gPMConstants.PMEReturnCode)

        ' Save the Registry Settings for View Quick Start & Available Tasks
        SetRegistrySettings()

        ' Terminate the Control Class.
        Dispose()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DisplayErrorMessage
    '
    ' Description: Displays an error message on the Status Bar.
    ' ***************************************************************** '
    Private Sub DisplayErrorMessage(ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing)

        If m_fMainForm IsNot Nothing Then
            m_fMainForm.UpdateStatusBar(v_vErrorMsg:=sMsg)
        End If
        If iType <> gPMConstants.PMELogLevel.PMLogInfo Then
            gPMFunctions.LogMessageToFile(sUsername:=Username, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod))
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: SplashScreen
    '
    ' Description: Displays/Hides the splash screen.
    '
    ' ***************************************************************** '
    Private Sub SplashScreen(ByRef bDisplay As Boolean)

        Static oSplash As iPMSplash.Interface_Renamed
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sVersion, sVersionLabel, sRelease, sSiriusType As String



        ' Check if need to display the splash.
        If bDisplay Then
            ' Display the splash screen.

            ' Get an instance of the splash object.
            If oSplash Is Nothing Then

                ' Get an instance of the object.
                oSplash = New iPMSplash.Interface_Renamed()
                'Developer Guide No. 9
                lReturn = oSplash.Initialise()

                ' RDC 20082002 get the Sirius version
                'tarun modified below line
                'lReturn = CType(gPMFunctions.GetSiriusVersion(sVersion, sRelease, sSiriusType), gPMConstants.PMEReturnCode)
                lReturn = gPMFunctions.GetSiriusVersion(sVersion, sRelease, sSiriusType)

                oSplash.CallingAppName = ACApp

                'Do Not Show Build Numbers.
                If sRelease.StartsWith("0.") Then
                    sVersionLabel = sVersion
                Else
                    If sRelease.IndexOf("."c) >= 0 Then
                        sVersionLabel = sVersion & ", SR" & sRelease.Substring(0, sRelease.IndexOf("."c))
                    Else
                        sVersionLabel = sVersion & ", SR" & sRelease
                    End If
                End If

                If sSiriusType = "" Then
                    oSplash.TitleName = "Sirius Architecture v" & sVersionLabel
                Else
                    oSplash.TitleName = "SSP Pure " & sSiriusType & " v" & sVersionLabel
                End If

                lReturn = oSplash.Start()

            End If

        Else
            ' Destroy the splash screen.

            If Not (oSplash Is Nothing) Then

                lReturn = oSplash.Finish()
                oSplash.Dispose()
                oSplash = Nothing

            End If

        End If


    End Sub

    ' ***************************************************************** '
    ' Name: GetRegistrySettings
    '
    ' Description:
    ' ***************************************************************** '
    Private Sub GetRegistrySettings()
        Dim sViewSplash, sViewQuickStart, sViewAvailableTasks As String
        'DAK231299
        Dim sViewToolbar, sViewStatusBar, sViewGridLines, sViewGraphics As String
        'DAK110100
        Dim sIsAutoRefresh, sRefreshRate As String
        Dim lReturn As gPMConstants.PMEReturnCode
        'DAK110700
        Dim sFormCaption, sPMSupportWebAddress As String
        ' RDC 22112000
        Dim sColumnWidths As String = ""
        ' RDC 17072002
        Dim sComboSettings As String = ""



        ' Get the PM News Home Page
        ' NOTE: This comes from the Registry on the SERVER.

        m_fMainForm.PMNewsWebAddress = m_oBusiness.GetPMNewsWebAddress
        'DAK190600 - Get web tab caption from Server too.

        m_fMainForm.WebTabCaption = m_oBusiness.GetWebTabCaption
        'DAK110700 - Get Form caption from Server.

        ' TODO()
        lReturn = m_oServerRegistry.GetServerRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.ACWrkManRegFormCaption, r_sSettingValue:=sFormCaption, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sFormCaption = ""
        End If

        m_fMainForm.FormCaption = sFormCaption

        'DAK110700 - Get PM Support Web Address from Server.

        lReturn = m_oServerRegistry.GetServerRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.ACWrkManRegSupportWebAddress, r_sSettingValue:=sPMSupportWebAddress, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sPMSupportWebAddress = ""
        End If

        m_fMainForm.PMSupportWebAddress = sPMSupportWebAddress

        ' Get the View Splash Setting from the Registry
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewSplash, r_sSettingValue:=sViewSplash, v_sSubKey:=gPMConstants.ACWrkManRegSubKey), gPMConstants.PMEReturnCode)

        ' Is there a setting
        Dim dbNumericTemp As Double
        If sViewSplash.Trim() = "" Then
            ' No, so display it
            m_fMainForm.ViewSplash = True
            ' Is it Numeric
        ElseIf (Double.TryParse(sViewSplash, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
            ' Is it True or False
            If CInt(sViewSplash) = gPMConstants.PMEReturnCode.PMTrue Then
                ' True, so display
                m_fMainForm.ViewSplash = True
            Else
                ' False, so do NOT display
                m_fMainForm.ViewSplash = False
            End If
        Else
            ' Not Numeric so display it
            m_fMainForm.ViewSplash = True
        End If

        ' Get the View Quick Start setting
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewQuickStart, r_sSettingValue:=sViewQuickStart, v_sSubKey:=gPMConstants.ACWrkManRegSubKey), gPMConstants.PMEReturnCode)

        ' Is there a setting
        Dim dbNumericTemp2 As Double
        If sViewQuickStart.Trim() = "" Then
            ' No, so display it
            m_fMainForm.ViewQuickStart = True
            ' Is it Numeric
        ElseIf (Double.TryParse(sViewQuickStart, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
            ' Is it True or False
            If CInt(sViewQuickStart) = gPMConstants.PMEReturnCode.PMTrue Then
                ' True, so display
                m_fMainForm.ViewQuickStart = True
            Else
                ' False, so do NOT display
                m_fMainForm.ViewQuickStart = False
            End If
        Else
            ' Not Numeric so display it
            m_fMainForm.ViewQuickStart = True
        End If

        ' Get the View Available Tasks Setting
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewAvailableTasks, r_sSettingValue:=sViewAvailableTasks, v_sSubKey:=gPMConstants.ACWrkManRegSubKey), gPMConstants.PMEReturnCode)

        ' Is there a setting
        Dim dbNumericTemp3 As Double
        If sViewAvailableTasks.Trim() = "" Then
            ' No, so display it
            m_fMainForm.ViewAvailableTasks = True
            ' Is it Numeric
        ElseIf (Double.TryParse(sViewAvailableTasks, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Then
            ' Is it True or False
            If CInt(sViewAvailableTasks) = gPMConstants.PMEReturnCode.PMTrue Then
                ' True, so display
                m_fMainForm.ViewAvailableTasks = True
            Else
                ' False, so do NOT display
                m_fMainForm.ViewAvailableTasks = False
            End If
        Else
            ' Not Numeric so display it
            m_fMainForm.ViewAvailableTasks = True
        End If

        'DAK231299
        ' Get the View Toolbar setting
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewToolbar, r_sSettingValue:=sViewToolbar, v_sSubKey:=gPMConstants.ACWrkManRegSubKey), gPMConstants.PMEReturnCode)

        ' Is there a setting
        Dim dbNumericTemp4 As Double
        If sViewToolbar.Trim() = "" Then
            ' No, so display it
            m_fMainForm.ViewToolbar = True
            ' Is it Numeric
        ElseIf (Double.TryParse(sViewToolbar, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4)) Then
            ' Is it True or False
            If CInt(sViewToolbar) = gPMConstants.PMEReturnCode.PMTrue Then
                ' True, so display
                m_fMainForm.ViewToolbar = True
            Else
                ' False, so do NOT display
                m_fMainForm.ViewToolbar = False
            End If
        Else
            ' Not Numeric so display it
            m_fMainForm.ViewToolbar = True
        End If

        ' Get the View Status Bar setting
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewStatusBar, r_sSettingValue:=sViewStatusBar, v_sSubKey:=gPMConstants.ACWrkManRegSubKey), gPMConstants.PMEReturnCode)

        ' Is there a setting
        Dim dbNumericTemp5 As Double
        If sViewStatusBar.Trim() = "" Then
            ' No, so display it
            m_fMainForm.ViewStatusBar = True
            ' Is it Numeric
        ElseIf (Double.TryParse(sViewStatusBar, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5)) Then
            ' Is it True or False
            If CInt(sViewStatusBar) = gPMConstants.PMEReturnCode.PMTrue Then
                ' True, so display
                m_fMainForm.ViewStatusBar = True
            Else
                ' False, so do NOT display
                m_fMainForm.ViewStatusBar = False
            End If
        Else
            ' Not Numeric so display it
            m_fMainForm.ViewStatusBar = True
        End If

        ' Get the View Grid Lines setting
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewGridLines, r_sSettingValue:=sViewGridLines, v_sSubKey:=gPMConstants.ACWrkManRegSubKey), gPMConstants.PMEReturnCode)

        ' Is there a setting
        Dim dbNumericTemp6 As Double
        If sViewGridLines.Trim() = "" Then
            ' No, so display it
            m_fMainForm.ViewGridLines = True
            ' Is it Numeric
        ElseIf (Double.TryParse(sViewGridLines, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6)) Then
            ' Is it True or False
            If CInt(sViewGridLines) = gPMConstants.PMEReturnCode.PMTrue Then
                ' True, so display
                m_fMainForm.ViewGridLines = True
            Else
                ' False, so do NOT display
                m_fMainForm.ViewGridLines = False
            End If
        Else
            ' Not Numeric so display it
            m_fMainForm.ViewGridLines = True
        End If

        ' Get the View Graphics setting
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACRegKeyViewGraphics, r_sSettingValue:=sViewGraphics), gPMConstants.PMEReturnCode)

        ' Is there a setting
        Dim dbNumericTemp7 As Double
        If sViewGraphics.Trim() = "" Then
            ' No, so display it
            m_fMainForm.ViewGraphics = True
            ' Is it Numeric
        ElseIf (Double.TryParse(sViewGraphics, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7)) Then
            ' Is it True or False
            If CInt(sViewGraphics) = gPMConstants.PMEReturnCode.PMTrue Then
                ' True, so display
                m_fMainForm.ViewGraphics = True
            Else
                ' False, so do NOT display
                m_fMainForm.ViewGraphics = False
            End If
        Else
            ' Not Numeric so display it
            m_fMainForm.ViewGraphics = True
        End If

        ' Splash screen is a graphic. Therefore if ViewGraphics is false...
        If Not m_fMainForm.ViewGraphics Then
            m_fMainForm.ViewSplash = False
        End If

        ' Get the Auto Refresh setting
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegIsAutoRefresh, r_sSettingValue:=sIsAutoRefresh, v_sSubKey:=gPMConstants.ACWrkManRegSubKey), gPMConstants.PMEReturnCode)

        ' Is there a setting
        Dim dbNumericTemp8 As Double
        If sIsAutoRefresh.Trim() = "" Then
            ' No, so display it
            m_fMainForm.IsAutoRefresh = True
            ' Is it Numeric
        ElseIf (Double.TryParse(sIsAutoRefresh, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8)) Then
            ' Is it True or False
            If CInt(sIsAutoRefresh) = gPMConstants.PMEReturnCode.PMTrue Then
                ' True, so display
                m_fMainForm.IsAutoRefresh = True
            Else
                ' False, so do NOT display
                m_fMainForm.IsAutoRefresh = False
            End If
        Else
            ' Not Numeric so display it
            m_fMainForm.IsAutoRefresh = True
        End If

        ' Get the Refresh Rate setting
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegRefreshRate, r_sSettingValue:=sRefreshRate, v_sSubKey:=gPMConstants.ACWrkManRegSubKey), gPMConstants.PMEReturnCode)

        ' Is there a setting
        Dim dbNumericTemp9 As Double
        If sRefreshRate.Trim() = "" Then
            ' No, so default it
            m_fMainForm.RefreshRate = 1
            ' Is it Numeric
        ElseIf (Double.TryParse(sRefreshRate, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9)) Then
            'It should be between 1 and 60
            If CInt(sRefreshRate) < 1 Then
                m_fMainForm.RefreshRate = 1
            ElseIf CInt(sRefreshRate) > 60 Then
                m_fMainForm.RefreshRate = 60
            Else
                m_fMainForm.RefreshRate = CInt(sRefreshRate)
            End If
        Else
            ' Not Numeric so default it
            m_fMainForm.RefreshRate = 1
        End If

        ' RDC 22112000 new setting for listview column widths
        ' Get the column width setting
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACWrkManRegColumnWidths, r_sSettingValue:=sColumnWidths, v_sSubKey:=gPMConstants.ACWrkManRegSubKey), gPMConstants.PMEReturnCode)

        ' if no setting, will be created when WM exits, so
        ' set up default widths (from listview column width properties).
        If sColumnWidths = "" Then
            'sColumnWidths = "400;800;600;1440;1440;1600;1440;600;0;"
            'sColumnWidths = "80;100;80;100;144;160;140;60;0;"
            sColumnWidths = "40;80;60;100;100;100;100;60;0;"
        End If

        m_fMainForm.ColumnWidths = sColumnWidths

        ' RDC 17072002 combobox settings
        lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACWrkManRegComboSettings, r_sSettingValue:=sComboSettings, v_sSubKey:=gPMConstants.ACWrkManRegSubKey), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sComboSettings = ""
        End If

        m_fMainForm.ComboSettings = sComboSettings

        ' RDC 23032001 Splash screen application title
        ' may be 'Work Manager' or 'Sirius', depending on customer

        lReturn = m_oServerRegistry.GetServerRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=ACWrkManRegSplashAppTitle, r_sSettingValue:=m_sSplashAppTitle, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        If m_sSplashAppTitle = "" Then
            m_sSplashAppTitle = "Work Manager"
        End If


    End Sub

    ' ***************************************************************** '
    ' Name: SetRegistrySettings
    '
    ' Description:
    ' ***************************************************************** '
    Private Sub SetRegistrySettings()
        Dim sViewSplash, sViewQuickStart, sViewAvailableTasks As String
        Dim lReturn As Integer
        'DAK241299
        Dim sViewToolbar, sViewStatusBar, sViewGridLines As String
        'Dim sViewGraphics As String
        'DAK110100
        Dim sIsAutoRefresh, sRefreshRate As String
        ' RDC 22112000
        Dim sColumnWidths As String = ""
        ' RDC 17072002
        Dim sComboSettings As String = ""



        ' NOTE: PMNews Web Adress is amended at the time it is changed.

        ' Set the View Splash Setting from the Registry
        If m_fMainForm.ViewSplash Then
            sViewSplash = CStr(gPMConstants.PMEReturnCode.PMTrue)
        Else
            sViewSplash = CStr(gPMConstants.PMEReturnCode.PMFalse)
        End If

        lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewSplash, v_sSettingValue:=sViewSplash, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)


        ' Set the View Quick Start setting
        If m_fMainForm.ViewQuickStart Then
            sViewQuickStart = CStr(gPMConstants.PMEReturnCode.PMTrue)
        Else
            sViewQuickStart = CStr(gPMConstants.PMEReturnCode.PMFalse)
        End If

        lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewQuickStart, v_sSettingValue:=sViewQuickStart, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        ' Set the View Available Tasks Setting
        If m_fMainForm.ViewAvailableTasks Then
            sViewAvailableTasks = CStr(gPMConstants.PMEReturnCode.PMTrue)
        Else
            sViewAvailableTasks = CStr(gPMConstants.PMEReturnCode.PMFalse)
        End If

        lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewAvailableTasks, v_sSettingValue:=sViewAvailableTasks, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        'DAK241299 - more registry settings
        ' Set the View Toolbar Setting
        If m_fMainForm.ViewToolbar Then
            sViewToolbar = CStr(gPMConstants.PMEReturnCode.PMTrue)
        Else
            sViewToolbar = CStr(gPMConstants.PMEReturnCode.PMFalse)
        End If

        lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewToolbar, v_sSettingValue:=sViewToolbar, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        ' Set the View Status Bar Setting
        If m_fMainForm.ViewStatusBar Then
            sViewStatusBar = CStr(gPMConstants.PMEReturnCode.PMTrue)
        Else
            sViewStatusBar = CStr(gPMConstants.PMEReturnCode.PMFalse)
        End If

        lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewStatusBar, v_sSettingValue:=sViewStatusBar, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        ' Set the View Grid Lines Setting
        If m_fMainForm.ViewGridLines Then
            sViewGridLines = CStr(gPMConstants.PMEReturnCode.PMTrue)
        Else
            sViewGridLines = CStr(gPMConstants.PMEReturnCode.PMFalse)
        End If

        lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewGridLines, v_sSettingValue:=sViewGridLines, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        ' Set the View Graphics Setting
        '    If (m_fMainForm.ViewGraphics = True) Then
        '        sViewGraphics = PMTrue
        '    Else
        '        sViewGraphics = PMFalse
        '    End If

        '    lReturn& = SetPMRegSetting( _
        'v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
        'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        'v_lPMERegSettingLevel:=pmeRSLClient, _
        'v_sSettingName:=ACRegKeyViewGraphics, _
        'v_sSettingValue:=sViewGraphics)

        ' Set the Is Auto Refresh Setting
        If m_fMainForm.IsAutoRefresh Then
            sIsAutoRefresh = CStr(gPMConstants.PMEReturnCode.PMTrue)
        Else
            sIsAutoRefresh = CStr(gPMConstants.PMEReturnCode.PMFalse)
        End If

        lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegIsAutoRefresh, v_sSettingValue:=sIsAutoRefresh, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        ' Set the Refresh Rate Setting
        sRefreshRate = CStr(m_fMainForm.RefreshRate)
        lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegRefreshRate, v_sSettingValue:=sRefreshRate, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        ' RDC 22112000 set listview column widths
        sColumnWidths = m_fMainForm.ColumnWidths
        lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACWrkManRegColumnWidths, v_sSettingValue:=sColumnWidths, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        ' RDC 17072002 set combobox settings
        sComboSettings = m_fMainForm.ComboSettings
        lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACWrkManRegComboSettings, v_sSettingValue:=sComboSettings, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)


    End Sub

    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()


        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Private Sub m_fMainForm_AddTaskToFavourites(ByVal v_sAvailableTaskKey As String, ByVal v_iIndex As Integer) Handles m_fMainForm.AddTaskToFavourites


        ' Add the Task to the Favourites Group
        Dim lReturn As gPMConstants.PMEReturnCode = CType(AddTaskToFavourites(v_sAvailableTaskKey, v_iIndex), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub m_fMainForm_AddTaskToQSBar(ByVal v_sAvailableTaskKey As String) Handles m_fMainForm.AddTaskToQSBar

        ' Add the Task to the QS Bar.
        Dim lReturn As gPMConstants.PMEReturnCode = CType(AddTaskToQSBar(v_sAvailableTaskKey), gPMConstants.PMEReturnCode)

    End Sub

    'DAK110700
    Private Sub m_fMainForm_ChangeMainFormCaption() Handles m_fMainForm.ChangeMainFormCaption
        Dim lReturn As gPMConstants.PMEReturnCode





        lReturn = m_oServerRegistry.SetServerRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.ACWrkManRegFormCaption, v_sSettingValue:=m_fMainForm.FormCaption, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to write " & _
                                gPMConstants.ACWrkManRegFormCaption & _
                                " to Registry on Server.")
        End If

        If m_fMainForm.FormCaption = "" Then
            m_fMainForm.Text = ACMainFormCaption & " (" & Username & ")"
        Else
            m_fMainForm.Text = m_fMainForm.FormCaption & " (" & Username & ")"
        End If



    End Sub

    Private Sub m_fMainForm_ChangePMNewsAddress() Handles m_fMainForm.ChangePMNewsAddress
        Dim lReturn As gPMConstants.PMEReturnCode




        lReturn = m_oBusiness.SetPMNewsWebAddress(m_fMainForm.PMNewsWebAddress)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to write to Registry on Server.")
        End If



    End Sub

    Private Sub m_fMainForm_ChangePMSupportAddress() Handles m_fMainForm.ChangePMSupportAddress
        Dim lReturn As gPMConstants.PMEReturnCode





        lReturn = m_oServerRegistry.SetServerRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.ACWrkManRegSupportWebAddress, v_sSettingValue:=m_fMainForm.PMSupportWebAddress, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to write " & _
                                gPMConstants.ACWrkManRegSupportWebAddress & _
                                " to Registry on Server.")
        End If



    End Sub

    Private Sub m_fMainForm_ChangeWebTabCaption() Handles m_fMainForm.ChangeWebTabCaption
        Dim lReturn As gPMConstants.PMEReturnCode




        lReturn = m_oBusiness.SetWebTabCaption(m_fMainForm.WebTabCaption)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to write " & _
                                gPMConstants.ACWrkManRegWebTabCaption & _
                                " to Registry on Server.")
        End If



    End Sub

    Private Sub m_fMainForm_CheckForDueSystemTasks() Handles m_fMainForm.CheckForDueSystemTasks

        Dim lReturn As gPMConstants.PMEReturnCode = CType(CheckForDueSystemTasks(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub m_fMainForm_DecrementFavouritesIndex(ByVal v_iIndex As Integer) Handles m_fMainForm.DecrementFavouritesIndex


        Dim lReturn As Integer = DecrementFavouritesIndex(v_iIndex)

    End Sub

    Private Sub m_fMainForm_DoTaskNow(ByVal v_sAvailableTaskKey As String) Handles m_fMainForm.DoTaskNow

        ' Start the Task Now.
        Dim lReturn As gPMConstants.PMEReturnCode = CType(StartQuickStartTask(v_sAvailableTaskKey), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub m_fMainForm_FormClose() Handles m_fMainForm.FormClose

        Dim lReturn As gPMConstants.PMEReturnCode = CType(CloseWorkManager(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub m_fMainForm_IncrementFavouritesIndex(ByVal v_iIndex As Integer) Handles m_fMainForm.IncrementFavouritesIndex


        Dim lReturn As Integer = IncrementFavouritesIndex(v_iIndex)

    End Sub

    Private Sub m_fMainForm_MoveFavouriteTask(ByVal v_sAvailableTaskKey As String, ByVal v_iIndex As Integer) Handles m_fMainForm.MoveFavouriteTask


        Dim lReturn As gPMConstants.PMEReturnCode = CType(MoveFavouriteTask(v_sAvailableTaskKey, v_iIndex), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub m_fMainForm_NewTaskKnownType(ByVal v_sAvailableTaskKey As String) Handles m_fMainForm.NewTaskKnownType

        ' Get a Reference to the Available Task using the Key
        Dim oAvailableTask As PMWorkManager.AvailableTask = m_oAvailableTasks.Item(v_sAvailableTaskKey)

        If oAvailableTask Is Nothing Then
            Exit Sub
        End If

        ' Get the Task Group and Task ID
        Dim lPMTaskGroupID As Integer = oAvailableTask.TaskGroupID
        Dim lPMTaskID As Integer = oAvailableTask.TaskID

        ' Add a New Task of Known Group/Task
        Dim lReturn As gPMConstants.PMEReturnCode = CType(NewSchedTask(r_lPMWrkTaskInstanceCnt:=0, v_lPMWrkTaskGroupID:=lPMTaskGroupID, v_lPMWrkTaskID:=lPMTaskID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub m_fMainForm_NewTaskUnknownType() Handles m_fMainForm.NewTaskUnknownType
        ' Add a New Task of Unknown Group/Task
        Dim lReturn As gPMConstants.PMEReturnCode = CType(NewSchedTask(r_lPMWrkTaskInstanceCnt:=0), gPMConstants.PMEReturnCode)
    End Sub

    ' RDC 17052002 edited to use generic browser routine m_fMainForm_ShowWebsite
    ' it is highly unlikely that m_fMainForm_PMSiriusSupport will be used again as the old Support
    ' button and menu items are now invisible and their details have moved to the pmwrk_websites lookup table
    Private Sub m_fMainForm_PMSiriusSupport() Handles m_fMainForm.PMSiriusSupport
        'Dim oSiriusSupport As Object
        'Dim lReturn As Long
        '
        '
        '    ' Get the Sirius Support interface object
        '    lReturn = m_oObjectManager.GetInstance( _
        ''        oObject:=oSiriusSupport, _
        ''        sClassName:="iPMSiriusSupport.PMSiriusSupport", _
        ''        vinstancemanager:=PMGetLocalInterface)
        '    If (lReturn <> PMTrue) Then
        '        SetMousePointer PMMouseNormal
        '        DisplayErrorMessage _
        ''            iType:=PMLogError, _
        ''            sMsg:="Failed to Launch Internet Explorer"
        '        Exit Sub
        '    End If
        '
        '    oSiriusSupport.PMSiriusSupportURL = m_fMainForm.PMSupportWebAddress
        '
        ' RDC 17052002 call generic browser routine
        m_fMainForm_ShowWebsite(m_fMainForm.PMSupportWebAddress)
        '
        '    lReturn = oSiriusSupport.PMSiriusSupport
        '
        '    oSiriusSupport.Terminate
        '    Set oSiriusSupport = Nothing
        '
        '    If (lReturn <> PMTrue) Then
        '        SetMousePointer PMMouseNormal
        '        DisplayErrorMessage _
        ''            iType:=PMLogError, _
        ''            sMsg:="Failed to Launch Internet Explorer"
        '        Exit Sub
        '    End If
        '
        '    m_fMainForm.UpdateStatusBar v_vActivity:=""
        '
        '    Exit Sub

    End Sub

    ' RDC 16052002 to support new toolbar icons and Links menu
    Private Sub m_fMainForm_ShowWebsite(ByVal sURL As String) Handles m_fMainForm.ShowWebsite

        'TODO
        'Dim oSiriusSupport As iPMSiriusSupport.PMSiriusSupport
        Dim oSiriusSupport As Object

        ' Get the Sirius Support interface object
        Dim temp_oSiriusSupport As Object
        Dim lReturn As gPMConstants.PMEReturnCode = m_oObjectManager.GetInstance(temp_oSiriusSupport, "iPMSiriusSupport.PMSiriusSupport", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oSiriusSupport = temp_oSiriusSupport

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Launch Internet Explorer")
            Exit Sub
        End If


        oSiriusSupport.PMSiriusSupportURL = sURL


        lReturn = oSiriusSupport.PMSiriusSupport


        oSiriusSupport.Dispose()

        oSiriusSupport = Nothing
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Launch Internet Explorer")
            Exit Sub
        End If

        m_fMainForm.UpdateStatusBar(v_vActivity:="")

    End Sub

    Public Sub m_fMainForm_RefreshAvailableTasks() Handles m_fMainForm.RefreshAvailableTasks
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim selGroup As ListBarGroup = m_fMainForm.albAvailableTasks.SelectedGroup
        ' If the Form is not shown yet, then do not Refresh the Available Tasks.
        If m_fMainForm.FormDisplayed Then
            Dim bl As Boolean = m_fMainForm.albAvailableTasks.ClearHashGroup()
            m_fMainForm.albAvailableTasks.Groups.Clear()

            'DAK301199
            lReturn = CType(SaveFavouritesTasks(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            ' Populate the Available Tasks
            lReturn = CType(PopulateAvailableTasks(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            lReturn = CType(LoadFavourites(), gPMConstants.PMEReturnCode)
            If (m_fMainForm.albAvailableTasks.Groups.Count > 0) Then
                m_fMainForm.albAvailableTasks.SelectedGroup = selGroup
                m_fMainForm.albAvailableTasks.CurrentSelected = selGroup
                m_fMainForm.albAvailableTasks.SelectGroup(selGroup)
                m_fMainForm.albAvailableTasks.CurrentSelected = Nothing
            Else
                MessageBox.Show("Synchronizing please refresh again...")
            End If
        End If

    End Sub

    Private Sub m_fMainForm_RefreshScheduledTasks(ByVal v_bForceRefresh As Boolean) Handles m_fMainForm.RefreshScheduledTasks
        Dim lReturn As gPMConstants.PMEReturnCode

        ' If the Form is not shown yet, then do not Refresh the Scheduled Tasks.
        If m_fMainForm.FormDisplayed Then
            ' Populate the Scheduled Tasks
            lReturn = CType(PopulateSchedTasks(v_bForceRefresh:=v_bForceRefresh), gPMConstants.PMEReturnCode)
        End If

    End Sub
    Private Sub m_fMainForm_RefreshBatchTasks(ByVal v_bForceRefresh As Boolean) Handles m_fMainForm.RefreshBatchTasks
        Dim lReturn As gPMConstants.PMEReturnCode
        Try
            If m_fMainForm.FormDisplayed And m_fMainForm.tabMain.SelectedIndex = 1 Then
                lReturn = CType(PopulateBatchTasks(bForceRefresh:=v_bForceRefresh), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to load Batch tasks.")
                End If
            End If
        Catch ex As Exception
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error in Populate Batch Tasks. Error: " + ex.Message, vApp:=ACApp, vClass:=ACClass)
        End Try
    End Sub

    Private Sub m_fMainForm_RemoveTaskFromFavourites(ByVal v_sAvailableTaskKey As String, ByVal v_iIndex As Integer) Handles m_fMainForm.RemoveTaskFromFavourites


        Dim lReturn As gPMConstants.PMEReturnCode = CType(RemoveTaskFromFavourites(v_sAvailableTaskKey, v_iIndex), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub m_fMainForm_RemoveFromQSBar(ByVal v_sAvailableTaskKey As String) Handles m_fMainForm.RemoveFromQSBar

        ' Remove the Task
        Dim lReturn As gPMConstants.PMEReturnCode = CType(RemoveTaskFromQSBar(v_sAvailableTaskKey), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub m_fMainForm_ScheduledTaskAction(ByVal eAction As MainModule.ACESchedTaskAction) Handles m_fMainForm.ScheduledTaskAction

        Dim lReturn As gPMConstants.PMEReturnCode

        ' Is there a Selected Scheduled Task
        If ScheduledTaskKey = "" Then
            ' No, so do Nothing
            Exit Sub
        End If

        ' Get a Reference to the loaded Scheduled Task
        Dim oSchedTask As PMWorkManager.ScheduledTask = m_oSchedTasks.Item(ScheduledTaskKey)

        ' If we haven't got it for some reason, then exit.
        If oSchedTask Is Nothing Then
            Exit Sub
        End If

        ' If the User has Double Clicked on a Task which is already in Progress
        ' then ingore.
        If ((oSchedTask.TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress) And (eAction = MainModule.ACESchedTaskAction.aceSTAStart)) Then
            oSchedTask = Nothing
            Exit Sub
        End If

        ' If the User has Double Clicked on a Complete Task
        ' then Delete It.
        If (oSchedTask.TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete) And (eAction = MainModule.ACESchedTaskAction.aceSTAStart) Then
            eAction = MainModule.ACESchedTaskAction.aceSTADelete
        End If
        If oSchedTask.IsReadOnlyTask = 1 Then
            eAction = MainModule.ACESchedTaskAction.aceSTAView
        End If
        ' If the User has Double Clicked on a Memo Task
        If (oSchedTask.TypeOfTask = gPMConstants.PMEWrkManTaskType.pmeWMTTMemo) And (eAction = MainModule.ACESchedTaskAction.aceSTAStart) Then

            ' If the Memo Task has NO Assigned User
            If oSchedTask.UserID = "" Then
                ' Assign It
                eAction = MainModule.ACESchedTaskAction.aceSTAAssign

                ' Else If the Task is Assigned to ME
            ElseIf (CDbl(oSchedTask.UserID) = UserID) Then
                ' Start it

                ' Otherwise, it is NOT Assigned to ME
            Else
                ' Assign It
                eAction = MainModule.ACESchedTaskAction.aceSTAAssign
            End If

        End If

        ' Get the Task Instance Cnt.
        Dim lPMWrkTaskInstanceCnt As Integer = oSchedTask.PMWrkTaskInstanceCnt
        ' Release Reference.
        oSchedTask = Nothing


        Select Case eAction
            Case MainModule.ACESchedTaskAction.aceSTAAssign
                ' For the Puroposes of this Form Delete = ReAssign
                lReturn = CType(EditViewAssignSchedTask(v_lAction:=gPMConstants.PMEComponentAction.PMDelete, v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)

            Case MainModule.ACESchedTaskAction.aceSTAComplete
                lReturn = CType(CompleteSchedTask(v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)

            Case MainModule.ACESchedTaskAction.aceSTADelete
                lReturn = CType(DeleteSchedTask(v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)

            Case MainModule.ACESchedTaskAction.aceSTAEdit
                lReturn = CType(EditViewAssignSchedTask(v_lAction:=gPMConstants.PMEComponentAction.PMEdit, v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)

            Case MainModule.ACESchedTaskAction.aceSTAIncomplete
                lReturn = CType(IncompleteSchedTask(v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)

            Case MainModule.ACESchedTaskAction.aceSTAStart
                'AR20050428 - PN7388 Pass FromSchedule flag
                lReturn = CType(StartScheduledTask(v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_bFromSchedule:=True), gPMConstants.PMEReturnCode)

            Case MainModule.ACESchedTaskAction.aceSTATaskLog
                'DAK071299
                lReturn = CType(AddViewSchedTaskLog(v_lAction:=gPMConstants.PMEComponentAction.PMEdit, v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)

            Case MainModule.ACESchedTaskAction.aceSTAView
                lReturn = CType(EditViewAssignSchedTask(v_lAction:=gPMConstants.PMEComponentAction.PMView, v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)

            Case Else
                Exit Sub

        End Select

    End Sub

    Private Sub m_fMainForm_ScheduledTaskClick(ByVal v_sScheduledTaskKey As String) Handles m_fMainForm.ScheduledTaskClick

        ' Store the Scheduled Task Key that was clicked on
        ScheduledTaskKey = v_sScheduledTaskKey

        ' Set the Task Menu options accordingly
        SetTaskMenuOptions()

    End Sub

    Private Sub m_fMainForm_ScheduledTaskRightClick(ByVal v_sScheduledTaskKey As String, ByVal v_lSelectedUserGroupId As Integer) Handles m_fMainForm.ScheduledTaskRightClick

        ' Store the Scheduled Task Key that was clicked on
        ScheduledTaskKey = v_sScheduledTaskKey
        m_lSelectedUserGroupId = v_lSelectedUserGroupId

        ' Set the Task Menu options accordingly
        SetTaskMenuOptions()

        ' Display the Task Menu
        m_fMainForm.DisplayTaskMenu()

    End Sub

    Private Sub m_oInProgTasks_InProgTaskClose(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_bStatusUpdated As Boolean) Handles m_oInProgTasks.InProgTaskClose

        Dim lReturn As gPMConstants.PMEReturnCode

        ' A Navigator Instance has closed

        ' Has the Process Status Already Been Updated
        If v_bStatusUpdated Then
            ' Yes, so do nothing
        Else
            ' No, so update the Task Status of the associated Task.
            lReturn = CType(UpdateSchedTaskStatus(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
        End If

    End Sub

    Private Sub m_oInProgTasks_InProgTaskUpdateStatus(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_bComplete As Boolean) Handles m_oInProgTasks.InProgTaskUpdateStatus


        ' A Navigator Instance has told us what the Process Status is
        ' so update it.
        Dim lReturn As gPMConstants.PMEReturnCode = CType(UpdateSchedTaskStatus(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_vComplete:=v_bComplete), gPMConstants.PMEReturnCode)

    End Sub

    ' ***************************************************************** '
    ' DAK130999
    ' Name: IncrementFavouritesIndex
    '
    ' Description:
    ' ***************************************************************** '
    Public Function IncrementFavouritesIndex(ByVal v_iIndex As Integer) As Integer



        For Each oAvailableTask As PMWorkManager.AvailableTask In m_oAvailableTasks
            With oAvailableTask
                If .FavouritesIndex >= v_iIndex Then
                    .FavouritesIndex += 1
                End If
            End With
        Next oAvailableTask


    End Function

    ' ***************************************************************** '
    ' DAK130999
    ' Name: DecrementFavouritesIndex
    '
    ' Description:
    ' ***************************************************************** '
    Public Function DecrementFavouritesIndex(ByVal v_iIndex As Integer) As Integer



        For Each oAvailableTask As PMWorkManager.AvailableTask In m_oAvailableTasks
            With oAvailableTask
                If .FavouritesIndex > v_iIndex Then
                    .FavouritesIndex -= 1
                End If
            End With
        Next oAvailableTask


    End Function

    ' RDC 16052002 get toolbar website button data
    Public Function GetToolBarWebsiteData(ByRef vButtonData As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            lReturn = m_oBusiness.GetToolBarWebsiteData(vButtonData)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name : m_fMainForm_ScheduledMultipleTaskAction
    '
    ' Desc : Event triggered when the listview is double clicked
    '        after multiple tasks are selected
    '
    ' Edit History
    ' RAM20020712 : Created
    ' ***************************************************************** '
    Private Sub m_fMainForm_ScheduledMultipleTaskAction(ByVal eAction As MainModule.ACESchedTaskAction, ByVal v_vScheduledTaskKey As Object) Handles m_fMainForm.ScheduledMultipleTaskAction

        Dim lPMWrkTaskInstanceCnt As Integer
        Dim oSchedTask As PMWorkManager.ScheduledTask
        Dim vTaskInstanceCntArray() As Object
        Dim sTaskKey As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        m_fMainForm.UpdateStatusBar(v_vActivity:="Tasks Reassign in Process...")

        ' Is there any Selected Scheduled Task
        If Information.IsArray(v_vScheduledTaskKey) Then


            For iCounter As Integer = v_vScheduledTaskKey.GetLowerBound(0) To v_vScheduledTaskKey.GetUpperBound(0)


                sTaskKey = CStr(v_vScheduledTaskKey(iCounter))

                ' Get a Reference to the selected Scheduled Task
                oSchedTask = m_oSchedTasks.Item(sTaskKey)

                ' If we haven't got it for some reason, then ignore it
                If oSchedTask Is Nothing Then
                Else
                    ' Get the Task Instance Cnt.
                    lPMWrkTaskInstanceCnt = oSchedTask.PMWrkTaskInstanceCnt

                    If Information.IsArray(vTaskInstanceCntArray) Then

                        ReDim Preserve vTaskInstanceCntArray(vTaskInstanceCntArray.GetUpperBound(0) + 1)
                    Else
                        ReDim vTaskInstanceCntArray(0)
                    End If


                    vTaskInstanceCntArray(vTaskInstanceCntArray.GetUpperBound(0)) = lPMWrkTaskInstanceCnt

                    ' Release Reference.
                    oSchedTask = Nothing

                End If

            Next iCounter


            lReturn = CType(EditViewAssignMultipleSchedTask(v_lAction:=gPMConstants.PMEComponentAction.PMDelete, v_vPMWrkTaskInstanceCntArray:=vTaskInstanceCntArray), gPMConstants.PMEReturnCode)
            ' Do we need to log any error here ?
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log error if we want
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            Else
            End If

        End If

        m_fMainForm.UpdateStatusBar(v_vActivity:="")
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    ' ***************************************************************** '
    ' Name : m_fMainForm_ScheduledMultipleTaskRightClick
    ' Desc : Event triggered when the right mouse button is clicked
    '        after multiple tasks are selected
    '
    ' Edit History
    ' RAM20020712 : Created (Being this is a simple event, no error handling required)
    ' RAM20020712 : Display the Menu with Assign Option only
    ' ***************************************************************** '
    Private Sub m_fMainForm_ScheduledMultipleTaskRightClick(ByVal v_vScheduledTaskKey As Object) Handles m_fMainForm.ScheduledMultipleTaskRightClick

        ' Is there any Selected Scheduled Task
        If Information.IsArray(v_vScheduledTaskKey) Then

            ' Set the Task Menu options accordingly
            m_fMainForm.SetTaskMenuOptions(v_bNewEnabled:=False, v_bEditEnabled:=False, v_bAssignEnabled:=True, v_bViewEnabled:=False, v_bStartEnabled:=False, v_bCompleteEnabled:=False, v_bIncompleteEnabled:=False, v_bDeleteEnabled:=False, v_bTaskLogEnabled:=False)

            ' Display the Task Menu
            m_fMainForm.DisplayTaskMenu()

        Else
            ' No, so do Nothing
            Exit Sub
        End If


    End Sub

    ' ***************************************************************** '
    ' Name : EditViewAssignMultipleSchedTask
    ' Desc : Function to call the Task Instances form for Multiple Scheduled Task
    '
    ' Edit History
    ' RAM20020715 : Created
    ' ***************************************************************** '
    Private Function EditViewAssignMultipleSchedTask(ByVal v_lAction As Integer, ByVal v_vPMWrkTaskInstanceCntArray() As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sCurrentlyLockedBy As String = ""
        Dim lPMWrkTaskInstanceCnt As Integer
        Dim vTasksLocked() As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        If Information.IsArray(v_vPMWrkTaskInstanceCntArray) Then

            For Each v_vPMWrkTaskInstanceCntArray_item As Object In v_vPMWrkTaskInstanceCntArray


                lPMWrkTaskInstanceCnt = CInt(v_vPMWrkTaskInstanceCntArray_item)

                ' Lock the Task Instance

                lReturn = m_oBusiness.LockTaskInstance(v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, r_sCurrentlyLockedBy:=sCurrentlyLockedBy)

                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    If Information.IsArray(vTasksLocked) Then

                        ReDim Preserve vTasksLocked(vTasksLocked.GetUpperBound(0) + 1)
                    Else
                        ReDim vTasksLocked(0)
                    End If


                    vTasksLocked(vTasksLocked.GetUpperBound(0)) = lPMWrkTaskInstanceCnt

                Else

                    DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Scheduled Task : " & lPMWrkTaskInstanceCnt & " is Locked by user " & sCurrentlyLockedBy.Trim())

                    If Information.IsArray(vTasksLocked) Then


                        For iCounter2 As Integer = vTasksLocked.GetLowerBound(0) To vTasksLocked.GetUpperBound(0)


                            lPMWrkTaskInstanceCnt = CInt(vTasksLocked(iCounter2))

                            ' UnLock the Task Instance

                            lReturn = m_oBusiness.UnlockTaskInstance(v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to UnLock Scheduled Task : " & lPMWrkTaskInstanceCnt & ". Contact your system Administrator.")
                            End If

                        Next iCounter2

                        ' Reset the Locked Tasks Array

                        vTasksLocked = Nothing

                    End If

                End If

            Next v_vPMWrkTaskInstanceCntArray_item

            If Information.IsArray(vTasksLocked) Then
                ' So we locked the tasks with no problem
                ' We have to display the Multiple tasks Assignment form


                lReturn = CType(DisplayMultipleTaskInstanceForm(v_lAction:=v_lAction, r_vPMWrkTaskInstanceCntArray:=vTasksLocked), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to display the Task Instance Form.")
                    Return result
                End If

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                ' UnLock the Task Instance
                If Information.IsArray(vTasksLocked) Then


                    For iCounter2 As Integer = vTasksLocked.GetLowerBound(0) To vTasksLocked.GetUpperBound(0)


                        lPMWrkTaskInstanceCnt = CInt(vTasksLocked(iCounter2))

                        ' UnLock the Task Instance

                        lReturn = m_oBusiness.UnlockTaskInstance(v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to UnLock Scheduled Task : " & lPMWrkTaskInstanceCnt & ". Contact your system Administrator.")
                        End If

                    Next iCounter2

                    ' Reset the Locked Tasks Array

                    vTasksLocked = Nothing

                End If

            End If
        Else
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name : DisplayMultipleTaskInstanceForm
    ' Desc : Displays the Task Instances form for Multiple Scheduled Task
    '
    ' Edit History
    ' RAM20020715 : Created
    ' ***************************************************************** '
    Private Function DisplayMultipleTaskInstanceForm(ByVal v_lAction As Integer, ByRef r_vPMWrkTaskInstanceCntArray() As Object, Optional ByVal v_lPMWrkTaskGroupID As Integer = 0, Optional ByVal v_lPMWrkTaskID As Integer = 0) As Integer

        Dim result As Integer = 0

        'Developer Guide No. 88(Guide)
        Dim oTaskInstance As iPMWrkTaskInstance.Interface_Renamed
        Dim lReturn As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create the Component
        Dim temp_oTaskInstance As Object
        lReturn = m_oObjectManager.GetInstance(temp_oTaskInstance, sClassName:="iPMWrkTaskInstance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oTaskInstance = temp_oTaskInstance
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Set Process Modes

        lReturn = oTaskInstance.SetProcessModes(vTask:=v_lAction, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        oTaskInstance.UserID = m_iUserID

        oTaskInstance.PMUserGroupID = m_fMainForm.cboUserGroup.UserGroupID

        ' Edit, View, ReAssign Mode so set the Key

        oTaskInstance.PMWrkTaskInstanceCntArray = r_vPMWrkTaskInstanceCntArray

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        ' Start the Form

        lReturn = oTaskInstance.Start
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Task Instance Form:- iPMWrkTaskInstance.Interface")

            'lReturn = oTaskInstance.Terminate()
            oTaskInstance.Dispose()
            oTaskInstance = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' If the User Canceled then exit as we do not need
        ' to Refresh the Form details.

        If oTaskInstance.Status = gPMConstants.PMEReturnCode.PMCancel Then
            r_vPMWrkTaskInstanceCntArray = Nothing

            oTaskInstance.Dispose()
            oTaskInstance = Nothing
            Return result
        End If


        oTaskInstance.Dispose()
        oTaskInstance = Nothing

        Return result

    End Function
    ''' <summary>
    ''' Check User authority for Can User view Batch Task Status
    ''' </summary>
    ''' <param name="nHasViewBatchProcessStatus"></param>
    ''' <returns></returns>
    Public Function GetBatchProcessTabUserAuthority(ByRef nHasViewBatchProcessStatus As Integer) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Dim m_vAllocationData(,) As Object
        nResult = m_oBusiness.GetViewBatchProcessStatusUserAuthority(m_vAllocationData)

        Dim sMethodName As String = "GetReceiptReversalUserAuthority"

        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + sMethodName + ", GetReceiptReversalUserAuthority failed")
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If Not Information.IsArray(m_vAllocationData) Then
            nHasViewBatchProcessStatus = 0
        Else
            nHasViewBatchProcessStatus = gPMFunctions.ToSafeInteger(m_vAllocationData(kACHasViewBatchProcessStatusArrPos, 0))
        End If

        Return nResult

    End Function
End Class

