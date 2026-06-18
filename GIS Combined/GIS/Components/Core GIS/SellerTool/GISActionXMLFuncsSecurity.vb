Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
'developer guide no. 129
Imports SharedFiles
Module GISActionXMLFuncsSecurity
	' ***************************************************************** '
	' Module Name: GISActionXMLFuncsSecurity
	'
	' Date:  CL040600
	'
	' Description: Functions to handle the Action XML
	'
	' Edit History:
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "GISActionXMLFuncsSecurity"
	
	Public Const ACXMLActionSecurity As String = "ACTION_SECURITY"
	Public Const ACXMLActionSecurityEndTag As String = "</ACTION_SECURITY>"
	
	Public Const ACXMLActionSecurityClassName As String = "SECURITY"
	
	Public Const ACXMLARetDummyResponse As String = "DATACASH_RESPONSE"
	
	
	' RAG07062000 - constants for Security Attributes
	Public Const ACXMLAttribUserID As String = "UserID"
	' RFC200700 - Return the PMUserID if the login is a TPA
	Public Const ACXMLAttribPMUserID As String = "PMUserID"
	Public Const ACXMLAttribPassword As String = "Password"
	Public Const ACXMLAttribForename As String = "Forename"
	Public Const ACXMLAttribSurname As String = "Surname"
	Public Const ACXMLAttribMothersMaidenName As String = "MothersMaidenName"
	Public Const ACXMLAttribDateOfBirth As String = "DateOfBirth"
	Public Const ACXMLAttribEmailAddress As String = "EmailAddress"
	Public Const ACXMLAttribMemorableDate As String = "MemorableDate"
	Public Const ACXMLAttribAQuestion As String = "AQuestion"
	Public Const ACXMLAttribTheAnswer As String = "TheAnswer"
	Public Const ACXMLAttribCurrentRenewalDate As String = "CurrentRenewalDate"
	' RFC050900 - Extra Parameters required for New Register User Method
	Public Const ACXMLAttribTitle As String = "Title"
	Public Const ACXMLAttribMaritalStatusCode As String = "MaritalStatusCode"
	Public Const ACXMLAttribAddress1 As String = "Address1"
	Public Const ACXMLAttribAddress2 As String = "Address2"
	Public Const ACXMLAttribAddress3 As String = "Address3"
	Public Const ACXMLAttribAddress4 As String = "Address4"
	Public Const ACXMLAttribPostcode As String = "Postcode"
	
	' ***************************************************************** '
	' Name: FormatActionXML
	'
	' Description:
	'
	' RFC13012000 - Add Effective Date
	' RFC050900 - Extra Parameters required for New Register User Method
	' ***************************************************************** '
    Public Function FormatActionXMLSecurity(ByVal v_lAction As Integer, ByVal v_sSellerGUID As String, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef r_sActionXML As String, Optional ByVal v_sUserID As String = "", Optional ByVal v_sPassword As String = "", Optional ByVal v_sForename As String = "", Optional ByVal v_sSurname As String = "", Optional ByVal v_sMothersMaidenName As String = "", Optional ByVal v_sDateOfBirth As String = "", Optional ByVal v_sEmailAddress As String = "", Optional ByVal v_sMemorableDate As String = "", Optional ByVal v_sAQuestion As String = "", Optional ByVal v_sTheAnswer As String = "", Optional ByVal v_sCurrentRenewalDate As String = "", Optional ByVal v_sTitle As String = "", Optional ByVal v_sMaritalStatusCode As String = "", Optional ByVal v_sAddress1 As String = "", Optional ByVal v_sAddress2 As String = "", Optional ByVal v_sAddress3 As String = "", Optional ByVal v_sAddress4 As String = "", Optional ByVal v_sPostcode As String = "", Optional ByRef v_vAdditionalDataArray(,) As Object = Nothing) As Integer

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
            oActionChildElem = oAction.CreateElement(ACXMLActionSecurity)

            ' Set the Action Value
            'oActionChildElem.Text = v_lAction
            oActionChildElem.SetAttribute(ACXMLAttribGISAction, v_lAction)

            ' Set the Other Attributes TEMPORARILY until BGIS is changed
            oActionChildElem.SetAttribute(ACXMLAttribDataModelCode, v_sDataModelCode)

            oActionChildElem.SetAttribute(ACXMLAttribUserID, v_sUserID)
            oActionChildElem.SetAttribute(ACXMLAttribPassword, v_sPassword)
            oActionChildElem.SetAttribute(ACXMLAttribForename, v_sForename)
            oActionChildElem.SetAttribute(ACXMLAttribSurname, v_sSurname)
            oActionChildElem.SetAttribute(ACXMLAttribMothersMaidenName, v_sMothersMaidenName)
            oActionChildElem.SetAttribute(ACXMLAttribDateOfBirth, v_sDateOfBirth)
            oActionChildElem.SetAttribute(ACXMLAttribEmailAddress, v_sEmailAddress)
            oActionChildElem.SetAttribute(ACXMLAttribMemorableDate, v_sMemorableDate)
            oActionChildElem.SetAttribute(ACXMLAttribAQuestion, v_sAQuestion)
            oActionChildElem.SetAttribute(ACXMLAttribTheAnswer, v_sTheAnswer)
            oActionChildElem.SetAttribute(ACXMLAttribCurrentRenewalDate, v_sCurrentRenewalDate)

            ' RFC050900 - Extra Parameters required for New Register User Method - START
            oActionChildElem.SetAttribute(ACXMLAttribBusinessTypeCode, v_sBusinessTypeCode)
            oActionChildElem.SetAttribute(ACXMLAttribTitle, v_sTitle)
            oActionChildElem.SetAttribute(ACXMLAttribMaritalStatusCode, v_sMaritalStatusCode)
            oActionChildElem.SetAttribute(ACXMLAttribAddress1, v_sAddress1)
            oActionChildElem.SetAttribute(ACXMLAttribAddress2, v_sAddress2)
            oActionChildElem.SetAttribute(ACXMLAttribAddress3, v_sAddress3)
            oActionChildElem.SetAttribute(ACXMLAttribAddress4, v_sAddress4)
            oActionChildElem.SetAttribute(ACXMLAttribPostcode, v_sPostcode)
            ' RFC050900 - Extra Parameters required for New Register User Method - END

            ' Add the Additional Data Items

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
            bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatActionXMLSecurity Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatActionXMLSecurity", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	' ***************************************************************** '
	' Name: UnFormatActionXMLSecurity
	'
	' Description:
	' RFC050900 - Extra Parameters required for New Register User Method
	' ***************************************************************** '
    Public Function UnFormatActionXMLSecurity(ByVal v_sActionXML As String, ByRef r_lAction As Integer, ByRef r_sSellerGUID As String, Optional ByRef r_sDataModelCode As String = "", Optional ByRef r_sBusinessTypeCode As String = "", Optional ByRef r_sUserID As String = "", Optional ByRef r_sPassword As String = "", Optional ByRef r_sForename As String = "", Optional ByRef r_sSurname As String = "", Optional ByRef r_sMothersMaidenName As String = "", Optional ByRef r_sDateOfBirth As String = "", Optional ByRef r_sEmailAddress As String = "", Optional ByRef r_sMemorableDate As String = "", Optional ByRef r_sAQuestion As String = "", Optional ByRef r_sTheAnswer As String = "", Optional ByRef r_sCurrentRenewalDate As String = "", Optional ByRef r_sTitle As String = "", Optional ByRef r_sMaritalStatusCode As String = "", Optional ByRef r_sAddress1 As String = "", Optional ByRef r_sAddress2 As String = "", Optional ByRef r_sAddress3 As String = "", Optional ByRef r_sAddress4 As String = "", Optional ByRef r_sPostcode As String = "", Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer



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
                bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & v_sActionXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionXMLSecurity")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oActionElem = oAction.DocumentElement
            oActionElem = oAction.DocumentElement.FirstChild ' CL240500

            ' Get the Action Value
            'r_lAction = oActionElem.Text

            r_lAction = CInt(oActionElem.GetAttribute(ACXMLAttribGISAction))

            r_sDataModelCode = CStr(oActionElem.GetAttribute(ACXMLAttribDataModelCode))


            r_sUserID = CStr(oActionElem.GetAttribute(ACXMLAttribUserID))

            r_sPassword = CStr(oActionElem.GetAttribute(ACXMLAttribPassword))

            r_sForename = CStr(oActionElem.GetAttribute(ACXMLAttribForename))

            r_sSurname = CStr(oActionElem.GetAttribute(ACXMLAttribSurname))

            r_sMothersMaidenName = CStr(oActionElem.GetAttribute(ACXMLAttribMothersMaidenName))

            r_sDateOfBirth = CStr(oActionElem.GetAttribute(ACXMLAttribDateOfBirth))

            r_sEmailAddress = CStr(oActionElem.GetAttribute(ACXMLAttribEmailAddress))

            r_sMemorableDate = CStr(oActionElem.GetAttribute(ACXMLAttribMemorableDate))

            r_sAQuestion = CStr(oActionElem.GetAttribute(ACXMLAttribAQuestion))

            r_sTheAnswer = CStr(oActionElem.GetAttribute(ACXMLAttribTheAnswer))

            r_sCurrentRenewalDate = CStr(oActionElem.GetAttribute(ACXMLAttribCurrentRenewalDate))

            ' RFC050900 - Extra Parameters required for New Register User Method - START

            r_sBusinessTypeCode = CStr(oActionElem.GetAttribute(ACXMLAttribBusinessTypeCode))

            r_sTitle = CStr(oActionElem.GetAttribute(ACXMLAttribTitle))

            r_sMaritalStatusCode = CStr(oActionElem.GetAttribute(ACXMLAttribMaritalStatusCode))

            r_sAddress1 = CStr(oActionElem.GetAttribute(ACXMLAttribAddress1))

            r_sAddress2 = CStr(oActionElem.GetAttribute(ACXMLAttribAddress2))

            r_sAddress3 = CStr(oActionElem.GetAttribute(ACXMLAttribAddress3))

            r_sAddress4 = CStr(oActionElem.GetAttribute(ACXMLAttribAddress4))

            r_sPostcode = CStr(oActionElem.GetAttribute(ACXMLAttribPostcode))
            ' RFC050900 - Extra Parameters required for New Register User Method - END

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
            bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatActionXMLSecurity Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionXMLSecurity", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	
	' ***************************************************************** '
	' Name: FormatActionReturnXMLSecurity
	'
	' Description:
	' RFC200700 - Return the PMUserID if the login is a TPA
	' RFC310700 - Return user details for pre-population of data
	' CJB170902 - Format all dates consistently - they must be formatted
	'             or they may be misinterpreted.
	' ***************************************************************** '
	Public Function FormatActionReturnXMLSecurity(ByVal v_lReturnValue As Integer, ByRef r_sActionReturnXML As String, Optional ByVal v_sUserID As String = "", Optional ByVal v_sPassword As String = "", Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lPMUserID As Integer = -1, Optional ByVal v_sForename As String = "", Optional ByVal v_sSurname As String = "", Optional ByVal v_dtDateOfBirth As Date = iGISSharedConstants.GISLowDate, Optional ByVal v_sEmailAddress As String = "") As Integer
		
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
			
			' RAG070600
			oActionReturnElem.SetAttribute(ACXMLAttribUserID, v_sUserID)
			oActionReturnElem.SetAttribute(ACXMLAttribPassword, v_sPassword)
			oActionReturnElem.SetAttribute(ACXMLAttribPartyCnt, v_lPartyCnt)
			' RFC200700 - Return the PMUserID if the login is a TPA
			oActionReturnElem.SetAttribute(ACXMLAttribPMUserID, v_lPMUserID)
			' RFC310700 - Return user details for pre-population of data
			oActionReturnElem.SetAttribute(ACXMLAttribForename, v_sForename)
			oActionReturnElem.SetAttribute(ACXMLAttribSurname, v_sSurname)
			
			' CJB170902 - Format all dates consistently
			oActionReturnElem.SetAttribute(ACXMLAttribDateOfBirth, v_dtDateOfBirth.ToString("yyyy-MM-dd HH:mm:ss"))
			oActionReturnElem.SetAttribute(ACXMLAttribEmailAddress, v_sEmailAddress)
			
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
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatActionReturnXMLSecurity Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatActionReturnXMLSecurity", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: UnFormatActionReturnXMLSecurity
	'
	' Description:
	' RFC200700 - Return the PMUserID if the login is a TPA
	' RFC310700 - Return user details for pre-population of data
	' CJB170902 - Format all dates consistently
	' ***************************************************************** '
	Public Function UnFormatActionReturnXMLSecurity(ByVal v_sActionReturnXML As String, ByRef r_lReturnValue As Integer, Optional ByRef r_sUserID As String = "", Optional ByRef r_sPassword As String = "", Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_lPMUserID As Integer = -1, Optional ByRef r_sForename As String = "", Optional ByRef r_sSurname As String = "", Optional ByRef r_dtDateOfBirth As Date = iGISSharedConstants.GISLowDate, Optional ByRef r_sEmailAddress As String = "") As Integer
		
		Dim result As Integer = 0
		Dim oActionReturnElem As XmlElement
		Dim oActionReturn As XmlDocument
		Dim bLoaded As Boolean
		Dim oMatches As XmlNodeList
		Dim oMatch As XmlElement
		Dim oAddLineElem, oElem As XmlNodeList
		Dim lNumMatches, lRow As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create a New Document
			oActionReturn = New XmlDocument()
			
			' Load the Action XML

            'developer guide no. no solution 22
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
				bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & v_sActionReturnXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionReturnXMLSecurity")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oActionReturnElem = oActionReturn.DocumentElement
			
			' Get the Attributes

			r_lReturnValue = CInt(oActionReturnElem.GetAttribute(ACXMLAttribReturnValue))
			
			' RAG070600

			r_sUserID = CStr(oActionReturnElem.GetAttribute(ACXMLAttribUserID))

			r_sPassword = CStr(oActionReturnElem.GetAttribute(ACXMLAttribPassword))

			r_lPartyCnt = CInt(oActionReturnElem.GetAttribute(ACXMLAttribPartyCnt))
			' RFC200700 - Return the PMUserID if the login is a TPA

			r_lPMUserID = CInt(oActionReturnElem.GetAttribute(ACXMLAttribPMUserID))
			' RFC310700 - Return user details for pre-population of data

			r_sForename = CStr(oActionReturnElem.GetAttribute(ACXMLAttribForename))

			r_sSurname = CStr(oActionReturnElem.GetAttribute(ACXMLAttribSurname))
			
			' CJB170902 - Format all dates consistently

			r_dtDateOfBirth = CDate(oActionReturnElem.GetAttribute(ACXMLAttribDateOfBirth))

			r_sEmailAddress = CStr(oActionReturnElem.GetAttribute(ACXMLAttribEmailAddress))
			
			oActionReturnElem = Nothing
			oActionReturn = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			
			' Log Error Message
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatActionReturnXMLSecurity Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionReturnXMLSecurity", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module
