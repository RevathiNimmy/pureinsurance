Imports System.Configuration.ConfigurationManager

Namespace Nexus
    Partial Class Controls_CtrlDashboard : Inherits System.Web.UI.UserControl
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
            Dim btnDashboard As LinkButton = New LinkButton()
            btnDashboard.ID = "btnDashboard"
            btnDashboard.CausesValidation = False
            If GetLocalResourceObject("btnDashboard") IsNot Nothing Then
                btnDashboard.Text = GetLocalResourceObject("btnDashboard")
            End If
            btnDashboard.SkinID = _CustomSkinid

            If HttpContext.Current.Session.IsCookieless Then
                btnDashboard.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "Modal/Dashboard.aspx?modal=true&KeepThis=true&FromPage=BG&TB_iframe=true&height=500&width=750&Mode=Dashboard' , null);return false;"
            Else
                btnDashboard.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/Dashboard.aspx?modal=true&KeepThis=true&FromPage=BG&TB_iframe=true&height=800&width=750&Mode=Dashboard' , null);return false;"
            End If
            Controls.Add(btnDashboard)
        End Sub

    End Class
End Namespace

