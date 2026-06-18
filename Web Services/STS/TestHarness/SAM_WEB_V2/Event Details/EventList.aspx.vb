Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Imports System.Data.SqlClient

Partial Class Event_Details_EventList
    Inherits System.Web.UI.Page


    Dim arr As New ArrayList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2


        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        'set up the proxy object
        If Not Page.IsPostBack Then


            BuildLists(oSAM, ddlEventType, STSListType.PMLookup, "event_type_group", "")
            ddlEventType.Items.Insert(0, New ListItem("All", "-1"))
            'ddlEventType.SelectedValue = "All"

            '' BuildLists(oSAM, ddlUserName, STSListType.Missing, "PMUser")
        End If
        
        If Not Session("PolicyNumber") Is Nothing Then
            txtPolicyCode.Text = Session("PolicyNumber")
        End If

        Dim oGetEventDetailsRequest As New GetEventDetailsRequestType
        Dim oGetEventDetailsResponse As New GetEventDetailsResponseType

        With oGetEventDetailsRequest
            .PartyKey = Session("PARTYKEY")
            .AccountKeySpecified = False
            .BaseCaseKeySpecified = False
            .BaseClaimKey = False
            .BranchCode = "HeadOff"
            .CaseKeySpecified = False
            If txtClaimCode.Text <> "" Then
                .ClaimKey = hfClaimKey.Value
                .ClaimKeySpecified = True
            Else
                .ClaimKeySpecified = False
            End If
            If txtPolicyCode.Text <> "" Then
                .InsuranceFileKey = hfinsurancekey.Value
                .InsuranceFileKeySpecified = True
            Else
                .InsuranceFileKeySpecified = False
            End If


            If txtToDate.Text <> "" Then
                .DateTo = txtToDate.Text
                .DateToSpecified = True
            Else
                .DateToSpecified = False
            End If

            If txtFromDate.Text <> "" Then
                .FromDate = txtFromDate.Text
                .FromDateSpecified = True
            Else
                .FromDateSpecified = False
            End If

            'If Not Session("InsuranceFileKey") Is Nothing Then
            '    .InsuranceFileKey = Session("InsuranceFileKey")
            '    .InsuranceFileKeySpecified = True
            'ElseIf txtPolicyCode.Text <> "" And hfinsurancekey.Value <> "" Then
            '    .InsuranceFileKey = Convert.ToInt32(hfinsurancekey.Value)
            '    .InsuranceFileKeySpecified = True
            'Else
            '    .InsuranceFileKeySpecified = False

            'End If
            'If (Not txtPolicyCode.Text.Equals(String.Empty)) Then
            '    .InsuranceFileKey = Convert.ToInt32(hfinsurancekey.Value)
            '    .InsuranceFileKeySpecified = True
            '    'Session("AgentKey") = Convert.ToInt32(hfinsurancekey.Value)
            'End If
            If Not Session("InsuranceFolderKey") Is Nothing Then
                .InsuranceFolderKey = Session("InsuranceFolderKey")
                .InsuranceFolderKeySpecified = True
            Else
                .InsuranceFolderKeySpecified = False
            End If
            Dim oreq As New AddEventRequestType



            Try
                oGetEventDetailsResponse = oSAM.GetEventDetails(oGetEventDetailsRequest)
                With oGetEventDetailsResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        Session("Events") = .EventDetails
                        gvEventList.DataSource = .EventDetails
                        gvEventList.DataBind()

                    End If

                End With

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)

            Finally
                'clean up any objects here
            End Try




        End With


    End Sub
    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = ESTSLookup
        oRequest.ListCode = ListCode


        Try
            oResponse = oSAM.GetList(oRequest)

            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "Key"
                    If (BindValue = "") Then
                        objControl.Items.Insert(0, New ListItem("All", "All"))
                    Else
                        objControl.SelectedValue = BindValue
                    End If
                    objControl.DataBind()

                End If
                If ListCode = "source" Then
                    Session("Branch") = oResponse.List
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try

    End Sub

    Protected Sub ddlEventType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEventType.SelectedIndexChanged

        Dim oEventDetails() As BaseGetEventDetailsResponseTypeRow
        oEventDetails = CType(Session("Events"), BaseGetEventDetailsResponseTypeRow()) 'DirectCast(Session("Events"), BaseGetEventDetailsResponseTypeRow())
        gvEventList.DataSource = oEventDetails
        gvEventList.DataBind()
    End Sub

    Protected Sub gvEventList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEventList.RowDataBound


        Dim oDataItem As New BaseGetEventDetailsResponseTypeRow

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            oDataItem = DirectCast(e.Row.DataItem, BaseGetEventDetailsResponseTypeRow)
            ' oDataItem = e.Row.DataItem
            If ddlEventType.SelectedItem.Text = "All" Then

                If oDataItem.UserName = ddlUserName.SelectedItem.Text Or ddlUserName.SelectedItem.Text = "All" Then
                    e.Row.Visible = True
                Else
                    e.Row.Visible = False
                End If
                If oDataItem.EventType = "Notes - Customer" Or oDataItem.EventType = "Notes - Customer Warning" Then
                    arr.Add(oDataItem.Description)
                    oDataItem.Description = "Note:" + oDataItem.EventDescription
                Else

                End If

                e.Row.DataItem = oDataItem
                e.Row.DataBind()
            Else
                oDataItem = DirectCast(e.Row.DataItem, BaseGetEventDetailsResponseTypeRow)
                If oDataItem.TypeKey = Int32.Parse(ddlEventType.SelectedValue) Then
                    If oDataItem.UserName = ddlUserName.SelectedItem.Text Or ddlUserName.SelectedItem.Text = "All" Then
                        e.Row.Visible = True
                    Else
                        e.Row.Visible = False
                    End If
                Else
                    e.Row.Visible = False
                End If
            End If
            Dim lbl As New Label
            lbl = DirectCast(e.Row.Cells(8).FindControl("Status"), Label)
            If oDataItem.EventType = "Notes - Customer" Then
                lbl.Visible = False
            ElseIf oDataItem.EventType = "Notes - Customer Warning" Then
                If oDataItem.StatusKey = 1 Then
                    lbl.Visible = True
                    lbl.Text = "Completed"
                Else
                    lbl.Visible = True
                    lbl.Text = "Outstanding"
                End If
            Else
                lbl.Visible = False
            End If
        End If

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("AddEventNote.aspx")
    End Sub



    Protected Sub gvEventList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvEventList.SelectedIndexChanged

        Session("Context") = DirectCast(Session("Events"), BaseGetEventDetailsResponseTypeRow())(gvEventList.SelectedIndex).EventType
        Session("subject") = DirectCast(Session("Events"), BaseGetEventDetailsResponseTypeRow())(gvEventList.SelectedIndex).EventDescription
        Session("EventDate") = DirectCast(Session("Events"), BaseGetEventDetailsResponseTypeRow())(gvEventList.SelectedIndex).EventDate
        Session("Username") = DirectCast(Session("Events"), BaseGetEventDetailsResponseTypeRow())(gvEventList.SelectedIndex).UserName
        If arr IsNot Nothing And gvEventList.SelectedRow.RowIndex < arr.Count Then
            Session("Text") = arr.Item(gvEventList.SelectedRow.RowIndex)
        End If


        Session("EVENTKEY") = DirectCast(Session("Events"), BaseGetEventDetailsResponseTypeRow())(gvEventList.SelectedIndex).EventKey
        If (DirectCast(Session("Events"), BaseGetEventDetailsResponseTypeRow())(gvEventList.SelectedIndex).EventType = "Notes - Customer") Or (DirectCast(Session("Events"), BaseGetEventDetailsResponseTypeRow())(gvEventList.SelectedIndex).EventType = "Notes - Customer Warning") Then
            BtnViewnote.Enabled = True
        Else
            BtnViewnote.Enabled = False
        End If
    End Sub

    Protected Sub btnEventNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEventNotes.Click

    End Sub


    Protected Sub ddlUserName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserName.SelectedIndexChanged
        Dim oEventDetails() As BaseGetEventDetailsResponseTypeRow
        oEventDetails = DirectCast(Session("Events"), BaseGetEventDetailsResponseTypeRow())
        gvEventList.DataSource = oEventDetails
        gvEventList.DataBind()
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Response.Redirect("ClientManager.aspx")
    End Sub

    Protected Sub BtnViewnote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnViewnote.Click
        Response.Redirect("GetEventNote.aspx")
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Response.Redirect("ClientManager.aspx")
    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2


        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        If Not Session("PolicyNumber") Is Nothing Then
            txtPolicyCode.Text = Session("PolicyNumber")
        End If

        Dim oGetEventDetailsRequest As New GetEventDetailsRequestType
        Dim oGetEventDetailsResponse As New GetEventDetailsResponseType

        With oGetEventDetailsRequest
            .PartyKey = Session("PARTYKEY")
            .AccountKeySpecified = False
            .BaseCaseKeySpecified = False
            .BaseClaimKey = False
            .BranchCode = "HeadOff"
            .CaseKeySpecified = False
            If txtClaimCode.Text <> "" Then
                .ClaimKey = hfClaimKey.Value
                .ClaimKeySpecified = True
            Else
                .ClaimKeySpecified = False
            End If

            If txtPolicyCode.Text <> "" Then
                .InsuranceFileKey = Convert.ToInt32(hfinsurancekey.Value)
                .InsuranceFileKeySpecified = True
            Else
                .InsuranceFileKeySpecified = False
            End If


            If txtToDate.Text <> "" Then
                .DateTo = txtToDate.Text
                .DateToSpecified = True
            Else
                .DateToSpecified = False
            End If

            If txtFromDate.Text <> "" Then
                .FromDate = txtFromDate.Text
                .FromDateSpecified = True
            Else
                .FromDateSpecified = False
            End If

            'If Session("InsuranceFileKey") Is Nothing And hfinsurancekey.Value <> "" And txtPolicyCode.Text <> "" Then
            '    .InsuranceFileKey = Convert.ToInt32(hfinsurancekey.Value)
            '    .InsuranceFileKeySpecified = True
            'ElseIf Not (Session("InsuranceFileKey") Is Nothing) Then
            '    .InsuranceFileKey = Session("InsuranceFileKey")
            '    .InsuranceFileKeySpecified = False
            'Else

            'End If

            'If (Not txtPolicyCode.Text.Equals(String.Empty)) Then
            '    .InsuranceFileKey = Convert.ToInt32(hfinsurancekey.Value)
            '    .InsuranceFileKeySpecified = True
            '    'Session("AgentKey") = Convert.ToInt32(hfinsurancekey.Value)
            'End If
            If Not Session("InsuranceFolderKey") Is Nothing Then
                .InsuranceFolderKey = Session("InsuranceFolderKey")
                .InsuranceFolderKeySpecified = True
            Else
                .InsuranceFolderKeySpecified = False
            End If
            Dim oreq As New AddEventRequestType



            Try
                oGetEventDetailsResponse = oSAM.GetEventDetails(oGetEventDetailsRequest)
                With oGetEventDetailsResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        Session("Events") = .EventDetails
                        gvEventList.DataSource = .EventDetails
                        gvEventList.DataBind()

                    End If

                End With

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)

            Finally
                'clean up any objects here
            End Try
        End With
    End Sub

  

End Class
