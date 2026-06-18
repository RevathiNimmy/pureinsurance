<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BankDetails.ascx.vb"
    Inherits="Nexus.Controls_BankDetails" %>

<script language="javascript" type="text/javascript">
    function ReceiveBankData(sContactData, sPostBackTo) {
        document.getElementById('<%= txtBankDetailData.ClientID %>').value = sContactData;
        __doPostBack(sPostBackTo, 'UpdateBank');
    }

    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
        HideGridMenuInCaseOfNoButtons();
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'btnEdit' || postBackElement.id == "btnBankDelete" || postBackElement.id == "btnSelect") {
            $get(UpBankDetails).style.display = "block";
        }
    }
</script>

<asp:HiddenField ID="txtBankDetailData" runat="server"></asp:HiddenField>
<div id="Controls_BankDetails">
    <div class="md-whiteframe-z0 bg-white">
        <ul class="nav nav-lines nav-tabs b-danger">
            <li><a href="#tab-paymentdetail" data-bs-toggle="tab" aria-expanded="true" class="active">
                <asp:Literal runat="server" Text="<%$ Resources:lbl_tabPaymentDetails %>"></asp:Literal>
            </a></li>
            <li><a href="#tab-history" data-bs-toggle="tab" aria-expanded="true">
                <asp:Literal runat="server" Text="<%$ Resources:lbl_tabHistory %>"></asp:Literal></a></li>
        </ul>
        <div class="tab-content clearfix p b-t b-t-2x">
            <div id="tab-paymentdetail" class="tab-pane animated fadeIn active" role="tabpanel">
                <legend>
                    <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_BankDetails %>"></asp:Label></legend>
                <asp:UpdatePanel runat="server" ID="UpdBankDetails" ChildrenAsTriggers="true" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="grid-card table-responsive no-margin">
                            <asp:GridView ID="grdBankDetails" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="No Data found" PageSize="10" AllowPaging="true" AllowSorting="true" DataKeyNames="Key">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:gvbf_Active %>" SortExpression="IsActive">
                                        <ItemTemplate>
                                            <%#Microsoft.VisualBasic.IIf(Eval("IsActive") = "True", "Yes", "No")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BankPaymentTypeCode" HeaderText="<%$ Resources:gvbf_BankPaymentType %>"></asp:BoundField>
                                    <asp:BoundField DataField="AccountType" HeaderText="<%$ Resources:gvbf_AccountType %>"></asp:BoundField>
                                    <asp:BoundField DataField="AccountHolderName" HeaderText="<%$ Resources:gvbf_AccountHolderName %>"></asp:BoundField>
                                    <asp:BoundField DataField="AccountNumber" HeaderText="<%$ Resources:gvbf_AccountNumber %>"></asp:BoundField>
                                    <asp:BoundField DataField="BranchCode" HeaderText="<%$ Resources:gvbf_BankBranchCode %>"></asp:BoundField>
                                    <asp:BoundField DataField="BIC" HeaderText="<%$ Resources:gvbf_BIC %>"></asp:BoundField>
                                    <asp:BoundField DataField="IBAN" HeaderText="<%$ Resources:gvbf_IBAN %>"></asp:BoundField>
                                    <asp:BoundField DataField="BankBranch" HeaderText="<%$ Resources:gvbf_BankBranch %>"></asp:BoundField>
                                    <asp:BoundField DataField="BankName" HeaderText="<%$ Resources:gvbf_BankName %>"></asp:BoundField>
                                    <asp:BoundField DataField="StreetName" HeaderText="<%$ Resources:gvbf_StreetName %>"></asp:BoundField>
                                    <asp:BoundField DataField="Locality" HeaderText="<%$ Resources:gvbf_Locality %>"></asp:BoundField>
                                    <asp:BoundField DataField="PostTown" HeaderText="<%$ Resources:gvbf_PostTown %>"></asp:BoundField>
                                    <asp:BoundField DataField="County" HeaderText="<%$ Resources:gvbf_County %>"></asp:BoundField>
                                    <asp:BoundField DataField="PostCode" HeaderText="<%$ Resources:gvbf_PostCode %>"></asp:BoundField>
                                    <asp:BoundField DataField="Country" HeaderText="<%$ Resources:gvbf_Country %>"></asp:BoundField>
                                    <asp:BoundField DataField="PartyBankKey" HeaderText="<%$ Resources:gvbf_PartyBankKey %>" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="Key" HeaderText="<%$ Resources:gvbf_Key %>" Visible="false"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol class="list-inline no-margin">
                                                    <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                        <ol id="menu_<%# Eval("PartyBankKey") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                            <li>
                                                                <asp:LinkButton ID="btnSelect" runat="server" Text="<%$ Resources:lkbtn_Select %>" CommandName="Select" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Key")%>'></asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnEdit" runat="server" Text="<%$ Resources:lkbtn_Edit %>" CommandName="Edit" CausesValidation="false"></asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnBankDelete" runat="server" Text="<%$ Resources:lkbtn_Delete %>" CausesValidation="False" CommandName="DeleteRow"></asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnBankActivate" runat="server" Text="<%$ Resources:lkbtn_Deactivate %>" CausesValidation="False" CommandName="Activate"></asp:LinkButton>
                                                            </li>
                                                        </ol>
                                                    </li>
                                                </ol>

                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="UpBankDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdBankDetails" runat="server">
                    <progresstemplate>
                                    </progresstemplate>
                </Nexus:ProgressIndicator>
                <asp:LinkButton ID="hypBank" runat="server" SkinID="btnSM" Text="<%$ Resources:lkbtn_AddBank %>"></asp:LinkButton>
                <asp:LinkButton ID="hypOnlineRegisteredCard" runat="server" SkinID="btnSM" Text="<%$ Resources:lkbtn_OnlineRegisterdCard %>"></asp:LinkButton>
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
                                    <asp:BoundField DataField="ActionDate" HeaderText="<%$ Resources:gvbf_Date %>" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:BoundField DataField="BankName" HeaderText="<%$ Resources:gvbf_BankName %>"></asp:BoundField>
                                    <asp:BoundField DataField="AccountHolderName" HeaderText="<%$ Resources:gvbf_AccountHolderName %>"></asp:BoundField>
                                    <asp:BoundField DataField="BranchCode" HeaderText="<%$ Resources:gvbf_BankBranchCode %>"></asp:BoundField>
                                    <asp:BoundField DataField="BIC" HeaderText="<%$ Resources:gvbf_BIC %>"></asp:BoundField>
                                    <asp:BoundField DataField="IBAN" HeaderText="<%$ Resources:gvbf_IBAN %>"></asp:BoundField>
                                    <asp:BoundField DataField="BankBranch" HeaderText="<%$ Resources:gvbf_BankBranch %>"></asp:BoundField>
                                    <asp:BoundField DataField="AccountNumber" HeaderText="<%$ Resources:gvbf_AccountNumber %>"></asp:BoundField>
                                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:gvbf_User %>"></asp:BoundField>
                                    <asp:BoundField DataField="StreetName" HeaderText="<%$ Resources:gvbf_StreetName %>"></asp:BoundField>
                                    <asp:BoundField DataField="PostCode" HeaderText="<%$ Resources:gvbf_PostCode %>"></asp:BoundField>
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
