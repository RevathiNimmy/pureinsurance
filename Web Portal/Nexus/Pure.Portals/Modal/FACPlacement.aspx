<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="FACPlacement.aspx.vb" Inherits="Modal_FACPlacement" %>

<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        function updateThisPercControls(elementId) {
            var parentElementRef = document.getElementById(elementId);
            var elementRefArray = parentElementRef.getElementsByTagName('INPUT');
            var textBoxTotals = 0;

            for (var i = 0; i < elementRefArray.length; i++) {
                var elementRef = elementRefArray[i];

                if ((elementRef.type == 'text') && (elementRef.gridTextBoxType == 'calcTextBox')) {
                    var textBoxAmount = parseFloat(elementRef.value);
                    if (isNaN(textBoxAmount) == true)
                        textBoxAmount = 0;

                    textBoxTotals += textBoxAmount;
                }
            }

            if (isNaN(textBoxTotals) == true)
                textBoxTotals = 0;

            for (var i = 0; i < elementRefArray.length; i++) {
                var elementRef = elementRefArray[i];

                if ((elementRef.type == 'text') && (elementRef.gridTextBoxType == 'totalsTextBox')) {
                    elementRef.value = textBoxTotals;
                }
            }
            if (textBoxTotals > 100) {
                alert("Total Participant percentage should be equal to 100%.")
            }

        }

        function ShowMsg(sMessage) {
            alert(sMessage);
        }
    </script>
    <div id="Modal_FACPlacement">
        <asp:Panel ID="pnlFACPlacement" runat="server" DefaultButton="btnSearch" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblFindReinsurer" runat="server" Text="<%$ Resources:lblFindReinsurer %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblReinsurerCode" runat="server" AssociatedControlID="txtReinsurerCode" Text="<%$ Resources:lblReinsurerCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtReinsurerCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="<%$ Resources:lblName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFileCode" runat="server" AssociatedControlID="txtFileCode" Text="<%$ Resources:lblFileCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFileCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblType" runat="server" AssociatedControlID="drpType" Text="<%$ Resources:lblType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="drpType" runat="server" CssClass="form-control form-select">
                            </asp:DropDownList>
                        </div>
                    </div>

                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btnNewSearch %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSearch" runat="server" Text="<%$ Resources:btnSearch %>" SkinID="btnPrimary"></asp:LinkButton>
                
            </div>
        </asp:Panel>
        <div class="card">
            <div class="card-body clearfix">
                <div class="grid-card table-responsive no-margin">
                    <asp:GridView ID="grdvSearchResults" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData" DataKeyNames="RIName">
                        <Columns>
                            <asp:BoundField DataField="ReinsurerCode" HeaderText="<%$ Resources:grdv_ReinsurerCode %>" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="RIName" HeaderText="<%$ Resources:grdv_Name %>" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="AccountType" HeaderText="<%$ Resources:grdv_AccType%>"></asp:BoundField>
                            <asp:BoundField DataField="Address1" HeaderText="<%$ Resources:grdv_Address1%>"></asp:BoundField>
                            <asp:BoundField DataField="Address2" HeaderText="<%$ Resources:grdv_Address2%>"></asp:BoundField>
                            <asp:BoundField DataField="PostCode" HeaderText="<%$ Resources:grdv_PostCode%>"></asp:BoundField>
                            <asp:BoundField DataField="ReinsuranceTypeCode" HeaderText="<%$ Resources:grdv_Type%>"></asp:BoundField>
                            <asp:BoundField DataField="BranchCode" HeaderText="<%$ Resources:grdv_Source%>"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("ReinsurerCode") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="btnSelect" runat="server" Text="<%$ Resources:btnSelect%>" CommandName="Select" CausesValidation="false" SkinID="btnGrid"></asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <asp:Panel ID="pnlParticipants" runat="server" Visible="false">
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <legend>
                            <asp:Label ID="Label1" runat="server" Text="Placement Details"></asp:Label></legend>

                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblLower" runat="server" AssociatedControlID="txtLower" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lblLower%>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtLower" runat="server" CssClass="form-control" Text='0' AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblUpper" runat="server" AssociatedControlID="txtUpper" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lblUpper%>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtUpper" runat="server" Text='0' CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body clearfix">
                    <div class="grid-card table-responsive no-margin">
                        <asp:GridView ID="grdPlacements" runat="server" ShowFooter="true" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                            <Columns>
                                <asp:BoundField DataField="PartyCode" HeaderText="<%$ Resources:grdPlacement_ReinsurerCode%>" HtmlEncode="false"></asp:BoundField>
                                <asp:BoundField DataField="PartyName" HeaderText="<%$ Resources:grdPlacement_Name%>" HtmlEncode="false"></asp:BoundField>
                                <asp:BoundField DataField="AccountType" HeaderText="<%$ Resources:grdPlacement_AcctType%>" FooterText="<%$ Resources:grdPlacement_Footer_AcctType%>"></asp:BoundField>
                                <Nexus:TemplateField HeaderText="<%$ Resources:grdPlacement_Participation%>" DataType="Percentage" FooterStyle-CssClass="Doub">
                                    <itemtemplate>
                                        <asp:TextBox ID="txtParticipation" runat="server" OnTextChanged="txtParticipation_TextChanged"   Text='<%# Eval("ParticipationPercentage")%>' AutoPostBack="true" DataFormatString="{0:N2}%" CssClass="form-control Doub"></asp:TextBox>
                                    </itemtemplate>
                                    <footertemplate>
                                        <asp:Label ID="lblParticipationTotal" runat="server" Enabled="false" DataFormatString="{0:N2}%" Text='<%# Eval("TotalParticipationPercentage")%>'></asp:Label>
                                    </footertemplate>
                                </Nexus:TemplateField>
                                <Nexus:TemplateField HeaderText="<%$ Resources:grdPlacement_SumInsured%>" DataType="Currency" FooterStyle-CssClass="Doub">
                                    <itemtemplate>
                                        <asp:Label ID="lblSI" runat="server" Enabled="false" Text='<%# Eval("SumInsured")%>'></asp:Label>
                                    </itemtemplate>
                                    <footertemplate>
                                        <asp:Label ID="lblSITotal" runat="server" Enabled="false" Text='<%# Eval("TotalSI")%>'></asp:Label>
                                    </footertemplate>
                                </Nexus:TemplateField>
                                <Nexus:TemplateField HeaderText="<%$ Resources:grdPlacement_Premium%>" DataType="Currency" FooterStyle-CssClass="Doub">
                                    <itemtemplate>
                                        <asp:TextBox ID="txtPremium" runat="server" AutoPostBack="true" Text='<%# Eval("PremiumValue")%>' OnTextChanged="txtPremium_TextChanged" DataFormatString="{0:N2}%" CssClass="form-control Doub"></asp:TextBox>
                                    </itemtemplate>
                                    <footertemplate>
                                        <asp:Label ID="lblPremiumTotal" runat="server" Enabled="false" Text='<%# Eval("PremiumValueTotal")%>' DataFormatString="{0:N2}%"></asp:Label>
                                    </footertemplate>
                                </Nexus:TemplateField>
                                <Nexus:TemplateField HeaderText="<%$ Resources:grdPlacement_Tax%>" DataType="Currency" FooterStyle-CssClass="Doub">
                                    <itemtemplate>
                                        <asp:Label ID="lblTax" runat="server" Enabled="false" Text='<%# Eval("PremiumTax")%>'></asp:Label>
                                    </itemtemplate>
                                    <footertemplate>
                                        <asp:Label ID="lblTaxTotal" runat="server" Enabled="false"></asp:Label>
                                    </footertemplate>
                                </Nexus:TemplateField>
                                <Nexus:TemplateField HeaderText="<%$ Resources:grdPlacement_ComPerc%>" DataType="Percentage" FooterStyle-CssClass="Doub">
                                    <itemtemplate>
                                        <asp:TextBox ID="txtComPerc" runat="server" OnTextChanged="txtComPerc_TextChanged" DataFormatString="{0:N2}%" AutoPostBack="true" Text='<%# Eval("CommissionPercent")%>' CssClass="form-control Doub"></asp:TextBox>
                                    </itemtemplate>
                                    <footertemplate>
                                        <asp:Label ID="lblComPercTotal" runat="server" Enabled="false" DataFormatString="{0:N2}%"></asp:Label>
                                    </footertemplate>
                                </Nexus:TemplateField>
                                <Nexus:TemplateField HeaderText="<%$ Resources:grdPlacement_Com%>" DataType="Currency" FooterStyle-CssClass="Doub">
                                    <itemtemplate>
                                        <asp:Label ID="lblCom" runat="server" Enabled="false" Text='<%# Eval("CommissionValue")%>'></asp:Label>
                                    </itemtemplate>
                                    <footertemplate>
                                        <asp:Label ID="lblComTotal" runat="server" Enabled="false"></asp:Label>
                                    </footertemplate>
                                </Nexus:TemplateField>
                                <Nexus:TemplateField HeaderText="<%$ Resources:grdPlacement_ComTax%>" DataType="Currency" FooterStyle-CssClass="Doub">
                                    <itemtemplate>
                                        <asp:Label ID="lblComTax" runat="server" Enabled="false" Text='<%# Eval("CommissionTax")%>'></asp:Label>
                                    </itemtemplate>
                                    <footertemplate>
                                        <asp:Label ID="lblComTaxTotal" runat="server" Enabled="false"></asp:Label>
                                    </footertemplate>
                                </Nexus:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:grdPlacement_AgreeCode%>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAgreeCode" runat="server" Text='<%# Eval("AgreementCode")%>' CssClass="form-control" OnTextChanged="txtAgreeCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="rowMenu">
                                            <ol class="list-inline no-margin">
                                                <li class="dropdown no-padding">
                                                    <a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle">
                                                        <i class="fa fa-ellipsis-v" aria-hidden="true"></i>
                                                    </a>
                                                    <ol id='menu_<%# Eval("PartyCode") %>' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                        <li>
                                                            <asp:LinkButton ID="btnRemove" runat="server" Text="<%$ Resources:btnRemove%>" CommandName="Remove" CausesValidation="false"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="btnView" runat="server" Text="<%$ Resources:btnView%>" CommandName="View" CausesValidation="false"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="btnEdit" runat="server" Text="<%$ Resources:btnEdit%>" CommandName="EditPlacement" CausesValidation="false"></asp:LinkButton>
                                                        </li>
                                                    </ol>
                                                </li>
                                            </ol>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PartyKey"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel%>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btnOk%>" CausesValidation="false" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>

</asp:Content>
