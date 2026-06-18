Imports System.Web
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    ''' <summary>
    ''' Allow new users to register, this will create a part within backoffice
    ''' </summary>
    Partial Class Register : Inherits Frontend.clsCMSPage

        ''' <summary>
        ''' Check the user isn't already authenticated as an agent, redirect to agent page if so.
        ''' Allow a user to register or edit their existing details if logged in as a custemer
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Response.Cache.SetCacheability(HttpCacheability.NoCache)

            If HttpContext.Current.User.Identity.IsAuthenticated Then

                Select Case CType(Session(CNLoginType), LoginType)
                    Case LoginType.Agent
                        'agent user should not use this page. redirect them to the most relevant page instead
                        If PersonalClient.Visible Then
                            Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?mode=add", False)
                        Else
                            Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?mode=add", False)
                        End If
                    Case LoginType.Customer
                        'only customers should be able to edit their party details, an agent should use the agent pages
                        PnlCheck.Visible = False
                        If Not IsPostBack Then
                            PopulateFields()
                        End If
                End Select
            End If
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "EnsureChecked", _
                "<script language=""JavaScript"" type=""text/javascript"">function EnsureChecked(oSrc, args){args.IsValid = document.getElementById('" + ChkConfirmation.ClientID + "').checked;}</script>")
        End Sub

        ''' <summary>
        ''' Load the party details into the form
        ''' </summary>
        Protected Sub PopulateFields()

            lblHeading.Text = GetLocalResourceObject("lbl_EditDetails").ToString() ' "Edit Details"
            lblPageInfo.Text = GetLocalResourceObject("lbl_EditDetailsText").ToString() '"Enter any details that have changed and click Submit."
           
            Select Case True
                Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                    PersonalClient.Party = Session(CNParty)
                Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                    CorporateClient.Party = Session(CNParty)
            End Select

        End Sub

        ''' <summary>
        ''' Add or Update the party details to BackOffice via SAM and email details to user if a new user.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click

            If Page.IsValid Then

                If User.Identity.IsAuthenticated And CType(Session.Item(CNLoginType), LoginType) = LoginType.Customer Then


                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    Dim user As MembershipUser = Nothing
                    Dim sClientCode As String = Nothing

                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            sClientCode = CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim
                            user = Membership.GetUser(CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim)
                            PersonalClient.UpdateParty(oParty)
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            sClientCode = CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim
                            user = Membership.GetUser(CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim)
                            CorporateClient.UpdateParty(oParty)
                    End Select

                    Try
                        If user IsNot Nothing Then
                            Dim oContact As NexusProvider.Contact
                            oContact = oParty.Contacts(NexusProvider.ContactType.Email)
                            user.Email = oContact.Number.Trim

                            Membership.UpdateUser(user)

                            'Dim myProfile As ProfileCommon = CType(ProfileCommon.Create(oParty.UserName, True), ProfileCommon)
                            Dim myProfile As MembershipProvider.CustomProfile = MembershipProvider.CustomProfile.GetUserProfile(sClientCode)

                            With myProfile.RegistrationDetails

                                If oContact IsNot Nothing Then
                                    .Email = oContact.Number
                                End If

                                .PartyKey = oParty.Key
                            End With

                            myProfile.Save()
                        End If
                        
                        oWebService.UpdateParty(oParty)
                        Session(CNParty) = oParty
                    Finally
                        oWebService = Nothing
                    End Try

                    Response.Redirect("~/secure/QuoteRetrieval.aspx", False)

                Else

                    If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DefaultUserType = "Personal" Then
                        If Not String.IsNullOrEmpty(Membership.GetUserNameByEmail(PersonalClient.Email)) Then
                            vldEmailExists.IsValid = False
                        End If
                    Else
                        If Not String.IsNullOrEmpty(Membership.GetUserNameByEmail(CorporateClient.Email)) Then
                            vldEmailExists.IsValid = False
                        End If
                    End If

                    If vldEmailExists.IsValid = True Then
                        'Add party if an agent or logged in but without a matching backup office partykey, otherwise update
                        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim oParty As NexusProvider.BaseParty = Nothing

                        Select Case CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DefaultUserType.Trim.ToUpper
                            Case "PERSONAL"
                                PersonalClient.UpdateParty(oParty)
                            Case "CORPORATE"
                                CorporateClient.UpdateParty(oParty)
                        End Select

                        Try
                            oWebService.AddParty(oParty)

                            oParty = oWebService.GetParty(oParty.Key)

                            Select Case True
                                Case TypeOf oParty Is NexusProvider.CorporateParty
                                    With CType(oParty, NexusProvider.CorporateParty)
                                        oParty.UserName = .ClientSharedData.ShortName.Trim
                                    End With
                                Case TypeOf oParty Is NexusProvider.PersonalParty
                                    With CType(oParty, NexusProvider.PersonalParty)
                                        oParty.UserName = .ClientSharedData.ShortName.Trim
                                    End With
                            End Select

                            Session(CNParty) = oParty
                        Finally
                            oWebService = Nothing
                        End Try

                        'Dim myProfile As ProfileCommon = CType(ProfileCommon.Create(oParty.UserName, True), ProfileCommon)
                        Dim myProfile As MembershipProvider.CustomProfile = MembershipProvider.CustomProfile.GetUserProfile(oParty.UserName)

                        With myProfile.RegistrationDetails

                            Dim oContact As NexusProvider.Contact = oParty.Contacts(NexusProvider.ContactType.Email)
                            If oContact IsNot Nothing Then
                                .Email = oContact.Number
                            End If

                            .PartyKey = oParty.Key
                        End With

                        myProfile.Save()

                        Dim sPassword As String = Utils.makePassword(7)
                        Dim user As MembershipUser = Membership.CreateUser(oParty.UserName, sPassword, myProfile.RegistrationDetails.Email())

                        'SEND REGISTRATION EMAIL
                        Dim oEmailTemplate As Config.EmailTemplate = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).EmailTemplates.EmailTemplate("Registration")

                        If oEmailTemplate IsNot Nothing Then

                            'SET UP HASH TABLE FOR DATA THAT NEEDS TO BE COLLECTED FOR EMAIL
                            Dim EmailDetails As New Hashtable
                            EmailDetails.Add("[!TITLE!]", GetLocalResourceObject("lbl_Email_Title").ToString()) 'Insurance Registration
                            EmailDetails.Add("[!HEADER!]", GetLocalResourceObject("lbl_Email_Title").ToString()) 'Insurance Registration
                            If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DefaultUserType = "Personal" Then
                                EmailDetails.Add("[!NAME!]", PersonalClient.Title & " " & PersonalClient.FirstName & " " & PersonalClient.Surname)
                            Else 'for corporate
                                EmailDetails.Add("[!NAME!]", CorporateClient.CompanyName)
                            End If
                            EmailDetails.Add("[!USERNAME!]", oParty.UserName)
                            EmailDetails.Add("[!PASSWORD!]", sPassword)

                            'SEND REGISTRATION EMAIL
                            SendEmail(oEmailTemplate.Sender, myProfile.RegistrationDetails.Email,
                                GetLocalResourceObject("lbl_Email_Title").ToString(), "", EmailDetails,
                                oEmailTemplate.Path, AppSettings("WebRoot"), Nothing, Nothing, Nothing)
                        End If

                        If oParty.UserName Is Nothing Then
                            Response.Redirect("~/default.aspx", False)
                        End If

                        Security.FormsAuthentication.SetAuthCookie(oParty.UserName, False)
                        Session.Add(CNLoginType, LoginType.Customer)
                        Session.Add(CNParty, oParty)
                        Session.Add(CNLoginName, oParty.UserName) 'store UserName in session for use in application

                        Select Case CType(Session.Item(CNMode), Mode)

                            Case Mode.Save, Mode.Buy

                                'Now the user is registered go back to QQPremium to either complete the save or buy
                                Response.Redirect("~/QQPremium.aspx", False)

                            Case Else

                                'need to check if user is immediatley register against PN: 41223
                                Response.Redirect("~/Secure/QuoteRetrieval.aspx?NewUser=1", False)
                        End Select

                    End If
                End If

            End If

        End Sub

        Protected Sub vldConfirmation_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldConfirmation.ServerValidate

            args.IsValid = ChkConfirmation.Checked

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Select Case CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DefaultUserType.Trim.ToUpper
                Case "PERSONAL"
                    PersonalClient.Visible = True

                    If Not IsPostBack Then
                        PersonalClient.SetFocus() 'To Set Focus on PersonalClient Control 
                    End If

                Case "CORPORATE"
                    CorporateClient.Visible = True

                    If Not IsPostBack Then
                        CorporateClient.SetFocus() 'To Set Focus on CorporateClient Control
                    End If

            End Select
        End Sub
    End Class

End Namespace

