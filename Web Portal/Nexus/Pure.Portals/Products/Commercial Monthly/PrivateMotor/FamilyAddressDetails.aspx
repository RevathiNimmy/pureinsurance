<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FamilyAddressDetails.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="Nexus.FamilyAddressDetails" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/AddressCntrl.ascx" TagName="AddressControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/FindParty.ascx" TagName="FindParty" TagPrefix="NexusPartyControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="cntMainBody" contentplaceholderid="cntMainBody" runat="Server" xmlns:asp="remove" xmlns:nexus="remove" xmlns:nexuscontrol="remove" xmlns:nexusprovider="remove">


<script type="text/javascript">


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


    $(document).ready(function () {

        disableAddressControl();

    });

    </script>

 <cc1:ToolkitScriptManager runat="server">
    </cc1:ToolkitScriptManager>
    <div class="risk-screen">
        <div id="divMain" onscroll="SetDivPosition()">
            <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
            <div class="card">
                <nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="TabNav" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></nexus:tabindex>
                <div class="card-body clearfix">
                    <legend><span>Members Details</span></legend>
                    <div class="grid-card table-responsive">
                        <nexus:itemgrid id="MS__DRIVER" runat="server" screencode="DRIVERDETA" autogeneratecolumns="false" gridlines="None" childpage="Child/MEMDETAILS_GENERAL.aspx">
                            <columns><nexus:riskattribute headertext="Name" datafield="DD_NAME"></nexus:riskattribute></columns>
                            <columns><nexus:riskattribute headertext="Date of Birth" datafield="DD_DOB" dataformatstring="{0:d}/"></nexus:riskattribute></columns>
                            <columns><nexus:riskattribute headertext="Licence Number" datafield="DD_DRILIC_NO"></nexus:riskattribute></columns>
                        </nexus:itemgrid>
                    </div>
                </div>
                <div class='card-footer'>
                    <asp:LinkButton id="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" onclick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton id="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" onclick="buttonPrimary" SkinID="btnPrimary"></asp:LinkButton>
                        
                </div>
            </div>
        </div>
    </div>
</asp:content>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<html xmlns:mso="urn:schemas-microsoft-com:office:office" xmlns:msdt="uuid:C2F41010-65B3-11d1-A29F-00AA00C14882"><head>
<!--[if gte mso 9]><SharePoint:CTFieldRefs runat=server Prefix="mso:" FieldList="FileLeafRef,PublishingStartDate,PublishingExpirationDate,Product,Release,Customer,Document_x0020_Type,CR_x0020_Status,_dlc_DocIdPersistId,_dlc_DocId,_dlc_DocIdUrl,Steering_x0020_Date,IconOverlay,Status,Sign_x002d_off_x0020_status,MediaLengthInSeconds,Jira"><xml>
<mso:CustomDocumentProperties>
<mso:_dlc_DocId msdt:dt="string">SSPIDCO-871300273-239134</mso:_dlc_DocId>
<mso:_dlc_DocIdItemGuid msdt:dt="string">daa60ade-71af-49fa-b8fb-de3d4ebdbe27</mso:_dlc_DocIdItemGuid>
<mso:_dlc_DocIdUrl msdt:dt="string">https://ssplimited.sharepoint.com/sites/Ops/Dep/_layouts/15/DocIdRedir.aspx?ID=SSPIDCO-871300273-239134, SSPIDCO-871300273-239134</mso:_dlc_DocIdUrl>
</mso:CustomDocumentProperties>
</xml></SharePoint:CTFieldRefs><![endif]-->
</head>