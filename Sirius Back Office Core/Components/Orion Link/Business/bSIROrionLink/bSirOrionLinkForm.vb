Option Strict Off
Option Explicit On
Imports SSP.Shared
'Developer Guide No. 129
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 31/03/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a SirOrionLink.
    '
    ' Edit History:
    ' SJP04072002 Account Key now party count to allow for more than 8 branches
    ' RAW 14/02/2003 : ISS2153 : added new function GetAccountFromParty to eventually replace GetAccountIDs
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 14/01/2004
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

    ' Component Services object

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

    'EK 5/11/99
    Dim m_lSalesNodeId As Integer
    Dim m_lPurchaseNodeId As Integer
    Dim m_lInsurerNodeID As Integer
    Dim m_lAgentNodeID As Integer
    Dim m_lFeeNodeID As Integer
    Dim m_lCommissionNodeID As Integer
    Dim m_lDiscountNodeID As Integer
    Dim m_lPremiumFinanceNodeID As Integer

    'KB PN 6138
    Dim m_vMultiStructureTree As gPMConstants.PMEReturnCode
    Dim m_iPartyCompany As Integer
    Dim oACTExplorer As Object ' need this globally
    Private m_lPostingAccountId As Integer

    ' Primary Keys to work with
    ' Party Cnt
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    'RWH(21/06/01)
    Private m_lPostingPeriodNumber As Integer
    'sj 15/07/2002 - start
    ' TransactionExportFolderCnt
    Private m_lTransactionExportFolderCnt As Integer
    'sj 15/07/2002 - end
    Private m_lJournalExportFolderCnt As Integer

    'PN37017 When Premium is zero and earned commission is set when client pays then set this property to TRUE
    Private m_bIsPostCommission As Boolean

    'sj 15/07/2002 - start
    Public Property TransactionExportFolderCnt() As Integer
        Get
            Return m_lTransactionExportFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lTransactionExportFolderCnt = Value
        End Set
    End Property
    'sj 15/07/2002 - end

    Public Property JournalExportFolderCnt() As Integer
        Get
            Return m_lJournalExportFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lJournalExportFolderCnt = Value
        End Set
    End Property

    'RWH(21/06/01)
    Public Property PostingPeriodNumber() As Integer
        Get
            Return m_lPostingPeriodNumber
        End Get
        Set(ByVal Value As Integer)
            m_lPostingPeriodNumber = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

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

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property ShortName() As String
        Get

            Return m_sShortName

        End Get
        Set(ByVal Value As String)

            m_sShortName = Value

        End Set
    End Property

    Public WriteOnly Property IsPostCommission() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsPostCommission = Value

        End Set
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
                End If
                m_oDatabase = Nothing
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

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' RAW 14/02/2003 : ISS2153 : added
    ' ***************************************************************** '
    ' Name: GetAccountFromParty
    '
    ' Description: Gets account details for a Party
    '
    ' ***************************************************************** '
    Public Function GetAccountFromParty(ByVal v_lPartyCnt As Integer, ByRef r_lAccountID As Integer, Optional ByRef r_sAccountTypeCode As String = "", Optional ByRef r_sLedgerTypeCode As String = "", Optional ByRef r_sLedgerCode As String = "") As Integer

        Const ksMyProcedureName As String = "GetAccountFromParty"
        Dim lMyReturn As gPMConstants.PMEReturnCode
        'Dim lReturn As Integer

        Dim vResultArray(,) As Object = Nothing
        Const klResultArrayCol_AccountID As Integer = 0
        Const klResultArrayCol_AccountTypeCode As Integer = 1
        'Const klResultArrayCol_LedgerID As Integer = 2
        Const klResultArrayCol_LedgerCode As Integer = 3
        Const klResultArrayCol_LedgerTypeCode As Integer = 4

        Dim lAccountID As Integer
        Dim sAccountTypeCode, sLedgerTypeCode, sLedgerCode As String

        Try

            lMyReturn = gPMConstants.PMEReturnCode.PMTrue

            ' Validate incoming parameters

            If v_lPartyCnt <= 0 Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="An invalid PartyCnt has been passed to this function", vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)

                lMyReturn = gPMConstants.PMEReturnCode.PMFalse
                Return lMyReturn
            End If

            ' Now do the business

            With m_oDatabase
                .Parameters.Clear()

                ' load the parameters
                'PN6169 New parameter for companyid
                m_lReturn = .Parameters.Add(sName:="v_iCompanyId", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                '

                m_lReturn = .Parameters.Add(sName:="v_iPartyCnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'run the sql

                m_lReturn = .SQLSelect(sSQL:=ACGetAccountFromPartyCntSQL, sSQLName:=ACGetAccountFromPartyCntName, bStoredProcedure:=ACGetAccountFromPartyCntStored, vResultArray:=vResultArray)

                ' did it work

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    lMyReturn = gPMConstants.PMEReturnCode.PMFalse
                    Return lMyReturn
                End If

                If Not Informations.IsArray(vResultArray) Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to return any account rows for partycnt_id " & v_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)

                    lMyReturn = gPMConstants.PMEReturnCode.PMNotFound
                    Return lMyReturn
                End If

                If CDbl(vResultArray(klResultArrayCol_AccountID, 0)) > 0 Then
                    ' good we have found an account
                Else
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid account_id returned for partycnt_id " & v_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)

                    lMyReturn = gPMConstants.PMEReturnCode.PMFalse
                    Return lMyReturn
                End If

            End With

            ' Extract details from result array - row 0 only

            lAccountID = CInt(vResultArray(klResultArrayCol_AccountID, 0))

            sAccountTypeCode = CStr(vResultArray(klResultArrayCol_AccountTypeCode, 0))

            sLedgerTypeCode = CStr(vResultArray(klResultArrayCol_LedgerTypeCode, 0))

            sLedgerCode = CStr(vResultArray(klResultArrayCol_LedgerCode, 0))

            ' set return params
            r_lAccountID = lAccountID
            If (Not False) Then r_sAccountTypeCode = sAccountTypeCode
            If (Not False) Then r_sLedgerTypeCode = sLedgerTypeCode
            If (Not False) Then r_sLedgerCode = sLedgerCode
            Return lMyReturn

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ksMyProcedureName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    lMyReturn = gPMConstants.PMEReturnCode.PMError
                    Return lMyReturn
            End Select

        Finally


        End Try
        Return lMyReturn
    End Function
    ' RAW 14/02/2003 : ISS2153 : added

    ' ***************************************************************** '
    ' Name: GetAccountIDs (Public)
    '
    ' Description: Gets AccountIDs from Orion for given PartyCnt.
    ' ***************************************************************** '
    'EK 5/11/99 Extra ledger accounts
    'DC 15/12/04 added introducer ledger
    ' ============================================================================================
    ' PLEASE NOTE
    ' Function GetAccountFromParty has been added as an alternative to this next function.
    ' The process is much simpler and makes use of the prescence of account_key on the account
    ' record to hold party_cnt
    ' At the moment it only accepts PartyCnt but this can be extended to include short_name
    ' in the future if required.
    ' It will return an account code for any ledger type - which is a limitation with this function
    ' ============================================================================================
    Public Function GetAccountIDs(ByRef r_lSalesAccountID As Integer, ByRef r_lPurchaseAccountID As Integer, ByRef r_lInsurerAccountID As Integer, ByRef r_lAgentAccountID As Integer, ByRef r_lFeeAccountID As Integer, ByRef r_lCommissionAccountID As Integer, ByRef r_lDiscountAccountID As Integer, ByRef r_lPremiumFinanceAccountID As Integer, ByRef r_lSubAgentAccountID As Integer, Optional ByRef r_lIntroducerAccountId As Integer = 0, Optional ByRef r_lNominalAccountID As Integer = 0, Optional ByRef r_lOtherPartyPayAccountID As Integer = 0, Optional ByRef r_lOtherPartyRecAccountID As Integer = 0, Optional ByVal v_vPartyCnt As Object = Nothing, Optional ByVal v_vPartyShortName As Object = Nothing, Optional ByVal v_vAccountKey As Object = Nothing, Optional ByRef r_iPostingCompany As Integer = 0) As Integer

        'PF251001 - Parameters changed to optional for Broking compatibility. No
        ' checks are performed for empty/nothing as the parameters are longs so
        ' we can't work it out anyway.
        '   r_lNominalAccountID
        '   r_lOtherPartyPayAccountID
        '   r_lOtherPartyRecAccountID

        'ECK PN6169 Added an optional parameter r_iPostingCompany for use in
        'multi-branch accounting

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sShortName As String = ""
        Dim oACTExplorer As bACTExplorer.Form
        Dim oACTAccount As bACTAccount.Form
        Dim vAccountIDArray(,) As Object = Nothing
        '
        Dim lLedgerId, lAccountID As Integer

        Dim lPartyID As Integer
        Dim iSourceID As Integer
        Dim lAccountKey As Integer

        Dim lSalesAccountId, lPurchaseAccountId, lInsurerAccountId, lAgentAccountId, lCommissionAccountId, lFeeAccountId, lDiscountAccountId, lPremiumFinanceAccountId, lSubAGentAccountId As Integer
        'Tomo22112000
        Dim lNominalAccountId As Integer
        'RWH(24/07/01) Other Party stuff.
        Dim lOtherPartyPayAccountId, lOtherPartyRecAccountId As Integer
        'DC151204
        Dim lIntroducerAccountId As Integer

        'EK9/9/99
        Dim vAccountArray(,) As Object = Nothing

        'DD 08/08/2002
        Dim lSubBranchID As Integer
        '
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Make sure some lookup details have been supplied

            If (Informations.IsNothing(v_vPartyCnt)) And (Informations.IsNothing(v_vPartyShortName)) And (Informations.IsNothing(v_vAccountKey)) Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Party id details supplied.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' No Party data required if Account Key supplied

            If Not Informations.IsNothing(v_vAccountKey) Then

                lAccountKey = CInt(v_vAccountKey)

                'DJM 02/03/2004
                'Get the source id for the branch that the party is stored under.

                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(CInt(v_vAccountKey)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyKeyFromPartyCntSQL, sSQLName:=ACGetPartyKeyFromPartyCntName, bStoredProcedure:=ACGetPartyKeyFromPartyCntStored, vResultArray:=vAccountArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get PartyKey from PartyCnt '" & CStr(v_vPartyCnt) & "'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lPartyID = CInt(vAccountArray(0, 0))

                iSourceID = CInt(vAccountArray(1, 0))

                ' Get Account Key if ShortName supplied
            ElseIf (Not Informations.IsNothing(v_vPartyShortName)) Then
                With m_oDatabase
                    .Parameters.Clear()

                    m_lReturn = .Parameters.Add(sName:="shortname", vValue:=CStr(v_vPartyShortName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    'EK 09/09/99 Added Return Array
                    m_lReturn = .SQLSelect(sSQL:=ACGetPartyKeyFromShortnameSQL, sSQLName:=ACGetPartyKeyFromShortnameName, bStoredProcedure:=ACGetPartyKeyFromShortnameStored, vResultArray:=vAccountArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get PartyKey from Shortname '" & CStr(v_vPartyShortName) & "'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If .Records.Count() < 1 Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Shortname '" & CStr(v_vPartyShortName) & "' not found on Party table.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        'GetAccountIDs = PMFalse
                        Return result
                    End If
                    'EK 9/9/99 Replaced these which didn't work
                    '            lPartyID& = .Parameters.Item("party_id").Value
                    '            iSourceID% = .Parameters.Item("source_id").Value

                    lPartyID = CInt(vAccountArray(0, 0))

                    iSourceID = CInt(vAccountArray(1, 0))

                    '   SJP 04072002 - Account Key is now = Party Count
                    '       Still passed into CalcCombinedKey but should just be
                    '           returned.

                    lAccountKey = CInt(vAccountArray(2, 0)) ' This is returned by the SQL
                    m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=iSourceID, v_lKeyID:=lPartyID, r_lCombinedKeyID:=lAccountKey)
                End With

                ' Get Account Key if PartyCnt supplied
            ElseIf (Not Informations.IsNothing(v_vPartyCnt)) Then
                With m_oDatabase
                    .Parameters.Clear()

                    m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(CInt(v_vPartyCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    'EK 09/09/99 Added Return Array
                    m_lReturn = .SQLSelect(sSQL:=ACGetPartyKeyFromPartyCntSQL, sSQLName:=ACGetPartyKeyFromPartyCntName, bStoredProcedure:=ACGetPartyKeyFromPartyCntStored, vResultArray:=vAccountArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get PartyKey from PartyCnt '" & CStr(v_vPartyCnt) & "'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'EK 9/9/99 Replaced these which didn't work
                    '            lPartyID& = .Parameters.Item("party_id").Value
                    '            iSourceID% = .Parameters.Item("source_id").Value

                    lPartyID = CInt(vAccountArray(0, 0))

                    iSourceID = CInt(vAccountArray(1, 0))

                    '   SJP 04072002 - Account Key is now = Party Count
                    '       Still passed into CalcCombinedKey but should just be
                    '           returned.

                    lAccountKey = CInt(v_vPartyCnt)
                    m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=iSourceID, v_lKeyID:=lPartyID, r_lCombinedKeyID:=lAccountKey)
                End With
            End If

            ' Cannot continue without valid Account Key
            If lAccountKey = 0 Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to establish valid Account Key.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DD 08/08/2002: Added for multi-branch accounting
            m_lReturn = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=lSubBranchID, v_vPartyCnt:=CStr(lAccountKey))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Get Sub Branch ID.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lSubBranchID = 0 Then
                lSubBranchID = 1
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="sub_branch_id is zero setting to 1", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs")
            End If

            ' Get Sales & Purchase Ledger IDs
            'EK 5/11/99
            'DC151204 added Introducer ledger
            m_lReturn = GetLedgerIDs(r_lSalesLedgerID:=lSalesAccountId, r_lPurchaseLedgerID:=lPurchaseAccountId, r_lInsurerLedgerId:=lInsurerAccountId, r_lAgentLedgerId:=lAgentAccountId, r_lFeeLedgerId:=lFeeAccountId, r_lCommissionLedgerId:=lCommissionAccountId, r_lDiscountLedgerId:=lDiscountAccountId, r_lPremiumFinanceLedgerId:=lPremiumFinanceAccountId, r_lSubAgentLedgerId:=lSubAGentAccountId, r_lNominalLedgerID:=lNominalAccountId, r_lOtherPartyPayLedgerID:=lOtherPartyPayAccountId, r_lOtherPartyRecLedgerID:=lOtherPartyRecAccountId, r_lIntroducerLedgerId:=lIntroducerAccountId, v_lSubBranchID:=m_iSourceID) 'eck PN6169
            '      v_lSubBranchID:=lSubBranchID)


            oACTExplorer = New bACTExplorer.Form
            m_lReturn = oACTExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bACTExplorer.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' Get Orion Account IDs
            'ECK PN6169 include the posting company

            m_lReturn = oACTExplorer.GetAccountIdFromKey(v_lKey:=lAccountKey, r_vAccountIds:=vAccountIDArray, v_iCompanyId:=m_iSourceID)

            oACTExplorer.Dispose()
            oACTExplorer = Nothing

            ' Exit if no accounts exist
            If Not Informations.IsArray(vAccountIDArray) Then
                'No we now need to consider the multi-structure tree & create new accounts if appropriate
                'are we multi-structure tree?
                m_lReturn = bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=CStr(m_vMultiStructureTree))

                If m_vMultiStructureTree = gPMConstants.PMEReturnCode.PMTrue Then
                    'ECK PN6169 this may be because we are raising a policy in a company for which the
                    'client doesn't have an account. If so we must create one.

                    m_lReturn = CreatePostingCompanyAccount(lAccountKey, iSourceID, lAccountID)
                    If lAccountID <> 0 Then
                        ReDim vAccountIDArray(0, 0)

                        vAccountIDArray(0, 0) = lAccountID
                    Else
                        Return result
                    End If
                Else

                    Return result
                End If
            End If

            ' Allocate IDs to correct ledgers

            oACTAccount = New bACTAccount.Form
            m_lReturn = oACTAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bACTAccount.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'TF140802 - Need to set ID same as Key
            lAccountID = lAccountKey

            For iCount As Integer = 0 To vAccountIDArray.GetUpperBound(1)
                ' Get Account details from Orion

                m_lReturn = oACTAccount.GetDetails(vAccountID:=vAccountIDArray(0, iCount))

                m_lReturn = oACTAccount.GetNext(vLedgerId:=lLedgerId, vAccountID:=lAccountID)
                Select Case lLedgerId
                    Case Is = lSalesAccountId
                        r_lSalesAccountID = lAccountID
                    Case Is = lPurchaseAccountId
                        r_lPurchaseAccountID = lAccountID
                        'EK 5/11/99 Extra Account Type
                    Case Is = lInsurerAccountId
                        r_lInsurerAccountID = lAccountID
                    Case Is = lAgentAccountId
                        r_lAgentAccountID = lAccountID
                    Case Is = lFeeAccountId
                        r_lFeeAccountID = lAccountID
                    Case Is = lCommissionAccountId
                        r_lCommissionAccountID = lAccountID
                    Case Is = lDiscountAccountId
                        r_lDiscountAccountID = lAccountID
                    Case Is = lPremiumFinanceAccountId
                        r_lPremiumFinanceAccountID = lAccountID
                    Case Is = lSubAGentAccountId
                        r_lSubAgentAccountID = lAccountID
                        'Tomo22112000
                    Case Is = lNominalAccountId
                        r_lNominalAccountID = lAccountID

                        'RWH(24/07/01) Other Party stuff.
                    Case Is = lOtherPartyPayAccountId
                        r_lOtherPartyPayAccountID = lAccountID
                    Case Is = lOtherPartyRecAccountId
                        r_lOtherPartyRecAccountID = lAccountID
                        'DC151204
                    Case Is = lIntroducerAccountId
                        r_lIntroducerAccountId = lAccountID

                End Select
            Next iCount

            oACTAccount.Dispose()
            oACTAccount = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetAccountIDs.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDs", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLedgerIDs (Public)
    '
    ' Description: Gets LedgerIDs from Orion
    ' ***************************************************************** '
    'EK 5/11/99 lots more parameters
    'EK 16/11/99 Added subagent ledger
    'DD 08/08/2002: Added SubBranchID for Multi-Branch Accounting
    'DC151204 added introducer ledger
    Public Function GetLedgerIDs(ByRef r_lSalesLedgerID As Integer, ByRef r_lPurchaseLedgerID As Integer, ByRef r_lInsurerLedgerId As Integer, ByRef r_lAgentLedgerId As Integer, ByRef r_lFeeLedgerId As Integer, ByRef r_lCommissionLedgerId As Integer, ByRef r_lDiscountLedgerId As Integer, ByRef r_lPremiumFinanceLedgerId As Integer, ByRef r_lSubAgentLedgerId As Integer, ByRef r_lNominalLedgerID As Integer, ByRef r_lOtherPartyPayLedgerID As Integer, ByRef r_lOtherPartyRecLedgerID As Integer, ByRef r_lIntroducerLedgerId As Integer, ByVal v_lSubBranchID As Integer) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oACTLedger As bACTLedger.Form
        Dim lLedgerId As Integer
        Dim sLedgerShortName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of Orion Account Ledger class

            oACTLedger = New bACTLedger.Form
            m_lReturn = oACTLedger.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the Account Ledger object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DD 08/08/2002: New parameter for multi-branch accounting

            m_lReturn = oACTLedger.GetDetails(vSubBranchID:=v_lSubBranchID)

            For iCount As Integer = 1 To oACTLedger.RecordCount

                m_lReturn = oACTLedger.GetNext(vLedgerID:=lLedgerId, vLedgerShortName:=sLedgerShortName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Next Account Ledger object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
                'EK 24/11/99 Modified to use constants
                Select Case sLedgerShortName.Substring(0, 1)
                    Case gSIRLibrary.SIRACTSalesLedgerShortName
                        r_lSalesLedgerID = lLedgerId
                    Case gSIRLibrary.SIRACTPurchaseLedgerShortName
                        r_lPurchaseLedgerID = lLedgerId
                        'EK 16/11/99 Added subagent
                    Case gSIRLibrary.SIRACTInsurerLedgerShortName
                        r_lInsurerLedgerId = lLedgerId
                    Case gSIRLibrary.SIRACTAgentLedgerShortName
                        r_lAgentLedgerId = lLedgerId
                    Case gSIRLibrary.SIRACTFeeLedgerShortName
                        r_lFeeLedgerId = lLedgerId
                    Case gSIRLibrary.SIRACTCommissionLedgerShortName
                        r_lCommissionLedgerId = lLedgerId
                    Case gSIRLibrary.SIRACTDiscountLedgerShortName
                        r_lDiscountLedgerId = lLedgerId
                    Case gSIRLibrary.SIRACTPremiumFinanceLedgerShortName
                        r_lPremiumFinanceLedgerId = lLedgerId
                    Case gSIRLibrary.SIRACTSubAgentLedgerShortName
                        r_lSubAgentLedgerId = lLedgerId
                        'Tomo 22112000
                    Case gSIRLibrary.SIRACTNominalLedgerShortName
                        r_lNominalLedgerID = lLedgerId

                        'RWH(24/07/01) Other Party stuff.
                    Case gSIRLibrary.SIRACTOtherPartySingleCharCode
                        Select Case (sLedgerShortName.Trim())
                            Case gSIRLibrary.SIRACTOtherPartyRecLedgerShortName
                                r_lOtherPartyRecLedgerID = lLedgerId
                            Case gSIRLibrary.SIRACTOtherPartyPayLedgerShortName
                                r_lOtherPartyPayLedgerID = lLedgerId
                        End Select
                        'DC141204
                    Case gSIRLibrary.SIRACTIntroducerLedgerShortname
                        r_lIntroducerLedgerId = lLedgerId

                End Select

            Next iCount

            oACTLedger.Dispose()
            oACTLedger = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetLedgerIDs.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerIDs", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'eck110400
    'Pass Document Source
    ' ***************************************************************** '
    ' Name: PostDocument (Public)
    '
    ' Description:
    ' ***************************************************************** '
    'sj 01/08/2002 - Add InsuranceFileCnt and Reason as parameters
    Public Function PostDocument(ByVal v_sDocRef As String, ByVal v_sDocDebitCredit As String, ByVal v_sDocTransactionTypeCode As String, ByVal v_dtDocDate As Date, ByVal v_dtDocAccountingDate As Date, ByVal v_sDocComments As String, ByVal v_sDocCurrencyCode As String, ByVal v_sDocBusinessTypeCode As String, ByVal v_sDocInsuranceRef As String, ByVal v_sDocProductCode As String, ByVal v_sDocBranchCode As String, ByVal v_sDocLeadAgentShortName As String, ByVal v_sDocInsuranceHolderShortName As String, ByVal v_dtDocInsuranceEffectiveDate As Date, ByVal v_iDocOperatorID As Integer, ByVal v_vTransactionsArray(,) As Object, ByVal v_iDocIsPayableByInstalments As Integer, ByVal v_lDocInsuranceHolderKey As Integer, ByVal v_lDocAgentKey As Integer, ByRef r_lDocPostedStatus As Integer, ByVal v_iDocSourceID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sReason As String, Optional ByRef r_vNewDocumentId As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing, Optional ByRef r_sFailureReason As String = "") As Integer

        Dim result As Integer = 0
        Dim sShortName As String = ""
        Dim oOrion As bACTImportSiriusTrans.Business
        Dim lSalesAccountId, lPurchaseAccountId, lInsurerAccountId, lAgentAccountId, lCommissionAccountId As Integer
        Dim lFeeAccountId, lPremiumFinanceAccountId, lSubAGentAccountId, lAccountKey As Integer
        Dim lIntroducerAccountId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Make sure all ledgers exist
            For iCount As Integer = v_vTransactionsArray.GetLowerBound(1) To v_vTransactionsArray.GetUpperBound(1)

                'TF291099 - Array will now contain AccountID or zero
                'EK 9/11/99 - If no key was passed no need to create an account

                If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountKey, iCount)) > 1 Then

                    sShortName = CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportRelativeCode, iCount))

                    lAccountKey = CInt(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountKey, iCount))
                    If sShortName.Trim().ToUpper() = v_sDocInsuranceHolderShortName.Trim().ToUpper() Then
                        lAccountKey = v_lDocInsuranceHolderKey
                    End If

                    If sShortName.Trim().ToUpper() = v_sDocLeadAgentShortName.Trim().ToUpper() Then
                        lAccountKey = v_lDocAgentKey
                    End If

                    Select Case CStr(v_vTransactionsArray(ACTBatchConst.ACTTransImportLedgerCode, iCount)).Substring(0, 2)
                        Case "SL"

                            If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iCount)) < 1 Then
                                m_lReturn = CreateOrionAccounts(r_lAccountID:=lSalesAccountId, v_sLedgerFlag:=gSIRLibrary.SIRACTSalesLedgerShortName, v_lAccountKey:=lAccountKey)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMError
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccounts Failed for ledger " & gSIRLibrary.SIRACTSalesLedgerShortName & " AccountKey " & CStr(lAccountKey), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")

                                    Return result
                                End If
                            End If
                        Case "PL"

                            If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iCount)) < 1 Then
                                m_lReturn = CreateOrionAccounts(r_lAccountID:=lPurchaseAccountId, v_sLedgerFlag:=gSIRLibrary.SIRACTPurchaseLedgerShortName, v_lAccountKey:=lAccountKey)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMError
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccounts Failed for ledger " & gSIRLibrary.SIRACTPurchaseLedgerShortName & " AccountKey " & CStr(lAccountKey), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")

                                    Return result
                                End If
                            End If

                        Case "IN"

                            If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iCount)) < 1 Then
                                m_lReturn = CreateOrionAccounts(r_lAccountID:=lInsurerAccountId, v_sLedgerFlag:=gSIRLibrary.SIRACTInsurerLedgerShortName, v_lAccountKey:=lAccountKey)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMError
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccounts Failed for ledger " & gSIRLibrary.SIRACTInsurerLedgerShortName & " AccountKey " & CStr(lAccountKey), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")

                                    Return result
                                End If
                            End If

                        Case "AG"

                            If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iCount)) < 1 Then
                                m_lReturn = CreateOrionAccounts(r_lAccountID:=lAgentAccountId, v_sLedgerFlag:=gSIRLibrary.SIRACTAgentLedgerShortName, v_lAccountKey:=lAccountKey)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMError
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccounts Failed for ledger " & gSIRLibrary.SIRACTAgentLedgerShortName & " AccountKey " & CStr(lAccountKey), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")

                                    Return result
                                End If
                            End If

                        Case "CO"

                            If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iCount)) < 1 Then
                                m_lReturn = CreateOrionAccounts(r_lAccountID:=lCommissionAccountId, v_sLedgerFlag:=gSIRLibrary.SIRACTCommissionLedgerShortName, v_lAccountKey:=lAccountKey)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMError
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccounts Failed for ledger " & gSIRLibrary.SIRACTCommissionLedgerShortName & " AccountKey " & CStr(lAccountKey), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")

                                    Return result
                                End If
                            End If

                        Case "FE"

                            If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iCount)) < 1 Then
                                m_lReturn = CreateOrionAccounts(r_lAccountID:=lFeeAccountId, v_sLedgerFlag:=gSIRLibrary.SIRACTFeeLedgerShortName, v_lAccountKey:=lAccountKey)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMError
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccounts Failed for ledger " & gSIRLibrary.SIRACTFeeLedgerShortName & " AccountKey " & CStr(lAccountKey), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")

                                    Return result
                                End If
                            End If

                        Case "PF"

                            If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iCount)) < 1 Then
                                m_lReturn = CreateOrionAccounts(r_lAccountID:=lPremiumFinanceAccountId, v_sLedgerFlag:=gSIRLibrary.SIRACTPremiumFinanceLedgerShortName, v_lAccountKey:=lAccountKey)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMError
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccounts Failed for ledger " & gSIRLibrary.SIRACTPremiumFinanceLedgerShortName & " AccountKey " & CStr(lAccountKey), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")

                                    Return result
                                End If
                            End If

                        Case "SB"

                            If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iCount)) < 1 Then
                                m_lReturn = CreateOrionAccounts(r_lAccountID:=lSubAGentAccountId, v_sLedgerFlag:=gSIRLibrary.SIRACTSubAgentLedgerShortName, v_lAccountKey:=lAccountKey)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMError
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccounts Failed for ledger " & gSIRLibrary.SIRACTSubAgentLedgerShortName & " AccountKey " & CStr(lAccountKey), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")

                                    Return result
                                End If
                            End If

                        Case "TR"

                            If CDbl(v_vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, iCount)) < 1 Then
                                m_lReturn = CreateOrionAccounts(r_lAccountID:=lIntroducerAccountId, v_sLedgerFlag:=gSIRLibrary.SIRACTIntroducerLedgerShortname, v_lAccountKey:=lAccountKey)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMError
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccounts Failed for ledger " & gSIRLibrary.SIRACTIntroducerLedgerShortname & " AccountKey " & CStr(lAccountKey), vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")

                                    Return result
                                End If
                            End If

                    End Select
                End If

            Next iCount

            ' Create an instance of the Orion Import object

            oOrion = New bACTImportSiriusTrans.Business
            m_lReturn = oOrion.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bACTImportSiriusTrans.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")
                Return result
            End If

            'RWH(21/06/01) Use Posting Period from transaction export folder.

            oOrion.PostingPeriodNumber = m_lPostingPeriodNumber

            'PN37017 Set for move commission when premium is zero and earned commission is set when client pays

            oOrion.IsPostCommission = m_bIsPostCommission

            'Journals or Transactions?
            If m_lJournalExportFolderCnt = 0 Then
                'Process as before

                oOrion.TransactionExportFolderCnt = m_lTransactionExportFolderCnt

            Else
                'IMPORTANT - Setting the JournalExportFolderCnt property notifies
                'bACTImportSiriusTrans.Business that we are processing Journals
                '(providing it is not 0 which it will never be in a real world scenario)
                With oOrion

                    .JournalExportFolderCnt = m_lJournalExportFolderCnt
                    '...if that is the ase then we don't need the TransactionExportFolderCnt

                    .TransactionExportFolderCnt = 0
                End With
            End If

            'Multi Branch
            ' Send Document info to Orion
            'sj 01/08/2002 - Add InsuranceFileCnt and Reason as parameters

            m_lReturn = oOrion.PostDocument(v_sDocRef:=v_sDocRef, v_sDocDebitCredit:=v_sDocDebitCredit, v_sDocTransactionTypeCode:=v_sDocTransactionTypeCode, v_dtDocDate:=v_dtDocDate, v_dtDocAccountingDate:=v_dtDocAccountingDate, v_sDocComments:=v_sDocComments, v_sDocCurrencyCode:=v_sDocCurrencyCode, v_sDocBusinessTypeCode:=v_sDocBusinessTypeCode, v_sDocInsuranceRef:=v_sDocInsuranceRef, v_sDocProductCode:=v_sDocProductCode, v_sDocBranchCode:=v_sDocBranchCode, v_sDocLeadAgentShortName:=v_sDocLeadAgentShortName, v_sDocInsuranceHolderShortName:=v_sDocInsuranceHolderShortName, v_dtDocInsuranceEffectiveDate:=v_dtDocInsuranceEffectiveDate, v_iDocOperatorID:=v_iDocOperatorID, v_vTransactionsArray:=v_vTransactionsArray, r_lDocPostedStatus:=r_lDocPostedStatus, v_iDocSourceID:=v_iDocSourceID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sReason:=v_sReason, r_vNewDocumentId:=r_vNewDocumentId, v_vTermsOfPaymentId:=v_vTermsOfPaymentId, v_vPaymentDueDate:=v_vPaymentDueDate, r_sfailureReason:=r_sFailureReason)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oOrion.PostDocument Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocuments")
                oOrion = Nothing
                Return result
            End If

            'MSS210901 - Added for merge
            result = m_lReturn
            'MSS210901 - Merge end

            oOrion = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Post Document Details to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateOrionAccounts (Public)
    '
    ' Description:
    ' ***************************************************************** '
    Public Function CreateOrionAccounts(ByRef r_lAccountID As Integer, ByVal v_sLedgerFlag As String, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lAccountKey As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oParty As bSIRParty.Services
        Dim oOrionAccount As bACTAccount.Form
        Dim oOrionExplorer As bACTExplorer.Form
        Dim oOrionLedger As bACTLedger.Form

        Dim iSourceID As Integer
        Dim sShortName, sPartyType As String
        Dim lPartyID As Integer
        Dim sName As String = ""
        Dim iCurrencyID As Integer
        Dim sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode As String
        Dim iCountryID As Integer
        Dim sContactName, sContactTelAreaCode, sContactTelNumber, sContactTelExtension, sContactFaxAreaCode, sContactFaxNumber, sContactFaxExtension As String

        Dim lAccountID As Integer
        Dim iAccountTypeID As Integer
        Dim lLedgerId, lSalesLedgerID, lPurchaseLedgerID As Integer
        'EK 5/11/99 More ledgers
        Dim lInsurerLedgerId, lAgentLedgerID, lFeeLedgerId, lCommissionLedgerID, lDiscountLedgerID, lPRemiumFinanceLedgerId As Integer
        'EK 16/11/99
        Dim lSubAgentLedgerId As Integer
        'Tomo22112000
        Dim lNominalLedgerId As Integer
        'RWH(24/07/01) Other Party stuff.
        Dim lOtherPartyPayLedgerId, lOtherPartyRecLedgerId As Integer
        'DC151204
        Dim lIntroducerLedgerId As Integer

        Dim lNodeId, lElementId As Integer

        Dim lPartyCnt, lAccountKey As Integer

        Dim sSQL As String = ""

        'DD 08/08/2002
        Dim lSubBranchID As Integer

        'DD 23/08/2002
        Dim vValue As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the PartyAddressContact object

            oParty = New bSIRParty.Services
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bSIRParty.Services", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts")

                Return result
            End If

            If Informations.IsNothing(v_lPartyCnt) Then

                If Informations.IsNothing(v_lAccountKey) Then
                    result = gPMConstants.PMEReturnCode.PMError

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Party ID supplied.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts")

                    Return result
                Else

                    m_lReturn = GetPartyCntFromKey(v_lKey:=CInt(v_lAccountKey), r_lPartyCnt:=lPartyCnt)
                    v_lPartyCnt = lPartyCnt
                End If
            End If

            ' Cannot continue without valid PartyCnt

            If (Informations.IsNothing(v_lPartyCnt)) Or (v_lPartyCnt.Equals(0)) Or (v_lPartyCnt < 1) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to determine valid Party Cnt.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts")

                Return result
            End If

            With oParty

                .PartyCnt = v_lPartyCnt

                m_lReturn = .GetDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for  Party Cnt " & v_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts")

                    Return result
                End If

                iSourceID = .SourceID

                sShortName = .ShortName

                sPartyType = .PartyType

                lPartyID = .PartyID

                sName = .Name

                iCurrencyID = .CurrencyID

                sContactName = .ResolvedName

                sAddress1 = .Address1

                sAddress2 = .Address2

                sAddress3 = .Address3

                sAddress4 = .Address4

                sPostalCode = .PostalCode
                iCountryID = 0 '.AddressCountryID

                sContactTelAreaCode = .AreaCode

                sContactTelNumber = .Number

                sContactTelExtension = .Extension

                sContactFaxAreaCode = ""
                sContactFaxNumber = ""
                sContactFaxExtension = ""
            End With

            ' Destroy Party object

            oParty.Dispose()
            oParty = Nothing

            ' Make sure Account Key is set

            If Not Informations.IsNothing(v_lAccountKey) Then

                lAccountKey = CInt(v_lAccountKey)
            Else
                '   SJP 04072002 - Account Key is now = Party Count
                '       Still passed into CalcCombinedKey but should just be
                '           returned.
                lAccountKey = v_lPartyCnt
                m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=iSourceID, v_lKeyID:=lPartyID, r_lCombinedKeyID:=lAccountKey)
            End If

            ' Create Orion objects
            ' Create an instance of the Orion Account object

            oOrionAccount = New bACTAccount.Form
            m_lReturn = oOrionAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bACTAccount.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' Create an instance of the Orion Ledger object

            oOrionLedger = New bACTLedger.Form
            m_lReturn = oOrionLedger.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bACTLedger.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' Create an instance of the Orion Account Explorer object

            oOrionExplorer = New bACTExplorer.Form
            m_lReturn = oOrionExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bACTExplorer.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'DD 23/08/2002: Added Product option for installations with only one
            'Accounts Structure Tree
            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, m_iSourceID, vValue)
            If gPMFunctions.NullToString(vValue) = "1" Then
                'DD 08/08/2002: Added for multi-branch accounting
                m_lReturn = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=lSubBranchID, v_vPartyCnt:=CStr(v_lPartyCnt))
            Else
                'Hard coded for performance reasons. Sub Branch ID 1 will
                'always be the branch under which the tree is linked to.
                lSubBranchID = 1
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Get Sub Branch ID.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add new ledger accounts
            'EK 5/11/99 More mapped ledgers
            'DC151204 added introducer ledger
            m_lReturn = GetLedgerIDs(r_lSalesLedgerID:=lSalesLedgerID, r_lPurchaseLedgerID:=lPurchaseLedgerID, r_lInsurerLedgerId:=lInsurerLedgerId, r_lAgentLedgerId:=lAgentLedgerID, r_lFeeLedgerId:=lFeeLedgerId, r_lCommissionLedgerId:=lCommissionLedgerID, r_lDiscountLedgerId:=lDiscountLedgerID, r_lPremiumFinanceLedgerId:=lPRemiumFinanceLedgerId, r_lSubAgentLedgerId:=lSubAgentLedgerId, r_lNominalLedgerID:=lNominalLedgerId, r_lOtherPartyPayLedgerID:=lOtherPartyPayLedgerId, r_lOtherPartyRecLedgerID:=lOtherPartyRecLedgerId, r_lIntroducerLedgerId:=lIntroducerLedgerId, v_lSubBranchID:=lSubBranchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLedgerIDs Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'EK 5/11/99 Use a case statement here
            Select Case v_sLedgerFlag
                Case gSIRLibrary.SIRACTSalesLedgerShortName
                    iAccountTypeID = ACTConst.ACTAccountTypeAsset
                    lLedgerId = lSalesLedgerID
                Case gSIRLibrary.SIRACTPurchaseLedgerShortName
                    iAccountTypeID = ACTConst.ACTAccountTypeLiability
                    lLedgerId = lPurchaseLedgerID
                Case "I"
                    iAccountTypeID = ACTConst.ACTAccountTypeLiability
                    lLedgerId = lInsurerLedgerId

                Case "A"
                    iAccountTypeID = ACTConst.ACTAccountTypeLiability
                    lLedgerId = lAgentLedgerID

                Case "F"
                    iAccountTypeID = ACTConst.ACTAccountTypeLiability
                    lLedgerId = lFeeLedgerId

                Case "C"
                    iAccountTypeID = ACTConst.ACTAccountTypeLiability
                    lLedgerId = lCommissionLedgerID

                Case "D"
                    iAccountTypeID = ACTConst.ACTAccountTypeLiability
                    lLedgerId = lDiscountLedgerID

                Case "R"
                    iAccountTypeID = ACTConst.ACTAccountTypeLiability
                    lLedgerId = lPRemiumFinanceLedgerId
                    '16/11/99 SubAGent Ledger
                Case "U"
                    iAccountTypeID = ACTConst.ACTAccountTypeAsset
                    lLedgerId = lSubAgentLedgerId

                    'Tomo22112000
                Case "N"
                    iAccountTypeID = ACTConst.ACTAccountTypeLiability 'Total guess
                    lLedgerId = lNominalLedgerId

                    'RWH(24/07/01) Other Party stuff.
                Case gSIRLibrary.SIRACTOtherPartyPayLedgerShortName
                    iAccountTypeID = ACTConst.ACTAccountTypeLiability
                    lLedgerId = lOtherPartyPayLedgerId

                Case gSIRLibrary.SIRACTOtherPartyRecLedgerShortName
                    iAccountTypeID = ACTConst.ACTAccountTypeAsset
                    lLedgerId = lOtherPartyRecLedgerId

                    'DC151204
                Case "T"
                    iAccountTypeID = ACTConst.ACTAccountTypeLiability
                    lLedgerId = lIntroducerLedgerId

            End Select
            ' CF120399 - Added vAccountStatusID:=ACTAccountStatusActive
            'eck110400
            'MultiBranch
            'MSS210901 - Changed from m_lAccountKey to lAccountKey to keep in line with SFORB

            m_lReturn = oOrionAccount.DirectAdd(vAccountId:=lAccountID, vPurgeFrequencyId:=ACTConst.ACTPurgeFreqNever, vCurrencyID:=iCurrencyID, vAccounttypeID:=iAccountTypeID, vLedgerId:=lLedgerId, vAccountName:=sName, vShortCode:=sShortName, vRestrictEnquiry:=False, vRestrictUpdate:=False, vDeleteAtPurge:=False, vContactName:=sContactName, vAddress1:=sAddress1, vAddress2:=sAddress2, vAddress3:=sAddress3, vAddress4:=sAddress4, vPostalCode:=sPostalCode, vAddressCountry:=iCountryID, vPhoneAreaCode:=sContactTelAreaCode, vPhoneNumber:=sContactTelNumber, vPhoneExtension:=sContactTelExtension, vFaxAreaCode:=sContactFaxAreaCode, vFaxNumber:=sContactFaxNumber, vFaxExtension:=sContactFaxExtension, vAccountKey:=lAccountKey, vAccountStatusID:=ACTConst.ACTAccountStatusActive, vPartySourceID:=iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oOrionAccount.DirectAdd Failed for " & sShortName.Trim() & "/" & sName.Trim(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts")

                ' Destroy the Orion object

                oOrionAccount.Dispose()
                oOrionAccount = Nothing

                Return result
            End If
            'TF291099 - Return new AccountID
            r_lAccountID = lAccountID
            '

            oOrionAccount.Dispose()
            oOrionAccount = Nothing

            ' Get Ledger Node ID
            'EK 5/11/99 Add Case statement
            'EK 140100 Was not passing correct Ledger mapping codes
            Dim sMessage As String = ""
            Select Case v_sLedgerFlag
                Case gSIRLibrary.SIRACTSalesLedgerShortName

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTClientLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTClientLedgerMappingCode
                Case gSIRLibrary.SIRACTPurchaseLedgerShortName

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTPurchaseLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTPurchaseLedgerMappingCode
                Case gSIRLibrary.SIRACTInsurerLedgerShortName

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTInsurerLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTInsurerLedgerMappingCode
                Case gSIRLibrary.SIRACTAgentLedgerShortName

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTAgentLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTAgentLedgerMappingCode
                Case gSIRLibrary.SIRACTFeeLedgerShortName

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTFeeLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTFeeLedgerMappingCode
                Case gSIRLibrary.SIRACTDiscountLedgerShortName

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTDiscountLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTDiscountLedgerMappingCode
                Case gSIRLibrary.SIRACTCommissionLedgerShortName

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTCommissionLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTCommissionLedgerMappingCode
                Case gSIRLibrary.SIRACTPremiumFinanceLedgerShortName

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTPremiumFinanceLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTPremiumFinanceLedgerMappingCode
                    'EK 16/11/99
                Case gSIRLibrary.SIRACTSubAgentLedgerShortName
                    'eck091100 - stupid bug
                    '        m_lReturn = oOrionLedger.GetLedgerNodeId("SIRACTSubAgentLedgerMappingCode", lNodeID)

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTSubAgentLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTSubAgentLedgerMappingCode
                    'Tomo22112000
                Case gSIRLibrary.SIRACTNominalLedgerShortName

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTNominalLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTNominalLedgerMappingCode
                    'RWH(24/07/01) Other Party stuff.
                Case gSIRLibrary.SIRACTOtherPartyPayLedgerShortName
                    m_lReturn = GetLedgerNodeIdFromCode(v_sLedgerFlag, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & v_sLedgerFlag

                Case gSIRLibrary.SIRACTOtherPartyRecLedgerShortName
                    m_lReturn = GetLedgerNodeIdFromCode(v_sLedgerFlag, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & v_sLedgerFlag
                    'DC151204
                Case gSIRLibrary.SIRACTIntroducerLedgerShortname

                    m_lReturn = oOrionLedger.GetLedgerNodeId(gSIRLibrary.SIRACTIntroducerLedgerMappingCode, lNodeId)
                    sMessage = "oOrionLedger.GetLedgerNodeId Failed for code " & gSIRLibrary.SIRACTIntroducerLedgerMappingCode
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oOrionLedger.Dispose()
            oOrionLedger = Nothing

            'Insert new element

            lElementId = oOrionExplorer.InsertElement(sShortName)

            If lElementId > 0 Then

                lNodeId = oOrionExplorer.InsertNode(lParentNodeId:=lNodeId, lElementId:=lElementId, vAccountId:=lAccountID)
            Else
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oOrionExplorer.InsertElement Failed for " & sShortName, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOrionAccounts")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oOrionExplorer.Dispose()
            oOrionExplorer = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateOrionAccounts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:  UpdateAccExportStatus (Public)
    '
    ' Description: Updates the Accounts Export Status.
    '
    ' Changes: Added optional paramter to allow update of Generic Journals
    '
    ' ***************************************************************** '

    Public Function UpdateAccExportStatus(ByVal v_lTransactionFolderCnt As Integer, ByVal v_sAccountsExportStatus As String) As Integer

        Dim result As Integer = 0
        Dim vDatabase As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC130503 -ISS4035 -incase it loses it

            vDatabase = DBNull.Value

            If m_oDatabase.Parameters Is Nothing Then

                m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'DC130503 -ISS4035

            'If the JournalExportFolderCnt property has not been set then we are processing transactions
            '(Ie default processing)
            If m_lJournalExportFolderCnt = 0 Then

                ' Set parameters for SQL call
                With m_oDatabase

                    ' Clear the Database Parameters Collection
                    .Parameters.Clear()

                    ' Add the Transaction Export Folder Cnt INPUT parameter
                    m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add the Accounts Export Status INPUT parameter
                    m_lReturn = .Parameters.Add(sName:="accounts_export_status", vValue:=v_sAccountsExportStatus, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Call the Update Status SQL
                    m_lReturn = .SQLAction(sSQL:=ACUpdateAccExportStatusSQL, sSQLName:=ACUpdateAccExportStatusName, bStoredProcedure:=ACUpdateAccExportStatusStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End With

            Else
                'The m_lJournalExportFolderCnt property has been set so
                'update the Journal_Export_Folder table instead

                ' Set parameters for SQL call
                With m_oDatabase

                    ' Clear the Database Parameters Collection
                    .Parameters.Clear()

                    ' Add the Transaction Export Folder Cnt INPUT parameter
                    m_lReturn = .Parameters.Add(sName:="journal_export_folder_cnt", vValue:=CStr(m_lJournalExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add the Accounts Export Status INPUT parameter
                    m_lReturn = .Parameters.Add(sName:="accounts_export_status", vValue:=v_sAccountsExportStatus, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Call the Update Status SQL
                    m_lReturn = .SQLAction(sSQL:=ACUpdateJournalExportStatusSQL, sSQLName:=ACUpdateJournalExportStatusName, bStoredProcedure:=ACUpdateJournalExportStatusStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End With

            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the Accounts Export Status", vApp:=ACApp, vClass:=ACClass, vMethod:=" UpdateAccExportStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: GetPartyCntFromKey
    '
    ' Description: Get PartyCnt from derived Key [(Source * 2^21) + id]
    '
    ' ***************************************************************** '
    Private Function GetPartyCntFromKey(ByVal v_lKey As Integer, ByRef r_lPartyCnt As Integer) As Integer

        'Dim lSourceID As Long
        'Dim lPartyID As Long

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '   SJP 04072002 - Account Key is now = Party Count
        '       Therefore we can save a database call
        r_lPartyCnt = v_lKey

        '    ' Extract IDs from key
        '    m_lReturn& = gPMComponentServices.UncalcCombinedKey( _
        ''        r_lSourceID:=lSourceID&, _
        ''        r_lKeyID:=lPartyID&, _
        ''        v_lCombinedKeyID:=v_lKey&)
        '
        '    ' Get PartyCnt from database
        '    With m_oDatabase
        '        .Parameters.Clear
        '
        '        m_lReturn& = .Parameters.Add( _
        ''            sName:="source_id", _
        ''            vValue:=CInt(lSourceID&), _
        ''            iDirection:=PMParamInput, _
        ''            iDataType:=PMInteger)
        '
        '        m_lReturn& = .Parameters.Add( _
        ''            sName:="party_id", _
        ''            vValue:=lPartyID&, _
        ''            iDirection:=PMParamInput, _
        ''            iDataType:=PMLong)
        '
        '        m_lReturn& = .SQLSelect( _
        ''            sSQL:=ACGetPartyCntFromKeySQL, _
        ''            sSQLName:=ACGetPartyCntFromKeyName, _
        ''            bStoredProcedure:=ACGetPartyCntFromKeyStored)
        '
        '        r_lPartyCnt& = .Records.Item(1).Fields.Item("party_cnt").Value
        '    End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetContactValues (Private)
    '
    ' Description: Reselects the contact details to populate accountcomm
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetContactValues) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetContactValues(ByRef lPartyCnt As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRecordCount, lCurrRecNo As Integer
    'Dim bTelFound, bFaxFound As Boolean
    'Dim oFields As ADODB.Fields
    '
    '
    '    ' Clear the Database Parameters Collection
    '    m_oDatabase.Parameters.Clear
    ''
    '    ' Add the ContactID parameter (INPUT)
    '    m_lReturn& = m_oDatabase.Parameters.Add( _
    ''            sName:="party_cnt", _
    ''            vValue:=lPartyCnt, _
    ''            iDirection:=PMParamInput, _
    ''            iDataType:=PMInteger)
    ''
    '    If (m_lReturn& <> PMTrue) Then
    '        GetContactValues = PMFalse
    '        Exit Function
    '    End If
    ''
    '    ' Execute SQL Statement
    '    m_lReturn& = m_oDatabase.SQLSelect( _
    ''        sSql:=ACSelectConForPartySQL, _
    ''        sSQLName:=ACSelectConForPartyName, _
    ''        bStoredProcedure:=ACSelectConForPartyStored, _
    ''        lNumberRecords:=0)
    ''
    '    If (m_lReturn& <> PMTrue) Then
    '        GetContactValues = PMFalse
    '        Exit Function
    '    End If
    ''
    '    ' How many records were selected
    '    lRecordCount& = m_oDatabase.Records.Count
    ''
    '    ' Do we have any records ?
    ''
    '    If (lRecordCount& < 1) Then
    ''
    '        ' No Records, return PMFalse
    '        GetContactValues = PMNotFound
    '        Exit Function
    '    End If
    ''
    '    'we want a telephone contact and a fax contact
    '    bTelFound = False
    '    bFaxFound = False
    ''
    '    For lCurrRecNo& = 1 To lRecordCount&
    ''
    '        ' Set oFields to refer to Field Collection for lCurrRecNo
    '        Set oFields = m_oDatabase.Records.Item(CVar(1)).Fields
    ''
    '        ' TF141097 - OrionLink properties
    '        If (bTelFound = False And Trim$(oFields.Item("Code").Value) = "Telephone") Then
    '            m_oOrionLink.ContactName = oFields.Item("Description").Value
    '            m_oOrionLink.ContactTelIntCode = oFields.Item("Telephone_code").Value
    '            m_oOrionLink.ContactTelAreaCode = oFields.Item("Area_code").Value
    '            m_oOrionLink.ContactTelNumber = oFields.Item("Number").Value
    '            bTelFound = True
    '        End If
    '        If (bFaxFound = False And Trim$(oFields.Item("Code").Value) = "Fax") Then
    '            m_oOrionLink.ContactFaxIntCode = oFields.Item("Telephone_code").Value
    '            m_oOrionLink.ContactFaxAreaCode = oFields.Item("Area_code").Value
    '            m_oOrionLink.ContactFaxNumber = oFields.Item("Number").Value
    '            bFaxFound = True
    '        End If
    ''
    '        If (bTelFound = True And bFaxFound = True) Then
    '            'exit the record loop if we have already found the contacts
    '            'because we only want one of each
    '            Exit For
    '        End If
    ''
    '    Next lCurrRecNo&
    ''
    '    Set oFields = Nothing
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    '
    '
    ' Error.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetContactValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    '
    'End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
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


    ' ***************************************************************** '
    '
    ' Name: GetLedgerNodeIdFromCode
    '
    ' Description:
    '
    ' History: 24/07/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function GetLedgerNodeIdFromCode(ByRef v_sLedgerShortName As String, ByRef r_lNodeId As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResult(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "SELECT st.node_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "FROM Ledger l, StructureTree st" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "WHERE l.ledger_short_name = '" & v_sLedgerShortName & "'" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND st.mapping_id = l.mapping_id"

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetLedgerNodeIdFromCode", bStoredProcedure:=False, vResultArray:=vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        r_lNodeId = CInt(vResult(0, 0))

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccountStatusFromAccountID
    '
    ' Description:
    '
    ' History: 10/12/2003 MKW - Created.
    '
    ' ***************************************************************** '
    Public Function GetAccountStatusFromAccountID(ByRef v_lAccountID As Integer, ByRef r_lAccountStatus As Integer) As Integer
        Dim result As Integer = 0
        Dim vResult(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAccountStatusFromAccountIDSQL, sSQLName:=ACGetAccountStatusFromAccountIDName, bStoredProcedure:=ACGetAccountStatusFromAccountIDStored, vResultArray:=vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_lAccountStatus = CInt(vResult(0, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountStatusFromAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountStatusFromAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Name: CreatePostingCompanyAccount
    '
    ' Description: For multibranch accounting
    '              If we are adding a policy for a company other than that to which the client
    '              belongs we must use the client's account in that company
    '              If the account does not exist this will create it.
    ' DJM 02/03/2004 : Replaced old version of this function with updated one from iPMBTransactions.
    ' ***************************************************************** '
    Private Function CreatePostingCompanyAccount(ByVal v_lPartyCnt As Integer, ByVal v_lSourceID As Integer, ByRef r_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim lAccountID As Integer
        Dim vAccountIDArray As Object
        Dim sAccountName As String = String.Empty
        Dim sShortName As String = String.Empty
        Dim lLedgerId, lMappingId, lNodeId, lElementId As Integer
        Dim oAccount As New bACTAccount.Form
        Dim oExplorer As New bACTExplorer.Form



        result = gPMConstants.PMEReturnCode.PMTrue

        'Initialise variables
        r_lAccountID = 0

        vAccountIDArray = (0)

        'Create business objects
        If oExplorer Is Nothing Then

            oExplorer = New bACTExplorer.Form
            m_lReturn = oExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Account", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If oAccount Is Nothing Then

            oAccount = New bACTAccount.Form
            m_lReturn = oAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Account", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If

        'Get the account ID for this party at it's original branch

        m_lReturn = oExplorer.GetAccountIDFromKey(v_lKey:=v_lPartyCnt, r_vAccountIDs:=vAccountIDArray, v_iCompanyId:=v_lSourceID)

        lAccountID = CInt(vAccountIDArray(0, 0))

        'Create new account record for this branch.

        m_lReturn = oAccount.CreateAccountForCompany(vAccountId:=lAccountID, vAccountName:=sAccountName, vShortName:=sShortName, vLedgerId:=lLedgerId, vMappingId:=lMappingId, vCompanyId:=m_iSourceID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create Account", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePostingCompanyAccount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set return parameter to the new account id.
        r_lAccountID = lAccountID

        'Now create a record for the new account in the structure tree
        'First get the node for the client ledger for the new accoaunt

        lNodeId = oExplorer.GetNodeFromMappingID(v_lMappingId:=lMappingId)
        If lNodeId < 0 Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Node Id for Client Ledger", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePostingCompanyAccount")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Insert new element

        lElementId = oExplorer.InsertElement(sShortName)

        If lElementId > 0 Then

            lNodeId = oExplorer.InsertNode(lParentNodeId:=lNodeId, lElementId:=lElementId, vAccountId:=lAccountID)
        Else
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oOrionExplorer.InsertElement Failed for " & sShortName, vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePostingCompanyAccount")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Release the objects.

        oAccount.Dispose()
        oAccount = Nothing

        oExplorer.Dispose()
        oExplorer = Nothing

        Return result

    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

