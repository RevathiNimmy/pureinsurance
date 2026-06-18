<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="EditInstallment.aspx.vb" Inherits="Nexus.Modal_EditInstallment1" EnableViewState="true"
    EnableEventValidation="true" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="Modal_EditInstallment">
        <asp:HiddenField ID="hvPfInstalmentKey" runat="server"></asp:HiddenField>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal></h1>
            </div>
            <asp:UpdatePanel ID="updStatus" runat="server">
                <ContentTemplate>
                    <div class="card-body clearfix">
                        <div class="form-horizontal">
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblInstalmentNo" runat="server" AssociatedControlID="txtInstalmentNo" Text="<%$ Resources:lbl_InstalmentNo %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtInstalmentNo" runat="server" CssClass="short form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:HiddenField ID="hfPreviousStatus" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="<%$ Resources:lbl_Status %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <NexusProvider:LookupList ID="ddlStatus" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="pfInstalments_status" DefaultText="<%$ Resources:lbl_PleaseSelect %>" CssClass="field-medium field-mandatory form-control form-select"></NexusProvider:LookupList>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus" Display="None" ErrorMessage="<%$ Resources:msgStatus %>" SetFocusOnError="True" InitialValue=""></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvInstalmentStatus" runat="server" Display="None" ErrorMessage="<%$ Resources:msgConfigurationMissing %>" ControlToValidate="ddlStatus"></asp:CustomValidator>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblBatchNo" runat="server" AssociatedControlID="txtBatchNo" Text="<%$ Resources:lbl_BatchNo %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtBatchNo" runat="server" CssClass="short form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblReason" runat="server" AssociatedControlID="txtReason" Text="<%$ Resources:lbl_Reason %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtReason" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" Text="<%$ Resources:lbl_Amount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="short form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblFee" runat="server" AssociatedControlID="txtFee" Text="<%$ Resources:lbl_Fee %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtFee" runat="server" CssClass="short form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblDueDate" runat="server" AssociatedControlID="txtDueDate" Text="<%$ Resources:lbl_DueDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="clDueDate" runat="server" LinkedControl="txtDueDate" HLevel="3"></uc1:CalendarLookup>
                                    </div>
                                </div>
                                   <asp:HiddenField ID="hfPreviousDueDate" runat="server" />
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblPostedDate" runat="server" AssociatedControlID="txtPostedDate" Text="<%$ Resources:lbl_PostedDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtPostedDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox><uc1:CalendarLookup ID="clPostedDate" runat="server" LinkedControl="txtPostedDate" HLevel="3"></uc1:CalendarLookup>
                                    </div>
                                </div>

                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblPaidDate" runat="server" AssociatedControlID="txtPaidDate" Text="<%$ Resources:lbl_PaidDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtPaidDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox><uc1:CalendarLookup ID="clPaidDate" runat="server" LinkedControl="txtPaidDate" HLevel="3" ReadOnly="true"></uc1:CalendarLookup>
                                    </div>
                                </div>

                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTransactionCode" runat="server" AssociatedControlID="txtTransactionCode" Text="<%$ Resources:lbl_TransactionCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtTransactionCode" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="grdvSearchResults" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="noData">
                                <Columns>
                                    <asp:BoundField DataField="FinanceProvider" HeaderText="<%$ Resources:gvbf_PaidDate%>"></asp:BoundField>
                                    <asp:BoundField DataField="PolicyNumber" HeaderText="<%$ Resources:gvbf_Status %>"></asp:BoundField>
                                    <asp:BoundField DataField="PlanReference" HeaderText="<%$ Resources:gvbf_Reason %>"></asp:BoundField>
                                    <asp:BoundField DataField="AccountNo" HeaderText="<%$ Resources:gvbf_Description %>"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="card-footer">
                         <asp:LinkButton ID="btnClose" runat="server" Text="<%$ Resources:btnClose %>" Visible="false" SkinID="btnSecondary"></asp:LinkButton>
                        <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btn_Ok %>" SkinID="btnPrimary"></asp:LinkButton>
                       
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnOk"></asp:PostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_EditInstalment_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
