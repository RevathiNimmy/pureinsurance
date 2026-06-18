<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="Summary.aspx.vb" Inherits="Nexus.Framework_summary" %>

<%@ Register TagPrefix="OC" Namespace="Nexus" %>
<%@ Register Src="~/Controls/ClaimsProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript">
        function DeclinePayment(sMessage) {
            alert(sMessage);
            window.location = '<%=ResolveClientUrl("~/secure/AuthoriseClaimPayments.aspx")%>';
        }
        function Confirmation(source, msg) {
            $(source).easyconfirm({ locale: { title: 'Confirm', text: msg } });
            $(source).click(function () {
                var result = $(source).attr('tag');
                if (result == true) {
                    $('#<%=hidChkChoice.ClientID%>').val(result);
                }
                else {
                    $('#<%=hidChkChoice.ClientID%>').val(result);
                }
                return true;
            });
        }

        $(document).ready(function () {

            $('.nav-tabs li:first-child a').tab('show');

        });
    </script>

    <div id="Claims_Summary">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="litPageTitle" runat="server" Text="<%$ Resources:Summary_pageheading %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Claim_Summary %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRType" runat="server" Text="<%$ Resources:summary_risktype %>" AssociatedControlID="lblRTypeValue" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblRTypeValue" runat="server"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCType" runat="server" Text="<%$ Resources:summary_claimtype %>" AssociatedControlID="lblCTypeValue" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblCTypeValue" runat="server"></asp:Label>
                            </p>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLDate" runat="server" Text="<%$ Resources:summary_lossdate %>" AssociatedControlID="lblLDateValue" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblLDateValue" runat="server"></asp:Label>
                            </p>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRDate" runat="server" Text="<%$ Resources:summary_reportdate %>" AssociatedControlID="lblRDateValue" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblRDateValue" runat="server"></asp:Label>
                            </p>
                        </div>

                    </div>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddInfo" runat="server" Text="<%$ Resources:summary_addinfo %>" AssociatedControlID="lblAddInfoValue" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblAddInfoValue" runat="server"></asp:Label>
                            </p>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBDesc" runat="server" Text="<%$ Resources:summary_bdesc %>" AssociatedControlID="lblBDescValue" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblBDescValue" runat="server"></asp:Label>
                            </p>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFinalPayment" runat="server" Text="<%$ Resources:Summary_lblFinalpayment %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkFinalPayment" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="litPerils" runat="server" Text="<%$ Resources:litPerils %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <asp:Repeater ID="dlOuter" runat="server">
                    <HeaderTemplate>
                        <div class="md-whiteframe-z0 bg-white">
                            <ul id="Ul1" runat="server" class="nav nav-lines nav-tabs b-danger">
                                <%--<li><a href="#tab-claimsummary">Perils</a></li>--%>
                                <asp:Literal ID="lbltabContent" runat="server"></asp:Literal></a>
                            </ul>
                            <div class="tab-content clearfix p b-t b-t-2x">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="claimsummary" runat="server"></asp:Literal>
                        <div id="tab_claimsummary" runat="server" class="tab-pane animated fadeIn" role="tabpanel">
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblPerilDescription" runat="server"></asp:Label>
                                </legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblSumInsuredHeader" AssociatedControlID="lblSumInsured" runat="server" Text="<%$ Resources:Summary_lblSumInsured %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblSumInsured" runat="server"></asp:Label>
                                        </p>
                                    </div>

                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblReserveTotalHeader" AssociatedControlID="lblReserveTotal" runat="server" Text="<%$ Resources:Summary_lblReserveTotal %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblReserveTotal" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPaidHeader" AssociatedControlID="lblPaid" runat="server" Text="<%$ Resources:Summary_lblPaid %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblPaid" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel runat="server" ID="pnlPerilDetails">
                                <asp:Repeater ID="dlInner" runat="server" OnItemDataBound="dlInner_ItemDataBound">
                                    <HeaderTemplate>
                                        <div class="grid-card table-responsive no-margin">
                                            <table class="grid-table">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            <asp:Literal ID="ltlReserveItem" runat="server" Text="<%$ Resources:Summary_dlInner_lblReserveItem%>"></asp:Literal>
                                                        </th>
                                                        <th class="Doub">
                                                            <asp:Literal ID="ltlReservePrice" runat="server" Text="<%$ Resources:Summary_dlInner_lblReserve %>"></asp:Literal>
                                                        </th>
                                                    </tr>
                                                </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblReserveItem" runat="server"></asp:Label>
                                                </td>
                                                <td class="Doub">
                                                    <asp:Label ID="lblReservePrice" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                                </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </asp:Panel>
                        </div>

                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
        <asp:HiddenField ID="hidChkChoice" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidChlClaimClose" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidChkPaymentMsg" runat="server"></asp:HiddenField>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:UpdatePanel runat="server" ID="updSummary" UpdateMode="Always" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="card-footer">
                    <asp:LinkButton ID="btnBack" runat="server" Text="<%$ Resources:ClaimsResource, claimoverview_btnback %>" Visible="false" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:summary_btnCancel %>" Visible="false" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnDecline" runat="server" Text="<%$ Resources:summary_btnDecline %>" Visible="false" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnFinish" runat="server" Text="<%$ Resources:summary_btnFinish %>" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnRecommend" runat="server" Text="<%$ Resources:summary_btnRecommend %>" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnAuthorise" runat="server" Text="<%$ Resources:summary_btnAuthorise %>" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:ClaimsResource, claimoverview_btnSubmit %>" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upSummary" OverlayCssClass="updating" AssociatedUpdatePanelID="updSummary" runat="server">
            <progresstemplate>
            </progresstemplate>
        </Nexus:ProgressIndicator>
    </div>
</asp:Content>
