Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Event_Details_AddEventNote
    Inherits System.Web.UI.Page

    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        txtEventDate.Text = Date.Now
        txtUserName.Text = "sirius"
        If Not Page.IsPostBack Then
            BuildLists(oSAM, ddlSubject, STSListType.PMLookup, "Event_Log_Subject")
        End If
        lblpriority.Visible = False
        lblstatus.Visible = False
        ddlstatus.Visible = False
        ddlpriority.Visible = False


    End Sub

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String)
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

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim oAddEventRequest As New AddEventRequestType
        Dim oAddEventResponse As New AddEventResponseType

        With oAddEventRequest
            .BranchCode = "HeadOff"
            .EventDate = txtEventDate.Text
            .PartyKey = Session("PartyKey")
            .RtfText = txtText.Text
            .UserName = txtUserName.Text
            .EventLogSubjectKey = Int32.Parse(ddlSubject.SelectedValue)
            .EventTypeKey = Int32.Parse(ddlContext.SelectedValue)
            If ddlContext.SelectedValue = 37 Then
                .Priority = ddlpriority.SelectedValue
                .StatusKey = ddlstatus.SelectedValue
            End If

            
        End With

        Try
            oAddEventResponse = oSAM.AddEvent(oAddEventRequest)
            With oAddEventResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    Response.Redirect("EventList.aspx")
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

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Response.Redirect("EventList.aspx")
    End Sub

    Protected Sub ddlContext_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlContext.SelectedIndexChanged
        If ddlContext.SelectedValue = 37 Then
            lblpriority.Visible = True
            lblstatus.Visible = True
            ddlstatus.Visible = True
            ddlpriority.Visible = True
        Else
            lblpriority.Visible = False
            lblstatus.Visible = False
            ddlstatus.Visible = False
            ddlpriority.Visible = False
        End If
    End Sub
End Class
