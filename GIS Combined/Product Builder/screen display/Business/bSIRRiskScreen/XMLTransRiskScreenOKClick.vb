Option Strict Off
Option Explicit On
Imports System.Xml
Imports SSP.Shared
'developer guide no. 129
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
        Dim lRiskId As Object
        Dim lRiskTypeId As Integer
        Dim sGisDataModelCode As Object
        Dim lGISDataModelType As Integer
        Dim lObjectType As Integer
        Dim sGISXMLDataset As Object
        Dim sMyOIKey As Object
        Dim sMyObjectName As Object
        Dim sParentOIKey As Object
        Dim sParentObjectName As Object
        Dim lPolicyLinkId As Object
        Dim lInsuranceFileCnt As Object
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


            StoreItem(vDataStore, lLastPos, "lPartyCnt", oTypeIn.lPartyCnt)

            StoreItem(vDataStore, lLastPos, "dtPolicyStartDate", oTypeIn.dtPolicyStartDate)

            StoreItem(vDataStore, lLastPos, "dtPolicyEndDate", oTypeIn.dtPolicyEndDate)

            StoreItem(vDataStore, lLastPos, "lAgentCnt", oTypeIn.lAgentCnt)

            StoreItem(vDataStore, lLastPos, "lRiskGroupId", oTypeIn.lRiskGroupId)

            StoreItem(vDataStore, lLastPos, "lRiskCodeId", oTypeIn.lRiskCodeId)

            StoreItem(vDataStore, lLastPos, "lCountryId", oTypeIn.lCountryId)


            Append(vDataStore, lLastPos, "</RiskScreenOKClickIn>")


            'developer guide no 271. 
            Dim tempArray(vDataStore.Length - 1) As String
            vDataStore.CopyTo(CType(tempArray, Object), 0)
            Return String.Concat(tempArray)

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", SerializeRiskScreenOKClickIn, " + "Error: " & Informations.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function SerializeRiskScreenOKClickOut(ByRef oTypeOut As RiskScreenOKClickOut) As String
        Try

            Dim vDataStore As Object = Nothing
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


            'developer guide no 271. 
            Dim tempArray(vDataStore.Length - 1) As String
            vDataStore.CopyTo(CType(tempArray, Object), 0)
            Return String.Concat(tempArray)

        Catch excep As System.Exception

            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", SerializeRiskScreenOKClickOut, " + "Error: " & Informations.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function DeserializeRiskScreenOKClickIn(ByVal sXML As String) As RiskScreenOKClickIn
        Try

            Dim oType As RiskScreenOKClickIn = RiskScreenOKClickIn.CreateInstance()
            Dim oXML As XmlDocument
            Dim oNode As XmlNode

            oXML = New XmlDocument()

            'developer guide no. no solution 22
            'oXML.validateOnParse = True

            'oXML.async = False

            'oXML.setProperty("NewParser", True)

            Try
                oXML.LoadXml(sXML)



            Catch parseError As System.Xml.XmlException


                'developer guide no. no solution 22
                'Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenOKClickIn, " + "XML Parse Error: " & parseError.errorCode & " - " & parseError.Message)
                Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenOKClickIn, " + "XML Parse Error: " & " - " & parseError.Message)
            End Try

            oType.iTask = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/iTask")))
            oType.iSourceID = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/iSourceID")))
            oType.lNavigate = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lNavigate")))
            oType.lProcessMode = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lProcessMode")))
            oType.sTransactionType = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sTransactionType"))
            oType.dtEffectiveDate = CDate(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/dtEffectiveDate")))
            oType.bSubScreen = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/bSubScreen")))
            oType.lScreenId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lScreenId")))
            oType.lRiskId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lRiskId")))
            oType.lRiskTypeId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lRiskTypeId")))
            oType.sGisDataModelCode = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sGisDataModelCode"))
            oType.lGISDataModelType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lGISDataModelType")))
            oType.lObjectType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lObjectType")))
            oType.sGISXMLDataset = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sGISXMLDataset"))
            oType.sMyOIKey = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sMyOIKey"))
            oType.sMyObjectName = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sMyObjectName"))
            oType.sParentOIKey = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sParentOIKey"))
            oType.sParentObjectName = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/sParentObjectName"))
            oType.lPolicyLinkId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lPolicyLinkId")))
            oType.lInsuranceFileCnt = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lInsuranceFileCnt")))
            oType.lPartyCnt = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lPartyCnt")))
            If Not oXML.SelectSingleNode("RiskScreenOKClickIn/dtPolicyStartDate") Is Nothing Then
                oType.dtPolicyStartDate = CDate(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/dtPolicyStartDate")))
            End If
            If Not oXML.SelectSingleNode("RiskScreenOKClickIn/dtPolicyEndDate") Is Nothing Then
                oType.dtPolicyEndDate = CDate(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/dtPolicyEndDate")))
            End If
            If Not oXML.SelectSingleNode("RiskScreenOKClickIn/lAgentCnt") Is Nothing Then
                oType.lAgentCnt = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lAgentCnt")))
            End If
            If Not oXML.SelectSingleNode("RiskScreenOKClickIn/lRiskGroupId") Is Nothing Then
                oType.lRiskGroupId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lRiskGroupId")))
            End If
            If Not oXML.SelectSingleNode("RiskScreenOKClickIn/lRiskCodeId") Is Nothing Then
                oType.lRiskCodeId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lRiskCodeId")))
            End If
            If Not oXML.SelectSingleNode("RiskScreenOKClickIn/lCountryId") Is Nothing Then
                oType.lCountryId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickIn/lCountryId")))
            End If

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



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenOKClickIn, " + "Error: " & Informations.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function DeserializeRiskScreenOKClickOut(ByVal sXML As String) As RiskScreenOKClickOut
        Try

            Dim oType As RiskScreenOKClickOut = RiskScreenOKClickOut.CreateInstance()
            Dim oXML As XmlDocument
            Dim oNode As XmlNode
            Dim oNodes As XmlNodeList

            oXML = New XmlDocument()

            'developer guide no. no solution 22
            'oXML.validateOnParse = True

            'oXML.async = False

            'oXML.setProperty("NewParser", True)

            Try
                oXML.LoadXml(sXML)



            Catch parseError As System.Xml.XmlException


                'developer guide no. no solution 22
                'Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenOKClickOut, " + "XML Parse Error: " & parseError.errorCode & " - " & parseError.Message)
                Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenOKClickOut, " + "XML Parse Error: " & " - " & parseError.Message)
            End Try

            oNodes = oXML.SelectNodes("RiskScreenOKClickOut/ERRORS")
            oType.Errors = ExportErrorArray(oNodes, oType.HasErrors, oType.HasInvalidDataErrors, oType.HasBusinessRuleErrors, oType.HasBackOfficeErrors, oType.HasInternalExceptionErrors)

            oNodes = oXML.SelectNodes("RiskScreenOKClickOut/WARNINGS/WARNING")
            oType.Warnings = ExportMessageArray(oNodes, oType.HasWarnings)
            oType.sGISXMLDataset = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sGISXMLDataset"))

            oNode = oXML.SelectSingleNode("RiskScreenOKClickOut/vScreenValuesArray")

            If Not (oNode Is Nothing) Then


                oType.vScreenValuesArray = ExportItemArray(oNode)
            End If

            oNode = oXML.SelectSingleNode("RiskScreenOKClickOut/vRiskDetailsArray")

            If Not (oNode Is Nothing) Then


                oType.vRiskDetailsArray = ExportItemArray(oNode)
            End If
            oType.sReferReasons = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sReferReasons"))
            oType.sDeclineReasons = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sDeclineReasons"))
            oType.sMessages = ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/sMessages"))
            oType.lQuoteType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenOKClickOut/lQuoteType")))

            oNode = Nothing
            oXML = Nothing


            Return oType

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenOKClickOut, " + "Error: " & Informations.Err().Number & " - " & excep.Message)

        End Try
    End Function
End Module