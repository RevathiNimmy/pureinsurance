Imports System.Configuration.ConfigurationManager

Namespace Nexus
    Partial Class Controls_CtrlWorkManagerDashboard : Inherits System.Web.UI.UserControl
        Private _CustomSkinid As String = "btnSM"
        Public Property CustomSkinID() As String
            Get
                Return _CustomSkinid
            End Get
            Set(ByVal value As String)
                _CustomSkinid = value
            End Set
        End Property
        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            Dim btnWMDashboard As LinkButton = New LinkButton()
            btnWMDashboard.ID = "btnWMDashboard"
            btnWMDashboard.CausesValidation = False
            btnWMDashboard.Text = "Dashboard"
            btnWMDashboard.SkinID = _CustomSkinid

            If HttpContext.Current.Session.IsCookieless Then
                btnWMDashboard.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "Modal/WorkManagerDashboard.aspx?modal=true&KeepThis=true&TB_iframe=true&height=800&width=1200' , null);return false;"
            Else
                btnWMDashboard.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/WorkManagerDashboard.aspx?modal=true&KeepThis=true&TB_iframe=true&height=800&width=1200' , null);return false;"
            End If
            Controls.Add(btnWMDashboard)
        End Sub

    End Class
End Namespace
