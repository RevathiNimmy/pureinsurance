Imports NUnit.Framework

<TestFixture()> _
Public Class UpdateParty
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nPartyCnt As Integer
    Private m_btPartyTimeStamp() As Byte

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        'HD_AgentKey
        HD_BranchCode
        HD_PartyKey
        HD_PartyTimestamp
        PC_BranchCode
        PC_Surname
        PC_Forename
        PC_Title
        PC_GenderCode
        PC_DateOfBirth
        CC_BranchCode
        CC_CompanyName
        AD_AddressLine1
        AD_AddressLine2
        AD_PostCode
        CN_ContactItem
    End Enum

    Private Enum enumInvalidLookup
        HD_BranchCode
        HD_SubBranchCode
        'GenderCode
        'Title
        'MaritalStatusCode
        None
    End Enum

    Private Enum enumPartyType
        PartyPC
        PartyCC
        PartyOther
    End Enum

    Private Enum enumSTSBusinessError
        None
        'InvalidAgentKey
    End Enum

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.UpdatePartyRequestType, _
                                    ByVal eTestCases As enumTestCases, _
                                    ByVal ePartyType As enumPartyType)
        If epartytype = enumPartyType.PartyOther Then

            Dim oOtherParty As ProxyWS.BasePartyOTHERType
            oOtherParty = TryCast(oRequest.Item, ProxyWS.BasePartyOTHERType)
            If oOtherParty IsNot Nothing Then
                If eTestCases = enumTestCases.AddConvictionAccident Then
                    For cntConv As Integer = oOtherParty.Conviction.GetLowerBound(0) To _
                                                oOtherParty.Conviction.GetUpperBound(0)


                        oOtherParty.Conviction(cntConv).ConvictionKey = 0
                    Next
                    For cntAcci As Integer = oOtherParty.Accident.GetLowerBound(0) To _
                                                oOtherParty.Accident.GetUpperBound(0)

                        oOtherParty.Accident(cntAcci).AccidentKey = 0
                    Next
                    oRequest.Item = oOtherParty
                ElseIf eTestCases = enumTestCases.AllMandatoryData Then
                    oOtherParty.Conviction = Nothing
                    oOtherParty.Accident = Nothing
                    oOtherParty.Addresses = Nothing
                    oOtherParty.Contacts = Nothing
                    oOtherParty.SupplierBusiness = Nothing
                    oRequest.Item = oOtherParty
                ElseIf eTestCases = enumTestCases.InvalidAccidentId Then
                    For cntAcci As Integer = oOtherParty.Accident.GetLowerBound(0) To _
                                                oOtherParty.Accident.GetUpperBound(0)
                        oOtherParty.Accident(cntAcci).AccidentKey = "90909"
                    Next
                    oRequest.Item = oOtherParty
                ElseIf eTestCases = enumTestCases.InvalidConvictionId Then
                    For cntConv As Integer = oOtherParty.Conviction.GetLowerBound(0) To _
                        oOtherParty.Conviction.GetUpperBound(0)
                        oOtherParty.Conviction(cntConv).ConvictionKey = "90909"
                    Next
                    oRequest.Item = oOtherParty
                ElseIf eTestCases = enumTestCases.MultipleAddWithNoContacts Then
                    For cntAdd As Integer = oOtherParty.Addresses.GetLowerBound(0) To _
                                                oOtherParty.Addresses.GetUpperBound(0)
                        oOtherParty.Addresses(cntAdd).Contacts = Nothing
                    Next
                ElseIf eTestCases = enumTestCases.AddUpdateConvictionAccident Then
                    For cntConv As Integer = oOtherParty.Conviction.GetLowerBound(0) To _
                                                oOtherParty.Conviction.GetUpperBound(0)
                        If cntConv = 1 Then
                            oOtherParty.Conviction(cntConv).ConvictionKey = 0
                        End If
                    Next
                    For cntAcci As Integer = oOtherParty.Accident.GetLowerBound(0) To _
                                                oOtherParty.Accident.GetUpperBound(0)

                        If cntAcci = 1 Then
                            oOtherParty.Accident(cntAcci).AccidentKey = 0
                        End If
                    Next
                    oRequest.Item = oOtherParty
                End If
            End If
        End If
    End Sub

    Private Enum enumTestCases
        None
        AddConvictionAccident
        AddUpdateConvictionAccident
        AllMandatoryData
        MultipleAddWithNoContacts
        MissingConvictionId
        MissingAccidentId
        InvalidConvictionId
        InvalidAccidentId

    End Enum

#End Region

#Region "Private Setup Preconditions"

    Private Sub AddParty(ByVal PartyType As Integer)

        Dim oAddParty As New AddParty
        oAddParty.SupportMethod(m_nPartyCnt, m_btPartyTimeStamp, PartyType)

    End Sub

    Private Sub GetParty()

        Dim oGetParty As New GetParty
        oGetParty.SupportMethod(m_btPartyTimeStamp)

    End Sub

    Private Sub GetSpecifiedParty(ByVal PartyKey As Integer)
        Dim oGetParty As New GetParty
        oGetParty.SupportMethod(PartyKey, m_btPartyTimeStamp)
    End Sub

#End Region

#Region "Private Methods"

    Private Sub UpdateOtherPartyTest()

        Dim oRequest As New ProxyWS.UpdatePartyRequestType
        Dim oResponse As ProxyWS.UpdatePartyResponseType = Nothing
        Dim oParty As ProxyWS.BasePartyType
        Dim oPartyOther As New ProxyWS.BasePartyOTHERType

        Dim oUpdatePartyTestData As UpdatePartyOtherTestData = m_oTestData.UpdatePartyOtherTestData

        'AddParty(enumPartyType.PartyOther)

        m_nPartyCnt = 1186

        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")
        Dim oGetPartyRequest As ProxyWS.GetPartyRequestType = New ProxyWS.GetPartyRequestType

        oGetPartyRequest.BranchCode = "HEADOFF"
        oGetPartyRequest.PartyKey = m_nPartyCnt
        Dim oGetPartyResponse As ProxyWS.GetPartyResponseType = oProxy.GetParty(oGetPartyRequest)

        oPartyOther.AccountExecutive = String.Empty
        oPartyOther.ActiveIndicator = True
        oPartyOther.ActiveIndicatorSpecified = True
        oPartyOther.AfterHoursIndicator = True
        oPartyOther.AfterHoursIndicatorSpecified = True
        oPartyOther.BranchCode = "HEADOFF"
        oPartyOther.Code = "SAMOT1"
        oPartyOther.Currency = "GBP"
        oPartyOther.DateOfBirth = "01-01-1970"
        oPartyOther.DomiciledForTax = True
        oPartyOther.DomiciledForTaxSpecified = True
        oPartyOther.DriverStatusCode = "HV"
        oPartyOther.Gender = "M"
        oPartyOther.LicenseNumber = "LCNO1212443"
        oPartyOther.LicenseTypeCode = "lTM"
        oPartyOther.Name = "SAMOtherParty"
        oPartyOther.PriorityIndicator = 1
        oPartyOther.PriorityIndicatorSpecified = True
        oPartyOther.RegNumber = "REGNO123"
        oPartyOther.TaxExempt = False
        oPartyOther.TaxExemptSpecified = True
        oPartyOther.TaxNumber = "TAXNUMBER123"
        oPartyOther.TaxPercentage = 4.532
        oPartyOther.TaxPercentageSpecified = True
        oPartyOther.TPIntroducer = String.Empty
        oPartyOther.TPUserCode = String.Empty
        oPartyOther.TypeCode = "OTSUPPLIER"

        Dim oGetPartyOther As ProxyWS.BasePartyOTHERType = TryCast(oGetPartyResponse.Item, ProxyWS.BasePartyOTHERType)
        If oGetPartyOther IsNot Nothing Then

            oPartyOther.Accident = oGetPartyOther.Accident
            If oPartyOther.Accident IsNot Nothing Then
                oPartyOther.Accident(0).AccidentKey = 0
            End If

            oPartyOther.Conviction = oGetPartyOther.Conviction
            If oPartyOther.Conviction IsNot Nothing Then
                oPartyOther.Conviction(0).ConvictionKey = 0
            End If

            oPartyOther.SupplierBusiness = oGetPartyOther.SupplierBusiness
            If oPartyOther.SupplierBusiness Is Nothing Then

                Dim oListOfSupplierBusiness As New List(Of ProxyWS.BasePartyOTHERTypeSupplierBusiness)
                Dim oPartySupplierBusiness As ProxyWS.BasePartyOTHERTypeSupplierBusiness
                oPartySupplierBusiness = New ProxyWS.BasePartyOTHERTypeSupplierBusiness

                oPartySupplierBusiness.BusinessCode = "SBM"
                oPartySupplierBusiness.SpecialityCode = "SSM"

                oListOfSupplierBusiness.Add(oPartySupplierBusiness)

                oPartySupplierBusiness = New ProxyWS.BasePartyOTHERTypeSupplierBusiness

                oPartySupplierBusiness.BusinessCode = "SBM"
                oPartySupplierBusiness.SpecialityCode = "PS"

                oListOfSupplierBusiness.Add(oPartySupplierBusiness)

                oPartySupplierBusiness = New ProxyWS.BasePartyOTHERTypeSupplierBusiness

                oPartySupplierBusiness.BusinessCode = "Printer"
                oPartySupplierBusiness.SpecialityCode = "SSM"

                oListOfSupplierBusiness.Add(oPartySupplierBusiness)

                oPartySupplierBusiness = New ProxyWS.BasePartyOTHERTypeSupplierBusiness

                oPartySupplierBusiness.BusinessCode = "Printer"
                oPartySupplierBusiness.SpecialityCode = "PS"

                oListOfSupplierBusiness.Add(oPartySupplierBusiness)

                oPartySupplierBusiness = New ProxyWS.BasePartyOTHERTypeSupplierBusiness

                oPartySupplierBusiness.BusinessCode = "FS"
                oPartySupplierBusiness.SpecialityCode = "SSM"

                oListOfSupplierBusiness.Add(oPartySupplierBusiness)

                oPartySupplierBusiness = New ProxyWS.BasePartyOTHERTypeSupplierBusiness

                oPartySupplierBusiness.BusinessCode = "FS"
                oPartySupplierBusiness.SpecialityCode = Nothing

                oListOfSupplierBusiness.Add(oPartySupplierBusiness)

                oPartyOther.SupplierBusiness = oListOfSupplierBusiness.ToArray

            Else
                oPartyOther.SupplierBusiness = oGetPartyOther.SupplierBusiness
            End If

        Else
            oPartyOther.Accident = Nothing
            oPartyOther.Conviction = Nothing
            oPartyOther.SupplierBusiness = Nothing


        End If

        oPartyOther.Addresses = oUpdatePartyTestData.ListOfAddressesWithContacts.ToArray
        oPartyOther.Contacts = oUpdatePartyTestData.ListOfContacts.ToArray

        ' assign the other party to the partytype object
        oParty = oPartyOther

        ' assign the partytype object to the request
        oRequest.Item = oParty

        oRequest.BranchCode = "HEADOFF"
        oRequest.PartyKey = m_nPartyCnt
        oRequest.PartyTimestamp = oGetPartyResponse.PartyTimestamp
        oRequest.SubBranchCode = "HEADOFF"

        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        oResponse = oProxy.UpdateParty(oRequest)

        If oResponse.Errors IsNot Nothing Then
            Debug.Assert(True)
        End If

    End Sub


    Private Sub UpdatePartyTest( _
        Optional ByVal ePartyType As enumPartyType = enumPartyType.PartyPC, _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByRef r_nPartyCnt As Integer = 0, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None, _
        Optional ByVal TestCases As enumTestCases = enumTestCases.None)

        Dim oRequest As New ProxyWS.UpdatePartyRequestType
        Dim oResponse As ProxyWS.UpdatePartyResponseType
        Dim oParty As New ProxyWS.BasePartyType
        Dim nBusinessError As Integer = 274

        If ePartyType = enumPartyType.PartyOther Then
            UpdateOtherPartyTest()
            Exit Sub
        End If

        ' If no party cnt specified in the test data, create a new one
        If m_oTestData.PartyCnt = 0 Then
            AddParty(ePartyType)
        Else
            m_nPartyCnt = m_oTestData.PartyCnt
        End If
        Try
            ' Set the specific condition on the input class

            If ePartyType = enumPartyType.PartyPC Then
                Dim oPartyPC As New ProxyWS.BasePartyPCType
                If eMissingData <> enumMissingData.PC_BranchCode Then
                    If eInvalidLookup = enumInvalidLookup.HD_BranchCode Then
                        oPartyPC.BranchCode = m_oTestData.InvalidLookupCode
                    Else
                        oPartyPC.BranchCode = m_oTestData.BranchCode
                    End If
                End If
                If eMissingData <> enumMissingData.PC_Forename Then oPartyPC.Forename = m_oTestData.Forename
                If eMissingData <> enumMissingData.PC_Surname Then oPartyPC.Surname = m_oTestData.Surname
                If eMissingData <> enumMissingData.PC_DateOfBirth Then oPartyPC.DateOfBirth = m_oTestData.DateOfBirth
                oPartyPC.DateOfBirthSpecified = (oPartyPC.DateOfBirth <> New Date)
                oPartyPC.Initials = m_oTestData.Initials
                If eMissingData <> enumMissingData.PC_Title Then oPartyPC.Title = m_oTestData.Title
                oPartyPC.TPIntroducer = m_oTestData.TPIntroducer
                oPartyPC.TPUserCode = m_oTestData.TPUser
                oPartyPC.MaritalStatusCode = m_oTestData.MaritalStatusCode
                oPartyPC.MaritalStatusCodeSpecified = True
                oPartyPC.EmploymentStatusCode = m_oTestData.EmploymentStatusCode
                oPartyPC.EmploymentStatusCodeSpecified = True
                oPartyPC.AlternativeId = m_oTestData.AlternativeId
                oPartyPC.EmployersBusinessCode = m_oTestData.EmployerBusinessCode
                oPartyPC.OccupationCode = m_oTestData.OccupationCode
                If eMissingData <> enumMissingData.PC_GenderCode Then oPartyPC.GenderCode = m_oTestData.GenderCode
                oParty = oPartyPC
            ElseIf ePartyType = enumPartyType.PartyCC Then
                Dim oPartyCC As New ProxyWS.BasePartyCCType
                If eMissingData <> enumMissingData.CC_BranchCode Then oPartyCC.BranchCode = m_oTestData.BranchCode
                If eMissingData <> enumMissingData.CC_CompanyName Then oPartyCC.CompanyName = m_oTestData.CompanyName
                oPartyCC.BusinessCode = m_oTestData.BusinessCode
                oPartyCC.TPIntroducer = m_oTestData.TPIntroducer
                oPartyCC.TPUserCode = m_oTestData.TPUser
                oParty = oPartyCC
            ElseIf ePartyType = enumPartyType.PartyOther Then 'GAURAV

                Dim oPartyOther As New ProxyWS.BasePartyOTHERType

                Dim oConviction As ProxyWS.BasePartyOTHERTypeConviction
                For cnt As Integer = m_oTestData.Convictions.GetLowerBound(0) To m_oTestData.Convictions.GetUpperBound(0)
                    oConviction = New ProxyWS.BasePartyOTHERTypeConviction
                    ReDim Preserve oPartyOther.Conviction(cnt)

                    oConviction.AlcoholLevel = m_oTestData.Convictions(cnt).AlcoholLevel
                    oConviction.AlcoholMeasurementMethod = m_oTestData.Convictions(cnt).AlcoholMeasurementMethod
                    oConviction.Date = m_oTestData.Convictions(cnt).Date
                    oConviction.Description = m_oTestData.Convictions(cnt).Description
                    oConviction.DrivingLicencePenaltyPoints = m_oTestData.Convictions(cnt).DrivingLicencePenaltyPoints
                    oConviction.FineAmount = m_oTestData.Convictions(cnt).FineAmount
                    oConviction.SentenceDescription = m_oTestData.Convictions(cnt).SentenceDescription
                    oConviction.SentenceDuration = m_oTestData.Convictions(cnt).SentenceDuration
                    oConviction.SentenceDurationQualifier = m_oTestData.Convictions(cnt).SentenceDurationQualifier

                    oConviction.SentenceEffectiveDate = m_oTestData.Convictions(cnt).SentenceEffectiveDate
                    oConviction.SentenceTypeCode = m_oTestData.Convictions(cnt).SentenceTypeCode
                    oConviction.StatusCode = m_oTestData.Convictions(cnt).StatusCode

                    oConviction.TypeCode = m_oTestData.Convictions(cnt).TypeCode

                    oPartyOther.Conviction(cnt) = oConviction
                Next

                Dim oAccident As ProxyWS.BasePartyOTHERTypeAccident
                For cnt As Integer = m_oTestData.Accidents.GetLowerBound(0) To m_oTestData.Accidents.GetUpperBound(0)
                    ReDim Preserve oPartyOther.Accident(cnt)
                    oAccident = New ProxyWS.BasePartyOTHERTypeAccident

                    oAccident.Date = m_oTestData.Accidents(cnt).Date
                    oAccident.Description = m_oTestData.Accidents(cnt).Description
                    oAccident.IsAtFault = m_oTestData.Accidents(cnt).IsAtFault <> 0
                    oPartyOther.Accident(cnt) = oAccident
                Next
                Dim oSuppBusiness As ProxyWS.BasePartyOTHERTypeSupplierBusiness
                If m_oTestData.SuppBusiness IsNot Nothing Then
                    For cnt As Integer = m_oTestData.SuppBusiness.GetLowerBound(0) To m_oTestData.SuppBusiness.GetUpperBound(0)
                        ReDim Preserve oPartyOther.SupplierBusiness(cnt)
                        oSuppBusiness = New ProxyWS.BasePartyOTHERTypeSupplierBusiness

                        oSuppBusiness.BusinessCode = m_oTestData.SuppBusiness(cnt).BusinessCode
                        oSuppBusiness.SpecialityCode = m_oTestData.SuppBusiness(cnt).SpecialityCode

                        oPartyOther.SupplierBusiness(cnt) = oSuppBusiness
                    Next
                End If

                'Dim oOtherPartyInfo As New ProxyWS.BasePartyOTHERType

                oPartyOther.ActiveIndicator = m_oTestData.OtherPartyInfo.ActiveIndicator
                oPartyOther.AfterHoursIndicator = m_oTestData.OtherPartyInfo.AfterHoursIndicator
                oPartyOther.Code = m_oTestData.OtherPartyInfo.Code

                oPartyOther.DateOfBirth = m_oTestData.OtherPartyInfo.DateOfBirth
                oPartyOther.DriverStatusCode = m_oTestData.OtherPartyInfo.DriverStatusCode
                oPartyOther.Gender = m_oTestData.OtherPartyInfo.Gender

                oPartyOther.LicenseNumber = m_oTestData.OtherPartyInfo.LicenseNumber
                oPartyOther.LicenseTypeCode = m_oTestData.OtherPartyInfo.LicenseTypeCode
                oPartyOther.Name = m_oTestData.OtherPartyInfo.Name

                oPartyOther.PriorityIndicator = m_oTestData.OtherPartyInfo.PriorityIndicator
                oPartyOther.RegNumber = m_oTestData.OtherPartyInfo.RegNumber
                oPartyOther.TypeCode = m_oTestData.OtherPartyInfo.TypeCode

                'opartyother.
                'for m_otestdata.
                oParty = oPartyOther
            End If '-------------------------------------------------------------------

            Dim oAddress As ProxyWS.BaseAddressWithContactsType
            ReDim oParty.Addresses(m_oTestData.Addresses.GetUpperBound(0))
            For iCnt As Integer = m_oTestData.Addresses.GetLowerBound(0) To m_oTestData.Addresses.GetUpperBound(0)
                oAddress = New ProxyWS.BaseAddressWithContactsType
                oAddress.AddressTypeCode = m_oTestData.Addresses(iCnt).TypeCode
                If eMissingData <> enumMissingData.AD_AddressLine1 Then oAddress.AddressLine1 = m_oTestData.Addresses(iCnt).Line1
                If eMissingData <> enumMissingData.AD_AddressLine2 Then oAddress.AddressLine2 = m_oTestData.Addresses(iCnt).Line2
                oAddress.AddressLine3 = m_oTestData.Addresses(iCnt).Line3
                oAddress.AddressLine4 = m_oTestData.Addresses(iCnt).Line4
                If eMissingData <> enumMissingData.AD_PostCode Then oAddress.PostCode = m_oTestData.Addresses(iCnt).PostCode
                oAddress.CountryCode = m_oTestData.Addresses(iCnt).CountryCode

                Dim uBoundContacts As Integer = m_oTestData.Addresses(iCnt).Contact.GetUpperBound(0)
                Dim lBoundContacts As Integer = m_oTestData.Addresses(iCnt).Contact.GetLowerBound(0)
                ReDim oAddress.Contacts(uBoundContacts)
                Dim oAddressContact As New ProxyWS.BaseContactType

                For icntContacts As Integer = lBoundContacts To uBoundContacts
                    'Dim AssignContact As ProxyWS.BaseContactType = ProxyWS.UpdatePartyRequest.Item.Addresses(iCnt%).Contacts(icntContacts)
                    oAddressContact.AreaCode = m_oTestData.Addresses(iCnt).Contact(icntContacts).AreaCode
                    oAddressContact.ContactDetail = New ProxyWS.BaseContactDetailType
                    If m_oTestData.Addresses(iCnt).Contact(icntContacts).ElementName = 0 Then
                        oAddressContact.ContactDetail.ItemElementName = ProxyWS.ItemChoiceType.Number
                    Else
                        oAddressContact.ContactDetail.ItemElementName = ProxyWS.ItemChoiceType.EmailAddress
                    End If
                    oAddressContact.ContactTypeCode = m_oTestData.Addresses(iCnt).Contact(icntContacts).Type
                    oAddressContact.ContactDetail.Item = m_oTestData.Addresses(iCnt).Contact(icntContacts).Item
                    oAddress.Contacts(icntContacts) = oAddressContact
                Next

                oParty.Addresses(iCnt) = oAddress
            Next

            ReDim oParty.Contacts(m_oTestData.Contacts.GetUpperBound(0))
            Dim oContact As ProxyWS.BaseContactType
            Dim oContactDetail As ProxyWS.BaseContactDetailType
            For iCnt As Integer = m_oTestData.Contacts.GetLowerBound(0) To m_oTestData.Contacts.GetUpperBound(0)
                oContact = New ProxyWS.BaseContactType
                oContactDetail = New ProxyWS.BaseContactDetailType
                oContact.AreaCode = m_oTestData.Contacts(iCnt).AreaCode
                oContact.ContactTypeCode = m_oTestData.Contacts(iCnt).Type
                oContactDetail.ItemElementName = m_oTestData.Contacts(iCnt).ElementName
                If eMissingData <> enumMissingData.CN_ContactItem Then oContactDetail.Item = m_oTestData.Contacts(iCnt).Item
                oContact.ContactDetail = oContactDetail
                oParty.Contacts(iCnt) = oContact
            Next

            'GAURAV

            With oRequest
                If eMissingData <> enumMissingData.HD_BranchCode Then
                    If eInvalidLookup = enumInvalidLookup.HD_BranchCode Then
                        .BranchCode = m_oTestData.InvalidLookupCode
                    Else
                        .BranchCode = m_oTestData.BranchCode
                    End If
                End If
                If eMissingData <> enumMissingData.HD_PartyKey Then
                    .PartyKey = m_nPartyCnt
                End If
                If eMissingData <> enumMissingData.HD_PartyTimestamp Then
                    .PartyTimestamp = m_btPartyTimeStamp
                End If
                If eInvalidLookup = enumInvalidLookup.HD_SubBranchCode Then
                    .SubBranchCode = m_oTestData.InvalidLookupCode
                Else
                    .SubBranchCode = m_oTestData.SubBranch
                End If
                .Item = oParty
            End With

            ProcessTestCases(oRequest, TestCases, ePartyType)

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.UpdateParty(oRequest)

            With oResponse
                If eMissingData <> enumMissingData.None Then
                    ' Missing Data tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorInvalidData(oResponse, 0, 100, "Mandatory " & Mid(eMissingData.ToString, 4) & " is missing")
                ElseIf eInvalidLookup <> enumInvalidLookup.None Then
                    ' Invalid Lookup tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)
                    Select Case eInvalidLookup
                        Case enumInvalidLookup.HD_BranchCode, enumInvalidLookup.HD_SubBranchCode
                            Assert.AreEqual(210, oError.Code, "Unexpected Invalid Data error: " & oError.Description)
                        Case Else
                            Assert.AreEqual(102, oError.Code, "Unexpected Invalid Data error: " & oError.Description)
                            Assert.AreEqual(Mid(eInvalidLookup.ToString, 4) & " is invalid", oError.Description, "Unexpected description for error 102: " & oError.Description)
                    End Select
                ElseIf eSTSBusinessError <> enumSTSBusinessError.None Then
                    ' Business Error
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorBusinessRule(oResponse, 0, nBusinessError)
                ElseIf eSTSBusinessError <> enumSTSBusinessError.None Then
                    ' Business Rule tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorBusinessRule(oResponse, 0, nBusinessError)
                Else
                    ' Success Tests
                    SAMTest.AssertCallSucceeded(oResponse)
                    Assert.IsNotNull(oResponse.PartyTimestamp, "Party TimeStamp not returned.")
                End If
            End With

        Catch ex As AssertionException
            Throw
        Catch ex As SoapException
            WSETest.HandleException(ex, nWSETestCaseScenario)
        Catch ex As Exception
            WSETest.HandleException(ex)
        Finally
            oRequest = Nothing
            oResponse = Nothing
        End Try

    End Sub

#End Region

#Region "Success"

    Public Sub SupportMethod(ByRef r_nPartyCnt As Integer)
        UpdatePartyTest(r_nPartyCnt:=r_nPartyCnt)
    End Sub

    <Test()> _
    Public Sub Success_PC()
        UpdatePartyTest(enumPartyType.PartyPC)
    End Sub

    <Test()> _
    Public Sub Success_CC()
        UpdatePartyTest(enumPartyType.PartyCC)
    End Sub

    <Test()> _
    Public Sub Success_Other()
        UpdatePartyTest(enumPartyType.PartyOther)
    End Sub

    <Test()> _
    Public Sub AddConvictionAccident()
        UpdatePartyTest(enumPartyType.PartyOther, , , , , , enumTestCases.AddConvictionAccident)
    End Sub

    <Test()> _
    Public Sub UpdateConvictionAccident()
        UpdatePartyTest(enumPartyType.PartyOther, , , , , , enumTestCases.AddUpdateConvictionAccident)
    End Sub

    <Test()> _
    Public Sub MultiAddWithNoCotacts()
        UpdatePartyTest(enumPartyType.PartyOther, , , , , , enumTestCases.MultipleAddWithNoContacts)
    End Sub

    <Test()> _
    Public Sub AllMandatoryData()
        UpdatePartyTest(enumPartyType.PartyOther, , , , , , enumTestCases.AllMandatoryData)
    End Sub
#End Region

    <Test()> _
  Public Sub InvalidData_Missing_AddressLine1()
        UpdatePartyTest(eMissingData:=enumMissingData.AD_AddressLine1)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PostCode()
        UpdatePartyTest(eMissingData:=enumMissingData.AD_PostCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_CC_BranchCode()
        UpdatePartyTest(eMissingData:=enumMissingData.CC_BranchCode, ePartyType:=enumPartyType.PartyCC)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_CC_CompanyName()
        UpdatePartyTest(eMissingData:=enumMissingData.CC_CompanyName, ePartyType:=enumPartyType.PartyCC)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_ContactItem()
        UpdatePartyTest(eMissingData:=enumMissingData.CN_ContactItem)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        UpdatePartyTest(eMissingData:=enumMissingData.HD_BranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PartyKey()
        UpdatePartyTest(eMissingData:=enumMissingData.HD_PartyKey)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PartyTimeStamp()
        UpdatePartyTest(eMissingData:=enumMissingData.HD_PartyTimeStamp)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_BranchCode()
        UpdatePartyTest(eMissingData:=enumMissingData.PC_BranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_DateOfBirth()
        UpdatePartyTest(eMissingData:=enumMissingData.PC_DateOfBirth)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_Forename()
        UpdatePartyTest(eMissingData:=enumMissingData.PC_Forename)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_GenderCode()
        UpdatePartyTest(eMissingData:=enumMissingData.PC_GenderCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_Surname()
        UpdatePartyTest(eMissingData:=enumMissingData.PC_Surname)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_Title()
        UpdatePartyTest(eMissingData:=enumMissingData.PC_Title)
    End Sub


#Region "Missing Data"
    ' 
    <Test()> _
    Public Sub MissingPartyKey()
        UpdatePartyTest(eMissingData:=enumMissingData.CC_BranchCode, ePartyType:=enumPartyType.PartyCC)
    End Sub

    <Test()> _
    Public Sub MissingConvictionId()
        UpdatePartyTest(eMissingData:=enumMissingData.CC_CompanyName, ePartyType:=enumPartyType.PartyCC)
    End Sub

    <Test()> _
    Public Sub MissingAccidentId()
        UpdatePartyTest(eMissingData:=enumMissingData.CN_ContactItem)
    End Sub



#End Region

#Region "Invalid Lookup value"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        UpdatePartyTest(eInvalidLookup:=enumInvalidLookup.HD_BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_SubBranchCode()
        UpdatePartyTest(eInvalidLookup:=enumInvalidLookup.HD_SubBranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidPartyKey()
        UpdatePartyTest(eMissingData:=enumMissingData.HD_BranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidConvictionId()
        UpdatePartyTest(eMissingData:=enumMissingData.HD_PartyKey)
    End Sub

    <Test()> _
    Public Sub InvalidAccidentId()
        UpdatePartyTest(eMissingData:=enumMissingData.HD_PartyTimestamp)
    End Sub


#End Region

#Region "Invalid Format"

    ' dateOB

#End Region

#Region "STS Business Rules"

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        UpdatePartyTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        UpdatePartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        UpdatePartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        UpdatePartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
