Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Text
'developer guide no 129. 
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 23-07-1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Account.
    '
    ' Edit History: TF131198 -  AccountKey property added
    '                           Redundant bits removed
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Collection of Accounts (Private)
    Private m_oAccounts As bACTAccount.Accounts

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database


    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' RDC 12112003 for removal of global variables
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    'Private m_sCallingAppName As String
    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iCompanyID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    ' NavigatorV3 variables
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    ' RAW 12/16/2002 : PS187 : replaces local definitions within procedures
    Private Const m_kiTableAccountType As Integer = 0
    Private Const m_kiTablePurgeFrequency As Integer = 1
    Private Const m_kiTableAddressCountry As Integer = 2
    Private Const m_kiTablePaymentType As Integer = 3
    Private Const m_kiTableBankCountry As Integer = 4
    Private Const m_kiTableAccountStatus As Integer = 5
    Private Const m_kiTableProofListReport As Integer = 6 ' RAW 17/12/2002 : PS187 : added
    Private Const m_kiTableBordereauReport As Integer = 7 ' RAW 17/12/2002 : PS187 : added
    Private Const m_kiTableArrayUpperBound As Integer = 7 ' RAW 17/12/2002 : PS187 : added
    ' RAW 12/16/2002 : PS187 : end

    ' RDC 12112003 moved from MainModule
    Private m_iBaseCurrencyId As Integer
    'Private m_iCompanyID As Integer
    Private m_iPartySourceId As Integer

    ' KG 03/07/03
    Private Const ACInstalmentDebt As Integer = 0

    Private Const ACIncludeTaxOnYTDTurnover As Integer = 5007
    Private Const ACIncludeTaxOnYTDIncome As Integer = 5008

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property


    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oAccounts.Count()
                    m_lCurrentRecord = m_oAccounts.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            If m_oAccounts.Count > 0 AndAlso m_oAccounts.Item(0) Is Nothing Then
                Return m_oAccounts.Count - 1
            Else
                Return m_oAccounts.Count
            End If

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

    Public Function GetAccountSecurity(ByVal v_lAccountId As Integer, ByRef r_bHasUnrestrictedEnquiry As Boolean, ByRef r_bHasUnrestrictedUpdate As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetAccountSecurity
        ' PURPOSE: Returns the Enhanced Security settings for an Account
        ' AUTHOR: Danny Davis
        ' DATE: 15/07/2002, 09:55
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("Account_id", CStr(v_lAccountId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("PMUser_id", CStr(m_iUserID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                'developer guide no. 85(Guide)
                .Parameters.Add("Has_Unrestricted_Enquiry", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
                'developer guide no. 85(Guide)
                .Parameters.Add("Has_Unrestricted_Update", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
                m_lReturn = .SQLAction(ACGetAccountSecuritySQL, ACGetAccountSecurityName, ACGetAccountSecurityStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    'Developer Guide No 98
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountSecurity", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return m_lReturn
                End If

                'Get the results
                r_bHasUnrestrictedEnquiry = (.Parameters.Item("Has_Unrestricted_Enquiry").Value = 1)
                r_bHasUnrestrictedUpdate = (.Parameters.Item("Has_Unrestricted_Update").Value = 1)
            End With



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    'Developer Guide No 98
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountSecurity", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetAccountStatus
    '
    ' Description: Gets the account status for the passed account
    '
    ' ***************************************************************** '

    Public Function GetAccountStatus(ByVal v_lAccountId As Integer, ByRef r_iAccountStatus As Integer) As Integer
        Return GetAccountStatus(v_lAccountId:=v_lAccountId, r_iAccountStatus:=r_iAccountStatus, r_bIsStopped:=False, r_sAccountCode:="")
    End Function
    Public Function GetAccountStatus(ByVal v_lAccountId As Integer, ByRef r_iAccountStatus As Integer, ByRef r_bIsStopped As Boolean) As Integer
        Return GetAccountStatus(v_lAccountId:=v_lAccountId, r_iAccountStatus:=r_iAccountStatus, r_bIsStopped:=r_bIsStopped, r_sAccountCode:="")
    End Function
    Public Function GetAccountStatus(ByVal v_lAccountId As Integer, ByRef r_iAccountStatus As Integer, ByRef r_sAccountCode As String) As Integer
        Return GetAccountStatus(v_lAccountId:=v_lAccountId, r_iAccountStatus:=r_iAccountStatus, r_bIsStopped:=False, r_sAccountCode:=r_sAccountCode)
    End Function
    Public Function GetAccountStatus(ByVal v_lAccountId As Integer, ByRef r_iAccountStatus As Integer, ByRef r_bIsStopped As Boolean, ByRef r_sAccountCode As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT accountstatus_id " &
                   "FROM account " &
                   "WHERE account_id = {account_id}"

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add account_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountStatus", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                ' Get the accountstatus from the returned results

                r_iAccountStatus = CInt(vResultArray(0, 0))

                ' Does the user want to know if it's stopped ?
                If Not False Then
                    r_bIsStopped = (r_iAccountStatus = gACTLibrary.ACTAccountStatusStopped)
                End If

                ' Does the user want the status code too?
                If Not False Then

                    m_lReturn = m_oLookup.GetCodeFromID(v_sTableName:=gACTLibrary.ACTLookupAccountStatus, v_lID:=r_iAccountStatus, r_sCode:=r_sAccountCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            Else

                ' Something's gone wrong
                r_iAccountStatus = -1

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                'Developer Guide No 98
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountStatus query returned no results.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountStatus", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)

                Return result

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountStatus", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetAccountLedger
    '
    ' Description: Gets the ledger details for the passed account
    '
    ' EK 130300
    '
    ' ***************************************************************** '
    Public Function GetAccountLedger(ByVal v_lAccountId As Integer, ByRef v_lLedgerId As Integer, ByRef v_sLedgerCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                ' Clear the parameters
                .Parameters.Clear()

                ' Add account_id
                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="ledger_id", vValue:=CStr(v_lLedgerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="ledger_code", vValue:=v_sLedgerCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Perform the SQL
                m_lReturn = .SQLAction(sSQL:=ACGetAccountLedgerSQL, sSQLName:=ACGetAccountLedgerName, bStoredProcedure:=ACGetAccountLedgerStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                v_lLedgerId = gPMFunctions.ToSafeLong(.Parameters.Item("Ledger_id").Value, 0)
                v_sLedgerCode = gPMFunctions.ToSafeString(.Parameters.Item("ledger_code").Value).Trim()
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountLedger Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountLedger", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetAccountDetails
    '
    ' Description:
    '
    ' History: 24/08/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetAccountDetails(ByRef r_lAccountID As Integer, ByRef sAccountName As String, ByRef sContactName As String, ByRef sPhoneAreaCode As String, ByRef sPhoneNumber As String, ByRef sPhoneExtension As String, ByRef r_vdAccountBalance As Object, ByRef r_sAccountCode As String) As Integer

        Dim result As Integer = 0
        Dim iAccountStatusID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' GetDetails
            m_lReturn = CType(GetDetails(vAccountID:=r_lAccountID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_lAccountID = 0
                Return result
            End If

            If r_lAccountID = 0 Then
                Return result
            End If

            ' GetNext
            m_lReturn = CType(GetNext(vAccountName:=sAccountName, vContactName:=sContactName, vPhoneAreaCode:=sPhoneAreaCode, vPhoneNumber:=sPhoneNumber, vPhoneExtension:=sPhoneExtension, vAccountStatusID:=iAccountStatusID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' GetAccountBalance

            m_lReturn = CType(GetAccountBalance(r_vdAccountBalance:=r_vdAccountBalance, v_vAccountID:=r_lAccountID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Calculate the Account Status
            m_lReturn = m_oLookup.GetCodeFromID(v_sTableName:=gACTLibrary.ACTLookupAccountStatus, v_lID:=iAccountStatusID, r_sCode:=r_sAccountCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountDetails", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Navigator SetKeys function.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description: Navigator GetKeys function.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = ""

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description: GetSummary Navigator function.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Navigator Start function. Entry point into Navigator.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetBaseCurrency
    '
    ' Description:
    '
    ' History: 21/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetBaseCurrency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the currency_id for GBP
        sSQL = "SELECT currency_id FROM Currency WHERE iso_code = 'GBP'"

        ' Perform the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBaseCurrency", bStoredProcedure:=False, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' CF - I don't know why _i is declared as Long !!!
        If Informations.IsArray(vResultArray) Then

            m_iBaseCurrencyId = CInt(vResultArray(0, 0))
        Else
            m_iBaseCurrencyId = 26
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCompanyID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' CF030399 - Changed to use component services


            m_lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create Accounts Collection
            m_oAccounts = New bACTAccount.Accounts()

            ' Create PMLookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'JSB 28/10/98
            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion




            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                m_oAccounts = Nothing

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
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetMandatory (Public)
    '
    ' Description: Sets and returns the Mandy fields required.
    '
    ' ***************************************************************** '
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' RDC 12112003 added vAllowElectronicPayments
    Public Function GetMandatory(Optional ByRef lAccountIDMandy As Integer = 0, Optional ByRef lPurgefrequencyIDMandy As Integer = 0, Optional ByRef lCurrencyIDMandy As Integer = 0, Optional ByRef lAccounttypeIDMandy As Integer = 0, Optional ByRef lLedgerIDMandy As Integer = 0, Optional ByRef lPaymenttypeIDMandy As Integer = 0, Optional ByRef lAccountNameMandy As Integer = 0, Optional ByRef lShortCodeMandy As Integer = 0, Optional ByRef lRestrictEnquiryMandy As Integer = 0, Optional ByRef lRestrictUpdateMandy As Integer = 0, Optional ByRef lDeleteAtPurgeMandy As Integer = 0, Optional ByRef lContactNameMandy As Integer = 0, Optional ByRef lAddress1Mandy As Integer = 0, Optional ByRef lAddress2Mandy As Integer = 0, Optional ByRef lAddress3Mandy As Integer = 0, Optional ByRef lAddress4Mandy As Integer = 0, Optional ByRef lPostalCodeMandy As Integer = 0, Optional ByRef lAddressCountryMandy As Integer = 0, Optional ByRef lPhoneAreaCodeMandy As Integer = 0, Optional ByRef lPhoneNumberMandy As Integer = 0, Optional ByRef lPhoneExtensionMandy As Integer = 0, Optional ByRef lFaxAreaCodeMandy As Integer = 0, Optional ByRef lFaxNumberMandy As Integer = 0, Optional ByRef lFaxExtensionMandy As Integer = 0, Optional ByRef lPaymentNameMandy As Integer = 0, Optional ByRef lPaymentAccountCodeMandy As Integer = 0, Optional ByRef lPaymentBranchCodeMandy As Integer = 0, Optional ByRef lPaymentExpiryDateMandy As Integer = 0, Optional ByRef lPaymentReference1Mandy As Integer = 0, Optional ByRef lPaymentReference2Mandy As Integer = 0, Optional ByRef lCreditLimitMandy As Integer = 0, Optional ByRef lDiscountPercentageMandy As Integer = 0, Optional ByRef lSettlementPeriodMandy As Integer = 0, Optional ByRef lBankNameMandy As Integer = 0, Optional ByRef lBankAddress1Mandy As Integer = 0, Optional ByRef lBankAddress2Mandy As Integer = 0, Optional ByRef lBankAddress3Mandy As Integer = 0, Optional ByRef lBankAddress4Mandy As Integer = 0, Optional ByRef lBankPostalCodeMandy As Integer = 0, Optional ByRef lBankCountryMandy As Integer = 0, Optional ByRef lBankPhoneAreaCodeMandy As Integer = 0, Optional ByRef lBankPhoneNumberMandy As Integer = 0, Optional ByRef lBankPhoneExtensionMandy As Integer = 0, Optional ByRef lBankFaxAreaCodeMandy As Integer = 0, Optional ByRef lBankFaxNumberMandy As Integer = 0, Optional ByRef lBankFaxExtensionMandy As Integer = 0, Optional ByRef lCommentsMandy As Integer = 0, Optional ByRef lAccountKeyMandy As gPMConstants.PMEMandatoryStatus = 0, Optional ByRef lNominalAccountID As Integer = 0, Optional ByRef lAccountStatusIDMandy As Integer = 0, Optional ByRef lProofListReportIDMandy As Integer = 0, Optional ByRef lBordereauReportIDMandy As Integer = 0, Optional ByRef lAllowElectronicPayment As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Mandy fields.

            lAccountIDMandy = gPMConstants.PMEMandatoryStatus.PMMandatory
            lPurgefrequencyIDMandy = gPMConstants.PMEMandatoryStatus.PMMandatory
            lCurrencyIDMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAccounttypeIDMandy = gPMConstants.PMEMandatoryStatus.PMMandatory
            lLedgerIDMandy = gPMConstants.PMEMandatoryStatus.PMMandatory
            lPaymenttypeIDMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAccountNameMandy = gPMConstants.PMEMandatoryStatus.PMMandatory
            lShortCodeMandy = gPMConstants.PMEMandatoryStatus.PMMandatory
            lRestrictEnquiryMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lRestrictUpdateMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lDeleteAtPurgeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lContactNameMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAddress1Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAddress2Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAddress3Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAddress4Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPostalCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAddressCountryMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPhoneAreaCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPhoneNumberMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPhoneExtensionMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lFaxAreaCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lFaxNumberMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lFaxExtensionMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentNameMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentAccountCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentBranchCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentExpiryDateMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentReference1Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentReference2Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lProofListReportIDMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory ' RAW 17/12/2002 : PS187 : Added
            lBordereauReportIDMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory ' RAW 17/12/2002 : PS187 : Added
            lCreditLimitMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lDiscountPercentageMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lSettlementPeriodMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankNameMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankAddress1Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankAddress2Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankAddress3Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankAddress4Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankPostalCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankCountryMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankPhoneAreaCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankPhoneNumberMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankPhoneExtensionMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankFaxAreaCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankFaxNumberMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBankFaxExtensionMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCommentsMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAccountKeyMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lNominalAccountID = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAccountStatusIDMandy = gPMConstants.PMEMandatoryStatus.PMMandatory
            lAllowElectronicPayment = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a Account.
    '
    ' ***************************************************************** '
    'developer guide no. 71(Guide)
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oaccount As bACTAccount.Account = Nothing
        Dim dtEffectiveDate As Date

        ' RAW 17/12/2002 : PS187 : Table index constants moved to module level constants
        'Const CAccountType = 0
        'Const CPurgeFrequency = 1
        'Const CAddressCountry = 2
        'Const CPaymentType = 3
        'Const CBankCountry = 4
        'Const CAccountStatus = 5

        ' {* USER DEFINED CODE (Begin) *}
        ' RAW 17/12/2002 : PS187 : increased upper bound
        Dim vTabArray As Array = Array.CreateInstance(GetType(Object), New Integer() {4, m_kiTableArrayUpperBound - m_kiTableAccountType + 1}, New Integer() {0, m_kiTableAccountType})
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, m_kiTableAccountType) = gACTLibrary.ACTLookupAccountType ' RAW 17/12/2002 : PS187 : constant renamed

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, m_kiTablePurgeFrequency) = gACTLibrary.ACTLookupPurgeFrequency ' RAW 17/12/2002 : PS187 : constant renamed

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, m_kiTableAddressCountry) = gPMConstants.PMLookupCountry ' RAW 17/12/2002 : PS187 : constant renamed

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, m_kiTablePaymentType) = gACTLibrary.ACTLookupPaymentType ' RAW 17/12/2002 : PS187 : constant renamed

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, m_kiTableBankCountry) = gPMConstants.PMLookupCountry ' RAW 17/12/2002 : PS187 : constant renamed

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, m_kiTableAccountStatus) = gACTLibrary.ACTLookupAccountStatus ' RAW 17/12/2002 : PS187 : constant renamed

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, m_kiTableProofListReport) = gACTLibrary.ACTLookupReport ' RAW 17/12/2002 : PS187 : Added

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, m_kiTableBordereauReport) = gACTLibrary.ACTLookupReport ' RAW 17/12/2002 : PS187 : Added

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oaccount = m_oAccounts.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableAccountType) = "" ' RAW 17/12/2002 : PS187 : constant renamed

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTablePurgeFrequency) = "" ' RAW 17/12/2002 : PS187 : constant renamed

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableAddressCountry) = "" ' RAW 17/12/2002 : PS187 : constant renamed

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTablePaymentType) = "" ' RAW 17/12/2002 : PS187 : constant renamed

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableBankCountry) = "" ' RAW 17/12/2002 : PS187 : constant renamed

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableAccountStatus) = "" ' RAW 17/12/2002 : PS187 : constant renamed

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableProofListReport) = "" ' RAW 17/12/2002 : PS187 : added

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableBordereauReport) = "" ' RAW 17/12/2002 : PS187 : added

                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oaccount

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableAccountType) = .AccounttypeID ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTablePurgeFrequency) = .PurgefrequencyID ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableAddressCountry) = .AddressCountry ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTablePaymentType) = .PaymenttypeID ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableBankCountry) = .BankCountry ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableAccountStatus) = .AccountStatusID ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableProofListReport) = .ProofListReportID ' RAW 17/12/2002 : PS187 : added

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableBordereauReport) = .BordereauReportID ' RAW 17/12/2002 : PS187 : added
                        dtEffectiveDate = DateTime.Now
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oaccount

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableAccountType) = .AccounttypeID ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTablePurgeFrequency) = .PurgefrequencyID ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableAddressCountry) = .AddressCountry ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTablePaymentType) = .PaymenttypeID ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableBankCountry) = .BankCountry ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableAccountStatus) = .AccountStatusID ' RAW 17/12/2002 : PS187 : constant renamed

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableProofListReport) = .ProofListReportID ' RAW 17/12/2002 : PS187 : added

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, m_kiTableBordereauReport) = .BordereauReportID ' RAW 17/12/2002 : PS187 : added
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select

            ' Release Account reference
            oaccount = Nothing

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CheckShortCodeUnique
    '
    ' Description: Checks if the short code passed exists on the database
    ' PMNotFound = Unique i.e. doesnt exist
    ' SP167000 - this isnt actually doing anything as the wrong variable
    '            is used.
    ' ***************************************************************** '
    Private Function CheckShortCodeUnique(ByVal v_sShortCode As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String
        Dim vValue As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' CTAF 260400
        m_lReturn = CType(bPMFunc.ValidateSQL(sSQLStatement:=v_sShortCode), gPMConstants.PMEReturnCode)

        ' construct the sql according to multibranch core accounts setting
        ' RDC 13112003
        'MKW 180604 PN12470 Use Multitree Accounting switch and company one options.
        m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iCompanyID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)
        If vValue = "1" Then
            sSQL = "SELECT short_code FROM account WHERE short_code = '" &
                       v_sShortCode.Trim() & "' AND company_id = " & CStr(m_iCompanyID)
        Else

            sSQL = "SELECT short_code FROM account WHERE short_code = '" &
                       v_sShortCode.Trim() & "'"
        End If

        ' query the database
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckShortCodeUnique", bStoredProcedure:=False)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check if it returned any results
        If m_oDatabase.Records.Count() > 0 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    'eck110400
    'Multi Branch
    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Account directly into the database.
    '        Note: The Account will NOT be added to the collection.
    '
    ' ***************************************************************** '
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' RDC 12112003 added vAllowElectronicPayment
    Public Function DirectAdd(Optional ByRef vAccountID As Integer = 0, Optional ByRef vPurgefrequencyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vAccounttypeID As Object = Nothing, Optional ByRef vLedgerId As Object = Nothing, Optional ByRef vPaymenttypeID As Object = Nothing, Optional ByRef vAccountName As Object = Nothing, Optional ByRef vShortCode As Object = Nothing, Optional ByRef vRestrictEnquiry As Object = Nothing, Optional ByRef vRestrictUpdate As Object = Nothing, Optional ByRef vDeleteAtPurge As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vAddressCountry As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vPaymentName As Object = Nothing, Optional ByRef vPaymentAccountCode As Object = Nothing, Optional ByRef vPaymentBranchCode As Object = Nothing, Optional ByRef vPaymentExpiryDate As Object = Nothing, Optional ByRef vPaymentReference1 As Object = Nothing, Optional ByRef vPaymentReference2 As Object = Nothing, Optional ByRef vCreditLimit As Object = Nothing, Optional ByRef vDiscountPercentage As Object = Nothing, Optional ByRef vSettlementPeriod As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vNominalAccountID As Object = Nothing, Optional ByRef vAccountStatusID As Object = Nothing, Optional ByRef vPartySourceID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vProofListReportID As Object = Nothing, Optional ByRef vBordereauReportID As Object = Nothing, Optional ByRef vAllowElectronicPayment As Object = Nothing, Optional ByRef vMoneyCalcAccType As Object = Nothing, Optional ByRef vClientBankAccType As Object = Nothing, Optional ByRef vParamArray() As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oaccount As Account
        Dim vArray() As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ReDim vArray(ACHLastArrayPosition)

            ' Create a new Account
            oaccount = New bACTAccount.Account()



            vArray(ACClientBankAccTypeArrPos) = vClientBankAccType

            If Informations.IsArray(vParamArray) Then
                vArray(ACMerchantIdArrPos) = vParamArray(0)
            End If

            ' Populate Account Attributes
            'eck161100 Pass the PartySourceID
            ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID























































            'm_lReturn = CType(SetProperties(oaccount, gPMConstants.PMEComponentAction.PMAdd, vAccountID:=vAccountID, vPurgefrequencyID:=CInt(vPurgefrequencyID), vCurrencyID:=CInt(vCurrencyID), vAccounttypeID:=CInt(vAccounttypeID), vLedgerId:=CInt(vLedgerId), vPaymenttypeID:=CInt(vPaymenttypeID), vAccountName:=CStr(vAccountName), vShortCode:=CStr(vShortCode), vRestrictEnquiry:=CInt(vRestrictEnquiry), vRestrictUpdate:=CInt(vRestrictUpdate), vDeleteAtPurge:=CInt(vDeleteAtPurge), vContactName:=CStr(vContactName), vAddress1:=CStr(vAddress1), vAddress2:=CStr(vAddress2), vAddress3:=CStr(vAddress3), vAddress4:=CStr(vAddress4), vPostalCode:=CStr(vPostalCode), vAddressCountry:=CInt(vAddressCountry), vPhoneAreaCode:=CStr(vPhoneAreaCode), vPhoneNumber:=CStr(vPhoneNumber), vPhoneExtension:=CStr(vPhoneExtension), vFaxAreaCode:=CStr(vFaxAreaCode), vFaxNumber:=CStr(vFaxNumber), vFaxExtension:=CStr(vFaxExtension), vPaymentName:=CStr(vPaymentName), vPaymentAccountCode:=CStr(vPaymentAccountCode), vPaymentBranchCode:=CStr(vPaymentBranchCode), vPaymentExpiryDate:=vPaymentExpiryDate, vPaymentReference1:=CStr(vPaymentReference1), vPaymentReference2:=CStr(vPaymentReference2), vCreditLimit:=CDbl(vCreditLimit), vDiscountPercentage:=CDbl(vDiscountPercentage), vSettlementPeriod:=CInt(vSettlementPeriod), vBankName:=CStr(vBankName), vBankAddress1:=CStr(vBankAddress1), vBankAddress2:=CStr(vBankAddress2), vBankAddress3:=CStr(vBankAddress3), vBankAddress4:=CStr(vBankAddress4), vBankPostalCode:=CStr(vBankPostalCode), vBankCountry:=CInt(vBankCountry), vBankPhoneAreaCode:=CStr(vBankPhoneAreaCode), vBankPhoneNumber:=CStr(vBankPhoneNumber), vBankPhoneExtension:=CStr(vBankPhoneExtension), vBankFaxAreaCode:=CStr(vBankFaxAreaCode), vBankFaxNumber:=CStr(vBankFaxNumber), vBankFaxExtension:=CStr(vBankFaxExtension), vComments:=CStr(vComments), vAccountKey:=CInt(vAccountKey), vNominalAccountID:=CInt(vNominalAccountID), vAccountStatusID:=CInt(vAccountStatusID), vPartySourceID:=CStr(vPartySourceID), vSubBranchID:=CInt(vSubBranchID), vProofListReportID:=CInt(vProofListReportID), vBordereauReportID:=CInt(vBordereauReportID), vAllowElectronicPayment:=CBool(vAllowElectronicPayment), vMoneyCalcAccType:=CInt(vMoneyCalcAccType), vParamArray:=vArray), gPMConstants.PMEReturnCode)
            m_lReturn = CType(SetProperties(oaccount, gPMConstants.PMEComponentAction.PMAdd, vAccountID:=vAccountID, vPurgefrequencyID:=vPurgefrequencyID, vCurrencyID:=vCurrencyID, vAccounttypeID:=vAccounttypeID, vLedgerId:=vLedgerId, vPaymenttypeID:=vPaymenttypeID, vAccountName:=vAccountName, vShortCode:=vShortCode, vRestrictEnquiry:=vRestrictEnquiry, vRestrictUpdate:=vRestrictUpdate, vDeleteAtPurge:=vDeleteAtPurge, vContactName:=vContactName, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vAddressCountry:=vAddressCountry, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vPaymentName:=vPaymentName, vPaymentAccountCode:=vPaymentAccountCode, vPaymentBranchCode:=vPaymentBranchCode, vPaymentExpiryDate:=vPaymentExpiryDate, vPaymentReference1:=vPaymentReference1, vPaymentReference2:=vPaymentReference2, vCreditLimit:=vCreditLimit, vDiscountPercentage:=vDiscountPercentage, vSettlementPeriod:=vSettlementPeriod, vBankName:=vBankName, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vAccountKey:=vAccountKey, vNominalAccountID:=vNominalAccountID, vAccountStatusID:=vAccountStatusID, vPartySourceID:=vPartySourceID, vSubBranchID:=vSubBranchID, vProofListReportID:=vProofListReportID, vBordereauReportID:=vBordereauReportID, vAllowElectronicPayment:=vAllowElectronicPayment, vMoneyCalcAccType:=vMoneyCalcAccType, vParamArray:=vArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CF140199
            ' Check that the short code doesnt exist already
            m_lReturn = CType(CheckShortCodeUnique(oaccount.ShortCode), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Return RecordInUse
                Return gPMConstants.PMEReturnCode.PMRecordInUse
            End If
            'eck110400
            'Multi Branch

            If Not Informations.IsNothing(vPartySourceID) Then

                If Not (Convert.IsDBNull(vPartySourceID) Or Informations.IsNothing(vPartySourceID)) Then

                    m_iPartySourceId = CInt(vPartySourceID)
                End If
            End If
            ' Add the Account to the Database
            m_lReturn = CType(AddItem(oaccount), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Account Added

            If Not Informations.IsNothing(vAccountID) Then
                vAccountID = oaccount.AccountID
            End If

            ' {* USER DEFINED CODE (End) *}

            oaccount = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Account directly from the database.
    '        Note: The Account will NOT be deleted from the collection.
    '
    ' ***************************************************************** '

    Public Function DirectDelete(ByRef vID As Object) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ''PN-62336 Date-15/01/2010
            If GetIsLedgerExist(vID) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the AccountID INPUT parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't deleted, error
            If lRecordsAffected > 0 Then
                ' Deleted, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the Account.
    '
    ' ***************************************************************** '
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' RDC 12112003 added vAllowElectronicPayment
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPurgefrequencyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vAccounttypeID As Object = Nothing, Optional ByRef vLedgerId As Object = Nothing, Optional ByRef vPaymenttypeID As Object = Nothing, Optional ByRef vAccountName As Object = Nothing, Optional ByRef vShortCode As Object = Nothing, Optional ByRef vRestrictEnquiry As Object = Nothing, Optional ByRef vRestrictUpdate As Object = Nothing, Optional ByRef vDeleteAtPurge As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vAddressCountry As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vPaymentName As Object = Nothing, Optional ByRef vPaymentAccountCode As Object = Nothing, Optional ByRef vPaymentBranchCode As Object = Nothing, Optional ByRef vPaymentExpiryDate As Object = Nothing, Optional ByRef vPaymentReference1 As Object = Nothing, Optional ByRef vPaymentReference2 As Object = Nothing, Optional ByRef vCreditLimit As Object = Nothing, Optional ByRef vDiscountPercentage As Object = Nothing, Optional ByRef vSettlementPeriod As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vNominalAccountID As Object = Nothing, Optional ByRef vAccountStatusID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vProofListReportID As Object = Nothing, Optional ByRef vBordereauReportID As Object = Nothing, Optional ByRef vAllowElectronicPayment As Object = Nothing, Optional ByRef vMoneyCalcAccType As Object = Nothing, Optional ByRef vClientBankAccType As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
            ' RDC 12112003

            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vAccountID:=vAccountID, vPurgefrequencyID:=vPurgefrequencyID, vCurrencyID:=vCurrencyID, vAccounttypeID:=vAccounttypeID, vLedgerId:=vLedgerId, vPaymenttypeID:=vPaymenttypeID, vAccountName:=vAccountName, vShortCode:=vShortCode, vRestrictEnquiry:=vRestrictEnquiry, vRestrictUpdate:=vRestrictUpdate, vDeleteAtPurge:=vDeleteAtPurge, vContactName:=vContactName, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vAddressCountry:=vAddressCountry, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vPaymentName:=vPaymentName, vPaymentAccountCode:=vPaymentAccountCode, vPaymentBranchCode:=vPaymentBranchCode, vPaymentExpiryDate:=vPaymentExpiryDate, vPaymentReference1:=vPaymentReference1, vPaymentReference2:=vPaymentReference2, vCreditLimit:=vCreditLimit, vDiscountPercentage:=vDiscountPercentage, vSettlementPeriod:=vSettlementPeriod, vBankName:=vBankName, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vAccountKey:=vAccountKey, vNominalAccountID:=vNominalAccountID, vAccountStatusID:=vAccountStatusID, vSubBranchID:=vSubBranchID, vProofListReportID:=vProofListReportID, vBordereauReportID:=vBordereauReportID, vAllowElectronicPayment:=vAllowElectronicPayment, vMoneyCalcAccType:=vMoneyCalcAccType, vClientBankAccType:=vClientBankAccType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLedgerDetails (Public)
    '
    ' Description: Return all ledgers for this company in an array
    '
    ' ***************************************************************** '
    Public Function GetLedgerDetails(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'developer guide no. 112(Guide)
        Dim oFields As DataRow

        Const CLedgerID As Integer = 0
        Const CLedgerName As Integer = 1
        Const CLedgerShortName As Integer = 2
        Const CLedgerType As Integer = 3
        Const CMappingID As Integer = 4
        Const CIsDeletable As Integer = 5

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the ComapnyID parameter (INPUT)
            ' Query is all ledgers for company

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLedgerDetailsSQL, sSQLName:=ACGetLedgerDetailsName, bStoredProcedure:=ACGetLedgerDetailsStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            Else

                Dim vResult As Array = Array.CreateInstance(GetType(Object), New Integer() {lRecordCount + 1, CIsDeletable - CLedgerID + 1}, New Integer() {0, CLedgerID})
                ' Yes, load them into the return array

                'Developer Guide No 111
                For lSub As Integer = 0 To lRecordCount - 1

                    oFields = m_oDatabase.Records.Item(lSub).Fields()


                    If Convert.IsDBNull(oFields("ledger_id")) Or Informations.IsNothing(oFields("ledger_id")) Then

                        vResult(lSub, CLedgerID) = 0
                    Else

                        vResult(lSub, CLedgerID) = oFields("ledger_id")
                    End If


                    If Convert.IsDBNull(oFields("ledger_name")) Or Informations.IsNothing(oFields("ledger_name")) Then

                        vResult(lSub, CLedgerName) = ""
                    Else

                        vResult(lSub, CLedgerName) = oFields("ledger_name").trim()
                    End If


                    If Convert.IsDBNull(oFields("ledger_short_name")) Or Informations.IsNothing(oFields("ledger_short_name")) Then

                        vResult(lSub, CLedgerShortName) = ""
                    Else

                        vResult(lSub, CLedgerShortName) = oFields("ledger_short_name").trim()
                    End If


                    If Convert.IsDBNull(oFields("ledgertype_id")) Or Informations.IsNothing(oFields("ledgertype_id")) Then

                        vResult(lSub, CLedgerType) = 0
                    Else

                        vResult(lSub, CLedgerType) = oFields("ledgertype_id")
                    End If


                    If Convert.IsDBNull(oFields("mapping_id")) Or Informations.IsNothing(oFields("mapping_id")) Then

                        vResult(lSub, CMappingID) = 0
                    Else

                        vResult(lSub, CMappingID) = oFields("mapping_id")
                    End If


                    If Convert.IsDBNull(oFields("is_deletable")) Or Informations.IsNothing(oFields("is_deletable")) Then

                        vResult(lSub, CIsDeletable) = False
                    Else

                        vResult(lSub, CIsDeletable) = oFields("is_deletable")
                    End If

                Next lSub
                vResultArray = vResult
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLedgerDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerDetails", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetCaptions (Public)
    '
    ' Description: Get the requested caption fields for a record.
    '
    ' ***************************************************************** '
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer
        Return GetCaptions(vID:=vID, vFieldArray:=vFieldArray, vResultArray:=vResultArray, vTable:=Nothing)
    End Function
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer

        Dim result As Integer = 0
        Dim oFields As ADODB.Fields

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vFieldArray) Then
                ' Log Error Message
                'Developer Guide No 98
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Table name
            'BB If (IsMissing(vTable) = False) Then

            ' Is this our table
            '    If (Trim$(vTable) <> PMTableAccount) Then

            '        GetCaptions = PMInvalidRequest


            '       Exit Function

            '    End If

            'End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CType(CheckID(vID:=vID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Resize the Temporary Results Array
            Dim vResults(vFieldArray.GetUpperBound(0)) As Object

            ' Get a reference to the Fields returned
            'Developer Guide no. 111
            oFields = m_oDatabase.Records.Item(0).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'AK 230702 - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            'developer guide no. 47(No Solutions)
                            Case DbType.String, DbType.String, DbType.String, DbType.String, DbType.String

                                vResults(lSub) = ""
                                'developer guide no. 47(No Solutions)
                                'Case DbType.Date, adDBDate 'Case DbType.Date, adDBDate 
                            Case DbType.Date

                                vResults(lSub) = -1
                            Case Else

                                vResults(lSub) = 0
                        End Select
                    End If

                Next lSub

            End With

            ' Assign the results
            vResultArray = vResults

            ' Release the reference to the Fields
            oFields = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Accounts and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vAccountID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oaccount As bACTAccount.Account

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oAccounts.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vAccountID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vAccountID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    'Developer Guide No 98
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vAccountID =" & CStr(vAccountID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                    Return result

                End If

                ' Add the AccountID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Account_id", vValue:=CStr(vAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound

            Else

                ' Yes, load them into the collection

                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New Account
                    oaccount = New bACTAccount.Account()

                    m_lReturn = CType(SetPropertiesFromDB(oaccount:=oaccount, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Account to collection
                    If (m_oAccounts.Count = 0) Then
                        m_oAccounts.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oAccounts.Add(oNewAccount:=oaccount), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oaccount = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    '''  Gets the required Accounts and populate the Collection
    ''' </summary>
    ''' <param name="vAccountID"></param>
    ''' <param name="vPurgefrequencyID"></param>
    ''' <param name="vCurrencyID"></param>
    ''' <param name="vAccounttypeID"></param>
    ''' <param name="vLedgerId"></param>
    ''' <param name="vPaymenttypeID"></param>
    ''' <param name="vAccountName"></param>
    ''' <param name="vShortCode"></param>
    ''' <param name="vRestrictEnquiry"></param>
    ''' <param name="vRestrictUpdate"></param>
    ''' <param name="vDeleteAtPurge"></param>
    ''' <param name="vContactName"></param>
    ''' <param name="vAddress1"></param>
    ''' <param name="vAddress2"></param>
    ''' <param name="vAddress3"></param>
    ''' <param name="vAddress4"></param>
    ''' <param name="vPostalCode"></param>
    ''' <param name="vAddressCountry"></param>
    ''' <param name="vPhoneAreaCode"></param>
    ''' <param name="vPhoneNumber"></param>
    ''' <param name="vPhoneExtension"></param>
    ''' <param name="vFaxAreaCode"></param>
    ''' <param name="vFaxNumber"></param>
    ''' <param name="vFaxExtension"></param>
    ''' <param name="vPaymentName"></param>
    ''' <param name="vPaymentAccountCode"></param>
    ''' <param name="vPaymentBranchCode"></param>
    ''' <param name="vPaymentExpiryDate"></param>
    ''' <param name="vPaymentReference1"></param>
    ''' <param name="vPaymentReference2"></param>
    ''' <param name="vCreditLimit"></param>
    ''' <param name="vDiscountPercentage"></param>
    ''' <param name="vSettlementPeriod"></param>
    ''' <param name="vBankName"></param>
    ''' <param name="vBankAddress1"></param>
    ''' <param name="vBankAddress2"></param>
    ''' <param name="vBankAddress3"></param>
    ''' <param name="vBankAddress4"></param>
    ''' <param name="vBankPostalCode"></param>
    ''' <param name="vBankCountry"></param>
    ''' <param name="vBankPhoneAreaCode"></param>
    ''' <param name="vBankPhoneNumber"></param>
    ''' <param name="vBankPhoneExtension"></param>
    ''' <param name="vBankFaxAreaCode"></param>
    ''' <param name="vBankFaxNumber"></param>
    ''' <param name="vBankFaxExtension"></param>
    ''' <param name="vComments"></param>
    ''' <param name="vAccountKey"></param>
    ''' <param name="vNominalAccountID"></param>
    ''' <param name="vAccountStatusID"></param>
    ''' <param name="vPartySourceID"></param>
    ''' <param name="vSubBranchID"></param>
    ''' <param name="vProofListReportID"></param>
    ''' <param name="vBordereauReportID"></param>
    ''' <param name="vAllowElectronicPayment"></param>
    ''' <param name="vMoneyCalcAccType"></param>
    ''' <param name="vClientBankAccType"></param>
    ''' <param name="vParamArray"></param>
    ''' <param name="vCompanyId"></param>
    ''' <param name="r_sBIC"></param>
    ''' <param name="r_sIBAN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNext(Optional ByRef vAccountID As Object = Nothing,
                            Optional ByRef vPurgefrequencyID As Object = Nothing,
                            Optional ByRef vCurrencyID As Object = Nothing,
                            Optional ByRef vAccounttypeID As Object = Nothing,
                            Optional ByRef vLedgerId As Object = Nothing,
                            Optional ByRef vPaymenttypeID As Object = Nothing,
                            Optional ByRef vAccountName As Object = Nothing,
                            Optional ByRef vShortCode As Object = Nothing,
                            Optional ByRef vRestrictEnquiry As Object = Nothing,
                            Optional ByRef vRestrictUpdate As Object = Nothing,
                            Optional ByRef vDeleteAtPurge As Object = Nothing,
                            Optional ByRef vContactName As Object = Nothing,
                            Optional ByRef vAddress1 As Object = Nothing,
                            Optional ByRef vAddress2 As Object = Nothing,
                            Optional ByRef vAddress3 As Object = Nothing,
                            Optional ByRef vAddress4 As Object = Nothing,
                            Optional ByRef vPostalCode As Object = Nothing,
                            Optional ByRef vAddressCountry As Object = Nothing,
                            Optional ByRef vPhoneAreaCode As Object = Nothing,
                            Optional ByRef vPhoneNumber As Object = Nothing,
                            Optional ByRef vPhoneExtension As Object = Nothing,
                            Optional ByRef vFaxAreaCode As Object = Nothing,
                            Optional ByRef vFaxNumber As Object = Nothing,
                            Optional ByRef vFaxExtension As Object = Nothing,
                            Optional ByRef vPaymentName As Object = Nothing,
                            Optional ByRef vPaymentAccountCode As Object = Nothing,
                            Optional ByRef vPaymentBranchCode As Object = Nothing,
                            Optional ByRef vPaymentExpiryDate As Object = Nothing,
                            Optional ByRef vPaymentReference1 As Object = Nothing,
                            Optional ByRef vPaymentReference2 As Object = Nothing,
                            Optional ByRef vCreditLimit As Object = Nothing,
                            Optional ByRef vDiscountPercentage As Object = Nothing,
                            Optional ByRef vSettlementPeriod As Object = Nothing,
                            Optional ByRef vBankName As Object = Nothing,
                            Optional ByRef vBankAddress1 As Object = Nothing,
                            Optional ByRef vBankAddress2 As Object = Nothing,
                            Optional ByRef vBankAddress3 As Object = Nothing,
                            Optional ByRef vBankAddress4 As Object = Nothing,
                            Optional ByRef vBankPostalCode As Object = Nothing,
                            Optional ByRef vBankCountry As Object = Nothing,
                            Optional ByRef vBankPhoneAreaCode As Object = Nothing,
                            Optional ByRef vBankPhoneNumber As Object = Nothing,
                            Optional ByRef vBankPhoneExtension As Object = Nothing,
                            Optional ByRef vBankFaxAreaCode As Object = Nothing,
                            Optional ByRef vBankFaxNumber As Object = Nothing,
                            Optional ByRef vBankFaxExtension As Object = Nothing,
                            Optional ByRef vComments As Object = Nothing,
                            Optional ByRef vAccountKey As Object = Nothing,
                            Optional ByRef vNominalAccountID As Object = Nothing,
                            Optional ByRef vAccountStatusID As Object = Nothing,
                            Optional ByRef vPartySourceID As Object = Nothing,
                            Optional ByRef vSubBranchID As Object = Nothing,
                            Optional ByRef vProofListReportID As Object = Nothing,
                            Optional ByRef vBordereauReportID As Object = Nothing,
                            Optional ByRef vAllowElectronicPayment As Object = Nothing,
                            Optional ByRef vMoneyCalcAccType As Object = Nothing,
                            Optional ByRef vClientBankAccType As Object = Nothing,
                            Optional ByRef vParamArray As Object = Nothing,
                            Optional ByRef vCompanyId As Integer = 0,
                            Optional ByRef r_sBIC As String = "",
                            Optional ByRef r_sIBAN As String = "") As Integer


        Dim result As Integer = 0
        Dim oaccount As bACTAccount.Account
        Dim iStatus As Integer
        Dim vArray() As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ReDim vArray(ACHLastArrayPosition)

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oAccounts.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oaccount = m_oAccounts.Item(m_lCurrentRecord)

            ' Get the Account Property Values
            ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
            ' RDC 12112003 added vAllowElectronicPayment
            m_lReturn = GetProperties(oaccount, iStatus,
            vAccountID:=vAccountID, vPurgefrequencyID:=vPurgefrequencyID, vCurrencyID:=vCurrencyID, vAccounttypeID:=vAccounttypeID, vLedgerId:=vLedgerId, vPaymenttypeID:=vPaymenttypeID,
            vAccountName:=vAccountName, vShortCode:=vShortCode, vRestrictEnquiry:=vRestrictEnquiry, vRestrictUpdate:=vRestrictUpdate, vDeleteAtPurge:=vDeleteAtPurge, vContactName:=vContactName,
            vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vAddressCountry:=vAddressCountry,
            vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension,
            vPaymentName:=vPaymentName, vPaymentAccountCode:=vPaymentAccountCode, vPaymentBranchCode:=vPaymentBranchCode, vPaymentExpiryDate:=vPaymentExpiryDate, vPaymentReference1:=vPaymentReference1, vPaymentReference2:=vPaymentReference2,
            vCreditLimit:=vCreditLimit, vDiscountPercentage:=vDiscountPercentage, vSettlementPeriod:=vSettlementPeriod, vBankName:=vBankName, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2,
            vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber,
            vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vAccountKey:=vAccountKey,
            vNominalAccountID:=vNominalAccountID, vAccountStatusID:=vAccountStatusID, vPartySourceID:=vPartySourceID, vSubBranchID:=vSubBranchID, vProofListReportID:=vProofListReportID, vBordereauReportID:=vBordereauReportID,
            vAllowElectronicPayment:=vAllowElectronicPayment, vMoneyCalcAccType:=vMoneyCalcAccType, vParamArray:=vArray, vCompanyId:=vCompanyId, r_sBIC:=r_sBIC, r_sIBAN:=r_sIBAN)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ReDim vParamArray(0)
            If Informations.IsArray(vArray) Then
                vClientBankAccType = vArray(ACClientBankAccTypeArrPos)
                vParamArray(0) = vArray(ACMerchantIdArrPos)
            End If

            oaccount = Nothing
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Account into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    ' eck010900 Add Company as a modifyable property
    ' ***************************************************************** '
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' RDC 12112003 added vAllowElectronicPayment
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPurgefrequencyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vAccounttypeID As Object = Nothing, Optional ByRef vLedgerId As Object = Nothing, Optional ByRef vPaymenttypeID As Object = Nothing, Optional ByRef vAccountName As Object = Nothing, Optional ByRef vShortCode As Object = Nothing, Optional ByRef vRestrictEnquiry As Object = Nothing, Optional ByRef vRestrictUpdate As Object = Nothing, Optional ByRef vDeleteAtPurge As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vAddressCountry As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vPaymentName As Object = Nothing, Optional ByRef vPaymentAccountCode As Object = Nothing, Optional ByRef vPaymentBranchCode As Object = Nothing, Optional ByRef vPaymentExpiryDate As Object = Nothing, Optional ByRef vPaymentReference1 As Object = Nothing, Optional ByRef vPaymentReference2 As Object = Nothing, Optional ByRef vCreditLimit As Object = Nothing, Optional ByRef vDiscountPercentage As Object = Nothing, Optional ByRef vSettlementPeriod As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vNominalAccountID As Object = Nothing, Optional ByRef vAccountStatusID As Object = Nothing, Optional ByRef vPartySourceID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vProofListReportID As Object = Nothing, Optional ByRef vBordereauReportID As Object = Nothing, Optional ByRef vAllowElectronicPayment As Object = Nothing, Optional ByRef vMoneyCalcAccType As Object = Nothing, Optional ByRef vClientBankAccType As Object = Nothing, Optional ByRef vParamArray() As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oaccount As bACTAccount.Account
        Dim vArray() As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ReDim vArray(ACHLastArrayPosition)



            vArray(ACClientBankAccTypeArrPos) = vClientBankAccType
            If Informations.IsArray(vParamArray) Then


                vArray(ACMerchantIdArrPos) = vParamArray(0)
            End If


            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oAccounts.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Account
            oaccount = New bACTAccount.Account()

            ' Populate Account Attributes
            ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
            ' RDC 12112003 added vAllowElectronicPayment

            m_lReturn = CType(SetProperties(oaccount, gPMConstants.PMEComponentAction.PMAdd, vAccountID:=vAccountID, vPurgefrequencyID:=vPurgefrequencyID, vCurrencyID:=vCurrencyID, vAccounttypeID:=vAccounttypeID, vLedgerId:=vLedgerId, vPaymenttypeID:=vPaymenttypeID, vAccountName:=vAccountName, vShortCode:=vShortCode, vRestrictEnquiry:=vRestrictEnquiry, vRestrictUpdate:=vRestrictUpdate, vDeleteAtPurge:=vDeleteAtPurge, vContactName:=vContactName, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vAddressCountry:=vAddressCountry, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vPaymentName:=vPaymentName, vPaymentAccountCode:=vPaymentAccountCode, vPaymentBranchCode:=vPaymentBranchCode, vPaymentExpiryDate:=vPaymentExpiryDate, vPaymentReference1:=vPaymentReference1, vPaymentReference2:=vPaymentReference2, vCreditLimit:=vCreditLimit, vDiscountPercentage:=vDiscountPercentage, vSettlementPeriod:=vSettlementPeriod, vBankName:=vBankName, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vAccountKey:=vAccountKey, vNominalAccountID:=vNominalAccountID, vAccountStatusID:=vAccountStatusID, vPartySourceID:=vPartySourceID, vSubBranchID:=vSubBranchID, vProofListReportID:=vProofListReportID, vBordereauReportID:=vBordereauReportID, vAllowElectronicPayment:=vAllowElectronicPayment, vMoneyCalcAccType:=vMoneyCalcAccType, vParamArray:=vArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oaccount = Nothing
                Return result
            End If

            ' CF140199
            ' Check that the short code doesnt exist already
            m_lReturn = CType(CheckShortCodeUnique(oaccount.ShortCode), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Return RecordInUse
                Return gPMConstants.PMEReturnCode.PMRecordInUse
            End If

            ' Add Account to collection
            If (m_oAccounts.Count = 0) Then
                m_oAccounts.Add(Nothing)
            End If
            m_lReturn = CType(m_oAccounts.Add(oNewAccount:=oaccount), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oaccount = Nothing
                Return result
            End If

            oaccount = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the Account
    '              specified and updates the Account with the new values.
    '
    ' eck010900 Add Company as a modifyable property
    ' ***************************************************************** '
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' RDC 12112003 added vAllowElectronicPayment
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPurgefrequencyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vAccounttypeID As Object = Nothing, Optional ByRef vLedgerId As Object = Nothing, Optional ByRef vPaymenttypeID As Object = Nothing, Optional ByRef vAccountName As Object = Nothing, Optional ByRef vShortCode As String = "", Optional ByRef vRestrictEnquiry As Object = Nothing, Optional ByRef vRestrictUpdate As Object = Nothing, Optional ByRef vDeleteAtPurge As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vAddressCountry As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vPaymentName As Object = Nothing, Optional ByRef vPaymentAccountCode As Object = Nothing, Optional ByRef vPaymentBranchCode As Object = Nothing, Optional ByRef vPaymentExpiryDate As Object = Nothing, Optional ByRef vPaymentReference1 As Object = Nothing, Optional ByRef vPaymentReference2 As Object = Nothing, Optional ByRef vCreditLimit As Object = Nothing, Optional ByRef vDiscountPercentage As Object = Nothing, Optional ByRef vSettlementPeriod As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vNominalAccountID As Object = Nothing, Optional ByRef vAccountStatusID As Object = Nothing, Optional ByRef vPartySourceID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vProofListReportID As Object = Nothing, Optional ByRef vBordereauReportID As Object = Nothing, Optional ByRef vAllowElectronicPayment As Object = Nothing, Optional ByRef vMoneyCalcAccType As Object = Nothing, Optional ByRef vClientBankAccType As Object = Nothing, Optional ByRef vParamArray() As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oaccount As bACTAccount.Account
        Dim iStatus As gPMConstants.PMEComponentAction
        Dim vArray() As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vArray(ACHLastArrayPosition)



            vArray(ACClientBankAccTypeArrPos) = vClientBankAccType
            If Informations.IsArray(vParamArray) Then


                vArray(ACMerchantIdArrPos) = vParamArray(0)
            End If


            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oAccounts.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oaccount = m_oAccounts.Item(lRow)

            ' Check the Status of the Account

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oaccount.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' CF140199
            ' Check that the short code doesnt exist already

            ' Only check if its changed
            If vShortCode IsNot Nothing AndAlso vShortCode.Trim().ToLower() <> oaccount.ShortCode.Trim().ToLower() Then
                m_lReturn = CType(CheckShortCodeUnique(vShortCode), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Return RecordInUse
                    Return gPMConstants.PMEReturnCode.PMRecordInUse
                End If
            End If

            ' Update Account Attributes
            ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
            ' RDC 12112003 added vAllowElectronicPayment

            'Developer Guide 101
            'm_lReturn = CType(SetProperties(oaccount, iStatus, vAccountID:=CInt(vAccountID), vPurgefrequencyID:=CInt(vPurgefrequencyID), vCurrencyID:=CInt(vCurrencyID), vAccounttypeID:=CInt(vAccounttypeID), vLedgerId:=CInt(vLedgerId), vPaymenttypeID:=CInt(vPaymenttypeID), vAccountName:=CStr(vAccountName), vShortCode:=vShortCode, vRestrictEnquiry:=CInt(vRestrictEnquiry), vRestrictUpdate:=CInt(vRestrictUpdate), vDeleteAtPurge:=CInt(vDeleteAtPurge), vContactName:=CStr(vContactName), vAddress1:=CStr(vAddress1), vAddress2:=CStr(vAddress2), vAddress3:=CStr(vAddress3), vAddress4:=CStr(vAddress4), vPostalCode:=CStr(vPostalCode), vAddressCountry:=CInt(vAddressCountry), vPhoneAreaCode:=CStr(vPhoneAreaCode), vPhoneNumber:=CStr(vPhoneNumber), vPhoneExtension:=CStr(vPhoneExtension), vFaxAreaCode:=CStr(vFaxAreaCode), vFaxNumber:=CStr(vFaxNumber), vFaxExtension:=CStr(vFaxExtension), vPaymentName:=CStr(vPaymentName), vPaymentAccountCode:=CStr(vPaymentAccountCode), vPaymentBranchCode:=CStr(vPaymentBranchCode), vPaymentExpiryDate:=vPaymentExpiryDate, vPaymentReference1:=CStr(vPaymentReference1), vPaymentReference2:=CStr(vPaymentReference2), vCreditLimit:=CDbl(vCreditLimit), vDiscountPercentage:=CDbl(vDiscountPercentage), vSettlementPeriod:=CInt(vSettlementPeriod), vBankName:=CStr(vBankName), vBankAddress1:=CStr(vBankAddress1), vBankAddress2:=CStr(vBankAddress2), vBankAddress3:=CStr(vBankAddress3), vBankAddress4:=CStr(vBankAddress4), vBankPostalCode:=CStr(vBankPostalCode), vBankCountry:=CInt(vBankCountry), vBankPhoneAreaCode:=CStr(vBankPhoneAreaCode), vBankPhoneNumber:=CStr(vBankPhoneNumber), vBankPhoneExtension:=CStr(vBankPhoneExtension), vBankFaxAreaCode:=CStr(vBankFaxAreaCode), vBankFaxNumber:=CStr(vBankFaxNumber), vBankFaxExtension:=CStr(vBankFaxExtension), vComments:=CStr(vComments), vAccountKey:=CInt(vAccountKey), vNominalAccountID:=CInt(vNominalAccountID), vAccountStatusID:=CInt(vAccountStatusID), vPartySourceID:=CStr(vPartySourceID), vProofListReportID:=CInt(vProofListReportID), vBordereauReportID:=CInt(vBordereauReportID), vAllowElectronicPayment:=CBool(vAllowElectronicPayment), vMoneyCalcAccType:=CInt(vMoneyCalcAccType), vParamArray:=vArray), gPMConstants.PMEReturnCode)
            m_lReturn = CType(SetProperties(oaccount, iStatus, vAccountID:=vAccountID, vPurgefrequencyID:=vPurgefrequencyID, vCurrencyID:=vCurrencyID, vAccounttypeID:=vAccounttypeID, vLedgerId:=vLedgerId, vPaymenttypeID:=vPaymenttypeID, vAccountName:=vAccountName, vShortCode:=vShortCode, vRestrictEnquiry:=vRestrictEnquiry, vRestrictUpdate:=vRestrictUpdate, vDeleteAtPurge:=vDeleteAtPurge, vContactName:=vContactName, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vAddressCountry:=vAddressCountry, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vPaymentName:=vPaymentName, vPaymentAccountCode:=vPaymentAccountCode, vPaymentBranchCode:=vPaymentBranchCode, vPaymentExpiryDate:=vPaymentExpiryDate, vPaymentReference1:=vPaymentReference1, vPaymentReference2:=vPaymentReference2, vCreditLimit:=vCreditLimit, vDiscountPercentage:=vDiscountPercentage, vSettlementPeriod:=vSettlementPeriod, vBankName:=vBankName, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vAccountKey:=vAccountKey, vNominalAccountID:=vNominalAccountID, vAccountStatusID:=vAccountStatusID, vPartySourceID:=vPartySourceID, vProofListReportID:=vProofListReportID, vBordereauReportID:=vBordereauReportID, vAllowElectronicPayment:=vAllowElectronicPayment, vMoneyCalcAccType:=vMoneyCalcAccType, vParamArray:=vArray, vSubBranchID:=vSubBranchID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oaccount = Nothing
                Return result
            End If

            ' Release reference to Account
            oaccount = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Account can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oaccount As bACTAccount.Account

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oAccounts.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oaccount = m_oAccounts.Item(lRow)

            ' Check the Status of the Account

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oaccount.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oaccount.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oaccount.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Account
            oaccount = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oAccounts.Count()
                If (m_oAccounts.Item(lSub) IsNot Nothing) Then
                    Select Case m_oAccounts.Item(lSub).DatabaseStatus
                        Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                        Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                            result = gPMConstants.PMEReturnCode.PMDataChanged
                            Exit For
                    End Select
                End If
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oaccount As bACTAccount.Account
        Dim bTransStarted As Boolean
        Dim sCreditControlValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oAccounts.Count()
                oaccount = m_oAccounts.Item(lSub)


                Select Case oaccount.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(AddItem(oaccount), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(UpdateItem(oaccount), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                        'SD 09/01/2003 Start Credit Control changes
                        'If the account is being taken off hold, then carry out credit control operations
                        If oaccount.IsTakenOffHold Then
                            m_lReturn = CType(GetSystemOptionValue(CInt(ACCreditControlOptionNo), sCreditControlValue), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                Exit For
                            End If

                            'Only proceed if system option has been set
                            If String.Compare(sCreditControlValue, ACValueWhenCreditControlSet) = 0 Then

                                m_lReturn = TakeOffHold(oaccount.AccountID)
                                'Check for errors
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    Exit For
                                End If
                            End If
                        End If
                        'SD 09/01/2003 End

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(DeleteItem(oaccount), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oaccount = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oAccounts.Count()

                        ' With the item
                        With m_oAccounts.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oAccounts.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'eck180901 Add optional account display currency
    'developer guide no.33

    Public Function GetAccountBalance(ByRef r_vdAccountBalance As Object, ByVal v_vAccountID As Object) As Integer
        Return GetAccountBalance(r_vdAccountBalance:=r_vdAccountBalance, v_vAccountID:=v_vAccountID, v_vAccountingDate:=Nothing, r_vResultArray:=Nothing, r_vlAccountCurrencyId:=Nothing, r_vdAccountDebt:=Nothing, r_vAccountFloatBalance:=Nothing, r_vAccountOverDraftBalance:=Nothing)
    End Function

    Public Function GetAccountBalance(ByVal v_vAccountID As Object, ByVal v_vAccountingDate As Object, ByRef r_vResultArray(,) As Object) As Integer
        Return GetAccountBalance(r_vdAccountBalance:=Nothing, v_vAccountID:=v_vAccountID, v_vAccountingDate:=v_vAccountingDate, r_vResultArray:=r_vResultArray, r_vlAccountCurrencyId:=Nothing, r_vdAccountDebt:=Nothing, r_vAccountFloatBalance:=Nothing, r_vAccountOverDraftBalance:=Nothing)
    End Function

    Public Function GetAccountBalance(ByRef r_vdAccountBalance As Object, ByVal v_vAccountID As Object, ByVal v_vAccountingDate As Object, ByRef r_vlAccountCurrencyId As Object) As Integer
        Return GetAccountBalance(r_vdAccountBalance:=r_vdAccountBalance, v_vAccountID:=v_vAccountID, v_vAccountingDate:=v_vAccountingDate, r_vResultArray:=Nothing, r_vlAccountCurrencyId:=r_vlAccountCurrencyId, r_vdAccountDebt:=Nothing, r_vAccountFloatBalance:=Nothing, r_vAccountOverDraftBalance:=Nothing)
    End Function

    Public Function GetAccountBalance(ByRef r_vdAccountBalance As Object, ByVal v_vAccountID As Object, ByVal v_vAccountingDate As Object, ByRef r_vResultArray(,) As Object, ByRef r_vlAccountCurrencyId As Object, ByRef r_vdAccountDebt As Object, ByRef r_vAccountFloatBalance As Object, ByRef r_vAccountOverDraftBalance As Object) As Integer

        ' Optional params for the SP
        'developer guide no.33
        'start
        Dim result As Object
        Dim vCompanyIdParam As Object
        Dim vAccountIdParam As Object
        Dim vAccountingDateParam As Object
        Dim vPostingStatusParam As Object
        'end
        Dim vValue As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CTAF 130100 - Always use global value of Company ID.
            vCompanyIdParam = m_iCompanyID


            If Informations.IsNothing(v_vAccountID) Then


                vAccountIdParam = DBNull.Value
            Else


                vAccountIdParam = v_vAccountID
            End If

            ' CTAF 130300 - Removed Y2K problems!

            If Not Informations.IsNothing(v_vAccountingDate) Then
                'DD 16/07/2003: Trap no end date
                If v_vAccountingDate = #12/31/9999# Then

                    vAccountingDateParam = Nothing
                Else
                    vAccountingDateParam = v_vAccountingDate
                End If
            Else
                'eck010302 When getting balance need to include all transactions on account

                vAccountingDateParam = Nothing
            End If

            vPostingStatusParam = gACTLibrary.ACTPostStatusPosted

            m_oDatabase.Parameters.Clear()

            ' Filter by company Id if Multibranch accounting enabled
            ' RDC 13112003

            'Developer guide no.33
            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iCompanyID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)



            If gPMFunctions.NullToString(vValue) = "1" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(vCompanyIdParam), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else

                'developer guide no 85. 
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(vAccountIdParam), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="accounting_date", vValue:=vAccountingDateParam, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="postingstatus_id", vValue:=CStr(vPostingStatusParam), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectBalanceSQL, sSQLName:=ACSelectBalanceName, bStoredProcedure:=ACSelectBalanceStored, lNumberRecords:=gACTLibrary.ACTMaxAccounts, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DD 13/08/2002: Altered for new dPMDAO

            If Not Informations.IsNothing(r_vResultArray) Then
                ' with no array and more than one result
                ' what else can we do ?
                If m_oDatabase.Records.Count() = 0 Then
                    r_vdAccountBalance = 0
                Else
                    'Developer Guide no. 111
                    r_vdAccountBalance = m_oDatabase.Records.Item(0).Fields()("sum_amount")
                    'eck180901

                    If Not Informations.IsNothing(r_vlAccountCurrencyId) Then
                        'Developer Guide no. 111
                        r_vlAccountCurrencyId = m_oDatabase.Records.Item(0).Fields()("currency_id")
                    End If

                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountBalance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountBalance", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    '
    ' Description - GetInstalmentDebt
    '               Return total amount of Instalments that are overdue for a particular account or policy.
    '
    '
    ' Author : Kevin Grandison
    '
    ' Date  : 02/07/2003
    '
    Public Function GetInstalmentDebt(ByVal lAccountID As Integer, ByRef r_vdInstalmentDebt As Double) As Integer

        Dim result As Integer = 0
        Try

            Dim vThisResultArray(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no 85. 
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInstalmentDebtSQL, sSQLName:=ACSelectInstalmentDebtName, bStoredProcedure:=ACSelectInstalmentDebtstored, vResultArray:=vThisResultArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Informations.IsArray(vThisResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get Instalment debt

            r_vdInstalmentDebt = Val(CStr(vThisResultArray(ACInstalmentDebt, 0)))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstalmentDebt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstalmentDebt", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAccountOSTransactions(ByRef vAccount_id As Object, ByRef vOSTransactions(,) As Object) As Integer
        Return GetAccountOSTransactions(vAccount_id:=vAccount_id, vOSTransactions:=vOSTransactions, r_cAccountBaseBalance:=0, r_iBaseCount:=0, r_cAccountBalance:=0, r_iAccountCount:=0)
    End Function

    Public Function GetAccountOSTransactions(ByRef vAccount_id As Object, ByRef vOSTransactions(,) As Object, ByRef r_cAccountBaseBalance As Decimal) As Integer
        Return GetAccountOSTransactions(vAccount_id:=vAccount_id, vOSTransactions:=vOSTransactions, r_cAccountBaseBalance:=r_cAccountBaseBalance, r_iBaseCount:=0, r_cAccountBalance:=0, r_iAccountCount:=0)
    End Function

    'eck230501 Get list of outstanding transactions
    Public Function GetAccountOSTransactions(ByRef vAccount_id As Object, ByRef vOSTransactions(,) As Object, ByRef r_cAccountBaseBalance As Decimal, ByRef r_iBaseCount As Integer, ByRef r_cAccountBalance As Decimal, ByRef r_iAccountCount As Integer, Optional ByVal receipt_transdetail_id As Integer = 0, Optional r_iDNGIND As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vValue As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", CInt(vAccount_id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            'Filter by company Id if Multibranch accounting enabled

            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iCompanyID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)

            If gPMFunctions.NullToString(vValue) = "1" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "company_id", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "company_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "receipt_transdetail_id", receipt_transdetail_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "base_currency_count", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "base_balance", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_currency_count", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_balance", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "DNGIND", r_iDNGIND, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectTransForAllocationSQL, sSQLName:=ACSelectTransForAllocationName, bStoredProcedure:=ACSelectTransForAllocationStored, vResultArray:=vOSTransactions)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The count is the number of branch currencies found for the set of OS Transactions
            r_iBaseCount = gPMFunctions.NullToInteger(m_oDatabase.Parameters.Item("base_currency_count").Value)

            Select Case r_iBaseCount
                Case 0
                    Return gPMConstants.PMEReturnCode.PMNotFound
                Case 1
                    result = gPMConstants.PMEReturnCode.PMTrue
                Case Else
                    'There is more than one base currency so auto-allocation is not allowed
                    Return gPMConstants.PMEReturnCode.PMNotFound
            End Select

            'The count is the number of account currencies found for the set of OS Transactions
            r_iAccountCount = gPMFunctions.NullToInteger(m_oDatabase.Parameters.Item("account_currency_count").Value)

            'This is the balance of the account based on the Base Currency
            r_cAccountBaseBalance = gPMFunctions.NullToCurrency(m_oDatabase.Parameters.Item("base_balance").Value)

            'This is the balance of the account based on the Account Currency
            r_cAccountBalance = gPMFunctions.NullToCurrency(m_oDatabase.Parameters.Item("account_balance").Value)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountOSTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountOSTransactions", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    Public Function GetAccountOSTransactionsForReceipt(ByRef vAccount_id As Object, ByRef vOSTransactions(,) As Object, ByRef r_cAccountBaseBalance As Decimal) As Integer
        Return GetAccountOSTransactionsForReceipt(vAccount_id:=vAccount_id, vOSTransactions:=vOSTransactions, r_cAccountBaseBalance:=r_cAccountBaseBalance, r_iBaseCount:=0, r_cAccountBalance:=0, r_iAccountCount:=0)
    End Function

    Public Function GetAccountOSTransactionsForReceipt(ByRef vAccount_id As Object, ByRef vOSTransactions(,) As Object, ByRef r_cAccountBaseBalance As Decimal, ByRef r_iBaseCount As Integer, ByRef r_cAccountBalance As Decimal, ByRef r_iAccountCount As Integer) As Integer

        Dim result As Integer = 0
        'Dim iCount As Integer
        Dim vValue As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", CInt(vAccount_id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            'Filter by company Id if Multibranch accounting enabled

            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iCompanyID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)

            If gPMFunctions.NullToString(vValue) = "1" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "company_id", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "company_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            End If


            bPMAddParameter.AddParameterLite(m_oDatabase, "base_currency_count", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "base_balance", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_currency_count", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_balance", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency, False)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectTransForReceiptAllocationSQL, sSQLName:=ACSelectTransForAllocationName, bStoredProcedure:=ACSelectTransForAllocationStored, vResultArray:=vOSTransactions)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The count is the number of branch currencies found for the set of OS Transactions
            r_iBaseCount = gPMFunctions.NullToInteger(m_oDatabase.Parameters.Item("base_currency_count").Value)

            Select Case r_iBaseCount
                Case 0
                    Return gPMConstants.PMEReturnCode.PMNotFound
                Case 1
                    result = gPMConstants.PMEReturnCode.PMTrue
                Case Else
                    'There is more than one base currency so auto-allocation is not allowed
                    Return gPMConstants.PMEReturnCode.PMNotFound
            End Select

            'The count is the number of account currencies found for the set of OS Transactions
            r_iAccountCount = gPMFunctions.NullToInteger(m_oDatabase.Parameters.Item("account_currency_count").Value)

            'This is the balance of the account based on the Base Currency
            r_cAccountBaseBalance = gPMFunctions.NullToCurrency(m_oDatabase.Parameters.Item("base_balance").Value)

            'This is the balance of the account based on the Account Currency
            r_cAccountBalance = gPMFunctions.NullToCurrency(m_oDatabase.Parameters.Item("account_balance").Value)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountOSTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountOSTransactions", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Public Function GetAccountOSTransactionsForReceipt(ByRef vAccount_id As Object, ByRef vOSTransactions As Object, ByRef r_cAccountBaseBalance As Decimal) As Integer
        Return GetAccountOSTransactionsForReceipt(vAccount_id:=vAccount_id, vOSTransactions:=vOSTransactions, r_cAccountBaseBalance:=r_cAccountBaseBalance, r_iBaseCount:=0, r_cAccountBalance:=0, r_iAccountCount:=0, receipt_transdetail_id:=0, v_sInstalment_Plan_Ref:="")
    End Function

    Public Function GetAccountOSTransactionsForReceipt(ByRef vAccount_id As Object, ByRef vOSTransactions As Object, ByRef r_cAccountBaseBalance As Decimal, ByVal receipt_transdetail_id As Integer) As Integer
        Return GetAccountOSTransactionsForReceipt(vAccount_id:=vAccount_id, vOSTransactions:=vOSTransactions, r_cAccountBaseBalance:=r_cAccountBaseBalance, r_iBaseCount:=0, r_cAccountBalance:=0, r_iAccountCount:=0, receipt_transdetail_id:=receipt_transdetail_id, v_sInstalment_Plan_Ref:="")
    End Function

    Public Function GetAccountOSTransactionsForReceipt(ByRef vAccount_id As Object, ByRef vOSTransactions As Object, ByRef r_cAccountBaseBalance As Decimal, ByVal receipt_transdetail_id As Integer, ByVal v_sInstalment_Plan_Ref As String) As Integer
        Return GetAccountOSTransactionsForReceipt(vAccount_id:=vAccount_id, vOSTransactions:=vOSTransactions, r_cAccountBaseBalance:=r_cAccountBaseBalance, r_iBaseCount:=0, r_cAccountBalance:=0, r_iAccountCount:=0, receipt_transdetail_id:=receipt_transdetail_id, v_sInstalment_Plan_Ref:=v_sInstalment_Plan_Ref)
    End Function

    Public Function GetAccountOSTransactionsForReceipt(ByRef vAccount_id As Object, ByRef vOSTransactions As Object, ByRef r_cAccountBaseBalance As Decimal, ByRef r_iBaseCount As Integer, ByRef r_cAccountBalance As Decimal, ByRef r_iAccountCount As Integer, ByVal receipt_transdetail_id As Integer, ByVal v_sInstalment_Plan_Ref As String) As Integer

        Dim result As Integer = 0
        Dim vValue As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", CInt(vAccount_id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            'Filter by company Id if Multibranch accounting enabled

            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iCompanyID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)

            If gPMFunctions.NullToString(vValue) = "1" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "company_id", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "company_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "receipt_transdetail_id", receipt_transdetail_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "base_currency_count", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "base_balance", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_currency_count", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger, False)

            bPMAddParameter.AddParameterLite(m_oDatabase, "account_balance", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency, False)

            If Not String.IsNullOrEmpty(v_sInstalment_Plan_Ref) Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "instalment_plan_ref", v_sInstalment_Plan_Ref, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectTransForReceiptAllocationSQL, sSQLName:=ACSelectTransForAllocationName, bStoredProcedure:=ACSelectTransForAllocationStored, vResultArray:=vOSTransactions)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The count is the number of branch currencies found for the set of OS Transactions
            r_iBaseCount = gPMFunctions.NullToInteger(m_oDatabase.Parameters.Item("base_currency_count").Value)

            Select Case r_iBaseCount
                Case 0
                    Return gPMConstants.PMEReturnCode.PMNotFound
                Case 1
                    result = gPMConstants.PMEReturnCode.PMTrue
                Case Else
                    'There is more than one base currency so auto-allocation is not allowed
                    Return gPMConstants.PMEReturnCode.PMNotFound
            End Select

            'The count is the number of account currencies found for the set of OS Transactions
            r_iAccountCount = gPMFunctions.NullToInteger(m_oDatabase.Parameters.Item("account_currency_count").Value)

            'This is the balance of the account based on the Base Currency
            r_cAccountBaseBalance = gPMFunctions.NullToCurrency(m_oDatabase.Parameters.Item("base_balance").Value)

            'This is the balance of the account based on the Account Currency
            r_cAccountBalance = gPMFunctions.NullToCurrency(m_oDatabase.Parameters.Item("account_balance").Value)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountOSTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountOSTransactions", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: DeleteAllocationLocks
    '
    ' Description: Deletes all the allocation locks in the pmlock table
    '              for the current user
    '
    ' History: 13/05/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteAllocationLocks(ByVal v_vOSTransactions(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACTransdetailId As Integer = 0

            Dim sSQL As New StringBuilder

            m_oDatabase.Parameters.Clear()

            sSQL = New StringBuilder("")
            For i As Integer = 0 To v_vOSTransactions.GetUpperBound(1)

                sSQL.Append(
                            " EXEC spu_ACT_Delete_Allocation_Locks " & CStr(m_iUserID) & "," & CStr(v_vOSTransactions(ACTransdetailId, i)) & Strings.ChrW(13) & Strings.ChrW(10))
            Next i

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:=ACDeleteAllocationLocksName, bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAllocationLocks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllocationLocks", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ''SW 28/01/2003 Get list of outstanding debit transactions against a given account filtered by purchase Invoice No
    'Public Function GetAccountOSTransForClaimPayment( _
    ''                            ByVal v_lAccount_id As Long, _
    ''                            ByVal v_lDocumentId As Long, _
    ''                            ByRef vOSTransactions As Variant) As Long
    '
    'Dim iCount As Integer
    '
    '
    'On Error GoTo Err_GetAccountOSTransForClaimPayment
    '
    '    GetAccountOSTransForClaimPayment = PMTrue
    '
    '    m_oDatabase.Parameters.Clear
    '
    '
    '    m_lReturn& = m_oDatabase.Parameters.Add( _
    ''            sName:="account_id", _
    ''            vValue:=CLng(v_lAccount_id), _
    ''            iDirection:=PMParamInput, _
    ''            iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetAccountOSTransForClaimPayment = PMFalse
    '        Exit Function
    '    End If
    '
    '
    '    m_lReturn& = m_oDatabase.Parameters.Add( _
    ''            sName:="purchaseinvoiceno", _
    ''            vValue:=v_sPurchaseInvoiceNo, _
    ''            iDirection:=PMParamInput, _
    ''            iDataType:=PMString)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetAccountOSTransForClaimPayment = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn& = m_oDatabase.SQLSelect( _
    ''        sSQL:=ACSelectAccountOSTransForClaimPaymentSQL, _
    ''        sSQLName:=ACAccountOSTransForClaimPaymentName, _
    ''        bStoredProcedure:=True, _
    ''        vResultArray:=vOSTransactions)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '      GetAccountOSTransForClaimPayment = PMFalse
    '      Exit Function
    '    End If
    '
    '    If IsArray(vOSTransactions) = False Then
    '        GetAccountOSTransForClaimPayment = PMNotFound
    '        Exit Function
    '    End If
    '
    '    Exit Function
    '
    '
    'Err_GetAccountOSTransForClaimPayment:
    '
    '    GetAccountOSTransForClaimPayment = PMError
    '
    '    ' Log Error Message
    '    LogMessage sUsername:=m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetAccountOSTransForClaimPayment Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetAccountOSTransForClaimPayment", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    '
    'End Function



    ' ***************************************************************** '
    ' Name : IsPostCode (Public)
    '
    ' Desc : return PMTRUE if this address required postcode
    '
    ' Hist : 29 June 2001 Tinny - Created
    ' ***************************************************************** '
    Public Function IsPostCode(ByVal v_lAccountId As Integer, ByRef r_lResult As Integer) As Integer


        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACSelIsPostCodeSQL, sSQLName:=ACSelIsPostCodeName, bStoredProcedure:=ACSelIsPostCodeStored, vResultArray:=vResultArray, bKeepNulls:=True)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'if address_country is null then we show postcode
            If Not Informations.IsArray(vResultArray) Then
                r_lResult = gPMConstants.PMEReturnCode.PMTrue
                Return result
            End If

            Dim auxVar As Object = vResultArray(0, 0)


            If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                If CStr(vResultArray(0, 0)).Trim().ToUpper() = "GBR" Then
                    r_lResult = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsPostCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsPostCode", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' PUBLIC Methods (End)
    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oaccount As bACTAccount.Account) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add AccountID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Account_id", vValue:=CStr(oaccount.AccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oaccount:=oaccount), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oaccount.AccountID = m_oDatabase.Parameters.Item("Account_id").Value

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oaccount As bACTAccount.Account) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' CTAF 191200 - Switched the parameters around

        ' Add AccountID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Account_id", vValue:=CStr(oaccount.AccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oaccount:=oaccount), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oAccount.Timestamp, _
        'iDirection:=PMParamInput, _
        'iDataType:=PMBinary)

        'If (m_lReturn& <> PMTrue) Then
        '    UpdateItem = PMFalse
        '    Exit Function
        'End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check to see that the record was updated OK

        If lRecordsAffected > 0 Then
            ' Updated No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oaccount As bACTAccount.Account) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the AccountID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Account_id", vValue:=CStr(oaccount.AccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            ' Deleted, No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied Account properties from a database
    '              record.
    'eck010900 return company
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oaccount As bACTAccount.Account, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no. 112(Guide)
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oaccount

            .AccountID = oFields("account_id")

            If Convert.IsDBNull(oFields("purgefrequency_id")) Or Informations.IsNothing(oFields("purgefrequency_id")) Then
                .PurgefrequencyID = 0
            Else
                .PurgefrequencyID = oFields("purgefrequency_id")
            End If
            'eck010900

            If Convert.IsDBNull(oFields("company_id")) Or Informations.IsNothing(oFields("company_id")) Then
                .CompanyID = 0
            Else
                .CompanyID = oFields("company_id")
            End If
            '

            If Convert.IsDBNull(oFields("currency_id")) Or Informations.IsNothing(oFields("currency_id")) Then
                .CurrencyID = 0
            Else
                .CurrencyID = oFields("currency_id")
            End If

            If Convert.IsDBNull(oFields("accounttype_id")) Or Informations.IsNothing(oFields("accounttype_id")) Then
                .AccounttypeID = 0
            Else
                .AccounttypeID = oFields("accounttype_id")
            End If

            If Convert.IsDBNull(oFields("ledger_id")) Or Informations.IsNothing(oFields("ledger_id")) Then
                .LedgerID = 0
            Else
                .LedgerID = oFields("ledger_id")
            End If

            If Convert.IsDBNull(oFields("paymenttype_id")) Or Informations.IsNothing(oFields("paymenttype_id")) Then
                .PaymenttypeID = 0
            Else
                .PaymenttypeID = oFields("paymenttype_id")
            End If

            If Convert.IsDBNull(oFields("account_name")) Or Informations.IsNothing(oFields("account_name")) Then
                .AccountName = ""
            Else
                .AccountName = oFields("account_name")
            End If
            If Convert.IsDBNull(oFields("short_code")) Or Informations.IsNothing(oFields("short_code")) Then
                .ShortCode = ""
            Else
                .ShortCode = oFields("short_code")
            End If
            If Convert.IsDBNull(oFields("restrict_enquiry")) Or Informations.IsNothing(oFields("restrict_enquiry")) Then
                .RestrictEnquiry = ""
            Else
                .RestrictEnquiry = oFields("restrict_enquiry")
            End If
            If Convert.IsDBNull(oFields("restrict_update")) Or Informations.IsNothing(oFields("restrict_update")) Then
                .RestrictUpdate = ""
            Else
                .RestrictUpdate = oFields("restrict_update")
            End If
            If Convert.IsDBNull(oFields("delete_at_purge")) Or Informations.IsNothing(oFields("delete_at_purge")) Then
                .DeleteAtPurge = ""
            Else
                .DeleteAtPurge = oFields("delete_at_purge")
            End If
            If Convert.IsDBNull(oFields("contact_name")) Or Informations.IsNothing(oFields("contact_name")) Then
                .ContactName = ""
            Else
                .ContactName = oFields("contact_name")
            End If

            If Convert.IsDBNull(oFields("address1")) Or Informations.IsNothing(oFields("address1")) Then
                .Address1 = ""
            Else
                .Address1 = oFields("address1")
            End If

            If Convert.IsDBNull(oFields("address2")) Or Informations.IsNothing(oFields("address2")) Then
                .Address2 = ""
            Else
                .Address2 = oFields("address2")
            End If

            If Convert.IsDBNull(oFields("address3")) Or Informations.IsNothing(oFields("address3")) Then
                .Address3 = ""
            Else
                .Address3 = oFields("address3")
            End If

            If Convert.IsDBNull(oFields("address4")) Or Informations.IsNothing(oFields("address4")) Then
                .Address4 = ""
            Else
                .Address4 = oFields("address4")
            End If

            If Convert.IsDBNull(oFields("postal_code")) Or Informations.IsNothing(oFields("postal_code")) Then
                .PostalCode = ""
            Else
                .PostalCode = oFields("postal_code")
            End If

            If Convert.IsDBNull(oFields("address_country")) Or Informations.IsNothing(oFields("address_country")) Then
                .AddressCountry = 0
            Else
                .AddressCountry = oFields("address_country")
            End If

            If Convert.IsDBNull(oFields("phone_area_code")) Or Informations.IsNothing(oFields("phone_area_code")) Then
                .PhoneAreaCode = ""
            Else
                .PhoneAreaCode = oFields("phone_area_code")
            End If

            If Convert.IsDBNull(oFields("phone_number")) Or Informations.IsNothing(oFields("phone_number")) Then
                .PhoneNumber = ""
            Else
                .PhoneNumber = oFields("phone_number")
            End If

            If Convert.IsDBNull(oFields("phone_extension")) Or Informations.IsNothing(oFields("phone_extension")) Then
                .PhoneExtension = ""
            Else
                .PhoneExtension = oFields("phone_extension")
            End If

            If Convert.IsDBNull(oFields("fax_area_code")) Or Informations.IsNothing(oFields("fax_area_code")) Then
                .FaxAreaCode = ""
            Else
                .FaxAreaCode = oFields("fax_area_code")
            End If

            If Convert.IsDBNull(oFields("fax_number")) Or Informations.IsNothing(oFields("fax_number")) Then
                .FaxNumber = ""
            Else
                .FaxNumber = oFields("fax_number")
            End If

            If Convert.IsDBNull(oFields("fax_extension")) Or Informations.IsNothing(oFields("fax_extension")) Then
                .FaxExtension = ""
            Else
                .FaxExtension = oFields("fax_extension")
            End If

            If Convert.IsDBNull(oFields("payment_name")) Or Informations.IsNothing(oFields("payment_name")) Then
                .PaymentName = ""
            Else
                .PaymentName = oFields("payment_name")
            End If

            If Convert.IsDBNull(oFields("payment_account_code")) Or Informations.IsNothing(oFields("payment_account_code")) Then
                .PaymentAccountCode = ""
            Else
                .PaymentAccountCode = oFields("payment_account_code")
            End If

            If Convert.IsDBNull(oFields("payment_branch_code")) Or Informations.IsNothing(oFields("payment_branch_code")) Then
                .PaymentBranchCode = ""
            Else
                .PaymentBranchCode = oFields("payment_branch_code")
            End If

            If Convert.IsDBNull(oFields("payment_expiry_date")) Or Informations.IsNothing(oFields("payment_expiry_date")) Then
                .PaymentExpiryDate = #12/30/1899#
            Else
                .PaymentExpiryDate = oFields("payment_expiry_date")
            End If

            If Convert.IsDBNull(oFields("payment_reference1")) Or Informations.IsNothing(oFields("payment_reference1")) Then
                .PaymentReference1 = ""
            Else
                .PaymentReference1 = oFields("payment_reference1")
            End If

            If Convert.IsDBNull(oFields("payment_reference2")) Or Informations.IsNothing(oFields("payment_reference2")) Then
                .PaymentReference2 = ""
            Else
                .PaymentReference2 = oFields("payment_reference2")
            End If
            ' RAW 17/12/2002 : PS187 : Added

            If Convert.IsDBNull(oFields("prooflist_report_id")) Or Informations.IsNothing(oFields("prooflist_report_id")) Then
                .ProofListReportID = 0
            Else
                .ProofListReportID = oFields("prooflist_report_id")
            End If

            If Convert.IsDBNull(oFields("bordereau_report_id")) Or Informations.IsNothing(oFields("bordereau_report_id")) Then
                .BordereauReportID = 0
            Else
                .BordereauReportID = oFields("bordereau_report_id")
            End If
            ' RAW 17/12/2002 : PS187 : end

            If Convert.IsDBNull(oFields("credit_limit")) Or Informations.IsNothing(oFields("credit_limit")) Then
                .CreditLimit = 0
            Else
                .CreditLimit = oFields("credit_limit")
            End If

            If Convert.IsDBNull(oFields("discount_percentage")) Or Informations.IsNothing(oFields("discount_percentage")) Then
                .DiscountPercentage = 0
            Else
                .DiscountPercentage = oFields("discount_percentage")
            End If

            If Convert.IsDBNull(oFields("settlement_period")) Or Informations.IsNothing(oFields("settlement_period")) Then
                .SettlementPeriod = 0
            Else
                .SettlementPeriod = oFields("settlement_period")
            End If

            If Convert.IsDBNull(oFields("bank_name")) Or Informations.IsNothing(oFields("bank_name")) Then
                .BankName = ""
            Else
                .BankName = oFields("bank_name")
            End If

            If Convert.IsDBNull(oFields("bank_address1")) Or Informations.IsNothing(oFields("bank_address1")) Then
                .BankAddress1 = ""
            Else
                .BankAddress1 = oFields("bank_address1")
            End If

            If Convert.IsDBNull(oFields("bank_address2")) Or Informations.IsNothing(oFields("bank_address2")) Then
                .BankAddress2 = ""
            Else
                .BankAddress2 = oFields("bank_address2")
            End If

            If Convert.IsDBNull(oFields("bank_address3")) Or Informations.IsNothing(oFields("bank_address3")) Then
                .BankAddress3 = ""
            Else
                .BankAddress3 = oFields("bank_address3")
            End If

            If Convert.IsDBNull(oFields("bank_address4")) Or Informations.IsNothing(oFields("bank_address4")) Then
                .BankAddress4 = ""
            Else
                .BankAddress4 = oFields("bank_address4")
            End If

            If Convert.IsDBNull(oFields("bank_postal_code")) Or Informations.IsNothing(oFields("bank_postal_code")) Then
                .BankPostalCode = ""
            Else
                .BankPostalCode = oFields("bank_postal_code")
            End If

            If Convert.IsDBNull(oFields("bank_country")) Or Informations.IsNothing(oFields("bank_country")) Then
                .BankCountry = 0
            Else
                .BankCountry = oFields("bank_country")
            End If

            If Convert.IsDBNull(oFields("bank_phone_area_code")) Or Informations.IsNothing(oFields("bank_phone_area_code")) Then
                .BankPhoneAreaCode = ""
            Else
                .BankPhoneAreaCode = oFields("bank_phone_area_code")
            End If

            If Convert.IsDBNull(oFields("bank_phone_number")) Or Informations.IsNothing(oFields("bank_phone_number")) Then
                .BankPhoneNumber = ""
            Else
                .BankPhoneNumber = oFields("bank_phone_number")
            End If

            If Convert.IsDBNull(oFields("bank_phone_extension")) Or Informations.IsNothing(oFields("bank_phone_extension")) Then
                .BankPhoneExtension = ""
            Else
                .BankPhoneExtension = oFields("bank_phone_extension")
            End If

            If Convert.IsDBNull(oFields("bank_fax_area_code")) Or Informations.IsNothing(oFields("bank_fax_area_code")) Then
                .BankFaxAreaCode = ""
            Else
                .BankFaxAreaCode = oFields("bank_fax_area_code")
            End If

            If Convert.IsDBNull(oFields("bank_fax_number")) Or Informations.IsNothing(oFields("bank_fax_number")) Then
                .BankFaxNumber = ""
            Else
                .BankFaxNumber = oFields("bank_fax_number")
            End If

            If Convert.IsDBNull(oFields("bank_fax_extension")) Or Informations.IsNothing(oFields("bank_fax_extension")) Then
                .BankFaxExtension = ""
            Else
                .BankFaxExtension = oFields("bank_fax_extension")
            End If

            If Convert.IsDBNull(oFields("comments")) Or Informations.IsNothing(oFields("comments")) Then
                .Comments = ""
            Else
                .Comments = oFields("comments")
            End If

            If Convert.IsDBNull(oFields("account_key")) Or Informations.IsNothing(oFields("account_key")) Then
                .AccountKey = 0
            Else
                .AccountKey = oFields("account_key")
            End If


            If Convert.IsDBNull(oFields("nominal_account_id")) Or Informations.IsNothing(oFields("nominal_account_id")) Then
                .NominalAccountID = 0
            Else
                .NominalAccountID = oFields("nominal_account_id")
            End If


            If Convert.IsDBNull(oFields("accountstatus_id")) Or Informations.IsNothing(oFields("accountstatus_id")) Then
                .AccountStatusID = 0
            Else
                .AccountStatusID = oFields("accountstatus_id")
            End If

            ' RDC 12112003

            If Convert.IsDBNull(oFields("allow_electronic_payment")) Or Informations.IsNothing(oFields("allow_electronic_payment")) Then
                .AllowElectronicPayment = False
            Else
                .AllowElectronicPayment = oFields("allow_electronic_payment")
            End If


            If Convert.IsDBNull(oFields("client_money_calc_account_type")) Or Informations.IsNothing(oFields("client_money_calc_account_type")) Then
                .MoneyCalcAccType = 0
            Else
                .MoneyCalcAccType = oFields("client_money_calc_account_type")
            End If


            If Convert.IsDBNull(oFields("client_bank_account_type")) Or Informations.IsNothing(oFields("client_bank_account_type")) Then
                .ClientBankAccType = "-1" 'CStr(-1)
            Else
                .ClientBankAccType = oFields("client_bank_account_type")
            End If


            If Convert.IsDBNull(oFields("merchant_id")) Or Informations.IsNothing(oFields("merchant_id")) Then
                .MerchantId = ""
            Else
                .MerchantId = oFields("merchant_id")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Account property values.
    '
    ' eck010900 Add Company as a modifyable property
    ' ***************************************************************** '
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' RDC 12112003 added vAllowElectronicPayment
    'Developer Guide No 101
    Private Function SetProperties(ByRef oaccount As bACTAccount.Account, ByRef iStatus As Integer, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPurgefrequencyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vAccounttypeID As Object = Nothing, Optional ByRef vLedgerId As Object = Nothing, Optional ByRef vPaymenttypeID As Object = Nothing, Optional ByRef vAccountName As Object = Nothing, Optional ByRef vShortCode As Object = Nothing, Optional ByRef vRestrictEnquiry As Object = Nothing, Optional ByRef vRestrictUpdate As Object = Nothing, Optional ByRef vDeleteAtPurge As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vAddressCountry As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vPaymentName As Object = Nothing, Optional ByRef vPaymentAccountCode As Object = Nothing, Optional ByRef vPaymentBranchCode As Object = Nothing, Optional ByRef vPaymentExpiryDate As Object = Nothing, Optional ByRef vPaymentReference1 As Object = Nothing, Optional ByRef vPaymentReference2 As Object = Nothing, Optional ByRef vCreditLimit As Double = 0, Optional ByRef vDiscountPercentage As Double = 0, Optional ByRef vSettlementPeriod As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vNominalAccountID As Object = Nothing, Optional ByRef vAccountStatusID As Object = Nothing, Optional ByRef vPartySourceID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vProofListReportID As Object = Nothing, Optional ByRef vBordereauReportID As Object = Nothing, Optional ByRef vAllowElectronicPayment As Object = Nothing, Optional ByRef vMoneyCalcAccType As Object = Nothing, Optional ByRef vParamArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean
        'Developer Guide 33
        'Dim vClientBankAccType As String = ""
        'Dim vMerchantId As String = ""
        Dim vClientBankAccType As Object = Nothing
        Dim vMerchantId As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        If Informations.IsArray(vParamArray) Then
            'Developer Guide no 33
            vClientBankAccType = vParamArray(ACClientBankAccTypeArrPos)

            vMerchantId = vParamArray(ACMerchantIdArrPos)
        End If

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
            ' RDC 12112003 added vAllowElectronicPayment
            m_lReturn = CType(CheckMandatory(vAccountID:=vAccountID, vPurgefrequencyID:=vPurgefrequencyID, vCurrencyID:=vCurrencyID, vAccounttypeID:=vAccounttypeID, vLedgerId:=vLedgerId, vPaymenttypeID:=vPaymenttypeID, vAccountName:=vAccountName, vShortCode:=vShortCode, vRestrictEnquiry:=vRestrictEnquiry, vRestrictUpdate:=vRestrictUpdate, vDeleteAtPurge:=vDeleteAtPurge, vContactName:=vContactName, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vAddressCountry:=vAddressCountry, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vPaymentName:=vPaymentName, vPaymentAccountCode:=vPaymentAccountCode, vPaymentBranchCode:=vPaymentBranchCode, vPaymentExpiryDate:=vPaymentExpiryDate, vPaymentReference1:=vPaymentReference1, vPaymentReference2:=vPaymentReference2, vCreditLimit:=vCreditLimit, vDiscountPercentage:=vDiscountPercentage, vSettlementPeriod:=vSettlementPeriod, vBankName:=vBankName, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vAccountKey:=vAccountKey, vNominalAccountID:=vNominalAccountID, vAccountStatusID:=vAccountStatusID, vSubBranchID:=vSubBranchID, vProofListReportID:=vProofListReportID, vBordereauReportID:=vBordereauReportID, vAllowElectronicPayment:=vAllowElectronicPayment, vMoneyCalcAccType:=vMoneyCalcAccType, vClientBankAccType:=vClientBankAccType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
            ' RDC 12112003 added vAllowElectronicPayment

            'developer guide no. 'Change as per VB Code
            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vAccountID:=vAccountID, vPurgefrequencyID:=vPurgefrequencyID, vCurrencyID:=vCurrencyID, vAccounttypeID:=vAccounttypeID, vLedgerId:=vLedgerId, vPaymenttypeID:=vPaymenttypeID, vAccountName:=vAccountName, vShortCode:=vShortCode, vRestrictEnquiry:=vRestrictEnquiry, vRestrictUpdate:=vRestrictUpdate, vDeleteAtPurge:=vDeleteAtPurge, vContactName:=vContactName, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vAddressCountry:=vAddressCountry, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vPaymentName:=vPaymentName, vPaymentAccountCode:=vPaymentAccountCode, vPaymentBranchCode:=vPaymentBranchCode, vPaymentExpiryDate:=vPaymentExpiryDate, vPaymentReference1:=vPaymentReference1, vPaymentReference2:=vPaymentReference2, vCreditLimit:=vCreditLimit, vDiscountPercentage:=vDiscountPercentage, vSettlementPeriod:=vSettlementPeriod, vBankName:=vBankName, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vAccountKey:=vAccountKey, vNominalAccountID:=vNominalAccountID, vAccountStatusID:=vAccountStatusID, vPartySourceID:=vPartySourceID, vSubBranchID:=vSubBranchID, vProofListReportID:=vProofListReportID, vBordereauReportID:=vBordereauReportID, vAllowElectronicPayment:=vAllowElectronicPayment, vMoneyCalcAccType:=vMoneyCalcAccType, vClientBankAccType:=vClientBankAccType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
        ' RDC 12112003 added vAllowElectronicPayment
        m_lReturn = CType(Validate(vAccountID:=vAccountID, vPurgefrequencyID:=vPurgefrequencyID, vCurrencyID:=vCurrencyID, vAccounttypeID:=vAccounttypeID, vLedgerId:=vLedgerId, vPaymenttypeID:=vPaymenttypeID, vAccountName:=vAccountName, vShortCode:=vShortCode, vRestrictEnquiry:=vRestrictEnquiry, vRestrictUpdate:=vRestrictUpdate, vDeleteAtPurge:=vDeleteAtPurge, vContactName:=vContactName, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vAddressCountry:=vAddressCountry, vPhoneAreaCode:=vPhoneAreaCode, vPhoneNumber:=vPhoneNumber, vPhoneExtension:=vPhoneExtension, vFaxAreaCode:=vFaxAreaCode, vFaxNumber:=vFaxNumber, vFaxExtension:=vFaxExtension, vPaymentName:=vPaymentName, vPaymentAccountCode:=vPaymentAccountCode, vPaymentBranchCode:=vPaymentBranchCode, vPaymentExpiryDate:=vPaymentExpiryDate, vPaymentReference1:=vPaymentReference1, vPaymentReference2:=vPaymentReference2, vCreditLimit:=vCreditLimit, vDiscountPercentage:=vDiscountPercentage, vSettlementPeriod:=vSettlementPeriod, vBankName:=vBankName, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vAccountKey:=vAccountKey, vNominalAccountID:=vNominalAccountID, vAccountStatusID:=vAccountStatusID, vPartySourceID:=vPartySourceID, vSubBranchID:=vSubBranchID, vProofListReportID:=vProofListReportID, vBordereauReportID:=vBordereauReportID, vAllowElectronicPayment:=vAllowElectronicPayment, vMoneyCalcAccType:=vMoneyCalcAccType, vClientBankAccType:=vClientBankAccType), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oaccount


            If Not Informations.IsNothing(vAccountID) Then
                If .AccountID <> vAccountID Then
                    .AccountID = vAccountID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPurgefrequencyID) Then
                If .PurgefrequencyID <> vPurgefrequencyID Then
                    .PurgefrequencyID = vPurgefrequencyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCurrencyID) Then
                If .CurrencyID <> vCurrencyID Then
                    .CurrencyID = vCurrencyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAccounttypeID) Then
                If .AccounttypeID <> vAccounttypeID Then
                    .AccounttypeID = vAccounttypeID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vLedgerId) Then
                If .LedgerID <> vLedgerId Then
                    .LedgerID = vLedgerId
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPaymenttypeID) Then
                If .PaymenttypeID <> vPaymenttypeID Then
                    .PaymenttypeID = vPaymenttypeID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAccountName) Then
                If .AccountName.Trim() <> vAccountName.Trim() Then
                    .AccountName = vAccountName
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vShortCode) Then
                If .ShortCode.Trim() <> vShortCode.Trim() Then
                    .ShortCode = vShortCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vRestrictEnquiry) Then
                If .RestrictEnquiry <> vRestrictEnquiry Then
                    .RestrictEnquiry = vRestrictEnquiry
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vRestrictUpdate) Then
                If .RestrictUpdate <> vRestrictUpdate Then
                    .RestrictUpdate = vRestrictUpdate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDeleteAtPurge) Then
                If .DeleteAtPurge <> vDeleteAtPurge Then
                    .DeleteAtPurge = vDeleteAtPurge
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vContactName) Then
                If .ContactName.Trim() <> vContactName.Trim() Then
                    .ContactName = vContactName
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAddress1) Then
                If .Address1.Trim() <> vAddress1.Trim() Then
                    .Address1 = vAddress1
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAddress2) Then
                If .Address2.Trim() <> vAddress2.Trim() Then
                    .Address2 = vAddress2
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAddress3) Then
                If .Address3.Trim() <> vAddress3.Trim() Then
                    .Address3 = vAddress3
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAddress4) Then
                If .Address4.Trim() <> vAddress4.Trim() Then
                    .Address4 = vAddress4
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPostalCode) Then
                If .PostalCode.Trim() <> vPostalCode.Trim() Then
                    .PostalCode = vPostalCode
                    bDataChanged = True
                End If
            End If


            'Developer Guide No 258
            If Not Informations.IsNothing(vAddressCountry) AndAlso Not Convert.IsDBNull(vAddressCountry) Then
                If .AddressCountry <> vAddressCountry Then
                    .AddressCountry = vAddressCountry
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPhoneAreaCode) Then
                If .PhoneAreaCode.Trim() <> vPhoneAreaCode.Trim() Then
                    .PhoneAreaCode = vPhoneAreaCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPhoneNumber) Then
                If .PhoneNumber.Trim() <> vPhoneNumber.Trim() Then
                    .PhoneNumber = vPhoneNumber
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPhoneExtension) Then
                If .PhoneExtension.Trim() <> vPhoneExtension.Trim() Then
                    .PhoneExtension = vPhoneExtension
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFaxAreaCode) Then
                If .FaxAreaCode.Trim() <> vFaxAreaCode.Trim() Then
                    .FaxAreaCode = vFaxAreaCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFaxNumber) Then
                If .FaxNumber.Trim() <> vFaxNumber.Trim() Then
                    .FaxNumber = vFaxNumber
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFaxExtension) Then
                If .FaxExtension.Trim() <> vFaxExtension.Trim() Then
                    .FaxExtension = vFaxExtension
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPaymentName) Then
                If .PaymentName.Trim() <> vPaymentName.Trim() Then
                    .PaymentName = vPaymentName
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPaymentAccountCode) Then
                If .PaymentAccountCode.Trim() <> vPaymentAccountCode.Trim() Then
                    .PaymentAccountCode = vPaymentAccountCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPaymentBranchCode) Then
                If .PaymentBranchCode.Trim() <> vPaymentBranchCode.Trim() Then
                    .PaymentBranchCode = vPaymentBranchCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPaymentExpiryDate) Then

                If .PaymentExpiryDate <> CDate(vPaymentExpiryDate) Then

                    .PaymentExpiryDate = CDate(vPaymentExpiryDate)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPaymentReference1) Then
                If .PaymentReference1.Trim() <> vPaymentReference1.Trim() Then
                    .PaymentReference1 = vPaymentReference1
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPaymentReference2) Then
                If .PaymentReference2.Trim() <> vPaymentReference2.Trim() Then
                    .PaymentReference2 = vPaymentReference2
                    bDataChanged = True
                End If
            End If

            ' RAW 17/12/2002 : PS187 : added

            If Not Informations.IsNothing(vProofListReportID) Then
                If .ProofListReportID <> vProofListReportID Then
                    .ProofListReportID = vProofListReportID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBordereauReportID) Then
                If .BordereauReportID <> vBordereauReportID Then
                    .BordereauReportID = vBordereauReportID
                    bDataChanged = True
                End If
            End If
            ' RAW 17/12/2002 : PS187 : end


            If Not Informations.IsNothing(vCreditLimit) Then
                If .CreditLimit <> vCreditLimit Then
                    .CreditLimit = vCreditLimit
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDiscountPercentage) Then
                If .DiscountPercentage <> vDiscountPercentage Then
                    .DiscountPercentage = vDiscountPercentage
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vSettlementPeriod) Then
                If .SettlementPeriod <> vSettlementPeriod Then
                    .SettlementPeriod = vSettlementPeriod
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankName) Then
                If .BankName.Trim() <> vBankName.Trim() Then
                    .BankName = vBankName
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankAddress1) Then
                If .BankAddress1.Trim() <> vBankAddress1.Trim() Then
                    .BankAddress1 = vBankAddress1
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankAddress2) Then
                If .BankAddress2.Trim() <> vBankAddress2.Trim() Then
                    .BankAddress2 = vBankAddress2
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankAddress3) Then
                If .BankAddress3.Trim() <> vBankAddress3.Trim() Then
                    .BankAddress3 = vBankAddress3
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankAddress4) Then
                If .BankAddress4.Trim() <> vBankAddress4.Trim() Then
                    .BankAddress4 = vBankAddress4
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankPostalCode) Then
                If .BankPostalCode.Trim() <> vBankPostalCode.Trim() Then
                    .BankPostalCode = vBankPostalCode
                    bDataChanged = True
                End If
            End If


            'If Not Informations.IsNothing(vBankCountry)  Then
            If Not Informations.IsNothing(vBankCountry) AndAlso Not (vBankCountry Is DBNull.Value) Then
                If .BankCountry <> vBankCountry Then
                    .BankCountry = vBankCountry
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankPhoneAreaCode) AndAlso Not (vBankPhoneAreaCode Is DBNull.Value) Then
                If .BankPhoneAreaCode.Trim() <> vBankPhoneAreaCode.Trim() Then
                    .BankPhoneAreaCode = vBankPhoneAreaCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankPhoneNumber) Then
                If .BankPhoneNumber.Trim() <> vBankPhoneNumber.Trim() Then
                    .BankPhoneNumber = vBankPhoneNumber
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankPhoneExtension) Then
                If .BankPhoneExtension.Trim() <> vBankPhoneExtension.Trim() Then
                    .BankPhoneExtension = vBankPhoneExtension
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankFaxAreaCode) Then
                If .BankFaxAreaCode.Trim() <> vBankFaxAreaCode.Trim() Then
                    .BankFaxAreaCode = vBankFaxAreaCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankFaxNumber) Then
                If .BankFaxNumber.Trim() <> vBankFaxNumber.Trim() Then
                    .BankFaxNumber = vBankFaxNumber
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankFaxExtension) Then
                If .BankFaxExtension.Trim() <> vBankFaxExtension.Trim() Then
                    .BankFaxExtension = vBankFaxExtension
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vComments) Then
                If .Comments.Trim() <> vComments.Trim() Then
                    .Comments = vComments
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAccountKey) Then
                If .AccountKey <> vAccountKey Then
                    .AccountKey = vAccountKey
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vNominalAccountID) AndAlso (Not vNominalAccountID Is DBNull.Value) Then
                If .NominalAccountID <> vNominalAccountID Then
                    .NominalAccountID = vNominalAccountID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAccountStatusID) Then
                If .AccountStatusID <> vAccountStatusID Then
                    'SD 09/01/2003
                    'If the account was initially closed (ID=2 hardcoded) and now
                    ' is Active (ID =1) then it is being taken off hold
                    If vAccountStatusID = ACAccountActiveID And .AccountStatusID = ACAccountClosedID Then
                        .IsTakenOffHold = True
                    End If
                    .AccountStatusID = vAccountStatusID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vPartySourceID) Then
                'Changes done as per VB code
                'If CStr(.CompanyID).Trim() <> vPartySourceID.Trim() Then
                If CStr(.CompanyID).Trim() <> CStr(vPartySourceID).Trim() Then
                    .CompanyID = CInt(vPartySourceID)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vSubBranchID) AndAlso (Not vSubBranchID Is DBNull.Value) Then
                If .SubBranchID <> vSubBranchID Then
                    .SubBranchID = vSubBranchID
                    bDataChanged = True
                End If
            End If

            ' RDC 12112003

            If Not Informations.IsNothing(vAllowElectronicPayment) Then
                If .AllowElectronicPayment <> vAllowElectronicPayment Then
                    .AllowElectronicPayment = vAllowElectronicPayment
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vMoneyCalcAccType) Then
                If .MoneyCalcAccType <> vMoneyCalcAccType Then
                    .MoneyCalcAccType = vMoneyCalcAccType
                    bDataChanged = True
                End If
            End If

            'Changes done as per VB Code
            'If Not False Then
            '    .ClientBankAccType = vClientBankAccType
            '    bDataChanged = True
            'End If

            'If Not False Then
            '    .MerchantId = vMerchantId
            '    bDataChanged = True
            'End If
            If Not Informations.IsNothing(vClientBankAccType) Then
                .ClientBankAccType = vClientBankAccType
                bDataChanged = True
            End If

            If Not Informations.IsNothing(vMerchantId) Then
                .MerchantId = vMerchantId
                bDataChanged = True
            End If


            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied Account property values.
    '
    ' ***************************************************************** '
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' RDC 12112003

    Private Function GetProperties(ByRef oaccount As bACTAccount.Account,
                                   ByRef iStatus As Integer,
                                   Optional ByRef vAccountID As Object = Nothing,
                                   Optional ByRef vPurgefrequencyID As Object = Nothing,
                                   Optional ByRef vCurrencyID As Object = Nothing,
                                   Optional ByRef vAccounttypeID As Object = Nothing,
                                   Optional ByRef vLedgerId As Object = Nothing,
                                   Optional ByRef vPaymenttypeID As Object = Nothing,
                                   Optional ByRef vAccountName As Object = Nothing,
                                   Optional ByRef vShortCode As Object = Nothing,
                                   Optional ByRef vRestrictEnquiry As Object = Nothing,
                                   Optional ByRef vRestrictUpdate As Object = Nothing,
                                   Optional ByRef vDeleteAtPurge As Object = Nothing,
                                   Optional ByRef vContactName As Object = Nothing,
                                   Optional ByRef vAddress1 As Object = Nothing,
                                   Optional ByRef vAddress2 As Object = Nothing,
                                   Optional ByRef vAddress3 As Object = Nothing,
                                   Optional ByRef vAddress4 As Object = Nothing,
                                   Optional ByRef vPostalCode As Object = Nothing,
                                   Optional ByRef vAddressCountry As Object = Nothing,
                                   Optional ByRef vPhoneAreaCode As Object = Nothing,
                                   Optional ByRef vPhoneNumber As Object = Nothing,
                                   Optional ByRef vPhoneExtension As Object = Nothing,
                                   Optional ByRef vFaxAreaCode As Object = Nothing,
                                   Optional ByRef vFaxNumber As Object = Nothing,
                                   Optional ByRef vFaxExtension As Object = Nothing,
                                   Optional ByRef vPaymentName As Object = Nothing,
                                   Optional ByRef vPaymentAccountCode As Object = Nothing,
                                   Optional ByRef vPaymentBranchCode As Object = Nothing,
                                   Optional ByRef vPaymentExpiryDate As Object = Nothing,
                                   Optional ByRef vPaymentReference1 As Object = Nothing,
                                   Optional ByRef vPaymentReference2 As Object = Nothing,
                                   Optional ByRef vCreditLimit As Object = Nothing,
                                   Optional ByRef vDiscountPercentage As Object = Nothing,
                                   Optional ByRef vSettlementPeriod As Object = Nothing,
                                   Optional ByRef vBankName As Object = Nothing,
                                   Optional ByRef vBankAddress1 As Object = Nothing,
                                   Optional ByRef vBankAddress2 As Object = Nothing,
                                   Optional ByRef vBankAddress3 As Object = Nothing,
                                   Optional ByRef vBankAddress4 As Object = Nothing,
                                   Optional ByRef vBankPostalCode As Object = Nothing,
                                   Optional ByRef vBankCountry As Object = Nothing,
                                   Optional ByRef vBankPhoneAreaCode As Object = Nothing,
                                   Optional ByRef vBankPhoneNumber As Object = Nothing,
                                   Optional ByRef vBankPhoneExtension As Object = Nothing,
                                   Optional ByRef vBankFaxAreaCode As Object = Nothing,
                                   Optional ByRef vBankFaxNumber As Object = Nothing,
                                   Optional ByRef vBankFaxExtension As Object = Nothing,
                                   Optional ByRef vComments As Object = Nothing,
                                   Optional ByRef vAccountKey As Object = Nothing,
                                   Optional ByRef vNominalAccountID As Object = Nothing,
                                   Optional ByRef vAccountStatusID As Object = Nothing,
                                   Optional ByRef vPartySourceID As Object = Nothing,
                                   Optional ByRef vSubBranchID As Object = Nothing,
                                   Optional ByRef vProofListReportID As Object = Nothing,
                                   Optional ByRef vBordereauReportID As Object = Nothing,
                                   Optional ByRef vAllowElectronicPayment As Object = Nothing,
                                   Optional ByRef vMoneyCalcAccType As Object = Nothing,
                                   Optional ByRef vParamArray As Object = Nothing,
                                   Optional ByRef vCompanyId As Integer = 0,
                                   Optional ByRef r_sBIC As String = "",
                                   Optional ByRef r_sIBAN As String = "") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With oaccount
            vAccountID = .AccountID
            vPurgefrequencyID = .PurgefrequencyID
            vCurrencyID = .CurrencyID
            vAccounttypeID = .AccounttypeID
            vLedgerId = .LedgerID
            vPaymenttypeID = .PaymenttypeID
            vAccountName = .AccountName
            vShortCode = .ShortCode
            vRestrictEnquiry = .RestrictEnquiry
            vRestrictUpdate = .RestrictUpdate
            vDeleteAtPurge = .DeleteAtPurge
            vContactName = .ContactName
            vAddress1 = .Address1
            vAddress2 = .Address2
            vAddress3 = .Address3
            vAddress4 = .Address4
            vPostalCode = .PostalCode
            vAddressCountry = .AddressCountry
            vPhoneAreaCode = .PhoneAreaCode
            vPhoneNumber = .PhoneNumber
            vPhoneExtension = .PhoneExtension
            vFaxAreaCode = .FaxAreaCode
            vFaxNumber = .FaxNumber
            vFaxExtension = .FaxExtension
            vPaymentName = .PaymentName
            vPaymentAccountCode = .PaymentAccountCode
            vPaymentBranchCode = .PaymentBranchCode
            vPaymentExpiryDate = .PaymentExpiryDate
            vPaymentReference1 = .PaymentReference1
            vPaymentReference2 = .PaymentReference2
            vProofListReportID = .ProofListReportID
            vBordereauReportID = .BordereauReportID
            vCreditLimit = .CreditLimit
            vDiscountPercentage = .DiscountPercentage
            vSettlementPeriod = .SettlementPeriod
            vBankName = .BankName
            vBankAddress1 = .BankAddress1
            vBankAddress2 = .BankAddress2
            vBankAddress3 = .BankAddress3
            vBankAddress4 = .BankAddress4
            vBankPostalCode = .BankPostalCode
            vBankCountry = .BankCountry
            vBankPhoneAreaCode = .BankPhoneAreaCode
            vBankPhoneNumber = .BankPhoneNumber
            vBankPhoneExtension = .BankPhoneExtension
            vBankFaxAreaCode = .BankFaxAreaCode
            vBankFaxNumber = .BankFaxNumber
            vBankFaxExtension = .BankFaxExtension
            vComments = .Comments
            vAccountKey = .AccountKey
            vNominalAccountID = .NominalAccountID
            vAccountStatusID = .AccountStatusID
            vPartySourceID = .CompanyID
            vSubBranchID = .SubBranchID
            vAllowElectronicPayment = .AllowElectronicPayment
            vMoneyCalcAccType = .MoneyCalcAccType
            vParamArray(ACClientBankAccTypeArrPos) = .ClientBankAccType
            vParamArray(ACMerchantIdArrPos) = .MerchantId
            iStatus = .DatabaseStatus
            vCompanyId = .CompanyID
            r_sBIC = .BIC
            r_sIBAN = .IBAN
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oaccount As bACTAccount.Account) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            'eck110400
            'Multi Branch
            If oaccount.CompanyID < 1 Then
                If m_iPartySourceId > 0 Then
                    m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(m_iPartySourceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                Else
                    m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(m_iCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                End If
            Else
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(oaccount.CompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oaccount.PurgefrequencyID < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="purgefrequency_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="purgefrequency_id", vValue:=CStr(oaccount.PurgefrequencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oaccount.AccounttypeID < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="accounttype_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="accounttype_id", vValue:=CStr(oaccount.AccounttypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oaccount.PaymenttypeID < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="paymenttype_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="paymenttype_id", vValue:=CStr(oaccount.PaymenttypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oaccount.CurrencyID < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(oaccount.CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oaccount.LedgerID < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="ledger_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="ledger_id", vValue:=CStr(oaccount.LedgerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="account_name", vValue:=oaccount.AccountName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="short_code", vValue:=oaccount.ShortCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF - 191200 Changed to PMBoolean
            'Developer Guide No 101
            m_lReturn = .Parameters.Add(sName:="restrict_enquiry", vValue:=oaccount.RestrictEnquiry, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF - 191200 Changed to PMBoolean
            'Developer Guide No 101
            m_lReturn = .Parameters.Add(sName:="restrict_update", vValue:=oaccount.RestrictUpdate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF - 191200 Changed to PMBoolean
            'Developer Guide No 101
            m_lReturn = .Parameters.Add(sName:="delete_at_purge", vValue:=oaccount.DeleteAtPurge, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="contact_name", vValue:=oaccount.ContactName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address1", vValue:=oaccount.Address1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address2", vValue:=oaccount.Address2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address3", vValue:=oaccount.Address3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address4", vValue:=oaccount.Address4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="postal_code", vValue:=oaccount.PostalCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oaccount.AddressCountry < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="address_country", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="address_country", vValue:=CStr(oaccount.AddressCountry), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="phone_area_code", vValue:=oaccount.PhoneAreaCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="phone_number", vValue:=oaccount.PhoneNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="phone_extension", vValue:=oaccount.PhoneExtension, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fax_area_code", vValue:=oaccount.FaxAreaCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fax_number", vValue:=oaccount.FaxNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fax_extension", vValue:=oaccount.FaxExtension, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_name", vValue:=oaccount.PaymentName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_account_code", vValue:=oaccount.PaymentAccountCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_branch_code", vValue:=oaccount.PaymentBranchCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide no. 40
            m_lReturn = .Parameters.Add(sName:="payment_expiry_date", vValue:=oaccount.PaymentExpiryDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_reference1", vValue:=oaccount.PaymentReference1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_reference2", vValue:=oaccount.PaymentReference2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 17/12/2002 : PS187 : Added
            If oaccount.ProofListReportID < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="prooflist_report_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="prooflist_report_id", vValue:=CStr(oaccount.ProofListReportID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oaccount.BordereauReportID < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="bordereau_report_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="bordereau_report_id", vValue:=CStr(oaccount.BordereauReportID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RAW 17/12/2002 : PS187 : Added

            ' CTAF - 191200 Changed to PMDouble
            m_lReturn = .Parameters.Add(sName:="credit_limit", vValue:=CStr(oaccount.CreditLimit), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF - 191200 Changed to PMDouble
            m_lReturn = .Parameters.Add(sName:="discount_percentage", vValue:=CStr(oaccount.DiscountPercentage), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="settlement_period", vValue:=CStr(oaccount.SettlementPeriod), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_name", vValue:=oaccount.BankName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_address1", vValue:=oaccount.BankAddress1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_address2", vValue:=oaccount.BankAddress2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_address3", vValue:=oaccount.BankAddress3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_address4", vValue:=oaccount.BankAddress4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_postal_code", vValue:=oaccount.BankPostalCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oaccount.BankCountry < 1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="bank_country", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="bank_country", vValue:=CStr(oaccount.BankCountry), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_phone_area_code", vValue:=oaccount.BankPhoneAreaCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_phone_number", vValue:=oaccount.BankPhoneNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_phone_extension", vValue:=oaccount.BankPhoneExtension, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_fax_area_code", vValue:=oaccount.BankFaxAreaCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_fax_number", vValue:=oaccount.BankFaxNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_fax_extension", vValue:=oaccount.BankFaxExtension, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="comments", vValue:=oaccount.Comments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="account_key", vValue:=CStr(oaccount.AccountKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="nominal_account_id", vValue:=CStr(oaccount.NominalAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="accountstatus_id", vValue:=CStr(oaccount.AccountStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="sub_branch_id", vValue:=CStr(oaccount.SubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RDC 12112003
            If oaccount.AllowElectronicPayment Then
                m_lReturn = .Parameters.Add(sName:="allow_electronic_payment", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="allow_electronic_payment", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="client_money_calc_account_type", vValue:=CStr(oaccount.MoneyCalcAccType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If CDbl(oaccount.ClientBankAccType) = -1 Then

                'developer guide no 85. 
                m_lReturn = .Parameters.Add(sName:="client_bank_account_type", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="client_bank_account_type", vValue:=oaccount.ClientBankAccType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="merchant_id", vValue:=oaccount.MerchantId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Account.
    '
    ' eck010900 Add Company as a modifyable property
    ' ***************************************************************** '
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' RDC 12112003 added vAllowElectronicPayment
    'developer guide no. Changes done as per VB Code
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPurgefrequencyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vAccounttypeID As Object = Nothing, Optional ByRef vLedgerId As Object = Nothing, Optional ByRef vPaymenttypeID As Object = Nothing, Optional ByRef vAccountName As Object = Nothing, Optional ByRef vShortCode As Object = Nothing, Optional ByRef vRestrictEnquiry As Object = Nothing, Optional ByRef vRestrictUpdate As Object = Nothing, Optional ByRef vDeleteAtPurge As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vAddressCountry As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vPaymentName As Object = Nothing, Optional ByRef vPaymentAccountCode As Object = Nothing, Optional ByRef vPaymentBranchCode As Object = Nothing, Optional ByRef vPaymentExpiryDate As Object = Nothing, Optional ByRef vPaymentReference1 As Object = Nothing, Optional ByRef vPaymentReference2 As Object = Nothing, Optional ByRef vCreditLimit As Object = Nothing, Optional ByRef vDiscountPercentage As Object = Nothing, Optional ByRef vSettlementPeriod As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vNominalAccountID As Object = Nothing, Optional ByRef vAccountStatusID As Object = Nothing, Optional ByRef vPartySourceID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vProofListReportID As Object = Nothing, Optional ByRef vBordereauReportID As Object = Nothing, Optional ByRef vAllowElectronicPayment As Object = Nothing, Optional ByRef vMoneyCalcAccType As Object = Nothing, Optional ByRef vClientBankAccType As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' CF 211099
        ' Get the base currency
        m_lReturn = CType(GetBaseCurrency(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        If (Informations.IsNothing(vAccountID)) OrElse (vAccountID.Equals(0)) Or (bDefaultAll) Then
            vAccountID = 0
        End If



        If (Informations.IsNothing(vPurgefrequencyID)) OrElse (vPurgefrequencyID.Equals(0)) Or (bDefaultAll) Then
            vPurgefrequencyID = 0
        End If



        If (Informations.IsNothing(vCurrencyID)) OrElse (vCurrencyID.Equals(0)) Or (bDefaultAll) Or (vCurrencyID = 0) Then
            vCurrencyID = m_iBaseCurrencyId
        End If



        If (Informations.IsNothing(vAccounttypeID)) OrElse (Object.Equals(vAccounttypeID, Nothing)) Or (bDefaultAll) Then


            vAccounttypeID = DBNull.Value
        End If



        If (Informations.IsNothing(vLedgerId)) OrElse (Object.Equals(vLedgerId, Nothing)) Or (bDefaultAll) Then


            vLedgerId = DBNull.Value
        End If



        If (Informations.IsNothing(vPaymenttypeID)) OrElse (vPaymenttypeID.Equals(0)) Or (bDefaultAll) Then
            vPaymenttypeID = gACTLibrary.ACTPaymentTypeCheque
        End If



        If (Informations.IsNothing(vAccountName)) OrElse (String.IsNullOrEmpty(vAccountName)) Or (bDefaultAll) Then
            vAccountName = ""
        End If



        If (Informations.IsNothing(vShortCode)) OrElse (String.IsNullOrEmpty(vShortCode)) Or (bDefaultAll) Then
            vShortCode = ""
        End If



        If (Informations.IsNothing(vRestrictEnquiry)) OrElse (vRestrictEnquiry.Equals(False)) Or (bDefaultAll) Then
            vRestrictEnquiry = False
        End If



        If (Informations.IsNothing(vRestrictUpdate)) OrElse (vRestrictUpdate.Equals(False)) Or (bDefaultAll) Then
            vRestrictUpdate = False
        End If



        If (Informations.IsNothing(vDeleteAtPurge)) OrElse (vDeleteAtPurge.Equals(False)) Or (bDefaultAll) Then
            vDeleteAtPurge = False
        End If



        If (Informations.IsNothing(vContactName)) OrElse (String.IsNullOrEmpty(vContactName)) Or (bDefaultAll) Then
            vContactName = ""
        End If



        If (Informations.IsNothing(vAddress1)) OrElse (String.IsNullOrEmpty(vAddress1)) Or (bDefaultAll) Then
            vAddress1 = ""
        End If



        If (Informations.IsNothing(vAddress2)) OrElse (String.IsNullOrEmpty(vAddress2)) Or (bDefaultAll) Then
            vAddress2 = ""
        End If



        If (Informations.IsNothing(vAddress3)) OrElse (String.IsNullOrEmpty(vAddress3)) Or (bDefaultAll) Then
            vAddress3 = ""
        End If



        If (Informations.IsNothing(vAddress4)) OrElse (String.IsNullOrEmpty(vAddress4)) Or (bDefaultAll) Then
            vAddress4 = ""
        End If



        If (Informations.IsNothing(vPostalCode)) OrElse (String.IsNullOrEmpty(vPostalCode)) Or (bDefaultAll) Then
            vPostalCode = ""
        End If



        If (Informations.IsNothing(vAddressCountry)) OrElse (Object.Equals(vAddressCountry, Nothing)) Or (bDefaultAll) Then


            vAddressCountry = DBNull.Value
        End If



        If (Informations.IsNothing(vPhoneAreaCode)) OrElse (String.IsNullOrEmpty(vPhoneAreaCode)) Or (bDefaultAll) Then
            vPhoneAreaCode = ""
        End If



        If (Informations.IsNothing(vPhoneNumber)) OrElse (String.IsNullOrEmpty(vPhoneNumber)) Or (bDefaultAll) Then
            vPhoneNumber = ""
        End If



        If (Informations.IsNothing(vPhoneExtension)) OrElse (String.IsNullOrEmpty(vPhoneExtension)) Or (bDefaultAll) Then
            vPhoneExtension = ""
        End If



        If (Informations.IsNothing(vFaxAreaCode)) OrElse (String.IsNullOrEmpty(vFaxAreaCode)) Or (bDefaultAll) Then
            vFaxAreaCode = ""
        End If



        If (Informations.IsNothing(vFaxNumber)) OrElse (String.IsNullOrEmpty(vFaxNumber)) Or (bDefaultAll) Then
            vFaxNumber = ""
        End If



        If (Informations.IsNothing(vFaxExtension)) OrElse (String.IsNullOrEmpty(vFaxExtension)) Or (bDefaultAll) Then
            vFaxExtension = ""
        End If



        If (Informations.IsNothing(vPaymentName)) OrElse (String.IsNullOrEmpty(vPaymentName)) Or (bDefaultAll) Then
            vPaymentName = ""
        End If



        If (Informations.IsNothing(vPaymentAccountCode)) OrElse (String.IsNullOrEmpty(vPaymentAccountCode)) Or (bDefaultAll) Then
            vPaymentAccountCode = ""
        End If



        If (Informations.IsNothing(vPaymentBranchCode)) Or (String.IsNullOrEmpty(vPaymentBranchCode)) Or (bDefaultAll) Then
            vPaymentBranchCode = ""
        End If



        'developer Guide NO 115
        If (Informations.IsNothing(vPaymentExpiryDate)) OrElse (vPaymentExpiryDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vPaymentExpiryDate = DateTime.Now
        End If



        If (Informations.IsNothing(vPaymentReference1)) Or (String.IsNullOrEmpty(vPaymentReference1)) Or (bDefaultAll) Then
            vPaymentReference1 = ""
        End If



        If (Informations.IsNothing(vPaymentReference2)) Or (String.IsNullOrEmpty(vPaymentReference2)) Or (bDefaultAll) Then
            vPaymentReference2 = ""
        End If

        ' RAW 17/12/2002 : PS187 : Added


        If (Informations.IsNothing(vProofListReportID)) OrElse (vProofListReportID.Equals(0)) Or (bDefaultAll) Then
            vProofListReportID = 0
        End If



        If (Informations.IsNothing(vBordereauReportID)) OrElse (vBordereauReportID.Equals(0)) Or (bDefaultAll) Then
            vBordereauReportID = 0
        End If
        ' RAW 17/12/2002 : PS187 : Added



        If (Informations.IsNothing(vCreditLimit)) OrElse (vCreditLimit.Equals(0)) Or (bDefaultAll) Then
            vCreditLimit = 0
        End If



        If (Informations.IsNothing(vDiscountPercentage)) OrElse (vDiscountPercentage.Equals(0)) Or (bDefaultAll) Then
            vDiscountPercentage = 0
        End If



        If (Informations.IsNothing(vSettlementPeriod)) OrElse (vSettlementPeriod.Equals(0)) Or (bDefaultAll) Then
            vSettlementPeriod = 0
        End If



        If (Informations.IsNothing(vBankName)) OrElse (String.IsNullOrEmpty(vBankName)) Or (bDefaultAll) Then
            vBankName = ""
        End If



        If (Informations.IsNothing(vBankAddress1)) Or (String.IsNullOrEmpty(vBankAddress1)) Or (bDefaultAll) Then
            vBankAddress1 = ""
        End If



        If (Informations.IsNothing(vBankAddress2)) Or (String.IsNullOrEmpty(vBankAddress2)) Or (bDefaultAll) Then
            vBankAddress2 = ""
        End If



        If (Informations.IsNothing(vBankAddress3)) Or (String.IsNullOrEmpty(vBankAddress3)) Or (bDefaultAll) Then
            vBankAddress3 = ""
        End If



        If (Informations.IsNothing(vBankAddress4)) Or (String.IsNullOrEmpty(vBankAddress4)) Or (bDefaultAll) Then
            vBankAddress4 = ""
        End If



        If (Informations.IsNothing(vBankPostalCode)) Or (String.IsNullOrEmpty(vBankPostalCode)) Or (bDefaultAll) Then
            vBankPostalCode = ""
        End If



        If (Informations.IsNothing(vBankCountry)) OrElse (Object.Equals(vBankCountry, Nothing)) Or (bDefaultAll) Then


            vBankCountry = DBNull.Value
        End If



        If (Informations.IsNothing(vBankPhoneAreaCode)) Or (String.IsNullOrEmpty(vBankPhoneAreaCode)) Or (bDefaultAll) Then
            vBankPhoneAreaCode = ""
        End If



        If (Informations.IsNothing(vBankPhoneNumber)) Or (String.IsNullOrEmpty(vBankPhoneNumber)) Or (bDefaultAll) Then
            vBankPhoneNumber = ""
        End If



        If (Informations.IsNothing(vBankPhoneExtension)) Or (String.IsNullOrEmpty(vBankPhoneExtension)) Or (bDefaultAll) Then
            vBankPhoneExtension = ""
        End If



        If (Informations.IsNothing(vBankFaxAreaCode)) Or (String.IsNullOrEmpty(vBankFaxAreaCode)) Or (bDefaultAll) Then
            vBankFaxAreaCode = ""
        End If



        If (Informations.IsNothing(vBankFaxNumber)) Or (String.IsNullOrEmpty(vBankFaxNumber)) Or (bDefaultAll) Then
            vBankFaxNumber = ""
        End If



        If (Informations.IsNothing(vBankFaxExtension)) Or (String.IsNullOrEmpty(vBankFaxExtension)) Or (bDefaultAll) Then
            vBankFaxExtension = ""
        End If



        If (Informations.IsNothing(vComments)) Or (String.IsNullOrEmpty(vComments)) Or (bDefaultAll) Then
            vComments = ""
        End If



        If (Informations.IsNothing(vAccountKey)) OrElse (vAccountKey.Equals(0)) Or (bDefaultAll) Then
            vAccountKey = 0
        End If



        If (Informations.IsNothing(vNominalAccountID)) OrElse (Object.Equals(vNominalAccountID, Nothing)) Or (bDefaultAll) Then


            vNominalAccountID = DBNull.Value
        End If



        If (Informations.IsNothing(vAccountStatusID)) OrElse (vAccountStatusID.Equals(0)) Or (bDefaultAll) Then
            vAccountStatusID = 1
        End If
        'eck010900


        If (Informations.IsNothing(vPartySourceID)) OrElse (vPartySourceID.Equals(0)) Or (bDefaultAll) Then
            vPartySourceID = m_iCompanyID
        End If



        If (Informations.IsNothing(vSubBranchID)) OrElse (Object.Equals(vSubBranchID, Nothing)) Or (bDefaultAll) Then


            vSubBranchID = DBNull.Value
        End If

        ' RDC 12112003


        If (Informations.IsNothing(vAllowElectronicPayment)) OrElse (vAllowElectronicPayment.Equals(False)) Or (bDefaultAll) Then
            vAllowElectronicPayment = False
        End If



        If (Informations.IsNothing(vMoneyCalcAccType)) OrElse (vMoneyCalcAccType.Equals(0)) Or (bDefaultAll) Then
            vMoneyCalcAccType = 0
        End If



        'Developer Guide No. 151
        If (Informations.IsNothing(vClientBankAccType)) OrElse (vClientBankAccType.Equals(0)) OrElse (bDefaultAll) Then
            vClientBankAccType = -1
        End If
        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Account.
    '
    ' eck010900 Add Company as a modifyable property
    ' ***************************************************************** '
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' RDC 12112003 added vAllowElectronicPayment
    Private Function CheckMandatory(Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPurgefrequencyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vAccounttypeID As Object = Nothing, Optional ByRef vLedgerId As Object = Nothing, Optional ByRef vPaymenttypeID As Object = Nothing, Optional ByRef vAccountName As Object = Nothing, Optional ByRef vShortCode As Object = Nothing, Optional ByRef vRestrictEnquiry As Object = Nothing, Optional ByRef vRestrictUpdate As Object = Nothing, Optional ByRef vDeleteAtPurge As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vAddressCountry As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vPaymentName As Object = Nothing, Optional ByRef vPaymentAccountCode As Object = Nothing, Optional ByRef vPaymentBranchCode As Object = Nothing, Optional ByRef vPaymentExpiryDate As Object = Nothing, Optional ByRef vPaymentReference1 As Object = Nothing, Optional ByRef vPaymentReference2 As Object = Nothing, Optional ByRef vCreditLimit As Object = Nothing, Optional ByRef vDiscountPercentage As Object = Nothing, Optional ByRef vSettlementPeriod As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vNominalAccountID As Object = Nothing, Optional ByRef vAccountStatusID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vProofListReportID As Object = Nothing, Optional ByRef vBordereauReportID As Object = Nothing, Optional ByRef vAllowElectronicPayment As Object = Nothing, Optional ByRef vMoneyCalcAccType As Object = Nothing, Optional ByRef vClientBankAccType As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vAccountID)) Or (Object.Equals(vAccountID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vPurgefrequencyID)) Or (Object.Equals(vPurgefrequencyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCurrencyID)) Or (Object.Equals(vCurrencyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAccounttypeID)) Or (Object.Equals(vAccounttypeID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vLedgerId)) Or (Object.Equals(vLedgerId, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAccountName)) Or (Object.Equals(vAccountName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAccountStatusID)) Or (Object.Equals(vAccountStatusID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateAccountForCompany (Public)
    '
    ' Description: For use in multi-structure tree accounting
    '              Takes an account id and a company id, gets the details of the account
    '              Creates a new account with the same basic details
    '              replacing the company id with that supplied (target company), and
    '              determining the coresponding ledger  in the target company
    '              Returns the id of the newly created account record
    '              ECK 171103 PN6169
    ' ***************************************************************** '

    Public Function CreateAccountForCompany(ByRef vAccountID As Integer, ByRef vAccountName As String, ByRef vShortName As String, ByRef vLedgerId As Integer, ByRef vMappingId As Integer, ByVal vCompanyId As String) As Integer

        Dim result As Integer = 0
        Dim oaccount As bACTAccount.Account
        Dim l_sLedgerCode As String = ""
        Dim l_vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetDetails(vAccountID:=vAccountID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lCurrentRecord = 1 'we are only interested in the first record

            oaccount = m_oAccounts.Item(m_lCurrentRecord) 'copy original account details
            vLedgerId = m_oAccounts.Item(m_lCurrentRecord).LedgerID

            m_lReturn = CType(GetAccountLedger(v_lAccountId:=vAccountID, v_lLedgerId:=vLedgerId, v_sLedgerCode:=l_sLedgerCode), gPMConstants.PMEReturnCode)

            'l_lLedgerId now has the ledger for the existing account
            'need to find the equivalent ledger for the posting company

            'use getledger details to pick up all ledgers for the new company


            m_lReturn = CType(GetLedgerDetails(vResultArray:=l_vResultArray), gPMConstants.PMEReturnCode)

            'now find which one matches the code identified earlier


            For iCount As Integer = 0 To l_vResultArray.GetUpperBound(0)

                If CStr(l_vResultArray(iCount, 1)).Trim() = l_sLedgerCode Then

                    vLedgerId = CInt(l_vResultArray(iCount, 0))

                    vMappingId = CInt(l_vResultArray(iCount, 4))

                    Exit For
                End If
            Next iCount

            'if we haven't found a new ledger we will use the old one
            'this probably means something is set up incorrectly - but lets wait and see

            ' update new account record details

            oaccount.CompanyID = CInt(vCompanyId) 'replace company id
            oaccount.LedgerID = vLedgerId
            oaccount.AccountID = 0

            m_lReturn = CType(AddItem(oaccount), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'our new account  has been created - here is the id

            vAccountID = oaccount.AccountID
            vAccountName = oaccount.AccountName
            vShortName = oaccount.ShortCode

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccountForCompany Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccountForCompany", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Account for Consistency.
    '
    ' eck010900 Add Company as a modifyable property
    ' ***************************************************************** '
    'eck060700
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' RDC 12112003 added vAllowElectronicPayment
    Private Function Validate(Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPurgefrequencyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vAccounttypeID As Object = Nothing, Optional ByRef vLedgerId As Object = Nothing, Optional ByRef vPaymenttypeID As Object = Nothing, Optional ByRef vAccountName As Object = Nothing, Optional ByRef vShortCode As Object = Nothing, Optional ByRef vRestrictEnquiry As Object = Nothing, Optional ByRef vRestrictUpdate As Object = Nothing, Optional ByRef vDeleteAtPurge As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vAddressCountry As Object = Nothing, Optional ByRef vPhoneAreaCode As Object = Nothing, Optional ByRef vPhoneNumber As Object = Nothing, Optional ByRef vPhoneExtension As Object = Nothing, Optional ByRef vFaxAreaCode As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vFaxExtension As Object = Nothing, Optional ByRef vPaymentName As Object = Nothing, Optional ByRef vPaymentAccountCode As Object = Nothing, Optional ByRef vPaymentBranchCode As Object = Nothing, Optional ByRef vPaymentExpiryDate As Object = Nothing, Optional ByRef vPaymentReference1 As Object = Nothing, Optional ByRef vPaymentReference2 As Object = Nothing, Optional ByRef vCreditLimit As Object = Nothing, Optional ByRef vDiscountPercentage As Object = Nothing, Optional ByRef vSettlementPeriod As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vNominalAccountID As Object = Nothing, Optional ByRef vAccountStatusID As Object = Nothing, Optional ByRef vPartySourceID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vProofListReportID As Object = Nothing, Optional ByRef vBordereauReportID As Object = Nothing, Optional ByRef vAllowElectronicPayment As Object = Nothing, Optional ByRef vMoneyCalcAccType As Object = Nothing, Optional ByRef vClientBankAccType As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vAccountID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If Not Informations.IsNothing(vPurgefrequencyID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vPurgefrequencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If Not Informations.IsNothing(vAccounttypeID) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vAccounttypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vLedgerId) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vLedgerId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vPaymenttypeID) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vPaymenttypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vRestrictEnquiry) Then

            'If Not Double.TryParse(CStr(vRestrictEnquiry), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
            If Not ToSafeBoolean(vRestrictEnquiry) = False Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If Not Informations.IsNothing(vRestrictUpdate) Then

            'If Not Double.TryParse(CStr(vRestrictUpdate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
            If Not ToSafeBoolean(vRestrictUpdate) = False Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDeleteAtPurge) Then

            'If Not Double.TryParse(CStr(vDeleteAtPurge), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9) Then
            If Not ToSafeBoolean(vDeleteAtPurge) = False Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        'Developer Guide No 258
        If Not Informations.IsNothing(vAddressCountry) AndAlso Not Convert.IsDBNull(vAddressCountry) Then


            Dim dbNumericTemp10 As Double
            If (Not Double.TryParse(Convert.ToString(vAddressCountry), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp10)) And (Not (Convert.IsDBNull(vAddressCountry) Or Informations.IsNothing(vAddressCountry))) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vPaymentExpiryDate) Then
            If Not Informations.IsDate(vPaymentExpiryDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If Not Informations.IsNothing(vCreditLimit) Then

            Dim dbNumericTemp11 As Double
            If Not Double.TryParse(CStr(vCreditLimit), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp11) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If Not Informations.IsNothing(vSettlementPeriod) Then

            Dim dbNumericTemp12 As Double
            If Not Double.TryParse(CStr(vSettlementPeriod), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp12) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vBankCountry) AndAlso Not Informations.IsDBNull(vBankCountry) Then
            vBankCountry = ToSafeString(vBankCountry)
            'If (Not Double.TryParse(CStr(vBankCountry), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp13)) And (Not (Convert.IsDBNull(vBankCountry) Or Informations.IsNothing(vBankCountry))) Then
            If String.IsNullOrEmpty(vBankCountry) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If Not Informations.IsNothing(vAccountStatusID) Then

            Dim dbNumericTemp14 As Double
            If Not Double.TryParse(CStr(vAccountStatusID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp14) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'eck010900

        If Not Informations.IsNothing(vPartySourceID) Then

            Dim dbNumericTemp15 As Double
            If Not Double.TryParse(CStr(vPartySourceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp15) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'DN 22/10/02 - Allow for when sub branch is not passed and hence set to NULL

        If Not Informations.IsNothing(vSubBranchID) AndAlso Not Informations.IsDBNull(vSubBranchID) Then
            'If (Not Double.TryParse(CStr(vSubBranchID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp16)) And (Not (Convert.IsDBNull(vSubBranchID) Or Informations.IsNothing(vSubBranchID))) Then
            If String.IsNullOrEmpty(vSubBranchID) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        ' RAW 17/12/2002 : PS187 : Added

        If Not Informations.IsNothing(vProofListReportID) Then

            Dim dbNumericTemp17 As Double
            If Not Double.TryParse(CStr(vProofListReportID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp17) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vBordereauReportID) Then

            Dim dbNumericTemp18 As Double
            If Not Double.TryParse(CStr(vBordereauReportID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp18) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        ' RAW 17/12/2002 : PS187 : end


        ' {* USER DEFINED CODE (End) *}

        Return result

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



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
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


    'SD 09/01/2003 Start - Use system option for Credit Control changes
    ' ***************************************************************** '
    ' Name: GetSystemOptionValue
    ' Description:
    ' History: 09/01/2003 sd - Created.
    '
    ' ***************************************************************** '
    Private Function GetSystemOptionValue(ByRef v_iOptionNumber As Integer, ByRef r_sValue As String) As Integer

        Dim result As Integer = 0
        Dim oOptions As bSIROptions.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of component services

        oOptions = New bSIROptions.Business
        m_lReturn = oOptions.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the value for this option

        m_lReturn = oOptions.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

            oOptions = Nothing

            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get system option", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOptionValue")

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        'If system option not found, default to zero
        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            r_sValue = "0"
        End If


        oOptions.Dispose()
        oOptions = Nothing

        Return result

    End Function
    'SD 09/01/2003 End

    '*************************************************************************
    ' Function Name:    GetAccountBalanceLite
    ' Description:      A lighter / faster version of GetAccountBalance for
    '                   underwriting Account Search screens
    ' History:          08/09/2003 - Tracy Richards - Created to overcome
    '                   performance issues issue with original
    ' Parameters:       v_vCompanyID = null for NON-MultiBranching
    ' ***************************************************************** '
    Public Function GetAccountBalanceLite(ByVal v_lAccountId As Integer, ByVal v_vCompanyID As Object, ByRef r_curBalance As Decimal) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear down the database object
            m_oDatabase.Parameters.Clear()

            'Populate the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="postingstatus_id", vValue:=CStr(gACTLibrary.ACTPostStatusPosted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_vCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectBalanceLiteSQL, sSQLName:=ACSelectBalanceLiteName, bStoredProcedure:=ACSelectBalanceLiteStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' with no array and more than one result what else can we do ?
            If m_oDatabase.Records.Count() = 0 Then
                r_curBalance = 0
            Else
                'Developer Guide no. 111
                r_curBalance = m_oDatabase.Records.Item(0).Fields()("sum_amount")
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            'Developer Guide No 98
            bPMFunc.LogMessage("", CStr(gPMConstants.PMELogLevel.PMLogOnError), "GetAccountBalanceLite Failed", ACApp, ACClass, "GetAccountBalanceLite", Informations.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer) As Integer
        Return GetAccountID(v_sShortCode:=v_sShortCode, r_lAccountID:=r_lAccountID, r_iDeleteAtPurge:=0)
    End Function

    Public Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer, ByRef r_iDeleteAtPurge As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "company_id", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ShortCode", v_sShortCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "AccountID", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "delete_at_purge", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            ' Peform the SQL Select statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_ACT_Get_AccountID_From_ShortCode", sSQLName:="GetAccountID From ShortCode", bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Grab the return value...

            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("AccountID").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("AccountID").Value)) Then
                r_lAccountID = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("AccountID").Value)
                r_iDeleteAtPurge = gPMFunctions.ToSafeInteger(m_oDatabase.Parameters.Item("delete_at_purge").Value)
            Else
                ' ...or not
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Developer Guide No 98
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGLAccount
    '
    ' Description: Gets the gains/loss ledger account_id depending on sales
    '              or purchase ledger
    ' ***************************************************************** '
    Public Function GetGLAccount(ByVal v_cAmount As Decimal, ByRef r_lGLAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sShortCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_cAmount > 0 Then
                m_lReturn = CType(bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_iOptionNumber:=ACCurrencyDifferenceDebitAccount, r_sOptionValue:=sShortCode), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_iOptionNumber:=ACCurrencyDifferenceCrebitAccount, r_sOptionValue:=sShortCode), gPMConstants.PMEReturnCode)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sShortCode = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the account_id from the business
            m_lReturn = CType(GetAccountID(v_sShortCode:=sShortCode, r_lAccountID:=r_lGLAccountID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGLAccountFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGLAccount", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function IsDeleted(ByVal v_lAccountId As Integer, ByRef r_bIsDeleted As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "IsDeleted"

        Dim vResultArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", v_lAccountId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsDeletedSQL, sSQLName:=ACIsDeletedName, bStoredProcedure:=ACIsDeletedStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQLName = " & ACIsDeletedName, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResultArray) Then


                r_bIsDeleted = CInt(vResultArray(0, 0)) = 1

            Else

                gPMFunctions.RaiseError("IsArray(vResultArray)", "SQL did not return an array", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    '*************************************************************************
    ' Function Name:    GetClientAccountDetails
    ' Description:      Get Turnover Details for Client Manager
    '
    ' History:          26/04/2005 - Elaine Knott 2005 Changes
    '
    ' ***************************************************************** '
    Public Function GetClientAccountDetails(ByVal v_lAccountKey As Integer, ByVal v_lCompanyID As Integer, ByRef r_curYearToDateTurnover As Decimal, ByRef r_curLastYearTurnover As Decimal, ByRef r_curClientBalance As Decimal) As Integer
        Return GetClientAccountDetails(v_lAccountKey:=v_lAccountKey, v_lCompanyID:=v_lCompanyID, r_curYearToDateTurnover:=r_curYearToDateTurnover, r_curLastYearTurnover:=r_curLastYearTurnover, r_curClientBalance:=r_curClientBalance, r_curYearToDateIncome:=0, r_curYearToDateNetIncome:=0)
    End Function

    Public Function GetClientAccountDetails(ByVal v_lAccountKey As Integer, ByVal v_lCompanyID As Integer, ByRef r_curYearToDateTurnover As Decimal, ByRef r_curLastYearTurnover As Decimal, ByRef r_curClientBalance As Decimal, ByRef r_curYearToDateIncome As Decimal, ByRef r_curYearToDateNetIncome As Decimal) As Integer


        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        'DC130606 Show Year To Date figures for Datasure
        Dim lIncludeTaxOnYTDTurnover, lIncludeTaxOnYTDIncome As Integer
        Dim sValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear down the database object
            m_oDatabase.Parameters.Clear()

            'Populate the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_key", vValue:=CStr(v_lAccountKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_lCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC130606 -Start -Show Year To Date figures for Datasure
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACIncludeTaxOnYTDTurnover, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)
            If String.IsNullOrEmpty(sValue) Then
                lIncludeTaxOnYTDTurnover = 0
            Else
                lIncludeTaxOnYTDTurnover = CInt(sValue)
            End If

            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACIncludeTaxOnYTDIncome, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)
            If String.IsNullOrEmpty(sValue) Then
                lIncludeTaxOnYTDIncome = 0
            Else
                lIncludeTaxOnYTDIncome = CInt(sValue)
            End If




            m_lReturn = m_oDatabase.Parameters.Add(sName:="Include_Tax_On_YTD_Turnover", vValue:=CStr(lIncludeTaxOnYTDTurnover), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Include_Tax_On_YTD_Income", vValue:=CStr(lIncludeTaxOnYTDIncome), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'DC130606 -End -Show Year To Date figures for Datasure


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectClientAccountDetailsSQL, sSQLName:=ACSelectClientAccountDetailsName, bStoredProcedure:=ACSelectClientAccountDetailsStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                r_curYearToDateTurnover = 0
                r_curLastYearTurnover = 0
                r_curClientBalance = 0
                'DC130606 Show Year To Date figures for Datasure
                r_curYearToDateIncome = 0
                r_curYearToDateNetIncome = 0
            Else

                r_curYearToDateTurnover = CDec(vResultArray(0, 0))

                r_curLastYearTurnover = CDec(vResultArray(1, 0))

                r_curClientBalance = CDec(vResultArray(2, 0))
                'DC130606 Show Year To Date figures for Datasure

                r_curYearToDateIncome = CDec(vResultArray(3, 0))

                r_curYearToDateNetIncome = CDec(vResultArray(4, 0))
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            'Developer Guide No 98
            bPMFunc.LogMessage("", CStr(gPMConstants.PMELogLevel.PMLogOnError), "GetClientAccountDetails Failed", ACApp, ACClass, "GetClientAccountDetails", Informations.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    Public Function GetAccountOSCommForDocuments(ByVal v_lAccountId As Integer, ByVal v_vDocumentIds As Object, ByRef r_vOSTransactions As Object) As Integer

        Const kMethodName As String = "GetAccountOSCommForDocuments"

        Dim lReturn As Integer
        Dim lLBound As Integer
        Dim lUBound As Integer
        Dim lDocument As Integer
        Dim lDocumentId As Integer
        Dim vDocTransactions As Object = Nothing

        Try

            GetAccountOSCommForDocuments = gPMConstants.PMEReturnCode.PMTrue

            ' is there are documents in the document array
            If Informations.IsArray(v_vDocumentIds) Then

                ' get the number of documents specified
                lLBound = LBound(v_vDocumentIds, 1)
                lUBound = UBound(v_vDocumentIds, 1)

                ' for each document
                For lDocument = lLBound To lUBound

                    ' get document id
                    lDocumentId = v_vDocumentIds(lDocument)

                    ' get outstanding tranactions for the specified document / account
                    lReturn = GetAccountOSCommForDocument(v_lAccountId:=v_lAccountId, v_lDocumentId:=lDocumentId, r_vOSTransactions:=vDocTransactions)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetAccountOSTransForDocument Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Informations.IsArray(vDocTransactions) Then
                        ' add the transactions for the specified document / account to the overall
                        ' transaction array
                        lReturn = MergeArrays(r_vOSTransactions, vDocTransactions)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "MergeArrays Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                Next

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetAccountOSCommForDocuments, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try

        Exit Function

    End Function
    Private Function GetAccountOSCommForDocument(ByVal v_lAccountId As Integer, ByVal v_lDocumentId As Integer, ByRef r_vOSTransactions(,) As Object) As Integer

        Const kMethodName As String = "GetAccountOSCommForDocument"

        GetAccountOSCommForDocument = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        'UPGRADE_WARNING: Couldn't resolve default property of object m_oDatabase.Parameters. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        Call AddParameterLite(m_oDatabase, "account_id", v_lAccountId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

        Call AddParameterLite(m_oDatabase, "document_id", v_lDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        ' Execute selection Query
        'UPGRADE_WARNING: Couldn't resolve default property of object m_oDatabase.SQLSelect. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If m_oDatabase.SQLSelect(sSQL:=kGetAccountOSCommForDocumentSQL, sSQLName:=kGetAccountOSCommForDocumentName, bStoredProcedure:=True, vResultArray:=r_vOSTransactions, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

            RaiseError(kMethodName, kGetAccountOSCommForDocumentSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If


        Exit Function

    End Function
    ' ***************************************************************** '
    ' Name: GetAccountOSTransForDocument
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetAccountOSTransForDocument(ByVal v_lAccountId As Integer, ByVal v_lDocumentId As Integer, ByRef r_vOSTransactions(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAccountOSTransForDocument"

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", v_lAccountId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

        bPMAddParameter.AddParameterLite(m_oDatabase, "document_id", v_lDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        ' Execute selection Query
        If m_oDatabase.SQLSelect(sSQL:=kGetAccountOSTransForDocumentSQL, sSQLName:=kGetAccountOSTransForDocumentName, bStoredProcedure:=True, vResultArray:=r_vOSTransactions, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kGetAccountOSTransForDocumentSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetAccountOSTransForDocuments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-01-2006 : Cheque Production Workflow
    ' ***************************************************************** '
    Public Function GetAccountOSTransForDocuments(ByVal v_lAccountId As Integer, ByVal v_vDocumentIds() As Object, ByRef r_vOSTransactions As Array) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAccountOSTransForDocuments"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lLBound, lUBound, lDocumentId As Integer
        Dim vDocTransactions As Array = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' is there are documents in the document array
            If Informations.IsArray(v_vDocumentIds) Then

                ' get the number of documents specified
                lLBound = v_vDocumentIds.GetLowerBound(0)
                lUBound = v_vDocumentIds.GetUpperBound(0)

                ' for each document
                For lDocument As Integer = lLBound To lUBound

                    ' get document id

                    lDocumentId = CInt(v_vDocumentIds(lDocument))

                    ' get outstanding tranactions for the specified document / account
                    lReturn = CType(GetAccountOSTransForDocument(v_lAccountId:=v_lAccountId, v_lDocumentId:=lDocumentId, r_vOSTransactions:=vDocTransactions), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetAccountOSTransForDocument Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Informations.IsArray(vDocTransactions) Then
                        ' add the transactions for the specified document / account to the overall
                        ' transaction array
                        lReturn = CType(MergeArrays(r_vOSTransactions, vDocTransactions), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "MergeArrays Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                Next

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: MergeArrays
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-01-2006 : Process ID
    ' ***************************************************************** '
    Public Function MergeArrays(ByRef r_vMainArray As Array, ByVal v_vArrayToAdd As Array) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "MergeArrays"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lLBound, lUBound, lLBoundArray2, lUBoundArray2, lLowerArrayElement, lUpperArrayElement, lNoOfAdditionalItems, lNewUBound, lPrevUBound As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(r_vMainArray) And Informations.IsArray(v_vArrayToAdd) Then

                r_vMainArray = Array.CreateInstance(GetType(Object), New Integer() {lUpperArrayElement - lLowerArrayElement + 1, 1}, New Integer() {lLowerArrayElement, 0})

                r_vMainArray = v_vArrayToAdd
                Return result

            Else


                ' check if the arrays can be merged
                ' they need to have the same bounds
                lReturn = CType(ArrayBoundsMatch(v_vArray1:=r_vMainArray, v_vArray2:=v_vArrayToAdd, r_lLBound:=lLowerArrayElement, r_lUBound:=lUpperArrayElement), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ArrayBoundsMatch Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            ' get array boundaries
            lLBound = r_vMainArray.GetLowerBound(1)
            lUBound = r_vMainArray.GetUpperBound(1)
            lUBoundArray2 = v_vArrayToAdd.GetUpperBound(1)
            lLBoundArray2 = v_vArrayToAdd.GetLowerBound(1)

            ' determine new bounds and no of additional items
            lNoOfAdditionalItems = (lUBoundArray2 - lLBoundArray2) + 1

            lNewUBound = lUBound + lNoOfAdditionalItems
            lPrevUBound = lUBound + 1

            ' resize main array
            r_vMainArray = ArraysHelper.RedimPreserve(Of Object(,))(r_vMainArray, New Integer() {lUpperArrayElement - lLowerArrayElement + 1, lNewUBound - lLBound + 1}, New Integer() {lLowerArrayElement, lLBound})

            ' for each new item in the main array
            For lItem As Integer = lPrevUBound To lNewUBound

                ' for each array element
                For lArrayElement As Integer = lLowerArrayElement To lUpperArrayElement

                    ' copy the details into the main array


                    r_vMainArray(lArrayElement, lItem) = v_vArrayToAdd(lArrayElement, lItem - lPrevUBound)

                Next

            Next


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ArrayBoundsMatch
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ArrayBoundsMatch(ByVal v_vArray1(,) As Object, ByVal v_vArray2(,) As Object, ByRef r_lLBound As Integer, ByRef r_lUBound As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ArrayBoundsMatch"

        Dim lReturn As Integer = 0
        Dim llBoundArray1, lUBoundArray1 As Integer

        Dim lLBoundArray2, lUBoundArray2 As Integer


        Try



            result = gPMConstants.PMEReturnCode.PMFalse

            ' if both arrays parameters have actually received arrays
            If Informations.IsArray(v_vArray1) And Informations.IsArray(v_vArray2) Then

                ' get the arrays boundaries

                llBoundArray1 = v_vArray1.GetLowerBound(0)

                lUBoundArray1 = v_vArray1.GetUpperBound(0)


                lLBoundArray2 = v_vArray2.GetLowerBound(0)

                lUBoundArray2 = v_vArray2.GetUpperBound(0)


                ' if the boundaries match
                If llBoundArray1 = lLBoundArray2 Or lUBoundArray1 = lUBoundArray2 Then

                    r_lLBound = llBoundArray1
                    r_lUBound = lUBoundArray1

                    ' return true to indicate they match
                    result = gPMConstants.PMEReturnCode.PMTrue

                End If


            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetAccountDetailsFromPartyCnt
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function GetAccountDetailsFromPartyCnt(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAccountDetailsFromPartyCnt"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAccountDetailsFromPartyCntSQL, sSQLName:=kGetAccountDetailsFromPartyCntName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetAccountDetailsFromPartyCntSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetUnallocatedClaimPayments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function GetUnallocatedClaimPayments(ByVal v_lAccountId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUnallocatedClaimPayments"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "account_id", v_lAccountId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetUnallocatedClaimPaymentsSQL, sSQLName:=kGetUnallocatedClaimPaymentsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetUnallocatedClaimPaymentsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' return "NOT FOUND" if there are no results
            If Not Informations.IsArray(r_vResults) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetUnallocatedClaimPaymentsForPaymentDate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-04-2006 :  Claims Document Production
    ' ***************************************************************** '
    Public Function GetUnallocatedClaimPaymentsForPaymentDate(ByVal v_dtPaymentDateFrom As Date, ByVal v_dtPaymentDateTo As Date, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUnallocatedClaimPaymentsForPaymentDate"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "date_of_payment", v_dtPaymentDateFrom, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "date_of_payment_to", v_dtPaymentDateTo, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetUnallocatedClaimPaymentsForPaymentDateSQL, sSQLName:=kGetUnallocatedClaimPaymentsForPaymentDateName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetUnallocatedClaimPaymentsForPaymentDateSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function IsValidAccountCode(ByVal m_sAccountCode As String, ByRef IsAccountCode As Boolean, ByRef Account_id As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim r_vResults(,) As Object = Nothing
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add Short_code
            m_lReturn = m_oDatabase.Parameters.Add(sName:="short_code", vValue:=m_sAccountCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Account_ID", vValue:=CStr(Account_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckCodeSQL, sSQLName:=ACCheckCodeName, bStoredProcedure:=ACCheckCodeStored, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                IsAccountCode = True
                Account_id = m_oDatabase.Parameters.Item("Account_ID").Value
            End If

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetBaseCountry
    '
    ' Description: Gets the ledger details for the passed account
    '
    ' EK 130300
    '
    ' ***************************************************************** '
    Public Function GetBaseCountry(ByVal v_lAccountId As Integer, ByRef r_lCountryId As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                ' Clear the parameters
                .Parameters.Clear()

                ' Add account_id
                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Perform the SQL
                m_lReturn = .SQLSelect(sSQL:=ACGetBaseCountrySQL, sSQLName:=ACGetBaseCountryName, bStoredProcedure:=ACGetBaseCountryStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vResultArray) Then

                    r_lCountryId = CInt(vResultArray(0, 0))
                Else
                    r_lCountryId = 0
                End If

            End With


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBaseCountry Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCountry", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Party Bank Details
    Public Function GetAccountKey(ByVal lAccountID As Integer, ByRef r_lAccountKey As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAccountKey"
        Const knAccountKey As Integer = 48

        Try

            Dim vResultArray(,) As Object = Nothing



            result = gPMConstants.PMEReturnCode.PMTrue



            result = gPMConstants.PMEReturnCode.PMTrue

            r_lAccountKey = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Do we have a key
            Dim dbNumericTemp As Double
            If (Not False) And (Double.TryParse(CStr(lAccountID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then

                ' Add the AccountID parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Account_id", vValue:=CStr(lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                If Informations.IsArray(vResultArray) Then
                    r_lAccountKey = gPMFunctions.ToSafeLong(vResultArray(knAccountKey, 0))
                Else
                    r_lAccountKey = 0
                End If

            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result

    End Function

    ''PN-62336 Date-15/01/2010
    Private Function GetIsLedgerExist(ByVal lAccountID As Object) As Boolean

        Dim result As Boolean = False

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMFalse

        m_oDatabase.Parameters.Clear()

        'Add the AccountID INPUT parameter

        lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsLedgerExistSQL, sSQLName:=ACIsLedgerExistName, bStoredProcedure:=ACIsledgerExist, vResultArray:=vResultArray)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = False
        End If

        If Informations.IsArray(vResultArray) Then
            result = gPMFunctions.ToSafeBoolean(vResultArray(0, 0))
        End If


        Return result
    End Function
    Private Function TakeOffHold(ByVal v_lAccountId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Account ID parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_ACT_Credit_Control_Take_Off_Hold", sSQLName:="spu_ACT_Credit_Control_Take_Off_Hold", bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to process TakeOffHold method", vApp:=ACApp, vClass:=ACClass, vMethod:="TakeOffHold", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
