<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="PremiumConfirmation.aspx.vb" Inherits="Nexus.Modal_PremiumConfirmation"
    Title="Premium by Policy Section" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/CommissionUpdate.ascx" TagName="CommissionUpdate" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        function ReviseRate(dAmount, ctrlRevised) {
            if (IsNumeric(dAmount) == true) {
                ctrlRevised.value = parseFloat(dAmount);
            }
        }

        function Revise(dAmount, dOriginal, ctrlexcl) {

            if (dAmount != dOriginal) {
                ctrlexcl.setAttribute('Class', 'updated');
            }
        }
        function IsNumeric(sNumber) {
            var bIsValid = true;

            if (sNumber.length > 0) {
                for (i = 0; i < sNumber.length; i++) {
                    if (sNumber.charAt(i) == '+' || sNumber.charAt(i) == '-'
                        || sNumber.charAt(i) == '.' || sNumber.charAt(i) == ','
                        || parseInt(sNumber.charAt(i)) >= 0) {
                    }
                    else {
                        bIsValid = false;
                    }
                }
            }
            else {
                bIsValid = false;
            }

            return bIsValid;
        }

        function isInteger1(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            var ValidChars = "-0123456789.";
            var IsNumber = true;
            if (ValidChars.indexOf(keychar) == -1) {
                IsNumber = false;
            }
            return IsNumber;
        }
        function toggleScreens(flag) {
            if (flag) {
                document.getElementById('divPremiumConfirmation').style.display = 'none';
                document.getElementById('divReason').style.display = 'block';

            }
            else {
                document.getElementById('divPremiumConfirmation').style.display = 'block';
                document.getElementById('divReason').style.display = 'none';

            }
            return false;
        }
    </script>

    <div id="Modal_PremiumConfirmation">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div id="divPremiumConfirmation">
                    <asp:GridView ID="grdvPremiumConfirmation" runat="server" AutoGenerateColumns="false" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField ShowHeader="false" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicySectionCode" Visible="false" Text='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("COB_CODE").Value%>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_PolicySection %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicySection" Text='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("COB_DESCRIPTION").Value%>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_NetAnnualPremium %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNetAnnualPremium" Text='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("NET_ANNUAL_COMMISSION_OVERRIDEN").Value%>' onkeypress="javascript:return isInteger1(event);" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="lblOriginalNetPrem" Value='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("NET_ANNUAL_COMMISSION").Value%>' runat="server"></asp:HiddenField>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNetAnnualPremium" Text='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("NET_ANNUAL_COMMISSION_OVERRIDEN").Value%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="lblOriginalNetPrem" Value='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("NET_ANNUAL_COMMISSION").Value%>' runat="server"></asp:HiddenField>
                                    <img id="imgNetAnnualPremium" style="vertical-align: bottom;" visible="false" src="~/App_Themes/Internal/images/error-icon.gif" runat="server" alt="">
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalNetAnnualPremium" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_GrossAnnualPremium%>">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossAnnualPremium" Text='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("GROSS_ANNUAL_COMMISSION").Value%>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalGrossAnnualPremium" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_NetAPRP%>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNetAPRP" Text='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("NET_AP_RP_OVERRIDEN").Value%>' onkeypress="javascript:return isInteger1(event);" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="lblOriginalNetAPRP" Value='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("NET_AP_RP").Value%>' runat="server"></asp:HiddenField>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNetAPRP" Text='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("NET_AP_RP_OVERRIDEN").Value%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="lblOriginalNetAPRP" Value='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("NET_AP_RP").Value%>' runat="server"></asp:HiddenField>
                                    <img id="imgNetAPRP" style="vertical-align: bottom;" visible="false" src="~/App_Themes/Internal/images/error-icon.gif" runat="server" alt="">
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalNetAPRP" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_GrossAPRP%>">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossAPRP" Text='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("GROSS_AP_RP").Value%>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalGrossAPRP" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol class="list-inline no-margin">
                                            <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                <%-- <ol id='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("COB_DESCRIPTION").Value%>'>--%>
                                                <ol class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                    <li>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("COB_CODE").Value%>' CausesValidation="False" Text="<%$Resources:lbl_Edit%>">
                                                        </asp:LinkButton>
                                                    </li>
                                                </ol>
                                                <ol class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                    <li>
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Save" CommandArgument='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("COB_CODE").Value%>' CausesValidation="False" Text="<%$Resources:lbl_Save%>" Visible="false" OnClientClick="return toggleScreens(1);">
                                                        </asp:LinkButton>
                                                    </li>
                                                </ol>
                                                <ol class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                    <li>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" CommandArgument='<%# CType(Container.DataItem,System.Xml.Linq.XElement).Attribute("COB_CODE").Value%>' CausesValidation="False" OnClientClick="javascript:Page_ValidationActive = false;" Text="<%$Resources:lbl_Cancel%>" Visible="false">
                                                        </asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="false" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblOI" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("OI").Value%>' Visible="false" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div id="divTotals" runat="server" class="card-body clearfix">
                        <div class="bottom-corners">
                            <asp:Label ID="lblFinalPremium" runat="server" Text="<%$ Resources:lbl_FinalPremium%>" Font-Bold="true">
                            </asp:Label>
                            <table width="100%" cellpadding="0" class="modal-table" cellspacing="0">
                                <tr>
                                    <td width="70%">
                                        <asp:Label ID="lblPremium" runat="server" Text="<%$ Resources:lbl_Premium%>" AssociatedControlID="lblPremiumValue" Font-Bold="true">
                                        </asp:Label>
                                    </td>
                                    <td width="30%">
                                        <asp:Label ID="lblPremiumValue" runat="server">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="modal-table-left-cell">
                                        <asp:Label ID="lblIPT" runat="server" Text="<%$ Resources:lbl_IPT%>" AssociatedControlID="lblIPTValue" Font-Bold="true"> 
                                        </asp:Label>
                                    </td>
                                    <td class="modal-table-right-cell">
                                        <asp:Label ID="lblIPTValue" runat="server">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTotalPremium" runat="server" Text="<%$ Resources:lbl_TotalPremium%>" AssociatedControlID="lblTotalPremiumValue" Font-Bold="true">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalPremiumValue" runat="server">
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnExit" runat="server" Text="<%$ Resources:lbl_btnExit %>" UseSubmitBehavior="true" CausesValidation="false" OnClientClick="javascript:Page_ValidationActive = false;" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnDefault" runat="server" Text="<%$ Resources:lbl_btnDefault %>" UseSubmitBehavior="true" SkinID="btnPrimary"></asp:LinkButton>
                <uc1:CommissionUpdate ID="CommissionUpdate" CssClass="submit" runat="server"></uc1:CommissionUpdate>
            </div>
            <div id="divReason" style="display: none" title=" Reason For Change ">
                <div class="card" style="margin-left: 40px;">
                    <div class="card-body clearfix">

                        <h4>
                            <asp:Label ID="lblReason" runat="server" Text="<%$ Resources:lbl_txtReason %>"></asp:Label></h4>
                        <div class="form-horizontal">


                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" CssClass="field-mandatory multi-line form-control" ValidationGroup="vldGroupSave"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RqdReasonText" ControlToValidate="txtReason" runat="server" ErrorMessage="<%$ Resources:ReasonText_DescError %>" Display="none" SetFocusOnError="true" ValidationGroup="vldGroupSave"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                    </div>
                </div>
                <asp:ValidationSummary ID="vldsumSummary" DisplayMode="BulletList" ValidationGroup="vldGroupSave" HeaderText="<%$ Resources:vldsummaryReasonText%>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                <div class="card-footer">
                    <asp:LinkButton ID="btnApplyBack" runat="server" Text="<%$ Resources:lbl_btnApplyBack %>" UseSubmitBehavior="true" OnClientClick="return toggleScreens(0);" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnApplySave" runat="server" ValidationGroup="vldGroupSave" Text="<%$ Resources:lbl_btnApplySave %>" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
