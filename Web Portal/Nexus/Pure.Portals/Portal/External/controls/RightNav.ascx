<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RightNav.ascx.vb" Inherits="portal_External_controls_RightNav" %>

<li>
    <asp:HyperLink ID="hypBrokerHomePage" runat="server" NavigateUrl="~/secure/FindPolicy.aspx">
        <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">Broker Home Page</span>
    </asp:HyperLink>
</li>
<li>
    <asp:HyperLink ID="hypFindClientPage" runat="server" NavigateUrl="~/secure/agent/FindClient.aspx">
       <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">Find Client</span>
    </asp:HyperLink>
</li>
<li>
    <asp:HyperLink ID="hypDocumentManager" runat="server" NavigateUrl="~/secure/DocumentManager.aspx">
        <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">Document Manager</span>
    </asp:HyperLink>
</li>
<li>
    <asp:HyperLink ID="hypFindFiles" runat="server" NavigateUrl="~/secure/FindFiles.aspx">
        <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">Find Files</span>
    </asp:HyperLink>
</li>
<li>
    <asp:HyperLink ID="hypNewQuote" runat="server" NavigateUrl="~/secure/newQuote.aspx">
          <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">Get a new Quotation</span>
    </asp:HyperLink>
</li>
<li>
    <asp:HyperLink ID="hypEnquiry" runat="server" NavigateUrl="~/secure/FindPolicy.aspx">
         <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">Enquiry</span>
    </asp:HyperLink>
</li>
<li>
    <asp:HyperLink ID="hypRenewalSelection" runat="server" NavigateUrl="~/secure/Agent/RenewalSelection.aspx">
         <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">Renewal Selection</span>
    </asp:HyperLink>
</li>
<li>
    <asp:HyperLink ID="hypViewRenewal" runat="server" NavigateUrl="~/secure/RenewalManager.aspx">
        <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">View Renewals</span>
    </asp:HyperLink>
</li>
<li>
    <asp:HyperLink ID="hypMTA" runat="server" Text="" NavigateUrl="~/secure/FindPolicy.aspx">
          <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">Mid-term Adjustments</span>
    </asp:HyperLink>
</li>
<li>
    <asp:HyperLink ID="hypInsurerPayment" runat="server" Text=" " NavigateUrl="~/secure/InsurerPayments.aspx">
         <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">Insurer Payment</span>
    </asp:HyperLink>
</li>

<li>
    <asp:HyperLink ID="hypTasks" runat="server" NavigateUrl="~/secure/workmanager.aspx">
        <i class="fa fa-dot-circle-o" aria-hidden="true"></i>
        <span class="font-normal">Diary Tasks</span>
    </asp:HyperLink>
</li>


