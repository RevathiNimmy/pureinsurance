Imports System
Imports System.Exception
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Linq

Namespace Nexus

    Partial Class Modal_EventDetails
        Inherits System.Web.UI.Page

        Dim oWebService As NexusProvider.ProviderBase
        Dim iEventkey As Integer
        Dim sDocumentType As String = "Production Of Document"
        Dim sExternalDocumentType As String = "External Document Stored"
        Dim sEMailType As String = "Email Sent"

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(txtNewEventNotes)

            If Request("EventDetailID") <> String.Empty Then
                ' obtaining data from the main page
                Dim oEventCollection As NexusProvider.EventDetailsCollection = CType(Session.Item(CNEvent), NexusProvider.EventDetailsCollection)
                Dim oEventSelected = (From obj In oEventCollection Where obj.EventKey = CType(Request("EventDetailID"), Integer)
                                      Select obj).ToList()
                If oEventSelected.Count > 0 Then
                    Dim oEvent As NexusProvider.EventDetails = CType(oEventSelected(0), NexusProvider.EventDetails)
                    Dim oSPFileList As New NexusProvider.SharepointFileList
                    Dim PartyShortName As String = String.Empty
                    If String.IsNullOrEmpty(PartyShortName) Then
                        If Session(CNParty) IsNot Nothing Then
                            PartyShortName = CType(Session(CNParty), NexusProvider.BaseParty).UserName
                        End If
                    End If

                    With oEvent
                        ' assigning values to the controls
                        lblContextData.Text = .EventType
                        lblSubjectData.Text = .EventDescription
                        lblDetailsData.Text = .Description
                        If .Priority IsNot Nothing And .Priority <> "" Then
                            lblPriorityData.Text = .Priority
                            If .StatusKey = 0 Then
                                lblStatusData.Text = If(GetLocalResourceObject("lstItemTextOutstanding") IsNot Nothing, GetLocalResourceObject("lstItemTextOutstanding"), "Outstanding")
                            ElseIf .StatusKey = 1 Then
                                lblStatusData.Text = If(GetLocalResourceObject("lstItemTextCompleted") IsNot Nothing, GetLocalResourceObject("lstItemTextCompleted"), "Completed")
                            End If
                        Else
                            lblPriorityData.Visible = False
                            lblPriority.Visible = False
                            lblStatusData.Visible = False
                            lblStatus.Visible = False
                        End If
                        iEventkey = .EventKey
                        
                        If .EventType.ToString.ToUpper = sExternalDocumentType.ToString.ToUpper Or _
                            .EventType.ToString.ToUpper = sEMailType.ToString.ToUpper Or _
                            .EventType.ToString.ToUpper = sDocumentType.ToString.ToUpper Then
                            hypDoc.Visible = True

                            Dim oDocumentArchiveSetting As NexusProvider.OptionTypeSetting
                            oWebService = New NexusProvider.ProviderManager().Provider
                            oDocumentArchiveSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 10)
                            If Not String.IsNullOrEmpty(.Document_Path) AndAlso oDocumentArchiveSetting.OptionValue = "2" Then
                                oWebService = New NexusProvider.ProviderManager().Provider
                                oSPFileList = oWebService.GetSharePointFileList(PartyShortName, Nothing, Nothing, Nothing, Nothing, False)
                                hypDoc.NavigateUrl = oSPFileList.FolderPath.FolderPath.Replace("General/", .Document_Path.Replace("\", "/"))
                            ElseIf Not String.IsNullOrEmpty(.DocumentKey) Then
                                hypDoc.NavigateUrl = "~/secure/document.aspx"
                                Session(CNDocumentRef) = .DocumentKey
                            Else
                                hypDoc.Visible = False
                            End If

                        End If

                    End With
                End If

            End If

            If Not IsPostBack Then
                ' call to populate the list with notes
                txtEventNotes.Attributes.Add("readonly", "readonly")
                PopulateNotesList()

            End If
        End Sub

        Protected Sub btnAddNote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNote.Click
            If Page.IsValid Then

                oWebService = New NexusProvider.ProviderManager().Provider
                Dim oEventDetails As New NexusProvider.EventDetails

                If Request("EventDetailID") <> String.Empty Then
                    Dim oEventCollection As NexusProvider.EventDetailsCollection = CType(Session.Item(CNEvent), NexusProvider.EventDetailsCollection)
                    Dim oEventSelected = (From obj In oEventCollection Where obj.EventKey = CType(Request("EventDetailID"), Integer)
                                          Select obj).ToList()
                    Dim oEvent As NexusProvider.EventDetails = CType(oEventSelected(0), NexusProvider.EventDetails)

                    iEventkey = oEvent.EventKey
                    oEvent = Nothing
                End If

                'Try
                oEventDetails.EventKey = iEventkey

                ' checks whether any data is entered in adding New Note
                If txtNewEventNotes.Text.Trim().Length > 0 Then
                    oEventDetails.EventText = txtNewEventNotes.Text.Trim()
                Else
                    oEventDetails.EventText = String.Empty
                End If

                ' call adding new event note
                oWebService.AddEventNote(oEventDetails)

                ' call to populate the list 
                PopulateNotesList()

                'add javascript to call script in parent page which will close modal dialog
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "');", True)

                'Catch ex As Exception

                ' Finally
                oEventDetails = Nothing
                oWebService = Nothing

                ' clearing the value in the New Event textbox
                txtNewEventNotes.Text = String.Empty
                ' End Try
            End If

        End Sub


        Protected Sub PopulateNotesList()

            Dim oEventDetailCollection As New NexusProvider.EventDetailsCollection
            Dim EventNoteStringBuilder As New StringBuilder
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim iCounter As Integer = 0

            'Try
            ' Calls SAm to obtain the notes
            oEventDetailCollection = oWebService.GetEventNote(iEventkey)

            'iterates the collection and adds the data in a string
            For Each oEventDetails As NexusProvider.EventDetails In oEventDetailCollection
                EventNoteStringBuilder.AppendLine(oEventDetails.EventText.Trim())
                iCounter += 1
                If iCounter Mod 2 = 0 Then
                    EventNoteStringBuilder.AppendLine()
                End If
            Next

            ' assigns the data from the string to textbox
            txtEventNotes.Text = EventNoteStringBuilder.ToString()

            'Catch ex As Exception


            'Finally
            oEventDetailCollection = Nothing
            oWebService = Nothing
            'End Try

        End Sub
    End Class

End Namespace
