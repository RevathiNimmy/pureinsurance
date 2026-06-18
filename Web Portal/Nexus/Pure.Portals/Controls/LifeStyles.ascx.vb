Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_LifeStyles
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
        Protected Sub BindLifestyleData()
            'Need to Retreive the Data from Session
            RetreiveData()

            Dim Lifestyle As NexusProvider.LifestyleCollection = oParty.Lifestyle
            drgLifestyle.DataSource = Lifestyle
            drgLifestyle.DataBind()
        End Sub

        Protected Sub drgLifestyle_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgLifestyle.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If CType(Session(CNClientMode), Mode) = Mode.Add Or CType(Session(CNClientMode), Mode) = Mode.Edit Then
                    'Finding the Edit link in the gridview to assign the navigate URL along with LifeStyleID.
                    Dim hypEdit As LinkButton = e.Row.Cells(8).FindControl("hypLifestyleEdit")
                    If HttpContext.Current.Session.IsCookieless Then
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Lifestyle.aspx?PostbackTo=" & PnlLifestyle.ClientID.ToString & "&LifestyleID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/Lifestyle.aspx?PostbackTo=" & PnlLifestyle.ClientID.ToString & "&LifestyleID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If
                    Dim hypDelete As LinkButton = e.Row.Cells(8).FindControl("hypLifestyleDelete")
                    hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.Lifestyle).Key

                    'NOTE - this will need to be changed to give each row a unique id
                    'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                    e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Lifestyle).Key)
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                If Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then
                    drgLifestyle.Columns(8).Visible = False
                Else
                    drgLifestyle.Columns(8).Visible = True
                End If
            End If
        End Sub

        Protected Sub drgLifestyle_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles drgLifestyle.RowCommand
            If e.CommandName = "DeleteRow" Then
                'Need to Retreive the Data from Session
                RetreiveData()

                Dim iCount As Integer
                Dim oColl As NexusProvider.LifestyleCollection = oParty.Lifestyle
                For iCount = 0 To oColl.Count - 1
                    If oColl(iCount).Key.Trim = e.CommandArgument.ToString.Trim Then
                        oParty.Lifestyle.Remove(iCount)
                        Exit For
                    End If
                Next
                Session(CNParty) = oParty
                BindLifestyleData()
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Catlin Performance Fix
            If Not IsPostBack AndAlso Me.Visible = True Then
                BindLifestyleData()
            End If

            If Request("__EVENTARGUMENT") = "UpdateLifeStyle" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sLifeStyleData() As String = txtLifeStyleData.Value.Split(";")

                'Need to Retreive the Data from Session
                RetreiveData()

                If sLifeStyleData(0).ToUpper = "ADD" Then

                    Dim oNewLifeStyle As New NexusProvider.Lifestyle

                    With oNewLifeStyle
                        .Name = sLifeStyleData(1)

                        If Not String.IsNullOrEmpty(sLifeStyleData(2).Trim()) And sLifeStyleData(2).Trim() <> System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper() Then ' If Condition Added  on 19th Jan 2009.
                            .DateOfBirth = CType(sLifeStyleData(2).Trim(), Date)
                            .DateOfBirthSpecified = True
                        End If

                        If String.IsNullOrEmpty(sLifeStyleData(3)) Then
                            .CategoryCode = "UNDEFINED"
                        Else
                            .CategoryCode = sLifeStyleData(3)
                        End If

                        If Not String.IsNullOrEmpty(sLifeStyleData(4)) Then
                            .GenderCode = sLifeStyleData(4)
                            .GenderCodeSpecified = True
                        Else
                            .GenderCodeSpecified = False
                        End If

                        .OccupationCode = sLifeStyleData(5)
                        .SecOccupationCode = sLifeStyleData(6)
                        .Smoker = sLifeStyleData(7)
                       
                    End With

                    If Not String.IsNullOrEmpty(sLifeStyleData(1)) Then
                        oParty.Lifestyle.Add(oNewLifeStyle)
                    End If

                    Session(CNParty) = oParty

                ElseIf sLifeStyleData(0).ToUpper = "UPDATE" Then
                    Dim LifeStyle As NexusProvider.LifestyleCollection = oParty.Lifestyle
                    Dim oUpdateLifeStyle As NexusProvider.Lifestyle = oParty.Lifestyle.Item(CType(sLifeStyleData(8), Integer))

                    With oUpdateLifeStyle
                        .Name = sLifeStyleData(1)

                        If Not String.IsNullOrEmpty(sLifeStyleData(2).Trim()) Then
                            .DateOfBirth = CType(sLifeStyleData(2).Trim(), Date)
                            .DateOfBirthSpecified = True
                        Else
                            .DateOfBirth = Nothing
                            .DateOfBirthSpecified = False
                        End If

                        If String.IsNullOrEmpty(sLifeStyleData(3)) Then
                            .CategoryCode = "UNDEFINED"
                        Else
                            .CategoryCode = sLifeStyleData(3)
                        End If

                        If Not String.IsNullOrEmpty(sLifeStyleData(4)) Then
                            .GenderCode = sLifeStyleData(4)
                            .GenderCodeSpecified = True
                        Else
                            .GenderCodeSpecified = False
                        End If

                        .OccupationCode = sLifeStyleData(5)
                        .SecOccupationCode = sLifeStyleData(6)
                        .Smoker = sLifeStyleData(7)

                    End With

                    LifeStyle.Update(oUpdateLifeStyle)

                    Session(CNParty) = oParty

                End If
                BindLifestyleData()
            End If

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Assign nagivate URL along with Client type to differentiate corporate / personal client.
            If HttpContext.Current.Session.IsCookieless Then
                hypLifestyle.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Lifestyle.aspx?PostbackTo=" & PnlLifestyle.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypLifestyle.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/Lifestyle.aspx?PostbackTo=" & PnlLifestyle.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            If CType(Session(CNClientMode), Mode) = Mode.Edit Then
                're-binding Lifestyle in edit mode.
                BindLifestyleData()
                hypLifestyle.Visible = True
            ElseIf CType(Session(CNClientMode), Mode) = Mode.View Then
                BindLifestyleData()
                hypLifestyle.Visible = False
            End If
        End Sub

    End Class
End Namespace
