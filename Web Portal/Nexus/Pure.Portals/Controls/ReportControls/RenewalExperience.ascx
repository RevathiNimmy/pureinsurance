<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RenewalExperience.ascx.vb"
    Inherits="Nexus.Controls_ReportControls_RenewalExperience" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {

    });
    function setQuote(sPolicyRef, iFileKey, sClientCode) {
        tb_remove();
        document.getElementById('<%= RP__POLICYNUMBER.ClientId%>').value = sPolicyRef;
        document.getElementById('<%= RP__POLICYNUMBER.ClientId%>').focus();
        document.getElementById('<%= txtPolicyRefKey.ClientId%>').value = iFileKey;
    }
    function setClient(sClientName, sClientKey) {
        tb_remove();
        document.getElementById('<%= RP__CORPORATECLIENT.ClientId%>').value = unescape(sClientName);
        document.getElementById('<%= txtClientKey.ClientId%>').value = sClientKey;
        document.getElementById('<%= RP__CORPORATECLIENT.ClientId%>').focus();
    }
    function validate() {

        var field = document.getElementById('<%=RP__CORPORATECLIENT.ClientID %>').value;
        var field1 = document.getElementById('<%=RP__POLICYNUMBER.ClientID %>').value;

        if ((field = "") && (field1 = "")) {
            ValidatorEnable($("#<%= rqdPolicy.ClientID %>")[0], true);
        }
        else {
            ValidatorEnable($("#<%= rqdPolicy.ClientID %>")[0], false);
        }
    }

</script>

<div id="renewalexperience-control">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="RP__CORPORATECLIENT" Text="<%$ Resources:btn_Client %>" ID="lblbtnClient"></asp:Label><div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__CORPORATECLIENT" runat="server" CssClass="form-control" Text="ALL" onBlur="validate(this)"></asp:TextBox><span class="input-group-btn">
                                <asp:LinkButton ID="btnClient" runat="server" OnClientClick="tb_show(null , '../Secure/Agent/FindClient.aspx?modal=true&RequestPage=BG&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;" SkinID="btnModal">
                                <i class="fa fa-search"></i>
                                 <span class="btn-fnd-txt">Client</span>
                                </asp:LinkButton></span>
                        </div>
                    </div>
                    <asp:HiddenField ID="txtClientKey" runat="server"></asp:HiddenField>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateExtracedTo" runat="server" AssociatedControlID="RP__END_DATE" Text="<%$ Resources:lbl_DateExtracedTo %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__END_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calDateOfBirth" runat="server" LinkedControl="RP__END_DATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="vldDateExtracedToRequired" Display="None" ControlToValidate="RP__END_DATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_DateExtracedTo %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="custrngvldDateExtracedTo" runat="server" Display="None" ControlToValidate="RP__END_DATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_DateExtracedTo %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldDateExtracedTo" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_DateExtracedTo %>" ControlToValidate="RP__END_DATE" Display="None" ValidationGroup="vldReportsControlsGroup">
                    </asp:RangeValidator>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="RP__POLICYNUMBER" Text="<%$ Resources:btn_InsuranceFile %>" ID="lblbtnInsuranceFile"></asp:Label><div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__POLICYNUMBER" runat="server" CssClass="form-control" Text="ALL" onBlur="validate(this)"></asp:TextBox><span class="input-group-btn">
                                <asp:LinkButton ID="btnInsuranceFile" runat="server" OnClientClick="tb_show(null , '../Modal/FindInsuranceFile.aspx?Page=Report&modal=true&KeepThis=true&FromPage=Report&TB_iframe=true&height=500&width=750' , null);return false;" SkinID="btnModal">
                                    <i class="fa fa-search"></i>
                                    <span class="btn-fnd-txt">Policy Number</span>

                                </asp:LinkButton></span>
                        </div>

                    </div>


                    <asp:HiddenField ID="txtPolicyRefKey" runat="server"></asp:HiddenField>
                    <asp:RequiredFieldValidator ID="rqdPolicy" runat="server" ControlToValidate="RP__POLICYNUMBER" Display="None" ErrorMessage="<%$ Resources:lbl_req_Policy %>" SetFocusOnError="true" ValidationGroup="vldReportsControlsGroup" Enabled="false"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblLapseCancelFlag" runat="server" AssociatedControlID="RP__LAPSE_CANCEL_FLAG" Text="<%$ Resources:lbl_Lapse_Cancel_Flag %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__LAPSE_CANCEL_FLAG" runat="server" CssClass="form-control form-select">
                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>
</div>
