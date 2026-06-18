Option Strict Off
Option Explicit On
Imports System.Xml
Imports SSP.Shared
'developer guide no. 129
Module XMLTransRiskScreenCancel

    Public Structure RiskScreenCancelIn
        Dim iTask As Integer
        Dim iSourceID As Integer
        Dim lNavigate As Integer
        Dim lProcessMode As Integer
        Dim sTransactionType As String
        Dim dtEffectiveDate As Date
        Dim bSubScreen As Boolean
        Dim lScreenId As Integer
        Dim lRiskId As Integer
        Dim sGisDataModelCode As Object
        Dim lGISDataModelType As Integer
        Dim lObjectType As Integer
        Dim sGISXMLDataset As Object
        Dim sMyOIKey As Object
        Dim sMyObjectName As Object
        Dim sParentOIKey As String
        Dim sParentObjectName As String
        Dim lPolicyLinkId As Integer
        Dim bRevertBackRisk As Boolean
        Dim lInsuranceFileCnt As Integer
        Dim bRiskAdded As Boolean
        Dim bRiskCopied As Boolean
        Public Shared Function CreateInstance() As RiskScreenCancelIn
            Dim result As New RiskScreenCancelIn
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

    Public Structure RiskScreenCancelOut
        Dim Errors As XMLSupport.PMXMLErrorTypes
        Dim Warnings As XMLSupport.PMXMLWarningMsg
        Dim HasErrors As Boolean
        Dim HasInvalidDataErrors As Boolean
        Dim HasBusinessRuleErrors As Boolean
        Dim HasBackOfficeErrors As Boolean
        Dim HasInternalExceptionErrors As Boolean
        Dim HasWarnings As Boolean
        Dim sGISXMLDataset As String
        Public Shared Function CreateInstance() As RiskScreenCancelOut
            Dim result As New RiskScreenCancelOut
            result.Errors = PMXMLErrorTypes.CreateInstance()
            result.sGISXMLDataset = String.Empty
            Return result
        End Function
    End Structure

    Public Function SerializeRiskScreenCancelIn(ByRef oTypeIn As RiskScreenCancelIn) As String
        Try

            Dim vDataStore As Object = Nothing
            Dim lLastPos As Integer


            Append(vDataStore, lLastPos, "<RiskScreenCancelIn>")


            StoreItem(vDataStore, lLastPos, "iTask", oTypeIn.iTask)

            StoreItem(vDataStore, lLastPos, "iSourceID", oTypeIn.iSourceID)

            StoreItem(vDataStore, lLastPos, "lNavigate", oTypeIn.lNavigate)

            StoreItem(vDataStore, lLastPos, "lProcessMode", oTypeIn.lProcessMode)

            StoreItem(vDataStore, lLastPos, "sTransactionType", oTypeIn.sTransactionType)

            StoreItem(vDataStore, lLastPos, "dtEffectiveDate", oTypeIn.dtEffectiveDate)

            StoreItem(vDataStore, lLastPos, "bSubScreen", oTypeIn.bSubScreen)

            StoreItem(vDataStore, lLastPos, "lScreenId", oTypeIn.lScreenId)

            StoreItem(vDataStore, lLastPos, "lRiskId", oTypeIn.lRiskId)

            StoreItem(vDataStore, lLastPos, "sGisDataModelCode", oTypeIn.sGisDataModelCode)

            StoreItem(vDataStore, lLastPos, "lGISDataModelType", oTypeIn.lGISDataModelType)

            StoreItem(vDataStore, lLastPos, "lObjectType", oTypeIn.lObjectType)

            StoreItem(vDataStore, lLastPos, "sGISXMLDataset", oTypeIn.sGISXMLDataset)

            StoreItem(vDataStore, lLastPos, "sMyOIKey", oTypeIn.sMyOIKey)

            StoreItem(vDataStore, lLastPos, "sMyObjectName", oTypeIn.sMyObjectName)

            StoreItem(vDataStore, lLastPos, "sParentOIKey", oTypeIn.sParentOIKey)

            StoreItem(vDataStore, lLastPos, "sParentObjectName", oTypeIn.sParentObjectName)

            StoreItem(vDataStore, lLastPos, "lPolicyLinkId", oTypeIn.lPolicyLinkId)

            StoreItem(vDataStore, lLastPos, "bRevertBackRisk", oTypeIn.bRevertBackRisk)

            StoreItem(vDataStore, lLastPos, "lInsuranceFileCnt", oTypeIn.lInsuranceFileCnt)

            StoreItem(vDataStore, lLastPos, "bRiskAdded", oTypeIn.bRiskAdded)

            StoreItem(vDataStore, lLastPos, "bRiskCopied", oTypeIn.bRiskCopied)


            Append(vDataStore, lLastPos, "</RiskScreenCancelIn>")


            'developer guide no 271. 
            Dim tempArray(vDataStore.Length - 1) As String
            vDataStore.CopyTo(CType(tempArray, Object), 0)
            Return String.Concat(tempArray)

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", SerializeRiskScreenCancelIn, " + "Error: " & Informations.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function SerializeRiskScreenCancelOut(ByRef oTypeOut As RiskScreenCancelOut) As String
        Try

            Dim vDataStore As Object = Nothing
            Dim lLastPos As Integer


            Append(vDataStore, lLastPos, "<RiskScreenCancelOut>")


            StoreErrorArray(vDataStore, lLastPos, oTypeOut.Errors)

            StoreMessageArray(vDataStore, lLastPos, oTypeOut.Warnings)

            StoreItem(vDataStore, lLastPos, "sGISXMLDataset", oTypeOut.sGISXMLDataset)


            Append(vDataStore, lLastPos, "</RiskScreenCancelOut>")


            'developer guide no 271. 
            Dim tempArray(vDataStore.Length - 1) As String
            vDataStore.CopyTo(CType(tempArray, Object), 0)
            Return String.Concat(tempArray)

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", SerializeRiskScreenCancelOut, " + "Error: " & Informations.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function DeserializeRiskScreenCancelIn(ByVal sXML As String) As RiskScreenCancelIn
        Try

            Dim oType As RiskScreenCancelIn = RiskScreenCancelIn.CreateInstance()
            Dim oXML As XmlDocument
            Dim oNode As XmlNode

            oXML = New XmlDocument()

            'developer guide no solution 22
            'oXML.validateOnParse = True

            'oXML.async = False

            '         oXML.setProperty("NewParser", True)

            Try
                oXML.LoadXml(sXML)



            Catch parseError As System.Xml.XmlException


                'developer guide no. no solution 22
                Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenCancelIn, " + "XML Parse Error: " & " - " & parseError.Message)
            End Try

            oType.iTask = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/iTask")))
            oType.iSourceID = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/iSourceID")))
            oType.lNavigate = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/lNavigate")))
            oType.lProcessMode = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/lProcessMode")))
            oType.sTransactionType = ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/sTransactionType"))
            oType.dtEffectiveDate = CDate(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/dtEffectiveDate")))
            oType.bSubScreen = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/bSubScreen")))
            oType.lScreenId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/lScreenId")))
            oType.lRiskId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/lRiskId")))
            oType.sGisDataModelCode = ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/sGisDataModelCode"))
            oType.lGISDataModelType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/lGISDataModelType")))
            oType.lObjectType = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/lObjectType")))
            oType.sGISXMLDataset = ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/sGISXMLDataset"))
            oType.sMyOIKey = ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/sMyOIKey"))
            oType.sMyObjectName = ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/sMyObjectName"))
            oType.sParentOIKey = ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/sParentOIKey"))
            oType.sParentObjectName = ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/sParentObjectName"))
            oType.lPolicyLinkId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/lPolicyLinkId")))
            oType.bRevertBackRisk = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/bRevertBackRisk")))
            oType.lInsuranceFileCnt = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/lInsuranceFileCnt")))
            oType.bRiskAdded = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/bRiskAdded")))
            oType.bRiskCopied = CBool(ExportItem(oXML.SelectSingleNode("RiskScreenCancelIn/bRiskCopied")))

            oNode = Nothing
            oXML = Nothing


            Return oType

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenCancelIn, " + "Error: " & Informations.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function DeserializeRiskScreenCancelOut(ByVal sXML As String) As RiskScreenCancelOut
        Try

            Dim oType As RiskScreenCancelOut = RiskScreenCancelOut.CreateInstance()
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
                Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenCancelOut, " + "XML Parse Error: " & " - " & parseError.Message)
            End Try

            oNodes = oXML.SelectNodes("RiskScreenCancelOut/ERRORS")
            oType.Errors = ExportErrorArray(oNodes, oType.HasErrors, oType.HasInvalidDataErrors, oType.HasBusinessRuleErrors, oType.HasBackOfficeErrors, oType.HasInternalExceptionErrors)

            oNodes = oXML.SelectNodes("RiskScreenCancelOut/WARNINGS/WARNING")
            oType.Warnings = ExportMessageArray(oNodes, oType.HasWarnings)
            oType.sGISXMLDataset = ExportItem(oXML.SelectSingleNode("RiskScreenCancelOut/sGISXMLDataset"))

            oNode = Nothing
            oXML = Nothing


            Return oType

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenCancelOut, " + "Error: " & Informations.Err().Number & " - " & excep.Message)

        End Try
    End Function
End Module