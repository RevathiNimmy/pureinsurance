Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 11/08/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBPartyAH"
	'EK 12/10/99
	Public Const ACHandler As String = "AH"
	Public Const ACExecutive As String = "CO"
	'DC260903 -PS256 fsa compliance
	Public Const ACExecHandler As String = "HC"
	'EK 15/10/99
	Public Const PMKeyNameSpecialParty As String = "special_party"
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACClientCode As Integer = 103
	Public Const ACLastname As Integer = 104
	Public Const ACForename As Integer = 105
	Public Const ACCurrency As Integer = 106
	Public Const ACDepartment As Integer = 107
	Public Const ACTitle As Integer = 108
	Public Const ACInitials As Integer = 109
	'EK 15/11/99
	Public Const ACTabTitle3 As Integer = 110
	Public Const ACExecCode As Integer = 111
	'SJP (CMG) PS235 01/04/2003
	Public Const ACCommissionAccount As Integer = 112
	'DC260903 -PS256 fsa compliance
	Public Const ACTabTitle4 As Integer = 113
	Public Const ACTabTitle5 As Integer = 114
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAdd As Integer = 204
	Public Const ACEdit As Integer = 205
	Public Const ACDelete As Integer = 206
	
	'CMG/PB 15/7/2002 Form Constants for Address ListView
	Public Const ACAddressListPostCode As Integer = 155
	Public Const ACAddressListUsage As Integer = 156
	Public Const ACAddressListLine1 As Integer = 157
	Public Const ACAddressListLine2 As Integer = 158
	Public Const ACAddressListLine3 As Integer = 159
	Public Const ACAddressListLine4 As Integer = 160
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	'CMG/PB 15/7/2002 Add Address Tab
	Public Const ACIADDRESS As String = "ADDRESS"
	'ABI Description for Sirius Main Address - ie this is the ABI desc for
	'the address that Sirius considers 'main' address and is used for
	'linking to Gemini
	
	'02082002 CMG/PB Scalabilty changes, these are in GSIRLibrary
	'Public Const SIRMainAddressABIDescription = "Correspondence Address"
	'Public Const SIRMainAddressABICode = "3131 XCO"
	'Public Const SIRBusinessAddressABIDescription = "Business Address"
	'Public Const SIRBusinessAddressABICode = "3131 002"
	'CMG End
	
	' Array Positions
	Public Const ACPartyCnt As Integer = 0
	Public Const ACPartyShortName As Integer = 1
	Public Const ACPartyResolvedName As Integer = 2
	Public Const ACPartySourceId As Integer = 3
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUnderwritingOrAgency As String = ""
	
	Private m_lReturn As Integer
	
	Sub Main_Renamed()
		
	End Sub
	
	Public Function GetHiddenOptions(ByVal v_lSourceId As Integer, Optional ByRef r_vLinkToCommission As Boolean = False) As Integer
		
		Dim result As Integer = 0
		Dim vValue As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Link Account Executives To Commission

			If Not Information.IsNothing(r_vLinkToCommission) Then
				m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, v_vBranch:=v_lSourceId, r_vUnderwriting:=vValue)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
					
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				'KB 24042003  PN  3753 cater for no value in vValue
				If vValue <> "" Then
					r_vLinkToCommission = (CBool(vValue))
				Else
					r_vLinkToCommission = False
				End If
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
    End Function
    

End Module
