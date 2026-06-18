Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Text
Imports SSP.Shared
'developer guide no. 129
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
    '              the Account Explorer interface.
    '
    ' Edit History:
    ' CJB 140905 PN22196 Changed UpdateElementExtras to not store 0 for
    '            account_map_id when none has been set - store null!
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
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    Private m_oAccount As bACTAccount.Form
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




            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Initialisation Code.

            ' Get Reference to Database
            '    Set m_oDatabase = GetOrionDatabase(m_lReturn, m_bCloseDatabase, vDatabase)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = PMProductFamily


            m_oAccount = New bACTAccount.Form
            m_lReturn = m_oAccount.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If

                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
                m_oAccount = Nothing
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



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Public)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Public)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Public)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
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

    Private Function IsInArray(ByRef vItem As Integer, ByRef vArray() As Object) As Boolean

        ' Check whether vItem is in [first dimension of] vArray

        Dim result As Boolean = False

        If Not Informations.IsArray(vArray) Then Return result

        For i As Integer = vArray.GetLowerBound(0) To vArray.GetUpperBound(0)

            If vItem = CDbl(vArray(i)) Then
                ' Found item
                Return True
            End If
        Next i
        Return result
    End Function
    ' eck PN5946 110803 Pass ledger_id
    'developer guide no. 101
    Public Sub GetAccountDetails(ByRef lAccountId As Integer, Optional ByRef vAccountName As String = "", Optional ByRef vShortCode As String = "", Optional ByRef vAccountType As Object = Nothing, Optional ByRef vFullKey As Object = Nothing, Optional ByRef vBalance As Object = Nothing, Optional ByRef vNominalCode As Object = Nothing, Optional ByRef vCompanyId As Object = Nothing, Optional ByRef vLedgerID As Object = Nothing, Optional ByRef iSubBranchID As Integer = 0)

        ' Query the Account table on the given lAccountID
        ' returning the optional parameters defined
        'developer guide no. 112
        Dim oFields As DataRow

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAccountDetailsSQL, sSQLName:=ACGetAccountDetailsName, bStoredProcedure:=ACGetAccountDetailsStored, lNumberRecords:=0)

        With m_oDatabase.Records
            'SD 23/07/2002 START Check for NULL values in Fields collection
            If .Count() <> 0 Then
                'Developer Guide No 111
                oFields = .Item(0).Fields()


                If Not Informations.IsNothing(vAccountName) Then

                    If Not (Convert.IsDBNull(oFields("account_name")) Or Informations.IsNothing(oFields("account_name"))) Then
                        vAccountName = oFields("account_name")
                    Else
                        vAccountName = ""
                    End If
                End If

                If Not Informations.IsNothing(vShortCode) Then

                    If Not (Convert.IsDBNull(oFields("short_code")) Or Informations.IsNothing(oFields("short_code"))) Then
                        vShortCode = oFields("short_code")
                    Else
                        vShortCode = ""
                    End If
                End If

                If Not Informations.IsNothing(vAccountType) Then

                    If Not (Convert.IsDBNull(oFields("accounttype_id")) Or Informations.IsNothing(oFields("accounttype_id"))) Then
                        vAccountType = oFields("accounttype_id")
                    Else
                        vAccountType = 0
                    End If
                End If

                If Not Informations.IsNothing(vFullKey) Then

                    'developer guide no. 98
                    GetFullPath(lNodeId:=GetNodeFromAccountID(lAccountId), vFullPath:=vFullKey)
                End If

                If Not Informations.IsNothing(vNominalCode) Then

                    If Not (Convert.IsDBNull(oFields("nominal_account_id")) Or Informations.IsNothing(oFields("nominal_account_id"))) Then
                        vNominalCode = oFields("nominal_account_id")
                    Else
                        vNominalCode = 0
                    End If
                End If

                If Not Informations.IsNothing(vCompanyId) Then

                    If Not (Convert.IsDBNull(oFields("company_id")) Or Informations.IsNothing(oFields("company_id"))) Then
                        vCompanyId = oFields("company_id")
                    Else
                        vCompanyId = 0
                    End If
                End If
                ' eck PN5946 110803

                If Not Informations.IsNothing(vLedgerID) Then

                    If Not (Convert.IsDBNull(oFields("ledger_id")) Or Informations.IsNothing(oFields("ledger_id"))) Then
                        vLedgerID = oFields("ledger_id")
                    Else
                        vLedgerID = 0
                    End If
                End If
                If Not Informations.IsNothing(vAccountType) Then

                    If Not (Convert.IsDBNull(oFields("sub_branch_id")) Or Informations.IsNothing(oFields("sub_branch_id"))) Then
                        iSubBranchID = oFields("sub_branch_id")
                    Else
                        iSubBranchID = 0
                    End If
                End If

                'SD 23/07/2002 END Check for NULL values in Fields collection
            End If
        End With


        'developer guide no. 118
        m_lReturn = m_oAccount.GetAccountBalance(r_vdAccountBalance:=vBalance, v_vAccountID:=lAccountId)

    End Sub

    Public Function GetAccountIdFromType(ByVal v_iAccountTypeId As Integer, ByRef r_vAccountIds(,) As Object, ByVal v_vLedgerID As String) As Integer

        ' Query the Account table on the given Account Type
        ' returning a variant array containing a list of account ids

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT account_id FROM Account WHERE accounttype_id = " &
                   v_iAccountTypeId & " AND ledger_id = " & v_vLedgerID

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountIDFromType", bStoredProcedure:=False, vResultArray:=r_vAccountIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_oDatabase.Parameters.Clear
            '
            '    m_lReturn = m_oDatabase.Parameters.Add( _
            ''            sName:="accounttype_id", _
            ''            vValue:=v_iAccountTypeId, _
            ''            iDirection:=PMParamInput, _
            ''            iDataType:=PMInteger)
            '
            '    m_lReturn = m_oDatabase.SQLSelect( _
            ''        sSQL:=ACSelectAccountByTypeSQL, _
            ''        sSQLName:=ACSelectAccountByTypeName, _
            ''        bStoredProcedure:=ACSelectAccountByTypeStored, _
            ''        lNumberRecords:=0, _
            ''        vResultArray:=r_vAccountIds)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountIdFromType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIdFromType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck PN6169 Add optional company parameter
    Public Function GetAccountIdFromShort(ByVal v_sShortCode As String, ByRef r_vAccountIds(,) As Object) As Integer
        Return GetAccountIdFromShort(v_sShortCode:=v_sShortCode, r_vAccountIds:=r_vAccountIds, v_vCompanyId:=Nothing)
    End Function

    Public Function GetAccountIdFromShort(ByVal v_sShortCode As String, ByRef r_vAccountIds(,) As Object, ByVal v_vCompanyId As Object) As Integer

        ' Query the Account table on the given Account Type
        ' returning a variant array containing a list of account ids
        Dim result As Integer = 0
        Dim vCompanyId As Object = DBNull.Value
        Dim sOptionValue As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=gPMConstants.PMSysOptionRestricteduserbranchOption, r_sOptionValue:=sOptionValue)

            'eck PN6169

            If Informations.IsNothing(v_vCompanyId) Then


                vCompanyId = DBNull.Value
            Else

                If Not (Convert.IsDBNull(v_vCompanyId) Or Informations.IsNothing(v_vCompanyId)) Then

                    If CInt(v_vCompanyId) > 0 Then


                        vCompanyId = v_vCompanyId
                    Else


                        vCompanyId = DBNull.Value
                    End If
                End If
            End If




            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="short_code", vValue:=v_sShortCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'eck PN6169

            If (vCompanyId Is DBNull.Value) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=vCompanyId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(vCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAccountByShortSQL, sSQLName:=ACSelectAccountByShortName, bStoredProcedure:=ACSelectAccountByShortStored, lNumberRecords:=0, vResultArray:=r_vAccountIds)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountIdFromShort Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIdFromShort", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAccountIdFromKey(ByVal v_lKey As Integer, ByRef r_vAccountIds(,) As Object) As Integer
        Return GetAccountIdFromKey(v_lKey:=v_lKey, r_vAccountIds:=r_vAccountIds, v_iCompanyId:=0)
    End Function
    Public Function GetAccountIdFromKey(ByVal v_lKey As Integer, ByRef r_vAccountIds(,) As Object, ByVal v_iCompanyId As Integer) As Integer

        ' Query the Account table on the given Account Key
        ' returning a variant array containing a list of account ids
        'ECK PN6169 Need to incorporate the company_id to ensure we get the correct
        'account_id

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="account_key", vValue:=CStr(v_lKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'ECK PN6169 company is a parameter too
                'if its not passed use current company
                If v_iCompanyId = 0 Then
                    v_iCompanyId = m_iSourceID
                End If
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(v_iCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .SQLSelect(sSQL:=ACSelectAccountByKeySQL, sSQLName:=ACSelectAccountByKeyName, bStoredProcedure:=ACSelectAccountByKeyStored, lNumberRecords:=0, vResultArray:=r_vAccountIds)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMFail

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountIdFromKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIdFromKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'eck080500
    Public Function InsertAccount(ByRef r_lAccountID As Integer, ByRef r_vAccountName As Object) As Integer
        Return InsertAccount(r_lAccountID:=r_lAccountID, r_vAccountName:=r_vAccountName, r_vShortCode:=Nothing, r_vAccountType:=Nothing, r_vLedgerId:=Nothing, r_vCompanyID:=Nothing, r_vSubBranchID:=Nothing)
    End Function

    Public Function InsertAccount(ByRef r_lAccountID As Integer, ByRef r_vAccountName As Object, ByRef r_vShortCode As Object, ByRef r_vAccountType As Object, ByRef r_vLedgerId As Object, ByRef r_vCompanyID As Object) As Integer
        Return InsertAccount(r_lAccountID:=r_lAccountID, r_vAccountName:=r_vAccountName, r_vShortCode:=r_vShortCode, r_vAccountType:=r_vAccountType, r_vLedgerId:=r_vLedgerId, r_vCompanyID:=r_vCompanyID, r_vSubBranchID:=Nothing)
    End Function

    Public Function InsertAccount(ByRef r_lAccountID As Integer, ByRef r_vAccountName As Object, ByRef r_vShortCode As Object, ByRef r_vAccountType As Object, ByRef r_vLedgerId As Object, ByRef r_vCompanyID As Object, ByRef r_vSubBranchID As Object) As Integer

        Dim result As Integer = 0
        Dim sAccountName, sShortCode As String
        Dim vAccountTypeId As Object
        Dim lAccountId As Integer
        Dim iLedgerId, iCompanyID As Integer
        Dim vSubBranchID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Insert the account with the optional parameters defined
            ' Returning the new AccountID

            With m_oAccount

                lAccountId = 0

                If Not False Then

                    sAccountName = CStr(r_vAccountName)
                Else
                    sAccountName = ""
                End If


                If Not Informations.IsNothing(r_vShortCode) Then

                    sShortCode = CStr(r_vShortCode)
                Else
                    sShortCode = ""
                End If


                If Not Informations.IsNothing(r_vAccountType) Then


                    vAccountTypeId = r_vAccountType
                Else


                    vAccountTypeId = DBNull.Value
                End If


                If Not Informations.IsNothing(r_vLedgerId) Then

                    iLedgerId = CInt(r_vLedgerId)
                Else
                    iLedgerId = 1
                End If
                'eck080500

                If Not Informations.IsNothing(r_vCompanyID) Then

                    iCompanyID = CInt(r_vCompanyID)
                Else
                    iCompanyID = 1
                End If

                'PWF 09/10/2002

                If Not Informations.IsNothing(r_vSubBranchID) Then

                    vSubBranchID = CInt(r_vSubBranchID)
                Else

                    vSubBranchID = Nothing
                End If


                m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
                'eck080500

                m_lReturn = .DirectAdd(vAccountID:=lAccountId, vPurgefrequencyID:=gACTLibrary.ACTPurgeFreqNever, vCurrencyID:=m_iCurrencyID, vLedgerId:=iLedgerId, vAccountName:=sAccountName, vShortCode:=sShortCode, vAccounttypeID:=vAccountTypeId, vAccountStatusID:=gACTLibrary.ACTAccountStatusActive, vPartySourceID:=iCompanyID, vSubBranchID:=vSubBranchID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                r_lAccountID = lAccountId

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertAccount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function DeleteAccount(ByVal lAccountId As Integer) As Boolean

        ' Delete the account with the AccountID supplied

        Dim result As Boolean = False

        BeginTrans()

        ' First delete the node that this account belongs to
        ' Then delete the account itself

        If InternalDeleteAccount(lAccountId) Then
            CommitTrans()
            Return True
        Else
            RollbackTrans()
            Return False
        End If

        Return result
    End Function
    Private Function InternalDeleteAccount(ByRef lAccountId As Integer) As Boolean

        ' Physical delete the account with the AccountID supplied
        ' and update the node break the connection with account

        Dim result As Boolean = False
        Dim lRecordsAffected As Integer

        Dim oAccount As bACTAccount.Form


        ' First update the node

        Dim lNodeId As Integer = GetNodeFromAccountID(lAccountId)

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="node_id", vValue:=CStr(lNodeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


        'developer guide no. 85
        m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateStructureTreeAccountIdSQL, sSQLName:=ACUpdateStructureTreeAccountIdName, bStoredProcedure:=ACUpdateStructureTreeAccountIdStored, lRecordsAffected:=lRecordsAffected)

        If Not DeleteNode(lNodeId) Then
            Return result
        End If



        oAccount = New bACTAccount.Form
        m_lReturn = oAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


        'Set oAccount = GetOrionBusiness(v_sClassName:="bACTAccount.Form", v_vDatabase:=m_oDatabase)

        'If oAccount Is Nothing Then
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return False
        End If

        With oAccount


            m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMDelete)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            m_lReturn = .DirectDelete(vID:=lAccountId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            '.Terminate()
            .Dispose()

        End With

        oAccount = Nothing

        Return True

    End Function
    'EK 220300
    Public Function GetElementFromAccountID(ByVal v_lAccountId As Integer) As Integer

        ' Given an account_id return any corresponsing node_id entry

        Dim lElementId, lRecordsAffected As Integer

        Dim lAccountId As Integer = v_lAccountId

        m_lReturn = GetStructureTree(r_vNodeId:=Nothing,
                                      r_vMappingID:=Nothing,
                                      r_vAccountId:=lAccountId,
                                      r_vElementId:=lElementId,
                                      r_vParentNodeId:=Nothing,
                                      r_vResultArray:=Nothing,
                                      r_vRecordCount:=lRecordsAffected)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And lRecordsAffected = 1 Then
            Return lElementId
        Else
            Return 0
        End If

    End Function
    Public Function GetNodeFromAccountID(ByVal v_lAccountId As Integer) As Integer
        'mkw 170603 PN 4457 Changed to Public from Private.
        ' Given an account_id return any corresponsing node_id entry

        Dim lRecordsAffected As Integer

        Dim lNodeId As Integer = 0
        Dim lAccountId As Integer = v_lAccountId

        m_lReturn = GetStructureTree(r_vNodeId:=lNodeId,
                              r_vMappingID:=Nothing,
                              r_vAccountId:=lAccountId,
                              r_vElementId:=Nothing,
                              r_vParentNodeId:=Nothing,
                              r_vResultArray:=Nothing,
                              r_vRecordCount:=lRecordsAffected)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And lRecordsAffected = 1 Then
            Return lNodeId
        Else
            Return 0
        End If

    End Function

    Public Function GetNodeFromMappingID(ByVal v_lMappingId As Integer) As Integer
        Return GetNodeFromMappingID(v_lMappingId:=v_lMappingId, r_vParentNodeId:=0)
    End Function
    Public Function GetNodeFromMappingID(ByVal v_lMappingId As Integer, ByRef r_vParentNodeId As Integer) As Integer

        ' Given a mapping_id return any corresponsing node_id entry

        Dim result As Integer = 0
        Dim lRecordsAffected, lParentNodeId As Integer

        Dim lNodeId As Integer = 0
        Dim lMappingId As Integer = v_lMappingId

        m_lReturn = GetStructureTree(r_vNodeId:=lNodeId,
                              r_vMappingID:=lMappingId,
                              r_vAccountId:=Nothing,
                              r_vElementId:=Nothing,
                              r_vParentNodeId:=lParentNodeId,
                              r_vResultArray:=Nothing,
                              r_vRecordCount:=lRecordsAffected)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And lRecordsAffected = 1 Then
            result = lNodeId

            If Not Informations.IsNothing(r_vParentNodeId) Then
                r_vParentNodeId = lParentNodeId
            End If
        Else
            result = 0
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetNodeFromMappingText (Public)
    '
    ' Description: Gets Ledger Node Id
    '
    ' ***************************************************************** '
    Public Function GetNodeFromMappingText(ByVal v_sMappingText As String, ByRef v_lNodeId As Integer) As Integer

        ' This routine returns NodeId from Mapping Description
        ' as defined in the mapping table. This is in place due to
        ' the lack of mapping short name or similar
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="mapping_description", vValue:=v_sMappingText, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Start - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectNodeFromMappingSQL, sSQLName:=ACSelectNodeFromMappingName, bStoredProcedure:=ACSelectNodeFromMappingStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Records.Count() <> 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SD 23/07/2002 Check if fields collection has NULL Value

            'Developer Guide No 111
            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("node_id")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("node_id"))) Then
                v_lNodeId = m_oDatabase.Records.Item(0).Fields()("node_id")
            Else
                v_lNodeId = 0
            End If
            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNodeFromMappingText Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeFromMappingText", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub GetAccountIdFromFullPath(ByVal v_sFullKey As String, ByRef r_lAccountID As Integer)
        GetAccountIdFromFullPath(v_sFullKey:=v_sFullKey, r_lAccountID:=r_lAccountID, v_vCompanyId:=Nothing)
    End Sub

    Public Sub GetAccountIdFromFullPath(ByVal v_sFullKey As String, ByRef r_lAccountID As Integer, ByVal v_vCompanyId As Object)

        Dim lNodeId As Integer

        ' Wrap the unfriendly GetNodeId call
        GetNodeIdFromFullPath(v_sFullKey:=v_sFullKey, r_lNodeId:=lNodeId, r_vElementId:=0, r_vAccountId:=r_lAccountID, r_vParentNodeId:=0, v_vSeparator:=Nothing, v_vCompanyId:=v_vCompanyId)

    End Sub

    Public Sub GetNodeIdFromFullPath(ByVal v_sFullKey As String, ByRef r_lNodeId As Integer, ByRef r_vElementId As Double, ByRef r_vParentNodeId As Double)
        GetNodeIdFromFullPath(v_sFullKey:=v_sFullKey, r_lNodeId:=r_lNodeId, r_vElementId:=r_vElementId, r_vAccountId:=0, r_vParentNodeId:=r_vParentNodeId, v_vSeparator:=Nothing, v_vCompanyId:=Nothing)
    End Sub

    Public Sub GetNodeIdFromFullPath(ByVal v_sFullKey As String, ByRef r_lNodeId As Integer, ByVal v_vCompanyId As Object)
        GetNodeIdFromFullPath(v_sFullKey:=v_sFullKey, r_lNodeId:=r_lNodeId, r_vElementId:=0, r_vAccountId:=0, r_vParentNodeId:=0, v_vSeparator:=Nothing, v_vCompanyId:=v_vCompanyId)
    End Sub

    Public Sub GetNodeIdFromFullPath(ByVal v_sFullKey As String, ByRef r_lNodeId As Integer, ByRef r_vElementId As Double, ByRef r_vAccountId As Double, ByRef r_vParentNodeId As Double, ByVal v_vSeparator As Object, ByVal v_vCompanyId As Object)

        ' Take a complete or partial full path
        ' and return a node id, element id & account id

        Dim sSeparator As String = ""

        ' return values

        ' String manipulate
        Dim nPos As Integer

        ' Element ids of fullpath elements
        Dim lElementArray() As Object = Nothing
        Dim sElement As String = ""

        ' Work Variables
        Dim lNodeId, lElementId As Integer

        ' to hold nodes of the terminal element
        Dim vNodeArray(,) As Object = Nothing
        Dim lNodeCount, lNodeIndex As Integer

        ' Element data returned from GetFullPath
        Dim vNodeElementArray As Object = Nothing
        Dim bElementMatch As Boolean

        ' Initialise return values
        Dim lNodeIdReturn As Integer = 0
        Dim lElementIdReturn As Integer = 0
        Dim lAccountIdReturn As Integer = 0
        Dim lParentNodeIdReturn As Integer = 0

        ' and return them in case of an early exit
        r_lNodeId = lNodeIdReturn


        If Not Informations.IsNothing(r_vElementId) Then
            r_vElementId = lElementIdReturn
        End If


        If Not Informations.IsNothing(r_vAccountId) Then
            r_vAccountId = lAccountIdReturn
        End If


        If Not Informations.IsNothing(r_vParentNodeId) Then
            r_vParentNodeId = lParentNodeIdReturn
        End If

        ' Analyse the full path and split into array of element ids

        Dim lElementCount As Integer = 0
        Dim sElements(lElementCount) As Object


        If Informations.IsNothing(v_vSeparator) Then
            sSeparator = "\" ' Default
        Else

            sSeparator = CStr(v_vSeparator)
        End If

        Dim nFrom As Integer = 1

        If v_sFullKey.Substring(0, sSeparator.Length) = sSeparator Then
            nFrom = sSeparator.Length + 1
        End If

        ' Loop round looking at separators
        Do While nFrom <= v_sFullKey.Length
            nPos = v_sFullKey.IndexOf(sSeparator, nFrom) + 1
            If nPos <> 0 Then
                sElement = Mid(v_sFullKey, nFrom, nPos - nFrom)
                nFrom = nPos + 1
            Else
                ' last one (no trailing separator)
                sElement = Mid(v_sFullKey, nFrom)
                nFrom = v_sFullKey.Length + 1
            End If

            If sElement > "" Then
                ' Get the Id from the Name (no id = invalid name)

                'developer guide no. 98
                lElementId = LookupElementId(sElementName:=sElement, lCompanyID:=v_vCompanyId)
                If lElementId <> 0 Then
                    ReDim Preserve lElementArray(lElementCount)
                    lElementArray(lElementCount) = lElementId
                    lElementCount += 1
                Else
                    ' Invalid so might as well exit now
                    Exit Sub
                End If
            End If
        Loop

        ' No elements means no matches
        If lElementCount = 0 Then
            Exit Sub
        End If

        ' Now find all nodes that use the rightmost element
        lElementId = lElementArray(lElementCount - 1)

        m_lReturn = GetStructureTree(r_vNodeId:=Nothing,
                              r_vMappingID:=Nothing,
                              r_vAccountId:=Nothing,
                              r_vElementId:=lElementId,
                              r_vParentNodeId:=Nothing,
                              r_vResultArray:=vNodeArray,
                              r_vRecordCount:=lNodeCount)
        ' Failed for some reason (includes no records)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' No nodes defined with this element
        If lNodeCount <= 0 Then
            Exit Sub
        End If

        ' Get the parent element list of each node and
        ' compare with the supplied list
        Dim bNodeMatch As Boolean = False

        For lNodeIndex = 0 To lNodeCount - 1

            lNodeId = CInt(Val(CStr(vNodeArray(ACTExplorerConst.ACGetStructureTreeNodeID, lNodeIndex))))

            ' If there is only one then it's a direct hit anyway
            If lNodeCount = 1 Then
                bNodeMatch = True
                Exit For
            End If


            GetFullPath(lNodeId:=lNodeId, vNodeArray:=Nothing, vElementArray:=vNodeElementArray, vFullPath:=Nothing, vSeparator:=Nothing, vCompanyId:=Nothing)

            ' do all the elements match ?
            ' must have the same number of elements at least

            If lElementCount = vNodeElementArray.GetUpperBound(0) + 1 Then
                ' look at each one (arrays are in opposite order)
                bElementMatch = True
                For lElementIndex As Integer = 0 To lElementCount - 1

                    If CDbl(vNodeElementArray(lElementCount - lElementIndex - 1)) <> lElementArray(lElementIndex) Then
                        bElementMatch = False
                        Exit For
                    End If
                Next lElementIndex

                If bElementMatch Then
                    bNodeMatch = True
                    Exit For
                End If
            End If
        Next lNodeIndex

        If bNodeMatch Then

            lNodeIdReturn = CInt(Val(CStr(vNodeArray(ACTExplorerConst.ACGetStructureTreeNodeID, lNodeIndex))))

            lElementIdReturn = CInt(Val(CStr(vNodeArray(ACTExplorerConst.ACGetStructureTreeElementID, lNodeIndex))))

            lAccountIdReturn = CInt(Val(CStr(vNodeArray(ACTExplorerConst.ACGetStructureTreeAccountID, lNodeIndex))))

            lParentNodeIdReturn = CInt(Val(CStr(vNodeArray(ACTExplorerConst.ACGetStructureTreeParentNodeID, lNodeIndex))))
        End If

        ' return values
        r_lNodeId = lNodeIdReturn


        If Not Informations.IsNothing(r_vElementId) Then
            r_vElementId = lElementIdReturn
        End If


        If Not Informations.IsNothing(r_vAccountId) Then
            r_vAccountId = lAccountIdReturn
        End If


        If Not Informations.IsNothing(r_vParentNodeId) Then
            r_vParentNodeId = lParentNodeIdReturn
        End If

    End Sub


    Public Sub GetNode(ByVal lNodeId As Integer, ByRef vResultArray(,) As Object)

        ' Get details of the given node as a single row
        ' of a two dimensional result array.
        ' Columns in the result array identified
        ' by constants with prefix ACGetNode.

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="node_id", vValue:=CStr(lNodeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStructNodeDetailsSQL, sSQLName:=ACGetStructNodeDetailsName, bStoredProcedure:=ACGetStructNodeDetailsStored, lNumberRecords:=0, vResultArray:=vResultArray)

    End Sub
    Public Sub GetAccountsOfNode(ByVal v_lParentNodeId As Integer, ByRef r_vResultArray() As Object)

        ' Build an array of all the account ids below this node
        ' by recursively calling self and adding to the end
        ' the result array is common through the calls

        Dim lNodeId As Integer
        Dim vNodeData(,) As Object = Nothing
        Dim lNumberRecords, lCount, lAccountId As Integer

        ' First time through ?
        If Not Informations.IsArray(r_vResultArray) Then
            lCount = 0

            ReDim r_vResultArray(lCount)
        Else
            lCount = r_vResultArray.GetUpperBound(0)
        End If

        GetChildrenOfNode(lParentNodeId:=v_lParentNodeId, lNumberRecords:=0, vResultArray:=vNodeData)
        If Not Informations.IsArray(vNodeData) Then
            lNumberRecords = 0
        Else

            lNumberRecords = vNodeData.GetUpperBound(1) + 1
        End If

        For lRecord As Integer = 0 To lNumberRecords - 1

            lNodeId = CInt(Val(CStr(vNodeData(ACTExplorerConst.ACGetNodeNodeID, lRecord))))

            lAccountId = CInt(Val(CStr(vNodeData(ACTExplorerConst.ACGetNodeAccountID, lRecord))))
            If lAccountId > 0 Then

                r_vResultArray(lCount) = lAccountId
                lCount += 1
                ReDim Preserve r_vResultArray(lCount)
            End If
            GetAccountsOfNode(lNodeId, r_vResultArray)
        Next lRecord

    End Sub

    'eck020801 add optional client search
    Public Sub GetChildrenOfNode(ByVal lParentNodeId As Integer, ByRef lNumberRecords As Integer, ByRef vResultArray(,) As Object)
        GetChildrenOfNode(lParentNodeId:=lParentNodeId, lNumberRecords:=lNumberRecords, vResultArray:=vResultArray, sCode:="", lCompanyID:=Nothing)
    End Sub
    Public Sub GetChildrenOfNode(ByVal lParentNodeId As Integer, ByRef lNumberRecords As Integer, ByRef vResultArray(,) As Object, ByRef lCompanyID As Object)
        GetChildrenOfNode(lParentNodeId:=lParentNodeId, lNumberRecords:=lNumberRecords, vResultArray:=vResultArray, sCode:="", lCompanyID:=lCompanyID)
    End Sub
    Public Sub GetChildrenOfNode(ByVal lParentNodeId As Integer, ByRef lNumberRecords As Integer, ByRef vResultArray(,) As Object, ByRef sCode As String)
        GetChildrenOfNode(lParentNodeId:=lParentNodeId, lNumberRecords:=lNumberRecords, vResultArray:=vResultArray, sCode:=sCode, lCompanyID:=Nothing)
    End Sub

    Public Sub GetChildrenOfNode(ByVal lParentNodeId As Integer, ByRef lNumberRecords As Integer, ByRef vResultArray(,) As Object, ByRef sCode As String, ByRef lCompanyID As Object)

        ' PWF 19/09/2002 - Add company id to the call

        If Informations.IsNothing(lCompanyID) Then


            lCompanyID = DBNull.Value
        End If

        ' Get any children of the given node as a two dimensional result array.
        ' Columns in the result array identified by constants with prefix ACGetNode.
        m_oDatabase.Parameters.Clear()

        ' Add parent node id
        m_lReturn = m_oDatabase.Parameters.Add(sName:="node_id", vValue:=lParentNodeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        ' Add code if supplied
        If sCode.Length Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        End If

        ' Add company id (or null)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=lCompanyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        ' Call appropriate function depending on code
        If sCode.Length Then
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStructClientDetailsSQL, sSQLName:=ACGetStructClientDetailsName, bStoredProcedure:=ACGetStructClientDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
        Else
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStructChildrenDetailsSQL, sSQLName:=ACGetStructChildrenDetailsName, bStoredProcedure:=ACGetStructChildrenDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
        End If

    End Sub

    Public Function FullKey(ByVal lAccountId As Integer) As String

        Dim sFullKey As String = ""

        Dim lNodeId As Integer = GetNodeFromAccountID(v_lAccountId:=lAccountId)

        GetFullPath(lNodeId:=lNodeId, vFullPath:=sFullKey)

        Return sFullKey

    End Function

    Public Function FullKeyExists(ByVal v_sFullKey As String) As Integer
        Return FullKeyExists(v_sFullKey:=v_sFullKey, r_vAccountId:=0, v_vCompanyId:=Nothing)
    End Function

    Public Function FullKeyExists(ByVal v_sFullKey As String, ByRef r_vAccountId As Integer, ByVal v_vCompanyId As Object) As Integer

        Dim result As Integer = 0
        Dim iStart, iEnd As Integer
        Dim sParentCode As New StringBuilder
        Dim lParentNodeId As Integer
        Dim vNodeData(,) As Object = Nothing
        Dim lNumberRecords As Integer
        Dim sElementName As String = ""

        'TF291099
        Dim lAccountId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check company id

            If Informations.IsNothing(v_vCompanyId) Then


                v_vCompanyId = DBNull.Value
            End If

            If (v_sFullKey.IndexOf("\"c) + 1) = 1 Then
                iStart = 2
            Else
                iStart = 1
            End If

            iEnd = v_sFullKey.IndexOf("\", iStart + 1)
            sParentCode = New StringBuilder("")
            lParentNodeId = 0

            Do
                If iEnd = 0 Then
                    iEnd = v_sFullKey.Length + 1
                End If

                sElementName = v_sFullKey.Substring(iStart - 1, Math.Min(v_sFullKey.Length, iEnd - iStart))

                GetChildrenOfNode(lParentNodeId:=lParentNodeId, lNumberRecords:=0, vResultArray:=vNodeData, lCompanyID:=v_vCompanyId)

                If Not Informations.IsArray(vNodeData) Then
                    lNumberRecords = 0
                Else

                    lNumberRecords = vNodeData.GetUpperBound(1) + 1
                End If

                result = gPMConstants.PMEReturnCode.PMFalse

                For lRecord As Integer = 0 To lNumberRecords - 1

                    If CStr(vNodeData(ACTExplorerConst.ACGetNodeElementName, lRecord)) = sElementName Then
                        result = gPMConstants.PMEReturnCode.PMTrue
                        Exit For
                    End If
                Next lRecord

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                sParentCode.Append("\" & v_sFullKey.Substring(iStart - 1, Math.Min(v_sFullKey.Length, iEnd - iStart)))

                'TF291099 - Include optional AccountID parameter
                'PWF 19/09/2002 - Include optional company id parameter
                GetNodeIdFromFullPath(v_sFullKey:=sParentCode.ToString(), r_lNodeId:=lParentNodeId, r_vElementId:=0, r_vAccountId:=lAccountId, r_vParentNodeId:=0, v_vSeparator:=Nothing, v_vCompanyId:=v_vCompanyId)

                If Not Informations.IsNothing(r_vAccountId) Then
                    If lAccountId <> 0 Then
                        r_vAccountId = lAccountId
                    End If
                End If

                'EK
                If iEnd >= v_sFullKey.Length + 1 Then Exit Do

                If lParentNodeId = 0 Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                iStart = iEnd + 1
                iEnd = v_sFullKey.IndexOf("\", iStart)

            Loop

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FullKeyExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FullKeyExists", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetBranches(ByRef r_vBranchArray(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetBranches
        ' PURPOSE: Retrieve all branches
        ' AUTHOR: David Newson
        ' DATE: 09-Oct-02, 03:08 PM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            ' Pass to common function
            result = SiriusCoreFunc.GetBranches(m_oDatabase, r_vBranchArray)


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranches", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function
    Public Function GetSubBranches(ByRef r_vSubBranchArray(,) As Object) As Integer
        Return GetSubBranches(r_vSubBranchArray:=r_vSubBranchArray, v_vCompanyId:=0)
    End Function
    Public Function GetSubBranches(ByRef r_vSubBranchArray(,) As Object, ByVal v_vCompanyId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSubBranches
        ' PURPOSE: Retrieve all sub_branches for current company
        ' AUTHOR: Peter Finney
        ' DATE: 09-Oct-02, 11:31 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            ' Pass to common function

            If Informations.IsNothing(v_vCompanyId) Then
                result = SiriusCoreFunc.GetSubBranches(m_oDatabase, m_iSourceID, r_vSubBranchArray)
            Else
                result = SiriusCoreFunc.GetSubBranches(m_oDatabase, v_vCompanyId, r_vSubBranchArray)
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranches", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function

    'Developer Guie No 101
    Public Sub GetFullPath(ByVal lNodeId As Integer, ByRef vFullPath As Object)
        GetFullPath(lNodeId:=lNodeId, vNodeArray:=Nothing, vElementArray:=Nothing, vFullPath:=vFullPath, vSeparator:=Nothing, vCompanyId:=Nothing)
    End Sub
    Public Sub GetFullPath(ByVal lNodeId As Integer, ByRef vFullPath As Object, ByRef vCompanyId As Object)
        GetFullPath(lNodeId:=lNodeId, vNodeArray:=Nothing, vElementArray:=Nothing, vFullPath:=vFullPath, vSeparator:=Nothing, vCompanyId:=vCompanyId)
    End Sub
    Public Sub GetFullPath(ByVal lNodeId As Integer, ByRef vNodeArray() As Object, ByRef vElementArray() As Object, ByRef vFullPath As Object, ByRef vSeparator As Object, ByRef vCompanyId As Object)

        ' Get the full path of the given node in one or more forms
        ' vNodeArray    if defined returns an array of NodeIDs
        ' vElementArray if defined returns an array of ElementIDs
        ' vFullPath     if defined returns path as a string of element names
        '               separated by '\' or vSeparator if defined

        Dim sSeparator As String = ""
        'developer guide no.112
        Dim oFields As DataRow

        Dim Count As Integer = 0


        If Not Informations.IsNothing(vNodeArray) Then

            ReDim vNodeArray(Count)
        End If

        If Not Informations.IsNothing(vElementArray) Then
            ReDim vElementArray(Count)
        End If

        If Informations.IsNothing(vFullPath) Then
            vFullPath = ""
        End If

        If Informations.IsNothing(vSeparator) Then
            sSeparator = "\" ' Default
        Else

            sSeparator = CStr(vSeparator)
        End If

        Do While lNodeId > 0
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="node_id", vValue:=CStr(lNodeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="element_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_node_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="mapping_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStructureTreeDetailsSQL, sSQLName:=ACGetStructureTreeDetailsName, bStoredProcedure:=ACGetStructureTreeDetailsStored, lNumberRecords:=0)

            With m_oDatabase.Records
                If .Count() = 0 Then
                    lNodeId = 0
                Else
                    'Developer Guide No 111
                    oFields = .Item(0).Fields()


                    If Not Informations.IsNothing(vNodeArray) Then
                        ReDim Preserve vNodeArray(Count)

                        vNodeArray(Count) = lNodeId
                    End If


                    ReDim Preserve vElementArray(Count)

                    'SD 23/07/2002 Check for NULL Values

                    If Not (Convert.IsDBNull(oFields("element_id")) Or Informations.IsNothing(oFields("element_id"))) Then

                        vElementArray(Count) = oFields("element_id")
                    Else

                        vElementArray(Count) = 0
                    End If


                    If Not Informations.IsNothing(vFullPath) Then

                        If Not (Convert.IsDBNull(oFields("element_name")) Or Informations.IsNothing(oFields("element_name"))) Then
                            vFullPath = sSeparator & oFields("element_name").ToString.Trim() & vFullPath
                        Else
                            vFullPath = sSeparator & vFullPath
                        End If
                    End If

                    Count += 1

                    If Not (Convert.IsDBNull(oFields("parent_node_id")) Or Informations.IsNothing(oFields("parent_node_id"))) Then
                        lNodeId = oFields("parent_node_id")
                    Else
                        lNodeId = 0
                    End If

                    ' PWF 19/09/2002 - Return company_id if requested

                    If Not Informations.IsNothing(vCompanyId) Then

                        vCompanyId = oFields("company_id")
                    End If
                End If
            End With
        Loop

    End Sub


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRClaim.
    '
    '
    ' ***************************************************************** '
    'developer guide no. 17
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object) As Integer
        Return GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageID, vResultArray:=vResultArray, vReportMapId:="")
    End Function

    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object, ByRef vReportMapId As String) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(,) As Object
        Dim vFieldArray As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'developer guide no. 17

            vResultArray = Nothing
            ' Reset Table Array

            'developer guide no. 17
            vTableArray = Nothing

            ReDim vTabArray(3, 0)


            'MKW090603 PN4574 Changed to use Table CostCentre instead of Department.
            ' {* USER DEFINED CODE (Begin) *}
            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "CostCentre"
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vReportMapId
            ' {* USER DEFINED CODE (End) *}
            iLookupType = gPMConstants.PMELookupType.PMLookupAllEffective



            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Get the Lookup items

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array


            vTableArray = vTabArray

            vFieldArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'EK 220300
    'developer guide no. 101
    Public Sub GetCompanyDetails(ByVal lAccountId As Integer, ByRef vCompany As Object, ByRef vSubBranch As String)
        'deveoper guide no.112
        Dim oFields As DataRow

        Try

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACSelectBranchByAccountSQL, sSQLName:=ACSelectBranchByAccountName, bStoredProcedure:=ACSelectBranchByAccountStored, lNumberRecords:=0)

            End With

            'Stop object errors if there are no records
            If m_oDatabase.Records.Count() > 0 Then
                With m_oDatabase.Records
                    ' Developer Guide No 111
                    oFields = m_oDatabase.Records.Item(0).Fields()


                    If Not Informations.IsNothing(vCompany) Then

                        If Not (Convert.IsDBNull(oFields("company_desc")) Or Informations.IsNothing(oFields("company_desc"))) Then
                            vCompany = oFields("company_desc")
                        Else
                            vCompany = 0
                        End If
                    End If

                    If Not Informations.IsNothing(vSubBranch) Then

                        If Not (Convert.IsDBNull(oFields("sub_branch")) Or Informations.IsNothing(oFields("sub_branch"))) Then
                            vSubBranch = oFields("sub_branch")
                        Else
                            vSubBranch = ""
                        End If
                    End If

                End With
            End If

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCompanyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompanyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    'KB PN 7526 : Required for Multi-branch filtering
    'developer guide no. 101
    Public Sub GetCompanyIdDetails(ByVal lAccountId As Integer)
        GetCompanyIdDetails(lAccountId:=lAccountId, vCompanyId:=Nothing)
    End Sub
    Public Sub GetCompanyIdDetails(ByVal lAccountId As Integer, ByRef vCompanyId As Object)
        'developer guide no.112
        Dim oFields As DataRow

        Try

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACSelectBranchIdByAccountSQL, sSQLName:=ACSelectBranchIdByAccountName, bStoredProcedure:=ACSelectBranchIdByAccountStored, lNumberRecords:=0)

            End With

            'Stop object errors if there are no records
            If m_oDatabase.Records.Count() > 0 Then
                With m_oDatabase.Records

                    'Developer Guide No 111
                    oFields = m_oDatabase.Records.Item(0).Fields()


                    If Not Informations.IsNothing(vCompanyId) Then

                        If Not (Convert.IsDBNull(oFields("company_id")) Or Informations.IsNothing(oFields("company_id"))) Then
                            vCompanyId = oFields("company_id")
                        Else
                            vCompanyId = 0
                        End If
                    End If

                End With
            End If

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCompanyIdDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompanyIdDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    'developer guide no. 101
    Public Sub GetElementExtras(ByVal lElementId As Integer, ByRef vReportMapId As Object, ByRef vAccountMapID As Object)
        GetElementExtras(lElementId:=lElementId, vTotallingId:=Nothing, vDescription:="", vReportMapId:=vReportMapId, vAccountMapID:=vAccountMapID, vIsDeletable:=Nothing, vGroupForGLExportInd:=Nothing)
    End Sub
    Public Sub GetElementExtras(ByVal lElementId As Integer, ByRef vTotallingId As Object, ByRef vDescription As String, ByRef vReportMapId As Object, ByRef vAccountMapID As Object, ByRef vIsDeletable As Object, ByRef vGroupForGLExportInd As Object)
        'developer guide no. 112
        Dim oFields As DataRow

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="element_id", vValue:=CStr(lElementId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACGetElementExtrasDetailsSQL, sSQLName:=ACGetElementExtrasDetailsName, bStoredProcedure:=ACGetElementExtrasDetailsStored, lNumberRecords:=0)

        End With

        'DD 12/07/2002
        'Stop object errors if there are no records
        If m_oDatabase.Records.Count() > 0 Then
            With m_oDatabase.Records

                'Developer Guide No 111
                oFields = m_oDatabase.Records.Item(0).Fields()

                'SD 23/07/2002 START Check for NULL in fields collection

                If Not Informations.IsNothing(vTotallingId) Then

                    If Not (Convert.IsDBNull(oFields("totalling_id")) Or Informations.IsNothing(oFields("totalling_id"))) Then
                        vTotallingId = oFields("totalling_id")
                    Else
                        vTotallingId = 0
                    End If
                End If

                If Not Informations.IsNothing(vDescription) Then

                    If Not (Convert.IsDBNull(oFields("description")) Or Informations.IsNothing(oFields("description"))) Then
                        vDescription = oFields("description")
                    Else
                        vDescription = ""
                    End If
                End If

                If Not Informations.IsNothing(vReportMapId) Then

                    If Not (Convert.IsDBNull(oFields("report_map_id")) Or Informations.IsNothing(oFields("report_map_id"))) Then
                        vReportMapId = oFields("report_map_id")
                    Else
                        vReportMapId = 0
                    End If
                End If

                If Not Informations.IsNothing(vAccountMapID) Then

                    If Not (Convert.IsDBNull(oFields("account_map_id")) Or Informations.IsNothing(oFields("account_map_id"))) Then
                        vAccountMapID = oFields("account_map_id")
                    Else
                        vAccountMapID = 0
                    End If
                End If

                If Not Informations.IsNothing(vIsDeletable) Then

                    If Not (Convert.IsDBNull(oFields("is_deletable")) Or Informations.IsNothing(oFields("is_deletable"))) Then
                        vIsDeletable = oFields("is_deletable")
                    Else
                        vIsDeletable = 0
                    End If
                End If

                If Not Informations.IsNothing(vGroupForGLExportInd) Then

                    If Not (Convert.IsDBNull(oFields("group_for_gl_export_ind")) Or Informations.IsNothing(oFields("group_for_gl_export_ind"))) Then
                        vGroupForGLExportInd = oFields("group_for_gl_export_ind")
                    Else
                        vGroupForGLExportInd = 0
                    End If
                End If
                'SD 23/07/2002 END Check for NULL in fields collection

            End With
        End If
    End Sub
    ' ***************************************************************** '
    ' Name: GetLedgerDetails (Public)
    '
    ' Description: Return all ledgers for this company in an array
    '
    ' eck PN5946 110803
    ' ***************************************************************** '
    Public Function GetLedgerDetails(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'Developer Guide No 21
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
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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

                For lSub As Integer = 1 To lRecordCount

                    'Developer Guide No 162
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()


                    If Convert.IsDBNull(oFields("ledger_id")) Or Informations.IsNothing(oFields("ledger_id")) Then

                        vResult(lSub, CLedgerID) = 0
                    Else

                        vResult(lSub, CLedgerID) = oFields("ledger_id")
                    End If


                    If Convert.IsDBNull(oFields("ledger_name")) Or Informations.IsNothing(oFields("ledger_name")) Then

                        vResult(lSub, CLedgerName) = ""
                    Else

                        vResult(lSub, CLedgerName) = oFields("ledger_name")
                    End If


                    If Convert.IsDBNull(oFields("ledger_short_name")) Or Informations.IsNothing(oFields("ledger_short_name")) Then

                        vResult(lSub, CLedgerShortName) = ""
                    Else

                        vResult(lSub, CLedgerShortName) = oFields("ledger_short_name")
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLedgerDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    Public Function InsertElementExtras(ByRef lElementId As Integer, ByRef vTotallingId As Object, ByRef vDescription As Object, ByRef vReportMapId As Object, ByRef vAccountMapID As Object, ByRef vIsDeletable As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer


        ' Insert the element with the required parameter sElementName and optional parameter vParentID
        ' Returning the new ElementID

        m_oDatabase.Parameters.Clear()

        ' Add ElementID as an INTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_id", vValue:=CStr(lElementId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Add Totalling ID as an INPUT param for an insert

        If Not Informations.IsNothing(vTotallingId) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="totalling_id", vValue:=CStr(CInt(vTotallingId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="totalling_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Add Description as an INPUT param for an insert

        If Not Informations.IsNothing(vDescription) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(vDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If


        ' Add Report Mapping ID as an INPUT param for an insert

        If Not Informations.IsNothing(vReportMapId) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="report_map_id", vValue:=CStr(CInt(vReportMapId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="report_map_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If
        ' Add Account Mapping ID as an INPUT param for an insert

        If Not Informations.IsNothing(vAccountMapID) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_map_id", vValue:=CStr(CInt(vAccountMapID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_map_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If
        ' Add Is Deletable Indicator

        If Not Informations.IsNothing(vIsDeletable) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_deletable", vValue:=CStr(CInt(vIsDeletable)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        Else
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_Deletable", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If
        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddElementExtrasSQL, sSQLName:=ACAddElementExtrasName, bStoredProcedure:=ACAddElementExtrasStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If lRecordsAffected <> 1 Then
            Return result
        End If

        Return result
    End Function
    Public Function UpdateElementExtras(ByRef lElementId As Integer, ByRef vTotallingId As Object, ByRef vDescription As Object, ByRef vReportMapId As Object, ByRef vAccountMapID As Object, ByRef vIsDeletable As Object, ByRef vGroupForGLExportInd As Object) As Boolean

        Dim result As Boolean = False
        Dim lRecordsAffected As Integer
        ' Update the element with the optional parameters vElementName, vParentID



        ' Having determined the ElementID Update the database

        m_oDatabase.Parameters.Clear()

        ' Add ElementID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_id", vValue:=CStr(lElementId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Add Totalling Id as an INPUT param for an update

        m_lReturn = m_oDatabase.Parameters.Add(sName:="totalling_id", vValue:=CStr(CInt(vTotallingId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Add Description as an INPUT param for an update

        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(vDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Add Report map Id as an INPUT param for an update

        m_lReturn = m_oDatabase.Parameters.Add(sName:="report_map_id", vValue:=CStr(CInt(vReportMapId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Add Account Map Id as an INPUT param for an update
        ' Do not pass zero for this if not set - pass NULL!  PN22196

        If CInt(vAccountMapID) = 0 Then

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_map_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_map_id", vValue:=CStr(CInt(vAccountMapID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If
        ' Add Is Deletable as an INPUT param for an update

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deletable", vValue:=CStr(CInt(vIsDeletable)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'sj 21/11/2002 - start
        'PS700
        ' Add group_for_gl_export_ind as an INPUT param for an update

        m_lReturn = m_oDatabase.Parameters.Add(sName:="group_for_gl_export_ind", vValue:=CStr(CInt(vGroupForGLExportInd)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If
        'sj 21/11/2002 - end

        ' Execute SQL UPdate Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateElementExtrasSQL, sSQLName:=ACUpdateElementExtrasName, bStoredProcedure:=ACUpdateElementExtrasStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If lRecordsAffected = 1 Then
            result = True ' Success
        End If

        Return result
    End Function
    Public Function DeleteElementExtras(ByVal v_lElementId As Integer) As Boolean

        Dim result As Boolean = False
        Dim lRecordsAffected As Integer



        Dim lElementId As Integer = v_lElementId

        ' Perform a query to check that the Element is not
        ' referenced in the Element table

        m_oDatabase.Parameters.Clear()


        ' OK To Attempt the delete now

        m_oDatabase.Parameters.Clear()

        ' Add ElementID as an INPUT param for a delete
        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_id", vValue:=CStr(v_lElementId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteElementExtrasSQL, sSQLName:=ACDeleteElementExtrasName, bStoredProcedure:=ACDeleteElementExtrasStored, lRecordsAffected:=lRecordsAffected)

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            result = True ' Success

        End If
        Return result



        ' Error.
        result = False

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteElementExtras Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteElementExtras", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    Public Function DeleteElement(ByVal v_lElementId As Integer) As Boolean
        Return DeleteElement(v_lElementId:=v_lElementId, vErrorNum:=0)
    End Function
    Public Function DeleteElement(ByVal v_lElementId As Integer, ByRef vErrorNum As Integer) As Boolean

        Dim result As Boolean = False
        Dim lRecordsAffected As Integer


        If Not Informations.IsNothing(vErrorNum) Then
            vErrorNum = ACTExplorerConst.ACExpErrGeneralFailure
        End If

        ' Delete the element defined by lElementID
        ' Will fail if the element is referenced in the StructureTree
        ' or Element tables

        ' Perform a query to check that the Element is not
        ' referenced in the StructureTree table

        Dim lElementId As Integer = v_lElementId

        m_lReturn = GetStructureTree(r_vNodeId:=Nothing,
                              r_vMappingID:=Nothing,
                              r_vAccountId:=Nothing,
                              r_vElementId:=lElementId,
                              r_vParentNodeId:=Nothing,
                              r_vResultArray:=Nothing,
                              r_vRecordCount:=lRecordsAffected)
        ' If any were selected then error
        If lRecordsAffected <> 0 Then

            If Not Informations.IsNothing(vErrorNum) Then
                vErrorNum = ACTExplorerConst.ACExpErrElementInStructure
            End If
            Return result
        End If

        ' Perform a query to check that the Element is not
        ' referenced in the Element table

        m_oDatabase.Parameters.Clear()

        ' Add ElementID as an INPUT param for the select

        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_id", vValue:=CStr(v_lElementId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsUsedElementSQL, sSQLName:=ACIsUsedElementName, bStoredProcedure:=ACIsUsedElementStored)

        ' If references to element_id exist then can't delete
        If m_oDatabase.Records.Count() > 0 Then

            If Not Informations.IsNothing(vErrorNum) Then
                vErrorNum = ACTExplorerConst.ACExpErrElementInElement
            End If
        End If

        ' OK To Attempt the delete now

        'EK 220300 Delete the Extras
        m_lReturn = DeleteElementExtras(v_lElementId:=v_lElementId)
        ' If cannot delete the extras it cannot delete the element
        If m_oDatabase.Records.Count() > 0 Then

            If Not Informations.IsNothing(vErrorNum) Then
                vErrorNum = ACTExplorerConst.ACExpErrElementInElement
            End If
        End If
        m_oDatabase.Parameters.Clear()

        ' Add ElementID as an INPUT param for a delete
        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_id", vValue:=CStr(v_lElementId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteElementSQL, sSQLName:=ACDeleteElementName, bStoredProcedure:=ACDeleteElementStored, lRecordsAffected:=lRecordsAffected)

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            result = True ' Success

            If Not Informations.IsNothing(vErrorNum) Then
                vErrorNum = ACTExplorerConst.ACExpErrOK
            End If
        End If
        Return result



        ' Error.
        result = False

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteElement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteElement", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    Public Function UpdateElement(ByRef vNodeId As Integer, ByRef vElementName As Object, ByRef vErrorNum As Integer) As Boolean
        Return UpdateElement(vNodeId:=vNodeId, vElementID:=Nothing, vElementName:=vElementName, vParentID:=Nothing, vErrorNum:=vErrorNum)
    End Function
    Public Function UpdateElement(ByRef vNodeId As Integer, ByRef vElementID As Object, ByRef vElementName As Object, ByRef vParentID As Object, ByRef vErrorNum As Integer) As Boolean

        ' Update the element with the optional parameters vElementName, vParentID
        ' If vNodeID is defined then vElementID is determined from the StructureTree table

        Dim result As Boolean = False
        Dim lNodeId, lElementId, lParentNodeId, lRecordsAffected As Integer



        If Not Informations.IsNothing(vErrorNum) Then
            vErrorNum = ACTExplorerConst.ACExpErrGeneralFailure
        End If


        If Not Informations.IsNothing(vNodeId) Then
            lNodeId = vNodeId
            ' Get the ElementID and the ParentID from the StructureTree table
            m_lReturn = GetStructureTree(r_vNodeId:=lNodeId,
                              r_vMappingID:=Nothing,
                              r_vAccountId:=Nothing,
                              r_vElementId:=lElementId,
                              r_vParentNodeId:=lParentNodeId,
                              r_vResultArray:=Nothing,
                              r_vRecordCount:=lRecordsAffected)

            If lRecordsAffected <> 1 Then
                Return result
            End If


            If Not Informations.IsNothing(vElementName) Then
                ' Check that the new ElementName is not a duplicate
                ' of an existing sibling
                If IsDuplicateError(lParentNodeId, vElementName:=vElementName, vElementID:=Nothing, vErrorNum:=0) Then

                    If Not Informations.IsNothing(vErrorNum) Then
                        vErrorNum = ACTExplorerConst.ACExpErrDuplicateElement
                    End If
                    Return result
                End If
            End If

        Else

            If Not Informations.IsNothing(vElementID) Then
                ' Get the ElementID from the parameter

                lElementId = CInt(vElementID)
            Else
                Return result
            End If
        End If


        ' Having determined the ElementID Update the database

        m_oDatabase.Parameters.Clear()

        ' Add ElementID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_id", vValue:=CStr(lElementId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Add ElementName as an INPUT param for an update

        If Not Informations.IsNothing(vElementName) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="element_name", vValue:=CStr(vElementName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="element_name", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Add ParentID as an INPUT param for an update

        If Not Informations.IsNothing(vParentID) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_id", vValue:=CStr(CInt(vParentID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Insert Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateElementSQL, sSQLName:=ACUpdateElementName, bStoredProcedure:=ACUpdateElementStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If lRecordsAffected = 1 Then
            result = True ' Success

            If Not Informations.IsNothing(vErrorNum) Then
                vErrorNum = ACTExplorerConst.ACExpErrOK
            End If
        End If

        Return result
    End Function

    Public Function InsertElement(ByRef sElementName As String) As Integer
        Return InsertElement(sElementName:=sElementName, vParentID:=Nothing, vTotallingId:=Nothing, vDescription:=Nothing, vReportMapId:=Nothing, vAccountMapID:=Nothing, vIsDeletable:=Nothing)
    End Function

    Public Function InsertElement(ByRef sElementName As String, ByRef vParentID As Object) As Integer
        Return InsertElement(sElementName:=sElementName, vParentID:=vParentID, vTotallingId:=Nothing, vDescription:=Nothing, vReportMapId:=Nothing, vAccountMapID:=Nothing, vIsDeletable:=Nothing)
    End Function

    Public Function InsertElement(ByRef sElementName As String, ByRef vTotallingId As Object, ByRef vDescription As Object, ByRef vReportMapId As Object, ByRef vAccountMapID As Object, ByRef vIsDeletable As Object) As Integer
        Return InsertElement(sElementName:=sElementName, vParentID:=Nothing, vTotallingId:=vTotallingId, vDescription:=vDescription, vReportMapId:=vReportMapId, vAccountMapID:=vAccountMapID, vIsDeletable:=vIsDeletable)
    End Function

    Public Function InsertElement(ByRef sElementName As String, ByRef vTotallingId As Object, ByRef vDescription As Object, ByRef vReportMapId As Object, ByRef vAccountMapID As Object) As Integer
        Return InsertElement(sElementName:=sElementName, vParentID:=Nothing, vTotallingId:=vTotallingId, vDescription:=vDescription, vReportMapId:=vReportMapId, vAccountMapID:=vAccountMapID, vIsDeletable:=Nothing)
    End Function

    Public Function InsertElement(ByRef sElementName As String, ByRef vParentID As Object, ByRef vTotallingId As Object, ByRef vDescription As Object, ByRef vReportMapId As Object, ByRef vAccountMapID As Object, ByRef vIsDeletable As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer


        ' Insert the element with the required parameter sElementName and optional parameter vParentID
        ' Returning the new ElementID

        m_oDatabase.Parameters.Clear()

        ' Add ElementID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Add ElementName as an INPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_name", vValue:=sElementName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Add ParentID as an INPUT param for an insert

        If Not Informations.IsNothing(vParentID) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_id", vValue:=CStr(CInt(vParentID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddElementSQL, sSQLName:=ACAddElementName, bStoredProcedure:=ACAddElementStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If lRecordsAffected <> 1 Then
            Return result
        End If

        ' Get the ID of the record inserted
        result = m_oDatabase.Parameters.Item("element_id").Value
        'EK 220300
        m_lReturn = InsertElementExtras(lElementId:=result, vTotallingId:=vTotallingId, vDescription:=vDescription, vReportMapId:=vReportMapId, vAccountMapID:=vAccountMapID, vIsDeletable:=vIsDeletable)

        Return result
    End Function

    Public Function LookupElementName(ByRef lElementId As Integer) As String


        'Return the ElementName matching the given ElementID
        Dim result As String = String.Empty
        result = ""

        m_oDatabase.Parameters.Clear()

        ' Add the ElementID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_id", vValue:=CStr(lElementId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        ' Add the ElementName parameter (INPUT)

        'developer guide no. 85
        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_name", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        ' Add the CompanyID parameter (INPUT)

        'developer guide no. 85
        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetElementDetailsSQL, sSQLName:=ACGetElementDetailsName, bStoredProcedure:=ACGetElementDetailsStored, lNumberRecords:=0)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If m_oDatabase.Records.Count() <> 1 Then
            Return result
        End If

        'developer Guide No 162 and 21
        Dim oFields As DataRow = m_oDatabase.Records.Item(0).Fields()


        If Convert.IsDBNull(oFields("element_name")) Or Informations.IsNothing(oFields("element_name")) Then
            Return ""
        Else
            Return oFields("element_name")
        End If

        Return result
    End Function


    ' *********************************************************** '
    ' LookupElementID
    '
    ' Syntax:
    '   sElementName    Input - Element name to search for.
    '   lCompanyID      Input - Company ID to restrict search
    '                  Output - Returns company id of found node
    '
    ' History:
    '   PWF 19/09/2002 - Added company_id functionality
    ' *********************************************************** '
    'developer guide no.101
    Public Function LookupElementId(ByVal sElementName As String) As Integer
        Return LookupElementId(sElementName:=sElementName, lCompanyID:=Nothing)
    End Function
    Public Function LookupElementId(ByVal sElementName As String, ByRef lCompanyID As Object) As Integer

        Dim result As Integer = 0

        Try

            'Return the ElementIdmatching the given ElementName

            ' PWF 19/09/2002 - Default company id if not supplied

            If Informations.IsNothing(lCompanyID) Then
                lCompanyID = DBNull.Value
            End If

            With m_oDatabase
                .Parameters.Clear()

                ' Add the ElementID parameter (INPUT)

                'developer guide no. 85
                m_lReturn = .Parameters.Add(sName:="element_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Add the ElementName parameter (INPUT)
                ' CTAF 020101 - Changed DataType from PMLong to PMString
                m_lReturn = .Parameters.Add(sName:="element_name", vValue:=sElementName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                ' PWF 19/09/2002 - Added company id parameter
                'developer guide no.85
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=lCompanyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetElementDetailsSQL, sSQLName:=ACGetElementDetailsName, bStoredProcedure:=ACGetElementDetailsStored, lNumberRecords:=0)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (m_oDatabase.Records.Count() <> 1) Then
                    Return result
                End If

                ' Set return values
                'Developer Guide No 111
                result = gPMFunctions.NullToLong(.Records.Item(0).Fields()("element_id"))
                lCompanyID = gPMFunctions.NullToLong(.Records.Item(0).Fields()("company_id"))
            End With

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LookupElementID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LookupElementID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)



            Return result
        End Try
    End Function

    Public Sub GetElementRelationships(ByRef lNumberRecords As Integer, ByRef vResultArray(,) As Object)

        ' Get any relationships defined in the Elements table.
        ' Does not reference the StructureTree table.
        ' Columns in the result array identified
        ' by constants with prefix ACGetRel.

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllElementDetailsSQL, sSQLName:=ACGetAllElementDetailsName, bStoredProcedure:=ACGetAllElementDetailsStored, lNumberRecords:=lNumberRecords, vResultArray:=vResultArray)

    End Sub
    Public Function InsertMapping(ByRef sMapName As String) As Integer
        Return InsertMapping(sMapName:=sMapName, vMapType:=Nothing)
    End Function

    Public Function InsertMapping(ByRef sMapName As String, ByRef vMapType As Object) As Integer

        Dim result As Integer = 0

        ' Insert the mapping with the required parameter sMapName
        ' and optional parameter vMapType
        ' Returning the new MapID

        Dim lRecordsAffected As Integer

        result = 0

        m_oDatabase.Parameters.Clear()

        ' Add MapID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="mapping_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' CJB PN15688 - Cater for multi-branch accounting (was prev hard-coded to 1)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If


        If Informations.IsNothing(vMapType) Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="maptype_id", vValue:=CStr(ACTExplorerConst.ACDefaultMapType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        Else

            m_lReturn = m_oDatabase.Parameters.Add(sName:="maptype_id", vValue:=CStr(vMapType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=sMapName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddMappingSQL, sSQLName:=ACAddMappingName, bStoredProcedure:=ACAddMappingStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If lRecordsAffected = 1 Then
            ' Get the ID of the record inserted
            Return m_oDatabase.Parameters.Item("mapping_id").Value
        End If

        Return result
    End Function
    Public Function UpdateMapping(ByRef lMapID As Integer) As Boolean
        Return UpdateMapping(lMapID:=lMapID, vMapName:=Nothing, vMapType:=Nothing)
    End Function
    Public Function UpdateMapping(ByRef lMapID As Integer, ByRef vMapName As Object) As Boolean
        Return UpdateMapping(lMapID:=lMapID, vMapName:=vMapName, vMapType:=Nothing)
    End Function
    Public Function UpdateMapping(ByRef lMapID As Integer, ByRef vMapName As Object, ByRef vMapType As Object) As Boolean

        Dim result As Boolean = False
        Dim iMapType As Integer
        Dim sMapName As String = ""
        Dim lRecordsAffected As Integer

        ' Update the mapping with the optional parameters vMapName, vMapType



        If Informations.IsNothing(vMapType) Then
            iMapType = ACTExplorerConst.ACDefaultMapType
        Else

            iMapType = CInt(vMapType)
        End If


        If Informations.IsNothing(vMapName) Then
            sMapName = ""
        Else

            sMapName = CStr(vMapName)
        End If


        m_oDatabase.Parameters.Clear()

        ' Add ElementID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="mapping_id", vValue:=CStr(lMapID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'SP DEBUG - FIX THIS HARD CODING
        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(ACTExplorerConst.ACDefaultCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' CTAF 191200 - Moved this above description

        If Informations.IsNothing(vMapType) Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="maptype_id", vValue:=CStr(ACTExplorerConst.ACDefaultMapType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        Else

            m_lReturn = m_oDatabase.Parameters.Add(sName:="maptype_id", vValue:=CStr(vMapType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=sMapName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Insert Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateMappingSQL, sSQLName:=ACUpdateMappingName, bStoredProcedure:=ACUpdateMappingStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If lRecordsAffected = 1 Then
            result = True ' Success
        End If

        Return result
    End Function

    Public Function DeleteMapping(ByRef lMapID As Integer) As Boolean

        Dim result As Boolean = False
        Dim lRecordsAffected As Integer


        ' Delete the mapping identified by lMapID
        ' will fail if lMapID is referenced by StructureTree table.

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="mapping_id", vValue:=CStr(lMapID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteMappingSQL, sSQLName:=ACDeleteMappingName, bStoredProcedure:=ACDeleteMappingStored, lRecordsAffected:=lRecordsAffected)

        ' Should affect one record only
        If lRecordsAffected = 1 Then
            result = True ' Success
        End If

        Return result
    End Function

    Public Function IsDuplicateError(ByRef lParentNodeId As Integer, ByRef vElementName As Object, ByRef vElementID As Object, ByRef vErrorNum As Integer) As Boolean

        Dim result As Boolean = False

        Dim sElementName As String = ""


        If Not Informations.IsNothing(vElementID) Then
            ' Get the ElementName from the Element table

            sElementName = LookupElementName(CInt(vElementID))
        ElseIf Not Informations.IsNothing(vElementName) Then

            sElementName = CStr(vElementName)
        Else
            Return result
        End If

        ' Check whether sElementName is a duplicate
        ' of an existing sibling

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_node_id", vValue:=CStr(lParentNodeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_name", vValue:=sElementName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsDuplicateElementSQL, sSQLName:=ACIsDuplicateElementName, bStoredProcedure:=ACIsDuplicateElementStored, lNumberRecords:=0)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If m_oDatabase.Records.Count() <> 0 Then
            result = True

            If Not Informations.IsNothing(vErrorNum) Then
                vErrorNum = ACTExplorerConst.ACExpErrDuplicateElement
            End If
            Return result
        End If

        Return result
    End Function

    Public Function InsertNode(ByRef lParentNodeId As Integer, ByRef lElementId As Integer) As Integer
        Return InsertNode(lParentNodeId:=lParentNodeId, lElementId:=lElementId, vAccountID:=Nothing, vErrorNum:=0)
    End Function

    Public Function InsertNode(ByRef lParentNodeId As Integer, ByRef lElementId As Integer, ByRef vAccountID As Object) As Integer
        Return InsertNode(lParentNodeId:=lParentNodeId, lElementId:=lElementId, vAccountID:=vAccountID, vErrorNum:=0)
    End Function

    Public Function InsertNode(ByRef lParentNodeId As Integer, ByRef lElementId As Integer, ByRef vAccountID As Object, ByRef vErrorNum As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer


        If Not Informations.IsNothing(vErrorNum) Then vErrorNum = ACTExplorerConst.ACExpErrGeneralFailure

        ' Insert the node with the required parameter lParentNode, lElementID
        ' and optional parameter vAccountID
        ' Returning the new NodeID

        ' Check that lElementID is not already in the full path
        Dim vElementArray As Object = Nothing
        GetFullPath(lParentNodeId, vNodeArray:=Nothing, vElementArray:=vElementArray, vFullPath:=Nothing, vSeparator:=Nothing, vCompanyId:=Nothing)

        If IsInArray(CInt(lElementId), vElementArray) Then
            ' It is... return error status

            If Not Informations.IsNothing(vErrorNum) Then vErrorNum = ACTExplorerConst.ACExpErrMultipleElementInPath
            Return result
        End If

        '    ' Check for duplicate - Don't cos its no longer just siblings - no longer appropriate
        '    If IsDuplicateError(lParentNodeId, vElementID:=CVar(lElementID)) Then
        '        If IsMissing(vErrorNum) = False Then vErrorNum = ACExpErrDuplicateElement
        '        Exit Function
        '    End If

        m_oDatabase.Parameters.Clear()

        ' Add NodeID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="node_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'DD 12/08/2002: Multi-branch accounting
        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If


        'developer guide no. 85
        m_lReturn = m_oDatabase.Parameters.Add(sName:="mapping_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If


        If Not Informations.IsNothing(vAccountID) Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(vAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="element_id", vValue:=CStr(lElementId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_node_id", vValue:=CStr(lParentNodeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStructureTreeSQL, sSQLName:=ACAddStructureTreeName, bStoredProcedure:=ACAddStructureTreeStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If lRecordsAffected = 1 Then
            ' Get the ID of the record inserted
            Return m_oDatabase.Parameters.Item("node_id").Value
        End If

        Return result
    End Function

    Public Function MoveNode(ByRef lSrceNodeID As Integer, ByRef lDestNodeID As Integer, ByRef vErrorNum As Integer) As Boolean

        ' Move the node identified by lSrceNodeID
        ' by setting its ParentNodeID to lDestNodeID

        Dim result As Boolean = False
        Dim lRecordsAffected As Integer

        vErrorNum = ACTExplorerConst.ACExpErrGeneralFailure

        ' Check whether node is movable
        If IsNodeMovable(lSrceNodeID, lDestNodeID, vErrorNum) Then

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="node_id", vValue:=CStr(lSrceNodeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_node_id", vValue:=CStr(lDestNodeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateStructureTreeParentIDSQL, sSQLName:=ACUpdateStructureTreeParentIDName, bStoredProcedure:=ACUpdateStructureTreeParentIDStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Should affect one record only
            If lRecordsAffected = 1 Then
                result = True ' Success
                vErrorNum = ACTExplorerConst.ACExpErrOK
                Return result
            End If
        End If
        Return result
    End Function

    Public Function MapNode(ByRef lNodeId As Integer, ByRef lMapID As Integer, ByRef vErrorNum As Integer) As Boolean

        ' If lMapID > 0 Then Map the node identified by lNodeID
        '       by setting its MapID to lMapID
        '       which must be defined in Mapping (see InsertMapping).
        ' If lMapID = 0 Then Unmaps the node identified by lNodeID

        Dim result As Boolean = False
        Dim lRecordsAffected As Integer

        vErrorNum = ACTExplorerConst.ACExpErrGeneralFailure

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="node_id", vValue:=CStr(lNodeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' If the MapId is missing then the SP will update to NULL

        If lMapID <> 0 Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="mapping_id", vValue:=CStr(lMapID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="mapping_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If


        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateStructureTreeMapIDSQL, sSQLName:=ACUpdateStructureTreeMapIDName, bStoredProcedure:=ACUpdateStructureTreeMapIDStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If lRecordsAffected = 1 Then
            result = True ' Success
            vErrorNum = ACTExplorerConst.ACExpErrOK
            Return result
        End If

        Return result
    End Function

    Public Function IsNodeDeletable(ByRef lNodeId As Integer) As Boolean
        ' Public entry point to private function
        Return InternalDeleteNode(lNodeId, r_bDeleteIt:=False)
    End Function


    Public Function IsNodeMovable(ByRef lSrceNodeID As Integer, ByRef lDestNodeID As Integer, ByRef vErrorNum As Integer) As Boolean
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: IsNodeMovable
        ' PURPOSE: Returns True if a Node is movable in the Tree
        ' AUTHOR: Danny Davis
        ' DATE: 20/09/2002, 10:33
        ' CHANGES: Changed so that individual Accounts can be moved
        ' ---------------------------------------------------------------------------

        Dim result As Boolean = False

        Try


            Dim lRecordsAffected As Integer
            Dim vNodeArray(,) As Object = Nothing


            Dim lElementId As Integer

            ' Get the ElementID of the source node from the StructureTree table

            m_lReturn = GetStructureTree(r_vNodeId:=lSrceNodeID,
                              r_vMappingID:=Nothing,
                              r_vAccountId:=Nothing,
                              r_vElementId:=lElementId,
                              r_vParentNodeId:=Nothing,
                              r_vResultArray:=vNodeArray,
                              r_vRecordCount:=lRecordsAffected)

            If lRecordsAffected <> 1 Then
                Return result
            End If

            ' Is this Node an Account

            If CStr(vNodeArray(2, 0)) <> "" Then
                'It is, let it proceed
                Return True
            End If

            ' Get the full path of the destination node
            Dim vElementArray As Object = Nothing

            GetFullPath(lDestNodeID, vNodeArray:=Nothing, vElementArray:=vElementArray, vFullPath:=Nothing, vSeparator:=Nothing, vCompanyId:=Nothing)

            result = InternalDeleteNode(lSrceNodeID, r_bDeleteIt:=False, r_vElementArray:=vElementArray, r_vErrorNum:=vErrorNum)


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    '''''MessageBox.Show("bACTExplorer.Form.IsNodeMovable" &
                    '''''                "Version: " & CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision) &
                    '''''                " At line: " & CStr(Informations.Erl()) & "|" & Informations.Err().Source & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                    '''''                Informations.Err().Number & ":" & Informations.Err().Description, Application.ProductName)

                    Return result
            End Select

        Finally



        End Try
        Return result
    End Function

    Public Function DeleteNode(ByRef lNodeId As Integer) As Boolean
        ' Public entry point to private function
        BeginTrans()
        If InternalDeleteNode(lNodeId, r_bDeleteIt:=True) Then
            CommitTrans()
            Return True
        Else
            RollbackTrans()
            Return False
        End If
    End Function

    Private Function InternalDoDeleteNode(ByRef lNodeId As Integer) As Boolean

        Dim result As Boolean = False

        Dim lRecordsAffected As Integer

        Dim lElementId As Integer = 0

        ' Get the ElementID of the node to be deleted
        ' from the StructureTree table
        m_lReturn = GetStructureTree(r_vNodeId:=lNodeId,
                              r_vMappingID:=Nothing,
                              r_vAccountId:=Nothing,
                              r_vElementId:=lElementId,
                              r_vParentNodeId:=Nothing,
                              r_vResultArray:=Nothing,
                              r_vRecordCount:=lRecordsAffected)
        If lRecordsAffected <> 1 Then
            Return result
        End If

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="node_id", vValue:=CStr(lNodeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteStructureTreeSQL, sSQLName:=ACDeleteStructureTreeName, bStoredProcedure:=ACDeleteStructureTreeStored, lRecordsAffected:=lRecordsAffected)

        ' Should affect one record only
        If lRecordsAffected = 1 Then
            result = True ' Success
            ' Now attempt to delete the Element
            If Not DeleteElement(lElementId) Then
                ' Element was referenced elsewhere
            End If
        End If

        Return result
    End Function

    Private Function InternalDeleteNode(ByVal v_lNodeId As Integer, ByRef r_bDeleteIt As Boolean, Optional ByRef r_vElementArray() As Object = Nothing, Optional ByRef r_vErrorNum As Integer = 0) As Boolean

        ' r_vElementArray used to check for multiple elements
        ' if r_vElementArray is defined then r_vErrorNum is required

        Dim lAccountId, lElementId, lRecord As Integer
        Dim lRecordCount As gPMConstants.PMEReturnCode

        ' To hold Node data between recursions
        Dim vNodeArray(,) As Object = Nothing

        Dim bResult As Boolean = True

        Dim lNodeId As Integer = v_lNodeId

        ' Check this node's AccountID and ElementID
        m_lReturn = GetStructureTree(r_vNodeId:=lNodeId,
                              r_vMappingID:=Nothing,
                              r_vAccountId:=lAccountId,
                              r_vElementId:=lElementId,
                              r_vParentNodeId:=Nothing,
                              r_vResultArray:=Nothing,
                              r_vRecordCount:=lRecordCount)
        If lRecordCount = gPMConstants.PMEReturnCode.PMTrue Then
            ' This is an account if AccountID defined
            If lAccountId <> 0 Then

                If Not Informations.IsNothing(r_vErrorNum) Then
                    r_vErrorNum = ACTExplorerConst.ACExpErrNodeHasAccounts
                End If
                bResult = False
            Else

                If Not Informations.IsNothing(r_vElementArray) Then
                    ' Is this ElementID in the array?
                    If IsInArray(CInt(lElementId), r_vElementArray) Then
                        ' It is... return error status
                        r_vErrorNum = ACTExplorerConst.ACExpErrMultipleElementInPath
                        bResult = False
                    End If
                End If
            End If
        Else
            bResult = False
        End If

        If bResult Then

            ' Search for child nodes

            m_lReturn = GetStructureTree(r_vNodeId:=Nothing,
                              r_vMappingID:=Nothing,
                              r_vAccountId:=Nothing,
                              r_vElementId:=Nothing,
                              r_vParentNodeId:=lNodeId,
                              r_vResultArray:=vNodeArray,
                              r_vRecordCount:=lRecordCount)
            If lRecordCount = gPMConstants.PMEReturnCode.PMTrue Then
                lRecord = 0

                ' Move through Recordset
                Do While Not (lRecord > (lRecordCount - 1)) And bResult

                    lNodeId = CInt(vNodeArray(ACTExplorerConst.ACGetStructureTreeNodeID, lRecord))
                    ' Check next node (recursive)

                    If Informations.IsNothing(r_vElementArray) Then
                        bResult = InternalDeleteNode(lNodeId, r_bDeleteIt)
                    Else
                        bResult = InternalDeleteNode(lNodeId, r_bDeleteIt, r_vElementArray, r_vErrorNum)
                    End If
                    lRecord += 1

                Loop
            End If
        End If

        If bResult And r_bDeleteIt Then
            ' Now delete the node
            bResult = InternalDoDeleteNode(v_lNodeId)
        End If

        Return bResult

    End Function
    'developer guide no.17
    Public Function GetStructureTree(ByRef r_vNodeId As Object,
                                     ByRef r_vMappingID As Object,
                                     ByRef r_vAccountId As Object,
                                     ByRef r_vElementId As Object,
                                     ByRef r_vParentNodeId As Object,
                                     ByRef r_vResultArray(,) As Object,
                                     ByRef r_vRecordCount As Object) As Integer


        ' Selects :
        '   zero or more StructureTree records using 5 possible key params
        '   or combinations of the keys.
        '
        '        NodeId
        '        ElementId
        '        ParentNodeId
        '        AccountId
        '        MappingId
        '
        '  If the param is supplied and is not zero then it is passed to the SP
        '  if the param is missing or zero then it is passed to the SP as NULL
        '  and the SP ignores it.
        '
        ' Returns :
        '  If only one record is returned then the byref parameter variables

        '  are populated with the field values
        '  If multiple records are returned then the resuls are in vResultArray

        ' Possible return values (for a single record)

        Dim result As Integer = 0
        Dim lNodeId, lMappingId, lAccountId, lElementId, lParentNodeId, lRecordCount As Integer

        ' Params for PMDAO
        'developer guide no. 101
        Dim vNodeIdParam As Object
        Dim vElementIdParam As Object
        Dim vParentNodeIdParam As Object
        Dim vAccountIdParam As Object
        Dim vMappingIdParam As Object
        Dim bWhereConditionExists As Boolean = False
        Try

            Dim sSQL, sPrefix As String

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Assume all parameters are not passed (null) initially


            vNodeIdParam = Nothing

            vElementIdParam = Nothing

            vParentNodeIdParam = Nothing

            vAccountIdParam = Nothing

            vMappingIdParam = Nothing

            ' Test each parameter to see if it should be passed to the SP


            If Not Informations.IsNothing(r_vNodeId) Then
                If r_vNodeId <> 0 Then
                    vNodeIdParam = r_vNodeId
                End If
            End If


            If Not Informations.IsNothing(r_vElementId) Then
                If r_vElementId <> 0 Then
                    vElementIdParam = r_vElementId
                End If
            End If


            If Not Informations.IsNothing(r_vParentNodeId) Then
                If r_vParentNodeId <> 0 Then
                    vParentNodeIdParam = r_vParentNodeId
                End If
            End If


            If Not Informations.IsNothing(r_vAccountId) Then
                If r_vAccountId <> 0 Then
                    vAccountIdParam = r_vAccountId
                End If
            End If


            If Not Informations.IsNothing(r_vMappingID) Then
                If r_vMappingID <> 0 Then
                    vMappingIdParam = r_vMappingID
                End If
            End If

            ' In case no values are returned

            lRecordCount = 0
            lNodeId = 0
            lMappingId = 0
            lAccountId = 0
            lElementId = 0
            lParentNodeId = 0

            With m_oDatabase
                sSQL = "SELECT s.node_id, s.mapping_id, s.account_id, s.element_id, s.parent_node_id, e.element_name " &
                       "FROM   StructureTree s " &
                       "JOIN   Element e ON s.element_id = e.element_id "

                sPrefix = "WHERE "


                If Not (Convert.IsDBNull(vNodeIdParam) Or Informations.IsNothing(vNodeIdParam)) Then
                    sSQL = sSQL & sPrefix & "(s.node_id = " & CStr(vNodeIdParam) & ") "
                    sPrefix = "AND "
                    bWhereConditionExists = True
                End If


                If Not (Convert.IsDBNull(vElementIdParam) Or Informations.IsNothing(vElementIdParam)) Then
                    sSQL = sSQL & sPrefix & "(s.element_id = " & CStr(vElementIdParam) & ") "
                    sPrefix = "AND "
                    bWhereConditionExists = True
                End If


                If Not (Convert.IsDBNull(vParentNodeIdParam) Or Informations.IsNothing(vParentNodeIdParam)) Then
                    sSQL = sSQL & sPrefix & "(s.parent_node_id = " & CStr(vParentNodeIdParam) & ") "
                    sPrefix = "AND "
                    bWhereConditionExists = True
                End If


                If Not (Convert.IsDBNull(vAccountIdParam) Or Informations.IsNothing(vAccountIdParam)) Then
                    sSQL = sSQL & sPrefix & "(s.account_id = " & CStr(vAccountIdParam) & ") "
                    sPrefix = "AND "
                    bWhereConditionExists = True
                End If


                If Not (Convert.IsDBNull(vMappingIdParam) Or Informations.IsNothing(vMappingIdParam)) Then
                    sSQL = sSQL & sPrefix & "(s.mapping_id = " & CStr(vMappingIdParam) & ")"
                    bWhereConditionExists = True
                End If

                If Not bWhereConditionExists Then
                    sSQL = "SELECT top 1 s.node_id, s.mapping_id, s.account_id, s.element_id, s.parent_node_id, e.element_name " &
                      "FROM   StructureTree s " &
                      "JOIN   Element e ON s.element_id = e.element_id "
                End If
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetStructureTree", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' No array so use Records collection

                If Informations.IsNothing(r_vResultArray) Then
                    lRecordCount = .Records.Count()
                    If lRecordCount > 0 Then
                        'Developer Guide No 111
                        'Starts

                        'SD 23/07/2002 START Check for NULL values

                        With m_oDatabase.Records.Item(0).Fields
                            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields("node_id")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields("node_id"))) Then
                                lNodeId = m_oDatabase.Records.Item(0).Fields()("node_id")
                            Else
                                lNodeId = 0
                            End If

                            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields("mapping_id")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields("mapping_id"))) Then
                                lMappingId = m_oDatabase.Records.Item(0).Fields("mapping_id")
                            Else
                                lMappingId = 0
                            End If

                            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields("account_id")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields("account_id"))) Then
                                lAccountId = m_oDatabase.Records.Item(0).Fields("account_id")
                            Else
                                lAccountId = 0
                            End If

                            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields("element_id")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields("element_id"))) Then
                                lElementId = m_oDatabase.Records.Item(0).Fields("element_id")
                            Else
                                lElementId = 0
                            End If

                            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields("parent_node_id")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields("parent_node_id"))) Then
                                lParentNodeId = m_oDatabase.Records.Item(0).Fields("parent_node_id")
                            Else
                                lParentNodeId = 0
                            End If
                            'SD 23/07/2002 END Check for NULL values
                        End With
                        'Ends
                    End If
                Else
                    ' Get Values from the array

                    If Informations.IsArray(r_vResultArray) Then
                        ' Return first record values
                        lRecordCount = r_vResultArray.GetUpperBound(1) + 1

                        lNodeId = CInt(Val(CStr(r_vResultArray(ACTExplorerConst.ACGetStructureTreeNodeID, 0))))

                        lMappingId = CInt(Val(CStr(r_vResultArray(ACTExplorerConst.ACGetStructureTreeMappingID, 0))))

                        lAccountId = CInt(Val(CStr(r_vResultArray(ACTExplorerConst.ACGetStructureTreeAccountID, 0))))

                        lElementId = CInt(Val(CStr(r_vResultArray(ACTExplorerConst.ACGetStructureTreeElementID, 0))))

                        lParentNodeId = CInt(Val(CStr(r_vResultArray(ACTExplorerConst.ACGetStructureTreeParentNodeID, 0))))
                    End If
                End If

            End With


            'developer guide no. 118
            r_vNodeId = lNodeId



            'developer guide no. 118
            r_vMappingID = lMappingId


            'developer guide no. 118
            r_vAccountId = lAccountId



            'developer guide no. 118
            r_vElementId = lElementId



            'developer guide no. 118
            r_vParentNodeId = lParentNodeId



            'developer guide no. 118
            r_vRecordCount = lRecordCount



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStructureTree Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStructureTree", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetLedgerOfNode(ByRef r_lNodeId As Integer, ByRef r_iLedgerId As Integer) As Integer

        Dim result As Integer = 0
        Const ACTopOfTheTree As Integer = 1
        Const ACGeneralLedger As Integer = 1
        Dim lNextNodeId, lParentNodeId As Integer
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lNextNodeId = r_lNodeId

            r_iLedgerId = ACGeneralLedger 'if in doubt it's the GL

            Do

                If lNextNodeId <= ACTopOfTheTree Then
                    r_iLedgerId = ACGeneralLedger
                    Exit Do
                End If


                GetNode(lNextNodeId, vResultArray)
                'ECK221100 Set valid ledger id
                '        If vResultArray(ACGetNodeLedgerId, 0) <> "" Then

                If CStr(vResultArray(9, 0)) <> "" Then

                    r_iLedgerId = CInt(vResultArray(9, 0))
                    Exit Do
                Else
                    r_iLedgerId = 1
                End If

                m_lReturn = GetStructureTree(r_vNodeId:=lNextNodeId, _
                              r_vMappingID:=Nothing, _
                              r_vAccountId:=Nothing, _
                              r_vElementId:=Nothing, _
                              r_vParentNodeId:=lParentNodeId, _
                              r_vResultArray:=Nothing, _
                              r_vRecordCount:=Nothing)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    lNextNodeId = lParentNodeId
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Loop

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLedgerOfNode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerOfNode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListLoad
    '
    ' Description: Standard method for the PickList control to call
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function PickListLoad(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0

        Try

            With m_oDatabase
                .Parameters.Clear()

                'Load the parameters

                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Next iParam

                'Call the SP
                m_lReturn = .SQLSelect("{call spu_ACTSecurity_PLL" & _
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
    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys As Object) As Integer

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

                m_lReturn = .SQLAction("{call spu_ACTSecurity_PLD" & _
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
                        m_lReturn = .SQLAction("{call spu_ACTSecurity_PLS" & _
                                    sPickListType & "(" & PickListParams(vFKArray) & ",?)}", sPickListType & " PickList Load", True)

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
End Class
