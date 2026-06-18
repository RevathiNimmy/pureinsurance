Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Globalization.CultureInfo
Imports Nexus.Constants
Imports Nexus.Constants.Session



Namespace Nexus
    Partial Class Modal_Loyalty : Inherits System.Web.UI.Page
        Dim oParty As NexusProvider.BaseParty = Nothing

#Region "Public Properties"

        Public Property LoyaltySchemeName() As String
            Get
                Return LoyaltyScheme.Value
            End Get
            Set(ByVal value As String)
                LoyaltyScheme.Value = value
            End Set
        End Property
        ''' <summary>
        ''' To return the Membership Number
        ''' </summary> 
        Public Property MembershipNumber() As String
            Get
                Return txtMemberShipNo.Text
            End Get
            Set(ByVal value As String)
                txtMemberShipNo.Text = value
            End Set
        End Property
        ''' <summary>
        ''' To return the Other Reference
        ''' </summary>
        Public Property OtherReference() As String
            Get
                Return txtOtherRef.Text
            End Get
            Set(ByVal value As String)
                txtOtherRef.Text = value
            End Set
        End Property
        ''' <summary>
        ''' To return the Start Date
        ''' </summary> 
        Public Property StartDate() As String
            Get
                Return txtStartDate.Text
            End Get
            Set(ByVal value As String)
                txtStartDate.Text = value
            End Set
        End Property

        ''' <summary>
        ''' To return the End Date
        ''' </summary> 
        Public Property EndDate() As String
            Get
                Return txtEndDate.Text
            End Get
            Set(ByVal value As String)
                txtEndDate.Text = value
            End Set
        End Property

        ''' <summary>
        ''' To return the Main Member
        ''' </summary> 
        Public Property MainMember() As String
            Get
                Return txtMainMember.Text
            End Get
            Set(ByVal value As String)
                txtMainMember.Text = value
            End Set
        End Property
        ''' <summary>
        ''' To return the Active Status
        ''' </summary> 
        Public Property ActiveStatus() As Boolean
            Get
                Return chkActive.Checked
            End Get
            Set(ByVal value As Boolean)
                chkActive.Checked = value
            End Set
        End Property
#End Region

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Not IsPostBack Then
                txtStartDate.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'To set the Focus
            Page.SetFocus(LoyaltyScheme)

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
               
                If Request("LoyaltyID") <> "" Then
                    txtKey.Value = Request("LoyaltyID")
                    txtMode.Value = "Update"
                    btnAddLoyalty.Visible = False
                    btnUpdateLoyalty.Visible = True

                    Dim oLoyalty As NexusProvider.Loyalty = oParty.Loyalty.Item(CType(Request("LoyaltyID"), Integer))
                    With oLoyalty
                        LoyaltyScheme.Value = .LoyaltySchemeCode
                        txtMemberShipNo.Text = .MembershipNumber
                        txtOtherRef.Text = .OtherReference
                        txtStartDate.Text = .StartDate
                        If .EndDate.ToShortDateString.Trim = "1/1/0001" Then
                            txtEndDate.Text = Nothing
                        Else
                            txtEndDate.Text = .EndDate
                        End If
                        txtMainMember.Text = .MainMember
                        chkActive.Checked = .Active
                    End With

                Else
                    txtMode.Value = "Add"
                End If
            End If

            rangevldStartDate.MaximumValue = Now.Date

            If oParty IsNot Nothing Then
                If oParty.Loyalty IsNot Nothing AndAlso oParty.Loyalty.Count > 0 Then
                    For iCount As Integer = 0 To oParty.Loyalty.Count - 1
                        LoyaltyType.Value &= oParty.Loyalty(iCount).LoyaltySchemeCode & ";"
                    Next
                End If
            End If
        End Sub
    End Class
End Namespace
