Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Modal_Lifestyle : Inherits System.Web.UI.Page

        Dim oParty As NexusProvider.BaseParty = Nothing
#Region "Public Properities"
        Public Property LifeStyle() As NexusProvider.LifeStyle
            Get
                Dim oLifestyle As New NexusProvider.LifeStyle
                With oLifestyle
                    .Name = Name
                    .DateOfBirth = DOB
                    .GenderCode = GenderCode
                    .CategoryCode = Category
                    .OccupationCode = OccupationCode
                    .SecOccupationCode = SecOccupationCode
                    .Smoker = Smoker
                End With
                Return oLifestyle
            End Get
            Set(ByVal value As NexusProvider.LifeStyle)
                If value Is Nothing Then
                    Name = String.Empty
                    DOB = String.Empty
                    GenderCode = String.Empty
                    Category = String.Empty
                    OccupationCode = String.Empty
                    SecOccupationCode = String.Empty
                Else
                    With value
                        Name = .Name
                        DOB = .DateOfBirth
                        GenderCode = .GenderCode
                        Category = .CategoryCode
                        OccupationCode = .OccupationCode
                        SecOccupationCode = .SecOccupationCode
                        Smoker = .Smoker
                    End With
                End If
            End Set
        End Property

        Public Property Name() As String
            Get
                Return txtName.Text
            End Get
            Set(ByVal value As String)
                txtName.Text = value
            End Set
        End Property
        Public Property DOB() As String
            Get
                Return txtDOB.Text
            End Get
            Set(ByVal value As String)
                txtDOB.Text = value
            End Set
        End Property
        Public Property Category() As String
            Get
                Return GISLifestyle_Category.Value
            End Get
            Set(ByVal value As String)
                GISLifestyle_Category.Value = value
            End Set
        End Property
        Public Property GenderCode() As String
            Get
                Return GISLifestyle_Gender.Value
            End Get
            Set(ByVal value As String)
                GISLifestyle_Gender.Value = value
            End Set
        End Property

        Public Property OccupationCode() As String
            Get
                Return GISLifestyle_Occupation.Value
            End Get
            Set(ByVal value As String)
                GISLifestyle_Occupation.Value = value
            End Set
        End Property

        Public Property SecOccupationCode() As String
            Get
                Return GISLifestyle_SecOccupation.Value
            End Get
            Set(ByVal value As String)
                GISLifestyle_SecOccupation.Value = value
            End Set
        End Property
        Public Property Smoker() As String
            Get
                Return chkSmoker.Checked
            End Get
            Set(ByVal value As String)
                chkSmoker.Checked = value
            End Set
        End Property
#End Region

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(txtName)

            'Need to store this value in hidden in order to read from javascript
            txtPostBackTo.Value = Request.QueryString("PostbackTo")

            'Set the max and min value in rangevalidator of the DOB
            rvDOB.MaximumValue = Date.Now.ToShortDateString
            rvDOB.MinimumValue = "01/01/1900"

            If Not IsPostBack Then
                If Session(CNParty) IsNot Nothing Then
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    End Select
                End If

                If Request("LifeStyleID") <> "" Then
                    txtMode.Value = "Update"
                    txtKey.Value = Request("LifeStyleID")
                    'load the address and change visibility of buttons
                    btnAddLifestyle.Visible = False
                    btnUpdateLifestyle.Visible = True

                    Dim oLifeStyle As NexusProvider.Lifestyle = oParty.Lifestyle.Item(Request("LifeStyleID"))

                    txtName.Text = oLifeStyle.Name
                    txtDOB.Text = oLifeStyle.DateOfBirth()

                    GISLifestyle_Gender.Value = oLifeStyle.GenderCode

                    GISLifestyle_Occupation.Value = oLifeStyle.OccupationCode
                    GISLifestyle_Category.Value = oLifeStyle.CategoryCode
                    GISLifestyle_SecOccupation.Value = oLifeStyle.SecOccupationCode
                    chkSmoker.Checked = oLifeStyle.Smoker

                Else
                    txtMode.Value = "Add"
                End If
            End If
            
            Dim oListItemInsured As NexusProvider.LookupListItem = Nothing

            For Each oListItem As NexusProvider.LookupListItem in GISLifestyle_Category.Items
                If oListItem.Code = "INSURED" Then
                    oListItemInsured = New NexusProvider.LookupListItem()
                    oListItemInsured = oListItem
                End If
            Next

            If Not IsNothing(oListItemInsured) Then
                GISLifestyle_Category.Items.Remove(oListItemInsured)
                oListItemInsured = Nothing
            End If

        End Sub
    End Class
End Namespace
