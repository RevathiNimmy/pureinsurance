<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CreditCardDetails.ascx.vb"
    Inherits="Nexus.Controls_CreditCardDetails" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:HiddenField ID="txtBankDetailData" runat="server"></asp:HiddenField>
<div id="Controls_CreditCardDetails">
    <div class="md-whiteframe-z0 bg-white">
        <ul class="nav nav-lines nav-tabs b-danger">
            <li>
                <a href="#tbcurrent" data-bs-toggle="tab" aria-expanded="true" class="active">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:lbl_tabCurrent %>"></asp:Literal>
                </a>
            </li>
            <li class="">
                <a href="#tbhistory" data-bs-toggle="tab" aria-expanded="true">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:lbl_tabHistory %>"></asp:Literal>
                </a>
            </li>
        </ul>
        <div class="tab-content clearfix p b-t b-t-2x">
            <div id="tbcurrent" class="tab-pane animated fadeIn active" role="tabpanel">
                <div class="card-body no-padding clearfix">
                    <div class="form-horizontal">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblTokenNumber" runat="server" AssociatedControlID="txtTokenNumber" Text="<%$ Resources:lblTokenNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtTokenNumber" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="cvTokenNumber" runat="server" OnServerValidate="cvTokenNumber_ServerValidate" Display="None" ErrorMessage="<%$ Resources:lbl_ErrorMsg_TokenNumber %>"></asp:CustomValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblExistingTokens" runat="server" AssociatedControlID="ddlExistingTokens" Visible="false" Text="<%$ Resources:lbl_ExistingTokens %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlExistingTokens" runat="server" Visible="false" CssClass="field-medium form-control form-select">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <div class="col-md-8 col-sm-9">
                                <asp:CheckBox runat="server" ID="chkCCCancelled" Text="<%$ Resources:chk_Cancelled %>" TextAlign="Right" CssClass="asp-check"></asp:CheckBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="tbhistory" class="tab-pane animated fadeIn" role="tabpanel">
                <div class="card-body no-padding clearfix">
                    <legend>
                        <asp:Label ID="lblHistory" runat="server" Text="<%$ Resources:lbl_History %>"></asp:Label></legend>
                    <asp:UpdatePanel runat="server" ID="updCCHistory" ChildrenAsTriggers="true" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="grid-card table-responsive no-margin">
                                <asp:GridView ID="grdCCHistory" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="No Data found">
                                    <Columns>
                                        <asp:BoundField DataField="ActionCode" HeaderText="<%$ Resources:gvbf_ActionCode %>"></asp:BoundField>
                                        <asp:BoundField DataField="DateModified" HeaderText="<%$ Resources:gvbf_DateModified %>" DataFormatString="{0:d}"></asp:BoundField>
                                        <asp:BoundField DataField="CCNumber" HeaderText="<%$ Resources:gvbf_CCNumber %>"></asp:BoundField>
                                        <asp:BoundField DataField="CCStartDate" HeaderText="<%$ Resources:gvbf_StartDate %>" DataFormatString="{0:d}"></asp:BoundField>
                                        <asp:BoundField DataField="CCExpiry_date" HeaderText="<%$ Resources:gvbf_ExpiryDate %>" DataFormatString="{0:d}"></asp:BoundField>
                                        <asp:BoundField DataField="CardHolderName" HeaderText="<%$ Resources:gvbf_CardHolderName %>"></asp:BoundField>
                                        <asp:BoundField DataField="CardHolderAddress1" HeaderText="<%$ Resources:gvbf_Address1 %>"></asp:BoundField>
                                        <asp:BoundField DataField="CardHolderPostCode" HeaderText="<%$ Resources:gvbf_PostCode %>"></asp:BoundField>
                                        <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:gvbf_User %>"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <Nexus:ProgressIndicator ID="upCCHistory" OverlayCssClass="updating" AssociatedUpdatePanelID="updCCHistory" runat="server">
                        <progresstemplate>
                                </progresstemplate>
                    </Nexus:ProgressIndicator>
                </div>
            </div>
        </div>
    </div>
</div>




