Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF
Imports Internal = SiriusFS.SAM.Structure.BaseImplementationTypes
Imports Sirius.Architecture.ExceptionHandling
Imports Sirius.Architecture.ExceptionHandling.Handler
Imports SiriusFS.SAM.CoreImplementation
Imports Sirius.Architecture.Data
Imports Sirius.Architecture.Utility
Imports Sirius.Architecture.Configuration.Database
Imports System.Xml.Serialization
Imports System.Linq
Imports Sirius.Architecture.Security
Imports System.ServiceModel.Activation
Imports System.Web.Services.Protocols

Partial Public Class PureService

    Public Function GetClaimReinsuranceArrangementLines(ByVal GetClaimReinsuranceArrangementLinesRequest As GetClaimReinsuranceArrangementLinesRequestType) As GetClaimReinsuranceArrangementLinesResponseType Implements IPureService.GetClaimReinsuranceArrangementLines
        Try
            Dim sUserName As String = GetClaimReinsuranceArrangementLinesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)

            Dim oResponse As New GetClaimReinsuranceArrangementLinesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimReinsuranceArrangementLinesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementLinesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementLinesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimReinsuranceArrangementLinesRequest.BranchCode
            oImpRequest.ClaimId = GetClaimReinsuranceArrangementLinesRequest.ClaimId
            oImpRequest.ArrangementId = GetClaimReinsuranceArrangementLinesRequest.ArrangementId

            oImpRequest.ModeSpecified = GetClaimReinsuranceArrangementLinesRequest.ModeSpecified
            If (oImpRequest.ModeSpecified) Then
                oImpRequest.Mode = GetClaimReinsuranceArrangementLinesRequest.Mode
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimReinsuranceArrangementLines(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementLinesResponseTypeReinsuranceArrangementLines), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementLinesResponseTypeReinsuranceArrangementLinesRow), oXmlAttributes)

                '' Deserialize the ArrangementLines XML from the implementation resultdataset into
                '' the correct messaging format
                Dim oArrangementLinesDataSet As SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementLinesResponseTypeReinsuranceArrangementLines = Nothing
                Dim oArrangementLinesDataSetObject As Object = Nothing
                Dim oXMLSerializerArrangementLines As New XmlSerializer(GetType(SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementLinesResponseTypeReinsuranceArrangementLines), oXmlOverride)
                If oImpResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializerArrangementLines, r_oResultDataSet:=oArrangementLinesDataSetObject)
                    oArrangementLinesDataSet = DirectCast(oArrangementLinesDataSetObject, SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementLinesResponseTypeReinsuranceArrangementLines)
                End If

                ' Retrieve the values from the implementation response structure
                If oArrangementLinesDataSet IsNot Nothing AndAlso oArrangementLinesDataSet.Row IsNot Nothing AndAlso oArrangementLinesDataSet.Row.Count > 0 Then
                    oResponse.ReinsuranceArrangementLines = oArrangementLinesDataSet.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementLinesResponseTypeReinsuranceArrangementLinesRow, BaseGetClaimReinsuranceArrangementLinesResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetClaimReinsuranceArrangementLinesList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function

    Public Function GetClaimReinsuranceArrangements(ByVal GetClaimReinsuranceArrangementsRequest As GetClaimReinsuranceArrangementsRequestType) As GetClaimReinsuranceArrangementsResponseType Implements IPureService.GetClaimReinsuranceArrangements

        Try
            Dim sUserName As String = GetClaimReinsuranceArrangementsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)

            Dim oResponse As New GetClaimReinsuranceArrangementsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimReinsuranceArrangementsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimReinsuranceArrangementsRequest.BranchCode
            oImpRequest.ClaimId = GetClaimReinsuranceArrangementsRequest.ClaimId

            oImpRequest.ModeSpecified = GetClaimReinsuranceArrangementsRequest.ModeSpecified
            If (GetClaimReinsuranceArrangementsRequest.ModeSpecified) Then
                oImpRequest.Mode = GetClaimReinsuranceArrangementsRequest.Mode
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimReinsuranceArrangements(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementsResponseTypeReinsuranceArrangements), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementsResponseTypeReinsuranceArrangementsRow), oXmlAttributes)

                '' Deserialize the Arrangements XML from the implementation resultdataset into
                '' the correct messaging format
                Dim oArrangementsDataSet As SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementsResponseTypeReinsuranceArrangements = Nothing
                Dim oArrangementsDataSetObject As Object = Nothing
                Dim oXMLSerializerArrangements As New XmlSerializer(GetType(SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementsResponseTypeReinsuranceArrangements), oXmlOverride)
                If oImpResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializerArrangements, r_oResultDataSet:=oArrangementsDataSetObject)
                    oArrangementsDataSet = DirectCast(oArrangementsDataSetObject, SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementsResponseTypeReinsuranceArrangements)
                End If

                ' Retrieve the values from the implementation response structure
                If oArrangementsDataSet IsNot Nothing AndAlso oArrangementsDataSet.Row IsNot Nothing AndAlso oArrangementsDataSet.Row.Count > 0 Then
                    oResponse.ReinsuranceArrangements = oArrangementsDataSet.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementsResponseTypeReinsuranceArrangementsRow, BaseGetClaimReinsuranceArrangementsResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetClaimReinsuranceArrangementsList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function

    Public Function GetClaimReinsuranceBands(ByVal GetClaimReinsuranceBandsRequest As GetClaimReinsuranceBandsRequestType) As GetClaimReinsuranceBandsResponseType Implements IPureService.GetClaimReinsuranceBands

        Try
            Dim sUserName As String = GetClaimReinsuranceBandsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)

            Dim oResponse As New GetClaimReinsuranceBandsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimReinsuranceBandsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceBandsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceBandsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimReinsuranceBandsRequest.BranchCode
            oImpRequest.ClaimId = GetClaimReinsuranceBandsRequest.ClaimId

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimReinsuranceBands(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceBandsResponseTypeReinsuranceBands), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceBandsResponseTypeReinsuranceBandsRow), oXmlAttributes)

                '' Deserialize the Bands XML from the implementation resultdataset into
                '' the correct messaging format
                Dim oBandsDataSet As SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceBandsResponseTypeReinsuranceBands = Nothing
                Dim oBandsDataSetObject As Object = Nothing
                Dim oXMLSerializerBands As New XmlSerializer(GetType(SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceBandsResponseTypeReinsuranceBands), oXmlOverride)
                If oImpResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializerBands, r_oResultDataSet:=oBandsDataSetObject)
                    oBandsDataSet = DirectCast(oBandsDataSetObject, SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceBandsResponseTypeReinsuranceBands)
                End If

                ' Retrieve the values from the implementation response structure
                If oBandsDataSet IsNot Nothing AndAlso oBandsDataSet.Row IsNot Nothing AndAlso oBandsDataSet.Row.Count > 0 Then
                    oResponse.ReinsuranceBands = oBandsDataSet.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceBandsResponseTypeReinsuranceBandsRow, BaseGetClaimReinsuranceBandsResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetClaimReinsuranceBandsList))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function

    Public Function GetRecoveryReinsurance(ByVal GetRecoveryReinsuranceRequest As GetRecoveryReinsuranceRequestType) As GetRecoveryReinsuranceResponseType Implements IPureService.GetRecoveryReinsurance

        Try
            Dim sUserName As String = GetRecoveryReinsuranceRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRREINS", iUserId)


            Dim oResponse As New GetRecoveryReinsuranceResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRecoveryReinsuranceRequest.BranchCode)

            ' Implementation structures
            Dim oImplementationRequest As New SAMForInsuranceV2ImplementationTypes.GetRecoveryReinsuranceRequestType
            Dim oImplementationResponse As SAMForInsuranceV2ImplementationTypes.GetRecoveryReinsuranceResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImplementationRequest.BranchCode = GetRecoveryReinsuranceRequest.BranchCode
            oImplementationRequest.ClaimPerilKey = GetRecoveryReinsuranceRequest.ClaimPerilKey
            oImplementationRequest.IsSalvage = GetRecoveryReinsuranceRequest.IsSalvage

            Try
                ' Call the implementation method
                oImplementationResponse = oBusiness.GetRecoveryReinsurance(oImplementationRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImplementationResponse.STSError)

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetRecoveryReinsuranceResponseTypeReinsurances), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetRecoveryReinsuranceResponseTypeReinsurancesRow), oXmlAttributes)

                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As SFI.SAMForInsuranceV2.BaseGetRecoveryReinsuranceResponseTypeReinsurances = Nothing
                Dim oResultDataSetObject As Object = Nothing
                Dim oXMLSerializer As New XmlSerializer(GetType(SFI.SAMForInsuranceV2.BaseGetRecoveryReinsuranceResponseTypeReinsurances), oXmlOverride)
                If oImplementationResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImplementationResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject)
                    oResultDataSet = DirectCast(oResultDataSetObject, SFI.SAMForInsuranceV2.BaseGetRecoveryReinsuranceResponseTypeReinsurances)
                End If

                ' Retrieve the values from the implementation response structure
                If oResultDataSet IsNot Nothing AndAlso oResultDataSet.Row IsNot Nothing AndAlso oResultDataSet.Row.Count > 0 Then
                    oResponse.Reinsurances = oResultDataSet.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseGetRecoveryReinsuranceResponseTypeReinsurancesRow, BaseGetRecoveryReinsuranceResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetRecoveryReinsuranceList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImplementationResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function

    Public Function GetRiskReinsuranceArrangementLines(ByVal GetRiskReinsuranceArrangementLinesRequest As GetRiskReinsuranceArrangementLinesRequestType) As GetRiskReinsuranceArrangementLinesResponseType Implements IPureService.GetRiskReinsuranceArrangementLines
        Try
            Dim sUserName As String = GetRiskReinsuranceArrangementLinesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)

            Dim oResponse As New GetRiskReinsuranceArrangementLinesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRiskReinsuranceArrangementLinesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementLinesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementLinesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetRiskReinsuranceArrangementLinesRequest.BranchCode
            oImpRequest.ArrangementId = GetRiskReinsuranceArrangementLinesRequest.ArrangementId

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRiskReinsuranceArrangementLines(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementLinesResponseTypeArrangementLines), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementLinesResponseTypeArrangementLinesRow), oXmlAttributes)

                '' Deserialize the ArrangementLines XML from the implementation resultdataset into
                '' the correct messaging format
                Dim oArrangementLinesDataSet As SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementLinesResponseTypeArrangementLines = Nothing
                Dim oArrangementLinesDataSetObject As Object = Nothing
                Dim oXMLSerializerArrangementLines As New XmlSerializer(GetType(SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementLinesResponseTypeArrangementLines), oXmlOverride)
                If oImpResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializerArrangementLines, r_oResultDataSet:=oArrangementLinesDataSetObject)
                    oArrangementLinesDataSet = DirectCast(oArrangementLinesDataSetObject, SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementLinesResponseTypeArrangementLines)
                End If

                If oArrangementLinesDataSet IsNot Nothing AndAlso oArrangementLinesDataSet.Row IsNot Nothing AndAlso oArrangementLinesDataSet.Row.Count > 0 Then
                    oResponse.ArrangementLines = oArrangementLinesDataSet.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementLinesResponseTypeArrangementLinesRow, BaseGetRiskReinsuranceArrangementLinesResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetRiskReinsuranceArrangementLinesList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function

    Public Function GetRiskReinsuranceArrangements(ByVal GetRiskReinsuranceArrangementsRequest As GetRiskReinsuranceArrangementsRequestType) As GetRiskReinsuranceArrangementsResponseType Implements IPureService.GetRiskReinsuranceArrangements
        Try
            Dim sUserName As String = GetRiskReinsuranceArrangementsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRIA", iUserId)

            Dim oResponse As New GetRiskReinsuranceArrangementsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRiskReinsuranceArrangementsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetRiskReinsuranceArrangementsRequest.BranchCode
            oImpRequest.RiskKey = GetRiskReinsuranceArrangementsRequest.RiskKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRiskReinsuranceArrangements(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementsResponseTypeArrangements), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementsResponseTypeArrangementsRow), oXmlAttributes)

                '' Deserialize the Arrangements XML from the implementation resultdataset into
                '' the correct messaging format
                Dim oArrangementsDataSet As SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementsResponseTypeArrangements = Nothing
                Dim oArrangementsDataSetObject As Object = Nothing
                Dim oXMLSerializerArrangements As New XmlSerializer(GetType(SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementsResponseTypeArrangements), oXmlOverride)
                If oImpResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializerArrangements, r_oResultDataSet:=oArrangementsDataSetObject)
                    oArrangementsDataSet = DirectCast(oArrangementsDataSetObject, SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementsResponseTypeArrangements)
                End If

                If oArrangementsDataSet IsNot Nothing AndAlso oArrangementsDataSet.Row IsNot Nothing AndAlso oArrangementsDataSet.Row.Count > 0 Then
                    oResponse.Arrangements = oArrangementsDataSet.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementsResponseTypeArrangementsRow, BaseGetRiskReinsuranceArrangementsResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetRiskReinsuranceArrangementsList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function

    Public Function GetRiskReinsuranceBands(ByVal GetRiskReinsuranceBandsRequest As GetRiskReinsuranceBandsRequestType) As GetRiskReinsuranceBandsResponseType Implements IPureService.GetRiskReinsuranceBands

        Try
            Dim sUserName As String = GetRiskReinsuranceBandsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)

            Dim oResponse As New GetRiskReinsuranceBandsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRiskReinsuranceBandsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceBandsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceBandsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetRiskReinsuranceBandsRequest.BranchCode
            oImpRequest.RiskKey = GetRiskReinsuranceBandsRequest.RiskKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRiskReinsuranceBands(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceBandsResponseTypeReinsuranceBands), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceBandsResponseTypeReinsuranceBandsRow), oXmlAttributes)

                '' Deserialize the Bands XML from the implementation resultdataset into
                '' the correct messaging format
                Dim oBandsDataSet As SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceBandsResponseTypeReinsuranceBands = Nothing
                Dim oBandsDataSetObject As Object = Nothing
                Dim oXMLSerializerBands As New XmlSerializer(GetType(SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceBandsResponseTypeReinsuranceBands), oXmlOverride)
                If oImpResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializerBands, r_oResultDataSet:=oBandsDataSetObject)
                    oBandsDataSet = DirectCast(oBandsDataSetObject, SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceBandsResponseTypeReinsuranceBands)
                End If

                If oBandsDataSet IsNot Nothing AndAlso oBandsDataSet.Row IsNot Nothing AndAlso oBandsDataSet.Row.Count > 0 Then
                    oResponse.ReinsuranceBands = oBandsDataSet.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceBandsResponseTypeReinsuranceBandsRow, BaseGetRiskReinsuranceBandsResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetRiskReinsuranceBandsList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function

    Public Function GetRiskReinsuranceArrangementLinesRI2007(ByVal GetRiskReinsuranceArrangementLinesRI2007Request As GetRiskReinsuranceArrangementLinesRI2007RequestType) As GetRiskReinsuranceArrangementLinesRI2007ResponseType Implements IPureService.GetRiskReinsuranceArrangementLinesRI2007
        Try

            Dim sUserName As String = GetRiskReinsuranceArrangementLinesRI2007Request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRI07", iUserId)

            Dim oResponse As New GetRiskReinsuranceArrangementLinesRI2007ResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRiskReinsuranceArrangementLinesRI2007Request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementLinesRI2007RequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementLinesRI2007ResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetRiskReinsuranceArrangementLinesRI2007Request.BranchCode
            oImpRequest.ArrangementKey = GetRiskReinsuranceArrangementLinesRI2007Request.ArrangementKey
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRiskReinsuranceArrangementLinesRI2007(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If (oImpResponse.ArrangementLines IsNot Nothing) Then
                    oResponse.ArrangementLines = oImpResponse.ArrangementLines.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseRiskRIArrangementLineType, BaseRiskRIArrangementLineType)(AddressOf CommonFunctions.ToServiceRiskRIArrangementLineTypeList))
                End If
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function

    Public Function GetRIModelDetails(ByVal GetRIModelDetailsRequest As GetRIModelDetailsRequestType) As GetRIModelDetailsResponseType Implements IPureService.GetRIModelDetails
        Try

            Dim sUserName As String = GetRIModelDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRIMOD", iUserId)
            Dim oResponse As New GetRIModelDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRIModelDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRIModelDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRIModelDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetRIModelDetailsRequest.BranchCode
            oImpRequest.RIModelCode = GetRIModelDetailsRequest.RIModelCode
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRIModelDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Map to the Reponse object to send RI Lines back to the Front End
                With oResponse
                    .RIModelKey = oImpResponse.RIModelKey
                    .Code = oImpResponse.Code
                    .Description = oImpResponse.Description
                    .EffectiveDate = oImpResponse.EffectiveDate
                    .CurrencyCode = oImpResponse.CurrencyCode
                    .ExpiryDate = oImpResponse.ExpiryDate
                    .FACPremiums = oImpResponse.FACPremiums
                    .RIModelType = oImpResponse.RIModelType
                    .ClaimAllocationType = oImpResponse.ClaimAllocationType
                End With
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function

    Public Function GetRIModelLineDetails(ByVal oGetRIModelLineDetailsRequest As GetRIModelLineDetailsRequestType) As GetRIModelLineDetailsResponseType Implements IPureService.GetRIModelLineDetails

        Try

            Dim sUserName As String = oGetRIModelLineDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRIMOD", iUserId)
            Dim oResponse As New GetRIModelLineDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetRIModelLineDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRIModelLineDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRIModelLineDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetRIModelLineDetailsRequest.BranchCode
            oImpRequest.RIModelCode = oGetRIModelLineDetailsRequest.RIModelCode

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetRIModelLineDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetRIModelLineDetailsResponseTypeLines), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetRIModelLineDetailsResponseTypeLinesRow), oXmlAttributes)
                Dim oGetRIModelLineDetailsDataset As SFI.SAMForInsuranceV2.BaseGetRIModelLineDetailsResponseTypeLines = Nothing
                Dim GetRIModelLineDetailsDataSetObject As Object = Nothing
                Dim oXMLSerializerUserDetails As New XmlSerializer(GetType(SFI.SAMForInsuranceV2.BaseGetRIModelLineDetailsResponseTypeLines), oXmlOverride)
                If oImpResponse.Lines IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.Lines.OuterXml, oXMLSerializer:=oXMLSerializerUserDetails, r_oResultDataSet:=GetRIModelLineDetailsDataSetObject)
                    oGetRIModelLineDetailsDataset = DirectCast(GetRIModelLineDetailsDataSetObject, SFI.SAMForInsuranceV2.BaseGetRIModelLineDetailsResponseTypeLines)
                End If

                If oGetRIModelLineDetailsDataset IsNot Nothing AndAlso oGetRIModelLineDetailsDataset.Row IsNot Nothing Then
                    oResponse.Lines = oGetRIModelLineDetailsDataset.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseGetRIModelLineDetailsResponseTypeLinesRow, BaseGetRIModelLineDetailsResponseTypeLinesRow)(AddressOf CommonFunctions.ToServiceRIModelLinesList))
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function

    Public Function FindReinsurer(ByVal oFindReinsurerRequest As FindReinsurerRequestType) As FindReinsurerResponseType Implements IPureService.FindReinsurer

        Try

            Dim sUserName As String = oFindReinsurerRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFINDRI", iUserId)
            Dim oResponse As New FindReinsurerResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindReinsurerRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindReinsurerRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindReinsurerResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindReinsurerRequest.BranchCode
            oImpRequest.FileCode = oFindReinsurerRequest.FileCode
            oImpRequest.IsBroker = oFindReinsurerRequest.IsBroker
            oImpRequest.IsBrokerSpecified = oFindReinsurerRequest.IsBrokerSpecified
            oImpRequest.IsRetained = oFindReinsurerRequest.IsRetained
            oImpRequest.IsRetainedSpecified = oFindReinsurerRequest.IsRetainedSpecified
            oImpRequest.RICode = oFindReinsurerRequest.RICode
            oImpRequest.RIName = oFindReinsurerRequest.RIName
            oImpRequest.RITypeCode = oFindReinsurerRequest.RITypeCode
            oImpRequest.IsFAX = oFindReinsurerRequest.IsFAX
            oImpRequest.IsFAXSpecified = oFindReinsurerRequest.IsFAXSpecified
            oImpRequest.IncludeClosedBranches = oFindReinsurerRequest.IncludeClosedBranches

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.FindReinsurer(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseFindReinsurerResponseTypeReinsurers), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseFindReinsurerResponseTypeReinsurersRow), oXmlAttributes)
                Dim oFindReinsurerDataset As SFI.SAMForInsuranceV2.BaseFindReinsurerResponseTypeReinsurers = Nothing
                Dim FindReinsurerDataSetObject As Object = Nothing
                Dim oXMLSerializerUserDetails As New XmlSerializer(GetType(SFI.SAMForInsuranceV2.BaseFindReinsurerResponseTypeReinsurers), oXmlOverride)
                If oImpResponse.Reinsurers IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.Reinsurers.OuterXml, oXMLSerializer:=oXMLSerializerUserDetails, r_oResultDataSet:=FindReinsurerDataSetObject)
                    oFindReinsurerDataset = DirectCast(FindReinsurerDataSetObject, SFI.SAMForInsuranceV2.BaseFindReinsurerResponseTypeReinsurers)
                End If

                If oFindReinsurerDataset IsNot Nothing AndAlso oFindReinsurerDataset.Row IsNot Nothing Then
                    oResponse.Reinsurers = oFindReinsurerDataset.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseFindReinsurerResponseTypeReinsurersRow, BaseFindReinsurerResponseTypeReinsurersRow)(AddressOf CommonFunctions.ToServiceReinsurerList))
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function

    Public Function GetTreatyPartyDetails(ByVal oGetTreatyPartyDetailsRequest As GetTreatyPartyDetailsRequestType) As GetTreatyPartyDetailsResponseType Implements IPureService.GetTreatyPartyDetails

        Try

            Dim sUserName As String = oGetTreatyPartyDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGTtyPty", iUserId)
            Dim oResponse As New GetTreatyPartyDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetTreatyPartyDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetTreatyPartyDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetTreatyPartyDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetTreatyPartyDetailsRequest.BranchCode
            oImpRequest.TreatyCode = oGetTreatyPartyDetailsRequest.TreatyCode

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetTreatyPartyDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetTreatyPartyDetailsResponseTypeParties), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetTreatyPartyDetailsResponseTypePartiesRow), oXmlAttributes)
                Dim oGetTreatyPartyDetailsDataset As SFI.SAMForInsuranceV2.BaseGetTreatyPartyDetailsResponseTypeParties = Nothing
                Dim GetTreatyPartyDetailsDataSetObject As Object = Nothing
                Dim oXMLSerializerUserDetails As New XmlSerializer(GetType(SFI.SAMForInsuranceV2.BaseGetTreatyPartyDetailsResponseTypeParties), oXmlOverride)
                If oImpResponse.Parties IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.Parties.OuterXml, oXMLSerializer:=oXMLSerializerUserDetails, r_oResultDataSet:=GetTreatyPartyDetailsDataSetObject)
                    oGetTreatyPartyDetailsDataset = DirectCast(GetTreatyPartyDetailsDataSetObject, SFI.SAMForInsuranceV2.BaseGetTreatyPartyDetailsResponseTypeParties)
                End If

                If oGetTreatyPartyDetailsDataset IsNot Nothing AndAlso oGetTreatyPartyDetailsDataset.Row IsNot Nothing Then
                    oResponse.Parties = oGetTreatyPartyDetailsDataset.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseGetTreatyPartyDetailsResponseTypePartiesRow, BaseGetTreatyPartyDetailsResponseTypePartiesRow)(AddressOf CommonFunctions.ToServiceTreatyPartiesList))
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function

    Public Function GetClaimRIArrangementLinesRI2007(ByVal GetClaimRIArrangementLinesRI2007Request As GetClaimRIArrangementLinesRI2007RequestType) As GetClaimRIArrangementLinesRI2007ResponseType Implements IPureService.GetClaimRIArrangementLinesRI2007
        Try

            Dim sUserName As String = GetClaimRIArrangementLinesRI2007Request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCLRI07", iUserId)
            Dim oResponse As New GetClaimRIArrangementLinesRI2007ResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimRIArrangementLinesRI2007Request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimRIArrangementLinesRI2007RequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimRIArrangementLinesRI2007ResponseType '= Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimRIArrangementLinesRI2007Request.BranchCode
            oImpRequest.ClaimKey = GetClaimRIArrangementLinesRI2007Request.ClaimKey
            oImpRequest.ArrangementKey = GetClaimRIArrangementLinesRI2007Request.ArrangementKey
            oImpRequest.ModeSpecified = GetClaimRIArrangementLinesRI2007Request.ModeSpecified
            If (GetClaimRIArrangementLinesRI2007Request.ModeSpecified) Then
                oImpRequest.Mode = GetClaimRIArrangementLinesRI2007Request.Mode
            End If
            oImpRequest.IsRecoverySpecified = GetClaimRIArrangementLinesRI2007Request.IsRecoverySpecified
            If (GetClaimRIArrangementLinesRI2007Request.IsRecoverySpecified) Then
                oImpRequest.IsRecovery = GetClaimRIArrangementLinesRI2007Request.IsRecovery
            End If
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimRIArrangementLinesRI2007(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If (oImpResponse.RIArrangementLines IsNot Nothing) Then
                    oResponse.RIArrangementLines = oImpResponse.RIArrangementLines.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClaimRiskRIArrangementLineType, BaseClaimRiskRIArrangementLineType)(AddressOf CommonFunctions.ToServiceClaimRiskRIArrangementLineTypeList))
                End If
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function

    Public Function UpdateArrangementLinesRI2007(ByVal UpdateArrangementLinesRI2007Request As UpdateArrangementLinesRI2007RequestType) As UpdateArrangementLinesRI2007ResponseType Implements IPureService.UpdateArrangementLinesRI2007
        Try

            Dim sUserName As String = UpdateArrangementLinesRI2007Request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURI07", iUserId)
            Dim oResponse As New UpdateArrangementLinesRI2007ResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateArrangementLinesRI2007Request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateArrangementLinesRI2007RequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateArrangementLinesRI2007ResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateArrangementLinesRI2007Request.BranchCode
            If (UpdateArrangementLinesRI2007Request.RIArrangementLines IsNot Nothing) Then
                With UpdateArrangementLinesRI2007Request
                    ReDim oImpRequest.RIArrangementLines(.RIArrangementLines.Count - 1)
                    For iCount As Integer = 0 To .RIArrangementLines.Count - 1
                        oImpRequest.RIArrangementLines(iCount) = New BaseImplementationTypes.BaseRiskRIArrangementLineType
                        oImpRequest.RIArrangementLines(iCount).RIArrangementKey = .RIArrangementLines(iCount).RIArrangementKey
                        oImpRequest.RIArrangementLines(iCount).RIArrangementLineKey = .RIArrangementLines(iCount).RIArrangementLineKey
                        oImpRequest.RIArrangementLines(iCount).RIPlacement = .RIArrangementLines(iCount).RIPlacement
                        oImpRequest.RIArrangementLines(iCount).RIName = .RIArrangementLines(iCount).RIName
                        oImpRequest.RIArrangementLines(iCount).Retained = .RIArrangementLines(iCount).Retained
                        oImpRequest.RIArrangementLines(iCount).DefaultSharePercent = .RIArrangementLines(iCount).DefaultSharePercent / 100

                        oImpRequest.RIArrangementLines(iCount).ThisSharePercent = .RIArrangementLines(iCount).ThisSharePercent / 100
                        oImpRequest.RIArrangementLines(iCount).LowerLimit = .RIArrangementLines(iCount).LowerLimit
                        oImpRequest.RIArrangementLines(iCount).LowerLimitSpecified = .RIArrangementLines(iCount).LowerLimitSpecified

                        oImpRequest.RIArrangementLines(iCount).PartyKeySpecified = .RIArrangementLines(iCount).PartyKeySpecified
                        oImpRequest.RIArrangementLines(iCount).PremiumTaxSpecified = .RIArrangementLines(iCount).PremiumTaxSpecified
                        oImpRequest.RIArrangementLines(iCount).RetainedSpecified = .RIArrangementLines(iCount).RetainedSpecified

                        oImpRequest.RIArrangementLines(iCount).LineLimit = .RIArrangementLines(iCount).LineLimit
                        oImpRequest.RIArrangementLines(iCount).SumInsured = .RIArrangementLines(iCount).SumInsured
                        oImpRequest.RIArrangementLines(iCount).PremiumValue = .RIArrangementLines(iCount).PremiumValue
                        oImpRequest.RIArrangementLines(iCount).PremiumTax = .RIArrangementLines(iCount).PremiumTax
                        oImpRequest.RIArrangementLines(iCount).CommissionTax = .RIArrangementLines(iCount).CommissionTax
                        oImpRequest.RIArrangementLines(iCount).CommissionTaxSpecified = .RIArrangementLines(iCount).CommissionTaxSpecified
                        oImpRequest.RIArrangementLines(iCount).CommissionPercent = .RIArrangementLines(iCount).CommissionPercent / 100
                        oImpRequest.RIArrangementLines(iCount).CommissionValue = .RIArrangementLines(iCount).CommissionValue
                        oImpRequest.RIArrangementLines(iCount).AgreementCode = .RIArrangementLines(iCount).AgreementCode
                        oImpRequest.RIArrangementLines(iCount).IsDomiciledForTax = .RIArrangementLines(iCount).IsDomiciledForTax
                        oImpRequest.RIArrangementLines(iCount).Grouping = .RIArrangementLines(iCount).Grouping
                        oImpRequest.RIArrangementLines(iCount).GroupingSpecified = .RIArrangementLines(iCount).GroupingSpecified
                        oImpRequest.RIArrangementLines(iCount).IsRIBroker = .RIArrangementLines(iCount).IsRIBroker
                        oImpRequest.RIArrangementLines(iCount).ReinsuranceTypeCode = .RIArrangementLines(iCount).ReinsuranceTypeCode
                        oImpRequest.RIArrangementLines(iCount).TreatyCode = .RIArrangementLines(iCount).TreatyCode
                        oImpRequest.RIArrangementLines(iCount).PartyKey = .RIArrangementLines(iCount).PartyKey
                        oImpRequest.RIArrangementLines(iCount).Priority = .RIArrangementLines(iCount).Priority
                        oImpRequest.RIArrangementLines(iCount).NumberOfLines = .RIArrangementLines(iCount).NumberOfLines
                        oImpRequest.RIArrangementLines(iCount).PremiumPercent = .RIArrangementLines(iCount).PremiumPercent / 100
                        oImpRequest.RIArrangementLines(iCount).ParticipationPercent = .RIArrangementLines(iCount).ParticipationPercent
                        oImpRequest.RIArrangementLines(iCount).ParticipationPercentSpecified = .RIArrangementLines(iCount).ParticipationPercentSpecified
                        oImpRequest.RIArrangementLines(iCount).IsCommissionModified = .RIArrangementLines(iCount).IsCommissionModified
                        oImpRequest.RIArrangementLines(iCount).CedePremiumOnly = .RIArrangementLines(iCount).CedePremiumOnly
                        oImpRequest.RIArrangementLines(iCount).Type = .RIArrangementLines(iCount).Type
                        oImpRequest.RIArrangementLines(iCount).ActionType = CType(.RIArrangementLines(iCount).ActionType, BaseImplementationTypes.RowAction)
                        If (.RIArrangementLines(iCount).BrokerParticipants IsNot Nothing) Then
                            ReDim oImpRequest.RIArrangementLines(iCount).BrokerParticipants(.RIArrangementLines(iCount).BrokerParticipants.Count - 1)
                            For iCount1 As Integer = 0 To .RIArrangementLines(iCount).BrokerParticipants.Count - 1
                                oImpRequest.RIArrangementLines(iCount).BrokerParticipants(iCount1) = New BaseImplementationTypes.BaseBrokerParticipants
                                oImpRequest.RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyKey = .RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyKey
                                oImpRequest.RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyCode = .RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyCode
                                oImpRequest.RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyName = .RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyName
                                oImpRequest.RIArrangementLines(iCount).BrokerParticipants(iCount1).ParticpationPercentage = .RIArrangementLines(iCount).BrokerParticipants(iCount1).ParticpationPercentage
                            Next
                        End If

                        If (.RIArrangementLines(iCount).FAXParticipants IsNot Nothing) Then
                            ReDim oImpRequest.RIArrangementLines(iCount).FAXParticipants(.RIArrangementLines(iCount).FAXParticipants.Count - 1)
                            For iCount1 As Integer = 0 To .RIArrangementLines(iCount).FAXParticipants.Count - 1
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1) = New BaseImplementationTypes.BaseFAXParticipants
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).RIArrangementLineKey = .RIArrangementLines(iCount).FAXParticipants(iCount1).RIArrangementLineKey
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PartyKey = .RIArrangementLines(iCount).FAXParticipants(iCount1).PartyKey
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PartyCode = .RIArrangementLines(iCount).FAXParticipants(iCount1).PartyCode
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PartyName = .RIArrangementLines(iCount).FAXParticipants(iCount1).PartyName
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).AccountType = .RIArrangementLines(iCount).FAXParticipants(iCount1).AccountType
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).ParticpationPercentage = .RIArrangementLines(iCount).FAXParticipants(iCount1).ParticpationPercentage
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionTaxSpecified = .RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionTaxSpecified
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumTaxSpecified = .RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumTaxSpecified
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).SumInsured = .RIArrangementLines(iCount).FAXParticipants(iCount1).SumInsured
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumValue = .RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumValue
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumTax = .RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumTax
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionPercent = .RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionPercent / 100
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionTax = .RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionTax
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionValue = .RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionValue
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).AgreementCode = .RIArrangementLines(iCount).FAXParticipants(iCount1).AgreementCode
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).ActionType = CType(.RIArrangementLines(iCount).FAXParticipants(iCount1).ActionType, BaseImplementationTypes.RowAction)
                                If (.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants IsNot Nothing) Then
                                    ReDim oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants.Count - 1)
                                    For iCount2 As Integer = 0 To .RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants.Count - 1
                                        oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2) = New BaseImplementationTypes.BaseBrokerParticipants
                                        oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyKey = .RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyKey
                                        oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyCode = .RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyCode
                                        oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyName = .RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyName
                                        oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).ParticpationPercentage = .RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).ParticpationPercentage
                                    Next
                                End If
                            Next
                        End If
                    Next
                End With
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateArrangementLinesRI2007(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function
    Public Function UpdateClaimRIArrangementLinesRI2007(ByVal UpdateClaimRIArrangementLinesRI2007Request As UpdateClaimRIArrangementLinesRI2007RequestType) As UpdateClaimRIArrangementLinesRI2007ResponseType Implements IPureService.UpdateClaimRIArrangementLinesRI2007
        Try

            Dim sUserName As String = UpdateClaimRIArrangementLinesRI2007Request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUCLRI07", iUserId)
            Dim oResponse As New UpdateClaimRIArrangementLinesRI2007ResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateClaimRIArrangementLinesRI2007Request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateClaimRIArrangementLinesRI2007RequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateClaimRIArrangementLinesRI2007ResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateClaimRIArrangementLinesRI2007Request.BranchCode
            If (UpdateClaimRIArrangementLinesRI2007Request.ClaimRIArrangementLines IsNot Nothing) Then
                With UpdateClaimRIArrangementLinesRI2007Request
                    ReDim oImpRequest.ClaimRIArrangementLines(.ClaimRIArrangementLines.Count - 1)
                    For iCount As Integer = 0 To .ClaimRIArrangementLines.Count - 1
                        oImpRequest.ClaimRIArrangementLines(iCount) = New BaseImplementationTypes.BaseClaimRiskRIArrangementLineType
                        oImpRequest.ClaimRIArrangementLines(iCount).ActionType = CType(.ClaimRIArrangementLines(iCount).ActionType, BaseImplementationTypes.RowAction)
                        oImpRequest.ClaimRIArrangementLines(iCount).RIArrangementKey = .ClaimRIArrangementLines(iCount).RIArrangementKey
                        oImpRequest.ClaimRIArrangementLines(iCount).RIArrangementLineKey = .ClaimRIArrangementLines(iCount).RIArrangementLineKey
                        oImpRequest.ClaimRIArrangementLines(iCount).RIPlacement = .ClaimRIArrangementLines(iCount).RIPlacement
                        oImpRequest.ClaimRIArrangementLines(iCount).RIName = .ClaimRIArrangementLines(iCount).RIName
                        oImpRequest.ClaimRIArrangementLines(iCount).Type = .ClaimRIArrangementLines(iCount).Type
                        oImpRequest.ClaimRIArrangementLines(iCount).Retained = .ClaimRIArrangementLines(iCount).Retained / 100
                        oImpRequest.ClaimRIArrangementLines(iCount).DefaultSharePercent = .ClaimRIArrangementLines(iCount).DefaultSharePercent
                        oImpRequest.ClaimRIArrangementLines(iCount).ThisSharePercent = .ClaimRIArrangementLines(iCount).ThisSharePercent
                        oImpRequest.ClaimRIArrangementLines(iCount).LowerLimit = .ClaimRIArrangementLines(iCount).LowerLimit
                        oImpRequest.ClaimRIArrangementLines(iCount).LineLimit = .ClaimRIArrangementLines(iCount).LineLimit
                        oImpRequest.ClaimRIArrangementLines(iCount).AgreementCode = .ClaimRIArrangementLines(iCount).AgreementCode
                        oImpRequest.ClaimRIArrangementLines(iCount).IsDomiciledForTax = .ClaimRIArrangementLines(iCount).IsDomiciledForTax
                        oImpRequest.ClaimRIArrangementLines(iCount).Grouping = .ClaimRIArrangementLines(iCount).Grouping
                        oImpRequest.ClaimRIArrangementLines(iCount).IsRIBroker = .ClaimRIArrangementLines(iCount).IsRIBroker
                        oImpRequest.ClaimRIArrangementLines(iCount).ReinsuranceTypeCode = .ClaimRIArrangementLines(iCount).ReinsuranceTypeCode
                        oImpRequest.ClaimRIArrangementLines(iCount).TreatyCode = .ClaimRIArrangementLines(iCount).TreatyCode
                        oImpRequest.ClaimRIArrangementLines(iCount).PartyKey = .ClaimRIArrangementLines(iCount).PartyKey
                        oImpRequest.ClaimRIArrangementLines(iCount).Priority = .ClaimRIArrangementLines(iCount).Priority
                        oImpRequest.ClaimRIArrangementLines(iCount).NumberOfLines = .ClaimRIArrangementLines(iCount).NumberOfLines
                        oImpRequest.ClaimRIArrangementLines(iCount).CedePremiumOnly = .ClaimRIArrangementLines(iCount).CedePremiumOnly
                        oImpRequest.ClaimRIArrangementLines(iCount).SumInsured = .ClaimRIArrangementLines(iCount).SumInsured
                        oImpRequest.ClaimRIArrangementLines(iCount).Incurred = .ClaimRIArrangementLines(iCount).Incurred
                        oImpRequest.ClaimRIArrangementLines(iCount).ReserveToDate = .ClaimRIArrangementLines(iCount).ReserveToDate
                        oImpRequest.ClaimRIArrangementLines(iCount).ThisReserve = .ClaimRIArrangementLines(iCount).ThisReserve
                        oImpRequest.ClaimRIArrangementLines(iCount).PaymentToDate = .ClaimRIArrangementLines(iCount).PaymentToDate
                        oImpRequest.ClaimRIArrangementLines(iCount).ThisPayment = .ClaimRIArrangementLines(iCount).ThisPayment
                        oImpRequest.ClaimRIArrangementLines(iCount).RecoverToDate = .ClaimRIArrangementLines(iCount).RecoverToDate
                        oImpRequest.ClaimRIArrangementLines(iCount).Balance = .ClaimRIArrangementLines(iCount).Balance

                        If (.ClaimRIArrangementLines(iCount).BrokerParticipants IsNot Nothing) Then
                            ReDim oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(.ClaimRIArrangementLines(iCount).BrokerParticipants.Count - 1)
                            For iCount1 As Integer = 0 To .ClaimRIArrangementLines(iCount).BrokerParticipants.Count - 1
                                oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1) = New BaseImplementationTypes.BaseBrokerParticipants
                                oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyKey = .ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyKey
                                oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyCode = .ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyCode
                                oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyName = .ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyName
                                oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).ParticpationPercentage = .ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).ParticpationPercentage
                            Next
                        End If

                        If (.ClaimRIArrangementLines(iCount).FAXParticipants IsNot Nothing) Then
                            ReDim oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(.ClaimRIArrangementLines(iCount).FAXParticipants.Count - 1)
                            For iCount1 As Integer = 0 To .ClaimRIArrangementLines(iCount).FAXParticipants.Count - 1
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1) = New BaseImplementationTypes.BaseClaimFAXParticipants
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ActionType = CType(.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ActionType, BaseImplementationTypes.RowAction)
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).RIArrangementLineKey = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).RIArrangementLineKey
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyKey = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyKey
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyCode = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyCode
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyName = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyName
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).AccountType = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).AccountType
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ParticpationPercentage = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ParticpationPercentage
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).SumInsured = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).SumInsured
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).AgreementCode = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).AgreementCode
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ReserveToDate = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ReserveToDate
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ThisReserve = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ThisReserve
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PaymentToDate = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PaymentToDate
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ThisPayment = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ThisPayment
                                If (.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants IsNot Nothing) Then
                                    ReDim oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants.Count - 1)
                                    For iCount2 As Integer = 0 To .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants.Count - 1
                                        oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2) = New BaseImplementationTypes.BaseBrokerParticipants
                                        oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyKey = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyKey
                                        oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyCode = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyCode
                                        oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyName = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyName
                                        oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).ParticpationPercentage = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).ParticpationPercentage
                                    Next
                                End If
                            Next
                        End If
                    Next
                End With
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateClaimRIArrangementLinesRI2007(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oImpRequest)
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function

    Public Function CalculateRITax(ByVal CalculateRITaxRequest As CalculateRITaxRequestType) As CalculateRITaxResponseType Implements IPureService.CalculateRITax
        Try

            Dim sUserName As String = CalculateRITaxRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURI07", iUserId)
            Dim oResponse As New CalculateRITaxResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, CalculateRITaxRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CalculateRITaxRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CalculateRITaxResponseType = Nothing

            oImpRequest.BranchCode = CalculateRITaxRequest.BranchCode
            oImpRequest.InsuranceFileKey = CalculateRITaxRequest.InsuranceFileKey
            oImpRequest.RiskKey = CalculateRITaxRequest.RiskKey
            oImpRequest.PartyKey = CalculateRITaxRequest.PartyKey
            oImpRequest.RIArrangementLineKey = CalculateRITaxRequest.RIArrangementLineKey
            oImpRequest.RIArrangementLineKeySpecified = CalculateRITaxRequest.RIArrangementLineKeySpecified
            oImpRequest.Premium = CalculateRITaxRequest.Premium
            oImpRequest.Commission = CalculateRITaxRequest.Commission

            Try
                ' Call the implementation method

                oImpResponse = oBusiness.CalculateRITax(oImpRequest)
                oResponse.CommissionTax = oImpResponse.CommissionTax
                oResponse.PremiumTax = oImpResponse.PremiumTax
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function

    Public Function UpdateRiskStatus(ByVal oUpdateRiskStatusRequest As UpdateRiskStatusRequestType) As UpdateRiskStatusResponseType Implements IPureService.updateriskstatus

        Try

            Dim sUserName As String = oUpdateRiskStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            If Not String.IsNullOrEmpty(oUpdateRiskStatusRequest.LoginUserName) Then
                sUserName = oUpdateRiskStatusRequest.LoginUserName.ToString
                CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
                CommonFunctions.CheckAuthority("SAMURsk", iUserId)
            Else
                CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
                CommonFunctions.CheckAuthority("SAMURsk", iUserId)
            End If

            Dim oResponse As New UpdateRiskStatusResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateRiskStatusRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateRiskStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateRiskStatusResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateRiskStatusRequest.BranchCode
            If oUpdateRiskStatusRequest.InsuranceFileKeySpecified Then
                oImpRequest.InsuranceFileKey = oUpdateRiskStatusRequest.InsuranceFileKey
            End If
            If oUpdateRiskStatusRequest.RiskKeySpecified Then
                oImpRequest.RiskKey = oUpdateRiskStatusRequest.RiskKey
            End If
            oImpRequest.RiskStatusCode = oUpdateRiskStatusRequest.RiskStatusCode.ToString

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateRiskStatus(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function
End Class
