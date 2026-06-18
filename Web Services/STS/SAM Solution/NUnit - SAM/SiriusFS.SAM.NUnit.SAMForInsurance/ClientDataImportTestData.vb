Option Strict On

Imports System

Friend Class ClientDataImportTestDataXMLStruture

    Public BranchCode As String
    Public AgentKey As Integer
    Public oPartyPC As New ProxyWS.BasePartyPCType
    Public oPartyCC As New ProxyWS.BasePartyCCType
    Public ListOfPolicyVersion As New List(Of ProxyWS.BaseQuoteRiskMsgType)
    Public ListOfAccountDocuments As New List(Of ProxyWS.BasePostDocumentRequestType)

    Public Sub Load(ByVal oClientDataImportNode As XmlNode, ByVal oTestData As TestData)

        Try

            For Each oXMLNode As XmlNode In oClientDataImportNode.ChildNodes

                Select Case oXMLNode.Name

                    Case "BASEPARTY"
                        LoadBasePartyDataWrapper(oXMLNode, oTestData)

                    Case "ACCOUNTDOCUMENTS"
                        ListOfAccountDocuments.Add(LoadAccountdocuments(oXMLNode, oTestData))

                    Case "PARTYCC"
                        LoadPartyCC(oXMLNode, oTestData)

                    Case "PARTYPC"
                        LoadPartyPC(oXMLNode, oTestData)

                    Case "POLICYVERSION"
                        ListOfPolicyVersion.Add(LoadPolicyVersion(oXMLNode, oTestData))

                End Select

            Next

            AgentKey = Integer.Parse(oTestData.CheckAttribute(oClientDataImportNode, "AgentKey"))
            BranchCode = oTestData.CheckAttribute(oClientDataImportNode, "BranchCode")

        Catch ex As Exception
            Throw
        End Try
    End Sub


    Private Sub LoadBasePartyDataWrapper(ByVal oBasePartyNode As XmlNode, ByVal oTestData As TestData)

        Dim oPartyTemp As ProxyWS.BasePartyType

        oPartyTemp = oPartyPC

        LoadBasePartyData(oPartyTemp, oBasePartyNode, oTestData)

        oPartyTemp = oPartyCC

        LoadBasePartyData(oPartyTemp, oBasePartyNode, oTestData)

    End Sub

    Private Sub LoadBasePartyData(ByVal oParty As ProxyWS.BasePartyType, ByVal oBasePartyNode As XmlNode, ByVal oTestData As TestData)

        Try

            Dim ListOfAddressesWithContacts As New List(Of ProxyWS.BaseAddressWithContactsType)
            Dim ListOfContacts As New List(Of ProxyWS.BaseContactType)
            For Each oXMLNode As XmlNode In oBasePartyNode.ChildNodes

                Select Case oXMLNode.Name

                    Case "ADDRESS"
                        ListOfAddressesWithContacts.Add(LoadAddressWithContactsType(oXMLNode, oTestData))

                    Case "CONTACT"
                        ListOfContacts.Add(LoadContact(oXMLNode, oTestData))

                End Select

            Next

            oParty.Addresses = ListOfAddressesWithContacts.ToArray
            oParty.Contacts = ListOfContacts.ToArray

            oParty.AccountExecutive = oTestData.CheckAttribute(oBasePartyNode, "AccountExecutive")
            oParty.BranchCode = oTestData.CheckAttribute(oBasePartyNode, "BranchCode")
            oParty.Currency = oTestData.CheckAttribute(oBasePartyNode, "CurrencyCode")
            oParty.DomiciledForTax = Boolean.Parse(oTestData.CheckAttribute(oBasePartyNode, "DomiciledForTax"))
            oParty.DomiciledForTaxSpecified = True
            oParty.TaxExempt = Boolean.Parse(oTestData.CheckAttribute(oBasePartyNode, "TaxExempt"))
            oParty.TaxExemptSpecified = True
            oParty.TaxNumber = oTestData.CheckAttribute(oBasePartyNode, "TaxNumber")
            oParty.TaxPercentage = Decimal.Parse(oTestData.CheckAttribute(oBasePartyNode, "TaxPercentage"))
            oParty.TaxPercentageSpecified = True
            oParty.TPIntroducer = oTestData.CheckAttribute(oBasePartyNode, "TPIntroducer")
            oParty.TPUserCode = oTestData.CheckAttribute(oBasePartyNode, "TPUserCode")

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function LoadAddressWithContactsType(ByVal oAddressNode As XmlNode, ByVal oTestData As TestData) As ProxyWS.BaseAddressWithContactsType

        Dim ListOfContacts As New List(Of ProxyWS.BaseContactType)
        Dim oAddress As New ProxyWS.BaseAddressWithContactsType

        For Each oXMLNode As XmlNode In oAddressNode.ChildNodes

            If oXMLNode.Name = "CONTACT" Then
                ListOfContacts.Add(LoadContact(oXMLNode, oTestData))
            End If

        Next

        oAddress.AddressLine1 = oTestData.CheckAttribute(oAddressNode, "Line1")
        oAddress.AddressLine2 = oTestData.CheckAttribute(oAddressNode, "Line2")
        oAddress.AddressLine3 = oTestData.CheckAttribute(oAddressNode, "Line3")
        oAddress.AddressLine4 = oTestData.CheckAttribute(oAddressNode, "Line4")

        oAddress.AddressTypeCode = CType(System.Enum.Parse(GetType(ProxyWS.AddressTypeType), oTestData.CheckAttribute(oAddressNode, "TypeEnum")), ProxyWS.AddressTypeType)
        oAddress.CountryCode = oTestData.CheckAttribute(oAddressNode, "CountryCode")
        oAddress.PostCode = oTestData.CheckAttribute(oAddressNode, "PostCode")

        oAddress.Contacts = ListOfContacts.ToArray

        Return oAddress

    End Function


    Private Function LoadAddress(ByVal oAddressNode As XmlNode, ByVal oTestData As TestData) As ProxyWS.BaseAddressType

        Dim oAddress As New ProxyWS.BaseAddressType

        oAddress.AddressLine1 = oTestData.CheckAttribute(oAddressNode, "Line1")
        oAddress.AddressLine2 = oTestData.CheckAttribute(oAddressNode, "Line2")
        oAddress.AddressLine3 = oTestData.CheckAttribute(oAddressNode, "Line3")
        oAddress.AddressLine4 = oTestData.CheckAttribute(oAddressNode, "Line4")
        oAddress.AddressTypeCode = CType(System.Enum.Parse(GetType(ProxyWS.AddressTypeType), oTestData.CheckAttribute(oAddressNode, "TypeEnum")), ProxyWS.AddressTypeType)
        oAddress.CountryCode = oTestData.CheckAttribute(oAddressNode, "CountryCode")
        oAddress.PostCode = oTestData.CheckAttribute(oAddressNode, "PostCode")

        Return oAddress

    End Function

    Private Function LoadContact(ByVal oContactNode As XmlNode, ByVal oTestData As TestData) As ProxyWS.BaseContactType

        Dim oContact As New ProxyWS.BaseContactType
        Dim oContactDetail As New ProxyWS.BaseContactDetailType

        oContact.AreaCode = oTestData.CheckAttribute(oContactNode, "AreaCode")
        oContact.ContactTypeCode = CType(System.Enum.Parse(GetType(ProxyWS.ContactTypeType), oTestData.CheckAttribute(oContactNode, "ContactTypeEnum")), ProxyWS.ContactTypeType)

        oContactDetail.Item = oTestData.CheckAttribute(oContactNode, "ContactDetail")
        oContactDetail.ItemElementName = CType(System.Enum.Parse(GetType(ProxyWS.ItemChoiceType), oTestData.CheckAttribute(oContactNode, "ContactDetailTypeEnum")), ProxyWS.ItemChoiceType)

        oContact.ContactDetail = oContactDetail

        Return oContact

    End Function

    Private Function LoadAccountdocuments(ByVal oAccountDocumentNode As XmlNode, ByVal oTestData As TestData) As ProxyWS.BasePostDocumentRequestType
        Try


            Dim oAccountDocument As New ProxyWS.BasePostDocumentRequestType
            Dim ListOfTransactions As New List(Of ProxyWS.BaseTransactionType)

            For Each oXMLNode As XmlNode In oAccountDocumentNode.ChildNodes
                If oXMLNode.Name = "TRANSACTIONS" Then
                    ListOfTransactions.Add(LoadTransaction(oXMLNode, oTestData))
                End If
            Next

            With oAccountDocument
                .BranchCode = oTestData.CheckAttribute(oAccountDocumentNode, "BranchCode")
                .Comment = oTestData.CheckAttribute(oAccountDocumentNode, "Comment")
                .DocumentType = CType(System.Enum.Parse(GetType(ProxyWS.DocumentTypeType), oTestData.CheckAttribute(oAccountDocumentNode, "DocumentType")), ProxyWS.DocumentTypeType)
                .Transactions = ListOfTransactions.ToArray
            End With

            Return oAccountDocument
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function LoadTransaction(ByVal oTranactionNode As XmlNode, ByVal oTestData As TestData) As ProxyWS.BaseTransactionType
        Try


            Dim oTransaction As New ProxyWS.BaseTransactionType

            oTransaction.AccountCode = oTestData.CheckAttribute(oTranactionNode, "AccountCode")
            oTransaction.Amount = ToSafeDecimal(oTestData.CheckAttribute(oTranactionNode, "Amount"), 0)
            oTransaction.Comment = oTestData.CheckAttribute(oTranactionNode, "Comment")
            oTransaction.UnderwritingYearCode = oTestData.CheckAttribute(oTranactionNode, "UnderwritingYearCode")

            Return oTransaction
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub LoadPartyCC(ByVal oPartyCCNode As XmlNode, ByVal oTestData As TestData)

        Try

            oPartyCC.BusinessCode = oTestData.CheckAttribute(oPartyCCNode, "BusinessCode")
            oPartyCC.CompanyName = oTestData.CheckAttribute(oPartyCCNode, "CompanyName")
            oPartyCC.MainContact = oTestData.CheckAttribute(oPartyCCNode, "MainContact")
            oPartyCC.NumberOfEmployees = Integer.Parse(oTestData.CheckAttribute(oPartyCCNode, "NumberOfEmployees"))
            oPartyCC.NumberOfEmployeesSpecified = True
            oPartyCC.NumberOfOffices = Integer.Parse(oTestData.CheckAttribute(oPartyCCNode, "NumberOfOffices"))
            oPartyCC.NumberOfOfficesSpecified = True

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadPartyPC(ByVal oPartyPCNode As XmlNode, ByVal oTestData As TestData)

        Try
            oPartyPC.AlternativeId = oTestData.CheckAttribute(oPartyPCNode, "AlternativeId")
            oPartyPC.DateOfBirth = Date.Parse(oTestData.CheckAttribute(oPartyPCNode, "DateOfBirth"))
            oPartyPC.DateOfBirthSpecified = True
            oPartyPC.Forename = oTestData.CheckAttribute(oPartyPCNode, "Forename")
            oPartyPC.GenderCode = oTestData.CheckAttribute(oPartyPCNode, "GenderCode")
            oPartyPC.Initials = oTestData.CheckAttribute(oPartyPCNode, "Initials")
            oPartyPC.MaritalStatusCode = CType(System.Enum.Parse(GetType(ProxyWS.MaritalStatusCodeType), oTestData.CheckAttribute(oPartyPCNode, "MaritalStatusCode")), ProxyWS.MaritalStatusCodeType)
            oPartyPC.MaritalStatusCodeSpecified = True
            oPartyPC.OccupationCode = oTestData.CheckAttribute(oPartyPCNode, "OccupationCode")
            oPartyPC.Surname = oTestData.CheckAttribute(oPartyPCNode, "Surname")
            oPartyPC.Title = oTestData.CheckAttribute(oPartyPCNode, "Title")

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function LoadPolicyVersion(ByVal oPolicyVersionNode As XmlNode, ByVal oTestData As TestData) As ProxyWS.BaseQuoteRiskMsgType

        Try

            Dim ListOfRisks As New List(Of ProxyWS.BaseRiskType)
            Dim oPolicyVersion As New ProxyWS.BaseQuoteRiskMsgType
            For Each oXMLNode As XmlNode In oPolicyVersionNode.ChildNodes

                If oXMLNode.Name = "RISK" Then
                    ListOfRisks.Add(LoadRisk(oXMLNode, oTestData))
                ElseIf oXMLNode.Name = "BANKADDRESS" Then
                    oPolicyVersion.BankAddress = LoadAddress(oXMLNode, oTestData)
                End If
            Next

            With oPolicyVersion
                .AgentKey = Integer.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "AgentKey"))
                .AgentKeySpecified = False
                .AmountToFinance = Double.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "AmountToFinance"))
                .AmountToFinanceSpecified = True
                .AnalysisCode = oTestData.CheckAttribute(oPolicyVersionNode, "AnalysisCode")
                .BankAccountName = oTestData.CheckAttribute(oPolicyVersionNode, "BankAccountName")
                .BankAccountNo = oTestData.CheckAttribute(oPolicyVersionNode, "BankAccountNo")
                .BankAreaCode = oTestData.CheckAttribute(oPolicyVersionNode, "BankAreaCode")
                .BankBranch = oTestData.CheckAttribute(oPolicyVersionNode, "BankBranch")
                .BankExtn = oTestData.CheckAttribute(oPolicyVersionNode, "BankExtn")
                .BankFax = oTestData.CheckAttribute(oPolicyVersionNode, "BankFax")
                .BankFaxCode = oTestData.CheckAttribute(oPolicyVersionNode, "BankFaxCode")
                .BankName = oTestData.CheckAttribute(oPolicyVersionNode, "BankName")
                .BankPhone = oTestData.CheckAttribute(oPolicyVersionNode, "BankPhone")
                .BankSortCode = oTestData.CheckAttribute(oPolicyVersionNode, "BankSortCode")
                .BranchCode = oTestData.CheckAttribute(oPolicyVersionNode, "BranchCode")
                .CoverEndDate = Date.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "CoverEndDate"))
                .CoverStartDate = Date.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "CoverStartDate"))
                .CurrencyCode = oTestData.CheckAttribute(oPolicyVersionNode, "CurrencyCode")
                .DayOfWeekOrMonth = Integer.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "DayOfWeekOrMonth"))
                .DayOfWeekOrMonthSpecified = True
                .Description = oTestData.CheckAttribute(oPolicyVersionNode, "Description")
                .FinanceCompanyNo = Integer.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "FinanceCompanyNo"))
                .FinanceCompanyNoSpecified = True
                .FinanceEndDate = Date.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "FinanceEndDate"))
                .FinanceEndDateSpecified = True
                .FinancePreferredDate = Date.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "FinancePreferredDate"))
                .FinancePreferredDateSpecified = True
                .FinanceQuoteDate = Date.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "FinanceQuoteDate"))
                .FinanceQuoteDateSpecified = True
                .FinanceSchemeNo = Integer.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "FinanceSchemeNo"))
                .FinanceSchemeNoSpecified = True
                .FinanceSchemeVersion = Integer.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "FinanceSchemeVersion"))
                .FinanceSchemeVersionSpecified = True
                .FinanceStartDate = Date.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "FinanceStartDate"))
                .FinanceStartDateSpecified = True
                .InsuredName = oTestData.CheckAttribute(oPolicyVersionNode, "InsuredName")
                .PaymentProtection = Boolean.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "PaymentProtection"))
                .PaymentProtectionSpecified = True
                .PolicyStatusCode = oTestData.CheckAttribute(oPolicyVersionNode, "PolicyStatusCode")
                .PolicyVersion = Integer.Parse(oTestData.CheckAttribute(oPolicyVersionNode, "PolicyVersion"))
                .ProductCode = oTestData.CheckAttribute(oPolicyVersionNode, "ProductCode")
                .QuoteRef = Guid.NewGuid.ToString 'oTestData.CheckAttribute(oPolicyVersionNode, "QuoteRef")
                ' .Risks = ListOfRisks.ToArray

            End With

            Return oPolicyVersion

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function LoadRisk(ByVal oRiskNode As XmlNode, ByVal oTestData As TestData) As ProxyWS.BaseRiskType

        Try
            Dim oRisk As New ProxyWS.BaseRiskType

            oRisk.BranchCode = oTestData.CheckAttribute(oRiskNode, "BranchCode")
            oRisk.DataModelCode = oTestData.CheckAttribute(oRiskNode, "DataModelCode")
            oRisk.RiskDescription = oTestData.CheckAttribute(oRiskNode, "RiskDescription")
            oRisk.RiskTypeCode = oTestData.CheckAttribute(oRiskNode, "RiskTypeCode")
            oRisk.RunDefaultRules = False
            oRisk.ScreenCode = oTestData.CheckAttribute(oRiskNode, "ScreenCode")

            Dim sXMLDoc As String = My.Computer.FileSystem.ReadAllText(My.Settings.Item("TestXMLDataSetFileName").ToString)
            Dim oXML As New System.Xml.XmlDocument
            oXML.LoadXml(sXMLDoc)
            oRisk.XMLDataSet = oXML.InnerXml

            Return oRisk
        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function ToSafeInteger(ByVal sValue As String, ByVal iDefault As Integer) As Integer
        Dim iValue As Integer
        If Integer.TryParse(sValue, iValue) Then
            Return iValue
        Else
            Return iDefault
        End If
    End Function

    Private Function ToSafeString(ByVal sValue As String, ByVal sDefault As String) As String
        If Not String.IsNullOrEmpty(sValue) Then
            Return sValue
        Else
            Return sDefault
        End If
    End Function

    Private Function ToSafeDouble(ByVal sValue As String, ByVal dDefault As Double) As Double
        Dim dValue As Double
        If Double.TryParse(sValue, dValue) Then
            Return dValue
        Else
            Return dDefault
        End If
    End Function

    Private Function ToSafeDecimal(ByVal sValue As String, ByVal dDefault As Decimal) As Decimal
        Dim dValue As Decimal
        If Decimal.TryParse(sValue, dValue) Then
            Return dValue
        Else
            Return dDefault
        End If
    End Function








End Class
