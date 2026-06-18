Imports System.Configuration.Provider
Imports System.Reflection
Imports System.ServiceModel
Imports System.Text
Imports System.Xml
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports SSP.PureInsuranceRestAPIHandler
Imports SSP.PureInsuranceRestAPIHandler.BaseClasses
Imports SSP.PureInsuranceRestAPIHandler.Enums
Partial Public Class ProviderSAMForInsuranceV2 : Inherits NexusProvider.ProviderBase

    Public Overrides Sub ActivatePartyBankDetails(ByVal v_iPartyBankKey As Integer,
                                      ByVal vPartyBankDetails As BankCollection,
                                      Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oActivatePartyBankRequest As BaseClasses.BaseActivatePartyBankRequestType
            Dim oActivatePartyBankResponse As BaseClasses.BaseActivatePartyBankResponseType
            Dim oBankDetails As BaseClasses.BaseActivatePartyBankRequestTypeRow
            Dim sbLogMessage As StringBuilder

            Try
                oActivatePartyBankRequest = New BaseClasses.BaseActivatePartyBankRequestType
                oActivatePartyBankResponse = New BaseClasses.BaseActivatePartyBankResponseType
                sbLogMessage = New StringBuilder


                With oActivatePartyBankRequest
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If vPartyBankDetails IsNot Nothing AndAlso vPartyBankDetails.Count > 0 Then
                        .PartBankDetails = New List(Of BaseClasses.BaseActivatePartyBankRequestTypeRow)
                        For iCount As Integer = 0 To vPartyBankDetails.Count - 1
                            oBankDetails = New BaseClasses.BaseActivatePartyBankRequestTypeRow
                            oBankDetails.PartyBankKey = vPartyBankDetails.Item(iCount).PartyBankKey
                            oBankDetails.MakeActive = vPartyBankDetails.Item(iCount).IsActive
                            .PartBankDetails.Add(oBankDetails)
                        Next

                    Else
                        Throw New ArgumentNullException("vPartyBankDetails")
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim fullUrl = $"/parties/activateBankDetails"
                    Dim result As String = ApiClient.Post(ApiMethods.ActivateBankDetails, oActivatePartyBankRequest)
                    oActivatePartyBankResponse = ApiClient.DeserializeJson(Of BaseClasses.BaseActivatePartyBankResponseType)(result)
                End Using

                With oActivatePartyBankResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("ActivatePartyBankDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("r_oAddress = " & r_oAddress.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oActivatePartyBankRequest = Nothing
                oActivatePartyBankResponse = Nothing
            End Try

        End SyncLock
    End Sub

    Public Overrides Sub AddParty(ByRef r_oParty As BaseParty,
                                    Optional ByVal v_sBranchCode As String = Nothing,
                                    Optional ByVal v_sSubBranchCode As String = Nothing)

        SyncLock oLock

            Dim oAddPartyRequest As BaseClasses.AddPartyCommand
            Dim oAddPartyResponse As BaseClasses.AddPartyCommandResponse
            Dim oAddress As NexusProvider.Address = r_oParty.Addresses.Item(AddressType.CorrespondenceAddress)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oCurrencyColl As NexusProvider.CurrencyCollection
            Dim iContactCount As Integer
            Dim sbLogMessage As StringBuilder

            Try
                oAddPartyRequest = New BaseClasses.AddPartyCommand
                oAddPartyResponse = New BaseClasses.AddPartyCommandResponse
                sbLogMessage = New StringBuilder

                With oAddPartyRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .SubBranchCode = v_sSubBranchCode

                    If r_oParty Is Nothing Then
                        Throw New ArgumentNullException("Party")
                    Else
                        If r_oParty.Addresses Is Nothing Then
                            Throw New ArgumentNullException("Party.Address")
                        Else

                            If oAddress Is Nothing Then
                                Throw New ArgumentException("Party must contain an address of type Correspondence Address", "Party.Address")
                            End If
                        End If
                        If TypeOf r_oParty Is PersonalParty Then
                            .BasePartyPCType = ConvertPartyPC(r_oParty)
                            If String.IsNullOrEmpty(.BasePartyPCType.Currency) Then
                                'Currency will be populated with BaseCurrency set in Bo
                                oCurrencyColl = oWebService.GetCurrenciesByBranch(.BasePartyPCType.BranchCode)
                                .BasePartyPCType.Currency = oCurrencyColl(0).BaseCurrencyCode
                            End If
                            iContactCount = .BasePartyPCType.Contacts.Count
                            If iContactCount >= 0 Then
                                For iContactCount = 0 To iContactCount - 1
                                    If DirectCast(DirectCast(.BasePartyPCType, BaseClasses.BasePartyType).Contacts(iContactCount).ContactDetail, BaseClasses.BaseContactDetailType).Item = "" Then
                                        .BasePartyPCType.Contacts(iContactCount) = Nothing
                                    End If
                                Next
                            End If
                        ElseIf TypeOf r_oParty Is CorporateParty Then
                            .BasePartyCCType = ConvertPartyCC(r_oParty)
                            If String.IsNullOrEmpty(.BasePartyCCType.Currency) Then
                                'Currency will be populated with BaseCurrency set in Bo
                                oCurrencyColl = oWebService.GetCurrenciesByBranch(.BasePartyCCType.BranchCode)
                                .BasePartyCCType.Currency = oCurrencyColl(0).BaseCurrencyCode
                            End If
                            iContactCount = .BasePartyCCType.Contacts.Count
                            If iContactCount >= 0 Then
                                For iContactCount = 0 To iContactCount - 1
                                    If DirectCast(DirectCast(.BasePartyCCType, BaseClasses.BasePartyType).Contacts(iContactCount).ContactDetail, BaseClasses.BaseContactDetailType).Item = "" Then
                                        .BasePartyCCType.Contacts(iContactCount) = Nothing
                                    End If
                                Next
                            End If
                        ElseIf TypeOf r_oParty Is OtherParty Then
                            .BasePartyOTHERType = ConvertPartyOther(r_oParty)
                            .BasePartyOTHERType.BranchCode = .BranchCode
                            .BasePartyOTHERType.SubBranchCode = .SubBranchCode
                            If String.IsNullOrEmpty(.BasePartyOTHERType.Currency) Then
                                'Currency will be populated with BaseCurrency set in Bo
                                oCurrencyColl = oWebService.GetCurrenciesByBranch(.BasePartyOTHERType.BranchCode)
                                .BasePartyOTHERType.Currency = oCurrencyColl(0).BaseCurrencyCode
                            End If
                            iContactCount = .BasePartyOTHERType.Contacts.Count
                            If iContactCount >= 0 Then
                                For iContactCount = 0 To iContactCount - 1
                                    If DirectCast(DirectCast(.BasePartyOTHERType, BaseClasses.BasePartyType).Contacts(iContactCount).ContactDetail, BaseClasses.BaseContactDetailType).Item = "" Then
                                        .BasePartyOTHERType.Contacts(iContactCount) = Nothing
                                    End If
                                Next
                            End If
                        End If
                    End If
                    'Added following code to fix issue 3344(It will assign contact item to nothing if Telephone ,email is blank in Nexus)


                End With
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.AddParty, oAddPartyRequest)
                    oAddPartyResponse = ApiClient.DeserializeJson(Of AddPartyCommandResponse)(result)
                End Using

                With oAddPartyResponse.AddPartyResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oParty.Key = .PartyKey
                        r_oParty.ResolvedName = .ResolvedName
                        r_oParty.UserName = .Shortname
                        r_oParty.TimeStamp = .PartyTimestamp
                        r_oParty.XMLDataset = .XMLDataset
                    End If

                End With
                If Logger.IsLoggingEnabled Then

                    sbLogMessage.AppendLine("AddParty executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oParty = " & r_oParty.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sSubBranchCode) Then
                        sbLogMessage.AppendLine("v_sSubBranchCode = " & v_sSubBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sSubBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oAddPartyRequest = Nothing
                oAddPartyResponse = Nothing
            End Try

        End SyncLock

    End Sub
    Private Function ConvertPartyOther(ByVal r_oParty As BaseParty) As BaseClasses.BasePartyType

        Dim oParty As BaseClasses.BasePartyType = Nothing
        If TypeOf r_oParty Is OtherParty Then
            oParty = New BasePartyOtherType

            With DirectCast(oParty, BasePartyOtherType)

                .Name = DirectCast(r_oParty, OtherParty).Name
                .Code = DirectCast(r_oParty, OtherParty).Code
                .Currency = DirectCast(r_oParty, OtherParty).Currency
                .BranchCode = DirectCast(r_oParty, OtherParty).BranchCode
                .SubBranchCode = DirectCast(r_oParty, OtherParty).SubBranchCode
                .DateOfBirth = DirectCast(r_oParty, OtherParty).DateOfBirth
                .FileCode = DirectCast(r_oParty, OtherParty).FileCode
                .Gender = DirectCast(r_oParty, OtherParty).Gender
                .LicenseTypeCode = DirectCast(r_oParty, OtherParty).LicenseTypeCode
                .LicenseNumber = DirectCast(r_oParty, OtherParty).LicenseNumber
                .RegNumber = DirectCast(r_oParty, OtherParty).RegistrationNumber
                .DriverStatusCode = DirectCast(r_oParty, OtherParty).DriverStatusCode
                .IsTPASettleDirectly = DirectCast(r_oParty, OtherParty).IsTPASettleDirectly
                .TaxNumber = DirectCast(r_oParty, OtherParty).TaxNumber
                .DomiciledForTax = DirectCast(r_oParty, OtherParty).DomiciledForTax
                .TypeCode = DirectCast(r_oParty, OtherParty).TypeCode
                If DirectCast(r_oParty, OtherParty).TaxExemptSpecified Then
                    .TaxExempt = DirectCast(r_oParty, OtherParty).TaxExempt
                End If
                .TaxExemptSpecified = DirectCast(r_oParty, OtherParty).TaxExemptSpecified
                If DirectCast(r_oParty, OtherParty).TaxPercentageSpecified Then
                    .TaxPercentage = DirectCast(r_oParty, OtherParty).TaxPercentage
                End If
                .TaxPercentageSpecified = DirectCast(r_oParty, OtherParty).TaxPercentageSpecified

                .XMLDataset = DirectCast(r_oParty, OtherParty).XMLDataset

                'Convictions
                If r_oParty.Conviction IsNot Nothing AndAlso r_oParty.Conviction.Count > 0 Then
                    .Convictions = New List(Of BaseConvictionType)
                    For iCnt As Integer = 0 To r_oParty.Conviction.Count - 1
                        Dim oConviction As New BaseConvictionType

                        With r_oParty.Conviction(iCnt)
                            oConviction.ConvictionKey = .ConvictionKey
                            oConviction.Description = .Description
                            oConviction.TypeCode = .TypeCode
                            oConviction.Date = .ConvictionDate
                            If .AlcoholLevel > 0 Then
                                oConviction.AlcoholLevel = .AlcoholLevel
                                oConviction.AlcoholLevelSpecified = True
                            Else
                                oConviction.AlcoholLevelSpecified = False
                            End If
                            oConviction.AlcoholMeasurementMethod = .AlcoholMeasurementMethod

                            If .DrivingLicensePenaltyPoints > 0 Then
                                oConviction.DrivingLicensePenaltyPoints = .DrivingLicensePenaltyPoints
                                oConviction.DrivingLicensePenaltyPointsSpecified = True
                            Else
                                oConviction.DrivingLicensePenaltyPointsSpecified = False
                            End If

                            If .FineAmount > 0 Then
                                oConviction.FineAmount = .FineAmount
                                oConviction.FineAmountSpecified = True
                            Else
                                oConviction.FineAmountSpecified = False
                            End If

                            oConviction.SentenceDescription = .SentenceDescription

                            If .SentenceDuration > 0 Then
                                oConviction.SentenceDuration = .SentenceDuration
                                oConviction.SentenceDurationSpecified = True
                            Else
                                oConviction.SentenceDurationSpecified = False
                            End If

                            ' oConviction.SentenceDurationQualifier = .SentenceDurationQualifier

                            If .SentenceEffectiveDate <> Date.MinValue Then
                                oConviction.SentenceEffectiveDate = .SentenceEffectiveDate
                                oConviction.SentenceEffectiveDateSpecified = True
                            Else
                                oConviction.SentenceEffectiveDateSpecified = False
                            End If

                            oConviction.SentenceTypeCode = .SentenceTypeCode
                            oConviction.StatusCode = .StatusCode
                        End With
                        .Convictions.Add(oConviction)
                    Next
                End If

                'Accidents
                If r_oParty.Accidents IsNot Nothing AndAlso r_oParty.Accidents.Count > 0 Then
                    .Accident = New List(Of BasePartyOtherTypeAccident)
                    For iCnt As Integer = 0 To r_oParty.Accidents.Count - 1
                        Dim oAccident As New BasePartyOtherTypeAccident
                        With r_oParty.Accidents(iCnt)
                            oAccident.AccidentKey = .AccidentKey
                            oAccident.Date = .AccidentDate
                            oAccident.Description = .Description
                            oAccident.IsAtFault = .IsAtFault
                        End With
                        .Accident.Add(oAccident)
                    Next
                End If

                'Supplier Bussiness
                If r_oParty.SupplierBusinesses IsNot Nothing AndAlso r_oParty.SupplierBusinesses.Count > 0 Then
                    .SupplierBusiness = New List(Of BasePartyOtherTypeSupplierBusiness)
                    For iCnt As Integer = 0 To r_oParty.SupplierBusinesses.Count - 1
                        Dim oSupplies As New BasePartyOtherTypeSupplierBusiness
                        With r_oParty.SupplierBusinesses(iCnt)
                            oSupplies.BusinessCode = .BusinessCode
                            oSupplies.SpecialityCode = .SpecialityCode
                            'oSupplies.ExtensionData = .
                        End With
                        .SupplierBusiness.Add(oSupplies)
                    Next
                End If

                'Branches
                If DirectCast(r_oParty, OtherParty).Branches IsNot Nothing AndAlso DirectCast(r_oParty, OtherParty).Branches.Count > 0 Then
                    .Branches = New List(Of BasePartyOtherTypeBranch)
                    For iCnt As Integer = 0 To DirectCast(r_oParty, OtherParty).Branches.Count - 1
                        Dim oBranch As New BasePartyOtherTypeBranch
                        With DirectCast(r_oParty, OtherParty).Branches(iCnt)
                            oBranch.BranchId = .BranchKey
                            oBranch.Description = .Description
                        End With
                        .Branches.Add(oBranch)
                    Next
                End If

            End With
        End If

        oParty = ConvertPartyCommon(r_oParty, Nothing, Nothing, oParty)

        Return oParty

    End Function
    Private Function ConvertPartyCommon(ByVal r_oParty As BaseParty, Optional ByRef oPartyPC As BasePartyPCType = Nothing, Optional ByRef oPartyCC As BasePartyCCType = Nothing, Optional ByRef oPartyOther As BasePartyOtherType = Nothing)
        Dim oParty As BasePartyType = Nothing
        If oPartyPC IsNot Nothing Then
            oParty = oPartyPC
        ElseIf oPartyCC IsNot Nothing Then
            oParty = oPartyCC
        ElseIf oPartyOther Is Nothing Then
            oParty = oPartyOther
        End If

        With oParty

            .AccountExecutive = r_oParty.AccountExecutive
            .AccountExecutiveCode = r_oParty.AccountExecutiveCode
            .Currency = r_oParty.Currency

            If r_oParty.DomiciledForTaxSpecified Then
                .DomiciledForTax = r_oParty.DomiciledForTax
            End If

            .DomiciledForTaxSpecified = r_oParty.DomiciledForTaxSpecified
            .FileCode = r_oParty.FileCode

            If r_oParty.TaxExemptSpecified Then
                .TaxExempt = r_oParty.TaxExempt
            End If

            .TaxExemptSpecified = r_oParty.TaxExemptSpecified
            .TaxNumber = r_oParty.TaxNumber

            If r_oParty.TaxPercentageSpecified Then
                .TaxPercentage = r_oParty.TaxPercentage
            End If

            .TaxPercentageSpecified = r_oParty.TaxPercentageSpecified
            .TPIntroducer = r_oParty.TPIntroducer
            .TPUserCode = r_oParty.TPUserCode
            .XMLDataset = r_oParty.XMLDataset

            If r_oParty.Addresses IsNot Nothing Then
                .Addresses = New List(Of BaseAddressWithContactsType)
                For i As Integer = 0 To r_oParty.Addresses.Count - 1

                    Dim oAddress As New BaseAddressWithContactsType

                    With r_oParty.Addresses(i)

                        If Not .Address1 Is Nothing Then oAddress.AddressLine1 = .Address1.Trim
                        If Not .Address2 Is Nothing Then oAddress.AddressLine2 = .Address2.Trim
                        If Not .Address3 Is Nothing Then oAddress.AddressLine3 = .Address3.Trim
                        If Not .Address4 Is Nothing Then oAddress.AddressLine4 = .Address4.Trim

                        If Not .Address5 Is Nothing Then oAddress.AddressLine5 = .Address5.Trim
                        If Not .Address6 Is Nothing Then oAddress.AddressLine6 = .Address6.Trim
                        If Not .Address7 Is Nothing Then oAddress.AddressLine7 = .Address7.Trim
                        If Not .Address8 Is Nothing Then oAddress.AddressLine8 = .Address8.Trim
                        If Not .Address9 Is Nothing Then oAddress.AddressLine9 = .Address9.Trim
                        If Not .Address10 Is Nothing Then oAddress.AddressLine10 = .Address10.Trim

                        oAddress.AddressTypeCode = .AddressType
                        oAddress.CountryCode = .CountryCode
                        oAddress.PostCode = .PostCode

                    End With

                    .Addresses.Add(oAddress)

                Next

            End If

            If r_oParty.Contacts IsNot Nothing Then
                .Contacts = New List(Of BaseContactType)
                For i As Integer = 0 To r_oParty.Contacts.Count - 1

                    Dim oContact As New BaseContactType()

                    With r_oParty.Contacts(i)

                        oContact.AreaCode = .AreaCode
                        oContact.ContactDetail = New BaseContactDetailType()
                        Select Case .ContactType
                            Case ContactType.Email
                                oContact.ContactDetail.Item = r_oParty.Contacts(i).Number '.ContactType.Email
                                oContact.ContactDetail.ItemElementName = ItemChoiceType.EmailAddress
                                Exit Select
                            Case ContactType.Fax
                                oContact.ContactDetail.Item = r_oParty.Contacts(i).Number ' .ContactType.Fax
                                oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
                                Exit Select
                            Case ContactType.HomePhone
                                oContact.ContactDetail.Item = r_oParty.Contacts(i).Number '.ContactType.HomePhone
                                oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
                                Exit Select
                            Case ContactType.Main
                                oContact.ContactDetail.Item = r_oParty.Contacts(i).Number ' .ContactType.Main
                                oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
                                Exit Select
                            Case ContactType.Mobile
                                oContact.ContactDetail.Item = r_oParty.Contacts(i).Number ' .ContactType.Mobile
                                oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
                                Exit Select
                            Case ContactType.Web
                                oContact.ContactDetail.Item = r_oParty.Contacts(i).Number ' .ContactType.Web
                                oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
                                Exit Select
                            Case Else
                                oContact.ContactDetail.Item = r_oParty.Contacts(i).Number
                                oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
                                Exit Select
                        End Select

                        oContact.ContactTypeCode = .ContactType
                        oContact.AreaCode = .AreaCode
                        oContact.Description = .Description
                        oContact.Extension = .Extension
                        oContact.OtherContactTypeCode = .OtherContactTypeCode
                    End With

                    .Contacts.Add(oContact)

                Next

            End If

        End With

        If Logger.IsLoggingEnabled Then
            Dim sbLogMessage As New StringBuilder
            sbLogMessage.AppendLine("ConvertParty executed ok" & vbCrLf)
            sbLogMessage.AppendLine("Input:" & vbCrLf)
            sbLogMessage.AppendLine("r_oParty = " & r_oParty.Print.Replace("<br />", vbCrLf))

            sbLogMessage.AppendLine("Returned " & oParty.ToString & vbCrLf)

            Dim logEntry As New LogEntry()

            logEntry.Categories.Clear()
            logEntry.Categories.Add(Category.General)
            logEntry.Priority = Priority.Normal
            logEntry.Severity = TraceEventType.Verbose
            logEntry.Message = sbLogMessage.ToString

            Logger.Write(logEntry)
        End If
        Return oParty
    End Function
    Private Function ConvertPartyCC(ByVal r_oParty As BaseParty) As BaseClasses.BasePartyType

        Dim oParty As BaseClasses.BasePartyType = Nothing

        If TypeOf r_oParty Is CorporateParty Then

            oParty = New BaseClasses.BasePartyCCType
            With DirectCast(oParty, BaseClasses.BasePartyCCType)
                .Currency = DirectCast(r_oParty, CorporateParty).Currency
                .AccountExecutive = DirectCast(r_oParty, CorporateParty).AccountExecutive
                .AccountExecutiveCode = DirectCast(r_oParty, CorporateParty).AccountExecutiveCode

                .AlternativeId = DirectCast(r_oParty, CorporateParty).AlternativeId
                .BusinessCode = DirectCast(r_oParty, CorporateParty).BusinessCode
                If String.IsNullOrEmpty(DirectCast(r_oParty, CorporateParty).CompanyName) Then
                    Throw New ArgumentNullException("Party.CompanyName")
                Else
                    .CompanyName = DirectCast(r_oParty, CorporateParty).CompanyName
                End If
                .CompanyReg = DirectCast(r_oParty, CorporateParty).CompanyReg

                .Currency = DirectCast(r_oParty, CorporateParty).Currency
                If DirectCast(r_oParty, CorporateParty).DomiciledForTaxSpecified Then
                    .DomiciledForTax = DirectCast(r_oParty, CorporateParty).DomiciledForTax
                End If
                .DomiciledForTaxSpecified = DirectCast(r_oParty, CorporateParty).DomiciledForTaxSpecified

                Dim oClient As New BaseClientSharedDataType '= Nothing


                With DirectCast(r_oParty, CorporateParty).ClientSharedData

                    oClient.AccountBalance = .AccountBalance
                    oClient.AgentReference = .AgentReference
                    oClient.AreaCode = .AreaCode
                    oClient.CorrespondenceCode = .CorrespondenceCode
                    oClient.CountyCourtJudgments = .CountyCourtJudgments
                    oClient.CurrentIntermediaryKey = .CurrentIntermediaryKey
                    oClient.CurrentIntermediaryName = .CurrentIntermediaryName
                    oClient.IsAgent = .IsAgent
                    oClient.IsProspect = False '.IsProspect
                    oClient.IsProspectSpecified = False
                    oClient.LastYearTurnover = .LastYearTurnover
                    oClient.LeadAgentCode = .LeadAgentCode
                    If .LeadAgentKey > 0 Then
                        oClient.LeadAgentKey = .LeadAgentKey
                        oClient.LeadAgentKeySpecified = True
                    Else
                        oClient.LeadAgentKeySpecified = False
                    End If
                    oClient.LeadAgentName = .LeadAgentName
                    oClient.LoyaltyNumber = .LoyaltyNumber
                    oClient.PaymentCode = .PaymentCode
                    oClient.PaymentTermCode = .PaymentTermCode
                    oClient.PreviousBrokerCode = .PreviousBrokerCode
                    oClient.PreviousBrokerKey = .PreviousBrokerKey
                    oClient.PreviousBrokerName = .PreviousBrokerName
                    oClient.PreviousInsurerCode = .PreviousInsurerCode
                    oClient.PreviousInsurerKey = .PreviousInsurerKey
                    oClient.PreviousInsurerName = .PreviousInsurerName
                    oClient.ReminderCode = .ReminderCode
                    oClient.RenewalStopCode = .RenewalStopCode
                    oClient.SeasonalGiftCode = .SeasonalGiftCode
                    oClient.ServiceLevelCode = .ServiceLevelCode
                    oClient.ShortName = .ShortName
                    oClient.StatusCode = .StatusCode
                    oClient.StrengthCode = .StrengthCode
                    oClient.YearToDateTurnover = .YearToDateTurnover
                    oClient.BlacklistReasonCode = .BlacklistReasonCode
                End With

                'Note: BlacklistReasonCode and RenewalStopCode are stored in BaseParty, not in BaseClientSharedDataType
                'They will be retrieved from CorporateParty when needed

                .ClientDetail = oClient

                'AssociateType
                If r_oParty.Associate IsNot Nothing AndAlso r_oParty.Associate.Count > 0 Then
                    .ClientDetail.Associates = New List(Of BaseAssociateType)
                    For iCnt As Integer = 0 To r_oParty.Associate.Count - 1
                        Dim oAssociate As New BaseAssociateType

                        With r_oParty.Associate(iCnt)
                            oAssociate.AssociateKey = .AssociateKey
                            oAssociate.AssociateCode = .AssociateCode
                            oAssociate.AssociateName = .AssociateName
                            oAssociate.ClientKey = IIf(r_oParty.Key, r_oParty.Key, .ClientKey)
                            oAssociate.RelationshipCode = .RelationshipCode
                            oAssociate.RelationshipDescription = .RelationshipDescription
                        End With
                        .ClientDetail.Associates.Add(oAssociate)
                    Next
                End If

                'Convictions
                If r_oParty.Conviction IsNot Nothing AndAlso r_oParty.Conviction.Count > 0 Then
                    .ClientDetail.Convictions = New List(Of BaseConvictionType)
                    For iCnt As Integer = 0 To r_oParty.Conviction.Count - 1
                        Dim oConviction As New BaseConvictionType

                        With r_oParty.Conviction(iCnt)
                            oConviction.ConvictionKey = .ConvictionKey
                            oConviction.Description = .Description
                            oConviction.TypeCode = .TypeCode
                            oConviction.Date = .ConvictionDate
                            If .AlcoholLevel > 0 Then
                                oConviction.AlcoholLevel = .AlcoholLevel
                                oConviction.AlcoholLevelSpecified = True
                            Else
                                oConviction.AlcoholLevelSpecified = False
                            End If
                            oConviction.AlcoholMeasurementMethod = .AlcoholMeasurementMethod

                            If .DrivingLicensePenaltyPoints > 0 Then
                                oConviction.DrivingLicensePenaltyPoints = .DrivingLicensePenaltyPoints
                                oConviction.DrivingLicensePenaltyPointsSpecified = True
                            Else
                                oConviction.DrivingLicensePenaltyPointsSpecified = False
                            End If

                            If .FineAmount > 0 Then
                                oConviction.FineAmount = .FineAmount
                                oConviction.FineAmountSpecified = True
                            Else
                                oConviction.FineAmountSpecified = False
                            End If

                            oConviction.SentenceDescription = .SentenceDescription

                            If .SentenceDuration > 0 Then
                                oConviction.SentenceDuration = .SentenceDuration
                                oConviction.SentenceDurationSpecified = True
                            Else
                                oConviction.SentenceDurationSpecified = False
                            End If

                            '   oConviction.SentenceDurationQualifier = .SentenceDurationQualifier

                            If .SentenceEffectiveDate <> Date.MinValue Then
                                oConviction.SentenceEffectiveDate = .SentenceEffectiveDate
                                oConviction.SentenceEffectiveDateSpecified = True
                            Else
                                oConviction.SentenceEffectiveDateSpecified = False
                            End If

                            oConviction.SentenceTypeCode = .SentenceTypeCode
                            oConviction.StatusCode = .StatusCode
                        End With

                        .ClientDetail.Convictions.Add(oConviction)
                    Next
                End If

                'LoyaltyScheme
                If r_oParty.Loyalty IsNot Nothing AndAlso r_oParty.Loyalty.Count > 0 Then
                    .ClientDetail.LoyaltyScheme = New List(Of BaseClientSharedDataTypeLoyaltyScheme)
                    For iCnt As Integer = 0 To r_oParty.Loyalty.Count - 1
                        Dim oLoyalty As New BaseClientSharedDataTypeLoyaltyScheme

                        With r_oParty.Loyalty(iCnt)
                            oLoyalty.LoyaltySchemeCode = .LoyaltySchemeCode
                            oLoyalty.LoyaltySchemeKey = .LoyaltySchemeKey
                            oLoyalty.MainMember = .MainMember
                            oLoyalty.MembershipNumber = .MembershipNumber
                            oLoyalty.OtherReference = .OtherReference
                            oLoyalty.Active = .Active
                            oLoyalty.ActiveSpecified = .Active
                            oLoyalty.StartDate = .StartDate

                            If .EndDate <> Date.MinValue Then
                                oLoyalty.EndDate = .EndDate
                                oLoyalty.EndDateSpecified = True
                            Else
                                oLoyalty.EndDateSpecified = False
                            End If
                        End With

                        .ClientDetail.LoyaltyScheme.Add(oLoyalty)
                    Next
                End If

                'ProspectPolicies
                If r_oParty.ProspectPolicy IsNot Nothing AndAlso r_oParty.ProspectPolicy.Count > 0 Then
                    .ClientDetail.ProspectPolicies = New List(Of BaseClientSharedDataTypeProspectPolicies)
                    For iCnt As Integer = 0 To r_oParty.ProspectPolicy.Count - 1
                        Dim oProspectPolicies As New BaseClientSharedDataTypeProspectPolicies
                        With r_oParty.ProspectPolicy(iCnt)
                            oProspectPolicies.ProspectPolicyKey = .ProspectPolicyKey
                            oProspectPolicies.ProspectTypeCode = .ProspectTypeCode

                            If .RenewalDate <> Date.MinValue Then
                                oProspectPolicies.RenewalDate = .RenewalDate
                                oProspectPolicies.RenewalDateSpecified = True
                            Else
                                oProspectPolicies.RenewalDateSpecified = False
                            End If

                            If .TargetPremium > 0 Then
                                oProspectPolicies.TargetPremium = .TargetPremium
                                oProspectPolicies.TargetPremiumSpecified = True
                            Else
                                oProspectPolicies.TargetPremiumSpecified = False
                            End If

                            If .TimesQuoted > 0 Then
                                oProspectPolicies.TimesQuoted = .TimesQuoted
                                oProspectPolicies.TimesQuotedSpecified = True
                            Else
                                oProspectPolicies.TimesQuotedSpecified = False
                            End If

                        End With

                        .ClientDetail.ProspectPolicies.Add(oProspectPolicies)
                    Next
                End If

                If DirectCast(r_oParty, CorporateParty).eMPSSpecified Then
                    .EMPS = DirectCast(r_oParty, CorporateParty).eMPS
                End If
                .EMPSSpecified = DirectCast(r_oParty, CorporateParty).eMPSSpecified
                .FileCode = DirectCast(r_oParty, CorporateParty).FileCode
                If DirectCast(r_oParty, CorporateParty).FinancialYearSpecified Then
                    .FinancialYear = DirectCast(r_oParty, CorporateParty).FinancialYear
                End If
                .FinancialYearSpecified = DirectCast(r_oParty, CorporateParty).FinancialYearSpecified

                .MainContact = DirectCast(r_oParty, CorporateParty).MainContact
                If DirectCast(r_oParty, CorporateParty).MPSSpecified Then
                    .MPS = DirectCast(r_oParty, CorporateParty).MPS
                End If
                .MPSSpecified = DirectCast(r_oParty, CorporateParty).MPSSpecified

                .NumberOfEmployees = DirectCast(r_oParty, CorporateParty).NumberOfEmployees

                If DirectCast(r_oParty, CorporateParty).NumberOfOfficesSpecified Then
                    .NumberOfOffices = DirectCast(r_oParty, CorporateParty).NumberOfOffices
                End If

                .NumberOfOfficesSpecified = DirectCast(r_oParty, CorporateParty).NumberOfOfficesSpecified
                .Salutation = DirectCast(r_oParty, CorporateParty).Salutation
                .SICCode = DirectCast(r_oParty, CorporateParty).SICCode
                .Source = DirectCast(r_oParty, CorporateParty).Source
                If DirectCast(r_oParty, CorporateParty).TaxExemptSpecified Then
                    .TaxExempt = DirectCast(r_oParty, CorporateParty).TaxExempt
                End If

                .TaxExemptSpecified = DirectCast(r_oParty, CorporateParty).TaxExemptSpecified
                .TaxNumber = DirectCast(r_oParty, CorporateParty).TaxNumber
                If DirectCast(r_oParty, CorporateParty).TaxPercentageSpecified Then
                    .TaxPercentage = DirectCast(r_oParty, CorporateParty).TaxPercentage
                End If

                .TaxPercentageSpecified = DirectCast(r_oParty, CorporateParty).TaxPercentageSpecified
                .TPIntroducer = DirectCast(r_oParty, CorporateParty).TPIntroducer

                .TPUserCode = DirectCast(r_oParty, CorporateParty).TPUserCode
                .TradeCode = DirectCast(r_oParty, CorporateParty).TradeCode
                If DirectCast(r_oParty, CorporateParty).TPSSpecified Then
                    .TPS = DirectCast(r_oParty, CorporateParty).TPS
                End If

                .TPSSpecified = DirectCast(r_oParty, CorporateParty).TPSSpecified
                If DirectCast(r_oParty, CorporateParty).TradingsinceSpecified Then
                    .TradingSince = DirectCast(r_oParty, CorporateParty).TradingSince
                End If
                .TradingSinceSpecified = DirectCast(r_oParty, CorporateParty).TradingsinceSpecified
                .TurnoverCode = DirectCast(r_oParty, CorporateParty).TurnoverCode
                If DirectCast(r_oParty, CorporateParty).WageRollSpecified Then
                    .WageRoll = DirectCast(r_oParty, CorporateParty).WageRoll
                End If
                .WageRollSpecified = DirectCast(r_oParty, CorporateParty).WageRollSpecified
                .BranchCode = DirectCast(r_oParty, CorporateParty).BranchCode
                .XMLDataset = DirectCast(r_oParty, CorporateParty).XMLDataset
            End With

        End If
        oParty = ConvertPartyCommon(r_oParty, Nothing,oParty, Nothing)

        Return oParty

    End Function

    Private Function ConvertPartyPC(ByVal r_oParty As BaseParty) As BaseClasses.BasePartyType

        Dim oParty As BaseClasses.BasePartyType = Nothing
        If TypeOf r_oParty Is PersonalParty Then

            oParty = New BasePartyPCType

            With DirectCast(oParty, BasePartyPCType)
                .Currency = DirectCast(r_oParty, PersonalParty).Currency
                .AccommodationCode = DirectCast(r_oParty, PersonalParty).AccommodationCode
                .AccountExecutive = DirectCast(r_oParty, PersonalParty).AccountExecutive
                .AccountExecutiveCode = DirectCast(r_oParty, PersonalParty).AccountExecutiveCode
                .AlternativeId = DirectCast(r_oParty, PersonalParty).AlternativeID

                'LifeStyle (only for PCType)
                If r_oParty.Lifestyle IsNot Nothing AndAlso r_oParty.Lifestyle.Count > 0 Then

                    .Lifestyle = New List(Of BasePartyPCTypeLifestyle)

                    For iCnt As Integer = 0 To r_oParty.Lifestyle.Count - 1
                        Dim oLifeStyle As New BasePartyPCTypeLifestyle

                        With r_oParty.Lifestyle(iCnt)
                            oLifeStyle.LifestyleKey = .LifestyleKey
                            oLifeStyle.Name = .Name
                            oLifeStyle.CategoryCode = .CategoryCode
                            oLifeStyle.OccupationCode = .OccupationCode
                            oLifeStyle.SecOccupationCode = .SecOccupationCode
                            oLifeStyle.Smoker = .Smoker
                            oLifeStyle.SmokerSpecified = .Smoker

                            'Check if exists before setting DOB value
                            If .DateOfBirth IsNot Nothing Then
                                If .DateOfBirth.Trim.Length <> 0 Then
                                    oLifeStyle.DateOfBirth = CDate(.DateOfBirth.Trim())
                                    oLifeStyle.DateOfBirthSpecified = True
                                Else
                                    oLifeStyle.DateOfBirthSpecified = False
                                End If
                            Else
                                oLifeStyle.DateOfBirthSpecified = False
                            End If

                            'Check if exists before settfsing Gender value
                            If .GenderCode IsNot Nothing Then
                                Select Case .GenderCode.Trim.ToUpper()
                                    Case "MALE"
                                        oLifeStyle.GenderCode = GenderCodeType.Male
                                        oLifeStyle.GenderCodeSpecified = True
                                    Case "FEMALE"
                                        oLifeStyle.GenderCode = GenderCodeType.Female
                                        oLifeStyle.GenderCodeSpecified = True
                                End Select
                                'oLifeStyle.GenderCode = .GenderCode
                            End If

                        End With
                        .Lifestyle.Add(oLifeStyle)
                    Next

                End If

                Dim oClient As New BaseClientSharedDataType

                With DirectCast(r_oParty, PersonalParty).ClientSharedData

                    oClient.AccountBalance = .AccountBalance
                    oClient.AgentReference = .AgentReference
                    oClient.AreaCode = .AreaCode
                    oClient.CorrespondenceCode = .CorrespondenceCode
                    oClient.CountyCourtJudgments = .CountyCourtJudgments
                    oClient.CurrentIntermediaryKey = .CurrentIntermediaryKey
                    oClient.CurrentIntermediaryName = .CurrentIntermediaryName
                    oClient.IsAgent = .IsAgent
                    oClient.IsProspect = False '.IsProspect
                    oClient.IsProspectSpecified = False
                    oClient.LastYearTurnover = .LastYearTurnover
                    oClient.LeadAgentCode = .LeadAgentCode
                    If .LeadAgentKey > 0 Then
                        oClient.LeadAgentKey = .LeadAgentKey
                        oClient.LeadAgentKeySpecified = True
                    Else
                        oClient.LeadAgentKeySpecified = False
                    End If

                    oClient.LeadAgentName = .LeadAgentName
                    oClient.LoyaltyNumber = .LoyaltyNumber
                    oClient.PaymentCode = .PaymentCode
                    oClient.PaymentTermCode = .PaymentTermCode
                    oClient.PreviousBrokerCode = .PreviousBrokerCode
                    oClient.PreviousBrokerKey = .PreviousBrokerKey
                    oClient.PreviousBrokerName = .PreviousBrokerName
                    oClient.PreviousInsurerCode = .PreviousInsurerCode
                    oClient.PreviousInsurerKey = .PreviousInsurerKey
                    oClient.PreviousInsurerName = .PreviousInsurerName
                    oClient.ReminderCode = .ReminderCode
                    oClient.RenewalStopCode = .RenewalStopCode
                    oClient.SeasonalGiftCode = .SeasonalGiftCode
                    oClient.ServiceLevelCode = .ServiceLevelCode
                    oClient.ShortName = .ShortName
                    oClient.StatusCode = .StatusCode
                    oClient.StrengthCode = .StrengthCode
                    oClient.YearToDateTurnover = .YearToDateTurnover
                    oClient.BlacklistReasonCode = .BlacklistReasonCode
                End With

                .ClientDetail = oClient
                'AssociateType
                If r_oParty.Associate IsNot Nothing AndAlso r_oParty.Associate.Count > 0 Then
                    .ClientDetail.Associates = New List(Of BaseAssociateType)
                    For iCnt As Integer = 0 To r_oParty.Associate.Count - 1
                        Dim oAssociate As New BaseAssociateType

                        With r_oParty.Associate(iCnt)
                            oAssociate.AssociateKey = .AssociateKey
                            oAssociate.AssociateCode = .AssociateCode
                            oAssociate.AssociateName = .AssociateName
                            oAssociate.ClientKey = IIf(r_oParty.Key, r_oParty.Key, .ClientKey)
                            oAssociate.RelationshipCode = .RelationshipCode
                            oAssociate.RelationshipDescription = .RelationshipDescription
                        End With
                        .ClientDetail.Associates.Add(oAssociate)
                    Next
                End If

                'Convictions
                If r_oParty.Conviction IsNot Nothing AndAlso r_oParty.Conviction.Count > 0 Then
                    .ClientDetail.Convictions = New List(Of BaseConvictionType)
                    For iCnt As Integer = 0 To r_oParty.Conviction.Count - 1
                        Dim oConviction As New BaseConvictionType

                        With r_oParty.Conviction(iCnt)
                            oConviction.ConvictionKey = .ConvictionKey
                            oConviction.Description = .Description
                            oConviction.TypeCode = .TypeCode
                            oConviction.Date = .ConvictionDate
                            If .AlcoholLevel > 0 Then
                                oConviction.AlcoholLevel = .AlcoholLevel
                                oConviction.AlcoholLevelSpecified = True
                            Else
                                oConviction.AlcoholLevelSpecified = False
                            End If
                            oConviction.AlcoholMeasurementMethod = .AlcoholMeasurementMethod

                            If .DrivingLicensePenaltyPoints > 0 Then
                                oConviction.DrivingLicensePenaltyPoints = .DrivingLicensePenaltyPoints
                                oConviction.DrivingLicensePenaltyPointsSpecified = True
                            Else
                                oConviction.DrivingLicensePenaltyPointsSpecified = False
                            End If

                            If .FineAmount > 0 Then
                                oConviction.FineAmount = .FineAmount
                                oConviction.FineAmountSpecified = True
                            Else
                                oConviction.FineAmountSpecified = False
                            End If

                            oConviction.SentenceDescription = .SentenceDescription

                            If .SentenceDuration > 0 Then
                                oConviction.SentenceDuration = .SentenceDuration
                                oConviction.SentenceDurationSpecified = True
                            Else
                                oConviction.SentenceDurationSpecified = False
                            End If

                            ' oConviction.SentenceDurationQualifier = .SentenceDurationQualifier

                            If .SentenceEffectiveDate <> Date.MinValue Then
                                oConviction.SentenceEffectiveDate = .SentenceEffectiveDate
                                oConviction.SentenceEffectiveDateSpecified = True
                            Else
                                oConviction.SentenceEffectiveDateSpecified = False
                            End If

                            oConviction.SentenceTypeCode = .SentenceTypeCode
                            oConviction.StatusCode = .StatusCode
                        End With
                        .ClientDetail.Convictions.Add(oConviction)
                    Next
                End If


                'LoyaltyScheme
                If r_oParty.Loyalty IsNot Nothing AndAlso r_oParty.Loyalty.Count > 0 Then
                    .ClientDetail.LoyaltyScheme = New List(Of BaseClientSharedDataTypeLoyaltyScheme)
                    For iCnt As Integer = 0 To r_oParty.Loyalty.Count - 1
                        Dim oLoyalty As New BaseClientSharedDataTypeLoyaltyScheme

                        With r_oParty.Loyalty(iCnt)
                            oLoyalty.LoyaltySchemeCode = .LoyaltySchemeCode
                            oLoyalty.LoyaltySchemeKey = .LoyaltySchemeKey
                            oLoyalty.MainMember = .MainMember
                            oLoyalty.MembershipNumber = .MembershipNumber
                            oLoyalty.OtherReference = .OtherReference
                            oLoyalty.Active = .Active
                            oLoyalty.ActiveSpecified = .Active
                            oLoyalty.StartDate = .StartDate

                            If .EndDate <> Date.MinValue Then
                                oLoyalty.EndDate = .EndDate
                                oLoyalty.EndDateSpecified = True
                            Else
                                oLoyalty.EndDateSpecified = False
                            End If
                        End With
                        .ClientDetail.LoyaltyScheme.Add(oLoyalty)
                    Next
                End If

                'ProspectPolicies
                If r_oParty.ProspectPolicy IsNot Nothing AndAlso r_oParty.ProspectPolicy.Count > 0 Then
                    .ClientDetail.ProspectPolicies = New List(Of BaseClasses.BaseClientSharedDataTypeProspectPolicies)
                    For iCnt As Integer = 0 To r_oParty.ProspectPolicy.Count - 1
                        Dim oProspectPolicies As New BaseClientSharedDataTypeProspectPolicies

                        With r_oParty.ProspectPolicy(iCnt)
                            oProspectPolicies.ProspectPolicyKey = .ProspectPolicyKey
                            oProspectPolicies.ProspectTypeCode = .ProspectTypeCode

                            If .RenewalDate <> Date.MinValue Then
                                oProspectPolicies.RenewalDate = .RenewalDate
                                oProspectPolicies.RenewalDateSpecified = True
                            Else
                                oProspectPolicies.RenewalDateSpecified = False
                            End If

                            If .TargetPremium > 0 Then
                                oProspectPolicies.TargetPremium = .TargetPremium
                                oProspectPolicies.TargetPremiumSpecified = True
                            Else
                                oProspectPolicies.TargetPremiumSpecified = False
                            End If

                            If .TimesQuoted > 0 Then
                                oProspectPolicies.TimesQuoted = .TimesQuoted
                                oProspectPolicies.TimesQuotedSpecified = True
                            Else
                                oProspectPolicies.TimesQuotedSpecified = False
                            End If

                        End With
                        .ClientDetail.ProspectPolicies.Add(oProspectPolicies)
                    Next
                End If

                .Currency = DirectCast(r_oParty, PersonalParty).Currency

                If DirectCast(r_oParty, PersonalParty).DateOfBirthSpecified Then
                    .DateOfBirth = DirectCast(r_oParty, PersonalParty).DateOfBirth
                End If
                '.DateOfBirthSpecified = DirectCast(r_oParty, PersonalParty).DateOfBirthSpecified
                If DirectCast(r_oParty, PersonalParty).DomiciledForTaxSpecified Then
                    .DomiciledForTax = DirectCast(r_oParty, PersonalParty).DomiciledForTax
                End If
                .DomiciledForTaxSpecified = DirectCast(r_oParty, PersonalParty).DomiciledForTaxSpecified

                .EmployersBusinessCode = DirectCast(r_oParty, PersonalParty).EmployersBusinessCode

                If DirectCast(r_oParty, PersonalParty).EmploymentStatusCodeSpecified Then
                    .EmploymentStatusCode = DirectCast(r_oParty, PersonalParty).EmploymentStatusCode
                End If

                .EmploymentStatusCodeSpecified = DirectCast(r_oParty, PersonalParty).EmploymentStatusCodeSpecified

                If DirectCast(r_oParty, PersonalParty).eMPSSpecified Then
                    .EMPS = DirectCast(r_oParty, PersonalParty).eMPS
                End If
                .EMPSSpecified = DirectCast(r_oParty, PersonalParty).eMPSSpecified
                .FileCode = DirectCast(r_oParty, PersonalParty).FileCode

                If String.IsNullOrEmpty(DirectCast(r_oParty, PersonalParty).Forename) Then
                    Throw New ArgumentNullException("Party.Forename")
                Else
                    .Forename = DirectCast(r_oParty, PersonalParty).Forename
                End If


                .GenderCode = DirectCast(r_oParty, PersonalParty).GenderCode

                If String.IsNullOrEmpty(DirectCast(r_oParty, PersonalParty).Initials) Then
                    Throw New ArgumentNullException("Party.Initials")
                Else
                    .Initials = DirectCast(r_oParty, PersonalParty).Initials
                End If

                If DirectCast(r_oParty, PersonalParty).MaritalStatusCodeSpecified Then
                    .MaritalStatusCode = DirectCast(r_oParty, PersonalParty).MaritalStatusCode
                End If

                .MaritalStatusCodeSpecified = DirectCast(r_oParty, PersonalParty).MaritalStatusCodeSpecified
                If DirectCast(r_oParty, PersonalParty).MPSSpecified Then
                    .MPS = DirectCast(r_oParty, PersonalParty).MPS
                End If

                .MPSSpecified = DirectCast(r_oParty, PersonalParty).MPSSpecified
                .NationalityCode = DirectCast(r_oParty, PersonalParty).NationalityCode
                .OccupationCode = DirectCast(r_oParty, PersonalParty).OccupationCode
                If DirectCast(r_oParty, PersonalParty).PetOwnerSpecified Then
                    .PetOwner = DirectCast(r_oParty, PersonalParty).PetOwner
                End If

                .PetOwnerSpecified = DirectCast(r_oParty, PersonalParty).PetOwnerSpecified
                .Salutation = DirectCast(r_oParty, PersonalParty).Salutation
                If String.IsNullOrEmpty(DirectCast(r_oParty, PersonalParty).Lastname) Then
                    Throw New ArgumentNullException("Party.Lastname")
                Else
                    .Surname = DirectCast(r_oParty, PersonalParty).Lastname
                End If
                .SecEmployersBusinessCode = DirectCast(r_oParty, PersonalParty).SecEmployersBusinessCode

                If DirectCast(r_oParty, PersonalParty).SecEmploymentStatusCodeSpecified Then
                    .SecEmploymentStatusCode = DirectCast(r_oParty, PersonalParty).SecEmploymentStatusCode
                End If

                .SecEmploymentStatusCodeSpecified = DirectCast(r_oParty, PersonalParty).SecEmploymentStatusCodeSpecified
                .SecOccupationCode = DirectCast(r_oParty, PersonalParty).SecOccupationCode
                .Source = DirectCast(r_oParty, PersonalParty).Source
                '.TaxExempt = DirectCast(r_oParty, PersonalParty).TaxExempt
                If DirectCast(r_oParty, PersonalParty).TaxExemptSpecified Then
                    .TaxExempt = DirectCast(r_oParty, PersonalParty).TaxExempt
                End If

                .TaxExemptSpecified = DirectCast(r_oParty, PersonalParty).TaxExemptSpecified
                .TaxNumber = DirectCast(r_oParty, PersonalParty).TaxNumber
                If DirectCast(r_oParty, PersonalParty).TaxPercentageSpecified Then
                    .TaxPercentage = DirectCast(r_oParty, PersonalParty).TaxPercentage
                End If

                .TaxPercentageSpecified = DirectCast(r_oParty, PersonalParty).TaxPercentageSpecified
                If String.IsNullOrEmpty(DirectCast(r_oParty, PersonalParty).Title) Then
                    Throw New ArgumentNullException("Party.Title")
                Else
                    .Title = DirectCast(r_oParty, PersonalParty).Title
                End If
                .TPIntroducer = DirectCast(r_oParty, PersonalParty).TPIntroducer
                If DirectCast(r_oParty, PersonalParty).TPSSpecified Then
                    .TPS = DirectCast(r_oParty, PersonalParty).TPS
                End If
                .BranchCode = DirectCast(r_oParty, PersonalParty).BranchCode
                .XMLDataset = DirectCast(r_oParty, PersonalParty).XMLDataset
            End With

        End If

        oParty = ConvertPartyCommon(r_oParty, oParty, Nothing, Nothing)

        Return oParty

    End Function
    'Private Function ConvertParty(ByVal r_oParty As BaseParty) As BaseClasses.BasePartyType

    '    Dim oParty As BaseClasses.BasePartyType = Nothing

    '    Select Case True
    '        Case TypeOf r_oParty Is PersonalParty

    '            oParty = New BasePartyPCType

    '            With DirectCast(oParty, BasePartyPCType)
    '                .Currency = DirectCast(r_oParty, PersonalParty).Currency
    '                .AccommodationCode = DirectCast(r_oParty, PersonalParty).AccommodationCode
    '                .AccountExecutive = DirectCast(r_oParty, PersonalParty).AccountExecutive
    '                .AccountExecutiveCode = DirectCast(r_oParty, PersonalParty).AccountExecutiveCode
    '                .AlternativeId = DirectCast(r_oParty, PersonalParty).AlternativeID

    '                'LifeStyle (only for PCType)
    '                If r_oParty.Lifestyle IsNot Nothing AndAlso r_oParty.Lifestyle.Count > 0 Then

    '                    .Lifestyle = New List(Of BasePartyPCTypeLifestyle)

    '                    For iCnt As Integer = 0 To r_oParty.Lifestyle.Count - 1
    '                        Dim oLifeStyle As New BasePartyPCTypeLifestyle

    '                        With r_oParty.Lifestyle(iCnt)
    '                            oLifeStyle.LifestyleKey = .LifestyleKey
    '                            oLifeStyle.Name = .Name
    '                            oLifeStyle.CategoryCode = .CategoryCode
    '                            oLifeStyle.OccupationCode = .OccupationCode
    '                            oLifeStyle.SecOccupationCode = .SecOccupationCode
    '                            oLifeStyle.Smoker = .Smoker
    '                            oLifeStyle.SmokerSpecified = .Smoker

    '                            'Check if exists before setting DOB value
    '                            If .DateOfBirth IsNot Nothing Then
    '                                If .DateOfBirth.Trim.Length <> 0 Then
    '                                    oLifeStyle.DateOfBirth = CDate(.DateOfBirth.Trim())
    '                                    oLifeStyle.DateOfBirthSpecified = True
    '                                Else
    '                                    oLifeStyle.DateOfBirthSpecified = False
    '                                End If
    '                            Else
    '                                oLifeStyle.DateOfBirthSpecified = False
    '                            End If

    '                            'Check if exists before settfsing Gender value
    '                            If .GenderCode IsNot Nothing Then
    '                                Select Case .GenderCode.Trim.ToUpper()
    '                                    Case "MALE"
    '                                        oLifeStyle.GenderCode = GenderCodeType.Male
    '                                        oLifeStyle.GenderCodeSpecified = True
    '                                    Case "FEMALE"
    '                                        oLifeStyle.GenderCode = GenderCodeType.Female
    '                                        oLifeStyle.GenderCodeSpecified = True
    '                                End Select
    '                                'oLifeStyle.GenderCode = .GenderCode
    '                            End If

    '                        End With
    '                        .Lifestyle.Add(oLifeStyle)
    '                    Next

    '                End If

    '                Dim oClient As New BaseClientSharedDataType

    '                With DirectCast(r_oParty, PersonalParty).ClientSharedData

    '                    oClient.AccountBalance = .AccountBalance
    '                    oClient.AgentReference = .AgentReference
    '                    oClient.AreaCode = .AreaCode
    '                    oClient.CorrespondenceCode = .CorrespondenceCode
    '                    oClient.CountyCourtJudgments = .CountyCourtJudgments
    '                    oClient.CurrentIntermediaryKey = .CurrentIntermediaryKey
    '                    oClient.CurrentIntermediaryName = .CurrentIntermediaryName
    '                    oClient.IsAgent = .IsAgent
    '                    oClient.IsProspect = False '.IsProspect
    '                    oClient.IsProspectSpecified = False
    '                    oClient.LastYearTurnover = .LastYearTurnover
    '                    oClient.LeadAgentCode = .LeadAgentCode
    '                    If .LeadAgentKey > 0 Then
    '                        oClient.LeadAgentKey = .LeadAgentKey
    '                        oClient.LeadAgentKeySpecified = True
    '                    Else
    '                        oClient.LeadAgentKeySpecified = False
    '                    End If

    '                    oClient.LeadAgentName = .LeadAgentName
    '                    oClient.LoyaltyNumber = .LoyaltyNumber
    '                    oClient.PaymentCode = .PaymentCode
    '                    oClient.PaymentTermCode = .PaymentTermCode
    '                    oClient.PreviousBrokerCode = .PreviousBrokerCode
    '                    oClient.PreviousBrokerKey = .PreviousBrokerKey
    '                    oClient.PreviousBrokerName = .PreviousBrokerName
    '                    oClient.PreviousInsurerCode = .PreviousInsurerCode
    '                    oClient.PreviousInsurerKey = .PreviousInsurerKey
    '                    oClient.PreviousInsurerName = .PreviousInsurerName
    '                    oClient.ReminderCode = .ReminderCode
    '                    oClient.RenewalStopCode = .RenewalStopCode
    '                    oClient.SeasonalGiftCode = .SeasonalGiftCode
    '                    oClient.ServiceLevelCode = .ServiceLevelCode
    '                    oClient.ShortName = .ShortName
    '                    oClient.StatusCode = .StatusCode
    '                    oClient.StrengthCode = .StrengthCode
    '                    oClient.YearToDateTurnover = .YearToDateTurnover
    '                End With

    '                .ClientDetail = oClient
    '                'AssociateType
    '                If r_oParty.Associate IsNot Nothing AndAlso r_oParty.Associate.Count > 0 Then
    '                    .ClientDetail.Associates = New List(Of BaseAssociateType)
    '                    For iCnt As Integer = 0 To r_oParty.Associate.Count - 1
    '                        Dim oAssociate As New BaseAssociateType

    '                        With r_oParty.Associate(iCnt)
    '                            oAssociate.AssociateKey = .AssociateKey
    '                            oAssociate.AssociateCode = .AssociateCode
    '                            oAssociate.AssociateName = .AssociateName
    '                            oAssociate.ClientKey = IIf(r_oParty.Key, r_oParty.Key, .ClientKey)
    '                            oAssociate.RelationshipCode = .RelationshipCode
    '                            oAssociate.RelationshipDescription = .RelationshipDescription
    '                        End With
    '                        .ClientDetail.Associates.Add(oAssociate)
    '                    Next
    '                End If

    '                'Convictions
    '                If r_oParty.Conviction IsNot Nothing AndAlso r_oParty.Conviction.Count > 0 Then
    '                    .ClientDetail.Convictions = New List(Of BaseConvictionType)
    '                    For iCnt As Integer = 0 To r_oParty.Conviction.Count - 1
    '                        Dim oConviction As New BaseConvictionType

    '                        With r_oParty.Conviction(iCnt)
    '                            oConviction.ConvictionKey = .ConvictionKey
    '                            oConviction.Description = .Description
    '                            oConviction.TypeCode = .TypeCode
    '                            oConviction.Date = .ConvictionDate
    '                            If .AlcoholLevel > 0 Then
    '                                oConviction.AlcoholLevel = .AlcoholLevel
    '                                oConviction.AlcoholLevelSpecified = True
    '                            Else
    '                                oConviction.AlcoholLevelSpecified = False
    '                            End If
    '                            oConviction.AlcoholMeasurementMethod = .AlcoholMeasurementMethod

    '                            If .DrivingLicensePenaltyPoints > 0 Then
    '                                oConviction.DrivingLicensePenaltyPoints = .DrivingLicensePenaltyPoints
    '                                oConviction.DrivingLicensePenaltyPointsSpecified = True
    '                            Else
    '                                oConviction.DrivingLicensePenaltyPointsSpecified = False
    '                            End If

    '                            If .FineAmount > 0 Then
    '                                oConviction.FineAmount = .FineAmount
    '                                oConviction.FineAmountSpecified = True
    '                            Else
    '                                oConviction.FineAmountSpecified = False
    '                            End If

    '                            oConviction.SentenceDescription = .SentenceDescription

    '                            If .SentenceDuration > 0 Then
    '                                oConviction.SentenceDuration = .SentenceDuration
    '                                oConviction.SentenceDurationSpecified = True
    '                            Else
    '                                oConviction.SentenceDurationSpecified = False
    '                            End If

    '                            ' oConviction.SentenceDurationQualifier = .SentenceDurationQualifier

    '                            If .SentenceEffectiveDate <> Date.MinValue Then
    '                                oConviction.SentenceEffectiveDate = .SentenceEffectiveDate
    '                                oConviction.SentenceEffectiveDateSpecified = True
    '                            Else
    '                                oConviction.SentenceEffectiveDateSpecified = False
    '                            End If

    '                            oConviction.SentenceTypeCode = .SentenceTypeCode
    '                            oConviction.StatusCode = .StatusCode
    '                        End With
    '                        .ClientDetail.Convictions.Add(oConviction)
    '                    Next
    '                End If


    '                'LoyaltyScheme
    '                If r_oParty.Loyalty IsNot Nothing AndAlso r_oParty.Loyalty.Count > 0 Then
    '                    .ClientDetail.LoyaltyScheme = New List(Of BaseClientSharedDataTypeLoyaltyScheme)
    '                    For iCnt As Integer = 0 To r_oParty.Loyalty.Count - 1
    '                        Dim oLoyalty As New BaseClientSharedDataTypeLoyaltyScheme

    '                        With r_oParty.Loyalty(iCnt)
    '                            oLoyalty.LoyaltySchemeCode = .LoyaltySchemeCode
    '                            oLoyalty.LoyaltySchemeKey = .LoyaltySchemeKey
    '                            oLoyalty.MainMember = .MainMember
    '                            oLoyalty.MembershipNumber = .MembershipNumber
    '                            oLoyalty.OtherReference = .OtherReference
    '                            oLoyalty.Active = .Active
    '                            oLoyalty.ActiveSpecified = .Active
    '                            oLoyalty.StartDate = .StartDate

    '                            If .EndDate <> Date.MinValue Then
    '                                oLoyalty.EndDate = .EndDate
    '                                oLoyalty.EndDateSpecified = True
    '                            Else
    '                                oLoyalty.EndDateSpecified = False
    '                            End If
    '                        End With
    '                        .ClientDetail.LoyaltyScheme.Add(oLoyalty)
    '                    Next
    '                End If

    '                'ProspectPolicies
    '                If r_oParty.ProspectPolicy IsNot Nothing AndAlso r_oParty.ProspectPolicy.Count > 0 Then
    '                    .ClientDetail.ProspectPolicies = New List(Of BaseClasses.BaseClientSharedDataTypeProspectPolicies)
    '                    For iCnt As Integer = 0 To r_oParty.ProspectPolicy.Count - 1
    '                        Dim oProspectPolicies As New BaseClientSharedDataTypeProspectPolicies

    '                        With r_oParty.ProspectPolicy(iCnt)
    '                            oProspectPolicies.ProspectPolicyKey = .ProspectPolicyKey
    '                            oProspectPolicies.ProspectTypeCode = .ProspectTypeCode

    '                            If .RenewalDate <> Date.MinValue Then
    '                                oProspectPolicies.RenewalDate = .RenewalDate
    '                                oProspectPolicies.RenewalDateSpecified = True
    '                            Else
    '                                oProspectPolicies.RenewalDateSpecified = False
    '                            End If

    '                            If .TargetPremium > 0 Then
    '                                oProspectPolicies.TargetPremium = .TargetPremium
    '                                oProspectPolicies.TargetPremiumSpecified = True
    '                            Else
    '                                oProspectPolicies.TargetPremiumSpecified = False
    '                            End If

    '                            If .TimesQuoted > 0 Then
    '                                oProspectPolicies.TimesQuoted = .TimesQuoted
    '                                oProspectPolicies.TimesQuotedSpecified = True
    '                            Else
    '                                oProspectPolicies.TimesQuotedSpecified = False
    '                            End If

    '                        End With
    '                        .ClientDetail.ProspectPolicies.Add(oProspectPolicies)
    '                    Next
    '                End If

    '                .Currency = DirectCast(r_oParty, PersonalParty).Currency

    '                If DirectCast(r_oParty, PersonalParty).DateOfBirthSpecified Then
    '                    .DateOfBirth = DirectCast(r_oParty, PersonalParty).DateOfBirth
    '                End If
    '                .DateOfBirthSpecified = DirectCast(r_oParty, PersonalParty).DateOfBirthSpecified
    '                If DirectCast(r_oParty, PersonalParty).DomiciledForTaxSpecified Then
    '                    .DomiciledForTax = DirectCast(r_oParty, PersonalParty).DomiciledForTax
    '                End If
    '                .DomiciledForTaxSpecified = DirectCast(r_oParty, PersonalParty).DomiciledForTaxSpecified

    '                .EmployersBusinessCode = DirectCast(r_oParty, PersonalParty).EmployersBusinessCode

    '                If DirectCast(r_oParty, PersonalParty).EmploymentStatusCodeSpecified Then
    '                    .EmploymentStatusCode = DirectCast(r_oParty, PersonalParty).EmploymentStatusCode
    '                End If

    '                .EmploymentStatusCodeSpecified = DirectCast(r_oParty, PersonalParty).EmploymentStatusCodeSpecified

    '                If DirectCast(r_oParty, PersonalParty).eMPSSpecified Then
    '                    .EMPS = DirectCast(r_oParty, PersonalParty).eMPS
    '                End If
    '                .EMPSSpecified = DirectCast(r_oParty, PersonalParty).eMPSSpecified
    '                .FileCode = DirectCast(r_oParty, PersonalParty).FileCode

    '                If String.IsNullOrEmpty(DirectCast(r_oParty, PersonalParty).Forename) Then
    '                    Throw New ArgumentNullException("Party.Forename")
    '                Else
    '                    .Forename = DirectCast(r_oParty, PersonalParty).Forename
    '                End If


    '                .GenderCode = DirectCast(r_oParty, PersonalParty).GenderCode

    '                If String.IsNullOrEmpty(DirectCast(r_oParty, PersonalParty).Initials) Then
    '                    Throw New ArgumentNullException("Party.Initials")
    '                Else
    '                    .Initials = DirectCast(r_oParty, PersonalParty).Initials
    '                End If

    '                If DirectCast(r_oParty, PersonalParty).MaritalStatusCodeSpecified Then
    '                    .MaritalStatusCode = DirectCast(r_oParty, PersonalParty).MaritalStatusCode
    '                End If

    '                .MaritalStatusCodeSpecified = DirectCast(r_oParty, PersonalParty).MaritalStatusCodeSpecified
    '                If DirectCast(r_oParty, PersonalParty).MPSSpecified Then
    '                    .MPS = DirectCast(r_oParty, PersonalParty).MPS
    '                End If

    '                .MPSSpecified = DirectCast(r_oParty, PersonalParty).MPSSpecified
    '                .NationalityCode = DirectCast(r_oParty, PersonalParty).NationalityCode
    '                .OccupationCode = DirectCast(r_oParty, PersonalParty).OccupationCode
    '                If DirectCast(r_oParty, PersonalParty).PetOwnerSpecified Then
    '                    .PetOwner = DirectCast(r_oParty, PersonalParty).PetOwner
    '                End If

    '                .PetOwnerSpecified = DirectCast(r_oParty, PersonalParty).PetOwnerSpecified
    '                .Salutation = DirectCast(r_oParty, PersonalParty).Salutation
    '                If String.IsNullOrEmpty(DirectCast(r_oParty, PersonalParty).Lastname) Then
    '                    Throw New ArgumentNullException("Party.Lastname")
    '                Else
    '                    .Surname = DirectCast(r_oParty, PersonalParty).Lastname
    '                End If
    '                .SecEmployersBusinessCode = DirectCast(r_oParty, PersonalParty).SecEmployersBusinessCode

    '                If DirectCast(r_oParty, PersonalParty).SecEmploymentStatusCodeSpecified Then
    '                    .SecEmploymentStatusCode = DirectCast(r_oParty, PersonalParty).SecEmploymentStatusCode
    '                End If

    '                .SecEmploymentStatusCodeSpecified = DirectCast(r_oParty, PersonalParty).SecEmploymentStatusCodeSpecified
    '                .SecOccupationCode = DirectCast(r_oParty, PersonalParty).SecOccupationCode
    '                .Source = DirectCast(r_oParty, PersonalParty).Source
    '                '.TaxExempt = DirectCast(r_oParty, PersonalParty).TaxExempt
    '                If DirectCast(r_oParty, PersonalParty).TaxExemptSpecified Then
    '                    .TaxExempt = DirectCast(r_oParty, PersonalParty).TaxExempt
    '                End If

    '                .TaxExemptSpecified = DirectCast(r_oParty, PersonalParty).TaxExemptSpecified
    '                .TaxNumber = DirectCast(r_oParty, PersonalParty).TaxNumber
    '                If DirectCast(r_oParty, PersonalParty).TaxPercentageSpecified Then
    '                    .TaxPercentage = DirectCast(r_oParty, PersonalParty).TaxPercentage
    '                End If

    '                .TaxPercentageSpecified = DirectCast(r_oParty, PersonalParty).TaxPercentageSpecified
    '                If String.IsNullOrEmpty(DirectCast(r_oParty, PersonalParty).Title) Then
    '                    Throw New ArgumentNullException("Party.Title")
    '                Else
    '                    .Title = DirectCast(r_oParty, PersonalParty).Title
    '                End If
    '                .TPIntroducer = DirectCast(r_oParty, PersonalParty).TPIntroducer
    '                If DirectCast(r_oParty, PersonalParty).TPSSpecified Then
    '                    .TPS = DirectCast(r_oParty, PersonalParty).TPS
    '                End If
    '                .BranchCode = DirectCast(r_oParty, PersonalParty).BranchCode
    '                .XMLDataset = DirectCast(r_oParty, PersonalParty).XMLDataset
    '            End With

    '        Case TypeOf r_oParty Is CorporateParty

    '            oParty = New BaseClasses.BasePartyCCType
    '            With DirectCast(oParty, BaseClasses.BasePartyCCType)
    '                .Currency = DirectCast(r_oParty, CorporateParty).Currency
    '                .AccountExecutive = DirectCast(r_oParty, CorporateParty).AccountExecutive
    '                .AccountExecutiveCode = DirectCast(r_oParty, CorporateParty).AccountExecutiveCode

    '                .AlternativeId = DirectCast(r_oParty, CorporateParty).AlternativeId
    '                .BusinessCode = DirectCast(r_oParty, CorporateParty).BusinessCode
    '                If String.IsNullOrEmpty(DirectCast(r_oParty, CorporateParty).CompanyName) Then
    '                    Throw New ArgumentNullException("Party.CompanyName")
    '                Else
    '                    .CompanyName = DirectCast(r_oParty, CorporateParty).CompanyName
    '                End If
    '                .CompanyReg = DirectCast(r_oParty, CorporateParty).CompanyReg

    '                .Currency = DirectCast(r_oParty, CorporateParty).Currency
    '                If DirectCast(r_oParty, CorporateParty).DomiciledForTaxSpecified Then
    '                    .DomiciledForTax = DirectCast(r_oParty, CorporateParty).DomiciledForTax
    '                End If
    '                .DomiciledForTaxSpecified = DirectCast(r_oParty, CorporateParty).DomiciledForTaxSpecified

    '                Dim oClient As New BaseClientSharedDataType '= Nothing


    '                With DirectCast(r_oParty, CorporateParty).ClientSharedData

    '                    oClient.AccountBalance = .AccountBalance
    '                    oClient.AgentReference = .AgentReference
    '                    oClient.AreaCode = .AreaCode
    '                    oClient.CorrespondenceCode = .CorrespondenceCode
    '                    oClient.CountyCourtJudgments = .CountyCourtJudgments
    '                    oClient.CurrentIntermediaryKey = .CurrentIntermediaryKey
    '                    oClient.CurrentIntermediaryName = .CurrentIntermediaryName
    '                    oClient.IsAgent = .IsAgent
    '                    oClient.IsProspect = False '.IsProspect
    '                    oClient.IsProspectSpecified = False
    '                    oClient.LastYearTurnover = .LastYearTurnover
    '                    oClient.LeadAgentCode = .LeadAgentCode
    '                    If .LeadAgentKey > 0 Then
    '                        oClient.LeadAgentKey = .LeadAgentKey
    '                        oClient.LeadAgentKeySpecified = True
    '                    Else
    '                        oClient.LeadAgentKeySpecified = False
    '                    End If
    '                    oClient.LeadAgentName = .LeadAgentName
    '                    oClient.LoyaltyNumber = .LoyaltyNumber
    '                    oClient.PaymentCode = .PaymentCode
    '                    oClient.PaymentTermCode = .PaymentTermCode
    '                    oClient.PreviousBrokerCode = .PreviousBrokerCode
    '                    oClient.PreviousBrokerKey = .PreviousBrokerKey
    '                    oClient.PreviousBrokerName = .PreviousBrokerName
    '                    oClient.PreviousInsurerCode = .PreviousInsurerCode
    '                    oClient.PreviousInsurerKey = .PreviousInsurerKey
    '                    oClient.PreviousInsurerName = .PreviousInsurerName
    '                    oClient.ReminderCode = .ReminderCode
    '                    oClient.RenewalStopCode = .RenewalStopCode
    '                    oClient.SeasonalGiftCode = .SeasonalGiftCode
    '                    oClient.ServiceLevelCode = .ServiceLevelCode
    '                    oClient.ShortName = .ShortName
    '                    oClient.StatusCode = .StatusCode
    '                    oClient.StrengthCode = .StrengthCode
    '                    oClient.YearToDateTurnover = .YearToDateTurnover
    '                End With

    '                .ClientDetail = oClient

    '                'AssociateType
    '                If r_oParty.Associate IsNot Nothing AndAlso r_oParty.Associate.Count > 0 Then
    '                    .ClientDetail.Associates = New List(Of BaseAssociateType)
    '                    For iCnt As Integer = 0 To r_oParty.Associate.Count - 1
    '                        Dim oAssociate As New BaseAssociateType

    '                        With r_oParty.Associate(iCnt)
    '                            oAssociate.AssociateKey = .AssociateKey
    '                            oAssociate.AssociateCode = .AssociateCode
    '                            oAssociate.AssociateName = .AssociateName
    '                            oAssociate.ClientKey = IIf(r_oParty.Key, r_oParty.Key, .ClientKey)
    '                            oAssociate.RelationshipCode = .RelationshipCode
    '                            oAssociate.RelationshipDescription = .RelationshipDescription
    '                        End With
    '                        .ClientDetail.Associates.Add(oAssociate)
    '                    Next
    '                End If

    '                'Convictions
    '                If r_oParty.Conviction IsNot Nothing AndAlso r_oParty.Conviction.Count > 0 Then
    '                    .ClientDetail.Convictions = New List(Of BaseConvictionType)
    '                    For iCnt As Integer = 0 To r_oParty.Conviction.Count - 1
    '                        Dim oConviction As New BaseConvictionType

    '                        With r_oParty.Conviction(iCnt)
    '                            oConviction.ConvictionKey = .ConvictionKey
    '                            oConviction.Description = .Description
    '                            oConviction.TypeCode = .TypeCode
    '                            oConviction.Date = .ConvictionDate
    '                            If .AlcoholLevel > 0 Then
    '                                oConviction.AlcoholLevel = .AlcoholLevel
    '                                oConviction.AlcoholLevelSpecified = True
    '                            Else
    '                                oConviction.AlcoholLevelSpecified = False
    '                            End If
    '                            oConviction.AlcoholMeasurementMethod = .AlcoholMeasurementMethod

    '                            If .DrivingLicensePenaltyPoints > 0 Then
    '                                oConviction.DrivingLicensePenaltyPoints = .DrivingLicensePenaltyPoints
    '                                oConviction.DrivingLicensePenaltyPointsSpecified = True
    '                            Else
    '                                oConviction.DrivingLicensePenaltyPointsSpecified = False
    '                            End If

    '                            If .FineAmount > 0 Then
    '                                oConviction.FineAmount = .FineAmount
    '                                oConviction.FineAmountSpecified = True
    '                            Else
    '                                oConviction.FineAmountSpecified = False
    '                            End If

    '                            oConviction.SentenceDescription = .SentenceDescription

    '                            If .SentenceDuration > 0 Then
    '                                oConviction.SentenceDuration = .SentenceDuration
    '                                oConviction.SentenceDurationSpecified = True
    '                            Else
    '                                oConviction.SentenceDurationSpecified = False
    '                            End If

    '                            '   oConviction.SentenceDurationQualifier = .SentenceDurationQualifier

    '                            If .SentenceEffectiveDate <> Date.MinValue Then
    '                                oConviction.SentenceEffectiveDate = .SentenceEffectiveDate
    '                                oConviction.SentenceEffectiveDateSpecified = True
    '                            Else
    '                                oConviction.SentenceEffectiveDateSpecified = False
    '                            End If

    '                            oConviction.SentenceTypeCode = .SentenceTypeCode
    '                            oConviction.StatusCode = .StatusCode
    '                        End With

    '                        .ClientDetail.Convictions.Add(oConviction)
    '                    Next
    '                End If

    '                'LoyaltyScheme
    '                If r_oParty.Loyalty IsNot Nothing AndAlso r_oParty.Loyalty.Count > 0 Then
    '                    .ClientDetail.LoyaltyScheme = New List(Of BaseClientSharedDataTypeLoyaltyScheme)
    '                    For iCnt As Integer = 0 To r_oParty.Loyalty.Count - 1
    '                        Dim oLoyalty As New BaseClientSharedDataTypeLoyaltyScheme

    '                        With r_oParty.Loyalty(iCnt)
    '                            oLoyalty.LoyaltySchemeCode = .LoyaltySchemeCode
    '                            oLoyalty.LoyaltySchemeKey = .LoyaltySchemeKey
    '                            oLoyalty.MainMember = .MainMember
    '                            oLoyalty.MembershipNumber = .MembershipNumber
    '                            oLoyalty.OtherReference = .OtherReference
    '                            oLoyalty.Active = .Active
    '                            oLoyalty.ActiveSpecified = .Active
    '                            oLoyalty.StartDate = .StartDate

    '                            If .EndDate <> Date.MinValue Then
    '                                oLoyalty.EndDate = .EndDate
    '                                oLoyalty.EndDateSpecified = True
    '                            Else
    '                                oLoyalty.EndDateSpecified = False
    '                            End If
    '                        End With

    '                        .ClientDetail.LoyaltyScheme.Add(oLoyalty)
    '                    Next
    '                End If

    '                'ProspectPolicies
    '                If r_oParty.ProspectPolicy IsNot Nothing AndAlso r_oParty.ProspectPolicy.Count > 0 Then
    '                    .ClientDetail.ProspectPolicies = New List(Of BaseClientSharedDataTypeProspectPolicies)
    '                    For iCnt As Integer = 0 To r_oParty.ProspectPolicy.Count - 1
    '                        Dim oProspectPolicies As New BaseClientSharedDataTypeProspectPolicies
    '                        With r_oParty.ProspectPolicy(iCnt)
    '                            oProspectPolicies.ProspectPolicyKey = .ProspectPolicyKey
    '                            oProspectPolicies.ProspectTypeCode = .ProspectTypeCode

    '                            If .RenewalDate <> Date.MinValue Then
    '                                oProspectPolicies.RenewalDate = .RenewalDate
    '                                oProspectPolicies.RenewalDateSpecified = True
    '                            Else
    '                                oProspectPolicies.RenewalDateSpecified = False
    '                            End If

    '                            If .TargetPremium > 0 Then
    '                                oProspectPolicies.TargetPremium = .TargetPremium
    '                                oProspectPolicies.TargetPremiumSpecified = True
    '                            Else
    '                                oProspectPolicies.TargetPremiumSpecified = False
    '                            End If

    '                            If .TimesQuoted > 0 Then
    '                                oProspectPolicies.TimesQuoted = .TimesQuoted
    '                                oProspectPolicies.TimesQuotedSpecified = True
    '                            Else
    '                                oProspectPolicies.TimesQuotedSpecified = False
    '                            End If

    '                        End With

    '                        .ClientDetail.ProspectPolicies.Add(oProspectPolicies)
    '                    Next
    '                End If

    '                If DirectCast(r_oParty, CorporateParty).eMPSSpecified Then
    '                    .EMPS = DirectCast(r_oParty, CorporateParty).eMPS
    '                End If
    '                .EMPSSpecified = DirectCast(r_oParty, CorporateParty).eMPSSpecified
    '                .FileCode = DirectCast(r_oParty, CorporateParty).FileCode
    '                If DirectCast(r_oParty, CorporateParty).FinancialYearSpecified Then
    '                    .FinancialYear = DirectCast(r_oParty, CorporateParty).FinancialYear
    '                End If
    '                .FinancialYearSpecified = DirectCast(r_oParty, CorporateParty).FinancialYearSpecified

    '                .MainContact = DirectCast(r_oParty, CorporateParty).MainContact
    '                If DirectCast(r_oParty, CorporateParty).MPSSpecified Then
    '                    .MPS = DirectCast(r_oParty, CorporateParty).MPS
    '                End If
    '                .MPSSpecified = DirectCast(r_oParty, CorporateParty).MPSSpecified

    '                .NumberOfEmployees = DirectCast(r_oParty, CorporateParty).NumberOfEmployees

    '                If DirectCast(r_oParty, CorporateParty).NumberOfOfficesSpecified Then
    '                    .NumberOfOffices = DirectCast(r_oParty, CorporateParty).NumberOfOffices
    '                End If

    '                .NumberOfOfficesSpecified = DirectCast(r_oParty, CorporateParty).NumberOfOfficesSpecified
    '                .Salutation = DirectCast(r_oParty, CorporateParty).Salutation
    '                .SICCode = DirectCast(r_oParty, CorporateParty).SICCode
    '                .Source = DirectCast(r_oParty, CorporateParty).Source
    '                If DirectCast(r_oParty, CorporateParty).TaxExemptSpecified Then
    '                    .TaxExempt = DirectCast(r_oParty, CorporateParty).TaxExempt
    '                End If

    '                .TaxExemptSpecified = DirectCast(r_oParty, CorporateParty).TaxExemptSpecified
    '                .TaxNumber = DirectCast(r_oParty, CorporateParty).TaxNumber
    '                If DirectCast(r_oParty, CorporateParty).TaxPercentageSpecified Then
    '                    .TaxPercentage = DirectCast(r_oParty, CorporateParty).TaxPercentage
    '                End If

    '                .TaxPercentageSpecified = DirectCast(r_oParty, CorporateParty).TaxPercentageSpecified
    '                .TPIntroducer = DirectCast(r_oParty, CorporateParty).TPIntroducer

    '                .TPUserCode = DirectCast(r_oParty, CorporateParty).TPUserCode
    '                .TradeCode = DirectCast(r_oParty, CorporateParty).TradeCode
    '                If DirectCast(r_oParty, CorporateParty).TPSSpecified Then
    '                    .TPS = DirectCast(r_oParty, CorporateParty).TPS
    '                End If

    '                .TPSSpecified = DirectCast(r_oParty, CorporateParty).TPSSpecified
    '                If DirectCast(r_oParty, CorporateParty).TradingsinceSpecified Then
    '                    .TradingSince = DirectCast(r_oParty, CorporateParty).TradingSince
    '                End If
    '                .TradingSinceSpecified = DirectCast(r_oParty, CorporateParty).TradingsinceSpecified
    '                .TurnoverCode = DirectCast(r_oParty, CorporateParty).TurnoverCode
    '                If DirectCast(r_oParty, CorporateParty).WageRollSpecified Then
    '                    .WageRoll = DirectCast(r_oParty, CorporateParty).WageRoll
    '                End If
    '                .WageRollSpecified = DirectCast(r_oParty, CorporateParty).WageRollSpecified
    '                .BranchCode = DirectCast(r_oParty, CorporateParty).BranchCode
    '                .XMLDataset = DirectCast(r_oParty, CorporateParty).XMLDataset
    '            End With

    '        Case TypeOf r_oParty Is OtherParty
    '            oParty = New BasePartyOtherType

    '            With DirectCast(oParty, BasePartyOtherType)

    '                .Name = DirectCast(r_oParty, OtherParty).Name
    '                .Code = DirectCast(r_oParty, OtherParty).Code
    '                .Currency = DirectCast(r_oParty, OtherParty).Currency
    '                .BranchCode = DirectCast(r_oParty, OtherParty).BranchCode
    '                .SubBranchCode = DirectCast(r_oParty, OtherParty).SubBranchCode
    '                .DateOfBirth = DirectCast(r_oParty, OtherParty).DateOfBirth
    '                .FileCode = DirectCast(r_oParty, OtherParty).FileCode
    '                .Gender = DirectCast(r_oParty, OtherParty).Gender
    '                .LicenseTypeCode = DirectCast(r_oParty, OtherParty).LicenseTypeCode
    '                .LicenseNumber = DirectCast(r_oParty, OtherParty).LicenseNumber
    '                .RegNumber = DirectCast(r_oParty, OtherParty).RegistrationNumber
    '                .DriverStatusCode = DirectCast(r_oParty, OtherParty).DriverStatusCode
    '                .IsTPASettleDirectly = DirectCast(r_oParty, OtherParty).IsTPASettleDirectly
    '                .TaxNumber = DirectCast(r_oParty, OtherParty).TaxNumber
    '                .DomiciledForTax = DirectCast(r_oParty, OtherParty).DomiciledForTax
    '                .TypeCode = DirectCast(r_oParty, OtherParty).TypeCode
    '                If DirectCast(r_oParty, OtherParty).TaxExemptSpecified Then
    '                    .TaxExempt = DirectCast(r_oParty, OtherParty).TaxExempt
    '                End If
    '                .TaxExemptSpecified = DirectCast(r_oParty, OtherParty).TaxExemptSpecified
    '                If DirectCast(r_oParty, OtherParty).TaxPercentageSpecified Then
    '                    .TaxPercentage = DirectCast(r_oParty, OtherParty).TaxPercentage
    '                End If
    '                .TaxPercentageSpecified = DirectCast(r_oParty, OtherParty).TaxPercentageSpecified

    '                .XMLDataset = DirectCast(r_oParty, OtherParty).XMLDataset

    '                'Convictions
    '                If r_oParty.Conviction IsNot Nothing AndAlso r_oParty.Conviction.Count > 0 Then
    '                    .Convictions = New List(Of BaseConvictionType)
    '                    For iCnt As Integer = 0 To r_oParty.Conviction.Count - 1
    '                        Dim oConviction As New BaseConvictionType

    '                        With r_oParty.Conviction(iCnt)
    '                            oConviction.ConvictionKey = .ConvictionKey
    '                            oConviction.Description = .Description
    '                            oConviction.TypeCode = .TypeCode
    '                            oConviction.Date = .ConvictionDate
    '                            If .AlcoholLevel > 0 Then
    '                                oConviction.AlcoholLevel = .AlcoholLevel
    '                                oConviction.AlcoholLevelSpecified = True
    '                            Else
    '                                oConviction.AlcoholLevelSpecified = False
    '                            End If
    '                            oConviction.AlcoholMeasurementMethod = .AlcoholMeasurementMethod

    '                            If .DrivingLicensePenaltyPoints > 0 Then
    '                                oConviction.DrivingLicensePenaltyPoints = .DrivingLicensePenaltyPoints
    '                                oConviction.DrivingLicensePenaltyPointsSpecified = True
    '                            Else
    '                                oConviction.DrivingLicensePenaltyPointsSpecified = False
    '                            End If

    '                            If .FineAmount > 0 Then
    '                                oConviction.FineAmount = .FineAmount
    '                                oConviction.FineAmountSpecified = True
    '                            Else
    '                                oConviction.FineAmountSpecified = False
    '                            End If

    '                            oConviction.SentenceDescription = .SentenceDescription

    '                            If .SentenceDuration > 0 Then
    '                                oConviction.SentenceDuration = .SentenceDuration
    '                                oConviction.SentenceDurationSpecified = True
    '                            Else
    '                                oConviction.SentenceDurationSpecified = False
    '                            End If

    '                            ' oConviction.SentenceDurationQualifier = .SentenceDurationQualifier

    '                            If .SentenceEffectiveDate <> Date.MinValue Then
    '                                oConviction.SentenceEffectiveDate = .SentenceEffectiveDate
    '                                oConviction.SentenceEffectiveDateSpecified = True
    '                            Else
    '                                oConviction.SentenceEffectiveDateSpecified = False
    '                            End If

    '                            oConviction.SentenceTypeCode = .SentenceTypeCode
    '                            oConviction.StatusCode = .StatusCode
    '                        End With
    '                        .Convictions.Add(oConviction)
    '                    Next
    '                End If

    '                'Accidents
    '                If r_oParty.Accidents IsNot Nothing AndAlso r_oParty.Accidents.Count > 0 Then
    '                    .Accident = New List(Of BasePartyOtherTypeAccident)
    '                    For iCnt As Integer = 0 To r_oParty.Accidents.Count - 1
    '                        Dim oAccident As New BasePartyOtherTypeAccident
    '                        With r_oParty.Accidents(iCnt)
    '                            oAccident.AccidentKey = .AccidentKey
    '                            oAccident.Date = .AccidentDate
    '                            oAccident.Description = .Description
    '                            oAccident.IsAtFault = .IsAtFault
    '                        End With
    '                        .Accident.Add(oAccident)
    '                    Next
    '                End If

    '                'Supplier Bussiness
    '                If r_oParty.SupplierBusinesses IsNot Nothing AndAlso r_oParty.SupplierBusinesses.Count > 0 Then
    '                    .SupplierBusiness = New List(Of BasePartyOtherTypeSupplierBusiness)
    '                    For iCnt As Integer = 0 To r_oParty.SupplierBusinesses.Count - 1
    '                        Dim oSupplies As New BasePartyOtherTypeSupplierBusiness
    '                        With r_oParty.SupplierBusinesses(iCnt)
    '                            oSupplies.BusinessCode = .BusinessCode
    '                            oSupplies.SpecialityCode = .SpecialityCode
    '                            'oSupplies.ExtensionData = .
    '                        End With
    '                        .SupplierBusiness.Add(oSupplies)
    '                    Next
    '                End If

    '                'Branches
    '                If DirectCast(r_oParty, OtherParty).Branches IsNot Nothing AndAlso DirectCast(r_oParty, OtherParty).Branches.Count > 0 Then
    '                    .Branches = New List(Of BasePartyOtherTypeBranch)
    '                    For iCnt As Integer = 0 To DirectCast(r_oParty, OtherParty).Branches.Count - 1
    '                        Dim oBranch As New BasePartyOtherTypeBranch
    '                        With DirectCast(r_oParty, OtherParty).Branches(iCnt)
    '                            oBranch.BranchId = .BranchKey
    '                            oBranch.Description = .Description
    '                        End With
    '                        .Branches.Add(oBranch)
    '                    Next
    '                End If

    '            End With
    '    End Select


    '    With oParty

    '        .AccountExecutive = r_oParty.AccountExecutive
    '        .AccountExecutiveCode = r_oParty.AccountExecutiveCode
    '        .Currency = r_oParty.Currency

    '        If r_oParty.DomiciledForTaxSpecified Then
    '            .DomiciledForTax = r_oParty.DomiciledForTax
    '        End If

    '        .DomiciledForTaxSpecified = r_oParty.DomiciledForTaxSpecified
    '        .FileCode = r_oParty.FileCode

    '        If r_oParty.TaxExemptSpecified Then
    '            .TaxExempt = r_oParty.TaxExempt
    '        End If

    '        .TaxExemptSpecified = r_oParty.TaxExemptSpecified
    '        .TaxNumber = r_oParty.TaxNumber

    '        If r_oParty.TaxPercentageSpecified Then
    '            .TaxPercentage = r_oParty.TaxPercentage
    '        End If

    '        .TaxPercentageSpecified = r_oParty.TaxPercentageSpecified
    '        .TPIntroducer = r_oParty.TPIntroducer
    '        .TPUserCode = r_oParty.TPUserCode
    '        .XMLDataset = r_oParty.XMLDataset

    '        If r_oParty.Addresses IsNot Nothing Then
    '            .Addresses = New List(Of BaseAddressWithContactsType)
    '            For i As Integer = 0 To r_oParty.Addresses.Count - 1

    '                Dim oAddress As New BaseAddressWithContactsType

    '                With r_oParty.Addresses(i)

    '                    If Not .Address1 Is Nothing Then oAddress.AddressLine1 = .Address1.Trim
    '                    If Not .Address2 Is Nothing Then oAddress.AddressLine2 = .Address2.Trim
    '                    If Not .Address3 Is Nothing Then oAddress.AddressLine3 = .Address3.Trim
    '                    If Not .Address4 Is Nothing Then oAddress.AddressLine4 = .Address4.Trim

    '                    If Not .Address5 Is Nothing Then oAddress.AddressLine5 = .Address5.Trim
    '                    If Not .Address6 Is Nothing Then oAddress.AddressLine6 = .Address6.Trim
    '                    If Not .Address7 Is Nothing Then oAddress.AddressLine7 = .Address7.Trim
    '                    If Not .Address8 Is Nothing Then oAddress.AddressLine8 = .Address8.Trim
    '                    If Not .Address9 Is Nothing Then oAddress.AddressLine9 = .Address9.Trim
    '                    If Not .Address10 Is Nothing Then oAddress.AddressLine10 = .Address10.Trim

    '                    oAddress.AddressTypeCode = .AddressType
    '                    oAddress.CountryCode = .CountryCode
    '                    oAddress.PostCode = .PostCode

    '                End With

    '                .Addresses.Add(oAddress)

    '            Next

    '        End If

    '        If r_oParty.Contacts IsNot Nothing Then
    '            .Contacts = New List(Of BaseContactType)
    '            For i As Integer = 0 To r_oParty.Contacts.Count - 1

    '                Dim oContact As New BaseContactType()

    '                With r_oParty.Contacts(i)

    '                    oContact.AreaCode = .AreaCode
    '                    oContact.ContactDetail = New BaseContactDetailType()
    '                    Select Case .ContactType
    '                        Case ContactType.Email
    '                            oContact.ContactDetail.Item = r_oParty.Contacts(i).Number '.ContactType.Email
    '                            oContact.ContactDetail.ItemElementName = ItemChoiceType.EmailAddress
    '                            Exit Select
    '                        Case ContactType.Fax
    '                            oContact.ContactDetail.Item = r_oParty.Contacts(i).Number ' .ContactType.Fax
    '                            oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
    '                            Exit Select
    '                        Case ContactType.HomePhone
    '                            oContact.ContactDetail.Item = r_oParty.Contacts(i).Number '.ContactType.HomePhone
    '                            oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
    '                            Exit Select
    '                        Case ContactType.Main
    '                            oContact.ContactDetail.Item = r_oParty.Contacts(i).Number ' .ContactType.Main
    '                            oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
    '                            Exit Select
    '                        Case ContactType.Mobile
    '                            oContact.ContactDetail.Item = r_oParty.Contacts(i).Number ' .ContactType.Mobile
    '                            oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
    '                            Exit Select
    '                        Case ContactType.Web
    '                            oContact.ContactDetail.Item = r_oParty.Contacts(i).Number ' .ContactType.Web
    '                            oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
    '                            Exit Select
    '                        Case Else
    '                            oContact.ContactDetail.Item = r_oParty.Contacts(i).Number
    '                            oContact.ContactDetail.ItemElementName = ItemChoiceType.Number
    '                            Exit Select
    '                    End Select

    '                    oContact.ContactTypeCode = .ContactType
    '                    oContact.AreaCode = .AreaCode
    '                    oContact.Description = .Description
    '                    oContact.Extension = .Extension
    '                    oContact.OtherContactTypeCode = .OtherContactTypeCode
    '                End With

    '                .Contacts.Add(oContact)

    '            Next

    '        End If

    '    End With

    '    If Logger.IsLoggingEnabled Then
    '        Dim sbLogMessage As New StringBuilder
    '        sbLogMessage.AppendLine("ConvertParty executed ok" & vbCrLf)
    '        sbLogMessage.AppendLine("Input:" & vbCrLf)
    '        sbLogMessage.AppendLine("r_oParty = " & r_oParty.Print.Replace("<br />", vbCrLf))

    '        sbLogMessage.AppendLine("Returned " & oParty.ToString & vbCrLf)

    '        Dim logEntry As New LogEntry()

    '        logEntry.Categories.Clear()
    '        logEntry.Categories.Add(Category.General)
    '        logEntry.Priority = Priority.Normal
    '        logEntry.Severity = TraceEventType.Verbose
    '        logEntry.Message = sbLogMessage.ToString

    '        Logger.Write(logEntry)
    '    End If


    '    Return oParty

    'End Function
    Private Function ConvertPartyPC(ByVal r_oItem As BasePartyPCType) As BaseParty

        Dim oParty As BaseParty

        Select Case (True)
            Case TypeOf r_oItem Is BasePartyPCType

                oParty = New PersonalParty()
                With DirectCast(r_oItem, BasePartyPCType)

                    DirectCast(oParty, PersonalParty).Currency = .Currency
                    DirectCast(oParty, PersonalParty).AlternativeID = .AlternativeId
                    '.BranchCode
                    DirectCast(oParty, PersonalParty).DateOfBirth = .DateOfBirth
                    DirectCast(oParty, PersonalParty).EmployersBusinessCode = .EmployersBusinessCode

                    DirectCast(oParty, PersonalParty).NationalityCode = .NationalityCode

                    If .EmploymentStatusCodeSpecified Then
                        DirectCast(oParty, PersonalParty).EmploymentStatusCode = .EmploymentStatusCode
                    End If

                    DirectCast(oParty, PersonalParty).Forename = .Forename.Trim()
                    DirectCast(oParty, PersonalParty).GenderCode = .GenderCode
                    DirectCast(oParty, PersonalParty).Initials = .Initials.Trim()

                    DirectCast(oParty, PersonalParty).MaritalStatusCode = MaritalStatusCodeTypes.NotAvailable
                    If .MaritalStatusCodeSpecified Then
                        DirectCast(oParty, PersonalParty).MaritalStatusCode = .MaritalStatusCode
                    End If

                    DirectCast(oParty, PersonalParty).BranchCode = .BranchCode

                    DirectCast(oParty, PersonalParty).OccupationCode = .OccupationCode
                    DirectCast(oParty, PersonalParty).SecOccupationCode = .SecOccupationCode
                    DirectCast(oParty, PersonalParty).Lastname = .Surname.Trim()
                    DirectCast(oParty, PersonalParty).Title = .Title.Trim()
                    DirectCast(oParty, PersonalParty).FileCode = .FileCode

                    If .ClientDetail IsNot Nothing Then
                        DirectCast(oParty, PersonalParty).ServiceLevelCode = .ClientDetail.ServiceLevelCode
                        DirectCast(oParty, PersonalParty).BlacklistReasonCode = .ClientDetail.BlacklistReasonCode
                        DirectCast(oParty, PersonalParty).RenewalStopCode = .ClientDetail.RenewalStopCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.AccountBalance = .ClientDetail.AccountBalance
                        DirectCast(oParty, PersonalParty).ClientSharedData.AgentReference = .ClientDetail.AgentReference
                        DirectCast(oParty, PersonalParty).ClientSharedData.AreaCode = .ClientDetail.AreaCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.CorrespondenceCode = .ClientDetail.CorrespondenceCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.CountyCourtJudgments = .ClientDetail.CountyCourtJudgments
                        DirectCast(oParty, PersonalParty).ClientSharedData.CurrentIntermediaryKey = .ClientDetail.CurrentIntermediaryKey
                        DirectCast(oParty, PersonalParty).ClientSharedData.CurrentIntermediaryName = .ClientDetail.CurrentIntermediaryName
                        DirectCast(oParty, PersonalParty).ClientSharedData.IsAgent = .ClientDetail.IsAgent

                        DirectCast(oParty, PersonalParty).ClientSharedData.IsProspect = False ' .ClientDetail.IsProspect
                        DirectCast(oParty, PersonalParty).ClientSharedData.LastYearTurnover = .ClientDetail.LastYearTurnover
                        DirectCast(oParty, PersonalParty).ClientSharedData.LeadAgentCode = .ClientDetail.LeadAgentCode
                        If .ClientDetail.LeadAgentKey > 0 Then
                            DirectCast(oParty, PersonalParty).ClientSharedData.LeadAgentKey = .ClientDetail.LeadAgentKey
                        End If
                        DirectCast(oParty, PersonalParty).ClientSharedData.LeadAgentName = .ClientDetail.LeadAgentName
                        DirectCast(oParty, PersonalParty).ClientSharedData.LoyaltyNumber = .ClientDetail.LoyaltyNumber
                        DirectCast(oParty, PersonalParty).ClientSharedData.PaymentCode = .ClientDetail.PaymentCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.PaymentTermCode = .ClientDetail.PaymentTermCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.PreviousBrokerCode = .ClientDetail.PreviousBrokerCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.PreviousBrokerKey = .ClientDetail.PreviousBrokerKey
                        DirectCast(oParty, PersonalParty).ClientSharedData.PreviousBrokerName = .ClientDetail.PreviousBrokerName
                        DirectCast(oParty, PersonalParty).ClientSharedData.PreviousInsurerCode = .ClientDetail.PreviousInsurerCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.PreviousInsurerKey = .ClientDetail.PreviousInsurerKey
                        DirectCast(oParty, PersonalParty).ClientSharedData.PreviousInsurerName = .ClientDetail.PreviousInsurerName
                        DirectCast(oParty, PersonalParty).ClientSharedData.ReminderCode = .ClientDetail.ReminderCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.RenewalStopCode = .ClientDetail.RenewalStopCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.SeasonalGiftCode = .ClientDetail.SeasonalGiftCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.ServiceLevelCode = .ClientDetail.ServiceLevelCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.ShortName = .ClientDetail.ShortName
                        If oParty.UserName Is Nothing OrElse (oParty.UserName IsNot Nothing AndAlso String.IsNullOrEmpty(oParty.UserName)) Then
                            oParty.UserName = .ClientDetail.ShortName
                        End If
                        DirectCast(oParty, PersonalParty).ClientSharedData.ResolvedName = .ClientDetail.ResolvedName
                        DirectCast(oParty, PersonalParty).ClientSharedData.StatusCode = .ClientDetail.StatusCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.StrengthCode = .ClientDetail.StrengthCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.YearToDateTurnover = .ClientDetail.YearToDateTurnover
                        DirectCast(oParty, PersonalParty).ClientSharedData.BlacklistReasonCode = .ClientDetail.BlacklistReasonCode
                        DirectCast(oParty, PersonalParty).ClientSharedData.RenewalStopCode = .ClientDetail.RenewalStopCode
                    End If

                    'LifeStyle (only for PCType)
                    If .Lifestyle IsNot Nothing AndAlso .Lifestyle.Count > 0 Then

                        oParty.Lifestyle = New LifestyleCollection

                        Dim oNewLifeStlye As New Lifestyle

                        For Each oLifeStyle As BasePartyPCTypeLifestyle In .Lifestyle
                            If oLifeStyle.CategoryCode.Trim.ToUpper <> "INSURED" Then
                                oNewLifeStlye = New Lifestyle()

                                With oNewLifeStlye
                                    .LifestyleKey = oLifeStyle.LifestyleKey
                                    .Name = oLifeStyle.Name
                                    .CategoryCode = oLifeStyle.CategoryCode
                                    .OccupationCode = oLifeStyle.OccupationCode
                                    .SecOccupationCode = oLifeStyle.SecOccupationCode
                                    .Smoker = oLifeStyle.Smoker
                                    If oLifeStyle.DateOfBirth <> Date.MinValue Then
                                        .DateOfBirth = oLifeStyle.DateOfBirth
                                    End If
                                    Select Case oLifeStyle.GenderCode
                                        Case GenderCodeType.Male
                                            .GenderCode = "Male"
                                        Case GenderCodeType.Female
                                            .GenderCode = "Female"
                                    End Select
                                End With
                                oParty.Lifestyle.Add(oNewLifeStlye)
                            End If
                        Next
                    End If

                    'Associate
                    If .ClientDetail IsNot Nothing AndAlso .ClientDetail.Associates IsNot Nothing Then
                        oParty.Associate = New AssociateCollection

                        Dim oNewAssociate As New Associate

                        For Each oAssociate As BaseAssociateType In .ClientDetail.Associates
                            oNewAssociate = New Associate()

                            With oNewAssociate
                                .AssociateKey = oAssociate.AssociateKey
                                .AssociateCode = oAssociate.AssociateCode
                                .AssociateName = oAssociate.AssociateName
                                .RelationshipCode = oAssociate.RelationshipCode
                                .RelationshipDescription = oAssociate.RelationshipDescription
                                .AccountBalance = oAssociate.AccountBalance
                                .ClaimIncurred = oAssociate.ClaimIncurred
                                .CurrencyCode = oAssociate.CurrencyCode
                            End With

                            oParty.Associate.Add(oNewAssociate)
                        Next
                    End If


                    'Convictions
                    If .ClientDetail IsNot Nothing AndAlso .ClientDetail.Convictions IsNot Nothing AndAlso .ClientDetail.Convictions.Count > 0 Then
                        oParty.Conviction = New ConvictionCollection

                        Dim oNewConviction As New Convictions

                        For Each oConviction As BaseConvictionType In .ClientDetail.Convictions
                            oNewConviction = New Convictions()

                            With oNewConviction
                                .ConvictionKey = oConviction.ConvictionKey
                                .Description = oConviction.Description
                                .TypeCode = oConviction.TypeCode
                                .ConvictionDate = oConviction.Date

                                .AlcoholLevel = oConviction.AlcoholLevel
                                .AlcoholMeasurementMethod = oConviction.AlcoholMeasurementMethod
                                .FineAmount = oConviction.FineAmount
                                .DrivingLicensePenaltyPoints = oConviction.DrivingLicensePenaltyPoints

                                .SentenceTypeCode = oConviction.SentenceTypeCode
                                .StatusCode = oConviction.StatusCode
                                .SentenceDescription = oConviction.SentenceDescription
                                .SentenceDuration = oConviction.SentenceDuration
                                .SentenceDurationQualifier = oConviction.SentenceDurationQualifier
                                If oConviction.SentenceEffectiveDate <> Date.MinValue Then
                                    .SentenceEffectiveDate = oConviction.SentenceEffectiveDate
                                End If
                            End With

                            oParty.Conviction.Add(oNewConviction)

                        Next
                    End If


                    'LoyaltySchemes
                    If .ClientDetail IsNot Nothing AndAlso .ClientDetail.LoyaltyScheme IsNot Nothing AndAlso .ClientDetail.LoyaltyScheme.Count > 0 Then
                        oParty.Loyalty = New LoyaltyCollection
                        Dim oNewLoyalty As New Loyalty

                        For Each oLoyalty As BaseClientSharedDataTypeLoyaltyScheme In .ClientDetail.LoyaltyScheme
                            oNewLoyalty = New Loyalty()

                            With oNewLoyalty
                                .LoyaltySchemeKey = oLoyalty.LoyaltySchemeKey
                                .LoyaltySchemeCode = oLoyalty.LoyaltySchemeCode
                                .MainMember = oLoyalty.MainMember
                                .MembershipNumber = oLoyalty.MembershipNumber
                                .OtherReference = oLoyalty.OtherReference
                                .StartDate = oLoyalty.StartDate
                                .EndDate = oLoyalty.EndDate
                                .Active = oLoyalty.Active
                            End With

                            oParty.Loyalty.Add(oNewLoyalty)
                        Next
                    End If


                    'Prospect Policies
                    If .ClientDetail IsNot Nothing AndAlso .ClientDetail.ProspectPolicies IsNot Nothing AndAlso .ClientDetail.ProspectPolicies.Count > 0 Then
                        oParty.ProspectPolicy = New ProspectPolicyCollection
                        Dim oNewProspectPolicy As New ProspectPolicies

                        For Each oProspectPolicy As BaseClientSharedDataTypeProspectPolicies In .ClientDetail.ProspectPolicies
                            oNewProspectPolicy = New ProspectPolicies()

                            With oNewProspectPolicy
                                .ProspectPolicyKey = oProspectPolicy.ProspectPolicyKey
                                .ProspectTypeCode = oProspectPolicy.ProspectTypeCode
                                .RenewalDate = oProspectPolicy.RenewalDate
                                .TargetPremium = oProspectPolicy.TargetPremium
                                .TimesQuoted = oProspectPolicy.TimesQuoted
                            End With

                            oParty.ProspectPolicy.Add(oNewProspectPolicy)
                        Next
                    End If

                End With
            Case Else
                Throw New ProviderException("Party type " & r_oItem.GetType.ToString & " not supported by provider.")
        End Select

        oParty.AccountExecutive = r_oItem.AccountExecutive
        oParty.Addresses = New AddressCollection

        If r_oItem.Addresses IsNot Nothing AndAlso r_oItem.Addresses.Count > 0 Then

            Dim oNewAddress As Address
            For Each oAddress As BaseAddressWithContactsType In r_oItem.Addresses

                oNewAddress = New Address()

                With oNewAddress
                    .Address1 = oAddress.AddressLine1.Trim()
                    .Address2 = oAddress.AddressLine2.Trim()
                    .Address3 = oAddress.AddressLine3.Trim()
                    .Address4 = oAddress.AddressLine4.Trim()

                    .AddressType = oAddress.AddressTypeCode
                    .CountryCode = oAddress.CountryCode.Trim()
                    .PostCode = oAddress.PostCode.Trim()
                    .Address5 = oAddress.AddressLine5.Trim()
                    .Address6 = oAddress.AddressLine6.Trim()
                    .Address7 = oAddress.AddressLine7.Trim()
                    .Address8 = oAddress.AddressLine8.Trim()
                    .Address9 = oAddress.AddressLine9.Trim()
                    .Address10 = oAddress.AddressLine10.Trim()

                End With

                oParty.Addresses.Add(oNewAddress)

            Next

        End If

        oParty.Contacts = New ContactCollection

        If r_oItem.Contacts IsNot Nothing AndAlso r_oItem.Contacts.Count > 0 Then

            Dim oNewContact As Contact

            For Each oContact As BaseContactType In r_oItem.Contacts

                oNewContact = New Contact()

                With oNewContact
                    .AreaCode = oContact.AreaCode.Trim()
                    .ContactType = oContact.ContactTypeCode
                    .Description = oContact.Description
                    .Extension = oContact.Extension
                    .ContactTypeDescription = oContact.ContactTypeDescription
                    If oContact.OtherContactTypeCode <> String.Empty Then
                        .OtherContactTypeCode = oContact.OtherContactTypeCode
                    Else
                        .OtherContactTypeCode = CType(oContact.ContactTypeCode, ContactType).ToString()
                    End If
                    Select Case oContact.ContactTypeCode
                        Case ContactTypeType.EMAIL
                            .ContactDetailType = ItemChoiceTypes.EmailAddress
                        Case Else
                            .ContactDetailType = ItemChoiceTypes.Number
                    End Select

                    .Number = oContact.ContactDetail.Item.Trim()

                End With

                oParty.Contacts.Add(oNewContact)

            Next
        End If

        oParty.Currency = r_oItem.Currency
        oParty.TPIntroducer = r_oItem.TPIntroducer
        oParty.TPUserCode = r_oItem.TPUserCode

        If r_oItem.DomiciledForTaxSpecified Then
            oParty.DomiciledForTax = r_oItem.DomiciledForTax
        End If

        If r_oItem.TaxExemptSpecified Then
            oParty.TaxExempt = r_oItem.TaxExempt
        End If

        oParty.TaxNumber = r_oItem.TaxNumber

        If r_oItem.TaxPercentageSpecified Then
            oParty.TaxPercentage = r_oItem.TaxPercentage
        End If

        If Logger.IsLoggingEnabled Then
            Dim sbLogMessage As New StringBuilder
            sbLogMessage.AppendLine("Convertparty executed ok" & vbCrLf)
            sbLogMessage.AppendLine("Input:" & vbCrLf)
            sbLogMessage.AppendLine("r_oItem = " & r_oItem.ToString & vbCrLf)

            sbLogMessage.AppendLine("Returned " & oParty.Print.Replace("<br />", vbCrLf))

            Dim logEntry As New LogEntry()

            logEntry.Categories.Clear()
            logEntry.Categories.Add(Category.General)
            logEntry.Priority = Priority.Normal
            logEntry.Severity = TraceEventType.Verbose
            logEntry.Message = sbLogMessage.ToString

            Logger.Write(logEntry)
        End If

        Return oParty

    End Function
    Private Function ConvertpartyCC(ByVal r_oItem As BasePartyType) As BaseParty

        Dim oParty As BaseParty

        If TypeOf r_oItem Is BasePartyCCType Then

            oParty = New CorporateParty()

            With DirectCast(r_oItem, BasePartyCCType)
                DirectCast(oParty, CorporateParty).Currency = .Currency
                DirectCast(oParty, CorporateParty).BusinessCode = .BusinessCode
                DirectCast(oParty, CorporateParty).CompanyName = .CompanyName
                DirectCast(oParty, CorporateParty).MainContact = .MainContact
                DirectCast(oParty, CorporateParty).CompanyReg = .CompanyReg
                DirectCast(oParty, CorporateParty).NumberOfEmployees = .NumberOfEmployees
                DirectCast(oParty, CorporateParty).FileCode = .FileCode
                DirectCast(oParty, CorporateParty).BranchCode = .BranchCode
                If .NumberOfOfficesSpecified Then
                    DirectCast(oParty, CorporateParty).NumberOfOffices = .NumberOfOffices
                End If

                If .ClientDetail IsNot Nothing Then
                    DirectCast(oParty, CorporateParty).ServiceLevelCode = .ClientDetail.ServiceLevelCode
                    DirectCast(oParty, CorporateParty).BlacklistReasonCode = .ClientDetail.BlacklistReasonCode
                    DirectCast(oParty, CorporateParty).RenewalStopCode = .ClientDetail.RenewalStopCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.AccountBalance = .ClientDetail.AccountBalance

                    DirectCast(oParty, CorporateParty).ClientSharedData.AgentReference = .ClientDetail.AgentReference
                    DirectCast(oParty, CorporateParty).ClientSharedData.AreaCode = .ClientDetail.AreaCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.CorrespondenceCode = .ClientDetail.CorrespondenceCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.CountyCourtJudgments = .ClientDetail.CountyCourtJudgments
                    DirectCast(oParty, CorporateParty).ClientSharedData.CurrentIntermediaryKey = .ClientDetail.CurrentIntermediaryKey
                    DirectCast(oParty, CorporateParty).ClientSharedData.CurrentIntermediaryName = .ClientDetail.CurrentIntermediaryName
                    DirectCast(oParty, CorporateParty).ClientSharedData.IsAgent = .ClientDetail.IsAgent

                    DirectCast(oParty, CorporateParty).ClientSharedData.IsProspect = False ' .ClientDetail.IsProspect
                    DirectCast(oParty, CorporateParty).ClientSharedData.LastYearTurnover = .ClientDetail.LastYearTurnover
                    DirectCast(oParty, CorporateParty).ClientSharedData.LeadAgentCode = .ClientDetail.LeadAgentCode
                    If .ClientDetail.LeadAgentKey > 0 Then
                        DirectCast(oParty, CorporateParty).ClientSharedData.LeadAgentKey = .ClientDetail.LeadAgentKey
                    End If
                    DirectCast(oParty, CorporateParty).ClientSharedData.LeadAgentName = .ClientDetail.LeadAgentName
                    DirectCast(oParty, CorporateParty).ClientSharedData.LoyaltyNumber = .ClientDetail.LoyaltyNumber
                    DirectCast(oParty, CorporateParty).ClientSharedData.PaymentCode = .ClientDetail.PaymentCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.PaymentTermCode = .ClientDetail.PaymentTermCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.PreviousBrokerCode = .ClientDetail.PreviousBrokerCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.PreviousBrokerKey = .ClientDetail.PreviousBrokerKey
                    DirectCast(oParty, CorporateParty).ClientSharedData.PreviousBrokerName = .ClientDetail.PreviousBrokerName
                    DirectCast(oParty, CorporateParty).ClientSharedData.PreviousInsurerCode = .ClientDetail.PreviousInsurerCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.PreviousInsurerKey = .ClientDetail.PreviousInsurerKey
                    DirectCast(oParty, CorporateParty).ClientSharedData.PreviousInsurerName = .ClientDetail.PreviousInsurerName
                    DirectCast(oParty, CorporateParty).ClientSharedData.ReminderCode = .ClientDetail.ReminderCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.RenewalStopCode = .ClientDetail.RenewalStopCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.SeasonalGiftCode = .ClientDetail.SeasonalGiftCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.ServiceLevelCode = .ClientDetail.ServiceLevelCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.ShortName = .ClientDetail.ShortName
                    DirectCast(oParty, CorporateParty).ClientSharedData.ResolvedName = .ClientDetail.ResolvedName
                    DirectCast(oParty, CorporateParty).ClientSharedData.StatusCode = .ClientDetail.StatusCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.StrengthCode = .ClientDetail.StrengthCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.YearToDateTurnover = .ClientDetail.YearToDateTurnover
                    DirectCast(oParty, CorporateParty).ClientSharedData.BlacklistReasonCode = .ClientDetail.BlacklistReasonCode
                    DirectCast(oParty, CorporateParty).ClientSharedData.RenewalStopCode = .ClientDetail.RenewalStopCode

                End If

                'Associate
                If .ClientDetail IsNot Nothing AndAlso .ClientDetail.Associates IsNot Nothing AndAlso .ClientDetail.Associates.Count > 0 Then

                    oParty.Associate = New AssociateCollection

                    Dim oNewAssociate As New Associate

                    For Each oAssociate As BaseAssociateType In .ClientDetail.Associates
                        oNewAssociate = New Associate()

                        With oNewAssociate
                            .AssociateKey = oAssociate.AssociateKey
                            .AssociateCode = oAssociate.AssociateCode
                            .AssociateName = oAssociate.AssociateName
                            .RelationshipCode = oAssociate.RelationshipCode
                            .RelationshipDescription = oAssociate.RelationshipDescription
                            .AccountBalance = oAssociate.AccountBalance
                            .ClaimIncurred = oAssociate.ClaimIncurred
                            .CurrencyCode = oAssociate.CurrencyCode
                        End With

                        oParty.Associate.Add(oNewAssociate)
                    Next
                End If

                'Convictions
                If .ClientDetail IsNot Nothing AndAlso .ClientDetail.Convictions IsNot Nothing AndAlso .ClientDetail.Convictions.Count > 0 Then

                    oParty.Conviction = New ConvictionCollection

                    Dim oNewConviction As New Convictions

                    For Each oConviction As BaseConvictionType In .ClientDetail.Convictions
                        oNewConviction = New Convictions()

                        With oNewConviction
                            .ConvictionKey = oConviction.ConvictionKey
                            .Description = oConviction.Description
                            .TypeCode = oConviction.TypeCode
                            .ConvictionDate = oConviction.Date

                            .AlcoholLevel = oConviction.AlcoholLevel
                            .AlcoholMeasurementMethod = oConviction.AlcoholMeasurementMethod
                            .FineAmount = oConviction.FineAmount
                            .DrivingLicensePenaltyPoints = oConviction.DrivingLicensePenaltyPoints


                            .SentenceTypeCode = oConviction.SentenceTypeCode
                            .StatusCode = oConviction.StatusCode
                            .SentenceDescription = oConviction.SentenceDescription
                            .SentenceDuration = oConviction.SentenceDuration
                            .SentenceDurationQualifier = oConviction.SentenceDurationQualifier
                            .SentenceEffectiveDate = oConviction.SentenceEffectiveDate

                        End With

                        oParty.Conviction.Add(oNewConviction)

                    Next
                End If


                'LoyaltySchemes
                If .ClientDetail IsNot Nothing AndAlso .ClientDetail.LoyaltyScheme IsNot Nothing AndAlso .ClientDetail.LoyaltyScheme.Count > 0 Then

                    oParty.Loyalty = New LoyaltyCollection
                    Dim oNewLoyalty As New Loyalty

                    For Each oLoyalty As BaseClientSharedDataTypeLoyaltyScheme In .ClientDetail.LoyaltyScheme
                        oNewLoyalty = New Loyalty()

                        With oNewLoyalty
                            .LoyaltySchemeKey = oLoyalty.LoyaltySchemeKey
                            .LoyaltySchemeCode = oLoyalty.LoyaltySchemeCode
                            .MainMember = oLoyalty.MainMember
                            .MembershipNumber = oLoyalty.MembershipNumber
                            .OtherReference = oLoyalty.OtherReference
                            .StartDate = oLoyalty.StartDate
                            .EndDate = oLoyalty.EndDate
                            .Active = oLoyalty.Active
                        End With

                        oParty.Loyalty.Add(oNewLoyalty)
                    Next
                End If


                'Prospect Policies
                If .ClientDetail IsNot Nothing AndAlso .ClientDetail.ProspectPolicies IsNot Nothing AndAlso .ClientDetail.ProspectPolicies.Count > 0 Then

                    oParty.ProspectPolicy = New ProspectPolicyCollection
                    Dim oNewProspectPolicy As New ProspectPolicies

                    For Each oProspectPolicy As BaseClientSharedDataTypeProspectPolicies In .ClientDetail.ProspectPolicies
                        oNewProspectPolicy = New ProspectPolicies()

                        With oNewProspectPolicy
                            .ProspectPolicyKey = oProspectPolicy.ProspectPolicyKey
                            .ProspectTypeCode = oProspectPolicy.ProspectTypeCode
                            .RenewalDate = oProspectPolicy.RenewalDate
                            .TargetPremium = oProspectPolicy.TargetPremium
                            .TimesQuoted = oProspectPolicy.TimesQuoted
                        End With

                        oParty.ProspectPolicy.Add(oNewProspectPolicy)
                    Next
                End If
            End With
        Else
            Throw New ProviderException("Party type " & r_oItem.GetType.ToString & " not supported by provider.")
        End If

        oParty.AccountExecutive = r_oItem.AccountExecutive
        oParty.Addresses = New AddressCollection

        If r_oItem.Addresses IsNot Nothing AndAlso r_oItem.Addresses.Count > 0 Then

            Dim oNewAddress As Address
            For Each oAddress As BaseAddressWithContactsType In r_oItem.Addresses

                oNewAddress = New Address()

                With oNewAddress
                    .Address1 = oAddress.AddressLine1.Trim()
                    .Address2 = oAddress.AddressLine2.Trim()
                    .Address3 = oAddress.AddressLine3.Trim()
                    .Address4 = oAddress.AddressLine4.Trim()

                    .AddressType = oAddress.AddressTypeCode
                    .CountryCode = oAddress.CountryCode.Trim()
                    .PostCode = oAddress.PostCode.Trim()
                    .Address5 = oAddress.AddressLine5.Trim()
                    .Address6 = oAddress.AddressLine6.Trim()
                    .Address7 = oAddress.AddressLine7.Trim()
                    .Address8 = oAddress.AddressLine8.Trim()
                    .Address9 = oAddress.AddressLine9.Trim()
                    .Address10 = oAddress.AddressLine10.Trim()

                End With

                oParty.Addresses.Add(oNewAddress)

            Next

        End If

        oParty.Contacts = New ContactCollection

        If r_oItem.Contacts IsNot Nothing AndAlso r_oItem.Contacts.Count > 0 Then

            Dim oNewContact As Contact

            For Each oContact As BaseContactType In r_oItem.Contacts

                oNewContact = New Contact()

                With oNewContact
                    .AreaCode = oContact.AreaCode.Trim()
                    .ContactType = oContact.ContactTypeCode
                    .Description = oContact.Description
                    .Extension = oContact.Extension
                    .ContactTypeDescription = oContact.ContactTypeDescription
                    If oContact.OtherContactTypeCode <> String.Empty Then
                        .OtherContactTypeCode = oContact.OtherContactTypeCode
                    Else
                        .OtherContactTypeCode = CType(oContact.ContactTypeCode, ContactType).ToString()
                    End If
                    Select Case oContact.ContactTypeCode
                        Case ContactTypeType.EMAIL
                            .ContactDetailType = ItemChoiceTypes.EmailAddress
                        Case Else
                            .ContactDetailType = ItemChoiceTypes.Number
                    End Select

                    .Number = oContact.ContactDetail.Item.Trim()

                End With

                oParty.Contacts.Add(oNewContact)

            Next
        End If

        oParty.Currency = r_oItem.Currency
        oParty.TPIntroducer = r_oItem.TPIntroducer
        oParty.TPUserCode = r_oItem.TPUserCode

        If r_oItem.DomiciledForTaxSpecified Then
            oParty.DomiciledForTax = r_oItem.DomiciledForTax
        End If

        If r_oItem.TaxExemptSpecified Then
            oParty.TaxExempt = r_oItem.TaxExempt
        End If

        oParty.TaxNumber = r_oItem.TaxNumber

        If r_oItem.TaxPercentageSpecified Then
            oParty.TaxPercentage = r_oItem.TaxPercentage
        End If

        If Logger.IsLoggingEnabled Then
            Dim sbLogMessage As New StringBuilder
            sbLogMessage.AppendLine("Convertparty executed ok" & vbCrLf)
            sbLogMessage.AppendLine("Input:" & vbCrLf)
            sbLogMessage.AppendLine("r_oItem = " & r_oItem.ToString & vbCrLf)

            sbLogMessage.AppendLine("Returned " & oParty.Print.Replace("<br />", vbCrLf))

            Dim logEntry As New LogEntry()

            logEntry.Categories.Clear()
            logEntry.Categories.Add(Category.General)
            logEntry.Priority = Priority.Normal
            logEntry.Severity = TraceEventType.Verbose
            logEntry.Message = sbLogMessage.ToString

            Logger.Write(logEntry)
        End If

        Return oParty

    End Function
    Private Function ConvertpartyOther(ByVal r_oItem As BasePartyType) As BaseParty

        Dim oParty As BaseParty

        If TypeOf r_oItem Is BasePartyOtherType Then
            oParty = New OtherParty()
            With DirectCast(r_oItem, BasePartyOtherType)
                DirectCast(oParty, OtherParty).Name = .Name
                DirectCast(oParty, OtherParty).ShortName = .Code
                DirectCast(oParty, OtherParty).DOB = .DateOfBirth.ToString()
                DirectCast(oParty, OtherParty).DateOfBirth = .DateOfBirth
                DirectCast(oParty, OtherParty).Code = .Name
                DirectCast(oParty, OtherParty).Gender = .Gender
                DirectCast(oParty, OtherParty).LicenseTypeCode = .LicenseTypeCode
                DirectCast(oParty, OtherParty).LicenseNumber = .LicenseNumber
                DirectCast(oParty, OtherParty).DriverStatusCode = .DriverStatusCode
                DirectCast(oParty, OtherParty).RegistrationNumber = .RegNumber
                DirectCast(oParty, OtherParty).BranchCode = .BranchCode
                DirectCast(oParty, OtherParty).SubBranchCode = .SubBranchCode
                DirectCast(oParty, OtherParty).Currency = .Currency
                DirectCast(oParty, OtherParty).IsTPASettleDirectly = IIf(String.IsNullOrEmpty(.IsTPASettleDirectly) OrElse .IsTPASettleDirectly.Trim.ToUpper = "FALSE", 0, 1)

                If .Convictions IsNot Nothing AndAlso .Convictions.Count > 0 Then
                    oParty.Conviction = New ConvictionCollection

                    Dim oNewConviction As New Convictions

                    For Each oConviction As BaseConvictionType In .Convictions
                        oNewConviction = New Convictions()

                        With oNewConviction
                            .ConvictionKey = oConviction.ConvictionKey
                            .Description = oConviction.Description
                            .TypeCode = oConviction.TypeCode
                            .ConvictionDate = oConviction.Date

                            .AlcoholLevel = oConviction.AlcoholLevel
                            .AlcoholMeasurementMethod = oConviction.AlcoholMeasurementMethod
                            .FineAmount = oConviction.FineAmount
                            .DrivingLicensePenaltyPoints = oConviction.DrivingLicensePenaltyPoints

                            .SentenceTypeCode = oConviction.SentenceTypeCode
                            .StatusCode = oConviction.StatusCode
                            .SentenceDescription = oConviction.SentenceDescription
                            .SentenceDuration = oConviction.SentenceDuration
                            .SentenceDurationQualifier = oConviction.SentenceDurationQualifier
                            If oConviction.SentenceEffectiveDate <> Date.MinValue Then
                                .SentenceEffectiveDate = oConviction.SentenceEffectiveDate
                            End If
                        End With

                        oParty.Conviction.Add(oNewConviction)

                    Next
                End If

                If .Accident IsNot Nothing AndAlso .Accident.Count > 0 Then
                    oParty.Accidents = New AccidentCollection

                    Dim oNewAccident As New Accident
                    For Each oAccident As BasePartyOtherTypeAccident In .Accident
                        oNewAccident = New Accident()

                        With oNewAccident
                            .AccidentKey = oAccident.AccidentKey
                            .AccidentDate = oAccident.Date
                            .Description = oAccident.Description
                            .IsAtFault = oAccident.IsAtFault
                        End With
                        oParty.Accidents.Add(oNewAccident)

                    Next
                End If

                If .SupplierBusiness IsNot Nothing AndAlso .SupplierBusiness.Count > 0 Then
                    oParty.SupplierBusinesses = New SupplierBusinessCollection

                    Dim oNewSupplierBusiness As New SupplierBusiness

                    For Each oSupplierBusiness As BasePartyOtherTypeSupplierBusiness In .SupplierBusiness
                        oNewSupplierBusiness = New SupplierBusiness()

                        With oNewSupplierBusiness
                            .BusinessCode = oSupplierBusiness.BusinessCode
                            .SpecialityCode = oSupplierBusiness.SpecialityCode
                        End With
                        oParty.SupplierBusinesses.Add(oNewSupplierBusiness)

                    Next
                End If

                If .Branches IsNot Nothing AndAlso .Branches.Count > 0 Then
                    DirectCast(oParty, OtherParty).Branches = New BranchCollection

                    Dim oNewBranch As New Branch

                    For Each oBranch As BasePartyOtherTypeBranch In .Branches
                        oNewBranch = New Branch()

                        With oNewBranch
                            .BranchKey = oBranch.BranchId
                            .Description = oBranch.Description
                        End With
                        DirectCast(oParty, OtherParty).Branches.Add(oNewBranch)

                    Next
                End If

            End With
        Else
            Throw New ProviderException("Party type " & r_oItem.GetType.ToString & " not supported by provider.")
        End If

        oParty.AccountExecutive = r_oItem.AccountExecutive
        oParty.Addresses = New AddressCollection

        If r_oItem.Addresses IsNot Nothing AndAlso r_oItem.Addresses.Count > 0 Then

            Dim oNewAddress As Address
            For Each oAddress As BaseAddressWithContactsType In r_oItem.Addresses

                oNewAddress = New Address()

                With oNewAddress
                    .Address1 = oAddress.AddressLine1.Trim()
                    .Address2 = oAddress.AddressLine2.Trim()
                    .Address3 = oAddress.AddressLine3.Trim()
                    .Address4 = oAddress.AddressLine4.Trim()

                    .AddressType = oAddress.AddressTypeCode
                    .CountryCode = oAddress.CountryCode.Trim()
                    .PostCode = oAddress.PostCode.Trim()
                    .Address5 = oAddress.AddressLine5.Trim()
                    .Address6 = oAddress.AddressLine6.Trim()
                    .Address7 = oAddress.AddressLine7.Trim()
                    .Address8 = oAddress.AddressLine8.Trim()
                    .Address9 = oAddress.AddressLine9.Trim()
                    .Address10 = oAddress.AddressLine10.Trim()

                End With

                oParty.Addresses.Add(oNewAddress)

            Next

        End If

        oParty.Contacts = New ContactCollection

        If r_oItem.Contacts IsNot Nothing AndAlso r_oItem.Contacts.Count > 0 Then

            Dim oNewContact As Contact

            For Each oContact As BaseContactType In r_oItem.Contacts

                oNewContact = New Contact()

                With oNewContact
                    .AreaCode = oContact.AreaCode.Trim()
                    .ContactType = oContact.ContactTypeCode
                    .Description = oContact.Description
                    .Extension = oContact.Extension
                    .ContactTypeDescription = oContact.ContactTypeDescription
                    If oContact.OtherContactTypeCode <> String.Empty Then
                        .OtherContactTypeCode = oContact.OtherContactTypeCode
                    Else
                        .OtherContactTypeCode = CType(oContact.ContactTypeCode, ContactType).ToString()
                    End If
                    Select Case oContact.ContactTypeCode
                        Case ContactTypeType.EMAIL
                            .ContactDetailType = ItemChoiceTypes.EmailAddress
                        Case Else
                            .ContactDetailType = ItemChoiceTypes.Number
                    End Select

                    .Number = oContact.ContactDetail.Item.Trim()

                End With

                oParty.Contacts.Add(oNewContact)

            Next
        End If

        oParty.Currency = r_oItem.Currency
        oParty.TPIntroducer = r_oItem.TPIntroducer
        oParty.TPUserCode = r_oItem.TPUserCode

        If r_oItem.DomiciledForTaxSpecified Then
            oParty.DomiciledForTax = r_oItem.DomiciledForTax
        End If

        If r_oItem.TaxExemptSpecified Then
            oParty.TaxExempt = r_oItem.TaxExempt
        End If

        oParty.TaxNumber = r_oItem.TaxNumber

        If r_oItem.TaxPercentageSpecified Then
            oParty.TaxPercentage = r_oItem.TaxPercentage
        End If

        If Logger.IsLoggingEnabled Then
            Dim sbLogMessage As New StringBuilder
            sbLogMessage.AppendLine("Convertparty executed ok" & vbCrLf)
            sbLogMessage.AppendLine("Input:" & vbCrLf)
            sbLogMessage.AppendLine("r_oItem = " & r_oItem.ToString & vbCrLf)

            sbLogMessage.AppendLine("Returned " & oParty.Print.Replace("<br />", vbCrLf))

            Dim logEntry As New LogEntry()

            logEntry.Categories.Clear()
            logEntry.Categories.Add(Category.General)
            logEntry.Priority = Priority.Normal
            logEntry.Severity = TraceEventType.Verbose
            logEntry.Message = sbLogMessage.ToString

            Logger.Write(logEntry)
        End If

        Return oParty

    End Function

    ''' <summary>
    ''' Add Party Bank Details
    ''' </summary>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_PartyBankDetails"></param>
    ''' <param name="v_sBranchCode"></param>
    Public Overrides Sub AddPartyBankDetails(ByVal v_iPartyKey As Integer,
                                             ByRef v_PartyBankDetails As BankCollection,
                                            Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oAddPartyBankDetailsRequest As AddPartyBankDetailsCommand
            Dim oAddPartyBankDetailsResponse As AddPartyBankDetailsCommandResponse
            Dim oBankDetails As BasePartyBankType
            Dim sbLogMessage As StringBuilder

            Try
                oAddPartyBankDetailsRequest = New AddPartyBankDetailsCommand
                oAddPartyBankDetailsResponse = New AddPartyBankDetailsCommandResponse
                sbLogMessage = New StringBuilder

                With oAddPartyBankDetailsRequest
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iPartyKey > 0 Then
                        .PartyKey = v_iPartyKey
                    Else
                        Throw New ArgumentNullException("PartyKey")
                    End If


                    If v_PartyBankDetails IsNot Nothing AndAlso v_PartyBankDetails.Count > 0 Then

                        'Create oBasePartyBankType from v_PartyBankDetails by looping
                        .PartyBankDetails = New List(Of BaseClasses.BasePartyBankType)

                        For iCount As Integer = 0 To v_PartyBankDetails.Count - 1

                            oBankDetails = New BaseClasses.BasePartyBankType

                            oBankDetails.AccountHolderName = v_PartyBankDetails.Item(iCount).AccountHolderName
                            oBankDetails.AccountType = v_PartyBankDetails.Item(iCount).AccountType
                            oBankDetails.BankPaymentTypeCode = v_PartyBankDetails.Item(iCount).BankPaymentTypeCode
                            oBankDetails.Bank = New BaseClasses.BaseBankType
                            oBankDetails.Bank.AccountNumber = v_PartyBankDetails.Item(iCount).AccountNumber
                            oBankDetails.Bank.BIC = v_PartyBankDetails.Item(iCount).BIC
                            oBankDetails.Bank.IBAN = v_PartyBankDetails.Item(iCount).IBAN

                            If v_PartyBankDetails.Item(iCount).PartyBankAddress IsNot Nothing Then
                                oBankDetails.Bank.BankAddress = New BaseClasses.BaseSimpleAddressType
                                oBankDetails.Bank.BankAddress.AddressLine1 = v_PartyBankDetails.Item(iCount).PartyBankAddress.Address1
                                oBankDetails.Bank.BankAddress.AddressLine2 = v_PartyBankDetails.Item(iCount).PartyBankAddress.Address2
                                oBankDetails.Bank.BankAddress.AddressLine3 = v_PartyBankDetails.Item(iCount).PartyBankAddress.Address3
                                oBankDetails.Bank.BankAddress.AddressLine4 = v_PartyBankDetails.Item(iCount).PartyBankAddress.Address4
                                oBankDetails.Bank.BankAddress.CountryCode = v_PartyBankDetails.Item(iCount).PartyBankAddress.CountryCode
                                oBankDetails.Bank.BankAddress.PostCode = v_PartyBankDetails.Item(iCount).PartyBankAddress.PostCode
                            End If

                            oBankDetails.Bank.BankCode = v_PartyBankDetails.Item(iCount).BankCode
                            oBankDetails.Bank.Branch = v_PartyBankDetails.Item(iCount).BankBranch
                            oBankDetails.Bank.BranchCode = v_PartyBankDetails.Item(iCount).BranchCode
                            oBankDetails.IsBankItem = True 'To verify as the other option Credit Card is not required for ETANA
                            oBankDetails.BankPaymentTypeCode = v_PartyBankDetails.Item(iCount).BankPaymentTypeCode
                            oBankDetails.IsDeleted = False
                            oBankDetails.PartyBankKey = v_PartyBankDetails.Item(iCount).PartyBankKey
                            If v_PartyBankDetails.Item(iCount).CreditCard IsNot Nothing Then
                                Dim oCreditCard As New BaseCreditCardType
                                oCreditCard.Number = v_PartyBankDetails.Item(iCount).CreditCard.Number
                                oCreditCard.AuthCode = v_PartyBankDetails.Item(iCount).CreditCard.AuthCode
                                oCreditCard.ManualAuthCode = oCreditCard.AuthCode
                                oCreditCard.ExpiryDate = v_PartyBankDetails.Item(iCount).CreditCard.ExpiryDate
                                oCreditCard.PartyBankKey = v_PartyBankDetails.Item(iCount).CreditCard.PartyBankKey
                                oCreditCard.NameOnCreditCard = v_PartyBankDetails.Item(iCount).CreditCard.NameOnCreditCard
                                oCreditCard.TrackingNumber = v_PartyBankDetails.Item(iCount).CreditCard.TrackingNumber
                                oCreditCard.IsRegisteredCardHolder = True
                                oBankDetails.CreditCard = oCreditCard
                                oBankDetails.IsBankItem = False

                            End If

                            .PartyBankDetails.Add(oBankDetails)

                        Next
                    Else
                        Throw New ArgumentNullException("v_PartyBankDetails")
                    End If
                End With


                'add trace to allow us to debug slow SAM calls
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.AddPartyBankDetails, oAddPartyBankDetailsRequest)
                    oAddPartyBankDetailsResponse = ApiClient.DeserializeJson(Of AddPartyBankDetailsCommandResponse)(result)
                End Using

                With oAddPartyBankDetailsResponse.AddPartyBankDetailsResponse
                    If .Errors IsNot Nothing Then
                        Throw New NexusException(.Errors)
                    Else
                        Dim iCount As Integer = 0
                        If .PartyBankStatus IsNot Nothing Then
                            For Each oAddPartyBankStatusType As BaseAddPartyBankStatusType In .PartyBankStatus
                                v_PartyBankDetails(iCount).PartyBankKey = oAddPartyBankStatusType.PartyBankKey
                                v_PartyBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.Unchanged
                                iCount = iCount + 1
                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddPartyBankDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    sbLogMessage.AppendLine("v_PartyBankDetails = " & v_PartyBankDetails.Print.Replace("<br />", vbCrLf))

                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine("v_PartyBankDetails = " & v_PartyBankDetails.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                oAddPartyBankDetailsRequest = Nothing
                oAddPartyBankDetailsResponse = Nothing
            End Try

        End SyncLock

    End Sub

    ''' <summary>
    ''' This Method Calculate Net FAC BAND for Claim
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <remarks></remarks>
    Sub CalculateClaimFACNet(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument, ByVal RIBand As XmlElement)
        Dim bDataFound As Boolean = False

        'Calculate/Retreive Band Total
        Dim dBANDThisPayment, dBANDSumInsured, dBANDReserveToDate, dBANDThisReserve, dBANDPaymentToDate, dBANDBalance, dBANDRecoverToDate As Decimal
        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Band Total']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dBANDSumInsured)
            Decimal.TryParse(oNode.Attributes("ReserveToDate").Value, dBANDReserveToDate)
            Decimal.TryParse(oNode.Attributes("ThisReserve").Value, dBANDThisReserve)
            Decimal.TryParse(oNode.Attributes("PaymentToDate").Value, dBANDPaymentToDate)
            Decimal.TryParse(oNode.Attributes("Balance").Value, dBANDBalance)
            Decimal.TryParse(oNode.Attributes("RecoverToDate").Value, dBANDRecoverToDate)
            Decimal.TryParse(oNode.Attributes("ThisPayment").Value, dBANDThisPayment)
        End If

        'Retreive the FAC Prop
        Dim dFACTotThisPayment, dFACTotSumInsured, dFACTotReserveToDate, dFACTotThisReserve, dFACTotPaymentToDate, dFACTotBalance, dFACTotRecoverToDate As Decimal

        Dim xmlNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Placement='Treaty QSH']")
        Dim xmlNode As XmlNode = Nothing
        If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then
            bDataFound = True
            For Each xmlNode In xmlNodes
                If xmlNode.Attributes("IsObligatory").Value = True Then
                    Dim dOblQSHThisPayment, dOblQSHSumInsured, dOblQSHReserveToDate, dOblQSHThisReserve, dOblQSHPaymentToDate, dOblQSHBalance, dOblQSHRecoverToDate As Decimal
                    Decimal.TryParse(xmlNode.Attributes("SumInsured").Value, dOblQSHSumInsured)
                    Decimal.TryParse(xmlNode.Attributes("ReserveToDate").Value, dOblQSHReserveToDate)
                    Decimal.TryParse(xmlNode.Attributes("ThisReserve").Value, dOblQSHThisReserve)
                    Decimal.TryParse(xmlNode.Attributes("PaymentToDate").Value, dOblQSHPaymentToDate)
                    Decimal.TryParse(xmlNode.Attributes("Balance").Value, dOblQSHBalance)
                    Decimal.TryParse(xmlNode.Attributes("RecoverToDate").Value, dOblQSHRecoverToDate)
                    Decimal.TryParse(xmlNode.Attributes("ThisPayment").Value, dOblQSHThisPayment)

                    dFACTotSumInsured += dOblQSHSumInsured
                    dFACTotReserveToDate += dOblQSHReserveToDate
                    dFACTotThisReserve += dOblQSHThisReserve
                    dFACTotPaymentToDate += dOblQSHPaymentToDate
                    dFACTotBalance += dOblQSHBalance
                    dFACTotRecoverToDate += dOblQSHRecoverToDate
                    dFACTotThisPayment += dOblQSHThisPayment
                End If
            Next
        End If

        xmlNodes = Nothing
        xmlNodes = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Placement='FAC Prop']")
        If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then
            bDataFound = True
            For Each xmlNode In xmlNodes
                Dim dFACThisPayment, dFACSumInsured, dFACReserveToDate, dFACThisReserve, dFACPaymentToDate, dFACBalance, dFACRecoverToDate As Decimal
                Decimal.TryParse(xmlNode.Attributes("SumInsured").Value, dFACSumInsured)
                Decimal.TryParse(xmlNode.Attributes("ReserveToDate").Value, dFACReserveToDate)
                Decimal.TryParse(xmlNode.Attributes("ThisReserve").Value, dFACThisReserve)
                Decimal.TryParse(xmlNode.Attributes("PaymentToDate").Value, dFACPaymentToDate)
                Decimal.TryParse(xmlNode.Attributes("Balance").Value, dFACBalance)
                Decimal.TryParse(xmlNode.Attributes("RecoverToDate").Value, dFACRecoverToDate)
                Decimal.TryParse(xmlNode.Attributes("ThisPayment").Value, dFACThisPayment)

                dFACTotSumInsured += dFACSumInsured
                dFACTotReserveToDate += dFACReserveToDate
                dFACTotThisReserve += dFACThisReserve
                dFACTotPaymentToDate += dFACPaymentToDate
                dFACTotBalance += dFACBalance
                dFACTotRecoverToDate += dFACRecoverToDate
                dFACTotThisPayment += dFACThisPayment
            Next
        End If

        'Retreive the FAC XOL
        xmlNodes = Nothing
        xmlNodes = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Placement='FAC XOL']")

        If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then
            bDataFound = True
            xmlNode = Nothing
            For Each xmlNode In xmlNodes
                Dim dFACThisPayment, dFACSumInsured, dFACReserveToDate, dFACThisReserve, dFACPaymentToDate, dFACBalance, dFACRecoverToDate As Decimal
                Decimal.TryParse(xmlNode.Attributes("SumInsured").Value, dFACSumInsured)
                Decimal.TryParse(xmlNode.Attributes("ReserveToDate").Value, dFACReserveToDate)
                Decimal.TryParse(xmlNode.Attributes("ThisReserve").Value, dFACThisReserve)
                Decimal.TryParse(xmlNode.Attributes("PaymentToDate").Value, dFACPaymentToDate)
                Decimal.TryParse(xmlNode.Attributes("Balance").Value, dFACBalance)
                Decimal.TryParse(xmlNode.Attributes("RecoverToDate").Value, dFACRecoverToDate)
                Decimal.TryParse(xmlNode.Attributes("ThisPayment").Value, dFACThisPayment)

                dFACTotSumInsured += dFACSumInsured
                dFACTotReserveToDate += dFACReserveToDate
                dFACTotThisReserve += dFACThisReserve
                dFACTotPaymentToDate += dFACPaymentToDate
                dFACTotBalance += dFACBalance
                dFACTotRecoverToDate += dFACRecoverToDate
                dFACTotThisPayment += dFACThisPayment
            Next
        End If

        'Add into XML
        Dim sArrangementRow As String = "ArrangementRow"
        Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
        Dim myCol As DataColumn
        Dim sValue As String = ""
        For Each myCol In oRITable.Columns
            If myCol.ColumnName = "SumInsured" Or myCol.ColumnName = "ReserveToDate" _
              Or myCol.ColumnName = "Name" Or myCol.ColumnName = "ThisReserve" _
              Or myCol.ColumnName = "PaymentToDate" Or myCol.ColumnName = "Balance" _
              Or myCol.ColumnName = "RecoverToDate" _
              Or myCol.ColumnName = "ThisPayment" _
              Or myCol.ColumnName = "Placement" Then

                'Placement
                If myCol.ColumnName = "Placement" Then
                    ArrangementRow.SetAttribute(myCol.ColumnName, "GROSS NET")
                End If
                'Name
                If myCol.ColumnName = "Name" Then
                    ArrangementRow.SetAttribute(myCol.ColumnName, "Net of FAC")
                End If

                'Sum Insured
                sValue = ""
                If myCol.ColumnName = "SumInsured" Then
                    sValue = dBANDSumInsured - dFACTotSumInsured
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'ReserveToDate
                sValue = ""
                If myCol.ColumnName = "ReserveToDate" Then
                    sValue = dBANDReserveToDate - dFACTotReserveToDate
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'ThisReserve
                sValue = ""
                If myCol.ColumnName = "ThisReserve" Then
                    sValue = dBANDThisReserve - dFACTotThisReserve
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'PaymentToDate
                sValue = ""
                If myCol.ColumnName = "PaymentToDate" Then
                    sValue = dBANDPaymentToDate - dFACTotPaymentToDate
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'Balance
                sValue = ""
                If myCol.ColumnName = "Balance" Then
                    sValue = dBANDBalance - dFACTotBalance
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'RecoverToDate
                sValue = ""
                If myCol.ColumnName = "RecoverToDate" Then
                    sValue = dBANDRecoverToDate - dFACTotRecoverToDate
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'ThisPayment
                sValue = ""
                If myCol.ColumnName = "ThisPayment" Then
                    sValue = dBANDThisPayment - dFACTotThisPayment
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If
            Else
                sValue = ""
                ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
            End If
        Next myCol

        If bDataFound = True Then
            RIBand.InsertAfter(ArrangementRow, xmlNode)
        End If

    End Sub

    Public Overrides Sub DeletePartyBankDetails(ByVal v_iPartyKey As Integer,
                                     ByVal vPartyBankDetails As BankCollection,
                                     Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oDeletePartyBankDetailsRequest As DeletePartyBankDetailsCommandBase
            Dim oDeletePartyBankDetailsResponse As DeletePartyBankDetailsCommandBaseResponse
            Dim oBankDetails As BaseDeletePartyBankDetailsRequestTypeRow
            Dim sbLogMessage As StringBuilder

            Try
                oDeletePartyBankDetailsRequest = New DeletePartyBankDetailsCommandBase
                oDeletePartyBankDetailsResponse = New DeletePartyBankDetailsCommandBaseResponse
                sbLogMessage = New StringBuilder

                With oDeletePartyBankDetailsRequest
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If vPartyBankDetails IsNot Nothing AndAlso vPartyBankDetails.Count > 0 Then

                        .PartBankDetails = New List(Of BaseClasses.BaseDeletePartyBankDetailsRequestTypeRow)

                        For iCount As Integer = 0 To vPartyBankDetails.Count - 1
                            oBankDetails = New BaseClasses.BaseDeletePartyBankDetailsRequestTypeRow

                            oBankDetails.PartyBankKey = vPartyBankDetails.Item(iCount).PartyBankKey
                            .PartBankDetails.Add(oBankDetails)
                        Next

                    Else
                        Throw New ArgumentNullException("vPartyBankDetails")
                    End If

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Delete(ApiMethods.DeletePartyBankDetails, oDeletePartyBankDetailsRequest)
                    oDeletePartyBankDetailsResponse = ApiClient.DeserializeJson(Of DeletePartyBankDetailsCommandBaseResponse)(result)
                End Using

                With oDeletePartyBankDetailsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("DeletePartyBankDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iPartyKey = " & vPartyBankDetails.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("vPartyBankDetails = " & vPartyBankDetails.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                oDeletePartyBankDetailsRequest = Nothing
                oDeletePartyBankDetailsResponse = Nothing
            End Try

        End SyncLock
    End Sub

    ''' <summary>
    ''' Converts string party type to PartyTypeType enum, handling different enum value mappings
    ''' </summary>
    ''' <param name="partyTypeString">String representation of party type</param>
    ''' <returns>PartyTypeType enum value</returns>
    Private Function ConvertPartyTypeFromString(ByVal partyTypeString As String) As Enums.PartyTypeType
        ' Handle numeric values first
        Dim numericValue As Integer
        If Integer.TryParse(partyTypeString, numericValue) Then
            Select Case numericValue
                Case 0 : Return Enums.PartyTypeType.PC
                Case 1 : Return Enums.PartyTypeType.GC
                Case 2 : Return Enums.PartyTypeType.AG
                Case 3 : Return Enums.PartyTypeType.CC
                Case 5 : Return Enums.PartyTypeType.AH
                Case 6 : Return Enums.PartyTypeType.IN
                Case 18 : Return Enums.PartyTypeType.AGG
                Case 23 : Return Enums.PartyTypeType.OTOTHERPARTY
                Case Else : Return Enums.PartyTypeType.AG
            End Select
        End If

        ' Handle string values
        Select Case partyTypeString.ToUpper()
            Case "PC" : Return Enums.PartyTypeType.PC
            Case "GC" : Return Enums.PartyTypeType.GC
            Case "AG" : Return Enums.PartyTypeType.AG
            Case "CC" : Return Enums.PartyTypeType.CC
            Case "AH" : Return Enums.PartyTypeType.AH
            Case "IN" : Return Enums.PartyTypeType.IN
            Case "AGG" : Return Enums.PartyTypeType.AGG
            Case "OTOTHERPARTY" : Return Enums.PartyTypeType.OTOTHERPARTY
            Case Else : Return Enums.PartyTypeType.AG
        End Select
    End Function

    ''' <summary>
    ''' FindParty
    ''' </summary>
    ''' <param name="v_oPartySearchCriteria"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function FindParty(ByVal v_oPartySearchCriteria As PartySearchCriteria,
                                   Optional ByVal v_sBranchCode As String = Nothing,
                                   Optional ByVal sSearchType As String = "") As PartyCollection

        SyncLock oLock

            Dim oFindPartyRequest As FindPartyQueryBase
            Dim oFindPartyResponse As FindPartyQueryBaseResponse
            Dim oListOfParties As PartyCollection
            Dim oParty As BaseParty = Nothing
            Dim iCounter As Integer = 0
            Dim bMatched As Boolean = False
            Dim sbLogMessage As StringBuilder

            Try

                oFindPartyRequest = New FindPartyQueryBase
                oFindPartyResponse = New FindPartyQueryBaseResponse
                sbLogMessage = New StringBuilder

                With oFindPartyRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If v_oPartySearchCriteria.PartyType IsNot Nothing AndAlso v_oPartySearchCriteria.PartyType.ToUpper <> Nothing Then
                        .PartyType = ConvertPartyTypeFromString(v_oPartySearchCriteria.PartyType.ToUpper)
                        If .PartyType = PartyTypeType.OTOTHERPARTY Then
                            .OtherPartyTypeCode = v_oPartySearchCriteria.OtherPartyTypeCode
                        End If
                    End If
                    .Shortname = v_oPartySearchCriteria.ShortName
                    '.AlternativeId = v_oPartySearchCriteria.AlternativeID
                    .Name = v_oPartySearchCriteria.Name
                    '.Firstname = v_oPartySearchCriteria.FirstName

                    .IsAnySelected = v_oPartySearchCriteria.IsAnySelected


                    If v_oPartySearchCriteria.Address IsNot Nothing Then
                        .AddressLine1 = v_oPartySearchCriteria.Address.Address1
                        .AddressLine2 = v_oPartySearchCriteria.Address.Address2
                        ' .AddressLine3 = v_oPartySearchCriteria.Address.Address3
                        ' .AddressLine4 = v_oPartySearchCriteria.Address.Address4
                        .PostCode = v_oPartySearchCriteria.Address.PostCode
                    End If

                    .AreaCode = v_oPartySearchCriteria.AreaCode
                    .TelephoneNumber = v_oPartySearchCriteria.TelephoneNumber

                    If v_oPartySearchCriteria.DateOfBirth <> Date.MinValue Then

                        .DateOfBirth = v_oPartySearchCriteria.DateOfBirth
                        .DateOfBirthSpecified = True
                    Else
                        .DateOfBirthSpecified = False
                    End If

                    .PolicyRef = v_oPartySearchCriteria.PolicyRef
                    .RiskIndex = v_oPartySearchCriteria.RiskIndex
                    .PartyIndex = v_oPartySearchCriteria.PartyIndex
                    .FileCode = v_oPartySearchCriteria.FileCode
                    'for new updated SAM we need status parameter also

                    If String.IsNullOrEmpty(v_oPartySearchCriteria.Status) = True Then
                        .Status = "All"
                    ElseIf String.IsNullOrEmpty(v_oPartySearchCriteria.Status) = False Then
                        .Status = v_oPartySearchCriteria.Status
                    End If

                    .RiskRequestdex = v_oPartySearchCriteria.RiskRequestIndex
                    .ClaimsRiskIndex = v_oPartySearchCriteria.ClaimsRiskIndex
                    .IncludeClosedBranches = v_oPartySearchCriteria.IncludeClosedBranches
                    .ClaimNumber = v_oPartySearchCriteria.ClaimNumber

                    If v_oPartySearchCriteria.SupressSubAgents > 0 Then
                        .SupressSubAgents = v_oPartySearchCriteria.SupressSubAgents
                        .SupressSubAgentsSpecified = True
                    Else
                        .SupressSubAgentsSpecified = False
                    End If

                    If v_oPartySearchCriteria.PartySourceId > 0 Then
                        .PartySourceId = v_oPartySearchCriteria.PartySourceId
                        .PartySourceIdSpecified = True
                    Else
                        .PartySourceIdSpecified = False
                    End If

                    .TransactionType = v_oPartySearchCriteria.TransactionType

                    If v_oPartySearchCriteria.AgentType IsNot Nothing Then
                        .AgentType = v_oPartySearchCriteria.AgentType
                        .AgentTypeSpecified = True
                    Else
                        .AgentTypeSpecified = False
                    End If

                    If v_oPartySearchCriteria.MaxRowsToFetch > 0 Then
                        .MaxRowsToFetch = v_oPartySearchCriteria.MaxRowsToFetch
                        .MaxRowsToFetchSpecified = True
                    Else
                        .MaxRowsToFetchSpecified = False
                    End If
                    If v_oPartySearchCriteria.CaseNumber IsNot Nothing Then
                        .CaseNumber = v_oPartySearchCriteria.CaseNumber
                        .CaseNumberSpecified = True
                    Else
                        .CaseNumberSpecified = False
                    End If
                    .AgentGroup = v_oPartySearchCriteria.AgentGroupCode
                End With

                oListOfParties = New PartyCollection()


                'PartyType is required to make the SAM call, so don't bother it not set
                If v_oPartySearchCriteria.PartyTypeSpecified Then

                    For Each oPartyType As PartyType In v_oPartySearchCriteria.PartyTypes
                        If sSearchType = "" Then
                            oFindPartyRequest.PartyType = ConvertPartyTypeFromString(oPartyType)
                            oFindPartyRequest.PartyTypeSpecified = True
                        Else
                            oFindPartyRequest.PartyTypeSpecified = False
                            oFindPartyRequest.SearchType = sSearchType
                        End If

                        Using trace As New Tracer(Category.Trace)
                            ApiClient._tokenModel = GetApiTokendetails()
                            Dim result As String = ApiClient.Get(ApiMethods.FindParty, oFindPartyRequest)
                            oFindPartyResponse = ApiClient.DeserializeJson(Of FindPartyQueryBaseResponse)(result)
                        End Using
                        With oFindPartyResponse

                            If .Parties Is Nothing Then

                                'Process the error object if errors, and throw as a single exception
                                'Throw New NexusException(.Parties)
                            Else

                                If .Parties IsNot Nothing Then

                                    For Each oBaseParty As BaseFindPartyResponseTypeRow In .Parties
                                        bMatched = False

                                        Select Case oPartyType
                                            Case PartyType.Personal
                                                oParty = New PersonalParty(oBaseParty.PartyKey, oBaseParty.ShortName)
                                                With CType(oParty, PersonalParty)
                                                    .DateOfBirth = oBaseParty.DateOfBirth
                                                End With
                                            Case PartyType.Corporate
                                                oParty = New CorporateParty(oBaseParty.PartyKey, oBaseParty.ShortName)
                                            Case Else
                                                If (oBaseParty.ClientCode = "PC") Then
                                                    oParty = New PersonalParty(oBaseParty.PartyKey, oBaseParty.ShortName)
                                                    With CType(oParty, PersonalParty)
                                                        .DateOfBirth = oBaseParty.DateOfBirth
                                                    End With
                                                ElseIf (oBaseParty.ClientCode = "CC") Then
                                                    oParty = New CorporateParty(oBaseParty.PartyKey, oBaseParty.ShortName)
                                                Else
                                                    oParty = New PersonalParty(oBaseParty.PartyKey, oBaseParty.ShortName)
                                                End If

                                        End Select

                                        With oBaseParty
                                            oParty.AgentKey = .AgentKey
                                            oParty.ResolvedName = .ResolvedName
                                            oParty.Name = .Name
                                            oParty.AgentType = .AgentType
                                            oParty.AddressLine1 = .AddressLine1
                                            oParty.AddressLine2 = .AddressLine2
                                            oParty.PartySourceDescription = .PartySourceDescription
                                            oParty.Type = .Type
                                            oParty.PostCode = .PostCode
                                            oParty.Addresses.Add(New Address(.AddressLine1, .PostCode, .CountryCode))
                                            oParty.Contacts.Add(New Contact(ContactType.Main, .ContactTelephoneNumber))
                                            oParty.ShortName = .ShortName
                                            oParty.DateCancelled = .DateCancelled
                                            oParty.ServiceLevelCode = .ServiceLevelCode
                                            oParty.ServiceLevelDescription = .ServiceLevelDescription
                                            oParty.FileCode = IIf(.FileCode Is Nothing, String.Empty, .FileCode)
                                            oParty.DOB = .DateOfBirth
                                            oParty.BranchCode = .PartySourceId
                                            oParty.PartyTypeId = .PartyTypeId
                                        End With

                                        For iCounter = 0 To oListOfParties.Count - 1
                                            If oListOfParties(iCounter).Key = oParty.Key Then
                                                bMatched = True
                                                Exit For
                                            End If
                                        Next

                                        If (bMatched = False) And (v_oPartySearchCriteria.MaxRowsToFetch <= 0 Or oListOfParties.Count < v_oPartySearchCriteria.MaxRowsToFetch) Then
                                            oListOfParties.Add(oParty)
                                        End If
                                    Next
                                End If

                            End If

                        End With
                        If sSearchType <> "" Then
                            Exit For
                        End If
                    Next

                Else
                    Throw New ArgumentNullException("PartySearchCriteria.PartyType", "PartySearchCriteria must contain at least one PartyType to call Find Party")
                End If

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindParty executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oPartySearchCriteria = " & v_oPartySearchCriteria.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(oListOfParties) Then
                        sbLogMessage.AppendLine("Returned " & oListOfParties.Count.ToString & " results" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Returned 0 results" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                '  oSAM.Close()
                oFindPartyRequest = Nothing
                oFindPartyResponse = Nothing
            End Try

            Return oListOfParties

        End SyncLock

    End Function
    ''' <summary>
    ''' This Method is used for getting a party detail. 
    ''' </summary>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetParty(ByVal v_iPartyKey As Integer,
                                   Optional ByVal v_sBranchCode As String = Nothing) As BaseParty

        SyncLock oLock

            Dim oGetPartyRequest As GetPartyQuery
            Dim oGetPartyResponse As GetPartyQueryResponse
            Dim oParty As BaseParty = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oGetPartyRequest = New GetPartyQuery
                oGetPartyResponse = New GetPartyQueryResponse
                sbLogMessage = New StringBuilder

                With oGetPartyRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iPartyKey > 0 Then
                        .PartyKey = v_iPartyKey
                    Else
                        Throw New ArgumentNullException("PartyKey")
                    End If

                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetParty, oGetPartyRequest)
                    oGetPartyResponse = ApiClient.DeserializeJson(Of GetPartyQueryResponse)(result)
                End Using
                With oGetPartyResponse

                    Dim oBasePartyType As New BasePartyType
                    ' CopyRequestProperties(.PCType, oBasePartyType)
                    If .PCType IsNot Nothing Then
                        oParty = ConvertPartyPC(.PCType)
                        oParty.XMLDataset = .PCType.XMLDataset
                    ElseIf .CCType IsNot Nothing Then
                        oParty = ConvertpartyCC(.CCType)
                        oParty.XMLDataset = .CCType.XMLDataset
                    ElseIf .OTHERType IsNot Nothing Then
                        oParty = ConvertpartyOther(.OTHERType)
                        oParty.XMLDataset = .OTHERType.XMLDataset
                    End If
                    oParty.Key = v_iPartyKey
                    oParty.TimeStamp = .PartyTimestamp

                    oParty.NoofPolicies = .NoofPolicies
                    oParty.NoofOpenClaims = .NoofOpenClaims
                    oParty.NoofClosedClaims = .NoofClosedClaims

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetParty executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iPartyKey = " & v_iPartyKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oParty.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                ' oSAM.Close()
                oGetPartyRequest = Nothing
                oGetPartyResponse = Nothing
            End Try

            Return oParty

        End SyncLock

    End Function
    Private Shared Sub CopyRequestProperties(source As Object, destination As Object)
        Dim destinationProperties As PropertyInfo() = destination.[GetType]().GetProperties()
        For Each destinationPi As PropertyInfo In destinationProperties

            Dim sourcePi As PropertyInfo = source?.[GetType]()?.GetProperty(destinationPi.Name)

            destinationPi.SetValue(destination, sourcePi?.GetValue(source, Nothing), Nothing)
        Next
    End Sub
    Public Overrides Function GetPartyBankDetails(ByVal v_iPartyKey As Integer,
                                   Optional ByVal v_sBranchCode As String = Nothing) As BankCollection
        SyncLock oLock

            Dim oGetPartyBankDetailsRequest As GetPartyBankDetailsQuery
            Dim oGetPartyBankDetailsResponse As GetPartyBankDetailsQueryResponse
            Dim oBank As Bank
            Dim oBankHistory As BankHistory
            Dim oBankCollection As BankCollection = Nothing
            Dim oBankHistoryCollection As BankHistoryCollection
            Dim oBaseBankType As BasePartyBankType
            Dim oBaseBankHistory As BasePartyBankHistoryType
            Dim oCreditCard As CreditCard
            Dim sbLogMessage As StringBuilder

            Try
                oGetPartyBankDetailsRequest = New GetPartyBankDetailsQuery
                oGetPartyBankDetailsResponse = New GetPartyBankDetailsQueryResponse
                sbLogMessage = New StringBuilder


                With oGetPartyBankDetailsRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iPartyKey > 0 Then
                        .PartyKey = v_iPartyKey
                    Else
                        Throw New ArgumentNullException("PartyKey")
                    End If

                    .IncludeHistory = True
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetPartyBankDetails, oGetPartyBankDetailsRequest)
                    oGetPartyBankDetailsResponse = ApiClient.DeserializeJson(Of GetPartyBankDetailsQueryResponse)(result)
                End Using
                oBankCollection = New BankCollection

                If oGetPartyBankDetailsResponse IsNot Nothing Then
                    With oGetPartyBankDetailsResponse.GetPartyBankDetailsResponse

                        If .PartyBankDetails IsNot Nothing Then

                            For Each oBaseBankType In .PartyBankDetails
                                oBank = New Bank
                                oBankHistoryCollection = New BankHistoryCollection
                                oBank.AccountHolderName = oBaseBankType.AccountHolderName
                                oBank.AccountKey = oBaseBankType.AccountKey
                                oBank.AccountType = oBaseBankType.AccountType
                                oBank.IsBankItem = oBaseBankType.IsBankItem
                                oBank.IsDeleted = oBaseBankType.IsDeleted
                                oBank.PartyBankKey = oBaseBankType.PartyBankKey
                                oBank.RowKey = oBaseBankType.RowKey
                                oBank.BankPaymentTypeCode = oBaseBankType.BankPaymentTypeCode

                                oBank.IsExternalCreditCardHandling = .IsExternalCreditCardHandling
                                oBank.LastTransactedPartyBankKey = .LastTransactedPartyBankKey
                                oBank.TimeStamp = .ApiTimeStamp
                                oBank.IsActive = Not oBaseBankType.IsDeleted
                                oBank.IsPartyBankInUse = oBaseBankType.IsPartyBankInUse
                                oBank.IsPartyBankLinkedWithInst = oBaseBankType.IsPartyBankLinkedWithInst

                                If oBaseBankType.IsBankItem Then
                                    If oBaseBankType.Bank IsNot Nothing Then
                                        oBank.AccountNumber = oBaseBankType.Bank.AccountNumber
                                        oBank.BankCode = oBaseBankType.Bank.BankCode
                                        oBank.BankName = oBaseBankType.Bank.BankName
                                        oBank.BankBranch = oBaseBankType.Bank.Branch
                                        oBank.BranchCode = oBaseBankType.Bank.BranchCode
                                        oBank.BIC = oBaseBankType.Bank.BIC
                                        oBank.IBAN = oBaseBankType.Bank.IBAN

                                        'For Bank Address
                                        If oBaseBankType.Bank.BankAddress IsNot Nothing Then
                                            oBank.StreetName = oBaseBankType.Bank.BankAddress.AddressLine1
                                            oBank.Locality = oBaseBankType.Bank.BankAddress.AddressLine2
                                            oBank.PostTown = oBaseBankType.Bank.BankAddress.AddressLine3
                                            oBank.County = oBaseBankType.Bank.BankAddress.AddressLine4
                                            oBank.PostCode = oBaseBankType.Bank.BankAddress.PostCode
                                            oBank.Country = oBaseBankType.Bank.BankAddress.CountryCode
                                            oBank.PartyBankAddress.Address1 = oBaseBankType.Bank.BankAddress.AddressLine1
                                            oBank.PartyBankAddress.Address2 = oBaseBankType.Bank.BankAddress.AddressLine2
                                            oBank.PartyBankAddress.Address3 = oBaseBankType.Bank.BankAddress.AddressLine3
                                            oBank.PartyBankAddress.Address4 = oBaseBankType.Bank.BankAddress.AddressLine4
                                            oBank.PartyBankAddress.PostCode = oBaseBankType.Bank.BankAddress.PostCode
                                            oBank.PartyBankAddress.CountryCode = oBaseBankType.Bank.BankAddress.CountryCode

                                        End If
                                    End If
                                ElseIf oBaseBankType.CreditCard IsNot Nothing Then

                                    oCreditCard = New CreditCard

                                    With oBaseBankType.CreditCard
                                        If .CardHolder IsNot Nothing Then
                                            oCreditCard.Address1 = .CardHolder.AddressLine1
                                            oCreditCard.Address2 = .CardHolder.AddressLine2
                                            oCreditCard.Address3 = .CardHolder.AddressLine3
                                            oCreditCard.Address4 = .CardHolder.AddressLine4
                                            oCreditCard.CountryCode = .CardHolder.CountryCode
                                            oCreditCard.PostCode = .CardHolder.PostCode
                                            oCreditCard.Name = .CardHolder.Name
                                        End If

                                        oCreditCard.AccountType = .AccountType

                                        oCreditCard.AuthCode = .AuthCode
                                        oCreditCard.CCCustomer = .CustomerPresent
                                        oCreditCard.CCIssue = .Issue
                                        oCreditCard.CCIssueBank = .PartyBankKey
                                        oCreditCard.CCPin = .Pin
                                        oCreditCard.CCSlipNumber = .TransactionSlipNumber
                                        oCreditCard.CCTypeOfCard = .TypeCode

                                        oCreditCard.CustomerPresent = .CustomerPresent
                                        oCreditCard.ExpiryDate = .ExpiryDate
                                        oCreditCard.Issue = .Issue
                                        oCreditCard.ManualAuthCode = .ManualAuthCode

                                        oCreditCard.NameOnCreditCard = .NameOnCreditCard
                                        oCreditCard.Number = .Number
                                        oCreditCard.Pin = .Pin

                                        oCreditCard.StartDate = .StartDate
                                        oCreditCard.TrackingNumber = .TrackingNumber
                                        oCreditCard.IsDefaultCreditCard = .IsDefaultCreditCard


                                        oBank.CreditCard = oCreditCard

                                        ' oBankCollection.Add(oCreditCard)
                                    End With
                                End If

                                If oBaseBankType.History IsNot Nothing Then
                                    For Each oBaseBankHistory In oBaseBankType.History
                                        oBankHistory = New BankHistory

                                        oBankHistory.AccountHolderName = oBaseBankHistory.AccountHolderName
                                        oBankHistory.AccountNumber = oBaseBankHistory.AccountNumber
                                        oBankHistory.AccountType = oBaseBankHistory.AccountType
                                        oBankHistory.ActionCode = oBaseBankHistory.ActionCode
                                        oBankHistory.BranchCode = oBaseBankHistory.BankBranchCode
                                        oBankHistory.BankName = oBaseBankHistory.BankName
                                        oBankHistory.ActionDate = oBaseBankHistory.DateModified
                                        oBankHistory.PartyBankKey = oBaseBankHistory.PartyBankKey
                                        oBankHistory.PostCode = oBaseBankHistory.PostCode
                                        oBankHistory.StreetName = oBaseBankHistory.StreetName
                                        oBankHistory.UserName = oBaseBankHistory.UserName
                                        oBankHistory.BankBranch = oBaseBankHistory.BankBranch
                                        oBankHistory.BIC = oBaseBankHistory.BIC
                                        oBankHistory.IBAN = oBaseBankHistory.IBAN
                                        oBankHistoryCollection.Add(oBankHistory)
                                    Next
                                    oBank.History = oBankHistoryCollection
                                End If
                                oBankCollection.Add(oBank)
                            Next
                        End If
                    End With
                End If

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetPartyBankDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iPartyKey = " & v_iPartyKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetPartyBankDetailsRequest = Nothing
                oGetPartyBankDetailsResponse = Nothing
            End Try


            Return oBankCollection
        End SyncLock

    End Function

    Public Overrides Function GetPartyPolicies(ByVal v_sPartyCode As String,
                                               Optional ByVal v_sBranchCode As String = Nothing,
                                               Optional ByVal v_nAgentKeyFilter As Integer = 0,
                                               Optional ByVal v_sAgentTypeFilter As String = Nothing) As PartySummary

        SyncLock oLock

            Dim oGetPartyPoliciesRequest As GetPartyPoliciesQuery
            Dim oGetPartyPoliciesResponse As GetPartyPoliciesQueryResponse
            Dim oPartySummary As PartySummary = Nothing
            Dim oNewPolicy As Policy
            Dim sbLogMessage As StringBuilder

            Try
                oGetPartyPoliciesRequest = New GetPartyPoliciesQuery
                oGetPartyPoliciesResponse = New GetPartyPoliciesQueryResponse
                sbLogMessage = New StringBuilder

                With oGetPartyPoliciesRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .PartyCode = v_sPartyCode
                    If GetOptionSetting(NexusProvider.OptionType.ProductOption, NexusProvider.SystemOptions.AllowPolicyClientAssociations).OptionValue = "1" Then
                        .RetrieveAssociates = True
                    Else
                        .RetrieveAssociates = False
                    End If
                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetPartyPolicies, oGetPartyPoliciesRequest)
                    oGetPartyPoliciesResponse = ApiClient.DeserializeJson(Of GetPartyPoliciesQueryResponse)(result)
                End Using

                With oGetPartyPoliciesResponse

                    If .PartyPolicies IsNot Nothing Then

                        oPartySummary = New PartySummary()

                        If .PartyPolicies IsNot Nothing Then

                            oPartySummary.Policies = New PolicyCollection()

                            For Each oPolicy As BaseClasses.BaseGetPartyPoliciesResponseTypeRow In .PartyPolicies

                                If v_nAgentKeyFilter > 0 AndAlso oPolicy.LeadAgentKey <> v_nAgentKeyFilter AndAlso v_sAgentTypeFilter = "AG" Then
                                    Continue For
                                End If
                                oNewPolicy = New Policy(oPolicy.InsuranceRef)

                                With oNewPolicy
                                    .InsuranceFileKey = oPolicy.InsuranceFileKey
                                    .Reference = oPolicy.InsuranceRef
                                    .InsuranceFolderKey = oPolicy.InsuranceFolderKey
                                    .PolicyTypeID = oPolicy.PolicyTypeKey '.PolicyTypeId
                                    .DateIssued = oPolicy.DateCreated '.DateIssued
                                    .RenewalDate = oPolicy.RenewalDate
                                    .ProductKey = oPolicy.ProductKey
                                    .LeadAgentKey = oPolicy.LeadAgentKey
                                    .ThisPremium = oPolicy.ThisPremium
                                    .PartyShortName = oGetPartyPoliciesResponse.PartyCode
                                    .ProductCode = oPolicy.ProductCode
                                    .InsuranceFileTypeCode = oPolicy.TypeCode
                                    .PolicyStatusCode = "LIVE"
                                    .InsurerShortName = oGetPartyPoliciesResponse.PartyName
                                    .PolicyTypeCode = oPolicy.TypeCode
                                    .PolicyTypeDescription = oPolicy.PolicyTypeDesc
                                    .RiskTypeDescription = oPolicy.PolicyTypeDesc
                                    .AgentCode = oPolicy.LeadAgentCode
                                    .InsuranceHolderKey = oPolicy.InsuranceHolderKey
                                    .ClosePolicyClaims = oPolicy.ClosePolicyClaims
                                    .InsuranceDesc = oPolicy.InsuranceDesc
                                    .InsuranceFileSourceKey = oPolicy.InsuranceFileSourceKey
                                    .LastTransDesc = oPolicy.LastTransDesc
                                    .OpenPolicyClaims = oPolicy.OpenPolicyClaims
                                    .CoverStartDate = oPolicy.CoverStartDate
                                    .CoverEndDate = oPolicy.ExpiryDate
                                    .ExpiryDate = oPolicy.ExpiryDate
                                    .MarkedQuoteForCollection = oPolicy.MarkedForCollection
                                    .RenewedVersion = oPolicy.RenewedVersion
                                    .PolicyStatus = oPolicy.PolicyStatus
                                    .IsMarketPlacePolicy = oPolicy.IsMarketPlacePolicy
                                    .CorrespondenceType = oPolicy.CorrespondenceType
                                    .DefaultPreferredCorrespondence = oPolicy.DefaultPreferredCorrespondence
                                    .IsAgentReceiveCorrespondence = oPolicy.IsAgentReceiveCorrespondence
                                    .AssociatedClients = oPolicy.AssociatedClients
                                End With

                                oPartySummary.Policies.Add(oNewPolicy)

                            Next

                        End If

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetPartySummary executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_sPartyCode = " & v_sPartyCode.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As FaultException(Of SamMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetPartyPoliciesRequest = Nothing
                oGetPartyPoliciesResponse = Nothing
            End Try

            Return oPartySummary

        End SyncLock

    End Function

    ''' <summary>
    ''' This method is used to get Party summary details
    ''' </summary>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPartySummary(ByVal v_iPartyKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As PartySummary

        SyncLock oLock

            Dim oGetPartySummaryRequest As GetPartySummaryQuery
            Dim oGetPartySummaryResponse As GetPartySummaryQueryResponse
            Dim oPartySummary As PartySummary = Nothing
            Dim oNewPolicy As Policy
            Dim sbLogMessage As StringBuilder

            Try
                oGetPartySummaryRequest = New GetPartySummaryQuery
                oGetPartySummaryResponse = New GetPartySummaryQueryResponse
                sbLogMessage = New StringBuilder

                With oGetPartySummaryRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iPartyKey > 0 Then
                        .PartyKey = v_iPartyKey
                    Else
                        Throw New ArgumentNullException("PartyKey")
                    End If

                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetPartySummary, oGetPartySummaryRequest)
                    oGetPartySummaryResponse = ApiClient.DeserializeJson(Of GetPartySummaryQueryResponse)(result)
                End Using

                With oGetPartySummaryResponse

                    If .Item IsNot Nothing Then

                        oPartySummary = New PartySummary()
                        If .PCType IsNot Nothing Then
                            oPartySummary.Party = ConvertPartyPC(.PCType)
                            oPartySummary.Party.XMLDataset = .PCType.XMLDataset
                        ElseIf .CCType IsNot Nothing Then
                            oPartySummary.Party = ConvertpartyCC(.CCType)
                            oPartySummary.Party.XMLDataset = .CCType.XMLDataset
                        End If
                        '  oPartySummary.Party = Convertparty(.Item)
                        oPartySummary.Party.TimeStamp = .PartyTimestamp

                        If .Policies IsNot Nothing Then

                            oPartySummary.Policies = New PolicyCollection()

                            For Each oPolicy As BaseGetPartySummaryResponseTypeRow In .Policies

                                oNewPolicy = New Policy(oPolicy.PolicyRef)

                                With oNewPolicy
                                    .InsuranceFileID = oPolicy.InsuranceFileId
                                    .BranchCode = oPolicy.BranchCode
                                    .BranchKey = oPolicy.BranchKey
                                    .InsuranceFileKey = oPolicy.InsuranceFileKey
                                    .Reference = oPolicy.PolicyRef
                                    .InsuranceFolderKey = oPolicy.InsuranceFolderKey
                                    .PolicyTypeID = oPolicy.PolicyTypeId
                                    .LeadInsurerKey = oPolicy.LeadInsurerKey
                                    .DateIssued = oPolicy.DateIssued
                                    .CoverStartDate = oPolicy.CoverStartDate
                                    .ExpiryDate = oPolicy.ExpiryDate
                                    .RenewalDate = oPolicy.RenewalDate
                                    .InsuredKey = oPolicy.InsuredKey
                                    .ProductKey = oPolicy.ProductKey
                                    .LeadAgentKey = oPolicy.LeadAgentKey
                                    .ThisPremium = oPolicy.ThisPremium
                                    .AnnualPremium = oPolicy.AnnualPremium
                                    .NetPremium = oPolicy.NetPremium
                                    .TaxAmount = oPolicy.TaxAmount
                                    .GeminiPolicyStatus = oPolicy.GeminiPolicyStatus
                                    .PartyShortName = oPolicy.PartyShortName
                                    .ProductCode = oPolicy.ProductCode
                                    .ProductDescription = oPolicy.ProductDesc
                                    .InsuranceFileTypeCode = IIf(oPolicy.InsuranceFileTypeCode Is Nothing, "", oPolicy.InsuranceFileTypeCode)
                                    .PolicyStatusCode = IIf(oPolicy.PolicyStatusCode Is Nothing, "", oPolicy.PolicyStatusCode)
                                    .InsurerShortName = oPolicy.InsurerShortName
                                    .AgentShortName = IIf(oPolicy.AgentShortName Is Nothing, "", oPolicy.AgentShortName)
                                    .PolicyTypeCode = IIf(oPolicy.InsuranceFileTypeCode Is Nothing, "", oPolicy.InsuranceFileTypeCode)
                                    .PolicyTypeDescription = oPolicy.PolicyTypeDesc
                                    .CurrencyCode = oPolicy.CurrencyCode
                                    .AlternativeRef = oPolicy.AlternativeRef
                                    .Regarding = oPolicy.Regarding
                                    .PolicyStatus = IIf(oPolicy.PolicyStatus Is Nothing, "", oPolicy.PolicyStatus)
                                    .RiskTypeDescription = oPolicy.RiskTypeDescription
                                    .EventDesc = oPolicy.EventDescription
                                    .MarkedQuoteForCollection = oPolicy.MarkedForCollection
                                    .IsCurrent = oPolicy.IsCurrent
                                    .BaseInsuranceFolderKey = oPolicy.BaseInsuranceFolderKey
                                    .QuoteVersion = oPolicy.QuoteVersion
                                    .QuoteStatusKey = oPolicy.QuoteStatusKey
                                    .QuoteExpiryDate = oPolicy.QuoteExpiryDate
                                    .RenewedVersion = oPolicy.RenewedVersion
                                    .RiskStatus = oPolicy.RiskStatus
                                    .IsMarketPlacePolicy = oPolicy.IsMarketPlacePolicy
                                    .IsMigratedPolicy = oPolicy.IsMigratedPolicy
                                    .IsReadOnly = oPolicy.IsReadOnly
                                End With

                                oPartySummary.Policies.Add(oNewPolicy)

                            Next

                        End If

                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetPartySummary executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iPartyKey = " & v_iPartyKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If oPartySummary.Policies IsNot Nothing Then
                        sbLogMessage.AppendLine("Returned " & oPartySummary.Policies.Count.ToString & " results" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As FaultException(Of SamMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetPartySummaryRequest = Nothing
                oGetPartySummaryResponse = Nothing
            End Try

            Return oPartySummary

        End SyncLock

    End Function

    Public Overrides Sub ReplacePartyContact(ByVal v_iPartyKey As Integer,
                                            ByVal v_oContactCollection As ContactCollection,
                                            Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oReplacePartyContactRequest As ReplacePartyContactCommand
            Dim oReplacePartyContactResponse As ReplacePartyContactCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oReplacePartyContactRequest = New ReplacePartyContactCommand
                oReplacePartyContactResponse = New ReplacePartyContactCommandResponse
                sbLogMessage = New StringBuilder
                With oReplacePartyContactRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .Contacts = New List(Of BaseClasses.BaseContactType)
                    For Each oNewContact As BaseClasses.BaseContactType In v_oContactCollection

                        .Contacts.Add(oNewContact)
                    Next
                    '.Contacts = v_oContactCollection
                    .PartyKey = v_iPartyKey
                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Put(ApiMethods.ReplacePartyContact, oReplacePartyContactRequest)
                    oReplacePartyContactResponse = ApiClient.DeserializeJson(Of ReplacePartyContactCommandResponse)(result)
                End Using

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("ReplacePartyContact executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("iPartyKey = " & v_iPartyKey & vbCrLf)


                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                oReplacePartyContactRequest = Nothing
                oReplacePartyContactResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub UpdateParty(ByRef r_oParty As BaseParty,
                                Optional ByVal v_sBranchCode As String = Nothing,
                                Optional ByVal v_sSubBranchCode As String = Nothing)

        SyncLock oLock

            Dim oUpdatePartyRequest As UpdatePartyCommand
            Dim oUpdatePartyResponse As UpdatePartyCommandResponse
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oCurrencyColl As NexusProvider.CurrencyCollection
            Dim iContactCount As Integer
            Dim sbLogMessage As StringBuilder

            Try
                oUpdatePartyRequest = New UpdatePartyCommand
                oUpdatePartyResponse = New UpdatePartyCommandResponse
                sbLogMessage = New StringBuilder

                With oUpdatePartyRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                        .SubBranchCode = v_sSubBranchCode
                    End If
                    If TypeOf r_oParty Is PersonalParty Then
                        .BasePartyPCType = ConvertPartyPC(r_oParty)
                        If String.IsNullOrEmpty(.BasePartyPCType.Currency) Then
                            'Currency will be populated with BaseCurrency set in Bo
                            oCurrencyColl = oWebService.GetCurrenciesByBranch(.BasePartyPCType.BranchCode)
                            .BasePartyPCType.Currency = oCurrencyColl(0).BaseCurrencyCode
                        End If
                        iContactCount = .BasePartyPCType.Contacts.Count
                        If iContactCount >= 0 Then
                            For iContactCount = 0 To iContactCount - 1
                                If DirectCast(DirectCast(.BasePartyPCType, BaseClasses.BasePartyType).Contacts(iContactCount).ContactDetail, BaseClasses.BaseContactDetailType).Item = "" Then
                                    .BasePartyPCType.Contacts(iContactCount) = Nothing
                                End If
                            Next
                        End If
                    ElseIf TypeOf r_oParty Is CorporateParty Then
                        .BasePartyCCType = ConvertPartyCC(r_oParty)
                        If String.IsNullOrEmpty(.BasePartyCCType.Currency) Then
                            'Currency will be populated with BaseCurrency set in Bo
                            oCurrencyColl = oWebService.GetCurrenciesByBranch(.BasePartyCCType.BranchCode)
                            .BasePartyCCType.Currency = oCurrencyColl(0).BaseCurrencyCode
                        End If
                        iContactCount = .BasePartyCCType.Contacts.Count
                        If iContactCount >= 0 Then
                            For iContactCount = 0 To iContactCount - 1
                                If DirectCast(DirectCast(.BasePartyCCType, BaseClasses.BasePartyType).Contacts(iContactCount).ContactDetail, BaseClasses.BaseContactDetailType).Item = "" Then
                                    .BasePartyCCType.Contacts(iContactCount) = Nothing
                                End If
                            Next
                        End If
                    ElseIf TypeOf r_oParty Is OtherParty Then
                        .BasePartyOTHERType = ConvertPartyOther(r_oParty)
                        .BasePartyOTHERType.BranchCode = .BranchCode
                        .BasePartyOTHERType.SubBranchCode = .SubBranchCode
                        If String.IsNullOrEmpty(.BasePartyOTHERType.Currency) Then
                            'Currency will be populated with BaseCurrency set in Bo
                            oCurrencyColl = oWebService.GetCurrenciesByBranch(.BasePartyOTHERType.BranchCode)
                            .BasePartyOTHERType.Currency = oCurrencyColl(0).BaseCurrencyCode
                        End If
                        iContactCount = .BasePartyOTHERType.Contacts.Count
                        If iContactCount >= 0 Then
                            For iContactCount = 0 To iContactCount - 1
                                If DirectCast(DirectCast(.BasePartyOTHERType, BaseClasses.BasePartyType).Contacts(iContactCount).ContactDetail, BaseClasses.BaseContactDetailType).Item = "" Then
                                    .BasePartyOTHERType.Contacts(iContactCount) = Nothing
                                End If
                            Next
                        End If
                    End If

                    .PartyKey = r_oParty.Key

                    ' If TimeStamp is missing (e.g. party loaded from session without a fresh GetParty call),
                    ' fetch it now to avoid a 400 Bad Request from the REST API optimistic concurrency check.
                    If r_oParty.TimeStamp Is Nothing OrElse r_oParty.TimeStamp.Length = 0 Then
                        Dim oFreshParty As BaseParty = GetParty(r_oParty.Key, v_sBranchCode)
                        If oFreshParty IsNot Nothing AndAlso oFreshParty.TimeStamp IsNot Nothing Then
                            r_oParty.TimeStamp = oFreshParty.TimeStamp
                        End If
                    End If

                    .PartyTimestamp = r_oParty.TimeStamp
                    .SubBranchCode = v_sSubBranchCode

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Put(ApiMethods.UpdateParty, oUpdatePartyRequest)
                    oUpdatePartyResponse = ApiClient.DeserializeJson(Of UpdatePartyCommandResponse)(result)
                End Using

                With oUpdatePartyResponse.UpdatePartyResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oParty.TimeStamp = .PartyTimestamp
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateParty executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oParty = " & r_oParty.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sSubBranchCode) Then
                        sbLogMessage.AppendLine("v_sSubBranchCode = " & v_sSubBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sSubBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oUpdatePartyRequest = Nothing
                oUpdatePartyResponse = Nothing
            End Try

        End SyncLock

    End Sub

    ''' <summary>
    ''' This Function is used to Get address details of a client depending upon the �AddressKey� Passed as parameter to the method. 
    ''' This method takes �AddressKey� as input and returns an object of �Address� 
    ''' Class.
    ''' </summary>
    ''' <param name="v_iAddressKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetAddress(ByVal v_iAddressKey As Integer,
                                            Optional ByVal v_iPartyKey As Integer = Nothing,
                                            Optional ByVal v_sBranchCode As String = Nothing) As Address

        SyncLock oLock

            Dim oGetAddressRequest As BaseClasses.GetAddressQuery
            Dim oGetAddressResponse As BaseClasses.GetAddressQueryResponse
            Dim oAddress As Address
            Dim sbLogMessage As StringBuilder

            Try
                oGetAddressRequest = New BaseClasses.GetAddressQuery
                oGetAddressResponse = New BaseClasses.GetAddressQueryResponse
                sbLogMessage = New StringBuilder


                With oGetAddressRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    .AddressKey = v_iAddressKey

                    If v_iPartyKey > 0 Then
                        .PartyKey = v_iPartyKey
                        .PartyKeySpecified = True
                    End If

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetAddress, oGetAddressRequest)
                    oGetAddressResponse = ApiClient.DeserializeJson(Of BaseClasses.GetAddressQueryResponse)(result)
                End Using

                With oGetAddressResponse.GetAddressResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        oAddress = New Address()


                        Dim code = If(.Address.AddressTypeCode, AddressTypeType.Correspondence)
                        oAddress.Key = v_iAddressKey
                        oAddress.Address1 = .Address.AddressLine1
                        oAddress.Address2 = .Address.AddressLine2
                        oAddress.Address3 = .Address.AddressLine3
                        oAddress.Address4 = .Address.AddressLine4
                        oAddress.AddressType = code
                        oAddress.CountryCode = .Address.CountryCode
                        oAddress.PostCode = .Address.PostCode
                        oAddress.Address5 = .Address.AddressLine5
                        oAddress.Address6 = .Address.AddressLine6
                        oAddress.Address7 = .Address.AddressLine7
                        oAddress.Address8 = .Address.AddressLine8
                        oAddress.Address9 = .Address.AddressLine9
                        oAddress.Address10 = .Address.AddressLine10

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetAddress executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iAddressKey = " & v_iAddressKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oAddress.Print.Replace("<br />", vbCrLf) & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If
            Catch ex As Exception
                Throw
            Finally
                oGetAddressRequest = Nothing
                oGetAddressResponse = Nothing
            End Try

            Return oAddress

        End SyncLock
    End Function

    '#Region "WPR12 - Manage Party Bank Details"
    Public Overrides Sub UpdatePartyBankDetails(ByVal v_iPartyKey As Integer,
                                 ByVal vPartyBankDetails As BankCollection,
                                 Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oUpdatePartyBankDetailsRequest As UpdatePartyBankDetailsCommand
            Dim oUpdatePartyBankDetailsResponse As UpdatePartyBankDetailsCommandResponse
            Dim oBankDetails As BasePartyBankType
            Dim sbLogMessage As StringBuilder

            Try
                oUpdatePartyBankDetailsRequest = New UpdatePartyBankDetailsCommand
                oUpdatePartyBankDetailsResponse = New UpdatePartyBankDetailsCommandResponse
                sbLogMessage = New StringBuilder


                With oUpdatePartyBankDetailsRequest
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iPartyKey > 0 Then
                        .PartyKey = v_iPartyKey
                    Else
                        Throw New ArgumentNullException("PartyKey")
                    End If

                    If vPartyBankDetails IsNot Nothing AndAlso vPartyBankDetails.Count > 0 Then


                        .PartyBankDetails = New List(Of BaseClasses.BasePartyBankType)
                        For iCount As Integer = 0 To vPartyBankDetails.Count - 1
                            oBankDetails = New BaseClasses.BasePartyBankType

                            oBankDetails.AccountHolderName = vPartyBankDetails.Item(iCount).AccountHolderName
                            oBankDetails.AccountType = vPartyBankDetails.Item(iCount).AccountType
                            oBankDetails.BankPaymentTypeCode = vPartyBankDetails.Item(iCount).BankPaymentTypeCode
                            oBankDetails.Bank = New BaseClasses.BaseBankType
                            oBankDetails.Bank.AccountNumber = vPartyBankDetails.Item(iCount).AccountNumber
                            oBankDetails.Bank.BIC = vPartyBankDetails.Item(iCount).BIC
                            oBankDetails.Bank.IBAN = vPartyBankDetails.Item(iCount).IBAN

                            If vPartyBankDetails.Item(iCount).PartyBankAddress IsNot Nothing Then
                                oBankDetails.Bank.BankAddress = New BaseSimpleAddressType
                                oBankDetails.Bank.BankAddress.AddressLine1 = vPartyBankDetails.Item(iCount).PartyBankAddress.Address1
                                oBankDetails.Bank.BankAddress.AddressLine2 = vPartyBankDetails.Item(iCount).PartyBankAddress.Address2
                                oBankDetails.Bank.BankAddress.AddressLine3 = vPartyBankDetails.Item(iCount).PartyBankAddress.Address3
                                oBankDetails.Bank.BankAddress.AddressLine4 = vPartyBankDetails.Item(iCount).PartyBankAddress.Address4
                                oBankDetails.Bank.BankAddress.CountryCode = vPartyBankDetails.Item(iCount).PartyBankAddress.CountryCode
                                oBankDetails.Bank.BankAddress.PostCode = vPartyBankDetails.Item(iCount).PartyBankAddress.PostCode
                            End If

                            oBankDetails.Bank.BankCode = vPartyBankDetails.Item(iCount).BankCode
                            oBankDetails.Bank.Branch = vPartyBankDetails.Item(iCount).BankBranch
                            oBankDetails.Bank.BranchCode = vPartyBankDetails.Item(iCount).BranchCode
                            oBankDetails.IsBankItem = True 'To verify as the other option Credit Card is not required for ETANA
                            oBankDetails.BankPaymentTypeCode = vPartyBankDetails.Item(iCount).BankPaymentTypeCode
                            oBankDetails.IsDeleted = False
                            oBankDetails.PartyBankKey = vPartyBankDetails.Item(iCount).PartyBankKey
                            oBankDetails.AccountKey = vPartyBankDetails.Item(iCount).AccountKey
                            If vPartyBankDetails.Item(iCount).CreditCard IsNot Nothing Then
                                Dim oCreditCard As New BaseCreditCardType
                                oCreditCard.Number = vPartyBankDetails.Item(iCount).CreditCard.Number
                                oCreditCard.AuthCode = vPartyBankDetails.Item(iCount).CreditCard.AuthCode
                                oCreditCard.ManualAuthCode = vPartyBankDetails.Item(iCount).CreditCard.ManualAuthCode
                                oCreditCard.ExpiryDate = vPartyBankDetails.Item(iCount).CreditCard.ExpiryDate
                                oCreditCard.PartyBankKey = vPartyBankDetails.Item(iCount).CreditCard.PartyBankKey
                                oCreditCard.NameOnCreditCard = vPartyBankDetails.Item(iCount).CreditCard.NameOnCreditCard
                                oCreditCard.TrackingNumber = vPartyBankDetails.Item(iCount).CreditCard.TrackingNumber
                                oCreditCard.IsDefaultCreditCard = vPartyBankDetails.Item(iCount).CreditCard.IsDefaultCreditCard
                                oCreditCard.IsRegisteredCardHolder = True
                                oBankDetails.CreditCard = oCreditCard
                                oBankDetails.IsBankItem = False

                            End If
                            .PartyBankDetails.Add(oBankDetails)
                        Next

                    Else
                        Throw New ArgumentNullException("vPartyBankDetails")
                    End If
                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Put(ApiMethods.UpdatePartyBankDetails, oUpdatePartyBankDetailsRequest)
                    oUpdatePartyBankDetailsResponse = ApiClient.DeserializeJson(Of UpdatePartyBankDetailsCommandResponse)(result)
                End Using

                With oUpdatePartyBankDetailsResponse.UpdatePartyBankDetailsResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdatePartyBankDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iPartyKey = " & v_iPartyKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("vPartyBankDetails = " & vPartyBankDetails.Print.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                oUpdatePartyBankDetailsRequest = Nothing
                oUpdatePartyBankDetailsResponse = Nothing
            End Try


        End SyncLock
    End Sub
    '#End Region

    Public Overrides Function GetClientDataExtract(ByVal nPartyCnt As Integer, ByVal sFilePassword As String) As Byte()
        SyncLock oLock

            Dim oGetClientDataExtractRequest As GetClientDataExtractQueryBase
            Dim oGetClientDataExtractResponse As GetClientDataExtractQueryBaseResponse
            Dim sbLogMessage As StringBuilder
            Dim abClientDataExtract As Byte() = Nothing

            Try
                oGetClientDataExtractRequest = New GetClientDataExtractQueryBase
                oGetClientDataExtractResponse = New GetClientDataExtractQueryBaseResponse
                sbLogMessage = New StringBuilder

                With oGetClientDataExtractRequest
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode
                    End If

                    If String.IsNullOrEmpty(nPartyCnt) Then
                        Throw New ArgumentNullException("PartyCnt")
                    Else
                        .PartyCnt = nPartyCnt
                    End If

                    If String.IsNullOrEmpty(sFilePassword) Then
                        Throw New ArgumentNullException("FilePassword")
                    Else
                        .FilePassword = sFilePassword
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetClientDataExtract, oGetClientDataExtractRequest)
                    oGetClientDataExtractResponse = ApiClient.DeserializeJson(Of GetClientDataExtractQueryBaseResponse)(result)
                End Using

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetClientDataExtract executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("nPartyCnt = " & nPartyCnt.ToString() & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If
                abClientDataExtract = oGetClientDataExtractResponse.ClientDataFile
            Catch ex As Exception
                Throw
            Finally
                oGetClientDataExtractRequest = Nothing
                oGetClientDataExtractResponse = Nothing
            End Try

            Return abClientDataExtract
        End SyncLock
    End Function
    Public Overrides Function CheckRetainedCoInsurerExists(ByVal v_CoInsurerKeys As CoInsurersCollections, Optional ByVal v_sBranchCode As String = Nothing) As Boolean

        Dim oGetRetainedCoInsurerRequest As CheckRetainedCoInsurerExistsCommand     ' Request Type
        Dim oGetRetainedCoInsurerResponse As CheckRetainedCoInsurerExistsCommandResponse   ' Response Type
        Dim bIsRetainedExists As Boolean

        Dim sbLogMessage As StringBuilder

        Try
            oGetRetainedCoInsurerRequest = New CheckRetainedCoInsurerExistsCommand
            oGetRetainedCoInsurerResponse = New CheckRetainedCoInsurerExistsCommandResponse
            sbLogMessage = New StringBuilder

            Dim lCoInsurersKeys As New List(Of Integer)

            For Each coinsurer As CoInsurers In v_CoInsurerKeys
                lCoInsurersKeys.Add(coinsurer.CoInsurerKey)
            Next

            With oGetRetainedCoInsurerRequest
                'if the passed parameter v_sBranchCode is empty 
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    .BranchCode = v_sBranchCode

                End If
                .CoInsurerKeys = lCoInsurersKeys
            End With


            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Post(ApiMethods.GetRetainedCoInsurer, oGetRetainedCoInsurerRequest)
                oGetRetainedCoInsurerResponse = ApiClient.DeserializeJson(Of CheckRetainedCoInsurerExistsCommandResponse)(result)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object


            With oGetRetainedCoInsurerResponse 'With Response Type

                bIsRetainedExists = oGetRetainedCoInsurerResponse.IsChecked
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("CheckRetainedCoInsurerExistsexecuted ok" & vbCrLf)
                sbLogMessage.AppendLine("Input: PartyKey " & String.Join(",", v_CoInsurerKeys) & vbCrLf)
                If Not IsNothing(v_sBranchCode) Then
                    sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                Else
                    sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                End If
                LogMessageEntry(sbLogMessage)
            End If

        Catch ex As FaultException(Of SAMMethodResponseData)

            '''''''FaultErrorHandler(ex) ' handling fault error messages 

        Catch ex As Exception
            Throw (ex)
        Finally
            oGetRetainedCoInsurerRequest = Nothing
            oGetRetainedCoInsurerResponse = Nothing
        End Try
        Return bIsRetainedExists
    End Function
End Class
