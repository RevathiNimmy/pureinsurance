Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
'Developer Guide No. 129
Imports SharedFiles
Module XMLTransRiskScreenLinkToParty

    Public Structure RiskScreenLinkToPartyIn
        Dim iTask As Integer
        Dim iSourceID As Integer
        Dim lNavigate As Integer
        Dim lProcessMode As Integer
        Dim sTransactionType As String
        Dim dtEffectiveDate As Date
        Dim lOldPartyId As Integer
        Dim lNewPartyId As Integer
        Dim lClaimID As Integer
        Public Shared Function CreateInstance() As RiskScreenLinkToPartyIn
            Dim result As New RiskScreenLinkToPartyIn
            result.sTransactionType = String.Empty
            Return result
        End Function
    End Structure

    Public Structure RiskScreenLinkToPartyOut
        Dim Errors As XMLSupport.PMXMLErrorTypes
        Dim Warnings As XMLSupport.PMXMLWarningMsg
        Dim HasErrors As Boolean
        Dim HasInvalidDataErrors As Boolean
        Dim HasBusinessRuleErrors As Boolean
        Dim HasBackOfficeErrors As Boolean
        Dim HasInternalExceptionErrors As Boolean
        Dim HasWarnings As Boolean
        Public Shared Function CreateInstance() As RiskScreenLinkToPartyOut
            Dim result As New RiskScreenLinkToPartyOut
            result.Errors = PMXMLErrorTypes.CreateInstance()
            Return result
        End Function
    End Structure

    Public Function SerializeRiskScreenLinkToPartyIn(ByRef oTypeIn As RiskScreenLinkToPartyIn) As String
        Try

            Dim vDataStore As Object
            Dim lLastPos As Integer


            Append(vDataStore, lLastPos, "<RiskScreenLinkToPartyIn>")


            StoreItem(vDataStore, lLastPos, "iTask", oTypeIn.iTask)

            StoreItem(vDataStore, lLastPos, "iSourceID", oTypeIn.iSourceID)

            StoreItem(vDataStore, lLastPos, "lNavigate", oTypeIn.lNavigate)

            StoreItem(vDataStore, lLastPos, "lProcessMode", oTypeIn.lProcessMode)

            StoreItem(vDataStore, lLastPos, "sTransactionType", oTypeIn.sTransactionType)

            StoreItem(vDataStore, lLastPos, "dtEffectiveDate", oTypeIn.dtEffectiveDate)

            StoreItem(vDataStore, lLastPos, "lOldPartyId", oTypeIn.lOldPartyId)

            StoreItem(vDataStore, lLastPos, "lNewPartyId", oTypeIn.lNewPartyId)

            StoreItem(vDataStore, lLastPos, "lClaimID", oTypeIn.lClaimID)


            Append(vDataStore, lLastPos, "</RiskScreenLinkToPartyIn>")


            'Developer Guide No 271. 
            Dim tempArray(vDataStore.Length - 1) As String
            vDataStore.CopyTo(tempArray, 0)
            Return String.Concat(tempArray)


        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", SerializeRiskScreenLinkToPartyIn, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function SerializeRiskScreenLinkToPartyOut(ByRef oTypeOut As RiskScreenLinkToPartyOut) As String
        Try

            Dim vDataStore As Object
            Dim lLastPos As Integer


            Append(vDataStore, lLastPos, "<RiskScreenLinkToPartyOut>")


            StoreErrorArray(vDataStore, lLastPos, oTypeOut.Errors)

            StoreMessageArray(vDataStore, lLastPos, oTypeOut.Warnings)


            Append(vDataStore, lLastPos, "</RiskScreenLinkToPartyOut>")


            'Developer Guide No 271. 
            Dim tempArray(vDataStore.Length - 1) As String
            vDataStore.CopyTo(tempArray, 0)
            Return String.Concat(tempArray)


        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", SerializeRiskScreenLinkToPartyOut, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function DeserializeRiskScreenLinkToPartyIn(ByVal sXML As String) As RiskScreenLinkToPartyIn
        Try

            Dim oType As RiskScreenLinkToPartyIn = RiskScreenLinkToPartyIn.CreateInstance()
            Dim oXML As XmlDocument
            Dim oNode As XmlNode

            oXML = New XmlDocument()
            Try
                oXML.LoadXml(sXML)



            Catch parseError As System.Xml.XmlException


                Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenLinkToPartyIn, " + "XML Parse Error: " & " - " & parseError.Message)
            End Try

            oType.iTask = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLinkToPartyIn/iTask")))
            oType.iSourceID = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLinkToPartyIn/iSourceID")))
            oType.lNavigate = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLinkToPartyIn/lNavigate")))
            oType.lProcessMode = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLinkToPartyIn/lProcessMode")))
            oType.sTransactionType = ExportItem(oXML.SelectSingleNode("RiskScreenLinkToPartyIn/sTransactionType"))
            oType.dtEffectiveDate = CDate(ExportItem(oXML.SelectSingleNode("RiskScreenLinkToPartyIn/dtEffectiveDate")))
            oType.lOldPartyId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLinkToPartyIn/lOldPartyId")))
            oType.lNewPartyId = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLinkToPartyIn/lNewPartyId")))
            oType.lClaimID = CInt(ExportItem(oXML.SelectSingleNode("RiskScreenLinkToPartyIn/lClaimID")))

            oNode = Nothing
            oXML = Nothing


            Return oType

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenLinkToPartyIn, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function

    Public Function DeserializeRiskScreenLinkToPartyOut(ByVal sXML As String) As RiskScreenLinkToPartyOut
        Try

            Dim oType As RiskScreenLinkToPartyOut = RiskScreenLinkToPartyOut.CreateInstance()
            Dim oXML As XmlDocument
            Dim oNode As XmlNode
            Dim oNodes As XmlNodeList

            oXML = New XmlDocument()
            Try
                oXML.LoadXml(sXML)



            Catch parseError As System.Xml.XmlException


                Throw New System.Exception(gPMConstants.PMEReturnCode.PMXMLParseError.ToString() + ", DeserializeRiskScreenLinkToPartyOut, " + "XML Parse Error: " & " - " & parseError.Message)
            End Try

            oNodes = oXML.SelectNodes("RiskScreenLinkToPartyOut/ERRORS")
            oType.Errors = ExportErrorArray(oNodes, oType.HasErrors, oType.HasInvalidDataErrors, oType.HasBusinessRuleErrors, oType.HasBackOfficeErrors, oType.HasInternalExceptionErrors)

            oNodes = oXML.SelectNodes("RiskScreenLinkToPartyOut/WARNINGS/WARNING")
            oType.Warnings = ExportMessageArray(oNodes, oType.HasWarnings)

            oNode = Nothing
            oXML = Nothing


            Return oType

        Catch excep As System.Exception



            Throw New System.Exception(gPMConstants.PMEReturnCode.PMError.ToString() + ", DeserializeRiskScreenLinkToPartyOut, " + "Error: " & Information.Err().Number & " - " & excep.Message)

        End Try
    End Function
End Module