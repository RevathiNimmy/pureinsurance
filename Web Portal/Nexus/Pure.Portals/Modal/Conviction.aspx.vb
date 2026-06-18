Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus
    Partial Class Modal_Conviction : Inherits System.Web.UI.Page
        Dim oParty As NexusProvider.BaseParty = Nothing
#Region "Public Properties"

#Region "Conviction"
        Public Property GetConvictionType() As String
            Get
                Return ConvictionType.Value
            End Get
            Set(ByVal value As String)
                ConvictionType.Value = value
            End Set
        End Property
        Public Property GetConvictionStatus() As String
            Get
                Return ConvictionStatus.Value
            End Get
            Set(ByVal value As String)
                ConvictionStatus.Value = value
            End Set
        End Property
        Public Property ConvictionDescription() As String
            Get
                Return txtConvDescription.Text
            End Get
            Set(ByVal value As String)
                txtConvDescription.Text = value
            End Set
        End Property
        Public Property ConvictionFine() As String
            Get
                Return txtConvFine.Text
            End Get
            Set(ByVal value As String)
                txtConvFine.Text = value
            End Set
        End Property
        Public Property ConvictionDate() As String
            Get
                Return DATE__CONVICTION.Text
            End Get
            Set(ByVal value As String)
                DATE__CONVICTION.Text = value
            End Set
        End Property
#End Region

#Region "Sentence"
        Public Property GetSentenceType() As String
            Get
                Return SentenceType.Value
            End Get
            Set(ByVal value As String)
                SentenceType.Value = value
            End Set
        End Property
        Public Property SentenceDescription() As String
            Get
                Return txtSentenceDescription.Text
            End Get
            Set(ByVal value As String)
                txtSentenceDescription.Text = value
            End Set
        End Property
        Public Property SentenceDate() As Date
            Get
                Return txtSentenceDate.Text
            End Get
            Set(ByVal value As Date)
                txtSentenceDate.Text = value
            End Set
        End Property
        Public Property SentenceDuration() As String
            Get
                Return txtSentenceDuration.Text
            End Get
            Set(ByVal value As String)
                txtSentenceDuration.Text = value
            End Set
        End Property
        Public Property SentenceTimeUnit() As String
            Get
                Return TimeUnit.Value
            End Get
            Set(ByVal value As String)
                TimeUnit.Value = value
            End Set
        End Property
#End Region

#Region "Motor Related"
        Public Property GetAlcoholMSRMethod() As String
            Get
                Return AlcoholMsrMethod.Value
            End Get
            Set(ByVal value As String)
                AlcoholMsrMethod.Value = value
            End Set
        End Property
        Public Property AlcoholLevel() As String
            Get
                Return txtAlcoholLevel.Text
            End Get
            Set(ByVal value As String)
                txtAlcoholLevel.Text = value
            End Set
        End Property
        Public Property PenaltyPoints() As String
            Get
                Return txtPenaltyPoints.Text
            End Get
            Set(ByVal value As String)
                txtPenaltyPoints.Text = value
            End Set
        End Property
#End Region

#End Region

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
          
            'To set the Focus
            Page.SetFocus(ConvictionType)

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

                If Request("ConvictionID") <> "" Then
                    txtMode.Value = "Update"
                    txtKey.Value = Request("ConvictionID")
                    btnAddConviction.Visible = False
                    btnUpdateConviction.Visible = True

                    Dim oConviction As NexusProvider.Convictions = oParty.Conviction.Item(CType(Request("ConvictionID"), Integer))
                    ConvictionType.Value = oConviction.TypeCode
                    ConvictionStatus.Value = oConviction.StatusCode
                    AlcoholMsrMethod.Value = oConviction.AlcoholMeasurementMethod
                    SentenceType.Value = oConviction.SentenceTypeCode
                    txtConvFine.Text = Math.Round(oConviction.FineAmount, 2)
                    txtConvDescription.Text = oConviction.Description
                    DATE__CONVICTION.Text = oConviction.ConvictionDate
                    txtAlcoholLevel.Text = oConviction.AlcoholLevel
                    txtPenaltyPoints.Text = oConviction.DrivingLicensePenaltyPoints
                    txtSentenceDescription.Text = oConviction.SentenceDescription
                    txtSentenceDuration.Text = oConviction.SentenceDuration

                    If oConviction.SentenceEffectiveDate <> Date.MinValue Then
                        txtSentenceDate.Text = oConviction.SentenceEffectiveDate
                    End If

                    TimeUnit.Value = oConviction.SentenceDurationQualifier



                    If oConviction.ConvictionDate = Nothing Then
                        DATE__CONVICTION.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                    End If

                    'If oConviction.SentenceEffectiveDate = Nothing Then
                    '    txtSentenceDate.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                    'End If
                Else
                    txtMode.Value = "Add"
                End If
                rangevldConvDate.MaximumValue = Now.Date

            End If
        End Sub
    End Class
End Namespace

