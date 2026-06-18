<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="CashDepositPayment.aspx.vb" Inherits="Nexus.secure_payment_CashDepositPayment" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smCashDepositPayment" runat="server"></asp:ScriptManager>
    <div id="secure_payment_CashDepositPayment">
        <div id="nexus-container">
            <div class="card">
                <div class="card-heading">
                    <h1>
                        <asp:Literal ID="ltHeading" runat="server" Text="<%$ Resources:ltHeading %>"></asp:Literal></h1>
                </div>
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <legend>
                            <asp:Literal ID="fdHeading" runat="server" Text="<%$ Resources:fdHeading %>"></asp:Literal></legend>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" id="olPartyType" runat="server">
                            <asp:Label runat="server" ID="lblPartyType" Text="<%$ Resources:lblPartyType %>" AssociatedControlID="radioUserType" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:RadioButtonList ID="radioUserType" runat="server" RepeatDirection="horizontal" AutoPostBack="true" CssClass="asp-radio">
                                    <asp:ListItem Text="<%$ Resources:listitem_Client %>" Value="Client"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:listitem_Agent %>" Value="Agent"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblPartyCode" runat="server" AssociatedControlID="lblPartyCodeValue" Text="<%$ Resources:lblPartyCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <asp:Label ID="lblPartyCodeValue" CssClass="field-medium" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblPartyName" runat="server" AssociatedControlID="lblPartyNameValue" Text="<%$ Resources:lblPartyName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <asp:Label ID="lblPartyNameValue" CssClass="field-medium" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdCashDepositPay" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="grid-card table-responsive no-margin">
                            <asp:GridView ID="grdvCDDetailsForAgents" runat="server" AlternatingRowStyle-BorderStyle="none" AutoGenerateColumns="False" GridLines="None" PageSize="10" AllowPaging="True" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField HeaderText="<%$ Resources:CDNumber %>" DataField="CashDepositRef"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:AvailBalance %>" DataField="AvailableBalance"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:CollectionDate %>" DataField="DateCreated" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id="menu_<%# Eval("CashDepositKey") %>" class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton ID="lnkSelect" runat="server" Text="<%$ Resources:lbl_Select %>" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CashDepositRef")%>' SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="grid-card table-responsive no-margin">
                                <asp:GridView ID="grdvCDDetailsForClients" runat="server" AlternatingRowStyle-BorderStyle="none" AutoGenerateColumns="False" GridLines="None" CellPadding="0" CellSpacing="0" PageSize="10" AllowPaging="True" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                    <Columns>
                                        <asp:BoundField HeaderText="<%$ Resources:CDNumber %>" DataField="CashDepositRef"></asp:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:AvailBalance %>" DataField="AvailableBalance"></asp:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:CollectionDate %>" DataField="DateCreated" DataFormatString="{0:d}"></asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="rowMenu">
                                                    <ol id="menu_<%# Eval("CashDepositKey") %>" class="list-inline no-margin">
                                                        <li>
                                                            <asp:LinkButton ID="lnkSelect" runat="server" Text="<%$ Resources:lbl_Select %>" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CashDepositRef")%>'></asp:LinkButton>
                                                        </li>
                                                    </ol>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdvCDDetailsForAgents" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvCDDetailsForAgents" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvCDDetailsForAgents" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvCDDetailsForClients" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvCDDetailsForClients" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvCDDetailsForClients" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="UpCashDepositPayment" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdCashDepositPay" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </Nexus:ProgressIndicator>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" Text="<%$ Resources:btn_Cancel %>" runat="server" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
