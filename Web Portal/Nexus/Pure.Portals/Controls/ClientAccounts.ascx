<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClientAccounts.ascx.vb"
    Inherits="Nexus.Controls_ClientAccounts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<script language="javascript" type="text/javascript">

    function collapseAccountGrid() {
        //Collapse all child grids as default
        $("[id$='_gvClientAccountChild']").css("display", "none");
    }

    function toggleExpandCollapse(obj, flag) {
        var imgid = document.getElementById(obj).id;
        if (flag) {
            document.getElementById(obj.substr(0, 65) + 'gvClientAccountChild').style.display = 'none';
            document.getElementById(obj).style.display = 'none';
            document.getElementById(obj.substr(0, 68) + 'Expand').style.display = 'block';
            $('#' + imgid).closest("tr").removeClass('grid-sel-r');

        }
        else {
            var grid = document.getElementById(obj.substr(0, 65) + 'gvClientAccountChild')
            document.getElementById(obj.substr(0, 65) + 'gvClientAccountChild').style.display = 'block';
            document.getElementById(obj).style.display = 'block';
            document.getElementById(obj.substr(0, 68) + 'Collapse').style.display = 'block';
            document.getElementById(obj.substr(0, 68) + 'Expand').style.display = 'none';
            $('#' + imgid).closest("tr").addClass('grid-sel-r');
        }
    }
</script>

<div id="Controls_ClientAccounts">
    <asp:Panel ID="pnlClientAccounts" runat="server" Visible="true">
        <asp:UpdatePanel ID="updClientAccounts" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="card">
                    <div class="card-heading">
                        <h1>
                            <asp:Literal ID="litAccountsHeader" runat="server" Text="<%$ Resources:lbl_Account_header %>"></asp:Literal></h1>
                    </div>
                    <div class="card-body clearfix">
                        <div class="col-lg-6 col-md-12">
                            <div class="form-horizontal">
                                <div class="form-group form-group-sm ">
                                    <asp:Label ID="lblDocumentType" runat="server" AssociatedControlID="DocumentType" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_DocumentType%>"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="DocumentType" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="pmlookup" ListCode="DOCUMENTTYPE" CssClass="form-control form-select" TabIndex="1" DefaultText="All"></NexusProvider:LookupList>
                                    </div>
                                </div>

                                <div class="form-group form-group-sm">
                                    <asp:Label ID="lblDocumentRef" runat="server" AssociatedControlID="txtDocumentRef" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_DocumentRef%>"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtDocumentRef" TabIndex="2" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group form-group-sm">
                                    <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_PolicyNumber%>"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtPolicyNumber" TabIndex="3" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-12">
                            <div class="form-horizontal">
                                <div class="form-group form-group-sm">
                                    <div class="col-lg-12 col-sm-12">
                                        <asp:CheckBox ID="chkOutstandingTransaction" runat="server" TabIndex="4" Text="<%$ Resources:lbl_OutstandingTransaction%>" Checked="true" CssClass="asp-check"></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm">
                                    <div class="col-lg-12 col-sm-12">
                                        <asp:CheckBox ID="chkExcludeInstalments" runat="server" text="<%$ Resources:lbl_ExcludeInstalments%>" TabIndex="5" Checked="false" CssClass="asp-check"></asp:CheckBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer">
                        <asp:LinkButton ID="btnSearch" runat="server" Text="<%$ Resources:btnSearch %>" EnableViewState="false" OnClick="btnSearch_Click" CausesValidation="false" SkinID="btnPrimary"></asp:LinkButton>
                    </div>
                </div>
                <div class="text-right p-v-sm">
                    <div class="form-inline">
                        <div class="form-group form-group-sm">
                            <asp:Label ID="lblAmountOutstanding" runat="server" AssociatedControlID="txtAmountOutstanding" CssClass="m-r-sm font-bold text-dark">
                                <asp:Literal ID="ltAmountOutstanding" runat="server" Text="<%$ Resources:lbl_AmountOutstanding%>"></asp:Literal></asp:Label>
                            <asp:TextBox ID="txtAmountOutstanding" runat="server" CssClass="form-control Doub" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnlAccounts" runat="server">
                    <div class="grid-card table-responsive no-margin">
                        <asp:UpdatePanel ID="updPanelClientQuotes" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvClientAccountParent" runat="server" PageSize="10" AllowSorting="true" AutoGenerateColumns="false" GridLines="None" AllowPaging="true" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle CssClass="border"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Image ID="ImgCollapse" runat="server" ImageUrl="~/images/minus.png" Style="display: none;" onClick="toggleExpandCollapse(this.id,1);"></asp:Image>
                                                <asp:Image ID="ImgExpand" runat="server" ImageUrl="~/images/plus.png" onClick="toggleExpandCollapse(this.id,0);"></asp:Image>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="<%$ Resources:lbl_DocRef %>" DataField="DocRef" SortExpression="DocRef"></asp:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:lbl_DocType %>" DataField="DocumentTypeCode" SortExpression="DocumentTypeCode"></asp:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:lbl_EffectiveDate %>" DataField="EffectiveDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="EffectiveDate"></asp:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:lbl_TransDate %>" DataField="TransDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="TransDate"></asp:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:lbl_MediaType %>" DataField="MediaType" SortExpression="MediaType"></asp:BoundField>
                                        <nexus:BoundField HeaderText="<%$ Resources:lbl_AccountAmount %>" DataField="AccountAmount" DataType="Currency"></nexus:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:lbl_PaidDate %>" DataField="PaidDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="PaidDate"></asp:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:lbl_PolicyNumber %>" DataField="Reference" SortExpression="Reference"></asp:BoundField>
                                        <nexus:BoundField HeaderText="<%$ Resources:lbl_AccountOutStandingAmount %>" DataField="AccountOutStandingAmount" DataType="Currency"></nexus:BoundField>
                                        <nexus:BoundField HeaderText="<%$ Resources:lbl_CurrencyAmount %>" DataField="CurrencyAmount" DataType="Currency"></nexus:BoundField>
                                        <nexus:BoundField HeaderText="<%$ Resources:lbl_OutStandingCurrencyAmount %>" DataField="OutStandingCurrencyAmount" DataType="Currency"></nexus:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:lbl_Plan %>" ShowHeader="false">
                                            <ItemTemplate>
                                                <div class="rowMenu">
                                                    <ol id='ViewPlan_<%# Eval("TransdetailId") %>' class="list-inline no-margin">
                                                        <li>
                                                            <asp:LinkButton ID="lnkbtnViewPlan" runat="server" CausesValidation="False" SkinID="btnGrid" Text="<i class='fa fa-eye' aria-hidden='true'></i> View Plan" CommandName="ViewPlan"></asp:LinkButton></li>
                                                    </ol>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <tr>
                                                    <td colspan="100%">
                                                        <div class="grid-card table-responsive no-margin">
                                                            <asp:GridView ID="gvClientAccountChild" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvClientAccountChild_RowDataBound"
                                                                OnDataBinding="gvClientAccountChild_DataBinding" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_EffectiveDate %>" DataField="EffectiveDate" HtmlEncode="False" DataFormatString="{0:d}"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_TransDate %>" DataField="TransDate" HtmlEncode="False" DataFormatString="{0:d}"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_MediaType %>" DataField="MediaType"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_MediaRef %>" DataField="MediaRef"></asp:BoundField>
                                                                    <nexus:BoundField HeaderText="<%$ Resources:lbl_AccountAmount %>" DataField="AccountAmount" DataType="Currency"></nexus:BoundField>
                                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_PaidDate %>" DataField="PaidDate" HtmlEncode="False" DataFormatString="{0:d}"></asp:BoundField>
                                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_PolicyNumber %>" DataField="Reference"></asp:BoundField>
                                                                    <nexus:BoundField HeaderText="<%$ Resources:lbl_AccountOutStandingAmount %>" DataField="AccountOutStandingAmount" DataType="Currency"></nexus:BoundField>
                                                                    <nexus:BoundField HeaderText="<%$ Resources:lbl_CurrencyAmount %>" DataField="CurrencyAmount" DataType="Currency"></nexus:BoundField>
                                                                    <nexus:BoundField HeaderText="<%$ Resources:lbl_OutStandingCurrencyAmount %>" DataField="OutStandingCurrencyAmount" DataType="Currency"></nexus:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvClientAccountParent" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <nexus:ProgressIndicator ID="upClientAccounts" OverlayCssClass="updating" AssociatedUpdatePanelID="updPanelClientQuotes" runat="server">
                            <ProgressTemplate>
                            </ProgressTemplate>
                        </nexus:ProgressIndicator>
                    </div>
                </asp:Panel>


            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

</div>
