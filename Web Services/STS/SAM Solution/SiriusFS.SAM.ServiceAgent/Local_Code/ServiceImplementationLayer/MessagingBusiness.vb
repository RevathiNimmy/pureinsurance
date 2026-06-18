Option Strict On

Imports SiriusFS.SAM.CoreImplementation
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.Structure.MessagingImplementationTypes
Imports STSCoreStruct = SiriusFS.SAM.Structure.Core
Imports Microsoft.ApplicationBlocks.Data
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports SiriusFS.SAM.CoreImplementation.InternalSAMConstants
Imports SiriusFS.SAM.CoreImplementation.SAMFunc
Imports SiriusFS.SAM.Structure.BaseImplementationTypes

Friend Class MessagingBusiness

    Private Const DataModelCode As String = "SGT5"

    Private Const CNClaimMode As String = "CLAIM_MODE"
    Private Const CNDataModel As String = "CONTROL__XML_DATAMODEL"
    Private Const CNNBTransactMessage As String = "NBTRANSACT_MESSAGE"
    Private Const CNNewPolicyNumber As String = "NEW_POLICY_NUMBER"
    Private Const Header_BusinessType As String = "NB"
    Private Const CNPartyCnt As String = "CONTROL__PARTY_CNT"
    Private Const CNInsuranceFolderCnt As String = "CONTROL__INSURANCE_FOLDER_CNT"
    Private Const CNInsuranceFileCnt As String = "CONTROL__INSURANCE_FILE_CNT"
    Private Const CNRiskCnt As String = "CONTROL__RISK_CNT"

    Private Const m_sDefaultNameSpace As String = "http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429"
    Public Function GetDatasetSchema(ByVal GetDatasetSchemaRequest As BaseGetDatasetSchemaRequestType) As BaseGetDatasetSchemaResponseType

        Dim oGetDatasetSchemaResponse As BaseImplementationTypes.BaseGetDatasetSchemaResponseType
        ' Declare the Response object
        Dim oResponse As New BaseGetDatasetSchemaResponseType

        ' Declare the Core SAM business object
        Dim oBusiness As New CoreSAMBusiness

        ' Process the GetDatasetSchema by calling the implementation method 
        oGetDatasetSchemaResponse = oBusiness.GetDatasetSchema(GetDatasetSchemaRequest)

        oResponse.DatasetSchema = oGetDatasetSchemaResponse.DatasetSchema
        oResponse.STSError = oGetDatasetSchemaResponse.STSError

        Return oResponse

    End Function

    ' ***************************************************************** '
    ' Name: NewBusiness
    '
    ' Description: This method is the implementation of the NewBusiness 
    '              Web Method on the Messaging service
    '
    ' ***************************************************************** '
    Public Function NewBusiness(ByVal NewBusinessRequest As NewBusinessRequestType) As NewBusinessResponseType

        Const ACMethodName As String = "NewBusiness"

        ' Declare the Response object
        Dim oResponse As New NewBusinessResponseType

        ' Declare the Core SAM business object
        Dim oBusiness As New CoreSAMBusiness

        Dim oAddPartyRequest As New BaseImplementationTypes.BaseAddPartyRequestType
        Dim STSError As New STSErrorPublisher

        ' Check if the branch code was passed in
        If SAMFunc.NothingToString(NewBusinessRequest.BranchCode) = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        ' Check if the client structure was passed in
        If (NewBusinessRequest.Party Is Nothing) = True Then
            STSError.AddInvalidField("Client", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Client"), "")
        End If

        ' Check if the policy structure was passed in
        If (NewBusinessRequest.Policy Is Nothing) = True Then
            STSError.AddInvalidField("Policy", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Policy"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        ' Add the Party record by calling the implementation method 
        oAddPartyRequest.BranchCode = NewBusinessRequest.BranchCode
        oAddPartyRequest.Party = NewBusinessRequest.Party
        oResponse.Insured = oBusiness.AddParty( _
                    AddPartyRequest:=oAddPartyRequest)

        ' Check response for errors
        If (oResponse.Insured.STSError Is Nothing) = False Then
            oResponse.STSError = oResponse.Insured.STSError
            Return oResponse
        End If

        ' Set the Party ID on the Policy request structure 
        NewBusinessRequest.Policy.PartyKey = oResponse.Insured.PartyKey

        ' Determine the Party Type
        If NewBusinessRequest.Party.GetType Is GetType(BasePartyPCType) Then
            ' Process personal client
            Dim oBasePartyPC As BasePartyPCType = DirectCast(NewBusinessRequest.Party, BasePartyPCType)
            If NewBusinessRequest.Policy.InsuredName = "" Then
                NewBusinessRequest.Policy.InsuredName = oBasePartyPC.Title & " " & oBasePartyPC.Initials & " " & oBasePartyPC.Surname
            End If
        Else
            ' Process corporate client
            Dim oBasePartyCC As BasePartyCCType = DirectCast(NewBusinessRequest.Party, BasePartyCCType)
            If NewBusinessRequest.Policy.InsuredName = "" Then
                NewBusinessRequest.Policy.InsuredName = oBasePartyCC.CompanyName
            End If
        End If

        ' Add the Party record by calling the implementation method 
        ReDim oResponse.Policy(0)
        oResponse.Policy(0) = oBusiness.AddPolicy(NewBusinessRequest.Policy)

        ' Check response for errors
        If (oResponse.Policy(0).STSError Is Nothing) = False Then
            oResponse.STSError = oResponse.Policy(0).STSError
            Return oResponse
        End If

        Return oResponse

    End Function

    Public Function NBTransact(ByVal NBTransactionRequest As NBTransactRequestType) As NBTransactResponseType

        Dim oNBTransactionResponse As BaseImplementationTypes.BaseTransactResponseType
        ' Declare the Response object
        Dim oResponse As New NBTransactResponseType

        ' Declare the Core SAM business object
        Dim oBusiness As New CoreSAMBusiness

        ' Process the NBTransact by calling the implementation method 
        oNBTransactionResponse = oBusiness.NBTransact(NBTransactionRequest)

        oResponse.Policy = oNBTransactionResponse.Policy
        oResponse.STSError = oNBTransactionResponse.STSError

        Return oResponse

    End Function

    Public Function ProcessClaim(ByVal oRequest As BaseImplementationTypes.BaseClaimProcessRequestType) As BaseImplementationTypes.BaseClaimProcessResponseType

        Dim oSFIBusiness As New CoreSAMBusiness("sirius", oRequest.BranchCode)
        Dim oResponse As New BaseImplementationTypes.BaseClaimProcessResponseType
        Dim oSAMErrorCollection As New SAMErrorCollection

        Try

            'Validate Request
            oRequest.Validate(CObj(oSAMErrorCollection))
            oSAMErrorCollection.CheckForErrors()

            If Not oRequest.Claim.ClaimBuilderDetail Is Nothing Then
                Dim xDoc As New XmlDocument()

                'Validate the XPath values XML
                Dim xNode As XmlNode
                For Each oAmend As BaseImplementationTypes.BaseClaimProcessBuilderRiskType In oRequest.Claim.ClaimBuilderDetail
                    Dim xpath As String = oAmend.ClaimBuilderData.ItemName
                    Try
                        xNode = xDoc.SelectSingleNode(xpath)
                    Catch exXpathException As XPathException
                        oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.InvalidXpathString, SAMConstants.SAMInvalidData.InvalidXpathString.ToString, "ClaimsBuilderData.ItemName", xpath)
                    End Try
                Next
            End If

            oSAMErrorCollection.CheckForErrors()

            If Not oRequest.Claim.ClaimPeril Is Nothing Then
                For lCount As Int32 = 0 To oRequest.Claim.ClaimPeril.Length - 1
                    If oRequest.Claim.ClaimPeril(lCount).TypeCode = String.Empty Then
                        Using con As SiriusConnection = SiriusConnection.FromAny(SAMFunc.ConnectionString)
                            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Peril_types")
                                Dim dtPerilTypeCode As DataTable = con.ExecuteDataTable(cmd)
                                If dtPerilTypeCode IsNot Nothing AndAlso dtPerilTypeCode.Rows.Count = 1 Then
                                    oRequest.Claim.ClaimPeril(lCount).TypeCode = Cast.ToStringTrim(dtPerilTypeCode.Rows(0).Item("Code"), String.Empty)
                                    If oRequest.Claim.ClaimPeril(lCount).Description = String.Empty Then
                                        oRequest.Claim.ClaimPeril(lCount).Description = oRequest.Claim.ClaimPeril(lCount).TypeCode
                                    End If
                                End If
                            End Using
                        End Using
                    End If
                Next
            End If

            'New or Existing Claim
            If oRequest.Claim.BaseClaimKey <> 0 Then

                Dim oGetVersionRequest As New SAMForInsuranceV2ImplementationTypes.GetVersionsForClaimRequestType
                Dim oGetVersionResponse As New BaseImplementationTypes.BaseGetVersionsForClaimResponseType
                oGetVersionRequest.BranchCode = oRequest.BranchCode
                If oRequest.ClaimNumber = String.Empty Then
                    oRequest.ClaimNumber = "TEMP"
                End If
                oGetVersionRequest.ClaimKey = oRequest.ClaimNumber
                oGetVersionRequest.BaseClaimKey = oRequest.Claim.BaseClaimKey
                oGetVersionResponse = oSFIBusiness.GetVersionsForClaim(oGetVersionRequest)

                CheckForErrors(oResponse, oGetVersionResponse.Errors, "GetVersionsForClaim")

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseGetVersionsForClaimResponseTypeVersions), oXmlAttributes)
                oXmlOverride.Add(GetType(SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseGetVersionsForClaimResponseTypeVersionsRow), oXmlAttributes)

                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseGetVersionsForClaimResponseTypeVersions = Nothing
                Dim oResultDataSetObject As Object = Nothing
                Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseGetVersionsForClaimResponseTypeVersions), oXmlOverride)
                If oGetVersionResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oGetVersionResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseGetVersionsForClaimResponseTypeVersions)
                    ' Retrieve the values from the implementation response structure
                    oRequest.ClaimKey = oResultDataSet.Row(0).ClaimKey
                    oRequest.ClaimNumber = oResultDataSet.Row(0).claim_number
                End If

                GetClaimDetails(oRequest, oSFIBusiness, oResponse)
                If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                    Return oResponse
                End If

                MaintainClaim(oRequest, oSFIBusiness, oResponse)
                If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                    Return oResponse
                End If

                'GetClaimRisk(oRequest, oSFIBusiness, oResponse)
                'If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                '    Return oResponse
                'End If

                'ProcessClaimRiskData(oRequest, oResponse)
                'If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                '    Return oResponse
                'End If

            Else
                OpenClaim(oRequest, oSFIBusiness, oResponse)
                If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                    Return oResponse
                End If
                AddClaimRisk(oRequest, oSFIBusiness, oResponse)
                If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                    Return oResponse
                End If
                ProcessClaimRiskData(oRequest, oResponse)
                If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                    Return oResponse
                End If
            End If

            UpdateClaimReservesOrPayments(oRequest, oSFIBusiness, oResponse)
            If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                Return oResponse
            End If

            BindClaim(oRequest, False, oSFIBusiness, oResponse)
            If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                Return oResponse
            End If

            'these two sub routines shoud be called after BindClaim 
            If oRequest.Claim.BaseClaimKey <> 0 Then
                GetClaimRisk(oRequest, oSFIBusiness, oResponse)
                If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                    Return oResponse
                End If

                ProcessClaimRiskData(oRequest, oResponse)
                If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                    Return oResponse
                End If
            End If


            UpdateClaimRisk(oRequest, oSFIBusiness, oResponse)
            If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                Return oResponse
            End If

            GetClaimDetails(oRequest, oSFIBusiness, oResponse)
            ClaimReceipt(oRequest, oSFIBusiness, oResponse)
            If oRequest.Claim.IgnoreWarnings = False And (oResponse.Warnings Is Nothing) = False Then
                Return oResponse
            End If

            If oRequest.IsPaymentRequired Then
                GetClaimDetails(oRequest, oSFIBusiness, oResponse)
                PayClaim(oRequest, oSFIBusiness, oResponse)
            End If

            With oResponse
                .BaseClaimKey = oRequest.BaseClaimKey
                .ClaimKey = oRequest.ClaimKey
                .ClaimNumber = oRequest.ClaimNumber
                .TimeStamp = oRequest.TimeStamp
                .Version = oRequest.ClaimVersion
            End With

        Catch SAMException As Sirius.Architecture.ExceptionHandling.SAMErrorException
            Throw SAMException
        Catch ex As Exception
            Handler.BusinessLayerBoundary(ex, oResponse)
        End Try

        Return oResponse
    End Function

    Private Sub GetClaimDetails(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByVal oSFIBusiness As CoreSAMBusiness, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType)

        Dim oGetClaimDetailsRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimDetailsRequestType
        Dim oGetClaimDetailsResponse As New BaseImplementationTypes.BaseGetClaimDetailsResponseType

        oGetClaimDetailsRequest.BranchCode = oRequest.BranchCode
        oGetClaimDetailsRequest.ClaimKey = oRequest.ClaimKey

        oGetClaimDetailsResponse = oSFIBusiness.GetClaimDetails(oGetClaimDetailsRequest)
        CheckForErrors(oResponse, oGetClaimDetailsResponse.Errors, "GetClaimDetails")
        If oGetClaimDetailsResponse.TimeStamp IsNot Nothing Then
            oRequest.TimeStamp = oGetClaimDetailsResponse.TimeStamp
        End If
        oRequest.BaseClaimKey = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.BaseClaimKey
        oRequest.ClaimDetails = New BaseGetClaimDetailsType
        oRequest.ClaimDetails.ClaimDetails = New BaseGetClaimDetailsTypeClaimDetails
        oRequest.ClaimDetails.ClaimDetails.AgentCnt = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.AgentCnt
        oRequest.ClaimDetails.ClaimDetails.BaseClaimKey = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.BaseClaimKey
        oRequest.ClaimDetails.ClaimDetails.CatastropheCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.CatastropheCode
        oRequest.ClaimDetails.ClaimDetails.CatastropheId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.CatastropheId
        oRequest.ClaimDetails.ClaimDetails.ClaimId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClaimId
        oRequest.ClaimDetails.ClaimDetails.ClaimKey = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClaimKey
        oRequest.ClaimDetails.ClaimDetails.ClaimNumber = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClaimNumber
        oRequest.ClaimDetails.ClaimDetails.ClaimStatus = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClaimStatus
        oRequest.ClaimDetails.ClaimDetails.ClaimStatusDate = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClaimStatusDate
        oRequest.ClaimDetails.ClaimDetails.ClaimStatusId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClaimStatusId
        oRequest.ClaimDetails.ClaimDetails.ClaimVersion = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClaimVersion
        oRequest.ClaimDetails.ClaimDetails.ClaimVersionDescription = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClaimVersionDescription
        oRequest.ClaimDetails.ClaimDetails.Client = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client
        oRequest.ClaimDetails.ClaimDetails.ClientAddressId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClientAddressId
        oRequest.ClaimDetails.ClaimDetails.ClientAddressTypeId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClientAddressTypeId
        oRequest.ClaimDetails.ClaimDetails.ClientEmail = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClientEmail
        oRequest.ClaimDetails.ClaimDetails.ClientFaxNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClientFaxNo
        oRequest.ClaimDetails.ClaimDetails.ClientMobileNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClientMobileNo
        oRequest.ClaimDetails.ClaimDetails.ClientName = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClientName
        oRequest.ClaimDetails.ClaimDetails.ClientShortName = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClientShortName
        oRequest.ClaimDetails.ClaimDetails.ClientTelNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClientTelNo
        oRequest.ClaimDetails.ClaimDetails.ClientTelNoOff = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClientTelNoOff
        oRequest.ClaimDetails.ClaimDetails.Comments = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Comments
        oRequest.ClaimDetails.ClaimDetails.CurrencyCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.CurrencyCode
        oRequest.ClaimDetails.ClaimDetails.CurrencyId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.CurrencyId
        oRequest.ClaimDetails.ClaimDetails.Description = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Description
        oRequest.ClaimDetails.ClaimDetails.GisScreenCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.GisScreenCode
        oRequest.ClaimDetails.ClaimDetails.HandlerCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.HandlerCode
        oRequest.ClaimDetails.ClaimDetails.HandlerId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.HandlerId
        oRequest.ClaimDetails.ClaimDetails.InfoOnly = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InfoOnly
        oRequest.ClaimDetails.ClaimDetails.InsuranceFileKey = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsuranceFileKey
        oRequest.ClaimDetails.ClaimDetails.InsuranceFolderCnt = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsuranceFolderCnt
        oRequest.ClaimDetails.ClaimDetails.InsuranceHolderCnt = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsuranceHolderCnt
        oRequest.ClaimDetails.ClaimDetails.InsuranceRef = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsuranceRef
        oRequest.ClaimDetails.ClaimDetails.InsuredCnt = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsuredCnt
        oRequest.ClaimDetails.ClaimDetails.Insurer = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Insurer
        oRequest.ClaimDetails.ClaimDetails.InsurerAddressId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsurerAddressId
        oRequest.ClaimDetails.ClaimDetails.InsurerAddressTypeId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsurerAddressTypeId
        oRequest.ClaimDetails.ClaimDetails.InsurerEmail = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsurerEmail
        oRequest.ClaimDetails.ClaimDetails.InsurerFaxNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsurerFaxNo
        oRequest.ClaimDetails.ClaimDetails.InsurerName = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsurerName
        oRequest.ClaimDetails.ClaimDetails.InsurerShortName = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsurerShortName
        oRequest.ClaimDetails.ClaimDetails.InsurerTelNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InsurerTelNo
        oRequest.ClaimDetails.ClaimDetails.LastModifiedDate = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.LastModifiedDate
        oRequest.ClaimDetails.ClaimDetails.LikelyClaim = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.LikelyClaim
        oRequest.ClaimDetails.ClaimDetails.Location = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Location
        oRequest.ClaimDetails.ClaimDetails.LossFromDate = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.LossFromDate
        oRequest.ClaimDetails.ClaimDetails.LossToDate = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.LossToDate
        oRequest.ClaimDetails.ClaimDetails.LossToDateSpecified = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.LossToDateSpecified
        oRequest.ClaimDetails.ClaimDetails.PaymentSuppressPayments = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PaymentSuppressPayments
        oRequest.ClaimDetails.ClaimDetails.PaymentSuppressRecoveries = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PaymentSuppressRecoveries
        oRequest.ClaimDetails.ClaimDetails.PaymentSuppressReserves = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PaymentSuppressReserves
        oRequest.ClaimDetails.ClaimDetails.PolicyNumber = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PolicyNumber
        oRequest.ClaimDetails.ClaimDetails.PreviousCatastropheCodeid = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousCatastropheCodeid
        oRequest.ClaimDetails.ClaimDetails.PreviousClaimStatusCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousClaimStatusCode
        oRequest.ClaimDetails.ClaimDetails.PreviousClientClaimNumber = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousClientClaimNumber
        oRequest.ClaimDetails.ClaimDetails.PreviousClientEmail = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousClientEmail
        oRequest.ClaimDetails.ClaimDetails.PreviousClientFaxNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousClientFaxNo
        oRequest.ClaimDetails.ClaimDetails.PreviousClientMobileNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousClientMobileNo
        oRequest.ClaimDetails.ClaimDetails.PreviousClientTaxRegistered = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousClientTaxRegistered
        oRequest.ClaimDetails.ClaimDetails.PreviousClientTelNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousClientTelNo
        oRequest.ClaimDetails.ClaimDetails.PreviousClientTelNoOff = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousClientTelNoOff
        oRequest.ClaimDetails.ClaimDetails.PreviousComments = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousComments
        oRequest.ClaimDetails.ClaimDetails.PreviousInfoOnly = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousInfoOnly
        oRequest.ClaimDetails.ClaimDetails.PreviousInsurerClaimNumber = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousInsurerClaimNumber
        oRequest.ClaimDetails.ClaimDetails.PreviousInsurerContact = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousInsurerContact
        oRequest.ClaimDetails.ClaimDetails.PreviousInsurerEmail = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousInsurerEmail
        oRequest.ClaimDetails.ClaimDetails.PreviousInsurerFaxNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousInsurerFaxNo
        oRequest.ClaimDetails.ClaimDetails.PreviousInsurerTelNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousInsurerTelNo
        oRequest.ClaimDetails.ClaimDetails.PreviousLocation = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousLocation
        oRequest.ClaimDetails.ClaimDetails.PreviousLossFromDate = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousLossFromDate
        oRequest.ClaimDetails.ClaimDetails.PreviousLossToDate = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousLossToDate
        oRequest.ClaimDetails.ClaimDetails.PreviousSecondaryCauseId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousSecondaryCauseId
        oRequest.ClaimDetails.ClaimDetails.PreviousTownId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousTownId
        oRequest.ClaimDetails.ClaimDetails.PreviousUserDefFldAId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousUserDefFldAId
        oRequest.ClaimDetails.ClaimDetails.PreviousUserDefFldBId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousUserDefFldBId
        oRequest.ClaimDetails.ClaimDetails.PreviousUserDefFldCId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousUserDefFldCId
        oRequest.ClaimDetails.ClaimDetails.PreviousUserDefFldDId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousUserDefFldDId
        oRequest.ClaimDetails.ClaimDetails.PreviousUserDefFldEId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousUserDefFldEId
        oRequest.ClaimDetails.ClaimDetails.PreviousVatRegNo = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PreviousVatRegNo
        oRequest.ClaimDetails.ClaimDetails.PrimaryCauseCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PrimaryCauseCode
        oRequest.ClaimDetails.ClaimDetails.PrimaryCauseId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PrimaryCauseId
        oRequest.ClaimDetails.ClaimDetails.ProductCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ProductCode
        oRequest.ClaimDetails.ClaimDetails.ProductId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ProductId
        oRequest.ClaimDetails.ClaimDetails.ProgressStatusCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ProgressStatusCode
        oRequest.ClaimDetails.ClaimDetails.ProgressStatusId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ProgressStatusId
        oRequest.ClaimDetails.ClaimDetails.ReportedDate = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ReportedDate
        oRequest.ClaimDetails.ClaimDetails.ReserveOnly = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ReserveOnly
        oRequest.ClaimDetails.ClaimDetails.RiskKey = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.RiskKey
        oRequest.ClaimDetails.ClaimDetails.SamStagingClaimKey = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.SamStagingClaimKey
        oRequest.ClaimDetails.ClaimDetails.SecondaryCauseCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.SecondaryCauseCode
        oRequest.ClaimDetails.ClaimDetails.SecondaryCauseId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.SecondaryCauseId
        oRequest.ClaimDetails.ClaimDetails.SourceDescription = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.SourceDescription
        oRequest.ClaimDetails.ClaimDetails.SourceId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.SourceId
        oRequest.ClaimDetails.ClaimDetails.StatsFolderId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.StatsFolderId
        oRequest.ClaimDetails.ClaimDetails.TownCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.TownCode
        oRequest.ClaimDetails.ClaimDetails.TownId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.TownId
        oRequest.ClaimDetails.ClaimDetails.TransactionTypeDescription = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.TransactionTypeDescription
        oRequest.ClaimDetails.ClaimDetails.TransactionTypeId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.TransactionTypeId
        oRequest.ClaimDetails.ClaimDetails.UnderwritingYearCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UnderwritingYearCode
        oRequest.ClaimDetails.ClaimDetails.UnderwritingYearId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UnderwritingYearId
        oRequest.ClaimDetails.ClaimDetails.UserDefFldACode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldACode
        oRequest.ClaimDetails.ClaimDetails.UserDefFldAId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldAId
        oRequest.ClaimDetails.ClaimDetails.UserDefFldATableCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldATableCode
        oRequest.ClaimDetails.ClaimDetails.UserDefFldBCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldBCode
        oRequest.ClaimDetails.ClaimDetails.UserDefFldBId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldBId
        oRequest.ClaimDetails.ClaimDetails.UserDefFldBTableCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldBTableCode
        oRequest.ClaimDetails.ClaimDetails.UserDefFldCCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldCCode
        oRequest.ClaimDetails.ClaimDetails.UserDefFldCId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldCId
        oRequest.ClaimDetails.ClaimDetails.UserDefFldCTableCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldCTableCode
        oRequest.ClaimDetails.ClaimDetails.UserDefFldDCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldDCode
        oRequest.ClaimDetails.ClaimDetails.UserDefFldDId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldDId
        oRequest.ClaimDetails.ClaimDetails.UserDefFldDTableCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldDTableCode
        oRequest.ClaimDetails.ClaimDetails.UserDefFldECode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldECode
        oRequest.ClaimDetails.ClaimDetails.UserDefFldEId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldEId
        oRequest.ClaimDetails.ClaimDetails.UserDefFldETableCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.UserDefFldETableCode
        oRequest.ClaimDetails.ClaimDetails.VersionId = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.VersionId
        If Not oGetClaimDetailsResponse.ClaimDetails.ClaimPeril Is Nothing Then
            ReDim oRequest.ClaimDetails.ClaimPeril(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length - 1)
            For lCount As Int32 = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length - 1
                oRequest.ClaimDetails.ClaimPeril(lCount) = New BaseImplementationTypes.BaseGetClaimPerilDetailsType
                oRequest.ClaimDetails.ClaimPeril(lCount).BaseClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).BaseClaimPerilKey
                oRequest.ClaimDetails.ClaimPeril(lCount).ClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).ClaimPerilKey
                oRequest.ClaimDetails.ClaimPeril(lCount).Comments = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Comments
                oRequest.ClaimDetails.ClaimPeril(lCount).Description = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Description
                oRequest.ClaimDetails.ClaimPeril(lCount).GisScreenCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).GisScreenCode
                oRequest.ClaimDetails.ClaimPeril(lCount).RIBand = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).RIBand
                oRequest.ClaimDetails.ClaimPeril(lCount).SumInsured = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).SumInsured
                oRequest.ClaimDetails.ClaimPeril(lCount).TypeCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).TypeCode
                'fetch if required
                'oRequest.ClaimDetails.ClaimPeril(lCount).Recovery
                If Not oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve Is Nothing Then
                    ReDim oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve.Length - 1)
                    For lRSCount As Int32 = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve.Length - 1
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount) = New BaseImplementationTypes.BaseGetClaimReserveDetailsType
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).BaseReserveKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).BaseReserveKey
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).CanDelete = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).CanDelete
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).ClaimPerilId = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).ClaimPerilId
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).InitialReserve = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).InitialReserve
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).IsExcess = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).IsExcess
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).IsExpense = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).IsExpense
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).IsIndemnity = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).IsIndemnity
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).PaidAmount = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).PaidAmount
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).RevisedReserve = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).RevisedReserve
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).SumInsured = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).SumInsured
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).TypeCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).TypeCode
                        oRequest.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).TypeKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Reserve(lRSCount).TypeKey
                    Next

                End If
                If Not oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery Is Nothing Then
                    ReDim oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery.Length - 1)
                    For lRSCount As Int32 = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery.Length - 1
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount) = New BaseImplementationTypes.BaseGetClaimRecoveryDetailsType
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).BaseRecoveryKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).BaseRecoveryKey
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).CanDelete = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).CanDelete
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).ClaimPerilId = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).ClaimPerilId
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).InitialRecovery = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).InitialRecovery
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).ReceiptedAmount = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).ReceiptedAmount
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).ReceiptedTaxAmount = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).ReceiptedTaxAmount
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyCode
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyKey
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyKeySpecified = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyKeySpecified
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyTypeCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyTypeCode
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyTypeKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyTypeKey
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyTypeKeySpecified = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RecoveryPartyTypeKeySpecified
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RevisedRecovery = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).RevisedRecovery
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).TypeCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).TypeCode
                        oRequest.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).TypeKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(lCount).Recovery(lRSCount).TypeKey
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub MaintainClaim(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByVal oSFIBusiness As CoreSAMBusiness, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType)
        Dim oMaintainClaimRequest As New SAMForInsuranceV2ImplementationTypes.MaintainClaimRequestType
        Dim oMaintainClaimResponse As New BaseImplementationTypes.BaseClaimResponseType

        oMaintainClaimRequest.BranchCode = oRequest.BranchCode
        oMaintainClaimRequest.TimeStamp = oRequest.TimeStamp
        oMaintainClaimRequest.Claim = New BaseImplementationTypes.BaseClaimMaintainType

        oMaintainClaimRequest.Claim.BaseClaimKey = oRequest.ClaimDetails.ClaimDetails.BaseClaimKey
        oMaintainClaimRequest.Claim.CatastropheCode = oRequest.Claim.CatastropheCode
        oMaintainClaimRequest.Claim.ClaimStatus = oRequest.Claim.ClaimStatus
        oMaintainClaimRequest.Claim.ClaimStatusDate = DateTime.Now
        oMaintainClaimRequest.Claim.ClaimVersion = oRequest.ClaimDetails.ClaimDetails.ClaimVersion + 1
        oMaintainClaimRequest.Claim.ClaimVersionDescription = "Maintain Claim"

        oMaintainClaimRequest.Claim.ClientEmail = oRequest.ClaimDetails.ClaimDetails.ClientEmail
        oMaintainClaimRequest.Claim.ClientFaxNo = oRequest.ClaimDetails.ClaimDetails.ClientFaxNo
        oMaintainClaimRequest.Claim.ClientMobileNo = oRequest.ClaimDetails.ClaimDetails.ClientMobileNo
        oMaintainClaimRequest.Claim.ClientTelNo = oRequest.ClaimDetails.ClaimDetails.ClientTelNo
        oMaintainClaimRequest.Claim.ClientTelNoOff = oRequest.ClaimDetails.ClaimDetails.ClientTelNoOff
        oMaintainClaimRequest.Claim.Comments = oRequest.ClaimDetails.ClaimDetails.Comments
        oMaintainClaimRequest.Claim.Description = oRequest.ClaimDetails.ClaimDetails.Description
        oMaintainClaimRequest.Claim.HandlerCode = oRequest.Claim.HandlerCode
        oMaintainClaimRequest.Claim.IgnoreWarnings = False
        oMaintainClaimRequest.Claim.InfoOnly = oRequest.ClaimDetails.ClaimDetails.InfoOnly
        oMaintainClaimRequest.Claim.LastModifiedDate = DateTime.Now
        oMaintainClaimRequest.Claim.LikelyClaim = oRequest.Claim.LikelyClaim
        oMaintainClaimRequest.Claim.Location = oRequest.ClaimDetails.ClaimDetails.Location
        oMaintainClaimRequest.Claim.LossFromDate = oRequest.Claim.LossFromDate
        oMaintainClaimRequest.Claim.LossToDate = oRequest.Claim.LossToDate
        oMaintainClaimRequest.Claim.LossToDateSpecified = oRequest.Claim.LossToDateSpecified
        oMaintainClaimRequest.Claim.PrimaryCauseCode = oRequest.Claim.PrimaryCauseCode
        oMaintainClaimRequest.Claim.ProgressStatusCode = oRequest.Claim.ProgressStatusCode
        oMaintainClaimRequest.Claim.ReportedDate = oRequest.Claim.ReportedDate
        oMaintainClaimRequest.Claim.SecondaryCauseCode = oRequest.Claim.SecondaryCauseCode
        oMaintainClaimRequest.Claim.TownCode = oRequest.ClaimDetails.ClaimDetails.TownCode
        oMaintainClaimRequest.Claim.ReserveOnly = True

        If oMaintainClaimRequest.Claim.ProgressStatusCode.ToUpper() = "CLOSED" Then
            oMaintainClaimRequest.Claim.CloseClaimOnZeroReserveRecoveryBalance = oRequest.Claim.CloseClaimOnZeroReserveRecoveryBalance
        End If

        oMaintainClaimResponse = oSFIBusiness.MaintainClaim(oMaintainClaimRequest)
        CheckForErrors(oResponse, oMaintainClaimResponse.Errors, "MaintainClaim")

        ' Process the Warnings
        If oRequest.Claim.IgnoreWarnings = False And (oMaintainClaimResponse.Warnings Is Nothing) = False Then
            ReDim oResponse.Warnings(oMaintainClaimResponse.Warnings.Length - 1)
            For i As Integer = oMaintainClaimResponse.Warnings.GetLowerBound(0) To oMaintainClaimResponse.Warnings.GetUpperBound(0)
                oResponse.Warnings(i) = New BaseImplementationTypes.BaseClaimProcessResponseTypeWarnings
                oResponse.Warnings(i).Code = oMaintainClaimResponse.Warnings(i).Code
                oResponse.Warnings(i).Description = oMaintainClaimResponse.Warnings(i).Description
            Next
        End If
        If oMaintainClaimResponse.TimeStamp IsNot Nothing Then
            oRequest.TimeStamp = oMaintainClaimResponse.TimeStamp
        End If
        oRequest.ClaimKey = oMaintainClaimResponse.ClaimKey

    End Sub

    Public Sub CheckForErrors(ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType, ByRef oError() As BaseImplementationTypes.SAMError, ByVal sMethodName As String)
        If Not oError Is Nothing Then
            If oError.Length > 0 Then
                oResponse.Errors = oError
                Throw New Exception(sMethodName & " Failed during processing")
            End If
        End If
    End Sub

    Private Sub OpenClaim(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByVal oSFIBusiness As CoreSAMBusiness, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType)

        Const ACMethodName As String = "OpenClaim"

        Dim oOpenClaimRequest As New SAMForInsuranceV2ImplementationTypes.OpenClaimRequestType
        Dim oOpenClaimResponse As BaseImplementationTypes.BaseClaimResponseType

        Dim oFindInsuranceFileRequest As New BaseImplementationTypes.BaseFindInsuranceFileRequestType

        Dim STSError As New STSErrorPublisher

        If oRequest.Claim.InsuranceFileKey = 0 Then
            If oRequest.Claim.InsuranceRef = "" Then 'does not exist then throw a missing data error
                STSError.AddInvalidField("InsuranceFileKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "InsuranceFileKey"), "")
            End If
            oFindInsuranceFileRequest.InsuranceRef = oRequest.Claim.InsuranceRef
            oFindInsuranceFileRequest.SourceId = oRequest.SourceId
            oFindInsuranceFileRequest.SearchDate = oRequest.Claim.LossFromDate
            oSFIBusiness.FindInsuranceFileForClaimsVersion2(oFindInsuranceFileRequest)
        End If

        If oRequest.Claim.RiskKey = 0 Then

            Dim riskKey As Integer

            oSFIBusiness.GetRiskCntFromInsuranceFileCnt(oRequest.Claim.InsuranceFileKey, riskKey)

            If riskKey > 0 Then
                oRequest.Claim.RiskKey = riskKey
            Else
                STSError.AddInvalidField("RiskKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "RiskKey"), "")
                ' exit if there are any missing parameters
                If STSError.HasErrors Then
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
                    SAMErrorCollection.CheckForErrorsFromSTS(oResponse.STSError)
                    Exit Sub
                End If
            End If
        End If

        oOpenClaimRequest.BranchCode = oRequest.BranchCode
        oOpenClaimRequest.Claim = New BaseImplementationTypes.BaseClaimOpenType
        oOpenClaimRequest.Claim.CatastropheCode = oRequest.Claim.CatastropheCode
        oOpenClaimRequest.Claim.ClaimStatus = oRequest.Claim.ClaimStatus
        oOpenClaimRequest.Claim.ClaimStatusDate = DateTime.Now
        oOpenClaimRequest.Claim.ClaimVersion = 1
        oOpenClaimRequest.Claim.ClaimVersionDescription = "Open Claim"
        oOpenClaimRequest.Claim.ClientEmail = oRequest.Claim.ClientEmail
        oOpenClaimRequest.Claim.ClientFaxNo = oRequest.Claim.ClientFaxNo
        oOpenClaimRequest.Claim.ClientMobileNo = oRequest.Claim.ClientMobileNo
        oOpenClaimRequest.Claim.ClientTelNo = oRequest.Claim.ClientTelNo
        oOpenClaimRequest.Claim.ClientTelNoOff = ""
        oOpenClaimRequest.Claim.Comments = oRequest.Claim.Comments
        oOpenClaimRequest.Claim.CurrencyCode = oRequest.Claim.CurrencyCode
        oOpenClaimRequest.Claim.Description = oRequest.Claim.Description
        oOpenClaimRequest.Claim.HandlerCode = oRequest.Claim.HandlerCode
        oOpenClaimRequest.Claim.IgnoreWarnings = False
        oOpenClaimRequest.Claim.InfoOnly = False
        oOpenClaimRequest.Claim.InsuranceFileKey = oRequest.Claim.InsuranceFileKey
        oOpenClaimRequest.Claim.LastModifiedDate = DateTime.Now
        oOpenClaimRequest.Claim.LikelyClaim = oRequest.Claim.LikelyClaim
        oOpenClaimRequest.Claim.Location = ""
        oOpenClaimRequest.Claim.LossFromDate = oRequest.Claim.LossFromDate
        oOpenClaimRequest.Claim.LossToDate = oRequest.Claim.LossToDate
        oOpenClaimRequest.Claim.LossToDateSpecified = oRequest.Claim.LossToDateSpecified
        oOpenClaimRequest.Claim.PrimaryCauseCode = oRequest.Claim.PrimaryCauseCode
        oOpenClaimRequest.Claim.ProgressStatusCode = oRequest.Claim.ProgressStatusCode
        oOpenClaimRequest.Claim.ReportedDate = oRequest.Claim.ReportedDate
        oOpenClaimRequest.Claim.RiskKey = oRequest.Claim.RiskKey
        oOpenClaimRequest.Claim.SecondaryCauseCode = oRequest.Claim.SecondaryCauseCode
        oOpenClaimRequest.Claim.TownCode = ""
        oOpenClaimRequest.Claim.UnderwritingYearCode = oRequest.Claim.UnderwritingYearCode
        oOpenClaimRequest.Claim.ReserveOnly = True

        oOpenClaimResponse = oSFIBusiness.OpenClaim(oOpenClaimRequest)
        CheckForErrors(oResponse, oOpenClaimResponse.Errors, "OpenClaim")

        ' Process the Warnings
        If oRequest.Claim.IgnoreWarnings = False And (oOpenClaimResponse.Warnings Is Nothing) = False Then
            ReDim oResponse.Warnings(oOpenClaimResponse.Warnings.Length - 1)
            For i As Integer = oOpenClaimResponse.Warnings.GetLowerBound(0) To oOpenClaimResponse.Warnings.GetUpperBound(0)
                oResponse.Warnings(i) = New BaseImplementationTypes.BaseClaimProcessResponseTypeWarnings
                oResponse.Warnings(i).Code = oOpenClaimResponse.Warnings(i).Code
                oResponse.Warnings(i).Description = oOpenClaimResponse.Warnings(i).Description
            Next
        End If

        oRequest.BaseClaimKey = oOpenClaimResponse.BaseClaimKey
        oRequest.ClaimKey = oOpenClaimResponse.ClaimKey
        oRequest.ClaimNumber = oOpenClaimResponse.ClaimNumber
        If oOpenClaimResponse.TimeStamp IsNot Nothing Then
            oRequest.TimeStamp = oOpenClaimResponse.TimeStamp
        End If
        oRequest.ClaimVersion = oOpenClaimResponse.Version

    End Sub

    Private Sub AddClaimRisk(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByVal oSFIBusiness As CoreSAMBusiness, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType)
        Dim oAddClaimRiskRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimRiskRequestType
        Dim oAddClaimRiskResponse As New BaseImplementationTypes.BaseGetClaimRiskResponseType

        oAddClaimRiskRequest.BaseClaimKey = oRequest.BaseClaimKey
        oAddClaimRiskRequest.BranchCode = oRequest.BranchCode
        oAddClaimRiskRequest.Task = SAMConstants.SAMComponentAction.PMAdd
        oAddClaimRiskRequest.TimeStamp = oRequest.TimeStamp

        ' Call CoreSAMBusiness method
        oAddClaimRiskResponse = oSFIBusiness.GetClaimRisk(oAddClaimRiskRequest)
        ' Call CheckForErrors
        CheckForErrors(oResponse, oAddClaimRiskResponse.Errors, "AddClaimRisk")
        If oAddClaimRiskResponse.TimeStamp IsNot Nothing Then
            oRequest.TimeStamp = oAddClaimRiskResponse.TimeStamp
        End If
        oRequest.ClaimRiskXML = SAMFunc.TransformDatasetPBtoSAM(oAddClaimRiskResponse.XMLDataSet)

    End Sub

    Private Sub GetClaimRisk(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByVal oSFIBusiness As CoreSAMBusiness, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType)
        Dim oGetClaimRiskRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimRiskRequestType
        Dim oGetClaimRiskResponse As New BaseImplementationTypes.BaseGetClaimRiskResponseType

        oGetClaimRiskRequest.BaseClaimKey = oRequest.ClaimDetails.ClaimDetails.BaseClaimKey
        oGetClaimRiskRequest.BranchCode = oRequest.BranchCode
        oGetClaimRiskRequest.ClaimKey = oRequest.ClaimKey
        oGetClaimRiskRequest.TimeStamp = oRequest.TimeStamp

        oGetClaimRiskResponse = oSFIBusiness.GetClaimRisk(oGetClaimRiskRequest)
        'Call CoreSAMBusiness method
        'Call CheckForErrors
        CheckForErrors(oResponse, oGetClaimRiskResponse.Errors, "GetClaimRisk")

        oRequest.ClaimRiskXML = oGetClaimRiskResponse.XMLDataSet
        If oGetClaimRiskResponse.TimeStamp IsNot Nothing Then
            oRequest.TimeStamp = oGetClaimRiskResponse.TimeStamp
        End If
    End Sub

    Private Sub UpdateClaimReservesOrPayments(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByVal oSFIBusiness As CoreSAMBusiness, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType)

        Dim oUpdateClaimReservesOrPaymentsRequest As New SAMForInsuranceV2ImplementationTypes.UpdateClaimReservesOrPaymentsRequestType
        Dim oUpdateClaimReservesOrPaymentsResponse As New BaseImplementationTypes.BaseUpdateClaimReservesOrPaymentsResponseType

        oUpdateClaimReservesOrPaymentsRequest.BranchCode = oRequest.BranchCode
        oUpdateClaimReservesOrPaymentsRequest.ClaimKey = oRequest.ClaimKey

        If oRequest.BaseClaimKey = oRequest.ClaimKey Then
            oUpdateClaimReservesOrPaymentsRequest.ProcessType = 1 'Open Claim
        Else
            oUpdateClaimReservesOrPaymentsRequest.ProcessType = 2 'maintain
        End If

        oUpdateClaimReservesOrPaymentsRequest.TimeStamp = oRequest.TimeStamp

        If Not oRequest.Claim.ClaimPeril Is Nothing Then
            Dim PerilList As New List(Of BaseImplementationTypes.BaseClaimPerilType)

            For Each inPeril As BaseImplementationTypes.BaseClaimProcessPerilType In oRequest.Claim.ClaimPeril
                Dim SAMPeril As New BaseImplementationTypes.BaseClaimPerilType

                SAMPeril.Description = inPeril.Description
                SAMPeril.TypeCode = inPeril.TypeCode

                If Not inPeril.Recovery Is Nothing Then
                    Dim RecoveryList As New List(Of BaseImplementationTypes.BaseClaimPerilRecoveryType)
                    For Each inRecovery As BaseImplementationTypes.BaseClaimProcessPerilRecoveryType In inPeril.Recovery
                        Dim SAMRecovery As New BaseImplementationTypes.BaseClaimPerilRecoveryType

                        SAMRecovery.RevisionAmount = inRecovery.Amount
                        SAMRecovery.RecoveryPartyCode = inRecovery.RecoveryPartyCode
                        SAMRecovery.RecoveryPartyTypeCode = inRecovery.RecoveryPartyTypeCode
                        SAMRecovery.TypeCode = inRecovery.TypeCode

                        RecoveryList.Add(SAMRecovery)
                    Next

                    SAMPeril.Recovery = RecoveryList.ToArray
                End If

                If Not inPeril.Reserve Is Nothing Then
                    Dim ReserveList As New List(Of BaseImplementationTypes.BaseClaimPerilReserveType)
                    For Each inReserve As BaseImplementationTypes.BaseClaimProcessPerilReserveType In inPeril.Reserve
                        Dim SFIReserve As New BaseImplementationTypes.BaseClaimPerilReserveType

                        'The amount passed in is the full amount.  Work out the revision amount
                        Using con As SiriusConnection = SiriusConnection.FromAny(SAMFunc.ConnectionString)
                            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("Spu_CLM_Get_Reserve_Revised_Amount")
                                cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = oRequest.ClaimKey
                                cmd.AddInParameter("@peril_type_code", SqlDbType.VarChar, 30).Value = inPeril.TypeCode
                                cmd.AddInParameter("@reserve_type_code", SqlDbType.VarChar, 30).Value = inReserve.TypeCode

                                Dim dtReserve As DataTable = con.ExecuteDataTable(cmd)
                                If dtReserve IsNot Nothing AndAlso dtReserve.Rows.Count = 1 Then
                                    inReserve.Amount = inReserve.Amount - Cast.ToDecimal(dtReserve.Rows(0).Item("Revised_reserve"), 0D)
                                End If
                            End Using
                        End Using

                        SFIReserve.RevisionAmount = inReserve.Amount + inReserve.PaymentAmount
                        SFIReserve.TypeCode = inReserve.TypeCode

                        'Check for payments so we know to process later
                        If inReserve.PaymentAmount > 0 Then
                            oRequest.IsPaymentRequired = True
                        End If

                        ReserveList.Add(SFIReserve)
                    Next

                    SAMPeril.Reserve = ReserveList.ToArray()
                End If

                PerilList.Add(SAMPeril)
            Next

            oUpdateClaimReservesOrPaymentsRequest.ClaimPeril = PerilList.ToArray()

            oUpdateClaimReservesOrPaymentsResponse = oSFIBusiness.UpdateClaimReservesOrPayments(oUpdateClaimReservesOrPaymentsRequest)
            CheckForErrors(oResponse, oUpdateClaimReservesOrPaymentsResponse.Errors, "UpdateClaimReservesOrPayments")
            If oUpdateClaimReservesOrPaymentsRequest.ProcessType <> 1 Then
                oRequest.TimeStamp = oUpdateClaimReservesOrPaymentsResponse.TimeStamp
            End If
        End If

    End Sub

    Private Sub ProcessClaimRiskData(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType)
        Dim oSFIBusiness As New CoreSAMBusiness("sirius", oRequest.BranchCode)
        oSFIBusiness.ProcessClaimRiskData(oRequest.ClaimRiskXML, oRequest.Claim.ClaimBuilderDetail)
    End Sub

    Private Sub BindClaim(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByVal isPayClaim As Boolean, ByVal oSFIBusiness As CoreSAMBusiness, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType, Optional ByVal oPayClaimRequest As BaseImplementationTypes.BaseClaimPaymentRequestType = Nothing)
        Dim oBindClaimRequest As New SAMForInsuranceV2ImplementationTypes.BindClaimRequestType
        Dim oBindClaimResponse As New BaseImplementationTypes.BaseBindClaimResponseType

        oBindClaimRequest.BranchCode = oRequest.BranchCode
        oBindClaimRequest.ClaimKey = oRequest.ClaimKey
        oBindClaimRequest.TimeStamp = oRequest.TimeStamp
        oBindClaimRequest.ExternalHandler = oRequest.Claim.ExternalHandler

        If isPayClaim Then
            oBindClaimRequest.ProcessType = 3 ' Pay Claim
            oBindClaimRequest.ClaimPayment = New BaseImplementationTypes.BaseClaimPaymentType
            oBindClaimRequest.ClaimPayment.CurrencyCode = oRequest.Claim.CurrencyCode
            oBindClaimRequest.ClaimPayment.BaseClaimKey = oRequest.BaseClaimKey
            oBindClaimRequest.ClaimPayment.BaseClaimPerilKey = oPayClaimRequest.ClaimPayment.BaseClaimPerilKey
            oBindClaimRequest.ClaimPayment.CashList = oPayClaimRequest.ClaimPayment.CashList
            If UCase(Trim(oRequest.Claim.ProgressStatusCode)) = "CLOSED" Then
                oBindClaimRequest.ClaimPayment.CloseClaimOnZeroReserveRecoveryBalance = True
            End If
            ReDim oBindClaimRequest.ClaimPayment.ClaimPaymentItem(oPayClaimRequest.ClaimPayment.ClaimPaymentItem.Length - 1)
            For lCPICount As Int32 = 0 To oPayClaimRequest.ClaimPayment.ClaimPaymentItem.Length - 1
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount) = New BaseImplementationTypes.BaseClaimPaymentItemType
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).BaseReserveKey = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).BaseReserveKey
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ClaimPaymentItemId = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ClaimPaymentItemId
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).CurrencyAmount = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).CurrencyAmount
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).CurrencyTax = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).CurrencyTax
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).CurrentReserve = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).CurrentReserve
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ExcessAmount = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ExcessAmount
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).IsExcess = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).IsExcess
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).IsWHTTax = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).IsWHTTax
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).LCurrencyAmount = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).LCurrencyAmount
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).LCurrencyTax = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).LCurrencyTax
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).LCurrencyTaxWHT = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).LCurrencyTaxWHT
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).PaymentAdjustment = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).PaymentAdjustment

                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).PaymentAmount = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).PaymentAmount
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).RecoveryId = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).RecoveryId
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).RecoveryTypeId = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).RecoveryTypeId
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ReserveId = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ReserveId
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ReverseExcess = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ReverseExcess

                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).RevisionAmount = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).RevisionAmount
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxAmount = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxAmount
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxAmountWHT = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxAmountWHT
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxGroupCode = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxGroupCode
                oBindClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxGroupId = oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxGroupId
            Next
        Else
            If oRequest.ClaimVersion = 1 Then
                oBindClaimRequest.ProcessType = 1 ' Open Claim
            Else
                oBindClaimRequest.ProcessType = 2 ' Maintain Claim
            End If
        End If

        ' Call CoreSAMBusiness method
        oBindClaimResponse = oSFIBusiness.BindClaim(oBindClaimRequest)

        ' Call CheckForErrors
        CheckForErrors(oResponse, oBindClaimResponse.Errors, "BindClaim")

        If oBindClaimRequest.ProcessType <> 1 Then ' Open Claim
            oRequest.TimeStamp = oBindClaimResponse.TimeStamp
        End If

    End Sub

    Private Sub PayClaim(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByVal oSFIBusiness As CoreSAMBusiness, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType)

        Dim lCount As Int32
        Dim lPCount As Int32

        For Each inPeril As BaseImplementationTypes.BaseClaimProcessPerilType In oRequest.Claim.ClaimPeril
            Dim oPayClaimRequest As New SAMForInsuranceV2ImplementationTypes.PayClaimRequestType
            Dim oPayClaimResponse As New SAMForInsuranceV2ImplementationTypes.PayClaimResponseType

            'Payment is per-reserve
            If Not inPeril.Reserve Is Nothing Then
                For Each inReserve As BaseImplementationTypes.BaseClaimProcessPerilReserveType In inPeril.Reserve
                    Dim oCashlist As BaseImplementationTypes.BasePaymentCashListType
                    Dim oPaymentList As New List(Of BaseImplementationTypes.BaseClaimPaymentItemType)
                    oCashlist = New BaseImplementationTypes.BasePaymentCashListType
                    lPCount = 0

                    If inReserve.PaymentAmount > 0 Then
                        Dim inPayment As New BaseImplementationTypes.BaseClaimPaymentItemType

                        inPayment.BaseReserveKey = GetBaseReserveKey(inPeril.TypeCode, inReserve.TypeCode, oRequest.ClaimDetails)
                        inPayment.PaymentAmount = inReserve.PaymentAmount
                        inPayment.ReverseExcess = False
                        inPayment.TaxGroupCode = inReserve.TaxGroupCode

                        oPaymentList.Add(inPayment)

                        oCashlist.BranchCode = oRequest.BranchCode
                        oCashlist.BankAccountCode = inReserve.PaymentDetails.PaymentBankCode

                        oCashlist.CurrencyCode = oRequest.Claim.CurrencyCode
                        oCashlist.ListDate = DateTime.Now
                        ' Length of reference field is 25 characters.
                        oCashlist.Reference = Right("CLP " & oRequest.ClaimDetails.ClaimDetails.ClaimNumber, 25)
                        oCashlist.StatusCode = "E"
                        oCashlist.TypeCode = "CP"
                        ReDim Preserve oCashlist.PaymentItem(lPCount)
                        oCashlist.PaymentItem(lPCount) = New BaseImplementationTypes.BasePaymentCashListItemType

                        With oCashlist.PaymentItem(lPCount)
                            '.AccountKey
                            .AccountShortCode = kAccountShortnameCLMPAYABLE
                            '.AllocationDetailKey
                            '.AllocationDetails
                            .AllocationStatusCode = ""
                            '.AllocationStatusKey
                            .Amount = inReserve.PaymentAmount
                            .Bank = New BaseImplementationTypes.BaseBankPaymentType
                            .Bank.AccountCode = inReserve.PaymentDetails.PaymentBankCode
                            .Bank.PayeeName = inReserve.PaymentDetails.PaymentPayee
                            '.BankKey
                            '.BankReference
                            '.CashListItemKey
                            '.ContactAddress
                            '.ContactName
                            '.CreditCard
                            '.FurtherDetails
                            '.IsProduceDocument
                            '.isValidated
                            .MediaReference = inReserve.PaymentDetails.PaymentMediaReference
                            .MediaTypeCode = inReserve.PaymentDetails.PaymentMediaTypeCode
                            '.MediaTypeKey
                            '.OurReference
                            '.PartyKey
                            '.StatusCode
                            '.StatusKey
                            '.TheirReference
                            .TransactionDate = DateTime.Now
                            '.TransDetailKey
                            .TypeCode = "CLP"
                            '.TypeKey
                            '.UserId
                            '.UserName
                            'TODO: Complete additional CLI items
                        End With
                        lPCount = lPCount + 1
                    End If

                    oPayClaimRequest.BranchCode = oRequest.BranchCode
                    oPayClaimRequest.TimeStamp = oRequest.TimeStamp
                    oPayClaimRequest.ClaimPayment = New BaseImplementationTypes.BaseClaimPaymentType

                    'If Peril has a payment
                    If oPaymentList.Count > 0 Then
                        ReDim oPayClaimRequest.ClaimPayment.ClaimPaymentItem(oPaymentList.Count - 1)
                        'For Each oPaymentItem As BaseImplementationTypes.BaseClaimPaymentItemType In oPaymentList
                        For lCPICount As Int32 = 0 To oPaymentList.Count - 1
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount) = New BaseImplementationTypes.BaseClaimPaymentItemType
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).BaseReserveKey = oPaymentList(lCPICount).BaseReserveKey
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ClaimPaymentItemId = oPaymentList(lCPICount).ClaimPaymentItemId
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).CurrencyAmount = oPaymentList(lCPICount).CurrencyAmount
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).CurrencyTax = oPaymentList(lCPICount).CurrencyTax
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).CurrentReserve = oPaymentList(lCPICount).CurrentReserve
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ExcessAmount = oPaymentList(lCPICount).ExcessAmount
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).IsExcess = oPaymentList(lCPICount).IsExcess
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).IsWHTTax = oPaymentList(lCPICount).IsWHTTax
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).LCurrencyAmount = oPaymentList(lCPICount).LCurrencyAmount
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).LCurrencyTax = oPaymentList(lCPICount).LCurrencyTax
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).LCurrencyTaxWHT = oPaymentList(lCPICount).LCurrencyTaxWHT
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).PaymentAdjustment = oPaymentList(lCPICount).PaymentAdjustment

                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).PaymentAmount = oPaymentList(lCPICount).PaymentAmount
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).RecoveryId = oPaymentList(lCPICount).RecoveryId
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).RecoveryTypeId = oPaymentList(lCPICount).RecoveryTypeId
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ReserveId = oPaymentList(lCPICount).ReserveId
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).ReverseExcess = oPaymentList(lCPICount).ReverseExcess

                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).RevisionAmount = oPaymentList(lCPICount).RevisionAmount
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxAmount = oPaymentList(lCPICount).TaxAmount
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxAmountWHT = oPaymentList(lCPICount).TaxAmountWHT
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxGroupCode = oPaymentList(lCPICount).TaxGroupCode
                            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(lCPICount).TaxGroupId = oPaymentList(lCPICount).TaxGroupId
                        Next

                        oPayClaimRequest.ClaimPayment.BaseClaimKey = oRequest.BaseClaimKey
                        oPayClaimRequest.ClaimPayment.ClaimVersionDescription = "Payment of Claim"
                        'oPayClaimRequest.ClaimPayment.ClientKey = oRequest.ClaimDetails.ClaimDetails.Client.PartyKe
                        oPayClaimRequest.ClaimPayment.CloseClaimOnZeroReserveRecoveryBalance = oRequest.Claim.CloseClaimOnZeroReserveRecoveryBalance
                        oPayClaimRequest.ClaimPayment.CurrencyCode = oRequest.Claim.CurrencyCode
                        oPayClaimRequest.ClaimPayment.PaymentOnly = True
                        oPayClaimRequest.ClaimPayment.PaymentPartyType = BaseImplementationTypes.ClaimPaymentPartyTypeType.CLMPAYABLE
                        oPayClaimRequest.ClaimPayment.BaseClaimPerilKey = oRequest.ClaimDetails.ClaimPeril(lCount).BaseClaimPerilKey
                        oPayClaimRequest.ClaimPayment.TransactionDate = Date.Now
                        oPayClaimRequest.ClaimPayment.DateOfPayment = oPayClaimRequest.ClaimPayment.TransactionDate
                        oPayClaimRequest.ClaimPayment.CashList = oCashlist

                        ' Call CoreSAMBusiness method
                        oPayClaimResponse = CType(oSFIBusiness.PayClaim(oPayClaimRequest), SAMForInsuranceV2ImplementationTypes.PayClaimResponseType)
                        If oPayClaimResponse.TimeStamp IsNot Nothing Then
                            oRequest.TimeStamp = oPayClaimResponse.TimeStamp
                        End If
                        CheckForErrors(oResponse, oPayClaimResponse.Errors, "PayClaim")
                        oRequest.ClaimKey = oPayClaimResponse.ClaimKey
                        'For each payment
                        BindClaim(oRequest, True, oSFIBusiness, oResponse, oPayClaimRequest)
                    End If

                Next
            End If
            lCount = lCount + 1
        Next
        Dim oSAMErrorCollection As New SAMErrorCollection
        Using con As SiriusConnection = SiriusConnection.FromAny(SAMFunc.ConnectionString)
            If oSFIBusiness.GetAndValidateSpecifiedTableCode(con, "Progress_status", "is_closed_check_status", "Code", Trim(oRequest.Claim.ProgressStatusCode), oSAMErrorCollection, "Code") = 1 And _
                oSFIBusiness.GetAndValidateSpecifiedTableCode(con, "Progress_status", "is_deleted", "Code", Trim(oRequest.Claim.ProgressStatusCode), oSAMErrorCollection, "Code") = 0 Then
            End If
        End Using
    End Sub

    Private Sub ClaimReceipt(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByVal oSFIBusiness As CoreSAMBusiness, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType)

        Dim lCount As Int32
        Dim lPCount As Int32 = -1
        If Not oRequest.Claim.ClaimPeril Is Nothing Then
            For Each inPeril As BaseImplementationTypes.BaseClaimProcessPerilType In oRequest.Claim.ClaimPeril
                lPCount = lPCount + 1
                Dim oClaimReceiptRequest As New SAMForInsuranceV2ImplementationTypes.ClaimReceiptRequestType
                Dim oClaimReceiptResponse As New SAMForInsuranceV2ImplementationTypes.ClaimReceiptResponseType

                oClaimReceiptRequest.BranchCode = oRequest.BranchCode
                oClaimReceiptRequest.TimeStamp = oRequest.TimeStamp

                oClaimReceiptRequest.ClaimReceipt = New BaseImplementationTypes.BaseClaimReceiptType
                oClaimReceiptRequest.ClaimReceipt.BaseClaimKey = oRequest.BaseClaimKey
                If IsArray(oRequest.ClaimDetails.ClaimPeril) AndAlso ((oRequest.ClaimDetails.ClaimPeril(lPCount) Is Nothing) = False) Then
                    oClaimReceiptRequest.ClaimReceipt.BaseClaimPerilKey = oRequest.ClaimDetails.ClaimPeril(lPCount).BaseClaimPerilKey
                End If
                oClaimReceiptRequest.ClaimReceipt.CurrencyCode = oRequest.Claim.CurrencyCode
                oClaimReceiptRequest.ClaimReceipt.PartyKey = oRequest.ClaimDetails.ClaimDetails.InsuranceHolderCnt
                oClaimReceiptRequest.ClaimReceipt.ReceiptPartyType = BaseImplementationTypes.ClaimReceiptPartyTypeType.CLMRECEIVABLE

                'Receipt is per-reserve
                If (Not oRequest.ClaimDetails.ClaimPeril(lPCount).Recovery Is Nothing) AndAlso (Not inPeril.Recovery Is Nothing) Then
                    Dim oReceiptList() As BaseImplementationTypes.BaseClaimReceiptItemType
                    ReDim oReceiptList(inPeril.Recovery.Length - 1)
                    lCount = -1
                    For Each inRecovery As BaseImplementationTypes.BaseClaimProcessPerilRecoveryType In inPeril.Recovery

                        Dim inReceipt As New BaseImplementationTypes.BaseClaimReceiptItemType

                        lCount = lCount + 1

                        inReceipt.BaseRecoveryKey = GetBaseRecoveryKey(inPeril.TypeCode, inRecovery.TypeCode, oRequest.ClaimDetails)
                        inReceipt.ReceiptAmount = inRecovery.Amount

                        oReceiptList(lCount) = inReceipt
                    Next

                    oClaimReceiptRequest.ClaimReceipt.ReceiptItem = oReceiptList
                    oClaimReceiptRequest.ClaimReceipt.IsGetClaimReceiptTaxesType = False

                    ' Call CoreSAMBusiness method
                    oClaimReceiptResponse = CType(oSFIBusiness.ClaimReceipt(oClaimReceiptRequest), SAMForInsuranceV2ImplementationTypes.ClaimReceiptResponseType)
                    If oClaimReceiptResponse.TimeStamp IsNot Nothing Then
                        oRequest.TimeStamp = oClaimReceiptResponse.TimeStamp
                    End If
                    oRequest.ClaimKey = oClaimReceiptResponse.ClaimKey
                    CheckForErrors(oResponse, oClaimReceiptResponse.Errors, "ClaimReceipt")
                    'For each Receipt
                    BindClaim(oRequest, False, oSFIBusiness, oResponse)
                End If
                lCount = lCount + 1
            Next
        End If

    End Sub

    Private Sub UpdateClaimRisk(ByRef oRequest As BaseImplementationTypes.BaseClaimProcessRequestType, ByVal oSFIBusiness As CoreSAMBusiness, ByRef oResponse As BaseImplementationTypes.BaseClaimProcessResponseType)

        Dim oUpdateClaimRiskRequest As New SAMForInsuranceV2ImplementationTypes.UpdateClaimRiskRequestType
        Dim oUpdateClaimRiskResponse As New BaseImplementationTypes.BaseUpdateClaimRiskResponseType

        oUpdateClaimRiskRequest.BaseClaimKey = oRequest.BaseClaimKey
        oUpdateClaimRiskRequest.BranchCode = oRequest.BranchCode
        oUpdateClaimRiskRequest.TimeStamp = oRequest.TimeStamp
        oUpdateClaimRiskRequest.XMLDataSet = oRequest.ClaimRiskXML

        'TODO - Call CoreSAMBusiness method
        oUpdateClaimRiskResponse = oSFIBusiness.UpdateClaimRisk(oUpdateClaimRiskRequest)
        'TODO - Call CheckForErrors
        CheckForErrors(oResponse, oUpdateClaimRiskResponse.Errors, "UpdateClaimRisk")
        If oUpdateClaimRiskResponse.TimeStamp IsNot Nothing Then
            oRequest.TimeStamp = oUpdateClaimRiskResponse.TimeStamp
        End If
    End Sub

    Private Function GetBaseReserveKey(ByVal sPerilCode As String, ByVal sReserveCode As String, ByVal oClaim As BaseImplementationTypes.BaseGetClaimDetailsType) As Integer

        If Not oClaim.ClaimPeril Is Nothing Then
            For Each inPeril As BaseImplementationTypes.BaseGetClaimPerilDetailsType In oClaim.ClaimPeril
                If inPeril.TypeCode.Trim.ToUpper = sPerilCode.Trim.ToUpper Then
                    If Not inPeril.Reserve Is Nothing Then
                        For Each inReserve As BaseImplementationTypes.BaseGetClaimReserveDetailsType In inPeril.Reserve
                            If inReserve.TypeCode.Trim.ToUpper = sReserveCode.Trim.ToUpper Then
                                Return inReserve.BaseReserveKey
                            End If
                        Next
                    End If
                End If
            Next
        End If
    End Function

    Private Function GetBaseRecoveryKey(ByVal sPerilCode As String, ByVal sRecoveryCode As String, ByVal oClaim As BaseImplementationTypes.BaseGetClaimDetailsType) As Integer

        If Not oClaim.ClaimPeril Is Nothing Then
            For Each inPeril As BaseImplementationTypes.BaseGetClaimPerilDetailsType In oClaim.ClaimPeril
                If inPeril.TypeCode.Trim.ToUpper = sPerilCode.Trim.ToUpper Then
                    If Not inPeril.Recovery Is Nothing Then
                        For Each inRecovery As BaseImplementationTypes.BaseGetClaimRecoveryDetailsType In inPeril.Recovery
                            If inRecovery.TypeCode.Trim.ToUpper = sRecoveryCode.Trim.ToUpper Then
                                Return inRecovery.BaseRecoveryKey
                            End If
                        Next
                    End If
                End If
            Next
        End If
    End Function

    Public Function GenerateResponse(ByVal oRequest As BaseImplementationTypes.BaseClaimProcessRequestType) As BaseImplementationTypes.BaseClaimProcessResponseType
        Dim oResponse As New BaseImplementationTypes.BaseClaimProcessResponseType
        With oResponse
            .BaseClaimKey = oRequest.BaseClaimKey
            .ClaimKey = oRequest.ClaimKey
            .ClaimNumber = oRequest.ClaimNumber
            '.Errors = oRequest.
            '.HandlingInstanceID = oRequest.
            '.ResultingStatus = oRequest.
            '.STSError = oRequest.
            .TimeStamp = oRequest.TimeStamp
            .Version = oRequest.ClaimVersion
            '.Warnings = oRequest.warnings
        End With
        Return oResponse
    End Function


#Region " ProcessClaim"
    Public Sub ProcessClaimRiskData(ByRef sRiskXML As String, ByVal oAmendments() As BaseImplementationTypes.BaseProductBuilderRiskType)
        Dim xDoc As New XmlDocument()

        'Load XML
        xDoc.LoadXml(sRiskXML)

        'Manipluate XML
        Dim xNode As XmlNode
        ' For each ClaimBuilder data item
        For Each oAmend As BaseImplementationTypes.BaseProductBuilderRiskType In oAmendments
            ' Get the XPath value
            Dim xpath As String = oAmend.ProductBuilderData.ItemName
            ' Select the node using the XPath
            xNode = xDoc.SelectSingleNode(xpath)
            ' If the node has been found then
            If Not xNode Is Nothing Then
                ' Set the value of the attribute to the value passed in
                xNode.InnerText = oAmend.ProductBuilderData.Value
                ' Set the UpdateStatus of the Object
                Dim attr As XmlAttribute = CType(xNode, XmlAttribute)
                If (attr.OwnerElement.Attributes("US") Is Nothing) = False Then
                    If attr.OwnerElement.Attributes("US").Value = "0" Then
                        attr.OwnerElement.Attributes("US").Value = "2"
                    End If
                End If
                ' Else no attribute found so create one
            Else
                Dim objectName As String = String.Empty
                ' Split the Object and property name of the Xpath
                Dim propertyName As String = String.Empty
                Dim split As String() = xpath.Split(New [Char]() {"@"c})
                If split.Length = 2 Then
                    objectName = IIf(split(0).EndsWith("/"), split(0).Substring(0, split(0).Length - 1), split(0)).ToString
                    propertyName = split(1)
                    ' Find the Element/Object
                    xNode = xDoc.SelectSingleNode(objectName)
                    If Not xNode Is Nothing Then
                        ' Create the attribute and set the value
                        Dim attr As XmlNode = xDoc.CreateNode(XmlNodeType.Attribute, propertyName, "")
                        attr.Value = oAmend.ProductBuilderData.Value
                        xNode.Attributes.SetNamedItem(attr)
                        ' Set the UpdateStatus of the Object
                        If (xNode.Attributes("US") Is Nothing) = False Then
                            If xNode.Attributes("US").Value = "0" Then
                                xNode.Attributes("US").Value = "2"
                            End If
                        End If
                    End If
                End If
            End If
        Next

        'Update the return string
        sRiskXML = xDoc.OuterXml

    End Sub
#End Region

    Private Function CheckForPartyRecord(ByVal PolicyProcessRequest As PolicyProcessRequestType, ByVal oBusiness As CoreSAMBusiness) As PolicyProcessResponseType

        Dim response As New PolicyProcessResponseType

        ' Check for Party record

        If PolicyProcessRequest.Party.GetType Is GetType(BasePartyPCType) And UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "POLICY" Then

            ' Process personal client
            Dim oBasePartyPC As BasePartyPCType = DirectCast(PolicyProcessRequest.Party, BasePartyPCType)

            Dim ds As New DataSet

            Using con As SiriusConnection = SiriusConnection.FromAny(SAMFunc.ConnectionString)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Check_Personal_Client")
                    cmd.AddInParameter("@Surname", SqlDbType.VarChar, 255).Value = Convert.ToString(oBasePartyPC.Surname)
                    cmd.AddInParameter("@ResolvedName", SqlDbType.VarChar, 387).Value = Convert.ToString(oBasePartyPC.Title) + " " + Convert.ToString(oBasePartyPC.Forename) + " " + Convert.ToString(oBasePartyPC.Surname)
                    cmd.AddInParameter("@AddressLine1", SqlDbType.VarChar, 60).Value = Convert.ToString(PolicyProcessRequest.Party.Addresses(0).AddressLine1)
                    Dim ret As Integer = con.ExecuteDataSet(cmd, ds, "Row")
                End Using
            End Using
            response.Insured = New BaseAddPartyResponseType
            If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows IsNot Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                If ds.Tables(0).Rows.Count > 1 Then
                    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PartyRecordNotFound, "Party cannot be identified", "More than one party record exists for Party name - '" & Convert.ToString(oBasePartyPC.Surname) & "' at address - '" & Convert.ToString(PolicyProcessRequest.Party.Addresses(0).AddressLine1) & "'")
                    STSErrorEx.SetContext(response.STSError, HttpContext.Current.Request.Url.ToString(), "CheckForPartyRecord", "Initialise.Business", True)
                    Return response
                Else
                    With ds.Tables(0).Rows(0)

                        response.Insured.Shortname = Convert.ToString(.Item("resolved_name"))
                        response.Insured.PartyKey = Convert.ToInt32(.Item("party_cnt").ToString)

                        PolicyProcessRequest.Policy.InsuredName = Convert.ToString(.Item("resolved_name"))
                        PolicyProcessRequest.Policy.PartyKey = Convert.ToInt32(.Item("party_cnt").ToString)

                    End With
                End If
            End If
            Return response
        Else
            Dim findPartyRequest As New SAMForInsuranceV2ImplementationTypes.FindPartyRequestType
            Dim findPartyResponse As BaseFindPartyResponseType

            findPartyRequest.BranchCode = PolicyProcessRequest.Party.BranchCode
            findPartyRequest.Status = "0"

            Dim oBasePartyCC As BasePartyCCType
            Dim oBasePartyPC As BasePartyPCType

            ' Arch BDX ----------------------------------------- Start
            If PolicyProcessRequest.Party.GetType Is GetType(BasePartyPCType) Then
                If UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "POLICY" Then
                    ' Process personal client
                    oBasePartyPC = DirectCast(PolicyProcessRequest.Party, BasePartyPCType)
                    findPartyRequest.Name = oBasePartyPC.Surname
                    findPartyRequest.PartyType = "PC"
                    findPartyRequest.PartyTypeSpecified = True
                ElseIf UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "MTA PERM" Or _
                            UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "MTACAN" Or _
                          UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "MTR" Or _
                            UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "RENEWAL" Then
                    ' Map Insurance Ref in case of MTA and MTC to search for client record
                    findPartyRequest.PolicyRef = PolicyProcessRequest.Policy.QuoteRef
                    findPartyRequest.PartyType = "BDXIgnorePartyType"
                    findPartyRequest.PartyTypeSpecified = True
                End If

            Else
                ' Process corporate client
                oBasePartyCC = DirectCast(PolicyProcessRequest.Party, BasePartyCCType)
                If UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "POLICY" Then
                    findPartyRequest.Name = oBasePartyCC.CompanyName
                    findPartyRequest.PartyType = "CC"
                    findPartyRequest.PartyTypeSpecified = True
                ElseIf UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "MTA PERM" Or _
                            UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "MTACAN" Or _
                          UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "MTR" Or _
                            UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "RENEWAL" Then
                    ' Map Insurance Ref in case of MTA and MTC to search for client record
                    findPartyRequest.PolicyRef = PolicyProcessRequest.Policy.QuoteRef
                    findPartyRequest.PartyType = "BDXIgnorePartyType"
                    findPartyRequest.PartyTypeSpecified = True
                End If

                ' Ensure there is a correspondence address
            End If

            Dim bFound As Boolean = False
            If UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "POLICY" Then
                For iCnt As Integer = 0 To PolicyProcessRequest.Party.Addresses.GetUpperBound(0)
                    If PolicyProcessRequest.Party.Addresses(iCnt).AddressTypeCode = AddressTypeType.Correspondence Then
                        findPartyRequest.AddressLine1 = PolicyProcessRequest.Party.Addresses(iCnt).AddressLine1
                        findPartyRequest.AddressLine2 = PolicyProcessRequest.Party.Addresses(iCnt).AddressLine2
                        findPartyRequest.PostCode = PolicyProcessRequest.Party.Addresses(iCnt).PostCode
                        Exit For
                    End If
                Next
            End If


            findPartyResponse = oBusiness.FindParty(findPartyRequest)

            If findPartyResponse.ResultDataset IsNot Nothing Then

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseFindPartyResponseTypeParties), oXmlAttributes)
                oXmlOverride.Add(GetType(SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseFindPartyResponseTypePartiesRow), oXmlAttributes)

                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseFindPartyResponseTypeParties = Nothing
                Dim oResultDataSetObject As Object = Nothing
                Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseFindPartyResponseTypeParties), m_sDefaultNameSpace)
                If findPartyResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=findPartyResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseFindPartyResponseTypeParties)
                End If
                ' Retrieve the values from the implementation response structure
                If oResultDataSet.Row.GetUpperBound(0) > 0 Then

                    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PartyRecordNotFound, "Party cannot be identified", "More than one party record exists for Party name - '" & findPartyRequest.Name & "' at address - '" & findPartyRequest.AddressLine1 & "'")
                    STSErrorEx.SetContext(response.STSError, HttpContext.Current.Request.Url.ToString(), "CheckForPartyRecord", "Initialise.Business", True)
                    Return response

                ElseIf oResultDataSet.Row.GetUpperBound(0) = 0 Then

                    response.Insured = New BaseAddPartyResponseType

                    response.Insured.Shortname = oResultDataSet.Row(0).ResolvedName
                    response.Insured.PartyKey = oResultDataSet.Row(0).PartyKey

                    PolicyProcessRequest.Policy.InsuredName = oResultDataSet.Row(0).ResolvedName
                    PolicyProcessRequest.Policy.PartyKey = oResultDataSet.Row(0).PartyKey
                End If

            End If
            Return response
        End If

    End Function
    ' ***************************************************************** '
    ' Name: NewBusiness
    '
    ' Description: This method is the implementation of the NewBusiness 
    '              Web Method on the Messaging service
    '
    ' ***************************************************************** '
    Public Function PolicyProcess(ByVal PolicyProcessRequest As PolicyProcessRequestType) As PolicyProcessResponseType

        Const ACMethodName As String = "PolicyProcess"

        ' Declare the Response object
        Dim oResponse As New PolicyProcessResponseType

        ' Declare the Core SAM business object
        Dim oBusiness As New CoreSAMBusiness
        Dim agentKey As Integer = 0
        Dim STSError As New STSErrorPublisher

        ' Check if the branch code was passed in
        If SAMFunc.NothingToString(PolicyProcessRequest.BranchCode) = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        ' Check if the client structure was passed in
        If (PolicyProcessRequest.Party Is Nothing) = True Then
            STSError.AddInvalidField("Client", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Client"), "")
        End If

        ' Check if the policy structure was passed in
        If (PolicyProcessRequest.Policy Is Nothing) = True Then
            STSError.AddInvalidField("Policy", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Policy"), "")
        End If

        If PolicyProcessRequest.AgentCode <> String.Empty Then
            Dim samError As New SAMErrorCollection
            agentKey = oBusiness.GetAndValidateSpecifiedTableCode("party", "party_cnt", "shortname", PolicyProcessRequest.AgentCode, samError, "AgentCode")
            ' Check if Agent Code passed in relates to a record in Backoffice
            If (agentKey = 0) Then
                STSError.AddInvalidField("AgentCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(MandatoryInputInvalid, "AgentCode"), "")
            End If

        End If

        ' Check if the policy structure was passed in
        If (PolicyProcessRequest.Policy Is Nothing) = True Then
            STSError.AddInvalidField("Policy", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Policy"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        'Check if the Party record already exists
        oResponse = CheckForPartyRecord(PolicyProcessRequest, oBusiness)

        If oResponse.STSError IsNot Nothing Then
            Return oResponse
        End If

        ' If no matching client record has been found then create a new one
        If PolicyProcessRequest.Policy.PartyKey = 0 Then ' This is only possible in case of NB and REN

            ' Add the Party record by calling the implementation method 
            Dim oAddPartyRequest As New BaseImplementationTypes.BaseAddPartyRequestType
            oAddPartyRequest.BranchCode = PolicyProcessRequest.BranchCode
            oAddPartyRequest.AgentKey = agentKey
            oAddPartyRequest.Party = PolicyProcessRequest.Party
            oResponse.Insured = oBusiness.AddParty( _
                        AddPartyRequest:=oAddPartyRequest)

            ' Check response for errors
            If (oResponse.Insured.STSError Is Nothing) = False Then
                oResponse.STSError = oResponse.Insured.STSError
                Return oResponse
            End If

            ' Set the Party ID on the Policy request structure 
            PolicyProcessRequest.Policy.PartyKey = oResponse.Insured.PartyKey

            ' Determine the Party Type
            If PolicyProcessRequest.Party.GetType Is GetType(BasePartyPCType) Then
                ' Process personal client
                Dim oBasePartyPC As BasePartyPCType = DirectCast(PolicyProcessRequest.Party, BasePartyPCType)
                If PolicyProcessRequest.Policy.InsuredName = "" Then
                    PolicyProcessRequest.Policy.InsuredName = oBasePartyPC.Title & " " & oBasePartyPC.Initials & " " & oBasePartyPC.Surname
                End If
            Else
                ' Process corporate client
                Dim oBasePartyCC As BasePartyCCType = DirectCast(PolicyProcessRequest.Party, BasePartyCCType)
                If PolicyProcessRequest.Policy.InsuredName = "" Then
                    PolicyProcessRequest.Policy.InsuredName = oBasePartyCC.CompanyName
                End If
            End If

            ' Arch BDX ----------------------------------------- Start - Update Party Record
        Else
            ' Arch BDX ----------------------------------------- Start - Validate whether data maches with what is supplied with BDX file
            ' Added a new method for updating a party record as for now requirement is only to update few records on Party Table 
            ' and it is not suggestable to make a call to existing Update party method due to the size and performance of a call
            If PolicyProcessRequest.UpdateParty Then
                If PolicyProcessRequest.Party.GetType Is GetType(BasePartyCCType) Then
                    Dim oUpdatePartyRequest As New BaseImplementationTypes.BasePartyCCType
                    oUpdatePartyRequest.BranchCode = PolicyProcessRequest.BranchCode
                    oUpdatePartyRequest = CType(PolicyProcessRequest.Party, BasePartyCCType)

                    oBusiness.UpdatePartyMainDetails(oUpdatePartyRequest, PolicyProcessRequest.Policy.PartyKey)
                End If
                ' Arch BDX ----------------------------------------- End


                For iAddressCnt As Integer = PolicyProcessRequest.Party.Addresses.GetLowerBound(0) To PolicyProcessRequest.Party.Addresses.GetUpperBound(0)
                    If PolicyProcessRequest.Party.Addresses(iAddressCnt).AddressTypeCode = AddressTypeType.Correspondence Then
                        Dim oUpdateAddressRequest As New BaseUpdateAddressRequestType
                        Dim oUpdateAddressResponse As New BaseUpdateAddressResponseType
                        oUpdateAddressRequest.AddressLine1 = PolicyProcessRequest.Party.Addresses(iAddressCnt).AddressLine1
                        oUpdateAddressRequest.AddressLine2 = PolicyProcessRequest.Party.Addresses(iAddressCnt).AddressLine2
                        oUpdateAddressRequest.AddressLine3 = PolicyProcessRequest.Party.Addresses(iAddressCnt).AddressLine3
                        oUpdateAddressRequest.AddressLine4 = PolicyProcessRequest.Party.Addresses(iAddressCnt).AddressLine4
                        oUpdateAddressRequest.CountryCode = PolicyProcessRequest.Party.Addresses(iAddressCnt).CountryCode
                        oUpdateAddressRequest.PostCode = PolicyProcessRequest.Party.Addresses(iAddressCnt).PostCode
                        oUpdateAddressRequest.PartyKey = PolicyProcessRequest.Policy.PartyKey
                        oUpdateAddressResponse = oBusiness.UpdateAddress(oUpdateAddressRequest)

                    Else
                        Dim oAddAddressRequest As New BaseAddAddressRequestType
                        Dim oAddAddressResponse As New BaseAddAddressResponseType

                        oAddAddressRequest.AddressTypeCode = PolicyProcessRequest.Party.Addresses(iAddressCnt).AddressTypeCode
                        oAddAddressRequest.AddressLine1 = PolicyProcessRequest.Party.Addresses(iAddressCnt).AddressLine1
                        oAddAddressRequest.AddressLine2 = PolicyProcessRequest.Party.Addresses(iAddressCnt).AddressLine2
                        oAddAddressRequest.AddressLine3 = PolicyProcessRequest.Party.Addresses(iAddressCnt).AddressLine3
                        oAddAddressRequest.AddressLine4 = PolicyProcessRequest.Party.Addresses(iAddressCnt).AddressLine4
                        oAddAddressRequest.CountryCode = PolicyProcessRequest.Party.Addresses(iAddressCnt).CountryCode
                        oAddAddressRequest.PostCode = PolicyProcessRequest.Party.Addresses(iAddressCnt).PostCode
                        'oAddAddressRequest.PartyKey = PolicyProcessRequest.Policy.PartyKey
                        oAddAddressResponse = oBusiness.AddAddress(oAddAddressRequest)

                    End If

                Next

            End If

        End If

        PolicyProcessRequest.Policy.AgentKey = agentKey
        PolicyProcessRequest.Policy.AgentKeySpecified = (agentKey <> 0)

        ' Add the Policy record by calling the implementation method 
        ReDim oResponse.Policy(0)
        oResponse.Policy(0) = oBusiness.AddPolicyV2(PolicyProcessRequest.Policy)

        ' Check response for errors
        If (oResponse.Policy(0).STSError Is Nothing) = False Then
            oResponse.STSError = oResponse.Policy(0).STSError
            Return oResponse
        End If


        Return oResponse

    End Function

End Class

