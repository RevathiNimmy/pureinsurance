<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" MaintainScrollPositionOnPostback="true"
    CodeFile="MOTOR_LIABCOVER.aspx.vb" Inherits="Nexus.MOTOR_FAMILYDETAILS" %>

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
        CalcPremTPL();
        CalcPremCarHire();
        CalcPremContLiab();
        CalcPremShrtFall();
        CalcPremWreck();
    }
    function SetDivPosition() {
        var intY = document.getElementById("divMain").scrollTop;
        document.cookie = "yPos=!~" + intY + "~!";
    }

 
    function CalcPremTPL() {
        document.getElementById('<%=VEHDET__TPL_PREMIUM.ClientID%>').value = document.getElementById('<%=VEHDET__TPL_LIMIT_IDEMNITY.ClientID%>').value * parseInt(document.getElementById('<%=VEHDET__TPL_RATEN.ClientID%>').value) * .01;
    }

        function CalcPremCarHire() {

            document.getElementById('<%=MS__DLC_CHR_PREM.ClientID%>').value =
               document.getElementById('<%=MS__DLC_CHR_LIMIT_IDEMN.ClientID%>').value * parseInt(document.getElementById('<%=MS__DLC_CHR_RATEN.ClientID%>').value) * .01;
    }

    function CalcPremContLiab() {

        document.getElementById('<%=MS__DLC_CL_PREM.ClientID%>').value =
       document.getElementById('<%=MS__DLC_CL_LIMIT_IDEMN.ClientID%>').value * parseInt(document.getElementById('<%=MS__DLC_CL_RATEN.ClientID%>').value) * .01;
    }

    function CalcPremShrtFall() {

        document.getElementById('<%=MS__DLC_CR_PREM.ClientID%>').value =
       document.getElementById('<%=MS__DLC_CR_LIMIT_IDEMN.ClientID%>').value * parseInt(document.getElementById('<%=MS__DLC_CR_RATEN.ClientID%>').value) * .01;
    }

        function CalcPremWreck() {

            document.getElementById('<%=MS__DLC_WR_PREM.ClientID%>').value =
            document.getElementById('<%=MS__DLC_WR_LIMIT_IDEMN.ClientID%>').value * parseInt(document.getElementById('<%=MS__DLC_WR_RATEN.ClientID%>').value) * .01;
    }
 
        function isInteger(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            var ValidChars = "0123456789.";
            var IsNumber = true;
            if (ValidChars.indexOf(keychar) == -1) {
                IsNumber = false;
            }
            return IsNumber;
        }

        function ValidateNumberOnly() {
            if ((event.keyCode < 48="" ||="" event.keycode=""> 57)) {
                event.returnValue = false;
            }
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
                            
<%--                            <fieldset>
                                <ol>
                                    </ol>
                                    <legend><span>FamilyDetails</span></legend>
                                    <li>
                                        <label>
                                            FamilyTitle</label><asp:TextBox ID="FAMILYDETAILS__FAMILYTITLE" runat="server" />
                                    </li>
                                    <li>
                                        <label>
                                            TotalMembers</label><asp:TextBox ID="FAMILYDETAILS__TOTALMEMBERS" runat="server" />
                                    </li>
                                </ol>
                            </fieldset>--%>
                            
                        </div>
                    </div>
                    <%--<div class="standard-form">
                        <div class="fieldset-wrapper">
                            <div class="top-corners">
                            </div>
                            <fieldset>
                                <legend><span>MembersDetials</span></legend>
                                <div class="table-wrapper">
                                    <Nexus:ItemGrid ID="FAMILYDETAILS__MEMBERSDETIALS" runat="server" ScreenCode="MemDetails"
                                        AutoGenerateColumns="false" GridLines="None" ChildPage="Child/MEMDETAILS_GENERAL.aspx">
                                        <columns><Nexus:RiskAttribute HeaderText="Name" DataField="NAME" /></columns>
                                    </Nexus:ItemGrid>
                                </div>
                            </fieldset>
                            <div class="bottom-corners">
                                <div>
                                </div>
                            </div>
                        </div>
                    </div>--%>

                    <table class="checkTable" cellspacing="0" cellpadding="0">
                                        <tr class="headerRow">
                                            <th>
                                                Test
                                            </th>
                                            <th>
                                                 Limit of Indemnity  
                                            </th>      
                                            <th>
                                                Rate  
                                            </th> 
                                            <th>
                                                Premium  
                                            </th>                                      
                                        </tr>
                                        <tr class="detailRow">
                                            <td class="extentionName"> 
                                                <label>Third Party Liability</label>
                                                <asp:checkbox id="VEHDET__DLC_THIRD_PARTY_LIABILITY" runat="server" autopostback="true" Text=" " CssClass="asp-check"></asp:checkbox>
                                            </td>
                                            <td class="limitOfIndemnity">
                                                <asp:textbox id="VEHDET__TPL_LIMIT_IDEMNITY" runat="server" onchange='return CalcPremTPL();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:textbox>
                                              
                                            </td>
                                            <td class="fapPerc">     
                                                <asp:textbox id="VEHDET__TPL_RATEN" runat="server" onchange='return CalcPremTPL();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:textbox>                                           
                                            </td>
                                            <td class="premium">  
                                                <asp:textbox id="VEHDET__TPL_PREMIUM" runat="server" enabled="false"></asp:textbox>                                             
                                            </td>                                                                                       
                                        </tr>                                        
                    </table>


                    <table class="checkTable" cellspacing="0" cellpadding="0">
                                        <tr class="headerRow">
                                            <th>
                                                Section Name
                                            </th>
                                            <th>
                                                Limit of Indemnity
                                            </th>  
                                            <th>
                                                Rate
                                            </th> 
                                            <th>
                                                Premium
                                            </th>                                                                                  
                                        </tr>
                                        <tr class="detailRow">
                                            <td class="extentionName">
                                                <label>
                                                    Cars Hire </label>
                                                <asp:checkbox id="MS__DLC_CAR_HIRE" runat="server" autopostback="true" Text=" " CssClass="asp-check"></asp:checkbox>
                                            </td>
                                            <td class="limitOfIndemnity">
                                                <asp:textbox id="MS__DLC_CHR_LIMIT_IDEMN" runat="server" onchange='return CalcPremCarHire();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:textbox>
                                            </td>
                                            <td class="fapPerc">     
                                                <asp:textbox id="MS__DLC_CHR_RATEN" runat="server" onchange='return CalcPremCarHire();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:textbox>                                           
                                            </td>
                                            <td class="premium">     
                                                <asp:textbox id="MS__DLC_CHR_PREM" runat="server" enabled="false"></asp:textbox>                                          
                                            </td>                                                                                     
                                        </tr>  
                                        <tr class="detailRow">
                                            <td class="extentionName">
                                                <label>
                                                    Contigent Liability</label>
                                                <asp:checkbox id="MS__DLC_CONTIGENT_LIAB" runat="server" autopostback="true" Text=" " CssClass="asp-check"></asp:checkbox>
                                            </td>
                                            <td class="limitOfIndemnity">
                                                <asp:textbox id="MS__DLC_CL_LIMIT_IDEMN" runat="server" onchange='return CalcPremContLiab();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:textbox>
                                            </td>
                                            <td class="fapPerc">     
                                                <asp:textbox id="MS__DLC_CL_RATEN" runat="server" onchange='return CalcPremContLiab();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:textbox>                                           
                                            </td>
                                            <td class="premium">     
                                                <asp:textbox id="MS__DLC_CL_PREM" runat="server" enabled="false"></asp:textbox>                                          
                                            </td>                                                                                     
                                        </tr>   
                                        <tr class="detailRow">
                                            <td class="extentionName">
                                                <label>
                                                    Credit Shortfall</label>
                                                <asp:checkbox id="MS__DLC_CR_SHRTFALL" runat="server" autopostback="true" Text=" " CssClass="asp-check"></asp:checkbox>
                                            </td>
                                            <td class="limitOfIndemnity">
                                                <asp:textbox id="MS__DLC_CR_LIMIT_IDEMN" runat="server" onchange='return CalcPremShrtFall();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:textbox>
                                            </td>
                                            <td class="fapPerc">     
                                                <asp:textbox id="MS__DLC_CR_RATEN" runat="server" onchange='return CalcPremShrtFall();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:textbox>                                           
                                            </td>
                                            <td class="premium">     
                                                <asp:textbox id="MS__DLC_CR_PREM" runat="server" enabled="false"></asp:textbox>                                          
                                            </td>                                                                                     
                                        </tr>
                                        <tr class="detailRow">
                                            <td class="extentionName">
                                                <label>
                                                    Wreckage Removal </label>
                                                <asp:checkbox id="MS__DLC_WRECKAGE_REMOVAL" runat="server" autopostback="true" Text=" " CssClass="asp-check"></asp:checkbox>
                                            </td>
                                            <td class="limitOfIndemnity">
                                                <asp:textbox id="MS__DLC_WR_LIMIT_IDEMN" runat="server" onchange='return CalcPremWreck();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:textbox>
                                            </td>
                                            <td class="fapPerc">     
                                                <asp:textbox id="MS__DLC_WR_RATEN" runat="server" onchange='return CalcPremWreck();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:textbox>                                           
                                            </td>
                                            <td class="premium">     
                                                <asp:textbox id="MS__DLC_WR_PREM" runat="server" enabled="false"></asp:textbox>                                          
                                            </td>                                                                                     
                                        </tr>
                                                                             
                                    </table>

                    
                    
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
