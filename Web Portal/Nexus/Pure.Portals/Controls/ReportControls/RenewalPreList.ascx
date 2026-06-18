<%@ control language="VB" autoeventwireup="false" inherits="Nexus.Controls_ReportControls_RenewalPreList" Codefile="RenewalPreList.ascx.vb"%>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>


<script language="javascript" type="text/javascript">
   
</script>

<div id="Controls_ReportControls_RenewalPreList">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
				 
				  	<div id="liProduct" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblProduct" runat="server" AssociatedControlID="RP__product_code" Text="<%$ Resources:lbl_Product%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__product_code" runat="server" CssClass="field-medium form-control">
                        </asp:DropDownList>
						
                    </div>
                </div>
				 
					<div id="liBranch" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblBatch" runat="server" AssociatedControlID="RP__BRANCH_ID" Text="<%$ Resources:lbl_Batch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__BRANCH_ID" runat="server" CssClass="field-medium form-control">
                        </asp:DropDownList>
						
                    </div>
                </div>
                 <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblPeriodEndDate" runat="server" AssociatedControlID="RP__END_DATE" Text="<%$ Resources:lbl_PeriodEndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__END_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calPeriodEndDate" runat="server" LinkedControl="RP__END_DATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldPeriodEndDate" Display="None" ControlToValidate="RP__END_DATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_PeriodEndDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldPeriodEndDate" runat="server" Display="None" ControlToValidate="RP__END_DATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_PeriodEndDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldPeriodEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_PeriodEndDate %>" ControlToValidate="RP__END_DATE" Display="None" ValidationGroup="vldReportsControlsGroup">
                    </asp:RangeValidator>
                </div>
                
                 <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblSort" runat="server" AssociatedControlID="RP__SORTORDER" Text="<%$ Resources:lbl_Sort%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__SORTORDER" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_Client %>" Value="Client"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PolicyNumber %>" Value="Policy Number"></asp:ListItem>  
                            <asp:ListItem Text="<%$ Resources:li_Agent %>" Value="Agent"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CoverToDate %>" Value="Cover To Date "></asp:ListItem> 
                            <asp:ListItem Text="<%$ Resources:li_ProductID%>" Value="Product ID "></asp:ListItem> 
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style="display: none;">
                    <asp:Label ID="lblBranch" runat="server" AssociatedControlID="RP__BRANCH" Text="<%$ Resources:lbl_Branch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="RP__BRANCH" runat="server" CssClass="field-medium form-control">                           
                        </asp:TextBox>
                    </div>
                </div>
				
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>
</div>