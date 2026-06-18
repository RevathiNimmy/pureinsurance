
Option Strict Off
Option Explicit On
Imports SharedFiles
Friend Class startForm
    Inherits System.Windows.Forms.Form

    Dim mainForm As New mainForm

    ' RAW 02/09/2003 : CQ2158 : added code to get SQLServer version
    Private Sub startForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim g_oObjectManager As bObjectManager.ObjectManager
        Dim bPMUser As bPMUser.Business
        Dim lReturn As Integer

        Dim sDontShowWarning As String

        'force a log on before calling mainForm
        g_oObjectManager = New bObjectManager.ObjectManager

        ' Call the initialise method.

        lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            'try to access the database
            lReturn = gPMComponentServices.NewDatabase(g_sUsername, g_iSourceID, g_iLanguageID, _
                                                       v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=g_oDatabase)
        End If

        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID

                lReturn = g_oObjectManager.GetInstance(bPMUser, "bPMUser.Business", vInstanceManager:="ClientManager")

            End With
        End If

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                MessageBox.Show("User cancelled logon. Application will now terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Could not log on to Sirius Solutions", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            If Not (g_oObjectManager Is Nothing) Then
                g_oObjectManager.Dispose()
            End If
            g_oObjectManager = Nothing
            If Not (bPMUser Is Nothing) Then

                bPMUser.Dispose()
            End If
            bPMUser = Nothing
            Me.Close()
            Environment.Exit(0)
        End If

        'check if logged on with system access
        lReturn = GetSysAdminStatus(bPMUser)

        If Not (g_oObjectManager Is Nothing) Then
            g_oObjectManager.Dispose()
        End If
        'UPGRADE_NOTE: Object g_oObjectManager may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        g_oObjectManager = Nothing
        If Not (bPMUser Is Nothing) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object oBusiness.Terminate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            bPMUser.Dispose()
        End If
        'UPGRADE_NOTE: Object oBusiness may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        bPMUser = Nothing

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Me.Close()
            End
        End If

        ' RAW 02/09/2003 : CQ2158 : added
        If GetSQLServerVersion(r_oDatabase:=g_oDatabase) <= 0 Then
            MsgBox("Failed to get SQLServer Version", MsgBoxStyle.Critical, ACApp)
            Me.Close()
            End
        End If

        'run the new users logged on functionality
        'replicated from UsersLoggedOn.exe
        'PIE development November 2008
        lReturn = CheckUsersLoggedOn 'THIS LINE NEEDS AMENDING
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Me.Close()
            End
        End If

        'now we've only got the one user on the system we can set everyone except this
        'user to deleted_flag = 1, but we need to store their userids so we can set them back following
        'the process ending

        lReturn = DisableUserLogins
        'END PIE development November 2008

        'Richard Clarke January 2009 - enhancements
        'get the registry setting for whether to show the form or not
        GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, _
                        v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_DoNotShowWarning", r_sSettingValue:=sDontShowWarning)

        'only show the warning form if the reg value is 0
        'BSJ Use Val to check
        If Val(sDontShowWarning) = 0 Then
            warningForm.ShowDialog()
        End If
        'Richard Clarke January 2009 - enhancements

        'warningForm.Show 1 'Richard Clarke January 2009 - commented out - see above
        PbEdoEXport.objFrmMainForm = Me.mainForm

        mainForm.ShowDialog()

        'mainForm.radioExportBasedOn(radioExportBasedOn_Migration).Visible = False
        'mainForm.txtdatamodelId.Visible = False
        mainForm.Show()
        Me.Close()

    End Sub
    ' ***************************************************************** '
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 16102002 created
    ' ***************************************************************** '
    Private Function GetSysAdminStatus(ByRef r_oBusiness As Object) As Integer

        Dim lStatus As Integer
        Dim lReturn As Integer

        Try

            GetSysAdminStatus = gPMConstants.PMEReturnCode.PMFalse

            'UPGRADE_WARNING: Couldn't resolve default property of object r_oBusiness.GetSysAdminStatus. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lReturn = r_oBusiness.GetSysAdminStatus(lStatus)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue OrElse lStatus = 0 Then

                MsgBox("You do not have permission to access " & ACApp & "." & vbCrLf & vbCrLf & "Please contact your System Administrator.", MsgBoxStyle.Critical, ACApp)

                Exit Function
            End If

            GetSysAdminStatus = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            GetSysAdminStatus = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminStatus Failed", vApp:=ACApp, vClass:="", vMethod:="GetSysAdminStatus", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function

    'new function that disables all user logins except
    'the current logged in user using PIE
    Private Function DisableUserLogins() As Integer

        Dim sSQL As String
        Dim vResults(,) As Object
        Dim iCounter As Short

        'now need to create the column if it doesn't exist
        sSQL = "EXEC DDLADDCOLUMN 'PMUser', 'previous_is_deleted', 'TINYINT'"
        g_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CreatePreviousIsDeleted", bStoredProcedure:=False)

        'first check that the application didn't crash (only 1 user set to is_deleted = 0)
        sSQL = "SELECT COUNT(user_id) FROM PMUser with (nolock) WHERE is_deleted = 0 OR is_deleted IS NULL"
        g_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Retrieving user logins", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

        If IsArray(vResults) Then
            If Convert.ToInt32(vResults(0, 0)) = 0 Then
                'something went wrong, don't set the is_deleted flags and previous_is_deleted flags
                DisableUserLogins = gPMConstants.PMEReturnCode.PMTrue
                Exit Function
            End If
        End If

        'now get all the users and set their previous_is_deleted and new is_deleted values
        'so once the PIE import /export is completed they can be re-enabled
        sSQL = "SELECT user_id, ISNULL(is_deleted, 0), username FROM PMUser with (nolock)"
        g_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Retrieving user logins", bStoredProcedure:=False, bKeepNulls:=True, lnumberrecords:=gPMConstants.PMAllRecords, vResultArray:=g_UserIDs)

        If IsArray(g_UserIDs) Then
            For iCounter = 0 To UBound(g_UserIDs, 2)
                'disable this user's login ability
                'UPGRADE_WARNING: Couldn't resolve default property of object g_UserIDs(0, iCounter). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object g_UserIDs(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sSQL = "UPDATE PMUser SET previous_is_deleted = " & g_UserIDs(1, iCounter) & " WHERE user_id = " & g_UserIDs(0, iCounter)
                g_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Disabling user logins", bStoredProcedure:=False)

                'now we've backed up their status, set the is_deleted flag to 1
                'only do this if it's 0 and the user <> g_sUsername
                'UPGRADE_WARNING: Couldn't resolve default property of object g_UserIDs(2, iCounter). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object g_UserIDs(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If CShort(g_UserIDs(1, iCounter)) = 0 AndAlso g_UserIDs(2, iCounter) <> g_sUsername Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object g_UserIDs(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQL = "UPDATE PMUser SET is_deleted = 1 WHERE user_id = " & g_UserIDs(0, iCounter)
                    g_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Disabling user logins", bStoredProcedure:=False)
                End If

            Next iCounter
        End If

    End Function
End Class