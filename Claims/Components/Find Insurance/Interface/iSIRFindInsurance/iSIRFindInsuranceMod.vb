Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no.129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 11/07/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iSIRFindInsurance"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' General Icons
	
	'Date Formats
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "long date"
	Public Const ACShortDate As String = "short date"
	
	' Caption Constants for Tab on the Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	
	'Caption Constants for Labels on the form
	Public Const ACPolicyNumber As Integer = 103
	Public Const ACPolicyCode As Integer = 104
	Public Const ACRiskIndex As Integer = 105
	Public Const ACClaimDate As Integer = 106
	Public Const ACShortName As Integer = 107
	Public Const ACPostCode As Integer = 108
	Public Const ACFromDate As Integer = 109
	Public Const ACToDate As Integer = 110
	
	'Caption Constants for Column Headings of List View
	
	Public Const ACListTitle1 As Integer = 113 'Policy Holder
	Public Const ACListTitle2 As Integer = 114 'Policy Reference
	Public Const ACListTitle3 As Integer = 115 'Riskindex
	Public Const ACListTitle4 As Integer = 116 'Product Code
	Public Const ACListTitle5 As Integer = 117 'From Date
	Public Const ACListTitle6 As Integer = 118 'To Date
	Public Const ACListTitle7 As Integer = 119 'Post Code
	'DC060301 - added new title
	Public Const ACListTitle8 As Integer = 120 'Risk
	Public Const ACListTitle9 As Integer = 125 'Insurer
	
	'TN20010426 Start
	Public Const ACListTitleClientCode As Integer = 121
	Public Const ACListTitleClientName As Integer = 122
	Public Const ACListTitleAddress1 As Integer = 123
	'TN20010426 End
	
	Public Const ACListTitleRenewalDate As Integer = 124 'To Date
	
	'Caption Constants for  Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACNewButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACFindNowButton As Integer = 206
	Public Const ACNewSearchButton As Integer = 207
	Public Const ACViewButton As Integer = 208
	
	'Caption Constants for Messages to User
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	Public Const ACLookupFailTitle As Integer = 308
	Public Const ACLookupFail As Integer = 309
	
	Public Const ACInvalidDateMsg As Integer = 400
	Public Const ACDateDiffError As Integer = 401
	
	Public Const ACLossDateBefore As Integer = 404
	Public Const ACLossDateAfter As Integer = 405
	Public Const ACPolicyVoided As Integer = 406
	Public Const ACNoPolicyFound As Integer = 407
	
	
	'Positionioning Values for Claim Date Controls
	Public Const ACLblClaimDateLeft As Integer = 120
	'Changed here For enhancement
	Public Const ACTxtClaimDateLeft As Integer = 1560
	'SP080102 - Merge catch up
	'DC251001 -changed to slot in Find Party (was 960)
	Public Const ACClaimDateTop As Integer = 1440
	
	'DC251001 -added
	Public Const ACCmdRelatedPartyFindLeft As Integer = 120
	Public Const ACTxtShortnameLeft As Integer = 1560
	Public Const ACTxtShortnameTop As Integer = 960
	
	Public Const ACLblPostCodeLeft As Integer = 120
	Public Const ACPostCodeTop As Integer = 480
	Public Const ACTxtPostCodeLeft As Integer = 1560
	
	Public Const ACLblInForceFromDateLeft As Integer = 120
	Public Const ACInForceFromDateTop As Integer = 960
	Public Const ACTxtInForceFromDateLeft As Integer = 1560
	
	Public Const ACLblInForceToDateLeft As Integer = 120
	Public Const ACInForceToDateTop As Integer = 1440
	Public Const ACTxTInForceToDateLeft As Integer = 1560
	
	'PSL 21/02/2003 Issue 2403 Prevent Wildcard Search on Risk Index
	Public Const ACRiskIndexTitle As Integer = 402
	Public Const ACRiskIndexMessage As Integer = 403
	
	' Menus
	
	
	' Constants for the search data array indexes.
	Public Const ACIInsuranceFilecnt As Integer = 0
	Public Const ACIPolicyHolder As Integer = 1
	Public Const ACIPolicyNumber As Integer = 2
	
	'UnderWriting Array Constants
	
	Public Const ACIURiskIndex As Integer = 3
	Public Const ACIUProductCode As Integer = 4
	Public Const ACIUFromDate As Integer = 5
	Public Const ACIUToDate As Integer = 6
	Public Const ACIUPostCode As Integer = 7
	
	'TN20010426 Start
	Public Const ACIUAddress1 As Integer = 8
	Public Const ACIUPartyName As Integer = 9
	'TN20010426 End
	Public Const ACIURenewalDate As Integer = 10
	
	Public Const ACIUIsSourceClosed As Integer = 11
	Public Const ACIUClaimsAllowed As Integer = 12
	
	'Broking Array Constants
	
	Public Const ACIBFromDate As Integer = 3
	Public Const ACIBToDate As Integer = 4
	Public Const ACIBPostCode As Integer = 5
	'DC060301 - added new one for risk description
	Public Const ACIBRiskDescription As Integer = 6
	Public Const ACIBInsurerCode As Integer = 7
	'S4B Claim Enhancements R&D 2005
	Public Const ACIBAddressLine1 As Integer = 8
    Public Const ACIBAccountExec As Integer = 9

    Public Const ACIInsuranceFileStatus As Integer = 15
    Public Const ACILapseDate As Integer = 22

	
	Public Const PMKeyNameVehicleRegNo As String = "vehicle_reg"
	
	
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
	
	'Constants for Date and Date Sort Column
	Public Const ACFormDateColumn As Integer = 3
	Public Const ACDateColumn As Integer = 4
	Public Const ACDateSortColumn As Integer = 6
	
	' Public source and language ID's from the
    ' Object Manager.
    
    Public g_iSourceID As Integer
    
    Public g_iLanguageID As Integer
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bBackOfficeLink.bBOLink
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    ' MKW 190503 PN2032 START    
    Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
	' MKW 190503 PN2032 END
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.1.3)
	Public Const kUSLangId As Integer = 2
	Public Const kUKLangId As Integer = 1
	'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.1.3)
	
	
	' Public instance of the business object.
	'Public g_oBusiness As bBackOfficeLink.bBOLink
	'Public g_oBusiness As bSIRFindInsurance.Form
	
	
	
	Sub Main_Renamed()
        'developer guide no.108
        Dim o As New Interface_Renamed
		Dim lReturn As gPMConstants.PMEReturnCode = CType(CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
		lReturn = CType(o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)
		lReturn = o.Start()
		o.Dispose()
		
		
	End Sub
	
	Public Function ProcessFSAAccess(ByVal lPartyCnt As Integer, ByVal sPartyType As String, ByRef bProceed As Boolean) As Integer
		Dim result As Integer = 0
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
			

			m_lReturn = oFindParty.FSACustomerVal(lPartyCnt, sPartyType, bProceed)
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