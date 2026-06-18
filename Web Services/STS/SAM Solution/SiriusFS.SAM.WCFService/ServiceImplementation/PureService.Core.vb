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
Imports System.Xml


Partial Public Class PureService


    ''' <summary>
    ''' To Add a new Address
    ''' </summary>
    ''' <param name="AddAddressRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddAddress(ByVal AddAddressRequest As AddAddressRequestType) As AddAddressResponseType Implements IPurePolicyService.AddAddress

        Try

            Dim sUserName As String = AddAddressRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAAddr", iUserId)
            CommonFunctions.CheckSecurityToken(AddAddressRequest.WCFSecurityToken)
            Dim oResponse As New AddAddressResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddAddressRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddAddressRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddAddressResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AddressKey = AddAddressRequest.AddressKey
            oImpRequest.AddressLine1 = AddAddressRequest.AddressLine1
            oImpRequest.AddressLine2 = AddAddressRequest.AddressLine2
            oImpRequest.AddressLine3 = AddAddressRequest.AddressLine3
            oImpRequest.AddressLine4 = AddAddressRequest.AddressLine4
            oImpRequest.AddressLine5 = AddAddressRequest.AddressLine5
            oImpRequest.AddressLine6 = AddAddressRequest.AddressLine6
            oImpRequest.AddressLine7 = AddAddressRequest.AddressLine7
            oImpRequest.AddressLine8 = AddAddressRequest.AddressLine8
            oImpRequest.AddressLine9 = AddAddressRequest.AddressLine9
            oImpRequest.AddressLine10 = AddAddressRequest.AddressLine10

            oImpRequest.AddressTypeCode = CType(AddAddressRequest.AddressTypeCode, BaseImplementationTypes.AddressTypeType)
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.CountryCode = AddAddressRequest.CountryCode
            oImpRequest.PostCode = AddAddressRequest.PostCode
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddAddress(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.AddressKey = oImpResponse.AddressKey

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddAddressRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddAddressRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    '''This web method is used to add the Bank Guarantee items 
    '''</summary>
    '''<param name="oAddBankGuaranteeRequest">An object of the class AddBankGuaranteeRequestType</param>
    '''<remarks></remarks> 
    Public Function AddBankGuarantee(ByVal oAddBankGuaranteeRequest As AddBankGuaranteeRequestType) As AddBankGuaranteeResponseType Implements IPureAccountService.AddBankGuarantee

        Try


            Dim sUserName As String = oAddBankGuaranteeRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMBGADD", iUserId)
            CommonFunctions.CheckSecurityToken(oAddBankGuaranteeRequest.WCFSecurityToken)
            Dim oResponse As New AddBankGuaranteeResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAddBankGuaranteeRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddBankGuaranteeRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddBankGuaranteeResponseType = Nothing
            If (oAddBankGuaranteeRequest IsNot Nothing) Then
                If (oAddBankGuaranteeRequest.BankGuarantee.Count >= 1) Then
                    ReDim Preserve oImpRequest.BankGuarantee(oAddBankGuaranteeRequest.BankGuarantee.Count - 1)
                    For iCount As Integer = 0 To oAddBankGuaranteeRequest.BankGuarantee.Count - 1
                        oImpRequest.BranchCode = oAddBankGuaranteeRequest.BranchCode
                        oImpRequest.BankGuarantee(iCount) = New BaseImplementationTypes.BaseBankGuaranteeItemType
                        With oImpRequest.BankGuarantee(iCount)
                            'Bank Guarantee Main fields
                            .BankBranch = oAddBankGuaranteeRequest.BankGuarantee(iCount).BankBranch
                            .BankGuaranteeRef = oAddBankGuaranteeRequest.BankGuarantee(iCount).BankGuaranteeRef
                            .BankNameDesc = oAddBankGuaranteeRequest.BankGuarantee(iCount).BankNameDesc
                            .BankNameKey = oAddBankGuaranteeRequest.BankGuarantee(iCount).BankNameKey
                            .BGKey = oAddBankGuaranteeRequest.BankGuarantee(iCount).BGKey
                            .BGCurrencyCode = oAddBankGuaranteeRequest.BankGuarantee(iCount).BGCurrencyCode
                            .BGLimit = oAddBankGuaranteeRequest.BankGuarantee(iCount).BGLimit
                            .BGStatusCode = oAddBankGuaranteeRequest.BankGuarantee(iCount).BGStatusCode
                            .BGTimeStamp = oAddBankGuaranteeRequest.BankGuarantee(iCount).BGTimeStamp
                            .CustodyBranchCode = oAddBankGuaranteeRequest.BankGuarantee(iCount).CustodyBranchCode
                            .ExpiryDate = oAddBankGuaranteeRequest.BankGuarantee(iCount).ExpiryDate
                            .IssueDate = oAddBankGuaranteeRequest.BankGuarantee(iCount).IssueDate
                            .IsDeleted = oAddBankGuaranteeRequest.BankGuarantee(iCount).IsDeleted
                            .IsPolicyLock = oAddBankGuaranteeRequest.BankGuarantee(iCount).IsPolicyLock
                            .LimitAvailable = oAddBankGuaranteeRequest.BankGuarantee(iCount).LimitAvailable
                            .PartyKey = oAddBankGuaranteeRequest.BankGuarantee(iCount).PartyKey

                            'Products
                            If (oAddBankGuaranteeRequest.BankGuarantee(iCount).Products IsNot Nothing) Then
                                ReDim Preserve oImpRequest.BankGuarantee(iCount).Products(oAddBankGuaranteeRequest.BankGuarantee(iCount).Products.Count - 1)
                                For iProductCount As Integer = 0 To oAddBankGuaranteeRequest.BankGuarantee(iCount).Products.Count - 1
                                    .Products(iProductCount) = New BaseImplementationTypes.BaseBankGuaranteeItemTypeProducts
                                    .Products(iProductCount).ProductCode = oAddBankGuaranteeRequest.BankGuarantee(iCount).Products(iProductCount).ProductCode
                                    .Products(iProductCount).Description = oAddBankGuaranteeRequest.BankGuarantee(iCount).Products(iProductCount).Description
                                Next
                            End If
                            'Branches

                            If (oAddBankGuaranteeRequest.BankGuarantee(iCount).Branches IsNot Nothing) Then
                                ReDim Preserve oImpRequest.BankGuarantee(iCount).Branches(oAddBankGuaranteeRequest.BankGuarantee(iCount).Branches.Count - 1)
                                For iBranchCount As Integer = 0 To oAddBankGuaranteeRequest.BankGuarantee(iCount).Branches.Count - 1
                                    .Branches(iBranchCount) = New BaseImplementationTypes.BaseBankGuaranteeItemTypeBranches
                                    .Branches(iBranchCount).BranchCode = oAddBankGuaranteeRequest.BankGuarantee(iCount).Branches(iBranchCount).BranchCode
                                    .Branches(iBranchCount).Description = oAddBankGuaranteeRequest.BankGuarantee(iCount).Branches(iBranchCount).Description
                                Next
                            End If
                        End With

                    Next
                End If
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AddBankGuarantee(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.BankGuarantee = New BaseAddBankGuaranteeResponseTypeBankGuarantee

                oResponse.BankGuarantee = oImpResponse.BankGuarantee.Row.ToList().ConvertAll(
                                   New Converter(Of BaseImplementationTypes.BaseAddBankGuaranteeResponseTypeBankGuaranteeRow, BaseAddBankGuaranteeResponseTypeRow) _
                                   (AddressOf CommonFunctions.ToServiceBankGuaranteeList))

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAddBankGuaranteeRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAddBankGuaranteeRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This web method is used to add the Cash Deposit items 
    ''' </summary>
    ''' <param name="oAddCashDepositRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddCashDeposit(ByVal oAddCashDepositRequest As AddCashDepositRequestType) As AddCashDepositResponseType Implements IPureAccountService.AddCashDeposit



        Dim sUserName As String = oAddCashDepositRequest.LoginUserName
        Dim iAgentKey As Integer
        Dim iUserId As Integer

        CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
        CommonFunctions.CheckAuthority("SAMCDAdd", iUserId)
        CommonFunctions.CheckSecurityToken(oAddCashDepositRequest.WCFSecurityToken)
        Dim oResponse As New AddCashDepositResponseType
        Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAddCashDepositRequest.BranchCode)

        ' Implementation structures
        Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddCashDepositRequestType
        Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddCashDepositResponseType = Nothing

        ' Pass the values to the implementation request structure
        Try
            If (oAddCashDepositRequest IsNot Nothing) Then
                If (oAddCashDepositRequest.CashDeposit.Count >= 1) Then
                    ReDim Preserve oImpRequest.CashDeposit(oAddCashDepositRequest.CashDeposit.Count - 1)
                    For iCount As Integer = 0 To oAddCashDepositRequest.CashDeposit.Count - 1
                        oImpRequest.BranchCode = oAddCashDepositRequest.BranchCode
                        oImpRequest.CashDeposit(iCount) = New BaseImplementationTypes.BaseCommonCashDepositItemType
                        With oImpRequest.CashDeposit(iCount)
                            .CashDepositRef = oAddCashDepositRequest.CashDeposit(iCount).CashDepositRef
                            .IsSinglePolicy = oAddCashDepositRequest.CashDeposit(iCount).IsSinglePolicy
                            .CDTimeStamp = oAddCashDepositRequest.CashDeposit(iCount).CDTimeStamp
                            .PartyType = CType([Enum].ToObject(GetType(ClientAgentType), oAddCashDepositRequest.CashDeposit(iCount).PartyType), BaseImplementationTypes.ClientAgentType)
                            .PartyCode = oAddCashDepositRequest.CashDeposit(iCount).PartyCode
                            .UserName = oAddCashDepositRequest.CashDeposit(iCount).UserName
                            If (oAddCashDepositRequest.CashDeposit(iCount).Products IsNot Nothing) Then
                                ReDim Preserve oImpRequest.CashDeposit(iCount).Products(oAddCashDepositRequest.CashDeposit(iCount).Products.Count - 1)
                                For iProductCount As Integer = 0 To oAddCashDepositRequest.CashDeposit(iCount).Products.Count - 1
                                    .Products(iProductCount) = New BaseImplementationTypes.BaseCommonCashDepositItemTypeProducts
                                    .Products(iProductCount).ProductCode = oAddCashDepositRequest.CashDeposit(iCount).Products(iProductCount).ProductCode
                                    .Products(iProductCount).Description = oAddCashDepositRequest.CashDeposit(iCount).Products(iProductCount).Description
                                Next
                            End If
                            'Branches

                            If (oAddCashDepositRequest.CashDeposit(iCount).Branches IsNot Nothing) Then
                                ReDim Preserve oImpRequest.CashDeposit(iCount).Branches(oAddCashDepositRequest.CashDeposit(iCount).Branches.Count - 1)
                                For iBranchCount As Integer = 0 To oAddCashDepositRequest.CashDeposit(iCount).Branches.Count - 1
                                    .Branches(iBranchCount) = New BaseImplementationTypes.BaseCommonCashDepositItemTypeBranches
                                    .Branches(iBranchCount).BranchCode = oAddCashDepositRequest.CashDeposit(iCount).Branches(iBranchCount).BranchCode
                                    .Branches(iBranchCount).Description = oAddCashDepositRequest.CashDeposit(iCount).Branches(iBranchCount).Description
                                Next
                            End If
                        End With

                    Next
                End If
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AddCashDeposit(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.CashDeposit = oImpResponse.CashDeposit.Row.ToList().ConvertAll(
                                  New Converter(Of BaseImplementationTypes.BaseAddCashDepositResponseTypeCashDepositRow, BaseAddCashDepositResponseTypeRow) _
                                  (AddressOf CommonFunctions.ToServiceCashDepositList))



            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAddCashDepositRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAddCashDepositRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This web method is used to add the Cover Notebook Items
    ''' </summary>
    ''' <param name="oAddCoverNoteBookRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddCoverNoteBook(ByVal oAddCoverNoteBookRequest As AddCoverNoteBookRequestType) As AddCoverNoteBookResponseType Implements IPureCoreService.AddCoverNoteBook




        Dim sUserName As String = oAddCoverNoteBookRequest.LoginUserName
        Dim iAgentKey As Integer
        Dim iUserId As Integer

        CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
        CommonFunctions.CheckAuthority("SAMACNBKey", iUserId)
        CommonFunctions.CheckSecurityToken(oAddCoverNoteBookRequest.WCFSecurityToken)
        Dim oResponse As New AddCoverNoteBookResponseType
        Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAddCoverNoteBookRequest.BranchCode)

        ' Implementation structures
        Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddCoverNoteBookRequestType
        Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddCoverNoteBookResponseType = Nothing
        Try
            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oAddCoverNoteBookRequest.BranchCode
            oImpRequest.BookNumber = oAddCoverNoteBookRequest.BookNumber
            oImpRequest.StartNumber = oAddCoverNoteBookRequest.StartNumber
            oImpRequest.EndNumber = oAddCoverNoteBookRequest.EndNumber
            oImpRequest.CoverNoteStatusCode = oAddCoverNoteBookRequest.CoverNoteStatusCode
            oImpRequest.AgentKeySpecified = oAddCoverNoteBookRequest.AgentKeySpecified
            oImpRequest.AgentKey = oAddCoverNoteBookRequest.AgentKey
            oImpRequest.EffectiveDate = oAddCoverNoteBookRequest.EffectiveDate
            oImpRequest.CoverNoteBranchCode = oAddCoverNoteBookRequest.CoverNoteBranchCode

            If oAddCoverNoteBookRequest.CoverNoteProducts IsNot Nothing Then
                If oAddCoverNoteBookRequest.CoverNoteProducts.Count > 0 Then
                    Dim iLength As Int32 = oAddCoverNoteBookRequest.CoverNoteProducts.Count
                    oImpRequest.CoverNoteProducts = New BaseImplementationTypes.BaseCoverNoteBookTypeCoverNoteProducts
                    For icount As Integer = 0 To iLength - 1
                        ReDim Preserve oImpRequest.CoverNoteProducts.Row(icount)
                        oImpRequest.CoverNoteProducts.Row(icount) = New BaseImplementationTypes.BaseCoverNoteBookTypeCoverNoteProductsRow
                        oImpRequest.CoverNoteProducts.Row(icount).ProductCode = oAddCoverNoteBookRequest.CoverNoteProducts.Item(icount).ProductCode
                    Next
                End If
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AddCoverNoteBook(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.CoverNoteBookTimestamp = oImpResponse.CoverNoteBookTimestamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAddCoverNoteBookRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAddCoverNoteBookRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used to Add Cover Note Sheet
    ''' </summary>  
    ''' <param name="oAddCoverNoteSheetRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddCoverNoteSheetRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddCoverNoteSheetResponseType</returns>  
    Public Function AddCoverNoteSheet(ByVal oAddCoverNoteSheetRequest As AddCoverNoteSheetRequestType) As AddCoverNoteSheetResponseType Implements IPureCoreService.AddCoverNoteSheet

        Try
            'CheckAuthority("SAMACNSKey")

            Dim sUserName As String = oAddCoverNoteSheetRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New AddCoverNoteSheetResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAddCoverNoteSheetRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddCoverNoteSheetRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddCoverNoteSheetResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oAddCoverNoteSheetRequest.BranchCode
            oImpRequest.CoverNoteBookKey = oAddCoverNoteSheetRequest.CoverNoteBookKey
            oImpRequest.CoverNoteSheetNumber = oAddCoverNoteSheetRequest.CoverNoteSheetNumber
            oImpRequest.CoverNoteStatusCode = oAddCoverNoteSheetRequest.CoverNoteStatusCode
            oImpRequest.Comments = oAddCoverNoteSheetRequest.Comments
            oImpRequest.CoverNoteBookTimestamp = oAddCoverNoteSheetRequest.CoverNoteBookTimestamp

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AddCoverNoteSheet(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Set the response values 
                oResponse.CoverNoteBookTimestamp = oImpResponse.CoverNoteBookTimestamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAddCoverNoteSheetRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAddCoverNoteSheetRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This web services method is used to Add Document to DocuMaster
    ''' </summary>
    ''' <param name="AddDocumentToDocumasterRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddDocumentToDocumaster(ByVal AddDocumentToDocumasterRequest As AddDocumentToDocumasterRequestType) As AddDocumentToDocumasterResponseType Implements IPureCoreService.AddDocumentToDocumaster

        Try

            Dim sUserName As String = AddDocumentToDocumasterRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMADDOC", iUserId)
            CommonFunctions.CheckSecurityToken(AddDocumentToDocumasterRequest.WCFSecurityToken)
            Dim oResponse As New AddDocumentToDocumasterResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddDocumentToDocumasterRequest.BranchCode)

            'Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddDocumentToDocumasterRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddDocumentToDocumasterResponseType = Nothing

            'Pass the values to the implementation request structure
            With AddDocumentToDocumasterRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.ClaimKey = .ClaimKey
                oImpRequest.Description = .Description
                oImpRequest.Document = .Document
                oImpRequest.FileName = .FileName
                oImpRequest.InsuranceFolderKey = .InsuranceFolderKey
                oImpRequest.PartyKey = .PartyKey
                oImpRequest.VisibleFromWeb = .VisibleFromWeb
                oImpRequest.FolderNum = .FolderNum
            End With

            Try
                'Call the implementation method
                oImpResponse = oBusiness.AddDocumentToDocumaster(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Get values from implementation layer
                With oImpResponse
                    oResponse.DocNum = .DocNum
                End With
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddDocumentToDocumasterRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddDocumentToDocumasterRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' To Add an Event with details provided in request
    ''' </summary>
    ''' <param name="oAddEventRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddEvent(ByVal oAddEventRequest As AddEventRequestType) As AddEventResponseType Implements IPureCoreService.AddEvent, IPurePolicyService.AddEvent, IPurePartyService.AddEvent
        Try


            Dim sUserName As String = oAddEventRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMEVTLOG", iUserId)
            CommonFunctions.CheckSecurityToken(oAddEventRequest.WCFSecurityToken)
            Dim oResponse As New AddEventResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAddEventRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddEventRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddEventResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oAddEventRequest.BranchCode
            oImpRequest.EventTypeKey = oAddEventRequest.EventTypeKey
            oImpRequest.EventLogSubjectKey = oAddEventRequest.EventLogSubjectKey
            oImpRequest.EventDate = oAddEventRequest.EventDate
            oImpRequest.UserName = oAddEventRequest.UserName
            oImpRequest.RtfText = oAddEventRequest.RtfText
            oImpRequest.PartyKey = oAddEventRequest.PartyKey
            oImpRequest.InsuranceFileKey = oAddEventRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = oAddEventRequest.InsuranceFolderKey
            oImpRequest.ClaimKey = oAddEventRequest.ClaimKey
            oImpRequest.Priority = oAddEventRequest.Priority
            oImpRequest.StatusKey = oAddEventRequest.StatusKey
            oImpRequest.IsManualDescription = oAddEventRequest.IsManualDescription
            oImpRequest.ClaimKey = oAddEventRequest.ClaimKey
            oImpRequest.Document_Path = oAddEventRequest.Document_Path
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AddEvent(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.EventKey = oImpResponse.EventKey

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAddEventRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAddEventRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This Web method is used to add the Event Notes details by passing the Request parameters as request type objects
    ''' and also the response object event key and EventPublicTextKey is being returned .
    '''</summary>
    '''<param name="oAddEventNoteRequest" type="AddEventNoteRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddEventNoteResponseType</returns>  
    '''<remarks></remarks>
    Public Function AddEventNote(ByVal oAddEventNoteRequest As AddEventNoteRequestType) As AddEventNoteResponseType Implements IPureCoreService.AddEventNote

        Try


            Dim sUserName As String = oAddEventNoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMEVTLOG", iUserId)
            CommonFunctions.CheckSecurityToken(oAddEventNoteRequest.WCFSecurityToken)
            Dim oResponse As New AddEventNoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAddEventNoteRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddEventNoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddEventNoteResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oAddEventNoteRequest.BranchCode
            oImpRequest.EventKey = oAddEventNoteRequest.EventKey
            oImpRequest.EventText = oAddEventNoteRequest.EventText

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AddEventNote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.EventKey = oImpResponse.EventKey
                oResponse.EventPublicTextKey = oImpResponse.EventPublicTextKey

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAddEventNoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAddEventNoteRequest))
            Return Nothing
        End Try
    End Function

    '''<summary>
    ''' This is webmethod for AddTaskGroup
    '''</summary>
    '''<param name="AddTaskGroupRequest" type="AddTaskGroupRequestType"></param>   
    '''<returns>AddTaskGroupResponseType</returns>
    '''<remarks></remarks> 
    Public Function AddTaskGroup(ByVal AddTaskGroupRequest As AddTaskGroupRequestType) As AddTaskGroupResponseType Implements IPureCoreService.AddTaskGroup

        Try
            'Assign appropriate key, i.e.
            Dim sUserName As String = AddTaskGroupRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            Dim oResponse As New AddTaskGroupResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddTaskGroupRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddTaskGroupRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddTaskGroupResponseType = Nothing

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMATG", iUserId)
            CommonFunctions.CheckSecurityToken(AddTaskGroupRequest.WCFSecurityToken)
            'India : Pass the values to the implementation request structure, i.e.
            oImpRequest.BranchCode = AddTaskGroupRequest.BranchCode
            oImpRequest.Code = AddTaskGroupRequest.Code
            oImpRequest.Description = AddTaskGroupRequest.Description
            oImpRequest.IsDeleted = AddTaskGroupRequest.IsDeleted
            oImpRequest.TaskGroupCategoryKey = AddTaskGroupRequest.TaskGroupCategoryKey
            oImpRequest.EffectiveDate = AddTaskGroupRequest.EffectiveDate

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AddTaskGroup(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'India : Set the response values here, e.g.
                oResponse.TaskGroupKey = oImpResponse.TaskGroupKey
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddTaskGroupRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddTaskGroupRequest))
            Return Nothing

        End Try

    End Function

    ''' <summary> 
    ''' Adds a new user group and retrieves the newly added user group id. 
    ''' </summary>  
    ''' <param name="AddUserGroupRequest">An object of type SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.AddUserGroupRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.AddUserGroupResponseType</returns>  
    Public Function AddUserGroup(ByVal AddUserGroupRequest As AddUserGroupRequestType) As AddUserGroupResponseType Implements IPureCoreService.AddUserGroup

        Try



            Dim sUserName As String = AddUserGroupRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAUG", iUserId)
            CommonFunctions.CheckSecurityToken(AddUserGroupRequest.WCFSecurityToken)
            Dim oResponse As New AddUserGroupResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddUserGroupRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddUserGroupRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddUserGroupResponseType = Nothing

            oImpRequest.BranchCode = AddUserGroupRequest.BranchCode
            oImpRequest.Code = AddUserGroupRequest.Code
            oImpRequest.Description = AddUserGroupRequest.Description
            oImpRequest.EffectiveDate = AddUserGroupRequest.EffectiveDate
            oImpRequest.IsDeleted = AddUserGroupRequest.IsDeleted
            oImpRequest.IsSysAdmin = AddUserGroupRequest.IsSysAdmin

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AddUserGroup(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.UserGroupKey = oImpResponse.UserGroupKey

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddUserGroupRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddUserGroupRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This is webservice method for  Add WorkManager Task Log
    '''<param name="AddWmTaskLogRequest" type="AddWmTaskLogRequestType"></param>   
    '''</summary>
    '''<remarks></remarks>  
    Public Function AddWmTaskLog(ByVal AddWmTaskLogRequest As AddWmTaskLogRequestType) As AddWmTaskLogResponseType Implements IPureCoreService.AddWmTaskLog

        Try


            Dim sUserName As String = AddWmTaskLogRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAWTLKey", iUserId)
            CommonFunctions.CheckSecurityToken(AddWmTaskLogRequest.WCFSecurityToken)
            Dim oResponse As New AddWmTaskLogResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddWmTaskLogRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddWmTaskLogRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddWmTaskLogResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = AddWmTaskLogRequest.BranchCode
            oImpRequest.TaskInstanceKey = AddWmTaskLogRequest.TaskInstanceKey
            oImpRequest.LogText = AddWmTaskLogRequest.LogText

            Try
                oImpResponse = oBusiness.AddWmTaskLog(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddWmTaskLogRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddWmTaskLogRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to Attach a cover note by passing the Request parameters as request type objects
    ''' and also the response object  is being returned .
    '''</summary>
    '''<param name="oAttachCoverNoteRequest" type="AttachCoverNoteRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsurance.AttachCoverNoteResponseType</returns>  
    '''<remarks></remarks>

    Public Function AttachCoverNote(ByVal oAttachCoverNoteRequest As AttachCoverNoteRequestType) As AttachCoverNoteResponseType

        Try
            'Assign appropriate key


            Dim sUserName As String = oAttachCoverNoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMARDCN", iUserId)
            CommonFunctions.CheckSecurityToken(oAttachCoverNoteRequest.WCFSecurityToken)
            Dim oResponse As New AttachCoverNoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAttachCoverNoteRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AttachCoverNoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AttachCoverNoteResponseType = Nothing
            Dim oCoverNote As New BaseCoverNoteRiskItemType

            'Pass the values to the implementation request structure as below for all input parameters 
            oImpRequest.BranchCode = oAttachCoverNoteRequest.BranchCode
            oImpRequest.GenerateCoverNoteDocs = oAttachCoverNoteRequest.GenerateCoverNoteDocs
            oImpRequest.ProcessType = CType([Enum].ToObject(GetType(CoverNoteProcessType), oAttachCoverNoteRequest.ProcessType), BaseImplementationTypes.CoverNoteProcessType)
            oImpRequest.CoverNote = New BaseImplementationTypes.BaseCoverNoteRiskItemType
            oImpRequest.CoverNote.RiskKey = oAttachCoverNoteRequest.CoverNote.RiskKey
            oImpRequest.CoverNote.RiskDesc = oAttachCoverNoteRequest.CoverNote.RiskDesc
            oImpRequest.CoverNote.CoverNoteNumber = oAttachCoverNoteRequest.CoverNote.CoverNoteNumber
            oImpRequest.CoverNote.CoverNoteFrom = oAttachCoverNoteRequest.CoverNote.CoverNoteFrom
            oImpRequest.CoverNote.CoverNoteFromSpecified = oAttachCoverNoteRequest.CoverNote.CoverNoteFromSpecified
            oImpRequest.CoverNote.CoverNoteTo = oAttachCoverNoteRequest.CoverNote.CoverNoteTo
            oImpRequest.CoverNote.CoverNoteToSpecified = oAttachCoverNoteRequest.CoverNote.CoverNoteToSpecified
            oImpRequest.CoverNote.TImeStamp = oAttachCoverNoteRequest.CoverNote.TImeStamp

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AttachCoverNote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.CoverNoteRiskId = oImpResponse.CoverNoteRiskId
                oResponse.RiskKey = oImpResponse.RiskKey
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAttachCoverNoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAttachCoverNoteRequest))
            Return Nothing
        End Try

    End Function

    Public Function CreateBackgroundJob(ByVal CreateBackgroundJobRequest As CreateBackgroundJobRequestType) As CreateBackgroundJobResponseType Implements IPureCoreService.CreateBackgroundJob

        Try


            Dim sUserName As String = CreateBackgroundJobRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMADBGJ", iUserId)
            CommonFunctions.CheckSecurityToken(CreateBackgroundJobRequest.WCFSecurityToken)
            Dim oResponse As New CreateBackgroundJobResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, CreateBackgroundJobRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CreateBackgroundJobRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CreateBackgroundJobResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.Description = CreateBackgroundJobRequest.Description
            oImpRequest.JobXML = CreateBackgroundJobRequest.JobXML
            oImpRequest.JobWhenToStart = CreateBackgroundJobRequest.JobWhenToStart
            oImpRequest.BranchCode = CreateBackgroundJobRequest.BranchCode
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CreateBackgroundJob(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.BackgroundJobID = oImpResponse.BackgroundJobID

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(CreateBackgroundJobRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(CreateBackgroundJobRequest))
            Return Nothing
        End Try

    End Function

    Public Function CreateWmTask(ByVal CreateWmTaskRequest As CreateWmTaskRequestType) As CreateWmTaskResponseType Implements IPureCoreService.CreateWmTask

        Try




            Dim sUserName As String = CreateWmTaskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCWMKey", iUserId)
            CommonFunctions.CheckSecurityToken(CreateWmTaskRequest.WCFSecurityToken)
            Dim oResponse As New CreateWmTaskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, CreateWmTaskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CreateWmTaskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CreateWmTaskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AllocationUser = CreateWmTaskRequest.AllocationUser
            oImpRequest.AllocationUserGroup = CreateWmTaskRequest.AllocationUserGroup
            oImpRequest.BranchCode = CreateWmTaskRequest.BranchCode
            oImpRequest.Client = CreateWmTaskRequest.Client
            oImpRequest.Description = CreateWmTaskRequest.Description
            oImpRequest.DueDateTime = CreateWmTaskRequest.DueDateTime
            oImpRequest.IsComplete = CreateWmTaskRequest.IsComplete
            oImpRequest.IsUrgent = CreateWmTaskRequest.IsUrgent
            oImpRequest.Task = CreateWmTaskRequest.Task

            oImpRequest.IsTaskReview = CreateWmTaskRequest.IsTaskReview

            oImpRequest.TaskGroup = CreateWmTaskRequest.TaskGroup
            oImpRequest.UserId = iUserId

            oImpRequest.IsExternalItem = CreateWmTaskRequest.IsExternalItem
            oImpRequest.ParentTaskId = CreateWmTaskRequest.ParentTaskId
            oImpRequest.ExternalTaskCategoryCode = CreateWmTaskRequest.ExternalTaskCategoryCode
            oImpRequest.LockName = CType([Enum].ToObject(GetType(TaskLockName), CreateWmTaskRequest.LockName), BaseImplementationTypes.TaskLockName)
            oImpRequest.LockValue = CreateWmTaskRequest.LockValue
            oImpRequest.GuidPMExternalItem = CreateWmTaskRequest.GuidPMExternalItem
            oImpRequest.ExternalTaskStatus = CreateWmTaskRequest.ExternalTaskStatus
            oImpRequest.IsExternalChildTask = CreateWmTaskRequest.IsExternalChildTask


            If CreateWmTaskRequest.KeyData IsNot Nothing Then
                Dim iLength As Int32 = CreateWmTaskRequest.KeyData.Count
                oImpRequest.KeyData = New BaseImplementationTypes.BaseCreateWmTaskRequestTypeKeyData
                For icount As Integer = 0 To iLength - 1
                    ReDim Preserve oImpRequest.KeyData.Row(icount)
                    oImpRequest.KeyData.Row(icount) = New BaseImplementationTypes.BaseCreateWmTaskRequestTypeKeyDataRow
                    oImpRequest.KeyData.Row(icount).KeyName = CreateWmTaskRequest.KeyData(icount).KeyName
                    oImpRequest.KeyData.Row(icount).KeyValue = CreateWmTaskRequest.KeyData(icount).KeyValue
                Next
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CreateWmTask(oImpRequest)
                If Not oImpResponse Is Nothing Then
                    oResponse.GuidPMExternalItem = oImpResponse.GuidPMExternalItem
                    oResponse.TaskInstanceKey = oImpResponse.TaskInstanceKey
                End If
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(CreateWmTaskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(CreateWmTaskRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    '''This Web method is used to Delete the cover note sheet by passing the CoverNoteSheetKey in the request object 
    '''</summary>
    '''<param name="oDeleteCoverNoteSheetRequest" type="DeleteCoverNoteSheetRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseDeleteCoverNoteSheetResponseType</returns>
    '''<remarks></remarks>
    Public Function DeleteCoverNoteSheet(ByVal oDeleteCoverNoteSheetRequest As DeleteCoverNoteSheetRequestType) As DeleteCoverNoteSheetResponseType Implements IPureCoreService.DeleteCoverNoteSheet

        Try


            Dim sUserName As String = oDeleteCoverNoteSheetRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMDCNSKey", iUserId)
            CommonFunctions.CheckSecurityToken(oDeleteCoverNoteSheetRequest.WCFSecurityToken)
            Dim oResponse As New DeleteCoverNoteSheetResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oDeleteCoverNoteSheetRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.DeleteCoverNoteSheetRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.DeleteCoverNoteSheetResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oDeleteCoverNoteSheetRequest.BranchCode
            oImpRequest.CoverNoteSheetKey = oDeleteCoverNoteSheetRequest.CoverNoteSheetKey
            oImpRequest.CoverNoteBookKey = oDeleteCoverNoteSheetRequest.CoverNoteBookKey
            oImpRequest.CoverNoteBookTimestamp = oDeleteCoverNoteSheetRequest.CoverNoteBookTimestamp

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.DeleteCoverNoteSheet(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Set the response values 
                oResponse.CoverNoteBookTimestamp = oImpResponse.CoverNoteBookTimestamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oDeleteCoverNoteSheetRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oDeleteCoverNoteSheetRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This method is used to delete or undelete user groups
    ''' </summary>  
    ''' <param name="DeleteUndeleteUserGroupRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.DeleteUndeleteUserGroupRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.DeleteUndeleteUserGroupResponseType</returns>  
    Public Function DeleteUndeleteUserGroup(ByVal DeleteUndeleteUserGroupRequest As DeleteUndeleteUserGroupRequestType) As DeleteUndeleteUserGroupResponseType Implements IPureCoreService.DeleteUndeleteUserGroup

        Try


            Dim sUserName As String = DeleteUndeleteUserGroupRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAUG", iUserId)
            CommonFunctions.CheckSecurityToken(DeleteUndeleteUserGroupRequest.WCFSecurityToken)
            Dim oResponse As New DeleteUndeleteUserGroupResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, DeleteUndeleteUserGroupRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.DeleteUndeleteUserGroupRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.DeleteUndeleteUserGroupResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = DeleteUndeleteUserGroupRequest.BranchCode
            oImpRequest.UserGroupCode = DeleteUndeleteUserGroupRequest.UserGroupCode
            oImpRequest.Deleted = DeleteUndeleteUserGroupRequest.Deleted

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.DeleteUndeleteUserGroup(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(DeleteUndeleteUserGroupRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(DeleteUndeleteUserGroupRequest))
            Return Nothing
        End Try

    End Function

    Public Function DeleteWmTask(ByVal oDeleteWmTaskRequest As DeleteWmTaskRequestType) As DeleteWmTaskResponseType Implements IPureCoreService.DeleteWmTask

        Try
            'CheckAuthority("SAMWMDKey")

            Dim sUserName As String = oDeleteWmTaskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New DeleteWmTaskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oDeleteWmTaskRequest.BranchCode)


            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.DeleteWmTaskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.DeleteWmTaskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oDeleteWmTaskRequest.BranchCode
            oImpRequest.TaskInstanceKey = oDeleteWmTaskRequest.TaskInstanceKey
            oImpRequest.TaskTimeStamp = oDeleteWmTaskRequest.TaskTimeStamp

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.DeleteWmTask(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.TaskTimeStamp = oImpResponse.TaskTimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oDeleteWmTaskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oDeleteWmTaskRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used for Post Code and Address Validation.
    ''' </summary>  
    ''' <param name= "FindAddress"> An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindCaseRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.Find CaseResponseType</returns>  
    Public Function FindAddress(ByVal oFindAddressRequest As FindAddressRequestType) As FindAddressResponseType Implements IPureCoreService.FindAddress

        Try

            Dim sUserName As String = oFindAddressRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFINDADD", iUserId)
            CommonFunctions.CheckSecurityToken(oFindAddressRequest.WCFSecurityToken)
            Dim oResponse As New FindAddressResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindAddressRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindAddressRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindAddressResponseType = Nothing


            ' Pass the values to the implementation request structure
            oImpRequest.AddressLine1 = oFindAddressRequest.AddressLine1
            oImpRequest.AddressLine2 = oFindAddressRequest.AddressLine2
            oImpRequest.AddressLine3 = oFindAddressRequest.AddressLine3
            oImpRequest.AddressLine4 = oFindAddressRequest.AddressLine4
            oImpRequest.AddressLine5 = oFindAddressRequest.AddressLine5
            oImpRequest.AddressLine6 = oFindAddressRequest.AddressLine6
            oImpRequest.AddressLine7 = oFindAddressRequest.AddressLine7
            oImpRequest.AddressLine8 = oFindAddressRequest.AddressLine8
            oImpRequest.AddressLine9 = oFindAddressRequest.AddressLine9
            oImpRequest.AddressLine10 = oFindAddressRequest.AddressLine10

            oImpRequest.CountryCode = oFindAddressRequest.CountryCode
            oImpRequest.BranchCode = oFindAddressRequest.BranchCode
            oImpRequest.PostCode = oFindAddressRequest.PostCode
            oImpRequest.WCFSecurityToken = If(oFindAddressRequest.WCFSecurityToken.Length > 0, oFindAddressRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindAddress(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.AddressLookup = SAMFunc.GetDeserializedValues(Of List(Of BaseAddressLookupType))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindAddressResponseTypeAddressLookup", sConvertToTypeName:="BaseAddressLookupType", sFromTypeIsArrayOf:="FindAddress")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.AddressLookup = DataTabletoList_AddressLookupType(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception

                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindAddressRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindAddressRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This method is used to Find the Banks
    ''' </summary>  
    ''' <param name="oFindBankRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindBankRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindBankResponseType</returns>  
    Public Function FindBank(ByVal oFindBankRequest As FindBankRequestType) As FindBankResponseType Implements IPureAccountService.FindBank

        Try
            Dim sUserName As String = oFindBankRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFIB", iUserId)
            CommonFunctions.CheckSecurityToken(oFindBankRequest.WCFSecurityToken)

            Dim oResponse As New FindBankResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindBankRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindBankRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindBankResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindBankRequest.BranchCode
            oImpRequest.ShortCode = oFindBankRequest.ShortCode
            oImpRequest.BankName = oFindBankRequest.BankName
            oImpRequest.MaxRowsToFetch = oFindBankRequest.MaxRowsToFetch
            oImpRequest.MaxRowsToFetchSpecified = oFindBankRequest.MaxRowsToFetchSpecified
            If oFindBankRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = oFindBankRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            oImpRequest.WCFSecurityToken = If(oFindBankRequest.WCFSecurityToken.Length > 0, oFindBankRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.FindBank(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Bank = SAMFunc.GetDeserializedValues(Of List(Of BaseFindBankResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindBankResponseTypeBank", sConvertToTypeName:="BaseFindBankResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Bank = DataTabletoList_FindBank(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindBankRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindBankRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''This Web method is used to find the Bank Guarantee
    '''</summary>
    '''<param name="oFindBankGuaranteeRequest" type="FindBankGuaranteeRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindBankGuaranteeResponseType</returns>
    '''<remarks></remarks>
    Function FindBankGuarantee(ByVal oFindBankGuaranteeRequest As FindBankGuaranteeRequestType) As FindBankGuaranteeResponseType Implements IPureAccountService.FindBankGuarantee
        Try



            Dim sUserName As String = oFindBankGuaranteeRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFIBG", iUserId)
            CommonFunctions.CheckSecurityToken(oFindBankGuaranteeRequest.WCFSecurityToken)

            Dim oResponse As New FindBankGuaranteeResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindBankGuaranteeRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindBankGuaranteeRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindBankGuaranteeResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindBankGuaranteeRequest.BranchCode
            oImpRequest.PartyCode = oFindBankGuaranteeRequest.PartyCode
            oImpRequest.AgentCode = oFindBankGuaranteeRequest.AgentCode
            oImpRequest.InsuranceRef = oFindBankGuaranteeRequest.InsuranceRef
            oImpRequest.BankGuaranteeRef = oFindBankGuaranteeRequest.BankGuaranteeRef
            oImpRequest.BankNameCode = oFindBankGuaranteeRequest.BankNameCode
            oImpRequest.BGStatusCode = oFindBankGuaranteeRequest.BGStatusCode

            oImpRequest.MaxRowsToFetchSpecified = oFindBankGuaranteeRequest.MaxRowsToFetchSpecified
            If oFindBankGuaranteeRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = oFindBankGuaranteeRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            oImpRequest.WCFSecurityToken = If(oFindBankGuaranteeRequest.WCFSecurityToken.Length > 0, oFindBankGuaranteeRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.FindBankGuarantee(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.BankGuarantee = SAMFunc.GetDeserializedValues(Of List(Of BaseFindBankGuaranteeResponseTypeBankGuaranteeRow))(elmResultDataSet:=oImpResponse.BankGuaranteeDataset, sFromTypeName:="BaseFindBankGuaranteeResponseTypeBankGuarantee", sConvertToTypeName:="BaseFindBankGuaranteeResponseTypeBankGuaranteeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.BankGuarantee = DataTabletoList_FindBankGuaranteeBankGuarantee(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindBankGuaranteeRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindBankGuaranteeRequest))
            Return Nothing
        End Try

    End Function

    Public Function FindControlSearch(ByVal FindControlSearchRequest As FindControlSearchRequestType) As FindControlSearchResponseType Implements IPureCoreService.FindControlSearch

        Try
            Dim sUserName As String = FindControlSearchRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFCtlSrc", iUserId)
            CommonFunctions.CheckSecurityToken(FindControlSearchRequest.WCFSecurityToken)

            Dim oResponse As New FindControlSearchResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, FindControlSearchRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindControlSearchRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindControlSearchResponseType = Nothing

            ' Pass the values to the implementation request structure

            oImpRequest.BranchCode = FindControlSearchRequest.BranchCode
            oImpRequest.FindControlKey = FindControlSearchRequest.FindControlKey
            oImpRequest.BranchCode = FindControlSearchRequest.BranchCode

            Dim lCntSearch As Integer
            Dim lLBndSearch As Integer = 0
            Dim lUBndSearch As Integer = FindControlSearchRequest.SearchCriteria.Count - 1

            ReDim oImpRequest.SearchCriteria(lUBndSearch)

            For lCntSearch = lLBndSearch To lUBndSearch
                oImpRequest.SearchCriteria(lCntSearch) = New BaseImplementationTypes.BaseSearchCriteriaType
                oImpRequest.SearchCriteria(lCntSearch).ObjectName = FindControlSearchRequest.SearchCriteria(lCntSearch).ObjectName
                oImpRequest.SearchCriteria(lCntSearch).PropertyName = FindControlSearchRequest.SearchCriteria(lCntSearch).PropertyName
                oImpRequest.SearchCriteria(lCntSearch).Value = FindControlSearchRequest.SearchCriteria(lCntSearch).Value
            Next lCntSearch

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindControlSearch(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.Matches = oImpResponse.Matches

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(FindControlSearchRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(FindControlSearchRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used to find Cover Note Books.
    ''' </summary>  
    ''' <param name="oFindCoverNoteBooksRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindCoverNoteBooksRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindCoverNoteBooksResponseType</returns>  
    Public Function FindCoverNoteBooks(ByVal oFindCoverNoteBooksRequest As FindCoverNoteBooksRequestType) As FindCoverNoteBooksResponseType Implements IPureCoreService.FindCoverNoteBooks

        Try


            Dim sUserName As String = oFindCoverNoteBooksRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFCNBKey", iUserId)
            CommonFunctions.CheckSecurityToken(oFindCoverNoteBooksRequest.WCFSecurityToken)
            Dim oResponse As New FindCoverNoteBooksResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindCoverNoteBooksRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindCoverNoteBooksRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindCoverNoteBooksResponseType = Nothing

            ' Pass the values to the implementation request structure
            With oFindCoverNoteBooksRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.BookNumber = .BookNumber
                oImpRequest.StartNumberSpecified = .StartNumberSpecified
                oImpRequest.StartNumber = .StartNumber
                oImpRequest.EndNumberSpecified = .EndNumberSpecified
                oImpRequest.EndNumber = .EndNumber
                oImpRequest.AgentKeySpecified = .AgentKeySpecified
                oImpRequest.AgentKey = .AgentKey
                oImpRequest.CoverNoteBranchCode = .CoverNoteBranchCode
                oImpRequest.LastUpdatedSpecified = .LastUpdatedSpecified
                oImpRequest.LastUpdated = .LastUpdated
                oImpRequest.CoverNoteStatusCode = .CoverNoteStatusCode
                oImpRequest.PolicyNumber = .PolicyNumber
                oImpRequest.AssignedDateSpecified = .AssignedDateSpecified
                oImpRequest.AssignedDate = .AssignedDate

                oImpRequest.MaxRowsToFetchSpecified = .MaxRowsToFetchSpecified
                If .MaxRowsToFetchSpecified = True Then
                    oImpRequest.MaxRowsToFetch = .MaxRowsToFetch
                Else
                    oImpRequest.MaxRowsToFetch = -1
                End If
                oImpRequest.WCFSecurityToken = If(.WCFSecurityToken.Length > 0, .WCFSecurityToken, "WCFSecurityToken")
            End With

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindCoverNoteBooks(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.FindCoverNoteBooks = SAMFunc.GetDeserializedValues(Of List(Of BaseFindCoverNoteBooksResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindCoverNoteBooksResponseTypeFindCoverNoteBooks", sConvertToTypeName:="BaseFindCoverNoteBooksResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.FindCoverNoteBooks = DataTabletoList_FindCoverNoteBooks(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindCoverNoteBooksRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindCoverNoteBooksRequest))
            Return Nothing
        End Try

    End Function

    Public Function FindDMEDocuments(ByVal FindDMEDocumentsRequest As FindDMEDocumentsRequestType) As FindDMEDocumentsResponseType Implements IPureCoreService.FindDMEDocuments
        Try


            Dim sUserName As String = FindDMEDocumentsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFINDDME", iUserId)
            CommonFunctions.CheckSecurityToken(FindDMEDocumentsRequest.WCFSecurityToken)
            Dim oResponse As New FindDMEDocumentsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, FindDMEDocumentsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindDMEDocumentsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindDMEDocumentsResponseType = Nothing

            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = FindDMEDocumentsRequest.BranchCode
            oImpRequest.ClaimNumber = FindDMEDocumentsRequest.ClaimNumber
            oImpRequest.DocumentDescription = FindDMEDocumentsRequest.DocumentDescription
            oImpRequest.IncludeFiles = FindDMEDocumentsRequest.IncludeFiles
            oImpRequest.PartyCode = FindDMEDocumentsRequest.PartyCode
            oImpRequest.PartyName = FindDMEDocumentsRequest.PartyName
            oImpRequest.PolicyNumber = FindDMEDocumentsRequest.PolicyNumber
            oImpRequest.PostCode = FindDMEDocumentsRequest.PostCode
            oImpRequest.RiskIndex = FindDMEDocumentsRequest.RiskIndex
            oImpRequest.FolderName = FindDMEDocumentsRequest.FolderName
            oImpRequest.ParentNum = FindDMEDocumentsRequest.ParentNum
            oImpRequest.AgentKey = iAgentKey
            Try
                ' Call the implementation method

                oImpResponse = oBusiness.FindDMEDocuments(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If (oImpResponse.Folders IsNot Nothing) Then
                    oResponse.Folders = oImpResponse.Folders.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseDMEFolderType, BaseDMEFolderType)(AddressOf CommonFunctions.ToServiceDmeFolderList))
                End If

                If (oImpResponse.Documents IsNot Nothing) Then
                    oResponse.Documents = oImpResponse.Documents.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseDocumentType, BaseDocumentType)(AddressOf CommonFunctions.ToServiceDocumentList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(FindDMEDocumentsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(FindDMEDocumentsRequest))
            Return Nothing
        End Try

    End Function

    Public Function FindDocumentTemplates(ByVal oFindDocumentTemplatesRequest As FindDocumentTemplatesRequestType) As FindDocumentTemplatesResponseType Implements IPurePolicyService.FindDocumentTemplates
        Try


            Dim sUserName As String = oFindDocumentTemplatesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(oFindDocumentTemplatesRequest.WCFSecurityToken)
            Dim oResponse As New FindDocumentTemplatesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindDocumentTemplatesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindDocumentTemplatesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindDocumentTemplatesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindDocumentTemplatesRequest.BranchCode
            oImpRequest.Code = oFindDocumentTemplatesRequest.Code
            oImpRequest.EffectiveDate = oFindDocumentTemplatesRequest.EffectiveDate
            oImpRequest.EffectiveDateSpecified = oFindDocumentTemplatesRequest.EffectiveDateSpecified
            oImpRequest.TypeCode = oFindDocumentTemplatesRequest.TypeCode

            oImpRequest.ProductCode = oFindDocumentTemplatesRequest.ProductCode
            oImpRequest.RiskTypeCode = oFindDocumentTemplatesRequest.RiskTypeCode
            oImpRequest.ViaClientManager = oFindDocumentTemplatesRequest.ViaClientManager

            oImpRequest.ObjectName = oFindDocumentTemplatesRequest.ObjectName
            oImpRequest.PropertyName = oFindDocumentTemplatesRequest.PropertyName

            oImpRequest.MaxRowsToFetchSpecified = oFindDocumentTemplatesRequest.MaxRowsToFetchSpecified
            If oFindDocumentTemplatesRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = oFindDocumentTemplatesRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            oImpRequest.WCFSecurityToken = If(oFindDocumentTemplatesRequest.WCFSecurityToken.Length > 0, oFindDocumentTemplatesRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindDocumentTemplates(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.DocumentTemplates = SAMFunc.GetDeserializedValues(Of List(Of BaseFindDocumentTemplatesResponseTypeRow))(elmResultDataSet:=oImpResponse.DocumentTemplatesDataset, sFromTypeName:="BaseFindDocumentTemplatesResponseTypeDocumentTemplates", sConvertToTypeName:="BaseFindDocumentTemplatesResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.DocumentTemplates = DataTabletoList_FindDocumentTemplates(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindDocumentTemplatesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindDocumentTemplatesRequest))
            Return Nothing
        End Try

    End Function

    Public Function FindUsers(ByVal oFindUsersRequest As FindUsersRequestType) As FindUsersResponseType Implements IPureCoreService.FindUsers

        Try


            Dim sUserName As String = oFindUsersRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFUKey", iUserId)
            CommonFunctions.CheckSecurityToken(oFindUsersRequest.WCFSecurityToken)
            Dim oResponse As New FindUsersResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindUsersRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindUsersRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindUsersResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindUsersRequest.BranchCode
            oImpRequest.UserName = oFindUsersRequest.UserName
            oImpRequest.FullName = oFindUsersRequest.FullName
            oImpRequest.MaxRowsToFetchSpecified = oFindUsersRequest.MaxRowsToFetchSpecified
            If oFindUsersRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = oFindUsersRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            oImpRequest.WCFSecurityToken = If(oFindUsersRequest.WCFSecurityToken.Length > 0, oFindUsersRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.FindUsers(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Users = SAMFunc.GetDeserializedValues(Of List(Of BaseFindUsersResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindUsersResponseTypeUsers", sConvertToTypeName:="BaseFindUsersResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Users = DataTabletoList_FindUsers(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindUsersRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindUsersRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    '''This web method is used to get the Bank Guarantee items 
    '''</summary>
    '''<param name="oGetBankGuaranteeRequest">An object of the class GetBankGuaranteeRequestType</param>
    '''<remarks></remarks> 

    Public Function GetBankGuarantee(ByVal oGetBankGuaranteeRequest As GetBankGuaranteeRequestType) As GetBankGuaranteeResponseType Implements IPureAccountService.GetBankGuarantee

        Try


            Dim sUserName As String = oGetBankGuaranteeRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGBGET", iUserId)
            CommonFunctions.CheckSecurityToken(oGetBankGuaranteeRequest.WCFSecurityToken)
            Dim oResponse As New GetBankGuaranteeResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetBankGuaranteeRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetBankGuaranteeRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetBankGuaranteeResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetBankGuaranteeRequest.BranchCode
            oImpRequest.BGKey = oGetBankGuaranteeRequest.BGKey

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetBankGuarantee(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                With oResponse
                    .BankGuarantee = New BaseBankGuaranteeItemType
                    .BankGuarantee.BankBranch = oImpResponse.BankGuarantee.BankBranch
                    .BankGuarantee.BGKey = oImpResponse.BankGuarantee.BGKey
                    .BankGuarantee.BankNameKey = oImpResponse.BankGuarantee.BankNameKey
                    .BankGuarantee.BankNameDesc = oImpResponse.BankGuarantee.BankNameDesc
                    .BankGuarantee.BankGuaranteeRef = oImpResponse.BankGuarantee.BankGuaranteeRef
                    .BankGuarantee.BGCurrencyCode = oImpResponse.BankGuarantee.BGCurrencyCode
                    .BankGuarantee.BGTimeStamp = oImpResponse.BankGuarantee.BGTimeStamp
                    .BankGuarantee.LimitAvailable = oImpResponse.BankGuarantee.LimitAvailable
                    .BankGuarantee.BGStatusCode = oImpResponse.BankGuarantee.BGStatusCode
                    .BankGuarantee.IssueDate = oImpResponse.BankGuarantee.IssueDate
                    .BankGuarantee.BGLimit = oImpResponse.BankGuarantee.BGLimit

                    .BankGuarantee.ExpiryDate = oImpResponse.BankGuarantee.ExpiryDate
                    .BankGuarantee.IsPolicyLock = oImpResponse.BankGuarantee.IsPolicyLock
                    .BankGuarantee.IsDeleted = oImpResponse.BankGuarantee.IsDeleted
                    .BankGuarantee.PartyKey = oImpResponse.BankGuarantee.PartyKey
                    .BankGuarantee.CustodyBranchCode = oImpResponse.BankGuarantee.CustodyBranchCode

                    '****************************************************************************************
                    'Note:Currency Conversion related codes are commented below since it may be used in future
                    'and it is told by Gaurav
                    '****************************************************************************************
                    '.BankGuarantee.CurrencyRates = New BaseCoreCurrencyExchangeRatesType
                    'With .BankGuarantee.CurrencyRates
                    '    If (oImpResponse.BankGuarantee.CurrencyRates.AccountCurrencyRate <> 0) Then
                    '        .AccountCurrencyRateSpecified = True
                    '    End If
                    '    .AccountCurrencyRate = oImpResponse.BankGuarantee.CurrencyRates.AccountCurrencyRate
                    '    .AccountCurrencyDesc = oImpResponse.BankGuarantee.CurrencyRates.AccountCurrencyDesc
                    '    .AccountCurrencyKey = oImpResponse.BankGuarantee.CurrencyRates.AccountCurrencyKey
                    '    .AccountCurrencyDate = oImpResponse.BankGuarantee.CurrencyRates.AccountCurrencyDate
                    '    .BaseCurrencyDate = oImpResponse.BankGuarantee.CurrencyRates.BaseCurrencyDate
                    '    .BaseCurrencyDesc = oImpResponse.BankGuarantee.CurrencyRates.BaseCurrencyDesc
                    '    .BaseCurrencyKey = oImpResponse.BankGuarantee.CurrencyRates.BaseCurrencyKey
                    '    If (oImpResponse.BankGuarantee.CurrencyRates.BaseCurrencyRate <> 0) Then
                    '        .BaseCurrencyRateSpecified = True
                    '    End If
                    '    .BaseCurrencyRate = oImpResponse.BankGuarantee.CurrencyRates.BaseCurrencyRate
                    '    .SystemCurrencyDate = oImpResponse.BankGuarantee.CurrencyRates.SystemCurrencyDate
                    '    If (oImpResponse.BankGuarantee.CurrencyRates.ExchangeRateOverrideReasonKey <> 0) Then
                    '        .ExchangeRateOverrideReasonKeySpecified = True
                    '    End If
                    '    .ExchangeRateOverrideReasonKey = oImpResponse.BankGuarantee.CurrencyRates.ExchangeRateOverrideReasonKey
                    '    If (oImpResponse.BankGuarantee.CurrencyRates.SystemCurrrencyRate <> 0) Then
                    '        .SystemCurrrencyRateSpecified = True
                    '    End If
                    '    .SystemCurrrencyRate = oImpResponse.BankGuarantee.CurrencyRates.SystemCurrrencyRate
                    '    .SystemCurrencyKey = oImpResponse.BankGuarantee.CurrencyRates.SystemCurrencyKey
                    '    .TransactionCurrencyDesc = oImpResponse.BankGuarantee.CurrencyRates.TransactionCurrencyDesc
                    '    .TransactionCurrencyKey = oImpResponse.BankGuarantee.CurrencyRates.TransactionCurrencyKey


                    If (oImpResponse.BankGuarantee.Products IsNot Nothing) Then
                        oResponse.BankGuarantee.Products = oImpResponse.BankGuarantee.Products.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseBankGuaranteeItemTypeProducts, BaseBankGuaranteeItemTypeProducts)(AddressOf CommonFunctions.ToServiceBaseBankGuaranteeItemTypeProductsList))

                    End If
                    If (oImpResponse.BankGuarantee.Branches IsNot Nothing) Then
                        oResponse.BankGuarantee.Branches = oImpResponse.BankGuarantee.Branches.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseBankGuaranteeItemTypeBranches, BaseBankGuaranteeItemTypeBranches)(AddressOf CommonFunctions.ToServiceBankGuaranteeItemTypeBranchesList))

                    End If
                End With

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetBankGuaranteeRequest))
                oResponse = Nothing
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetBankGuaranteeRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetCoverNoteBook(ByVal GetCoverNoteBookRequest As GetCoverNoteBookRequestType) As GetCoverNoteBookResponseType Implements IPureCoreService.GetCoverNoteBook

        Try

            Dim sUserName As String = GetCoverNoteBookRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            'GetIdentity(sUserName, iAgentKey, iUserId)

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCNBKey", iUserId)
            CommonFunctions.CheckSecurityToken(GetCoverNoteBookRequest.WCFSecurityToken)
            Dim oResponse As New GetCoverNoteBookResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetCoverNoteBookRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCoverNoteBookRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCoverNoteBookResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetCoverNoteBookRequest.BranchCode
            oImpRequest.CoverNoteBookKey = GetCoverNoteBookRequest.CoverNoteBookKey
            oImpRequest.WCFSecurityToken = If(GetCoverNoteBookRequest.WCFSecurityToken.Length > 0, GetCoverNoteBookRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetCoverNoteBook(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.BookNumber = oImpResponse.BookNumber
                oResponse.StartNumber = oImpResponse.StartNumber
                oResponse.EndNumber = oImpResponse.EndNumber
                oResponse.EffectiveDate = oImpResponse.EffectiveDate
                oResponse.EffectiveDateSpecified = oImpResponse.EffectiveDateSpecified
                oResponse.AgentKey = oImpResponse.AgentKey
                oResponse.AgentKeySpecified = oImpResponse.AgentKeySpecified
                oResponse.AgentName = oImpResponse.AgentName
                oResponse.CoverNoteBranchKey = oImpResponse.CoverNoteBranchKey
                oResponse.CoverNoteBranchKeySpecified = oImpResponse.CoverNoteBranchKeySpecified
                oResponse.CoverNoteBranchCode = oImpResponse.CoverNoteBranchCode
                oResponse.CoverNoteBookStatusKey = oImpResponse.CoverNoteBookStatusKey
                oResponse.CoverNoteBookStatusKeySpecified = oImpResponse.CoverNoteBookStatusKeySpecified
                oResponse.CoverNoteBookStatusCode = oImpResponse.CoverNoteBookStatusCode
                oResponse.DateCreated = oImpResponse.DateCreated
                oResponse.CoverNoteBookTimestamp = oImpResponse.CoverNoteBookTimestamp

                'oResponse.CoverNoteBookProducts = SAMFunc.GetDeserializedValues(Of List(Of BaseGetCoverNoteBookResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultCoverNoteBookProductsDataset, sFromTypeName:="BaseGetCoverNoteBookResponseTypeCoverNoteBookProducts", sConvertToTypeName:="BaseGetCoverNoteBookResponseTypeRow")
                If oImpResponse.ResultDataCoverNoteBookProduct IsNot Nothing AndAlso oImpResponse.ResultDataCoverNoteBookProduct.Tables(0) IsNot Nothing Then
                    oResponse.CoverNoteBookProducts = DataTabletoList_GetCoverNoteBook(oImpResponse.ResultDataCoverNoteBookProduct.Tables(0))
                End If

                'oResponse.CoverNoteSheets = SAMFunc.GetDeserializedValues(Of List(Of BaseGetCoverNoteBookResponseTypeRow1))(elmResultDataSet:=oImpResponse.ResultCoverNoteSheetsDataset, sFromTypeName:="BaseGetCoverNoteBookResponseTypeCoverNoteSheets", sConvertToTypeName:="BaseGetCoverNoteBookResponseTypeRow1")
                If oImpResponse.ResultDataCoverNoteSheets IsNot Nothing AndAlso oImpResponse.ResultDataCoverNoteSheets.Tables(0) IsNot Nothing Then
                    oResponse.CoverNoteSheets = DataTabletoList_GetCoverNoteBook1(oImpResponse.ResultDataCoverNoteSheets.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetCoverNoteBookRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetCoverNoteBookRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetCoverNoteSheet(ByVal GetCoverNoteSheetRequest As GetCoverNoteSheetRequestType) As GetCoverNoteSheetResponseType Implements IPureCoreService.GetCoverNoteSheet

        Try


            Dim sUserName As String = GetCoverNoteSheetRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCNSKey", iUserId)
            CommonFunctions.CheckSecurityToken(GetCoverNoteSheetRequest.WCFSecurityToken)
            Dim oResponse As New GetCoverNoteSheetResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetCoverNoteSheetRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCoverNoteSheetRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCoverNoteSheetResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetCoverNoteSheetRequest.BranchCode
            oImpRequest.CoverNoteBookKey = GetCoverNoteSheetRequest.CoverNoteBookKey
            oImpRequest.CoverNoteSheetNumber = GetCoverNoteSheetRequest.CoverNoteSheetNumber

            Try

                ' Call the implementation method

                oImpResponse = oBusiness.GetCoverNoteSheet(oImpRequest)
                oResponse.AssignedDate = oImpResponse.AssignedDate
                oResponse.AssignedDateSpecified = oImpResponse.AssignedDateSpecified
                oResponse.Code = oImpResponse.Code
                oResponse.Comments = oImpResponse.Comments
                oResponse.CoverNoteBookTimestamp = oImpResponse.CoverNoteBookTimestamp
                oResponse.CoverNoteSheetKey = oImpResponse.CoverNoteSheetKey
                oResponse.CoverNoteSheetNumber = oImpResponse.CoverNoteSheetNumber
                oResponse.CoverNoteStatusKey = oImpResponse.CoverNoteStatusKey
                oResponse.Description = oImpResponse.Description
                oResponse.InsuranceFileCnt = oImpResponse.InsuranceFileCnt
                oResponse.InsuranceFileCntSpecified = oImpResponse.InsuranceFileCntSpecified
                oResponse.InsuranceRef = oImpResponse.InsuranceRef

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                'CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetCoverNoteSheetRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetCoverNoteSheetRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetCurrenciesByBranch(ByVal GetCurrenciesByBranchRequest As GetCurrenciesByBranchRequestType) As GetCurrenciesByBranchResponseType Implements IPureCoreService.GetCurrenciesByBranch, IPureAccountService.GetCurrenciesByBranch, IPureClaimService.GetCurrenciesByBranch, IPurePartyService.GetCurrenciesByBranch, IPurePolicyService.GetCurrenciesByBranch

        Try
            Dim sUserName As String = GetCurrenciesByBranchRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCurByB", iUserId)
            CommonFunctions.CheckSecurityToken(GetCurrenciesByBranchRequest.WCFSecurityToken)
            Dim oResponse As New GetCurrenciesByBranchResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetCurrenciesByBranchRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCurrenciesByBranchRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCurrenciesByBranchResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetCurrenciesByBranchRequest.BranchCode
            oImpRequest.UserName = sUserName
            oImpRequest.WCFSecurityToken = If(GetCurrenciesByBranchRequest.WCFSecurityToken.Length > 0, GetCurrenciesByBranchRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetCurrenciesByBranch(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.BaseCurrencyCode = oImpResponse.BaseCurrencyCode
                oResponse.BaseCurrencyDescription = oImpResponse.BaseCurrencyDescription

                'oResponse.Currencies = SAMFunc.GetDeserializedValues(Of List(Of BaseGetCurrenciesByBranchResponseTypeRow))(elmResultDataSet:=oImpResponse.Currencies, sFromTypeName:="BaseGetCurrenciesByBranchResponseTypeCurrencies", sConvertToTypeName:="BaseGetCurrenciesByBranchResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Currencies = DataTabletoList_GetCurrenciesByBranch(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetCurrenciesByBranchRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetCurrenciesByBranchRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used to Get Currency Exchange Rates
    ''' </summary>  
    ''' <param name="oGetCurrencyExchangeRatesRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetCurrencyExchangeRatesRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetCurrencyExchangeRatesResponseType</returns>  

    Public Function GetCurrencyExchangeRates(ByVal oGetCurrencyExchangeRatesRequest As GetCurrencyExchangeRatesRequestType) As GetCurrencyExchangeRatesResponseType Implements IPureAccountService.GetCurrencyExchangeRates, IPureClaimService.GetCurrencyExchangeRates

        Try


            Dim sUserName As String = oGetCurrencyExchangeRatesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCEXKey", iUserId)
            CommonFunctions.CheckSecurityToken(oGetCurrencyExchangeRatesRequest.WCFSecurityToken)
            Dim oResponse As New GetCurrencyExchangeRatesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetCurrencyExchangeRatesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCurrencyExchangeRatesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCurrencyExchangeRatesResponseType = Nothing

            ' Pass the values to the implementation request structure and do remember to map the specified fields also
            With oGetCurrencyExchangeRatesRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.AccountCode = .AccountCode
                oImpRequest.CurrencyAmountUnRoundedSpecified = .CurrencyAmountUnRoundedSpecified
                oImpRequest.CurrencyAmountUnRounded = .CurrencyAmountUnRounded
                oImpRequest.Mode = .Mode
                oImpRequest.TransactionCurrencyCode = .TransactionCurrencyCode
                oImpRequest.ClaimKey = .ClaimKey
                oImpRequest.IsManualJournal = .IsManualJournal
            End With
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetCurrencyExchangeRates(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.AccountAmount = oImpResponse.AccountAmount
                oResponse.AccountAmountUnrounded = oImpResponse.AccountAmountUnrounded
                oResponse.BaseAmount = oImpResponse.BaseAmount
                oResponse.BaseAmountUnrounded = oImpResponse.BaseAmountUnrounded
                oResponse.SystemAmount = oImpResponse.SystemAmount
                oResponse.SystemAmountUnrounded = oImpResponse.SystemAmountUnrounded

                oResponse.CurrencyRates = New BaseCoreCurrencyExchangeRatesType

                oResponse.CurrencyRates.AccountCurrencyDesc = oImpResponse.CurrencyRates.AccountCurrencyDesc
                oResponse.CurrencyRates.AccountCurrencyKey = oImpResponse.CurrencyRates.AccountCurrencyKey
                oResponse.CurrencyRates.AccountCurrencyDate = oImpResponse.CurrencyRates.AccountCurrencyDate
                oResponse.CurrencyRates.AccountCurrencyRate = oImpResponse.CurrencyRates.AccountCurrencyRate
                oResponse.CurrencyRates.AccountCurrencyRateSpecified = oImpResponse.CurrencyRates.AccountCurrencyRateSpecified
                oResponse.CurrencyRates.BaseCurrencyDate = oImpResponse.CurrencyRates.BaseCurrencyDate
                oResponse.CurrencyRates.BaseCurrencyDesc = oImpResponse.CurrencyRates.BaseCurrencyDesc
                oResponse.CurrencyRates.BaseCurrencyKey = oImpResponse.CurrencyRates.BaseCurrencyKey
                oResponse.CurrencyRates.BaseCurrencyRate = oImpResponse.CurrencyRates.BaseCurrencyRate
                oResponse.CurrencyRates.BaseCurrencyRateSpecified = oImpResponse.CurrencyRates.BaseCurrencyRateSpecified
                oResponse.CurrencyRates.ExchangeRateOverrideReasonKey = oImpResponse.CurrencyRates.ExchangeRateOverrideReasonKey
                oResponse.CurrencyRates.ExchangeRateOverrideReasonKeySpecified = oImpResponse.CurrencyRates.ExchangeRateOverrideReasonKeySpecified
                oResponse.CurrencyRates.SystemCurrencyDate = oImpResponse.CurrencyRates.SystemCurrencyDate
                oResponse.CurrencyRates.SystemCurrencyKey = oImpResponse.CurrencyRates.SystemCurrencyKey
                oResponse.CurrencyRates.SystemCurrrencyRate = oImpResponse.CurrencyRates.SystemCurrrencyRate
                oResponse.CurrencyRates.SystemCurrrencyRateSpecified = oImpResponse.CurrencyRates.SystemCurrrencyRateSpecified
                oResponse.CurrencyRates.TransactionCurrencyKey = oImpResponse.CurrencyRates.TransactionCurrencyKey
                oResponse.CurrencyRates.TransactionCurrencyDesc = oImpResponse.CurrencyRates.TransactionCurrencyDesc
                oResponse.CurrencyRates.TransactionCurrrencyRate = oImpResponse.CurrencyRates.TransactionCurrrencyRate
                oResponse.CurrencyRates.TransactionCurrrencyRateSpecified = oImpResponse.CurrencyRates.TransactionCurrrencyRateSpecified

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetCurrencyExchangeRatesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetCurrencyExchangeRatesRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used to Get Currency to Currency Exchange Rate
    ''' </summary>  
    ''' <param name="oGetCurrencyToCurrencyExchangeRateRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetCurrencyToCurrencyExchangeRateRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetCurrencyToCurrencyExchangeRateResponseType</returns>  

    Public Function GetCurrencyToCurrencyExchangeRate(ByVal oGetCurrencyToCurrencyExchangeRateRequest As GetCurrencyToCurrencyExchangeRateRequestType) As GetCurrencyToCurrencyExchangeRateResponseType Implements IPureCoreService.GetCurrencyToCurrencyExchangeRate

        Try


            Dim sUserName As String = oGetCurrencyToCurrencyExchangeRateRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCEXKey", iUserId)
            CommonFunctions.CheckSecurityToken(oGetCurrencyToCurrencyExchangeRateRequest.WCFSecurityToken)

            Dim ore As New GetAccountDetailsResponseType
            Dim oResponse As New GetCurrencyToCurrencyExchangeRateResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetCurrencyToCurrencyExchangeRateRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCurrencyToCurrencyExchangeRateRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCurrencyToCurrencyExchangeRateResponseType = Nothing

            ' Pass the values to the implementation request structure and do remember to map the specified fields also
            With oGetCurrencyToCurrencyExchangeRateRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.CurrencyAmountUnRoundedSpecified = .CurrencyAmountUnRoundedSpecified
                oImpRequest.CurrencyAmountUnRounded = .CurrencyAmountUnRounded
                oImpRequest.CurrencyCodeFrom = .CurrencyCodeFrom
                oImpRequest.CurrencyCodeTo = .CurrencyCodeTo
            End With
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetCurrencyToCurrencyExchangesRate(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.BaseAmount = oImpResponse.BaseAmount
                oResponse.ConversionRate = oImpResponse.ConversionRate
                oResponse.CurrencyAmount = oImpResponse.CurrencyAmount

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetCurrencyToCurrencyExchangeRateRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetCurrencyToCurrencyExchangeRateRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetDMEFolder(ByVal GetDMEFolderRequest As GetDMEFolderRequestType) As GetDMEFolderResponseType Implements IPureCoreService.GetDMEFolder, IPureClaimService.GetDMEFolder, IPurePolicyService.GetDMEFolder
        Try


            Dim sUserName As String = GetDMEFolderRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMDMEFOLD", iUserId)
            CommonFunctions.CheckSecurityToken(GetDMEFolderRequest.WCFSecurityToken)
            Dim oResponse As New GetDMEFolderResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetDMEFolderRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetDMEFolderRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetDMEFolderResponseType = Nothing

            oImpRequest.BranchCode = GetDMEFolderRequest.BranchCode
            oImpRequest.FolderNum = GetDMEFolderRequest.FolderNum
            oImpRequest.FolderPath = GetDMEFolderRequest.FolderPath
            oImpRequest.IncludeFiles = GetDMEFolderRequest.IncludeFiles


            Try
                ' Call the implementation method

                oImpResponse = oBusiness.GetDMEFolder(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.ParentNum = oImpResponse.ParentNum



                If (oImpResponse.SubFolders IsNot Nothing) Then
                    oResponse.SubFolders = oImpResponse.SubFolders.ToList().ConvertAll(
                    New Converter(Of BaseImplementationTypes.BaseDMEFolderType, BaseDMEFolderType)(AddressOf CommonFunctions.ToBaseDMEFolderType))
                End If


                If oImpResponse.Documents IsNot Nothing Then
                    oResponse.Documents = oImpResponse.Documents.ToList().ConvertAll(
                    New Converter(Of BaseImplementationTypes.BaseDocumentType, BaseDocumentType)(AddressOf CommonFunctions.ToBaseDocumentType))
                End If


            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetDMEFolderRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetDMEFolderRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetDatasetDefinition(ByVal GetDatasetDefinitionRequest As GetDatasetDefinitionRequestType) As GetDatasetDefinitionResponseType Implements IPureCoreService.GetDatasetDefinition, IPurePolicyService.GetDatasetDefinition, IPureClaimService.GetDatasetDefinition

        Try


            Dim sUserName As String = GetDatasetDefinitionRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer


            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGDSDef", iUserId)
            CommonFunctions.CheckSecurityToken(GetDatasetDefinitionRequest.WCFSecurityToken)
            Dim oResponse As New GetDatasetDefinitionResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetDatasetDefinitionRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetDatasetDefinitionRequestType
            'Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetDatasetDefinitionResponseType = Nothing
            Dim oImpResponse As BaseImplementationTypes.BaseGetDatasetDefinitionResponseType = Nothing
            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetDatasetDefinitionRequest.BranchCode
            oImpRequest.UserName = sUserName
            oImpRequest.DataModelCode = GetDatasetDefinitionRequest.DataModelCode

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetDatasetDefinition(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.XMLDatasetDefinition = oImpResponse.XMLDatasetDefinition

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetDatasetDefinitionRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetDatasetDefinitionRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetDatasetSchema(ByVal GetDatasetSchemaRequest As GetDatasetSchemaRequestType) As GetDatasetSchemaResponseType Implements IPureCoreService.GetDatasetSchema

        Try
            Dim sUserName As String = GetDatasetSchemaRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGDSSch", iUserId)
            CommonFunctions.CheckSecurityToken(GetDatasetSchemaRequest.WCFSecurityToken)

            Dim oResponse As New GetDatasetSchemaResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetDatasetSchemaRequest.BranchCode)

            Dim oImpRequest As New BaseImplementationTypes.BaseGetDatasetSchemaRequestType
            Dim oImpResponse As BaseImplementationTypes.BaseGetDatasetSchemaResponseType = Nothing

            oImpRequest.BranchCode = GetDatasetSchemaRequest.BranchCode
            oImpRequest.DataModelCode = GetDatasetSchemaRequest.DataModelCode

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetDatasetSchema(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Return details
                oResponse.DatasetSchema = oImpResponse.DatasetSchema

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetDatasetSchemaRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetDatasetSchemaRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetList(ByVal GetListRequest As GetListRequestType) As GetListResponseType Implements IPureCoreService.GetList, IPureAccountService.GetList, IPurePolicyService.GetList, IPurePartyService.GetList, IPureClaimService.GetList

        Try


            Dim sUserName As String = GetListRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGLst", iUserId)
            CommonFunctions.CheckSecurityToken(GetListRequest.WCFSecurityToken)
            Dim oResponse As New GetListResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetListRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetListRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetListResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetListRequest.BranchCode
            oImpRequest.ListCode = GetListRequest.ListCode
            oImpRequest.ListType = CType([Enum].ToObject(GetType(STSListType), GetListRequest.ListType), BaseImplementationTypes.STSListType)
            oImpRequest.UserName = sUserName

            oImpRequest.ExcludeDeletedRecords = GetListRequest.ExcludeDeletedRecords
            oImpRequest.ExcludeEffectiveDate = GetListRequest.ExcludeEffectiveDate
            oImpRequest.ParentFieldName = GetListRequest.ParentFieldName
            oImpRequest.ParentFieldValue = GetListRequest.ParentFieldValue
            oImpRequest.ParentFieldValueSpecified = GetListRequest.ParentFieldValueSpecified


            oImpRequest.FilterType = CType([Enum].ToObject(GetType(GetListFilterType), GetListRequest.FilterType), BaseImplementationTypes.GetListFilterType)
            oImpRequest.FilterValue = GetListRequest.FilterValue

            oImpRequest.VersionSpecified = GetListRequest.VersionSpecified
            oImpRequest.Version = GetListRequest.Version
            oImpRequest.EffectiveDateSpecified = GetListRequest.EffectiveDateSpecified
            oImpRequest.EffectiveDate = GetListRequest.EffectiveDate
            oImpRequest.WCFSecurityToken = If(GetListRequest.WCFSecurityToken.Length > 0, GetListRequest.WCFSecurityToken, "WCFSecurityToken")


            '-----Convert Seperate Lists to one Collection of Core.BaseFilterOptions-----'
            '-----Easier to handle as one object instead of three -----'
            Dim GetListWhereClause As New System.Collections.ObjectModel.Collection(Of Core.BaseListFilterOptions)

            'Ensure that none of the lists are empty
            If GetListRequest.WhereColumnName IsNot Nothing And GetListRequest.WhereOperator IsNot Nothing And GetListRequest.WhereValue IsNot Nothing Then
                Dim count As Integer = GetListRequest.WhereColumnName.Count
                For i As Integer = 0 To count - 1
                    GetListWhereClause.Add(New Core.BaseListFilterOptions With {
                        .ColumnName = GetListRequest.WhereColumnName(i),
                        .FilterOperator = GetListRequest.WhereOperator(i),
                        .FilterValue = GetListRequest.WhereValue(i)
                    })
                Next
            End If
            '---End Statement---'

            oImpRequest.WhereClause = GetListWhereClause

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetList(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.List = SAMFunc.GetDeserializedValues(Of List(Of BaseGetListResponseTypeRow))(elmResultDataSet:=oImpResponse.ListItems, sFromTypeName:="BaseGetListResponseTypeList", sConvertToTypeName:="BaseGetListResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.List = DataTabletoList_GetList(oImpResponse.ResultData.Tables(0))
                End If

                oResponse.AdditionalResult = oImpResponse.AdditionalResult

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetListRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetListRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetListSPUICCS(ByVal GetListRequest As GetListRequestType) As GetListResponseType Implements IPureCoreService.GetListSPUICCS, IPureAccountService.GetListSPUICCS, IPurePolicyService.GetListSPUICCS, IPurePartyService.GetListSPUICCS, IPureClaimService.GetListSPUICCS

        Try


            Dim sUserName As String = GetListRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGLst", iUserId)

            Dim oResponse As New GetListResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetListRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetListRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetListResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetListRequest.BranchCode
            oImpRequest.ListCode = GetListRequest.ListCode
            oImpRequest.ListType = CType([Enum].ToObject(GetType(STSListType), GetListRequest.ListType), BaseImplementationTypes.STSListType)
            oImpRequest.UserName = sUserName

            oImpRequest.ExcludeDeletedRecords = GetListRequest.ExcludeDeletedRecords
            oImpRequest.ExcludeEffectiveDate = GetListRequest.ExcludeEffectiveDate
            oImpRequest.ParentFieldName = GetListRequest.ParentFieldName
            oImpRequest.ParentFieldValue = GetListRequest.ParentFieldValue
            oImpRequest.ParentFieldValueSpecified = GetListRequest.ParentFieldValueSpecified


            oImpRequest.FilterType = CType([Enum].ToObject(GetType(GetListFilterType), GetListRequest.FilterType), BaseImplementationTypes.GetListFilterType)
            oImpRequest.FilterValue = GetListRequest.FilterValue

            oImpRequest.VersionSpecified = GetListRequest.VersionSpecified
            oImpRequest.Version = GetListRequest.Version
            oImpRequest.EffectiveDateSpecified = GetListRequest.EffectiveDateSpecified
            oImpRequest.EffectiveDate = GetListRequest.EffectiveDate


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetListSPUICCS(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.List = SAMFunc.GetDeserializedValues(Of List(Of BaseGetListResponseTypeRow))(elmResultDataSet:=oImpResponse.ListItems, sFromTypeName:="BaseGetListResponseTypeList", sConvertToTypeName:="BaseGetListResponseTypeRow")

                oResponse.AdditionalResult = oImpResponse.AdditionalResult

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function

    Public Function GetProductByAgent(ByVal GetProductByAgentRequest As GetProductByAgentRequestType) As GetProductByAgentResponseType Implements IPurePolicyService.GetProductByAgent

        Try


            Dim sUserName As String = GetProductByAgentRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGPrdByA", iUserId)
            CommonFunctions.CheckSecurityToken(GetProductByAgentRequest.WCFSecurityToken)
            Dim oResponse As New GetProductByAgentResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetProductByAgentRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetProductByAgentRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetProductByAgentResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetProductByAgentRequest.BranchCode
            oImpRequest.UserName = sUserName
            oImpRequest.WCFSecurityToken = If(GetProductByAgentRequest.WCFSecurityToken.Length > 0, GetProductByAgentRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetProductByAgent(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Products = SAMFunc.GetDeserializedValues(Of List(Of BaseGetProductByAgentResponseTypeRow))(elmResultDataSet:=oImpResponse.Products, sFromTypeName:="BaseGetProductByAgentResponseTypeProducts", sConvertToTypeName:="BaseGetProductByAgentResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Products = DataTabletoList_GetProductByAgent(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetProductByAgentRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetProductByAgentRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used to get claim workflow options for a product.
    ''' </summary>  
    ''' <param name="oGetProductClaimsWorkflowOptionsRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetProductClaimsWorkflowOptionsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetProductClaimsWorkflowOptionsResponseType</returns>  
    Public Function GetProductClaimsWorkflowOptions(ByVal oGetProductClaimsWorkflowOptionsRequest As GetProductClaimsWorkflowOptionsRequestType) As GetProductClaimsWorkflowOptionsResponseType Implements IPureClaimService.GetProductClaimsWorkflowOptions
        Try

            Dim sUserName As String = oGetProductClaimsWorkflowOptionsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGPCWFO", iUserId)
            CommonFunctions.CheckSecurityToken(oGetProductClaimsWorkflowOptionsRequest.WCFSecurityToken)
            Dim oResponse As New GetProductClaimsWorkflowOptionsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetProductClaimsWorkflowOptionsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetProductClaimsWorkflowOptionsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetProductClaimsWorkflowOptionsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetProductClaimsWorkflowOptionsRequest.BranchCode
            oImpRequest.ProductCode = oGetProductClaimsWorkflowOptionsRequest.ProductCode
            oImpRequest.ClaimProcessType = CType(oGetProductClaimsWorkflowOptionsRequest.ClaimProcessType, BaseImplementationTypes.ClaimProcessType)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetProductClaimsWorkflowOptions(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                With oImpResponse
                    oResponse.CashPaymentProcess = .CashPaymentProcess
                    oResponse.CheckDeferredReinsurance = .CheckDeferredReinsurance
                    oResponse.CheckUnpaidStatus = .CheckUnpaidStatus
                    oResponse.ClaimNotificationDocMessage = .ClaimNotificationDocMessage
                    oResponse.ClaimPaymentDocMessage = .ClaimPaymentDocMessage
                    oResponse.ClaimPaymentProcess = .ClaimPaymentProcess
                    oResponse.DescriptionForChangeInPayment = .DescriptionForChangeInPayment
                    oResponse.DescriptionForChangeInReserve = .DescriptionForChangeInReserve
                    oResponse.ExternalClaimHandling = .ExternalClaimHandling
                    oResponse.FastTrackClaims = .FastTrackClaims
                    oResponse.GenerateClaimNotificationDoc = .GenerateClaimNotificationDoc
                    oResponse.GenerateClaimPaymentDoc = .GenerateClaimPaymentDoc
                    oResponse.MakeFurtherPayments = .MakeFurtherPayments
                    oResponse.ReinsurancePayment = .ReinsurancePayment
                    oResponse.ReinsuranceRecovery = .ReinsuranceRecovery
                    oResponse.SalvageRecovery = .SalvageRecovery
                    oResponse.ThirdPartyRecovery = .ThirdPartyRecovery
                End With

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetProductClaimsWorkflowOptionsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetProductClaimsWorkflowOptionsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetProductRiskOptionValue(ByVal oGetProductRiskOptionValueRequest As ProductRiskOptionValueRequestType) As ProductRiskOptionValueResponseType Implements IPureCoreService.GetProductRiskOptionValue, IPurePolicyService.GetProductRiskOptionValue, IPureClaimService.GetProductRiskOptionValue

        Try
            Dim sUserName As String = oGetProductRiskOptionValueRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            Dim oBusiness As CoreSAMBusiness
            'Assign appropriate key
            If oGetProductRiskOptionValueRequest.ActionType = ProductConfigActionType.ProductRiskMaintenance Then
                CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
                CommonFunctions.CheckAuthority("SAMGPROV", iUserId)
                CommonFunctions.CheckSecurityToken(oGetProductRiskOptionValueRequest.WCFSecurityToken)
                oBusiness = New CoreSAMBusiness(sUserName, oGetProductRiskOptionValueRequest.BranchCode)
            ElseIf oGetProductRiskOptionValueRequest.ActionType = ProductConfigActionType.RiskTypeMaintenance Then
                oBusiness = New CoreSAMBusiness()
            End If

            Dim oResponse As New ProductRiskOptionValueResponseType


            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ProductRiskOptionValueRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ProductRiskOptionValueResponseType = Nothing

            'Pass the values to the implementation request structure as below for all input parameters 
            oImpRequest.BranchCode = oGetProductRiskOptionValueRequest.BranchCode
            oImpRequest.ProducRiskOption = CType([Enum].ToObject(GetType(ProductRiskOptions), oGetProductRiskOptionValueRequest.ProducRiskOption), BaseImplementationTypes.ProductRiskOptions)
            oImpRequest.ProducRiskOptionSpecified = oGetProductRiskOptionValueRequest.ProducRiskOptionSpecified
            oImpRequest.ProductCode = oGetProductRiskOptionValueRequest.ProductCode
            oImpRequest.RiskTypeOption = CType([Enum].ToObject(GetType(RiskTypeOptions), oGetProductRiskOptionValueRequest.RiskTypeOption), BaseImplementationTypes.RiskTypeOptions)
            oImpRequest.RiskTypeOptionSpecified = oGetProductRiskOptionValueRequest.RiskTypeOptionSpecified
            oImpRequest.RiskTypeCode = oGetProductRiskOptionValueRequest.RiskTypeCode
            oImpRequest.ActionType = CType([Enum].ToObject(GetType(ProductConfigActionType), oGetProductRiskOptionValueRequest.ActionType), BaseImplementationTypes.ProductConfigActionType)

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetProductRiskOptionValue(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.ProductRiskOptionValue = oImpResponse.ProductRiskOptionValue

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetProductRiskOptionValueRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetProductRiskOptionValueRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetRatingSectionByRiskType(ByVal oGetRatingSectionByRiskTypeRequest As GetRatingSectionByRiskTypeRequestType) As GetRatingSectionByRiskTypeResponseType Implements IPurePolicyService.GetRatingSectionByRiskType
        Try



            Dim sUserName As String = oGetRatingSectionByRiskTypeRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRatDet", iUserId)
            CommonFunctions.CheckSecurityToken(oGetRatingSectionByRiskTypeRequest.WCFSecurityToken)
            Dim oGetRatingSectionByRiskTypeResponse As New GetRatingSectionByRiskTypeResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetRatingSectionByRiskTypeRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRatingSectionByRiskTypeRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRatingSectionByRiskTypeResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetRatingSectionByRiskTypeRequest.BranchCode
            oImpRequest.RiskTypeCode = oGetRatingSectionByRiskTypeRequest.RiskTypeCode
            oImpRequest.WCFSecurityToken = If(oGetRatingSectionByRiskTypeRequest.WCFSecurityToken.Length > 0, oGetRatingSectionByRiskTypeRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetRatingSectionByRiskType(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oGetRatingSectionByRiskTypeResponse.RatingSections = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRatingSectionByRiskTypeResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetRatingSectionByRiskTypeResponseTypeRatingSections", sConvertToTypeName:="BaseGetRatingSectionByRiskTypeResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oGetRatingSectionByRiskTypeResponse.RatingSections = DataTabletoList_GetRatingSectionByRiskType(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oGetRatingSectionByRiskTypeResponse, ex, CommonFunctions.CreateDictionary(oGetRatingSectionByRiskTypeRequest))
            End Try

            Return oGetRatingSectionByRiskTypeResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetRatingSectionByRiskTypeRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to get the Rating Section Types details by passing the InsuranceFileKey 
    ''' and also the response object is being passed to the correct messaging format as dataset.
    '''</summary>
    '''<returns> object of type GetRatingSectionTypesResponseType as dataset </returns>
    '''<remarks></remarks> 
    Public Function GetRatingSectionTypes(ByVal oGetRatingSectionTypesRequest As GetRatingSectionTypesRequestType) As GetRatingSectionTypesResponseType Implements IPurePolicyService.GetRatingSectionTypes

        Try



            Dim sUserName As String = oGetRatingSectionTypesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRatDet", iUserId)
            CommonFunctions.CheckSecurityToken(oGetRatingSectionTypesRequest.WCFSecurityToken)
            Dim oResponse As New GetRatingSectionTypesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetRatingSectionTypesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRatingSectionTypesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRatingSectionTypesResponseType = Nothing

            'Pass the values to the implementation request structure as below for all input parameters 
            oImpRequest.BranchCode = oGetRatingSectionTypesRequest.BranchCode
            oImpRequest.InsuranceFIleKey = oGetRatingSectionTypesRequest.InsuranceFIleKey
            oImpRequest.WCFSecurityToken = If(oGetRatingSectionTypesRequest.WCFSecurityToken.Length > 0, oGetRatingSectionTypesRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetRatingSectionTypes(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' oResponse.RatingSectionTypes = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRatingSectionTypesResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetRatingSectionTypesResponseTypeRatingSectionTypes", sConvertToTypeName:="BaseGetRatingSectionTypesResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.RatingSectionTypes = DataTabletoList_GetRatingSectionTypes(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetRatingSectionTypesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetRatingSectionTypesRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetReport(ByVal oGetReportRequest As GetReportRequestType) As GetReportResponseType Implements IPureCoreService.GetReport

        Try

            'TODO:-Create the task under the task group reports



            Dim sUserName As String = oGetReportRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGETREPT", iUserId)
            CommonFunctions.CheckSecurityToken(oGetReportRequest.WCFSecurityToken)
            Dim oResponse As New GetReportResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetReportRequest.BranchCode)
            Dim iCount As Integer = 0
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetReportRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetReportResponseType = Nothing

            'TODO:-Assign the frontend values

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetReportRequest.BranchCode
            oImpRequest.ReportName = oGetReportRequest.ReportName
            If (oGetReportRequest.FormatType = DocumentFormatType.EXCEL) Then
                oImpRequest.FormatType = BaseImplementationTypes.DocumentFormatType.EXCEL
            ElseIf (oGetReportRequest.FormatType = DocumentFormatType.HTML) Then
                oImpRequest.FormatType = BaseImplementationTypes.DocumentFormatType.HTML
            ElseIf (oGetReportRequest.FormatType = DocumentFormatType.None) Then
                oImpRequest.FormatType = BaseImplementationTypes.DocumentFormatType.None
            ElseIf (oGetReportRequest.FormatType = DocumentFormatType.PDF) Then
                oImpRequest.FormatType = BaseImplementationTypes.DocumentFormatType.PDF
            End If

            If oGetReportRequest.Parameters IsNot Nothing Then
                With oGetReportRequest
                    ReDim Preserve oImpRequest.Parameters(.Parameters.Count - 1)
                    For iCount = 0 To .Parameters.Count - 1
                        oImpRequest.Parameters(iCount) = New BaseImplementationTypes.BaseGetReportRequestTypeParameters
                        oImpRequest.Parameters(iCount).ParamName = .Parameters(iCount).ParamName
                        If (.Parameters(iCount).ParamName = "user_id") Then
                            oImpRequest.Parameters(iCount).ParamValue = iUserId
                        Else
                            oImpRequest.Parameters(iCount).ParamValue = .Parameters(iCount).ParamValue
                        End If
                    Next
                End With
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetReport(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'TODO:-Return back the Byte array to front End

                oResponse.ReportDocument = oImpResponse.ReportDocument
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetReportRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetReportRequest))

        End Try
    End Function


    ''' <summary>
    ''' This Web method is used to get the sharepoint file list
    ''' </summary>
    ''' <param name="GetSharepointFileListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSharepointFileList(ByVal GetSharepointFileListRequest As GetSharepointFileListRequestType) As GetSharepointFileListResponseType Implements IPureCoreService.GetSharepointFileList

        Try
            Dim sUserName As String = GetSharepointFileListRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGDocLst", iUserId)
            CommonFunctions.CheckSecurityToken(GetSharepointFileListRequest.WCFSecurityToken)

            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetSharepointFileListRequest.BranchCode)
            Dim oResponse As New GetSharepointFileListResponseType

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetSharepointFileListRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetSharepointFileListResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetSharepointFileListRequest.BranchCode
            oImpRequest.PartyShortname = SAMFunc.NothingToString(GetSharepointFileListRequest.PartyShortname)
            oImpRequest.PolicyNumber = SAMFunc.NothingToString(GetSharepointFileListRequest.PolicyNumber)
            oImpRequest.ClaimNumber = SAMFunc.NothingToString(GetSharepointFileListRequest.ClaimNumber)
            oImpRequest.FolderPath = SAMFunc.NothingToString(GetSharepointFileListRequest.FolderPath)
            oImpRequest.CreateFolder = GetSharepointFileListRequest.CreateFolder
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetSharepointFileList(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.FolderPath = oImpResponse.FolderPath

                If Not oImpResponse.ItemList Is Nothing Then

                    oResponse.ItemList = oImpResponse.ItemList.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseGetSharepointFileListResponseTypeItemList, ArrayOfBaseGetSharepointFileListResponseTypeItemListBaseGetSharepointFileListResponseTypeItemList)(AddressOf CommonFunctions.ToServiceSharepointFileList))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetSharepointFileListRequest))
            End Try

            Return oResponse

        Catch ex As Exception

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This Web method is used to Get the Task Group tasks by passing the Branchcode,TaskGroupKey and EffectiveDate as  request type objects
    ''' and also the response object is being returned as XML element.
    '''</summary>
    '''<param name="oGetTaskGroupTasksRequest" type="GetTaskGroupTasksRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTaskGroupTasksResponseType</returns>  
    '''<remarks></remarks>
    Public Function GetTaskGroupTasks(ByVal oGetTaskGroupTasksRequest As GetTaskGroupTasksRequestType) As GetTaskGroupTasksResponseType Implements IPureCoreService.GetTaskGroupTasks

        Try

            Dim sUserName As String = oGetTaskGroupTasksRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGUG", iUserId)
            CommonFunctions.CheckSecurityToken(oGetTaskGroupTasksRequest.WCFSecurityToken)
            Dim oResponse As New GetTaskGroupTasksResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetTaskGroupTasksRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetTaskGroupTasksRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetTaskGroupTasksResponseType = Nothing

            'India : Pass the values to the implementation request structure, i.e.
            oImpRequest.BranchCode = oGetTaskGroupTasksRequest.BranchCode
            oImpRequest.TaskGroupKey = oGetTaskGroupTasksRequest.TaskGroupKey
            oImpRequest.EffectiveDate = oGetTaskGroupTasksRequest.EffectiveDate
            oImpRequest.WCFSecurityToken = If(oGetTaskGroupTasksRequest.WCFSecurityToken.Length > 0, oGetTaskGroupTasksRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetTaskGroupTasks(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.TaskGroupTasks = SAMFunc.GetDeserializedValues(Of List(Of BaseGetTaskGroupTasksResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetTaskGroupTasksResponseTypeTaskGroupTasks", sConvertToTypeName:="BaseGetTaskGroupTasksResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.TaskGroupTasks = DataTabletoList_GetTaskGroupTasks(oImpResponse.ResultData.Tables(0))
                End If
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetTaskGroupTasksRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetTaskGroupTasksRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to Get the Task Groups by passing the Branchcode as  request type objects
    ''' and also the response object is being returned as XML element.
    '''</summary>
    '''<param name="oGetTaskGroupsRequest" type="GetTaskGroupsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTaskGroupsResponseType</returns>  
    '''<remarks></remarks>
    Public Function GetTaskGroups(ByVal oGetTaskGroupsRequest As GetTaskGroupsRequestType) As GetTaskGroupsResponseType Implements IPureCoreService.GetTaskGroups

        Try



            Dim sUserName As String = oGetTaskGroupsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGUG", iUserId)
            CommonFunctions.CheckSecurityToken(oGetTaskGroupsRequest.WCFSecurityToken)
            Dim oResponse As New GetTaskGroupsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetTaskGroupsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetTaskGroupsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetTaskGroupsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetTaskGroupsRequest.BranchCode
            oImpRequest.WCFSecurityToken = If(oGetTaskGroupsRequest.WCFSecurityToken.Length > 0, oGetTaskGroupsRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetTaskGroups(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.TaskGroups = SAMFunc.GetDeserializedValues(Of List(Of BaseGetTaskGroupsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetTaskGroupsResponseTypeTaskGroups", sConvertToTypeName:="BaseGetTaskGroupsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.TaskGroups = DataTabletoList_GetTaskGroups(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetTaskGroupsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetTaskGroupsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' </summary>
    ''' <param name="oGetUserDetailsRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserDetails(oGetUserDetailsRequestType As GetUserDetailsRequestType) As GetUserDetailsResponseType Implements IPureCoreService.GetUserDetails, IPureAccountService.GetUserDetails, IPureClaimService.GetUserDetails, IPurePartyService.GetUserDetails, IPurePolicyService.GetUserDetails
        Dim oGetUserDetailsResponseType As New GetUserDetailsResponseType

        Try
            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()
            If oGetUserDetailsRequestType.IsSSO Then
                oGetUserDetailsRequestType.LoginUserName = oBusiness.GetAndValidateDescriptionById("PmUser", "username", "sso_preferred_username", oGetUserDetailsRequestType.LoginUserName)
            End If

            CommonFunctions.CheckSecurityToken(oGetUserDetailsRequestType.WCFSecurityToken)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetUserDetailsRequestType
            Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.GetUserDetailsResponseType

            'Get PureBO user name corresponding to windows login username
            If Not String.IsNullOrEmpty(oGetUserDetailsRequestType.LoginUserName) Then
                sUserName = oGetUserDetailsRequestType.LoginUserName
                If oGetUserDetailsRequestType.LoginUserName.Contains("\") Then
                    Dim SAMErrorCollection As New SAMErrorCollection
                    Try
                        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_LoginUserName")
                                cmd.AddInParameter("@usernameIN", SqlDbType.NVarChar, 255).Value = Cast.NullIfDefault(sUserName)
                                cmd.AddOutParameter("@usernameOUT", SqlDbType.NVarChar, 255)

                                Dim ds As DataSet = con.ExecuteDataSet(cmd, "UserDetails")

                                If IsDBNull(cmd.Parameters("@usernameOUT").Value) Then
                                    Throw New AuthorisationException(sUserName)
                                Else
                                    sUserName = Cast.ToString(cmd.Parameters("@usernameOUT").Value, String.Empty)
                                End If
                            End Using
                        End Using
                    Catch ex As Exception
                        CommonFunctions.BusinessLayerBoundary(oImpResponse, oGetUserDetailsResponseType, ex, CommonFunctions.CreateDictionary(oGetUserDetailsRequestType))
                    End Try
                End If
            End If

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGUsrDet", iUserId)

            Try

                ' Call the implementation method
                oImpRequest.Username = sUserName
                oImpRequest.UserId = iUserId
                oImpRequest.AgentKey = iAgentKey

                oImpResponse = DirectCast(oBusiness.GetUserDetails(oImpRequest), Internal.BaseGetUserDetailsResponseType)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oGetUserDetailsResponseType.EmailAddress = oImpResponse.EmailAddress
                oGetUserDetailsResponseType.FullUsername = oImpResponse.FullUsername
                If oImpResponse.LastLogin <> Date.MinValue Then
                    oGetUserDetailsResponseType.LastLogin = oImpResponse.LastLogin
                    oGetUserDetailsResponseType.LastLoginSpecified = True
                Else
                    oGetUserDetailsResponseType.LastLoginSpecified = False
                End If
                If oImpResponse.PasswordChangeDate <> Date.MinValue Then
                    oGetUserDetailsResponseType.PasswordChangeDate = oImpResponse.PasswordChangeDate
                    oGetUserDetailsResponseType.PasswordChangeDateSpecified = True
                Else
                    oGetUserDetailsResponseType.PasswordChangeDateSpecified = False
                End If
                oGetUserDetailsResponseType.PartyKey = oImpResponse.PartyKey
                oGetUserDetailsResponseType.PartyKeySpecified = True
                oGetUserDetailsResponseType.PartyName = oImpResponse.PartyName
                oGetUserDetailsResponseType.PartyShortName = oImpResponse.PartyShortName
                oGetUserDetailsResponseType.PartyType = oImpResponse.PartyType
                oGetUserDetailsResponseType.ConsolidatedAgentCommission = oImpResponse.ConsolidatedAgentCommission
                oGetUserDetailsResponseType.PureUsername = oImpResponse.PureUsername
                oGetUserDetailsResponseType.UserId = oImpRequest.UserId

                oGetUserDetailsResponseType.SourceList = oImpResponse.SourceList.ToList().ConvertAll(
                    New Converter(Of BaseImplementationTypes.BaseBranchType, BaseBranchType)(AddressOf CommonFunctions.ToServiceSourceList))
                ' oGetUserDetailsResponseType.UserGroups = SAMFunc.GetDeserializedValues(Of List(Of BaseGetUserDetailsResponseTypeRow))(elmResultDataSet:=oImpResponse.UserGroups, sFromTypeName:="BaseGetUserDetailsResponseTypeUserGroups", sConvertToTypeName:="BaseGetUserDetailsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oGetUserDetailsResponseType.UserGroups = DataTabletoList_GetUserDetails(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oGetUserDetailsResponseType, ex, CommonFunctions.CreateDictionary(oGetUserDetailsRequestType))
            End Try

            Return oGetUserDetailsResponseType

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetUserDetailsRequestType))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to Get the Task Group tasks by passing the Branchcode,UserGroupKey and EffectiveDate as  request type objects
    ''' and also the response object is being returned as XML element.
    '''</summary>
    '''<param name="oGetUserGroupTaskGroupsRequest" type="GetUserGroupTaskGroupsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetUserGroupTaskGroupsResponseType</returns>  
    '''<remarks></remarks>
    Public Function GetUserGroupTaskGroups(ByVal oGetUserGroupTaskGroupsRequest As GetUserGroupTaskGroupsRequestType) As GetUserGroupTaskGroupsResponseType Implements IPureCoreService.GetUserGroupTaskGroups

        Try
            'Assign appropriate key, i.e.


            Dim sUserName As String = oGetUserGroupTaskGroupsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGUGTG", iUserId)
            CommonFunctions.CheckSecurityToken(oGetUserGroupTaskGroupsRequest.WCFSecurityToken)
            Dim oResponse As New GetUserGroupTaskGroupsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetUserGroupTaskGroupsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetUserGroupTaskGroupsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetUserGroupTaskGroupsResponseType = Nothing

            'India : Pass the values to the implementation request structure, i.e.
            oImpRequest.BranchCode = oGetUserGroupTaskGroupsRequest.BranchCode
            oImpRequest.EffectiveDate = oGetUserGroupTaskGroupsRequest.EffectiveDate
            'Replacing UserGroupKey instead of UserGroupCode as discussion with Rahul (Vijayakumar  Ramasamy)
            oImpRequest.UserGroupCode = oGetUserGroupTaskGroupsRequest.UserGroupCode
            oImpRequest.WCFSecurityToken = If(oGetUserGroupTaskGroupsRequest.WCFSecurityToken.Length > 0, oGetUserGroupTaskGroupsRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetUserGroupTaskGroups(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.TaskGroups = SAMFunc.GetDeserializedValues(Of List(Of BaseGetUserGroupTaskGroupsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetUserGroupTaskGroupsResponseTypeTaskGroups", sConvertToTypeName:="BaseGetUserGroupTaskGroupsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.TaskGroups = DataTabletoList_GetUserGroupTaskGroups(oImpResponse.ResultData.Tables(0))
                End If
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetUserGroupTaskGroupsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetUserGroupTaskGroupsRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>  
    ''' This web services method is used to fetche user group's users.
    ''' </summary>  
    ''' <param name="oGetUserGroupUsersRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetUserGroupUsersRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetUserGroupUsersResponseType</returns>  
    Public Function GetUserGroupUsers(ByVal oGetUserGroupUsersRequest As GetUserGroupUsersRequestType) As GetUserGroupUsersResponseType Implements IPureCoreService.GetUserGroupUsers, IPureAccountService.GetUserGroupUsers, IPureClaimService.GetUserGroupUsers, IPurePartyService.GetUserGroupUsers, IPurePolicyService.GetUserGroupUsers

        Try


            Dim sUserName As String = oGetUserGroupUsersRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGUG", iUserId)
            CommonFunctions.CheckSecurityToken(oGetUserGroupUsersRequest.WCFSecurityToken)
            Dim oResponse As New GetUserGroupUsersResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetUserGroupUsersRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetUserGroupUsersRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetUserGroupUsersResponseType = Nothing

            With oGetUserGroupUsersRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.UserGroupCode = .UserGroupCode
                oImpRequest.EffectiveDate = .EffectiveDate
                oImpRequest.RestrictUserListSpecified = .RestrictUserListSpecified
                oImpRequest.RestrictUserList = .RestrictUserList
                oImpRequest.AgentKey = iAgentKey
                oImpRequest.WCFSecurityToken = If(.WCFSecurityToken.Length > 0, .WCFSecurityToken, "WCFSecurityToken")

            End With

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetUserGroupUsers(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.EmailAddress = oImpResponse.EmailAddress
                'oResponse.UserGroupUsers = SAMFunc.GetDeserializedValues(Of List(Of BaseGetUserGroupUsersResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetUserGroupUsersResponseTypeUserGroupUsers", sConvertToTypeName:="BaseGetUserGroupUsersResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.UserGroupUsers = DataTabletoList_GetUserGroupUsers(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetUserGroupUsersRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetUserGroupUsersRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetUserGroups(ByVal GetUserGroupsRequest As GetUserGroupsRequestType) As GetUserGroupsResponseType Implements IPureCoreService.GetUserGroups, IPureAccountService.GetUserGroups, IPureClaimService.GetUserGroups, IPurePartyService.GetUserGroups, IPurePolicyService.GetUserGroups, IPureSecurityService.GetUserGroups

        Try

            Dim sUserName As String = GetUserGroupsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGUG", iUserId)
            ' CommonFunctions.CheckSecurityToken(GetUserGroupsRequest.WCFSecurityToken)
            Dim oResponse As New GetUserGroupsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetUserGroupsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetUserGroupsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetUserGroupsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetUserGroupsRequest.BranchCode

            If GetUserGroupsRequest.ShowForCurrentUserOnly = True Then
                oImpRequest.Username = GetUserGroupsRequest.LoginUserName
            End If
            '  oImpRequest.WCFSecurityToken = If(GetUserGroupsRequest.WCFSecurityToken.Length > 0, GetUserGroupsRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetUserGroups(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.UserGroups = SAMFunc.GetDeserializedValues(Of List(Of BaseGetUserGroupsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetUserGroupsResponseTypeUserGroups", sConvertToTypeName:="BaseGetUserGroupsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.UserGroups = DataTabletoList_GetUserGroups(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetUserGroupsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetUserGroupsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This   web service  method  for Get UserGroups by Task
    '''<param name="oGetUserGroupsbyTaskRequestType" type="GetUserGroupsbyTaskRequestType"></param>   
    '''<returns>oGetUserGroupsbyTaskResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetUserGroupsbyTask(ByVal oGetUserGroupsbyTaskRequest As GetUserGroupsbyTaskRequestType) As GetUserGroupsbyTaskResponseType Implements IPureCoreService.GetUserGroupsbyTask
        Try

            ' CheckAuthority("SAMGUGT")
            Dim sUserName As String = oGetUserGroupsbyTaskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oGetUserGroupsbyTaskRequest.WCFSecurityToken)
            Dim oResponse As New GetUserGroupsbyTaskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetUserGroupsbyTaskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetUserGroupsbyTaskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetUserGroupsbyTaskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetUserGroupsbyTaskRequest.BranchCode
            oImpRequest.TaskGroupCode = oGetUserGroupsbyTaskRequest.TaskGroupCode
            oImpRequest.WCFSecurityToken = If(oGetUserGroupsbyTaskRequest.WCFSecurityToken.Length > 0, oGetUserGroupsbyTaskRequest.WCFSecurityToken, "WCFSecurityToken")


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetUserGroupsbyTask(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.UserGroups = SAMFunc.GetDeserializedValues(Of List(Of BaseGetUserGroupsbyTaskResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetUserGroupsbyTaskResponseTypeUserGroups", sConvertToTypeName:="BaseGetUserGroupsbyTaskResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.UserGroups = DataTabletoList_GetUserGroupsbyTask(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetUserGroupsbyTaskRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetUserGroupsbyTaskRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This Web method is used to get the View Task details by passing the BranchCode and TaskInstanceKey as request type objects
    ''' and also the response object is being passed to the correct messaging format.
    '''</summary>
    '''<param name="oGetWmTaskRequest" type="GetWmTaskRequestType"></param>   
    '''<remarks></remarks>
    Public Function GetWmTask(ByVal oGetWmTaskRequest As GetWmTaskRequestType) As GetWmTaskResponseType Implements IPureCoreService.GetWmTask

        Try

            Dim sUserName As String = oGetWmTaskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGWMTKey", iUserId)
            CommonFunctions.CheckSecurityToken(oGetWmTaskRequest.WCFSecurityToken)
            Dim oResponse As New GetWmTaskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetWmTaskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetWmTaskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetWmTaskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetWmTaskRequest.BranchCode
            oImpRequest.TaskInstanceKey = oGetWmTaskRequest.TaskInstanceKey
            oImpRequest.WCFSecurityToken = If(oGetWmTaskRequest.WCFSecurityToken.Length > 0, oGetWmTaskRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetWmTask(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.KeyData = SAMFunc.GetDeserializedValues(Of List(Of BaseGetWmTaskResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetWmTaskResponseTypeKeyData", sConvertToTypeName:="BaseGetWmTaskResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.KeyData = DataTabletoList_GetWmTask(oImpResponse.ResultData.Tables(0))
                End If

                'India : Set the response values here, e.g.
                oResponse.TaskInstanceKey = oImpResponse.TaskInstanceKey
                oResponse.TaskGroupKey = oImpResponse.TaskGroupKey
                oResponse.TaskGroupCode = oImpResponse.TaskGroupCode
                oResponse.TaskKey = oImpResponse.TaskKey
                oResponse.TaskCode = oImpResponse.TaskCode
                oResponse.Customer = oImpResponse.Customer
                oResponse.DueDate = oImpResponse.DueDate
                oResponse.UserGroupKey = oImpResponse.UserGroupKey
                oResponse.UserGroupCode = oImpResponse.UserGroupCode
                oResponse.UserKey = oImpResponse.UserKey
                oResponse.UserCode = oImpResponse.UserCode
                oResponse.Description = oImpResponse.Description
                oResponse.TaskStatusKey = oImpResponse.TaskStatusKey
                oResponse.TaskStatusKeySpecified = oImpResponse.TaskStatusKeySpecified
                oResponse.IsUrgent = oImpResponse.IsUrgent
                oResponse.IsUrgentSpecified = oImpResponse.IsUrgentSpecified
                oResponse.IsTaskReview = oImpResponse.IsTaskReview
                oResponse.IsTaskReviewSpecified = oImpResponse.IsTaskReviewSpecified
                oResponse.CreatedByKey = oImpResponse.CreatedByKey
                oResponse.CreatedByKeySpecified = oImpResponse.CreatedByKeySpecified
                oResponse.DateCreated = oImpResponse.DateCreated
                oResponse.DateCreatedSpecified = oImpResponse.DateCreatedSpecified
                oResponse.ModifiedByKey = oImpResponse.ModifiedByKey
                oResponse.ModifiedByKeySpecified = oImpResponse.ModifiedByKeySpecified
                oResponse.LastModified = oImpResponse.LastModified
                oResponse.LastModifiedSpecified = oImpResponse.LastModifiedSpecified
                oResponse.Isvisible = oImpResponse.Isvisible
                oResponse.IsvisibleSpecified = oImpResponse.IsvisibleSpecified
                oResponse.WorkflowInformation = oImpResponse.WorkflowInformation
                oResponse.TaskTimestamp = oImpResponse.TaskTimestamp
                oResponse.CreatedByUser = oImpResponse.CreatedByUser
                oResponse.ModifiedByUser = oImpResponse.ModifiedByUser
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetWmTaskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetWmTaskRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This is webservice method for  Get WorkManager Task Log
    '''<param name="GetWmTaskLogRequest" type="GetWmTaskLogRequestType"></param>   
    '''</summary>
    '''<remarks></remarks>  

    Public Function GetWmTaskLog(ByVal GetWmTaskLogRequest As GetWmTaskLogRequestType) As GetWmTaskLogResponseType Implements IPureCoreService.GetWmTaskLog

        Try


            Dim sUserName As String = GetWmTaskLogRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGWTLKey", iUserId)
            CommonFunctions.CheckSecurityToken(GetWmTaskLogRequest.WCFSecurityToken)
            Dim oResponse As New GetWmTaskLogResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetWmTaskLogRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetWmTaskLogRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetWmTaskLogResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetWmTaskLogRequest.BranchCode
            oImpRequest.TaskInstanceKey = GetWmTaskLogRequest.TaskInstanceKey
            oImpRequest.WCFSecurityToken = If(GetWmTaskLogRequest.WCFSecurityToken.Length > 0, GetWmTaskLogRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetWmTaskLog(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.TaskLog = SAMFunc.GetDeserializedValues(Of List(Of BaseGetWmTaskLogResponseTypeTaskLogRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetWmTaskLogResponseTypeTaskLog", sConvertToTypeName:="BaseGetWmTaskLogResponseTypeTaskLogRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.TaskLog = DataTabletoList_GetWmTaskLogTaskLog(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetWmTaskLogRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetWmTaskLogRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This method is used to Get The Work Manager Tasks
    ''' </summary>  
    ''' <param name="GetWorkManagerScheduledTasksRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetWorkManagerScheduledTasksRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetWorkManagerScheduledTasksResponseType</returns>  
    Public Function GetWorkManagerScheduledTasks(ByVal GetWorkManagerScheduledTasksRequest As GetWorkManagerScheduledTasksRequestType) As GetWorkManagerScheduledTasksResponseType Implements IPureCoreService.GetWorkManagerScheduledTasks, IPureAccountService.GetWorkManagerScheduledTasks, IPureClaimService.GetWorkManagerScheduledTasks, IPurePartyService.GetWorkManagerScheduledTasks, IPurePolicyService.GetWorkManagerScheduledTasks

        Try



            Dim sUserName As String = GetWorkManagerScheduledTasksRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAWMTKey", iUserId)
            CommonFunctions.CheckSecurityToken(GetWorkManagerScheduledTasksRequest.WCFSecurityToken)
            Dim oResponse As New GetWorkManagerScheduledTasksResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetWorkManagerScheduledTasksRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetWorkManagerScheduledTasksRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetWorkManagerScheduledTasksResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetWorkManagerScheduledTasksRequest.BranchCode

            oImpRequest.TaskStatusKey = CType(GetWorkManagerScheduledTasksRequest.TaskStatusKey, BaseImplementationTypes.TaskStatus)
            oImpRequest.TaskStatusKeySpecified = GetWorkManagerScheduledTasksRequest.TaskStatusKeySpecified
            oImpRequest.UserGroupCODE = GetWorkManagerScheduledTasksRequest.UserGroupCODE
            oImpRequest.UserCODE = GetWorkManagerScheduledTasksRequest.UserCODE
            oImpRequest.ShowSystemKEY = CType(GetWorkManagerScheduledTasksRequest.ShowSystemKEY, BaseImplementationTypes.ShowType)
            oImpRequest.ShowSystemKEYSpecified = GetWorkManagerScheduledTasksRequest.ShowSystemKEYSpecified

            oImpRequest.Date = CType(GetWorkManagerScheduledTasksRequest.Date, BaseImplementationTypes.DateRange)

            oImpRequest.DateSpecified = GetWorkManagerScheduledTasksRequest.DateSpecified
            oImpRequest.PartyKey = GetWorkManagerScheduledTasksRequest.PartyKey
            oImpRequest.ReferenceNumber = GetWorkManagerScheduledTasksRequest.ReferenceNumber
            oImpRequest.WCFSecurityToken = If(GetWorkManagerScheduledTasksRequest.WCFSecurityToken.Length > 0, GetWorkManagerScheduledTasksRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetWorkManagerScheduledTasks(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Tasks = SAMFunc.GetDeserializedValues(Of List(Of BaseGetWorkManagerScheduledTasksResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetWorkManagerScheduledTasksResponseTypeTasks", sConvertToTypeName:="BaseGetWorkManagerScheduledTasksResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Tasks = DataTabletoList_GetWorkManagerScheduledTasks(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetWorkManagerScheduledTasksRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetWorkManagerScheduledTasksRequest))
            Return Nothing
        End Try

    End Function

    Public Function ReAssignMultipleWmTasks(ByVal ReAssignMultipleWmTasksRequest As ReAssignMultipleWmTasksRequestType) As ReAssignMultipleWmTasksResponseType Implements IPureCoreService.ReAssignMultipleWmTasks

        Try
            Dim sUserName As String = ReAssignMultipleWmTasksRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMMAWMKey", iUserId)
            CommonFunctions.CheckSecurityToken(ReAssignMultipleWmTasksRequest.WCFSecurityToken)
            Dim oResponse As New ReAssignMultipleWmTasksResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, ReAssignMultipleWmTasksRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ReAssignMultipleWmTasksRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ReAssignMultipleWmTasksResponseType = Nothing

            ' Pass the values to the implementation request structure in a loop for ReAssignMultipleWmTasksRequest collection

            oImpRequest.BranchCode = ReAssignMultipleWmTasksRequest.BranchCode
            Dim iCount As Integer = 0
            oImpRequest.Tasks = New BaseImplementationTypes.BaseReAssignMultipleWmTasksRequestTypeTasks
            ReDim Preserve oImpRequest.Tasks.Row(ReAssignMultipleWmTasksRequest.Tasks.Count - 1)
            For iCount = 0 To ReAssignMultipleWmTasksRequest.Tasks.Count - 1
                oImpRequest.Tasks.Row(iCount) = New BaseImplementationTypes.BaseReAssignMultipleWmTasksRequestTypeTasksRow
                oImpRequest.Tasks.Row(iCount).TaskInstanceKey = ReAssignMultipleWmTasksRequest.Tasks(iCount).TaskInstanceKey
                oImpRequest.Tasks.Row(iCount).UserGroupCode = ReAssignMultipleWmTasksRequest.Tasks(iCount).UserGroupCode
                oImpRequest.Tasks.Row(iCount).UserCode = ReAssignMultipleWmTasksRequest.Tasks(iCount).UserCode
            Next
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ReAssignMultipleWmTasks(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(ReAssignMultipleWmTasksRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(ReAssignMultipleWmTasksRequest))
            Return Nothing
        End Try

    End Function

    Public Function UpdateBankGuaranteeConditionally(ByVal oUpdateBankGuaranteeConditionallyRequest As UpdateBankGuaranteeConditionallyRequestType) As UpdateBankGuaranteeConditionallyResponseType Implements IPureAccountService.UpdateBankGuaranteeConditionally

        Try

            Dim sUserName As String = oUpdateBankGuaranteeConditionallyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMBGConl", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateBankGuaranteeConditionallyRequest.WCFSecurityToken)
            Dim oResponse As New UpdateBankGuaranteeConditionallyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateBankGuaranteeConditionallyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateBankGuaranteeConditionallyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateBankGuaranteeConditionallyResponseType = Nothing

            'Map all the specified fields if they are available            
            oImpRequest.BranchCode = oUpdateBankGuaranteeConditionallyRequest.BranchCode

            If oUpdateBankGuaranteeConditionallyRequest IsNot Nothing And oUpdateBankGuaranteeConditionallyRequest.Item IsNot Nothing Then

                For iCount As Integer = 0 To oUpdateBankGuaranteeConditionallyRequest.Item.Count - 1
                    ReDim Preserve oImpRequest.Item(iCount)

                    oImpRequest.Item(iCount) = New BaseImplementationTypes.BaseUpdateBankGuaranteeConditionallyRequestTypeItem

                    oImpRequest.Item(iCount).ActionType = CType([Enum].ToObject(GetType(BGUCActionType), oUpdateBankGuaranteeConditionallyRequest.Item(iCount).ActionType), BaseImplementationTypes.BGUCActionType)
                    oImpRequest.Item(iCount).BGKey = oUpdateBankGuaranteeConditionallyRequest.Item(iCount).BGKey
                    oImpRequest.Item(iCount).BGRef = oUpdateBankGuaranteeConditionallyRequest.Item(iCount).BGRef
                    oImpRequest.Item(iCount).InvokedDate = oUpdateBankGuaranteeConditionallyRequest.Item(iCount).InvokedDate
                    oImpRequest.Item(iCount).InvokedDateSpecified = oUpdateBankGuaranteeConditionallyRequest.Item(iCount).InvokedDateSpecified
                    oImpRequest.Item(iCount).IsDeleted = oUpdateBankGuaranteeConditionallyRequest.Item(iCount).IsDeleted
                    oImpRequest.Item(iCount).IsDeletedSpecified = oUpdateBankGuaranteeConditionallyRequest.Item(iCount).IsDeletedSpecified
                Next
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.UpdateBankGuaranteeConditionally(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateBankGuaranteeConditionallyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateBankGuaranteeConditionallyRequest))
            Return Nothing
        End Try

    End Function

    Public Function UpdateBankGuarantee(ByVal oUpdateBankGuaranteeRequest As UpdateBankGuaranteeRequestType) As UpdateBankGuaranteeResponseType Implements IPureAccountService.UpdateBankGuarantee

        Try


            Dim sUserName As String = oUpdateBankGuaranteeRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMBGUPD", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateBankGuaranteeRequest.WCFSecurityToken)
            Dim oResponse As New UpdateBankGuaranteeResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateBankGuaranteeRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateBankGuaranteeRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateBankGuaranteeResponseType = Nothing

            Dim iCount As Integer = 0
            oImpRequest.BranchCode = oUpdateBankGuaranteeRequest.BranchCode
            If (oUpdateBankGuaranteeRequest.BankGuarantee IsNot Nothing) Then
                If (oUpdateBankGuaranteeRequest.BankGuarantee.Count > 0) Then
                    ReDim Preserve oImpRequest.BankGuarantee(oUpdateBankGuaranteeRequest.BankGuarantee.Count - 1)
                    For iCount = 0 To oUpdateBankGuaranteeRequest.BankGuarantee.Count - 1
                        With oUpdateBankGuaranteeRequest
                            oImpRequest.BankGuarantee(iCount) = New BaseImplementationTypes.BaseBankGuaranteeItemType
                            oImpRequest.BankGuarantee(iCount).BGKey = .BankGuarantee(iCount).BGKey
                            oImpRequest.BankGuarantee(iCount).BankNameKey = .BankGuarantee(iCount).BankNameKey
                            oImpRequest.BankGuarantee(iCount).BankBranch = .BankGuarantee(iCount).BankBranch
                            oImpRequest.BankGuarantee(iCount).BGCurrencyCode = .BankGuarantee(iCount).BGCurrencyCode
                            oImpRequest.BankGuarantee(iCount).PartyKey = .BankGuarantee(iCount).PartyKey
                            oImpRequest.BankGuarantee(iCount).BankGuaranteeRef = .BankGuarantee(iCount).BankGuaranteeRef
                            oImpRequest.BankGuarantee(iCount).CustodyBranchCode = .BankGuarantee(iCount).CustodyBranchCode
                            oImpRequest.BankGuarantee(iCount).BGLimit = .BankGuarantee(iCount).BGLimit
                            oImpRequest.BankGuarantee(iCount).LimitAvailable = .BankGuarantee(iCount).LimitAvailable
                            oImpRequest.BankGuarantee(iCount).ExpiryDate = .BankGuarantee(iCount).ExpiryDate
                            oImpRequest.BankGuarantee(iCount).IssueDate = .BankGuarantee(iCount).IssueDate
                            oImpRequest.BankGuarantee(iCount).IsPolicyLock = .BankGuarantee(iCount).IsPolicyLock
                            oImpRequest.BankGuarantee(iCount).BGStatusCode = .BankGuarantee(iCount).BGStatusCode
                            oImpRequest.BankGuarantee(iCount).BGTimeStamp = .BankGuarantee(iCount).BGTimeStamp

                            Dim iCounter As Integer = 0
                            Dim oProduct As BaseBankGuaranteeItemTypeProducts
                            If (.BankGuarantee(iCount).Products IsNot Nothing) Then
                                ReDim Preserve oImpRequest.BankGuarantee(iCount).Products(.BankGuarantee(iCount).Products.Count - 1)
                                For Each oProduct In .BankGuarantee(iCount).Products
                                    oImpRequest.BankGuarantee(iCount).Products(iCounter) = New BaseImplementationTypes.BaseBankGuaranteeItemTypeProducts
                                    oImpRequest.BankGuarantee(iCount).Products(iCounter).ProductCode = oProduct.ProductCode
                                    iCounter = iCounter + 1
                                Next
                            End If

                            Dim oBranch As BaseBankGuaranteeItemTypeBranches
                            If (.BankGuarantee(iCount).Branches IsNot Nothing) Then
                                ReDim Preserve oImpRequest.BankGuarantee(iCount).Branches(.BankGuarantee(iCount).Branches.Count - 1)
                                iCounter = 0
                                For Each oBranch In .BankGuarantee(iCount).Branches
                                    oImpRequest.BankGuarantee(iCount).Branches(iCounter) = New BaseImplementationTypes.BaseBankGuaranteeItemTypeBranches
                                    oImpRequest.BankGuarantee(iCount).Branches(iCounter).BranchCode = oBranch.BranchCode
                                    iCounter = iCounter + 1
                                Next
                            End If
                        End With
                    Next
                End If
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateBankGuarantee(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.BankGuarantee = oImpResponse.BankGuarantee.Row.ToList().ConvertAll(
                        New Converter(Of SFI.SAMForInsuranceV2.BaseUpdateBankGuaranteeResponseTypeBankGuaranteeRow, BaseUpdateBankGuaranteeResponseTypeRow)(AddressOf CommonFunctions.ToServicUpdateBankGuaranteeList))

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateBankGuaranteeRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateBankGuaranteeRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''This Web method is used to update Task group task details 
    '''</summary>
    '''<param name="oUpdateTaskGroupTasksRequest" type="UpdateTaskGroupTasksRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.UpdateTaskGroupTasksResponseType</returns>
    '''<remarks></remarks>
    Public Function UpdateTaskGroupTasks(ByVal oUpdateTaskGroupTasksRequest As UpdateTaskGroupTasksRequestType) As UpdateTaskGroupTasksResponseType Implements IPureCoreService.UpdateTaskGroupTasks

        Try
            'Assign appropriate key


            Dim sUserName As String = oUpdateTaskGroupTasksRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAUTUG", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateTaskGroupTasksRequest.WCFSecurityToken)
            Dim oResponse As New UpdateTaskGroupTasksResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateTaskGroupTasksRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateTaskGroupTasksRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateTaskGroupTasksResponseType = Nothing

            oImpRequest.BranchCode = oUpdateTaskGroupTasksRequest.BranchCode
            oImpRequest.TaskGroupKey = oUpdateTaskGroupTasksRequest.TaskGroupKey
            oImpRequest.TimeStamp = oUpdateTaskGroupTasksRequest.TimeStamp
            If oUpdateTaskGroupTasksRequest.Tasks IsNot Nothing AndAlso oUpdateTaskGroupTasksRequest.Tasks.Count > 0 Then
                Dim iLength As Int32 = oUpdateTaskGroupTasksRequest.Tasks.Count
                oImpRequest.Tasks = New BaseImplementationTypes.BaseUpdateTaskGroupTasksRequestTypeTasks
                ReDim oImpRequest.Tasks.Row(iLength - 1)
                For icount As Integer = 0 To iLength - 1
                    oImpRequest.Tasks.Row(icount) = New BaseImplementationTypes.BaseUpdateTaskGroupTasksRequestTypeTasksRow
                    oImpRequest.Tasks.Row(icount).TaskCode = oUpdateTaskGroupTasksRequest.Tasks(icount).TaskCode
                    oImpRequest.Tasks.Row(icount).DisplaySequence = oUpdateTaskGroupTasksRequest.Tasks(icount).DisplaySequence
                    oImpRequest.Tasks.Row(icount).DisplaySequenceSpecified = oUpdateTaskGroupTasksRequest.Tasks(icount).DisplaySequenceSpecified
                Next
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.UpdateTaskGroupTasks(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Set the response values 
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateTaskGroupTasksRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateTaskGroupTasksRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to updates the usergroup details by passing the Request parameters as request type objects
    ''' and also the response object Timestamp is being returned .
    '''</summary>
    '''<param name="oUpdateUserGroupRequest" type="UpdateUserGroupRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.UpdateUserGroupResponseType</returns>  
    '''<remarks></remarks>

    Public Function UpdateUserGroup(ByVal oUpdateUserGroupRequest As UpdateUserGroupRequestType) As UpdateUserGroupResponseType Implements IPureCoreService.UpdateUserGroup

        Try



            Dim sUserName As String = oUpdateUserGroupRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAUG", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateUserGroupRequest.WCFSecurityToken)
            Dim oResponse As New UpdateUserGroupResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateUserGroupRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateUserGroupRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateUserGroupResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateUserGroupRequest.BranchCode
            oImpRequest.UserGroupKey = oUpdateUserGroupRequest.UserGroupKey
            oImpRequest.IsSysAdmin = oUpdateUserGroupRequest.IsSysAdmin
            oImpRequest.IsDeleted = oUpdateUserGroupRequest.IsDeleted
            oImpRequest.EffectiveDate = oUpdateUserGroupRequest.EffectiveDate
            oImpRequest.Description = oUpdateUserGroupRequest.Description
            oImpRequest.Code = oUpdateUserGroupRequest.Code

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.UpdateUserGroup(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateUserGroupRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateUserGroupRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    '''This Web method is used to update user group user details 
    '''</summary>
    '''<param name="oUpdateUserGroupUsersRequest" type="UpdateUserGroupUsersRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.UpdateUserGroupUsersResponseType</returns>
    '''<remarks></remarks>
    Public Function UpdateUserGroupUsers(ByVal oUpdateUserGroupUsersRequest As UpdateUserGroupUsersRequestType) As UpdateUserGroupUsersResponseType Implements IPureCoreService.UpdateUserGroupUsers

        Try
            'Assign appropriate key


            Dim sUserName As String = oUpdateUserGroupUsersRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAUTUG", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateUserGroupUsersRequest.WCFSecurityToken)
            Dim oResponse As New UpdateUserGroupUsersResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateUserGroupUsersRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateUserGroupUsersRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateUserGroupUsersResponseType = Nothing

            oImpRequest.BranchCode = oUpdateUserGroupUsersRequest.BranchCode
            oImpRequest.UserGroupKey = oUpdateUserGroupUsersRequest.UserGroupKey
            oImpRequest.TimeStamp = oUpdateUserGroupUsersRequest.TimeStamp
            If oUpdateUserGroupUsersRequest.Users IsNot Nothing AndAlso oUpdateUserGroupUsersRequest.Users.Count > 0 Then
                Dim iLength As Int32 = oUpdateUserGroupUsersRequest.Users.Count
                oImpRequest.Users = New BaseImplementationTypes.BaseUpdateUserGroupUsersRequestTypeUsers
                ReDim oImpRequest.Users.Row(iLength - 1)
                For icount As Integer = 0 To iLength - 1
                    oImpRequest.Users.Row(icount) = New BaseImplementationTypes.BaseUpdateUserGroupUsersRequestTypeUsersRow
                    oImpRequest.Users.Row(icount).UserKey = oUpdateUserGroupUsersRequest.Users(icount).UserKey
                    oImpRequest.Users.Row(icount).IsSupervisor = oUpdateUserGroupUsersRequest.Users(icount).IsSupervisor
                    oImpRequest.Users.Row(icount).DisplaySequence = oUpdateUserGroupUsersRequest.Users(icount).DisplaySequence
                    oImpRequest.Users.Row(icount).DisplaySequenceSpecified = oUpdateUserGroupUsersRequest.Users(icount).DisplaySequenceSpecified

                Next
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.UpdateUserGroupUsers(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateUserGroupUsersRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateUserGroupUsersRequest))
            Return Nothing
        End Try

    End Function

#Region "ValidateBankAccountNumber"
    ''' <summary>  
    ''' This web services method is used to Validate BankAccountNumber.
    ''' </summary>  
    ''' <param name="ValidateBankAccountNumber">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.BaseValidateBankAccountNumberRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.BaseValidateBankAccountNumberResponseType</returns>  
    Public Function ValidateBankAccountNumber(ByVal oValidateBankAccountNumberRequest As ValidateBankAccountNumberRequestType) As ValidateBankAccountNumberResponseType Implements IPureAccountService.ValidateBankAccountNumber

        Try

            Dim sUserName As String = oValidateBankAccountNumberRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMVALBANO", iUserId)
            CommonFunctions.CheckSecurityToken(oValidateBankAccountNumberRequest.WCFSecurityToken)

            Dim oResponse As New ValidateBankAccountNumberResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oValidateBankAccountNumberRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ValidateBankAccountNumberRequestType()
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ValidateBankAccountNumberResponseType = Nothing

            ' Convert the incoming interface structures into the implementation structures
            oImpRequest.BankMediaKey = oValidateBankAccountNumberRequest.BankMediaKey
            oImpRequest.BankCountryKey = oValidateBankAccountNumberRequest.BankCountryKey
            oImpRequest.AccountNumber = oValidateBankAccountNumberRequest.AccountNumber
            oImpRequest.BankMediaCode = oValidateBankAccountNumberRequest.BankMediaCode
            oImpRequest.WCFSecurityToken = If(oValidateBankAccountNumberRequest.WCFSecurityToken.Length > 0, oValidateBankAccountNumberRequest.WCFSecurityToken, "WCFSecurityToken")
            oImpRequest.BIC = oValidateBankAccountNumberRequest.BIC
            oImpRequest.IBAN = oValidateBankAccountNumberRequest.IBAN
            oImpRequest.BankName = oValidateBankAccountNumberRequest.BankName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ValidateBankAccountNumber(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.ValidationDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseValidateBankAccountNumberResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseValidateBankAccountNumberResponseTypeValidationDetails", sConvertToTypeName:="BaseValidateBankAccountNumberResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing Then
                    oResponse.ValidationDetails = DataTabletoList_ValidateBankAccountNumber(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oValidateBankAccountNumberRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oValidateBankAccountNumberRequest))
            Return Nothing
        End Try

    End Function
#End Region

    Public Function UpdateWmTask(ByVal oUpdateWmTaskRequest As UpdateWmTaskRequestType) As UpdateWmTaskResponseType Implements IPureCoreService.UpdateWmTask

        Try
            Dim sUserName As String = oUpdateWmTaskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateWmTaskRequest.WCFSecurityToken)
            If oUpdateWmTaskRequest.ActionType = WMActionType.Update Then
                CommonFunctions.CheckAuthority("SAMUWMKey", iUserId)
            ElseIf oUpdateWmTaskRequest.ActionType = WMActionType.Assign Then
                CommonFunctions.CheckAuthority("SAMANWMKey", iUserId)
            ElseIf oUpdateWmTaskRequest.ActionType = WMActionType.Complete Then
                CommonFunctions.CheckAuthority("SAMCMWMKey", iUserId)
            ElseIf oUpdateWmTaskRequest.ActionType = WMActionType.InComplete Then
                CommonFunctions.CheckAuthority("SAMICWMKey", iUserId)
            ElseIf oUpdateWmTaskRequest.ActionType = WMActionType.Run Then

            End If

            Dim oResponse As New UpdateWmTaskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateWmTaskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateWmTaskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateWmTaskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateWmTaskRequest.BranchCode
            oImpRequest.TaskInstanceKey = oUpdateWmTaskRequest.TaskInstanceKey
            oImpRequest.DueDate = oUpdateWmTaskRequest.DueDate
            oImpRequest.Client = oUpdateWmTaskRequest.Client
            oImpRequest.Description = oUpdateWmTaskRequest.Description
            oImpRequest.Urgent = oUpdateWmTaskRequest.Urgent
            oImpRequest.TaskStatusKey = oUpdateWmTaskRequest.TaskStatusKey
            oImpRequest.UserGroupCode = oUpdateWmTaskRequest.UserGroupCode
            oImpRequest.UserCode = oUpdateWmTaskRequest.UserCode
            oImpRequest.TaskTimeStamp = oUpdateWmTaskRequest.TaskTimeStamp

            Dim iCount As Integer = 0
            oImpRequest.KeyData = New BaseImplementationTypes.BaseUpdateWmTaskRequestTypeKeyData

            If (oUpdateWmTaskRequest.KeyData IsNot Nothing) Then
                If (oUpdateWmTaskRequest.KeyData.Count > 0) Then
                    ReDim Preserve oImpRequest.KeyData.Row(oUpdateWmTaskRequest.KeyData.Count - 1)
                    For iCount = 0 To oUpdateWmTaskRequest.KeyData.Count - 1
                        oImpRequest.KeyData.Row(iCount) = New BaseImplementationTypes.BaseUpdateWmTaskRequestTypeKeyDataRow
                        oImpRequest.KeyData.Row(iCount).KeyName = oUpdateWmTaskRequest.KeyData(iCount).KeyName
                        oImpRequest.KeyData.Row(iCount).KeyValue = oUpdateWmTaskRequest.KeyData(iCount).KeyValue
                    Next
                End If
            End If

            Try
                oImpResponse = oBusiness.UpdateWmTask(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.TaskTimeStamp = oImpResponse.TaskTimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateWmTaskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateWmTaskRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This method is used to update user groups
    '''<param name="UpdateTaskGroupsRequest" type="BaseUpdateTaskGroupsRequestType"></param>   
    '''<returns>BaseUpdateTaskGroupsResponseType</returns>
    '''<remarks></remarks>  
    Public Function UpdateTaskGroups(ByVal oUpdateTaskGroupsRequest As UpdateTaskGroupsRequestType) As UpdateTaskGroupsResponseType Implements IPureCoreService.UpdateTaskGroups

        Try


            Dim sUserName As String = oUpdateTaskGroupsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAUTGS", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateTaskGroupsRequest.WCFSecurityToken)
            Dim oResponse As New UpdateTaskGroupsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateTaskGroupsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateTaskGroupsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateTaskGroupsResponseType = Nothing

            'India : Pass the values to the implementation request structure, i.e.
            oImpRequest.BranchCode = oUpdateTaskGroupsRequest.BranchCode

            oImpRequest.TaskGroupKey = oUpdateTaskGroupsRequest.TaskGroupKey
            oImpRequest.CaptionId = oUpdateTaskGroupsRequest.CaptionId
            oImpRequest.Code = oUpdateTaskGroupsRequest.Code
            oImpRequest.Description = oUpdateTaskGroupsRequest.Description
            oImpRequest.IsDeleted = oUpdateTaskGroupsRequest.IsDeleted
            oImpRequest.EffectiveDate = oUpdateTaskGroupsRequest.EffectiveDate
            oImpRequest.TaskGroupCategoryKey = oUpdateTaskGroupsRequest.TaskGroupCategoryKey
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.UpdateTaskGroups(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateTaskGroupsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateTaskGroupsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetDefaultRiskClauses(ByVal GetDefaultRiskClausesRequest As GetDefaultRiskClausesRequestType) As GetDefaultRiskClausesResponseType Implements IPureCoreService.GetDefaultRiskClauses

        Try

            Dim sUserName As String = GetDefaultRiskClausesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMARsk", iUserId)
            CommonFunctions.CheckSecurityToken(GetDefaultRiskClausesRequest.WCFSecurityToken)
            Dim oResponse As New GetDefaultRiskClausesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetDefaultRiskClausesRequest.BranchCode)

            ' Implementation structures

            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetDefaultRiskClausesRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetDefaultRiskClausesResponseType = Nothing

            oImpRequest.BranchCode = GetDefaultRiskClausesRequest.BranchCode
            oImpRequest.CurrentBranchCode = GetDefaultRiskClausesRequest.CurrentBranchCode
            oImpRequest.RiskTypeCode = GetDefaultRiskClausesRequest.RiskTypeCode
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetDefaultRiskClauses(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Retrieve the values from the implementation response structure
                If (oImpResponse IsNot Nothing AndAlso oImpResponse.Documents IsNot Nothing AndAlso oImpResponse.Documents.Row IsNot Nothing) Then
                    oResponse.Documents = oImpResponse.Documents.Row.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGetDefaultRiskClausesResponseTypeDocumentsRow, BaseGetDefaultRiskClausesResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetDefaultRiskClausesList))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetDefaultRiskClausesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetDefaultRiskClausesRequest))
            Return Nothing
        End Try
    End Function

    Public Function GetDocument(ByVal GetDocumentRequest As GetDocumentRequestType) As GetDocumentResponseType Implements IPureCoreService.GetDocument
        Try
            Dim sUserName As String = GetDocumentRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGDoc", iUserId)
            CommonFunctions.CheckSecurityToken(GetDocumentRequest.WCFSecurityToken)

            Dim oResponse As New GetDocumentResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetDocumentRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetDocumentRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetDocumentResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetDocumentRequest.BranchCode
            oImpRequest.DocNum = GetDocumentRequest.DocNum
            oImpRequest.Compress = GetDocumentRequest.Compress
            oImpRequest.ConvertPdf = GetDocumentRequest.ConvertPdf
            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetDocument(oImpRequest)

                ' Retrieve the values from the implementation response structure
                oResponse.PdfDocument = oImpResponse.PdfDocument
                oResponse.FileExtension = oImpResponse.FileExtension

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetDocumentRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetDocumentRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetDocumentDefaults(ByVal GetDocumentDefaultsRequest As GetDocumentDefaultsRequestType) As GetDocumentDefaultsResponseType Implements IPureCoreService.GetDocumentDefaults

        Try
            Dim sUserName As String = GetDocumentDefaultsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGenDoc", iUserId)
            CommonFunctions.CheckSecurityToken(GetDocumentDefaultsRequest.WCFSecurityToken)

            Dim oResponse As New GetDocumentDefaultsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetDocumentDefaultsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetDocumentDefaultsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetDocumentDefaultsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetDocumentDefaultsRequest.BranchCode
            oImpRequest.DocumentTemplateCodes = GetDocumentDefaultsRequest.DocumentTemplateCodes
            oImpRequest.DocumentTemplateKeys = GetDocumentDefaultsRequest.DocumentTemplateKeys

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetDocumentDefaults(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)


                If Not oImpResponse.DocumentTemplates Is Nothing Then
                    'Dim iRow As Integer
                    oResponse.DocumentTemplates = oImpResponse.DocumentTemplates.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseGetDocumentDefaultsResponseTypeDocumentTemplates, BaseGetDocumentDefaultsResponseTypeDocumentTemplates)(AddressOf CommonFunctions.ToServiceGetDocumentDefaultsList))

                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetDocumentDefaultsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetDocumentDefaultsRequest))
            Return Nothing
        End Try
    End Function



    ''' <summary>
    ''' This Web method is used to get the Event Task details by passing the Request parameters as request type objects
    ''' and also the response object is being passed to the correct messaging format.
    '''</summary>
    '''<param name="oGetEventDetailsRequest" type="GetEventDetailsRequestType"></param>   
    '''<remarks></remarks>

    Public Function GetEventDetails(ByVal oGetEventDetailsRequest As GetEventDetailsRequestType) As GetEventDetailsResponseType Implements IPureCoreService.GetEventDetails, IPurePartyService.GetEventDetails, IPurePolicyService.GetEventDetails

        Try


            Dim sUserName As String = oGetEventDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMEVTLOG", iUserId)
            CommonFunctions.CheckSecurityToken(oGetEventDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetEventDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetEventDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetEventDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetEventDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetEventDetailsRequest.BranchCode
            oImpRequest.InsuranceFolderKey = oGetEventDetailsRequest.InsuranceFolderKey
            oImpRequest.InsuranceFolderKeySpecified = oGetEventDetailsRequest.InsuranceFolderKeySpecified
            oImpRequest.InsuranceFileKey = oGetEventDetailsRequest.InsuranceFileKey
            oImpRequest.InsuranceFileKeySpecified = oGetEventDetailsRequest.InsuranceFileKeySpecified
            oImpRequest.ClaimKey = oGetEventDetailsRequest.ClaimKey
            oImpRequest.ClaimKeySpecified = oGetEventDetailsRequest.ClaimKeySpecified
            oImpRequest.OldPartyTypeKey = oGetEventDetailsRequest.OldPartyTypeKey
            oImpRequest.OldPartyTypeKeySpecified = oGetEventDetailsRequest.OldPartyTypeKeySpecified
            oImpRequest.FSAComplaintFolderKey = oGetEventDetailsRequest.FSAComplaintFolderKey
            oImpRequest.FSAComplaintFolderKeySpecified = oGetEventDetailsRequest.FSAComplaintFolderKeySpecified
            oImpRequest.AccountKey = oGetEventDetailsRequest.AccountKey
            oImpRequest.AccountKeySpecified = oGetEventDetailsRequest.AccountKeySpecified
            oImpRequest.FromDate = oGetEventDetailsRequest.FromDate
            oImpRequest.FromDateSpecified = oGetEventDetailsRequest.FromDateSpecified
            oImpRequest.DateTo = oGetEventDetailsRequest.DateTo
            oImpRequest.DateToSpecified = oGetEventDetailsRequest.DateToSpecified
            oImpRequest.BaseClaimKey = oGetEventDetailsRequest.BaseClaimKey
            oImpRequest.BaseClaimKeySpecified = oGetEventDetailsRequest.BaseClaimKeySpecified
            oImpRequest.CaseKey = oGetEventDetailsRequest.CaseKey
            oImpRequest.CaseKeySpecified = oGetEventDetailsRequest.CaseKeySpecified
            oImpRequest.BaseCaseKey = oGetEventDetailsRequest.BaseCaseKey
            oImpRequest.BaseCaseKeySpecified = oGetEventDetailsRequest.BaseCaseKeySpecified
            oImpRequest.PartyKey = oGetEventDetailsRequest.PartyKey
            ' Start Change request as said by gaurav on 07-08-2008
            oImpRequest.BGKey = oGetEventDetailsRequest.BGKey
            oImpRequest.BGKeySpecified = oGetEventDetailsRequest.BGKeySpecified
            ' End Change request as said by gaurav on 07-08-2008
            oImpRequest.EventTypeKey = oGetEventDetailsRequest.EventTypeKey
            oImpRequest.EventTypeKeySpecified = oGetEventDetailsRequest.EventTypeKeySpecified
            oImpRequest.UserId = oGetEventDetailsRequest.UserId
            oImpRequest.UserIdSpecified = oGetEventDetailsRequest.UserIdSpecified
            oImpRequest.ClaimNumber = oGetEventDetailsRequest.ClaimNumber
            oImpRequest.ClaimNumberSpecified = oGetEventDetailsRequest.ClaimNumberSpecified
            oImpRequest.WCFSecurityToken = If(oGetEventDetailsRequest.WCFSecurityToken.Length > 0, oGetEventDetailsRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetEventDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.EventDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseGetEventDetailsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetEventDetailsResponseTypeEventDetails", sConvertToTypeName:="BaseGetEventDetailsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.EventDetails = DataTabletoList_GetEventDetails(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetEventDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetEventDetailsRequest))
            Return Nothing
        End Try

    End Function


    ''' <summary>
    ''' </summary>
    ''' <param name="oGetAuditTrailUserRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailUser(oGetAuditTrailUserRequestType As GetAuditTrailUserRequestType) As GetAuditTrailUserResponseType Implements IPureCoreService.GetAuditTrailUser
        Dim oGetAuditTrailUserResponseType As New GetAuditTrailUserResponseType

        Try

            Dim sUserName As String = oGetAuditTrailUserRequestType.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAUT", iUserId)
            CommonFunctions.CheckSecurityToken(oGetAuditTrailUserRequestType.WCFSecurityToken)
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetAuditTrailUserRequestType.BranchCode)

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAuditTrailUserRequestType
            Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.GetAuditTrailUserResponseType



            Try

                ' Call the implementation method

                oImpRequest.UserId = iUserId
                oImpResponse = DirectCast(oBusiness.GetAuditTrailUser(oImpRequest), Internal.BaseGetAudittrailUserResponseType)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oGetAuditTrailUserResponseType.AuditTrailUser = DataTabletoList_GetAuditTrailUser(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oGetAuditTrailUserResponseType, ex, CommonFunctions.CreateDictionary(oGetAuditTrailUserRequestType))
            End Try
            Return oGetAuditTrailUserResponseType

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetAuditTrailUserRequestType))
            Return Nothing
        End Try


    End Function

    ''' <summary>
    ''' </summary>
    ''' <param name="oGetAuditTrailModuleRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuditTrailModule(oGetAuditTrailModuleRequestType As GetAuditTrailModuleRequestType) As GetAuditTrailModuleResponseType Implements IPureCoreService.GetAuditTrailModule
        Dim oGetAuditTrailModuleResponseType As New GetAuditTrailModuleResponseType
        Try
            Dim sUserName As String = oGetAuditTrailModuleRequestType.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAUT", iUserId)
            CommonFunctions.CheckSecurityToken(oGetAuditTrailModuleRequestType.WCFSecurityToken)
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetAuditTrailModuleRequestType.BranchCode)

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAuditTrailModuleRequestType
            Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.GetAuditTrailModuleResponseType


            Try


                oImpResponse = DirectCast(oBusiness.GetAuditTrailModule(oImpRequest), Internal.BaseGetAudittrailModuleResponseType)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oGetAuditTrailModuleResponseType.AuditTrailModule = DataTabletoList_GetAuditTrailModule(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oGetAuditTrailModuleResponseType, ex, CommonFunctions.CreateDictionary(oGetAuditTrailModuleRequestType))
            End Try


            Return oGetAuditTrailModuleResponseType

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetAuditTrailModuleRequestType))
            Return Nothing
        End Try


    End Function


    ''' <summary>
    ''' This Web method is used to get the System Event Task details by passing the Request parameters as request type objects
    ''' and also the response object is being passed to the correct messaging format.
    '''</summary>
    '''<param name="oGetAuditTrailRequest" type="GetAuditTrailRequestType"></param>   
    '''<remarks></remarks>

    Public Function GetAuditTrailDetails(ByVal oGetAuditTrailRequest As GetAuditTrailRequestType) As GetAuditTrailResponseType Implements IPureCoreService.GetAuditTrailDetails
        Dim oGetAuditTrailResponseType As New GetAuditTrailResponseType
        Try
            Dim sUserName As String = oGetAuditTrailRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            'CommonFunctions.CheckAuthority("SAMAUT", iUserId)
            CommonFunctions.CheckSecurityToken(oGetAuditTrailRequest.WCFSecurityToken)
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetAuditTrailRequest.BranchCode)

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAuditTrailRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAuditTrailResponseType = Nothing


            Try
                oImpRequest.ModuleId = oGetAuditTrailRequest.ModuleId
                oImpRequest.ModuleIdSpecified = oGetAuditTrailRequest.ModuleIdSpecified
                oImpRequest.UserId = oGetAuditTrailRequest.UserId
                oImpRequest.UserIdSpecified = oGetAuditTrailRequest.UserIdSpecified
                oImpRequest.FromDate = oGetAuditTrailRequest.FromDate
                oImpRequest.FromDateSpecified = oGetAuditTrailRequest.FromDateSpecified
                oImpRequest.DateTo = oGetAuditTrailRequest.DateTo
                oImpRequest.DateToSpecified = oGetAuditTrailRequest.DateToSpecified

                oImpResponse = DirectCast(oBusiness.GetAuditTrailsDetails(oImpRequest), Internal.BaseGetAudittrailResponseType)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oGetAuditTrailResponseType.AuditTrail = DataTabletoList_GetAuditTrail(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oGetAuditTrailResponseType, ex, CommonFunctions.CreateDictionary(oGetAuditTrailRequest))
            End Try


            Return oGetAuditTrailResponseType

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetAuditTrailRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''This Web method is used to get the subagents details by passing the EventKey in the request object and output is deserialized before it is 
    '''being sent to front end
    '''</summary>
    '''<param name="oGetEventNoteRequest" type="GetEventNoteRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetEventNoteResponseType</returns>
    '''<remarks></remarks>

    Public Function GetEventNote(ByVal oGetEventNoteRequest As GetEventNoteRequestType) As GetEventNoteResponseType Implements IPureCoreService.GetEventNote
        Try


            Dim sUserName As String = oGetEventNoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMEVTLOG", iUserId)
            CommonFunctions.CheckSecurityToken(oGetEventNoteRequest.WCFSecurityToken)
            Dim oResponse As New GetEventNoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetEventNoteRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetEventNoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetEventNoteResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetEventNoteRequest.BranchCode
            oImpRequest.EventKey = oGetEventNoteRequest.EventKey
            oImpRequest.WCFSecurityToken = If(oGetEventNoteRequest.WCFSecurityToken.Length > 0, oGetEventNoteRequest.WCFSecurityToken, "WCFSecurityToken")


            Try

                'Call the implementation method
                oImpResponse = oBusiness.GetEventNote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.EventNotes = SAMFunc.(Of List(Of BaseGetEventNoteResponseTypeRow))(elmResultDataSet:=oImpResponse.EventNotesDataset, sFromTypeName:="BaseGetEventNoteResponseTypeEventNotes", sConvertToTypeName:="BaseGetEventNoteResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.EventNotes = DataTabletoList_GetEventNote(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetEventNoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetEventNoteRequest))
            Return Nothing
        End Try

    End Function


    Public Function UpdateStandardPolicyWordings(ByVal oUpdateStandardPolicyWordingsRequest As UpdateStandardPolicyWordingsRequestType) As UpdateStandardPolicyWordingsResponseType Implements IPurePolicyService.UpdateStandardPolicyWordings

        Try
            Dim sUserName As String = oUpdateStandardPolicyWordingsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateStandardPolicyWordingsRequest.WCFSecurityToken)

            Dim oResponse As New UpdateStandardPolicyWordingsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateStandardPolicyWordingsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateStandardPolicyWordingsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateStandardPolicyWordingsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateStandardPolicyWordingsRequest.BranchCode
            oImpRequest.InsuranceFileKey = oUpdateStandardPolicyWordingsRequest.InsuranceFileKey
            oImpRequest.TimeStamp = oUpdateStandardPolicyWordingsRequest.TimeStamp

            If ((oUpdateStandardPolicyWordingsRequest.PolicyStandardWordings IsNot Nothing) AndAlso (oUpdateStandardPolicyWordingsRequest.PolicyStandardWordings IsNot Nothing)) Then
                Dim iLength As Int32 = oUpdateStandardPolicyWordingsRequest.PolicyStandardWordings.Count
                oImpRequest.PolicyStandardWordings = New BaseImplementationTypes.BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordings
                For icount As Integer = 0 To iLength - 1
                    ReDim Preserve oImpRequest.PolicyStandardWordings.Row(icount)
                    oImpRequest.PolicyStandardWordings.Row(icount) = New BaseImplementationTypes.BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordingsRow
                    oImpRequest.PolicyStandardWordings.Row(icount).Code = oUpdateStandardPolicyWordingsRequest.PolicyStandardWordings(icount).Code
                    oImpRequest.PolicyStandardWordings.Row(icount).DocumentTemplateKey = oUpdateStandardPolicyWordingsRequest.PolicyStandardWordings(icount).DocumentTemplateKey
                Next
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateStandardPolicyWordings(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateStandardPolicyWordingsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateStandardPolicyWordingsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GenerateDocument(ByVal GenerateDocumentRequest As GenerateDocumentRequestType) As GenerateDocumentResponseType Implements IPureCoreService.GenerateDocument

        Try


            Dim sUserName As String = GenerateDocumentRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGenDoc", iUserId)
            CommonFunctions.CheckSecurityToken(GenerateDocumentRequest.WCFSecurityToken)
            Dim oResponse As New GenerateDocumentResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GenerateDocumentRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GenerateDocumentRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GenerateDocumentResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GenerateDocumentRequest.BranchCode
            oImpRequest.DocumentTemplateCode = GenerateDocumentRequest.DocumentTemplateCode
            oImpRequest.InsuranceFileKey = GenerateDocumentRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = GenerateDocumentRequest.InsuranceFolderKey
            oImpRequest.Mode = GenerateDocumentRequest.Mode
            oImpRequest.OutputAsHTML = GenerateDocumentRequest.OutputAsHTML
            oImpRequest.OutputAsPDF = GenerateDocumentRequest.OutputAsPDF
            oImpRequest.ParameterXML = GenerateDocumentRequest.ParameterXML
            oImpRequest.PartyKey = GenerateDocumentRequest.PartyKey
            oImpRequest.UserName = sUserName
            oImpRequest.ClaimKey = GenerateDocumentRequest.ClaimKey
            oImpRequest.DocumentRef = GenerateDocumentRequest.DocumentRef
            oImpRequest.SkipArchiveOnEdit = GenerateDocumentRequest.SkipArchiveOnEdit
            oImpRequest.OutputAsTXT = GenerateDocumentRequest.OutputAsTXT
            oImpRequest.ArchiveDocFileName = GenerateDocumentRequest.ArchiveDocFileName
            oImpRequest.DestinationFileName = GenerateDocumentRequest.DestinationFileName
            oImpRequest.IsSuppressArchive = GenerateDocumentRequest.IsSuppressArchive
            If GenerateDocumentRequest.SpoolDocumentOnlySpecified = True Then
                oImpRequest.SpoolDocumentOnly = GenerateDocumentRequest.SpoolDocumentOnly
            Else
                oImpRequest.SpoolDocumentOnly = False
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GenerateDocument(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpRequest.SpoolDocumentOnly = False Then
                    ' Retrieve the values from the implementation response structure
                    oResponse.MergedFilePath = oImpResponse.MergedFilePath
                    oResponse.SpooledZipFile = oImpResponse.SpooledZipFile
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GenerateDocumentRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GenerateDocumentRequest))
            Return Nothing
        End Try

    End Function

    Public Function UpdateCashDeposit(ByVal oUpdateCashDepositRequest As UpdateCashDepositRequestType) As UpdateCashDepositResponseType Implements IPureAccountService.UpdateCashDeposit

        Try


            Dim sUserName As String = oUpdateCashDepositRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCDUPD", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateCashDepositRequest.WCFSecurityToken)
            Dim oResponse As New UpdateCashDepositResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateCashDepositRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateCashDepositRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateCashDepositResponseType = Nothing

            Dim iCount As Integer = 0
            oImpRequest.BranchCode = oUpdateCashDepositRequest.BranchCode
            If (oUpdateCashDepositRequest.CashDeposit IsNot Nothing) Then
                If (oUpdateCashDepositRequest.CashDeposit.Count > 0) Then
                    ReDim Preserve oImpRequest.CashDeposit(oUpdateCashDepositRequest.CashDeposit.Count - 1)
                    For iCount = 0 To oUpdateCashDepositRequest.CashDeposit.Count - 1
                        With oUpdateCashDepositRequest
                            oImpRequest.CashDeposit(iCount) = New BaseImplementationTypes.BaseCommonCashDepositItemType
                            oImpRequest.CashDeposit(iCount).CashDepositRef = .CashDeposit(iCount).CashDepositRef
                            oImpRequest.CashDeposit(iCount).CDTimeStamp = .CashDeposit(iCount).CDTimeStamp
                            oImpRequest.CashDeposit(iCount).IsSinglePolicy = .CashDeposit(iCount).IsSinglePolicy
                            oImpRequest.CashDeposit(iCount).PartyCode = .CashDeposit(iCount).PartyCode
                            oImpRequest.CashDeposit(iCount).UserName = .CashDeposit(iCount).UserName

                            Dim iCounter As Integer = 0
                            Dim oProduct As BaseCommonCashDepositItemTypeProducts
                            If (.CashDeposit(iCount).Products IsNot Nothing) Then
                                ReDim Preserve oImpRequest.CashDeposit(iCount).Products(.CashDeposit(iCount).Products.Count - 1)
                                For Each oProduct In .CashDeposit(iCount).Products
                                    oImpRequest.CashDeposit(iCount).Products(iCounter) = New BaseImplementationTypes.BaseCommonCashDepositItemTypeProducts
                                    oImpRequest.CashDeposit(iCount).Products(iCounter).ProductCode = oProduct.ProductCode
                                    iCounter = iCounter + 1
                                Next
                            End If

                            Dim oBranch As BaseCommonCashDepositItemTypeBranches
                            If (.CashDeposit(iCount).Branches IsNot Nothing) Then
                                ReDim Preserve oImpRequest.CashDeposit(iCount).Branches(.CashDeposit(iCount).Branches.Count - 1)
                                iCounter = 0
                                For Each oBranch In .CashDeposit(iCount).Branches
                                    oImpRequest.CashDeposit(iCount).Branches(iCounter) = New BaseImplementationTypes.BaseCommonCashDepositItemTypeBranches
                                    oImpRequest.CashDeposit(iCount).Branches(iCounter).BranchCode = oBranch.BranchCode
                                    iCounter = iCounter + 1
                                Next
                            End If
                        End With
                    Next
                End If
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.UpdateCashDeposit(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse IsNot Nothing AndAlso oImpResponse.CashDeposit IsNot Nothing Then
                    oResponse.CashDeposit = oImpResponse.CashDeposit.Row.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseUpdateCashDepositResponseTypeCashDepositRow, BaseUpdateCashDepositResponseTypeRow)(AddressOf CommonFunctions.ToServiceUpdateCashDepositList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateCashDepositRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateCashDepositRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetNextCashDepositRef(ByVal oGetNextCashDepositRequest As GetNextCashDepositRefRequestType) As GetNextCashDepositRefResponseType Implements IPureAccountService.GetNextCashDepositRef

        Try


            Dim sUserName As String = oGetNextCashDepositRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGETNTCD", iUserId)
            CommonFunctions.CheckSecurityToken(oGetNextCashDepositRequest.WCFSecurityToken)
            Dim oResponse As New GetNextCashDepositRefResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetNextCashDepositRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetNextCashDepositRefRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetNextCashDepositRefResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetNextCashDepositRequest.BranchCode
            oImpRequest.PartyCode = oGetNextCashDepositRequest.PartyCode
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetNextCashDepositRef(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                With oResponse
                    .CashDepositRef = oImpResponse.CashDepositRef
                    .CDTimeStamp = oImpResponse.CDTimeStamp
                    .NextCashDepositNumber = oImpResponse.NextCashDepositNumber
                End With

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetNextCashDepositRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetNextCashDepositRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetLinkedCashDeposits(ByVal oGetLinkedCashDepositsRequest As GetLinkedCashDepositsRequestType) As GetLinkedCashDepositsResponseType Implements IPureAccountService.GetLinkedCashDeposits
        Try



            Dim sUserName As String = oGetLinkedCashDepositsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer


            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCDGETLN", iUserId)
            CommonFunctions.CheckSecurityToken(oGetLinkedCashDepositsRequest.WCFSecurityToken)
            Dim oResponse As New GetLinkedCashDepositsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetLinkedCashDepositsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetLinkedCashDepositsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetLinkedCashDepositsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetLinkedCashDepositsRequest.BranchCode
            oImpRequest.PartyCode = oGetLinkedCashDepositsRequest.PartyCode
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetLinkedCashDeposits(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.CashDeposit IsNot Nothing Then
                    If IsArray(oImpResponse.CashDeposit) Then
                        oResponse.CashDeposit = oImpResponse.CashDeposit.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseCashDepositItemType, BaseCashDepositItemType)(AddressOf CommonFunctions.ToServiceFindCashDepositList))
                    End If
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetLinkedCashDepositsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetLinkedCashDepositsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetCashDeposit(ByVal oGetCashDepositRequest As GetCashDepositRequestType) As GetCashDepositResponseType Implements IPureAccountService.GetCashDeposit

        Try


            Dim sUserName As String = oGetCashDepositRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCDGET", iUserId)
            CommonFunctions.CheckSecurityToken(oGetCashDepositRequest.WCFSecurityToken)
            Dim oResponse As New GetCashDepositResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetCashDepositRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCashDepositRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCashDepositResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetCashDepositRequest.BranchCode
            oImpRequest.PartyCode = oGetCashDepositRequest.PartyCode
            oImpRequest.CashDepositRef = oGetCashDepositRequest.CashDepositRef

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetCashDeposit(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                With oResponse
                    .AccountKey = oImpResponse.AccountKey
                    .IsDeleted = oImpResponse.IsDeleted
                    .PartyKey = oImpResponse.PartyKey
                    .IsSinglePolicy = oImpResponse.IsSinglePolicy
                    .CashDepositKey = oImpResponse.CashDepositKey
                    .CDTimeStamp = oImpResponse.CDTimeStamp

                    If oImpResponse.Products IsNot Nothing Then
                        If IsArray(oImpResponse.Products) Then
                            oResponse.Products = oImpResponse.Products.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGetCashDepositResponseTypeProducts, BaseGetCashDepositResponseTypeProducts)(AddressOf CommonFunctions.ToServiceProductList))
                        End If
                    End If

                    If oImpResponse.Branches IsNot Nothing Then
                        If IsArray(oImpResponse.Branches) Then
                            oResponse.Branches = oImpResponse.Branches.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGetCashDepositResponseTypeBranches, BaseGetCashDepositResponseTypeBranches)(AddressOf CommonFunctions.ToServiceBranchList))
                        End If
                    End If


                End With

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetCashDepositRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetCashDepositRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' GetAgentSettings
    ''' </summary>
    ''' <param name="oGetAgentSettingsRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAgentSettings(ByVal oGetAgentSettingsRequestType As GetAgentSettingsRequestType) As GetAgentSettingsResponseType Implements IPureCoreService.GetAgentSettings
        Try

            Dim sUserName As String = oGetAgentSettingsRequestType.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGAGSETT", iUserId)
            CommonFunctions.CheckSecurityToken(oGetAgentSettingsRequestType.WCFSecurityToken)
            Dim oResponse As New GetAgentSettingsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetAgentSettingsRequestType.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAgentSettingsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAgentSettingsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetAgentSettingsRequestType.BranchCode
            If oGetAgentSettingsRequestType.AgentKey <> 0 Then
                oImpRequest.AgentKey = oGetAgentSettingsRequestType.AgentKey
            Else
                oImpRequest.AgentKey = iAgentKey
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetAgentSettings(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.AllowConsolidatedCommission = oImpResponse.AllowConsolidatedCommission
                oResponse.AlternateReferenceForEachTransaction = oImpResponse.AlternateReferenceForEachTransaction
                oResponse.AlternateReferenceMandatory = oImpResponse.AlternateReferenceMandatory
                oResponse.CanMakeLiveBankGuarantee = oImpResponse.CanMakeLiveBankGuarantee
                oResponse.CanMakeLiveInstalments = oImpResponse.CanMakeLiveInstalments
                oResponse.CanMakeLiveInvoice = oImpResponse.CanMakeLiveInvoice
                oResponse.CanMakeLivePaynow = oImpResponse.CanMakeLivePaynow
                oResponse.DaysAllowed = oImpResponse.DaysAllowed
                oResponse.DomiciledForTax = oImpResponse.DomiciledForTax
                oResponse.ExpectedDailyPremium = oImpResponse.ExpectedDailyPremium
                oResponse.FloatBalanceLimit = oImpResponse.FloatBalanceLimit
                oResponse.IsFloatBalanceAccount = oImpResponse.IsFloatBalanceAccount
                oResponse.IsOverdraftAccount = oImpResponse.IsOverdraftAccount
                oResponse.IsPrepaymentAccount = oImpResponse.IsPrepaymentAccount
                oResponse.IsStandardAccount = oImpResponse.IsStandardAccount
                oResponse.OverdraftExpiry = oImpResponse.OverdraftExpiry
                oResponse.OverdraftLimit = oImpResponse.OverdraftLimit
                oResponse.UseOverrideCommissionRate = oImpResponse.UseOverrideCommissionRate
                oResponse.CanMakeLiveCashDeposit = oImpResponse.CanMakeLiveCashDeposit
                oResponse.PartyAgentDateCancelled = oImpResponse.PartyAgentDateCancelled
                oResponse.Branches = SAMFunc.GetDeserializedValues(Of List(Of BaseGetAgentSettingsResponseTypeBranchesRow))(elmResultDataSet:=oImpResponse.ResultDataSet, sFromTypeName:="BaseGetAgentSettingsResponseTypeBranches", sConvertToTypeName:="BaseGetAgentSettingsResponseTypeBranchesRow")



                '                oResponse.CanMakeLiveCashDeposit = oImpResponse.CanMakeLiveCashDeposit
                oResponse.CorrespondenceType = oImpResponse.CorrespondenceType
                oResponse.IsReceiveClientCorrespondence = oImpResponse.IsReceiveClientCorrespondence
                If oImpResponse.Users IsNot Nothing Then
                    If IsArray(oImpResponse.Users) Then
                        oResponse.Users = oImpResponse.Users.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseUserDetailsType, BaseUserDetailsType)(AddressOf CommonFunctions.ToServiceBaseUserDetailsTypeList))
                    End If
                End If

                If oImpResponse.Contacts IsNot Nothing Then
                    If IsArray(oImpResponse.Contacts) Then
                        oResponse.Contacts = oImpResponse.Contacts.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseContactType, BaseContactType)(AddressOf CommonFunctions.ToServiceBaseContactTypeList))
                    End If
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetAgentSettingsRequestType))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetAgentSettingsRequestType))
            Return Nothing
        End Try
    End Function

    Public Function GetEventNoteType(ByVal GetEventNoteTypeRequest As GetEventNoteTypeRequestType) As GetEventNoteTypeResponseType Implements IPureCoreService.GetEventNoteType
        Try


            Dim sUserName As String = GetEventNoteTypeRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMEVTTY", iUserId)
            CommonFunctions.CheckSecurityToken(GetEventNoteTypeRequest.WCFSecurityToken)

            Dim oResponse As New GetEventNoteTypeResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetEventNoteTypeRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetEventNoteTypeRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetEventNoteTypeResponseType = Nothing

            oImpRequest.BranchCode = GetEventNoteTypeRequest.BranchCode
            oImpRequest.AccountKey = GetEventNoteTypeRequest.AccountKey
            oImpRequest.AccountKeySpecified = GetEventNoteTypeRequest.AccountKeySpecified
            oImpRequest.InsuranceFileKey = GetEventNoteTypeRequest.InsuranceFileKey
            oImpRequest.InsuranceFileKeySpecified = GetEventNoteTypeRequest.InsuranceFileKeySpecified
            oImpRequest.CaseKey = GetEventNoteTypeRequest.CaseKey
            oImpRequest.CaseKeySpecified = GetEventNoteTypeRequest.CaseKeySpecified
            oImpRequest.ClaimKey = GetEventNoteTypeRequest.ClaimKey
            oImpRequest.ClaimKeySpecified = GetEventNoteTypeRequest.ClaimKeySpecified
            oImpRequest.WCFSecurityToken = If(GetEventNoteTypeRequest.WCFSecurityToken.Length > 0, GetEventNoteTypeRequest.WCFSecurityToken, "WCFSecurityToken")


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetEventNoteType(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.EventTypes = SAMFunc.GetDeserializedValues(Of List(Of BaseGetEventNoteTypeResponseTypeRow))(elmResultDataSet:=oImpResponse.EventTypes, sFromTypeName:="BaseGetEventNoteTypeResponseTypeEventTypes", sConvertToTypeName:="BaseGetEventNoteTypeResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.EventTypes = DataTabletoList_GetEventNoteType(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetEventNoteTypeRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetEventNoteTypeRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This is webservice method for  Get FinancePlans
    '''<param name="oGetFinancePlansRequest" type="GetFinancePlansRequestType"></param>   
    ''' <returns>GetFinancePlansResponseType</returns>  
    '''</summary>
    '''<remarks></remarks>  
    Public Function GetFinancePlans(ByVal oGetFinancePlansRequest As GetFinancePlansRequestType) As GetFinancePlansResponseType Implements IPureCoreService.GetFinancePlans

        Try



            Dim sUserName As String = oGetFinancePlansRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFP", iUserId)
            CommonFunctions.CheckSecurityToken(oGetFinancePlansRequest.WCFSecurityToken)
            Dim oResponse As New GetFinancePlansResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetFinancePlansRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetFinancePlansRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetFinancePlansResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetFinancePlansRequest.BranchCode
            oImpRequest.PartyKey = oGetFinancePlansRequest.PartyKey
            oImpRequest.StatusKey = CType([Enum].ToObject(GetType(FinancePlanStatus), oGetFinancePlansRequest.StatusKey), BaseImplementationTypes.FinancePlanStatus)

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetFinancePlans(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If (oImpResponse.FinancePlans IsNot Nothing) Then
                    If (oImpResponse.FinancePlans.Row IsNot Nothing) Then
                        'oResponse.FinancePlans = oImpResponse.FinancePlans.Row.ToList().ConvertAll(New Converter(Of SFI.SAMForInsuranceV2.BaseGetFinancePlansResponseTypeFinancePlansRow, BaseGetFinancePlansResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetFinancePlansList))
                        oResponse.FinancePlans = oImpResponse.FinancePlans.Row.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGetFinancePlansResponseTypeFinancePlansRow, BaseGetFinancePlansResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetFinancePlansList))
                    End If
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetFinancePlansRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetFinancePlansRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetMediaType(ByVal oGetMediaTypeRequest As GetMediaTypeRequestType) As GetMediaTypeResponseType Implements IPureCoreService.GetMediaType
        Try



            Dim sUserName As String = oGetMediaTypeRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGetMedt", iUserId)
            CommonFunctions.CheckSecurityToken(oGetMediaTypeRequest.WCFSecurityToken)
            Dim oGetMediaTypeResponse As New GetMediaTypeResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetMediaTypeRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetMediaTypeRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetMediaTypeResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetMediaTypeRequest.BranchCode
            oImpRequest.MediaTypeCode = oGetMediaTypeRequest.MediaTypeCode

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetMediaType(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oGetMediaTypeResponse.Description = oImpResponse.Description
                oGetMediaTypeResponse.MediaTypeCode = oImpResponse.MediaTypeCode
                oGetMediaTypeResponse.MediaTypeKey = oImpResponse.MediaTypeKey
                oGetMediaTypeResponse.RefundDelay = oImpResponse.RefundDelay

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oGetMediaTypeResponse, ex, CommonFunctions.CreateDictionary(oGetMediaTypeRequest))
            End Try

            Return oGetMediaTypeResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetMediaTypeRequest))
            Return Nothing
        End Try

    End Function


    Public Function GetOptionSetting(ByVal GetOptionSettingRequest As GetOptionSettingRequestType) As GetOptionSettingResponseType Implements IPureCoreService.GetOptionSetting, IPureAccountService.GetOptionSetting, IPureClaimService.GetOptionSetting, IPurePartyService.GetOptionSetting, IPurePolicyService.GetOptionSetting, IPureSecurityService.GetOptionSetting
        Try
            Dim sUserName As String = GetOptionSettingRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGOS", iUserId)
            '    CommonFunctions.CheckSecurityToken(GetOptionSettingRequest.WCFSecurityToken)

            Dim oResponse As New GetOptionSettingResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetOptionSettingRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetOptionSettingRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetOptionSettingResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetOptionSettingRequest.BranchCode
            oImpRequest.OptionType = CType(GetOptionSettingRequest.OptionType, BaseImplementationTypes.OptionType)
            oImpRequest.OptionNumber = GetOptionSettingRequest.OptionNumber

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetOptionSetting(oImpRequest)

                ' Retrieve the values from the implementation response structure
                oResponse.OptionValue = oImpResponse.OptionValue
                oResponse.UnderwritingType = oImpResponse.UnderwritingType
                oResponse.AccountType = oImpResponse.AccountType

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetOptionSettingRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetOptionSettingRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetPeriod(ByVal GetPeriodRequest As GetPeriodRequestType) As GetPeriodResponseType Implements IPureAccountService.GetPeriod
        Try

            Dim sUserName As String = GetPeriodRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGDoc", iUserId)
            CommonFunctions.CheckSecurityToken(GetPeriodRequest.WCFSecurityToken)

            Dim oResponse As New GetPeriodResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetPeriodRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPeriodRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPeriodResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetPeriodRequest.BranchCode
            oImpRequest.GetPaymentsAllocated = GetPeriodRequest.GetPaymentsAllocated
            oImpRequest.WCFSecurityToken = If(GetPeriodRequest.WCFSecurityToken.Length > 0, GetPeriodRequest.WCFSecurityToken, "WCFSecurityToken")

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetPeriod(oImpRequest)

                ' Retrieve the values from the implementation response structure
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.Periods = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPeriodResponseTypeRow))(elmResultDataSet:=oImpResponse.Periods, sFromTypeName:="BaseGetPeriodResponseTypePeriods", sConvertToTypeName:="BaseGetPeriodResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Periods = DataTabletoList_GetPeriod(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetPeriodRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetPeriodRequest))
            Return Nothing
        End Try
    End Function

    Public Function UpdateCoverNoteSheet(ByVal UpdateCoverNoteSheetRequest As UpdateCoverNoteSheetRequestType) As UpdateCoverNoteSheetResponseType Implements IPureCoreService.UpdateCoverNoteSheet

        Try


            Dim sUserName As String = UpdateCoverNoteSheetRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUCNSKey", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateCoverNoteSheetRequest.WCFSecurityToken)
            Dim oResponse As New UpdateCoverNoteSheetResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateCoverNoteSheetRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateCoverNoteSheetRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateCoverNoteSheetResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateCoverNoteSheetRequest.BranchCode
            oImpRequest.CoverNoteBookKey = UpdateCoverNoteSheetRequest.CoverNoteBookKey
            oImpRequest.OldCoverNoteSheetNumber = UpdateCoverNoteSheetRequest.OldCoverNoteSheetNumber
            oImpRequest.NewCoverNoteSheetNumber = UpdateCoverNoteSheetRequest.NewCoverNoteSheetNumber
            oImpRequest.InsuranceFileCntSpecified = UpdateCoverNoteSheetRequest.InsuranceFileCntSpecified
            If (oImpRequest.InsuranceFileCntSpecified = UpdateCoverNoteSheetRequest.InsuranceFileCntSpecified) Then
                oImpRequest.InsuranceFileCnt = UpdateCoverNoteSheetRequest.InsuranceFileCnt
            End If
            oImpRequest.AssignedDate = UpdateCoverNoteSheetRequest.AssignedDate
            oImpRequest.Comments = UpdateCoverNoteSheetRequest.Comments
            oImpRequest.CoverNoteStatusCode = UpdateCoverNoteSheetRequest.CoverNoteStatusCode
            oImpRequest.CoverNoteBookTimestamp = UpdateCoverNoteSheetRequest.CoverNoteBookTimestamp

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateCoverNoteSheet(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Set the response values here
                oResponse.CoverNoteBookTimestamp = oImpResponse.CoverNoteBookTimestamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdateCoverNoteSheetRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateCoverNoteSheetRequest))
            Return Nothing
        End Try
    End Function

    Public Function UpdateCoverNoteBook(ByVal oUpdateCoverNoteBookRequest As UpdateCoverNoteBookRequestType) As UpdateCoverNoteBookResponseType Implements IPureCoreService.UpdateCoverNoteBook

        Try


            Dim sUserName As String = oUpdateCoverNoteBookRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUCNBKey", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateCoverNoteBookRequest.WCFSecurityToken)
            Dim oResponse As New UpdateCoverNoteBookResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateCoverNoteBookRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateCoverNoteBookRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateCoverNoteBookResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateCoverNoteBookRequest.BranchCode
            oImpRequest.AgentKeySpecified = oUpdateCoverNoteBookRequest.AgentKeySpecified
            oImpRequest.AgentKey = oUpdateCoverNoteBookRequest.AgentKey
            oImpRequest.CoverNoteBookKey = oUpdateCoverNoteBookRequest.CoverNoteBookKey
            oImpRequest.CoverNoteBookTimestamp = oUpdateCoverNoteBookRequest.CoverNoteBookTimestamp
            oImpRequest.CoverNoteBranchCode = oUpdateCoverNoteBookRequest.CoverNoteBranchCode
            oImpRequest.CoverNoteStatusCode = oUpdateCoverNoteBookRequest.CoverNoteStatusCode
            oImpRequest.EffectiveDate = oUpdateCoverNoteBookRequest.EffectiveDate

            ' Pass the values to the implementation request structure in a loop for CoverNoteProducts collection
            If oUpdateCoverNoteBookRequest.CoverNoteProducts IsNot Nothing Then
                If oUpdateCoverNoteBookRequest.CoverNoteProducts.Count > 0 Then
                    Dim iLength As Int32 = oUpdateCoverNoteBookRequest.CoverNoteProducts.Count
                    oImpRequest.CoverNoteProducts = New BaseImplementationTypes.BaseCoverNoteBookTypeCoverNoteProducts
                    For icount As Integer = 0 To iLength - 1
                        ReDim Preserve oImpRequest.CoverNoteProducts.Row(icount)
                        oImpRequest.CoverNoteProducts.Row(icount) = New BaseImplementationTypes.BaseCoverNoteBookTypeCoverNoteProductsRow
                        oImpRequest.CoverNoteProducts.Row(icount).ProductCode = oUpdateCoverNoteBookRequest.CoverNoteProducts(icount).ProductCode
                    Next
                End If
            End If
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.UpdateCoverNoteBook(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.CoverNoteBookTimestamp = oImpResponse.CoverNoteBookTimestamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateCoverNoteBookRequest))
            End Try
            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateCoverNoteBookRequest))
            Return Nothing
        End Try

    End Function


    Public Function GetUserAuthorityValue(ByVal oGetUserAuthorityValueRequest As GetUserAuthorityValueRequestType) As GetUserAuthorityValueResponseType Implements IPureCoreService.GetUserAuthorityValue, IPureAccountService.GetUserAuthorityValue, IPureClaimService.GetUserAuthorityValue, IPurePartyService.GetUserAuthorityValue, IPurePolicyService.GetUserAuthorityValue

        Try

            Dim sUserName As String = oGetUserAuthorityValueRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGUAV", iUserId)
            CommonFunctions.CheckSecurityToken(oGetUserAuthorityValueRequest.WCFSecurityToken)
            Dim oResponse As New GetUserAuthorityValueResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetUserAuthorityValueRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetUserAuthorityValueRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetUserAuthorityValueResponseType = Nothing

            'Pass the values to the implementation request structure as below for all input parameters 
            oImpRequest.BranchCode = oGetUserAuthorityValueRequest.BranchCode

            If oImpRequest.UserCode <> "" Then
                oImpRequest.UserCode = oGetUserAuthorityValueRequest.UserCode
            Else
                oImpRequest.UserCode = sUserName
            End If

            oImpRequest.UserAuthorityOption = CType([Enum].ToObject(GetType(UserAuthorityOptions), oGetUserAuthorityValueRequest.UserAuthorityOption), BaseImplementationTypes.UserAuthorityOptions)

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetUserAuthorityValue(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.UserAuthorityValue = oImpResponse.UserAuthorityValue
                oResponse.UserAuthorityOptionalValue1Specified = oImpResponse.UserAuthorityOptionalValue1Specified
                oResponse.UserAuthorityOptionalValue1 = oImpResponse.UserAuthorityOptionalValue1
                oResponse.UserAuthorityOptionalValue2Specified = oImpResponse.UserAuthorityOptionalValue2Specified
                oResponse.UserAuthorityOptionalValue2 = oImpResponse.UserAuthorityOptionalValue2
                oResponse.UserAuthorityOptionalValue3Specified = oImpResponse.UserAuthorityOptionalValue3Specified
                oResponse.UserAuthorityOptionalValue3 = oImpResponse.UserAuthorityOptionalValue3
                oResponse.UserAuthorityOptionalValue3_baseAmount = oImpResponse.UserAuthorityOptionalValue3_BaseAmount
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetUserAuthorityValueRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetUserAuthorityValueRequest))
            Return Nothing
        End Try

    End Function

    Public Function ClearCache(ByVal request As ClearCacheRequestType) As ClearCacheResponseType Implements IPureCoreService.ClearCache

        Try
            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            sUserName = request.LoginUserName.ToString
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(request.WCFSecurityToken)
            Dim oResponse As New ClearCacheResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ClearCacheRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ClearCacheResponseType = Nothing

            '  oImpRequest.BatchId = request.BatchId
            oImpRequest.BranchCode = request.BranchCode
            oImpRequest.LoginUserName = request.LoginUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ClearCache(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(request))
            End Try


            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(request))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This  webservice method  GetAccountBalanceByAccountCode  gets ccountBalance details
    '''<param name="oGetAccountBalanceByAccountCodeRequest" type="GetAccountBalanceByAccountCodeRequestType"></param>   
    ''' <returns>GetAccountBalanceByAccountCodeResponseType</returns>  
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetAccountBalanceByAccountCode(ByVal GetAccountBalanceByAccountCodeRequest As GetAccountBalanceByAccountCodeRequestType) As GetAccountBalanceByAccountCodeResponseType Implements IPureCoreService.GetAccountBalanceByAccountCode
        Try

            Dim sUserName As String = GetAccountBalanceByAccountCodeRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGetAccB", iUserId)
            CommonFunctions.CheckSecurityToken(GetAccountBalanceByAccountCodeRequest.WCFSecurityToken)
            Dim oGetAccountBalanceByAccountCodeResponse As New GetAccountBalanceByAccountCodeResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetAccountBalanceByAccountCodeRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAccountBalanceByAccountCodeRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAccountBalanceByAccountCodeResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetAccountBalanceByAccountCodeRequest.BranchCode
            oImpRequest.AccountCode = GetAccountBalanceByAccountCodeRequest.AccountCode
            oImpRequest.RestrictToNonPolicyTransactions = GetAccountBalanceByAccountCodeRequest.RestrictToNonPolicyTransactions

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetAccountBalanceByAccountCode(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oGetAccountBalanceByAccountCodeResponse.AccountBalance = oImpResponse.AccountBalance

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oGetAccountBalanceByAccountCodeResponse, ex, CommonFunctions.CreateDictionary(GetAccountBalanceByAccountCodeRequest))
            End Try

            Return oGetAccountBalanceByAccountCodeResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetAccountBalanceByAccountCodeRequest))
            Return Nothing
        End Try

    End Function

    Public Function ClaimDataImport(ByVal ClaimDataImportRequest As ClaimDataImportRequestType) As ClaimDataImportResponseType Implements IPureCoreService.ClaimDataImport

        Try
            Dim sUserName As String = ClaimDataImportRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCLMData", iUserId)
            CommonFunctions.CheckSecurityToken(ClaimDataImportRequest.WCFSecurityToken)

            Dim serviceResponse As New ClaimDataImportResponseType

            ' Implementation structures
            Dim impRequest As New SAMForInsuranceV2ImplementationTypes.ClaimDataImportRequestType
            Dim impResponse As SAMForInsuranceV2ImplementationTypes.ClaimDataImportResponseType = Nothing

            ' convert the service request to the base request
            ClaimDataImportIn(ClaimDataImportRequest, impRequest)

            Dim oBusiness As DataTransfer.General = New DataTransfer.General()

            Try

                ' call into the business layer
                impResponse = oBusiness.ClaimDataImport(impRequest)

                ' historical check to ensure any sts errors raised are 
                ' actually picked up
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(impResponse, serviceResponse, ex, CommonFunctions.CreateDictionary(ClaimDataImportRequest))

            End Try

            ' build service response from base response
            ClaimDataImportOut(serviceResponse, impResponse)

            Return serviceResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(ClaimDataImportRequest))
            Return Nothing
        End Try

    End Function

    Public Function ProcessClaim(ByVal oClaimProcessRequest As BaseClaimProcessRequestType) As BaseClaimProcessResponseType Implements IPureCoreService.ProcessClaim

        Dim sUserName As String = oClaimProcessRequest.LoginUserName
        Dim iAgentKey As Integer
        Dim iUserId As Integer

        CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
        CommonFunctions.CheckSecurityToken(oClaimProcessRequest.WCFSecurityToken)
        Dim oResponse As New BaseClaimProcessResponseType

        Dim oImpRequest As New BaseImplementationTypes.BaseClaimProcessRequestType
        Dim oImpResponse As BaseImplementationTypes.BaseClaimProcessResponseType
        Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oClaimProcessRequest.BranchCode)

        Try
            oImpRequest.BranchCode = oClaimProcessRequest.BranchCode
            oImpRequest.ExclusiveLock = oClaimProcessRequest.ExclusiveLock
            oImpRequest.SessionValue = oClaimProcessRequest.SessionValue
            oImpRequest.IsMaintainClaim = oClaimProcessRequest.IsMaintainClaim
            oImpRequest.Claim = New BaseImplementationTypes.BaseClaimProcessType
            oImpRequest.Claim.BaseClaimKey = oClaimProcessRequest.Claim.BaseClaimKey
            oImpRequest.ClaimNumber = oClaimProcessRequest.Claim.ClaimNumber
            oImpRequest.Claim.CatastropheCode = oClaimProcessRequest.Claim.CatastropheCode

            oImpRequest.Claim.ClaimStatus = oClaimProcessRequest.Claim.ClaimStatus
            oImpRequest.Claim.ClaimStatusDate = oClaimProcessRequest.Claim.ClaimStatusDate
            oImpRequest.Claim.ClaimVersion = oClaimProcessRequest.Claim.ClaimVersion
            oImpRequest.Claim.ClaimVersionDescription = oClaimProcessRequest.Claim.ClaimVersionDescription

            oImpRequest.Claim.ClientEmail = oClaimProcessRequest.Claim.ClientEmail
            oImpRequest.Claim.ClientFaxNo = oClaimProcessRequest.Claim.ClientFaxNo
            oImpRequest.Claim.ClientMobileNo = oClaimProcessRequest.Claim.ClientMobileNo
            oImpRequest.Claim.ClientName = oClaimProcessRequest.Claim.ClientName

            oImpRequest.Claim.ClientTelNo = oClaimProcessRequest.Claim.ClientTelNo
            oImpRequest.Claim.Comments = oClaimProcessRequest.Claim.Comments
            oImpRequest.Claim.CurrencyCode = oClaimProcessRequest.Claim.CurrencyCode

            oImpRequest.Claim.Description = oClaimProcessRequest.Claim.Description
            oImpRequest.Claim.ExternalHandler = oClaimProcessRequest.Claim.ExternalHandler
            oImpRequest.Claim.HandlerCode = oClaimProcessRequest.Claim.HandlerCode
            oImpRequest.Claim.IgnoreWarnings = oClaimProcessRequest.Claim.IgnoreWarnings
            oImpRequest.Claim.InfoOnly = oClaimProcessRequest.Claim.InfoOnly

            oImpRequest.Claim.InsuranceFileKey = oClaimProcessRequest.Claim.InsuranceFileKey
            oImpRequest.Claim.LastModifiedDate = oClaimProcessRequest.Claim.LastModifiedDate
            oImpRequest.Claim.LikelyClaim = oClaimProcessRequest.Claim.LikelyClaim
            oImpRequest.Claim.LossFromDate = oClaimProcessRequest.Claim.LossFromDate
            oImpRequest.Claim.TPA = oClaimProcessRequest.Claim.TPA
            oImpRequest.Claim.IsTPASettleDirectly = oClaimProcessRequest.Claim.IsTPASettleDirectly
            oImpRequest.Claim.Location = oClaimProcessRequest.Claim.Location
            If oClaimProcessRequest.Claim.CloseClaimOnZeroReserveRecoveryBalanceSpecified Then
                oImpRequest.Claim.CloseClaimOnZeroReserveRecoveryBalance = oClaimProcessRequest.Claim.CloseClaimOnZeroReserveRecoveryBalance
            Else
                oImpRequest.Claim.CloseClaimOnZeroReserveRecoveryBalance = False
            End If
            If oImpRequest.Claim.LossToDateSpecified = False Then
                oImpRequest.Claim.LossToDate = oClaimProcessRequest.Claim.LossFromDate
                oImpRequest.Claim.LossToDateSpecified = True
            Else
                oImpRequest.Claim.LossToDate = oClaimProcessRequest.Claim.LossToDate
                oImpRequest.Claim.LossToDateSpecified = oClaimProcessRequest.Claim.LossToDateSpecified
            End If

            oImpRequest.Claim.PrimaryCauseCode = oClaimProcessRequest.Claim.PrimaryCauseCode
            oImpRequest.Claim.ProgressStatusCode = oClaimProcessRequest.Claim.ProgressStatusCode
            oImpRequest.Claim.ReportedDate = oClaimProcessRequest.Claim.ReportedDate
            oImpRequest.Claim.RiskKey = oClaimProcessRequest.Claim.RiskKey
            oImpRequest.Claim.SecondaryCauseCode = oClaimProcessRequest.Claim.SecondaryCauseCode

            If Not oClaimProcessRequest.Claim.ClaimBuilderDetail Is Nothing Then
                ReDim oImpRequest.Claim.ClaimBuilderDetail(oClaimProcessRequest.Claim.ClaimBuilderDetail.Count - 1)
                For lCount As Int32 = 0 To oClaimProcessRequest.Claim.ClaimBuilderDetail.Count - 1
                    oImpRequest.Claim.ClaimBuilderDetail(lCount) = New BaseImplementationTypes.BaseClaimProcessBuilderRiskType
                    oImpRequest.Claim.ClaimBuilderDetail(lCount).ClaimBuilderData = New BaseImplementationTypes.BaseClaimProcessBuilderRiskTypeClaimBuilderData
                    oImpRequest.Claim.ClaimBuilderDetail(lCount).ClaimBuilderData.ItemName = oClaimProcessRequest.Claim.ClaimBuilderDetail(lCount).ClaimBuilderData.ItemName
                    oImpRequest.Claim.ClaimBuilderDetail(lCount).ClaimBuilderData.Value = oClaimProcessRequest.Claim.ClaimBuilderDetail(lCount).ClaimBuilderData.Value
                Next
            End If
            If Not oClaimProcessRequest.Claim.ClaimPeril Is Nothing Then
                ReDim oImpRequest.Claim.ClaimPeril(oClaimProcessRequest.Claim.ClaimPeril.Count - 1)
                For lCount As Int32 = 0 To oClaimProcessRequest.Claim.ClaimPeril.Count - 1
                    oImpRequest.Claim.ClaimPeril(lCount) = New BaseImplementationTypes.BaseClaimProcessPerilType

                    If oClaimProcessRequest.Claim.ClaimPeril(lCount).Description = String.Empty Then
                        oImpRequest.Claim.ClaimPeril(lCount).Description = oClaimProcessRequest.Claim.ClaimPeril(lCount).TypeCode
                    Else
                        oImpRequest.Claim.ClaimPeril(lCount).Description = oClaimProcessRequest.Claim.ClaimPeril(lCount).Description
                    End If
                    If Not oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery Is Nothing Then
                        ReDim oImpRequest.Claim.ClaimPeril(lCount).Recovery(oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery.Count - 1)
                        For lRCount As Int32 = 0 To oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery.Count - 1
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount) = New BaseImplementationTypes.BaseClaimProcessPerilRecoveryType
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).Amount = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).Amount
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryAmount = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryAmount
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryPartyCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryPartyCode
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryPartyTypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryPartyTypeCode
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).TypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).TypeCode
                            If Not oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails Is Nothing Then
                                oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails = New BaseImplementationTypes.BaseClaimProcessReceiptDetailsType
                                oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptBankCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptBankCode
                                ' If no MediaType passed in then default to Cheque
                                If oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptMediaTypeCode = String.Empty Then
                                    oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptMediaTypeCode = "CQ"
                                Else
                                    oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptMediaTypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptMediaTypeCode
                                End If
                                oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptMediaReference = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptMediaReference
                                oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptPayee = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptPayee
                                oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptPartyType = DirectCast(oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptPartyType, BaseImplementationTypes.ClaimPaymentPartyTypeType)
                                oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptPartyKey = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptPartyKey
                                If String.IsNullOrEmpty(oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptCurrencyCode) Then
                                    oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptCurrencyCode = oClaimProcessRequest.Claim.CurrencyCode
                                Else
                                    oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptCurrencyCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryDetails.ReceiptCurrencyCode
                                End If
                            End If
                            'oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).ReverseExcess = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRCount).ReverseExcess
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).taxGroupCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).taxGroupCode
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).TypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).TypeCode
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).isReceiptToDate = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).isReceiptToDate
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).isRecoverToDate = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).isRecoverToDate
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).IsSalvageRecovery = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).IsSalvageRecovery
                        Next
                    End If
                    If Not oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve Is Nothing Then
                        ReDim oImpRequest.Claim.ClaimPeril(lCount).Reserve(oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve.Count - 1)
                        For lRSCount As Int32 = 0 To oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve.Count - 1
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount) = New BaseImplementationTypes.BaseClaimProcessPerilReserveType
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).Amount = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).Amount

                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentAmount = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentAmount
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentAmountSpecified = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentAmountSpecified
                            If Not oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails Is Nothing Then
                                oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails = New BaseImplementationTypes.BaseClaimProcessPaymentDetailsType
                                oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentBankCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentBankCode
                                ' If no MediaType passed in then default to Cheque
                                If oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaTypeCode = String.Empty Then
                                    oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaTypeCode = "CQ"
                                Else
                                    oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaTypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaTypeCode
                                End If
                                oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaReference = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaReference
                                oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentPayee = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentPayee
                                oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentPartyType = DirectCast(oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentPartyType, BaseImplementationTypes.ClaimPaymentPartyTypeType)
                                oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentPartyKey = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentPartyKey
                                If Not String.IsNullOrEmpty(oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.UltimatePayee) Then
                                    oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.UltimatePayee = oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.UltimatePayee
                                End If
                                If String.IsNullOrEmpty(oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentCurrencyCode) Then
                                    oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentCurrencyCode = oClaimProcessRequest.Claim.CurrencyCode
                                Else
                                    oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentCurrencyCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentCurrencyCode
                                End If
                            End If
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).ReverseExcess = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).ReverseExcess
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).TaxGroupCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).TaxGroupCode
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).TypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).TypeCode
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).IsPaidToDate = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).IsPaidToDate
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).IsReserveToDate = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).IsReserveToDate
                            'oImpRequest.Claim.ClaimPeril(lCount).Reerve(lRSCount).IsTPASettleDirectly = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).IsTPASettleDirectly
                        Next
                    End If
                    oImpRequest.Claim.ClaimPeril(lCount).TypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).TypeCode
                Next
            End If

            If oClaimProcessRequest.Claim.Payee IsNot Nothing Then
                oImpRequest.Claim.Payee = New BaseImplementationTypes.BaseClaimPayeeType
                oImpRequest.Claim.Payee.BankCode = oClaimProcessRequest.Claim.Payee.BankCode
                oImpRequest.Claim.Payee.BankName = oClaimProcessRequest.Claim.Payee.BankName
                oImpRequest.Claim.Payee.BankNumber = oClaimProcessRequest.Claim.Payee.BankNumber
                oImpRequest.Claim.Payee.MediaReference = oClaimProcessRequest.Claim.Payee.MediaReference
                oImpRequest.Claim.Payee.MediaTypeCode = oClaimProcessRequest.Claim.Payee.MediaTypeCode
                oImpRequest.Claim.Payee.Name = oClaimProcessRequest.Claim.Payee.Name
                oImpRequest.Claim.Payee.TheirReference = oClaimProcessRequest.Claim.Payee.TheirReference
                oImpRequest.Claim.Payee.Comments = oClaimProcessRequest.Claim.Payee.Comments
                oImpRequest.Claim.Payee.PartyBankKey = oClaimProcessRequest.Claim.Payee.PartyBankKey

                If oClaimProcessRequest.Claim.Payee.Address IsNot Nothing Then
                    oImpRequest.Claim.Payee.Address = New BaseImplementationTypes.BaseAddressType
                    oImpRequest.Claim.Payee.Address.AddressLine1 = oClaimProcessRequest.Claim.Payee.Address.AddressLine1
                    oImpRequest.Claim.Payee.Address.AddressLine2 = oClaimProcessRequest.Claim.Payee.Address.AddressLine2
                    oImpRequest.Claim.Payee.Address.AddressLine3 = oClaimProcessRequest.Claim.Payee.Address.AddressLine3
                    oImpRequest.Claim.Payee.Address.AddressLine4 = oClaimProcessRequest.Claim.Payee.Address.AddressLine4
                    oImpRequest.Claim.Payee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), oClaimProcessRequest.Claim.Payee.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                    oImpRequest.Claim.Payee.Address.PostCode = oClaimProcessRequest.Claim.Payee.Address.PostCode
                    oImpRequest.Claim.Payee.Address.CountryCode = oClaimProcessRequest.Claim.Payee.Address.CountryCode
                End If
            End If
            If oClaimProcessRequest.Claim.ReceiptPayee IsNot Nothing Then
                oImpRequest.Claim.ReceiptPayee = New BaseImplementationTypes.BaseClaimReceiptPayeeType
                oImpRequest.Claim.ReceiptPayee.BankCode = oClaimProcessRequest.Claim.ReceiptPayee.BankCode
                oImpRequest.Claim.ReceiptPayee.BankName = oClaimProcessRequest.Claim.ReceiptPayee.BankName
                oImpRequest.Claim.ReceiptPayee.BankNumber = oClaimProcessRequest.Claim.ReceiptPayee.BankNumber
                oImpRequest.Claim.ReceiptPayee.MediaReference = oClaimProcessRequest.Claim.ReceiptPayee.MediaReference
                oImpRequest.Claim.ReceiptPayee.MediaTypeCode = oClaimProcessRequest.Claim.ReceiptPayee.MediaTypeCode
                oImpRequest.Claim.ReceiptPayee.Name = oClaimProcessRequest.Claim.ReceiptPayee.Name
                oImpRequest.Claim.ReceiptPayee.TheirReference = oClaimProcessRequest.Claim.ReceiptPayee.TheirReference
                oImpRequest.Claim.ReceiptPayee.Comments = oClaimProcessRequest.Claim.ReceiptPayee.Comments
                oImpRequest.Claim.ReceiptPayee.PartyBankKey = oClaimProcessRequest.Claim.ReceiptPayee.PartyBankKey

                If oClaimProcessRequest.Claim.ReceiptPayee.Address IsNot Nothing Then
                    oImpRequest.Claim.ReceiptPayee.Address = New BaseImplementationTypes.BaseAddressType
                    oImpRequest.Claim.ReceiptPayee.Address.AddressLine1 = oClaimProcessRequest.Claim.ReceiptPayee.Address.AddressLine1
                    oImpRequest.Claim.ReceiptPayee.Address.AddressLine2 = oClaimProcessRequest.Claim.ReceiptPayee.Address.AddressLine2
                    oImpRequest.Claim.ReceiptPayee.Address.AddressLine3 = oClaimProcessRequest.Claim.ReceiptPayee.Address.AddressLine3
                    oImpRequest.Claim.ReceiptPayee.Address.AddressLine4 = oClaimProcessRequest.Claim.ReceiptPayee.Address.AddressLine4
                    oImpRequest.Claim.ReceiptPayee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), oClaimProcessRequest.Claim.ReceiptPayee.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                    oImpRequest.Claim.ReceiptPayee.Address.PostCode = oClaimProcessRequest.Claim.ReceiptPayee.Address.PostCode
                    oImpRequest.Claim.ReceiptPayee.Address.CountryCode = oClaimProcessRequest.Claim.ReceiptPayee.Address.CountryCode
                End If
            End If
            Try
                'Calling base function
                oImpResponse = oBusiness.ProcessClaim(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.BaseClaimKey = oImpResponse.BaseClaimKey
                oResponse.ClaimKey = oImpResponse.ClaimKey
                oResponse.ClaimNumber = oImpResponse.ClaimNumber
                If (oImpResponse.Warnings IsNot Nothing) Then

                    oResponse.Warnings = oImpResponse.Warnings.ToList().ConvertAll(
                         New Converter(Of BaseImplementationTypes.BaseClaimProcessResponseTypeWarnings, BaseClaimProcessResponseTypeWarnings)(AddressOf CommonFunctions.ToBaseClaimProcessResponseTypeWarningsList))

                End If
                oResponse.ResultingStatus = oImpResponse.ResultingStatus
                oResponse.TimeStamp = oImpResponse.TimeStamp
                oResponse.Version = oImpResponse.Version
                Return oResponse

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oClaimProcessRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oClaimProcessRequest))
            Return Nothing
        End Try

    End Function

#Region "GetCaseDetails"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetCaseDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCaseDetails(ByVal oGetCaseDetailsRequest As GetCaseDetailsRequestType) As GetCaseDetailsResponseType Implements IPureCoreService.GetCaseDetails

        Try



            Dim sUserName As String = oGetCaseDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCASE", iUserId)
            CommonFunctions.CheckSecurityToken(oGetCaseDetailsRequest.WCFSecurityToken)

            Dim oResponse As New GetCaseDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetCaseDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCaseDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCaseDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetCaseDetailsRequest.BranchCode
            oImpRequest.CaseKey = oGetCaseDetailsRequest.CaseKey
            oImpRequest.CaseKeySpecified = oGetCaseDetailsRequest.CaseKeySpecified
            oImpRequest.CaseNumber = oGetCaseDetailsRequest.CaseNumber
            oImpRequest.Analyst = oGetCaseDetailsRequest.Analyst
            oImpRequest.Assistant = oGetCaseDetailsRequest.Assistant
            oImpRequest.CaseOpenDate = oGetCaseDetailsRequest.CaseOpenDate
            oImpRequest.ProgressStatusCode = oGetCaseDetailsRequest.ProgressStatusCode

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetCaseDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.LinkedClaims = SAMFunc.GetDeserializedValues(Of List(Of BaseCaseItemsResponseTypeLinkedClaimsRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseCaseItemsResponseTypeLinkedClaims", sConvertToTypeName:="BaseCaseItemsResponseTypeLinkedClaimsRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.LinkedClaims = DataTabletoList_CaseItemsLinkedClaims(oImpResponse.ResultData.Tables(0))
                End If
                oResponse.Analyst = oImpResponse.Analyst
                oResponse.Assistant = oImpResponse.Assistant
                oResponse.BaseCaseKey = oImpResponse.BaseCaseKey
                oResponse.CaseKey = oImpResponse.CaseKey
                oResponse.CaseNumber = oImpResponse.CaseNumber
                oResponse.CaseOpenedDate = oImpResponse.CaseOpenedDate
                oResponse.CaseProgressDescription = oImpResponse.CaseProgressDescription
                oResponse.CaseProgressStatusCode = oImpResponse.CaseProgressStatusCode
                oResponse.CaseVersion = oImpResponse.CaseVersion

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetCaseDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetCaseDetailsRequest))
            Return Nothing
        End Try

    End Function
#End Region

    ''' <summary>
    ''' to settle multiple claim payments in one go
    ''' </summary>
    ''' <param name="oSettleAllClaimPaymentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SettleAllClaimPayments(ByVal oSettleAllClaimPaymentsRequest As SettleAllClaimPaymentsRequestType) As SettleAllClaimPaymentsResponseType Implements IPureCoreService.SettleAllClaimPayments
        Try

            Dim sUserName As String = oSettleAllClaimPaymentsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMSETALL", iUserId)
            CommonFunctions.CheckSecurityToken(oSettleAllClaimPaymentsRequest.WCFSecurityToken)

            Dim oResponse As New SettleAllClaimPaymentsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(oSettleAllClaimPaymentsRequest.LoginUserName, oSettleAllClaimPaymentsRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.SettleAllClaimPaymentsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.SettleAllClaimPaymentsResponseType = Nothing

            ' Pass the values to the implementation request structure
            Try
                oImpRequest.BranchCode = oSettleAllClaimPaymentsRequest.BranchCode
                Dim oReqClaimPaymentsItem As BaseImplementationTypes.BaseSettleClaimPaymentType
                oImpRequest.claimPayments = New System.Collections.Generic.List(Of BaseImplementationTypes.BaseSettleClaimPaymentType)
                For Each oPayment As BaseSettleClaimPaymentType In oSettleAllClaimPaymentsRequest.ClaimPayments
                    oReqClaimPaymentsItem = New BaseImplementationTypes.BaseSettleClaimPaymentType
                    With oReqClaimPaymentsItem
                        .AccountCode = oPayment.AccountCode
                        .BankAccountCode = oPayment.BankAccountCode
                        .ClaimPaymentAmount = oPayment.ClaimPaymentAmount
                        .ClaimPaymentBranchCode = oPayment.ClaimPaymentBranchCode
                        .ClaimPaymentKey = oPayment.ClaimPaymentKey
                        .CurrencyCode = oPayment.CurrencyCode
                        .DocumentKey = oPayment.DocumentKey
                        .MediaTypeCode = oPayment.MediaTypeCode
                        .OurRef = oPayment.OurRef
                        .PayeeName = oPayment.PayeeName
                        .DocumentRef = oPayment.DocumentRef
                    End With
                    oImpRequest.claimPayments.Add(oReqClaimPaymentsItem)
                Next

                oImpResponse = oBusiness.SettleAllClaimPayments(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.Summary IsNot Nothing AndAlso oImpResponse.Summary.Count > 0 Then
                    oResponse.Summary = oImpResponse.Summary.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseSettleAllClaimPaymentSummaryResponseType, BaseSettleAllClaimPaymentSummaryResponseType)(AddressOf CommonFunctions.ToServiceSettleAllClaimPaymentsSummaryList))
                End If
                If oImpResponse.Warnings IsNot Nothing AndAlso oImpResponse.Warnings.Count > 0 Then
                    oResponse.Warnings = oImpResponse.Warnings.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGeneralWarningResponseType, BaseGeneralWarningResponseType)(AddressOf CommonFunctions.ToServiceWarningTypeList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oSettleAllClaimPaymentsRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oSettleAllClaimPaymentsRequest))
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' This Web method is used to Attach a cover note by passing the Request parameters as request type objects
    ''' and also the response object  is being returned .
    '''</summary>
    '''<param name="oGetNumberingSchemeNo" type="GetNumberingSchemeNoRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsurance.GetNumberingSchemeNoResponseType</returns>  
    '''<remarks></remarks>

    Public Function GetNumberingSchemeNo(ByVal oGetNumberingSchemeNo As GetNumberingSchemeNoRequestType) As GetNumberingSchemeNoResponseType Implements IPureCoreService.GetNumberingSchemeNo

        Try
            'Assign appropriate key


            Dim sUserName As String = oGetNumberingSchemeNo.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGNCNO", iUserId)
            CommonFunctions.CheckSecurityToken(oGetNumberingSchemeNo.WCFSecurityToken)
            Dim oResponse As New GetNumberingSchemeNoResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetNumberingSchemeNo.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetNumberingSchemeNoRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetNumberingSchemeNoResponseType = Nothing
            Dim oCoverNote As New BaseCoverNoteRiskItemType

            'Pass the values to the implementation request structure as below for all input parameters 
            oImpRequest.BranchCode = oGetNumberingSchemeNo.BranchCode
            oImpRequest.ProductKey = oGetNumberingSchemeNo.ProductKey
            oImpRequest.AgentKey = oGetNumberingSchemeNo.AgentKey
            oImpRequest.SchemeType = CType([Enum].ToObject(GetType(NumberingSchemeType), oGetNumberingSchemeNo.SchemeType), BaseImplementationTypes.NumberingSchemeType)

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetNumberingSchemeNo(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.FailureReason = oImpResponse.FailureReason
                oResponse.GeneratedCode = oImpResponse.GeneratedCode

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetNumberingSchemeNo))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetNumberingSchemeNo))
            Return Nothing
        End Try

    End Function

    Public Function ClientDataImport(ByVal ClientDataImportRequest As ClientDataImportRequestType) As ClientDataImportResponseType Implements IPureCoreService.ClientDataImport

        Try
            Dim sUserName As String = ClientDataImportRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCliData", iUserId)
            CommonFunctions.CheckSecurityToken(ClientDataImportRequest.WCFSecurityToken)
            Dim msgResponse As New ClientDataImportResponseType

            ' Implementation structures
            Dim impRequest As New SAMForInsuranceV2ImplementationTypes.ClientDataImportRequestType()
            Dim impResponse As SAMForInsuranceV2ImplementationTypes.ClientDataImportResponseType

            ' Convert the incoming interface structures into the implementation structures
            If ClientDataImportRequest.AgentKeySpecified Then
                impRequest.AgentKey = ClientDataImportRequest.AgentKey
            End If

            impRequest.BranchCode = ClientDataImportRequest.BranchCode

            ' Process the Party structure.  We 1st need to check the party type of the incoming message
            If (ClientDataImportRequest.Item Is Nothing) = False Then

                Dim impParty As New BaseImplementationTypes.BasePartyType
                Dim impAddress As New BaseImplementationTypes.BaseAddressWithContactsType
                Dim impContact As New BaseImplementationTypes.BaseContactType

                ' Check the type of the party object to see if it is Personal or Corporate
                If ClientDataImportRequest.Item.GetType Is GetType(BasePartyPCType) Then

                    ' Process personal client
                    Dim msgParty As BasePartyPCType = DirectCast(ClientDataImportRequest.Item, BasePartyPCType)
                    Dim impPartyPC As New BaseImplementationTypes.BasePartyPCType

                    impPartyPC.Forename = msgParty.Forename
                    impPartyPC.Surname = msgParty.Surname
                    impPartyPC.Initials = msgParty.Initials
                    impPartyPC.Title = msgParty.Title
                    impPartyPC.DateOfBirth = msgParty.DateOfBirth
                    impPartyPC.GenderCode = msgParty.GenderCode
                    impPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MaritalStatusCodeType), msgParty.MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)
                    impPartyPC.OccupationCode = msgParty.OccupationCode
                    impPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), msgParty.EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                    impPartyPC.EmployersBusinessCode = msgParty.EmployersBusinessCode
                    impPartyPC.AlternativeId = msgParty.AlternativeId

                    impParty = impPartyPC

                Else

                    ' Process corporate client
                    Dim msgParty As BasePartyCCType = DirectCast(ClientDataImportRequest.Item, BasePartyCCType)
                    Dim impPartyCC As New BaseImplementationTypes.BasePartyCCType

                    impPartyCC.CompanyName = msgParty.CompanyName
                    impPartyCC.BusinessCode = msgParty.BusinessCode
                    impPartyCC.MainContact = msgParty.MainContact
                    impPartyCC.NumberOfEmployees = msgParty.NumberOfEmployees.ToString
                    impPartyCC.NumberOfOffices = msgParty.NumberOfOffices

                    impParty = impPartyCC

                End If

                ' Common to PC and CC
                impParty.BranchCode = ClientDataImportRequest.Item.BranchCode
                impParty.Currency = ClientDataImportRequest.Item.Currency
                impParty.TPUserCode = ClientDataImportRequest.Item.TPUserCode
                impParty.TPIntroducer = ClientDataImportRequest.Item.TPIntroducer
                impParty.AccountExecutive = ClientDataImportRequest.Item.AccountExecutive

                impParty.AccountExecutiveCode = ClientDataImportRequest.Item.AccountExecutiveCode
                impParty.SubBranchCode = ClientDataImportRequest.Item.SubBranchCode

                impParty.DomiciledForTax = ClientDataImportRequest.Item.DomiciledForTax
                impParty.TaxExempt = ClientDataImportRequest.Item.TaxExempt
                impParty.TaxNumber = ClientDataImportRequest.Item.TaxNumber
                impParty.TaxPercentage = ClientDataImportRequest.Item.TaxPercentage
                impParty.XMLDataset = ClientDataImportRequest.Item.XMLDataset
                impParty.FileCode = ClientDataImportRequest.Item.FileCode

                If ClientDataImportRequest.Item.Addresses IsNot Nothing AndAlso ClientDataImportRequest.Item.Addresses.Count > 0 Then
                    ' Process the address structure
                    impParty.Addresses = Array.ConvertAll(ClientDataImportRequest.Item.Addresses.ToArray(),
                                            New Converter(Of BaseAddressWithContactsType,
                                            BaseImplementationTypes.BaseAddressWithContactsType) _
                                            (AddressOf CommonFunctions.ToBaseImpBaseAddressType))
                End If

                If ClientDataImportRequest.Item.Contacts IsNot Nothing AndAlso ClientDataImportRequest.Item.Contacts.Count > 0 Then
                    ' Process the Contact structure
                    impParty.Contacts = Array.ConvertAll(ClientDataImportRequest.Item.Contacts.ToArray(),
                                        New Converter(Of BaseContactType,
                                        BaseImplementationTypes.BaseContactType) _
                                        (AddressOf CommonFunctions.ToBaseImpBaseContactType))
                End If
                impRequest.Party = impParty

            End If

            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, ClientDataImportRequest.BranchCode)

            Try

                'Call into the buisiness layer
                impResponse = oBusiness.ClientDataImport(impRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)

                msgResponse.PartyKey = impResponse.PartyKey

                If impResponse.PolicyVersion IsNot Nothing Then
                    ' Process the Policy Version structure
                    If IsArray(impResponse.PolicyVersion) = True Then
                        msgResponse.PolicyVersion = impResponse.PolicyVersion.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersion, BaseClientDataImportResponseTypePolicyVersion)(AddressOf CommonFunctions.ToServiceBaseClientDataImportResponseTypePolicyVersion))
                    End If
                End If

                If impResponse.AccountsDocuments IsNot Nothing Then
                    ' Process the Policy Version structure
                    If IsArray(impResponse.AccountsDocuments) = True Then
                        msgResponse.AccountsDocuments = impResponse.AccountsDocuments.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClientDataImportResponseTypeAccountsDocuments, BaseClientDataImportResponseTypeAccountsDocuments)(AddressOf CommonFunctions.ToServiceBaseClientDataImportResponseTypeAccountsDocuments))
                    End If
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(impResponse, msgResponse, ex, CommonFunctions.CreateDictionary(ClientDataImportRequest))
            End Try

            Return msgResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(ClientDataImportRequest))
            Return Nothing
        End Try

    End Function



    Public Function UpdatePartyRisk(ByVal UpdatePartyRiskRequest As UpdatePartyRiskRequestType) As UpdatePartyRiskResponseType Implements IPureCoreService.UpdatePartyRisk

        Try
            Dim sUserName As String = UpdatePartyRiskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMPtyRUpd", iUserId)
            CommonFunctions.CheckSecurityToken(UpdatePartyRiskRequest.WCFSecurityToken)

            Dim serviceResponse As New UpdatePartyRiskResponseType

            ' Implementation structures
            Dim impRequest As New SAMForInsuranceV2ImplementationTypes.UpdatePartyRiskRequestType
            Dim impResponse As SAMForInsuranceV2ImplementationTypes.UpdatePartyRiskResponseType = Nothing

            ' convert the service request to the base request
            UpdatePartyRiskIn(UpdatePartyRiskRequest, impRequest)

            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdatePartyRiskRequest.BranchCode)
            Dim coreXBuilderParty As New CoreImplementation.XBuilder.Party()

            Try

                ' call into the business layer
                impResponse = coreXBuilderParty.UpdateDataSet(impRequest)

                ' historical check to ensure any sts errors raised are 
                ' actually picked up
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)

            Catch ex As Exception

                CommonFunctions.BusinessLayerBoundary(impResponse, serviceResponse, ex, CommonFunctions.CreateDictionary(UpdatePartyRiskRequest))

            End Try

            ' build service response from base response
            UpdatePartyRiskOut(serviceResponse, impResponse)

            Return serviceResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdatePartyRiskRequest))
            Return Nothing
        End Try

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oDeleteBackDatedVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteBackDatedVersions(ByVal oDeleteBackDatedVersionsRequest As DeleteBackDatedVersionsRequestType) As DeleteBackDatedVersionsResponseType Implements IPureCoreService.DeleteBackDatedVersions

        Try

            Dim sUserName As String = oDeleteBackDatedVersionsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAMtaQte", iUserId)
            CommonFunctions.CheckSecurityToken(oDeleteBackDatedVersionsRequest.WCFSecurityToken)
            Dim oResponse As New DeleteBackDatedVersionsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oDeleteBackDatedVersionsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.DeleteBackDatedVersionsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.DeleteBackDatedVersionsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oDeleteBackDatedVersionsRequest.BranchCode
            oImpRequest.InsuranceFileKey = oDeleteBackDatedVersionsRequest.InsuranceFileKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.DeleteBackDatedVersions(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oDeleteBackDatedVersionsRequest))
            End Try
            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oDeleteBackDatedVersionsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GenerateDocumentsForEventRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateDocumentsForEvent(ByVal GenerateDocumentsForEventRequest As GenerateDocumentsForEventRequestType) As GenerateDocumentsForEventResponseType Implements IPureCoreService.GenerateDocumentsForEvent

        Try

            Dim sUserName As String = GenerateDocumentsForEventRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGDOCE", iUserId)
            CommonFunctions.CheckSecurityToken(GenerateDocumentsForEventRequest.WCFSecurityToken)
            Dim oResponse As New GenerateDocumentsForEventResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GenerateDocumentsForEventRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GenerateDocumentsForEventRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GenerateDocumentsForEventResponseType = Nothing

            ' Pass the values to the implementation request structure
            With GenerateDocumentsForEventRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.InsuranceFileKey = .InsuranceFileKey
                oImpRequest.ProcessTypeCode = .ProcessTypeCode
                oImpRequest.GenerateDocuments = .GenerateDocuments
            End With

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GenerateDocumentsForEvent(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.Documents IsNot Nothing AndAlso oImpResponse.Documents.Count > 0 Then

                    oResponse.Documents = oImpResponse.Documents.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseGenerateDocumentsForEventResponseDocument, BaseGenerateDocumentsForEventResponseDocument)(AddressOf CommonFunctions.ToServiceGenerateDocumentsForEvent))

                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GenerateDocumentsForEventRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GenerateDocumentsForEventRequest))
            Return Nothing
        End Try

    End Function


    ''' <summary>  
    ''' Retrieves Policy related amount Information for all the live versions
    ''' </summary>  
    ''' <param name="GetAllLivePolicyVersionAmountsRequest">An object of type SiriusFS.SAM.Structure.SFI.SAMForInsurancev2.GetAllLivePolicyVersionAmountsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.Structure.SFI.SAMForInsurancev2.GetAllLivePolicyVersionAmountsResponseType</returns>  
    Public Function GetAllLivePolicyVersionAmounts(ByVal GetAllLivePolicyVersionAmountsRequest As GetAllLivePolicyVersionAmountsRequestType) As GetAllLivePolicyVersionAmountsResponseType Implements IPureCoreService.GetAllLivePolicyVersionAmounts

        Try
            Dim sUserName As String = GetAllLivePolicyVersionAmountsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGALPVA", iUserId)
            CommonFunctions.CheckSecurityToken(GetAllLivePolicyVersionAmountsRequest.WCFSecurityToken)
            Dim oResponse As New GetAllLivePolicyVersionAmountsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetAllLivePolicyVersionAmountsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAllLivePolicyVersionAmountsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAllLivePolicyVersionAmountsResponseType = Nothing

            ' Pass the values to the implementation request structure
            With GetAllLivePolicyVersionAmountsRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.InsuranceFolderKey = .InsuranceFolderKey
                oImpRequest.InsuranceFileKey = .InsuranceFileKey
            End With

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetAllLivePolicyVersionAmounts(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Get values from implementation layer
                With oImpResponse
                    If .LivePolicyVerisonDetails IsNot Nothing Then
                        If IsArray(.LivePolicyVerisonDetails) Then
                            oResponse.LivePolicyVerisonDetails = .LivePolicyVerisonDetails.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseLivePolicyAmountDetailsType, BaseLivePolicyAmountDetailsType)(AddressOf CommonFunctions.ToServiceLivePolicyVerisonDetailsList))
                        End If
                    End If
                End With
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetAllLivePolicyVersionAmountsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetAllLivePolicyVersionAmountsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetBalanceForCDRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBalanceForCD(ByVal oGetBalanceForCDRequest As GetBalanceForCDRequestType) As GetBalanceForCDResponseType Implements IPureCoreService.GetBalanceForCD

        Try

            Dim sUserName As String = oGetBalanceForCDRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGETPCDS", iUserId)
            CommonFunctions.CheckSecurityToken(oGetBalanceForCDRequest.WCFSecurityToken)
            Dim oResponse As New GetBalanceForCDResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetBalanceForCDRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetBalanceForCDRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetBalanceForCDResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetBalanceForCDRequest.BranchCode
            oImpRequest.CashDepositRef = oGetBalanceForCDRequest.CashDepositRef
            oImpRequest.InsuranceFileKey = oGetBalanceForCDRequest.InsuranceFileKey

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetBalanceForCD(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                With oResponse
                    .RunningBalance = oImpResponse.RunningBalance
                    .AvailableBalance = oImpResponse.AvailableBalance
                End With

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetBalanceForCDRequest))
                'oResponse = Nothing
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetBalanceForCDRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ProcessPaymentAndBindQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessPaymentAndBindQuote(ByVal ProcessPaymentAndBindQuoteRequest As ProcessPaymentAndBindQuoteRequestType) As ProcessPaymentAndBindQuoteResponseType Implements IPureCoreService.ProcessPaymentAndBindQuote

        Try
            Dim sUserName As String = ProcessPaymentAndBindQuoteRequest.LoginUserName
            Dim iUserId As Int32 = 0
            Dim iAgentKey As Int32 = 0


            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMBQuote", iUserId)
            CommonFunctions.CheckSecurityToken(ProcessPaymentAndBindQuoteRequest.WCFSecurityToken)
            Dim oResponse As New ProcessPaymentAndBindQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, ProcessPaymentAndBindQuoteRequest.BranchCode)
            Dim oErrorBusinessRule As New SFI.SAMForInsuranceV2.SAMErrorBusinessRule
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ProcessPaymentAndBindQuoteRequestType
            Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.ProcessPaymentAndBindQuoteResponseType


            oImpRequest.AgentKey = iAgentKey

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = ProcessPaymentAndBindQuoteRequest.BranchCode
            oImpRequest.InsuranceFileKey = ProcessPaymentAndBindQuoteRequest.InsuranceFileKey
            oImpRequest.PaymentMethod = CType(ProcessPaymentAndBindQuoteRequest.PaymentMethod, BaseImplementationTypes.PaymentMethodType)
            oImpRequest.PaymentMethodSpecified = ProcessPaymentAndBindQuoteRequest.PaymentMethodSpecified
            oImpRequest.TransactionType = ProcessPaymentAndBindQuoteRequest.TransactionType

            oImpRequest.InstalmentType = CType([Enum].ToObject(GetType(InstalmentType), ProcessPaymentAndBindQuoteRequest.InstalmentType), BaseImplementationTypes.InstalmentType)
            oImpRequest.InstalmentTypeSpecified = ProcessPaymentAndBindQuoteRequest.InstalmentTypeSpecified

            If ProcessPaymentAndBindQuoteRequest.AcceptRenewalSpecified Then
                oImpRequest.AcceptRenewal = ProcessPaymentAndBindQuoteRequest.AcceptRenewal
            End If
            'Deviation from spec: quote time stamp is also assigned to implementation request type
            oImpRequest.QuoteTimeStamp = ProcessPaymentAndBindQuoteRequest.QuoteTimeStamp

            If ProcessPaymentAndBindQuoteRequest.CoverStartDateSpecified Then
                oImpRequest.CoverStartDate = ProcessPaymentAndBindQuoteRequest.CoverStartDate
            End If

            oImpRequest.IsBackDatedMTA = ProcessPaymentAndBindQuoteRequest.IsBackDatedMTA

            ' default transaction type to new business if not specified
            If String.IsNullOrEmpty(oImpRequest.TransactionType) Then
                oImpRequest.TransactionType = "NB"
            End If

            oImpRequest.SelectedInstalmentQuoteSpecified = False

            If ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote IsNot Nothing Then
                oImpRequest.SelectedInstalmentQuoteSpecified = True
                oImpRequest.SelectedInstalmentQuote = New BaseImplementationTypes.BaseSelectedInstalmentQuoteType
                oImpRequest.SelectedInstalmentQuote.BankAccountName = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAccountName
                oImpRequest.SelectedInstalmentQuote.BankAccountNo = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAccountNo
                oImpRequest.SelectedInstalmentQuote.BankSortCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankSortCode
                oImpRequest.SelectedInstalmentQuote.BIC = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BIC
                oImpRequest.SelectedInstalmentQuote.IBAN = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.IBAN
                oImpRequest.SelectedInstalmentQuote.BankAddress = New BaseImplementationTypes.BaseAddressType
                If ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote IsNot Nothing Then
                    If ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress IsNot Nothing Then
                        oImpRequest.SelectedInstalmentQuote.BankAddress.AddressLine1 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine1
                        oImpRequest.SelectedInstalmentQuote.BankAddress.AddressLine2 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine2
                        oImpRequest.SelectedInstalmentQuote.BankAddress.AddressLine3 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine3
                        oImpRequest.SelectedInstalmentQuote.BankAddress.AddressLine4 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine4
                        oImpRequest.SelectedInstalmentQuote.BankAddress.AddressTypeCode = CType(ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressTypeCode, BaseImplementationTypes.AddressTypeType)
                        oImpRequest.SelectedInstalmentQuote.BankAddress.CountryCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.CountryCode
                        oImpRequest.SelectedInstalmentQuote.BankAddress.PostCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAddress.PostCode
                    End If
                    oImpRequest.SelectedInstalmentQuote.BankAreaCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankAreaCode
                    oImpRequest.SelectedInstalmentQuote.BankBranch = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankBranch
                    oImpRequest.SelectedInstalmentQuote.BankExtn = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankExtn
                    oImpRequest.SelectedInstalmentQuote.BankFax = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankFax
                    oImpRequest.SelectedInstalmentQuote.BankFaxCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankFaxCode
                    oImpRequest.SelectedInstalmentQuote.BankName = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankName
                    oImpRequest.SelectedInstalmentQuote.BankPhone = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.BankPhone
                    oImpRequest.SelectedInstalmentQuote.SelectedSchemeNo = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.SelectedSchemeNo
                    oImpRequest.SelectedInstalmentQuote.SelectedSchemeVersion = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.SelectedSchemeVersion
                    oImpRequest.SelectedInstalmentQuote.QuoteDate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.QuoteDate.Date
                    oImpRequest.SelectedInstalmentQuote.StartDate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.StartDate.Date
                    oImpRequest.SelectedInstalmentQuote.EndDate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.EndDate.Date
                    oImpRequest.SelectedInstalmentQuote.PreferredDate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.PreferredDate.Date
                    oImpRequest.SelectedInstalmentQuote.MonthDay = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.MonthDay
                    oImpRequest.SelectedInstalmentQuote.WeekDay = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.WeekDay
                    oImpRequest.SelectedInstalmentQuote.AmountToFinance = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.AmountToFinance
                    oImpRequest.SelectedInstalmentQuote.PaymentProtection = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.PaymentProtection
                    oImpRequest.SelectedInstalmentQuote.OverrideRate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.OverrideRate
                    oImpRequest.SelectedInstalmentQuote.OverrideInterestRate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.OverrideInterestRate
                    oImpRequest.SelectedInstalmentQuote.AmountPaid = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.AmountPaid
                    oImpRequest.SelectedInstalmentQuote.PFRF_ID = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.PFRF_ID

                    oImpRequest.SelectedInstalmentQuote.PartyBankKey = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.PartyBankKey


                    ' if the payment method was credit card then also retrieve any credit card details 
                    ' passed in the request
                    If oImpRequest.PaymentMethodSpecified AndAlso
                        oImpRequest.PaymentMethod = BaseImplementationTypes.PaymentMethodType.CreditCard AndAlso
                                ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard IsNot Nothing Then

                        oImpRequest.SelectedInstalmentQuote.CreditCard = New BaseImplementationTypes.BaseCreditCardType

                        oImpRequest.SelectedInstalmentQuote.CreditCard.ExpiryDate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.ExpiryDate
                        oImpRequest.SelectedInstalmentQuote.CreditCard.Issue = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.Issue
                        oImpRequest.SelectedInstalmentQuote.CreditCard.NameOnCreditCard = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.NameOnCreditCard
                        oImpRequest.SelectedInstalmentQuote.CreditCard.Number = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.Number
                        oImpRequest.SelectedInstalmentQuote.CreditCard.Pin = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.Pin
                        oImpRequest.SelectedInstalmentQuote.CreditCard.StartDate = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.StartDate
                        oImpRequest.SelectedInstalmentQuote.CreditCard.TypeCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.TypeCode
                        oImpRequest.SelectedInstalmentQuote.CreditCard.AuthCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.AuthCode
                        oImpRequest.SelectedInstalmentQuote.CreditCard.TrackingNumber = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.TrackingNumber
                        oImpRequest.SelectedInstalmentQuote.CreditCard.AccountType = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.AccountType
                        oImpRequest.SelectedInstalmentQuote.CreditCard.PartyBankKey = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.PartyBankKey

                        ' retrieve the credit card cardholder details if provided
                        oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder = New BaseImplementationTypes.BaseCreditCardTypeCardHolder
                        If ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder IsNot Nothing Then
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.Name = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.Name
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine1 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine1
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine2 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine2
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine3 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine3
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine4 = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine4
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.CountryCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.CountryCode
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.PostCode = ProcessPaymentAndBindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.PostCode
                        End If

                    End If

                End If
            End If


            oImpRequest.WritePolicy = ProcessPaymentAndBindQuoteRequest.WritePolicy
            oImpRequest.OverriddenPolicyNumber = ProcessPaymentAndBindQuoteRequest.OverriddenPolicyNumber
            oImpRequest.InstalmentDepositAmount = ProcessPaymentAndBindQuoteRequest.InstalmentDepositAmount
            oImpRequest.PaymentGatewayToken = ProcessPaymentAndBindQuoteRequest.PaymentGatewayToken
            oImpRequest.DepositPartialCCNumber = ProcessPaymentAndBindQuoteRequest.DepositPartialCCNumber
            oImpRequest.DepositCCAuthCode = ProcessPaymentAndBindQuoteRequest.DepositCCAuthCode

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ProcessPaymentAndBindQuote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Return details
                oResponse.Policy = New BaseTransactResponseTypePolicy

                If oImpResponse.Policy IsNot Nothing Then

                    oResponse.Policy.CommissionAmount = oImpResponse.Policy.CommissionAmount
                    oResponse.Policy.PolicyRef = oImpResponse.Policy.PolicyRef
                    oResponse.Policy.PremiumDueGross = oImpResponse.Policy.PremiumDueGross
                    oResponse.Policy.PremiumDueNet = oImpResponse.Policy.PremiumDueNet
                    oResponse.Policy.PremiumDueTax = oImpResponse.Policy.PremiumDueTax
                    oResponse.Policy.TotalAnnualTax = oImpResponse.Policy.TotalAnnualTax
                    oResponse.Policy.DocumentComment = oImpResponse.Policy.DocumentComment
                    oResponse.Policy.AutoGeneratedPlanRef = oImpResponse.Policy.AutoGeneratedPlanRef
                    oResponse.Policy.DepositTransDetailID = oImpResponse.Policy.DepositTransDetailID
                    'Note:For MTA and MTC, Multiple policy details will be returned

                    If oImpResponse.Policy.MultiplePolicies IsNot Nothing AndAlso oImpResponse.Policy.MultiplePolicies.Count > 0 Then
                        oResponse.Policy.MultiplePolicies = oImpResponse.Policy.MultiplePolicies.ToList().ConvertAll(
                            New Converter(Of BaseImplementationTypes.BaseTransactResponseTypePolicyMultiplePolicies, BaseTransactResponseTypePolicyMultiplePolicies) _
                                                            (AddressOf CommonFunctions.ToServiceMultiplePoliciesList))

                    End If

                    oResponse.Policy.PolicyLevelTaxesAndFees = CommonFunctions.ToServiceTaxesAndFeesTypeList(oImpResponse.Policy.PolicyLevelTaxesAndFees)

                End If


            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(ProcessPaymentAndBindQuoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(ProcessPaymentAndBindQuoteRequest))
            Return Nothing
        End Try

    End Function

    Public Function TransferQuote(ByVal oTransferQuoteRequest As TransferQuoteRequestType) As TransferQuoteResponseType Implements IPureCoreService.TransferQuote

        Try

            Dim sUserName As String = oTransferQuoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMTQUOT", iUserId)
            CommonFunctions.CheckSecurityToken(oTransferQuoteRequest.WCFSecurityToken)
            Dim oResponse As New TransferQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oTransferQuoteRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.TransferQuoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.TransferQuoteResponseType = Nothing

            'TODO:-Assign the frontend values

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oTransferQuoteRequest.BranchCode
            oImpRequest.InsuranceFileKey = oTransferQuoteRequest.InsuranceFileKey
            oImpRequest.PartyFromKey = oTransferQuoteRequest.PartyFromKey
            oImpRequest.PartyToKey = oTransferQuoteRequest.PartyToKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.TransferQuote(oImpRequest)

                ' Response will only contain errors so no translation required
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oTransferQuoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oTransferQuoteRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UpdateCoinsuranceValuesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateCoinsuranceValues(ByVal UpdateCoinsuranceValuesRequest As UpdateCoinsuranceValuesRequestType) As UpdateCoinsuranceValuesResponseType Implements IPureCoreService.UpdateCoinsuranceValues

        Try

            Dim sUserName As String = UpdateCoinsuranceValuesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUCIV", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateCoinsuranceValuesRequest.WCFSecurityToken)
            Dim oResponse As New UpdateCoinsuranceValuesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateCoinsuranceValuesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateCoinsuranceValuesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateCoinsuranceValuesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateCoinsuranceValuesRequest.BranchCode

            'India : Populate the request structure with the relevant values, i.e.
            oImpRequest.InsuranceFileKey = UpdateCoinsuranceValuesRequest.InsuranceFileKey
            oImpRequest.IsRecovered = UpdateCoinsuranceValuesRequest.IsRecovered
            oImpRequest.IsSurcharged = UpdateCoinsuranceValuesRequest.IsSurcharged
            oImpRequest.DefaultId = UpdateCoinsuranceValuesRequest.DefaultId
            oImpRequest.TimeStamp = UpdateCoinsuranceValuesRequest.TimeStamp

            If UpdateCoinsuranceValuesRequest.CoInsurers IsNot Nothing Then

                oImpRequest.CoInsurers = New BaseImplementationTypes.BaseUpdateCoinsuranceValuesRequestTypeCoInsurers

                If UpdateCoinsuranceValuesRequest.CoInsurers IsNot Nothing AndAlso UpdateCoinsuranceValuesRequest.CoInsurers.Count <> 0 Then
                    ReDim oImpRequest.CoInsurers.Row(UpdateCoinsuranceValuesRequest.CoInsurers.Count - 1)
                    Dim iLenght As Integer = UpdateCoinsuranceValuesRequest.CoInsurers.Count - 1
                    Dim iRowIndex As Integer = 0
                    For iCnt As Integer = 0 To iLenght
                        Dim oImpRow As New BaseImplementationTypes.BaseUpdateCoinsuranceValuesRequestTypeCoInsurersRow
                        oImpRow.CoInsurerKey = UpdateCoinsuranceValuesRequest.CoInsurers(iCnt).CoInsurerKey
                        oImpRow.ArrangementRef = UpdateCoinsuranceValuesRequest.CoInsurers(iCnt).ArrangementRef
                        oImpRow.SharePerc = UpdateCoinsuranceValuesRequest.CoInsurers(iCnt).SharePerc
                        oImpRow.CommissionPerc = UpdateCoinsuranceValuesRequest.CoInsurers(iCnt).CommissionPerc
                        oImpRequest.CoInsurers.Row(iRowIndex) = oImpRow
                        iRowIndex = iRowIndex + 1
                    Next
                End If
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateCoinsuranceValues(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdateCoinsuranceValuesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateCoinsuranceValuesRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetPolicyDetailsForBouncedReceiptRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyDetailsForBouncedReceipt(ByVal oGetPolicyDetailsForBouncedReceiptRequest As GetPolicyDetailsForBouncedReceiptRequestType) As GetPolicyDetailsForBouncedReceiptResponseType Implements IPureCoreService.GetPolicyDetailsForBouncedReceipt
        Try

            Dim sUserName As String = oGetPolicyDetailsForBouncedReceiptRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHSKey", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPolicyDetailsForBouncedReceiptRequest.WCFSecurityToken)
            Dim oResponse As New GetPolicyDetailsForBouncedReceiptResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPolicyDetailsForBouncedReceiptRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPolicyDetailsForBouncedReceiptRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPolicyDetailsForBouncedReceiptResponseType = Nothing

            oImpRequest.BranchCode = oGetPolicyDetailsForBouncedReceiptRequest.BranchCode
            oImpRequest.DocumentRef = oGetPolicyDetailsForBouncedReceiptRequest.DocumentRef
            oImpRequest.InsuranceRef = oGetPolicyDetailsForBouncedReceiptRequest.InsuranceRef
            oImpRequest.MediaRef = oGetPolicyDetailsForBouncedReceiptRequest.MediaRef
            oImpRequest.WCFSecurityToken = If(oGetPolicyDetailsForBouncedReceiptRequest.WCFSecurityToken.Length > 0, oGetPolicyDetailsForBouncedReceiptRequest.WCFSecurityToken, "WCFSecurityToken")

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetPolicyDetailsForBouncedReceipt(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.Policies = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPolicyDetailsForBouncedReceiptResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataSet, sFromTypeName:="BaseGetPolicyDetailsForBouncedReceiptResponseTypePolicies", sConvertToTypeName:="BaseGetPolicyDetailsForBouncedReceiptResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Policies = DataTabletoList_GetPolicyDetailsForBouncedReceipt(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPolicyDetailsForBouncedReceiptRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPolicyDetailsForBouncedReceiptRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to run the default rules files explicitly
    ''' </summary>
    ''' <param name="RunDefaultRulesAddRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RunDefaultRulesAdd(ByVal RunDefaultRulesAddRequest As RunDefaultRulesAddRequestType) As RunDefaultRulesAddResponseType Implements IPureCoreService.RunDefaultRulesAdd, IPurePolicyService.RunDefaultRulesAdd, IPureClaimService.RunDefaultRulesAdd

        Try

            Dim sUserName As String = RunDefaultRulesAddRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMRDfRulA", iUserId)
            CommonFunctions.CheckSecurityToken(RunDefaultRulesAddRequest.WCFSecurityToken)
            Dim oResponse As New RunDefaultRulesAddResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, RunDefaultRulesAddRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunDefaultRulesAddRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.RunDefaultRulesAddResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = RunDefaultRulesAddRequest.BranchCode
            oImpRequest.ScreenCode = RunDefaultRulesAddRequest.ScreenCode
            oImpRequest.UserName = sUserName
            oImpRequest.XMLDataSet = SAMFunc.TransformDatasetSAMtoPB(RunDefaultRulesAddRequest.XMLDataSet)
            oImpRequest.SkipSaveToDB = RunDefaultRulesAddRequest.SkipSaveToDB

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.RunDefaultRulesAdd(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet, Nothing, RunDefaultRulesAddRequest.SkipSaveToDB, sCalledVia:="SKIP", iRiskTypeID:=oImpResponse.RiskTypeID, iSourceID:=oImpResponse.BranchID)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(RunDefaultRulesAddRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(RunDefaultRulesAddRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RunDefaultRulesEditRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RunDefaultRulesEdit(ByVal RunDefaultRulesEditRequest As RunDefaultRulesEditRequestType) As RunDefaultRulesEditResponseType Implements IPureCoreService.RunDefaultRulesEdit, IPurePolicyService.RunDefaultRulesEdit, IPureClaimService.RunDefaultRulesEdit

        Try
            Dim sUserName As String = RunDefaultRulesEditRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMRDfRulE", iUserId)

            CommonFunctions.CheckSecurityToken(RunDefaultRulesEditRequest.WCFSecurityToken)
            Dim oResponse As New RunDefaultRulesEditResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, RunDefaultRulesEditRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunDefaultRulesEditRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.RunDefaultRulesEditResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = RunDefaultRulesEditRequest.BranchCode
            oImpRequest.ScreenCode = RunDefaultRulesEditRequest.ScreenCode
            oImpRequest.UserName = sUserName
            oImpRequest.XMLDataSet = SAMFunc.TransformDatasetSAMtoPB(RunDefaultRulesEditRequest.XMLDataSet)

            oImpRequest.ClaimKeySpecified = RunDefaultRulesEditRequest.ClaimKeySpecified
            oImpRequest.ClaimPerilKeySpecified = RunDefaultRulesEditRequest.ClaimPerilKeySpecified

            If RunDefaultRulesEditRequest.ClaimKeySpecified Then
                oImpRequest.ClaimKey = RunDefaultRulesEditRequest.ClaimKey
            End If

            If RunDefaultRulesEditRequest.ClaimPerilKeySpecified Then
                oImpRequest.ClaimPerilKey = RunDefaultRulesEditRequest.ClaimPerilKey
            End If

            oImpRequest.TransactionType = RunDefaultRulesEditRequest.TransactionType
            oImpRequest.SkipSaveToDB = RunDefaultRulesEditRequest.SkipSaveToDB

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.RunDefaultRulesEdit(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet, Nothing, RunDefaultRulesEditRequest.SkipSaveToDB)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(RunDefaultRulesEditRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(RunDefaultRulesEditRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' Get Product wise user branch
    ''' </summary>
    ''' <param name="oGetProductsForUserBranchRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProductsForUserBranch(ByVal oGetProductsForUserBranchRequest As GetProductsForUserBranchRequestType) _
        As GetProductsForUserBranchResponseType Implements IPureCoreService.GetProductsForUserBranch

        Try

            Dim sUserName As String = oGetProductsForUserBranchRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGPROBR", iUserId)
            CommonFunctions.CheckSecurityToken(oGetProductsForUserBranchRequest.WCFSecurityToken)
            Dim oResponse As New GetProductsForUserBranchResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetProductsForUserBranchRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetProductsForUserBranchResponseType = Nothing

            oImpRequest.LoginUserName = oGetProductsForUserBranchRequest.LoginUserName
            Try
                ' Call the implementation method   
                oImpResponse = oBusiness.GetProductsForUserBranch(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If (oImpResponse.Products IsNot Nothing AndAlso oImpResponse.Products.Row IsNot Nothing AndAlso
                     oImpResponse.Products.Row.Count > 0) Then
                    oResponse.Products = oImpResponse.Products.Row.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseGetProductsForUserBranchResponseTypeProductsRow,
                                      BaseGetProductsForUserBranchResponseTypeProductsRow)(AddressOf CommonFunctions.ToServiceGetProductsForUserBranchList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetProductsForUserBranchRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetProductsForUserBranchRequest))
            Return Nothing
        End Try
    End Function

#Region "DocumentPost- DTU"
    ''' <summary>
    ''' This will be used to import Document via DTU2
    ''' </summary>
    ''' <param name="oDocumentDataImportRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DocumentDataImport(ByVal oDocumentDataImportRequest As DocumentDataImportRequestType) As DocumentDataImportResponseType Implements IPureCoreService.DocumentDataImport

        Try

            Dim sUserName As String = String.Empty
            Dim nAgentKey As Integer = 0
            Dim nUserId As Integer = 0

            sUserName = oDocumentDataImportRequest.LoginUserName.ToString
            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMDocData", nUserId)

            Dim oMsgResponse As New DocumentDataImportResponseType

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.DocumentDataImportRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.PostDocumentResponseType

            oImpRequest.BranchCode = oDocumentDataImportRequest.BranchCode
            oImpRequest.SAMStagingDocumentKey = oDocumentDataImportRequest.SAMStagingDocumentKey
            oImpRequest.Document = ToBaseImpBasePostDocumentRequestType(oDocumentDataImportRequest.Document)

            ' Load the Request object for the next SAM layer . . . . . . . . 
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oDocumentDataImportRequest.BranchCode)

            Try

                oImpResponse = oBusiness.DocumentDataImport(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oMsgResponse.HandlingInstanceID = oImpResponse.HandlingInstanceID

            Catch exError As Exception
                Handler.BusinessLayerBoundary(exError, oMsgResponse)
            End Try

            Return oMsgResponse

        Catch exError As Exception
            CommonFunctions.BusinessLayerLastResort(exError, CommonFunctions.CreateDictionary(oDocumentDataImportRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' this will convert list rquest into array from.
    ''' </summary>
    ''' <param name="oServicePostDocumentRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToBaseImpBasePostDocumentRequestType(ByVal oServicePostDocumentRequestType As BasePostDocumentRequestType) As SAMForInsuranceV2ImplementationTypes.PostDocumentRequestType
        Dim oImpPostDocumentRequestType As New SAMForInsuranceV2ImplementationTypes.PostDocumentRequestType

        If oServicePostDocumentRequestType IsNot Nothing Then

            oImpPostDocumentRequestType.BranchCode = oServicePostDocumentRequestType.BranchCode
            oImpPostDocumentRequestType.DocumentType = CType([Enum].ToObject(GetType(DocumentTypeType), oServicePostDocumentRequestType.DocumentType), BaseImplementationTypes.DocumentTypeType)
            oImpPostDocumentRequestType.Comment = oServicePostDocumentRequestType.Comment
            oImpPostDocumentRequestType.DocumentReference = oServicePostDocumentRequestType.DocumentReference
            oImpPostDocumentRequestType.DocumentTypeCode = oServicePostDocumentRequestType.DocumentTypeCode
            oImpPostDocumentRequestType.SAMStagingPolicyKey = oServicePostDocumentRequestType.SAMStagingPolicyKey
            oImpPostDocumentRequestType.InsuranceFileKey = oServicePostDocumentRequestType.InsuranceFileKey
            oImpPostDocumentRequestType.InsuranceFileKeySpecified = oServicePostDocumentRequestType.InsuranceFileKeySpecified

            If oServicePostDocumentRequestType.Transactions IsNot Nothing Then
                oImpPostDocumentRequestType.Transactions = Array.ConvertAll(
                                                            oServicePostDocumentRequestType.Transactions.ToArray,
                                                            New Converter(Of BaseTransactionType,
                                                                BaseImplementationTypes.BaseTransactionType) _
                                                                (AddressOf CommonFunctions.ToBaseImpBaseTransactionType))

            End If
        End If

        Return oImpPostDocumentRequestType

    End Function
#End Region


#Region "GetTaskOnKeys"

    'Public Function GetTaskOnKeys(<XmlElement(elementName:=" GetTaskOnKeysRequest", Namespace:=ServiceNamespace)> _
    'ByVal oGetTaskOnKeysRequest As GetTaskOnKeysRequestType) As _
    '<XmlElement(elementName:=" GetTaskOnKeysResponse", Namespace:=ServiceNamespace)> GetTaskOnKeysResponseType
    Public Function GetTaskOnKeys(ByVal oGetTaskOnKeysRequest As GetTaskOnKeysRequestType) As GetTaskOnKeysResponseType Implements IPureCoreService.GetTaskOnKeys
        Try
            Dim sUserName As String = oGetTaskOnKeysRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            'CheckAuthority("SAMGTOKey")
            CommonFunctions.CheckAuthority("SAMGTOKey", iUserId)
            'Dim sUserName As String = String.Empty
            'Dim iAgentKey As Integer
            'Dim iUserId As Integer
            'GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetTaskOnKeysResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetTaskOnKeysRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetTaskOnKeysResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetTaskOnKeysRequest.BranchCode

            If oGetTaskOnKeysRequest.KeyData IsNot Nothing Then
                Dim iLength As Int32 = oGetTaskOnKeysRequest.KeyData.Row.Length
                oImpRequest.KeyData = New BaseImplementationTypes.BaseCreateWmTaskRequestTypeKeyData
                For icount As Integer = 0 To iLength - 1
                    ReDim Preserve oImpRequest.KeyData.Row(icount)
                    oImpRequest.KeyData.Row(icount) = New BaseImplementationTypes.BaseCreateWmTaskRequestTypeKeyDataRow
                    oImpRequest.KeyData.Row(icount).KeyName = oGetTaskOnKeysRequest.KeyData.Row(icount).KeyName
                    oImpRequest.KeyData.Row(icount).KeyValue = oGetTaskOnKeysRequest.KeyData.Row(icount).KeyValue
                Next
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetTaskOnKeys(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' oResponse.RatingSectionTypes = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRatingSectionTypesResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetRatingSectionTypesResponseTypeRatingSectionTypes", sConvertToTypeName:="BaseGetRatingSectionTypesResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Tasks = DataTabletoList_GetTaskOnKeys(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch exError As Exception
            'Handler.BusinessLayerLastResort(ex, Context)
            CommonFunctions.BusinessLayerLastResort(exError, CommonFunctions.CreateDictionary(oGetTaskOnKeysRequest))
            Return Nothing
        End Try

    End Function
#End Region

#Region "UpdateTaskStatus"
    Public Function UpdateTaskStatus(ByVal oUpdateTaskStatusRequest As UpdateTaskStatusRequestType) As UpdateTaskStatusResponseType Implements IPureCoreService.UpdateTaskStatus

        Try
            Dim sUserName As String = oUpdateTaskStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)

            If oUpdateTaskStatusRequest.ActionType = WMActionType.Update Then
                CommonFunctions.CheckAuthority("SAMUWMKey", iUserId)
            ElseIf oUpdateTaskStatusRequest.ActionType = WMActionType.Assign Then
                CommonFunctions.CheckAuthority("SAMANWMKey", iUserId)
            ElseIf oUpdateTaskStatusRequest.ActionType = WMActionType.Complete Then
                CommonFunctions.CheckAuthority("SAMCMWMKey", iUserId)
            ElseIf oUpdateTaskStatusRequest.ActionType = WMActionType.InComplete Then
                CommonFunctions.CheckAuthority("SAMICWMKey", iUserId)
            ElseIf oUpdateTaskStatusRequest.ActionType = WMActionType.Run Then
                CommonFunctions.CheckAuthority("SAMRWMKey", iUserId)
            End If

            Dim oResponse As New UpdateTaskStatusResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateTaskStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateTaskStatusResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateTaskStatusRequest.BranchCode
            oImpRequest.TaskStatusKey = oUpdateTaskStatusRequest.TaskStatusKey
            oImpRequest.TaskInstanceKey = oUpdateTaskStatusRequest.TaskInstanceKey
            oImpRequest.GuidPMExternalItem = oUpdateTaskStatusRequest.GuidPMExternalItem
            oImpRequest.ExternalTaskStatus = oUpdateTaskStatusRequest.ExternalTaskStatus
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateTaskStatus(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateTaskStatusRequest))
            End Try

            Return oResponse

        Catch exError As Exception
            CommonFunctions.BusinessLayerLastResort(exError, CommonFunctions.CreateDictionary(oUpdateTaskStatusRequest))
            Return Nothing
        End Try

    End Function

#End Region
#Region "GetPolicyOutstandingAmount"


    Public Function GetPolicyOutstandingAmount(ByVal oGetPolicyOutstandingAmountRequest As GetPolicyOutstandingAmountRequestType) As GetPolicyOutstandingAmountResponseType Implements IPureCoreService.GetPolicyOutstandingAmount
        Try
            Dim sUserName As String = oGetPolicyOutstandingAmountRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGPOA", iUserId)

            Dim oResponse As New GetPolicyOutstandingAmountResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPolicyOutstandingAmountRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPolicyOutstandingAmountResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetPolicyOutstandingAmountRequest.BranchCode
            oImpRequest.Insurancefilecnt = oGetPolicyOutstandingAmountRequest.InsuarnaceFilecnt

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetPolicyOutstandingAmount(oImpRequest)
                oResponse.OutstandingAmount = oImpResponse.OutStandingAmount
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch exError As Exception
            'Handler.BusinessLayerLastResort(ex, Context)
            CommonFunctions.BusinessLayerLastResort(exError)
            Return Nothing
        End Try

    End Function
#End Region

#Region "GetLockDetails"

    ''' <summary>  
    ''' Gets all the details from PMLock Table related to locks in the system
    ''' </summary>  
    ''' <param name="oGetLockDetailsRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.BaseGetLockDetailsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetLockDetailsResponseType</returns>  

    Public Function GetLockDetails(ByVal oGetLockDetailsRequest As GetLockDetailsRequestType) As GetLockDetailsResponseType Implements IPureCoreService.GetLockDetails
        Try


            Dim oResponse As New GetLockDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetLockDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetLockDetailsResponseType = Nothing

            oImpRequest.BranchCode = oGetLockDetailsRequest.BranchCode

            Try
                oImpResponse = oBusiness.GetLockDetails(oImpRequest)

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False
                oXmlOverride.Add(GetType(BaseGetLockDetailsResponseTypeDetails), oXmlAttributes)
                oXmlOverride.Add(GetType(BaseGetLockDetailsResponseTypeDetailsRow), oXmlAttributes)

                Dim oResultDataSet As BaseGetLockDetailsResponseTypeDetails = Nothing
                Dim oResultDataSetObject As Object = Nothing
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Details = DataTabletoList_GetLockDetails(oImpResponse.ResultData.Tables(0))
                End If

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse
        Catch exError As Exception
            CommonFunctions.BusinessLayerLastResort(exError)
            Return Nothing
        End Try

    End Function


#End Region

#Region "MaintainLock"

    ''' <summary>  
    ''' Following funcitonality it suuported by this WebService Method
    ''' 1.Clearing locks on LogOut, using LogOutUserID and LogOutSessionValue request parameters
    ''' 2.Lock Administrator: Clear all locks, using ClearAllLocks
    ''' 3.Lock Administrator: Delete individual or multiple locks from lock administrator, using LockDetails object
    ''' </summary>  
    ''' <param name="oMaintainLockRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.BaseMaintainLockRequestType</param> 
    Public Function MaintainLock(ByVal oMaintainLockRequest As MaintainLockRequestType) As MaintainLockResponseType Implements IPureCoreService.MaintainLock
        Try

            Dim oResponse As New MaintainLockResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()


            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.MaintainLockRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.MaintainLockResponseType = Nothing

            oImpRequest.BranchCode = oMaintainLockRequest.BranchCode

            'Log Off via Portal
            oImpRequest.LogOutSessionValue = oMaintainLockRequest.LogOutSessionValue

            'Via Portal Lock Administrator, we try to clear all locks. Is_System_Lock can not be deleted.
            oImpRequest.ClearAllLocks = oMaintainLockRequest.ClearAllLocks

            'Via Portal Lock Administrator we select multiple Locks for deletion
            Dim oLockDetails(0) As BaseImplementationTypes.BaseLockDetails
            If oMaintainLockRequest.LockDetails IsNot Nothing AndAlso IsArray(oMaintainLockRequest.LockDetails) Then

                For nCount As Integer = oMaintainLockRequest.LockDetails.GetLowerBound(0) To _
                                        oMaintainLockRequest.LockDetails.GetUpperBound(0)
                    ReDim Preserve oLockDetails(nCount)
                    oLockDetails(nCount) = New BaseImplementationTypes.BaseLockDetails
                    oLockDetails(nCount).LockName = oMaintainLockRequest.LockDetails(nCount).LockName
                    oLockDetails(nCount).LockValue = oMaintainLockRequest.LockDetails(nCount).LockValue

                Next

                oImpRequest.LockDetails = oLockDetails
            End If

            Try
                oImpResponse = oBusiness.MaintainLock(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse
        Catch exError As Exception
            CommonFunctions.BusinessLayerLastResort(exError)
            Return Nothing
        End Try

    End Function



#End Region

#Region "GetPaymentHubSystemOptions"


    Public Function GetPaymentHubSystemOptions(ByVal oGetPaymentHubSystemOptionsRequest As GetPaymentHubSystemOptionsRequestType) As GetPaymentHubSystemOptionsResponseType Implements IPureCoreService.GetPaymentHubSystemOptions
        Try
            Dim sUserName As String = oGetPaymentHubSystemOptionsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            'CommonFunctions.CheckAuthority("SAMGPOA", iUserId)

            Dim oResponse As New GetPaymentHubSystemOptionsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPaymentHubSystemOptionsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPaymentHubSystemOptionResponseType = Nothing

            '' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetPaymentHubSystemOptionsRequest.BranchCode


            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetPaymentHubSystemOptions(oImpRequest)
                oResponse.SystemUserName = oImpResponse.SystemUserName
                oResponse.Password = oImpResponse.Password
                oResponse.ClientName = oImpResponse.ClientName
                oResponse.BrokerSCID = oImpResponse.BrokerSCID
                oResponse.MerchantID = oImpResponse.MerchantID
                oResponse.Customer = oImpResponse.Customer
                oResponse.SystemGUID = oImpResponse.SystemGUID
                oResponse.SystemPasscode = oImpResponse.SystemPasscode
                oResponse.AccountID = oImpResponse.AccountID
                oResponse.TransactionIPAddress = oImpResponse.TransactionIPAddress
                oResponse.RefundPasscode = oImpResponse.RefundPasscode
                oResponse.CaptureMethod = oImpResponse.CaptureMethod
                oResponse.RefundPremiumthroughInvoice = oImpResponse.RefundPremiumthroughInvoice
                oResponse.Donotuseoldcarddetailsforsubsequentpayments = oImpResponse.Donotuseoldcarddetailsforsubsequentpayments
                oResponse.MarkDefaultCreditCard = oImpResponse.MarkDefaultCreditCard
                oResponse.LanguageTemplateID = oImpResponse.LanguageTemplateID
                oResponse.MerchantTemplateID = oImpResponse.MerchantTemplateID
                oResponse.AccountPassCode = oImpResponse.AccountPassCode
                oResponse.PaymentHubServiceUrl = oImpResponse.PaymentHubServiceUrl

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch exError As Exception
            'Handler.BusinessLayerLastResort(ex, Context)
            CommonFunctions.BusinessLayerLastResort(exError)
            Return Nothing
        End Try

    End Function
#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oProcessListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessList(ByVal oProcessListRequest As ProcessListRequestType) As ProcessListResponseType Implements IPureCoreService.ProcessList

        Try

            Dim sUserName As String = oProcessListRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFindPol", iUserId)
            CommonFunctions.CheckSecurityToken(oProcessListRequest.WCFSecurityToken)

            Dim oResponse As New ProcessListResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oProcessListRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ProcessListRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ProcessListResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oProcessListRequest.BranchCode
            oImpRequest.TableData = oProcessListRequest.TableData
            oImpRequest.TableName = oProcessListRequest.TableName

            'oImpResponse.
            'oImpRequest.WCFSecurityToken = If(oProcessListRequest.WCFSecurityToken.Length > 0, oProcessListRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ProcessList(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.status = oImpResponse.Status
                oResponse.ResponseMessage = oImpResponse.ResponseMessage

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oProcessListRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oProcessListRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Execute any stored procedure in Pure database
    ''' </summary>
    ''' <param name="oCallNamedStoredProcedureRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CallNamedStoredProcedure(ByVal oCallNamedStoredProcedureRequest As CallNamedStoredProcedureRequestType) _
        As CallNamedStoredProcedureResponseType Implements IPureCoreService.CallNamedStoredProcedure

        Try

            Dim sUserName As String = oCallNamedStoredProcedureRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMSPEXC", iUserId)
            CommonFunctions.CheckSecurityToken(oCallNamedStoredProcedureRequest.WCFSecurityToken)
            Dim oResponse As New CallNamedStoredProcedureResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CallNamedStoredProcedureRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CallNamedStoredProcedureResponseType = Nothing

            oImpRequest.LoginUserName = oCallNamedStoredProcedureRequest.LoginUserName
            Try
                ' Call the implementation method
                oImpRequest.LoginUserName = oCallNamedStoredProcedureRequest.LoginUserName
                oImpRequest.ProcedureName = oCallNamedStoredProcedureRequest.ProcedureName
                oImpRequest.IsReportDataset = oCallNamedStoredProcedureRequest.IsReportDataset
                If oCallNamedStoredProcedureRequest.Parameters IsNot Nothing AndAlso oCallNamedStoredProcedureRequest.Parameters.GetUpperBound(0) >= 0 Then
                    Dim oParameters() As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseParameterType = Nothing
                    Dim oParametersItem As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseParameterType

                    ReDim oParameters(oCallNamedStoredProcedureRequest.Parameters.GetUpperBound(0))
                    For iCntIndex As Integer = 0 To oCallNamedStoredProcedureRequest.Parameters.GetUpperBound(0)
                        oParametersItem = New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseParameterType
                        oParametersItem.ParamName = oCallNamedStoredProcedureRequest.Parameters(iCntIndex).ParamName
                        oParametersItem.ParamValue = oCallNamedStoredProcedureRequest.Parameters(iCntIndex).ParamValue
                        oParameters(iCntIndex) = oParametersItem
                    Next
                    oImpRequest.Parameters = oParameters
                End If
                oImpResponse = oBusiness.CallNamedStoredProcedure(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.Results = oImpResponse.Results
                If oImpRequest.IsReportDataset Then
                    oResponse.ReportDataset = New List(Of Dictionary(Of String, Object))
                    oResponse.ReportDataset.AddRange(oImpResponse.ReportDataset)
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCallNamedStoredProcedureRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCallNamedStoredProcedureRequest))
            Return Nothing
        End Try
    End Function

End Class
