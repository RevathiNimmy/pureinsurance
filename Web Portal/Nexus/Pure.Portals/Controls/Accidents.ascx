<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Accidents.ascx.vb" Inherits="Nexus.Controls_Accidents" %>

<script language="javascript" type="text/javascript">
    function ReceiveAccidentData(sAccidentData, sPostBackTo) {
        document.getElementById('<%=txtAccidentData.ClientID %>').value = sAccidentData;
        __doPostBack(sPostBackTo, 'UpdateAccident');
    }

    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'hypAccident' || postBackElement.id == "hypAccidentEdit") {
            $get(uprogQuotes).style.display = "block";
        }
    }
</script>

<div id="Controls_Accidents">
    <asp:HiddenField ID="txtAccidentData" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="PnlAccident" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
        <ContentTemplate>
            <legend>
                <asp:Label ID="lblAccidentHeading" runat="server" Text="<%$ Resources:lbl_Accident_heading %>"></asp:Label>
            </legend>
            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="drgAccidents" runat="server" AutoGenerateColumns="false" GridLines="None" DataKeyNames="AccidentKey"
                    EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:BoundField DataField="AccidentKey" HeaderText="<%$ Resources:lbl_Accident_Key %>" Visible="false"></asp:BoundField>
                        <asp:BoundField DataField="AccidentDate" HeaderText="<%$ Resources:lbl_Accident_Date %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_Accident_Description %>"></asp:BoundField>
                        <asp:BoundField DataField="IsAtFault" HeaderText="<%$ Resources:lbl_Accident_IsAtFault %>"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <div class="rowMenu">
                                    <ol class="list-inline no-margin">
                                        <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                            <ol id="menu_<%# Eval("AccidentKey") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                <li>
                                                    <asp:LinkButton ID="hypAccidentEdit" runat="server" Text="<%$ Resources:lbl_Accident_Edit %>" CausesValidation="False"></asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="hypAccidentDelete" runat="server" Text="<%$ Resources:lbl_Accident_Delete %>" CausesValidation="False" CommandName="DeleteRow"></asp:LinkButton>
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
            <asp:LinkButton ID="hypAccidentAdd" runat="server" SkinID="btnSM" Text="<%$ Resources:lit_Accident_Add %>"></asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="upAccidents" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlAccident" runat="server">
    </Nexus:ProgressIndicator>
</div>
    