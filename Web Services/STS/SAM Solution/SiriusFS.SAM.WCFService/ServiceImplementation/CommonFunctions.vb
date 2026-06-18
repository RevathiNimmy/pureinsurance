Imports System.Drawing
Imports System.Reflection
Imports System.Web.Configuration
Imports Sirius.Architecture.Data
Imports Sirius.Architecture.ExceptionHandling
Imports Sirius.Architecture.Security
Imports Sirius.Architecture.Utility
Imports SiriusFS.SAM.CoreImplementation
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF
Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SSP.Shared

''' <summary>
''' A class for common shared functions
''' </summary>
''' <remarks></remarks>
Public Class CommonFunctions
    Public Class Priority
        Public Const Lowest As Integer = 0
        Public Const Low As Integer = 1
        Public Const Normal As Integer = 2
        Public Const High As Integer = 3
        Public Const Highest As Integer = 4
    End Class

    Public Class Category
        Public Const General As String = "General"
        Public Const Trace As String = "Trace"
        Public Const CriticalError As String = "Critical Error"
    End Class
    Public Shared Function ToServiceClaimWarningTypeList(ByVal oServiceClaimWarningType As BaseImplementationTypes.BaseClaimResponseTypeWarnings) As BaseClaimResponseTypeWarnings

        Dim serviceServiceClaimWarningType As New BaseClaimResponseTypeWarnings
        If oServiceClaimWarningType IsNot Nothing Then
            With oServiceClaimWarningType
                serviceServiceClaimWarningType.Code = .Code
                serviceServiceClaimWarningType.Description = .Description
            End With
        End If
        Return serviceServiceClaimWarningType

    End Function

    ''' <summary>
    ''' To convert from internal PartyList to WCF service PartyList
    ''' </summary>
    ''' <param name="oclaimList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceClaimList(ByVal oclaimList As SFI.SAMForInsuranceV2.BaseFindClaimResponseTypeClaimsRow) As BaseFindClaimResponseTypeRow

        Dim oServiceClaimList As BaseFindClaimResponseTypeRow = New BaseFindClaimResponseTypeRow

        If oclaimList IsNot Nothing Then
            oServiceClaimList.BaseClaimKey = oclaimList.BaseClaimKey
            oServiceClaimList.CaseNumber = oclaimList.CaseNumber
            oServiceClaimList.CatastropheCode = oclaimList.CatastropheCode
            oServiceClaimList.ClaimDescription = oclaimList.ClaimDescription
            oServiceClaimList.ClaimHandlerDescription = oclaimList.ClaimHandlerDescription
            oServiceClaimList.ClaimKey = oclaimList.ClaimKey
            oServiceClaimList.ClaimNumber = oclaimList.ClaimNumber
            oServiceClaimList.ClaimStatusID = oclaimList.ClaimStatusID
            oServiceClaimList.ClientClaimNumber = oclaimList.ClientClaimNumber
            oServiceClaimList.ClientName = oclaimList.ClientName
            oServiceClaimList.ClientShortName = oclaimList.ClientShortName
            oServiceClaimList.ClientTelephoneNumber = oclaimList.ClientTelephoneNumber
            oServiceClaimList.ClientTelephoneNumberOffice = oclaimList.ClientTelephoneNumberOffice
            oServiceClaimList.CoverFrom = oclaimList.CoverFrom
            oServiceClaimList.CoverTo = oclaimList.CoverTo
            oServiceClaimList.CurrencyISOCode = oclaimList.CurrencyISOCode
            oServiceClaimList.InfoOnly = oclaimList.InfoOnly
            oServiceClaimList.InsuranceFileKey = oclaimList.InsuranceFileKey
            oServiceClaimList.InsuranceRef = oclaimList.InsuranceRef
            oServiceClaimList.InsurerClaimNumber = oclaimList.InsurerClaimNumber
            oServiceClaimList.IsAllowedClosedClaims = oclaimList.IsAllowedClosedClaims
            oServiceClaimList.IsDeleted = oclaimList.IsDeleted
            oServiceClaimList.LastModifiedDate = oclaimList.LastModifiedDate
            oServiceClaimList.LeadAgentName = oclaimList.LeadAgentName
            oServiceClaimList.LossDateFrom = oclaimList.LossDateFrom
            oServiceClaimList.NotificationDate = oclaimList.NotificationDate
            oServiceClaimList.Payments = oclaimList.Payments
            oServiceClaimList.PrimaryCauseDescription = oclaimList.PrimaryCauseDescription
            oServiceClaimList.ProductDescription = oclaimList.ProductDescription
            oServiceClaimList.ProgressStatusDescription = oclaimList.ProgressStatusDescription
            oServiceClaimList.ReportedDate = oclaimList.ReportedDate
            oServiceClaimList.Reserve = oclaimList.Reserve
            oServiceClaimList.RiskKey = oclaimList.RiskKey
            oServiceClaimList.SecondaryCauseDescription = oclaimList.SecondaryCauseDescription
        End If

        Return oServiceClaimList

    End Function

    Public Shared Function ToBaseImpBaseContactType(ByVal msgContact As SFI.SAMForInsuranceV2.BaseContactType) As BaseImplementationTypes.BaseContactType
        Dim impContact As BaseImplementationTypes.BaseContactType = New BaseImplementationTypes.BaseContactType

        If msgContact IsNot Nothing Then

            impContact.AreaCode = Trim(msgContact.AreaCode)
            impContact.Description = Trim(msgContact.Description) ' Vivek: 20080704 - added the missing item to fix bug in existing system (Amend Client)            
            impContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
            impContact.ContactDetail.Item = Trim(msgContact.ContactDetail.Item)
            If msgContact.ContactTypeCode <> BaseImplementationTypes.ContactTypeType.OTHER Then
                impContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), msgContact.ContactTypeCode), BaseImplementationTypes.ContactTypeType)
            Else
                impContact.ContactTypeCode = BaseImplementationTypes.ContactTypeType.OTHER
                impContact.OtherContactTypeCode = Trim(msgContact.OtherContactTypeCode)
            End If
            impContact.Extension = Trim(msgContact.Extension) 'Added on 22-09-08

        End If
        Return impContact
    End Function

    Public Shared Function ToServiceGenerateClaimsDocumentsList(ByVal oGenerateClaimsDocumentsList As BaseImplementationTypes.BaseGenerateClaimsDocumentsResponseTypeDocumentsRow) As BaseGenerateClaimsDocumentsResponseTypeRow

        Dim oServiceClaimList As BaseGenerateClaimsDocumentsResponseTypeRow = New BaseGenerateClaimsDocumentsResponseTypeRow

        If oGenerateClaimsDocumentsList IsNot Nothing Then
            With oGenerateClaimsDocumentsList
                oServiceClaimList.DocumentDescription = .DocumentDescription
                oServiceClaimList.DocumentName = .DocumentName
            End With

        End If

        Return oServiceClaimList

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCoinsuranceDefaultsList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToCoinsuranceDefaultsList(ByVal oCoinsuranceDefaultsList As SFI.SAMForInsuranceV2.BaseGetCoinsuranceDefaultsResponseTypeDefaultsRow) As BaseGetCoinsuranceDefaultsResponseTypeRow

        Dim oServiceCoinsuranceDefaultsList As BaseGetCoinsuranceDefaultsResponseTypeRow = New BaseGetCoinsuranceDefaultsResponseTypeRow

        If oCoinsuranceDefaultsList IsNot Nothing Then
            With oCoinsuranceDefaultsList
                oServiceCoinsuranceDefaultsList.Code = .Code
                oServiceCoinsuranceDefaultsList.CoinsuranceDefault = .CoinsuranceDefault
                oServiceCoinsuranceDefaultsList.CoinsuranceDefaultId = .CoinsuranceDefaultId
                oServiceCoinsuranceDefaultsList.IsRecovered = .IsRecovered
                oServiceCoinsuranceDefaultsList.IsSurcharged = .IsSurcharged
            End With
        End If
        Return oServiceCoinsuranceDefaultsList

    End Function












    Public Shared Function ToServiceHeaderAndRisksByKeyResponseTypeRisksList(ByVal oHeaderAndRisksByKeyResponseTypeRisksList As BaseImplementationTypes.BaseGetHeaderAndRisksByKeyResponseTypeRisksRow) As BaseGetHeaderAndRisksByKeyResponseTypeRow

        Dim oServiceHeaderAndRisksByKeyResponseTypeRisksList As BaseGetHeaderAndRisksByKeyResponseTypeRow = New BaseGetHeaderAndRisksByKeyResponseTypeRow

        If oHeaderAndRisksByKeyResponseTypeRisksList IsNot Nothing Then
            With oHeaderAndRisksByKeyResponseTypeRisksList
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.Coverage = .Coverage

                If .CoverNote IsNot Nothing Then
                    With .CoverNote
                        oServiceHeaderAndRisksByKeyResponseTypeRisksList.CoverNote = New BaseCoverNoteRiskItemType
                        oServiceHeaderAndRisksByKeyResponseTypeRisksList.CoverNote.CoverNoteFrom = .CoverNoteFrom
                        oServiceHeaderAndRisksByKeyResponseTypeRisksList.CoverNote.CoverNoteFromSpecified = .CoverNoteFromSpecified
                        oServiceHeaderAndRisksByKeyResponseTypeRisksList.CoverNote.CoverNoteNumber = .CoverNoteNumber
                        oServiceHeaderAndRisksByKeyResponseTypeRisksList.CoverNote.CoverNoteTo = .CoverNoteTo
                        oServiceHeaderAndRisksByKeyResponseTypeRisksList.CoverNote.CoverNoteToSpecified = .CoverNoteToSpecified
                        oServiceHeaderAndRisksByKeyResponseTypeRisksList.CoverNote.RiskDesc = .RiskDesc
                        oServiceHeaderAndRisksByKeyResponseTypeRisksList.CoverNote.RiskKey = .RiskKey
                        oServiceHeaderAndRisksByKeyResponseTypeRisksList.CoverNote.TImeStamp = .TImeStamp
                    End With
                End If
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.Description = .Description
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.Discounted = .Discounted
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.EndDate = .EndDate
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.Extensions = .Extensions
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.FeePremium = .FeePremium
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.FeeTax = .FeeTax
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.InsuredItem = .InsuredItem
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.IsRisk = .IsRisk
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.OriginalRiskKey = .OriginalRiskKey
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.Premium = .Premium
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.RiskFolderKey = .RiskFolderKey
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.RiskKey = .RiskKey
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.RiskNumber = .RiskNumber
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.RiskTax = .RiskTax
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.RiskTypeCode = .RiskTypeCode
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.RiskTypeDescription = .RiskTypeDescription
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.StampDutyInsured = .StampDutyInsured
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.StampDutyInsurer = .StampDutyInsurer
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.StartDate = .StartDate
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.StatusCode = .StatusCode
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.StatusDescription = .StatusDescription
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.TotalSumInsured = .TotalSumInsured
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.Variation = .Variation
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.IsMandatoryRisk = .IsMandatoryRisk
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.riskLinkStatusFlag = .riskLinkStatusFlag
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.riskLinkChangeDate = .riskLinkChangeDate
                oServiceHeaderAndRisksByKeyResponseTypeRisksList.Is_Auto_Rated = .Is_Auto_Rated
            End With
        End If
        Return oServiceHeaderAndRisksByKeyResponseTypeRisksList

    End Function


    Public Shared Function ToServiceTaxesAndFeesType(ByVal taxesAndFees As BaseImplementationTypes.BaseTaxesAndFeesType) As BaseTaxesAndFeesType

        Dim serviceTaxesAndFees As BaseTaxesAndFeesType = New BaseTaxesAndFeesType

        If taxesAndFees IsNot Nothing Then

            If taxesAndFees.Fees IsNot Nothing Then

                serviceTaxesAndFees.Fees = taxesAndFees.Fees.ToList().ConvertAll(
                                        New Converter(Of BaseImplementationTypes.BaseFeesType, BaseFeesType)(AddressOf CommonFunctions.ToServiceFeesType))


            End If

            If taxesAndFees.Taxes IsNot Nothing Then

                serviceTaxesAndFees.Taxes = taxesAndFees.Taxes.ToList().ConvertAll(
                                        New Converter(Of BaseImplementationTypes.BaseTaxesType, BaseTaxesType)(AddressOf CommonFunctions.ToServiceTaxesType))

            End If

        End If

        Return serviceTaxesAndFees

    End Function


    Public Shared Function ToBaseImpBaseClaimPerilMaintainType(ByVal msgClaimPerilType As BaseClaimPerilMaintainType) As BaseImplementationTypes.BaseClaimPerilMaintainType


        Dim impClaimPerilType As BaseImplementationTypes.BaseClaimPerilMaintainType = New BaseImplementationTypes.BaseClaimPerilMaintainType

        If msgClaimPerilType IsNot Nothing Then

            impClaimPerilType.Description = msgClaimPerilType.Description
            impClaimPerilType.TypeCode = msgClaimPerilType.TypeCode
            impClaimPerilType.BaseClaimPerilKeySpecified = msgClaimPerilType.BaseClaimPerilKeySpecified
            impClaimPerilType.BaseClaimPerilKey = msgClaimPerilType.BaseClaimPerilKey

            If msgClaimPerilType.Reserve IsNot Nothing Then
                impClaimPerilType.Reserve = Array.ConvertAll(
                                                     msgClaimPerilType.Reserve.ToArray,
                                                     New Converter(Of BaseClaimPerilReserveType,
                                                         BaseImplementationTypes.BaseClaimPerilReserveType) _
                                                         (AddressOf CommonFunctions.ToBaseImpBaseClaimPerilReserveType))
            End If

            If msgClaimPerilType.Recovery IsNot Nothing Then
                impClaimPerilType.Recovery = Array.ConvertAll(
                                                     msgClaimPerilType.Recovery.ToArray,
                                                     New Converter(Of BaseClaimPerilRecoveryType,
                                                         BaseImplementationTypes.BaseClaimPerilRecoveryType) _
                                                         (AddressOf CommonFunctions.ToBaseImpBaseClaimPerilRecoveryType))

            End If

        End If

        Return impClaimPerilType

    End Function

    Public Shared Function ToBaseImpBaseClaimPerilType(ByVal msgClaimPerilType As BaseClaimPerilType) As BaseImplementationTypes.BaseClaimPerilType


        Dim impClaimPerilType As BaseImplementationTypes.BaseClaimPerilType = New BaseImplementationTypes.BaseClaimPerilType

        If msgClaimPerilType IsNot Nothing Then

            impClaimPerilType.Description = msgClaimPerilType.Description
            impClaimPerilType.TypeCode = msgClaimPerilType.TypeCode

            If msgClaimPerilType.Reserve IsNot Nothing Then
                impClaimPerilType.Reserve = Array.ConvertAll(
                                                     msgClaimPerilType.Reserve.ToArray,
                                                     New Converter(Of BaseClaimPerilReserveType,
                                                         BaseImplementationTypes.BaseClaimPerilReserveType) _
                                                         (AddressOf CommonFunctions.ToBaseImpBaseClaimPerilReserveType))
            End If

            If msgClaimPerilType.Recovery IsNot Nothing Then
                impClaimPerilType.Recovery = Array.ConvertAll(
                                                     msgClaimPerilType.Recovery.ToArray,
                                                     New Converter(Of BaseClaimPerilRecoveryType,
                                                         BaseImplementationTypes.BaseClaimPerilRecoveryType) _
                                                         (AddressOf CommonFunctions.ToBaseImpBaseClaimPerilRecoveryType))

            End If

        End If

        Return impClaimPerilType

    End Function
    Public Shared Function ToBaseImpBaseClaimPerilReserveType(ByVal oReserveList As BaseClaimPerilReserveType) As BaseImplementationTypes.BaseClaimPerilReserveType

        Dim oServiceReserveList As BaseImplementationTypes.BaseClaimPerilReserveType = New BaseImplementationTypes.BaseClaimPerilReserveType
        If oReserveList IsNot Nothing Then
            With oReserveList
                oServiceReserveList.RevisionAmount = .RevisionAmount
                oServiceReserveList.TypeCode = .TypeCode
                oServiceReserveList.GrossReserve = .GrossReserve
                oServiceReserveList.Tax = .Tax
                oServiceReserveList.RevisedGrossReserve = .RevisedGrossReserve
                oServiceReserveList.RevisedTaxReserve = .RevisedTaxReserve
            End With
        End If
        Return oServiceReserveList
    End Function
    Public Shared Function ToBaseImpBaseClaimPerilRecoveryType(ByVal oRecoveryList As BaseClaimPerilRecoveryType) As BaseImplementationTypes.BaseClaimPerilRecoveryType

        Dim oServiceRecoveryList As BaseImplementationTypes.BaseClaimPerilRecoveryType = New BaseImplementationTypes.BaseClaimPerilRecoveryType
        If oRecoveryList IsNot Nothing Then
            With oRecoveryList
                oServiceRecoveryList.BaseRecoveryKey = .BaseRecoveryKey
                oServiceRecoveryList.Initialamount = .Initialamount
                oServiceRecoveryList.IsDeletedRecovery = .IsDeletedRecovery
                oServiceRecoveryList.RevisionAmount = .RevisionAmount
                oServiceRecoveryList.TypeCode = .TypeCode
                oServiceRecoveryList.IsNew = .IsNew
            End With
        End If
        Return oServiceRecoveryList
    End Function

    Public Shared Function ToServiceImpBaseClaimResponseTypeWarnings(ByVal oWarnings As BaseImplementationTypes.BaseClaimResponseTypeWarnings) As BaseClaimResponseTypeWarnings

        Dim oServiceImpClaimPerilRecoveryType As BaseClaimResponseTypeWarnings = New BaseClaimResponseTypeWarnings

        If oWarnings IsNot Nothing Then

            oServiceImpClaimPerilRecoveryType.Code = oWarnings.Code
            oServiceImpClaimPerilRecoveryType.Description = oWarnings.Description

        End If

        Return oServiceImpClaimPerilRecoveryType

    End Function


    Public Shared Function ToServiceBaseBankGuaranteeItemTypeProductsList(ByVal oProductsList As SFI.SAMForInsuranceV2.BaseBankGuaranteeItemTypeProducts) As BaseBankGuaranteeItemTypeProducts

        Dim oServiceoProductsList As BaseBankGuaranteeItemTypeProducts = New BaseBankGuaranteeItemTypeProducts
        If oProductsList IsNot Nothing Then
            With oProductsList
                oServiceoProductsList.Description = .Description
                oServiceoProductsList.ProductCode = .ProductCode

            End With
        End If
        Return oServiceoProductsList
    End Function

    Public Shared Function ToServiceBankGuaranteeItemTypeBranchesList(ByVal oBranchesList As SFI.SAMForInsuranceV2.BaseBankGuaranteeItemTypeBranches) As BaseBankGuaranteeItemTypeBranches

        Dim oServiceBranchesList As BaseBankGuaranteeItemTypeBranches = New BaseBankGuaranteeItemTypeBranches
        If oBranchesList IsNot Nothing Then
            With oBranchesList
                oServiceBranchesList.Description = .Description
                oServiceBranchesList.BranchCode = .BranchCode

            End With
        End If
        Return oServiceBranchesList
    End Function
    Public Shared Function ToBaseClaimPaymentTaxItemType(ByVal oClaimPaymentTaxItem As BaseClaimPaymentTaxItemType) As BaseImplementationTypes.BaseClaimPaymentTaxItemType

        Dim oServiceClaimPaymentTaxItemsList As BaseImplementationTypes.BaseClaimPaymentTaxItemType = New BaseImplementationTypes.BaseClaimPaymentTaxItemType

        oServiceClaimPaymentTaxItemsList.Amount = oClaimPaymentTaxItem.Amount
        oServiceClaimPaymentTaxItemsList.Percentage = oClaimPaymentTaxItem.Percentage
        oServiceClaimPaymentTaxItemsList.ReserveType = oClaimPaymentTaxItem.ReserveType
        oServiceClaimPaymentTaxItemsList.TaxBandCode = oClaimPaymentTaxItem.TaxBandCode
        oServiceClaimPaymentTaxItemsList.TaxGroupCode = oClaimPaymentTaxItem.TaxGroupCode
        oServiceClaimPaymentTaxItemsList.ClassOfBusinessID = oClaimPaymentTaxItem.ClassOfBusinessID
        oServiceClaimPaymentTaxItemsList.Sequence = oClaimPaymentTaxItem.Sequence
        oServiceClaimPaymentTaxItemsList.IsManuallyChanges = oClaimPaymentTaxItem.IsManuallyChanges
        oServiceClaimPaymentTaxItemsList.TaxBandId = oClaimPaymentTaxItem.TaxBandId
        oServiceClaimPaymentTaxItemsList.TaxGroupId = oClaimPaymentTaxItem.TaxGroupId

        Return oServiceClaimPaymentTaxItemsList
    End Function

    Public Shared Function ToBaseClaimPaymentItemType(ByVal oClaimPaymentItem As BaseClaimPaymentItemType) As BaseImplementationTypes.BaseClaimPaymentItemType

        Dim oServiceClaimPaymentItemList As BaseImplementationTypes.BaseClaimPaymentItemType = New BaseImplementationTypes.BaseClaimPaymentItemType
        With oClaimPaymentItem
            oServiceClaimPaymentItemList.BaseReserveKey = .BaseReserveKey
            oServiceClaimPaymentItemList.PaymentAmount = .PaymentAmount
            oServiceClaimPaymentItemList.ReverseExcess = .ReverseExcess
            oServiceClaimPaymentItemList.TaxGroupCode = .TaxGroupCode
            oServiceClaimPaymentItemList.IsTaxOverridden = .IsTaxOverridden
            oServiceClaimPaymentItemList.OverriddedTaxAmount = .OverriddedTaxAmount
        End With
        Return oServiceClaimPaymentItemList
    End Function


    Public Shared Function ToBasePaymentCashListItemType(ByVal oPaymentCashListItem As BasePaymentCashListItemType) As BaseImplementationTypes.BasePaymentCashListItemType

        Dim oServicePaymentCashListItemList As BaseImplementationTypes.BasePaymentCashListItemType = New BaseImplementationTypes.BasePaymentCashListItemType
        With oPaymentCashListItem
            oServicePaymentCashListItemList.StatusCode = .StatusCode
            oServicePaymentCashListItemList.TypeCode = .TypeCode
            oServicePaymentCashListItemList.TransactionDate = .TransactionDate
            oServicePaymentCashListItemList.MediaTypeCode = .MediaTypeCode
            oServicePaymentCashListItemList.MediaReference = .MediaReference
            oServicePaymentCashListItemList.OurReference = .OurReference
            oServicePaymentCashListItemList.TheirReference = .TheirReference
            oServicePaymentCashListItemList.AccountShortCode = .AccountShortCode
            oServicePaymentCashListItemList.AllocationStatusCode = .AllocationStatusCode
            oServicePaymentCashListItemList.Amount = .Amount
            oServicePaymentCashListItemList.ContactName = .ContactName
            oServicePaymentCashListItemList.FurtherDetails = .FurtherDetails

            'Address Details
            If .ContactAddress IsNot Nothing Then
                oServicePaymentCashListItemList.ContactAddress = New BaseImplementationTypes.BaseSimpleAddressType
                With .ContactAddress
                    oServicePaymentCashListItemList.ContactAddress.AddressLine1 = .AddressLine1
                    oServicePaymentCashListItemList.ContactAddress.AddressLine2 = .AddressLine2
                    oServicePaymentCashListItemList.ContactAddress.AddressLine3 = .AddressLine3
                    oServicePaymentCashListItemList.ContactAddress.AddressLine4 = .AddressLine4
                    oServicePaymentCashListItemList.ContactAddress.PostCode = .PostCode
                    oServicePaymentCashListItemList.ContactAddress.CountryCode = .CountryCode
                End With

            End If

            'Bank Details
            If (.Bank IsNot Nothing) Then
                oServicePaymentCashListItemList.Bank = New BaseImplementationTypes.BaseBankPaymentType
                With .Bank
                    oServicePaymentCashListItemList.Bank.PayeeName = .PayeeName
                    oServicePaymentCashListItemList.Bank.AccountCode = .AccountCode
                    oServicePaymentCashListItemList.Bank.BranchCode = .BranchCode
                    oServicePaymentCashListItemList.Bank.ExpiryDateSpecified = .ExpiryDateSpecified
                    oServicePaymentCashListItemList.Bank.ExpiryDate = .ExpiryDate
                    oServicePaymentCashListItemList.Bank.Reference1 = .Reference1
                    oServicePaymentCashListItemList.Bank.Reference2 = .Reference2
                    oServicePaymentCashListItemList.Bank.BIC = .BIC
                    oServicePaymentCashListItemList.Bank.IBAN = .IBAN
                End With

            End If

            'Credit Card Details
            If (.CreditCard IsNot Nothing) Then
                oServicePaymentCashListItemList.CreditCard = New BaseImplementationTypes.BaseCreditCardType
                With .CreditCard
                    oServicePaymentCashListItemList.CreditCard.Number = .Number
                    oServicePaymentCashListItemList.CreditCard.NameOnCreditCard = .NameOnCreditCard
                    oServicePaymentCashListItemList.CreditCard.ExpiryDate = .ExpiryDate
                    oServicePaymentCashListItemList.CreditCard.StartDate = .StartDate
                    oServicePaymentCashListItemList.CreditCard.Issue = .Issue

                    oServicePaymentCashListItemList.CreditCard.TrackingNumber = .TrackingNumber

                    oServicePaymentCashListItemList.CreditCard.PartyBankKey = .PartyBankKey

                End With

            End If
        End With


        Return oServicePaymentCashListItemList
    End Function

    Public Shared Function ToBaseClaimProcessResponseTypeWarningsList(ByVal oBaseClaimProcessResponseTypeWarningsList As BaseImplementationTypes.BaseClaimProcessResponseTypeWarnings) As BaseClaimProcessResponseTypeWarnings

        Dim oServiceBaseClaimProcessResponseTypeWarningsList As BaseClaimProcessResponseTypeWarnings = New BaseClaimProcessResponseTypeWarnings
        With oBaseClaimProcessResponseTypeWarningsList
            oServiceBaseClaimProcessResponseTypeWarningsList.Code = .Code
            oServiceBaseClaimProcessResponseTypeWarningsList.Description = .Description
        End With
        Return oServiceBaseClaimProcessResponseTypeWarningsList
    End Function

    Public Shared Function ToBaseDocumentType(ByVal oBaseDocumentTypeList As BaseImplementationTypes.BaseDocumentType) As BaseDocumentType

        Dim oServiceBaseDocumentTypeList As BaseDocumentType = New BaseDocumentType
        With oBaseDocumentTypeList
            oServiceBaseDocumentTypeList.CreateDate = .CreateDate
            oServiceBaseDocumentTypeList.DocDescription = .DocDescription
            oServiceBaseDocumentTypeList.DocNum = .DocNum
            oServiceBaseDocumentTypeList.DocumentType = .DocumentType
            oServiceBaseDocumentTypeList.FolderNum = .FolderNum
            oServiceBaseDocumentTypeList.FolderPath = .FolderPath
        End With
        Return oServiceBaseDocumentTypeList
    End Function
    Public Shared Function ToBaseDMEFolderType(ByVal oBaseDMEFolderTypeList As BaseImplementationTypes.BaseDMEFolderType) As BaseDMEFolderType

        Dim oServiceBaseDMEFolderTypeList As BaseDMEFolderType = New BaseDMEFolderType
        With oBaseDMEFolderTypeList
            oServiceBaseDMEFolderTypeList.CreateDate = .CreateDate
            oServiceBaseDMEFolderTypeList.ExternalCode = .ExternalCode
            oServiceBaseDMEFolderTypeList.FolderLevel = .FolderLevel
            oServiceBaseDMEFolderTypeList.FolderNum = .FolderNum
            oServiceBaseDMEFolderTypeList.Name = .Name
            oServiceBaseDMEFolderTypeList.ParentNum = .ParentNum
        End With
        Return oServiceBaseDMEFolderTypeList
    End Function

    Public Shared Function ToBaseImpBaseCDTClaimType(ByVal service As BaseCDTClaimType) _
        As BaseImplementationTypes.BaseCDTClaimType

        Dim implementation As BaseImplementationTypes.BaseCDTClaimType = Nothing

        If service IsNot Nothing Then

            implementation = New BaseImplementationTypes.BaseCDTClaimType

            implementation.CatastropheCode = service.CatastropheCode
            implementation.ClaimNumber = service.ClaimNumber
            implementation.ClaimVersionDescription = service.ClaimVersionDescription
            implementation.Comments = service.Comments
            implementation.CurrencyCode = service.CurrencyCode
            implementation.Description = service.Description
            implementation.HandlerCode = service.HandlerCode
            implementation.InfoOnly = service.InfoOnly
            implementation.LikelyClaim = service.LikelyClaim
            implementation.Location = service.Location
            implementation.LossFromDate = service.LossFromDate
            implementation.LossToDate = service.LossToDate
            implementation.LossToDateSpecified = service.LossToDateSpecified
            implementation.PrimaryCauseCode = service.PrimaryCauseCode
            implementation.ProgressStatusCode = service.ProgressStatusCode
            implementation.ReportedDate = service.ReportedDate
            implementation.SAMStagingClaimKey = service.SAMStagingClaimKey
            implementation.SecondaryCauseCode = service.SecondaryCauseCode
            implementation.SiriusInsuranceFileKey = service.SiriusInsuranceFileKey
            implementation.SiriusRiskKey = service.SiriusRiskKey
            implementation.TownCode = service.TownCode
            implementation.UnderwritingYearCode = service.UnderwritingYearCode
            implementation.XMLDATASET = service.XMLDATASET

            implementation.TransactionDateSpecified = True
            If service.TransactionDateSpecified Then
                implementation.TransactionDate = service.TransactionDate
            Else
                implementation.TransactionDate = Date.Now
            End If

            implementation.VersionNoSpecified = True
            If service.VersionNoSpecified Then
                implementation.VersionNo = service.VersionNo
            Else
                implementation.VersionNo = 0
            End If

            If service.ClaimPeril IsNot Nothing Then
                implementation.ClaimPeril = Array.ConvertAll(service.ClaimPeril.ToArray,
                                        New Converter(Of BaseCDTClaimPerilType,
                                                BaseImplementationTypes.BaseCDTClaimPerilType) _
                                            (AddressOf ToBaseImpBaseCDTClaimPerilType))

            End If

            If service.ClaimReinsurance IsNot Nothing Then
                implementation.ClaimReinsuranceForDTU = ToBaseImpBaseCDTClaimReinsuranceType(service.ClaimReinsurance)
            End If

        End If

        Return implementation

    End Function

    Public Shared Function ToBaseImpBaseCDTClaimPerilType(ByVal service As BaseCDTClaimPerilType) _
        As BaseImplementationTypes.BaseCDTClaimPerilType

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimPerilType

        If service IsNot Nothing Then

            implementation.SAMStagingBaseClaimPerilKey = service.BaseClaimPerilKey
            implementation.SAMStagingClaimPerilKey = service.SAMStagingClaimPerilKey
            implementation.Description = service.Description
            implementation.TypeCode = service.TypeCode

            If service.ClaimPayment IsNot Nothing Then
                implementation.ClaimPayment = Array.ConvertAll(service.ClaimPayment.ToArray,
                                                    New Converter(Of BaseCDTClaimPaymentType, BaseImplementationTypes.BaseCDTClaimPaymentType) _
                                                    (AddressOf ToBaseImpBaseCDTClaimPaymentType))
            End If

            If service.ClaimReceipt IsNot Nothing Then
                implementation.ClaimReceipt = Array.ConvertAll(service.ClaimReceipt.ToArray,
                                                    New Converter(Of BaseCDTClaimReceiptType, BaseImplementationTypes.BaseCDTClaimReceiptType) _
                                                    (AddressOf ToBaseImpBaseCDTClaimReceiptType))
            End If

            If service.Recovery IsNot Nothing Then
                implementation.Recovery = Array.ConvertAll(service.Recovery.ToArray,
                                                    New Converter(Of BaseCDTRecoveryType, BaseImplementationTypes.BaseCDTRecoveryType) _
                                                    (AddressOf ToBaseImpBaseCDTRecoveryType))
            End If

            If service.Reserve IsNot Nothing Then
                implementation.Reserve = Array.ConvertAll(service.Reserve.ToArray,
                                                New Converter(Of BaseCDTReserveType, BaseImplementationTypes.BaseCDTReserveType) _
                                                    (AddressOf ToBaseImpBaseCDTReserveType))
            End If

        End If

        Return implementation

    End Function
    Public Shared Function ToBaseImpBaseCDTClaimReinsuranceType(ByVal service As BaseCDTClaimReinsuranceType) _
        As BaseImplementationTypes.BaseCDTClaimReinsuranceTypeForDTU

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimReinsuranceTypeForDTU

        If service IsNot Nothing Then

            implementation.ClaimRIArrangement = ToBaseImpCDTCLaimReinsuranceTypeRIArrangement(service.ClaimRIArrangement)

        End If

        Return implementation

    End Function

    Public Shared Function ToBaseImpBaseCDTClaimPaymentType(ByVal service As BaseCDTClaimPaymentType) _
        As BaseImplementationTypes.BaseCDTClaimPaymentType

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimPaymentType

        If service.ClaimPaymentItem IsNot Nothing Then
            implementation.ClaimPaymentItem = Array.ConvertAll(service.ClaimPaymentItem.ToArray,
                                    New Converter(Of BaseCDTClaimPaymentItemType,
                                    BaseImplementationTypes.BaseCDTClaimPaymentItemType) _
                                        (AddressOf ToBaseImpBaseCDTClaimPaymentItemType))

            implementation.ClaimReinsuranceForDTU = ToBaseImpBaseCDTClaimReinsuranceType(service.ClaimReinsurance)
            implementation.CurrencyCode = service.CurrencyCode
            implementation.PartyKey = service.PartyKey
            implementation.TransactionDate = service.TransactionDate
            implementation.TransactionDateSpecified = service.TransactionDateSpecified

            If service.Payee IsNot Nothing Then
                implementation.Payee = ToBaseImpBaseClaimPayeeType(service.Payee)
            End If

            implementation.PaymentPartyType = CType([Enum].ToObject(GetType(ClaimPaymentPartyTypeType), service.PaymentPartyType), BaseImplementationTypes.ClaimPaymentPartyTypeType)
            implementation.SAMStagingClaimPaymentKey = service.SAMStagingClaimPaymentKey
        End If

        Return implementation

    End Function

    Public Shared Function ToBaseImpBaseCDTClaimReceiptType(ByVal service As BaseCDTClaimReceiptType) _
        As BaseImplementationTypes.BaseCDTClaimReceiptType

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimReceiptType

        If service IsNot Nothing Then

            If service.ClaimReceiptItem IsNot Nothing Then
                implementation.ClaimReceiptItem = Array.ConvertAll(service.ClaimReceiptItem.ToArray,
                                                    New Converter(Of BaseCDTReceiptItemType,
                                                     BaseImplementationTypes.BaseCDTReceiptItemType) _
                                                        (AddressOf ToBaseImpBaseCDTReceiptITemType))
            End If

            If service.ClaimReinsurance IsNot Nothing Then
                implementation.ClaimReinsuranceForDTU = ToBaseImpBaseCDTClaimReinsuranceType(service.ClaimReinsurance)
            End If

            implementation.CurrencyCode = service.CurrencyCode
            implementation.PartyKey = service.PartyKey
            implementation.TransactionDate = service.TransactionDate
            implementation.TransactionDateSpecified = service.TransactionDateSpecified
            implementation.IsSalvageRecovery = service.IsSalvageRecovery

            If service.Payee IsNot Nothing Then
                implementation.Payee = ToBaseImpBaseClaimPayeeType(service.Payee)
            End If

            implementation.ReceiptPartyType = CType([Enum].ToObject(GetType(ClaimReceiptPartyTypeType), service.ReceiptPartyType), BaseImplementationTypes.ClaimReceiptPartyTypeType)

            'implementation.ReceiptPartyType = service.ReceiptPartyType
            implementation.SAMStagingClaimReceiptKey = service.SAMStagingClaimReceiptKey

        End If

        Return implementation

    End Function

    Public Shared Function ToBaseImpBaseCDTRecoveryType(ByVal service As BaseCDTRecoveryType) _
           As BaseImplementationTypes.BaseCDTRecoveryType

        Dim implementation As New BaseImplementationTypes.BaseCDTRecoveryType

        If service IsNot Nothing Then

            implementation.RevisionAmount = service.RevisionAmount
            implementation.SAMStagingRecoveryKey = service.SAMStagingRecoveryKey
            implementation.TypeCode = service.TypeCode

        End If

        Return implementation

    End Function

    Public Shared Function ToBaseImpBaseCDTReserveType(ByVal service As BaseCDTReserveType) _
       As BaseImplementationTypes.BaseCDTReserveType

        Dim implementation As New BaseImplementationTypes.BaseCDTReserveType

        If service IsNot Nothing Then
            implementation.RevisionAmount = service.RevisionAmount
            implementation.SAMStagingReserveKey = service.SAMStagingReserveKey
            implementation.TypeCode = service.TypeCode
        End If

        Return implementation

    End Function

    Public Shared Function ToBaseImpCDTCLaimReinsuranceTypeRIArrangement(ByVal service() As BaseCDTClaimReinsuranceTypeClaimRIArrangement) _
            As List(Of BaseImplementationTypes.BaseCDTClaimReinsuranceTypeClaimRIArrangement)

        Dim implementation As New List(Of BaseImplementationTypes.BaseCDTClaimReinsuranceTypeClaimRIArrangement)
        Dim implementation1 As New BaseImplementationTypes.BaseCDTClaimReinsuranceTypeClaimRIArrangement
        Dim i As Integer = 0

        If service IsNot Nothing Then
            For i = 0 To service.Length - 1
                implementation1.ClaimAllocationType = service(i).ClaimAllocationType
                implementation1.ClaimRIArrangmentLine = Array.ConvertAll(service(i).ClaimRIArrangmentLine.ToArray,
                                                            New Converter(Of BaseCDTClaimRIArrangmentLineType,
                                                             BaseImplementationTypes.BaseCDTClaimRIArrangmentLineType) _
                                                                (AddressOf ToBaseImpBaseCDTClaimRIArrangmentLineType))

                implementation1.Payment = service(i).Payment
                implementation1.Recovery = service(i).Recovery
                implementation1.Reserve = service(i).Reserve
                implementation1.RIArrangementKey = service(i).RIArrangementKey
                implementation1.RIBandCode = service(i).RIBandCode
                implementation1.RIModelCode = service(i).RIModelCode
                implementation1.Salvage = service(i).Salvage
                implementation1.SAMStagingClaimRIArrangementKey = service(i).SAMStagingClaimRIArrangementKey
                implementation1.SumInsured = service(i).SumInsured
                implementation1.ThisPayment = service(i).ThisPayment
                implementation1.ThisRecovery = service(i).ThisRecovery
                implementation1.ThisReserve = service(i).ThisReserve
                implementation1.ThisSalvage = service(i).ThisSalvage
                implementation.Add(implementation1)
            Next
        End If

        Return implementation

    End Function
    Public Shared Function ToBaseImpBaseCDTClaimPaymentItemType(ByVal service As BaseCDTClaimPaymentItemType) _
        As BaseImplementationTypes.BaseCDTClaimPaymentItemType

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimPaymentItemType

        If service IsNot Nothing Then

            implementation.PaymentAmount = service.PaymentAmount
            implementation.ReserveTypeCode = service.ReserveTypeCode
            implementation.ReverseExcess = service.ReverseExcess
            implementation.SAMStagingClaimPaymentItemKey = service.SAMStagingClaimPaymentItemKey
            implementation.TaxGroupCode = service.TaxGroupCode

        End If

        Return implementation

    End Function

    ''' <summary>
    ''' Convert service claim payee into base type
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseClaimPayeeType(ByVal oService As BaseClaimPayeeType) As BaseImplementationTypes.BaseClaimPayeeType

        Dim oImplementation As New BaseImplementationTypes.BaseClaimPayeeType

        With oService
            oImplementation.Address = ToBaseImpBaseClaimPayeeAddressType(.Address)
            oImplementation.BankCode = .BankCode
            oImplementation.BankName = .BankName
            oImplementation.BankNumber = .BankNumber
            oImplementation.Comments = .Comments
            oImplementation.MediaReference = .MediaReference()
            oImplementation.MediaTypeCode = .MediaTypeCode
            oImplementation.Name = .Name
            oImplementation.TheirReference = .TheirReference
            oImplementation.BIC = .BIC
            oImplementation.IBAN = .IBAN
        End With

        Return oImplementation

    End Function

    Public Shared Function ToBaseImpBaseCDTReceiptITemType(ByVal service As BaseCDTReceiptItemType) _
        As BaseImplementationTypes.BaseCDTReceiptItemType

        Dim implementation As New BaseImplementationTypes.BaseCDTReceiptItemType

        If service IsNot Nothing Then

            implementation.ReceiptAmount = service.ReceiptAmount
            implementation.RecoveryTypeCode = service.RecoveryTypeCode
            implementation.SAMStagingClaimReceiptItemKey = service.SAMStagingClaimReceiptItemKey
            implementation.TaxGroupCode = service.TaxGroupCode

        End If

        Return implementation
    End Function
    Public Shared Function ToBaseImpBaseCDTClaimRIArrangmentLineType(ByVal service As BaseCDTClaimRIArrangmentLineType) _
            As BaseImplementationTypes.BaseCDTClaimRIArrangmentLineType

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimRIArrangmentLineType

        If service IsNot Nothing Then

            implementation.AgreementCode = service.AgreementCode
            implementation.DefaultSharePercent = service.DefaultSharePercent
            implementation.Grouping = service.Grouping
            implementation.LineLimit = service.LineLimit
            implementation.LowerLimit = service.LowerLimit
            implementation.NumberOfLines = service.NumberOfLines
            implementation.ParticipationPercent = service.ParticipationPercent
            implementation.PartyKey = service.PartyKey
            implementation.Payment = service.Payment
            implementation.Priority = service.Priority
            implementation.Recovery = service.Recovery
            implementation.Reserve = service.Reserve
            implementation.Retained = service.Retained
            implementation.Salvage = service.Salvage
            implementation.SAMStagingClaimRIArrangementKey = service.SAMStagingClaimRIArrangementKey
            implementation.SAMStagingClaimRIArrangementLineKey = service.SAMStagingClaimRIArrangementLineKey
            implementation.SumInsured = service.SumInsured
            implementation.ThisPayment = service.ThisPayment
            implementation.ThisRecovery = service.ThisRecovery
            implementation.ThisReserve = service.ThisReserve
            implementation.ThisSalvage = service.ThisSalvage
            implementation.ThisSharePercent = service.ThisSharePercent
            implementation.TreatyCode = service.TreatyCode
            implementation.Type = service.Type

        End If

        Return implementation

    End Function

    ''' <summary>
    ''' To convert from internal Source List to WCF Service Source List
    ''' </summary>
    ''' <param name="oSourceList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceSourceList(ByVal oSourceList As BaseImplementationTypes.BaseBranchType) As BaseBranchType

        Dim oServiceSourceList As BaseBranchType = New BaseBranchType

        If oSourceList IsNot Nothing Then
            oServiceSourceList.BranchCode = oSourceList.BranchCode
            oServiceSourceList.Description = oSourceList.Description
            oServiceSourceList.BranchKey = oSourceList.BranchKey
            oServiceSourceList.AgentCode = oSourceList.AgentCode
            oServiceSourceList.BusinessType = oSourceList.BusinessType
            oServiceSourceList.AgentKey = oSourceList.AgentKey

        End If

        Return oServiceSourceList

    End Function

    ''' <summary>
    ''' To convert from Internal AddressWithContactType to WCF Service AddressWithContactType
    ''' </summary>
    ''' <param name="oImplementation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceBaseAddressWithContactsType(ByVal oImplementation As BaseImplementationTypes.BaseAddressType) As BaseAddressWithContactsType

        Dim oService As New BaseAddressWithContactsType

        If oImplementation IsNot Nothing Then

            Dim oActualImplementation As BaseImplementationTypes.BaseAddressWithContactsType

            oActualImplementation = TryCast(oImplementation, BaseImplementationTypes.BaseAddressWithContactsType)

            If oActualImplementation IsNot Nothing Then
                oService.AddressLine1 = oActualImplementation.AddressLine1
                oService.AddressLine2 = oActualImplementation.AddressLine2
                oService.AddressLine3 = oActualImplementation.AddressLine3
                oService.AddressLine4 = oActualImplementation.AddressLine4
                oService.AddressLine5 = oActualImplementation.AddressLine5
                oService.AddressLine6 = oActualImplementation.AddressLine6
                oService.AddressLine7 = oActualImplementation.AddressLine7
                oService.AddressLine8 = oActualImplementation.AddressLine8
                oService.AddressLine9 = oActualImplementation.AddressLine9
                oService.AddressLine10 = oActualImplementation.AddressLine10

                oService.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), oActualImplementation.AddressTypeCode), AddressTypeType)
                oService.CountryCode = oActualImplementation.CountryCode
                oService.PostCode = oActualImplementation.PostCode

                If oActualImplementation.Contacts IsNot Nothing Then
                    oService.Contacts = oActualImplementation.Contacts.ToList().ConvertAll(
                                New Converter(Of BaseImplementationTypes.BaseContactType, BaseContactType) _
                                (AddressOf CommonFunctions.ToServiceContactType))
                End If
            Else
                oService.AddressLine1 = oImplementation.AddressLine1
                oService.AddressLine2 = oImplementation.AddressLine2
                oService.AddressLine3 = oImplementation.AddressLine3
                oService.AddressLine4 = oImplementation.AddressLine4
                oService.AddressLine5 = oImplementation.AddressLine5
                oService.AddressLine6 = oImplementation.AddressLine6
                oService.AddressLine7 = oImplementation.AddressLine7
                oService.AddressLine8 = oImplementation.AddressLine8
                oService.AddressLine9 = oImplementation.AddressLine9
                oService.AddressLine10 = oImplementation.AddressLine10
                oService.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), oImplementation.AddressTypeCode), AddressTypeType)
                oService.CountryCode = oImplementation.CountryCode
                oService.PostCode = oImplementation.PostCode
                oService.Contacts = Nothing
            End If
        End If

        Return oService

    End Function

    ''' <summary>
    ''' To convert from Internal ContactType to WCF Service ContactType
    ''' </summary>
    ''' <param name="oContact"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceContactType(ByVal oContact As BaseImplementationTypes.BaseContactType) As BaseContactType

        Dim oServiceContact As BaseContactType = New BaseContactType

        If oContact IsNot Nothing Then

            oServiceContact.AreaCode = oContact.AreaCode
            oServiceContact.Description = oContact.Description
            oServiceContact.ContactDetail = New BaseContactDetailType
            oServiceContact.ContactDetail.Item = oContact.ContactDetail.Item
            oServiceContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(BaseImplementationTypes.ItemChoiceType), oContact.ContactDetail.ItemElementName), ItemChoiceType)
            If oContact.ContactTypeCode <> BaseImplementationTypes.ContactTypeType.OTHER Then
                oServiceContact.ContactTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.ContactTypeType), oContact.ContactTypeCode), ContactTypeType)
            Else
                oServiceContact.ContactTypeCode = ContactTypeType.OTHER
                oServiceContact.OtherContactTypeCode = Trim(oContact.OtherContactTypeCode)
            End If
            oServiceContact.ContactTypeDescription = oContact.ContactTypeDescription
            oServiceContact.Extension = oContact.Extension
        End If

        Return oServiceContact

    End Function

    ''' <summary>
    ''' TO convert from InternalAssociateType to WCF Service AssociateType
    ''' </summary>
    ''' <param name="oAssociate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceAssociateType(
            ByVal oAssociate As BaseImplementationTypes.BaseAssociateType) As BaseAssociateType

        Dim oServiceAssociate As BaseAssociateType = New BaseAssociateType

        If oAssociate IsNot Nothing Then

            oServiceAssociate.ClientKey = oAssociate.ClientKey
            oServiceAssociate.AssociateKey = oAssociate.AssociateKey
            oServiceAssociate.RelationshipCode = oAssociate.RelationshipCode
            oServiceAssociate.RelationshipDescription = oAssociate.RelationshipDescription

            oServiceAssociate.AssociateCode = oAssociate.AssociateCode
            oServiceAssociate.AssociateName = oAssociate.AssociateName
            oServiceAssociate.AccountBalance = oAssociate.AccountBalance
            oServiceAssociate.ClaimIncurred = oAssociate.ClaimIncurred
            oServiceAssociate.CurrencyCode = oAssociate.CurrencyCode
        End If

        Return oServiceAssociate

    End Function

    ''' <summary>
    ''' To convert from Internal ConvictionType to WCF Service ConvictionType
    ''' </summary>
    ''' <param name="oConviction"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceConvictionType(
            ByVal oConviction As BaseImplementationTypes.BaseConvictionType) As BaseConvictionType

        Dim oServiceConviction As BaseConvictionType = New BaseConvictionType

        If oConviction IsNot Nothing Then

            oServiceConviction.AlcoholLevel = oConviction.AlcoholLevel
            oServiceConviction.AlcoholLevelSpecified = oConviction.AlcoholLevelSpecified
            oServiceConviction.AlcoholMeasurementMethod = oConviction.AlcoholMeasurementMethod
            oServiceConviction.ConvictionKey = oConviction.ConvictionKey
            oServiceConviction.Date = oConviction.Date
            oServiceConviction.Description = oConviction.Description
            oServiceConviction.DrivingLicensePenaltyPoints = oConviction.DrivingLicensePenaltyPoints
            oServiceConviction.DrivingLicensePenaltyPointsSpecified = oConviction.DrivingLicensePenaltyPointsSpecified
            oServiceConviction.FineAmount = oConviction.FineAmount
            oServiceConviction.FineAmountSpecified = oConviction.FineAmountSpecified
            oServiceConviction.SentenceDescription = oConviction.SentenceDescription
            oServiceConviction.SentenceDuration = oConviction.SentenceDuration
            oServiceConviction.SentenceDurationQualifier = oConviction.SentenceDurationQualifier
            oServiceConviction.SentenceDurationSpecified = oConviction.SentenceDurationSpecified
            oServiceConviction.SentenceEffectiveDate = oConviction.SentenceEffectiveDate
            oServiceConviction.SentenceEffectiveDateSpecified = oConviction.SentenceEffectiveDateSpecified
            oServiceConviction.SentenceTypeCode = oConviction.SentenceTypeCode
            oServiceConviction.StatusCode = oConviction.StatusCode
            oServiceConviction.TypeCode = oConviction.TypeCode

        End If

        Return oServiceConviction

    End Function

    ''' <summary>
    ''' To convert from internal LoyaltyScheme to WCF service LoyaltyScheme
    ''' </summary>
    ''' <param name="oLoyaltyScheme"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceLoyaltyScheme(
                ByVal oLoyaltyScheme As BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme) As BaseClientSharedDataTypeLoyaltyScheme

        Dim oServiceLoyaltyScheme As BaseClientSharedDataTypeLoyaltyScheme = New BaseClientSharedDataTypeLoyaltyScheme

        If oLoyaltyScheme IsNot Nothing Then

            oServiceLoyaltyScheme.LoyaltySchemeKey = oLoyaltyScheme.LoyaltySchemeKey
            oServiceLoyaltyScheme.LoyaltySchemeCode = oLoyaltyScheme.LoyaltySchemeCode
            oServiceLoyaltyScheme.MembershipNumber = oLoyaltyScheme.MembershipNumber
            oServiceLoyaltyScheme.OtherReference = oLoyaltyScheme.OtherReference
            oServiceLoyaltyScheme.StartDate = oLoyaltyScheme.StartDate
            oServiceLoyaltyScheme.EndDate = oLoyaltyScheme.EndDate
            oServiceLoyaltyScheme.EndDateSpecified = oLoyaltyScheme.EndDateSpecified
            oServiceLoyaltyScheme.MainMember = oLoyaltyScheme.MainMember
            oServiceLoyaltyScheme.Active = oLoyaltyScheme.Active
            oServiceLoyaltyScheme.ActiveSpecified = oLoyaltyScheme.ActiveSpecified

        End If

        Return oServiceLoyaltyScheme

    End Function

    ''' <summary>
    ''' To convert from internal ProspectPolicies to WCF service ProspectPolicies
    ''' </summary>
    ''' <param name="oProspectPolicies"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceProspectPolicies(
                ByVal oProspectPolicies As BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies) As BaseClientSharedDataTypeProspectPolicies

        Dim oServiceProspectPolicies As BaseClientSharedDataTypeProspectPolicies = New BaseClientSharedDataTypeProspectPolicies

        If oProspectPolicies IsNot Nothing Then

            oServiceProspectPolicies.ProspectPolicyKey = oProspectPolicies.ProspectPolicyKey
            oServiceProspectPolicies.ProspectTypeCode = oProspectPolicies.ProspectTypeCode
            oServiceProspectPolicies.RenewalDate = oProspectPolicies.RenewalDate
            oServiceProspectPolicies.RenewalDateSpecified = oProspectPolicies.RenewalDateSpecified
            oServiceProspectPolicies.TimesQuoted = oProspectPolicies.TimesQuoted
            oServiceProspectPolicies.TimesQuotedSpecified = oProspectPolicies.TimesQuotedSpecified
            oServiceProspectPolicies.TargetPremium = oProspectPolicies.TargetPremium
            oServiceProspectPolicies.TargetPremiumSpecified = oProspectPolicies.TargetPremiumSpecified

        End If

        Return oServiceProspectPolicies

    End Function

    ''' <summary>
    ''' To convert from Internal Accident to WCF service Accident
    ''' </summary>
    ''' <param name="oAccident"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceAccident(ByVal oAccident As BaseImplementationTypes.BasePartyOTHERTypeAccident) As BasePartyOTHERTypeAccident

        Dim oServiceAccident As BasePartyOTHERTypeAccident = New BasePartyOTHERTypeAccident

        If oAccident IsNot Nothing Then

            oServiceAccident.AccidentKey = oAccident.AccidentKey
            oServiceAccident.Date = oAccident.Date
            oServiceAccident.Description = oAccident.Description
            oServiceAccident.IsAtFault = oAccident.IsAtFault

        End If

        Return oServiceAccident

    End Function

    ''' <summary>
    ''' To convert from inetrnal SupplierBusiness to WCF Service SupplierBusiness
    ''' </summary>
    ''' <param name="oSupplierBusiness"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceSupplierBusiness(ByVal oSupplierBusiness As BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness) As BasePartyOTHERTypeSupplierBusiness

        Dim oServiceSupplierBusiness As BasePartyOTHERTypeSupplierBusiness = New BasePartyOTHERTypeSupplierBusiness

        If oSupplierBusiness IsNot Nothing Then

            oServiceSupplierBusiness.BusinessCode = oSupplierBusiness.BusinessCode
            oServiceSupplierBusiness.SpecialityCode = oSupplierBusiness.SpecialityCode

        End If

        Return oServiceSupplierBusiness

    End Function

    ''' <summary>
    ''' To convert from inetrnal SupplierBusiness to WCF Service SupplierBusiness
    ''' </summary>
    ''' <param name="oSupplierBusiness"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceBranch(ByVal oBranch As BaseImplementationTypes.BasePartyOTHERTypeBranch) As BasePartyOTHERTypeBranch

        Dim oServiceBranch As BasePartyOTHERTypeBranch = New BasePartyOTHERTypeBranch

        If oBranch IsNot Nothing Then

            oServiceBranch.BranchId = oBranch.BranchId
            oServiceBranch.Description = oBranch.Description

        End If

        Return oServiceBranch

    End Function

    ''' <summary>
    ''' To convert from internal BaseClientSharedDataTypeLifeStyle to WCF service BaseClientSharedDataTypeLifeStyle
    ''' </summary>
    ''' <param name="oImplementation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceBaseClientSharedDataTypeLifeStyle(
            ByVal oImplementation As BaseImplementationTypes.BasePartyPCTypeLifestyle) As BasePartyPCTypeLifestyle

        Dim oService As New BasePartyPCTypeLifestyle

        If oImplementation IsNot Nothing Then

            oService.LifestyleKey = oImplementation.LifestyleKey
            oService.Name = oImplementation.Name
            oService.DateOfBirth = oImplementation.DateOfBirth
            oService.DateOfBirthSpecified = oImplementation.DateOfBirthSpecified
            oService.CategoryCode = oImplementation.CategoryCode
            If oImplementation.GenderCode = BaseImplementationTypes.GenderCodeType.Male Then
                oService.GenderCode = GenderCodeType.M
            Else
                oService.GenderCode = GenderCodeType.F
            End If
            oService.GenderCodeSpecified = oImplementation.GenderCodeSpecified
            oService.OccupationCode = oImplementation.OccupationCode
            oService.SecOccupationCode = oImplementation.SecOccupationCode
            oService.Smoker = oImplementation.Smoker
            oService.SmokerSpecified = oImplementation.SmokerSpecified

        End If

        Return oService

    End Function



    ''' <summary>
    ''' To convert from WCF Service BaseConvictionTypes to Internal BaseConvictionTypes
    ''' </summary>
    ''' <param name="msgPartyConvictions"></param>
    ''' <param name="impPartyConvictions"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToBaseImpBaseConvictionTypes(ByRef msgPartyConvictions As List(Of BaseConvictionType),
                                                    ByRef impPartyConvictions As List(Of BaseImplementationTypes.BaseConvictionType))

        If (msgPartyConvictions) IsNot Nothing Then

            Dim msgConviction As BaseConvictionType
            Dim impConviction As BaseImplementationTypes.BaseConvictionType = Nothing

            For iCnt As Integer = 0 To msgPartyConvictions.Count - 1

                msgConviction = msgPartyConvictions(iCnt)
                impConviction = New BaseImplementationTypes.BaseConvictionType

                impConviction.AlcoholLevel = msgConviction.AlcoholLevel
                impConviction.AlcoholLevelSpecified = msgConviction.AlcoholLevelSpecified
                impConviction.AlcoholMeasurementMethod = msgConviction.AlcoholMeasurementMethod
                impConviction.Date = msgConviction.Date
                impConviction.Description = msgConviction.Description
                impConviction.DrivingLicensePenaltyPoints = msgConviction.DrivingLicensePenaltyPoints
                impConviction.DrivingLicensePenaltyPointsSpecified = msgConviction.DrivingLicensePenaltyPointsSpecified
                impConviction.FineAmount = msgConviction.FineAmount
                impConviction.FineAmountSpecified = msgConviction.FineAmountSpecified
                impConviction.SentenceDescription = msgConviction.SentenceDescription
                impConviction.SentenceDuration = msgConviction.SentenceDuration
                impConviction.SentenceDurationSpecified = msgConviction.SentenceDurationSpecified
                impConviction.SentenceDurationQualifier = msgConviction.SentenceDurationQualifier
                impConviction.SentenceEffectiveDate = msgConviction.SentenceEffectiveDate
                impConviction.SentenceEffectiveDateSpecified = msgConviction.SentenceEffectiveDateSpecified
                impConviction.SentenceTypeCode = msgConviction.SentenceTypeCode
                impConviction.StatusCode = msgConviction.StatusCode
                impConviction.TypeCode = msgConviction.TypeCode

                impPartyConvictions.Add(impConviction)

            Next iCnt

        End If

    End Sub

    ''' <summary>
    ''' To convert from WCF Service BaseAssociateType to internal BaseAssociateType
    ''' </summary>
    ''' <param name="msgAssociate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseAssociateType(ByVal msgAssociate As SFI.SAMForInsuranceV2.WCF.BaseAssociateType) As BaseImplementationTypes.BaseAssociateType

        Dim impAssociate As BaseImplementationTypes.BaseAssociateType = New BaseImplementationTypes.BaseAssociateType

        impAssociate.ClientKey = msgAssociate.ClientKey
        impAssociate.AssociateKey = msgAssociate.AssociateKey
        impAssociate.RelationshipCode = msgAssociate.RelationshipCode
        impAssociate.RelationshipDescription = msgAssociate.RelationshipDescription

        Return impAssociate

    End Function

    ''' <summary>
    ''' To convert from WCF Service BaseClientSharedDataTypeLoyaltyScheme to internal BaseClientSharedDataTypeLoyaltyScheme
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseClientSharedDataTypeLoyaltyScheme(
                ByVal oService As BaseClientSharedDataTypeLoyaltyScheme) As BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme

        Dim oImplementation As New BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme

        If oService IsNot Nothing Then

            oImplementation.LoyaltySchemeKey = oService.LoyaltySchemeKey
            oImplementation.LoyaltySchemeCode = oService.LoyaltySchemeCode
            oImplementation.MembershipNumber = oService.MembershipNumber
            oImplementation.OtherReference = oService.OtherReference
            oImplementation.StartDate = oService.StartDate
            oImplementation.EndDate = oService.EndDate
            oImplementation.EndDateSpecified = oService.EndDateSpecified
            oImplementation.MainMember = oService.MainMember
            oImplementation.Active = oService.Active
            oImplementation.ActiveSpecified = oService.ActiveSpecified

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF Service BaseClientSharedDataTypeProspectPolicies to internal BaseClientSharedDataTypeProspectPolicies
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseClientSharedDataTypeProspectPolicies(
                ByVal oService As BaseClientSharedDataTypeProspectPolicies) As BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies

        Dim oImplementation As New BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies

        If oService IsNot Nothing Then

            oImplementation.ProspectPolicyKey = oService.ProspectPolicyKey
            oImplementation.ProspectTypeCode = oService.ProspectTypeCode
            oImplementation.RenewalDate = oService.RenewalDate
            oImplementation.RenewalDateSpecified = oService.RenewalDateSpecified
            oImplementation.TimesQuoted = oService.TimesQuoted
            oImplementation.TimesQuotedSpecified = oService.TimesQuotedSpecified
            oImplementation.TargetPremium = oService.TargetPremium
            oImplementation.TargetPremiumSpecified = oService.TargetPremiumSpecified

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service BaseClientSharedDataTypeLifeStyle to internal BaseClientSharedDataTypeLifeStyle
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseClientSharedDataTypeLifeStyle(
            ByVal oService As BasePartyPCTypeLifestyle) As BaseImplementationTypes.BasePartyPCTypeLifestyle

        Dim oImplementation As New BaseImplementationTypes.BasePartyPCTypeLifestyle

        If oService IsNot Nothing Then

            oImplementation.LifestyleKey = oService.LifestyleKey
            oImplementation.Name = oService.Name
            oImplementation.DateOfBirth = oService.DateOfBirth
            oImplementation.DateOfBirthSpecified = oService.DateOfBirthSpecified
            oImplementation.CategoryCode = oService.CategoryCode
            If oService.GenderCode = GenderCodeType.M Then
                oImplementation.GenderCode = BaseImplementationTypes.GenderCodeType.Male
            ElseIf oService.GenderCode = GenderCodeType.F Then
                oImplementation.GenderCode = BaseImplementationTypes.GenderCodeType.Female
            Else
                oImplementation.GenderCode = BaseImplementationTypes.GenderCodeType.NotApplicable
            End If
            oImplementation.GenderCodeSpecified = oService.GenderCodeSpecified
            oImplementation.OccupationCode = oService.OccupationCode
            oImplementation.SecOccupationCode = oService.SecOccupationCode
            oImplementation.Smoker = oService.Smoker
            oImplementation.SmokerSpecified = oService.SmokerSpecified

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service BaseConvictionType to internal BaseConvictionType
    ''' </summary>
    ''' <param name="msgConviction"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseConvictionType(
            ByVal msgConviction As BaseConvictionType) As BaseImplementationTypes.BaseConvictionType

        Dim impConviction As BaseImplementationTypes.BaseConvictionType = New BaseImplementationTypes.BaseConvictionType

        If msgConviction IsNot Nothing Then

            impConviction.AlcoholLevel = msgConviction.AlcoholLevel
            impConviction.AlcoholLevelSpecified = msgConviction.AlcoholLevelSpecified
            impConviction.AlcoholMeasurementMethod = msgConviction.AlcoholMeasurementMethod
            impConviction.ConvictionKey = msgConviction.ConvictionKey
            impConviction.Date = msgConviction.Date
            impConviction.Description = msgConviction.Description
            impConviction.DrivingLicensePenaltyPoints = msgConviction.DrivingLicensePenaltyPoints
            impConviction.DrivingLicensePenaltyPointsSpecified = msgConviction.DrivingLicensePenaltyPointsSpecified
            impConviction.FineAmount = msgConviction.FineAmount
            impConviction.FineAmountSpecified = msgConviction.FineAmountSpecified
            impConviction.SentenceDescription = msgConviction.SentenceDescription
            impConviction.SentenceDuration = msgConviction.SentenceDuration
            impConviction.SentenceDurationQualifier = msgConviction.SentenceDurationQualifier
            impConviction.SentenceDurationSpecified = msgConviction.SentenceDurationSpecified
            impConviction.SentenceEffectiveDate = msgConviction.SentenceEffectiveDate
            impConviction.SentenceEffectiveDateSpecified = msgConviction.SentenceEffectiveDateSpecified
            impConviction.SentenceTypeCode = msgConviction.SentenceTypeCode
            impConviction.StatusCode = msgConviction.StatusCode
            impConviction.TypeCode = msgConviction.TypeCode

        End If

        Return impConviction

    End Function

    ''' <summary>
    ''' it will compare security token recieved from the STS application wtih token available in web.config file 
    ''' </summary>
    ''' <param name="sWCFSecurityToken"></param>
    ''' <remarks></remarks>
    Public Shared Sub CheckSTSSecurityToken(ByVal sWCFSecurityToken As String)
        Dim sSecurityToken As String
        Dim oCache As System.Web.Caching.Cache = HttpContext.Current.Cache()
        Try
            If oCache("WCFSecurityToken") Is Nothing And ConfigurationManager.AppSettings("WCFSecurityToken") IsNot Nothing Then
                sSecurityToken = ConfigurationManager.AppSettings("WCFSecurityToken").ToString
                HttpContext.Current.Cache.Insert("WCFSecurityToken", sSecurityToken)
            Else
                sSecurityToken = oCache("WCFSecurityToken")
            End If
            'lstWCFSecurityToken.Add(sSecurityToken)
            If sSecurityToken.Length > 0 Then
                If Not BCrypt.Net.BCrypt.Verify(sSecurityToken, sWCFSecurityToken) = True Then
                    Throw New AuthorisationException("CheckSecurityToken")
                End If
            End If
        Catch ex As Exception
            BusinessLayerLastResort(ex)
        Finally
        End Try
    End Sub
    ''' <summary>
    ''' it will compare security token recieved from the calling application wtih token available in web.config file 
    ''' </summary>
    ''' <param name="sWCFSecurityToken"></param>
    ''' <remarks></remarks>
    Public Shared Sub CheckSecurityToken(ByVal sWCFSecurityToken As String)
        Dim sSecurityToken As String
        Dim lstWCFSecurityToken As New List(Of String)
        Dim oCache As System.Web.Caching.Cache = HttpContext.Current.Cache()
        Try
            If oCache("WCFSecurityToken") Is Nothing And ConfigurationManager.AppSettings("WCFSecurityToken") IsNot Nothing Then
                sSecurityToken = ConfigurationManager.AppSettings("WCFSecurityToken").ToString
                HttpContext.Current.Cache.Insert("WCFSecurityToken", sSecurityToken)
            Else
                sSecurityToken = oCache("WCFSecurityToken")
            End If
            'lstWCFSecurityToken.Add(sSecurityToken)
            If sSecurityToken.Length > 0 Then
                If oCache("lstWCFSecurityToken") Is Nothing Then
                    If Not BCrypt.Net.BCrypt.Verify(sSecurityToken, sWCFSecurityToken) = True Then
                        Throw New AuthorisationException("CheckSecurityToken")
                    End If
                    lstWCFSecurityToken.Add(sWCFSecurityToken)
                    HttpContext.Current.Cache.Insert("lstWCFSecurityToken", lstWCFSecurityToken)
                Else
                    lstWCFSecurityToken = oCache("lstWCFSecurityToken")
                    Dim iCount As Integer
                    Dim bFlag As Boolean = False
                    For iCount = 0 To lstWCFSecurityToken.Count - 1
                        If lstWCFSecurityToken.Item(iCount).ToString().Equals(sWCFSecurityToken) Then
                            bFlag = True
                        End If
                    Next
                    If bFlag = False Then
                        If Not BCrypt.Net.BCrypt.Verify(sSecurityToken, sWCFSecurityToken) = True Then
                            Throw New AuthorisationException("CheckSecurityToken")
                        End If
                        If lstWCFSecurityToken.Count >= 20 Then
                            lstWCFSecurityToken.RemoveAt(19)
                        End If
                        lstWCFSecurityToken.Add(sWCFSecurityToken)
                        HttpContext.Current.Cache.Insert("lstWCFSecurityToken", lstWCFSecurityToken)
                    End If
                End If
            End If

        Catch ex As Exception
            BusinessLayerLastResort(ex)
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' Check whether the current user has the authority to perform a task.
    ''' </summary>
    ''' <param name="sTaskCode">The PMWrk_Task code to check for.</param>
    ''' <exception cref="AuthorisationException">The user does not have the authority.</exception>
    Public Shared Sub CheckAuthority(ByVal sTaskCode As String, ByVal iUserKey As Integer)


        Dim bExists As Boolean

        Dim oCache As System.Web.Caching.Cache = HttpContext.Current.Cache()
        Dim sCacheKey As String = String.Empty
        Dim oReturnValue As Object = Nothing
        sCacheKey = "CheckAuth" & iUserKey.ToString & sTaskCode.ToString

        ' Try to get the setting from the Cache
        oReturnValue = oCache(sCacheKey)


        Try
            If oReturnValue Is Nothing Then
                Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAN_PMWrk_Task_check")

                        cmd.AddInParameter("@user_id", SqlDbType.SmallInt).Value = iUserKey
                        cmd.AddInParameter("@task_code", SqlDbType.NChar, 10).Value = sTaskCode
                        cmd.AddOutParameter("@exists", SqlDbType.Bit)

                        Dim ds As DataSet = con.ExecuteDataSet(cmd, "PMWrk_Task_check")

                        If IsDBNull(cmd.Parameters("@exists").Value) Then
                            Throw New AuthorisationException(sTaskCode)
                        Else
                            bExists = Cast.ToBoolean(cmd.Parameters("@exists").Value, False)
                            oReturnValue = bExists

                            'Add the dataset into the cache
                            oCache.Insert(sCacheKey, oReturnValue)

                            If bExists = False Then
                                Throw New AuthorisationException("CheckAuthority")
                            End If
                        End If
                    End Using
                End Using
            Else
                bExists = CType(oReturnValue, Boolean)
                If bExists = False Then
                    Throw New AuthorisationException("CheckAuthority")
                End If
            End If


        Catch ex As Exception
            BusinessLayerLastResort(ex)
        Finally

        End Try
    End Sub
    Public Shared Function ValidateLicense(ByVal iccs As String) As Boolean
        Dim oBPMLicenseManager As New bPMLicenceManager.LicenseManager
        Dim errorMessage As String = ""
        Dim isValid As Boolean = True
        Try
            isValid = oBPMLicenseManager.IsThisLicenseValid(iccs, errorMessage)
        Catch
            isValid = False
        End Try
        Return isValid
    End Function
    Private Shared Function GetICCSNumber(ByVal con As SiriusConnection) As String
        Const kGetICCSSQL As String = "spu_pm_iccs"

        Dim objICCS As String = ""
        Using con
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure(kGetICCSSQL)
                cmd.AddInOutParameter("@ICCS", SqlDbType.VarChar, 4)
                con.ExecuteNonQuery(cmd)
                objICCS = Cast.ToString(cmd.Parameters("@ICCS").Value, "")
            End Using
        End Using
        Return objICCS
    End Function
    Public Shared Function ValidateLicence(ByRef errorMessage As String, ByVal con As SiriusConnection) As Boolean
        Dim iccs As String = GetICCSNumber(con)
        Return ValidateLicense(iccs)
    End Function
    ''' <summary>
    ''' Retrieve the authenticated user name and agent key (if applicable).
    ''' </summary>
    ''' <param name="o_sUserName">The authenticated user name returned.</param>
    ''' <param name="o_iAgentKey">The agent key returned, or zero if the user is not an agent.</param>
    Public Shared Sub GetIdentity(ByRef o_sUserName As String, ByRef o_iAgentKey As Integer, ByRef o_iUserId As Integer)
        Dim errorMessage As String = ""
        If ValidateLicence(errorMessage, SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)) = False Then
            Throw New Exception("Product is not correctly Licenced. Contact SSP Pure Support.")
        End If
        Dim UserID As Integer = 0
        Dim LanguageID As Integer = 0
        Dim PartyCnt As Integer = 0
        Dim SAMErrorCollection As New SAMErrorCollection
        Dim _oCache As System.Web.Caching.Cache = HttpContext.Current.Cache()
        Dim sCacheKey As String = String.Empty
        Dim oIdentity As Dictionary(Of String, Integer)
        Dim sPartyTypeCode As String = ""

        Try
            sCacheKey = "GetIdent" & o_sUserName
            ' Try to get the Full List from the Cache
            oIdentity = CType(_oCache(sCacheKey), Dictionary(Of String, Integer))
            If oIdentity Is Nothing Then

                Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_PMUser_check")

                        cmd.AddInParameter("@username", SqlDbType.NVarChar, 255).Value = Cast.NullIfDefault(o_sUserName)
                        cmd.AddOutParameter("@password", SqlDbType.NVarChar, 30)
                        cmd.AddOutParameter("@user_id", SqlDbType.SmallInt)
                        cmd.AddOutParameter("@language_id", SqlDbType.SmallInt)
                        cmd.AddOutParameter("@email_address", SqlDbType.NVarChar, 255)
                        cmd.AddOutParameter("@party_cnt", SqlDbType.Int)
                        cmd.AddOutParameter("@party_type_code", SqlDbType.NChar, 10)

                        Dim ds As DataSet = con.ExecuteDataSet(cmd, "UserDetails")

                        If IsDBNull(cmd.Parameters("@user_id").Value) Then
                            Throw New AuthorisationException(o_sUserName)
                        Else
                            o_iUserId = Cast.ToInt32(cmd.Parameters("@user_id").Value, 0)
                            o_iAgentKey = Cast.ToInt32(cmd.Parameters("@party_cnt").Value, 0)
                            LanguageID = Cast.ToInt32(cmd.Parameters("@language_id").Value, 0)
                            sPartyTypeCode = Cast.ToStringTrim(cmd.Parameters("@party_type_code").Value, "")
                            oIdentity = New Dictionary(Of String, Integer)
                            oIdentity.Add("user_id", o_iUserId)
                            oIdentity.Add("party_cnt", o_iAgentKey)
                            oIdentity.Add("language_id", LanguageID)
                            If sPartyTypeCode = PartyType.Agent Then
                                oIdentity.Add("party_type_code", 1)
                            Else
                                oIdentity.Add("party_type_code", 0)
                                o_iAgentKey = 0
                            End If
                            _oCache.Insert(sCacheKey, oIdentity)

                        End If
                    End Using
                End Using
            Else
                ' See if this key exists.
                If oIdentity.ContainsKey("user_id") Then
                    ' Write value of the key.
                    o_iUserId = CInt(oIdentity.Item("user_id"))
                End If
                If oIdentity.ContainsKey("party_cnt") Then
                    ' Write value of the key.
                    o_iAgentKey = CInt(oIdentity.Item("party_cnt"))
                End If
                If oIdentity.ContainsKey("party_type_code") Then
                    ' Write value of the key.
                    If CInt(oIdentity.Item("party_type_code")) = 0 Then
                        o_iAgentKey = 0
                    End If
                End If

            End If

        Catch ex As Exception
            BusinessLayerLastResort(ex)
        End Try

    End Sub

    ''' <summary>
    ''' To convert from WCF service BasePartyOTHERTypeAccident to internal BasePartyOTHERTypeAccident
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBasePartyOTHERTypeAccident(ByVal oService As BasePartyOTHERTypeAccident) As BaseImplementationTypes.BasePartyOTHERTypeAccident

        Dim oImplementation As New BaseImplementationTypes.BasePartyOTHERTypeAccident

        If oService IsNot Nothing Then

            oImplementation.AccidentKey = oService.AccidentKey
            oImplementation.Date = oService.Date
            oImplementation.Description = oService.Description
            oImplementation.IsAtFault = oService.IsAtFault

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service BasePartyOTHERTypeSupplierBusiness to internal 
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBasePartyOTHERTypeSupplierBusiness(
        ByVal oService As BasePartyOTHERTypeSupplierBusiness) As BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness

        Dim oImplementation As New BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness

        If oService IsNot Nothing Then

            oImplementation.BusinessCode = oService.BusinessCode
            oImplementation.SpecialityCode = oService.SpecialityCode

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service BasePartyOTHERTypeBranch to internal 
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBasePartyOTHERTypeBranch(ByVal oService As BasePartyOTHERTypeBranch) As BaseImplementationTypes.BasePartyOTHERTypeBranch

        Dim oImplementation As New BaseImplementationTypes.BasePartyOTHERTypeBranch

        If oService IsNot Nothing Then

            oImplementation.BranchId = oService.BranchId
            oImplementation.Description = oService.Description

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service to internal BaseAddressWithContactsType
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseAddressWithContactsType(
            ByVal oService As BaseAddressWithContactsType) As BaseImplementationTypes.BaseAddressWithContactsType

        Dim oImplementation As New BaseImplementationTypes.BaseAddressWithContactsType

        If oService IsNot Nothing Then

            oImplementation.AddressLine1 = oService.AddressLine1
            oImplementation.AddressLine2 = oService.AddressLine2
            oImplementation.AddressLine3 = oService.AddressLine3
            oImplementation.AddressLine4 = oService.AddressLine4
            oImplementation.AddressLine5 = oService.AddressLine5
            oImplementation.AddressLine6 = oService.AddressLine6
            oImplementation.AddressLine7 = oService.AddressLine7
            oImplementation.AddressLine8 = oService.AddressLine8
            oImplementation.AddressLine9 = oService.AddressLine9
            oImplementation.AddressLine10 = oService.AddressLine10



            oImplementation.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), oService.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
            oImplementation.CountryCode = oService.CountryCode
            oImplementation.PostCode = oService.PostCode

            If oService.Contacts IsNot Nothing Then
                oImplementation.Contacts = Array.ConvertAll(oService.Contacts.ToArray(),
                            New Converter(Of BaseContactType, BaseImplementationTypes.BaseContactType) _
                            (AddressOf ToBaseImpBaseContactType))
            End If

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service BaseContactType to inernal BaseContactType
    ''' </summary>
    ''' <param name="msgContact"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseContactType(ByVal msgContact As BaseContactType) As BaseImplementationTypes.BaseContactType

        If msgContact IsNot Nothing Then
            Dim impContact As BaseImplementationTypes.BaseContactType = New BaseImplementationTypes.BaseContactType
            impContact.AreaCode = Trim(msgContact.AreaCode)
            impContact.Description = Trim(msgContact.Description)
            impContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
            impContact.ContactDetail.Item = Trim(msgContact.ContactDetail.Item)
            If msgContact.ContactTypeCode <> BaseImplementationTypes.ContactTypeType.OTHER Then
                impContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), msgContact.ContactTypeCode), BaseImplementationTypes.ContactTypeType)
            Else
                impContact.ContactTypeCode = BaseImplementationTypes.ContactTypeType.OTHER
                impContact.OtherContactTypeCode = Trim(msgContact.OtherContactTypeCode)
            End If

            impContact.Extension = Trim(msgContact.Extension)
            Return impContact
        End If
    End Function

    ''' <summary>
    ''' To convert from internal SAMError to WCF service SAMError
    ''' </summary>
    ''' <param name="oImplementation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function ToServiceSAMErrorType(ByVal oImplementation As BaseImplementationTypes.SAMError) As SFI.SAMForInsuranceV2.WCF.SAMError

        Select Case oImplementation.GetType()

            Case GetType(BaseImplementationTypes.SAMErrorBusinessRule)

                Dim oActualImplementation As BaseImplementationTypes.SAMErrorBusinessRule = CType(oImplementation, BaseImplementationTypes.SAMErrorBusinessRule)
                Dim oService As New SFI.SAMForInsuranceV2.WCF.SAMErrorBusinessRule
                oService.Code = oActualImplementation.Code
                oService.Description = oActualImplementation.Description
                oService.Detail = oActualImplementation.Detail
                Return oService

            Case GetType(BaseImplementationTypes.SAMErrorInvalidData)

                Dim oActualImplementation As BaseImplementationTypes.SAMErrorInvalidData = CType(oImplementation, BaseImplementationTypes.SAMErrorInvalidData)
                Dim oService As New SFI.SAMForInsuranceV2.WCF.SAMErrorInvalidData
                oService.Code = oActualImplementation.Code
                oService.Description = oActualImplementation.Description
                oService.FieldName = oActualImplementation.FieldName
                oService.Reason = oActualImplementation.Reason
                oService.SuppliedValue = oActualImplementation.SuppliedValue
                Return oService

            Case Else

                Dim oActualImplementation As BaseImplementationTypes.SAMErrorFatal = CType(oImplementation, BaseImplementationTypes.SAMErrorFatal)
                Dim oService As New SFI.SAMForInsuranceV2.WCF.SAMErrorFatal
                oService.Type = oActualImplementation.Type
                Return oService

        End Select

    End Function

    ''' <summary>
    ''' To convert from internal SAMError to WCF service SAMError
    ''' </summary>
    ''' <param name="SAMErrorEx"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function ToServiceSAMErrorType(ByVal SAMErrorEx As Sirius.Architecture.ExceptionHandling.SAMError) As SFI.SAMForInsuranceV2.WCF.SAMError

        Select Case SAMErrorEx.GetType()

            Case GetType(Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule)

                Dim oActualImplementation As Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule = CType(SAMErrorEx, Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule)
                Dim oService As New SFI.SAMForInsuranceV2.WCF.SAMErrorBusinessRule
                oService.Code = oActualImplementation.Code
                oService.Description = oActualImplementation.Description
                oService.Detail = oActualImplementation.Detail
                Return oService

            Case GetType(Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData)

                Dim oActualImplementation As Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData = CType(SAMErrorEx, Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData)
                Dim oService As New SFI.SAMForInsuranceV2.WCF.SAMErrorInvalidData
                oService.Code = oActualImplementation.Code
                oService.Description = oActualImplementation.Description
                oService.FieldName = oActualImplementation.FieldName
                oService.SuppliedValue = oActualImplementation.SuppliedValue
                Return oService

            Case Else

                Dim oActualImplementation As Sirius.Architecture.ExceptionHandling.SAMErrorFatal = CType(SAMErrorEx, Sirius.Architecture.ExceptionHandling.SAMErrorFatal)
                Dim oService As New SFI.SAMForInsuranceV2.WCF.SAMErrorFatal
                oService.Type = oActualImplementation.Type
                Return oService

        End Select

    End Function

    ''' <summary>
    ''' To return logged in user name from security token
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserName() As String
        'definition ned to be written after security implementation
        Return "sirius"

    End Function

    ''' <summary>
    ''' To throw fault contract to WCF service client
    ''' </summary>
    ''' <param name="ex"></param>
    ''' <remarks></remarks>
    Public Shared Sub BusinessLayerLastResort(ByVal ex As Exception)
        BusinessLayerLastResort(ex, Nothing)
    End Sub

    Public Shared Sub BusinessLayerLastResort(ByVal ex As Exception, ByRef oDicParams As Dictionary(Of String, Object))

        'Need to uncomment below line after getting a compatible function in error handler dll
        'Handler.BusinessLayerLastResort(ex, HttpContext.Current)
        Dim oException As New SFI.SAMForInsuranceV2.WCF.SAMMethodResponseData
        Dim oErrors As New List(Of SFI.SAMForInsuranceV2.WCF.SAMError)
        Dim oError As New SFI.SAMForInsuranceV2.WCF.WCFUnhandledError
        Dim handlingInstanceID As Guid
        Dim sErrUniqueID As String = ""
        sErrUniqueID = gPMFunctions.GenerateUniqueSSPExceptionRef(gPMConstants.ERROR_NO_LENGTH)
        handlingInstanceID = Guid.NewGuid()
        oError.Reason = SAMErrorCode.GeneralFailure
        oError.Description = ex.Message.ToString() & Environment.NewLine & sErrUniqueID
        oError.Detail = ex.StackTrace.ToString()
        oErrors.Add(oError)
        oException.HandlingInstanceID = handlingInstanceID
        oException.Errors = oErrors
        '' below code will log the error in event Log 
        'Dim logEntry As New LogEntry()
        'logEntry.Categories.Clear()
        'logEntry.Categories.Add(Category.General)
        'logEntry.Priority = Priority.Normal
        'logEntry.Severity = TraceEventType.Verbose
        'logEntry.Message = ex.Message.ToString().ToString
        'Logger.Write(logEntry)

        If Not gPMFunctions.EventLogWrite(Priority.Normal, "SAM Error", oDicParams, sErrUniqueID, ex) = gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception
        End If
        ex.HelpLink = sErrUniqueID
        Dim oExceptionTrace = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex)
        oExceptionTrace.Throw()
        'Throw New FaultException(Of SFI.SAMForInsuranceV2.WCF.SAMMethodResponseData)(oException, ex.Message.ToString())
    End Sub

    Public Shared Sub BusinessLayerBoundary(ByVal oImpResponse As Object, ByRef oResponse As Object, ByVal ex As Exception)
        BusinessLayerBoundary(oImpResponse, oResponse, ex, Nothing)
    End Sub

    ''' <summary>
    ''' To set Errors collection in WCF service response
    ''' </summary>
    ''' <param name="oImpResponse"></param>
    ''' <param name="oResponse"></param>
    ''' <param name="ex"></param>
    ''' <remarks></remarks>
    Public Shared Sub BusinessLayerBoundary(ByVal oImpResponse As Object, ByRef oResponse As Object, ByVal ex As Exception, ByRef oDicParams As Dictionary(Of String, Object))
        Try
            Dim bInvalidDataError As Boolean = False
            Dim bLogBusinessRuleErrors As Boolean = False
            If Boolean.TryParse(WebConfigurationManager.AppSettings("LogBusinessRuleErrors").ToString(), bLogBusinessRuleErrors) = True Then
                bLogBusinessRuleErrors = Boolean.Parse(WebConfigurationManager.AppSettings("LogBusinessRuleErrors"))
            End If
            Dim sErrUniqueID As String = ""
            sErrUniqueID = gPMFunctions.GenerateUniqueSSPExceptionRef(gPMConstants.ERROR_NO_LENGTH)
            ex.HelpLink = sErrUniqueID
            Dim oExceptionTrace = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex)
            If oImpResponse IsNot Nothing Then
                Handler.BusinessLayerBoundary(ex, oImpResponse)

                oResponse.Errors = CType(oImpResponse.Errors, BaseImplementationTypes.SAMError()).ToList().ConvertAll(
                                New Converter(Of BaseImplementationTypes.SAMError, SFI.SAMForInsuranceV2.WCF.SAMError) _
                                (AddressOf ToServiceSAMErrorType))

                Dim samEx As SAMErrorException = TryCast(ex, SAMErrorException)
                If samEx IsNot Nothing Then
                    Dim sExceptionMessage As String = ""
                    For idx As Integer = 0 To samEx.Errors.Count - 1
                        Dim oException As Object = Nothing
                        oException = TryCast(samEx.Errors(idx), Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule)
                        If oException Is Nothing Then
                            oException = TryCast(samEx.Errors(idx), Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData)
                            sExceptionMessage += "Additional Description:Invalid Data" + Environment.NewLine
                        End If
                        If oException Is Nothing Then
                            oException = TryCast(samEx.Errors(idx), Sirius.Architecture.ExceptionHandling.SAMErrorFatal)
                            sExceptionMessage += "Additional Description:Fatal Error" + Environment.NewLine
                        End If

                        If Not oException Is Nothing Then
                            sExceptionMessage += oException.ToString() + Environment.NewLine
                        Else
                            sExceptionMessage += ex.Message + Environment.NewLine
                        End If
                    Next
                    If samEx.Errors.Count > 0 Then
                        If TryCast(samEx.Errors(0), Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData) IsNot Nothing Then
                            bInvalidDataError = True
                        End If
                    End If
                    If bInvalidDataError = True Then
                        If bLogBusinessRuleErrors = True Then
                            If Not gPMFunctions.EventLogWrite(Priority.High, sExceptionMessage, oDicParams, sErrUniqueID, ex) = gPMConstants.PMEReturnCode.PMTrue Then
                                oExceptionTrace.Throw()
                            End If
                        End If
                    Else
                        If Not gPMFunctions.EventLogWrite(Priority.High, sExceptionMessage, oDicParams, sErrUniqueID, ex) = gPMConstants.PMEReturnCode.PMTrue Then
                            oExceptionTrace.Throw()
                        End If
                    End If
                    Try
                        If oResponse.Errors.Count > 0 Then
                            If TypeOf (oResponse.Errors(0)) Is SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF.SAMErrorBusinessRule Then
                                oResponse.Errors(0).Detail = sErrUniqueID.Substring(0, ERROR_LABEL.Length + ERROR_NO_LENGTH) & " " & oResponse.Errors(0).Detail
                            End If
                        End If
                    Catch
                    End Try
                Else
                    Throw New Exception(ex.Message & vbCrLf & ex.StackTrace)
                End If
            ElseIf oImpResponse Is Nothing Then

                Dim samEx As SAMErrorException = TryCast(ex, SAMErrorException)

                If samEx IsNot Nothing Then

                    ' Populate the SAMMethodResponseData from the SAMMethodException just caught.
                    oResponse.Errors = samEx.Errors.ConvertAll(
                                New Converter(Of Sirius.Architecture.ExceptionHandling.SAMError, SFI.SAMForInsuranceV2.WCF.SAMError) _
                                (AddressOf ToServiceSAMErrorType))
                    'Dim cd As String = samEx.Errors(0).ToString()
                    Dim sExceptionMessage As String = ""
                    For idx As Integer = 0 To samEx.Errors.Count - 1
                        Dim oException As Object = Nothing
                        oException = TryCast(samEx.Errors(idx), Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule)
                        If oException Is Nothing Then
                            oException = TryCast(samEx.Errors(idx), Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData)
                            sExceptionMessage += "Additional Description:Invalid Data" + Environment.NewLine
                        End If
                        If oException Is Nothing Then
                            oException = TryCast(samEx.Errors(idx), Sirius.Architecture.ExceptionHandling.SAMErrorFatal)
                            sExceptionMessage += "Additional Description:Fatal Error" + Environment.NewLine
                        End If

                        If Not oException Is Nothing Then
                            sExceptionMessage += oException.ToString() + Environment.NewLine
                        Else
                            sExceptionMessage += ex.Message + Environment.NewLine
                        End If
                    Next
                    If samEx.Errors.Count > 0 Then
                        If TryCast(samEx.Errors(0), Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData) IsNot Nothing Then
                            bInvalidDataError = True
                        End If
                    End If
                    If bInvalidDataError = True Then
                        If bLogBusinessRuleErrors = True Then
                            If Not gPMFunctions.EventLogWrite(Priority.High, sExceptionMessage, oDicParams, sErrUniqueID, ex) = gPMConstants.PMEReturnCode.PMTrue Then
                                oExceptionTrace.Throw()
                            End If
                        End If
                    Else
                        If Not gPMFunctions.EventLogWrite(Priority.High, sExceptionMessage, oDicParams, sErrUniqueID, ex) = gPMConstants.PMEReturnCode.PMTrue Then
                            oExceptionTrace.Throw()
                        End If
                    End If
                    Try
                        If oResponse.Errors.Count > 0 Then
                            If TypeOf (oResponse.Errors(0)) Is SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF.SAMErrorBusinessRule Then
                                oResponse.Errors(0).Detail = sErrUniqueID.Substring(0, ERROR_LABEL.Length + ERROR_NO_LENGTH) & " " & oResponse.Errors(0).Detail
                            End If
                        End If
                    Catch
                    End Try
                Else
                    oExceptionTrace.Throw()
                    'Throw New Exception(ex.Message & vbCrLf & ex.StackTrace)
                End If
            Else
                oExceptionTrace.Throw()
                'Throw New Exception(ex.Message & vbCrLf & ex.StackTrace)
            End If
        Catch
            Throw
        End Try
    End Sub


    Public Shared Function ToServiceTaxesAndFeesTypeList(ByVal taxesAndFees As BaseImplementationTypes.BaseTaxesAndFeesType) As BaseTaxesAndFeesType

        Dim serviceTaxesAndFees As New BaseTaxesAndFeesType
        If taxesAndFees IsNot Nothing Then
            Dim oActualImplementation As BaseImplementationTypes.BaseTaxesAndFeesType
            oActualImplementation = TryCast(taxesAndFees, BaseImplementationTypes.BaseTaxesAndFeesType)

            If taxesAndFees.Fees IsNot Nothing Then

                serviceTaxesAndFees.Fees = oActualImplementation.Fees.ToList().ConvertAll(
                                     New Converter(Of BaseImplementationTypes.BaseFeesType, BaseFeesType) _
                                                              (AddressOf CommonFunctions.ToServiceFeesType))
            End If

            If taxesAndFees.Taxes IsNot Nothing Then
                serviceTaxesAndFees.Taxes = oActualImplementation.Taxes.ToList().ConvertAll(
                                   New Converter(Of BaseImplementationTypes.BaseTaxesType, BaseTaxesType) _
                                                            (AddressOf CommonFunctions.ToServiceTaxesType))
            End If

        End If

        Return serviceTaxesAndFees

    End Function

    Public Shared Function ToServiceFeesType(ByVal fees As BaseImplementationTypes.BaseFeesType) As BaseFeesType

        Dim serviceFees As BaseFeesType = New BaseFeesType

        If fees IsNot Nothing Then
            serviceFees.Amount = fees.Amount
            serviceFees.Description = fees.Description
        End If

        Return serviceFees

    End Function

    Public Shared Function ToServiceTaxesType(ByVal taxes As BaseImplementationTypes.BaseTaxesType) As BaseTaxesType

        Dim serviceTaxes As BaseTaxesType = New BaseTaxesType

        If taxes IsNot Nothing Then
            serviceTaxes.Amount = taxes.Amount
            serviceTaxes.Description = taxes.Description
        End If

        Return serviceTaxes
    End Function



    Public Shared Function ToBaseImpBaseBankReceiptType(
             ByVal oService As BaseBankReceiptType) As BaseImplementationTypes.BaseBankReceiptType

        Dim oImplementation As New BaseImplementationTypes.BaseBankReceiptType

        If oService IsNot Nothing Then

            oImplementation.BankCode = oService.BankCode
            oImplementation.ChequeDate = oService.ChequeDate
            oImplementation.PayerName = oService.PayerName
            oImplementation.BankBranch = oService.BankBranch
            oImplementation.BankLocation = oService.BankLocation
            oImplementation.ChequeClearingTypeCode = oService.ChequeClearingTypeCode
            oImplementation.ChequeDate = oService.ChequeDate
            oImplementation.ChequeTypeCode = oService.ChequeTypeCode

        End If

        Return oImplementation

    End Function


    ''' <summary>
    ''' To convert from internal BankGaurantee to WCF service BankGaurantee
    ''' </summary>
    ''' <param name="oBankGaurantee"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServicUpdateBankGuaranteeList(ByVal oBankGaurantee As SFI.SAMForInsuranceV2.BaseUpdateBankGuaranteeResponseTypeBankGuaranteeRow) As BaseUpdateBankGuaranteeResponseTypeRow

        Dim oServiceBankGaurantee As BaseUpdateBankGuaranteeResponseTypeRow = New BaseUpdateBankGuaranteeResponseTypeRow

        If oBankGaurantee IsNot Nothing Then
            oServiceBankGaurantee.BGKey = oBankGaurantee.BGKey
            oServiceBankGaurantee.BGTimeStamp = oBankGaurantee.BGTimeStamp
        End If

        Return oServiceBankGaurantee

    End Function
    Public Shared Function ToServiceMultiplePoliciesList(ByVal oMultiplePolicies As BaseImplementationTypes.BaseTransactResponseTypePolicyMultiplePolicies) As BaseTransactResponseTypePolicyMultiplePolicies

        Dim serviceMultiplePolicies As New BaseTransactResponseTypePolicyMultiplePolicies
        If serviceMultiplePolicies IsNot Nothing Then
            serviceMultiplePolicies.PremiumDueGross = oMultiplePolicies.PremiumDueGross
            serviceMultiplePolicies.PolicyRef = oMultiplePolicies.PolicyRef
            serviceMultiplePolicies.PremiumDueNet = oMultiplePolicies.PremiumDueNet
            serviceMultiplePolicies.PremiumDueTax = oMultiplePolicies.PremiumDueTax
            serviceMultiplePolicies.TotalAnnualTax = oMultiplePolicies.TotalAnnualTax
            serviceMultiplePolicies.CommissionAmount = oMultiplePolicies.CommissionAmount
            serviceMultiplePolicies.DocumentComment = oMultiplePolicies.DocumentComment
            serviceMultiplePolicies.AutoGeneratedPlanRef = oMultiplePolicies.AutoGeneratedPlanRef

        End If
        Return serviceMultiplePolicies

    End Function


    ''' <summary>
    ''' To convert from WCF Service oProductService to Internal oProductBase
    ''' </summary>
    ''' <param name="oProductService"></param>
    ''' <param name="oProductBase"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToBaseImpBaseGetQuotesMarkedForCollection(ByRef oProductService As List(Of BaseGetQuotesMarkedForCollectionRequestTypeProducts),
                                                    ByRef oProductBase As List(Of BaseImplementationTypes.BaseGetQuotesMarkedForCollectionRequestTypeProducts))

        If (oProductService) IsNot Nothing Then

            Dim msgProduct As BaseGetQuotesMarkedForCollectionRequestTypeProducts
            Dim impProduct As BaseImplementationTypes.BaseGetQuotesMarkedForCollectionRequestTypeProducts

            For iCnt As Integer = 0 To oProductService.Count - 1

                msgProduct = oProductService(iCnt)
                impProduct = New BaseImplementationTypes.BaseGetQuotesMarkedForCollectionRequestTypeProducts
                impProduct.ProductCode = msgProduct.ProductCode

                oProductBase.Add(impProduct)

            Next iCnt

        End If

    End Sub


    Public Shared Function ToServiceGetDefaultRiskClausesList(ByVal oGetDefaultRiskClauses As BaseImplementationTypes.BaseGetDefaultRiskClausesResponseTypeDocumentsRow) As BaseGetDefaultRiskClausesResponseTypeRow

        Dim serviceGetDefaultRiskClauses As New BaseGetDefaultRiskClausesResponseTypeRow
        If oGetDefaultRiskClauses IsNot Nothing Then
            With oGetDefaultRiskClauses
                serviceGetDefaultRiskClauses.Code = .Code
                serviceGetDefaultRiskClauses.Description = .Description
            End With
        End If
        Return serviceGetDefaultRiskClauses

    End Function



    Public Shared Function ToBaseImpBaseBindQuoteCreditTransactions(ByRef oCreditTransactionsService As List(Of BaseBindQuoteRequestTypeCreditTransactionsRow)) As BaseImplementationTypes.BaseBindQuoteRequestTypeCreditTransactions

        If (oCreditTransactionsService) IsNot Nothing Then
            Dim oCreditTransactionsBase As New BaseImplementationTypes.BaseBindQuoteRequestTypeCreditTransactions

            ReDim oCreditTransactionsBase.Row(oCreditTransactionsService.Count - 1)
            For iCnt As Integer = 0 To oCreditTransactionsService.Count - 1
                oCreditTransactionsBase.Row(iCnt) = New BaseImplementationTypes.BaseBindQuoteRequestTypeCreditTransactionsRow
                oCreditTransactionsBase.Row(iCnt).AccountKey = oCreditTransactionsService(iCnt).AccountKey
                oCreditTransactionsBase.Row(iCnt).Amount = oCreditTransactionsService(iCnt).Amount
                oCreditTransactionsBase.Row(iCnt).CollectionDate = oCreditTransactionsService(iCnt).CollectionDate
                oCreditTransactionsBase.Row(iCnt).TransDetailKey = oCreditTransactionsService(iCnt).TransDetailKey
            Next iCnt
            Return oCreditTransactionsBase
        End If
        Return Nothing
    End Function

    Public Shared Function ToServiceWarningTypeList(ByVal oServiceWarningType As BaseImplementationTypes.BaseGeneralWarningResponseType) As BaseGeneralWarningResponseType

        Dim serviceServiceWarningType As New BaseGeneralWarningResponseType
        If oServiceWarningType IsNot Nothing Then
            With oServiceWarningType
                serviceServiceWarningType.Code = .Code
                serviceServiceWarningType.Description = .Description
            End With
        End If
        Return serviceServiceWarningType

    End Function

    Public Shared Function ToBaseImpBaseUpdateTaxes(ByRef oUpdateTaxesService As List(Of BaseUpdateTaxesRequestTypeRow)) As BaseImplementationTypes.BaseUpdateTaxesRequestTypeRow()

        If (oUpdateTaxesService) IsNot Nothing Then
            Dim oBaseTaxRow(oUpdateTaxesService.Count - 1) As BaseImplementationTypes.BaseUpdateTaxesRequestTypeRow
            For iCnt As Integer = 0 To oUpdateTaxesService.Count - 1
                oBaseTaxRow(iCnt) = New BaseImplementationTypes.BaseUpdateTaxesRequestTypeRow
                oBaseTaxRow(iCnt).IsEdited = oUpdateTaxesService(iCnt).IsEdited
                oBaseTaxRow(iCnt).IsValue = oUpdateTaxesService(iCnt).IsValue
                oBaseTaxRow(iCnt).TaxCalculationKey = oUpdateTaxesService(iCnt).TaxCalculationKey
                oBaseTaxRow(iCnt).TaxPercentage = oUpdateTaxesService(iCnt).TaxPercentage
                oBaseTaxRow(iCnt).TaxValue = oUpdateTaxesService(iCnt).TaxValue
            Next iCnt
            Return oBaseTaxRow
        End If
        Return Nothing

    End Function

    Public Shared Function ToServiceGetDocumentDefaultsList(ByVal oGetDocumentDefaultsList As BaseImplementationTypes.BaseGetDocumentDefaultsResponseTypeDocumentTemplates) As BaseGetDocumentDefaultsResponseTypeDocumentTemplates
        Dim oServiceGetDocumentDefaultList As New BaseGetDocumentDefaultsResponseTypeDocumentTemplates
        If (oGetDocumentDefaultsList) IsNot Nothing Then
            With oGetDocumentDefaultsList
                oServiceGetDocumentDefaultList.DocumentGroupCode = .DocumentGroupCode
                oServiceGetDocumentDefaultList.DocumentGroupDescription = .DocumentGroupDescription
                oServiceGetDocumentDefaultList.DocumentGroupID = .DocumentGroupID
                oServiceGetDocumentDefaultList.DocumentSubGroupCode = .DocumentSubGroupCode
                oServiceGetDocumentDefaultList.DocumentSubGroupDescription = .DocumentSubGroupDescription
                oServiceGetDocumentDefaultList.DocumentSubGroupID = .DocumentSubGroupID
                oServiceGetDocumentDefaultList.DocumentTemplateCode = .DocumentTemplateCode
                oServiceGetDocumentDefaultList.DocumentTemplateDescription = .DocumentTemplateDescription
                oServiceGetDocumentDefaultList.DocumentTemplateID = .DocumentTemplateID
                oServiceGetDocumentDefaultList.InternalOnly = .InternalOnly
                oServiceGetDocumentDefaultList.SelectedByDefault = .SelectedByDefault
                oServiceGetDocumentDefaultList.EmailDocumentSubjectCode = .EmailDocumentSubjectCode
                oServiceGetDocumentDefaultList.EmailDocumentAttachmentCode = .EmailDocumentAttachmentCode
            End With
        End If

        Return oServiceGetDocumentDefaultList

    End Function



    Public Shared Function ToServiceGenerateDocumentsForEvent(ByVal oGenerateDocumentsForEvent As BaseImplementationTypes.BaseGenerateDocumentsForEventResponseDocument) As BaseGenerateDocumentsForEventResponseDocument
        Dim oServiceGenerateDocumentsForEvent As New BaseGenerateDocumentsForEventResponseDocument
        If (oGenerateDocumentsForEvent) IsNot Nothing Then
            With oGenerateDocumentsForEvent



                oServiceGenerateDocumentsForEvent.DocumentTemplateCode = oGenerateDocumentsForEvent.DocumentTemplateCode
                oServiceGenerateDocumentsForEvent.DocumentTemplateDescription = oGenerateDocumentsForEvent.DocumentTemplateDescription

            End With

        End If

        Return oServiceGenerateDocumentsForEvent

    End Function

    Public Shared Function ToBaseImpBaseUpdateAgents(ByRef oUpdateAgentsService As List(Of BaseUpdateSubAgentsRequestTypeSubAgentsRow)) As BaseImplementationTypes.BaseUpdateSubAgentsRequestTypeSubAgents

        If (oUpdateAgentsService) IsNot Nothing Then
            Dim oUpdateAgents As New BaseImplementationTypes.BaseUpdateSubAgentsRequestTypeSubAgents

            ReDim oUpdateAgents.Row(oUpdateAgentsService.Count - 1)
            For iCnt As Integer = 0 To oUpdateAgentsService.Count - 1
                oUpdateAgents.Row(iCnt) = New BaseImplementationTypes.BaseUpdateSubAgentsRequestTypeSubAgentsRow
                oUpdateAgents.Row(iCnt).Amount = oUpdateAgentsService(iCnt).Amount
                oUpdateAgents.Row(iCnt).PartyKey = oUpdateAgentsService(iCnt).PartyKey
                oUpdateAgents.Row(iCnt).Percentage = oUpdateAgentsService(iCnt).Percentage
            Next iCnt
            Return oUpdateAgents
        End If
        Return Nothing
    End Function

    Public Shared Function ToServiceUpdateCashDepositList(ByVal oServiceeUpdateCashDepositType As BaseImplementationTypes.BaseUpdateCashDepositResponseTypeCashDepositRow) As BaseUpdateCashDepositResponseTypeRow

        Dim serviceeUpdateCashDeposit As New BaseUpdateCashDepositResponseTypeRow
        If oServiceeUpdateCashDepositType IsNot Nothing Then
            With oServiceeUpdateCashDepositType
                serviceeUpdateCashDeposit.CashDepositKey = .CashDepositKey
                serviceeUpdateCashDeposit.CashDepositRef = .CashDepositRef
                serviceeUpdateCashDeposit.CDTimeStamp = .CDTimeStamp
            End With
        End If
        Return serviceeUpdateCashDeposit
    End Function

    Public Shared Function ToServiceBaseClientDataImportResponseTypeAccountsDocuments(ByVal oImpAccountsDocuments As BaseImplementationTypes.BaseClientDataImportResponseTypeAccountsDocuments) As BaseClientDataImportResponseTypeAccountsDocuments
        Dim msgAccountsDocuments As BaseClientDataImportResponseTypeAccountsDocuments = New BaseClientDataImportResponseTypeAccountsDocuments

        If oImpAccountsDocuments IsNot Nothing Then
            msgAccountsDocuments.DocumentRef = oImpAccountsDocuments.DocumentRef
        End If
        Return msgAccountsDocuments

    End Function

    Public Shared Function ToServiceBaseClientDataImportResponseTypePolicyVersion(ByVal oImpPolicyVersion As BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersion) As BaseClientDataImportResponseTypePolicyVersion

        Dim msgPolicyVersion As BaseClientDataImportResponseTypePolicyVersion = New BaseClientDataImportResponseTypePolicyVersion

        If oImpPolicyVersion IsNot Nothing Then

            msgPolicyVersion.InsuranceFileKey = oImpPolicyVersion.InsuranceFileKey
            msgPolicyVersion.InsuranceFolderKey = oImpPolicyVersion.InsuranceFolderKey
            msgPolicyVersion.SAMStagingPolicyKey = oImpPolicyVersion.SAMStagingPolicyKey

            ' Process the Risks structure
            If IsArray(oImpPolicyVersion.Risks) = True Then
                msgPolicyVersion.Risks = oImpPolicyVersion.Risks.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersionRisks, BaseClientDataImportResponseTypePolicyVersionRisks)(AddressOf CommonFunctions.ToServiceBaseClientDataImportResponseTypePolicyVersionRisks))
            End If

            ' Process the Risks structure
            If oImpPolicyVersion.Claim IsNot Nothing Then
                msgPolicyVersion.Claim = oImpPolicyVersion.Claim.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersionClaim, BaseClientDataImportResponseTypePolicyVersionClaim)(AddressOf CommonFunctions.ToServiceBaseClientDataImportResponseTypePolicyVersionClaim))
            End If

        End If

        Return msgPolicyVersion

    End Function

    Public Shared Function ToServiceBaseClientDataImportResponseTypePolicyVersionRisks(ByVal oImpRisk As BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersionRisks) As BaseClientDataImportResponseTypePolicyVersionRisks

        Dim msgRisk As BaseClientDataImportResponseTypePolicyVersionRisks = New BaseClientDataImportResponseTypePolicyVersionRisks

        If oImpRisk IsNot Nothing Then

            msgRisk.RiskFolderKey = oImpRisk.RiskFolderKey
            msgRisk.RiskKey = oImpRisk.RiskKey
            msgRisk.SAMStagingRiskKey = oImpRisk.SAMStagingRiskKey

        End If

        Return msgRisk

    End Function

    Public Shared Function ToServiceBaseClientDataImportResponseTypePolicyVersionClaim(
    ByVal ConvertFrom As BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersionClaim) _
        As BaseClientDataImportResponseTypePolicyVersionClaim

        Dim ConvertTo As BaseClientDataImportResponseTypePolicyVersionClaim = Nothing

        If ConvertFrom IsNot Nothing Then

            ConvertTo = New BaseClientDataImportResponseTypePolicyVersionClaim

            ConvertTo.ClaimKey = ConvertFrom.ClaimKey
            ConvertTo.SAMStagingClaimKey = ConvertFrom.SAMStagingClaimKey

        End If

        Return ConvertTo

    End Function

    Public Shared Function ToBaseImpBaseAddressType(ByVal msgAddress As BaseAddressWithContactsType) As BaseImplementationTypes.BaseAddressWithContactsType

        Dim impAddress As BaseImplementationTypes.BaseAddressWithContactsType = New BaseImplementationTypes.BaseAddressWithContactsType

        If msgAddress IsNot Nothing Then

            impAddress.AddressLine1 = msgAddress.AddressLine1
            impAddress.AddressLine2 = msgAddress.AddressLine2
            impAddress.AddressLine3 = msgAddress.AddressLine3
            impAddress.AddressLine4 = msgAddress.AddressLine4

            impAddress.AddressLine5 = msgAddress.AddressLine5
            impAddress.AddressLine6 = msgAddress.AddressLine6
            impAddress.AddressLine7 = msgAddress.AddressLine7
            impAddress.AddressLine8 = msgAddress.AddressLine8
            impAddress.AddressLine9 = msgAddress.AddressLine9
            impAddress.AddressLine10 = msgAddress.AddressLine10

            impAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), msgAddress.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
            impAddress.CountryCode = msgAddress.CountryCode
            impAddress.PostCode = msgAddress.PostCode

            If msgAddress.Contacts IsNot Nothing Then

                impAddress.Contacts = Array.ConvertAll(msgAddress.Contacts.ToArray, New Converter(Of BaseContactType, BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf CommonFunctions.ToBaseImpBaseContactType))
            End If

        End If

        Return impAddress

    End Function

    Public Shared Function ToServiceCreatePaymentCashListWithItemsList(ByVal oCreatePaymentCashListWithItems As BaseImplementationTypes.BaseCreatePaymentCashListWithItemsResponseTypeCashListItem) As BaseCreatePaymentCashListWithItemsResponseTypeCashListItem

        Dim ServiceCreatePaymentCashListWithItems As BaseCreatePaymentCashListWithItemsResponseTypeCashListItem = Nothing
        If oCreatePaymentCashListWithItems IsNot Nothing Then
            ServiceCreatePaymentCashListWithItems = New BaseCreatePaymentCashListWithItemsResponseTypeCashListItem
            ServiceCreatePaymentCashListWithItems.AccountShortCode = oCreatePaymentCashListWithItems.AccountShortCode
            ServiceCreatePaymentCashListWithItems.CashListItemKey = oCreatePaymentCashListWithItems.CashListItemKey
            ServiceCreatePaymentCashListWithItems.TransDetailKey = oCreatePaymentCashListWithItems.TransDetailKey
            ServiceCreatePaymentCashListWithItems.AutoAllocatePaymentSuccessful = oCreatePaymentCashListWithItems.AutoAllocatePaymentSuccessful
            ServiceCreatePaymentCashListWithItems.DocumentCode = oCreatePaymentCashListWithItems.DocumentCode
            ServiceCreatePaymentCashListWithItems.DocumentRef = oCreatePaymentCashListWithItems.DocumentRef
        End If

        Return ServiceCreatePaymentCashListWithItems

    End Function

    Public Shared Function ToServiceCreateReceiptCashListItemList(ByVal oCreateReceiptCashListItem As BaseImplementationTypes.BaseCreateReceiptCashListItemResponseTypeCashListItem) As BaseCreateReceiptCashListItemResponseTypeCashListItem

        Dim ServiceCreateReceiptCashListItem As BaseCreateReceiptCashListItemResponseTypeCashListItem = Nothing
        If oCreateReceiptCashListItem IsNot Nothing Then
            ServiceCreateReceiptCashListItem = New BaseCreateReceiptCashListItemResponseTypeCashListItem
            ServiceCreateReceiptCashListItem.CashListItemKey = oCreateReceiptCashListItem.CashListItemKey
            ServiceCreateReceiptCashListItem.TransDetailKey = oCreateReceiptCashListItem.TransDetailKey
        End If

        Return ServiceCreateReceiptCashListItem

    End Function

    Public Shared Function ToServiceCreateReceiptCashListWithItemsList(ByVal oCreateReceiptCashListWithItems As BaseImplementationTypes.BaseCreateReceiptCashListWithItemsResponseTypeCashListItem) As BaseCreateReceiptCashListWithItemsResponseTypeCashListItem

        Dim ServicCreateReceiptCashListWithItems As BaseCreateReceiptCashListWithItemsResponseTypeCashListItem = Nothing
        If oCreateReceiptCashListWithItems IsNot Nothing Then
            ServicCreateReceiptCashListWithItems = New BaseCreateReceiptCashListWithItemsResponseTypeCashListItem
            ServicCreateReceiptCashListWithItems.AccountShortCode = oCreateReceiptCashListWithItems.AccountShortCode
            ServicCreateReceiptCashListWithItems.CashListItemKey = oCreateReceiptCashListWithItems.CashListItemKey
            ServicCreateReceiptCashListWithItems.TransDetailKey = oCreateReceiptCashListWithItems.TransDetailKey
            ServicCreateReceiptCashListWithItems.AccountShortCode = oCreateReceiptCashListWithItems.AccountShortCode
            ServicCreateReceiptCashListWithItems.DocumentCode = oCreateReceiptCashListWithItems.DocumentCode
            ServicCreateReceiptCashListWithItems.DocumentRef = oCreateReceiptCashListWithItems.DocumentRef
            ServicCreateReceiptCashListWithItems.AutoAllocatePaymentSuccessful = oCreateReceiptCashListWithItems.AutoAllocatePaymentSuccessful
        End If

        Return ServicCreateReceiptCashListWithItems

    End Function

    Public Shared Function ToServiceFindCashDepositList(ByVal oFindCashDeposit As BaseImplementationTypes.BaseCashDepositItemType) As BaseCashDepositItemType

        Dim ServicFindCashDeposit As BaseCashDepositItemType = Nothing
        If oFindCashDeposit IsNot Nothing Then
            With oFindCashDeposit
                ServicFindCashDeposit = New BaseCashDepositItemType

                ServicFindCashDeposit.AccountKey = .AccountKey
                ServicFindCashDeposit.Amount = .Amount
                ServicFindCashDeposit.AvailableBalance = .AvailableBalance
                ServicFindCashDeposit.BankName = .BankName
                ServicFindCashDeposit.Branch = .Branch
                ServicFindCashDeposit.CashDepositKey = .CashDepositKey
                ServicFindCashDeposit.CashDepositRef = .CashDepositRef
                ServicFindCashDeposit.IsDeleted = .IsDeleted
                ServicFindCashDeposit.IsSinglePolicy = .IsSinglePolicy
                ServicFindCashDeposit.PartyKey = .PartyKey
                ServicFindCashDeposit.PartyName = .PartyName
                ServicFindCashDeposit.Product = .Product
                ServicFindCashDeposit.PartyCode = .PartyCode
                ServicFindCashDeposit.CurrencyCode = .CurrencyCode
            End With

        End If

        Return ServicFindCashDeposit

    End Function
    Public Shared Function ToServiceFindCashListReceiptsList(ByVal oFindCashListReceipts As SFI.SAMForInsuranceV2.BaseFindCashListReceiptsResponseTypeCashListItemsRow) As BaseFindCashListReceiptsResponseTypeRow

        Dim ServicFindCashListReceipts As BaseFindCashListReceiptsResponseTypeRow = Nothing
        If oFindCashListReceipts IsNot Nothing Then
            With oFindCashListReceipts
                ServicFindCashListReceipts = New BaseFindCashListReceiptsResponseTypeRow
                ServicFindCashListReceipts.BranchDescription = .BranchDescription
                ServicFindCashListReceipts.CashListItemKey = .CashListItemKey
                ServicFindCashListReceipts.ClientCode = .ClientCode
                ServicFindCashListReceipts.ClientName = .ClientName
                ServicFindCashListReceipts.CurrentStatus = .CurrentStatus
                ServicFindCashListReceipts.DocumentRef = .DocumentRef
                ServicFindCashListReceipts.DrawnBankName = .DrawnBankName
                ServicFindCashListReceipts.InsuranceFileKey = .InsuranceFileKey
                ServicFindCashListReceipts.MediaReference = .MediaReference
                ServicFindCashListReceipts.MediaTypeCode = .MediaTypeCode
                ServicFindCashListReceipts.MediaTypeDescription = .MediaTypeDescription
                ServicFindCashListReceipts.MediaTypeKey = .MediaTypeKey
                ServicFindCashListReceipts.MediaTypeStatusCode = .MediaTypeStatusCode
                ServicFindCashListReceipts.MediaTypeStatusDescription = .MediaTypeStatusDescription
                ServicFindCashListReceipts.MediaTypeStatusKey = .MediaTypeStatusKey
                ServicFindCashListReceipts.PolicyNumber = .PolicyNumber

            End With

        End If

        Return ServicFindCashListReceipts

    End Function
    Public Shared Function ToServiceGetReceiptCashListItemsList(ByVal oGetReceiptCashListItems As SFI.SAMForInsuranceV2.BaseGetReceiptCashListItemsResponseTypeReceiptCashListItemsRow) As BaseGetReceiptCashListItemsResponseTypeRow

        Dim ServiceGetReceiptCashListItems As BaseGetReceiptCashListItemsResponseTypeRow = Nothing
        If oGetReceiptCashListItems IsNot Nothing Then
            With oGetReceiptCashListItems
                ServiceGetReceiptCashListItems = New BaseGetReceiptCashListItemsResponseTypeRow
                ServiceGetReceiptCashListItems.AccountShortCode = .AccountShortCode
                ServiceGetReceiptCashListItems.Amount = .Amount
                ServiceGetReceiptCashListItems.CashListItemKey = .CashListItemKey
                ServiceGetReceiptCashListItems.Letter = .Letter
                ServiceGetReceiptCashListItems.MediaReference = .MediaReference
                ServiceGetReceiptCashListItems.MediaType = .MediaType
                ServiceGetReceiptCashListItems.Status = .Status
            End With

        End If

        Return ServiceGetReceiptCashListItems

    End Function

    Public Shared Function ToServiceAgentCommissionList(ByVal oAgentCommission As BaseImplementationTypes.BaseAgentCommissionResponseTypeAgentCommissionRow) As BaseAgentCommissionResponseTypeRow

        Dim ServicAgentCommission As BaseAgentCommissionResponseTypeRow = Nothing
        If oAgentCommission IsNot Nothing Then
            With oAgentCommission
                ServicAgentCommission = New BaseAgentCommissionResponseTypeRow
                ServicAgentCommission.Agent = .Agent
                ServicAgentCommission.AgentType = .AgentType
                ServicAgentCommission.CommissionBand = .CommissionBand
                ServicAgentCommission.CommissionRate = .CommissionRate
                ServicAgentCommission.CommissionValue = .CommissionValue
                ServicAgentCommission.IsLeadAgent = .IsLeadAgent
                ServicAgentCommission.IsValue = .IsValue
                ServicAgentCommission.MaximumRate = .MaximumRate
                ServicAgentCommission.Premium = .Premium
                ServicAgentCommission.RiskType = .RiskType
                ServicAgentCommission.TaxGroup = .TaxGroup
                ServicAgentCommission.TaxValue = .TaxValue
                ServicAgentCommission.TaxGroupDescription = .TaxGroupDescription
                ServicAgentCommission.IsAmended = .IsAmended
                ServicAgentCommission.OverRideReason = .OverRideReason
                ServicAgentCommission.PerilType = .PerilType
            End With

        End If

        Return ServicAgentCommission

    End Function

    Public Shared Function ToServiceBaseUserDetailsTypeList(ByVal oUserDetails As BaseImplementationTypes.BaseUserDetailsType) As BaseUserDetailsType

        Dim ServicUserDetails As BaseUserDetailsType = Nothing
        If oUserDetails IsNot Nothing Then
            With oUserDetails
                ServicUserDetails = New BaseUserDetailsType()
                ServicUserDetails.EmailAddress = .EmailAddress
                ServicUserDetails.UserKey = .UserKey
                ServicUserDetails.FullName = .FullName
                ServicUserDetails.UserName = .UserName
                ServicUserDetails.EffectiveDate = .EffectiveDate
            End With

        End If

        Return ServicUserDetails

    End Function

    Public Shared Function ToServiceBaseContactTypeList(ByVal oContactDetails As BaseImplementationTypes.BaseContactType) As BaseContactType

        Dim ServiceContactType As BaseContactType = Nothing
        If oContactDetails IsNot Nothing Then
            With oContactDetails
                ServiceContactType = New BaseContactType()
                ServiceContactType.AreaCode = .AreaCode
                ServiceContactType.ContactTypeCode = .ContactTypeCode
                ServiceContactType.OtherContactTypeCode = .OtherContactTypeCode
                ServiceContactType.Description = .Description
                ServiceContactType.Extension = .Extension
            End With
        End If

        Return ServiceContactType

    End Function

    Public Shared Function ToServiceProductList(ByVal oProduct As BaseImplementationTypes.BaseGetCashDepositResponseTypeProducts) As BaseGetCashDepositResponseTypeProducts

        Dim ServiceProduct As BaseGetCashDepositResponseTypeProducts = Nothing
        If oProduct IsNot Nothing Then
            With oProduct
                ServiceProduct = New BaseGetCashDepositResponseTypeProducts
                ServiceProduct.Description = .Description
                ServiceProduct.ProductCode = .ProductCode
                ServiceProduct.ProductKey = .ProductKey
            End With

        End If

        Return ServiceProduct

    End Function

    Public Shared Function ToServiceAgentProductList(ByVal oProduct As SFI.SAMForInsuranceV2.BaseGetProductByAgentResponseTypeProductsRow) As BaseGetProductByAgentResponseTypeRow

        Dim ServiceProduct As BaseGetProductByAgentResponseTypeRow = Nothing
        If oProduct IsNot Nothing Then
            With oProduct
                ServiceProduct = New BaseGetProductByAgentResponseTypeRow
                ServiceProduct.Description = .Description
                ServiceProduct.ProductCode = .ProductCode
                ServiceProduct.ProductKey = .ProductKey
                ServiceProduct.BlockNumber = .BlockNumber
                ServiceProduct.ConsolidatedLeadAgentCommission = .ConsolidatedLeadAgentCommission
                ServiceProduct.ConsolidatedSubAgentCommission = .ConsolidatedSubAgentCommission
                ServiceProduct.SchemeAgencyRef = .SchemeAgencyRef
            End With

        End If

        Return ServiceProduct

    End Function

    Public Shared Function ToServiceBranchList(ByVal oBranch As BaseImplementationTypes.BaseGetCashDepositResponseTypeBranches) As BaseGetCashDepositResponseTypeBranches

        Dim ServiceBranch As BaseGetCashDepositResponseTypeBranches = Nothing
        If oBranch IsNot Nothing Then
            With oBranch
                ServiceBranch = New BaseGetCashDepositResponseTypeBranches
                ServiceBranch.Description = .Description
                ServiceBranch.BranchCode = .BranchCode
                ServiceBranch.BranchKey = .BranchKey
            End With

        End If

        Return ServiceBranch

    End Function

    Public Shared Function ToServiceCashDepositPoliciesList(ByVal oCashDepositPolicies As SFI.SAMForInsuranceV2.BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow) As BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow

        Dim ServiceCashDepositPolicies As BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow = Nothing
        If oCashDepositPolicies IsNot Nothing Then
            With oCashDepositPolicies
                ServiceCashDepositPolicies = New BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow
                ServiceCashDepositPolicies.AccountKey = .AccountKey
                ServiceCashDepositPolicies.Amount = .Amount
                ServiceCashDepositPolicies.AvailableBalance = .AvailableBalance
                ServiceCashDepositPolicies.CashDepositKey = .CashDepositKey
                ServiceCashDepositPolicies.CashDepositRef = .CashDepositRef
                ServiceCashDepositPolicies.DateCreated = .DateCreated
                ServiceCashDepositPolicies.PartyKey = .PartyKey

            End With

        End If

        Return ServiceCashDepositPolicies

    End Function

    Public Shared Function ToServiceGetClaimReinsuranceArrangementLinesList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementLinesResponseTypeReinsuranceArrangementLinesRow) As BaseGetClaimReinsuranceArrangementLinesResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetClaimReinsuranceArrangementLinesResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetClaimReinsuranceArrangementLinesResponseTypeRow
                ServiceReinsuranceArrangementLines.Agreement = .Agreement
                ServiceReinsuranceArrangementLines.Balance = .Balance
                ServiceReinsuranceArrangementLines.DefaultPerc = .DefaultPerc
                ServiceReinsuranceArrangementLines.IsObligatory = .IsObligatory
                ServiceReinsuranceArrangementLines.Name = .Name
                ServiceReinsuranceArrangementLines.PaymentToDate = .PaymentToDate
                ServiceReinsuranceArrangementLines.ReserveToDate = .ReserveToDate
                ServiceReinsuranceArrangementLines.SumInsured = .SumInsured
                ServiceReinsuranceArrangementLines.ThisPayment = .ThisPayment
                ServiceReinsuranceArrangementLines.ThisPerc = .ThisPerc
                ServiceReinsuranceArrangementLines.ThisReserve = .ThisReserve
            End With

        End If

        Return ServiceReinsuranceArrangementLines

    End Function

    Public Shared Function ToServiceGetClaimReinsuranceArrangementsList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementsResponseTypeReinsuranceArrangementsRow) As BaseGetClaimReinsuranceArrangementsResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetClaimReinsuranceArrangementsResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetClaimReinsuranceArrangementsResponseTypeRow
                ServiceReinsuranceArrangementLines.ArrangementId = .ArrangementId
                ServiceReinsuranceArrangementLines.Balance = .Balance
                ServiceReinsuranceArrangementLines.BandId = .BandId
                ServiceReinsuranceArrangementLines.PaymentToDate = .PaymentToDate
                ServiceReinsuranceArrangementLines.ReserveToDate = .ReserveToDate
                ServiceReinsuranceArrangementLines.SumInsured = .SumInsured
                ServiceReinsuranceArrangementLines.ReserveToDate = .ReserveToDate
                ServiceReinsuranceArrangementLines.ThisPayment = .ThisPayment
                ServiceReinsuranceArrangementLines.RecoveryToDate = .RecoveryToDate
                ServiceReinsuranceArrangementLines.ThisReserve = .ThisReserve
            End With

        End If

        Return ServiceReinsuranceArrangementLines

    End Function
    Public Shared Function ToServiceGetRiskReinsuranceArrangementsList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementsResponseTypeArrangementsRow) As BaseGetRiskReinsuranceArrangementsResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetRiskReinsuranceArrangementsResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetRiskReinsuranceArrangementsResponseTypeRow
                ServiceReinsuranceArrangementLines.ArrangementId = .ArrangementId
                ServiceReinsuranceArrangementLines.BandId = .BandId
                ServiceReinsuranceArrangementLines.FACPremiumType = .FACPremiumType
                ServiceReinsuranceArrangementLines.IsModified = .IsModified
                ServiceReinsuranceArrangementLines.IsOriginal = .IsOriginal
                ServiceReinsuranceArrangementLines.ModelId = .ModelId
                ServiceReinsuranceArrangementLines.Premium = .Premium
                ServiceReinsuranceArrangementLines.SumInsured = .SumInsured
                ServiceReinsuranceArrangementLines.RIModelCode = .RIModelCode
                ServiceReinsuranceArrangementLines.ExtendedLimitAmount = .ExtendedLimitAmount
                ServiceReinsuranceArrangementLines.IsExtendedLimitApplied = .IsExtendedLimitApplied
            End With

        End If

        Return ServiceReinsuranceArrangementLines

    End Function
    Public Shared Function ToServiceGetClaimReinsuranceBandsList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceBandsResponseTypeReinsuranceBandsRow) As BaseGetClaimReinsuranceBandsResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetClaimReinsuranceBandsResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetClaimReinsuranceBandsResponseTypeRow
                ServiceReinsuranceArrangementLines.Band = .Band
                ServiceReinsuranceArrangementLines.BandId = .BandId
            End With
        End If

        Return ServiceReinsuranceArrangementLines

    End Function

    Public Shared Function ToServiceGetRiskReinsuranceBandsList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceBandsResponseTypeReinsuranceBandsRow) As BaseGetRiskReinsuranceBandsResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetRiskReinsuranceBandsResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetRiskReinsuranceBandsResponseTypeRow
                ServiceReinsuranceArrangementLines.Band = .Band
                ServiceReinsuranceArrangementLines.BandKey = .BandKey
            End With
        End If

        Return ServiceReinsuranceArrangementLines

    End Function
    Public Shared Function ToServiceSubAgentsList(ByVal oSubAgent As SFI.SAMForInsuranceV2.BaseGetSubAgentsResponseTypeSubAgentsRow) As BaseGetSubAgentsResponseTypeRow

        Dim ServiceSubAgent As BaseGetSubAgentsResponseTypeRow = Nothing
        If oSubAgent IsNot Nothing Then
            With oSubAgent
                ServiceSubAgent = New BaseGetSubAgentsResponseTypeRow
                ServiceSubAgent.Amount = .Amount
                ServiceSubAgent.Code = .Code
                ServiceSubAgent.Name = .Name
                ServiceSubAgent.PartyKey = .PartyKey
                ServiceSubAgent.Percentage = .Percentage
            End With
        End If

        Return ServiceSubAgent

    End Function



    Public Shared Function ToServiceGetPaymentCashListItemsList(ByVal oGetPaymentCashListItems As SFI.SAMForInsuranceV2.BaseGetPaymentCashListItemsResponseTypePaymentCashListItemsRow) As BaseGetPaymentCashListItemsResponseTypeRow

        Dim ServiceGetPaymentCashListItems As BaseGetPaymentCashListItemsResponseTypeRow = Nothing
        If oGetPaymentCashListItems IsNot Nothing Then
            With oGetPaymentCashListItems
                ServiceGetPaymentCashListItems = New BaseGetPaymentCashListItemsResponseTypeRow
                ServiceGetPaymentCashListItems.AccountShortCode = .AccountShortCode
                ServiceGetPaymentCashListItems.Amount = .Amount
                ServiceGetPaymentCashListItems.CashListItemKey = .CashListItemKey
                ServiceGetPaymentCashListItems.Letter = .Letter
                ServiceGetPaymentCashListItems.MediaReference = .MediaReference
                ServiceGetPaymentCashListItems.MediaType = .MediaType
                ServiceGetPaymentCashListItems.Status = .Status
            End With

        End If

        Return ServiceGetPaymentCashListItems

    End Function

    Public Shared Function ToServiceGetRecoveryReinsuranceList(ByVal oGetRecoveryReinsurance As SFI.SAMForInsuranceV2.BaseGetRecoveryReinsuranceResponseTypeReinsurancesRow) As BaseGetRecoveryReinsuranceResponseTypeRow

        Dim ServiceGetRecoveryReinsurance As BaseGetRecoveryReinsuranceResponseTypeRow = Nothing
        If oGetRecoveryReinsurance IsNot Nothing Then
            With oGetRecoveryReinsurance
                ServiceGetRecoveryReinsurance = New BaseGetRecoveryReinsuranceResponseTypeRow
                ServiceGetRecoveryReinsurance.PartyKey = .PartyKey
                ServiceGetRecoveryReinsurance.RecoveryKey = .RecoveryKey
                ServiceGetRecoveryReinsurance.RecoveryToDate = .RecoveryToDate
                ServiceGetRecoveryReinsurance.RecoveryType = .RecoveryType
                ServiceGetRecoveryReinsurance.Reinsurer = .Reinsurer
                ServiceGetRecoveryReinsurance.ThisRecovery = .ThisRecovery
                ServiceGetRecoveryReinsurance.ThisSalvage = .ThisSalvage
                ServiceGetRecoveryReinsurance.Salvage = .Salvage
                ServiceGetRecoveryReinsurance.Recovery = .Recovery
                ServiceGetRecoveryReinsurance.SharePercent = .SharePercent
            End With

        End If

        Return ServiceGetRecoveryReinsurance

    End Function

    Public Shared Function ToServiceGetRiskReinsuranceArrangementLinesList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementLinesResponseTypeArrangementLinesRow) As BaseGetRiskReinsuranceArrangementLinesResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetRiskReinsuranceArrangementLinesResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetRiskReinsuranceArrangementLinesResponseTypeRow
                ServiceReinsuranceArrangementLines.Agreement = .Agreement
                ServiceReinsuranceArrangementLines.DefaultPerc = .DefaultPerc
                ServiceReinsuranceArrangementLines.IsObligatory = .IsObligatory
                ServiceReinsuranceArrangementLines.Name = .Name
                ServiceReinsuranceArrangementLines.SumInsured = .SumInsured
                ServiceReinsuranceArrangementLines.ThisPerc = .ThisPerc
                ServiceReinsuranceArrangementLines.Commission = .Commission
                ServiceReinsuranceArrangementLines.CommissionPerc = .CommissionPerc
                ServiceReinsuranceArrangementLines.CommissionTax = .CommissionTax
                ServiceReinsuranceArrangementLines.Premium = .Premium
                ServiceReinsuranceArrangementLines.Tax = .Tax
                ServiceReinsuranceArrangementLines.ThisPerc = .ThisPerc
            End With

        End If

        Return ServiceReinsuranceArrangementLines

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBankGuarantee"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceBankGuaranteeList(
                         ByVal oBankGuarantee As BaseImplementationTypes.BaseAddBankGuaranteeResponseTypeBankGuaranteeRow) As BaseAddBankGuaranteeResponseTypeRow

        Dim oServiceBankGuarantee As New BaseAddBankGuaranteeResponseTypeRow

        If oBankGuarantee IsNot Nothing Then

            oServiceBankGuarantee.BGKey = oBankGuarantee.BGKey
            oServiceBankGuarantee.BGRef = oBankGuarantee.BGRef
            oServiceBankGuarantee.BGTimeStamp = oBankGuarantee.BGTimeStamp

        End If

        Return oServiceBankGuarantee

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCashDeposit"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceCashDepositList(
                         ByVal oCashDeposit As BaseImplementationTypes.BaseAddCashDepositResponseTypeCashDepositRow) As BaseAddCashDepositResponseTypeRow

        Dim oServiceCashDeposit As New BaseAddCashDepositResponseTypeRow

        If oCashDeposit IsNot Nothing Then

            oServiceCashDeposit.CashDepositKey = oCashDeposit.CashDepositKey
            oServiceCashDeposit.CashDepositRef = oCashDeposit.CashDepositRef
            oServiceCashDeposit.CDTimeStamp = oCashDeposit.CDTimeStamp

        End If

        Return oServiceCashDeposit

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oPartyBGPolicyDetailsList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServicePartyBGPolicyDetailsList(ByVal oPartyBGPolicyDetailsList As SFI.SAMForInsuranceV2.BaseGetPoliciesOnBankGuaranteeByKeyResponseTypePartyBGPolicyDetailsRow) As BaseGetPoliciesOnBankGuaranteeByKeyResponseTypeRow

        Dim oServiceBGPolicyDetailsList As BaseGetPoliciesOnBankGuaranteeByKeyResponseTypeRow = New BaseGetPoliciesOnBankGuaranteeByKeyResponseTypeRow()

        If oPartyBGPolicyDetailsList IsNot Nothing Then
            oServiceBGPolicyDetailsList.AgentCode = oPartyBGPolicyDetailsList.AgentCode
            oServiceBGPolicyDetailsList.BranchDesc = oPartyBGPolicyDetailsList.BranchDesc
            oServiceBGPolicyDetailsList.ClientCode = oPartyBGPolicyDetailsList.ClientCode
            oServiceBGPolicyDetailsList.ClientName = oPartyBGPolicyDetailsList.ClientName
            oServiceBGPolicyDetailsList.CoverEndDate = oPartyBGPolicyDetailsList.CoverEndDate
            oServiceBGPolicyDetailsList.CoverStartDate = oPartyBGPolicyDetailsList.CoverStartDate
            oServiceBGPolicyDetailsList.InsuranceFileKey = oPartyBGPolicyDetailsList.InsuranceFileKey
            oServiceBGPolicyDetailsList.PolicyRef = oPartyBGPolicyDetailsList.PolicyRef
            oServiceBGPolicyDetailsList.PremiumAmount = oPartyBGPolicyDetailsList.PremiumAmount
            oServiceBGPolicyDetailsList.ProductDesc = oPartyBGPolicyDetailsList.ProductDesc
        End If

        Return oServiceBGPolicyDetailsList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oPartyBGPolicyDetailsList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServicePartyBGPolicyForReceiptResposeDetailsList(ByVal oPartyBGPolicyDetailsList As SFI.SAMForInsuranceV2.BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypePartyBGPolicyDetailsRow) As BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypeRow

        Dim oServiceBGPolicyDetailsList As BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypeRow = New BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypeRow()

        If oPartyBGPolicyDetailsList IsNot Nothing Then
            oServiceBGPolicyDetailsList.BankGuaranteeRef = oPartyBGPolicyDetailsList.BankGuaranteeRef
            oServiceBGPolicyDetailsList.BankName = oPartyBGPolicyDetailsList.BankName
            oServiceBGPolicyDetailsList.BankNameKey = oPartyBGPolicyDetailsList.BankNameKey
            oServiceBGPolicyDetailsList.BGDueDate = oPartyBGPolicyDetailsList.BGDueDate
            oServiceBGPolicyDetailsList.BGKey = oPartyBGPolicyDetailsList.BGKey
            oServiceBGPolicyDetailsList.BranchCode = oPartyBGPolicyDetailsList.BranchCode
            oServiceBGPolicyDetailsList.BranchDesc = oPartyBGPolicyDetailsList.BranchDesc
            oServiceBGPolicyDetailsList.CoverEndDate = oPartyBGPolicyDetailsList.CoverEndDate
            oServiceBGPolicyDetailsList.CoverStartDate = oPartyBGPolicyDetailsList.CoverStartDate
            oServiceBGPolicyDetailsList.OutstandingPolicyAmt = oPartyBGPolicyDetailsList.OutstandingPolicyAmt
            oServiceBGPolicyDetailsList.PolicyKey = oPartyBGPolicyDetailsList.PolicyKey
            oServiceBGPolicyDetailsList.PolicyRef = oPartyBGPolicyDetailsList.PolicyRef
            oServiceBGPolicyDetailsList.PremiumAmount = oPartyBGPolicyDetailsList.PremiumAmount
            oServiceBGPolicyDetailsList.ProductCode = oPartyBGPolicyDetailsList.ProductCode
            oServiceBGPolicyDetailsList.ProductDesc = oPartyBGPolicyDetailsList.ProductDesc
        End If

        Return oServiceBGPolicyDetailsList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oRatingDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceRatingDetailsList(
                         ByVal oRatingDetails As SFI.SAMForInsuranceV2.BaseGetRatingDetailsResponseTypeRatingDetailsRow) As BaseGetRatingDetailsResponseTypeRow

        Dim oServiceRatingDetails As New BaseGetRatingDetailsResponseTypeRow

        If oRatingDetails IsNot Nothing Then

            oServiceRatingDetails.AnnualPremium = oRatingDetails.AnnualPremium
            oServiceRatingDetails.AnnualRate = oRatingDetails.AnnualRate
            oServiceRatingDetails.CalculatedPremium = oRatingDetails.CalculatedPremium
            oServiceRatingDetails.Country = oRatingDetails.Country
            oServiceRatingDetails.CountryCode = oRatingDetails.CountryCode
            oServiceRatingDetails.CountryId = oRatingDetails.CountryId
            oServiceRatingDetails.CurrencyCode = oRatingDetails.CurrencyCode
            oServiceRatingDetails.CurrencyId = oRatingDetails.CurrencyId
            oServiceRatingDetails.EarningPattern = oRatingDetails.EarningPattern
            oServiceRatingDetails.EarningPatternCode = oRatingDetails.EarningPatternCode
            oServiceRatingDetails.EarningPatternId = oRatingDetails.EarningPatternId
            oServiceRatingDetails.IsAmended = oRatingDetails.IsAmended
            oServiceRatingDetails.OriginalFlag = oRatingDetails.OriginalFlag
            oServiceRatingDetails.OverrideReason = oRatingDetails.OverrideReason
            oServiceRatingDetails.PolicySectionType = oRatingDetails.PolicySectionType
            oServiceRatingDetails.PolicySectionTypeId = oRatingDetails.PolicySectionTypeId
            oServiceRatingDetails.RateType = oRatingDetails.RateType
            oServiceRatingDetails.RateTypeId = oRatingDetails.RateTypeId
            oServiceRatingDetails.RatingSectionId = oRatingDetails.RatingSectionId
            oServiceRatingDetails.RatingSectionType = oRatingDetails.RatingSectionType
            oServiceRatingDetails.RatingSectionTypeCode = oRatingDetails.RatingSectionTypeCode
            oServiceRatingDetails.RatingSectionTypeId = oRatingDetails.RatingSectionTypeId
            oServiceRatingDetails.RatingTypeCode = oRatingDetails.RatingTypeCode
            oServiceRatingDetails.State = oRatingDetails.State
            oServiceRatingDetails.StateCode = oRatingDetails.StateCode
            oServiceRatingDetails.StateId = oRatingDetails.StateId
            oServiceRatingDetails.SumInsured = oRatingDetails.SumInsured
            oServiceRatingDetails.ThisPremium = oRatingDetails.ThisPremium

        End If

        Return oServiceRatingDetails

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oSectionList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceSectionList(
                         ByVal oSectionList As SFI.SAMForInsuranceV2.BaseGetRatingSectionTypesResponseTypeRatingSectionTypesRow) As BaseGetRatingSectionTypesResponseTypeRow

        Dim oServiceSection As New BaseGetRatingSectionTypesResponseTypeRow

        If oSectionList IsNot Nothing Then

            oServiceSection.CountryCode = oSectionList.CountryCode
            oServiceSection.CountryId = oSectionList.CountryId
            oServiceSection.CurrencyCode = oSectionList.CurrencyCode
            oServiceSection.CurrencyId = oSectionList.CurrencyId
            oServiceSection.Description = oSectionList.Description
            oServiceSection.EarningPatternCode = oSectionList.EarningPatternCode
            oServiceSection.Rate = oSectionList.Rate
            oServiceSection.RateTypeCode = oSectionList.RateTypeCode
            oServiceSection.RateTypeId = oSectionList.RateTypeId
            oServiceSection.RatingSectionTypeCode = oSectionList.RatingSectionTypeCode
            oServiceSection.RatingSectionTypeId = oSectionList.RatingSectionTypeId
            oServiceSection.StateCode = oSectionList.StateCode
            oServiceSection.StateId = oSectionList.StateId

        End If

        Return oServiceSection

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oReferredPaymentList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceReferredPaymentList(
                         ByVal oReferredPaymentList As SFI.SAMForInsuranceV2.BaseGetReferredPaymentsResponseTypeCashListItemsRow) As BaseGetReferredPaymentsResponseTypeRow

        Dim oServiceReferredPaymentList As New BaseGetReferredPaymentsResponseTypeRow

        If oReferredPaymentList IsNot Nothing Then

            oServiceReferredPaymentList.ClaimKey = oReferredPaymentList.ClaimKey
            oServiceReferredPaymentList.ClaimNumber = oReferredPaymentList.ClaimNumber
            oServiceReferredPaymentList.PolicyNumber = oReferredPaymentList.PolicyNumber
            oServiceReferredPaymentList.ClaimPaymentKey = oReferredPaymentList.ClaimPaymentKey
            oServiceReferredPaymentList.ClientName = oReferredPaymentList.ClientName
            oServiceReferredPaymentList.CreatedBy = oReferredPaymentList.CreatedBy

            oServiceReferredPaymentList.PaymentAmount = oReferredPaymentList.PaymentAmount
            oServiceReferredPaymentList.PaymentDate = oReferredPaymentList.PaymentDate
            oServiceReferredPaymentList.PaymentDate = oReferredPaymentList.PaymentDate

            oServiceReferredPaymentList.Status = oReferredPaymentList.Status
            oServiceReferredPaymentList.CaseNumber = oReferredPaymentList.CaseNumber
            oServiceReferredPaymentList.PayeeName = oReferredPaymentList.PayeeName
            oServiceReferredPaymentList.PayeeType = oReferredPaymentList.PayeeType

        End If

        Return oServiceReferredPaymentList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetSharepointFielList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceSharepointFileList(ByVal oGetSharepointFileList As BaseImplementationTypes.BaseGetSharepointFileListResponseTypeItemList) As ArrayOfBaseGetSharepointFileListResponseTypeItemListBaseGetSharepointFileListResponseTypeItemList
        Dim oServiceGetSharepointFileList As New ArrayOfBaseGetSharepointFileListResponseTypeItemListBaseGetSharepointFileListResponseTypeItemList
        If (oGetSharepointFileList) IsNot Nothing Then
            With oGetSharepointFileList

                oServiceGetSharepointFileList.CreatedDate = .CreatedDate
                oServiceGetSharepointFileList.DocumentTemplateGroup = .DocumentTemplateGroup
                oServiceGetSharepointFileList.DocumentTemplateSubGroup = .DocumentTemplateSubGroup
                oServiceGetSharepointFileList.Filename = .Filename
                oServiceGetSharepointFileList.InternalOnly = .InternalOnly
                oServiceGetSharepointFileList.ItemType = .ItemType
                oServiceGetSharepointFileList.LastModifiedDate = .LastModifiedDate
                oServiceGetSharepointFileList.PureUser = .PureUser
                oServiceGetSharepointFileList.Title = .Title
                oServiceGetSharepointFileList.URL = .URL

            End With
        End If

        Return oServiceGetSharepointFileList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetTransactionDetailsList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceTransactionDetailsList(ByVal oGetTransactionDetailsList As SFI.SAMForInsuranceV2.BaseGetTransactionDetailsResponseTypeTransactionsRow) As BaseGetTransactionDetailsResponseTypeRow
        Dim oServiceTransactionDetailsList As New BaseGetTransactionDetailsResponseTypeRow
        If (oGetTransactionDetailsList) IsNot Nothing Then
            With oGetTransactionDetailsList

                oServiceTransactionDetailsList.AccountCode = .AccountCode
                oServiceTransactionDetailsList.Accountkey = .Accountkey
                oServiceTransactionDetailsList.AllocationTimeStamp = .AllocationTimeStamp
                oServiceTransactionDetailsList.AltRef = .AltRef
                oServiceTransactionDetailsList.Amount = .Amount
                oServiceTransactionDetailsList.Currency = .Currency
                oServiceTransactionDetailsList.CurrencyDiff = .CurrencyDiff
                oServiceTransactionDetailsList.DocRef = .DocRef
                oServiceTransactionDetailsList.EffectiveDate = .EffectiveDate
                oServiceTransactionDetailsList.MediaRef = .MediaRef
                oServiceTransactionDetailsList.MediaType = .MediaType
                oServiceTransactionDetailsList.OutstandingAmount = .OutstandingAmount
                oServiceTransactionDetailsList.TaxBand = .TaxBand
                oServiceTransactionDetailsList.TransactionCurrenciesAmount = .TransactionCurrenciesAmount
                oServiceTransactionDetailsList.TransactionCurrency = .TransactionCurrency
                oServiceTransactionDetailsList.TransDate = .TransDate
                oServiceTransactionDetailsList.TransDetailKey = .TransDetailKey
                oServiceTransactionDetailsList.TransactionCurrencyCode = .TransactionCurrencyCode
                oServiceTransactionDetailsList.CurrencyCode = .CurrencyCode
            End With
        End If

        Return oServiceTransactionDetailsList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetPrimaryCausesList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServicePrimaryCausesList(ByVal oGetPrimaryCausesList As SFI.SAMForInsuranceV2.BaseGetValidPrimaryCausesResponseTypePrimaryCausesRow) As BaseGetValidPrimaryCausesResponseTypeRow
        Dim oServicePrimaryCausesList As New BaseGetValidPrimaryCausesResponseTypeRow
        If (oGetPrimaryCausesList) IsNot Nothing Then
            With oGetPrimaryCausesList

                oServicePrimaryCausesList.code = .code
                oServicePrimaryCausesList.description = .description
                oServicePrimaryCausesList.primary_cause_id = .primary_cause_id

            End With
        End If

        Return oServicePrimaryCausesList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetWorkManagerScheduledTasksList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceWorkManagerScheduledTasksList(ByVal oGetWorkManagerScheduledTasksList As SFI.SAMForInsuranceV2.BaseGetWorkManagerScheduledTasksResponseTypeTasksRow) As BaseGetWorkManagerScheduledTasksResponseTypeRow
        Dim oServiceWorkManagerScheduledTasksList As New BaseGetWorkManagerScheduledTasksResponseTypeRow
        If (oGetWorkManagerScheduledTasksList) IsNot Nothing Then
            With oGetWorkManagerScheduledTasksList

                oServiceWorkManagerScheduledTasksList.Customer = .Customer
                oServiceWorkManagerScheduledTasksList.Branch = .Branch
                oServiceWorkManagerScheduledTasksList.Description = .Description
                oServiceWorkManagerScheduledTasksList.DueDate = .DueDate
                oServiceWorkManagerScheduledTasksList.PartyKey = .PartyKey
                oServiceWorkManagerScheduledTasksList.PartyName = .PartyName
                oServiceWorkManagerScheduledTasksList.TaskGroupKey = .TaskGroupKey
                oServiceWorkManagerScheduledTasksList.TaskInstanceKey = .TaskInstanceKey
                oServiceWorkManagerScheduledTasksList.TaskKey = .TaskKey
                oServiceWorkManagerScheduledTasksList.TaskStatusKey = .TaskStatusKey
                oServiceWorkManagerScheduledTasksList.Type = .Type
                oServiceWorkManagerScheduledTasksList.Urgent = .Urgent
                oServiceWorkManagerScheduledTasksList.UserCode = .UserCode
                oServiceWorkManagerScheduledTasksList.UserGroupCode = .UserGroupCode
                oServiceWorkManagerScheduledTasksList.UserGroupDescription = .UserGroupDescription
                oServiceWorkManagerScheduledTasksList.UserGroupKey = .UserGroupKey
                oServiceWorkManagerScheduledTasksList.UserKey = .UserKey

            End With
        End If

        Return oServiceWorkManagerScheduledTasksList

    End Function


    Public Shared Function ToServiceAccountBalanceList(ByVal oAccountBalanceList As SFI.SAMForInsuranceV2.BaseGetAccountBalanceResponseTypeAccountBalanceRow) As BaseGetAccountBalanceResponseTypeRow

        Dim oServiceUserGroupList As New BaseGetAccountBalanceResponseTypeRow

        If oAccountBalanceList IsNot Nothing Then
            oServiceUserGroupList.SumAmount = oAccountBalanceList.SumAmount
            oServiceUserGroupList.CurrencyCode = oAccountBalanceList.CurrencyCode
            oServiceUserGroupList.Overdraft = oAccountBalanceList.Overdraft
            oServiceUserGroupList.FloatBalance = oAccountBalanceList.FloatBalance
        End If

        Return oServiceUserGroupList

    End Function

    Public Shared Function ToServiceGetFinancePlanDetailsInstalmentList(ByVal oGetFinancePlanDetailsInstalment As BaseImplementationTypes.BaseGetFinancePlanDetailsResponseTypeInstalmentsRow) As BaseGetFinancePlanDetailsResponseTypeRow

        Dim oServiceGetFinancePlanDetailsInstalmentList As New BaseGetFinancePlanDetailsResponseTypeRow

        If oGetFinancePlanDetailsInstalment IsNot Nothing Then

            With oGetFinancePlanDetailsInstalment

                oServiceGetFinancePlanDetailsInstalmentList.Amount = .Amount
                oServiceGetFinancePlanDetailsInstalmentList.DueDate = .DueDate
                oServiceGetFinancePlanDetailsInstalmentList.InstalmentNumber = .InstalmentNumber
                oServiceGetFinancePlanDetailsInstalmentList.PaymentDate = .PaymentDate
                oServiceGetFinancePlanDetailsInstalmentList.PaymentDateSpecified = .PaymentDateSpecified
                oServiceGetFinancePlanDetailsInstalmentList.Reason = .Reason
                oServiceGetFinancePlanDetailsInstalmentList.Status = .Status

            End With

        End If

        Return oServiceGetFinancePlanDetailsInstalmentList

    End Function

    Public Shared Function ToServiceGetAccountDetailsTransactionsList(ByVal oGetAccountDetailsTransactions As BaseImplementationTypes.BaseGetAccountDetailsResponseTypeTransactionsRow) As BaseGetAccountDetailsResponseTypeRow

        Dim oServiceGetAccountDetailsTransactionsList As New BaseGetAccountDetailsResponseTypeRow

        If oGetAccountDetailsTransactions IsNot Nothing Then

            With oGetAccountDetailsTransactions

                oServiceGetAccountDetailsTransactionsList.Account = .Account
                oServiceGetAccountDetailsTransactionsList.AccountAmount = .AccountAmount
                oServiceGetAccountDetailsTransactionsList.AccountCurrencyCode = .AccountCurrencyCode
                oServiceGetAccountDetailsTransactionsList.Accountkey = .Accountkey
                oServiceGetAccountDetailsTransactionsList.AccountOutStandingAmount = .AccountOutStandingAmount
                oServiceGetAccountDetailsTransactionsList.AltRef = .AltRef
                oServiceGetAccountDetailsTransactionsList.Amount = .Amount
                oServiceGetAccountDetailsTransactionsList.BalanceType = .BalanceType
                oServiceGetAccountDetailsTransactionsList.BaseCurrencyCode = .BaseCurrencyCode
                oServiceGetAccountDetailsTransactionsList.BGRef = .BGRef
                oServiceGetAccountDetailsTransactionsList.BranchKey = .BranchKey
                oServiceGetAccountDetailsTransactionsList.Client = .Client
                oServiceGetAccountDetailsTransactionsList.ClientCode = .ClientCode
                oServiceGetAccountDetailsTransactionsList.CurrencyAmount = .CurrencyAmount
                oServiceGetAccountDetailsTransactionsList.CurrencyCode = .CurrencyCode
                oServiceGetAccountDetailsTransactionsList.DocRef = .DocRef
                oServiceGetAccountDetailsTransactionsList.DocTypeDescription = .DocTypeDescription
                oServiceGetAccountDetailsTransactionsList.DocTypeId = .DocTypeId
                oServiceGetAccountDetailsTransactionsList.DocumentComment = .DocumentComment
                oServiceGetAccountDetailsTransactionsList.DocumentGroupCode = .DocumentGroupCode
                oServiceGetAccountDetailsTransactionsList.DocumentGroupId = .DocumentGroupId
                oServiceGetAccountDetailsTransactionsList.DocumentTypeCode = .DocumentTypeCode
                oServiceGetAccountDetailsTransactionsList.DueDate = .DueDate
                oServiceGetAccountDetailsTransactionsList.EffectiveDate = .EffectiveDate
                oServiceGetAccountDetailsTransactionsList.InstalmentCollection = .InstalmentCollection
                oServiceGetAccountDetailsTransactionsList.MediaRef = .MediaRef
                oServiceGetAccountDetailsTransactionsList.MediaType = .MediaType
                oServiceGetAccountDetailsTransactionsList.OperatorName = .OperatorName
                oServiceGetAccountDetailsTransactionsList.OutstandingAmount = .OutstandingAmount
                oServiceGetAccountDetailsTransactionsList.OutStandingCurrencyAmount = .OutStandingCurrencyAmount
                oServiceGetAccountDetailsTransactionsList.PaidDate = .PaidDate
                oServiceGetAccountDetailsTransactionsList.PayeeName = .PayeeName
                oServiceGetAccountDetailsTransactionsList.Period = .Period
                oServiceGetAccountDetailsTransactionsList.PrimarySettled = .PrimarySettled
                oServiceGetAccountDetailsTransactionsList.Reference = .Reference
                oServiceGetAccountDetailsTransactionsList.TransDate = .TransDate
                oServiceGetAccountDetailsTransactionsList.TransDetailKey = .TransDetailKey
                oServiceGetAccountDetailsTransactionsList.UnderwritingYear = .UnderwritingYear

                oServiceGetAccountDetailsTransactionsList.CashListKey = .CashListKey
                oServiceGetAccountDetailsTransactionsList.IsSplitReceipt = .IsSplitReceipt
                oServiceGetAccountDetailsTransactionsList.IsLeadAgent = .IsLead
                oServiceGetAccountDetailsTransactionsList.CashListItemKey = .CashListItemKey
                oServiceGetAccountDetailsTransactionsList.BankAccountID = .BankAccountID
                oServiceGetAccountDetailsTransactionsList.PartyCnt = .PartyCnt
                oServiceGetAccountDetailsTransactionsList.FinancePlanKey = .FinancePlanKey
                oServiceGetAccountDetailsTransactionsList.FinancePlanVersion = .FinancePlanVersion
                oServiceGetAccountDetailsTransactionsList.FinancePlanStatus = .FinancePlanStatus
                oServiceGetAccountDetailsTransactionsList.Insurance_file_cnt = .Insurance_file_cnt
                oServiceGetAccountDetailsTransactionsList.Insurance_folder_cnt = .Insurance_folder_cnt
            End With

        End If

        Return oServiceGetAccountDetailsTransactionsList

    End Function

    Public Shared Function ToServiceGetAccountingPeriodList(ByVal oGetAccountingPeriod As SFI.SAMForInsuranceV2.BaseGetAccountingPeriodResponseTypePeriodRow) As BaseGetAccountingPeriodResponseTypeRow

        Dim oServiceGetAccountingPeriodList As New BaseGetAccountingPeriodResponseTypeRow

        If oGetAccountingPeriod IsNot Nothing Then

            With oGetAccountingPeriod
                oServiceGetAccountingPeriodList.PeriodEndDate = .PeriodEndDate
                oServiceGetAccountingPeriodList.PeriodKey = .PeriodKey
                oServiceGetAccountingPeriodList.PeriodName = .PeriodName
                oServiceGetAccountingPeriodList.YearName = .YearName

            End With

        End If

        Return oServiceGetAccountingPeriodList

    End Function

    Public Shared Function ToServiceGetBankAccountsList(ByVal oGetBankAccounts As SFI.SAMForInsuranceV2.BaseGetBankAccountsResponseTypeBankAccountsRow) As BaseGetBankAccountsResponseTypeRow

        Dim oServiceGetBankAccountsList As New BaseGetBankAccountsResponseTypeRow

        If oGetBankAccounts IsNot Nothing Then

            With oGetBankAccounts
                oServiceGetBankAccountsList.BankAccountKey = .BankAccountKey
                oServiceGetBankAccountsList.BankAccountName = .BankAccountName
                oServiceGetBankAccountsList.BankAccountNumber = .BankAccountNumber
                oServiceGetBankAccountsList.Code = .Code
                oServiceGetBankAccountsList.CurrencyCode = .CurrencyCode
                oServiceGetBankAccountsList.CurrencyKey = .CurrencyKey
                oServiceGetBankAccountsList.Description = .Description
                oServiceGetBankAccountsList.EffectiveDate = .EffectiveDate
                oServiceGetBankAccountsList.IsDeleted = .IsDeleted

            End With

        End If

        Return oServiceGetBankAccountsList

    End Function

    Public Shared Function ToServiceGetClaimPaymentTaxGroupsList(ByVal oGetClaimPaymentTaxGroups As SFI.SAMForInsuranceV2.BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroupRow) As BaseGetClaimPaymentTaxGroupsResponseTypeRow

        Dim oServiceGetClaimPaymentTaxGroupsList As New BaseGetClaimPaymentTaxGroupsResponseTypeRow

        If oGetClaimPaymentTaxGroups IsNot Nothing Then

            With oGetClaimPaymentTaxGroups
                oServiceGetClaimPaymentTaxGroupsList.Code = .Code
                oServiceGetClaimPaymentTaxGroupsList.Description = .Description
                oServiceGetClaimPaymentTaxGroupsList.IsWithholdingTax = .IsWithholdingTax
            End With

        End If

        Return oServiceGetClaimPaymentTaxGroupsList

    End Function

    Public Shared Function ToServiceGetEventNoteList(ByVal oGetEventNote As SFI.SAMForInsuranceV2.BaseGetEventNoteTypeResponseTypeEventTypesRow) As BaseGetEventNoteTypeResponseTypeRow

        Dim oServiceGetEventNoteList As New BaseGetEventNoteTypeResponseTypeRow

        If oGetEventNote IsNot Nothing Then

            With oGetEventNote
                oServiceGetEventNoteList.EventTypeCode = .EventTypeCode
                oServiceGetEventNoteList.EventTypeDescription = .EventTypeDescription
                oServiceGetEventNoteList.EventTypeKey = .EventTypeKey
            End With

        End If

        Return oServiceGetEventNoteList

    End Function

    Public Shared Function ToServiceGetFinancePlansList(ByVal oGetFinancePlans As BaseImplementationTypes.BaseGetFinancePlansResponseTypeFinancePlansRow) As BaseGetFinancePlansResponseTypeRow

        Dim oServiceGetFinancePlansList As New BaseGetFinancePlansResponseTypeRow

        If oGetFinancePlans IsNot Nothing Then

            With oGetFinancePlans
                oServiceGetFinancePlansList.AccountNumber = .AccountNumber
                oServiceGetFinancePlansList.Amount = .Amount
                oServiceGetFinancePlansList.FinancePlanKey = .FinancePlanKey
                oServiceGetFinancePlansList.FinancePlanVersion = .FinancePlanVersion
                oServiceGetFinancePlansList.FinanceProvider = .FinanceProvider
                oServiceGetFinancePlansList.Frequency = .Frequency
                oServiceGetFinancePlansList.InsuranceRef = .InsuranceRef
                oServiceGetFinancePlansList.NextDueDate = .NextDueDate
                oServiceGetFinancePlansList.PlanReference = .PlanReference
                oServiceGetFinancePlansList.RemainingInstalments = .RemainingInstalments
                oServiceGetFinancePlansList.Status = .Status
            End With

        End If

        Return oServiceGetFinancePlansList

    End Function

    Public Shared Function ToServiceGetInsurerPaymentsList(ByVal oGetFinancePlans As SFI.SAMForInsuranceV2.BaseGetInsurerPaymentsResponseTypeInsurerPaymentsRow) As BaseGetInsurerPaymentsResponseTypeRow

        Dim oServiceGetInsurerPaymentsList As New BaseGetInsurerPaymentsResponseTypeRow

        If oGetFinancePlans IsNot Nothing Then

            With oGetFinancePlans
                oServiceGetInsurerPaymentsList.AccountAmount = .AccountAmount
                oServiceGetInsurerPaymentsList.AccountBaseRate = .AccountBaseRate
                oServiceGetInsurerPaymentsList.AccountCurrencyCode = .AccountCurrencyCode
                oServiceGetInsurerPaymentsList.AccountCurrencyId = .AccountCurrencyId
                oServiceGetInsurerPaymentsList.AccountingDate = .AccountingDate
                oServiceGetInsurerPaymentsList.AlternateReference = .AlternateReference
                oServiceGetInsurerPaymentsList.BranchCode = .BranchCode
                oServiceGetInsurerPaymentsList.ClientOutstanding = .ClientOutstanding
                oServiceGetInsurerPaymentsList.ClientOutstandingAccountAmount = .ClientOutstandingAccountAmount
                oServiceGetInsurerPaymentsList.CompanyId = .CompanyId
                oServiceGetInsurerPaymentsList.ConsolidateBinder = .ConsolidateBinder
                oServiceGetInsurerPaymentsList.CurrencyAmount = .CurrencyAmount
                oServiceGetInsurerPaymentsList.CurrencyBaseRate = .CurrencyBaseRate
                oServiceGetInsurerPaymentsList.CurrencyCode = .CurrencyCode
                oServiceGetInsurerPaymentsList.CurrencyId = .CurrencyId
                oServiceGetInsurerPaymentsList.DocumentId = .DocumentId
                oServiceGetInsurerPaymentsList.DocumentRef = .DocumentRef
                oServiceGetInsurerPaymentsList.EffectiveDate = .EffectiveDate
                oServiceGetInsurerPaymentsList.FullyPaidAccountAmount = .FullyPaidAccountAmount
                oServiceGetInsurerPaymentsList.FullyPaidAmount = .FullyPaidAmount
                oServiceGetInsurerPaymentsList.InsurerRef = .InsurerRef
                oServiceGetInsurerPaymentsList.MarkedAccountAmount = .MarkedAccountAmount
                oServiceGetInsurerPaymentsList.MarkedAmount = .MarkedAmount
                oServiceGetInsurerPaymentsList.Month = .Month
                oServiceGetInsurerPaymentsList.PaidAccountAmount = .PaidAccountAmount
                oServiceGetInsurerPaymentsList.PaidAmount = .PaidAmount
                oServiceGetInsurerPaymentsList.PeriodName = .PeriodName
                oServiceGetInsurerPaymentsList.ResolvedName = .ResolvedName
                oServiceGetInsurerPaymentsList.ShortName = .ShortName
                oServiceGetInsurerPaymentsList.Spare = .Spare
                oServiceGetInsurerPaymentsList.TransdetailId = .TransdetailId
                oServiceGetInsurerPaymentsList.YearName = .YearName
                oServiceGetInsurerPaymentsList.DueDate = .DueDate
            End With

        End If

        Return oServiceGetInsurerPaymentsList

    End Function

    Public Shared Function ToServiceGetListResponseList(ByVal oGetList As SFI.SAMForInsuranceV2.BaseGetListResponseTypeListRow) As BaseGetListResponseTypeRow

        Dim oServiceGetListResponseList As New BaseGetListResponseTypeRow

        If oGetList IsNot Nothing Then

            With oGetList
                oServiceGetListResponseList.Code = .Code
                oServiceGetListResponseList.Description = .Description
                oServiceGetListResponseList.EffectiveDate = .EffectiveDate
                oServiceGetListResponseList.IsDeleted = .IsDeleted
                oServiceGetListResponseList.Key = .Key
                oServiceGetListResponseList.ParentKey = .ParentKey
                oServiceGetListResponseList.ParentKeySpecified = .ParentKeySpecified
            End With
        End If

        Return oServiceGetListResponseList

    End Function

    Public Shared Function ToServiceGetMIDFileDetailsPoliciesList(ByVal oGetMIDFileDetails As BaseImplementationTypes.BaseGetMIDFileDetailsResponseTypePoliciesRow) As BaseGetMIDFileDetailsResponseTypeRow

        Dim oServiceGetMIDFileDetailsPoliciesList As New BaseGetMIDFileDetailsResponseTypeRow

        If oGetMIDFileDetails IsNot Nothing Then

            With oGetMIDFileDetails
                oServiceGetMIDFileDetailsPoliciesList.BatchKey = .BatchKey
                oServiceGetMIDFileDetailsPoliciesList.BatchRef = .BatchRef
                oServiceGetMIDFileDetailsPoliciesList.InsuranceFileKey = .InsuranceFileKey
                oServiceGetMIDFileDetailsPoliciesList.InsuranceFileRef = .InsuranceFileRef
                oServiceGetMIDFileDetailsPoliciesList.MidPolicyKey = .MidPolicyKey
                oServiceGetMIDFileDetailsPoliciesList.MidPolicyStatusCode = .MidPolicyStatusCode
                oServiceGetMIDFileDetailsPoliciesList.PPPC = .PPPC
                oServiceGetMIDFileDetailsPoliciesList.ExpectedPPPC = .ExpectedPPPC
                oServiceGetMIDFileDetailsPoliciesList.RejectErrorCodes = .RejectErrorCodes
                oServiceGetMIDFileDetailsPoliciesList.RejectReference = .RejectReference
                oServiceGetMIDFileDetailsPoliciesList.StatusCode = .StatusCode
                oServiceGetMIDFileDetailsPoliciesList.UpdateType = .UpdateType
                If .Vehicles IsNot Nothing Then

                    oServiceGetMIDFileDetailsPoliciesList.Vehicles = .Vehicles.Row.ToList().ConvertAll(
                            New Converter(Of BaseImplementationTypes.BaseGetMIDFileDetailsResponseTypePoliciesRowVehiclesRow, BaseGetMIDFileDetailsResponseTypeRowRow)(AddressOf CommonFunctions.ToServiceGetMIDFileDetailsVehiclesList))
                End If

            End With
        End If

        Return oServiceGetMIDFileDetailsPoliciesList

    End Function

    Public Shared Function ToServiceGetMIDFileDetailsVehiclesList(ByVal oGetMIDFileDetails As BaseImplementationTypes.BaseGetMIDFileDetailsResponseTypePoliciesRowVehiclesRow) As BaseGetMIDFileDetailsResponseTypeRowRow

        Dim oServiceGetMIDFileDetailsVehiclesList As New BaseGetMIDFileDetailsResponseTypeRowRow

        If oGetMIDFileDetails IsNot Nothing Then

            With oGetMIDFileDetails
                oServiceGetMIDFileDetailsVehiclesList.IsForeignReg = .IsForeignReg
                oServiceGetMIDFileDetailsVehiclesList.IsTradeReg = .IsTradeReg
                oServiceGetMIDFileDetailsVehiclesList.Make = .Make
                oServiceGetMIDFileDetailsVehiclesList.MIDPolicyKey = .MIDPolicyKey
                oServiceGetMIDFileDetailsVehiclesList.MIDVehicleKey = .MIDVehicleKey
                oServiceGetMIDFileDetailsVehiclesList.Model = .Model
                oServiceGetMIDFileDetailsVehiclesList.OffDate = .OffDate
                oServiceGetMIDFileDetailsVehiclesList.OnDate = .OnDate
                oServiceGetMIDFileDetailsVehiclesList.Registration = .Registration
                oServiceGetMIDFileDetailsVehiclesList.RejectErrorCodes = .RejectErrorCodes
                oServiceGetMIDFileDetailsVehiclesList.RejectReference = .RejectReference
                oServiceGetMIDFileDetailsVehiclesList.StatusCode = .StatusCode
                oServiceGetMIDFileDetailsVehiclesList.UpdateType = .UpdateType

            End With
        End If

        Return oServiceGetMIDFileDetailsVehiclesList

    End Function

    Public Shared Function ToServiceGetMIDFilesMIDFilesList(ByVal oGetMIDFilesMIDFiles As BaseImplementationTypes.BaseGetMIDFilesResponseTypeMIDFilesRow) As BaseGetMIDFilesResponseTypeRow

        Dim oServiceGetMIDFilesMIDFilesList As New BaseGetMIDFilesResponseTypeRow

        If oGetMIDFilesMIDFiles IsNot Nothing Then

            With oGetMIDFilesMIDFiles
                oServiceGetMIDFilesMIDFilesList.DateGenerated = .DateGenerated
                oServiceGetMIDFilesMIDFilesList.FileName = .FileName
                oServiceGetMIDFilesMIDFilesList.FileSequenceNumber = .FileSequenceNumber
                oServiceGetMIDFilesMIDFilesList.MIDFileKey = .MIDFileKey
                oServiceGetMIDFilesMIDFilesList.MIDFileKeySpecified = .MIDFileKeySpecified
                oServiceGetMIDFilesMIDFilesList.StatusDescription = .StatusDescription

            End With
        End If

        Return oServiceGetMIDFilesMIDFilesList

    End Function

    Public Shared Function ToServiceGetPeriodList(ByVal oGetPeriod As SFI.SAMForInsuranceV2.BaseGetPeriodResponseTypePeriodsRow) As BaseGetPeriodResponseTypeRow

        Dim oServiceGetPeriodList As New BaseGetPeriodResponseTypeRow

        If oGetPeriod IsNot Nothing Then

            With oGetPeriod
                oServiceGetPeriodList.AllocationIndicator = .AllocationIndicator
                oServiceGetPeriodList.PeriodID = .PeriodID
                oServiceGetPeriodList.PeriodName = .PeriodName
                oServiceGetPeriodList.YearName = .YearName

            End With
        End If

        Return oServiceGetPeriodList

    End Function

    Public Shared Function ToServiceGetPoliciesForRenewalSelectionList(ByVal oGetPoliciesForRenewalSelection As SFI.SAMForInsuranceV2.BaseGetPoliciesForRenewalSelectionResponseTypePoliciesRow) As BaseGetPoliciesForRenewalSelectionResponseTypeRow

        Dim oServiceGetPoliciesForRenewalSelectionList As New BaseGetPoliciesForRenewalSelectionResponseTypeRow

        If oGetPoliciesForRenewalSelection IsNot Nothing Then

            With oGetPoliciesForRenewalSelection
                oServiceGetPoliciesForRenewalSelectionList.AnniversaryCopy = .AnniversaryCopy
                oServiceGetPoliciesForRenewalSelectionList.Client = .Client
                oServiceGetPoliciesForRenewalSelectionList.ClientCode = .ClientCode
                oServiceGetPoliciesForRenewalSelectionList.CoverEndDate = .CoverEndDate
                oServiceGetPoliciesForRenewalSelectionList.CoverStartDate = .CoverStartDate
                oServiceGetPoliciesForRenewalSelectionList.InsuranceFileKey = .InsuranceFileKey
                oServiceGetPoliciesForRenewalSelectionList.InsuranceFileRef = .InsuranceFileRef
                oServiceGetPoliciesForRenewalSelectionList.InsuranceFolderKey = .InsuranceFolderKey
                oServiceGetPoliciesForRenewalSelectionList.IsAutoRenewable = .IsAutoRenewable
                oServiceGetPoliciesForRenewalSelectionList.IsClosed = .IsClosed
                oServiceGetPoliciesForRenewalSelectionList.IsInTransferMode = .IsInTransferMode
                oServiceGetPoliciesForRenewalSelectionList.IsTrueMonthlyPolicy = .IsTrueMonthlyPolicy
                oServiceGetPoliciesForRenewalSelectionList.LeadAgent = .LeadAgent
                oServiceGetPoliciesForRenewalSelectionList.LeadAgentKey = .LeadAgentKey
                oServiceGetPoliciesForRenewalSelectionList.PartyKey = .PartyKey
                oServiceGetPoliciesForRenewalSelectionList.ProductDescription = .ProductDescription
                oServiceGetPoliciesForRenewalSelectionList.ProductKey = .ProductKey
                oServiceGetPoliciesForRenewalSelectionList.RenewalCount = .RenewalCount
                oServiceGetPoliciesForRenewalSelectionList.RenewalDate = .RenewalDate

            End With
        End If

        Return oServiceGetPoliciesForRenewalSelectionList

    End Function

    Public Shared Function ToServiceGetPoliciesInRenewalList(ByVal oGetPoliciesInRenewal As SFI.SAMForInsuranceV2.BaseGetPoliciesInRenewalResponseTypePoliciesRow) As BaseGetPoliciesInRenewalResponseTypeRow

        Dim oServiceGetPoliciesInRenewalList As New BaseGetPoliciesInRenewalResponseTypeRow

        If oGetPoliciesInRenewal IsNot Nothing Then

            With oGetPoliciesInRenewal
                oServiceGetPoliciesInRenewalList.AccHandler = .AccHandler
                oServiceGetPoliciesInRenewalList.AnniversaryCopy = .AnniversaryCopy
                oServiceGetPoliciesInRenewalList.BranchCode = .BranchCode
                oServiceGetPoliciesInRenewalList.ClaimIndicator = .ClaimIndicator
                oServiceGetPoliciesInRenewalList.CoverEndDate = .CoverEndDate
                oServiceGetPoliciesInRenewalList.CoverStartDate = .CoverStartDate
                oServiceGetPoliciesInRenewalList.InsuranceFileKey = .InsuranceFileKey
                oServiceGetPoliciesInRenewalList.InsuranceFileRef = .InsuranceFileRef
                oServiceGetPoliciesInRenewalList.InsuranceFileStatusDescription = .InsuranceFileStatusDescription
                oServiceGetPoliciesInRenewalList.InsuranceFileTypeDescription = .InsuranceFileTypeDescription
                oServiceGetPoliciesInRenewalList.InsuranceFolderKey = .InsuranceFolderKey
                oServiceGetPoliciesInRenewalList.IsClosed = .IsClosed
                oServiceGetPoliciesInRenewalList.IsTrueMonthlyPolicy = .IsTrueMonthlyPolicy
                oServiceGetPoliciesInRenewalList.LeadAgent = .LeadAgent
                oServiceGetPoliciesInRenewalList.LeadAgentKey = .LeadAgentKey
                oServiceGetPoliciesInRenewalList.PartyKey = .PartyKey
                oServiceGetPoliciesInRenewalList.PartyName = .PartyName
                oServiceGetPoliciesInRenewalList.ProductCode = .ProductCode
                oServiceGetPoliciesInRenewalList.ProductDescription = .ProductDescription
                oServiceGetPoliciesInRenewalList.RenewalDate = .RenewalDate
                oServiceGetPoliciesInRenewalList.RenewalPremium = .RenewalPremium
                oServiceGetPoliciesInRenewalList.RenewalStatusKey = .RenewalStatusKey
                oServiceGetPoliciesInRenewalList.RenewalStatusTypeCode = .RenewalStatusTypeCode
                oServiceGetPoliciesInRenewalList.RenewalStatusTypeDescription = .RenewalStatusTypeDescription

            End With
        End If

        Return oServiceGetPoliciesInRenewalList

    End Function


    Public Shared Function ToServiceGetTaskGroupTasksList(ByVal oGetTaskGroupTasks As SFI.SAMForInsuranceV2.BaseGetTaskGroupTasksResponseTypeTaskGroupTasksRow) As BaseGetTaskGroupTasksResponseTypeRow

        Dim oServiceGetTaskGroupTasksList As New BaseGetTaskGroupTasksResponseTypeRow

        If oGetTaskGroupTasks IsNot Nothing Then

            With oGetTaskGroupTasks
                oServiceGetTaskGroupTasksList.Description = .Description
                oServiceGetTaskGroupTasksList.DisplayIcon = .DisplayIcon
                oServiceGetTaskGroupTasksList.EffectiveDate = .EffectiveDate
                oServiceGetTaskGroupTasksList.IsAvailable = .IsAvailable
                oServiceGetTaskGroupTasksList.IsDeleted = .IsDeleted
                oServiceGetTaskGroupTasksList.IsIncluded = .IsIncluded
                oServiceGetTaskGroupTasksList.IsViewOnly = .IsViewOnly
                oServiceGetTaskGroupTasksList.Name = .Name
                oServiceGetTaskGroupTasksList.TaskCategoryKey = .TaskCategoryKey
                oServiceGetTaskGroupTasksList.TaskKey = .TaskKey

            End With
        End If

        Return oServiceGetTaskGroupTasksList

    End Function

    Public Shared Function ToServiceGetTaskGroupsList(ByVal oGetTaskGroups As SFI.SAMForInsuranceV2.BaseGetTaskGroupsResponseTypeTaskGroupsRow) As BaseGetTaskGroupsResponseTypeRow

        Dim oServiceGetTaskGroupsList As New BaseGetTaskGroupsResponseTypeRow

        If oGetTaskGroups IsNot Nothing Then

            With oGetTaskGroups
                oServiceGetTaskGroupsList.Description = .CaptionID
                oServiceGetTaskGroupsList.Code = .Code
                oServiceGetTaskGroupsList.Description = .Description
                oServiceGetTaskGroupsList.EffectiveDate = .EffectiveDate
                oServiceGetTaskGroupsList.IsDeleted = .IsDeleted
                oServiceGetTaskGroupsList.TaskGroupCategoryKey = .TaskGroupCategoryKey
                oServiceGetTaskGroupsList.TaskGroupKey = .TaskGroupKey

            End With
        End If

        Return oServiceGetTaskGroupsList

    End Function

    Public Shared Function ToServiceGetTaxGroupsForClaimsList(ByVal oGetTaxGroupsForClaims As SFI.SAMForInsuranceV2.BaseGetTaxGroupsForClaimsResponseTypeTaxGroupsRow) As BaseGetTaxGroupsForClaimsResponseTypeRow

        Dim oServiceGetTaxGroupsForClaimsList As New BaseGetTaxGroupsForClaimsResponseTypeRow

        If oGetTaxGroupsForClaims IsNot Nothing Then

            With oGetTaxGroupsForClaims
                oServiceGetTaxGroupsForClaimsList.AdvanceTaxScript = .AdvanceTaxScript
                oServiceGetTaxGroupsForClaimsList.Code = .Code
                oServiceGetTaxGroupsForClaimsList.Description = .Description
                oServiceGetTaxGroupsForClaimsList.IsWithHoldingTax = .IsWithHoldingTax
                oServiceGetTaxGroupsForClaimsList.TaxGroupKey = .TaxGroupKey

            End With
        End If

        Return oServiceGetTaxGroupsForClaimsList

    End Function

    Public Shared Function ToServiceGetUserGroupTaskGroupsList(ByVal oGetUserGroupTaskGroups As SFI.SAMForInsuranceV2.BaseGetUserGroupTaskGroupsResponseTypeTaskGroupsRow) As BaseGetUserGroupTaskGroupsResponseTypeRow

        Dim oServiceGetUserGroupTaskGroupsList As New BaseGetUserGroupTaskGroupsResponseTypeRow

        If oGetUserGroupTaskGroups IsNot Nothing Then

            With oGetUserGroupTaskGroups
                oServiceGetUserGroupTaskGroupsList.Code = .Code
                oServiceGetUserGroupTaskGroupsList.Description = .Description
                oServiceGetUserGroupTaskGroupsList.DisplaySequence = .DisplaySequence
                oServiceGetUserGroupTaskGroupsList.EffectiveDate = .EffectiveDate
                oServiceGetUserGroupTaskGroupsList.IsDeleted = .IsDeleted
                oServiceGetUserGroupTaskGroupsList.IsIncluded = .IsIncluded
                oServiceGetUserGroupTaskGroupsList.TaskGroupKey = .TaskGroupKey
            End With
        End If

        Return oServiceGetUserGroupTaskGroupsList

    End Function

    Public Shared Function ToServiceGetUserGroupUsersList(ByVal oGetUserGroupUsers As SFI.SAMForInsuranceV2.BaseGetUserGroupUsersResponseTypeUserGroupUsersRow) As BaseGetUserGroupUsersResponseTypeRow

        Dim oServiceGetUserGroupUsersList As New BaseGetUserGroupUsersResponseTypeRow

        If oGetUserGroupUsers IsNot Nothing Then

            With oGetUserGroupUsers
                oServiceGetUserGroupUsersList.EmailAddress = .EmailAddress
                oServiceGetUserGroupUsersList.Name = .Name
                oServiceGetUserGroupUsersList.UserKey = .UserKey

            End With
        End If

        Return oServiceGetUserGroupUsersList

    End Function

    Public Shared Function ToServiceGetUserGroupsList(ByVal oGetUserGroups As SFI.SAMForInsuranceV2.BaseGetUserGroupsResponseTypeUserGroupsRow) As BaseGetUserGroupsResponseTypeRow

        Dim oServiceGetUserGroupsList As New BaseGetUserGroupsResponseTypeRow

        If oGetUserGroups IsNot Nothing Then

            With oGetUserGroups
                oServiceGetUserGroupsList.Code = .Code
                oServiceGetUserGroupsList.Description = .Description
                oServiceGetUserGroupsList.EffectiveDate = .EffectiveDate
                oServiceGetUserGroupsList.IsDeleted = .IsDeleted
                oServiceGetUserGroupsList.IsSystemAdmin = .IsSystemAdmin
                oServiceGetUserGroupsList.UserGroupKey = .UserGroupKey
            End With
        End If

        Return oServiceGetUserGroupsList

    End Function

    Public Shared Function ToServiceGetUserGroupsbyTaskList(ByVal oGetUserGroupsbyTask As SFI.SAMForInsuranceV2.BaseGetUserGroupsbyTaskResponseTypeUserGroupsRow) As BaseGetUserGroupsbyTaskResponseTypeRow

        Dim oServiceGetUserGroupsbyTaskList As New BaseGetUserGroupsbyTaskResponseTypeRow

        If oGetUserGroupsbyTask IsNot Nothing Then

            With oGetUserGroupsbyTask
                oServiceGetUserGroupsbyTaskList.UserGroupCode = .UserGroupCode
                oServiceGetUserGroupsbyTaskList.UserGroupDescription = .UserGroupDescription
                oServiceGetUserGroupsbyTaskList.UserGroupKey = .UserGroupKey
            End With
        End If

        Return oServiceGetUserGroupsbyTaskList

    End Function

    Public Shared Function ToServiceValidateBankAccountNumberList(ByVal oValidateBankAccountNumber As SFI.SAMForInsuranceV2.BaseValidateBankAccountNumberResponseTypeValidationDetailsRow) As BaseValidateBankAccountNumberResponseTypeRow

        Dim oServiceValidateBankAccountNumberList As New BaseValidateBankAccountNumberResponseTypeRow

        If oValidateBankAccountNumber IsNot Nothing Then

            With oValidateBankAccountNumber
                oServiceValidateBankAccountNumberList.AddressLine1 = .AddressLine1
                oServiceValidateBankAccountNumberList.AddressLine2 = .AddressLine2
                oServiceValidateBankAccountNumberList.AddressLine3 = .AddressLine3
                oServiceValidateBankAccountNumberList.AddressLine4 = .AddressLine4
                oServiceValidateBankAccountNumberList.BankName = .BankName
                oServiceValidateBankAccountNumberList.IsValid = .IsValid
                oServiceValidateBankAccountNumberList.IsValidationOverridable = .IsValidationOverridable
                oServiceValidateBankAccountNumberList.IsValidationOverridableSpecified = .IsValidationOverridableSpecified
                oServiceValidateBankAccountNumberList.IsValidSpecified = .IsValidSpecified
                oServiceValidateBankAccountNumberList.PostalCode = .PostalCode
                oServiceValidateBankAccountNumberList.ValidationMessageDataset = .ValidationMessageDataset
            End With
        End If

        Return oServiceValidateBankAccountNumberList

    End Function

    Public Shared Function ToBaseImpBasePostDocumentRequestType(ByVal oServicePostDocumentRequestType As BasePostDocumentRequestType) As SAMForInsuranceV2ImplementationTypes.PostDocumentRequestType
        Dim oImpPostDocumentRequestType As New SAMForInsuranceV2ImplementationTypes.PostDocumentRequestType

        If oServicePostDocumentRequestType IsNot Nothing Then

            oImpPostDocumentRequestType.BranchCode = oServicePostDocumentRequestType.BranchCode
            oImpPostDocumentRequestType.DocumentType = CType([Enum].ToObject(GetType(DocumentTypeType), oServicePostDocumentRequestType.DocumentType), BaseImplementationTypes.DocumentTypeType)
            'oImpPostDocumentRequestType.DocumentType = oServicePostDocumentRequestType.DocumentType
            oImpPostDocumentRequestType.Comment = oServicePostDocumentRequestType.Comment
            oImpPostDocumentRequestType.DocumentReference = oServicePostDocumentRequestType.DocumentReference
            oImpPostDocumentRequestType.DocumentTypeCode = oServicePostDocumentRequestType.DocumentTypeCode
            oImpPostDocumentRequestType.SAMStagingPolicyKey = oServicePostDocumentRequestType.SAMStagingPolicyKey

            If oServicePostDocumentRequestType.Transactions IsNot Nothing Then

                oImpPostDocumentRequestType.Transactions = Array.ConvertAll(
                                                            oServicePostDocumentRequestType.Transactions.ToArray(),
                                                            New Converter(Of BaseTransactionType,
                                                                BaseImplementationTypes.BaseTransactionType) _
                                                                (AddressOf ToBaseImpBaseTransactionType))

            End If
        End If

        Return oImpPostDocumentRequestType

    End Function

    Public Shared Function ToBaseImpBaseTransactionType(ByVal oServiceTransactionType As BaseTransactionType) As BaseImplementationTypes.BaseTransactionType
        Dim oImpTransactionType As BaseImplementationTypes.BaseTransactionType = New BaseImplementationTypes.BaseTransactionType

        If oServiceTransactionType IsNot Nothing Then

            oImpTransactionType.AccountCode = oServiceTransactionType.AccountCode
            oImpTransactionType.Amount = oServiceTransactionType.Amount
            oImpTransactionType.Comment = oServiceTransactionType.Comment
            oImpTransactionType.UnderwritingYearCode = oServiceTransactionType.UnderwritingYearCode
            oImpTransactionType.Reference = oServiceTransactionType.Reference
            oImpTransactionType.TransactionDate = oServiceTransactionType.TransactionDate
            oImpTransactionType.TransactionDateSpecified = oServiceTransactionType.TransactionDateSpecified
            oImpTransactionType.Username = oServiceTransactionType.Username
            oImpTransactionType.PartyKey = oServiceTransactionType.PartyKey
        End If

        Return oImpTransactionType

    End Function


    Public Shared Function ToServiceGetClaimRiskLinksResponseTypePerilType(ByVal oImpPerilType As BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilType) As BaseGetClaimRiskLinksResponseTypePerilType

        Dim oServicePerilType As New BaseGetClaimRiskLinksResponseTypePerilType

        If oImpPerilType IsNot Nothing Then

            oServicePerilType.Code = oImpPerilType.Code
            oServicePerilType.Description = oImpPerilType.Description
            oServicePerilType.SumInsured = oImpPerilType.SumInsured


            If oImpPerilType.ReserveType IsNot Nothing Then

                oServicePerilType.ReserveType = oImpPerilType.ReserveType.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilTypeReserveType, BaseGetClaimRiskLinksResponseTypePerilTypeReserveType)(AddressOf CommonFunctions.ToServiceGetClaimPerilReserveType))
            End If

            If oImpPerilType.RecoveryType IsNot Nothing Then

                oServicePerilType.RecoveryType = oImpPerilType.RecoveryType.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType, BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType)(AddressOf CommonFunctions.ToServiceGetClaimPerilRecoveryType))
            End If

        End If

        Return oServicePerilType

    End Function

    Public Shared Function ToServiceGetClaimPerilReserveType(ByVal msgClaimPerilReserveType As BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilTypeReserveType) As BaseGetClaimRiskLinksResponseTypePerilTypeReserveType
        Dim impClaimPerilReserveType As BaseGetClaimRiskLinksResponseTypePerilTypeReserveType = New BaseGetClaimRiskLinksResponseTypePerilTypeReserveType

        If msgClaimPerilReserveType IsNot Nothing Then

            impClaimPerilReserveType.Code = msgClaimPerilReserveType.Code
            impClaimPerilReserveType.Description = msgClaimPerilReserveType.Description

        End If

        Return impClaimPerilReserveType

    End Function

    Public Shared Function ToServiceGetClaimPerilRecoveryType(ByVal oImpPerilTypeRecoveryType As BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType) As BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType

        Dim oServicePerilTypeRecoveryType As New BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType

        If oImpPerilTypeRecoveryType IsNot Nothing Then

            oServicePerilTypeRecoveryType.Code = oImpPerilTypeRecoveryType.Code
            oServicePerilTypeRecoveryType.Description = oImpPerilTypeRecoveryType.Description
            oServicePerilTypeRecoveryType.IsSalvage = oImpPerilTypeRecoveryType.IsSalvage

        End If

        Return oServicePerilTypeRecoveryType

    End Function



    Public Shared Function ToBaseImpBaseClaimReceiptItemType(ByVal oService As BaseClaimReceiptItemType) As BaseImplementationTypes.BaseClaimReceiptItemType

        Dim oImplementation As BaseImplementationTypes.BaseClaimReceiptItemType = New BaseImplementationTypes.BaseClaimReceiptItemType

        If oService IsNot Nothing Then

            oImplementation.BaseRecoveryKey = oService.BaseRecoveryKey
            oImplementation.ReceiptAmount = oService.ReceiptAmount
            oImplementation.TaxGroupCode = oService.TaxGroupCode
            oImplementation.IsTaxOverridden = oService.IsTaxOverridden
            oImplementation.OverriddedTaxAmount = oService.OverriddedTaxAmount
            oImplementation.RecoveryTypeCode = oService.RecoveryTypeCode
        End If

        Return oImplementation

    End Function

    Public Shared Function ToServiceBaseGetClaimPaymentTaxesResponseTypePaymentType(
    ByVal oImplementation As BaseImplementationTypes.BaseGetClaimPaymentTaxesResponseTypePaymentItems) As BaseGetClaimPaymentTaxesResponseTypePaymentItems

        Dim oService As New BaseGetClaimPaymentTaxesResponseTypePaymentItems

        If oImplementation IsNot Nothing Then

            oService.BaseReserveKey = oImplementation.BaseReserveKey
            oService.PaymentAdjustment = Decimal.Round(oImplementation.PaymentAdjustment, 2)
            oService.PaymentAmount = Decimal.Round(oImplementation.PaymentAmount, 2)
            oService.ReverseExcess = oImplementation.ReverseExcess
            oService.TaxAmount = Decimal.Round(oImplementation.TaxAmount, 2)
            oService.TaxGroupCode = oImplementation.TaxGroupCode

        End If

        Return oService

    End Function

    Public Shared Function ToServiceBaseClaimPerilReservePaymentType(
   ByVal oImplementation As BaseImplementationTypes.BaseClaimPerilReservePaymentType) As BaseClaimPerilReservePaymentType

        Dim oService As New BaseClaimPerilReservePaymentType

        If oImplementation IsNot Nothing Then

            oService.BaseReserveKey = oImplementation.BaseReserveKey
            oService.CostToClaim = Decimal.Round(oImplementation.CostToClaim, 2)
            oService.CurrentReserve = Decimal.Round(oImplementation.CurrentReserve, 2)
            oService.PaidToDate = Decimal.Round(oImplementation.PaidToDate, 2)
            oService.PaidToDateTax = Decimal.Round(oImplementation.PaidToDateTax, 2)
            oService.ThisPaymentINCLTax = Decimal.Round(oImplementation.ThisPaymentINCLTax, 2)
            oService.ThisPaymentTax = Decimal.Round(oImplementation.ThisPaymentTax, 2)
            oService.TotalReserve = Decimal.Round(oImplementation.TotalReserve, 2)
            oService.TypeCode = oImplementation.TypeCode

        End If

        Return oService

    End Function

    Public Shared Function ToServiceBaseClaimPaymentTaxItemType(
       ByVal oImplementation As BaseImplementationTypes.BaseClaimPaymentTaxItemType) As BaseClaimPaymentTaxItemType

        Dim oService As New BaseClaimPaymentTaxItemType

        If oImplementation IsNot Nothing Then

            oService.Amount = Decimal.Round(oImplementation.Amount, 2)
            oService.Percentage = oImplementation.Percentage
            oService.ReserveType = oImplementation.ReserveType
            oService.TaxBandCode = oImplementation.TaxBandCode
            oService.TaxGroupCode = oImplementation.TaxGroupCode
            oService.ClassOfBusinessID = oImplementation.ClassOfBusinessID
            oService.IsManuallyChanges = oImplementation.IsManuallyChanges
            oService.Sequence = oImplementation.Sequence
            oService.TaxBandId = oImplementation.TaxBandId
            oService.TaxGroupId = oImplementation.TaxGroupId
        End If

        Return oService

    End Function

    Public Shared Function ToServiceBaseClaimReceiptTaxItemType(
  ByVal oImplementation As BaseImplementationTypes.BaseClaimReceiptTaxItemType) As BaseClaimReceiptTaxItemType

        Dim oService As New BaseClaimReceiptTaxItemType

        If oImplementation IsNot Nothing Then
            With oService
                .Amount = Decimal.Round(oImplementation.Amount, 2)
                .Percentage = oImplementation.Percentage
                .RecoveryType = oImplementation.RecoveryType
                .TaxBandCode = oImplementation.TaxBandCode
                .TaxGroupCode = oImplementation.TaxGroupCode
                .ClassOfBusinessID = oImplementation.ClassOfBusinessID
                .IsManuallyChanges = oImplementation.IsManuallyChanges
                .Sequence = oImplementation.Sequence
                .TaxBandId = oImplementation.TaxBandId
                .TaxGroupId = oImplementation.TaxGroupId
            End With
        End If

        Return oService

    End Function


    ''' <summary>
    ''' This method converts an error object of type SiriusFS.SAM.Structure.BaseImplementationTypes.SAMError to SiriusFS.
    ''' </summary>
    ''' <param name="oError">Error object of type SiriusFS.SAM.Structure.BaseImplementationTypes.SAMError</param>
    ''' <returns>Error object of type Sirius.Architecture.ExceptionHandling.SAMError</returns>
    ''' <remarks></remarks>

    Public Shared Function ConvertToSFIV2SAMError(ByVal oError As BaseImplementationTypes.SAMError) As SFI.SAMForInsuranceV2.WCF.SAMError

        'Dim oReturnError As New List(Of SFI.SAMForInsuranceV2.WCF.SAMError)
        Select Case oError.GetType()
            Case GetType(BaseImplementationTypes.SAMErrorInvalidData)
                Dim oSError As SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorInvalidData = CType(oError, BaseImplementationTypes.SAMErrorInvalidData)
                Dim oDError As New SFI.SAMForInsuranceV2.WCF.SAMErrorInvalidData
                oDError.Code = oSError.Code
                oDError.Description = oSError.Description
                oDError.FieldName = oSError.FieldName
                oDError.SuppliedValue = oSError.SuppliedValue
                Return oDError
            Case GetType(BaseImplementationTypes.SAMErrorBusinessRule)
                Dim oSError As SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorBusinessRule = CType(oError, BaseImplementationTypes.SAMErrorBusinessRule)
                Dim oDError As New SFI.SAMForInsuranceV2.WCF.SAMErrorBusinessRule
                oDError.Code = oSError.Code
                oDError.Description = oSError.Description
                oDError.Detail = oSError.Detail
                Return oDError
            Case GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorFatal)
                Dim osError As SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorFatal = CType(oError, BaseImplementationTypes.SAMErrorFatal)
                Dim oDError As New SFI.SAMForInsuranceV2.WCF.SAMErrorFatal
                oDError.Type = osError.Type
                Return oDError
        End Select
    End Function

    Public Shared Function ToServiceDocumentTypeList(ByVal oServiceDocumentTypeList As BaseImplementationTypes.BaseDocumentType) As BaseDocumentType

        Dim ServiceDocumentTypeList As New BaseDocumentType

        If oServiceDocumentTypeList IsNot Nothing Then
            With oServiceDocumentTypeList
                ServiceDocumentTypeList.CreateDate = .CreateDate
                ServiceDocumentTypeList.DocDescription = .DocDescription
                ServiceDocumentTypeList.DocNum = .DocNum
                ServiceDocumentTypeList.DocumentType = .DocumentType
                ServiceDocumentTypeList.FolderNum = .FolderNum
                ServiceDocumentTypeList.FolderPath = .FolderPath
            End With
        End If
        Return ServiceDocumentTypeList

    End Function

    Public Shared Function ToServiceLivePolicyVerisonDetailsList(ByVal oServiceLivePolicyVerisonDetailsList As BaseImplementationTypes.BaseLivePolicyAmountDetailsType) As BaseLivePolicyAmountDetailsType

        Dim oLivePolicyDetails As New BaseLivePolicyAmountDetailsType

        If oServiceLivePolicyVerisonDetailsList IsNot Nothing Then
            With oServiceLivePolicyVerisonDetailsList
                oLivePolicyDetails.InsuranceFileKey = .InsuranceFileKey
                oLivePolicyDetails.PaymentMethod = CType([Enum].ToObject(GetType(BaseImplementationTypes.PolicyPaymentMethod), .PaymentMethod), BaseImplementationTypes.PolicyPaymentMethod)
                oLivePolicyDetails.PolicyClientTaxesTotal = .PolicyClientTaxesTotal
                oLivePolicyDetails.PolicyFeesTotal = .PolicyFeesTotal
                oLivePolicyDetails.PolicyNonClientTaxesTotal = .PolicyNonClientTaxesTotal
                oLivePolicyDetails.RiskClientTaxesTotal = .RiskClientTaxesTotal
                oLivePolicyDetails.RiskNonClientTaxesTotal = .RiskNonClientTaxesTotal
                oLivePolicyDetails.ThisPremium = .ThisPremium
                oLivePolicyDetails.AmountCollected = .AmountCollected
                oLivePolicyDetails.PlanOutstandingAmount = .PlanOutstandingAmount
                oLivePolicyDetails.OutstandingAmount = .OutstandingAmount
                oLivePolicyDetails.TransactionAmount = .TransactionAmount
            End With
        End If
        Return oLivePolicyDetails

    End Function



    Public Shared Function ToServiceAddPartyBankStatusList(ByVal oImpList As BaseImplementationTypes.BaseAddPartyBankStatusType) As BaseAddPartyBankStatusType

        Dim oServiceList As New BaseAddPartyBankStatusType

        If oImpList IsNot Nothing Then
            With oImpList
                oServiceList.RowKey = .RowKey
                oServiceList.PartyBankKey = .PartyBankKey
                If .Errors IsNot Nothing Then
                    oServiceList.Errors = .Errors.ToList().ConvertAll(
                        New Converter(Of SiriusFS.SAM.Structure.BaseImplementationTypes.SAMError, SFI.SAMForInsuranceV2.WCF.SAMError)(AddressOf CommonFunctions.ConvertToSFIV2SAMError))
                End If

            End With
        End If
        Return oServiceList

    End Function

    Public Shared Function ToServiceUpdatePartyBankStatusList(ByVal oImpList As BaseImplementationTypes.BaseUpdatePartyBankStatusType) As BaseUpdatePartyBankStatusType

        Dim oServiceList As New BaseUpdatePartyBankStatusType

        If oImpList IsNot Nothing Then
            With oImpList
                oServiceList.RowKey = .RowKey
                oServiceList.PartyBankKey = .PartyBankKey
                If .Errors IsNot Nothing Then
                    oServiceList.Errors = .Errors.ToList().ConvertAll(
                        New Converter(Of SiriusFS.SAM.Structure.BaseImplementationTypes.SAMError, SFI.SAMForInsuranceV2.WCF.SAMError)(AddressOf CommonFunctions.ConvertToSFIV2SAMError))
                End If

            End With
        End If
        Return oServiceList

    End Function

    Public Shared Function ToServicePartyBankTypeList(ByVal oImpList As BaseImplementationTypes.BasePartyBankType) As BasePartyBankType

        Dim oServiceList As New BasePartyBankType
        Dim oBank As BaseBankType = Nothing
        Dim oBankAddress As BaseSimpleAddressType = Nothing
        Dim oCreditCard As BaseCreditCardType = Nothing
        Dim oCardHolder As BaseCreditCardTypeCardHolder = Nothing
        If oImpList IsNot Nothing Then
            With oImpList
                oServiceList.RowKey = .RowKey
                oServiceList.PartyBankKey = .PartyBankKey
                oServiceList.BankPaymentTypeCode = .BankPaymentTypeCode
                oServiceList.AccountHolderName = .AccountHolderName
                oServiceList.AccountKey = .AccountKey
                oServiceList.AccountType = .AccountType
                oServiceList.IsBankItem = .IsBankItem
                oServiceList.IsDeleted = .IsDeleted
                oServiceList.IsPartyBankInUse = .IsPartyBankInUse
                oServiceList.IsPartyBankLinkedWithInst = .IsPartyBankLinkedWithInst

                'Bank Details
                If (.Bank IsNot Nothing) Then
                    oBank = New BaseBankType
                    With .Bank
                        oBank.AccountNumber = .AccountNumber
                        oBank.BankCode = .BankCode
                        oBank.Branch = .Branch
                        oBank.BranchCode = .BranchCode
                        oBank.BankName = .BankName
                        oBank.BIC = .BIC
                        oBank.IBAN = .IBAN
                        'Bank Address Details
                        If .BankAddress IsNot Nothing Then
                            oBankAddress = New BaseSimpleAddressType
                            With .BankAddress
                                oBankAddress.AddressLine1 = .AddressLine1
                                oBankAddress.AddressLine2 = .AddressLine2
                                oBankAddress.AddressLine3 = .AddressLine3
                                oBankAddress.AddressLine4 = .AddressLine4
                                oBankAddress.PostCode = .PostCode
                                oBankAddress.CountryCode = .CountryCode
                            End With
                            oBank.BankAddress = oBankAddress
                            oBankAddress = Nothing
                        End If
                    End With
                    oServiceList.Bank = oBank
                    oBank = Nothing
                End If

                'Credit Card Details
                If (.CreditCard IsNot Nothing) Then
                    oCreditCard = New BaseCreditCardType
                    With .CreditCard
                        oCreditCard.Number = .Number
                        oCreditCard.StartDate = .StartDate
                        oCreditCard.ExpiryDate = .ExpiryDate
                        oCreditCard.NameOnCreditCard = .NameOnCreditCard
                        oCreditCard.Issue = .Issue
                        oCreditCard.Pin = .Pin
                        oCreditCard.IsRegisteredCardHolder = .IsRegisteredCardHolder
                        oCreditCard.ManualAuthCode = .ManualAuthCode
                        oCreditCard.TrackingNumber = .TrackingNumber
                        oCreditCard.IsDefaultCreditCard = .IsDefaultCreditCard

                        'CardHolder Address Details
                        If .CardHolder IsNot Nothing Then
                            oCardHolder = New BaseCreditCardTypeCardHolder
                            With .CardHolder
                                oCardHolder.AddressLine1 = .AddressLine1
                                oCardHolder.AddressLine2 = .AddressLine2
                                oCardHolder.AddressLine3 = .AddressLine3
                                oCardHolder.AddressLine4 = .AddressLine4
                                oCardHolder.PostCode = .PostCode
                                oCardHolder.CountryCode = .CountryCode
                            End With
                            oCreditCard.CardHolder = oCardHolder
                            oCardHolder = Nothing
                        End If
                    End With
                    oServiceList.CreditCard = oCreditCard
                    oCreditCard = Nothing
                End If

                'History Items
                If .History IsNot Nothing Then
                    oServiceList.History = .History.ToList().ConvertAll(
                      New Converter(Of SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyBankHistoryType, SFI.SAMForInsuranceV2.WCF.BasePartyBankHistoryType)(AddressOf CommonFunctions.ToServicePartyBankHistoryTypeList))
                End If

            End With
        End If
        Return oServiceList

    End Function

    Public Shared Function ToServicePartyBankHistoryTypeList(ByVal oImpList As BaseImplementationTypes.BasePartyBankHistoryType) As BasePartyBankHistoryType

        Dim oPartyBankHistoryItem As New BasePartyBankHistoryType

        If oImpList IsNot Nothing Then
            With oImpList
                oPartyBankHistoryItem.PartyBankKey = .PartyBankKey
                oPartyBankHistoryItem.AccountHolderName = .AccountHolderName
                oPartyBankHistoryItem.AccountNumber = .AccountNumber
                oPartyBankHistoryItem.ActionCode = .ActionCode
                oPartyBankHistoryItem.AccountType = .AccountType
                oPartyBankHistoryItem.BankBranchCode = .BankBranchCode
                oPartyBankHistoryItem.BankName = .BankName
                oPartyBankHistoryItem.DateModified = .DateModified
                oPartyBankHistoryItem.PostCode = .PostCode
                oPartyBankHistoryItem.StreetName = .StreetName
                oPartyBankHistoryItem.UserName = .UserName
                oPartyBankHistoryItem.BankBranch = .BankBranch
                oPartyBankHistoryItem.BIC = .BIC
                oPartyBankHistoryItem.IBAN = .IBAN
            End With
        End If
        Return oPartyBankHistoryItem

    End Function




    Public Shared Function ToServiceClaimRiskRIArrangementLineTypeList(ByVal oImpList As BaseImplementationTypes.BaseClaimRiskRIArrangementLineType) As BaseClaimRiskRIArrangementLineType

        Dim oService As New BaseClaimRiskRIArrangementLineType

        If oImpList IsNot Nothing Then
            With oImpList
                oService.ActionType = RowAction.EditRow
                oService.RIArrangementKey = .RIArrangementKey
                oService.RIArrangementLineKey = .RIArrangementLineKey
                oService.RIPlacement = .RIPlacement
                oService.RIName = .RIName
                oService.Type = .Type
                oService.Retained = .Retained * 100
                oService.RetainedSpecified = .RetainedSpecified
                oService.DefaultSharePercent = .DefaultSharePercent
                oService.ThisSharePercent = .ThisSharePercent
                oService.LowerLimit = .LowerLimit
                oService.LineLimit = .LineLimit
                oService.SumInsured = .SumInsured
                oService.AgreementCode = .AgreementCode
                oService.IsDomiciledForTax = .IsDomiciledForTax
                oService.Grouping = .Grouping
                oService.IsRIBroker = .IsRIBroker
                oService.ReinsuranceTypeCode = .ReinsuranceTypeCode
                oService.TreatyCode = .TreatyCode
                oService.PartyKey = .PartyKey
                oService.Priority = .Priority
                oService.NumberOfLines = .NumberOfLines
                oService.Incurred = .Incurred
                oService.ReserveToDate = .ReserveToDate
                oService.ThisReserve = .ThisReserve
                oService.PaymentToDate = .PaymentToDate
                oService.ThisPayment = .ThisPayment
                oService.RecoverToDate = .RecoverToDate
                oService.Balance = .Balance
                oService.CedePremiumOnly = .CedePremiumOnly
                oService.IsObligatory = .IsObligatory

                If (.BrokerParticipants IsNot Nothing) Then
                    oService.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf CommonFunctions.ToServiceBaseBrokerParticipants))
                End If

                If (.FAXParticipants IsNot Nothing) Then
                    oService.FAXParticipants = .FAXParticipants.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClaimFAXParticipants, BaseClaimFAXParticipants)(AddressOf CommonFunctions.ToServiceBaseFAXBrokerParticipants))
                End If

            End With
        End If
        Return oService

    End Function

    Public Shared Function ToServiceBaseBrokerParticipants(ByVal oImpList As BaseImplementationTypes.BaseBrokerParticipants) As BaseBrokerParticipants

        Dim oservice As New BaseBrokerParticipants

        If oImpList IsNot Nothing Then
            With oImpList
                oservice.PartyKey = .PartyKey
                oservice.PartyCode = .PartyCode
                oservice.PartyName = .PartyName
                oservice.ParticpationPercentage = .ParticpationPercentage
            End With
        End If
        Return oservice

    End Function

    Public Shared Function ToServiceBaseFAXBrokerParticipants(ByVal oImpList As BaseImplementationTypes.BaseClaimFAXParticipants) As BaseClaimFAXParticipants

        Dim oservice As New BaseClaimFAXParticipants

        If oImpList IsNot Nothing Then
            With oImpList
                oservice.ActionType = RowAction.EditRow
                oservice.RIArrangementLineKey = .RIArrangementLineKey
                oservice.PartyKey = .PartyKey
                oservice.PartyCode = .PartyCode
                oservice.PartyName = .PartyName
                oservice.AccountType = .AccountType
                oservice.ParticpationPercentage = .ParticpationPercentage
                oservice.SumInsured = .SumInsured
                oservice.AgreementCode = .AgreementCode
                oservice.ReserveToDate = .ReserveToDate
                oservice.ThisReserve = .ThisReserve
                oservice.PaymentToDate = .PaymentToDate
                oservice.ThisPayment = .ThisPayment
                oservice.RecoverToDate = .RecoverToDate
                If (.BrokerParticipants IsNot Nothing) Then
                    oservice.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf CommonFunctions.ToServiceBaseBrokerParticipants))
                End If
            End With
        End If
        Return oservice

    End Function



    Public Shared Function ToServiceRiskRIArrangementLineTypeList(ByVal oImpList As BaseImplementationTypes.BaseRiskRIArrangementLineType) As BaseRiskRIArrangementLineType

        Dim oService As New BaseRiskRIArrangementLineType

        If oImpList IsNot Nothing Then
            With oImpList
                oService.RIArrangementKey = .RIArrangementKey
                oService.RIArrangementLineKey = .RIArrangementLineKey
                oService.RIPlacement = .RIPlacement
                oService.RIName = .RIName
                oService.Retained = .Retained
                oService.DefaultSharePercent = .DefaultSharePercent * 100
                oService.ThisSharePercent = .ThisSharePercent * 100
                oService.LowerLimit = .LowerLimit
                oService.LowerLimitSpecified = True
                oService.LineLimit = .LineLimit
                oService.SumInsured = .SumInsured
                oService.PremiumValue = .PremiumValue
                oService.PremiumTax = .PremiumTax
                oService.PremiumTaxSpecified = True
                oService.CommissionTax = .CommissionTax
                oService.CommissionTaxSpecified = True
                oService.CommissionPercent = .CommissionPercent * 100
                oService.CommissionValue = .CommissionValue
                oService.AgreementCode = .AgreementCode
                oService.IsDomiciledForTax = .IsDomiciledForTax
                oService.Grouping = .Grouping
                oService.GroupingSpecified = True
                oService.IsRIBroker = .IsRIBroker
                oService.ReinsuranceTypeCode = .ReinsuranceTypeCode
                oService.TreatyCode = .TreatyCode
                oService.PartyKey = .PartyKey
                oService.PartyKeySpecified = True
                oService.Priority = .Priority
                oService.NumberOfLines = .NumberOfLines
                oService.PremiumPercent = .PremiumPercent * 100
                oService.IsCommissionModified = .IsCommissionModified
                oService.CedePremiumOnly = .CedePremiumOnly
                oService.ActionType = CType(.ActionType, RowAction)
                oService.Type = .Type
                oService.ParticipationPercent = .ParticipationPercent
                oService.ParticipationPercentSpecified = True
                oService.IsObligatory = .IsObligatory

                If (.BrokerParticipants IsNot Nothing) Then
                    oService.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf CommonFunctions.ToServiceBaseBrokerParticipants))
                End If

                If (.FAXParticipants IsNot Nothing) Then
                    oService.FAXParticipants = .FAXParticipants.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseFAXParticipants, BaseFAXParticipants)(AddressOf CommonFunctions.ToServiceBaseRiskFAXBrokerParticipants))
                End If

            End With
        End If
        Return oService

    End Function

    Public Shared Function ToServiceBaseRiskFAXBrokerParticipants(ByVal oImpList As BaseImplementationTypes.BaseFAXParticipants) As BaseFAXParticipants

        Dim oservice As New BaseFAXParticipants

        If oImpList IsNot Nothing Then
            With oImpList
                oservice.RIArrangementLineKey = .RIArrangementLineKey
                oservice.PartyKey = .PartyKey
                oservice.PartyCode = .PartyCode
                oservice.PartyName = .PartyName
                oservice.AccountType = .AccountType
                oservice.ParticpationPercentage = .ParticpationPercentage
                oservice.SumInsured = .SumInsured
                oservice.PremiumValue = .PremiumValue
                oservice.PremiumTax = .PremiumTax
                oservice.PremiumTaxSpecified = True
                oservice.CommissionPercent = .CommissionPercent
                oservice.CommissionTax = .CommissionTax
                oservice.CommissionTaxSpecified = True
                oservice.CommissionValue = .CommissionValue
                oservice.AgreementCode = .AgreementCode
                oservice.ActionType = CType(.ActionType, RowAction)

                If (.BrokerParticipants IsNot Nothing) Then
                    oservice.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf CommonFunctions.ToServiceBaseBrokerParticipants))
                End If
            End With
        End If
        Return oservice

    End Function

    Public Shared Function ToServiceBaseGetClaimReceiptTaxesResponseTypeReceiptItemsType(
  ByVal oImplementation As BaseImplementationTypes.BaseGetClaimReceiptTaxesResponseTypeReceiptItems) As BaseGetClaimReceiptTaxesResponseTypeReceiptItems

        Dim oService As New BaseGetClaimReceiptTaxesResponseTypeReceiptItems

        If oImplementation IsNot Nothing Then
            With oImplementation
                oService.BaseRecoveryKey = .BaseRecoveryKey
                oService.ReceiptAmount = .ReceiptAmount
                oService.TaxAmount = .TaxAmount
                oService.TaxGroupCode = .TaxGroupCode
            End With
        End If

        Return oService

    End Function
    Public Shared Function ToServiceBaseClaimPerilRecoveryReceiptType(
 ByVal oImplementation As BaseImplementationTypes.BaseClaimPerilRecoveryReceiptType) As BaseClaimPerilRecoveryReceiptType

        Dim oService As New BaseClaimPerilRecoveryReceiptType

        If oImplementation IsNot Nothing Then
            With oImplementation
                oService.BaseRecoveryKey = .BaseRecoveryKey
                oService.TotalRecoveryAmount = .TotalRecoveryAmount
                oService.TotalReceiptAmount = .TotalReceiptAmount
                oService.ThisReceiptINCLTaxAmount = .ThisReceiptINCLTaxAmount
                oService.ThisReceiptTaxAmount = .ThisReceiptTaxAmount
                oService.ThisReceiptNetAmount = .ThisReceiptNetAmount
                oService.BalanceAmount = .BalanceAmount
            End With
        End If

        Return oService

    End Function
    Public Shared Function ToServiceClaimReceipttaxItemType(ByVal oImplementation As BaseImplementationTypes.BaseClaimReceiptTaxItemType) As BaseClaimReceiptTaxItemType

        Dim oService As New BaseClaimReceiptTaxItemType

        If oImplementation IsNot Nothing Then
            With oImplementation

                oService.Amount = .Amount
                oService.Percentage = .Percentage
                oService.RecoveryType = .RecoveryType
                oService.TaxBandCode = .TaxBandCode
                oService.TaxGroupCode = .TaxGroupCode

            End With
        End If

        Return oService

    End Function

    Public Shared Function ToServiceSettleAllClaimPaymentsSummaryList(ByVal oBaseSettleAllClaimPaymentSummaryResponseType As BaseImplementationTypes.BaseSettleAllClaimPaymentSummaryResponseType) As BaseSettleAllClaimPaymentSummaryResponseType

        Dim serviceSettleAllClaimPaymentSummaryResponseType As New BaseSettleAllClaimPaymentSummaryResponseType
        If oBaseSettleAllClaimPaymentSummaryResponseType IsNot Nothing Then
            With oBaseSettleAllClaimPaymentSummaryResponseType
                serviceSettleAllClaimPaymentSummaryResponseType.Amount = .Amount
                serviceSettleAllClaimPaymentSummaryResponseType.MediaTypeCode = .MediaTypeCode
                serviceSettleAllClaimPaymentSummaryResponseType.NoOfTransactions = .NoOfTransactions
                serviceSettleAllClaimPaymentSummaryResponseType.StatusOfTransaction = .StatusOfTransaction
            End With
        End If
        Return serviceSettleAllClaimPaymentSummaryResponseType

    End Function

    Public Shared Function ToServiceGetProductsForUserBranchList(ByVal oBaseSettleAllClaimPaymentSummaryResponseType As BaseImplementationTypes.BaseGetProductsForUserBranchResponseTypeProductsRow) As BaseGetProductsForUserBranchResponseTypeProductsRow

        Dim oService As New BaseGetProductsForUserBranchResponseTypeProductsRow
        If oBaseSettleAllClaimPaymentSummaryResponseType IsNot Nothing Then
            With oBaseSettleAllClaimPaymentSummaryResponseType
                oService.ProductCode = .ProductCode
                oService.ProductDescription = .ProductDescription
                oService.ProductKey = .ProductKey

                oService.Branches = .Branches.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseBranchType, BaseBranchType)(AddressOf ToServiceSourceList))

            End With
        End If
        Return oService

    End Function
#Region "Claim Data Import- Claim Payee Address Type"
    ''' <summary>
    ''' Convert Service claim payee address type into internal type
    ''' </summary>
    ''' <param name="oMsgAddress"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseClaimPayeeAddressType(ByVal oMsgAddress As BaseAddressType) As BaseImplementationTypes.BaseAddressType

        Dim oImpAddress As BaseImplementationTypes.BaseAddressType = New BaseImplementationTypes.BaseAddressType

        If oMsgAddress IsNot Nothing Then
            With oMsgAddress
                oImpAddress.AddressLine1 = .AddressLine1
                oImpAddress.AddressLine2 = .AddressLine2
                oImpAddress.AddressLine3 = .AddressLine3
                oImpAddress.AddressLine4 = .AddressLine4
                oImpAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), .AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                oImpAddress.CountryCode = .CountryCode
                oImpAddress.PostCode = .PostCode
            End With
        End If

        Return oImpAddress

    End Function
#End Region


    Public Shared Function ToServiceDmeFolderList(ByVal oImpList As BaseImplementationTypes.BaseDMEFolderType) As BaseDMEFolderType

        Dim oService As New BaseDMEFolderType

        If oImpList IsNot Nothing Then
            With oImpList
                oService.FolderNum = .FolderNum
                oService.Name = .Name
                oService.ParentNum = .ParentNum
                oService.FolderLevel = .FolderLevel
                oService.ExternalCode = .ExternalCode
                oService.CreateDate = .CreateDate
            End With
        End If

        Return oService

    End Function

    Public Shared Function ToServiceDocumentList(ByVal oImpList As BaseImplementationTypes.BaseDocumentType) As BaseDocumentType

        Dim oService As New BaseDocumentType

        If oImpList IsNot Nothing Then
            With oImpList
                oService.DocDescription = .DocDescription
                oService.DocNum = .DocNum
                oService.CreateDate = .CreateDate
                oService.FolderNum = .FolderNum
                oService.FolderPath = .FolderPath
                oService.DocumentType = CType(.DocumentType, DMEDocType)
            End With
        End If

        Return oService

    End Function

    'User Story 24 (Changes also done in Shared Files\gPMFunctions.vb)
    Public Overloads Shared Function CreateDictionary(sParams() As String) As Dictionary(Of String, Object)
        Dim cDicParams As New Dictionary(Of String, Object)
        Try
            For Each sParam As String In sParams
                If sParam.Contains("=") Then
                    cDicParams.Add(sParam.Split("=")(0).Trim, sParam.Split("=")(1).Trim)
                End If
            Next
        Catch
            cDicParams.Clear()
        End Try
        Return cDicParams
    End Function

    Public Overloads Shared Function CreateDictionary(sParams As String) As Dictionary(Of String, Object)
        Return CreateDictionary(sParams, ",")
    End Function

    Public Overloads Shared Function CreateDictionary(sParams As String, chDelimiter As Char) As Dictionary(Of String, Object)
        Dim cDicParams As New Dictionary(Of String, Object)
        Try
            For Each sParam As String In sParams.Split(chDelimiter)
                If sParam.Contains("=") Then
                    cDicParams.Add(sParam.Split("=")(0).Trim, sParam.Split("=")(1).Trim)
                End If
            Next
        Catch
            cDicParams.Clear()
        End Try
        Return cDicParams
    End Function

    Public Overloads Shared Function CreateDictionary(ByVal obj As Object) As Dictionary(Of String, Object)
        Dim cDicParams As New Dictionary(Of String, Object)
        Dim objAttrbList As Object = New gPMConstants.LOG_FIELDS_LIST

        Try
            'Add valid fields to the Dictionary as specified in LOG_FIELDS_LIST
            For Each field As FieldInfo In obj.GetType().GetFields()
                Dim bToBeAdded As Boolean = False
                For Each constAttrb In objAttrbList.GetType().GetFields()
                    If field.Name.ToLower.EndsWith(constAttrb.GetValue(objAttrbList)) Then
                        bToBeAdded = True
                        Exit For
                    End If
                Next

                If bToBeAdded Then
                    If field.GetValue(obj) IsNot Nothing Then
                        cDicParams.Add(field.Name, field.GetValue(obj).ToString())
                    Else
                        cDicParams.Add(field.Name, "")
                    End If
                End If
            Next

            'Add valid properties to the Dictionary as specified in LOG_FIELDS_LIST
            For Each prop As PropertyInfo In obj.GetType().GetProperties
                Dim bToBeAdded As Boolean = False
                For Each constAttrb In objAttrbList.GetType().GetFields()
                    If prop.Name.ToLower.EndsWith(constAttrb.GetValue(objAttrbList)) Then
                        bToBeAdded = True
                        Exit For
                    End If
                Next

                If bToBeAdded Then
                    If prop.GetValue(obj, Nothing) IsNot Nothing Then
                        cDicParams.Add(prop.Name, prop.GetValue(obj, Nothing).ToString())
                    Else
                        cDicParams.Add(prop.Name, "")
                    End If
                End If
            Next
        Catch
            cDicParams.Clear()
        Finally
            objAttrbList = Nothing
        End Try
        Return cDicParams
    End Function

End Class
