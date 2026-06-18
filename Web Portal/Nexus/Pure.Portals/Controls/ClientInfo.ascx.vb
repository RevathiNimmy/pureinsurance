Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus
    Partial Class ClientInfo : Inherits System.Web.UI.UserControl
        Const ClientMode As String = "CLIENT_MODE"
        Dim ShowControl As Boolean = True
        Dim RestrictedPageURLs As String
        Dim sReturnUrl As String = String.Empty

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            ShowClient()
        End Sub

        Sub ShowClient()
            Dim RequestedPageURL As String = Request.Url.Segments(Request.Url.Segments.Length - 1).ToString
            Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oDocumentArchiveOptionSettings As NexusProvider.OptionTypeSetting

            If Session(CNLoginName) IsNot Nothing Then
                oDocumentArchiveOptionSettings = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, 10)
            End If

            'Dim RequestedQuery As String = Request.Url.Query
            RestrictedPageURLs = "login.aspx,FindClaim.aspx,FindClient.aspx,FindInsuranceFile.aspx"

            If RestrictedPageURLs.Contains(RequestedPageURL) Then
                ShowControl = False
            Else
                Select Case CType(Session(CNClientMode), Mode)
                    Case Mode.Edit
                        ShowControl = False
                    Case Mode.View
                        ShowControl = True
                    Case Else 'Add
                        Select Case CType(Session(CNMode), Mode)
                            Case Mode.NewClaim, Mode.EditClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.ViewClaim, Mode.ViewClaimPayment, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend
                                ShowControl = True
                            Case Else
                                ShowControl = False
                        End Select

                End Select
            End If

            If Session(CNIsAnonymous) IsNot Nothing AndAlso Session(CNIsAnonymous) = True Then
                Me.Visible = False
            ElseIf (((Session.Item(CNParty) IsNot Nothing AndAlso CType(Session.Item(CNParty), NexusProvider.BaseParty).Key = 0) Or Session(CNClaimQuote) IsNot Nothing) And ShowControl = False) Then
                'Party creation has not completed, so hide control
                Me.Visible = False
            ElseIf Session(CNParty) Is Nothing Then
                'No party selected, so hide control
                Me.Visible = False
            ElseIf Session.Item(CNParty) IsNot Nothing AndAlso CType(Session.Item(CNParty), NexusProvider.BaseParty).Key > 0 Then
                Me.Visible = True
                Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)
                If CType(ViewState(ClientMode), Mode) = Mode.View Then
                    If Request("Code") <> Nothing Then
                        oParty.UserName = Request("Code")
                        Session.Item(CNParty) = oParty
                    End If
                End If

                liClientCode.Visible = True
                'For NB Section
                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        With CType(oParty, NexusProvider.CorporateParty)
                            ltClientName.Text = .MainContact
                            hypClientName.Text = .MainContact
                            hypCompanyName.Text = .CompanyName 'to make the company name as hyper link
                            If Session(CNLoginType) = LoginType.Customer Then
                                hypCompanyName.NavigateUrl = "~/secure/QuoteRetrieval.aspx"
                            Else
                                If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    ltClientCode.Text = .ClientSharedData.ShortName.Trim()
                                    hypCompanyName.NavigateUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim()
                                ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                    ltClientCode.Text = .UserName.Trim()
                                    hypCompanyName.NavigateUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .UserName.Trim()
                                End If

                            End If
                            ltCompanyName.Text = .CompanyName
                            ltCompanyName.Visible = False
                            phldrCompanyName.Visible = True
                            ltClientCode.Visible = True
                            ltClientName.Visible = False
                        End With
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        With CType(oParty, NexusProvider.PersonalParty)
                            ltClientName.Text = .Title & " " & .Forename & " " & .Lastname
                            hypClientName.Text = .Title & " " & .Forename & " " & .Lastname
                            Session(CNPartyKey) = .Key
                            If Session(CNLoginType) = LoginType.Customer Then
                                hypClientName.NavigateUrl = "~/secure/QuoteRetrieval.aspx"
                                If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    ltClientCode.Text = .ClientSharedData.ShortName.Trim()
                                ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                    ltClientCode.Text = .UserName.Trim()
                                End If
                            Else
                                If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    hypClientName.NavigateUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim()
                                    ltClientCode.Text = .ClientSharedData.ShortName.Trim()
                                ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                    hypClientName.NavigateUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & .UserName.Trim()
                                    ltClientCode.Text = .UserName.Trim()
                                End If
                            End If
                            phldrClientName.Visible = True
                            ltClientName.Visible = False
                            ltClientCode.Visible = True
                        End With
                End Select

                If Session.Item(CNQuote) IsNot Nothing Then
                    'This code is added as when user view the client details the previouly visited Quote/Policy Reference is Displayed 
                    'Now it will be displayed Only when user enters the product screen.
                    RestrictedPageURLs = "LOGIN.ASPX,FINDCLAIM.ASPX,CLIENTPOLICYSEARCH.ASPX,PERSONALCLIENTDETAILS.ASPX,CORPORATECLIENTDETAILS.ASPX,RENEWALSELECTION.ASPX"
                    If RestrictedPageURLs.Contains(RequestedPageURL) Or RestrictedPageURLs.Contains(UCase(RequestedPageURL)) Then
                        plcPolicy.Visible = False
                    Else
                        plcPolicy.Visible = True
                    End If
                    'code Ends Here
                    If Session.Item(CNPolicy_Summary) IsNot Nothing Then
                        Dim oPolicySummary As NexusProvider.PolicySummary
                        oPolicySummary = Session.Item(CNPolicy_Summary)
                        '   If RequestedPageURL = "TransactionConfirmation.aspx" Or InStr(Request.ServerVariables("SCRIPT_NAME"), "Claims") Then
                        If oPolicySummary IsNot Nothing AndAlso oPolicySummary.Reference IsNot Nothing Then
                            lblPolicyRef.Text = oPolicySummary.Reference
                        End If
                        'To display Policy Ref On the MTA Transaction Page 
                        If lblPolicyRef.Text = "" Then
                            lblPolicyRef.Text = CType(Session.Item(CNQuote), NexusProvider.Quote).InsuranceFileRef
                        End If
                    Else
                        lblPolicyRef.Text = CType(Session.Item(CNQuote), NexusProvider.Quote).InsuranceFileRef
                    End If

                    'set up documents hyperlink to launch modal, passing the file key in the query 
                    hypQuoteDocs.NavigateUrl = "~/Modal/DocumentManager.aspx" & "?FileKey=" & CType(Session.Item(CNQuote), NexusProvider.Quote).InsuranceFileKey & "&PartyKey=" & oParty.Key & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=750', null);return false;"

                End If
                If Session(CNMode) IsNot Nothing AndAlso Session(CNMode) <> Mode.Review OrElse Not String.IsNullOrEmpty(lblPolicyRef.Text) Then
                    Dim WebRoot As String = AppSettings("WebRoot")
                    sReturnUrl = Request.Path.Replace(WebRoot, "~/")
                    Session(CNReturnURL) = sReturnUrl
                End If
                hypEventList.NavigateUrl = "~/secure/EventList.aspx?ReturnUrl=" + sReturnUrl
                'End of NB Section

                'For Claim Section
                If Session(CNClaimQuote) IsNot Nothing And InStr(Request.ServerVariables("SCRIPT_NAME"), "Claims") Then
                    plcPolicy.Visible = True
                    lblPolicyRef.Text = CType(Session(CNClaimQuote), NexusProvider.Quote).InsuranceFileRef 'Session(CNPolicyNumber)
                    'Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oPartyDetails As NexusProvider.InsuranceFileDetails = Nothing
                    Dim oBaseParty As NexusProvider.BaseParty = Nothing
                    'oPartyDetails = oWebservice.GetClaimPartyDetails(Session(CNInsuranceFileKey))
                    'If oPartyDetails IsNot Nothing Then
                    If Session.Item(CNParty) IsNot Nothing AndAlso CType(Session.Item(CNParty), NexusProvider.BaseParty).Key > 0 Then
                        oBaseParty = Session.Item(CNParty)
                        'oBaseParty = oWebservice.GetParty(oPartyDetails.PartyKey)
                    End If
                    If oBaseParty IsNot Nothing Then
                        Select Case True
                            Case TypeOf oBaseParty Is NexusProvider.CorporateParty
                                With CType(oBaseParty, NexusProvider.CorporateParty)
                                    ltClientName.Text = .MainContact
                                    hypClientName.Text = .MainContact
                                    hypCompanyName.Text = .CompanyName 'to make the company name as hyper link
                                    hypCompanyName.NavigateUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oBaseParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim()
                                    ltCompanyName.Text = .CompanyName
                                    ltCompanyName.Visible = False
                                    phldrCompanyName.Visible = True
                                    ltClientCode.Text = .ClientSharedData.ShortName.Trim()
                                    ltClientCode.Visible = True
                                    ltClientName.Visible = False
                                End With
                            Case TypeOf oBaseParty Is NexusProvider.PersonalParty
                                With CType(oBaseParty, NexusProvider.PersonalParty)
                                    ltClientName.Text = .Title & " " & .Forename & " " & .Lastname
                                    hypClientName.Text = .Title & " " & .Forename & " " & .Lastname
                                    hypClientName.NavigateUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oBaseParty.Key & "&Code=" & .ClientSharedData.ShortName.Trim()
                                    ltClientCode.Text = .ClientSharedData.ShortName.Trim()
                                    phldrClientName.Visible = True
                                    ltClientName.Visible = False
                                    ltClientCode.Visible = True
                                End With

                        End Select
                    End If
                    'End If
                    'set up documents hyperlink to launch modal, passing the file key in the query 
                    hypQuoteDocs.NavigateUrl = "~/Modal/DocumentManager.aspx" & "?FileKey=" & CType(Session(CNClaimQuote), NexusProvider.Quote).InsuranceFileKey & "&PartyKey=" & oParty.Key & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=750', null);return false;"

                End If
                'End of Claim Section
                'set up documents hyperlink to launch modal, passing the party key in the query string
                'Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                If oDocumentArchiveOptionSettings.OptionValue = "1" Then
                    If (ltClientCode.Text <> "") Then
                        Dim oDMESearchCriteria As New NexusProvider.DMESearchCriteria
                        Dim oDME As NexusProvider.DME
                        Dim Server_Name As String
                        Dim HTTPS_Protocol As String
                        Dim Requset_Protocol As String = "http://"
                        Server_Name = Request.ServerVariables("SERVER_NAME").ToString()
                        HTTPS_Protocol = Request.ServerVariables("HTTPS").ToString()
                        If HTTPS_Protocol.ToLower() = "on" Then
                            Requset_Protocol = "https://"
                        End If
                        'Initializing the values
                        oDMESearchCriteria.PartyCode = Trim(ltClientCode.Text)
                        oDMESearchCriteria.IncludeFiles = False
                        ''Sam Call with Search criteria
                        Try
                            oDME = oWebservice.FindDMEDocuments(oDMESearchCriteria)
                            If oDME IsNot Nothing And oDME.SubFolder IsNot Nothing And oDME.SubFolder.Count > 0 Then
                                hypClientDocs.NavigateUrl = "~/Modal/DMEDocumentManager.aspx" & "?modal=true&KeepThis=true&fromlink=client&TB_iframe=true&height=550&width=750&BranchCode=" & CType(Session(CNParty), NexusProvider.BaseParty).BranchCode & "&FolderName=" & oDME.SubFolder(0).Name.Trim() & "&FolderNum=" & oDME.SubFolder(0).FolderNum
                                If Session(CNQuote) IsNot Nothing Then
                                    hypQuoteDocs.NavigateUrl = "~/Modal/DMEDocumentManager.aspx" & "?modal=true&KeepThis=true&fromlink=policy&TB_iframe=true&height=550&width=750&BranchCode=" & CType(Session(CNQuote), NexusProvider.Quote).BranchCode & "&FolderName=" & oDME.SubFolder(0).Name.Trim() & "&FolderNum=" & oDME.SubFolder(0).FolderNum
                                ElseIf Session(CNClaimQuote) IsNot Nothing Then
                                    hypQuoteDocs.NavigateUrl = "~/Modal/DMEDocumentManager.aspx" & "?modal=true&KeepThis=true&fromlink=policy&TB_iframe=true&height=550&width=750&BranchCode=" & CType(Session(CNClaimQuote), NexusProvider.Quote).BranchCode & "&FolderName=" & oDME.SubFolder(0).Name.Trim() & "&FolderNum=" & oDME.SubFolder(0).FolderNum
                                Else
                                    hypQuoteDocs.NavigateUrl = "~/Modal/DMEDocumentManager.aspx" & "?modal=true&KeepThis=true&fromlink=policy&TB_iframe=true&height=550&width=750&BranchCode=" & CType(Session(CNParty), NexusProvider.Quote).BranchCode & "&FolderName=" & oDME.SubFolder(0).Name.Trim() & "&FolderNum=" & oDME.SubFolder(0).FolderNum
                                End If
                                Session("PolicyNo") = lblPolicyRef.Text
                            End If
                        Catch ex As System.Exception

                        End Try


                    End If
                Else
                    hypClientDocs.NavigateUrl = "~/Modal/DocumentManager.aspx" & "?PartyKey=" & oParty.Key & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=750', null);return false;"
                End If

            End If
        End Sub
        Public WriteOnly Property ShowLinkToClient() As Boolean
            Set(ByVal value As Boolean)
                If (value = True) Then
                    hypClientName.Visible = True
                    ltClientName.Visible = False
                    lblClientName.AssociatedControlID = hypClientName.ID.ToString

                    hypCompanyName.Visible = True
                    ltCompanyName.Visible = False
                    lblCompanyName.AssociatedControlID = hypCompanyName.ID.ToString
                Else
                    hypClientName.Visible = False
                    ltClientName.Visible = True
                    lblClientName.AssociatedControlID = ltClientName.ID.ToString

                    hypCompanyName.Visible = False
                    ltCompanyName.Visible = True
                    lblCompanyName.AssociatedControlID = ltCompanyName.ID.ToString
                End If
            End Set
        End Property
    End Class


End Namespace



