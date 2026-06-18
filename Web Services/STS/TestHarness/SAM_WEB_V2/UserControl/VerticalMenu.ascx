<%@ Control Language="VB" AutoEventWireup="false" CodeFile="VerticalMenu.ascx.vb"
    Inherits="UserControl_VerticalMenu" %>
<link href="../Images/loginstylesheet.css" rel="stylesheet" type="text/css" />
<table cellpadding="0" cellspacing="0" id="box" >
    <tr>
        <td width="16px">
            <img src="../images/box_topleft.gif" /></td>
        <td>
            <img src="../images/box_topmid.gif" /></td>
        <td width="16px">
            <img src="../images/box_topright.gif" /></td>
    </tr>
    <tr>
        <td colspan="3" class="boxbg" valign ="top" align ="center">
            <table class="links">
                <tr>
                    <td align="left" valign ="top">
                        <asp:TreeView ID="TreeView1" runat="server" Height="34%" ImageSet="Arrows" >
                <ParentNodeStyle Font-Bold="False" />
                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                <SelectedNodeStyle Font-Underline="False" HorizontalPadding="0px"
                    VerticalPadding="0px" ForeColor="#BB0D28" />
                <Nodes>
                    <asp:TreeNode Text="Claim" Value="View Claim">
                        <asp:TreeNode Text="Open Claim" Value="Open Claim" NavigateUrl="~/OpenClaim/1_FindInsuranceFileForClaim.aspx"></asp:TreeNode>
                        <asp:TreeNode Text="View Claim" Value="View Claim" NavigateUrl="~/View Claim/1_FindClaim.aspx"></asp:TreeNode>
                        <asp:TreeNode Text="Maintain Claim" Value="Maintain Claim" NavigateUrl="~/Maintain Claim/1_FindClaim.aspx"></asp:TreeNode>
                        <asp:TreeNode Text="Claim Payment" Value="Claim Payment" NavigateUrl="~/Claim Payment/1_FindClaim.aspx"></asp:TreeNode>
                        <asp:TreeNode Text="Claim Receipt" Value="Claim Receipt" NavigateUrl="~/Claim Receipt/1_FindClaim.aspx"></asp:TreeNode>
                        
                    </asp:TreeNode>
                     <asp:TreeNode Text="Insurer Payment" Value="Insurer Payment" NavigateUrl="~/InsurerAgentPayment/InsurerAgentPayment.aspx"></asp:TreeNode>
                     <asp:TreeNode Text="Work Manager" Value="Work Manager" NavigateUrl="~/Work Manager Exposure/gettask.aspx"></asp:TreeNode>
                    <asp:TreeNode Text="CoverNote Maintenence" Value="CoverNoteMaintenence" NavigateUrl="~/CoverNoteMaintenance/FindCoverNoteBook.aspx"></asp:TreeNode>
                    <asp:TreeNode Text="New Business" Value="New Business" NavigateUrl="~/New Business/Find Party.aspx"></asp:TreeNode>
                    <asp:TreeNode NavigateUrl="~/MTA/Find Party.aspx" Text="MTA" Value="MTA"></asp:TreeNode>
                    <asp:TreeNode Text="Policy Renewal" Value="Policy Renewal">
                        <asp:TreeNode NavigateUrl="~/PolicyRenewal/wfrmRunRenewalSelection.aspx" Text="Renweal Selection"
                            Value="RenwealSelection"></asp:TreeNode>
                        <asp:TreeNode NavigateUrl="~/PolicyRenewal/wfrmRenewalSelectionByPolicy.aspx" Text="Renewal Selection by Policy"
                            Value="Renewal Selection by Policy"></asp:TreeNode>
                        <asp:TreeNode NavigateUrl="~/PolicyRenewal/wfrmGenerateInvite.aspx" Text="Renewal Invite"
                            Value="Renewal Invite"></asp:TreeNode>
                        <asp:TreeNode NavigateUrl="~/PolicyRenewal/wfrmRenewalAmendment.aspx" Text="Renewal Amendment"
                            Value="Renewal Amendment"></asp:TreeNode>
                    </asp:TreeNode>
                    <asp:TreeNode Text="Client Manager" Value="Client Manager" NavigateUrl="~/Client Manager/Find Party.aspx"></asp:TreeNode>
                     <asp:TreeNode Text="MTC" Value="MTC" NavigateUrl="~/MTC/Find Party.aspx"></asp:TreeNode>
                      <asp:TreeNode Text="MTR" Value="MTR" NavigateUrl="~/MTR/Find Party.aspx"></asp:TreeNode>
                      <asp:TreeNode Text="Cash Deposit Maintenance" Value="Cash Deposit Maintenance" NavigateUrl="~/Cash Deposit/FindCashDepositAccount.aspx"></asp:TreeNode>
                </Nodes>
                <NodeStyle Font-Size="12px" Font-Names="Arial" ForeColor="#BB0D28" HorizontalPadding="5px"
                    NodeSpacing="0px" VerticalPadding="0px" />
            </asp:TreeView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td width="16px">
            <img src="../images/box_btmleft.gif" /></td>
        <td>
            <img src="../images/box_btmmid.gif"></td>
        <td width="16px">
            <img src="../images/box_btmright.gif"></td>
    </tr>
</table>
