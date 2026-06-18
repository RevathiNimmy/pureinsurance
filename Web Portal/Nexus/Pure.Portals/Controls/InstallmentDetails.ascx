<%@ Control Language="VB" AutoEventWireup="false" CodeFile="InstallmentDetails.ascx.vb"
    Inherits="Nexus.Controls_InstallmentDetails" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<script type="text/javascript">

    $(document).ready(function () {
        TotalReverseInstalment();
    });
    function Validate() {
        var isChecked = false;
        var ogrdInstallmentQuotes = document.getElementById("<%= grdInstallmentQuotes.ClientID%>")

        if (ogrdInstallmentQuotes != null) {
            ogrdInstallmentQuotes = document.getElementById("<%= grdInstallmentQuotes.ClientID%>").getElementsByTagName("tr");
            var hdnReverseInstalmentAuthority = document.getElementById("<%= hdnReverseInstalmentAuthority.ClientID%>")
            if (ogrdInstallmentQuotes.length > 0) {
                for (rowCount = 1; rowCount < ogrdInstallmentQuotes.length; rowCount++) {
                    if (ogrdInstallmentQuotes[rowCount].cells[0].getElementsByTagName("input")[0].checked == true) {
                        isChecked = true;

                        if ((ogrdInstallmentQuotes[rowCount].cells[5].innerHTML).toUpperCase() != 'COLLECTED') {
                            alert(('<%= GetLocalResourceObject("msg_InstalmentStatusCollectedOnly")%>').replace('#InstalmentNumber', ogrdInstallmentQuotes[rowCount].cells[1].innerHTML).replace('#InstalmentStatus', ogrdInstallmentQuotes[rowCount].cells[5].innerHTML));
                            return false;
                        } else if (hdnReverseInstalmentAuthority.value == '0') {
                            alert('<%= GetLocalResourceObject("msg_ReverseInstalmentAuthority")%>');
                            return false;
                        }
                        else if ((ogrdInstallmentQuotes[rowCount].cells[5].innerHTML).toUpperCase() == 'COLLECTED' && ogrdInstallmentQuotes[rowCount].cells[8].innerHTML == "1") {
                            alert(('<%= GetLocalResourceObject("msg_InstalmentReversalDayExceed")%>').replace('#InstalmentNumber', ogrdInstallmentQuotes[rowCount].cells[1].innerHTML));
                                return false;
                            }
                }
            }
            if (isChecked == false) {
                alert('<%= GetLocalResourceObject("msg_ReverseInstalmentMandatory")%>');
                    return false;
                }
                if (confirm('<%= GetLocalResourceObject("msg_ReverseInstalmentConfirm")%>') == false) {
                    return false;
                }
            }
        }
    }
    function TotalReverseInstalment() {
        var crTotalAmount = 0.00;
        var Amount = 0.00;
        var isChecked = false;
        var ogrdInstallmentQuotes = document.getElementById("<%= grdInstallmentQuotes.ClientID%>")
        if (ogrdInstallmentQuotes != null) {

            ogrdInstallmentQuotes = document.getElementById("<%= grdInstallmentQuotes.ClientID%>").getElementsByTagName("tr");
            var txtSelectedTotal = document.getElementById("<%= txtSelectedTotal.ClientID%>");
            if (ogrdInstallmentQuotes.length > 0) {
                for (rowCount = 1; rowCount < ogrdInstallmentQuotes.length; rowCount++) {
                    if (ogrdInstallmentQuotes[rowCount].cells[0].getElementsByTagName("input")[0].checked == true) {
                        isChecked = true;
                        Amount = ogrdInstallmentQuotes[rowCount].cells[4].innerHTML.replace(",", "");
                        crTotalAmount = crTotalAmount + parseFloat(cleanNumber(Amount));
                    }
                }
                var formattedValue = currencyFormat(crTotalAmount);
                txtSelectedTotal.value = formattedValue;
            }
        }
    }

    function cleanNumber(num) {
        var re = new RegExp("(£)|(\\s)|(,)|([A-Za-z])|(%)", "g");
        return num.replace(re, '');
    }
    function currencyFormat(num) {
        return num.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    }
</script>
<div id="Controls_InstallmentDetails">
    <asp:UpdatePanel runat="server" ID="updInstallments" ChildrenAsTriggers="true" UpdateMode="Always">
        <ContentTemplate>
            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="grdInstallmentQuotes" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="No Data found" DataKeyNames="PFInstalmentsKey">
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="GridItemStyletoCenter">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkReverseInstalment" runat="server" Text=" " CssClass="asp-check"  onclick="TotalReverseInstalment();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="InstalmentNumber" HeaderText="<%$ Resources:gvbf_Number %>"></asp:BoundField>
                        <asp:BoundField DataField="DueDate" HeaderText="<%$ Resources:gvbf_DueDate %>" DataFormatString="{0:d}"></asp:BoundField>
                        <asp:BoundField DataField="PostedDate" HeaderText="<%$ Resources:gvbf_DatePaid %>" DataFormatString="{0:d}"></asp:BoundField>
                        <Nexus:BoundField DataField="Amount" HeaderText="<%$ Resources:gvbf_Amount %>" DataType="Currency"></Nexus:BoundField>
                        <asp:BoundField DataField="StatusDescription" HeaderText="<%$ Resources:gvbf_Status %>"></asp:BoundField>
                        <asp:BoundField DataField="Reason" HeaderText="<%$ Resources:gvbf_Reason %>"></asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" SkinID="btnGrid" Text="<%$ Resources:Installment_btnEdit %>" Visible="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="none" HeaderStyle-CssClass="none">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnReverseInstalmentExceed" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <strong><span id="msg_noinstalment" runat="server"></span></strong>
            <asp:HiddenField ID="PlanStatus" runat="server"></asp:HiddenField>
        </ContentTemplate>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="upInstallments" OverlayCssClass="updating" AssociatedUpdatePanelID="updInstallments" runat="server">
        <progresstemplate>
        </progresstemplate>
    </Nexus:ProgressIndicator>
</div>
<div class="form-inline m-t-md">
    <div class="form-group form-group-sm">
        <asp:Label ID="lblSelectedTotal" runat="server" AssociatedControlID="txtSelectedTotal" Text="<%$ Resources:lbl_lblSelectedTotal %>" />
        <asp:TextBox ID="txtSelectedTotal" runat="server" Enabled="false" CssClass="form-control text-right m-r-md partial-TxtBox" />
    </div>
    <asp:LinkButton ID="btnReverseInstalment" runat="server" Text="<%$ Resources:btnReverseInstalment %>" SkinID="btnSecondary" OnClientClick="return Validate();" OnClick="btnReverseInstalment_Click"></asp:LinkButton>
    <asp:HiddenField ID="hdnReverseInstalmentAuthority" runat="server" />
    <asp:HiddenField ID="hdnPlanStatus" runat="server" />
    <asp:HiddenField ID="hdnReverseInstalmentNoOfDays" runat="server" />
</div>

