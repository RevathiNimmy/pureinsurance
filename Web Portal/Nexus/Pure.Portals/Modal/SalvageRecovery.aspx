<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SalvageRecovery.aspx.vb"
    Inherits="Nexus.Modal_SalvageRecovery" MasterPageFile="~/default.master" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript">
        //    function isInteger(e){
        //	    var key = window.event ? e.keyCode : e.which;
        //	    var keychar = String.fromCharCode(key);
        //	    reg = /\d/;
        //	    return reg.test(keychar);
        //    }
        function ConvertFindToDecimal() {
            var ctrl = document.getElementById('<%=txtInitialReserve.ClientID %>').value;
            var myNum = new Number(ctrl);
            if (myNum.toFixed(2) != 'NaN') {
                document.getElementById('<%=txtInitialReserve.ClientID %>').value = myNum.toFixed(2);
            }
        }
        function isInteger(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            var ValidChars = "0123456789.";
            var IsNumber = true;
            if (ValidChars.indexOf(keychar) == -1) {
                IsNumber = false;
            }
            return IsNumber;
        }
        function setOtherParty(sName, sKey, sAgentCode) {
            document.getElementById('<%= txtParty.ClientID %>').value = sName;
            document.getElementById('<%= hPartyKey.ClientID %>').value = sKey;
            tb_remove();
        }
        function setReInsurer(sName, sKey, sCode) {
            document.getElementById('<%= txtParty.ClientID %>').value = sName;
            document.getElementById('<%= hPartyKey.ClientID %>').value = sKey;
            tb_remove();
        }
    </script>

    <div id="Modal_SalvageRecovery">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_SalvageRecovery_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblSalvageRecoveryReserve" runat="server" Text="<%$ Resources:lbl_SalvageRecoveryReserve %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRecoveryType" runat="server" AssociatedControlID="ddlRecoveryType" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltRecoveryType" runat="server" Text="<%$ Resources:lbl_RecoveryType%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlRecoveryType" runat="server" Enabled="True" CssClass="form-control form-select">
                                </asp:DropDownList>
                            </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPartyType" runat="server" AssociatedControlID="GISLookup_RecoveryPartyType" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltPartyType" runat="server" Text="<%$ Resources:lbl_PartyType%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="GISLookup_RecoveryPartyType" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Recovery_Party_Type" CssClass="form-control form-select" DefaultText="(Please Select)" AutoPostBack="true"></NexusProvider:LookupList>
                            </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblParty" runat="server" AssociatedControlID="txtParty" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltParty" runat="server" Text="<%$ Resources:lbl_Party%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div style="display:flex; gap:4px; align-items:center;">
                                <asp:TextBox ID="txtParty" runat="server" CssClass="form-control" style="flex:1; min-width:0;" />
                                <asp:LinkButton ID="btnParty" runat="server" SkinID="btnPrimary" CausesValidation="false" Text="Find Party" style="white-space:nowrap; flex-shrink:0;"></asp:LinkButton>
                            </div>
                        </div>
                        <asp:HiddenField ID="hPartyKey" runat="server"></asp:HiddenField>
                    </div>
                    <div class="AlternateListItem form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInitialReserve" runat="server" AssociatedControlID="txtInitialReserve" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltInitialReserve" runat="server" Text="<%$ Resources:lbl_InitialReserve%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtInitialReserve" runat="server" CssClass="form-control e-num2 form-control" MaxLength="15" onkeypress="javascript:return isInteger(event);" onblur="ConvertFindToDecimal()"></asp:TextBox>
                            </div>
                    </div>
                </div>
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblRecoveryParties" runat="server" Text="<%$ Resources:lbl_RecoveryParties %>"></asp:Label></legend>
                    <div class="grid-card table-responsive">
                        <asp:GridView ID="gvRecoveryParty" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:lbl_NoParties %>">
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <Columns>
                                <asp:BoundField HeaderText="<%$ Resources:lbl_PartyType %>" DataField="RecoveryPartyTypeCode"></asp:BoundField>
                                <asp:BoundField HeaderText="<%$ Resources:lbl_PartyName %>" DataField="PartyShortName"></asp:BoundField>
                                <asp:BoundField HeaderText="<%$ Resources:lbl_InitialReserve %>" DataField="InitialRecovery" DataFormatString="{0:N2}"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="rowMenu">
                                            <ol class="list-inline no-margin">
                                                <li>
                                                    <asp:LinkButton ID="btnRemove" runat="server" CommandName="Delete" Text="<%$ Resources:btn_Remove %>" SkinID="btnGrid" CausesValidation="false"></asp:LinkButton>
                                                </li>
                                            </ol>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style="margin-top:15px;">
                        <asp:Label ID="lblTotalLabel" runat="server" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltTotal" runat="server" Text="<%$ Resources:lbl_Total%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:Label ID="lblRecoveryReserveTotal" runat="server" CssClass="form-control" Text="0.00"></asp:Label>
                            </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary" CausesValidation="false"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btnAddParty %>" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" Text="<%$ Resources:btnNext %>" SkinID="btnPrimary" CausesValidation="false"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="CustValidType" runat="server" ControlToValidate="txtInitialReserve" SetFocusOnError="true" Display="none"></asp:CustomValidator>
        <asp:CustomValidator ID="CustValidPartyType" runat="server" Display="none"></asp:CustomValidator>
        <asp:CustomValidator ID="CustValidParty" runat="server" Display="none"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
