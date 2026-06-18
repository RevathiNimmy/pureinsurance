<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="UIIC_demo_Login" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body style="background-color: White">
    <form id="form1" runat="server">
        <div>
            <table width="100%" style="height: 100%" border="0">
                <tr style="height: 10%">
                    <td style="width: 10%" align="right">
                        <table width="100%">
                            <tr>
                                <td>
                                    <uc1:Header ID="Header1" runat="server" />
                                      </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="height: 90%">
                    <td>
                        <table border="0" width="100%" height="100%">
                            <tr style="height: 100%">
                                <td valign="TOP" style="width: 75%">
                                    <table style="font:Trebuchet MS">
                                        <tr valign="top">
                                            <td style="color:Gray;font:Trebuchet MS">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <h1 style="color:Red">
                                                                5</h1>
                                                        </td>
                                                        <td>
                                                            <h5 style="color: Teal">
                                                                Reasons why SSP is the right choice for your business.
                                                            </h5>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <marquee direction="UP" scrollamount="3" onmouseover="this.stop();" onmouseout="this.start();">
                                                            1. Preferred supplier to 20 of the top 50 global insurers.
                                                            <br />
                                                            <br />
                                                            2. Benefit more than 75 insurers worldwide as well as deploy & support solutions
                                                            in more than 50 countries.
                                                            <br />
                                                            <br />
                                                            3. Used by over 40,000 insurance & financial services professionals worldwide.
                                                            <br />
                                                            <br />
                                                            4. Focus on understanding clients needs to deliver world-class solutions & services
                                                            by employing 750+ of the most talented IT & business specialists in the insurance
                                                            industry.
                                                            <br />
                                                            <br />
                                                            5. Generate revenues exceeding £75m with operating profits of 20% & a recurring
                                                            income exceeding 60%.
                                                            <br />
                                                            </marquee>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:Image ID="imgmap" ImageUrl="~/Images/map.png" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" style="width: 25%">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top">
                                                &nbsp;<asp:Login ID="Login1" runat="server"  BackColor="#F7F6F3" BorderColor="#E6E2D8"
                                                    BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana"
                                                    Font-Size="0.8em" ForeColor="#333333">
                                                    <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
                                                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                                                    <TextBoxStyle Font-Size="0.8em" />
                                                    <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
                                                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
                                                    <LayoutTemplate>
                                                        <table border="0" cellpadding="4" cellspacing="0" style="border-collapse: collapse">
                                                            <tr>
                                                                <td>
                                                                    <table border="0" cellpadding="0">
                                                                        <tr>
                                                                            <td align="center" colspan="2" style="font-weight: bold; font-size: 0.9em; color: white;
                                                                                background-color: #5d7b9d">
                                                                                Log In</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width:101px">
                                                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" EnableTheming="False" Width="100px">User Name:</asp:Label></td>
                                                                            <td>
                                                                                <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em" EnableTheming="false"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 101px">
                                                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" EnableTheming="false">Password:</asp:Label></td>
                                                                            <td>
                                                                                <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password" EnableTheming="false" Width="100" ></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="height: 20px">
                                                                                <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" colspan="2" style="color: red">
                                                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" colspan="2">
                                                                                <asp:Button ID="LoginButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                                                                                    BorderStyle="Solid" BorderWidth="1px" CommandName="Login" Font-Names="Verdana"
                                                                                    Font-Size="0.8em" ForeColor="#284775" Text="Log In" ValidationGroup="Login1"
                                                                                    OnClick="LoginButton_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                </asp:Login>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
