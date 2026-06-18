Imports Nexus.Utils
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_Associates
        Inherits System.Web.UI.UserControl
        Dim oParty As NexusProvider.BaseParty = Nothing
        Public sColAssociateKeys As String
        ''' <summary>
        ''' binds the Associate collection from the Session.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindAssociateData()
            'Need to Retreive the Data from Session
            RetreiveData()

            Dim Associate As NexusProvider.AssociateCollection = oParty.Associate

            sColAssociateKeys = String.Empty
            If Associate.Count > 0 Then
                For i As Integer = 0 To Associate.Count - 1
                    sColAssociateKeys = sColAssociateKeys & "-" & Associate.Item(i).AssociateCode.ToString().Trim()
                Next
            End If

            drgAssociate.DataSource = Associate
            drgAssociate.DataBind()

        End Sub

        ''' <summary>
        ''' Associate DataBound event for corporate / personal client
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgAssociate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgAssociate.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oAssociates As NexusProvider.Associate
                oAssociates = CType(e.Row.DataItem, NexusProvider.Associate)
                If CType(Session(CNClientMode), Mode) = Mode.Add Or CType(Session(CNClientMode), Mode) = Mode.Edit Then
                    'Finding the Edit link in the gridview to assign the navigate URL along with AssociateID. 
                    Dim hypEdit As LinkButton = e.Row.Cells(5).FindControl("hypAssociateEdit")

                    If HttpContext.Current.Session.IsCookieless Then
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindClient.aspx?ClientCode=" & Convert.ToString(Request.QueryString("Code")) & "&PostbackTo=" & PnlAssociate.ClientID.ToString & "&AssociateKeys=" & sColAssociateKeys & "&AssociateID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/FindClient.aspx?ClientCode=" & Convert.ToString(Request.QueryString("Code")) & "&PostbackTo=" & PnlAssociate.ClientID.ToString & "&AssociateKeys=" & sColAssociateKeys & "&AssociateID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If

                    Dim hypDelete As LinkButton = e.Row.Cells(6).FindControl("hypAssociateDelete")
                    hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.Associate).Key
                    'NOTE - this will need to be changed to give each row a unique id
                    'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                    e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Associate).Key)
                End If
                If Not String.IsNullOrEmpty(oAssociates.CurrencyCode) Then
                    e.Row.Cells(3).Text = New Money(oAssociates.AccountBalance, oAssociates.CurrencyCode).Formatted 'AccountBalance
                    e.Row.Cells(4).Text = New Money(oAssociates.ClaimIncurred, oAssociates.CurrencyCode).Formatted 'ClaimIncurred
                Else
                    e.Row.Cells(3).Text = ""
                    e.Row.Cells(4).Text = ""
                End If

            ElseIf e.Row.RowType = DataControlRowType.Header Then
                If Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then
                    drgAssociate.Columns(6).Visible = False
                Else
                    drgAssociate.Columns(6).Visible = True
                End If
            End If
        End Sub

        Protected Sub drgAssociate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles drgAssociate.RowCommand
            If e.CommandName = "DeleteRow" Then
                'Need to Retreive the Data from Session
                RetreiveData()

                Dim iCount As Integer
                Dim oColl As NexusProvider.AssociateCollection = oParty.Associate
                For iCount = 0 To oColl.Count - 1
                    If oColl(iCount).Key.Trim = e.CommandArgument.ToString.Trim Then
                        oParty.Associate.Remove(iCount)
                        Exit For
                    End If
                Next
                Session(CNParty) = oParty
                BindAssociateData()
            End If
        End Sub
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
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Catlin Performance Fix
            If Not IsPostBack AndAlso Me.Visible = True Then
                BindAssociateData()
            End If

            If Request("__EVENTARGUMENT") = "UpdateAssociate" Then

                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sAssociateData() As String = txtAssociateData.Value.Split(";")

                'Need to Retreive the Data from Session
                RetreiveData()

                If sAssociateData(0).ToUpper = "ADD" Then

                    Dim oAssociate As New NexusProvider.Associate

                    oAssociate.AssociateCode = sAssociateData(1)
                    oAssociate.AssociateKey = sAssociateData(2)
                    oAssociate.AssociateName = sAssociateData(3)
                    oAssociate.ClientKey = sAssociateData(2)
                    oAssociate.RelationshipCode = sAssociateData(4)
                    oAssociate.RelationshipDescription = sAssociateData(5)

                    If Session(CNParty) IsNot Nothing Then
                        Dim oBaseParty As NexusProvider.BaseParty = Session(CNParty)

                        If oBaseParty.Associate.Count = 0 Then
                            oParty.Associate.Add(oAssociate)
                            Session(CNParty) = oParty
                        Else
                            Dim bAssociateCodeExist As Boolean = False
                            Dim sAssociateCode As String
                            For icount As Integer = 0 To oBaseParty.Associate.Count - 1
                                sAssociateCode = oBaseParty.Associate.Item(icount).AssociateCode
                                If oAssociate.AssociateCode = sAssociateCode Then
                                    bAssociateCodeExist = True
                                    Exit For
                                End If
                            Next
                            If Not bAssociateCodeExist Then
                                   oParty.Associate.Add(oAssociate)
                                   Session(CNParty) = oParty
                          End If
                        End If
                    End If

                ElseIf sAssociateData(0).ToUpper = "UPDATE" Then
                    Dim Associates As NexusProvider.AssociateCollection = oParty.Associate
                    Dim oUpdateAssociate As NexusProvider.Associate = oParty.Associate.Item(CType(sAssociateData(6), Integer))

                    oUpdateAssociate.AssociateCode = sAssociateData(1)
                    oUpdateAssociate.AssociateKey = sAssociateData(2)
                    oUpdateAssociate.AssociateName = sAssociateData(3)
                    oUpdateAssociate.ClientKey = sAssociateData(2)
                    oUpdateAssociate.RelationshipCode = sAssociateData(4)
                    oUpdateAssociate.RelationshipDescription = sAssociateData(5)
                    oUpdateAssociate.AccountBalance = 0
                    oUpdateAssociate.ClaimIncurred = 0
                    oUpdateAssociate.CurrencyCode = Nothing
                    Associates.Update(oUpdateAssociate)

                    Session(CNParty) = oParty
                End If
                BindAssociateData()
            End If

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            'Assign nagivate URL along with Client type to differentiate corporate / personal client.
            If HttpContext.Current.Session.IsCookieless Then
                hypAssociate.OnClientClick = "tb_show(null ,'" & AppSettings("webroot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindClient.aspx?ClientCode=" & Convert.ToString(Request.QueryString("Code")) & "&PostbackTo=" & PnlAssociate.ClientID.ToString & "&PartyKey=" & Convert.ToString(Request.QueryString("PartyKey")) & "&Associate=true&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypAssociate.OnClientClick = "tb_show(null ,'" & AppSettings("webroot") & "Modal/FindClient.aspx?ClientCode=" & Convert.ToString(Request.QueryString("Code")) & "&PostbackTo=" & PnlAssociate.ClientID.ToString & "&PartyKey=" & Convert.ToString(Request.QueryString("PartyKey")) & "&Associate=true&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            If CType(Session(CNClientMode), Mode) = Mode.Edit Then
                're-binding associate in edit mode.
                BindAssociateData()
                hypAssociate.Visible = True
            ElseIf CType(Session(CNClientMode), Mode) = Mode.View Then
                BindAssociateData()
                hypAssociate.Visible = False
            End If

        End Sub

    End Class
End Namespace