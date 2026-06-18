Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
Partial Class Controls_Loyalty
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
        Protected Sub BindLoyaltyData()
            'Need to Retreive the Data from Session
            RetreiveData()

            Dim Loyalty As NexusProvider.LoyaltyCollection = oParty.Loyalty
            drgLoyalty.DataSource = Loyalty
            drgLoyalty.DataBind()
        End Sub

        Protected Sub drgLoyalty_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgLoyalty.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If CType(Session(CNClientMode), Mode) = Mode.Add Or CType(Session(CNClientMode), Mode) = Mode.Edit Then
                    'Finding the Edit link in the gridview to assign the navigate URL along with LoyaltyID.
                    Dim hypEdit As LinkButton = e.Row.Cells(8).FindControl("hypLoyaltyEdit")

                    If HttpContext.Current.Session.IsCookieless Then
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Loyalty.aspx?PostbackTo=" & PnlLoyalty.ClientID.ToString & "&LoyaltyID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        hypEdit.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/Loyalty.aspx?PostbackTo=" & PnlLoyalty.ClientID.ToString & "&LoyaltyID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If

                    Dim hypDelete As LinkButton = e.Row.Cells(8).FindControl("hypLoyaltyDelete")
                    hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.Loyalty).Key

                    'NOTE - this will need to be changed to give each row a unique id
                    'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                    e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Loyalty).Key)

                End If
                    'To Restrict the invalid date 
                    If CType(e.Row.DataItem, NexusProvider.Loyalty).EndDate.ToShortDateString.Trim = "01/01/0001" Then
                        e.Row.Cells(4).Text = ""
                    End If
                ElseIf e.Row.RowType = DataControlRowType.Header Then
                    If Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then
                        drgLoyalty.Columns(8).Visible = False
                    Else
                        drgLoyalty.Columns(8).Visible = True
                    End If
                End If
        End Sub

        Protected Sub drgLoyalty_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles drgLoyalty.RowCommand
            If e.CommandName = "DeleteRow" Then
                'Need to Retreive the Data from Session
                RetreiveData()

                Dim iCount As Integer
                Dim oColl As NexusProvider.LoyaltyCollection = oParty.Loyalty
                For iCount = 0 To oColl.Count - 1
                    If oColl(iCount).Key.Trim = e.CommandArgument.ToString.Trim Then
                        oParty.Loyalty.Remove(iCount)
                        Exit For
                    End If
                Next
                Session(CNParty) = oParty
                BindLoyaltyData()
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Catlin Performance Fix
            If Not IsPostBack AndAlso Me.Visible = True Then
                BindLoyaltyData()
            End If

            If Request("__EVENTARGUMENT") = "UpdatesLoyalty" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sLoyaltyData() As String = txtLoyaltyData.Value.Split(";")

                'Need to Retreive the Data from Session
                RetreiveData()

                If sLoyaltyData(0).ToUpper = "ADD" Then

                    Dim oLoyalty As New NexusProvider.Loyalty

                    With oLoyalty
                        .LoyaltySchemeCode = sLoyaltyData(1)
                        .MembershipNumber = sLoyaltyData(2)
                        .OtherReference = sLoyaltyData(3)
                        .StartDate = CDate(sLoyaltyData(4))
                        If sLoyaltyData(5).Trim.Length <> 0 Then
                            .EndDate = CDate(sLoyaltyData(5))
                        Else
                            .EndDate = Nothing
                        End If

                        .MainMember = sLoyaltyData(6)
                        .Active = sLoyaltyData(7)
                    End With

                    oParty.Loyalty.Add(oLoyalty)

                    Session(CNParty) = oParty

                ElseIf sLoyaltyData(0).ToUpper = "UPDATE" Then

                    Dim Loyalty As NexusProvider.LoyaltyCollection = oParty.Loyalty
                    Dim oUpdateLoyalty As NexusProvider.Loyalty = oParty.Loyalty.Item(CType(sLoyaltyData(8), Integer))

                    With oUpdateLoyalty
                        .LoyaltySchemeCode = sLoyaltyData(1)
                        .MembershipNumber = sLoyaltyData(2)
                        .OtherReference = sLoyaltyData(3)
                        .StartDate = CDate(sLoyaltyData(4))
                        If sLoyaltyData(5).Trim.Length <> 0 Then
                            .EndDate = CDate(sLoyaltyData(5))
                        Else
                            .EndDate = Nothing
                        End If

                        .MainMember = sLoyaltyData(6)
                        .Active = sLoyaltyData(7)
                    End With

                    Loyalty.Update(oUpdateLoyalty)
                    Session(CNParty) = oParty

                End If
                BindLoyaltyData()
            End If

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Assign nagivate URL along with Client type to differentiate corporate / personal client.

            If HttpContext.Current.Session.IsCookieless Then
                hypLoyalty.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Loyalty.aspx?PostbackTo=" & PnlLoyalty.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypLoyalty.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/Loyalty.aspx?PostbackTo=" & PnlLoyalty.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If


            If CType(Session(CNClientMode), Mode) = Mode.Edit Then
                're-binding loyalty in edit mode.
                BindLoyaltyData()
                hypLoyalty.Visible = True
            ElseIf CType(Session(CNClientMode), Mode) = Mode.View Then
                BindLoyaltyData()
                hypLoyalty.Visible = False
            End If
        End Sub

End Class
End Namespace