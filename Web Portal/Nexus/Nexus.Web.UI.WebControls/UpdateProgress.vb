Imports System
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Security.Permissions

<ToolboxData("<{0}:UpdateProgress runat=server id=upProgress />")> _
Public Class UpdateProgress
    Inherits System.Web.UI.UpdateProgress

    'declare private variables
    Private _width, _height As Integer
    Private _center As Boolean
    Private _centeroncontrol As String

    ''' <summary>
    ''' width propery to set the width of the container div, needed to allow for positioning of div in center of updatepanel
    ''' </summary>
    <Browsable(True)> _
    <Category("Behavior")> _
    <Themeable(True)> _
    <DefaultValue("")> _
    <Description("Width propery to set the width of the container div")> _
    Public WriteOnly Property Width() As Integer
        Set(ByVal value As Integer)
            _width = value
        End Set
    End Property


    ''' <summary>
    ''' Height propery to set the height of the container div, needed to allow for positioning of div in center of updatepanel
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Browsable(True)> _
    <Category("Behavior")> _
    <Themeable(True)> _
    <DefaultValue("")> _
    <Description("Height propery to set the height of the container div")> _
    Public WriteOnly Property Height() As Integer
        Set(ByVal value As Integer)
            _height = value
        End Set
    End Property

    ''' <summary>
    ''' Property to determine if update indicator should be centered on update panel. 
    ''' If false then the update indicator will appear wherever it is placed in the page flow
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Browsable(True)> _
    <Category("Behavior")> _
    <Themeable(True)> _
    <DefaultValue("True")> _
    <Description("Determines if update indicator should be centered on update panel")> _
    Public WriteOnly Property Center() As Boolean
        Set(ByVal value As Boolean)
            _center = value
        End Set
    End Property

    ''' <summary>
    ''' Property specifies the control which the update indicator will be centered on
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Browsable(True)> _
    <Category("Behavior")> _
    <Themeable(True)> _
    <DefaultValue("")> _
    <Description("Control which the update indicator will be centered on")> _
    Public WriteOnly Property CenterOnControl() As String
        Set(ByVal value As String)
            _centeroncontrol = value
        End Set
    End Property

    Protected Overloads Overrides Sub OnPreRender(ByVal e As EventArgs)
        MyBase.OnPreRender(e)
        Me.Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "UpdateProgress", Page.ClientScript.GetWebResourceUrl(Me.GetType(), "Nexus.Web.UI.WebControls.UpdateProgress.js"))
        If _center Then
            'fire javascript to center the progress indicator on page load
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "UpdateProgressLocation", "SetUpdateDivLocation('" & Me.ClientID & "' , '" & FindControl(_centeroncontrol).ClientID & "' , '" & _width.ToString & " ' , '" & _height.ToString & "');", True)
            'fire javascript to center the progress indicator on window resize
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "UpdateProgressLocationOnResize", "document.body.OnResize=SetUpdateDivLocation('" & Me.ClientID & "' , '" & FindControl(_centeroncontrol).ClientID & "' , '" & _width.ToString & " ' , '" & _height.ToString & "');", True)
        End If
    End Sub
End Class
