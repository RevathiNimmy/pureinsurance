<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    
<head runat="server">
    <title>XMLGenerator</title>
   
</head>
<body>
    <form id="form1" runat="server">
            <div>
            <img src="images/ssp-logo.png" alt="SSP logo" /></div>
        <div style="clear: both;">
            <ul>
               <li>
                    <asp:Label ID="lblSelect" runat="server" Width="70" AssociatedControlID="ddlProducts" Text="Select Product : " />
                    <asp:DropDownList ID="ddlProducts" Width="150" runat="server" AutoPostBack="True" /><br />
                </li>
                <br />
                <li>
                    <asp:Label ID="lblSelectScreen" runat="server" Width="70" AssociatedControlID="ddlRisk" Text="Select Risk :&nbsp&nbsp&nbsp&nbsp&nbsp"></asp:Label>
                    <asp:DropDownList ID="ddlRisk" runat="server" Width="150" AutoPostBack="True">
                    </asp:DropDownList>&nbsp;<br />
                </li>
                <br />
                <li>
                    <asp:Button ID="btnSubmit" runat="server" Text="Generate XML" /><asp:Label ID="lblPath" runat="server" Text="" /><br /></li>
                 <li>
                     <asp:Label ID="lblCountFields" Visible="false" runat="server" />
                 </li>
            </ul>
            <ul>
               <li>
                   <asp:CheckBox ID="chkOnlyMap" Text ="Only Mapping" runat="server" AutoPostBack="True" /><br />
                   <asp:Label ID="lblfileupload" runat="server" Visible ="false"  Text="Select Excel File :(Any Excel File in which you want to map)" />
                  
                   <asp:FileUpload Visible="false" Width ="300" ID="FileUpexcel" runat="server" />  <br /><br />
                  
                   <asp:Label ID="lblxmlfile" runat="server" Visible ="false"  Text="Select XML File :(Grnerated By XML Generator)&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp " />
                 
                   <asp:FileUpload Visible="false" Width ="300" ID="xmlfileupload" runat="server" />
                   
                </li>
                </ul> 
             <ul>
                
               <li>
                    <asp:Label ID="lblExcelMap" Visible="false" runat="server" AssociatedControlID="txtColumnName" Text="Enter Column Name(From where you want to start mapping - Exapmle A or AA or AAA . . . ) :" />

                   <asp:TextBox ID="txtColumnName" Visible="false" runat="server"></asp:TextBox><br /><br />
                </li>
                  <li>
                    <asp:Label ID="lblExcelMaprow" Visible="false" runat="server" AssociatedControlID="txtRowNo" Text="Enter Row Number(From where you want to start mapping - Exapmle 1 or 2 or 3 . . .) :&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" />

                   <asp:TextBox ID="txtRowNo" Visible="false" runat="server"></asp:TextBox><br /><br />
                </li>
                   <li>
                    <asp:Button ID="btnMapping" Visible="false" runat="server" Text="Mapping" /></li><br />

                 <li>
                     <asp:Label ID="lblWarning" Visible="False" runat="server" ForeColor="Red" />
                 </li>
               </ul> 
        </div>
    </form>
</body>
</html>
