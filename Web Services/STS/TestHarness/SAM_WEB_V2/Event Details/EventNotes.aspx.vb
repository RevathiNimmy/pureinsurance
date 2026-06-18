Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class Event_Details_EventNotes

    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        If Not Page.IsPostBack Then
            Dim oGetEventNoteRequest As New GetEventNoteRequestType
            Dim oGetEventNoteResponse As New GetEventNoteResponseType

            With oGetEventNoteRequest
                .BranchCode = "HeadOff"
                .EventKey = Session("EVENTKEY")
            End With
            Try
                oGetEventNoteResponse = oSAM.GetEventNote(oGetEventNoteRequest)
                With oGetEventNoteResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        lstEventText.DataSource = .EventNotes
                        lstEventText.DataTextField = "EventText"
                        lstEventText.DataBind()
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

        End If

    End Sub

    Protected Sub btnAddText_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddText.Click

        lstEventText.Visible = False
        txtEventText.Visible = True

    End Sub

    Protected Sub lstEventText_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstEventText.SelectedIndexChanged

    End Sub

    Protected Sub btnAddEventNote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddEventNote.Click
        If txtEventText.Text <> "" Then
            Dim oAddEventNoteRequest As New AddEventNoteRequestType
            Dim oAddEventNoteResponse As New AddEventNoteResponseType

            With oAddEventNoteRequest
                .BranchCode = "HeadOff"
                .EventKey = Session("EVENTKEY")
                .EventText = txtEventText.Text
            End With


            Try
                oAddEventNoteResponse = oSAM.AddEventNote(oAddEventNoteRequest)
                With oAddEventNoteResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)

                    Else

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
        End If

    End Sub

    Protected Sub Btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btncancel.Click
        Response.Redirect("EventList.aspx")
    End Sub
End Class
