Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Text.RegularExpressions

Namespace Nexus

    <DefaultProperty("ThemeVariableName"), _
     ToolboxData("<{0}:Styles runat=\""server\""></{0}:Styles>"), _
     Themeable(True)> _
    Public Class Styles
        Inherits PlaceHolder

        <Bindable(True), _
         Category("Appearance"), _
         DefaultValue("%Theme"), _
         Localizable(False), _
         Description("Name of the variable that should be replaced by the actual theme path")> _
        Public Property ThemeVariableName() As String
            Get
                Dim s As String = CType(ViewState("ThemeVariableName"), String)
                Return (IIf(s Is Nothing, "%Theme", s))
            End Get
            Set(ByVal value As String)
                ViewState("ThemeVariableName") = value
            End Set
        End Property

        ''' <summary>
        ''' Get the theme path
        ''' </summary>
        Public ReadOnly Property ThemePath() As String
            Get
                If Me.Page.Request.ApplicationPath = "/" Then
                    Return String.Format("/App_Themes/{0}", Me.Page.Theme)
                Else
                    Return String.Format("{0}/App_Themes/{1}", Me.Page.Request.ApplicationPath, Me.Page.Theme)
                End If
            End Get
        End Property

        ''' <summary>
        ''' Fix controls before render
        ''' </summary>
        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
            MyBase.OnPreRender(e)
            If Me.Visible Then
                ' Hide any server side css 
                For Each c As Control In Me.Page.Header.Controls
                    If (TypeOf c Is HtmlControl AndAlso CType(c, HtmlControl).TagName.Equals("link", StringComparison.OrdinalIgnoreCase)) Then
                        c.Visible = False
                    End If
                Next
                ' Replace ThemeVariableName with actual theme path
                Dim reg As Regex = New Regex(ThemeVariableName, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                For Each c As Control In Me.Controls
                    If (TypeOf c Is LiteralControl) Then
                        Dim l As LiteralControl = CType(c, LiteralControl)
                        l.Text = reg.Replace(l.Text, Me.ThemePath)
                    End If
                Next
            End If
        End Sub
    End Class


End Namespace