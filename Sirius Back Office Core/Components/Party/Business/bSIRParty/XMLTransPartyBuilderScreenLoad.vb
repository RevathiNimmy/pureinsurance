Option Strict On


Imports SSP.Shared
Imports System.Threading
Imports System.Xml

Friend Class XMLTransPartyBuilderScreenLoad
    Inherits XMLSupport

    Friend Structure PartyBuilderScreenLoadIn
        Dim nTask As Integer
        Dim nSourceID As Integer
        Dim nNavigate As Integer
        Dim nProcessMode As Integer
        Dim sTransactionType As String
        Dim dtEffectiveDate As Date
        Dim bSubScreen As Boolean
        Dim nScreenId As Integer
        Dim nRiskId As Integer
        Dim nRiskTypeId As Integer
        Dim sGisDataModelCode As String
        Dim nGISDataModelType As Integer
        Dim nObjectType As Integer
        Dim sGISXMLDataset As String
        Dim sMyOIKey As String
        Dim sMyObjectName As String
        Dim sParentOIKey As String
        Dim sParentObjectName As String
        Dim nPolicyLinkId As Integer
        Dim nInsuranceFolderCnt As Integer
        Dim nInsuranceFileCnt As Integer
        Dim oScreenDetailsArray As Object
        Dim oScreenValuesArray As Object
        Dim oRiskDetailsArray As Object
        Dim oRiskTypeDetailsArray As Object
        Dim nTransactionType As Integer
        Dim nProductId As Integer
        Dim nPartyCnt As Integer
        Dim nClaimID As Integer
        Dim bCopyRisk As Boolean
    End Structure

    Friend Structure PartyBuilderScreenLoadOut
        Dim bHasErrors As Boolean
        Dim bHasInvalidDataErrors As Boolean
        Dim bHasBusinessRuleErrors As Boolean
        Dim bHasBackOfficeErrors As Boolean
        Dim bHasInternalExceptionErrors As Boolean
        Dim bHasWarnings As Boolean
        Dim nRiskId As Integer
        Dim sGISXMLDataset As String
        Dim sMyOIKey As String
        Dim sMyObjectName As String
        Dim sParentOIKey As String
        Dim sParentObjectName As String
        Dim nPolicyLinkId As Integer
        Dim oScreenValuesArray As Object
        Dim oRiskDetailsArray As Object
        Dim oRiskTypeDetailsArray As Object
        Dim bChildAddStatus As Boolean
        Dim bRiskAdded As Boolean
        Dim bRiskCopied As Boolean
        Dim sReferReasons As String
        Dim sDeclineReasons As String
        Dim sMessages As String
        Dim nQuoteType As Integer
    End Structure
    ''' <summary>
    ''' Serialize PartyBuilder ScreenLoadIn
    ''' </summary>
    ''' <param name="r_oTypeIn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function SerializePartyBuilderScreenLoadIn(ByRef r_oTypeIn As PartyBuilderScreenLoadIn) As String
        Dim oDataStore As Object = Nothing
        Dim nLastPos As Integer
        Dim oDataStortObject As Object() = Nothing
        Try
            Append(oDataStore, nLastPos, "<RiskScreenLoadRiskIn>")

            StoreItem(oDataStore, nLastPos, "iTask", r_oTypeIn.nTask)
            StoreItem(oDataStore, nLastPos, "iSourceID", r_oTypeIn.nSourceID)
            StoreItem(oDataStore, nLastPos, "lNavigate", r_oTypeIn.nNavigate)
            StoreItem(oDataStore, nLastPos, "lProcessMode", r_oTypeIn.nProcessMode)
            StoreItem(oDataStore, nLastPos, "sTransactionType", r_oTypeIn.sTransactionType)
            StoreItem(oDataStore, nLastPos, "dtEffectiveDate", r_oTypeIn.dtEffectiveDate)
            StoreItem(oDataStore, nLastPos, "bSubScreen", r_oTypeIn.bSubScreen)
            StoreItem(oDataStore, nLastPos, "lScreenId", r_oTypeIn.nScreenId)
            StoreItem(oDataStore, nLastPos, "lRiskId", r_oTypeIn.nRiskId)
            StoreItem(oDataStore, nLastPos, "lRiskTypeId", r_oTypeIn.nRiskTypeId)
            StoreItem(oDataStore, nLastPos, "sGisDataModelCode", r_oTypeIn.sGisDataModelCode)
            StoreItem(oDataStore, nLastPos, "lGISDataModelType", r_oTypeIn.nGISDataModelType)
            StoreItem(oDataStore, nLastPos, "lObjectType", r_oTypeIn.nObjectType)
            StoreItem(oDataStore, nLastPos, "sGISXMLDataset", r_oTypeIn.sGISXMLDataset)
            StoreItem(oDataStore, nLastPos, "sMyOIKey", r_oTypeIn.sMyOIKey)
            StoreItem(oDataStore, nLastPos, "sMyObjectName", r_oTypeIn.sMyObjectName)
            StoreItem(oDataStore, nLastPos, "sParentOIKey", r_oTypeIn.sParentOIKey)
            StoreItem(oDataStore, nLastPos, "sParentObjectName", r_oTypeIn.sParentObjectName)
            StoreItem(oDataStore, nLastPos, "lPolicyLinkId", r_oTypeIn.nPolicyLinkId)
            StoreItem(oDataStore, nLastPos, "lInsuranceFolderCnt", r_oTypeIn.nInsuranceFolderCnt)
            StoreItem(oDataStore, nLastPos, "lInsuranceFileCnt", r_oTypeIn.nInsuranceFileCnt)
            StoreItemArray(oDataStore, nLastPos, "vScreenDetailsArray", r_oTypeIn.oScreenDetailsArray)
            StoreItemArray(oDataStore, nLastPos, "vScreenValuesArray", r_oTypeIn.oScreenValuesArray)
            StoreItemArray(oDataStore, nLastPos, "vRiskDetailsArray", r_oTypeIn.oRiskDetailsArray)
            StoreItemArray(oDataStore, nLastPos, "vRiskTypeDetailsArray", r_oTypeIn.oRiskTypeDetailsArray)
            StoreItem(oDataStore, nLastPos, "lTransactionType", r_oTypeIn.nTransactionType)
            StoreItem(oDataStore, nLastPos, "lProductId", r_oTypeIn.nProductId)
            StoreItem(oDataStore, nLastPos, "lPartyCnt", r_oTypeIn.nPartyCnt)
            StoreItem(oDataStore, nLastPos, "lClaimID", r_oTypeIn.nClaimID)
            StoreItem(oDataStore, nLastPos, "bCopyRisk", r_oTypeIn.bCopyRisk)

            Append(oDataStore, nLastPos, "</RiskScreenLoadRiskIn>")
            oDataStortObject = DirectCast(oDataStore, Object())

        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "SerializeRiskScreenLoadRiskIn" & ex.Message)
        End Try

        ' This Need to be test after implementation 
        SerializePartyBuilderScreenLoadIn = String.Join("", oDataStortObject)


    End Function

    ''' <summary>
    ''' Deserialize PartyBuilder ScreenLoadOut
    ''' </summary>
    ''' <param name="r_sXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function DeserializePartyBuilderScreenLoadOut(ByVal r_sXML As String) As PartyBuilderScreenLoadOut
        Dim oType As PartyBuilderScreenLoadOut = Nothing
        Dim oXML As XmlDocument
        Dim oNode As XmlNode
        Dim oNodes As XmlNodeList
        Try
            oXML = New XmlDocument

            Try
                oXML.LoadXml(r_sXML)
            Catch xmlParseException As XmlException
                gPMFunctions.RaiseError(CStr(kPMXMLParseError), "DeserializePartyBuilderScreenLoadOut. Error: XML Parse Error: " & xmlParseException.Message)
            End Try
            oNodes = oXML.SelectNodes("RiskScreenLoadRiskOut/ERRORS")
            oNodes = oXML.SelectNodes("RiskScreenLoadRiskOut/WARNINGS/WARNING")
            oType.nRiskId = Convert.ToInt32(Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/lRiskId"))))
            oType.sGISXMLDataset = Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sGISXMLDataset")))
            oType.sMyOIKey = Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sMyOIKey")))
            oType.sMyObjectName = Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sMyObjectName")))
            oType.sParentOIKey = Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sParentOIKey")))
            oType.sParentObjectName = Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sParentObjectName")))
            oType.nPolicyLinkId = Convert.ToInt32(Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/lPolicyLinkId"))))
            oNode = oXML.SelectSingleNode("RiskScreenLoadRiskOut/vScreenValuesArray")

            If Not oNode Is Nothing Then
                oType.oScreenValuesArray = ExportItemArray(oNode)
            End If
            oNode = oXML.SelectSingleNode("RiskScreenLoadRiskOut/vRiskDetailsArray")
            If Not oNode Is Nothing Then
                oType.oRiskDetailsArray = ExportItemArray(oNode)
            End If
            oNode = oXML.SelectSingleNode("RiskScreenLoadRiskOut/vRiskTypeDetailsArray")

            If Not oNode Is Nothing Then
                oType.oRiskTypeDetailsArray = ExportItemArray(oNode)
            End If
            oType.bChildAddStatus = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/bChildAddStatus")))
            oType.bRiskAdded = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/bRiskAdded")))
            oType.bRiskCopied = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/bRiskCopied")))
            oType.sReferReasons = Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sReferReasons")))
            oType.sDeclineReasons = Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sDeclineReasons")))
            oType.sMessages = Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/sMessages")))
            oType.nQuoteType = Convert.ToInt32(Convert.ToString(ExportItem(oXML.SelectSingleNode("RiskScreenLoadRiskOut/lQuoteType"))))
            oNode = Nothing
            oXML = Nothing
        Catch ex As Exception
            gPMFunctions.RaiseError(CStr(PMEReturnCode.PMError), "DeserializePartyBuilderScreenLoadOut" & ex.Message)
        End Try
        DeserializePartyBuilderScreenLoadOut = oType
    End Function

End Class

