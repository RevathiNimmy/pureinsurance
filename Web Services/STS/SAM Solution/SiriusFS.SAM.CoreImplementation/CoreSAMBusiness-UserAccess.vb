Option Strict On
Option Explicit On

Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.Structure
Imports System.IO
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports Microsoft.ApplicationBlocks.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports dPMDAOBridge

Partial Public Class CoreSAMBusiness

    ''' <summary>  
    ''' Adds a new user group to the database and retrieves the newly added user group id. 
    ''' </summary>  
    ''' <param name="oAddUserGroupRequest">An object of type SiriusFS.SAM.Structure.BaseImplementationTypes.BaseAddUserGroupRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.Structure.BaseImplementationTypes.BaseAddUserGroupResponseType</returns>  
    Public Overloads Function AddUserGroup(ByVal oAddUserGroupRequest As BaseAddUserGroupRequestType) As BaseAddUserGroupResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oResponse As BaseAddUserGroupResponseType

            oResponse = AddUserGroup(con, oAddUserGroupRequest)

            Return oResponse

        End Using
    End Function

    ''' <summary>  
    ''' Adds a new user group to the database and retrieves the newly added user group id. 
    ''' </summary>  
    ''' <param name="oAddUserGroupRequest">An object of type SiriusFS.SAM.Structure.BaseImplementationTypes.BaseAddUserGroupRequestType</param>  
    ''' <param name="con">An object of type SiriusConnection</param>  
    ''' <returns>An object of type SiriusFS.SAM.Structure.BaseImplementationTypes.BaseAddUserGroupResponseType</returns>  
    Public Overloads Function AddUserGroup(ByVal con As SiriusConnection, ByVal oAddUserGroupRequest As BaseAddUserGroupRequestType) As BaseAddUserGroupResponseType

        Dim oAddUserGroupResponse As BaseImplementationTypes.BaseAddUserGroupResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim iBranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        'Variable required for caption
        Dim iCaptionId As Integer

        If oAddUserGroupRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddUserGroupRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oAddUserGroupResponse = New SAMForInsuranceV2ImplementationTypes.AddUserGroupResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        '**************************
        ' STRUCTURE VALIDATION 
        '**************************

        ' Validate the mandatory structure data (empty strings etc)
        oAddUserGroupRequest.Validate(CObj(oErrors))

        ' If there were any errors throw an exception
        oErrors.CheckForErrors()

        '**************************
        ' DATA VALIDATION 
        '**************************
        ' Lookup Codes to ensure they are valid
        Try
            iBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
                                                                    PMLookupTable.Source, _
                                                                    oAddUserGroupRequest.BranchCode, _
                                                                    "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                        "BranchCode", _
                                        oAddUserGroupRequest.BranchCode)
        End Try

        'Deviation from tech spec- Instead of spu_pm_caption stored procedure, spu_pm_caption_id_return is called as per discussion with Rahul
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pm_caption_id_return")

            cmd.AddInParameter("@language_id", SqlDbType.SmallInt).Value = _SiriusUser.LanguageID
            cmd.AddInParameter("@caption", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oAddUserGroupRequest.Description, Nothing)
            cmd.AddOutParameter("@caption_id", SqlDbType.Int)
            con.ExecuteNonQuery(cmd)
            iCaptionId = Cast.ToInt32(cmd.Parameters.Item("@caption_id").Value, 0)
        End Using

        'Add the new user group and get the user group id
        Dim dtUserGroup As New DataTable
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmuser_group_add")

            cmd.AddInParameter("@caption_id", SqlDbType.Int).Value = iCaptionId
            cmd.AddInParameter("@code", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(oAddUserGroupRequest.Code, Nothing)
            cmd.AddInParameter("@description", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oAddUserGroupRequest.Description, Nothing)
            cmd.AddInParameter("@is_deleted", SqlDbType.Int).Value = Cast.ToInt16(oAddUserGroupRequest.IsDeleted)
            cmd.AddInParameter("@effective_date", SqlDbType.DateTime).Value = oAddUserGroupRequest.EffectiveDate
            cmd.AddInParameter("@is_sys_admin_group", SqlDbType.Int).Value = Cast.ToInt16(oAddUserGroupRequest.IsSysAdmin)
            con.ExecuteDataTable(cmd, dtUserGroup)
        End Using

        If dtUserGroup IsNot Nothing AndAlso dtUserGroup.Rows.Count > 0 Then
            Dim drUserGroup As DataRow = dtUserGroup.Rows(0)
            'Since this stored procedure returns a result set with no column name, we are using column index to access value
            oAddUserGroupResponse.UserGroupKey = Cast.ToInt32(drUserGroup.Item(0), 0)
        End If

        Return oAddUserGroupResponse
    End Function

    ''' <summary>
    '''This method pass the request object UpdateUserGroup along with the connection object
    '''</summary>
    '''<param name="oUpdateUserGroupRequest" type="BaseUpdateUserGroupRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseUpdateUserGroupResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function UpdateUserGroup(ByVal oUpdateUserGroupRequest As BaseUpdateUserGroupRequestType) As BaseUpdateUserGroupResponseType

        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseUpdateUserGroupResponseType

            oResponse = UpdateUserGroup(con, oUpdateUserGroupRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method pass the request object to the stored procedure and timestamp as  response value
    '''</summary>
    '''<param name="oUpdateUserGroupRequest" type="BaseUpdateUserGroupRequestType"></param>
    '''<param name="con" type="SiriusConnection"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseUpdateUserGroupResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function UpdateUserGroup(ByVal con As SiriusConnection, ByVal oUpdateUserGroupRequest As BaseUpdateUserGroupRequestType) As BaseUpdateUserGroupResponseType

        Dim oUpdateUserGroupResponse As New BaseImplementationTypes.BaseUpdateUserGroupResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim ibranchId As Int32

        If oUpdateUserGroupRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateUserGroupRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oUpdateUserGroupResponse = New SAMForInsuranceV2ImplementationTypes.UpdateUserGroupResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oUpdateUserGroupRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "BranchCode")
        End If

        If oUpdateUserGroupRequest.UserGroupKey = 0 Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "UserGroupKey")
        End If
        If String.IsNullOrEmpty(oUpdateUserGroupRequest.Code) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "Code")
        End If

        If oUpdateUserGroupRequest.EffectiveDate = Date.MinValue OrElse oUpdateUserGroupRequest.EffectiveDate = Nothing Then
            oErrors.AddInvalidData( _
            SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            "EffectiveDate")
        End If
        'exit if there are any missing parameters
        oErrors.CheckForErrors()
        ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
          PMLookupTable.Source, oUpdateUserGroupRequest.BranchCode, "BranchCode", oErrors)

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        ' Check that the timestamp matches and lock the task instance
        Try
            con.BeginTransaction() 'Said by Rahul

            Dim dsGetCaptionID As DataSet = Nothing
            Using cmdGetCaptionID As SiriusCommand = SiriusCommand.FromProcedure("spu_pm_caption")
                cmdGetCaptionID.AddInParameter("@language_id", SqlDbType.TinyInt).Value = _SiriusUser.LanguageID
                cmdGetCaptionID.AddInParameter("@caption", SqlDbType.VarChar, 255).Value = oUpdateUserGroupRequest.Description
                dsGetCaptionID = con.ExecuteDataSet(cmdGetCaptionID, "CaptionID")
            End Using
            Using cmdUpdateUserGroup As SiriusCommand = SiriusCommand.FromProcedure("spu_pmuser_group_upd")
                cmdUpdateUserGroup.AddInParameter("@pmuser_group_id", SqlDbType.Int).Value = oUpdateUserGroupRequest.UserGroupKey
                cmdUpdateUserGroup.AddInParameter("@caption_id", SqlDbType.Int).Value = Cast.ToInt32(dsGetCaptionID.Tables(0).Rows(0).Item("caption_id"), 0)
                cmdUpdateUserGroup.AddInParameter("@code", SqlDbType.VarChar, 10).Value = oUpdateUserGroupRequest.Code
                cmdUpdateUserGroup.AddInParameter("@description", SqlDbType.VarChar, 255).Value = oUpdateUserGroupRequest.Description
                cmdUpdateUserGroup.AddInParameter("@is_deleted", SqlDbType.TinyInt).Value = Convert.ToByte(oUpdateUserGroupRequest.IsDeleted)
                cmdUpdateUserGroup.AddInParameter("@effective_date", SqlDbType.DateTime).Value = oUpdateUserGroupRequest.EffectiveDate
                cmdUpdateUserGroup.AddInParameter("@is_sys_admin_group", SqlDbType.Int).Value = Convert.ToInt32(oUpdateUserGroupRequest.IsSysAdmin)
                con.ExecuteNonQuery(cmdUpdateUserGroup)
            End Using
            con.CommitTransaction()
        Catch ex As Exception
            con.RollbackTransaction()
        End Try

        Return oUpdateUserGroupResponse
    End Function

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the user details
    '''</summary>
    '''<param name="oUpdateUserGroupUsersRequest" type="BaseUpdateUserGroupUsersRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseUpdateUserGroupUsersResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function UpdateUserGroupUsers(ByVal oUpdateUserGroupUsersRequest As BaseUpdateUserGroupUsersRequestType) As BaseUpdateUserGroupUsersResponseType
        Using conUsers As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oResponse As BaseUpdateUserGroupUsersResponseType

            oResponse = UpdateUserGroupUsers(conUsers, oUpdateUserGroupUsersRequest)

            Return oResponse

        End Using

    End Function
    Public Overloads Function UpdateUserGroupUsers(ByVal conUsers As SiriusConnection, ByVal oUpdateUserGroupUsersRequest As BaseUpdateUserGroupUsersRequestType) As BaseUpdateUserGroupUsersResponseType

        Dim oUpdateUserGroupUsersResponse As New BaseImplementationTypes.BaseUpdateUserGroupUsersResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim branchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oUpdateUserGroupUsersRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateUserGroupUsersRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oUpdateUserGroupUsersResponse = New SAMForInsuranceV2ImplementationTypes.UpdateUserGroupUsersResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oUpdateUserGroupUsersRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "BranchCode")
        End If
        Try
            branchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oUpdateUserGroupUsersRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                        "BranchCode", _
                                        oUpdateUserGroupUsersRequest.BranchCode)
        End Try

        If oUpdateUserGroupUsersRequest.UserGroupKey = 0 Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "UserGroupKey")
        End If
        If oUpdateUserGroupUsersRequest.Users IsNot Nothing AndAlso oUpdateUserGroupUsersRequest.Users.Row IsNot Nothing Then
            For iCount As Integer = 0 To oUpdateUserGroupUsersRequest.Users.Row.Length - 1
                If oUpdateUserGroupUsersRequest.Users.Row(iCount).UserKey = 0 Then
                    oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                                SAMInvalidData.MandatoryInputMissing.ToString, _
                                                "UserKey")
                End If
            Next
        End If

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        'variation from the tech spec (as said by gaurav)
        Dim oAnyError As STSErrorType
        oAnyError = oCoreBusiness.CheckTSAndLock( _
            BranchCode:=oUpdateUserGroupUsersRequest.BranchCode, _
            Lockname:=CoreBusiness.LockName.UserGroupCnt, _
            LockValue:=oUpdateUserGroupUsersRequest.UserGroupKey, _
            TStamp:=oUpdateUserGroupUsersRequest.TimeStamp)

        ' Check for Errors
        If oAnyError Is Nothing = False Then
            ' Either the timestamp didn't match or the record couldn't be locked for some reason, return the error.
            oUpdateUserGroupUsersResponse.STSError = oAnyError
            Return oUpdateUserGroupUsersResponse
        End If
        Try

            conUsers.BeginTransaction() 'Said by Rahul

            'Call the stored procedure  spu_pmuser_group_user_del with the UserGroupKey parameter
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmuser_group_user_del")

                cmd.AddInParameter("@pmuser_group_id", SqlDbType.Int).Value = oUpdateUserGroupUsersRequest.UserGroupKey
                conUsers.ExecuteNonQuery(cmd)

            End Using

            'Loop through all users in the request type, and call the stored procedure  spu_pmuser_group_user_add with all the input parameters
            If oUpdateUserGroupUsersRequest.Users IsNot Nothing AndAlso oUpdateUserGroupUsersRequest.Users.Row IsNot Nothing Then
                For iCount As Integer = 0 To oUpdateUserGroupUsersRequest.Users.Row.Length - 1
                    Using cmdUpdateUser As SiriusCommand = SiriusCommand.FromProcedure("spu_pmuser_group_user_add")
                        cmdUpdateUser.Parameters.Clear()
                        cmdUpdateUser.AddInParameter("@pmuser_group_id", SqlDbType.Int).Value = oUpdateUserGroupUsersRequest.UserGroupKey
                        cmdUpdateUser.AddInParameter("@user_id", SqlDbType.Int).Value = Cast.ToInt16(oUpdateUserGroupUsersRequest.Users.Row(iCount).UserKey)
                        If oUpdateUserGroupUsersRequest.Users.Row(iCount).DisplaySequenceSpecified Then
                            cmdUpdateUser.AddInParameter("@display_sequence_num", SqlDbType.SmallInt).Value = Cast.ToInt16(oUpdateUserGroupUsersRequest.Users.Row(iCount).DisplaySequence, 0)
                        Else
                            cmdUpdateUser.AddInParameter("@display_sequence_num", SqlDbType.SmallInt).Value = 0
                        End If
                        cmdUpdateUser.AddInParameter("@is_supervisor", SqlDbType.Int).Value = Cast.ToInt16(oUpdateUserGroupUsersRequest.Users.Row(iCount).IsSupervisor, 0)
                        conUsers.ExecuteNonQuery(cmdUpdateUser)
                    End Using
                Next
            End If

            conUsers.CommitTransaction()
        Catch ex As Exception
            conUsers.RollbackTransaction()
        End Try

        'variation from the tech spec (as said by gaurav)
        oAnyError = oCoreBusiness.UnlockAndGetTS( _
       BranchCode:=oUpdateUserGroupUsersRequest.BranchCode, _
       Lockname:=CoreBusiness.LockName.UserGroupCnt, _
       LockValue:=oUpdateUserGroupUsersRequest.UserGroupKey, _
       TStamp:=oUpdateUserGroupUsersResponse.TimeStamp)

        ' Check for Errors
        If oAnyError Is Nothing = False Then
            ' Unable to unlock, return the error.
            oUpdateUserGroupUsersResponse.STSError = oAnyError
            Return oUpdateUserGroupUsersResponse
        End If

        Return oUpdateUserGroupUsersResponse

    End Function

    ''' <summary>
    '''This method pass the request object GetUserGroups along with the connection object
    '''</summary>
    '''<param name="oGetUserGroupsRequest" type="BaseGetUserGroupsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetUserGroupsResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function GetUserGroups(ByVal oGetUserGroupsRequest As BaseGetUserGroupsRequestType) As BaseGetUserGroupsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseGetUserGroupsResponseType
            oResponse = GetUserGroups(con, oGetUserGroupsRequest)
            Return oResponse
        End Using
    End Function

    Public Overloads Function GetUserGroups(ByVal con As SiriusConnection, ByVal oGetUserGroupsRequest As BaseGetUserGroupsRequestType) As BaseGetUserGroupsResponseType

        Dim oGetUserGroupsResponse As New BaseImplementationTypes.BaseGetUserGroupsResponseType

        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim lbranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetUserGroupsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetUserGroupsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetUserGroupsResponse = New SAMForInsuranceV2ImplementationTypes.GetUserGroupsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oGetUserGroupsRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "BranchCode")
        End If

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()
        'Lookup Codes to ensure they are valid
        Try
            lbranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oGetUserGroupsRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                        "BranchCode", _
                                        oGetUserGroupsRequest.BranchCode)
        End Try
        ' Dataset that will hold the returned results		
        Dim dsUserGroups As DataSet = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmuser_group")
            If oGetUserGroupsRequest.Username IsNot Nothing AndAlso Not String.IsNullOrEmpty(oGetUserGroupsRequest.Username) Then
                cmd.AddInParameter("@UserName", SqlDbType.VarChar).Value = oGetUserGroupsRequest.Username
            End If
            dsUserGroups = con.ExecuteDataSet(cmd, "Row")
        End Using
        Dim dsFilter As DataSet = Nothing
        dsFilter = New DataSet
        With dsUserGroups.Tables(0)
            dsFilter.Tables.Add()
            dsFilter.Tables(0).Columns.Add("UserGroupKey", GetType(System.Int16))
            dsFilter.Tables(0).Columns.Add("Code", GetType(System.String))
            dsFilter.Tables(0).Columns.Add("Description", GetType(System.String))
            dsFilter.Tables(0).Columns.Add("IsDeleted", GetType(System.Boolean))
            dsFilter.Tables(0).Columns.Add("IsSystemAdmin", GetType(System.Boolean))
            dsFilter.Tables(0).Columns.Add("EffectiveDate", GetType(System.DateTime))
            dsFilter.Tables(0).Columns.Add("IsDebtorPMUserGroup", GetType(System.Boolean))

        End With
        Dim iCount As Integer
        For iCount = 0 To dsUserGroups.Tables(0).Rows.Count - 1
            dsFilter.Tables(0).Rows.Add()
            dsFilter.Tables(0).Rows(iCount)("UserGroupKey") = Cast.ToInt16(dsUserGroups.Tables(0).Rows(iCount).Item("id"), 0)
            dsFilter.Tables(0).Rows(iCount)("Code") = dsUserGroups.Tables(0).Rows(iCount).Item("code").ToString
            dsFilter.Tables(0).Rows(iCount)("Description") = dsUserGroups.Tables(0).Rows(iCount).Item("description").ToString
            dsFilter.Tables(0).Rows(iCount)("IsDeleted") = Cast.ToBoolean(dsUserGroups.Tables(0).Rows(iCount).Item("is_deleted"), False)
            dsFilter.Tables(0).Rows(iCount)("IsSystemAdmin") = Cast.ToBoolean(dsUserGroups.Tables(0).Rows(iCount).Item("is_sys_admin_group"), False)
            dsFilter.Tables(0).Rows(iCount)("EffectiveDate") = Cast.ToDateTime(dsUserGroups.Tables(0).Rows(iCount).Item("effective_date"), Date.Now)
            dsFilter.Tables(0).Rows(iCount)("IsDebtorPMUserGroup") = Cast.ToBoolean(dsUserGroups.Tables(0).Rows(iCount).Item("Is_debtor_pmuser_group"), False)

        Next

        If (dsFilter.Tables.Count > 0) Then
            If (dsFilter.Tables(0).Rows.Count > 0) Then

                Dim xmlDoc As New System.Xml.XmlDocument
                dsFilter.DataSetName = "BaseGetUserGroupsResponseTypeUserGroups"
                dsFilter.Tables(0).TableName = "Row"

                If oGetUserGroupsRequest.WCFSecurityToken = "" Then
                    xmlDoc.LoadXml(dsFilter.GetXml)
                    If (dsFilter.Tables.Count >= 1) Then
                        'Populate the XML Document here with the dsUserGroups information
                        oGetUserGroupsResponse.ResultDataset = xmlDoc.DocumentElement()

                    End If
                End If
                oGetUserGroupsResponse.ResultData = dsFilter
            End If
        End If

        Return oGetUserGroupsResponse

    End Function



    ''' <summary>
    '''This method pass the request object GetTaskGroups along with the connection object
    '''</summary>
    '''<param name="oGetTaskGroupsRequest" type="BaseGetTaskGroupsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetTaskGroupsResponseType</returns>
    '''<remarks></remarks>

    Public Overloads Function GetTaskGroups(ByVal oGetTaskGroupsRequest As BaseGetTaskGroupsRequestType) As BaseGetTaskGroupsResponseType

        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseGetTaskGroupsResponseType

            oResponse = GetTaskGroups(con, oGetTaskGroupsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method pass the request object to the stored procedure and resultant value as dataset and pass this as XML back
    '''</summary>
    '''<param name="oGetTaskGroupsRequest" type="BaseGetTaskGroupsRequestType"></param>
    '''<param name="con" type="SiriusConnection"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetTaskGroupsResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function GetTaskGroups(ByVal con As SiriusConnection, ByVal oGetTaskGroupsRequest As BaseGetTaskGroupsRequestType) As BaseGetTaskGroupsResponseType

        Dim oGetTaskGroupsResponse As New BaseImplementationTypes.BaseGetTaskGroupsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim ibranchId As Int32

        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetTaskGroupsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetTaskGroupsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oGetTaskGroupsResponse = New SAMForInsuranceV2ImplementationTypes.GetTaskGroupsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oGetTaskGroupsRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "BranchCode")
        End If

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
        PMLookupTable.Source, oGetTaskGroupsRequest.BranchCode, "BranchCode", oErrors)

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        ' Dataset that will hold the returned results		
        Dim dsTaskGroups As DataSet = Nothing
        Dim dsResultantDataSet As New DataSet
        Dim icount As Int32

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmwrk_task_group")

            dsTaskGroups = con.ExecuteDataSet(cmd, "Row")

        End Using
        With dsTaskGroups.Tables(0)
            dsResultantDataSet.Tables.Add()
            dsResultantDataSet.Tables(0).Columns.Add("TaskGroupKey", GetType(System.Int32))
            dsResultantDataSet.Tables(0).Columns.Add("Code", GetType(System.String))
            dsResultantDataSet.Tables(0).Columns.Add("Description", GetType(System.String))
            dsResultantDataSet.Tables(0).Columns.Add("IsDeleted", GetType(System.Boolean))
            dsResultantDataSet.Tables(0).Columns.Add("TaskGroupCategoryKey", GetType(System.Int32))
            dsResultantDataSet.Tables(0).Columns.Add("EffectiveDate", GetType(System.DateTime))
            dsResultantDataSet.Tables(0).Columns.Add("CaptionID", GetType(System.Int32))

            For icount = 0 To dsTaskGroups.Tables(0).Rows.Count - 1
                'Data are extracted from the DataSet and put it into corresponding objects
                dsResultantDataSet.Tables(0).Rows.Add()
                dsResultantDataSet.Tables(0).Rows(icount)("TaskGroupKey") = dsTaskGroups.Tables(0).Rows(icount).Item("id")
                dsResultantDataSet.Tables(0).Rows(icount)("Code") = dsTaskGroups.Tables(0).Rows(icount).Item("code")
                dsResultantDataSet.Tables(0).Rows(icount)("Description") = dsTaskGroups.Tables(0).Rows(icount).Item("description")
                dsResultantDataSet.Tables(0).Rows(icount)("IsDeleted") = Cast.ToBoolean(dsTaskGroups.Tables(0).Rows(icount).Item("is_deleted"))
                dsResultantDataSet.Tables(0).Rows(icount)("TaskGroupCategoryKey") = dsTaskGroups.Tables(0).Rows(icount).Item("display_icon")
                dsResultantDataSet.Tables(0).Rows(icount)("EffectiveDate") = dsTaskGroups.Tables(0).Rows(icount).Item("effective_date")
                dsResultantDataSet.Tables(0).Rows(icount)("CaptionID") = dsTaskGroups.Tables(0).Rows(icount).Item("caption_id")

            Next

        End With

        Dim xmlDoc As New System.Xml.XmlDocument
        dsResultantDataSet.DataSetName = "BaseGetTaskGroupsResponseTypeTaskGroups"
        dsResultantDataSet.Tables(0).TableName = "Row"
        If (dsResultantDataSet.Tables(0).Rows.Count >= 1) Then
            If oGetTaskGroupsRequest.WCFSecurityToken = "" Then
                xmlDoc.LoadXml(dsResultantDataSet.GetXml)
                oGetTaskGroupsResponse.ResultDataset = xmlDoc.DocumentElement()
            End If
            oGetTaskGroupsResponse.ResultData = dsResultantDataSet
        End If
        Return oGetTaskGroupsResponse
    End Function

    '''<summary>
    ''' This is core SAM method for AddTaskGroup
    '''</summary>
    '''<param name="AddTaskGroupRequest" type="BaseAddTaskGroupRequestType"></param>   
    '''<returns>BaseAddTaskGroupResponseType</returns>
    '''<remarks></remarks> 
    Public Overloads Function AddTaskGroup(ByVal AddTaskGroupRequest As BaseAddTaskGroupRequestType) As BaseAddTaskGroupResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseAddTaskGroupResponseType

            oResponse = AddTaskGroup(con, AddTaskGroupRequest)

            Return oResponse

        End Using

    End Function

    '''<summary>
    ''' This is core SAM method for AddTaskGroup
    '''</summary>
    '''<param name="con" type="SiriusConnection"></param>   
    '''<param name="oAddTaskGroupRequest" type="BaseAddTaskGroupRequestType"></param>   
    '''<returns>BaseAddTaskGroupResponseType</returns>
    '''<remarks></remarks> 
    Public Overloads Function AddTaskGroup(ByVal con As SiriusConnection, ByVal oAddTaskGroupRequest As BaseAddTaskGroupRequestType) As BaseAddTaskGroupResponseType

        Dim oAddTaskGroupResponse As New BaseAddTaskGroupResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim branchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iCaptionId As Integer

        If oAddTaskGroupRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddTaskGroupRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oAddTaskGroupResponse = New SAMForInsuranceV2ImplementationTypes.AddTaskGroupResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oAddTaskGroupRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "BranchCode")
        End If
        If String.IsNullOrEmpty(oAddTaskGroupRequest.Code) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "Code")
        End If
        If oAddTaskGroupRequest.EffectiveDate <= Date.MinValue Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "EffectiveDate")
        End If
        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid
        Try
            branchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oAddTaskGroupRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                        "BranchCode", _
                                        oAddTaskGroupRequest.BranchCode)
        End Try

        Dim dsGetCaptionID As DataSet = Nothing
        Dim dsGetTaskGrpId As DataSet = Nothing
        Dim dsTaskGrpCodeCheck As DataSet = Nothing
        Using cmdGetCaptionID As SiriusCommand = SiriusCommand.FromProcedure("spu_pm_caption")
            cmdGetCaptionID.AddInParameter("@language_id", SqlDbType.TinyInt).Value = _SiriusUser.LanguageID
            cmdGetCaptionID.AddInParameter("@caption", SqlDbType.VarChar, 255).Value = oAddTaskGroupRequest.Description
            dsGetCaptionID = con.ExecuteDataSet(cmdGetCaptionID, "CaptionID")
        End Using

        If dsGetCaptionID.Tables(0).Rows.Count = 1 Then
            iCaptionId = Convert.ToInt32(dsGetCaptionID.Tables(0).Rows(0).Item("caption_id").ToString)

            Using cmdCheckGroupCode As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_pmwrk_task_group_code_check")
                cmdCheckGroupCode.AddInParameter("@Code", SqlDbType.VarChar, 10).Value = oAddTaskGroupRequest.Code
                dsTaskGrpCodeCheck = con.ExecuteDataSet(cmdCheckGroupCode, "TaskGrpCodeCheck")
            End Using

            If dsTaskGrpCodeCheck.Tables(0).Rows.Count = 0 Then
                Using cmdGetTaskGroupId As SiriusCommand = SiriusCommand.FromProcedure("spu_pmwrk_task_group_add")
                    cmdGetTaskGroupId.AddInParameter("@caption_id", SqlDbType.Int).Value = iCaptionId
                    cmdGetTaskGroupId.AddInParameter("@code", SqlDbType.VarChar, 10).Value = oAddTaskGroupRequest.Code
                    cmdGetTaskGroupId.AddInParameter("@description", SqlDbType.VarChar, 255).Value = oAddTaskGroupRequest.Description
                    cmdGetTaskGroupId.AddInParameter("@is_deleted", SqlDbType.TinyInt).Value = IIf(oAddTaskGroupRequest.IsDeleted, 1, 0)
                    cmdGetTaskGroupId.AddInParameter("@effective_date", SqlDbType.DateTime).Value = oAddTaskGroupRequest.EffectiveDate
                    cmdGetTaskGroupId.AddInParameter("@display_icon", SqlDbType.Int).Value = oAddTaskGroupRequest.TaskGroupCategoryKey
                    dsGetTaskGrpId = con.ExecuteDataSet(cmdGetTaskGroupId, "TaskGrpId")
                End Using
            Else
                oErrors.AddInvalidData(SAMInvalidData.CodeAlreadyExists, _
                                        SAMInvalidData.CodeAlreadyExists.ToString, _
                                        "Code", _
                                        oAddTaskGroupRequest.Code)
                oErrors.CheckForErrors()
            End If
        End If

        If dsGetTaskGrpId.Tables(0).Rows.Count = 1 Then
            oAddTaskGroupResponse.TaskGroupKey = Convert.ToInt32(dsGetTaskGrpId.Tables(0).Rows(0).Item("pmwrk_task_group_id").ToString())
            Dim oAnyError As STSErrorType
            Dim bIsLocked As Boolean
            oAnyError = oCoreBusiness.GetTimestamp(con, _
                                oAddTaskGroupRequest.BranchCode, _
                                CoreBusiness.LockName.TaskGroupCnt, _
                                 oAddTaskGroupResponse.TaskGroupKey, _
                                oAddTaskGroupResponse.TimeStamp, _
                                bIsLocked)
            ' Return AnyErrors
            If oAnyError Is Nothing = False Then
                oAddTaskGroupResponse.STSError = oAnyError
            End If
        End If
        Return oAddTaskGroupResponse
    End Function

    ''' <summary>  
    '''  This method is used to Delete-Undelete User Group.
    ''' </summary>  
    ''' <param name="DeleteUndeleteUserGroupRequest">An object of type SiriusFS.SAM.BaseImplementationTypes.BaseDeleteUndeleteUserGroupRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.BaseImplementationTypes.BaseDeleteUndeleteUserGroupResponseType</returns>  
    Public Overloads Function DeleteUndeleteUserGroup(ByVal DeleteUndeleteUserGroupRequest As BaseDeleteUndeleteUserGroupRequestType) As BaseDeleteUndeleteUserGroupResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseDeleteUndeleteUserGroupResponseType

            oResponse = DeleteUndeleteUserGroup(con, DeleteUndeleteUserGroupRequest)

            Return oResponse

        End Using

    End Function

    Public Overloads Function DeleteUndeleteUserGroup(ByVal con As SiriusConnection, ByVal oDeleteUndeleteUserGroupRequest As BaseDeleteUndeleteUserGroupRequestType) As BaseDeleteUndeleteUserGroupResponseType

        Dim oDeleteUndeleteUserGroupResponse As New BaseImplementationTypes.BaseDeleteUndeleteUserGroupResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim branchId As Integer
        Dim iUserGroupKey As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oDeleteUndeleteUserGroupRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.DeleteUndeleteUserGroupRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oDeleteUndeleteUserGroupResponse = New SAMForInsuranceV2ImplementationTypes.DeleteUndeleteUserGroupResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oDeleteUndeleteUserGroupRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "BranchCode")
        End If

        If String.IsNullOrEmpty(oDeleteUndeleteUserGroupRequest.UserGroupCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "UserGroupCode")
        End If

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid
        Try
            branchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oDeleteUndeleteUserGroupRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                        "BranchCode", _
                                        oDeleteUndeleteUserGroupRequest.BranchCode)
        End Try
        Try
            iUserGroupKey = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.UserGroup, oDeleteUndeleteUserGroupRequest.UserGroupCode, "UserGroupCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                        "UserGroupCode", _
                                        oDeleteUndeleteUserGroupRequest.UserGroupCode)
        End Try

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmuser_group_del")
            cmd.AddInParameter("@pmuser_group_id", SqlDbType.Int).Value = iUserGroupKey
            cmd.AddInParameter("@is_deleted", SqlDbType.TinyInt).Value = Convert.ToByte(oDeleteUndeleteUserGroupRequest.Deleted)
            con.ExecuteNonQuery(cmd)
        End Using

        Return oDeleteUndeleteUserGroupResponse
    End Function

    ''' <summary>  
    '''  This method is used to update User Groups
    '''<param name="UpdateTaskGroupsRequest" type="BaseUpdateTaskGroupsRequestType"></param>   
    '''<returns>BaseUpdateTaskGroupsResponseType</returns>
    '''<remarks></remarks>   
    Public Overloads Function UpdateTaskGroups(ByVal UpdateTaskGroupsRequest As BaseUpdateTaskGroupsRequestType) As BaseUpdateTaskGroupsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseUpdateTaskGroupsResponseType

            oResponse = UpdateTaskGroups(con, UpdateTaskGroupsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>  
    '''  This method is used to update User Groups
    '''<param name="oUpdateTaskGroupsRequest" type="BaseUpdateTaskGroupsRequestType"></param>   
    '''<returns>BaseUpdateTaskGroupsResponseType</returns>
    '''<remarks></remarks>   

    Public Overloads Function UpdateTaskGroups(ByVal con As SiriusConnection, ByVal oUpdateTaskGroupsRequest As BaseUpdateTaskGroupsRequestType) As BaseUpdateTaskGroupsResponseType

        Dim oUpdateTaskGroupsResponse As New BaseUpdateTaskGroupsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim branchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iCaptionId As Integer

        If oUpdateTaskGroupsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateTaskGroupsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oUpdateTaskGroupsResponse = New SAMForInsuranceV2ImplementationTypes.UpdateTaskGroupsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields, i.e.
        If String.IsNullOrEmpty(oUpdateTaskGroupsRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "BranchCode")
        End If
        If oUpdateTaskGroupsRequest.TaskGroupKey = 0 Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "TaskGroupKey")
        End If
        If String.IsNullOrEmpty(oUpdateTaskGroupsRequest.Code) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "Code")
        End If
        If oUpdateTaskGroupsRequest.CaptionId = 0 Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "CaptionId")
        End If
        If oUpdateTaskGroupsRequest.TaskGroupCategoryKey = 0 Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "TaskGroupCategoryKey")
        End If

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid, i.e.
        Try
            branchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oUpdateTaskGroupsRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                        "BranchCode", _
                                        oUpdateTaskGroupsRequest.BranchCode)
        End Try

        'India – Loop through all users in the request type, the stored procedure spu_pm_caption passing in the Description field, and the user’s language key ( _SiriusUser.LanguageID ). Retrieve back the captionId. Then call the stored procedure  spu_pmwrk_task_group_upd with all the input parameters
        Try
            con.BeginTransaction() 'Said by Rahul

            Dim dsGetCaptionID As DataSet = Nothing
            Using cmdGetCaptionID As SiriusCommand = SiriusCommand.FromProcedure("spu_pm_caption")
                cmdGetCaptionID.AddInParameter("@language_id", SqlDbType.TinyInt).Value = _SiriusUser.LanguageID
                cmdGetCaptionID.AddInParameter("@caption", SqlDbType.VarChar, 255).Value = oUpdateTaskGroupsRequest.Description
                dsGetCaptionID = con.ExecuteDataSet(cmdGetCaptionID, "CaptionID")
            End Using

            If dsGetCaptionID.Tables(0).Rows.Count = 1 Then
                iCaptionId = Convert.ToInt32(dsGetCaptionID.Tables(0).Rows(0).Item("caption_id").ToString)

                Using cmdUpdateTaskGroups As SiriusCommand = SiriusCommand.FromProcedure("spu_pmwrk_task_group_upd")
                    cmdUpdateTaskGroups.AddInParameter("@pmwrk_task_group_id", SqlDbType.Int).Value = oUpdateTaskGroupsRequest.TaskGroupKey
                    'cmdUpdateTaskGroups.AddInParameter("@caption_id", SqlDbType.Int).Value = Cast.ToInt32(dsGetCaptionID.Tables(0).Rows(0).Item("caption_id"), 0)
                    cmdUpdateTaskGroups.AddInParameter("@caption_id", SqlDbType.Int).Value = iCaptionId
                    cmdUpdateTaskGroups.AddInParameter("@code", SqlDbType.VarChar, 10).Value = oUpdateTaskGroupsRequest.Code
                    cmdUpdateTaskGroups.AddInParameter("@description", SqlDbType.VarChar, 255).Value = oUpdateTaskGroupsRequest.Description
                    cmdUpdateTaskGroups.AddInParameter("@is_deleted", SqlDbType.TinyInt).Value = Convert.ToByte(oUpdateTaskGroupsRequest.IsDeleted)
                    cmdUpdateTaskGroups.AddInParameter("@effective_date", SqlDbType.DateTime).Value = oUpdateTaskGroupsRequest.EffectiveDate
                    cmdUpdateTaskGroups.AddInParameter("@display_icon", SqlDbType.Int).Value = Convert.ToInt32(oUpdateTaskGroupsRequest.TaskGroupCategoryKey)
                    con.ExecuteNonQuery(cmdUpdateTaskGroups)
                End Using
            End If
            con.CommitTransaction()
        Catch ex As Exception
            con.RollbackTransaction()
        End Try
        'Set the response value

        Return oUpdateTaskGroupsResponse

    End Function

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the task details
    '''</summary>
    '''<param name="oUpdateTaskGroupTasksRequest" type="BaseUpdateTaskGroupTasksRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseUpdateTaskGroupTasksResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function UpdateTaskGroupTasks(ByVal oUpdateTaskGroupTasksRequest As BaseUpdateTaskGroupTasksRequestType) As BaseUpdateTaskGroupTasksResponseType
        Using conTask As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseUpdateTaskGroupTasksResponseType

            oResponse = UpdateTaskGroupTasks(conTask, oUpdateTaskGroupTasksRequest)

            Return oResponse

        End Using

    End Function
    Public Overloads Function UpdateTaskGroupTasks(ByVal conTask As SiriusConnection, ByVal oUpdateTaskGroupTasksRequest As BaseUpdateTaskGroupTasksRequestType) As BaseUpdateTaskGroupTasksResponseType

        Dim oUpdateTaskGroupTasksResponse As New BaseImplementationTypes.BaseUpdateTaskGroupTasksResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim branchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oUpdateTaskGroupTasksRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateTaskGroupTasksRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oUpdateTaskGroupTasksResponse = New SAMForInsuranceV2ImplementationTypes.UpdateTaskGroupTasksResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oUpdateTaskGroupTasksRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "BranchCode")
        End If

        If oUpdateTaskGroupTasksRequest.TaskGroupKey = 0 Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "TaskGroupKey")
        End If
        If oUpdateTaskGroupTasksRequest.Tasks IsNot Nothing AndAlso oUpdateTaskGroupTasksRequest.Tasks.Row IsNot Nothing Then
            For iCount As Integer = 0 To oUpdateTaskGroupTasksRequest.Tasks.Row.Length - 1
                If oUpdateTaskGroupTasksRequest.Tasks.Row(iCount).TaskKey = 0 Then
                    oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                                SAMInvalidData.MandatoryInputMissing.ToString, _
                                                "TaskKey")
                End If
            Next
        End If

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid
        Try
            branchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oUpdateTaskGroupTasksRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                        "BranchCode", _
                                        oUpdateTaskGroupTasksRequest.BranchCode)
        End Try

        ' Check that the timestamp matches and lock the task instance 
        Dim oAnyError As STSErrorType
        oAnyError = oCoreBusiness.CheckTSAndLock( _
           BranchCode:=oUpdateTaskGroupTasksRequest.BranchCode, _
           Lockname:=CoreBusiness.LockName.TaskGroupCnt, _
           LockValue:=oUpdateTaskGroupTasksRequest.TaskGroupKey, _
           TStamp:=oUpdateTaskGroupTasksRequest.TimeStamp)

        ' Check for Errors
        If oAnyError Is Nothing = False Then
            ' Either the timestamp didn't match or the record couldn't be locked for some reason, return the error.
            oUpdateTaskGroupTasksResponse.STSError = oAnyError
            Return oUpdateTaskGroupTasksResponse
        End If
        Try

            conTask.BeginTransaction() 'Said by Rahul

            'Call the stored procedure  spu_SAM_pmwrk_task_group_tasks_del with the TaskGroupKey parameter
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmwrk_task_group_tasks_del")

                cmd.AddInParameter("@pmwrk_task_group_id", SqlDbType.Int).Value = oUpdateTaskGroupTasksRequest.TaskGroupKey
                conTask.ExecuteNonQuery(cmd)

            End Using
            'Loop through all users in the request type, and call the stored procedure  spu_SAM_pmwrk_task_group_tasks_add with all the input parameters
            If oUpdateTaskGroupTasksRequest.Tasks IsNot Nothing AndAlso oUpdateTaskGroupTasksRequest.Tasks.Row IsNot Nothing Then
                For iCount As Integer = 0 To oUpdateTaskGroupTasksRequest.Tasks.Row.Length - 1
                    Using cmdUpdateTask As SiriusCommand = SiriusCommand.FromProcedure("spu_pmwrk_task_group_task_add")
                        cmdUpdateTask.Parameters.Clear()
                        cmdUpdateTask.AddInParameter("@pmwrk_task_group_id", SqlDbType.Int).Value = oUpdateTaskGroupTasksRequest.TaskGroupKey
                        cmdUpdateTask.AddInParameter("@pmwrk_task_id", SqlDbType.Int).Value = oUpdateTaskGroupTasksRequest.Tasks.Row(iCount).TaskKey
                        If oUpdateTaskGroupTasksRequest.Tasks.Row(iCount).DisplaySequenceSpecified Then
                            cmdUpdateTask.AddInParameter("@display_sequence_num", SqlDbType.Int).Value = oUpdateTaskGroupTasksRequest.Tasks.Row(iCount).DisplaySequence
                        Else
                            cmdUpdateTask.AddInParameter("@display_sequence_num", SqlDbType.Int).Value = 0
                        End If
                        conTask.ExecuteNonQuery(cmdUpdateTask)
                    End Using
                Next
            End If

            conTask.CommitTransaction()
        Catch ex As Exception
            conTask.RollbackTransaction()
        End Try

        'variation from the tech spec (as said by gaurav)
        oAnyError = oCoreBusiness.UnlockAndGetTS( _
       BranchCode:=oUpdateTaskGroupTasksRequest.BranchCode, _
       Lockname:=CoreBusiness.LockName.TaskGroupCnt, _
       LockValue:=oUpdateTaskGroupTasksRequest.TaskGroupKey, _
       TStamp:=oUpdateTaskGroupTasksResponse.TimeStamp)

        ' Check for Errors
        If oAnyError Is Nothing = False Then
            ' Unable to unlock, return the error.
            oUpdateTaskGroupTasksResponse.STSError = oAnyError
            Return oUpdateTaskGroupTasksResponse
        End If

        Return oUpdateTaskGroupTasksResponse

    End Function

    ''' <summary>
    '''This method pass the request object GetTaskGroups along with the connection object
    '''</summary>
    '''<param name="oGetTaskGroupTasksRequest" type="BaseGetTaskGroupTasksRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetTaskGroupTasksResponseType</returns>
    '''<remarks></remarks>

    Public Overloads Function GetTaskGroupTasks(ByVal oGetTaskGroupTasksRequest As BaseGetTaskGroupTasksRequestType) As BaseGetTaskGroupTasksResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                           _SiriusUser.Username, _SiriusUser.SourceID, _
                                           _SiriusUser.LanguageID, _
                                           SiriusUserDefaults.AppName)

            Dim oResponse As BaseGetTaskGroupTasksResponseType

            oResponse = GetTaskGroupTasks(con, oGetTaskGroupTasksRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method pass the request object to the stored procedure and resultant value as dataset and pass this as XML back
    '''</summary>
    '''<param name="oGetTaskGroupTasksRequest" type="BaseGetTaskGroupTasksRequestType"></param>
    '''<param name="con" type="SiriusConnection"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetTaskGroupTasksResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function GetTaskGroupTasks(ByVal con As SiriusConnection, ByVal oGetTaskGroupTasksRequest As BaseGetTaskGroupTasksRequestType) As BaseGetTaskGroupTasksResponseType

        Dim oGetTaskGroupTasksResponse As New BaseImplementationTypes.BaseGetTaskGroupTasksResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim ibranchId As Int32
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetTaskGroupTasksRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetTaskGroupTasksRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetTaskGroupTasksResponse = New SAMForInsuranceV2ImplementationTypes.GetTaskGroupTasksResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oGetTaskGroupTasksRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "BranchCode")
        End If
        If oGetTaskGroupTasksRequest.EffectiveDate <= Date.MinValue OrElse oGetTaskGroupTasksRequest.EffectiveDate = Nothing Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "EffectiveDate")
        End If
        If oGetTaskGroupTasksRequest.TaskGroupKey = 0 Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "TaskGroupKey")
        End If

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid
        ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
        PMLookupTable.Source, oGetTaskGroupTasksRequest.BranchCode, "BranchCode", oErrors)

        oErrors.CheckForErrors()

        ' Dataset that will hold the returned results		
        Dim dsTaskGroupsTasks As DataSet = Nothing
        Dim dsResultantDataSet As New DataSet
        Dim icount As Int32

        Using cmdGetTaskGroupsTask As SiriusCommand = SiriusCommand.FromProcedure("spu_pmwrk_task_group_everyone")
            cmdGetTaskGroupsTask.AddInParameter("@pmwrk_task_group_id", SqlDbType.Int).Value = oGetTaskGroupTasksRequest.TaskGroupKey
            cmdGetTaskGroupsTask.AddInParameter("@effective_date", SqlDbType.DateTime).Value = oGetTaskGroupTasksRequest.EffectiveDate
            dsTaskGroupsTasks = con.ExecuteDataSet(cmdGetTaskGroupsTask, "Row")

        End Using
        With dsTaskGroupsTasks.Tables(0)
            dsResultantDataSet.Tables.Add()
            dsResultantDataSet.Tables(0).Columns.Add("TaskKey", GetType(System.Int32))
            dsResultantDataSet.Tables(0).Columns.Add("Name", GetType(System.String))
            dsResultantDataSet.Tables(0).Columns.Add("EffectiveDate", GetType(System.DateTime))
            dsResultantDataSet.Tables(0).Columns.Add("Description", GetType(System.String))
            dsResultantDataSet.Tables(0).Columns.Add("IsDeleted", GetType(System.Boolean))
            dsResultantDataSet.Tables(0).Columns.Add("IsIncluded", GetType(System.Boolean))
            dsResultantDataSet.Tables(0).Columns.Add("IsViewOnly", GetType(System.Boolean))
            dsResultantDataSet.Tables(0).Columns.Add("IsAvailable", GetType(System.Boolean))
            dsResultantDataSet.Tables(0).Columns.Add("TaskCategoryKey", GetType(System.Int32))
            dsResultantDataSet.Tables(0).Columns.Add("DisplayIcon", GetType(System.Int32))

            For icount = 0 To .Rows.Count - 1
                dsResultantDataSet.Tables(0).Rows.Add()
                dsResultantDataSet.Tables(0).Rows(icount)("TaskKey") = .Rows(icount).Item("id")
                dsResultantDataSet.Tables(0).Rows(icount)("Name") = .Rows(icount).Item("code")
                dsResultantDataSet.Tables(0).Rows(icount)("EffectiveDate") = .Rows(icount).Item("effective_date")
                dsResultantDataSet.Tables(0).Rows(icount)("Description") = .Rows(icount).Item("description")
                dsResultantDataSet.Tables(0).Rows(icount)("IsDeleted") = Cast.ToBoolean(.Rows(icount).Item("is_deleted"))
                dsResultantDataSet.Tables(0).Rows(icount)("IsIncluded") = Cast.ToBoolean(.Rows(icount).Item("included"))
                dsResultantDataSet.Tables(0).Rows(icount)("IsViewOnly") = Cast.ToBoolean(.Rows(icount).Item("is_view_only_task"))
                dsResultantDataSet.Tables(0).Rows(icount)("IsAvailable") = Cast.ToBoolean(.Rows(icount).Item("is_available_task"))
                dsResultantDataSet.Tables(0).Rows(icount)("TaskCategoryKey") = .Rows(icount).Item("task_category_id")
                dsResultantDataSet.Tables(0).Rows(icount)("DisplayIcon") = .Rows(icount).Item("display_icon")
            Next
        End With

        Dim xmlDoc As New System.Xml.XmlDocument
        dsResultantDataSet.DataSetName = "BaseGetTaskGroupTasksResponseTypeTaskGroupTasks"
        dsResultantDataSet.Tables(0).TableName = "Row"
        If (dsResultantDataSet.Tables(0).Rows.Count >= 1) Then
            If oGetTaskGroupTasksRequest.WCFSecurityToken = "" Then
                xmlDoc.LoadXml(dsResultantDataSet.GetXml)
                oGetTaskGroupTasksResponse.ResultDataset = xmlDoc.DocumentElement()
            End If
            oGetTaskGroupTasksResponse.ResultData = dsResultantDataSet
        End If

        Dim bIsLocked As Boolean
        Dim AnyError As STSErrorType
        AnyError = oCoreBusiness.GetTimestamp(con, _
                            oGetTaskGroupTasksRequest.BranchCode, _
                            CoreBusiness.LockName.TaskGroupCnt, _
                            oGetTaskGroupTasksRequest.TaskGroupKey, _
                            oGetTaskGroupTasksResponse.TimeStamp, _
                            bIsLocked)
        ' Return AnyErrors
        If AnyError Is Nothing = False Then
            oGetTaskGroupTasksResponse.STSError = AnyError
        End If
        Return oGetTaskGroupTasksResponse
    End Function

    ''' <summary>  
    '''  This method is used to create a connection object and to call GetUserGroupUsers to fetch User Group's Users.
    ''' </summary>  
    ''' <param name="oGetUserGroupUsersRequest">An object of type SiriusFS.SAM.BaseImplementationTypes.BaseGetUserGroupUsersRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.BaseImplementationTypes.BaseGetUserGroupUsersResponseType</returns>  
    Public Overloads Function GetUserGroupUsers(ByVal oGetUserGroupUsersRequest As BaseGetUserGroupUsersRequestType) As BaseGetUserGroupUsersResponseType

        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oResponse As BaseGetUserGroupUsersResponseType
            oResponse = GetUserGroupUsers(con, oGetUserGroupUsersRequest)
            Return oResponse
        End Using

    End Function

    ''' <summary>  
    '''  This method is used to fetch User Group's Users.
    ''' </summary>  
    ''' <param name="con">An object of type Sirius.Architecture.Data.SiriusConnection</param>  
    ''' <param name="oGetUserGroupUsersRequest">An object of type SiriusFS.SAM.BaseImplementationTypes.BaseGetUserGroupUsersRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.BaseImplementationTypes.BaseGetUserGroupUsersResponseType</returns>  
    Private Overloads Function GetUserGroupUsers(ByVal con As SiriusConnection, ByVal oGetUserGroupUsersRequest As BaseGetUserGroupUsersRequestType) As BaseGetUserGroupUsersResponseType

        Dim oGetUserGroupUsersResponse As New BaseGetUserGroupUsersResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection

        Dim lBranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetUserGroupUsersRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetUserGroupUsersRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oGetUserGroupUsersResponse = New SAMForInsuranceV2ImplementationTypes.GetUserGroupUsersResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oGetUserGroupUsersRequest.BranchCode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, "BranchCode")
        End If
        'Variation from Tech Spec - As asked by Gaurav made UserGroupKey as optional 
        'If (oGetUserGroupUsersRequest.UserGroupKey = 0) Then
        '    oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
        '                                SAMInvalidData.MandatoryInputMissing.ToString, "UserGroupKey")

        If (oGetUserGroupUsersRequest.EffectiveDate <= Date.MinValue) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, "EffectiveDate")
        End If
        oSAMErrorCollection.CheckForErrors()

        ' Lookup Codes to ensure they are valid
        lBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
                        PMLookupTable.Source, oGetUserGroupUsersRequest.BranchCode, "BranchCode", _
                        oSAMErrorCollection)
        Dim iUserGroupId As Integer
        If Not String.IsNullOrEmpty(oGetUserGroupUsersRequest.UserGroupCode) Then
            iUserGroupId = GetAndValidateSpecifiedTableCode(con, PMLookupTable.UserGroup, "pmuser_group_id", "Code", oGetUserGroupUsersRequest.UserGroupCode.ToString, oSAMErrorCollection, "pmuser_group_id")
        End If
        oSAMErrorCollection.CheckForErrors()

        Dim iRestrictUserList As Integer
        If (oGetUserGroupUsersRequest.RestrictUserListSpecified) Then
            If (oGetUserGroupUsersRequest.RestrictUserList) Then
                iRestrictUserList = 1
            Else
                iRestrictUserList = 0
            End If
        Else
            iRestrictUserList = 0
        End If

        ' Dataset that will hold the returned results		
        Dim dsGroupUsers As DataSet = Nothing
        If String.IsNullOrEmpty(oGetUserGroupUsersRequest.UserGroupCode) Then
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmuser_all_users_sel")
                With oGetUserGroupUsersRequest
                    cmd.AddInParameter("@effective_date", SqlDbType.DateTime).Value = .EffectiveDate
                    cmd.AddInParameter("@source_id", SqlDbType.Int).Value = lBranchId
                    cmd.AddInParameter("@RestrictUserList", SqlDbType.Int).Value = iRestrictUserList
                End With
                dsGroupUsers = con.ExecuteDataSet(cmd, "Row")
            End Using
        Else
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmuser_group_users_sel")
                With oGetUserGroupUsersRequest
                    cmd.AddInParameter("@pmuser_group_id", SqlDbType.Int).Value = iUserGroupId
                    cmd.AddInParameter("@effective_date", SqlDbType.DateTime).Value = .EffectiveDate
                    cmd.AddInParameter("@source_id", SqlDbType.Int).Value = lBranchId
                    cmd.AddInParameter("@RestrictUserList", SqlDbType.Int).Value = iRestrictUserList
                    If .AgentKey <> 0 Then
                        cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = .AgentKey
                    End If

                End With
                dsGroupUsers = con.ExecuteDataSet(cmd, "Row")
            End Using
        End If
        oGetUserGroupUsersResponse.EmailAddress = ""

        dsGroupUsers.DataSetName = "BaseGetUserGroupUsersResponseTypeUserGroupUsers"
        If (dsGroupUsers.Tables(0).Rows.Count > 0) Then
            dsGroupUsers.Tables("Row").Columns("user_id").ColumnName = "UserKey"
            dsGroupUsers.Tables("Row").Columns("username").ColumnName = "Name"
            dsGroupUsers.Tables("Row").Columns("email_address").ColumnName = "EmailAddress"

            For iRow As Integer = 0 To dsGroupUsers.Tables(0).Rows.Count - 1
                If Not dsGroupUsers.Tables(0).Rows(iRow).Item("EmailAddress") Is Nothing AndAlso dsGroupUsers.Tables(0).Rows(iRow).Item("EmailAddress").ToString <> "" Then
                    If oGetUserGroupUsersResponse.EmailAddress = "" Then
                        oGetUserGroupUsersResponse.EmailAddress = dsGroupUsers.Tables(0).Rows(iRow).Item("EmailAddress").ToString
                    Else
                        oGetUserGroupUsersResponse.EmailAddress = oGetUserGroupUsersResponse.EmailAddress & "; " & dsGroupUsers.Tables(0).Rows(iRow).Item("EmailAddress").ToString
                    End If
                End If
            Next
        End If


        Dim xmlDoc As New System.Xml.XmlDocument
        If oGetUserGroupUsersRequest.WCFSecurityToken = "" Then
            xmlDoc.LoadXml(dsGroupUsers.GetXml)
            oGetUserGroupUsersResponse.ResultDataset = xmlDoc.DocumentElement()
        End If
        oGetUserGroupUsersResponse.ResultData = dsGroupUsers
        Return oGetUserGroupUsersResponse

    End Function

    ''' <summary>
    '''This method pass the request object GetUserGroupTaskGroups along with the connection object
    '''</summary>
    '''<param name="oGetUserGroupTaskGroupsRequest" type="BaseGetUserGroupTaskGroupsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetUserGroupTaskGroupsResponseType</returns>
    '''<remarks></remarks>

    Public Overloads Function GetUserGroupTaskGroups(ByVal oGetUserGroupTaskGroupsRequest As BaseGetUserGroupTaskGroupsRequestType) As BaseGetUserGroupTaskGroupsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseGetUserGroupTaskGroupsResponseType

            oResponse = GetUserGroupTaskGroups(con, oGetUserGroupTaskGroupsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method pass the request object to the stored procedure and resultant value as dataset and pass this as XML back
    '''</summary>
    '''<param name="oGetUserGroupTaskGroupsRequest" type="BaseGetUserGroupTaskGroupsRequestType"></param>
    '''<param name="con" type="SiriusConnection"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetUserGroupTaskGroupsRequestType</returns>
    '''<remarks></remarks>

    Public Overloads Function GetUserGroupTaskGroups(ByVal con As SiriusConnection, ByVal oGetUserGroupTaskGroupsRequest As BaseGetUserGroupTaskGroupsRequestType) As BaseGetUserGroupTaskGroupsResponseType

        Dim oGetUserGroupTaskGroupsResponse As New BaseImplementationTypes.BaseGetUserGroupTaskGroupsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim ibranchId As Int32
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetUserGroupTaskGroupsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetUserGroupTaskGroupsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetUserGroupTaskGroupsResponse = New SAMForInsuranceV2ImplementationTypes.GetUserGroupTaskGroupsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        If String.IsNullOrEmpty(oGetUserGroupTaskGroupsRequest.BranchCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "BranchCode")
        End If
        If oGetUserGroupTaskGroupsRequest.EffectiveDate <= Date.MinValue OrElse oGetUserGroupTaskGroupsRequest.EffectiveDate = Nothing Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "EffectiveDate")
        End If
        If String.IsNullOrEmpty(oGetUserGroupTaskGroupsRequest.UserGroupCode) Then
            oErrors.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                        SAMInvalidData.MandatoryInputMissing.ToString, _
                                        "UserGroupCode")
        End If

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid
        If Not String.IsNullOrEmpty(oGetUserGroupTaskGroupsRequest.BranchCode) Then
            ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
            PMLookupTable.Source, oGetUserGroupTaskGroupsRequest.BranchCode, "BranchCode", oErrors)
        End If
        Dim iUserGroupKey As Integer
        iUserGroupKey = GetAndValidateSpecifiedTableCode(con, PMLookupTable.UserGroup, "pmuser_group_id", "Code", oGetUserGroupTaskGroupsRequest.UserGroupCode.ToString, oErrors, "pmuser_group_id")
        oErrors.CheckForErrors()

        ' Dataset that will hold the returned results		
        Dim dsGetUserGroupTaskGroups As DataSet = Nothing
        Dim dsResultantDataSet As New DataSet
        Dim icount As Int32

        Using cmdGetUserGroupTaskGroups As SiriusCommand = SiriusCommand.FromProcedure("spu_pmuser_group_tasks")
            cmdGetUserGroupTaskGroups.AddInParameter("@pmuser_group_id", SqlDbType.Int).Value = iUserGroupKey
            cmdGetUserGroupTaskGroups.AddInParameter("@effective_date", SqlDbType.DateTime).Value = oGetUserGroupTaskGroupsRequest.EffectiveDate
            dsGetUserGroupTaskGroups = con.ExecuteDataSet(cmdGetUserGroupTaskGroups, "Row")

        End Using
        If (dsGetUserGroupTaskGroups.Tables.Count >= 1) AndAlso (dsGetUserGroupTaskGroups.Tables(0).Rows.Count >= 1) Then
            With dsGetUserGroupTaskGroups.Tables(0)
                dsResultantDataSet.Tables.Add()
                dsResultantDataSet.Tables(0).Columns.Add("TaskGroupKey", GetType(System.Int32))
                dsResultantDataSet.Tables(0).Columns.Add("Code", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("EffectiveDate", GetType(System.DateTime))
                dsResultantDataSet.Tables(0).Columns.Add("Description", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("IsDeleted", GetType(System.Boolean))
                dsResultantDataSet.Tables(0).Columns.Add("IsIncluded", GetType(System.Boolean))
                dsResultantDataSet.Tables(0).Columns.Add("DisplaySequence", GetType(System.Int32))

                For icount = 0 To .Rows.Count - 1
                    dsResultantDataSet.Tables(0).Rows.Add()
                    dsResultantDataSet.Tables(0).Rows(icount)("TaskGroupKey") = .Rows(icount).Item("id")
                    dsResultantDataSet.Tables(0).Rows(icount)("Code") = .Rows(icount).Item("code")
                    dsResultantDataSet.Tables(0).Rows(icount)("EffectiveDate") = .Rows(icount).Item("effective_date")
                    dsResultantDataSet.Tables(0).Rows(icount)("Description") = .Rows(icount).Item("description")
                    dsResultantDataSet.Tables(0).Rows(icount)("IsDeleted") = Cast.ToBoolean(.Rows(icount).Item("is_deleted"))
                    dsResultantDataSet.Tables(0).Rows(icount)("IsIncluded") = Cast.ToBoolean(.Rows(icount).Item("included"))
                    dsResultantDataSet.Tables(0).Rows(icount)("DisplaySequence") = .Rows(icount).Item("displaySequence")
                Next
            End With
        End If

        Dim Docxml As New System.Xml.XmlDocument
        dsResultantDataSet.DataSetName = "BaseGetUserGroupTaskGroupsResponseTypeTaskGroups"
        dsResultantDataSet.Tables(0).TableName = "Row"
        If (dsResultantDataSet.Tables(0).Rows.Count >= 1) Then
            If oGetUserGroupTaskGroupsRequest.WCFSecurityToken = "" Then
                Docxml.LoadXml(dsResultantDataSet.GetXml)
                oGetUserGroupTaskGroupsResponse.ResultDataset = Docxml.DocumentElement()
            End If
            oGetUserGroupTaskGroupsResponse.ResultData = dsResultantDataSet
        End If

        Dim bIsLocked As Boolean
        Dim AnyError As STSErrorType
        AnyError = oCoreBusiness.GetTimestamp(con, _
                            oGetUserGroupTaskGroupsRequest.BranchCode, _
                            CoreBusiness.LockName.UserGroupCnt, _
                           iUserGroupKey, _
                            oGetUserGroupTaskGroupsResponse.TimeStamp, _
                            bIsLocked)
        ' Return AnyErrors
        If AnyError Is Nothing = False Then
            oGetUserGroupTaskGroupsResponse.STSError = AnyError
        End If

        Return oGetUserGroupTaskGroupsResponse

    End Function

    ''' <summary>
    '''This method pass the request object oGetReferredPaymentsRequest along with the connection object
    '''</summary>
    '''<param name="oGetReferredPaymentsRequest" type="BaseGetReferredPaymentsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetReferredPaymentsResponseType</returns>
    '''<remarks></remarks>

    Public Overloads Function GetReferredPayments(ByVal oGetReferredPaymentsRequest As BaseGetReferredPaymentsRequestType) As BaseGetReferredPaymentsResponseType

        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseGetReferredPaymentsResponseType

            oResponse = GetReferredPayments(con, oGetReferredPaymentsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method pass the request object to the stored procedure and the cover note is being added
    '''</summary>
    '''<param name="oGetReferredPaymentsRequest" type="BaseGetReferredPaymentsRequestType"></param>
    '''<param name="con" type="SiriusConnection"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetReferredPaymentsResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function GetReferredPayments(ByVal con As SiriusConnection, ByVal oGetReferredPaymentsRequest As BaseGetReferredPaymentsRequestType) As BaseGetReferredPaymentsResponseType

        Dim oGetReferredPaymentsResponse As New BaseGetReferredPaymentsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage
        ' PN 78207 , Start
        Dim sDisableWildcardSearchOption As String = String.Empty
        Dim sEnablePartialWildcardSearchOption As String = String.Empty
        Dim bDisableWildcardSearchOption As Boolean = False
        Dim bEnablePartialWildcardSearchOption As Boolean = False
        Dim sQueryLink As String = " Like "
        ' PN 78207 , End

        Dim lUserId As Integer
        Dim lReferredBranchId As Integer
        Dim sCaseNumber As String = String.Empty
        Dim sPayeeName As String = String.Empty

        If oGetReferredPaymentsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetReferredPaymentsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetReferredPaymentsResponse = New SAMForInsuranceV2ImplementationTypes.GetReferredPaymentsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        oGetReferredPaymentsRequest.Validate(CObj(oErrors))
        oErrors.CheckForErrors()

        'If oGetReferredPaymentsRequest.BranchCode IsNot Nothing AndAlso oGetReferredPaymentsRequest.UserCode IsNot Nothing AndAlso oGetReferredPaymentsRequest.ReferredPaymentsBranchCode IsNot Nothing Then
        ValidateReferredPaymentsLookup(con, CObj(oErrors), lUserId, oGetReferredPaymentsRequest.BranchCode, oGetReferredPaymentsRequest.UserCode, lReferredBranchId, oGetReferredPaymentsRequest.ReferredPaymentsBranchCode)
        oErrors.CheckForErrors()
        'Else
        '    lReferredBranchId = Nothing
        '    lUserId = Nothing
        'End If

        Dim dsReferredPayments As DataSet = Nothing
        Dim dsResultantDataSet As New DataSet
        Dim icount As Int32
        Dim sSQL As String = ""
        Dim bPresent As Boolean

        ' PN 78207 , Start

        ' Fetch System Options
        oCoreBusiness.GetSystemOption(oGetReferredPaymentsRequest.BranchCode, SystemOption.DisableWildcardSearch, sDisableWildcardSearchOption)
        oCoreBusiness.GetSystemOption(oGetReferredPaymentsRequest.BranchCode, SystemOption.EnablePartialWildcardSearch, sEnablePartialWildcardSearchOption)

        If (Trim(sDisableWildcardSearchOption) = "1") Then
            bDisableWildcardSearchOption = True
        Else
            bDisableWildcardSearchOption = False
        End If

        If (Trim(sEnablePartialWildcardSearchOption) = "1") Then
            bEnablePartialWildcardSearchOption = True
        Else
            bEnablePartialWildcardSearchOption = False
        End If


        ' Validate all the fileds through which wild card search is possible
        If Not oCoreBusiness.ValidWildcardSearch( _
          bDisableWildcardSearchOption, _
          bEnablePartialWildcardSearchOption, _
          oGetReferredPaymentsRequest.ClaimNumber) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "ClaimNumber")
        End If
        oErrors.CheckForErrors()


        If Not oCoreBusiness.ValidWildcardSearch( _
        bDisableWildcardSearchOption, _
        bEnablePartialWildcardSearchOption, _
        oGetReferredPaymentsRequest.PolicyNumber) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "PolicyNumber")
        End If

        oErrors.CheckForErrors()


        If Not oCoreBusiness.ValidWildcardSearch( _
        bDisableWildcardSearchOption, _
        bEnablePartialWildcardSearchOption, _
        oGetReferredPaymentsRequest.ClientShortName) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "ClientShortName")
        End If

        oErrors.CheckForErrors()


        If Not oCoreBusiness.ValidWildcardSearch( _
        bDisableWildcardSearchOption, _
        bEnablePartialWildcardSearchOption, _
        oGetReferredPaymentsRequest.PartyKey.ToString()) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "PartyKey")
        End If

        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
        bDisableWildcardSearchOption, _
        bEnablePartialWildcardSearchOption, _
        oGetReferredPaymentsRequest.CaseNumber) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "Case Number")
        End If

        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
        bDisableWildcardSearchOption, _
        bEnablePartialWildcardSearchOption, _
        oGetReferredPaymentsRequest.PayeeName) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "PayeeName")
        End If

        oErrors.CheckForErrors()


        If oGetReferredPaymentsRequest.CaseNumber IsNot Nothing Then
            sCaseNumber = oGetReferredPaymentsRequest.CaseNumber
        End If

        If oGetReferredPaymentsRequest.PayeeName IsNot Nothing Then
            sPayeeName = oGetReferredPaymentsRequest.PayeeName
        End If


        dsReferredPayments = GetReferredPaymentList(con, oGetReferredPaymentsRequest.ClaimNumber, oGetReferredPaymentsRequest.PolicyNumber, oGetReferredPaymentsRequest.PartyKey, oGetReferredPaymentsRequest.DateOfPayment, lUserId, lReferredBranchId, sCaseNumber:=sCaseNumber, sPayeeName:=sPayeeName)

        Dim dataViewRP As DataView = New DataView

        'If Disable wild Card Search option is True
        If bDisableWildcardSearchOption = True Then
            sQueryLink = " = "
        End If


        With dataViewRP
            .Table = dsReferredPayments.Tables(0)
            If oGetReferredPaymentsRequest.ClaimNumber IsNot Nothing Then
                sSQL = "Claim_number " + sQueryLink + " '" + Cast.ToString(oGetReferredPaymentsRequest.ClaimNumber) + "'"
                bPresent = True
            End If

            If oGetReferredPaymentsRequest.PolicyNumber IsNot Nothing Then
                If bPresent Then
                    sSQL = sSQL + " AND policy_number " + sQueryLink + " '" + oGetReferredPaymentsRequest.PolicyNumber + "'"
                Else
                    sSQL = " policy_number " + sQueryLink + " '" + oGetReferredPaymentsRequest.PolicyNumber + "'"
                    bPresent = True
                End If
            End If

            If oGetReferredPaymentsRequest.ClientShortName IsNot Nothing Then
                If bPresent Then
                    sSQL = sSQL + " AND client_code " + sQueryLink + " ' " + oGetReferredPaymentsRequest.ClientShortName + "'"
                Else
                    sSQL = " client_code " + sQueryLink + " '" + oGetReferredPaymentsRequest.ClientShortName + "'"
                    bPresent = True
                End If
            End If

            If oGetReferredPaymentsRequest.UserCode IsNot Nothing Then
                If bPresent Then
                    sSQL = sSQL + " AND username " + sQueryLink + " '" + oGetReferredPaymentsRequest.UserCode + "'"
                Else
                    sSQL = "username " + sQueryLink + " '" + oGetReferredPaymentsRequest.UserCode + "'"
                End If
            End If

            .RowFilter = sSQL
        End With

        Dim nCurrencyFromToBaseXRate As Decimal
        Dim nCurrencyID As Integer
        Dim nSourceId As Integer
        Dim dtDateOfPayments As Date
        Dim nPaymentAmount As Double

        If (Not String.IsNullOrEmpty(oGetReferredPaymentsRequest.BranchCode)) Then
            nSourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
                PMLookupTable.Source, oGetReferredPaymentsRequest.BranchCode, "BranchCode", oErrors)
        End If


        If dataViewRP IsNot Nothing Then
            With dataViewRP.ToTable
                dsResultantDataSet.Tables.Add()
                dsResultantDataSet.Tables(0).Columns.Add("ClaimPaymentKey", GetType(System.Int32))
                dsResultantDataSet.Tables(0).Columns.Add("ClaimKey", GetType(System.Int32))
                dsResultantDataSet.Tables(0).Columns.Add("ClaimNumber", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("PolicyNumber", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("ClientName", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("PaymentAmount", GetType(System.Double))
                dsResultantDataSet.Tables(0).Columns.Add("PaymentDate", GetType(System.DateTime))
                dsResultantDataSet.Tables(0).Columns.Add("CreatedBy", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("Status", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("CaseNumber", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("PayeeName", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("PayeeType", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("AllowedClosedBranchClaims", GetType(System.Int32))
                dsResultantDataSet.Tables(0).Columns.Add("IsReferredForRecommendation", GetType(System.Boolean))
                dsResultantDataSet.Tables(0).Columns.Add("RecommendedBy", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("CurrencyCode", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("PaymentAmountBaseCurrency", GetType(System.Double))
                dsResultantDataSet.Tables(0).Columns.Add("PaymentAmountBaseCurrencySpecified", GetType(System.Double))
                dsResultantDataSet.Tables(0).Columns.Add("CurrencyId", GetType(System.Int32))


                For icount = 0 To .Rows.Count - 1
                    dsResultantDataSet.Tables(0).Rows.Add()
                    dsResultantDataSet.Tables(0).Rows(icount)("ClaimPaymentKey") = .Rows(icount).Item("claim_payment_id")
                    dsResultantDataSet.Tables(0).Rows(icount)("ClaimKey") = .Rows(icount).Item("Claim_id")
                    dsResultantDataSet.Tables(0).Rows(icount)("ClaimNumber") = .Rows(icount).Item("Claim_Number")
                    dsResultantDataSet.Tables(0).Rows(icount)("PolicyNumber") = .Rows(icount).Item("Policy_Number")
                    dsResultantDataSet.Tables(0).Rows(icount)("ClientName") = .Rows(icount).Item("Client_name")
                    dsResultantDataSet.Tables(0).Rows(icount)("PaymentAmount") = .Rows(icount).Item("Payment_Amount")
                    dsResultantDataSet.Tables(0).Rows(icount)("PaymentDate") = .Rows(icount).Item("date_of_payment")
                    dsResultantDataSet.Tables(0).Rows(icount)("CreatedBy") = .Rows(icount).Item("username")
                    dsResultantDataSet.Tables(0).Rows(icount)("Status") = .Rows(icount).Item("Status")
                    dsResultantDataSet.Tables(0).Rows(icount)("CaseNumber") = .Rows(icount).Item("CaseNumber")
                    dsResultantDataSet.Tables(0).Rows(icount)("PayeeName") = .Rows(icount).Item("PayeeName")
                    dsResultantDataSet.Tables(0).Rows(icount)("PayeeType") = .Rows(icount).Item("PayeeType")
                    dsResultantDataSet.Tables(0).Rows(icount)("AllowedClosedBranchClaims") = .Rows(icount).Item("AllowedClosedBranchClaims")
                    If IsDBNull(.Rows(icount).Item("is_referred_for_recommendation")) Then
                        dsResultantDataSet.Tables(0).Rows(icount)("IsReferredForRecommendation") = False
                    Else
                        dsResultantDataSet.Tables(0).Rows(icount)("IsReferredForRecommendation") = Cast.ToBoolean(.Rows(icount).Item("is_referred_for_recommendation"), False)
                    End If

                    dsResultantDataSet.Tables(0).Rows(icount)("RecommendedBy") = Convert.ToString(.Rows(icount).Item("Recommender"))
                    dsResultantDataSet.Tables(0).Rows(icount)("CurrencyCode") = Convert.ToString(.Rows(icount).Item("CurrencyCode"))
                    nCurrencyID = CInt(.Rows(icount).Item("currency_id"))
                    dtDateOfPayments = CDate(.Rows(icount).Item("date_of_payment"))
                    nPaymentAmount = CDbl(.Rows(icount).Item("Payment_Amount"))
                    GetCurrencyToBaseXRate(con, nCurrencyID, nSourceId, dtDateOfPayments, nCurrencyFromToBaseXRate)
                    dsResultantDataSet.Tables(0).Rows(icount)("PaymentAmountBaseCurrency") = nPaymentAmount * nCurrencyFromToBaseXRate
                    dsResultantDataSet.Tables(0).Rows(icount)("PaymentAmountBaseCurrencySpecified") = True
                Next
            End With

            dsResultantDataSet.DataSetName = "BaseGetReferredPaymentsResponseTypeCashListItems"
            dsResultantDataSet.Tables(0).TableName = "Row"


            If oGetReferredPaymentsRequest.WCFSecurityToken = "" Then
                Dim Docxml As New System.Xml.XmlDocument

                If (dsResultantDataSet.Tables(0).Rows.Count >= 1) Then
                    Docxml.LoadXml(dsResultantDataSet.GetXml)
                    oGetReferredPaymentsResponse.ResultDataset = Docxml.DocumentElement()
                Else
                    oGetReferredPaymentsResponse.ResultDataset = Nothing
                End If
            Else
                oGetReferredPaymentsResponse.ResultDataset = Nothing

            End If
            If dsResultantDataSet.Tables(0).Rows.Count >= 1 Then
                oGetReferredPaymentsResponse.ResultData = dsResultantDataSet
            Else
                oGetReferredPaymentsResponse.ResultData = Nothing
            End If

        End If

        Return oGetReferredPaymentsResponse
    End Function

    ''' <summary>
    '''This method executes the dataset using procedure
    '''</summary>
    '''<param name="con" type="SiriusConnection"></param>
    '''<param name="sClaimNumber" type="String"></param>
    '''<param name="sPolicyNumber" type="String"></param>
    '''<param name="lPartyKey" type="Integer"></param>
    '''<param name="dtDateOfPayments" type="Date"></param>
    '''<param name="lUserId" type="Integer"></param>
    '''<param name="lReferredBranchId" type="Integer"></param>
    '''<remarks></remarks>
    Public Function GetReferredPaymentList(ByVal con As SiriusConnection, _
ByVal sClaimNumber As String, _
ByVal sPolicyNumber As String, _
ByVal lPartyKey As Integer, _
ByVal dtDateOfPayments As Date, _
ByVal lUserId As Integer, _
ByVal lReferredBranchId As Integer, _
Optional ByVal sClientCode As String = "", _
Optional ByVal sUserName As String = "", _
Optional ByVal sQueryLink As String = "LIKE", _
Optional ByVal nAuthoriserId As Integer = 0, _
Optional ByVal sCaseNumber As String = "", _
Optional ByVal sPayeeName As String = "") As DataSet


        Dim dsUser As DataSet = Nothing
        Dim sTPA As Integer = 0

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_User_OtherPartyID")
            cmd.AddInParameter("@userid", SqlDbType.Int).Value = _SiriusUser.UserID
            dsUser = con.ExecuteDataSet(cmd, "User")
        End Using

        If Not dsUser Is Nothing AndAlso Not dsUser.Tables Is Nothing Then
            If dsUser.Tables(0).Rows.Count > 0 And Not dsUser.Tables(0).Rows(0).Item("shortname") Is Nothing Then
                If dsUser.Tables(0).Rows(0).Item("other_party_id").ToString() = "" Then
                    sTPA = 0
                Else
                    sTPA = Convert.ToInt32(dsUser.Tables(0).Rows(0).Item("other_party_id"))
                End If

            End If
        End If


        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_clm_get_referred_payments")
            cmd.AddInParameter("@claim_number", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(sClaimNumber)
            cmd.AddInParameter("@policy_number", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(sPolicyNumber)
            cmd.AddInParameter("@client_id", SqlDbType.Int).Value = Cast.NullIfDefault(lPartyKey)
            cmd.AddInParameter("@date_of_payment", SqlDbType.DateTime).Value = Cast.NullIfDefault(dtDateOfPayments)
            cmd.AddInParameter("@userid", SqlDbType.Int).Value = Cast.NullIfDefault(lUserId)
            cmd.AddInParameter("@source_id", SqlDbType.Int).Value = Cast.NullIfDefault(lReferredBranchId)
            cmd.AddInParameter("other_party_id", SqlDbType.Int).Value = Cast.NullIfDefault(sTPA)
            cmd.AddInParameter("@sclient_code", SqlDbType.Char, 20).Value = Cast.NullIfDefault(sClientCode)
            cmd.AddInParameter("@suser_name", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(sUserName)
            cmd.AddInParameter("@sQuery", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(sQueryLink)
            If nAuthoriserId <> 0 Then
                cmd.AddInParameter("@nAuthorizerId", SqlDbType.Int).Value = Cast.NullIfDefault(nAuthoriserId)
            End If
            cmd.AddInParameter("@CaseNumber", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(sCaseNumber)
            cmd.AddInParameter("@PayeeName", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(sPayeeName)
            GetReferredPaymentList = con.ExecuteDataSet(cmd, "CashListItems")
        End Using
        Return GetReferredPaymentList
    End Function

    ''' <summary>
    '''This method validates Data validations
    '''</summary>
    '''<param name="oErrorCollection" type="Object"></param>
    '''<param name="oGetReferredPaymentsRequest" type="BaseGetReferredPaymentsRequestType"></param>
    '''<remarks></remarks>
    Private Sub ValidateReferredPaymentsData(ByRef oErrorCollection As Object, ByVal oGetReferredPaymentsRequest As BaseGetReferredPaymentsRequestType)
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        If oGetReferredPaymentsRequest.PartyKeySpecified = True Then
            If oGetReferredPaymentsRequest.PartyKey <= 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                                   "PartyKey")
            End If
        End If

        If oGetReferredPaymentsRequest.DateOfPaymentSpecified = True Then
            If oGetReferredPaymentsRequest.DateOfPayment <= Date.MinValue Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                                   "DateOfPayment")
            End If
        End If

    End Sub
    ''' <summary>
    '''This  method validates lookup validations
    '''</summary>
    '''<param name="oErrorCollection" type="Object"></param>
    '''<param name="lUserId" type="Integer"></param>
    '''<param name="BranchCode" type="String"></param>
    '''<param name="UserCode" type="String"></param>
    '''<param name="lReferredBranchId" type="Integer"></param>
    '''<param name="ReferredPaymentsBranchCode" type="String"></param>
    '''<remarks></remarks>
    Public Sub ValidateReferredPaymentsLookup(ByVal con As SiriusConnection, ByRef oErrorCollection As Object, ByRef lUserId As Integer, ByVal BranchCode As String, ByVal UserCode As String, ByRef lReferredBranchId As Integer, ByVal ReferredPaymentsBranchCode As String)
        Dim oCoreBusiness As New CoreBusiness
        Dim ibranchId As Int32
        Dim oSAMErrorCollection As New SAMErrorCollection

        If BranchCode IsNot Nothing Then
            ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
            PMLookupTable.Source, BranchCode, "Source", oSAMErrorCollection)
        Else
            ibranchId = 0
        End If
        If UserCode IsNot Nothing Then
            lUserId = GetAndValidateSpecifiedTableCode(con, "pmuser", "user_id", "username", UserCode, oSAMErrorCollection, "username")
        Else
            lUserId = 0
        End If

        If Not String.IsNullOrEmpty(ReferredPaymentsBranchCode) Then
            lReferredBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
            PMLookupTable.Source, ReferredPaymentsBranchCode, "Source", oSAMErrorCollection)
        Else
            lReferredBranchId = 0
        End If

    End Sub

    ''' <summary>
    ''' For returning Hashed Password and password history for an user
    ''' </summary>
    ''' <param name="vRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function ValidateUser(ByVal vRequest As ValidateUserRequestType) As ValidateUserResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As ValidateUserResponseType = Nothing

            oResponse = ValidateUser(con, vRequest.UserName)

            Return oResponse

        End Using
    End Function

    ''' <summary>
    ''' For returning Hashed Password and password history for an user
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="vUserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function ValidateUser(ByVal con As SiriusConnection, ByVal vUserName As String) As ValidateUserResponseType
        Const ACMethodName As String = "ValidateUser"

        Dim oCoreBusiness As New CoreBusiness
        Dim oResponse As New ValidateUserResponseType
        Dim STSError As New STSErrorPublisher

        '*******************
        ' DATA VALIDATION
        '*******************
        If String.IsNullOrEmpty(vUserName) Then
            STSError.AddInvalidField("vUserName", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "vUserName"), "")
        End If

        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        Dim dsUserDetail As New DataSet

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_check_Logon")
            cmd.AddInParameter("@username", SqlDbType.VarChar).Value = vUserName
            cmd.AddInParameter("@effective_date", SqlDbType.DateTime).Value = DateTime.Now
            dsUserDetail = con.ExecuteDataSet(cmd, "UserDetail")
        End Using

        If dsUserDetail.Tables(0).Rows.Count > 0 Then
            Dim dsUserPasswordHistory As New DataSet
            oResponse.UserId = Convert.ToInt32(dsUserDetail.Tables(0).Rows(0).Item("user_id"))
            oResponse.PasswordHash = dsUserDetail.Tables(0).Rows(0).Item("secure_password").ToString()
           
            If oResponse.PasswordHash = String.Empty Then
                oResponse.SystemUpgradeChangePasswordRequired = True
                oResponse.PasswordHash = dsUserDetail.Tables(0).Rows(0).Item("password").ToString()
            Else
                oResponse.SystemUpgradeChangePasswordRequired = False
            End If

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_sir_passwordhistory_sel")
                cmd.AddInParameter("@user_id", SqlDbType.Int).Value = dsUserDetail.Tables(0).Rows(0).Item("user_id")
                dsUserPasswordHistory = con.ExecuteDataSet(cmd, "PasswordHistory")
            End Using

            If dsUserPasswordHistory.Tables.Count > 0 AndAlso dsUserPasswordHistory.Tables(0).Rows.Count > 0 Then
                oResponse.PasswordHistory = New List(Of String)
                For iCt As Integer = 0 To dsUserPasswordHistory.Tables(0).Rows.Count - 1
                    oResponse.PasswordHistory.Add(dsUserPasswordHistory.Tables(0).Rows(iCt).Item("historic_password").ToString())
                Next
            End If

            If CInt(dsUserDetail.Tables(0).Rows(0).Item("Is_Temp_Password")) = 1 Then
                If oResponse.PasswordHistory IsNot Nothing Then
                    oResponse.PasswordHistory = New List(Of String)
                End If
                oResponse.PasswordHistory.Add(dsUserDetail.Tables(0).Rows(0).Item("secure_password").ToString())
            End If
        End If

        Return oResponse

    End Function

    Public Overloads Function UpdateUserDetail(ByVal vRequest As UpdateUserDetailRequestType) As UpdateUserDetailResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As UpdateUserDetailResponseType = Nothing

            oResponse = UpdateUserDetail(con, vRequest)

            Return oResponse

        End Using
    End Function

    Public Overloads Function UpdateUserDetail(ByVal con As SiriusConnection, ByVal vRequest As UpdateUserDetailRequestType) As UpdateUserDetailResponseType
        Const ACMethodName As String = "UpdateUserDetail"

        Dim oCoreBusiness As New CoreBusiness
        Dim oResponse As New UpdateUserDetailResponseType
        Dim STSError As New STSErrorPublisher
        Dim bLockingEnabled As Boolean = False
        Dim iIncorrectAttemptCount As Integer
        Dim bIsLocked As Boolean = False
        '*******************
        ' DATA VALIDATION
        '*******************

        If String.IsNullOrEmpty(vRequest.UserName) Then
            STSError.AddInvalidField("UserName", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "UserName"), "")
        End If

        If String.IsNullOrEmpty(vRequest.BranchCode) Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        Try

            Dim UserAttemptCount As String
            oCoreBusiness.GetSystemOption(vRequest.BranchCode, SystemOption.UserAttemptCount, UserAttemptCount)

            If Not String.IsNullOrEmpty(UserAttemptCount) AndAlso Convert.ToInt32(UserAttemptCount) > 0 Then ' Locking enabled
                bLockingEnabled = True
                iIncorrectAttemptCount = Convert.ToInt32(UserAttemptCount)
            End If

            If bLockingEnabled = True Then
                If vRequest.IsAuthenticated = False Then
                    'Update Failure Count and Return if Account Locked
                    UpdateIncorrectAttemptsAndLockUnlock(con, vRequest.UserId, 1, iIncorrectAttemptCount, bIsLocked)
                    oResponse.IsLocked = bIsLocked
                Else
                    'Update failure count to 0
                    UpdateIncorrectAttemptsAndLockUnlock(con, vRequest.UserId, 2, iIncorrectAttemptCount, bIsLocked)
                    'Get other attributes
                    Dim dsUserDetail As New DataSet
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_check_Logon")
                        cmd.AddInParameter("@username", SqlDbType.VarChar).Value = vRequest.UserName
                        cmd.AddInParameter("@effective_date", SqlDbType.DateTime).Value = DateTime.Now()
                        dsUserDetail = con.ExecuteDataSet(cmd, "UserDetail")
                        If dsUserDetail.Tables(0).Rows.Count > 0 Then
                            If dsUserDetail.Tables(0).Rows(0).Item("full_name") IsNot Nothing Then
                                oResponse.FullName = dsUserDetail.Tables(0).Rows(0).Item("full_name").ToString()
                            End If
                            If dsUserDetail.Tables(0).Rows(0).Item("email_address") IsNot Nothing Then
                                oResponse.EmailAddress = dsUserDetail.Tables(0).Rows(0).Item("email_address").ToString()
                            End If
                            If dsUserDetail.Tables(0).Rows(0).Item("is_locked") IsNot Nothing Then
                                oResponse.IsLocked = Convert.ToBoolean(dsUserDetail.Tables(0).Rows(0).Item("is_locked"))
                            End If

                            If dsUserDetail.Tables(0).Rows(0).Item("Is_Temp_Password") IsNot Nothing Then
                                oResponse.IsTempPassword = Convert.ToBoolean(dsUserDetail.Tables(0).Rows(0).Item("Is_Temp_Password"))
                            End If
                            If dsUserDetail.Tables(0).Rows(0).Item("password_change_date") IsNot Nothing Then
                                oResponse.PasswordChangeDate = Convert.ToDateTime(dsUserDetail.Tables(0).Rows(0).Item("password_change_date"))
                            End If
                        End If
                    End Using
                End If
            Else
                Dim dsUserDetail As New DataSet
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_check_Logon")
                    cmd.AddInParameter("@username", SqlDbType.VarChar).Value = vRequest.UserName
                    cmd.AddInParameter("@effective_date", SqlDbType.DateTime).Value = DateTime.Now()
                    dsUserDetail = con.ExecuteDataSet(cmd, "UserDetail")
                    If dsUserDetail.Tables(0).Rows.Count > 0 Then
                        If dsUserDetail.Tables(0).Rows(0).Item("full_name") IsNot Nothing Then
                            oResponse.FullName = dsUserDetail.Tables(0).Rows(0).Item("full_name").ToString()
                        End If
                        If dsUserDetail.Tables(0).Rows(0).Item("email_address") IsNot Nothing Then
                            oResponse.EmailAddress = dsUserDetail.Tables(0).Rows(0).Item("email_address").ToString()
                        End If
                        If dsUserDetail.Tables(0).Rows(0).Item("is_locked") IsNot Nothing Then
                            oResponse.IsLocked = Convert.ToBoolean(dsUserDetail.Tables(0).Rows(0).Item("is_locked"))
                        End If

                        If dsUserDetail.Tables(0).Rows(0).Item("Is_Temp_Password") IsNot Nothing Then
                            oResponse.IsTempPassword = Convert.ToBoolean(dsUserDetail.Tables(0).Rows(0).Item("Is_Temp_Password"))
                        End If
                        If dsUserDetail.Tables(0).Rows(0).Item("password_change_date") IsNot Nothing Then
                            oResponse.PasswordChangeDate = Convert.ToDateTime(dsUserDetail.Tables(0).Rows(0).Item("password_change_date"))
                        End If
                    End If
                End Using
            End If
        Catch ex As Exception
            Throw
        End Try

        Return oResponse

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="v_iUserId"></param>
    ''' <param name="v_iMode"></param>
    ''' <param name="v_iIncorrectAttemptAllowed"></param>
    ''' <param name="r_iIsLocked"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateIncorrectAttemptsAndLockUnlock(ByVal con As SiriusConnection, ByVal v_iUserId As Integer, ByVal v_iMode As Integer, ByVal v_iIncorrectAttemptAllowed As Integer, ByRef r_iIsLocked As Boolean) As Integer

        Try

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PM_User_Set_Attempts_and_Lock")
                cmd.AddInParameter("@user_id", SqlDbType.Int).Value = v_iUserId
                cmd.AddInParameter("@mode", SqlDbType.Int).Value = v_iMode
                cmd.AddInParameter("@incorrect_attempts_allowed", SqlDbType.Int).Value = v_iIncorrectAttemptAllowed
                cmd.AddOutParameter("@is_locked", SqlDbType.Int).Value = r_iIsLocked
                Dim lReturn As Integer = con.ExecuteNonQuery(cmd)

                If lReturn = 1 Then
                    r_iIsLocked = Convert.ToBoolean(cmd.Parameters("@is_locked").Value)
                End If
            End Using

        Catch ex As Exception
            Throw
        End Try

    End Function

End Class
