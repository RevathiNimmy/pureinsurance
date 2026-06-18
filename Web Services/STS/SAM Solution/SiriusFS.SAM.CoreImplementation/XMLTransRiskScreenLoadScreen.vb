Option Strict On

Imports SiriusFS.SAM.ServiceAgent.InternalSAMConstants
Imports SSP.Shared
Imports System.Xml

Friend Class XMLTransRiskScreenLoadScreen
    Inherits XMLSupport

    Friend Structure RiskScreenLoadRiskIn
        Dim iTask As Short
        Dim iSourceID As Short
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
    End Structure

    Friend Structure RiskScreenLoadRiskOut
        'UPGRADE_ISSUE: PMXMLErrorTypes object was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"'
        Dim Errors As PMXMLErrorTypes
        'UPGRADE_ISSUE: PMXMLWarningMsg object was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"'
        Dim Warnings As PMXMLWarningMsg
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
    End Structure

    Public Function SerializeRiskScreenLoadRiskIn(ByRef oTypeIn As RiskScreenLoadRiskIn) As String


        Dim vDataStore As Object = Nothing
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

        Append(vDataStore, lLastPos, "</RiskScreenLoadRiskIn>")

        Dim vDataStortObject As Object() = DirectCast(vDataStore, Object())

        'UPGRADE_WARNING: Couldn't resolve default property of object vDataStore. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        SerializeRiskScreenLoadRiskIn = Join(vDataStortObject, "")

        Exit Function


    End Function

    Public Function DeserializeRiskScreenLoadRiskOut(ByVal sXML As String) As RiskScreenLoadRiskOut


        Dim oType As RiskScreenLoadRiskOut = Nothing 'RiskScreenLoadRiskOut.CreateInstance()
        Dim oXML As System.Xml.XmlDocument
        Dim oNode As XmlNode
        Dim oNodes As XmlNodeList

        oXML = New System.Xml.XmlDocument()

        Try
            oXML.LoadXml(sXML)
        Catch parseError As System.Xml.XmlException
            Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenLoadRiskOut, " + "XML Parse Error: " & " - " & parseError.Message)
        End Try

        oNodes = oXML.SelectNodes("RiskScreenLoadRiskOut/ERRORS")


        oType.Errors = ExportErrorArray(oNodes, oType.HasErrors, oType.HasInvalidDataErrors, oType.HasBusinessRuleErrors, oType.HasBackOfficeErrors, oType.HasInternalExceptionErrors)

        oNodes = oXML.SelectNodes("RiskScreenLoadRiskOut/WARNINGS/WARNING")

        oType.Warnings = ExportMessageArray(oNodes, oType.HasWarnings)

        oType.lRiskId = XmlSafeConvert.ToInt32(Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/lRiskId")), String.Empty), 0)
        oType.sGISXMLDataset = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sGISXMLDataset")), String.Empty)
        oType.sMyOIKey = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sMyOIKey")), String.Empty)
        oType.sMyObjectName = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sMyObjectName")), String.Empty)
        oType.sParentOIKey = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sParentOIKey")), String.Empty)
        oType.sParentObjectName = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sParentObjectName")), String.Empty)
        oType.lPolicyLinkId = XmlSafeConvert.ToInt32(Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/lPolicyLinkId")), String.Empty), 0)
        oNode = oXML.SelectSingleNode("RiskScreenLoadRiskOut/vScreenValuesArray")

        If Not oNode Is Nothing Then
            oType.vScreenValuesArray = ExportItemArray(oNode)
        End If

        oNode = oXML.SelectSingleNode("RiskScreenLoadRiskOut/vRiskDetailsArray")

        If Not oNode Is Nothing Then
            oType.vRiskDetailsArray = ExportItemArray(oNode)
        End If


        oNode = oXML.SelectSingleNode("RiskScreenLoadRiskOut/vRiskTypeDetailsArray")

        If Not oNode Is Nothing Then
            oType.vRiskTypeDetailsArray = ExportItemArray(oNode)
        End If
        oType.bChildAddStatus = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/bChildAddStatus")))
        oType.bRiskAdded = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/bRiskAdded")))
        oType.bRiskCopied = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/bRiskCopied")))
        oType.sReferReasons = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sReferReasons")), String.Empty)
        oType.sDeclineReasons = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sDeclineReasons")), String.Empty)
        oType.sMessages = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sMessages")), String.Empty)
        oType.lQuoteType = XmlSafeConvert.ToInt32(Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/lQuoteType")), String.Empty), 0)

        oNode = Nothing
        oXML = Nothing

        'DeserializeRiskScreenLoadRiskOut = oType
        Return oType
        Exit Function




    End Function

End Class

