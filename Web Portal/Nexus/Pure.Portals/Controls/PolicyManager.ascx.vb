Imports System.Resources
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XmlReader
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Linq
Imports System.Data


Namespace Nexus

    Partial Class Controls_PolicyManager : Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oPartySummary As NexusProvider.PartySummary
        Private iPartyKey As Integer
        Public Const ItemDeleted As String = "ItemDeleted"
        Public Const CNPolicyCollection As String = "PolicyCollection"
        Public Const CNBrokerCollection As String = "BrokerCollection"
        Public Const CNPolicyChildCollection As String = "PolicyChildCollection"
        Public Const CNFilteredPolicyCollection As String = "FilteredPolicyCollection"
        Public Const CNIsReinstateLink As String = "ISReinstateLink"
        Public Const CNSelectedQuote As String = "SelectedQuote"
        Public sInsuranceFileRef As String

        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
        Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
        Dim oRiskType As New NexusProvider.RiskType
        Dim oSelectedQuote As NexusProvider.Quote

        ''' <summary>
        ''' Page init event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnMarkedConfirmation",
                       "<script language=""JavaScript"" type=""text/javascript"">function UnMarkedConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmUnMarkedCollection").ToString() & "');document.getElementById('" & hdMarkedtext.ClientID & "').value=IsConfirm;return IsConfirm;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnReInstatementConfirmation",
                      "<script language=""JavaScript"" type=""text/javascript"">function UnReInstatementConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmReInstatement").ToString() & "');return IsConfirm;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MarketPlacePolicyConfirmation",
                        "<script language=""JavaScript"" type=""text/javascript"">function MarketPlacePolicyConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy1").ToString() & "'); if(IsConfirm==true) { IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy2").ToString() & "'); if(IsConfirm==true) { document.getElementById('" & hvMarketPlacePolicy.ClientID & "').value = false; } return IsConfirm; } else {return IsConfirm;} }</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Quote Expire",
                      "<script language=""JavaScript"" type=""text/javascript"">function ShowError(){alert('" & GetLocalResourceObject("quoteexpire") & "');}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "RetainPolicyNumberConfirmation",
                        "<script language=""JavaScript"" type=""text/javascript"">function RetainPolicyNumberConfirmation(){if (confirm('" & GetLocalResourceObject("msg_ConfirmRetainPolicyNumberOnCopy").ToString() & "') == true) {document.getElementById('" & hvRetainPolNum.ClientID & "').value=true; return true;} else {document.getElementById('" & hvRetainPolNum.ClientID & "').value=false;} }</script>")

          Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "VoidPolicyVersionConfirmation",
                       "<script language=""JavaScript"" type=""text/javascript"">function VoidPolicyVersionConfirmation(){ var sVoidConfirmationMessage = document.getElementById('" & hvVoidMessage.ClientID & "').value; if (confirm(sVoidConfirmationMessage) == true) {document.getElementById('" & hvVoidConfirm.ClientID & "').value=true; return true;} else {document.getElementById('" & hvVoidConfirm.ClientID & "').value=false;} }</script>")
		       
        End Sub
        ''' <summary>
        ''' page load event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                btnBuy.Visible = False
                btnChange.Visible = False
                btnClaim.Visible = False
                btnCopy.Visible = False
                btnDetails.Visible = False
                btnEdit.Visible = False
                btnReinstatement.Visible = False
                btnView.Visible = False
                btnVoid.Visible = False
                ShowHideAssocoate()
                Dim oFormatString As Config.FormatString
                oFormatString = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency")
                ViewState("CurrencyDataFormatString") = oFormatString.DataFormatString

                If Session(CNRiskViewStartPoint) IsNot Nothing AndAlso Session(CNRiskViewStartPoint) = "ClientManager" Then
                    Session.Remove(CNRiskViewStartPoint)
                End If
            End If
        End Sub

        Protected Sub gvPolicyVersions_Load(sender As Object, e As EventArgs) Handles gvPolicyVersions.Load
            If gvPolicyVersions.PageCount = 1 Then
                gvPolicyVersions.AllowPaging = False
            Else
                gvPolicyVersions.AllowPaging = True
                gvPolicyVersions.PageSize = 10
                gvPolicyVersions.PageIndex = 0
            End If
        End Sub

        Protected Sub gvRisks_Load(sender As Object, e As EventArgs) Handles gvRisks.Load
            If gvRisks.PageCount = 1 Then
                gvRisks.AllowPaging = False
            Else
                gvRisks.AllowPaging = True
            End If
            Session.Remove(CNOnlyOriginalRating)
        End Sub

        Public Property PartyKey() As Integer
            Set(ByVal value As Integer)
                iPartyKey = value
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                Dim nAgentKey As Integer = 0
                Dim sAgentType As String = String.Empty
                Try
                    Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
                    Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                            .Portals.Portal(Portal.GetPortalID()).Products
                    If oUserDetails IsNot Nothing Then
                        If oUserDetails.Key <> 0 Then
                            nAgentKey = oUserDetails.Key
                            sAgentType = oUserDetails.PartyType
                        End If
                    End If

                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            With CType(oParty, NexusProvider.CorporateParty)
                                oPartySummary = oWebService.GetPartyPolicies(.ClientSharedData.ShortName.Trim, Nothing, nAgentKey, sAgentType)
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            With CType(oParty, NexusProvider.PersonalParty)
                                oPartySummary = oWebService.GetPartyPolicies(.ClientSharedData.ShortName.Trim, Nothing, nAgentKey, sAgentType)
                            End With
                    End Select

                    If Session(CNRiskIndex) IsNot Nothing AndAlso Not String.IsNullOrEmpty(Session(CNRiskIndex)) Then
                        Dim sInsuranceFolderKeys As String = String.Empty
                        Dim sInsuranceFileKeys As String = String.Empty
                        Dim sRiskKeys As String = String.Empty
                        'new function will return list of insurancefoldersnts,insurancefilecnts and riskcnts in comma separate list so add LINQ filter to collection 
                        oWebService.FindPoliciesByRiskIndex(iPartyKey, Session(CNRiskIndex), sInsuranceFolderKeys, sInsuranceFileKeys, sRiskKeys)
                        If Not String.IsNullOrEmpty(sInsuranceFolderKeys) Then
                            Dim oInsuranceFolderCnts() As String = sInsuranceFolderKeys.Split(",")
                            If Not String.IsNullOrEmpty(sInsuranceFileKeys) Then
                                ViewState.Add("InsuranceFileKeys", sInsuranceFileKeys)
                            End If
                            If Not String.IsNullOrEmpty(sRiskKeys) Then
                                ViewState.Add("RiskKeys", sRiskKeys)
                            End If
                            Dim oFilterPoliciesColl As New NexusProvider.PolicyCollection
                            Dim oFilterPolicies = From oPolicy As NexusProvider.Policy In oPartySummary.Policies Where oInsuranceFolderCnts.Contains(oPolicy.InsuranceFolderKey) Select oPolicy
                            If oFilterPolicies.Count > 0 Then
                                For Each row As NexusProvider.Policy In oFilterPolicies
                                    oFilterPoliciesColl.Add(row)
                                Next
                            End If
                            oPartySummary.Policies = oFilterPoliciesColl
                        End If
                    End If

                    'store the data in ViewState to use again for page indexing
                    If oPartySummary IsNot Nothing Then
                        If oPartySummary.Policies IsNot Nothing Then
                            ViewState.Add(CNPolicyCollection, oPartySummary.Policies)
                        End If
                    End If
                    'BindTree from ViewState(CNPolicyCollection)
                    PopulateTreeView()

                Finally
                    oWebService = Nothing
                    oPartySummary = Nothing
                End Try
            End Set
            Get
                Return iPartyKey
            End Get
        End Property

        ''' <summary>
        ''' Populate the treeview with status nodes
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateTreeView()

            Dim oAllPolicies As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)
            'Root Node
            Dim RootNode As System.Web.UI.WebControls.TreeNode = New System.Web.UI.WebControls.TreeNode("All")
            RootNode.SelectAction = TreeNodeSelectAction.None
            RootNode.PopulateOnDemand = False
            RootNode.Expanded = True
            Dim bIsNodeExists As Boolean = False

            If (tvClientPolicy.Nodes.Count = 0) Then
                tvClientPolicy.Nodes.Add(RootNode)
            End If

            'Bind Node for All Status
            If oAllPolicies IsNot Nothing Then
                Dim PolicyStatusList = From Status In oAllPolicies Select Status.PolicyStatus
                Dim PolicyStatusListDistinct = From OBJ In PolicyStatusList Distinct

                If PolicyStatusListDistinct.Count > 0 Then
                    Dim StatusNode As System.Web.UI.WebControls.TreeNode
                    For iCt As Integer = 0 To PolicyStatusListDistinct.Count - 1
                        StatusNode = New System.Web.UI.WebControls.TreeNode(Trim(PolicyStatusListDistinct(iCt)))
                        StatusNode.SelectAction = TreeNodeSelectAction.None
                        StatusNode.PopulateOnDemand = False
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        If oQuote IsNot Nothing Then
                            sInsuranceFileRef = oQuote.InsuranceFileRef
                        ElseIf Session(CNInsuranceFileKey) IsNot Nothing Then
                            Dim nFileKey As Integer = CInt(Session(CNInsuranceFileKey))
                            Dim oMatchByKey = From p As NexusProvider.Policy In oAllPolicies Where p.InsuranceFileKey = nFileKey
                            If oMatchByKey.Count > 0 Then sInsuranceFileRef = oMatchByKey.First().Reference
                        End If

                        If sInsuranceFileRef <> "" Then
                            Dim oMatchingPolicies = From p As NexusProvider.Policy In oAllPolicies _
                                                    Where Trim(p.PolicyStatus) = Trim(PolicyStatusListDistinct(iCt)) _
                                                    AndAlso Trim(p.Reference) = Trim(sInsuranceFileRef)
                            StatusNode.Expanded = (oMatchingPolicies.Count > 0)
                        Else
                            StatusNode.Expanded = False
                        End If
                        RootNode.ChildNodes.Add(StatusNode)

                        PopulateProductsAndPolicies(Trim(Convert.ToString(PolicyStatusListDistinct(iCt))), StatusNode)
                        tvClientPolicy_SelectedNodeChanged(Nothing, Nothing)
                    Next
                End If
            End If
        End Sub

        Private Function NodeExists(node As TreeNode, key As String) As Boolean
            For Each subNode As TreeNode In node.ChildNodes
                If subNode.Text = key Then
                    Return True
                End If
                If node.ChildNodes.Count > 0 Then
                    NodeExists(node, key)
                End If
            Next
            Return False
        End Function

        ''' <summary>
        ''' Populate the status nodes with policy/quote types
        ''' </summary>
        ''' <param name="vStatus"></param>
        ''' <param name="vParentNode"></param>
        ''' <remarks></remarks>
        Private Sub PopulateProductsAndPolicies(ByVal vStatus As String, vParentNode As System.Web.UI.WebControls.TreeNode)
            Dim oAllPolicies As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)

            Dim ProductsList = From products In oAllPolicies Where Trim(Convert.ToString(products.PolicyStatus)) = vStatus Select products.ProductCode
            Dim ProductListDistinct = From OBJ In ProductsList Distinct

            If ProductListDistinct.Count > 0 Then
                Dim ProductNode As System.Web.UI.WebControls.TreeNode
                For iCt As Integer = 0 To ProductListDistinct.Count - 1
                    ProductNode = New System.Web.UI.WebControls.TreeNode(Trim(ProductListDistinct(iCt)), vParentNode.Text + "|" + Trim(ProductListDistinct(iCt)))
                    ProductNode.SelectAction = TreeNodeSelectAction.None
                    ProductNode.PopulateOnDemand = False
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    If oQuote IsNot Nothing Then
                        sInsuranceFileRef = oQuote.InsuranceFileRef
                    ElseIf Session(CNInsuranceFileKey) IsNot Nothing Then
                        Dim nFileKey As Integer = CInt(Session(CNInsuranceFileKey))
                        Dim oMatchByKey = From p As NexusProvider.Policy In oAllPolicies Where p.InsuranceFileKey = nFileKey
                        If oMatchByKey.Count > 0 Then sInsuranceFileRef = oMatchByKey.First().Reference
                    End If
                    If sInsuranceFileRef <> "" Then
                        Dim oMatchingPolicies = From p As NexusProvider.Policy In oAllPolicies _
                                                Where Trim(p.PolicyStatus) = vStatus _
                                                AndAlso Trim(p.ProductCode) = Trim(ProductListDistinct(iCt)) _
                                                AndAlso Trim(p.Reference) = Trim(sInsuranceFileRef)
                        ProductNode.Expanded = (oMatchingPolicies.Count > 0)
                    Else
                        ProductNode.Expanded = False
                    End If
                    vParentNode.ChildNodes.Add(ProductNode)
                    PopulatePolicies(vStatus, Trim(ProductListDistinct(iCt)), ProductNode)
                Next
            End If
        End Sub

        ''' <summary>
        ''' Populate the status nodes with respective policy types
        ''' </summary>
        ''' <param name="vStatus"></param>
        ''' <param name="vProductCode"></param>
        ''' <param name="vParentNode"></param>
        ''' <remarks></remarks>
        Private Sub PopulatePolicies(vStatus As String, vProductCode As String, vParentNode As System.Web.UI.WebControls.TreeNode)
            Dim oAllPolicies As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)
            Dim PolicyList = From policies In oAllPolicies Where Trim(Convert.ToString(policies.PolicyStatus)) = vStatus And Trim(policies.ProductCode.ToString()) =
                     vProductCode Order By policies.InsuranceFileKey Select policies.InsuranceFolderKey, policies.Reference, policies.ClosePolicyClaims, policies.OpenPolicyClaims, policies.IsMarketPlacePolicy

            If PolicyList.Count > 0 Then
                Dim PolicyNode As System.Web.UI.WebControls.TreeNode
                For iCt As Integer = 0 To PolicyList.Count - 1
                    If PolicyList(iCt).ClosePolicyClaims > 0 Or PolicyList(iCt).OpenPolicyClaims > 0 Then
                        PolicyNode = New System.Web.UI.WebControls.TreeNode("<font color='Red'>" + Trim(PolicyList(iCt).Reference) + "</font>", PolicyList(iCt).InsuranceFolderKey)
                    ElseIf PolicyList(iCt).IsMarketPlacePolicy Then
                        PolicyNode = New System.Web.UI.WebControls.TreeNode("<font color='Green'>" + Trim(PolicyList(iCt).Reference) + "</font>", PolicyList(iCt).InsuranceFolderKey)
                    Else
                        PolicyNode = New System.Web.UI.WebControls.TreeNode(Trim(PolicyList(iCt).Reference), PolicyList(iCt).InsuranceFolderKey)
                    End If
                    PolicyNode.SelectAction = TreeNodeSelectAction.Select
                    PolicyNode.PopulateOnDemand = False
                    PolicyNode.Expanded = False
                    If sInsuranceFileRef = PolicyNode.Text Then
                        PolicyNode.Selected = True
                    End If
                    vParentNode.ChildNodes.Add(PolicyNode)
                Next
            End If
        End Sub
        ''' <summary>
        ''' Page pre render event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
            If Not IsPostBack AndAlso Me.Visible = True Then
                If Me.PartyKey = 0 AndAlso CType(Session(CNParty), NexusProvider.BaseParty).Key <> 0 _
                AndAlso Session(CNClientMode) = Mode.View AndAlso Session(CNIsNewClient) Is Nothing Then
                    'get the party key from session
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            Me.PartyKey = CType(Session(CNParty), NexusProvider.PersonalParty).Key
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            Me.PartyKey = CType(Session(CNParty), NexusProvider.CorporateParty).Key
                    End Select
                End If

                'Initialise this to false
                PanelViewAllPolicies(False)
                If UserCanDoTask("ViewOldPolicies") Then
                    PanelViewAllPolicies(True)
                Else
                    PanelViewAllPolicies(False)
                End If
            End If

            If gvRisks.Rows.Count = 0 Then
                btnBuy.Visible = False
                If (Request.UrlReferrer IsNot Nothing AndAlso Request.UrlReferrer.AbsoluteUri.Contains("RiskDetails.aspx")) _
                     OrElse (Session("CNPolicyBackFlag") IsNot Nothing AndAlso Session("CNPolicyBackFlag").ToString() = "BackFlag") Then
                    Dim oQuote As NexusProvider.Quote = Session(Constants.CNQuote)
                    If oQuote IsNot Nothing Then
                        Dim nSelectedInsuranceFileKey As Integer = oQuote.InsuranceFileKey
                        txtPolicyNo.Text = oQuote.InsuranceFileRef
                        btnSearch_Click(btnSearch, Nothing)
                        Dim nSelectedRowIndex As Integer = 0
                        For Each oRow As GridViewRow In gvPolicyVersions.Rows
                            If gvPolicyVersions.DataKeys(oRow.RowIndex).Values("InsuranceFileKey") = nSelectedInsuranceFileKey Then
                                gvPolicyVersions.SelectedIndex = nSelectedRowIndex
                                Exit For
                            End If
                            nSelectedRowIndex += 1
                        Next
                        If (Session("CNRiskBackFlag") IsNot Nothing AndAlso Session("CNRiskBackFlag").ToString() = "RiskBackFlag") Then
                            Dim nSelectedRiskIndex As Integer = 0
                            If gvRisks.Rows.Count > 0 AndAlso Session(CNCurrentRiskKey) IsNot Nothing Then
                                For Each oRow As GridViewRow In gvRisks.Rows
                                    If gvRisks.DataKeys(oRow.RowIndex).Values("Key") = oQuote.Risks(Session(CNCurrentRiskKey)).Key Then
                                        gvRisks.SelectedIndex = nSelectedRiskIndex
                                        Exit For
                                    End If
                                    nSelectedRiskIndex += 1
                                Next
                            End If

                        End If
                        Session.Remove("CNPolicyBackFlag")
                        Session.Remove("CNRiskBackFlag")
                        gvPolicyVersions_SelectedIndexChanged(Nothing, Nothing)
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "SelectQuotePolicyTab", "SelectQuotePolicyTab();", True)
                    End If
                End If
            End If
        End Sub

        Sub PanelViewAllPolicies(ByVal bStatus As Boolean)
            chkViewAllPolicies.Visible = bStatus
            lbl_ViewAllPolicies.Visible = bStatus
        End Sub
        ''' <summary>
        ''' To check if the policies are re-instated
        ''' </summary>
        ''' <param name="PolicyRef"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsReinstated(ByVal PolicyRef As String) As Boolean
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNFilteredPolicyCollection)
            Dim bStatus As Boolean = False
            Dim TempVar As Integer
            For TempVar = 0 To oPolicyCollection.Count - 1
                If oPolicyCollection(TempVar).InsuranceFileTypeCode IsNot Nothing Then
                    If oPolicyCollection(TempVar).Reference.Trim = PolicyRef.Trim _
                    And oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "MTAREINS" Then
                        bStatus = True
                        'Yes Policy Has been Cancelled/Lapsed
                        'Check whether it has been reinstated or not
                    End If
                End If
            Next
            Return bStatus
        End Function
        ''' <summary>
        ''' To check if the policies are renewed
        ''' </summary>
        ''' <param name="PolicyRef"></param>
        ''' <param name="CoverStartDate"></param>
        ''' <param name="iInsuranceFileKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsRenewed(ByVal PolicyRef As String, ByVal CoverStartDate As Date, ByVal iInsuranceFileKey As Integer) As Boolean
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNFilteredPolicyCollection)
            Dim bStatus As Boolean = False
            Dim TempVar As Integer
            For TempVar = 0 To oPolicyCollection.Count - 1
                If oPolicyCollection(TempVar).InsuranceFileTypeCode IsNot Nothing Then
                    If oPolicyCollection(TempVar).Reference.Trim = PolicyRef.Trim And oPolicyCollection(TempVar).CoverStartDate > CoverStartDate _
                And (oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "POLICY" Or
                oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "MTAREINS") _
                And oPolicyCollection(TempVar).InsuranceFileKey <> iInsuranceFileKey _
                And oPolicyCollection(TempVar).PolicyStatusCode.Trim.ToUpper <> "CAN" Then
                        bStatus = True
                        'Yes Policy Has been Renewed
                    End If
                End If
            Next
            Return bStatus
        End Function
        ''' <summary>
        ''' To check if the policies are in renewal
        ''' </summary>
        ''' <param name="PolicyRef"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsInRenewal(ByVal PolicyRef As String) As Boolean
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNFilteredPolicyCollection)
            Dim bStatus As Boolean = False
            Dim TempVar As Integer
            For TempVar = 0 To oPolicyCollection.Count - 1
                If oPolicyCollection(TempVar).InsuranceFileTypeCode IsNot Nothing Then
                    If oPolicyCollection(TempVar).Reference.Trim = PolicyRef.Trim _
                    And oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "RENEWAL" Then
                        bStatus = True
                        'Yes Policy Has been Renewed
                    End If
                End If
            Next
            Return bStatus
        End Function


        ''' <summary>
        ''' To check if polciy is cancelled
        ''' </summary>
        ''' <param name="v_sPolicyRef"></param>
        ''' <param name="v_sPolicyType"></param>
        ''' <param name="v_sPolicyStatus"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsPolicyCancelled(ByVal v_sPolicyRef As String, ByVal v_sPolicyType As String, ByVal v_sPolicyStatus As String) As Boolean
            'Policy Collection has any PolicyStatusCode="CAN" against the passed Policy 
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)
            Dim bStatus As Boolean = False

            'if Any Policy version has been Cancelled
            If v_sPolicyType = "MTAQUOTE" And v_sPolicyStatus = "CAN" Then
                'Flag set as TRUE without Check all Policy Version
                bStatus = True
            Else
                For nCount As Integer = 0 To oPolicyCollection.Count - 1
                    If oPolicyCollection(nCount).PolicyStatusCode IsNot Nothing Then
                        If oPolicyCollection(nCount).Reference.Trim = v_sPolicyRef.Trim _
                        And (oPolicyCollection(nCount).PolicyStatusCode.Trim = "CAN" Or oPolicyCollection(nCount).PolicyStatusCode.Trim = "LAP") And
                        (Not (oPolicyCollection(nCount).PolicyStatusCode.Trim = "CAN" And (oPolicyCollection(nCount).InsuranceFileTypeCode.Trim = "MTAQUOTE" Or
                         oPolicyCollection(nCount).InsuranceFileTypeCode.Trim = "MTAQCAN" Or oPolicyCollection(nCount).InsuranceFileTypeCode.Trim = "MTA PERM"))) Then
                            bStatus = True
                            'Yes Policy Has been Cancelled/Lapsed
                            'Check whether it has been reinstated or not
                            If IsReinstated(oPolicyCollection(nCount).Reference.Trim) = True And
                            IsRenewed(oPolicyCollection(nCount).Reference.Trim, oPolicyCollection(nCount).CoverStartDate, oPolicyCollection(nCount).InsuranceFileKey) = True Then
                                bStatus = False
                            End If
                            'Check whether MTA Quote has been reinstated or not
                            If ((v_sPolicyType.Trim = "POLICY" Or v_sPolicyType.Trim = "MTAREINS") And v_sPolicyStatus.Trim = "LIVE") Then
                                bStatus = False
                            End If
                        End If
                    End If
                Next
            End If
            Return bStatus
        End Function

        ''' <summary>
        ''' Perform specific tasks on the selection of treeview nodes
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub tvClientPolicy_SelectedNodeChanged(sender As Object, e As EventArgs)
            Session.Remove(CNInsuranceFileKey)
            Session.Remove(CNInsuranceFolderKey)
            Session.Remove(CNClaim)
            Dim iInsuranceFolderKey As Integer
            'Dim iInsuranceFileKey As Integer
            If tvClientPolicy.SelectedValue <> "" Then
                iInsuranceFolderKey = tvClientPolicy.SelectedValue
            ElseIf hvInsuranceFolderKey.Value <> "" Then
                iInsuranceFolderKey = CInt(hvInsuranceFolderKey.Value)
            End If
            Session(CNInsuranceFolderKey) = iInsuranceFolderKey
            Dim oPolicy As New NexusProvider.PolicyCollection
            If iInsuranceFolderKey <> 0 Then
                oPolicy = oWebService.GetAllPolicyVersions(iInsuranceFolderKey)
            End If

            Dim oFilteredPolicy As New NexusProvider.PolicyCollection
            If Not tvClientPolicy.SelectedNode Is Nothing Then
                Dim sToCompare As String = Regex.Replace(tvClientPolicy.SelectedNode.Text, "</?(font|div)[^>]*>", String.Empty, RegexOptions.IgnoreCase)
                Dim oPolicyRow = From policyRow In oPolicy Where policyRow.Reference.trim() = sToCompare
                                 Select policyRow
                Session(CNInsuranceFileKey) = CType(oPolicyRow(0), NexusProvider.Policy).InsuranceFileKey
            End If
            If oPolicy IsNot Nothing Then
                Dim sQuotesInsuranceFileTypes As String = "MTAQUOTE,MTAQTETEMP,MTAQREINS,MTAQCAN,RENEWAL"
                'Check if any non-quote policy version is in LIVE state
                Dim bHasLivePolicy As Boolean = False
                If Not chkViewAllPolicies.Checked Then
                    Dim sQuoteTypeCodes As String = "QUOTE,MTAQUOTE,MTAQTETEMP,MTAQREINS,MTAQCAN,RENEWAL,WRITTEN"
                    For Each pol As NexusProvider.Policy In oPolicy
                        If pol.PolicyStatusCode IsNot Nothing AndAlso pol.PolicyStatusCode.Trim().ToUpper() = "LIVE" _
                            AndAlso pol.InsuranceFileTypeCode IsNot Nothing _
                            AndAlso Not sQuoteTypeCodes.Contains(pol.InsuranceFileTypeCode.Trim().ToUpper()) Then
                            bHasLivePolicy = True
                            Exit For
                        End If
                    Next
                End If
                For Each policy As NexusProvider.Policy In oPolicy
                    If Not ((sQuotesInsuranceFileTypes.Contains(policy.InsuranceFileTypeCode.Trim().ToUpper()) And policy.PolicyStatus.Trim().ToUpper() = "CANCELLED") Or policy.IsOutOfSequenceReplaced = True _
                            Or (policy.PolicyStatus.Trim().ToUpper() = "VOIDED" Or policy.InsuranceFileTypeCode.Trim().ToUpper() = "VOID" Or policy.InsuranceFileTypeCode.Trim().ToUpper() = "VOIDREP")) Then
                        'Hide QUOTE versions on live policy when checkbox is unchecked
                        If bHasLivePolicy AndAlso policy.InsuranceFileTypeCode.Trim().ToUpper() = "QUOTE" Then
                            Continue For
                        End If
                        oFilteredPolicy.Add(policy)
                    End If
                Next
            End If

            If Not chkViewAllPolicies.Checked AndAlso oFilteredPolicy IsNot Nothing Then
                'Doing further filteration if client searched by Risk Index
                If ViewState("InsuranceFileKeys") IsNot Nothing AndAlso Not String.IsNullOrEmpty(ViewState("InsuranceFileKeys")) Then
                    Dim oInsuranceFileCnts() As String = ViewState("InsuranceFileKeys").ToString.Split(",")
                    Dim oFilterPolicies = From oPolicies As NexusProvider.Policy In oFilteredPolicy Where oInsuranceFileCnts.Contains(oPolicies.InsuranceFileKey) Select oPolicies
                    If oFilterPolicies.Count > 0 Then
                        Dim oFilterPoliciesColl As New NexusProvider.PolicyCollection
                        For Each row As NexusProvider.Policy In oFilterPolicies
                            oFilterPoliciesColl.Add(row)
                        Next
                        oFilteredPolicy = oFilterPoliciesColl
                    End If
                End If
                DoInitialPagingSetttings(oFilteredPolicy.Count)
                gvPolicyVersions.DataSource = oFilteredPolicy
                gvPolicyVersions.DataBind()
                ViewState.Add(CNFilteredPolicyCollection, oFilteredPolicy)

            ElseIf oPolicy IsNot Nothing Then
                'Doing further filteration if client searched by Risk Index
                If ViewState("InsuranceFileKeys") IsNot Nothing AndAlso Not String.IsNullOrEmpty(ViewState("InsuranceFileKeys")) Then
                    Dim oInsuranceFileCnts() As String = ViewState("InsuranceFileKeys").ToString.Split(",")
                    Dim oFilterPolicies = From oPolicies As NexusProvider.Policy In oPolicy Where oInsuranceFileCnts.Contains(oPolicies.InsuranceFileKey) Select oPolicies
                    If oFilterPolicies.Count > 0 Then
                        Dim oFilterPoliciesColl As New NexusProvider.PolicyCollection
                        For Each row As NexusProvider.Policy In oFilterPolicies
                            oFilterPoliciesColl.Add(row)
                        Next
                        oPolicy = oFilterPoliciesColl
                    End If
                End If
                DoInitialPagingSetttings(oPolicy.Count)
                gvPolicyVersions.DataSource = oPolicy
                gvPolicyVersions.DataBind()
                ViewState.Add(CNFilteredPolicyCollection, oPolicy)
            End If

            ISReinstateLink()
            If gvPolicyVersions.Rows.Count > 0 Then
                gvPolicyVersions.SelectedIndex = 0
                gvPolicyVersions_SelectedIndexChanged(sender, Nothing)
                ctrlLetterWriting.Visible = True
            End If


        End Sub

        ''' <summary>
        ''' To enable paging for policy collections having policy count more than the page size of the grid
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub DoInitialPagingSetttings(ByVal iRowCount As Integer)
            If iRowCount > gvPolicyVersions.PageSize Then
                gvPolicyVersions.AllowPaging = True
                gvPolicyVersions.PageIndex = 0
            Else
                gvPolicyVersions.AllowPaging = False
            End If
        End Sub
        ''' <summary>
        ''' Event to be fired on the selection of a policy version
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPolicyVersions_SelectedIndexChanged(sender As Object, e As EventArgs)
            btnBuy.Visible = False
            btnChange.Visible = False
            btnClaim.Visible = False
            btnCopy.Visible = False
            btnDetails.Visible = False
            btnEdit.Visible = False
            btnReinstatement.Visible = False
            btnView.Visible = False
            btnVoid.Visible = False
            btnVoid.Attributes.Remove("OnClick")
            Dim iInsuranceFileKey As Integer = gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(2)
            Dim sRetainPolicyNumberOnCopy As String = ""
            Dim sPolicyNumberAtQuote As String = ""
            Session(CNInsuranceFileKey) = iInsuranceFileKey
            oSelectedQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey)

            'Calling Risk Detail for calculation purpose
            oWebService.GetHeaderAndRisksByKey(oSelectedQuote, Nothing)

            Session(CNQuote) = oSelectedQuote

            If oSelectedQuote IsNot Nothing Then
                'filterRisk here
                If ViewState("RiskKeys") IsNot Nothing AndAlso Not String.IsNullOrEmpty(ViewState("RiskKeys")) Then
                    Dim oRiskCnts() As String = ViewState("RiskKeys").ToString.Split(",")
                    Dim oFilterRisks = From oRisk As NexusProvider.Risk In oSelectedQuote.Risks Where oRiskCnts.Contains(oRisk.Key) Select oRisk
                    Dim oFilterRiskColl As New NexusProvider.RiskCollection
                    For Each row As NexusProvider.Risk In oFilterRisks
                        oFilterRiskColl.Add(row)
                    Next
                    oSelectedQuote.Risks = oFilterRiskColl
                End If
                gvRisks.DataSource = oSelectedQuote.Risks
                gvRisks.DataBind()
                ViewState.Add(CNSelectedQuote, oSelectedQuote)
                sRetainPolicyNumberOnCopy = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RetainPolicyNumberonCopy, NexusProvider.RiskTypeOptions.Code, oSelectedQuote.ProductCode, "")
                sPolicyNumberAtQuote = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.PolicyNumberAtQuote, NexusProvider.RiskTypeOptions.Code, oSelectedQuote.ProductCode, "")
            End If

            If UserCanDoTask("OpenClaim") Then
                btnClaim.Visible = True
            End If
            Dim bIsReferred As Boolean = Convert.ToBoolean((From oRisks As NexusProvider.Risk In oSelectedQuote.Risks Where oRisks.StatusCode = "REFERRED" Select oRisks).Count)

            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                Session(CNIsMarketPlacePolicy) = True
            Else
                Session(CNIsMarketPlacePolicy) = False
            End If

            Select Case UCase(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("InsuranceFileTypeCode").Trim())

                Case "POLICY"
                    
                    '' need to check if the Policy has been CANCELLED then can't allow POLICY CHANGE again.
                    If UserCanDoTask("CopyPolicy") Then
                        btnCopy.Visible = True
                        btnCopy.CommandArgument = iInsuranceFileKey
                        btnCopy.CommandName = "CopyPolicy"

                        If Not IsNothing(sPolicyNumberAtQuote) AndAlso Not String.IsNullOrEmpty(sPolicyNumberAtQuote) AndAlso sPolicyNumberAtQuote.Trim = "1" Then
                            If Not String.IsNullOrEmpty(sRetainPolicyNumberOnCopy) AndAlso sRetainPolicyNumberOnCopy.Trim = "1" Then
                                btnCopy.Attributes.Add("OnClick", "javascript:return RetainPolicyNumberConfirmation();")
                            End If
                        End If
                    End If

                    If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsCurrent")) Then
                        ShowHideVoidButton(iInsuranceFileKey)
                        If UserCanDoTask("MidTermAdjustment") Then
                            If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "LAP" Then
                                'Disable Temporary MTA system option
                                Dim oDisableTemporaryMTAOptionSettings As NexusProvider.OptionTypeSetting
                                Try
                                    oWebService = New NexusProvider.ProviderManager().Provider
                                    oDisableTemporaryMTAOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.DisableTempMTAs)
                                Finally
                                    oWebService = Nothing
                                End Try
                                'Allow Permanent MTA on lapsed quote if "DISABLE TEMP MTA" system option is ON
                                If oDisableTemporaryMTAOptionSettings.OptionValue = "1" Then
                                    btnChange.CommandArgument = iInsuranceFileKey
                                    btnChange.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                    btnChange.CommandName = "MTAquote"
                                    btnChange.Visible = True
                                End If
                                'replace status from "Policy" to "Lapsed"
                                CType(gvPolicyVersions.SelectedRow.FindControl("lbl_Status"), Label).Text = GetLocalResourceObject("lbl_Lapsed")
                            Else
                                'if AllowMTA then only user will be able to see option CHANGE                                

                                'Only Allow MTA if User Has Authority to do that
                                If Convert.ToBoolean(IsRenewed(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("Reference"), gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("CoverStartDate"), gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("InsuranceFileKey"))) = False Then
                                    If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                        btnChange.CommandArgument = iInsuranceFileKey
                                        btnChange.CommandName = "MTAquote"
                                        btnChange.Visible = True
                                        'This code is added for unmarking the quote for collection
                                        If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("MarkedQuoteForCollection")) Then
                                            btnChange.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                        End If
                                        If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                                            btnChange.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewpolicy"
                        btnView.Visible = True
                    ElseIf Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(5)).Trim().ToUpper() = "CAN" _
                            Or Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(5)).Trim().ToUpper() = "REN" Then
                        'if the plocy has been cancelled then only one link i.e VIEW
                        If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(5)).Trim().ToUpper() = "REN" AndAlso Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsCurrent")) Then
                            If IsInRenewal(Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(8)).Trim()) = True Then 'reference
                                If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                    btnChange.CommandArgument = iInsuranceFileKey
                                    btnChange.CommandName = "MTAquote"
                                    btnChange.Visible = True
                                    'This code is added for unmarking the quote for collection
                                    If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(4)) Then
                                        btnChange.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                    End If
                                    If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                                        btnChange.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                    End If
                                    'end
                                End If
                            End If
                        End If

                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewMTA"
                        btnView.Visible = True
                        'Various cases copied from clientquotes.ascx.vb for safe coding as future perspective
                    ElseIf Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(5)).Trim().ToUpper() = "LAP" Then

                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewpolicy"
                        btnView.Visible = True
                        CType(gvPolicyVersions.SelectedRow.FindControl("lbl_Status"), Label).Text = GetLocalResourceObject("lbl_Lapsed")

                    ElseIf Convert.ToBoolean(IsRenewed(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("Reference"), gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("CoverStartDate"), gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("InsuranceFileKey"))) = True Then
                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewpolicy"
                        btnView.Visible = True
                    Else
                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewpolicy"
                        btnView.Visible = True
                    End If

                Case "QUOTE"

                    'Check if any non-quote policy version is in LIVE state
                    Dim bAnyVersionLive As Boolean = False
                    Dim sQuoteTypesCodes As String = "QUOTE,MTAQUOTE,MTAQTETEMP,MTAQREINS,MTAQCAN,RENEWAL,WRITTEN"
                    Dim oAllPolicyVersions As NexusProvider.PolicyCollection = ViewState(CNFilteredPolicyCollection)
                    If oAllPolicyVersions IsNot Nothing Then
                        For Each oPolVersion As NexusProvider.Policy In oAllPolicyVersions
                            If oPolVersion.PolicyStatusCode IsNot Nothing AndAlso oPolVersion.PolicyStatusCode.Trim().ToUpper() = "LIVE" _
                                AndAlso oPolVersion.InsuranceFileTypeCode IsNot Nothing _
                                AndAlso Not sQuoteTypesCodes.Contains(oPolVersion.InsuranceFileTypeCode.Trim().ToUpper()) Then
                                bAnyVersionLive = True
                                Exit For
                            End If
                        Next
                    End If

                    If bAnyVersionLive Then
                        'When any version is LIVE, only show View button for quotation
                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewpolicy"
                        btnView.Visible = True
                        btnEdit.Visible = False
                        btnBuy.Visible = False
                        btnCopy.Visible = False
                        btnClaim.Visible = False
                    Else
                        Dim dExpiryDate As Date = Convert.ToDateTime(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("QuoteExpiryDate"))
                        btnCopy.Attributes.Remove("OnClick")
                        If UserCanDoTask("NewBusiness") AndAlso Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() <> "LAP" Then

                            If (dExpiryDate < DateTime.Today) Then
                                btnView.CommandArgument = iInsuranceFileKey
                                btnView.CommandName = "viewpolicy"
                                btnView.Visible = True
                            Else
                                btnEdit.CommandArgument = iInsuranceFileKey
                                btnEdit.CommandName = "editquote"
                                btnEdit.Visible = True
                                If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) AndAlso Not bIsReferred Then
                                    btnEdit.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                End If
                            End If
                            'This code is added for unmarking the quote for collection
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(4)) Then
                                btnEdit.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                            Else
                                If (oSelectedQuote.QuoteExpiryDate < DateTime.Now) Then
                                    btnEdit.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                                    btnBuy.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                                End If
                            End If
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) AndAlso Not bIsReferred Then
                                btnBuy.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                            Else
                                If (oSelectedQuote.QuoteExpiryDate < DateTime.Now) Then
                                    btnEdit.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                                    btnBuy.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                                End If
                            End If
                            'end
                            If Not UserCanDoTask("DisableBuyNow") Then
                                btnBuy.CommandArgument = iInsuranceFileKey
                                btnBuy.CommandName = "buyquote"
                                btnBuy.Visible = True
                            End If
                        ElseIf UserCanDoTask("ViewQuote") Then
                            btnView.CommandArgument = iInsuranceFileKey
                            btnView.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            btnView.CommandName = "viewpolicy"
                            btnView.Visible = True

                        End If
                        If Session(CNLoginType) = LoginType.Agent And UserCanDoTask("CopyQuote") Then

                            'Copy link will be available only for agents, if has Authority
                            'Make the column and link available to user
                            btnCopy.Visible = True
                            btnCopy.CommandArgument = iInsuranceFileKey
                            btnCopy.CommandName = "CopyQuote"

                        End If
                        'claim not allowed on quotations
                        btnClaim.Visible = False
                    End If





                Case "WRITTEN"
                    If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(10)).Trim() IsNot Nothing Then 'Event Desc

                        'Edit/Buy options will be available only if user has Authority
                        If UserCanDoTask("NewBusiness") And Not Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(10)).Trim().Contains("Written Renewal") Then
                            'code commented to hide the edit button for Written Policy status

                            'This code is added for unmarking the quote for collection
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(4)) Then
                                btnEdit.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                btnEdit.Visible = True
                                If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                                    btnEdit.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                End If
                            End If
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                                btnBuy.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                            End If
                            'end
                            If Not UserCanDoTask("DisableBuyNow") Then
                                btnBuy.CommandArgument = iInsuranceFileKey
                                btnBuy.CommandName = "buyquote"
                                btnBuy.Visible = True
                            End If
                        End If
                        If UserCanDoTask("ViewQuote") Then
                            btnView.CommandArgument = iInsuranceFileKey
                            btnView.CommandName = "viewpolicy"
                            btnView.Visible = True
                        End If
                    End If

                Case "MTAQUOTE", "MTAQTETEMP", "MTAQCAN"
                    btnClaim.Visible = False
                    btnChange.Visible = False
                    btnEdit.Visible = False
                    btnBuy.Visible = False
                    btnView.Visible = False
                    If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(5)).Trim().ToUpper() = "LIVE" Then

                        If UserCanDoTask("MidTermAdjustment") Then
                            btnEdit.CommandArgument = iInsuranceFileKey
                            btnEdit.CommandName = "editmtaquote"
                            btnEdit.Visible = True
                            'This code is added for unmarking the quote for collection
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(4)) Then
                                btnEdit.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                            Else
                                If (oSelectedQuote.QuoteExpiryDate < DateTime.Now) Then
                                    btnEdit.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                                End If
                            End If
                            'end
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) AndAlso Not bIsReferred Then
                                btnEdit.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                btnBuy.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                            Else
                                If (oSelectedQuote.QuoteExpiryDate < DateTime.Now) Then
                                    btnEdit.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                                End If
                            End If
                            If Not UserCanDoTask("DisableBuyNow") Then
                                btnBuy.CommandArgument = iInsuranceFileKey
                                btnBuy.CommandName = "buymtaquote"
                                btnBuy.Visible = True
                            End If

                            If UserCanDoTask("ViewQuote") Then
                                btnView.CommandArgument = iInsuranceFileKey
                                btnView.CommandName = "viewMTA"
                                btnView.Visible = True
                            End If
                        End If

                    ElseIf Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(5)).Trim().ToUpper() = "CAN" _
                        Or Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(5)).Trim().ToUpper() = "LAP" _
                        Or Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(5)).Trim().ToUpper() = "REP" Then
                        'if the plocy has been cancelled then only one link i.e VIEW
                        If UserCanDoTask("ViewQuote") Then
                            btnView.CommandArgument = iInsuranceFileKey
                            btnView.CommandName = "viewMTA"
                            btnView.Visible = True
                        End If

                    End If
                    'claim not allowed on quotations
                    btnClaim.Visible = False
                Case "MTAQREINS" '
                    btnClaim.Visible = False
                    'Only Allow MTA if User Has Authority to do that

                    '' need to check if the Policy has been CANCELLED then can't allow POLICY CHANGE again.
                    If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(5)).Trim().ToUpper() = "LIVE" Then

                        If UserCanDoTask("MidTermReinstatement") Then
                            btnEdit.CommandArgument = iInsuranceFileKey
                            btnEdit.CommandName = "editmtaquote"
                            btnEdit.Visible = True
                            'This code is added for unmarking the quote for collection
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(4)) Then
                                btnEdit.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                            Else
                                If (oSelectedQuote.QuoteExpiryDate < DateTime.Now) Then
                                    btnEdit.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                                End If
                            End If
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) AndAlso Not bIsReferred Then
                                btnEdit.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                btnBuy.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                            Else
                                If (oSelectedQuote.QuoteExpiryDate < DateTime.Now) Then
                                    btnEdit.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                                End If
                            End If
                            'end
                            If Not UserCanDoTask("DisableBuyNow") Then
                                btnBuy.CommandArgument = iInsuranceFileKey
                                btnBuy.CommandName = "buymtaquote"
                                btnBuy.Visible = True
                            End If

                            If (oSelectedQuote.QuoteExpiryDate < DateTime.Now) Then
                                btnBuy.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                            End If

                        End If

                    Else
                        'if the plocy has been cancelled then only one link i.e VIEW
                        If UserCanDoTask("ViewQuote") Then
                            btnView.CommandArgument = iInsuranceFileKey
                            btnView.CommandName = "viewMTA"
                            btnView.Visible = True
                        End If

                    End If
                    'claim not allowed on quotations
                    btnClaim.Visible = False
                Case "MTA PERM", "MTA TEMP"

                    If UserCanDoTask("CopyPolicy") Then

                        btnCopy.Visible = True
                        btnCopy.CommandArgument = iInsuranceFileKey
                        btnCopy.CommandName = "CopyPolicy"

                        If Not IsNothing(sPolicyNumberAtQuote) AndAlso Not String.IsNullOrEmpty(sPolicyNumberAtQuote) AndAlso sPolicyNumberAtQuote.Trim = "1" Then
                            If Not String.IsNullOrEmpty(sRetainPolicyNumberOnCopy) AndAlso sRetainPolicyNumberOnCopy.Trim = "1" Then
                                btnCopy.Attributes.Add("OnClick", "javascript:return RetainPolicyNumberConfirmation();")
                            End If
                        End If
                    End If
                    Select Case Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("InsuranceFileTypeCode")).Trim().ToUpper() 'InsuranceFileTypeCode
                        Case "MTA PERM"
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsCurrent")) Then
                                ShowHideVoidButton(iInsuranceFileKey)
                                If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                    If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "LAP" Then
                                        'Disable Temporary MTA system option
                                        Dim oDisableTemporaryMTAOptionSettings As NexusProvider.OptionTypeSetting
                                        Try
                                            oWebService = New NexusProvider.ProviderManager().Provider
                                            oDisableTemporaryMTAOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.DisableTempMTAs)
                                        Finally
                                            oWebService = Nothing
                                        End Try
                                        'WPR 59 - Allow Permanent MTA on lapsed quote if "DISABLE TEMP MTA" system option is ON
                                        If oDisableTemporaryMTAOptionSettings.OptionValue = "1" Then
                                            btnChange.CommandArgument = iInsuranceFileKey
                                            btnChange.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                            btnChange.CommandName = "MTAquote"
                                            btnChange.Visible = True
                                        End If
                                    Else
                                        btnChange.CommandArgument = iInsuranceFileKey
                                        btnChange.CommandName = "MTAquote"
                                        btnChange.Visible = True
                                    End If
                                    If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                                        btnChange.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                    End If
                                End If

                            ElseIf Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "CAN" _
                            Or Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "REN" Then 'PolicystatusCdoe
                                'if the plocy has been cancelled then only one link i.e VIEW
                                If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "REN" Then
                                    If IsInRenewal(Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("Reference")).Trim()) = True Then 'reference
                                        If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                            btnChange.CommandArgument = iInsuranceFileKey
                                            btnChange.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                            btnChange.CommandName = "MTAquote"
                                            btnChange.Visible = True
                                            'This code is added for unmarking the quote for collection
                                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("MarkedQuoteForCollection")) Then
                                                btnChange.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                            End If
                                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                                                btnChange.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                            End If
                                            'end
                                        End If
                                    End If
                                End If
                            ElseIf Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "LAP" Then

                                btnView.CommandArgument = iInsuranceFileKey
                                btnView.CommandName = "viewpolicy"
                                btnView.Visible = True
                                CType(gvPolicyVersions.SelectedRow.FindControl("lbl_Status"), Label).Text = GetLocalResourceObject("lbl_Lapsed")
                            End If

                            If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "LAP" Then
                                CType(gvPolicyVersions.SelectedRow.FindControl("lbl_Status"), Label).Text = GetLocalResourceObject("lbl_Lapsed")
                            End If

                        Case "MTA TEMP"

                            ShowHideVoidButton(iInsuranceFileKey)

                    End Select

                    If oPortalConfig.ViewOnlyLatestPolicyVersion = True And Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("InsuranceFileTypeCode")).Trim().ToUpper() = "MTA PERM" _
                      And Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.Rows.Count - 1).Values("PolicyStatusCode")).Trim().ToUpper() <> "CAN" Then
                        If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                            btnChange.CommandArgument = iInsuranceFileKey
                            btnChange.CommandName = "MTAquote"
                            btnChange.Visible = True
                            'Call this for UnMarking the Quote For collection
                            'This code is added for unmarking the quote for collection
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("MarkedQuoteForCollection")) Then
                                btnChange.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                            End If
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                                btnChange.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                            End If
                            'end
                        End If
                    End If

                    btnView.CommandArgument = iInsuranceFileKey
                    btnView.CommandName = "viewMTA"
                    btnView.Visible = True

                Case "MTAREINS"
                    If UserCanDoTask("CopyPolicy") Then

                        btnCopy.Visible = True
                        btnCopy.CommandArgument = iInsuranceFileKey
                        btnCopy.CommandName = "CopyPolicy"

                        If Not IsNothing(sPolicyNumberAtQuote) AndAlso Not String.IsNullOrEmpty(sPolicyNumberAtQuote) AndAlso sPolicyNumberAtQuote.Trim = "1" Then
                            If Not String.IsNullOrEmpty(sRetainPolicyNumberOnCopy) AndAlso sRetainPolicyNumberOnCopy.Trim = "1" Then
                                btnCopy.Attributes.Add("OnClick", "javascript:return RetainPolicyNumberConfirmation();")
                            End If
                        End If
                    End If

                    '' need to check if the Policy has been CANCELLED then can't allow POLICY CHANGE again.
                    If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsCurrent")) Then
                        ShowHideVoidButton(iInsuranceFileKey)
                        If UserCanDoTask("MidTermAdjustment") Then
                            If IsInRenewal(Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("Reference")).Trim()) = True _
                                Or Convert.ToBoolean(IsRenewed(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("Reference"), gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("CoverStartDate"), gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("InsuranceFileKey"))) = False _
                                Or IsReinstated(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("Reference")) = True Then
                                If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                    btnChange.CommandArgument = iInsuranceFileKey
                                    btnChange.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                    btnChange.CommandName = "MTAquote"
                                    btnChange.Visible = True
                                    'This code is added for unmarking the quote for collection
                                    If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("MarkedQuoteForCollection")) Then
                                        btnChange.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                    End If
                                    If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                                        btnChange.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                    End If
                                    'end
                                End If
                            End If
                        End If
                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewpolicy"
                        btnView.Visible = True

                    ElseIf Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "CAN" _
                      Or Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "LAP" _
                      Or Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "REN" _
                      Or Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "REP" Then
                        'if the plocy has been cancelled then only one link i.e VIEW

                        If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "LAP" Then
                            If UserCanDoTask("MidTermReinstatement") Then
                                btnReinstatement.CommandArgument = iInsuranceFileKey
                                btnReinstatement.CommandName = "Reinstatement"
                                btnReinstatement.Visible = True
                            End If
                        Else
                            btnView.CommandArgument = iInsuranceFileKey
                            btnView.CommandName = "viewpolicy"
                            btnView.Visible = True
                        End If
                    ElseIf Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "REN" Then
                        If Convert.ToBoolean(IsInRenewal(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("Reference"))) = True Then
                            If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                btnChange.CommandArgument = iInsuranceFileKey
                                btnChange.CommandName = "MTAquote"
                                btnChange.Visible = True
                                'This code is added for unmarking the quote for collection
                                If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("MarkedQuoteForCollection")) Then
                                    btnChange.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                End If
                                If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMarketPlacePolicy")) Then
                                    btnChange.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                End If
                            End If

                            btnView.CommandArgument = iInsuranceFileKey
                            btnView.CommandName = "viewpolicy"
                            btnView.Visible = True
                        End If
                    ElseIf (Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "LIVE") Then
                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewpolicy"
                        btnView.Visible = True
                    Else
                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewpolicy"
                        btnView.Visible = True
                    End If



                Case "RENEWAL"
                    ShowHideVoidButton(iInsuranceFileKey)
                    If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "LAP" Then 'Migrated LAPSED Policy
                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.Text = GetLocalResourceObject("lbl_view").ToString()
                        btnView.CommandName = "viewpolicy"
                        CType(gvPolicyVersions.SelectedRow.FindControl("lbl_Status"), Label).Text = GetLocalResourceObject("lbl_Lapsed")
                    Else
                        'need to show only one link i.e. "Details"
                        'Check the roles before displaying the "Details" link
                        If UserCanDoTask("Renewals") Then
                            btnDetails.CommandArgument = iInsuranceFileKey
                            btnDetails.CommandName = "viewDetails"
                            btnDetails.Visible = True
                            'This code is added for unmarking the quote for collection
                            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("MarkedQuoteForCollection")) Then
                                btnDetails.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                            End If
                        End If
                    End If
                    If UserCanDoTask("ViewQuote") Then
                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewpolicy"
                        btnView.Visible = True
                    End If
                Case "MTACAN"
                    If UserCanDoTask("CopyPolicy") Then

                        btnCopy.Visible = True
                        btnCopy.CommandArgument = iInsuranceFileKey
                        btnCopy.CommandName = "CopyPolicy"

                        If Not IsNothing(sPolicyNumberAtQuote) AndAlso Not String.IsNullOrEmpty(sPolicyNumberAtQuote) AndAlso sPolicyNumberAtQuote.Trim = "1" Then
                            If Not String.IsNullOrEmpty(sRetainPolicyNumberOnCopy) AndAlso sRetainPolicyNumberOnCopy.Trim = "1" Then
                                btnCopy.Attributes.Add("OnClick", "javascript:return RetainPolicyNumberConfirmation();")
                            End If
                        End If
                    End If
                    If Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "CAN" Then

                        'Now the Reinstatement button will only be shown if user has access to MTR/MTC
                        If IsRenewed(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("Reference"),
                           gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("CoverStartDate"),
                            gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("InsuranceFileKey")) = False _
                        And Convert.ToBoolean(IsInRenewal(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("Reference"))) = False Then
                            If ViewState(CNIsReinstateLink) IsNot Nothing AndAlso ViewState(CNIsReinstateLink) = gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("InsuranceFileKey") Then
                                If UserCanDoTask("MidTermReinstatement") Then
                                    btnReinstatement.CommandArgument = iInsuranceFileKey
                                    btnReinstatement.CommandName = "Reinstatement"
                                    btnReinstatement.Attributes.Add("OnClick", "javascript:return UnReInstatementConfirmation();")
                                    btnReinstatement.Visible = True
                                End If
                            End If
                        End If


                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewMTA"
                        btnView.Visible = True
                    ElseIf Convert.ToString(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("PolicyStatusCode")).Trim().ToUpper() = "REP" Then
                        btnView.CommandArgument = iInsuranceFileKey
                        btnView.CommandName = "viewMTA"
                        btnView.Visible = True
                    End If

                Case "VOID", "VOIDREP", "VOIDRENREP"
                    btnClaim.Visible = False
                    btnView.CommandArgument = iInsuranceFileKey
                    btnView.CommandName = "viewMTA"
                    btnView.Visible = True

                    If UserCanDoTask("CopyPolicy") Then
                        btnCopy.Visible = True
                        btnCopy.CommandArgument = iInsuranceFileKey
                        btnCopy.CommandName = "CopyPolicy"

                        If Not IsNothing(sPolicyNumberAtQuote) AndAlso Not String.IsNullOrEmpty(sPolicyNumberAtQuote) AndAlso sPolicyNumberAtQuote.Trim = "1" Then
                            If Not String.IsNullOrEmpty(sRetainPolicyNumberOnCopy) AndAlso sRetainPolicyNumberOnCopy.Trim = "1" Then
                                btnCopy.Attributes.Add("OnClick", "javascript:return RetainPolicyNumberConfirmation();")
                            End If
                        End If
                    End If

                Case Else

                    btnEdit.Visible = False

            End Select

            Select Case UCase(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("InsuranceFileTypeCode").Trim())
                Case "MTA PERM"
                    btnView.CommandName = "viewMTA"
            End Select

            If (ViewState("PageIndex") IsNot Nothing) Then
                gvPolicyVersions.PageIndex = CInt(ViewState("PageIndex"))
            End If

            If Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsMigratedPolicy")) Then 'Migrated policies are ulways under renewal
                'need to show only one link i.e. "Details"
                'Check the roles before displaying the "Details" link
                If UserCanDoTask("Renewals") Then
                    'Do'nt show view link.
                    btnView.Visible = False
                    btnChange.CommandArgument = iInsuranceFileKey
                    btnChange.Text = GetLocalResourceObject("lbl_details").ToString() '"details"
                    btnChange.CommandName = "viewDetails"
                Else
                    'Do,nt show change link.Only view link should be displayed
                    btnChange.Visible = False
                End If
            ElseIf Convert.ToBoolean(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values("IsReadOnly")) = True Then
                btnView.Visible = True
                btnChange.Visible = False
                btnClaim.Visible = False
                btnDetails.Visible = False
                btnEdit.Visible = False
                btnReinstatement.Visible = False
                btnBuy.Visible = False
            End If
        End Sub
        ''' <summary>
        ''' on the row data bound event of policy version grid, tasks to be performed such as replacing the min date with blank field.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPolicyVersions_RowDataBound(sender As Object, e As GridViewRowEventArgs)
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).Style.Add(HtmlTextWriterStyle.Display, "none")
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Style.Add(HtmlTextWriterStyle.Display, "none")
                e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvPolicyVersions, "Select$" + e.Row.RowIndex.ToString())




                If e.Row.Cells(2).Text = DateTime.MinValue Then
                    e.Row.Cells(2).Text = ""
                Else
                    Dim dt As DateTime = DateTime.Parse(e.Row.Cells(2).Text)

                    Dim dtYear As Int32 = dt.Year
                    If dtYear = 1899 Then
                        e.Row.Cells(2).Text = ""
                    End If
                End If

                If e.Row.Cells(3).Text = DateTime.MinValue Then
                    e.Row.Cells(3).Text = ""
                Else
                    Dim dt As DateTime = DateTime.Parse(e.Row.Cells(3).Text)

                    Dim dtYear As Int32 = dt.Year
                    If dtYear = 1899 Then
                        e.Row.Cells(3).Text = ""
                    End If
                End If

                If e.Row.Cells(4).Text = DateTime.MinValue Then
                    e.Row.Cells(4).Text = ""
                Else
                    Dim dt As DateTime = DateTime.Parse(e.Row.Cells(4).Text)

                    Dim dtYear As Int32 = dt.Year
                    If dtYear = 1899 Then
                        e.Row.Cells(4).Text = ""
                    End If
                End If

                If e.Row.Cells(9).Text = DateTime.MinValue Then
                    e.Row.Cells(9).Text = ""
                Else
                    Dim dt As DateTime = DateTime.Parse(e.Row.Cells(9).Text)

                    Dim dtYear As Int32 = dt.Year
                    If dtYear = 1899 Then
                        e.Row.Cells(9).Text = ""
                    End If
                End If
                Dim sDescription As String = e.Row.Cells(7).Text
                If Not String.IsNullOrWhiteSpace(sDescription) AndAlso sDescription.Length > 0 Then
                    If sDescription.Contains(" - ") = True Then
                        sDescription = sDescription.Remove(0, sDescription.IndexOf("-") + 2).Trim
                        e.Row.Cells(7).Attributes.Add("title", sDescription)
                    End If
                End If

                Select Case UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim())
                    Case "RENEWAL"
                        If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "LAP" Then 'Migrated LAPSED Policy
                        Else
                            CType(e.Row.FindControl("lbl_Status"), Label).Text = GetLocalResourceObject("lbl_RenewalQuote")
                        End If
                End Select
            End If
        End Sub

        ''' <summary>
        ''' Will return the quote grace period for the product whose ProductCode is passed
        ''' </summary>
        ''' <param name="sProductCode"></param>
        ''' <remarks></remarks>
        Protected Function GetQuoteGracePeriod(ByVal sProductCode As String) As String
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oRiskType As NexusProvider.RiskType = Session(CNRiskType)
            Dim sProductPath() As String
            sProductPath = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                       .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)
            Dim oProduct As Config.Product = CType(GetSection("NexusFrameWork"),
             Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products.GetProductByName(Server.UrlDecode(
             Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))
            Dim iGracePeriod As String = ""
            iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, sProductCode, "")
            Return iGracePeriod
        End Function

        ''' <summary>
        ''' To handle the paging event for risks
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRisks_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvRisks.PageIndexChanging
            If ViewState(CNSelectedQuote) IsNot Nothing Then
                Dim oSelectedQuote As New NexusProvider.Quote
                oSelectedQuote = ViewState(CNSelectedQuote)
                gvRisks.PageIndex = e.NewPageIndex
                gvRisks.DataSource = oSelectedQuote.Risks
                gvRisks.DataBind()
            End If

            If gvPolicyVersions.Rows.Count > 0 AndAlso gvPolicyVersions.SelectedIndex = -1 Then
                gvPolicyVersions.SelectedIndex = 0
            End If
            gvPolicyVersions_SelectedIndexChanged(sender, Nothing)

        End Sub
        ''' <summary>
        ''' on the selection on risk in risk grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRisks_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvRisks.RowCommand
            If e.CommandName <> "Page" Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim nCurrentRiskIndex As Integer
                oQuote = oWebService.GetHeaderAndSummariesByKey(gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(2))
                oWebService.GetHeaderAndRisksByKey(oQuote)
                Session(CNQuote) = oQuote
                Session(CNCurrenyCode) = oQuote.CurrencyCode
                Dim bIgnoreLocking As Boolean = False
                If e.CommandName = "viewMTA" OrElse e.CommandName = "viewpolicy" Then
                    bIgnoreLocking = True
                End If

                For iCounter As Integer = 0 To oQuote.Risks.Count - 1
                    If oQuote.Risks(iCounter).Key = CInt(e.CommandArgument) Then
                        Session(CNCurrentRiskKey) = iCounter
                        nCurrentRiskIndex = iCounter
                        'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                        oWebService.GetRisk(oQuote.Risks(iCounter).Key, iCounter, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                        Exit For
                    End If
                Next

                'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                oWebService.GetRisk(CInt(e.CommandArgument), nCurrentRiskIndex, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                Session(CNQuote) = oQuote
                Session(CNQuoteInSync) = True
                Session(CNQuoteMode) = QuoteMode.FullQuote

                Dim sRedirectPath As String = String.Empty
                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name

                Session(CNMode) = Mode.Edit
                Session.Remove(CNOI)
                Session.Remove(CNPolicyAllTaxesColl)
                'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                If oQuote.MarkedQuoteForCollection = True Then
                    oQuote.MarkedQuoteForCollection = False
                    oQuote.MarkedDateforCollection = Date.Now.Date
                    oWebService.UpdateQuotev2(oQuote)
                    Session(CNQuote) = oQuote
                End If

                Dim oRiskType As Config.RiskType
                If oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim)
                Else
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode.Trim)
                End If

                Dim oRisk As New NexusProvider.RiskType
                oRisk.DataModelCode = oRiskType.DataModelCode
                oRisk.Name = oRiskType.Name
                oRisk.Path = oRiskType.Path
                oRisk.RiskCode = oRiskType.RiskCode
                Session(CNRiskType) = oRisk

                If IsDataSetQuickQuote() = False Then
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                Else
                    Session(CNQuoteMode) = QuoteMode.QuickQuote

                End If

                Dim sRiskFolder As String = sProductFolder & "/" & oRisk.Path & "/"
                Dim bMaindetails As String = String.Empty
                Dim sPage As String = GetFirstRiskScreen(sProductFolder & "/" & CType(Session(CNRiskType), NexusProvider.RiskType).Path & "/" & oProduct.FullQuoteConfig, bMaindetails)


                Session(CNMode) = Mode.View
                Session(CNQuoteInSync) = False

                Session(CNDataModelCode) = oRisk.DataModelCode
                If bMaindetails.ToLower = "true" Then
                    sRiskFolder = sProductFolder
                End If

                Session.Remove(CNViewType)

                Select Case btnView.CommandName
                    Case "viewMTA"
                        Session(CNRenewal) = Nothing
                        If oQuote.InsuranceFileTypeCode.Trim() = "MTACAN" Then
                            Session(CNMTAType) = MTAType.CANCELLATION
                            'Hold the View Type of Selected InsuranceFileType
                            Session(CNViewType) = ViewType.CANCELLATION_MTA
                        ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTA PERM" Then
                            'Hold the View Type of Selected InsuranceFileType
                            Session(CNViewType) = ViewType.PERMANENT_MTA
                        ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTA TEMP" Then
                            'Hold the View Type of Selected InsuranceFileType
                            Session(CNViewType) = ViewType.TEMPORARY_MTA
                        ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQUOTE" Then
                            Session(CNViewType) = ViewType.PERMANENT_MTAQUOTE
                        ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQTETEMP" Then
                            Session(CNViewType) = ViewType.TEMPORARY_MTAQUOTE
                        End If
                    Case "viewpolicy"
                        If oQuote.InsuranceFileTypeCode.Trim() = "MTAQUOTE" Then
                            Session(CNViewType) = ViewType.PERMANENT_MTAQUOTE
                        ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQTETEMP" Then
                            Session(CNViewType) = ViewType.TEMPORARY_MTAQUOTE
                        ElseIf oQuote.InsuranceFileTypeCode = "MTAQCAN" Then
                            Session(CNViewType) = ViewType.CANCELLATION_MTAQUOTE
                        End If

                End Select
                Session(CNRiskViewStartPoint) = "ClientManager"
                If String.IsNullOrEmpty(bMaindetails) = False AndAlso bMaindetails.ToLower = "false" Then
                    Response.Redirect(sRiskFolder & sPage, False)
                Else
                    Response.Redirect(sRiskFolder & "/" & sPage, False)
                End If
            End If
        End Sub

        ''' <summary>
        ''' event to be executed on the selected checked change of "view all cancelled policies checkbox
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub chkViewAllPolicies_CheckedChanged(sender As Object, e As EventArgs) Handles chkViewAllPolicies.CheckedChanged
            If Session(CNParty) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then

                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                PartyKey = oParty.Key
            End If
            tvClientPolicy_SelectedNodeChanged(sender, e)
        End Sub

        Protected Sub gvPolicyVersions_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvPolicyVersions.PageIndexChanging
            gvPolicyVersions.PageIndex = e.NewPageIndex
            ViewState("PageIndex") = e.NewPageIndex
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNFilteredPolicyCollection)
            gvPolicyVersions.DataSource = oPolicyCollection
            gvPolicyVersions.DataBind()
            ISReinstateLink()
            gvPolicyVersions.SelectedIndex = 0
            gvPolicyVersions_SelectedIndexChanged(sender, e)

        End Sub

        Sub ShowHideAssocoate()
            Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oAllowPolicyClientAssociationsOptionSettings = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)
            If oAllowPolicyClientAssociationsOptionSettings Is Nothing Then
                oAllowPolicyClientAssociationsOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.AllowPolicyClientAssociations)
                ViewState("AllowPolicyClientAssociationsOptionSettings") = oAllowPolicyClientAssociationsOptionSettings
            End If

            If oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                gvPolicyVersions.Columns(10).Visible = True
            Else
                gvPolicyVersions.Columns(10).Visible = False
            End If
            oWebService = Nothing
        End Sub
        Protected Sub gvPolicyVersions_RowDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPolicyVersions.RowDataBound
            Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
            oAllowPolicyClientAssociationsOptionSettings = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)
            If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    Dim xmldoc As New System.Xml.XmlDocument
                    If ((CType(e.Row.DataItem, NexusProvider.Policy).AssociatedClients IsNot Nothing) AndAlso Not (String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.Policy).AssociatedClients))) Then
                        xmldoc.InnerXml = CType(e.Row.DataItem, NexusProvider.Policy).AssociatedClients

                        Dim rptrFolderNavigation As Repeater = e.Row.FindControl("rptrAssociateClient")
                        If rptrFolderNavigation IsNot Nothing Then
                            rptrFolderNavigation.DataSource = xmldoc.SelectNodes("/Associates/Associate")
                            rptrFolderNavigation.DataBind()
                        End If

                    End If
                    xmldoc = Nothing
                End If
            End If
        End Sub
        ''' <summary>
        ''' to perform sorting on particular seelcted column of the policy version grid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPolicyVersions_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvPolicyVersions.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNFilteredPolicyCollection)
            Dim oNewPolicyCollection As New NexusProvider.PolicyCollection
            For Each oPolicy As NexusProvider.Policy In oPolicyCollection
                If oPolicy.IsCurrent Then
                    oPolicy.PolicyStatus = "Current"
                End If
                If oPolicy.EventDesc = Nothing Then
                    oPolicy.EventDesc = ""
                End If
                oNewPolicyCollection.Add(oPolicy)
            Next

            oNewPolicyCollection.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oNewPolicyCollection.SortingOrder = _sortDirection
            oNewPolicyCollection.Sort()
            CType(sender, GridView).DataSource = oNewPolicyCollection
            CType(sender, GridView).DataBind()
            Dim index As Integer
            index = CType(sender, GridView).PageIndex * CType(sender, GridView).PageSize
            oSelectedQuote = oWebService.GetHeaderAndSummariesByKey(oPolicyCollection.Item(index).InsuranceFileKey)

            'Calling Risk Detail for calculation purpose
            oWebService.GetHeaderAndRisksByKey(oSelectedQuote, Nothing)

            If oSelectedQuote IsNot Nothing Then
                gvRisks.DataSource = oSelectedQuote.Risks
                gvRisks.DataBind()
                ViewState.Add(CNSelectedQuote, oSelectedQuote)
            End If
        End Sub
        ''' <summary>
        ''' To reset all the required session values
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ResetTransactionInSession()
            Session.Remove(CNMTAType)
            Session.Remove(CNMTATypeDesc)
            Session.Remove(CNRenewal)
            Session.Remove(CNRenewalShowPremium)
        End Sub
        ''' <summary>
        ''' Event on the click of edit button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Session.Remove(CNOldPremium) 'Remove the old premium from session
            Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            ClearQuoteCollectionSessionValues()
            ClearQuote()


            Try
                If hvMarketPlacePolicy.Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(hvMarketPlacePolicy.Value) AndAlso hvMarketPlacePolicy.Value.ToString.ToUpper = "FALSE" Then
                    oWebService.UpdateMarketplacePolicyStatus(btnEdit.CommandArgument, Convert.ToBoolean(hvMarketPlacePolicy.Value))
                End If

                oQuote = oWebService.GetHeaderAndSummariesByKey(btnEdit.CommandArgument)
                Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                If oExclusiveLocking.OptionValue = "1" Then
                    Dim sUserName As String = CheckLock(oQuote.InsuranceFolderKey, Session(CNBranchCode).ToString)
                    If sUserName.Trim.Length > 0 Then
                        Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Return
                    Else
                        oQuote = oWebService.GetHeaderAndSummariesByKey(btnEdit.CommandArgument, bExclusiveLock:=True)
                    End If
                End If

                'Locking message is required for edit Mode
                Dim bIgnoreLocking As Boolean = False

                ' Put highest risk key into Session
                If Not oQuote.Risks Is Nothing AndAlso oQuote.Risks.Count > 0 Then
                    If oQuote.Risks.Count > 1 Then
                        For nMaxcount As Integer = 0 To oQuote.Risks.Count - 1
                            'Populate XML dataset For Every model beause it's used further
                            oWebService.GetRisk(oQuote.Risks(nMaxcount).Key, nMaxcount, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                        Next
                    Else

                        'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                        oWebService.GetRisk(oQuote.Risks(0).Key, 0, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                    End If
                    Session(CNCurrentRiskKey) = oQuote.Risks.Count - 1
                End If

                oWebService.GetHeaderAndRisksByKey(oQuote)

                Session(CNQuote) = oQuote

            Catch ex As NexusProvider.NexusException
                'Policy locking error
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200", "1000158", "1000160" 'Policy Locking
                        'Show policy locking error as alert

                        Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", (ex.Errors(0).Detail.Split(":"))(2) + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally

            End Try
            If btnEdit.CommandName = "editquote" Or btnEdit.CommandName = "editmtaquote" Then

                Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean
                Dim sMessage As String = ""
                If bIsPendingCloneTransfer = True Or bIsPendingPortfolioTransfer = True Then
                    If bIsPendingPortfolioTransfer = True Then
                        sMessage = GetLocalResourceObject("msg_PendingPortfolioTransfer")
                    ElseIf bIsPendingCloneTransfer = True Then
                        sMessage = GetLocalResourceObject("msg_PendingClonedTransfer")
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                    Exit Sub
                End If
            End If
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()
            End If
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sRedirectPath As String = String.Empty
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Session(CNMTAType) = Nothing
            Select Case btnEdit.CommandName

                Case "editquote"
                    'Check product option for quote versioning on edit
                    Dim sQuoteVersioning As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                    If Not String.IsNullOrEmpty(sQuoteVersioning) AndAlso sQuoteVersioning.Trim = "1" Then
                        Dim iNewInsuranceFileKey As Integer = oQuote.InsuranceFileKey
                        Dim iNewInsuranceFolderKey As Integer = 0
                        oWebService.CopyQuote(iNewInsuranceFileKey, iNewInsuranceFolderKey, oQuote.BranchCode, v_bIsQuoteVersioning:=True)

                        oQuote = oWebService.GetHeaderAndSummariesByKey(iNewInsuranceFileKey, bExclusiveLock:=True)

                        For i As Integer = 0 To oQuote.Risks.Count - 1
                            oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
                        Next

                        oWebService.GetHeaderAndRisksByKey(oQuote)
                        Session(CNQuote) = oQuote
                        Session(CNInsuranceFileKey) = iNewInsuranceFileKey
                    End If

                    'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                    If hdMarkedtext.Text = "true" And oQuote.MarkedQuoteForCollection Then
                        oQuote.MarkedQuoteForCollection = False
                        oQuote.MarkedDateforCollection = Date.Now.Date
                        oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                        Session(CNQuote) = oQuote
                    Else

                        If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                            oWebService.UpdateQuotev2(oQuote)
                            Session(CNQuote) = oQuote
                        End If
                    End If

                    Session(CNRenewal) = Nothing
                    Session(CNMode) = Mode.Edit
                    Session(CNQuoteInSync) = False
                    Session.Remove(CNOI)
                    Session(CNInsuranceFileKey) = btnEdit.CommandArgument
                    Session(CNQuoteInSync) = False


                    If IsDataSetQuickQuote() = False Then
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                    Else
                        Session(CNQuoteMode) = QuoteMode.QuickQuote

                    End If
                    If oProduct.AllowMultiRisks = False Then
                        Dim oRiskType As New NexusProvider.RiskType
                        Dim oRisk As Config.RiskType
                        Dim sFirstRiskPage As String

                        If oQuote.Risks(0).RiskCode Is Nothing Then
                            oRisk = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskTypeCode.Trim)
                        Else
                            oRisk = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskCode)
                        End If

                        oRiskType.DataModelCode = oRisk.DataModelCode
                        oRiskType.Name = oRisk.Name
                        oRiskType.Path = oRisk.Path
                        oRiskType.RiskCode = oRisk.RiskCode
                        Session(CNRiskType) = oRiskType
                        Dim sScreenCode As String = Nothing
                        If oQuote.Risks(0).ScreenCode IsNot Nothing Then
                            If oQuote.Risks(0).ScreenCode.Trim.Length <> 0 Then
                                sScreenCode = oQuote.Risks(0).ScreenCode
                            End If
                        Else
                            sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
                        End If

                        If Session(CNMTAType) IsNot Nothing And Session(CNRenewal) Is Nothing Then
                            If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                                oQuote.Risks(0).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(0).XMLDataset, Nothing, Nothing, "MTA")
                            ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                                oQuote.Risks(0).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(0).XMLDataset, Nothing, Nothing, "MTC")
                            ElseIf (Session(CNMTAType) = MTAType.REINSTATEMENT) Then
                                oQuote.Risks(0).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(0).XMLDataset, Nothing, Nothing, "MTR")
                            End If
                        ElseIf Session(CNMTAType) Is Nothing And Session(CNRenewal) IsNot Nothing Then
                            oQuote.Risks(0).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(0).XMLDataset, Nothing, Nothing, "REN")
                        Else
                            oQuote.Risks(0).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(0).XMLDataset, Nothing, Nothing, "NB")
                        End If
                        'Get first risk page 
                        sFirstRiskPage = GetFirstRiskScreen(sProductFolder & "/" & oRiskType.Path & "/fullquote.config")
                        sRedirectPath = sProductFolder & "/" & oRiskType.Path & "/" & sFirstRiskPage
                    Else
                        sRedirectPath = "~/secure/PremiumDisplay.aspx"
                    End If

                Case "editmtaquote"
                    'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                    If hdMarkedtext.Text = "true" And oQuote.MarkedQuoteForCollection Then
                        oQuote.MarkedQuoteForCollection = False
                        oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                        Session(CNQuote) = oQuote
                    End If
                    Session(CNRenewal) = Nothing
                    'before proceding BUY MTAQUOTE we need to check if the policy already have existing MTA
                    Session(CNMtaReasonSelected) = Nothing
                    Dim oPolicy As NexusProvider.PolicyCollection
                    Dim TempVar As Integer
                    Dim SelMTAQuoteStartDate, ExistingMTAStartDate As Date
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)
                    SetCurrentMTATypeSession()
                    If Not GetCurrentMTAType = MTAType.TEMPORARY Then
                        For TempVar = 0 To oPolicy.Count - 1
                            If oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTA PERM" Or oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "POLICY" Or
                            oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTACAN" Or oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTAREINS" Then
                                SelMTAQuoteStartDate = oQuote.CoverStartDate
                                ExistingMTAStartDate = oPolicy.Item(TempVar).CoverStartDate
                                If SelMTAQuoteStartDate < ExistingMTAStartDate Then
                                    'if yes then Backdated is true
                                    Session(CNIsBackDatedMTA) = True
                                    Exit For
                                End If
                            End If

                        Next
                    End If
                    Session(CNQuote) = oQuote
                    If oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN" Then
                        Session(CNMTAType) = MTAType.CANCELLATION
                    ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQREINS" Then
                        Session(CNMTAType) = MTAType.REINSTATEMENT
                    ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQTETEMP" Then
                        Session(CNMTAType) = MTAType.TEMPORARY
                    Else
                        Session(CNMTAType) = MTAType.PERMANENT
                    End If
                    Session.Remove(CNOI)
                    Session(CNInsuranceFileKey) = btnEdit.CommandArgument
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Session.Item(CNMode) = Mode.Edit
                    Session(CNQuoteInSync) = False
                    Session(CNMtaReasonSelected) = Nothing

                    Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
                    oAllowPolicyClientAssociationsOptionSettings = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)
                    If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                        Dim oPartyCollection As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                        If Trim(oQuote.PartyKey) <> Trim(oPartyCollection.Key) Then
                            Dim oNewParty As NexusProvider.BaseParty
                            oWebService = New NexusProvider.ProviderManager().Provider
                            oNewParty = oWebService.GetParty(oQuote.PartyKey)
                            Session(CNParty) = oNewParty
                        End If
                    End If
                    sRedirectPath = "~/secure/premiumdisplay.aspx"

            End Select

            If oPortal.EnableMasterClientAssociate = True Then
                Dim oPartyCollection As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                If Trim(oQuote.PartyKey) <> Trim(oPartyCollection.Key) Then
                    Dim oNewParty As NexusProvider.BaseParty
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oNewParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oNewParty
                End If
            End If
            Response.Redirect(sRedirectPath, False)

        End Sub
        ''' <summary>
        ''' On the click of buy button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnBuy_Click(sender As Object, e As EventArgs) Handles btnBuy.Click

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Dim iCurrentRiskKey As Integer
            Session.Remove(CNOldPremium) 'Remove the old premium from session
            Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            ClearQuoteCollectionSessionValues()
            ClearQuote()
            Dim sRedirectPath As String = String.Empty

            Try
                If hvMarketPlacePolicy.Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(hvMarketPlacePolicy.Value) AndAlso hvMarketPlacePolicy.Value.ToString.ToUpper = "FALSE" Then
                    oWebService.UpdateMarketplacePolicyStatus(btnBuy.CommandArgument, Convert.ToBoolean(hvMarketPlacePolicy.Value))
                End If

                oQuote = oWebService.GetHeaderAndSummariesByKey(btnBuy.CommandArgument)
                Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                If oExclusiveLocking.OptionValue = "1" Then
                    Dim sUserName As String = CheckLock(oQuote.InsuranceFolderKey, Session(CNBranchCode).ToString)
                    If sUserName.Trim.Length > 0 Then
                        Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Return
                    End If
                    oQuote = oWebService.GetHeaderAndSummariesByKey(btnBuy.CommandArgument, bExclusiveLock:=True)
                End If
                'Put highest risk key into Session
                For i As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote)
                    iCurrentRiskKey = oQuote.Risks(i).Key
                Next
                oWebService.GetHeaderAndRisksByKey(oQuote)

                Session(CNQuote) = oQuote

            Catch ex As NexusProvider.NexusException
                'Catch Policy Locking error and show error alert
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        'Show policy locking error as alert
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select

            Finally

            End Try
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()

                DataSetFunctions.GetScreens()
            End If

            Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
            oAllowPolicyClientAssociationsOptionSettings = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)
            If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                Dim oPartyCollection As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                If Trim(oQuote.PartyKey) <> Trim(oPartyCollection.Key) Then
                    Dim oNewParty As NexusProvider.BaseParty
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oNewParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oNewParty
                End If
            End If

            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Session(CNMTAType) = Nothing
            Select Case btnBuy.CommandName
                Case "buymtaquote"
                    Session(CNRenewal) = Nothing
                    'before proceding BUY MTAQUOTE we need to check if the policy already have existing MTA
                    Session(CNMtaReasonSelected) = Nothing
                    If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                        oWebService.UpdateQuotev2(oQuote)
                        Session(CNQuote) = oQuote
                    End If
                    Dim oPolicy As NexusProvider.PolicyCollection
                    Dim TempVar As Integer
                    Dim SelMTAQuoteStartDate, ExistingMTAStartDate As Date
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)
                    SetCurrentMTATypeSession()
                    If Not GetCurrentMTAType = MTAType.TEMPORARY Then
                        For TempVar = 0 To oPolicy.Count - 1
                            If oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTA PERM" Or oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "POLICY" Or
                            oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTACAN" Or oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTAREINS" Then
                                SelMTAQuoteStartDate = oQuote.CoverStartDate
                                ExistingMTAStartDate = oPolicy.Item(TempVar).CoverStartDate
                                If SelMTAQuoteStartDate < ExistingMTAStartDate Then
                                    'if yes then User cant proceed
                                    Session(CNIsBackDatedMTA) = True
                                    'Exit Sub
                                End If
                            End If

                        Next
                    End If
                    Session(CNQuote) = oQuote
                    If oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN" Then
                        Session(CNMTAType) = MTAType.CANCELLATION
                    ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQREINS" Then
                        Session(CNMTAType) = MTAType.REINSTATEMENT
                    Else
                        Session(CNMTAType) = MTAType.PERMANENT
                    End If

                    Session.Remove(CNOI)
                    Session(CNInsuranceFileKey) = btnBuy.CommandArgument
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Session.Item(CNMode) = Mode.Buy
                    Session(CNQuoteInSync) = False

                    'sRedirectPath = "~/secure/premiumdisplay.aspx"
                    If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                        sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                    Else
                        sRedirectPath = "~/secure/PremiumDisplay.aspx"
                    End If
                Case "buyquote"
                    Session(CNRenewal) = Nothing
                    Session.Remove(CNOI)
                    Session(CNInsuranceFileKey) = btnBuy.CommandArgument

                    If oQuote.InsuranceFileTypeCode = "WRITTEN" And oQuote.InsuranceFileVersion > 1 Then
                        Session(CNRenewal) = True
                    End If
                    'TO Be Cross Check
                    Session(CNQuoteInSync) = False
                    Dim oRisk As NexusProvider.Risk = oQuote.Risks.FindItemByRiskKey(iCurrentRiskKey)

                    If oRisk IsNot Nothing Then

                        Select Case oRisk.StatusCode
                            Case "DECLINE"
                                'sRedirectPath = "~/declined.aspx"
                                If DataSetFunctions.sDeclineScreen.ToLower = "true" Then
                                    Response.Redirect(DataSetFunctions.sDeclineScreenURL)
                                Else
                                    sRedirectPath = "~/declined.aspx"
                                End If
                            Case "REFER"
                                'sRedirectPath = "~/referred.aspx"
                                If DataSetFunctions.sReferScreen.ToLower = "true" Then
                                    Response.Redirect(DataSetFunctions.sReferScreenURL)
                                Else
                                    sRedirectPath = "~/referred.aspx"
                                End If
                            Case "QUOTED"
                                Session.Item(CNMode) = Mode.Buy
                                Session(CNCurrentRiskKey) = 0
                                If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                                    oWebService.UpdateQuotev2(oQuote)
                                    Session(CNQuote) = oQuote
                                End If
                                If IsDataSetQuickQuote() = True Then
                                    If CheckRefer() = True Then
                                        Session(CNQuoteMode) = QuoteMode.FullQuote
                                        If DataSetFunctions.sReferScreen.ToLower = "true" Then
                                            Response.Redirect(DataSetFunctions.sReferScreenURL)
                                        Else
                                            Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                                        End If
                                        'Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                                    ElseIf CheckDecline() = True Then
                                        Session(CNQuoteMode) = QuoteMode.FullQuote
                                        If DataSetFunctions.sDeclineScreen.ToLower = "true" Then
                                            Response.Redirect(DataSetFunctions.sDeclineScreenURL)
                                        Else
                                            Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
                                        End If
                                        'Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
                                    Else
                                        Session(CNQuoteMode) = QuoteMode.FullQuote
                                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                                        Else
                                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                                        End If
                                    End If
                                Else
                                    Session(CNQuoteMode) = QuoteMode.QuickQuote
                                    sRedirectPath = "~/QQPremium.aspx"
                                End If
                            Case Else
                                Session.Item(CNMode) = Mode.Buy
                                Session(CNCurrentRiskKey) = 0
                                If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                                    oWebService.UpdateQuotev2(oQuote)
                                    Session(CNQuote) = oQuote
                                End If
                                If IsDataSetQuickQuote() = False Then
                                    If CheckRefer() = True Then
                                        Session(CNQuoteMode) = QuoteMode.FullQuote
                                        'Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                                        If DataSetFunctions.sReferScreen.ToLower = "true" Then
                                            Response.Redirect(DataSetFunctions.sReferScreenURL)
                                        Else
                                            Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                                        End If
                                    ElseIf CheckDecline() = True Then
                                        Session(CNQuoteMode) = QuoteMode.FullQuote
                                        If DataSetFunctions.sDeclineScreen.ToLower = "true" Then
                                            Response.Redirect(DataSetFunctions.sDeclineScreenURL)
                                        Else
                                            Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
                                        End If
                                        'Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
                                    Else
                                        Session(CNQuoteMode) = QuoteMode.FullQuote
                                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                                        Else
                                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                                        End If
                                    End If
                                Else
                                    Session(CNQuoteMode) = QuoteMode.QuickQuote
                                    sRedirectPath = "~/QQPremium.aspx"
                                End If
                        End Select
                        oRisk = Nothing
                    End If
            End Select
            Response.Redirect(sRedirectPath, False)
        End Sub
        ''' <summary>
        ''' On the click of change button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Dim iCurrentRiskKey As Integer
            Session.Remove(CNOldPremium) 'Remove the old premium from session
            Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            ClearQuoteCollectionSessionValues()
            ClearQuote()
            Dim bExclusiveLock As Boolean = True

            Try
                If hvMarketPlacePolicy.Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(hvMarketPlacePolicy.Value) AndAlso hvMarketPlacePolicy.Value.ToString.ToUpper = "FALSE" Then
                    oWebService.UpdateMarketplacePolicyStatus(btnChange.CommandArgument, Convert.ToBoolean(hvMarketPlacePolicy.Value))
                End If
                oQuote = oWebService.GetHeaderAndSummariesByKey(btnChange.CommandArgument)
                Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                If oExclusiveLocking.OptionValue = "1" Then
                    Dim sUserName As String = CheckLock(oQuote.InsuranceFolderKey, Session(CNBranchCode).ToString)
                    If sUserName.Trim.Length > 0 Then
                        Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Return
                    End If
                    oQuote = oWebService.GetHeaderAndSummariesByKey(btnChange.CommandArgument, bExclusiveLock:=True)
                End If

                'Put highest risk key into Session
                For i As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote)
                    iCurrentRiskKey = oQuote.Risks(i).Key
                Next
                oWebService.GetHeaderAndRisksByKey(oQuote)
                Session(CNQuote) = oQuote
            Catch ex As NexusProvider.NexusException
                'Policy locking error
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        'Show policy locking error as alert
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case "1000148", "1000158" 'Policy Locking
                        'Show policy locking error as alert
                        Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", (ex.Errors(0).Detail.Split(":"))(2) + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally
            End Try
           
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                Session(CNCurrentRiskKey) = 0
                GetDataSetDefinition()
            End If
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sRedirectPath As String = String.Empty
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Session(CNMTAType) = Nothing
            Select Case btnChange.CommandName
                Case "MTAquote"
                    'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                    If hdMarkedtext.Text = "true" And oQuote.MarkedQuoteForCollection Then
                        oQuote.MarkedQuoteForCollection = False
                        oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                        Session(CNQuote) = oQuote
                    End If
                    Session(CNMode) = Mode.Edit
                    Session.Remove(CNOI)
                    Session(CNRenewal) = Nothing
                    Session(CNInsuranceFileKey) = btnChange.CommandArgument
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Session(CNQuoteInSync) = False
                    Session(CNMtaReasonSelected) = Nothing
                    sRedirectPath = "~/secure/MTAReason.aspx"
            End Select
            Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
            oAllowPolicyClientAssociationsOptionSettings = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)
            If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then

                Dim oPartyCollection As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                If Trim(oQuote.PartyKey) <> Trim(oPartyCollection.Key) Then
                    Dim oNewParty As NexusProvider.BaseParty
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oNewParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oNewParty
                End If
            End If
            Response.Redirect(sRedirectPath, False)
        End Sub
        ''' <summary>
        ''' On the click of view button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Dim iCurrentRiskKey As Integer
            Session.Remove(CNOldPremium) 'Remove the old premium from session
            Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            ClearQuoteCollectionSessionValues()
            ClearQuote()

            'Pure 3.0 ---- WPR 41
            Dim sRedirectPath As String = String.Empty
            Try
                Session(CNPolicyBackButton) = "ClientView"
                oQuote = oWebService.GetHeaderAndSummariesByKey(btnView.CommandArgument)
                'Put highest risk key into Session
                For i As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, v_bIgnoreLocking:=True)
                    iCurrentRiskKey = oQuote.Risks(i).Key
                Next
                oWebService.GetHeaderAndRisksByKey(oQuote)
                Session(CNQuote) = oQuote
            Catch ex As NexusProvider.NexusException
                'Catch Policy Locking error and show error alert
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        'Show policy locking error as alert
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally

            End Try
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()
                DataSetFunctions.GetScreens()
            End If
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Session(CNMTAType) = Nothing
            Session(CNViewType) = Nothing
            Select Case btnView.CommandName
                Case "viewMTA"
                    Session(CNRenewal) = Nothing
                    If oQuote.InsuranceFileTypeCode.Trim = "MTACAN" Then
                        Session(CNMTAType) = MTAType.CANCELLATION
                        'Hold the View Type of Selected InsuranceFileType
                        Session(CNViewType) = ViewType.CANCELLATION_MTA
                    ElseIf RTrim(oQuote.InsuranceFileTypeCode) = "MTA PERM" Then
                        'Hold the View Type of Selected InsuranceFileType
                        Session(CNViewType) = ViewType.PERMANENT_MTA
                        Session(CNMTAType) = MTAType.PERMANENT
                    ElseIf oQuote.InsuranceFileTypeCode.Trim = "MTA TEMP" Then
                        'Hold the View Type of Selected InsuranceFileType
                        Session(CNViewType) = ViewType.TEMPORARY_MTA
                        Session(CNMTAType) = MTAType.TEMPORARY
                    ElseIf oQuote.InsuranceFileTypeCode.Trim = "MTAQUOTE" Then
                        Session(CNViewType) = ViewType.PERMANENT_MTAQUOTE
                        Session(CNMTAType) = MTAType.PERMANENT
                    End If
                    Session(CNMode) = Mode.View
                    Session.Remove(CNOI)
                    Session(CNQuoteInSync) = False
                    Session(CNQuoteMode) = QuoteMode.MTAQuote
                    If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                        sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                    Else
                        sRedirectPath = "~/secure/PremiumDisplay.aspx"
                    End If
                Case "viewpolicy"
                    Session(CNRenewal) = Nothing
                    Session(CNMode) = Mode.View
                    Session.Remove(CNOI)
                    Session.Remove(CNQuoteMode)
                    Session(CNQuoteInSync) = False
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    'WILL IT BE PREMIUM DISPLAY FOR FULL QUOTE ALWAYS????
                    'DO WE NEED TO ADD POLICY DETAILS TO SESSION? HOW WILL THE PREMIUM DISPLAY PAGE GET THE POLICY NUMBER FOR WHICH THE DETAILS NEEDS TO FETCH?
                    If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                        sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                    Else
                        sRedirectPath = "~/secure/PremiumDisplay.aspx"
                    End If
            End Select
            Response.Redirect(sRedirectPath, False)
        End Sub
        ''' <summary>
        ''' On the click of Details button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnDetails_Click(sender As Object, e As EventArgs) Handles btnDetails.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Session.Remove(CNOldPremium) 'Remove the old premium from session
            Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            ClearQuoteCollectionSessionValues()
            ClearQuote()
            Try
                oQuote = oWebService.GetHeaderAndSummariesByKey(btnDetails.CommandArgument)

                Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                If oExclusiveLocking.OptionValue = "1" Then
                    Dim sUserName As String = CheckLock(oQuote.InsuranceFolderKey, Session(CNBranchCode).ToString)
                    If sUserName.Trim.Length > 0 Then
                        Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Return
                    Else
                        oQuote = oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=btnDetails.CommandArgument, bExclusiveLock:=True)
                    End If
                End If

                'Clear mark quote collection status
                If oQuote.MarkedQuoteForCollection = True Then
                    oQuote.MarkedQuoteForCollection = False
                    oQuote.MarkedDateforCollection = Date.Now.Date
                    oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                    Session(CNQuote) = oQuote
                End If
                'Locking message is required for details Mode
                Dim bIgnoreLocking As Boolean = False
                ' Put highest risk key into Session
                If Not oQuote.Risks Is Nothing AndAlso oQuote.Risks.Count > 0 Then
                    'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                    For i As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                    Next
                    Session(CNCurrentRiskKey) = oQuote.Risks.Count - 1
                End If

                oWebService.GetHeaderAndRisksByKey(oQuote)
                Session(CNQuote) = oQuote
            Catch ex As NexusProvider.NexusException
                'Policy locking error
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        'Show policy locking error as alert
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally

            End Try
            If btnDetails.CommandName = "viewDetails" Then
                Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean
                oWebService.IsPendingTransfer(oQuote.InsuranceFileKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, oQuote.InsuranceFileRef, oQuote.BranchCode)
                Dim sMessage As String = ""
                If bIsPendingCloneTransfer = True Or bIsPendingPortfolioTransfer = True Then
                    If bIsPendingPortfolioTransfer = True Then
                        sMessage = GetLocalResourceObject("msg_PendingPortfolioTransfer")
                    ElseIf bIsPendingCloneTransfer = True Then
                        sMessage = GetLocalResourceObject("msg_PendingClonedTransfer")
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                    Exit Sub
                End If
            End If
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()
            End If
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sRedirectPath As String = String.Empty
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Session(CNMTAType) = Nothing
            Select Case btnDetails.CommandName
                Case "viewDetails" 'Renewal Policy is being viewed
                    ResetTransactionInSession()
                    Session(CNMode) = Mode.Buy
                    Session.Remove(CNOI)
                    Session(CNRenewal) = True
                    Session.Remove(CNQuoteMode)
                    Session(CNQuoteInSync) = False
                    Session(CNQuoteMode) = QuoteMode.FullQuote

                    sRedirectPath = "~/secure/PremiumDisplay.aspx"
                    If oPortal.EnableMasterClientAssociate = True Then
                        Dim oPartyCollection As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                        If Trim(oQuote.PartyKey) <> Trim(oPartyCollection.Key) Then
                            Dim oNewParty As NexusProvider.BaseParty
                            oWebService = New NexusProvider.ProviderManager().Provider
                            oNewParty = oWebService.GetParty(oQuote.PartyKey)
                            Session(CNParty) = oNewParty
                        End If
                    End If
            End Select

            Response.Redirect(sRedirectPath, False)
        End Sub
        ''' <summary>
        ''' On the click of Re-instatement button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnReinstatement_Click(sender As Object, e As EventArgs) Handles btnReinstatement.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Session.Remove(CNOldPremium) 'Remove the old premium from session
            Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            Try
                Dim nInsuranceFileKey As Integer = btnReinstatement.CommandArgument
                oQuote = oWebService.GetHeaderAndSummariesByKey(nInsuranceFileKey)

                Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                If oExclusiveLocking.OptionValue = "1" Then
                    Dim sUserName As String = CheckLock(oQuote.InsuranceFolderKey, Session(CNBranchCode).ToString)
                    If sUserName.Trim.Length > 0 Then
                        Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Return
                    Else
                        oQuote = oWebService.GetHeaderAndSummariesByKey(nInsuranceFileKey, bExclusiveLock:=True)
                    End If
                End If

                'Locking message is required for reinstatement Mode
                Dim bIgnoreLocking As Boolean = False

                ' Put highest risk key into Session
                If Not oQuote.Risks Is Nothing AndAlso oQuote.Risks.Count > 0 Then
                    'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                    For i As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                    Next

                    Session(CNCurrentRiskKey) = 0
                End If

                oWebService.GetHeaderAndRisksByKey(oQuote)

                Session(CNQuote) = oQuote

            Catch ex As NexusProvider.NexusException
                'Policy locking error
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        'Show policy locking error as alert
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally
                'oWebService = Nothing
            End Try
            If btnReinstatement.CommandName = "Reinstatement" Then
                Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean
                oWebService.IsPendingTransfer(oQuote.InsuranceFileKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, oQuote.InsuranceFileRef, oQuote.BranchCode)
                Dim sMessage As String = ""
                If bIsPendingCloneTransfer = True Or bIsPendingPortfolioTransfer = True Then
                    If bIsPendingPortfolioTransfer = True Then
                        sMessage = GetLocalResourceObject("msg_PendingPortfolioTransfer")
                    ElseIf bIsPendingCloneTransfer = True Then
                        sMessage = GetLocalResourceObject("msg_PendingClonedTransfer")
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                    Exit Sub
                End If
            End If
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()
            End If
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sRedirectPath As String = String.Empty
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Session(CNMTAType) = Nothing
            Select Case btnReinstatement.CommandName
                Case "Reinstatement"

                    'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                    If hdMarkedtext.Text = "true" And oQuote.MarkedQuoteForCollection Then
                        oQuote.MarkedQuoteForCollection = False
                        oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                        Session(CNQuote) = oQuote
                    End If
                    Session.Remove(CNRenewal)
                    Session.Remove(CNIsBackDatedMTA)
                    Session(CNQuote) = oQuote
                    Session(CNMTAType) = MTAType.REINSTATEMENT
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Session.Remove(CNOI)
                    Session(CNInsuranceFileKey) = btnReinstatement.CommandArgument
                    Session(CNMtaReasonSelected) = Nothing
                    sRedirectPath = "~/secure/MTAReason.aspx"
            End Select

            Response.Redirect(sRedirectPath, False)
        End Sub
        ''' <summary>
        ''' On the click of copy button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Dim iCurrentRiskKey As Integer
            Session.Remove(CNOldPremium) 'Remove the old premium from session
            Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            ClearQuoteCollectionSessionValues()
            ClearQuote()
            'Pure 3.0 ---- WPR 41
            Dim sRedirectPath As String = String.Empty


            'User has decided to opt "Copy" quote option
            Dim iInsuranceFile As Integer = btnCopy.CommandArgument

            Select Case btnCopy.CommandName

                Case "CopyQuote"

                    CopyQuote(iInsuranceFile)

                Case "CopyPolicy"

                    If hvRetainPolNum.Value.ToUpper = "TRUE" Then
                        CopyQuote(iInsuranceFile, True)
                    Else
                        CopyQuote(iInsuranceFile, bCalledFromClonePolicy:=True)
                    End If

            End Select

            Try

                oQuote = oWebService.GetHeaderAndSummariesByKey(btnCopy.CommandArgument)


                'Put highest risk key into Session
                For i As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote)
                    iCurrentRiskKey = oQuote.Risks(i).Key
                Next


                oWebService.GetHeaderAndRisksByKey(oQuote)

                Session(CNQuote) = oQuote

            Catch ex As NexusProvider.NexusException
                'Catch Policy Locking error and show error alert
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        'Show policy locking error as alert
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                        Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select

            Finally
                'oWebService = Nothing
            End Try
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE

            Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
            oAllowPolicyClientAssociationsOptionSettings = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)
            If oAllowPolicyClientAssociationsOptionSettings IsNot Nothing AndAlso oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                Dim oPartyCollection As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                If Trim(oQuote.PartyKey) <> Trim(oPartyCollection.Key) Then
                    Dim oNewParty As NexusProvider.BaseParty
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oNewParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oNewParty
                End If
            End If

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()

                DataSetFunctions.GetScreens()
            End If

            If (tvClientPolicy.SelectedValue <> "") Then
                hvInsuranceFolderKey.Value = tvClientPolicy.SelectedValue
                sInsuranceFileRef = tvClientPolicy.SelectedNode.Text
            End If

            tvClientPolicy.Nodes.Clear()
            If Me.PartyKey = 0 AndAlso CType(Session(CNParty), NexusProvider.BaseParty).Key <> 0 _
               AndAlso Session(CNClientMode) = Mode.View AndAlso Session(CNIsNewClient) Is Nothing Then
                'get the party key from session
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        Me.PartyKey = CType(Session(CNParty), NexusProvider.PersonalParty).Key
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        Me.PartyKey = CType(Session(CNParty), NexusProvider.CorporateParty).Key
                End Select
            End If

            'Initialise this to false
            PanelViewAllPolicies(False)
            If UserCanDoTask("ViewOldPolicies") Then
                PanelViewAllPolicies(True)
            Else
                PanelViewAllPolicies(False)
            End If
            tvClientPolicy_SelectedNodeChanged(sender, e)

        End Sub
        ''' <summary>
        ''' On the click of Change button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnClaim_Click(sender As Object, e As EventArgs) Handles btnClaim.Click
            ''Used For accounts having claim already made and policy coming in Red.
            Dim sPolicyNumber As String = tvClientPolicy.SelectedNode.Text.ToString()
            If tvClientPolicy.SelectedNode.Text.ToString().Contains("<font") Then
                sPolicyNumber = Regex.Replace(sPolicyNumber, "</?font.*?>", String.Empty)
            End If

            Response.Redirect("~/Claims/FindInsuranceFile.aspx?Policyno=" & sPolicyNumber & "&submit=false")
        End Sub

        Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
            Dim oAllPolicies As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)
            If Not oAllPolicies Is Nothing Then
                Dim oPolicyToSelect = From policy In oAllPolicies Where Trim(policy.Reference.ToString()) = txtPolicyNo.Text.Trim() Select policy
                If oPolicyToSelect.Count > 0 Then
                    If tvClientPolicy.SelectedNode IsNot Nothing Then
                        tvClientPolicy.SelectedNode.Selected = False
                    End If
                    hvInsuranceFolderKey.Value = oPolicyToSelect.ElementAt(0).InsuranceFolderKey
                    tvClientPolicy_SelectedNodeChanged(sender, e)
                    For i As Integer = 0 To tvClientPolicy.Nodes.Count - 1
                        BrowseTreeNodes(tvClientPolicy.Nodes(i), 0)
                    Next
                End If
            End If
        End Sub

        Private Sub BrowseTreeNodes(subRoot As TreeNode, level As Integer)
            If subRoot Is Nothing Then
                Return
            End If
            ' do what you need here (just print to output for the purpose of demonstration - this is where "level" is used)
            Dim nodeText As String = subRoot.Text.PadLeft(subRoot.Text.Length + level, ControlChars.Tab)
            Console.WriteLine(nodeText)
            ' loop through the children
            For i As Integer = 0 To subRoot.ChildNodes.Count - 1
                BrowseTreeNodes(subRoot.ChildNodes(i), level + 1)
                If subRoot.ChildNodes(i).Text.Trim() = txtPolicyNo.Text.Trim() Or subRoot.ChildNodes(i).Text.Trim().Contains(txtPolicyNo.Text.Trim()) Then
                    tvClientPolicy.CollapseAll()
                    subRoot.ChildNodes(i).Expand()
                    subRoot.ChildNodes(i).Parent.Expand()
                    subRoot.ChildNodes(i).Parent.Parent.Expand()
                    subRoot.ChildNodes(i).Parent.Parent.Parent.Expand()
                    subRoot.ChildNodes(i).Selected = True
                    tvClientPolicy.Focus()
                End If
            Next
        End Sub

        ''' <summary>
        ''' This event will fire while binding row to grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRisks_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRisks.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oRisk As NexusProvider.Risk = CType(e.Row.DataItem, NexusProvider.Risk)
                'Change the link status code to descriptive text by getting the text from the resource file.
                Dim sRiskLinkStatusFlag As String = oRisk.RiskLinkStatusFlag
                ' Display the status as Added if the status is C and the OriginalRiskKey = 0.
                If sRiskLinkStatusFlag.ToUpper() = "C" AndAlso (oRisk.OriginalRiskKey = 0) Then
                    sRiskLinkStatusFlag = sRiskLinkStatusFlag & "_Added"
                End If
                e.Row.Cells(5).Text = GetLinkStatusText(sRiskLinkStatusFlag)

                ' Show a - when the link status date is empty.
                If e.Row.Cells(6).Text = "01/01/0001" Then
                    e.Row.Cells(6).Text = "-"
                End If

                If oSelectedQuote IsNot Nothing AndAlso oSelectedQuote.Risks IsNot Nothing Then
                    If e.Row.RowType = DataControlRowType.DataRow Then
                        If ViewState("CurrencyDataFormatString") IsNot Nothing Then
                            e.Row.Cells(3).Text = String.Format(ViewState("CurrencyDataFormatString"), Val(oSelectedQuote.Risks(e.Row.DataItemIndex).Premium) +
                                                                                    Val(oSelectedQuote.Risks(e.Row.DataItemIndex).FeePremium) +
                                                                Val(oSelectedQuote.Risks(e.Row.DataItemIndex).TotalAnnualTax))

                            e.Row.Cells(2).Text = String.Format(ViewState("CurrencyDataFormatString"), Val(oSelectedQuote.Risks(e.Row.DataItemIndex).TotalSumInsured))
                        Else
                            e.Row.Cells(3).Text = Val(oSelectedQuote.Risks(e.Row.DataItemIndex).Premium) +
                                                                                    Val(oSelectedQuote.Risks(e.Row.DataItemIndex).FeePremium) +
                                                                Val(oSelectedQuote.Risks(e.Row.DataItemIndex).TotalAnnualTax)

                            e.Row.Cells(2).Text = Val(oSelectedQuote.Risks(e.Row.DataItemIndex).TotalSumInsured)
                        End If

                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Change the status code to descriptive text by getting the text from the resource file.
        ''' </summary>
        ''' <param name="code">Link Status Code</param>
        ''' <returns>Link Status Description from the resource file</returns>
        ''' <remarks>Returns - if code not found.</remarks>
        Private Function GetLinkStatusText(ByVal code As String) As String

            Dim description As String = GetLocalResourceObject("lbl_ChangedStatus_" & code)
            If description Is Nothing Then
                description = "-"
            End If
            GetLinkStatusText = description

        End Function

        Private Sub ISReinstateLink()
            Dim iCount As Integer
            For iCount = gvPolicyVersions.Rows.Count - 1 To 0 Step -1

                If Convert.ToString(gvPolicyVersions.DataKeys(iCount).Values("InsuranceFileTypeCode")).Trim().ToUpper() = "MTACAN" And gvPolicyVersions.DataKeys(iCount).Values("BaseInsuranceFileKey") = 0 Or gvPolicyVersions.DataKeys(iCount).Values("BaseInsuranceFileKey") = gvPolicyVersions.DataKeys(iCount).Values("InsuranceFileKey") Then
                    ViewState(CNIsReinstateLink) = gvPolicyVersions.DataKeys(iCount).Values("InsuranceFileKey")
                    Exit For
                End If
            Next
        End Sub

        Protected Sub gvRisksk_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRisks.PageIndexChanging
            Dim oQuote As NexusProvider.Quote
            If Not Session(CNQuote) Is Nothing Then
                oQuote = Session(CNQuote)
            Else
                Dim iInsuranceFileKey As Integer = gvPolicyVersions.DataKeys(gvPolicyVersions.SelectedRow.RowIndex).Values(2)
                oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey)
            End If

            If oQuote IsNot Nothing Then
                gvRisks.PageIndex = e.NewPageIndex
                gvRisks.DataSource = oQuote.Risks
                gvRisks.DataBind()
            End If
        End Sub

        Private Sub CopyQuote(ByVal v_iInsuranceFile As Integer, Optional ByVal bCloneQuoteFromLivePolicy As Boolean = False, Optional ByVal bCalledFromClonePolicy As Boolean = False)
            Dim iInsuranceFile As Integer = v_iInsuranceFile

            If bCloneQuoteFromLivePolicy Then

                oWebService.CloneQuoteFromLivePolicy(iInsuranceFile)
            Else
                ''call SAM method "CopyQuote" to create copy of the quote 
                oWebService.CopyQuote(iInsuranceFile, v_bCalledFromClonePolicy:=bCalledFromClonePolicy)
            End If

        End Sub

        ReadOnly Property GetCurrentMTAType() As MTAType
            Get
                Return CType(HttpContext.Current.Session(CNMTAType), MTAType)
            End Get
        End Property

       Private Sub ShowHideVoidButton(ByVal v_lInsuranceFileKey As Integer)
            Dim bShowVoidButton As Boolean = False
            Dim bInstalmentExists As Boolean = False
            Dim bQuoteExists As Boolean = False
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sb As New StringBuilder

            sb.Append(GetLocalResourceObject("msg_ConfirmVoidPolicyVersion").ToString.Replace("\n", Environment.NewLine))
            If v_lInsuranceFileKey = 0 Then
                v_lInsuranceFileKey = Session(CNInsuranceFileKey)
            End If
            If v_lInsuranceFileKey > 0 Then
                oWebService.IsVoidPolicyVersion(v_lInsuranceFileKey, bShowVoidButton, bInstalmentExists, bQuoteExists)
            End If
            If bShowVoidButton Then
                If bInstalmentExists OrElse bQuoteExists Then
                    sb.Append("WARNING: ")
                End If
                If bInstalmentExists Then
                    sb.Append(GetLocalResourceObject("msg_InsPlanExists").ToString.Replace("\n", Environment.NewLine))
                End If
                If bQuoteExists Then
                    sb.Append(GetLocalResourceObject("msg_QuoteExists").ToString.Replace("\n", Environment.NewLine))
                End If
                hvVoidMessage.Value = sb.ToString()
                btnVoid.Visible = True
                btnVoid.CommandArgument = v_lInsuranceFileKey
                btnVoid.Attributes.Add("OnClick", "javascript:return VoidPolicyVersionConfirmation();")
            End If
        End Sub

        Protected Sub btnVoid_Click(sender As Object, e As EventArgs) Handles btnVoid.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim v_lInsuranceFileKey As Integer
            Dim v_lInsuranceFolderKey As Integer
            Dim bCreated As Boolean
            Dim sMessage As String = ""
            If hvVoidConfirm.Value.ToUpper = "TRUE" Then
                v_lInsuranceFileKey = CInt(btnVoid.CommandArgument)
                v_lInsuranceFolderKey = Session(CNInsuranceFolderKey)
                If v_lInsuranceFileKey > 0 Then
                    ' Make the latest live version void
                    oWebService.CreateVoidPolicyVersion(v_iInsuranceFileKey:=v_lInsuranceFileKey, v_iInsuranceFolderKey:=v_lInsuranceFolderKey,
                                                     r_bIsCreatedVoidPolicyVersion:=bCreated, r_sFailureMessage:=sMessage)
                    If bCreated Then
                        tvClientPolicy_SelectedNodeChanged(sender, e)
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CallRenewalConfirmation", "alert('" & GetLocalResourceObject("msg_VoidPolicyFailure").ToString & "');", True)
                        Return
                    End If
                End If
            Else
                'do nothing 
            End If
        End Sub
    End Class

End Namespace

