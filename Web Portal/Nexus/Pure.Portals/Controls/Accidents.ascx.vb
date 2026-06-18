Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_Accidents
        Inherits System.Web.UI.UserControl

        Dim oParty As NexusProvider.BaseParty = Nothing

        ''' <summary>
        '''Binds the Accident collection from the Session.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindAccidentsData()
            'Need to Retreive the Data from Session
            RetreiveData()

            Dim oAccidents As NexusProvider.AccidentCollection = oParty.Accidents
            drgAccidents.DataSource = oAccidents
            drgAccidents.DataBind()
        End Sub

        ''' <summary>
        ''' Accident DataBound event for corporate / personal client
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgAccident_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgAccidents.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If CType(Session(CNClientMode), Mode) = Mode.Add Or CType(Session(CNClientMode), Mode) = Mode.Edit Then
                    'Finding the Edit link in the gridview to assign the navigate URL along with AccidentID.
                    Dim hypEdit As LinkButton = e.Row.Cells(4).FindControl("hypAccidentEdit")
                    If HttpContext.Current.Session.IsCookieless Then
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Accident.aspx?PostbackTo=" & PnlAccident.ClientID.ToString & "&AccidentID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/Accident.aspx?PostbackTo=" & PnlAccident.ClientID.ToString & "&AccidentID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If
                    Dim hypDelete As LinkButton = e.Row.Cells(4).FindControl("hypAccidentDelete")
                    hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.Accident).AccidentKey

                    'NOTE - this will need to be changed to give each row a unique id
                    'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                    e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Accident).AccidentKey)
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                If Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then
                    drgAccidents.Columns(4).Visible = False
                Else
                    drgAccidents.Columns(4).Visible = True
                End If
            End If
        End Sub

        ''' <summary>
        ''' Delete Accident
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgAccident_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles drgAccidents.RowCommand
            If e.CommandName = "DeleteRow" Then
                'Need to Retreive the Data from Session
                RetreiveData()

                Dim iCount As Integer
                Dim oColl As NexusProvider.AccidentCollection = oParty.Accidents
                For iCount = 0 To oColl.Count - 1
                    If oColl(iCount).AccidentKey.ToString().Trim = e.CommandArgument.ToString().Trim Then
                        oParty.Accidents.Remove(iCount)
                        Exit For
                    End If
                Next
                Session(CNParty) = oParty
                BindAccidentsData()
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
                    Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                        oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                End Select
            End If
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Catlin Performance Fix
            If Not IsPostBack AndAlso Me.Visible = True Then
                BindAccidentsData()
            End If

            If Request("__EVENTARGUMENT") = "UpdateAccident" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sAccidentData As String() = txtAccidentData.Value.Split(";")

                'Need to Retreive the Data from Session
                RetreiveData()

                If sAccidentData(0).ToUpper = "ADD" Then

                    Dim oAccident As New NexusProvider.Accident

                    With oAccident
                        .AccidentDate = CType(sAccidentData(1), Date)
                        .Description = sAccidentData(2)
                        .IsAtFault = CType(sAccidentData(3), Boolean)
                    End With

                    oParty.Accidents.Add(oAccident)
                    Session(CNParty) = oParty

                ElseIf sAccidentData(0).ToUpper = "UPDATE" Then
                    Dim Accident As NexusProvider.AccidentCollection = oParty.Accidents
                    Dim oUpdateAccident As NexusProvider.Accident = oParty.Accidents.Item(CType(sAccidentData(4), Integer))

                    With oUpdateAccident
                        .AccidentKey = sAccidentData(4)
                        .AccidentDate = CType(sAccidentData(1), Date)
                        .Description = sAccidentData(2)
                        .IsAtFault = CType(sAccidentData(3), Boolean)
                    End With

                    Accident.Update(oUpdateAccident)
                    Session(CNParty) = oParty

                End If
                BindAccidentsData()
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Assign nagivate URL along with Client type to differentiate corporate / personal client.
            If HttpContext.Current.Session.IsCookieless Then
                hypAccidentAdd.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Accident.aspx?PostbackTo=" & PnlAccident.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypAccidentAdd.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/Accident.aspx?PostbackTo=" & PnlAccident.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            If CType(Session(CNClientMode), Mode) = Mode.Edit Then
                BindAccidentsData()
                hypAccidentAdd.Visible = True
            ElseIf CType(Session(CNClientMode), Mode) = Mode.View Then
                BindAccidentsData()
                hypAccidentAdd.Visible = False
            End If
        End Sub

    End Class
End Namespace
