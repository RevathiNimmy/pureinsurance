Option Strict Off
Option Explicit On
Imports System.Xml
Imports SSP.Shared
' developer guide no. 129
<System.Runtime.InteropServices.ProgId("Application_NET.Application")>
Public NotInheritable Class Application
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Application
    '
    ' Date: 23/08/2000
    '
    ' Description:
    '
    ' Edit History:
    '
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/09/2003
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

    Private m_sErrorDescription As String = ""

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Application"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer





        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
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

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

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
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: BankAccountValidation
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '          17/11/2000 CJB - Pass in v_sDataModelCode,v_sBusinessTypeCode and v_sQuoteRef
    '                           to be used to send emails when errors occur.
    '
    ' ***************************************************************** '
    Public Function BankAccountValidation(ByVal v_sSenderID As String, ByVal v_sCoverType As String, ByVal v_sGnetClientCode As String, ByVal v_sBusinessStatus As String, ByVal v_sBankAccountName As String, ByVal v_sBankAccountNo As String, ByVal v_sBankSortCode As String, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_sQuoteRef As String, ByRef r_sStatusCode As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oDocument As New XmlDocument
        Dim oRequest As XmlElement

        Dim sRequest As String = ""
        Dim sResponse As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start the Bank Account Validation Request
            lReturn = StartBankAccountValidation(r_oDocument:=oDocument, r_oRequestElem:=oRequest)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Add the GNet Element
            lReturn = AddGnetElement(r_oDocument:=oDocument, r_oParentElem:=oRequest, v_sSenderID:=v_sSenderID, v_sCoverType:=v_sCoverType, v_sGnetClientCode:=v_sGnetClientCode, v_sBusinessStatus:=v_sBusinessStatus)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Add the Bank Account Element
            lReturn = AddBankAccountElement(r_oDocument:=oDocument, r_oParentElem:=oRequest, v_sBankAccountName:=v_sBankAccountName, v_sBankAccountNo:=v_sBankAccountNo, v_sBankSortCode:=v_sBankSortCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the XML as a string
            sRequest = oDocument.InnerXml

            ' Process the request
            lReturn = ProcessRequest(v_sXMLRequest:=sRequest, v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_sQuoteRef:=v_sQuoteRef, r_sXMLResponse:=sResponse)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Return the response
            lReturn = UnscrambleResponseXML(v_sResponseXML:=sResponse, v_sExpectedResponse:=ACBANK_ACCOUNT_VALIDATION_RESPONSE, v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_sQuoteRef:=v_sQuoteRef, r_sStatusCode:=r_sStatusCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            oDocument = Nothing
            oRequest = Nothing

            'Check if bank a/c or sort code is invalid and return specific code to enable web pages to show specific error

            Select Case r_sStatusCode.Trim()
                Case GISPromptConstants.PromptInvalidAccount
                    result = CInt(GISPromptConstants.PromptBankAccountError)

                Case GISPromptConstants.PromptInvalidSortCode
                    result = CInt(GISPromptConstants.PromptSortCodeError)

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BankAccountValidation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountValidation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: PremiumFinanceTransact
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '          17/11/2000 CJB - Pass in v_sDataModelCode and v_sBusinessTypeCode
    '                           to be used to send emails when errors occur.
    '
    ' ***************************************************************** '
    Public Function PremiumFinanceTransact(ByVal v_sSenderID As String, ByVal v_sCoverType As String, ByVal v_sGnetClientCode As String, ByVal v_sBusinessStatus As String, ByVal v_cTotalPremiumAmount As Decimal, ByVal v_cOutstandingAmount As Decimal, ByVal v_sPaymentMethod As String, ByVal v_cInsurerPremium As Decimal, ByVal v_cSenderCharge As Decimal, ByVal v_cPMCharge As Decimal, ByVal v_cPromptAdminCharge As Decimal, ByVal v_sTitle As String, ByVal v_sCustomerName As String, ByVal v_sAddressLine1 As String, ByVal v_sAddressLine2 As String, ByVal v_sAddressLine3 As String, ByVal v_sAddressLine4 As String, ByVal v_sPostcode As String, ByVal v_sTelephoneNo As String, ByVal v_sEmailAddress As String, ByVal v_sGnetPolicyCode As String, ByVal v_sInsurerCode As String, ByVal v_sSchemeCode As String, ByVal v_sBrokerCode As String, ByVal v_sTermID As String, ByVal v_dtInceptionDate As Date, ByVal v_dtExpiryDate As Date, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef r_sStatusCode As String, ByRef r_sPremiumFinanceRef As String, ByRef r_sTransNumber As String, ByRef r_sLoanRenewalID As String, ByRef r_dtActualPaymentDate As Date, Optional ByVal v_vDepositAmount As Object = Nothing, Optional ByVal v_lInstalmentNo As Integer = -1, Optional ByVal v_lPreferredPaymentDate As Integer = -1, Optional ByVal v_vPolicyAdministratorCharge As Object = Nothing, Optional ByVal v_vInterestRate As Object = Nothing, Optional ByVal v_dtMTADate As Date = iGISSharedConstants.GISLowDate, Optional ByVal v_sBankAccountName As String = "", Optional ByVal v_sBankAccountNo As String = "", Optional ByVal v_sBankSortCode As String = "", Optional ByVal v_sCardDesc As String = "", Optional ByVal v_sCardAccountHolder As String = "", Optional ByVal v_sCardNumber As String = "", Optional ByVal v_dtCardStartDate As Date = iGISSharedConstants.GISLowDate, Optional ByVal v_dtCardExpiryDate As Date = iGISSharedConstants.GISLowDate, Optional ByVal v_sCardTitle As String = "", Optional ByVal v_lCardIssueNumber As Integer = -1) As Integer


        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oDocument As New XmlDocument
        Dim oRequest As XmlElement

        Dim sRequest As String = ""
        Dim sResponse As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Must have either Bank Account or Card Details depending on Payment Type
            If v_sPaymentMethod.Trim() = GISPromptConstants.PromptPaymentMethodDirectDebit Then
                If v_sBankAccountName.Trim() = "" Then
                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Bank Account Details found for Direct Debit Payment", vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceTransact")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                If v_sCardDesc.Trim() = "" Then
                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Payment Card Details found for Credit/Debit Card Payment", vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceTransact")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Start the Premium Finance Transact Request
            lReturn = StartPremiumFinanceTransact(r_oDocument:=oDocument, r_oRequestElem:=oRequest)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Add the GNet Element
            lReturn = AddGnetElement(r_oDocument:=oDocument, r_oParentElem:=oRequest, v_sSenderID:=v_sSenderID, v_sCoverType:=v_sCoverType, v_sGnetClientCode:=v_sGnetClientCode, v_sBusinessStatus:=v_sBusinessStatus)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If




            lReturn = AddPremiumElement(r_oDocument:=oDocument, r_oParentElem:=oRequest, v_cTotalPremiumAmount:=v_cTotalPremiumAmount, v_vDepositAmount:=CStr(v_vDepositAmount), v_cOutstandingAmount:=v_cOutstandingAmount, v_sPaymentMethod:=v_sPaymentMethod, v_cInsurerPremium:=v_cInsurerPremium, v_cSenderCharge:=v_cSenderCharge, v_cPMCharge:=v_cPMCharge, v_cPromptAdminCharge:=v_cPromptAdminCharge, v_lInstalmentNo:=v_lInstalmentNo, v_lPreferredPaymentDate:=v_lPreferredPaymentDate, v_vPolicyAdministratorCharge:=CStr(v_vPolicyAdministratorCharge), v_vInterestRate:=CStr(v_vInterestRate))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' If we have the Bank Account Name
            If v_sBankAccountName.Trim() <> "" Then
                ' Add the Bank Account Element
                lReturn = AddBankAccountElement(r_oDocument:=oDocument, r_oParentElem:=oRequest, v_sBankAccountName:=v_sBankAccountName, v_sBankAccountNo:=v_sBankAccountNo, v_sBankSortCode:=v_sBankSortCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            ' If we have the Payment Card Details
            If v_sCardDesc.Trim() <> "" Then
                ' Add the Payment Card Element
                lReturn = AddPaymentCardElement(r_oDocument:=oDocument, r_oParentElem:=oRequest, v_sCardDesc:=v_sCardDesc, v_sAccountHolder:=v_sCardAccountHolder, v_sCardNumber:=v_sCardNumber, v_dtStartDate:=v_dtCardStartDate, v_dtExpiryDate:=v_dtCardExpiryDate, v_sTitle:=v_sCardTitle, v_lIssueNumber:=v_lCardIssueNumber)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

            End If

            ' Add the Customer Element
            lReturn = AddCustomerElement(r_oDocument:=oDocument, r_oParentElem:=oRequest, v_sTitle:=v_sTitle, v_sCustomerName:=v_sCustomerName, v_sAddressLine1:=v_sAddressLine1, v_sAddressLine2:=v_sAddressLine2, v_sAddressLine3:=v_sAddressLine3, v_sAddressLine4:=v_sAddressLine4, v_sPostcode:=v_sPostcode, v_sTelephoneNo:=v_sTelephoneNo, v_sEmailAddress:=v_sEmailAddress)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Add the PolicyElement
            lReturn = AddPolicyElement(r_oDocument:=oDocument, r_oParentElem:=oRequest, v_sGnetPolicyCode:=v_sGnetPolicyCode, v_sInsurerCode:=v_sInsurerCode, v_sSchemeCode:=v_sSchemeCode, v_sBrokerCode:=v_sBrokerCode, v_sTermID:=v_sTermID, v_dtInceptionDate:=v_dtInceptionDate, v_dtMTADate:=v_dtMTADate, v_dtExpiryDate:=v_dtExpiryDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the XML as a string
            sRequest = oDocument.InnerXml

            ' Process the Request
            lReturn = ProcessRequest(v_sXMLRequest:=sRequest, v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_sPolicyNum:=v_sGnetPolicyCode, r_sXMLResponse:=sResponse)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Return the Response
            lReturn = UnscrambleResponseXML(v_sResponseXML:=sResponse, v_sExpectedResponse:=ACPREMIUM_FINANCE_TRANSACT_RESPONSE, v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_sPolicyNum:=v_sGnetPolicyCode, r_sStatusCode:=r_sStatusCode, r_sPremiumFinanceRef:=r_sPremiumFinanceRef, r_sTransNumber:=r_sTransNumber, r_sLoanRenewalID:=r_sLoanRenewalID, r_dtActualPaymentDate:=r_dtActualPaymentDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            oDocument = Nothing
            oRequest = Nothing

            'Check if bank a/c or sort code is invalid and return specific code to enable web pages to show specific error

            Select Case r_sStatusCode.Trim()
                Case GISPromptConstants.PromptInvalidAccount
                    result = CInt(GISPromptConstants.PromptBankAccountError)

                Case GISPromptConstants.PromptInvalidSortCode
                    result = CInt(GISPromptConstants.PromptSortCodeError)

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PremiumFinanceTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceTransact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: PremiumFinanceQuote
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '          17/11/2000 CJB - Pass in v_sDataModelCode and v_sBusinessTypeCode
    '                           to be used to send emails when errors occur.
    '          07/12/2001 CJB - Complete rewrite - old message never used.
    '
    ' ***************************************************************** '
    Public Function PremiumFinanceQuote(ByVal v_vDataModelCode As Object, ByVal v_vBusinessTypeCode As Object, ByVal v_vBusinessStatus As String, ByVal v_vPremiumAmount As String, ByVal v_vPremiumFinanceRef As String, ByVal v_vEffectiveDate As String, ByVal v_vPolicyNo As Object, ByRef r_vStatusCode As String, ByRef r_vStatusExplanation As String, ByRef r_vTotalPayable As Object, ByRef r_vNumberOfInstalmentsLeft As Object, ByRef r_vFirstInstalAmt As Object, ByRef r_vSubsequentInstalAmt As Object, ByRef r_vActualPaymentDate As Date, ByRef r_vInterestAmount As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oDocument As New XmlDocument
        Dim oRequest, oNewElem As XmlElement

        Dim sRequest As String = ""
        Dim sResponse As String = ""
        Dim sStatusCode As String = ""
        Dim dtActualPaymentDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start the Premium Finance Quote Request
            lReturn = CType(StartPremiumFinanceQuote(r_oDocument:=oDocument, r_oRequestElem:=oRequest), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Add the elements required
            oNewElem = oDocument.CreateElement(ACBUSINESS_STATUS)
            oNewElem.InnerText = v_vBusinessStatus.Trim()

            oRequest.AppendChild(oNewElem)

            oNewElem = oDocument.CreateElement(ACTOTAL_PREMIUM_AMT)
            oNewElem.InnerText = v_vPremiumAmount.Trim()

            oRequest.AppendChild(oNewElem)

            oNewElem = oDocument.CreateElement(ACPREMIUM_FINANCE_REF)
            oNewElem.InnerText = v_vPremiumFinanceRef.Trim()

            oRequest.AppendChild(oNewElem)

            oNewElem = oDocument.CreateElement(ACEFFECTIVE_DATE)
            oNewElem.InnerText = v_vEffectiveDate.Trim()

            oRequest.AppendChild(oNewElem)

            oNewElem = Nothing

            ' Get the XML as a string
            sRequest = oDocument.InnerXml

            ' Process the request



            lReturn = CType(ProcessRequest(v_sXMLRequest:=sRequest, v_sDataModelCode:=CStr(v_vDataModelCode), v_sBusinessTypeCode:=CStr(v_vBusinessTypeCode), v_sPolicyNum:=CStr(v_vPolicyNo), r_sXMLResponse:=sResponse), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'Return the error description from the ProcessRequest function error handler to the web pages
                r_vStatusExplanation = m_sErrorDescription

                Return lReturn
            End If

            ' Return the Response








            lReturn = CType(UnscrambleResponseXML(v_sResponseXML:=sResponse, v_sExpectedResponse:=ACPREMIUM_FINANCE_QUOTE_RESPONSE, v_sDataModelCode:=CStr(v_vDataModelCode), v_sBusinessTypeCode:=CStr(v_vBusinessTypeCode), r_sStatusCode:=sStatusCode, v_sPolicyNum:=CStr(v_vPolicyNo), r_vStatusExplanation:=r_vStatusExplanation, r_vTotalPayable:=CStr(r_vTotalPayable), r_vRemainingInst:=CStr(r_vNumberOfInstalmentsLeft), r_vFirstInstalAmt:=CStr(r_vFirstInstalAmt), r_vSubsequentInstalAmt:=CStr(r_vSubsequentInstalAmt), r_dtActualPaymentDate:=dtActualPaymentDate, r_vInterestAmount:=CStr(r_vInterestAmount)), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            oDocument = Nothing
            oRequest = Nothing

            'Cast these o/p vars back - as they were ByRef then they needed to be the correct type if they
            'were to be used direct into the UnscrambleResponseXML function. We couldn't change them in there
            'as they're already being used in NB...
            r_vStatusCode = sStatusCode
            r_vActualPaymentDate = dtActualPaymentDate

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PremiumFinanceQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)

    ' FRIEND Methods (Begin)

    ' FRIEND Methods (End)

    ' PRIVATE Methods (Begin)


    ' ***************************************************************** '
    '
    ' Name: ProcessRequest
    '
    ' Description:
    '
    ' History: 24/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    'AS per requirement to remove MSXML (https://sspengineering.visualstudio.com/SspEngineering/_workitems/edit/96304) 
    'functionality of ProcessRequest() is removed as this is no longer in use.
    ' ***************************************************************** '
    Private Function ProcessRequest(ByVal v_sXMLRequest As String, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef r_sXMLResponse As String, Optional ByVal v_sQuoteRef As String = "", Optional ByVal v_sPolicyNum As String = "") As Integer

        Dim errorMessage As String = "This functionality is no longer in use. Please contact administrator for further assistance."
        'Log Error Message
        iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=errorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRequest")
        Return gPMConstants.PMEReturnCode.PMError
    End Function




    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()


        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Log Error Message
        'iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
    ' Name: PremiumFinanceMTATransact
    '
    ' Description:
    '
    ' History: 07/12/2001 CJB - Created - was previously going to use
    '                           PremiumFinanceTransact message for MTAs too but
    '                           overkill so using this much smaller message.
    '
    ' ***************************************************************** '
    Public Function PremiumFinanceMTATransact(ByVal v_vDataModelCode As Object, ByVal v_vBusinessTypeCode As Object, ByVal v_vPremiumAmount As String, ByVal v_vPremiumFinanceRef As String, ByVal v_vEffectiveDate As String, ByVal v_vPolicyNo As Object, ByRef r_vStatusCode As String, ByRef r_vStatusExplanation As String, ByRef r_vTotalPayable As Object, ByRef r_vNumberOfInstalmentsLeft As Object, ByRef r_vFirstInstalAmt As Object, ByRef r_vSubsequentInstalAmt As Object, ByRef r_vActualPaymentDate As Date, ByRef r_vNewPremiumFinanceRef As String, ByRef r_vInterestAmount As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oDocument As New XmlDocument
        Dim oRequest, oNewElem As XmlElement

        Dim sRequest As String = ""
        Dim sResponse As String = ""
        Dim sStatusCode As String = ""
        Dim dtActualPaymentDate As Date
        Dim sNewPremiumFinanceRef As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start the Premium Finance Transact MTA Request
            lReturn = StartPremiumFinanceMTATransact(r_oDocument:=oDocument, r_oRequestElem:=oRequest)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Add the elements required
            oNewElem = oDocument.CreateElement(ACTOTAL_PREMIUM_AMT)
            oNewElem.InnerText = v_vPremiumAmount.Trim()

            oRequest.AppendChild(oNewElem)

            oNewElem = oDocument.CreateElement(ACPREMIUM_FINANCE_REF)
            oNewElem.InnerText = v_vPremiumFinanceRef.Trim()

            oRequest.AppendChild(oNewElem)

            oNewElem = oDocument.CreateElement(ACEFFECTIVE_DATE)
            oNewElem.InnerText = v_vEffectiveDate.Trim()

            oRequest.AppendChild(oNewElem)

            oNewElem = Nothing

            ' Get the XML as a string
            sRequest = oDocument.InnerXml

            ' Process the request



            lReturn = ProcessRequest(v_sXMLRequest:=sRequest, v_sDataModelCode:=CStr(v_vDataModelCode), v_sBusinessTypeCode:=CStr(v_vBusinessTypeCode), v_sPolicyNum:=CStr(v_vPolicyNo), r_sXMLResponse:=sResponse)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'Return the error description from the ProcessRequest function error handler to the web pages
                r_vStatusExplanation = m_sErrorDescription

                Return lReturn
            End If

            ' Return the Response








            lReturn = UnscrambleResponseXML(v_sResponseXML:=sResponse, v_sExpectedResponse:=ACPREMIUM_FINANCE_MTA_TRANSACT_RESPONSE, v_sDataModelCode:=CStr(v_vDataModelCode), v_sBusinessTypeCode:=CStr(v_vBusinessTypeCode), r_sStatusCode:=sStatusCode, v_sPolicyNum:=CStr(v_vPolicyNo), r_vStatusExplanation:=r_vStatusExplanation, r_vTotalPayable:=CStr(r_vTotalPayable), r_vRemainingInst:=CStr(r_vNumberOfInstalmentsLeft), r_vFirstInstalAmt:=CStr(r_vFirstInstalAmt), r_vSubsequentInstalAmt:=CStr(r_vSubsequentInstalAmt), r_dtActualPaymentDate:=dtActualPaymentDate, r_sPremiumFinanceRef:=sNewPremiumFinanceRef, r_vInterestAmount:=CStr(r_vInterestAmount))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_vStatusCode = sStatusCode
                Return lReturn
            End If

            oDocument = Nothing
            oRequest = Nothing

            'Cast these o/p vars back - as they were ByRef then they needed to be the correct type if they
            'were to be used direct into the UnscrambleResponseXML function. We couldn't change them in there
            'as they're already being used in NB...
            r_vStatusCode = sStatusCode
            r_vActualPaymentDate = dtActualPaymentDate
            r_vNewPremiumFinanceRef = sNewPremiumFinanceRef
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PremiumFinanceMTATransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceMTATransact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PremiumFinanceCancellation
    '
    ' Description: Send a Cancellation Request XML message to Prompt to
    '              cancel the finance.
    '
    ' History: 25/08/2002 CJB - Created
    '
    ' ***************************************************************** '
    Public Function PremiumFinanceCancellation(ByVal v_vDataModelCode As Object, ByVal v_vBusinessTypeCode As Object, ByVal v_vPremiumFinanceRef As String, ByVal v_vCancellationDate As String, ByVal v_vPolicyNo As Object, ByRef r_vStatusCode As String, ByRef r_vStatusExplanation As String, ByRef r_vCancellationAmount As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oDocument As New XmlDocument
        Dim oRequest, oNewElem As XmlElement

        Dim sRequest As String = ""
        Dim sResponse As String = ""
        Dim sStatusCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start the Premium Finance Cancellation Request
            lReturn = StartPremiumFinanceCancellation(r_oDocument:=oDocument, r_oRequestElem:=oRequest)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Add the elements required
            oNewElem = oDocument.CreateElement(ACPREMIUM_FINANCE_REF)
            oNewElem.InnerText = v_vPremiumFinanceRef.Trim()

            oRequest.AppendChild(oNewElem)

            oNewElem = oDocument.CreateElement(ACCANCELLATION_DATE)
            oNewElem.InnerText = v_vCancellationDate.Trim()

            oRequest.AppendChild(oNewElem)

            oNewElem = Nothing

            ' Get the XML as a string
            sRequest = oDocument.InnerXml

            ' Process the request



            lReturn = ProcessRequest(v_sXMLRequest:=sRequest, v_sDataModelCode:=CStr(v_vDataModelCode), v_sBusinessTypeCode:=CStr(v_vBusinessTypeCode), v_sPolicyNum:=CStr(v_vPolicyNo), r_sXMLResponse:=sResponse)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'Return the error description from the ProcessRequest function error handler to the web pages
                r_vStatusExplanation = m_sErrorDescription

                Return lReturn
            End If

            ' Return the Response




            lReturn = UnscrambleResponseXML(v_sResponseXML:=sResponse, v_sExpectedResponse:=ACPREMIUM_FINANCE_CANCELLATION_RESPONSE, v_sDataModelCode:=CStr(v_vDataModelCode), v_sBusinessTypeCode:=CStr(v_vBusinessTypeCode), r_sStatusCode:=sStatusCode, v_sPolicyNum:=CStr(v_vPolicyNo), r_vStatusExplanation:=r_vStatusExplanation, r_vCancellationAmount:=CStr(r_vCancellationAmount))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_vStatusCode = sStatusCode
                Return lReturn
            End If

            oDocument = Nothing
            oRequest = Nothing

            'Cast these o/p vars back - as they were ByRef then they needed to be the correct type if they
            'were to be used direct into the UnscrambleResponseXML function. We couldn't change them in there
            'as they're already being used in NB...
            r_vStatusCode = sStatusCode
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PremiumFinanceCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceCancellation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
End Class

