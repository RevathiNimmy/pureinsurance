<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Viewallocation.aspx.vb"
    Inherits="Modal_Viewallocation" MasterPageFile="~/default.master" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript" language="javascript">
        function ShowMsg(sErrorMessage) {
            alert(sErrorMessage);
            return false;
        }
        function ReverseAmountWarning(sReverseAmountWarning) {
            alert(sReverseAmountWarning);
            return false;
        }

        function ReversalConfirmation(sWarningMessage, sSplitMessage, bWarnning, sTotalWarning) {
            var IsSplitReceipt = getQuerystring('IsSplitReceipt');
            var IsLeadAgent = getQuerystring('IsLeadAgent');
            if (bWarnning == "false") {
                alert(sTotalWarning);
                document.getElementById('<%=hidChkChoice.ClientID %>').value = false;
                return false;
            }
            if (IsSplitReceipt == "True" && IsLeadAgent == "True") {
                var Split = confirm(sSplitMessage);
                if (Split) {
                    var result = confirm(sWarningMessage);
                    document.getElementById('<%=hidChkChoice.ClientID %>').value = result;
                    return result;
                }
            }
            else {
                var result = confirm(sWarningMessage);
                document.getElementById('<%=hidChkChoice.ClientID %>').value = result;
                return result;
            }
        }

        function getQuerystring(key, default_) {

            if (default_ == null) default_ = "";
            key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
            var qs = regex.exec(window.location.href);
            if (qs == null)
                return default_;
            else
                return qs[1];

        }
    </script>
   <asp:ScriptManager ID="ScriptManager1" runat="server" />
   <style type="text/css">.hidden-col { display: none; }</style>
    <div id="Modal_Viewallocation">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal></h1>
            </div>
            <asp:UpdatePanel ID="updViewAllocation" runat="server" UpdateMode="Always">
            <ContentTemplate>
            <div class="card-body clearfix">
                <legend>
                    <asp:Label ID="lblViewallocation" runat="server" Text="<%$ Resources:lbl_Credit %>"></asp:Label></legend>
                <asp:Label ID="lblInformation" runat="server" Visible="false"></asp:Label>
                <div class="grid-card table-responsive maxheight">
                    <asp:GridView ID="gvCredits" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" ShowFooter="true" EmptyDataText="<%$ Resources:ErrorMessage %>">

                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="checkAllCredit" runat="server" AutoPostBack="true" OnCheckedChanged="chkCreditSelectAll_OnCheckedChanged" Text=" " CssClass="asp-check" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCredit" runat="server" AutoPostBack="true" OnCheckedChanged="chkCredit_OnCheckedChanged" Text=" " CssClass="asp-check" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocRef %>" DataField="DocRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_TransDate %>" DataField="TransDate" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_AllocatedDate %>" DataField="AllocatedDate" DataFormatString="{0:d}"></asp:BoundField>
                            <Nexus:BoundField HeaderText="<%$ Resources:lbl_AllocatedAmount %>" DataField="AllocatedAmount" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField HeaderText="<%$ Resources:lbl_OriginalAmount %>" DataField="OriginalAmount" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField HeaderText="<%$ Resources:lbl_WriteOffAmount %>" DataField="WriteOffAmount" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocType %>" DataField="DocType"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_InsuranceRef %>" DataField="InsuranceRef" FooterText="<%$ Resources:lbl_TotalReversalAmount %>"></asp:BoundField>
                            <%-- <asp:BoundField HeaderText="<%$ Resources:lbl_Account %>" DataField="Account"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_User %>" DataField="User"></asp:BoundField>--%>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_TransdetailKey %>" DataField="TransdetailKey" ItemStyle-CssClass="hidden-col" HeaderStyle-CssClass="hidden-col" FooterStyle-CssClass="hidden-col"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_AmountReverse %>" ItemStyle-CssClass="GridAmtCol" FooterStyle-CssClass="GridAmtCol"
                                HeaderStyle-CssClass="GridAmtCol" ControlStyle-CssClass="GridAmtCol">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAllocated" runat="server" Enabled="true" Visible="true" CssClass="FormatDecimal Doub form-control form-control-textbox"
                                        AutoCompleteType="None" OnTextChanged="txtAllocated_TextChanged" onClick="this.select();" AutoPostBack="true"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="text-end" style="text-align: right!important; padding-right: 12px;">
                                        <asp:Label ID="lblCreditAllocatedTotal" runat="server" />
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>               
                <legend>
                    <asp:Label ID="lblDebit" runat="server" Text="<%$ Resources:lbl_Debit %>"></asp:Label></legend>
                <div class="grid-card table-responsive maxheight">
                    <asp:GridView ID="gvDebit" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" ShowFooter="true" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="checkAllDebit" runat="server" AutoPostBack="true" OnCheckedChanged="chkDebitSelectAll_OnCheckedChanged" Text=" " CssClass="asp-check" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkDebit" runat="server" CommandName="Select" CausesValidation="false" AutoPostBack="true" OnCheckedChanged="chkDebit_OnCheckedChanged" Text=" " CssClass="asp-check" />
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocRef %>" DataField="DocRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_TransDate %>" DataField="TransDate" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_AllocatedDate %>" DataField="AllocatedDate" DataFormatString="{0:d}"></asp:BoundField>
                            <Nexus:BoundField HeaderText="<%$ Resources:lbl_AllocatedAmount %>" DataField="AllocatedAmount" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField HeaderText="<%$ Resources:lbl_OriginalAmount %>" DataField="OriginalAmount" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField HeaderText="<%$ Resources:lbl_WriteOffAmount %>" DataField="WriteOffAmount" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocType %>" DataField="DocType"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_InsuranceRef %>" DataField="InsuranceRef" FooterText="<%$ Resources:lbl_TotalReversalAmount %>"></asp:BoundField>
                            <%-- <asp:BoundField HeaderText="<%$ Resources:lbl_Account %>" DataField="Account"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_User %>" DataField="User"></asp:BoundField>--%>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_TransdetailKey %>" DataField="TransdetailKey" ItemStyle-CssClass="hidden-col" HeaderStyle-CssClass="hidden-col" FooterStyle-CssClass="hidden-col"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_AmountReverse %>" ItemStyle-CssClass="GridAmtCol" FooterStyle-CssClass="GridAmtCol"
                                HeaderStyle-CssClass="GridAmtCol" ControlStyle-CssClass="GridAmtCol">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAllocatedDebit" runat="server" Enabled="true" Visible="true" CssClass="FormatDecimal Doub form-control form-control-textbox"
                                        AutoCompleteType="None" OnTextChanged="txtAllocatedDebit_TextChanged" onClick="this.select();" AutoPostBack="true"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="text-end" style="text-align: right!important; padding-right: 12px;">
                                        <asp:Label ID="lblDebitAllocatedTotal" runat="server" />
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>                   
                    <asp:HiddenField ID="hidChkAuthority" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hidTransID" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hidAllocID" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hidNoOfDays" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hidAllocationDate" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hidChkChoice" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hidExpAlloc" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hidAllocationTimeStamp" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hidAccountKey" runat="server"></asp:HiddenField>
                </div>
            </div>

            <div class="card-footer">
                <asp:LinkButton ID="btnClose" runat="server" Text="<%$ Resources:btnClose %>" OnClientClick="self.parent.tb_remove(); return false;" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnReverseAllocation" runat="server" Text="<%$ Resources:btnReverseAllocation %>" Visible="false" SkinID="btnPrimary"></asp:LinkButton>

            </div>
                      
                <asp:Literal ID="lblPartialAllocNote" runat="server" visible="false" Text="<%$ Resources:PartialAllocNote%>"></asp:Literal>
               </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
