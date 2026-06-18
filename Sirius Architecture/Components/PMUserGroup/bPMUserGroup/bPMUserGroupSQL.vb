Option Strict Off
Option Explicit On
Module PMUserGroupSQL
    ' ***************************************************************** '
    ' Class Name: PMUserSQL
    '
    ' Date: 17 October 1996
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Enbedded SQL) manipulate an PMUser
    '
    ' Edit History:
    ' RFC290398 - LoggedOnAtClient added, Timestamp removed.
    ' RFC270398 - Logon and Logoff added.
    ' RFC250398 - Stored Procedure Calls replaced by embedded sql to
    ' RFC250398 - allow Architecture components to work in db's that
    ' RFC250398 - don't support stored procedures.
    'RFC300399 - Change to use ODBC Call syntax for stored procedures.
    ' DAK300899 - Add SQL details for group membership check
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectPMUserGroup"
    ' Public Const ACSelectSQL = "SELECT * FROM PMUser_Group WHERE PMUser_id = {PMUser_id}"

    ' Select PMUserGroup SQL
    'RFC300399
    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectPMUserGroup"
    'Developer Guide No. 39
    Public Const ACGetDetailsSQL As String = "spu_pmuser_group"

    ' Select Count Of All System Administrators SQL
    'RFC300399
    Public Const ACGetSystemAdministratorsStored As Boolean = True
    Public Const ACGetSystemAdministratorsName As String = "SelectSystemAdministrators"
    Public Const ACGetSystemAdministratorsSQL As String = "spu_pmuser_group_sysadmins"

    ' Select All PMUserGroups SQL
    'RFC300399
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllPMUserGroup"
    Public Const ACGetAllDetailsSQL As String = "spu_pmuser_group_all"

    ' Select All PMUserGroups SQL
    'RFC300399
    Public Const ACGetEveryoneDetailsStored As Boolean = True
    Public Const ACGetEveryoneDetailsName As String = "SelectEveryonePMUserGroup"
    Public Const ACGetEveryoneDetailsSQL As String = "spu_pmuser_group_everyone"

    ' Add PMUserGroup SQL
    'RFC300399
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPMUserGroup"
    Public Const ACAddSQL As String = "spu_pmuser_group_add"

    ' Delete PMUserGroup SQL
    'RFC300399
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeletePMUserGroup"
    Public Const ACDeleteSQL As String = "spu_pmuser_group_del"

    ' Update PMUserGroup SQL
    'RFC300399
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePMUserGroup"
    Public Const ACUpdateSQL As String = "spu_pmuser_group_upd"

    ' Delete Group Group Membership SQL
    'RFC300399
    Public Const ACDeleteGroupMembershipStored As Boolean = True
    Public Const ACDeleteGroupMembershipName As String = "DeleteGroupMembership"
    Public Const ACDeleteGroupMembershipSQL As String = "spu_pmuser_group_group_del"

    ' Add Group Group Membership SQL
    'RFC300399
    Public Const ACAddGroupMembershipStored As Boolean = True
    Public Const ACAddGroupMembershipName As String = "AddGroupMembership"
    Public Const ACAddGroupMembershipSQL As String = "spu_pmuser_group_group_add"

    ' Delete Group User Membership SQL
    'RFC300399
    Public Const ACDeleteUserMembershipStored As Boolean = True
    Public Const ACDeleteUserMembershipName As String = "DeleteUserMembership"
    Public Const ACDeleteUserMembershipSQL As String = "spu_pmuser_group_user_del"

    ' Add Group User Membership SQL
    'RFC300399
    Public Const ACAddUserMembershipStored As Boolean = True
    Public Const ACAddUserMembershipName As String = "AddUserMembership"
    Public Const ACAddUserMembershipSQL As String = "spu_pmuser_group_user_add"

    ' Select All PMUserGroups SQL
    'RFC300399
    Public Const ACGetAllTasksStored As Boolean = True
    Public Const ACGetAllTasksName As String = "SelectAllPMUserGroupTasks"
    Public Const ACGetAllTasksSQL As String = "spu_pmuser_group_tasks"

    ' Delete Group Tasks SQL
    'RFC300399
    Public Const ACDeleteGroupTasksStored As Boolean = True
    Public Const ACDeleteGroupTasksName As String = "DeleteGroupTasks"
    Public Const ACDeleteGroupTasksSQL As String = "spu_pmuser_group_activity_del"

    ' Add Group Tasks SQL
    'RFC300399
    Public Const ACAddGroupTaskStored As Boolean = True
    Public Const ACAddGroupTaskName As String = "AddGroupTask"
    Public Const ACAddGroupTaskSQL As String = "spu_pmuser_group_activity_add"

    ' Is The User In A System Administrator Group SQL
    'RFC300399
    Public Const ACIsSystemAdministratorStored As Boolean = True
    Public Const ACIsSystemAdministratorName As String = "IsSystemAdministrator"
    'Developer Guide No. 39
    Public Const ACIsSystemAdministratorSQL As String = "spu_pmuser_is_sysadmin"

    ' Is The User A Supervisor for the Group SQL
    'RFC300399
    Public Const ACIsSupervisorStored As Boolean = True
    Public Const ACIsSupervisorName As String = "IsSupervisor"
    Public Const ACIsSupervisorSQL As String = "spu_pmuser_is_supervisor"

    ' Is The User A Supervisor for the Group SQL
    Public Const ACUserSupervisesGroupsStored As Boolean = True
    Public Const ACUserSupervisesGroupsName As String = "UserSupervisesGroups"
    'Developer Guide No. 39
    Public Const ACUserSupervisesGroupsSQL As String = "spu_pmuser_supervises_groups"


    ' Is The User Id A Member of the Group SQL
    'DAK300899
    Public Const ACIsMemberIdStored As Boolean = True
    Public Const ACIsMemberIdName As String = "IsMemberId"
    Public Const ACIsMemberIdSQL As String = "spu_pmuser_is_id_member"

    ' Is The User Name A Member of the Group SQL
    'DAK300899
    Public Const ACIsMemberNameStored As Boolean = True
    Public Const ACIsMemberNameName As String = "IsMemberName"
    Public Const ACIsMemberNameSQL As String = "spu_pmuser_is_name_member"

    'DC260903 -PS256 -fsa compliance
    Public Const ACGetUserGroupInfoStored As Boolean = True
    Public Const ACGetUserGroupInfoName As String = "GetUserGroupInfo"
    Public Const ACGetUserGroupInfoSQL As String = "spu_get_pmuser_group_info"

    Public Const ACDeleteUserGroupInfoStored As Boolean = True
    Public Const ACDeleteUserGroupInfoName As String = "DeleteUserGroupInfo"
    Public Const ACDeleteUserGroupInfoSQL As String = "spu_delete_pmuser_group_info"

    Public Const ACUpdateUserGroupInfoStored As Boolean = True
    Public Const ACUpdateUserGroupInfoName As String = "UpdateUserGroupInfo"
    Public Const ACUpdateUserGroupInfoSQL As String = "spu_update_pmuser_group_info"
End Module