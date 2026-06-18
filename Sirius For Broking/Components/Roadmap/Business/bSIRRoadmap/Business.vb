Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.Text
'developer guide no 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ************************************************
    ' Added to replace global variables 10/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)

    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    ' Work Task Control
    Private m_oWrkTaskInstance As bPMWrkTaskInstance.TaskControl

    ' PMUserGroupLookup Object
    Private m_oPMUserGroupLookup As bPMUserGroup.Lookup

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_bCloseDatabase As Boolean

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 13/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel



            ' Get a new instance of component services

            ' Get an instance of the architecture database

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the work manager control

            m_oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
            m_lReturn = m_oWrkTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the bPMUserGroup Business object
            'JAS 11/11/02 for issue 1132

            m_oPMUserGroupLookup = New bPMUserGroup.Lookup
            m_lReturn = m_oPMUserGroupLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bPMUserGroup.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If



            ' Remove component services

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 13/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 13/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCustomerName
    '
    ' Description:
    '
    ' History: 22/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetCustomerName(ByVal v_lPartyCnt As Object, ByRef v_sPartyName As Object) As Integer

        Dim result As Integer = 0
        Dim oParty As Object



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Get an instance of bSIRParty

        'oParty = New bSIRParty.Business
        oParty = Nothing

        result = gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:="bSIRParty.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Dim r_sMessage As String = "Failed to create an instance of bSIRParty.Business"
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSIRParty.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            Return result
        End If


        'm_lReturn = oParty.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=CType(m_oDatabase, dPMDAO.Database))


        '' Remove Component services

        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '    result = gPMConstants.PMEReturnCode.PMFalse
        '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRParty", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCustomerName", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        '    Return result
        'End If

        ' Get the details

        m_lReturn = oParty.GetDetails(vPartyCnt:=v_lPartyCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the values

        m_lReturn = oParty.GetNext(vPartyCnt:=v_lPartyCnt, vResolvedName:=v_sPartyName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Terminate and clear up

        oParty.Dispose()

        oParty = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessDescription
    '
    ' Description: Parses it for any % commands and expands them
    '
    ' History: 31/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessDescription(ByRef r_sDescription As String, Optional ByVal v_sCustomer As String = "") As Integer

        Dim result As Integer = 0
        Dim sNewDesc As New StringBuilder
        Dim sByte_Renamed As New StringsHelper.FixedLengthString(1)



        result = gPMConstants.PMEReturnCode.PMTrue

        sNewDesc = New StringBuilder("")

        ' Check if there is a description
        If r_sDescription.Length = 0 Then
            Return result
        End If

        ' No need to parse it if there's no % in the description
        If (r_sDescription.IndexOf("%"c) + 1) = 0 Then
            ' Set to the old version
            sNewDesc = New StringBuilder(r_sDescription)
            ' Exit out of here
            Return result
        End If

        ' Scan it for %'s
        For iLoop1 As Integer = 1 To r_sDescription.Length

            ' Grab the next character
            sByte_Renamed.Value = r_sDescription.Substring(iLoop1 - 1, 1)

            If sByte_Renamed.Value = "%" Then

                ' Move to the next character
                iLoop1 += 1

                ' Grab the next character
                sByte_Renamed.Value = r_sDescription.Substring(iLoop1 - 1, 1)

                Select Case sByte_Renamed.Value
                    Case "p", "P"
                        ' Customer name
                        If Not False Then
                            sNewDesc.Append(v_sCustomer)
                        End If
                    Case Else

                End Select

            Else

                ' Slap it on the end
                sNewDesc.Append(sByte_Renamed.Value)

            End If

        Next iLoop1

        ' Return the value
        r_sDescription = sNewDesc.ToString()

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateWorkManagerTask
    '
    ' Description: Does what it says on the label
    '
    ' History: 13/06/2000 CTAF - Created.
    '
    ' JAS 11/11/02 added optional v_lUserGroupID v_lUserID for issue 1132
    '***************************************************************** '
    Public Function CreateWorkManagerTask(ByVal v_lPartyCnt As Integer, ByVal v_sDescription As String, ByVal v_sTask As String, ByVal v_lNumDays As Integer, ByVal v_vKeyArray(,) As Object, Optional ByRef v_lUserGroupID As Integer = 0, Optional ByRef v_lUserID As Integer = 0, Optional ByRef v_lTaskGroupId As Integer = 0, Optional ByRef v_lTaskId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lPMWrkTaskID, lPMWrkTaskGroupID, lPMUserGroupID, lPMUserID As Integer
        Dim sCustomer As String = ""
        Dim lPMWrkTaskInstanceCnt As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim dtDueDate As Date
        Dim sSQL As String = ""
        'FSA Phase II

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Only get the customer name if a Party Cnt has been passed in
            If v_lPartyCnt <> 0 Then

                m_lReturn = GetCustomerName(v_lPartyCnt:=v_lPartyCnt, v_sPartyName:=sCustomer)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                sCustomer = ""

            End If

            'FSA Phase III Override Customer if passed as key
            If Informations.IsArray(v_vKeyArray) Then
                For iLoopByKeyArray As Integer = v_vKeyArray.GetLowerBound(1) To v_vKeyArray.GetUpperBound(1)

                    Select Case CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoopByKeyArray)).Trim()
                        Case PMNavKeyConst.PMKeyNameTaskCustomer

                            sCustomer = CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoopByKeyArray))
                            Exit For
                    End Select
                Next iLoopByKeyArray
            End If
            'FSA Phase IIIEnd
            'DJM 09/03/2004 : Order so that the latest one is used. Can get others on system if it has errored.
            sSQL = "SELECT pmwrk_task_group_id, pmuser_group_id,pmwrk_task_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                    "FROM PMWrk_Task_Instance" & Strings.ChrW(13) & Strings.ChrW(10) &
                    "WHERE user_id = {user_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                    "AND task_status = " & CStr(gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress) & Strings.ChrW(13) & Strings.ChrW(10) &
                   "ORDER BY last_modified DESC"

            ' Clear the database parameters

            m_oDatabase.Parameters.Clear()

            ' Add the user_id parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetGroupIDs", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the values
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the task_group_id

            lPMWrkTaskGroupID = CInt(vResultArray(0, 0))
            ' Get the user_group_id

            lPMUserGroupID = CInt(vResultArray(1, 0))
            'Task ID

            lPMWrkTaskID = CInt(vResultArray(2, 0))



            'JAS Start issue 1132 =========================================================

            ' Return the actual values if they are provided
            If Not False Then
                If v_lUserGroupID <> 0 Then
                    lPMUserGroupID = v_lUserGroupID
                End If
            End If


            If Not Informations.IsNothing(v_lUserID) Then
                lPMUserID = v_lUserID
            Else
                lPMUserID = m_iUserID
            End If

            If Not False Then
                If v_lTaskGroupId <> 0 Then
                    lPMWrkTaskGroupID = v_lTaskGroupId
                End If
            End If
            'JAS End==========================================================

            If Not False Then
                If v_lTaskId <> 0 Then
                    lPMWrkTaskID = v_lTaskId
                Else
                    ' Get the task_id
                    sSQL = "SELECT pmwrk_task_id FROM PMWrk_Task WHERE code = '" & v_sTask & "'"


                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskID", bStoredProcedure:=False, vResultArray:=vResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not Informations.IsArray(vResultArray) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get the task_id

                    lPMWrkTaskID = CInt(vResultArray(0, 0))
                End If
            End If

            ' CTAF 020800 - For some reason Date$ returns USA format and this messes up!!!!
            'dtDueDate = DateAdd("d", v_lNumDays, Date$)
            dtDueDate = DateTime.Today.AddDays(v_lNumDays)

            ' Process the description
            m_lReturn = ProcessDescription(r_sDescription:=v_sDescription, v_sCustomer:=sCustomer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create the task in work manager
            ' IMPORTANT NOTE: The status is Incomplete, and not New

            m_lReturn = m_oWrkTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=lPMWrkTaskGroupID, v_lPMWrkTaskID:=lPMWrkTaskID, v_sCustomer:=sCustomer, v_dtTaskDueDate:=dtDueDate, v_lPMUserGroupID:=lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete, v_iIsUrgent:=0, r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_iUserID:=lPMUserID, v_vKeyArray:=v_vKeyArray, v_iIsVisible:=1)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create task in Work Manager.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWorkManagerTask", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkManagerTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkManagerTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTemporaryWMTaskDetails
    '
    ' Description: Gets the cnt value,TaskID,TaskDescription for the in progress wmtask
    '               from PMWrk_Task_Instance table
    '
    ' History: 08/11/2002 JAS - Created.
    '
    ' ***************************************************************** '
    Public Function GetTemporaryWMTaskDetails(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            sSQL = "SELECT ti.pmwrk_task_instance_cnt,t.description " &
                   "FROM PMWrk_Task_Instance ti, PMWrk_Task t " &
                   "WHERE user_id = {user_id} " &
                   " AND ti.task_status = " & CStr(gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress) &
                   " AND t.pmwrk_task_id = ti.pmwrk_task_id"





            ' Clear the database parameters

            m_oDatabase.Parameters.Clear()

            ' Add the user_id parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL Add Params for GetTempWrkCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemporaryWMTaskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTempWrkCnt", bStoredProcedure:=False, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL Select GetTempWrkCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemporaryWMTaskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTemporaryWMTaskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemporaryWMTaskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccountFromParty
    '
    ' Description: Gets an account id, for a party
    '
    ' History: 16/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetAccountFromParty(ByVal v_lPartyCnt As Integer, ByRef r_lAccountID As Integer) As Integer
        Dim result As Integer = 0
        Dim oPartyDB As New dPMDAO.Database
        Dim bCloseDatabase As Boolean
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        'eckPN10993
        Dim vMultiCompany As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'eckPN10993
            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, 1, vMultiCompany)


            ' Have a new instance of dPMDAO in here because we're gonna be switching
            ' databases with it
            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=oPartyDB, v_vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Construct the SQL
            'TR 27/02/04 - For multi branch - need to get exact account for this party at this company
            'eck PN10993 Only check company if running true multi company
            '    sSQL$ = "SELECT account_id FROM Account WHERE account_key = {account_key} AND company_id = {company_id}"
            sSQL = "SELECT account_id FROM Account WHERE account_key = {account_key} "

            If ToSafeDouble(vMultiCompany) = 1 Then
                sSQL = sSQL & " AND company_id = {company_id}"
            End If

            ' Clear the paramters

            oPartyDB.Parameters.Clear()

            ' Add the Account_ID and Company_id paramaters

            m_lReturn = oPartyDB.Parameters.Add(sName:="account_key", vValue:=v_lPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck PN10993
            If ToSafeDouble(vMultiCompany) = 1 Then

                m_lReturn = oPartyDB.Parameters.Add(sName:="company_id", vValue:=m_iSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Execute the SQL

            m_lReturn = oPartyDB.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountFromParty", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Get the result (should only return 1 record now we are filtering by company_id

                r_lAccountID = CInt(vResultArray(0, 0))
            End If

            ' Close if need be
            If bCloseDatabase Then

                m_lReturn = oPartyDB.CloseDatabase()
            End If

            oPartyDB = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountFromParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountFromParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetOSCashForDebit
    '
    ' Description: Gets Outstanding cas for the Debit just posted
    '
    ' History: eck220102 - Created.
    '
    ' ***************************************************************** '
    Public Function GetOSCashForDebit(ByRef r_lAccountID As Integer, ByRef r_sDocumentRef As String, ByRef r_cCash As Decimal) As Integer
        Dim result As Integer = 0
        Dim oOrionDatabase As New dPMDAO.Database
        Dim bCloseDatabase As Boolean
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim vTransAmount As Object = Nothing
        Dim vTransPaid As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Connect to Orion

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=oOrionDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Construct the SQL
            sSQL = "{spu_ACT_Do_Get_OS_Cash_For_Client_Debit}"

            ' Clear the paramters
            With oOrionDatabase


                .Parameters.Clear()

                ' Add paramaters

                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=r_lAccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="document_ref", vValue:=r_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Execute the SQL

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetOSCashForDebit", bStoredProcedure:=True, lNumberRecords:=2, vResultArray:=vResultArray)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Get the result


                vTransAmount = vResultArray(0, 0)


                vTransPaid = vResultArray(1, 0)
            End If


            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vTransAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                r_cCash = CDec(vTransAmount)
            End If


            Dim dbNumericTemp2 As Double
            If Double.TryParse(CStr(vTransPaid), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                r_cCash -= CDec(vTransPaid)
            End If


            ' Close if need be
            If bCloseDatabase Then

                m_lReturn = oOrionDatabase.CloseDatabase()
            End If

            oOrionDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOSCashForDebit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOSCashForDebit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function CreatePMWrkTaskInstanceTask(ByVal v_sTaskCode As String, ByVal v_vKeyArray(,) As Object, ByRef v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oObject As bPMWrkTaskInstance.TaskControl

        Dim lPMWrkTaskGroupID, lPMUserGroupID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get bPMWrkTaskInstance

            oObject = New bPMWrkTaskInstance.TaskControl
            m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMWrkTaskInstance.TaskControl", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePMWrkTaskInstanceTask")
                Return result
            End If

            'Get lPMWrkTaskGroupID
            m_lReturn = GetTaskGroupID(v_sTaskCode:=v_sTaskCode, r_lTaskGroupID:=lPMWrkTaskGroupID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetTaskGroupID(" & v_sTaskCode & ").", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask")
                Return result
            End If

            'Get lPMUserGroupID
            m_lReturn = GetUserGroupID(v_sUserGroupCode:="SYSADMIN", r_lUserGroupID:=lPMUserGroupID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetUserGroupID(SYSADMIN).", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask")
                Return result
            End If

            ' Create the task

            m_lReturn = oObject.CreateNewByCode(v_lPMWrkTaskGroupID:=lPMWrkTaskGroupID, v_sPMWrkTaskCode:=v_sTaskCode, v_sCustomer:="None", v_dtTaskDueDate:=DateTime.Now, v_lPMUserGroupID:=lPMUserGroupID, v_sDescription:=v_sTaskCode, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=gPMConstants.PMEReturnCode.PMFalse, r_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_vKeyArray:=v_vKeyArray, v_iIsVisible:=gPMConstants.PMEReturnCode.PMFalse, v_iUserID:=m_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Work Manager Task", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask")
                Return result
            End If

            'Set Status To InProgress.

            m_lReturn = oObject.SetStatusInProgress(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)

            ' Remove objects

            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePMWrkTaskInstanceTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePMWrkTaskInstanceTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Task and associated bits.
    '
    '
    ' ***************************************************************** '
    Public Function DeletePMWrkTaskInstanceTask(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oObject As bPMWrkTaskInstance.TaskControl

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oObject = New bPMWrkTaskInstance.TaskControl
            m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMWrkTaskInstance.TaskControl", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePMWrkTaskInstanceTask")
                Return result
            End If


            m_lReturn = oObject.SetStatusComplete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = oObject.Delete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Remove objects

            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePMWrkTaskInstanceTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: GetTaskGroupID
    '
    ' Description: Gets the task group that a task code belongs to
    '
    ' History: 13/02/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetTaskGroupID(ByVal v_sTaskCode As String, ByRef r_lTaskGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check for quotes
        v_sTaskCode = v_sTaskCode.Replace("'", "''")

        ' Construct the SQL
        sSQL = "SELECT MIN(tgt.pmwrk_task_group_id) FROM PMWrk_Task_Group_Task tgt " &
               "INNER JOIN pmwrk_task t " &
               "ON t.pmwrk_task_id = tgt.pmwrk_task_id " &
               "WHERE t.code = '" & v_sTaskCode & "'"

        ' Perform the SQL
        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskGroup", bStoredProcedure:=False, vResultArray:=vResultArray)
        End With
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL Failed: " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskGroupID ")
            Return result
        End If

        ' Check the results
        If Informations.IsArray(vResultArray) Then

            r_lTaskGroupID = CInt(vResultArray(0, 0))
        Else
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetUserGroupID
    '
    ' Description:
    '
    ' History: 13/02/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetUserGroupID(ByVal v_sUserGroupCode As String, ByRef r_lUserGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check for quotes
        v_sUserGroupCode = v_sUserGroupCode.Replace("'", "''")

        ' Construct the SQL
        sSQL = "SELECT pmuser_group_id FROM PMUser_Group WHERE code = '" & v_sUserGroupCode & "'"

        ' Perform the SQL
        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetUserGroup", bStoredProcedure:=False, vResultArray:=vResultArray)
        End With
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL Failed" & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroupID")
            Return result
        End If

        ' Check the results
        If Informations.IsArray(vResultArray) Then

            r_lUserGroupID = CInt(vResultArray(0, 0))
        Else
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function
End Class
