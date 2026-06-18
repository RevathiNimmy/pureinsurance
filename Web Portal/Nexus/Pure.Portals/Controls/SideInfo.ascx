<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SideInfo.ascx.vb" Inherits="Controls_SideInfo"
    EnableViewState="true" %>

<%@ Register Src="~/Controls/ClientInfo.ascx" TagName="ClientInfo" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ClaimInfo.ascx" TagName="ClaimInfo" TagPrefix="uc3" %>
<script>
    $(window).on("load", function() {
        $('#ClientInf').collapse('hide');
        $('#ClaimInf').collapse('hide');
        $("#ctl00_SideInfo_ClientInfo").find(".fa").removeClass("fa-minus").addClass("fa-plus");
        $("#ctl00_SideInfo_ClaimInfo").find(".fa").removeClass("fa-minus").addClass("fa-plus");

        $("#ClientInf").on('show.bs.collapse', function () {
            $("#ctl00_SideInfo_ClientInfo").find(".fa").removeClass("fa-plus").addClass("fa-minus");
        }).on('hide.bs.collapse', function () {
            $("#ctl00_SideInfo_ClientInfo").find(".fa").removeClass("fa-minus").addClass("fa-plus");
        });

        $("#ClientInf").on('show.bs.collapse', function () {
            $("#ctl00_SideInfo_ClientInfo").find(".fa").removeClass("fa-plus").addClass("fa-minus");
        }).on('hide.bs.collapse', function () {
            $("#ctl00_SideInfo_ClientInfo").find(".fa").removeClass("fa-minus").addClass("fa-plus");
        });
    });
</script>
<div id="Controls_SideInfo">
    <div class="modal fade" id="infoSection" data-bs-backdrop="false">
        <div class="modal-dialog right  w-xxl bg-white md-whiteframe-z2">
                <div class="modal-content">
      
            <div class="box">
                <div class="p p-h-md bg-light">
                    <a href="#" data-bs-dismiss="modal" class="pull-right text-muted-lt text-2x m-t-n inline p-sm">&times;</a>
                    <strong>Info Section</strong>
                </div>
                <div class="box-row">
                    <div class="box-cell">
                        <div class="box-inner">
                            <div class="list-group no-radius no-borders Client-info-heading">
                                <!-- Info section -->
                                <div id="side-accordion">
                                    <div class="md-whiteframe-z0 m-b-lg no-margin">
                                        <ul class="nav scroll-y" ui-nav="">
                                            <li id="ClientInfo" runat="server" visible="false"  class="active">
                                                <a id="#" class="auto" md-ink-ripple="">
                                                    <span class="pull-right text-muted m-r-xs">
                                                     <button type="button" class="btn btn-link collapsed" data-bs-toggle="collapse" data-bs-target="#ClientInf"><i class="fa fa-minus"></i></button>   
                                                    </span>
                                                    <asp:Label ID="Label1" Text="<%$ Resources:lbl_ClientInfoHeader %>" runat="server" CssClass="Info-label"></asp:Label>
                                                </a>
                                                <div id="ClientInf" class="nav nav-sub nav-sm collapse in">
                                                    <uc2:ClientInfo ID="ClientInfoCtrl" runat="server"></uc2:ClientInfo>
                                                </div>
                                            </li>
                                            <li id="ClaimInfo" runat="server" visible="false" class="active">
                                                <a href="#" class="auto" md-ink-ripple="">
                                                    <span class="pull-right text-muted m-r-xs">                                             
                                                        <button type="button" class="btn btn-link collapsed" data-bs-toggle="collapse" data-bs-target="#ClaimInf"><i class="fa fa-minus"></i></button>   
                                                    </span>
                                                    <asp:Label ID="Label2" Text="<%$ Resources:lbl_ClaimInfoHeader %>" runat="server" CssClass="Info-label"></asp:Label>
                                                </a>
                                                <div id="ClaimInf" class="nav nav-sub nav-sm collapse in">
                                                    <uc3:ClaimInfo ID="ClaimInfoCtrl" runat="server"></uc3:ClaimInfo>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <!-- Info section -->
                            </div>
                        </div>
                    </div>
                </div>
                <div class="p-h-md p-v bg-light">
                    <div id="ManualTransfer" runat="server" visible="false" class="error">
                        <asp:Label ID="lblManualTransfer" Text="<%$ Resources:lbl_ManualTransfer %>" Visible="false" runat="server"></asp:Label>
                    </div>
                </div>
            </div>

     
                </div>
            </div>
        
        
        
    </div>
    </div>



