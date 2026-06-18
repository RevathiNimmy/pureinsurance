<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="PremiumSummary.aspx.vb" Inherits="Nexus.Modal_PremiumSummary" Title="Premium Summary" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        function ReviseRate(dAmount, ctrlRevised) {
            if (IsNumeric(dAmount) == true) {
                ctrlRevised.value = parseFloat(dAmount);
            }
        }

        function ReviseAppRate(dAmount, dOriginal, ctrlexcl) {

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

        function toggleScreens(flag) {
            if (flag) {
                document.getElementById('divPremiumSummary').style.display = 'none';
                document.getElementById('divReason').style.display = 'block';

            }
            else {
                document.getElementById('divPremiumSummary').style.display = 'block';
                document.getElementById('divReason').style.display = 'none';

            }
            return false;
        }
    </script>

    <div id="Modal_PremiumSummary">






        <div class="card">
            <div id="divPremiumSummary">
                <div class="card-heading">
                    <h1>
                        <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal></h1>
                </div>
                <div class="card-body clearfix">
                    <asp:Repeater ID="rptPremiumSummary" runat="server">
                        <ItemTemplate>
                            <legend>
                                <asp:Label ID="lblHeader" runat="server" ForeColor="Black"></asp:Label></legend>
                            <div class="grid-card table-responsive">
                                <asp:GridView ID="grdvPremiumSummary" runat="server" AutoGenerateColumns="false" OnRowDataBound="grdvPremiumSummary_RowDataBound" ShowFooter="true">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRatingSectionGroups" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("LEVEL2").Value%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotals" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$Resources:lbl_SumInsured%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSumInsured" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("SUM_INSURED").Value%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblSumInsuredTotal" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$Resources:lbl_ApplicableRate%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApplicableRate" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("PERCENT_OVERRIDE").Value%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$Resources:lbl_NetAnnualPremium%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNetAnnualPremium" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("PREMIUM_OVERRIDE").Value%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblNetAnnualPremiumTotal" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$Resources:lbl_APRP%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNetAPRP" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("NET_AP_RP").Value%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblNetAPRPTotal" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <div class="rowMenu">
                                                    <ol id='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("LEVEL2").Value%>' class="list-inline no-margin">
                                                        <li>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CssClass="model" Text="<%$Resources:lbl_Edit%>" SkinID="btnGrid">
                                                            </asp:LinkButton>
                                                        </li>
                                                    </ol>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="lblOriginalBaseRate" Value='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("PERCENT_BASE").Value%>' runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="lblOriginalAppRate" Value='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("PERCENT_APPLICABLE").Value%>' runat="server"></asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div id="divTotals" runat="server" class="card-body clearfix">
                    <div class="form-horizontal">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblTotalPremium" runat="server" Text="<%$ Resources:lbl_TotalPremium %>" AssociatedControlID="lblTotalPremiumValue" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                            <asp:Label ID="lblTotalPremiumValue" runat="server" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <div class="card-footer">
                    <asp:LinkButton ID="btnExit" runat="server" Text="<%$ Resources:lbl_btnExit %>" UseSubmitBehavior="true" CausesValidation="false" OnClientClick="javascript:Page_ValidationActive = false;" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnDefault" runat="server" Text="<%$ Resources:lbl_btnDefault %>" UseSubmitBehavior="true" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
        </div>
        <div id="divReason" style="display: none" title=" Reason For Change ">
            <div class="card">
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <legend>
                            <asp:Label ID="lblReason" runat="server" Text="<%$ Resources:lbl_txtReason %>"></asp:Label></legend>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" CssClass="field-mandatory form-control" ValidationGroup="vldGroupSave"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RqdReasonText" ControlToValidate="txtReason" runat="server" ErrorMessage="<%$ Resources:ReasonText_DescError %>" Display="none" SetFocusOnError="true" ValidationGroup="vldGroupSave"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="card-footer">
                    <asp:LinkButton ID="btnApplyBack" runat="server" Text="<%$ Resources:lbl_btnApplyBack %>" UseSubmitBehavior="true" OnClientClick="return toggleScreens(0);" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnApplySave" runat="server" Text="<%$ Resources:lbl_btnApplySave %>" ValidationGroup="vldGroupSave" SkinID="btnPrimary"></asp:LinkButton>

                </div>
            </div>
            <asp:ValidationSummary ID="vldsumSummary" DisplayMode="BulletList" ValidationGroup="vldGroupSave" HeaderText="<%$ Resources:vldsummaryReasonText%>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
        </div>
    </div>
</asp:Content>
