<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="PremiumFinancePlan.aspx.vb" Inherits="Nexus.PremiumFinance_PremiumFinancePlan" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript" language="javascript">
        function setClient(sClientName, sClientKey) {
            tb_remove();
            document.getElementById('<%=txtClientCode.ClientId %>').value = sClientName;
            document.getElementById('<%=txtClientCode.ClientId %>').focus();
            document.getElementById('<%=hvAgentKey.ClientId %>').value = sClientKey;
        }
        $(document).ready(function () {
            document.getElementById('<%=txtClientCode.ClientId %>').readOnly = true;
        });

    </script>

    <div id="Premiun_Finance_Plan">
        <asp:Panel ID="PnlPremiumFinancePlan" CssClass="card" runat="server">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClientCode" Text="<%$ Resources:btn_ClientCode%>" ID="lblbtnClientCode"></asp:Label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtClientCode" runat="server" CssClass="field-mandatory form-control"></asp:TextBox><span class="input-group-btn">
                                    <asp:LinkButton ID="btnClientCode" runat="server" CausesValidation="false" associatedcontrolid="txtClientCode" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Client Code</span>
                                    </asp:LinkButton></span>
                            </div>
                        </div>
                        <%--Text="000111"--%>
                        <asp:RequiredFieldValidator ID="vldClientCodeRequired" runat="server" Display="none" ControlToValidate="txtClientCode" ErrorMessage="<%$ Resources:ClientCode_ErrorMessage%>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hvAgentName" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hvAgentKey" runat="server"></asp:HiddenField>
                        <%--Value="1553"--%>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="<%$ Resources:lbl_Status %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control form-select" AutoPostBack="true" CausesValidation="true">
                                <asp:ListItem Value="000" Text="All"></asp:ListItem>
                                <asp:ListItem Value="040" Text="Live" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="010" Text="Saved"></asp:ListItem>
                                <asp:ListItem Value="011" Text="Updated"></asp:ListItem>
                                <asp:ListItem Value="012" Text="Quote Printed"></asp:ListItem>
                                <asp:ListItem Value="140" Text="On Hold"></asp:ListItem>
                                <asp:ListItem Value="900" Text="Completed"></asp:ListItem>
                                <asp:ListItem Value="990" Text="Superseded"></asp:ListItem>
                                <asp:ListItem Value="999" Text="Cancelled"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdDd1Status" runat="server" ControlToValidate="ddlStatus" Display="none" Enabled="true" ValidationGroup="" SetFocusOnError="true" ErrorMessage="<%$ Resources:Status_ErrorMessage%>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimNumber" runat="server" AssociatedControlID="txtClaimNumber" Text="Claim Number" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimNumber" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton runat="server" ID="btnFind" Text="<%$ Resources:btn_Find %>" Visible="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </asp:Panel>

        <div class="grid-card table-responsive">
            <asp:GridView ID="grdvSearchResults" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" DataKeyNames="FinancePlanKey,FinancePlanVersion" AutoGenerateColumns="false" AllowSorting="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                <Columns>
                    <asp:BoundField DataField="FinanceProvider" HeaderText="<%$ Resources:lblFinance_Provider%>" SortExpression="FinanceProvider"></asp:BoundField>
                    <asp:BoundField DataField="Insuranceref" HeaderText="<%$ Resources:lblPolicy_Number %>" SortExpression="FinancePlanKey"></asp:BoundField>
                    <asp:BoundField DataField="PlanReference" HeaderText="<%$ Resources:lbl_PlanReference %>" SortExpression="FinancePlanKey"></asp:BoundField>
                    <asp:BoundField DataField="AccountNumber" HeaderText="<%$ Resources:lblAccount_No %>" SortExpression="AccountNumber"></asp:BoundField>
                    <nexus:BoundField DataField="Amount" HeaderText="<%$ Resources:lblAmount %>" SortExpression="Amount" DataType="Currency"></nexus:BoundField>
                    <asp:BoundField DataField="Frequency" HeaderText="<%$ Resources:lblFrequency %>" SortExpression="Frequency"></asp:BoundField>
                    <asp:BoundField DataField="NextDueDate" HeaderText="<%$ Resources:lblNext_Due %>" DataFormatString="{0:d}" SortExpression="NextDueDate"></asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="<%$ Resources:lblStatus1 %>" SortExpression="Status"></asp:BoundField>
                    <asp:BoundField DataField="RemainingInstalments" HeaderText="<%$ Resources:lblRemaining %>" SortExpression="RemainingInstalments"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" Text="<%$ Resources:FindPremium_btnEdit %>" SkinID="btnGrid" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="false"></asp:LinkButton>
                            <asp:LinkButton ID="btnView" runat="server" Text="<%$ Resources:FindPremium_btnView %>" SkinID="btnGrid" CommandName="View" CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="false"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="p-v-sm text-right">
            <asp:LinkButton ID="btnNewPlan" runat="server" Text="<%$ Resources:lblNew %>" CausesValidation="true" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
        </div>

        <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
