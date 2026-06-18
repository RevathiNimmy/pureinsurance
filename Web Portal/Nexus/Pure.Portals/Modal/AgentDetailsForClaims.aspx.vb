Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports NexusProvider.SAMForInsurance
Imports Nexus.Library
Imports Nexus.Utils

Namespace Nexus
    Partial Class Modal_AgentDetailsForClaims
        Inherits System.Web.UI.Page

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oClaimDetails As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oAddress As New NexusProvider.Address
            Dim oAddressCollection As New NexusProvider.AddressCollection
            'To set the Focus
            Page.SetFocus(txtTelephoneNumber)

            If Not IsPostBack Then
                Select Case CType(Session(CNMode), Mode)
                    Case Mode.NewClaim
                        txtContact.Enabled = True
                        txtTelephoneNumber.Enabled = True
                        txtEmailAddress.Enabled = True
                        txtFaxNumber.Enabled = True
                        txtAgentClaimNumber.Enabled = True
                    Case Mode.ViewClaim, Mode.PayClaim
                        txtContact.Enabled = False
                        txtTelephoneNumber.Enabled = False
                        txtEmailAddress.Enabled = False
                        txtFaxNumber.Enabled = False
                    Case Mode.EditClaim
                        txtContact.Enabled = True
                        txtTelephoneNumber.Enabled = True
                        txtEmailAddress.Enabled = True
                        txtFaxNumber.Enabled = True
                        txtAgentClaimNumber.Enabled = True
                End Select
                oAddress.Address1 = oClaimDetails.Insurer.Address.Address1
                oAddress.Address2 = oClaimDetails.Insurer.Address.Address2
                oAddress.Address3 = oClaimDetails.Insurer.Address.Address3
                oAddress.Address4 = oClaimDetails.Insurer.Address.Address4
                oAddress.AddressType = oClaimDetails.Insurer.Address.AddressType
                oAddress.CountryCode = oClaimDetails.Insurer.Address.CountryCode
                oAddress.CountryDescription = oClaimDetails.Insurer.Address.CountryDescription
                oAddress.PostCode = oClaimDetails.Insurer.Address.PostCode
                oAddressCollection.Add(oAddress)
                Session(CNAAAddresses) = oAddressCollection
                DisplayAddress(oAddressCollection)
                If HttpContext.Current.Session.IsCookieless Then
                    btnAgentAddress.OnClientClick = "tb_show(null ," & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & " /Modal/Address.aspx?modal=true&KeepThis=true&FromPage=CA&Page=Agent&AddressID=0&TB_iframe=true&height=300&width=650' , null);return false;"
                Else
                    btnAgentAddress.OnClientClick = "tb_show(null , '../Modal/Address.aspx?modal=true&KeepThis=true&FromPage=CA&Page=Agent&AddressID=0&TB_iframe=true&height=300&width=650' , null);return false;"
                End If
                DisplayInsurerDetails()

                If Request.UrlReferrer IsNot Nothing AndAlso (Not Request.UrlReferrer.AbsolutePath.Contains("Overview.aspx") Or Session(CNMode) = Mode.ViewClaim _
               Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.PayClaim _
               Or Session(CNMode) = Mode.TPRecovery) Then
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    DisableControls(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName))
                    btnAgentAddress.Enabled = False
                End If
            End If
            If Request("__EVENTARGUMENT") = "UpdateAddress" Then
                Dim sAddressData() As String = txtAddressData.Value.Split(";")
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

                If sAddressData(0).ToUpper = "UPDATE" Then
                    Dim oUpdateAddress As NexusProvider.Address

                    If oClaim.Insurer IsNot Nothing Then
                        oUpdateAddress = oClaim.Insurer.Address
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

                    Dim ClaimAddresses As NexusProvider.AddressCollection = CType(Session.Item(CNCAAddresses), NexusProvider.AddressCollection)
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
                    Session(CNCAAddresses) = ClaimAddresses

                End If
                Session(CNClaim) = oClaim
                BindAddressData()
            End If
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Select Case CType(Session(CNMode), Mode)
                Case Mode.NewClaim, Mode.EditClaim
                    If Session.Item(CNClaim) IsNot Nothing Then
                        Dim oInsurerDetatils As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        Dim Addresses As NexusProvider.AddressCollection = CType(Session.Item(CNCAAddresses), NexusProvider.AddressCollection)
                        Dim oContacts As New NexusProvider.ContactCollection
                        Dim oContactEmail As New NexusProvider.Contact
                        Dim oContactFax As New NexusProvider.Contact
                        Dim oContactTelephone As New NexusProvider.Contact
                        If txtEmailAddress.Text IsNot Nothing Then
                            oContactEmail.ContactType = NexusProvider.ContactType.Email
                            oContactEmail.Number = txtEmailAddress.Text
                            oContacts.Add(oContactEmail)
                        End If
                        If txtFaxNumber.Text IsNot Nothing Then
                            oContactFax.ContactType = NexusProvider.ContactType.Fax
                            oContactFax.Number = txtFaxNumber.Text
                            oContacts.Add(oContactFax)
                        End If
                        If txtTelephoneNumber.Text IsNot Nothing Then
                            oContactTelephone.ContactType = NexusProvider.ContactType.Main
                            oContactTelephone.Number = txtTelephoneNumber.Text
                            oContacts.Add(oContactTelephone)
                        End If
                        With oInsurerDetatils
                            .Insurer.Contact = oContacts
                            .Insurer.PartyClaimNumber = txtAgentClaimNumber.Text
                            .Insurer.ContactName = txtAgentName.Text.Trim
                            .Insurer.InsurerContact = txtContact.Text.Trim
                            .Insurer.InsurerEmail = txtEmailAddress.Text.Trim
                            .Insurer.InsurerFaxNo = txtFaxNumber.Text.Trim
                            .Insurer.InsurerTelNo = txtTelephoneNumber.Text.Trim()

                            If Addresses IsNot Nothing Then
                                For Each oAddress As NexusProvider.Address In Addresses
                                    With .Insurer.Address
                                        .Address1 = oAddress.Address1
                                        .Address2 = oAddress.Address2
                                        .Address3 = oAddress.Address3
                                        .Address4 = oAddress.Address4
                                        .AddressType = oAddress.AddressType
                                        .CountryCode = oAddress.CountryCode
                                        .CountryDescription = oAddress.CountryDescription
                                        .Key = oAddress.Key
                                        .PostCode = oAddress.PostCode
                                    End With
                                Next
                            End If
                        End With
                        Session(CNClaim) = oInsurerDetatils
                    End If
            End Select
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)

        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        Protected Sub BindAddressData()
            Dim Addresses As NexusProvider.AddressCollection = CType(Session.Item(CNCAAddresses), NexusProvider.AddressCollection)
            DisplayAddress(Addresses)
            Dim oInsurerDetails As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            If Addresses IsNot Nothing Then
                With oInsurerDetails
                    .Insurer.Address.Address1 = Addresses(0).Address1
                    .Insurer.Address.Address2 = Addresses(0).Address2
                    .Insurer.Address.Address3 = Addresses(0).Address3
                    .Insurer.Address.Address4 = Addresses(0).Address4
                    .Insurer.Address.AddressType = Addresses(0).AddressType
                    .Insurer.Address.CountryCode = Addresses(0).CountryCode
                    .Insurer.Address.PostCode = Addresses(0).PostCode
                End With
                Session(CNClaim) = oInsurerDetails
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
            txtAddress.Text = AddressStringBuilder.ToString()
        End Sub

        Protected Sub DisplayInsurerDetails()
            Dim oInsurerDetails As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oContact As NexusProvider.Contact
            txtAgentName.Text = oInsurerDetails.Insurer.ContactName
            txtAgentClaimNumber.Text = oInsurerDetails.Insurer.PartyClaimNumber
            Dim oAddress As New NexusProvider.Address
            Dim oAddressCollection As New NexusProvider.AddressCollection
            oAddress.Address1 = oInsurerDetails.Insurer.Address.Address1
            oAddress.Address2 = oInsurerDetails.Insurer.Address.Address2
            oAddress.Address3 = oInsurerDetails.Insurer.Address.Address3
            oAddress.Address4 = oInsurerDetails.Insurer.Address.Address4
            oAddress.Key = oInsurerDetails.Insurer.Address.Key
            oAddress.CountryCode = oInsurerDetails.Insurer.Address.CountryCode
            oAddress.CountryDescription = oInsurerDetails.Insurer.Address.CountryDescription
            oAddress.PostCode = oInsurerDetails.Insurer.Address.PostCode
            oAddressCollection.Add(oAddress)
            Session(CNCAAddresses) = oAddressCollection
            DisplayAddress(oAddressCollection)
            ' txtContact.Text = oInsurerDetails.Insurer.ContactName
            If oInsurerDetails.Insurer.Contact IsNot Nothing Then
                txtContact.Text = oInsurerDetails.Insurer.InsurerContact
            End If
            If oInsurerDetails.Insurer.InsurerEmail IsNot Nothing Then
                txtEmailAddress.Text = oInsurerDetails.Insurer.InsurerEmail
            End If
            If oInsurerDetails.Insurer.InsurerFaxNo IsNot Nothing Then
                txtFaxNumber.Text = oInsurerDetails.Insurer.InsurerFaxNo
            End If
            If oInsurerDetails.Insurer.InsurerTelNo IsNot Nothing Then
                txtTelephoneNumber.Text = oInsurerDetails.Insurer.InsurerTelNo
            End If

            'For Each oContact In oInsurerDetails.Insurer.Contact
            '    If oContact.ContactType = NexusProvider.ContactType.Email Then
            '        txtEmailAddress.Text = oContact.Number
            '    End If
            '    If oContact.ContactType = NexusProvider.ContactType.Fax Then
            '        txtFaxNumber.Text = oContact.Number
            '    End If
            '    If oContact.ContactType = NexusProvider.ContactType.Main Or oContact.ContactType = NexusProvider.ContactType.HomePhone Then
            '        txtTelephoneNumber.Text = oContact.Number
            '    End If
            'Next
        End Sub
    End Class
End Namespace
