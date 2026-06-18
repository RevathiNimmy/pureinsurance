<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClientQuotes.ascx.vb"
    Inherits="Nexus.Controls_ClientQuotes" EnableViewState="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div id="Controls_ClientQuotes">

    <script type="text/javascript" language="javascript">

        function collapseAllGrid() {


            var ids = document.getElementById('<%=hvGridIDs.ClientID %>').value;
            var array = ids.split(',');
            var i = 0;
            for (i = 0; i < array.length; i++) {
                if (document.getElementById(array[i]) != null) {
                    document.getElementById(array[i]).style.display = 'none';
                }
            }
        }
        function toggle(obj, flag) {

            if (flag) {
                document.getElementById(obj.substr(0, 52) + 'grdvSubBroker').style.display = 'none';
                document.getElementById(obj).style.display = 'none';
                document.getElementById(obj.substr(0, 55) + 'Expand').style.display = 'block';

            }
            else {
                document.getElementById(obj.substr(0, 52) + 'grdvSubBroker').style.display = 'block';
                document.getElementById(obj).style.display = 'block';

                document.getElementById(obj.substr(0, 55) + 'Collapse').style.display = 'block';
                document.getElementById(obj.substr(0, 55) + 'Expand').style.display = 'none';
            }


        }
    </script>

    <div class="fieldset-wrapper">
        
        <fieldset>
            <legend><span id="pnlQuotesPoliciesTitle" runat="server" title="<%$ Resources:titleExpandCollapse %>">
                <asp:Literal ID="lbl_Quote_header" runat="server" Text="<%$ Resources:lbl_Quote_header %>"></asp:Literal></span></legend>
            <asp:Panel ID="pnl_QuotesPolicies" runat="server">
              <%--  <asp:UpdatePanel ID="updPanelClientQuotes" runat="server">
                    <ContentTemplate>--%>
                        <asp:HiddenField ID="hvGridIDs" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hvIsBroker" runat="server"></asp:HiddenField>
                        <div class="showall">
                            <asp:Panel ID="PanelViewAllQuotesPolicies" runat="server">
                                <asp:CheckBox ID="chkViewAllPolicies" runat="server" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                <asp:Label ID="lbl_ViewAllPolicies" runat="server" Text="<%$ Resources:lbl_ViewAllPolicies %>"></asp:Label>
                            </asp:Panel>
                        </div>
                        <asp:Label ID="lbl_FD_MTAExist" runat="server" Text="<%$ Resources:lbl_FD_MTAExistMsg %>" Visible="false" CssClass="error"></asp:Label>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="grdvQuotes" runat="server" A="" AutoGenerateColumns="False" DataKeyNames="InsuranceFolderKey,BaseInsuranceFolderKey" GridLines="None" PageSize="10" AllowPaging="true" AllowSorting="true" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle CssClass="border"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="ImgCollapse" runat="server" ImageUrl="~/images/collapse.gif" border="0" Style="display: none;" onClick="toggle(this.id,1);"></asp:Image>
                                            <asp:Image ID="ImgExpand" runat="server" ImageUrl="~/images/expand.gif" border="0" onClick="toggle(this.id,0);"></asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="<%$ Resources:lbl_Reference %>"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="InsuranceFileTypeCode">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Status" runat="server" Text='<%# Eval("InsuranceFileTypeCode") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RiskStatus" SortExpression="RiskStatus" HeaderText="<%$ Resources:lbl_RiskStatus %>"></asp:BoundField>
                                    <asp:BoundField DataField="ProductCode" SortExpression="ProductCode" HeaderText="<%$ Resources:lbl_Type %>"></asp:BoundField>
                                    <asp:BoundField DataField="DateIssued" SortExpression="DateIssued" HeaderText="<%$ Resources:lbl_DateIssued %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                    <asp:BoundField DataField="LeadAgent" SortExpression="LeadAgent" HeaderText="<%$ Resources:lbl_AgentName %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                    <asp:BoundField DataField="CoverStartDate" SortExpression="CoverStartDate" HeaderText="<%$ Resources:lbl_StartDate %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:BoundField DataField="ExpiryDate" SortExpression="ExpiryDate" HeaderText="<%$ Resources:lbl_EndDate %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_RiskType %>" SortExpression="RiskType">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_RiskType" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="<%$ Resources:lbl_RiskDescription %>" SortExpression="RiskDescription">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_RiskDesc" runat="server"/>
                                        </ItemTemplate>
                                         </asp:TemplateField>
                            <nexus:boundfield datafield="AnnualPremium" sortexpression="AnnualPremium" headertext="<%$ Resources:lbl_AnnualPremium %>" dataformatstring="{0:N2}" datatype="Currency" />
                            <asp:TemplateField HeaderText="Referral Status" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_IsReferred" runat="server" />
                                </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <div id="divMenuItem" runat="server" class="rowMenu"><ol class="list-inline no-margin"><li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                <ol id="menu_<%# Eval("InsuranceFileKey") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                    <li>
                                                        <asp:linkbutton id="lnkbtnSelect" runat="server" causesvalidation="False" commandname="Select"></asp:linkbutton></li>
                                                    <li>
                                                        <asp:linkbutton id="lnkbtnSelect2" runat="server" causesvalidation="False" commandname="Select"></asp:linkbutton></li>  
                                                    <li>
                                                        <asp:LinkButton ID="lnkbtnView" runat="server" CausesValidation="False" CommandName="Select" /></li>                                                    
                                                    <li> 
                                                        <asp:linkbutton id="lnkbtnCopyQuote" runat="server" visible="false" text="<%$ Resources:lbl_CopyQuote %>" commandname="CopyQuote" causesvalidation="False"></asp:linkbutton></li>
                                                </ol>
                                            </li></ol></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <tr>
                                                <td colspan="100%">
                                                    <div style="position: relative; background-color: White; overflow: auto; left: 0px; max-height: 200px;">
                                                        <asp:GridView ID="grdvSubBroker" runat="server" AutoGenerateColumns="False" DataKeyNames="InsuranceFolderKey,InsuranceFileKey" GridLines="None" CellPadding="0" CellSpacing="0" PageSize="20" AllowPaging="False" AllowSorting="true" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData" Width="100%" OnRowCommand="grdvSubBroker_RowCommand" OnRowDataBound="grdvSubBroker_RowDataBound" ShowFooter="false">
                                                            <Columns>
                                                                <asp:BoundField DataField="Reference" HeaderText="<%$ Resources:lbl_Reference %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_ChildStatus" runat="server" Text='<%#Eval("InsuranceFileTypeCode")%>'></asp:Label>
                                                                        <%--  <asp:BoundField DataField="InsuranceFileTypeCode" SortExpression="InsuranceFileTypeCode"
                                                                    HeaderText="<%$ Resources:lbl_Status %>" ItemStyle-CssClass="span-3" />--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ProductCode" HeaderText="<%$ Resources:lbl_Type %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                                        <asp:BoundField DataField="DateIssued" HeaderText="<%$ Resources:lbl_DateIssued %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                                        <asp:BoundField DataField="LeadAgent" HeaderText="<%$ Resources:lbl_AgentName %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                                        <%--   <asp:BoundField DataField="ClientName" HeaderText="<%$ Resources:lbl_InsurerShortName %>"
                                                                    ItemStyle-CssClass="span-2" />--%>
                                                        <asp:BoundField DataField="CoverStartDate" HeaderText="<%$ Resources:lbl_StartDate %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                                        <asp:BoundField DataField="ExpiryDate" HeaderText="<%$ Resources:lbl_EndDate %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                                        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="span-3">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtnDetails" runat="server" CausesValidation="False" CommandName="Details" Text="Details" CommandArgument='<%#Eval("InsuranceFileKey") %>'></asp:LinkButton>
                                                                <%-- <asp:LinkButton ID="lnkbtnDetails" runat="server" CausesValidation="False" CommandName="Details"
                                                                            Text="Details" CommandArgument='<%#Eval("BaseInsuranceFolderKey") %>' />--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblInsuranceFolderKey" runat="server" CausesValidation="False" Text='<%#Eval("InsuranceFolderKey") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                        
                    </asp:GridView>
                </div>
                <%--  </ContentTemplate>
                </asp:UpdatePanel>
                <nexus:ProgressIndicator ID="upQuotes" OverlayCssClass="updating" AssociatedUpdatePanelID="updPanelClientQuotes"
                    runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </nexus:ProgressIndicator>--%>
            </asp:Panel>
            <asp:TextBox ID="hdMarkedtext" runat="server" Style="visibility: hidden"></asp:TextBox>
        </fieldset>
    </div>
    

    <script type="text/javascript" language="javascript">
        collapseAllGrid();
    </script>

</div>
