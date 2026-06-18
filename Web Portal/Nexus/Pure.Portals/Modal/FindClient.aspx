<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindClient.aspx.vb" Inherits="Nexus.modal_FindClient"
    MasterPageFile="~/default.master" EnableViewState="false" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function CheckDuplicateRecord(src, args) {
            var sAssociateCollection = document.getElementById('<%=hdfAssociateKeys.ClientID%>').value.split("-");
            if (sAssociateCollection.length > 0) {


                for (var CountVar = 0; CountVar < sAssociateCollection.length; CountVar++) {
                    //Add Mode         
                    if (document.getElementById('<%=txtMode.ClientID %>').value == "Add") {
                        if (document.getElementById('<%=txtClient.ClientID%>').value == sAssociateCollection[CountVar].trim()) {
                            args.IsValid = false;
                        }
                    }
                    //Update Mode
                    if (document.getElementById('<%=txtMode.ClientID %>').value == "Update") {
                        if ((document.getElementById('<%=txtClient.ClientID%>').value == sAssociateCollection[CountVar].trim()) && (document.getElementById('<%=txtClient.ClientID%>').value != document.getElementById('<%=hdfAssociateKey.ClientID%>').value.trim())) {
                            args.IsValid = false;
                        }
                    }
                }

            }
        }
        function checkSameClient(src, args) {
            var sPartyCnt = document.getElementById('<%=hdfPartyCnt.ClientID%>').value;
            var sClientKey = document.getElementById('<%=hdfClientCnt.ClientID%>').value;
            if (sPartyCnt == sClientKey) {
                args.IsValid = false;
            }
        }


        function setAssociate(sClientName, sClientKey, sResolveName, sMode) {
            document.getElementById('<%=HidtxtClient.ClientID %>').value = sClientName;
            document.getElementById('<%=txtClient.ClientID %>').value = unescape(sClientName);

            document.getElementById('<%=AssociateKey.ClientID %>').value = sClientKey;
            document.getElementById('<%=AssociateName.ClientID %>').value = unescape(sResolveName);

            document.getElementById('<%=pnlAssociate.ClientID %>').style.display = "block";
            document.getElementById('<%=pnlSecondaryAssociate.ClientID %>').style.display = "none";
            document.getElementById('<%=hdfClientCnt.ClientID%>').value = sClientKey;

            if (sMode == "Add") {
                //Add Mode
                document.getElementById('<%=btnAddAssociate.ClientID %>').style.display = "inline-block";
                document.getElementById('<%=btnUpdateAssociate.ClientID %>').style.display = "none";
            }
            else {
                //Edit Mode
                document.getElementById('<%=btnAddAssociate.ClientID %>').style.display = "none";
                document.getElementById('<%=btnUpdateAssociate.ClientID %>').style.display = "inline-block";
            }
            checkSameClient();

        }
        function setPolicyAssociate(sClientName, sClientKey, sResolveName, sMode) {
            document.getElementById('<%=HidtxtClient.ClientID %>').value = sClientName;
            document.getElementById('<%=txtMainClient.ClientID %>').value = sClientName;

            document.getElementById('<%=AssociateKey.ClientID %>').value = sClientKey;
            document.getElementById('<%=AssociateName.ClientID %>').value = sResolveName;

            document.getElementById('<%=pnlSecondaryAssociate.ClientID %>').style.display = "block";
            if (sMode == "Add") {
                //Add Mode
                document.getElementById('<%=btnAddPolicyAssociate.ClientID %>').style.display = "inline-block";
            }
        }

        function UpdateAssociateData() {
            var AssociateData;
            var oDDL;
            //to Fire teh Client Validation first
            Page_ClientValidate("AssociateGroup");

            if (Page_IsValid == true) {
                //Mode
                AssociateData = document.getElementById('<%=txtMode.ClientID %>').value + ";";
                //Client Code
                AssociateData += document.getElementById('<%=txtClient.ClientID %>').value + ";";
                //Client Key
                AssociateData += document.getElementById('<%=AssociateKey.ClientID %>').value + ";";
                //Client Name
                AssociateData += document.getElementById('<%=AssociateName.ClientID %>').value + ";";
                //Relationship Code
                oDDL = document.getElementById('<%=GISAssociate_Relationship.ClientID %>');
                AssociateData += oDDL.options[oDDL.selectedIndex].value + ";";
                //Relationship Description
                AssociateData += oDDL.options[oDDL.selectedIndex].text + ";";
                //Key
                AssociateData += document.getElementById('<%=txtKey.ClientID %>').value + ";";

                self.parent.tb_remove();
                self.parent.ReceiveAssociateData(AssociateData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }
            else
                return false;
        }

        function Displaydetails() {
            window.onload = function () {
                document.getElementById('<%=pnlAssociate.ClientID %>').style.display = "none";
                document.getElementById('<%=pnlSecondaryAssociate.ClientID %>').style.display = "none";
                document.getElementById('<%=btnAddPolicyAssociate.ClientID %>').style.display = "none";

                var AssociateID = getQuerystring('AssociateID');
                var SecAssociateID = getQuerystring('SecondaryAssociate');
                if (AssociateID == '') {
                    document.getElementById('<%=btnAddAssociate.ClientID %>').style.display = "none";
                }
                else {
                    if (AssociateID != '') {
                        document.getElementById('<%=pnlAssociate.ClientID %>').style.display = "block";
                        //document.getElementById('<%=txtClientCode.ClientID %>').value=document.getElementById('<%=txtClient.ClientID %>').value;
                    }
                }
            }
        }

        function getQuerystring(key, default_) {
            if (default_ == null) default_ = "";
            key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
            var qs = regex.exec(window.location.href);
            if (qs == null)
                return default_;
            else
                return qs[1];

        }

        function pageLoad() {
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
        }

        function OnBeginRequest(sender, args) {
            var postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'hypSelect') {
                $get(uprogQuotes).style.display = "block";
            }
        }
        function checkTextAreaMaxLength(textBox, e, length) {
            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                    {
                        e.returnValue = false;
                        return false;
                    }
                    else//Firefox
                        e.preventDefault();
                }
            }
        }

        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 35 && e.keyCode != 36 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }
    </script>

    <asp:ScriptManager ID="smModelFindClient" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hdfAssociateKeys" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdfAssociateMode" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdfPartyCnt" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdfClientCnt" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdfPartyCorCnt" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdfAssociateKey" runat="server"></asp:HiddenField>
    <div id="Modal_FindClient">
        <asp:Panel ID="pnlFindBG" runat="server" CssClass="card" DefaultButton="btnSearch">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="litFindClientHeader" runat="server" Text="<%$ Resources:lbl_FindClient_header %>" EnableViewState="false"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label>
                    </legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClientName" runat="server" AssociatedControlID="txtClientName" Text="<%$ Resources:lblClientName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClientName" TabIndex="1" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClientCode" runat="server" AssociatedControlID="txtClientCode" Text="<%$ Resources:lblClientCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClientCode" TabIndex="2" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFileCode" runat="server" AssociatedControlID="txtFileCode" Text="<%$ Resources:lblFileCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFileCode" TabIndex="3" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" Text="<%$ Resources:lblAddress %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAddress" TabIndex="4" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" Text="<%$ Resources:lblPolicyNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyNumber" TabIndex="5" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyRiskIndex" runat="server" AssociatedControlID="txtPolicyRiskIndex" Text="<%$ Resources:lblPolicyRiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyRiskIndex" TabIndex="6" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimNumber" runat="server" AssociatedControlID="txtClaimNumber" Text="<%$ Resources:lblClaimNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimNumber" TabIndex="7" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimRiskIndex" runat="server" AssociatedControlID="txtClaimRiskIndex" Text="<%$ Resources:lblClaimRiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimRiskIndex" TabIndex="8" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" Text="<%$ Resources:lblPhone %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPhone" TabIndex="9" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDOB" runat="server" AssociatedControlID="txtDOB" Text="<%$ Resources:lblDOB %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtDOB" TabIndex="10" CssClass="field-medium form-control" runat="server"></asp:TextBox><uc1:CalendarLookup ID="calEventFromDate" runat="server" LinkedControl="txtDOB" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RangeValidator ID="rvDOB" runat="Server" Type="Date" Display="none" ControlToValidate="txtDOB" MinimumValue="01/01/1900" ErrorMessage="<%$ Resources:lbl_InvalidDOBFormat %>"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPostcode" runat="server" AssociatedControlID="txtPostcode" Text="<%$ Resources:lblPostcode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPostcode" TabIndex="11" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClientType" runat="server" AssociatedControlID="ddlClientType" Text="<%$ Resources:lbl_ClientType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlClientType" TabIndex="12" CssClass="field-medium form-control form-select" runat="server">
                                <asp:ListItem Value="ANY" Text="<%$ Resources:ddlClientType_ListItem1 %>"></asp:ListItem>
                                <asp:ListItem Value="PC" Text="<%$ Resources:ddlClientType_ListItem2 %>"></asp:ListItem>
                                <asp:ListItem Value="CC" Text="<%$ Resources:ddlClientType_ListItem3 %>"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="<%$ Resources:lbl_Status %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlStatus" TabIndex="13" CssClass="field-medium form-control form-select" runat="server">
                                <asp:ListItem Value="ANY" Text="<%$ Resources:ddlStatusType_ListItem1 %>"></asp:ListItem>
                                <asp:ListItem Value="CLIENT" Text="<%$ Resources:ddlStatusType_ListItem2 %>"></asp:ListItem>
                                <asp:ListItem Value="PROSPECT" Text="<%$ Resources:ddlStatusType_ListItem3 %>"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblIncludeClosedBranches" runat="server" AssociatedControlID="chkIncludeClosedBranches" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litIncludeClosedBranches" runat="server" Text="<%$ Resources:chkIncludeClosedBranches  %>"></asp:Literal></asp:Label><asp:CheckBox ID="chkIncludeClosedBranches" runat="server" TabIndex="14" Text=" " CssClass="asp-check"></asp:CheckBox>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnSearch" runat="server" TabIndex="15" Text="<%$ Resources:btnSearch %>" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </asp:Panel>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtClientName,txtClientCode,txtFileCode,txtAddress,txtPolicyNumber,txtPolicyRiskIndex,txtClaimNumber,txtClaimRiskIndex,txtPhone,txtPostcode" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:ValidationSummary ID="AssicateValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="AssociateGroup" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="PolicyAssociateGroup" CssClass="validation-summary" />
        <asp:UpdatePanel ID="UpSearchResults" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvSearchResults" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" OnPageIndexChanging="grdvSearchResults_PageIndexChanging" EmptyDataText="<%$ Resources:ErrorMessage %>" OnRowCommand="grdvSearchResults_RowCommand" DataKeyNames="UserName" EmptyDataRowStyle-CssClass="noData" OnDataBound="grdvSearchResults_DataBound">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:lblRegistration_Firstname_g %>"></asp:BoundField>
                            <asp:BoundField DataField="ResolvedName" HeaderText="<%$ Resources:lblBusiness_Lastname_g %>"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lblPhone_g %>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltPhone" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lblPostcode_g %>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltPostcode" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lblCustomerType_g %>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltCustomerType" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("UserName") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:HyperLink runat="server" ID="hypSelect" Text="<%$ Resources:btnSelect%>" SkinID="btnHGrid"></asp:HyperLink>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="UpdSearchReslts" OverlayCssClass="updating" AssociatedUpdatePanelID="UpSearchResults" runat="server">
            <progresstemplate>
            </progresstemplate>
        </Nexus:ProgressIndicator>
        <asp:Panel runat="server" ID="pnlAssociate" CssClass="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtMainClient" Text="<%$ Resources:lbl_ClientName%>" class=" col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClient" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAssociate" runat="server" AssociatedControlID="GISAssociate_Relationship" Text="<%$ Resources:lbl_PnlAssociate_Relationship%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISAssociate_Relationship" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Relationship_Type" CssClass="field-medium field-mandatory form-control form-select" DefaultText="(Please Select)"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdAssociate" runat="server" ControlToValidate="GISAssociate_Relationship" ErrorMessage="<%$ Resources:lbl_ErrMsg_Associate %>" Display="none" SetFocusOnError="true" Enabled="true" ValidationGroup="AssociateGroup"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="custValidator" runat="server" ErrorMessage="Associate already exits" Display="None" ValidationGroup="AssociateGroup" ClientValidationFunction="CheckDuplicateRecord"></asp:CustomValidator>
                        <asp:CustomValidator ID="cvPolicyAssociateExists" runat="server" Display="None" ValidationGroup="AssociateGroup" ErrorMessage="Duplicate Associate or Master Client as Associate cannot be created." ControlToValidate="txtClient" OnServerValidate="cvPolicyAssociateExists_ServerValidate"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstClientExists" runat="server" ErrorMessage="<%$ Resources:lbl_ClientExists_error %>" Display="None" ValidationGroup="AssociateGroup" ClientValidationFunction="checkSameClient"></asp:CustomValidator>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlSecondaryAssociate" CssClass="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblclient" runat="server" AssociatedControlID="txtMainClient" Text="<%$ Resources:lbl_ClientName%>" class=" col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtMainClient" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        <asp:CustomValidator ID="cvPolicyAssociate" runat="server" Display="None" ValidationGroup="PolicyAssociateGroup" ErrorMessage="Duplicate Policy Associate or Master Client as Associate cannot be created." ControlToValidate="txtMainClient" OnServerValidate="cvPolicyAssociate_ServerValidate"></asp:CustomValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAssociation" runat="server" AssociatedControlID="ddlAssociation" Text="<%$ Resources:lbl_Association%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlAssociation" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Association_Type" CssClass="field-medium field-mandatory form-control form-select" DefaultText="(Please Select)"></NexusProvider:LookupList>
                        </div>
                       
                        <asp:RequiredFieldValidator ID="rfvPolicyAssociate" runat="server" ControlToValidate="ddlAssociation" Enabled="false" ErrorMessage="<%$ Resources:lbl_ErrMsg_PolicyAssociate %>" Display="none" SetFocusOnError="true" ValidationGroup="PolicyAssociateGroup"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAssociationDetail" runat="server" AssociatedControlID="txtAssociationDetail" Text="<%$ Resources:lbl_AssociationDetail %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAssociationDetail" CssClass="form-control" TextMode="MultiLine" runat="server" Rows="5" Columns="80"  MaxLength="245" onkeyDown="return checkTextAreaMaxLength(this,event,'245');" ></asp:TextBox>
                            

                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <div class="card-footer">
            <asp:LinkButton ID="btnAddAssociate" runat="server" Text="<%$ Resources:lbl_btnAddAssociate %>" ValidationGroup="AssociateGroup" CausesValidation="true" SkinID="btnPrimary" OnClientClick="return UpdateAssociateData()" ></asp:LinkButton>
            <asp:LinkButton ID="btnUpdateAssociate" runat="server" Text="<%$ Resources:lbl_btnUpdateAssociate %>" ValidationGroup="AssociateGroup" CausesValidation="true" Style="display: none" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnAddPolicyAssociate" runat="server" Text="<%$ Resources:lbl_btnAddAssociate %>" ValidationGroup="PolicyAssociateGroup"  CausesValidation="true" SkinID="btnPrimary" />
        </div>
        <asp:HiddenField ID="HidtxtClient" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="AssociateKey" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="AssociateName" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtKey" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnDateAttached" runat="server" />
        <asp:HiddenField ID="hdnDateRemoved" runat="server" />
    </div>
</asp:Content>
