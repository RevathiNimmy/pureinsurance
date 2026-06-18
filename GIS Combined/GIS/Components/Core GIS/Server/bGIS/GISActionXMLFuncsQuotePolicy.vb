Option Strict Off
Option Explicit On
Imports System.Xml
Imports SSP.Shared

Module GISActionXMLFuncsQuotePolicy
    ' ***************************************************************** '
    ' Module Name: GISActionXMLFuncsQuotePolicy
    '
    ' Date:  RFC210600
    '
    ' Description: Functions to handle the Action XML
    '
    ' Edit History:
    ' ***************************************************************** '


    Private Const ACClass As String = "GISActionXMLFuncsQuotePolicy"

    Public Const ACXMLActionQuotePolicy As String = "ACTION_QUOTEPOLICY"
    Public Const ACXMLActionQuotePolicyEndTag As String = "</ACTION_QUOTEPOLICY>"

    Public Const ACXMLActionQuotePolicyClassName As String = "QUOTEPOLICY"

    Public Const ACXMLAttribPolicyTypeCode As String = "PolicyTypeCode"
    Public Const ACXMLAttribInsuranceFileCnt As String = "InsuranceFileCnt"
    'Public Const ACXMLAttribInsuranceFolderCnt As String = "InsuranceFolderCnt"
    ' RFC120700  - Get Policy Versions Via Insurance Ref
    Public Const ACXMLAttribInsuranceFileRef As String = "InsuranceFileRef"
    Public Const ACXMLAttribInsFileSearchType As String = "InsFileSearchType"

    Public Const ACXMLARetInsuranceFile As String = "INSURANCE_FILE"
    Public Const ACXMLARetPolicyVersion As String = "POLICY_VERSION"
    Public Const ACXMLARetInsFileType As String = "INS_FILE_TYPE"
    Public Const ACXMLARetInsFileCnt As String = "INS_FILE_CNT"
    Public Const ACXMLARetPartyName As String = "PARTY_NAME"
    Public Const ACXMLARetQuoteOrPolNo As String = "QUOTE_OR_POL_NO"
    Public Const ACXMLARetDescription As String = "DESCRIPTION"
    Public Const ACXMLARetDate As String = "DATE"
    Public Const ACXMLARetInsFileVersion As String = "VERSION"
    Public Const ACXMLARetInsFolderCnt As String = "INS_FOLDER_CNT"

    'RJG 17/07/2000 - Expiry Date
    Public Const ACXMLARetExpiryDate As String = "EXPIRY_DATE"

    'RJG 21/11/2000 - XML tags for FindParty, AddParty and GetQuotesForParty methods

    'RJG 21/11/2000 - Input parameters for FindParty
    Public Const ACXMLAttribPartyTypeCode As String = "PartyTypeCode"
    Public Const ACXMLAttribPartyShortname As String = "ShortName"
    Public Const ACXMLAttribPartyResolvedName As String = "ResolvedName"
    Public Const ACXMLAttribPartyUserID As String = "UserID"
    Public Const ACXMLAttribPartyLeadAgentCnt As String = "LeadAgentCnt"
    Public Const ACXMLAttribPartyAdditionalData As String = "AdditionalData"

    'RJG 21/11/2000 - Input params for AddParty
    Public Const ACXMLAttribPartyForename As String = "Forename"
    Public Const ACXMLAttribPartySurname As String = "Surname"
    Public Const ACXMLAttribPartyDateOfBirth As String = "DateOfBirth"
    Public Const ACXMLAttribPartyEMailAddress As String = "EMailAddress"
    Public Const ACXMLAttribPartyCurrentRenewalDate As String = "CurrentRenewalDate"
    Public Const ACXMLAttribPartyAddress1 As String = "Address1"
    Public Const ACXMLAttribPartyAddress2 As String = "Address2"
    Public Const ACXMLAttribPartyAddress3 As String = "Address3"
    Public Const ACXMLAttribPartyAddress4 As String = "Address4"
    Public Const ACXMLAttribPartyPostCode As String = "PostCode"
    Public Const ACXMLAttribPartyTitle As String = "Title"
    Public Const ACXMLAttribPartyMaritalStatusCode As String = "MaritalStatusCode"
    Public Const ACXMLAttribPartyGenderCode As String = "GenderCode"
    Public Const ACXMLAttribPartyInitials As String = "Initials"
    Public Const ACXMLAttribPartyTelephoneNumber As String = "TelephoneNumber"

    'RJG 21/11/2000 - Output Params for AddParty
    Public Const ACXMLARetPartyCnt As String = "PARTY_CNT"

    'RJG 22/11/2000 - Other return tags for FindParty method
    Public Const ACXMLARetPartyDetail As String = "PARTY_DETAIL"
    Public Const ACXMLARetPartyShortname As String = "SHORTNAME"
    Public Const ACXMLARetPartyResolvedName As String = "RESOLVED_NAME"
    Public Const ACXMLARetPartyAddress1 As String = "ADDRESS1"
    Public Const ACXMLARetPartyPostcode As String = "POSTCODE"
    Public Const ACXMLARetPartyTelephoneNumber As String = "TELEPHONE_NUMBER"
    Public Const ACXMLARetPartyDateOfBirth As String = "DATE_OF_BIRTH"
    '**** START CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 09:38   ****
    '**** Added the agent_cnt to the result array
    Public Const ACXMLARetPartyAgentCnt As String = "Agent_Cnt"
    '****   END CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 09:38   ****

    '**** START CHANGES - Changed By: AAB  - Changed On: 05-Sep-2002 08:57   ****
    '**** Input Param for GetRiskForProduct method
    Public Const ACXMLAttribProductID As String = "PRODUCT_ID"
    '****   END CHANGES - Changed By: AAB  - Changed On: 05-Sep-2002 08:57   ****

    'RJG 28/11/2000 - Return Tags for GetParty
    Public Const ACXMLARetPartySurname As String = "SURNAME"
    Public Const ACXMLARetPartyForename As String = "FORENAME"
    Public Const ACXMLARetPartyTypeCode As String = "PARTY_TYPE_CODE"
    Public Const ACXMLARetPartyAddress2 As String = "ADDRESS2"
    Public Const ACXMLARetPartyAddress3 As String = "ADDRESS3"
    Public Const ACXMLARetPartyAddress4 As String = "ADDRESS4"
    Public Const ACXMLARetPartyEMail As String = "E_MAIL"
    Public Const ACXMLARetPartyUserID As String = "USER_ID"
    Public Const ACXMLARetPartyPassword As String = "PASSWORD"
    Public Const ACXMLARetPartyMothersMaidenName As String = "MOTHERS_MAIDEN_NAME"
    Public Const ACXMLARetPartyTPUserCode As String = "TP_USER_CODE"
    Public Const ACXMLARetPartyTPIntroducer As String = "TP_INTRODUCER"
    Public Const ACXMLARetPartyAQuestion As String = "A_QUESTION"
    Public Const ACXMLARetPartyTheAnswer As String = "THE_ANSWER"
    Public Const ACXMLARetPartyMemorableDate As String = "MEMORABLE_DATE"
    Public Const ACXMLARetPartyCurrInsRenewalDate As String = "CURR_INS_RENEWAL_DATE"
    Public Const ACXMLARetPartyTitle As String = "TITLE"
    Public Const ACXMLARetPartyMaritalStatusCode As String = "MARITAL_STATUS_CODE"
    Public Const ACXMLARetPartyInitials As String = "INITIALS"
    Public Const ACXMLARetPartyGenderCode As String = "GENDER"

    'RJG 21/11/2000 - Input params for GetQuotesForParty
    Public Const ACXMLAttribPartyPartyCnt As String = "PartyCnt"

    'RJG 04/12/2000 - Input params for FindQuote
    Public Const ACXMLAttribInsFolderDescription As String = "Ins_Folder_Description"
    Public Const ACXMLAttribCoverStartDate As String = "Cover_Start_Date"

    'RJG 10/01/2001 - Return XML tags for GetProductByAgent
    Public Const ACXMLARetProduct As String = "PRODUCT"
    Public Const ACXMLARetProductID As String = "PRODUCT_ID"
    Public Const ACXMLARetProductCode As String = "PRODUCT_CODE"
    Public Const ACXMLARetProductDescription As String = "PRODUCT_DESCRIPTION"
    Public Const ACXMLARetProductSchemeAgencyRef As String = "SCHEME_AGENCY_REF"
    Public Const ACXMLARetProductBlockNo As String = "BLOCK_NO"

    'CL240101
    Public Const ACXMLARetPolicyDetail As String = "POLICY_DETAIL"
    Public Const ACXMLARetPolicyNo As String = "Policy_No"
    Public Const ACXMLARetPolicyRefNo As String = "Policy_Ref_No"
    Public Const ACXMLARetPolicyEffStartDate As String = "Policy_Eff_Start_Date"
    Public Const ACXMLARetPolicySurname As String = "Policy_Surname"
    Public Const ACXMLARetPolicyPostcode As String = "Policy_Postcode"
    Public Const ACXMLARetPolicyRegno As String = "Policy_Regno"

    ' ***************************************************************** '
    ' Name: FormatActionXML
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function FormatActionXMLQuotePolicy(ByVal v_lAction As Integer, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef r_sActionXML As String, Optional ByVal v_lInsFileSearchType As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_sPolicyTypeCode As String = "", Optional ByVal v_sInsuranceFileRef As String = "", Optional ByVal v_sPartyTypeCode As String = "", Optional ByRef r_sShortName As String = "", Optional ByVal v_sResolvedName As String = "", Optional ByRef r_sUserID As String = "", Optional ByRef r_sPassword As String = "", Optional ByVal v_lLeadAgentCnt As Integer = 0, Optional ByVal v_sForename As String = "", Optional ByVal v_sSurname As String = "", Optional ByVal v_sDateOfBirth As String = "", Optional ByVal v_sEmailAddress As String = "", Optional ByVal v_sCurrentRenewalDate As String = "", Optional ByVal v_sAddress1 As String = "", Optional ByVal v_sAddress2 As String = "", Optional ByVal v_sAddress3 As String = "", Optional ByVal v_sAddress4 As String = "", Optional ByVal v_sPostcode As String = "", Optional ByVal v_sTitle As String = "", Optional ByVal v_sMaritalStatusCode As String = "", Optional ByVal v_sGenderCode As String = "", Optional ByVal v_sInitials As String = "", Optional ByVal v_sTelephoneNumber As String = "", Optional ByVal v_sInsFolderDescription As String = "", Optional ByVal v_dCoverStartDate As String = "", Optional ByRef r_vAdditionalDataArray As Object = Nothing, Optional ByVal v_lProductID As Integer = 0) As Integer
        '**** Added By: AAB  -  Added On:  16-Sep-2002 17:16 ****
        '**** Added the optional v_lProductID parameter to support Agents On Line


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
            oActionChildElem = oAction.CreateElement(ACXMLActionQuotePolicy)

            ' Set the Action Value
            'oActionChildElem.Text = v_lAction
            oActionChildElem.SetAttribute(ACXMLAttribGISAction, v_lAction)

            oActionChildElem.SetAttribute(ACXMLAttribDataModelCode, v_sDataModelCode)
            oActionChildElem.SetAttribute(ACXMLAttribBusinessTypeCode, v_sBusinessTypeCode)

            oActionChildElem.SetAttribute(ACXMLAttribPartyPartyCnt, v_lPartyCnt)
            oActionChildElem.SetAttribute(ACXMLAttribInsuranceFileCnt, v_lInsuranceFileCnt)
            oActionChildElem.SetAttribute(ACXMLAttribInsFileSearchType, v_lInsFileSearchType)
            oActionChildElem.SetAttribute(ACXMLAttribPolicyTypeCode, v_sPolicyTypeCode)
            ' RFC120700  - Get Policy Versions Via Insurance Ref
            oActionChildElem.SetAttribute(ACXMLAttribInsuranceFileRef, v_sInsuranceFileRef)

            ' RJG 22/11/2000 - Additional data elements for FindParty, AddParty and GetQuotesForParty
            oActionChildElem.SetAttribute(ACXMLAttribPartyTypeCode, v_sPartyTypeCode)
            oActionChildElem.SetAttribute(ACXMLAttribPartyShortname, r_sShortName)
            oActionChildElem.SetAttribute(ACXMLAttribPartyResolvedName, v_sResolvedName)
            oActionChildElem.SetAttribute(ACXMLAttribPartyUserID, r_sUserID)
            oActionChildElem.SetAttribute(ACXMLAttribPartyLeadAgentCnt, v_lLeadAgentCnt)
            oActionChildElem.SetAttribute(ACXMLAttribPartyForename, v_sForename)
            oActionChildElem.SetAttribute(ACXMLAttribPartySurname, v_sSurname)
            oActionChildElem.SetAttribute(ACXMLAttribPartyDateOfBirth, v_sDateOfBirth)
            oActionChildElem.SetAttribute(ACXMLAttribPartyEMailAddress, v_sEmailAddress)
            oActionChildElem.SetAttribute(ACXMLAttribPartyCurrentRenewalDate, v_sCurrentRenewalDate)
            oActionChildElem.SetAttribute(ACXMLAttribPartyAddress1, v_sAddress1)
            oActionChildElem.SetAttribute(ACXMLAttribPartyAddress2, v_sAddress2)
            oActionChildElem.SetAttribute(ACXMLAttribPartyAddress3, v_sAddress3)
            oActionChildElem.SetAttribute(ACXMLAttribPartyAddress4, v_sAddress4)
            oActionChildElem.SetAttribute(ACXMLAttribPartyPostCode, v_sPostcode)
            oActionChildElem.SetAttribute(ACXMLAttribPartyTitle, v_sTitle)
            oActionChildElem.SetAttribute(ACXMLAttribPartyMaritalStatusCode, v_sMaritalStatusCode)
            oActionChildElem.SetAttribute(ACXMLAttribPartyGenderCode, v_sGenderCode)
            oActionChildElem.SetAttribute(ACXMLAttribPartyInitials, v_sInitials)
            oActionChildElem.SetAttribute(ACXMLAttribPartyTelephoneNumber, v_sTelephoneNumber)

            'RJG 04/12/2000 - Elements for FindQuote method
            oActionChildElem.SetAttribute(ACXMLAttribInsFolderDescription, v_sInsFolderDescription)
            oActionChildElem.SetAttribute(ACXMLAttribCoverStartDate, v_dCoverStartDate)

            '**** START CHANGES - Changed By: AAB  - Changed On: 05-Sep-2002 09:04   ****
            '**** Elements for GetRisksForProduct method
            oActionChildElem.SetAttribute(ACXMLAttribProductID, v_lProductID)
            '****   END CHANGES - Changed By: AAB  - Changed On: 05-Sep-2002 09:04   ****

            ' Add the Additional Data Items

            lReturn = CType(FormatAdditionalDataXML(r_oDocument:=oAction, r_oParentElem:=oActionChildElem, v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatActionXMLQuotePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatActionXMLQuotePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnFormatActionXMLQuotePolicy
    '
    ' Description:
    ' ***************************************************************** '

    Public Function UnFormatActionXMLQuotePolicy(ByRef r_lAction As Integer, ByRef r_sDataModelCode As String, ByRef r_sBusinessTypeCode As String, ByVal v_sActionXML As String, Optional ByRef r_lInsFileSearchType As Integer = 0, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_lInsuranceFileCnt As Integer = 0, Optional ByRef r_sPolicyTypeCode As String = "", Optional ByRef r_sInsuranceFileRef As String = "", Optional ByRef r_sPartyTypeCode As String = "", Optional ByRef r_sShortName As String = "", Optional ByRef r_sResolvedName As String = "", Optional ByRef r_sUserID As String = "", Optional ByRef r_lLeadAgentCnt As Integer = 0, Optional ByRef r_sForename As String = "", Optional ByRef r_sSurname As String = "", Optional ByRef r_sDateOfBirth As String = "", Optional ByRef r_sEmailAddress As String = "", Optional ByRef r_sCurrentRenewalDate As String = "", Optional ByRef r_sAddress1 As String = "", Optional ByRef r_sAddress2 As String = "", Optional ByRef r_sAddress3 As String = "", Optional ByRef r_sAddress4 As String = "", Optional ByRef r_sPostcode As String = "", Optional ByRef r_sTitle As String = "", Optional ByRef r_sMaritalStatusCode As String = "", Optional ByRef r_sGenderCode As String = "", Optional ByRef r_sInitials As String = "", Optional ByRef r_sTelephoneNumber As String = "", Optional ByRef r_sInsFolderDescription As String = "", Optional ByRef r_dCoverStartDate As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing, Optional ByRef r_lProductID As Integer = 0, Optional ByRef r_vInsuranceFolderCnt As Object = 0) As Integer
        '**** Added By: AAB  -  Added On:  05-Sep-2002 09:32 ****
        '**** Added an optional variable r_lProductID to support Agents On Line

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
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & v_sActionXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionXMLQuotePolicy")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oActionElem = oAction.DocumentElement
            oActionElem = oAction.DocumentElement.FirstChild ' CL240500

            ' Get the Action Value
            'r_lAction = oActionElem.Text

            r_lAction = CInt(oActionElem.GetAttribute(ACXMLAttribGISAction))

            r_sDataModelCode = CStr(oActionElem.GetAttribute(ACXMLAttribDataModelCode))

            r_sBusinessTypeCode = CStr(oActionElem.GetAttribute(ACXMLAttribBusinessTypeCode))


            r_lInsFileSearchType = CInt(oActionElem.GetAttribute(ACXMLAttribInsFileSearchType))

            r_lPartyCnt = CInt(oActionElem.GetAttribute(ACXMLAttribPartyPartyCnt))

            r_lInsuranceFileCnt = CInt(oActionElem.GetAttribute(ACXMLAttribInsuranceFileCnt))

            r_sPolicyTypeCode = CStr(oActionElem.GetAttribute(ACXMLAttribPolicyTypeCode))
            ' RFC120700  - Get Policy Versions Via Insurance Ref

            r_sInsuranceFileRef = CStr(oActionElem.GetAttribute(ACXMLAttribInsuranceFileRef))

            'RJG 22/11/2000 - Data items for FindParty, AddParty and GetQuotesForParty method

            r_sPartyTypeCode = CStr(oActionElem.GetAttribute(ACXMLAttribPartyTypeCode))

            r_sShortName = CStr(oActionElem.GetAttribute(ACXMLAttribPartyShortname))

            r_sResolvedName = CStr(oActionElem.GetAttribute(ACXMLAttribPartyResolvedName))

            r_sUserID = CStr(oActionElem.GetAttribute(ACXMLAttribPartyUserID))

            r_lLeadAgentCnt = CInt(oActionElem.GetAttribute(ACXMLAttribPartyLeadAgentCnt))

            r_sForename = CStr(oActionElem.GetAttribute(ACXMLAttribPartyForename))

            r_sSurname = CStr(oActionElem.GetAttribute(ACXMLAttribPartySurname))

            r_sDateOfBirth = CStr(oActionElem.GetAttribute(ACXMLAttribPartyDateOfBirth))

            r_sEmailAddress = CStr(oActionElem.GetAttribute(ACXMLAttribPartyEMailAddress))

            r_sCurrentRenewalDate = CStr(oActionElem.GetAttribute(ACXMLAttribPartyCurrentRenewalDate))

            r_sAddress1 = CStr(oActionElem.GetAttribute(ACXMLAttribPartyAddress1))

            r_sAddress2 = CStr(oActionElem.GetAttribute(ACXMLAttribPartyAddress2))

            r_sAddress3 = CStr(oActionElem.GetAttribute(ACXMLAttribPartyAddress3))

            r_sAddress4 = CStr(oActionElem.GetAttribute(ACXMLAttribPartyAddress4))

            r_sPostcode = CStr(oActionElem.GetAttribute(ACXMLAttribPartyPostCode))

            r_sTitle = CStr(oActionElem.GetAttribute(ACXMLAttribPartyTitle))

            r_sMaritalStatusCode = CStr(oActionElem.GetAttribute(ACXMLAttribPartyMaritalStatusCode))

            r_sGenderCode = CStr(oActionElem.GetAttribute(ACXMLAttribPartyGenderCode))

            r_sInitials = CStr(oActionElem.GetAttribute(ACXMLAttribPartyInitials))

            r_sTelephoneNumber = CStr(oActionElem.GetAttribute(ACXMLAttribPartyTelephoneNumber))


            r_sInsFolderDescription = CStr(oActionElem.GetAttribute(ACXMLAttribInsFolderDescription))


            r_dCoverStartDate = oActionElem.GetAttribute(ACXMLAttribCoverStartDate)

            '**** Added By: AAB  -  Added On:  05-Sep-2002 09:33 ****
            '**** Added to Support Agents On Line

            r_lProductID = CInt(oActionElem.GetAttribute(ACXMLAttribProductID))

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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatActionXMLQuotePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionXMLQuotePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: FormatActionReturnXMLQuotePolicy
    '
    ' Description:
    '   RJG 22/11/2000 - Added PartyArray and QuoteArray as optional params
    '
    ' ***************************************************************** '
    Public Function FormatActionReturnXMLQuotePolicy(ByVal v_lReturnValue As Integer, ByRef r_sActionReturnXML As String, Optional ByVal v_vQuotePolicyArray(,) As Object = Nothing, Optional ByVal v_vPolicyVersionArray(,) As Object = Nothing, Optional ByVal v_vPartyArray(,) As Object = Nothing, Optional ByVal v_vQuoteArray(,) As Object = Nothing, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_sSurname As String = "", Optional ByVal v_sForename As String = "", Optional ByVal v_sPartyTypeCode As String = "", Optional ByVal v_sAddress1 As String = "", Optional ByVal v_sAddress2 As String = "", Optional ByVal v_sAddress3 As String = "", Optional ByVal v_sAddress4 As String = "", Optional ByVal v_sPostcode As String = "", Optional ByVal v_sDateOfBirth As String = "", Optional ByVal v_sEMail As String = "", Optional ByVal r_sUserID As String = "", Optional ByVal v_sPassword As String = "", Optional ByVal v_sShortname As String = "", Optional ByVal v_sResolvedName As String = "", Optional ByVal v_sMothersMaidenName As String = "", Optional ByVal v_sTPUserCode As String = "", Optional ByVal v_sTPIntroducer As String = "", Optional ByVal v_sAQuestion As String = "", Optional ByVal v_sTheAnswer As String = "", Optional ByVal v_sMemorableDate As String = "", Optional ByVal v_sCurrInsRenewalDate As String = "", Optional ByVal v_sTitle As String = "", Optional ByVal v_sMaritalStatusCode As String = "", Optional ByVal v_sGenderCode As String = "", Optional ByVal v_sInitials As String = "", Optional ByVal v_sTelephoneNumber As String = "", Optional ByVal v_vFindQuoteArray(,) As Object = Nothing, Optional ByVal v_vFindPolicyArray(,) As Object = Nothing, Optional ByVal v_vProductArray(,) As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oActionReturn As XmlDocument
        Dim oActionReturnElem, oInsFileElem, oInsFileDetailElem As XmlElement

        Dim oElem, oElemChild As XmlElement


        '**** Added By: AAB  -  Added On:  04-Sep-2002 14:53 ****
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

            ' Do we have a Quote Policy Arrray
            If Informations.IsArray(v_vQuotePolicyArray) Then

                ' Add Each Match
                For lRow As Integer = v_vQuotePolicyArray.GetLowerBound(1) To v_vQuotePolicyArray.GetUpperBound(1)

                    oInsFileElem = oActionReturn.CreateElement(ACXMLARetInsuranceFile)

                    oActionReturnElem.AppendChild(oInsFileElem)


                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetInsFileType)

                    oInsFileDetailElem.InnerText = CStr(v_vQuotePolicyArray(0, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetInsFileCnt)

                    oInsFileDetailElem.InnerText = CStr(v_vQuotePolicyArray(1, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyName)

                    oInsFileDetailElem.InnerText = CStr(v_vQuotePolicyArray(2, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetQuoteOrPolNo)

                    oInsFileDetailElem.InnerText = CStr(v_vQuotePolicyArray(3, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetDescription)

                    oInsFileDetailElem.InnerText = CStr(v_vQuotePolicyArray(4, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetDate)

                    oInsFileDetailElem.InnerText = CStr(v_vQuotePolicyArray(5, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                Next lRow

                oInsFileElem = Nothing
                oInsFileDetailElem = Nothing

            End If

            ' Do we have a Policy Version Arrray
            If Informations.IsArray(v_vPolicyVersionArray) Then

                ' Add Each Match
                For lRow As Integer = v_vPolicyVersionArray.GetLowerBound(1) To v_vPolicyVersionArray.GetUpperBound(1)

                    oInsFileElem = oActionReturn.CreateElement(ACXMLARetPolicyVersion)

                    oActionReturnElem.AppendChild(oInsFileElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetInsFileType)

                    oInsFileDetailElem.InnerText = CStr(v_vPolicyVersionArray(0, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetInsFileCnt)

                    oInsFileDetailElem.InnerText = CStr(v_vPolicyVersionArray(1, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyName)

                    oInsFileDetailElem.InnerText = CStr(v_vPolicyVersionArray(2, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetQuoteOrPolNo)

                    oInsFileDetailElem.InnerText = CStr(v_vPolicyVersionArray(3, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetDescription)

                    oInsFileDetailElem.InnerText = CStr(v_vPolicyVersionArray(4, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetDate)

                    oInsFileDetailElem.InnerText = CStr(v_vPolicyVersionArray(5, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetInsFileVersion)

                    oInsFileDetailElem.InnerText = CStr(v_vPolicyVersionArray(6, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetExpiryDate)

                    oInsFileDetailElem.InnerText = CStr(v_vPolicyVersionArray(7, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                Next lRow

                oInsFileElem = Nothing
                oInsFileDetailElem = Nothing

            End If

            ' RJG 22/11/2000 - Do we have a QuoteArray
            If Informations.IsArray(v_vQuoteArray) Then

                ' Add Each Match
                For lRow As Integer = v_vQuoteArray.GetLowerBound(1) To v_vQuoteArray.GetUpperBound(1)

                    oInsFileElem = oActionReturn.CreateElement(ACXMLARetInsuranceFile)

                    oActionReturnElem.AppendChild(oInsFileElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetInsFileType)

                    oInsFileDetailElem.InnerText = CStr(v_vQuoteArray(0, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetInsFileCnt)

                    oInsFileDetailElem.InnerText = CStr(v_vQuoteArray(1, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyName)

                    oInsFileDetailElem.InnerText = CStr(v_vQuoteArray(2, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetQuoteOrPolNo)

                    oInsFileDetailElem.InnerText = CStr(v_vQuoteArray(3, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetDescription)

                    oInsFileDetailElem.InnerText = CStr(v_vQuoteArray(4, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetDate)

                    oInsFileDetailElem.InnerText = CStr(v_vQuoteArray(5, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                Next lRow

                oInsFileElem = Nothing
                oInsFileDetailElem = Nothing

            End If

            'RJG 05/01/2001 - FindQuote array returns slightly different info to the quotearray above
            If Informations.IsArray(v_vFindQuoteArray) Then

                ' Add Each Match
                For lRow As Integer = v_vFindQuoteArray.GetLowerBound(1) To v_vFindQuoteArray.GetUpperBound(1)

                    oInsFileElem = oActionReturn.CreateElement(ACXMLARetInsuranceFile)

                    oActionReturnElem.AppendChild(oInsFileElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetInsFileCnt)

                    oInsFileDetailElem.InnerText = CStr(v_vFindQuoteArray(0, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetQuoteOrPolNo)

                    oInsFileDetailElem.InnerText = CStr(v_vFindQuoteArray(1, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyName)

                    oInsFileDetailElem.InnerText = CStr(v_vFindQuoteArray(2, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetDate)

                    oInsFileDetailElem.InnerText = CStr(v_vFindQuoteArray(3, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetDescription)

                    oInsFileDetailElem.InnerText = CStr(v_vFindQuoteArray(4, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyCnt)

                    oInsFileDetailElem.InnerText = CStr(v_vFindQuoteArray(5, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetInsFolderCnt)

                    oInsFileDetailElem.InnerText = CStr(v_vFindQuoteArray(6, lRow)) & ""

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                Next lRow

                oInsFileElem = Nothing
                oInsFileDetailElem = Nothing

            End If

            ' RJG 22/11/2000 - Do we have a PartyArray
            If Informations.IsArray(v_vPartyArray) Then

                ' Add Each Match
                For lRow As Integer = v_vPartyArray.GetLowerBound(1) To v_vPartyArray.GetUpperBound(1)

                    oInsFileElem = oActionReturn.CreateElement(ACXMLARetPartyDetail)

                    oActionReturnElem.AppendChild(oInsFileElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyCnt)

                    oInsFileDetailElem.InnerText = CStr(v_vPartyArray(0, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyShortname)

                    oInsFileDetailElem.InnerText = CStr(v_vPartyArray(1, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyResolvedName)

                    oInsFileDetailElem.InnerText = CStr(v_vPartyArray(2, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyAddress1)

                    oInsFileDetailElem.InnerText = CStr(v_vPartyArray(3, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyPostcode)

                    oInsFileDetailElem.InnerText = CStr(v_vPartyArray(4, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyTelephoneNumber)

                    oInsFileDetailElem.InnerText = CStr(v_vPartyArray(5, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyDateOfBirth)

                    oInsFileDetailElem.InnerText = CStr(v_vPartyArray(6, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    '**** START CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 11:14   ****
                    '**** Added the Agent Cnt  to the return result array
                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPartyAgentCnt)

                    oInsFileDetailElem.InnerText = CStr(v_vPartyArray(7, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)
                    '****   END CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 11:14   ****


                Next lRow

                oInsFileElem = Nothing
                oInsFileDetailElem = Nothing

            End If

            ' Do we have a Product Array
            If Informations.IsArray(v_vProductArray) Then

                ' Add Each Match
                For lRow As Integer = v_vProductArray.GetLowerBound(1) To v_vProductArray.GetUpperBound(1)

                    oInsFileElem = oActionReturn.CreateElement(ACXMLARetProduct)

                    oActionReturnElem.AppendChild(oInsFileElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetProductID)

                    oInsFileDetailElem.InnerText = CStr(v_vProductArray(0, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetProductCode)

                    oInsFileDetailElem.InnerText = CStr(v_vProductArray(1, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetProductDescription)

                    oInsFileDetailElem.InnerText = CStr(v_vProductArray(2, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetProductSchemeAgencyRef)

                    oInsFileDetailElem.InnerText = CStr(v_vProductArray(3, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetProductBlockNo)

                    oInsFileDetailElem.InnerText = CStr(v_vProductArray(4, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                Next lRow

                oInsFileElem = Nothing
                oInsFileDetailElem = Nothing

            End If


            If Informations.IsArray(v_vFindPolicyArray) Then

                ' Add Each Match
                For lRow As Integer = v_vFindPolicyArray.GetLowerBound(1) To v_vFindPolicyArray.GetUpperBound(1)

                    oInsFileElem = oActionReturn.CreateElement(ACXMLARetPolicyDetail)

                    oActionReturnElem.AppendChild(oInsFileElem)

                    ' JP 18/05/2001 Append empty string to front of PolicyNo to prevent problems with nulls

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPolicyNo)

                    oInsFileDetailElem.InnerText = "" & CStr(v_vFindPolicyArray(0, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPolicyRefNo)

                    oInsFileDetailElem.InnerText = CStr(v_vFindPolicyArray(1, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPolicyEffStartDate)

                    oInsFileDetailElem.InnerText = CStr(v_vFindPolicyArray(2, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPolicySurname)

                    oInsFileDetailElem.InnerText = CStr(v_vFindPolicyArray(3, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPolicyPostcode)

                    oInsFileDetailElem.InnerText = CStr(v_vFindPolicyArray(4, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                    oInsFileDetailElem = oActionReturn.CreateElement(ACXMLARetPolicyRegno)

                    oInsFileDetailElem.InnerText = CStr(v_vFindPolicyArray(5, lRow))

                    oInsFileElem.AppendChild(oInsFileDetailElem)

                Next lRow

                oInsFileElem = Nothing
                oInsFileDetailElem = Nothing

            End If



            oActionReturnElem.SetAttribute(ACXMLARetPartyCnt, v_lPartyCnt)

            'RJG 28/11/2000 - Return values for the GetParty method
            oActionReturnElem.SetAttribute(ACXMLARetPartySurname, v_sSurname)
            oActionReturnElem.SetAttribute(ACXMLARetPartyForename, v_sForename)
            oActionReturnElem.SetAttribute(ACXMLARetPartyTypeCode, v_sPartyTypeCode)
            oActionReturnElem.SetAttribute(ACXMLARetPartyAddress1, v_sAddress1)
            oActionReturnElem.SetAttribute(ACXMLARetPartyAddress2, v_sAddress2)
            oActionReturnElem.SetAttribute(ACXMLARetPartyAddress3, v_sAddress3)
            oActionReturnElem.SetAttribute(ACXMLARetPartyAddress4, v_sAddress4)
            oActionReturnElem.SetAttribute(ACXMLARetPartyPostcode, v_sPostcode)
            oActionReturnElem.SetAttribute(ACXMLARetPartyDateOfBirth, v_sDateOfBirth)
            oActionReturnElem.SetAttribute(ACXMLARetPartyEMail, v_sEMail)
            oActionReturnElem.SetAttribute(ACXMLARetPartyUserID, r_sUserID)
            oActionReturnElem.SetAttribute(ACXMLARetPartyPassword, v_sPassword)
            oActionReturnElem.SetAttribute(ACXMLARetPartyShortname, v_sShortname)
            oActionReturnElem.SetAttribute(ACXMLARetPartyResolvedName, v_sResolvedName)
            oActionReturnElem.SetAttribute(ACXMLARetPartyMothersMaidenName, v_sMothersMaidenName)
            oActionReturnElem.SetAttribute(ACXMLARetPartyTPUserCode, v_sTPUserCode)
            oActionReturnElem.SetAttribute(ACXMLARetPartyTPIntroducer, v_sTPIntroducer)
            oActionReturnElem.SetAttribute(ACXMLARetPartyAQuestion, v_sAQuestion)
            oActionReturnElem.SetAttribute(ACXMLARetPartyTheAnswer, v_sTheAnswer)
            oActionReturnElem.SetAttribute(ACXMLARetPartyMemorableDate, v_sMemorableDate)
            oActionReturnElem.SetAttribute(ACXMLARetPartyCurrInsRenewalDate, v_sCurrInsRenewalDate)
            oActionReturnElem.SetAttribute(ACXMLARetPartyTitle, v_sTitle)
            oActionReturnElem.SetAttribute(ACXMLARetPartyMaritalStatusCode, v_sMaritalStatusCode)
            oActionReturnElem.SetAttribute(ACXMLARetPartyGenderCode, v_sGenderCode)
            oActionReturnElem.SetAttribute(ACXMLARetPartyInitials, v_sInitials)
            oActionReturnElem.SetAttribute(ACXMLARetPartyTelephoneNumber, v_sTelephoneNumber)

            '**** START CHANGES - Changed By: AAB  - Changed On: 04-Sep-2002 14:53   ****
            '**** To get back the additional data array
            ' Add the Additional Data Items

            lReturn = CType(FormatAdditionalDataXML(r_oDocument:=oActionReturn, r_oParentElem:=oActionReturnElem, v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
            '****   END CHANGES - Changed By: AAB  - Changed On: 04-Sep-2002 14:53   ****

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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatActionReturnXMLQuotePolicyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatActionReturnXMLQuotePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnFormatActionReturnXMLQuotePolicy
    '
    ' Description:
    '  RJG 22/11/2000 - Added PartyArray
    '                   and QuoteArray (Same structure as QuotePolicyArray)
    '
    ' ***************************************************************** '
    Public Function UnFormatActionReturnXMLQuotePolicy(ByVal v_sActionReturnXML As String, ByRef r_lReturnValue As Integer, Optional ByRef r_vQuotePolicyArray As Object = Nothing, Optional ByRef r_vPolicyVersionArray As Object = Nothing, Optional ByRef r_vPartyArray As Object = Nothing, Optional ByRef r_vQuoteArray As Object = Nothing, Optional ByRef r_lPartyCnt As Object = Nothing, Optional ByRef r_sSurname As Object = Nothing, Optional ByRef r_sForename As Object = Nothing, Optional ByRef r_sPartyTypeCode As Object = Nothing, Optional ByRef r_sAddress1 As Object = Nothing, Optional ByRef r_sAddress2 As Object = Nothing, Optional ByRef r_sAddress3 As Object = Nothing, Optional ByRef r_sAddress4 As Object = Nothing, Optional ByRef r_sPostcode As Object = Nothing, Optional ByRef r_sDateOfBirth As Object = Nothing, Optional ByRef r_sEMail As Object = Nothing, Optional ByRef r_sUserID As Object = Nothing, Optional ByRef r_sPassword As Object = Nothing, Optional ByRef r_sShortName As Object = Nothing, Optional ByRef r_sResolvedName As Object = Nothing, Optional ByRef r_sMothersMaidenName As Object = Nothing, Optional ByRef r_sTPUserCode As Object = Nothing, Optional ByRef r_sTPIntroducer As Object = Nothing, Optional ByRef r_sAQuestion As Object = Nothing, Optional ByRef r_sTheAnswer As Object = Nothing, Optional ByRef r_sMemorableDate As Object = Nothing, Optional ByRef r_sCurrInsRenewalDate As Object = Nothing, Optional ByRef r_sTitle As Object = Nothing, Optional ByRef r_sMaritalStatusCode As Object = Nothing, Optional ByRef r_sGenderCode As Object = Nothing, Optional ByRef r_sInitials As Object = Nothing, Optional ByRef r_sTelephoneNumber As Object = Nothing, Optional ByRef r_vFindQuoteArray As Object = Nothing, Optional ByRef r_vFindPolicyArray As Object = Nothing, Optional ByRef r_vProductArray As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing, Optional ByRef r_vRiskArray As Object = Nothing, Optional ByRef r_vInsuranceFolderCnt As Object = Nothing) As Integer
        '**** Added By: AAB  -  Added On:  05-Sep-2002 09:56 ****
        '**** Added r_vAdditionalDataArray and r_vRiskArray optional parameters to support Agents On Line

        Dim result As Integer = 0
        Dim oActionReturnElem As XmlElement
        Dim oActionReturn As XmlDocument
        Dim bLoaded As Boolean
        Dim oInsFiles As XmlNodeList
        Dim oInsFile As XmlElement
        Dim oInsFileElem As XmlNodeList
        Dim lNumMatches As Integer
        '**** Added By: AAB  -  Added On:  04-Sep-2002 14:53 ****
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Document
            oActionReturn = New XmlDocument()

            ' Load the Action XML

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
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & v_sActionReturnXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionReturnXMLQuotePolicy")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oActionReturnElem = oActionReturn.DocumentElement

            ' Get the Attributes

            r_lReturnValue = CInt(oActionReturnElem.GetAttribute(ACXMLAttribReturnValue))

            ' Do we have the QuotePolicyArray

            If Not Informations.IsNothing(r_vQuotePolicyArray) Then

                ' Yes, so initialise it


                r_vQuotePolicyArray = Nothing

                ' Are there any InsuranceFiles returned
                oInsFiles = oActionReturn.GetElementsByTagName(ACXMLARetInsuranceFile)
                If Not (oInsFiles Is Nothing) Then

                    ' How Many Matches are there
                    lNumMatches = oInsFiles.Count

                    ' If there are matches then
                    If lNumMatches > 0 Then

                        ReDim r_vQuotePolicyArray(5, lNumMatches - 1)

                        ' Add them to the Array
                        For lRow As Integer = 0 To lNumMatches - 1

                            ' Get a Reference to a Match
                            oInsFile = oInsFiles.Item(lRow)

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetInsFileType)

                            r_vQuotePolicyArray(0, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetInsFileCnt)

                            r_vQuotePolicyArray(1, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyName)

                            r_vQuotePolicyArray(2, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetQuoteOrPolNo)

                            r_vQuotePolicyArray(3, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetDescription)

                            r_vQuotePolicyArray(4, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetDate)

                            r_vQuotePolicyArray(5, lRow) = oInsFileElem.Item(0).InnerText

                        Next lRow

                    End If

                End If

            End If

            ' Do we have the PolicyVersionArray

            If Not Informations.IsNothing(r_vPolicyVersionArray) Then

                ' Yes so initialise it


                r_vPolicyVersionArray = Nothing

                ' Are there any Policy Versions returned
                oInsFiles = oActionReturn.GetElementsByTagName(ACXMLARetPolicyVersion)
                If Not (oInsFiles Is Nothing) Then

                    ' How Many Matches are there
                    lNumMatches = oInsFiles.Count

                    ' If there are matches then
                    If lNumMatches > 0 Then

                        ReDim r_vPolicyVersionArray(7, lNumMatches - 1)

                        ' Add them to the Array
                        For lRow As Integer = 0 To lNumMatches - 1

                            ' Get a Reference to a Match
                            oInsFile = oInsFiles.Item(lRow)

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetInsFileType)

                            r_vPolicyVersionArray(0, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetInsFileCnt)

                            r_vPolicyVersionArray(1, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyName)

                            r_vPolicyVersionArray(2, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetQuoteOrPolNo)

                            r_vPolicyVersionArray(3, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetDescription)

                            r_vPolicyVersionArray(4, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetDate)

                            r_vPolicyVersionArray(5, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetInsFileVersion)

                            r_vPolicyVersionArray(6, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetExpiryDate)

                            r_vPolicyVersionArray(7, lRow) = oInsFileElem.Item(0).InnerText

                        Next lRow

                    End If

                End If

            End If

            'RJG 22/11/2000 Do we have the QuoteArray

            If Not Informations.IsNothing(r_vQuoteArray) Then

                'RJG 22/11/2000 - Yes, so initialise it


                r_vQuoteArray = Nothing

                'RJG 22/11/2000 - Are there any rows returned
                oInsFiles = oActionReturn.GetElementsByTagName(ACXMLARetInsuranceFile)
                If Not (oInsFiles Is Nothing) Then

                    'RJG 22/11/2000 - How Many Matches are there
                    lNumMatches = oInsFiles.Count

                    'RJG 22/11/2000 - If there are matches then
                    If lNumMatches > 0 Then

                        ReDim r_vQuoteArray(5, lNumMatches - 1)

                        ' Add them to the Array
                        For lRow As Integer = 0 To lNumMatches - 1

                            ' Get a Reference to a Match
                            oInsFile = oInsFiles.Item(lRow)

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetInsFileType)

                            r_vQuoteArray(0, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetInsFileCnt)

                            r_vQuoteArray(1, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyName)

                            r_vQuoteArray(2, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetQuoteOrPolNo)

                            r_vQuoteArray(3, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetDescription)

                            r_vQuoteArray(4, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetDate)

                            r_vQuoteArray(5, lRow) = oInsFileElem.Item(0).InnerText

                        Next lRow

                    End If

                End If

            End If

            'RJG 05/01/2001 Do we have the QuoteArray

            If Not Informations.IsNothing(r_vFindQuoteArray) Then

                'RJG 05/01/2001 - Yes, so initialise it


                r_vFindQuoteArray = Nothing

                'RJG 05/01/2001 - Are there any rows returned
                oInsFiles = oActionReturn.GetElementsByTagName(ACXMLARetInsuranceFile)
                If Not (oInsFiles Is Nothing) Then

                    'RJG 05/01/2001 - How Many Matches are there
                    lNumMatches = oInsFiles.Count

                    'RJG 05/01/2001 - If there are matches then
                    If lNumMatches > 0 Then

                        ReDim r_vFindQuoteArray(6, lNumMatches - 1)

                        ' Add them to the Array
                        For lRow As Integer = 0 To lNumMatches - 1

                            ' Get a Reference to a Match
                            oInsFile = oInsFiles.Item(lRow)

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetInsFileCnt)

                            r_vFindQuoteArray(0, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetQuoteOrPolNo)

                            r_vFindQuoteArray(1, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyName)

                            r_vFindQuoteArray(2, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetDate)

                            r_vFindQuoteArray(3, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetDescription)

                            r_vFindQuoteArray(4, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyCnt)

                            r_vFindQuoteArray(5, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetInsFolderCnt)

                            r_vFindQuoteArray(6, lRow) = oInsFileElem.Item(0).InnerText


                        Next lRow

                    End If

                End If

            End If

            'RJG 22/11/2000 Do we have the QuoteArray

            If Not Informations.IsNothing(r_vPartyArray) Then

                'RJG 22/11/2000 - Yes, so initialise it


                r_vPartyArray = Nothing

                'RJG 22/11/2000 - Are there any rows returned
                oInsFiles = oActionReturn.GetElementsByTagName(ACXMLARetPartyDetail)
                If Not (oInsFiles Is Nothing) Then

                    'RJG 22/11/2000 - How Many Matches are there
                    lNumMatches = oInsFiles.Count

                    'RJG 22/11/2000 - If there are matches then
                    If lNumMatches > 0 Then

                        '**** START CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 09:36   ****
                        '**** change from 6 to 7 to added the agentcnt, Party Type
                        ReDim r_vPartyArray(7, lNumMatches - 1)
                        '****   END CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 09:36   ****


                        ' Add them to the Array
                        For lRow As Integer = 0 To lNumMatches - 1

                            ' Get a Reference to a Match
                            oInsFile = oInsFiles.Item(lRow)

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyCnt)

                            r_vPartyArray(0, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyShortname)

                            r_vPartyArray(1, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyResolvedName)

                            r_vPartyArray(2, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyAddress1)

                            r_vPartyArray(3, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyPostcode)

                            r_vPartyArray(4, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyTelephoneNumber)

                            r_vPartyArray(5, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyDateOfBirth)

                            r_vPartyArray(6, lRow) = oInsFileElem.Item(0).InnerText

                            '**** START CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 09:36   ****
                            '**** Added the 7th element to the Array.
                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPartyAgentCnt)

                            r_vPartyArray(7, lRow) = oInsFileElem.Item(0).InnerText
                            '****   END CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 09:36   ****

                        Next lRow

                    End If

                End If

            End If


            'CL240101

            If Not Informations.IsNothing(r_vFindPolicyArray) Then

                'RJG 22/11/2000 - Yes, so initialise it


                r_vFindPolicyArray = Nothing

                'RJG 22/11/2000 - Are there any rows returned
                oInsFiles = oActionReturn.GetElementsByTagName(ACXMLARetPolicyDetail)
                If Not (oInsFiles Is Nothing) Then

                    'RJG 22/11/2000 - How Many Matches are there
                    lNumMatches = oInsFiles.Count

                    'RJG 22/11/2000 - If there are matches then
                    If lNumMatches > 0 Then

                        ReDim r_vFindPolicyArray(5, lNumMatches - 1)

                        ' Add them to the Array
                        For lRow As Integer = 0 To lNumMatches - 1

                            ' Get a Reference to a Match
                            oInsFile = oInsFiles.Item(lRow)

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPolicyNo)

                            r_vFindPolicyArray(0, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPolicyRefNo)

                            r_vFindPolicyArray(1, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPolicyEffStartDate)

                            r_vFindPolicyArray(2, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPolicySurname)

                            r_vFindPolicyArray(3, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPolicyPostcode)

                            r_vFindPolicyArray(4, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetPolicyRegno)

                            r_vFindPolicyArray(5, lRow) = oInsFileElem.Item(0).InnerText

                        Next lRow

                    End If

                End If

            End If


            ' Do we have the ProductArray

            If Not Informations.IsNothing(r_vProductArray) Then

                ' Yes, so initialise it


                r_vProductArray = Nothing

                ' Are there any InsuranceFiles returned
                oInsFiles = oActionReturn.GetElementsByTagName(ACXMLARetProduct)
                If Not (oInsFiles Is Nothing) Then

                    ' How Many Matches are there
                    lNumMatches = oInsFiles.Count

                    ' If there are matches then
                    If lNumMatches > 0 Then

                        ReDim r_vProductArray(4, lNumMatches - 1)

                        ' Add them to the Array
                        For lRow As Integer = 0 To lNumMatches - 1

                            ' Get a Reference to a Match
                            oInsFile = oInsFiles.Item(lRow)

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetProductID)

                            r_vProductArray(0, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetProductCode)

                            r_vProductArray(1, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetProductDescription)

                            r_vProductArray(2, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetProductSchemeAgencyRef)

                            r_vProductArray(3, lRow) = oInsFileElem.Item(0).InnerText

                            oInsFileElem = oInsFile.GetElementsByTagName(ACXMLARetProductBlockNo)

                            r_vProductArray(4, lRow) = oInsFileElem.Item(0).InnerText

                        Next lRow

                    End If

                End If

            End If

            'RJG 28/11/2000 - Unformat elements for AddParty, GetParty


            r_lPartyCnt = oActionReturnElem.GetAttribute(ACXMLARetPartyCnt)


            r_sSurname = oActionReturnElem.GetAttribute(ACXMLARetPartySurname)


            r_sForename = oActionReturnElem.GetAttribute(ACXMLARetPartyForename)


            r_sPartyTypeCode = oActionReturnElem.GetAttribute(ACXMLARetPartyTypeCode)


            r_sAddress1 = oActionReturnElem.GetAttribute(ACXMLARetPartyAddress1)


            r_sAddress2 = oActionReturnElem.GetAttribute(ACXMLARetPartyAddress2)


            r_sAddress3 = oActionReturnElem.GetAttribute(ACXMLARetPartyAddress3)


            r_sAddress4 = oActionReturnElem.GetAttribute(ACXMLARetPartyAddress4)


            r_sPostcode = oActionReturnElem.GetAttribute(ACXMLARetPartyPostcode)


            r_sDateOfBirth = oActionReturnElem.GetAttribute(ACXMLARetPartyDateOfBirth)


            r_sEMail = oActionReturnElem.GetAttribute(ACXMLARetPartyEMail)


            r_sUserID = oActionReturnElem.GetAttribute(ACXMLARetPartyUserID)


            r_sPassword = oActionReturnElem.GetAttribute(ACXMLARetPartyPassword)


            r_sShortName = oActionReturnElem.GetAttribute(ACXMLARetPartyShortname)


            r_sResolvedName = oActionReturnElem.GetAttribute(ACXMLARetPartyResolvedName)


            r_sMothersMaidenName = oActionReturnElem.GetAttribute(ACXMLARetPartyMothersMaidenName)


            r_sTPUserCode = oActionReturnElem.GetAttribute(ACXMLARetPartyTPUserCode)


            r_sTPIntroducer = oActionReturnElem.GetAttribute(ACXMLARetPartyTPIntroducer)


            r_sAQuestion = oActionReturnElem.GetAttribute(ACXMLARetPartyAQuestion)


            r_sTheAnswer = oActionReturnElem.GetAttribute(ACXMLARetPartyTheAnswer)


            r_sMemorableDate = oActionReturnElem.GetAttribute(ACXMLARetPartyMemorableDate)


            r_sCurrInsRenewalDate = oActionReturnElem.GetAttribute(ACXMLARetPartyCurrInsRenewalDate)


            r_sTitle = oActionReturnElem.GetAttribute(ACXMLARetPartyTitle)


            r_sMaritalStatusCode = oActionReturnElem.GetAttribute(ACXMLARetPartyMaritalStatusCode)


            r_sGenderCode = oActionReturnElem.GetAttribute(ACXMLARetPartyGenderCode)


            r_sInitials = oActionReturnElem.GetAttribute(ACXMLARetPartyInitials)


            r_sTelephoneNumber = oActionReturnElem.GetAttribute(ACXMLARetPartyTelephoneNumber)

            '**** START CHANGES - Changed By: AAB  - Changed On: 04-Sep-2002 14:51   ****
            '**** To get back the Additional DataArray.
            ' Return the Additional Data Array

            lReturn = CType(UnFormatAdditionalDataXML(r_oDocument:=oActionReturn, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
            '****   END CHANGES - Changed By: AAB  - Changed On: 04-Sep-2002 14:51   ****

            oInsFiles = Nothing
            oInsFile = Nothing

            oActionReturnElem = Nothing
            oActionReturn = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatActionReturnXMLQuotePolicyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionReturnXMLQuotePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
