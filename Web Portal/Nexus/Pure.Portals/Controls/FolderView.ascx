<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FolderView.ascx.vb" Inherits="Controls_FolderView" %>

<script type="text/javascript">
    function URLDecode(psEncodeString) {
        // Create a regular expression to search all +s in the string
        var lsRegExp = /\+/g;
        // Return the decoded string
        return unescape(String(psEncodeString).replace(lsRegExp, " "));
    }

    function SetFolders(sSelcetdClientsDetail, sSelcetdBranchDetails) {
        //add code to pass the client codes so that they can be read in OnTreeNodePopulate
        document.getElementById('<%= HidSelectedClients.ClientId%>').value = sSelcetdClientsDetail;
        document.getElementById('<%= HidSelectBranchDetail.ClientId%>').value = URLDecode(sSelcetdBranchDetails);

        //expand the originally selected node
        //ExpandCollapse__AspNetTreeView_Original(origSourceElement, 0);
        //hide the modal
        hide_modal();
        __doPostBack('', 'PopulateClientData');
    }
    $(document).ready(function () {

        //ExpandCollapse__AspNetTreeView_Original = ExpandCollapse__AspNetTreeView;
        //ExpandCollapse__AspNetTreeView = ExpandCollapse__AspNetTreeView_Override;
    });

    var origSourceElement;

    function ExpandCollapse__AspNetTreeView_Override(sourceElement, depth) {
        //if we're at 0 depth, i.e. branch level and the element is not expanded then we need to load modal
        if (depth == 0 && IsExpanded__AspNetTreeView(sourceElement) == false) {
            //store the sourceElement so that we can use this when the modal is closed
            origSourceElement = sourceElement;
            //launch modal to filter the client records to display
            //this will need to call show_modal
        }
        else {
            //   ExpandCollapse__AspNetTreeView_Original(sourceElement, depth);
        }
    }
</script>

<div class="card card-secondary">
    <div class="card-heading">
        <h4>
            <asp:Literal ID="lblHeading" runat="server" Text="<%$ Resources:lblPageHeader %>" /></h4>
    </div>
    <div class="card-body clearfix scrollable p-sm " style="height: 400px;">
        <asp:TreeView runat="server" ID="DMEtree" OnTreeNodePopulate="PopulateNode" CollapseImageUrl="~/images/icon-folder-open.png" EnableTheming="false"
            ExpandImageUrl="~/images/icon-folder-close.png"
            LeafNodeStyle-ImageUrl="~/images/icon-file.png"
            Width="100%" ExpandDepth="0" PathSeparator="|">

            <NodeStyle HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
            <ParentNodeStyle HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" Font-Size="12px" Font-Bold="true" ForeColor="" />
            <LeafNodeStyle HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" Font-Size="13px" Font-Bold="true" ForeColor="#343434" />
            <RootNodeStyle HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" Font-Size="13px" Font-Bold="true" />
            <SelectedNodeStyle HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" ForeColor="red" />
        </asp:TreeView>
        <asp:HiddenField ID="HidSelectedClients" runat="server" />
        <asp:HiddenField ID="HidSelectBranchDetail" runat="server" />
    </div>
</div>
