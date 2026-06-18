<%@ Control Language="VB" AutoEventWireup="true" CodeFile="QuickSearch.ascx.vb" EnableViewState="true"
    Inherits="Nexus.Controls_QuickSearch" %>

<script type="text/javascript">

    function OnEnter() {
        //  debugger;
        if (event.keyCode == 13) {
            setSelectedValue();
            return false;
        }
        else
            return true;
    }
    function setSearchBy() {
        var ddlSearchType = document.getElementById('<%=ddlSearchType.ClientID%>');
        var ddlSearchBy = document.getElementById('<%=ddlSearchBy.ClientID%>');

        var opt = document.createElement("option");
        for (i = ddlSearchBy.length - 1; i >= 0; i--) { ddlSearchBy.remove(i); }
        opt.text = '- - Please Select - -';
        opt.value = '0';
        ddlSearchBy.options.add(opt);

        switch (ddlSearchType.value) {
            case 'POLICY':
                var arrText = ["Quote/Policy Reference", "Insured Name"];
                var arrValues = ["txtPolicyNumber", "txtClientName"];
                populateSearchBy(arrText, arrValues, ddlSearchBy);
                break;
            case 'CLIENT':
                var arrText = ["Client Name", "Client Code", "File Code", "Address", "Policy Number", "Policy Risk Index", "Claim Number", "Claim Risk Index", "Phone", "DOB", "Post Code"];
                var arrValues = ["txtClientName", "txtClientCode", "txtFileCode", "txtAddress", "txtPolicyNumber", "txtPolicyRiskIndex", "txtClaimNumber", "txtClaimRiskIndex", "txtPhone", "txtDOB", "txtPostcode"];
                populateSearchBy(arrText, arrValues, ddlSearchBy);
                break;
            case 'CLAIM':
                var arrText = ["Claim Reference", "Policy", "Client Code", "Risk Index"];
                var arrValues = ["txtClaimReference", "txtPolicy", "txtClient", "txtRiskIndex"];
                populateSearchBy(arrText, arrValues, ddlSearchBy);
                break;
        }
    }

    function populateSearchBy(arrText, arrValues, ddlSearchBy) {
        for (i = 0; i < arrText.length; i++) {
            opt = document.createElement("option");
            opt.text = arrText[i];
            opt.value = arrValues[i];
            ddlSearchBy.options.add(opt);
        }
    }

    function setSelectedValue() {
        document.cookie = "";
        var ddlSearchBy = document.getElementById('<%=ddlSearchBy.ClientID%>');
        var txtSearchFor = document.getElementById('<%=txtSearchFor.ClientID%>');
        var ddlSearchType = document.getElementById('<%=ddlSearchType.ClientID%>');

        var rqdSearchFor = document.getElementById('<%=rqdSearchFor.ClientID%>');
        var rqdSearchType = document.getElementById('<%=rqdSearchType.ClientID%>');
        var rqdSearchBy = document.getElementById('<%=rqdSearchBy.ClientID%>');

        ValidatorEnable(rqdSearchFor, true);
        ValidatorEnable(rqdSearchType, true);
        ValidatorEnable(rqdSearchBy, true);

        var valueToSearch = txtSearchFor.value;
        var encodedSearchFor = escape(valueToSearch);  //Encode the query string

        if (Page_IsValid) {
            switch (ddlSearchType.value) {
                case 'POLICY':
                    window.location = '<%=ResolveClientUrl("~/Secure/agent/FindClient.aspx") %>?' + ddlSearchBy.value + '=' + encodedSearchFor + '&dosearch=true&jumptoresult=true';
                    break;
                case 'CLAIM':
                    window.location = '<%=ResolveClientUrl("~/Claims/FindClaim.aspx") %>?' + ddlSearchBy.value + '=' + encodedSearchFor + '&dosearch=true&jumptoresult=true';
                    break;
                case 'CLIENT':
                    window.location = '<%=ResolveClientUrl("~/secure/agent/FindClient.aspx")%>?' + ddlSearchBy.value + '=' + encodedSearchFor + '&dosearch=true&jumptoresult=true';
                    break;
            }
        }
    }
</script>

<div id="Controls_QuickSearch">
    <div class="modal fade" id="searchSection" role="dialog" data-bs-backdrop="false">
         <div class="modal-dialog right w-xxl bg-white md-whiteframe-z2">
                <div class="modal-content">
<div class="box">
                <div class="p p-h-md bg-light lt">
                    <a data-bs-dismiss="modal" class="pull-right text-muted-lt text-2x m-t-n inline p-sm">&times;</a>
                    <strong>Search Section</strong>
                </div>
                <div class="box-row">
                    <div class="box-cell">
                        <div class="box-inner">
                            <div class="panel text-color m">
                                <asp:Panel ID="pnlQuickSearch" runat="server" CssClass="ribbon-section quick-search" Visible="true">
                                    <div class="m-b text-sm">
                                        <asp:Label ID="lbl_SearchHeading" runat="server" Text="<%$ Resources:lbl_SearchHeading %>"></asp:Label>
                                    </div>

                                    <div class="form-group form-group-sm">
                                        <asp:Label ID="lblSearchFor" runat="server" AssociatedControlID="txtSearchFor" CssClass="control-label" Text="<%$ Resources:lbl_SearchFor %>"></asp:Label>
                                        <asp:TextBox ID="txtSearchFor" runat="server" ValidationGroup="QuickSearch" CssClass="form-control field-mandatory" onkeydown="javascript:return OnEnter()"></asp:TextBox>
                                    </div>
                                    <div class="form-group form-group-sm">
                                        <asp:Label ID="lblSearchType" runat="server" AssociatedControlID="ddlSearchType" CssClass="control-label" Text="<%$ Resources:lbl_SearchType %>"></asp:Label>
                                        <asp:DropDownList ID="ddlSearchType" runat="server" EnableViewState="false" onchange="setSearchBy();" ValidationGroup="QuickSearch" CssClass="form-control field-mandatory form-select">
                                            <asp:ListItem Text="- - Please Select - -" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Quote/Policy" Value="POLICY"></asp:ListItem>
                                            <asp:ListItem Text="Client" Value="CLIENT"></asp:ListItem>
                                            <asp:ListItem Text="Claim" Value="CLAIM"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group form-group-sm">
                                        <asp:Label ID="lblSearchBy" runat="server" AssociatedControlID="ddlSearchBy" CssClass="control-label" Text="<%$ Resources:lbl_SearchBy %>"></asp:Label>
                                        <asp:DropDownList ID="ddlSearchBy" runat="server" EnableViewState="false" CausesValidation="false" ValidationGroup="QuickSearch" CssClass="form-control field-mandatory form-select">
                                            <asp:ListItem Text="- - Please Select - -" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <asp:LinkButton ID="btnAddTrasaction" runat="server" Text="<%$ Resources:lbl_Search %>" OnClientClick="setSelectedValue(); return false;" SkinID="btnPrimary"></asp:LinkButton>
                                    <asp:RequiredFieldValidator ID="rqdSearchFor" runat="server" ControlToValidate="txtSearchFor" ErrorMessage="<%$ Resources:lbl_ErrMsg_SearchFor %>" Display="none" SetFocusOnError="true" ValidationGroup="QuickSearch" Enabled="false"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rqdSearchType" runat="server" ControlToValidate="ddlSearchType" ErrorMessage="<%$ Resources:lbl_ErrMsg_SearchType %>" Display="none" SetFocusOnError="true" InitialValue="0" ValidationGroup="QuickSearch" Enabled="false"></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hfSelectedValue" runat="server"></asp:HiddenField>
                                    <asp:RequiredFieldValidator ID="rqdSearchBy" runat="server" ControlToValidate="ddlSearchBy" ErrorMessage="<%$ Resources:lbl_ErrMsg_SearchBy %>" Display="none" SetFocusOnError="true" InitialValue="0" ValidationGroup="QuickSearch" Enabled="false"></asp:RequiredFieldValidator>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="p-h-md p-v bg-light lt">
                </div>
            </div>
                </div>
            </div>
        
            
       
    </div>
</div>
