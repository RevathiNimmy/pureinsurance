Imports System.Data
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants.Session
Imports Nexus.Constants
Imports NexusProvider
Imports Nexus

Partial Class Controls_FolderView
    Inherits System.Web.UI.UserControl

    Dim sSelectedNode As String = Nothing
    Dim iBranchDepth As Integer = 0
    Public Const CNDMECollection As String = "DMECollection"
    Dim sCurrentBranchName As String = ""

    ''' <summary>Page Load Event - Display Top Level Node </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("BranchCode") <> "" Then
            sCurrentBranchName = GetDescriptionForCode(NexusProvider.ListType.PMLookup, Request.QueryString("BranchCode").Trim(), "SOURCE")
        ElseIf Session(CNBranchCode) IsNot Nothing AndAlso Session(CNBranchCode) <> "" Then
            sCurrentBranchName = GetDescriptionForCode(NexusProvider.ListType.PMLookup, Session(CNBranchCode).Trim(), "SOURCE")
        Else
            sCurrentBranchName = Convert.ToString(Session("BranchName"))
        End If

        If Not IsPostBack Then

            If ViewState(CNDMECollection) Is Nothing Then
                'create a unique key and add this to viewstate
                'this will be used to cache the results of the SAM
                ViewState.Add(CNDMECollection, Session.SessionID)

                'GET TOP Level Node
                GetBranchList()
            End If

            'Call "DefaultSelectedNodes" Function When query string have folder path - Selected Node to be opened on page load
            If Not String.IsNullOrEmpty(Request.QueryString("path")) Then
                DefaultSelectedNodes()
            End If
        Else
            If Request("__EVENTARGUMENT") = "PopulateClientData" Then
                DMEtree.Nodes(HidSelectBranchDetail.Value.Split(":")(0)).ChildNodes.Clear()
                For iiCount As Integer = 0 To HidSelectedClients.Value.Split(";").Length - 2
                    ' 'Add onclick client side event with each node - when folder is clicked to populate file view control. The file view control will have a client side “setFolder” method which takes a new path to load into the control.
                    Dim newNode As TreeNode = New TreeNode(("<span onclick=javascript:setFolder('" & Server.UrlEncode(Trim(HidSelectBranchDetail.Value.Split(":")(1))) & DMEtree.PathSeparator & Server.UrlEncode(Trim((HidSelectedClients.Value.Split(";")(iiCount)).Split(":")(0))) & "','" & (HidSelectedClients.Value.Split(";")(iiCount)).Split(":")(1) & "'); return true;>" & Trim((HidSelectedClients.Value.Split(";")(iiCount)).Split(":")(0)) & "</span>"), (HidSelectedClients.Value.Split(";")(iiCount)).Split(":")(0))
                    newNode.Select()
                    newNode.Expanded = True
                    newNode.SelectAction = TreeNodeSelectAction.Expand
                    newNode.ExpandAll()
                    DMEtree.Nodes(HidSelectBranchDetail.Value.Split(":")(0)).ChildNodes.Add(newNode)
                    If (DMEtree.Nodes(iBranchDepth).ChildNodes.Count > iiCount) Then
                        GetSubFolderList(DMEtree.Nodes(iBranchDepth).ChildNodes(iiCount))
                    End If

                Next
            End If

        End If
        If Not String.IsNullOrEmpty(Request.QueryString("FolderNum")) And Not String.IsNullOrEmpty(Request.QueryString("FolderName")) And sCurrentBranchName <> "" Then
            HidSelectedClients.Value = Request.QueryString("FolderName").ToString() + ":" + Request.QueryString("FolderNum").ToString() + ";"
        End If
        If sCurrentBranchName <> "" Then
            If Not String.IsNullOrWhiteSpace(HidSelectBranchDetail.Value) Then
                iBranchDepth = HidSelectBranchDetail.Value.Split(":")(0)
            End If

            DMEtree.Nodes(iBranchDepth).ChildNodes.Clear()
            Dim sBranchName As String = sCurrentBranchName
            For iiCount As Integer = 0 To HidSelectedClients.Value.Split(";").Length - 2
                ' 'Add onclick client side event with each node - when folder is clicked to populate file view control. The file view control will have a client side “setFolder” method which takes a new path to load into the control.
                Dim newNode As TreeNode = New TreeNode(("<span onclick=javascript:setFolder('" & Server.UrlEncode(Trim(sBranchName)) & DMEtree.PathSeparator & Server.UrlEncode(Trim((HidSelectedClients.Value.Split(";")(iiCount)).Split(":")(0))) & "','" & (HidSelectedClients.Value.Split(";")(iiCount)).Split(":")(1) & "'); return true;>" & Trim((HidSelectedClients.Value.Split(";")(iiCount)).Split(":")(0)) & "</span>"), (HidSelectedClients.Value.Split(";")(iiCount)).Split(":")(0))
                newNode.Select()
                newNode.Expanded = True
                newNode.SelectAction = TreeNodeSelectAction.Expand
                newNode.ExpandAll()
                'newNode.PopulateOnDemand = True
                DMEtree.Nodes(iBranchDepth).ChildNodes.Add(newNode)
                GetSubFolderList(DMEtree.Nodes(iBranchDepth).ChildNodes(iiCount))
            Next
        End If
    End Sub


    ''' <summary>Display Top Level Node</summary>
    ''' <remarks></remarks>
    Protected Sub GetBranchList()
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oDMEFolder As New DME
        Dim oTreeView As TreeNodeCollection = CType(Cache.Item(ViewState(CNDMECollection)), TreeNodeCollection)
        '  Dim 
        If oTreeView Is Nothing Or (oTreeView IsNot Nothing AndAlso oTreeView.Count > 0) Then
            oDMEFolder = oWebService.GetDMEFolder(0, Nothing, False)
            If oDMEFolder IsNot Nothing Then
                For iCount As Integer = 0 To oDMEFolder.SubFolder.Count - 1
                    'Add onclick client side event with each node - when folder is clicked to populate file view control. The file view control will have a client side “setFolder” method which takes a new path to load into the control.
                    'Dim newNode As TreeNode = New TreeNode(("<span onclick=javascript:setFolder('" & Trim(oDMEFolder.SubFolder(iCount).Name).Replace(" ", "&nbsp;") & "','" & oDMEFolder.SubFolder(iCount).FolderNum & "'); return false;>" & Trim(oDMEFolder.SubFolder(iCount).Name).Replace(" ", "&nbsp;") & "</span>"), oDMEFolder.SubFolder(iCount).Name.Replace(" ", "&nbsp;"))

                    Dim newNode As TreeNode = New TreeNode(("<span onclick=javascript:setFolder('" & Server.UrlEncode(Trim(oDMEFolder.SubFolder(iCount).Name)) & "','" & oDMEFolder.SubFolder(iCount).FolderNum & "'); return false;>" & Trim(oDMEFolder.SubFolder(iCount).Name) & "</span>"), Trim(oDMEFolder.SubFolder(iCount).Name))
                    If Request.Url.ToString().Contains("DMEDocumentManager.aspx") Then
                        newNode.NavigateUrl = ""
                    Else
                        If HttpContext.Current.Session.IsCookieless Then
                            newNode.NavigateUrl = "javascript:tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SelectFolders.aspx?ParentNum=" & oDMEFolder.ParentNum & "&FolderNum=" & oDMEFolder.SubFolder(iCount).FolderNum & "&SelectBranchDetail=" & iCount & ":" & Server.UrlEncode(Trim(oDMEFolder.SubFolder(iCount).Name)) & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);"
                        Else
                            newNode.NavigateUrl = "javascript:tb_show(null , ' " & AppSettings("WebRoot") & "Modal/SelectFolders.aspx?ParentNum=" & oDMEFolder.ParentNum & "&FolderNum=" & oDMEFolder.SubFolder(iCount).FolderNum & "&SelectBranchDetail=" & iCount & ":" & Server.UrlEncode(Trim(oDMEFolder.SubFolder(iCount).Name)) & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);"
                        End If
                    End If
                    'newNode.Selected = True
                    newNode.SelectAction = TreeNodeSelectAction.Expand
                    newNode.ExpandAll()
                    newNode.PopulateOnDemand = False
                    DMEtree.Nodes.Add(newNode)
                    If sCurrentBranchName IsNot Nothing And Request.Url.ToString().Contains("DMEDocumentManager.aspx") Then
                        iBranchDepth = -1
                        DMEtree.Nodes.Clear()
                        If oDMEFolder.SubFolder(iCount).Name.Trim.ToUpper = sCurrentBranchName.ToString().Trim.ToUpper Then
                            iBranchDepth = 0
                            DMEtree.Nodes.Add(newNode)
                            Exit For
                        End If
                    End If
                Next
            End If
        Else
            ' Bind The TOP Level Node with the help of cache treenode
            DMEtree.Nodes.Clear()
            For Q = 0 To oTreeView.Count - 1
                DMEtree.Nodes.Add(oTreeView(0))
            Next
            DMEtree.DataBind()

            ' Populate The TOP Level Node with the help of cache treenode
            For iCount As Integer = 0 To oTreeView.Count - 1
                'Add onclick client side event with each node - when folder is clicked to populate file view control. The file view control will have a client side “setFolder” method which takes a new path to load into the control.
                Dim newNode As TreeNode = New TreeNode(DMEtree.Nodes(iCount).Text, DMEtree.Nodes(iCount).Value)
                If HttpContext.Current.Session.IsCookieless Then
                    newNode.NavigateUrl = "javascript:tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SelectFolders.aspx?FolderNum=" & DMEtree.Nodes(iCount).Value & "&SelectBranchDetail=" & iCount & ":" & Trim(DMEtree.Nodes(iCount).Text) & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);"
                Else
                    newNode.NavigateUrl = "javascript:tb_show(null , ' " & AppSettings("WebRoot") & "Modal/SelectFolders.aspx?FolderNum=" & DMEtree.Nodes(iCount).Value & "&SelectBranchDetail=" & iCount & ":" & Trim(DMEtree.Nodes(iCount).Text) & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);"
                End If

                newNode.SelectAction = TreeNodeSelectAction.Expand
                newNode.Selected = True
                newNode.ExpandAll()
                newNode.PopulateOnDemand = False
                DMEtree.Nodes.Add(newNode)
            Next
        End If
    End Sub
    ''' <summary>Display SubFolder Nodes - Based on PopulateNode repopulate the control asynchronously</summary>
    ''' <remarks></remarks>
    Protected Sub GetSubFolderList(ByVal node As TreeNode)
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oDME As New DME
        'if Any Node have Space(&nbsp;) in between Value, replace with space (' ')
        Dim sCurrent_Folder As String = Replace(node.ValuePath, "&nbsp;", " ")

        ' Reset GeneralFolderNum/GeneralFolderExist before scanning to prevent stale state
        If Request.QueryString("fromlink") IsNot Nothing AndAlso Request.QueryString("fromlink").ToString() = "client" Then
            Session("GeneralFolderNum") = Nothing

        End If

        ' Call GetDMEFolder to get the contents of that folder
        oDME = oWebService.GetDMEFolder(0, sCurrent_Folder, False)

        Dim oPartySummary As NexusProvider.PartySummary
        Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
        Dim nAgentKey As Integer = 0
        Dim sAgentType As String = String.Empty
        If oUserDetails IsNot Nothing Then
            If (oUserDetails.Key <> 0 AndAlso sCurrent_Folder.Split("|").Length > 1) Then
                nAgentKey = oUserDetails.Key
                sAgentType = oUserDetails.PartyType
                Dim codeList As New System.Collections.ArrayList


                oPartySummary = oWebService.GetPartyPolicies(sCurrent_Folder.Split("|")(1), Nothing, nAgentKey, sAgentType)

                For Each policy As Policy In oPartySummary.Policies
                    codeList.Add(policy.InsuranceFolderKey.ToString())
                Next
                Dim oClaimSearchCriteria As New NexusProvider.ClaimSearchCriteria
                oClaimSearchCriteria.ClientShortName = sCurrent_Folder.Split("|")(1)
                oClaimSearchCriteria.IncludeClosedClaim = True
                Dim oClaims As NexusProvider.ClaimCollection
                oClaims = oWebService.FindClaim(oClaimSearchCriteria)

                For Each claim As Claim In oClaims
                    codeList.Add("C" & Strings.StrDup(10 - claim.BaseClaimKey.ToString().Length - 1, "0") & claim.BaseClaimKey.ToString())
                Next


                If Not String.IsNullOrEmpty(Request.QueryString("path")) AndAlso sCurrent_Folder.Split(DMEtree.PathSeparator).Length = 1 Then
                    If oDME IsNot Nothing Then
                        For iCount As Integer = 0 To oDME.SubFolder.Count - 1
                            If codeList.Contains(oDME.SubFolder(iCount).ExternalCode) Or oDME.SubFolder(iCount).ExternalCode = "GENERAL" Then
                                If Server.UrlEncode(Trim(oDME.SubFolder(iCount).Name)) = Server.UrlEncode(Trim(Request.QueryString("path").Split(DMEtree.PathSeparator)(1))) Then
                                    ' 'Add onclick client side event with each node - when folder is clicked to populate file view control. The file view control will have a client side “setFolder” method which takes a new path to load into the control.
                                    Dim newNode As TreeNode = New TreeNode(("<span onclick=javascript: setFolder('" & Server.UrlEncode(Trim(node.ValuePath)) & DMEtree.PathSeparator & Server.UrlEncode(Trim(oDME.SubFolder(iCount).Name)) & "','" & oDME.SubFolder(iCount).FolderNum & "'); return false;>" & Trim(oDME.SubFolder(iCount).Name) & "</span>"), oDME.SubFolder(iCount).Name)

                                    If Request.Url.ToString().Contains("DMEDocumentManager.aspx") Then
                                        If Request.QueryString("fromlink").ToString() = "policy" And Session("PolicyNo") IsNot Nothing Then
                                            If oDME.SubFolder(iCount).Name.ToUpper().Trim().Contains(Session("PolicyNo").ToString().ToUpper().Trim()) Then
                                                Session("PolicyFolderName") = oDME.SubFolder(iCount).Name.ToString()
                                                Session("PolicyFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                                node.ChildNodes.Add(newNode)
                                            End If
                                        ElseIf Request.QueryString("fromlink").ToString() = "claim" And Session("claimno") IsNot Nothing Then
                                            If oDME.SubFolder(iCount).Name.ToUpper().Trim().Contains(Session("claimno").ToString().ToUpper().Trim()) Then
                                                Session("ClaimFolderName") = oDME.SubFolder(iCount).Name.ToString()
                                                Session("ClaimFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                                node.ChildNodes.Add(newNode)
                                            End If
                                        ElseIf Request.QueryString("fromlink").ToString() = "client" And oDME.SubFolder(iCount).Name.Trim().ToUpper() = "GENERAL" Then
                                            Session("GeneralFolderExist") = "General"
                                            Session("GeneralFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                            node.ChildNodes.Add(newNode)
                                        End If
                                    Else
                                        newNode.SelectAction = TreeNodeSelectAction.Expand
                                        newNode.Selected = True
                                        newNode.Expanded = False
                                        newNode.PopulateOnDemand = True
                                        node.ChildNodes.Add(newNode)
                                    End If
                                End If
                            End If
                        Next
                    End If
                Else
                    If oDME IsNot Nothing Then
                        For iCount As Integer = 0 To oDME.SubFolder.Count - 1
                            If codeList.Contains(oDME.SubFolder(iCount).ExternalCode) Or oDME.SubFolder(iCount).ExternalCode = "GENERAL" Then
                                ' 'Add onclick client side event with each node - when folder is clicked to populate file view control. The file view control will have a client side “setFolder” method which takes a new path to load into the control.
                                Dim newNode As TreeNode = New TreeNode(("<span onclick=javascript:setFolder('" & Server.UrlEncode(Trim(node.ValuePath)) & DMEtree.PathSeparator & Server.UrlEncode(Trim(oDME.SubFolder(iCount).Name)) & "','" & oDME.SubFolder(iCount).FolderNum & "'); return false;>" & Trim(oDME.SubFolder(iCount).Name) & "</span>"), oDME.SubFolder(iCount).Name)
                                If Request.Url.ToString().Contains("DMEDocumentManager.aspx") Then
                                    If Request.QueryString("fromlink").ToString() = "policy" And Session("PolicyNo") IsNot Nothing Then
                                        If oDME.SubFolder(iCount).Name.ToUpper().Trim().Contains(Session("PolicyNo").ToString().ToUpper().Trim()) Then
                                            Session("PolicyFolderName") = oDME.SubFolder(iCount).Name.ToString()
                                            Session("PolicyFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                            node.ChildNodes.Add(newNode)
                                        End If
                                    ElseIf Request.QueryString("fromlink").ToString() = "claim" And Session("claimno") IsNot Nothing Then
                                        If oDME.SubFolder(iCount).Name.ToUpper().Trim().Contains(Session("claimno").ToString().ToUpper().Trim()) Then
                                            Session("ClaimFolderName") = oDME.SubFolder(iCount).Name.ToString()
                                            Session("ClaimFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                            node.ChildNodes.Add(newNode)
                                        End If
                                    ElseIf Request.QueryString("fromlink").ToString() = "client" And oDME.SubFolder(iCount).Name.ToString().ToUpper().Trim() = "GENERAL" Then
                                        Session("GeneralFolderExist") = "General"
                                        Session("GeneralFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                        node.ChildNodes.Add(newNode)
                                    End If
                                Else
                                    newNode.SelectAction = TreeNodeSelectAction.Expand
                                    newNode.Selected = True
                                    newNode.Expanded = False
                                    newNode.PopulateOnDemand = True
                                    node.ChildNodes.Add(newNode)
                                End If
                            End If
                        Next
                    End If
                End If
            Else
                If Not String.IsNullOrEmpty(Request.QueryString("path")) AndAlso sCurrent_Folder.Split(DMEtree.PathSeparator).Length = 1 Then
                    If oDME IsNot Nothing Then
                        For iCount As Integer = 0 To oDME.SubFolder.Count - 1
                            If Server.UrlEncode(Trim(oDME.SubFolder(iCount).Name)) = Server.UrlEncode(Trim(Request.QueryString("path").Split(DMEtree.PathSeparator)(1))) Then
                                ' 'Add onclick client side event with each node - when folder is clicked to populate file view control. The file view control will have a client side “setFolder” method which takes a new path to load into the control.
                                Dim newNode As TreeNode = New TreeNode(("<span onclick=javascript:setFolder('" & Server.UrlEncode(Trim(node.ValuePath)) & DMEtree.PathSeparator & Server.UrlEncode(Trim(oDME.SubFolder(iCount).Name)) & "','" & oDME.SubFolder(iCount).FolderNum & "'); return false;>" & Trim(oDME.SubFolder(iCount).Name) & "</span>"), oDME.SubFolder(iCount).Name)

                                If Request.Url.ToString().Contains("DMEDocumentManager.aspx") Then
                                    If Request.QueryString("fromlink").ToString() = "policy" And Session("PolicyNo") IsNot Nothing Then
                                        If oDME.SubFolder(iCount).Name.ToUpper().Trim().Contains(Session("PolicyNo").ToString().ToUpper().Trim()) Then
                                            Session("PolicyFolderName") = oDME.SubFolder(iCount).Name.ToString()
                                            Session("PolicyFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                            node.ChildNodes.Add(newNode)
                                        End If
                                    ElseIf Request.QueryString("fromlink").ToString() = "claim" And Session("claimno") IsNot Nothing Then
                                        If oDME.SubFolder(iCount).Name.ToUpper().Trim().Contains(Session("claimno").ToString().ToUpper().Trim()) Then
                                            Session("ClaimFolderName") = oDME.SubFolder(iCount).Name.ToString()
                                            Session("ClaimFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                            node.ChildNodes.Add(newNode)
                                        End If
                                    ElseIf Request.QueryString("fromlink").ToString() = "client" And oDME.SubFolder(iCount).Name.Trim().ToUpper() = "GENERAL" Then
                                        Session("GeneralFolderExist") = "General"
                                        Session("GeneralFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                        node.ChildNodes.Add(newNode)
                                    End If
                                Else
                                    newNode.SelectAction = TreeNodeSelectAction.Expand
                                    newNode.Selected = True
                                    newNode.Expanded = False
                                    newNode.PopulateOnDemand = True
                                    node.ChildNodes.Add(newNode)
                                End If
                            End If
                        Next
                    End If
                Else
                    If oDME IsNot Nothing Then
                        For iCount As Integer = 0 To oDME.SubFolder.Count - 1
                            ' 'Add onclick client side event with each node - when folder is clicked to populate file view control. The file view control will have a client side “setFolder” method which takes a new path to load into the control.
                            Dim newNode As TreeNode = New TreeNode(("<span onclick=javascript:setFolder('" & Server.UrlEncode(Trim(node.ValuePath)) & DMEtree.PathSeparator & Server.UrlEncode(Trim(oDME.SubFolder(iCount).Name)) & "','" & oDME.SubFolder(iCount).FolderNum & "'); return false;>" & Trim(oDME.SubFolder(iCount).Name) & "</span>"), oDME.SubFolder(iCount).Name)
                            If Request.Url.ToString().Contains("DMEDocumentManager.aspx") Then
                                If Request.QueryString("fromlink").ToString() = "policy" And Session("PolicyNo") IsNot Nothing Then
                                    If oDME.SubFolder(iCount).Name.ToUpper().Trim().Contains(Session("PolicyNo").ToString().ToUpper().Trim()) Then
                                        Session("PolicyFolderName") = oDME.SubFolder(iCount).Name.ToString()
                                        Session("PolicyFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                        node.ChildNodes.Add(newNode)
                                    End If
                                ElseIf Request.QueryString("fromlink").ToString() = "claim" And Session("claimno") IsNot Nothing Then
                                    If oDME.SubFolder(iCount).Name.ToUpper().Trim().Contains(Session("claimno").ToString().ToUpper().Trim()) Then
                                        Session("ClaimFolderName") = oDME.SubFolder(iCount).Name.ToString()
                                        Session("ClaimFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                        node.ChildNodes.Add(newNode)
                                    End If
                                ElseIf Request.QueryString("fromlink").ToString() = "client" And oDME.SubFolder(iCount).Name.ToString().ToUpper().Trim() = "GENERAL" Then
                                    Session("GeneralFolderExist") = "General"
                                    Session("GeneralFolderNum") = oDME.SubFolder(iCount).FolderNum.ToString()
                                    node.ChildNodes.Add(newNode)
                                End If
                            Else
                                newNode.SelectAction = TreeNodeSelectAction.Expand
                                newNode.Selected = True
                                newNode.Expanded = False
                                newNode.PopulateOnDemand = True
                                node.ChildNodes.Add(newNode)
                            End If
                        Next
                    End If
                End If

            End If
        End If



        ' End If
    End Sub
    ''' <summary>
    '''Based on PopulateNode repopulate the control asynchronously
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PopulateNode(ByVal source As Object, ByVal e As TreeNodeEventArgs)
        If IsPostBack Then
            GetSubFolderList(e.Node)
        End If
    End Sub
    ''' <summary>
    '''Based on PopulateNode recursively TreeNode with checking of cache treenode
    ''' </summary>
    ''' <remarks></remarks>
    Sub FuncRecursivelyFindNode(ByVal TreeNodeCollection As TreeNodeCollection)
        Dim node As TreeNode
        Dim i As Integer = 0
        Dim iMatchIndex As Integer

        For Each oControl In TreeNodeCollection
            If Not String.IsNullOrEmpty(Request.QueryString("path")) Then
                If Request.QueryString("path").Split(DMEtree.PathSeparator).Length <> 0 Then

                    sSelectedNode = "<span onclick=javascript:setFolder('"
                    For temp = 0 To TreeNodeCollection(i).Depth
                        If temp = 0 Then
                            sSelectedNode = sSelectedNode & Server.UrlEncode(Trim(Request.QueryString("path").Split(DMEtree.PathSeparator)(temp)))
                        Else
                            sSelectedNode = sSelectedNode & DMEtree.PathSeparator & Server.UrlEncode(Trim(Request.QueryString("path").Split(DMEtree.PathSeparator)(temp)))
                        End If
                    Next
                    sSelectedNode = sSelectedNode & "','"

                    iMatchIndex = TreeNodeCollection(i).Text.Replace("%7c", "|").IndexOf(sSelectedNode, 0)

                    'if Branch Name of QueryString match with CreatedNode , Call The setFolder Javascript Function with appropriate node value  (if no Client and Transaction querystring present)
                    If iMatchIndex <> -1 Then
                        Dim sJavascript As String
                        sJavascript = "setFolder('"
                        If Request.QueryString("path").Split(DMEtree.PathSeparator).Length - 1 = TreeNodeCollection(i).Depth Then
                            For temp = 0 To Request.QueryString("path").Split(DMEtree.PathSeparator).Length - 1
                                If temp = 0 Then
                                    sJavascript = sJavascript & Trim(Request.QueryString("path").Split(DMEtree.PathSeparator)(temp))
                                Else
                                    sJavascript = sJavascript & DMEtree.PathSeparator & Trim(Request.QueryString("path").Split(DMEtree.PathSeparator)(temp))
                                End If
                            Next
                            sJavascript = sJavascript & "','" & FindFolderNum(Trim(Request.QueryString("path").Split(DMEtree.PathSeparator)(Request.QueryString("path").Split(DMEtree.PathSeparator).Length - 1)), TreeNodeCollection(i).Text, sSelectedNode) & "');"
                            Page.ClientScript.RegisterStartupScript(GetType(String), "StartupScripts", sJavascript, True)
                            Exit Sub
                        End If

                        'Checking of - treeview-root-node-has-child-nodes-exists-or-not
                        If TreeNodeCollection(i).ChildNodes.Count = 0 Then
                            node = TreeNodeCollection(i)
                            node.Select()
                            node.Expanded = True
                            node.SelectAction = TreeNodeSelectAction.Expand
                            node.ExpandAll()
                            ' Call GetDMEFolder to get the contents of that folder
                            GetSubFolderList(node)
                        End If

                        'recursively call TreeNode 
                        FuncRecursivelyFindNode(TreeNodeCollection(i).ChildNodes)
                        Exit For
                    End If

                    i = i + 1
                End If
            End If
        Next
    End Sub
    ''' <summary>
    '''When query string have folder path - Selected Node to be opened on page load
    ''' </summary>
    ''' <remarks></remarks>
    Sub DefaultSelectedNodes()
        If Request.QueryString("path") IsNot Nothing Then
            FuncRecursivelyFindNode(DMEtree.Nodes)
            'Cache.Insert(Session.SessionID, DMEtree.Nodes)
            Cache.Insert(ViewState(CNDMECollection), DMEtree.Nodes, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
        End If
    End Sub
    ''' <summary>
    '''Retrieve the Folder Number from Selected Node 
    ''' <param name="QueryStr_FolderName"></param>
    ''' <param name="Full_FolderName"></param>
    ''' <param name="LeftSelectedNode"></param>
    ''' </summary>
    ''' <remarks></remarks>
    Function FindFolderNum(ByVal QueryStr_FolderName As String, ByVal Full_FolderName As String, ByVal LeftSelectedNode As String) As Integer
        Dim sRightContain As String
        Dim sRight As String
        Dim ifolderNum As Integer

        sRightContain = Full_FolderName.Replace(LeftSelectedNode, "")
        sRight = "'); return true;>" & Trim(QueryStr_FolderName) & "</span>"

        'Replace unwanted Javascript character from Selected Node 
        If IsNumeric(sRightContain.Replace(sRight, "")) Then
            ifolderNum = sRightContain.Replace(sRight, "")
        Else
            ifolderNum = 0
        End If
        Return ifolderNum
    End Function


End Class
