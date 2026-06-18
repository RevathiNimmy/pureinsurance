<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CoInsurance.aspx.vb" Inherits="Nexus.Modal_CoInsurance1"
    MasterPageFile="~/default.master" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function setReInsurer(sName, sKey) //setCoinsurer
        {
            //debugger
            document.getElementById('<%=txtCoinsurer.ClientID %>').value = unescape(sName);
            document.getElementById('<%=hiddenCoinsurerCode.ClientID %>').value = sKey;
            tb_remove()
        }
    </script>

    <div id="Modal_CoInsurance">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="CoInsurance"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCoInsurer" CssClass="col-md-4 col-sm-3 control-label" runat="server" Text="<%$ Resources:lbl_Coinsurer %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtCoinsurer" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnCoInsurer" SkinID="btnModal" runat="server" CausesValidation="false" OnClientClick="tb_show(null , '../Modal/FindReinsurer.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;">
                                    <i class="fa fa-search"></i>
                                    <span class="btn-fnd-txt">Coinsurer</span>
                                    </asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCoinsurer" ValidationGroup="CoInsuranceGroup" ErrorMessage="<%$Resources:lbl_Error_CoInsurer %>" Display="none" SetFocusOnError="True" Enabled="True"></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hiddenCoinsurerCode" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblArrangement_Ref" runat="server" Text="<%$Resources:lbl_Arrangement_Ref %>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtArrangementRef"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtArrangementRef" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblShare" runat="server" Text="<%$Resources:lbl_SharePerc %>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtShare"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtShare" runat="server" CssClass="form-control field-mandatory e-num2" Text="0.00"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdShare" runat="server" ControlToValidate="txtShare" ValidationGroup="CoInsuranceGroup" ErrorMessage="<%$Resources:lbl_Error_SharePerc %>" Display="none" SetFocusOnError="True" Enabled="True"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCommission" runat="server" Text="<%$Resources:lbl_Commission %>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtCommission"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCommission" CssClass="form-control" runat="server" Text="0.00"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnAdd" runat="server" Text="<%$Resources:lbl_Add %>" ValidationGroup="CoInsuranceGroup" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnUpdate" runat="server" Text="<%$Resources:lbl_Update %>" ValidationGroup="CoInsuranceGroup" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="cusValidCoinsurer" runat="server" ValidationGroup="CoInsuranceGroup" ErrorMessage="<%$Resources:Err_ValidCo_Insurer %>"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" HeaderText="<%$Resources:lbl_Error_Summary %>" DisplayMode="BulletList" ValidationGroup="CoInsuranceGroup" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
    <script type="text/javascript">
        document.getElementById("<%= txtCoinsurer.ClientID%>").readOnly = true;
    </script>
</asp:Content>
