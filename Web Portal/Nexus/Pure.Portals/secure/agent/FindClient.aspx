<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindClient.aspx.vb" Inherits="Nexus.secure_FindClient"
    MasterPageFile="~/default.master" EnableViewState="true" ValidateRequest="false" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smFindCllient" runat="server"></asp:ScriptManager>
    <script type="text/javascript" language="javascript">
        function ClearPartyIndexCall() {
            var PartyIndex = $("#<%= txtPartyIndex.ClientID %>").val();
            $("#aspnetForm").find('input:text, input:password, input:file, select, textarea').val('');
            $("#aspnetForm").find('input:checkbox').attr('checked', false);
            $("#aspnetForm").find('select option[value="ANY"]').attr("selected", "selected");
            $("#<%= txtPartyIndex.ClientID %>").val(PartyIndex);
            $("#<%= btnSearch.ClientID %>").attr('disabled', false);
        }

        function ClearRiskIndexCall() {
            var RiskIndex = $("#<%= txtPolicyRiskIndex.ClientID %>").val();
            $("#aspnetForm").find('input:text, input:password, input:file, select, textarea').val('');
            $("#aspnetForm").find('input:checkbox').attr('checked', false);
            $("#aspnetForm").find('select option[value="ANY"]').attr("selected", "selected");
            $("#<%= txtPolicyRiskIndex.ClientID %>").val(RiskIndex);
            $("#<%= btnSearch.ClientID %>").attr('disabled', false);
        }

        function ContinueWithServiceLevel(sServiceLevel) {
            var IsConfirm = true;
            sServiceLevel = sServiceLevel.trim().toUpperCase();
            if (sServiceLevel == "RESTRICTED") {
                IsConfirm = window.confirm("Client is Restricted, do you want to continue?", "Service Level Warning");
            }
            if (sServiceLevel == "OBJECTED") {
                IsConfirm = window.confirm("Client has Objected, do you want to continue?", "Service Level Warning");
            }
            return IsConfirm;
        }

    </script>
    <div id="secure_agent_FindClient">
        <asp:Panel ID="pnlFindBG" runat="server" CssClass="card" DefaultButton="btnSearch">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="litFindClientHeader" runat="server" Text="<%$ Resources:lbl_FindClient_header %>" EnableViewState="false"></asp:Literal>
                    <asp:Literal ID="litHeaderMessage" runat="server" Text="<%$ Resources:lbl_FindClient_headerMessage_SaveQuote %>" EnableViewState="false" Visible="false"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClientName" runat="server" AssociatedControlID="txtClientName" Text="<%$ Resources:lblClientName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClientName" TabIndex="1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClientCode" runat="server" AssociatedControlID="txtClientCode" Text="<%$ Resources:lblClientCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClientCode" TabIndex="2" CssClass="form-control" runat="server" MaxLength="20"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFileCode" runat="server" AssociatedControlID="txtFileCode" Text="<%$ Resources:lblFileCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFileCode" TabIndex="3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" Text="<%$ Resources:lblAddress %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAddress" TabIndex="4" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" Text="<%$ Resources:lblPolicyNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyNumber" TabIndex="5" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyRiskIndex" runat="server" AssociatedControlID="txtPolicyRiskIndex" Text="<%$ Resources:lblPolicyRiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyRiskIndex" TabIndex="6" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div id="liCaseNumber" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCaseNumber" runat="server" AssociatedControlID="txtCaseNumber" Text="<%$ Resources:lblCaseNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCaseNumber" TabIndex="7" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimNumber" runat="server" AssociatedControlID="txtClaimNumber" Text="<%$ Resources:lblClaimNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimNumber" TabIndex="8" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimRiskIndex" runat="server" AssociatedControlID="txtClaimRiskIndex" Text="<%$ Resources:lblClaimRiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimRiskIndex" TabIndex="9" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" Text="<%$ Resources:lblPhone %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPhone" TabIndex="10" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDOB" runat="server" AssociatedControlID="txtDOB" Text="<%$ Resources:lblDOB %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtDOB" TabIndex="11" CssClass="form-control" runat="server"></asp:TextBox>
                                <uc1:CalendarLookup ID="calEventFromDate" runat="server" LinkedControl="txtDOB" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RangeValidator ID="rvDOB" runat="Server" Type="Date" Display="none" ControlToValidate="txtDOB" MinimumValue="01/01/1900" ErrorMessage="<%$ Resources:lbl_InvalidDOBFormat %>"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPostcode" runat="server" AssociatedControlID="txtPostcode" Text="<%$ Resources:lblPostcode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPostcode" TabIndex="12" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClientType" runat="server" AssociatedControlID="ddlClientType" Text="<%$ Resources:lbl_ClientType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlClientType" TabIndex="13" CssClass="field-medium form-control form-select" runat="server">
                                <asp:ListItem Value="ANY" Text="<%$ Resources:ddlClientType_ListItem1 %>"></asp:ListItem>
                                <asp:ListItem Value="CC" Text="<%$ Resources:ddlClientType_ListItem3 %>"></asp:ListItem>
                                <asp:ListItem Value="PC" Text="<%$ Resources:ddlClientType_ListItem2 %>"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="<%$ Resources:lbl_Status %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlStatus" TabIndex="14" CssClass="field-medium form-control " runat="server">
                                <asp:ListItem Value="ANY" Text="<%$ Resources:ddlStatusType_ListItem1 %>"></asp:ListItem>
                                <asp:ListItem Value="CLIENT" Text="<%$ Resources:ddlStatusType_ListItem2 %>"></asp:ListItem>
                                <asp:ListItem Value="PROSPECT" Text="<%$ Resources:ddlStatusType_ListItem3 %>"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPartyIndex" runat="server" AssociatedControlID="txtPartyIndex" Text="<%$ Resources:lblPartyIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPartyIndex" TabIndex="14" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblIncludeClosedBranches" runat="server" AssociatedControlID="chkIncludeClosedBranches" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litIncludeClosedBranches" runat="server" Text="<%$ Resources:chkIncludeClosedBranches  %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:CheckBox ID="chkIncludeClosedBranches" runat="server" TabIndex="15" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btnClear %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnPersonalCustomer" runat="server" TabIndex="17" Text="<%$ Resources:btnPersonalCustomer %>" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnCorporateCustomer" runat="server" TabIndex="18" Text="<%$ Resources:btnCorporateCustomer %>" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnSearch" runat="server" TabIndex="16" Text="<%$ Resources:btnSearch %>" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </asp:Panel>

        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="No wild card except at the end please" NoWildCardErrorMessage="No Wild card is allowed" ControlsToValidate="txtClientName,txtClientCode,txtFileCode,txtAddress,txtPolicyNumber,txtPolicyRiskIndex,txtClaimNumber,txtClaimRiskIndex,txtPhone,txtPostcode" Condition="Auto" Display="none" runat="server" EnableClientScript="true"></Nexus:WildCardValidator><asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>

        <div class="grid-card table-responsive">
            <asp:UpdatePanel ID="uPFindClient" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                <ContentTemplate>
                    <asp:GridView ID="grdvSearchResults" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" OnPageIndexChanging="grdvSearchResults_PageIndexChanging" OnRowCommand="grdvSearchResults_RowCommand" DataKeyNames="UserName" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" OnDataBound="grdvSearchResults_DataBound" CssClass="noborder">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:lblRegistration_Firstname_g %>"></asp:BoundField>
                            <asp:BoundField DataField="ResolvedName" HeaderText="<%$ Resources:lblBusiness_Lastname_g %>"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lblAddress1_g %>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltAddress1" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lblPostcode_g %>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltPostcode" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lblCustomerType_g %>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltCustomerType" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lblFileCode %>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltFileCode" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lblDOB %>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltDOBirth" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PartySourceDescription" HeaderText="<%$ Resources:lblBranch %>" SortExpression="PartySourceDescription"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("UserName") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton Text="<%$ Resources:lblview %>" ID="lnkDetails" runat="server" SkinID="btnGrid"></asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton Text="<%$ Resources:lbl_select %>" ID="lnkSelect" runat="server" SkinID="btnGrid" CommandName="Select" CommandArgument='<%# Container.DisplayIndex%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="DataBound"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
            <Nexus:ProgressIndicator ID="UpdFndClient" OverlayCssClass="updating" AssociatedUpdatePanelID="uPFindClient" runat="server">
                <progresstemplate>
                            </progresstemplate>
            </Nexus:ProgressIndicator>
        </div>
    </div>
</asp:Content>
