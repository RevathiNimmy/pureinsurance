Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
'developer guide no. 129
Imports SharedFiles
Module GISActionXMLFuncsRenewals
	' ***************************************************************** '
	' Module Name: GISActionXMLFuncsRenewals
	'
	' Date:  22/10/2001
	'
	' Description: Functions to map the calls for Renewals
	'
	' Edit History:
	' DD22102001 - Created
	' DD29102001 - Added SuspensionLevel parameter
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "GISActionXMLFuncsRenewals"
	
	Public Const ACXMLActionRenewals As String = "ACTION_RENEWAL"
	Public Const ACXMLActionRenewalsEndTag As String = "</ACTION_RENEWAL>"
	
	Public Const ACXMLActionRenewalsClassName As String = "Renewals"
	
	'Renewals attributes used in addition to other global constants
	Public Const ACXMLAttribBusinessTypeID As String = "BusinessTypeID"
	Public Const ACXMLAttribDataModelID As String = "DataModelID"
	Public Const ACXMLAttribInsuranceFolderCnt As String = "InsuranceFolderCnt"
	Public Const ACXMLAttribProductID As String = "ProductID"
	Public Const ACXMLAttribRenewalDate As String = "RenewalDate"
	Public Const ACXMLAttribRiskCodeID As String = "RiskCodeID"
	Public Const ACXMLAttribRenewalEDIAuditID As String = "RenewalEDIAuditID"
	Public Const ACXMLAttribRenewalInsuranceFileCnt As String = "RenewalInsuranceFileCnt"
	Public Const ACXMLAttribRenewalSchemeID As String = "RenewalSchemeID"
	Public Const ACXMLAttribRenewalStatusCode As String = "RenewalStatusCode"
	Public Const ACXMLAttribRenewalOldInsuranceFileCnt As String = "RenewalOldInsuranceFileCnt"
	Public Const ACXMLAttribRenewalDueDateStart As String = "RenewalDueDateStart"
	Public Const ACXMLAttribRenewalDueDateLimit As String = "RenewalDueDateLimit"
	Public Const ACXMLAttribRenewalClientCode As String = "RenewalClientCode"
	Public Const ACXMLAttribRenewalPolicyNo As String = "RenewalPolicyNo"
	Public Const ACXMLAttribRenewalInsurerID As String = "RenewalInsurerID"
	Public Const ACXMLAttribRenewalSuspensionLevel As String = "SuspensionLevel"
	
	'Tag for the returned array
	Public Const ACXMLAttribRenewalArray As String = "RenewalArray"
	
	' ***************************************************************** '
	' Name: FormatActionXMLRenewals
	'
	' Description:  Set Renewal specific XML properties from parameters.
	'               Called by iGISSellerTool before sending to bGIS.
	'
	' DD22102001 - Created
	' DD29102001 - Added SuspensionLevel parameter
	' CJB17092002 -Format all dates consistently - they must be formatted
	'              or they may be misinterpreted.
	' ***************************************************************** '
	Public Function FormatActionXMLRenewals(ByVal v_lAction As Integer, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef r_sActionXML As String, Optional ByVal v_sSellerGUID As String = "", Optional ByVal v_lBusinessTypeID As Integer = 0, Optional ByVal v_lDataModelID As Integer = 0, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lProductID As Integer = 0, Optional ByVal v_dtRenewalDate As Date = #12/30/1899#, Optional ByVal v_lRiskCodeID As Integer = 0, Optional ByVal v_lRenewalEdiAuditId As Integer = 0, Optional ByVal v_lRenewalInsuranceFileCnt As Integer = 0, Optional ByVal v_lRenewalSchemeID As Integer = 0, Optional ByVal v_sRenewalStatusCode As String = "", Optional ByVal v_lSchemeID As Integer = 0, Optional ByVal v_lOldInsuranceFileCnt As Integer = 0, Optional ByVal v_dtDueDateStart As Date = #12/30/1899#, Optional ByVal v_dtDueDateLimit As Date = #12/30/1899#, Optional ByVal v_sClientCode As String = "", Optional ByVal v_sPolicyNo As String = "", Optional ByVal v_lInsurerId As Integer = 0, Optional ByVal v_lSuspensionLevel As Integer = 0) As Integer
		
		Dim result As Integer = 0
		Dim oAction As XmlDocument
		Dim oActionElem, oActionChildElem As XmlElement
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create a New XML Document
			oAction = New XmlDocument()
			
			' Create the Action Element
			oActionElem = oAction.CreateElement(ACXMLAction)
			oActionChildElem = oAction.CreateElement(ACXMLActionRenewals)
			
			' Set the Action Value
			'oActionChildElem.Text = v_lAction
			oActionChildElem.SetAttribute(ACXMLAttribGISAction, v_lAction)
			
			' Set the Other Attributes TEMPORARILY until BGIS is changed
			oActionChildElem.SetAttribute(ACXMLAttribDataModelCode, v_sDataModelCode)
			
			' Renewals Specific Attributes
			oActionChildElem.SetAttribute(ACXMLAttribSellerGUID, v_sSellerGUID)
			oActionChildElem.SetAttribute(ACXMLAttribBusinessTypeCode, v_sBusinessTypeCode)
			oActionChildElem.SetAttribute(ACXMLAttribBusinessTypeID, v_lBusinessTypeID)
			oActionChildElem.SetAttribute(ACXMLAttribDataModelID, v_lDataModelID)
			oActionChildElem.SetAttribute(ACXMLAttribInsuranceFolderCnt, v_lInsuranceFolderCnt)
			oActionChildElem.SetAttribute(ACXMLAttribInsuranceFileCnt, v_lInsuranceFileCnt)
			oActionChildElem.SetAttribute(ACXMLAttribPartyCnt, v_lPartyCnt)
			oActionChildElem.SetAttribute(ACXMLAttribProductID, v_lProductID)
			
			' CJB 17092002 Format all dates consistently
			oActionChildElem.SetAttribute(ACXMLAttribRenewalDate, v_dtRenewalDate.ToString("yyyy-MM-dd HH:mm:ss"))
			oActionChildElem.SetAttribute(ACXMLAttribRiskCodeID, v_lRiskCodeID)
			oActionChildElem.SetAttribute(ACXMLAttribRenewalEDIAuditID, v_lRenewalEdiAuditId)
			oActionChildElem.SetAttribute(ACXMLAttribRenewalInsuranceFileCnt, v_lRenewalInsuranceFileCnt)
			oActionChildElem.SetAttribute(ACXMLAttribRenewalSchemeID, v_lRenewalSchemeID)
			oActionChildElem.SetAttribute(ACXMLAttribRenewalStatusCode, v_sRenewalStatusCode)
			oActionChildElem.SetAttribute(ACXMLAttribSchemeID, v_lSchemeID)
			oActionChildElem.SetAttribute(ACXMLAttribRenewalOldInsuranceFileCnt, v_lOldInsuranceFileCnt)
			oActionChildElem.SetAttribute(ACXMLAttribRenewalDueDateStart, v_dtDueDateStart.ToString("yyyy-MM-dd HH:mm:ss"))
			oActionChildElem.SetAttribute(ACXMLAttribRenewalDueDateLimit, v_dtDueDateLimit.ToString("yyyy-MM-dd HH:mm:ss"))
			oActionChildElem.SetAttribute(ACXMLAttribRenewalClientCode, v_sClientCode)
			oActionChildElem.SetAttribute(ACXMLAttribRenewalPolicyNo, v_sPolicyNo)
			oActionChildElem.SetAttribute(ACXMLAttribRenewalInsurerID, v_lInsurerId)
			oActionChildElem.SetAttribute(ACXMLAttribRenewalSuspensionLevel, v_lSuspensionLevel)
			

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
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatActionXMLRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatActionXMLRenewals", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: UnFormatActionXMLRenewals
	'
	' Description:  Extract the XML properties into Renewal specific
	'               parameters. This is called on the way in to bGIS.
	'
	' DD22102001 - Created
	' DD29102001 - Added SuspensionLevel parameter
	' CJB17092002 -Format all dates consistently - they must be formatted
	'              or they may be misinterpreted.
	' ***************************************************************** '
	Public Function UnFormatActionXMLRenewals(ByRef r_lAction As Integer, ByRef r_sDataModelCode As String, ByRef r_sBusinessTypeCode As String, ByVal v_sActionXML As String, Optional ByRef r_sSellerGUID As String = "", Optional ByRef r_lBusinessTypeID As Integer = 0, Optional ByRef r_lDataModelID As Integer = 0, Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_lInsuranceFileCnt As Integer = 0, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_lProductID As Integer = 0, Optional ByRef r_dtRenewalDate As Date = #12/30/1899#, Optional ByRef r_lRiskCodeID As Integer = 0, Optional ByRef r_lRenewalEdiAuditId As Integer = 0, Optional ByRef r_lRenewalInsuranceFileCnt As Integer = 0, Optional ByRef r_lRenewalSchemeID As Integer = 0, Optional ByRef r_sRenewalStatusCode As String = "", Optional ByRef r_lSchemeID As Integer = 0, Optional ByRef r_lOldInsuranceFileCnt As Integer = 0, Optional ByRef r_dtDueDateStart As Date = #12/30/1899#, Optional ByRef r_dtDueDateLimit As Date = #12/30/1899#, Optional ByRef r_sClientCode As String = "", Optional ByRef r_sPolicyNo As String = "", Optional ByRef r_lInsurerID As Integer = 0, Optional ByRef r_lSuspensionLevel As Integer = 0) As Integer
		
		Dim result As Integer = 0
		Dim oActionElem As XmlElement
		Dim oAction As XmlDocument
		Dim bLoaded As Boolean
		
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
				bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & v_sActionXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionXMLRenewals")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oActionElem = oAction.DocumentElement
			oActionElem = oAction.DocumentElement.FirstChild ' CL240500
			
			'Set local variables from Renewal specific XML properties
			

			r_lAction = CInt(oActionElem.GetAttribute(ACXMLAttribGISAction))

			r_sDataModelCode = CStr(oActionElem.GetAttribute(ACXMLAttribDataModelCode))

			r_sBusinessTypeCode = CStr(oActionElem.GetAttribute(ACXMLAttribBusinessTypeCode))
			

			r_sSellerGUID = CStr(oActionElem.GetAttribute(ACXMLAttribSellerGUID))

			r_lBusinessTypeID = CInt(oActionElem.GetAttribute(ACXMLAttribBusinessTypeID))

			r_lDataModelID = CInt(oActionElem.GetAttribute(ACXMLAttribDataModelID))

			r_lInsuranceFolderCnt = CInt(oActionElem.GetAttribute(ACXMLAttribInsuranceFolderCnt))

			r_lInsuranceFileCnt = CInt(oActionElem.GetAttribute(ACXMLAttribInsuranceFileCnt))

			r_lPartyCnt = CInt(oActionElem.GetAttribute(ACXMLAttribPartyCnt))

			r_lProductID = CInt(oActionElem.GetAttribute(ACXMLAttribProductID))
			' CJB17092002- Format all dates consistently

			r_dtRenewalDate = CDate(oActionElem.GetAttribute(ACXMLAttribRenewalDate))

			r_lRiskCodeID = CInt(oActionElem.GetAttribute(ACXMLAttribRiskCodeID))

			r_lRenewalEdiAuditId = CInt(oActionElem.GetAttribute(ACXMLAttribRenewalEDIAuditID))

			r_lRenewalInsuranceFileCnt = CInt(oActionElem.GetAttribute(ACXMLAttribRenewalInsuranceFileCnt))

			r_lRenewalSchemeID = CInt(oActionElem.GetAttribute(ACXMLAttribRenewalSchemeID))

			r_sRenewalStatusCode = CStr(oActionElem.GetAttribute(ACXMLAttribRenewalStatusCode))

			r_lSchemeID = CInt(oActionElem.GetAttribute(ACXMLAttribSchemeID))

			r_lOldInsuranceFileCnt = CInt(oActionElem.GetAttribute(ACXMLAttribRenewalOldInsuranceFileCnt))

			r_dtDueDateStart = CDate(oActionElem.GetAttribute(ACXMLAttribRenewalDueDateStart))

			r_dtDueDateLimit = CDate(oActionElem.GetAttribute(ACXMLAttribRenewalDueDateLimit))

			r_sClientCode = CStr(oActionElem.GetAttribute(ACXMLAttribRenewalClientCode))

			r_sPolicyNo = CStr(oActionElem.GetAttribute(ACXMLAttribRenewalPolicyNo))

			r_lInsurerID = CInt(oActionElem.GetAttribute(ACXMLAttribRenewalInsurerID))

			r_lSuspensionLevel = CInt(oActionElem.GetAttribute(ACXMLAttribRenewalSuspensionLevel))
			
			oActionElem = Nothing
			oAction = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			
			
			' Log Error Message
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatActionXMLRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionXMLRenewals", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: FormatActionReturnXMLRenewals
	'
	' Description:  Pass the local variables back into the XML for
	'               returning from bGIS to the iGISSellerTool.
	'
	' DD22102001 - Created
	' DD29102001 - Added SuspensionLevel parameter
	' CJB17092002 -Format all dates consistently - they must be formatted
	'              or they may be misinterpreted.
	' ***************************************************************** '
	Public Function FormatActionReturnXMLRenewals(ByVal v_lReturnValue As Integer, ByRef r_sActionReturnXML As String, Optional ByVal v_sBusinessTypeCode As String = "", Optional ByVal v_lBusinessTypeID As Integer = 0, Optional ByVal v_lDataModelID As Integer = 0, Optional ByVal v_sDataModelCode As String = "", Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lProductID As Integer = 0, Optional ByVal v_dtRenewalDate As Date = #12/30/1899#, Optional ByVal v_lRiskCodeID As Integer = 0, Optional ByVal v_lRenewalEdiAuditId As Integer = 0, Optional ByVal v_lRenewalInsuranceFileCnt As Integer = 0, Optional ByVal v_lRenewalSchemeID As Integer = 0, Optional ByVal v_sRenewalStatusCode As String = "", Optional ByVal v_lSchemeID As Integer = 0, Optional ByVal v_lOldInsuranceFileCnt As Integer = 0, Optional ByVal v_dtDueDateStart As Date = #12/30/1899#, Optional ByVal v_dtDueDateLimit As Date = #12/30/1899#, Optional ByVal v_sClientCode As String = "", Optional ByVal v_sPolicyNo As String = "", Optional ByVal v_lInsurerId As Integer = 0, Optional ByVal v_lSuspensionLevel As Integer = 0, Optional ByVal v_vRenewalArray As Array = Nothing) As Integer
		
		Dim result As Integer = 0
		Dim oActionReturn As XmlDocument
		Dim oActionReturnElem As XmlElement
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create a New XML Document
			oActionReturn = New XmlDocument()
			
			' Create the Action Element
			oActionReturnElem = oActionReturn.CreateElement(ACXMLActionReturn)
			
			oActionReturnElem.InnerText = "Action Return"
			
			' Set the Attributes
			oActionReturnElem.SetAttribute(ACXMLAttribReturnValue, v_lReturnValue)
			
			' Renewals Specific Attributes
			oActionReturnElem.SetAttribute(ACXMLAttribBusinessTypeCode, v_sBusinessTypeCode)
			oActionReturnElem.SetAttribute(ACXMLAttribBusinessTypeID, v_lBusinessTypeID)
			oActionReturnElem.SetAttribute(ACXMLAttribDataModelID, v_lDataModelID)
			oActionReturnElem.SetAttribute(ACXMLAttribDataModelCode, v_sDataModelCode)
			oActionReturnElem.SetAttribute(ACXMLAttribInsuranceFolderCnt, v_lInsuranceFolderCnt)
			oActionReturnElem.SetAttribute(ACXMLAttribInsuranceFileCnt, v_lInsuranceFileCnt)
			oActionReturnElem.SetAttribute(ACXMLAttribPartyCnt, v_lPartyCnt)
			oActionReturnElem.SetAttribute(ACXMLAttribProductID, v_lProductID)
			' CJB17092002- Format all dates consistently
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalDate, v_dtRenewalDate.ToString("yyyy-MM-dd HH:mm:ss"))
			oActionReturnElem.SetAttribute(ACXMLAttribRiskCodeID, v_lRiskCodeID)
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalEDIAuditID, v_lRenewalEdiAuditId)
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalInsuranceFileCnt, v_lRenewalInsuranceFileCnt)
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalSchemeID, v_lRenewalSchemeID)
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalStatusCode, v_sRenewalStatusCode)
			oActionReturnElem.SetAttribute(ACXMLAttribSchemeID, v_lSchemeID)
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalOldInsuranceFileCnt, v_lOldInsuranceFileCnt)
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalDueDateStart, v_dtDueDateStart.ToString("yyyy-MM-dd HH:mm:ss"))
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalDueDateLimit, v_dtDueDateLimit.ToString("yyyy-MM-dd HH:mm:ss"))
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalClientCode, v_sClientCode)
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalPolicyNo, v_sPolicyNo)
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalInsurerID, v_lInsurerId)
			oActionReturnElem.SetAttribute(ACXMLAttribRenewalSuspensionLevel, v_lSuspensionLevel)
			
			' Drop in the array
			FormatArrayToXML(oActionReturn, oActionReturnElem, ACXMLAttribRenewalArray, v_vRenewalArray)
			
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
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatActionReturnXMLRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatActionReturnXMLRenewals", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: UnFormatActionReturnXMLRenewals
	'
	' Description:  Extract the parameters from the XML returned back
	'               from the GIS. This is called by iGISSellerTool.
	'
	' DD22102001 - Created
	' DD29102001 - Added SuspensionLevel parameter
	' CJB17092002 -Format all dates consistently - they must be formatted
	'              or they may be misinterpreted.
	' ***************************************************************** '
	Public Function UnFormatActionReturnXMLRenewals(ByVal v_sActionReturnXML As String, ByRef r_lReturnValue As Integer, Optional ByRef r_sBusinessTypeCode As String = "", Optional ByRef r_lBusinessTypeID As Integer = 0, Optional ByRef r_lDataModelID As Integer = 0, Optional ByRef r_sDataModelCode As String = "", Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_lInsuranceFileCnt As Integer = 0, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_lProductID As Integer = 0, Optional ByRef r_dtRenewalDate As Date = #12/30/1899#, Optional ByRef r_lRiskCodeID As Integer = 0, Optional ByRef r_lRenewalEdiAuditId As Integer = 0, Optional ByRef r_lRenewalInsuranceFileCnt As Integer = 0, Optional ByRef r_lRenewalSchemeID As Integer = 0, Optional ByRef r_sRenewalStatusCode As String = "", Optional ByRef r_lSchemeID As Integer = 0, Optional ByRef r_lOldInsuranceFileCnt As Integer = 0, Optional ByRef r_dtDueDateStart As Date = #12/30/1899#, Optional ByRef r_dtDueDateLimit As Date = #12/30/1899#, Optional ByRef r_sClientCode As String = "", Optional ByRef r_sPolicyNo As String = "", Optional ByRef r_lInsurerID As Integer = 0, Optional ByRef r_lSuspensionLevel As Integer = 0, Optional ByRef r_vRenewalArray As Array = Nothing) As Integer
		
		
		Dim result As Integer = 0
		Dim oActionReturnElem As XmlElement
		Dim oActionReturn As XmlDocument
		Dim bLoaded As Boolean
		
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
				bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Load Data Set Definition from XML String : " & v_sActionReturnXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionReturnXMLRenewals")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oActionReturnElem = oActionReturn.DocumentElement
			
			' Get the Attributes

			r_lReturnValue = CInt(oActionReturnElem.GetAttribute(ACXMLAttribReturnValue))
			
			'Set local variables from Renewal specific XML properties

			r_sBusinessTypeCode = CStr(oActionReturnElem.GetAttribute(ACXMLAttribBusinessTypeCode))

			r_lBusinessTypeID = CInt(oActionReturnElem.GetAttribute(ACXMLAttribBusinessTypeID))

			r_lDataModelID = CInt(oActionReturnElem.GetAttribute(ACXMLAttribDataModelID))

			r_sDataModelCode = CStr(oActionReturnElem.GetAttribute(ACXMLAttribDataModelCode))

			r_lInsuranceFolderCnt = CInt(oActionReturnElem.GetAttribute(ACXMLAttribInsuranceFolderCnt))

			r_lInsuranceFileCnt = CInt(oActionReturnElem.GetAttribute(ACXMLAttribInsuranceFileCnt))

			r_lPartyCnt = CInt(oActionReturnElem.GetAttribute(ACXMLAttribPartyCnt))

			r_lProductID = CInt(oActionReturnElem.GetAttribute(ACXMLAttribProductID))
			' CJB17092002 Format all dates consistently

			r_dtRenewalDate = CDate(oActionReturnElem.GetAttribute(ACXMLAttribRenewalDate))

			r_lRiskCodeID = CInt(oActionReturnElem.GetAttribute(ACXMLAttribRiskCodeID))

			r_lRenewalEdiAuditId = CInt(oActionReturnElem.GetAttribute(ACXMLAttribRenewalEDIAuditID))

			r_lRenewalInsuranceFileCnt = CInt(oActionReturnElem.GetAttribute(ACXMLAttribRenewalInsuranceFileCnt))

			r_lRenewalSchemeID = CInt(oActionReturnElem.GetAttribute(ACXMLAttribRenewalSchemeID))

			r_sRenewalStatusCode = CStr(oActionReturnElem.GetAttribute(ACXMLAttribRenewalStatusCode))

			r_lSchemeID = CInt(oActionReturnElem.GetAttribute(ACXMLAttribSchemeID))

			r_lOldInsuranceFileCnt = CInt(oActionReturnElem.GetAttribute(ACXMLAttribRenewalOldInsuranceFileCnt))

			r_dtDueDateStart = CDate(oActionReturnElem.GetAttribute(ACXMLAttribRenewalDueDateStart))

			r_dtDueDateLimit = CDate(oActionReturnElem.GetAttribute(ACXMLAttribRenewalDueDateLimit))

			r_sClientCode = CStr(oActionReturnElem.GetAttribute(ACXMLAttribRenewalClientCode))

			r_sPolicyNo = CStr(oActionReturnElem.GetAttribute(ACXMLAttribRenewalPolicyNo))

			r_lInsurerID = CInt(oActionReturnElem.GetAttribute(ACXMLAttribRenewalInsurerID))

			r_lSuspensionLevel = CInt(oActionReturnElem.GetAttribute(ACXMLAttribRenewalSuspensionLevel))
			
			'Get the array
			UnformatXMLtoArray(oActionReturnElem, ACXMLAttribRenewalArray, r_vRenewalArray)
			
			oActionReturnElem = Nothing
			oActionReturn = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			
			' Log Error Message
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatActionReturnXMLRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatActionReturnXMLRenewals", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module
