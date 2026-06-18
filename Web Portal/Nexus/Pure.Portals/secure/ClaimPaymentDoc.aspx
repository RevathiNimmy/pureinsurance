<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="ClaimPaymentDoc.aspx.vb" Inherits="Nexus.secure_ClaimPaymentDoc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script language="javascript" type="text/javascript">
        function DeclinePayment(sMessage) {
            alert(sMessage);
            window.location = '<%=ResolveClientUrl("~/secure/AuthoriseClaimPayments.aspx")%>';
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="secure_ClaimPaymentDoc">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComents" Text="<%$ Resources:lbl_Comments %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9 my-1">
                            <asp:TextBox ID="txtComents" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="grid-card table-responsive">
                    <asp:GridView runat="server" ID="grdDocumentLinks" GridLines="None" AllowPaging="True" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_DocLinks %>">
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id='menu_<%# Eval("DocumentTemplateCode") %>' class="list-inline no-margin">
                                            <li>
                                                <asp:HyperLink ID="lnkDocument" runat="server" CausesValidation="False" Target="_blank" Text='<%# Eval("DTDescription") %>' NavigateUrl='<%# "~/secure/document.aspx?docClaimPaymentCode=" + Eval("DocumentTemplateCode") + "&PreGenerate=False" %>' SkinID="btnHGrid"></asp:HyperLink>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btn_Ok%>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
