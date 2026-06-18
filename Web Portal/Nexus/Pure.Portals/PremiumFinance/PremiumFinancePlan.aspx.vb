Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Configuration
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class PremiumFinance_PremiumFinancePlan
        Inherits CMS.Library.Frontend.clsCMSPage
        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'Cleaning of the session values
                ClearQuote()
                ClearClaims()
                ClearHeader()
            End If
            If Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "AddPlan" Then

                btnNewPlan.Visible = True
                Session(CNInstalmentPlanMode) = InstalmentPlanType.Add
            ElseIf Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "EditPlan" Then
                Session(CNInstalmentPlanMode) = InstalmentPlanType.edit
                btnNewPlan.Visible = False
            Else
                btnNewPlan.Visible = False
            End If



        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnClientCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/secure/agent/FindClient.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&IncludeAgent=true' , null);return false;"
            Else
                btnClientCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/secure/agent/FindClient.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&IncludeAgent=true' , null);return false;"
            End If
        End Sub

        Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
            If Page.IsValid Then
                FindPremiumFinancePlans()
            End If
        End Sub
        Public Sub FindPremiumFinancePlans()
            Dim oInstalment As New BaseInstalment

            ' If claim number is specified, search by claim number
            If txtClaimNumber.Text.Trim() <> String.Empty Then
                oInstalment.GetPremiumFinancePlanByClaimNumber(hvAgentKey.Value, ddlStatus.SelectedValue, txtClaimNumber.Text.Trim())
            Else
                oInstalment.GetPremiumFinancePlan(hvAgentKey.Value, ddlStatus.SelectedValue, Nothing)
            End If

            If CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection) IsNot Nothing AndAlso CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection).Count > 0 Then
                grdvSearchResults.AllowPaging = True
                grdvSearchResults.PageIndex = 0
                grdvSearchResults.DataSource = CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection)
                grdvSearchResults.DataBind()

            Else
                grdvSearchResults.DataSource = Nothing
                grdvSearchResults.DataBind()
                grdvSearchResults.AllowPaging = False


            End If
            FillPartySession()
        End Sub

        Protected Sub grdvSearchResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSearchResults.PageIndexChanging
            CType(sender, GridView).PageIndex = e.NewPageIndex
            CType(sender, GridView).DataSource = CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection)
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub grdvSearchResults_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSearchResults.RowCommand
            If e.CommandName <> "Sort" AndAlso e.CommandName <> "Page" AndAlso e.CommandName <> "Edit" Then
                Dim nFinancePlanKey As Integer = Convert.ToInt32(CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection).Item(e.CommandArgument).FinancePlanKey)
                Dim nFinancePlanVersion As Integer = Convert.ToInt32(CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection).Item(e.CommandArgument).FinancePlanVersion)
                Dim sInsuranceRef As String = Convert.ToString(CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection).Item(e.CommandArgument).InsuranceRef)
                If e.CommandName = "Edit" AndAlso e.CommandArgument <> "" Then
                    Session(CNInstalmentPlanMode) = InstalmentPlanType.edit
                Else
                    Session(CNInstalmentPlanMode) = InstalmentPlanType.View
                End If
                Response.Redirect("~/PremiumFinance/FinancePlanDetails.aspx?ShowButtons=True&FinancePlanKey=" & nFinancePlanKey & "&FinancePlanVersion=" & nFinancePlanVersion & "&PolicyNumber=" & sInsuranceRef)
            End If
        End Sub


        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then

                If e.Row.Cells(7).Text <> "" Then
                    Dim btnEdit1 As LinkButton = e.Row.FindControl("btnEdit")
                    Dim btnView1 As LinkButton = e.Row.FindControl("btnView")
                    If btnEdit1 IsNot Nothing Then
                        If UserCanDoTask("EditInstalmentPlan") Then
                            If e.Row.Cells(7).Text = "Item040" OrElse e.Row.Cells(7).Text = "Item140" Then
                                btnEdit1.Visible = True
                            Else
                                btnEdit1.Attributes.Add("style", "visibility:hidden")
                            End If
                        Else
                            btnEdit1.Attributes.Add("style", "visibility:hidden")
                        End If
                    End If

                    If (oPortal.MaskBankAccountNumber) Then
                        Dim account_no As String = e.Row.Cells(3).Text.Trim()
                        If account_no <> "" And account_no.Length > 4 Then
                            Dim sFirstStr As String = Mid(account_no, 1, account_no.Length - 4)
                            Dim sLastStr As String = Mid(account_no, sFirstStr.Length + 1)
                            For iCount As Integer = 0 To sFirstStr.Length - 1
                                sFirstStr = sFirstStr.Replace(sFirstStr.Chars(iCount), "*")
                            Next
                            account_no = sFirstStr & sLastStr
                            e.Row.Cells(3).Text = account_no
                        End If
                    End If
                    If btnView1 IsNot Nothing Then
                        If UserCanDoTask("ViewInstalmentPlan") Then
                            btnView1.Visible = True
                        Else
                            btnView1.Visible = False
                        End If
                    End If

                    Select Case e.Row.Cells(7).Text
                        Case "Item000"
                            e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item000)
                        Case "Item010"
                            e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item010)
                        Case "Item011"
                            e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item011)
                        Case "Item012"
                            e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item012)
                        Case "Item040"
                            e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item040)
                        Case "Item140"
                            e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item140)
                        Case "Item900"
                            e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item900)
                        Case "Item990"
                            e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item990)
                        Case "Item999"
                            e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item999)
                        Case Else
                            e.Row.Cells(7).Text = ""
                    End Select
                End If
                'Set the value Null in case the there is no valid dates.
                If e.Row.Cells(6).Text = Date.MinValue Then
                    e.Row.Cells(6).Text = ""
                End If

                Dim btnEditN As LinkButton = e.Row.FindControl("btnEdit")
                Dim btnViewN As LinkButton = e.Row.FindControl("btnView")
                If btnEditN IsNot Nothing Then
                    If UserCanDoTask("EditInstalmentPlan") AndAlso (e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item040) OrElse e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item140) OrElse e.Row.Cells(7).Text = FinancePlanStatusDesc(FinancePlanStatus.Item010)) Then

                        btnEditN.Visible = True
                    Else
                        btnEditN.Visible = False
                    End If
                End If
                If btnViewN IsNot Nothing Then
                    If UserCanDoTask("ViewInstalmentPlan") Then
                        btnViewN.Visible = True
                    Else
                        btnViewN.Visible = False
                    End If
                End If
            End If

        End Sub

        Protected Sub grdvSearchResults_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvSearchResults.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oFinancePlanCollection As NexusProvider.FinancePlanCollection = CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection)
            oFinancePlanCollection.SortColumn = e.SortExpression
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
            oFinancePlanCollection.SortingOrder = _sortDirection
            oFinancePlanCollection.Sort()
            CType(sender, GridView).DataSource = oFinancePlanCollection
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub ddlStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStatus.SelectedIndexChanged
            If hvAgentKey.Value <> "" Then
                FindPremiumFinancePlans()
            End If
        End Sub

        Protected Sub FillPartySession()
            Dim oNewParty As NexusProvider.BaseParty
            Dim oWebservice As NexusProvider.ProviderBase
            oWebservice = New NexusProvider.ProviderManager().Provider
            If hvAgentKey.Value <> "" Then
                oNewParty = oWebservice.GetParty(hvAgentKey.Value)
                Session(CNParty) = oNewParty
            End If

        End Sub

        Protected Sub btnNewPlan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewPlan.Click
            'Party data is required on the next page "PlanTransactions" 
            FillPartySession()
            Response.Redirect("~/PremiumFinance/PlanTransactions.aspx")
        End Sub

        ''' <summary>
        ''' Check if the Instalment Plan is locked by another user
        ''' </summary>
        ''' <param name="nFinancePlanKey">Finance Plan Key</param>
        ''' <param name="sBranchCode">Branch Code</param>
        ''' <returns>Username who has the lock, empty string if not locked</returns>
        Private Function CheckInstalmentPlanLock(ByVal nFinancePlanKey As Integer, ByVal sBranchCode As String) As String
            Dim sLockedByUser As String = String.Empty
            Dim oWebService As NexusProvider.ProviderBase = Nothing

            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim oLockCollection As NexusProvider.LockCollection = oWebService.GetLockDetails(sBranchCode)

                If oLockCollection IsNot Nothing Then
                    For Each oLockItem As NexusProvider.Locks In oLockCollection
                        If oLockItem.LockName.Trim().ToLower() = "pfprem_finance_cnt" AndAlso oLockItem.LockValue = nFinancePlanKey Then
                            If oLockItem.IsExclusiveLock AndAlso
                               oLockItem.LockUserName.Trim().ToUpper() = Session(CNLoginName).ToString().Trim().ToUpper() AndAlso
                               oLockItem.SessionID.Trim() <> Session.SessionID.Trim() Then
                                ' Same user, different session - block
                                sLockedByUser = oLockItem.LockUserName.Trim()
                                Exit For
                            ElseIf oLockItem.LockUserName.Trim().ToUpper() <> Session(CNLoginName).ToString().Trim().ToUpper() Then
                                ' Different user - block
                                sLockedByUser = oLockItem.LockUserName.Trim()
                                Exit For
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ' Log error but don't throw - allow process to continue
            Finally
                oWebService = Nothing
            End Try

            Return sLockedByUser
        End Function

        ''' <summary>
        ''' Validate exclusive lock before editing plan
        ''' </summary>
        ''' <param name="nFinancePlanKey">Finance Plan Key</param>
        ''' <returns>True if can proceed, False if locked by another user</returns>
        Private Function ValidateExclusiveLockForEdit(ByVal nFinancePlanKey As Integer) As Boolean
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim oOptionSettings As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)

                If oOptionSettings IsNot Nothing AndAlso oOptionSettings.OptionValue = "1" Then
                    Dim sLockedByUser As String = CheckInstalmentPlanLock(nFinancePlanKey, Session(CNBranchCode).ToString)
                    If sLockedByUser.Trim.Length > 0 Then
                        Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_Planlocked_Error"), "{1}", sLockedByUser + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "instalmentPlanlocked", sMessage, True)
                        Return False
                    End If
                End If
            Catch ex As Exception
                ' If lock check fails, allow to continue
            Finally
                oWebService = Nothing
            End Try
            Return True
        End Function


        Protected Sub grdvSearchResults_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdvSearchResults.RowEditing
            Dim nFinancePlanKey As Integer = Convert.ToInt32(CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection).Item(e.NewEditIndex).FinancePlanKey)
            Dim nFinancePlanVersion As Integer = Convert.ToInt32(CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection).Item(e.NewEditIndex).FinancePlanVersion)
            Dim sInsuranceRef As String = Convert.ToString(CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection).Item(e.NewEditIndex).InsuranceRef)

            Session(CNInstalmentPlanMode) = InstalmentPlanType.edit

            If Not ValidateExclusiveLockForEdit(nFinancePlanKey) Then
                e.Cancel = True
                Return
            End If

            Response.Redirect("~/PremiumFinance/FinancePlanDetails.aspx?ShowButtons=True&FinancePlanKey=" & nFinancePlanKey & "&FinancePlanVersion=" & nFinancePlanVersion & "&PolicyNumber=" & sInsuranceRef)
        End Sub
    End Class
End Namespace
