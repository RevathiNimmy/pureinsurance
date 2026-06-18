Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Modal_ProspectPolicies : Inherits System.Web.UI.Page
        Dim oParty As NexusProvider.BaseParty = Nothing

        Public Property ProspectPolicies() As NexusProvider.ProspectPolicies
            Get
                Dim oPPolicy As New NexusProvider.ProspectPolicies
                With oPPolicy
                    .ProspectTypeCode = TypeCode
                    .RenewalDate = RenewalDate
                    .TimesQuoted = TimesQuoted
                    .TargetPremium = TargetPremium
                End With
                Return oPPolicy
            End Get
            Set(ByVal value As NexusProvider.ProspectPolicies)
                If value Is Nothing Then
                    TypeCode = String.Empty
                    RenewalDate = String.Empty
                    TimesQuoted = String.Empty
                    TargetPremium = String.Empty
                Else
                    TypeCode = TypeCode
                    RenewalDate = RenewalDate
                    TimesQuoted = TimesQuoted
                    TargetPremium = TargetPremium
                End If

            End Set
        End Property

        Public Property TypeCode() As String
            Get
                Return GISPolicy.value
            End Get
            Set(ByVal value As String)
                GISPolicy.Value = value
            End Set
        End Property

        Public Property RenewalDate() As String
            Get
                Return DATE_RENEWAL.Text
            End Get
            Set(ByVal value As String)
                DATE_RENEWAL.Text = value
            End Set
        End Property

        Public Property TimesQuoted() As String
            Get
                Return txtQuote.Text
            End Get
            Set(ByVal value As String)
                txtQuote.Text = value
            End Set
        End Property

        Public Property TargetPremium() As String
            Get
                Return txtPremium.Text
            End Get
            Set(ByVal value As String)
                txtPremium.Text = value
            End Set
        End Property

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Not IsPostBack Then
                DATE_RENEWAL.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
            End If
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
      
            'To set the Focus
            Page.SetFocus(GISPolicy)

            'Need to store this value in hidden in order to read from javascript
            txtPostBackTo.Value = Request.QueryString("PostbackTo")

            If Not IsPostBack Then
                If Session(CNParty) IsNot Nothing Then
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    End Select
                End If

                If Request("ProspectPolicyID") <> "" Then
                    txtKey.Value = Request("ProspectPolicyID")
                    txtMode.Value = "Update"
                    'load the address and change visibility of buttons
                    btnAddPolicy.Visible = False
                    btnUpdatePolicy.Visible = True

                    Dim oPolicy As NexusProvider.ProspectPolicies = oParty.ProspectPolicy.Item(Request("ProspectPolicyID"))
                    GISPolicy.Value = oPolicy.ProspectTypeCode
                    'calRenewalDate.Value = oPolicy.RenewalDate
                    DATE_RENEWAL.Text = oPolicy.RenewalDate
                    txtQuote.Text = oPolicy.TimesQuoted
                    txtPremium.Text = oPolicy.TargetPremium
                Else
                    txtMode.Value = "Add"
                End If
            End If
        End Sub
    End Class
End Namespace
