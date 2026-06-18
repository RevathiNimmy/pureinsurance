<%@ page title="" language="VB" masterpagefile="~/default.master" autoeventwireup="false" codefile="ApproversComment.aspx.vb" inherits="secure_ApproversComment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptIncludes" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntLeftContent" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntProgressBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="script1" runat="server"></asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        function displayMessage(message) {
            alert(message)
        }
     
    </script>
    <div id="Modal_Viewallocation">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">

                    <div class="row ">
                        <div class="col-3 ">
                            <asp:Label ID="lbl_NewComments" style="margin-left: 15px; margin-top: 70px;" AssociatedControlID="txtNewComments" runat="server" Text="<%$ Resources:lblNewComments%>" class="col-md-3 col-sm-4 control-label"></asp:Label>
                        </div>
                        <div class="col-8 ">
                            <asp:TextBox ID="txtNewComments" runat="server" TextMode="MultiLine" MaxLength="180" style="margin-top: 20px; margin-bottom: 15px; height: 100px;" CssClass="form-control"></asp:TextBox>
                        </div>

                    </div>
                </div>
            </div>
            <div class="card-footer">
                <%--<asp:LinkButton ID="ViewBtn_Back" runat="server" Text="<%$ Resources:btnBack %>"  SkinID="btnSecondary"></asp:LinkButton>--%>
                <asp:LinkButton ID="btnOK" runat="server" TabIndex="5" Text="<%$ Resources:btnOK %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>

