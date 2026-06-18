<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="PolicyLapsed.aspx.vb" Inherits="Nexus.secure_PolicyLapsed" Title="Untitled Page" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        onload = function () {
            if (document.all.rdoLapsedReasonList[0] != null) {
                document.all.rdoLapsedReasonList[0].focus();
            }
        }
        function LapsedReasonRequired_ClientValidation(source, arguments) {

            //at least one Radio Button should be selected
            var rdoLapsedReasonListCount = document.all["rdoLapsedReasonList"].length; // count the radio buttons
            if (rdoLapsedReasonListCount == null) {//if only one reason is in the list then rdoMTAReasonListCount==null
                if (document.all.rdoLapsedReasonList.checked) {
                    //if there is only one and IsSelected then return true
                    arguments.IsValid = true;
                    return;
                }
            }

            //if more than two records then need to check which one is selected using the loop 
            for (var CountVar = 0; CountVar < rdoLapsedReasonListCount; CountVar++) {
                if (document.all.rdoLapsedReasonList[CountVar].checked) {
                    //if any one is selected then return true
                    arguments.IsValid = true;
                    return;
                }
            }
            //if no one is selected then return false
            arguments.IsValid = false;
        }
    </script>

    <div id="secure_PolicyLapsed">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblHeaderTitle" runat="server" Text="<%$ Resources:lbl_HeaderTitle%>"></asp:Literal></h1>
            </div>
            <asp:Panel ID="pnlLapsedReason" runat="server">
                <div class="card-body clearfix">
                    <div class="grid-card table-responsive">
                        <asp:GridView ID="GridLapsedReasons" runat="server" AutoGenerateColumns="False" Caption="<%$ Resources:lbl_LapsedReason_Heading %>" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_LapsedTable_Col1Heading %>">
                                    <ItemTemplate>
                                        <asp:Literal ID="literal" runat="server" Text='<%#Eval("Column1")%>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_LapsedTable_Col2Heading %>">
                                    <ItemTemplate>
                                        <input type="radio" name="rdoLapsedReasonList" value='<%#Eval("Column2")%>' <%#eval("column3")%>>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <div class="card-footer">
                    <asp:LinkButton ID="btnLapse" runat="server" Text="<%$ Resources:btn_LapsePolicy%>" SkinID="btnPrimary"></asp:LinkButton>
                </div>
                <asp:CustomValidator ID="CustVldMTAReasonRequired" runat="server" ErrorMessage="<%$ Resources:lbl_LapsedReason %>" ClientValidationFunction="LapsedReasonRequired_ClientValidation" SetFocusOnError="true" Display="None"></asp:CustomValidator>
                <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
            </asp:Panel>
            <asp:Panel ID="pnlLapsedMsg" runat="server" Visible="false">
                <h2>
                    <asp:Literal ID="ltConfirmationMessage" runat="server" Text="<%$ Resources: lt_ConfirmationMessage%>"></asp:Literal>
                </h2>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
