Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer giude no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 23/06/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' RAW 18/12/2002 : PS187 : Add new constants for WHTax
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBPartyAG"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACTabTitle3 As Integer = 103
	Public Const ACReference As Integer = 104
	Public Const ACPostcode As Integer = 105
	Public Const ACName As Integer = 106
	Public Const ACIsBranch As Integer = 107
	Public Const ACAgencyAgreement As Integer = 110
	Public Const ACAgencyNextReview As Integer = 111
	Public Const ACSource As Integer = 112
	Public Const ACIsHeadOffice As Integer = 113
	Public Const ACfraAppointment As Integer = 114
	Public Const ACHeadOffice As Integer = 115
	' PW120702
	Public Const ACPaymentMethod As Integer = 116
	Public Const ACPaymentFrequency As Integer = 117
	Public Const ACAddressOnNotice As Integer = 118
	Public Const ACTAXRegistered As Integer = 119
	Public Const ACWithholdingTaxType As Integer = 120
	Public Const ACIssuesOwnTax As Integer = 121
	Public Const ACTitle As Integer = 122
	Public Const ACBranch As Integer = 123
	Public Const ACSubBranch As Integer = 124
	Public Const ACMultipac As Integer = 125
	Public Const ACRenewalStopCode As Integer = 126
	Public Const ACWithholdingTaxRate As Integer = 127
	Public Const ACTaxNumber As Integer = 128
	Public Const ACContactPerson As Integer = 129
	Public Const ACFirstName As Integer = 130
	Public Const ACBankAccount As Integer = 131
	Public Const ACDateCancelled As Integer = 132
	
	' TF031298
	Public Const ACFinancial As Integer = 150
	Public Const ACNotes As Integer = 153
	Public Const ACLetter As Integer = 154
	
	'RWH(24/07/2000) RSAIB Process 004
	' Form Constants for Address ListView
	Public Const ACAddressListPostCode As Integer = 155
	Public Const ACAddressListUsage As Integer = 156
	Public Const ACAddressListLine1 As Integer = 157
	Public Const ACAddressListLine2 As Integer = 158
	Public Const ACAddressListLine3 As Integer = 159
	Public Const ACAddressListLine4 As Integer = 160
	
	' CMG/PB 16072002
	Public Const ACInterfaceTitle2 As Integer = 170
	Public Const ACTabTitle4 As Integer = 171
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACDeleteButton As Integer = 206
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACHeadOfficeMissing As Integer = 305
	Public Const ACRefExists As Integer = 306
	'PW090702
	Public Const ACConsultantMissing As Integer = 307
	Public Const ACAgentGroupMissing As Integer = 308
	Public Const ACInvalidComboEntry As Integer = 309
	Public Const ACInvalidTaxNumber As Integer = 310
	Public Const ACInvalidTaxRate As Integer = 311
	Public Const ACBankAccountRequired As Integer = 312
	Public Const ACInvalidPaymentMethod As Integer = 313
	Public Const ACTaxNumberRequired As Integer = 314
	Public Const ACAddressOnNoticeError As Integer = 315
	Public Const ACTitleRequired As Integer = 316
	
	
	Public Const m_ksWithholdingTaxCodeSpecial As String = "SPL" ' RAW 18/12/2002 : PS187 : Added
	Public Const m_ksWithholdingTaxCodeStandard As String = "STD" ' RAW 18/12/2002 : PS187 : Added
	
	'PSL 22/02/2003 NAP is now N/A in the mediatype table
	'Public Const m_ksPaymentMethodCodeNotApplicable = "NAP"     ' RAW 18/12/2002 : PS187 : Added
	Public Const m_ksPaymentMethodCodeNotApplicable As String = "N/A" ' RAW 18/12/2002 : PS187 : Added
	Public Const m_ksPaymentMethodCodeNotApplicableOther As String = "NAP" 'IAR 19/06/2003 : ENDVR00001152 : Added
	Public Const m_ksPaymentMethodCodeEFT As String = "EFT" ' RAW 18/12/2002 : PS187 : Added
	
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	Public Const ACIADDRESS As String = "ADDRESS"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserId As Integer
	
    'DC220803 -fsa compliance
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lReturn As Integer
	
	'SD 14/08/2002 START
	Public Const ACConstantBlank As String = ""
	Public Const ACConstantYes As String = "Yes"
	Public Const ACConstantNo As String = "No"
	
	'Following Enum types are also the database field values and combobox listindex
	Public Enum ACYesNoComboOption
		ACComboOptionBlank = 0
		ACComboOptionYes = 1
		ACComboOptionNo = 2
	End Enum
	'SD 14/08/2002 END
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Public Const ScreenHelpID As Integer = 7
	
	'****************************************
	' Party Detail Array Position Constants
	Public Const kPartyDetailTaxNumber As Integer = 0
	Public Const kPartyDetailDomiciledForTax As Integer = 1
	Public Const kPartyDetailTaxExempt As Integer = 2
	Public Const kPartyDetailTaxPercentage As Integer = 3
	'****************************************
	
	'Public constant decleration for Tabs
	Public Const AC_TAB_PAG_LOWER As Integer = 0
	Public Const AC_TAB_PAG_AGENCY As Integer = 0
	Public Const AC_TAB_PAG_ADDRESS As Integer = 1
	Public Const AC_TAB_PAG_CONTACTS As Integer = 2
	Public Const AC_TAB_PAG_PAYMENTS As Integer = 3
	Public Const AC_TAB_PAG_DOCUMENTS As Integer = 4
	Public Const AC_TAB_PAG_ADDITIONAL_DETAILS As Integer = 5
	Public Const AC_TAB_PAG_FSA_COMPLIANCE As Integer = 6
	Public Const AC_TAB_PAG_TAX As Integer = 7
    Public Const AC_TAB_PAG_BRANCHES As Short = 8
    Public Const AC_TAB_PAG_UPPER As Short = 8
    Public Const AC_TAB_PAG_PRODUCTS As Short = 10
    Public Const AC_TAB_PAG_USERS As Short = 11
    Public Const AC_TAB_PAG_CERTIFICATEYEARS As Short = 12
	
	
	Sub Main_Renamed()
		
		'Dim o As iPMBPartyAG.Interface
		'Dim lReturn As Long
		'Dim vKeyArray As Variant
		'Dim s As String
		'
		'    Set o = New Interface
		'
		'    ReDim vKeyArray(1, 0)
		'
		'    s = MsgBox("yes to add, on to edit", vbYesNo)
		'
		'    If s = vbYes Then
		'
		'        'New ***************************************
		'        lReturn = o.Initialise()
		'
		'        o.CallingAppName = "TEST"
		'
		'        lReturn = o.SetProcessModes( _
		''            vTask:=PMAdd)
		'
		'        'New ***************************************
		'
		'    Else
		'
		'    'Edit ***************************************
		'        s = InputBox("Enter Party_cnt ", , 554)
		'        vKeyArray(0, 0) = PMKeyNamePartyCnt
		'                vKeyArray(1, 0) = CLng(s)
		'
		'        lReturn = o.SetKeys(vKeyArray)
		'        lReturn = o.Initialise()
		'
		'        o.CallingAppName = "TEST"
		'
		'        lReturn = o.SetProcessModes( _
		''            vTask:=PMEdit)
		'
		'    'Edit ***************************************
		'
		'    End If
		'
		'    lReturn = o.Start()
		'
		'    lReturn = o.GetKeys(vKeyArray)
		'
		'    'MsgBox "Are we done?..." & o.StepStatus
		'
		''    If o.Status = PMOK Then
		''        ' must change to InsuranceFileCnt! - JW
		''        MsgBox "SELECTED: " & o.PartyCnt & ", " & _
		'''        o.PartyCnt
		''    End If
		'
		'    lReturn = o.Terminate()
		'
		'    Set o = Nothing
		
	End Sub
	
	
	'DC220803 -PS253 -fsa compliance
	Public Function GetHiddenOption(ByVal v_lSourceId As Integer, Optional ByRef r_vEnableFSACompliance As Boolean = False) As Integer
		
		Dim result As Integer = 0
		Dim vValue As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Link Account Executives To Commission

			If Not Information.IsNothing(r_vEnableFSACompliance) Then
				m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=v_lSourceId, r_vUnderwriting:=vValue)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOption")
					
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				If vValue <> "" Then
					r_vEnableFSACompliance = (CBool(vValue))
				Else
					r_vEnableFSACompliance = False
				End If
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
    End Function

    'Get CertificateYear Tab value from db
    Public Function GetHiddenOptionForCertificateYearTab(ByVal iSourceId As Integer, ByRef bCertificateYearTab As Boolean) As Integer
        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sCertificateYearTabValue As String = ""
        Try

            If Not Information.IsNothing(bCertificateYearTab) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSubAgentCertificateYears, v_vBranch:=iSourceId, r_vUnderwriting:=sCertificateYearTabValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOption")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sCertificateYearTabValue <> "" Then
                    bCertificateYearTab = (CBool(sCertificateYearTabValue))
                Else
                    bCertificateYearTab = False
                End If
            End If
            Return iResult
        Catch excep As System.Exception
            iResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOptionForSubAgentCertificateYear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOption", excep:=excep)
            Return iResult
        End Try
    End Function

End Module
