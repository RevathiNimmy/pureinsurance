Imports NexusProvider.SAMForInsurance
Imports Nexus.Library
Imports System.Configuration.ConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Framework_ViewReserveItems
        Inherits CMS.Library.Frontend.clsCMSPage
#Region "Page Events"
        ''' <summary>
        ''' This evnet is fired on Page Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Session(CNMode) Is Nothing Then
                Response.Redirect(AppSettings("WebRoot") & "Login.aspx", False)
            End If
            If Not IsPostBack Then
                Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
                If Request.QueryString("PerilIndex") IsNot Nothing Then
                    iPeril = CInt(Request.QueryString("PerilIndex"))
                    Session(CNClaimPerilIndex) = iPeril
                ElseIf Session(CNClaimPerilIndex) IsNot Nothing Then
                    iPeril = Session(CNClaimPerilIndex)
                End If
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)

                'if page is loaded first time then setting of the status of progres bar
                ucProgressBar.OverviewStyle = "complete"
                ucProgressBar.PerilsStyle = "in-progress"
                ucProgressBar.SummaryStyle = "incomplete"
                ucProgressBar.ReinsuranceStyle = "incomplete"
                ucProgressBar.CompleteStyle = "incomplete"

                If oClaim IsNot Nothing Then
                    grdvReserveItem.DataSource = oClaim.ClaimPeril(iPeril).Reserve
                    grdvReserveItem.DataBind()
                    lblPageHeading.Text = lblPageHeading.Text & oClaim.ClaimPeril(iPeril).Description
                End If
            End If
        End Sub
        ''' <summary>
        ''' This event is fired on Page Pre Init.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            ' CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
#End Region
#Region "GridView Events"
        ''' <summary>
        ''' This event is fired on Grid View Row DataBound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvReserveItem_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvReserveItem.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim lblDescription As Label = CType(e.Row.FindControl("lblDescription"), Label)

                If lblDescription IsNot Nothing And Session.Item(CNReserveDescriptions) IsNot Nothing Then
                    lblDescription.Text = CType(Session.Item(CNReserveDescriptions), Hashtable).Item(CType(e.Row.DataItem, NexusProvider.Reserve).TypeCode)
                End If

                Dim lblCurrentReserve As Label = CType(e.Row.FindControl("lblCurrentReserve"), Label)

                Select Case CType(Session.Item(CNMode), Mode)
                    Case Mode.NewClaim
                        lblCurrentReserve.Text = New Money(CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString()
                    Case Mode.EditClaim, Mode.ViewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery
                        If lblCurrentReserve IsNot Nothing Then
                            Dim sRevisedReserve As Double = CType(e.Row.DataItem, NexusProvider.Reserve).RevisedReserve
                            Dim sInitialReserve As Double = CType(e.Row.DataItem, NexusProvider.Reserve).InitialReserve
                            Dim sCurrentReserve As Double = sRevisedReserve + sInitialReserve
                            lblCurrentReserve.Text = New Money(sCurrentReserve, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString()
                        End If
                End Select
            End If
        End Sub
#End Region
#Region "Button Events"
        ''' <summary>
        ''' This event is fired on Next Button Click.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
            Session.Remove(CNClaimPerilIndex)
            Response.Redirect("~/Claims/Perils.aspx", False)
        End Sub

#End Region


    End Class

End Namespace
