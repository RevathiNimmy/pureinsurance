<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="FindCDAccount.aspx.vb" Inherits="Nexus.secure_FindCDAccount" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntScriptIncludes" ContentPlaceHolderID="ScriptIncludes" runat="Server">

    <script language="javascript" type="text/javascript">
        function setAgent(sName, sKey, sCode) {
            tb_remove();
            //User has selected An Agent so set the selected values and make Client values Clear
            document.getElementById('<%= txtAgentCode.ClientId%>').value = unescape(sCode);
            document.getElementById('<%= txtClient.ClientId%>').value = '';
            document.getElementById('<%= txtClientKey.ClientId%>').value = '';
            document.getElementById('<%= txtAgentCode.ClientId%>').focus();
            EnableDisableClientCode();
        }

        function setClient(sClientName, sClientKey) {
            tb_remove();
            //User has selected a Client so set the selected values and make an Agent values Clear
            document.getElementById('<%= txtClient.ClientId%>').value = unescape(sClientName);
            document.getElementById('<%= txtClientKey.ClientId%>').value = sClientKey;
            document.getElementById('<%= txtAgentCode.ClientId%>').value = '';
            document.getElementById('<%= txtClient.ClientId%>').focus();
            EnableDisableAgentCode();
        }

        function EnableDisableClientCode() {
            if (document.getElementById('<%= txtAgentCode.ClientId%>').value != "") {
                //set the Client button disable and textboxs disable+blank, if a value is present in the Agent code text box
                document.getElementById('<%= txtClient.ClientId%>').value = "";
                document.getElementById('<%= txtClient.ClientId%>').disabled = true;
                document.getElementById('<%= btnClient.ClientId%>').disabled = true;
            }
            else {
                //set the Client button enable and textboxs enable+blank, if a value is not present in the Agent code text box
                document.getElementById('<%= txtClient.ClientId%>').value = "";
                document.getElementById('<%= txtClient.ClientId%>').disabled = false;
                document.getElementById('<%= btnClient.ClientId%>').disabled = false;
            }
        }

        function EnableDisableAgentCode() {
            if (document.getElementById('<%= txtClient.ClientId%>').value != "") {
                //set the Agent button disable and textboxs disable+blank, if a value is present in the Client code text box
                document.getElementById('<%= txtAgentCode.ClientId%>').value = "";
                document.getElementById('<%= txtAgentCode.ClientId%>').disabled = true;
                document.getElementById('<%= btnAgent.ClientId%>').disabled = true;
            }
            else {
                //set the Client button enable and textboxs enable+blank, if a value is not present in the Client code text box
                document.getElementById('<%= txtAgentCode.ClientId%>').value = "";
                document.getElementById('<%= txtAgentCode.ClientId%>').disabled = false;
                document.getElementById('<%= btnAgent.ClientId%>').disabled = false;
            }
        }

        function SelectClientorAgent(source, arguments) {
            var sAgentCode = document.getElementById('<%= txtAgentCode.ClientId%>').value;
            var sClientCode = document.getElementById('<%= txtClient.ClientId%>').value;

            //Select an Agent or Client
            if (sAgentCode == '' && sClientCode == '')
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
    </script>

</asp:Content>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="secure_FindCDAccount">
        <asp:Panel ID="pnlFindCDAccount" runat="server" CssClass="card" DefaultButton="btnSearch">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="litFindCDAccountHeader" runat="server" Text="<%$ Resources:lbl_FindCDAccount_header %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClient" Text="<%$ Resources:btnClient%>" ID="lblbtnClient"></asp:Label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtClient" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox><span class="input-group-btn">
                                    <asp:LinkButton ID="btnClient" runat="server" TabIndex="1" CausesValidation="false" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Client</span>
                                    </asp:LinkButton></span>
                            </div>
                        </div>


                        <asp:HiddenField ID="txtClientKey" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAgentCode" Text="<%$ Resources:btnAgent%>" ID="lblbtnAgent"></asp:Label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtAgentCode" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox><span class="input-group-btn">
                                    <asp:LinkButton ID="btnAgent" runat="server" TabIndex="3" CausesValidation="false" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Agent Code</span>
                                    </asp:LinkButton></span>
                            </div>
                        </div>


                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblCDNumber" AssociatedControlID="txtCDNumber" Text="<%$ Resources:lblCDNumber%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox runat="server" ID="txtCDNumber" CssClass="form-control" TabIndex="5"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblBankName" AssociatedControlID="GISLookup_CashListItemBank" Text="<%$ Resources:btn_BankName%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISLookup_CashListItemBank" runat="server" DataItemText="Description" DataItemValue="Code" ListCode="CashListItem_Bank" ListType="PMLookup" CssClass="field-medium form-control form-select" DefaultText="<%$ Resources:lbl_BankDefault%>" TabIndex="6"></NexusProvider:LookupList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:lbl_NewSearch %>" TabIndex="9" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNew" runat="server" Text="<%$ Resources:lbl_New%>" TabIndex="7" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnSearch" runat="server" Text="<%$ Resources: btnSearch %>" TabIndex="8" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </asp:Panel>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_Err_WildCardAtEnd %>" NoWildCardErrorMessage="<%$ Resources:lbl_Err_NoWildCard %>" ControlsToValidate="txtClient,txtAgentCode,txtCDNumber" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>
        <asp:CustomValidator ID="CustVld_AnagentorClient" runat="server" ClientValidationFunction="SelectClientorAgent" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:lbl_Err_SelectAgentorClient %>"></asp:CustomValidator>
        <asp:CustomValidator ID="CustVld_ValidAgentorClient" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:lbl_Err_ValidAgentorClient %>"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:UpdatePanel ID="UpdCDAccount" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvCDAccount" runat="server" AutoGenerateColumns="False" GridLines="None" PageSize="10" AllowPaging="True" PagerSettings-Mode="Numeric" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                        <Columns>
                            <asp:BoundField HeaderText="<%$ Resources:BankName %>" DataField="BankName"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:CDNumber %>" DataField="CashDepositRef"></asp:BoundField>
                            <Nexus:BoundField HeaderText="<%$ Resources:AvailableBalance %>" DataField="AvailableBalance" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:Party %>" DataField="PartyName"></asp:BoundField>
                            <asp:BoundField HeaderText="PartyCode" DataField="PartyCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:Product %>" DataField="ProductName"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:Branch %>" DataField="BranchName"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id='menu_<%# Eval("CashDepositKey") %>' class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="lnkSelect" runat="server" Text="<%$ Resources:btnSelect %>" SkinID="btnGrid"></asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle BorderStyle="None"></AlternatingRowStyle>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvCDAccount" EventName="DataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvCDAccount" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvCDAccount" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upCDAccount" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdCDAccount" runat="server">
            <progresstemplate>
                                </progresstemplate>
        </Nexus:ProgressIndicator>
    </div>
</asp:Content>
