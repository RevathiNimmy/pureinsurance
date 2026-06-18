<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Associate.aspx.vb" Inherits="Nexus.Modal_Associate"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function setClient(sClientName, sClientKey, sResolveName) {
            tb_remove();
            document.getElementById('<%=txtClient.ClientID %>').value = unescape(sClientName);
            document.getElementById('<%=AssociateKey.ClientID %>').value = sClientKey;
            document.getElementById('<%=AssociateName.ClientID %>').value = unescape(sResolveName);
        }

        function UpdateAssociateData() {
            var AssociateData;
            var oDDL;
            //to Fire teh Client Validation first
            Page_ClientValidate();

            if (Page_IsValid == true) {
                //Mode
                AssociateData = document.getElementById('<%=txtMode.ClientID %>').value + ";";
                //Client Code
                AssociateData += document.getElementById('<%=txtClient.ClientID %>').value + ";";
                //Client Key
                AssociateData += document.getElementById('<%=AssociateKey.ClientID %>').value + ";";
                //Client Name
                AssociateData += document.getElementById('<%=AssociateName.ClientID %>').value + ";";
                //Relationship Code
                oDDL = document.getElementById('<%=GISAssociate_Relationship.ClientID %>');
                AssociateData += oDDL.options[oDDL.selectedIndex].value + ";";
                //Relationship Description
                AssociateData += oDDL.options[oDDL.selectedIndex].text + ";";
                //Key
                AssociateData += document.getElementById('<%=txtKey.ClientID %>').value + ";";

                self.parent.tb_remove();
                self.parent.ReceiveAssociateData(AssociateData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }
        }
    </script>

    <div id="Modal_Associate">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Associate_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClient" Text="<%$Resources:lbl_btnClient %>" ID="lblbtnClient"></asp:Label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtClient" CssClass="field-mandatory form-control" runat="server"></asp:TextBox><span class="input-group-btn"><asp:LinkButton ID="btnClient" runat="server" OnClientClick="tb_show(null , '../secure/agent/FindClient.aspx?RequestPage=BG&ClaimFlag=1&modal=true&KeepThis=true&TB_iframe=true&height=550&width=650' , null);return false;" SkinID="btnModal">
                                    <i class="fa fa-search"></i>
                                     <span class="btn-fnd-txt">Client</span>
                                </asp:LinkButton></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdClientCode" runat="server" ControlToValidate="txtClient" ValidationGroup="AssociateGroup" ErrorMessage="<%$Resources:lbl_Error_ClientID %>" Display="none" SetFocusOnError="True" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAssociate" runat="server" AssociatedControlID="GISAssociate_Relationship" Text="<%$ Resources:lbl_PnlAssociate_Relationship%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISAssociate_Relationship" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Relationship_Type" CssClass="field-medium field-mandatory form-control form-select" DefaultText="(Please Select)"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdAssociate" runat="server" ControlToValidate="GISAssociate_Relationship" ErrorMessage="<%$ Resources:lbl_ErrMsg_Associate %>" Display="none" SetFocusOnError="true" Enabled="true" ValidationGroup="AssociateGroup"></asp:RequiredFieldValidator>
                    </div>
                    <asp:HiddenField ID="AssociateKey" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="AssociateName" runat="server"></asp:HiddenField>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnAddAssociate" runat="server" Text="<%$ Resources:lbl_btnAddAssociate %>" ValidationGroup="AssociateGroup" CausesValidation="true" OnClientClick="UpdateAssociateData()" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnUpdateAssociate" runat="server" Text="<%$ Resources:lbl_btnUpdateAssociate %>" Visible="false" ValidationGroup="AssociateGroup" CausesValidation="true" OnClientClick="UpdateAssociateData()" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtKey" runat="server"></asp:HiddenField>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" HeaderText="<%$ Resources:lbl_ValidationSummary %>" DisplayMode="BulletList" ValidationGroup="AssociateGroup" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
