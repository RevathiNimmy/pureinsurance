Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
' developer guide no. 129
Imports SharedFiles
Module GISActionXMLFuncsFinancial
	' ***************************************************************** '
	' Module Name: GISActionXMLFuncs
	'
	' Date:  09/08/1999
	'
	' Description: Functions to handle the Action XML
	'
	' Edit History:
	' RFC13012000 - Add Effective Date and Guaranteed Quote Date params.
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "GISActionXMLFuncsFinancial"
	
	Public Const ACXMLActionFinancial As String = "ACTION_FINANCIAL"
	Public Const ACXMLActionFinancialEndTag As String = "</ACTION_FINANCIAL>"
	
	Public Const ACXMLActionFinancialClassName As String = "FINANCIAL" 'CL240500
	
	' CL190500 (home)
	Public Const ACXMLARetDatacashResponse As String = "DATACASH_RESPONSE"
	Public Const ACXMLARetDatacashResult As String = "RESULT_CODE"
	Public Const ACXMLARetDatacashAuthCode As String = "AUTH_CODE"
	Public Const ACXMLARetDatacashUniqueRef As String = "UNIQUE_REF"
	Public Const ACXMLARetDatacashTimeStamp As String = "TIMESTAMP"
	Public Const ACXMLARetDatacashCardType As String = "CARD_TYPE"
	Public Const ACXMLARetDatacashIssuer As String = "ISSUER"
	Public Const ACXMLARetDatacashCountry As String = "COUNTRY"
	Public Const ACXMLARetDatacashCV2AVSStatus As String = "CV2AVS_STATUS"
	Public Const ACXMLARetDatacashCV2AVSReversalStatus As String = "CV2AVS_REVERSAL_STATUS"
	Public Const ACXMLARetDatacashCV2AVSPolicy As String = "CV2AVS_POLICY"
	
	' CJB111000
	Public Const ACXMLARetBankAccountValidationResponse As String = "BANK_ACCOUNT_VALIDATION_RESPONSE"
	Public Const ACXMLARetBankAccountValidationResult As String = "STATUS_CODE"
	
	'CJB 121200
	Public Const ACXMLARetPremiumFinanceQuoteResponse As String = "PREMIUM_FINANCE_QUOTE_RESPONSE"
	Public Const ACXMLARetPremiumFinanceQuoteResult As String = "STATUS_CODE"
	Public Const ACXMLARetPremiumFinanceQuoteRemainingInst As String = "REMAINING_INST"
	Public Const ACXMLARetPremiumFinanceQuoteFirstInstalAmt As String = "FIRST_INSTAL_AMT"
	Public Const ACXMLARetPremiumFinanceQuoteSubsequentInstalAmt As String = "SUBSEQUENT_INSTAL_AMT"
	Public Const ACXMLARetPremiumFinanceQuoteActualPaymentDate As String = "ACTUAL_PAYMENT_DATE"
	
	
	' CJB181000
	Public Const ACXMLARetCalcPaymentMethodChargeResponse As String = "CALC_PAYMENT_METHOD_CHARGE_RESPONSE"
	Public Const ACXMLARetCalcPaymentMethodChargeInterestRate As String = "INTEREST_RATE"
	Public Const ACXMLARetCalcPaymentMethodChargeAPR As String = "APR"
	Public Const ACXMLARetCalcPaymentMethodChargeInterestCost As String = "INTEREST_COST"
	Public Const ACXMLARetCalcPaymentMethodChargeNoOfInstalments As String = "NO_OF_INSTALMENTS"
	Public Const ACXMLARetCalcPaymentMethodChargeFirstInstalment As String = "FIRST_INSTALMENT"
	Public Const ACXMLARetCalcPaymentMethodChargeOthInstalments As String = "OTH_INSTALMENTS"
	Public Const ACXMLARetCalcPaymentMethodChargeDeposit As String = "DEPOSIT"
	Public Const ACXMLARetCalcPaymentMethodChargeArrangementFee As String = "ARRANGEMENT_FEE"
	Public Const ACXMLARetCalcPaymentMethodChargeDepositPercent As String = "DEPOSIT_PERCENT"
	Public Const ACXMLARetCalcPaymentMethodChargeCompanyName As String = "COMPANY_NAME"
	Public Const ACXMLARetCalcPaymentMethodChargeCompanyNo As String = "COMPANY_NO"
	Public Const ACXMLARetCalcPaymentMethodChargeSchemeName As String = "SCHEME_NAME"
	Public Const ACXMLARetCalcPaymentMethodChargeSchemeNo As String = "SCHEME_NO"
	Public Const ACXMLARetCalcPaymentMethodChargeSchemeVer As String = "SCHEME_VER"
	Public Const ACXMLARetCalcPaymentMethodChargeBasisOfCalc As String = "BASIS_OF_CALC"
	
	' Datacash attributes - CL190500 (home)
	Public Const ACXMLAttribDatacashRequestType As String = "DatacashRequestType"
	Public Const ACXMLAttribDatacashRef As String = "DatacashRef"
	Public Const ACXMLAttribDatacashCardNum As String = "DatacashCardNum"
	Public Const ACXMLAttribDatacashExpMonth As String = "DatacashExpMonth"
	Public Const ACXMLAttribDatacashExpYear As String = "DatacashExpYear"
	Public Const ACXMLAttribDatacashAmt As String = "DatacashAmt"
	Public Const ACXMLAttribDatacashAuthCode As String = "DatacashAuthCode"
	Public Const ACXMLAttribDatacashSwitchExtraInfo As String = "DatacashSwitchExtraInfo"
	Public Const ACXMLAttribPolicyLinkID As String = "PolicyLinkID"
	Public Const ACXMLAttribDatacashTransactionType As String = "DatacashTransactionType"
	Public Const ACXMLAttribDatacashCV2Code As String = "DataCashCV2Code" 'SPW 020604
	Public Const ACXMLAttribDCStreetAdd1 As String = "DataCashAVSAddress1" 'SPW 170604
	Public Const ACXMLAttribDCStreetAdd2 As String = "DataCashAVSAddress2" 'SPW 170604
	Public Const ACXMLAttribDCStreetAdd3 As String = "DataCashAVSAddress3" 'SPW 170604
	Public Const ACXMLAttribDCStreetAdd4 As String = "DataCashAVSAddress4" 'SPW 170604
	Public Const ACXMLAttribDCPostcode As String = "DataCashAVSPostcode" 'SPW 170604
	Public Const ACXMLAttribDCCustomerStreetAdd1 As String = "DataCashCustomerAdd1" 'SPW 190704
	Public Const ACXMLAttribDCCustomerStreetAdd2 As String = "DataCashCustomerAdd2" 'SPW 190704
	Public Const ACXMLAttribDCCustomerStreetAdd3 As String = "DataCashCustomerAdd3" 'SPW 190704
	Public Const ACXMLAttribDCCustomerStreetAdd4 As String = "DataCashCustomerAdd4" 'SPW 190704
	Public Const ACXMLAttribDCCustomerPostcode As String = "DataCashCustomerPostcode" 'SPW 190704
	Public Const ACXMLAttribDCCustomerPhoneNo As String = "DataCashCustomerPhoneNo" 'SPW 190704
	Public Const ACXMLAttribDCCustomerForename As String = "DataCashCustomerForename" 'SPW 190704
	Public Const ACXMLAttribDCCustomerSurname As String = "DataCashCustomerSurname" 'SPW 190704
	Public Const ACXMLAttribDCCustomerEmail As String = "DataCashCustomerEmail" 'SPW 190704
	Public Const ACXMLAttribDCVehicleReg As String = "DataCashCustomerVehicleReg" 'SPW 190704
	
	
	
	' Bank Account Validation attributes - CJB101000
	Public Const ACXMLAttribBankAccountValidationSenderID As String = "BankAccountValidationSenderID"
	Public Const ACXMLAttribBankAccountValidationCoverType As String = "BankAccountValidationCoverType"
	Public Const ACXMLAttribGnetClientCode As String = "GnetClientCode"
	Public Const ACXMLAttribBusinessStatus As String = "BusinessStatus"
	Public Const ACXMLAttribBankAccountValidationBankAccountName As String = "BankAccountValidationBankAccountName"
	Public Const ACXMLAttribBankAccountValidationBankAccountNo As String = "BankAccountValidationBankAccountNo"
	Public Const ACXMLAttribBankAccountValidationBankSortCode As String = "BankAccountValidationBankSortCode"
	Public Const ACXMLAttribBankAccountValidationStatusCode As String = "BankAccountValidationStatusCode"
	
	' Calc Payment Method Charge attributes - CJB181000
	Public Const ACXMLAttribCalcPaymentMethodChargeProductFamily As String = "CalcPaymentMethodChargeProductFamily"
	Public Const ACXMLAttribCalcPaymentMethodChargeTransactionType As String = "CalcPaymentMethodChargeTransactionType"
	Public Const ACXMLAttribCalcPaymentMethodChargePaymentMethod As String = "CalcPaymentMethodChargePaymentMethod"
	Public Const ACXMLAttribCalcPaymentMethodChargeStartDate As String = "CalcPaymentMethodChargeStartDate"
	Public Const ACXMLAttribCalcPaymentMethodChargeAmountToFinance As String = "CalcPaymentMethodChargeAmountToFinance"
	Public Const ACXMLAttribCalcPaymentMethodChargeNoOfInstalments As String = "CalcPaymentMethodChargeNoOfInstalments"
	Public Const ACXMLAttribCalcPaymentMethodChargeActionType As String = "CalcPaymentMethodChargeActionType"
	Public Const ACXMLAttribCalcPaymentMethodChargeRequestedDepositPercent As String = "CalcPaymentMethodChargeRequestedDepositPercent"
	
	' Premium Finance Quote attributes - CJB121200
	Public Const ACXMLAttribPremiumFinanceQuotePremiumDifference As String = "PremiumFinanceQuotePremiumDifference"
	Public Const ACXMLAttribPremiumFinanceQuoteMTAStartDate As String = "MTAStartDate"
	Public Const ACXMLAttribDataset As String = "Dataset"
	
	
	
	' ***************************************************************** '
	' Name: FormatActionXML
	'
	' Description: This builds up the XML message to send to bGIS (from Seller Tool)
	'
	' RFC13012000 - Add Effective Date
	' CJB10102000 - Add Optional Bank Account Validation parameters
	' CJB18102000 - Add Optional Calc Payment Method Charge parameters
	' CJB17112000 - Add Optional Quote Ref parameter for Bank Account Validation
	' CJB12122000 - Add Optional Dataset and PremiumDifference parms for Premium Finance Quote
	' ***************************************************************** '
	Public Function FormatActionXMLFinancial(ByVal v_lAction As Integer, ByVal v_sSellerGUID As String, ByVal v_sDataModelCode As String, ByRef r_sActionXML As String, Optional ByVal v_iDatacashRequestType As String = "", Optional ByVal v_sDatacashRef As String = "", Optional ByVal v_sDatacashCardNum As String = "", Optional ByVal v_iDatacashExpMonth As Integer = 0, Optional ByVal v_iDatacashExpYear As Integer = 0, Optional ByVal v_sDatacashAmt As String = "", Optional ByVal v_sDatacashAuthCode As String = "", Optional ByVal v_lPolicyLinkID As Integer = 0, Optional ByVal v_sTransactionType As String = "", Optional ByVal v_sDatacashSwitchExtraInfo As String = "", Optional ByVal v_sBankAccountValidationSenderID As String = "", Optional ByVal v_sBankAccountValidationCoverType As String = "", Optional ByVal v_sGnetClientCode As String = "", Optional ByVal v_sBusinessStatus As String = "", Optional ByVal v_sBankAccountValidationBankAccountName As String = "", Optional ByVal v_sBankAccountValidationBankAccountNo As String = "", Optional ByVal v_sBankAccountValidationBankSortCode As String = "", Optional ByRef v_sQuoteReference As String = "", Optional ByVal v_sCalcPaymentMethodChargeProductFamily As String = "", Optional ByVal v_sBusinessTypeCode As String = "", Optional ByVal v_sCalcPaymentMethodChargeTransactionType As String = "", Optional ByVal v_sCalcPaymentMethodChargePaymentMethod As String = "", Optional ByVal v_sCalcPaymentMethodChargeStartDate As String = "", Optional ByVal v_sCalcPaymentMethodChargeAmountToFinance As String = "", Optional ByVal v_sCalcPaymentMethodChargeNoOfInstalments As String = "", Optional ByVal v_sCalcPaymentMethodChargeActionType As String = "", Optional ByVal v_sCalcPaymentMethodChargeRequestedDepositPercent As String = "", Optional ByVal v_sDataset As String = "", Optional ByVal v_sMTAStartDate As String = "", Optional ByVal v_sPremiumFinanceQuotePremiumDifference As String = "", Optional ByVal v_sCV2Code As String = "", Optional ByVal v_sAVSStreet_Add1 As String = "", Optional ByVal v_sAVSStreet_Add2 As String = "", Optional ByVal v_sAVSStreet_Add3 As String = "", Optional ByVal v_sAVSStreet_Add4 As String = "", Optional ByVal v_sAVSPostcode As String = "", Optional ByVal v_sCustomerStreet_Add1 As String = "", Optional ByVal v_sCustomerStreet_Add2 As String = "", Optional ByVal v_sCustomerStreet_Add3 As String = "", Optional ByVal v_sCustomerStreet_Add4 As String = "", Optional ByVal v_sCustomerPostcode As String = "", Optional ByVal v_sCustomerPhoneNo As String = "", Optional ByVal v_sCustomerForename As String = "", Optional ByVal v_sCustomerSurname As String = "", Optional ByVal v_sCustomerEmail As String = "", Optional ByVal v_sVehicleReg As String = "") As Integer
		
		Dim result As Integer = 0
		Dim oAction As XmlDocument
		Dim oActionElem, oActionChildElem As XmlElement
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create a New XML Document
			oAction = New XmlDocument()
			
			' Create the Action Element
			oActionElem = oAction.CreateElement(ACXMLAction)
			oActionChildElem = oAction.CreateElement(ACXMLActionFinancial)
			
			' Set the Action Value
			oActionChildElem.InnerText = CStr(v_lAction)
			
			
			' Set the Other Attributes TEMPORARILY until BGIS is changed
			oActionChildElem.SetAttribute(ACXMLAttribDataModelCode, v_sDataModelCode)
			
			' Add Datacash parameters - CL190500 (home)
			oActionChildElem.SetAttribute(ACXMLAttribDatacashRequestType, v_iDatacashRequestType)
			oActionChildElem.SetAttribute(ACXMLAttribDatacashRef, v_sDatacashRef)
			oActionChildElem.SetAttribute(ACXMLAttribDatacashCardNum, v_sDatacashCardNum)
			oActionChildElem.SetAttribute(ACXMLAttribDatacashExpMonth, v_iDatacashExpMonth)
			oActionChildElem.SetAttribute(ACXMLAttribDatacashExpYear, v_iDatacashExpYear)
			oActionChildElem.SetAttribute(ACXMLAttribDatacashAmt, v_sDatacashAmt)
			oActionChildElem.SetAttribute(ACXMLAttribDatacashAuthCode, v_sDatacashAuthCode)
			oActionChildElem.SetAttribute(ACXMLAttribDatacashSwitchExtraInfo, v_sDatacashSwitchExtraInfo)
			oActionChildElem.SetAttribute(ACXMLAttribPolicyLinkID, v_lPolicyLinkID)
			oActionChildElem.SetAttribute(ACXMLAttribDatacashTransactionType, v_sTransactionType)
			oActionChildElem.SetAttribute(ACXMLAttribDatacashCV2Code, v_sCV2Code)
			oActionChildElem.SetAttribute(ACXMLAttribDCStreetAdd1, v_sAVSStreet_Add1) 'SPW 170604
			oActionChildElem.SetAttribute(ACXMLAttribDCStreetAdd2, v_sAVSStreet_Add2) 'SPW 170604
			oActionChildElem.SetAttribute(ACXMLAttribDCStreetAdd3, v_sAVSStreet_Add3) 'SPW 170604
			oActionChildElem.SetAttribute(ACXMLAttribDCStreetAdd4, v_sAVSStreet_Add4) 'SPW 170604
			oActionChildElem.SetAttribute(ACXMLAttribDCPostcode, v_sAVSPostcode) 'SPW 170604
			oActionChildElem.SetAttribute(ACXMLAttribDCCustomerStreetAdd1, v_sCustomerStreet_Add1) 'SPW 190704
			oActionChildElem.SetAttribute(ACXMLAttribDCCustomerStreetAdd2, v_sCustomerStreet_Add2) 'SPW 190704
			oActionChildElem.SetAttribute(ACXMLAttribDCCustomerStreetAdd3, v_sCustomerStreet_Add3) 'SPW 190704
			oActionChildElem.SetAttribute(ACXMLAttribDCCustomerStreetAdd4, v_sCustomerStreet_Add4) 'SPW 190704
			oActionChildElem.SetAttribute(ACXMLAttribDCCustomerPostcode, v_sCustomerPostcode) 'SPW 190704
			oActionChildElem.SetAttribute(ACXMLAttribDCCustomerPhoneNo, v_sCustomerPhoneNo) 'SPW 190704
			oActionChildElem.SetAttribute(ACXMLAttribDCCustomerForename, v_sCustomerForename) 'SPW 190704
			oActionChildElem.SetAttribute(ACXMLAttribDCCustomerSurname, v_sCustomerSurname) 'SPW 190704
			oActionChildElem.SetAttribute(ACXMLAttribDCCustomerEmail, v_sCustomerEmail) 'SPW 190704
			oActionChildElem.SetAttribute(ACXMLAttribDCVehicleReg, v_sVehicleReg) 'SPW 190704
			
			
			' Add Bank Account Validation parameters - CJB101000
			oActionChildElem.SetAttribute(ACXMLAttribBankAccountValidationSenderID, v_sBankAccountValidationSenderID)
			oActionChildElem.SetAttribute(ACXMLAttribBankAccountValidationCoverType, v_sBankAccountValidationCoverType)
			oActionChildElem.SetAttribute(ACXMLAttribGnetClientCode, v_sGnetClientCode)
			oActionChildElem.SetAttribute(ACXMLAttribQuoteReference, v_sQuoteReference)
			oActionChildElem.SetAttribute(ACXMLAttribBusinessStatus, v_sBusinessStatus)
			oActionChildElem.SetAttribute(ACXMLAttribBankAccountValidationBankAccountName, v_sBankAccountValidationBankAccountName)
			oActionChildElem.SetAttribute(ACXMLAttribBankAccountValidationBankAccountNo, v_sBankAccountValidationBankAccountNo)
			oActionChildElem.SetAttribute(ACXMLAttribBankAccountValidationBankSortCode, v_sBankAccountValidationBankSortCode)
			
			' Add Calc Payment Method Charge parameters - CJB181000
			oActionChildElem.SetAttribute(ACXMLAttribCalcPaymentMethodChargeProductFamily, v_sCalcPaymentMethodChargeProductFamily)
			oActionChildElem.SetAttribute(ACXMLAttribBusinessTypeCode, v_sBusinessTypeCode)
			oActionChildElem.SetAttribute(ACXMLAttribCalcPaymentMethodChargeTransactionType, v_sCalcPaymentMethodChargeTransactionType)
			oActionChildElem.SetAttribute(ACXMLAttribCalcPaymentMethodChargePaymentMethod, v_sCalcPaymentMethodChargePaymentMethod)
			oActionChildElem.SetAttribute(ACXMLAttribCalcPaymentMethodChargeStartDate, v_sCalcPaymentMethodChargeStartDate)
			oActionChildElem.SetAttribute(ACXMLAttribCalcPaymentMethodChargeAmountToFinance, v_sCalcPaymentMethodChargeAmountToFinance)
			oActionChildElem.SetAttribute(ACXMLAttribCalcPaymentMethodChargeNoOfInstalments, v_sCalcPaymentMethodChargeNoOfInstalments)
			oActionChildElem.SetAttribute(ACXMLAttribCalcPaymentMethodChargeActionType, v_sCalcPaymentMethodChargeActionType)
			oActionChildElem.SetAttribute(ACXMLAttribCalcPaymentMethodChargeRequestedDepositPercent, v_sCalcPaymentMethodChargeRequestedDepositPercent)
			
			' Add Premium Finance Quote parameters - CJB121200
			oActionChildElem.SetAttribute(ACXMLAttribDataset, v_sDataset)
			oActionChildElem.SetAttribute(ACXMLAttribPremiumFinanceQuoteMTAStartDate, v_sMTAStartDate)
			oActionChildElem.SetAttribute(ACXMLAttribPremiumFinanceQuotePremiumDifference, v_sPremiumFinanceQuotePremiumDifference)
			
			

			oActionElem.AppendChild(oActionChildElem)
			
			If Not (oAction.DocumentElement Is Nothing) Then
				oAction.RemoveChild(oAction.DocumentElement)
			End If
			oAction.AppendChild(oActionElem)
			
			oActionElem = Nothing
			
			r_sActionXML = oAction.InnerXml
			
			oAction = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			
			' Log Error Message
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatActionXMLFinancial Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatActionXMLFinancial", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: UnFormatActionXMLFinancial
	'
	' Description: This deconstructs the XML message received from the Seller Tool.
	'
	' CJB 111000 Add Bank Account Validation parameters
	' CJB 191000 Add Calc Payment Method Charge parameters
	' CJB 121200 Add Premium Finance Quote parameters
	' ***************************************************************** '
	Public Function UnFormatActionXMLFinancial(ByVal v_sActionXML As String, ByRef r_lAction As Integer, ByRef r_sSellerGUID As String, ByRef r_sQuoteReference As String, Optional ByRef r_sDataModelCode As String = "", Optional ByRef r_sDatacashRequestType As String = "", Optional ByRef r_sDatacashRef As String = "", Optional ByRef r_sDatacashCardNum As String = "", Optional ByRef r_iDatacashExpMonth As Integer = 0, Optional ByRef r_iDatacashExpYear As Integer = 0, Optional ByRef r_sDatacashAmt As String = "", Optional ByRef r_sDatacashAuthCode As String = "", Optional ByRef r_sDatacashSwitchExtraInfo As String = "", Optional ByRef r_lPolicyLinkID As Integer = 0, Optional ByRef r_sDatacashTransactionType As String = "", Optional ByRef r_sBankAccountValidationSenderID As String = "", Optional ByRef r_sBankAccountValidationCoverType As String = "", Optional ByRef r_sGnetClientCode As String = "", Optional ByRef r_sBusinessStatus As String = "", Optional ByRef r_sBankAccountValidationBankAccountName As String = "", Optional ByRef r_sBankAccountValidationBankAccountNo As String = "", Optional ByRef r_sBankAccountValidationBankSortCode As String = "", Optional ByRef r_sCalcPaymentMethodChargeProductFamily As String = "", Optional ByRef r_sBusinessTypeCode As String = "", Optional ByRef r_sCalcPaymentMethodChargeTransactionType As String = "", Optional ByRef r_sCalcPaymentMethodChargePaymentMethod As String = "", Optional ByRef r_sCalcPaymentMethodChargeStartDate As String = "", Optional ByRef r_sCalcPaymentMethodChargeAmountToFinance As String = "", Optional ByRef r_sCalcPaymentMethodChargeNoOfInstalments As String = "", Optional ByRef r_sCalcPaymentMethodChargeActionType As String = "", Optional ByRef r_sCalcPaymentMethodChargeRequestedDepositPercent As String = "", Optional ByRef r_sDataset As String = "", Optional ByRef r_sPremiumFinanceQuoteMTAStartDate As String = "", Optional ByRef r_sPremiumFinanceQuotePremiumDifference As String = "", Optional ByRef r_sCV2Code As String = "", Optional ByRef r_sAVSStreet_Add1 As String = "", Optional ByRef r_sAVSStreet_Add2 As String = "", Optional ByRef r_sAVSStreet_Add3 As String = "", Optional ByRef r_sAVSStreet_Add4 As String = "", Optional ByRef r_sAVSPostcode As String = "", Optional ByRef r_sCustomerStreet_Add1 As String = "", Optional ByRef r_sCustomerStreet_Add2 As String = "", Optional ByRef r_sCustomerStreet_Add3 As String = "", Optional ByRef r_sCustomerStreet_Add4 As String = "", Optional ByRef r_sCustomerPostcode As String = "", Optional ByRef r_sCustomerPhoneNo As String = "", Optional ByRef r_sCustomerForename As String = "", Optional ByRef r_sCustomerSurname As String = "", Optional ByRef r_sCustomerEmail As String = "", Optional ByRef r_sVehicleReg As String = "") As Integer
		
		Dim result As Integer = 0
		Dim oActionElem As XmlElement
		Dim oAction As XmlDocument
		Dim bLoaded As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create a New Document
			oAction = New XmlDocument()
			
			' Load the Action XML

            ' developer guide no. no solution 22
            'oAction.validateOnParse = False
			Dim temp_xml_result As Boolean
			Try 
				oAction.LoadXml(v_sActionXML)
				temp_xml_result = True
			
			Catch parseError As System.Exception
				temp_xml_result = False
			End Try
			bLoaded = temp_xml_result
			If Not bLoaded Then
				bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & v_sActionXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionXMLFinancial")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oActionElem = oAction.DocumentElement
			oActionElem = oAction.DocumentElement.FirstChild ' CL240500
			
			' Get the Action Value
			r_lAction = CInt(oActionElem.InnerText)

			r_sDataModelCode = CStr(oActionElem.GetAttribute(ACXMLAttribDataModelCode))
			
			' Datacash parameters - CL190500 (home)

			r_sDatacashRequestType = CStr(oActionElem.GetAttribute(ACXMLAttribDatacashRequestType))

			r_sDatacashRef = CStr(oActionElem.GetAttribute(ACXMLAttribDatacashRef))

			r_sDatacashCardNum = CStr(oActionElem.GetAttribute(ACXMLAttribDatacashCardNum))

			r_iDatacashExpMonth = CInt(oActionElem.GetAttribute(ACXMLAttribDatacashExpMonth))

			r_iDatacashExpYear = CInt(oActionElem.GetAttribute(ACXMLAttribDatacashExpYear))

			r_sDatacashAmt = CStr(oActionElem.GetAttribute(ACXMLAttribDatacashAmt))

			r_sDatacashAuthCode = CStr(oActionElem.GetAttribute(ACXMLAttribDatacashAuthCode))

			r_sDatacashSwitchExtraInfo = CStr(oActionElem.GetAttribute(ACXMLAttribDatacashSwitchExtraInfo))

			r_lPolicyLinkID = CInt(oActionElem.GetAttribute(ACXMLAttribPolicyLinkID))

			r_sDatacashTransactionType = CStr(oActionElem.GetAttribute(ACXMLAttribDatacashTransactionType))

			r_sCV2Code = CStr(oActionElem.GetAttribute(ACXMLAttribDatacashCV2Code))

			r_sAVSStreet_Add1 = CStr(oActionElem.GetAttribute(ACXMLAttribDCStreetAdd1)) 'SPW 170604

			r_sAVSStreet_Add2 = CStr(oActionElem.GetAttribute(ACXMLAttribDCStreetAdd2)) 'SPW 170604

			r_sAVSStreet_Add3 = CStr(oActionElem.GetAttribute(ACXMLAttribDCStreetAdd3)) 'SPW 170604

			r_sAVSStreet_Add4 = CStr(oActionElem.GetAttribute(ACXMLAttribDCStreetAdd4)) 'SPW 170604

			r_sAVSPostcode = CStr(oActionElem.GetAttribute(ACXMLAttribDCPostcode)) 'SPW 170604

			r_sCustomerStreet_Add1 = CStr(oActionElem.GetAttribute(ACXMLAttribDCCustomerStreetAdd1)) 'SPW 190704

			r_sCustomerStreet_Add2 = CStr(oActionElem.GetAttribute(ACXMLAttribDCCustomerStreetAdd2)) 'SPW 190704

			r_sCustomerStreet_Add3 = CStr(oActionElem.GetAttribute(ACXMLAttribDCCustomerStreetAdd3)) 'SPW 190704

			r_sCustomerStreet_Add4 = CStr(oActionElem.GetAttribute(ACXMLAttribDCCustomerStreetAdd4)) 'SPW 190704

			r_sCustomerPostcode = CStr(oActionElem.GetAttribute(ACXMLAttribDCCustomerPostcode)) 'SPW 190704

			r_sCustomerPhoneNo = CStr(oActionElem.GetAttribute(ACXMLAttribDCCustomerPhoneNo)) 'SPW 190704

			r_sCustomerForename = CStr(oActionElem.GetAttribute(ACXMLAttribDCCustomerForename)) 'SPW 190704

			r_sCustomerSurname = CStr(oActionElem.GetAttribute(ACXMLAttribDCCustomerSurname)) 'SPW 190704

			r_sCustomerEmail = CStr(oActionElem.GetAttribute(ACXMLAttribDCCustomerEmail)) 'SPW 190704

			r_sVehicleReg = CStr(oActionElem.GetAttribute(ACXMLAttribDCVehicleReg)) 'SPW 190704
			
			
			' Add Bank Account Validation parameters - CJB101000

			r_sQuoteReference = CStr(oActionElem.GetAttribute(ACXMLAttribQuoteReference))

			r_sBankAccountValidationSenderID = CStr(oActionElem.GetAttribute(ACXMLAttribBankAccountValidationSenderID))

			r_sBankAccountValidationCoverType = CStr(oActionElem.GetAttribute(ACXMLAttribBankAccountValidationCoverType))

			r_sGnetClientCode = CStr(oActionElem.GetAttribute(ACXMLAttribGnetClientCode))

			r_sBusinessStatus = CStr(oActionElem.GetAttribute(ACXMLAttribBusinessStatus))

			r_sBankAccountValidationBankAccountName = CStr(oActionElem.GetAttribute(ACXMLAttribBankAccountValidationBankAccountName))

			r_sBankAccountValidationBankAccountNo = CStr(oActionElem.GetAttribute(ACXMLAttribBankAccountValidationBankAccountNo))

			r_sBankAccountValidationBankSortCode = CStr(oActionElem.GetAttribute(ACXMLAttribBankAccountValidationBankSortCode))
			
			' Add Calc Payment Method Charge parameters - CJB191000

			r_sCalcPaymentMethodChargeProductFamily = CStr(oActionElem.GetAttribute(ACXMLAttribCalcPaymentMethodChargeProductFamily))

			r_sBusinessTypeCode = CStr(oActionElem.GetAttribute(ACXMLAttribBusinessTypeCode))

			r_sCalcPaymentMethodChargeTransactionType = CStr(oActionElem.GetAttribute(ACXMLAttribCalcPaymentMethodChargeTransactionType))

			r_sCalcPaymentMethodChargePaymentMethod = CStr(oActionElem.GetAttribute(ACXMLAttribCalcPaymentMethodChargePaymentMethod))

			r_sCalcPaymentMethodChargeStartDate = CStr(oActionElem.GetAttribute(ACXMLAttribCalcPaymentMethodChargeStartDate))

			r_sCalcPaymentMethodChargeAmountToFinance = CStr(oActionElem.GetAttribute(ACXMLAttribCalcPaymentMethodChargeAmountToFinance))

			r_sCalcPaymentMethodChargeNoOfInstalments = CStr(oActionElem.GetAttribute(ACXMLAttribCalcPaymentMethodChargeNoOfInstalments))

			r_sCalcPaymentMethodChargeActionType = CStr(oActionElem.GetAttribute(ACXMLAttribCalcPaymentMethodChargeActionType))

			r_sCalcPaymentMethodChargeRequestedDepositPercent = CStr(oActionElem.GetAttribute(ACXMLAttribCalcPaymentMethodChargeRequestedDepositPercent))
			
			' Add Premium Finance Quote parameters - CJB 121200

			r_sDataset = CStr(oActionElem.GetAttribute(ACXMLAttribDataset))

			r_sPremiumFinanceQuoteMTAStartDate = CStr(oActionElem.GetAttribute(ACXMLAttribPremiumFinanceQuoteMTAStartDate))

			r_sPremiumFinanceQuotePremiumDifference = CStr(oActionElem.GetAttribute(ACXMLAttribPremiumFinanceQuotePremiumDifference))
			
			oActionElem = Nothing
			oAction = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			
			
			' Log Error Message
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatActionXMLFinancial Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionXMLFinancial", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: FormatActionReturnXMLFinancial
	'
	' Description: This builds up the XML Return Message to send from bGIS to
	'              the Seller Tool.
	'
	' CJB 111000 Add Bank Account Validation Return Value
	' CJB 191000 Add Calc Payment Method Charge Return Values
	' CJB 121200 Add premium Finance Quote Return Values
	' ***************************************************************** '
	Public Function FormatActionReturnXMLFinancial(ByVal v_lReturnValue As Integer, ByRef r_sActionReturnXML As String, Optional ByVal v_vDatacashResponseArray() As Object = Nothing, Optional ByVal v_sBankAccountValidationStatusCode As String = "", Optional ByVal v_sCalcPaymentMethodChargeInterestRate As String = "", Optional ByVal v_sCalcPaymentMethodChargeAPR As String = "", Optional ByVal v_sCalcPaymentMethodChargeInterestCost As String = "", Optional ByVal v_sCalcPaymentMethodChargeNoOfInstalments As String = "", Optional ByVal v_sCalcPaymentMethodChargeFirstInstalment As String = "", Optional ByVal v_sCalcPaymentMethodChargeOthInstalments As String = "", Optional ByVal v_sCalcPaymentMethodChargeDeposit As String = "", Optional ByVal v_sCalcPaymentMethodChargeArrangementFee As String = "", Optional ByVal v_sCalcPaymentMethodChargeDepositPercent As String = "", Optional ByVal v_sCalcPaymentMethodChargeCompanyName As String = "", Optional ByVal v_sCalcPaymentMethodChargeCompanyNo As String = "", Optional ByVal v_sCalcPaymentMethodChargeSchemeName As String = "", Optional ByVal v_sCalcPaymentMethodChargeSchemeNo As String = "", Optional ByVal v_sCalcPaymentMethodChargeSchemeVer As String = "", Optional ByVal v_sCalcPaymentMethodChargeBasisOfCalc As String = "", Optional ByVal v_sPremiumFinanceQuoteStatusCode As String = "", Optional ByVal v_sPremiumFinanceQuoteRemainingInst As String = "", Optional ByVal v_sPremiumFinanceQuoteFirstInstalAmt As String = "", Optional ByVal v_sPremiumFinanceQuoteSubsequentInstalAmt As String = "", Optional ByVal v_sPremiumFinanceQuoteActualPaymentDate As String = "") As Integer
		
		Dim result As Integer = 0
		Dim oActionReturn As XmlDocument
		Dim oActionReturnElem, oAddressElem, oAddLineElem As XmlElement
		
		Dim oElem, oElemChild As XmlElement
		
		Dim lRow As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create a New XML Document
			oActionReturn = New XmlDocument()
			
			' Create the Action Element
			oActionReturnElem = oActionReturn.CreateElement(ACXMLActionReturn)
			
			oActionReturnElem.InnerText = "Action Return"
			
			' Set the Attributes
			oActionReturnElem.SetAttribute(ACXMLAttribReturnValue, v_lReturnValue)
			
			
			' Do we have a Datacash Response Arrray? - CL190500 (home)
			If Information.IsArray(v_vDatacashResponseArray) Then
				
				oElem = oActionReturn.CreateElement(ACXMLARetDatacashResponse)

				oActionReturnElem.AppendChild(oElem)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetDatacashResult)

				oElemChild.InnerText = CStr(v_vDatacashResponseArray(iGISSharedConstants.GISDatacashResult))

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetDatacashAuthCode)

				oElemChild.InnerText = CStr(v_vDatacashResponseArray(iGISSharedConstants.GISDatacashAuthCode))

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetDatacashUniqueRef)

				oElemChild.InnerText = CStr(v_vDatacashResponseArray(iGISSharedConstants.GISDatacashUniqueRef))

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetDatacashTimeStamp)

				oElemChild.InnerText = CStr(v_vDatacashResponseArray(iGISSharedConstants.GISDatacashTimeStamp))

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetDatacashCardType)

				oElemChild.InnerText = CStr(v_vDatacashResponseArray(iGISSharedConstants.GISDatacashCardType))

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetDatacashIssuer)

				oElemChild.InnerText = CStr(v_vDatacashResponseArray(iGISSharedConstants.GISDatacashIssuer))

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetDatacashCountry)

				oElemChild.InnerText = CStr(v_vDatacashResponseArray(iGISSharedConstants.GISDatacashCountry))

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetDatacashCV2AVSStatus)

				oElemChild.InnerText = CStr(v_vDatacashResponseArray(iGISSharedConstants.GISDataCashCV2AVSStatus))

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetDatacashCV2AVSReversalStatus)

				oElemChild.InnerText = CStr(v_vDatacashResponseArray(iGISSharedConstants.GISDataCashCV2AVSReversalStatus))

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetDatacashCV2AVSPolicy)

				oElemChild.InnerText = CStr(v_vDatacashResponseArray(iGISSharedConstants.GISDataCashCV2AVSPolicy))

				oElem.AppendChild(oElemChild)
				
				oElem = Nothing
				oElemChild = Nothing
				
			End If
			
			'Populate Bank Account Validation return value (if appropriate) CJB 111000
			If Not False Then
				
				oElem = oActionReturn.CreateElement(ACXMLARetBankAccountValidationResponse)

				oActionReturnElem.AppendChild(oElem)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetBankAccountValidationResult)
				oElemChild.InnerText = v_sBankAccountValidationStatusCode

				oElem.AppendChild(oElemChild)
				
			End If
			
			
			'Populate Calc Payment Method Charge return values (if appropriate) CJB 191000
			'Note that if the Interest Rate param is present we will assume all are for this message
			If Not False Then
				
				'Create parent return element
				oElem = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeResponse)

				oActionReturnElem.AppendChild(oElem)
				
				'Now create all children return elements
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeInterestRate)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeInterestRate

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeAPR)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeAPR

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeInterestCost)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeInterestCost

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeNoOfInstalments)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeNoOfInstalments

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeFirstInstalment)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeFirstInstalment

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeOthInstalments)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeOthInstalments

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeDeposit)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeDeposit

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeArrangementFee)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeArrangementFee

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeDepositPercent)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeDepositPercent

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeCompanyName)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeCompanyName

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeCompanyNo)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeCompanyNo

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeSchemeName)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeSchemeName

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeSchemeNo)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeSchemeNo

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeSchemeVer)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeSchemeVer

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetCalcPaymentMethodChargeBasisOfCalc)
				oElemChild.InnerText = v_sCalcPaymentMethodChargeBasisOfCalc

				oElem.AppendChild(oElemChild)
				
			End If
			
			'Populate Premium Finance Quote return values (if appropriate) CJB 121200
			'Note that if the Status Code param is present we will assume all are for this message
			If Not False Then
				
				'Create parent return element
				oElem = oActionReturn.CreateElement(ACXMLARetPremiumFinanceQuoteResponse)

				oActionReturnElem.AppendChild(oElem)
				
				'Now create all children return elements
				oElemChild = oActionReturn.CreateElement(ACXMLARetPremiumFinanceQuoteResult)
				oElemChild.InnerText = v_sPremiumFinanceQuoteStatusCode

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetPremiumFinanceQuoteRemainingInst)
				oElemChild.InnerText = v_sPremiumFinanceQuoteRemainingInst

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetPremiumFinanceQuoteFirstInstalAmt)
				oElemChild.InnerText = v_sPremiumFinanceQuoteFirstInstalAmt

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetPremiumFinanceQuoteSubsequentInstalAmt)
				oElemChild.InnerText = v_sPremiumFinanceQuoteSubsequentInstalAmt

				oElem.AppendChild(oElemChild)
				
				oElemChild = oActionReturn.CreateElement(ACXMLARetPremiumFinanceQuoteActualPaymentDate)
				oElemChild.InnerText = v_sPremiumFinanceQuoteActualPaymentDate

				oElem.AppendChild(oElemChild)
				
			End If
			
			If Not (oActionReturn.DocumentElement Is Nothing) Then
				oActionReturn.RemoveChild(oActionReturn.DocumentElement)
			End If
			oActionReturn.AppendChild(oActionReturnElem)
			
			oActionReturnElem = Nothing
			
			r_sActionReturnXML = oActionReturn.InnerXml
			
			oActionReturn = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			
			' Log Error Message
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatActionReturnXMLFinancial Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatActionReturnXMLFinancial", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: UnFormatActionReturnXMLFinancial
	'
	' Description: This deconstructs the XML response message received from bGIS.
	'
	' CJB 111000 Add Bank Account Validation Return Value
	' CJB 191000 Add Calc Payment Method Charge Return Values
	' CJB 121200 Add Premium Finance Quote Return Values
	' ***************************************************************** '
	Public Function UnFormatActionReturnXMLFinancial(ByVal v_sActionReturnXML As String, ByRef r_lReturnValue As Integer, Optional ByRef r_vDatacashResponseArray() As Object = Nothing, Optional ByRef r_vBankAccountValidationStatusCode As String = "", Optional ByRef r_vCalcPaymentMethodChargeInterestRate As String = "", Optional ByRef r_vCalcPaymentMethodChargeAPR As String = "", Optional ByRef r_vCalcPaymentMethodChargeInterestCost As String = "", Optional ByRef r_vCalcPaymentMethodChargeNoOfInstalments As String = "", Optional ByRef r_vCalcPaymentMethodChargeFirstInstalment As String = "", Optional ByRef r_vCalcPaymentMethodChargeOthInstalments As String = "", Optional ByRef r_vCalcPaymentMethodChargeDeposit As String = "", Optional ByRef r_vCalcPaymentMethodChargeArrangementFee As String = "", Optional ByRef r_vCalcPaymentMethodChargeDepositPercent As String = "", Optional ByRef r_vCalcPaymentMethodChargeCompanyName As String = "", Optional ByRef r_vCalcPaymentMethodChargeCompanyNo As String = "", Optional ByRef r_vCalcPaymentMethodChargeSchemeName As String = "", Optional ByRef r_vCalcPaymentMethodChargeSchemeNo As String = "", Optional ByRef r_vCalcPaymentMethodChargeSchemeVer As String = "", Optional ByRef r_vCalcPaymentMethodChargeBasisOfCalc As String = "", Optional ByRef r_vPremiumFinanceQuoteStatusCode As String = "", Optional ByRef r_vPremiumFinanceQuoteRemainingInst As String = "", Optional ByRef r_vPremiumFinanceQuoteFirstInstalAmt As String = "", Optional ByRef r_vPremiumFinanceQuoteSubsequentInstalAmt As String = "", Optional ByRef r_vPremiumFinanceQuoteActualPaymentDate As String = "", Optional ByRef r_vCV2Code As Object = Nothing) As Integer
		
		
		Dim result As Integer = 0
		Dim oActionReturnElem As XmlElement
		Dim oActionReturn As XmlDocument
		Dim bLoaded As Boolean
		Dim oMatches As XmlNodeList
		Dim oMatch As XmlElement
		Dim oElem As XmlNodeList
		Dim lNumMatches, lRow As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create a New Document
			oActionReturn = New XmlDocument()
			
			' Load the Action XML

            ' developer guide no. no solution 22
            'oActionReturn.validateOnParse = False
			Dim temp_xml_result As Boolean
			Try 
				oActionReturn.LoadXml(v_sActionReturnXML)
				temp_xml_result = True
			
			Catch parseError As System.Exception
				temp_xml_result = False
			End Try
			bLoaded = temp_xml_result
			If Not bLoaded Then
				bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & v_sActionReturnXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionReturnXMLFinancial")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oActionReturnElem = oActionReturn.DocumentElement
			
			' Get the Attributes

			r_lReturnValue = CInt(oActionReturnElem.GetAttribute(ACXMLAttribReturnValue))
			
			
			' Is there a Datacash Response Array? - CL190500 (home)
			oMatches = oActionReturn.GetElementsByTagName(ACXMLARetDatacashResponse)
			If Not (oMatches Is Nothing) Then
				
				' How Many Matches are there
				lNumMatches = oMatches.Count
				
				' If there are matches then
				If lNumMatches = 1 Then
					
					' Resize the Array
					ReDim r_vDatacashResponseArray(iGISSharedConstants.GISDataCashCV2AVSPolicy)
					
					' Add them to the Array
					
					' Get a Reference to a Match
					oMatch = oMatches.Item(0)
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetDatacashResult)

					r_vDatacashResponseArray(iGISSharedConstants.GISDatacashResult) = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetDatacashAuthCode)

					r_vDatacashResponseArray(iGISSharedConstants.GISDatacashAuthCode) = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetDatacashUniqueRef)

					r_vDatacashResponseArray(iGISSharedConstants.GISDatacashUniqueRef) = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetDatacashTimeStamp)

					r_vDatacashResponseArray(iGISSharedConstants.GISDatacashTimeStamp) = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetDatacashCardType)

					r_vDatacashResponseArray(iGISSharedConstants.GISDatacashCardType) = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetDatacashIssuer)

					r_vDatacashResponseArray(iGISSharedConstants.GISDatacashIssuer) = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetDatacashCountry)

					r_vDatacashResponseArray(iGISSharedConstants.GISDatacashCountry) = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetDatacashCV2AVSStatus)

					r_vDatacashResponseArray(iGISSharedConstants.GISDataCashCV2AVSStatus) = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetDatacashCV2AVSReversalStatus)

					r_vDatacashResponseArray(iGISSharedConstants.GISDataCashCV2AVSReversalStatus) = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetDatacashCV2AVSPolicy)

					r_vDatacashResponseArray(iGISSharedConstants.GISDataCashCV2AVSPolicy) = oElem.Item(0).InnerText
					
					
				End If
				
			End If
			
			'CJB 111000 Process a Bank Account Validation Status Code return value
			'Is there a Bank Account Validation element?
			oMatches = oActionReturn.GetElementsByTagName(ACXMLARetBankAccountValidationResponse)
			If Not (oMatches Is Nothing) Then
				
				' How Many Matches are there
				lNumMatches = oMatches.Count
				
				' If there are matches then
				If lNumMatches = 1 Then
					
					' Get a Reference to a Match
					oMatch = oMatches.Item(0)
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetBankAccountValidationResult)
					r_vBankAccountValidationStatusCode = oElem.Item(0).InnerText
					
				End If
			End If
			
			
			'CJB 181000 Process the Calc Payment Method Charge Status o/p values
			'Is there a Calc Payment Method Charge element?
			oMatches = oActionReturn.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeResponse)
			If Not (oMatches Is Nothing) Then
				
				' How Many Matches are there
				lNumMatches = oMatches.Count
				
				' If there are matches then
				If lNumMatches = 1 Then
					
					' Get a Reference to a Match
					oMatch = oMatches.Item(0)
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeInterestRate)
					r_vCalcPaymentMethodChargeInterestRate = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeAPR)
					r_vCalcPaymentMethodChargeAPR = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeInterestCost)
					r_vCalcPaymentMethodChargeInterestCost = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeNoOfInstalments)
					r_vCalcPaymentMethodChargeNoOfInstalments = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeFirstInstalment)
					r_vCalcPaymentMethodChargeFirstInstalment = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeOthInstalments)
					r_vCalcPaymentMethodChargeOthInstalments = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeDeposit)
					r_vCalcPaymentMethodChargeDeposit = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeArrangementFee)
					r_vCalcPaymentMethodChargeArrangementFee = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeDepositPercent)
					r_vCalcPaymentMethodChargeDepositPercent = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeCompanyName)
					r_vCalcPaymentMethodChargeCompanyName = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeCompanyNo)
					r_vCalcPaymentMethodChargeCompanyNo = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeSchemeName)
					r_vCalcPaymentMethodChargeSchemeName = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeSchemeNo)
					r_vCalcPaymentMethodChargeSchemeNo = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeSchemeVer)
					r_vCalcPaymentMethodChargeSchemeVer = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetCalcPaymentMethodChargeBasisOfCalc)
					r_vCalcPaymentMethodChargeBasisOfCalc = oElem.Item(0).InnerText
					
				End If
			End If
			
			'CJB 121200 Process a premium Finance Quote Status Code return value
			'Is there a Premium Finance Quote element?
			oMatches = oActionReturn.GetElementsByTagName(ACXMLARetPremiumFinanceQuoteResponse)
			If Not (oMatches Is Nothing) Then
				
				' How Many Matches are there
				lNumMatches = oMatches.Count
				
				' If there are matches then
				If lNumMatches = 1 Then
					
					' Get a Reference to a Match
					oMatch = oMatches.Item(0)
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetPremiumFinanceQuoteResult)
					r_vPremiumFinanceQuoteStatusCode = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetPremiumFinanceQuoteRemainingInst)
					r_vPremiumFinanceQuoteRemainingInst = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetPremiumFinanceQuoteFirstInstalAmt)
					r_vPremiumFinanceQuoteFirstInstalAmt = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetPremiumFinanceQuoteSubsequentInstalAmt)
					r_vPremiumFinanceQuoteSubsequentInstalAmt = oElem.Item(0).InnerText
					
					oElem = oMatch.GetElementsByTagName(ACXMLARetPremiumFinanceQuoteActualPaymentDate)
					r_vPremiumFinanceQuoteActualPaymentDate = oElem.Item(0).InnerText
					
				End If
			End If
			
			oActionReturnElem = Nothing
			oActionReturn = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			
			' Log Error Message
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatActionReturnXMLFinancial Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionReturnXMLFinancial", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module
