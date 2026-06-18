Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
'developer guide no. 129
Imports SharedFiles
Module GISActionXMLFuncs
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
	
	
	Private Const ACClass As String = "GISActionXMLFuncs"
	
	Public Const ACXMLAction As String = "ACTION"
	Public Const ACXMLActionEndTag As String = "</ACTION>"
	
	Public Const ACXMLActionRiskClassName As String = "APPLICATION" 'CL240500
	
	
	Public Const ACXMLActionRisk As String = "ACTION_RISK"
	Public Const ACXMLActionRiskEndTag As String = "</ACTION_RISK>"
	
	Public Const ACXMLDataset As String = "<DATA_SET>" ' CL170100
	Public Const ACXMLDatasetEndTag As String = "</DATA_SET>" ' CL170100
	
	Public Const ACXMLActionReturn As String = "ACTION_RETURN"
	Public Const ACXMLActionReturnEndTag As String = "</ACTION_RETURN>"
	
	Public Const ACXMLARetAddressMatch As String = "ADDRESS_MATCH"
	Public Const ACXMLARetPremise As String = "PREMISE"
	Public Const ACXMLARetSubPremise As String = "SUBPREMISE"
	Public Const ACXMLARetAddressLine As String = "ADDRESS_LINE"
	Public Const ACXMLARetPostCode As String = "POSTCODE"
	
	' RDC30052001
	Public Const ACXMLActionLookupClassName As String = "APPLICATION"
	
	Public Const ACXMLActionLookup As String = "ACTION_LOOKUP"
	Public Const ACXMLActionLookupEndTag As String = "</ACTION_LOOKUP>"
	
	
	' Action Attributes
	Public Const ACXMLAttribSellerGUID As String = "SellerGUID"
	Public Const ACXMLAttribQuoteType As String = "QuoteType"
	Public Const ACXMLAttribGISAction As String = "GISAction" ' RG201100
	Public Const ACXMLAttribDataModelCode As String = "DataModelCode"
	Public Const ACXMLAttribBusinessTypeCode As String = "BusinessTypeCode"
	Public Const ACXMLAttribQuoteReference As String = "QuoteReference"
	Public Const ACXMLAttribQuoteRefPassword As String = "QuoteRefPassword"
	Public Const ACXMLAttribSearchNameNum As String = "SearchNameNum"
	Public Const ACXMLAttribSearchPostcode As String = "SearchPostCode"
	Public Const ACXMLAttribPolicyLinkID As String = "PolicyLinkID"
	Public Const ACXMLAttribSchemeID As String = "SchemeID"
	Public Const ACXMLAttribInsurerCode As String = "InsurerCode"
	' RFC13012000 - Add Effective Date and Guaranteed Quote Date params.
	Public Const ACXMLAttribEffectiveDate As String = "EffectiveDate"
	Public Const ACXMLAttribGteedQuoteDate As String = "GuaranteedQuoteDate"
	' RAG15062000
	Public Const ACXMLAttribPartyCnt As String = "PartyCnt"
	'RFC260600
	Public Const ACXMLAttribCoverStartDate As String = "CoverStartDate"
	Public Const ACXMLAttribExpiryDate As String = "ExpiryDate"
	Public Const ACXMLAttribPolicyVersion As String = "PolicyVersion"
	'CL100800
	Public Const ACXMLAttribMTAType As String = "MTAType"
	'RFC271000
	Public Const ACXMLAttribRiskID As String = "RiskID"
	'RAG171100
	Public Const ACXMLAttribVehicleReg As String = "VehicleReg"
	
	'RAG201100 - Email parameters
	Public Const ACXMLAttribEmailType As String = "EmailType"
	Public Const ACXMLAttribEmailFrom As String = "EmailFrom"
	Public Const ACXMLAttribEmailTo As String = "EmailTo"
	Public Const ACXMLAttribEmailCC As String = "EmailCC"
	Public Const ACXMLAttribEmailSubject As String = "EmailSubject"
	Public Const ACXMLAttribEmailText As String = "EmailText"
	
	' RDC 30052001 - PM & User Def Lookup attribs
	Public Const ACXMLAttribLookupObject As String = "LookupObject"
	Public Const ACXMLAttribLookupProperty As String = "LookupProperty"
	Public Const ACXMLAttribLookupSearchCode As String = "LookupSearchCode"
	Public Const ACXMLAttribLookupData As String = "LookupData"
	
	'Action Return Attributes
	Public Const ACXMLAttribReturnValue As String = "ReturnValue"
	
	'RJG 13082001 - Extra params needed for Continuation Covernote printing
	Public Const ACXMLAttribFormNumber As String = "FormNumber"
	' ***************************************************************** '
	' Name: FormatActionXML
	'
	' Description:
	'
	' RFC13012000 - Add Effective Date
	' RFC26062000 - Add Insurance File Cnt
	' RFC271000 - Add optional RiskID as required by underwriting.
	' RAG171100 - Add optional Registration Number for VehicleLookup
	' RAG201100 - Add optional Email parameters for SendEmail method
	' CJB170902 - Format all dates consistently - they must be formatted
	'             or they may be misinterpreted.
	' ***************************************************************** '
    Public Function FormatActionXML(ByVal v_lAction As Integer, ByVal v_sSellerGUID As String, ByVal v_sDataModelCode As String, ByRef r_sActionXML As String, Optional ByVal v_lQuoteType As Integer = -1, Optional ByVal v_sBusinessTypeCode As String = "", Optional ByVal v_sQuoteReference As String = "", Optional ByVal v_sQuoteRefPassword As String = "", Optional ByVal v_sSearchNameNum As String = "", Optional ByVal v_sSearchPostCode As String = "", Optional ByVal v_lPolicyLinkID As Integer = -1, Optional ByVal v_lSchemeID As Integer = -1, Optional ByVal v_sInsurerCode As String = "", Optional ByVal v_dtEffectiveDate As Date = iGISSharedConstants.GISLowDate, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = -1, Optional ByVal v_dtCoverStartDate As Date = iGISSharedConstants.GISLowDate, Optional ByVal v_dtExpiryDate As Date = iGISSharedConstants.GISLowDate, Optional ByVal v_lPolicyVersion As Integer = -1, Optional ByVal v_iType As Integer = 0, Optional ByVal v_lRiskID As Integer = -1, Optional ByVal v_sVehicleReg As String = "", Optional ByVal v_lEMailType As Integer = 0, Optional ByVal v_sEMailFrom As String = "", Optional ByVal v_sEMailTo As String = "", Optional ByVal v_sEMailCC As String = "", Optional ByVal v_sEMailSubject As String = "", Optional ByVal v_sEMailText As String = "", Optional ByVal v_vAdditionalDataArray(,) As Object = Nothing, Optional ByVal v_lFormNumber As Integer = 0, Optional ByVal v_lInsuranceFolderCnt As Integer = -1) As Integer


        Dim result As Integer = 0
        Dim oAction As XmlDocument
        Dim oActionElem, oActionChildElem As XmlElement
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New XML Document
            oAction = New XmlDocument()

            ' Create the Action Element
            oActionElem = oAction.CreateElement(ACXMLAction)
            oActionChildElem = oAction.CreateElement(ACXMLActionRisk)

            ' Set the Action Value
            'oActionChildElem.Text = v_lAction
            oActionChildElem.SetAttribute(ACXMLAttribGISAction, v_lAction)

            ' Set the Other Attributes
            oActionChildElem.SetAttribute(ACXMLAttribSellerGUID, v_sSellerGUID)
            oActionChildElem.SetAttribute(ACXMLAttribDataModelCode, v_sDataModelCode)
            oActionChildElem.SetAttribute(ACXMLAttribQuoteType, v_lQuoteType)
            oActionChildElem.SetAttribute(ACXMLAttribBusinessTypeCode, v_sBusinessTypeCode)
            oActionChildElem.SetAttribute(ACXMLAttribQuoteReference, v_sQuoteReference)
            oActionChildElem.SetAttribute(ACXMLAttribQuoteRefPassword, v_sQuoteRefPassword)
            oActionChildElem.SetAttribute(ACXMLAttribSearchNameNum, v_sSearchNameNum)
            oActionChildElem.SetAttribute(ACXMLAttribSearchPostcode, v_sSearchPostCode)
            oActionChildElem.SetAttribute(ACXMLAttribPolicyLinkID, v_lPolicyLinkID)
            oActionChildElem.SetAttribute(ACXMLAttribSchemeID, v_lSchemeID)
            oActionChildElem.SetAttribute(ACXMLAttribInsurerCode, v_sInsurerCode)

            ' CJB170902 - Change formatting below at RFCs request
            ' RFC13012000 - Add Effective Date
            ' oActionChildElem.setAttribute ACXMLAttribEffectiveDate, Format$(v_dtEffectiveDate, "dd/mm/yyyy hh:nn:ss")
            oActionChildElem.SetAttribute(ACXMLAttribEffectiveDate, v_dtEffectiveDate.ToString("yyyy-MM-dd HH:mm:ss"))

            ' RAG15062000 - Add PartyCnt for UpdatePartyCnt Method
            oActionChildElem.SetAttribute(ACXMLAttribPartyCnt, v_lPartyCnt)

            ' RFC260600 - Add InsuranceFileCnt
            oActionChildElem.SetAttribute(ACXMLAttribInsuranceFileCnt, v_lInsuranceFileCnt)
            oActionChildElem.SetAttribute(ACXMLAttribInsuranceFolderCnt, v_lInsuranceFolderCnt)

            ' RFC260600 - Extra Params required by MTA Start
            ' CJB170902 - Format all dates consistently
            oActionChildElem.SetAttribute(ACXMLAttribCoverStartDate, v_dtCoverStartDate.ToString("yyyy-MM-dd HH:mm:ss"))
            oActionChildElem.SetAttribute(ACXMLAttribExpiryDate, v_dtExpiryDate.ToString("yyyy-MM-dd HH:mm:ss"))
            oActionChildElem.SetAttribute(ACXMLAttribPolicyVersion, v_lPolicyVersion)

            ' CL100800 - MTA type
            oActionChildElem.SetAttribute(ACXMLAttribMTAType, v_iType)

            'RFC271000 - RiskID
            oActionChildElem.SetAttribute(ACXMLAttribRiskID, v_lRiskID)

            ' RAG171100 - Add RegistrationNumber for VehicleLookup Method
            oActionChildElem.SetAttribute(ACXMLAttribVehicleReg, v_sVehicleReg)

            ' RAG201100 - Add Email paramters for SendEmail Method
            oActionChildElem.SetAttribute(ACXMLAttribEmailType, v_lEMailType)
            oActionChildElem.SetAttribute(ACXMLAttribEmailFrom, v_sEMailFrom)
            oActionChildElem.SetAttribute(ACXMLAttribEmailTo, v_sEMailTo)
            oActionChildElem.SetAttribute(ACXMLAttribEmailCC, v_sEMailCC)
            oActionChildElem.SetAttribute(ACXMLAttribEmailSubject, v_sEMailSubject)
            oActionChildElem.SetAttribute(ACXMLAttribEmailText, v_sEMailText)

            'RJG13082001 - Additional Params for PrintForm method
            oActionChildElem.SetAttribute(ACXMLAttribFormNumber, v_lFormNumber)


            ' RAG171100 - Add the Additional Data Items

            lReturn = CType(FormatAdditionalDataXML(r_oDocument:=oAction, r_oParentElem:=oActionChildElem, v_vAdditionalDataArray:=v_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If



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
            bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatActionXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatActionXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	' ***************************************************************** '
	' Name: UnFormatActionXML
	'
	' Description:
	'
	' RFC13012000 - Add Effective Date
	' RFC26062000 - Add Insurance File Cnt
	' RFC271000 - Add optional RiskID as required by underwriting.
	' RAG171100 - Add optional vehicle reg for vehicle lookup
	' RAG201100 - Add optional parameters for SendEmail method
	' ***************************************************************** '
    Public Function UnFormatActionXML(ByVal v_sActionXML As String, ByRef r_lAction As Integer, ByRef r_sSellerGUID As String, Optional ByRef r_sDataModelCode As String = "", Optional ByRef r_lQuoteType As Integer = 0, Optional ByRef r_sBusinessTypeCode As String = "", Optional ByRef r_sQuoteReference As String = "", Optional ByRef r_sQuoteRefPassword As String = "", Optional ByRef r_sSearchNameNum As String = "", Optional ByRef r_sSearchPostCode As String = "", Optional ByRef r_lPolicyLinkID As Integer = 0, Optional ByRef r_lSchemeID As Integer = 0, Optional ByRef r_sInsurerCode As String = "", Optional ByRef r_dtEffectiveDate As Date = #12/30/1899#, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_lInsuranceFileCnt As Integer = 0, Optional ByRef r_dtCoverStartDate As Date = #12/30/1899#, Optional ByRef r_dtExpiryDate As Date = #12/30/1899#, Optional ByRef r_lPolicyVersion As Integer = 0, Optional ByRef r_iType As Integer = 0, Optional ByRef r_lRiskID As Integer = -1, Optional ByRef r_sVehicleReg As String = "", Optional ByRef r_lEMailType As Integer = 0, Optional ByRef r_sEMailFrom As String = "", Optional ByRef r_sEMailTo As String = "", Optional ByRef r_sEMailCC As String = "", Optional ByRef r_sEMailSubject As String = "", Optional ByRef r_sEMailText As String = "", Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing, Optional ByRef r_lFormNumber As Integer = -1, Optional ByRef r_lInsuranceFolderCnt As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim oActionElem As XmlElement
        Dim oAction As XmlDocument
        Dim bLoaded As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Document
            oAction = New XmlDocument()

            ' Load the Action XML

            'developer guide no. no solution 22
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
                bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & v_sActionXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionXML")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oActionElem = oAction.DocumentElement
            oActionElem = oAction.DocumentElement.FirstChild ' CL240500

            ' Get the Action Value
            'r_lAction = oActionElem.Text

            r_lAction = CInt(oActionElem.GetAttribute(ACXMLAttribGISAction))

            r_sDataModelCode = CStr(oActionElem.GetAttribute(ACXMLAttribDataModelCode))

            ' Get the Other Attributes

            r_sSellerGUID = CStr(oActionElem.GetAttribute(ACXMLAttribSellerGUID))

            r_sDataModelCode = CStr(oActionElem.GetAttribute(ACXMLAttribDataModelCode))

            r_lQuoteType = CInt(oActionElem.GetAttribute(ACXMLAttribQuoteType))

            r_sBusinessTypeCode = CStr(oActionElem.GetAttribute(ACXMLAttribBusinessTypeCode))

            r_sQuoteReference = CStr(oActionElem.GetAttribute(ACXMLAttribQuoteReference))

            r_sQuoteRefPassword = CStr(oActionElem.GetAttribute(ACXMLAttribQuoteRefPassword))

            r_sSearchNameNum = CStr(oActionElem.GetAttribute(ACXMLAttribSearchNameNum))

            r_sSearchPostCode = CStr(oActionElem.GetAttribute(ACXMLAttribSearchPostcode))

            r_lPolicyLinkID = CInt(oActionElem.GetAttribute(ACXMLAttribPolicyLinkID))

            r_lSchemeID = CInt(oActionElem.GetAttribute(ACXMLAttribSchemeID))

            r_sInsurerCode = CStr(oActionElem.GetAttribute(ACXMLAttribInsurerCode))
            ' RFC13012000 - Add Effective Date

            r_dtEffectiveDate = CDate(oActionElem.GetAttribute(ACXMLAttribEffectiveDate))
            ' RAG15062000 - Add PartyCnt (for UpdatePartyCnt method)

            r_lPartyCnt = CInt(oActionElem.GetAttribute(ACXMLAttribPartyCnt))
            ' RFC26062000 - Add InsuranceFileCnt

            r_lInsuranceFileCnt = CInt(oActionElem.GetAttribute(ACXMLAttribInsuranceFileCnt))
            ' RDT 21112002

            r_lInsuranceFolderCnt = CInt(oActionElem.GetAttribute(ACXMLAttribInsuranceFolderCnt))
            ' RFC260600 - Extra Params required by MTA Start

            r_dtCoverStartDate = CDate(oActionElem.GetAttribute(ACXMLAttribCoverStartDate))

            r_dtExpiryDate = CDate(oActionElem.GetAttribute(ACXMLAttribExpiryDate))

            r_lPolicyVersion = CInt(oActionElem.GetAttribute(ACXMLAttribPolicyVersion))


            r_iType = CInt(oActionElem.GetAttribute(ACXMLAttribMTAType))

            'RFC271000

            r_lRiskID = CInt(oActionElem.GetAttribute(ACXMLAttribRiskID))

            ' RAG171100

            r_sVehicleReg = CStr(oActionElem.GetAttribute(ACXMLAttribVehicleReg))

            ' RAG201100 - EMail paramters for SendEmail method

            r_lEMailType = CInt(oActionElem.GetAttribute(ACXMLAttribEmailType))

            r_sEMailFrom = CStr(oActionElem.GetAttribute(ACXMLAttribEmailFrom))

            r_sEMailTo = CStr(oActionElem.GetAttribute(ACXMLAttribEmailTo))

            r_sEMailCC = CStr(oActionElem.GetAttribute(ACXMLAttribEmailCC))

            r_sEMailSubject = CStr(oActionElem.GetAttribute(ACXMLAttribEmailSubject))

            r_sEMailText = CStr(oActionElem.GetAttribute(ACXMLAttribEmailText))

            'RJG13082001 - Params for PrintForm method

            r_lFormNumber = CInt(oActionElem.GetAttribute(ACXMLAttribFormNumber))

            ' RAG 201100
            ' Return the Additional Data Array

            lReturn = CType(UnFormatAdditionalDataXML(r_oDocument:=oAction, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If


            oActionElem = Nothing
            oAction = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError



            ' Log Error Message
            bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatActionXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	
	' ***************************************************************** '
	' Name: FormatActionReturnXML
	'
	' Description:
	'
	' RFC13012000 - Add Guaranteed Quote Date
	' RFC26062000 - Add Insurance File Cnt & PolicyLinkID
	' CJB170902 - Format all dates consistently - they must be formatted
	'             or they may be misinterpreted.
	' ***************************************************************** '
    Public Function FormatActionReturnXML(ByVal v_lReturnValue As Integer, ByRef r_sActionReturnXML As String, Optional ByVal v_sQuoteReference As String = "", Optional ByVal v_sQuoteRefPassword As String = "", Optional ByVal v_dtGuaranteedQuoteDate As Date = iGISSharedConstants.GISLowDate, Optional ByVal v_vAddressArray(,) As Object = Nothing, Optional ByVal v_lInsuranceFileCnt As Integer = -1, Optional ByVal v_lPolicyLinkID As Integer = -1, Optional ByVal v_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oActionReturn As XmlDocument
        Dim oActionReturnElem, oAddressElem, oAddLineElem As XmlElement

        Dim oElem, oElemChild As XmlElement

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New XML Document
            oActionReturn = New XmlDocument()

            ' Create the Action Element
            oActionReturnElem = oActionReturn.CreateElement(ACXMLActionReturn)

            oActionReturnElem.InnerText = "Action Return"

            ' Set the Attributes
            oActionReturnElem.SetAttribute(ACXMLAttribReturnValue, v_lReturnValue)
            oActionReturnElem.SetAttribute(ACXMLAttribQuoteReference, v_sQuoteReference)
            oActionReturnElem.SetAttribute(ACXMLAttribQuoteRefPassword, v_sQuoteRefPassword)
            ' RFC13012000 - Add Guaranteed Quote Date
            ' CJB170902 - Change formatting below at RFCs request
            'oActionReturnElem.setAttribute ACXMLAttribGteedQuoteDate, Format$(v_dtGuaranteedQuoteDate, "dd/mm/yyyy hh:nn:ss")
            oActionReturnElem.SetAttribute(ACXMLAttribGteedQuoteDate, v_dtGuaranteedQuoteDate.ToString("yyyy-MM-dd HH:mm:ss"))

            ' RFC260600 - Add InsuranceFileCnt
            oActionReturnElem.SetAttribute(ACXMLAttribInsuranceFileCnt, v_lInsuranceFileCnt)
            oActionReturnElem.SetAttribute(ACXMLAttribPolicyLinkID, v_lPolicyLinkID)

            ' Do we have an Address Match Arrray
            If Information.IsArray(v_vAddressArray) Then

                ' Add Each Match
                For lRow As Integer = v_vAddressArray.GetLowerBound(1) To v_vAddressArray.GetUpperBound(1)

                    oAddressElem = oActionReturn.CreateElement(ACXMLARetAddressMatch)

                    oActionReturnElem.AppendChild(oAddressElem)


                    oAddLineElem = oActionReturn.CreateElement(ACXMLARetPremise)

                    oAddLineElem.InnerText = CStr(v_vAddressArray(iGISSharedConstants.GISAddressPremiseNumber, lRow))

                    oAddressElem.AppendChild(oAddLineElem)

                    oAddLineElem = oActionReturn.CreateElement(ACXMLARetSubPremise)

                    oAddLineElem.InnerText = CStr(v_vAddressArray(iGISSharedConstants.GISAddressSubPremise, lRow))

                    oAddressElem.AppendChild(oAddLineElem)

                    oAddLineElem = oActionReturn.CreateElement(ACXMLARetAddressLine & CStr(iGISSharedConstants.GISAddressLine1))

                    oAddLineElem.InnerText = CStr(v_vAddressArray(iGISSharedConstants.GISAddressLine1, lRow))

                    oAddressElem.AppendChild(oAddLineElem)

                    oAddLineElem = oActionReturn.CreateElement(ACXMLARetAddressLine & CStr(iGISSharedConstants.GISAddressLine2))

                    oAddLineElem.InnerText = CStr(v_vAddressArray(iGISSharedConstants.GISAddressLine2, lRow))

                    oAddressElem.AppendChild(oAddLineElem)

                    oAddLineElem = oActionReturn.CreateElement(ACXMLARetAddressLine & CStr(iGISSharedConstants.GISAddressLine3))

                    oAddLineElem.InnerText = CStr(v_vAddressArray(iGISSharedConstants.GISAddressLine3, lRow))

                    oAddressElem.AppendChild(oAddLineElem)

                    oAddLineElem = oActionReturn.CreateElement(ACXMLARetPostCode)

                    oAddLineElem.InnerText = CStr(v_vAddressArray(iGISSharedConstants.GISAddressPostCode, lRow))

                    oAddressElem.AppendChild(oAddLineElem)

                Next lRow

                oAddressElem = Nothing
                oAddLineElem = Nothing

            End If

            'RJG061200 - Add the Additional Data Items

            lReturn = CType(FormatAdditionalDataXML(r_oDocument:=oActionReturn, r_oParentElem:=oActionReturnElem, v_vAdditionalDataArray:=v_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatActionReturnXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatActionReturnXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	' ***************************************************************** '
	' Name: UnFormatActionReturnXML
	'
	' Description:
	'
	' RFC13012000 - Add Guaranteed Quote Date'
	' RFC26062000 - Add Insurance File Cnt & PolicyLinkID
	' ***************************************************************** '
    Public Function UnFormatActionReturnXML(ByVal v_sActionReturnXML As String, ByRef r_lReturnValue As Integer, Optional ByRef r_sQuoteReference As String = "", Optional ByRef r_sQuoteRefPassword As String = "", Optional ByRef r_vAddressArray(,) As Object = Nothing, Optional ByRef r_dtGuaranteedQuoteDate As Date = #12/30/1899#, Optional ByRef r_lInsuranceFileCnt As Integer = 0, Optional ByRef r_lPolicyLinkID As Integer = 0, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oActionReturnElem As XmlElement
        Dim oActionReturn As XmlDocument
        Dim bLoaded As Boolean
        Dim oMatches As XmlNodeList
        Dim oMatch As XmlElement
        Dim oAddLineElem, oElem As XmlNodeList
        Dim lNumMatches As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

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
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & v_sActionReturnXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionReturnXML")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oActionReturnElem = oActionReturn.DocumentElement

            ' Get the Attributes

            r_lReturnValue = CInt(oActionReturnElem.GetAttribute(ACXMLAttribReturnValue))

            r_sQuoteReference = CStr(oActionReturnElem.GetAttribute(ACXMLAttribQuoteReference))

            r_sQuoteRefPassword = CStr(oActionReturnElem.GetAttribute(ACXMLAttribQuoteRefPassword))
            ' RFC13012000 - Add Guaranteed Quote Date

            r_dtGuaranteedQuoteDate = CDate(oActionReturnElem.GetAttribute(ACXMLAttribGteedQuoteDate))
            ' RFC26062000 - AddInsuranceFileCnt

            r_lInsuranceFileCnt = CInt(oActionReturnElem.GetAttribute(ACXMLAttribInsuranceFileCnt))

            r_lPolicyLinkID = CInt(oActionReturnElem.GetAttribute(ACXMLAttribPolicyLinkID))

            ' Are there any Postcode Matches
            oMatches = oActionReturn.GetElementsByTagName(ACXMLARetAddressMatch)
            If Not (oMatches Is Nothing) Then

                ' How Many Matches are there
                lNumMatches = oMatches.Count

                ' If there are matches then
                If lNumMatches > 0 Then

                    ' Resize the Array (to 5 'DB 16/11/99)
                    ReDim r_vAddressArray(iGISSharedConstants.GISAddressPostCode, lNumMatches - 1)

                    ' Add them to the Array
                    For lRow As Integer = 0 To lNumMatches - 1

                        ' Get a Reference to a Match
                        oMatch = oMatches.Item(lRow)

                        oAddLineElem = oMatch.GetElementsByTagName(ACXMLARetSubPremise)

                        r_vAddressArray(iGISSharedConstants.GISAddressSubPremise, lRow) = oAddLineElem.Item(0).InnerText

                        oAddLineElem = oMatch.GetElementsByTagName(ACXMLARetPremise)

                        r_vAddressArray(iGISSharedConstants.GISAddressPremiseNumber, lRow) = oAddLineElem.Item(0).InnerText

                        oAddLineElem = oMatch.GetElementsByTagName(ACXMLARetAddressLine & "2") 'DB (1->2)

                        r_vAddressArray(iGISSharedConstants.GISAddressLine1, lRow) = oAddLineElem.Item(0).InnerText

                        oAddLineElem = oMatch.GetElementsByTagName(ACXMLARetAddressLine & "3") 'DB (2->3)

                        r_vAddressArray(iGISSharedConstants.GISAddressLine2, lRow) = oAddLineElem.Item(0).InnerText

                        oAddLineElem = oMatch.GetElementsByTagName(ACXMLARetAddressLine & "4") 'DB (3->4)

                        r_vAddressArray(iGISSharedConstants.GISAddressLine3, lRow) = oAddLineElem.Item(0).InnerText

                        oAddLineElem = oMatch.GetElementsByTagName(ACXMLARetPostCode)

                        r_vAddressArray(iGISSharedConstants.GISAddressPostCode, lRow) = oAddLineElem.Item(0).InnerText

                    Next lRow

                End If

            End If

            ' Return the Additional Data Array

            lReturn = CType(UnFormatAdditionalDataXML(r_oDocument:=oActionReturn, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            oActionReturnElem = Nothing
            oActionReturn = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatActionReturnXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionReturnXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
