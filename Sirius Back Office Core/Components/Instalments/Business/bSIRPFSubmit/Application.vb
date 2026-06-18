Option Strict Off
Option Explicit On
Imports System.Threading
Imports System.Xml
Imports SSP.Shared
'Modified by Vijay Pal on 6/1/2010 5:45:00 PM refer developer guide no. 129
<System.Runtime.InteropServices.ProgId("Application_NET.Application")>
Public NotInheritable Class Application
    ' ***************************************************************** '
    ' Class Name    : Application
    ' File Name     : Application.cls
    ' Date          : 29-09-2004
    ' Author        : Aaron Rhodes
    ' Description   : Business class for PF Submit component
    ' Edit History  :
    '   PW150905 - PN23065 - remove bObjectManager as this should not ever be used
    '              in a server side object. Even though it was not actually being
    '              used here, it was being initialised.
    ' ***************************************************************** '

    Private Const ACApp As String = "bSIRPFSubmit"

    ' Added to replace global variables 02/04/2007
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
    Private Const ACClass As String = "Application"

    'path to our xsl's..
    Private Const ACXSLPath As String = "\PremiumFinance"
    Private Const ACXSLOut As String = "\Out"
    Private Const ACXSLIn As String = "\In"
    Private Const ACXSLGetRates As String = "\GetRates.xsl"
    Private Const ACXSLAcceptRates As String = "\AcceptRates.xsl"
    Private Const ACXSLGetMTARates As String = "\GetMTARates.xsl"
    Private Const ACXSLAcceptMTARates As String = "\AcceptMTARates.xsl"
    Private Const ACXSLGetOverrideAuth As String = "\GetOverrideAuth.xsl"
    Private Const ACXSLGetDocument As String = "\GetDocument.xsl"
    Private Const ACXSLUpdateBankDetails As String = "\UpdateBankDetails.xsl"
    Private Const ACXSLCancelPlan As String = "\CancelPlan.xsl"
    Private Const ACXSLUpdateAddress As String = "\UpdateAddressDetails.xsl"
    Private Const ACXSLUpdateBank As String = "\UpdateBankDetails.xsl"
    Private Const ACXSLUpdateDate As String = "\UpdateDateDetails.xsl"
    Private Const ACXSLAcceptRenewal As String = "\AcceptRenewal.xsl"

    'mapping document
    Private Const ACXSLMappingDoc As String = "\Mappings.xsl"
    Private Const ACXSLBusinessMappings As String = "\BusinessMappings.xsl"
    Private Const ACXSLInsurerMappings As String = "\InsurerMappings.xsl"

    'enum list for our current message type
    Public Enum eMsgType
        eGetRates = 0
        eAcceptRates = 1
        eGetMTARates = 2
        eAcceptMTARates = 3
        eGetOverrideAuth = 4
        eGetDocument = 5
        eCancelPlan = 6
        eUpdateAddress = 7
        eUpdateBank = 8
        eUpdateDate = 9
        eAcceptRenewal = 10
    End Enum

    'Logon details

    'response data storage
    Private m_lPartyCnt As Integer
    Private m_lMessageType As eMsgType
    Private m_vRatesArray As Object
    Private m_sURL As String = ""
    Private m_dPremium As Double
    Private m_lTimeout As Integer
    Private m_sProviderCode As String = ""
    Private m_sBrokerID As String = ""
    Private m_vRateArray(,) As Object
    Private m_lStatusCode As Integer
    Private m_sClientName As String = ""
    Private m_sSortCode As String = ""
    Private m_sAccountNo As String = ""
    Private m_sAccountName As String = ""
    Private m_lInstalments As Integer
    Private m_lTerms As Integer
    Private m_dPercentage As Double
    Private m_dStartDate As Date
    Private m_dEndDate As Date
    Private m_dTotalPayable As Double
    Private m_sReference As String = ""
    Private m_sDocURL As String = ""
    Private m_dOverrideRate As Double
    Private m_sErrorText As String = ""
    Private m_sErrorCode As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_sCompanyRegNo As String = ""
    Private m_sRefundType As String = ""
    Private m_sBusinessCode As String = ""
    Private m_sFirstInstalment As String = ""
    Private m_sOtherInstalment As String = ""
    Private m_sAddress1 As String = ""
    Private m_sAddress2 As String = ""
    Private m_sAddress3 As String = ""
    Private m_sAddress4 As String = ""
    Private m_sPostCode As String = ""
    Private m_cDeposit As Decimal
    Private m_sExistingReference As String = ""
    Private m_vTransactionArray(,) As Object
    Private m_cCreditCharge As Decimal
    Private m_sCardNumber As String = ""
    Private m_sCardExpiryDate As String = ""
    Private m_sCardStartDate As String = ""
    Private m_sCardName As String = ""
    Private m_sCardIssueNo As String = ""
    Private m_bCardHolder As Boolean
    Private m_sCardTPName As String = ""
    Private m_sCardTPAddress1 As String = ""
    Private m_sCardTPAddress2 As String = ""
    Private m_sCardTPAddress3 As String = ""
    Private m_sCardTPAddress4 As String = ""
    Private m_sCardTPPostCode As String = ""
    Private m_sCardType As String = ""
    Private m_dInstalmentAmount As Double
    Private m_dFirstInstalmentDate As Date
    Private m_sSchemeType As String = ""
    Private m_sSchemeCode As String = ""
    Private m_lPaymentDay As Integer
    Private m_bBrokerRenewal As Boolean
    Private m_bRenewal As Boolean
    Private m_sClientType As String = ""
    Private m_lSourceID As Integer
    'variable's end

    Private m_vPartnerArray(,) As String

    Private m_OutputDoc As XmlDocument
    Private m_oXSLDoc As XmlDocument

    'is the app in debug?
    Private m_bDebug As Boolean

    'so we can give control back to the calling app..
    Private WithEvents tmrAsync As Timer

    'property variables
    Private m_sFaultString As String = ""
    Private m_bFinished As Boolean
    Private m_lProgress As Integer
    'property var's end

    Private m_oDatabase As Object

    Private m_oDoc As XmlDocument

    'our basic xml definition
    Const AC_DEFAULTXML As String = "<?xml version=""1.0"" encoding=""UTF-8""?>" & Strings.ChrW(13) & Strings.ChrW(10) & "<sirius_finance/>"

    Dim m_lReturn As gPMConstants.PMEReturnCode

    'PUBLIC PROPERTIES
    Public ReadOnly Property ErrorText() As String
        Get
            Return m_sErrorText
        End Get
    End Property

    Public ReadOnly Property ErrorCode() As String
        Get
            Return m_sErrorCode
        End Get
    End Property

    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public WriteOnly Property URL() As String
        Set(ByVal Value As String)
            m_sURL = Value
        End Set
    End Property

    Public WriteOnly Property Timeout() As Integer
        Set(ByVal Value As Integer)
            m_lTimeout = Value
        End Set
    End Property

    Public WriteOnly Property Premium() As Double
        Set(ByVal Value As Double)
            m_dPremium = Value
        End Set
    End Property

    Public WriteOnly Property ProviderCode() As String
        Set(ByVal Value As String)
            m_sProviderCode = Value
        End Set
    End Property

    Public WriteOnly Property Username() As String
        Set(ByVal Value As String)
            m_sUsername = Value
        End Set
    End Property

    Public WriteOnly Property Password() As String
        Set(ByVal Value As String)
            m_sPassword = Value
        End Set
    End Property

    Public WriteOnly Property BrokerID() As String
        Set(ByVal Value As String)
            m_sBrokerID = Value
        End Set
    End Property

    Public ReadOnly Property RateArray() As Object
        Get
            Return (m_vRateArray).Clone()
        End Get
    End Property


    Public Property MessageType() As Integer
        Get
            Return m_lMessageType
        End Get
        Set(ByVal Value As Integer)
            m_lMessageType = Value
        End Set
    End Property

    Public ReadOnly Property StatusCode() As Integer
        Get
            Return m_lStatusCode
        End Get
    End Property

    Public WriteOnly Property ClientName() As String
        Set(ByVal Value As String)
            m_sClientName = Value
        End Set
    End Property

    Public WriteOnly Property SortCode() As String
        Set(ByVal Value As String)
            m_sSortCode = Value
        End Set
    End Property

    Public WriteOnly Property AccountNo() As String
        Set(ByVal Value As String)
            m_sAccountNo = Value
        End Set
    End Property

    Public WriteOnly Property AccountName() As String
        Set(ByVal Value As String)
            m_sAccountName = Value
        End Set
    End Property


    Public Property Instalments() As Integer
        Get
            Return m_lInstalments
        End Get
        Set(ByVal Value As Integer)
            m_lInstalments = Value
        End Set
    End Property

    Public WriteOnly Property Terms() As Integer
        Set(ByVal Value As Integer)
            m_lTerms = Value
        End Set
    End Property

    Public WriteOnly Property Percentage() As Double
        Set(ByVal Value As Double)
            m_dPercentage = Value
        End Set
    End Property

    Public WriteOnly Property StartDate() As Date
        Set(ByVal Value As Date)
            m_dStartDate = Value
        End Set
    End Property

    Public WriteOnly Property EndDate() As Date
        Set(ByVal Value As Date)
            m_dEndDate = Value
        End Set
    End Property


    Public Property Reference() As String
        Get
            Return m_sReference
        End Get
        Set(ByVal Value As String)
            m_sReference = Value
        End Set
    End Property

    Public ReadOnly Property TotalPayable() As Double
        Get
            Return m_dTotalPayable
        End Get
    End Property

    Public ReadOnly Property DocURL() As String
        Get
            Return m_sDocURL
        End Get
    End Property

    Public ReadOnly Property FirstInstalment() As String
        Get
            Return m_sFirstInstalment
        End Get
    End Property

    Public ReadOnly Property OtherInstalment() As String
        Get
            Return m_sOtherInstalment
        End Get
    End Property

    Public WriteOnly Property OverrideRate() As Double
        Set(ByVal Value As Double)
            m_dOverrideRate = Value
        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property CompanyRegNo() As String
        Set(ByVal Value As String)
            m_sCompanyRegNo = Value
        End Set
    End Property

    Public WriteOnly Property RefundType() As String
        Set(ByVal Value As String)
            m_sRefundType = Value
        End Set
    End Property

    Public WriteOnly Property BusinessCode() As String
        Set(ByVal Value As String)
            m_sBusinessCode = Value.Trim()
        End Set
    End Property

    Public WriteOnly Property Address1() As String
        Set(ByVal Value As String)
            m_sAddress1 = Value.Trim()
        End Set
    End Property

    Public WriteOnly Property Address2() As String
        Set(ByVal Value As String)
            m_sAddress2 = Value.Trim()
        End Set
    End Property

    Public WriteOnly Property Address3() As String
        Set(ByVal Value As String)
            m_sAddress3 = Value.Trim()
        End Set
    End Property

    Public WriteOnly Property Address4() As String
        Set(ByVal Value As String)
            m_sAddress4 = Value.Trim()
        End Set
    End Property

    Public WriteOnly Property PostCode() As String
        Set(ByVal Value As String)
            m_sPostCode = Value
        End Set
    End Property

    Public WriteOnly Property Deposit() As Decimal
        Set(ByVal Value As Decimal)
            m_cDeposit = Value
        End Set
    End Property

    Public WriteOnly Property ExistingReference() As String
        Set(ByVal Value As String)
            m_sExistingReference = Value
        End Set
    End Property

    'Developer Guide No 33
    'Public WriteOnly Property TransactionArray() As Object()
    '	Set(ByVal Value() As Object)
    '		m_vTransactionArray = Value
    '	End Set
    'End Property
    Public WriteOnly Property TransactionArray() As Object
        Set(ByVal Value As Object)
            m_vTransactionArray = Value
        End Set
    End Property

    Public ReadOnly Property CreditCharge() As Decimal
        Get
            Return m_cCreditCharge
        End Get
    End Property

    Public WriteOnly Property CardNumber() As String
        Set(ByVal Value As String)
            m_sCardNumber = Value
        End Set
    End Property

    Public WriteOnly Property CardExpiryDate() As String
        Set(ByVal Value As String)
            m_sCardExpiryDate = Value
        End Set
    End Property

    Public WriteOnly Property CardStartDate() As String
        Set(ByVal Value As String)
            m_sCardStartDate = Value
        End Set
    End Property

    Public WriteOnly Property CardName() As String
        Set(ByVal Value As String)
            m_sCardName = Value
        End Set
    End Property

    Public WriteOnly Property CardIssueNo() As String
        Set(ByVal Value As String)
            m_sCardIssueNo = Value
        End Set
    End Property

    Public WriteOnly Property CardHolder() As Boolean
        Set(ByVal Value As Boolean)
            m_bCardHolder = Value
        End Set
    End Property

    Public WriteOnly Property CardTPName() As String
        Set(ByVal Value As String)
            m_sCardTPName = Value
        End Set
    End Property

    Public WriteOnly Property CardTPAddress1() As String
        Set(ByVal Value As String)
            m_sCardTPAddress1 = Value
        End Set
    End Property

    Public WriteOnly Property CardTPAddress2() As String
        Set(ByVal Value As String)
            m_sCardTPAddress2 = Value
        End Set
    End Property

    Public WriteOnly Property CardTPAddress3() As String
        Set(ByVal Value As String)
            m_sCardTPAddress3 = Value
        End Set
    End Property

    Public WriteOnly Property CardTPAddress4() As String
        Set(ByVal Value As String)
            m_sCardTPAddress4 = Value
        End Set
    End Property

    Public WriteOnly Property CardTPPostCode() As String
        Set(ByVal Value As String)
            m_sCardTPAddress3 = Value
        End Set
    End Property

    Public WriteOnly Property CardType() As String
        Set(ByVal Value As String)
            m_sCardType = Value
        End Set
    End Property

    Public WriteOnly Property InstalmentAmount() As Double
        Set(ByVal Value As Double)
            m_dInstalmentAmount = Value
        End Set
    End Property

    Public WriteOnly Property FirstInstalmentDate() As Date
        Set(ByVal Value As Date)
            m_dFirstInstalmentDate = Value
        End Set
    End Property

    Public WriteOnly Property SchemeType() As String
        Set(ByVal Value As String)
            m_sSchemeType = Value
        End Set
    End Property

    Public WriteOnly Property SchemeCode() As String
        Set(ByVal Value As String)
            m_sSchemeCode = Value
        End Set
    End Property

    Public WriteOnly Property PaymentDay() As Integer
        Set(ByVal Value As Integer)
            m_lPaymentDay = Value
        End Set
    End Property

    Public WriteOnly Property BrokerRenewal() As Boolean
        Set(ByVal Value As Boolean)
            m_bBrokerRenewal = Value
        End Set
    End Property

    Public WriteOnly Property Renewal() As Boolean
        Set(ByVal Value As Boolean)
            m_bRenewal = Value
        End Set
    End Property

    Public WriteOnly Property ClientType() As String
        Set(ByVal Value As String)
            m_sClientType = Value
        End Set
    End Property

    Public WriteOnly Property SourceID() As Integer
        Set(ByVal Value As Integer)
            m_lSourceID = Value
        End Set
    End Property

    'RESPONSE PROPERTIES!
    Public ReadOnly Property RatesArray() As Object
        Get
            Return m_vRatesArray
        End Get
    End Property
    'PUBLIC PROPERTIES END

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As gPMConstants.PMEReturnCode

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

            'm_sCoverStartDate = ""



            lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=sUsername, v_iSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=True, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'AS per requirement to remove MSXML (https://sspengineering.visualstudio.com/SspEngineering/_workitems/edit/96304) 
    'functionality of Submit() is removed as this is no longer in use.
    '***********************************************************************
    Public Function Submit() As Integer
        Dim errorMessage As String = "This functionality is no longer in use. Please contact administrator for further assistance."
        'Log Error Message
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=errorMessage,
                           vApp:=ACApp, vClass:=ACClass, vMethod:="Submit", vErrNo:=gPMConstants.PMEReturnCode.PMError, vErrDesc:="", excep:=Nothing)
        Return gPMConstants.PMEReturnCode.PMError

    End Function


    Public Function GetRates(ByRef sRef As String, ByRef r_vArray(,) As Object) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            Const ACCreateQuoteSQL As String = "{call spu_GetPFRateData(?)}"
            Const ACCreateQuoteName As String = "GetRates"

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="reference", vValue:=ToSafeBoolean(sRef), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

                ' Run 'create quote' stored procedure

                m_lReturn = .SQLSelect(sSQL:=ACCreateQuoteSQL, sSQLName:=ACCreateQuoteName, bStoredProcedure:=True, vResultArray:=CType(r_vArray, Object))

                'For x = 1 To .Records.Count

            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetRates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRates", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'Developer Guide No 101
    'Public Function LoadPartners(ByRef v_vPartnerArray() As String) As Integer
    Public Function LoadPartners(ByRef v_vPartnerArray As Object) As Integer
        m_vPartnerArray = v_vPartnerArray
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    Public Function GetBankDetailsFromParty(ByRef v_lPartyCnt As Integer) As Integer




        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing
        Dim lUpper As Integer

        sSQL = "SELECT BankAccountNo, BankSortCode, BankAccountName FROM pfpremiumfinance WHERE clientid = " & (v_lPartyCnt).ToString()


        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), bStoredProcedure:=False, sSQLName:="BankAccount Details", vResultArray:=CType(vArray, Object))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Err_GetBankDetailsFromParty
        End If

        lUpper = -1
        Try

            lUpper = vArray.GetUpperBound(1)

        Catch
        End Try



        If lUpper > -1 Then
            For x As Integer = 0 To lUpper
                If m_sAccountNo.Trim() = "" Then

                    m_sAccountNo = CStr(vArray(0, x))

                    m_sSortCode = CStr(vArray(1, x))

                    m_sAccountName = CStr(vArray(2, x))
                End If
            Next x
        End If

        Exit Function
Err_GetBankDetailsFromParty:

        Return gPMConstants.PMEReturnCode.PMError

    End Function

    Public Function GetCompanyRegNo(ByRef v_lPartyCnt As Integer) As Integer




        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing
        Dim lUpper As Integer

        sSQL = "SELECT company_reg FROM Party_Corporate_Client WHERE party_cnt = " & (v_lPartyCnt).ToString()


        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), bStoredProcedure:=False, sSQLName:="BankAccount Details", vResultArray:=CType(vArray, Object))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Err_GetCompanyRegNo
        End If

        lUpper = -1
        Try

            lUpper = vArray.GetUpperBound(0)

        Catch
        End Try



        If lUpper > -1 Then
            For x As Integer = 0 To lUpper
                If m_sCompanyRegNo.Trim() = "" Then

                    m_sCompanyRegNo = CStr(vArray(0, x))
                End If
                Exit For
            Next x
        End If

        Exit Function
Err_GetCompanyRegNo:

        Return gPMConstants.PMEReturnCode.PMError
    End Function

    Public Function GetFinancePlanTransactions(ByRef vFinancePlanCnt As Object, ByRef vFinancePlanVersion As Object, ByRef vFinancePlanTransactionArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Build SQL String to get all of the fields from PFPremiumFinance
            With m_oDatabase


                .Parameters.Clear()

                ' Add the new one


                m_lReturn = m_oDatabase.Parameters.Add(sName:="financeplancnt", vValue:=CInt(vFinancePlanCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)




                m_lReturn = m_oDatabase.Parameters.Add(sName:="financeplanversion", vValue:=CInt(vFinancePlanVersion), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = m_oDatabase.SQLSelect(sSQL:="{call spe_PFTransaction_Id_sel (?,?)}", sSQLName:="GetTransactions", bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=CType(vFinancePlanTransactionArray, Object))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFinancePlanTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFinancePlanTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (DumpXML) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DumpXML(ByRef oDoc As XmlDocument) As Object
    'oDoc.Save("C:\xmldump.xml")
    'End Function


End Class

