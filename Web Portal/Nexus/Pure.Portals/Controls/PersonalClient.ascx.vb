Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Globalization

Namespace Nexus

    Partial Class Controls_PersonalClient : Inherits System.Web.UI.UserControl
        Private sPCMandatoryFields As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).PCMandatoryFields

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Not String.IsNullOrEmpty(sPCMandatoryFields) Then
                If sPCMandatoryFields.Contains("DOB") Then
                    'lblDateOfBirthRequired.Visible = True
                    txtDOB.CssClass = txtDOB.CssClass & " field-mandatory"
                    vldDateOfBirthRequired.Enabled = True
                    vldDateOfBirthRequired.InitialValue = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                End If

                If sPCMandatoryFields.Contains("Email") Then
                    'lblEmailAddressRequired.Visible = True
                    txtEmail.CssClass = txtEmail.CssClass & " field-mandatory"
                    vldEmailRequired.Enabled = True
                    vldEmailRegex.Enabled = True
                    'lblConfirmEmailAddressRequired.Visible = True
                    txtConfirmEmail.CssClass = txtConfirmEmail.CssClass & " field-mandatory"
                    vldConfirmEmailRequired.Enabled = True
                    vldEmailCompare.Enabled = True
                    vldConfirmEmailRegex.Enabled = True
                Else
                    'For Direct Customer
                    If Session(CNLoginType) Is Nothing Or CType(Session(CNLoginType), LoginType) = LoginType.Customer Then
                        'lblEmailAddressRequired.Visible = True
                        txtEmail.CssClass = txtEmail.CssClass & " field-mandatory"
                        vldEmailRequired.Enabled = True
                        vldEmailRegex.Enabled = True
                        'lblConfirmEmailAddressRequired.Visible = True
                        txtConfirmEmail.CssClass = txtConfirmEmail.CssClass & " field-mandatory"
                        vldConfirmEmailRequired.Enabled = True
                        vldEmailCompare.Enabled = True
                        vldConfirmEmailRegex.Enabled = True
                    End If
                End If

                If sPCMandatoryFields.Contains("Tel") Then
                    'lblTelephoneRequired.Visible = True
                    txtTelephone.CssClass = txtTelephone.CssClass & " field-mandatory"
                    vldTelephoneRequired.Enabled = True
                End If
            Else

                'For Direct Customer
                If Session(CNLoginType) Is Nothing Or CType(Session(CNLoginType), LoginType) = LoginType.Customer Then
                    'lblEmailAddressRequired.Visible = True
                    txtEmail.CssClass = txtEmail.CssClass & " field-mandatory"
                    vldEmailRequired.Enabled = True
                    vldEmailRegex.Enabled = True
                    'lblConfirmEmailAddressRequired.Visible = True
                    txtConfirmEmail.CssClass = txtConfirmEmail.CssClass & " field-mandatory"
                    vldConfirmEmailRequired.Enabled = True
                    vldEmailCompare.Enabled = True
                    vldConfirmEmailRegex.Enabled = True
                End If
            End If

            If Not IsPostBack Then
                txtDOB.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
            End If
            'show the File Code textbox only if EnableFileCodeSearch=true in web.config <portal> specific
            liFileCode.Visible = CBool(CType(WebConfigurationManager.GetSection("NexusFrameWork"), _
                Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID).AllowFileCodeField)
        End Sub

        Public Sub SetFocus()
            'To set Focus on ddTitle Control
            ddlTitle.Focus()
        End Sub

        Public WriteOnly Property RegisterClient() As Boolean
            Set(ByVal value As Boolean)
                pnlConfirmEmail.Visible = value
            End Set
        End Property

        Public Sub UpdateParty(ByRef v_oParty As NexusProvider.PersonalParty)

            Dim oAddress As NexusProvider.Address

            If v_oParty Is Nothing Then

                'New Client
                v_oParty = New NexusProvider.PersonalParty()
            Else

                'Existing Client
                oAddress = v_oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                'Remove old address from the party object as well add the new one later
                'No address keys in the object so no need to worry about duplicate addresses
                v_oParty.Addresses.Remove(oAddress)

            End If

            With v_oParty

                oAddress = PersonalCtrlAddress.Address
                oAddress.AddressType = NexusProvider.AddressType.CorrespondenceAddress
                .Addresses.Add(oAddress)
                'PN 42242
                Dim oContact As NexusProvider.Contact = .Contacts(NexusProvider.ContactType.HomePhone)
                If oContact Is Nothing Then
                    If Not (String.IsNullOrEmpty(txtTelephone.Text.Trim())) Then
                        .Contacts.Add(New NexusProvider.Contact(NexusProvider.ContactType.HomePhone, txtTelephone.Text.Trim()))
                    End If
                Else
                    .Contacts.Remove(oContact)
                    oContact.AreaCode = String.Empty
                    oContact.Number = txtTelephone.Text.Trim()
                    .Contacts.Add(oContact)
                End If

                oContact = .Contacts(NexusProvider.ContactType.Email)
                If oContact Is Nothing Then
                    If Not (String.IsNullOrEmpty(txtEmail.Text.Trim())) Then
                        .Contacts.Add(New NexusProvider.Contact(NexusProvider.ContactType.Email, txtEmail.Text.Trim()))
                    End If
                Else
                    .Contacts.Remove(oContact)
                    oContact.Number = txtEmail.Text.Trim()
                    .Contacts.Add(oContact)
                End If

                .Title = ddlTitle.Value
                .Initials = Left(txtFirstName.Text, 1)
                .Forename = txtFirstName.Text.Trim()
                '.Surname = txtSurname.Text.Trim()
                .Lastname = txtSurname.Text.Trim()
                .FileCode = txtFileCode.Text
                If Not String.IsNullOrEmpty(txtDOB.Text) Then
                    If Not (txtDOB.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()) Then
                        .DateOfBirth = CType(txtDOB.Text, Date)
                    End If
                End If
            End With

        End Sub

        Public WriteOnly Property Party() As NexusProvider.PersonalParty
            Set(ByVal value As NexusProvider.PersonalParty)

                If value Is Nothing Then

                    ddlTitle.Value = String.Empty
                    txtFirstName.Text = String.Empty
                    txtSurname.Text = String.Empty
                    txtDOB.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                    txtEmail.Text = String.Empty
                    txtConfirmEmail.Text = String.Empty
                    txtTelephone.Text = String.Empty
                    PersonalCtrlAddress.Address = Nothing
                    txtFileCode.Text = String.Empty
                Else

                    With value
                        ddlTitle.Value = .Title
                        txtFirstName.Text = .Forename
                        'txtSurname.Text = .Surname
                        txtSurname.Text = .Lastname
                        txtFileCode.Text = .FileCode

                        If .DateOfBirthSpecified Then
                            If Not .DateOfBirth.ToShortDateString.Contains("1899") Then
                                txtDOB.Text = .DateOfBirth.ToShortDateString
                            End If

                        End If

                       

                        'Get Email Address
                        Dim oContact As NexusProvider.Contact = .Contacts.Item(NexusProvider.ContactType.Email)
                        If oContact IsNot Nothing Then
                            txtEmail.Text = oContact.Number
                            txtConfirmEmail.Text = oContact.Number
                        End If

                        'Get Main Contact Number
                        'PN 42242
                        oContact = .Contacts.Item(NexusProvider.ContactType.HomePhone)
                        If oContact IsNot Nothing Then
                            txtTelephone.Text = oContact.AreaCode & oContact.Number
                        End If

                        'Get Correspondence Address
                        PersonalCtrlAddress.Address = .Addresses.Item(NexusProvider.AddressType.CorrespondenceAddress)

                    End With

                End If

            End Set
        End Property

        Public ReadOnly Property Title() As String
            Get
                Return ddlTitle.Value
            End Get
        End Property

        Public ReadOnly Property FirstName() As String
            Get
                Return txtFirstName.Text.Trim()
            End Get
        End Property

        Public ReadOnly Property Surname() As String
            Get
                Return txtSurname.Text.Trim()
            End Get
        End Property

        Public ReadOnly Property Telephone() As String
            Get
                Return txtTelephone.Text
            End Get
        End Property

        Public ReadOnly Property Email() As String
            Get
                Return txtEmail.Text.Trim()
            End Get
        End Property

        Public ReadOnly Property Address1() As String
            Get
                Return PersonalCtrlAddress.Address1
            End Get
        End Property

        Public ReadOnly Property Address2() As String
            Get
                Return PersonalCtrlAddress.Address2
            End Get
        End Property

        Public ReadOnly Property Address3() As String
            Get
                Return PersonalCtrlAddress.Address3
            End Get
        End Property

        Public ReadOnly Property Address4() As String
            Get
                Return PersonalCtrlAddress.Address4
            End Get
        End Property

        Public ReadOnly Property PostCode() As String
            Get
                Return PersonalCtrlAddress.Postcode
            End Get
        End Property

        Public ReadOnly Property Country() As String
            Get
                Return PersonalCtrlAddress.Country
            End Get
        End Property

        Protected Sub custrngvldDateOfBirth_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custrngvldDateOfBirth.ServerValidate
            If txtDOB.Text.Trim.Length <> 0 And txtDOB.Text.Trim.ToUpper <> System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper Then

                Dim dateValue As DateTime
                If Not (Date.TryParseExact(txtDOB.Text.Trim, "dd/MM/yyyy", New CultureInfo("en-US"), DateTimeStyles.None, dateValue)) Then
                    args.IsValid = False
                    custrngvldDateOfBirth.ErrorMessage = GetLocalResourceObject("lbl_InvalidDOBFormat")
                    txtDOB.Focus()
                Else
                    Dim dDateofBirth As Date = CDate(txtDOB.Text)
                    If dDateofBirth > Date.Today Or dDateofBirth <= Date.Today.AddYears(-120) Then
                        args.IsValid = False
                        custrngvldDateOfBirth.ErrorMessage = GetLocalResourceObject("lbl_InvalidDOB")
                        txtDOB.Focus()
                    End If

                End If
            End If

        End Sub

    End Class

End Namespace