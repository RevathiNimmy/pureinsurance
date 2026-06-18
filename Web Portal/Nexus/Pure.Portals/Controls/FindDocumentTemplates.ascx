<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FindDocumentTemplates.ascx.vb" Inherits="Controls_FindDocumentTemplates" %>

<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<div id="Controls_FindDocumentTemplates">
    <script type="text/javascript" language="javascript">
        window.onload = function () {
            document.getElementById('<%= txtCode.ClientId%>').focus();
        };
    </script>

    <asp:Panel ID="pnlsearch" runat="server" CssClass="card">
        <div class="card-heading">
            <h1>
                <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblHeader%>"></asp:Literal>
            </h1>
        </div>
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblCode" runat="server" AssociatedControlID="txtCode" Text="<%$ Resources:lblCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtCode" TabIndex="1" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblEffectiveDate" runat="server" AssociatedControlID="txtEffectiveDate" Text="<%$ Resources:lblEffectiveDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtEffectiveDate" TabIndex="2" CssClass="field-date form-control" runat="server"></asp:TextBox><uc1:CalendarLookup ID="dtEffectiveDate" runat="server" LinkedControl="txtEffectiveDate" HLevel="2"></uc1:CalendarLookup>
                        </div>
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblType" runat="server" AssociatedControlID="ddlType" Text="<%$ Resources:lblType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="field-medium form-control form-select" TabIndex="3">
                        </asp:DropDownList>
                    </div>
                    <asp:RequiredFieldValidator ID="RqdddlType" runat="server" InitialValue="--Please Select--" ControlToValidate="ddlType" ErrorMessage="<%$ Resources:lbl_ErrMsg_ddlType %>" Display="none" SetFocusOnError="true" Enabled="true"></asp:RequiredFieldValidator>
                </div>

            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btnNewSearch %>" CausesValidation="false" TabIndex="5" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:btnFindNow %>" TabIndex="4" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </asp:Panel>
    <asp:ValidationSummary ID="vldDocumentTemplates" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
    <div class="grid-card table-responsive">
        <asp:GridView ID="grdvTemplates" runat="server" AutoGenerateColumns="False" GridLines="None" CellPadding="0" CellSpacing="0" PageSize="20" AllowPaging="False" AllowSorting="true" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData" DataKeyNames="DocumentTemplateId" Width="100%" ShowFooter="false">
            <Columns>
                <asp:BoundField DataField="Code" HeaderText="<%$ Resources:lbl_Code %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_Description %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                <asp:BoundField DataField="TypeCode" HeaderText="<%$ Resources:lblType %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                <asp:BoundField DataField="EffectiveDate" HeaderText="<%$ Resources:lbl_EffectiveDate %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="span-3">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtnSelect" runat="server" CausesValidation="False" SkinID="btnGrid" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DocumentTemplateId")%>' Text="<%$ Resources:lblSelect %>"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
