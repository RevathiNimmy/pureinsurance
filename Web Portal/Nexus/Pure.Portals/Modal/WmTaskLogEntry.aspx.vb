Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Modal_WmTaskLogEntry : Inherits System.Web.UI.Page
        ''' <summary>
        ''' This event is fired on Ok Button Click which adds a new Task based on the LogText and TaskInstancekey.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
            If Request.QueryString("Mode").ToString <> WMMode.ViewTaskLog.ToString() Then
                Dim oWorkManager As New NexusProvider.WorkManager
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oWorkManager.TaskInstanceKey = CType(Session.Item(CNWMTaskInstanceKey), Integer)
                oWorkManager.LogText = txtEntry.Text
                oWorkManager.IsDeleted = False
                If oWorkManager.LogText IsNot String.Empty Then
                    oWebService.AddWmTaskLog(oWorkManager.TaskInstanceKey, oWorkManager.LogText)
                End If
            End If
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('page');", True)
        End Sub
        ''' <summary>
        ''' This Pageload Event binds all the related values to the controls on PageLoad
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Dim sDefaultfocus As String = "fnSetFocus('" & txtEntry.ClientID & "');"
            'Me.ClientScript.RegisterStartupScript(GetType(String), "StartupScripts", sDefaultfocus, True)
			Page.SetFocus(txtEntry)
            Dim oWorkManagerTaskfromSession As NexusProvider.WorkManager = CType(Session.Item(CNWMWorkManagerCollection), NexusProvider.WorkManagerCollection).Item(CType(Request("TaskKey"), Integer))
            Dim sMode As String = Request.QueryString("Mode")
            Dim sCreatedBy As String = Nothing
            Dim dtDate As DateTime
            If sMode = WMMode.ViewTaskLog.ToString() Then
                Dim oTaskLogfromSession As NexusProvider.TaskLog = CType(Session.Item(CNWMTaskLogCollection), NexusProvider.TaskLogCollection).Item(CType(Request("TaskInstanceKey"), Integer))
                txtCreatedBy.Text = oTaskLogfromSession.UserName
                txtDate.Text = oTaskLogfromSession.DateCreated
                txtEntry.Text = oTaskLogfromSession.LogText
                txtCreatedBy.Enabled = False
                txtDate.Enabled = False
                txtEntry.Enabled = False
            Else
                txtEntry.Enabled = True
                sCreatedBy = oWorkManagerTaskfromSession.UserCode
                dtDate = oWorkManagerTaskfromSession.DueDate
                txtDate.Text = Date.Now
                txtDate.Enabled = False
                txtCreatedBy.Text = Session(CNLoginName).ToString.Trim
            End If
        End Sub
        ''' <summary>
        ''' Clears the value of the TextBox and closes the Thick Box
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
        ''' <summary>
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
    End Class
End Namespace
