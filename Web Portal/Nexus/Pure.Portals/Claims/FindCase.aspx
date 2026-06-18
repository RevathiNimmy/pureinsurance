<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="FindCase.aspx.vb" Inherits="Claims_FindCase" %>

<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        //Will be called on page load
        //This will enable/disable calender controls on the basis of associated checkbox value
        onload = function () {

            if (!document.getElementById('<%= chkCaseOpenDate.ClientId%>').checked) {
                $('#<%= txtCaseOpenDate.ClientId %>').datepicker('disable').attr('disabled', 'disabled');
            }
        }

        function ShowCloseErrMsg(sMessage) {
            alert(sMessage)
        }

        function CloseCase(sDesc) {
            tb_remove();
            document.getElementById('<%=hDesc.ClientID %>').value = sDesc;
            __doPostBack('', 'CloseCase')
        }

        function CloseCaseConfirmation(sMsg, sCaseKey, sBaseCaseKey, sUrl) {
            var result = confirm(sMsg);
            if (result == true) {
                document.getElementById('<%=hCaseKey.ClientID %>').value = sCaseKey;
                document.getElementById('<%=hBaseCaseKey.ClientID %>').value = sBaseCaseKey;
                tb_show(null, sUrl, null);
                return false;
            }
            else {
                return false;
            }
        }

        //Enable or Disable the calender control and linked textbox 
        //on change of associated checkbox
        function EnableCalender(status) {
            if (status == true) {
                $('#<%= txtCaseOpenDate.ClientId %>').datepicker('enable').removeAttr('disabled');
            }
            else {
                $('#<%= txtCaseOpenDate.ClientId %>').datepicker('disable').attr('disabled', 'disabled');
            }
        }
    </script>

    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Portal_Templates_FindCase">
        <asp:Panel ID="pnlNewCase" runat="server" DefaultButton="btnSearch" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblFindCase" runat="server" Text="<%$ Resources:lbl_FindCase %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCaseNumber" runat="server" AssociatedControlID="txtCaseNumber" Text="<%$ Resources:lbl_CaseNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCaseNumber" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblProgressStatus" runat="server" AssociatedControlID="drpProgressStatus" Text="<%$ Resources:lbl_ProgressStatus %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="drpProgressStatus" runat="server" DataItemValue="Code" DataItemText="Description" ListType="PMLookup" ListCode="case_progress" DefaultText="Please Select" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCaseOpenDate" runat="server" AssociatedControlID="txtCaseOpenDate" Text="<%$ Resources:lbl_CaseOpenDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <asp:CheckBox ID="chkCaseOpenDate" runat="server" OnClick="EnableCalender(this.checked)" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </span>
                                <asp:TextBox ID="txtCaseOpenDate" CssClass="field-date form-control" runat="server"></asp:TextBox>
                                <uc1:CalendarLookup ID="CaseOpenDate_CalendarLookup" runat="server" LinkedControl="txtCaseOpenDate" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>
                        <asp:CustomValidator ID="VldDate" runat="server" Display="None" ErrorMessage="<%$ Resources:Err_InvalidOpenDate %>"></asp:CustomValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimNumber" runat="server" AssociatedControlID="txtClaimNumber" Text="<%$ Resources:lbl_ClaimNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimNumber" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskType" runat="server" AssociatedControlID="drpRiskType" Text="<%$ Resources:lbl_RiskType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="drpRiskType" runat="server" CssClass="form-control form-select"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskIndex" runat="server" AssociatedControlID="txtRiskIndex" Text="<%$ Resources:lbl_RiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtRiskIndex" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btn_NewSearch %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNewCase" runat="server" Text="<%$ Resources:btn_NewCase %>" CausesValidation="false" PostBackUrl="~/Claims/ClaimCase.aspx?NewCase=true" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnSearch" runat="server" Text="<%$ Resources:btn_Search %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </asp:Panel>
        <asp:CustomValidator ID="custValidate" runat="server" CssClass="error" SetFocusOnError="true" Display="None"></asp:CustomValidator>
        <nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_error %>" ControlsToValidate="txtCaseNumber,txtClaimNumber,txtRiskIndex" Condition="Auto" Display="none" runat="server" EnableClientScript="true"></nexus:WildCardValidator>
        <asp:ValidationSummary ID="vldFindCaseSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>


        <asp:UpdatePanel ID="PnlSearchResults" UpdateMode="conditional" ChildrenAsTriggers="true" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblInformation" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvSearchResults" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" DataKeyNames="BaseCaseKey,CaseKey" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData" AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="CaseNumber" HeaderText="<%$ Resources:grdhdr_CaseNumber %>" SortExpression="CaseNumber"></asp:BoundField>
                            <asp:BoundField DataField="CaseOpenDate" HeaderText="<%$ Resources:grdhdr_OpenedDate %>" DataFormatString="{0:d}" SortExpression="CaseOpenDate"></asp:BoundField>
                            <asp:BoundField DataField="Analyst" HeaderText="<%$ Resources:grdhdr_Analyst %>" SortExpression="Analyst"></asp:BoundField>
                            <asp:BoundField DataField="Assistant" HeaderText="<%$ Resources:grdhdr_Assistant %>" SortExpression="Assistant"></asp:BoundField>
                            <asp:BoundField DataField="CaseProgressDescription" HeaderText="<%$ Resources:grdhdr_ProgressStatus %>" SortExpression="CaseProgressDescription"></asp:BoundField>
                            <nexus:BoundField DataField="TotalIndemnity" HeaderText="<%$ Resources:grdhdr_TotalIndemnity %>" SortExpression="TotalIndemnity" DataType="Currency"></nexus:BoundField>
                            <nexus:BoundField DataField="TotalExpense" HeaderText="<%$ Resources:grdhdr_TotalExpense %>" SortExpression="TotalExpense" DataType="Currency"></nexus:BoundField>
                            <nexus:BoundField DataField="TotalExcess" HeaderText="<%$ Resources:grdhdr_TotalExcess %>" SortExpression="TotalExcess" DataType="Currency"></nexus:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol class="list-inline no-margin">
                                            <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                <ol id="menu_<%# Eval("CaseKey") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                    <li id="lnkEdit" visible="false" runat="server">
                                                        <asp:LinkButton ID="btnEdit" CommandName="edit" runat="server" Text="<%$ Resources:FindCase_btnEdit %>" CausesValidation="false"></asp:LinkButton>
                                                    </li>
                                                    <li id="liClose" runat="server">
                                                        <asp:LinkButton ID="btnClose" runat="server" Text="<%$ Resources:FindCase_btnClose %>" CommandName="Close" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CaseKey")%>' CausesValidation="false"></asp:LinkButton>
                                                    </li>
                                                    <li id="liSelect" runat="server" visible="false">
                                                        <asp:LinkButton ID="btnSelect" runat="server" Text="select" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CaseNumber")%>' CausesValidation="false"></asp:LinkButton>
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
        </asp:UpdatePanel>
        <nexus:ProgressIndicator ID="UpPnlSearchResults" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlSearchResults" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </nexus:ProgressIndicator>
        <asp:HiddenField ID="hDesc" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hCaseKey" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hBaseCaseKey" runat="server"></asp:HiddenField>

    </div>
</asp:Content>
