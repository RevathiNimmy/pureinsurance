Option Strict On
Option Explicit On

Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.STSErrorPublisher

Partial Public Class CoreSAMBusiness

#Region "NewBusiness- Messaging Services"

    ''' <summary>
    ''' this method will import new business data via messaging services
    ''' </summary>
    ''' <param name="NewBusinessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NewBusiness(ByVal NewBusinessRequest As MessagingImplementationTypes.NewBusinessRequestType) As MessagingImplementationTypes.NewBusinessResponseType

        Const kMethodName As String = "NewBusiness"

        ' Declare the Response object
        Dim oResponse As New MessagingImplementationTypes.NewBusinessResponseType

        ' Declare the Core SAM business object
        Dim oBusiness As New CoreSAMBusiness

        Dim oAddPartyRequest As New BaseImplementationTypes.BaseAddPartyRequestType
        Dim STSError As New STSErrorPublisher

        Try
            ' Check if the branch code was passed in
            If SAMFunc.NothingToString(NewBusinessRequest.BranchCode) = "" Then
                STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
            End If

            ' Check if the client structure was passed in
            If NewBusinessRequest.Party Is Nothing Then
                STSError.AddInvalidField("Client", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Client"), "")
            End If

            ' Check if the policy structure was passed in
            If NewBusinessRequest.Policy Is Nothing Then
                STSError.AddInvalidField("Policy", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Policy"), "")
            End If

            ' exit if there are any missing parameters
            If STSError.HasErrors Then
                STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), kMethodName, "Mandatory field validation", True)
                Return oResponse
            End If

            ' Add the Party record by calling the implementation method 
            oAddPartyRequest.BranchCode = NewBusinessRequest.BranchCode
            oAddPartyRequest.Party = NewBusinessRequest.Party
            oResponse.Insured = oBusiness.AddParty( _
                        AddPartyRequest:=oAddPartyRequest)

            ' Check response for errors
            If oResponse.Insured.STSError IsNot Nothing Then
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
            If oResponse.Policy(0).STSError IsNot Nothing Then
                oResponse.STSError = oResponse.Policy(0).STSError
                Return oResponse
            End If
        Catch exError As Exception
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), kMethodName, exError.Message.ToString, True)
            Return oResponse
        End Try
        Return oResponse

    End Function

#End Region

#Region "PolicyProcess- Messaging services"

    ''' <summary>
    ''' this method will process policy data via messaging services
    ''' </summary>
    ''' <param name="PolicyProcessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PolicyProcess(ByVal PolicyProcessRequest As MessagingImplementationTypes.PolicyProcessRequestType) As MessagingImplementationTypes.PolicyProcessResponseType

        Const kMethodName As String = "PolicyProcess"

        ' Declare the Response object
        Dim oResponse As New MessagingImplementationTypes.PolicyProcessResponseType

        ' Declare the Core SAM business object
        Dim oBusiness As New CoreSAMBusiness
        Dim nAgentKey As Integer = 0
        Dim STSError As New STSErrorPublisher

        Try
            ' Check if the branch code was passed in
            If SAMFunc.NothingToString(PolicyProcessRequest.BranchCode) = "" Then
                STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
            End If

            ' Check if the client structure was passed in
            If PolicyProcessRequest.Party Is Nothing Then
                STSError.AddInvalidField("Client", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Client"), "")
            End If

            ' Check if the policy structure was passed in
            If PolicyProcessRequest.Policy Is Nothing Then
                STSError.AddInvalidField("Policy", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Policy"), "")
            End If

            If PolicyProcessRequest.AgentCode <> String.Empty Then
                Dim samError As New SAMErrorCollection
                nAgentKey = oBusiness.GetAndValidateSpecifiedTableCode("party", "party_cnt", "shortname", PolicyProcessRequest.AgentCode, samError, "AgentCode")
                ' Check if Agent Code passed in relates to a record in Backoffice
                If (nAgentKey = 0) Then
                    STSError.AddInvalidField("AgentCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(MandatoryInputInvalid, "AgentCode"), "")
                End If
            End If

            ' Check if the policy structure was passed in
            If PolicyProcessRequest.Policy Is Nothing Then
                STSError.AddInvalidField("Policy", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Policy"), "")
            End If

            ' exit if there are any missing parameters
            If STSError.HasErrors Then
                STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), kMethodName, "Mandatory field validation", True)
                Return oResponse
            End If
            If PolicyProcessRequest.Policy.IsMarketPlacePolicy Then
                oResponse = IsMarketPlaceProduct(PolicyProcessRequest)
                If oResponse.STSError IsNot Nothing Then
                    Return oResponse
                End If
            End If

            If PolicyProcessRequest.ClientCodeSpecified Then
                oResponse.Insured = New BaseAddPartyResponseType
                oResponse.Insured.PartyKey = PolicyProcessRequest.ClientID
                PolicyProcessRequest.Policy.PartyKey = PolicyProcessRequest.ClientID
            Else
                'Check if the Party record already exists
                oResponse = CheckForPartyRecord(PolicyProcessRequest, oBusiness)
            End If
            If oResponse.STSError IsNot Nothing Then
                Return oResponse
            End If

            ' If no matching client record has been found then create a new one
            If PolicyProcessRequest.Policy.PartyKey = 0 Then ' This is only possible in case of NB and REN

                ' Add the Party record by calling the implementation method 
                Dim oAddPartyRequest As New BaseImplementationTypes.BaseAddPartyRequestType
                oAddPartyRequest.BranchCode = PolicyProcessRequest.BranchCode
                oAddPartyRequest.AgentKey = nAgentKey
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

            Else
                ' Validate whether data maches with what is supplied with BDX file
                ' Added a new method for updating a party record as for now requirement is only to update few records on Party Table 
                ' and it is not suggestable to make a call to existing Update party method due to the size and performance of a call
                If PolicyProcessRequest.UpdateParty Then
                    If PolicyProcessRequest.Party.GetType Is GetType(BasePartyCCType) Then
                        Dim oUpdatePartyRequest As New BaseImplementationTypes.BasePartyCCType
                        oUpdatePartyRequest.BranchCode = PolicyProcessRequest.BranchCode
                        oUpdatePartyRequest = CType(PolicyProcessRequest.Party, BasePartyCCType)

                        oBusiness.UpdatePartyMainDetails(oUpdatePartyRequest, PolicyProcessRequest.Policy.PartyKey)
                    End If
                    If PolicyProcessRequest.Party.GetType Is GetType(BasePartyPCType) Then
                        Dim oUpdatePartyRequest As New BaseImplementationTypes.BasePartyPCType
                        oUpdatePartyRequest.BranchCode = PolicyProcessRequest.BranchCode
                        oUpdatePartyRequest = CType(PolicyProcessRequest.Party, BasePartyPCType)
                        oBusiness.UpdatePartyPCMainDetails(oUpdatePartyRequest, PolicyProcessRequest.Policy.PartyKey)
                    End If
                    With PolicyProcessRequest.Party
                        For nAddressCnt As Integer = PolicyProcessRequest.Party.Addresses.GetLowerBound(0) To PolicyProcessRequest.Party.Addresses.GetUpperBound(0)
                            With .Addresses(nAddressCnt)
                                If PolicyProcessRequest.Party.Addresses(nAddressCnt).AddressTypeCode = AddressTypeType.Correspondence Then
                                    Dim oUpdateAddressRequest As New BaseUpdateAddressRequestType
                                    Dim oUpdateAddressResponse As New BaseUpdateAddressResponseType
                                    oUpdateAddressRequest.AddressLine1 = .AddressLine1
                                    oUpdateAddressRequest.AddressLine2 = .AddressLine2
                                    oUpdateAddressRequest.AddressLine3 = .AddressLine3
                                    oUpdateAddressRequest.AddressLine4 = .AddressLine4
                                    oUpdateAddressRequest.AddressLine5 = .AddressLine5
                                    oUpdateAddressRequest.AddressLine6 = .AddressLine6
                                    oUpdateAddressRequest.AddressLine7 = .AddressLine7
                                    oUpdateAddressRequest.AddressLine8 = .AddressLine8
                                    oUpdateAddressRequest.AddressLine9 = .AddressLine9
                                    oUpdateAddressRequest.AddressLine10 = .AddressLine10

                                    oUpdateAddressRequest.CountryCode = .CountryCode
                                    oUpdateAddressRequest.PostCode = .PostCode

                                    oUpdateAddressRequest.PartyKey = PolicyProcessRequest.Policy.PartyKey
                                    oUpdateAddressResponse = oBusiness.UpdateAddress(oUpdateAddressRequest)

                                Else
                                    Dim oAddAddressRequest As New BaseAddAddressRequestType
                                    Dim oAddAddressResponse As New BaseAddAddressResponseType

                                    oAddAddressRequest.AddressTypeCode = .AddressTypeCode
                                    oAddAddressRequest.AddressLine1 = .AddressLine1
                                    oAddAddressRequest.AddressLine2 = .AddressLine2
                                    oAddAddressRequest.AddressLine3 = .AddressLine3
                                    oAddAddressRequest.AddressLine4 = .AddressLine4
                                    oAddAddressRequest.AddressLine5 = .AddressLine5
                                    oAddAddressRequest.AddressLine6 = .AddressLine6
                                    oAddAddressRequest.AddressLine7 = .AddressLine7
                                    oAddAddressRequest.AddressLine8 = .AddressLine8
                                    oAddAddressRequest.AddressLine9 = .AddressLine9
                                    oAddAddressRequest.AddressLine10 = .AddressLine10
                                    oAddAddressRequest.CountryCode = .CountryCode
                                    oAddAddressRequest.PostCode = .PostCode
                                    oAddAddressResponse = oBusiness.AddAddress(oAddAddressRequest)
                                End If
                            End With
                        Next
                        'update contact in case client details are modified
                        If .Contacts IsNot Nothing Then
                            Dim oUpdateContactRequest As New BaseReplacePartyContactRequestType
                            Dim oUpdateContactResponse As New BaseReplacePartyContactResponseType
                            oUpdateContactRequest.BranchCode = PolicyProcessRequest.Party.BranchCode
                            oUpdateContactRequest.Contacts = PolicyProcessRequest.Party.Contacts
                            oUpdateContactRequest.PartyKey = PolicyProcessRequest.Policy.PartyKey
                            oUpdateContactRequest.SourceId = PolicyProcessRequest.Policy.SourceId
                            oUpdateContactRequest.UserId = _SiriusUser.UserID
                            oUpdateContactRequest.LoginUserName = PolicyProcessRequest.Policy.LoginUserName
                            oUpdateContactRequest.WCFSecurityToken = PolicyProcessRequest.Policy.WCFSecurityToken
                            oUpdateContactResponse = oBusiness.ReplacePartyContact(oUpdateContactRequest)
                        End If
                    End With
                End If

            End If

            PolicyProcessRequest.Policy.AgentKey = nAgentKey
            PolicyProcessRequest.Policy.AgentKeySpecified = (nAgentKey <> 0)
            PolicyProcessRequest.Policy.SkipGenerateRenewalPolicyNumber = True

            ' Add the Policy record by calling the implementation method 
            ReDim oResponse.Policy(0)
            oResponse.Policy(0) = oBusiness.AddPolicyV2(PolicyProcessRequest.Policy)

            ' Check response for errors
            If oResponse.Policy(0).STSError IsNot Nothing Then
                oResponse.STSError = oResponse.Policy(0).STSError
                Return oResponse
            End If
        Catch exError As Exception
            ' show all the errors in Bordereau
            Throw exError
        End Try
        Return oResponse

    End Function
    ''' <summary>
    ''' This will validate party data against system records
    ''' </summary>
    ''' <param name="PolicyProcessRequest"></param>
    ''' <param name="oBusiness"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckForPartyRecord(ByVal PolicyProcessRequest As MessagingImplementationTypes.PolicyProcessRequestType, ByVal oBusiness As CoreSAMBusiness) As MessagingImplementationTypes.PolicyProcessResponseType

        Dim oResponse As New MessagingImplementationTypes.PolicyProcessResponseType

        Try
            ' Check for Party record

            If PolicyProcessRequest.Party.GetType Is GetType(BasePartyPCType) And UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "POLICY" Then

                ' Process personal client
                Dim oBasePartyPC As BasePartyPCType = DirectCast(PolicyProcessRequest.Party, BasePartyPCType)

                Dim dsPCData As New DataSet

                Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Check_Personal_Client")
                        cmd.AddInParameter("@Surname", SqlDbType.VarChar, 255).Value = Convert.ToString(oBasePartyPC.Surname)
                        cmd.AddInParameter("@ResolvedName", SqlDbType.VarChar, 387).Value = Convert.ToString(oBasePartyPC.Title) + " " + Convert.ToString(oBasePartyPC.Forename) + " " + Convert.ToString(oBasePartyPC.Surname)
                        cmd.AddInParameter("@AddressLine1", SqlDbType.VarChar, 60).Value = Convert.ToString(PolicyProcessRequest.Party.Addresses(0).AddressLine1)
                        Dim ret As Integer = con.ExecuteDataSet(cmd, dsPCData, "Row")
                    End Using
                End Using
                oResponse.Insured = New BaseAddPartyResponseType
                If dsPCData IsNot Nothing AndAlso dsPCData.Tables IsNot Nothing AndAlso dsPCData.Tables.Count > 0 AndAlso dsPCData.Tables(0).Rows IsNot Nothing AndAlso dsPCData.Tables(0).Rows.Count > 0 Then
                    If dsPCData.Tables(0).Rows.Count > 1 Then
                        Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PartyRecordNotFound, "Party cannot be identified", "More than one party record exists for Party name - '" & Convert.ToString(oBasePartyPC.Surname) & "' at address - '" & Convert.ToString(PolicyProcessRequest.Party.Addresses(0).AddressLine1) & "'")
                        STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "CheckForPartyRecord", "Initialise.Business", True)
                        Return oResponse
                    Else
                        With dsPCData.Tables(0).Rows(0)

                            oResponse.Insured.Shortname = Convert.ToString(.Item("resolved_name"))
                            oResponse.Insured.PartyKey = Convert.ToInt32(.Item("party_cnt").ToString)

                            PolicyProcessRequest.Policy.InsuredName = Convert.ToString(.Item("resolved_name"))
                            PolicyProcessRequest.Policy.PartyKey = Convert.ToInt32(.Item("party_cnt").ToString)

                        End With
                    End If
                End If
                Return oResponse
            Else
                Dim findPartyRequest As New SAMForInsuranceV2ImplementationTypes.FindPartyRequestType
                Dim findPartyResponse As BaseFindPartyResponseType

                findPartyRequest.BranchCode = PolicyProcessRequest.Party.BranchCode
                findPartyRequest.Status = "0"

                Dim oBasePartyCC As BasePartyCCType
                Dim oBasePartyPC As BasePartyPCType

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
                End If

                If UCase(Trim(PolicyProcessRequest.Policy.TransactionTypeCode)) = "POLICY" Then
                    For nCnt As Integer = 0 To PolicyProcessRequest.Party.Addresses.GetUpperBound(0)
                        If PolicyProcessRequest.Party.Addresses(nCnt).AddressTypeCode = AddressTypeType.Correspondence Then
                            findPartyRequest.AddressLine1 = PolicyProcessRequest.Party.Addresses(nCnt).AddressLine1
                            findPartyRequest.AddressLine2 = PolicyProcessRequest.Party.Addresses(nCnt).AddressLine2
                            findPartyRequest.PostCode = PolicyProcessRequest.Party.Addresses(nCnt).PostCode
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
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseFindPartyResponseTypeParties), oXmlOverride)
                    If findPartyResponse.ResultDataset IsNot Nothing Then
                        SAMFunc.DeserializeImplementationDataSet(sXMLString:=findPartyResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject)
                        oResultDataSet = DirectCast(oResultDataSetObject, SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.BaseFindPartyResponseTypeParties)
                    End If
                    ' Retrieve the values from the implementation response structure
                    If oResultDataSet.Row.GetUpperBound(0) > 0 Then

                        Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PartyRecordNotFound, "Party cannot be identified", "More than one party record exists for Party name - '" & findPartyRequest.Name & "' at address - '" & findPartyRequest.AddressLine1 & "'")
                        STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "CheckForPartyRecord", "Initialise.Business", True)
                        Return oResponse

                    ElseIf oResultDataSet.Row.GetUpperBound(0) = 0 Then

                        oResponse.Insured = New BaseAddPartyResponseType

                        oResponse.Insured.Shortname = oResultDataSet.Row(0).ResolvedName
                        oResponse.Insured.PartyKey = oResultDataSet.Row(0).PartyKey

                        PolicyProcessRequest.Policy.InsuredName = oResultDataSet.Row(0).ResolvedName
                        PolicyProcessRequest.Policy.PartyKey = oResultDataSet.Row(0).PartyKey
                    End If

                End If
                Return oResponse
            End If
        Catch exError As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PartyRecordNotFound, "Party cannot be identified", exError.Message.ToString)
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "CheckForPartyRecord", exError.Message.ToString, True)
            Return oResponse
        End Try
    End Function

    ''' <summary>
    ''' this method to generate documents attached to a process(Quote,NB,MTAQuote,MTA)
    ''' </summary>
    ''' <param name="oGenerateDocumentsV2Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateDocumentsV2(ByVal oGenerateDocumentsV2Request As MessagingImplementationTypes.GenerateDocumentsV2RequestType) As MessagingImplementationTypes.GenerateDocumentsV2ResponseType
        Dim oResponse As New MessagingImplementationTypes.GenerateDocumentsV2ResponseType
        Dim oGenerateDocumentRequest As New BaseGenerateDocumentRequestType
        Dim oGenerateDocumentResponse As BaseGenerateDocumentResponseType
        Try
            Using conSiriusConnection As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                Using cmdSiriusCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_GetDocumentsForProcessType")
                    cmdSiriusCommand.AddInParameter("@nProcessType", SqlDbType.Int).Value = Convert.ToInt32(oGenerateDocumentsV2Request.DocumentProcessType)
                    cmdSiriusCommand.AddInParameter("@nInsuranceFileKey", SqlDbType.Int).Value = Convert.ToInt32(oGenerateDocumentsV2Request.InsuranceFileKey)
                    If oGenerateDocumentsV2Request.PolicyProcessType = PolicyProcessTypes.Quote Then
                        cmdSiriusCommand.AddInParameter("@bIsQuotation", SqlDbType.TinyInt).Value = True
                    End If
                    Dim dsResults As DataSet = Nothing
                    dsResults = conSiriusConnection.ExecuteDataSet(cmdSiriusCommand, "Documents")
                    If dsResults IsNot Nothing AndAlso dsResults.Tables.Count > 0 AndAlso dsResults.Tables(0).Rows.Count > 0 Then
                        With dsResults.Tables(0)
                            For iRowCount As Integer = 0 To .Rows.Count - 1
                                oGenerateDocumentRequest.BranchCode = oGenerateDocumentsV2Request.BranchCode
                                oGenerateDocumentRequest.DocumentTemplateCode = .Rows(iRowCount).Item("Code").ToString
                                oGenerateDocumentRequest.InsuranceFileKey = oGenerateDocumentsV2Request.InsuranceFileKey
                                oGenerateDocumentRequest.PartyKey = CInt(.Rows(iRowCount).Item("PartyKey"))
                                oGenerateDocumentRequest.InsuranceFolderKey = CInt(.Rows(iRowCount).Item("InsuranceFolderKey"))
                                oGenerateDocumentRequest.ClaimKey = oGenerateDocumentsV2Request.ClaimKey
                                oGenerateDocumentRequest.OutputAsPDF = True

                                oGenerateDocumentResponse = GenerateDocument(oGenerateDocumentRequest)
                                If oGenerateDocumentResponse IsNot Nothing AndAlso oGenerateDocumentResponse.Errors Is Nothing Then
                                    oResponse.Documents = New System.Collections.Generic.List(Of BaseGenerateDocumentV2ResponseTypeDocument)
                                    Dim oDocument As New BaseGenerateDocumentV2ResponseTypeDocument
                                    oDocument.DocumentCode = .Rows(iRowCount).Item("Code").ToString
                                    oDocument.DocumentDescription = .Rows(iRowCount).Item("description").ToString
                                    oDocument.SpooledZipFile = oGenerateDocumentResponse.SpooledZipFile
                                    oDocument.MergedFilePath = oGenerateDocumentResponse.MergedFilePath
                                    oResponse.Documents.Add(oDocument)
                                End If
                            Next
                        End With
                    End If
                End Using
            End Using

            Return oResponse
        Catch exError As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.GenerateDocumentFailed, "Failed to generate document", "Failed to generate document for this process")
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "GenerateDocumentsV2", "GenerateDocumentsV2", True)
            Return oResponse
        End Try
    End Function

    ''' <summary>
    ''' this method to check whether requested data model is valid market place product or not
    ''' </summary>
    ''' <param name="oPolicyProcessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsMarketPlaceProduct(ByVal oPolicyProcessRequest As MessagingImplementationTypes.PolicyProcessRequestType) As MessagingImplementationTypes.PolicyProcessResponseType

        Dim oResponse As New MessagingImplementationTypes.PolicyProcessResponseType

        Try
            Dim sDataModelCode As String
            For iCount As Integer = 0 To oPolicyProcessRequest.Policy.Risks.GetUpperBound(0)
                sDataModelCode = oPolicyProcessRequest.Policy.Risks(iCount).DataModelCode
                ' Check for MarketPlace Product
                Dim dsProduct As New DataSet

                Using conSiriusConnection As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                    Using cmdSiriusCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CheckIsMarketplaceProduct")
                        cmdSiriusCommand.AddInParameter("@sDataModelCode", SqlDbType.VarChar, 10).Value = sDataModelCode
                        conSiriusConnection.ExecuteDataSet(cmdSiriusCommand, dsProduct, "Row")
                    End Using
                End Using
                If dsProduct IsNot Nothing AndAlso dsProduct.Tables IsNot Nothing AndAlso dsProduct.Tables.Count > 0 AndAlso dsProduct.Tables(0).Rows IsNot Nothing AndAlso dsProduct.Tables(0).Rows.Count > 0 Then
                    If Not CBool(dsProduct.Tables(0).Rows(0).ItemArray(0)) Then
                        Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.NotValidMarketPlaceProduct, "Not Valid MarketPlace Product", "Not Valid MarketPlace Product")
                        STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "IsMarketPlaceProduct", "IsMarketPlaceProduct", True)
                        Return oResponse
                    End If
                Else
                    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.NotValidMarketPlaceProduct, "Not Valid MarketPlace Product", "Not Valid MarketPlace Product")
                    STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "IsMarketPlaceProduct", "IsMarketPlaceProduct", True)
                    Return oResponse
                End If
            Next
            Return oResponse
        Catch exError As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.NotValidMarketPlaceProduct, "Not Valid MarketPlace Product", "Not Valid MarketPlace Product")
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "IsMarketPlaceProduct", "IsMarketPlaceProduct", True)
            Return oResponse
        End Try
    End Function
#End Region

End Class
