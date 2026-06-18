<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClaimReinsurance2007.ascx.vb" Inherits="Nexus.Controls_ClaimReinsurance2007" %>
<div id="Controls_ClaimReinsurance2007">

    <script type="text/javascript" language="javascript">
        function ShowMsg(sMessage, iRIArrangementLineKey) {
            if (iRIArrangementLineKey == "null") {
                alert(sMessage)
                return false;
            }
            else {
                alert(sMessage)
                window.location = '<%=ResolveClientUrl("~/Modal/RIBrokerParticipant.aspx") %>?Mode=FACPROP&VMode=VIEW&RIArrangementLineKey=' + iRIArrangementLineKey;
            }
        }

        var dialogPaymentConfirmed = false;
        function PaymentConfirmation(obj, dialogText) {
            if (!dialogPaymentConfirmed) {
                BootstrapDialog.show({
                    title: 'Confirm',
                    closable: false,
                    type: BootstrapDialog.TYPE_WARNING,
                    message: dialogText,
                    buttons: [{
                        id: 'btn-No',
                        icon: 'fa fa-close',
                        label: 'No',
                        cssClass: 'btn btn-sm btn-dark',
                        autospin: false,
                        action: function (dialogRef) {
                            dialogRef.close();
                            dialogPaymentConfirmed = true;
                            var sMsgAnotherPayment = $('#<%= hfAnotherPaymentMessage.ClientID %>').val();
                            $('#<%=hidChkChoice.ClientID%>').val('false');
                            if (obj) obj.click();
                        }
                    },
                {
                    id: 'btn-Yes',
                    icon: 'fa fa-check',
                    label: 'Yes',
                    cssClass: 'btn btn-sm btn-primary',
                    autospin: false,
                    action: function (dialogRef) {
                        dialogRef.close();
                        dialogPaymentConfirmed = true;
                        $('#<%=hidChkChoice.ClientID%>').val('true');
                        if (obj) obj.click();
                    }
                }]
                });
        }
        return dialogPaymentConfirmed;
    }

    var dialogConfirmed = false;
    function ClaimCloseConfirmation(obj, dialogText) {
        if (!dialogConfirmed) {
            BootstrapDialog.show({
                title: 'Confirm',
                closable: false,
                type: BootstrapDialog.TYPE_WARNING,
                message: dialogText,
                buttons: [{
                    id: 'btn-No',
                    icon: 'fa fa-close',
                    label: 'No',
                    cssClass: 'btn btn-sm btn-dark',
                    autospin: false,
                    action: function (dialogRef) {
                        dialogRef.close();
                        var sMsgAnotherPayment = $('#<%= hfAnotherPaymentMessage.ClientID %>').val();
                        $('#<%=hidChlClaimClose.ClientID%>').val('false');
                        BootstrapDialog.show({
                            title: 'Confirm',
                            closable: false,
                            type: BootstrapDialog.TYPE_WARNING,
                            message: sMsgAnotherPayment,
                            buttons: [{
                                id: 'btn-No',
                                icon: 'fa fa-close',
                                label: 'No',
                                cssClass: 'btn btn-sm btn-dark',
                                autospin: false,
                                action: function (dialogRef) {
                                    dialogRef.close();
                                    dialogConfirmed = true;
                                    $('#<%=hidChkChoice.ClientID%>').val('false');
                                        if (obj) obj.click();
                                    }
                                },
                                    {
                                        id: 'btn-Yes',
                                        icon: 'fa fa-check',
                                        label: 'Yes',
                                        cssClass: 'btn btn-sm btn-primary',
                                        autospin: false,
                                        action: function (dialogRef) {
                                            dialogRef.close();
                                            dialogConfirmed = true;
                                            $('#<%=hidChkChoice.ClientID%>').val('true');
                                            if (obj) obj.click();
                                        }
                                    }]
                            });
                    }
                },
                                    {
                                        id: 'btn-Yes',
                                        icon: 'fa fa-check',
                                        label: 'Yes',
                                        cssClass: 'btn btn-sm btn-primary',
                                        autospin: false,
                                        action: function (dialogRef) {
                                            dialogRef.close();
                                            dialogConfirmed = true;
                                            $('#<%=hidChlClaimClose.ClientID%>').val('true');
                                            if (obj) obj.click();
                                        }
                                    }]
            })
                            }
                            return dialogConfirmed;
                        }

                        var dialogThirdpartyConfirmed = false;
                        function RecoveriesConfirmation(obj, dialogText) {

                            if (!dialogThirdpartyConfirmed) {
                                BootstrapDialog.show({
                                    title: 'Confirm',
                                    closable: false,
                                    type: BootstrapDialog.TYPE_WARNING,
                                    message: dialogText,
                                    buttons: [{
                                        id: 'btn-No',
                                        icon: 'fa fa-close',
                                        label: 'No',
                                        cssClass: 'btn btn-sm btn-dark',
                                        autospin: false,
                                        action: function (dialogRef) {
                                            dialogRef.close();
                                            dialogThirdpartyConfirmed = true;
                                            var sMsgAnotherPayment = $('#<%= hfAnotherPaymentMessage.ClientID %>').val();
                                            $('#<%=hidChkChoice.ClientID%>').val('false');
                                            if (obj) obj.click();
                                        }
                                    },
                {
                    id: 'btn-Yes',
                    icon: 'fa fa-check',
                    label: 'Yes',
                    cssClass: 'btn btn-sm btn-primary',
                    autospin: false,
                    action: function (dialogRef) {
                        dialogRef.close();
                        dialogThirdpartyConfirmed = true;
                        $('#<%=hidChkChoice.ClientID%>').val('true');
                        if (obj) obj.click();
                    }
                }]
                                });
        }
        return dialogThirdpartyConfirmed;
    }

    function FurtherPaymentConfirm(msg) {
        if (confirm(msg) == true) {
            document.getElementById('<%=hidChkChoice.ClientID%>').value = 'TRUE';
        }
        else {
            document.getElementById('<%=hidChkChoice.ClientID%>').value = 'FALSE';
        }
    }
    </script>


    <div class="card-body no-padding clearfix">
        <div class="form-horizontal">
            <legend>
                <asp:Label ID="lblReinsuranceMain" runat="server" Text="<%$ Resources:lbl_Reinsurance %>"></asp:Label></legend>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblReinsuranceBand" runat="server" AssociatedControlID="litReinsuranceBand" class="col-md-4 col-sm-3 control-label">
                    <asp:Literal ID="litReinsuranceBand" runat="server" Text="<%$ Resources:lblReinsuranceBand %>"></asp:Literal></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <asp:DropDownList ID="ddlReinsurance" runat="server" AutoPostBack="True" CssClass="field-medium form-control form-select">
                    </asp:DropDownList>
                </div>
                <br>
            </div>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="updClaimReinsurance" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="grid-card table-responsive">
                <asp:GridView ID="gvClaimReinsurance" runat="server" AllowPaging="false" AutoGenerateColumns="False" GridLines="None" PageSize="5" ShowHeader="True" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" CssClass="RITable">
                    <Columns>
                        <asp:BoundField DataField="RIarrangementKey" HeaderText="RIarrangementKey"></asp:BoundField>
                        <asp:BoundField DataField="RIArrangementLineKey" HeaderText="RIArrangementLineKey"></asp:BoundField>
                        <asp:BoundField DataField="IsRIBroker" HeaderText="IsRIBroker"></asp:BoundField>
                        <asp:BoundField DataField="Placement" HeaderText="<%$ Resources:lblPlacement %>"></asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lblName %>"></asp:BoundField>
                        <asp:BoundField DataField="Retained" HeaderText="<%$ Resources:lblRetained %>" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc"></asp:BoundField>
                        <asp:BoundField DataField="DefaultPerc" HeaderText="<%$ Resources:lblDefault %>" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc"></asp:BoundField>
                        <Nexus:TemplateField HeaderText="<%$ Resources:lblThis %>" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc" DataType="Percentage">
                            <itemtemplate>
                                            <asp:TextBox ID="txtThisPerc" runat="server" Visible="false" Text='<%# XPath("@ThisPerc")%>' OnTextChanged="txtThisPerc_TextChanged" AutoPostBack="true" CssClass="PercTextBox"></asp:TextBox>
                                            <asp:RangeValidator ID="rngThisPerc" runat="server" Display="None" ControlToValidate="txtThisPerc" MinimumValue="0.01" MaximumValue="100" Type="Double" ErrorMessage="<%$ Resources:Err_InvalidRange %>" Enabled="false"></asp:RangeValidator>
                                            <asp:Label ID="lblThisPerc" runat="server" Visible="false" Text='<%# XPath("@ThisPerc")%>' CssClass="Perc"></asp:Label>
                                        </itemtemplate>
                        </Nexus:TemplateField>
                        <Nexus:BoundField DataField="LowerLimit" HeaderText="<%$ Resources:lblLowerLimit %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="LineLimit" HeaderText="<%$ Resources:lblUpperLimit %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="SumInsured" HeaderText="<%$ Resources:lblSumInsured %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="ReserveToDate" HeaderText="<%$ Resources:lblReserveToDate %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="ThisReserve" HeaderText="<%$ Resources:lblThisReserve %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="PaymentToDate" HeaderText="<%$ Resources:lblPaymentToDate %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="ThisPayment" HeaderText="<%$ Resources:lblThisPayment %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="RecoverToDate" HeaderText="<%$ Resources:lblRecoverdToDate %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="Balance" HeaderText="<%$ Resources:lblBalance %>" DataType="Currency"></Nexus:BoundField>
                        <asp:BoundField DataField="Agreement" HeaderText="<%$ Resources:lblAgreement %>" HeaderStyle-CssClass="str"></asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="rowMenu">

                                    <ol id="menu_<%# Eval("RIArrangementLineKey") %>" class="list-inline no-margin">
                                        <li>
                                            <asp:LinkButton ID="lnkView" runat="server" Text="<%$ Resources:lbl_grdvPerils_linkView_text %>" Visible="false" CausesValidation="false" SkinID="btnGrid"></asp:LinkButton>
                                        </li>
                                    </ol>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvClaimReinsurance" EventName="Load"></asp:AsyncPostBackTrigger>
            <asp:AsyncPostBackTrigger ControlID="gvClaimReinsurance" EventName="RowCommand"></asp:AsyncPostBackTrigger>
            <asp:AsyncPostBackTrigger ControlID="gvClaimReinsurance" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
   <Nexus:ProgressIndicator ID="upClaimReinsurance" OverlayCssClass="updating" AssociatedUpdatePanelID="updClaimReinsurance" runat="server">
        <progresstemplate>
                    </progresstemplate>
    </Nexus:ProgressIndicator>
    <asp:HiddenField ID="hidChkChoice" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hidChlClaimClose" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hidChkPaymentMsg" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfAnotherPaymentMessage" runat="server"></asp:HiddenField>
    <div class="card-footer no-padding">
        <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lbl_btn_Cancel %>" SkinID="btnSecondary"></asp:LinkButton>
        <asp:LinkButton ID="btnNext" runat="server" Text="<%$ Resources:lbl_btn_Next %>" SkinID="btnPrimary"></asp:LinkButton>

    </div>

</div>
