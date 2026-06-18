Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports NexusProvider

Namespace Nexus
    Partial Class Modal_UpdateMediaStatus : Inherits System.Web.UI.Page

        Private sClientType As String = ""
        Dim oWebService As NexusProvider.ProviderBase

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
         
            'To set the Focus
            Page.SetFocus(GISMediaTypeStatus)

            If Request("MediaType") IsNot Nothing Then
                GISMediaTypeStatus.Value = Request("MediaType")
            End If
            txtUpdateDate.Text = FormatDateTime(Now(), DateFormat.ShortDate)
            rvUpdateDate.MaximumValue = FormatDateTime(DateTime.MaxValue, DateFormat.ShortDate)
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

       
        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oUpdateMediaTypeStatus As New CashListReceipts
            If Page.IsValid Then
                Try
                    If Session(CNCashListReceipt) IsNot Nothing Then
                        Dim oCashListReceipts As CashListReceipts = DirectCast(Session(CNCashListReceipt), CashListReceipts)
                        For Each oNewReceipt As CashListReceipt In oCashListReceipts
                            If oNewReceipt.IsSelected Then
                                oNewReceipt.Comments = Trim(txtComments.Text)
                                oNewReceipt.MediaTypeStatusCode = Trim(GISMediaTypeStatus.Value)
                                oNewReceipt.ModifiedDate = FormatDateTime(txtUpdateDate.Text, DateFormat.ShortDate)
                                oUpdateMediaTypeStatus.Add(oNewReceipt)
                            End If
                        Next
                        oWebService.UpdateReceiptMediaTypeStatus(oUpdateMediaTypeStatus)
                    End If
                Finally
                    oWebService = Nothing
                    oUpdateMediaTypeStatus = Nothing
                End Try
            End If

            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.RefreshStatus('" & Request.QueryString("PostbackTo") & "','RefreshMediaTypeStatus');", True)
            'Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
    End Class
End Namespace

