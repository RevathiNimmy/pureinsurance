<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CashListItems.ascx.vb"
    Inherits="Nexus.Controls_CashListItemstab" %>
<div id="Controls_CashListItems">
    <script language="javascript" type="text/javascript">
        function PostWarning(lblMsg) {
            alert(lblMsg);
        }
        function loadTab() {
            
            __doPostBack("btnAllocateCashlist", "RefreshTab");
        }
        function gridEditClick(Mode, CashListItemID, Type) {
            $('#hfMode').val(Mode); 
            $('#hfCashListItemID').val(CashListItemID); 
            $('#hfType').val(Type); 
            $('#hdnTabName').val("tab-CashListItem");

            __doPostBack("hypCashListItem", "EditClick");
        }
        function gridViewClick(Mode, CashListItemID, Type) {
            $('#hfMode').val(Mode);
            $('#hfCashListItemID').val(CashListItemID);
            $('#hfType').val(Type);
            $('#hdnTabName').val("tab-CashListItem");

            __doPostBack("hypView", "ViewClick");
        }
        function gridViewAllocationClick(TransdetailKeyVal) {
            $('#hfTransdetailKey').val(TransdetailKeyVal);
            $('#hdnTabName').val("tab-Allocate");

            __doPostBack("lblhypViewAllocation", "ViewAllocationClick");
        }
        function AddClick() {
            $('#hdnTabName').val("tab-CashListItem");
        }

        function CustVldCashListItemRequired_ClientValidation(source, arguments) {
            var accountKey = "";
            var chkBoxCheck = false;

            var grid = document.getElementById("<%=drgCashListItems.ClientID%>").getElementsByTagName("tr");
            if (grid.length > 0) {
                for (rowCount = 1; rowCount < grid.length ; rowCount++) {

                    if (grid[rowCount].getElementsByTagName("input")[0].type == 'checkbox' && grid[rowCount].getElementsByTagName("input")[0].checked==true) {
                        chkBoxCheck = true;
                        if (accountKey == "") {
                            accountKey = grid[rowCount].cells[4].innerHTML.trim();
                        }
                        if (rowCount != 0) {
                            if (accountKey == grid[rowCount].cells[4].innerHTML.trim()) {
                            }
                            else {
                                source.errormessage = "The allocation of multiple transactions must be for the same account.";
                                arguments.IsValid = false;
                                return;
                            }
                            if (grid[rowCount].cells[2].innerHTML.trim()== 'OCP' && grid[rowCount].cells[5].innerHTML.trim()== 'U'){
                                source.errormessage = "Payment for cash list item(s) with staus 'U' has not been processed. Please edit to use a different card or a different payment method.";
                                arguments.IsValid = false;
                                return;
                            }

                        }
                    }
                }
            }
            if (chkBoxCheck) {
                arguments.IsValid = true;
                return;
            }
            else {
                source.errormessage = "Please select at least one cash list item.";
                arguments.IsValid = false;
                return;
            }
        }     
    </script>
      <script>
          function RedirectToDownload() {
              var frame = $("#<%=docFrame.ClientID%>");
              frame.attr("src", "../Download.aspx");
          };
      </script>
    <div class="card">
        <asp:UpdatePanel ID="UpdCashListItems" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="card-heading">
                    <h1>
                        <asp:Label ID="lblCashListItemsHeading" runat="server" Text="<%$ Resources:lbl_CashListItemsHeading %>"></asp:Label></h1>
                </div>
                <div class="card-body clearfix">
                    <iframe id="docFrame" runat="server" width="0px" height="0px" class="hide"></iframe>
                    <div class="grid-card table-responsive">
                        <asp:CheckBox ID="chkAutoAllocate" TextAlign="Right" CssClass="normal-txt" Enabled="false"
                            runat="server" Text="<%$ Resources:lblChkAutoAllocate %>"></asp:CheckBox>
                        <asp:GridView ID="drgCashListItems" runat="server" AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkCashListItem" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MediaReference" HeaderText="<%$ Resources:lbl_MediaReference%>"></asp:BoundField>
                                <asp:BoundField DataField="MediaTypeCode" HeaderText="<%$ Resources:lbl_MediaType %>"></asp:BoundField>
                                <Nexus:BoundField DataField="Amount" HeaderText="<%$ Resources:lbl_Amount %>" DataType="Currency"></Nexus:BoundField>
                                <asp:BoundField DataField="AccountShortCode" HeaderText="<%$ Resources:lbl_Account %>"></asp:BoundField>
                                <asp:BoundField DataField="AllocationStatusCode" HeaderText="<%$ Resources:lbl_Status %>"></asp:BoundField>
                                <asp:BoundField DataField="Letter" HeaderText="<%$ Resources:lbl_Letter %>"></asp:BoundField>
                                <asp:TemplateField ShowHeader="False" HeaderText="<%$ Resources:lbl_Document %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDocument" runat="server" Text="<%$ Resources:lbl_PaymentLetter %>" CausesValidation="False" SkinID="btnGrid"></asp:LinkButton>
                                    </ItemTemplate>                                   
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" HeaderText="<%$ Resources:lbl_Email %>">
                                    <ItemTemplate>
                                         <asp:LinkButton ID="lnkEmail" runat="server" Text="<%$ Resources:lbl_EmailPayment %>" CommandName ="EMAILLETTER"  CausesValidation="False" SkinID="btnGrid"></asp:LinkButton>
                                    </ItemTemplate>                                   
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <div class="rowMenu">
                                            <ol class="list-inline no-margin">
                                                <li class="dropdown no-padding">
                                                    <a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle">
                                                        <i class="fa fa-ellipsis-v" aria-hidden="true"></i>
                                                    </a>
                                                    <ol id="menu_<%# Eval("Key") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                        <li id="liDelete" runat="server">
                                                            <asp:LinkButton ID="hypCashListItem" runat="server" Text="<%$ Resources:lbl_hypCashListItem %>"></asp:LinkButton>
                                                        </li>
                                                        <li id="liEdit" runat="server">
                                                            <asp:LinkButton ID="link_Delete" runat="server" CommandName="Delete" CausesValidation="false" Text="<%$ Resources:link_Delete %>"></asp:LinkButton>
                                                        </li>
                                                        <li id="liViewRecipt" runat="server">
                                                            <asp:LinkButton ID="hypView" runat="server" Text="<%$ Resources:lbl_hypView %>"></asp:LinkButton>
                                                        </li>
                                                         <li id="liViewAllocation" runat="server">
                                                            <asp:LinkButton ID="lblhypViewAllocation" runat="server" Text="<%$ Resources:btnAllocation %>"></asp:LinkButton>
                                                        </li>
                                                    </ol>
                                                </li>
                                            </ol>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:CustomValidator ID="CustVldCashListItemRequired" runat="server" ErrorMessage=""
                            ClientValidationFunction="CustVldCashListItemRequired_ClientValidation" SetFocusOnError="true"
                            Display="None" ValidationGroup="CashListItemToAllocate" />
                    </div>
                    <div class="form-inline">
                        <asp:LinkButton ID="btnPost" runat="server" SkinID="btnPrimary" Text="<%$ Resources:btn_Post %>" Enabled="false" CausesValidation="false"></asp:LinkButton>
                        <asp:LinkButton ID="btnAllocateCashlist" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btn_Allocate %>" Visible="false" ValidationGroup="CashListItemToAllocate" ></asp:LinkButton>
                        <div class="form-group form-group-sm m-l-md">
                            <asp:Label ID="lblTotal" runat="server" AssociatedControlID="txtTotal" CssClass="control-label">
                                <asp:Literal ID="ltTotal" runat="server" Text="<%$ Resources:lbl_Total %>"></asp:Literal></asp:Label>
                            <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="card-footer">
                    <%--<asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lbl_Cancel %>" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnAdd" runat="server" Text="<%$ Resources:btn_Add %>" SkinID="btnPrimary"></asp:LinkButton>--%>
                    <asp:LinkButton ID="btnOK" runat="server" Text="<%$ Resources:lbl_OK %>" Visible="false" SkinID="btnPrimary" CausesValidation="false"></asp:LinkButton>

                </div>
                    <asp:HiddenField ID="hfViewOption" runat="server" ClientIDMode="Static" />

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="drgCashListItems" EventName="RowDeleting"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="drgCashListItems" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnPost" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnAllocateCashlist" EventName="Click"></asp:AsyncPostBackTrigger>
               <%-- <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click"></asp:AsyncPostBackTrigger>--%>
                <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click"></asp:AsyncPostBackTrigger>
               <%-- <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"></asp:AsyncPostBackTrigger>--%>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upCashListsItems" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdCashListItems" runat="server">
            <progresstemplate>
            </progresstemplate>
        </Nexus:ProgressIndicator>
    </div>
    <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary" ValidationGroup="CashListItemToAllocate"></asp:ValidationSummary>
</div>
