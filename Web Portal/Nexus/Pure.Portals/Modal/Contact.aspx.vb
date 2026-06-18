Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_Contact : Inherits System.Web.UI.Page

        Private sPCContactMandatoryFields As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).PCMandatoryFields
        Private sCCContactMandatoryFields As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).CCMandatoryFields
        Dim oParty As NexusProvider.BaseParty = Nothing
        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Public Property Contact() As NexusProvider.Contact
            Get
                Dim oContact As New NexusProvider.Contact(ContactType, Number)
                With oContact
                    .ContactType = ContactType
                    .AreaCode = AreaCode
                    .Number = Number
                    .Description = Description
                    .Extension = Extension
                End With
                Return oContact
            End Get
            Set(ByVal value As NexusProvider.Contact)
                If value Is Nothing Then
                    ContactType = String.Empty
                    AreaCode = String.Empty
                    Number = String.Empty
                    Description = String.Empty
                    Extension = String.Empty
                Else
                    With value
                        ContactType = .ContactType
                        AreaCode = .AreaCode
                        Number = .Number
                        Description = .Description
                        Extension = .Extension
                    End With
                End If
            End Set
        End Property

        Public Property ContactType() As String
            Get
                Return GISContactType.Value
            End Get
            Set(ByVal value As String)
                GISContactType.Value = value
            End Set
        End Property

        Public Property AreaCode() As String
            Get
                Return txtAreaCode.Text
            End Get
            Set(ByVal value As String)
                txtAreaCode.Text = value
            End Set
        End Property

        Public Property Number() As String
            Get
                Return txtNumber.Text
            End Get
            Set(ByVal value As String)
                txtNumber.Text = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return txtDescription.Text
            End Get
            Set(ByVal value As String)
                txtDescription.Text = value
            End Set
        End Property

        Public Property Extension() As String
            Get
                Return txtExtension.Text
            End Get
            Set(ByVal value As String)
                txtExtension.Text = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(GISContactType)

            'Need to store this value in hidden in order to read from javascript
            txtPostBackTo.Value = Request.QueryString("PostbackTo")
            Dim oOptionSettings As NexusProvider.OptionTypeSetting
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            If Not IsPostBack Then
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

                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.EmailRegex)
                If (oOptionSettings.OptionValue <> "") Then
                    vldEmailRegex.ValidationExpression = oOptionSettings.OptionValue
                Else
                    vldEmailRegex.ValidationExpression = ValidEmailRegex.Value
                End If

                If Request("ContactID") <> "" Then
                    ContactKey.Value = Request("ContactID")
                    txtMode.Value = "Update"
                    'load the contacts and change visibility of buttons
                    btnAddContacts.Visible = False
                    btnUpdateContacts.Visible = True

                    Dim oContacts As NexusProvider.Contact = oParty.Contacts.Item(CType(Request("ContactID"), Integer))

                    Select Case oContacts.ContactType
                        Case NexusProvider.ContactType.Email
                            GISContactType.Value = "E-MAIL"
                            liAreaCode.Visible = False
                            liExtension.Visible = False
                            divEmail.Visible = True
                            divNumber.Visible = False
                            Exit Select
                        Case NexusProvider.ContactType.Fax
                            GISContactType.Value = "FAX"
                            divEmail.Visible = False
                            divNumber.Visible = True
                            Exit Select
                        Case NexusProvider.ContactType.HomePhone
                            GISContactType.Value = "HOMEPHONE"
                            divEmail.Visible = False
                            divNumber.Visible = True
                            Exit Select
                        Case NexusProvider.ContactType.Main
                            GISContactType.Value = "MAIN"
                            divEmail.Visible = False
                            divNumber.Visible = True
                            Exit Select
                        Case NexusProvider.ContactType.Mobile
                            GISContactType.Value = "MOBILE"
                            divEmail.Visible = False
                            divNumber.Visible = True
                            Exit Select
                        Case NexusProvider.ContactType.Web
                            GISContactType.Value = "WEB"
                            divEmail.Visible = False
                            divNumber.Visible = True
                            Exit Select
                        Case NexusProvider.ContactType.Letter
                            GISContactType.Value = "LETTER" 'oContacts.OtherContactTypeCode
                            divEmail.Visible = False
                            divNumber.Visible = True
                            liAreaCode.Visible = True
                            Exit Select
                        Case NexusProvider.ContactType.MEMAIL
                            GISContactType.Value = "MEMAIL"
                            liAreaCode.Visible = False
                            liExtension.Visible = False
                            divEmail.Visible = True
                            divNumber.Visible = False
                            Exit Select
                        Case NexusProvider.ContactType.Other
                            GISContactType.Value = "OTHER"
                            divEmail.Visible = False
                            divNumber.Visible = True
                            Exit Select
                        Case NexusProvider.ContactType.Telephone
                            GISContactType.Value = "TELEPHONE"
                            divEmail.Visible = False
                            divNumber.Visible = True
                            Exit Select
                        Case NexusProvider.ContactType.MoreName
                            GISContactType.Value = "MORENAME"
                            divEmail.Visible = False
                            divNumber.Visible = True
                            Exit Select
                        Case NexusProvider.ContactType.WORKPHONE
                            GISContactType.Value = "WORKPHONE"
                            divEmail.Visible = False
                            divNumber.Visible = True
                            Exit Select
                    End Select

                    txtDescription.Text = oContacts.Description
                    txtAreaCode.Text = oContacts.AreaCode
                    txtNumber.Text = oContacts.Number
                    txtExtension.Text = oContacts.Extension
                Else
                    txtMode.Value = "Add"
                End If

                If Request("Source") = "PaymentHub" Then
                    lblTitle.Text = GetLocalResourceObject("lbl_Contact_Title_PaymentHub")
                    GISContactType.Value = "MEMAIL"
                    liAreaCode.Visible = False
                    liExtension.Visible = False
                    divEmail.Visible = True
                    divNumber.Visible = False
                    GISContactType.Enabled = False

                End If

            End If

        End Sub

        Protected Sub GISContactType_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles GISContactType.SelectedIndexChange
            Dim sContactType As String = GISContactType.Value

            If (sContactType IsNot Nothing) Then
                Select Case sContactType.ToUpper
                    Case "E-MAIL" '" 'NexusProvider.ContactType.Email
                        vldEmailRegex.Enabled = True
                        vldNumberRegex.Enabled = False
                        liAreaCode.Visible = False
                        liExtension.Visible = False
                        divEmail.Visible = True
                        divNumber.Visible = False
                        txtNumber.Visible = True
                        lblNumber.Text = GetLocalResourceObject("lbl_Email")
                        RqdContact_Number.ErrorMessage = GetLocalResourceObject("lbl_ErrMsg_Email")

                    Case "MEMAIL" '" 'NexusProvider.ContactType.MainEmailContact
                        vldEmailRegex.Enabled = True
                        vldNumberRegex.Enabled = False
                        liAreaCode.Visible = False
                        liExtension.Visible = False
                        divEmail.Visible = True
                        divNumber.Visible = False
                        txtNumber.Visible = True
                        lblNumber.Text = GetLocalResourceObject("lbl_MeMail")
                        RqdContact_Number.ErrorMessage = GetLocalResourceObject("lbl_ErrMsg_MeMail")
                    Case "FAX" 'NexusProvider.ContactType.Fax
                        vldNumberRegex.Enabled = True
                        vldEmailRegex.Enabled = False
                        liAreaCode.Visible = True
                        liExtension.Visible = True
                        divEmail.Visible = False
                        divNumber.Visible = True
                        txtNumber.Visible = True
                        lblNumber.Text = GetLocalResourceObject("lbl_PnlContact_Number")
                        RqdContact_Number.ErrorMessage = GetLocalResourceObject("lbl_ErrMsg_Number")
                    Case "TELEPHONE", "WORKPHONE" 'NexusProvider.ContactType.HomePhone
                        vldNumberRegex.Enabled = True
                        vldEmailRegex.Enabled = False
                        liAreaCode.Visible = True
                        liExtension.Visible = True
                        txtNumber.Visible = True
                        divEmail.Visible = False
                        divNumber.Visible = True
                        lblNumber.Text = GetLocalResourceObject("lbl_PnlContact_Number")
                        RqdContact_Number.ErrorMessage = GetLocalResourceObject("lbl_ErrMsg_Number")
                    Case "MAIN" 'NexusProvider.ContactType.Main
                        vldNumberRegex.Enabled = True
                        vldEmailRegex.Enabled = False
                        liAreaCode.Visible = True
                        divEmail.Visible = False
                        divNumber.Visible = True
                        liExtension.Visible = True
                        lblNumber.Text = GetLocalResourceObject("lbl_PnlContact_Number")
                        RqdContact_Number.ErrorMessage = GetLocalResourceObject("lbl_ErrMsg_Number")
                    Case "MOBILE" 'NexusProvider.ContactType.Mobile
                        vldNumberRegex.Enabled = True
                        vldEmailRegex.Enabled = False
                        liAreaCode.Visible = True
                        liExtension.Visible = True
                        divEmail.Visible = False
                        divNumber.Visible = True
                        txtNumber.Visible = True
                        lblNumber.Text = GetLocalResourceObject("lbl_PnlContact_Number")
                        RqdContact_Number.ErrorMessage = GetLocalResourceObject("lbl_ErrMsg_Number")

                    Case "HOMEPHONE"
                        vldNumberRegex.Enabled = True
                        vldEmailRegex.Enabled = False
                        divNumber.Visible = True
                        liAreaCode.Visible = True
                        liExtension.Visible = True
                        txtNumber.Visible = True
                        divEmail.Visible = False
                        lblNumber.Text = GetLocalResourceObject("lbl_PnlContact_Number")
                        RqdContact_Number.ErrorMessage = GetLocalResourceObject("lbl_ErrMsg_Number")

                    Case "LETTER"
                        vldNumberRegex.Enabled = False
                        vldEmailRegex.Enabled = False
                        RqdContact_Number.Enabled = False
                        liAreaCode.Visible = True
                        liExtension.Visible = True
                        divEmail.Visible = False
                        divNumber.Visible = True
                        txtNumber.Visible = True
                        lblNumber.Text = GetLocalResourceObject("lbl_PnlContact_Number")
                        RqdContact_Number.ErrorMessage = GetLocalResourceObject("lbl_ErrMsg_Number")
                    Case "OTHER"
                        vldNumberRegex.Enabled = True
                        vldEmailRegex.Enabled = False
                        divNumber.Visible = True
                        liAreaCode.Visible = True
                        liExtension.Visible = True
                        divEmail.Visible = False
                        lblNumber.Text = GetLocalResourceObject("lbl_PnlContact_Number")
                        RqdContact_Number.ErrorMessage = GetLocalResourceObject("lbl_ErrMsg_Number")
                    Case Else
                        vldNumberRegex.Enabled = True
                        vldEmailRegex.Enabled = False
                        divNumber.Visible = True
                        txtNumber.Visible = True
                        liAreaCode.Visible = True
                        liExtension.Visible = True
                        divEmail.Visible = False
                        lblNumber.Text = GetLocalResourceObject("lbl_PnlContact_Number")
                        RqdContact_Number.ErrorMessage = GetLocalResourceObject("lbl_ErrMsg_Number")
                End Select
            End If
        End Sub

    End Class
End Namespace