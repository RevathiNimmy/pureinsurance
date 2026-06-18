<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PolicyAssociate.aspx.vb"
    Inherits="Nexus.Modal_PolicyAssociate" MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function setClient(sClientName, sClientKey, sResolveName) {
            tb_remove();
            document.getElementById('<%=txtClient.ClientID %>').value = sClientName;
            document.getElementById('<%=AssociateKey.ClientID %>').value = sClientKey;
            document.getElementById('<%=AssociateName.ClientID %>').value = sResolveName;
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
                oDDL = document.getElementById('<%=ddlAssociation.ClientID %>');
                AssociateData += oDDL.options[oDDL.selectedIndex].value + ";";
                //Relationship Description
                AssociateData += oDDL.options[oDDL.selectedIndex].text + ";";
                //Key
                AssociateData += document.getElementById('<%=txtKey.ClientID %>').value + ";";

                self.parent.tb_remove();
                self.parent.ReceiveAssociateData(AssociateData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }
        }

        function checkTextAreaMaxLength(textBox, e, length) {

            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                    {
                        e.returnValue = false;
                        return false;
                    }
                    else//Firefox
                        e.preventDefault();
                }
            }
        }

        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 35 && e.keyCode != 36 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
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
                        <asp:Label ID="lblClientName" runat="server" AssociatedControlID="txtClient" Text="<%$ Resources:lbl_Name %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClient" CssClass="field-mandatory form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAssociation" runat="server" AssociatedControlID="ddlAssociation" Text="<%$ Resources:lbl_Association%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlAssociation" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Association_Type" CssClass="field-medium field-mandatory form-control form-select" DefaultText="(Please Select)"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdAssociate" runat="server" ControlToValidate="ddlAssociation" ErrorMessage="<%$ Resources:lbl_ErrMsg_Associate %>" Display="none" SetFocusOnError="true" Enabled="true" ValidationGroup="AssociateGroup"></asp:RequiredFieldValidator>
                    </div>
                   
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtAssociationDetail" Text="<%$ Resources:lbl_AssociationDetail%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAssociationDetail" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="5" Columns="80"  MaxLength="245" onkeyDown="return checkTextAreaMaxLength(this,event,'245');" ></asp:TextBox>
                                                       
                        </div>
                    </div>
                    </div>
                    <asp:HiddenField ID="hdnDateAttached" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnDateRemoved" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="AssociateKey" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="AssociateName" runat="server"></asp:HiddenField>
                </div>
            </div>

        </div>

        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtKey" runat="server"></asp:HiddenField>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" HeaderText="<%$ Resources:lbl_ValidationSummary %>" DisplayMode="BulletList" ValidationGroup="AssociateGroup" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
        <div class="card-footer">
            <asp:LinkButton ID="btnOK" runat="server" Text="<%$ Resources:lbl_btnAddAssociate %>" ValidationGroup="AssociateGroup" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lbl_btnCancel %>" ValidationGroup="AssociateGroup" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
        </div>
    </div>
</asp:Content>
