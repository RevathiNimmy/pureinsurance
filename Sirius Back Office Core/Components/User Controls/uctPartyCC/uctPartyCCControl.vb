Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Globalization
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctPartyCCControl_NET.uctPartyCCControl")>
Partial Public Class uctPartyCCControl
	Inherits System.Windows.Forms.UserControl
    Implements IDisposable
	Public Event FromEventChange()
	Public Event OrgNameChange()
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 23/06/1998
	'
	' Description: Main interface.
	'
	' Edit History:
	' DJM 22/04/2002 : MainContactCnt changed from a int to a long.
	' TF031298 - Menu & Toolbar activity
	' SP050199 - TradingSinceDate and companyReg are now non-mandatory
	' RAW 18/11/2002 : PS005 : Add tab6 for customer loyalty scheme
	'
	' PN17181 - PW061204 - Use the m_oFormFields.UnformatControl to get
	'           the value of txtTobLetter in InterfaceToData. This will
	'           ensure the data is stored correctly and stop an empty
	'           date from appearing as "30/12/1899" in document production
	' RKS 24/01/2005 : Implemented Duplicate Client Identification
	' CJB 21/07/2005 : PN22533 Ensure cboEmployees & cboTurnover are not
	'                  positioned to default value (as they are mandatory)
	'                  & ensure they are validated to be required.
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "uctPartyCCControl"
	
	'Default Property Values:
	Const m_def_BackColor As Integer = 0
	Const m_def_ForeColor As Integer = 0
	Const m_def_Enabled As Integer = 0
	Const m_def_BackStyle As Integer = 0
	Const m_def_BorderStyle As Integer = 0
	Const m_def_PartyCnt As Integer = 0
	'Property Variables:
	Dim m_BackColor As Integer
	Dim m_ForeColor As Integer
	Dim m_Enabled As Boolean
	Dim m_Font As Font
	Dim m_BackStyle As Integer
	Dim m_BorderStyle As Integer
	Dim m_PartyCnt As Integer
	'Event Declarations:
	Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs)
	Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)
	Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs)
	Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs)
	Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs)
	Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
	Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
	Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_iProspectTask As gPMConstants.PMEComponentAction
	' {* USER DEFINED CODE (Begin) *}
	Private m_lAddressCount As Integer
	
	'JDW

	Dim m_oQASAddress As bSIRAddress.Business
	'bSIRAddress.business
	
	Public Structure QASNamesData
		Dim Title As String
		Dim Forename As String
		Dim Surname As String
		Dim Initial As String
		Dim Postcode As String
		Dim OrgName As String
		Dim Add1 As String
		Dim Add2 As String
		Dim Add3 As String
		Dim Add4 As String
		Dim IsOrg As Boolean
		Public Shared Function CreateInstance() As QASNamesData
			Dim result As New QASNamesData
			result.Title = String.Empty
			result.Forename = String.Empty
			result.Surname = String.Empty
			result.Initial = String.Empty
			result.Postcode = String.Empty
			result.OrgName = String.Empty
			result.Add1 = String.Empty
			result.Add2 = String.Empty
			result.Add3 = String.Empty
			result.Add4 = String.Empty
			Return result
		End Function
	End Structure
	
	Private oQNAddress As QASNamesData = QASNamesData.CreateInstance()
	
	
	Private m_iDefaultCountryID As Integer
	Private m_sDefaultCountryCode As String = ""
	
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Declare an instance of the Lock object.
	Private m_oPMLock As Object
	Private m_oPMUser As Object
	
	'2005 Layout Changes
	Private m_oAccount As Object
	Private m_oCurrencyConvert As Object
	
    ' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Declare an instance of the prospect policy interface.

    Private m_oProspectPolicy As Object
	
	Private m_bChangedProspect As Boolean
	
    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast(1, 6) As Control
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	'Data retrieved via Get Details from Party & PartyPC
	
	Private m_sOldIdReference As String = ""
	Private m_sOldResolvedName As String = ""
	
	Private m_lSwiftPartyID As Integer
	
	Private m_lPartyCnt As Integer
	Private m_iPartySourceId As Integer
	
	Private m_iNewPartySourceId As Integer
	Private m_lPartyId As Integer
	Private m_lNewPartyId As Integer
	'
	Private m_iPartyTypeId As Integer
	Private m_sShortName As String = ""
	Private m_sName As String = ""
	
	Private m_iIsAlsoAgent As CheckState
	Private m_iIsProspect As CheckState
	'2005 Screen Layout Changes
	Private m_cYearToDateTurnover As Decimal
	Private m_cLastYearTurnover As Decimal
	Private m_cClientBalance As Decimal
	
	Private m_lAgentCnt As Integer
	Private m_lConsultantCnt As Integer
	Private m_lInsurerCnt As Object
	Private m_lBrokerCnt As Object
	Private m_iAreaId As Integer
	Private m_sFileCode As String = ""
	
	Private m_sRecordStatus As String = ""
	Private m_iCurrencyId As Integer
	Private m_sPaymentMethodCode As String = ""
	Private m_lReminderTypeID As Integer
	Private m_lServiceLevelId As Integer
	Private m_sCreditCardCode As String = ""
    Private m_iPaymentTermId As Integer
	Private m_lCCJs As Integer
    Private m_sResolved As String = ""
    'developer guide no. 101
    Private m_lSeasonalGiftId As Object

    Private m_lStrengthCodeId As Integer
    'developer guide no. 101
    Private m_lSICCodeId As Object
	
	Private m_sCompanyReg As String = ""
	Private m_dtTradingSinceDate As Date
	Private m_sPartyBusinessID As String = ""
	Private m_lLocation As Integer
	Private m_lNoOfOffices As Integer
	Private m_lNoOfEmployees As Integer
	Private m_dtFinancialYear As Date
	Private m_sTradeCode As String = ""
	
	
	Private m_vWageRoll As Object
	Private m_vTurnover As Integer
	
	' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development
	Private m_lTradeID As Integer
	
	' DD 24/10/2003
	Private m_iTPSind As CheckState
	Private m_sSource As String = ""
	Private m_iMailshot As CheckState
	Private m_iEMPSind As CheckState
	Private m_sTPPassword As String = ""
	
	
    Private m_sSalutation As String = ""
	
    ' developer guide no. 101
    Private m_vSeasonalGiftId As Object
	Private m_vStrengthCodeId As Object
    Private m_vSICCodeId As Object
	Private m_vPreviousInsurerCnt As Object
	Private m_vPreviousBrokerCnt As Object
	
	Private m_vCorrespondenceTypeId As Integer
	
	
	Private m_vRenewalStopCodeId As Integer
	
	'References from Party Lookups
	Private m_sAgentRef As String = ""
	Private m_sAgentName As String = ""
	Private m_sConsultantRef As String = ""
    Private m_sConsultantName As String = ""
    'developer guide no. 101
    Private m_vAssociates As Object
	
	'
	Private m_vRelationships As Object
	
	Private m_sInsurerRef As String = ""
	Private m_sInsurerName As String = ""
	Private m_sBrokerRef As String = ""
	Private m_sBrokerName As String = ""
	
	Private m_lMainContactCnt As Integer
	Private m_sMainContactDesc As String = ""
	
	'Addresses and Contacts
	Private m_iLine As Integer
	Private m_lAddressCnt As Integer
	Private m_lAddressUsageTypeID As Integer
	Private m_lContactCnt As Integer
    Private m_sMainPostCode As String = ""
    Private m_vAddresses As Object
    Private m_vAddressTypes As Object
    Private m_vContacts As Object
    Private m_sAddressLine1 As String = ""

    Private m_vConvictions As Object
    Private m_vLoyaltySchemes As Object
    Private m_vCorrespondenceTypes As Object

	'Flag to indicate whether we need to check the headoffice id matches
	'the headoffice ref as user may change the reference directly
	Private m_bVerifyHeadOfficeCnt As Boolean
	Private m_bVerifyAgentCnt As Boolean
	Private m_bVerifyConsultantCnt As Boolean
	Private m_bVerifyBrokerCnt As Boolean
	Private m_bVerifyInsurerCnt As Boolean
	
	'Note the index in the lookup array of the main address
	Private m_iMainAddressIndex As Integer
    'developer guide no. 33
    Private m_vOldAddress As Object
    'developer guide no. 33
    Private m_vNewAddress As Object
	
	' Declare an instance of the address interface.

    'developer guide no. 108
    Private m_oAddress As iPMBAddress.Interface_Renamed
	
	' Declare an instance of the associate interface.

    'developer guide no. 108
    Private m_oAssociates As Object
	
	' Declare an instance of the contact interface.

    'developer guide no. 108
    Private m_oContact As iPMBContact.Interface_Renamed
	
	' Declare an instance of the conviction interface.

    'developer guide no. 108
    Private m_oConviction As iPMBPartyConviction.Interface_Renamed
	
	' Declare an instance of the prospecting interface.

    'developer guide no. 108
    Private m_oProspect As Object
	
	' Declare an instance of the PartyLoyaltyScheme interface.

    'developer guide no. 108
    Private m_oPartyLoyaltyScheme As iPMBPartyLoyaltyScheme.Interface_Renamed  'RAW 18/11/2002 : PS005 : Added
	
	Private m_bChangedProspectPolicies As Boolean
	
	' Agent
	Private m_lCurrentAgent As Integer
	Private m_sCurrentAgentRef As String = ""
	Private m_sCurrentAgentName As String = ""
	Private m_bVerifyCurrentAgentCnt As Boolean
	Private m_bEvent As Boolean
	
	Private m_sLoyaltyNumber As String = ""
	Private m_sAlternativeIdentifier As String = ""
	Private m_sTradingName As String = ""
	Private m_lSubBranchId As Integer
	Private m_bUserMode As Boolean
	Private m_bIsNRMA As Boolean
	Private m_bAONAffinity As Boolean
	Private m_bMultiTreeAccounting As Boolean
	
	Private m_bValidateAlternativeIdentifier As Boolean
	Private m_sBranchPrefix As String = ""
	Private m_sLoyaltyNumberScript As String = ""
	Private m_sAlternativeIdentifierScript As String = ""
	
	
	
	Private m_bFutureDateAddressChanges As Boolean
	Private m_vFutureDatedAddresses As Object
	Private m_bUpdateFutureDatedAddress As Boolean
	
	Private m_bBusinessFieldOnClientIsMandatory As Boolean
	Private m_dtTobLetter As Date 
	Private Const sADDR_USAGE_TITLE As String = "Address Usage"
	Private Const sADDR_LINE1_TITLE As String = "Address Line 1"
	Private Const sADDR_LINE2_TITLE As String = "Address Line 2"
	Private Const sADDR_LINE3_TITLE As String = "Address Line 3"
	Private Const sADDR_LINE4_TITLE As String = "Address Line 4"
	
	
	Private m_sUnderwritingOrAgency As String = ""
	Private m_bReadTradeABIList As Boolean
	Private m_bAONPRClientScreenChanges As Boolean
    Private m_oMembershipGroups As Object
	Private m_bIncludeClosedBranchChecked As Boolean
	
    Private m_bShowSubBranchID As Boolean
	Private m_bDuplicateClientIdentification As Boolean
	Private m_bCCNoOfEmployees As Boolean
	Private m_bCCTurnOverBand As Boolean
	
	'**************************************************
	Private m_sTaxNumber As String = ""
	Private m_bDomiciledForTax As Boolean
	Private m_bTaxExempt As Boolean
	Private m_dTaxPercentage As Double
	Private m_bSystemOptionClientBlacklistingInForce As Boolean
	Private m_vBlackListReasonId As Double
	'**************************************************
	Private m_vIsFeeClient As String = ""
    Private m_vSourceArray As Object
	Private m_oClientNumber As Object
	
	Private m_bIsSetMaskingCode As Boolean
	Private m_bIsReadOnly As Boolean
	Private m_sMaskCode As String = ""
	
    Private m_vPartyBankDetails As Object
	Private m_vPartyBankHistory As Object
	
	Private m_sCurrentResolvedName As String = ""
	
    <Browsable(True)>
	Public Property CurrentResolvedName() As String
		Get
			Return m_sCurrentResolvedName
		End Get
		Set(ByVal Value As String)
			m_sCurrentResolvedName = Value
		End Set
	End Property
	
    <Browsable(False)>
	Public ReadOnly Property Controls_Renamed() As Object
		Get
            Return Me.Controls
		End Get
	End Property
	
    <Browsable(False)>
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			Return m_lErrorNumber
		End Get
	End Property
	
    <Browsable(False)>
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			m_sCallingAppName = Value
		End Set
	End Property
	
    <Browsable(True)>
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
    <Browsable(True)>
	Public Property Task() As Integer
		Get
			Return m_iTask
		End Get
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
    <Browsable(False)>
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			m_lNavigate = Value
		End Set
	End Property
	
    <Browsable(False)>
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			m_lProcessMode = Value
		End Set
	End Property
	
    <Browsable(False)>
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			m_sTransactionType = Value
		End Set
	End Property
	
    <Browsable(False)>
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property

    <Browsable(True)>
	Public Property SwiftPartyID() As Integer
		Get
			Return m_lSwiftPartyID
		End Get
		Set(ByVal Value As Integer)
			m_lSwiftPartyID = Value
		End Set
	End Property
	
    <Browsable(True)>
	Public Property OrgName() As String
		Get
			Return txtName.Text
		End Get
		Set(ByVal Value As String)
			txtName.Text = Value
			RaiseEvent OrgNameChange()
		End Set
	End Property
	
    <Browsable(True)>
	Public Property PartySourceID() As Integer
		Get
			Return m_iPartySourceId
		End Get
		Set(ByVal Value As Integer)
			m_iPartySourceId = Value
		End Set
	End Property
	
    <Browsable(True)>
	Public Property PartyCnt() As Integer
		Get
			Return m_lPartyCnt
		End Get
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
    <Browsable(True)>
	Public Property ShortName() As String
		Get
			Return m_sShortName
		End Get
		Set(ByVal Value As String)
			m_sShortName = Value
		End Set
	End Property
    <Browsable(True)>
	Public Property LongName() As String
		Get
			Return m_sName
		End Get
		Set(ByVal Value As String)
			m_sName = Value
		End Set
	End Property
    <Browsable(True)>
	Public Property MainPostCode() As String
		Get
			Return m_sMainPostCode
		End Get
		Set(ByVal Value As String)
			m_sMainPostCode = Value
		End Set
	End Property
    <Browsable(True)>
	Public Property AddressLine1() As String
		Get
			Return m_sAddressLine1
		End Get
		Set(ByVal Value As String)
			m_sAddressLine1 = Value
		End Set
	End Property
	
    <Browsable(True)>
	Public Property FromEvent() As Boolean
		Get
			Return m_bEvent
		End Get
		Set(ByVal Value As Boolean)
			m_bEvent = Value
			RaiseEvent FromEventChange()
		End Set
	End Property
	
    <Browsable(True)>
	Public Property IsIncludeClosedBranchChecked() As Boolean
		Get
			Return m_bIncludeClosedBranchChecked
		End Get
		Set(ByVal Value As Boolean)
			m_bIncludeClosedBranchChecked = Value
		End Set
	End Property
	
    <Browsable(False)>
	Public ReadOnly Property BranchId() As Integer
		Get
			Dim result As Integer = 0
			If cboBranch.SelectedIndex > -1 Then
				result = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
			End If
			Return result
		End Get
	End Property
	
	'Party Bank Details
    <Browsable(False)>
	Public ReadOnly Property PartyBankDetails() As Object
		Get
			Return VB6.CopyArray(m_vPartyBankDetails)
		End Get
	End Property
	'Party Bank Details
    <Browsable(False)>
	Public ReadOnly Property PartyBankHistory() As Object
		Get
			Return m_vPartyBankHistory
		End Get
	End Property
	
	Public Function AddQASAddress(ByVal v_sAdd1 As String, ByVal v_sAdd2 As String, ByVal v_sAdd3 As String, ByVal v_sAdd4 As String, ByVal v_sPostCode As String) As Boolean
		
		Dim result As Boolean = False
		Try 
			
			result = True
			
			Dim oItem As ListViewItem
			Dim lBusinessDataID As Integer
			lBusinessDataID = 1
			
			'add address
			Dim temp_m_oQASAddress As Object
			m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oQASAddress, "bSIRAddress.Business", gPMConstants.PMGetViaClientManager)
			m_oQASAddress = temp_m_oQASAddress
			
			m_lReturn = m_oQASAddress.EditAdd(lRow:=lBusinessDataID, vAddress1:=v_sAdd1, vAddress2:=v_sAdd2, vAddress3:=v_sAdd3, vAddress4:=v_sAdd4, vPostalCode:=v_sPostCode, vCountryID:=m_iDefaultCountryID)

			Debug.WriteLine(m_oQASAddress.addresscnt)

			m_oQASAddress.Update()
			
			'add address to list view
			oItem = lvwAddresses.Items.Add(v_sPostCode)
			ListViewHelper.GetListViewSubItem(oItem, 1).Text = "Correspondence Address"
			ListViewHelper.GetListViewSubItem(oItem, 2).Text = v_sAdd1
			ListViewHelper.GetListViewSubItem(oItem, 3).Text = v_sAdd2
			ListViewHelper.GetListViewSubItem(oItem, 4).Text = v_sAdd3
			ListViewHelper.GetListViewSubItem(oItem, 5).Text = v_sAdd4

			oItem.Tag = m_oQASAddress.addresscnt
			
			'tidy up
			m_oQASAddress = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQASAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddQASAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	'sj 27/08/2002 - end
	
	' ***************************************************************** '
	' Name: DisplayCaptions
	'
	' Description: Display all language specific captions.
	'
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		Dim result As Integer = 0
		Dim iLanguageId As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_lReturn = gPMFunctions.GetUserIsAmericanLanguageID(iLanguageId)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
            lblYearToDateTurnover.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACYearToDateTurnOver, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblLastYearTurnover.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACLastYearTurnover, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLanguageId, MainModule.ACTabTitle1, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLanguageId, MainModule.ACTabTitle2, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLanguageId, MainModule.ACTabTitle3, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLanguageId, MainModule.ACTabTitle4, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 4, iPMFunc.GetResData(iLanguageId, MainModule.ACTabTitle5, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 5, iPMFunc.GetResData(iLanguageId, MainModule.ACTabTitle6, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 6, iPMFunc.GetResData(iLanguageId, MainModule.ACTabTitle7, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
            lblIDReference.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACClientCode, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblName.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACTradingName, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblMainContact.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACMainContact, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            fraBusinessDetails.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACBusinessDetails, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblBusiness.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACBusiness, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblOffices.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACNoOfOffices, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblTrade.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACTrade, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblEmployees.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACNoOfEmployees, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblCompanyReg.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACCompanyReg, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblTradingSince.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACTradingSince, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            fraAgent.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACLeadAgent, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdAgentLookUp.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACCode1, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdConsultantLookup.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACCode2, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblAgentName.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACName1, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblConsultantName.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACName2, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			' Tab 2
            cmdAddAd.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACAdd, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdDeleteAd.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACDelete, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEditAd.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACEdit, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Tab 3
            cmdAddCon.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACAdd, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdDeleteCon.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACDelete, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEditCon.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACEdit, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblPreferredCorrespondence.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACPreferredCorrespondence, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Tab 4
            cmdAddConv.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACAdd, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdDeleteConv.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACDelete, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEditConv.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACEdit, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblCCJ.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACCCC, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Tab 5
            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblServicelevel.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACServiceLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblPaymentMethod.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPaymentMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblCreditCard.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACCreditCardType, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblCreditCard.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACCreditCardType, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblReminderType.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACReminderType, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblTermsOfPayment.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACTermsOfPayment, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblFinancialYear.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACFinancialYear, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdAssociates.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACAssociates, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblArea.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACArea, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblFileCode.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACFileCode, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblWageRoll.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACWageRoll, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblTurnover.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACTurnover, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblSeasonalGift.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACSeasonalGift, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblRealArea.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACArea, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblRealFileCode.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACFileCode, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Tab 6
            cmdAddLoyaltyScheme.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACAdd, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdDeleteLoyaltyScheme.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACDelete, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEditLoyaltyScheme.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACEdit, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            fraLoyaltySchemes.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACLoyaltySchemes, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Tab 6 - End
            lblStrengthCode.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACStrengthCode, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblSICCode.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACSICCode, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblInsurerName.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACInsurerName, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblBrokerName.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACBrokerName, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            fraPreviousInsurer.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACPreviousInsurer, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdInsurerLookup.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACCode3, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            fraPreviousBroker.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACPreviousBroker, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdBrokerLookup.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACCode4, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblBranch.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACBranch, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			If m_bIsNRMA Then
				'This becomes "company name"
                lblName.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACCompanyName, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				'This becomes trading name
                lblCompanyReg.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACTradingName, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			End If
			
			If m_bAONAffinity Then
                fraAgent.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACAffinity, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                lblArea.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACTeam, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                lblFileCode.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACMembershipId, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			End If
			
            lblSubBranch.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACSubBranch, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblAlternativeIdentifier.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACAlternativeIdentifier, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblLoyaltyNumber.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.ACLoyaltyNumber, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            chkFeeClient.Text = CStr(iPMFunc.GetResData(iLanguageId, MainModule.AC_CAPTION_ISFEECLIENT, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			Dim sPOSTCODE_TITLE As Object
            sPOSTCODE_TITLE = iPMFunc.GetResData(iLanguageId, MainModule.ACPOSTCODETITLE, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
			
            'Check default country to see where Postcode is being displayed.
			Select Case (m_sDefaultCountryCode.Trim())
				Case "GBR"

					lvwAddresses.Columns.Item(0).Text = CStr(sPOSTCODE_TITLE)
					lvwAddresses.Columns.Item(1).Text = sADDR_USAGE_TITLE
					lvwAddresses.Columns.Item(2).Text = sADDR_LINE1_TITLE
					lvwAddresses.Columns.Item(3).Text = sADDR_LINE2_TITLE
					lvwAddresses.Columns.Item(4).Text = sADDR_LINE3_TITLE
					lvwAddresses.Columns.Item(5).Text = sADDR_LINE4_TITLE
					lvwAddresses.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1000))
					lvwAddresses.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2200))
					
				Case Else
					lvwAddresses.Columns.Item(0).Text = sADDR_USAGE_TITLE
					lvwAddresses.Columns.Item(1).Text = sADDR_LINE1_TITLE
					lvwAddresses.Columns.Item(2).Text = sADDR_LINE2_TITLE
					lvwAddresses.Columns.Item(3).Text = sADDR_LINE3_TITLE
					lvwAddresses.Columns.Item(4).Text = sADDR_LINE4_TITLE

					lvwAddresses.Columns.Item(5).Text = CStr(sPOSTCODE_TITLE)
					lvwAddresses.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(2200))
					lvwAddresses.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1000))
					
			End Select
			
			If m_bIsNRMA Then
                m_lReturn = PartyFunc.SetAddressHeaders(r_oAddresses:=lvwAddresses, v_sPostCode:=sPOSTCODE_TITLE, v_sAddressUsage:=sADDR_USAGE_TITLE)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	'JAS(CMG)05/09/02 added record_status
	' ***************************************************************** '
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' {* USER DEFINED CODE (Begin) *}
			'Text Boxes
			
			If m_bDuplicateClientIdentification Then
				'txtIDReference.Enabled = False
				'lblIDReference.FontBold = False
				'lblIDReference.ForeColor = QBColor(7)
				
				m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIDReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			Else
				'Reference must be entered
				m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIDReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			End If
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Name must be entered
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'DC 04/08/00 Main Contact
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtMainContact, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Agent Ref
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Consultant Ref
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtConsultantRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Company Reg
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCompanyReg, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Offices
			'ISS861 JAS 20/02/03 changed format to long
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOffices, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			'CCJ
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCCJ, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			'Trading Since Date
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTradingSince, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Financial Year
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFinancialYear, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'File Code
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFileCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'KB PN 5099 Real File Code
			
			'PN 18718 Need the file code in Underwriting also
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRealFileCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			'Record Status
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRecordStatus, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Wage Roll
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtWageRoll, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			
			' Previous Insurer
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsurerRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			' Previous Broker
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBrokerRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			
			'DJM 19/08/2003 : As mentioned above the normal way doesn't work so bodge it.
			If m_bBusinessFieldOnClientIsMandatory Then
				lblBusiness.Font = VB6.FontChangeBold(lblBusiness.Font, True)
			End If
			
			' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - trade combo
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTrade, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Currency must be entered
			'PSL 07/10/2002 Not any more. brought CC in line with PC for issue No  753
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCurrency, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' txtAgentReference
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			' hidden date
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtHiddenDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			' hidden currency
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtHiddenCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			
			' ctaf 270701
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSalutation, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			'DD 24/10/2003
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSource, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTPPassword, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString)
			
			'DJM 13/01/2004 : Copied from 1.8.5 issue 5877.
			If m_bAONPRClientScreenChanges Then
				m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboSICCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEmployees, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTurnover, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			Else
				If m_sUnderwritingOrAgency <> "U" Or m_bCCNoOfEmployees Then
					m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboEmployees, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
				Else
					m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboEmployees, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong)
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
				End If
				
				If m_sUnderwritingOrAgency <> "U" Or m_bCCTurnOverBand Then
					m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTurnover, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
				Else
					m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTurnover, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong)
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
				End If
				
			End If
			'FSA Phase III
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTobLetter, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			'FSA Phase III End
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			'Are we from events?

			m_oBusiness.FromEvent = FromEvent
			


            'developer guide no. 67
            m_lReturn = m_oBusiness.GetDetails(vPartyCnt:=m_lPartyCnt)
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
				
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: BusinessToInterface
	'
	' Description: Updates all interface details from the business
	'              object.
	'
	'JAS(CMG)03/09/02 - Added Record Status
	' ***************************************************************** '
	Public Function BusinessToInterface() As Integer
		
		Dim result As Integer = 0
		Dim sTemp, sDesc As String
		Dim sRecordStatus As String = ""
		Dim vTradingSinceDate, vFinancialYear As Object
		Dim sMainContactDesc As String = ""
		Dim sFormattedCurrency As String = ""
		Dim cConvertedAmount As Decimal
		
		Try 
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Assign the details from the business object
			' to the data storage.
			m_lReturn = BusinessToData()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Assign the details to the interface.
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtIDReference, vControlValue:=m_sShortName)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_sOldIdReference = m_sShortName
			m_sOldResolvedName = m_sName
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentRef, vControlValue:=m_sAgentRef)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			sTemp = m_sAgentName
			m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            lblPnlAgentName.Text = sTemp
			
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtConsultantRef, vControlValue:=m_sConsultantRef)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			sTemp = m_sConsultantName
			m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            lblPnlConsultantName.Text = sTemp
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iCurrencyId, lCompanyID:=g_iSourceID, cBaseAmount:=m_cClientBalance, cCurrencyAmount:=cConvertedAmount)
			
			m_cClientBalance = cConvertedAmount
			

			m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cClientBalance, vFormattedCurrency:=sFormattedCurrency)
			


            'pnlClientBalance.Name = sFormattedCurrency
            pnlClient.Text = sFormattedCurrency

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cYearToDateTurnover, vFormattedCurrency:=sFormattedCurrency)
			
            'pnlYearToDateTurnover.Name = sFormattedCurrency

            lblYearToDate.Text = sFormattedCurrency

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cLastYearTurnover, vFormattedCurrency:=sFormattedCurrency)
			
            'pnlLastYearTurnover.Name = sFormattedCurrency
            pnlLastYear.Text = sFormattedCurrency
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCompanyReg, vControlValue:=m_sCompanyReg)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If m_bAONAffinity Then
				Select Case m_sRecordStatus
					Case "aon_d", "aon_l"

						vTradingSinceDate = ""
					Case Else

						vTradingSinceDate = m_dtTradingSinceDate
				End Select
			Else

				vTradingSinceDate = m_dtTradingSinceDate
			End If
			
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTradingSince, vControlValue:=vTradingSinceDate)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtOffices, vControlValue:=m_lNoOfOffices)
			
			txtOffices.Text = CStr(m_lNoOfOffices)
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If m_bAONPRClientScreenChanges Then
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEmployees, vControlValue:=m_lNoOfEmployees)
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTurnover, vControlValue:=m_vTurnover)
			Else
				cboEmployees.ItemId = m_lNoOfEmployees
				cboTurnover.ItemId = gPMFunctions.NullToLong(m_vTurnover)
			End If
			
			If m_bAONAffinity Then
				Select Case m_sRecordStatus
					Case "aon_d", "aon_l"

						vFinancialYear = ""
					Case Else

						vFinancialYear = m_dtFinancialYear
				End Select
			Else

				vFinancialYear = m_dtFinancialYear
			End If
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFinancialYear, vControlValue:=vFinancialYear)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFileCode, vControlValue:=m_sFileCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			'PN 18718 Need the file code in Underwriting also
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRealFileCode, vControlValue:=m_sFileCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If m_bAONAffinity Then
				Select Case m_sRecordStatus
					Case "aon_d"
						sRecordStatus = "INACTIVE"
					Case "aon_l"
						sRecordStatus = "ACTIVE"
					Case Else
						sRecordStatus = ""
				End Select
			Else
				sRecordStatus = m_sRecordStatus
			End If
			
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRecordStatus, vControlValue:=sRecordStatus)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCCJ, vControlValue:=m_lCCJs)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If m_sPartyBusinessID <> "" Then
				sDesc = m_sPartyBusinessID
			End If
			
			ddBusiness.Text = sDesc
			sDesc = ""
			
			' Retrieve the Trade
			
			If m_sTradeCode <> "" Then
				sDesc = m_sTradeCode
			End If
			
			' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - replace employees textbox with combo
			If Not m_bReadTradeABIList Then
				cboTrade.ItemId = m_lTradeID
			Else
				ddTrade.Text = sDesc
				m_lTradeID = 0
			End If
			sDesc = ""
			
			' Retrieve the Payment Method
			If m_sPaymentMethodCode <> "" Then
				'        lReturn& = g_oGIS.GetDescriptionFromABICode(GIIMOccupation, GIIMOccupationEmployersBusiness, m_sPaymentMethodCode, sDesc$)
				sDesc = m_sPaymentMethodCode
			End If
			
			ddPaymentMethod.Text = sDesc
			sDesc = ""
			
			cboCreditCard.Text = m_sCreditCardCode.Trim()
			
			If ddPaymentMethod.Text = CreditCard Or ddPaymentMethod.Text = DebitCard Then
				cboCreditCard.Text = m_sCreditCardCode
				lblCreditCard.Visible = True
				cboCreditCard.Visible = True
			Else
				lblCreditCard.Visible = False
				cboCreditCard.Visible = False
				cboCreditCard.Text = ""
			End If
			
			chkAgent.CheckState = m_iIsAlsoAgent
			chkProspect.CheckState = m_iIsProspect
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtWageRoll, vControlValue:=m_vWageRoll)
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInsurerRef, vControlValue:=m_sInsurerRef)
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMainContact, vControlValue:=m_sMainContactDesc)
			
			chkTPS.CheckState = m_iTPSind
			chkMailshot.CheckState = m_iMailshot
			chkeMPS.CheckState = m_iEMPSind
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSource, vControlValue:=m_sSource)
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTPPassword, vControlValue:=m_sTPPassword)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			sTemp = m_sInsurerName
			m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            lblPnlInsurerName.Text = sTemp
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtBrokerRef, vControlValue:=m_sBrokerRef)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' CTAF 270701
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSalutation, vControlValue:=m_sSalutation)
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			sTemp = m_sBrokerName
			m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            lblPnlBrokerName.Text = sTemp
			
			'sj 13/06/2002 - start
			txtLoyaltyNumber.Text = m_sLoyaltyNumber.Trim()
			If m_bIsNRMA Then
				If m_sShortName.Trim() = m_sAlternativeIdentifier.Trim() Then
					txtAlternativeIdentifier.Text = PartyFunc.m_kBlankAlternativeIdentifier
				Else
					txtAlternativeIdentifier.Text = m_sAlternativeIdentifier.Trim()
				End If
			Else
				txtAlternativeIdentifier.Text = m_sAlternativeIdentifier.Trim()
			End If
			txtTradingName.Text = m_sTradingName.Trim()
			
			If m_bFutureDateAddressChanges Then

                m_lReturn = m_oBusiness.GetFutureDatedAddresses(m_vFutureDatedAddresses, m_lPartyCnt)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse
					iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface")
					Return result
				End If
			End If
			'FSA Phase III
			If m_dtTobLetter = CDate("00:00:00") Then
				txtTobLetter.Text = ""
			Else
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTobLetter, vControlValue:=m_dtTobLetter)
			End If
			'FSA Phase IIIEnd
			
			chkFeeClient.CheckState = IIf(gPMFunctions.ToSafeLong(m_vIsFeeClient) = 1, CheckState.Checked, CheckState.Unchecked)
			
			'Fill the contact grid
			PopulateContacts()
			
			'Fill the address grid
			PopulateAddresses()
			
			'    Fill the Convictions list view
			PopulateConvictions()
			'frmInterface.Caption = "Corporate Client: " & m_sShortName & " " & m_sMainPostCode
			
			'   Fill the Correspondence Type Combo Box
			PopulateCorrespondenceTypes()
			
			'    Fill the PartyLoyaltyScheme list view
			PopulateLoyaltySchemes() 'RAW 18/11/2002 : PS005 : Added
			
			'Party Bank Details
			LoadPartyBankControl()
			
			uctPartyTax1.TaxNumber = m_sTaxNumber
			uctPartyTax1.IsDomiciledForTax = m_bDomiciledForTax
			uctPartyTax1.TaxExempt = m_bTaxExempt
			uctPartyTax1.TaxPercentage = m_dTaxPercentage
			
			Return result
		
		Catch excep As System.Exception
			
			result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
			
		End Try

	End Function
	
	' ***************************************************************** '
	' Name: InterfaceToBusiness
	'
	' Description: Updates all business members from the interface
	'              details.
	'
	' ***************************************************************** '
	Public Function InterfaceToBusiness() As Integer
		
		Dim result As Integer = 0
		Dim lBusinessDataID As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the business object.
			
			' Assign the details from the interface to the data storage.
			m_lReturn = InterfaceToData()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the business data ID to one because we are only
			' dealing with one record item only.
			lBusinessDataID = 1
			
			' Check the task.
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMAdd
					' Inform the business object with a new data item.

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID,
                                                    vPartyCnt:=m_lPartyCnt, vCompanyReg:=m_sCompanyReg,
                                                    vTradingSinceDate:=m_dtTradingSinceDate, vPartyBusinessID:=m_sPartyBusinessID,
                                                    vNoOfOffices:=m_lNoOfOffices, vNoOfEmployees:=m_lNoOfEmployees,
                                                    vFinancialYear:=m_dtFinancialYear, vTradeCode:=m_sTradeCode,
                                                    vShortName:=m_sShortName, vName:=m_sName, vResolvedName:=m_sResolved,
                                                    vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect,
                                                    vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt,
                                                    vFileCode:=m_sFileCode, vCurrencyID:=m_iCurrencyId,
                                                    vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID,
                                                    vAreaId:=m_iAreaId, vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode,
                                                    vPaymentTermCode:=m_iPaymentTermId, vCCJs:=m_lCCJs,
                                                    vWageRoll:=m_vWageRoll, vTurnover:=m_vTurnover,
                                                    vSeasonalGiftID:=m_vSeasonalGiftId,
                                                    vSICCodeId:=m_vSICCodeId, vSourceID:=m_iNewPartySourceId,
                                                    vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vRenewalStopCodeId:=m_vRenewalStopCodeId,
                                                    vSwiftPartyID:=m_lSwiftPartyID, vSalutation:=m_sSalutation,
                                                    vLoyaltyNumber:=m_sLoyaltyNumber,
                                                    vAlternativeIdentifier:=m_sAlternativeIdentifier,
                                                    vTradingName:=m_sTradingName, vSubBranchId:=m_lSubBranchId, vTobLetter:=m_dtTobLetter,
                                                    vTradeID:=m_lTradeID, vTPSInd:=m_iTPSind, vMailshot:=m_iMailshot, vEMPSind:=m_iEMPSind,
               vSource:=m_sSource, vTPPassword:=m_sTPPassword, vIsFeeClient:=m_vIsFeeClient)
					
				Case gPMConstants.PMEComponentAction.PMEdit
					' Inform the business object with an updated data item.
                    m_lReturn = m_oBusiness.Editupdate(lRow:=lBusinessDataID, vPartyCnt:=m_lPartyCnt, vCompanyReg:=m_sCompanyReg, vTradingSinceDate:=m_dtTradingSinceDate, vPartyBusinessID:=m_sPartyBusinessID, vNoOfOffices:=m_lNoOfOffices, vNoOfEmployees:=m_lNoOfEmployees, vFinancialYear:=m_dtFinancialYear, vTradeCode:=m_sTradeCode, vShortName:=m_sShortName, vName:=m_sName, vResolvedName:=m_sResolved, vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt, vFileCode:=m_sFileCode, vCurrencyID:=m_iCurrencyId, vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, vAreaId:=m_iAreaId, vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, vPaymentTermCode:=m_iPaymentTermId, vCCJs:=m_lCCJs, vWageRoll:=m_vWageRoll, vTurnover:=m_vTurnover, vSeasonalGiftID:=m_vSeasonalGiftId, vSICCodeId:=m_vSICCodeId, vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vRenewalStopCodeId:=m_vRenewalStopCodeId, vSourceID:=m_iNewPartySourceId, vPartyID:=m_lNewPartyId, vSwiftPartyID:=m_lSwiftPartyID, vSalutation:=m_sSalutation, vLoyaltyNumber:=m_sLoyaltyNumber, vAlternativeIdentifier:=m_sAlternativeIdentifier, vTradingName:=m_sTradingName, vSubBranchId:=m_lSubBranchId, vTobLetter:=m_dtTobLetter, vTradeID:=m_lTradeID, vTPSInd:=m_iTPSind, vMailshot:=m_iMailshot, vEMPSind:=m_iEMPSind, vSource:=m_sSource, vTPPassword:=m_sTPPassword, vIsFeeClient:=m_vIsFeeClient)
			End Select
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
			End If
			
			Return result
		
		Catch excep As System.Exception
			result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
			
		End Try

	End Function
	
	' ***************************************************************** '
	' Name: DisplayLookupDetails
	'
	' Description: Displays all of the lookup details using the lookup
	'              values/details.
	'
	' ***************************************************************** '
	Public Function DisplayLookupDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            ' Get the lookup values.
			m_lReturn = GetLookupValues()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupArea, ctlLookup:=cboArea)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupArea, ctlLookup:=cboRealArea)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupProspectStatus, ctlLookup:=cboProspectingStatus)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupReminderType, ctlLookup:=cboReminderType)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupServiceLevel, ctlLookup:=cboServiceLevel)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRenewalStopCode, ctlLookup:=cboRenewalStopCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRiskGroup, ctlLookup:=cboPolicyType)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupSeasonalGift, ctlLookup:=cboSeasonalGift)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupSICCode, ctlLookup:=cboSICCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'eck 2005 Roadmap moved from Populate Prospect
			m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupStrengthCode, ctlLookup:=cboStrengthCode)

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPFFrequency, ctlLookup:=cboTermsOfPayment)
			
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
			
            SelectcboItem(cboTermsOfPayment, CInt(m_iPaymentTermId))
			
			m_lReturn = GetBranchDetails()

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = PartyFunc.GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=cboBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_lSubBranchId)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If m_bValidateAlternativeIdentifier Then
				'Get number validation scripts

				m_lReturn = m_oBusiness.GetNumberValidationScripts(v_sBranchPrefix:=m_sBranchPrefix, r_sLoyaltyNumberScript:=m_sLoyaltyNumberScript, r_sAlternativeIdentifierScript:=m_sAlternativeIdentifierScript)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetNumberValidationScripts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
					
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
            m_lReturn = m_oBusiness.GetRelationshipTypeLookups(m_vRelationships)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the relationship type lookups", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
			End If
			
			cmdAssociates.Enabled = Not (False)
			
			'blacklist reason id
            If m_bSystemOptionClientBlacklistingInForce Then
				
				m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupBlackListReason, ctlLookup:=cboBlackListReason, sInitialOption:="(not blacklisted)")
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				SelectcboItem(cboBlackListReason, CInt(m_vBlackListReasonId))
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
			
		End Try

	End Function
	
	' ***************************************************************** '
	' Name: PopulateAddresses
	'
	' Description: Fills the grid control with address details
	'
	' ***************************************************************** '
	Private Sub PopulateAddresses()
        Dim k As Integer
		Dim oListItem As ListViewItem
		Dim sAddressUsage As String = ""
		
		Try 
			
			'Just go if no addresses
			If Not Information.IsArray(m_vAddresses) Then
				Exit Sub
			End If
			
			lvwAddresses.Items.Clear()
			m_lAddressCount = 0
			
			' Assign the details to the interface.
			For i As Integer = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)
				For k = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
					If m_vAddresses(1, i).Equals(m_vAddressTypes(0, k)) Then
						sAddressUsage = CStr(m_vAddressTypes(1, k)).Trim()
						Exit For
					End If
				Next k
				
				'See if this is the main address
				If CStr(m_vAddressTypes(2, k)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
					m_sMainPostCode = CStr(m_vAddresses(0, i))
					m_iMainAddressIndex = CInt(m_vAddressTypes(0, k))
				End If
				
				'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
				Select Case (m_sDefaultCountryCode.Trim())
					Case "GBR"
						' Assign the details to the first column.
						' Postcode

                        oListItem = lvwAddresses.Items.Add(CStr(m_vAddresses(0, i)).Trim(), "AddressImage")
						
						' Assign details to other the columns
						' Address Usage
						ListViewHelper.GetListViewSubItem(oListItem, 1).Text = sAddressUsage
						' Address Line 1
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vAddresses(2, i)).Trim()
						' Address Line 2
						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vAddresses(3, i)).Trim()
						' Address Line 3
						ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vAddresses(4, i)).Trim()
						' Address Line 4
						ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vAddresses(5, i)).Trim()
						
					Case Else
						' Assign the details to the first column.
						' Address Usage

                        oListItem = lvwAddresses.Items.Add(sAddressUsage, "AddressImage")
						
						' Assign details to other the columns
						' Address Line 1
						ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAddresses(2, i)).Trim()
						' Address Line 2
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vAddresses(3, i)).Trim()
						' Address Line 3
						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vAddresses(4, i)).Trim()
						' Address Line 4
						ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vAddresses(5, i)).Trim()
						' Postcode
						ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vAddresses(0, i)).Trim()
						
				End Select
				
				' Store the Address_cnt
				oListItem.Tag = CStr(m_vAddresses(6, i)).Trim()
				
				' {* USER DEFINED CODE (End) *}
				
				m_lAddressCount += 1
				' Set the tag property with the index of
				' the search data storage.
				
			Next i
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: PopulateContacts
	'
	' Description: Fills the grid control with contact details
	'
	' ***************************************************************** '
	Private Sub PopulateContacts()
		
		Dim oListItem As ListViewItem
		
		
		Try 
			
			If Not Information.IsArray(m_vContacts) Then
				Exit Sub
			End If
			lvwContacts.Items.Clear()
			
			
			' Assign the details to the interface.
			For i As Integer = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)
				
				' {* USER DEFINED CODE (Begin) *}
				
				' Assign the details to the first column.
				' Column 1

                oListItem = lvwContacts.Items.Add(CStr(m_vContacts(1, i)).Trim(), "ContactImage")
				
				' Assign details to other the columns
				' Column 2
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vContacts(2, i)).Trim()
				
				' Column 3
				ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vContacts(3, i)).Trim()
				
				' Column 4
				ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vContacts(4, i)).Trim()
				
				' Column 5
				ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vContacts(5, i)).Trim()
				
				
				' Store the Contact_cnt
				oListItem.Tag = CStr(m_vContacts(0, i)).Trim()
				' {* USER DEFINED CODE (End) *}
				
				' Set the tag property with the index of
				' the search data storage.
				
			Next i
			'    'Populate the cells
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	'EK 9/12/99
	' ***************************************************************** '
	' Name: PopulateConvictions
	'
	' Description: Fills the grid control with contact details
	'
	' ***************************************************************** '
	Private Sub PopulateConvictions()
		Dim oListItem As ListViewItem
		
		Try 
			
			If Not Information.IsArray(m_vConvictions) Then
				Exit Sub
			End If
			
			lvwConvictions.Items.Clear()
			'EK 12/12/99 Correct column/data matching
			' Assign the details to the interface.
			For i As Integer = m_vConvictions.GetLowerBound(1) To m_vConvictions.GetUpperBound(1)
				
				' {* USER DEFINED CODE (Begin) *}
				
				' Assign the details to the first column.
				' Column 1

                oListItem = lvwConvictions.Items.Add(CStr(m_vConvictions(2, i)).Trim(), "ConvictionImage")
				
				' Assign details to other the columns
				' Date
				
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=CStr(m_vConvictions(3, i)).Trim())
				
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text.Trim()
				
				' Description
				ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vConvictions(4, i)).Trim()
				
				' Fine
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenCurrency, vControlValue:=CStr(m_vConvictions(5, i)).Trim())
				
				ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text.Trim()
				
				' Conviction Status
				ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vConvictions(11, i)).Trim()
				
				' Penalty points
				ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vConvictions(14, i)).Trim()
				
				' Store the Conviction_cnt
				oListItem.Tag = CStr(m_vConvictions(1, i)).Trim()
				' {* USER DEFINED CODE (End) *}
				
				' Set the tag property with the index of
				' the search data storage.
				
			Next i
			'    'Populate the cells
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateConvictions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: PopulateCorrespondenceTypes
	'
	' Description: Populate Correspondence Types
	'
	' ***************************************************************** '
	Private Function PopulateCorrespondenceTypes() As Integer
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear Correspondence List
			cboCorrespondenceType.Items.Clear()
			cboCorrespondenceType.SelectedIndex = -1
			
			' Check if there is a list
			If Not Information.IsArray(m_vCorrespondenceTypes) Then
				Return result
			End If
			
			' Assign the details to the interface.
			For lRow As Integer = m_vCorrespondenceTypes.GetLowerBound(1) To m_vCorrespondenceTypes.GetUpperBound(1)
				
				Dim cboCorrespondenceType_NewIndex As Integer = -1
				cboCorrespondenceType_NewIndex = cboCorrespondenceType.Items.Add(CStr(m_vCorrespondenceTypes(1, lRow)))
				VB6.SetItemData(cboCorrespondenceType, cboCorrespondenceType_NewIndex, CInt(m_vCorrespondenceTypes(0, lRow)))
				
				If CDbl(m_vCorrespondenceTypes(0, lRow)) = m_vCorrespondenceTypeId Then
					cboCorrespondenceType.SelectedIndex = cboCorrespondenceType_NewIndex
					cboCorrespondenceType.Text = CStr(m_vCorrespondenceTypes(1, lRow))
				End If
				
			Next lRow
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Populate Corresondence Types", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCorrespondenceTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateLoyaltySchemes
	'
	' Description: Fills the grid control with PartyLoyaltyScheme details
	'
	' ***************************************************************** '
	Private Sub PopulateLoyaltySchemes() 'RAW 18/11/2002 : PS005 : Added
		
		
		Dim oListItem As ListViewItem
		
		Try 
			
			If Not Information.IsArray(m_vLoyaltySchemes) Then
				Exit Sub
			End If
			
			lvwLoyaltySchemes.Items.Clear()
			
			' Assign the details to the interface.
			For i As Integer = m_vLoyaltySchemes.GetLowerBound(1) To m_vLoyaltySchemes.GetUpperBound(1)
				
				' {* USER DEFINED CODE (Begin) *}
				
				' Assign the details to the first column.
				' Column 1
				oListItem = lvwLoyaltySchemes.Items.Add(CStr(m_vLoyaltySchemes(PMBLoyaltyLoyaltySchemeName, i)).Trim())
				
				' Assign details to other the columns
				
				' MemberNumber
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vLoyaltySchemes(PMBLoyaltyMemberNumber, i)).Trim()
				
				' StartDate
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=CStr(m_vLoyaltySchemes(PMBLoyaltyStartDate, i)).Trim())
				
				ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtHiddenDate.Text.Trim()
				
				' EndDate
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=CStr(m_vLoyaltySchemes(PMBLoyaltyEndDate, i)).Trim())
				
				ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenDate.Text.Trim()
				
				' Store the PartyLoyaltySchemeId
				oListItem.Tag = CStr(m_vLoyaltySchemes(PMBLoyaltyPartyLoyaltySchemeID, i)).Trim()
				' {* USER DEFINED CODE (End) *}
				
				' Set the tag property with the index of
				' the search data storage.
				
			Next i
			'    'Populate the cells
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateLoyaltySchemes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: ValidateOK
	'
	' Description: This validates mandatory address types and duplicate
	' addresses
	'
	' ***************************************************************** '
	Private Function ValidateOK() As Integer
		'RWH(11/07/2000) Altered to move PostCode from far left to far
		'right of ListView.
		
		Dim result As Integer = 0
		Dim iMainAddresses As Integer
		Dim bDuplicate As Boolean
		Dim lAddressCnt As Integer
		Dim oListItem, oListItem2 As ListViewItem
		Dim sAddressUsage, sChangeCode As String
		Dim sPartyCode, sMsg As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'********************************************************************************
			'MKR 12/10/2004 PN 6021 - Restricting Entry of Spl. Chars in ClientCode   --Start
			txtIDReference.Text = txtIDReference.Text.Trim()
			If txtIDReference.Text.Trim() = "" Then
				MessageBox.Show("Must have Client Code", "Corporate Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
				SSTabHelper.SetSelectedIndex(tabMainTab, 0)
				If txtIDReference.Enabled Then
					txtIDReference.Focus()
				End If
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			If txtIDReference.Text.IndexOf("'"c) >= 0 OrElse txtIDReference.Text.IndexOf("’"c) >= 0 Then
				MessageBox.Show("' (Apostrophes) are not Allowed in Client Code", "Corporate Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
				SSTabHelper.SetSelectedIndex(tabMainTab, 0)
				If txtIDReference.Enabled Then
					txtIDReference.Focus()
				End If
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			If txtIDReference.Text.IndexOf("|"c) >= 0 Then
				MessageBox.Show("| (Pipes) are not Allowed in Client Code", "Corporate Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
				SSTabHelper.SetSelectedIndex(tabMainTab, 0)
				If txtIDReference.Enabled Then
					txtIDReference.Focus()
				End If
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			If txtIDReference.Text.IndexOf(","c) >= 0 Then
				MessageBox.Show(", (Commas) are not Allowed in Client Code", "Corporate Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
				SSTabHelper.SetSelectedIndex(tabMainTab, 0)
				If txtIDReference.Enabled Then
					txtIDReference.Focus()
				End If
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			'MKR 12/10/2004 PN 6021 - Restricting Entry of Spl. Chars in ClientCode   --End
			'********************************************************************************
			
			'Check to see if client code already exists
			
			'If this is a new client or an existing one with it's client code changed then
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or (m_iTask = gPMConstants.PMEComponentAction.PMEdit And txtIDReference.Text.Trim().ToUpper() <> m_sOldIdReference.Trim().ToUpper()) Then
				
				sPartyCode = txtIDReference.Text.Trim()
				

				m_lReturn = m_oBusiness.CheckReference(sPartyCode)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					MessageBox.Show("Unable to access business object", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				'If the returned code is an empty string, then the code already exists
				If sPartyCode = "" Then
					
					sMsg = "The client code entered already exists."
					MessageBox.Show(sMsg, "Invalid Client Code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				'Add this just in case Apply is added at later stage
				m_sOldIdReference = txtIDReference.Text
			End If
			
			'Display warning message that client code has changed.
			If m_iTask = gPMConstants.PMEComponentAction.PMEdit And txtIDReference.Text.Trim().ToUpper() <> m_sOldIdReference.Trim().ToUpper() Then
				
				sChangeCode = CStr(MessageBox.Show("Warning! You are about to change the client code. Do you wish to change the code?", "Client Code Changed", MessageBoxButtons.YesNo))
				Select Case sChangeCode
					Case CStr(System.Windows.Forms.DialogResult.No)
						txtIDReference.Text = m_sOldIdReference
				End Select
				
			End If
			

            ' Validate Preferred Correspondence Type
            Dim bValidCorrespondenceType As Boolean = False
            If cboCorrespondenceType.SelectedIndex = -1 AndAlso lvwContacts.Items.Count = 0 Then
                bValidCorrespondenceType = True
            ElseIf cboCorrespondenceType.SelectedIndex <> -1 AndAlso lvwContacts.Items.Count = 0 _
                AndAlso cboCorrespondenceType.SelectedItem.ToString.Trim.ToUpper <> "LETTER" Then
                bValidCorrespondenceType = False
            ElseIf cboCorrespondenceType.SelectedIndex = -1 AndAlso lvwContacts.Items.Count > 0 Then
                bValidCorrespondenceType = True
            Else
                If cboCorrespondenceType.SelectedItem.ToString.Trim.ToUpper = "LETTER" Then
                    bValidCorrespondenceType = True
                Else
                    For Each item As ListViewItem In lvwContacts.Items
                        If item.SubItems(3).Text.Trim.ToUpper = cboCorrespondenceType.SelectedItem.ToString.Trim.ToUpper Then
                            bValidCorrespondenceType = True
                            Exit For
                        End If
                    Next
                End If
            End If

            If Not bValidCorrespondenceType Then
                MessageBox.Show("No corresponding contact type exists. Please add one or change the preferred method of correspondence. ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                cboCorrespondenceType.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iMainAddresses = 0
			
			'Count how many addresses are main address
			If lvwAddresses.Items.Count > 0 Then
				For i As Integer = 1 To lvwAddresses.Items.Count
					oListItem = lvwAddresses.Items.Item(i - 1)
					
					'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
					Select Case (m_sDefaultCountryCode.Trim())
						Case "GBR"
							sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
						Case Else
							sAddressUsage = oListItem.Text.Trim()
					End Select
					
					For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
						'RWH(24/07/2000)
						If (sAddressUsage = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
							iMainAddresses += 1
						End If
					Next j
				Next i
			End If
			
			Select Case iMainAddresses
				Case 0
					'No
					MessageBox.Show("You must have an address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
					result = gPMConstants.PMEReturnCode.PMFalse
					SSTabHelper.SetSelectedIndex(tabMainTab, 1)
					Return result
				Case 1
					'Yes
				Case Else
					'No.
					MessageBox.Show("You can have only one address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
					result = gPMConstants.PMEReturnCode.PMFalse
					SSTabHelper.SetSelectedIndex(tabMainTab, 1)
					Return result
			End Select
			
			'Now Ensure addresses are not used twice
			'EK 15/11/99 Use latest count
			'If (m_lAddressCount < 2) Then
			If lvwAddresses.Items.Count < 2 Then
				'less than 2 addresses so cant have duplicates
				Return result
			End If
			
			bDuplicate = False
			
			'Check for duplicates
			'Need to find out how to calculate no of lines in the grid
			'EK 15/11/99 Use latest count
			'For i = 1 To (m_lAddressCount)
			For i As Integer = 1 To (lvwAddresses.Items.Count)
				oListItem = lvwAddresses.Items.Item(i - 1)
				If CBool(CStr(Convert.ToString(oListItem.Tag) <> "").Trim()) Then
					lAddressCnt = Convert.ToString(oListItem.Tag)
					'EK 15/11/99
					'For j = (i + 1) To m_lAddressCount
					For j As Integer = 1 To lvwAddresses.Items.Count
						If i <> j Then
							oListItem2 = lvwAddresses.Items.Item(j - 1)
							If CBool(CStr(Convert.ToString(oListItem2.Tag) <> "").Trim()) Then
								If (Convert.ToString(oListItem2.Tag)) = lAddressCnt Then
									bDuplicate = True
									Exit For
								End If
							End If
						End If
					Next j
				End If
			Next i
			
			If bDuplicate Then
				MessageBox.Show("An address can only be used once by a particular party.", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: UpdateAddressPostCodeProperties
	'
	' Description: This checks for the main address and gets the
	' post code and address line 1 for it via the address business
	'
	' ***************************************************************** '
	Private Sub UpdateAddressPostCodeProperties()
		Dim lAddressCnt As Integer

		Dim oAddressBusiness As bSIRAddress.Business
		Dim sAddressUsage As String = ""
		
		
		Try 
			
			'Find the main address
			For i As Integer = 1 To lvwAddresses.Items.Count
				
				'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
				Select Case (m_sDefaultCountryCode.Trim())
					Case "GBR"
						sAddressUsage = ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(i - 1), 1).Text.Trim()
					Case Else
						sAddressUsage = lvwAddresses.Items.Item(i - 1).Text.Trim()
				End Select
				
				For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
					'RWH(24/07/2000)
					If (sAddressUsage = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
						lAddressCnt = Convert.ToString(lvwAddresses.Items.Item(i - 1).Tag)
						Exit For
					End If
				Next j
			Next i
			
			'Get address business to retrieve
			Dim temp_oAddressBusiness As Object
			m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_oAddressBusiness, "bSIRAddress.Business", "ClientManager")
			oAddressBusiness = temp_oAddressBusiness
			
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			

            'developer guide no. 218
            ReflectionHelper.SetMember(oAddressBusiness, "AddressCnt", lAddressCnt)
			


            'developer guide no. 37
            m_lReturn = oAddressBusiness.GetDetails(vAddressCnt:=lAddressCnt)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oAddressBusiness.Dispose()
				oAddressBusiness = Nothing
				Exit Sub
			End If
			



            'developer guide no. 37
            m_lReturn = oAddressBusiness.GetNext(vPostalCode:=m_sMainPostCode, vAddress1:=m_sAddressLine1)

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oAddressBusiness.Dispose()
				oAddressBusiness = Nothing
				Exit Sub
			End If
			

            oAddressBusiness.Dispose()
			oAddressBusiness = Nothing
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateAddressPostCodePropertiesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddressPostCodeProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' PUBLIC Methods (End)
	
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	'JAS(CMG)03/09/02 - Added m_sRecordStatus
	'***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			'SP090998
			'DC 03/05/00
			'Multi-Branch - Added Party Source Id
			'DC 28/06/00
			'Added Correspondence Type Id
			'FSA Phase III Tob Letter














































            'developer guide no. 67(latest guide)
            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vSourceID:=m_iPartySourceId,
        vPartyID:=m_lPartyId, vCompanyReg:=m_sCompanyReg,
        vTradingSinceDate:=m_dtTradingSinceDate, vPartyBusinessID:=m_sPartyBusinessID,
        vNoOfOffices:=m_lNoOfOffices, vNoOfEmployees:=m_lNoOfEmployees,
        vFinancialYear:=m_dtFinancialYear, vTradeCode:=m_sTradeCode,
        vShortName:=m_sShortName, vName:=m_sName,
        vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect,
        vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt,
        vFileCode:=m_sFileCode, vRecordStatus:=m_sRecordStatus, vCurrencyID:=m_iCurrencyId,
        vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID,
        vAreaId:=m_iAreaId, vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode,
        vPaymentTermCode:=m_iPaymentTermId, vCCJs:=m_lCCJs,
        vWageRoll:=m_vWageRoll, vTurnover:=m_vTurnover,
        vSICCodeId:=m_vSICCodeId, vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vRenewalStopCodeId:=m_vRenewalStopCodeId,
        vSwiftPartyID:=m_lSwiftPartyID,
        vSalutation:=m_sSalutation,
        vLoyaltyNumber:=m_sLoyaltyNumber,
        vAlternativeIdentifier:=m_sAlternativeIdentifier,
        vTradingName:=m_sTradingName, vSubBranchId:=m_lSubBranchId, vTobLetter:=m_dtTobLetter,
        vTradeID:=m_lTradeID, vTPSInd:=m_iTPSind, vMailshot:=m_iMailshot, vEMPSind:=m_iEMPSind,
        vSource:=m_sSource, vTPPassword:=m_sTPPassword, vIsFeeClient:=m_vIsFeeClient)

			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			' returns additional party details
			' these detail are not returned from get next because of parameter limit of 60
			m_lReturn = GetPartyDetails()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			'Get additional details required for display that not stored on this
			'record

            'developer guide no. 67(latest guide)
            m_lReturn = m_oBusiness.GetOtherDetails(vAgentCnt:=m_lAgentCnt,
                                          vAgentref:=m_sAgentRef$,
                                          vAgentName:=m_sAgentName,
                                          vConsultantCnt:=m_lConsultantCnt,
                                          vConsultantRef:=m_sConsultantRef,
                                          vConsultantName:=m_sConsultantName,
                                          vInsurerCnt:=m_vPreviousInsurerCnt,
                                          vInsurerRef:=m_sInsurerRef,
                                          vInsurerName:=m_sInsurerName,
                                          vBrokerCnt:=m_vPreviousBrokerCnt,
                                          vBrokerRef:=m_sBrokerRef,
                                          vBrokerName:=m_sBrokerName,
                                          vPartyCnt:=m_lPartyCnt,
                                          vAssociates:=m_vAssociates)

			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
            If IsArray(m_vAssociates) Then
                lblAssociates.Visible = True
            Else
                lblAssociates.Visible = False
            End If
			'DC 04/08/00

            'developer guide no. 67(latest guide)
            m_lReturn = m_oBusiness.GetMainContact(lMainContactCnt:=m_lMainContactCnt,
                                            sMainContactDesc:=m_sMainContactDesc)

			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the main contact from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			'Get addresses for the party

            'developer guide no. 67(latest guide)
            m_lReturn = m_oBusiness.GetAddressDetails(
                                 vAddresses:=m_vAddresses)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			'Get addresse type lookups for the party

            'developer guide no. 67(latest guide)
            m_lReturn = m_oBusiness.GetAddresstypelookups(
                                vaddresstypes:=m_vAddressTypes)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			'Get contacts for the party

            'developer guide no. 67(latest guide)
            m_lReturn = m_oBusiness.GetContactDetails(
                               vContacts:=m_vContacts)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the contact details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			'EK 9/12/99
			'Get convictions for the Party

            'developer guide no. 67(latest guide)
            m_lReturn = m_oBusiness.GetConvictionDetails(vPartyCnt:=m_lPartyCnt, vConviction:=m_vConvictions)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the conviction details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			'DC 28/06/00
			'Get Correspondence Types

            'developer guide no. 67(latest guide)
            m_lReturn = m_oBusiness.GetCorrespondenceTypes(vCorrespondenceTypes:=m_vCorrespondenceTypes)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the correspondence types from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			'RAW 18/11/2002 : PS005 : Added - Begin
			'Get loyalty schemes for the Party

            'developer guide no. 67(latest guide)
            m_lReturn = m_oBusiness.GetLoyaltySchemeDetails(
                                    vPartyCnt:=m_lPartyCnt,
                                    vLoyaltySchemes:=m_vLoyaltySchemes)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the Loyalty Scheme details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			'RAW 18/11/2002 : PS005 : End
			'2005 Layout Changes - Get Client Account Details figures

            'developer guide no. 67(latest guide)
            m_lReturn = m_oAccount.GetClientAccountDetails(
                                    v_lAccountKey:=m_lPartyCnt,
                                    v_lCompanyID:=m_iPartySourceId,
                                    r_curYearToDateTurnover:=m_cYearToDateTurnover,
                                    r_curLastYearTurnover:=m_cLastYearTurnover,
                                    r_curClientBalance:=m_cClientBalance)

            ' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve turnover details from the account business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			'2005End
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			


			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: InterfaceToData
	'
	' Description: Updates the data storage from the interface details.
	'
	' ***************************************************************** '
	Private Function InterfaceToData() As Integer
		
		Dim result As Integer = 0
		Dim sMsg As String = ""
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_sShortName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtIDReference))
			
			
			

			m_sName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtName))
			


			m_sMainContactDesc = CStr(m_oFormFields.UnformatControl(ctlControl:=txtMainContact))
			
			m_sResolved = m_sName
			If CurrentResolvedName <> "" Then
				If CurrentResolvedName <> m_sResolved Then
					If m_sOldResolvedName <> m_sResolved Then
						If MessageBox.Show("The client name has been updated do you want to update the policyholder field on all of the policy records?", "Client Name", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
							m_sResolved = m_sResolved.Trim()
						Else
							m_sResolved = CurrentResolvedName.Trim()
						End If
					Else
						m_sResolved = CurrentResolvedName.Trim()
					End If
				End If
			End If

			m_sCompanyReg = CStr(m_oFormFields.UnformatControl(ctlControl:=txtCompanyReg))

			m_dtTradingSinceDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtTradingSince))


			m_lNoOfOffices = CInt(m_oFormFields.UnformatControl(ctlControl:=txtOffices))
			
			If m_bAONPRClientScreenChanges Then

				m_lNoOfEmployees = CInt(m_oFormFields.UnformatControl(ctlControl:=txtEmployees))

				m_vTurnover = CInt(m_oFormFields.UnformatControl(ctlControl:=txtTurnover))
			Else
				m_lNoOfEmployees = cboEmployees.ItemId
				m_vTurnover = cboTurnover.ItemId
			End If
			

			m_dtFinancialYear = CDate(m_oFormFields.UnformatControl(ctlControl:=txtFinancialYear))

			m_lCCJs = CInt(m_oFormFields.UnformatControl(ctlControl:=txtCCJ))
			'Pick up value de[pending on if AON or not
			If m_bAONAffinity Then

				m_sFileCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtFileCode))
			Else


				m_sFileCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtRealFileCode))
				
			End If
			


			m_vWageRoll = m_oFormFields.UnformatControl(ctlControl:=txtWageRoll)
			
			' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development
			If Not m_bReadTradeABIList Then
				m_lTradeID = cboTrade.ItemId
			End If
			
			m_sSalutation = m_oFormFields.UnformatControl(ctlControl:=txtSalutation)
			
			m_sPartyBusinessID = ddBusiness.Text
			
			m_sTradeCode = ddTrade.Text
			
			m_sPaymentMethodCode = ddPaymentMethod.Text
			
			If cboCreditCard.SelectedIndex <> -1 Then
				m_sCreditCardCode = cboCreditCard.Text
			Else
				m_sCreditCardCode = ""
			End If
			
			If cboArea.SelectedIndex <> -1 Then
				m_iAreaId = VB6.GetItemData(cboArea, cboArea.SelectedIndex)
			Else
				m_iAreaId = 0
			End If
			
			If cboRealArea.Visible Then
				If cboRealArea.SelectedIndex <> -1 Then
					m_iAreaId = VB6.GetItemData(cboRealArea, cboRealArea.SelectedIndex)
				Else
					m_iAreaId = 0
				End If
			End If
			
			m_iCurrencyId = cboCurrency.CurrencyId
			
			If cboReminderType.SelectedIndex <> -1 Then
				m_lReminderTypeID = VB6.GetItemData(cboReminderType, cboReminderType.SelectedIndex)
			Else
				m_lReminderTypeID = 0
			End If
			
			If cboServiceLevel.SelectedIndex <> -1 Then
				m_lServiceLevelId = VB6.GetItemData(cboServiceLevel, cboServiceLevel.SelectedIndex)
			Else
				m_lServiceLevelId = 0
			End If
			
			If cboSeasonalGift.SelectedIndex <> -1 Then
				m_vSeasonalGiftId = VB6.GetItemData(cboSeasonalGift, cboSeasonalGift.SelectedIndex)
			Else

				m_vSeasonalGiftId = Nothing
			End If
			
			If cboSICCode.SelectedIndex <> -1 Then
				m_vSICCodeId = VB6.GetItemData(cboSICCode, cboSICCode.SelectedIndex)
			Else

				m_vSICCodeId = Nothing
			End If
			
			'DC 28/06/00
			'Added Correspondence Type Id
			If cboCorrespondenceType.SelectedIndex <> -1 Then
				m_vCorrespondenceTypeId = VB6.GetItemData(cboCorrespondenceType, cboCorrespondenceType.SelectedIndex)
			Else

				m_vCorrespondenceTypeId = Nothing
			End If
			
			'Tomo060700
			'Added Renewal Stop Code Id
			If cboRenewalStopCode.SelectedIndex <> -1 Then
				m_vRenewalStopCodeId = VB6.GetItemData(cboRenewalStopCode, cboRenewalStopCode.SelectedIndex)
			Else

                m_vRenewalStopCodeId = 0
			End If
			
			'Check Boxes
			If chkAgent.CheckState = CheckState.Unchecked Then
				m_iIsAlsoAgent = CheckState.Unchecked
			Else
				m_iIsAlsoAgent = CheckState.Checked
			End If
			
			If chkProspect.CheckState = CheckState.Unchecked Then
				m_iIsProspect = CheckState.Unchecked
			Else
				m_iIsProspect = CheckState.Checked
			End If
			
			'eck010900
			'DN 01/06/01 - Change PartyIDs to Longs
			If cboBranch.SelectedIndex <> -1 Then
				If m_iPartySourceId <> VB6.GetItemData(cboBranch, cboBranch.SelectedIndex) Then
					' Don't show message box for new client
					If m_iPartySourceId > 0 Then
						MessageBox.Show("You are about to change the client branch remember to change any relevant policies", Application.ProductName)
					End If
					m_lNewPartyId = 0
				Else
					m_lNewPartyId = m_lPartyId
				End If
				m_iNewPartySourceId = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
			Else
				'MKW180703 PN5425 Default to original party source id (if available)
				If m_iPartySourceId > 0 Then
					m_iNewPartySourceId = m_iPartySourceId
				Else
					m_iNewPartySourceId = MainModule.g_iSourceID
				End If
				'MKW180703 PN5425
			End If
			
			'sj 13/06/2002 - start
			m_sLoyaltyNumber = txtLoyaltyNumber.Text.Trim()
			If m_bIsNRMA Then
				If txtAlternativeIdentifier.Text.Trim() = PartyFunc.m_kBlankAlternativeIdentifier Then
					m_sAlternativeIdentifier = m_sShortName.Trim()
				Else
					m_sAlternativeIdentifier = txtAlternativeIdentifier.Text.Trim()
				End If
			Else
				m_sAlternativeIdentifier = txtAlternativeIdentifier.Text.Trim()
			End If
			
			m_sTradingName = txtTradingName.Text.Trim()
			'sj 17/07/2002 - start
			'Make sure we have a sub branch selected
			If cboSubBranch.SelectedIndex >= 0 Then
				m_lSubBranchId = CInt(Conversion.Val(CStr(VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex))))
			Else
				m_lSubBranchId = 0
			End If
			'sj 17/07/2002 - end
			'sj 13/06/2002 - end
			
			'DD 24/10/2003
			If chkTPS.CheckState = CheckState.Unchecked Then
				m_iTPSind = CheckState.Unchecked
			Else
				m_iTPSind = CheckState.Checked
			End If
			If chkMailshot.CheckState = CheckState.Unchecked Then
				m_iMailshot = CheckState.Unchecked
			Else
				m_iMailshot = CheckState.Checked
			End If
			If chkeMPS.CheckState = CheckState.Unchecked Then
				m_iEMPSind = CheckState.Unchecked
			Else
				m_iEMPSind = CheckState.Checked
			End If

			m_sSource = CStr(m_oFormFields.UnformatControl(ctlControl:=txtSource))

			m_sTPPassword = CStr(m_oFormFields.UnformatControl(ctlControl:=txtTPPassword))
			
			'Validation of following not required if we are cancelling out
			
			If Status <> gPMConstants.PMEReturnCode.PMCancel Then
				
				'Get party count for Agent (if valid ref supplied)
				If (txtAgentRef.Text.Trim() <> "") And (m_bVerifyAgentCnt) Then
					

					m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtAgentRef.Text.Trim(), vPartyCnt:=m_lAgentCnt)
					
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
					
					If m_lAgentCnt = 0 Then
						
						'DC101204
						If m_sUnderwritingOrAgency = "U" Then
							
                            'developer guide no. 243
                            sMsg = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACAgentMissing, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
							MessageBox.Show("Agent Missing", Application.ProductName)
							
						Else
							
                            'developer guide no. 243
                            sMsg = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACThirdPartyMissing, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
							MessageBox.Show("Third Party Missing", Application.ProductName)
							
						End If
						
						iPMFunc.SelectText(txtAgentRef)
						txtAgentRef.Focus()
						
						Return gPMConstants.PMEReturnCode.PMFalse
						
					End If
					
				Else
					
					If txtAgentRef.Text.Trim() = "" Then
						m_lAgentCnt = 0
					Else
						m_lAgentCnt = CInt(Convert.ToString(txtAgentRef.Tag))
					End If
					
				End If
				
				'Get party count for Consultant (if valid ref supplied)
				If (txtConsultantRef.Text.Trim() <> "") And (m_bVerifyConsultantCnt) Then
					

					m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtConsultantRef.Text.Trim(), vPartyCnt:=m_lConsultantCnt)
					
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
					
					If m_lConsultantCnt = 0 Then
						
						'DC101204
						If m_sUnderwritingOrAgency = "U" Then
							

                            'developer guide no. 243
                            sMsg = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACAgentMissing, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
							
						Else
							

                            'developer guide no. 243
                            sMsg = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACThirdPartyMissing, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
							
						End If
						
						MessageBox.Show(sMsg, "Corporate Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
						
						iPMFunc.SelectText(txtConsultantRef)
						txtConsultantRef.Focus()
						
						Return gPMConstants.PMEReturnCode.PMFalse
						
					End If
					
				Else
					
					If txtConsultantRef.Text.Trim() = "" Then
						m_lConsultantCnt = 0
					Else
						m_lConsultantCnt = CInt(Convert.ToString(txtConsultantRef.Tag))
					End If
					
				End If
				
			End If
			'FSA Phase III
			' Unformat field correctly. PN17181

			m_dtTobLetter = CDate(m_oFormFields.UnformatControl(ctlControl:=txtTobLetter))
			'FSA Phase IIIEnd
			
			'***************************
			' get the party tax details from the party tax control
			m_sTaxNumber = uctPartyTax1.TaxNumber
			m_bDomiciledForTax = uctPartyTax1.IsDomiciledForTax
			m_bTaxExempt = uctPartyTax1.TaxExempt
			m_dTaxPercentage = uctPartyTax1.TaxPercentage
			'***************************
			
            If m_bSystemOptionClientBlacklistingInForce Then
				If cboBlackListReason.SelectedIndex <> -1 Then
					If cboBlackListReason.SelectedIndex = 0 Then

						m_vBlackListReasonId = Nothing
					Else
						m_vBlackListReasonId = VB6.GetItemData(cboBlackListReason, cboBlackListReason.SelectedIndex)
					End If
				End If
			Else

				m_vBlackListReasonId = Nothing
			End If
			
            If cboTermsOfPayment.SelectedIndex <> -1 Then
                If cboTermsOfPayment.SelectedIndex = 0 Then
                    m_iPaymentTermId = 0
                Else
                    m_iPaymentTermId = VB6.GetItemData(cboTermsOfPayment, cboTermsOfPayment.SelectedIndex)
                End If
            End If

			If m_sUnderwritingOrAgency = "A" Then
				m_vIsFeeClient = CStr(IIf(chkFeeClient.CheckState = CheckState.Checked, 1, 0))
			Else

				m_vIsFeeClient = Nothing
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' JAS(CMG) 03/09/02 - Added - txtRecordStatus, lblRecordStatus
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Dim vValue As String = ""
		Dim bNZConfig As Boolean

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'DD 28/10/2003 - only show password when FSA is enabled.
			m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, 1, vValue)
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If gPMFunctions.NullToString(vValue) <> "1" Then
				lblTPPassword.Visible = False
				txtTPPassword.Visible = False
			End If
			
			'AR20061107 - Get New Zealand product option
			m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTNewZealandConfiguration, v_vBranch:=1, r_vUnderwriting:=vValue)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			bNZConfig = (gPMFunctions.ToSafeString(vValue) = "1")
			
			'Get Country Code for Postcode checking.(Process 004)

			m_lReturn = m_oBusiness.GetDefaultCountryCode(v_iCountryID:=m_iDefaultCountryID, r_sCountryCode:=m_sDefaultCountryCode)
			
			' Check for errors
			If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get default country code", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
				
				Return result
			End If
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Me.Height = 6600
			'Me.Width = 9360
			
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			tabMainTab.Visible = True
			'tabMainTab.Top = 480
			'tabProspecting.Visible = False
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			cmdEditAd.Enabled = False
			cmdDeleteAd.Enabled = False
			cmdEditCon.Enabled = False
			cmdDeleteCon.Enabled = False
			cmdEditConv.Enabled = False
			cmdDeleteConv.Enabled = False
			cmdEditPol.Enabled = False
			cmdDeletePol.Enabled = False
			cmdEditLoyaltyScheme.Enabled = False ' RAW 18/11/2002 : PS005 : Added
			cmdDeleteLoyaltyScheme.Enabled = False ' RAW 18/11/2002 : PS005 : Added
			
			cboCreditCard.Visible = False
			lblCreditCard.Visible = False
			
			chkAgent.CheckState = CheckState.Unchecked
			chkProspect.CheckState = CheckState.Unchecked
			
			m_lReturn = SetFirstLastControls()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' CF020699 - Added list view functions
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwAddresses.Handle.ToInt32(), v_vShowRowSelect:=True)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If
			
            lvwAddresses.FullRowSelect = True
            lvwAddresses.HideSelection = False
			
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwCampaigns.Handle.ToInt32(), v_vShowRowSelect:=True)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If
			
            lvwCampaigns.FullRowSelect = True
            lvwCampaigns.HideSelection = False
			
            '         m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwContacts.Handle.ToInt32(), v_vShowRowSelect:=True)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If
			
            lvwContacts.FullRowSelect = True
            lvwContacts.HideSelection = False

            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwConvictions.Handle.ToInt32(), v_vShowRowSelect:=True)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            lvwConvictions.FullRowSelect = True
            lvwConvictions.HideSelection = False

            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwPolicies.Handle.ToInt32(), v_vShowRowSelect:=True)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            lvwPolicies.FullRowSelect = True
            lvwPolicies.HideSelection = False

			'RAW 18/11/2002 : PS005 : Added - Begin
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwLoyaltySchemes.Handle.ToInt32(), v_vShowRowSelect:=True)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            '         End If

            lvwLoyaltySchemes.FullRowSelect = True
            lvwLoyaltySchemes.HideSelection = False

			'RAW 18/11/2002 : PS005 : End
			
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				'AR20061107 - Default Domiciled for Tax to ticked for NZ market
				uctPartyTax1.IsDomiciledForTax = bNZConfig
			End If
			
            m_lReturn = m_oBusiness.GetCorrespondenceTypes(m_vCorrespondenceTypes)

			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the correspondence types from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
			End If

			PopulateCorrespondenceTypes()
            If cboCorrespondenceType.SelectedIndex = -1 AndAlso m_iTask = PMEComponentAction.PMAdd Then
                cboCorrespondenceType.Text = "Letter"
            End If

			'MSS200901 - Added from UW for merge
			'    If (Task <> PMAdd) Then
			
			'Maintain Client Code
			m_lReturn = SetClientCodeCntl()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
			End If
			
			
			If Task = gPMConstants.PMEComponentAction.PMView Then
				txtIDReference.Enabled = False
			End If
			'MSS200901 - Merge end
			'sj 12/06/2002 - start
			If m_bIsNRMA Then
				txtLoyaltyNumberPrefix.Visible = True
				'Loyalty number length must be 10 characters
				txtLoyaltyNumber.MaxLength = 10
				'Right justify
				txtAlternativeIdentifier.TextAlign = HorizontalAlignment.Right
				'Branch field read only
				cboBranch.Enabled = False
				txtAlternativeIdentifier.Text = PartyFunc.m_kBlankAlternativeIdentifier
				
				'Replace "company reg" field with "trading name"
				'lblCompanyReg.Caption = "Trading Name"
				txtCompanyReg.Visible = False
				txtTradingName.Left = txtCompanyReg.Left
				txtTradingName.Top = txtCompanyReg.Top
				txtTradingName.Visible = True
				
				'Rename "Trading Name" to "Company Name"
				'lblName.Caption = "Company name"
				
				'Remove "Financial year"
				lblFinancialYear.Visible = False
				txtFinancialYear.Visible = False
				'sj 25/07/2002 - start
			Else
				txtLoyaltyNumberPrefix.Visible = False
				txtLoyaltyNumber.Left = cboReminderType.Left
				txtLoyaltyNumber.Width = cboReminderType.Width
				'sj 25/07/2002 - end
			End If
			'sj 12/06/2002 - end
			' {* USER DEFINED CODE (End) *}
			
			'sj 23/09/2002 - start
			If m_bMultiTreeAccounting Then
				'Branch field read only
				cboBranch.Enabled = False
			End If
			'sj 23/09/2002 - end
			
			'KB PN 5099 default new fields to invisible
			'dont break AON with RealFileCode etc
			lblRealFileCode.Visible = False
			lblRealArea.Visible = False
			txtRealFileCode.Visible = False
			cboRealArea.Visible = False
			fraRealArea.Visible = False
			
			'sj 02/07/2002 - start
			If m_bAONAffinity Then
				'eck19052005 - Not room for this -leave as per design
				'fraAgent.Height = 1995
				txtFileCode.Visible = True
				lblFileCode.Visible = True
				fraAreaCode.Visible = True
				txtRecordStatus.Visible = True
				lblRecordStatus.Visible = True
				
			Else
				fraAgent.Height = fraConsultant.Height
				txtFileCode.Visible = False
				lblFileCode.Visible = False
				fraAreaCode.Visible = False
				txtRecordStatus.Visible = False
				lblRecordStatus.Visible = False
				lblRealFileCode.Visible = True
				lblRealArea.Visible = True
				txtRealFileCode.Visible = True
				cboRealArea.Visible = True
				fraRealArea.Visible = True
			End If
			'sj 02/07/2002 - end
			
			' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development
			' check for list system option and make visible appropriate dropdown
			If Not m_bReadTradeABIList Then
				With cboTrade
					.Enabled = Not (m_iTask = gPMConstants.PMEComponentAction.PMView)
					.SetBounds(ddBusiness.Left, ddBusiness.Top + VB6.TwipsToPixelsY(360), ddTrade.Width, 0, BoundsSpecified.X Or BoundsSpecified.Y Or BoundsSpecified.Width)
					.Visible = True
                End With
                'developer guide no. TODO
                ddTrade.Visible = False
				m_oFormFields.Item("cboTrade-0").IsMandatory = True
			Else
                With ddTrade
                    'developer guide no. TODO
                    .SetBounds(ddBusiness.Left, ddBusiness.Top + VB6.TwipsToPixelsY(360), ddBusiness.Width, 0, BoundsSpecified.X Or BoundsSpecified.Y Or BoundsSpecified.Width)
                    .Visible = True
                End With
				cboTrade.Enabled = False
				cboTrade.Visible = False
				m_oFormFields.Item("cboTrade-0").IsMandatory = False
				lblTrade.Font = VB6.FontChangeBold(lblTrade.Font, False)
			End If
			
			'DJM 13/01/2004 : Copied from 1.8.5 issue 5877.
			If m_bAONPRClientScreenChanges Then
				'DC300304 PN11259 was lblArea
				lblRealArea.Text = "County:"
				lblEmployees.Text = "No. of Principals:"
				lblSeasonalGift.Text = "Cancellation Reason:"
				lblSeasonalGift.Left -= VB6.TwipsToPixelsX(75)
				lblSeasonalGift.Width += VB6.TwipsToPixelsX(75)
				lblSICCode.Text = "Profession:"
				cmdMembershipGroups.Visible = False
				txtEmployees.Left = cboEmployees.Left
				txtEmployees.Top = cboEmployees.Top
				cboEmployees.Visible = False
				txtEmployees.Visible = True
				txtTurnover.Left = cboTurnover.Left
				txtTurnover.Top = cboTurnover.Top
				txtTurnover.Visible = True
				cboTurnover.Visible = False
			Else
				cmdMembershipGroups.Visible = False
				txtOffices.Left = cboEmployees.Left
				cboEmployees.Visible = True
				txtEmployees.Visible = False
				cboTurnover.Visible = True
                txtTurnover.Visible = False
                lblEmployees.Visible = True
                lblTurnover.Visible = True
				
				' If adding a new corporate client then position Employees & Turnover combos
				' to nothing to force selection  PN22533
				If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
					cboEmployees.ListIndex = -1
					cboTurnover.ListIndex = -1
				End If
				
			End If
			
			'PN 17153 Hiding sub branch for Broking depending upon the product option
			lblSubBranch.Visible = m_bShowSubBranchID
			cboSubBranch.Visible = m_bShowSubBranchID
			
			fraBlackList.Visible = False
			
			If m_sUnderwritingOrAgency = "U" Then
				If m_bSystemOptionClientBlacklistingInForce Then
					fraBlackList.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraAgent.Top) + VB6.PixelsToTwipsY(fraAgent.Height) + 40)
					fraBlackList.Visible = True
				End If
			End If
			
			chkFeeClient.Visible = (m_sUnderwritingOrAgency = "A")
			chkFeeClient.CheckState = CheckState.Unchecked
			
			'Party Bank Details
			m_lReturn = uctPartyBankControl1.SetInterfaceDefaults()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetFirstLastControls
	'
	' Description: Sets the first and last data entry controls for
	'              each tab to the control array, for use with the
	'              keyboard navigation.
	'
	' ***************************************************************** '
	Private Function SetFirstLastControls() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to set the first and last data entry
			' controls for all of the tabs.
			'
			' Example:-
			'
			'    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
			'    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			m_ctlTabFirstLast(MainModule.ACControlStart, 0) = txtIDReference
			m_ctlTabFirstLast(MainModule.ACControlEnd, 0) = cboArea
			m_ctlTabFirstLast(MainModule.ACControlStart, 1) = lvwAddresses
			m_ctlTabFirstLast(MainModule.ACControlEnd, 1) = txtSalutation
			m_ctlTabFirstLast(MainModule.ACControlStart, 2) = lvwContacts
			m_ctlTabFirstLast(MainModule.ACControlEnd, 2) = chkeMPS
			m_ctlTabFirstLast(MainModule.ACControlStart, 3) = lvwConvictions
			m_ctlTabFirstLast(MainModule.ACControlEnd, 3) = txtCCJ
			m_ctlTabFirstLast(MainModule.ACControlStart, 4) = cboCurrency
			m_ctlTabFirstLast(MainModule.ACControlEnd, 4) = txtTobLetter
			m_ctlTabFirstLast(MainModule.ACControlStart, 5) = lvwLoyaltySchemes
			m_ctlTabFirstLast(MainModule.ACControlEnd, 5) = cmdEditLoyaltyScheme
			m_ctlTabFirstLast(MainModule.ACControlStart, 6) = txtAgentReference
			m_ctlTabFirstLast(MainModule.ACControlEnd, 6) = cmdEditPol
			
			
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLookupValues
	'
	' Description: Gets all of the lookup values, ready to be used by
	'              the lookup function.
	'Update History:
	'JT PN-13238 Instead of PMLookupAllEffective now we will pass PMLookupAlldeleted
	' ***************************************************************** '
	Private Function GetLookupValues() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Gets all of the lookup values.
			
			' Check the task.
			
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMAdd
					' Get all of the lookup values.





                    'developer guide no. 37
                    m_lReturn = m_oBusiness.GetLookupValues(gPMConstants.PMELookupType.PMLookupAll, m_vLookupValues, MainModule.g_iLanguageID, m_vLookupDetails)
					
				Case gPMConstants.PMEComponentAction.PMEdit
					'AR20050120 - PN18207 Use All Effective
					' Get all of the lookup values with the correct
					' effective date.





                    'developer guide no. 37
                    m_lReturn = m_oBusiness.GetLookupValues(gPMConstants.PMELookupType.PMLookupAllEffective, m_vLookupValues, MainModule.g_iLanguageID, m_vLookupDetails)
					
					'DC030401 changed lookup type to All Effective
				Case gPMConstants.PMEComponentAction.PMView
					'AR20050120 - PN18207 Use All Effective
					' Get lookup values for viewing only.





                    'developer guide no. 37
                    m_lReturn = m_oBusiness.GetLookupValues(gPMConstants.PMELookupType.PMLookupAllEffective, m_vLookupValues, MainModule.g_iLanguageID, m_vLookupDetails)
			End Select
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
				
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLookupDetails
	'
	' Description: Gets all of the lookup details using the lookup
	'              values, then assigns them to the control passed.
	'
	' ***************************************************************** '
    'developer guide no. 101
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox, Optional ByRef bSecondary As Boolean = False, Optional ByRef sInitialOption As String = "") As Integer

        Dim result As Integer = 0
        Dim lRow, lRow2 As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False
            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If m_vLookupValues(ACValueTableName, lRow).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            'DJM 26/04/2002 : Clear the control first. This prevents duplicates in
            '                 the comboboxes when the apply button in clicked.

            ctlLookup.Items.Clear()

            If sInitialOption <> "" Then

                Dim newIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(sInitialOption, 0))

                ' NB:m_vLookupValues(ACValueID, lRow2&)) is the value from the party record
                ' if the value from the party matches the item
                If StringsHelper.ToDoubleSafe(m_vLookupValues(ACValueID, lRow)) = 0 Then
                    ' then select it.
                    ctlLookup.SelectedIndex = newIndex
                End If
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            ctlLookup.Items.Add("")

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow) + CInt(m_vLookupValues(ACValueNumber, lRow)))) - 1)

                ' Add the details to the control.
                Dim newIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))

                ' Check if this is the selected index.
                If bSecondary Then
                    lRow2 = lRow + 1
                Else
                    lRow2 = lRow
                End If

                If CStr(m_vLookupValues(ACValueID, lRow2)) <> "" Then

                    If gPMFunctions.ToSafeInteger(m_vLookupValues(ACValueID, lRow2)) = gPMFunctions.ToSafeInteger((m_vLookupDetails(ACDetailKey, lCntr))) Then
                        ctlLookup.SelectedIndex = newIndex
                    End If

                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow2)) = "" Then
                ctlLookup.SelectedIndex = -1
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try

    End Function
	'eck010900
	' ***************************************************************** '
	' Name: GetBranchDetails
	'
	' Description: Gets all of the branch details
	'
	' ***************************************************************** '
	Private Function GetBranchDetails() As Integer
		Dim result As Integer = 0
        Dim vSourceArray(,) As Object
        Dim i As Short
        Dim lReturn As gPMConstants.PMEReturnCode
		result = gPMConstants.PMEReturnCode.PMTrue
		'If this is a multi-company system.
		If m_bMultiTreeAccounting Then
			'Always show correct branch even if the user is not authorise for that branch.
			m_lReturn = m_oPMUser.GetAllSources(r_vSourceArray:=vSourceArray)
			
		Else
			'Only populate combo with addresses the user is authorised to access.

			m_lReturn = m_oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_iUserId, v_bIncludeDeletedSources:=m_bIncludeClosedBranchChecked)
		End If
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get branch details for the dropdown list")
		End If

		m_vSourceArray = vSourceArray
		'Clear combo.
		cboBranch.Items.Clear()
		'Populate branch combo.
        'developer guide no. 162
        For i = 0 To UBound(vSourceArray, 2)
            'Add using branch description (3).

            'cboBranch.Items.Add((Trim(vSourceArray(3, i))))
            'developer guide no. 162
            Dim listIndex As Integer = cboBranch.Items.Add(New VB6.ListBoxItem(Trim(vSourceArray(2, i)), CInt(vSourceArray(0, i))))


            'developer guide no. 162
            If CShort(vSourceArray(0, i)) = m_iPartySourceId Then

                cboBranch.SelectedIndex = listIndex

                'developer guide no. 162
                m_sBranchPrefix = Trim(vSourceArray(1, i))
            End If
        Next i
		
		'If this is a multi-company system.
		If m_bMultiTreeAccounting Then
			'Re-populate source array with branches that the user is authorised this.

			m_lReturn = m_oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_iUserId)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get users branch details")
			End If
		End If
        'developer guide no. 131(Guide)
        If cboBranch.SelectedIndex >= 0 Then
            cboCurrency.CompanyId = CInt(VB6.GetItemData(cboBranch, cboBranch.SelectedIndex))
        End If
        cboCurrency.RefreshList()
		
        'PN16993
        'cboCurrency.CurrencyID = m_iCurrencyId
        ' if this is add mode then default the currency id
        If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
            ' get the default currency id
            ' based on the selected parties branch id
            ' NB: This function also sets the currency id
            lReturn = GetSourceBaseCurrency()

            ' if the getsourcebasecurrency failed use
            ' the currency id defined on the object manager
            ' as a fall back
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                cboCurrency.CurrencyId = g_iCurrencyID
            End If
		Else
			' otherwise use the currency id that was previously saved
			cboCurrency.RefreshList()
			cboCurrency.CurrencyId = m_iCurrencyId
		End If
		
		m_bUserMode = True
		
		Return result
		
		
		
		' Error Section.
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the branch details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	Private Function GetOldAndNewAddress() As Integer
		
		Dim result As Integer = 0
		Dim lAddressCount As Integer
		Dim sAddressUsageDesc As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lAddressCount = 0
			
			For lLoopAddresses As Integer = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)
				
				sAddressUsageDesc = ""
				
				For lLoopAddresseTypes As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
					If m_vAddresses(1, lLoopAddresses).Equals(m_vAddressTypes(0, lLoopAddresseTypes)) Then
						sAddressUsageDesc = CStr(m_vAddressTypes(1, lLoopAddresseTypes)).Trim()
						Exit For
					End If
				Next 
				
				If sAddressUsageDesc <> "" Then
					If lAddressCount = 0 Then
						ReDim m_vOldAddress(6, lAddressCount)
					Else
						ReDim Preserve m_vOldAddress(6, lAddressCount)
					End If
					
					m_vOldAddress(0, lAddressCount) = CStr(m_vAddresses(6, lLoopAddresses))
					m_vOldAddress(1, lAddressCount) = sAddressUsageDesc
					m_vOldAddress(2, lAddressCount) = CStr(m_vAddresses(2, lLoopAddresses)).Trim()
					m_vOldAddress(3, lAddressCount) = CStr(m_vAddresses(3, lLoopAddresses)).Trim()
					m_vOldAddress(4, lAddressCount) = CStr(m_vAddresses(4, lLoopAddresses)).Trim()
					m_vOldAddress(5, lAddressCount) = CStr(m_vAddresses(5, lLoopAddresses)).Trim()
					m_vOldAddress(6, lAddressCount) = CStr(m_vAddresses(0, lLoopAddresses)).Trim()
					
					lAddressCount += 1
				End If
				
			Next 
			
			lAddressCount = 0
			
            For Each oListItem As ListViewItem In lvwAddresses.Items
				
				If lAddressCount = 0 Then
					ReDim m_vNewAddress(6, lAddressCount)
				Else
					ReDim Preserve m_vNewAddress(6, lAddressCount)
				End If
				
				'Check default country to see where Postcode is being displayed.
				Select Case m_sDefaultCountryCode.Trim()
					Case "GBR"
						sAddressUsageDesc = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
					Case Else
						sAddressUsageDesc = oListItem.Text.Trim()
				End Select
				
				Select Case m_sDefaultCountryCode.Trim()
					Case "GBR"
						m_vNewAddress(0, lAddressCount) = Convert.ToString(oListItem.Tag)
						m_vNewAddress(1, lAddressCount) = sAddressUsageDesc
						m_vNewAddress(2, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text.Trim()
						m_vNewAddress(3, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 3).Text.Trim()
						m_vNewAddress(4, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 4).Text.Trim()
						m_vNewAddress(5, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 5).Text.Trim()
						m_vNewAddress(6, lAddressCount) = oListItem.Text.Trim()
					Case Else
						m_vNewAddress(0, lAddressCount) = Convert.ToString(oListItem.Tag)
						m_vNewAddress(1, lAddressCount) = sAddressUsageDesc
						m_vNewAddress(2, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
						m_vNewAddress(3, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text.Trim()
						m_vNewAddress(4, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 3).Text.Trim()
						m_vNewAddress(5, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 4).Text.Trim()
						m_vNewAddress(6, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 5).Text.Trim()
				End Select
				
				lAddressCount += 1
				
			Next oListItem
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOldAndNewAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOldAndNewAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cboArea_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboArea.Enter
		If SSTabHelper.GetTabVisible(tabMainTab, 0) Then
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
		End If
	End Sub
	
	Private Sub cboBranch_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.SelectionChangeCommitted
		'sj 11/06/2002 - start
		If m_bUserMode Then
			m_lReturn = PartyFunc.GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=cboBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_lSubBranchId)
			
			If m_bShowSubBranchID Then 'PN 17153
				cboSubBranch.SelectedIndex = -1
			Else
				cboSubBranch.SelectedIndex = 0
			End If
		End If
		'sj 11/06/2002 - end

		' RDC 20042004
		m_lReturn = GetSourceBaseCurrency()
		m_iPartySourceId = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)

	End Sub
	
	' PRIVATE Methods (End)
	
	Private Sub cboCreditCard_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.SelectionChangeCommitted
		
		m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)
		
	End Sub
	
	Private Sub cboCreditCard_DropDown(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.DropDown
		
		m_lReturn = PMBGeneralFunc.FillCombo(cboControl:=cboCreditCard, bRefill:=True)
		
	End Sub
	
	Private Sub cboCreditCard_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.Enter
		m_lReturn = PMBGeneralFunc.ControlGotFocus(cboCreditCard)
		
	End Sub
	
	Private Sub cboCreditCard_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboCreditCard.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)
		
	End Sub
	
	Private Sub cboCreditCard_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.Leave
		'    m_lReturn& = ControlLostFocus(cboCreditCard)
		
	End Sub
	
    Private Sub cboCurrency_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.Enter
		
		' CTAF 220900
		' Make sure we're on the right tab incase this was called
		' from form controls.
		SSTabHelper.SetSelectedIndex(tabMainTab, 4)
		
	End Sub
	
	'PN23222
	Private Sub pnlAgentName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlAgentName.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		ToolTip1.SetToolTip(pnlAgentName, m_sAgentName)
	End Sub
	
	'PN23222
	Private Sub pnlBrokerName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlBrokerName.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		ToolTip1.SetToolTip(pnlBrokerName, m_sBrokerName)
	End Sub
	
	'PN23222
	Private Sub pnlConsultantName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlConsultantName.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		ToolTip1.SetToolTip(pnlConsultantName, m_sConsultantName)
	End Sub
	
	'PN23222
	Private Sub pnlCurrentAgent_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlCurrentAgent.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		ToolTip1.SetToolTip(pnlCurrentAgent, m_sCurrentAgentName)
	End Sub
	
	'PN23222
	Private Sub pnlInsurerName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlInsurerName.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		ToolTip1.SetToolTip(pnlInsurerName, m_sInsurerName)
	End Sub
	
	Private Sub txtEmployees_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEmployees.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEmployees)
	End Sub
	
	
	Private Sub txtEmployees_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEmployees.Leave
		
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEmployees)
		End If

	End Sub
	
	
	Private Sub cboEmployees_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboEmployees.GotFocus
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboEmployees)
	End Sub
	
	
	Private Sub cboEmployees_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboEmployees.LostFocus
		
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboEmployees)
		End If
		
	End Sub
	
	
	Private Sub cboStrengthCode_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboStrengthCode.SelectionChangeCommitted
		
		m_bChangedProspect = True
		
	End Sub
	
	Private Sub cboTrade_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTrade.GotFocus
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboTrade)
		
		If cboTrade.ListIndex = -1 Then
			m_lReturn = HighlightContol(cboTrade, optBoolDropDown:=True)
		End If
		
	End Sub
	
	
	Private Sub cboTrade_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTrade.LostFocus
		
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboTrade)
		End If
		
	End Sub
	Private Sub txtTurnover_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTurnover.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTurnover)
	End Sub
	
	
	Private Sub txtTurnover_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTurnover.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTurnover)
		End If
	End Sub
	
	
	Private Sub cboTurnover_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTurnover.GotFocus
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboTurnover)
	End Sub
	
	
	Private Sub cboTurnover_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTurnover.LostFocus
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboTurnover)
		End If
	End Sub
	
	Private Sub chkeMPS_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkeMPS.Enter
		If SSTabHelper.GetTabVisible(tabMainTab, 2) Then
			SSTabHelper.SetSelectedIndex(tabMainTab, 2)
		End If
	End Sub
	
	'DJM 13/01/2004 : Copied from 1.8.5 issue 5877.
	Private Sub cmdMembershipGroups_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMembershipGroups.Click
		'Allow Maintenance Of Membership groups
		
		Try 
			
			If m_lPartyCnt < 1 Then
				If MessageBox.Show("Must Save Client Details before adding Group Information. Do you wish to save Client Details now ?", "Client Details", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
					
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
					
					m_lReturn = OKClick()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					'If update was successful then we are now effectively in Edit mode.
					m_iTask = gPMConstants.PMEComponentAction.PMEdit

					m_lReturn = m_oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
					
					m_sOldIdReference = m_sShortName
					
					'Re get everything to ensure we are fully initialised in edit mode.
					m_lReturn = GetBusiness()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					m_lReturn = BusinessToInterface()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					m_lReturn = LockParty()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					
				Else
					Exit Sub
				End If
			End If
			
			If m_oMembershipGroups Is Nothing Then
				' Get an instance of the interface object via
				' the public object manager.
				Dim temp_m_oMembershipGroups As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oMembershipGroups, "IPMBMemGroups.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oMembershipGroups = temp_m_oMembershipGroups
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Membership Group object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMembershipGroups_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			

			m_oMembershipGroups.PartyCnt = m_lPartyCnt

			m_lReturn = m_oMembershipGroups.SetProcessModes(vTask:=m_iTask)
			

			m_lReturn = m_oMembershipGroups.Start()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdMembershipGroups_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAssociates_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	'DC071200 changed from combo to dropdown list
    Private Sub ddTermsOfPayment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
		m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)
	End Sub
	
	'DC071200 changed from combo to dropdown list
    Private Sub cboTermsOfPayment_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboTermsOfPayment)
	End Sub
	'DC071200 changed from combo to dropdown list
    'developer guide no. 191
    Private Sub ddTermsOfPayment_KeyDown(ByVal eventSender As Object, ByVal eventArgs As PMListMgrDropdown.uctDropdown.KeyDownEventArgs)
        Dim KeyCode As Integer = eventArgs.KeyCode
        'developer guide no. 191
        Dim Shift As Integer = eventArgs.Shift \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)
	End Sub
	'DC071200 changed from combo to dropdown list
    Private Sub cboTermsOfPayment_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboTermsOfPayment)
	End Sub
	
	Private Sub cmdAddAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAd.Click
		Dim sTmp As String = ""
		Dim oListItem As ListViewItem
		
		Try 
			'Create icontact if not already done so
			If m_oAddress Is Nothing Then
				
				' Get an instance of the address interface object via
				' the public object manager.
				Dim temp_m_oAddress As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oAddress, "iPMBAddress.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oAddress = temp_m_oAddress
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			
			'sj 23/07/2002 - start

			m_oAddress.CorrespondanceAddressExists = CorrespondanceAddressExists()
			'sj 23/07/2002 - end
			
			'sj 12/06/2002 - start

			m_oAddress.IsNRMA = m_bIsNRMA
			'sj 12/06/2002 - end
			
			'set the main postcode and reference

			m_oAddress.Reference = txtIDReference.Text
			'    m_lReturn& = ControlLostFocus(cboTermsOfPayment)
			

			m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			

			m_oAddress.Reference = txtIDReference.Text

            'developer guide no. 218
            m_oAddress.PostCode = m_sMainPostCode

            m_oAddress.CountryID = gPMFunctions.ToSafeLong(CStr(m_vSourceArray(3, cboBranch.SelectedIndex)))
			

            'developer guide no. 37
            m_lReturn = m_oAddress.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, add to grid

			If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
			Select Case (m_sDefaultCountryCode.Trim())
				Case "GBR"
					' Add the data to the list view


                    oListItem = lvwAddresses.Items.Add(m_oAddress.PostalCode, "AddressImage")
					' Address Usage

					ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.AddressUsageType
					' Address line 1

					ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address1
					' Address line 2

					ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address2
					' Address line 3

					ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address3
					' Address line 4

					ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.Address4
				Case Else
					' Add the data to the list view


                    oListItem = lvwAddresses.Items.Add(m_oAddress.AddressUsageType, "AddressImage")
					' Address line 1

					ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.Address1
					' Address line 2

					ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address2
					' Address line 3

					ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address3
					' Address line 4

					ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address4
					' Postcode

					ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.PostalCode
			End Select
			
			' Store the Address_cnt

            'developer guide no. 218
            oListItem.Tag = m_oAddress.AddressCnt
			

			If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

				m_sMainPostCode = m_oAddress.PostalCode

				m_iMainAddressIndex = m_oAddress.AddressUsageTypeID
				'Caption = "Corporate Client: " & m_sShortName & " " & m_sMainPostCode
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdAddCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddCon.Click
		Dim oListItem As ListViewItem
		
		Try 
			
			'Create icontact if not already done so
			If m_oContact Is Nothing Then
				
				' Get an instance of the contact interface object via
				' the public object manager.
				Dim temp_m_oContact As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oContact, "iPMBContact.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oContact = temp_m_oContact
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contacts", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					'RWH(09/06/2000) - Tidy up object.
					If Not (m_oContact Is Nothing) Then
						m_oContact = Nothing
					End If
					Exit Sub
					
				End If
				
			End If
			
			'set the main postcode and reference
			

            'developer guide no. 37
            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'RWH(09/06/2000) - Tidy up object.
				If Not (m_oContact Is Nothing) Then
					m_oContact = Nothing
				End If
				Exit Sub
			End If
			

			m_lReturn = m_oContact.ContactCnt
			

			m_oContact.Reference = txtIDReference.Text

			m_oContact.PostCode = m_sMainPostCode
			

			m_lReturn = m_oContact.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'RWH(09/06/2000) - Tidy up object.
				If Not (m_oContact Is Nothing) Then
					m_oContact = Nothing
				End If
				Exit Sub
			End If
			
			'If not cancelled, add to grid

			If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
				'RWH(09/06/2000) - Tidy up object.
				If Not (m_oContact Is Nothing) Then
					m_oContact = Nothing
				End If
				Exit Sub
			End If
			
			'Me.Refresh
			


            oListItem = lvwContacts.Items.Add(m_oContact.AreaCode, "ContactImage")
			
			' Assign details to other the columns
			' Column 2
			'Temporary thing

			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number
			
			' Column 3

			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension
			
			' Column 4

			ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType
			
			' Column 5

			ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description
			
			' Store the Address_cnt

			oListItem.Tag = m_oContact.contactcnt
			
			'RWH(09/06/2000) - Tidy up object.
			If Not (m_oContact Is Nothing) Then
				m_oContact = Nothing
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			'RWH(09/06/2000) - Tidy up object.
			If Not (m_oContact Is Nothing) Then
				m_oContact = Nothing
			End If
			Exit Sub
		End Try
	End Sub
	
	Private Sub cmdAddConv_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddConv.Click
		Dim oListItem As ListViewItem
		
		Try 
			
			'MSS200901 - Inserted UW code. Was more in depth
			If m_lPartyCnt < 1 Then
				'RWH(17/05/2001) Let's allow them to add convictions without having to exit
				'and come back in.
				If MessageBox.Show("Must Save Client Details before adding Convictions. Do you wish to save Client Details now ?", "Personal Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
					
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
					
					m_lReturn = OKClick()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
						Exit Sub
					End If
					'RWH(17/05/2001) If update was successful then we are now effectively in Edit mode.
					m_iTask = gPMConstants.PMEComponentAction.PMEdit

					m_lReturn = m_oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
					
					m_sOldIdReference = m_sShortName
					
					'RWH(18/05/2001) Re get everything to ensure we are fully initialised in edit mode.
					m_lReturn = GetBusiness()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					m_lReturn = BusinessToInterface()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					m_lReturn = LockParty()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					
				Else
					Exit Sub
				End If
			End If
			'MSS200901 - Commented above code (old SFORB). Merge End
			
			
			'Create icontact if not already done so
			If m_oConviction Is Nothing Then
				
				' Get an instance of the contact interface object via
				' the public object manager.
				Dim temp_m_oConviction As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oConviction, "iPMBPartyConviction.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oConviction = temp_m_oConviction
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get convictions", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			

			m_lReturn = m_oConviction.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			

			m_oConviction.PartyCnt = m_lPartyCnt
			

			m_lReturn = m_oConviction.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, add to grid

			If m_oConviction.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			'Me.Refresh
			


            oListItem = lvwConvictions.Items.Add(m_oConviction.Code.Trim(), "ConvictionImage")
			
			' Assign details to other the columns
			' Column 2
			'Temporary thing

			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oConviction.ConvictionDate
			
			' Column 3

			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oConviction.Description
			
			' Column 4

			ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oConviction.FineAmount
			
			' Column 5

			ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oConviction.StatusCode
			
			' Store the Address_cnt

			oListItem.Tag = m_oConviction.PartyConvictionID
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddConv_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: Refresh
	'
    ' Description: What is this supposed TODO?
	'
	' ***************************************************************** '
	Public Overrides Sub Refresh()
		
	End Sub
	
    Private Sub cmdAddPol_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddPol.Click

	
        Dim iRow As Integer
        Dim oListItem As ListViewItem

        'According to user guide this functionality is not supported more in Pure BO  (Confirmation by Mousumi Borah)
        Exit Sub

        If m_lPartyCnt < 1 Then
            MessageBox.Show("Must Save Client Details before adding Prospect Policies", Application.ProductName)
            Exit Sub
        End If

        Try

            'Create iProspectPolicy if not already done so
            If m_oProspectPolicy Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oProspectPolicy As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oProspectPolicy, sClassName:="iPMBProspectPolicy.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oProspectPolicy = temp_m_oProspectPolicy

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get prospect policies", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddPol_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If



            m_lReturn = m_oProspectPolicy.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oProspectPolicy.PolicyTypeID = -1

            m_oProspectPolicy.PolicyTypeDescription = ""

            m_oProspectPolicy.RenewalDate = DateTime.Today

            m_oProspectPolicy.NoOfTimesQuoted = 0

            m_oProspectPolicy.TargetPremium = 0


            m_lReturn = m_oProspectPolicy.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oProspectPolicy.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            m_bChangedProspectPolicies = True

            Me.Refresh()

            ' Column 1


            oListItem = lvwPolicies.Items.Add(m_oProspectPolicy.PolicyTypeDescription.Trim(), "PolicyImage")


            oListItem.Tag = m_oProspectPolicy.PolicyTypeID

            ' Assign details to other the columns
            ' Column 2

            m_lReturn = m_oFormFields.FormatControl(txtHiddenDate, m_oProspectPolicy.RenewalDate)
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oProspectPolicy.NoOfTimesQuoted
            ' Column 4


            m_lReturn = m_oFormFields.FormatControl(txtHiddenCurrency, m_oProspectPolicy.TargetPremium)
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text

            'Column 5

            'developer guide no. 197(Latest Guide)
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = DateTime.FromOADate(m_oProspectPolicy.RenewalDate.ToOADate).ToString("yyyyMMdd")

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddPol_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddPol_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub

	Private Sub cmdAddLoyaltyScheme_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddLoyaltyScheme.Click 'RAW 18/11/2002 : PS005 : Added
		Dim oListItem As ListViewItem
		
		Try 
			
			If m_lPartyCnt < 1 Then
				' This is a new party that has not yet been added to the database so no party_cnt has been generated
				
				'Let's allow them to add PartyLoyaltySchemes without having to exit
				'and come back in.
				If MessageBox.Show("Must Save Client Details before adding Loyalty Schemes. Do you wish to save Client Details now ?", "Personal Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
					
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
					
					m_lReturn = OKClick()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					'If update was successful then we are now effectively in Edit mode.
					m_iTask = gPMConstants.PMEComponentAction.PMEdit

					m_lReturn = m_oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
					
					m_sOldIdReference = m_sShortName
					
					'Re get everything to ensure we are fully initialised in edit mode.
					m_lReturn = GetBusiness()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					m_lReturn = BusinessToInterface()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					m_lReturn = LockParty()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Exit Sub
					End If
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					
				Else
					Exit Sub
				End If
			End If
			
			'Create iPartyLoyaltyScheme if not already done so
			If m_oPartyLoyaltyScheme Is Nothing Then
				
				' Get an instance of the contact interface object via
				' the public object manager.
				Dim temp_m_oPartyLoyaltyScheme As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oPartyLoyaltyScheme, sClassName:="iPMBPartyLoyaltyScheme.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
				m_oPartyLoyaltyScheme = temp_m_oPartyLoyaltyScheme
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get PartyLoyaltySchemes", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			

			m_lReturn = m_oPartyLoyaltyScheme.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			

			m_oPartyLoyaltyScheme.PartyCnt = m_lPartyCnt
			

			m_lReturn = m_oPartyLoyaltyScheme.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, add to grid

			If m_oPartyLoyaltyScheme.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			Me.Refresh()
			

			oListItem = lvwLoyaltySchemes.Items.Add(m_oPartyLoyaltyScheme.LoyaltySchemeName)
			
			' Assign details to other the columns
			' Column 2

			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oPartyLoyaltyScheme.MemberNumber
			
			' Column 3

			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=m_oPartyLoyaltyScheme.StartDate)
			
			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtHiddenDate.Text.Trim()
			
			' Column 4

			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=m_oPartyLoyaltyScheme.EndDate)
			
			ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenDate.Text.Trim()
			
			' Store the PartyLoyaltySchemeId

			oListItem.Tag = m_oPartyLoyaltyScheme.PartyLoyaltySchemeId
            'This line is basically not required
            'lvwLoyaltySchemes.FocusedItem.Selected = False
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddLoyaltyScheme_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdAddLoyaltyScheme_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddLoyaltyScheme.Enter 'RAW 18/11/2002 : PS005 : Added
		
		'If we're here by tabbing (or back-tabbing) we're on tab 5
		SSTabHelper.SetSelectedIndex(tabMainTab, 5)
		
	End Sub
	
    Private Sub cmdAgentLookUp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgentLookUp.Click

        Dim vCnt, vShortName, vName As Object
        Dim sTemp As String = ""
        Dim dDefaultDate As Date
        Dim vDateCancelled As Object

        Try


            'developer guide no. 98(latest guide)
            m_lReturn = SelectParty(vCnt, vShortName, vName, "AG", , vDateCancelled)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            dDefaultDate = CDate("01/01/1900")


            If (CDate(vDateCancelled) <= DateTime.Today) And (CDate(vDateCancelled) >= dDefaultDate) Then
                MessageBox.Show("Agency cancelled - no new transactions can be placed through this agent", "Agency Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            'save the count in the tag and update controls

            txtAgentRef.Tag = CStr(vCnt)


            m_sAgentRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(txtAgentRef, m_sAgentRef)


            m_sAgentName = CStr(vName)

            sTemp = m_sAgentName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(sTemp, "&")

            'developer guide no. 26
            lblPnlAgentName.Text = sTemp

            'because we know Agent cnt matches the Agent ref, can bypass
            'the validation at the end
            m_bVerifyAgentCnt = False

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, gPMConstants.PMErrorText, MainModule.ACApp, ACClass, "cmdAgentLookUp_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
	
	Private Sub cmdAssociates_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAssociates.Click
		
		'DC 03/05/00
		'Allow Maintenance Of Associates
		Dim vTask As gPMConstants.PMEComponentAction
		
		Try 
			'Create shares object if not already done so
			If m_oAssociates Is Nothing Then
				
				' Get an instance of the associates interface object via
				' the public object manager.
				Dim temp_m_oAssociates As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oAssociates, sClassName:="iPMBAssociates.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
				m_oAssociates = temp_m_oAssociates
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get associates object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMaintainAssociates_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			
			'DC030401 make sure correct mode
			'vTask = PMEdit
			vTask = m_iTask
			

			m_lReturn = m_oAssociates.SetProcessModes(vTask:=vTask)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			

			m_oAssociates.PartyCnt = m_lPartyCnt

			m_oAssociates.SearchData = m_vAssociates


			m_oAssociates.Relationships = m_vRelationships
			

			m_lReturn = m_oAssociates.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, add to grid

			If m_oAssociates.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			Me.Refresh()
			
			m_vAssociates = ""

			m_vAssociates = m_oAssociates.SearchData
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAssociates_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAssociates_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
    Private Sub cmdBrokerLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrokerLookup.Click

        Dim vCnt, vShortName, vName As Object
        Dim sTemp As String = ""

        Try


            'developer guide no. 98(latest guide)
            m_lReturn = SelectParty(vCnt, vShortName, vName, "BR")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            txtBrokerRef.Tag = CStr(vCnt)


            m_sBrokerRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(txtBrokerRef, m_sBrokerRef)


            m_sBrokerName = CStr(vName)

            sTemp = m_sBrokerName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(sTemp, "&")

            'developer guide no. 26
            lblPnlBrokerName.Text = sTemp
            m_bChangedProspect = True

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, gPMConstants.PMErrorText, MainModule.ACApp, ACClass, "Err_cmdBrokerLookup_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
	
    Private Sub cmdConsultantLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConsultantLookup.Click

        Dim vCnt, vShortName, vName As Object
        Dim sTemp, vResolvedName As String 'CT 19/07/00

        Try




            'developer guide no. 98(latest guide)
            m_lReturn = SelectParty(vCnt, vShortName, vName, "CO", vResolvedName) 'CT 19/07/00 added extra resolvedName parameter

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            txtConsultantRef.Tag = CStr(vCnt)


            m_sConsultantRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(txtConsultantRef, m_sConsultantRef)

            'm_sConsultantName$ = CStr(vName)
            m_sConsultantName = vResolvedName 'CT 19/07/00

            sTemp = m_sConsultantName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(sTemp, "&")

            'developer guide no. 26
            lblPnlConsultantName.Text = sTemp

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, gPMConstants.PMErrorText, MainModule.ACApp, ACClass, "cmdConsultantLookUp_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
	
    Private Sub cmdCurrentAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCurrentAgent.Click


        Dim vCnt, vShortName, vName As Object
        Dim sTemp As String = ""

        Try




            'developer guide no. 98(latset guide)
            m_lReturn = SelectParty(vCnt, vShortName, vName, "AG")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_lCurrentAgent = CInt(CStr(vCnt))


            m_sCurrentAgentRef = CStr(vShortName)


            m_sCurrentAgentName = CStr(vName)

            sTemp = m_sCurrentAgentName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(sTemp, "&")

            'developer guide no. 26
            lblPnlCurrentAgent.Text = sTemp

            'because we know Agent cnt matches the Agent ref, can bypass
            'the validation at the end
            m_bVerifyCurrentAgentCnt = False

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, gPMConstants.PMErrorText, MainModule.ACApp, ACClass, "cmdCurrentAgent_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
	
	Private Sub cmdDeleteAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAd.Click
		Try 
			'Set row to be deleted - if a valid one selected
			If lvwAddresses.Items.Count < 1 Then
				Exit Sub
			End If
			
			'sj 23/07/2002 - start
			'Do not allow user to delete correspondance address
			If IsCorrespondanceAddress() Then
				MessageBox.Show("You must have an address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Exit Sub
			End If
			
			'Reset Interface
			cmdEditAd.Enabled = False
			cmdDeleteAd.Enabled = False
			
			lvwAddresses.Items.RemoveAt(m_iLine - 1)
			
			' Decrement the number of addresses
			m_lAddressCount -= 1
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			
		End Try
		
	End Sub
	
	Private Sub cmdDeleteCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteCon.Click
		Try 
			
			'Set row to be deleted - if a valid one selected
			If lvwContacts.Items.Count < 1 Then
				Exit Sub
			End If
			
			'Reset Interface
			cmdEditCon.Enabled = False
			cmdDeleteCon.Enabled = False
			
			lvwContacts.Items.RemoveAt(m_iLine - 1)
		
		Catch excep As System.Exception
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			
		End Try
		
	End Sub
	
	Private Sub cmdDeleteConv_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteConv.Click
		
		Dim iRow As Integer
		
		Try 
			
			'Set row to be deleted - if a valid one selected
			If lvwConvictions.Items.Count < 1 Then
				Exit Sub
			End If
			

			iRow = Convert.ToString(lvwConvictions.FocusedItem.Tag)
			
			'Create conviction component if not already done so
			If m_oConviction Is Nothing Then
				
				' Get an instance of the contactinterface object via
				' the public object manager.
				Dim temp_m_oConviction As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oConviction, sClassName:="iPMBPartyConviction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
				m_oConviction = temp_m_oConviction
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get conviction component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			'EK 12/12/99 Must set task to delete or conviction cannot be removed

			m_lReturn = m_oConviction.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMDelete)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'set the various ids

			m_oConviction.PartyCnt = m_lPartyCnt

			m_oConviction.PartyConvictionID = iRow
			

			m_lReturn = m_oConviction.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, edit grid

			If m_oConviction.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			'Reset Interface
			cmdEditConv.Enabled = False
			cmdDeleteConv.Enabled = False
			
			lvwConvictions.Items.RemoveAt(lvwConvictions.FocusedItem.Index)
			
			lvwConvictions.Focus()
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteConv_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdDeletePol_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeletePol.Click
		Dim oListItem As ListViewItem
        Dim iRow As Integer
        'According to user guide this functionality is not supported more in Pure BO  (Confirmation by Mousumi Borah)
        Exit Sub
		
		Try 
			
			'Set row to be deleted - if a valid one selected
			If lvwPolicies.Items.Count < 1 Then
				Exit Sub
			End If
			
			'Create prospect policy component if not already done so
			If m_oProspectPolicy Is Nothing Then
				
				' Get an instance of the prospect policy interface object via
				' the public object manager.
				Dim temp_m_oProspectPolicy As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oProspectPolicy, "iPMBProspectPolicy.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oProspectPolicy = temp_m_oProspectPolicy
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get prospect policy component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeletePol_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			
			'User PMView instead
			'm_lReturn& = m_oProspectPolicy.SetProcessModes(vTask:=PMDelete)

			m_lReturn = m_oProspectPolicy.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'set the Ids
			oListItem = lvwPolicies.FocusedItem
			


			m_oProspectPolicy.PolicyTypeID = Convert.ToString(oListItem.Tag)
			

			m_oProspectPolicy.PolicyTypeDescription = oListItem.Text
			
			txtHiddenDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
			


			m_oProspectPolicy.RenewalDate = m_oFormFields.UnformatControl(txtHiddenDate)
			

			m_oProspectPolicy.NoOfTimesQuoted = ListViewHelper.GetListViewSubItem(oListItem, 2).Text
			
			txtHiddenCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, 3).Text
			


			m_oProspectPolicy.TargetPremium = m_oFormFields.UnformatControl(txtHiddenCurrency)
			

			m_lReturn = m_oProspectPolicy.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, edit grid

			If m_oProspectPolicy.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			m_bChangedProspectPolicies = True
			
			'Reset Interface
			cmdEditPol.Enabled = False
			cmdDeletePol.Enabled = False
			
			lvwPolicies.Items.RemoveAt(lvwPolicies.FocusedItem.Index)
			
			lvwPolicies.Focus()
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeletePol_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeletePol_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdDeleteLoyaltyScheme_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteLoyaltyScheme.Click 'RAW 18/11/2002 : PS005 : Added
		Dim iMsgResult As DialogResult
		Dim sMessage, sTitle As String
		
		Try 
			
			'Set row to be deleted - if a valid one selected
			If lvwLoyaltySchemes.FocusedItem.Index + 1 < 1 Then
				Exit Sub
			End If
			
			'Ensure that delete should proceed

            'developer guide no. 243
            sTitle = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACConfirmDeleteTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            'developer guide no. 243
            sMessage = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACConfirmDeleteDetails, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
			
			If iMsgResult = System.Windows.Forms.DialogResult.No Then
				Exit Sub
			End If
			
			' OK so we really do want to delete
			
			'Create PartyLoyaltyScheme component if not already done so
			If m_oPartyLoyaltyScheme Is Nothing Then
				
				' Get an instance of the contactinterface object via
				' the public object manager.
				Dim temp_m_oPartyLoyaltyScheme As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oPartyLoyaltyScheme, "iPMBPartyLoyaltyScheme.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oPartyLoyaltyScheme = temp_m_oPartyLoyaltyScheme
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PartyLoyaltyScheme component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			

			m_lReturn = m_oPartyLoyaltyScheme.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMDelete)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'set the various ids

			m_oPartyLoyaltyScheme.PartyCnt = m_lPartyCnt
			


            m_oPartyLoyaltyScheme.PartyLoyaltySchemeID = Convert.ToString(lvwLoyaltySchemes.FocusedItem.Tag)
			

			m_lReturn = m_oPartyLoyaltyScheme.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'Reset Interface
			cmdEditLoyaltyScheme.Enabled = False
			cmdDeleteLoyaltyScheme.Enabled = False
			
			lvwLoyaltySchemes.Items.RemoveAt(lvwLoyaltySchemes.FocusedItem.Index)
			
			lvwLoyaltySchemes.Focus()
		
		Catch excep As System.Exception
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteLoyaltyScheme_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Exit Sub
		End Try
	End Sub
	
	Private Sub cmdEditAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAd.Click
		Dim sTmp As String = ""
		Dim oListItem As ListViewItem
		Dim sAddressUsage As String = ""
		
		Try 
			
			'Set the address count being edited - if a valid one selected
			
			If lvwAddresses.Items.Count < 1 Then
				Exit Sub
			End If
			
			'Create address component if not already done so
			If m_oAddress Is Nothing Then
				
				' Get an instance of the contactinterface object via
				' the public object manager.
				Dim temp_m_oAddress As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oAddress, "iPMBAddress.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oAddress = temp_m_oAddress
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			
			'sj 12/06/2002 - start

			m_oAddress.IsNRMA = m_bIsNRMA
			'sj 12/06/2002 - end
			
			'sj 23/07/2002 - start
			If m_bFutureDateAddressChanges And IsCorrespondanceAddress() Then

				m_oAddress.FutureDateAddressChanges = True


				m_oAddress.FutureDatedAddresses = m_vFutureDatedAddresses
			Else

				m_oAddress.FutureDateAddressChanges = False

                'NIIT - Replaced with the Migrated code 1144 
                m_oAddress.FutureDatedAddresses = Nothing
            End If
			'sj 23/07/2002 - end
			

			m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			

			m_oAddress.Reference = txtIDReference.Text

            m_oAddress.PostCode = m_sMainPostCode
			
			'set the address id

            m_oAddress.AddressCnt = m_lAddressCnt
			
			oListItem = lvwAddresses.Items.Item(m_iLine - 1)
			
			'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
			Select Case (m_sDefaultCountryCode.Trim())
				Case "GBR"

                    m_oAddress.PostCode = oListItem.Text
					sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
				Case Else

                    m_oAddress.PostCode = ListViewHelper.GetListViewSubItem(oListItem, 5).Text
					sAddressUsage = oListItem.Text
			End Select
			
			For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
				If sAddressUsage = CStr(m_vAddressTypes(1, k)) Then

					m_oAddress.AddressUsageTypeID = m_vAddressTypes(0, k)
					Exit For
				End If
			Next k
			

			m_lReturn = m_oAddress.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, edit grid

			If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			'sj 23/07/2002 - start
			If m_bFutureDateAddressChanges And IsCorrespondanceAddress() Then


				m_vFutureDatedAddresses = m_oAddress.FutureDatedAddresses
			End If
			'sj 23/07/2002 - end
			
			'Reset Interface
			cmdEditAd.Enabled = False
			cmdDeleteAd.Enabled = False
			
			With oListItem
				'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
				Select Case (m_sDefaultCountryCode.Trim())
					Case "GBR"
						' Postcode

						.Text = m_oAddress.PostalCode
						' Address usage type

						ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.AddressUsageType
						' Address lines 1-4

						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address1

						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address2

						ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address3

						ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.Address4
					Case Else
						' Address usage type

						.Text = m_oAddress.AddressUsageType
						' Address lines 1-4

						ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.Address1

						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address2

						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address3

						ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address4
						'Postcode

						ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.PostalCode
				End Select
				

				If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

					m_sMainPostCode = m_oAddress.PostalCode
					'Caption = "Corporate Client: " & m_sShortName & " " & m_sMainPostCode
				End If
				'EK 15/11/99 update the tag
				' Store the Address_cnt

                oListItem.Tag = m_oAddress.AddressCnt
			End With
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: CorrespondanceAddressExists
	'
	' Description:
	'
	' History: 23/07/2002 SJ - Created.
	'
	' ***************************************************************** '
	Private Function CorrespondanceAddressExists() As Boolean
		
		Dim result As Boolean = False
		Try 
			
			
			
            For Each oListItem As ListViewItem In lvwAddresses.Items
				If IsCorrespondanceAddress(oListItem) Then
					Return True
				End If
			Next oListItem
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = False
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CorrespondanceAddressExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CorrespondanceAddressExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: IsCorrespondanceAddress
	'
	' Description:
	'
	' History: 23/07/2002 SJ - Created.
	'
	' ***************************************************************** '
	Private Function IsCorrespondanceAddress(Optional ByVal v_vListItem As ListViewItem = Nothing) As Boolean
		
		Dim result As Boolean = False
		Try 
			
			
			Dim oListItem As ListViewItem
			Dim sAddressUsageType As String = ""
			

            ' developer guide no. changes made as per the VB6.0 code
            If Not Information.IsNothing(v_vListItem) Then

                oListItem = v_vListItem
            Else
                oListItem = lvwAddresses.FocusedItem
            End If
			
			With oListItem
				Select Case (m_sDefaultCountryCode.Trim())
					Case "GBR"
						sAddressUsageType = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
					Case Else
						sAddressUsageType = .Text
				End Select
			End With
			If sAddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then
				result = True
			End If
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = False
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsCorrespondanceAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsCorrespondanceAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdEditCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditCon.Click
		Dim oListItem As ListViewItem
		
		Try 
			
			'Set row to be deleted - if a valid one selected
			If lvwContacts.Items.Count < 1 Then
				Exit Sub
			End If
			
			'Create address component if not already done so
			If m_oContact Is Nothing Then
				
				' Get an instance of the contactinterface object via
				' the public object manager.
				Dim temp_m_oContact As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oContact, "iPMBContact.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oContact = temp_m_oContact
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contact component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					'RWH(09/06/2000) - Tidy up object.
					If Not (m_oContact Is Nothing) Then
						m_oContact = Nothing
					End If
					Exit Sub
					
				End If
				
			End If
			

			m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'RWH(09/06/2000) - Tidy up object.
				If Not (m_oContact Is Nothing) Then
					m_oContact = Nothing
				End If
				Exit Sub
			End If
			
			'set the contact id

            m_oContact.ContactCnt = m_lContactCnt

			m_oContact.Reference = txtIDReference.Text

            m_oContact.PostCode = m_sMainPostCode
			

			m_lReturn = m_oContact.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'RWH(09/06/2000) - Tidy up object.
				If Not (m_oContact Is Nothing) Then
					m_oContact = Nothing
				End If
				Exit Sub
			End If
			
			'If not cancelled, edit grid

			If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
				'RWH(09/06/2000) - Tidy up object.
				If Not (m_oContact Is Nothing) Then
					m_oContact = Nothing
				End If
				Exit Sub
			End If
			
			
			'Reset Interface
			cmdEditCon.Enabled = False
			cmdDeleteCon.Enabled = False
			
			oListItem = lvwContacts.Items.Item(m_iLine - 1)
			

			oListItem.Text = m_oContact.AreaCode
			' Column 2

			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number
			
			' Column 3

			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension
			
			' Column 4

			ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType
			
			' Column 5

			ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description
			
			
			'RWH(09/06/2000) - Tidy up object.
			If Not (m_oContact Is Nothing) Then
				m_oContact = Nothing
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			'RWH(09/06/2000) - Tidy up object.
			If Not (m_oContact Is Nothing) Then
				m_oContact = Nothing
			End If
			Exit Sub
			
		End Try
	End Sub
	
	
	
	
	' ***************************************************************** '
	' Name: SelectParty
	'
	' Description: Call Find Party component to choose a party
	'
	' ***************************************************************** '
	Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As String = "", Optional ByRef vDateCancelled As Object = Nothing) As Integer 'CT 19/07/00 added vResolvedName parameter
		
		
		Dim result As Integer = 0
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            'developer guide no. 108
            oFindParty = New iPMBFindParty.Interface_Renamed
			
			'Set appropriate key if agent only


			If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

				ReDim vKeyArray(1, 1)

				vKeyArray(0, 0) = "special_party"

				vKeyArray(1, 0) = vSpecialParty

				vKeyArray(0, 1) = PMNavKeyConst.PMKeyNameSourceId

				vKeyArray(1, 1) = m_iPartySourceId

				m_lErrorNumber = oFindParty.SetKeys(vKeyArray)
				
				If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
					oFindParty = Nothing
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			m_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			
			If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
				oFindParty = Nothing
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oFindParty.CallingAppName = "iPMBPartyCC.Interface"
			
			'SD 30/07/2002
			m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=GIIPMProcessModeGeneric, vEffectiveDate:=DateTime.Now)
			
			If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
				oFindParty = Nothing
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oFindParty.NotEditable = 1
			
			m_lErrorNumber = oFindParty.Start()
			
			If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
				oFindParty = Nothing
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
				
				vPartyCnt = oFindParty.PartyCnt
				vShortName = oFindParty.ShortName


                'vDateCancelled = oFindParty.DateCancelled
                ' developer guide no. 143(latest guide)
                vName = oFindParty.LongName

                If Not Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                    'developer guide no. 229(latest guide)
                Else
                    vName = ""
                End If
				vResolvedName = oFindParty.ResolvedName 'CT 19/07/00
				
			Else
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
            oFindParty.Dispose()
			
			oFindParty = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectPartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	'EK 9/12/99
	' ***************************************************************** '
	' Name: LockParty
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function LockParty() As Integer
		
		Dim result As Integer = 0
		Dim sLockedBy As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_lReturn = m_oPMLock.LockKey(sKeyName:="party_cnt", vKeyValue:=m_lPartyCnt, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)
			
			
			Select Case m_lReturn
				Case gPMConstants.PMEReturnCode.PMTrue
					'OK
					
				Case gPMConstants.PMEReturnCode.PMFalse
					'Locked or error
					If sLockedBy = "ERROR" Then
						result = gPMConstants.PMEReturnCode.PMFalse
						' Log Error.
						iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
						Return result
					Else
						result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Party currently locked by " & sLockedBy &
						                Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Party Lock")
						Return result
					End If
					
					
				Case Else
					result = gPMConstants.PMEReturnCode.PMFalse
					' Log Error.
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Return result
					
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: UnlockParty
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function UnlockParty() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

            m_lReturn = m_oPMLock.UnLockKey("party_cnt", m_lPartyCnt, MainModule.g_iUserId)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to unlock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: UpdateAddresses
	'
	' Description: This goes thru all addresses in the the grid control
	' and the original address array and sees what the differences
	' are. It then adds new addresses or deletes existing ones according
	' to what user has done.
	'
	' ***************************************************************** '
	Private Function UpdateAddresses() As Integer
		'RWH(11/07/2000) Altered to move PostCode from far left to far
		'right of ListView.
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
        Dim vNewAddresses(,) As String
        Dim vOldAddresses(,) As String
		Dim bFirst As Boolean
        Dim i As Integer
		Dim sAddressUsage As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Go thru original address array to get list of old addresses
			If Information.IsArray(m_vAddresses) Then
				ReDim vOldAddresses(1, m_vAddresses.GetUpperBound(1))
				For i = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                    vOldAddresses(0, i) = CStr(CInt(m_vAddresses(6, i)))
                    vOldAddresses(1, i) = CStr(CInt(m_vAddresses(1, i)))
				Next i
			End If
			
			'Go thru addresses grid to get list of new addresses
			i = 1
			bFirst = True
			Do 
				If i > lvwAddresses.Items.Count Then
					Exit Do
				End If
				
				oListItem = lvwAddresses.Items.Item(i - 1)
				
				'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
				Select Case (m_sDefaultCountryCode.Trim())
					Case "GBR"
						sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
					Case Else
						sAddressUsage = oListItem.Text.Trim()
				End Select
				
				If sAddressUsage = "" Then
					Exit Do
				Else
					
					If bFirst Then
						ReDim vNewAddresses(1, i - 1)
						bFirst = False
					Else
						ReDim Preserve vNewAddresses(1, i - 1)
					End If
					


                    vNewAddresses(0, i - 1) = Convert.ToString(oListItem.Tag)

                    For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                        'RWH(24/07/2000)
                        If sAddressUsage = m_vAddressTypes(1, j) Then
                            vNewAddresses(1, i - 1) = m_vAddressTypes(0, j)
							Exit For
						End If
					Next j
					
				End If
				i += 1
				
			Loop 
			
			'Delete old address usages in database
			If (Information.IsArray(vOldAddresses)) And (Not Information.IsArray(vNewAddresses)) Then

				m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vDeleteAddresses:=vOldAddresses)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			'Add new addresses in database
			If (Not Information.IsArray(vOldAddresses)) And (Information.IsArray(vNewAddresses)) Then

				m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vAddAddresses:=vNewAddresses)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			'If we have old and new addresses, delete common ones
			If (Information.IsArray(vOldAddresses)) And (Information.IsArray(vNewAddresses)) Then
				
				'Delete unchanged addresses (ie set them to 0)

				For i = vOldAddresses.GetLowerBound(1) To vOldAddresses.GetUpperBound(1)

					For j As Integer = vNewAddresses.GetLowerBound(1) To vNewAddresses.GetUpperBound(1)




                        If (vNewAddresses(0, j) = vOldAddresses(0, i)) And (vNewAddresses(1, j) = vOldAddresses(1, i)) Then
                            vNewAddresses(0, j) = CStr(0)
                            vOldAddresses(0, i) = CStr(0)
						End If
					Next j
				Next i
				
				'update the database

				m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vDeleteAddresses:=vOldAddresses, vAddAddresses:=vNewAddresses)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddressesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'DC 03/05/00
	
	' ***************************************************************** '
	' Name: UpdateAssociates
	'
	' Description: This goes thru all Associates in the the grid control
	' and the original Associate array and sees what the differences
	' are. It then adds new Associates or deletes existing ones according
	' to what user has done.
	'
	' ***************************************************************** '
	Private Function UpdateAssociates() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_lReturn = m_oBusiness.UpdateAssociates(vPartyCnt:=m_lPartyCnt, vAssociates:=m_vAssociates)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAssociates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAssociates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	'DC 04/08/00
	' ***************************************************************** '
	' Name: UpdateMainContact
	'
	' Description: Updates the Main Contact for the party
	'
	' ***************************************************************** '
	Private Function UpdateMainContact() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_lReturn = m_oBusiness.UpdateMainContact(vPartyCnt:=m_lPartyCnt, lMainContactCnt:=m_lMainContactCnt, sMainContactDesc:=m_sMainContactDesc)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateMainContact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateMainContact", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: PopulateProspect
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function PopulateProspect() As Integer
		Dim result As Integer = 0
		Dim vAgentReference As Object
		Dim vCurrentIntermediary As Integer
		Dim vProspectStatusID As Integer
		Dim vPreviousInsurerCnt As String = ""
        Dim vPreviousBrokerCnt, vCampaigns(,) As Object
		Dim vStrengthCodeID As String = ""
        Dim vPolicies(,) As Object
		Dim sTemp As String = ""
		Dim oListItem As ListViewItem
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Create iProspect if not already done so
			
			If m_oProspect Is Nothing Then
				
				' Get an instance of the prospect interface object via
				' the public object manager.
				Dim temp_m_oProspect As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oProspect, "bSIRProspect.Business", gPMConstants.PMGetViaClientManager)
				m_oProspect = temp_m_oProspect
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get prospect", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdProspect_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Return result
					
				End If
				
			End If
			
			'MSS200901 - Added from UW
			If m_lPartyCnt = 0 Then
				m_iProspectTask = gPMConstants.PMEComponentAction.PMAdd
				Return result
			End If
			'MSS200901 - Merge End
			
			'Always do this, as the data may have changed...

			m_lReturn = m_oProspect.GetProspectData(v_vPartyCnt:=m_lPartyCnt, r_vAgentReference:=vAgentReference, r_vCurrentIntermediary:=vCurrentIntermediary, r_vProspectStatusID:=vProspectStatusID, r_vStrengthCodeId:=vStrengthCodeID, r_vPreviousInsurerCnt:=vPreviousInsurerCnt, r_vPreviousBrokerCnt:=vPreviousBrokerCnt, r_vCampaigns:=vCampaigns, r_vPolicies:=vPolicies)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				m_iProspectTask = gPMConstants.PMEComponentAction.PMAdd
			Else
				m_iProspectTask = gPMConstants.PMEComponentAction.PMEdit
			End If
			

			If Convert.IsDBNull(vAgentReference) Or IsNothing(vAgentReference) Then
				txtAgentReference.Text = ""
			Else
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentReference, vControlValue:=vAgentReference)
				
				' Check for errors
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to assign the data.
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			'Get additional details required for display that not stored on this
			'record

			m_lReturn = m_oBusiness.GetOtherDetails(vAgentCnt:=vCurrentIntermediary, vAgentref:=m_sCurrentAgentRef, vAgentName:=m_sCurrentAgentName, vInsurerCnt:=vPreviousInsurerCnt, vInsurerRef:=m_sInsurerRef, vInsurerName:=m_sInsurerName, vBrokerCnt:=vPreviousBrokerCnt, vBrokerRef:=m_sBrokerRef, vBrokerName:=m_sBrokerName)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateProspect")
			End If
			

			If Convert.IsDBNull(vCurrentIntermediary) Or IsNothing(vCurrentIntermediary) Then
				m_lCurrentAgent = -1
			Else
				m_lCurrentAgent = vCurrentIntermediary
			End If
			
			'MKR 11/10/2004 PN 15599 Doubling the & character to avoid displaying "_"
			sTemp = m_sCurrentAgentName
			m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            'developer guide no. 26
            lblPnlCurrentAgent.Text = sTemp
			
			

			If Convert.IsDBNull(vProspectStatusID) Or IsNothing(vProspectStatusID) Then
				cboProspectingStatus.SelectedIndex = -1
			Else
				For i As Integer = 0 To cboProspectingStatus.Items.Count - 1
					If VB6.GetItemData(cboProspectingStatus, i) = vProspectStatusID Then
						cboProspectingStatus.SelectedIndex = i
					End If
				Next i
			End If
			
			'eck 2005 Roadmap - moved to DisplayLookupDetails
			'    m_lReturn& = GetLookupDetails( _
			''        sLookupTable:=SIRLookupStrengthCode, _
			''        ctlLookup:=cboStrengthCode)
			

            'developer guide no. 26
            pnlInsurerName.Name = m_sInsurerName
            txtInsurerRef.Text = m_sInsurerRef

            txtInsurerRef.Tag = IIf(Convert.IsDBNull(vPreviousInsurerCnt) Or IsNothing(vPreviousInsurerCnt), "", vPreviousInsurerCnt)


            'developer guide no. 26
            pnlBrokerName.Name = m_sBrokerName
            txtBrokerRef.Text = m_sBrokerRef
			
			'MSS200901 - Used UW code as SFORB code was less efficient(and referred to as something rude)
			If vStrengthCodeID > "" Then
				For i As Integer = 0 To cboStrengthCode.Items.Count - 1
					If VB6.GetItemData(cboStrengthCode, i) = StringsHelper.ToDoubleSafe(vStrengthCodeID) Then
						cboStrengthCode.SelectedIndex = i
					End If
				Next i
			End If
			'MSS200901 - Merge end
			
			'Get additional details required for display that not stored on this
			'record
			
			lvwCampaigns.Items.Clear()
			lvwCampaigns.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1100))
			lvwCampaigns.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1600))
			lvwCampaigns.Columns.Item(2).Width = CInt(0)
			
			If Information.IsArray(vCampaigns) Then
				
				' Assign the details to the interface.

				For i As Integer = vCampaigns.GetLowerBound(1) To vCampaigns.GetUpperBound(1)
					
					' {* USER DEFINED CODE (Begin) *}
					
					' Assign the details to the first column.
					' Column 1
					'            Call GetCampaignLookups(vCategoryId:=vCampaigns(0, i), _
					'vCategoryDescription:=vCategory)
					
					'            Set oListItem = lvwLifestyle.ListItems.Add(, , _
					'Trim$(vCategory), , CampaignImage)


                    oListItem = lvwCampaigns.Items.Add(vCampaigns(MainModule.PMBCampaignDescription, i).Trim(), "")

                    ' Assign details to other the columns
                    ' Column 2
                    m_lReturn = m_oFormFields.FormatControl(txtHiddenDate, vCampaigns(MainModule.PMBCampaignCampaignDate, i).Trim())

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text.Trim()

                    Dim TempDate As Date
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = IIf(DateTime.TryParse(vCampaigns(MainModule.PMBCampaignCampaignDate, i), TempDate), TempDate.ToString("yyyyMMdd"), vCampaigns(MainModule.PMBCampaignCampaignDate, i))

                    ' Store the id
                    oListItem.Tag = vCampaigns(MainModule.PMBCampaignRecordNo, i).Trim()
					' {* USER DEFINED CODE (End) *}
					
				Next i
			End If
			
			lvwPolicies.Items.Clear()
			lvwPolicies.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1800))
			lvwPolicies.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1600))
			lvwPolicies.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(600))
			lvwPolicies.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1600))
			
			If Information.IsArray(vPolicies) Then
				
				' Assign the details to the interface.

				For i As Integer = vPolicies.GetLowerBound(1) To vPolicies.GetUpperBound(1)
					
					' {* USER DEFINED CODE (Begin) *}
					
					' Assign the details to the first column.
					' Column 1
					'            Call GetCampaignLookups(vCategoryId:=vCampaigns(0, i), _
					'vCategoryDescription:=vCategory)
					
					'            Set oListItem = lvwLifestyle.ListItems.Add(, , _
					'Trim$(vCategory), , CampaignImage)
					For j As Integer = 0 To cboPolicyType.Items.Count - 1

                        If StringsHelper.ToDoubleSafe(vPolicies(MainModule.PMBPolicyPolicyTypeID, i).Trim()) = VB6.GetItemData(cboPolicyType, j) Then
							sTemp = VB6.GetItemString(cboPolicyType, j).Trim()
							Exit For
						End If
					Next j
					
					'Set oListItem = lvwPolicies.ListItems.Add(, , _
					'Trim$(vPolicies(PMBPolicyTypeDescription, i)), , PolicyImage)
					

                    oListItem = lvwPolicies.Items.Add(sTemp, "PolicyImage")
					
					' Assign details to other the columns
					' Column 2

                    m_lReturn = m_oFormFields.FormatControl(txtHiddenDate, vPolicies(MainModule.PMBPolicyRenewalDate, i).Trim())

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text.Trim()

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = vPolicies(MainModule.PMBPolicyNoOfTimesQuoted, i).Trim()

                    m_lReturn = m_oFormFields.FormatControl(txtHiddenCurrency, vPolicies(MainModule.PMBPolicyTargetPremium, i).Trim())

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text.Trim()

                    Dim TempDate2 As Date
                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = IIf(DateTime.TryParse(vPolicies(MainModule.PMBPolicyRenewalDate, i), TempDate2), TempDate2.ToString("yyyyMMdd"), vPolicies(MainModule.PMBPolicyRenewalDate, i))

                    ' Store the id
                    oListItem.Tag = vPolicies(MainModule.PMBPolicyPolicyID, i).Trim()
					' {* USER DEFINED CODE (End) *}
					
				Next i
			End If
			
			'SD 22/08/2002 If Broking, then previous broker irrelevant
			If m_sUnderwritingOrAgency = "A" Then
				fraPreviousBroker.Visible = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateProspect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateProspect", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SaveProspect
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function SaveProspect() As Integer
		
		Dim result As Integer = 0
		Dim vTurnover As Integer
		Dim vAgentReference As Object
		Dim vCurrentIntermediary As Integer
		Dim vProspectStatusID As Integer
		Dim vStrengthCodeID As Integer
		Dim vPreviousInsurerCnt As Integer
		Dim vPreviousBrokerCnt As Integer
        Dim vWageRoll As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_bAONPRClientScreenChanges Then

				vTurnover = CInt(m_oFormFields.UnformatControl(ctlControl:=txtTurnover))
			Else
				vTurnover = cboTurnover.ItemId
			End If
			


			vWageRoll = m_oFormFields.UnformatControl(ctlControl:=txtWageRoll)
			


			vAgentReference = m_oFormFields.UnformatControl(ctlControl:=txtAgentReference)
			
			If cboProspectingStatus.SelectedIndex <> -1 Then
				vProspectStatusID = VB6.GetItemData(cboProspectingStatus, cboProspectingStatus.SelectedIndex)
			End If
			
			vCurrentIntermediary = m_lCurrentAgent
			
			If Convert.ToString(txtInsurerRef.Tag) = "" Then
				vPreviousInsurerCnt = 0
			Else
				vPreviousInsurerCnt = CInt(Convert.ToString(txtInsurerRef.Tag))
			End If
			If Convert.ToString(txtBrokerRef.Tag) = "" Then
				vPreviousBrokerCnt = 0
			Else
				vPreviousBrokerCnt = CInt(Convert.ToString(txtBrokerRef.Tag))
			End If
			If cboStrengthCode.SelectedIndex <> -1 Then
				vStrengthCodeID = VB6.GetItemData(cboStrengthCode, cboStrengthCode.SelectedIndex)
			Else
				vStrengthCodeID = 0
			End If
			
			If m_iProspectTask = gPMConstants.PMEComponentAction.PMAdd Then
				

				m_lReturn = m_oProspect.EditAdd(vPartyCnt:=m_lPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				m_iProspectTask = gPMConstants.PMEComponentAction.PMEdit
				
			Else
				

				m_lReturn = m_oProspect.Editupdate(vPartyCnt:=m_lPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				

				m_lReturn = m_oProspect.Update()
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveProspect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveProspect", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: UpdateContacts
	'
	' Description: This goes thru all contacts in the the grid control
	' and the original contact array and sees what the differences
	' are. It then adds new contacts or deletes existing ones according
	' to what user has done.
	'
	' ***************************************************************** '
	Private Function UpdateContacts() As Integer
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
		Dim vNewContacts, vOldContacts As Object
		Dim bFirst As Boolean
        Dim i As Integer
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			'Go thru original contact array to get list of old contacts
			If Information.IsArray(m_vContacts) Then
				ReDim vOldContacts(m_vContacts.GetUpperBound(1))
				For i = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                    vOldContacts(i) = CInt(m_vContacts(0, i))
				Next i
			End If
			
			'Go thru contacts grid to get list of new contacts
			'SP171298
			i = 1
			bFirst = True
			
			Do 
				If i > lvwContacts.Items.Count Then
					Exit Do
				End If
				oListItem = lvwContacts.Items.Item(i - 1)
				If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
					Exit Do
				Else
					If bFirst Then
						ReDim vNewContacts(i - 1)
						bFirst = False
					Else
						ReDim Preserve vNewContacts(i - 1)
					End If
					


                    vNewContacts(i - 1) = Convert.ToString(oListItem.Tag)
					
				End If
				i += 1
			Loop 
			
			
			'Delete old contact usages in database
			If (Information.IsArray(vOldContacts)) And (Not Information.IsArray(vNewContacts)) Then

				m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vDeleteContacts:=vOldContacts)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			'Add new contacts in database
			If (Not Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then

				m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vAddContacts:=vNewContacts)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			
			'If we have old and new Contacts, delete common ones
			If (Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then
				
				'Delete unchanged Contacts (ie set them to 0)

				For i = vOldContacts.GetLowerBound(0) To vOldContacts.GetUpperBound(0)

					For j As Integer = vNewContacts.GetLowerBound(0) To vNewContacts.GetUpperBound(0)


                        If vNewContacts(j).Equals(vOldContacts(i)) Then

                            vNewContacts(j) = 0

                            vOldContacts(i) = 0
						End If
					Next j
				Next i
				
				'update the database

				m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vDeleteContacts:=vOldContacts, vAddContacts:=vNewContacts)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContactsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	'eck060700
	' ***************************************************************** '
	' Name: UpdateOrion
	'
	' Description: Update the party_address usage table with old
	' and new addresses for the party.
	'
	' ***************************************************************** '
	Private Function UpdateOrion(ByRef vPartyCnt As Object) As gPMConstants.PMEReturnCode
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim oSIROrionUpdate As bSIROrionUpdate.Business
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If Not m_oBusiness.IsOrionInstalled Then
				Return result
			End If
			
			Dim temp_oSIROrionUpdate As Object
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_oSIROrionUpdate, "bSIROrionUpdate.Business", gPMConstants.PMGetViaClientManager)
			oSIROrionUpdate = temp_oSIROrionUpdate
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMError
				
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bSIROrionUpdate.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return result
			End If
			' Get Orion Account IDs
			'DN 01/06/01 - Change PartyIDs to Longs
			Select Case Task
				Case gPMConstants.PMEComponentAction.PMAdd

					m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=vPartyCnt)
				Case gPMConstants.PMEComponentAction.PMEdit

					m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=vPartyCnt, v_iOldSourceId:=m_iPartySourceId, v_iOldPartyId:=m_lPartyId)
			End Select
			

            oSIROrionUpdate.Dispose()
			
			oSIROrionUpdate = Nothing
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	
	Private Sub cmdEditConv_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditConv.Click
		Dim oListItem As ListViewItem
		Dim vCategory As String = ""
		
		If m_lPartyCnt < 1 Then
			MessageBox.Show("Must Save Client Details before editing Convictions", Application.ProductName)
		End If
		
		Try 
			
			'Set row to be edited - if a valid one selected
			If lvwConvictions.Items.Count < 1 Then
				MessageBox.Show("You must maintain Insured Details from the main screen", Application.ProductName)
				Exit Sub
			End If
			
			'Create conviction component if not already done so
			If m_oConviction Is Nothing Then
				
				' Get an instance of the contactinterface object via
				' the public object manager.
				Dim temp_m_oConviction As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oConviction, "iPMBPartyConviction.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oConviction = temp_m_oConviction
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get conviction component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			

			m_lReturn = m_oConviction.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'set the Ids

			m_oConviction.PartyCnt = m_lPartyCnt
			


			m_oConviction.PartyConvictionID = Convert.ToString(lvwConvictions.FocusedItem.Tag)
			
			m_iLine = lvwConvictions.FocusedItem.Index + 1
			

			m_lReturn = m_oConviction.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, edit grid

			If m_oConviction.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			'Reset Interface
			cmdEditConv.Enabled = False
			cmdDeleteConv.Enabled = False
			
			oListItem = lvwConvictions.Items.Item(m_iLine - 1)
			
			' Assign details to other the columns
			'EK 12/12 Fixed to match data/headings
			' Column 1

			oListItem.Text = m_oConviction.Code
			
			'Conviction Date

			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=m_oConviction.ConvictionDate.Trim())
			
			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text.Trim()
			
			'Description

			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oConviction.Description
			
			' Fine

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenCurrency, vControlValue:=m_oConviction.FineAmount)
			
			ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text.Trim()
			
			' Conviction Status

			ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oConviction.StatusCode
			
			' Penalty Points

			ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oConviction.DrivingLicencePenaltyPoints
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditConv_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdEditPol_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditPol.Click
		Dim oListItem As ListViewItem
        'According to user guide this functionality is not supported more in Pure BO  (Confirmation by Mousumi Borah)
        Exit Sub
		If m_lPartyCnt < 1 Then
			MessageBox.Show("Must Save Client Details before editing Prospect Policies", Application.ProductName)
		End If
		
		Try 
			
			'Create prospect policy component if not already done so
			If m_oProspectPolicy Is Nothing Then
				
				' Get an instance of the prospect policy interface object via
				' the public object manager.
				Dim temp_m_oProspectPolicy As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oProspectPolicy, "iPMBProspectPolicy.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oProspectPolicy = temp_m_oProspectPolicy
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get prospect policy component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditPol_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			

			m_lReturn = m_oProspectPolicy.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			
			'set the Ids
			oListItem = lvwPolicies.FocusedItem
			


			m_oProspectPolicy.PolicyTypeID = Convert.ToString(oListItem.Tag)
			

			m_oProspectPolicy.PolicyTypeDescription = oListItem.Text
			
			txtHiddenDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
			


			m_oProspectPolicy.RenewalDate = m_oFormFields.UnformatControl(txtHiddenDate)
			

			m_oProspectPolicy.NoOfTimesQuoted = ListViewHelper.GetListViewSubItem(oListItem, 2).Text
			
			txtHiddenCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, 3).Text
			


			m_oProspectPolicy.TargetPremium = m_oFormFields.UnformatControl(txtHiddenCurrency)
			

			m_lReturn = m_oProspectPolicy.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, edit grid

			If m_oProspectPolicy.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			m_bChangedProspectPolicies = True
			
			'Reset Interface
			cmdEditPol.Enabled = False
			cmdDeletePol.Enabled = False
			
			'Set oListItem = lvwLifestyle.ListItems.Item(m_iLine)
			

			oListItem.Text = m_oProspectPolicy.PolicyTypeDescription

			oListItem.Tag = m_oProspectPolicy.PolicyTypeID
			
			' Assign details to other the columns
			' Column 2

			m_lReturn = m_oFormFields.FormatControl(txtHiddenDate, m_oProspectPolicy.RenewalDate)
			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text
			
			' Column 3

			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oProspectPolicy.NoOfTimesQuoted
			' Column 4
			

			m_lReturn = m_oFormFields.FormatControl(txtHiddenCurrency, m_oProspectPolicy.TargetPremium)
			ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text
			
			'Column 5

            ' developer guide no. 197(Latest Guide)
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = DateTime.FromOADate(m_oProspectPolicy.RenewalDate.ToOADate).ToString("yyyyMMdd")
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditPol_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditPol_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdEditLoyaltyScheme_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditLoyaltyScheme.Click 'RAW 18/11/2002 : PS005 : Added
		Dim oListItem As ListViewItem
		Dim vCategory As String = ""
		
		Try 
			
			If m_lPartyCnt < 1 Then
				' This is a new party that has not yet been added to the database so no party_cnt has been generated
				
				' ?? will this ever happen since party was forced to be saved when
				' adding the first loyalty scheme and edit is only enabled when an
				' entry has been selected from the list view
				MessageBox.Show("Must Save Client Details before editing PartyLoyaltySchemes", Application.ProductName)
			End If
			
			'Set row to be edited - if a valid one selected
			If lvwLoyaltySchemes.FocusedItem.Index + 1 < 1 Then
				Exit Sub
			End If
			
			'Create PartyLoyaltyScheme component if not already done so
			If m_oPartyLoyaltyScheme Is Nothing Then
				
				' Get an instance of the contactinterface object via
				' the public object manager.
				Dim temp_m_oPartyLoyaltyScheme As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oPartyLoyaltyScheme, "iPMBPartyLoyaltyScheme.Interface_Renamed", gPMConstants.PMGetLocalInterface)
				m_oPartyLoyaltyScheme = temp_m_oPartyLoyaltyScheme
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PartyLoyaltyScheme component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			

			m_lReturn = m_oPartyLoyaltyScheme.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'set the Ids

			m_oPartyLoyaltyScheme.PartyCnt = m_lPartyCnt
			


            m_oPartyLoyaltyScheme.PartyLoyaltySchemeID = Convert.ToString(lvwLoyaltySchemes.FocusedItem.Tag)
			
			m_iLine = lvwLoyaltySchemes.FocusedItem.Index + 1
			

			m_lReturn = m_oPartyLoyaltyScheme.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, edit grid

			If m_oPartyLoyaltyScheme.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			'Reset Interface
			cmdEditLoyaltyScheme.Enabled = False
			cmdDeleteLoyaltyScheme.Enabled = False
			
			oListItem = lvwLoyaltySchemes.Items.Item(m_iLine - 1)
			
			' Assign details to other the columns
			' Column 1

			oListItem.Text = m_oPartyLoyaltyScheme.LoyaltySchemeName
			
			' Assign details to other the columns
			' Column 2

			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oPartyLoyaltyScheme.MemberNumber
			
			' Column 3

			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=m_oPartyLoyaltyScheme.StartDate)
			
			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtHiddenDate.Text.Trim()
			
			' Column 4

			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=m_oPartyLoyaltyScheme.EndDate)
			
			ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenDate.Text.Trim()
			
			' Store the PartyLoyaltySchemeId

            oListItem.Tag = m_oPartyLoyaltyScheme.PartyLoyaltySchemeID
			
			lvwLoyaltySchemes.FocusedItem.Selected = False
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditLoyaltyScheme_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdEditLoyaltyScheme_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditLoyaltyScheme.Enter 'RAW 18/11/2002 : PS005 : Added
		
		'If we're here by tabbing (or back-tabbing) we're on tab 5
		SSTabHelper.SetSelectedIndex(tabMainTab, 5)
		
	End Sub
	
    Public Sub ShowHelpScreen(Optional ByRef cmdHelp As Object = Nothing, Optional ByRef ScreenHelpID As Object = Nothing)
        ' Fire up the help screen
        'developer guide no. 184
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID)

    End Sub
	
    Private Sub cmdInsurerLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsurerLookup.Click

        Dim vCnt, vShortName, vName As Object
        Dim sTemp As String = ""

        Try




            'developer guide no. 98(latest guide)
            m_lReturn = SelectParty(vCnt, vShortName, vName, "IN")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            txtInsurerRef.Tag = CStr(vCnt)


            m_sInsurerRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(txtInsurerRef, m_sInsurerRef)


            m_sInsurerName = CStr(vName)

            sTemp = m_sInsurerName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(sTemp, "&")

            'developer guide no. 26
            lblPnlInsurerName.Text = sTemp

            m_bChangedProspect = True

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, gPMConstants.PMErrorText, MainModule.ACApp, ACClass, "Err_cmdInsurerLookup_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
	
	Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_8.Click, _cmdPrevious_7.Click, _cmdPrevious_6.Click, _cmdPrevious_5.Click, _cmdPrevious_3.Click, _cmdPrevious_4.Click, _cmdPrevious_2.Click, _cmdPrevious_1.Click
		Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)
		
		Try 
			
			' Change to the next tab.
			If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
				SSTabHelper.SetSelectedIndex(tabMainTab, Index - 1)
			End If
			
			' Set focus to the first control on the tab.
			If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(MainModule.ACControlStart, Index - 1).Focus()
			End If
		
		Catch 
			
			
			
			' Error Section
			
			Exit Sub
		End Try
		
		
	End Sub
	
	
	' PRIVATE Events (Begin)
	
	' ***************************************************************** '
	' Name: DisableForm
	'
	' Description: Sets all of the interface details to the disable
	'              state passed.
	'
	' ***************************************************************** '
	Private Function DisableForm(ByRef lDisabled As Integer) As Integer
		
        Dim result As Integer = 0
        Dim ctlFormControl As System.Windows.Forms.Control
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC030400 added check for uctDropdown
            ' Set all of the forms controls to the disable state.
            For Each ctlFormControl In Me.Controls
                If (TypeOf ctlFormControl Is System.Windows.Forms.TabControl) Then
                    For Each ctlTabPage As Control In ctlFormControl.Controls
                        If (TypeOf ctlTabPage Is System.Windows.Forms.TabPage) Then
                            For Each ctl As Control In ctlTabPage.Controls
                                If (TypeOf ctl Is GroupBox) Then
                                    For Each ctlChild As Control In ctl.Controls
                                        If (TypeOf ctlChild Is System.Windows.Forms.TextBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is System.Windows.Forms.ComboBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is System.Windows.Forms.CheckBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                            'ElseIf (TypeOf ctlChild Is AxThreed.AxSSOption) Then
                                            '    ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is PMListMgrDropdown.uctDropdown) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is PMLookupControl.cboPMLookup) Then
                                            ctlChild.Enabled = Not lDisabled
                                        End If
                                    Next
                                End If
                                'Check the type of the control.
                                If (TypeOf ctl Is System.Windows.Forms.TextBox) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is System.Windows.Forms.ComboBox) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is System.Windows.Forms.CheckBox) Then
                                    ctl.Enabled = Not lDisabled
                                    'ElseIf (TypeOf ctl Is AxThreed.AxSSOption) Then
                                    '    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is PMListMgrDropdown.uctDropdown) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is PMLookupControl.cboPMLookup) Then
                                    ctl.Enabled = Not lDisabled
                                End If
                            Next
                        End If
                    Next

                End If
            Next

            cboCurrency.Enabled = Not lDisabled

            'Now the command buttons...
            cmdAgentLookUp.Enabled = Not lDisabled

            cmdConsultantLookup.Enabled = Not lDisabled
            cmdAddAd.Enabled = Not lDisabled
            cmdAddCon.Enabled = Not lDisabled
            cmdAddConv.Enabled = Not lDisabled
            'cmdAddLife.Enabled = Not lDisabled
            cmdCurrentAgent.Enabled = Not lDisabled
            cmdAddPol.Enabled = Not lDisabled
            cmdAddLoyaltyScheme.Enabled = Not lDisabled ' RAW 18/11/2002 : PS005 : Added
            cmdCurrentAgent.Enabled = Not lDisabled
            cmdInsurerLookup.Enabled = Not lDisabled
            cmdBrokerLookup.Enabled = Not lDisabled
            uctPartyTax1.ReadOnly_Renamed = lDisabled

            uctPartyBankControl1.ReadOnly_Renamed = lDisabled
            'ED 19/11/2002 - Added
            'PSS224
            'cmdProspect.Enabled = Not lDisabled

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetParty
	'
	' Description:
	'
	' ***************************************************************** '
	Public Function GetParty() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check the task.
			If Task = gPMConstants.PMEComponentAction.PMEdit Or Task = gPMConstants.PMEComponentAction.PMView Then
				' Get the interface details from the
				' business object.
				m_lReturn = GetBusiness()
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to get the details.
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				' Assign the details from the business object
				' to the interface.
				m_lReturn = BusinessToInterface()
				
				' Check for errors
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to assign the details.
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			Else
				'sj 17/06/2002 - start
				m_lReturn = GetDefaultAgentDetails()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				'sj 17/06/2002 - end
				LoadPartyBankControl()
			End If
			
			' Display all of the lookup details.
			m_lReturn = DisplayLookupDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'2005 Roadmap changes
			'Prospect Now on the main tab
			
			PopulateProspect()
			
			' Check the task.
			If Task = gPMConstants.PMEComponentAction.PMView Then
				' Disable the interface to only allow viewing.
				m_lReturn = DisableForm(lDisabled:=True)
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to disable the interface
					result = gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'eck011001
	' ***************************************************************** '
	' Name: ApplyParty
	'
	' Description: Sets necessary business parameters to allow save after apply
	'
	' ***************************************************************** '
	Public Function ApplyParty() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_lReturn = LockParty()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to disable the interface
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = m_oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to disable the interface
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the party", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ProcessCommand
	'
	' Description: Determines which action to take on the details
	'              depending upon the task and interface state.
	'
	' ***************************************************************** '
	Public Function ProcessCommand() As Integer
		
		Dim result As Integer = 0
		Dim iMsgResult As DialogResult
		Dim sMessage, sTitle As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check the task.
			If Status <> gPMConstants.PMEReturnCode.PMCancel Then
				
				Select Case (Task)
					Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
						' Update the business from the interface.
						m_lReturn = InterfaceToBusiness()
						
						' Check for errors.
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							' Failed to update business.
							Return gPMConstants.PMEReturnCode.PMFalse
						End If
				End Select
				
			End If
			
			' Check the task.
			Select Case (Task)
				Case gPMConstants.PMEComponentAction.PMAdd
					' Check if form has been cancelled, if so,
					' prompt if you wish to lose details.
					If Status = gPMConstants.PMEReturnCode.PMCancel Then
						' Get string messages
						

                        'developer guide no. 243
                        sTitle = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACCancelDetailsTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        ' developer guide no. 243
                        sMessage = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACCancelDetails, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
						
						iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
						
						' Check message result.
						If iMsgResult = System.Windows.Forms.DialogResult.No Then
							' Set return to false, meaning
							' don't cancel.
							result = gPMConstants.PMEReturnCode.PMFalse
						End If
					Else
						' Form hasn't been cancelled, so we just go
						' ahead and add the details.
						
						' Add the details using the business object.

						m_lReturn = m_oBusiness.Update()
						
						' Check for errors.
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							' Failed to add the details
							result = gPMConstants.PMEReturnCode.PMFalse
							
							' Log Error.
							iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
						End If
					End If
					
				Case gPMConstants.PMEComponentAction.PMEdit
					' Check if form has been cancelled, if so,
					' check if the details have changed and if
					' so, prompt if they wish to cancel.
					If Status = gPMConstants.PMEReturnCode.PMCancel Then
						' Check the details havn't changed.

						m_lReturn = m_oBusiness.Cancel()
						'MH Request - Always confirm cancellation
						'                If (m_lReturn& = PMDataChanged) Then
						' Get string messages
						

                        sTitle = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACCancelDetailsTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        'developer guide no. 243
                        sMessage = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACCancelDetails, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
						
						iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
						
						' Check message result.
						If iMsgResult = System.Windows.Forms.DialogResult.No Then
							' Set return to false, meaning
							' don't cancel.
							result = gPMConstants.PMEReturnCode.PMFalse
						End If
						'                End If
					Else
						
						'Pass back old and new address
						m_lReturn = GetOldAndNewAddress()

						m_oBusiness.OldAddress = VB6.CopyArray(m_vOldAddress)

						m_oBusiness.NewAddress = VB6.CopyArray(m_vNewAddress)
						
						'Pass back old and new client code

						m_oBusiness.OldClientCode = m_sOldIdReference.Trim()

						m_oBusiness.NewClientCode = txtIDReference.Text.Trim()
						
						'Pass back old and new client name

						m_oBusiness.OldClientName = m_sOldResolvedName.Trim()

						m_oBusiness.NewClientName = m_sResolved.Trim()
						
						' Update the details using the business object.

						m_lReturn = m_oBusiness.Update()
						
						' Check for errors.
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							' Failed to update the details
							result = gPMConstants.PMEReturnCode.PMFalse
							
							' Log Error.
							iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
						End If
						'eck060700

						m_lReturn = UpdateOrion(vPartyCnt:=m_oBusiness.PartyCnt)
						' Check for errors.
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							' Failed to add the details
							result = gPMConstants.PMEReturnCode.PMFalse
							
							' Log Error.
							iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the Orion details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
						End If
					End If
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: Terminate (Standard Method)
	'
	' Description: Entry point for any termination code for this
	'              object.
	'
	' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
			
			
    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
			For i As Integer = m_ctlTabFirstLast.GetLowerBound(1) To m_ctlTabFirstLast.GetUpperBound(1)
                m_ctlTabFirstLast(MainModule.ACControlStart, i) = Nothing
                m_ctlTabFirstLast(MainModule.ACControlEnd, i) = Nothing
			Next 
				
                If g_oGIS IsNot Nothing Then
                    g_oGIS.Dispose()
				g_oGIS = Nothing
			End If
                If PMBGeneralFunc.g_oListManager IsNot Nothing Then
                    PMBGeneralFunc.g_oListManager.Dispose()
				PMBGeneralFunc.g_oListManager = Nothing
			End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
				m_oBusiness = Nothing
			End If
                If m_oPMLock IsNot Nothing Then
                    'If in edit mode, unlock the party
                    If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                        m_lReturn = UnlockParty()
                    End If
                    m_oPMLock.Dispose()
                    m_oPMLock = Nothing
                End If
                If m_oPMUser IsNot Nothing Then
                    m_oPMUser.Dispose()
				m_oPMUser = Nothing
			End If
                If m_oFormFields IsNot Nothing Then
                    m_oFormFields.Dispose()
				m_oFormFields = Nothing
			End If
                If m_oProspect IsNot Nothing Then
                    m_oProspect.Dispose()
				m_oProspect = Nothing
			End If
                If m_oProspectPolicy IsNot Nothing Then
                    m_oProspectPolicy.Dispose()
				m_oProspectPolicy = Nothing
			End If
                If m_oAddress IsNot Nothing Then
                    m_oAddress.Dispose()
				m_oAddress = Nothing
			End If
                If m_oContact IsNot Nothing Then
                    m_oContact.Dispose()
				m_oContact = Nothing
			End If
                If m_oConviction IsNot Nothing Then
                    m_oConviction.Dispose()
				m_oConviction = Nothing
			End If
                If m_oAssociates IsNot Nothing Then
                    m_oAssociates.Dispose()
				m_oAssociates = Nothing
			End If
                If m_oPartyLoyaltyScheme IsNot Nothing Then
                    m_oPartyLoyaltyScheme.Dispose()
				m_oPartyLoyaltyScheme = Nothing
			End If
                If m_oAccount IsNot Nothing Then
                    m_oAccount.Dispose()
				m_oAccount = Nothing
			End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
				m_oCurrencyConvert = Nothing
			End If
                If m_oClientNumber IsNot Nothing Then
                    m_oClientNumber.Dispose()
                    m_oClientNumber = Nothing
				End If
                If MainModule.g_oObjectManager IsNot Nothing Then
                    MainModule.g_oObjectManager.Dispose()
                MainModule.g_oObjectManager = Nothing
			End If
			
			
            End If
        End If
        Me.disposedValue = True
    End Sub
			
	
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Entry point for any initialisation code for this
	'              object.
	'
	' ***************************************************************** '
	Public Function Initialise() As Integer
		
		Dim result As Integer = 0
		Dim sHelpFile, sValue As String
		Dim m_lReturn As Integer
		
		Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
		Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
		Dim eProductFamily As gPMConstants.PMEProductFamily
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create an instance of the object manager.
            MainModule.g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = MainModule.g_oObjectManager.Initialise(MainModule.ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                MainModule.g_oObjectManager = Nothing
				
				' Log Error.
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
				
				Return result
			End If
			
			' Store the language ID from the object manager
			' to the public variables, to enable us to use
			' them throughout the object.
            With MainModule.g_oObjectManager
                MainModule.g_iLanguageID = .LanguageID
                MainModule.g_iSourceID = .SourceID
                MainModule.g_iUserId = .UserID
                MainModule.g_iCurrencyID = .CurrencyID 'PN16993
                m_iDefaultCountryID = .CountryID
			End With
			
            'Get bPMLock
            Dim temp_m_oPMLock As Object
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If
            'eck010900
            'Get Instance of bPMUser
            Dim temp_m_oPMUser As Object
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oPMUser, "bPMUser.Business", gPMConstants.PMGetViaClientManager)
            m_oPMUser = temp_m_oPMUser

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMUser", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            ' Initialise the process modes.
            'm_iTask% = PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            'SD 30/07/2002
            m_lProcessMode = GIIPMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'developer guide no. 39(No Solutions)
                'App.HelpFile = sHelpFile
            End If

            'DN 05/06/01 - Late bind list manager to avoid clash between GI and GII
            Dim temp_g_oListManager As Object
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_g_oListManager, "iGEMListManager.Interface_Renamed", gPMConstants.PMGetLocalInterface)
            PMBGeneralFunc.g_oListManager = temp_g_oListManager

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iGEMListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            '2005 New Client Screen Layout
            Dim temp_m_oAccount As Object
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oAccount, "bACTAccount.Form", gPMConstants.PMGetViaClientManager)
            m_oAccount = temp_m_oAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTAccount", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If
            Dim temp_m_oCurrencyConvert As Object
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", gPMConstants.PMGetViaClientManager)
            m_oCurrencyConvert = temp_m_oCurrencyConvert

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTCurrencyConvert", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If
            '2005 End

            'DJM 13/01/2004 : Copied m_bAONPRClientScreenChanges from 1.8.5 issue 5877.
            'sj 11/06/2002 - start
            'sj 02/07/2002 -Add AONAffinity option
            'DJM 19/08/2003 : Add mandatory business field option.
            m_lReturn = PartyFunc.GetHiddenOptions(MainModule.g_iSourceID, m_bIsNRMA, m_bValidateAlternativeIdentifier, m_bAONAffinity, , m_bFutureDateAddressChanges, m_bMultiTreeAccounting, , , m_bBusinessFieldOnClientIsMandatory, m_bAONPRClientScreenChanges)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'sj 11/06/2002 - end

            'FSA Phase III

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for FSA Compliance", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue <> "1" Then
                lblTobLetter.Visible = False
                lblTobLetter.Enabled = False
                txtTobLetter.Visible = False
                txtTobLetter.Enabled = False
            End If
            'FSA Phase III

            'PN 17153  - Start
            'Hiding Sub Branch in case of Broking and when the product option is not set
            m_lReturn = iPMFunc.getUnderwritingOrAgency(m_sUnderwritingOrAgency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingOrAgency Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Broking doesn't show Sub Branch by default
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSubBranchShowingForBroking, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bShowSubBranchID = (sValue = "1") Or (m_sUnderwritingOrAgency = "U")
            'PN 17153 End

            'Retrieve System Option for Duplicate Client Identification
            m_lReturn = iPMFunc.GetSystemOption(MainModule.SYSOPTDuplicateClientIdentification, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for Duplicate Client Identificaiton", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bDuplicateClientIdentification = (sValue = "1")

            ' Get System Option for Client BlackListing
            m_lReturn = iPMFunc.GetSystemOption(MainModule.kSystemOptionClientBlacklistingInForce, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for Client BlackListing In Force", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bSystemOptionClientBlacklistingInForce = (sValue = "1")

            'Get System Options for Corporate Clients
            'No. of Employees Mandetory
            m_lReturn = iPMFunc.GetSystemOption(MainModule.SYSOPTCCEmployees, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for Corporate Client No. of Employees Mandetory", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bCCNoOfEmployees = (sValue = "1")

            'Turnover Band Mandetory
            m_lReturn = iPMFunc.GetSystemOption(MainModule.SYSOPTCCTurnoverBand, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for Corporate Client Turnover Band Mandetory", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bCCTurnOverBand = (sValue = "1")

            Return result

        Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: UnloadControl
	'
	' Description:
	'
	' ***************************************************************** '
	Public Function UnloadControl() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = ProcessCommand()
			
			' Check the return value.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Return result
			End If
			
			' Terminate the general object.
            Dispose()
			' Terminate the business object

            m_oBusiness.Dispose()
			' Destroy the instance of the business object
			' from memory.
			m_oBusiness = Nothing
			
			' Terminate the form control object.
            m_oFormFields.Dispose()
			' Destroy the instance of the form control object
			' from memory.
			m_oFormFields = Nothing
			
			' Terminate the address object (if used)
			If Not (m_oAddress Is Nothing) Then
				
				
                m_oAddress.Dispose()
				' Destroy the instance of the Address object
				' from memory.
				m_oAddress = Nothing
				
			End If
			
			' Terminate the contact object (if used)
			If Not (m_oContact Is Nothing) Then
				
				
                m_oContact.Dispose()
				' Destroy the instance of the contact object
				' from memory.
				m_oContact = Nothing
				
			End If
			
			'2005 New Client Screen Layout
			If Not (m_oAccount Is Nothing) Then

                m_oAccount.Dispose()
				m_oAccount = Nothing
			End If
			
			
			
			' Reset the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnloadControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'Party Bank Details
	Public Function LoadPartyBankControl() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "LoadPartyBankControl"
		
        Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no. 9
        m_lReturn = uctPartyBankControl1.Initialise()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctPartyBankControl1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		uctPartyBankControl1.PartyCnt = m_lPartyCnt
		
        'developer guide no. 68
        m_lReturn = uctPartyBankControl1.Load_Renamed
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "uctPartyBankControl1.Load Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
		
		
		
        Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
        Finally
		
            '        Return result
            '        Resume
            '        Return result
        End Try
		Return result
	End Function
	
	Private Sub ddBusiness_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddBusiness.GotFocus
		
		If ddBusiness.Text = "" Then
			m_lReturn = HighlightContol(ddBusiness, optBoolDropDown:=True)
		End If
		
	End Sub
	
	Private Sub ddBusiness_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddBusiness.LostFocus
		
		m_lReturn = ValidateListField(ddBusiness)
		
    End Sub

    Private Sub ddPaymentMethod_Change(ByVal Sender As Object, ByVal e As System.EventArgs) Handles ddPaymentMethod.Change
        If ddPaymentMethod.Text = MainModule.CreditCard Or ddPaymentMethod.Text = MainModule.DebitCard Then
            cboCreditCard.Text = m_sCreditCardCode
            lblCreditCard.Visible = True
            cboCreditCard.Visible = True
        Else
            lblCreditCard.Visible = False
            cboCreditCard.Visible = False
            cboCreditCard.Text = ""
        End If
    End Sub


	Private Sub ddTrade_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddTrade.GotFocus
		
		If ddTrade.Text = "" Then
			m_lReturn = HighlightContol(ddTrade, optBoolDropDown:=True)
		End If
		
	End Sub
	
	Private Sub ddTrade_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddTrade.LostFocus
		
		m_lReturn = ValidateListField(ddTrade)
		
	End Sub
	
	Private Sub lvwAddresses_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.Click
		If lvwAddresses.Items.Count > 0 Then
			m_lAddressCnt = Convert.ToString(lvwAddresses.FocusedItem.Tag)
			m_iLine = lvwAddresses.FocusedItem.Index + 1
		End If
	End Sub
	
	Private Sub lvwAddresses_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.DoubleClick
		If lvwAddresses.Items.Count > 0 Then

			m_lAddressCnt = Convert.ToString(lvwAddresses.FocusedItem.Tag)
		End If
	End Sub
	
	Private Sub lvwAddresses_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.Enter
		'Check we are on correct tab
		If SSTabHelper.GetSelectedIndex(tabMainTab) <> 1 Then
			SSTabHelper.SetSelectedIndex(tabMainTab, 1)
		End If
	End Sub
	
	Private Sub lvwAddresses_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAddresses.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
		
		If lvwAddresses.GetItemAt(x, y) Is Nothing Then
			cmdDeleteAd.Enabled = False
			cmdEditAd.Enabled = False
		Else
			'DC030401 check for view mode first ....
			'        cmdDeleteAd.Enabled = True
			'        cmdEditAd.Enabled = True
			If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
				cmdEditAd.Enabled = True
				cmdDeleteAd.Enabled = True
			End If
		End If
		
	End Sub
	
	Private Sub lvwContacts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.Click
		
		If lvwContacts.Items.Count > 0 Then

			m_lContactCnt = Convert.ToString(lvwContacts.FocusedItem.Tag)
			m_iLine = lvwContacts.FocusedItem.Index + 1
		End If
		
		' Moved these into MouseDown event
		'cmdDeleteCon.Enabled = True
		'cmdEditCon.Enabled = True
		
	End Sub
	
	Private Sub lvwContacts_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.DoubleClick
		
		If lvwContacts.Items.Count > 0 Then

			m_lContactCnt = Convert.ToString(lvwContacts.FocusedItem.Tag)
		End If
		
		'cmdDeleteCon.Enabled = True
		'cmdEditCon.Enabled = True
		
	End Sub
	
	Private Sub lvwContacts_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.Enter
		'Check we are on correct tab
		If SSTabHelper.GetSelectedIndex(tabMainTab) <> 2 Then
			SSTabHelper.SetSelectedIndex(tabMainTab, 2)
		End If
	End Sub
	
	Private Sub lvwContacts_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContacts.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
		
		If lvwContacts.GetItemAt(x, y) Is Nothing Then
			cmdDeleteCon.Enabled = False
			cmdEditCon.Enabled = False
		Else
			'DC030401 check for view mode first ....
			'        cmdDeleteCon.Enabled = True
			'        cmdEditCon.Enabled = True
			If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
				cmdEditCon.Enabled = True
				cmdDeleteCon.Enabled = True
			End If
		End If
		
	End Sub
	
	Private Sub lvwConvictions_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwConvictions.Enter
		'Check we are on correct tab
		If SSTabHelper.GetSelectedIndex(tabMainTab) <> 3 Then
			SSTabHelper.SetSelectedIndex(tabMainTab, 3)
		End If
	End Sub
	
	Private Sub lvwConvictions_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwConvictions.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
        ' developer guide no. 70
      
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
		
		If lvwConvictions.GetItemAt(x, y) Is Nothing Then
			cmdDeleteConv.Enabled = False
			cmdEditConv.Enabled = False
		Else
			'DC030401 check for view mode first ....
			'        cmdDeleteConv.Enabled = True
			'        cmdEditConv.Enabled = True
			If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
				cmdEditConv.Enabled = True
				cmdDeleteConv.Enabled = True
			End If
		End If
		
	End Sub
	
	Private Sub lvwLoyaltySchemes_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwLoyaltySchemes.Enter
		'Check we are on correct tab
		If SSTabHelper.GetSelectedIndex(tabMainTab) <> 5 Then
			SSTabHelper.SetSelectedIndex(tabMainTab, 5)
		End If
	End Sub
	
	Private Sub lvwPolicies_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwPolicies.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwPolicies.Columns(eventArgs.Column)
		
		' Column click event for the campaigns
		
		Try 
			
			With lvwPolicies
				' If current sort column header is
				' pressed.
				If ListViewHelper.GetSortKeyProperty(lvwPolicies) = 4 Then
					ListViewHelper.SetSortKeyProperty(lvwPolicies, 2)
				End If
				
				If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwPolicies) Then
					' Set sort order opposite of
					' current direction.
					ListViewHelper.SetSortOrderProperty(lvwPolicies, (ListViewHelper.GetSortOrderProperty(lvwPolicies) + 1) Mod 2)
				Else
					' Sort by this column (ascending).
					ListViewHelper.SetSortedProperty(lvwPolicies, False)
					
					' Turn off sorting so that the list
					' is not sorted twice
					ListViewHelper.SetSortOrderProperty(lvwPolicies, SortOrder.Ascending)
					If ColumnHeader.Index + 1 = 2 Then
						ListViewHelper.SetSortKeyProperty(lvwPolicies, 4)
					Else
						ListViewHelper.SetSortKeyProperty(lvwPolicies, ColumnHeader.Index + 1 - 1)
					End If
					ListViewHelper.SetSortedProperty(lvwPolicies, True)
				End If
			End With
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPolicies_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwPolicies_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPolicies.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
		
		If lvwPolicies.GetItemAt(x, y) Is Nothing Then
			cmdDeletePol.Enabled = False
			cmdEditPol.Enabled = False
		Else
			'DC030401 check for view mode first ....
			'        cmdDeletePol.Enabled = True
			'        cmdEditPol.Enabled = True
			If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
				cmdEditPol.Enabled = True
				cmdDeletePol.Enabled = True
			End If
		End If
		
	End Sub
	
	Private Sub lvwLoyaltySchemes_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwLoyaltySchemes.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
      
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

		If lvwLoyaltySchemes.GetItemAt(x, y) Is Nothing Then
			' Nothing selected
			cmdEditLoyaltyScheme.Enabled = False
			cmdDeleteLoyaltyScheme.Enabled = False
		Else
			'Not if we're viewing...
			If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
				cmdEditLoyaltyScheme.Enabled = True
				cmdDeleteLoyaltyScheme.Enabled = True
			End If
		End If
		
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		Try 
			
			With tabMainTab
				
				' Set the default button.
				If SSTabHelper.GetSelectedIndex(tabMainTab) < cmdNext.Length Then
					If Not (cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)).FindForm() Is Nothing) Then
						VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)), True)
					End If
				Else
					'cmdOK.Default = True
				End If
				
				' Now I know this is crap, this goes against
				' all my principles, but for some reason when
				' using the mouse to select a tab the setfocus
				' code below doesn't work. The cursor sticks,
				' and you can't tab off. Therefore I've used
				' this to get around the problem.
				Application.DoEvents()
				
				' Set focus to the first control on the tab.
				If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(MainModule.ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
				End If
				
			End With
		
		Catch 
			
			
			
			' Error Section.
			
			
			tabMainTabPreviousTab = tabMainTab.SelectedIndex
		End Try
		
	End Sub
	
	Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_7.Click, _cmdNext_6.Click, _cmdNext_5.Click, _cmdNext_4.Click, _cmdNext_2.Click, _cmdNext_1.Click, _cmdNext_3.Click, _cmdNext_0.Click
		Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)
		
		Try 
			
			' Change to the next tab.
			If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
				SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
			End If
			
			' Set focus to the first control on the tab.
			If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(MainModule.ACControlStart, Index + 1).Focus()
			End If
		
		Catch 
			
			
			
			' Error Section
			
			Exit Sub
		End Try
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: CancelParty
	'
	' Description: Called when we wish to cancel any changes
	'
	' ***************************************************************** '
	Private Function CancelParty() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = ProcessCommand()
			
			' Check the return value.
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMTrue
			Else
				Return m_lReturn
			End If
		
		Catch 
		End Try
		
		
		
		' Error Section.
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the party", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: CancelClick
	'
	' Description:
	'
	' ***************************************************************** '
	Public Function CancelClick() As Integer
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If tabMainTab.Visible Then
				Return CancelParty()
			End If
			
			Return result
		
		Catch excep As System.Exception
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: OKClick
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Public Function OKClick() As Integer
		Dim result As Integer = 0
		Dim sText As String = ""
		Dim iInt As DialogResult
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim sMessage As String = ""
			
			If txtCCJ.Text.Trim() <> "" Then
				Dim dbNumericTemp As Double
				If Not Double.TryParse(txtCCJ.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
					MessageBox.Show("County court judgements must be a number", "Numeric Field", MessageBoxButtons.OK, MessageBoxIcon.Error)
					If txtCCJ.Visible Then
						SSTabHelper.SetSelectedIndex(tabMainTab, 4)
						txtCCJ.Focus()
						result = gPMConstants.PMEReturnCode.PMFalse
					End If
					Return result
				End If
			End If
			
			'DJM 19/08/2003 : Normal validation doesn't work on this combo field so bodge it.
			If m_bBusinessFieldOnClientIsMandatory Then
				If ddBusiness.Text.Trim().Length = 0 Then
					iInt = MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Business", MessageBoxButtons.OK, MessageBoxIcon.Error)
					SSTabHelper.SetSelectedIndex(tabMainTab, 0)
					ddBusiness.Focus()
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			If tabMainTab.Visible Then
				
				'MSS200901 - Added this UW section.
				m_lReturn = SaveParty()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return m_lReturn
				End If
            End If
			
			Return result
		
		Catch excep As System.Exception
			result = gPMConstants.PMEReturnCode.PMError
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
        End Try
			
	End Function
	
	'DC030401 allow editing of party
	' ***************************************************************** '
	' Name: EditClick
	'
	' Description:
	'
	' ***************************************************************** '
	Public Function EditClick() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_lReturn = LockParty()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private isInitializingComponent As Boolean

	Private Sub txtAgentRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentRef.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
        If txtAgentRef.Text.Trim.Length = 0 Then
            m_sAgentName = ""
            lblPnlAgentName.Text = ""
        End If
		m_bVerifyAgentCnt = True
	End Sub
	
	Private Sub txtAgentRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentRef.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgentRef)
	End Sub
	
	Private Sub txtAgentRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentRef.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgentRef)
        End If
        If txtAgentRef.Text.Trim.Length = 0 Then
            m_sAgentName = ""
            lblPnlAgentName.Text = ""
        End If
        chkAgent.Focus()
	End Sub
	
	Private Sub txtAgentReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentReference.Enter
		If SSTabHelper.GetTabVisible(tabMainTab, 6) Then
			SSTabHelper.SetSelectedIndex(tabMainTab, 6)
		End If
		
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgentReference)
		End If
	End Sub
	
	Private Sub txtAgentReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentReference.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgentReference)
		End If
	End Sub
	
	Private Sub txtBrokerRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBrokerRef.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		m_bVerifyBrokerCnt = True
	End Sub
	
	Private Sub txtBrokerRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBrokerRef.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtBrokerRef)
	End Sub
	
	Private Sub txtBrokerRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBrokerRef.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtBrokerRef)
	End Sub
	
	Private Sub txtCCJ_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCCJ.Enter
		If SSTabHelper.GetTabVisible(tabMainTab, 3) Then
			SSTabHelper.SetSelectedIndex(tabMainTab, 3)
		End If
	End Sub
	
	Private Sub txtCompanyReg_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCompanyReg.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCompanyReg)
	End Sub
	
	Private Sub txtCompanyReg_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCompanyReg.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCompanyReg)
        End If
        txtAlternativeIdentifier.Focus()
	End Sub
	
	Private Sub txtConsultantRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConsultantRef.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		m_bVerifyConsultantCnt = True
	End Sub
	
	Private Sub txtConsultantRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConsultantRef.Enter
		If SSTabHelper.GetTabVisible(tabMainTab, 0) Then
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
		End If
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtConsultantRef)
	End Sub
	
	Private Sub txtConsultantRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConsultantRef.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtConsultantRef)
        End If
        SSTabHelper.SetSelectedTabIndex(tabMainTab, 1)
	End Sub
	
	'MSS210901 - Added from UW
	Private Sub txtFinancialYear_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFinancialYear.Enter
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFinancialYear)
		
	End Sub
	
	Private Sub txtFinancialYear_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFinancialYear.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFinancialYear)
		End If
	End Sub
	'MSS210901 - Merge end
	
	Private Sub txtIDReference_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		'frmInterface.Caption = "Corporate Client: " & txtIDReference.Text & " " & m_sMainPostCode
	End Sub
	
	Private Sub txtIDReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.Enter
		
		' CTAF 220900
		' Make sure we're on the right tab incase this was called
		' from form controls.
		SSTabHelper.SetSelectedIndex(tabMainTab, 0)
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtIDReference)
		
	End Sub
	
	Private Sub txtIDReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtIDReference)
		End If
	End Sub
	
	Private Sub txtInsurerRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerRef.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		m_bVerifyInsurerCnt = True
	End Sub
	
	Private Sub txtInsurerRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerRef.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInsurerRef)
	End Sub
	
	Private Sub txtInsurerRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerRef.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInsurerRef)
		End If
	End Sub
	
    'developer guide no. 78
    Private Sub txtMainContact_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtMainContact.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		RestrictChars(KeyAscii, True)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub txtName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Enter
		
		' CTAF 220900
		' Make sure we're on the right tab incase this was called
		' from form controls.
		SSTabHelper.SetSelectedIndex(tabMainTab, 0)
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtName)
		
	End Sub
	
    'developer guide no. 78
    Private Sub txtName_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtName.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		'**************************************************************************
		'MKR 12/10/2004 PN 6021 -- Allowing "," in Trading Name (& Resolved Name)
		'    If KeyAscii <> 44 Then
		'        Call RestrictChars(KeyAscii, True)
		'    End If
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub txtName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Leave
		
		'DJM 09/05/2002 : Trading name is no longer forced into proper case.
		' CTAF 180200 - Format trading name
		'txtName.Text = StrConv(txtName.Text, vbProperCase)
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtName)
		End If
	End Sub
	
	Private Sub txtMainContact_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMainContact.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMainContact)
	End Sub
	
	Private Sub txtMainContact_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMainContact.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMainContact)
		End If
	End Sub

    Private Sub txtOffices_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtOffices.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If (KeyAscii < 48 Or KeyAscii > 57) And KeyAscii <> 8 Then
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

	Private Sub txtOffices_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOffices.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtOffices)
	End Sub
	
	Private Sub txtOffices_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOffices.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtOffices)
		End If
	End Sub
	
	Private Sub txtSalutation_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSalutation.Enter
		If SSTabHelper.GetTabVisible(tabMainTab, 1) Then
			SSTabHelper.SetSelectedIndex(tabMainTab, 1)
		End If
	End Sub
	
	Private Sub txtSource_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSource.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSource)
	End Sub
	
	Private Sub txtSource_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSource.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSource)
	End Sub
	
	Private Sub txtTobLetter_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTobLetter.Enter
		
		SSTabHelper.SetSelectedIndex(tabMainTab, 4)
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTobLetter)
		End If
		
	End Sub
	
	Private Sub txtTobLetter_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTobLetter.Leave
		'PN16761
		If Not (m_oFormFields Is Nothing) Then
			
			If txtTobLetter.Text <> "" Then
				m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTobLetter)
			End If
		End If
		
		
	End Sub
	
	Private Sub txtTPPassword_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTPPassword.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTPPassword)
	End Sub
	
	Private Sub txtTPPassword_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTPPassword.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTPPassword)
	End Sub
	
	Private Sub txtTradingSince_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTradingSince.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTradingSince)
	End Sub
	
	Private Sub txtTradingSince_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTradingSince.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTradingSince)
		End If
	End Sub
	
	Private Sub txtWageRoll_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtWageRoll.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtWageRoll)
	End Sub
	
	Private Sub txtWageRoll_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtWageRoll.Leave
		If Not (m_oFormFields Is Nothing) Then
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtWageRoll)
		End If
	End Sub
	
	' PRIVATE Events (End)
	
	' ***************************************************************** '
	' Name: LoadControl
	'
	' Description:
	'
	' ***************************************************************** '
	Public Function LoadControl() As Integer
		
		Dim result As Integer = 0
		Dim sMessage, sTitle As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Forms initialise event.
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyCC.Business", "ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                'developer guide no. 243
                sTitle = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACBusinessFailTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'developer guide no. 243
                sMessage = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACBusinessFail, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Return result
			End If
			
			'SD 22/08/2002 Query if agency or underwriting

			m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency
			
			' Create an instance of the form control object.
			m_oFormFields = New iPMFormControl.FormFields()
			
			' Set language
            m_oFormFields.LanguageID = MainModule.g_iLanguageID
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the process modes for the busines object.

			m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				
				Return result
			End If
			
			' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - get 'read trade' system option

			m_lReturn = m_oBusiness.GetReadTradeSysOptions(r_bReadTradeABIEnabled:=m_bReadTradeABIList)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read system options", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the business keys.
			' {* USER DEFINED CODE (Begin) *}

			m_oBusiness.PartyCnt = m_lPartyCnt
			' {* USER DEFINED CODE (End) *}
			If m_lPartyCnt > 0 Then
				m_lReturn = LogClientViewedEvent(m_lPartyCnt)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error but continue processing
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to log client viewed event", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")
				End If
			End If
			' Validate fields using Forms Control
			m_lReturn = SetFieldValidation()

			' Set the interface default values.
			m_lReturn = SetInterfaceDefaults()

			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

				Return result
			End If


			m_lReturn = PMBGeneralFunc.g_oListManager.CheckListVersions()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Faied to check list manager versions.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			End If

			'If adding, still need to get address types for populating
			'the combo box cells in the grid control
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

				'Get addresse type lookups for the party

				m_lReturn = m_oBusiness.GetAddresstypelookups(vaddresstypes:=m_vAddressTypes)

				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

					' Log Error.
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				End If

				'Set the index of the main address
				For i As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)

					'See if this is the main address
					If CStr(m_vAddressTypes(2, i)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
						m_iMainAddressIndex = CInt(m_vAddressTypes(0, i))
						Exit For
					End If

				Next i

			End If

			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

			Return result

		Catch excep As System.Exception



			result = gPMConstants.PMEReturnCode.PMError

			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

			Return result

		End Try
	End Function

	Private Function LogClientViewedEvent(lPartyCnt As Integer) As Integer
		Dim result As Integer = 0
		Try
			result = gPMConstants.PMEReturnCode.PMTrue

			Dim oPartyBusiness As bSIRParty.Business

			' we need bSIRParty.Business
			Dim temp_oPartyBusiness As Object = Nothing
			m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_oPartyBusiness, "bSIRParty.Business", gPMConstants.PMGetViaClientManager)
			oPartyBusiness = temp_oPartyBusiness

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="LogClientViewedEvent")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			m_lReturn = oPartyBusiness.LogClientViewedEvent(lPartyCnt)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to log client viewed event", vApp:=ACApp, vClass:=ACClass, vMethod:="LogClientViewedEvent")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			Return result

		Catch excep As System.Exception
			result = gPMConstants.PMEReturnCode.PMError

			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LogClientViewedEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LogClientViewedEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

			Return result

		End Try
	End Function

	' ***************************************************************** '
	' Name: SaveProspectPolicies
	'
	' Description: This deletes all the prospect policy records then recreates
	' them based on what's in the array
	'
	' ***************************************************************** '
	Private Function SaveProspectPolicies() As Integer

		Dim result As Integer = 0
		Dim i As Integer
		Dim bFirst As Boolean
		Dim oListItem As ListViewItem
		Dim vArray(,) As Object
		'DC 27/09/00
		Dim cPremium As Decimal

		Try

			result = gPMConstants.PMEReturnCode.PMTrue

			'Loop round the listview
			i = 1
			bFirst = True

			'developer guide no. 146
			vArray = Nothing

			Do
				If i > lvwPolicies.Items.Count Then
					Exit Do
				End If

				oListItem = lvwPolicies.Items.Item(i - 1)

				If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
					Exit Do
				Else
					If bFirst Then
						ReDim vArray(3, i - 1)
						bFirst = False
					Else
						ReDim Preserve vArray(3, i - 1)
					End If

					'Type


					vArray(0, i - 1) = Convert.ToString(oListItem.Tag)
					'Renewal Date
					txtHiddenDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 1).Text


					vArray(1, i - 1) = m_oFormFields.UnformatControl(txtHiddenDate)
					'No Of Times Quoted

					vArray(2, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text
					'Target Premium
					'DC 27/09/00 chnage way premium is unformatted
					'txtHiddenCurrency.Text = oListItem.SubItems(3)
					cPremium = CDec(ListViewHelper.GetListViewSubItem(oListItem, 3).Text)
					txtHiddenCurrency.Text = CStr(cPremium)
					'DC


					vArray(3, i - 1) = m_oFormFields.UnformatControl(txtHiddenCurrency)

				End If
				i += 1
			Loop

			'Delete old policies from database

			m_lReturn = m_oProspect.DeletePolicies(vPartyCnt:=m_lPartyCnt)

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			'Add new policies to database
			If Information.IsArray(vArray) Then

				m_lReturn = m_oProspect.AddPolicies(vPartyCnt:=m_lPartyCnt, vPolicies:=vArray)

				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If

			vArray = Nothing

			Return result

		Catch excep As System.Exception



			result = gPMConstants.PMEReturnCode.PMError

			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveProspectPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveProspectPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

			Return result

		End Try
	End Function

	' ***************************************************************** '
	'
	' Name: txtAlternativeIdentifier_LostFocus
	'
	' Description:
	'
	' History: 12/06/2002 SJ - Created.
	'
	' ***************************************************************** '
	Private Sub txtAlternativeIdentifier_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAlternativeIdentifier.Leave
		
		Try 
			
			Dim sMessage As String = ""
			
			If m_bIsNRMA Then
				If txtAlternativeIdentifier.Text <> PartyFunc.m_kBlankAlternativeIdentifier Then
					PartyFunc.AlternativeIdentifierLostFocus(v_oAlternativeIdentifier:=txtAlternativeIdentifier, v_sAlternativeIdentifierScript:=m_sAlternativeIdentifierScript, v_iTask:=Task)
				End If
			Else
				PartyFunc.AlternativeIdentifierLostFocus(v_oAlternativeIdentifier:=txtAlternativeIdentifier, v_sAlternativeIdentifierScript:=m_sAlternativeIdentifierScript, v_iTask:=Task)
			End If
		
		Catch excep As System.Exception
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtAlternativeIdentifier_LostFocus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtAlternativeIdentifier_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	' ***************************************************************** '
	'
	' Name: txtAlternativeIdentifier_Change
	'
	' Description:
	'
	' History: 12/06/2002 SJ - Created.
	'
	' ***************************************************************** '
	Private Sub txtAlternativeIdentifier_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAlternativeIdentifier.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Try 
			
			If m_bIsNRMA And m_bUserMode Then
				PartyFunc.AlternativeIdentifierChange(r_oAlternativeIdentifier:=txtAlternativeIdentifier)
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtAlternativeIdentifier_Change Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtAlternativeIdentifier_Change", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: txtAlternativeIdentifier_KeyPress
	'
	' Description:
	'
	' History: 12/06/2002 SJ - Created.
	'
	' ***************************************************************** '
    'developer guide no. 78
    Private Sub txtAlternativeIdentifier_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtAlternativeIdentifier.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		Try 
			
			If m_bIsNRMA Then
				PartyFunc.StopNonNumericCharacters(r_iKeyAscii:=KeyAscii)
			End If
			
			If KeyAscii = 0 Then
				eventArgs.Handled = True
			End If
			Exit Sub
		
		Catch 
		End Try
		
		
		
		
		' Log Error Message
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtAlternativeIdentifier_KeyPress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtAlternativeIdentifier_KeyPress", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		Exit Sub
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	Private Sub txtAlternativeIdentifier_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAlternativeIdentifier.Enter
		If m_bIsNRMA Then
			txtAlternativeIdentifier.SelectionStart = Strings.Len(txtAlternativeIdentifier.Text)
		End If
	End Sub
	
	' ***************************************************************** '
	'
	' Name: txtLoyaltyNumber_KeyPress
	'
	' Description:
	'
	' History: 12/06/2002 SJ - Created.
	'
	' ***************************************************************** '
    'developer guide no. 78
    Private Sub txtLoyaltyNumber_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtLoyaltyNumber.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		Try 
			
			If m_bIsNRMA Then
				PartyFunc.StopNonNumericCharacters(r_iKeyAscii:=KeyAscii)
			End If
			
			If KeyAscii = 0 Then
				eventArgs.Handled = True
			End If
			Exit Sub
		
		Catch 
		End Try
		
		
		
		
		' Log Error Message
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtLoyaltyNumber_KeyPress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtLoyaltyNumber_KeyPress", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		Exit Sub
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	' ***************************************************************** '
	'
	' Name: txtLoyaltyNumber_LostFocus
	'
	' Description:
	'
	' History: 12/06/2002 SJ - Created.
	'
	' ***************************************************************** '
	Private Sub txtLoyaltyNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLoyaltyNumber.Leave
		
		Try 
			
			Dim sMessage As String = ""
			
			'sj 25/07/2002 - start
			If m_bIsNRMA Then
				PartyFunc.LoyaltyNumberLostFocus(v_oNewLoyaltyNumber:=txtLoyaltyNumber, v_sOldLoyaltyNumber:=m_sLoyaltyNumber, v_sLoyaltyNumberScript:=m_sLoyaltyNumberScript, v_iTask:=Task)
			Else
				'MKW260903 PN7071 No Validation on Broking.
				If m_sUnderwritingOrAgency <> "A" Then
					PartyFunc.LoyaltyNumberLostFocus(v_oNewLoyaltyNumber:=txtLoyaltyNumber, v_sLoyaltyNumberScript:=m_sLoyaltyNumberScript, v_iTask:=Task)
				End If
			End If
			'sj 25/07/2002 - end
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtLoyaltyNumber_LostFocus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtLoyaltyNumber_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
		
	End Sub
	
	Private Sub UserControl_InitProperties()
		m_BackColor = m_def_BackColor
		m_ForeColor = m_def_ForeColor
		m_Enabled = m_def_Enabled

        'developer guide no. 3(no solution)
        m_Font = MyBase.Font
		m_BackStyle = m_def_BackStyle
		m_BorderStyle = m_def_BorderStyle
        m_PartyCnt = m_def_PartyCnt

        FillCombo()
	End Sub
	
	Private Sub uctPartyCCControl_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		Dim iCtrlDown As Integer
		
		Const ACCtrlMask As Integer = 2
		
		Try 
			
			' Set the control key value.
			iCtrlDown = (Shift And ACCtrlMask) > 0
			
			With tabMainTab
				' Check the key pressed.
				Select Case KeyCode
					Case Keys.PageUp
						' Page Up key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Display the first tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, 0)
						Else
							' Check we are not on the
							' first tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
								' Display the previous tab.
								SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
							End If
						End If
						
					Case Keys.PageDown
						' Page Down key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Display the last tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
						Else
							' Check we are not on the
							' last tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
								' Display the next tab.
								SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
							End If
						End If
						
					Case Keys.Home
						' Home key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Set focus the the start control on
							' the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(MainModule.ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
						
					Case Keys.End
						' End key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Set focus the the start control on
							' the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(MainModule.ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
				End Select
			End With
		
		Catch 
			
			
			
			' Error Section.
			
			Exit Sub
		End Try
		
		
    End Sub

    Private Sub uctPartyCCControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'added following line to hide "Prospect Policy" tab against issue no 1730
        'Made following tab hidden as per 'Stephen Ross' mail
        Me.tabMainTab.TabPages.RemoveAt(6)
    End Sub
	
	Private Sub uctPartyCCControl_Paint(ByVal eventSender As Object, ByVal eventArgs As PaintEventArgs) Handles MyBase.Paint
		Static bDoNotSetFocus As Boolean
		
		If Not DesignMode Then
			If Not bDoNotSetFocus Then
				If txtIDReference.Enabled Then
					txtIDReference.Focus()
				ElseIf txtName.Enabled Then 
					txtName.Focus()
				End If
				bDoNotSetFocus = True
			End If
		End If
	End Sub
	
	'Load property values from storage


    'developer guide no. 1(no solution)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
		


		m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


		m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


		m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


        'developer guide no. 3(no solution)
        m_Font = PropBag.ReadProperty("Font", MyBase.Font)


		m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


		m_BorderStyle = CInt(PropBag.ReadProperty("BorderStyle", m_def_BorderStyle))


		m_PartyCnt = CInt(PropBag.ReadProperty("PartyCnt", m_def_PartyCnt))
		
	End Sub
	
	Private Sub uctPartyCCControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		
		' Maintain minimum width
		If VB6.PixelsToTwipsX(Width) < 825 Then Width = VB6.TwipsToPixelsX(9825)
		' and minimum height
		If VB6.PixelsToTwipsY(Height) < 6720 Then Height = VB6.TwipsToPixelsY(6720)
		
	End Sub
	
    'developer guide no. 1(no solution)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
		

		PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

		PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

		PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)


        'developer guide no. 3(no solution)
        PropBag.WriteProperty("Font", m_Font, MyBase.Font)

		PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

		PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

		PropBag.WriteProperty("PartyCnt", m_PartyCnt, m_def_PartyCnt)
	End Sub
	
	'MSS200901 - Added from UW.
	
	' ***************************************************************** '
	' Name: SaveParty
	'
	' Description: Saves the displayed party details
	'
	' ***************************************************************** '
	Private Function SaveParty() As Integer
		
		Dim result As Integer = 0
		Dim sSelectedClientCode As String = ""
		Dim lSelectedClientPartyCnt As Integer
		Dim iOKAction As Integer
		
		Dim bDuplicateFound As Boolean
		Dim sClientcode As String = ""
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		' Check mandatory controls have been entered into.
		m_lReturn = CheckMainMandatoryControls()
		
		' Check for errors
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		Dim sOriginalReference As String = txtIDReference.Text
		
		If txtIDReference.Text = "" Then
			m_lReturn = GenerateClientCode(v_sName:=txtName.Text, r_sOriginalCode:=sOriginalReference, r_sClientCode:=sClientcode, r_bDuplicateFound:=bDuplicateFound)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
				Return gPMConstants.PMEReturnCode.PMFalse
			ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then 
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GenerateClientCode", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick")
				
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			txtIDReference.Text = sClientcode
		End If
		
		
		'Duplicate Client Identification
		If m_bDuplicateClientIdentification Then
			If sOriginalReference <> txtIDReference.Text Then
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				'Show Duplicate Client interface
				m_lReturn = ShowDuplicateParty(sOriginalClientCode:=sOriginalReference, sUniqueClientCode:=txtIDReference.Text, sSelectedClientCode:=sSelectedClientCode, lSelectedClientPartyCnt:=lSelectedClientPartyCnt, iOKAction:=iOKAction)
				
				If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
					txtIDReference.Text = ""
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				If m_lReturn = gPMConstants.PMEReturnCode.PMOK Then
                    If iOKAction = MainModule.kAbandonNewRecordandUseSelectedClient Then
						'Open Selected Client in Edit Mode
						Me.ShortName = sSelectedClientCode
						Me.PartyCnt = lSelectedClientPartyCnt

						m_oBusiness.PartyCnt = lSelectedClientPartyCnt
						Return gPMConstants.PMEReturnCode.PMTrue
					End If
				End If
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			End If
		End If
		
		
		'Validate some address stuff
		m_lReturn = ValidateOK()
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		
		' Process the next set of actions depending
		' upon the interface task etc.
		m_lReturn = ProcessCommand()
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		'update the party cnt property

		m_lPartyCnt = m_oBusiness.PartyCnt
		
		' save additional details back to party record.
		m_lReturn = UpdatePartyDetails()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save party details data.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			Return result
		End If
		
		'Update party addresses
		m_lReturn = UpdateAddresses()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Address Details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")
			Return result
		End If
		
		'Update party contacts
		m_lReturn = UpdateContacts()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")
			Return result
		End If
		
		'DC 03/05/00
		'Update party associates
		m_lReturn = UpdateAssociates()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Associate Details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")
			Return result
		End If
		
		'DC 04/08/00
		'Update main contact
		m_lReturn = UpdateMainContact()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Main Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")
			Return result
		End If
		
		'2005 Roadmap changes
		m_lReturn = SaveProspect()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save prospect data.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		End If
		
		If m_bChangedProspectPolicies Then
			m_lReturn = SaveProspectPolicies()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return m_lReturn
			End If
		End If
		
		'AR20050214 - PN18407
		m_lReturn = UpdateOrion(vPartyCnt:=m_lPartyCnt)
		
		' Check for errors.
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the Orion details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")
			
			Return result
			
		End If
		'PSL 13/08/2003 Issue 5856
		'Moved create account her, because the code uses the party object which requires
		'the address to be saved

		m_lReturn = UpdateOrion(vPartyCnt:=m_oBusiness.PartyCnt)
		' Check for errors.
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Failed to add the details
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the Orion details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
		End If
		
		'sj 23/07/2002 - start
		If m_bFutureDateAddressChanges Then

			m_lReturn = m_oBusiness.CreateFutureDatedAddresses(v_vFutureDatedAddresses:=m_vFutureDatedAddresses, v_lPartyCnt:=m_lPartyCnt)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")
				Return result
			End If
		End If
		'sj 23/07/2002 - end
		
		UpdateAddressPostCodeProperties()
		
		'Party Bank Details
        'developer guide no. 9
        m_lReturn = uctPartyBankControl1.Initialise()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError("SaveParty", "uctPartyBankControl1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		uctPartyBankControl1.PartyCnt = m_lPartyCnt
		m_lReturn = uctPartyBankControl1.UpdatePartyBankDetails()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError("SaveParty", "uctPartyBankControl1.UpdatePartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		Return result
		
		
		
		' Error Section.
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the party", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: GenerateClientCode
	'
	' Description:
	'
	' History: 23/08/2002 SJ - Created.
	'
	' ***************************************************************** '
	Private Function GenerateClientCode(ByVal v_sName As String, ByRef r_sOriginalCode As String, ByRef r_sClientCode As String, ByRef r_bDuplicateFound As Boolean) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            Dim vArray() As Object
			Dim sText As String = ""
			Dim sName As New StringBuilder
			Dim iMax As Integer
			Dim iPartyFound As gPMConstants.PMEReturnCode
			Dim sOriginalReference As String = ""
			Dim lPartyCnt As Integer
			Dim iNumber As Integer
			Dim sClientcode As String = ""
			
			'Set maximum length of name
			If m_sUnderwritingOrAgency = "U" Then
				iMax = 16
			Else
				iMax = 9
			End If
			
			ReDim vArray(0)
			sText = v_sName
			
			While sText.Length > 0

				ReDim Preserve vArray(vArray.GetUpperBound(0) + 1)
				If sText.IndexOf(" "c) > 0 Then


                    vArray(vArray.GetUpperBound(0)) = sText.Substring(0, sText.IndexOf(" "c)).ToUpper()
					sText = Mid(sText, (sText.IndexOf(" "c) + 1) + 1)
				Else


                    vArray(vArray.GetUpperBound(0)) = sText.ToUpper()
					sText = ""
				End If
			End While
			

			If vArray.GetUpperBound(0) > 0 Then

				For iInt As Integer = 1 To vArray.GetUpperBound(0)

                    sText = CStr(vArray(iInt))
					If sText = "LTD." Or sText = "LTD" Or sText = "LIMITED" Or sText = "CO." Or sText = "CO" Or sText = "COMPANY" Or sText = "PLC." Or sText = "PLC" Or sText = "&" Or sText = "SON" Or sText = "SONS" Or sText = "DAUGHTER" Or sText = "DAUGHTERS" Or sText = "ASSOC." Or sText = "ASSOC" Or sText = "ASSOCIATION" Or sText = "CLUB" Then
						'Do nothing
					Else
						sName.Append(sText)
					End If
					
				Next iInt
			End If
			
			'Remove illegal characters
			sText = sName.ToString()
			sName = New StringBuilder("")
			For iInt As Integer = 1 To sText.Length
				If sText.Substring(iInt - 1, 1) = " " Or sText.Substring(iInt - 1, 1) = "'" Or sText.Substring(iInt - 1, 1) = "|" Or sText.Substring(iInt - 1, 1) = "," Then
				Else
					sName.Append(sText.Substring(iInt - 1, 1))
				End If
			Next iInt
			
			If m_sUnderwritingOrAgency = "U" Then
				
				m_lReturn = ValidateNumberingScheme()
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return result
				End If
				
				'Generate Client Code
				m_lReturn = GetClientCode(r_sClientCode:=sClientcode, v_sTradingName:=sName.ToString())
				If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sClientcode = "") Then
					Return m_lReturn
				Else
					sName = New StringBuilder(sClientcode.Trim())
				End If
				
			Else
				'Make the name the correct length for Underwriting(16) or Broking(9)
				If sName.ToString().Length > iMax Then
					sName = New StringBuilder(sName.ToString().Substring(0, iMax))
				End If
			End If
			
			If m_sUnderwritingOrAgency = "U" Then
				iNumber = 1
			Else
				iNumber = 0
			End If
			
			iPartyFound = gPMConstants.PMEReturnCode.PMTrue
			r_bDuplicateFound = False
			r_sOriginalCode = sName.ToString()
			sOriginalReference = sName.ToString()
			
			Do While iPartyFound = gPMConstants.PMEReturnCode.PMTrue
				

				m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=sName.ToString(), vPartyCnt:=lPartyCnt)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				If lPartyCnt <> 0 Then
					iNumber += 1
					If m_sUnderwritingOrAgency = "U" Then
						If iNumber > 999 Then
                            MessageBox.Show("Client code counter exceeds 999." & Strings.Chr(13) & Strings.Chr(10) &
							                "You must manually enter a unique client code.", Application.ProductName)
							iPartyFound = gPMConstants.PMEReturnCode.PMFalse
							txtIDReference.Enabled = True
						Else
							sName = New StringBuilder(sOriginalReference & StringsHelper.Format(CStr(iNumber), "000"))
						End If
					Else
						sName = New StringBuilder(sOriginalReference & CStr(iNumber))
					End If
					r_bDuplicateFound = True
				Else
					iPartyFound = gPMConstants.PMEReturnCode.PMFalse
				End If
			Loop 
			
			r_sClientCode = sName.ToString()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateClientCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateClientCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: GetDefaultAgentDetails
	'
	' Description:
	'
	' History: 17/06/2002 SJ - Created.
	'
	' ***************************************************************** '
	Private Function GetDefaultAgentDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim lPartyCnt As Integer
			
			'See if there is an agent allocated to this user

			m_lReturn = m_oPMUser.GetDetails(vUserId:=g_iUserId)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMError
				
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPMUser.GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultAgentDetails")
				Return result
			End If
			

			m_lReturn = m_oPMUser.GetNext(vPartyCnt:=lPartyCnt)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMError
				
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPMUser.GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultAgentDetails")
				Return result
			End If
			
			If lPartyCnt = 0 Then
				' No agent allocated to this user so exit here
				Return result
			End If
			
			m_lAgentCnt = lPartyCnt
			
			'Get the agent cnt, name and ref from the "core" party table

			m_lReturn = m_oBusiness.GetCorePartyDetails(vPartyCnt:=m_lAgentCnt, vShortName:=m_sAgentRef, vName:=m_sAgentName)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMError
				
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetCorePartyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultAgentDetails")
				Return result
			End If
			
			m_sAgentRef = m_sAgentRef.Trim()
			m_sAgentName = m_sAgentName.Trim()
			
			'Update controls on the form
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentRef, vControlValue:=m_sAgentRef)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMError
			End If
			

            'developer guide no. 26
            pnlAgentName.Name = m_sAgentName
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultAgentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultAgentDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetSourceBaseCurrency
	'
	' Description: Set base currency when user changes source
	'
	' History: 20042004 RDC created
	'
	' ***************************************************************** '
	Private Function GetSourceBaseCurrency() As Integer
		Dim result As Integer = 0
		Dim iIndex, iBaseCurrencyID As Integer
		Dim lSourceId As Integer

		Dim oPartyBusiness As bSIRParty.Business
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iIndex = cboBranch.SelectedIndex
			
			If iIndex = -1 Then
				' no branch selected
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			' we need bSIRParty.Business
			Dim temp_oPartyBusiness As Object
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_oPartyBusiness, "bSIRParty.Business", gPMConstants.PMGetViaClientManager)
			oPartyBusiness = temp_oPartyBusiness
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceBaseCurrency")
				
				Return result
			End If
			
			' get the SourceID
			lSourceId = VB6.GetItemData(cboBranch, iIndex)
			
			' this value SHOULD exist, but more error trapping here?
			
			' call the business. to get the Base Currency ID

            m_lReturn = oPartyBusiness.GetBaseCurrencyID(lSourceID:=lSourceId, iCurrencyID:=iBaseCurrencyID)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get base currency for SourceID " & lSourceId, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceBaseCurrency")
				
				Return result
			End If
			
			cboCurrency.CompanyId = lSourceId
			cboCurrency.RefreshList()
			cboCurrency.CurrencyId = iBaseCurrencyID
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSourceBaseCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceBaseCurrency", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	' PRIVATE Events (End)
	
	
	' ***************************************************************** '
	'
	' Name: ShowDuplicateParty
	'
	' Description:
	'
	' History: 24/01/2005 RKS - Created.
	'
	' ***************************************************************** '
	Private Function ShowDuplicateParty(ByVal sOriginalClientCode As String, ByVal sUniqueClientCode As String, ByRef sSelectedClientCode As String, ByRef lSelectedClientPartyCnt As Integer, ByRef iOKAction As Integer) As Integer
		Dim result As Integer = 0

        'developer guide no. 108
        Dim oPMBPartyDuplicate As iPMBPartyDuplicate.Interface_Renamed
		Dim vDuplicateClientData As Object
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			

			m_lReturn = m_oBusiness.CheckDuplicateShortName(v_sShortName:=sOriginalClientCode, v_vMatchArray:=vDuplicateClientData)
			'Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get CheckDuplicateShortName", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDuplicateParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			'Get an instance of the Duplicate Client interface object via
			'the public object manager.
			Dim temp_oPMBPartyDuplicate As Object
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_oPMBPartyDuplicate, "iPMBPartyDuplicate.Interface_Renamed", gPMConstants.PMGetLocalInterface)
			oPMBPartyDuplicate = temp_oPMBPartyDuplicate
			
			'Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Duplicate Party Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDuplicateParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = oPMBPartyDuplicate.Initialise
			'Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initiliase Duplicate Party Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDuplicateParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			


			oPMBPartyDuplicate.ClientData = vDuplicateClientData

			oPMBPartyDuplicate.PartyTypeCode = gSIRLibrary.SIRPartyTypeCorporateClient

			oPMBPartyDuplicate.OriginalClientCode = sOriginalClientCode

			oPMBPartyDuplicate.UniqueClientCode = sUniqueClientCode
			

			oPMBPartyDuplicate.Start()
			
			

			iOKAction = oPMBPartyDuplicate.OKAction

			sSelectedClientCode = oPMBPartyDuplicate.SelectedClientCode

			lSelectedClientPartyCnt = oPMBPartyDuplicate.SelectedClientPartyCnt
			

			result = oPMBPartyDuplicate.Status
			

            oPMBPartyDuplicate.Dispose()
			oPMBPartyDuplicate = Nothing
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowDuplicateParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDuplicateParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Function CheckMainMandatoryControls() As Integer
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_oFormFields.Item("txtName-0").IsMandatory Then
				If txtName.Text.Trim().Length = 0 Then
					MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Trading Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
					'sj 18/06/2002 - start
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
					If txtName.Visible Then
                        txtName.Focus()
                    End If
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'sj 18/06/2002 - end
                    Return result
                Else
                    If Not IsValidString(txtName.Text) Then
                        MessageBox.Show("Trading Name can't contain any of the following characters. " & vbNewLine & ":~ "" # % & * : < > ? / \ { } | • ‣ ◦ ⁃", "Invalid Input - Trading Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
						SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        If txtName.Visible Then
						txtName.Focus()
                        End If
						result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
					End If
				End If
            End If

            Dim sOptionValue As String = String.Empty
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOptionValue, v_iSourceID:=g_oObjectManager.SourceID)

            If Not String.IsNullOrEmpty(sOptionValue) AndAlso sOptionValue = "2" Then
            If txtIDReference.Text <> "" Then
                If IsValidString(txtIDReference.Text) = False Then
                    MessageBox.Show("Client Code can't contain any of the following characters. " & vbNewLine & ":~ "" # % & * : < > ? / \ { } |", "Mandatory Field - Trading Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'sj 18/06/2002 - start
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    If txtIDReference.Visible Then
                        txtIDReference.Focus()
                        End If
                        result = gPMConstants.PMEReturnCode.PMFalse
                    'sj 18/06/2002 - end
                    Return result
                End If
            End If
            End If
			
            If Not m_bAONPRClientScreenChanges Then
                If m_oFormFields.Item("cboEmployees-0").IsMandatory Then 'PN22533
                    If cboEmployees.ListIndex = -1 Then
                        MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - No. of Employees", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        'developer guide no. 222(latest guide)
                        Dim selIndex As Integer = 0
                        selIndex = SSTabHelper.GetSelectedIndex(tabMainTab)
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        If cboEmployees.Visible Then
                            cboEmployees.Focus()
                        Else
                            SSTabHelper.SetSelectedIndex(tabMainTab, selIndex)
                        End If
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If

                If m_oFormFields.Item("cboTurnover-0").IsMandatory Then 'PN22533
                    If cboTurnover.ListIndex = -1 Then
                        MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Turnover", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        'developer guide no. 222(latest guide)
                        Dim selIndex As Integer = 0
                        selIndex = SSTabHelper.GetSelectedIndex(tabMainTab)
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        If cboTurnover.Visible Then
                            cboTurnover.Focus()
                        Else
                            SSTabHelper.SetSelectedIndex(tabMainTab, selIndex)
                        End If
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMainMandatoryControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMainMandatoryControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Public Function UpdateTurnoverDetails() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "UpdateTurnoverDetails"
		
		Dim sFormattedCurrency As String = ""
		Dim cConvertedAmount As Decimal
        Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		'Get Client Account Details

		m_lReturn = m_oAccount.GetClientAccountDetails(v_lAccountKey:=m_lPartyCnt, v_lCompanyID:=m_iPartySourceId, r_curYearToDateTurnover:=m_cYearToDateTurnover, r_curLastYearTurnover:=m_cLastYearTurnover, r_curClientBalance:=m_cClientBalance)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError("m_oAccount.GetClientAccountDetails", "v_lAccountKey = " & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
		End If
		
		

		m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iCurrencyId, lCompanyID:=g_iSourceID, cBaseAmount:=m_cClientBalance, cCurrencyAmount:=cConvertedAmount)
		
		m_cClientBalance = cConvertedAmount
		
		'Update the interface with the turnover details

		m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cClientBalance, vFormattedCurrency:=sFormattedCurrency)

        'developer guide no. 26
        'pnlClientBalance.Name = sFormattedCurrency
        pnlClient.Text = sFormattedCurrency
		

		m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cYearToDateTurnover, vFormattedCurrency:=sFormattedCurrency)

        'developer guide no. 26
        'pnlYearToDateTurnover.Name = sFormattedCurrency
        lblYearToDate.Text = sFormattedCurrency
		

		m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cLastYearTurnover, vFormattedCurrency:=sFormattedCurrency)

        'developer guide no. 26
        'pnlLastYearTurnover.Name = sFormattedCurrency
        pnlLastYear.Text = sFormattedCurrency
		
		
        Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
        Finally
		
		' Do any tidy up, e.g. Set x = Nothing here
		
        '        Return result
		
		' This is for debugging only
        '        Resume
		
        '        Return result
        End Try
		Return result
	End Function
	
	
	
	' ***************************************************************** '
	' Name: GetPartyDetails
	'
	' Parameters: n/a
	'
	' Description: Returns additional party details from party record
	'
	' History:
	'           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
	' ***************************************************************** '
	Private Function GetPartyDetails() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "GetPartyDetails"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim vPartyDetails As Object
		
		
		
        Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		

		lReturn = m_oBusiness.GetPartyDetails(v_lPartyCnt:=m_lPartyCnt, r_vResults:=vPartyDetails)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		If Information.IsArray(vPartyDetails) Then
			

            m_sTaxNumber = vPartyDetails(MainModule.kPartyDetailTaxNumber, 0)
            m_bDomiciledForTax = gPMFunctions.ToSafeBoolean(vPartyDetails(MainModule.kPartyDetailDomiciledForTax, 0), 0)
            m_bTaxExempt = gPMFunctions.ToSafeBoolean(vPartyDetails(MainModule.kPartyDetailTaxExempt, 0), 0)
            m_dTaxPercentage = gPMFunctions.ToSafeDouble(vPartyDetails(MainModule.kPartyDetailTaxPercentage, 0), 0)
            m_vBlackListReasonId = gPMFunctions.ToSafeLong(vPartyDetails(MainModule.kPartyDetailBlackListReasonId, 0), 0)
			
		End If
		

		
        Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
        Finally



		
        End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: UpdatePartyDetails
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
	' ***************************************************************** '
	Private Function UpdatePartyDetails() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "UpdatePartyDetails"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim vPartyDetails As Object
		
        Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		

		lReturn = CType(BuildPartyDetailArray(r_vPartyDetails:=vPartyDetails), gPMConstants.PMEReturnCode)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "BuildPartyDetailArray Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' update party details

		lReturn = m_oBusiness.UpdatePartyDetails(v_lPartyCnt:=m_lPartyCnt, v_vPartyDetails:=vPartyDetails)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "UpdatePartyDetails", gPMConstants.PMELogLevel.PMLogError)
		End If
		

		
        Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
        Finally



		
        End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: BuildPartyDetailArray
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
	' ***************************************************************** '
    'developer guide no. 101
    Private Function BuildPartyDetailArray(ByRef r_vPartyDetails As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BuildPartyDetailArray"

        Dim lReturn As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ReDim r_vPartyDetails(4, 0)

        If m_sTaxNumber <> "" Then

            r_vPartyDetails(MainModule.kPartyDetailTaxNumber, 0) = m_sTaxNumber
        Else

            r_vPartyDetails(MainModule.kPartyDetailTaxNumber, 0) = Nothing
        End If

        If Not m_bDomiciledForTax Then
            r_vPartyDetails(MainModule.kPartyDetailDomiciledForTax, 0) = 0
        Else
            r_vPartyDetails(MainModule.kPartyDetailDomiciledForTax, 0) = 1
        End If

        If Not m_bTaxExempt Then
            r_vPartyDetails(MainModule.kPartyDetailTaxExempt, 0) = 0
        Else
            r_vPartyDetails(MainModule.kPartyDetailTaxExempt, 0) = 1
        End If

        If m_dTaxPercentage = 0 Then

            r_vPartyDetails(MainModule.kPartyDetailTaxPercentage, 0) = Nothing
        Else
            r_vPartyDetails(MainModule.kPartyDetailTaxPercentage, 0) = m_dTaxPercentage
        End If

        If m_vBlackListReasonId = 0 Then

            r_vPartyDetails(MainModule.kPartyDetailBlackListReasonId, 0) = Nothing
        Else
            r_vPartyDetails(MainModule.kPartyDetailBlackListReasonId, 0) = m_vBlackListReasonId
        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
	
	' ***************************************************************** '
	' Name: SelectcboItem
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 03-10-2003 : 229
	' ***************************************************************** '
	Private Function SelectcboItem(ByRef r_oCbo As ComboBox, ByVal v_lSelectedId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SelectcboItem"
		
		Dim bItemNotFound As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			bItemNotFound = True
			
			' if the item id is valid
			If v_lSelectedId <> -1 Then
				
				' for each item in the list
				For lItem As Integer = 0 To r_oCbo.Items.Count
					' search the item data array for a match
					If VB6.GetItemData(r_oCbo, lItem) = v_lSelectedId Then
						
						' found a match - select the item
						r_oCbo.SelectedIndex = lItem
						bItemNotFound = False
						Exit For
					End If
					
				Next lItem
				
			End If
			
			If bItemNotFound Then
				
				' log that we havent found the specified item
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lSelectedId", v_lSelectedId)
                gPMFunctions.LogMessageToFile(MainModule.g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogOnError, sFunctionName & " Failed to find item with id:" & CStr(v_lSelectedId) & " in :" & r_oCbo.Name, MainModule.ACApp, ACClass, sFunctionName, oDicParms:=oDict)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lSelectedId", v_lSelectedId)
            gPMFunctions.LogMessageToFile(MainModule.g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogOnError, sFunctionName & " Failed", MainModule.ACApp, ACClass, sFunctionName, excep:=excep, oDicParms:=oDict)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: GetClientCode
	'
	' Description:
	'
	' History: VB
	'
	' ***************************************************************** '
	Private Function GetClientCode(ByRef r_sClientCode As String, ByVal v_sTradingName As String) As Integer
		Dim result As Integer = 0
		Dim sFailureReason As String = ""
		Dim iBranchId As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_oClientNumber Is Nothing Then
				Dim temp_m_oClientNumber As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oClientNumber, "bSIRPolicyNumMaint.Business", gPMConstants.PMGetViaClientManager)
				m_oClientNumber = temp_m_oClientNumber
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse
					' Log Error.
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRPolicyNumMaint.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode")
					Return result
				End If
			End If
			
			iBranchId = BranchId

			m_lReturn = m_oClientNumber.GenerateClientCode(v_sPartyType:=gSIRLibrary.SIRPartyTypeCorporateClient, v_iSourceID:=iBranchId, r_sGeneratedClientCode:=r_sClientCode, r_sFailureReason:=sFailureReason, v_sTradeName:=v_sTradingName.ToUpper())
			
			If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to generate client code", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode")
				
				Return result
			ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then  'Numbering Scheme not set
				MessageBox.Show("Numbering scheme for Corporate Client is not set.", "Corporate Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return gPMConstants.PMEReturnCode.PMCancel
			ElseIf sFailureReason <> "" Then 
				MessageBox.Show(sFailureReason, "Corporate Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return gPMConstants.PMEReturnCode.PMCancel
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: SetClientCodeCntl
	'
	' Description:
	'
	' History: VB
	'
	' ***************************************************************** '
	Private Function SetClientCodeCntl() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetClientCodeCntl"
		
		Dim r_bIsReadOnly, r_bIsNumberingSchemeExists As Boolean
		Dim r_sMaskCode As String = ""
        Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If m_oClientNumber Is Nothing Then
			Dim temp_m_oClientNumber As Object
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oClientNumber, "bSIRPolicyNumMaint.Business", gPMConstants.PMGetViaClientManager)
			m_oClientNumber = temp_m_oClientNumber
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Log Error.
				gPMFunctions.RaiseError("SetClientCodeCntl", "bSIRPolicyNumMaint.Business instance not Created")
                    Return result
			End If
		End If
		

		m_lReturn = m_oClientNumber.SendClientReadOnlyDetails(v_sPartyType:=gSIRLibrary.SIRPartyTypeCorporateClient, r_bIsReadOnly:=r_bIsReadOnly, r_bIsNumberingSchemeExists:=r_bIsNumberingSchemeExists, r_sMaskCode:=r_sMaskCode)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError("SetClientCodeCntl", "SendClientReadOnlyDetails falied")
                Return result
		End If
		
		m_bIsSetMaskingCode = r_bIsNumberingSchemeExists
		m_bIsReadOnly = r_bIsReadOnly
		m_sMaskCode = r_sMaskCode
		
		If (r_bIsNumberingSchemeExists And r_bIsReadOnly) Or (Not r_bIsNumberingSchemeExists And m_bDuplicateClientIdentification) Or (r_bIsNumberingSchemeExists And Not r_bIsReadOnly And m_bDuplicateClientIdentification) Then
			lblIDReference.Enabled = False
			txtIDReference.Enabled = False
		Else
			lblIDReference.Enabled = True
			txtIDReference.Enabled = True
		End If
		

		
        Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


		
        End Try
		Return result
	End Function
	
	Private Function ValidateNumberingScheme() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ValidateNumberingScheme"
		
        Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If m_sMaskCode <> "" Then
			' Branch
			If m_sMaskCode.IndexOf("B"c) >= 0 Then
				If cboBranch.SelectedIndex < 0 Or BranchId < 1 Then
					MessageBox.Show("Please select some Branch", "field - Branch", MessageBoxButtons.OK, MessageBoxIcon.Error)
					result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
				End If
			End If
			
			' Trading Name
			If m_sMaskCode.IndexOf("T"c) >= 0 Then
				If txtName.Text.Trim() = "" Then
					MessageBox.Show("Please enter Trading Name", "field - Trading Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
					result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
				End If
			End If
			
		End If
		


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
		
		
		
        End Try
		Return result
	End Function

  
    Private Sub FillCombo()
        'Developer Guide No.220
        Me.cboTrade.FirstItem = "(Not Known)"
        Me.cboCurrency.FirstItem = ""
        Me.cboTurnover.FirstItem = "(Not Known)"
        Me.cboEmployees.FirstItem = "(Not Known)"
    End Sub

 
    Private Sub cboSubBranch_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubBranch.Leave
        cmdAgentLookUp.Focus()
    End Sub

    Private Sub chkProspect_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkProspect.Leave
        ddBusiness.Focus()
    End Sub

    Private Sub txtSalutation_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSalutation.Leave
        SSTabHelper.SetSelectedTabIndex(tabMainTab, 2)
    End Sub

    Private Sub chkeMPS_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkeMPS.Leave
        SSTabHelper.SetSelectedTabIndex(tabMainTab, 3)
    End Sub

    Private Sub cmdAddConv_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddConv.Leave
        txtCCJ.Focus()
    End Sub

    Private Sub txtCCJ_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCCJ.Leave
        SSTabHelper.SetSelectedTabIndex(tabMainTab, 4)
    End Sub

    Private Sub txtRealFileCode_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRealFileCode.Leave
        cmdAssociates.Focus()
    End Sub

    Private Sub cmdAssociates_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAssociates.Leave
        SSTabHelper.SetSelectedTabIndex(tabMainTab, 5)
    End Sub

    Private Sub cmdAddLoyaltyScheme_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddLoyaltyScheme.Leave
        SSTabHelper.SetSelectedTabIndex(tabMainTab, 6)
    End Sub

    Private Sub cboEmployees_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEmployees.Leave
        cmdConsultantLookup.Focus()
    End Sub

    Private Function IsValidString(ByRef str As String) As Boolean
		Dim illegalChars As New RegularExpressions.Regex("[{}:~%*<>?'`’\\/|,\u2022,\u2023,\u25E6,\u2043,\u2219]")

		If illegalChars.IsMatch(str) Then
                Return False
            End If

        Return True
    End Function
    ''' <summary>
    ''' Add Party History
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddPartyHistory() As Integer
        Dim nReturn As Integer
        Dim oPartyBusiness As bSIRParty.Business = Nothing
        Try
            nReturn = g_oObjectManager.GetInstance(oPartyBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartyHistory")
                Return nReturn
            End If

            ' Create the Party history
            If PartyCnt > 0 Then
                nReturn = oPartyBusiness.AddPartyHistory(PartyCnt, String.Empty)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party History Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartyHistory")
                End If
            End If
            Return nReturn
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartyHistory")
            Return PMEReturnCode.PMFalse
        End Try

    End Function

    Private Sub txtName_Validated(sender As Object, e As EventArgs) Handles txtName.Validated
        If m_oFormFields Is Nothing = False Then
            m_lReturn = m_lReturn & m_oFormFields.LostFocus(ctlControl:=txtName)
            uctPartyBankControl1.PartyName = Trim$(txtName.Text)
        End If
    End Sub
End Class
