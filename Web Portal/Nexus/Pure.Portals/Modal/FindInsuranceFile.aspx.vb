Imports System.Web.Configuration
Imports Nexus.Library
Imports CMS.library
Imports Nexus.Utils
Imports Nexus.Constants
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager

Partial Class Modal_FindInsuranceFile
    Inherits Frontend.clsCMSPage

    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'create a unique key and add this to viewstate
            'this will be used to cache the results of the SAM call
            Dim pageCacheID As Guid
            pageCacheID = Guid.NewGuid
            ViewState.Add("pageCacheID", pageCacheID.ToString)
        End If
        'In case of Renewal "Policy" should be default selected
        If Request.QueryString("Page") = "Renewal" Then
            drpFileType.SelectedValue = "POLICY"
            drpFileType.Enabled = False
        ElseIf Request.QueryString("Page") = "EL" Then 'During EventList Client COde should be selected and ReadOnly
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        With CType(Session(CNParty), NexusProvider.CorporateParty)
                            txtClientCode.Text = .ClientSharedData.ShortName
                        End With
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        With CType(Session(CNParty), NexusProvider.PersonalParty)
                            txtClientCode.Text = .ClientSharedData.ShortName
                        End With
                End Select
            End If
            txtClientCode.Enabled = False
        End If
    End Sub


    ''' <summary>
    ''' Set the master page to the ModalPageTemplate set in config as we are running as a modal page
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
    End Sub

    ''' <summary>
    ''' Call BindGrid to get search results. Clear any previous results from cache first
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Stores search results in session to be used when rebinding on page index change</remarks>
    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        Cache.Remove(ViewState("pageCacheID"))
        BindGrid()
    End Sub

    ''' <summary>
    ''' Sets the new page index and rebinds data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdvSearchResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSearchResults.PageIndexChanging
        grdvSearchResults.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    ''' <summary>
    ''' Calls FindPolicy passing in search parameters and populates grid with results
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindGrid()
        'try to get the search results from the cache
        Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection = _
            CType(Cache.Item(ViewState("pageCacheID")), NexusProvider.InsuranceFileDetailsCollection)

        If oInsuranceFileDetailsCollection Is Nothing Then
            'Get search results by calling FindPolicy
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sInsuranceRef As String = IIf(String.IsNullOrEmpty(txtReference.Text), Nothing, txtReference.Text)
            Dim sRiskIndex As String = IIf(String.IsNullOrEmpty(txtRiskIndex.Text), Nothing, txtRiskIndex.Text)
            Dim sClientShortName As String = IIf(String.IsNullOrEmpty(txtClientCode.Text), Nothing, txtClientCode.Text)
            Dim sQuoteType As NexusProvider.InsuranceFileType
            Dim iMaxRowsToFetch As Integer = oPortal.MaxSearchResults

            Select Case drpFileType.SelectedValue
                Case "ALL"
                    sQuoteType = NexusProvider.InsuranceFileTypes.ALL
                Case "QUOTE"
                    sQuoteType = NexusProvider.InsuranceFileTypes.QUOTE
                Case "MTAQUOTE"
                    sQuoteType = NexusProvider.InsuranceFileTypes.MTAQUOTE
                Case "POLICY"
                    sQuoteType = NexusProvider.InsuranceFileTypes.POLICY
                Case "RENEWAL"
                    sQuoteType = NexusProvider.InsuranceFileTypes.RENEWAL
            End Select


            oInsuranceFileDetailsCollection = oWebService.FindPolicy(v_sInsuranceRef:=sInsuranceRef, v_sRiskIndex:=sRiskIndex, v_sClientShortName:=sClientShortName, _
                                                                     v_oQuoteType:=sQuoteType, v_bShowLapsedOnly:=False, v_iMaxRowsToFetch:=iMaxRowsToFetch)

            If oInsuranceFileDetailsCollection IsNot Nothing Then
                'add the results to the cache so that we don't need to call FindPolicy again
                'todo - cache length should be taken from config
                Cache.Insert(ViewState("pageCacheID"), oInsuranceFileDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
                If oInsuranceFileDetailsCollection.Count >= oPortal.MaxSearchResults Then
                    'create a custom validator
                    Dim cstMaxResults As New CustomValidator
                    cstMaxResults.IsValid = False
                    'look for a validation message in the page resources, but if there is not one defined add a default message
                    cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                    cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    'add the validator to the page, this will have the effect of making the page invalid
                    Page.Validators.Add(cstMaxResults)
                End If

            End If
        End If

        'Bind the grid to the search results and make sure that it is visible
        grdvSearchResults.DataSource = oInsuranceFileDetailsCollection
        grdvSearchResults.DataBind()
        grdvSearchResults.Visible = True
    End Sub

    ''' <summary>
    ''' Clears form, hides the results grid and also clears search results from cache
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
        grdvSearchResults.Visible = False
        txtClientCode.Text = String.Empty
        txtReference.Text = String.Empty
        txtRiskIndex.Text = String.Empty
        drpFileType.SelectedIndex = 0
        Cache.Remove(ViewState("pageCacheID"))
    End Sub

    Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'NOTE - this will need to be changed to give each row a unique id
            'this needs to be matched in markup for the menu (id="Menu_<%# Eval("InsuranceFileKey") %>")
            e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.InsuranceFileDetails).InsuranceFileKey)
        End If
    End Sub
End Class

