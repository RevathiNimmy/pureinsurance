Imports NUnit.Framework

<TestFixture()> _
Public Class AddParty
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nPartyCnt As Integer
    Private m_btPartyTimeStamp() As Byte

    Private m_oTestData As New TestData

    Private Enum enumPartyType
        PartyPC
        PartyCC
        PartyOther
    End Enum

    Private Enum enumMissingData
        None
        'HD_AgentKey
        HD_BranchCode
        PC_BranchCode
        PC_Surname
        PC_Forename
        PC_Title
        PC_Initials
        CC_BranchCode
        CC_CompanyName
        AD_Address
        AD_AddressLine1
        AD_CountryCode
        AD_PostCode
        CN_ContactItem
    End Enum

    Private Enum enumInvalidLookup
        HD_BranchCode
        HD_SubBranchCode
        PC_GenderCode
        PC_Title
        PC_OccupationCode
        PC_EmployersBusinessCode
        CC_BusinessCode
        None
    End Enum

    Private Enum enumSTSBusinessError
        None
        BOFailed
        NoCorrespondenceAddress
        DOBInFuture
        DOBTooOld
        'InvalidAgentKey
    End Enum

    Private Enum enumInvalidFormat
        None
        DateOfBirth
    End Enum

#End Region

#Region "Private Methods"

    Private Sub AddPartyTest( _
      Optional ByVal ePartyType As enumPartyType = enumPartyType.PartyPC, _
      Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
      Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
      Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
      Optional ByVal eInvalidFormat As enumInvalidFormat = enumInvalidFormat.None, _
      Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.AddPartyRequestType
        Dim oResponse As ProxyWS.AddPartyResponseType
        Dim oParty As ProxyWS.BasePartyType = Nothing
        Dim nBusinessError As Integer = 0

        ' Set STS error code
        Select Case eSTSBusinessError
            Case enumSTSBusinessError.NoCorrespondenceAddress
                nBusinessError = 228
            Case enumSTSBusinessError.BOFailed
                nBusinessError = 252
            Case enumSTSBusinessError.DOBInFuture
                nBusinessError = 213
            Case enumSTSBusinessError.DOBTooOld
                nBusinessError = 214
        End Select

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
                If eMissingData <> enumMissingData.PC_Forename Then
                    If eSTSBusinessError = enumSTSBusinessError.BOFailed Then
                        ' Pass a string that is too long.
                        ' However, This doesn't cause an error as it is truncated
                        ' TODO do something else to cause a BO error
                        oPartyPC.Forename = New String("x", 100)
                    Else
                        oPartyPC.Forename = m_oTestData.Forename
                    End If
                End If
                If eMissingData <> enumMissingData.PC_Surname Then
                    oPartyPC.Surname = m_oTestData.Surname
                End If
                If eSTSBusinessError = enumSTSBusinessError.DOBInFuture Then
                    oPartyPC.DateOfBirth = Today.AddDays(1)
                ElseIf eSTSBusinessError = enumSTSBusinessError.DOBTooOld Then
                    oPartyPC.DateOfBirth = Today.AddYears(-121)
                Else
                    oPartyPC.DateOfBirth = m_oTestData.DateOfBirth
                End If
                oPartyPC.DateOfBirthSpecified = (oPartyPC.DateOfBirth <> New Date)

                If eMissingData <> enumMissingData.PC_Initials Then
                    oPartyPC.Initials = m_oTestData.Initials
                End If

                If eMissingData <> enumMissingData.PC_Title Then
                    If eInvalidLookup = enumInvalidLookup.PC_Title Then
                        oPartyPC.Title = m_oTestData.InvalidLookupCode
                    Else
                        oPartyPC.Title = m_oTestData.Title
                    End If
                End If
                oPartyPC.TPIntroducer = m_oTestData.TPIntroducer
                oPartyPC.TPUserCode = m_oTestData.TPUser
                oPartyPC.MaritalStatusCode = m_oTestData.MaritalStatusCode
                oPartyPC.MaritalStatusCodeSpecified = True
                oPartyPC.EmploymentStatusCode = m_oTestData.EmploymentStatusCode
                oPartyPC.EmploymentStatusCodeSpecified = True
                oPartyPC.AlternativeId = m_oTestData.AlternativeId
                If eInvalidLookup = enumInvalidLookup.PC_EmployersBusinessCode Then
                    oPartyPC.EmployersBusinessCode = m_oTestData.InvalidLookupCode
                Else
                    oPartyPC.EmployersBusinessCode = m_oTestData.EmployerBusinessCode
                End If
                If eInvalidLookup = enumInvalidLookup.PC_OccupationCode Then
                    oPartyPC.OccupationCode = m_oTestData.InvalidLookupCode
                Else
                    oPartyPC.OccupationCode = m_oTestData.OccupationCode
                End If
                If eInvalidLookup = enumInvalidLookup.PC_GenderCode Then
                    oPartyPC.GenderCode = m_oTestData.InvalidLookupCode
                Else
                    oPartyPC.GenderCode = m_oTestData.GenderCode
                End If
                oParty = oPartyPC
            ElseIf ePartyType = enumPartyType.PartyCC Then
                Dim oPartyCC As New ProxyWS.BasePartyCCType
                If eMissingData <> enumMissingData.CC_BranchCode Then oPartyCC.BranchCode = m_oTestData.BranchCode
                If eMissingData <> enumMissingData.CC_CompanyName Then oPartyCC.CompanyName = m_oTestData.CompanyName
                If eInvalidLookup = enumInvalidLookup.CC_BusinessCode Then
                    oPartyCC.BusinessCode = m_oTestData.InvalidLookupCode
                Else
                    oPartyCC.BusinessCode = m_oTestData.BusinessCode
                End If
                oPartyCC.TPIntroducer = m_oTestData.TPIntroducer
                oPartyCC.TPUserCode = m_oTestData.TPUser
                oParty = oPartyCC
            ElseIf ePartyType = enumPartyType.PartyOther Then

                Dim oPartyOther As New ProxyWS.BasePartyOTHERType

                Dim oConviction As ProxyWS.BasePartyOTHERTypeConviction
                If Not m_oTestData.Convictions Is Nothing Then
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
                End If
                Dim oAccident As ProxyWS.BasePartyOTHERTypeAccident
                If Not m_oTestData.Accidents Is Nothing Then
                    For cnt As Integer = m_oTestData.Accidents.GetLowerBound(0) To m_oTestData.Accidents.GetUpperBound(0)
                        ReDim Preserve oPartyOther.Accident(cnt)
                        oAccident = New ProxyWS.BasePartyOTHERTypeAccident
                        oAccident.Date = m_oTestData.Accidents(cnt).Date
                        oAccident.Description = m_oTestData.Accidents(cnt).Description
                        oAccident.IsAtFault = m_oTestData.Accidents(cnt).IsAtFault
                        oPartyOther.Accident(cnt) = oAccident
                    Next
                End If
                Dim oSuppBusiness As ProxyWS.BasePartyOTHERTypeSupplierBusiness
                If Not m_oTestData.SuppBusiness Is Nothing Then
                    For cnt As Integer = m_oTestData.SuppBusiness.GetLowerBound(0) To m_oTestData.SuppBusiness.GetUpperBound(0)
                        ReDim Preserve oPartyOther.SupplierBusiness(cnt)
                        oSuppBusiness = New ProxyWS.BasePartyOTHERTypeSupplierBusiness
                        oSuppBusiness.BusinessCode = m_oTestData.SuppBusiness(cnt).BusinessCode
                        oSuppBusiness.SpecialityCode = m_oTestData.SuppBusiness(cnt).SpecialityCode
                        oPartyOther.SupplierBusiness(cnt) = oSuppBusiness
                    Next
                End If
                'Dim oOtherPartyInfo As New ProxyWS.BasePartyOTHERType

                oPartyOther.BranchCode = m_oTestData.BranchCode
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
                End If

                If eMissingData <> enumMissingData.AD_Address Then
                Dim oAddress As ProxyWS.BaseAddressWithContactsType
                If Not m_oTestData.Addresses Is Nothing Then
                    ReDim oParty.Addresses(m_oTestData.Addresses.GetUpperBound(0))
                    For iCnt As Integer = m_oTestData.Addresses.GetLowerBound(0) To m_oTestData.Addresses.GetUpperBound(0)
                        oAddress = New ProxyWS.BaseAddressWithContactsType
                        oAddress.AddressTypeCode = m_oTestData.Addresses(iCnt).TypeCode
                        If eSTSBusinessError = enumSTSBusinessError.NoCorrespondenceAddress And _
                           oAddress.AddressTypeCode = ProxyWS.AddressTypeType.Item3131XCO Then
                            oAddress.AddressTypeCode = ProxyWS.AddressTypeType.Item3131001
                        End If
                        If eMissingData <> enumMissingData.AD_AddressLine1 Then oAddress.AddressLine1 = m_oTestData.Addresses(iCnt).Line1
                        oAddress.AddressLine2 = m_oTestData.Addresses(iCnt).Line2
                        oAddress.AddressLine3 = m_oTestData.Addresses(iCnt).Line3
                        oAddress.AddressLine4 = m_oTestData.Addresses(iCnt).Line4
                        If eMissingData <> enumMissingData.AD_PostCode Then oAddress.PostCode = m_oTestData.Addresses(iCnt).PostCode
                        If eMissingData <> enumMissingData.AD_CountryCode Then oAddress.CountryCode = m_oTestData.Addresses(iCnt).CountryCode
                        If (m_oTestData.Addresses(iCnt).Contact) IsNot Nothing Then
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
                        End If
                        oParty.Addresses(iCnt) = oAddress
                    Next
                End If
            End If

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

            With oRequest
                If eMissingData <> enumMissingData.HD_BranchCode Then
                    If eInvalidLookup = enumInvalidLookup.HD_BranchCode Then
                        .BranchCode = m_oTestData.InvalidLookupCode
                    Else
                        .BranchCode = m_oTestData.BranchCode
                    End If
                End If
                If eInvalidLookup = enumInvalidLookup.HD_SubBranchCode Then
                    .SubBranchCode = m_oTestData.InvalidLookupCode
                Else
                    .SubBranchCode = m_oTestData.SubBranch
                End If
                .Item = oParty
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.AddParty(oRequest)

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
                            Assert.AreEqual(210, oError.Code, "Unexpected Invalid Data error")
                        Case Else
                            Assert.AreEqual(102, oError.Code, "Unexpected Invalid Data error")
                            Assert.AreEqual(Mid(eInvalidLookup.ToString, 4) & " is invalid", oError.Description, "Unexpected description")
                    End Select
                ElseIf eInvalidFormat <> enumInvalidFormat.None Then
                    ' Invalid Format tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorInvalidData(oResponse, 0, 101, eInvalidFormat.ToString & " is not a valid date")
                ElseIf eSTSBusinessError <> enumSTSBusinessError.None Then
                    ' Business Error
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorBusinessRule(oResponse, 0, nBusinessError)
                Else
                    ' Success Tests
                    SAMTest.AssertCallSucceeded(oResponse)
                    Console.WriteLine("AddParty.Success: Party Count = " & CStr(.PartyKey))
                    Assert.Greater(.PartyKey, 0, "Returned Party Count invalid.")
                    m_nPartyCnt = .PartyKey
                    m_btPartyTimeStamp = .PartyTimestamp
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

    Public Sub SupportMethod(ByRef r_nPartyCnt As Integer, ByRef r_btPartyTimeStamp() As Byte)

        AddPartyTest()

        r_nPartyCnt = m_nPartyCnt
        r_btPartyTimeStamp = m_btPartyTimeStamp

    End Sub

    Public Sub SupportMethod(ByRef r_nPartyCnt As Integer, ByRef r_btPartyTimeStamp() As Byte, ByVal PartyType As Integer)

        AddPartyTest(PartyType)

        r_nPartyCnt = m_nPartyCnt
        r_btPartyTimeStamp = m_btPartyTimeStamp

    End Sub

    <Test()> _
    Public Sub Success_PC()
        AddPartyTest(enumPartyType.PartyPC)
    End Sub

    <Test()> _
        Public Sub Success_Other()
        AddPartyTest(enumPartyType.PartyOther)
    End Sub

    <Test()> _
    Public Sub Success_CC()
        AddPartyTest(enumPartyType.PartyCC)
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub InvalidData_Missing_Address()
        AddPartyTest(eMissingData:=enumMissingData.AD_Address)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_AddressLine1()
        AddPartyTest(eMissingData:=enumMissingData.AD_AddressLine1)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_CountryCode()
        AddPartyTest(eMissingData:=enumMissingData.AD_CountryCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_CC_BranchCode()
        AddPartyTest(eMissingData:=enumMissingData.CC_BranchCode, ePartyType:=enumPartyType.PartyCC)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_CC_CompanyName()
        AddPartyTest(eMissingData:=enumMissingData.CC_CompanyName, ePartyType:=enumPartyType.PartyCC)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_ContactItem()
        AddPartyTest(eMissingData:=enumMissingData.CN_ContactItem)
    End Sub

    '<Test()> _
    'Public Sub InvalidData_Missing_BranchCode()
    '    AddPartyTest(eMissingData:=enumMissingData.HD_BranchCode)
    'End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_BranchCode()
        AddPartyTest(eMissingData:=enumMissingData.PC_BranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_Forename()
        AddPartyTest(eMissingData:=enumMissingData.PC_Forename)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_Initials()
        AddPartyTest(eMissingData:=enumMissingData.PC_Initials)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_Surname()
        AddPartyTest(eMissingData:=enumMissingData.PC_Surname)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PC_Title()
        AddPartyTest(eMissingData:=enumMissingData.PC_Title)
    End Sub

#End Region

#Region "Invalid Lookup value"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        AddPartyTest(eInvalidLookup:=enumInvalidLookup.HD_BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_SubBranchCode()
        AddPartyTest(eInvalidLookup:=enumInvalidLookup.HD_SubBranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_PC_GenderCode()
        AddPartyTest(eInvalidLookup:=enumInvalidLookup.PC_GenderCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_PC_Title()
        AddPartyTest(eInvalidLookup:=enumInvalidLookup.PC_Title)
    End Sub
    <Test()> _
    Public Sub InvalidData_PC_OccupationCode()
        AddPartyTest(eInvalidLookup:=enumInvalidLookup.PC_OccupationCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_PC_EmployersBusinessCode()
        AddPartyTest(eInvalidLookup:=enumInvalidLookup.PC_EmployersBusinessCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_CC_BusinessCode()
        AddPartyTest(eInvalidLookup:=enumInvalidLookup.CC_BusinessCode, ePartyType:=enumPartyType.PartyCC)
    End Sub

#End Region

#Region "Invalid Format"

    ' Can't test this from here as we are using structures directly and data type
    ' is Date, so won't allow to set as invalid
    '<Test()> _
    'Public Sub InvalidFormat_DOB()
    '    AddPartyTest(eInvalidFormat:=enumInvalidFormat.DateOfBirth)
    'End Sub

#End Region

#Region "STS Business Rule"

    <Test()> _
    Public Sub STSBusiness_NoCorrespondenceAddr()
        AddPartyTest(eSTSBusinessError:=enumSTSBusinessError.NoCorrespondenceAddress)
    End Sub
    <Test()> _
    Public Sub STSBusiness_DOBInFuture()
        AddPartyTest(eSTSBusinessError:=enumSTSBusinessError.DOBInFuture)
    End Sub
    <Test()> _
    Public Sub STSBusiness_DOBTooOld()
        AddPartyTest(eSTSBusinessError:=enumSTSBusinessError.DOBTooOld)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        AddPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        AddPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        AddPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        AddPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
