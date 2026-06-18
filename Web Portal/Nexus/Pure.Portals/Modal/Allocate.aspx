<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Allocate.aspx.vb" Inherits="Nexus.Modal_Allocate"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">


        function ShowOverAllocationMsg() {
            var isOverAllocatedAmt = 0;
            $(".grid-card table tbody tr").each(function () {
                var outStandingAmt = parseFloat($(this).find("td").eq(3).text().trim().replace(/[$,]+/g, "")).toFixed(2);
                var allocatedAmt = parseFloat($(this).find('input').val().trim().replace(/[$,]+/g, "")).toFixed(2);
                if (allocatedAmt > 0 && parseFloat(allocatedAmt) > parseFloat(outStandingAmt)) {
                    isOverAllocatedAmt = 1;
                }
            });

            if (Page_IsValid) {
                if (isOverAllocatedAmt == 1) {
                    var overAllocationMessage = '<%= GetLocalResourceObject("lbl_OverAllocation")%>';
                    alert(overAllocationMessage);
                }
            }

        }
        function SelectWriteOffreason(oSrc, args) {//if any 'write off entry' in the grid than make 'write off reason' mandatory
            var cntMainBody = "ctl00_cntMainBody_";
            var grid = document.getElementById(cntMainBody + "gvAllocate"); //selecting allocate grid
            var inputs = grid.getElementsByTagName("input"); //Retrieve all the input elements from the grid
            var iCountVar = 0;
            for (var i = 0; i < inputs.length; i += 1)//loop for all the inputs in the grid
            {
                if (inputs[i].value != "")// if any "write off" text box in the grid has any entry
                {
                    iCountVar = iCountVar + 1; //increase counter variable
                }
            }

            if (iCountVar >= 2) {
                args.IsValid = false; //if more than 1 "write off" text box has entry than show eeror message
            }
            else {
                args.IsValid = true;
            }
        }

        function SelectWriteOFFReasonSelect(oSrc, args) {
            var cntMainBody = "ctl00_cntMainBody_";
            var grid = document.getElementById(cntMainBody + "gvAllocate");
            var inputs = grid.getElementsByTagName("input");
            var SelWriteoffReason = document.getElementById(cntMainBody + "writeOffReason").value;
            var iCountVar = 0;
            for (var i = 0; i < inputs.length; i += 1) {
                if (inputs[i].value != "") {
                    iCountVar = iCountVar + 1;
                    //finding out the numeber of values entered
                }
            }
            if (iCountVar == 1)//if the Count =1 then Check for Write Off Reason
            {
                if (SelWriteoffReason == "")//Display Message Please select write Off Reason
                {
                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }
            }
        }

        function CheckWriteOffLimit(oSrc, args) {//if any 'write off entry' in the grid than make 'write off reason' mandatory
            var cntMainBody = "ctl00_cntMainBody_";
            var grid = document.getElementById(cntMainBody + "gvAllocate"); //selecting allocate grid
            var inputs = grid.getElementsByTagName("input"); //Retrieve all the input elements from the grid
            var iCountVar = 0;
            var Writeoffvalue = 0;
            var writeofflimit = document.getElementById(cntMainBody + "hidden_limitwriteOffamount").value;
            var SelWriteoffReason = document.getElementById(cntMainBody + "writeOffReason").value;

            //Get WriteOff Limit From the System option
            for (var i = 0; i < inputs.length; i += 1)//loop for all the inputs in the grid
            {
                if (inputs[i].value != "")// if any "write off" text box in the grid has any entry
                {
                    iCountVar = iCountVar + 1; //increase counter variable
                    Writeoffvalue = inputs[i].value
                }
                if (SelWriteoffReason != "") {
                    if (Number(Writeoffvalue) > Number(writeofflimit)) {
                        args.IsValid = false; //Writeoff value is greater than than the write off limit than give error
                    }
                    else {
                        args.IsValid = true;
                    }
                }
            }
        }

        function ValidateTextBox(oSrc, args) {
            var cntMainBody = "ctl00_cntMainBody_";
            var grid = document.getElementById(cntMainBody + "gvAllocate"); //selecting allocate grid
            var inputs = grid.getElementsByTagName("input"); //Retrieve all the input elements from the grid
            var iCountVar = 0;

            for (var i = 0; i < inputs.length; i += 1)//loop for all the inputs in the grid
            {
                if (inputs[i].value != "")// if any "write off" text box in the grid has any entry
                {
                    if (isNaN(inputs[i].value)) {
                        args.IsValid = false;
                    }
                    else {
                        args.IsValid = true;
                    }
                }
            }
        }

        function pageLoad() {
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
        }

        function OnBeginRequest(sender, args) {
            var postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'lnkbtnSelect') {
                $get(uprogQuotes).style.display = "block";
            }
        }
        //Error message is displayed when more then SPR/SPY is selected.
        function ValidateSelectedEntries(oSrc, args) {
            if (document.getElementById('<%= hfNumberOfSelectedCashEntry.ClientId %>').value > 1 && document.getElementById('<%= hfNumberOfSelectedOtherEntry.ClientId %>').value >= 1) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="Modal_Allocate">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <legend>
                    <asp:Label ID="lblBaseCurrency" runat="server" Text="<%$ Resources:lbl_BaseCurrency %>"></asp:Label></legend>
                <asp:UpdatePanel ID="updAllocate" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="gvAllocate" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" ShowFooter="true" EmptyDataText="<%$ Resources:ErrorMessage %>" FooterStyle-HorizontalAlign="Right">
                                <Columns>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_TransDetailKey %>" DataField="TransDetailKey"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_DocumnetRef %>" DataField="DocRef" FooterText="Total"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_Type %>" DataField="MediaType"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_TaxBand %>" DataField="TaxBand"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_TransactionCurrency %>" DataField="TransactionCurrency"></asp:BoundField>
                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_Amount %>" DataField="TransactionCurrencyAmount" DataType="Currency"></Nexus:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_BaseCurrency %>" DataField="Currency"></asp:BoundField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_Outstandingamount %>" ItemStyle-CssClass="GridAmtCol" FooterStyle-CssClass="GridAmtCol"
                                        HeaderStyle-CssClass="GridAmtCol" ControlStyle-CssClass="GridAmtCol">
                                        <ItemTemplate>
                                            <div style="text-align: right!important; min-width: 100px; max-width: 200px; padding-right: 12px;">
                                                <asp:Label ID="lblOSAmount" runat="server" Text='<%# Eval("OutstandingAmount","{0:N2}")%>' />
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div style="text-align: right!important; min-width: 100px; max-width: 200px; padding-right: 12px;">
                                                <asp:Label ID="lblOSAmountTotal" runat="server" />
                                            </div>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_Allocated %>" ItemStyle-CssClass="GridAmtCol" FooterStyle-CssClass="GridAmtCol"
                                        HeaderStyle-CssClass="GridAmtCol" ControlStyle-CssClass="GridAmtCol">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAllocated" runat="server" Enabled="true" Visible="true" CssClass="FormatDecimal Doub form-control form-control-textbox"
                                                AutoCompleteType="None" OnTextChanged="txtAllocated_TextChanged" onClick="this.select();" AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div style="text-align: right!important; min-width: 100px; max-width: 200px; padding-right: 12px;">
                                                <asp:Label ID="lblAllocatedTotal" runat="server" />
                                            </div>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_WriteOff %>" ItemStyle-CssClass="GridAmtCol" FooterStyle-CssClass="GridAmtCol"
                                        HeaderStyle-CssClass="GridAmtCol" ControlStyle-CssClass="GridAmtCol">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWriteOff" runat="server" Enabled="true" Visible="true" CssClass="FormatDecimal Doub form-control form-control-textbox"
                                                AutoCompleteType="None" OnTextChanged="txtWriteOff_TextChanged" onClick="this.select();" AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div style="text-align: right!important; min-width: 100px; max-width: 200px; padding-right: 12px;">
                                                <asp:Label ID="lblWriteOffTotal" runat="server" CssClass="FormatDecimal DoubLabel" />
                                            </div>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_CurrencyDiff %>" ItemStyle-CssClass="GridAmtCol" FooterStyle-CssClass="GridAmtCol"
                                        HeaderStyle-CssClass="GridAmtCol" ControlStyle-CssClass="GridAmtCol">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCurrencyDiff" runat="server" Enabled="true" Visible="true" CssClass="FormatDecimal Doub form-control form-control-textbox"
                                                AutoCompleteType="None" ondblclick="txtCurrencyDiff_DoubleClick" OnTextChanged="txtCurrencyDiff_TextChanged" onClick="this.select();" AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div style="text-align: right!important; min-width: 100px; max-width: 200px; padding-right: 12px;">
                                                <asp:Label ID="lblCurrencyDiffTotal" runat="server" CssClass="FormatDecimal DoubLabel" />
                                            </div>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id="menu_<%# Eval("TransDetailKey") %>" class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton ID="lnkbtnSelect" runat="server" CausesValidation="False" SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="hfNumberOfSelectedCashEntry" runat="server" Value="0"></asp:HiddenField>
                            <asp:HiddenField ID="hfNumberOfSelectedOtherEntry" runat="server" Value="0"></asp:HiddenField>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="upAllocate" OverlayCssClass="updating" AssociatedUpdatePanelID="updAllocate" runat="server">
                    <progresstemplate>
                    </progresstemplate>
                </Nexus:ProgressIndicator>

            </div>
            <div class="form-horizontal">
                <asp:UpdatePanel ID="updTotals" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container pt-3">
                            <div class="col-12">
                                <div class="row">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-1">
                                        <asp:Label ID="lblTotal" runat="server" AssociatedControlID="txtTotals" class="col-md-4 col-sm-3 control-label">
                                            <asp:Literal ID="ltTotal" runat="server" Text="<%$ Resources:lbl_Totals%>" />
                                        </asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtTotals" runat="server" CssClass="form-control" Enabled="false" Text="0.00" />
                                    </div>
                                    <div class="col-md-3"></div>
                                    <div class="col-md-1">
                                        <asp:Label ID="lblBalanceAmount" runat="server" AssociatedControlID="txtBalanceAmount" class="col-md-4 col-sm-3 control-label">
                                            <asp:Literal ID="ltBalanceAmount" runat="server" Text="<%$ Resources:lbl_Balance%>"></asp:Literal>
                                        </asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtBalanceAmount" runat="server" CssClass="form-control" Enabled="false" Text="0.00"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                            </div>

                        </div>
                        <div class="container pt-3">
                            <div class="col-12">
                                <div class="row">
                                   <div class="col-md-1"></div>
                                    <div id="liWriteOff" runat="server" class="col-md-2 text-end">
                                        <asp:Label ID="lblwriteOffReason" runat="server" AssociatedControlID="writeOffReason" style="font-size: 12px">
                                            <asp:Literal ID="litBranchCode" runat="server" Text="<%$ Resources:lbl_writeOffReason %>"></asp:Literal>
                                        </asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <NexusProvider:LookupList ID="writeOffReason" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="pmlookup" ListCode="Write_off_Reason" CssClass="field-medium form-control form-select" DefaultText="(n/a)"></NexusProvider:LookupList>
                                    </div>
                                     <div class="col-md-5"></div>
                                    
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Visible="false" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary" OnClientClick="ShowOverAllocationMsg();"></asp:LinkButton>
                <asp:LinkButton ID="btnAdd" runat="server" Visible="false" TabIndex="4" Text="<%$ Resources:btnAdd %>" CausesValidation="false" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <input type="hidden" id="hidden_limitwriteOffamount" runat="server" />
        <input type="hidden" id="hidden_WriteOffUserAuthority" runat="server" />
        <input type="hidden" id="hidden_WriteOffCurrency" runat="server" />
        <asp:HiddenField ID="hvKeyToRemove" runat="server" />
        <input type="hidden" id="hidden_CurrencyDiff" runat="server" />
        <asp:CustomValidator ID="custvldWriteoffAuthority" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_WriteOff_Limit %>" Enabled="true"></asp:CustomValidator>
        <asp:CustomValidator ID="custvldWriteOffOnlyOne" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_WriteOff_OnlyOne %>" Enabled="true"></asp:CustomValidator>
        <asp:CustomValidator ID="custvldWriteoffLimit" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_WriteOff_Limit %>" Enabled="true"></asp:CustomValidator>
        <asp:CustomValidator ID="custvldWriteoffReason" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_WriteOff_Reasonselect %>" Enabled="true"></asp:CustomValidator>
        <asp:CustomValidator ID="custvldValidateBalance" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_ValidateBalance %>" Enabled="true"></asp:CustomValidator>
        <asp:CustomValidator ID="custVldValidateInputNumbers" runat="server" Display="None" ClientValidationFunction="ValidateTextBox" ErrorMessage="<%$ Resources:lbl_validNumber %>" Enabled="false"></asp:CustomValidator>
        <asp:CustomValidator ID="cstvalidAccount" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_validAccount %>" Enabled="false"></asp:CustomValidator>
        <asp:CustomValidator ID="cvSingleSRPnSPY" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:lbl_SingleSRPSPYError %>" ClientValidationFunction="ValidateSelectedEntries" Enabled="false"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
