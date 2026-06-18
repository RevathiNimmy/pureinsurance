<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LifeStyles.ascx.vb" Inherits="Nexus.Controls_LifeStyles" %>

<script language="javascript" type="text/javascript">
    function ReceiveLifeStyleData(sLifeStyleData, sPostBackTo) {
        document.getElementById('<%=txtLifeStyleData.ClientID %>').value = sLifeStyleData;
        __doPostBack(sPostBackTo, 'UpdateLifeStyle');
    }

    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'hypLifestyle' || postBackElement.id == "hypLifestyleEdit") {
            $get(uprogQuotes).style.display = "block";
        }
    }
</script>

<div id="Controls_LifeStyles">
    <asp:HiddenField ID="txtLifeStyleData" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="PnlLifestyle" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
        <ContentTemplate>
            <legend>
                <asp:Label ID="lblLifestyleHeading" runat="server" Text="<%$ Resources:lbl_Lifestyle_heading %>"></asp:Label>
            </legend>
            <asp:Label runat="server" ID="Label7"></asp:Label>
            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="drgLifestyle" runat="server" AutoGenerateColumns="false" GridLines="None" DataKeyNames="Key" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lbl_Lifestyle_Name%>"></asp:BoundField>
                        <asp:BoundField DataField="DateOfBirth" HeaderText="<%$ Resources:lbl_Lifestyle_DateOfBirth %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                        <asp:BoundField DataField="CategoryCode" HeaderText="<%$ Resources:lbl_Lifestyle_CategoryCode %>"></asp:BoundField>
                        <asp:BoundField DataField="GenderCode" HeaderText="<%$ Resources:lbl_Lifestyle_GenderCode %>"></asp:BoundField>
                        <asp:BoundField DataField="OccupationCode" HeaderText="<%$ Resources:lbl_Lifestyle_OccupationCode %>"></asp:BoundField>
                        <asp:BoundField DataField="SecOccupationCode" HeaderText="<%$ Resources:lbl_Lifestyle_SecOccupationCode %>"></asp:BoundField>
                        <asp:BoundField DataField="Smoker" HeaderText="<%$ Resources:lbl_Lifestyle_Smoker %>"></asp:BoundField>
                        <asp:BoundField DataField="Key" HeaderText="<%$ Resources:lbl_Lifestyle_Key %>" Visible="false"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <div class="rowMenu">
                                    <ol class="list-inline no-margin">
                                        <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                            <ol id="menu_<%# Eval("Key") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                <li>
                                                    <asp:LinkButton ID="hypLifestyleEdit" runat="server" Text="<%$ Resources:lbl_Lifestyle_Edit %>" CausesValidation="False"></asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="hypLifestyleDelete" runat="server" Text="<%$ Resources:lbl_Lifestyle_Delete %>" CausesValidation="False" CommandName="DeleteRow"></asp:LinkButton>
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
            <asp:LinkButton ID="hypLifestyle" runat="server" SkinID="btnSM" Text="<%$ Resources:lit_AddLifestyle %>"></asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="UpLifestyle" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlLifestyle" runat="server">
        <progresstemplate>
        </progresstemplate>
    </Nexus:ProgressIndicator>
</div>
