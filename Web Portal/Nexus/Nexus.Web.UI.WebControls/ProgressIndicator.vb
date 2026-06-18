Imports System
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Security.Permissions

<ToolboxData("<{0}:ProgressIndicator runat=server id=upProgress />")> _
Public Class ProgressIndicator
    Inherits System.Web.UI.UpdateProgress

    'declare private variables
    Private _centeroncontrol, _overlaycssclass As String

    ''' <summary>
    ''' Property specifies the control which the update indicator will be centered on
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Browsable(True)> _
    <Category("Behavior")> _
    <Themeable(True)> _
    <DefaultValue("")> _
    <Description("Control which the update indicator will be centered on. This control must be inside the update panel")> _
    Public WriteOnly Property CenterOnControl() As String
        Set(ByVal value As String)
            _centeroncontrol = value
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
    <Description("Sets the css class of the overlay div")> _
    Public WriteOnly Property OverlayCssClass() As String
        Set(ByVal value As String)
            _overlaycssclass = value
        End Set
    End Property

    Protected Overloads Overrides Sub OnPreRender(ByVal e As EventArgs)

        Dim oAssociatedUpdatePanel As Object = Nothing
        oAssociatedUpdatePanel = Me.FindControl(Me.AssociatedUpdatePanelID)
        If oAssociatedUpdatePanel IsNot Nothing Then
            If Me.FindControl(Me.AssociatedUpdatePanelID).Visible Then
                'Add js function to page. 
                'This will only be added once, even if multiple controls are on a single page, due to the static key
                Me.Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "ProgressIndicator", Page.ClientScript.GetWebResourceUrl(Me.GetType(), "Nexus.Web.UI.WebControls.ProgressIndicator.js"))
                'if we have a control to center on then we need to find the client id of this to pass into the javascript
                'otherwise find the associated update panel
                Dim sControlToCenterOn As String
                If _centeroncontrol IsNot Nothing Then
                    sControlToCenterOn = _centeroncontrol
                Else
                    sControlToCenterOn = Me.Parent.FindControl(Me.AssociatedUpdatePanelID).ClientID
                End If

                'fire javascript to center the progress indicator on page load
                'dynamic key used to ensure that multiple entries are made on the page if multiple controls are on a single page
                Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID & "Location", "window.setTimeout('SetUpdateIndicatorLocation(\'" & sControlToCenterOn & "\' , \'" & Me.ClientID & "\' , \'" & _overlaycssclass & "\')',300);", True)

                'build javascript to fire resize div on window resize. 
                'this can't just specify resize as IE doesn't handle this well
                'string should end up like the one below:
                'Sys.Application.add_load(function(sender, args) {$addHandler(window, 'resize', window_resize);});var resizeTimeoutId;
                'function window_resize(e) {window.clearTimeout(resizeTimeoutId);resizeTimeoutId = window.setTimeout('SetUpdateIndicatorLocation(\'UpdatePanelID\' , \'UpdateProgressControl\' , \'CssClass\');', 100);}
                Dim sb As New Text.StringBuilder
                sb.Append("Sys.Application.add_load(function(sender, args) {$addHandler(window, 'resize', ")
                sb.Append(Me.ClientID)
                sb.Append("_resize);});")
                sb.Append("var ")
                sb.Append(Me.ClientID)
                sb.Append("_resizeTimeoutId;function ")
                sb.Append(Me.ClientID)
                sb.Append("_resize (e) {window.clearTimeout(")
                sb.Append(Me.ClientID)
                sb.Append("_resizeTimeoutId);")
                sb.Append(Me.ClientID)
                sb.Append("_resizeTimeoutId = window.setTimeout('SetUpdateIndicatorLocation(\'")
                sb.Append(sControlToCenterOn)
                sb.Append("\' , \'")
                sb.Append(Me.ClientID)
                sb.Append("\' , \'")
                sb.Append(_overlaycssclass)
                sb.Append("\');', 100);}")

                'Register the script as a startup script
                'Dynamic key used to ensure that multiple entries are made on the page if multiple controls are on a single page
                Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID & "OnResize", sb.ToString, True)
            End If
        End If
        MyBase.OnPreRender(e)
    End Sub
End Class
