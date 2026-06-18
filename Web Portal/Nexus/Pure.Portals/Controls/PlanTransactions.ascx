<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PlanTransactions.ascx.vb"
    Inherits="Controls_PlanTransactions" %>
<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<div id="Controls_PlanTransactions">

    <script language="javascript" type="text/javascript">
        function CloseFindAccount() {
            tb_remove();

        }
        function setClient(sClientName, sClientKey) {
            tb_remove();
            document.getElementById('<%=txtClientCode.ClientId %>').value = sClientName;
            document.getElementById('<%=txtClientCode.ClientId %>').focus();
            document.getElementById('<%=hvAgentKey.ClientId %>').value = sClientKey;
        }

        $(document).ready(function () {
            if (document.getElementById('<%=txtClientCode.ClientId %>') != null) {
                document.getElementById('<%=txtClientCode.ClientId %>').readOnly = true;
                document.getElementById('<%=txtTotalSelected.ClientId %>').readOnly = true;
            }
            //document.getElementById('<%=btnOk.ClientId %>').style.display = 'none';

        });

        function ShowHideButton(flag) {
            document.getElementById('<%=btnOk.ClientId %>').style.visibility = flag;
        }
        function tb_updatedWithAlert(postbackargument, postbacktarget, message) {
            tb_remove();
            self.parent.tb_updatedWithAlert(postbackargument, postbacktarget, message);
        }
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function () {
            setupNumericControls();
            SetScrollPosition();
            if (document.getElementById('<%=txtClientCode.ClientId %>') != null) {
                document.getElementById('<%=txtClientCode.ClientId %>').readOnly = true;
            }
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setupNumericControls();
            SetScrollPosition();
            if (document.getElementById('<%=txtClientCode.ClientId %>') != null) {
                document.getElementById('<%=txtClientCode.ClientId %>').readOnly = true;
            }
        });
        function SetScrollPosition() {
            var strCook = document.getElementById('<%=hvScrollPosition.ClientId %>').value;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                document.getElementById("HScroll").scrollTop = strPos;
            }
        }
        function SetDivPosition() {
            var intY = document.getElementById("HScroll").scrollTop;
            document.getElementById('<%=hvScrollPosition.ClientId %>').value = "yPos=!~" + intY + "~!";
        }
        function ClearTotalSelected() {
            document.getElementById('<%=txtTotalSelected.ClientId %>').value = "0.00";
            var grid = document.getElementById("<%=grdTransactions.ClientId %>").getElementsByTagName("tr");

            var chkUnselect;
            if (grid.length > 0) {
                for (rowCount = 1; rowCount < grid.length; rowCount++) {
                    chkUnselect = grid[rowCount].cells[0].getElementsByTagName("input")[0];
                    chkUnselect.checked = false;
                }
            }
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hvScrollPosition" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hvInsuranceRef" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="updPlanTransactions" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="card">
                <div class="card-heading">
                    <h1>
                        <asp:Label ID="lblPlanTransactions" runat="server" Text="<%$ Resources:lbl_PlanTransactions %>"></asp:Label>
                    </h1>
                </div>
                <asp:Panel ID="pnlClientAccounts" runat="server" Visible="true">
                    <div class="card-body clearfix">
                        <div class="form-horizontal">
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClientCode" Text="<%$ Resources:lblClientCode%>" ID="lblbtnClientCode"></asp:Label><div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtClientCode" runat="server" CssClass="field-mandatory form-control"></asp:TextBox><span class="input-group-btn">
                                            <asp:LinkButton ID="btnClientCode" runat="server" CausesValidation="false" associatedcontrolid="txtClientCode" SkinID="btnModal"><i class="fa fa-search"></i><span class="btn-fnd-txt">Client Code</span></asp:LinkButton>

                                        </span>
                                    </div>
                                </div>
                                <asp:RequiredFieldValidator ID="vldClientCodeRequired" runat="server" Display="none" ControlToValidate="txtClientCode" ErrorMessage="<%$ Resources:ClientCode_ErrorMessage%>" SetFocusOnError="True" ValidationGroup="GroupFindButton"></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hvAgentName" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hvAgentKey" runat="server"></asp:HiddenField>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblShowTransactions" runat="server" AssociatedControlID="dd1ShowTransactions" Text="<%$ Resources:lblShowTransactions %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:DropDownList ID="dd1ShowTransactions" runat="server" onchange="ClearTotalSelected();" CssClass="form-control form-select">
                                        <asp:ListItem>From Policies Already On The Plan</asp:ListItem>
                                        <asp:ListItem>From All Policies</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblDateTo" runat="server" AssociatedControlID="txtDateTo" Text="<%$ Resources:lblToDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calDateTo" runat="server" LinkedControl="txtDateTo" HLevel="4"></uc1:CalendarLookup>
                                    </div>
                                </div>

                                <asp:RequiredFieldValidator ID="RqdLookup_Locality" runat="server" ControlToValidate="txtDateTo" ValidationGroup="GroupFindButton" ErrorMessage="<%$ Resources:err_Message %>" SetFocusOnError="True" Display="none" Enabled="false"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="Rgp_Date" runat="server" Display="None" ControlToValidate="txtDateTo" SetFocusOnError="false" ValidationGroup="GroupFindButton" Type="Date" ErrorMessage="<%$ Resources:err_InvalidDate %>" MaximumValue="12/12/2099" MinimumValue="01/01/1900"></asp:RangeValidator>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblPolicyNo" runat="server" AssociatedControlID="txtPolicyNo" Text="<%$ Resources:lblPolicyNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtPolicyNo" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:HiddenField ID="hvInsuranceFileKey" runat="server"></asp:HiddenField>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblClaimNumber" runat="server" AssociatedControlID="txtClaimNumber" Text="Claim Number" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtClaimNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTotalSelected" runat="server" AssociatedControlID="txtTotalSelected" Text="<%$ Resources:lblTotalSelected %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtTotalSelected" runat="server" CssClass="short Doub form-control" Text="0.00" ReadOnly="true"></asp:TextBox>
                                </div>
                                
                                <asp:RequiredFieldValidator ID="rfvTotalSelected" runat="server" Display="none" ControlToValidate="txtTotalSelected" ValidationGroup="GroupOkButton" ErrorMessage="<%$ Resources:TotalSelected_ErrorMessage%>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RngAmount" runat="server" MinimumValue="-99999999" MaximumValue="99999999" Display="None" ErrorMessage="<%$ Resources:lblRngAmount %>" Type="Currency" ControlToValidate="txtTotalSelected" ValidationGroup="GroupOkButton"></asp:RangeValidator>
                               
                                <asp:HiddenField ID="chkHiddenField" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hvSelectedPlanTypes" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hvSourceType" runat="server"></asp:HiddenField>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer">
                        <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btn_NewSearch %>" SkinID="btnSecondary"></asp:LinkButton>
                        <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:btn_FindNow %>" ValidationGroup="GroupFindButton" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
            <asp:CustomValidator ID="CstmNegativeAmtCheck" runat="server" CssClass="error" ErrorMessage="<%$ Resources:msg_NegativeAmount %>" Display="none" ValidationGroup="GroupOkButton"></asp:CustomValidator>       
            <asp:ValidationSummary ID="ValidationSummary1" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_FPTransaction_ValidationSummary %>" runat="server" ValidationGroup="GroupFindButton" CssClass="validation-summary"></asp:ValidationSummary>
            <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="GroupOkButton" CssClass="validation-summary"></asp:ValidationSummary>

            <div id="HScroll" class="grid-card table-responsive" onscroll="SetDivPosition();">
                <asp:GridView ID="grdTransactions" runat="server" PagerSettings-Mode="Numeric" GridLines="None" AutoGenerateColumns="false" AllowSorting="true" EmptyDataText="<%$ Resources:EmptyDataMessage %>" EmptyDataRowStyle-CssClass="noData" DataKeyNames="DocRef,InsuranceFileKey,Amount">
                    <Columns>
                        <asp:TemplateField SortExpression="chkSelect">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SourceID" HeaderText="<%$ Resources:grdbf_BranchCode %>"></asp:BoundField>
                        <asp:BoundField DataField="InsuranceRefIndex" HeaderText="<%$ Resources:lblPolicyReference %>"></asp:BoundField>
                        <asp:BoundField DataField="DocRef" HeaderText="<%$ Resources:grdbf_DocRef %>"></asp:BoundField>
                        <asp:BoundField DataField="EffectiveDate" HeaderText="<%$ Resources:grdbf_EffectiveDate %>" DataFormatString="{0:d}"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" HeaderText="<%$ Resources:grdbf_TransDate %>" DataFormatString="{0:d}"></asp:BoundField>
                        <asp:BoundField DataField="Period" HeaderText="<%$ Resources:grdbf_Period %>"></asp:BoundField>
                        <Nexus:BoundField DataField="Amount" HeaderText="<%$ Resources:grdbf_Amount %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="OutStandingamount" HeaderText="<%$ Resources:grdbf_OSAmount %>" DataType="Currency"></Nexus:BoundField>
                        <asp:BoundField DataField="DocType" HeaderText="<%$ Resources:grdbf_DocType %>"></asp:BoundField>
                        <asp:BoundField DataField="spare" />

                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="card-footer">
       
        <asp:LinkButton ID="btnOk" runat="server" ValidationGroup="GroupOkButton" CausesValidation="true" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary"></asp:LinkButton>
    </div>
    <Nexus:ProgressIndicator ID="upPlanTransactions" AssociatedUpdatePanelID="updPlanTransactions" runat="server">
        <progresstemplate>
        </progresstemplate>
    </Nexus:ProgressIndicator>
</div>
