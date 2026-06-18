<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChangeClaim.ascx.vb"
    Inherits="Controls_ChangeClaim" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<script type="text/javascript">
    $(document).ready(function () {
        IfOther();
    });
    function IfOther() {
        var e = document.getElementById("<%=ddlReason.ClientID%>");
        var strSelectedValue = e.value.toUpperCase().trim();
        if (strSelectedValue == "OTHER") {
            document.getElementById("<%=lblIfOther.ClientID%>").style.visibility = "visible";
            document.getElementById("<%=txtIfOther.ClientID%>").style.visibility = "visible";
        }
        else {
            document.getElementById("<%=lblIfOther.ClientID%>").style.visibility = "hidden";
            document.getElementById("<%=txtIfOther.ClientID%>").style.visibility = "hidden";
            document.getElementById("<%=txtIfOther.ClientID%>").value = "";
        }
    }
</script>

<div class="card-body clearfix no-padding">
    <div class="form-horizontal">
        <legend>
            <asp:Label ID="lblReasonForChange" runat="server" Text="<%$ Resources:lbl_ReasonForChange %>" />
        </legend>
        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
            <asp:Label ID="lblReason" runat="server" AssociatedControlID="ddlReason" CssClass="col-md-4 col-sm-3 control-label">
                <asp:Literal ID="ltReason" runat="server" Text="<%$ Resources:lbl_Reason%>" /></asp:Label>
            <div class="col-md-8 col-sm-9">
                <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control form-select" onchange="IfOther();" />
            </div>
        </div>
        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
            <asp:Label ID="lblIfOther" runat="server" AssociatedControlID="txtIfOther" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_IfOther%>" />
            <div class="col-md-8 col-sm-9">
                <asp:TextBox ID="txtIfOther" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
</div>
<asp:HiddenField ID="hidChkChoice" runat="server" />
<asp:HiddenField ID="hidChlClaimClose" runat="server" />
<asp:HiddenField ID="hidChkPaymentMsg" runat="server" />
