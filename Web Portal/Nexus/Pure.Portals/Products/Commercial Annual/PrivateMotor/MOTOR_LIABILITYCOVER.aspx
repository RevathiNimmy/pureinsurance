<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" MaintainScrollPositionOnPostback="true"
    CodeFile="MOTOR_LIABILITYCOVER.aspx.vb" Inherits="Nexus.MOTOR_FAMILYDETAILS" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script language="javascript" type="text/javascript">
        window.onload = function () {
            var strCook = document.cookie;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                document.getElementById("divMain").scrollTop = strPos;
            }
        }
        function SetDivPosition() {
            var intY = document.getElementById("divMain").scrollTop;
            document.cookie = "yPos=!~" + intY + "~!";
        }
    </script>
    <div class="risk-screen">
        <div id="divMain" onscroll="SetDivPosition()">
            <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
            <div class="card">
                <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="TabNav" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <legend><span>FamilyDetails</span></legend>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                FamilyTitle</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="FAMILYDETAILS__FAMILYTITLE" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                TotalMembers</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="FAMILYDETAILS__TOTALMEMBERS" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                    </div>
                    <legend><span>MembersDetials</span></legend>
                    <div class="grid-card table-responsive">
                        <Nexus:itemgrid id="FAMILYDETAILS__MEMBERSDETIALS" runat="server" screencode="MemDetails" autogeneratecolumns="false" gridlines="None" childpage="Child/MEMDETAILS_GENERAL.aspx">
                            <columns><nexus:riskattribute headertext="Name" datafield="NAME"></nexus:riskattribute></columns>
                        </Nexus:itemgrid>
                    </div>
                    <div class="form-horizontal">
                        <legend><span>Default Liability Cover</span></legend>
                        <asp:Label ID="lblThirdPartyLiab" runat="server" AssociatedControlID="LIABCOVER__ThirdPartyLiab" Text="Third Party Liability">
                        </asp:Label>
                        <asp:CheckBox ID="LIABCOVER__ThirdPartyLiab" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        <asp:TextBox ID="LIABCOVER_TPL_LOI" runat="server"></asp:TextBox>
                        <asp:TextBox ID="LIABCOVER_TPL_RATE" runat="server"></asp:TextBox>
                        <asp:TextBox ID="LIABCOVER_TPL_PREMIUM" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-horizontal">
                        <div>
                            <legend><span>Optional Cover</span></legend>
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="LIABCOVER__CARHIRE" Text="Car Hire"></asp:Label>
                            <asp:CheckBox ID="LIABCOVER__CARHIRE" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label ID="Label2" runat="server" AssociatedControlID="LIABCOVER__CONTLIAB" Text="Contigent Liability"></asp:Label>
                            <asp:CheckBox ID="LIABCOVER__CONTLIAB" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label ID="Label3" runat="server" AssociatedControlID="LIABCOVER__CREDSHORT" Text="Credit Shortfall"></asp:Label>
                            <asp:CheckBox ID="LIABCOVER__CREDSHORT" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                            <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label ID="Label4" runat="server" AssociatedControlID="LIABCOVER__WRECKREMOVE" Text="Wreckage Removal"></asp:Label>
                            <asp:CheckBox ID="LIABCOVER__WRECKREMOVE" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                            <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
                        </div>

                    </div>
                </div>
                <div class='card-footer'>
                    <asp:LinkButton ID="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" OnClick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
            <asp:ValidationSummary ID="vldSummaryMainDetails" runat="server" HeaderText="Validation" CssClass="validation-summary"></asp:ValidationSummary>
        </div>
    </div>
</asp:Content>
