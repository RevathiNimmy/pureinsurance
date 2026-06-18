<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" MaintainScrollPositionOnPostback="true"
    CodeFile="MOTOR_LIABILITYCOVER.aspx.vb" Inherits="Nexus.MOTOR_FAMILYDETAILS" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
<script language="javascript" type="text/javascript">
    window.onload = function() {
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
        <div class="nexus-fluid-layout" id="divMain" onscroll="SetDivPosition()">
            <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
            <div class="card">
                <nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="TabNav" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></nexus:tabindex>
                <div class="card-body clearfix">
                    
                    <div class="standard-form">
                        <div class="fieldset-wrapper">
                            
                            <div class="form-horizontal">
                                
                            
                                    <legend><span>FamilyDetails</span></legend>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <label class="col-md-4 col-sm-3 control-label">
                                            FamilyTitle</label><div class="col-md-8 col-sm-9"><asp:textbox id="FAMILYDETAILS__FAMILYTITLE" runat="server" cssclass="form-control"></asp:textbox></div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <label class="col-md-4 col-sm-3 control-label">
                                            TotalMembers</label><div class="col-md-8 col-sm-9"><asp:textbox id="FAMILYDETAILS__TOTALMEMBERS" runat="server" cssclass="form-control"></asp:textbox></div>
                                    </div>
                                </div>
                            
                        </div>
                    </div>
                    <div class="standard-form">
                        <div class="fieldset-wrapper">
                            
                            <div class="form-horizontal">
                                <legend><span>MembersDetials</span></legend>
                                <div class="grid-card table-responsive">
                                    <nexus:itemgrid id="FAMILYDETAILS__MEMBERSDETIALS" runat="server" screencode="MemDetails" autogeneratecolumns="false" gridlines="None" childpage="Child/MEMDETAILS_GENERAL.aspx">
                                        <columns><nexus:riskattribute headertext="Name" datafield="NAME"></nexus:riskattribute></columns>
                                    </nexus:itemgrid>
                                </div>
                            </div>
                            
                        </div>
                    </div>

                    <div class="standard-form">
                        <div class="fieldset-wrapper">
                            
                            <div class="form-horizontal">
                                <ol>
                                    <legend><span>Default Liability Cover</span></legend>
	                            <asp:label id="lblThirdPartyLiab" runat="server" associatedcontrolid="LIABCOVER__ThirdPartyLiab" text="Third Party Liability">
	                            </asp:label>
	                            <asp:checkbox id="LIABCOVER__ThirdPartyLiab" runat="server" Text=" " CssClass="asp-check"></asp:checkbox>
                                        <asp:textbox id="LIABCOVER_TPL_LOI" runat="server"></asp:textbox>
                                        <asp:textbox id="LIABCOVER_TPL_RATE" runat="server"></asp:textbox>
                                        <asp:textbox id="LIABCOVER_TPL_PREMIUM" runat="server"></asp:textbox>
                                </ol>
                            </div>
                            
                        </div>
                    </div>
                    <h1>
__________________________________________________________________________________________________________________
                    </h1>

                     <br>

                    <div class="standard-form">
                        <div class="fieldset-wrapper">
                            
                            <div class="form-horizontal">
                                <ol>
                                    <legend><span>Optional Cover</span></legend>
	                            <asp:label id="Label1" runat="server" associatedcontrolid="LIABCOVER__CARHIRE" text="Car Hire"></asp:label>
	                            <asp:checkbox id="LIABCOVER__CARHIRE" runat="server" Text=" " CssClass="asp-check"></asp:checkbox>
                                        <asp:textbox id="TextBox1" runat="server"></asp:textbox>
                                        <asp:textbox id="TextBox2" runat="server"></asp:textbox>
                                        <asp:textbox id="TextBox3" runat="server"></asp:textbox>
                                </ol>
                                
                                <br>

                                <ol>
	                            <asp:label id="Label2" runat="server" associatedcontrolid="LIABCOVER__CONTLIAB" text="Contigent Liability"></asp:label>
	                            <asp:checkbox id="LIABCOVER__CONTLIAB" runat="server" Text=" " CssClass="asp-check"></asp:checkbox>
                                        <asp:textbox id="TextBox4" runat="server"></asp:textbox>
                                        <asp:textbox id="TextBox5" runat="server"></asp:textbox>
                                        <asp:textbox id="TextBox6" runat="server"></asp:textbox>
                                </ol>
                                <br>
                                <ol>
	                            <asp:label id="Label3" runat="server" associatedcontrolid="LIABCOVER__CREDSHORT" text="Credit Shortfall"></asp:label>
	                            <asp:checkbox id="LIABCOVER__CREDSHORT" runat="server" Text=" " CssClass="asp-check"></asp:checkbox>
                                        <asp:textbox id="TextBox7" runat="server"></asp:textbox>
                                        <asp:textbox id="TextBox8" runat="server"></asp:textbox>
                                        <asp:textbox id="TextBox9" runat="server"></asp:textbox>
                                </ol>
                                <br>
                                <ol>
	                            <asp:label id="Label4" runat="server" associatedcontrolid="LIABCOVER__WRECKREMOVE" text="Wreckage Removal"></asp:label>
	                            <asp:checkbox id="LIABCOVER__WRECKREMOVE" runat="server" Text=" " CssClass="asp-check"></asp:checkbox>
                                        <asp:textbox id="TextBox10" runat="server"></asp:textbox>
                                        <asp:textbox id="TextBox11" runat="server"></asp:textbox>
                                        <asp:textbox id="TextBox12" runat="server"></asp:textbox>
                                </ol>


                            </div>
                            
                        </div>
                    </div>


                    
                </div><div class='card-footer'>
                    <asp:LinkButton id="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" onclick="NextButton" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton id="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" onclick="BackButton" SkinID="btnSecondary"></asp:LinkButton>                    
                    <asp:LinkButton id="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" onclick="FinishButton" onprerender="PreRenderFinish" SkinID="btnSecondary"></asp:LinkButton>
                </div>
                <asp:validationsummary id="vldSummaryMainDetails" runat="server" headertext="Validation" cssclass="validation-summary"></asp:validationsummary>
                
            </div>
        </div>
    </div>
</asp:Content>
