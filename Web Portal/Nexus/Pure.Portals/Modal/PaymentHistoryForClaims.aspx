<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PaymentHistoryForClaims.aspx.vb"
    Inherits="Nexus.Modal_PaymentHistoryForClaims" MasterPageFile="~/default.master" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_PaymentHistoryForClaims">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPaymentDetails" runat="server" Text="<%$ Resources:lbl_PaymentHistory %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="grid-card table-responsive">
                    <asp:GridView ID="drgPaymentHistory" runat="server" AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="PaymentDate" HeaderText="<%$ Resources:lbl_Date %>" DataFormatString="{0:d}" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="DocumentReference" Visible ="False" HeaderText="<%$ Resources:lbl_DocumentReference %>"></asp:BoundField>
                            <asp:BoundField DataField="PartyPaidName" HeaderText="<%$ Resources:lbl_PartyPaid %>"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_payee %>">
                                <ItemTemplate>
                                    <asp:Literal ID="lblPayeeName" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <Nexus:BoundField DataField="PaymentAmount" HeaderText="<%$ Resources:lbl_Amount %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="TaxAmount" HeaderText="<%$ Resources:lbl_TaxAmount %>" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField DataField="CurrencyDescription" HeaderText="<%$ Resources:lbl_Currency %>"></asp:BoundField>
                            <Nexus:BoundField DataField="LossAmount" HeaderText="<%$ Resources:lbl_LossAmount%>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="BaseAmount" HeaderText="<%$ Resources:lbl_BaseAmount %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="PaymentStatus" HeaderText="<%$ Resources:lbl_PaymentStatus %>" ></Nexus:BoundField>
                            <Nexus:BoundField DataField="TheirReference" HeaderText="<%$ Resources:lbl_TheirReference %>" ></Nexus:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:PaymentHistory_btnOk %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
