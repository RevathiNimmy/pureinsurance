<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="OnlineCardPayment.aspx.vb" Inherits="Nexus.secure_payment_OnlineCardPayment" Title="Card Registration" EnableEventValidation="false" %>



<asp:Content ID="cntProgressBar" ContentPlaceHolderID="cntProgressBar" runat="Server">
</asp:Content>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <style type="text/css">
    .selected_row
    {
        background-color: #A1DCF2;
    }
</style>
    <script type="text/javascript">
        $(function () {
            
            $("[id*=grdCard] td").bind("click", function () {
                var row = $(this).parent();
                
                var rbs = row[0].cells['2'].childNodes['0'].nextSibling;
                if (rbs.checked) {
                    $("[id*=grdCard] tr").each(function () {
                        if ($(this)[0] != row[0]) {
                            $("td", this).removeClass("selected_row");
                        }
                    });
                    $("td", row).each(function () {

                        if (rbs.checked) {
                            $(this).addClass("selected_row");
                        } else {
                            $(this).removeClass("selected_row");
                        }
                    });
                }
            });
           $("[id*=grdCard] td").each(function () {
                var row = $(this).parent();

                var rbs = row[0].cells['2'].childNodes['0'].nextSibling;
                if (rbs.checked) {
                    $("[id*=grdCard] tr").each(function () {
                        if ($(this)[0] != row[0]) {
                            $("td", this).removeClass("selected_row");
                        }
                    });
                    $("td", row).each(function () {

                        if (rbs.checked) {
                            $(this).addClass("selected_row");
                        } else {
                            $(this).removeClass("selected_row");
                        }
                    });
                }
            });
        });
</script>
    <script type = "text/javascript">

        function RadioCheck(rb) {

            var gv = document.getElementById("<%=grdCard.ClientID%>");

        var rbs = gv.getElementsByTagName("input");



        var row = rb.parentNode.parentNode;

        for (var i = 0; i < rbs.length; i++) {

            if (rbs[i].type == "radio") {

                if (rbs[i].checked && rbs[i] != rb) {

                    rbs[i].checked = false;

                    break;

                }

            }

        }

        }
        function ValidateSelectedCard() {
            var gv = document.getElementById("<%=grdCard.ClientID%>");

            var rbs = gv.getElementsByTagName("input");
            for (var i = 0; i < rbs.length; i++) {

                if (rbs[i].type == "radio") {

                    if (rbs[i].checked) {

                        return true

                        break;

                    }

                }

            }
            alert("Please Select One Card");
            return false;
        }
        function ReceiveContactData(sContactData, sPostBackTo) {
            document.getElementById('<%=txtContactData.ClientID %>').value = sContactData;
             __doPostBack(sPostBackTo, 'UpdateContact');
         }


</script>
    <div id="secure_payment_OnlineCardPayment">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
         <div class="card-heading">
        <h1>
                <asp:Label ID="lblReceiptTypeHeading" runat="server" Text="<%$ Resources:lblHeaderPage %>"></asp:Label>
            </h1>
             </div>
        <asp:HiddenField ID="txtContactData" runat="server"></asp:HiddenField>
          <div class="grid-card table-responsive no-margin">
         <asp:GridView ID="grdCard" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="No Data found"  AllowPaging="False" AllowSorting="true" DataKeyNames="PartyBankKey" >

<Columns>

    
                                    <asp:BoundField DataField="Number" HeaderText="<%$ Resources:lblgrdHeader_CardNumber %>"></asp:BoundField>
                                    <asp:BoundField DataField="ExpiryDate" HeaderText="<%$ Resources:lblgrdHeader_ExpiryDate %>" ></asp:BoundField>
     <asp:BoundField DataField="NameOnCreditCard" HeaderText="Name" Visible="false"  ></asp:BoundField>
<asp:TemplateField>

<ItemTemplate>

    <asp:RadioButton ID="rdDefaultCard" runat="server"

        onclick = "RadioCheck(this);" Checked='<%#Eval("IsDefaultCreditCard")%>' />

    <asp:HiddenField ID="hdnTokenNo" runat="server"

        Value = '<%#Eval("TrackingNumber")%>' />
    <asp:HiddenField ID="hdnAuthCode" runat="server"

        Value = '<%#Eval("ManualAuthCode")%>' />
</ItemTemplate>

</asp:TemplateField>



</Columns>

</asp:GridView>
    </div>
       
         <asp:Panel ID="PanelButton" runat="server">
                
                <div class="card-footer">
                 
                    <asp:LinkButton ID="btnAddCard" runat="server" SkinID="btnSM" Text="<%$ Resources:btnAddCard %>"></asp:LinkButton>
                    <asp:LinkButton ID="btnProcess" runat="server" SkinID="btnSM" Text="<%$ Resources:btnProcess %>" OnClientClick="javascript:return ValidateSelectedCard()" ></asp:LinkButton>
                    <asp:LinkButton ID="btnOk" runat="server" SkinID="btnSM" Text="<%$ Resources:btnOk %>" ></asp:LinkButton>
                   
                </div>
            </asp:Panel>
        <h4 class="h4 m-v-lg TrnsCmplt">
                    
                    <asp:Literal ID="LblCardRegisterationFailed" runat="server"  />
                    
                               
                </h4>
                                               
                                       
    </div>
</asp:Content>
