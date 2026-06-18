Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
Imports SharedFiles
Module XMLTransRiskScreenLoadRisk
	
	Public Structure RiskScreenLoadRiskIn
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
		Dim lInsuranceFolderCnt As Integer
		Dim lInsuranceFileCnt As Integer
		Dim vScreenDetailsArray As Object
		Dim vScreenValuesArray As Object
		Dim vRiskDetailsArray As Object
		Dim vRiskTypeDetailsArray As Object
		Dim lTransactionType As Integer
		Dim lProductId As Integer
		Dim lPartyCnt As Integer
		Dim lClaimID As Integer
		Dim bCopyRisk As Boolean
		Dim lCaseID As Integer
		Public Shared Function CreateInstance() As RiskScreenLoadRiskIn
			Dim result As New RiskScreenLoadRiskIn
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
	
	Public Structure RiskScreenLoadRiskOut
		Dim Errors As XMLSupport.PMXMLErrorTypes
		Dim Warnings As XMLSupport.PMXMLWarningMsg
		Dim HasErrors As Boolean
		Dim HasInvalidDataErrors As Boolean
		Dim HasBusinessRuleErrors As Boolean
		Dim HasBackOfficeErrors As Boolean
		Dim HasInternalExceptionErrors As Boolean
		Dim HasWarnings As Boolean
		Dim lRiskId As Integer
		Dim sGISXMLDataset As String
		Dim sMyOIKey As String
		Dim sMyObjectName As String
		Dim sParentOIKey As String
		Dim sParentObjectName As String
		Dim lPolicyLinkId As Integer
		Dim vScreenValuesArray As Object
		Dim vRiskDetailsArray As Object
		Dim vRiskTypeDetailsArray As Object
		Dim bChildAddStatus As Boolean
		Dim bRiskAdded As Boolean
		Dim bRiskCopied As Boolean
		Dim sReferReasons As String
		Dim sDeclineReasons As String
		Dim sMessages As String
		Dim lQuoteType As Integer
		Dim lCaseID As Integer
		Public Shared Function CreateInstance() As RiskScreenLoadRiskOut
			Dim result As New RiskScreenLoadRiskOut
			result.Errors = PMXMLErrorTypes.CreateInstance()
			result.sGISXMLDataset = String.Empty
			result.sMyOIKey = String.Empty
			result.sMyObjectName = String.Empty
			result.sParentOIKey = String.Empty
			result.sParentObjectName = String.Empty
			result.sReferReasons = String.Empty
			result.sDeclineReasons = String.Empty
			result.sMessages = String.Empty
			Return result
		End Function
	End Structure
	
	Public Function SerializeRiskScreenLoadRiskIn(ByRef oTypeIn As RiskScreenLoadRiskIn) As String
		Try 
			
			Dim vDataStore As Object
			Dim lLastPos As Integer



			Append(vDataStore, lLastPos, "<RiskScreenLoadRiskIn>")
			

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

			StoreItem(vDataStore, lLastPos, "lInsuranceFolderCnt", oTypeIn.lInsuranceFolderCnt)

			StoreItem(vDataStore, lLastPos, "lInsuranceFileCnt", oTypeIn.lInsuranceFileCnt)

			StoreItemArray(vDataStore, lLastPos, "vScreenDetailsArray", oTypeIn.vScreenDetailsArray)

			StoreItemArray(vDataStore, lLastPos, "vScreenValuesArray", oTypeIn.vScreenValuesArray)

			StoreItemArray(vDataStore, lLastPos, "vRiskDetailsArray", oTypeIn.vRiskDetailsArray)

			StoreItemArray(vDataStore, lLastPos, "vRiskTypeDetailsArray", oTypeIn.vRiskTypeDetailsArray)

			StoreItem(vDataStore, lLastPos, "lTransactionType", oTypeIn.lTransactionType)

			StoreItem(vDataStore, lLastPos, "lProductId", oTypeIn.lProductId)

			StoreItem(vDataStore, lLastPos, "lPartyCnt", oTypeIn.lPartyCnt)

			StoreItem(vDataStore, lLastPos, "lClaimID", oTypeIn.lClaimID)

			StoreItem(vDataStore, lLastPos, "bCopyRisk", oTypeIn.bCopyRisk)

			StoreItem(vDataStore, lLastPos, "lCaseID", oTypeIn.lCaseID)
			

			Append(vDataStore, lLastPos, "</RiskScreenLoadRiskIn>")
			

            'Developer Guide No.271
            'Return String.Concat(vDataStore)
            Dim tempArray(vDataStore.Length - 1) As String
            vDataStore.CopyTo(tempArray, 0)
            Return String.Concat(tempArray)

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", SerializeRiskScreenLoadRiskIn, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function SerializeRiskScreenLoadRiskOut(ByRef oTypeOut As RiskScreenLoadRiskOut) As String
        Try

            Dim vDataStore As Object
            Dim lLastPos As Integer


            Append(vDataStore, lLastPos, "<RiskScreenLoadRiskOut>")


            StoreErrorArray(vDataStore, lLastPos, oTypeOut.Errors)

            StoreMessageArray(vDataStore, lLastPos, oTypeOut.Warnings)

            StoreItem(vDataStore, lLastPos, "lRiskId", oTypeOut.lRiskId)

            StoreItem(vDataStore, lLastPos, "sGISXMLDataset", oTypeOut.sGISXMLDataset)

            StoreItem(vDataStore, lLastPos, "sMyOIKey", oTypeOut.sMyOIKey)

            StoreItem(vDataStore, lLastPos, "sMyObjectName", oTypeOut.sMyObjectName)

            StoreItem(vDataStore, lLastPos, "sParentOIKey", oTypeOut.sParentOIKey)

            StoreItem(vDataStore, lLastPos, "sParentObjectName", oTypeOut.sParentObjectName)

            StoreItem(vDataStore, lLastPos, "lPolicyLinkId", oTypeOut.lPolicyLinkId)

            StoreItemArray(vDataStore, lLastPos, "vScreenValuesArray", oTypeOut.vScreenValuesArray)

            StoreItemArray(vDataStore, lLastPos, "vRiskDetailsArray", oTypeOut.vRiskDetailsArray)

            StoreItemArray(vDataStore, lLastPos, "vRiskTypeDetailsArray", oTypeOut.vRiskTypeDetailsArray)

            StoreItem(vDataStore, lLastPos, "bChildAddStatus", oTypeOut.bChildAddStatus)

            StoreItem(vDataStore, lLastPos, "bRiskAdded", oTypeOut.bRiskAdded)

            StoreItem(vDataStore, lLastPos, "bRiskCopied", oTypeOut.bRiskCopied)

            StoreItem(vDataStore, lLastPos, "sReferReasons", oTypeOut.sReferReasons)

            StoreItem(vDataStore, lLastPos, "sDeclineReasons", oTypeOut.sDeclineReasons)

            StoreItem(vDataStore, lLastPos, "sMessages", oTypeOut.sMessages)

            StoreItem(vDataStore, lLastPos, "lQuoteType", oTypeOut.lQuoteType)

            StoreItem(vDataStore, lLastPos, "lCaseID", oTypeOut.lCaseID)


            Append(vDataStore, lLastPos, "</RiskScreenLoadRiskOut>")


            'Developer Guide No.271
            'Return String.Concat(vDataStore)
            Dim tempArray(vDataStore.Length - 1) As String
            vDataStore.CopyTo(tempArray, 0)
            Return String.Concat(tempArray)

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", SerializeRiskScreenLoadRiskOut, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function DeserializeRiskScreenLoadRiskIn(ByVal sXML As String) As RiskScreenLoadRiskIn
        Try

            Dim oType As RiskScreenLoadRiskIn = RiskScreenLoadRiskIn.CreateInstance()
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
                'Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenLoadRiskIn, " + "XML Parse Error: " & parseError.errorCode & " - " & parseError.Message)
            End Try


            oType.iTask = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/iTask")))

            oType.iSourceID = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/iSourceID")))

            oType.lNavigate = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lNavigate")))

            oType.lProcessMode = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lProcessMode")))

            oType.sTransactionType = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/sTransactionType")))

            oType.dtEffectiveDate = CDate(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/dtEffectiveDate")))

            oType.bSubScreen = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/bSubScreen")))

            oType.lScreenId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lScreenId")))

            oType.lRiskId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lRiskId")))

            oType.lRiskTypeId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lRiskTypeId")))

            oType.sGisDataModelCode = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/sGisDataModelCode")))

            oType.lGISDataModelType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lGISDataModelType")))

            oType.lObjectType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lObjectType")))

            oType.sGISXMLDataset = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/sGISXMLDataset")))

            oType.sMyOIKey = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/sMyOIKey")))

            oType.sMyObjectName = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/sMyObjectName")))

            oType.sParentOIKey = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/sParentOIKey")))

            oType.sParentObjectName = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/sParentObjectName")))

            oType.lPolicyLinkId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lPolicyLinkId")))

            oType.lInsuranceFolderCnt = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lInsuranceFolderCnt")))

            oType.lInsuranceFileCnt = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lInsuranceFileCnt")))

            oNode = oXML.SelectSingleNode("RiskScreenLoadRiskIn/vScreenDetailsArray")

            If Not (oNode Is Nothing) Then

                oType.vScreenDetailsArray = ExportItemArray(oNode)
            End If

            oNode = oXML.SelectSingleNode("RiskScreenLoadRiskIn/vScreenValuesArray")

            If Not (oNode Is Nothing) Then

                oType.vScreenValuesArray = ExportItemArray(oNode)
            End If

            oNode = oXML.SelectSingleNode("RiskScreenLoadRiskIn/vRiskDetailsArray")

            If Not (oNode Is Nothing) Then

                oType.vRiskDetailsArray = ExportItemArray(oNode)
            End If

            oNode = oXML.SelectSingleNode("RiskScreenLoadRiskIn/vRiskTypeDetailsArray")

            If Not (oNode Is Nothing) Then

                oType.vRiskTypeDetailsArray = ExportItemArray(oNode)
            End If

            oType.lTransactionType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lTransactionType")))

            oType.lProductId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lProductId")))

            oType.lPartyCnt = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lPartyCnt")))

            oType.lClaimID = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lClaimID")))

            oType.bCopyRisk = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/bCopyRisk")))

            oType.lCaseID = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskIn/lCaseID")))

            oNode = Nothing
            oXML = Nothing


            Return oType

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenLoadRiskIn, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function DeserializeRiskScreenLoadRiskOut(ByVal sXML As String) As RiskScreenLoadRiskOut
        Try

            Dim oType As RiskScreenLoadRiskOut = RiskScreenLoadRiskOut.CreateInstance()
            Dim oXML As XmlDocument
            Dim oNode As XmlNode
            Dim oNodes As XmlNodeList

            oXML = New XmlDocument()

            'Developer Guide No. 22(no solution)
            'oXML.validateOnParse = True

            ''Developer Guide No. 22(no solution)
            'oXML.async = False

            ''Developer Guide No. 22(no solution)
            'oXML.setProperty("NewParser", True)

            Try
                oXML.LoadXml(sXML)



            Catch parseError As System.Xml.XmlException


                'Developer Guide No. 22(no solution)
                'Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenLoadRiskOut, " + "XML Parse Error: " & parseError.errorCode & " - " & parseError.Message)
            End Try

            oNodes = oXML.SelectNodes("RiskScreenLoadRiskOut/ERRORS")
            oType.Errors = ExportErrorArray(oNodes, oType.HasErrors, oType.HasInvalidDataErrors, oType.HasBusinessRuleErrors, oType.HasBackOfficeErrors, oType.HasInternalExceptionErrors)

            oNodes = oXML.SelectNodes("RiskScreenLoadRiskOut/WARNINGS/WARNING")
            oType.Warnings = ExportMessageArray(oNodes, oType.HasWarnings)

            oType.lRiskId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/lRiskId")))

            oType.sGISXMLDataset = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sGISXMLDataset")))

            oType.sMyOIKey = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sMyOIKey")))

            oType.sMyObjectName = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sMyObjectName")))

            oType.sParentOIKey = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sParentOIKey")))

            oType.sParentObjectName = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sParentObjectName")))

            oType.lPolicyLinkId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/lPolicyLinkId")))

            oNode = oXML.SelectSingleNode("RiskScreenLoadRiskOut/vScreenValuesArray")

            If Not (oNode Is Nothing) Then

                oType.vScreenValuesArray = ExportItemArray(oNode)
            End If

            oNode = oXML.SelectSingleNode("RiskScreenLoadRiskOut/vRiskDetailsArray")

            If Not (oNode Is Nothing) Then

                oType.vRiskDetailsArray = ExportItemArray(oNode)
            End If

            oNode = oXML.SelectSingleNode("RiskScreenLoadRiskOut/vRiskTypeDetailsArray")

            If Not (oNode Is Nothing) Then

                oType.vRiskTypeDetailsArray = ExportItemArray(oNode)
            End If

            oType.bChildAddStatus = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/bChildAddStatus")))

            oType.bRiskAdded = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/bRiskAdded")))

            oType.bRiskCopied = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/bRiskCopied")))

            oType.sReferReasons = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sReferReasons")))

            oType.sDeclineReasons = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sDeclineReasons")))

            oType.sMessages = CStr(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sMessages")))

            oType.lQuoteType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/lQuoteType")))

            oType.lCaseID = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/lCaseID")))

            oNode = Nothing
            oXML = Nothing


            Return oType

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenLoadRiskOut, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function
End Module