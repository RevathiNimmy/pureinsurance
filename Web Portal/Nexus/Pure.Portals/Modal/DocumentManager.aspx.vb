Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus

Partial Class Modal_DocumentManager
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sHeaderText As String 'this will hold the page header text which will be formed as we go along

        If Not String.IsNullOrEmpty(Request.QueryString("PartyKey")) Then
            'we have a party key passed in so set properties on the controls
            'if it is the claim or quote that is required these should be picked up automatically
            Dim iPartyKey As Integer
            Integer.TryParse(Request.QueryString("PartyKey"), iPartyKey)
            DocumentManager.PartyKey = iPartyKey

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oParty As NexusProvider.BaseParty = oWebService.GetParty(iPartyKey)
            Select Case True
                Case TypeOf oParty Is NexusProvider.CorporateParty
                    With CType(oParty, NexusProvider.CorporateParty)
                        If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                            SharepointView.PartyShortName = .ClientSharedData.ShortName.Trim()
                        ElseIf String.IsNullOrEmpty(.UserName) = False Then
                            SharepointView.PartyShortName = .UserName.Trim()
                        End If
                    End With
                Case TypeOf oParty Is NexusProvider.PersonalParty
                    With CType(oParty, NexusProvider.PersonalParty)
                        If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                            SharepointView.PartyShortName = .ClientSharedData.ShortName.Trim()
                        ElseIf String.IsNullOrEmpty(.UserName) = False Then
                            SharepointView.PartyShortName = .UserName.Trim()
                        End If
                    End With
            End Select
            sHeaderText = GetLocalResourceObject("ClientCode") & SharepointView.PartyShortName
        End If

        If Not String.IsNullOrEmpty(Request.QueryString("PartyCode")) Then
            'we may have the party code passed in the query string, so we'll use this overriding anything we set previously
            SharepointView.PartyShortName = Request.QueryString("PartyCode")
            sHeaderText = GetLocalResourceObject("ClientCode") & Request.QueryString("PartyCode")
        End If

        If Not String.IsNullOrEmpty(Request.QueryString("ClaimKey")) Then
            'we have a claim key passed in so set properties on the controls
            'if it is the claim or quote that is required these should be picked up automatically
            Dim iClaimKey As Integer
            Dim sClaimNumber As String
            sClaimNumber = Request.QueryString("ClaimNumber")
            Integer.TryParse(Request.QueryString("ClaimKey"), iClaimKey)
            DocumentManager.ClaimKey = iClaimKey

            If Not String.IsNullOrEmpty(sClaimNumber) Then
                SharepointView.ClaimNumber = Trim(sClaimNumber)

            Else
                'the sharepoint view control wants the party short name so we need to fetch this by making a call to SAM
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                'arch issue 268
                Dim oClaim As NexusProvider.ClaimDetails = GetClaimDetailsCall(iClaimKey)
                SharepointView.ClaimNumber = Trim(oClaim.ClaimNumber)

            End If
            sHeaderText = GetLocalResourceObject("ClaimNumber") & SharepointView.ClaimNumber
        End If

        If Not String.IsNullOrEmpty(Request.QueryString("FileKey")) Then
            'we have a party key passed in so set properties on the controls
            'if it is the claim or quote that is required these should be picked up automatically
            Dim iFileKey As Integer
            Integer.TryParse(Request.QueryString("FileKey"), iFileKey)
            DocumentManager.InsuranceFileKey = iFileKey
            'the sharepoint control wants the quote ref so make a SAM call to find this
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = oWebService.GetHeaderAndSummariesByKey(iFileKey)
            SharepointView.PolicyNumber = Trim(oQuote.InsuranceFileRef)
            sHeaderText = GetLocalResourceObject("PolicyNumber") & SharepointView.PolicyNumber

            'add the default header text "Document Manager" etc to the start of the header text, then set the label 
            sHeaderText = GetLocalResourceObject("lblDocumentManagerHeader") & sHeaderText
        End If
        'check the call made for reports and set the reports flag true and current branch code
        If Not String.IsNullOrEmpty(Request.QueryString("Reports")) Then
            SharepointView.Reports = Request.QueryString("Reports")
            SharepointView.Branch = Session(CNBranchCode)
            DocumentManager.Reports = Request.QueryString("Reports")
            DocumentManager.Branch = Session(CNBranchCode)
        End If
        ltPageHeader.Text = sHeaderText
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
    End Sub
End Class
