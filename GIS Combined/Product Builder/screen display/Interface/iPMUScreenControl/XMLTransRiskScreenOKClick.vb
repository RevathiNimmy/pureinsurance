Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
Imports SharedFiles
Module XMLTransRiskScreenOKClick
	
	Public Structure RiskScreenOKClickIn
		Dim iTask As Integer
		Dim iSourceID As Integer
		Dim lNavigate As Integer
		Dim lProcessMode As Integer
		Dim sTransactionType As String
		Dim dtEffectiveDate As Date
		Dim bSubScreen As Boolean
		Dim lScreenId As Integer
		Dim lRiskId As Integer
		Dim lRiskTypeId As Integer
		Dim sGisDataModelCode As String
		Dim lGISDataModelType As Integer
		Dim lObjectType As Integer
		Dim sGISXMLDataset As String
		Dim sMyOIKey As String
		Dim sMyObjectName As String
		Dim sParentOIKey As String
		Dim sParentObjectName As String
		Dim lPolicyLinkId As Integer
		Dim lInsuranceFileCnt As Integer
		Dim vScreenDetailsArray As Object
		Dim vScreenValuesArray As Object
		Dim vRiskDetailsArray As Object
		Dim lTransactionType As Integer
		Dim bPostQuote As Boolean
		Dim dtCoverStartDate As Date
		Dim lPartyCnt As Integer
		Dim dtPolicyStartDate As Date
		Dim dtPolicyEndDate As Date
		Dim lAgentCnt As Integer
		Dim lRiskGroupId As Integer
		Dim lRiskCodeId As Integer
		Dim lCountryId As Integer
		Public Shared Function CreateInstance() As RiskScreenOKClickIn
			Dim result As New RiskScreenOKClickIn
			result.sTransactionType = String.Empty
			result.sGisDataModelCode = String.Empty
			result.sGISXMLDataset = String.Empty
			result.sMyOIKey = String.Empty
			result.sMyObjectName = String.Empty
			result.sParentOIKey = String.Empty
			result.sParentObjectName = String.Empty
			Return result
		End Function
	End Structure
	
	Public Structure RiskScreenOKClickOut
		Dim Errors As XMLSupport.PMXMLErrorTypes
		Dim Warnings As XMLSupport.PMXMLWarningMsg
		Dim HasErrors As Boolean
		Dim HasInvalidDataErrors As Boolean
		Dim HasBusinessRuleErrors As Boolean
		Dim HasBackOfficeErrors As Boolean
		Dim HasInternalExceptionErrors As Boolean
		Dim HasWarnings As Boolean
		Dim sGISXMLDataset As String
		Dim vScreenValuesArray As Object
		Dim vRiskDetailsArray As Object
		Dim sReferReasons As String
		Dim sDeclineReasons As String
		Dim sMessages As String
		Dim lQuoteType As Integer
		Public Shared Function CreateInstance() As RiskScreenOKClickOut
			Dim result As New RiskScreenOKClickOut
			result.Errors = PMXMLErrorTypes.CreateInstance()
			result.sGISXMLDataset = String.Empty
			result.sReferReasons = String.Empty
			result.sDeclineReasons = String.Empty
			result.sMessages = String.Empty
			Return result
		End Function
	End Structure
	
	Public Function SerializeRiskScreenOKClickIn(ByRef oTypeIn As RiskScreenOKClickIn) As String
		Try 
			
			Dim vDataStore As Object
			Dim lLastPos As Integer
			

			Append(vDataStore, lLastPos, "<RiskScreenOKClickIn>")
			

			StoreItem(vDataStore, lLastPos, "iTask", oTypeIn.iTask)

			StoreItem(vDataStore, lLastPos, "iSourceID", oTypeIn.iSourceID)

			StoreItem(vDataStore, lLastPos, "lNavigate", oTypeIn.lNavigate)

			StoreItem(vDataStore, lLastPos, "lProcessMode", oTypeIn.lProcessMode)

			StoreItem(vDataStore, lLastPos, "sTransactionType", oTypeIn.sTransactionType)

			StoreItem(vDataStore, lLastPos, "dtEffectiveDate", oTypeIn.dtEffectiveDate)

			StoreItem(vDataStore, lLastPos, "bSubScreen", oTypeIn.bSubScreen)

			StoreItem(vDataStore, lLastPos, "lScreenId", oTypeIn.lScreenId)

			StoreItem(vDataStore, lLastPos, "lRiskId", oTypeIn.lRiskId)

			StoreItem(vDataStore, lLastPos, "lRiskTypeId", oTypeIn.lRiskTypeId)

			StoreItem(vDataStore, lLastPos, "sGisDataModelCode", oTypeIn.sGisDataModelCode)

			StoreItem(vDataStore, lLastPos, "lGISDataModelType", oTypeIn.lGISDataModelType)

			StoreItem(vDataStore, lLastPos, "lObjectType", oTypeIn.lObjectType)

			StoreItem(vDataStore, lLastPos, "sGISXMLDataset", oTypeIn.sGISXMLDataset)

			StoreItem(vDataStore, lLastPos, "sMyOIKey", oTypeIn.sMyOIKey)

			StoreItem(vDataStore, lLastPos, "sMyObjectName", oTypeIn.sMyObjectName)

			StoreItem(vDataStore, lLastPos, "sParentOIKey", oTypeIn.sParentOIKey)

			StoreItem(vDataStore, lLastPos, "sParentObjectName", oTypeIn.sParentObjectName)

			StoreItem(vDataStore, lLastPos, "lPolicyLinkId", oTypeIn.lPolicyLinkId)

			StoreItem(vDataStore, lLastPos, "lInsuranceFileCnt", oTypeIn.lInsuranceFileCnt)

			StoreItemArray(vDataStore, lLastPos, "vScreenDetailsArray", oTypeIn.vScreenDetailsArray)

			StoreItemArray(vDataStore, lLastPos, "vScreenValuesArray", oTypeIn.vScreenValuesArray)

			StoreItemArray(vDataStore, lLastPos, "vRiskDetailsArray", oTypeIn.vRiskDetailsArray)

			StoreItem(vDataStore, lLastPos, "lTransactionType", oTypeIn.lTransactionType)

			StoreItem(vDataStore, lLastPos, "bPostQuote", oTypeIn.bPostQuote)

			StoreItem(vDataStore, lLastPos, "dtCoverStartDate", oTypeIn.dtCoverStartDate)
			

			StoreItem(vDataStore, lLastPos, "lPartyCnt", oTypeIn.lPartyCnt)

			StoreItem(vDataStore, lLastPos, "dtPolicyStartDate", oTypeIn.dtPolicyStartDate)

			StoreItem(vDataStore, lLastPos, "dtPolicyEndDate", oTypeIn.dtPolicyEndDate)

			StoreItem(vDataStore, lLastPos, "lAgentCnt", oTypeIn.lAgentCnt)

			StoreItem(vDataStore, lLastPos, "lRiskGroupId", oTypeIn.lRiskGroupId)

			StoreItem(vDataStore, lLastPos, "lRiskCodeId", oTypeIn.lRiskCodeId)

			StoreItem(vDataStore, lLastPos, "lCountryId", oTypeIn.lCountryId)
			

			Append(vDataStore, lLastPos, "</RiskScreenOKClickIn>")
			

            'Developer Guide No.271
            Dim tempArray(vDataStore.Length - 1) As String
            vDataStore.CopyTo(tempArray, 0)
            Return String.Concat(tempArray)

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", SerializeRiskScreenOKClickIn, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function SerializeRiskScreenOKClickOut(ByRef oTypeOut As RiskScreenOKClickOut) As String
        Try

            Dim vDataStore As Object
            Dim lLastPos As Integer


            Append(vDataStore, lLastPos, "<RiskScreenOKClickOut>")


            StoreErrorArray(vDataStore, lLastPos, oTypeOut.Errors)

            StoreMessageArray(vDataStore, lLastPos, oTypeOut.Warnings)

            StoreItem(vDataStore, lLastPos, "sGISXMLDataset", oTypeOut.sGISXMLDataset)

            StoreItemArray(vDataStore, lLastPos, "vScreenValuesArray", oTypeOut.vScreenValuesArray)

            StoreItemArray(vDataStore, lLastPos, "vRiskDetailsArray", oTypeOut.vRiskDetailsArray)

            StoreItem(vDataStore, lLastPos, "sReferReasons", oTypeOut.sReferReasons)

            StoreItem(vDataStore, lLastPos, "sDeclineReasons", oTypeOut.sDeclineReasons)

            StoreItem(vDataStore, lLastPos, "sMessages", oTypeOut.sMessages)

            StoreItem(vDataStore, lLastPos, "lQuoteType", oTypeOut.lQuoteType)


            Append(vDataStore, lLastPos, "</RiskScreenOKClickOut>")


            'Developer Guide No.271
            Dim tempArray(vDataStore.Length - 1) As String
            vDataStore.CopyTo(tempArray, 0)
            Return String.Concat(tempArray)

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", SerializeRiskScreenOKClickOut, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function DeserializeRiskScreenOKClickIn(ByVal sXML As String) As RiskScreenOKClickIn
        Try

            Dim oType As RiskScreenOKClickIn = RiskScreenOKClickIn.CreateInstance()
            Dim oXML As XmlDocument
            Dim oNode As XmlNode

            oXML = New XmlDocument()

            'Developer Guide No. 22(no solution)
            'oXML.validateOnParse = True

            'Developer Guide No. 22(no solution)
            'oXML.async = False

            'Developer Guide No. 22(no solution)
            'oXML.setProperty("NewParser", True)

            Try
                oXML.LoadXml(sXML)



            Catch parseError As System.Xml.XmlException


                'Developer Guide No. 22(no solution)
                'Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenOKClickIn, " + "XML Parse Error: " & parseError.errorCode & " - " & parseError.Message)
            End Try


            oType.iTask = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/iTask")))

            oType.iSourceID = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/iSourceID")))

            oType.lNavigate = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lNavigate")))

            oType.lProcessMode = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lProcessMode")))

            oType.sTransactionType = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sTransactionType")))

            oType.dtEffectiveDate = CDate(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/dtEffectiveDate")))

            oType.bSubScreen = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/bSubScreen")))

            oType.lScreenId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lScreenId")))

            oType.lRiskId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lRiskId")))

            oType.lRiskTypeId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lRiskTypeId")))

            oType.sGisDataModelCode = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sGisDataModelCode")))

            oType.lGISDataModelType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lGISDataModelType")))

            oType.lObjectType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lObjectType")))

            oType.sGISXMLDataset = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sGISXMLDataset")))

            oType.sMyOIKey = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sMyOIKey")))

            oType.sMyObjectName = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sMyObjectName")))

            oType.sParentOIKey = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sParentOIKey")))

            oType.sParentObjectName = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sParentObjectName")))

            oType.lPolicyLinkId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lPolicyLinkId")))

            oType.lInsuranceFileCnt = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lInsuranceFileCnt")))

            oType.lPartyCnt = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lPartyCnt")))

            oType.dtPolicyStartDate = CDate(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/dtPolicyStartDate")))

            oType.dtPolicyEndDate = CDate(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/dtPolicyEndDate")))

            oType.lAgentCnt = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lAgentCnt")))

            oType.lRiskGroupId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lRiskGroupId")))

            oType.lRiskCodeId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lRiskCodeId")))

            oType.lCountryId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lCountryId")))

            oNode = oXML.SelectSingleNode("RiskScreenOKClickIn/vScreenDetailsArray")

            If Not (oNode Is Nothing) Then

                oType.vScreenDetailsArray = ExportItemArray(oNode)
            End If

            oNode = oXML.SelectSingleNode("RiskScreenOKClickIn/vScreenValuesArray")

            If Not (oNode Is Nothing) Then

                oType.vScreenValuesArray = ExportItemArray(oNode)
            End If

            oNode = oXML.SelectSingleNode("RiskScreenOKClickIn/vRiskDetailsArray")

            If Not (oNode Is Nothing) Then

                oType.vRiskDetailsArray = ExportItemArray(oNode)
            End If

            oType.lTransactionType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lTransactionType")))

            oType.bPostQuote = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/bPostQuote")))

            oType.dtCoverStartDate = CDate(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/dtCoverStartDate")))

            oNode = Nothing
            oXML = Nothing


            Return oType

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenOKClickIn, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function DeserializeRiskScreenOKClickOut(ByVal sXML As String) As RiskScreenOKClickOut
        Try

            Dim oType As RiskScreenOKClickOut = RiskScreenOKClickOut.CreateInstance()
            Dim oXML As XmlDocument
            Dim oNode As XmlNode
            Dim oNodes As XmlNodeList

            oXML = New XmlDocument()

            'Developer Guide No. 22(no solution)
            'oXML.validateOnParse = True

            'Developer Guide No. 22(no solution)
            'oXML.async = False

            'Developer Guide No. 22(no solution)
            'oXML.setProperty("NewParser", True)

            Try
                oXML.LoadXml(sXML)



            Catch parseError As System.Xml.XmlException


                'Developer Guide No. 22(no solution)
                'Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenOKClickOut, " + "XML Parse Error: " & parseError.errorCode & " - " & parseError.Message)
            End Try

            oNodes = oXML.SelectNodes("RiskScreenOKClickOut/ERRORS")
            oType.Errors = ExportErrorArray(oNodes, oType.HasErrors, oType.HasInvalidDataErrors, oType.HasBusinessRuleErrors, oType.HasBackOfficeErrors, oType.HasInternalExceptionErrors)

            oNodes = oXML.SelectNodes("RiskScreenOKClickOut/WARNINGS/WARNING")
            oType.Warnings = ExportMessageArray(oNodes, oType.HasWarnings)

            oType.sGISXMLDataset = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sGISXMLDataset")))

            oNode = oXML.SelectSingleNode("RiskScreenOKClickOut/vScreenValuesArray")

            If Not (oNode Is Nothing) Then

                oType.vScreenValuesArray = ExportItemArray(oNode)
            End If

            oNode = oXML.SelectSingleNode("RiskScreenOKClickOut/vRiskDetailsArray")

            If Not (oNode Is Nothing) Then

                oType.vRiskDetailsArray = ExportItemArray(oNode)
            End If

            oType.sReferReasons = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sReferReasons")))

            oType.sDeclineReasons = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sDeclineReasons")))

            oType.sMessages = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sMessages")))

            oType.lQuoteType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/lQuoteType")))

            oNode = Nothing
            oXML = Nothing


            Return oType

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenOKClickOut, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function
End Module