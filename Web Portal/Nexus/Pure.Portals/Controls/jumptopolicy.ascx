<%@ Control Language="VB" AutoEventWireup="false" CodeFile="JumpToPolicy.ascx.vb"
    Inherits="Nexus.Controls_jumptopolicy" EnableViewState="false" %>
<div id="Controls_jumptopolicy">
    <div class="card">
        <div class="card-body clearfix">
            
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblPolicySearchHeading" runat="server" Text="<%$ Resources:lbl_PolicySearchHeading %>"></asp:Label></legend>
                
            
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicySearch" runat="server" AssociatedControlID="txtPolicyNo" Text="<%$ Resources:lbl_PolicySearch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtPolicyNo" runat="server" cssclass="form-control"></asp:TextBox></div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_ErrMsg" runat="server" Visible="false" ForeColor="red" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                </div>
        </div>
        
    </div>
    <div class="card-footer">
        <asp:LinkButton ID="btnGo" Text="<%$ Resources:lbl_btnGo %>" runat="server" SkinID="btnPrimary"></asp:LinkButton>
    </div>
</div>
