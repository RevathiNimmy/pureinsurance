<%@ Page Title="Find FAC PROP" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false" CodeFile="FindFAC.aspx.vb" Inherits="Nexus.Modal_FindFAC" %>

<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<asp:Content ID="Content4" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_FACPlacement">

        <asp:Panel ID="pnlFACPlacement" runat="server" DefaultButton="btnSearch" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" Text="Find FAC PROP" runat="server"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblFindReinsurer" runat="server" Text="<%$ Resources:lblFindReinsurer %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblReinsurerCode" runat="server" AssociatedControlID="txtReinsurerCode" Text="<%$ Resources:lblReinsurerCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtReinsurerCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="<%$ Resources:lblName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFileCode" runat="server" AssociatedControlID="txtFileCode" Text="<%$ Resources:lblFileCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFileCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblType" runat="server" AssociatedControlID="drpType" Text="<%$ Resources:lblType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="drpType" runat="server" CssClass="form-control form-select">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btnNewSearch %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="Button1" runat="server" Text="<%$ Resources:btnCancel%>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSearch" runat="server" Text="<%$ Resources:btnSearch %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </asp:Panel>
        <div class="grid-card table-responsive">
            <asp:GridView ID="gvSearchResult" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData" DataKeyNames="RIName">
                <Columns>
                    <asp:BoundField DataField="ReinsurerCode" HeaderText="<%$ Resources:grdv_ReinsurerCode %>"></asp:BoundField>
                    <asp:BoundField DataField="RIName" HeaderText="<%$ Resources:grdv_Name %>"></asp:BoundField>
                    <asp:BoundField DataField="AccountType" HeaderText="<%$ Resources:grdv_AccType%>"></asp:BoundField>
                    <asp:BoundField DataField="Address1" HeaderText="<%$ Resources:grdv_Address1%>"></asp:BoundField>
                    <asp:BoundField DataField="Address2" HeaderText="<%$ Resources:grdv_Address2%>"></asp:BoundField>
                    <asp:BoundField DataField="PostCode" HeaderText="<%$ Resources:grdv_PostCode%>"></asp:BoundField>
                    <asp:BoundField DataField="ReinsuranceTypeCode" HeaderText="<%$ Resources:grdv_Type%>"></asp:BoundField>
                    <asp:BoundField DataField="BranchCode" HeaderText="<%$ Resources:grdv_Source%>"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div class="rowMenu">
                                <ol id='menu_<%# Eval("ReinsurerCode") %>' class="list-inline no-margin">
                                    <li>
                                        <asp:LinkButton ID="lnkSelect" runat="server" Text="<%$ Resources:btnSelect%>" CausesValidation="false" SkinID="btnGrid"></asp:LinkButton>
                                    </li>
                                </ol>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

