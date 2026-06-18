Imports CMS.Library
Imports System.Data
Imports Nexus.Library
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus


    Partial Class Modal_FindFAC : Inherits Frontend.clsCMSPage

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load

            If Not IsPostBack Then
                Dim ReinsurancepageCacheID As Guid
                ReinsurancepageCacheID = Guid.NewGuid()
                ViewState.Add("ReinsurancepageCacheID", ReinsurancepageCacheID.ToString)
            End If
            PopulateDropDown()
            FindReInsurer()
        End Sub

        Private Sub FindReInsurer(Optional ByVal bSearchButton As Boolean = False, Optional ByVal bNewSearch As Boolean = False)
            Dim oReinsurerColl As New NexusProvider.ReinsurerCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oReinsurer As New NexusProvider.ReinsurerSearchCriteria
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim sType As String
            If bSearchButton = False AndAlso bNewSearch = False AndAlso ViewState("ReinsurerpageCacheID") IsNot Nothing Then

                oReinsurer.IsFAX = False
                oReinsurer.IsFAXSpecified = False
                oReinsurer.MaxRowsToFetch = oPortal.MaxSearchResults
                oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
            Else

                If bSearchButton Then
                    If String.IsNullOrEmpty(txtReinsurerCode.Text) = False Then
                        oReinsurer.RICode = txtReinsurerCode.Text
                    End If
                    If String.IsNullOrEmpty(txtName.Text) = False Then
                        oReinsurer.RIName = txtName.Text
                    End If
                    If String.IsNullOrEmpty(txtFileCode.Text) = False Then
                        oReinsurer.FileCode = txtFileCode.Text
                    End If

                    oReinsurer.IsFAX = False
                    oReinsurer.IsFAXSpecified = False
                End If
                'to limit the search return from SAM
                oReinsurer.MaxRowsToFetch = oPortal.MaxSearchResults

                oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
                Cache.Insert(ViewState("ReinsurancepageCacheID"), oReinsurerColl, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))


            End If
            If bNewSearch Then
                oReinsurer.MaxRowsToFetch = 0
                txtReinsurerCode.Text = ""
                txtName.Text = ""
                txtFileCode.Text = ""
                oReinsurerColl = Nothing
            End If


            gvSearchResult.Visible = True
            gvSearchResult.DataSource = oReinsurerColl

            If oReinsurerColl IsNot Nothing AndAlso oReinsurerColl.Count > 0 Then
                gvSearchResult.AllowPaging = True
            Else
                gvSearchResult.AllowPaging = False
            End If
            gvSearchResult.DataBind()

        End Sub

        Private Sub PopulateDropDown()

            drpType.Items.Insert(0, New ListItem("Insurer", "Insurer"))
            drpType.DataBind()
            drpType.SelectedIndex = 0
            drpType.Enabled = False

        End Sub

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            FindReInsurer(True, False)
        End Sub

        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            FindReInsurer(False, True)
        End Sub

        Protected Sub gvSearchResult_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvSearchResult.PageIndexChanging
            gvSearchResult.PageIndex = e.NewPageIndex
            Dim oReinsurerdata As NexusProvider.ReinsurerCollection = CType(Cache.Item(ViewState("ReinsurancepageCacheID")), NexusProvider.ReinsurerCollection)
            If oReinsurerdata Is Nothing Then
                FindReInsurer()
            Else
                gvSearchResult.DataSource = oReinsurerdata
                gvSearchResult.DataBind()
            End If

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvSearchResult_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvSearchResult.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lnkSelect As LinkButton = e.Row.FindControl("lnkSelect")
                Dim sScript As String = "self.parent.addFac('" & DataBinder.Eval(e.Row.DataItem, "ReinsurerKey") & "','" & DataBinder.Eval(e.Row.DataItem, "ReinsurerCode") & "','" &
                    DataBinder.Eval(e.Row.DataItem, "RIName") & "','" & DataBinder.Eval(e.Row.DataItem, "CommissionPercentage") & "','" & DataBinder.Eval(e.Row.DataItem, "TaxPercentage") & "');"
                sScript = sScript & "self.parent.tb_remove();return false;"
                lnkSelect.Attributes.Add("onclick", sScript)
            End If
        End Sub
    End Class
    '
End Namespace
