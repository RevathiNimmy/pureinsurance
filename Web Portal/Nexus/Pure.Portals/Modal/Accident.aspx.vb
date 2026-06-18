Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus
    Partial Class Modal_Accident : Inherits System.Web.UI.Page
        Dim oParty As NexusProvider.BaseParty = Nothing

#Region "Public Properties"
        Public Property AccidentDate() As Date
            Get
                Return txtAccidentDate.Text
            End Get
            Set(ByVal value As Date)
                txtAccidentDate.Text = value
            End Set
        End Property
        Public Property AccidentDescription() As String
            Get
                Return txtAccidentDescription.Text
            End Get
            Set(ByVal value As String)
                txtAccidentDescription.Text = value
            End Set
        End Property
        Public Property IsAtFault() As Boolean
            Get
                Return chkIsAtFault.Checked
            End Get
            Set(ByVal value As Boolean)
                chkIsAtFault.Text = value
            End Set
        End Property

#End Region

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(txtAccidentDate)

            'Need to store this value in hidden in order to read from javascript
            txtPostBackTo.Value = Request.QueryString("PostbackTo")

            If Not IsPostBack Then

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

                If Request("AccidentID") <> "" Then
                    txtMode.Value = "Update"
                    txtAccidentKey.Value = Request("AccidentID")
                    btnAddAccident.Visible = False
                    btnUpdateAccident.Visible = True

                    Dim oAccident As NexusProvider.Accident = oParty.Accidents.Item(CType(Request("AccidentID"), Integer))
                    If oAccident.AccidentDate = Nothing Then
                        txtAccidentDate.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                    Else
                        txtAccidentDate.Text = oAccident.AccidentDate
                    End If
                    txtAccidentDescription.Text = oAccident.Description
                    chkIsAtFault.Checked = oAccident.IsAtFault
                Else
                    txtMode.Value = "Add"
                End If
                rangevldAccidentDate.MaximumValue = Now.Date

            End If
        End Sub

    End Class
End Namespace

