<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MotorAddRisk.aspx.vb" Inherits="New_Business_MotorCar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script language="javascript" type="text/javascript">
// <!CDATA[


    
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
    
    
    
   
    
    function LoadWindows(url,width,height)
    {
     window.open(url,"","width=" + width+ ",height="+ height+",scrollbars=1")
//     if (opener != null)
//    {
//    opener.document.getElementById("gv").value = document.getElementById("txtShortName").value;
//    self.close();
//    
//    }
//    else
//    {      
//    location.href = "list Policy.aspx"
//    }
    
    
    }
    
 

// ]]>
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal">
                            <Items>
                                <asp:MenuItem Text="Cover Selection" Value="0"></asp:MenuItem>
                                <asp:MenuItem Text="Additional Cover" Value="2"></asp:MenuItem>
                                <asp:MenuItem Text="Additional Cover (Cont..)" Value="3"></asp:MenuItem>
                                <asp:MenuItem Text="Previous Insurance" Value="1"></asp:MenuItem>
                                <asp:MenuItem Text="Legal Liability" Value="4"></asp:MenuItem>
                                <asp:MenuItem Text="Claim History" Value="5"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                        <asp:Label ID="lblSamErrorMessage" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:MultiView ID="MCView" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vwCoverSelection" runat="server">
                                <asp:Panel ID="Panel1" runat="server" Width="100%">
                                    <table width="100%" border="0">
                                        <tr>
                                            <td colspan="2">
                                                <strong>Cover Selection</strong></td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%">
                                                Policy Start Date</td>
                                            <td style="width: 20%">
                                                <asp:TextBox ID="txtPolicyStartDate" runat="server" Enabled="False"></asp:TextBox></td>
                                            <td style="width: 15%">
                                                &amp;Time</td>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txtTime1" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Cover Note Date</td>
                                            <td>
                                                <asp:TextBox ID="txtCoverNoteDate" runat="server"></asp:TextBox></td>
                                            <td>
                                                &amp;Time</td>
                                            <td>
                                                <asp:TextBox ID="txtTime2" runat="server" Enabled="False"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Policy Type</td>
                                            <td>
                                                <asp:DropDownList ID="ddlPolicyType" runat="server">
                                                </asp:DropDownList></td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkRestrictedCover" runat="server" Text="Restricted Cover Selected"
                                                    Width="180px" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Cover Option Package</td>
                                            <td>
                                                <asp:DropDownList ID="ddlCoverOptionpackage" runat="server">
                                                </asp:DropDownList></td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkAddonCover" runat="server" Text="Add On Cover" Width="139px" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <strong>Vehicle Details</strong></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%">
                                                VehicleMake</td>
                                            <td style="width: 20%">
                                                <asp:DropDownList ID="ddlVehicleMake" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="width: 20%">
                                                Date of</td>
                                            <td style="width: 50%">
                                                <asp:DropDownList ID="ddlDateOf" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnFind" runat="server" Text="Find" CausesValidation="False" />&nbsp;<asp:Button
                                                    ID="btnVehicleOk" runat="server" Text="Ok" CausesValidation="False" /></td>
                                            <td>
                                                <asp:Button ID="btnClear" runat="server" Text="Clear" CausesValidation="False" /></td>
                                            <td>
                                                Date</td>
                                            <td>
                                                <asp:TextBox ID="txtDate" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel ID="pnlVehicleMake" runat="server" ScrollBars="Auto" Height="144px" Width="466px">
                                                    <asp:GridView ID="gvVehicleMake" runat="server" Width="453px">
                                                        <Columns>
                                                            <asp:CommandField ShowSelectButton="True" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Vehicle Model</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtVehicleModel" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVehicleModel"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td>
                                                <strong>Year of Manufacture(MM/YYYY)</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtYearManufacture" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtYearManufacture"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Body Type</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtBodyType" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBodyType"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td>
                                                <strong>Engine Number</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtEngineNumber" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtEngineNumber"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Seating Capacity</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtSeatingcapacity" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSeatingcapacity"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td>
                                                <strong>Chassis Number</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtChasisNumber" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtChasisNumber"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>CC</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtCC" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator45" runat="server" ControlToValidate="txtCC"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkNewVehicle" runat="server" Width="176px" Text="New Vehicle" /></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 26px">
                                                Vehicle Color</td>
                                            <td style="height: 26px">
                                                <asp:TextBox ID="txtVehicleColor" runat="server"></asp:TextBox></td>
                                            <td style="height: 26px">
                                                <strong>Registration Number</strong></td>
                                            <td style="height: 26px">
                                                <asp:TextBox ID="txtReg1" runat="server" Width="23px" EnableTheming="false" MaxLength="3"></asp:TextBox>
                                                <asp:TextBox ID="txtReg2" runat="server" Width="23px" EnableTheming="false" MaxLength="3"></asp:TextBox>
                                                <asp:TextBox ID="txtReg3" runat="server" Width="23px" EnableTheming="false" MaxLength="3"></asp:TextBox>
                                                <asp:TextBox ID="txtReg4" runat="server" Width="29px" EnableTheming="false" MaxLength="4"></asp:TextBox>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Ex Show Room Price</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtExShowRoom" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator50" runat="server" ControlToValidate="txtExShowRoom"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td>
                                                Trailer Details</td>
                                            <td>
                                                <asp:CheckBox ID="ChkTrailer" runat="server" Text="TrailerAttached" AutoPostBack="True" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>IDV</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtIDV" runat="server" AutoPostBack="true" Enabled="False"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator51" runat="server" ControlToValidate="txtIDV"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td>
                                                <strong>Number of&nbsp; Trailers</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtNumberoftrailers" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtNumberoftrailers"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                IDV System</td>
                                            <td>
                                                <asp:TextBox ID="txtIDVSystem" runat="server" Enabled="False"></asp:TextBox></td>
                                            <td>
                                                <strong>Trailers Register Number</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtTrailersRegisterNumber" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtTrailersRegisterNumber"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Zone</td>
                                            <td>
                                                <asp:TextBox ID="txtZone" runat="server"></asp:TextBox></td>
                                            <td>
                                                <strong>Trailers IDV?</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtTrailersIDV" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtTrailersIDV"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <strong>Vehicle Registration Address</strong></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Line 1</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtLine1" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLine1"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td>
                                                <asp:CheckBox ID="chkFinancierDetails" runat="server" Text="FinancierDetails" AutoPostBack="True" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Line 2</td>
                                            <td>
                                                <asp:TextBox ID="txtLine2" runat="server"></asp:TextBox></td>
                                            <td colspan="2" rowspan="5">
                                                <asp:Panel ID="pnlFinancierDetails1" Height="100%" runat="server" Width="620px" ScrollBars="Horizontal">
                                                    <asp:GridView ID="gvFinancierDetails" runat="server">
                                                        <Columns>
                                                            <asp:CommandField ShowSelectButton="True" />
                                                            <asp:CommandField ShowDeleteButton="True" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                                <asp:Button ID="btnaddFinancier" runat="server" Text="Add" CausesValidation="False" /><asp:Button
                                                    ID="btnFinancierEdit" runat="server" Text="Edit" Visible="False" CausesValidation="False" /><asp:Button
                                                        ID="btnDeleteFinancier" runat="server" Text="Delete" Visible="False" CausesValidation="False" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>City</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCity"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>State</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtState" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtState"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>PinCode</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtPinCode" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtPinCode"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>RTO Location</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtRTOLocation" runat="server" Enabled="False"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtRTOLocation"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel ID="pnlVehicleAddress" Height="200" runat="server" ScrollBars="auto">
                                                    <asp:GridView ID="gvVehicleAddress" runat="server">
                                                        <Columns>
                                                            <asp:CommandField ShowSelectButton="True" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnFindVehicleAddress" runat="server" Text="Find" CausesValidation="False" />
                                                &nbsp;<asp:Button ID="btnVehicleAddressClear" runat="server" Text="Clear" CausesValidation="False" />
                                                &nbsp;<asp:Button ID="btnVehicleAddressOk" runat="server" Text="Ok" CausesValidation="False" /></td>
                                            <td>
                                            </td>
                                            <td colspan="1" rowspan="1">
                                                &nbsp; &nbsp; &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="height: 21px">
                                            </td>
                                            <td colspan="2" rowspan="1">
                                                <asp:Panel ID="pnlfinancierDetails" runat="server" BackColor="lightblue">
                                                    <table style="width: 353px">
                                                        <tr>
                                                            <td colspan="2">
                                                                <strong>Financier Details</strong></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                Agreement Type</td>
                                                            <td style="width: 171px">
                                                                <asp:DropDownList ID="ddlagreementType" runat="server">
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                Financier Type</td>
                                                            <td style="width: 171px">
                                                                <asp:DropDownList ID="ddlFinancerType" runat="server">
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                Name of Financier</td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtNameofFinancier" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtNameofFinancier"
                                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <strong>Financier Address</strong></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                <strong>Line 1</strong></td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtFinancierLine1" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtFinancierLine1"
                                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                <strong>Line 2</strong></td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtFinancierLine2" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                <strong>City</strong></td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtFinancierCity" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtFinancierCity"
                                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                <strong>State</strong></td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtFinancerState" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtFinancerState"
                                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                <strong>Pincode</strong></td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtFinancierPincode" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtFinancierPincode"
                                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Button ID="btnFinancierOk" runat="server" Text="Ok" />
                                                                <asp:Button ID="btnfinanciercancel" runat="server" Text="Cancel" CausesValidation="False" /></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <%-- <table width="100%" style="border-style: solid" border="1">
                                        <tr>
                                            <td colspan="4">
                                                <strong>Vehicle Details</strong></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50%">
                                                VehicleMake</td>
                                            <td style="width: 20%">
                                                <asp:DropDownList ID="ddlVehicleMake" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="width: 20%">
                                                Date of</td>
                                            <td style="width: 10%">
                                                <asp:DropDownList ID="ddlDateOf" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnFind" runat="server" Text="Find" />&nbsp;<asp:Button ID="btnVehicleOk"
                                                    runat="server" Text="Ok" /></td>
                                            <td>
                                                <asp:Button ID="btnClear" runat="server" Text="Clear" /></td>
                                            <td>
                                                Date</td>
                                            <td>
                                                <asp:TextBox ID="txtDate" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel ID="pnlVehicleMake" runat="server" ScrollBars="Auto" Height="144px" Width="466px">
                                                    <asp:GridView ID="gvVehicleMake" runat="server" Width="453px">
                                                        <Columns>
                                                            <asp:CommandField ShowSelectButton="True" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td >
                                                Vehicle Model</td>
                                            <td >
                                                <asp:TextBox ID="txtVehicleModel" runat="server"></asp:TextBox></td>
                                            <td >
                                                Year of Manufacture(MM/YYYY)</td>
                                            <td >
                                                <asp:TextBox ID="txtYearManufacture" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td >
                                                Body Type</td>
                                            <td >
                                                <asp:TextBox ID="txtBodyType" runat="server"></asp:TextBox></td>
                                            <td >
                                                Engine Number</td>
                                            <td >
                                                <asp:TextBox ID="txtEngineNumber" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Seating Capacity</td>
                                            <td>
                                                <asp:TextBox ID="txtSeatingcapacity" runat="server"></asp:TextBox></td>
                                            <td>
                                                Chassis Number</td>
                                            <td>
                                                <asp:TextBox ID="txtChasisNumber" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                CC</td>
                                            <td>
                                                <asp:TextBox ID="txtCC" runat="server"></asp:TextBox></td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkNewVehicle" runat="server" Width="176px" Text="New Vehicle" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Vehicle Color</td>
                                            <td>
                                                <asp:TextBox ID="txtVehicleColor" runat="server"></asp:TextBox></td>
                                            <td>
                                                Registration Number</td>
                                            <td>
                                                <asp:TextBox ID="txtReg1" runat="server" Width="23px" EnableTheming="false"></asp:TextBox>
                                                <asp:TextBox ID="txtReg2" runat="server" Width="23px" EnableTheming="false"></asp:TextBox>
                                                <asp:TextBox ID="txtReg3" runat="server" Width="23px" EnableTheming="false"></asp:TextBox>
                                                <asp:TextBox ID="txtReg4" runat="server" Width="23px" EnableTheming="false"></asp:TextBox>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Ex Show Room Price</td>
                                            <td>
                                                <asp:TextBox ID="txtExShowRoom" runat="server"></asp:TextBox></td>
                                            <td>
                                                Trailer Details</td>
                                            <td>
                                                <asp:CheckBox ID="ChkTrailer" runat="server" Text="TrailerAttached" AutoPostBack="True" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                IDV</td>
                                            <td>
                                                <asp:TextBox ID="txtIDV" runat="server"></asp:TextBox></td>
                                            <td>
                                                Number of&nbsp; Trailers</td>
                                            <td>
                                                <asp:TextBox ID="txtNumberoftrailers" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                IDV System</td>
                                            <td>
                                                <asp:TextBox ID="txtIDVSystem" runat="server"></asp:TextBox></td>
                                            <td>
                                                Trailers Register Number</td>
                                            <td>
                                                <asp:TextBox ID="txtTrailersRegisterNumber" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Zone</td>
                                            <td>
                                                <asp:TextBox ID="txtZone" runat="server"></asp:TextBox></td>
                                            <td>
                                                Trailers IDV?</td>
                                            <td>
                                                <asp:TextBox ID="txtTrailersIDV" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                Vehicle Registration Address</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Line 1</td>
                                            <td>
                                                <asp:TextBox ID="txtLine1" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:CheckBox ID="chkFinancierDetails" runat="server" Text="FinancierDetails" AutoPostBack="True" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Line 2</td>
                                            <td>
                                                <asp:TextBox ID="txtLine2" runat="server"></asp:TextBox></td>
                                            <td>
                                            </td>
                                            <td colspan="1" rowspan="5">
                                                <asp:Panel ID="pnlFinancierDetails1" Width="700" Height="100%" runat="server" ScrollBars="Horizontal">
                                                    <asp:GridView ID="gvFinancierDetails" runat="server">
                                                        <Columns>
                                                            <asp:CommandField ShowSelectButton="True" />
                                                            <asp:CommandField ShowDeleteButton="True" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                City</td>
                                            <td style="width: 15px; height: 21px">
                                                <asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                State</td>
                                            <td>
                                                <asp:TextBox ID="txtState" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PinCode</td>
                                            <td style="width: 15px; height: 21px">
                                                <asp:TextBox ID="txtPinCode" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                RTO Location</td>
                                            <td style="width: 15px; height: 21px">
                                                <asp:TextBox ID="txtRTOLocation" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlVehicleAddress" Height="500%" runat="server" ScrollBars="auto">
                                                    <asp:GridView ID="gvVehicleAddress" runat="server">
                                                        <Columns>
                                                            <asp:CommandField ShowSelectButton="True" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnFindVehicleAddress" runat="server" Text="Find" /><asp:Button ID="btnVehicleAddressClear"
                                                    runat="server" Text="Clear" /><asp:Button ID="btnVehicleAddressOk" runat="server"
                                                        Text="Ok" /></td>
                                            <td colspan="2" rowspan="1">
                                                &nbsp;
                                                <asp:Button ID="btnaddFinancier" runat="server" Text="Add" /><asp:Button ID="btnFinancierEdit"
                                                    runat="server" Text="Edit" /><asp:Button ID="btnDeleteFinancier" runat="server" Text="Delete" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="height: 21px">
                                            </td>
                                            <td colspan="2" rowspan="1">
                                                <asp:Panel ID="pnlfinancierDetails" runat="server" BackColor="goldenrod">
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <strong>Financier Details</strong></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                Agreement Type</td>
                                                            <td style="width: 171px">
                                                                <asp:DropDownList ID="ddlagreementType" runat="server">
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                Financier Type</td>
                                                            <td style="width: 171px">
                                                                <asp:DropDownList ID="ddlFinancerType" runat="server">
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                Name of Financier</td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtNameofFinancier" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <strong>Financier Address</strong></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                Line 1</td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtFinancierLine1" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                Line 2</td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtFinancierLine2" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                City</td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtFinancierCity" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                State</td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtFinancerState" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 125px">
                                                                Pincode</td>
                                                            <td style="width: 171px">
                                                                <asp:TextBox ID="txtFinancierPincode" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Button ID="btnFinancierOk" runat="server" Text="Ok" />
                                                                <asp:Button ID="btnfinanciercancel" runat="server" Text="Cancel" /></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>--%>
                                </asp:Panel>
                            </asp:View>
                            <asp:View ID="VwPreviousInsurance" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkIsNCBApplicable" runat="server" Text="Is NCB Applicable?" /></td>
                                        <td style="width: 100px">
                                            Insurer Name</td>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtInsuredName" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 125px">
                                            Policy Number</td>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox></td>
                                        <td style="width: 100px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 125px; height: 21px;">
                                            Policy Type</td>
                                        <td style="width: 100px; height: 21px;">
                                            <asp:DropDownList ID="ddlPloicyType" runat="server">
                                            </asp:DropDownList></td>
                                        <td style="width: 100px; height: 21px;">
                                        </td>
                                        <td style="width: 100px; height: 21px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 125px">
                                            Policy Expiry Date</td>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtPolicyExpiryDate" runat="server"></asp:TextBox></td>
                                        <td colspan="3" rowspan="3">
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <strong>Insurer Address</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        Line 1</td>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="InsAddLine1" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        Line 2</td>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="InsAddLine2" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        City</td>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="InsAddCity" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        State</td>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="InsAddState" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        Pincode</td>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="InsAddPinCode" runat="server"></asp:TextBox></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 125px">
                                            Previous Year NCB</td>
                                        <td style="width: 100px">
                                            <asp:DropDownList ID="DDLPreviousYearNCB" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 125px">
                                            Document Proof</td>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtDocProof" runat="server" Height="110px" TextMode="MultiLine"
                                                Width="273px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" rowspan="1">
                                            <asp:CheckBox ID="chkIsVechilePrePolSold" runat="server" Text="Is Vechile of previous policy sold ?"
                                                Width="285px" />
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            <asp:Label ID="Label1" runat="server" Text="Vehicle Inspection Report" Width="140px"
                                                Font-Bold="True"></asp:Label></td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="width: 100%">
                                            <table id="Table2" onclick="return TABLE1_onclick()" width="100%">
                                                <tr>
                                                    <td style="width: 115px; height: 31px;">
                                                        <strong>Previous Policy Expiry Date</strong></td>
                                                    <td style="width: 100px; height: 31px;">
                                                        <asp:TextBox ID="txtPreviousPolicyExpiryDate" runat="server"></asp:TextBox></td>
                                                    <td rowspan="4" align="center">
                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                        <asp:TextBox ID="txtInspectionReport" runat="server" Height="106px" TextMode="MultiLine"
                                                            Width="345px" EnableTheming="False"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator48" runat="server" ControlToValidate="txtInspectionReport"
                                                            ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 31px;" colspan="2">
                                                        <asp:CheckBox ID="chkVehicleInspection" runat="server" Text="Vehicle Inspected" AutoPostBack="True" /></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 115px">
                                                        <strong>Inspection Date</strong></td>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="txtInspectionDate" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator46" runat="server" ControlToValidate="txtInspectionDate"
                                                            ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 115px; height: 43px;">
                                                        <strong>Inspected By Whom</strong></td>
                                                    <td style="width: 100px; height: 43px;">
                                                        <asp:TextBox ID="txtInspectedByWhom" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server" ControlToValidate="txtInspectedByWhom"
                                                            ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="width: 100px">
                                            <asp:CheckBox ID="chkIsPreviousInsuranceHistory" runat="server" Text="Previous Insurance History?"
                                                Width="221px" AutoPostBack="True" /></td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px; height: 59px;">
                                            Detailed &nbsp;Information &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px;">
                                            <asp:Panel ID="PreviousInsurancehistory1" runat="server" Width="750" ScrollBars="Horizontal">
                                                <asp:GridView ID="gvPreviousInsurancehistory" runat="server" Width="394px" Height="42px">
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:Button ID="btnAddPreviousInsurancehistory" runat="server" Text="Add" />
                                            <asp:Button ID="btnEditPreviousInsurancehistory" runat="server" Text="Edit" Visible="False" />
                                            <asp:Button ID="btnDeletePreviousInsurancehistory" runat="server" Text="delete" Visible="False"
                                                Style="z-index: 100; left: 90px; position: absolute; top: 2439px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlPIHistory" BackColor="lightblue" runat="server">
                                                <table style="width: 273px">
                                                    <tr>
                                                        <td colspan="2" style="height: 21px">
                                                            <strong>Personal Insurance History</strong></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 247px">
                                                            <strong>Policy Number</strong></td>
                                                        <td style="width: 284px">
                                                            <asp:TextBox ID="txtPIPolicyNumber" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="txtPIPolicyNumber"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 247px">
                                                            <strong>Number of Claims</strong></td>
                                                        <td style="width: 284px">
                                                            <asp:TextBox ID="txtNumberofClaims" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="txtNumberofClaims"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 247px">
                                                            <strong>Year</strong></td>
                                                        <td style="width: 284px">
                                                            <asp:TextBox ID="txtYear" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="txtYear"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 247px">
                                                            <strong>Total amount of Claims</strong></td>
                                                        <td style="width: 284px">
                                                            <asp:TextBox ID="txtTotalamountofclaims" runat="server"></asp:TextBox>
                                                            &nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ControlToValidate="txtTotalamountofclaims"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 247px">
                                                            <strong>Incurred Loss Ratio</strong></td>
                                                        <td style="width: 284px">
                                                            <asp:TextBox ID="txtIncurredLossRatio" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ControlToValidate="txtIncurredLossRatio"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 247px">
                                                            &nbsp;</td>
                                                        <td style="width: 284px">
                                                            <asp:Button ID="btnPIHistory" runat="server" Text="OK" />
                                                            <asp:Button ID="btnPIHistoryCancel" runat="server" Text="Cancel" CausesValidation="False" /></td>
                                                        <td style="width: 180px">
                                                        </td>
                                                        <td style="width: 282px">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vwAdditionalCover" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="chkElectrical" Text="Electric/Electronic accessories?" runat="server"
                                                AutoPostBack="True" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <strong>Details of Electrical accessories</strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="gvElectricalaccessories" runat="server">
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True" />
                                                    <asp:CommandField ShowDeleteButton="True" />
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Button ID="btnElectriaclAdd" runat="server" Text="Add" CausesValidation="False" />
                                            <asp:Button ID="btnEdit" runat="server" Text="Edit" Visible="False" CausesValidation="False" />
                                            <asp:Button ID="btndelete" runat="server" Text="delete" Visible="False" CausesValidation="False" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlElectrical" BorderColor="lightblue" runat="server">
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <strong>Electrical </strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            Serial Number</td>
                                                        <td style="width: 141px">
                                                            <asp:TextBox ID="txtelectricalSerialNumber" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            Product serial Number</td>
                                                        <td style="width: 141px">
                                                            <asp:TextBox ID="txtelecricalProductserialNumber" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            make</td>
                                                        <td style="width: 141px">
                                                            <asp:TextBox ID="txtelecricalmake" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            Sum Insured</td>
                                                        <td style="width: 141px">
                                                            <asp:TextBox ID="txtelecricalSumInsured" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            Description</td>
                                                        <td style="width: 141px">
                                                            <asp:TextBox ID="txtelecricalDescription" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="height: 26px">
                                                            <asp:Button ID="btnelectricalOk" runat="server" Text="Ok" CausesValidation="False" />
                                                            <asp:Button ID="btnelectricalcancel" runat="server" Text="cancel" CausesValidation="False" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 25px;">
                                            Total Sum Insured</td>
                                        <td style="width: 100px; height: 25px;">
                                            <asp:TextBox ID="txttotalSumInsured" runat="server" Enabled="False"></asp:TextBox></td>
                                        <td style="width: 100px; height: 25px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="chkNonElectricalAccessoriesCovered" Text="NonElectricalAccessoriesCovered"
                                                runat="server" AutoPostBack="True" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <strong>Details of Non Electrical accessories</strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlNonElecTrical1" runat="server" Width="900" ScrollBars="Horizontal">
                                                <asp:GridView ID="gvNonElecTrical" runat="server">
                                                    <Columns>
                                                        <asp:CommandField ShowDeleteButton="True" />
                                                        <asp:CommandField ShowSelectButton="True" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button ID="btnNonElectricalAdd" Text="Add" runat="server" CausesValidation="False" />
                                            <asp:Button ID="btnNonElectricalEdit" Text="Edit" runat="server" Visible="False"
                                                CausesValidation="False" /><asp:Button ID="btnNonElectricalDelete" Text="Delete"
                                                    runat="server" Visible="False" CausesValidation="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlNonelectrical" BackColor="lightblue" runat="server">
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <strong>Non Electrical </strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            Serial Number</td>
                                                        <td style="width: 141px">
                                                            <asp:TextBox ID="txtNonelectricalSerialNumber" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            Product serial Number</td>
                                                        <td style="width: 141px">
                                                            <asp:TextBox ID="txtNonelectricalProductserialNumber" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            make</td>
                                                        <td style="width: 141px">
                                                            <asp:TextBox ID="txtNonelectricalmake" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            Sum Insured</td>
                                                        <td style="width: 141px">
                                                            <asp:TextBox ID="txtNonelectricalSumInsured" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            Description</td>
                                                        <td style="width: 141px">
                                                            <asp:TextBox ID="txtNonelectricalDescription" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="height: 21px">
                                                            <asp:Button ID="btnNonelectricAdd" runat="server" Text="Ok" />
                                                            <asp:Button ID="btnNonElectricalCancel" runat="server" Text="cancel" CausesValidation="False" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 28px;">
                                            Total Sum Insured</td>
                                        <td style="width: 100px; height: 28px;">
                                            <asp:TextBox ID="txtSumInsurNonelec" runat="server" Enabled="False" /></td>
                                        <td style="width: 100px; height: 28px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="chkincludeDriverDetails" Text="Do you want to include driver details"
                                                runat="server" AutoPostBack="True" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <strong>Driver Details</strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlDriverdetails1" runat="server" ScrollBars="Horizontal" Width="900">
                                                <asp:GridView ID="gvDriverDetails" runat="server">
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button ID="btnaddDriver" Text="Add" runat="server" CausesValidation="False" /><asp:Button
                                                ID="btneditdriver" Text="Edit" runat="server" Visible="False" CausesValidation="False" /><asp:Button
                                                    ID="btndeletedriver" Text="delete" runat="server" Visible="False" CausesValidation="False" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlDriverDetails" runat="server" BackColor="lightblue">
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="3">
                                                            <strong>Driver Details</strong></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px; height: 18px">
                                                            vehicle Driven by?</td>
                                                        <td style="width: 226px; height: 18px">
                                                            <asp:DropDownList ID="ddlvehicledrivenby" runat="server">
                                                            </asp:DropDownList></td>
                                                        <td style="width: 100px; height: 18px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px; height: 22px;">
                                                            <asp:CheckBox ID="chkPaidDriver" runat="server" Text="Whether paid Driver is Called on Temporary Basis?"
                                                                Width="414px" /></td>
                                                        <td style="width: 226px; height: 22px;">
                                                        </td>
                                                        <td style="width: 100px; height: 22px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkVehicleDrivenBymorethanoneperson" Text="Is the vehicle Driven by more then one person?"
                                                                runat="server" Width="311px" />
                                                        </td>
                                                        <td style="width: 226px">
                                                        </td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            <strong>Driver name</strong></td>
                                                        <td>
                                                            <asp:TextBox ID="txtDrivername" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtDrivername"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            <strong>Age</strong></td>
                                                        <td>
                                                            <asp:TextBox ID="txtAge" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtAge"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            Gender</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlGender" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            Relationship with owner
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRelationshipwithowner" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            Driver's Educational Qualification</td>
                                                        <td style="width: 226px">
                                                            <asp:TextBox ID="txtDriverEduQualification" runat="server"></asp:TextBox></td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            <strong>Driver's Licence Number</strong></td>
                                                        <td style="width: 226px">
                                                            <asp:TextBox ID="txtDriversLicenceNumber" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtDriversLicenceNumber"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            Name of Driver's Licence Issue Authority</td>
                                                        <td style="width: 226px">
                                                            <asp:TextBox ID="txtDriversLicenceIssueAutority" runat="server"></asp:TextBox></td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            <strong>Driver's Licence Issue Date</strong></td>
                                                        <td style="width: 226px">
                                                            <asp:TextBox ID="txtDriversIssueDate" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtDriversIssueDate"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            <strong>Driver's Licence Expiry Date</strong></td>
                                                        <td style="width: 226px">
                                                            <asp:TextBox ID="txtDriversExpiryDate" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtDriversExpiryDate"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px; height: 21px;">
                                                            Driver's Experience (In Years)</td>
                                                        <td style="width: 226px; height: 21px;">
                                                            <asp:TextBox ID="txtDriversExperience" runat="server"></asp:TextBox></td>
                                                        <td style="width: 100px; height: 21px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px; height: 20px;">
                                                            Type of Licence</td>
                                                        <td style="width: 226px; height: 20px;">
                                                            <asp:DropDownList ID="ddlTypeoflicence" runat="server">
                                                            </asp:DropDownList></td>
                                                        <td style="width: 100px; height: 20px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            <asp:CheckBox ID="chkHasthedriverUndergone" runat="server" Text="Has the driver undergone defensive/advanced Training?" /></td>
                                                        <td style="width: 226px">
                                                        </td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                            Endorsements</td>
                                                        <td style="width: 226px">
                                                            <asp:TextBox ID="txtEndorsements" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 526px">
                                                        </td>
                                                        <td style="width: 226px">
                                                            <asp:Button ID="btnOkdriverDetails" runat="server" Text="Ok" /></td>
                                                        <td style="width: 100px">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="VwAdditionalCoverconst" runat="server">
                                <asp:Panel ID="Additional" runat="server" Height="435px" Width="100%">
                                    <table width="100%" style="border-style: solid">
                                        <tr>
                                            <td style="width: 298px">
                                                <strong>Additional Cover</strong>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 298px">
                                                Where is the vechicle Usally Parked?
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlVechicleParked" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 298px">
                                                What is the vechicle Mainly Used for?
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlVechiclemainlyused" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 298px">
                                                What is the approximate distance driven in a year?</td>
                                            <td>
                                                <asp:TextBox ID="txtApproximatedistance" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 298px">
                                                Type of Roads on which usally driven</td>
                                            <td>
                                                <asp:DropDownList ID="ddlusuallydriven" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" style="border-style: solid" id="TABLE1" onclick="return TABLE1_onclick()">
                                        <tr>
                                            <td style="width: 290px">
                                                <strong>Additional Cover</strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="height: 22px">
                                                <asp:CheckBox ID="chkBifuel" runat="server" Text="whether Bi-fuel kit inbuilt? "
                                                    AutoPostBack="True" />
                                            </td>
                                            <td style="width: 76px; height: 22px">
                                            </td>
                                            <td style="width: 120px; height: 22px">
                                            </td>
                                            <td style="height: 22px" colspan="2">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;<asp:CheckBox ID="chkLPGkit" runat="server" Text="Whether LPG kit? " Width="175px"
                                                    AutoPostBack="True" Font-Bold="False" />
                                            </td>
                                            <td style="width: 76px">
                                            </td>
                                            <td style="width: 120px">
                                            </td>
                                            <td style="width: 161px">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <strong>LPG SI</strong></td>
                                            <td style="width: 76px">
                                                <asp:TextBox ID="txtLPGSI" runat="server"></asp:TextBox></td>
                                            <td style="width: 120px">
                                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator49" runat="server" ControlToValidate="txtCNGSI"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large" Width="1px"></asp:RequiredFieldValidator></td>
                                            <td style="width: 161px">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkCNGSI" runat="server" Text="Whether CNG  kit? " Width="175px"
                                                    AutoPostBack="True" Font-Bold="False"></asp:CheckBox>
                                            </td>
                                            <td style="width: 76px">
                                            </td>
                                            <td style="width: 120px">
                                                &nbsp;</td>
                                            <td style="width: 161px">
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px; height: 26px;">
                                                <strong>CNG SI </strong>
                                            </td>
                                            <td style="width: 1px; height: 26px;">
                                            </td>
                                            <td style="width: 76px; height: 26px;">
                                                <asp:TextBox ID="txtCNGSI" runat="server"></asp:TextBox></td>
                                            <td style="width: 120px; height: 26px;">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtCNGSI"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td style="width: 161px; height: 26px;">
                                            </td>
                                            <td style="height: 26px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 41px" colspan="2">
                                                <asp:CheckBox ID="chkPermissionRTA" runat="server" Text="Do you Have permission for RTA? " />
                                            </td>
                                            <td style="width: 76px; height: 41px">
                                            </td>
                                            <td colspan="3" rowspan="1" style="height: 41px">
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px; height: 26px;">
                                                <asp:CheckBox ID="chkVechileusedforrallies" runat="server" Width="268px" Text="will the vehicle be used for Rallies? "
                                                    AutoPostBack="True" /></td>
                                            <td style="width: 1px; height: 26px;">
                                            </td>
                                            <td style="width: 76px; height: 26px;">
                                            </td>
                                            <td style="width: 120px; height: 26px;">
                                            </td>
                                            <td style="width: 161px; height: 26px;">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px; height: 21px;">
                                                <strong>StartDate</strong></td>
                                            <td style="width: 1px; height: 21px;">
                                            </td>
                                            <td style="width: 76px; height: 21px;">
                                                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="txtStartDate"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td colspan="1" style="width: 120px; height: 21px;">
                                            </td>
                                            <td colspan="2" style="height: 21px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px">
                                                <strong>End Date</strong></td>
                                            <td style="width: 1px">
                                            </td>
                                            <td style="width: 76px">
                                                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="txtEndDate"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td colspan="1" style="width: 120px">
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px">
                                                <asp:CheckBox ID="chkNonConvectionalPower" runat="server" Text="Does Your Vechicle have non Convectional Source of power?"
                                                    AutoPostBack="True" Width="318px" /></td>
                                            <td style="width: 1px">
                                            </td>
                                            <td style="width: 76px">
                                            </td>
                                            <td colspan="1" style="width: 120px">
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px">
                                                Description
                                            </td>
                                            <td style="width: 1px">
                                            </td>
                                            <td style="width: 76px">
                                                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></td>
                                            <td colspan="1" style="width: 120px">
                                            </td>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkGeographical" runat="server" Text=" Does You RequireExtension of geographical area?"
                                                    AutoPostBack="True" />
                                            </td>
                                            <td style="width: 76px">
                                                &nbsp;</td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkFiberGlassFuelTank" Text="Does the vehicle has FibreGlass fuel Tank"
                                                    runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 22px">
                                                <asp:CheckBox ID="chkBangaldesh" Text="Bangaldesh" runat="server" /></td>
                                            <td colspan="3" style="height: 22px">
                                                <asp:CheckBox ID="chkIshandicapped" Text="Is the Vechicle Specially Designed for handicapped user?"
                                                    runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 20px">
                                                <asp:CheckBox ID="chkPakistan" Text="Pakistan" runat="server" /></td>
                                            <td colspan="3" style="height: 20px">
                                                <asp:CheckBox ID="chkAntitheft" Text="Is Anti-theft device fiited?" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 5px">
                                                <asp:CheckBox ID="chkBhutan" Text="Bhutan" runat="server" /></td>
                                            <td colspan="3" style="height: 5px">
                                                <asp:CheckBox ID="chkForiegnEmbassy" Text="Does the vehicle belong to foreign Embassy?"
                                                    runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkMaldives" Text="Maldives" runat="server" /></td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkIsvehicleusedfordrivingtutions" Text="Is the Vechicle used for Driving Tutitions?"
                                                    runat="server" /></td>
                                            <td colspan="1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 22px">
                                                <asp:CheckBox ID="chkNepal" Text="Nepal" runat="server" /></td>
                                            <td colspan="3" style="height: 22px">
                                                <asp:CheckBox ID="chkOwnprmesis" Text="Is the vehicle used limited to own premises?"
                                                    runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkSrilanka" Text="Srilanka" runat="server" /></td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkcertifiedVintage" Text="Is it a certified vintage car?" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                            </td>
                                            <td colspan="3">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 17px">
                                                <asp:CheckBox ID="chkBrokerageAgencyCommission" Text="Brokerage/Agency Commission Discount"
                                                    runat="server" Width="342px" AutoPostBack="True" /></td>
                                            <td colspan="2" style="height: 17px">
                                                <asp:CheckBox ID="chkReliabilitytrails" Text="Reliability Trials" runat="server"
                                                    Width="135px" /></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px; height: 17px">
                                                <strong>Discount</strong></td>
                                            <td colspan="2" style="height: 17px">
                                                <asp:TextBox ID="txtDiscountAgentCommission" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtDiscountAgentCommission"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                            <td colspan="2" style="height: 17px">
                                                <asp:CheckBox ID="chkVoluntaryDeductible" runat="server" Text="Is Voluntary Deductible opted for?"
                                                    Width="271px" AutoPostBack="True"></asp:CheckBox></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 22px; width: 290px;">
                                                <asp:CheckBox ID="chkGoodfeatureDiscount" runat="server" Text="Good Features discount"
                                                    Width="271px" AutoPostBack="True"></asp:CheckBox></td>
                                            <td colspan="2" style="height: 22px">
                                            </td>
                                            <td style="width: 120px; height: 22px">
                                                Voluntary Excess amount</td>
                                            <td style="width: 161px; height: 22px">
                                                <asp:DropDownList ID="ddlVoluntaryExcessamount" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px; height: 17px">
                                                <strong>Discount</strong></td>
                                            <td colspan="2" style="height: 17px">
                                                <asp:TextBox ID="txtDiscountGoodfeature" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtDiscountGoodfeature"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 120px; height: 17px">
                                                <p class="MsoNormal" style="margin: 0in 0in 0pt">
                                                    Imposed Excess</p>
                                            </td>
                                            <td style="width: 161px; height: 17px">
                                                <asp:TextBox ID="txtImposedExcess" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px; height: 17px">
                                                <asp:CheckBox ID="chkSpecialDiscount" Text="SpecialDiscount" runat="server" AutoPostBack="True" /></td>
                                            <td colspan="2" style="height: 17px">
                                            </td>
                                            <td style="width: 120px; height: 17px">
                                                <p class="MsoNormal" style="margin: 0in 0in 0pt">
                                                    Compulsory Excess</p>
                                            </td>
                                            <td style="width: 161px; height: 17px">
                                                <asp:TextBox ID="txtCompulsoryExcess" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px; height: 17px">
                                                <strong>Discount</strong></td>
                                            <td colspan="2" style="height: 17px">
                                                <asp:TextBox ID="txtDiscountSpecial" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="txtDiscountSpecial"
                                                    ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 120px; height: 17px">
                                            </td>
                                            <td style="width: 161px; height: 17px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px; height: 17px">
                                                Detarriff Discount</td>
                                            <td colspan="2" style="height: 17px">
                                                <asp:DropDownList ID="ddlDetarriffDiscount" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 120px; height: 17px">
                                            </td>
                                            <td style="width: 161px; height: 17px">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:View>
                            <asp:View ID="LegalLiability" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <strong>Legal Liability</strong></td>
                                        <td colspan="1">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 657px">
                                            Number Of Paid drives/Cleaners(WorkMan) &nbsp;For &nbsp;Operations/Maintanace requiring
                                            to be covered?</td>
                                        <td style="width: 608px">
                                            <asp:TextBox ID="txtNumberOfPaiddrivesCleanersWorkMan" runat="server"></asp:TextBox></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 657px">
                                            Number OF Employee Travelling/Driving</td>
                                        <td style="width: 608px">
                                            <asp:TextBox ID="txtNumberOFEmployeeTravellingDriving" runat="server"></asp:TextBox></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 657px">
                                            <asp:CheckBox ID="chkDoyouwishtorestrictTPPDCovertostatutoryLimitofRs6000" runat="server"
                                                Text="Do you wish to restrict TPPD Cover to statutory Limit of Rs.6000?" /></td>
                                        <td style="width: 608px">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="4">
                                                        <strong>Personal Accident Details</strong></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkMeetscompulsoryownerdriverCompulsoryPACoverrequirements" runat="server"
                                                            Text="Meets compulsory owner?driver Compulsory PA Cover requirements?" AutoPostBack="True" /><br />
                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; -Driver
                                                        owns a valid licence<br />
                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;-Vechicle
                                                        owned Peivately
                                                    </td>
                                                    <td style="width: 200px">
                                                        <asp:CheckBox ID="chkThisvechcle" runat="server" Text="This vechcle?" AutoPostBack="True" /></td>
                                                    <td colspan="2">
                                                        (Only the one Vechicle Per Policy)</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 200px">
                                                        Sum Insured</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSumInsured1" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkPersonalaccidentcoverforunnamedpersons" runat="server" Text="Personal accident cover for unnamed persons?"
                                                            AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 40px">
                                                    </td>
                                                    <td style="width: 200px; height: 40px">
                                                        <strong>Number Of unnamed Persons</strong></td>
                                                    <td style="height: 40px">
                                                        <asp:TextBox ID="txtNumberOfunnamedPersons" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ControlToValidate="txtNumberOfunnamedPersons"
                                                            ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 200px">
                                                        <strong>Sum Insured Per Person</strong></td>
                                                    <td>
                                                        <asp:TextBox ID="txtSumInsuredPerPerson" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="txtSumInsuredPerPerson"
                                                            ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkDoyouwishtoincludePersonalaccidentforpaiddrivercleaner" runat="server"
                                                            Text="Do you wish to include  Personal accident for paid driver/cleaner?" AutoPostBack="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 200px">
                                                        Number Of&nbsp; paid drivers</td>
                                                    <td>
                                                        <asp:TextBox ID="txtNumberOfpaiddrivers" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 200px">
                                                        Sum Insured</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSumInsured2" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkDoyouwishtotakepersonalaccidentcoverfornamedpersons" runat="server"
                                                            Text="Do you wish to take personal accident cover  for named persons?" AutoPostBack="True" /></td>
                                                </tr>
                                            </table>
                                            <strong>Personal Accident Details for Named Person<br />
                                                <asp:Button ID="btnAddpersonalAccidentDetails" runat="server" Text="Add" CausesValidation="False" /></strong></td>
                                        <td colspan="1">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="personalAccidentDetails1" runat="server" Width="800">
                                                <asp:GridView ID="gvpersonalAccidentDetails" runat="server">
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            &nbsp;
                                            <asp:Button ID="btneditpersonalAccidentDetails" runat="server" Text="Edit" Visible="False"
                                                CausesValidation="False" />
                                            <asp:Button ID="btnDeletepersonalAccidentDetails" runat="server" Text="Delete" Visible="False"
                                                CausesValidation="False" />
                                        </td>
                                        <td colspan="1" style="height: 154px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlPersonalaccidentDetails" BackColor="lightblue" runat="server">
                                                <table>
                                                    <tr>
                                                        <td colspan="2" style="height: 21px">
                                                            <strong>Personal Accident Details For Named person</strong></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            Serial Number</td>
                                                        <td style="width: 231px">
                                                            <asp:TextBox ID="txtSerialnumber" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            <strong>Name</strong></td>
                                                        <td style="width: 231px">
                                                            <asp:TextBox ID="txtname" runat="server"></asp:TextBox>&nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="txtname"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px; height: 21px">
                                                            <strong>Sum Insured</strong></td>
                                                        <td style="width: 231px; height: 21px">
                                                            <asp:TextBox ID="txtsumInsured" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ControlToValidate="txtsumInsured"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            <strong>Nominee Name</strong></td>
                                                        <td style="width: 231px">
                                                            <asp:TextBox ID="txtNomineename" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtNomineename"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <strong>Nominee Asddress</strong></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            <strong>Line 1</strong></td>
                                                        <td style="width: 231px">
                                                            <asp:TextBox ID="txtPALine1" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtPALine1"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            <strong>Line 2</strong></td>
                                                        <td style="width: 231px">
                                                            <asp:TextBox ID="txtPALine2" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px">
                                                            <strong>City</strong></td>
                                                        <td style="width: 231px">
                                                            <asp:TextBox ID="txtPACity" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ControlToValidate="txtPACity"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px; height: 21px">
                                                            <strong>State</strong></td>
                                                        <td style="width: 231px; height: 21px">
                                                            <asp:TextBox ID="txtPAState" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ControlToValidate="txtPAState"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px; height: 21px">
                                                            <strong>PinCode</strong></td>
                                                        <td style="width: 231px; height: 21px">
                                                            <asp:TextBox ID="txtPAPin" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server" ControlToValidate="txtPAPin"
                                                                ErrorMessage="*" Font-Bold="True" Font-Size="Large"></asp:RequiredFieldValidator></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 207px; height: 21px">
                                                        </td>
                                                        <td style="width: 231px; height: 21px">
                                                            <asp:Button ID="btnOkPersonal" runat="server" Text="Ok" />
                                                            <asp:Button ID="btnCancelPersonal" runat="server" Text="Cancel" CausesValidation="False" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1" style="width: 657px">
                                            Total Sum Insured of All Named Persons</td>
                                        <td colspan="1" style="width: 657px">
                                            <asp:TextBox ID="txtTotalsuminsuredofallnamedpersons" runat="server" Enabled="False"></asp:TextBox></td>
                                        <td colspan="1" style="width: 608px">
                                        </td>
                                        <td colspan="2">
                                        </td>
                                        <td colspan="1">
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vwClaimHistory" runat="server">
                                <table>
                                    <tr>
                                        <td style="width: 287px">
                                            <asp:GridView ID="gvClaimHistory" runat="server">
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
                        </asp:MultiView><asp:Button ID="btnAddTask" runat="server" Text="AddTask" Visible="False" /><asp:Button
                            ID="btnOk" runat="server" Text="Ok" />&nbsp;&nbsp<asp:Button ID="btnCancel" runat="server"
                                Text="Cancel" CausesValidation="False" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
