<%@ Page Language="VB" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="MotorAddRiskQQ.aspx.vb" Inherits="New_Business_MotorAddRiskQQ" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
       <script language="javascript" type="text/javascript">
    
     function IsNumFieldKeyPress()
{
  var element;
  
  element = event.srcElement;

  if ( event.keyCode >= 48 && event.keyCode <= 57 )
  {
    event.returnValue = true;
    return;
  }
  else if (event.keyCode == 46)
  {
    if ( element.value.indexOf('.', 0) == -1 )
    {
      event.returnValue = true;
      return;
    }      
  }
  event.returnValue = false;
}
    
    
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <uc1:Header ID="Header1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal">
                            <Items>
                                <asp:MenuItem Text="Quick Quote" Value="0"></asp:MenuItem>
                                <asp:MenuItem Text="Claim History" Value="1"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                        &nbsp;&nbsp;
                        <asp:Label ID="lblSamErrorMessage" runat="server"></asp:Label>
                        <asp:MultiView ActiveViewIndex="0" ID="MultiView1" runat="server">
                            <asp:View ID="QuickQuote" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <strong>Quick Quote - Rating Criteria</strong></td>
                                        <td style="width: 209px">
                                        </td>
                                        <td style="width: 87px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 21px;" colspan="2">
                                            <asp:CheckBox ID="chkswaptofullquote" runat="server" Text="Swap To Full Quote" Width="167px"
                                                AutoPostBack="True" Visible="False" /></td>
                                        <td colspan="2" style="height: 21px">
                                            <asp:CheckBox ID="chkNewVechicle" runat="server" Text="New Vechicle?" /></td>
                                        <td style="width: 100px; height: 21px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 248px">
                                        </td>
                                        <td style="width: 284px">
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkRenewalofExistingPolicy" runat="server" Text="Renewal of Existing Policy?" /></td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 248px">
                                        </td>
                                        <td style="width: 284px">
                                        </td>
                                        <td style="width: 209px">
                                            Vechicle Registered in Individual or Corparate Name?</td>
                                        <td style="width: 87px">
                                            <asp:DropDownList ID="ddlVechicleRegisteredinIndividualorCorparateName" runat="server">
                                            </asp:DropDownList></td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 248px">
                                        </td>
                                        <td style="width: 284px">
                                        </td>
                                        <td style="width: 209px" align="right">
                                            Year Of Manufacture</td>
                                        <td style="width: 87px">
                                            <asp:TextBox ID="txtYearOfManufacture" runat="server"></asp:TextBox></td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Vechicle Make</strong></td>
                                        <td style="width: 284px">
                                            <asp:TextBox ID="txtVechicleMake" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVechicleMake"
                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        <td align="right" style="height: 17px">
                                            <strong>Date of First Registration</strong></td>
                                        <td>
                                            <asp:TextBox ID="txtDateofFirstRegistration" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDateofFirstRegistration"
                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Vechicle Model</strong></td>
                                        <td style="width: 284px">
                                            <asp:TextBox ID="txtVechicleModel" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtVechicleModel"
                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        <td align="right">
                                            Cover Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCoverType" runat="server">
                                            </asp:DropDownList></td>
                                        <td style="width: 100px; height: 14px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 248px; height: 7px">
                                            Body Type</td>
                                        <td style="width: 284px; height: 7px;">
                                            <asp:TextBox ID="txtBodyTypeQQ" runat="server"></asp:TextBox></td>
                                        <td style="width: 209px; height: 7px;" align="right">
                                            <strong>RTO PIN Code</strong></td>
                                        <td style="width: 87px; height: 7px;">
                                            <asp:TextBox ID="txtRTOPINCode" runat="server"></asp:TextBox></td>
                                        <td style="width: 100px; height: 7px;">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtRTOPINCode"
                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 248px; height: 2px">
                                            <strong>Seating Capacity</strong></td>
                                        <td style="width: 284px; height: 2px;">
                                            <asp:TextBox ID="txtSeatingCapacityQQ" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSeatingCapacityQQ"
                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        <td style="width: 209px; height: 2px;" align="right">
                                            <strong>Vechicle Reg state</strong></td>
                                        <td style="width: 87px; height: 2px;">
                                            <asp:TextBox ID="txtVechicleRegstate" runat="server"></asp:TextBox></td>
                                        <td style="width: 100px; height: 2px;">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtVechicleRegstate"
                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 248px; height: 3px">
                                            <strong>Cubicle Capacity</strong></td>
                                        <td style="width: 284px; height: 3px;">
                                            <asp:TextBox ID="txtCubicleCapacity" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCubicleCapacity"
                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        <td style="width: 209px; height: 3px;" align="right">
                                            <strong>Vechicle Zone</strong></td>
                                        <td style="width: 87px; height: 3px;">
                                            <asp:TextBox ID="txtVechicleZone" runat="server"></asp:TextBox></td>
                                        <td style="width: 100px; height: 3px;">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtVechicleZone"
                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 248px">
                                            <asp:Button ID="btnfindQQ1" runat="server" Text="Find" CausesValidation="False" />
                                            <asp:Button ID="btnCancleQQ1" runat="server" Text="Clear" CausesValidation="False" />
                                            <asp:Button ID="BtnOkVD" runat="server" Text="Ok" CausesValidation="False" /></td>
                                        <td style="width: 284px; height: 15px;">
                                        </td>
                                        <td style="width: 209px; height: 15px;" align="right">
                                            <asp:Button ID="btnFindQQ" runat="server" Text="Find" CausesValidation="False" /><asp:Button
                                                ID="btnCancleQQ" runat="server" Text="Clear" CausesValidation="False" />
                                            <asp:Button ID="Button1" runat="server" Text="Ok" CausesValidation="False" /></td>
                                        <td style="width: 87px; height: 15px;">
                                        </td>
                                        <td style="width: 100px; height: 15px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlVehicleDetails" Visible="false" runat="server" Height="100px" Width="425px"
                                                ScrollBars="Vertical">
                                                <asp:GridView ID="gvVehicleDetails" runat="server" Width="425px">
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:HiddenField ID="hdnFC1" runat="server" />
                                        </td>
                                        <td colspan="2" align="right">
                                            <asp:Panel ID="pnlVechicleZone" runat="server" Visible="false" Height="100px" Width="400px"
                                                ScrollBars="Vertical">
                                                <asp:GridView ID="GvVechicleZone" runat="server" Width="400px">
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:HiddenField ID="hdnFC2" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 248px" align="right">
                                            Ex Show Room Price</td>
                                        <td style="width: 284px">
                                            <asp:TextBox ID="txtExShowRoomPrice" runat="server"></asp:TextBox></td>
                                        <td style="width: 209px">
                                        </td>
                                        <td style="width: 87px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 248px" align="right">
                                            Insured Declared Value</td>
                                        <td style="width: 284px">
                                            <asp:TextBox ID="txtInsuredDeclaredValue" runat="server"></asp:TextBox></td>
                                        <td style="width: 209px">
                                        </td>
                                        <td style="width: 87px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 248px">
                                        </td>
                                        <td style="width: 284px">
                                        </td>
                                        <td style="width: 209px">
                                        </td>
                                        <td style="width: 87px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <strong>Discounts</strong></td>
                                        <td style="width: 209px">
                                        </td>
                                        <td style="width: 87px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 248px">
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkAutomobileAssociationMembership" runat="server" Text="Automobile Association Membership"
                                                Width="287px" /></td>
                                        <td style="width: 87px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 248px">
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkARAIApprovedAntiTheftDeviceInstalled" runat="server" Text="ARAI- Approved Anti Theft Device Installed"
                                                Width="310px" /></td>
                                        <td style="width: 87px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 248px">
                                        </td>
                                        <td style="width: 284px" align="right">
                                            Volutary Dedecutible
                                        </td>
                                        <td style="width: 209px">
                                            <asp:DropDownList ID="ddlVolutaryDedecutible" runat="server">
                                            </asp:DropDownList></td>
                                        <td style="width: 87px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 248px">
                                            <asp:CheckBox ID="chkAddOnCovers" runat="server" Text="Add On Covers?" Width="217px"
                                                AutoPostBack="True" /></td>
                                        <td colspan="2">
                                        </td>
                                        <td style="width: 87px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Panel Visible="false" ID="pnlAddOnCovers" runat="server" Height="75px" Width="100%">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td style="width: 305px">
                                                            <asp:CheckBox ID="chkLegalLiablityforpaidDriver" runat="server" Text="Legal Liablity for paid Driver"
                                                                Width="310px" /></td>
                                                        <td style="width: 71px">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 136px">
                                                        </td>
                                                        <td  align="right" style="width: 305px">
                                                            Electical/Electronic &nbsp; Accessories</td>
                                                        <td style="width: 71px">
                                                            <asp:TextBox ID="txtElecticalElectronicAccessories" runat="server"></asp:TextBox></td>
                                                        <td style="width: 126px">
                                                        </td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 136px">
                                                        </td>
                                                        <td style="width: 305px" align="right">
                                                            NonElectical &nbsp; Accessories</td>
                                                        <td style="width: 71px">
                                                            <asp:TextBox ID="txtNonElecticalAccessories" runat="server"></asp:TextBox></td>
                                                        <td style="width: 126px">
                                                        </td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 136px; height: 5px;">
                                                        </td>
                                                        <td colspan="2" style="height: 5px">
                                                            <asp:CheckBox ID="chkPACoveredforunnamedpersons" runat="server" Text="PA Covered for unnamed persons"
                                                                Width="310px" AutoPostBack="True" /></td>
                                                        <td colspan="2" rowspan="2">
                                                            &nbsp;<asp:Panel Visible="false" ID="pnlPACoveredforunnamedpersons" runat="server"
                                                                Height="50px" Width="300px">
                                                                <table width="300">
                                                                    <tr>
                                                                        <td style="width: 100px; height: 24px;">
                                                                            <strong>No Of Person</strong></td>
                                                                        <td style="width: 100px; height: 24px;">
                                                                            <asp:TextBox ID="txtNoOfPerson" runat="server" AutoPostBack="True"></asp:TextBox>&nbsp;
                                                                        </td>
                                                                        <td style="width: 100px; height: 24px">
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*"
                                                                                Font-Bold="True" Font-Size="Large" ControlToValidate="txtNoOfPerson"></asp:RequiredFieldValidator></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100px; height: 5px;">
                                                                            <strong>Sum Insured Per Person</strong></td>
                                                                        <td style="width: 100px; height: 5px;">
                                                                            <asp:TextBox ID="txtSumInsuredPerPersonQQ" runat="server" AutoPostBack="True"></asp:TextBox>&nbsp;
                                                                        </td>
                                                                        <td style="width: 100px; height: 5px">
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*"
                                                                                Font-Bold="True" Font-Size="Large" ControlToValidate="txtSumInsuredPerPersonQQ"></asp:RequiredFieldValidator></td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 136px; height: 2px">
                                                        </td>
                                                        <td style="width: 305px; height: 2px;">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vwClaimHistory" runat="server">
                                <table>
                                    <tr>
                                        <td style="width: 287px">
                                            <asp:GridView ID="gvClaimHistory" runat="server">
                                                <Columns>
                                                    <asp:CommandField ShowEditButton="True" />
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Button ID="btnClaimHistoryAdd" runat="server" Text="Add" Enabled="false" /><asp:Button
                                                ID="btnClaimHistoryEdit" runat="server" Text="Edit" Enabled="false" /><asp:Button
                                                    ID="btnClaimHistoryDelete" runat="server" Enabled="false" Text="Delete" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 287px">
                                            <asp:Panel ID="pnlClaimHistory" runat="server">
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <strong>Specific Claim details</strong></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px">
                                                            Claim Number</td>
                                                        <td style="width: 100px">
                                                            <asp:TextBox ID="txtClaimNumber" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px">
                                                            Claim Status</td>
                                                        <td style="width: 100px">
                                                            <asp:TextBox ID="txtClaimStatus" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px">
                                                            Policy Number</td>
                                                        <td style="width: 100px">
                                                            <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px">
                                                            Loss Time</td>
                                                        <td style="width: 100px">
                                                            <asp:TextBox ID="txtlossTime" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px">
                                                            Loss Date</td>
                                                        <td style="width: 100px">
                                                            <asp:TextBox ID="txtLossDate" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px">
                                                            Cause</td>
                                                        <td style="width: 100px">
                                                            <asp:TextBox ID="txtCause" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px">
                                                            <asp:Button ID="btnClaimHistoryOk" runat="server" Text="Ok" CausesValidation="False" /></td>
                                                        <td style="width: 100px">
                                                            <asp:Button ID="txtClaimHistoryCancel" runat="server" Text="Cancel" CausesValidation="False" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        
                        </asp:MultiView>
                        <asp:Button ID="btnAddTask" runat="server" Text="AddTask" Visible="False" />&nbsp;
                        <asp:Button ID="btnOk" runat="server" Text="Ok" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" /></td>
                </tr>
            </table>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False" />
    </form>
</body>
</html>
