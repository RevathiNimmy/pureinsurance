Option Strict Off
Option Explicit On
Imports System.Xml
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("ActionViaHTTP_NET.ActionViaHTTP")>
Public NotInheritable Class ActionViaHTTP
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private Const ACClass As String = "ActionViaHTTP"

    Private m_oDatabase As dPMDAO.Database
    Private m_oDataSet As cGISDataSetControl.Application
    Private m_bCloseDatabase As Boolean
    Private m_lReturn As Integer

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public Function ProcessAction(ByRef r_sDataset As String) As Integer
        Dim result As Integer = 0
        Dim lAction As Integer
        Dim sSellerGUID As String = ""
        Dim lQuoteType As Integer
        Dim sBusinessTypeCode As String
        Dim sQuoteReference, sQuoteRefPassword As Object
        Dim lPolicyLinkID As Object
        Dim sDataModelCode As Object = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bLoaded As Boolean
        Dim sSearchNameNum, sSearchPostcode As String
        Dim sTopOIKey As Object
        Dim vMatchArray As Object
        Dim sInsurerCode As String = ""
        Dim lSchemeID As Integer
        Dim dtEffectiveDate As Date
        Dim sDatacashRequestType, sDatacashRef, sDatacashCardNum As String
        Dim iDatacashExpMonth, iDatacashExpYear As Integer
        Dim sDatacashAuthCode, sDatacashAmt, sDatacashSwitchExtraInfo, sDatacashTransactionType As String
        Dim sBankAccountValidationSenderID, sBankAccountValidationCoverType, sGnetClientCode, sBusinessStatus, sBankAccountValidationBankAccountName, sBankAccountValidationBankAccountNo, sBankAccountValidationBankSortCode, sBankAccountValidationStatusCode As Object
        Dim sCalcPaymentMethodChargeProductFamily, sCalcPaymentMethodChargeTransactionType, sCalcPaymentMethodChargePaymentMethod, sCalcPaymentMethodChargeStartDate, sCalcPaymentMethodChargeAmountToFinance, sCalcPaymentMethodChargeRequestedNoOfInstalments, sCalcPaymentMethodChargeActionType, sCalcPaymentMethodChargeInterestRate, sCalcPaymentMethodChargeRequestedDepositPercent, sCalcPaymentMethodChargeAPR, sCalcPaymentMethodChargeInterestCost, sCalcPaymentMethodChargeNoOfInstalments, sCalcPaymentMethodChargeFirstInstalment, sCalcPaymentMethodChargeOthInstalments, sCalcPaymentMethodChargeDeposit, sCalcPaymentMethodChargeArrangementFee, sCalcPaymentMethodChargeDepositPercent, sCalcPaymentMethodChargeCompanyName, sCalcPaymentMethodChargeCompanyNo, sCalcPaymentMethodChargeSchemeName, sCalcPaymentMethodChargeSchemeNo, sCalcPaymentMethodChargeSchemeVer, sCalcPaymentMethodChargeBasisOfCalc, sPremiumFinanceQuotePremiumDifference, sDatasetContents, sMTAStartDate, sPremiumFinanceQuoteStatusCode, sPremiumFinanceQuoteRemainingInst, sPremiumFinanceQuoteFirstInstalAmt, sPremiumFinanceQuoteSubsequentInstalAmt, sPremiumFinanceQuoteActualPaymentDate As Object
        Dim lPartyCnt As Object
        Dim sUserID, sPassword, sForename, sSurname, sMothersMaidenName, sDateOfBirth As Object
        Dim dtDateOfBirth As Object
        Dim sEmailAddress, sMemorableDate, sAQuestion, sTheAnswer, sCurrentRenewalDate As Object
        Dim sTitle, sMaritalStatusCode, sAddress1, sAddress2, sAddress3, sAddress4, sPostcode As Object
        Dim lInsFileSearchType, lInsuranceFileCnt As Object
        Dim sPolicyTypeCode As String = ""
        Dim vQuotePolicyArray, vPolicyVersionArray As Object
        Dim lRiskID As Integer
        Dim dtCoverStartDate, dtExpiryDate As Date
        Dim lPolicyVersion As Integer
        Dim lNewPolicyLinkID, lNewInsuranceFileCnt As Object
        Dim dtGuaranteedQuoteDate As Object
        Dim sInsuranceFileRef As String = ""
        Dim lPMUserID As Object
        Dim lDatasetStart, lEndOfAction, lDatasetEnd As Integer
        Dim sAction, sDataset, sXMLOldRisk, sEmptyDataSet, sDataSetDef As Object
        Dim lActionReturn As Integer
        Dim sActionReturnXML As String = ""
        Dim vDatacashResponseArray As Object
        Dim oAction As XmlDocument
        Dim oActionElem As XmlElement
        Dim oBusiness As Object
        Dim sClassName, sActionType As String
        Dim iMTAType As Integer
        Dim vAdditionalData As Object
        Dim sRegistrationNumber As String = ""
        Dim lEMailType As Integer
        Dim sEMailFrom, sEMailTo, sEMailCC, sEMailSubject, sEMailText As String
        Dim sPartyTypeCode, sShortName, sResolvedName As Object
        Dim lLeadAgentCnt As Integer
        Dim sGenderCode, sInitials, sTelephoneNumber As Object
        Dim vQuoteArray, vPartyArray As Object
        Dim sTPUserCode, sTPIntroducer As Object
        Dim vFindPolicyArray As Object
        Dim sInsFolderDescription As String = ""
        Dim dCoverStartDate As Object
        Dim vProductArray As Object
        Dim vListDataMerged As Object
        Dim lFormNumber As Integer
        Dim lBusinessTypeID, lDataModelID, lInsuranceFolderCnt, lProductID As Integer
        Dim dtRenewalDate As Date
        Dim lRiskCodeID, lRenewalEDIAuditID, lRenewalSchemeID As Integer
        Dim lRenewalInsuranceFileCnt As Object
        Dim sRenewalStatusCode As String = ""
        Dim lOldInsuranceFileCnt As Integer
        Dim dtDueDateStart, dtDueDateLimit As Date
        Dim sClientCode, sPolicyNo As String
        Dim lInsurerID, lSuspensionLevel As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDataSet = New cGISDataSetControl.Application()

            r_sDataset = StringsHelper.StrConv(r_sDataset, StringsHelper.VbStrConvEnum.vbUnicode)

            lEndOfAction = (r_sDataset.IndexOf(ACXMLActionEndTag, StringComparison.CurrentCultureIgnoreCase) + 1)
            If lEndOfAction < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sAction = r_sDataset.Substring(0, lEndOfAction + ACXMLActionEndTag.Length - 1)

            oAction = New XmlDocument()

            Dim temp_xml_result As Boolean
            Try
                oAction.LoadXml(sAction)
                temp_xml_result = True

            Catch parseError As System.Exception
                temp_xml_result = False
            End Try
            bLoaded = temp_xml_result
            If Not bLoaded Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & sAction, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAction")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oActionElem = oAction.DocumentElement

            sActionType = oActionElem.FirstChild.Name

            If sActionType = ACXMLGenericAction Then
                ' Process the Generic Action
                result = ExecGenericActionXML(sAction, sActionReturnXML)

                ' Convert the results back and Return the Data Set
                r_sDataset = StringsHelper.StrConv(sActionReturnXML, StringsHelper.VbStrConvEnum.vbFromUnicode)

                Return result
            End If

            Select Case sActionType
                Case ACXMLActionRisk
                    sClassName = ACXMLActionRiskClassName
                Case ACXMLActionFinancial
                    sClassName = ACXMLActionFinancialClassName
                Case ACXMLActionSecurity
                    sClassName = ACXMLActionSecurityClassName
                Case ACXMLActionQuotePolicy
                    sClassName = ACXMLActionQuotePolicyClassName
                Case ACXMLActionLookup
                    sClassName = ACXMLActionLookupClassName
                Case ACXMLActionRenewals
                    sClassName = ACXMLActionRenewalsClassName
                Case Else
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to determine the XML ACTION type : " & sAction, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAction")
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

            oBusiness = gPMFunctions.CreateLateBoundObject("bGIS." & sClassName)

            lReturn = oBusiness.Initialise("", "", 1, 1, 1, 1, GISSharedConstants.GetGISLogLevel(), ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sActionType = ACXMLActionRisk Then
                lDatasetStart = lEndOfAction + ACXMLActionEndTag.Length + 2
                lDatasetEnd = Informations.inStr(lDatasetStart, r_sDataset, ACXMLDatasetEndTag)

                If (lDatasetStart < 1) Or (lDatasetEnd < 1) Then
                    sDataset = ""
                    sXMLOldRisk = ""
                Else
                    sDataset = r_sDataset.Substring(lDatasetStart - 1, Math.Min(r_sDataset.Length, lDatasetEnd + ACXMLDatasetEndTag.Length - lDatasetStart))
                    sXMLOldRisk = r_sDataset.Substring(lDatasetEnd + ACXMLDatasetEndTag.Length + 1)
                End If
            Else
                sDataset = ""
                sXMLOldRisk = ""
            End If

            Select Case sActionType
                Case ACXMLActionRisk
                    ' Get the Action Details
                    lReturn = CType(UnFormatActionXML(v_sActionXML:=sAction, r_lAction:=lAction, r_sSellerGUID:=sSellerGUID, r_sDataModelCode:=sDataModelCode, r_lQuoteType:=lQuoteType,
                                                      r_sBusinessTypeCode:=sBusinessTypeCode, r_sQuoteReference:=sQuoteReference, r_sQuoteRefPassword:=sQuoteRefPassword,
                                                      r_sSearchNameNum:=sSearchNameNum, r_sSearchPostCode:=sSearchPostcode, r_lPolicyLinkID:=lPolicyLinkID, r_lSchemeID:=lSchemeID,
                                                      r_sInsurerCode:=sInsurerCode, r_dtEffectiveDate:=dtEffectiveDate, r_lPartyCnt:=lPartyCnt, r_lInsuranceFileCnt:=lInsuranceFileCnt,
                                                      r_dtCoverStartDate:=dtCoverStartDate, r_dtExpiryDate:=dtExpiryDate, r_lPolicyVersion:=lPolicyVersion, r_iType:=iMTAType,
                                                      r_lRiskID:=lRiskID, r_sVehicleReg:=sRegistrationNumber, r_lEMailType:=lEMailType, r_sEMailFrom:=sEMailFrom, r_sEMailTo:=sEMailTo,
                                                      r_sEMailCC:=sEMailCC, r_sEMailSubject:=sEMailSubject, r_sEMailText:=sEMailText, r_vAdditionalDataArray:=vAdditionalData,
                                                      r_lFormNumber:=lFormNumber, r_lInsuranceFolderCnt:=lInsuranceFolderCnt), gPMConstants.PMEReturnCode)

                    ' Get the Data Model Definition
                    lReturn = oBusiness.GetDataModelDef(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode))
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case ACXMLActionFinancial
                    ' Get the Action Details
                    lReturn = CType(UnFormatActionXMLFinancial(v_sActionXML:=sAction, r_lAction:=lAction, r_sSellerGUID:=sSellerGUID, r_sQuoteReference:=sQuoteReference,
                                                               r_sDataModelCode:=sDataModelCode, r_sDatacashRequestType:=sDatacashRequestType, r_sDatacashRef:=sDatacashRef,
                                                               r_sDatacashCardNum:=sDatacashCardNum, r_iDatacashExpMonth:=iDatacashExpMonth, r_iDatacashExpYear:=iDatacashExpYear,
                                                               r_sDatacashAmt:=sDatacashAmt, r_sDatacashAuthCode:=sDatacashAuthCode, r_sDatacashSwitchExtraInfo:=sDatacashSwitchExtraInfo,
                                                               r_lPolicyLinkID:=lPolicyLinkID, r_sDatacashTransactionType:=sDatacashTransactionType,
                                                               r_sBankAccountValidationSenderID:=sBankAccountValidationSenderID, r_sBankAccountValidationCoverType:=sBankAccountValidationCoverType,
                                                               r_sGnetClientCode:=sGnetClientCode, r_sBusinessStatus:=sBusinessStatus, r_sBankAccountValidationBankAccountName:=sBankAccountValidationBankAccountName,
                                                               r_sBankAccountValidationBankAccountNo:=sBankAccountValidationBankAccountNo, r_sBankAccountValidationBankSortCode:=sBankAccountValidationBankSortCode,
                                                               r_sCalcPaymentMethodChargeProductFamily:=sCalcPaymentMethodChargeProductFamily, r_sBusinessTypeCode:=sBusinessTypeCode,
                                                               r_sCalcPaymentMethodChargeTransactionType:=sCalcPaymentMethodChargeTransactionType, r_sCalcPaymentMethodChargePaymentMethod:=sCalcPaymentMethodChargePaymentMethod,
                                                               r_sCalcPaymentMethodChargeStartDate:=sCalcPaymentMethodChargeStartDate, r_sCalcPaymentMethodChargeAmountToFinance:=sCalcPaymentMethodChargeAmountToFinance,
                                                               r_sCalcPaymentMethodChargeNoOfInstalments:=sCalcPaymentMethodChargeRequestedNoOfInstalments, r_sCalcPaymentMethodChargeActionType:=sCalcPaymentMethodChargeActionType,
                                                               r_sCalcPaymentMethodChargeRequestedDepositPercent:=sCalcPaymentMethodChargeRequestedDepositPercent, r_sDataset:=sDatasetContents,
                                                               r_sPremiumFinanceQuotePremiumDifference:=sPremiumFinanceQuotePremiumDifference), gPMConstants.PMEReturnCode)
                Case ACXMLActionSecurity
                    ' Get the Action Details
                    lReturn = CType(UnFormatActionXMLSecurity(v_sActionXML:=sAction, r_lAction:=lAction, r_sSellerGUID:=sSellerGUID, r_sDataModelCode:=sDataModelCode,
                                                              r_sBusinessTypeCode:=sBusinessTypeCode, r_sUserID:=sUserID, r_sPassword:=sPassword, r_sForename:=sForename,
                                                              r_sSurname:=sSurname, r_sMothersMaidenName:=sMothersMaidenName, r_sDateOfBirth:=sDateOfBirth, r_sEmailAddress:=sEmailAddress,
                                                              r_sMemorableDate:=sMemorableDate, r_sAQuestion:=sAQuestion, r_sTheAnswer:=sTheAnswer, r_sCurrentRenewalDate:=sCurrentRenewalDate,
                                                              r_sTitle:=sTitle, r_sMaritalStatusCode:=sMaritalStatusCode, r_sAddress1:=sAddress1, r_sAddress2:=sAddress2,
                                                              r_sAddress3:=sAddress3, r_sAddress4:=sAddress4, r_sPostcode:=sPostcode, r_vAdditionalDataArray:=vAdditionalData),
                                                              gPMConstants.PMEReturnCode)
                Case ACXMLActionQuotePolicy
                    lReturn = CType(UnFormatActionXMLQuotePolicy(v_sActionXML:=sAction, r_lAction:=lAction, r_sDataModelCode:=sDataModelCode, r_sBusinessTypeCode:=sBusinessTypeCode,
                                                                 r_lInsFileSearchType:=lInsFileSearchType, r_lPartyCnt:=lPartyCnt, r_lInsuranceFileCnt:=lInsuranceFileCnt,
                                                                 r_sPolicyTypeCode:=sPolicyTypeCode, r_sInsuranceFileRef:=sInsuranceFileRef, r_sPartyTypeCode:=sPartyTypeCode,
                                                                 r_sShortName:=sShortName, r_sResolvedName:=sResolvedName, r_sUserID:=sUserID, r_lLeadAgentCnt:=lLeadAgentCnt,
                                                                 r_sForename:=sForename, r_sSurname:=sSurname, r_sDateOfBirth:=sDateOfBirth, r_sEmailAddress:=sEmailAddress,
                                                                 r_sCurrentRenewalDate:=sCurrentRenewalDate, r_sAddress1:=sAddress1, r_sAddress2:=sAddress2, r_sAddress3:=sAddress3,
                                                                 r_sAddress4:=sAddress4, r_sPostcode:=sPostcode, r_sTitle:=sTitle, r_sMaritalStatusCode:=sMaritalStatusCode,
                                                                 r_sGenderCode:=sGenderCode, r_sInitials:=sInitials, r_sTelephoneNumber:=sTelephoneNumber,
                                                                 r_sInsFolderDescription:=sInsFolderDescription, r_dCoverStartDate:=dCoverStartDate, r_vAdditionalDataArray:=vAdditionalData,
                                                                 r_lProductID:=lProductID, r_vInsuranceFolderCnt:=lInsuranceFolderCnt), gPMConstants.PMEReturnCode)
                Case ACXMLActionLookup
                    ' process PM & User Def lookup dataset
                    lReturn = CType(UnFormatActionXML(v_sActionXML:=GISSharedConstants.GISDSActionGetLookup, r_lAction:=lAction, r_sSellerGUID:=sSellerGUID,
                                                      r_vAdditionalDataArray:=vListDataMerged), gPMConstants.PMEReturnCode)
                Case ACXMLActionRenewals
                    lReturn = CType(UnFormatActionXMLRenewals(v_sActionXML:=sAction, r_lAction:=lAction, r_sDataModelCode:=sDataModelCode, r_sBusinessTypeCode:=sBusinessTypeCode,
                                                              r_lDataModelID:=lDataModelID, r_lInsuranceFolderCnt:=lInsuranceFolderCnt, r_lInsuranceFileCnt:=lInsuranceFileCnt,
                                                              r_lPartyCnt:=lPartyCnt, r_lProductID:=lProductID, r_dtRenewalDate:=dtRenewalDate, r_lRiskCodeID:=lRiskCodeID,
                                                              r_lRenewalEdiAuditId:=lRenewalEDIAuditID, r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt,
                                                              r_lRenewalSchemeID:=lRenewalSchemeID, r_sRenewalStatusCode:=sRenewalStatusCode, r_lSchemeID:=lSchemeID,
                                                              r_lOldInsuranceFileCnt:=lOldInsuranceFileCnt, r_dtDueDateStart:=dtDueDateStart, r_dtDueDateLimit:=dtDueDateLimit,
                                                              r_sClientCode:=sClientCode, r_sPolicyNo:=sPolicyNo, r_lInsurerID:=lInsurerID, r_lSuspensionLevel:=lSuspensionLevel), gPMConstants.PMEReturnCode)
            End Select

            ' Get the Empty Data Set Strings
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sDataSetDef, r_sXMLDataSet:=sEmptyDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Process the Action
            Select Case lAction
                Case GISSharedConstants.GISDSActionNewDataSet
                    ' Create the New Data Set
                    lActionReturn = oBusiness.NewRiskDataset(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), r_lPolicyLinkID:=lPolicyLinkID,
                                                             r_sTopOIKey:=sTopOIKey, r_sXMLDataSetDef:=sDataSetDef, r_sXMLDataset:=sDataset, r_sQuoteRef:=sQuoteReference,
                                                             r_sQuoteRefPassword:=sQuoteRefPassword, v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt))

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_sQuoteReference:=sQuoteReference, v_sQuoteRefPassword:=sQuoteRefPassword), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case GISSharedConstants.GISDSActionQuote
                    ' Perform the Quote
                    lActionReturn = oBusiness.NBQuote(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode),
                                                      v_lQuoteType:=gPMFunctions.ToSafeInteger(lQuoteType), r_sXMLDataset:=sDataset,
                                                      v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_dtEffectiveDate:=gPMFunctions.ToSafeDate(dtEffectiveDate), r_vAdditionalDataArray:=vAdditionalData)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vAdditionalDataArray:=vAdditionalData), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedConstants.GISDSActionMTAQuote
                    ' Perform the Quote
                    lActionReturn = oBusiness.MTAQuote(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_lQuoteType:=gPMFunctions.ToSafeInteger(lQuoteType), r_sXMLNewRisk:=sDataset, v_sXMLOldRisk:=gPMFunctions.ToSafeString(sXMLOldRisk), v_dtEffectiveDate:=gPMFunctions.ToSafeDate(dtEffectiveDate), r_vAdditionalDataArray:=vAdditionalData)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vAdditionalDataArray:=vAdditionalData), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case GISSharedConstants.GISDSActionMTATransact
                    ' Perform the Quote
                    lActionReturn = oBusiness.MTATransact(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_lQuoteType:=gPMFunctions.ToSafeInteger(lQuoteType), r_sXMLNewRisk:=sDataset, v_sXMLOldRisk:=gPMFunctions.ToSafeString(sXMLOldRisk), v_dtEffectiveDate:=gPMFunctions.ToSafeDate(dtEffectiveDate), r_vAdditionalDataArray:=vAdditionalData)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vAdditionalDataArray:=vAdditionalData), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case GISSharedConstants.GISDSActionSaveToDB
                    ' Save to Database
                    lActionReturn = oBusiness.SaveToDB(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), r_sXMLDataset:=sDataset)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case GISSharedConstants.GISDSActionLoadFromDB
                    ' Load From the Database
                    lActionReturn = oBusiness.LoadRiskFromDB(r_sXMLDataSetDef:=sDataSetDef, r_sXMLDataset:=sDataset,
                                                             r_sGISDataModelCode:=sDataModelCode, r_sQuoteRef:=sQuoteReference,
                                                             r_sQuoteRefPassword:=sQuoteRefPassword, r_dtGuaranteedQuoteDate:=dtGuaranteedQuoteDate,
                                                             v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lInsuranceFileCnt), v_lRiskID:=gPMFunctions.ToSafeInteger(lRiskID), v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt))

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_dtGuaranteedQuoteDate:=dtGuaranteedQuoteDate), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case GISSharedConstants.GISDSActionUpdateQtePassword
                    lActionReturn = oBusiness.UpdateQuoteRef(v_lGISPolicyLinkID:=gPMFunctions.ToSafeInteger(lPolicyLinkID), v_sQuoteRef:=gPMFunctions.ToSafeString(sQuoteReference), v_sQuoteRefPassword:=gPMFunctions.ToSafeString(sQuoteRefPassword), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode))

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Search For Address
                Case GISSharedConstants.GISDSActionSearchForAddress

                    lActionReturn = oBusiness.PostcodeSearch(v_sHouseNameNum:=gPMFunctions.ToSafeString(sSearchNameNum), v_sPostcode:=gPMFunctions.ToSafeString(sSearchPostcode), r_vMatchArray:=vMatchArray)

                    ' Format the Action Return XML

                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, v_vAddressArray:=vMatchArray, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' NB Transaction
                Case GISSharedConstants.GISDSActionNBTransact

                    lActionReturn = oBusiness.NBTransact(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), r_sXMLDataset:=sDataset, r_vAdditionalDataArray:=vAdditionalData)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vAdditionalDataArray:=vAdditionalData), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Update PartyCnt in gis_policy_link table
                Case GISSharedConstants.GISDSActionUpdatePartyCnt

                    ' Update the Party Cnt

                    lActionReturn = oBusiness.UpdatePartyCnt(v_lGISPolicyLinkID:=gPMFunctions.ToSafeInteger(lPolicyLinkID), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lInsuranceFileCnt), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode))

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' NBStart
                Case GISSharedConstants.GISDSActionNBStart

                    ' Start a New Business Quote

                    lActionReturn = oBusiness.NBStart(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), r_lPolicyLinkID:=lPolicyLinkID, r_sTopOIKey:=sTopOIKey, r_sXMLDataSetDef:=sDataSetDef, r_sXMLDataset:=sDataset, v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), r_lInsuranceFileCnt:=lInsuranceFileCnt, r_sQuoteRef:=sQuoteReference, r_sQuoteRefPassword:=sQuoteRefPassword, r_vAdditionalDataArray:=vAdditionalData)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_sQuoteReference:=sQuoteReference, v_sQuoteRefPassword:=sQuoteRefPassword, v_lInsuranceFileCnt:=lInsuranceFileCnt, v_vAdditionalDataArray:=vAdditionalData), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' MTAStart
                Case GISSharedConstants.GISDSActionMTAStart

                    ' Start a Mid Term Adjustment Business Quote

                    lActionReturn = oBusiness.MTAStart(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_iType:=gPMFunctions.ToSafeInteger(iMTAType),
                                                       v_dtCoverStartDate:=gPMFunctions.ToSafeDate(dtCoverStartDate), v_dtExpiryDate:=gPMFunctions.ToSafeDate(dtExpiryDate), v_lPolicyVersion:=gPMFunctions.ToSafeInteger(lPolicyVersion), r_sXMLDataSetDef:=sDataSetDef, r_sXMLDataset:=sDataset, v_lOldGISPolicyLinkID:=gPMFunctions.ToSafeInteger(lPolicyLinkID), v_lOldInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lInsuranceFileCnt),
                                                       r_lNewGISPolicyLinkID:=lNewPolicyLinkID, r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, r_vAdditionalDataArray:=vAdditionalData)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lInsuranceFileCnt:=lNewInsuranceFileCnt, v_lPolicyLinkID:=lNewPolicyLinkID, v_vAdditionalDataArray:=vAdditionalData), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refer
                Case GISSharedConstants.GISDSActionRefer

                    ' RAG 201100
                    ' Replace old refer method with new.

                    'lActionReturn = oBusiness.CallQEMToRefer( _
                    'v_sXMLDataSetDef:=gpmfunctions.tosafestring(sDataSetDef), _
                    'v_sGISDataModelCode:=gpmfunctions.tosafestring(sDataModelCode), _
                    'v_sGISBusinessTypeCode:="OIM", _
                    'v_sXMLDataSet:=gpmfunctions.tosafestring(sDataset), _
                    'v_sInsurerCode:=gpmfunctions.tosafestring(sInsurerCode))

                    lActionReturn = oBusiness.Refer(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode),
                                                    r_sXMLDataset:=sDataset, v_sInsurerCode:=gPMFunctions.ToSafeString(sInsurerCode), r_vAdditionalDataArray:=vAdditionalData)

                    ' RFC040501 - Added Additional Data Parameter
                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vAdditionalDataArray:=vAdditionalData), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RAG171100
                    ' VehicleLookup
                Case GISSharedConstants.GISDSActionVehicleLookup

                    ' Perform the Quote

                    lActionReturn = oBusiness.VehicleLookup(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), r_sXMLDataset:=sDataset, v_sRegistrationNumber:=gPMFunctions.ToSafeString(sRegistrationNumber), r_vAdditionalDataArray:=vAdditionalData)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RAG201100
                    ' SendEmail
                Case GISSharedConstants.GISDSActionSendEmail

                    ' Perform the Quote

                    lActionReturn = oBusiness.SendEmail(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), r_sXMLDataset:=sDataset,
                                                        v_lEMailType:=gPMFunctions.ToSafeInteger(lEMailType), v_sEMailFrom:=gPMFunctions.ToSafeString(sEMailFrom), v_sEMailTo:=gPMFunctions.ToSafeString(sEMailTo),
                                                        v_sEMailCC:=gPMFunctions.ToSafeString(sEMailCC), v_sEMailSubject:=gPMFunctions.ToSafeString(sEMailSubject), v_sEMailText:=gPMFunctions.ToSafeString(sEMailText), r_vAdditionalDataArray:=vAdditionalData)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Datacash transact - CL190500 (home)
                Case GISSharedConstants.GISDSActionFinancialDatacash

                    lActionReturn = oBusiness.DataCash(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sDatacashRequestType:=gPMFunctions.ToSafeString(sDatacashRequestType), v_sDatacashRef:=gPMFunctions.ToSafeString(sDatacashRef), v_sDatacashCardNum:=gPMFunctions.ToSafeString(sDatacashCardNum), v_iDatacashExpMonth:=gPMFunctions.ToSafeInteger(iDatacashExpMonth), v_iDatacashExpYear:=gPMFunctions.ToSafeInteger(iDatacashExpYear), v_sDatacashAmt:=gPMFunctions.ToSafeString(sDatacashAmt), v_sDatacashAuthCode:=gPMFunctions.ToSafeString(sDatacashAuthCode), v_sDatacashSwitchExtraInfo:=gPMFunctions.ToSafeString(sDatacashSwitchExtraInfo), v_lPolicyLinkID:=gPMFunctions.ToSafeInteger(lPolicyLinkID), v_sDatacashTransactionType:=gPMFunctions.ToSafeString(sDatacashTransactionType), r_vDatacashResponseArray:=vDatacashResponseArray)

                    ' Format the Action Return XML

                    lReturn = CType(FormatActionReturnXMLFinancial(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vDatacashResponseArray:=vDatacashResponseArray), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Bank Account Validation - CJB111000
                Case GISSharedConstants.GISDSActionFinancialBankAccountValidation

                    lActionReturn = oBusiness.BankAccountValidation(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_sQuoteRef:=gPMFunctions.ToSafeString(sQuoteReference), v_lPolicyLinkID:=gPMFunctions.ToSafeInteger(lPolicyLinkID), v_sBankAccountValidationSenderID:=gPMFunctions.ToSafeString(sBankAccountValidationSenderID), v_sBankAccountValidationCoverType:=gPMFunctions.ToSafeString(sBankAccountValidationCoverType), v_sBankAccountValidationGnetClientCode:=gPMFunctions.ToSafeString(sGnetClientCode), v_sBankAccountValidationBusinessStatus:=gPMFunctions.ToSafeString(sBusinessStatus),
                                                                    v_sBankAccountValidationBankAccountName:=gPMFunctions.ToSafeString(sBankAccountValidationBankAccountName), v_sBankAccountValidationBankAccountNo:=gPMFunctions.ToSafeString(sBankAccountValidationBankAccountNo),
                                                                    v_sBankAccountValidationBankSortCode:=gPMFunctions.ToSafeString(sBankAccountValidationBankSortCode), r_sBankAccountValidationStatusCode:=sBankAccountValidationStatusCode)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXMLFinancial(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_sBankAccountValidationStatusCode:=gPMFunctions.ToSafeString(sBankAccountValidationStatusCode)), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Calc Payment Method Charge - CJB191000
                Case GISSharedConstants.GISDSActionFinancialCalcPaymentMethodCharge

                    lActionReturn = oBusiness.CalcPaymentMethodCharge(v_sCalcPaymentMethodChargeProductFamily:=gPMFunctions.ToSafeString(sCalcPaymentMethodChargeProductFamily), v_sBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode),
                                                                      v_sCalcPaymentMethodChargeTransactionType:=gPMFunctions.ToSafeString(sCalcPaymentMethodChargeTransactionType), v_sCalcPaymentMethodChargePaymentMethod:=gPMFunctions.ToSafeString(sCalcPaymentMethodChargePaymentMethod),
                                                                      v_sCalcPaymentMethodChargeStartDate:=gPMFunctions.ToSafeString(sCalcPaymentMethodChargeStartDate), v_sCalcPaymentMethodChargeAmountToFinance:=gPMFunctions.ToSafeString(sCalcPaymentMethodChargeAmountToFinance), v_sCalcPaymentMethodChargeNoOfInstalments:=gPMFunctions.ToSafeString(sCalcPaymentMethodChargeRequestedNoOfInstalments),
                                                                      v_sCalcPaymentMethodChargeActionType:=gPMFunctions.ToSafeString(sCalcPaymentMethodChargeActionType), v_sCalcPaymentMethodChargeRequestedDepositPercent:=gPMFunctions.ToSafeString(sCalcPaymentMethodChargeRequestedDepositPercent), r_sCalcPaymentMethodChargeInterestRate:=sCalcPaymentMethodChargeInterestRate, r_sCalcPaymentMethodChargeAPR:=sCalcPaymentMethodChargeAPR,
                                                                      r_sCalcPaymentMethodChargeInterestCost:=sCalcPaymentMethodChargeInterestCost, r_sCalcPaymentMethodChargeNoOfInstalments:=sCalcPaymentMethodChargeNoOfInstalments,
                                                                      r_sCalcPaymentMethodChargeFirstInstalment:=sCalcPaymentMethodChargeFirstInstalment, r_sCalcPaymentMethodChargeOthInstalments:=sCalcPaymentMethodChargeOthInstalments, r_sCalcPaymentMethodChargeDeposit:=sCalcPaymentMethodChargeDeposit,
                                                                      r_sCalcPaymentMethodChargeArrangementFee:=sCalcPaymentMethodChargeArrangementFee, r_sCalcPaymentMethodChargeDepositPercent:=sCalcPaymentMethodChargeDepositPercent,
                                                                      r_sCalcPaymentMethodChargeCompanyName:=sCalcPaymentMethodChargeCompanyName, r_sCalcPaymentMethodChargeCompanyNo:=sCalcPaymentMethodChargeCompanyNo,
                                                                      r_sCalcPaymentMethodChargeSchemeName:=sCalcPaymentMethodChargeSchemeName, r_sCalcPaymentMethodChargeSchemeNo:=sCalcPaymentMethodChargeSchemeNo, r_sCalcPaymentMethodChargeSchemeVer:=sCalcPaymentMethodChargeSchemeVer, r_sCalcPaymentMethodChargeBasisOfCalc:=sCalcPaymentMethodChargeBasisOfCalc)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXMLFinancial(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML,
                                                                   v_sCalcPaymentMethodChargeInterestRate:=sCalcPaymentMethodChargeInterestRate, v_sCalcPaymentMethodChargeAPR:=sCalcPaymentMethodChargeAPR,
                                                                   v_sCalcPaymentMethodChargeInterestCost:=sCalcPaymentMethodChargeInterestCost, v_sCalcPaymentMethodChargeNoOfInstalments:=sCalcPaymentMethodChargeNoOfInstalments,
                                                                   v_sCalcPaymentMethodChargeFirstInstalment:=sCalcPaymentMethodChargeFirstInstalment, v_sCalcPaymentMethodChargeOthInstalments:=sCalcPaymentMethodChargeOthInstalments,
                                                                   v_sCalcPaymentMethodChargeDeposit:=sCalcPaymentMethodChargeDeposit, v_sCalcPaymentMethodChargeArrangementFee:=sCalcPaymentMethodChargeArrangementFee,
                                                                   v_sCalcPaymentMethodChargeDepositPercent:=sCalcPaymentMethodChargeDepositPercent, v_sCalcPaymentMethodChargeCompanyName:=sCalcPaymentMethodChargeCompanyName,
                                                                   v_sCalcPaymentMethodChargeCompanyNo:=sCalcPaymentMethodChargeCompanyNo, v_sCalcPaymentMethodChargeSchemeName:=sCalcPaymentMethodChargeSchemeName,
                                                                   v_sCalcPaymentMethodChargeSchemeNo:=sCalcPaymentMethodChargeSchemeNo, v_sCalcPaymentMethodChargeSchemeVer:=sCalcPaymentMethodChargeSchemeVer,
                                                                   v_sCalcPaymentMethodChargeBasisOfCalc:=sCalcPaymentMethodChargeBasisOfCalc), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Premium Finance Quote - CJB121200
                Case GISSharedConstants.GISDSActionFinancialPremiumFinanceQuote

                    lActionReturn = oBusiness.PremiumFinanceQuote(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode),
                                                                  v_sQuoteRef:=gPMFunctions.ToSafeString(sQuoteReference), v_lPolicyLinkID:=gPMFunctions.ToSafeInteger(lPolicyLinkID),
                                                                  v_sGnetClientCode:=gPMFunctions.ToSafeString(sGnetClientCode), v_sBusinessStatus:=gPMFunctions.ToSafeString(sBusinessStatus),
                                                                  v_sDatasetContents:=gPMFunctions.ToSafeString(sDatasetContents), v_sPremiumFinanceQuoteMTAStartDate:=gPMFunctions.ToSafeString(sMTAStartDate),
                                                                  v_sPremiumFinanceQuotePremiumDifference:=gPMFunctions.ToSafeString(sPremiumFinanceQuotePremiumDifference),
                                                                  r_sPremiumFinanceQuoteStatusCode:=sPremiumFinanceQuoteStatusCode, r_sPremiumFinanceQuoteRemainingInst:=sPremiumFinanceQuoteRemainingInst,
                                                                  r_sPremiumFinanceQuoteFirstInstalAmt:=sPremiumFinanceQuoteFirstInstalAmt, r_sPremiumFinanceQuoteSubsequentInstalAmt:=sPremiumFinanceQuoteSubsequentInstalAmt,
                                                                  r_sPremiumFinanceQuoteActualPaymentDate:=sPremiumFinanceQuoteActualPaymentDate)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXMLFinancial(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML,
                                                                   v_sPremiumFinanceQuoteStatusCode:=sPremiumFinanceQuoteStatusCode,
                                                                   v_sPremiumFinanceQuoteRemainingInst:=sPremiumFinanceQuoteRemainingInst,
                                                                   v_sPremiumFinanceQuoteFirstInstalAmt:=sPremiumFinanceQuoteFirstInstalAmt,
                                                                   v_sPremiumFinanceQuoteSubsequentInstalAmt:=sPremiumFinanceQuoteSubsequentInstalAmt,
                                                                   v_sPremiumFinanceQuoteActualPaymentDate:=sPremiumFinanceQuoteActualPaymentDate),
                                                                   gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedConstants.GISDSActionSecurityRegisterUser

                    ' RFC050900 - Added extra parameters required by new Register User Method
                    ' RFC050900 - Note we are not doing anything with the Add Data Array at the
                    '             moment to pass it through as empty.

                    lActionReturn = oBusiness.RegisterUser(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode),
                                                           v_sForename:=gPMFunctions.ToSafeString(sForename), v_sSurname:=gPMFunctions.ToSafeString(sSurname), v_sMothersMaidenName:=gPMFunctions.ToSafeString(sMothersMaidenName),
                                                           v_sDateOfBirth:=gPMFunctions.ToSafeString(sDateOfBirth), v_sEmailAddress:=gPMFunctions.ToSafeString(sEmailAddress),
                                                           v_sMemorableDate:=gPMFunctions.ToSafeString(sMemorableDate), v_sAQuestion:=gPMFunctions.ToSafeString(sAQuestion),
                                                           v_sTheAnswer:=gPMFunctions.ToSafeString(sTheAnswer), v_sCurrentRenewalDate:=gPMFunctions.ToSafeString(sCurrentRenewalDate),
                                                           r_sUserID:=sUserID, r_sPassword:=sPassword, r_lPartyCnt:=lPartyCnt,
                                                           v_sTitle:=gPMFunctions.ToSafeString(sTitle), v_sMaritalStatusCode:=gPMFunctions.ToSafeString(sMaritalStatusCode),
                                                           v_sAddress1:=gPMFunctions.ToSafeString(sAddress1), v_sAddress2:=gPMFunctions.ToSafeString(sAddress2), v_sAddress3:=gPMFunctions.ToSafeString(sAddress3),
                                                           v_sAddress4:=gPMFunctions.ToSafeString(sAddress4), v_sPostcode:=gPMFunctions.ToSafeString(sPostcode), r_vAdditionalDataArray:=vAdditionalData)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXMLSecurity(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_sUserID:=sUserID, v_sPassword:=sPassword, v_lPartyCnt:=lPartyCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedConstants.GISDSActionSecurityLoginUser

                    ' RFC050900 - Added extra parameters required by new LoginUser Method
                    ' RFC050900 - Note we are not doing anything with the Add Data Array at the
                    '             moment to pass it through as empty.

                    lActionReturn = oBusiness.LoginUser(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_sUserID:=gPMFunctions.ToSafeString(sUserID), v_sPassword:=gPMFunctions.ToSafeString(sPassword), r_lPartyCnt:=lPartyCnt,
                                                        r_lPMUserID:=lPMUserID, r_sSurname:=sSurname, r_sForename:=sForename, r_dtDateOfBirth:=dtDateOfBirth, r_sEmailAddress:=sEmailAddress, r_vAdditionalDataArray:=vAdditionalData)

                    ' Format the Action Return XML
                    lReturn = CType(FormatActionReturnXMLSecurity(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lPartyCnt:=lPartyCnt, v_lPMUserID:=lPMUserID, v_sForename:=sForename, v_sSurname:=sSurname, v_dtDateOfBirth:=dtDateOfBirth, v_sEmailAddress:=sEmailAddress), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RFC220600
                Case GISSharedConstants.GISDSActionGetQuotesPoliciesForParty

                    ' Call the method

                    lActionReturn = oBusiness.GetQuotesPoliciesForParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_lSearchType:=gPMFunctions.ToSafeInteger(lInsFileSearchType), r_vQuotePolicyArray:=vQuotePolicyArray, v_sPolicyTypeCode:=gPMFunctions.ToSafeString(sPolicyTypeCode))

                    lReturn = CType(FormatActionReturnXMLQuotePolicy(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vQuotePolicyArray:=vQuotePolicyArray), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RFC220600
                Case GISSharedConstants.GISDSActionGetPolicyVersions

                    ' Call the method

                    lActionReturn = oBusiness.GetPolicyVersions(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), r_vPolicyVersionArray:=vPolicyVersionArray, v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lInsuranceFileCnt), v_sInsuranceFileRef:=gPMFunctions.ToSafeString(sInsuranceFileRef))

                    lReturn = CType(FormatActionReturnXMLQuotePolicy(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vPolicyVersionArray:=vPolicyVersionArray), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'RJG 22/11/2000 Add AddParty, FindParty and GetQuotesForPart method calls
                Case GISSharedConstants.GISDSActionAddParty
                    ' Call the method

                    lActionReturn = oBusiness.AddParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_sPartyTypeCode:=gPMFunctions.ToSafeString(sPartyTypeCode), v_sForename:=gPMFunctions.ToSafeString(sForename), v_sSurname:=gPMFunctions.ToSafeString(sSurname), v_sDateOfBirth:=gPMFunctions.ToSafeString(sDateOfBirth), v_sEmailAddress:=gPMFunctions.ToSafeString(sEmailAddress), v_sCurrentRenewalDate:=gPMFunctions.ToSafeString(sCurrentRenewalDate), v_sAddress1:=gPMFunctions.ToSafeString(sAddress1), v_sAddress2:=gPMFunctions.ToSafeString(sAddress2), v_sAddress3:=gPMFunctions.ToSafeString(sAddress3), v_sAddress4:=gPMFunctions.ToSafeString(sAddress4), v_sPostcode:=gPMFunctions.ToSafeString(sPostcode), r_lPartyCnt:=lPartyCnt, v_sTitle:=gPMFunctions.ToSafeString(sTitle), v_sMaritalStatusCode:=gPMFunctions.ToSafeString(sMaritalStatusCode), v_sGenderCode:=gPMFunctions.ToSafeString(sGenderCode),
                                                       v_sInitials:=gPMFunctions.ToSafeString(sInitials), v_sTelephoneNumber:=gPMFunctions.ToSafeString(sTelephoneNumber), r_vAdditionalDataArray:=vAdditionalData)

                    lReturn = CType(FormatActionReturnXMLQuotePolicy(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lPartyCnt:=lPartyCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedConstants.GISDSActionFindParty
                    ' Call the method

                    lActionReturn = oBusiness.FindParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_sPartyType:=gPMFunctions.ToSafeString(sPartyTypeCode), v_sShortname:=gPMFunctions.ToSafeString(sShortName),
                                                        v_sResolvedName:=gPMFunctions.ToSafeString(sResolvedName), v_sUserID:=gPMFunctions.ToSafeString(sUserID), v_sTelephoneNumber:=gPMFunctions.ToSafeString(sTelephoneNumber), v_sPostcode:=gPMFunctions.ToSafeString(sPostcode), r_vResultArray:=vPartyArray,
                                                        v_lLeadAgentCnt:=gPMFunctions.ToSafeInteger(lLeadAgentCnt), v_vAdditionalDataArray:=vAdditionalData)

                    lReturn = CType(FormatActionReturnXMLQuotePolicy(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vPartyArray:=vPartyArray), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedConstants.GISDSActionGetQuotesForParty
                    ' Call the method

                    lActionReturn = oBusiness.GetQuotesForParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), r_vQuoteArray:=vQuoteArray, v_sPolicyTypeCode:=gPMFunctions.ToSafeString(sPolicyTypeCode))

                    lReturn = CType(FormatActionReturnXMLQuotePolicy(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vQuoteArray:=vQuoteArray), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedConstants.GISDSActionGetParty

                    lActionReturn = oBusiness.GetParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode),
                                                       v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), r_sSurname:=sSurname, r_sForename:=sForename, r_sPartyType:=sPartyTypeCode,
                                                       r_sAddress1:=sAddress1, r_sAddress2:=sAddress2, r_sAddress3:=sAddress3, r_sAddress4:=sAddress4, r_sPostcode:=sPostcode,
                                                       r_sDOB:=sDateOfBirth, r_sEMail:=sEmailAddress, r_sUserID:=sUserID, r_sPassword:=sPassword, r_sShortName:=sShortName,
                                                       r_sResolvedName:=sResolvedName, r_sMothersMaidenName:=sMothersMaidenName, r_sTPUserCode:=sTPUserCode, r_sTPIntroducer:=sTPIntroducer,
                                                       r_sAQuestion:=sAQuestion, r_sTheAnswer:=sTheAnswer, r_dMemorableDate:=sMemorableDate, r_dCurrInsRenewalDate:=sCurrentRenewalDate,
                                                       r_sTitle:=sTitle, r_sMaritalStatusCode:=sMaritalStatusCode, r_sGenderCode:=sGenderCode, r_sInitials:=sInitials, r_sTelephoneNumber:=sTelephoneNumber)

                    lReturn = CType(FormatActionReturnXMLQuotePolicy(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lPartyCnt:=lPartyCnt, v_sSurname:=sSurname, v_sForename:=sForename, v_sPartyTypeCode:=sPartyTypeCode, v_sAddress1:=sAddress1, v_sAddress2:=sAddress2, v_sAddress3:=sAddress3, v_sAddress4:=sAddress4, v_sPostcode:=sPostcode, v_sDateOfBirth:=sDateOfBirth, v_sEMail:=sEmailAddress, r_sUserID:=sUserID, v_sPassword:=sPassword, v_sShortname:=sShortName, v_sResolvedName:=sResolvedName, v_sMothersMaidenName:=sMothersMaidenName,
                                                                     v_sTPUserCode:=sTPUserCode, v_sTPIntroducer:=sTPIntroducer, v_sAQuestion:=sAQuestion, v_sTheAnswer:=sTheAnswer, v_sMemorableDate:=sMemorableDate, v_sCurrInsRenewalDate:=sCurrentRenewalDate, v_sTitle:=sTitle, v_sMaritalStatusCode:=sMaritalStatusCode, v_sGenderCode:=sGenderCode, v_sInitials:=sInitials, v_sTelephoneNumber:=sTelephoneNumber), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedConstants.GISDSActionFindQuote

                    lActionReturn = oBusiness.FindQuote(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), r_vResultArray:=vQuoteArray, v_sQuoteRef:=gPMFunctions.ToSafeString(sInsuranceFileRef), v_dCoverStartDate:=dCoverStartDate,
                                                        v_sDescription:=gPMFunctions.ToSafeString(sInsFolderDescription), v_lLeadAgentCnt:=gPMFunctions.ToSafeInteger(lLeadAgentCnt))

                    lReturn = CType(FormatActionReturnXMLQuotePolicy(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vFindQuoteArray:=vQuoteArray), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedConstants.GISDSActionGetProductByAgent

                    lActionReturn = oBusiness.GetProductByAgent(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_lAgentPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), r_vResultArray:=vProductArray, v_vAdditionalDataArray:=vAdditionalData)

                    lReturn = CType(FormatActionReturnXMLQuotePolicy(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vProductArray:=vProductArray), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case GISSharedConstants.GISDSActionFindPolicy
                    ' Call the method

                    lActionReturn = oBusiness.FindPolicy(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), r_vResultArray:=vFindPolicyArray, v_vAdditionalDataArray:=vAdditionalData)

                    lReturn = CType(FormatActionReturnXMLQuotePolicy(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vFindPolicyArray:=vFindPolicyArray), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RDC 30052001 - PM & User Def lookups
                Case GISSharedConstants.GISDSActionGetLookup
                    ' call routine that searches PM and User Def lookup tables
                    ' array contains object, property & optional search code

                    lActionReturn = oBusiness.ProcessLookupRequest(vAdditionalData, gPMFunctions.ToSafeString(sDataModelCode))

                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vAdditionalDataArray:=vAdditionalData), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'RJG13082001 - Print Continuation Cover Notes
                Case GISSharedConstants.GISDSActionPrintForm
                    ' Call the method

                    lActionReturn = oBusiness.PrintForm(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_sXMLDataSet:=gPMFunctions.ToSafeString(sDataset), v_lFormNumber:=gPMFunctions.ToSafeInteger(lFormNumber), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID))

                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 -
                Case GISSharedConstants.GISDSActionRenSelection

                    lActionReturn = oBusiness.RenSelection(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate),
                                                           v_lRiskCodeID:=gPMFunctions.ToSafeInteger(lRiskCodeID), r_sGISDataModelCode:=sDataModelCode, v_sBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_sDataModelCode:=sDataModelCode), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Create a new renewal quotation
                Case GISSharedConstants.GISDSActionRenQuotationBrokerLead

                    lActionReturn = oBusiness.RenQuotationBrokerLead(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate), v_lRiskCodeID:=gPMFunctions.ToSafeInteger(lRiskCodeID),
                                                                     v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt,
                                                                     v_lRenewalGISSchemeID:=gPMFunctions.ToSafeInteger(lRenewalSchemeID), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Generate an invite to renew a policy
                Case GISSharedConstants.GISDSActionRenInvitationBrokerLead

                    lActionReturn = oBusiness.RenInvitationBrokerLed(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate), v_lRiskCodeID:=gPMFunctions.ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID),
                                                                     v_lProductID:=gPMFunctions.ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=gPMFunctions.ToSafeInteger(lRenewalSchemeID),
                                                                     v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(lRenewalEDIAuditID), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lRenewalInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lRenewalInsuranceFileCnt)), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Reprint an invite to renew a policy
                Case GISSharedConstants.GISDSActionRenReprintInvitationBrokerLead

                    lActionReturn = oBusiness.RenReprintInvitationBrokerLead(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate), v_lRiskCodeID:=gPMFunctions.ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_lProductID:=gPMFunctions.ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=gPMFunctions.ToSafeInteger(lRenewalSchemeID), v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(lRenewalEDIAuditID), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Invite Preferred Quotations
                Case GISSharedConstants.GISDSActionRenInvitePreferredQuotes

                    lActionReturn = oBusiness.RenInvitePreferredQuotes(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate), v_lRiskCodeID:=gPMFunctions.ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_lProductID:=gPMFunctions.ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=gPMFunctions.ToSafeInteger(lRenewalSchemeID), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Produce docs for the policy
                Case GISSharedConstants.GISDSActionRenConfDocsHoldingInsurer

                    lActionReturn = oBusiness.RenConfDocsHoldingInsurer(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate), v_lRiskCodeID:=gPMFunctions.ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_lProductID:=gPMFunctions.ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=gPMFunctions.ToSafeInteger(lRenewalSchemeID), v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(lRenewalEDIAuditID), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Confirm a Renewal by EDI
                    'TF031201 - Call new function for GNet (Broker Led)
                Case GISSharedConstants.GISDSActionRenConfirmRenewal

                    lActionReturn = oBusiness.ConfirmRenewalBrokerLed(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lInsuranceFileCnt), v_lSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Confirm a lapse by EDI
                Case GISSharedConstants.GISDSActionRenConfirmLapse

                    lActionReturn = oBusiness.ConfirmLapse(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lInsuranceFileCnt), v_lSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Lapse a renewal/policy
                Case GISSharedConstants.GISDSActionRenCompLapse

                    lActionReturn = oBusiness.RenCompLapse(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(lRenewalEDIAuditID), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_lRenewalGISSchemeID:=gPMFunctions.ToSafeInteger(lRenewalSchemeID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lProductID:=gPMFunctions.ToSafeInteger(lProductID), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_lRiskCodeID:=gPMFunctions.ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_sRenewalStatusCode:=gPMFunctions.ToSafeString(sRenewalStatusCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Renew with the existing Insurer
                Case GISSharedConstants.GISDSActionRenCompHoldingInsurer

                    lActionReturn = oBusiness.RenCompHoldingInsurer(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(lRenewalEDIAuditID), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_lRenewalGISSchemeID:=gPMFunctions.ToSafeInteger(lRenewalSchemeID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lProductID:=gPMFunctions.ToSafeInteger(lProductID), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_lRiskCodeID:=gPMFunctions.ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Renew with another Insurer
                Case GISSharedConstants.GISDSActionRenCompAlternateInsurer

                    lActionReturn = oBusiness.RenCompAlternateInsurer(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(lRenewalEDIAuditID), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_lRenewalGISSchemeID:=gPMFunctions.ToSafeInteger(lRenewalSchemeID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lProductID:=gPMFunctions.ToSafeInteger(lProductID), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_lRiskCodeID:=gPMFunctions.ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode),
                                                                      v_lGisBusinessTypeId:=gPMFunctions.ToSafeInteger(lBusinessTypeID), r_lNewInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lInsuranceFileCnt))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lInsuranceFileCnt:=lInsuranceFileCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Get the Renewal version
                Case GISSharedConstants.GISDSActionRenGetPolicyRenewalVersion

                    lActionReturn = oBusiness.GetPolicyRenewalVersion(v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode), v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), r_vResultArray:=vPolicyVersionArray)

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vRenewalArray:=vPolicyVersionArray), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Produce the reminder docs
                Case GISSharedConstants.GISDSActionRenReminder

                    lActionReturn = oBusiness.RenReminder(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_lRenInsFileCnt:=gPMFunctions.ToSafeInteger(lRenewalInsuranceFileCnt), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Reprint the confirmation docs
                Case GISSharedConstants.GISDSActionRenReprintConfirm

                    lActionReturn = oBusiness.RenReprintConfirm(v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(lRenewalEDIAuditID), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Resend the EDI message for a policy
                Case GISSharedConstants.GISDSActionRenResendEDI

                    lActionReturn = oBusiness.RenResendEDI(v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(lRenewalEDIAuditID), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(sBusinessTypeCode))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - Complete the Renewal Proceess
                Case GISSharedConstants.GISDSActionRenCompletion

                    lActionReturn = oBusiness.RenCompletion(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(lRenewalEDIAuditID), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_lRenewalGISSchemeID:=gPMFunctions.ToSafeInteger(lRenewalSchemeID), v_lRenewalInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lRenewalInsuranceFileCnt), v_lProductID:=gPMFunctions.ToSafeInteger(lProductID), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate), v_lPartyCnt:=gPMFunctions.ToSafeInteger(lPartyCnt), v_lRiskCodeID:=gPMFunctions.ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_lGisBusinessTypeId:=gPMFunctions.ToSafeInteger(lBusinessTypeID),
                                                            v_lOldInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lOldInsuranceFileCnt))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD23102001 - List policies up for renewal
                Case GISSharedConstants.GISDSActionRenListRenewals

                    lActionReturn = oBusiness.ListRenewals(r_vResultArray:=vFindPolicyArray, v_sDataModelCode:=gPMFunctions.ToSafeString(sDataModelCode), v_sRenewalStatus:=gPMFunctions.ToSafeString(sRenewalStatusCode),
                                                           v_dtDueDateStart:=gPMFunctions.ToSafeDate(dtDueDateStart), v_dtDueDateLimit:=gPMFunctions.ToSafeDate(dtDueDateLimit), v_sClientCode:=gPMFunctions.ToSafeString(sClientCode),
                                                           v_sPolicyNo:=gPMFunctions.ToSafeString(sPolicyNo), v_lBusinessTypeID:=gPMFunctions.ToSafeInteger(lBusinessTypeID), v_lSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_lInsurerId:=gPMFunctions.ToSafeInteger(lInsurerID), v_lSuspensionLevel:=gPMFunctions.ToSafeInteger(lSuspensionLevel))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML, v_vRenewalArray:=vFindPolicyArray), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD05112001 - Update the Renewal Control Record
                Case GISSharedConstants.GISDSActionRenUpdateRenewalControl

                    lActionReturn = oBusiness.UpdateRenewalControl(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt), v_vProductID:=gPMFunctions.ToSafeInteger(lProductID), v_vRenewalInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lInsuranceFileCnt), v_vRenewalStatusTypeCode:=gPMFunctions.ToSafeString(sRenewalStatusCode), v_vSuspensionLevel:=gPMFunctions.ToSafeInteger(lSuspensionLevel), v_vRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(lRenewalEDIAuditID), v_vRenewalGisSchemeID:=gPMFunctions.ToSafeInteger(lRenewalSchemeID), v_vGISSchemeID:=gPMFunctions.ToSafeInteger(lSchemeID), v_vRenewalDate:=gPMFunctions.ToSafeDate(dtDueDateStart), v_vGISDataModelID:=gPMFunctions.ToSafeInteger(lDataModelID), v_vOldInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lOldInsuranceFileCnt))

                    lReturn = CType(FormatActionReturnXMLRenewals(v_lReturnValue:=lActionReturn, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Unknown Action
                Case Else

                    ' Format the Action Return XML (INVALID REQUEST)
                    lReturn = CType(FormatActionReturnXML(v_lReturnValue:=gPMConstants.PMEReturnCode.PMInvalidRequest, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

            End Select

            ' Return the Results from the Action
            r_sDataset = sActionReturnXML & sDataset

            ' Convert the results back and Return the Data Set
            r_sDataset = StringsHelper.StrConv(r_sDataset, StringsHelper.VbStrConvEnum.vbFromUnicode)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessActionFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

            Return result
        End Try
    End Function

    ' PUBLIC Methods (End)
End Class
