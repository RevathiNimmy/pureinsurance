<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PolicyDetails.ascx.vb"
    Inherits="Controls_PolicyDetails" EnableViewState="false" %>
<div id="Controls_PolicyDetails">
    <div class="card card-secondary">
        <div class="card-heading">
            <h4>
                <asp:Label ID="lblPolicyDetailsHeading" runat="server" Text="<%$ Resources:lblPolicyDetailsHeading %>"></asp:Label>
            </h4>
        </div>
        <div class="card-body clearfix">
            <asp:Label runat="server" ID="Label5"></asp:Label>
            <div class="form-horizontal">
                <div class="form-group form-group-sm col-lg-4 col-md-6 col-sm-12">
                    <asp:Label ID="lblPolicyNo" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="lt_PolicyNo">
                        <asp:Literal ID="litPolicyNo" runat="server"></asp:Literal></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <p class="form-control-static font-bold">
                            <asp:Literal ID="lt_PolicyNo" runat="server">
                            </asp:Literal>
                        </p>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-4 col-md-6 col-sm-12">
                    <asp:Label ID="lblClientName" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="lt_ClientName">
                        <asp:Literal ID="litClientName" runat="server"></asp:Literal></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <p class="form-control-static font-bold">
                            <asp:Literal ID="lt_ClientName" runat="server">
                            </asp:Literal>
                        </p>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-4 col-md-6 col-sm-12">
                    <asp:Label ID="lblCovertStartDate" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="lt_CovertStartDate">
                        <asp:Literal ID="litCovertStartDate" runat="server" Text="<%$ Resources:litCovertStartDate %>"></asp:Literal></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <p class="form-control-static font-bold">
                            <asp:Literal ID="lt_CovertStartDate" runat="server">
                            </asp:Literal>
                        </p>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-4 col-md-6 col-sm-12">
                    <asp:Label ID="lblCovertEndDate" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="lt_CovertEndDate">
                        <asp:Literal ID="litCovertEndDate" runat="server" Text="<%$ Resources:litCovertEndDate %>"></asp:Literal></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <p class="form-control-static font-bold">
                            <asp:Literal ID="lt_CovertEndDate" runat="server">
                            </asp:Literal>
                        </p>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-4 col-md-6 col-sm-12">
                    <asp:Label ID="lblPolicyType" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="lt_PolicyType">
                        <asp:Literal ID="litPolicyType" runat="server" Text="<%$ Resources:litPolicyType %>"></asp:Literal></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <p class="form-control-static font-bold">
                            <asp:Literal ID="lt_PolicyType" runat="server">
                            </asp:Literal>
                        </p>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-4 col-md-6 col-sm-12">
                    <asp:Label ID="lbl_BillingMethod" runat="server" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_BillingMethod%>" Visible="false" AssociatedControlID="txtBillingMethod">
                    </asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <p class="form-control-static font-bold">
                            <asp:Label ID="txtBillingMethod" runat="server" Visible="false"></asp:Label>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


