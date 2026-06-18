<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="SelectFolders.aspx.vb" Inherits="Nexus.Modal_SelectFolders" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript">

        function selectAll(invoker) {
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                // Filter through the input types looking for checkboxes
                if (myElement.type === "checkbox") {
                    // Use the invoker (our calling element) as the reference 
                    //  for our checkbox status
                    myElement.checked = invoker.checked;
                }
            }
        }
    </script>

    <div id="Modal_SelectFolders">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="Select Folders"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCompanye" runat="server" AssociatedControlID="txtCompany" Text="<%$ Resources:lblCompany %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCompany" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFolderName" runat="server" AssociatedControlID="txtFolderName" Text="<%$ Resources:lblFolderName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFolderName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnSearch" runat="server" Text="<%$ Resources:btnSearch %>" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtFolderName" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>
        <div class="grid-card table-responsive table-scroll">
            <asp:GridView ID="grdvSearchClients" runat="server" AllowPaging="false" AutoGenerateColumns="false" DataKeyNames="FolderNum" PageSize="50" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSelect" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                            <asp:Label ID="lblFolderNum" runat="server" Visible="false"></asp:Label>
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbSelectAll" runat="server" Text="Select All" OnClick="selectAll(this)" CssClass="asp-check"></asp:CheckBox>
                        </HeaderTemplate>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lblFolderName %>"></asp:BoundField>
                    <asp:BoundField DataField="CreateDate" SortExpression="CreateDate" HeaderText="Created date" DataFormatString="{0:d}"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnApply" runat="server" Text="<%$ Resources:btnApply %>" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
        </div>

    </div>
</asp:Content>
