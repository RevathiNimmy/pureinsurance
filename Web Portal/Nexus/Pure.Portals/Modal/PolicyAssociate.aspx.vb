Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Modal_PolicyAssociate : Inherits System.Web.UI.Page

        Dim oParty As NexusProvider.BaseParty = Nothing
        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim nInsurancefileKey As Integer = 0
            Dim nRowKey As Integer = Request.QueryString("RowKey")
            Dim sDisplayMode As String = Request.QueryString("displaymode")
            Dim oPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

			If oQuote Is Nothing Then
                Dim oClaim As NexusProvider.Claim = CType(Session(CNClaim), NexusProvider.Claim)
                If oClaim IsNot Nothing Then
                    nInsurancefileKey = oClaim.InsuranceFileKey
                    oQuote = oWebService.GetHeaderAndSummariesByKey(nInsurancefileKey)
                End If
            End If

            If Not Page.IsPostBack Then
                oPolicyAssociateCollection = oWebService.GetPolicyAssociates(oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey, Nothing)
                Session(CNPolicyAssociateCollection) = oPolicyAssociateCollection
                If oPolicyAssociateCollection IsNot Nothing Then
                    For Each oAssociate As NexusProvider.PolicyAssociate In oPolicyAssociateCollection
                        If oAssociate.InsuranceFileAssociatesKey = nRowKey Then
                            txtClient.Text = oAssociate.PartyCode
                            txtAssociationDetail.Text = oAssociate.AssociationDetail
                            ddlAssociation.Value = oAssociate.AssociationTypeKey
                            hdnDateAttached.Value = oAssociate.DateAttached
                            hdnDateRemoved.Value = oAssociate.DateRemoved
                            Exit For
                        End If
                    Next
                End If

                txtClient.Attributes.Add("readonly", "readonly")
                If sDisplayMode = "view" Then
                    txtAssociationDetail.Attributes.Add("readonly", "readonly")
                    ddlAssociation.Attributes.Add("disabled", "disabled")
                    btnOK.Visible = False
                End If
            End If
        End Sub

        Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
            If Page.IsValid Then
                Dim nRowKey As Integer = Request.QueryString("RowKey")
                Dim oPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection

                oPolicyAssociateCollection = Session(CNPolicyAssociateCollection)
                For iCount As Integer = 0 To oPolicyAssociateCollection.Count - 1
                    If oPolicyAssociateCollection(iCount).InsuranceFileAssociatesKey = nRowKey Then
                        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                        Dim oSecondaryPolicyAssociate As NexusProvider.PolicyAssociate = New NexusProvider.PolicyAssociate
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)

                        oSecondaryPolicyAssociate.InsuranceFileKey = oPolicyAssociateCollection(iCount).InsuranceFileKey
                        oSecondaryPolicyAssociate.InsuranceFileAssociatesKey = oPolicyAssociateCollection(iCount).InsuranceFileAssociatesKey
                        oSecondaryPolicyAssociate.InsuranceFolderCnt = oPolicyAssociateCollection(iCount).InsuranceFolderCnt
                        oSecondaryPolicyAssociate.IsDeleted = False
                        oSecondaryPolicyAssociate.PartyKey = oPolicyAssociateCollection(iCount).PartyKey
                        oSecondaryPolicyAssociate.RowKey = nRowKey
                        oSecondaryPolicyAssociate.AssociationDetail = txtAssociationDetail.Text
                        oSecondaryPolicyAssociate.AssociationTypeKey = ddlAssociation.Value
                        oSecondaryPolicyAssociate.DateAttached = FormatDateTime(hdnDateAttached.Value, DateFormat.GeneralDate)
                        oSecondaryPolicyAssociate.DateRemoved = FormatDateTime(hdnDateRemoved.Value, DateFormat.GeneralDate)
                        oSecondaryPolicyAssociate.ActionType = NexusProvider.PolicyAssociateActionType.EditRow

                        oPolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection
                        oPolicyAssociateCollection.Add(oSecondaryPolicyAssociate)

                        Dim oPolicyAssociate As NexusProvider.PolicyAssociate = New NexusProvider.PolicyAssociate
                        oPolicyAssociate = oWebService.UpdatePolicyAssociates(oPolicyAssociateCollection, oQuote.TimeStamp, Nothing)
                        oQuote.TimeStamp = oPolicyAssociate.TimeStamp
                        Session(CNQuote) = oQuote
                        Exit For
                    End If
                Next
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            End If
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
    End Class
End Namespace

