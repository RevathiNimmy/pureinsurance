<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="MidFile.aspx.vb" Inherits="Nexus.secure_MidFile" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <h1>
        <asp:Literal ID="lblHeader" runat="server" Text="<%$ Resources:lblHeader %>"></asp:Literal></h1>
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend><span>Summary</span></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblImportingFileLabel" runat="server" AssociatedControlID="lblImportingFile" Text="<%$ Resources:lblImportingFileLabel %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <asp:Label ID="lblImportingFile" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblFileCreatedLabel" runat="server" AssociatedControlID="lblFileCreated" Text="<%$ Resources:lblFileCreatedLabel %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <asp:Label ID="lblFileCreated" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblFileSequenceNoLable" runat="server" AssociatedControlID="lblFileSequenceNo" Text="<%$ Resources:lblFileSequenceNoLable %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <asp:Label ID="lblFileSequenceNo" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblErrorLabel" runat="server" AssociatedControlID="lblErrors" Text="<%$ Resources:lblErrorLabel %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <asp:Label ID="lblErrors" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblWarningsLabel" runat="server" AssociatedControlID="lblWarnings" Text="<%$ Resources:lblWarningsLabel %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <asp:Label ID="lblWarnings" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <div class="grid-card table-responsive">
        <asp:GridView ID="gvGetMIDFileDetails" runat="server" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowPaging="True" DataKeyNames="InsuranceFileCnt" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="error" PageSize="30">
            <Columns>
                <asp:BoundField HeaderText="<%$ Resources:lbl_Record_Type %>" DataField="RecordType" SortExpression="RecordType"></asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:lbl_Policy %>" DataField="Policy" ItemStyle-CssClass="span-2" SortExpression="Policy"></asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:lbl_PPCC_Sent %>" DataField="PPPC" ItemStyle-CssClass="span-2" SortExpression="PPPC"></asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:lbl_PPCC_Expected %>" DataField="ExpectedPPPC" ItemStyle-CssClass="span-2" SortExpression="ExpectedPPPC"></asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:lbl_Registration_No %>" DataField="Registration" ItemStyle-CssClass="span-2" SortExpression="Registration"></asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:lbl_Errors %>" DataField="Errors" ItemStyle-CssClass="span-2" SortExpression="Errors"></asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton Text="<%$ Resources:lblDetails %>" SkinID="btnGrid" ID="lnkDetails" runat="server"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
