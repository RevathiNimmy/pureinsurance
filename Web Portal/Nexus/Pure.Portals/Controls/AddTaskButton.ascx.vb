Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus


    Partial Class AddTaskButtonCntrl : Inherits System.Web.UI.UserControl
       
        Private s_CallingApp As String = String.Empty
        Public Property CallingApp() As String
            Get
                Return s_CallingApp
            End Get

            Set(value As String)
                s_CallingApp = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Session(CNWMMode) = WMMode.Add
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnAddTask.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/WrmTask.aspx?mode=add&modal=true&FromPage=WM&KeepThis=true&TB_iframe=true&height=500&width=750&CallingApp=" + CallingApp + "' , null);return false;"
            Else
                btnAddTask.OnClientClick = "tb_show(null ,'" & AppSettings("webroot") & "/Modal/WrmTask.aspx?mode=add&modal=true&FromPage=WM&KeepThis=true&TB_iframe=true&height=500&width=750&CallingApp=" + CallingApp + "' , null);return false;"
            End If
        End Sub
    End Class
End Namespace