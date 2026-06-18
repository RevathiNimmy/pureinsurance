<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Convictions.ascx.vb"
    Inherits="Nexus.Controls_Convictions" %>

<script language="javascript" type="text/javascript">
    function ReceiveConvictionData(sConvictionData, sPostBackTo) {
        document.getElementById('<%=txtConvictionData.ClientID %>').value = sConvictionData;
        __doPostBack(sPostBackTo, 'UpdateConviction');
    }

    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'hypConviction' || postBackElement.id == "hypConvictionEdit") {
            $get(uprogQuotes).style.display = "block";
        }
    }
</script>

<div id="Controls_Convictions">
    <asp:HiddenField ID="txtConvictionData" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="PnlConviction" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
        <ContentTemplate>
            <legend>
                <asp:Label ID="lblConvictionHeading" runat="server" Text="<%$ Resources:lbl_Conviction_heading %>"></asp:Label>
            </legend>
            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="drgConviction" runat="server" AutoGenerateColumns="false" GridLines="None" DataKeyNames="Key" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:BoundField DataField="TypeCode" HeaderText="<%$ Resources:lbl_Conviction_Type %>"></asp:BoundField>
                        <asp:BoundField DataField="ConvictionDate" HeaderText="<%$ Resources:lbl_Conviction_Date %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_Conviction_Description %>"></asp:BoundField>
                        <asp:BoundField DataField="FineAmount" HeaderText="<%$ Resources:lbl_Conviction_Fine %>" DataFormatString="{0:N2}"></asp:BoundField>
                        <asp:BoundField DataField="StatusCode" HeaderText="<%$ Resources:lbl_Conviction_Status%>"></asp:BoundField>
                        <asp:BoundField DataField="DrivingLicensePenaltyPoints" HeaderText="<%$ Resources:lbl_Conviction_PenaltyPoints %>"></asp:BoundField>
                        <asp:BoundField DataField="Key" HeaderText="<%$ Resources:lbl_Conviction_Key %>" Visible="false"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <div class="rowMenu">
                                    <ol class="list-inline no-margin">
                                        <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                            <ol id="menu_<%# Eval("Key") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                <li>
                                                    <asp:LinkButton ID="hypConvictionEdit" runat="server" Text="<%$ Resources:lbl_Conviction_Edit %>" CausesValidation="False"></asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="hypConvictionDelete" runat="server" Text="<%$ Resources:lbl_Conviction_Delete %>" CausesValidation="False" CommandName="DeleteRow"></asp:LinkButton>
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
             <div class ="m-v-sm">
            <asp:LinkButton ID="hypConviction" runat="server" SkinID="btnSM" Text="<%$ Resources:lit_AddConviction %>"></asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="upConvictions" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlConviction" runat="server">
        <progresstemplate>
        </progresstemplate>
    </Nexus:ProgressIndicator>
</div>
