Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Controls_SubAgents : Inherits System.Web.UI.UserControl

        Protected Sub BindSubAgentData()

            Dim SubAgents As NexusProvider.SubAgentCollection = CType(Session.Item(CNSubAgents), NexusProvider.SubAgentCollection)

            grdvSubAgents.DataSource = SubAgents
            grdvSubAgents.DataBind()
        End Sub

        ''' <summary>
        ''' Delete SubAgent 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgAddresses_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSubAgents.RowCommand
            If e.CommandName = "DeleteRow" Then
                Dim iCount As Integer
                Dim oColl As NexusProvider.SubAgentCollection = Session.Item(CNSubAgents)
                For iCount = 0 To oColl.Count - 1
                    If oColl(iCount).PartyKey = Convert.ToInt32(e.CommandArgument.ToString.Trim) Then
                        CType(Session.Item(CNSubAgents), NexusProvider.SubAgentCollection).Remove(iCount)
                        Exit For
                    End If
                Next
                BindSubAgentData()
            End If
        End Sub

        ''' <summary>
        ''' Address DataBound event for corporate / personal client
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgAddresses_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSubAgents.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim hypDelete As LinkButton = e.Row.FindControl("hypSubAgentDelete")
                hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.SubAgent).PartyKey
                If CType(Session(CNMode), Mode) <> Mode.View Then
                    'If Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.MTAQuote Or Session(CNQuoteMode) = QuoteMode.ReQuote Then
                    hypDelete.Visible = True
                Else
                    hypDelete.Visible = False
                End If
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request("__EVENTARGUMENT") = "RefreshSubAgent" Then
                Dim SubAgents As NexusProvider.SubAgentCollection = CType(Session.Item(CNSubAgents), NexusProvider.SubAgentCollection)
                Dim oNewSubagent As New NexusProvider.SubAgent
                With oNewSubagent
                    .PartyKey = hAgentKey.Value
                    .Name = hAgentName.Value
                    .Code = hAgentCode.Value
                End With
                If SubAgents Is Nothing Then
                    SubAgents = New NexusProvider.SubAgentCollection
                End If
                If SubAgents.FindItemByKey(oNewSubagent.PartyKey) Is Nothing Then
                    SubAgents.Add(oNewSubagent)
                End If
                Session.Item(CNSubAgents) = SubAgents
            End If

            BindSubAgentData()

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Assign nagivate URL along with Client type to differentiate corporate / personal client.
            'Also pass client id of the update panel as this allows for a partial postback
            If HttpContext.Current.Session.IsCookieless Then
                hypSubAgent.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindAgent.aspx?PostbackTo=" & PnlSubAgent.ClientID.ToString & "&AgentType=SubAgent&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypSubAgent.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/FindAgent.aspx?PostbackTo=" & PnlSubAgent.ClientID.ToString & "&AgentType=SubAgent&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            If CType(Session(CNMode), Mode) <> Mode.View Then
                'If Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.MTAQuote Or Session(CNQuoteMode) = QuoteMode.ReQuote Then
                hypSubAgent.Visible = True
            Else
                hypSubAgent.Visible = False
            End If
        End Sub

    End Class

End Namespace
