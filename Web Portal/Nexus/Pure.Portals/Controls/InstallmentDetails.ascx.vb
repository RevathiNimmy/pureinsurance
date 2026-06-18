Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Imports System.Web.HttpContext


Namespace Nexus
    Partial Class Controls_InstallmentDetails
        Inherits System.Web.UI.UserControl

#Region "Private Variables"

        Private sPlanStatus As String = Nothing
        Private iPlanKey As Integer = Nothing
        Private iPlanVersion As Integer = Nothing
        Public iLinkButtonEnable As Integer = 0
#End Region


#Region "Protected Events"

        ''' <summary>
        ''' On Load of control instalment collection is get through SAM
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If (Not IsPostBack) Then

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AllowReverseAllocation
                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.GetUserAuthorityValue(oUserAuthority)
                If oUserAuthority IsNot Nothing AndAlso oUserAuthority.UserAuthorityValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) Then
                    hdnReverseInstalmentAuthority.Value = oUserAuthority.UserAuthorityValue
                    hdnReverseInstalmentNoOfDays.Value = oUserAuthority.UserAuthorityOptionalValue1
                Else
                    hdnReverseInstalmentAuthority.Value = "0"
                    hdnReverseInstalmentNoOfDays.Value = "1"
                End If

                Dim oBaseInstalment As New BaseInstalment()

                Dim oInstalmentsCollection As New NexusProvider.InstalmentsCollection
                Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"),  _
                                                                  Config.NexusFrameWork)
                Dim sDocumentRef As String = ""

                sDocumentRef = Request.QueryString("DocRef")
                iPlanKey = Convert.ToInt32(Request.QueryString("FinancePlanKey"))
                iPlanVersion = Convert.ToInt32(Request.QueryString("FinancePlanVersion"))
                msg_noinstalment.Visible = True
                Try
                    '' Replace this with the Document Ref
                    If sDocumentRef <> "" Then
                        oBaseInstalment.GetPremiumFinancePlanDetails(sDocumentRef, iPlanKey, iPlanVersion, Nothing)
                    End If

                    If Session(CNFinancePlanDetails) IsNot Nothing Then

                        oInstalmentsCollection = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Instalments
                        oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails

                        If oInstalmentsCollection IsNot Nothing AndAlso oInstalmentsCollection.Count > 0 AndAlso _
                            oFinancePlanDetails IsNot Nothing Then
                            hdnPlanStatus.Value = FinancePlanStatusString(oFinancePlanDetails.StatusInd)
                            If (sDocumentRef <> "") Then
                                iPlanKey = oFinancePlanDetails.PFPremiumFinanceKey
                                sPlanStatus = Nexus.Constants.FinancePlanStatusDesc(oFinancePlanDetails.StatusInd)
                                Dim olblTitle As Label = _
                                        CType( _
                                            CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName),  _
                                                  ContentPlaceHolder).FindControl("lblTitle"),  _
                                            Label)
                                olblTitle.Text = olblTitle.Text & " " & iPlanKey.ToString() & " - " & sPlanStatus
                            End If

                            'Bind the grid with retrieved instalments details
                            grdInstallmentQuotes.DataSource = oInstalmentsCollection
                            grdInstallmentQuotes.DataBind()
                            msg_noinstalment.Visible = False

                            If Session(CNInstalmentPlanMode) <> InstalmentPlanType.View AndAlso oFinancePlanDetails.StatusInd = NexusProvider.FinancePlanStatus.Item040 OrElse oFinancePlanDetails.StatusInd = NexusProvider.FinancePlanStatus.Item900 Then
                                btnReverseInstalment.Enabled = True
                            Else
                                btnReverseInstalment.Enabled = False
                            End If
                        End If
                    End If
                    If grdInstallmentQuotes.Rows.Count = 0 Then
                        btnReverseInstalment.Enabled = False
                    End If
                Catch
                    oInstalmentsCollection = Nothing
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Number column and Date columns value is set 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdInstallmentQuotes_RowDataBound(ByVal sender As Object, _
                                                        ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) _
            Handles grdInstallmentQuotes.RowDataBound
            Dim nPFInstalmentsKey As Integer

            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(4).Text = FormatNumber(e.Row.Cells(4).Text, 2)
                e.Row.Cells(2).Text = FormatDateTime(e.Row.Cells(2).Text, DateFormat.ShortDate)
                e.Row.Cells(3).Text = FormatDateTime(e.Row.Cells(3).Text, DateFormat.ShortDate)

                nPFInstalmentsKey = grdInstallmentQuotes.DataKeys(e.Row.RowIndex).Values("PFInstalmentsKey").ToString()
                If (e.Row.Cells(3).Text = DateTime.MinValue) Then
                    e.Row.Cells(3).Text = ""
                End If
                If (e.Row.Cells(1).Text = "0" And e.Row.Cells(4).Text = "0.00") Then
                    e.Row.Visible = False
                End If
                If (e.Row.Cells(1).Text = "0") Then
                    e.Row.Visible = True
                    e.Row.Cells(1).Text = "Deposit"
                    iLinkButtonEnable = 1
                End If
                If iLinkButtonEnable = 0 Then
                    Dim lnkEdit As LinkButton = e.Row.Cells(7).FindControl("btnEdit")
                    Dim nFinancePlanKey As Integer
                    Dim nFinancePlanVersion As Integer
                    Dim oUserAuthority As New NexusProvider.UserAuthority
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    oUserAuthority.UserCode = Session(CNLoginName)
                    oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanUpdateInstalmentStatus
                    oWebservice.GetUserAuthorityValue(oUserAuthority)

                    Dim oUserAuthorityUpdateInstalmentDueDate As New NexusProvider.UserAuthority
                    oUserAuthorityUpdateInstalmentDueDate.UserCode = Session(CNLoginName)
                    oUserAuthorityUpdateInstalmentDueDate.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanUpdateInstalmentDueDate
                    oWebservice.GetUserAuthorityValue(oUserAuthorityUpdateInstalmentDueDate)

                    Dim oUserAuthorityEditInstalmentNoofDays As New NexusProvider.UserAuthority
                    oUserAuthorityEditInstalmentNoofDays.UserCode = Session(CNLoginName)
                    oUserAuthorityEditInstalmentNoofDays.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.EditInstalmentNoOfDays
                    oWebservice.GetUserAuthorityValue(oUserAuthorityEditInstalmentNoofDays)
                    If (e.Row.Cells(5).Text.ToLower() = "new" OrElse e.Row.Cells(5).Text.ToLower() = "hold") AndAlso (Session(CNInstalmentPlanMode) = InstalmentPlanType.edit AndAlso Request.QueryString("DocRef") Is Nothing) Then
                        If oUserAuthority.UserAuthorityValue IsNot Nothing AndAlso oUserAuthority.UserAuthorityValue = "1" OrElse (oUserAuthorityUpdateInstalmentDueDate.UserAuthorityValue IsNot Nothing AndAlso oUserAuthorityUpdateInstalmentDueDate.UserAuthorityValue = "1") Then
                            e.Row.Cells(7).Visible = True
                        Else
                            e.Row.Cells(7).Text = ""
                        End If

                        lnkEdit.Visible = True
                        iLinkButtonEnable = 1
                        If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                            nFinancePlanKey = Request.QueryString("FinancePlanKey")
                            nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
                        End If

                    ElseIf Session(CNInstalmentPlanMode) = InstalmentPlanType.View AndAlso Request.QueryString("DocRef") Is Nothing Then

                        e.Row.Cells(7).Visible = True
                        lnkEdit.Visible = True
                        lnkEdit.Text = GetLocalResourceObject("lblView")
                    ElseIf Request.QueryString("DocRef") IsNot Nothing Then
                        e.Row.Cells(7).Text = ""

                    Else
                        e.Row.Cells(7).Text = ""
                    End If
                    If HttpContext.Current.Session.IsCookieless Then
                        lnkEdit.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditInstallment.aspx?modal=true&KeepThis=true&TB_iframe=true&height=600&width=750&PfInstalmentKey=" & nPFInstalmentsKey & "&FinancePlanKey=" & nFinancePlanKey & "&FinancePlanVersion=" & nFinancePlanVersion & "&UserAuthorityInstalmentDueDate=" & oUserAuthorityUpdateInstalmentDueDate.UserAuthorityValue & "&UserAuthorityChangeStatus=" & oUserAuthority.UserAuthorityValue & "&Installmentno=" & e.Row.Cells(1).Text & "' , null);return false;"
                    Else
                        lnkEdit.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "/Modal/EditInstallment.aspx?modal=true&KeepThis=true&TB_iframe=true&height=600&width=750&PfInstalmentKey=" & nPFInstalmentsKey & "&FinancePlanKey=" & nFinancePlanKey & "&FinancePlanVersion=" & nFinancePlanVersion & "&UserAuthorityInstalmentDueDate=" & oUserAuthorityUpdateInstalmentDueDate.UserAuthorityValue & "&UserAuthorityChangeStatus=" & oUserAuthority.UserAuthorityValue & "&Installmentno=" & e.Row.Cells(1).Text & "' , null);return false;"
                    End If
                    If (lnkEdit.Visible = True Or lnkEdit.Text = "") And lnkEdit.Text <> GetLocalResourceObject("lblView") Then
                        iLinkButtonEnable = 1
                    End If
                End If
                If e.Row.Cells(5).Text.ToUpper() = "COLLECTED" Then

                    Dim dtPostedDate As Date

                    If Not String.IsNullOrEmpty(e.Row.Cells(3).Text) Then
                        dtPostedDate = e.Row.Cells(3).Text
                        dtPostedDate = DateAdd(DateInterval.Day, CInt(hdnReverseInstalmentNoOfDays.Value), dtPostedDate)

                        If CDate(Now) > CDate(dtPostedDate) Then
                            e.Row.Cells(8).Text = "1"
                        Else
                            e.Row.Cells(8).Text = "0"
                        End If
                    End If
                End If
            End If
        End Sub
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            msg_noinstalment.InnerHtml = GetLocalResourceObject("msg_no_installment").ToString()
        End Sub
        Protected Sub btnReverseInstalment_Click(sender As Object, e As EventArgs)

            If Session(CNFinancePlanDetails) IsNot Nothing Then
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim nPFInstalmentsKey As Integer
                Dim nReturn As Integer

                For Each row As GridViewRow In grdInstallmentQuotes.Rows
                    Dim chkReverseInstalment As CheckBox = row.FindControl("chkReverseInstalment")
                    nPFInstalmentsKey = grdInstallmentQuotes.DataKeys(row.RowIndex).Values("PFInstalmentsKey").ToString()
                    If chkReverseInstalment.Checked And row.Cells(5).Text.ToUpper() = "COLLECTED" Then
                        nReturn = oWebservice.ReverseCollectedInstalment(nPFInstalmentsKey, hdnPlanStatus.Value)
                        If nReturn <> 1 Then
                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "AlertMessage", "<script language=""JavaScript"" type=""text/javascript"">alert('" & GetLocalResourceObject("msg_ReverseInstalmentError").ToString().Replace("#InstalmentNumber", row.Cells(1).Text) & "');</script>")
                            Exit Sub
                        End If
                    End If
                Next
                Cache("InstalmentTabActive") = "1"
                Response.Redirect(Request.RawUrl)
            End If
        End Sub
#End Region

    End Class
End Namespace