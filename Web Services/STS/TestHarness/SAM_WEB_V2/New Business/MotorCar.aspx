<%@ Page Language="VB" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" CodeFile="MotorCar.aspx.vb" Inherits="New_Business_MotorCar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Menu  ID="Menu1" runat="server" Orientation="Horizontal">
                            <Items>
                                <asp:MenuItem Text="CoverSelection" Value="0"></asp:MenuItem>
                                <asp:MenuItem Text="PreviousInsurance" Value="1"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:MultiView ID="MCView" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vwCoverSelection" runat="server">
                                <asp:Panel ID="Panel1" runat="server" Height="435px" Width="100%">
                                    <table width="100%" style="border-style:solid">
                                        <tr>
                                            <td colspan="2">
                                                <strong>Cover Selection</strong></td>
                                            <td style="width: 66px">
                                            </td>
                                            <td style="width: 100px">
                                            </td>
                                            <td style="width: 172px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 165px">
                                                Policy Start Date</td>
                                            <td style="width: 100px">
                                                <asp:TextBox ID="txtPolicyStartDate" runat="server"></asp:TextBox></td>
                                            <td style="width: 66px">
                                                &nbsp;&amp;Time</td>
                                            <td style="width: 100px">
                                                <asp:TextBox ID="txtTime1" runat="server"></asp:TextBox></td>
                                            <td style="width: 172px">
                                                <asp:CheckBox ID="ChkRestrictedCover" runat="server" Text="Restricted Cover Selected" /></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 165px">
                                                Cover Note Date</td>
                                            <td style="width: 100px">
                                                <asp:TextBox ID="txtCoverNoteDate" runat="server"></asp:TextBox></td>
                                            <td style="width: 66px">
                                                &amp;Time</td>
                                            <td style="width: 100px">
                                                <asp:TextBox ID="txtTime2" runat="server"></asp:TextBox></td>
                                            <td style="width: 172px">
                                                <asp:CheckBox ID="chkAddonCover" runat="server" Text="Add On Cover" /></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 165px">
                                                Policy Type</td>
                                            <td style="width: 100px">
                                            </td>
                                            <td style="width: 66px">
                                            </td>
                                            <td style="width: 100px">
                                            </td>
                                            <td style="width: 172px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 165px">
                                                Cover Option Package</td>
                                            <td style="width: 100px">
                                                <asp:DropDownList ID="ddlCoverOptionpackage" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="width: 66px">
                                            </td>
                                            <td style="width: 100px">
                                            </td>
                                            <td style="width: 172px">
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" style="border-style:solid">
                                        <tr>
                                            <td colspan="4">
                                                <strong>Vehicle Details</strong></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">
                                                Vehicle Make</td>
                                            <td style="width: 56px">
                                                <asp:DropDownList ID="ddlVehicleMake" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="width: 84px">
                                                Date of</td>
                                            <td style="width: 100px">
                                                <asp:DropDownList ID="ddlDateOf" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Button ID="btnFind" runat="server" Text="Find" /></td>
                                            <td style="width: 56px">
                                                <asp:Button ID="btnClear" runat="server" Text="Clear" /></td>
                                            <td style="width: 84px">
                                                Date</td>
                                            <td style="width: 100px">
                                                <asp:TextBox ID="txtDate" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">
                                                Vehicle Model</td>
                                            <td style="width: 56px">
                                                <asp:TextBox ID="txtVehicleModel" runat="server"></asp:TextBox></td>
                                            <td style="width: 84px">
                                                Year of Manufacture(MM/YYYY)</td>
                                            <td style="width: 100px">
                                                <asp:TextBox ID="txtYearManufacture" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 21px">
                                                Body Type</td>
                                            <td style="width: 56px; height: 21px">
                                                <asp:TextBox ID="txtBodyType" runat="server"></asp:TextBox></td>
                                            <td style="width: 84px; height: 21px">
                                                Engine Number</td>
                                            <td style="width: 100px; height: 21px">
                                                <asp:TextBox ID="txtEngineNumber" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 40px">
                                                Seating Capacity</td>
                                            <td style="width: 56px; height: 40px">
                                                <asp:TextBox ID="txtSeatingcapacity" runat="server"></asp:TextBox></td>
                                            <td style="width: 84px; height: 40px">
                                                Chassis Number</td>
                                            <td style="width: 100px; height: 40px">
                                                <asp:TextBox ID="txtChasisNumber" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 22px">
                                                CC</td>
                                            <td style="width: 56px; height: 22px">
                                                <asp:TextBox ID="txtCC" runat="server"></asp:TextBox></td>
                                            <td style="width: 84px; height: 22px">
                                            </td>
                                            <td style="width: 100px; height: 22px">
                                                <asp:CheckBox ID="chkNewVehicle" runat="server" Width="176px" Text="New Vehicle" /></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 40px">
                                                Vehicle Color</td>
                                            <td style="width: 56px; height: 40px">
                                                <asp:TextBox ID="txtVehicleColor" runat="server"></asp:TextBox></td>
                                            <td style="width: 84px; height: 40px">
                                                Registration Number</td>
                                            <td >
                                                <asp:TextBox ID="txtReg1" runat="server" Width="23px" EnableTheming="false"></asp:TextBox>
                                                <asp:TextBox ID="txtReg2" runat="server" Width="23px" EnableTheming="false"></asp:TextBox>
                                                <asp:TextBox ID="txtReg3" runat="server" Width="23px" EnableTheming="false"></asp:TextBox>
                                                <asp:TextBox ID="txtReg4" runat="server" Width="23px" EnableTheming="false"></asp:TextBox>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 21px">
                                                Ex Show Room Price</td>
                                            <td style="width: 56px; height: 21px">
                                                <asp:TextBox ID="txtExShowRoom" runat="server"></asp:TextBox></td>
                                            <td style="width: 84px; height: 21px">
                                                Trailer Details</td>
                                            <td style="width: 100px; height: 21px">
                                                <asp:CheckBox ID="ChkTrailer" runat="server" Text="TrailerAttached" /></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 21px">
                                                IDV</td>
                                            <td style="width: 56px; height: 21px">
                                                <asp:TextBox ID="txtIDV" runat="server"></asp:TextBox></td>
                                            <td style="width: 84px; height: 21px">
                                            </td>
                                            <td style="width: 100px; height: 21px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 21px">
                                                IDV System</td>
                                            <td style="width: 56px; height: 21px">
                                                <asp:TextBox ID="txtIDVSystem" runat="server"></asp:TextBox></td>
                                            <td style="width: 84px; height: 21px">
                                            </td>
                                            <td style="width: 100px; height: 21px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 21px">
                                                Zone</td>
                                            <td style="width: 56px; height: 21px">
                                                <asp:TextBox ID="txtZone" runat="server"></asp:TextBox></td>
                                            <td style="width: 84px; height: 21px">
                                            </td>
                                            <td style="width: 100px; height: 21px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="height: 21px">
                                                Vehicle Registration Address</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 21px">
                                                Line 1</td>
                                            <td style="width: 56px; height: 21px">
                                                <asp:TextBox ID="txtLine1" runat="server"></asp:TextBox></td>
                                            <td style="width: 84px; height: 21px">
                                                Financier Details</td>
                                            <td style="width: 100px; height: 21px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 23px">
                                                Line 2</td>
                                            <td style="width: 56px; height: 23px">
                                                <asp:TextBox ID="txtLine2" runat="server"></asp:TextBox></td>
                                            <td colspan="2" rowspan="5">
                                                <asp:GridView ID="GridView1" runat="server">
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 21px">
                                                City</td>
                                            <td style="width: 56px; height: 21px">
                                                <asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 21px">
                                                State</td>
                                            <td style="width: 56px; height: 21px">
                                                <asp:TextBox ID="txtState" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 21px">
                                                PinCode</td>
                                            <td style="width: 56px; height: 21px">
                                                <asp:TextBox ID="txtPinCode" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 21px">
                                            </td>
                                            <td style="width: 56px; height: 21px">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                            </asp:View>
                        </asp:MultiView>
                        <asp:Button ID="btnAddTask" runat="server" Text="AddTask" />&nbsp;
                        <asp:Button ID="btnOk" runat="server" Text="Ok" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
