Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session


Namespace Nexus

    Partial Class Controls_CorporateClient : Inherits System.Web.UI.UserControl
        Private sCCMandatoryFields As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).CCMandatoryFields

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Not String.IsNullOrEmpty(sCCMandatoryFields) Then
                If sCCMandatoryFields.Contains("Main") Then
                    'lblMainContactRequired.Visible = True
                    txtMainContact.CssClass = txtMainContact.CssClass & " field-mandatory"
                    vldMainContact.Enabled = True
                End If
                If sCCMandatoryFields.Contains("Email") Then
                    'lblEmailRequired.Visible = True
                    txtEmail.CssClass = txtEmail.CssClass & " field-mandatory"
                    vldEmailRequired.Enabled = True
                    vldEmailRegex.Enabled = True
                    lblEmailAddressConfirm.Visible = True
                    'lblConfirmEmailRequired.Visible = True
                    txtConfirmEmail.CssClass = txtConfirmEmail.CssClass & " field-mandatory"
                    vldConfirmEmailRequired.Enabled = True
                    vldEmailCompare.Enabled = True
                Else
                    'For Direct Customer
                    If Session(CNLoginType) Is Nothing Then
                        'lblEmailRequired.Visible = True
                        txtEmail.CssClass = txtEmail.CssClass & " field-mandatory"
                        vldEmailRequired.Enabled = True
                        lblEmailAddressConfirm.Visible = True
                        'lblConfirmEmailRequired.Visible = True
                        txtConfirmEmail.CssClass = txtConfirmEmail.CssClass & " field-mandatory"
                        vldConfirmEmailRequired.Enabled = True
                        vldEmailCompare.Enabled = True
                    End If
                    vldEmailRegex.Enabled = True
                End If
                If sCCMandatoryFields.Contains("Tel") Then
                    'lblTelRequired.Visible = True
                    txtTelephone.CssClass = txtTelephone.CssClass & " field-mandatory"
                    vldTelephoneRequired.Enabled = True
                    vldConfirmEmailRequired.Enabled = True
                End If
            Else
                'For Direct Customer
                If Session(CNLoginType) Is Nothing Or CType(Session(CNLoginType), LoginType) = LoginType.Customer Then
                    'lblEmailRequired.Visible = True
                    txtEmail.CssClass = txtEmail.CssClass & " field-mandatory"
                    vldEmailRequired.Enabled = True
                    lblEmailAddressConfirm.Visible = True
                    'lblConfirmEmailRequired.Visible = True
                    txtConfirmEmail.CssClass = txtConfirmEmail.CssClass & " field-mandatory"
                    vldConfirmEmailRequired.Enabled = True
                    vldEmailCompare.Enabled = True
                    vldEmailRegex.Enabled = True
                End If
            End If
            'show the File Code textbox only if EnableFileCodeSearch=true in web.config <portal> specific
            liFileCode.Visible = CBool(CType(WebConfigurationManager.GetSection("NexusFrameWork"), _
                Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID).AllowFileCodeField)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'check if contact details are required or not
        End Sub

        Public Sub SetFocus()
            'To set Focus on txtCompanyName Control
            txtCompanyName.Focus()
        End Sub
        Public WriteOnly Property RegisterClient() As Boolean
            Set(ByVal value As Boolean)
                pnlConfirmEmail.Visible = value
            End Set
        End Property

        Public Sub UpdateParty(ByRef v_oParty As NexusProvider.CorporateParty)

            Dim oAddress As NexusProvider.Address

            If v_oParty Is Nothing Then
                'New Client
                v_oParty = New NexusProvider.CorporateParty()
            Else
                'Existing Client
                oAddress = v_oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                'Remove old address from the party object as well add the new one later
                'No address keys in the object so no need to worry about duplicate addresses
                v_oParty.Addresses.Remove(oAddress)
            End If

            With v_oParty

                oAddress = CorpCtrlAddress.Address
                oAddress.AddressType = NexusProvider.AddressType.CorrespondenceAddress
                .Addresses.Add(oAddress)

                .CompanyName = txtCompanyName.Text
                .MainContact = txtMainContact.Text
                .TPUserCode = HttpContext.Current.User.Identity.Name
                .FileCode = txtFileCode.Text

                Dim oContact As NexusProvider.Contact = .Contacts(NexusProvider.ContactType.HomePhone)
                If oContact Is Nothing Then
                    'If not then we just want to remove the contact details
                    If Not String.IsNullOrEmpty(txtTelephone.Text.Trim()) Then
                        .Contacts.Add(New NexusProvider.Contact(NexusProvider.ContactType.HomePhone, txtTelephone.Text.Trim()))
                    End If
                Else
                    .Contacts.Remove(oContact)
                    'check if telephone number field has a value. 
                    'If not then we just want to remove the contact details
                    If Not String.IsNullOrEmpty(txtTelephone.Text.Trim()) Then
                        oContact.AreaCode = String.Empty
                        oContact.Number = txtTelephone.Text.Trim()
                        .Contacts.Add(oContact)
                    End If
                End If

                oContact = .Contacts(NexusProvider.ContactType.Email)
                If oContact Is Nothing Then
                    'check if email details were entered. do nothing if the field is blank
                    If Not String.IsNullOrEmpty(txtEmail.Text.Trim()) Then
                        .Contacts.Add(New NexusProvider.Contact(NexusProvider.ContactType.Email, txtEmail.Text.Trim()))
                    End If
                Else
                    .Contacts.Remove(oContact)
                    'check if email field has been a value. If not then we just want to remove the contact details
                    If Not String.IsNullOrEmpty(txtEmail.Text.Trim()) Then
                        oContact.Number = txtEmail.Text.Trim()
                        .Contacts.Add(oContact)
                    End If
                End If
            End With

        End Sub

        Public WriteOnly Property Party() As NexusProvider.CorporateParty
            Set(ByVal value As NexusProvider.CorporateParty)
                If value Is Nothing Then
                    txtCompanyName.Text = String.Empty
                    txtMainContact.Text = String.Empty
                    CorpCtrlAddress.Address = Nothing
                    txtEmail.Text = String.Empty
                    txtTelephone.Text = String.Empty
                    txtFileCode.Text = String.Empty
                Else
                    With value
                        txtCompanyName.Text = .CompanyName
                        txtMainContact.Text = .MainContact
                        txtFileCode.Text = .FileCode
                        'Get Correspondence Address
                        CorpCtrlAddress.Address = .Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                        'Get Email Address
                        Dim oContact As NexusProvider.Contact = .Contacts(NexusProvider.ContactType.Email)
                        If oContact IsNot Nothing Then
                            txtEmail.Text = oContact.Number
                            txtConfirmEmail.Text = oContact.Number
                        End If
                        'PN 42242
                        'Get Main Contact Number
                        oContact = .Contacts(NexusProvider.ContactType.HomePhone)
                        If oContact IsNot Nothing Then
                            txtTelephone.Text = oContact.AreaCode & oContact.Number
                        End If
                    End With
                End If
            End Set
        End Property

        Public ReadOnly Property CompanyName() As String
            Get
                Return txtCompanyName.Text
            End Get
        End Property

        Public ReadOnly Property MainContact() As String
            Get
                Return txtMainContact.Text
            End Get
        End Property

        Public ReadOnly Property Address1() As String
            Get
                Return CorpCtrlAddress.Address1
            End Get
        End Property

        Public ReadOnly Property Address2() As String
            Get
                Return CorpCtrlAddress.Address2
            End Get
        End Property

        Public ReadOnly Property Address3() As String
            Get
                Return CorpCtrlAddress.Address3
            End Get
        End Property

        Public ReadOnly Property Address4() As String
            Get
                Return CorpCtrlAddress.Address4
            End Get
        End Property

        Public ReadOnly Property PostCode() As String
            Get
                Return CorpCtrlAddress.Postcode
            End Get
        End Property

        Public ReadOnly Property Country() As String
            Get
                Return CorpCtrlAddress.Country
            End Get
        End Property

        Public ReadOnly Property Telephone() As String
            Get
                Return txtTelephone.Text
            End Get
        End Property

        Public ReadOnly Property Email() As String
            Get
                Return txtEmail.Text
            End Get
        End Property

    End Class

End Namespace