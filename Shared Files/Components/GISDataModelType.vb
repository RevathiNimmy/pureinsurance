Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("GISDataModelType_NET.GISDataModelType")> _
 Public Module GISDataModelType
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	'data model types
	Public Const GISDMTypeNotSet As Integer = -1
	Public Const GISDMTypeNonDatabase As Integer = 0
	Public Const GISDMTypeRisk As Integer = 1
	Public Const GISDMTypeClaim As Integer = 2
	'22/04/2003 - PWC - (408) User Definable Fields
	Public Const GISDMTypePolicy As Integer = 3
	Public Const GISDMTypeParty As Integer = 4
	Public Const GISDMTypeCase As Integer = 5
	Public Const GISDMTypeLast As Integer = GISDMTypeCase
	
	Public Const GISDMCodeCommon As String = "SIR_Common"
	Public Const GISDMCodePolicy As String = "SIR_Policy"
	Public Const GISDMCodeParty As String = "SIR_Party"
	
	'object types
	Public Const GISOTRisk As Integer = 0 'standard risk objects
	Public Const GISOTNonGisSpecials As Integer = 1 'special object to hold non gis sum assured and standard wordings
	Public Const GISOTAssociatedClient As Integer = 2
	Public Const GISOTDisclosure As Integer = 3
	Public Const GISOTClaim As Integer = 4 'DMC_claim
	Public Const GISOTPeril As Integer = 5 'DMC_claim_peril
	Public Const GISOTClaim_org As Integer = 6 'DMC_claim
	Public Const GISOTPeril_org As Integer = 7 'DMC_peril
	' PW080503 - Terms needs to be a standard object (ENDVR00000841)
	'Public Const GISOTTerms = 8
	'22/04/2003 - PWC - (408) User Definable Fields
	Public Const GISOTParty As Integer = 9
	Public Const GISOTCase As Integer = 10
	Public Const GISOTLast As Integer = GISOTCase
End Module