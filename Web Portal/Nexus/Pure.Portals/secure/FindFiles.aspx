<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="FindFiles.aspx.vb" Inherits="Nexus.secure_FindFiles" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="secure_FindFiles">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClientCode" runat="server" AssociatedControlID="txtClientCode" Text="<%$ Resources:lbl_ClientCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClientCode" runat="server" CssClass="form-control"></asp:TextBox></div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClientName" runat="server" AssociatedControlID="txtClientName" Text="<%$ Resources:lbl_ClientName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control"></asp:TextBox></div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPostCode" runat="server" AssociatedControlID="txtPostCode" Text="<%$ Resources:lbl_PostCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="form-control"></asp:TextBox></div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" Text="<%$ Resources:lbl_PolicyNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyNumber" runat="server" CssClass="form-control"></asp:TextBox></div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimNumber" runat="server" AssociatedControlID="txtClaimNumber" Text="<%$ Resources:lbl_ClaimNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimNumber" runat="server" CssClass="form-control"></asp:TextBox></div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskIndex" runat="server" AssociatedControlID="txtRiskIndex" Text="<%$ Resources:lbl_RiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtRiskIndex" runat="server" CssClass="form-control"></asp:TextBox></div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFilename" runat="server" AssociatedControlID="txtFilename" Text="<%$ Resources:lbl_Filename %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFilename" runat="server" CssClass="form-control"></asp:TextBox></div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnSearch" runat="server" Text="<%$ Resources:btn_Search%>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>

        <nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_error %>" ControlsToValidate="txtClientCode,txtClientName,txtPostCode,txtPolicyNumber,txtClaimNumber,txtRiskIndex,txtFilename" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </nexus:WildCardValidator>
         <iframe id="docFrame1" runat="server" width="0px" height="0px" class="hide"  ></iframe>
        
        <asp:UpdatePanel runat="server" ID="updFindFilesSearch" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvDocumentResults" runat="server" PageSize="10" AllowPaging="true" AllowSorting="true" DataKeyNames="DocNum" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_Name%>" SortExpression="DocDescription">
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("DocNum") %>" class="list-inline no-margin">
                                            <li>
                                                 <asp:LinkButton ID="hypDocDescription" SkinID="btnHGrid" runat="server" OnCommand="lnkDocDescription_Command" ></asp:LinkButton></li>
                                                       </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CreateDate" SortExpression="CreateDate" HeaderText="<%$ Resources:lbl_Created%>" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_Type%>" SortExpression="DocumentType">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocumentType" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_Location%>" SortExpression="FolderPath">
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("DocNum") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:HyperLink ID="hypFolderPath" runat="server" SkinID="lnkHGrid"></asp:HyperLink>
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
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="DataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="RowEditing"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <nexus:ProgressIndicator ID="upSearchClaim" OverlayCssClass="updating" AssociatedUpdatePanelID="updFindFilesSearch" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </nexus:ProgressIndicator>
    </div>
</asp:Content>
