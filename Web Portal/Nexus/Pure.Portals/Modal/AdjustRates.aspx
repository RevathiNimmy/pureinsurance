<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="AdjustRates.aspx.vb" Inherits="Nexus.Modal_AdjustRates" Title="Adjust Rates" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function ReviseRate(dAmount, dOriginal, ctrlexcl) {
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
                document.getElementById('divAdjustRate').style.display = 'none';
                document.getElementById('divReason').style.display = 'block';

            }
            else {
                document.getElementById('divAdjustRate').style.display = 'block';
                document.getElementById('divReason').style.display = 'none';

            }
            return false;
        }
    </script>

    <div id="Modal_AdjustRates">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div id="divAdjustRate">
                    <legend>
                        <asp:Label ID="lblHeader" runat="server" ForeColor="Black"></asp:Label></legend>
                    <div class="grid-card table-responsive">
                        <asp:GridView ID="grdvAdjustRates" CellPadding="0" CellSpacing="0" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRatingSection" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("DESCRIPTION").Value%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_BaseRate %>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBaseRate" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("PERCENT_BASE").Value%>' runat="server" onkeypress="javascript:return isInteger1(event);"></asp:TextBox>
                                        <asp:HiddenField ID="lblOriginalBaseRate" Value='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("PERCENT_ORIGINAL").Value%>' runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="lblBaseRatePrev" Value='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("PERCENT_BASE_PREVIOUS").Value%>' runat="server"></asp:HiddenField>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:lbl_RatingString%>">
                                    <ItemTemplate>

                                        <asp:Label ID="lblRatingString" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("RATING_STRING").Value%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_ApplicableRate %>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtApplicableRate" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("PERCENT_OVERRIDE").Value%>' runat="server" onkeypress="javascript:return isInteger1(event);"></asp:TextBox>
                                        <asp:HiddenField ID="lblOriginalAppRate" Value='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("PERCENT_APPLICABLE").Value%>' runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="lblApplicableRatePrev" Value='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("PERCENT_OVERRIDE_PREVIOUS").Value%>' runat="server"></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="false" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOI" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("OI").Value%>' Visible="false" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divTotals" runat="server" class="card">
                        <div class="card-body clearfix">
                            <table width="100%" cellpadding="0" class="modal-table" cellspacing="0">
                                <tr>
                                    <td width="70%">
                                        <asp:Label ID="lblTotal" runat="server" Text="<%$ Resources:lbl_Total%>" Font-Bold="true" Font-Italic="false">                                
                                        </asp:Label>
                                    </td>
                                    <td width="30%">
                                        <asp:Label ID="lblTotalApplicable" runat="server">                                
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="modal-table-left-cell">
                                        <asp:Label ID="lblSumInsured" runat="server" Text="<%$ Resources:lbl_SumInsured%>" Font-Bold="true" Font-Italic="false">                                    
                                        </asp:Label>
                                    </td>
                                    <td class="modal-table-right-cell">
                                        <asp:Label ID="lblTotalSumInsured" runat="server">                                
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPremium" runat="server" Text="<%$ Resources:lbl_Premium%>" Font-Bold="true" Font-Italic="false">                                    
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalPremium" runat="server">                                
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnBack" runat="server" Text="<%$ Resources:lbl_btnBack %>" UseSubmitBehavior="true" CausesValidation="false" OnClientClick="javascript:Page_ValidationActive = false;" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnSave" runat="server" Text="<%$ Resources:lbl_btnSave %>" UseSubmitBehavior="true" OnClientClick="return toggleScreens(1);" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnDefault" runat="server" Text="<%$ Resources:lbl_btnDefault %>" UseSubmitBehavior="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <div id="divReason" style="display: none" title=" Reason For Change ">
            <div class="card">
                <div class="card-heading">
                    <h1>
                        <asp:Label ID="lblReason" runat="server" Text="<%$ Resources:lbl_txtReason %>"></asp:Label></h1>
                </div>
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" CssClass="field-mandatory multi-line form-control" ValidationGroup="vldGroupSave"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RqdReasonText" ControlToValidate="txtReason" runat="server" ErrorMessage="<%$ Resources:ReasonText_DescError %>" Display="none" SetFocusOnError="true" ValidationGroup="vldGroupSave"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="card-footer">
                    <asp:LinkButton ID="btnApplySave" runat="server" Text="<%$ Resources:lbl_btnApplySave %>" ValidationGroup="vldGroupSave" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnApplyBack" runat="server" Text="<%$ Resources:lbl_btnApplyBack %>" UseSubmitBehavior="true" OnClientClick="return toggleScreens(0);" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vldsumSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:vldsummaryReasonText%>" runat="server" ValidationGroup="vldGroupSave" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
