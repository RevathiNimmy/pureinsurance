Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_Convictions
        Inherits System.Web.UI.UserControl
        Dim oParty As NexusProvider.BaseParty = Nothing
        ''' <summary>
        '''binds the Conviction collection from the Session.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindConvictionData()
            'Need to Retreive the Data from Session
            RetreiveData()

            Dim Conviction As NexusProvider.ConvictionCollection = oParty.Conviction
            drgConviction.DataSource = Conviction
            drgConviction.DataBind()
        End Sub

        ''' <summary>
        ''' Conviction DataBound event for corporate / personal client
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgConviction_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgConviction.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If CType(Session(CNClientMode), Mode) = Mode.Add Or CType(Session(CNClientMode), Mode) = Mode.Edit Then
                    'Finding the Edit link in the gridview to assign the navigate URL along with ConvictionID.
                    Dim hypEdit As LinkButton = e.Row.Cells(7).FindControl("hypConvictionEdit")
                    If HttpContext.Current.Session.IsCookieless Then
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Conviction.aspx?PostbackTo=" & PnlConviction.ClientID.ToString & "&ConvictionID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/Conviction.aspx?PostbackTo=" & PnlConviction.ClientID.ToString & "&ConvictionID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If
                    Dim hypDelete As LinkButton = e.Row.Cells(7).FindControl("hypConvictionDelete")
                    hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.Convictions).Key

                    'NOTE - this will need to be changed to give each row a unique id
                    'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                    e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Convictions).Key)
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                If Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then
                    drgConviction.Columns(7).Visible = False
                Else
                    drgConviction.Columns(7).Visible = True
                End If
            End If
        End Sub

        ''' <summary>
        ''' Delete conviction
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgConviction_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles drgConviction.RowCommand
            If e.CommandName = "DeleteRow" Then
                'Need to Retreive the Data from Session
                RetreiveData()

                Dim iCount As Integer
                Dim oColl As NexusProvider.ConvictionCollection = oParty.Conviction
                For iCount = 0 To oColl.Count - 1
                    If oColl(iCount).Key.Trim = e.CommandArgument.ToString.Trim Then
                        oParty.Conviction.Remove(iCount)
                        Exit For
                    End If
                Next
                Session(CNParty) = oParty
                BindConvictionData()
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
                    Case Else
                        oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                End Select
            End If
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Catlin Performance Fix
            If Not IsPostBack AndAlso Me.Visible = True Then
                BindConvictionData()
            End If

            If Request("__EVENTARGUMENT") = "UpdateConviction" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sConvictionData() As String = txtConvictionData.Value.Split(";")

                'Need to Retreive the Data from Session
                RetreiveData()

                If sConvictionData(0).ToUpper = "ADD" Then

                    Dim oConviction As New NexusProvider.Convictions

                    With oConviction
                        .TypeCode = sConvictionData(1)
                        .FineAmount = IIf(sConvictionData(2) IsNot String.Empty, sConvictionData(2), 0.0)
                        .StatusCode = sConvictionData(3)
                        .Description = sConvictionData(4)
                        .ConvictionDate = CType(sConvictionData(5), Date)
                        .SentenceTypeCode = sConvictionData(6)
                        If Not String.IsNullOrEmpty(sConvictionData(7).Trim) And sConvictionData(7) <> System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper() Then ' If Condition Added  on 19th Jan 2009.
                            .SentenceEffectiveDate = CType(sConvictionData(7), Date)
                        End If
                        .SentenceDescription = sConvictionData(8)
                        .SentenceDuration = IIf(sConvictionData(9) IsNot String.Empty, sConvictionData(9), 0.0)
                        '   TimeUnit.Value = Need to write the property name after SAM changes with sConvictionData(10)
                        .SentenceDurationQualifier = sConvictionData(10).ToString().Trim


                        .AlcoholMeasurementMethod = sConvictionData(11)
                        .AlcoholLevel = IIf(sConvictionData(12) IsNot String.Empty, sConvictionData(12), 0.0)
                        .DrivingLicensePenaltyPoints = IIf(sConvictionData(13) IsNot String.Empty, sConvictionData(13), 0.0)
                    End With

                    oParty.Conviction.Add(oConviction)
                    Session(CNParty) = oParty

                ElseIf sConvictionData(0).ToUpper = "UPDATE" Then
                    Dim Conviction As NexusProvider.ConvictionCollection = oParty.Conviction
                    Dim oUpdateConviction As NexusProvider.Convictions = oParty.Conviction.Item(CType(sConvictionData(14), Integer))

                    With oUpdateConviction
                        .TypeCode = sConvictionData(1)
                        .FineAmount = IIf(sConvictionData(2) IsNot String.Empty, sConvictionData(2), 0.0)
                        .StatusCode = sConvictionData(3)
                        .Description = sConvictionData(4)
                        .ConvictionDate = CType(sConvictionData(5), Date)
                        .SentenceTypeCode = sConvictionData(6)
                        If Not String.IsNullOrEmpty(sConvictionData(7).Trim) And sConvictionData(7) <> System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper() Then ' If Condition Added  on 19th Jan 2009.
                            .SentenceEffectiveDate = CType(sConvictionData(7), Date)
                        End If
                        .SentenceDescription = sConvictionData(8)
                        .SentenceDuration = IIf(sConvictionData(9) IsNot String.Empty, sConvictionData(9), 0.0)
                        '   TimeUnit.Value = Need to write the property name after SAM changes with sConvictionData(10)
                        .SentenceDurationQualifier = sConvictionData(10).ToString.Trim


                        .AlcoholMeasurementMethod = sConvictionData(11)
                        .AlcoholLevel = IIf(sConvictionData(12) IsNot String.Empty, sConvictionData(12), 0.0)
                        .DrivingLicensePenaltyPoints = IIf(sConvictionData(13) IsNot String.Empty, sConvictionData(13), 0.0)
                    End With

                    Conviction.Update(oUpdateConviction)
                    Session(CNParty) = oParty

                End If
                BindConvictionData()
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Assign nagivate URL along with Client type to differentiate corporate / personal client.
            If HttpContext.Current.Session.IsCookieless Then
                hypConviction.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Conviction.aspx?PostbackTo=" & PnlConviction.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypConviction.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/Conviction.aspx?PostbackTo=" & PnlConviction.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            If CType(Session(CNClientMode), Mode) = Mode.Edit Then
                BindConvictionData()
                hypConviction.Visible = True
            ElseIf CType(Session(CNClientMode), Mode) = Mode.View Then
                BindConvictionData()
                hypConviction.Visible = False
            End If
        End Sub

    End Class
End Namespace
