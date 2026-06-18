<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="QuoteCollection.aspx.vb" MaintainScrollPositionOnPostback="true" EnableViewState="true"
    Inherits="secure_QuoteCollection" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ProductPickList.ascx" TagName="ProductPickList" TagPrefix="Product" %>
<asp:Content ID="cntScriptIncludes" ContentPlaceHolderID="ScriptIncludes" runat="server">

    <script type="text/javascript">

        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null)
                cb.checked = checkState;
        }

        function ValidateNumberSearch(oSrc, args) {

            var sGridView = document.getElementById('<%=grdQuotes.ClientId%>');

            if (sGridView != null) {
                args.IsValid = true;
                sGridView.style.display = "none";
            }
            else {
                args.IsValid = false;
            }
            alert(args.IsValid);

        }

        function ChangeAllCheckBoxStates(checkState) {
            // Toggles through all of the checkboxes defined in the CheckBoxIDs array
            // and updates their value to the checkState input parameter
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++) {
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
                }
            }
        }


        function setAgent(sName, sKey, sCode) {
            tb_remove();
            document.getElementById('<%= txtAgentCode.ClientId%>').value = unescape(sCode);
            document.getElementById('<%= txtAgentKey.ClientId%>').value = sKey;
            document.getElementById('<%= txtClient.ClientId%>').value = '';
            document.getElementById('<%= txtClientKey.ClientId%>').value = '';
            document.getElementById('<%= txtAgentCode.ClientId%>').focus
        }

        function setClient(sClientName, sClientKey) {
            tb_remove();
            document.getElementById('<%= txtClient.ClientId%>').value = unescape(sClientName);
            document.getElementById('<%= txtClientKey.ClientId%>').value = sClientKey;
            document.getElementById('<%= txtAgentCode.ClientId%>').value = '';
            document.getElementById('<%= txtAgentKey.ClientId%>').value = '';
            document.getElementById('<%= txtClient.ClientId%>').focus();
        }

        function setQuote(sInsuranceFileRef) {
            tb_remove();
            document.getElementById('<%= txtQuoteRef.ClientId%>').value = sInsuranceFileRef;
        }

    </script>

</asp:Content>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="secure_QuoteCollection">
        <asp:Panel ID="pnlsearch" runat="server" DefaultButton="btnFindNow">
            <div class="card">
                <div class="card-heading">
                    <h1>
                        <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal></h1>
                </div>
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <legend>
                            <asp:Label ID="lblSearchFormLegend" runat="server" Text="<%$ Resources:lblSearchFormLegend%>"></asp:Label></legend>


                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtQuoteRef" Text="<%$ Resources:btnQuote%>" ID="lblbtnQuote"></asp:Label><div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtQuoteRef" runat="server" CssClass=" form-control"></asp:TextBox><span class="input-group-btn"><asp:LinkButton ID="btnQuote" runat="server" CausesValidation="false" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                         <span class="btn-fnd-txt">Quote</span>  
                                    </asp:LinkButton></span>
                                </div>
                            </div>


                            <asp:CustomValidator ID="custQuoteRef" runat="server" Display="None" ControlToValidate="txtQuoteRef" ErrorMessage="<%$ Resources:InvalidQuoteRef%>" ValidationGroup="MakeSelection"></asp:CustomValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClient" Text="<%$ Resources:btnClient%>" ID="lblbtnClient"></asp:Label><div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtClient" runat="server" CssClass=" form-control"></asp:TextBox><span class="input-group-btn">
                                        <asp:LinkButton ID="btnClient" runat="server" CausesValidation="false" SkinID="btnModal">
                                            <i class="fa fa-search"></i>
                                             <span class="btn-fnd-txt">Client</span>  
                                        </asp:LinkButton></span>
                                </div>
                            </div>


                            <asp:HiddenField ID="txtClientKey" runat="server"></asp:HiddenField>
                            <asp:CustomValidator ID="custInValidClient" ControlToValidate="txtClient" runat="server" Display="none" ValidationGroup="MakeSelection" ErrorMessage="<%$ Resources:InvalidClient%>"></asp:CustomValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAgentCode" Text="<%$ Resources:btnAgent%>" ID="lblbtnAgent"></asp:Label><div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtAgentCode" runat="server" CssClass=" form-control"></asp:TextBox><span class="input-group-btn">
                                        <asp:LinkButton ID="btnAgent" runat="server" CausesValidation="false" SkinID="btnModal">
                                            <i class="fa fa-search"></i>
                                             <span class="btn-fnd-txt">Agent</span>  
                                        </asp:LinkButton></span>
                                </div>
                            </div>


                            <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>
                            <asp:CustomValidator ID="custInValidAgent" ControlToValidate="txtAgentCode" runat="server" Display="None" ValidationGroup="MakeSelection" ErrorMessage="<%$ Resources:InvalidAgent%>"></asp:CustomValidator>
                            <%--<asp:CustomValidator ID="custInValidAgent" runat="server" Display="none" ValidationGroup="MakeSelection"
                                            ErrorMessage="<%$ Resources:InvalidAgent%>" />--%>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label runat="server" ID="lblRiskIndex" AssociatedControlID="txtRiskIndex" Text="<%$ Resources:lblRiskIndex%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox runat="server" ID="txtRiskIndex" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div id="liSingleProduct" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label runat="server" ID="lblProduct" AssociatedControlID="drpProduct" Text="<%$ Resources:lblProduct%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList runat="server" ID="drpProduct" CssClass="form-control form-select"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label runat="server" ID="lblStartDate" AssociatedControlID="txtStartDate" Text="<%$ Resources:lblStartDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtStartDate" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calStartDate" runat="server" LinkedControl="txtStartDate" HLevel="2"></uc1:CalendarLookup>
                                </div>
                            </div>

                            <asp:RangeValidator ID="rngVldStartDate" runat="server" MinimumValue="01/01/1900" Type="Date" MaximumValue="1/12/2998" ControlToValidate="txtStartDate" Display="none" ValidationGroup="MakeSelection" ErrorMessage="<%$ Resources:Err_StartDate%>"></asp:RangeValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label runat="server" ID="lblEndDate" AssociatedControlID="txtEndDate" Text="<%$ Resources:lblEndDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtEndDate" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="CalendarLookup1" runat="server" LinkedControl="txtEndDate" HLevel="2"></uc1:CalendarLookup>
                                </div>
                            </div>

                            <asp:RangeValidator ID="rngVldEndDate" runat="server" MinimumValue="01/01/1900" Type="Date" MaximumValue="01/12/2998" ControlToValidate="txtEndDate" Display="none" ValidationGroup="MakeSelection" ErrorMessage="<%$ Resources:Err_EndDate%>"></asp:RangeValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label runat="server" ID="lblDirect" AssociatedControlID="chkDirect" Text="<%$ Resources:lblDirect%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:CheckBox runat="server" ID="chkDirect" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label runat="server" ID="lblProducts" AssociatedControlID="chkProduct" Text="<%$ Resources:lblSelectProduct%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:CheckBox AutoPostBack="true" runat="server" ID="chkProduct" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </div>
                        </div>
                    </div>
                    <div id="PickList" visible="false" runat="server">
                        <Product:ProductPickList ID="pckProduct" UseSearch="true" runat="server"></Product:ProductPickList>
                    </div>
                </div>
                <div class="card-footer">
                    <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btnNewSearch%>" OnClientClick="javascript:Page_ValidationActive = false;" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnFindNow" runat="server" ValidationGroup="MakeSelection" Text="<%$ Resources:btnFindNow%>" SkinID="btnPrimary"></asp:LinkButton>
                    
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlMakePayment" CssClass="submitarea" Visible="false">
                <asp:Button ID="btnMakePayment" runat="server" ValidationGroup="MakePayment" CssClass="submit" Text="<%$ Resources:btnMakePayment%>"></asp:Button>
            </asp:Panel>
            <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_Error%>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error%>" ControlsToValidate="txtQuoteRef,txtClient,txtAgentCode,txtRiskIndex" Condition="Auto" Display="none" runat="server" EnableClientScript="true" ValidationGroup="MakeSelection">
            </Nexus:WildCardValidator>
            <asp:CustomValidator ID="cusvldSomethingChecked" OnServerValidate="CheckSomethingChecked" runat="server" Text="Error message" Display="none" ValidationGroup="MakePayment" ErrorMessage="<%$ Resources:vldSomethingChecked%>"></asp:CustomValidator>
            <asp:CustomValidator ID="cusvldSameAgent" OnServerValidate="CheckSameAgent" runat="server" Text="Error message" Display="none" ValidationGroup="MakePayment" ErrorMessage="<%$ Resources:vldSameAgent%>"></asp:CustomValidator>
            <asp:CustomValidator ID="cusvldSameBranch" OnServerValidate="CheckSameBranch" runat="server" Text="Error message" Display="none" ValidationGroup="MakePayment" ErrorMessage="<%$ Resources:vldSameBranch%>"></asp:CustomValidator>
            <asp:CustomValidator ID="cusvldSameClient" OnServerValidate="CheckSameClient" runat="server" Text="Error message" Display="none" ValidationGroup="MakePayment" ErrorMessage="<%$ Resources:vldSameClient%>"></asp:CustomValidator>
            <asp:ValidationSummary ID="vldSummary" ValidationGroup="MakePayment" runat="server" HeaderText="<%$ Resources:lblValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
            <asp:ValidationSummary ID="vldSummary1" Visible="true" ValidationGroup="MakeSelection" runat="server" HeaderText="<%$ Resources:lblValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
            <div class="grid-card table-responsive">
                <asp:GridView runat="server" ID="grdQuotes" DataKeyNames="InsuranceFileKey" GridLines="None" AllowPaging="True" CellPadding="0" PagerSettings-Mode="Numeric" PageSize="10" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input type="checkbox" onclick="ChangeAllCheckBoxStates(this.checked);">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" CausesValidation="False" AutoPostBack="true" OnCheckedChanged="GrdChkSelected" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$ Resources:lblQuoteNumber %>" DataField="InsuranceFileRef"></asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:lblClientName %>" DataField="PartyName"></asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:lblAgent %>" DataField="Agent"></asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:lblProduct %>" DataField="ProductCode"></asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:lblBranch %>" DataField="BranchCode"></asp:BoundField>
                        <Nexus:BoundField HeaderText="<%$ Resources:lblAmountDue %>" DataField="GrossTotal" DataType="Currency"></Nexus:BoundField>
                    </Columns>
                </asp:GridView>
            </div>

        </asp:Panel>

        <div>
        </div>
    </div>
</asp:Content>
