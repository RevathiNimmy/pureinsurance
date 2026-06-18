Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("GEMListMgrConst_NET.GEMListMgrConst")> _
 Public Module GEMListMgrConst
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	'Constands for bGEMListMgr
	
	'List Item Fields constants
	Public Const LSTString As Integer = 0
	Public Const LSTFlags As Integer = 1
	Public Const LSTValueID As Integer = 2
	Public Const LSTPosID As Integer = 3
	Public Const LSTText As Integer = 4
	Public Const LSTABICode As Integer = 5
	Public Const LSTCommand As Integer = 6
	Public Const LSTType As Integer = 7
	Public Const LSTChanged As Integer = 8
	Public Const LSTMax As Integer = 8
	
	'List Types
	Public Const LSTTypeCustom As Integer = 0
	Public Const LSTTypeUser As Integer = 1
	Public Const LSTDeleted As String = "D"
	Public Const LSTAmmended As String = "C"
End Module