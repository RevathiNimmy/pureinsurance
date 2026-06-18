Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Automated_NET.Automated")>
Public NotInheritable Class Automated
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Automated
    '
    ' Date: 28/04/1997
    '
    ' Description: Creatable Automated class which contains all the
    '              Automated methods which can be called by Navigator.
    '
    ' Edit History: TF280497  - Created
    '               TF091297 - Import to Pinstripe bits removed
    '               PH250298 - Removed transaction_type_code parameter
    '                          from CreateStatsFolder
    '               TR011003 - Ran RemoveGlobals App and recompiled
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 01/10/2003
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
    Private Const ACClass As String = "Automated"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Calling Application Name
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

    ' Primary Keys to work with
    Private m_lInsuranceFileCnt As Integer
    Private m_lClaimID As Integer
    Private m_lWorkClaimID As Integer
    Private m_sClaimRef As String = ""
    Private m_lPerilID As Integer
    Private m_sDebitCredit As String = ""
    Private m_sDocumentComment As String = ""
    Private m_sDebitTransLedgerCode As String = ""
    Private m_sDebitAccountLedgerCode As String = ""
    Private m_sCreditTransLedgerCode As String = ""
    Private m_sCreditAccountLedgerCode As String = ""
    Private m_cTransactionAmount As Decimal
    Private m_lTransactionTypeID As Integer
    Private m_sTransactionTypeCode As String = ""
    Private m_lDocumentTypeID As Integer
    Private m_lDebitAccountID As Integer
    Private m_lCreditAccountID As Integer
    Private m_sCreditAccountMappingCode As String = ""
    Private m_sDebitAccountMappingCode As String = ""
    Private m_nIsCloned As Integer
    Private m_nIsClonedReversal As Integer



    Public Property DebitAccountMappingCode() As String
        Get
            Return m_sDebitAccountMappingCode
        End Get
        Set(ByVal Value As String)
            m_sDebitAccountMappingCode = Value
        End Set
    End Property

    Public Property CreditAccountMappingCode() As String
        Get
            Return m_sCreditAccountMappingCode
        End Get
        Set(ByVal Value As String)
            m_sCreditAccountMappingCode = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Standard Property.
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
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
    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public WriteOnly Property DebitCredit() As String
        Set(ByVal Value As String)
            m_sDebitCredit = Value
        End Set
    End Property
    Public WriteOnly Property DocumentComment() As String
        Set(ByVal Value As String)
            m_sDocumentComment = Value
        End Set
    End Property
    Public WriteOnly Property DebitTransLedgerCode() As String
        Set(ByVal Value As String)
            m_sDebitTransLedgerCode = Value
        End Set
    End Property
    Public WriteOnly Property DebitAccountLedgerCode() As String
        Set(ByVal Value As String)
            m_sDebitAccountLedgerCode = Value
        End Set
    End Property
    Public WriteOnly Property CreditTransLedgerCode() As String
        Set(ByVal Value As String)
            m_sCreditTransLedgerCode = Value
        End Set
    End Property
    Public WriteOnly Property CreditAccountLedgerCode() As String
        Set(ByVal Value As String)
            m_sCreditAccountLedgerCode = Value
        End Set
    End Property
    Public WriteOnly Property TransactionAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_cTransactionAmount = Value
        End Set
    End Property
    Public WriteOnly Property ClaimID() As Integer
        Set(ByVal Value As Integer)
            m_lClaimID = Value
            'get claim number
            m_lReturn = GetClaimRef(v_lClaimID:=Value, r_sClaimRef:=m_sClaimRef)
        End Set
    End Property
    Public WriteOnly Property WorkClaimID() As Integer
        Set(ByVal Value As Integer)
            m_lWorkClaimID = Value
        End Set
    End Property
    Public WriteOnly Property PerilID() As Integer
        Set(ByVal Value As Integer)
            m_lPerilID = Value
        End Set
    End Property
    Public WriteOnly Property TransactionTypeID() As Integer
        Set(ByVal Value As Integer)
            m_lTransactionTypeID = Value
        End Set
    End Property

    Public WriteOnly Property TransactionTypeCode() As String
        Set(ByVal Value As String)
            m_sTransactionTypeCode = Value
        End Set
    End Property
    Public WriteOnly Property DocumentTypeID() As Integer
        Set(ByVal Value As Integer)
            m_lDocumentTypeID = Value
        End Set
    End Property
    Public WriteOnly Property DebitAccountID() As Integer
        Set(ByVal Value As Integer)
            m_lDebitAccountID = Value
        End Set
    End Property
    Public WriteOnly Property CreditAccountID() As Integer
        Set(ByVal Value As Integer)
            m_lCreditAccountID = Value
        End Set
    End Property
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Public Property IsCloned() As Integer
        Get
            Return m_nIsCloned
        End Get
        Set(value As Integer)
            m_nIsCloned = value
        End Set
    End Property
    Public Property IsClonedReversal() As Integer
        Get
            Return m_nIsClonedReversal
        End Get
        Set(value As Integer)
            m_nIsClonedReversal = value
        End Set
    End Property

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
            ' Set Username and Password
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

            '    'Orion database
            '    m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, _
            ''        m_iLanguageID, v_lPMProductFamily:=pmePFOrion, _
            ''        r_oDatabase:=m_oDatabase)
            '    If (m_lReturn <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If

            ' Set Username and Password
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Informations.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                End Select

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFileCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAccountId
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetAccountId(ByRef r_lAccountID As Integer, ByVal v_sAccountCode As String) As Integer
        Return GetAccountId(r_lAccountID:=r_lAccountID, v_sAccountCode:=v_sAccountCode, v_lPartyCnt:=0)
    End Function

    Public Function GetAccountId(ByRef r_lAccountID As Integer, ByVal v_lPartyCnt As Integer) As Integer
        Return GetAccountId(r_lAccountID:=r_lAccountID, v_sAccountCode:="", v_lPartyCnt:=v_lPartyCnt)
    End Function

    Public Function GetAccountId(ByRef r_lAccountID As Integer) As Integer
        Return GetAccountId(r_lAccountID:=r_lAccountID, v_sAccountCode:="", v_lPartyCnt:=0)
    End Function

    Public Function GetAccountId(ByRef r_lAccountID As Integer, ByVal v_sAccountCode As String, ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAccountId"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            lReturn = CType(AddInputParameter(v_sName:="short_code", v_vValue:=v_sAccountCode, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            lReturn = CType(AddInputParameter(v_sName:="account_key", v_vValue:=v_lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAccountIdSQL, sSQLName:=kGetAccountIdName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetAccountIdSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' Check our resultset, both calls should return the same field
            If Informations.IsArray(vResults) Then
                ' Return the account id

                r_lAccountID = CInt(vResults(0, 0))
            Else
                ' Return not found
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


    '' ***************************************************************** '
    '' Name: GetAccountID (public)
    ''
    '' Description: get account id
    ''
    '' History:
    ''   22/03/2001 Created - Tinny
    ''   10/12/2002 PWF - Fixed party -> account link
    ''                    Added comments and informative error handling
    '' ***************************************************************** '
    'Public Function GetAccountId( _
    ''             ByRef r_lAccountID As Long, _
    ''    Optional ByVal v_sAccountCode As String, _
    ''    Optional ByVal v_lPartyCnt As Long) As Long
    '
    'Dim vResultArray As Variant
    '
    'Try:
    '    On Error GoTo Catch:
    '
    '    GetAccountId = PMTrue
    '
    '
    '
    '    ' We have an account short code...
    '    m_oDatabase.Parameters.Clear
    '
    '    lReturn = AddInputParameter(v_sName:="short_code", v_vValue:=v_sAccountCode, v_iType:=PMString)
    '    lReturn = AddInputParameter(v_sName:="account_key", v_vValue:=v_lPartyCnt, v_iType:=PMLong)
    '
    '    ' Execute short_code -> account_id query
    '    lReturn = m_oDatabase.SQLSelect( _
    ''                    sSQL:=ACSelAccountIDSQL, _
    ''                    sSQLName:=ACSelAccountIDName, _
    ''                    bStoredProcedure:=True, _
    ''                    vResultArray:=vResultArray)
    '    If lREturn <> PMTrue tehn
    '
    '
    '
    '    ' Check for valid parameters
    '    Select Case True
    '        Case Len(v_sAccountCode)
    '            ' We have an account short code...
    '            m_oDatabase.Parameters.Clear
    '
    '            ' Add parameters
    '            m_lReturn = m_oDatabase.Parameters.Add( _
    ''                sName:="short_code", _
    ''                vValue:=v_sAccountCode, _
    ''                idirection:=PMParamInput, _
    ''                iDataType:=PMString)
    '
    '            If (m_lReturn <> PMTrue) Then
    '                Err.Raise vbObjectError, "GetAccountID", "Unable to add parameter 'short_code'"
    '            End If
    '
    '            ' Execute short_code -> account_id query
    '            m_lReturn = m_oDatabase.SQLSelect( _
    ''                sSQL:=ACSelAccountIDSQL, _
    ''                sSQLName:=ACSelAccountIDName, _
    ''                bStoredProcedure:=True, _
    ''                vResultArray:=vResultArray)
    '
    '            If (m_lReturn <> PMTrue) Then
    '                Err.Raise vbObjectError, "GetAccountID", "Error " & m_lReturn & " executing ACSelAccountIDSQL"
    '            End If
    '
    '            If IsArray(vResultArray) Then
    '                Err.Raise vbObjectError, "GetAccountId", ACSelAccountIDSQL & " failed to return any details"
    '            End If
    '
    '            r_lAccountID = vResultArray(1, 0)
    '
    '        Case v_lPartyCnt
    '            ' We have a party_cnt...
    '            m_oDatabase.Parameters.Clear
    '
    '            ' Add parameters
    '            m_lReturn = m_oDatabase.Parameters.Add( _
    ''                sName:="account_key", _
    ''                vValue:=v_lPartyCnt, _
    ''                idirection:=PMParamInput, _
    ''                iDataType:=PMString)
    '
    '            If (m_lReturn <> PMTrue) Then
    '                Err.Raise vbObjectError, "GetAccountID", "Unable to add parameter 'account_key'"
    '            End If
    '
    '            ' Execute account_key (party_cnt) -> account_id query
    '            m_lReturn = m_oDatabase.SQLSelect( _
    ''                sSQL:=ACSelAccountIDForAccountKeySQL, _
    ''                sSQLName:=ACSelAccountIDForAccountKeyName, _
    ''                bStoredProcedure:=ACSelAccountIDForAccountKeyStored, _
    ''                vResultArray:=vResultArray)
    '
    '            If (m_lReturn <> PMTrue) Then
    '                Err.Raise vbObjectError, "GetAccountID", "Error " & m_lReturn & " executing ACSelAccountIDForAccountKeySQL"
    '            End If
    '
    '        Case Else
    '            ' Neither, raise this as an error
    '            Err.Raise vbObjectError, "GetAccountID", "No account_code or party_cnt supplied"
    '
    '    End Select
    '
    '    ' Check our resultset, both calls should return the same field
    '    If IsArray(vResultArray) Then
    '        ' Return the account id
    '        r_lAccountID = vResultArray(0, 0)
    '    Else
    '        ' Return not found
    '        GetAccountId = PMNotFound
    '    End If
    '
    '    GoTo Finally:
    '
    '    '----------------------------------------------------------------------------------------
    '    'Only for Debugging, the code will never execute this line
    '    '----------------------------------------------------------------------------------------
    '    Resume 0
    '
    'Catch:
    '    Select Case Err.Number
    '        Case vbObjectError
    '            ' Log Error.
    '            LogMessage m_sUsername, _
    ''                iType:=PMLogOnError, _
    ''                sMsg:=Err.Description, _
    ''                vApp:=ACApp, _
    ''                vClass:=ACClass, _
    ''                vMethod:="GetAccountID"
    '
    '            GetAccountId = PMFalse
    '
    '        Case Else
    '            ' Log Error.
    '            LogMessage m_sUsername, _
    ''                iType:=PMLogOnError, _
    ''                sMsg:="GetAccountID Failed", _
    ''                vApp:=ACApp, _
    ''                vClass:=ACClass, _
    ''                vMethod:="GetAccountID", _
    ''                vErrNo:=Err.Number, _
    ''                vErrDesc:=Err.Description
    '
    '            GetAccountId = PMError
    '
    '    End Select
    '
    'Finally:
    '
    'End Function

    ' ***************************************************************** '
    ' Name: ProcessJournal (public)
    ' Description: create stats details, create Document, TransDetail
    '              and send journal to orion
    'Hist : 22/03/2001 Created - Tinny
    ' ***************************************************************** '
    Public Function ProcessJournal() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
        'Removed 01/10/03
    End Function

    ' ***************************************************************** '
    ' Name: ProcessTransactions (public)
    ' Description: create stats details, export details and send to orion
    ' Hist : 22/03/2001 Created - Tinny
    ' ***************************************************************** '
    Public Function ProcessTransactions() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
        'Removed 01/10/03
    End Function

    ' ***************************************************************** '
    ' Name: FinaliseStats (public)
    ' Description: create stats details, export details and send to orion
    ' Hist : 18/06/2001 Created - Tom
    '       14/09/01    RWH - Pass in stats_folder_cnt to stored proc.
    ' ***************************************************************** '
    Public Function FinaliseStats(ByRef v_lStatsFolderCnt As Integer) As Integer
        Return FinaliseStats(v_lStatsFolderCnt:=v_lStatsFolderCnt, r_bStatsSuppressed:=False)
    End Function

    Public Function FinaliseStats(ByRef v_lStatsFolderCnt As Integer, ByRef r_bStatsSuppressed As Boolean) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(m_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", vValue:=CStr(m_lTransactionTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", vValue:=m_sTransactionTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="bstatssuppressed", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="nIsCloned", vValue:=IsCloned, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nIsCloned_reversal", vValue:=IsClonedReversal, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACFinaliseStatsSQl, sSQLName:=ACFinaliseStatsName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("bstatssuppressed").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("bstatssuppressed").Value) Then
                m_oDatabase.Parameters.Item("bstatssuppressed").Value = 0
            End If

            r_bStatsSuppressed = m_oDatabase.Parameters.Item("bstatssuppressed").Value

            m_lReturn = m_oDatabase.SQLCommitTrans()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FinaliseStats Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FinaliseStats", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CreateStatsFolder (Private)
    '
    ' Description: create stats record in stats_folder
    '
    ' Hist : 22/03/2001 Created - Tinny
    ' ***************************************************************** '
    Public Function CreateStatsFolder(ByRef r_lStatsFolderCnt As Integer, ByVal v_sTransactionTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=m_lClaimID, v_iType:=gPMConstants.PMEDataType.PMLong)

            lReturn = AddInputParameter(v_sName:="transaction_type_code", v_vValue:=v_sTransactionTypeCode, v_iType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStatsFolderSQL, sSQLName:=ACGetStatsFolderName, bStoredProcedure:=True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_lStatsFolderCnt = CInt(vResultArray(0, 0))
                Return result
            End If

            'If we've got this far then there is no existing folder.
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="debit_credit", vValue:=m_sDebitCredit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_comment", vValue:=m_sDocumentComment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", vValue:=CStr(m_lTransactionTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", vValue:=m_sTransactionTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_name", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(m_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="documenttype_id", vValue:=CStr(m_lDocumentTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = BeginTrans()

            ' Execute Add Stats Folder SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsFolderSQL, sSQLName:=ACAddStatsFolderName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            ' Get the Cnt of the record inserted
            r_lStatsFolderCnt = m_oDatabase.Parameters.Item("stats_folder_cnt").Value

            If r_lStatsFolderCnt < 1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If


            Return CommitTrans()

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateStatsFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStatsFolder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***************************************************************** '
    ' Name: CreateStatsDetails (Private)
    '
    ' Description:
    '
    ' Hist : 22/03/2001 Created - Tinny
    '        02/07/2001 Updated to be public and added additional params
    '                   for reinsurer identity and share.
    '
    ' Note : stored procedure will delete both folder and details record if errored
    ' ***************************************************************** '
    Public Function CreateStatsDetails(ByVal v_lStatsFolderCnt As Integer, ByVal v_sStatsDetailType As String, ByVal v_lClassOfBusId As Integer, ByVal v_sClassOfBusCode As String, ByVal v_lRIPartyCnt As Integer, ByVal v_sRIShortName As String, ByVal v_lRIPartyType As Integer, ByVal v_sglRISharePercent As Single) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(m_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="peril_id", vValue:=CStr(m_lPerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_detail_type", vValue:=v_sStatsDetailType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="class_of_business_id", vValue:=CStr(v_lClassOfBusId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="class_of_business_code", vValue:=v_sClassOfBusCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_party_cnt", vValue:=CStr(v_lRIPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_shortname", vValue:=v_sRIShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_party_type", vValue:=CStr(v_lRIPartyType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_share_percent", vValue:=CStr(v_sglRISharePercent), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_amount", vValue:=CStr(m_cTransactionAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="documenttype_id", vValue:=CStr(m_lDocumentTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsDetailsSQL, sSQLName:=ACAddStatsDetailsName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateStatsDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStatsDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateExport (Private)
    '
    ' Description:
    '
    'Hist : 22/03/2001 Created - Tinny
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CreateExport) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CreateExport(ByRef r_lTransactionExportFolderCnt As Integer, ByVal v_lStatsFolderCnt As Integer, ByVal v_sDebitTransLedgerCode As String, ByVal v_sDebitAccountTypeCode As String, ByVal v_sDebitAccountMappingCode As String, ByVal v_sCreditTransLedgerCode As String, ByVal v_sCreditAccountTypeCode As String, ByVal v_sCreditAccountMappingCode As String, ByVal v_sClaimRef As String) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_lReturn = CreateExportFolder(v_lStatsFolderCnt:=v_lStatsFolderCnt, r_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = CreateExportDetails(v_lStatsFolderCnt:=v_lStatsFolderCnt, v_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt, v_sDebitTransLedgerCode:=v_sDebitTransLedgerCode, v_sDebitAccountTypeCode:=v_sDebitAccountTypeCode, v_sDebitAccountMappingCode:=v_sDebitAccountMappingCode, v_sCreditTransLedgerCode:=v_sCreditTransLedgerCode, v_sCreditAccountTypeCode:=v_sCreditAccountTypeCode, v_sCreditAccountMappingCode:=v_sCreditAccountMappingCode, v_sClaimRef:=v_sClaimRef)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateExport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExport", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CreateExportFolder (Private)
    '
    ' Description:
    '
    'Hist : 22/03/2001 Created - Tinny
    ' ***************************************************************** '
    Private Function CreateExportFolder(ByVal v_lStatsFolderCnt As Integer, ByRef r_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add ExportFolderCnt as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(r_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Params
        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = BeginTrans()

        ' Execute Add Trans Export Folder SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddExportFolderSQL, sSQLName:=ACAddExportFolderName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = RollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the Cnt of the record inserted
        r_lTransactionExportFolderCnt = m_oDatabase.Parameters.Item("transaction_export_folder_cnt").Value

        If r_lTransactionExportFolderCnt < 1 Then
            m_lReturn = RollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CommitTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = RollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateExportDetails (Private)
    '
    ' Description: create one debit and one credit in Transaction_Export_Detail
    '
    'Hist : 22/03/2001  Created - Tinny
    '       22/06/01    RWH     - Added extra mapping_code parameters.
    '
    ' Note : stored procedure will delete both export detail and export folder if errored
    ' ***************************************************************** '
    Private Function CreateExportDetails(ByVal v_lStatsFolderCnt As Integer, ByVal v_lTransactionExportFolderCnt As Integer, ByVal v_sDebitTransLedgerCode As String, ByVal v_sDebitAccountTypeCode As String, ByVal v_sDebitAccountMappingCode As String, ByVal v_sCreditTransLedgerCode As String, ByVal v_sCreditAccountTypeCode As String, ByVal v_sCreditAccountMappingCode As String, ByVal v_sClaimRef As String) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Debit_Transaction_Ledger_Code", vValue:=v_sDebitTransLedgerCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Debit_Account_Type_Code", vValue:=v_sDebitAccountTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Debit_Mapping_Code", vValue:=v_sDebitAccountMappingCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Credit_Transaction_Ledger_Code", vValue:=v_sCreditTransLedgerCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Credit_Account_Type_Code", vValue:=v_sCreditAccountTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Credit_Mapping_Code", vValue:=v_sCreditAccountMappingCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="spare", vValue:=v_sClaimRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute Add Trans Export Details SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddExportDetailsSQL, sSQLName:=ACAddExportDetailsName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name : GetStatsFolderDetail (Private)
    '
    ' Desc : get details from stats folder
    '
    ' Hist : 22/03/2001 Created - Tinny
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetStatsFolderDetail) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetStatsFolderDetail(ByVal v_lStatsFolderCnt As Integer, ByRef r_sDocRef As String, ByRef r_sDocComment As String, ByRef r_dtDocDate As Date, ByRef r_sInsuranceRef As String) As Integer
    '
    '
    'Dim result As Integer = 0
    'Dim vResultArray(,) As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'execute sql
    'm_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStatsFolderDetailsSQL, sSQLName:=ACGetStatsFolderDetailsName, bStoredProcedure:=True, vResultArray:=vResultArray)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'If Not Informations.IsArray(vResultArray) Then
    'Return gPMConstants.PMEReturnCode.PMNotFound
    'End If
    '
    'get data back

    'r_sDocRef = CStr(vResultArray(0, 0))

    'r_sDocComment = CStr(vResultArray(1, 0))

    'r_dtDocDate = CDate(vResultArray(2, 0))

    'r_sInsuranceRef = CStr(vResultArray(3, 0))
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStatsFolderDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStatsFolderDetail", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name : CreateDocument (Private)
    '
    ' Desc : create document which will be referenced in TransDetail table
    '
    ' Hist : 22/03/2001 Created - Tinny
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CreateDocument) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CreateDocument(ByVal v_lDocumentTypeID As Integer, ByVal v_sDocumentRef As String, ByVal v_sDocumentComment As String, ByVal v_dtDocumentDate As Date, ByRef r_lDocumentId As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="document_id", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="postingstatus_id", vValue:=CStr(1), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="documenttype_id", vValue:=CStr(v_lDocumentTypeID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="auditset_id", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="document_ref", vValue:=v_sDocumentRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="document_date", vValue:=DateTimeHelper.ToString(v_dtDocumentDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="created_date", vValue:=DateTimeHelper.ToString(DateTime.Now), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="authorised_date", vValue:=DateTimeHelper.ToString(DateTime.Now), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="comment", vValue:=v_sDocumentComment, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="write_off_reason_id", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'start transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'execute sql
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddDocumentSQL, sSQLName:=ACAddDocumentName, bStoredProcedure:=ACAddDocumentStored)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'get back document id
    'r_lDocumentId = m_oDatabase.Parameters.Item("document_id").Value
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name : CreateTransactionDetail (Private)
    '
    ' Desc : create transaction details
    '
    ' Hist : 22/03/2001 Created - Tinny
    '
    ' Note : convert amount to correct currency if and when its required
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CreateTransactionDetail) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CreateTransactionDetail(ByVal v_lDocumentID As Integer, ByVal v_lAccountID As Integer, ByVal v_cAmount As Decimal, ByVal v_lDocSequence As Integer, ByVal v_sComment As String, ByVal v_sInsuranceRef As String, ByVal v_sClaimRef As String) As Integer
    '
    'Dim result As Integer = 0
    'Dim lPeriodID, lLedgerID As Integer
    'Dim dtRefDate As Date
    'Dim sLedgerShortName As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'get ledger id for account
    'm_lReturn = GetLedgerID(v_lAccountID:=v_lAccountID, r_lLedgerID:=lLedgerID, r_sLedgerShortName:=sLedgerShortName)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get ledgerId for account (" & v_lAccountID & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTransactionDetail", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'get period details
    'm_lReturn = GetPostingPeriod(v_lLedgerID:=lLedgerID, r_dtRefDate:=dtRefDate, r_lPeriodID:=lPeriodID)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get posting period for ledger id (" & lLedgerID & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTransactionDetail", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Clear the Database Parameters Collection
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="postingstatus_id", vValue:=CStr(1), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=CStr(m_iCurrencyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="period_id", vValue:=CStr(lPeriodID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="document_id", vValue:=CStr(v_lDocumentID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="document_sequence", vValue:=CStr(v_lDocSequence), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="accounting_date", vValue:=DateTimeHelper.ToString(DateTime.Today), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="amount", vValue:=CStr(v_cAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="base_amount_unrounded", vValue:=CStr(v_cAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="fully_matched", vValue:=CStr(gPMConstants.PMEReturnCode.PMFalse), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="currency_amount", vValue:=CStr(v_cAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="currency_amount_unrounded", vValue:=CStr(v_cAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="currency_base_xrate", vValue:=CStr(1), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="euro_currency_id", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="euro_amount", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="euro_base_xrate", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="euro_ccy_xrate", vValue:=CStr(1), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="comment", vValue:=v_sComment, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_ref", vValue:=v_sInsuranceRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="operator_id", vValue:=CStr(m_iUserID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="purchase_order_no", vValue:="", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="purchase_invoice_no", vValue:="", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="department", vValue:="(None)", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="spare", vValue:=v_sClaimRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="ref_date", vValue:=DateTimeHelper.ToString(dtRefDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="ref_amount", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="ref_quantity", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="ref_units", vValue:="", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_ref_index", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="department_id", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'start transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddTransDetailsSQL, sSQLName:=ACAddTransDetailsName, bStoredProcedure:=ACAddTransDetailsStored)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.SQLCommitTrans()
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateTransactionDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTransactionDetail", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name : GetLedgerID (Private)
    '
    ' Desc : get ledger ID for account
    '
    ' Hist : 22/03/2001 Created - Tinny
    ' ***************************************************************** '
    Public Function GetLedgerID(ByVal v_lAccountID As Integer, ByRef r_lLedgerID As Integer, ByRef r_sLedgerShortName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelLedgerIDSQL, sSQLName:=ACSelLedgerIDName, bStoredProcedure:=True, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lLedgerID = CInt(vResultArray(0, 0))

            r_sLedgerShortName = CStr(vResultArray(1, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLedgerID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetPostingPeriod (Private)
    '
    ' Desc : get posting period for date
    '
    ' Hist : 22/03/2001 Created - Tinny
    ' ***************************************************************** '
    Private Function GetPostingPeriod(ByVal v_lLedgerID As Integer, ByRef r_dtRefDate As Date, ByRef r_lPeriodID As Integer) As Integer

        Dim result As Integer = 0
        Dim oObject As bACTPeriod.Form



        result = gPMConstants.PMEReturnCode.PMTrue



        oObject = New bACTPeriod.Form
        m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bACTPeriod.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPostingPeriod", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result

        End If


        m_lReturn = oObject.GetPostingPeriodForDate(dtDateInPeriod:=r_dtRefDate, lPeriodID:=r_lPeriodID, lLedgerID:=v_lLedgerID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If


        oObject.Dispose()

        oObject = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    ' Name : PostJournal (Private)
    '
    ' Desc : post journal to orion
    '
    ' Hist : 22/03/2001 Created - Tinny
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (PostJournal) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function PostJournal(ByVal v_lDocumentID As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim oObject As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '
    'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oObject, v_sClassName:="bACTDocumentPost.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel)
    '
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bACTDocumentPost.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="PostJournal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    'End If
    '

    'm_lReturn = oObject.PostDocument(v_lDocumentID:=v_lDocumentID)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = oObject.Terminate()
    '
    'oObject = Nothing
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostJournal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostJournal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name : SendToOrion (Private)
    '
    ' Desc : post data to orion
    '
    ' Hist : 22/03/2001 Created - Tinny
    ' ***************************************************************** '
    Private Function SendToOrion(ByVal v_lTransactionExportFolderCnt As Integer) As Integer
        Return SendToOrion(v_lTransactionExportFolderCnt:=v_lTransactionExportFolderCnt, r_lDocumentId:=0)
    End Function

    Private Function SendToOrion(ByVal v_lTransactionExportFolderCnt As Integer, ByRef r_lDocumentId As Integer) As Integer

        Dim result As Integer = 0
        Dim oTransactionBusiness As bPMBTransactions.Automated



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = UpdateExportStatus(v_lTransExportFolderCnt:=v_lTransactionExportFolderCnt, v_sStatus:="f")



        oTransactionBusiness = New bPMBTransactions.Automated
        m_lReturn = oTransactionBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bPMBTransactions.Automated", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set the process modes

        m_lReturn = oTransactionBusiness.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            oTransactionBusiness.Dispose()
            oTransactionBusiness = Nothing
            Return result
        End If


        'Call the function to Send the Transaction to Orion

        m_lReturn = oTransactionBusiness.SendToOrion(v_lTransactionExportFolderCnt, r_lDocumentId:=r_lDocumentId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            result = gPMConstants.PMEReturnCode.PMFalse
        ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        Else
            m_lReturn = UpdateExportStatus(v_lTransExportFolderCnt:=v_lTransactionExportFolderCnt, v_sStatus:="c")

        End If


        oTransactionBusiness.Dispose()

        oTransactionBusiness = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    ' Name : GetClaimRef (Private)
    '
    ' Desc : get claim reference number
    '
    ' Hist : 28/03/2001 Created - Tinny
    ' ***************************************************************** '
    Private Function GetClaimRef(ByVal v_lClaimID As Integer, ByRef r_sClaimRef As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelClaimRefSQL, sSQLName:=ACSelClaimRefName, bStoredProcedure:=True, vResultArray:=vResultArray)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        r_sClaimRef = CStr(vResultArray(0, 0))

        Return result

    End Function

    ' ***************************************************************** '
    ' Name : UpdateExportStatus (Private)
    '
    ' Desc : update transaction export folder
    '
    ' Hist : 18/04/2001 Created - Tinny
    ' ***************************************************************** '
    Private Function UpdateExportStatus(ByVal v_lTransExportFolderCnt As Integer, ByVal v_sStatus As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="statuscode", vValue:=v_sStatus, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="exportfoldercnt", vValue:=CStr(v_lTransExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLBeginTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdTransExportFolderSQL, sSQLName:=ACUpdTransExportFolderName, bStoredProcedure:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = m_oDatabase.SQLRollbackTrans()
            Return result
        End If

        m_lReturn = m_oDatabase.SQLCommitTrans()

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

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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


    '' ***************************************************************** '
    ''
    '' Name: GetWorkStatsFolderId
    ''
    '' Description:
    ''
    '' History: 04/07/2001 RWH - Created.
    ''
    '' ***************************************************************** '
    'Public Function GetWorkStatsFolderId(r_lStatsFolderId As Long) As Long
    'Dim sSQL As String
    'Dim vResultArray As Variant
    '
    '    On Error GoTo Err_GetWorkStatsFolderId
    '
    '    GetWorkStatsFolderId = PMTrue
    '
    '    sSQL = "SELECT stats_folder_id" & vbCrLf
    '    sSQL = sSQL & "FROM work_stats_folder" & vbCrLf
    '    sSQL = sSQL & "WHERE loss_id = " & m_lClaimID
    '
    '    m_oDatabase.Parameters.Clear
    '
    '    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, _
    ''                                      sSQLName:="GetWorkStatsDetailId", _
    ''                                      bStoredProcedure:=False, _
    ''                                      vResultArray:=vResultArray)
    '    If (m_lReturn <> PMTrue) Then
    '        GetWorkStatsFolderId = PMFalse
    '        Exit Function
    '    End If
    '
    '    If Not IsArray(vResultArray) Then
    '        GetWorkStatsFolderId = PMNotFound
    '        Exit Function
    '    End If
    '
    '    'get data back
    '    r_lStatsFolderId = vResultArray(0, 0)
    '
    '    Exit Function
    '
    'Err_GetWorkStatsFolderId:
    '
    '    GetWorkStatsFolderId = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetWorkStatsFolderId Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetWorkStatsFolderId", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    '
    ' Name: CreateStatsForCoinsReins
    '
    ' Description:
    '
    ' History: 05/07/2001 RWH - Created.
    '           14/09/01  RWH - Pass in stats_folder_cnt to stored prgPMComponentServices.
    '
    ' ***************************************************************** '
    Public Function CreateStatsForCoinsReins(ByRef v_lStatsFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(m_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(14/09/01) Pass in stats_folder_cnt to proc.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsCoinsSQL, sSQLName:=ACAddStatsCoinsName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(m_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(14/09/01) Pass in stats_folder_cnt to proc.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="documenttype_id", vValue:=CStr(m_lDocumentTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsReinsSQL, sSQLName:=ACAddStatsReinsName, bStoredProcedure:=ACAddStatsReinsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateStatsForCoinsReins Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStatsForCoinsReins", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' CreateTransactions
    ''' </summary>
    ''' <param name="v_lStatsFolderCnt"></param>
    ''' <param name="r_lDocumentId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function CreateTransactions(ByVal v_lStatsFolderCnt As Integer) As Integer
        Return CreateTransactions(v_lStatsFolderCnt:=v_lStatsFolderCnt, r_lDocumentId:=0)
    End Function

    Public Function CreateTransactions(ByVal v_lStatsFolderCnt As Integer, ByRef r_lDocumentId As Integer) As Integer

        Dim nResult As Integer
        Dim nTransactionExportFolderCnt As Integer

        Try

            nResult = PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            If IsCloned = 1 OrElse IsClonedReversal = 1 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="nIsCloned", vValue:=1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(nTransactionExportFolderCnt), iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMLong)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddTransClaimsSQL, sSQLName:=ACAddTransClaimsName, bStoredProcedure:=ACAddTransClaimsStored)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            nTransactionExportFolderCnt = m_oDatabase.Parameters.Item("transaction_export_folder_cnt").Value

            'Call the routine to post transaction to the Orion
            m_lReturn = SendToOrion(v_lTransactionExportFolderCnt:=nTransactionExportFolderCnt, r_lDocumentId:=r_lDocumentId)

            If m_lReturn = PMEReturnCode.PMNotFound Then
                Return PMEReturnCode.PMNotFound
            ElseIf m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="CreateTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetStatsFolderForClaim
    '
    ' Description:
    '
    ' History: 15/08/2001 RWH - Created.
    '           14/09/01    RWH - Return array of folder cnts.
    ' ***************************************************************** '
    Public Function GetStatsFolderForClaim(ByRef r_vStatsFolderCnt As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object  = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=m_lClaimID, v_iType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetStatsFolderForClaimSQL, sSQLName:=kGetStatsFolderForClaimName, bStoredProcedure:=True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If



            r_vStatsFolderCnt = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStatsFolderForClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStatsFolderForClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName & _
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
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
End Class
