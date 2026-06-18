Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    '******************************************************************************
    ' Class Name: FindAccount
    '
    ' Date: 06 May 1997
    '
    ' Description: Creatable class used by the FindAccount interface.
    '
    ' Edit History:
    '   PW250705 - PN22590 - Find account as part of Insurer Payments was returning
    '              all accounts following fix PN21417.
    '******************************************************************************

    ' ************************************************
    ' Added to replace global variables 08/12/2003
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

    Private Const ACIFullKey As Integer = 0
    Private Const ACIShortCode As Integer = 1
    Private Const ACIAccountName As Integer = 2
    Private Const ACILedgerID As Integer = 3
    Private Const ACIAccountType As Integer = 4
    Private Const ACIAccountID As Integer = 5
    'eck050302
    Private Const ACIBalance As Integer = 11

    ' Constants for use in SQL
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As Integer

    ' Task
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As New StringsHelper.FixedLengthString(10)
    Private m_dtEffectiveDate As Date

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Component Sub Type
    Private m_sSubType As New StringsHelper.FixedLengthString(20)
    ' Variable Data Business Component (Private)

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    ' CTAF 080101 - Replaced the use of Explorer with a stored procedue
    '               Therefore all instances of m_oExplorer have been commented out.
    ' Account Explorer for LongKeys
    'Private m_oExplorer As Object

    ' Instance of account
    Private m_oAccount As bACTAccount.Form

    ' NavigatorV3 variables
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer
    'eck130600
    Private m_iHasUnrestrictedEnquiry As Integer
    Private m_iHasUnrestrictedUpdate As Integer

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

    ' PUBLIC Property Procedures (End)

    ' PUBLIC Methods (Begin)

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

                m_sTransactionType.Value = CStr(vTransactionType)
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
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values as defined by vTableArray.
    '
    '
    ' ***************************************************************** '

    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vresultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'developer guide no.17

            ' Get the Lookup items from the Business Component

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageID, dtEffectiveDate:=m_dtEffectiveDate, vResultArray:=vresultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password
            m_iUserID = iUserID

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType.Value = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business Object passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

            ' CTAF 080101 - Replaced the use of Explorer with a stored procedue
            '               Therefore all instances of m_oExplorer have been commented out.
            '    m_lReturn& = gPMComponentServices.CreateBusinessObject( _
            ''        r_oObject:=m_oExplorer, _
            ''        v_sClassName:="bACTExplorer.Form", _
            ''        v_sCallingAppName:=ACApp, _
            ''        v_sUserName:=sUsername, _
            ''        v_sPassword:=sPassword, _
            ''        v_iUserID:=iUserID, _
            ''        v_iSourceID:=iSourceID, _
            ''        v_iLanguageID:=iLanguageID, _
            ''        v_iCurrencyID:=iCurrencyID, _
            ''        v_iLogLevel:=iLogLevel, _
            ''        v_oDatabase:=m_oDatabase)
            '    If m_lReturn <> PMTrue Then
            '      Initialise = m_lReturn
            '      Exit Function
            '    End If

            ' CTAF 291099

            m_oAccount = New bACTAccount.Form
            m_lReturn = m_oAccount.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
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
                If m_oAccount IsNot Nothing Then
                    m_oAccount.Dispose()
                    m_oAccount = Nothing
                End If
                m_oLookup = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    '****************************************************************** '
    '* Name: SelectAccountQuery (Public)
    '*
    '* Description: Selects accounts according to the query by given
    '*               parameters
    '* History: CTAF 130100 - Created to replace above and it's stored procedure.
    '****************************************************************** '
    'developer guide no. 101
    Public Function SelectAccountQuery(ByRef lNumberOfRecords As Integer, ByRef vresultArray(,) As Object, ByVal iCompanyID As Integer, Optional ByVal vFullKey As Object = Nothing, Optional ByVal vLedgerID As Object = Nothing, Optional ByVal vAccountName As Object = Nothing, Optional ByVal vAccountType As Object = Nothing, Optional ByVal vShortCode As Object = Nothing, Optional ByVal vInsuranceRef As Object = Nothing, Optional ByVal vOperatorID As Object = Nothing, Optional ByVal vPurchaseOrderNo As Object = Nothing, Optional ByVal vPurchaseInvoiceNo As Object = Nothing, Optional ByVal vSpare As Object = Nothing, Optional ByVal vShowDeleted As Object = Nothing, Optional ByVal vPaymentLedgerIDs As Object = Nothing, Optional ByVal vExcludeLedgerIDs As Object = Nothing, Optional ByVal vShowBalance As Object = Nothing, Optional ByVal vBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL1, sSQL2, sSQL, sShortCode As String
        Dim lAccountID As Integer
        'Developer Guide No 101
        Dim vValue As Object = Nothing

        'Developer Guide No 101
        Dim vCompanyId As Object
        Dim sUnderwritingOrAgency As String = ""
        Dim curBalance As Decimal
        Dim bLedgerIDProcessed As Boolean ' PN22590. 
        Dim sOptionValue As String = String.Empty

        Try
            ' Filter by Resticted branch
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=gPMConstants.PMSysOptionRestricteduserbranchOption, r_sOptionValue:=sOptionValue)

            'eck130600 Get the users authority

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the intial SQL
            'eck090600 Pass back the company id
            'eck050302 Pass empty variable to load the account balance into
            sSQL1 = "SELECT DISTINCT " &
                    "'Full Key', " &
                    "a.short_code, a.account_name, " &
                    "a.ledger_id, a.accounttype_id, a.account_id, " &
                    " ISNULL(pr.party_cnt, 0), a.nominal_account_id, " &
                    "ast.description, a.accountstatus_id, a.company_id,'', " &
                    "a.contact_name, a.address1, ppc.forename,a.currency_id,c.iso_code,a.company_id,s.code,pa.is_gross_agent " &
                    "FROM Account a " &
                    "INNER JOIN AccountStatus ast ON a.accountstatus_id = ast.accountstatus_id " &
                    "LEFT JOIN party_personal_client ppc on a.account_key = ppc.party_cnt " &
                    "JOIN Currency c on c.currency_id = a.currency_id " &
                    "JOIN source s on s.source_id = a.company_id " &
                    " LEFT JOIN party pr ON a.ACCOUNT_KEY = pr.PARTY_CNT " &
                    " LEFT JOIN party_agent pa ON pa.party_cnt=pr.PARTY_CNT "

            'DJM 11/12/2003 : Restrict by company_id for multi-company systems.
            sSQL2 = "WHERE" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "(" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "        EXISTS (SELECT * FROM hidden_options WHERE " &
                    "option_number = 16 AND value = 1)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "        AND a.company_id = " & CStr(iCompanyID) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "    )" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "    OR" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "        NOT EXISTS (SELECT * FROM hidden_options WHERE " &
                    "option_number = 16 AND value = 1)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "        AND a.company_id <> 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "    )" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & ")" & Strings.ChrW(13) & Strings.ChrW(10)

            If sOptionValue = "1" Then

                'sSQL2 = sSQL2 & " AND  A.company_id IN (SELECT S.Source_id FROM Source S LEFT JOIN PMUSER_Source U " & Strings.ChrW(13) & Strings.ChrW(10)
                'sSQL2 = sSQL2 & " ON S.source_id = U.source_id AND U.user_id = " & CStr(m_iUserID) & Strings.ChrW(13) & Strings.ChrW(10)
                'sSQL2 = sSQL2 & " WHERE U.source_id IS NULL) "

                '31779 - System account to exclude branch access system option
                sSQL2 = sSQL2 & " AND ((  a.company_id IN (SELECT S.Source_id FROM Source S LEFT JOIN PMUSER_Source U " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL2 = sSQL2 & " ON S.source_id = U.source_id AND U.user_id = " & CStr(m_iUserID) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL2 = sSQL2 & " WHERE U.source_id IS NULL) )"
                sSQL2 = sSQL2 & " OR (short_code in (select value from system_options where option_number in(150,151,152,153,5028,5061))))"
            End If

            ' Now decide what else we need to add
            ' PSL 10/06/2003 Iss4434 If it is called by Insurer Payment then subset of ledgers
            ' PN 21417 If LedgerID is there is should override everything else.
            ' Need to drop through to other checks if nothing is processed for
            ' vLedgerID. PN22590.

            If Not Informations.IsNothing(vLedgerID) Then

                If CInt(vLedgerID) <> -1 Then

                    sSQL2 = sSQL2 & "AND (a.ledger_id = " & CStr(vLedgerID) & ") "
                    bLedgerIDProcessed = True
                End If
            End If

            If bLedgerIDProcessed Then
            ElseIf (Not Informations.IsNothing(vPaymentLedgerIDs)) Then
                sSQL2 = sSQL2 & "AND (a.Ledger_id IN (" & vPaymentLedgerIDs & ")) "
            ElseIf (Not Informations.IsNothing(vExcludeLedgerIDs)) Then
                sSQL2 = sSQL2 & "AND (a.Ledger_id NOT IN (" & vExcludeLedgerIDs & ")) "
            End If

            If Not Informations.IsNothing(vAccountName) Then

                If CStr(vAccountName).Length > 0 Then
                    m_lReturn = CType(bPMFunc.ValidateSQL(vAccountName), gPMConstants.PMEReturnCode)
                    If (CStr(vAccountName).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (a.account_name = '" & CStr(vAccountName) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (a.account_name LIKE '" & CStr(vAccountName) & "') "
                    End If
                End If
            End If

            If Not Informations.IsNothing(vAccountType) Then
                If CInt(vAccountType) <> -1 Then
                    sSQL2 = sSQL2 & "AND (a.accounttype_id = " & vAccountType & ") "
                End If
            End If

            'CT 4/8/00 fix to allow accounts with apostrophies in to be found
            sShortCode = vShortCode
            m_lReturn = CType(bPMFunc.ValidateSQL(sShortCode), gPMConstants.PMEReturnCode)
            vShortCode = sShortCode
            'CT end

            If Not Informations.IsNothing(vShortCode) Then

                If vShortCode.Length > 0 Then
                    If (vShortCode.IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (a.short_code = '" & vShortCode & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (a.short_code LIKE '" & vShortCode & "') "
                    End If
                End If
            End If
            'eck130600
            If m_iHasUnrestrictedEnquiry = 0 Then
                sSQL2 = sSQL2 & "AND (a.restrict_enquiry = 0) "
            End If

            If Not Informations.IsNothing(vShowDeleted) Then
                If vShowDeleted > 0 Then
                    sSQL2 = sSQL2 & "AND 1 = isnull((select is_deleted from party where party_cnt = " &
                            "a.account_key),0) "
                Else
                    'DJM 10/09/2003 : Only show accounts that haven't been set to deleted in the party table.
                    sSQL2 = sSQL2 & "AND 0 = isnull((select is_deleted from party where party_cnt = " &
                            "a.account_key),0) "
                End If
            End If

            'CMG/PB if there are any populated fields searching the transdetail table then add it it.
            'If a single % is entered then NULLs wont be found, were remove it so its not added as a criteria
            If vInsuranceRef = "%" Then vInsuranceRef = ""
            If vPurchaseOrderNo = "%" Then vPurchaseOrderNo = ""
            If vPurchaseInvoiceNo = "%" Then vPurchaseInvoiceNo = ""
            If vSpare = "%" Then vSpare = ""

            If ((Convert.ToString(vInsuranceRef).Length > 0) OrElse (Convert.ToString(vOperatorID).Length > 0) OrElse (Convert.ToString(vPurchaseOrderNo).Length > 0) OrElse (Convert.ToString(vPurchaseInvoiceNo).Length > 0) OrElse (Convert.ToString(vSpare).Length > 0)) OrElse vBrokerCnt <> 0 Then
                sSQL1 = sSQL1 & ", TransDetail t "
                sSQL2 = sSQL2 & "AND t.Account_Id = a.Account_Id "
            End If

            If vBrokerCnt <> 0 Then
                sSQL1 = sSQL1 & ", Insurance_File ifi "
                sSQL2 = sSQL2 & " AND ifi.lead_agent_Cnt = " & vBrokerCnt & " "
            End If

            If vInsuranceRef IsNot Nothing AndAlso Convert.ToString(vInsuranceRef).Length > 0 Then
                If (vInsuranceRef.IndexOf("%"c) + 1) = 0 Then
                    sSQL2 = sSQL2 & "AND (EXISTS (SELECT * FROM insurance_file WITH(NOLOCK) WHERE Insurance_File.insurance_ref ='" & vInsuranceRef & "' AND (insured_cnt = pr.party_cnt OR collection_from_cnt = pr.party_cnt))) "
                Else
                    sSQL2 = sSQL2 & "AND (EXISTS (SELECT * FROM insurance_file WITH(NOLOCK) WHERE Insurance_File.insurance_ref LIKE '" & vInsuranceRef & "' AND (insured_cnt = pr.party_cnt OR collection_from_cnt = pr.party_cnt))) "
                    'sSQL2 = sSQL2 & "AND (t.insurance_ref LIKE '" & vInsuranceRef & "') "
                End If
            End If

            If vOperatorID IsNot Nothing AndAlso (CStr(vOperatorID)).Length > 0 Then

                sSQL2 = sSQL2 & "AND (t.operator_id = " & CStr(vOperatorID) & ") "
            End If
            If vPurchaseOrderNo IsNot Nothing AndAlso vPurchaseOrderNo.Length > 0 Then
                If (vPurchaseOrderNo.IndexOf("%"c) + 1) = 0 Then
                    sSQL2 = sSQL2 & "AND (t.purchase_order_no = '" & vPurchaseOrderNo & "') "
                Else
                    sSQL2 = sSQL2 & "AND (t.purchase_order_no LIKE '" & vPurchaseOrderNo & "') "
                End If
            End If
            If vPurchaseInvoiceNo IsNot Nothing AndAlso vPurchaseInvoiceNo.Length > 0 Then
                If (vPurchaseInvoiceNo.IndexOf("%"c) + 1) = 0 Then
                    sSQL2 = sSQL2 & "AND (t.purchase_invoice_no = '" & vPurchaseInvoiceNo & "') "
                Else
                    sSQL2 = sSQL2 & "AND (t.purchase_invoice_no LIKE '" & vPurchaseInvoiceNo & "') "
                End If
            End If
            If vSpare IsNot Nothing AndAlso vSpare.Length > 0 Then
                If (vSpare.IndexOf("%"c) + 1) = 0 Then
                    sSQL2 = sSQL2 & "AND (t.spare = '" & vSpare & "') "
                Else
                    sSQL2 = sSQL2 & "AND (t.spare LIKE '" & vSpare & "') "
                End If
            End If

            sSQL2 = sSQL2 & "ORDER BY short_code"

            sSQL = sSQL1 & sSQL2

            ' Perform the SQL query - but restrict the number of records returned
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectAccountQuery", bStoredProcedure:=False, lNumberRecords:=lNumberOfRecords, vResultArray:=vresultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SQL Failed : " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectAccountQuery")
                Return result
            End If

            ' Check that there's a result set.
            If Not Informations.IsArray(vresultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            If Not Informations.IsNothing(vShowBalance) Then
                If vShowBalance = 1 Then

                    'TR - Find out if Multibranch accounting is switched on
                    bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, m_iSourceID, vValue)
                    If gPMFunctions.NullToString(vValue) = "1" Then
                        'Multibranchin is ON - Filter by company Id
                        vCompanyId = m_iSourceID
                    Else

                        vCompanyId = Nothing
                    End If

                    'TR - Get all the Account balances
                    For lCount As Integer = vresultArray.GetLowerBound(1) To vresultArray.GetUpperBound(1)


                        lAccountID = CInt(vresultArray(ACIAccountID, lCount))
                        'Tracy Richards - Call a lighter/quicker version of the Fetch
                        'balance function for Underwriting

                        m_lReturn = m_oAccount.GetAccountBalanceLite(v_lAccountId:=lAccountID, v_vCompanyID:=vCompanyId, r_curBalance:=curBalance)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception()
                        End If
                        'TR - Put the resulting balance into the main array

                        vresultArray(ACIBalance, lCount) = curBalance
                    Next lCount
                End If
            End If
            'eck050302End

            Return result

        Catch ex As Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed on SelectAccountQuery", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectAccountQuery", excep:=ex)

            Return result
        End Try

    End Function


    '****************************************************************** '
    '* Name: SelectAccountQueryFiltered (Public)
    '*
    '* Description: Selects accounts according to the query by given
    '*              parameters and the group level security
    '*
    '* History:
    '*              DD 12/07/2002: Created from a copy of SelectAccountQuery
    '*
    '****************************************************************** '
    'Developer guide No 101
    Public Function SelectAccountQueryFiltered(ByRef lNumberOfRecords As Integer, ByRef vresultArray(,) As Object, ByVal iCompanyID As Integer, Optional ByVal vFullKey As Object = Nothing, Optional ByVal vLedgerID As Object = Nothing, Optional ByVal vAccountName As Object = Nothing, Optional ByVal vAccountType As Object = Nothing, Optional ByVal vShortCode As Object = Nothing, Optional ByVal vInsuranceRef As Object = Nothing, Optional ByVal vOperatorID As Object = Nothing, Optional ByVal vPurchaseOrderNo As Object = Nothing, Optional ByVal vPurchaseInvoiceNo As Object = Nothing, Optional ByVal vSpare As Object = Nothing, Optional ByVal vShowDeleted As Object = Nothing, Optional ByVal v_bOnlyUpdatable As Boolean = False, Optional ByVal vPaymentLedgerIDs As Object = Nothing, Optional ByVal vExcludeLedgerIDs As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL1, sSQL2, sSQL, sShortCode As String
        Dim lAccountID As Integer
        Dim vBalance As Object = Nothing
        Dim bLedgerIDProcessed As Boolean ' PN22590. 
        Dim sOptionValue As String = String.Empty

        Try
            ' Filter by Resticted branch
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=gPMConstants.PMSysOptionRestricteduserbranchOption, r_sOptionValue:=sOptionValue)

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = sSQL & "    /* Find out which Groups the user is a member of. */" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    CREATE TABLE #Temp_uts_Groups (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        pmuser_group_id int NOT NULL," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        code Char(10), " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        Caption VarChar(255), " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        is_supervisor tinyint NOT NULL)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        DECLARE @today_date AS datetime" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        SET @today_date = GETDATE() " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    INSERT INTO #Temp_uts_Groups" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        (pmuser_group_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        code, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        Caption, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        is_supervisor) " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    EXECUTE spu_pmuser_users_groups_sel" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        @user_id=" & CStr(m_iUserID) & " ," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        @effective_date=@today_date," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        @language_id=" & CStr(m_iLanguageID) & " " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        /* Get transactions requested. */ " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        CREATE TABLE #Temp_stqf_transactions (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        [Full Key] varchar(50), " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        short_code char(20)," & Strings.ChrW(13) & Strings.ChrW(10)
            'RSB1301093 - CQ2748
            sSQL = sSQL & "        account_name varchar(60), " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ledger_id smallint, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        account_type_id smallint, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        account_id int, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        account_key int," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        norminal_account_id int," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        description varchar(50)," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        accountstatus_id smallint," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        company_id smallint," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        balance Numeric(19,2)," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        contact_name varchar(60)," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        address1 varchar(40)," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        forename varchar(60)," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        currencyid int," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        currencycode char(4)," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        sourceid int," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        sourcecode char(10)," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        Has_unrestricted_enquiry tinyint," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        Has_unrestricted_update tinyint)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        INSERT #Temp_stqf_transactions " & Strings.ChrW(13) & Strings.ChrW(10)

            'eck130600 Get the users authority
            ' Construct the intial SQL
            'eck090600 Pass back the company id
            'eck050302 Pass empty variable to load the account balance into
            sSQL1 = "SELECT DISTINCT " &
                    "'Full Key', " &
                    "a.short_code, a.account_name, " &
                    "a.ledger_id, a.accounttype_id, a.account_id, " &
                    "a.account_key, a.nominal_account_id, " &
                    "ast.description, a.accountstatus_id, a.company_id, 0, " &
                    "a.contact_name, a.address1, ppc.forename, " &
                    "a.currency_id,c.iso_code,a.company_id,s.code,0, 0 " &
                    "FROM Account a " &
                    "INNER JOIN AccountStatus ast ON a.accountstatus_id = ast.accountstatus_id " &
                    "LEFT JOIN party_personal_client ppc on a.account_key = ppc.party_cnt " &
                    "JOIN Currency c on c.currency_id = a.currency_id " &
                    "JOIN source s on s.source_id = a.company_id "

            'DJM 11/12/2003 : Restrict by company_id for multi-company systems.
            sSQL2 = "WHERE" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "(" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "        EXISTS (SELECT * FROM hidden_options WHERE option_number = 16 AND value = 1)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "        AND a.company_id = " & CStr(iCompanyID) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "    )" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "    OR" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "        NOT EXISTS (SELECT * FROM hidden_options WHERE option_number = 16 AND value = 1)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "        AND a.company_id <> 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & "    )" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL2 = sSQL2 & ")" & Strings.ChrW(13) & Strings.ChrW(10)

            If sOptionValue = "1" Then
                'sSQL2 = sSQL2 & " AND  A.company_id IN (SELECT S.Source_id FROM Source S LEFT JOIN PMUSER_Source U " & Strings.ChrW(13) & Strings.ChrW(10)
                'sSQL2 = sSQL2 & " ON S.source_id = U.source_id AND U.user_id = " & CStr(m_iUserID) & Strings.ChrW(13) & Strings.ChrW(10)
                'sSQL2 = sSQL2 & " WHERE U.source_id IS NULL) "

                '31779 - System account to exclude branch access system option
                sSQL2 = sSQL2 & " AND ((  a.company_id IN (SELECT S.Source_id FROM Source S LEFT JOIN PMUSER_Source U " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL2 = sSQL2 & " ON S.source_id = U.source_id AND U.user_id = " & CStr(m_iUserID) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL2 = sSQL2 & " WHERE U.source_id IS NULL) )"
                sSQL2 = sSQL2 & " OR (short_code in (select value from system_options where option_number in(150,151,152,153,5028,5061))))"
            End If

            ' Now decide what else we need to add
            'PSL 10/06/2003 Iss4434 If it is called by Insurer Payment then subset of ledgers
            'PN 21417 If LedgerID is there is should override everything else.
            ' Need to drop through to other checks if nothing is processed for
            ' vLedgerID. PN22590.

            If Not Informations.IsNothing(vLedgerID) Then

                If CInt(vLedgerID) <> -1 Then

                    sSQL2 = sSQL2 & "AND (a.ledger_id = " & CStr(vLedgerID) & ") "
                    bLedgerIDProcessed = True
                End If
            End If


            If bLedgerIDProcessed Then
            ElseIf (Not Informations.IsNothing(vPaymentLedgerIDs)) Then
                sSQL2 = sSQL2 & "AND (a.Ledger_id IN (" & vPaymentLedgerIDs & ")) "
            ElseIf (Not Informations.IsNothing(vExcludeLedgerIDs)) Then
                sSQL2 = sSQL2 & "AND (a.Ledger_id NOT IN (" & vExcludeLedgerIDs & ")) "
            End If


            If Not String.IsNullOrEmpty(vAccountName) Then

                If Marshal.SizeOf(vAccountName) > 0 Then

                    m_lReturn = CType(bPMFunc.ValidateSQL(vAccountName), gPMConstants.PMEReturnCode)

                    If (CStr(vAccountName).IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (a.account_name = '" & CStr(vAccountName) & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (a.account_name LIKE '" & CStr(vAccountName) & "') "
                    End If
                End If
            End If


            If Not Informations.IsNothing(vAccountType) Then
                If CInt(vAccountType) <> -1 Then
                    sSQL2 = sSQL2 & "AND (a.accounttype_id = " & vAccountType & ") "
                End If
            End If

            'CT 4/8/00 fix to allow accounts with apostrophies in to be found
            sShortCode = vShortCode
            m_lReturn = CType(bPMFunc.ValidateSQL(sShortCode), gPMConstants.PMEReturnCode)
            vShortCode = sShortCode
            'CT end


            If Not Informations.IsNothing(vShortCode) Then
                If vShortCode.Length > 0 Then
                    If (vShortCode.IndexOf("%"c) + 1) = 0 Then
                        sSQL2 = sSQL2 & "AND (a.short_code = '" & vShortCode & "') "
                    Else
                        sSQL2 = sSQL2 & "AND (a.short_code LIKE '" & vShortCode & "') "
                    End If
                End If
            End If
            'eck130600
            If m_iHasUnrestrictedEnquiry = 0 Then
                sSQL2 = sSQL2 & "AND (a.restrict_enquiry = 0) "
            End If


            If Not Informations.IsNothing(vShowDeleted) Then
                If vShowDeleted > 0 Then
                    sSQL2 = sSQL2 & "AND (a.delete_at_purge = 0) "
                End If
            End If

            'CMG/PB if there are any populated fields searching the transdetail table then add it it.


            If (Not String.IsNullOrEmpty(vInsuranceRef) AndAlso Marshal.SizeOf(vInsuranceRef) > 0) Or (Not String.IsNullOrEmpty(vOperatorID) AndAlso (CStr(vOperatorID)).Length > 0) Or (Not String.IsNullOrEmpty(vPurchaseOrderNo) AndAlso Marshal.SizeOf(vPurchaseOrderNo) > 0) Or (Not String.IsNullOrEmpty(vPurchaseInvoiceNo) AndAlso Marshal.SizeOf(vPurchaseInvoiceNo) > 0) Or (Not String.IsNullOrEmpty(vSpare) AndAlso Marshal.SizeOf(vSpare) > 0) Then
                sSQL1 = sSQL1 & ", TransDetail t "
                sSQL2 = sSQL2 & "AND t.Account_Id = a.Account_Id "
            End If


            If Not String.IsNullOrEmpty(vInsuranceRef) AndAlso Marshal.SizeOf(vInsuranceRef) > 0 Then
                If (CStr(vInsuranceRef).IndexOf("%"c) + 1) = 0 Then
                    sSQL2 = sSQL2 & "AND ((t.insurance_ref = '" & CStr(vInsuranceRef) & "') "
                Else
                    sSQL2 = sSQL2 & "AND ((t.insurance_ref LIKE '" & CStr(vInsuranceRef) & "') "
                End If
            End If

            If (CStr(vOperatorID)).Length > 0 Then

                sSQL2 = sSQL2 & "AND (t.operator_id = " & CStr(vOperatorID) & ") "
            End If

            If Not String.IsNullOrEmpty(vPurchaseOrderNo) AndAlso Marshal.SizeOf(vPurchaseOrderNo) > 0 Then
                If (CStr(vPurchaseOrderNo).IndexOf("%"c) + 1) = 0 Then
                    sSQL2 = sSQL2 & "AND (t.purchase_order_no = '" & CStr(vPurchaseOrderNo) & "') "
                Else
                    sSQL2 = sSQL2 & "AND (t.purchase_order_no LIKE '" & CStr(vPurchaseOrderNo) & "') "
                End If
            End If

            If Not String.IsNullOrEmpty(vPurchaseInvoiceNo) AndAlso Marshal.SizeOf(vPurchaseInvoiceNo) > 0 Then
                If (CStr(vPurchaseInvoiceNo).IndexOf("%"c) + 1) = 0 Then
                    sSQL2 = sSQL2 & "AND (t.purchase_invoice_no = '" & CStr(vPurchaseInvoiceNo) & "') "
                Else
                    sSQL2 = sSQL2 & "AND (t.purchase_invoice_no LIKE '" & CStr(vPurchaseInvoiceNo) & "') "
                End If
            End If

            If Not String.IsNullOrEmpty(vSpare) AndAlso Marshal.SizeOf(vSpare) > 0 Then
                If (CStr(vSpare).IndexOf("%"c) + 1) = 0 Then
                    sSQL2 = sSQL2 & "AND (t.spare = '" & CStr(vSpare) & "')) "
                Else
                    sSQL2 = sSQL2 & "AND (t.spare LIKE '" & CStr(vSpare) & "')) "
                End If
            End If
            'END CMG

            sSQL = sSQL & sSQL1 & sSQL2 & "ORDER BY short_code"

            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "/* Find transaction rights available to user. */ " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "CREATE TABLE #Temp_Account_Authorities (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "account_id integer," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Has_unrestricted_enquiry tinyint," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Has_unrestricted_update tinyint) " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "/* Get user authorities on all accounts in transaction list */ " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "insert #Temp_Account_Authorities" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " select tst.account_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  max(pmuga.Has_unrestricted_enquiry) as Has_unrestricted_enquiry," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  max(pmuga.Has_unrestricted_update) As Has_unrestricted_update" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  from #Temp_stqf_transactions tst" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  join StructureTree st on tst.account_id = st.account_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  join PMUser_Group_Authorities pmuga on st.parent_node_id=pmuga.node_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  join #Temp_uts_Groups tug on pmuga.pmuser_group_id=tug.pmuser_group_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  group by tst.account_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "UPDATE #Temp_stqf_transactions SET Has_unrestricted_enquiry = (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " CASE WHEN (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  EXISTS (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "   SELECT 1 FROM #Temp_Account_Authorities taa" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    WHERE taa.account_id = #Temp_stqf_transactions.account_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    AND taa.Has_unrestricted_enquiry = 1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  )) THEN 1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  ELSE 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  END) " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "UPDATE #Temp_stqf_transactions SET Has_unrestricted_update = (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " CASE WHEN (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  EXISTS (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "   SELECT 1 FROM #Temp_Account_Authorities taa" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    WHERE taa.account_id = #Temp_stqf_transactions.account_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    AND taa.Has_unrestricted_update = 1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  )) THEN 1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  ELSE 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "  END) " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            If v_bOnlyUpdatable Then
                sSQL = sSQL & "SELECT * from #Temp_stqf_transactions WHERE Has_unrestricted_update = 1 " & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "SELECT * from #Temp_stqf_transactions WHERE Has_unrestricted_enquiry = 1 " & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "DROP TABLE #Temp_uts_Groups " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "DROP TABLE #Temp_stqf_transactions " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "DROP TABLE #Temp_Account_Authorities " & Strings.ChrW(13) & Strings.ChrW(10)

            ' Perform the SQL query
            'MKR 27/08/04 PN : 8651 Restricting the records count to only 500
            If lNumberOfRecords <> -1 Then
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectAccountQueryFiltered", bStoredProcedure:=False, lNumberRecords:=lNumberOfRecords, vResultArray:=vresultArray)
            Else
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectAccountQueryFiltered", bStoredProcedure:=False, lNumberRecords:=500, vResultArray:=vresultArray)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SQL Failed : " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectAccountQueryFiltered")
                Return result
            End If

            ' Check that there's a result set.
            If Not Informations.IsArray(vresultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'eck050302 Get Account Balance
            For lCount As Integer = vresultArray.GetLowerBound(1) To vresultArray.GetUpperBound(1)

                lAccountID = CInt(vresultArray(ACIAccountID, lCount))

                m_lReturn = m_oAccount.GetAccountBalance(r_vdAccountBalance:=vBalance, v_vAccountID:=lAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If


                vresultArray(ACIBalance, lCount) = vBalance
            Next lCount
            'eck050302End

            Return result

        Catch ex As Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed on SelectAccountQueryFiltered", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectAccountQueryFiltered", excep:=ex)

            Return result
        End Try

    End Function


    '****************************************************************** '
    '* Name: GetUserAuthorities (Public)
    '*
    '* Description: Get user authorities for the user
    '*
    '*
    '****************************************************************** '
    Public Function GetUserAuthorities() As Integer

        Dim result As Integer = 0
        Dim lNumberRecords As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sSQL As String = "SELECT has_unrestricted_enquiry,has_unrestricted_update FROM user_authorities WHERE user_id = {user_id}"
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        ' Perform the query
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetEnquiryRestriction", bStoredProcedure:=False, lNumberRecords:=lNumberRecords)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            m_iHasUnrestrictedEnquiry = 0
            m_iHasUnrestrictedUpdate = 0
        End If

        ' Get the return value
        If lNumberRecords > 0 Then
            'developer guide no. 111
            m_iHasUnrestrictedEnquiry = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields()("has_unrestricted_enquiry"))
            m_iHasUnrestrictedUpdate = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields()("has_unrestricted_update"))
        Else
            m_iHasUnrestrictedEnquiry = 0
            m_iHasUnrestrictedUpdate = 0
        End If

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetUserAuthorities", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthorities")

        Return result

    End Function


    '****************************************************************** '
    '* Name: GetLedgersQuery (Public)
    '*
    '* Description: Get all ledgers for the company
    '*
    '*
    '****************************************************************** '
    Public Function GetLedgersQuery(ByRef lNumberOfRecords As Integer, ByRef vresultArray(,) As Object, ByVal vCompanyId As Object) As Integer

        ' Uses stored procedure

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CompanyID parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="CompanyID", vValue:=CStr(vCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgersQuery")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetLedgersQuerySQL, sSQLName:=ACGetLedgersQueryName, bStoredProcedure:=ACGetLedgersQueryStored, lNumberRecords:=lNumberOfRecords, vResultArray:=vresultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgersQuery")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            Return CheckResults(vresultArray)

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLedgersQuery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgersQuery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************** '
    '* Name: GetFullKey (Public)
    '*
    '* Description: Get FullKey from Account Explorer
    '****************************************************************** '
    Public Function GetFullKey(ByVal v_lAccountID As Integer, ByRef r_sFullKey As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CTAF 080101 - Replaced the use of Explorer with a stored procedue
            '               Therefore all instances of m_oExplorer have been commented out.
            'r_sFullKey$ = m_oExplorer.FullKey(v_lAccountID&)

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="full_path", vValue:=r_sFullKey, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetFullKeySQL, sSQLName:=ACGetFullKeyName, bStoredProcedure:=ACGetFullKeyStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the result
            r_sFullKey = gPMFunctions.NullToString(m_oDatabase.Parameters.Item("full_path").Value)
            If r_sFullKey = "" Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFullKey  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFullKey ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetID (Public)
    '
    ' Description: Gets the (last) AccountID matching
    '              the given parameters.
    '
    ' ***************************************************************** '

    Public Function GetID(ByRef lAccountID As Integer, ByVal iCompanyID As Integer, Optional ByVal vLedgerID As Object = Nothing, Optional ByVal vFullKey As Object = Nothing, Optional ByVal vShortCode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRowsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' At least one of vFullKey or vShortCode must be defined

            If Informations.IsNothing(vFullKey) And Informations.IsNothing(vShortCode) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lAccountID = -1
                Return result
            End If

            ' Supply defaults for missing parameters

            If (Informations.IsNothing(vLedgerID)) Then vLedgerID = -1

            If (Informations.IsNothing(vFullKey)) Then vFullKey = ""

            If (Informations.IsNothing(vShortCode)) Then vShortCode = ""

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CompanyID parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="CompanyID", vValue:=CStr(iCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("GetID", "Parameters.Add Failed.")
            End If

            ' Add the LedgerID parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="LedgerID", vValue:=CStr(vLedgerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("GetID", "Parameters.Add Failed.")
            End If

            ' Add the ShortCode parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="ShortCode", vValue:=vShortCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("GetID", "Parameters.Add Failed.")
            End If

            ' Add the FullKey parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="FullKey", vValue:=vFullKey, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("GetID", "Parameters.Add Failed.")
            End If

            ' Add the AccountID parameter (OUTPUT)
            If m_oDatabase.Parameters.Add(sName:="AccountID", vValue:=CStr(lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("GetID", "Parameters.Add Failed.")
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLAction(sSQL:=ACGetAccountIDSQL, sSQLName:=ACGetAccountIDName, bStoredProcedure:=ACGetAccountIDStored, lRecordsAffected:=lRowsAffected)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLAction failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the AccountID of the record selected
            lAccountID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("AccountID").Value)

            If lAccountID = -1 Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception
            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetID", excep:=ex)
        End Try
        Return result
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: CheckResults (Private)
    '
    ' Description: Checks the result array after a query
    '              If records found returns PMTrue
    '              If no records found returns PMNotFound
    '
    ' ***************************************************************** '
    Private Function CheckResults(ByRef vresultArray(,) As Object) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If NO records were found return PMNotFound
        If Not Informations.IsArray(vresultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

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
        ' Error Section.
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
    ' Name: GetAccountStatus
    '
    ' Description: Pass the call through to the account object
    '
    ' History: 29/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetAccountStatus(ByVal v_lAccountID As Integer, ByRef r_iAccountStatus As Integer, ByRef r_bIsStopped As Boolean) As Integer

        Dim result As Integer = 0
        Try



            Return m_oAccount.GetAccountStatus(v_lAccountID:=v_lAccountID, r_iAccountStatus:=r_iAccountStatus, r_bIsStopped:=r_bIsStopped)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************** '
    '* Name: GetPaymentLedgers (Public)
    '*
    '* Description: Get payment ledgers for the Insurer/agenty payment screen
    '*
    '*
    '****************************************************************** '
    Public Function GetPaymentLedgers(ByRef r_sPaymentLedgersString As String) As Integer

        Dim result As Integer = 0
        Dim vresultArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetPaymentLedgersSQL, sSQLName:=ACGetPaymentLedgersName, bStoredProcedure:=ACGetPaymentLedgersStored, vResultArray:=vresultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentLedgers")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            result = CheckResults(vresultArray)

            If result = gPMConstants.PMEReturnCode.PMTrue Then

                For iCnt As Integer = 0 To vresultArray.GetUpperBound(0)
                    If iCnt = 0 Then

                        r_sPaymentLedgersString = CStr(vresultArray(iCnt, 0))
                    Else

                        r_sPaymentLedgersString = r_sPaymentLedgersString & "," & CStr(vresultArray(iCnt, 0))
                    End If
                Next iCnt
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentLedgers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentLedgers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
