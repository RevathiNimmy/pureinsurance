Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_CtrlLetterWriting : Inherits System.Web.UI.UserControl

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
            Dim btnLetterWriting As LinkButton = New LinkButton()
            btnLetterWriting.ID = "btnLetterWriting"
            btnLetterWriting.CausesValidation = False
            If GetLocalResourceObject("btnLetterWriting") IsNot Nothing Then
                btnLetterWriting.Text = GetLocalResourceObject("btnLetterWriting")
            End If
            btnLetterWriting.SkinID = _CustomSkinid
			
            If HttpContext.Current.Session.IsCookieless Then
                btnLetterWriting.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/LetterWriting.aspx?modal=true&KeepThis=true&FromPage=BG&TB_iframe=true&height=500&width=750&Mode=LetterWriting' , null);return false;"
            Else
                btnLetterWriting.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/LetterWriting.aspx?modal=true&KeepThis=true&FromPage=BG&TB_iframe=true&height=500&width=750&Mode=LetterWriting' , null);return false;"
            End If
            controls_CtrlLetterWriting.Controls.Add(btnLetterWriting)
        End Sub
    End Class
End Namespace

