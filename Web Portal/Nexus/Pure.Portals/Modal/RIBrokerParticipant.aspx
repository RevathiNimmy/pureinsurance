<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="RIBrokerParticipant.aspx.vb" Inherits="Modal_RIBrokerParticipant" %>

<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        function updateControls(elementId) {
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
    <div id="Modal_RIBrokerParticipant">
        <asp:ScriptManager ID="smRIBroker" runat="server"></asp:ScriptManager>
        <asp:Panel ID="pnlFindRIBrokerPart" runat="server" DefaultButton="btnSearch" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lblTitle %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblFindRIBrokerPart" runat="server" Text="<%$ Resources:lblFindRIBrokerPart %>"></asp:Label></legend>
                  
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
        <div class="grid-card table-responsive">
            <asp:GridView ID="grdvSearchResults" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData" DataKeyNames="RIName">
                <Columns>
                    <asp:BoundField DataField="ReinsurerCode" HeaderText="<%$ Resources:grdv_ReinsurerCode %>"></asp:BoundField>
                    <asp:BoundField DataField="RIName" HeaderText="<%$ Resources:grdv_Name %>"></asp:BoundField>
                    <asp:BoundField DataField="Address1" HeaderText="<%$ Resources:grdv_Address1%>"></asp:BoundField>
                    <asp:BoundField DataField="Address2" HeaderText="<%$ Resources:grdv_Address2%>"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div class="rowMenu">
                                <ol id="menu_<%# Eval("ReinsurerCode") %>" class="list-inline no-margin">
                                    <li>
                                        <asp:LinkButton ID="btnSelect" runat="server" CommandArgument="ReinsurerCode" Text="<%$ Resources:btnSelect%>" CommandName="Select" CausesValidation="false" SkinID="btnGrid"></asp:LinkButton>
                                    </li>
                                </ol>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Panel ID="pnlFACProp_Part" CssClass="card" runat="server">
            <div class="card-body clearfix">
                <legend>
                    <asp:Label ID="lbl_pnlFACProp" runat="server" Text="<%$ Resources:lbl_pnlFACProp %>"></asp:Label></legend>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdParticipants" runat="server" ShowFooter="true" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                        <Columns>
                            <asp:BoundField DataField="PartyCode" HeaderText="<%$ Resources:grdPart_ReinsurerCode %>"></asp:BoundField>
                            <asp:BoundField DataField="PartyName" HeaderText="<%$ Resources:grdPart_Name %>" FooterText="<%$ Resources:grdPart_Total %>"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:grdPart_Participation %>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtParticipation" runat="server" CssClass="form-control" Text='<%# Eval("ParticipationPercentage")%>' DataFormatString="{0:N2}%" AutoPostBack="true" OnTextChanged="txtParticipation_TextChanged"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTotalParticipation" runat="server" CssClass="form-control" Enabled="false" Text='<%# Eval("TotalParticipationPercentage")%>' DataFormatString="{0:N2}%"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PartyKey" HeaderText="<%$ Resources:grdPart_ReinsurerCode %>"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("PartyCode") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="btnRemove" runat="server" Text="<%$ Resources:btnRemove %>" CommandName="Remove" CausesValidation="false" SkinID="btnGrid"></asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
        <div class="card-footer">
            <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btnOk %>" CausesValidation="false" SkinID="btnPrimary"></asp:LinkButton>
        </div>

    </div>
</asp:Content>
