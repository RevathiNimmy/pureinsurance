Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.Text
'developer guide no.129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 03/10/2002
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CashListDrawer.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
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

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    'SP to Add a CashList_Drawer
    Private Const ACDirectAddName As String = "AddCashListDrawer"
    Private Const ACDirectAddSQL As String = "{call spu_ACT_Add_CashListDrawer (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"

    'SP to Edit a CashList_Drawer
    Private Const ACDirectEditName As String = "UpdateCashListDrawer"
    Private Const ACDirectEditSQL As String = "{call spu_ACT_Update_CashListDrawer (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"

    'SP to Delete a CashList_Drawer
    Private Const ACDirectDeleteName As String = "DeleteCashListDrawer"
    Private Const ACDirectDeleteSQL As String = "{call spu_ACT_Delete_CashListDrawer (?,?)}"

    'SP to Select all CashListDrawer SQL
    Private Const ACSelAllCashListDrawerName As String = "SelAllCashListDrawer"
    Private Const ACSelAllCashListDrawerSQL As String = "{call spu_ACT_SelAll_CashListDrawer}"

    'SP to Select a single CashListDrawer
    Private Const ACSelectCashListDrawerName As String = "SelectCashListDrawer"
    Private Const ACSelectCashListDrawerSQL As String = "{call spu_ACT_Select_CashListDrawer (?)}"

    'SP to Select all CashList_Drawer_Security for a CashlistDrawer SQL
    Private Const ACSelAllCashListDrawerSecurityName As String = "SelAllCashListDrawerSecurity"
    Private Const ACSelAllCashListDrawerSecuritySQL As String = "{call spu_ACT_SelAll_CashListDrawer_Security (?)}"

    'SP to Add a CashList_Drawer_Security
    Private Const ACAddCashListDrawerSecurityName As String = "AddCashListDrawerSecurity"
    Private Const ACAddCashListDrawerSecuritySQL As String = "{call spu_ACT_Add_CashListDrawerSecurity (?,?,?)}"

    'SP to Update a CashList_Drawer_Security
    Private Const ACUpdateCashListDrawerSecurityName As String = "UpdateCashListDrawerSecurity"
    Private Const ACUpdateCashListDrawerSecuritySQL As String = "{call spu_ACT_Update_CashListDrawerSecurity (?,?,?,?,?)}"

    'SP to Delete a CashList_Drawer_Security
    Private Const ACDeleteCashListDrawerSecurityName As String = "DeleteCashListDrawerSecurity"
    Private Const ACDeleteCashListDrawerSecuritySQL As String = "{call spu_ACT_Delete_CashListDrawerSecurity (?,?,?)}"

    'SP to Select all CashList_Drawer_Banking for a CashlistDrawer SQL
    Private Const ACSelAllCashListDrawerBankingName As String = "SelAllCashListDrawerBanking"
    Private Const ACSelAllCashListDrawerBankingSQL As String = "{call spu_ACT_SelAll_CashListDrawer_Banking (?)}"

    'SP to Add a CashList_Drawer_Banking
    Private Const ACAddCashListDrawerBankingName As String = "AddCashListDrawerBanking"
    Private Const ACAddCashListDrawerBankingSQL As String = "{call spu_ACT_Add_CashListDrawerBanking (?,?)}"

    'SP to Update a CashList_Drawer_Security
    Private Const ACUpdateCashListDrawerBankingName As String = "UpdateCashListDrawerBanking"
    Private Const ACUpdateCashListDrawerBankingSQL As String = "{call spu_ACT_Update_CashListDrawerBanking (?,?,?)}"

    'SP to Delete a CashList_Drawer_Security
    Private Const ACDeleteCashListDrawerBankingName As String = "DeleteCashListDrawerBanking"
    Private Const ACDeleteCashListDrawerBankingSQL As String = "{call spu_ACT_Delete_CashListDrawerBanking (?,?)}"

    'sw added for front office receipting
    Private Const ACSelAllCashDrawerMediaCodesName As String = "SelectCashDrawerMediaCodes"
    Private Const ACSelAllCashDrawerMediaCodesSQL As String = "{call spu_ACT_Sel_CashDrawer_Media_Codes (?)}"

    Private Const ACSelAllCashDrawerReceiptCodesName As String = "SelectCashDrawerReceiptCodes"
    Private Const ACSelAllCashDrawerReceiptCodesSQL As String = "{call spu_ACT_Sel_CashDrawer_Receipt_Codes (?)}"

    'sw end of front office receipting 28-11-2002
    Private Const ACSelUsersForUserGroupsAndBranchesName As String = "SelectUsersForUserGroupsAndBranches"
    Private Const ACSelUsersForUserGroupsAndBranchesSQL As String = "{call spu_ACT_Sel_UsersForUserGroupsAndBranches (?)}"



    'Result Array columns for GetDetails
    Private Const ACDrawerId As Integer = 0
    Private Const ACCompanyId As Integer = 1
    Private Const ACDrawerCode As Integer = 2
    Private Const ACDrawerDesc As Integer = 3
    Private Const ACDrawerMultiUser As Integer = 4
    Private Const ACDrawerBankAccID As Integer = 5
    Private Const ACDrawerDepositAccID As Integer = 6
    Private Const ACDrawerSuspenseAccID As Integer = 7
    Private Const ACDrawerCollectionAccID As Integer = 8
    Private Const ACDrawerAdjustmentAccID As Integer = 9
    Private Const ACDrawerMediaTypeId As Integer = 10
    Private Const ACDrawerCashFloat As Integer = 11
    Private Const ACDrawerCashFloatAmt As Integer = 12
    Private Const ACDrawerGenerateTask As Integer = 13
    Private Const ACDrawerTaskGroupId As Integer = 14
    Private Const ACDrawerTaskStatus As Integer = 15
    Private Const ACDrawerTaskIsUrgent As Integer = 16
    Private Const ACDrawerTaskDescription As Integer = 17
    Private Const ACDrawerTaskDueDays As Integer = 18
    Private Const ACDrawerFutureChqDays As Integer = 19
    Private Const ACDrawerReceiptTypeId As Integer = 20
    Private Const ACDrawerMerchantNumber As Integer = 21
    Private Const ACDrawerAllowReversals As Integer = 22
    Private Const ACDrawerAutoClose As Integer = 23
    Private Const ACDrawerClosed As Integer = 24
    Private Const ACDrawerSubBranchID As Integer = 25

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property
    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property


    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get Reference to Database


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetAllCashListDrawer (Public)
    '
    ' Description: Select multiple CashList_Drawer records from the database.
    '
    '
    ' ***************************************************************** '
    Public Function GetAllCashListDrawer(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelAllCashListDrawerSQL, sSQLName:=ACSelAllCashListDrawerName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all the cash list drawer records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllCashListDrawer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Select a single CashListDrawer record from the database.
    '
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByVal v_lCashlistDrawerId As Integer, Optional ByRef r_vCompanyId As Object = Nothing, Optional ByRef r_vCode As Object = Nothing, Optional ByRef r_vDescription As Object = Nothing, Optional ByRef r_vMultiUser As Object = Nothing, Optional ByRef r_vBankAccountId As Object = Nothing, Optional ByRef r_vDepositBankAccountId As Object = Nothing, Optional ByRef r_vSuspenseAccountId As Object = Nothing, Optional ByRef r_vCollectionAccountId As Object = Nothing, Optional ByRef r_vAdjustmentAccountId As Object = Nothing, Optional ByRef r_vMediaTypeId As Object = Nothing, Optional ByRef r_vCashFloat As Object = Nothing, Optional ByRef r_vCashFloatAmount As Object = Nothing, Optional ByRef r_vGenerateTask As Object = Nothing, Optional ByRef r_vPMUserGroupId As Object = Nothing, Optional ByRef r_vTaskStatus As Object = Nothing, Optional ByRef r_vTaskIsUrgent As Object = Nothing, Optional ByRef r_vTaskDescription As Object = Nothing, Optional ByRef r_vTaskDueDays As Object = Nothing, Optional ByRef r_vFutureChequeDays As Object = Nothing, Optional ByRef r_vCashlistItemReceiptTypeId As Object = Nothing, Optional ByRef r_vMerchantNumber As Object = Nothing, Optional ByRef r_vAllowReversals As Object = Nothing, Optional ByRef r_vAutoClose As Object = Nothing, Optional ByRef r_vClosed As Object = Nothing, Optional ByRef r_vSubBranchID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Const klFirstRow As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ComponentNameID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectCashListDrawerSQL, sSQLName:=ACSelectCashListDrawerName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else
                'Populate the params

                If Not Informations.IsNothing(r_vCompanyId) Then


                    r_vCompanyId = vResultArray(ACCompanyId, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vCode) Then


                    r_vCode = vResultArray(ACDrawerCode, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vDescription) Then


                    r_vDescription = vResultArray(ACDrawerDesc, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vMultiUser) Then


                    r_vMultiUser = vResultArray(ACDrawerMultiUser, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vBankAccountId) Then


                    r_vBankAccountId = vResultArray(ACDrawerBankAccID, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vDepositBankAccountId) Then


                    r_vDepositBankAccountId = vResultArray(ACDrawerDepositAccID, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vSuspenseAccountId) Then


                    r_vSuspenseAccountId = vResultArray(ACDrawerSuspenseAccID, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vCollectionAccountId) Then


                    r_vCollectionAccountId = vResultArray(ACDrawerCollectionAccID, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vAdjustmentAccountId) Then


                    r_vAdjustmentAccountId = vResultArray(ACDrawerAdjustmentAccID, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vMediaTypeId) Then


                    r_vMediaTypeId = vResultArray(ACDrawerMediaTypeId, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vCashFloat) Then


                    r_vCashFloat = vResultArray(ACDrawerCashFloat, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vCashFloatAmount) Then


                    r_vCashFloatAmount = vResultArray(ACDrawerCashFloatAmt, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vGenerateTask) Then


                    r_vGenerateTask = vResultArray(ACDrawerGenerateTask, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vPMUserGroupId) Then


                    r_vPMUserGroupId = vResultArray(ACDrawerTaskGroupId, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vTaskStatus) Then


                    r_vTaskStatus = vResultArray(ACDrawerTaskStatus, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vTaskIsUrgent) Then


                    r_vTaskIsUrgent = vResultArray(ACDrawerTaskIsUrgent, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vTaskDescription) Then


                    r_vTaskDescription = vResultArray(ACDrawerTaskDescription, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vTaskDueDays) Then


                    r_vTaskDueDays = vResultArray(ACDrawerTaskDueDays, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vFutureChequeDays) Then


                    r_vFutureChequeDays = vResultArray(ACDrawerFutureChqDays, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vCashlistItemReceiptTypeId) Then


                    r_vCashlistItemReceiptTypeId = vResultArray(ACDrawerReceiptTypeId, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vMerchantNumber) Then


                    r_vMerchantNumber = vResultArray(ACDrawerMerchantNumber, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vAllowReversals) Then


                    r_vAllowReversals = vResultArray(ACDrawerAllowReversals, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vAutoClose) Then


                    r_vAutoClose = vResultArray(ACDrawerAutoClose, klFirstRow)
                End If

                If Not Informations.IsNothing(r_vClosed) Then


                    r_vClosed = vResultArray(ACDrawerClosed, klFirstRow)
                End If

                ' KG 11/06/03

                If Not Informations.IsNothing(r_vSubBranchID) Then


                    r_vSubBranchID = vResultArray(ACDrawerSubBranchID, klFirstRow)
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the cash list drawer", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllCashListDrawerSecurity (Public)
    '
    ' Description: Selects all security records from the database
    '     for a CashListDrawer
    '
    '
    ' ***************************************************************** '
    Public Function GetAllCashListDrawerSecurity(ByVal v_lCashlistDrawerId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ComponentNameID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelAllCashListDrawerSecuritySQL, sSQLName:=ACSelAllCashListDrawerSecurityName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the cash list drawer security", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllCashListDrawerSecurity", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllCashListDrawerBanking (Public)
    '
    ' Description: Selects all banking Banking records from the database
    '     for a CashListDrawer
    '
    '
    ' ***************************************************************** '
    Public Function GetAllCashListDrawerBanking(ByVal v_lCashlistDrawerId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ComponentNameID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelAllCashListDrawerBankingSQL, sSQLName:=ACSelAllCashListDrawerBankingName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the cash list drawer Banking", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllCashListDrawerBanking", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a CashList_Drawer record to the database
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef r_lCashlistDrawerId As Integer = 0, Optional ByVal v_vCompanyId As Object = Nothing, Optional ByVal v_vCode As Object = Nothing, Optional ByVal v_vDescription As Object = Nothing, Optional ByVal v_vMultiUser As Object = Nothing, Optional ByVal v_vBankAccountId As Object = Nothing, Optional ByVal v_vDepositBankAccountId As Object = Nothing, Optional ByVal v_vSuspenseAccountId As Object = Nothing, Optional ByVal v_vCollectionAccountId As Object = Nothing, Optional ByVal v_vAdjustmentAccountId As Object = Nothing, Optional ByVal v_vMediaTypeId As Byte = 0, Optional ByVal v_vCashFloat As Object = Nothing, Optional ByVal v_vCashFloatAmount As Object = Nothing, Optional ByVal v_vGenerateTask As Object = Nothing, Optional ByVal v_vPMUserGroupId As Object = Nothing, Optional ByVal v_vTaskStatus As Object = Nothing, Optional ByVal v_vTaskIsUrgent As Object = Nothing, Optional ByVal v_vTaskDescription As Object = Nothing, Optional ByVal v_vTaskDueDays As Object = Nothing, Optional ByVal v_vMerchantNumber As Object = Nothing, Optional ByVal v_vFutureChequeDays As Object = Nothing, Optional ByVal v_vCashlistItemReceiptTypeId As Byte = 0, Optional ByVal v_vAllowReversals As Object = Nothing, Optional ByVal v_vAutoClose As Object = Nothing, Optional ByVal v_vClosed As Object = Nothing, Optional ByVal v_vSubBranchId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Business Rules...

            'Ensure that Mandatory parameters have been passed
            m_lReturn = CType(CheckMandatory(r_vDescription:=v_vDescription, r_vMultiUser:=v_vMultiUser, r_vBankAccountId:=v_vBankAccountId, r_vDepositBankAccountId:=v_vDepositBankAccountId, r_vSuspenseAccountId:=v_vSuspenseAccountId, r_vCollectionAccountId:=v_vCollectionAccountId, r_vAdjustmentAccountId:=v_vAdjustmentAccountId, r_vMediaTypeId:=v_vMediaTypeId, r_vCashFloat:=v_vCashFloat, r_vGenerateTask:=v_vGenerateTask, r_vFutureChequeDays:=v_vFutureChequeDays, r_vCashlistItemReceiptTypeId:=v_vCashlistItemReceiptTypeId, r_vAllowReversals:=v_vAllowReversals, r_vAutoClose:=v_vAutoClose, r_vClosed:=v_vClosed), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Default the optional parameters
            m_lReturn = SetCashListDrawerDefaults(r_vCode:=v_vCode, r_vCashFloatAmount:=v_vCashFloatAmount, r_vPMUserGroupId:=v_vPMUserGroupId, r_vTaskStatus:=v_vTaskStatus, r_vTaskIsUrgent:=v_vTaskIsUrgent, r_vTaskDescription:=v_vTaskDescription, r_vTaskDueDays:=v_vTaskDueDays, r_vMerchantNumber:=v_vMerchantNumber)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Ensure that the CashListDrawer contains valid data
            m_lReturn = CType(ValidateCashListDrawer(r_vCode:=v_vCode, r_vDescription:=v_vDescription, r_vMultiUser:=v_vMultiUser, r_vBankAccountId:=v_vBankAccountId, r_vDepositBankAccountId:=v_vDepositBankAccountId, r_vSuspenseAccountId:=v_vSuspenseAccountId, r_vCollectionAccountId:=v_vCollectionAccountId, r_vAdjustmentAccountId:=v_vAdjustmentAccountId, r_vMediaTypeId:=v_vMediaTypeId, r_vCashFloat:=v_vCashFloat, r_vCashFloatAmount:=v_vCashFloatAmount, r_vGenerateTask:=v_vGenerateTask, r_vPMUserGroupId:=v_vPMUserGroupId, r_vTaskStatus:=v_vTaskStatus, r_vTaskIsUrgent:=v_vTaskIsUrgent, r_vTaskDescription:=v_vTaskDescription, r_vTaskDueDays:=v_vTaskDueDays, r_vMerchantNumber:=v_vMerchantNumber, r_vFutureChequeDays:=v_vFutureChequeDays, r_vCashlistItemReceiptTypeId:=v_vCashlistItemReceiptTypeId, r_vAllowReversals:=v_vAllowReversals, r_vAutoClose:=v_vAutoClose, r_vClosed:=v_vClosed), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlist_drawer_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(r_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add company_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_vCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add code as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(v_vCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add description as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(v_vDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add multi_user as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="multi_user", vValue:=CStr(v_vMultiUser), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add bank_account_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="bankaccount_id", vValue:=CStr(v_vBankAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add deposit_bankaccount_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="deposit_bankaccount_id", vValue:=CStr(v_vDepositBankAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add suspense_account_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="suspense_account_id", vValue:=CStr(v_vSuspenseAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add collection_account_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="collection_account_id", vValue:=CStr(v_vCollectionAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add adjustment_account_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="adjustment_account_id", vValue:=CStr(v_vAdjustmentAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_vMediaTypeId <> 0 Then
                ' Add media_type_id as an input param for an insert
                'only pass if <> 0 (a default is being set)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="media_type_id", vValue:=CStr(v_vMediaTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Add media_type_id as an input param for an insert
                'only pass if <> 0 (a default is being set)

                'developer guide no.85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="media_type_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Add cash_float as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cash_float", vValue:=CStr(v_vCashFloat), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add cash_float_amount as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cash_float_amount", vValue:=CStr(v_vCashFloatAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add generate_task as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="generate_task", vValue:=CStr(v_vGenerateTask), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add pmuser_group_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(v_vPMUserGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add task_status as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_status", vValue:=CStr(v_vTaskStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add task_is_urgent as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_is_urgent", vValue:=CStr(v_vTaskIsUrgent), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add task_description as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_description", vValue:=CStr(v_vTaskDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add task_due_days as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_due_days", vValue:=CStr(v_vTaskDueDays), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add future_cheque_days as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="future_cheque_days", vValue:=CStr(v_vFutureChequeDays), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_vCashlistItemReceiptTypeId <> 0 Then
                ' Add cashlistitem_receipt_type_id as an input param for an insert.
                'if = 0 then no default will be set
                m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_receipt_type_id", vValue:=CStr(v_vCashlistItemReceiptTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Add cashlistitem_receipt_type_id as an input param for an insert.
                'if = 0 then no default will be set

                'developer guide no.85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_receipt_type_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Add merchant_number as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="merchant_number", vValue:=CStr(v_vMerchantNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add allow_reversals as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_reversals", vValue:=CStr(v_vAllowReversals), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add auto_close as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="auto_close", vValue:=CStr(v_vAutoClose), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add closed as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="closed", vValue:=CStr(v_vClosed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' KG 10/06/03
            ' Add Sub_Branch_ID as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=CStr(v_vSubBranchId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectAddSQL, sSQLName:=ACDirectAddName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't added, error
            If lRecordsAffected > 0 Then
                ' Added, so return Id of new record if required
                If Not False Then
                    r_lCashlistDrawerId = m_oDatabase.Parameters.Item("cashlist_drawer_id").Value
                End If
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectEdit (Public)
    '
    ' Description: Updates a CashList_Drawer record to the database
    '
    ' ***************************************************************** '
    Public Function DirectEdit(ByVal v_lCashlistDrawerId As Integer, ByVal v_vCompanyId As Object, ByVal v_vCode As Object, ByVal v_vDescription As Object, ByVal v_vMultiUser As Object, ByVal v_vBankAccountId As Object, ByVal v_vDepositBankAccountId As Object, ByVal v_vSuspenseAccountId As Object, ByVal v_vCollectionAccountId As Object, ByVal v_vAdjustmentAccountId As Object, ByVal v_vMediaTypeId As Byte, ByVal v_vCashFloat As Object, ByVal v_vCashFloatAmount As Object, ByVal v_vGenerateTask As Object, ByVal v_vPMUserGroupId As Object, ByVal v_vTaskStatus As Object, ByVal v_vTaskIsUrgent As Object, ByVal v_vTaskDescription As Object, ByVal v_vTaskDueDays As Object, ByVal v_vFutureChequeDays As Object, ByVal v_vCashlistItemReceiptTypeId As Byte, ByVal v_vMerchantNumber As Object, ByVal v_vAllowReversals As Object, ByVal v_vAutoClose As Object, ByVal v_vClosed As Object, ByVal v_vSubBranchId As Object) As Integer


        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Ensure that the CashListDrawer contains valid data
            m_lReturn = CType(ValidateCashListDrawer(r_vCode:=v_vCode, r_vDescription:=v_vDescription, r_vMultiUser:=v_vMultiUser, r_vBankAccountId:=v_vBankAccountId, r_vDepositBankAccountId:=v_vDepositBankAccountId, r_vSuspenseAccountId:=v_vSuspenseAccountId, r_vCollectionAccountId:=v_vCollectionAccountId, r_vAdjustmentAccountId:=v_vAdjustmentAccountId, r_vMediaTypeId:=v_vMediaTypeId, r_vCashFloat:=v_vCashFloat, r_vCashFloatAmount:=v_vCashFloatAmount, r_vGenerateTask:=v_vGenerateTask, r_vPMUserGroupId:=v_vPMUserGroupId, r_vTaskStatus:=v_vTaskStatus, r_vTaskIsUrgent:=v_vTaskIsUrgent, r_vTaskDescription:=v_vTaskDescription, r_vTaskDueDays:=v_vTaskDueDays, r_vMerchantNumber:=v_vMerchantNumber, r_vFutureChequeDays:=v_vFutureChequeDays, r_vCashlistItemReceiptTypeId:=v_vCashlistItemReceiptTypeId, r_vAllowReversals:=v_vAllowReversals, r_vAutoClose:=v_vAutoClose, r_vClosed:=v_vClosed), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlist_drawer_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add company_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_vCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add code as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(v_vCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add description as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(v_vDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add suspense_account_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="suspense_account_id", vValue:=CStr(v_vSuspenseAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add collection_account_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="collection_account_id", vValue:=CStr(v_vCollectionAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add adjustment_account_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="adjustment_account_id", vValue:=CStr(v_vAdjustmentAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add multi_user as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="multi_user", vValue:=CStr(v_vMultiUser), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add bank_account_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="bankaccount_id", vValue:=CStr(v_vBankAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add deposit_bankaccount_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="deposit_bankaccount_id", vValue:=CStr(v_vDepositBankAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_vMediaTypeId <> 0 Then
                ' Add media_type_id as an input param for an insert
                'only pass if <> 0 (a default is being set)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="media_type_id", vValue:=CStr(v_vMediaTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Add media_type_id as an input param for an insert
                'only pass if <> 0 (a default is being set)

                'developer guide no.85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="media_type_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Add cash_float as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cash_float", vValue:=CStr(v_vCashFloat), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add cash_float_amount as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cash_float_amount", vValue:=CStr(v_vCashFloatAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add generate_task as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="generate_task", vValue:=CStr(v_vGenerateTask), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add pmuser_group_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(v_vPMUserGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add task_status as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_status", vValue:=CStr(v_vTaskStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add task_is_urgent as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_is_urgent", vValue:=CStr(v_vTaskIsUrgent), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add task_description as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_description", vValue:=CStr(v_vTaskDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add task_due_days as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_due_days", vValue:=CStr(v_vTaskDueDays), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add future_cheque_days as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="future_cheque_days", vValue:=CStr(v_vFutureChequeDays), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_vCashlistItemReceiptTypeId <> 0 Then
                ' Add cashlistitem_receipt_type_id as an input param for an insert.
                'if = 0 then no default will be set
                m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_receipt_type_id", vValue:=CStr(v_vCashlistItemReceiptTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Add cashlistitem_receipt_type_id as an input param for an insert.
                'if = 0 then no default will be set

                'developer guide no.85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_receipt_type_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            ' Add merchant_number as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="merchant_number", vValue:=CStr(v_vMerchantNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add allow_reversals as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_reversals", vValue:=CStr(v_vAllowReversals), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add auto_close as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="auto_close", vValue:=CStr(v_vAutoClose), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add closed as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="closed", vValue:=CStr(v_vClosed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' KG 10/06/03
            ' Add Sub_Branch_ID as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=CStr(v_vSubBranchId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectEditSQL, sSQLName:=ACDirectEditName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't edited, error
            If lRecordsAffected > 0 Then
                ' Updated, No action required

            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectEdit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a CashList_Drawer record and any child
    '              (banking / security) records
    '
    '(NOTE - record will not be deleted if drawer is used)
    '
    ' ***************************************************************** '
    Public Function DirectDelete(ByVal v_lCashlistDrawerId As Integer, ByRef r_lDrawerUsed As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlist_drawer_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add cashlist_drawer_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="drawer_used", vValue:=CStr(r_lDrawerUsed), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectDeleteSQL, sSQLName:=ACDirectDeleteName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return if the drawer was used or not
            '(NOTE - record will not be deleted if drawer is used)
            r_lDrawerUsed = m_oDatabase.Parameters.Item("drawer_used").Value

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSubBranches
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '          10/06/2003 KG - Pasted this module in.
    '
    ' ***************************************************************** '
    Public Function GetSubBranches(ByVal v_lSourceID As Integer, ByRef r_vSubBranchArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return SiriusCoreFunc.GetSubBranches(v_oDatabase:=m_oDatabase, v_lSourceID:=v_lSourceID, r_vSubBranchArray:=r_vSubBranchArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranches", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddCashListDrawerSecurity (Public)
    '
    ' Description: Adds a CashList_Drawer_Security record to the database
    '
    ' ***************************************************************** '
    Public Function AddCashListDrawerSecurity(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserGroupId As Integer, ByVal v_lCompanyId As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlist_drawer_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add pmuser_group_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(v_lUserGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add @company_id  as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_lCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddCashListDrawerSecuritySQL, sSQLName:=ACAddCashListDrawerSecurityName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't added, error
            If lRecordsAffected > 0 Then
                ' Added, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCashListDrawerSecurity Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCashListDrawerSecurity", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: AddCashListDrawerBanking (Public)
    '
    ' Description: Adds a CashList_Drawer_Banking record to the database
    '
    ' ***************************************************************** '
    Public Function AddCashListDrawerBanking(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserId As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlist_drawer_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add pmuser_group_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddCashListDrawerBankingSQL, sSQLName:=ACAddCashListDrawerBankingName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't added, error
            If lRecordsAffected > 0 Then
                ' Added, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCashListDrawerBanking Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCashListDrawerBanking", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateCashListDrawerSecurity (Public)
    '
    ' Description: Updates a CashList_Drawer_Security record on the database
    '
    ' ***************************************************************** '
    Public Function UpdateCashListDrawerSecurity(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserGroupId As Integer, ByVal v_lCompanyId As Integer, ByVal v_lNewUserGroupId As Integer, ByVal v_lNewCompanyId As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlist_drawer_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add pmuser_group_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(v_lUserGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add @company_id  as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_lCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add new_pmuser_group_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_pmuser_group_id", vValue:=CStr(v_lNewUserGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add new_company_id  as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_company_id", vValue:=CStr(v_lNewCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCashListDrawerSecuritySQL, sSQLName:=ACUpdateCashListDrawerSecurityName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't added, error
            If lRecordsAffected > 0 Then
                ' Updated, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCashListDrawerSecurity Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCashListDrawerSecurity", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateCashListDrawerBanking (Public)
    '
    ' Description: Updates a CashList_Drawer_Banking record on the database
    '
    ' ***************************************************************** '
    Public Function UpdateCashListDrawerBanking(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserId As Integer, ByVal v_lNewUserId As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlist_drawer_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add pmuser_group_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add new_pmuser_group_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_pmuser_id", vValue:=CStr(v_lNewUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCashListDrawerBankingSQL, sSQLName:=ACUpdateCashListDrawerBankingName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't added, error
            If lRecordsAffected > 0 Then
                ' Updated, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCashListDrawerBanking Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCashListDrawerBanking", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteCashListDrawerSecurity (Public)
    '
    ' Description: Deletes a CashList_Drawer_Security record from the database
    '
    ' ***************************************************************** '
    Public Function DeleteCashListDrawerSecurity(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserGroupId As Integer, ByVal v_lCompanyId As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlist_drawer_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add pmuser_group_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(v_lUserGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add @company_id  as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_lCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteCashListDrawerSecuritySQL, sSQLName:=ACDeleteCashListDrawerSecurityName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't added, error
            If lRecordsAffected > 0 Then
                ' Deleted, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteCashListDrawerSecurity Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteCashListDrawerSecurity", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteCashListDrawerBanking (Public)
    '
    ' Description: Deletes a CashList_Drawer_Banking record from the database
    '
    ' ***************************************************************** '
    Public Function DeleteCashListDrawerBanking(ByVal v_lCashlistDrawerId As Integer, ByVal v_lUserId As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlist_drawer_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashlistDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add pmuser_group_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteCashListDrawerBankingSQL, sSQLName:=ACDeleteCashListDrawerBankingName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't added, error
            If lRecordsAffected > 0 Then
                ' Deleted, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteCashListDrawerBanking Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteCashListDrawerBanking", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AddInputParam) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AddInputParam(ByRef r_vNames As Object, ByRef r_vTypes As Object, ByRef r_vValues As Object) As Integer
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'With m_oDatabase

    'For 'iItem As Integer = r_vNames.GetLowerBound(0) To r_vNames.GetUpperBound(0)



    'm_lReturn = .Parameters.Add(sName:=CStr(r_vNames(iItem)), vValue:=CStr(r_vValues(iItem)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=CType(Conversion.Val(CStr(r_vTypes(iItem))), gPMConstants.PMEDataType))
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'Next iItem
    'End With
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oParameters.Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddInputParam", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: PickListLoad
    '
    ' Description: Standard method for the PickList control to call
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function PickListLoad(ByRef sPickListType As String, ByRef vFKArray As Object, ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0

        Try

            With m_oDatabase
                .Parameters.Clear()

                'Load the parameters

                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Next iParam

                'Call the SP
                m_lReturn = .SQLSelect("{call spu_ACT_CashListDrawer_PLL" &
                            sPickListType & "(" & PickListParams(vFKArray) & ")}", sPickListType & " PickList Load", True, , vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Select Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return m_lReturn
                End If
            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListSave
    '
    ' Description:
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys() As Object) As Integer

        Dim result As Integer = 0

        Try

            BeginTrans()

            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                'Load the FK parameters

                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Next iParam

                m_lReturn = .SQLAction("{call spu_ACT_CashListDrawer_PLD" &
                            sPickListType & "(" & PickListParams(vFKArray) & ")}", sPickListType & " PickList Delete", True)

                'See if there is anything to save
                If Informations.IsArray(vKeys) Then

                    For iKey As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)
                        .Parameters.Clear()

                        'Load the FK parameters

                        For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                            .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        Next iParam


                        .Parameters.Add("Key", CStr(vKeys(iKey)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        'Call the SP
                        m_lReturn = .SQLAction("{call spu_ACT_CashListDrawer_PLS" &
                                    sPickListType & "(" & PickListParams(vFKArray) & ",?)}", sPickListType & " PickList Save", True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Write Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            RollbackTrans()
                            Return m_lReturn
                        End If
                    Next iKey
                End If
            End With

            CommitTrans()

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            RollbackTrans()
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListParams
    '
    ' Description: Returns a string of question marks for the SP definition
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Private Function PickListParams(ByRef vParams(,) As Object) As String

        Dim result As String = String.Empty


        Dim sComma As String = ""
        Dim sParam As New StringBuilder

        sComma = ""
        sParam = New StringBuilder("")

        For iParam As Integer = vParams.GetLowerBound(1) To vParams.GetUpperBound(1)
            sParam.Append(sComma & "?")
            sComma = ","
        Next iParam


        Return sParam.ToString()

    End Function


    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Private Function CheckMandatory(ByRef r_vDescription As Object, ByRef r_vMultiUser As Object, ByRef r_vBankAccountId As Object, ByRef r_vDepositBankAccountId As Object, ByRef r_vSuspenseAccountId As Object, ByRef r_vCollectionAccountId As Object, ByRef r_vAdjustmentAccountId As Object, ByRef r_vMediaTypeId As Object, ByRef r_vCashFloat As Object, ByRef r_vGenerateTask As Object, ByRef r_vFutureChequeDays As Object, ByRef r_vCashlistItemReceiptTypeId As Object, ByRef r_vAllowReversals As Object, ByRef r_vAutoClose As Object, ByRef r_vClosed As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CheckMandatory
        ' PURPOSE: Ensure mandatory parameters have been passed
        ' AUTHOR: Paul Cunnigham
        ' DATE: 25 October 2002, 09:31:59
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        'Test to see if parameters are missing and exit (returning PMFalse)
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result
        If False Then Return result

        'All mandatory params have been passed
        result = gPMConstants.PMEReturnCode.PMTrue

        Return result


    End Function

    Private Function ValidateCashListDrawer(ByRef r_vCode As Object, ByRef r_vDescription As Object, ByRef r_vMultiUser As Object, ByRef r_vBankAccountId As Object, ByRef r_vDepositBankAccountId As Object, ByRef r_vSuspenseAccountId As Object, ByRef r_vCollectionAccountId As Object, ByRef r_vAdjustmentAccountId As Object, ByRef r_vMediaTypeId As Object, ByRef r_vCashFloat As Object, ByRef r_vCashFloatAmount As Object, ByRef r_vGenerateTask As Object, ByRef r_vPMUserGroupId As Object, ByRef r_vTaskStatus As Object, ByRef r_vTaskIsUrgent As Object, ByRef r_vTaskDescription As Object, ByRef r_vTaskDueDays As Object, ByRef r_vMerchantNumber As Object, ByRef r_vFutureChequeDays As Object, ByRef r_vCashlistItemReceiptTypeId As Object, ByRef r_vAllowReversals As Object, ByRef r_vAutoClose As Object, ByRef r_vClosed As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ValidateCashListDrawer
        ' PURPOSE: Ensure mandatory parameters have been passed
        ' AUTHOR: Paul Cunnigham
        ' DATE: 25 October 2002, 09:31:59
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        'Test to see if parameters are missing and exit (returning PMFalse)

        'If not (?(v_vCode) Or _
        'IsNull(r_vCashFloatAmount)) Then Exit Function

        If (CStr(r_vDescription)).Length = 0 Then Return result

        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(r_vMultiUser), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then Return result

        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(r_vBankAccountId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then Return result

        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(r_vDepositBankAccountId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then Return result

        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(r_vSuspenseAccountId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then Return result

        Dim dbNumericTemp5 As Double
        If Not Double.TryParse(CStr(r_vCollectionAccountId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then Return result

        Dim dbNumericTemp6 As Double
        If Not Double.TryParse(CStr(r_vAdjustmentAccountId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then Return result

        Dim dbNumericTemp7 As Double
        If Not Double.TryParse(CStr(r_vMediaTypeId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then Return result

        Dim dbNumericTemp8 As Double
        If Not Double.TryParse(CStr(r_vCashFloat), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then Return result


        Dim dbNumericTemp9 As Double
        If Not (Double.TryParse(CStr(r_vCashFloatAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9) Or Convert.IsDBNull(r_vCashFloatAmount) Or Informations.IsNothing(r_vCashFloatAmount)) Then Return result

        Dim dbNumericTemp10 As Double
        If Not Double.TryParse(CStr(r_vGenerateTask), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp10) Then Return result


        Dim dbNumericTemp11 As Double
        If Not (Double.TryParse(CStr(r_vPMUserGroupId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp11) Or Convert.IsDBNull(r_vPMUserGroupId) Or Informations.IsNothing(r_vPMUserGroupId)) Then Return result


        Dim dbNumericTemp12 As Double
        If Not (Double.TryParse(CStr(r_vTaskStatus), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp12) Or Convert.IsDBNull(r_vTaskStatus) Or Informations.IsNothing(r_vTaskStatus)) Then Return result


        Dim dbNumericTemp13 As Double
        If Not (Double.TryParse(CStr(r_vTaskIsUrgent), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp13) Or Convert.IsDBNull(r_vTaskIsUrgent) Or Informations.IsNothing(r_vTaskIsUrgent)) Then Return result
        'If Not (?(r_vTaskDescription) Or _
        'IsNull(r_vTaskDescription)) Then Exit Function


        Dim dbNumericTemp14 As Double
        If Not (Double.TryParse(CStr(r_vTaskDueDays), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp14) Or Convert.IsDBNull(r_vTaskDueDays) Or Informations.IsNothing(r_vTaskDueDays)) Then Return result
        'If not (?(r_vMerchantNumber) Or _
        'IsNull(r_vMerchantNumber)) Then Exit Function

        Dim dbNumericTemp15 As Double
        If Not Double.TryParse(CStr(r_vFutureChequeDays), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp15) Then Return result

        Dim dbNumericTemp16 As Double
        If Not Double.TryParse(CStr(r_vCashlistItemReceiptTypeId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp16) Then Return result

        Dim dbNumericTemp17 As Double
        If Not Double.TryParse(CStr(r_vAllowReversals), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp17) Then Return result

        Dim dbNumericTemp18 As Double
        If Not Double.TryParse(CStr(r_vAutoClose), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp18) Then Return result

        Dim dbNumericTemp19 As Double
        If Not Double.TryParse(CStr(r_vClosed), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp19) Then Return result

        'All mandatory params have been passed
        result = gPMConstants.PMEReturnCode.PMTrue

        Return result


    End Function

    Private Function SetCashListDrawerDefaults(ByRef r_vCode As Object, ByRef r_vCashFloatAmount As Object, ByRef r_vPMUserGroupId As Object, ByRef r_vTaskStatus As Object, ByRef r_vTaskIsUrgent As Object, ByRef r_vTaskDescription As Object, ByRef r_vTaskDueDays As Object, ByRef r_vMerchantNumber As Object) As gPMConstants.PMEReturnCode
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SetCashListDrawerDefaults
        ' PURPOSE: Set default values for any optional parameters that weren't passed
        ' AUTHOR: Paul Cunnigham
        ' DATE: 25 October 2002, 09:46:14
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse



        If False Then


            r_vCode = DBNull.Value
        End If
        If False Then


            r_vCashFloatAmount = DBNull.Value
        End If
        If False Then


            r_vPMUserGroupId = DBNull.Value
        End If
        If False Then


            r_vTaskStatus = DBNull.Value
        End If
        If False Then


            r_vTaskIsUrgent = DBNull.Value
        End If
        If False Then


            r_vTaskDescription = DBNull.Value
        End If
        If False Then


            r_vTaskDueDays = DBNull.Value
        End If
        If False Then


            r_vMerchantNumber = DBNull.Value
        End If

        result = gPMConstants.PMEReturnCode.PMTrue

        Return result


    End Function



    ' ***************************************************************** '
    '
    ' Name: GetCashDrawerMediaTypes
    '
    ' Description: returns valid media types for a given cash drawer
    '
    ' History: 25-10-2002 - sw - Created.
    '
    ' ***************************************************************** '

    Public Function GetCashDrawerMediaTypes(ByVal v_lCashDrawerID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim sSQL As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashdrawerid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashdrawerid", vValue:=CStr(v_lCashDrawerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelAllCashDrawerMediaCodesSQL, sSQLName:=ACSelAllCashDrawerMediaCodesName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashDrawerMediaTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashDrawerMediaTypes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetCashDrawerReceiptTypes
    '
    ' Description: returns valid receipt types for a given cash drawer
    '
    ' History: 25-10-2002 - Created.
    '
    ' ***************************************************************** '

    Public Function GetCashDrawerReceiptTypes(ByVal v_lCashDrawerID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashdrawerid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashdrawerid", vValue:=CStr(v_lCashDrawerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelAllCashDrawerReceiptCodesSQL, sSQLName:=ACSelAllCashDrawerReceiptCodesName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashDrawerReceiptTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashDrawerReceiptTypes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetUsersForUserGroupsAndBranches
    '
    ' Description: Returns valid users for the arrays of user groups and branches paramters
    '
    ' History: 28-11-2002 - Created.
    '
    ' ***************************************************************** '

    Public Function GetUsersForUserGroupsAndBranches(ByVal v_lCashDrawerID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            ' Add cashdrawerid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashdrawerid", vValue:=CStr(v_lCashDrawerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelUsersForUserGroupsAndBranchesSQL, sSQLName:=ACSelUsersForUserGroupsAndBranchesName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUsersForUserGroupsAndBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUsersForUserGroupsAndBranches", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
