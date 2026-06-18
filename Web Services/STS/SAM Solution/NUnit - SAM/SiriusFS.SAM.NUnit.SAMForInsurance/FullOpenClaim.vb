
Public Class FullOpenClaim
    Inherits BaseTest

    Public Sub FullPayClaimTest()

        Try

            Dim BaseClaimKey As Integer
            Dim ClaimKey As Integer
            Dim BranchCode As String = String.Empty
            Dim TimeStamp As Byte()
            Dim XMLDataSet As String

            ' open the claim and add the relevant risk data to it
            FullOpenClaimTest(BaseClaimKey, ClaimKey, BranchCode)

            ' call get claim details
            Dim oGetClaimDetailsResponse As ProxyWS.GetClaimDetailsResponseType
            oGetClaimDetailsResponse = GetClaimDetails(ClaimKey, BranchCode)

            ' get the timestamp
            TimeStamp = oGetClaimDetailsResponse.TimeStamp

            ' call the pay claim method
            'Dim oPayClaimTest As PayClaim = New PayClaim
            'oPayClaimTest.SupportMethod(BaseClaimKey, ClaimKey, BranchCode, TimeStamp)

            ' call add claim risk
            Dim oGetClaimRiskResponse As ProxyWS.GetClaimRiskResponseType
            oGetClaimRiskResponse = GetClaimRisk(BaseClaimKey, BranchCode)

            ' get the dataset / time stamp
            XMLDataSet = oGetClaimRiskResponse.XMLDataSet
            TimeStamp = oGetClaimRiskResponse.TimeStamp

            ' load the xmldataset into an xml document
            Dim oXMLDoc As XmlDocument = New XmlDocument
            oXMLDoc.LoadXml(XMLDataSet)

            ' get the risk objects node
            Dim oClaimNode As XmlNode
            oClaimNode = oXMLDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("CLAIMS_POLICY_BINDER").SelectSingleNode("WORK_CLAIM")

            Dim ClaimPerilKey As Integer
            Dim ClaimPerilOI As String = String.Empty

            ' for each child node of the risk objects node
            For Each oXMLNode As XmlNode In oClaimNode.ChildNodes

                Select Case oXMLNode.Name

                    ' if the child node is a claim peril
                    Case "WORK_CLAIM_PERIL"

                        ' get the claim peril key from attributes - OI = "CP" & ClaimPerilKey
                        ClaimPerilOI = oXMLNode.Attributes("OI").Value

                        ' get the actual key from the OI value
                        ClaimPerilKey = ClaimPerilOI.Substring(2)

                        ' call run default rules edit
                        Dim oRunDefaultRulesEditResponse As ProxyWS.RunDefaultRulesEditResponseType
                        oRunDefaultRulesEditResponse = RunDefaultRulesEdit(ClaimKey, ClaimPerilKey, BranchCode, XMLDataSet)

                        ' get the xmldataset after the default script has been run against it
                        XMLDataSet = oRunDefaultRulesEditResponse.XMLDataSet

                        ' call run validation rules edit
                        Dim oRunValidationRulesEditResponse As ProxyWS.RunValidationRulesResponseType
                        oRunValidationRulesEditResponse = RunValidationRulesEdit(ClaimKey, ClaimPerilKey, BranchCode, XMLDataSet)

                        ' get the xmldataset after the validation script has been run against it
                        XMLDataSet = oRunValidationRulesEditResponse.XMLDataset

                End Select

            Next

            ' call update risk
            Dim oUpdateClaimRiskResponse As ProxyWS.UpdateClaimRiskResponseType
            oUpdateClaimRiskResponse = UpdateClaimRisk(BranchCode, BaseClaimKey, XMLDataSet, TimeStamp)

        Catch ex As Exception
            Debug.Assert(False)
        End Try


    End Sub


    Public Sub FullMaintainClaimTest()

        Try

            Dim BaseClaimKey As Integer
            Dim ClaimKey As Integer
            Dim BranchCode As String = String.Empty
            Dim TimeStamp As Byte()
            Dim XMLDataSet As String

            ' open the claim and add the relevant risk data to it
            FullOpenClaimTest(BaseClaimKey, ClaimKey, BranchCode)

            ' call get claim details
            Dim oGetClaimDetailsResponse As ProxyWS.GetClaimDetailsResponseType
            oGetClaimDetailsResponse = GetClaimDetails(ClaimKey, BranchCode)

            ' get the timestamp
            TimeStamp = oGetClaimDetailsResponse.TimeStamp

            ' call the open claim method
            Dim oMaintainClaimTest As MaintainClaim = New MaintainClaim
            oMaintainClaimTest.SupportMethod(BaseClaimKey, ClaimKey, BranchCode, TimeStamp)

            ' call add claim risk
            Dim oGetClaimRiskResponse As ProxyWS.GetClaimRiskResponseType
            oGetClaimRiskResponse = GetClaimRisk(BaseClaimKey, BranchCode)

            ' get the dataset / time stamp
            XMLDataSet = oGetClaimRiskResponse.XMLDataSet
            TimeStamp = oGetClaimRiskResponse.TimeStamp

            Dim oRunDefaultRulesAddResponse As ProxyWS.RunDefaultRulesAddResponseType
            oRunDefaultRulesAddResponse = RunDefaultRulesAdd(BranchCode, "CLMCHILD", XMLDataSet)

            XMLDataSet = oRunDefaultRulesAddResponse.XMLDataSet

            Dim oRunDefaultRulesEditNonClaimResponse As ProxyWS.RunDefaultRulesEditResponseType
            oRunDefaultRulesEditNonClaimResponse = RunDefaultRulesEdit(BranchCode, "CLMCHILD", XMLDataSet)

            XMLDataSet = oRunDefaultRulesEditNonClaimResponse.XMLDataSet

            ' load the xmldataset into an xml document
            Dim oXMLDoc As XmlDocument = New XmlDocument
            oXMLDoc.LoadXml(XMLDataSet)

            ' get the risk objects node
            Dim oClaimNode As XmlNode
            oClaimNode = oXMLDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("CLAIMS_POLICY_BINDER").SelectSingleNode("WORK_CLAIM")

            Dim ClaimPerilKey As Integer
            Dim ClaimPerilOI As String = String.Empty

            ' for each child node of the risk objects node
            For Each oXMLNode As XmlNode In oClaimNode.ChildNodes

                Select Case oXMLNode.Name

                    ' if the child node is a claim peril
                    Case "WORK_CLAIM_PERIL"

                        ' get the claim peril key from attributes - OI = "CP" & ClaimPerilKey
                        ClaimPerilOI = oXMLNode.Attributes("OI").Value

                        ' get the actual key from the OI value
                        ClaimPerilKey = ClaimPerilOI.Substring(2)

                        ' call run default rules edit
                        Dim oRunDefaultRulesEditResponse As ProxyWS.RunDefaultRulesEditResponseType
                        oRunDefaultRulesEditResponse = RunDefaultRulesEdit(ClaimKey, ClaimPerilKey, BranchCode, XMLDataSet)

                        ' get the xmldataset after the default script has been run against it
                        XMLDataSet = oRunDefaultRulesEditResponse.XMLDataSet

                        ' call run validation rules edit
                        Dim oRunValidationRulesEditResponse As ProxyWS.RunValidationRulesResponseType
                        oRunValidationRulesEditResponse = RunValidationRulesEdit(ClaimKey, ClaimPerilKey, BranchCode, XMLDataSet)

                        ' get the xmldataset after the validation script has been run against it
                        XMLDataSet = oRunValidationRulesEditResponse.XMLDataset

                End Select

            Next

            ' call update risk
            Dim oUpdateClaimRiskResponse As ProxyWS.UpdateClaimRiskResponseType
            oUpdateClaimRiskResponse = UpdateClaimRisk(BranchCode, BaseClaimKey, XMLDataSet, TimeStamp)

        Catch ex As Exception
            Debug.Assert(False)
        End Try

    End Sub

    Public Sub FullOpenClaimTest(ByRef BaseClaimKey As Integer, ByRef ClaimKey As Integer, ByRef BranchCode As String)

        Try

            Dim TimeStamp As Byte() = Nothing
            Dim XMLDataSet As String

            ' call the open claim method
            Dim oOpenClaimTest As OpenClaim = New OpenClaim
            oOpenClaimTest.SupportMethod(BaseClaimKey, ClaimKey, BranchCode, TimeStamp)

            ' call add claim risk
            Dim oAddClaimRiskResponse As ProxyWS.AddClaimRiskResponseType
            oAddClaimRiskResponse = AddClaimRisk(BaseClaimKey, BranchCode, TimeStamp)

            ' get the dataset / time stamp
            XMLDataSet = oAddClaimRiskResponse.XMLDataSet
            TimeStamp = oAddClaimRiskResponse.TimeStamp

            ' call get claim details
            Dim oGetClaimDetailsResponse As ProxyWS.GetClaimDetailsResponseType
            oGetClaimDetailsResponse = GetClaimDetails(ClaimKey, BranchCode)

            ' get the timestamp
            TimeStamp = oGetClaimDetailsResponse.TimeStamp

            Dim oRunDefaultRulesAddResponse As ProxyWS.RunDefaultRulesAddResponseType
            oRunDefaultRulesAddResponse = RunDefaultRulesAdd(BranchCode, "CLMCHILD", XMLDataSet)

            XMLDataSet = oRunDefaultRulesAddResponse.XMLDataSet

            ' XMLDataSet = oRunDefaultRulesEditNonClaimResponse.XMLDataSet

            ' load the xmldataset into an xml document
            Dim oXMLDoc As XmlDocument = New XmlDocument
            oXMLDoc.LoadXml(XMLDataSet)

            ' get the risk objects node
            Dim oClaimNode As XmlNode
            oClaimNode = oXMLDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("CLAIMS_POLICY_BINDER").SelectSingleNode("WORK_CLAIM")

            Dim ClaimPerilKey As Integer
            Dim ClaimPerilOI As String = String.Empty

            ' for each child node of the risk objects node
            For Each oXMLNode As XmlNode In oClaimNode.ChildNodes

                Select Case oXMLNode.Name

                    ' if the child node is a claim peril
                    Case "WORK_CLAIM_PERIL"

                        ' get the claim peril key from attributes - OI = "CP" & ClaimPerilKey
                        ClaimPerilOI = oXMLNode.Attributes("OI").Value

                        ' get the actual key from the OI value
                        ClaimPerilKey = ClaimPerilOI.Substring(2)

                        ' call run default rules edit
                        Dim oRunDefaultRulesEditResponse As ProxyWS.RunDefaultRulesEditResponseType
                        oRunDefaultRulesEditResponse = RunDefaultRulesEdit(ClaimKey, ClaimPerilKey, BranchCode, XMLDataSet)

                        ' get the xmldataset after the default script has been run against it
                        XMLDataSet = oRunDefaultRulesEditResponse.XMLDataSet

                        ' call run validation rules edit
                        Dim oRunValidationRulesEditResponse As ProxyWS.RunValidationRulesResponseType
                        oRunValidationRulesEditResponse = RunValidationRulesEdit(ClaimKey, ClaimPerilKey, BranchCode, XMLDataSet)

                        ' get the xmldataset after the validation script has been run against it
                        XMLDataSet = oRunValidationRulesEditResponse.XMLDataset

                End Select

            Next

            ' call update risk
            Dim oUpdateClaimRiskResponse As ProxyWS.UpdateClaimRiskResponseType
            oUpdateClaimRiskResponse = UpdateClaimRisk(BranchCode, BaseClaimKey, XMLDataSet, TimeStamp)

        Catch ex As Exception
            Debug.Assert(False)
        End Try


    End Sub

    Private Function RunDefaultRulesAdd(ByVal BranchCode As String, ByVal ScreenCode As String, ByVal XMLDAtaSet As String) As ProxyWS.RunDefaultRulesAddResponseType

        Dim oRequest As New ProxyWS.RunDefaultRulesAddRequestType
        Dim oResponse As ProxyWS.RunDefaultRulesAddResponseType = Nothing

        With oRequest
            .BranchCode = BranchCode
            .ScreenCode = ScreenCode
            .XMLDataSet = XMLDAtaSet
        End With

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        oResponse = oProxy.RunDefaultRulesAdd(oRequest)

        Return oResponse

    End Function

    Private Function RunDefaultRulesEdit( _
   ByVal BranchCode As String, _
   ByVal ScreenCode As String, _
   ByVal XMLDataSet As String) As ProxyWS.RunDefaultRulesEditResponseType

        Dim oRequest As New ProxyWS.RunDefaultRulesEditRequestType
        Dim oResponse As ProxyWS.RunDefaultRulesEditResponseType

        With oRequest
            .BranchCode = BranchCode
            .ScreenCode = ScreenCode
            .XMLDataSet = XMLDataSet
        End With

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        oResponse = oProxy.RunDefaultRulesEdit(oRequest)

        Return oResponse
    End Function

    Private Function GetClaimRisk(ByVal BaseClaimKey As Integer, ByVal BranchCode As String) As ProxyWS.GetClaimRiskResponseType

        Dim oRequest As New ProxyWS.GetClaimRiskRequestType
        Dim oResponse As ProxyWS.GetClaimRiskResponseType = Nothing

        With oRequest
            .BaseClaimKey = BaseClaimKey
            .BranchCode = BranchCode
        End With

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        oResponse = oProxy.GetClaimRisk(oRequest)

        Return oResponse

    End Function

    Private Function UpdateClaimRisk(ByVal BranchCode As String, _
    ByVal BaseClaimKey As Integer, _
    ByVal XMLDataSet As String, _
    ByVal TimeStamp As Byte()) As ProxyWS.UpdateClaimRiskResponseType

        Dim oRequest As New ProxyWS.UpdateClaimRiskRequestType
        Dim oResponse As ProxyWS.UpdateClaimRiskResponseType

        With oRequest
            .BranchCode = BranchCode
            .BaseClaimKey = BaseClaimKey
            .XMLDataSet = XMLDataSet
            .TimeStamp = TimeStamp
        End With

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        oResponse = oProxy.UpdateClaimRisk(oRequest)

        Return oResponse

    End Function

    Private Function RunValidationRulesEdit(ByVal ClaimKey As Integer, _
    ByVal ClaimPerilKey As Integer, _
    ByVal BranchCode As String, _
    ByVal XMLDataSet As String) As ProxyWS.RunValidationRulesResponseType

        Dim oRequest As New ProxyWS.RunValidationRulesRequestType
        Dim oResponse As ProxyWS.RunValidationRulesResponseType

        With oRequest
            .ClaimKey = ClaimKey
            .ClaimKeySpecified = True
            .ClaimPerilKey = ClaimPerilKey
            .ClaimPerilKeySpecified = True
            .BranchCode = BranchCode
            .XMLDataSet = XMLDataSet
        End With

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        oResponse = oProxy.RunValidationRules(oRequest)

        Return oResponse

    End Function

    Private Function RunDefaultRulesEdit( _
    ByVal ClaimKey As Integer, _
    ByVal ClaimPerilKey As Integer, _
    ByVal BranchCode As String, _
    ByVal XMLDataSet As String) As ProxyWS.RunDefaultRulesEditResponseType

        Dim oRequest As New ProxyWS.RunDefaultRulesEditRequestType
        Dim oResponse As ProxyWS.RunDefaultRulesEditResponseType

        With oRequest
            .ClaimKey = ClaimKey
            .ClaimKeySpecified = True
            .ClaimPerilKey = ClaimPerilKey
            .ClaimPerilKeySpecified = True
            .BranchCode = BranchCode
            .XMLDataSet = XMLDataSet
        End With

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        oResponse = oProxy.RunDefaultRulesEdit(oRequest)

        Return oResponse
    End Function

    Private Function AddClaimRisk(ByVal BaseClaimKey As Integer, ByVal BranchCode As String, ByVal TimeStamp As Byte())
        Dim oRequest As New ProxyWS.AddClaimRiskRequestType
        Dim oResponse As ProxyWS.AddClaimRiskResponseType = Nothing


        With oRequest
            .BaseClaimKey = BaseClaimKey
            .BranchCode = BranchCode
            .TimeStamp = TimeStamp
        End With

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        oResponse = oProxy.AddClaimRisk(oRequest)

        Return oResponse

    End Function

    Private Function GetClaimDetails(ByVal ClaimKey As Integer, ByVal BranchCode As String) As ProxyWS.GetClaimDetailsResponseType

        Dim oRequest As New ProxyWS.GetClaimDetailsRequestType
        Dim oResponse As ProxyWS.GetClaimDetailsResponseType = Nothing

        With oRequest
            .ClaimKey = ClaimKey
            .BranchCode = BranchCode
        End With

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        oResponse = oProxy.GetClaimDetails(oRequest)

        Return oResponse

    End Function

End Class
