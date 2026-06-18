<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CoverNoteBook.aspx.vb" Inherits="Nexus.secure_CoverNoteBook"
    MasterPageFile="~/default.master" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/ProductPickList.ascx" TagName="ProductPickList" TagPrefix="Product" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        function setAgent(sName, sKey) {
            tb_remove();
            document.getElementById('<%=txtAgentCode.ClientID %>').value = unescape(sName.replace(/\+/g, " "));;
            document.getElementById('<%=hiddenAgentCode.ClientID %>').value = sKey;
        }

        function pageLoad() {
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
        }

        function OnBeginRequest(sender, args) {
            var postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'lnkEdit') {
                $get(uprogQuotes).style.display = "block";
            }
        }

    </script>

    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="secure_CoverNoteBook">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label runat="server" ID="lblCoverNoteBook" Text="<%$ Resources:lbl_CoverNoteBook %>"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBookNumber" runat="server" AssociatedControlID="txtBookNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litBookNumber" runat="server" Text="<%$ Resources:lbl_BookNumber %>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtBookNumber" CssClass="field-medium field-mandatory form-control" runat="server"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdBookNumber" runat="server" ControlToValidate="txtBookNumber" Enabled="true" Display="None" ErrorMessage="<%$ Resources:lbl_ErrMsg_BookNumber %>" ValidationGroup="CoverNoteBookGroup"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStartNumber" runat="server" AssociatedControlID="txtStartNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litStartNumber" runat="server" Text="<%$ Resources:lbl_StartNumber %>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtStartNumber" CssClass="field-medium field-mandatory form-control" runat="server"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdStartNumber" runat="server" ControlToValidate="txtStartNumber" Enabled="true" Display="None" ErrorMessage="<%$ Resources:lbl_ErrMsg_StartNumber %>" ValidationGroup="CoverNoteBookGroup"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblEndNumber" runat="server" AssociatedControlID="txtEndNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litEndNumber" runat="server" Text="<%$ Resources:lbl_EndNumber%>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtEndNumber" CssClass="field-medium field-mandatory form-control" runat="server"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdEndNumber" runat="server" ControlToValidate="txtEndNumber" Enabled="true" Display="None" ErrorMessage="<%$ Resources:lbl_ErrMsg_EndNumber %>" ValidationGroup="CoverNoteBookGroup"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_EffectiveDate" runat="server" AssociatedControlID="txtEffectiveDate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litEffectiveDate" runat="server" Text="<%$ Resources:lbl_EffectiveDate%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtEffectiveDate" ReadOnly="false" runat="server" CssClass="field-date form-control"></asp:TextBox><uc1:CalendarLookup ID="calEffectiveDate" runat="server" LinkedControl="txtEffectiveDate" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAgentCode" Text="<%$ Resources:lbl_AgentCode%>" ID="lblbtnAgentCode"></asp:Label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtAgentCode" runat="server" CssClass="field-medium form-control"></asp:TextBox><span class="input-group-btn"><asp:LinkButton ID="btnAgentCode" runat="server" OnClientClick="tb_show(null , '../Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;" SkinID="btnModal">
                                    <i class="fa fa-search"></i>
                                    <span class="btn-fnd-txt">Agent Code</span>
                                </asp:LinkButton></span>
                            </div>
                        </div>


                        <asp:HiddenField ID="hiddenAgentCode" runat="server"></asp:HiddenField>
                        <asp:CustomValidator ID="AgentCustomvalidator" Display="None" runat="server" ValidationGroup="CoverNoteBookGroup" ErrorMessage="<%$ Resources:lbl_ErrMsg_AgentCode %>" Enabled="false"></asp:CustomValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBranch" runat="server" AssociatedControlID="ddlBranch" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litBranch" runat="server" Text="<%$ Resources:lbl_Branch%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control form-select">
                            </asp:DropDownList>
                        </div>
                        <asp:CustomValidator ID="RqdBranch" Display="None" runat="server" ValidationGroup="CoverNoteBookGroup" ErrorMessage="<%$ Resources:lbl_ErrMsg_Branch %>" Enabled="false"></asp:CustomValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCoverNoteStatus" runat="server" AssociatedControlID="ddlCoverNoteStatus" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litCoverNoteStatus" runat="server" Text="<%$ Resources:lbl_CoverNoteStatus%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlCoverNoteStatus" runat="server" DataItemValue="Code" DataItemText="Description" ListType="PMLookup" ListCode="cover_note_book_status" CssClass="field-medium form-control field-mandatory" DefaultText="   " Sort="ASC"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdCoverNoteStatus" Display="None" runat="server" ControlToValidate="ddlCoverNoteStatus" ValidationGroup="CoverNoteBookGroup" ErrorMessage="<%$ Resources:lbl_ErrMsg_CoverNoteStatus %>" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCreatedDate" runat="server" AssociatedControlID="txtCreatedDate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litCreatedDate" runat="server" Text="<%$ Resources:lbl_CreatedDate%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCreatedDate" CssClass="field-medium form-control" runat="server" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <asp:CustomValidator ID="cusValidNumber" runat="server" Display="None" ValidationGroup="CoverNoteBookGroup"></asp:CustomValidator>
                </div>
                <Product:ProductPickList ID="pckProduct" runat="server" UseSearch="false"></Product:ProductPickList>
                <asp:Panel CssClass="error" ID="PnlError" runat="server" Visible="false">
                    <asp:Label runat="server" ID="lblError" ForeColor="black"></asp:Label>
                </asp:Panel>
                <asp:UpdatePanel ID="UpdCNB" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="grdvCoverNoteBook" runat="server" GridLines="None" AutoGenerateColumns="False" PagerSettings-Mode="Numeric" AllowPaging="True" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="CoverNoteSheetNumber" HeaderText="<%$ Resources:lbl_CoverSheetNumber_g %>"></asp:BoundField>
                                    <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:lbl_CustomerName_g %>"></asp:BoundField>
                                    <asp:BoundField DataField="CoverNoteSheetStatusDescription" HeaderText="<%$ Resources:lbl_Status_g %>"></asp:BoundField>
                                    <asp:BoundField DataField="PolicyNumber" HeaderText="<%$ Resources:lbl_PolicyNumber_g %>"></asp:BoundField>
                                    <asp:BoundField DataField="BranchName" HeaderText="<%$ Resources:lbl_Branch_g %>"></asp:BoundField>
                                    <asp:BoundField DataField="AgentName" HeaderText="<%$ Resources:lbl_Agent_g %>"></asp:BoundField>
                                    <asp:BoundField DataField="DateImported" HeaderText="<%$ Resources:lbl_DateImported_g %>" DataFormatString="{0:d}" HtmlEncode="false"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol class="list-inline no-margin">
                                                    <li class="dropdown no-padding">
                                                        <a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle">
                                                            <i class="fa fa-ellipsis-v" aria-hidden="true"></i>
                                                        </a>
                                                        <ol id='menu_<%# Eval("CoverNoteSheetKey") %>' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                            <li id="liEdit" runat="server">
                                                                <asp:LinkButton Text="<%$ Resources:lbl_Edit %>" ID="lnkEdit" runat="server"></asp:LinkButton>
                                                            </li>
                                                            <li id="liDelete" runat="server">
                                                                <asp:HiddenField ID="hiddenKey" runat="server"></asp:HiddenField>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" Text="<%$ Resources:lbl_Delete %>" CommandName="Delete" CausesValidation="false"></asp:LinkButton>
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
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="upCoverNoteBook" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdCNB" runat="server">
                    <progresstemplate>
                </progresstemplate>
                </Nexus:ProgressIndicator>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnAdd" runat="server" Text="<%$ Resources:lbl_Add %>" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:lbl_Ok %>" ValidationGroup="CoverNoteBookGroup" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="ProductCustomValidator" Display="None" runat="server" ValidationGroup="CoverNoteBookGroup" ErrorMessage="Please select atleast one product" Enabled="false"></asp:CustomValidator>
        <asp:CustomValidator ID="VldEffectiveDate" runat="server" Display="None" ValidationGroup="CoverNoteBookGroup"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" HeaderText="<%$ Resources:lbl_ValidationSummary %>" DisplayMode="BulletList" ValidationGroup="CoverNoteBookGroup" runat="server" CssClass="validation-summary"></asp:ValidationSummary>

    </div>
</asp:Content>
