Imports Nexus.Utils
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports NexusProvider.SAMForInsurance
Imports Nexus.Library
Imports CMS.Library.Portal

Imports System
Imports System.Globalization
Imports System.Threading


Namespace Nexus
    Partial Class Modal_ClientDetailsForClaims : Inherits CMS.Library.Frontend.clsCMSPage


        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(txtTelephoneNumberHome)

            If Not IsPostBack Then
                Dim oAddress As New NexusProvider.Address
                Dim oAddressCollection As New NexusProvider.AddressCollection
                FillUserDefinedCombo()
                Select Case CType(Session(CNMode), Mode)
                    Case Mode.NewClaim
                        Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        With oOpenClaim
                            If Not IsNothing(.Client.Contact) Then
                                If .Client.Contact.Count > 0 AndAlso Request.UrlReferrer.AbsolutePath.ToUpper().Contains("OVERVIEW.ASPX") Then
                                    For Each ocontact As NexusProvider.Contact In .Client.Contact
                                        Select Case ocontact.ContactType
                                            Case NexusProvider.ContactType.Email
                                                txtEmailNumber.Text = ocontact.Number
                                            Case NexusProvider.ContactType.Fax
                                                txtFaxNumber.Text = ocontact.Number
                                            Case NexusProvider.ContactType.HomePhone
                                                txtTelephoneNumberHome.Text = ocontact.Number
                                            Case NexusProvider.ContactType.Main, NexusProvider.ContactType.WorkPhone
                                                txtTelephoneNumberoffice.Text = ocontact.Number
                                            Case NexusProvider.ContactType.Mobile
                                                txtMobileNumber.Text = ocontact.Number
                                        End Select
                                    Next
                                Else
                                    txtEmailNumber.Text = .ClientEmail
                                    txtFaxNumber.Text = .ClientFaxNo
                                    txtTelephoneNumberHome.Text = .ClientTelNo
                                    txtTelephoneNumberoffice.Text = .ClientTelNoOff
                                    txtMobileNumber.Text = .ClientMobileNo
                                End If
                            End If
                            txtClientName.Text = Session(CNInsurer_Header)
                            txtClientClaimNumber.Text = .Client.PartyClaimNumber

                            oAddress.Address1 = .Client.Address.Address1
                            oAddress.Address2 = .Client.Address.Address2
                            oAddress.Address3 = .Client.Address.Address3
                            oAddress.Address4 = .Client.Address.Address4
                            oAddress.PostCode = .Client.Address.PostCode
                            oAddress.CountryCode = .Client.Address.CountryCode
                            oAddress = .Client.Address
                            oAddress.AddressType = NexusProvider.AddressType.CorrespondenceAddress
                            oAddressCollection.Add(oAddress)
                            Session(CNAAAddresses) = oAddressCollection
                            DisplayAddress(oAddressCollection)
                            If HttpContext.Current.Session.IsCookieless Then
                                lnkClientAddress.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Address.aspx?modal=true&KeepThis=true&FromPage=AA&Page=Client&AddressID=0&TB_iframe=true&height=300&width=600' , null);return false;"
                            Else
                                lnkClientAddress.OnClientClick = "tb_show(null , '../Modal/Address.aspx?modal=true&KeepThis=true&FromPage=AA&Page=Client&AddressID=0&TB_iframe=true&height=300&width=600' , null);return false;"
                            End If

                            If .Client.TaxRegistrationNumber IsNot Nothing Then
                                If .Client.TaxRegistrationNumber.Trim.Length <> 0 Then
                                    chkVATRegistered.Checked = True
                                    txtVATRegistrationNumber.Text = .Client.TaxRegistrationNumber
                                    txtVATRegistrationNumber.Enabled = True
                                End If
                            End If


                            'If Value is available
                            If .UserDefFldACode IsNot Nothing Then
                                If .UserDefFldACode.Trim.Length <> 0 Then
                                    ddlExcessListA.SelectedValue = .UserDefFldACode.Trim
                                End If
                            End If
                            If .UserDefFldBCode IsNot Nothing Then
                                If .UserDefFldBCode.Trim.Length <> 0 Then
                                    ddlExcessListB.SelectedValue = .UserDefFldBCode.Trim
                                End If
                            End If
                            If .UserDefFldCCode IsNot Nothing Then
                                If .UserDefFldCCode.Trim.Length <> 0 Then
                                    ddlExcessListC.SelectedValue = .UserDefFldCCode.Trim
                                End If
                            End If
                            If .UserDefFldDCode IsNot Nothing Then
                                If .UserDefFldDCode.Trim.Length <> 0 Then
                                    ddlNCBPercentage.SelectedValue = .UserDefFldDCode.Trim
                                End If
                                If .UserDefFldECode IsNot Nothing Then
                                    If .UserDefFldECode.Trim.Length <> 0 Then
                                        ddlCoverType.SelectedValue = .UserDefFldECode.Trim
                                    End If
                                End If
                            End If
                        End With
                    Case Mode.EditClaim, Mode.ViewClaim, Mode.PayClaim, Mode.Review, Mode.SalvageClaim, Mode.TPRecovery
                        If Not Session(CNClaim) Is Nothing Then
                            Dim oClaimDetails As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                            With oClaimDetails
                                txtClientName.Text = Session(CNInsurer_Header)
                                txtClientClaimNumber.Text = .Client.PartyClaimNumber
                                If .ClaimStatus.ToUpper() = "CLOSED" OrElse .ClaimStatus.ToUpper() = "RECLOSED" _
                                    OrElse CType(Session(CNMode), Mode) = Mode.ViewClaim Then
                                    txtEmailNumber.Text = .ClientEmail
                                    txtFaxNumber.Text = .ClientFaxNo
                                    txtTelephoneNumberHome.Text = .ClientTelNo
                                    txtTelephoneNumberoffice.Text = .ClientTelNoOff
                                    txtMobileNumber.Text = .ClientMobileNo
                                Else
                                    If Not IsNothing(.Client.Contact) Then
                                        If .Client.Contact.Count > 0 Then
                                            For Each ocontact As NexusProvider.Contact In .Client.Contact
                                                Select Case ocontact.ContactType
                                                    Case NexusProvider.ContactType.Email
                                                        txtEmailNumber.Text = ocontact.Number
                                                    Case NexusProvider.ContactType.Fax
                                                        txtFaxNumber.Text = ocontact.Number
                                                    Case NexusProvider.ContactType.HomePhone
                                                        txtTelephoneNumberHome.Text = ocontact.Number
                                                    Case NexusProvider.ContactType.Main, NexusProvider.ContactType.WorkPhone
                                                        txtTelephoneNumberoffice.Text = ocontact.Number
                                                    Case NexusProvider.ContactType.Mobile
                                                        txtMobileNumber.Text = ocontact.Number
                                                End Select
                                            Next
                                        Else
                                            txtEmailNumber.Text = .Client.ClientEmail
                                            txtFaxNumber.Text = .Client.ClientFaxNo
                                            txtTelephoneNumberHome.Text = .Client.ClientTelNo
                                            txtTelephoneNumberoffice.Text = .Client.ClientTelNoOff
                                            txtMobileNumber.Text = .Client.ClientMobileNo
                                        End If

                                    Else
                                        txtEmailNumber.Text = .Client.ClientEmail
                                        txtFaxNumber.Text = .Client.ClientFaxNo
                                        txtTelephoneNumberHome.Text = .Client.ClientTelNo
                                        txtTelephoneNumberoffice.Text = .Client.ClientTelNoOff
                                        txtMobileNumber.Text = .Client.ClientMobileNo
                                    End If
                                End If

                                txtClientAddress.Text = .Client.Address.Address1.Trim()
                                If .Client.TaxRegistrationNumber IsNot Nothing Then
                                    If .Client.TaxRegistrationNumber.Trim.Length <> 0 Then
                                        chkVATRegistered.Checked = True
                                        txtVATRegistrationNumber.Text = .Client.TaxRegistrationNumber
                                        txtVATRegistrationNumber.Enabled = True
                                    End If
                                End If
                                oAddress.Address1 = .Client.Address.Address1
                                oAddress.Address2 = .Client.Address.Address2
                                oAddress.Address3 = .Client.Address.Address3
                                oAddress.Address4 = .Client.Address.Address4
                                oAddress.AddressType = .Client.Address.AddressType
                                oAddress.CountryCode = .Client.Address.CountryCode
                                oAddress.CountryDescription = .Client.Address.CountryDescription
                                oAddress.PostCode = .Client.Address.PostCode
                                oAddressCollection.Add(oAddress)
                                Session(CNAAAddresses) = oAddressCollection
                                DisplayAddress(oAddressCollection)

                                If HttpContext.Current.Session.IsCookieless Then
                                    lnkClientAddress.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Address.aspx?modal=true&KeepThis=true&FromPage=AA&Page=Client&Claim&AddressID=0&TB_iframe=true&height=300&width=400' , null);return false;"
                                Else
                                    lnkClientAddress.OnClientClick = "tb_show(null , '../Modal/Address.aspx?modal=true&KeepThis=true&FromPage=AA&Page=Client&Claim&AddressID=0&TB_iframe=true&height=300&width=400' , null);return false;"
                                End If
                                If .Client.TaxRegistrationNumber IsNot Nothing Then
                                    If .Client.TaxRegistrationNumber.Trim.Length <> 0 Then
                                        chkVATRegistered.Checked = True
                                        txtVATRegistrationNumber.Text = .Client.TaxRegistrationNumber
                                        txtVATRegistrationNumber.Enabled = True
                                    End If
                                End If

                                'If Value is available
                                If .UserDefFldACode IsNot Nothing Then
                                    If .UserDefFldACode.Trim.Length <> 0 Then
                                        ddlExcessListA.SelectedValue = .UserDefFldACode.Trim
                                    End If
                                End If
                                If .UserDefFldBCode IsNot Nothing Then
                                    If .UserDefFldBCode.Trim.Length <> 0 Then
                                        ddlExcessListB.SelectedValue = .UserDefFldBCode.Trim
                                    End If
                                End If
                                If .UserDefFldCCode IsNot Nothing Then
                                    If .UserDefFldCCode.Trim.Length <> 0 Then
                                        ddlExcessListC.SelectedValue = .UserDefFldCCode.Trim
                                    End If
                                End If
                                If .UserDefFldDCode IsNot Nothing Then
                                    If .UserDefFldDCode.Trim.Length <> 0 Then
                                        ddlNCBPercentage.SelectedValue = .UserDefFldDCode.Trim
                                    End If
                                End If
                                If .UserDefFldECode IsNot Nothing Then
                                    If .UserDefFldECode.Trim.Length <> 0 Then
                                        ddlCoverType.SelectedValue = .UserDefFldECode.Trim
                                    End If
                                End If
                            End With
                            If CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.PayClaim Then
                                DisableControls(GetMasterPlaceHolder(Page, "cntMainBody"))
                                lnkClientAddress.Visible = False
                            End If
                        End If
                End Select
                If Request.QueryString("InsuranceFileAssociateKey") IsNot Nothing Then
                    BindPolicyAssociateData()
                End If
                If Request.UrlReferrer IsNot Nothing AndAlso (Not Request.UrlReferrer.AbsolutePath.ToUpper().Contains("OVERVIEW.ASPX") Or Session(CNMode) = Mode.ViewClaim _
                Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.PayClaim _
                Or Session(CNMode) = Mode.TPRecovery) Then
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    DisableControls(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName))
                    lnkClientAddress.Enabled = False
                Else
                    lnkClientAddress.Enabled = True
                End If
            End If

            If Request("__EVENTARGUMENT") = "UpdateAddress" Then
                Dim sAddressData() As String = txtAddressData.Value.Split(";")
                Dim oUpdateAddress As NexusProvider.Address
                Dim oClientDetails As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

                If sAddressData(0).ToUpper = "UPDATE" Then

                    If oClientDetails.Client IsNot Nothing Then
                        oUpdateAddress = oClientDetails.Client.Address
                    End If

                    Dim sAddress As String = sAddressData(1).ToUpper()

                    Select Case sAddress
                        Case "3131 XBI"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.BillingAddress
                            Exit Select
                        Case "3131 XBA"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.BranchAddress
                            Exit Select
                        Case "3131 002"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.BusinessAddress
                            Exit Select
                        Case "3131 XCO"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.CorrespondenceAddress
                            Exit Select
                        Case "3131 ECK"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.EmailAddress
                            Exit Select
                        Case "3131 001"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.HomeAddress
                            Exit Select
                        Case "3131 0X9"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.OtherAddress
                            Exit Select
                        Case "3131 XPR"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.PreviousAddress
                            Exit Select
                        Case "3131 XRE"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.RegisteredAddress
                            Exit Select
                        Case "3131 XSA"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.SiteAddress
                            Exit Select
                        Case "3131 0XR"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.SubAgent
                            Exit Select
                    End Select

                    With oUpdateAddress
                        .Address1 = sAddressData(2)
                        .Address2 = sAddressData(3)
                        .Address3 = sAddressData(4)
                        .Address4 = sAddressData(5)
                        .StateCode = sAddressData(6)
                        .PostCode = sAddressData(7)
                        'Call SAM to get the Code by passing the Value for binding it in Address object
                        .CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, sAddressData(8), "COUNTRY", True)
                        .CountryDescription = sAddressData(9)
                    End With

                    'Update the session with updated data
                    Dim ClaimAddresses As NexusProvider.AddressCollection = CType(Session.Item(CNAAAddresses), NexusProvider.AddressCollection)
                    ClaimAddresses(0).Address1 = oUpdateAddress.Address1
                    ClaimAddresses(0).Address2 = oUpdateAddress.Address2
                    ClaimAddresses(0).Address3 = oUpdateAddress.Address3
                    ClaimAddresses(0).Address4 = oUpdateAddress.Address4
                    ClaimAddresses(0).AddressType = oUpdateAddress.AddressType
                    ClaimAddresses(0).CountryCode = oUpdateAddress.CountryCode
                    ClaimAddresses(0).CountryDescription = oUpdateAddress.CountryDescription
                    ClaimAddresses(0).CountryKey = oUpdateAddress.CountryKey
                    ClaimAddresses(0).Key = oUpdateAddress.Key
                    ClaimAddresses(0).PostCode = oUpdateAddress.PostCode
                    ClaimAddresses(0).StateCode = oUpdateAddress.StateCode
                    Session(CNAAAddresses) = ClaimAddresses
                End If
                Session(CNClaim) = oClientDetails
                BindAddressData()
            End If

        End Sub
        ''' <summary>
        ''' This procedure will display Policy associates data
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindPolicyAssociateData()
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())

            If oPortal.EnableMasterClientAssociate = True Then
                'lblTitle.Text = GetLocalResourceObject("lbl_Associate")
                Dim nInsuranceFileAssociateCnt As Integer = Request.QueryString("InsuranceFileAssociateKey")
                Dim oGetPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oGetPolicyAssociateCollection = oWebService.GetPolicyAssociates(Session(CNClaimQuote).InsuranceFileKey, Session(CNClaimQuote).InsuranceFolderKey, Nothing)
                For Each oAssociate As NexusProvider.PolicyAssociate In oGetPolicyAssociateCollection
                    If oAssociate.InsuranceFileAssociatesKey = nInsuranceFileAssociateCnt Then
                        txtAssociation.Text = GetDescriptionForCode(NexusProvider.ListType.PMLookup, GetCodeForKey(NexusProvider.ListType.PMLookup, oAssociate.AssociationTypeKey.ToString, "Association_Type", True), "Association_Type") 'oGetPolicyAssociateCollection(iCount).AssociationTypeKey
                        txtAssociationDetail.Text = oAssociate.AssociationDetail
                        txtDateAttached.Text = oAssociate.DateAttached
                        If (oAssociate.DateRemoved = Date.MinValue) Then
                            txtDateRemoved.Text = ""
                        Else
                            txtDateRemoved.Text = oAssociate.DateRemoved
                        End If
                        Dim oClaimDetails As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        With oClaimDetails
                            txtClientClaimNumber.Text = .Client.PartyClaimNumber
                            If .Client.TaxRegistrationNumber IsNot Nothing Then
                                If .Client.TaxRegistrationNumber.Trim.Length <> 0 Then
                                    chkVATRegistered.Checked = True
                                    txtVATRegistrationNumber.Text = .Client.TaxRegistrationNumber
                                    txtVATRegistrationNumber.Enabled = True
                                End If
                            End If

                            'If Value is available
                            If .UserDefFldACode IsNot Nothing Then
                                If .UserDefFldACode.Trim.Length <> 0 Then
                                    ddlExcessListA.SelectedValue = .UserDefFldACode.Trim
                                End If
                            End If
                            If .UserDefFldBCode IsNot Nothing Then
                                If .UserDefFldBCode.Trim.Length <> 0 Then
                                    ddlExcessListB.SelectedValue = .UserDefFldBCode.Trim
                                End If
                            End If
                            If .UserDefFldCCode IsNot Nothing Then
                                If .UserDefFldCCode.Trim.Length <> 0 Then
                                    ddlExcessListC.SelectedValue = .UserDefFldCCode.Trim
                                End If
                            End If
                            If .UserDefFldDCode IsNot Nothing Then
                                If .UserDefFldDCode.Trim.Length <> 0 Then
                                    ddlNCBPercentage.SelectedValue = .UserDefFldDCode.Trim
                                End If
                                If .UserDefFldECode IsNot Nothing Then
                                    If .UserDefFldECode.Trim.Length <> 0 Then
                                        ddlCoverType.SelectedValue = .UserDefFldECode.Trim
                                    End If
                                End If
                            End If
                        End With
                        Dim nAssociatePartyCnt As Integer = 0
                        nAssociatePartyCnt = oAssociate.PartyKey
                        Dim oAssociateParty As NexusProvider.BaseParty
                        oAssociateParty = oWebService.GetParty(nAssociatePartyCnt)

                        If Not IsNothing(oAssociateParty) Then
                            Select Case True
                                Case TypeOf oAssociateParty Is NexusProvider.CorporateParty
                                    txtClientName.Text = CType(oAssociateParty, NexusProvider.CorporateParty).TradeCode
                                    Exit Select
                                Case TypeOf oAssociateParty Is NexusProvider.PersonalParty
                                    txtClientName.Text = CType(oAssociateParty, NexusProvider.PersonalParty).Title + " " + CType(oAssociateParty, NexusProvider.PersonalParty).Forename + " " + CType(oAssociateParty, NexusProvider.PersonalParty).Lastname
                                    Exit Select
                            End Select
                        End If
                        For iCnt As Integer = 0 To oAssociateParty.Contacts.Count - 1
                            Select Case oAssociateParty.Contacts(iCnt).ContactType
                                Case NexusProvider.ContactType.Email
                                    txtEmailNumber.Text = oAssociateParty.Contacts(iCnt).Number
                                    Exit Select
                                Case NexusProvider.ContactType.Fax
                                    txtFaxNumber.Text = oAssociateParty.Contacts(iCnt).Number
                                    Exit Select
                                Case NexusProvider.ContactType.HomePhone
                                    txtTelephoneNumberHome.Text = oAssociateParty.Contacts(iCnt).Number
                                    Exit Select
                                Case NexusProvider.ContactType.Mobile
                                    txtMobileNumber.Text = oAssociateParty.Contacts(iCnt).Number
                                    Exit Select
                                Case NexusProvider.ContactType.WorkPhone, NexusProvider.ContactType.Main
                                    txtTelephoneNumberoffice.Text = oAssociateParty.Contacts(iCnt).Number
                                    Exit Select
                            End Select
                        Next
                        Dim Addresses As NexusProvider.AddressCollection = oAssociateParty.Addresses
                        If Not IsNothing(Addresses) Then
                            DisplayAddress(Addresses)
                        End If
                        Exit For
                    End If
                Next
            End If
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If Session.Item(CNClaim) IsNot Nothing Then

                If CType(Session(CNMode), Mode) <> Mode.ViewClaim Then
                    Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                    Dim oContacts As New NexusProvider.ContactCollection
                    Dim oContactEmail As New NexusProvider.Contact
                    Dim oContactFax As New NexusProvider.Contact
                    Dim oContactTelephoneoff As New NexusProvider.Contact
                    Dim oContactMobile As New NexusProvider.Contact
                    Dim oContactTelephoneHome As New NexusProvider.Contact

                    If txtEmailNumber.Text IsNot Nothing Then
                        oContactEmail.ContactType = NexusProvider.ContactType.Email
                        oContactEmail.Number = txtEmailNumber.Text
                        oContacts.Add(oContactEmail)
                    End If
                    If txtFaxNumber.Text IsNot Nothing Then
                        oContactFax.ContactType = NexusProvider.ContactType.Fax
                        oContactFax.Number = txtFaxNumber.Text
                        oContacts.Add(oContactFax)
                    End If
                    If txtMobileNumber.Text IsNot Nothing Then
                        oContactMobile.ContactType = NexusProvider.ContactType.Mobile
                        oContactMobile.Number = txtMobileNumber.Text
                        oContacts.Add(oContactMobile)
                    End If
                    If txtTelephoneNumberoffice.Text IsNot Nothing Then
                        oContactTelephoneoff.ContactType = NexusProvider.ContactType.Main
                        oContactTelephoneoff.Number = txtTelephoneNumberoffice.Text
                        oContacts.Add(oContactTelephoneoff)
                    End If
                    If txtTelephoneNumberHome.Text IsNot Nothing Then
                        oContactTelephoneHome.ContactType = NexusProvider.ContactType.HomePhone
                        oContactTelephoneHome.Number = txtTelephoneNumberHome.Text
                        oContacts.Add(oContactTelephoneHome)
                    End If

                    With oOpenClaim
                        .Client.Contact = oContacts
                        .Client.PartyClaimNumber = txtClientClaimNumber.Text
                        .ClientEmail = txtEmailNumber.Text
                        .ClientFaxNo = txtFaxNumber.Text
                        .ClientMobileNo = txtMobileNumber.Text
                        .ClientName = txtClientName.Text
                        .ClientTelNo = txtTelephoneNumberHome.Text
                        .ClientTelNoOff = txtTelephoneNumberoffice.Text
                        .Client.TaxRegistrationNumber = txtVATRegistrationNumber.Text
                        If chkVATRegistered.Checked Then
                            .Client.TaxRegistered = True
                        End If
                        .UserDefFldACode = ddlExcessListA.SelectedValue
                        .UserDefFldBCode = ddlExcessListB.SelectedValue
                        .UserDefFldCCode = ddlExcessListC.SelectedValue
                        .UserDefFldDCode = ddlNCBPercentage.SelectedValue
                        .UserDefFldECode = ddlCoverType.SelectedValue
                    End With
                    Session(CNClaim) = oOpenClaim
                End If
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            End If
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        Protected Sub BindAddressData()
            Dim Addresses As NexusProvider.AddressCollection = CType(Session.Item(CNAAAddresses), NexusProvider.AddressCollection)
            DisplayAddress(Addresses)
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            If Addresses IsNot Nothing Then
                With oOpenClaim
                    .Client.Address.Address1 = Addresses(0).Address1
                    .Client.Address.Address2 = Addresses(0).Address2
                    .Client.Address.Address3 = Addresses(0).Address3
                    .Client.Address.Address4 = Addresses(0).Address4
                    .Client.Address.AddressType = Addresses(0).AddressType
                    .Client.Address.CountryCode = Addresses(0).CountryCode
                    .Client.Address.PostCode = Addresses(0).PostCode
                End With
                Session(CNClaim) = oOpenClaim
            End If
        End Sub

        Protected Sub DisplayAddress(ByVal Addresses As NexusProvider.AddressCollection)
            Dim AddressStringBuilder As New StringBuilder
            For Each oAddress As NexusProvider.Address In Addresses
                AddressStringBuilder.AppendLine(oAddress.Address1)
                AddressStringBuilder.AppendLine(oAddress.Address2)
                AddressStringBuilder.AppendLine(oAddress.Address3)
                AddressStringBuilder.AppendLine(oAddress.Address4)
                AddressStringBuilder.AppendLine(GetDescriptionForCode(NexusProvider.ListType.PMLookup, oAddress.CountryCode, "country"))
                AddressStringBuilder.AppendLine(oAddress.PostCode)
            Next
            txtClientAddress.Text = AddressStringBuilder.ToString()
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Private Sub FillUserDefinedCombo()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sUserLookup As String
            Dim olist As NexusProvider.LookupListCollection

            sUserLookup = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.ClaimsUDTA, Nothing, CType(Session.Item(CNProductCode), String), Nothing)
            If String.IsNullOrEmpty(sUserLookup) = False Then
                olist = oWebService.GetList(NexusProvider.ListType.UserDefined, sUserLookup, True, True)
                ddlExcessListA.DataSource = olist
                ddlExcessListA.DataTextField = "Description"
                ddlExcessListA.DataValueField = "Code"
                ddlExcessListA.DataBind()
                ddlExcessListA.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_None")))
            End If

            sUserLookup = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.ClaimsUDTB, Nothing, CType(Session.Item(CNProductCode), String), Nothing)
            If String.IsNullOrEmpty(sUserLookup) = False Then
                olist = oWebService.GetList(NexusProvider.ListType.UserDefined, sUserLookup, True, True)
                ddlExcessListB.DataSource = olist
                ddlExcessListB.DataTextField = "Description"
                ddlExcessListB.DataValueField = "Code"
                ddlExcessListB.DataBind()
                ddlExcessListB.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_None")))
            End If

            sUserLookup = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.ClaimsUDTC, Nothing, CType(Session.Item(CNProductCode), String), Nothing)
            If String.IsNullOrEmpty(sUserLookup) = False Then
                olist = oWebService.GetList(NexusProvider.ListType.UserDefined, sUserLookup, True, True)
                ddlExcessListC.DataSource = olist
                ddlExcessListC.DataTextField = "Description"
                ddlExcessListC.DataValueField = "Code"
                ddlExcessListC.DataBind()
                ddlExcessListC.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_None")))
            End If

            sUserLookup = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.ClaimsUDTD, Nothing, CType(Session.Item(CNProductCode), String), Nothing)
            If String.IsNullOrEmpty(sUserLookup) = False Then
                olist = oWebService.GetList(NexusProvider.ListType.UserDefined, sUserLookup, True, True)
                ddlNCBPercentage.DataSource = olist
                ddlNCBPercentage.DataTextField = "Description"
                ddlNCBPercentage.DataValueField = "Code"
                ddlNCBPercentage.DataBind()
                ddlNCBPercentage.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_None")))
            End If

            sUserLookup = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.ClaimsUDTE, Nothing, CType(Session.Item(CNProductCode), String), Nothing)
            If String.IsNullOrEmpty(sUserLookup) = False Then
                olist = oWebService.GetList(NexusProvider.ListType.UserDefined, sUserLookup, True, True)
                ddlCoverType.DataSource = olist
                ddlCoverType.DataTextField = "Description"
                ddlCoverType.DataValueField = "Code"
                ddlCoverType.DataBind()
                ddlCoverType.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_None")))
            End If
        End Sub

        Protected Sub chkVATRegistered_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVATRegistered.CheckedChanged
            If chkVATRegistered.Checked = True Then
                txtVATRegistrationNumber.Enabled = True
            Else
                txtVATRegistrationNumber.Enabled = False
            End If
        End Sub
    End Class
End Namespace
