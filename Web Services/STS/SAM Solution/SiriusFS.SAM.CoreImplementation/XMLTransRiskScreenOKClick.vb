Option Strict On

Imports SiriusFS.SAM.ServiceAgent.InternalSAMConstants
Imports SSP.Shared
Imports System.Xml

Friend Class XMLTransRiskScreenOKClick
    Inherits XMLSupport

    Friend Structure RiskScreenOKClickIn
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
        Dim lInsuranceFileCnt As Integer
        Dim vScreenDetailsArray As Object
        Dim vScreenValuesArray As Object
        Dim vRiskDetailsArray As Object
        Dim lTransactionType As Integer
        Dim bPostQuote As Boolean
        Dim dtCoverStartDate As Date
        Dim lPartyKey As Integer
    End Structure

    Friend Structure RiskScreenOKClickOut
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
        Dim sGISXMLDataset As String
        Dim vScreenValuesArray As Object
        Dim vRiskDetailsArray As Object
        Dim sReferReasons As String
        Dim sDeclineReasons As String
        Dim sMessages As String
        Dim lQuoteType As Integer
    End Structure

    Public Function SerializeRiskScreenOKClickIn(ByRef oTypeIn As RiskScreenOKClickIn) As String


        Dim vDataStore As Object = Nothing
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
        StoreItem(vDataStore, lLastPos, "lPartyCnt", oTypeIn.lPartyKey)

        Append(vDataStore, lLastPos, "</RiskScreenOKClickIn>")

        Dim vDataStoreObject As Object() = DirectCast(vDataStore, Object())

        'UPGRADE_WARNING: Couldn't resolve default property of object vDataStore. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        SerializeRiskScreenOKClickIn = Join(vDataStoreObject, "")

        Exit Function

    End Function


    Public Function DeserializeRiskScreenOKClickOut(ByVal sXML As String) As RiskScreenOKClickOut
        Dim oType As RiskScreenOKClickOut = Nothing
        Dim oXML As System.Xml.XmlDocument
        Dim oNode As System.Xml.XmlNode
        Dim oNodes As System.Xml.XmlNodeList
        Try

            oXML = New System.Xml.XmlDocument()


            Try
                oXML.LoadXml(sXML)

            Catch parseError As System.Xml.XmlException
                Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenOKClickOut, " + "XML Parse Error: " & " - " & parseError.Message)
            End Try

            oNodes = oXML.SelectNodes("RiskScreenOKClickOut/ERRORS")
            oType.Errors = ExportErrorArray(oNodes, oType.HasErrors, oType.HasInvalidDataErrors, oType.HasBusinessRuleErrors, oType.HasBackOfficeErrors, oType.HasInternalExceptionErrors)

            oNodes = oXML.SelectNodes("RiskScreenOKClickOut/WARNINGS/WARNING")
            oType.Warnings = ExportMessageArray(oNodes, oType.HasWarnings)
            oType.sGISXMLDataset = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sGISXMLDataset")), String.Empty)

            oNode = oXML.SelectSingleNode("RiskScreenOKClickOut/vScreenValuesArray")

            If Not oNode Is Nothing Then
                oType.vScreenValuesArray = ExportItemArray(oNode)
            End If
            oNode = oXML.SelectSingleNode("RiskScreenOKClickOut/vRiskDetailsArray")

            If Not oNode Is Nothing Then
                oType.vRiskDetailsArray = ExportItemArray(oNode)
            End If
            oType.sReferReasons = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sReferReasons")), String.Empty)
            oType.sDeclineReasons = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sDeclineReasons")), String.Empty)
            oType.sMessages = Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sMessages")), String.Empty)
            oType.lQuoteType = XmlSafeConvert.ToInt32(Cast.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/lQuoteType")), String.Empty), 0)

            oNode = Nothing
            oXML = Nothing

            Return oType

        Catch excep As System.Exception

            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenOKClickOut, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

End Class

