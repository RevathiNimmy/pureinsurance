Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_ProspectPolicy
        Inherits System.Web.UI.UserControl
        Dim oParty As NexusProvider.BaseParty = Nothing

        ''' <summary>
        ''' Retreive the Party data from Session
        ''' </summary>
        ''' <remarks></remarks>
        Sub RetreiveData()
            'Need to Retreive the Data from Session
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                End Select
            End If
        End Sub
        Protected Sub BindProspectPolicyData()
            'Need to Retreive the Data from Session
            RetreiveData()

            Dim ProspectPolicy As NexusProvider.ProspectPolicyCollection = oParty.ProspectPolicy
            drgProspectPolicy.DataSource = ProspectPolicy
            drgProspectPolicy.DataBind()
        End Sub

        Protected Sub drgProspectPolicy_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgProspectPolicy.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If CType(Session(CNClientMode), Mode) = Mode.Add Or CType(Session(CNClientMode), Mode) = Mode.Edit Then
                    'Finding the Edit link in the gridview to assign the navigate URL along with PropectPolicyID.
                    Dim hypEdit As LinkButton = e.Row.Cells(5).FindControl("hypProspectPolicyEdit")
                    If HttpContext.Current.Session.IsCookieless Then
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/ProspectPolicies.aspx?PostbackTo=" & PnlProspectPolicy.ClientID.ToString & "&ProspectPolicyID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/ProspectPolicies.aspx?PostbackTo=" & PnlProspectPolicy.ClientID.ToString & "&ProspectPolicyID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If
                    Dim hypDelete As LinkButton = e.Row.Cells(5).FindControl("hypProspectPolicyDelete")
                    hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.ProspectPolicies).Key

                    'NOTE - this will need to be changed to give each row a unique id
                    'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                    e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.ProspectPolicies).Key)
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                If Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then
                    drgProspectPolicy.Columns(5).Visible = False
                Else
                    drgProspectPolicy.Columns(5).Visible = True
                End If
            End If
        End Sub

        Protected Sub drgProspectPolicy_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles drgProspectPolicy.RowCommand
            If e.CommandName = "DeleteRow" Then
                'Need to Retreive the Data from Session
                RetreiveData()

                Dim iCount As Integer
                Dim oColl As NexusProvider.ProspectPolicyCollection = oParty.ProspectPolicy
                For iCount = 0 To oColl.Count - 1
                    If oColl(iCount).Key.Trim = e.CommandArgument.ToString.Trim Then
                        oParty.ProspectPolicy.Remove(iCount)
                        Exit For
                    End If
                Next
                Session(CNParty) = oParty
                BindProspectPolicyData()
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Catlin Performance Fix
            If Not IsPostBack AndAlso Me.Visible = True Then
                BindProspectPolicyData()
            End If

            If Request("__EVENTARGUMENT") = "UpdatesProspectPolicy" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sProspectPolicyData() As String = txtProspectPolicyData.Value.Split(";")

                'Need to Retreive the Data from Session
                RetreiveData()

                If sProspectPolicyData(0).ToUpper = "ADD" Then

                    Dim oNewPolicy As New NexusProvider.ProspectPolicies
                    With oNewPolicy
                        .ProspectTypeCode = sProspectPolicyData(1)
                        .RenewalDate = CDate(sProspectPolicyData(2))
                        .TimesQuoted = IIf(String.IsNullOrEmpty(sProspectPolicyData(3)) = False, sProspectPolicyData(3), 0.0)
                        .TargetPremium = IIf(String.IsNullOrEmpty(sProspectPolicyData(4)) = False, sProspectPolicyData(4), 0.0)
                    End With

                    oParty.ProspectPolicy.Add(oNewPolicy)
                    Session(CNParty) = oParty

                ElseIf sProspectPolicyData(0).ToUpper = "UPDATE" Then

                    Dim ProspectPolicy As NexusProvider.ProspectPolicyCollection = oParty.ProspectPolicy
                    Dim oUpdatePolicy As NexusProvider.ProspectPolicies = oParty.ProspectPolicy.Item(sProspectPolicyData(5))
                    With oUpdatePolicy
                        .ProspectTypeCode = sProspectPolicyData(1)
                        .RenewalDate = CDate(sProspectPolicyData(2))
                        .TimesQuoted = IIf(String.IsNullOrEmpty(sProspectPolicyData(3)) = False, sProspectPolicyData(3), 0.0)
                        .TargetPremium = IIf(String.IsNullOrEmpty(sProspectPolicyData(4)) = False, sProspectPolicyData(4), 0.0)
                    End With

                    ProspectPolicy.Update(oUpdatePolicy)
                    Session(CNParty) = oParty

                End If
                BindProspectPolicyData()
            End If

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                hypProspectPolicy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/ProspectPolicies.aspx?PostbackTo=" & PnlProspectPolicy.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypProspectPolicy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/ProspectPolicies.aspx?PostbackTo=" & PnlProspectPolicy.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            If CType(Session(CNClientMode), Mode) = Mode.Edit Then
                're-binding prospectpolicy in edit mode.
                BindProspectPolicyData()
                hypProspectPolicy.Visible = True
            ElseIf CType(Session(CNClientMode), Mode) = Mode.View Then
                BindProspectPolicyData()
                hypProspectPolicy.Visible = False
            End If
        End Sub

    End Class
End Namespace