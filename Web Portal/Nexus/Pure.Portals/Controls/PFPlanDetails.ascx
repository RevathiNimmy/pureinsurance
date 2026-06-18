<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PFPlanDetails.ascx.vb"
    Inherits="Nexus.Controls_PFPlanDetails" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>


<script type="text/javascript">
    $(document).ready(function () {
     
        if (document.getElementById('<%=hvMediaTypeCode.ClientId %>').value.toString().trim() == 'DD') {
           
            document.getElementById('liBankDetails').style.display = 'block';
            document.getElementById('liCCDetails').style.display = 'none';

        }
        else if (document.getElementById('<%=hvMediaTypeCode.ClientId %>').value.toString().trim() == 'CA')
        {
            document.getElementById('liBankDetails').style.display = 'none';
            document.getElementById('liCCDetails').style.display = 'none';
        }
        else {

            document.getElementById('liBankDetails').style.display = 'none';
            document.getElementById('liCCDetails').style.display = 'block';
        }

    });
</script>
<asp:PlaceHolder ID="PnlPFPlanDetails" runat="server">
    <div id="Controls_PFPlanDetails">
        <div class="card-body no-padding clearfix">
            <asp:HiddenField ID="hvMediaTypeCode" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hdnIsSuppressDecimals" runat="server"></asp:HiddenField>

            
            <h5>
                <asp:Label ID="lblSchemeName" runat="server"></asp:Label></h5>
            <div class="form-horizontal">
                <legend>Summary</legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="<%$ Resources:lbl_Status %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Value="010" Text="Saved"></asp:ListItem>
                            <asp:ListItem Value="011" Text="Updated"></asp:ListItem>
                            <asp:ListItem Value="012" Text="Quote Printed"></asp:ListItem>
                            <asp:ListItem Value="040" Text="Live"></asp:ListItem>
                            <asp:ListItem Value="140" Text="On Hold"></asp:ListItem>
                            <asp:ListItem Value="900" Text="Completed"></asp:ListItem>
                            <asp:ListItem Value="990" Text="Superseded"></asp:ListItem>
                            <asp:ListItem Value="999" Text="Cancelled"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:RequiredFieldValidator ID="RqdDd1Status" runat="server" ControlToValidate="ddlStatus" Display="none" Enabled="true" ValidationGroup="" SetFocusOnError="true" ErrorMessage="<%$ Resources:Status_ErrorMessage%>"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:LinkButton ID="btnTrans" runat="server" Text="<%$ Resources:btn_Trans%>" CausesValidation="false" SkinID="btnSM" AssociatedControlID="ddlStatus"></asp:LinkButton>
                    <asp:LinkButton ID="btnHistory" runat="server" Text="<%$ Resources:btn_History%>" CausesValidation="false" SkinID="btnSM" AssociatedControlID="ddlStatus"></asp:LinkButton>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateCreated" runat="server" AssociatedControlID="txtDateCreated" Text="<%$ Resources:lbl_Date_Created %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtDateCreated" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="clDateCreated" runat="server" LinkedControl="txtDateCreated" HLevel="2"></uc1:CalendarLookup>
                        </div>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblCancelReason" runat="server" AssociatedControlID="ddlCancelReason" Text="<%$ Resources:lbl_Cancel_Reason %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <NexusProvider:LookupList ID="ddlCancelReason" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="PFPREMIUMFINANCE_CANCEL_REASON" DefaultText="<%$ Resources:lbl_None %>" CssClass="field-medium form-control form-select" AutoPostBack="true"></NexusProvider:LookupList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblModified" runat="server" AssociatedControlID="txtModified" Text="<%$ Resources:lbl_Date_Modified %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtModified" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="clDateModfied" runat="server" LinkedControl="txtModified" HLevel="2"></uc1:CalendarLookup>
                        </div>
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblStartDate" runat="server" AssociatedControlID="txtStartDate" Text="<%$ Resources:lbl_Start_Date %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="clCoverStartDate" runat="server" LinkedControl="txtStartDate" HLevel="2"></uc1:CalendarLookup>
                        </div>
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDaysDelay" runat="server" AssociatedControlID="txtDaysDelay" Text="<%$ Resources:lbl_Days_Delay %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtDaysDelay" runat="server" CssClass="Doub form-control" onkeypress="javascript:return isInteger(event);" MaxLength="3"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblOriginalDebt" runat="server" AssociatedControlID="txtOriginalDebt" Text="<%$ Resources:lbl_Original_Debt %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtOriginalDebt" runat="server" CssClass="Doub e-num2 form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblInstalments" runat="server" AssociatedControlID="txtInstalments" Text="<%$ Resources:lbl_Instalments %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtInstalments" runat="server" CssClass="Doub form-control" onkeypress="javascript:return isInteger(event);" MaxLength="2"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblFinanceAmount" runat="server" AssociatedControlID="txtFinanceAmount" Text="<%$ Resources:lbl_Finance_Amount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtFinanceAmount" runat="server" CssClass="Doub e-num2 form-control" onkeypress="javascript:return isInteger(event);" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblReference" runat="server" AssociatedControlID="txtReference" Text="<%$ Resources:lbl_Reference %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtReference" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="vldClientCodeRequired" runat="server" Display="none" ControlToValidate="txtReference" ErrorMessage="<%$ Resources:Ref_ErrorMessage %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-horizontal">
                <legend>Breakdown</legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblInterestRate" runat="server" AssociatedControlID="txtInterestRate" Text="<%$ Resources:lbl_Interest_Rate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtInterestRate" runat="server" CssClass="Doub e-num2 form-control" onkeypress="javascript:return isInteger(event);" MaxLength="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAPR" runat="server" AssociatedControlID="txtAPR" Text="<%$ Resources:lbl_APR %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtAPR" runat="server" CssClass="Doub e-num2 form-control" onkeypress="javascript:return isInteger(event);" MaxLength="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDeposit" runat="server" AssociatedControlID="txtDeposit" Text="<%$ Resources:lbl_Deposit %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtDeposit" runat="server" CssClass="Doub e-num2 form-control" onkeypress="javascript:return isInteger(event);" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lbl_FinanceCharge" runat="server" AssociatedControlID="txtFinanceCharge" Text="<%$ Resources:lbl_Finance_Charge %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtFinanceCharge" runat="server" CssClass="Doub e-num2 form-control" onkeypress="javascript:return isInteger(event);" MaxLength="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblProtection" runat="server" AssociatedControlID="txtProtection" Text="<%$ Resources:lbl_Protection %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtProtection" runat="server" CssClass="Doub e-num2 form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTaxes" runat="server" AssociatedControlID="txtTaxes" Text="<%$ Resources:lbl_Taxes %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtTaxes" runat="server" CssClass="Doub e-num2 form-control" onkeypress="javascript:return isInteger(event);" MaxLength="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblFirstInstalmentDate" runat="server" AssociatedControlID="txtFirstInstalmentDate" Text="<%$ Resources:lbl_First_Instalment_Date %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtFirstInstalmentDate" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="clFirstInstalmentDate" runat="server" LinkedControl="txtFirstInstalmentDate" HLevel="2"></uc1:CalendarLookup>
                        </div>
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblFirstInstalment" runat="server" AssociatedControlID="txtFirstInstalment" Text="<%$ Resources:lbl_First_Instalment %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtFirstInstalment" runat="server" CssClass="Doub e-num2 form-control" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblNextInstalmentDate" runat="server" AssociatedControlID="txtNextInstalmentDate" Text="<%$ Resources:lbl_Next_Instalment_Date %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtNextInstalmentDate" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="clNextInstalmentDate" runat="server" LinkedControl="txtNextInstalmentDate" HLevel="2"></uc1:CalendarLookup>
                        </div>
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblOtherInstalments" runat="server" AssociatedControlID="txtOtherInstalments" Text="<%$ Resources:lbl_Other_Instalments %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtOtherInstalments" runat="server" CssClass="Doub e-num2 form-control" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblLastInstalmentDate" runat="server" AssociatedControlID="txtLastInstalmentDate" Text="<%$ Resources:lbl_Last_Instalment_Date %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtLastInstalmentDate" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="clLastInstalmentDate" runat="server" LinkedControl="txtLastInstalmentDate" HLevel="2"></uc1:CalendarLookup>
                        </div>
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTotalAmount" runat="server" AssociatedControlID="txtTotalAmount" Text="<%$ Resources:lbl_Total_Amount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="Doub e-num2 form-control" onkeypress="javascript:return isInteger(event);" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:PlaceHolder>

