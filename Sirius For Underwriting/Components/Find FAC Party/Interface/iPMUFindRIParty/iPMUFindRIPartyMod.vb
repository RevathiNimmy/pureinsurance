Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 17/02/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' SP130199 - Remove NavigatorV3 class an put in stub so can be called
	' iteratively.
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMUFindRIParty"
	
	Public Const ThisApp As String = "Client Manager" ' Registry App constant.
	Public Const ThisKey As String = "Recent Files" ' Registry Key constant.
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	
	' Alix
	Public Const ACMultAddFormName As Integer = 312
	Public Const ACMultAddLabel As Integer = 313
	Public Const ACMultAddHouse As Integer = 314
	Public Const ACMultAddStreet As Integer = 315
	Public Const ACMultAddSuburb As Integer = 316
	Public Const ACMultAddCity As Integer = 317
	Public Const ACMultAddPostcode As Integer = 123
	
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACTabTitle3 As Integer = 103
	
	Public Const ACShortName As Integer = 104
	Public Const ACLongName As Integer = 105
	Public Const ACType As Integer = 106
	Public Const ACStatus As Integer = 107
	Public Const ACAddress1 As Integer = 108
	Public Const ACPostalCode As Integer = 109
	Public Const ACTelephone As Integer = 110
	Public Const ACInsReference As Integer = 111
	'RKS 141004 PN13238 & PN14838
	Public Const ACIncludeClosedBranches As Integer = 112
	
	Public Const ACListTitle1 As Integer = 120
	Public Const ACListTitle2 As Integer = 121
	Public Const ACListTitle3 As Integer = 122
	Public Const ACListTitle4 As Integer = 123
	Public Const ACListTitle5 As Integer = 124
	Public Const ACListTitleAON5 As Integer = 156
	Public Const ACListTitle6 As Integer = 125
	Public Const ACListTitle7 As Integer = 126
	Public Const ACListTitle8 As Integer = 127
	Public Const ACListTitleAddressLine2 As Integer = 155
	Public Const ACListTitleDateOfBirth As Integer = 153
	Public Const ACListTitleSwiftLink As Integer = 154
	'TF181000
	Public Const ACFindFinanceProviderTitle As Integer = 128
	'TN20000918
	Public Const ACFindReinsurerTitle As Integer = 129
	Public Const ACFindAgentTitle As Integer = 130
	Public Const ACFindConsultantTitle As Integer = 131
	Public Const ACFindAccountHandlerTitle As Integer = 132
	Public Const ACFindInsurerTitle As Integer = 133
	Public Const ACFindBrokerTitle As Integer = 134
	Public Const ACFindFeeTitle As Integer = 135
	Public Const ACFindExtraTitle As Integer = 136
	Public Const ACFindDiscountTitle As Integer = 137
	Public Const ACFindCommissionAccountTitle As Integer = 138
	Public Const ACFindIntermediaryTitle As Integer = 162
	
	'TF181000
	Public Const ACListFinanceProviderTitle As Integer = 139
	Public Const ACListAgentTitle As Integer = 140
	Public Const ACListConsultantTitle As Integer = 141
	Public Const ACListAccountHandlerTitle As Integer = 142
	Public Const ACListInsurerTitle As Integer = 143
	Public Const ACListBrokerTitle As Integer = 144
	Public Const ACListFeeTitle As Integer = 145
	Public Const ACListExtraTitle As Integer = 146
	Public Const ACListDiscountTitle As Integer = 147
	Public Const ACListCommissionAccountTitle As Integer = 148
	Public Const ACListIntermediaryTitle As Integer = 163
	
	'TN20000918
	Public Const ACListReinsurerTitle As Integer = 149
	
	Public Const ACFindOtherPartyTitle As Integer = 150
	Public Const ACOtherPartyCode As Integer = 151
	Public Const ACOtherPartyType As Integer = 152
	
	'CMG/PB 18072002
	Public Const ACFindAgentGroupTitle As Integer = 157
	Public Const ACListAgentGroupTitle As Integer = 158
	Public Const ACListBranchTitle As Integer = 159
	'DC101204
	Public Const ACListThirdPartyTitle As Integer = 160
	Public Const ACFindThirdPartyTitle As Integer = 161
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	Public Const ACFindNowButton As Integer = 204
	Public Const ACNewSearchButton As Integer = 205
	Public Const ACNewButton As Integer = 206
	Public Const ACEditButton As Integer = 207
	
	Public Const ACDeleteButton As Integer = 208
	Public Const ACUndeleteButton As Integer = 209
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	Public Const ACNoAgencyAgreementTitle As Integer = 308 'TF030399
	Public Const ACNoAgencyAgreement As Integer = 309
	
	Public Const ACRemoveClientTitle As Integer = 310 'CJB200802
	Public Const ACRemoveClient As Integer = 311 'CJB200802
	Public Const ACAmendClientDetails As Integer = 312
	
	Public Const PMSearchSirius As Integer = 0
	Public Const PMSearchPMB As Integer = 1
	Public Const PMSearchSiriusPMB As Integer = 2
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	'Modified by ECK 11/05/99
	Public Const ACMinSearchLength As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_oBusiness As bSIRFindRIParty.Business
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oClaimBusiness As bCLMFindClaim.Business
    ' Public instance of the back office business object.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oBackofficelink As Object

    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
    'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oUserAuthorities As Object '2005 Client Security
	'Public Const ScreenHelpID As Integer = 1
	Public Const ScreenHelpID As Integer = 5001
	Public Const UWScreenHelpID As Integer = 4001
	
	Public g_bGenericConnectionStatus As Boolean
	
	Public Const ACNowtSelected As Integer = 0
	Public Const ACNewPasswordEntered As Integer = 1
	Public Const ACPasswordOK As Integer = 2
	Public Const ACVoiceRecognised As Integer = 3
	Public Const ACTelephoneNumberRecognised As Integer = 4
	Public Const ACNoPasswordEntered As Integer = 5
	
	Public Const kSystemOptionClientBlacklistingInForce As Integer = 5011
	
	'QBENZ005
	Public Const ACIRI2007ShortName As Integer = 0
	Public Const ACIRI2007LongName As Integer = 1
	Public Const ACIRI2007AccType As Integer = 2
	Public Const ACIRI2007Address1 As Integer = 3
	Public Const ACIRI2007Address2 As Integer = 4
	Public Const ACIRI2007PostalCode As Integer = 5
	Public Const ACIRI2007RITypeCode As Integer = 6
	Public Const ACIRI2007FileCode As Integer = 7
	Public Const ACIRI2007SourceID As Integer = 8
	Public Const ACIRI2007SourceName As Integer = 9
	Public Const ACIRI2007PartyCnt As Integer = 10
	Public Const ACIRI2007PartyTypeCode As Integer = 11
	Public Const ACIRI2007DefaultCommission As Integer = 12
	
	'Constants for List View Column Headers
	Public Const GridCol_ReinsurerCode As Integer = 0
	Public Const GridCol_Name As Integer = 1
	Public Const GridCol_AcType As Integer = 2
	Public Const GridCol_Participation As Integer = 3
	Public Const GridCol_SumInsured As Integer = 4
	Public Const GridCol_Premium As Integer = 5
	Public Const GridCol_Tax As Integer = 6
	Public Const GridCol_Comm As Integer = 7
	Public Const GridCol_Commission As Integer = 8
	Public Const GridCol_CommTax As Integer = 9
	Public Const GridCol_AgreementCode As Integer = 10
	Public Const GridCol_PartyCnt As Integer = 11
	Public Const GridCol_RILineId As Integer = 12
	
	'Broker participant
	Public Const ACIBrokerShortName As Integer = 0
	Public Const ACIBrokerLongName As Integer = 1
	Public Const ACIBrokerParticipant_percent As Integer = 2
	Public Const ACIBrokerAssociationPartyCnt As Integer = 3
	Public Const ACIBrokerPartyCnt As Integer = 4
    'E005
    Public Const ACBPParticipantonTreatyID = 0
    Public Const ACBPTreatyID = 1
    Public Const ACBPTreatyPartyID = 2
    Public Const ACBPPassociatedPartyCnt = 3
    Public Const ACBPPartyCnt = 4
    Public Const ACBPParticipantPercent = 5
    Public Const ACBPShortCode = 6
    Public Const ACBPName = 7
	
	'qbenz005
	
	Sub Main_Renamed()
		
        Dim vId As Object
        Dim vKeyArray(,) As Object
        'Developer Guide no. 108
        Dim oFindParty As New Interface_Renamed
		
		Dim lError As Integer = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()
		
		oFindParty.CallingAppName = "TEST"
		
		lError = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)
		
		ReDim vKeyArray(1, 0)
		
		
		lError = oFindParty.Start()
		
		Dim lPartyCnt As Integer = oFindParty.PartyCnt
		
		
        oFindParty.Dispose()
		
		
	End Sub
	
	Public Function GetName(ByVal v_lPartyCnt As Integer, ByRef r_sPartyShortName As String, ByRef r_sPartyResolvedName As String) As Integer
		Dim result As Integer = 0
		Dim bSIRFindParty As Object
		

		Dim oObject As bSIRFindParty.Business
		Dim lReturn As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim temp_oObject As Object
			lReturn = g_oObjectManager.GetInstance(temp_oObject, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			oObject = temp_oObject
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRFindParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return result
			End If
			
			

			lReturn = oObject.GetName(v_lPartyCnt, r_sPartyShortName)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			lReturn = oObject.GetResolvedName(v_lPartyCnt, r_sPartyResolvedName)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

            oObject.Dispose()
			
			oObject = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module