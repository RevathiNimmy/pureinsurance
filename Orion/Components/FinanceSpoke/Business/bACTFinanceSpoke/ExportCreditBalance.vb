Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportCreditBalance
    '************************************************************************
    ' Class/Module: ExportCreditBalance
    '
    ' Description : Identifies all Accounts with a Credit Balance older than
    '               x number of months. This is a "trigger" export class that
    '               does not return any data at the moment.
    '
    ' Created: DD 20/02/2003
    '************************************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ExportCreditBalance"

    ' Private variables
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oBusiness As Business
    Private m_oDatabase As dPMDAO.Database

    ' ************************************************
    ' Added to replace global variables 24/09/2003
    ' Username.
    Private m_sUsername As String = ""
    ' Password.
    Private m_sPassword As String = ""
    ' User ID
    Private m_iUserID As Integer
    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constants for the HeaderData array
    Private Const kbHDAgeMonths As Byte = 9

    ' Component Object variables
    Private m_oFindTransaction As bACTFindTransaction.Business

    ' Stored procedures
    'developer guide no. 39
    Private Const ksSPExportCreditBalanceSQL As String = "spu_ACT_Spoke_ExportCreditBalance"
    Private Const ksSPExportCreditBalanceName As String = "GetExportCreditBalance"
    Private Const ksSPExportCreditBalanceStored As Boolean = True

    'developer guide no. 39
    Private Const ksSPGetPMUserGroupSQL As String = "spu_ACT_Spoke_GetPMUserGroup"
    Private Const ksSPGetPMUserGroupName As String = "GetPMUserGroup"
    Private Const ksSPGetPMUserGroupStored As Boolean = True

    'developer guide no. 39
    Private Const ksSPCheckTaskExistsSQL As String = "spu_ACT_Spoke_CheckTaskExists"
    Private Const ksSPCheckTaskExistsName As String = "CheckTaskExists"
    Private Const ksSPCheckTaskExistsStored As Boolean = True

    Private Const ksTaskDescription As String = "Credit Balance Requires Action"

    Friend WriteOnly Property Business() As Business
        Set(ByVal Value As Business)

            m_oBusiness = Value

        End Set
    End Property
    Friend WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property


    Friend Function PassThroughLogin(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef sCallingAppName As String, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PassThroughLogin
        ' PURPOSE: Pass through the module level login information to the Class.
        ' This is for COM+. Normally a business class will not require this but the Spoke
        ' design means that Classes are instantiated by the Business class and can
        ' no longer rely on global variables.
        ' AUTHOR: Danny Davis
        ' DATE: 24 September 2003, 11:55 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iSourceID = iSourceID
            m_iLanguageID = iLanguageID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PassThroughLogin", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function


    '*************************************************************************
    ' Name: ProcessCreditBalanceAccounts
    '
    ' Description: This function loops through all Accounts and produces Work
    '              Manager tasks to follow up Credit Balances of a certain age.
    '
    '              Unlike other Export classes, this one does not return any
    '              records, so a local variant array is declared to store the
    '              returned records from the initial select stored procedure.
    '
    '              In the future this array could be returned through the
    '              r_vResultArray parameter.
    '
    ' Created: DD 20/02/2003
    '*************************************************************************

    Private Function ProcessCreditBalanceAccounts(ByVal v_iAgeMonths As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim bInstalment As Boolean
        Dim lPMUserGroupID As Integer

        Dim lUserGroupID As Integer




        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add AgeMonths as an input param
        If m_oDatabase.Parameters.Add(sName:="AgeMonths", vValue:=CStr(v_iAgeMonths), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement to get Credit Control Items
        If m_oDatabase.SQLSelect(sSQL:=ksSPExportCreditBalanceSQL, sSQLName:=ksSPExportCreditBalanceName, bStoredProcedure:=ksSPExportCreditBalanceStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If



        r_vResultArray = vResultArray

        ' Process only if something returned
        If Information.IsArray(vResultArray) Then

            ' Loop through all Accounts found

            For lIndex As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                'Get the appropriate User Group for the Account

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add Account_id as an input param

                If m_oDatabase.Parameters.Add(sName:="Account_ID", vValue:=CStr(vResultArray(0, lIndex)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                ' The User Group is an output param
                If m_oDatabase.Parameters.Add(sName:="PMUserGroupID", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                ' Execute SQL Statement to get User Group
                If m_oDatabase.SQLAction(sSQL:=ksSPGetPMUserGroupSQL, sSQLName:=ksSPGetPMUserGroupName, bStoredProcedure:=ksSPGetPMUserGroupStored) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                lPMUserGroupID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("PMUserGroupID").Value)

                'Get the returned User Group - if zero then skip task
                If lPMUserGroupID > 0 Then
                    'Check to see if the Task Exists
                    m_oDatabase.Parameters.Clear()

                    ' Add Task Code as an input param
                    If m_oDatabase.Parameters.Add(sName:="TaskCode", vValue:="FINDTXN", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    ' The User Group is an output param
                    If m_oDatabase.Parameters.Add(sName:="PMUserGroupID", vValue:=CStr(lPMUserGroupID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    ' Add Customer string as an input param

                    If m_oDatabase.Parameters.Add(sName:="Customer", vValue:=CStr(vResultArray(1, lIndex)).Trim(), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    ' Add Description as an input param
                    If m_oDatabase.Parameters.Add(sName:="Description", vValue:=ksTaskDescription.Substring(0, 10), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    ' Execute SQL Statement to get User Group
                    If m_oDatabase.SQLSelect(sSQL:=ksSPCheckTaskExistsSQL, sSQLName:=ksSPCheckTaskExistsName, bStoredProcedure:=ksSPCheckTaskExistsStored, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    ' If there are no existing tasks then create one
                    If m_oDatabase.Records.Count() = 0 Then
                        'Create Task


                        If AddTaskToWorkManager(v_lUserGroupID:=lPMUserGroupID, v_lAccountId:=CInt(vResultArray(0, lIndex)), v_sAccountCode:=CStr(vResultArray(1, lIndex)).Trim()) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return result
                        End If
                    End If
                End If
            Next

        End If


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    '*********************************************************************
    ' Name: Start
    '
    ' Description: Start process for use case
    '
    ' Created: DD 20/02/2003
    '*********************************************************************
    Friend Function Start(ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef v_vHeaderData() As Object) As Integer

        Dim result As Integer = 0
        Dim iAgeMonths As Integer
        Dim vResults As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED

            'We need valid database and business objects
            If (m_oBusiness Is Nothing) Or (m_oDatabase Is Nothing) Then
                Return result
            End If

            ' Create an instance of the Find Transaction object
            m_oFindTransaction = New bACTFindTransaction.Business
            If m_oFindTransaction.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'OK do the Export processing...

            ' Assign the values

            iAgeMonths = CInt(v_vHeaderData(conValue)(kbHDAgeMonths))

            ' Process the Accounts based on the
            ' passed criteria
            If ProcessCreditBalanceAccounts(v_iAgeMonths:=iAgeMonths, r_vResultArray:=vResults) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Kill the instance of the Find Transaction object

            m_oFindTransaction.Dispose()
            m_oFindTransaction = Nothing

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE


            If Information.IsArray(vResults) Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch
        End Try



        ' Log Error.
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return gPMConstants.PMEReturnCode.PMError

    End Function

    ' ***************************************************************** '
    '
    ' Name: AddTaskToWorkManager
    '
    ' Description: add task to work manager
    '
    ' History: DD 20/02/2003 Taken from iACTFindTransaction
    '
    ' ***************************************************************** '
    Private Function AddTaskToWorkManager(ByVal v_lUserGroupID As Integer, ByVal v_lAccountId As Integer, ByVal v_sAccountCode As String) As Integer


        Dim result As Integer = 0
        Dim lTaskInstanceCnt As Integer
        Dim sTaskDesc, vCashListRef As String
        Dim vListDate As Date
        Dim vKeyArray(,) As Object
        Dim lAccountId As Integer
        Dim sAccountCode As String = ""





        result = gPMConstants.PMEReturnCode.PMTrue

        ' populate the key array
        ReDim vKeyArray(2, 0)

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lAccountId

        ' make the call to create the task

        m_lReturn = m_oFindTransaction.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:=v_sAccountCode, v_sDescription:=ksTaskDescription, v_dtTaskDueDate:=DateTime.Now.AddDays(7), v_sTaskCode:="FINDTXN", v_sTaskGroupCode:="PLACS", v_lUserGroupID:=v_lUserGroupID, v_vKeyArray:=vKeyArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
End Class

