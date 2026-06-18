Imports System.Web.Configuration
Imports Nexus.Library
Imports CMS.library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Data
Imports System.Configuration.ConfigurationManager
Imports System
Imports System.Globalization
Imports System.Threading
Imports NexusProvider.SAMForInsurance

Namespace Nexus
    Partial Class modal_FindClient
        Inherits BaseFindParty
        Shared oParty As NexusProvider.BaseParty = Nothing
        Private bIsAnySelected As Boolean = False
        ''' <summary>
        ''' Obtains the search result from database and populates the datagrid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            If Page.IsValid Then
                SetClient()
                FindParty(bIsAnySelected:=bIsAnySelected)
            End If
        End Sub

        Protected Sub SetClient()

            ' storing the type of client-search in the session
            Select Case ddlClientType.SelectedValue
                Case "PC"
                    Session(CNSearchType) = PartyType.PC
                Case "CC"
                    Session(CNSearchType) = PartyType.CC
                Case Else
                    Session(CNSearchType) = PartyType.GC
                    bIsAnySelected = True
            End Select

        End Sub
        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                Dim oItem As NexusProvider.BaseParty = Nothing
                Select Case True
                    Case TypeOf e.Row.DataItem Is NexusProvider.PersonalParty
                        'NOTE - this will need to be changed to give each row a unique id
                        'this needs to be matched in markup for the menu (id="Menu_<%# Eval("UserName") %>")
                        e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.PersonalParty).UserName)

                        oItem = CType(e.Row.DataItem, NexusProvider.PersonalParty)
                        Dim oAddress As NexusProvider.Address = oItem.Addresses(NexusProvider.AddressType.CorrespondenceAddress)
                        If oAddress IsNot Nothing Then
                            If oAddress.PostCode.Trim().Length = 0 Then
                                CType(e.Row.FindControl("ltPostcode"), Literal).Text = "&nbsp;"
                            Else
                                CType(e.Row.FindControl("ltPostcode"), Literal).Text = oAddress.PostCode
                            End If
                        End If

                        CType(e.Row.FindControl("ltCustomerType"), Literal).Text = "Personal"
                    Case TypeOf e.Row.DataItem Is NexusProvider.CorporateParty
                        'NOTE - this will need to be changed to give each row a unique id
                        'this needs to be matched in markup for the menu (id="Menu_<%# Eval("UserName") %>")
                        e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.CorporateParty).UserName)

                        oItem = CType(e.Row.DataItem, NexusProvider.CorporateParty)
                        Dim oAddress As NexusProvider.Address = oItem.Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                        If oAddress IsNot Nothing Then
                            If oAddress.PostCode.Trim().Length = 0 Then
                                CType(e.Row.FindControl("ltPostCode"), Literal).Text = "&nbsp;"
                            Else
                                CType(e.Row.FindControl("ltPostCode"), Literal).Text = oAddress.PostCode
                            End If
                        End If

                        CType(e.Row.FindControl("ltCustomerType"), Literal).Text = "Corporate"
                    Case Else

                End Select

                'conditional code below needs to be updated so that the hyperlink is properly formed
                If oItem IsNot Nothing Then
                    Dim sResolvedName As String
                    sResolvedName = oItem.ResolvedName.ToString.Replace("'", "\")
                    If Request("Associate") = "true" Or Request("AssociateID") <> "" Then
                        If Request("Associate") = "true" Then
                            CType(e.Row.Cells(5).FindControl("hypSelect"), HyperLink).Attributes.Add("OnClick", "setAssociate('" + oItem.UserName.ToString + "' , '" + oItem.Key.ToString + "','" + sResolvedName + "', 'Add')")
                        ElseIf Request("AssociateID") <> "" Then
                            CType(e.Row.Cells(5).FindControl("hypSelect"), HyperLink).Attributes.Add("OnClick", "setAssociate('" + oItem.UserName.ToString + "' , '" + oItem.Key.ToString + "','" + sResolvedName + "', 'Edit')")
                        End If
                    ElseIf Request("SecondaryAssociate") = "true" Then
                        CType(e.Row.Cells(5).FindControl("hypSelect"), HyperLink).Attributes.Add("OnClick", "setPolicyAssociate('" + oItem.UserName.ToString + "' , '" + oItem.Key.ToString + "','" + sResolvedName + "', 'Add')")

                    Else
                        If ViewState("AssociateID") <> Nothing Then
                            Response.Write(ViewState("AssociateID").ToString())
                        End If
                        CType(e.Row.Cells(5).FindControl("hypSelect"), HyperLink).Attributes.Add("OnClick", "self.parent.setClient('" + oItem.UserName.ToString.Trim + "' , '" + oItem.Key.ToString.Trim + "','" + sResolvedName + "',)")
                    End If
                    hdfClientCnt.Value = oItem.Key.ToString()
                    If TypeOf Session(CNParty) Is NexusProvider.PersonalParty Then
                        hdfPartyCnt.Value = DirectCast(Session(CNParty), NexusProvider.PersonalParty).Key.ToString()
                    ElseIf TypeOf Session(CNParty) Is NexusProvider.CorporateParty Then
                        hdfPartyCnt.Value = DirectCast(Session(CNParty), NexusProvider.CorporateParty).Key.ToString()
                    End If
                End If
            End If
        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request("Associate") = "true" OrElse Request("SecondaryAssociate") = "true" Then
                Me.ClientScript.RegisterStartupScript(GetType(String), "StartupScripts", "Displaydetails('Add');", True)
            Else
                Me.ClientScript.RegisterStartupScript(GetType(String), "StartupScripts", "Displaydetails('Edit');", True)
            End If
            If Request("AddAssociate") = "true" Then
                pnlAssociate.Visible = True
                rfvPolicyAssociate.Enabled = False
                RqdAssociate.Enabled = True
            End If
            If Request("SecondaryAssociate") = "true" Then
                rfvPolicyAssociate.Enabled = True
                pnlSecondaryAssociate.Visible = True
                RqdAssociate.Enabled = False
                If Session(CNQuote) IsNot Nothing Then
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    hdnDateAttached.Value = FormatDateTime(oQuote.CoverStartDate, DateFormat.GeneralDate)
                    hdnDateRemoved.Value = FormatDateTime(oQuote.CoverStartDate, DateFormat.GeneralDate)
                End If
            End If
            If Request("AssociateKeys") <> Nothing Then
                hdfAssociateKeys.Value = Request("AssociateKeys")
            End If

            txtClient.Attributes.Add("readonly", "readonly")
            txtMainClient.Attributes.Add("readonly", "readonly")
            rvDOB.MaximumValue = Now.Date

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
                If Request("AssociateID") <> "" Then
                    txtMode.Value = "Update"
                    txtKey.Value = Request("AssociateID")
                    hdfAssociateKey.Value = Request("AssociateKey")

                    Dim oAssociate As NexusProvider.Associate = oParty.Associate.Item(CType(Request("AssociateID"), Integer))
                    GISAssociate_Relationship.Value = oAssociate.RelationshipCode
                    Me.txtClient.Text = oAssociate.AssociateCode
                    Me.AssociateName.Value = oAssociate.AssociateName
                    Me.AssociateKey.Value = oAssociate.AssociateKey
                    btnAddAssociate.Attributes("style") = "display:none"
                    btnUpdateAssociate.Attributes("style") = "inline-block"
                Else
                    txtMode.Value = "Add"
                End If
            End If
        End Sub


        Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
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



        Protected Sub btnAddPolicyAssociate_Click(sender As Object, e As EventArgs) Handles btnAddPolicyAssociate.Click
            If Page.IsValid = True Then
                rfvPolicyAssociate.Enabled = True
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection
                Dim oSecondaryPolicyAssociate As NexusProvider.PolicyAssociate = New NexusProvider.PolicyAssociate
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)

                oSecondaryPolicyAssociate.InsuranceFileKey = oQuote.InsuranceFileKey
                'oSecondaryPolicyAssociate.InsuranceFileAssociatesKey = Trim(txtMainClient.Text)
                oSecondaryPolicyAssociate.InsuranceFolderCnt = oQuote.InsuranceFolderKey
                oSecondaryPolicyAssociate.IsDeleted = False
                oSecondaryPolicyAssociate.PartyKey = AssociateKey.Value

                oSecondaryPolicyAssociate.AssociationDetail = txtAssociationDetail.Text
                oSecondaryPolicyAssociate.AssociationTypeKey = ddlAssociation.Value
                oSecondaryPolicyAssociate.DateAttached = FormatDateTime(hdnDateAttached.Value, DateFormat.GeneralDate)
                oSecondaryPolicyAssociate.DateRemoved = FormatDateTime(hdnDateRemoved.Value, DateFormat.GeneralDate)
                oSecondaryPolicyAssociate.AddUnConfirmed = True
                oPolicyAssociateCollection.Add(oSecondaryPolicyAssociate)

                Dim oPolicyAssociate As NexusProvider.PolicyAssociate = New NexusProvider.PolicyAssociate
                oPolicyAssociate = oWebService.UpdatePolicyAssociates(oPolicyAssociateCollection, oQuote.TimeStamp, Nothing)
                oQuote.TimeStamp = oPolicyAssociate.TimeStamp
                Session(CNQuote) = oQuote

                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            End If
        End Sub

        Protected Sub cvPolicyAssociate_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvPolicyAssociate.ServerValidate
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oGetPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim sMasterClientCode As String = Request.QueryString("ClientCode")

            oGetPolicyAssociateCollection = oWebService.GetPolicyAssociates(oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey, Nothing)
            If (oGetPolicyAssociateCollection IsNot Nothing AndAlso oGetPolicyAssociateCollection.Count > 0) Then
                For Each oAssociate As NexusProvider.PolicyAssociate In oGetPolicyAssociateCollection
                    'WPR 24: Modified the validation to add Association Type
                    If ((oAssociate.PartyKey = AssociateKey.Value AndAlso _
                                       oAssociate.IsDeleted = False) OrElse _
                                      (oAssociate.PartyCode.Trim = sMasterClientCode) OrElse _
                                      (txtMainClient.Text.Trim = sMasterClientCode)) Then
                        args.IsValid = False
                        Exit For
                    Else
                        args.IsValid = True
                    End If
                Next
            Else
                If (Trim(sMasterClientCode) = Trim(txtMainClient.Text)) Then
                    args.IsValid = False
                Else
                    args.IsValid = True
                End If
            End If
        End Sub

        Protected Sub btnAddAssociate_Click(sender As Object, e As EventArgs) Handles btnAddAssociate.Click
            If Not Page.IsValid Then
                SetClient()
                FindParty()
                If Request("AddAssociate") = "true" Then
                    pnlAssociate.Visible = True
                    RqdAssociate.Enabled = True
                    btnAddAssociate.Visible = True
                End If
            End If
        End Sub

        Protected Sub cvPolicyAssociateExists_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvPolicyAssociate.ServerValidate
            Dim sMasterClientCode As String = Request.QueryString("ClientCode")
            Dim sPartyKey As String = Request.QueryString("PartyKey")
            If (Trim(sMasterClientCode) = Trim(txtClient.Text)) Then
                args.IsValid = False
            ElseIf GISAssociate_Relationship.Value = "" Then
                args.IsValid = False
                cvPolicyAssociateExists.ErrorMessage = ""
                If Session(CNParty) Is Nothing AndAlso Session(CNPartyKey) IsNot Nothing AndAlso Convert.ToString(Session(CNPartyKey)) <> "" Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    oParty = oWebService.GetParty(Session(CNPartyKey))
                    Session(CNParty) = oParty
                End If
            Else
                Dim oParty As NexusProvider.BaseParty = Nothing
                If Session(CNParty) Is Nothing Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    oParty = oWebService.GetParty(sPartyKey)
                    Session(CNParty) = oParty
                End If
                If Session(CNParty) IsNot Nothing Then
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    End Select
                End If
                Dim Associate As NexusProvider.AssociateCollection = oParty.Associate
                If Associate.Count > 0 Then
                    For i As Integer = 0 To Associate.Count - 1
                        If Associate.Item(i).AssociateCode.ToString().Trim() = txtClient.Text.Trim() AndAlso
                                                    Associate.Item(i).RelationshipCode.ToString().Trim() = GISAssociate_Relationship.Value.ToString().Trim() Then
                            args.IsValid = False
                            Exit For
                        Else
                            args.IsValid = True
                        End If
                    Next
                End If
            End If
            If args.IsValid Then
                Dim oPage = TryCast(HttpContext.Current.CurrentHandler, System.Web.UI.Page)
                ScriptManager.RegisterStartupScript(oPage.Page, oPage.GetType(), "PendingPortfolioTransfer", "UpdateAssociateData()", True)
            End If

        End Sub
    End Class

End Namespace