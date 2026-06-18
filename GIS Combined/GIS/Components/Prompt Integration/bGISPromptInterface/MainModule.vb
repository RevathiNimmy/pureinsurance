Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.Xml
Imports SSP.Shared
'developer guide no. 129
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  23/08/2000
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bGISPromptInterface"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"



    ' UserID

    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Public g_sXMLRequest As String
    '***********************************************
    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Public g_sErrorDescription As String
    '***********************************************
    Public Const ACDateTimeFormat As String = "dd/mm/yyyy hh:nn:ss"
    Public Const ACDateFormat As String = "dd/mm/yyyy"
    Public Const ACDateMonthYearFormat As String = "mmyy"
    Public Const ACDateMonthCenturyYearFormat As String = "mmyyyy"
    Public Const ACTimeFormat As String = "hh:nn:ss"

    ' XML Element Constants
    Public Const ACSTATUS_CODE As String = "STATUS_CODE"
    Public Const ACSTATUS_EXPLANATION As String = "STATUS_EXPLANATION"
    Public Const ACPREMIUM_FINANCE_REF As String = "PREMIUM_FINANCE_REF"
    Public Const ACTRANS_NUMBER As String = "TRANS_NUMBER"
    Public Const ACLOAN_RENEWAL_ID As String = "LOAN_RENEWAL_ID"
    Public Const ACACTUAL_PAYMENT_DATE As String = "ACTUAL_PAYMENT_DATE"
    Public Const ACMTA_AMT As String = "MTA_AMT"
    Public Const ACREMAINING_INST As String = "REMAINING_INST"
    Public Const ACFIRST_INSTAL_AMT As String = "FIRST_INSTAL_AMT"
    Public Const ACSUBSEQUENT_INSTAL_AMT As String = "SUBSEQUENT_INSTAL_AMT"
    Public Const ACSENDER_ID As String = "SENDER_ID"
    Public Const ACCOVER_TYPE As String = "COVER_TYPE"
    Public Const ACGNET_CLIENT_CODE As String = "GNET_CLIENT_CODE"
    Public Const ACBUSINESS_STATUS As String = "BUSINESS_STATUS"
    Public Const ACTITLE As String = "TITLE"
    Public Const ACCUSTOMER_NAME As String = "CUSTOMER_NAME"
    Public Const ACADDRESS_LINE_1 As String = "ADDRESS_LINE_1"
    Public Const ACADDRESS_LINE_2 As String = "ADDRESS_LINE_2"
    Public Const ACADDRESS_LINE_3 As String = "ADDRESS_LINE_3"
    Public Const ACADDRESS_LINE_4 As String = "ADDRESS_LINE_4"
    Public Const ACPOSTCODE As String = "POSTCODE"
    Public Const ACTELEPHONE_NO As String = "TELEPHONE_NO"
    Public Const ACEMAIL_ADDRESS As String = "EMAIL_ADDRESS"
    Public Const ACBANK_AC_NAME As String = "BANK_AC_NAME"
    Public Const ACBANK_AC_NO As String = "BANK_AC_NO"
    Public Const ACBANK_AC_SORT_CODE As String = "BANK_AC_SORT_CODE"
    Public Const ACCARD_TYPE As String = "CARD_TYPE"
    Public Const ACCARD_DESC As String = "CARD_DESC"
    Public Const ACACCOUNT_HOLDER As String = "ACCOUNT_HOLDER"
    Public Const ACCARD_NUMBER As String = "CARD_NUMBER"
    Public Const ACSTART_DATE As String = "START_DATE"
    Public Const ACEFFECTIVE_DATE As String = "EFFECTIVE_DATE"
    Public Const ACCANCELLATION_DATE As String = "CANCELLATION_DATE"
    Public Const ACEXPIRY_DATE As String = "EXPIRY_DATE"
    Public Const ACISSUE_NUMBER As String = "ISSUE_NUMBER"
    Public Const ACMTA_AMOUNT As String = "MTA_AMOUNT"
    Public Const ACARREARS_AMOUNT As String = "ARREARS_AMOUNT"
    Public Const ACGNET_POLICY_CODE As String = "GNET_POLICY_CODE"
    Public Const ACINSURER_CODE As String = "INSURER_CODE"
    Public Const ACSCHEME_CODE As String = "SCHEME_CODE"
    Public Const ACBROKER_CODE As String = "BROKER_CODE"
    Public Const ACTERM_ID As String = "TERM_ID"
    Public Const ACINCEPTION_DATE As String = "INCEPTION_DATE"
    Public Const ACMTA_DATE As String = "MTA_DATE"
    Public Const ACTOTAL_PREMIUM_AMT As String = "TOTAL_PREMIUM_AMT"
    Public Const ACDEPOSIT_AMT As String = "DEPOSIT_AMT"
    Public Const ACOUTSTANDING_AMT As String = "OUTSTANDING_AMT"
    Public Const ACPAYMENT_METHOD As String = "PAYMENT_METHOD"
    Public Const ACINSTALMENT_NO As String = "INSTALMENT_NO"
    Public Const ACPREFERRED_PAYMENT_DATE As String = "PREFERRED_PAYMENT_DATE"
    Public Const ACINSURER_PREMIUM As String = "INSURER_PREMIUM"
    Public Const ACSENDER_CHARGE As String = "SENDER_CHARGE"
    Public Const ACPM_CHARGE As String = "PM_CHARGE"
    Public Const ACPROMPT_ADMIN_CHARGE As String = "PROMPT_ADMIN_CHARGE"
    Public Const ACPOLICY_ADMINISTRATOR_CHARGE As String = "POLICY_ADMINISTRATOR_CHARGE"
    Public Const ACINTEREST_RATE As String = "INTEREST_RATE"
    Public Const ACREASON As String = "REASON"
    Public Const ACSOURCE_TEXT As String = "SOURCE_TEXT"
    Public Const ACLINE_NO As String = "LINE_NO"
    Public Const ACLINE_POS As String = "LINE_POS"
    Public Const ACELEMENT_NAME As String = "ELEMENT_NAME"
    Public Const ACSUPPLIED_VALUE As String = "SUPPLIED_VALUE"
    Public Const ACERROR_NUMBER As String = "ERROR_NUMBER"
    Public Const ACDESCRIPTION As String = "DESCRIPTION"
    Public Const ACTOTAL_PAYABLE As String = "TOTAL_PAYABLE"
    Public Const ACNUMBER_OF_INSTALMENTS_LEFT As String = "NUMBER_OF_INSTALMENTS_LEFT"

    Public Const ACGNET_PROMPT_INTERFACE As String = "GNET_PROMPT_INTERFACE"
    Public Const ACBANK_ACCOUNT_VALIDATION_REQUEST As String = "BANK_ACCOUNT_VALIDATION_REQUEST"
    Public Const ACPREMIUM_FINANCE_TRANSACT_REQUEST As String = "PREMIUM_FINANCE_TRANSACT_REQUEST"
    Public Const ACPREMIUM_FINANCE_MTA_TRANSACT_REQUEST As String = "PREMIUM_FINANCE_MTA_TRANSACT_REQUEST"
    Public Const ACPREMIUM_FINANCE_CANCELLATION_REQUEST As String = "PREMIUM_FINANCE_CANCELLATION_REQUEST"
    Public Const ACPREMIUM_FINANCE_QUOTE_REQUEST As String = "PREMIUM_FINANCE_QUOTE_REQUEST"
    Public Const ACGNET_ELEMENT As String = "GNET_ELEMENT"
    Public Const ACBANK_ACCOUNT_ELEMENT As String = "BANK_ACCOUNT_ELEMENT"
    Public Const ACPREMIUM_ELEMENT As String = "PREMIUM_ELEMENT"
    Public Const ACPREMIUM_BREAKDOWN As String = "PREMIUM_BREAKDOWN"
    Public Const ACPOLICY_ELEMENT As String = "POLICY_ELEMENT"
    Public Const ACPAYMENT_CARD_ELEMENT As String = "PAYMENT_CARD_ELEMENT"
    Public Const ACCUSTOMER_ELEMENT As String = "CUSTOMER_ELEMENT"

    Public Const ACERROR_RESPONSE As String = "ERROR_RESPONSE"
    Public Const ACBANK_ACCOUNT_VALIDATION_RESPONSE As String = "BANK_ACCOUNT_VALIDATION_RESPONSE"
    Public Const ACPREMIUM_FINANCE_TRANSACT_RESPONSE As String = "PREMIUM_FINANCE_TRANSACT_RESPONSE"
    Public Const ACPREMIUM_FINANCE_MTA_TRANSACT_RESPONSE As String = "PREMIUM_FINANCE_MTA_TRANSACT_RESPONSE"
    Public Const ACPREMIUM_FINANCE_CANCELLATION_RESPONSE As String = "PREMIUM_FINANCE_CANCELLATION_RESPONSE"
    Public Const ACPREMIUM_FINANCE_QUOTE_RESPONSE As String = "PREMIUM_FINANCE_QUOTE_RESPONSE"

    ' RFC250900 - Set the Message Version Number
    Public Const ACVERSION_NO As String = "VERSION_NO"

    Public Const ACXMLVersion As String = "GNET_PROMPT_INTERFACE"

    'PMSecurity Constants
    Public Const TMPPMKey As String = "978967896578976"


    ' ***************************************************************** '
    '
    ' Name: StartBankAccountValidation
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function StartBankAccountValidation(ByRef r_oDocument As XmlDocument, ByRef r_oRequestElem As XmlElement) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Document
            r_oDocument = New XmlDocument()

            ' Create the top level GNET PROMP ELEMTN
            r_oRequestElem = r_oDocument.CreateElement(ACGNET_PROMPT_INTERFACE)

            ' RFC250900 - Set the Message Versnio Number
            r_oRequestElem.SetAttribute(ACVERSION_NO, ACXMLVersion)

            ' Set it as the Document Element
            If Not (r_oDocument.DocumentElement Is Nothing) Then
                r_oDocument.RemoveChild(r_oDocument.DocumentElement)
            End If
            r_oDocument.AppendChild(r_oRequestElem)

            ' Create the Bank Account Validation Element
            r_oRequestElem = r_oDocument.CreateElement(ACBANK_ACCOUNT_VALIDATION_REQUEST)

            ' Append it as a child of the Document Element

            r_oDocument.DocumentElement.AppendChild(r_oRequestElem)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartBankAccountValidation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartBankAccountValidation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: StartPremiumFinanceTransact
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function StartPremiumFinanceTransact(ByRef r_oDocument As XmlDocument, ByRef r_oRequestElem As XmlElement) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Document
            r_oDocument = New XmlDocument()

            ' Create the top level GNET PROMP ELEMTN
            r_oRequestElem = r_oDocument.CreateElement(ACGNET_PROMPT_INTERFACE)

            ' RFC250900 - Set the Message Versnio Number
            r_oRequestElem.SetAttribute(ACVERSION_NO, ACXMLVersion)

            ' Set it as the Document Element
            If Not (r_oDocument.DocumentElement Is Nothing) Then
                r_oDocument.RemoveChild(r_oDocument.DocumentElement)
            End If
            r_oDocument.AppendChild(r_oRequestElem)

            ' Create the Bank Account Validation Element
            r_oRequestElem = r_oDocument.CreateElement(ACPREMIUM_FINANCE_TRANSACT_REQUEST)

            ' Append it as a child of the Document Element

            r_oDocument.DocumentElement.AppendChild(r_oRequestElem)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartPremiumFinanceTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartPremiumFinanceTransact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: StartPremiumFinanceQuote
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function StartPremiumFinanceQuote(ByRef r_oDocument As XmlDocument, ByRef r_oRequestElem As XmlElement) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Document
            r_oDocument = New XmlDocument()

            ' Create the top level GNET PROMP ELEMTN
            r_oRequestElem = r_oDocument.CreateElement(ACGNET_PROMPT_INTERFACE)

            ' RFC250900 - Set the Message Versnio Number
            r_oRequestElem.SetAttribute(ACVERSION_NO, ACXMLVersion)

            ' Set it as the Document Element
            If Not (r_oDocument.DocumentElement Is Nothing) Then
                r_oDocument.RemoveChild(r_oDocument.DocumentElement)
            End If
            r_oDocument.AppendChild(r_oRequestElem)

            ' Create the Premium Finance Quote Request Element
            r_oRequestElem = r_oDocument.CreateElement(ACPREMIUM_FINANCE_QUOTE_REQUEST)

            ' Append it as a child of the Document Element

            r_oDocument.DocumentElement.AppendChild(r_oRequestElem)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartPremiumFinanceQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartPremiumFinanceQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: AddGnetElement
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function AddGnetElement(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_sSenderID As String, ByVal v_sCoverType As String, ByVal v_sGnetClientCode As String, ByVal v_sBusinessStatus As String) As Integer

        Dim result As Integer = 0
        Dim oNewElem, oValue As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Craet the New Element
            oNewElem = r_oDocument.CreateElement(ACGNET_ELEMENT)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACSENDER_ID)
            oValue.InnerText = v_sSenderID.Trim()

            oNewElem.AppendChild(oValue)

            ' RFC250900 - Add COVER_TYPE to GNet Element
            oValue = r_oDocument.CreateElement(ACCOVER_TYPE)
            oValue.InnerText = v_sCoverType.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACGNET_CLIENT_CODE)
            oValue.InnerText = v_sGnetClientCode.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACBUSINESS_STATUS)
            oValue.InnerText = v_sBusinessStatus.Trim()

            oNewElem.AppendChild(oValue)

            ' Add the New Element to its Parent

            r_oParentElem.AppendChild(oNewElem)

            oNewElem = Nothing
            oValue = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddGnetElement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGnetElement", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddBankAccountElement
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function AddBankAccountElement(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_sBankAccountName As String, ByVal v_sBankAccountNo As String, ByVal v_sBankSortCode As String) As Integer

        Dim result As Integer = 0
        Dim oNewElem, oValue As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Craet the New Element
            oNewElem = r_oDocument.CreateElement(ACBANK_ACCOUNT_ELEMENT)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACBANK_AC_NAME)
            oValue.InnerText = v_sBankAccountName.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACBANK_AC_NO)
            oValue.InnerText = v_sBankAccountNo.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACBANK_AC_SORT_CODE)
            oValue.InnerText = v_sBankSortCode.Trim()

            oNewElem.AppendChild(oValue)

            ' Add the New Element to its Parent

            r_oParentElem.AppendChild(oNewElem)

            oNewElem = Nothing
            oValue = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddBankAccountElement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddBankAccountElement", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddPremiumElement
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function AddPremiumElement(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_cTotalPremiumAmount As Decimal, ByVal v_cOutstandingAmount As Decimal, ByVal v_sPaymentMethod As String, ByVal v_cInsurerPremium As Decimal, ByVal v_cSenderCharge As Decimal, ByVal v_cPMCharge As Decimal, ByVal v_cPromptAdminCharge As Decimal, Optional ByVal v_lInstalmentNo As Integer = -1, Optional ByVal v_lPreferredPaymentDate As Integer = -1, Optional ByVal v_vDepositAmount As String = "", Optional ByVal v_vPolicyAdministratorCharge As String = "", Optional ByVal v_vInterestRate As String = "") As Integer

        Dim result As Integer = 0
        Dim oNewElem, oBreakdown, oValue As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Validate the Parameters
            v_sPaymentMethod = v_sPaymentMethod.Trim()

            ' Payment Method
            Select Case v_sPaymentMethod
                ' DD, DC or CC
                Case GISPromptConstants.PromptPaymentMethodDirectDebit, GISPromptConstants.PromptPaymentMethodDebitCard, GISPromptConstants.PromptPaymentMethodCreditCard
                    ' All OK

                    ' Error
                Case Else
                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Payment Method must be DD, DC or CC. Supplied Value = " & v_sPaymentMethod, vApp:=ACApp, vClass:=ACClass, vMethod:="AddPremiumElement")
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

            ' Preferred Payment Date
            If v_lPreferredPaymentDate <> -1 Then
                ' Preferred Payment Date Must be between 1 & 28
                If v_lPreferredPaymentDate < 1 Or v_lPreferredPaymentDate > 28 Then
                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Preferred Payment Date must be between 1 & 28 inc. Supplied Value = " & v_lPreferredPaymentDate, vApp:=ACApp, vClass:=ACClass, vMethod:="AddPremiumElement")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Deposit Amount

            If Not Informations.IsNothing(v_vDepositAmount) Then
                ' Must Be Numeric
                Dim dbNumericTemp As Double
                If Not Double.TryParse(v_vDepositAmount, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Deposit Amount MUST be numeric if supplied. Supplied Value = " & v_vDepositAmount, vApp:=ACApp, vClass:=ACClass, vMethod:="AddPremiumElement")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Policy Administrator Charge

            If Not Informations.IsNothing(v_vPolicyAdministratorCharge) Then
                ' Must Be Numeric
                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(v_vPolicyAdministratorCharge, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Policy Administrator Charge MUST be numeric if supplied. Supplied Value = " & v_vPolicyAdministratorCharge, vApp:=ACApp, vClass:=ACClass, vMethod:="AddPremiumElement")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' APR Charge

            If Not Informations.IsNothing(v_vInterestRate) Then
                ' Must Be Numeric
                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(v_vInterestRate, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="APR Charge MUST be numeric if supplied. Supplied Value = " & v_vInterestRate, vApp:=ACApp, vClass:=ACClass, vMethod:="AddPremiumElement")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Create the New Element
            oNewElem = r_oDocument.CreateElement(ACPREMIUM_ELEMENT)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACTOTAL_PREMIUM_AMT)
            oValue.InnerText = CStr(v_cTotalPremiumAmount).Trim()

            oNewElem.AppendChild(oValue)

            ' RFC250900 - Add Deposit Amount & Outstanding Amount to Premium Elem

            If Not Informations.IsNothing(v_vDepositAmount) Then
                oValue = r_oDocument.CreateElement(ACDEPOSIT_AMT)
                oValue.InnerText = v_vDepositAmount.Trim()

                oNewElem.AppendChild(oValue)
            End If

            ' RFC250900 - Add Deposit Amount & Outstanding Amount to Premium Elem
            oValue = r_oDocument.CreateElement(ACOUTSTANDING_AMT)
            oValue.InnerText = CStr(v_cOutstandingAmount).Trim()

            oNewElem.AppendChild(oValue)

            ' Create the Premium Breakdown Elements
            oBreakdown = r_oDocument.CreateElement(ACPREMIUM_BREAKDOWN)

            oNewElem.AppendChild(oBreakdown)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACPAYMENT_METHOD)
            oValue.InnerText = v_sPaymentMethod.Trim()

            oNewElem.AppendChild(oValue)

            If v_lInstalmentNo > 0 Then
                ' Create the child value elements
                oValue = r_oDocument.CreateElement(ACINSTALMENT_NO)
                oValue.InnerText = CStr(v_lInstalmentNo)

                oNewElem.AppendChild(oValue)
            End If

            If v_lPreferredPaymentDate > 0 Then
                ' Create the child value elements
                oValue = r_oDocument.CreateElement(ACPREFERRED_PAYMENT_DATE)
                oValue.InnerText = CStr(v_lPreferredPaymentDate)

                oNewElem.AppendChild(oValue)
            End If

            ' Add the Breakdown Child Values

            oValue = r_oDocument.CreateElement(ACINSURER_PREMIUM)
            oValue.InnerText = CStr(v_cInsurerPremium).Trim()

            oBreakdown.AppendChild(oValue)

            oValue = r_oDocument.CreateElement(ACSENDER_CHARGE)
            oValue.InnerText = CStr(v_cSenderCharge).Trim()

            oBreakdown.AppendChild(oValue)

            oValue = r_oDocument.CreateElement(ACPM_CHARGE)
            oValue.InnerText = CStr(v_cPMCharge).Trim()

            oBreakdown.AppendChild(oValue)

            oValue = r_oDocument.CreateElement(ACPROMPT_ADMIN_CHARGE)
            oValue.InnerText = CStr(v_cPromptAdminCharge).Trim()

            oBreakdown.AppendChild(oValue)


            If Not Informations.IsNothing(v_vPolicyAdministratorCharge) Then
                oValue = r_oDocument.CreateElement(ACPOLICY_ADMINISTRATOR_CHARGE)
                oValue.InnerText = v_vPolicyAdministratorCharge.Trim()

                oBreakdown.AppendChild(oValue)
            End If


            If Not Informations.IsNothing(v_vInterestRate) Then
                oValue = r_oDocument.CreateElement(ACINTEREST_RATE)
                oValue.InnerText = v_vInterestRate.Trim()

                oBreakdown.AppendChild(oValue)
            End If

            ' Add the New Element to its Parent

            r_oParentElem.AppendChild(oNewElem)

            oNewElem = Nothing
            oBreakdown = Nothing
            oValue = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPremiumElement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPremiumElement", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddPolicyElement
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function AddPolicyElement(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_sGnetPolicyCode As String, ByVal v_sInsurerCode As String, ByVal v_sSchemeCode As String, ByVal v_sBrokerCode As String, ByVal v_sTermID As String, ByVal v_dtInceptionDate As Date, ByVal v_dtExpiryDate As Date, Optional ByVal v_dtMTADate As Date = iGISSharedConstants.GISLowDate) As Integer

        Dim result As Integer = 0
        Dim oNewElem, oValue As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Craet the New Element
            oNewElem = r_oDocument.CreateElement(ACPOLICY_ELEMENT)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACGNET_POLICY_CODE)
            oValue.InnerText = v_sGnetPolicyCode.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACINSURER_CODE)
            oValue.InnerText = v_sInsurerCode.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACSCHEME_CODE)
            oValue.InnerText = v_sSchemeCode.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACBROKER_CODE)
            oValue.InnerText = v_sBrokerCode.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACTERM_ID)
            oValue.InnerText = v_sTermID.Trim()

            oNewElem.AppendChild(oValue)

            If v_dtInceptionDate > iGISSharedConstants.GISLowDate Then
                oValue = r_oDocument.CreateElement(ACINCEPTION_DATE)
                oValue.InnerText = StringsHelper.Format(v_dtInceptionDate, ACDateFormat)

                oNewElem.AppendChild(oValue)
            End If

            If v_dtMTADate > iGISSharedConstants.GISLowDate Then
                oValue = r_oDocument.CreateElement(ACMTA_DATE)
                oValue.InnerText = StringsHelper.Format(v_dtMTADate, ACDateFormat)

                oNewElem.AppendChild(oValue)
            End If

            If v_dtExpiryDate > iGISSharedConstants.GISLowDate Then
                oValue = r_oDocument.CreateElement(ACEXPIRY_DATE)
                oValue.InnerText = StringsHelper.Format(v_dtExpiryDate, ACDateFormat)

                oNewElem.AppendChild(oValue)
            End If

            ' Add the New Element to its Parent

            r_oParentElem.AppendChild(oNewElem)

            oNewElem = Nothing
            oValue = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPolicyElement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPolicyElement", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: AddPaymentCardElement
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function AddPaymentCardElement(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_sCardDesc As String, ByVal v_sAccountHolder As String, ByVal v_sCardNumber As String, ByVal v_dtStartDate As Date, ByVal v_dtExpiryDate As Date, Optional ByVal v_sTitle As String = "", Optional ByVal v_lIssueNumber As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim oNewElem, oValue As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Card Type is Switch then Issue Number is Mandatory
            If v_sCardDesc.Trim() = GISPromptConstants.PromptCardDescSwitch Then
                If v_lIssueNumber < 1 Then
                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Issue Number is Mandatory for Switch Card", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPaymentCardElement")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_lIssueNumber > 99 Then
                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Issue Number is invalid. Supplied Value = " & v_lIssueNumber, vApp:=ACApp, vClass:=ACClass, vMethod:="AddPaymentCardElement")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create the New Element
            oNewElem = r_oDocument.CreateElement(ACPAYMENT_CARD_ELEMENT)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACCARD_DESC)
            oValue.InnerText = v_sCardDesc.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            If v_sTitle <> "" Then
                oValue = r_oDocument.CreateElement(ACTITLE)
                oValue.InnerText = v_sTitle.Trim()

                oNewElem.AppendChild(oValue)
            End If

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACACCOUNT_HOLDER)
            oValue.InnerText = v_sAccountHolder.Trim()

            oNewElem.AppendChild(oValue)

            oValue = r_oDocument.CreateElement(ACCARD_NUMBER)
            oValue.InnerText = v_sCardNumber.Trim()

            oNewElem.AppendChild(oValue)

            ' If we have a Start Date
            If v_dtStartDate > iGISSharedConstants.GISLowDate Then
                oValue = r_oDocument.CreateElement(ACSTART_DATE)
                oValue.InnerText = StringsHelper.Format(v_dtStartDate, ACDateMonthYearFormat)

                oNewElem.AppendChild(oValue)
            End If

            oValue = r_oDocument.CreateElement(ACEXPIRY_DATE)
            oValue.InnerText = StringsHelper.Format(v_dtExpiryDate, ACDateMonthYearFormat)

            oNewElem.AppendChild(oValue)

            If v_lIssueNumber > 0 Then
                oValue = r_oDocument.CreateElement(ACISSUE_NUMBER)
                oValue.InnerText = StringsHelper.Format(v_lIssueNumber, "##")

                oNewElem.AppendChild(oValue)
            End If

            ' Add the New Element to its Parent

            r_oParentElem.AppendChild(oNewElem)

            oNewElem = Nothing
            oValue = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPaymentCardElement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPaymentCardElement", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddCustomerElement
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function AddCustomerElement(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_sTitle As String, ByVal v_sCustomerName As String, ByVal v_sAddressLine1 As String, ByVal v_sAddressLine2 As String, ByVal v_sAddressLine3 As String, ByVal v_sAddressLine4 As String, ByVal v_sPostcode As String, ByVal v_sTelephoneNo As String, ByVal v_sEmailAddress As String) As Integer

        Dim result As Integer = 0
        Dim oNewElem, oValue As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the New Element
            oNewElem = r_oDocument.CreateElement(ACCUSTOMER_ELEMENT)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACTITLE)
            oValue.InnerText = v_sTitle.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACCUSTOMER_NAME)
            oValue.InnerText = v_sCustomerName.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACADDRESS_LINE_1)
            oValue.InnerText = v_sAddressLine1.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACADDRESS_LINE_2)
            oValue.InnerText = v_sAddressLine2.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACADDRESS_LINE_3)
            oValue.InnerText = v_sAddressLine3.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACADDRESS_LINE_4)
            oValue.InnerText = v_sAddressLine4.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACPOSTCODE)
            oValue.InnerText = v_sPostcode.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACTELEPHONE_NO)
            oValue.InnerText = v_sTelephoneNo.Trim()

            oNewElem.AppendChild(oValue)

            ' Create the child value elements
            oValue = r_oDocument.CreateElement(ACEMAIL_ADDRESS)
            oValue.InnerText = v_sEmailAddress.Trim()

            oNewElem.AppendChild(oValue)

            ' Add the New Element to its Parent

            r_oParentElem.AppendChild(oNewElem)

            oNewElem = Nothing
            oValue = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCustomerElement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCustomerElement", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddPremiumFinanceRef
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function AddPremiumFinanceRef(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_sPremiumFinanceRef As String) As Integer

        Dim result As Integer = 0
        Dim oNewElem As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Craet the New Element
            oNewElem = r_oDocument.CreateElement(ACPREMIUM_FINANCE_REF)
            oNewElem.InnerText = v_sPremiumFinanceRef.Trim()

            ' Add the New Element to its Parent

            r_oParentElem.AppendChild(oNewElem)

            oNewElem = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPremiumFinanceRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPremiumFinanceRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PremiumFinanceNBResponse
    '
    ' Description:
    '
    ' History: 23/08/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function PremiumFinanceNBResponse(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByRef r_sPremiumFinanceRef As String, ByRef r_lStatusCode As Integer, ByRef r_sTransNumber As String, ByRef r_sLoanRenewalID As String, ByRef r_dtActualPaymentDate As Date) As Integer

        Dim result As Integer = 0
        Dim vValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sPremiumFinanceRef = r_oParentElem.ChildNodes.Item(1).InnerText

            vValue = r_oParentElem.ChildNodes.Item(2).InnerText
            Dim dbNumericTemp As Double
            If Double.TryParse(vValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                r_lStatusCode = CInt(vValue)
            Else
                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Status Code should be NUMERIC. Supplied Value = " & vValue, vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceNBResponse")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_sTransNumber = r_oParentElem.ChildNodes.Item(3).InnerText

            r_sLoanRenewalID = r_oParentElem.ChildNodes.Item(4).InnerText

            vValue = r_oParentElem.ChildNodes.Item(5).InnerText
            If Informations.IsDate(vValue) Then
                r_dtActualPaymentDate = CDate(vValue)
            Else
                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Actual Payment date should be a DATE. Supplied Value = " & vValue, vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceNBResponse")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PremiumFinanceNBResponse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceNBResponse", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnscrambleResponseXML
    '
    ' Description:
    '
    ' History: 08/09/2000 RFC - Created.
    '          07/12/2001 CJB - Changed in accordance with revised MTA messages.
    '          25/09/2002 CJB - Added r_vCancellationAmount for Cancellation
    '                           message and relevant processing if this message
    '                           is received.
    ' ***************************************************************** '
    Public Function UnscrambleResponseXML(ByVal v_sResponseXML As String, ByVal v_sExpectedResponse As String, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef r_sStatusCode As String, Optional ByVal v_sQuoteRef As String = "", Optional ByVal v_sPolicyNum As String = "", Optional ByRef r_sPremiumFinanceRef As String = "", Optional ByRef r_sTransNumber As String = "", Optional ByRef r_sLoanRenewalID As String = "", Optional ByRef r_vTotalPayable As String = "", Optional ByRef r_vRemainingInst As String = "", Optional ByRef r_vFirstInstalAmt As String = "", Optional ByRef r_vSubsequentInstalAmt As String = "", Optional ByRef r_dtActualPaymentDate As Date = #12/30/1899#, Optional ByRef r_vStatusExplanation As String = "", Optional ByRef r_vInterestAmount As String = "", Optional ByRef r_vCancellationAmount As String = "") As Integer

        Dim result As Integer = 0
        Dim oDocument As XmlDocument
        Dim lReturn As Integer
        Dim bLoaded As Boolean
        Dim sResponse As String = ""
        Dim oResponse As XmlElement
        Dim lFound As Integer
        Dim boError As Boolean
        Dim sReason As String = ""
        Dim sErrNo As String = ""
        Dim sErrDesc As String = ""


        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_UnscrambleResponseXML)")

        result = gPMConstants.PMEReturnCode.PMTrue

        oDocument = New XmlDocument()

        ' Note, Note, Note ...................................
        ' Note : The LoadXML method always takes a Unicode BSTR that is encoded in UCS-2 or UTF-16
        ' only. If you pass in anything other than a valid Unicode BSTR to LoadXML, it will fail to load.
        ' Load (from file) will work but I don't want to have to save it to disc just to load it.
        ' Therefore have to remove the UTF-8 encoding that is being returned by Prompt
        ' Also remove the DTD reference
        lFound = (v_sResponseXML.IndexOf("<" & ACGNET_PROMPT_INTERFACE & ">", StringComparison.CurrentCultureIgnoreCase) + 1)
        If lFound = 0 Then
            lFound = (v_sResponseXML.IndexOf("<error>", StringComparison.CurrentCultureIgnoreCase) + 1)
        End If
        If lFound = 0 Then
            result = gPMConstants.PMEReturnCode.PMFalse

            'Send a message with the error details in to the Prompt Error Email address
            lReturn = GeneratePromptErrorEmail(v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=v_sBusinessTypeCode, v_sError:=v_sResponseXML, v_sQuoteRef:=v_sQuoteRef, v_sPolicyNum:=v_sPolicyNum)
            Return result
        Else
            v_sResponseXML = v_sResponseXML.Substring(lFound - 1, Math.Min(v_sResponseXML.Length, v_sResponseXML.Length - lFound))
        End If


        ' Load the Response ready for Parsing

        'developer guide no.22 (no solutions)
        'oDocument.validateOnParse = False
        Dim temp_xml_result As Boolean

        Try
            oDocument.LoadXml(v_sResponseXML)
            temp_xml_result = True

        Catch parseError As System.Exception
            temp_xml_result = False
        End Try
        bLoaded = temp_xml_result
        If Not bLoaded Then

            'Send a message with the error details in to the Prompt Error Email address
            lReturn = GeneratePromptErrorEmail(v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=v_sBusinessTypeCode, v_sError:=v_sResponseXML, v_sQuoteRef:=v_sQuoteRef, v_sPolicyNum:=v_sPolicyNum)

            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Load Response XML = " & v_sResponseXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnscrambleResponseXML")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the Response Tag Name
        sResponse = oDocument.DocumentElement.FirstChild.Name.Trim()

        ' If the Response we got is not the one we were expecting
        If sResponse <> v_sExpectedResponse Then
            ' If Error Response
            If sResponse = ACERROR_RESPONSE Then

                'Need to Process the Error
                'Return general code to enable web pages to show a general Prompt error page
                result = CInt(GISPromptConstants.PromptOtherError)

                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Got the following XML response from Prompt:" & v_sResponseXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnscrambleResponseXML")

                'Send a message with the error details in to the Prompt Error Email address
                lReturn = GeneratePromptErrorEmail(v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=v_sBusinessTypeCode, v_sError:=v_sResponseXML, v_sQuoteRef:=v_sQuoteRef, v_sPolicyNum:=v_sPolicyNum)
                Return result

            Else

                ' Wrong Type of Response
                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Expected a " & v_sExpectedResponse & " but got " & v_sResponseXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnscrambleResponseXML")
                result = gPMConstants.PMEReturnCode.PMFalse

                'Send a message with the error details in to the Prompt Error Email address
                lReturn = GeneratePromptErrorEmail(v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=v_sBusinessTypeCode, v_sError:=v_sResponseXML, v_sQuoteRef:=v_sQuoteRef, v_sPolicyNum:=v_sPolicyNum)
                Return result
            End If

        End If

        ' Get a ref to the Response
        oResponse = oDocument.DocumentElement.FirstChild

        ' What Type of Response have we got

        Select Case sResponse
            ' Bank Account Validation
            Case ACBANK_ACCOUNT_VALIDATION_RESPONSE

                r_sStatusCode = oResponse.FirstChild.InnerText.Trim()

                ' Quote
            Case ACPREMIUM_FINANCE_QUOTE_RESPONSE

                r_sStatusCode = oResponse.ChildNodes.Item(0).InnerText.Trim()
                r_vStatusExplanation = oResponse.ChildNodes.Item(1).InnerText.Trim()

                'CJB110102 MTA messages changed in that no error element was returned but the message content
                'is reduced - quick fix below to prevent errors occurring when this happens...

                'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Resume Next")
                r_vTotalPayable = oResponse.ChildNodes.Item(2).InnerText
                r_vRemainingInst = oResponse.ChildNodes.Item(3).InnerText
                r_vFirstInstalAmt = oResponse.ChildNodes.Item(5).InnerText
                r_vSubsequentInstalAmt = oResponse.ChildNodes.Item(6).InnerText
                r_dtActualPaymentDate = CDate(oResponse.ChildNodes.Item(4).InnerText)
                r_vInterestAmount = oResponse.ChildNodes.Item(7).InnerText

                'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_UnscrambleResponseXML)")

                ' Transact
            Case ACPREMIUM_FINANCE_TRANSACT_RESPONSE

                r_sPremiumFinanceRef = oResponse.ChildNodes.Item(0).InnerText.Trim()
                r_sStatusCode = oResponse.ChildNodes.Item(1).InnerText.Trim()
                r_sTransNumber = oResponse.ChildNodes.Item(2).InnerText.Trim()
                r_sLoanRenewalID = oResponse.ChildNodes.Item(3).InnerText.Trim()
                r_dtActualPaymentDate = CDate(oResponse.ChildNodes.Item(4).InnerText)

                ' MTA Transact
            Case ACPREMIUM_FINANCE_MTA_TRANSACT_RESPONSE

                r_sStatusCode = oResponse.ChildNodes.Item(0).InnerText.Trim()
                r_vStatusExplanation = oResponse.ChildNodes.Item(1).InnerText.Trim()

                'CJB110102 MTA messages changed in that no error element was returned but the message content
                'is reduced - quick fix below to prevent errors occurring when this happens...

                'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Resume Next")
                r_vTotalPayable = oResponse.ChildNodes.Item(2).InnerText
                r_vRemainingInst = oResponse.ChildNodes.Item(3).InnerText
                r_vFirstInstalAmt = oResponse.ChildNodes.Item(5).InnerText
                r_vSubsequentInstalAmt = oResponse.ChildNodes.Item(6).InnerText
                r_dtActualPaymentDate = CDate(oResponse.ChildNodes.Item(4).InnerText)
                r_vInterestAmount = oResponse.ChildNodes.Item(7).InnerText
                r_sPremiumFinanceRef = oResponse.ChildNodes.Item(8).InnerText

                'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_UnscrambleResponseXML)")

                'Set return code so that error will be returned to the web pages (as the status does
                'not get passed back...also send error email...
                If r_sStatusCode <> "00" Then
                    'Need to Process the Error
                    'Return general code to enable web pages to show a general Prompt error page
                    result = CInt(GISPromptConstants.PromptOtherError)

                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Got the following XML response from Prompt:" & v_sResponseXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnscrambleResponseXML")

                    'Send a message with the error details in to the Prompt Error Email address
                    lReturn = GeneratePromptErrorEmail(v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=v_sBusinessTypeCode, v_sError:=v_sResponseXML, v_sQuoteRef:=v_sQuoteRef, v_sPolicyNum:=v_sPolicyNum)
                    Return result
                End If

                ' CJB250902 Cater for Cancellation message response
                ' Cancellation
            Case ACPREMIUM_FINANCE_CANCELLATION_RESPONSE


                'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Resume Next")
                r_sStatusCode = oResponse.ChildNodes.Item(0).InnerText.Trim()
                r_vStatusExplanation = oResponse.ChildNodes.Item(1).InnerText.Trim()
                r_vCancellationAmount = oResponse.ChildNodes.Item(2).InnerText

                'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_UnscrambleResponseXML)")

                'Set return code so that error will be returned to the web pages (as the status does
                'not get passed back...also send error email...
                If r_sStatusCode <> "00" Then
                    'Need to Process the Error
                    'Return general code to enable web pages to show a general Prompt error page
                    result = CInt(GISPromptConstants.PromptOtherError)

                    iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Got the following XML response from Prompt:" & v_sResponseXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnscrambleResponseXML")

                    'Send a message with the error details in to the Prompt Error Email address
                    lReturn = GeneratePromptErrorEmail(v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=v_sBusinessTypeCode, v_sError:=v_sResponseXML, v_sQuoteRef:=v_sQuoteRef, v_sPolicyNum:=v_sPolicyNum)
                    Return result
                End If


        End Select

        'CJB 090101 Check the status code rcd and if NOT one of the following, send err email
        ' 00 - Success
        ' 06 - Invalid Bank A/C
        ' 22 - Sort code incorrect

        'CJB110102 These are just NB handled (but serious) errors - MTA ones are handled above...
        boError = True
        Select Case r_sStatusCode
            Case GISPromptConstants.PromptNoRateHeld
                sReason = "Status 01 - Rate quoted in message is not on table"
            Case GISPromptConstants.PromptFeesDisagree
                sReason = "Status 02 - Fee quoted disagrees with that held on table"
            Case GISPromptConstants.PromptRatesDisagree
                sReason = "Status 03 - Rate quoted disagrees with that held on table"
            Case GISPromptConstants.PromptInstalmentsDisagree
                sReason = "Status 04 - Instalments disagree with number held on table"
            Case GISPromptConstants.PromptAmountsDisagree
                sReason = "Status 05 - Amount of loan disagrees with that calculated"
            Case GISPromptConstants.PromptUnknownBusinessType
                sReason = "Status 13 - Business type is not NB, MTA, MTC or RN"
            Case GISPromptConstants.PromptOracleApplicationError
                sReason = "Status 21 - Oracle application error"
            Case Else
                boError = False

        End Select

        If boError Then
            'Send a message with the error details in to the Prompt Error Email address
            lReturn = GeneratePromptErrorEmail(v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=v_sBusinessTypeCode, v_sError:=sReason & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) & v_sResponseXML, v_sQuoteRef:=v_sQuoteRef, v_sPolicyNum:=v_sPolicyNum)
        End If


        oResponse = Nothing
        oDocument = Nothing

        Return result

Err_UnscrambleResponseXML:

        'Save error details first as after accessing them once, they are cleared
        sErrNo = CStr(Informations.Err().Number)
        sErrDesc = Informations.Err().Description

        'Send a message with the error details in to the Prompt Error Email address
        sReason = "bGISPromptInterface/MainModule/Err_UnscrambleResponseXML error handler : Err.Number=" & sErrNo & " Err.Description=" & sErrDesc & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        lReturn = GeneratePromptErrorEmail(v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=v_sBusinessTypeCode, v_sError:=sReason & v_sResponseXML, v_sQuoteRef:=v_sQuoteRef, v_sPolicyNum:=v_sPolicyNum)
        result = gPMConstants.PMEReturnCode.PMError

        oResponse = Nothing
        oDocument = Nothing

        ' Log Error Message
        iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnscrambleResponseXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnscrambleResponseXML", vErrNo:=sErrNo, vErrDesc:=sErrDesc)

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GeneratePromptErrorEmail
    '
    ' Description: Build and send an Email to the Prompt Errors email address
    '              containing details of the error that has occured. Note that
    '              we need to store the contents in an encrypted textfile
    '              attachment (using Blowfish).
    '
    ' Date: CJB171100
    '
    ' ***************************************************************** '
    Public Function GeneratePromptErrorEmail(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sError As String, Optional ByVal v_sQuoteRef As String = "", Optional ByVal v_sPolicyNum As String = "") As Integer
        Dim result As Integer = 0

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sMsg As String = ""
        Dim sPromptErrorsEmailAddress As String = ""
        Dim sPromptErrorsEmailFrom As String = ""
        Dim oCDONTS As Object

        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="v_sGISDataModelCode:" & v_sGisDataModelCode & ",v_sGISBusinessTypeCode:" & v_sGisBusinessTypeCode & ", v_sError:" & v_sError & ", v_sQuoteRef:" & v_sQuoteRef & ", v_sPolicyNum:" & v_sPolicyNum, vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePromptErrorEmail", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            ' Get the Prompt Errors Email Address from the registry
            lReturn = CType(iGISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=iGISSharedConstants.GISRegPromptErrorsEmailAddress, r_sSettingValue:=sPromptErrorsEmailAddress, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=iGISSharedConstants.GISRegSubKeyEmails), gPMConstants.PMEReturnCode)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to GetRegSettingFromDataBusModel returned:" & lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePromptErrorEmail (In bGISPromptInterface)", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If no value found in registry to send the emails to then just exit without error
            If sPromptErrorsEmailAddress.Trim() = "" Then

                iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="There is no value for the PromptErrorsEmailAddress in the registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePromptErrorEmail (In bGISPromptInterface)", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Prompt Errors Email FROM Address from the registry
            lReturn = CType(iGISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=iGISSharedConstants.GISRegPromptErrorsEmailFrom, r_sSettingValue:=sPromptErrorsEmailFrom, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=iGISSharedConstants.GISRegSubKeyEmails), gPMConstants.PMEReturnCode)

            ' Build the message
            sMsg = "An error has occurred during the Prompt (Premium Finance) integration on " & Informations.FormatDateTime(DateTime.Today) & " at " & Informations.FormatDateTime(DateTimeHelper.Time) & "." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

            If v_sQuoteRef <> "" Then
                sMsg = sMsg & "Quote Reference: " & v_sQuoteRef & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sMsg = sMsg & "Policy Number: " & v_sPolicyNum & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            sMsg = sMsg & "Error Details: " & Strings.ChrW(13) & Strings.ChrW(10) & v_sError & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

            'Also sent what we sent to Prompt in the message
            'sMsg = sMsg & "Message Sent: " & vbCrLf & g_sXMLRequest


            '-----------------------------------------------------------------------------------
            '21/12/2000 CJB
            'The encryption call has been removed due to unknown 'file does not exist' errors
            'in bPMZipper (possibly due to permissions). Therefore, we now send the contents of
            'the file that were to be encrypted in the actual msg body
            '-----------------------------------------------------------------------------------

            'Store the contents on the message in a plain text file first before attaching to mail message

            'Get Temp path
            '    lReturn = PMGetTempPath(sTempPath)
            '
            '    If (lReturn <> PMTrue) Then
            '
            '        LogMessageFile _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Call to PMGetTempPath in GeneratePromptErrorEmail returned:" & lReturn, _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GeneratePromptErrorEmail (In bGISPromptInterface)", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '
            '        GeneratePromptErrorEmail = PMFalse
            '        Exit Function
            '    End If
            '
            '    'Build filename
            '    sFileName = "PromptError_" & Format$(Now, "ddmmyyyy_hhnnss")
            '
            '    'Create temp file
            '    lFileHandle = FreeFile
            '
            '    'Open file, Write to file, close file
            '    Open sTempPath & "\" & sFileName & ".txt" For Output As #lFileHandle
            '        Print #lFileHandle, sMsg
            '    Close #lFileHandle
            '
            '    'Encrypt the output file
            '    sKey = TMPPMKey
            '
            '    'Create reference to the GIS Security component
            '    Set oGISSecurity = CreateObject("cGISSecurity.Application")
            '
            '    If oGISSecurity Is Nothing Then
            '        LogMessageFile _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to create object:cGISSecurity", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GeneratePromptErrorEmail (In bGISPromptInterface)", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '    End If
            '
            '    LogMessageFile _
            ''        iType:=PMLogOnError, _
            ''        sMsg:="About to call oGISSecurity.BlowfishEncryptFile for the Prompt Error email attachment:" & sTempPath & "\" & sFileName & ".txt", _
            ''        vApp:=ACApp, _
            ''        vClass:=ACClass, _
            ''        vMethod:="GeneratePromptErrorEmail (In bGISPromptInterface)", _
            ''        vErrNo:=Err.Number, _
            ''        vErrDesc:=Err.Description
            '
            '    lReturn = oGISSecurity.BlowfishEncryptFile(sTempPath & "\" & sFileName & ".txt", _
            ''                                       sTempPath & "\" & sFileName & ".pmx", _
            ''                                       sFileName & ".txt", _
            ''                                       sKey)
            '
            '    Set oGISSecurity = Nothing
            '
            '    LogMessageFile _
            ''        iType:=PMLogOnError, _
            ''        sMsg:="After calling oGISSecurity.BlowfishEncryptFile for the Prompt Error email attachment:" & sTempPath & "\" & sFileName & ".txt", _
            ''        vApp:=ACApp, _
            ''        vClass:=ACClass, _
            ''        vMethod:="GeneratePromptErrorEmail (In bGISPromptInterface)", _
            ''        vErrNo:=Err.Number, _
            ''        vErrDesc:=Err.Description
            '
            '    'Remove the un-encrypted file
            '    Kill sTempPath & "\" & sFileName & ".txt"
            '
            '    If (lReturn <> PMTrue) Then
            '
            '        LogMessageFile _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Call to oGISSecurity.BlowfishEncryptFile in GeneratePromptErrorEmail returned:" & lReturn, _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GeneratePromptErrorEmail (In bGISPromptInterface)", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '
            '        GeneratePromptErrorEmail = PMFalse
            '        Exit Function
            '    End If


            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Now send the email
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'developer guide no. 205 (latest guide) 

            oCDONTS = New Object()

            ' If we are running on PWS then we cannot send the Mail
            ' as CDONTS and the SMTP service is NOT available.
            If oCDONTS Is Nothing Then

                iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oCDONTS Is Nothing - cannot send Prompt error email.", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePromptErrorEmail (In bGISPromptInterface)", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="About to call oCDONTS.Send method", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePromptErrorEmail (In bGISPromptInterface)", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)


            'Set up the sender, recipient, subject & text message

            With oCDONTS

                .To = sPromptErrorsEmailAddress

                .From = sPromptErrorsEmailFrom

                .Subject = "Prompt Error"
                '.Body = "Please see attachment for details."

                .Body = sMsg

                'Attach our report file
                '.AttachFile sTempPath & "\" & sFileName & ".PMX", sFileName & ".PMX"

                'Send the message

                .send()
            End With

            iGISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="After calling oCDONTS.Send method", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePromptErrorEmail (In bGISPromptInterface)", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            'Remove the encrypted file
            'Kill sTempPath & "\" & sFileName & ".pmx"

            oCDONTS = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GeneratePromptErrorEmail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePromptErrorEmail", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: StartPremiumFinanceMTATransact
    '
    ' Description:
    '
    ' History: 07/12/2001 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function StartPremiumFinanceMTATransact(ByRef r_oDocument As XmlDocument, ByRef r_oRequestElem As XmlElement) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Document
            r_oDocument = New XmlDocument()

            ' Create the top level GNET PROMP ELEMTN
            r_oRequestElem = r_oDocument.CreateElement(ACGNET_PROMPT_INTERFACE)

            ' Set the Message Version Number
            r_oRequestElem.SetAttribute(ACVERSION_NO, ACXMLVersion)

            ' Set it as the Document Element
            If Not (r_oDocument.DocumentElement Is Nothing) Then
                r_oDocument.RemoveChild(r_oDocument.DocumentElement)
            End If
            r_oDocument.AppendChild(r_oRequestElem)

            ' Create the Premium Finance MTA Transact Request Element
            r_oRequestElem = r_oDocument.CreateElement(ACPREMIUM_FINANCE_MTA_TRANSACT_REQUEST)

            ' Append it as a child of the Document Element

            r_oDocument.DocumentElement.AppendChild(r_oRequestElem)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartPremiumFinanceMTATransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartPremiumFinanceMTATransact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: StartPremiumFinanceCancellation
    '
    ' Description: Create the skeleton definition of the Cancellation
    '              message that we're going to send to Prompt.
    '
    ' History: 25/09/2002 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function StartPremiumFinanceCancellation(ByRef r_oDocument As XmlDocument, ByRef r_oRequestElem As XmlElement) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Document
            r_oDocument = New XmlDocument()

            ' Create the top level GNET PROMPT ELEMENT
            r_oRequestElem = r_oDocument.CreateElement(ACGNET_PROMPT_INTERFACE)

            ' Set the Message Version Number
            r_oRequestElem.SetAttribute(ACVERSION_NO, ACXMLVersion)

            ' Set it as the Document Element
            If Not (r_oDocument.DocumentElement Is Nothing) Then
                r_oDocument.RemoveChild(r_oDocument.DocumentElement)
            End If
            r_oDocument.AppendChild(r_oRequestElem)

            ' Create the Premium Finance Cancellation Request Element
            r_oRequestElem = r_oDocument.CreateElement(ACPREMIUM_FINANCE_CANCELLATION_REQUEST)

            ' Append it as a child of the Document Element

            r_oDocument.DocumentElement.AppendChild(r_oRequestElem)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartPremiumFinanceCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartPremiumFinanceCancellation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
End Module