Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Modal_Associate : Inherits System.Web.UI.Page

        Dim oParty As NexusProvider.BaseParty = Nothing
        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Public Property Associate() As NexusProvider.Associate
            Get
                Dim oAssociates As New NexusProvider.Associate
                With oAssociates
                    .RelationshipCode = RelationshipCode
                End With
                Return oAssociates
            End Get
            Set(ByVal value As NexusProvider.Associate)
                If value Is Nothing Then
                    RelationshipCode = String.Empty
                Else
                    With value
                        RelationshipCode = .RelationshipCode
                    End With
                End If
            End Set
        End Property

        Public Property RelationshipCode() As String
            Get
                Return Me.GISAssociate_Relationship.Value
            End Get
            Set(ByVal value As String)
                Me.GISAssociate_Relationship.Value = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
         
            'To set the Focus
            Page.SetFocus(btnClient)

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

                txtClient.Attributes.Add("readonly", "readonly")

                If Request("AssociateID") <> "" Then
                    txtMode.Value = "Update"
                    txtKey.Value = Request("AssociateID")
                    btnAddAssociate.Visible = False
                    btnUpdateAssociate.Visible = True

                    Dim oAssociate As NexusProvider.Associate = oParty.Associate.Item(CType(Request("AssociateID"), Integer))
                    GISAssociate_Relationship.Value = oAssociate.RelationshipCode
                    Me.txtClient.Text = oAssociate.AssociateCode
                    Me.AssociateName.Value = oAssociate.AssociateName
                    Me.AssociateKey.Value = oAssociate.AssociateKey
                Else
                    txtMode.Value = "Add"
                End If
                btnAddAssociate.Attributes.Add("onclick", "javascript: if((Page_IsValid == true) && (typeof(Page_IsValid)!='undefined')){" & btnAddAssociate.ClientID + ".disabled=true;" & Me.ClientScript.GetPostBackEventReference(btnAddAssociate, Nothing) & ";}")
            End If
        End Sub
    End Class
End Namespace

