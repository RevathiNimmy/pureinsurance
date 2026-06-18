Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 14/06/1998
    '
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
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


    Private Const ACClass As String = "Business"

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sUnderwritingOrAgency As String = ""

    ' Instance of server component services

    ' Return value
    Private m_lReturn As Integer

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

    'TN20001119 (Start)
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property
    'TN20001119 (End)

    ' *********************** Public Functions ************************

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Standard initialise function.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise




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


            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' New instance of component services

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description: Standard terminate function.
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
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ************************************************************************** '
    ' Name: GetAccountID
    '
    ' Description: Gets the account_id from the account with the passed short code.
    '
    ' ************************************************************************** '
    Public Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer, ByRef r_iCompanyID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lNumberRecords As Integer
        Dim sShortCode As String = ""
        Dim iCompanyId As Integer 'PN6169
        ' Database reference
        Dim oDatabase As dPMDAO.Database
        Dim bCloseDatabase As Boolean

        Dim iFullEnquiryAccess, iRestrictEnquiry As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' New instance of the database
            oDatabase = New dPMDAO.Database()

            ' Get a reference to the Orion database
            'SD 02/08/2002
            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Search for an account id

            ' Construct the SQL
            'eck130600
            sSQL = "SELECT account_id,company_id,restrict_enquiry FROM account WHERE short_code = {short_code}"
            'eck PN6169
            If r_iCompanyID > 0 Then
                sSQL = sSQL & " AND company_id = {company_id}"
            End If

            ' CTAF 260400 - Validate the short code, incase it has apostrophes in it.
            oDatabase.Parameters.Clear()

            sShortCode = v_sShortCode
            iCompanyId = r_iCompanyID 'eck PN6169

            m_lReturn = bPMFunc.ValidateSQL(sSQLStatement:=sShortCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oDatabase.Parameters.Add(sName:="short_code", vValue:=sShortCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'eck PN6169
            m_lReturn = oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(iCompanyId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            ' Perform the query
            m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountID", bStoredProcedure:=False, lNumberRecords:=lNumberRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get the return value
            If lNumberRecords > 0 Then

                ' We have an account
                'developer guide no.162
                'start
                r_lAccountID = gPMFunctions.NullToLong(oDatabase.Records.Item(0).Fields()("account_id"))
                r_iCompanyID = gPMFunctions.NullToLong(oDatabase.Records.Item(0).Fields()("company_id"))
                iRestrictEnquiry = gPMFunctions.NullToLong(oDatabase.Records.Item(0).Fields()("restrict_enquiry"))
                'end
                If iRestrictEnquiry <> 0 Then

                    'eck130600 See whether the user has access to all accounts
                    sSQL = "SELECT has_unrestricted_enquiry FROM user_authorities WHERE user_id = {user_id}"
                    oDatabase.Parameters.Clear()
                    m_lReturn = oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    ' Perform the query
                    m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetEnquiryRestriction", bStoredProcedure:=False, lNumberRecords:=lNumberRecords)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If

                    ' Get the return value
                    If lNumberRecords > 0 Then
                        'SD 02/08/2002 Scalability
                        'developer guide no.162
                        iFullEnquiryAccess = gPMFunctions.NullToLong(oDatabase.Records.Item(0).Fields()("has_unrestricted_enquiry"))
                    Else
                        iFullEnquiryAccess = 0
                    End If

                    If iFullEnquiryAccess = 0 Then
                        r_lAccountID = 0
                        result = gPMConstants.PMEReturnCode.PMMNoAccess
                    End If
                End If
            Else

                ' We dont have an account, so make one
                m_lReturn = CreateAccount(v_sShortCode:=v_sShortCode, r_lAccountID:=r_lAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create account for '" & v_sShortCode & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End If

            ' Close the database if needed
            If bCloseDatabase Then
                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateAccount
    '
    ' Description: Creates an account on the orion database with the
    '              passed short code.
    '
    ' ***************************************************************** '
    Public Function CreateAccount(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim vLedgerFlag As String = ""
        Dim vSourceID As Integer
        Dim vShortName As String = ""
        Dim vPartyType As Integer
        Dim vPartyID As Integer
        Dim vName As Integer
        Dim vCurrencyID As Integer
        Dim vSalesAccountID As Object = Nothing
        Dim vPurchaseAccountID As Object = Nothing
        Dim vPartyCnt As Integer

        Dim sShortCode As String = ""

        Dim vResultArray(,) As Object = Nothing
        Dim oDatabase As dPMDAO.Database
        Dim bCloseDatabase As Boolean
        Dim sSQL As String = ""

        Dim oAccount As bPMBOrionLink.Account

        Dim vValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a connection to the sirius database

            ' New instance of the database
            oDatabase = New dPMDAO.Database()

            ' Get a reference to the Orion database
            'SD 02/08/2002
            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the required info
            sSQL = "SELECT source_id, party_type_id, party_id, name, currency_id, party_cnt " & _
                   "FROM Party WHERE shortname = {short_code}"

            ' Clear the database parameters
            oDatabase.Parameters.Clear()

            ' CTAF 260400
            ' Validate the SQL
            sShortCode = v_sShortCode
            m_lReturn = bPMFunc.ValidateSQL(sSQLStatement:=sShortCode)

            ' Add short code
            m_lReturn = oDatabase.Parameters.Add(sName:="short_code", vValue:=sShortCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the SQL
            m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyDetails", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the values from the array
            'MKW 180604 PN12470
            'Check for multitree accounting if so return login source, otherwise party source.
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=vValue)
            If vValue = "1" Then
                vSourceID = m_iSourceID
            Else

                vSourceID = CInt(vResultArray(0, 0))
            End If

            'vSourceID = vResultArray(0, 0)

            vPartyType = CInt(vResultArray(1, 0))

            vPartyID = CInt(vResultArray(2, 0))

            vName = CInt(vResultArray(3, 0))

            vCurrencyID = CInt(vResultArray(4, 0))

            vPartyCnt = CInt(vResultArray(5, 0))

            ' Set the rest
            vLedgerFlag = "S"
            vShortName = v_sShortCode

            ' Instance of account (private) object
            oAccount = New bPMBOrionLink.Account()

            m_lReturn = CType(oAccount, SSP.S4I.Interfaces.IBusiness).Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove the instance of account
                oAccount = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the account class to create the object


            m_lReturn = oAccount.CreateAccount(v_oDatabase:=oDatabase, vLedgerFlag:=vLedgerFlag, vSourceID:=vSourceID, vShortName:=vShortName, vPartyType:=vPartyType, vPartyID:=vPartyID, vName:=CStr(vName), vCurrencyID:=vCurrencyID, vSalesAccountID:=CInt(vSalesAccountID), vPurchaseAccountID:=CInt(vPurchaseAccountID), vPartyCnt:=vPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Terminate the account object
                oAccount.Dispose()
                oAccount = Nothing
                Return result
            End If

            ' Terminate the account object
            oAccount.Dispose()
            oAccount = Nothing

            ' Close the database if needed
            If bCloseDatabase Then

                m_lReturn = oDatabase.CloseDatabase()

                oDatabase = Nothing

            End If

            Select Case vLedgerFlag
                Case "S"

                    r_lAccountID = CInt(vSalesAccountID)
                Case "P"

                    r_lAccountID = CInt(vPurchaseAccountID)
            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' History : Tinny Created
    ' 14/06/2002 SP - moved to uniform Product Options scheme and bas file
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0



        Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

    End Function

    '*******************************************************************************
    ' Name : GetAgentShortName
    '
    ' Description : return agent shortname if available, else return client shortname
    '
    ' History : Tinny Created
    '
    '*******************************************************************************
    Public Function GetAgentShortName(ByVal v_sClientShortName As String, ByRef r_sAgentShortName As String) As Integer

        Dim result As Integer = 0
        Dim oDatabase As dPMDAO.Database
        Dim sSQL As String = ""
        Dim bCloseDatabase As Boolean
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' New instance of the database
            oDatabase = New dPMDAO.Database()

            ' Get a reference to the Orion database
            'SD 02/08/2002
            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "SELECT pt2.shortname" & Strings.Chr(13) & Strings.Chr(10) & _
                   "FROM party pt1, party pt2" & Strings.Chr(13) & Strings.Chr(10) & _
                   "WHERE pt1.agent_cnt = pt2.party_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AND pt1.shortname = {ClientShortName}"


            oDatabase.Parameters.Clear()

            m_lReturn = oDatabase.Parameters.Add(sName:="ClientShortName", vValue:=v_sClientShortName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAgentShortName", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have an agent shortname
            If Information.IsArray(vResultArray) Then
                'there is only one record

                r_sAgentShortName = CStr(vResultArray(0, 0))
            Else
                'default to client short name if there isn't an agent short name
                r_sAgentShortName = v_sClientShortName
            End If

            If bCloseDatabase Then
                m_lReturn = oDatabase.CloseDatabase()

                oDatabase = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgentShortName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgentShortName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

