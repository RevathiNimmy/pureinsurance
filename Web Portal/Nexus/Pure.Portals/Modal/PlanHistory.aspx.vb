Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class Modal_PlanHistory
        Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oFinancePlanHistoryCollection As New NexusProvider.FinancePlanHistoryCollection
            oFinancePlanHistoryCollection = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PFHistory
            If oFinancePlanHistoryCollection IsNot Nothing Then
                FillGrid(oFinancePlanHistoryCollection)
            End If
            btnOk.Attributes.Add("onclick", "self.parent.tb_remove();return false;")
        End Sub
        Private Sub FillGrid(ByVal oFinancePlanHistoryCollection As NexusProvider.FinancePlanHistoryCollection)
            If oFinancePlanHistoryCollection.Count > 0 Then
                grdHistory.DataSource = oFinancePlanHistoryCollection
                grdHistory.DataBind()
            End If
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'setting the default master page
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub grdHistory_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdHistory.PageIndexChanging
            CType(sender, GridView).PageIndex = e.NewPageIndex
            CType(sender, GridView).DataSource = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PFHistory
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub grdHistory_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdHistory.RowCommand
            If e.CommandName = "Select" And e.CommandName <> "Page" Then
                Dim nPFPremiumFinanceVersionKey As Integer
                Dim nPFPremiumFinanceKey As Integer
                Dim sPlanStatus As String
                Dim nIndex As Integer = Convert.ToInt32(e.CommandArgument)
                Dim oRow As GridViewRow = grdHistory.Rows(nIndex)
                nPFPremiumFinanceVersionKey = oRow.Cells(0).Text.ToString().Trim()
                nPFPremiumFinanceKey = oRow.Cells(1).Text.ToString().Trim()
                sPlanStatus = oRow.Cells(3).Text.ToString().Trim()

                If sPlanStatus = FinancePlanStatusDesc(FinancePlanStatus.Item990) Then ''Plan in VIEW mode in case of superceded plan
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_updated('RedirectFinancePlanDetails','~/PremiumFinance/FinancePlanDetails.aspx?FinancePlanKey=" & nPFPremiumFinanceKey & "&FinancePlanVersion=" & nPFPremiumFinanceVersionKey & " ');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_updated('RedirectFinancePlanDetailsEdit','~/PremiumFinance/FinancePlanDetails.aspx?ShowButtons=True&FinancePlanKey=" & nPFPremiumFinanceKey & "&FinancePlanVersion=" & nPFPremiumFinanceVersionKey & " ');", True)
                End If
            End If

        End Sub

        Protected Sub grdHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdHistory.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.Cells(3).Text <> "" Then
                    Select Case e.Row.Cells(3).Text
                        Case "Item000"
                            e.Row.Cells(3).Text = FinancePlanStatusDesc(FinancePlanStatus.Item000)
                        Case "Item010"
                            e.Row.Cells(3).Text = FinancePlanStatusDesc(FinancePlanStatus.Item010)
                        Case "Item011"
                            e.Row.Cells(3).Text = FinancePlanStatusDesc(FinancePlanStatus.Item011)
                        Case "Item012"
                            e.Row.Cells(3).Text = FinancePlanStatusDesc(FinancePlanStatus.Item012)
                        Case "Item040"
                            e.Row.Cells(3).Text = FinancePlanStatusDesc(FinancePlanStatus.Item040)
                        Case "Item140"
                            e.Row.Cells(3).Text = FinancePlanStatusDesc(FinancePlanStatus.Item140)
                        Case "Item900"
                            e.Row.Cells(3).Text = FinancePlanStatusDesc(FinancePlanStatus.Item900)
                        Case "Item990"
                            e.Row.Cells(3).Text = FinancePlanStatusDesc(FinancePlanStatus.Item990)
                        Case "Item999"
                            e.Row.Cells(3).Text = FinancePlanStatusDesc(FinancePlanStatus.Item999)
                        Case Else
                            e.Row.Cells(3).Text = ""
                    End Select
                End If
            End If

        End Sub

        Protected Sub grdHistory_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdHistory.Sorting

            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oFinancePlanHistoryCollection As NexusProvider.FinancePlanHistoryCollection = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PFHistory
            oFinancePlanHistoryCollection.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oFinancePlanHistoryCollection.SortingOrder = _sortDirection
            oFinancePlanHistoryCollection.Sort()
            CType(sender, GridView).DataSource = oFinancePlanHistoryCollection
            CType(sender, GridView).DataBind()
        End Sub
    End Class
End Namespace
