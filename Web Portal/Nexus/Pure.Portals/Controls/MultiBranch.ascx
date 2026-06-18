<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MultiBranch.ascx.vb"
    Inherits="Nexus.Controls_MultiBranch" EnableViewState="true" %>
<div id="Controls_MultiBranch" onkeypress="return WebForm_FireDefaultButton(event, '<%= btnNext.ClientID %>')">
    <div class="card">
        <div class="card-heading">
            <h1>
                <asp:Literal ID="ltTitle" runat="server" Text="<%$ Resources:lbl_Title %>" EnableViewState="false"></asp:Literal></h1>
        </div>
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <span>Select Branch</span>
                </legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblSelectBranch" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="ddlBranchlst" Text="<%$ Resources:lbl_SelectBranch %>"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="ddlBranchlst" runat="server" CssClass="form-control field-mandatory form-select" TabIndex="1"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="vldrqdBranchlst" runat="server" ControlToValidate="ddlBranchlst" Display="none" SetFocusOnError="true" ErrorMessage="<%$ Resources:Errlbl_rqdddlBranchlst%>" InitialValue=""></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                 <asp:Label ID="lblmsg" runat="server" Visible ="false"  Text="Please select branch !" ForeColor="Red" ></asp:Label>
            </div>
                </div>
        </div>
        <div class="card-footer">           
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:lbl_Review %>"></asp:Literal>
            <asp:LinkButton ID="btnNext" TabIndex="2" runat="server" Text="<%$ Resources: btn_Next %>" SkinID="btnPrimary">
            </asp:LinkButton>
        </div>
    </div>
    <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
</div>

