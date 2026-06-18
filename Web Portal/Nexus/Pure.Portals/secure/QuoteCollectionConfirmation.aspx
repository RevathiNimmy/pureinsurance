<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="QuoteCollectionConfirmation.aspx.vb" Inherits="secure_QuoteCollectionConfirmation" %>

<%--<%@ Register Src="~/Controls/RiskData.ascx" TagName="TransactionConfirmation" TagPrefix="uc1" %>--%>
<%@ Register Src="../Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Document.ascx" TagName="Document" TagPrefix="uc4" %>
<asp:Content ID="cntScriptIncludes" ContentPlaceHolderID="ScriptIncludes" runat="server">
</asp:Content>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="secure_QuoteCollectionConfirmation">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <asp:Literal ID="litMessage" runat="server" Text="<%$ Resources:litMessage%>"></asp:Literal>
                <div class="grid-card table-responsive">
                    <asp:GridView runat="server" ID="grdPolicies" GridLines="None" AllowPaging="True" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="<%$ Resources:lblPolicyNumber %>" DataField="Reference"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <uc4:Document ID="liScheduledocument" runat="server" DocumentName="Schedule" PreGenerate="false" Visible="true" Text="Schedule"></uc4:Document>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <uc4:Document ID="liCertificatedocument" runat="server" DocumentName="Certificate" PreGenerate="false" Visible="true" Text="Certificate"></uc4:Document>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <uc4:Document ID="liReceiptdocument" runat="server" DocumentName="Receipt" PreGenerate="false" Text="Receipt" Visible="true"></uc4:Document>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
