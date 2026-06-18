<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AgentDetailsForClaims.aspx.vb"
    Inherits="Nexus.Modal_AgentDetailsForClaims" MasterPageFile="~/default.master" %>

<%@ Register Src="~/Controls/AddressCntrl.ascx" TagName="AddressControl" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            if (!$("#<%=txtContact.ClientID %>").attr("disabled") && $("#<%=txtContact.ClientID %>").attr("readonly")) {
                $("#<%=txtContact.ClientID %>").removeAttr("readonly");
            }
            if (!$("#<%=txtTelephoneNumber.ClientID %>").attr("disabled") && $("#<%=txtTelephoneNumber.ClientID %>").attr("readonly")) {
                $("#<%=txtTelephoneNumber.ClientID %>").removeAttr("readonly");
            }
            if (!$("#<%=txtEmailAddress.ClientID %>").attr("disabled") && $("#<%=txtEmailAddress.ClientID %>").attr("readonly")) {
                $("#<%=txtEmailAddress.ClientID %>").removeAttr("readonly");
            }
            if (!$("#<%=txtFaxNumber.ClientID %>").attr("disabled") && $("#<%=txtFaxNumber.ClientID %>").attr("readonly")) {
                $("#<%=txtFaxNumber.ClientID %>").removeAttr("readonly");
            }
            if (!$("#<%=txtAgentClaimNumber.ClientID %>").attr("disabled") && $("#<%=txtAgentClaimNumber.ClientID %>").attr("readonly")) {
                $("#<%=txtAgentClaimNumber.ClientID %>").removeAttr("readonly");
            }
        });

        function ReceiveAddressData(sAddressData, sPostBackTo) {
            document.getElementById('<%=txtAddressData.ClientID %>').value = sAddressData;
            __doPostBack(sPostBackTo, 'UpdateAddress');
        }

        function pageLoad() {
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
        }

        function OnBeginRequest(sender, args) {
            var postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'btnAgentAddress') {
                $get(uprogQuotes).style.display = "block";
            }
        }

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="Modal_AgentDetailsForClaims">
        <div class="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAgentName" runat="server" Text="<%$ Resources:agentdetails_AgentName %>" AssociatedControlID="txtAgentName" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAgentName" runat="server" Columns="10" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:UpdatePanel ID="updateAddress" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblAddress" runat="server" Text="<%$ Resources:lbl_Address %>" AssociatedControlID="txtAddress" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Enabled="false" CssClass="form-control">
                                        </asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton SkinID="btnModal" runat="server" ID="btnAgentAddress">
                                                <i class="fa fa-search"></i>
                                                 <span class="btn-fnd-txt">Address</span>
                                            </asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="upAddress" OverlayCssClass="updating" AssociatedUpdatePanelID="updateAddress" runat="server">
                            <progresstemplate>
                                    </progresstemplate>
                        </Nexus:ProgressIndicator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTelephoneNumber" runat="server" Text="<%$ Resources:agentdetails_TelephoneNumber %>" AssociatedControlID="txtTelephoneNumber" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtTelephoneNumber" runat="server" Columns="10" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFaxNumber" runat="server" Text="<%$ Resources:agentdetails_FaxNumber %>" AssociatedControlID="txtFaxNumber" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFaxNumber" runat="server" Columns="10" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblContact" runat="server" Text="<%$ Resources:agentdetails_Contact %>" AssociatedControlID="txtContact" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtContact" runat="server" Columns="10" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblEmailAddress" runat="server" Text="<%$ Resources:agentdetails_EmailAddress %>" AssociatedControlID="txtEmailAddress" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtEmailAddress" runat="server" Columns="10" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAgentClaimNumber" runat="server" Text="<%$ Resources:agentdetails_AgentClaimNumber %>" AssociatedControlID="txtAgentClaimNumber" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAgentClaimNumber" runat="server" Columns="10" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:agentdetails_btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:agentdetails_btnOk %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
            <asp:HiddenField ID="txtAddressData" runat="server"></asp:HiddenField>
        </div>
    </div>
</asp:Content>
