Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_Contacts
        Inherits System.Web.UI.UserControl
        Dim oParty As NexusProvider.BaseParty = Nothing
        ''' <summary>
        ''' Finds the contact gridview and binds the Contact collection from the Session.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindContactData()
            'Need to Retreive the Data from Session
            RetreiveData()

            Dim Contact As NexusProvider.ContactCollection = oParty.Contacts
            drgContact.DataSource = Contact
            drgContact.DataBind()
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
        ''' <summary>
        ''' Contact DataBound event for corporate / personal client
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgContact_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgContact.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If CType(Session(CNClientMode), Mode) = Mode.Add Or CType(Session(CNClientMode), Mode) = Mode.Edit Then
                    'Finding the Edit link in the gridview to assign the navigate URL along with ContactID. 
                    Dim hypEdit As LinkButton = e.Row.Cells(6).FindControl("hypContactEdit")
                    hypEdit.Visible = True

                    If HttpContext.Current.Session.IsCookieless Then
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("webroot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Contact.aspx?PostbackTo=" & PnlContact.ClientID.ToString.Trim() & "&ContactID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("webroot") & "Modal/Contact.aspx?PostbackTo=" & PnlContact.ClientID.ToString.Trim() & "&ContactID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If

                    Dim hypDelete As LinkButton = e.Row.Cells(6).FindControl("hypContactDelete")
                    hypDelete.Visible = True
                    hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.Contact).Key

                    'NOTE - this will need to be changed to give each row a unique id
                    'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                    e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Contact).Key)

                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                If Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then
                    drgContact.Columns(6).Visible = False
                Else
                    drgContact.Columns(6).Visible = True
                End If
            End If
        End Sub

        ''' <summary>
        ''' Delete contact
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgContact_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles drgContact.RowCommand
            If e.CommandName = "DeleteRow" Then
                'Need to Retreive the Data from Session
                RetreiveData()

                Dim iCount As Integer
                Dim iDeletedIndex As Integer = 0
                Dim oColl As NexusProvider.ContactCollection = oParty.Contacts
                For iCount = 0 To oColl.Count - 1
                    If oColl(iCount).Key.Trim = e.CommandArgument.ToString.Trim Then
                        iDeletedIndex = iCount
                        oParty.Contacts.Remove(iCount)
                        Exit For
                    End If
                Next
                For iCount = iDeletedIndex To oColl.Count - 1
                    oParty.Contacts(iCount).Key = oParty.Contacts(iCount).Key - 1
                Next

                Session(CNParty) = oParty
                BindContactData()
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Catlin Performance Fix
            Dim strPartyType As String = ""
            If Not IsPostBack Then
                FillPreferredCorrespondence()
                BindContactData()
                CheckPartyType(oParty, strPartyType)
                If Not String.IsNullOrEmpty(strPartyType) Then
                    Select Case strPartyType
                        Case "PC"
                            If DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.CorrespondenceCode = "" Then
                                ddlPreferredCorrespondenceType.SelectedValue = "LETTER"
                            Else
                                ddlPreferredCorrespondenceType.SelectedValue = DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.CorrespondenceCode
                            End If

                        Case "CC"
                            If DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.CorrespondenceCode = "" Then
                                ddlPreferredCorrespondenceType.SelectedValue = "LETTER"
                            Else
                                ddlPreferredCorrespondenceType.SelectedValue = DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.CorrespondenceCode
                            End If
                    End Select
                End If
                If Not String.IsNullOrEmpty(strPartyType) AndAlso strPartyType = "OT" Then
                    pnlAdditionalInfo.Visible = False
                End If
            End If


            If Request("__EVENTARGUMENT") = "UpdateContact" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sContactData() As String = txtContactData.Value.Split(";")

                'Need to Retreive the Data from Session
                RetreiveData()

                If sContactData(0).ToUpper = "ADD" Then

                    Dim oNewContact As New NexusProvider.Contact
                    Dim sContactType As String = sContactData(1)

                    If (sContactType IsNot Nothing) Then
                        Select Case sContactType.ToUpper
                            Case "E-MAIL" '" 'NexusProvider.ContactType.Email
                                oNewContact.ContactType = NexusProvider.ContactType.Email
                                Exit Select
                            Case "FAX" 'NexusProvider.ContactType.Fax
                                oNewContact.ContactType = NexusProvider.ContactType.Fax
                                Exit Select
                            Case "HOMEPHONE" 'NexusProvider.ContactType.HomePhone
                                oNewContact.ContactType = NexusProvider.ContactType.HomePhone
                                Exit Select
                            Case "MAIN" 'NexusProvider.ContactType.Main
                                oNewContact.ContactType = NexusProvider.ContactType.Main
                                Exit Select
                            Case "MOBILE" 'NexusProvider.ContactType.Mobile
                                oNewContact.ContactType = NexusProvider.ContactType.Mobile
                                Exit Select
                            Case "WEB" 'NexusProvider.ContactType.Web
                                oNewContact.ContactType = NexusProvider.ContactType.Web
                                Exit Select
                            Case "LETTER"
                                oNewContact.ContactType = NexusProvider.ContactType.Letter
                                Exit Select
                            Case "MEMAIL"
                                oNewContact.ContactType = NexusProvider.ContactType.MEMAIL
                                Exit Select
                            Case "TELEPHONE"
                                oNewContact.ContactType = NexusProvider.ContactType.Telephone
                                Exit Select
                            Case "OTHER"
                                oNewContact.ContactType = NexusProvider.ContactType.Other
                                Exit Select
                            Case "WORKPHONE"
                                oNewContact.ContactType = NexusProvider.ContactType.WorkPhone
                            Case "MORENAME"
                                oNewContact.ContactType = NexusProvider.ContactType.MoreName
                                Exit Select
                            Case Else
                                oNewContact.ContactType = NexusProvider.ContactType.Letter
                                Exit Select

                        End Select
                    End If


                    With oNewContact
                        Select Case oNewContact.ContactType
                            Case NexusProvider.ContactType.Email
                                .ContactDetailType = NexusProvider.ItemChoiceTypes.EmailAddress
                            Case Else
                                .ContactDetailType = NexusProvider.ItemChoiceTypes.Number
                        End Select
                        .Description = sContactData(2)
                        .AreaCode = sContactData(3)
                        .Number = sContactData(4)
                        .Extension = sContactData(5)
                        If sContactType.ToUpper() = "OTHER" Then
                            .OtherContactTypeCode = sContactData(1)
                        Else
                            .OtherContactTypeCode = sContactData(7)
                        End If

                        .ContactTypeDescription = sContactData(7)
                    End With

                    oParty.Contacts.Add(oNewContact)

                    CheckPartyType(oParty, strPartyType)
                    If strPartyType = "PC" Then
                        DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.CorrespondenceCode = ddlPreferredCorrespondenceType.SelectedValue
                    ElseIf strPartyType = "CC" Then
                        DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.CorrespondenceCode = ddlPreferredCorrespondenceType.SelectedValue
                    End If
                    Session(CNParty) = oParty

                ElseIf sContactData(0).ToUpper = "UPDATE" Then
                    Dim oUpdateContactCollection As NexusProvider.ContactCollection = oParty.Contacts
                    Dim oUpdateContacts As NexusProvider.Contact = oParty.Contacts.Item(CType(sContactData(6), Integer))
                    Dim sContactType As String = sContactData(1)

                    If (sContactType IsNot Nothing) Then
                        Select Case sContactType.ToUpper
                            Case "E-MAIL" '" 'NexusProvider.ContactType.Email
                                oUpdateContacts.ContactType = NexusProvider.ContactType.Email
                                Exit Select
                            Case "FAX" 'NexusProvider.ContactType.Fax
                                oUpdateContacts.ContactType = NexusProvider.ContactType.Fax
                                Exit Select
                            Case "HOMEPHONE" 'NexusProvider.ContactType.HomePhone
                                oUpdateContacts.ContactType = NexusProvider.ContactType.HomePhone
                                Exit Select
                            Case "MAIN" 'NexusProvider.ContactType.Main
                                oUpdateContacts.ContactType = NexusProvider.ContactType.Main
                                Exit Select
                            Case "MOBILE" 'NexusProvider.ContactType.Mobile
                                oUpdateContacts.ContactType = NexusProvider.ContactType.Mobile
                                Exit Select
                            Case "WEB" 'NexusProvider.ContactType.Web
                                oUpdateContacts.ContactType = NexusProvider.ContactType.Web
                                Exit Select
                            Case "LETTER"
                                oUpdateContacts.ContactType = NexusProvider.ContactType.Letter
                                Exit Select
                            Case "MEMAIL"
                                oUpdateContacts.ContactType = NexusProvider.ContactType.MEMAIL
                                Exit Select
                            Case "OTHER"
                                oUpdateContacts.ContactType = NexusProvider.ContactType.Other
                                Exit Select
                            Case "TELEPHONE"
                                oUpdateContacts.ContactType = NexusProvider.ContactType.Telephone
                                Exit Select
                            Case "WORKPHONE" 'NexusProvider.ContactType.WORKPHONE
                                oUpdateContacts.ContactType = NexusProvider.ContactType.WORKPHONE
                            Case "MORENAME"
                                oUpdateContacts.ContactType = NexusProvider.ContactType.MoreName
                                Exit Select
                            Case Else
                                oUpdateContacts.ContactType = NexusProvider.ContactType.Letter
                                Exit Select

                        End Select
                    End If
                    With oUpdateContacts
                        Select Case oUpdateContacts.ContactType
                            Case NexusProvider.ContactType.Email
                                .ContactDetailType = NexusProvider.ItemChoiceTypes.EmailAddress
                            Case Else
                                .ContactDetailType = NexusProvider.ItemChoiceTypes.Number
                        End Select
                        .Description = sContactData(2)
                        .AreaCode = sContactData(3)
                        .Number = sContactData(4)
                        .Extension = sContactData(5)
                        If sContactType.ToUpper() = "OTHER" Then
                            .OtherContactTypeCode = sContactData(1)
                        Else
                            .OtherContactTypeCode = sContactData(7)
                        End If
                        .ContactTypeDescription = sContactData(7)
                    End With

                    oUpdateContactCollection.Update(oUpdateContacts)

                    CheckPartyType(oParty, strPartyType)
                    If strPartyType = "PC" Then
                        DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.CorrespondenceCode = ddlPreferredCorrespondenceType.SelectedValue
                    ElseIf strPartyType = "CC" Then
                        DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.CorrespondenceCode = ddlPreferredCorrespondenceType.SelectedValue
                    End If
                    Session(CNParty) = oParty
                End If
                BindContactData()
            Else
                If oParty IsNot Nothing Then
                    CheckPartyType(oParty, strPartyType)
                    If strPartyType = "PC" Then
                        DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.CorrespondenceCode = ddlPreferredCorrespondenceType.SelectedValue
                    ElseIf strPartyType = "CC" Then
                        DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.CorrespondenceCode = ddlPreferredCorrespondenceType.SelectedValue
                    End If
                    Session(CNParty) = oParty
                    BindContactData()
                End If
            End If

        End Sub

        Public Sub FillPreferredCorrespondence()


            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oLookUP As New NexusProvider.LookupListCollection
            oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Contact_Type", True, False, "is_correspondence_type", 1)

            'Populating the client and agent preferred correspondence control with the retreived values
            If oLookUP IsNot Nothing Then
                'existing items cleared
                If ddlPreferredCorrespondenceType IsNot Nothing Then
                    ddlPreferredCorrespondenceType.Items.Clear()

                    For iCount As Integer = 0 To oLookUP.Count - 1
                        Dim lstPreferredCorrespondence As New ListItem
                        lstPreferredCorrespondence.Text = oLookUP(iCount).Description
                        lstPreferredCorrespondence.Value = Trim(oLookUP(iCount).Code)
                        ddlPreferredCorrespondenceType.Items.Add(lstPreferredCorrespondence)
                        ddlPreferredCorrespondenceType.DataBind()
                    Next
                End If
                ddlPreferredCorrespondenceType.SelectedValue = "LETTER"
            End If
        End Sub


        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Assign nagivate URL along with Client type to differentiate corporate / personal client.

            If HttpContext.Current.Session.IsCookieless Then
                hypContact.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Contact.aspx?PostbackTo=" & PnlContact.ClientID.ToString.Trim() & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypContact.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/Contact.aspx?PostbackTo=" & PnlContact.ClientID.ToString.Trim() & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            If CType(Session(CNClientMode), Mode) = Mode.Edit Then
                're-binding contact in edit mode.
                BindContactData()
                hypContact.Visible = True
                ddlPreferredCorrespondenceType.Enabled = True
            ElseIf CType(Session(CNClientMode), Mode) = Mode.View Then
                BindContactData()
                hypContact.Visible = False
                ddlPreferredCorrespondenceType.Enabled = False
            End If
        End Sub

        Protected Sub ddlPreferredCorrespondenceType_SelectedIndexChange(sender As Object, e As EventArgs) Handles ddlPreferredCorrespondenceType.SelectedIndexChanged
            Dim strPartyType As String = ""
            RetreiveData()
            CheckPartyType(oParty, strPartyType)
            If oParty IsNot Nothing AndAlso Not String.IsNullOrEmpty(strPartyType) Then
                Select Case strPartyType
                    Case "PC"
                        DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.CorrespondenceCode = ddlPreferredCorrespondenceType.SelectedValue
                    Case "CC"
                        DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.CorrespondenceCode = ddlPreferredCorrespondenceType.SelectedValue
                End Select
            End If

        End Sub

        Public Sub CheckPartyType(ByVal oParty As NexusProvider.BaseParty, ByRef strPartyType As String)
            Select Case True
                Case TypeOf oParty Is NexusProvider.CorporateParty
                    strPartyType = "CC"
                Case TypeOf oParty Is NexusProvider.PersonalParty
                    strPartyType = "PC"
                Case TypeOf oParty Is NexusProvider.OtherParty
                    strPartyType = "OT"
            End Select
        End Sub
    End Class

End Namespace