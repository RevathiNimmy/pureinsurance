<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="BankGuarantee.aspx.vb" Inherits="Nexus.BankGuarantee" EnableViewState="true" %>

<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function setAgent(sName, sKey, sAgentCode) {
            tb_remove();
            document.getElementById('<%= txtAgent.ClientId%>').value = unescape(sAgentCode);
            document.getElementById('<%= txtAgentKey.ClientId%>').value = sKey;
            document.getElementById('<%= txtClient.ClientId%>').value = '';
            document.getElementById('<%= txtClientKey.ClientId%>').value = '';
            document.getElementById('<%= txtAgent.ClientId%>').focus();
            toggleControl();
        }
        function CloseBG() {
            tb_remove();
        }

        function setClient(sClientName, sClientKey) {
            tb_remove();
            document.getElementById('<%= txtClient.ClientId%>').value = unescape(sClientName);
            document.getElementById('<%= txtClientKey.ClientId%>').value = sClientKey;
            document.getElementById('<%= txtAgent.ClientId%>').value = '';
            document.getElementById('<%= txtAgentKey.ClientId%>').value = '';
            document.getElementById('<%= txtClient.ClientId%>').focus();
            toggleControl();
        }
        function setBank(sBankShortName, sBankKey, sBankName) {
            tb_remove();
            document.getElementById('<%= txtBankName.ClientId%>').value = unescape(sBankShortName);
            document.getElementById('<%= txtBankName.ClientId%>').focus();
        }

        function toggleControl() {
            var sAgent = document.getElementById('<%= txtAgentKey.ClientId%>').value
            var sClient = document.getElementById('<%= txtClient.ClientId%>').value
            var oBtnAdd = document.getElementById('<%= btnAdd.ClientId%>')

            if (sAgent == '' & sClient == '') {
                document.getElementById('<%= btnAdd.Clientid%>').disabled = true;

            }
            else {
                document.getElementById('<%= btnAdd.Clientid%>').disabled = false;

            }
        }

        function setAgentorClient() {
            if (document.getElementById('<%= txtAgent.ClientId%>').value != '') {
                document.getElementById('<%= hChoice.ClientId%>').value = AgentConfirmation();

            }
            else if (document.getElementById('<%= txtClient.ClientId%>').value != '') {
                document.getElementById('<%= hChoice.ClientId%>').value = ClientConfirmation();
            }
    }
    function setQuote(sPolicyRef, iFileKey, sClientCode) {

        tb_remove();
        document.getElementById('<%= txtInsuranceFile.ClientId%>').value = sPolicyRef;
        document.getElementById('<%= txtInsuranceFile.ClientId%>').focus();
        document.getElementById('<%= txtPolicyRefKey.ClientId%>').value = iFileKey;
        document.getElementById('<%= txtClient.ClientId%>').value = unescape(sClientCode);
        document.getElementById('<%= txtAgent.ClientId%>').value = '';
        document.getElementById('<%= txtAgentKey.ClientId%>').value = '';
    }
    </script>

    <asp:ScriptManager ID="scriptBankGuarantee" runat="server">
    </asp:ScriptManager>
    <div id="secure_BankGuarantee">
        <asp:Panel ID="pnlFindBG" runat="server" CssClass="card" DefaultButton="btnSubmit">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="litBankGuaranteeHeader" runat="server" Text="<%$ Resources:lbl_BankGuarantee_header%>" EnableViewState="false"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClient" Text="<%$ Resources:btn_Client %>" ID="lblbtnClient"></asp:Label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtClient" runat="server" CssClass="form-control" TabIndex="2" MaxLength="20"></asp:TextBox><span class="input-group-btn">
                                    <asp:LinkButton ID="btnClient" runat="server" TabIndex="1" CausesValidation="false" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Client</span>
                                    </asp:LinkButton></span>
                            </div>
                        </div>
                        <asp:HiddenField ID="txtClientKey" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAgent" Text="<%$ Resources:btn_Agent %>" ID="lblbtnAgent"></asp:Label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtAgent" runat="server" CssClass="form-control" TabIndex="4" MaxLength="20"></asp:TextBox><span class="input-group-btn">
                                    <asp:LinkButton ID="btnAgent" runat="server" TabIndex="3" CausesValidation="false" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Agent</span>
                                    </asp:LinkButton></span>
                            </div>
                        </div>
                        <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtInsuranceFile" Text="<%$ Resources:btn_InsuranceFile %>" ID="lblbtnInsuranceFile"></asp:Label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtInsuranceFile" runat="server" CssClass="form-control" TabIndex="9" MaxLength="30"></asp:TextBox><span class="input-group-btn">
                                    <asp:LinkButton ID="btnInsuranceFile" runat="server" TabIndex="8" CausesValidation="false" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Insurance File</span>
                                    </asp:LinkButton></span>
                            </div>
                        </div>


                        <asp:HiddenField ID="txtPolicyRefKey" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBankName" Text="<%$ Resources:btn_BankName %>" ID="lblbtnBankName"></asp:Label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" TabIndex="7" MaxLength="50"></asp:TextBox><span class="input-group-btn">
                                    <asp:LinkButton ID="btnBankName" runat="server" TabIndex="6" CausesValidation="false" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Bank Name</span>
                                    </asp:LinkButton>

                                </span>
                            </div>
                        </div>


                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBankGuaranteeRef" runat="server" AssociatedControlID="txtBankGuaranteeRef" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="btnBankGuaranteeRef" runat="server" Text="<%$ Resources:lbl_BankGuaranteeRef %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtBankGuaranteeRef" runat="server" TabIndex="5" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBGStatus" runat="server" AssociatedControlID="ddlBGStatus" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="btnBGStatus" runat="server" Text="<%$ Resources:lbl_BGStatus %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlBGStatus" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="pmlookup" ListCode="bg_status" TabIndex="10" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnClear" runat="server" TabIndex="11" Text="<%$ Resources:btn_Clear %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSubmit" runat="server" TabIndex="12" Text="<%$ Resources:btn_Submit %>" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnAdd" TabIndex="13" runat="server" Text="<%$ Resources:btn_Add %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </asp:Panel>
        <asp:HiddenField ID="hChoice" runat="server"></asp:HiddenField>
        <nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtClient,txtAgent,txtBankGuaranteeRef,txtBankName,txtInsuranceFile" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </nexus:WildCardValidator>
        <asp:ValidationSummary ID="ValidationSummary" runat="server" DisplayMode="BulletList" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:UpdatePanel ID="UpdBankGuarantee" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvBankGuarantee" runat="server" DataKeyNames="BGKey" PageSize="10" PagerSettings-Mode="Numeric" GridLines="None" AutoGenerateColumns="False" AllowPaging="true" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                        <Columns>
                            <asp:BoundField DataField="BankName" HeaderText="<%$ Resources:lbl_BankName %>"></asp:BoundField>
                            <asp:BoundField DataField="BankGuaranteeRef" HeaderText="<%$ Resources:lbl_BGNumber %>"></asp:BoundField>
                            <asp:BoundField DataField="BGKey" HeaderText="BG KEY"></asp:BoundField>
                            <nexus:BoundField DataField="BGLimit" HeaderText="<%$ Resources:lbl_BGLimit %>" DataType="Currency"></nexus:BoundField>
                            <nexus:BoundField DataField="AvailableBalance" HeaderText="<%$ Resources:lbl_AvailableBalance %>" DataType="Currency"></nexus:BoundField>
                            <asp:BoundField DataField="ExpiryDate" HeaderText="<%$ Resources:lbl_ExpiryDate %>" HtmlEncode="False" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:BoundField DataField="ClientShortName" HeaderText="<%$ Resources:lbl_ClientName %>"></asp:BoundField>
                            <asp:BoundField DataField="ClientResolvedName" HeaderText="<%$ Resources:lbl_ClientCode %>"></asp:BoundField>
                            <asp:BoundField DataField="Product" HeaderText="<%$ Resources:lbl_Products %>"></asp:BoundField>
                            <asp:BoundField DataField="Branch" HeaderText="<%$ Resources:lbl_Branches %>"></asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol class="list-inline no-margin">
                                            <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                <ol id="menu_<%# Eval("BGKey") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                    <li id="liDelete" runat="server">
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="<%$ Resources:lbl_Delete %>">
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li id="liEdit" runat="server">
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="<%$ Resources:lbl_Edit %>"></asp:LinkButton>
                                                    </li>
                                                    <li id="liView" runat="server">
                                                        <asp:LinkButton ID="lnkView" runat="server" Text="<%$ Resources:lbl_View %>"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvBankGuarantee" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvBankGuarantee" EventName="RowCreated"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvBankGuarantee" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvBankGuarantee" EventName="RowDeleting"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <nexus:ProgressIndicator ID="UpBnkGuarantee" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdBankGuarantee" runat="server">
            <progresstemplate>
            </progresstemplate>
        </nexus:ProgressIndicator>
    </div>
</asp:Content>
