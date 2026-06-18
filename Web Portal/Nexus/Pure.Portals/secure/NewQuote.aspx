<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="NewQuote.aspx.vb" Inherits="Nexus.secure_NewQuote" %>

<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
            <div class="grid-card table-responsive">
                <asp:GridView ID="grdProducts" runat="server" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Quotations">
                            <ItemTemplate>
                                <div class="products">
                                    <ul>
                                        <li>
                                            <asp:LinkButton runat="server" ID="lnkQuote" CommandName='<%#Eval("ProductCode")%>'>
                                                <%--<img id="Img" src='<%#Eval("ProductImage")%>' width="96" height="72" alt="" runat="server">--%>
                                                <span><%#Eval("Name")%></span>
                                            </asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Content>
