<%@ control language="VB" autoeventwireup="false" inherits="Nexus.Controls_ReportControls_PolicyListingShort" Codefile="PolicyListingShort.ascx.vb"%>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>


<script language="javascript" type="text/javascript">
   
</script>

<div id="Controls_ReportControls_PolicyListingShort">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
				 
				  <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblSort" runat="server" AssociatedControlID="RP__SORTORDER" Text="<%$ Resources:lbl_Sort %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__SORTORDER" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_Client%>" Value="Client"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Policy %>" Value="Policy"></asp:ListItem>  
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
				
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>
</div>