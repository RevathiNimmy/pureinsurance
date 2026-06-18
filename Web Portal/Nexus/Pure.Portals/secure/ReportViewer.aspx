<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReportViewer.aspx.vb" Inherits="secure_ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report Viewer</title>
    <style type="text/css">
 body:nth-of-type(1) img[src*="Blank.gif"]
 {
     display: none; 
 }
 </style>
     <script type="text/javascript">
         window.onunload = function () {
             if (window.opener && !window.opener.closed) {
                 const params = new URLSearchParams(window.location.search);

                 // Read specific values
                 const rptname = params.get("reportname");
                 
                 const rptpartyid = params.get("PartyKey");
                 if (rptname.includes("Remittance_Advice_Agency")) {
                     alert("Welcome, admin!");
                     window.opener.location.href = "InsurerPayments.aspx?mode=IP&PartyKey=" + rptpartyid; // Redirect parent
                 } 
                
             }
         };
     </script>
</head>
   
    <body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div style="display:flex; align-items:center; background-color:#f0f0f0; border-bottom:1px solid #ccc;">
            <div style="flex:1;">
                <!-- ReportViewer toolbar renders here naturally -->
            </div>
            <div id="divEmailToolbar" runat="server" style="padding:4px 10px;" visible="false">
                <asp:Button ID="btnEmailRemittanceAdvice" runat="server" Text="Email Remittance Advice" 
                    CssClass="btn btn-primary" OnClick="btnEmailRemittanceAdvice_Click" 
                    style="font-size:12px; padding:4px 12px; cursor:pointer;" />
            </div>
        </div>
        <div style="height:calc(100vh - 40px);">
            <rsweb:ReportViewer ID="viewerReportViewer" runat="server" Height="100%" Width="100%" OnReportRefresh="viewerReportViewer_ReportRefresh"></rsweb:ReportViewer>
        </div>
    </form>
</body>

</html>
