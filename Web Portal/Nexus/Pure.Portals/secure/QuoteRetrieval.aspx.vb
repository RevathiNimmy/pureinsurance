Imports CMS.library
Imports Nexus.Utils
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_QuoteRetrieval : Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If HttpContext.Current.User.Identity.IsAuthenticated And CType(Session(CNLoginType), LoginType) = LoginType.Agent Then

                Response.Redirect("~/secure/agent/FindClient.aspx", False)
            Else
                SetPageProgress(2)

                If Request.QueryString("NewUser") = "1" Then
                    'this checks for the first time when user has been created 
                    'then add him to the customer role
                    If Not Roles.RoleExists("Customer") Then
                        Roles.CreateRole("Customer")
                    End If

                    If Not User.IsInRole("Customer") Then
                        Dim oParty As NexusProvider.BaseParty
                        oParty = Session(CNParty)
                        Roles.AddUserToRole(oParty.UserName, "Customer")
                    End If
                    lblconfirmmsg.Visible = True
                Else
                    lblconfirmmsg.Visible = False
                End If
                'this checks whether B2C Customer has New Business task
                If UserCanDoTask("NewBusiness") Then
                    ctrlNewQuote.Visible = True
                Else
                    ctrlNewQuote.Visible = False
                End If
                If UserCanDoTask("EditClientDetails") Then
                    btnEditClient.Visible = True
                Else
                    btnEditClient.Visible = False
                End If

                If Session.Item(CNQuoteMode) = QuoteMode.QuickQuote Then
                    Select Case CType(Session.Item(CNMode), Mode)
                        Case Mode.Save, Mode.Buy
                            'Now the user is registered go back to QQPremium to either complete the save or buy
                            Response.Redirect("~/QQPremium.aspx", False)
                    End Select
                End If
                If CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).AddressControl.ShowCountry = True Then
                    licountry.Visible = True
                Else
                    licountry.Visible = False
                End If
                RetrieveQuote()
            End If
        End Sub

        Sub RetrieveQuote()

            If TypeOf Session(CNParty) Is NexusProvider.PersonalParty Then

                Dim oParty As NexusProvider.PersonalParty = Session(CNParty)
                With oParty

                    lblNameTitle.Text = GetLocalResourceObject("lbl_ClientName")


                    lblName.Text = .Title & " " & .Forename & " " & .Lastname

                    Dim oContact As NexusProvider.Contact = .Contacts(NexusProvider.ContactType.Email)
                    If oContact IsNot Nothing Then
                        LblEmail.Text = oContact.Number
                    End If

                    oContact = .Contacts(NexusProvider.ContactType.HomePhone)
                    If oContact IsNot Nothing Then
                        LblTelephone.Text = oContact.AreaCode & oContact.Number
                    End If

                    'Get Correspondence Address
                    Dim oAddress As NexusProvider.Address = .Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                    If oAddress IsNot Nothing Then

                        LblAddress1.Text = oAddress.Address1
                        LblAddress2.Text = oAddress.Address2
                        LblAddress3.Text = oAddress.Address3
                        LblAddress4.Text = oAddress.Address4
                        LblPostcode.Text = oAddress.PostCode

                        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Try
                            'DH - This list will most likely be cached is its used often, so there won't be a performance hit
                            Dim oItem As NexusProvider.LookupListItem = oWebService.GetList(NexusProvider.ListType.PMLookup, "COUNTRY", True, False).Item(oAddress.CountryCode)
                            If oItem IsNot Nothing Then
                                lblCountry.Text = oItem.Description
                            End If
                        Finally
                            oWebService = Nothing
                        End Try

                    End If

                    ctrlClientQuotes.PartyKey = .Key

                    If .ClientSharedData.ShortName IsNot Nothing Then
                        ctrlClientClaims.UserName = .ClientSharedData.ShortName.Trim
                    End If


                End With
            ElseIf TypeOf Session(CNParty) Is NexusProvider.CorporateParty Then

                Dim oParty As NexusProvider.CorporateParty = Session(CNParty)
                With oParty

                    lblNameTitle.Text = GetLocalResourceObject("lbl_CompanyName")
                    lblName.Text = oParty.CompanyName

                    'MainContact will be visible only for Corporate Client
                    liMainContact.Visible = True
                    lblMainContact.Text = oParty.MainContact


                    ' get email
                    Dim oContact As NexusProvider.Contact = .Contacts(NexusProvider.ContactType.Email)
                    If oContact IsNot Nothing Then
                        LblEmail.Text = oContact.Number
                    End If

                    'Get Main Contact Number
                    oContact = .Contacts(NexusProvider.ContactType.HomePhone)
                    If oContact IsNot Nothing Then
                        LblTelephone.Text = oContact.AreaCode & oContact.Number
                    End If

                    'Get Correspondence Address
                    Dim oAddress As NexusProvider.Address = .Addresses(NexusProvider.AddressType.CorrespondenceAddress)
                    If oAddress IsNot Nothing Then

                        LblAddress1.Text = oAddress.Address1
                        LblAddress2.Text = oAddress.Address2
                        LblAddress3.Text = oAddress.Address3
                        LblAddress4.Text = oAddress.Address4
                        LblPostcode.Text = oAddress.PostCode

                        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Try
                            Dim oItem As NexusProvider.LookupListItem = oWebService.GetList(NexusProvider.ListType.PMLookup, "COUNTRY", True, False).Item(oAddress.CountryCode)
                            If oItem IsNot Nothing Then
                                lblCountry.Text = oItem.Description
                            End If
                        Finally
                            oWebService = Nothing
                        End Try

                    End If

                    ctrlClientQuotes.PartyKey = .Key
                    If .ClientSharedData.ShortName IsNot Nothing Then
                        ctrlClientClaims.UserName = .ClientSharedData.ShortName.Trim
                    End If

                End With
            End If

        End Sub

        Protected Sub btnEditClient_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditClient.Click
            Response.Redirect("~/register.aspx", False)
        End Sub

    End Class

End Namespace
