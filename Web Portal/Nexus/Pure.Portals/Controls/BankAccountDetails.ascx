<%@ Control Language="VB" AutoEventWireup="false" CodeFile="~/Controls/BankAccountDetails.ascx.vb"
    Inherits="Nexus.Controls_BankAccountDetails" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {

    });
    function funAccountType() {
        if (document.getElementById('<%=ddlAccountType.ClientID %>').value == '') //
        {
            document.getElementById('<%=txtBankName.ClientID %>').value = '';
            document.getElementById('<%=txtAddressLine1.ClientID %>').value = '';
            document.getElementById('<%=txtBranch.ClientID %>').value = '';
            document.getElementById('<%=txtBranchCode.ClientID %>').value = '';
            document.getElementById('<%=txtAccountName.ClientID %>').value = '';
            document.getElementById('<%=txtAccountNumber.ClientID %>').value = '';
        }
    }
    function ReceiveBankData(sContactData, sPostBackTo) {
        document.getElementById('<%= txtBankDetailData.ClientID %>').value = sContactData;
        SetDropDownInitial();
        __doPostBack(sPostBackTo, 'UpdateBank');
    }

    function SetDropDownInitial() {
        var ddlAccountType = document.getElementById('ctl00_cntMainBody_fpBankDetails_ddlAccountType');
        var length = ddlAccountType.options.length;

        if (parseInt(length) > 2) {
            document.getElementById('ctl00_cntMainBody_fpBankDetails_GISAddress_Country').value = document.getElementById('ctl00_cntMainBody_fpBankDetails_GISAddress_Country').options[0].value;
        }
    }

</script>

<asp:HiddenField ID="txtBankDetailData" runat="server"></asp:HiddenField>
<div id="Controls_BankAccountDetails">
    <asp:HiddenField ID="hvVal" runat="server"></asp:HiddenField>
    <div class="md-whiteframe-z0 bg-white">
        <ul class="nav nav-lines nav-tabs b-danger">
            <li><a href="#tab-current" data-bs-toggle="tab" aria-expanded="true" class="active">
                <asp:Literal runat="server" Text="<%$ Resources:lbl_tabCurrent %>"></asp:Literal>
            </a></li>
            <li><a href="#tab-history" data-bs-toggle="tab" aria-expanded="true">
                <asp:Literal runat="server" Text="<%$ Resources:lbl_tabHistory %>"></asp:Literal></a></li>
        </ul>
        <div class="tab-content clearfix p b-t b-t-2x">
            <div id="tab-current" class="tab-pane animated fadeIn active" role="tabpanel">
                <asp:Panel runat="server" ID="pnlBankDetails">
                    <asp:UpdatePanel ID="upBankDetails" runat="server">
                        <ContentTemplate>
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_BankInformation %>"></asp:Label></legend>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAccountType" runat="server" AssociatedControlID="ddlAccountType" Text="<%$ Resources:lbl_AccountType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:DropDownList ID="ddlAccountType" runat="server" AutoPostBack="true" CssClass="form-control form-select"></asp:DropDownList>
                                    </div>
                                    <asp:CustomValidator ID="custValAccountType" runat="server" ControlToValidate="ddlAccountType" Enabled="false" Display="None" ErrorMessage="<%$ Resources:lbl_ErrorMsg_AccountType %>"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="rfvAccountType" runat="server" ControlToValidate="ddlAccountType" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_AccountType %>" InitialValue="<%$ Resources:lbl_Select_DefaultText %>" Enabled="false"></asp:RequiredFieldValidator>
                                </div>
                                <div id="liEditBank" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:LinkButton ID="hypBank" runat="server" SkinID="btnSM" Text="<%$ Resources:lkbtn_AddBank %>"></asp:LinkButton>
                                    <asp:LinkButton ID="hypBankEdit" runat="server" SkinID="btnSM" Text="<%$ Resources:lkbtn_EditBank %>" Visible="false"></asp:LinkButton>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" Text="<%$ Resources:lbl_BankName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtBankName" runat="server" CssClass="field-extralarge form-control"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvBankName" runat="server" ControlToValidate="txtBankName" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_BankName %>" InitialValue="" Enabled="false"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAddress1" runat="server" AssociatedControlID="txtAddressLine1" Text="<%$ Resources:lbl_Address1 %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtAddressLine1" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvAddress1" runat="server" ControlToValidate="txtAddressLine1" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_Address1 %>" InitialValue="" Enabled="false"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAddress2" runat="server" AssociatedControlID="txtAddressLine2" Text="<%$ Resources:lbl_Address2 %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtAddressLine2" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAddress3" runat="server" AssociatedControlID="txtAddressLine3" Text="<%$ Resources:lbl_Address3 %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtAddressLine3" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAddress4" runat="server" AssociatedControlID="txtAddressLine4" Text="<%$ Resources:lbl_Address4 %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtAddressLine4" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPostCode" runat="server" AssociatedControlID="txtPostCode" Text="<%$ Resources:lbl_PostCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtPostCode" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblCountry" runat="server" AssociatedControlID="GISAddress_Country" Text="<%$ Resources:lbl_Country %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="GISAddress_Country" runat="server" DataItemValue="key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="COUNTRY" DefaultText="(Please Select)" CssClass="form-control form-select"></NexusProvider:LookupList>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPhoneNumber" runat="server" AssociatedControlID="txtPhAreaCode" Text="<%$ Resources:lbl_PhoneNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <div class="row row-xs">
                                            <div class="col-md-3 col-sm-2">
                                                <asp:TextBox ID="txtPhAreaCode" runat="server" CssClass="short form-control" onkeypress="javascript:return isInteger(event);" MaxLength="10"></asp:TextBox>
                                                <asp:Label ID="lblAreaCode" runat="server" Font-Bold="true" Font-Size="X-Small" Text="<%$ Resources:lbl_AreaCode %>"></asp:Label>
                                            </div>
                                            <div class="col-md-7 col-sm-5">
                                                <asp:TextBox ID="txtPhNumber" runat="server" CssClass="form-control" onkeypress="javascript:return isInteger(event);" MaxLength="20"></asp:TextBox>
                                                <asp:Label ID="lblNumber" runat="server" Font-Bold="true" Font-Size="X-Small" Text="<%$ Resources:lbl_Number %>"></asp:Label>
                                            </div>
                                            <div class="col-md-2 col-sm-2">
                                                <asp:TextBox ID="txtPhExt" runat="server" CssClass="form-control" onkeypress="javascript:return isInteger(event);" MaxLength="10"></asp:TextBox>
                                                <asp:Label ID="lblExtension" runat="server" Font-Bold="true" Font-Size="X-Small" Text="<%$ Resources:lbl_Extension %>"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblFaxNumber" runat="server" AssociatedControlID="txtFaxAreaCode" Text="<%$ Resources:lbl_FaxNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <div class="row row-xs">
                                            <div class="col-md-3 col-sm-2">
                                                <asp:TextBox ID="txtFaxAreaCode" runat="server" CssClass="short form-control" onkeypress="javascript:return isInteger(event);" MaxLength="10"></asp:TextBox>
                                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="X-Small" Text="<%$ Resources:lbl_AreaCode %>"></asp:Label>
                                            </div>
                                            <div class="col-md-9 col-sm-10">
                                                <asp:TextBox ID="txtFaxNumber" runat="server" CssClass="form-control" onkeypress="javascript:return isInteger(event);" MaxLength="20"></asp:TextBox>
                                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Size="X-Small" Text="<%$ Resources:lbl_Number %>"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-horizontal">
                                <legend>Account Information</legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblBranch" runat="server" AssociatedControlID="txtBranch" Text="<%$ Resources:lbl_Branch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblBranchCode" runat="server" AssociatedControlID="txtBranchCode" Text="<%$ Resources:lbl_BranchCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtBranchCode" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" Text="<%$ Resources:lbl_AccountName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtAccountName" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvAccountName" runat="server" ControlToValidate="txtAccountName" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_AccountName %>"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" Text="<%$ Resources:lbl_AccountNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtAccountNumber" runat="server" onkeypress="javascript:return isInteger(event);" MaxLength="20" CssClass="field-mandatory form-control"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvAccountNumber" runat="server" ControlToValidate="txtAccountNumber" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_AccountNumber %>"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblDateChanged" runat="server" AssociatedControlID="txtDateChanged" Text="<%$ Resources:lbl_DateChanged %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtDateChanged" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calDateChanged" runat="server" LinkedControl="txtDateChanged" HLevel="4"></uc1:CalendarLookup>
                                        </div>
                                    </div>

                                </div>

                                <div class="li-margin form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <label class="col-md-4 col-sm-3 control-label"></label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:CheckBox runat="server" ID="chkDDCancelled" Text="<%$ Resources:chk_DDCancelled %>" TextAlign="Right" CssClass="asp-check"></asp:CheckBox>
                                        <asp:CheckBox runat="server" ID="chkPaperDD" Text="<%$ Resources:chk_PaperDD %>" TextAlign="Right" CssClass="asp-check"></asp:CheckBox>
                                    </div>
                                </div>

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <Nexus:ProgressIndicator ID="piBankDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="upBankDetails" runat="server">
                        <progresstemplate>
                            </progresstemplate>
                    </Nexus:ProgressIndicator>
                </asp:Panel>
            </div>
            <div id="tab-history" class="tab-pane animated fadeIn" role="tabpanel">
                <legend>
                    <asp:Label ID="lblHistory" runat="server" Text="<%$ Resources:lbl_History %>"></asp:Label></legend>
                <asp:UpdatePanel runat="server" ID="updBankHistory" ChildrenAsTriggers="true" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="grid-card table-responsive no-margin">
                            <asp:GridView ID="grdBankHistory" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="No Data found">
                                <Columns>
                                    <asp:BoundField DataField="ActionCode" HeaderText="<%$ Resources:gvbf_ActionCode %>"></asp:BoundField>
                                    <%--ActionDate--%>
                                    <asp:BoundField DataField="DateModified" HeaderText="<%$ Resources:gvbf_Date %>" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:BoundField DataField="PaymentType" HeaderText="<%$ Resources:gvbf_PaymentType %>"></asp:BoundField>
                                    <asp:BoundField DataField="AccountType" HeaderText="<%$ Resources:gvbf_AccountType %>"></asp:BoundField>
                                    <asp:BoundField DataField="BankName" HeaderText="<%$ Resources:gvbf_BankName %>"></asp:BoundField>
                                    <asp:BoundField DataField="BankBranch" HeaderText="<%$ Resources:gvbf_BankBranch %>"></asp:BoundField>
                                    <%--AccountHolderName--%>
                                    <asp:BoundField DataField="BankAccountName" HeaderText="<%$ Resources:gvbf_AccountHolderName %>"></asp:BoundField>
                                    <%--AccountCode--%><asp:BoundField DataField="BankSortCode" HeaderText="<%$ Resources:gvbf_BankAccountCode %>"></asp:BoundField>
                                    <asp:BoundField DataField="BankAccountNo" HeaderText="<%$ Resources:gvbf_AccountNumber %>"></asp:BoundField>
                                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:gvbf_User %>"></asp:BoundField>
                                    <asp:BoundField DataField="BankAddress1" HeaderText="<%$ Resources:gvbf_Address1 %>"></asp:BoundField>
                                    <asp:BoundField DataField="BankPostCode" HeaderText="<%$ Resources:gvbf_PostCode %>"></asp:BoundField>
                                    <asp:BoundField DataField="PaperDD" HeaderText="<%$ Resources:gvbf_PaperDD %>"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="upBankHistory" OverlayCssClass="updating" AssociatedUpdatePanelID="updBankHistory" runat="server">
                    <progresstemplate>
                                </progresstemplate>
                </Nexus:ProgressIndicator>
            </div>
        </div>
    </div>
</div>
