Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 15/07/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:Pandu
	' ***************************************************************** '
    'developer guide no. 107
    '<ThreadStatic()> _
    'Public lstitem1 As Integer
    'developer guide no. 107
    '<ThreadStatic()> _
    'Public lstitem2 As Integer
    ' Main public constant for all functions
    ' to identify which application this is.
	Public Const ACApp As String = "iCLMFindClaim"

	' String Resources
	Public Const ACInterfaceTitle As Integer = 100 'Find CLaim
	Public Const ACTabTitle1 As Integer = 101 '&1-General
	Public Const ACTabTitle2 As Integer = 102 '&2-Advanced
	Public Const ACClaimRef As Integer = 103 '&Claim Reference :
	Public Const ACPolicyRef As Integer = 104 '&Policy :
	Public Const ACPolicyHolder As Integer = 105 'C&lient :
	Public Const ACRegistrationNumber As Integer = 106 '&Registration Number :
	Public Const ACLossFromDate As Integer = 107 'Loss Date &Start Limit :
	Public Const ACLossToDate As Integer = 108 'Loss Date &End Limit :
	Public Const ACIncludeClosedClaims As Integer = 109 ' &Include Closed Claims :
	
	Public Const ACListTitle1 As Integer = 110 'Claim Type
	Public Const ACListTitle2 As Integer = 111 'Claim Reference
	Public Const ACListTitle3 As Integer = 112 'Insurance Reference
	'TN20010426 Start
	'Public Const ACListTitle4 = 113 'Risk Index
	Public Const ACListTitle4 As Integer = 116 'Client
	'TN20010426 End
	Public Const ACListTitle5 As Integer = 114 'Product Code
	Public Const ACListTitle6 As Integer = 115 'Loss Date
	'DC130202 -new headings for broking
	Public Const ACListTitle7 As Integer = 117 'Description
	Public Const ACListTitle8 As Integer = 118 'Handler
	Public Const ACListTitle9 As Integer = 119 'Status
	Public Const ACListTitle10 As Integer = 120 'Insurer '2005 Plan
	
	'Start - (Prakash Varghese) - (Tech Spec - TRAC 2899 Client Code Client Name.docx) - (6.1.1)
	'Constant to refer Client Name in the list header
	Public Const ACListTitle11 As Integer = 128 'Client Name
	'End - (Prakash Varghese) - (Tech Spec - TRAC 2899 Client Code Client Name.docx) - (6.1.1)
	'DC130202
	Public Const AC_RES_INSURER As Integer = 121
	Public Const AC_RES_ACCOUNTEXEC As Integer = 122
	Public Const AC_RES_CLIENTNAME As Integer = 123
	Public Const AC_RES_VEHICLEREG As Integer = 124
	
	Public Const AC_RES_LISTTITLE11 As Integer = 125
	Public Const AC_RES_LISTTITLE12 As Integer = 126
	Public Const AC_RES_LISTTITLE13 As Integer = 127
	
	'Date Formats
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "long date"
	Public Const ACShortDate As String = "short date"
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	Public Const ACFindNowButton As Integer = 204
	Public Const ACNewSearchButton As Integer = 205
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	Public Const ACInvalidDateMsg As Integer = 400 'Invalid Date Entered
	Public Const ACDateDiffError As Integer = 401 'Date Diff Error
	
	Public Const ACClosedBranchError As Integer = 402
	Public Const ACClosedBranchError_Payments As Integer = 403
	Public Const ACClosedBranchError_Recoveries As Integer = 404
	
	Public Const ACLookupFailTitle As Integer = 308
	Public Const ACLookupFail As Integer = 309
	
	
	' Constants for the search data array indexes.
	Public Const ACIInsuranceid As Integer = 0
	Public Const ACIClaimCnt As Integer = 1
	Public Const ACIClaimType As Integer = 2
	Public Const ACIClaimRef As Integer = 3
	Public Const ACIInsuranceRef As Integer = 4
    Public Const ACIUClientCode As Integer = 5

	Public Const ACIUProductCode As Integer = 6
	Public Const ACIULossDate As Integer = 7
	Public Const ACIUPolicyHolder As Integer = 8
	Public Const ACIUClaimStatus As Integer = 9
	
	'Constants for Broking
	Public Const ACIBLossDate As Integer = 5
	Public Const ACIBPolicyHolder As Integer = 6
	
	Public Const ACIRiskTypeID As Integer = 9
	'DC250501 for Broking
	Public Const ACIClientCode As Integer = 17
	'DC250501
	
	Public Const ACIInsurerCode As Integer = 20 '2005 add insurer code
	Public Const ACIBranchClosed As Integer = 21
	Public Const ACIClaimsAllowed As Integer = 22
    Public Const ACIOtherPartyID As Integer = 23
	'DC130202 for Broking
	Public Const ACIDescription As Integer = 2
	Public Const ACIStatusId As Integer = 7
	Public Const ACIHandler As Integer = 8
	'DC130202
	
	'S4B Claims Enhancements R&D 2005
	Public Const ACIBClientResolvedName As Integer = 21
	Public Const ACIBAccountExecutive As Integer = 22
	Public Const ACIBVehicleRegistration As Integer = 23
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBackofficelink As bBackOfficeLink.bBOLink
	' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bCLMFindClaim.Business
	
	' MKW 190503 PN2032 START
	Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	' MKW 190503 PN2032 END

	Sub Main_Renamed()
		
		'Dim o As iCLMFindClaim.Interface
		'Dim lReturn As Long
		'
		'    Set o = New Interface
		'    lReturn = o.Initialise()
		'    lReturn = o.SetProcessModes(vTask:=PMView)
		'    lReturn = o.Start
		'    lReturn = o.Terminate
		'
		'    Set o = Nothing
	End Sub
	
	Public Function ProcessFSAAccess(ByVal lPartyCnt As Integer, ByVal sPartyType As String, ByRef bProceed As Boolean) As Integer
		
		Dim result As Integer = 0
        'developer guide no. 108
        Dim oFindParty As iPMBFindParty.Interface_Renamed
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim temp_oFindParty As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oFindParty = temp_oFindParty
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBFindParty.frmInterface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFSAAccess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
			End If
			
			m_lReturn = CType(oFindParty.FSACustomerVal(lPartyCnt, sPartyType, bProceed), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
            oFindParty.Dispose()
			
			
			oFindParty = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessFSAAccess Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFSAAccess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
    End Function
    
End Module