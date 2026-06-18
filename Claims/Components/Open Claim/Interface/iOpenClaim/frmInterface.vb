Option Strict Off
Option Explicit On
Option Compare Text
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
'developer guide no. 211
Imports bOpenClaim
Public Class frmInterface
	Inherits System.Windows.Forms.Form
	
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 01/07/2000
	'
	' Description: Main interface.
	'
	' Edit History:Sravan
	'              Sameer:Display Messages From Resource File for Mandatory Fields,invalid date
	'              Pandu
	' JMK(28/02/2001): Add Claim Payment mode, allows limited editing. Assigned to g_nPMMode
	' CJB(27/04/2005): PN16040 Changed PrepareForm to not disable the comments textbox as we won't
	'                  be able to scroll then to read it in view only mode!
	' CJB(28/07/2005): PN22703 Changed cmdOK_Click to open up checking for duplicate claims for broking
	'                  too when in add new claim mode
	' MAE(28-10-2005): Added code for Suppress Claim Transactions
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.

    Dim objfrmDuplicateClaimsOverride As frmDuplicateClaimsOverride
    Private Const ACClass As String = "frmInterface"
    'Developer Guide no. 7(Latest Guide)
    Private Const vbFormCode As Integer = 0
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	'Status of FSA compliance flag.
	Private m_bFSAComplianceFlag As Boolean
	
	Private m_sClientTemp As String = ""
	Private m_sInsurertemp As String = ""
	
	'Status of the Claim on Load -Pandu
	Private m_lClaimStatusonLoad As Integer
	'S4B Claim Enhancements R&D 2005
	Private m_bClaimNcdBonusOnLoad As Boolean
	'Variable for RiskType Description
	Private m_sRiskTypeDescription As String = ""
	
	Private m_bFormload As Boolean
	Private m_bDataChanged As Boolean
    Private m_bLossDateChanged As Boolean
    Private m_bReportedDateChanged As Boolean
	
	'Variables for Lookup Values-Pandu
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object
	
	Private m_lPartyCnt As Integer
	
	Private m_sClaimNumber As String = ""
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iOpenClaim.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	
	' Declare an instance of the FormFieldsCollection.
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Stores the return value for the a function call.
	Private m_lReturn As Integer
	
	'To store the Secondary Cause Values
    Private m_vSecondaryCauseArray(,) As Object
	
	' RDT 08/04/03 - Start - IAG 215 Spec
    Private m_vPrimaryCauseArray(,) As Object
	' RDT 08/04/03 - End - IAG 215 Spec
	
	' Control array to store the first and last text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
	
	'RWH(10/11/2000) claim numbering.
    Private m_vInsuranceFileDetails(,) As Object
	
	'TN20010905 - redeclare at module level for use in cmdOK_Click
	Dim bHasAgent As Boolean
	
	' SET 30072002 - Private constants for toolbar images
	Private Const m_iEvent As Integer = 11
	Private Const m_iRisk As Integer = 15
	Private Const m_iInfo As Integer = 16
	' SET 30072002 - end
	Private Const m_kTOOLBAR_IMAGE_FINANCIAL As Integer = 17 'S4B Claim Enhancements R&D 2005
	'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.5.1)
	Private Const m_kTOOLBAR_IMAGE_DOCARCHIVE As Integer = 18
	'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.5.1)
	
	'Start Constant for email address char PN_69448
	Private Const StrEmail As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.@_"
	'end  PN_69448
	
	' RDT 08/04/03 - Start - Constant for 'Original Value'
	Private Const ORIGINAL_VALUE_STR As String = "Original Value: "
	' RDT 08/04/03 - End - Constant for 'Original Value'
	
	'DD 31/03/2004
	Private m_bOptionUnderwritingYear As Boolean
	'AR20050404 - PN15644
	
	Private m_bInsurerChanged As Boolean
	
	Private m_sDuplicateClaimEventDescription As String = ""
	Private m_bCreateDuplicateClaimOverrideEvent As Boolean
	
	'S4B Claim Enhancements R&D 2005
	Private m_sSiriusProduct As String = ""
	
	'eck 11/2005
	Private m_vCoInsurerSplit As Object
	Private m_bNZConfig As Boolean
	
	Private m_sLossDate As String = ""
	Private m_sLossTime As String = ""
	Private m_bLossDateTime As Boolean
	
	Private m_sPolicyRef As String = ""
	Private m_lPolicyId As Integer
	Private m_sClaimsTypeBasisCode As String = ""
	Private m_sClaimsCoverBasisCode As String = ""
    Private m_bDisplayValidVersionEnabled As Boolean

	Private m_dtInceptionDate As Date
    Private m_lBaseCaseID As Integer
    Private m_iUserOtherPartyID As Integer
	
	Private m_bPolicyFound As Boolean
	
	Private m_lClaimsUserDefTableA As Integer
	Private m_lClaimsUserDefTableB As Integer
	Private m_lClaimsUserDefTableC As Integer
	Private m_lClaimsUserDefTableD As Integer
	Private m_lClaimsUserDefTableE As Integer
	Private m_bDuplicateCheckEnabled As Boolean
	
	Private m_lWorkflowId As Integer
	Private m_lProductId As Integer
	Private m_bFastTrackEnabled As Boolean
	Private m_iClaimPaymentValid As Integer
	'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.2.1)
	Private m_bShowPaymentView As Boolean

    Private m_bClearRiskType As Boolean
	'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.2.1)
	
	'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.2.1)
	
    Private m_bAttachClaimOutsideOfPolicyPeriod As Boolean
	Public Property ShowPaymentView() As Boolean
		Get
			
			Return m_bShowPaymentView
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bShowPaymentView = Value
			
		End Set
	End Property
	'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.2.1)
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Return any error number that might have occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			' Set the navigate flag.
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public WriteOnly Property FSAComplianceFlag() As String
		Set(ByVal Value As String)
			
			'Set the FSAComplianceFlag.
			m_bFSAComplianceFlag = Value <> ""
			
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			' Set the process mode.
			m_lProcessMode = Value
			
		End Set
	End Property
	Public Property TransactionType() As String
		Get
			
			' Set the type of business.
			Return m_sTransactionType
			
		End Get
		Set(ByVal Value As String)
			
			' Set the type of business.
			m_sTransactionType = Value
			
		End Set
	End Property
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			' Set the effective date.
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	
	Public Property Task() As Integer
		Get
			Return m_iTask
		End Get
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	

	'Private Sub Status(ByVal Value As Integer)
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public WriteOnly Property WorkflowId() As Integer
		Set(ByVal Value As Integer)
			
			' Set the WorkflowId
			m_lWorkflowId = Value
			
		End Set
	End Property
	
	Public WriteOnly Property FastTrackEnabled() As Boolean
		Set(ByVal Value As Boolean)
			
			' Set the product's FastTrack status.
			m_bFastTrackEnabled = Value
			
		End Set
	End Property
	Public ReadOnly Property ClaimPaymentValid() As Integer
		Get
			Return m_iClaimPaymentValid
		End Get
	End Property
	
	
	Public Property OperateMode() As Integer
		Get
			Return g_nPMMode
		End Get
		Set(ByVal Value As Integer)
			g_nPMMode = Value
		End Set
	End Property
	
	
	Public Property ClaimNo() As String
		Get
			Return g_sClaimNo
		End Get
		Set(ByVal Value As String)
			g_sClaimNo = Value
		End Set
	End Property
	
	
	Public Property PolicyID() As Integer
		Get
			Return g_lPolicyID
		End Get
		Set(ByVal Value As Integer)
			g_lPolicyID = Value
			g_lOrPolicyID = Value
		End Set
	End Property
	
	
	Public Property PolicyNo() As String
		Get
			Return g_sPolicyNo
		End Get
		Set(ByVal Value As String)
			g_sPolicyNo = Value
		End Set
	End Property
	
	Public Property ClaimDate() As String
		Get
			Return g_vClaimDate
		End Get
		Set(ByVal Value As String)

			g_vClaimDate = CStr(Value)
		End Set
	End Property
	
	Public Property Claimid() As Integer
		Get
			Return g_lClaimID
		End Get
		Set(ByVal Value As Integer)
			g_lClaimID = Value
		End Set
	End Property
	'DC140904 PN14948 allow merge fields to work
	Public Property PartyCnt() As Integer
		Get
			Return g_lPartyCnt
		End Get
		Set(ByVal Value As Integer)
			g_lPartyCnt = Value
		End Set
	End Property
	
	
	
	Public Property BaseCaseID() As Integer
		Get
			Return m_lBaseCaseID
		End Get
		Set(ByVal Value As Integer)
			m_lBaseCaseID = Value
		End Set
	End Property
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		Dim result As Integer = 0
		Dim lBusinessTypeId As Integer
        Dim vReturnArray(,) As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'RWH(09/11/2000)
			'Insert calls to retrieve claim no. for Underwriting.
			'DC291100 is also required for broking ?????
			
			'RWH(14/11/2000) g_lPolicyID is actually the 'Event_log.event_cnt' value !!!!

			m_lReturn = g_oBusiness.GetInsuranceFile(g_lPolicyID, m_vInsuranceFileDetails)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = g_oBusiness.RetrieveCurrenciesForBranch(iSourceID:=CInt(m_vInsuranceFileDetails(ACInsFileSourceId, 0)), vReturnArray:=vReturnArray)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			For iLoop As Integer = vReturnArray.GetLowerBound(1) To vReturnArray.GetUpperBound(1)
				Dim cboCurrency_NewIndex As Integer = -1

                cboCurrency_NewIndex = cboCurrency.Items.Add(CStr(vReturnArray(1, iLoop)))

                VB6.SetItemData(cboCurrency, cboCurrency_NewIndex, CInt(vReturnArray(0, iLoop)))
			Next 
			'DC280601 -start -only do following for Underwriting
			
			
			
			Select Case g_nPMMode
				Case g_nADDMODE
					lBusinessTypeId = ACProvisionalClaim
					
					If Information.IsArray(m_vInsuranceFileDetails) Then
						m_lReturn = GenerateClaimNumber(lBusinessTypeId)
						
						'PSL 03/06/2003 Issue 4490
						'It won't know to change the Claim number without setting this
						g_sProvisionalClaimNo = g_sClaimNo
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							Return gPMConstants.PMEReturnCode.PMFalse
						End If
						
					End If
				Case Else
					
			End Select
			
			
			'DC280601 -end
			
			
			'End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values and prepares
	'              the form for data entry or for View depending on the Mode
	'
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Const kInceptionDate As Integer = 4
		Const kDisplayClaimReinsurance As Integer = 5
		'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.2.2)
		Const kPaymentTab As Integer = 4
		Const kDefault As Integer = 0
		'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.2.2)
        Dim RResult(,) As Object
		Dim sUsername As String = ""
		Dim vValue As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
            SSTabHelper.SetTabVisible(tabMainTab, 1, False)

            'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.2.2)
            If Not ShowPaymentView Then
                SSTabHelper.SetSelectedIndex(tabMainTab, kDefault)
            Else
                SSTabHelper.SetSelectedIndex(tabMainTab, kPaymentTab)
            End If
            'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.2.2)

			lblSubsidiaryCompany.Visible = False
			txtOpenClaim(g_nSUBSIDIARY_COMPANY).Visible = False
			lblPolicyNumber.Visible = False
			txtOpenClaim(g_nPOLICY_NUMBER).Visible = False
			
			lblAccountExec.Visible = False
			txtOpenClaim(g_nACCOUNT_EXEC).Visible = False
			lblAccountHandler.Visible = False
			txtOpenClaim(g_nACCOUNT_HANDLER).Visible = False
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = PrepareForm(g_nPMMode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = SetFirstLastControls()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Populate lookup combos
			m_lReturn = DisplayLookupDetails()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Populate lookup combos
			m_lReturn = DisplayUserDefinedFieldsSFU()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Populate Secondary Cause

			m_lReturn = g_oBusiness.SelectSecondaryCause(m_vSecondaryCauseArray)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
					
				Else
					
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			'Getting Risk Details from the Business Object

			m_lReturn = g_oBusiness.GetRiskDetails(g_lPolicyID, g_vClaimDate, RResult, g_lClaimID)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				lblClaimHandled.Visible = False 'PN-67174
				chkClaimHandled.Visible = False 'PN-67174
				'm_lErrorNumber& = PMFalse
				Return result
			End If
			
			If Information.IsArray(RResult) Then
				
				'Load Details of Risk Type in the combo
				'RWH(06/03/2001) This doesn't retrieve risk_type_id at all.


				LoadDataInCombo(cboRiskType, RResult, RResult.GetLowerBound(1), RResult.GetUpperBound(1) + 1)

                m_dtInceptionDate = gPMFunctions.ToSafeDate(CStr(RResult(kInceptionDate, 0)))
				
				'if available at product level
				If g_bDisplayClaimReinsurance Then

                    g_bDisplayClaimReinsurance = gPMFunctions.ToSafeBoolean(CStr(RResult(kDisplayClaimReinsurance, 0)))
				End If
			End If
			
			'**********************Bud id -13 & 15 Changes Start Here Pandu Defaulting Risk Type 17-10-2000**
			
			lblClaimHandled.Visible = False
			chkClaimHandled.Visible = False
			
			
			
			'Get the UserName
			

			sUsername = g_oBusiness.UserName
			
			'Check with existing handler if User Name is Available
			
			For nCount As Integer = 0 To cboHandler.Items.Count - 1
				If sUsername.Trim() = VB6.GetItemString(cboHandler, nCount) Then
					cboHandler.SelectedIndex = nCount
					Exit For
				End If
			Next 
			
			'****************Bud id -13 & 15 End of Changes ************************************************
			
			' DD 25/03/2004
			m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, 1, vValue)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			m_bOptionUnderwritingYear = (vValue = "1")
			
			If m_bOptionUnderwritingYear Then
				lblUnderwritingYearID.Visible = True
				cboUnderwritingYearID.Visible = True
			End If
			
			'S4B Claim Enhancements R&D 2005
			lblSubsidiaryCompany.Visible = False
			txtOpenClaim(g_nSUBSIDIARY_COMPANY).Visible = False
			
			m_lReturn = FormatDate(txtOpenClaim(g_nLOSS_DATE).Text, g_nLOSS_DATE)
			m_sLossDate = txtOpenClaim(g_nLOSS_DATE).Text
            m_sLossTime = txtOpenClaim(g_nLOSS_TIME).Text

            m_lReturn = GetUserOtherParty()


			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
    End Function

    Public Function GetUserOtherParty() As Long
        Dim iUserOtherPartyID As Integer
        Dim iResult As Integer
        Dim vResultArray(,) As Object
        Try

            'ToDo:= Get the other_party_id for g_iUserID 
            m_lReturn = g_oBusiness.GetUserOtherParty(iUserID:=g_oObjectManager.UserID, r_vResultArray:=vResultArray)
            If vResultArray(0, 0) <> "" Then
                iUserOtherPartyID = vResultArray(0, 0)
            End If

            'ToDo : 
            If iUserOtherPartyID <> 0 Then
                '   Default the TPA dropdown to the TPA mapping the Logged in User ID
                If g_lClaimID = 0 Then
                    txtTPA.Tag = CStr(iUserOtherPartyID)
                    txtTPA.Text = vResultArray(1, 0)
                End If

                cmdTPA.Enabled = False
            Else
                If m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CR" Then
                    cmdTPA.Enabled = True
                End If
            End If

            m_iUserOtherPartyID = iUserOtherPartyID
            'UserOtherPartyID = iUserOtherPartyID

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMTrue
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the Other Party ID", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherParty", excep:=ex)
        Finally

        End Try

        GetUserOtherParty = iResult

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
			ReDim m_ctlTabFirstLast(1, 3)
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			m_ctlTabFirstLast(ACControlStart, 0) = cboHandler
			'Set m_ctlTabFirstLast(ACControlEnd, 0) = chkLikelyClaim
			
			m_ctlTabFirstLast(ACControlStart, 1) = txtOpenClaim(g_nCLIENT_TELNO)
			m_ctlTabFirstLast(ACControlEnd, 1) = txtOpenClaim(g_nCLIENT_CLAIMNO)
			
			m_ctlTabFirstLast(ACControlStart, 2) = txtOpenClaim(g_nINSURER_TELNO)
			m_ctlTabFirstLast(ACControlEnd, 2) = txtOpenClaim(g_nINSURER_CLAIMNO)
			
			'Set m_ctlTabFirstLast(ACControlStart, 3) = txtComments
			'Set m_ctlTabFirstLast(ACControlEnd, 3) = txtComments
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DisplayCaptions
	'
	' Description: Display all language specific captions.
	'
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblRiskType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskTypeButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            CmdClient.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdInsurer.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'***Claim Details tab*****start*************

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1B, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblHandler.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHandler, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblProgressStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProgressStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClmStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblClaimStatusDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClmStatusDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPrimaryCausationCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPrimaryCause, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSecondaryCausationCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSecondaryCause, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCatastropheCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCatastrophecode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTown.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTown, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLocation.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLocation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLossDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLossDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLossToDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLossToDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblReportedDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReportedDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblReportedToDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReportedToDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLastModified.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLastModDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblClaimHandled.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimHandled, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblInformation.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLikelyToClaim.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLikelyToClaim, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '***Claim Details tab*****Finish*************

            'S4B Claim Enhancements R&D 2005

            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2B, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            '***Client Details tab*****start*************

            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3U, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblClientName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTelePhoneNumberH.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTelephoneNumberHome, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTelePhoneNumberO.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTelephoneNumberOffice, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblFaxNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientFaxNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblMobileNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMobileNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEmailAddress.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientEmailNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_bNZConfig Then

                lblVAT.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_RES_TAX_REGISTERED, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                lblVATRegistartionNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_RES_TAX_REGISTRATION_NUMBER, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                lblVAT.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACVATRegistered, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                lblVATRegistartionNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACVATRegistrationNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If


            lblClientClaimNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '***Client Details tab*****Finish*************



            '***Insurer Details tab*****Start*************


            SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle4U, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))





            lblInsurerName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 4, "4 - Payment History")



            lblTelephoneNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInsurerTelephoneNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblIFaxNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInsurerFaxNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblContact.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACContact, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEmailIns.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInsurerEmailNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC231100 if Underwriting display Agent not Insurer

            lblInsurerClaimNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC231100


            '******End of Change For User Defined Captions

            '***Comments tab*****Finish*************

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' JMK 31/05/2001 set focus to the required field
    ' ***************************************************************** '
    Private Function CheckMandatory() As Boolean

        Dim result As Boolean = False
        Dim lBusinessTypeId As Integer
        Dim sFailureReason As String = ""

        Try
            'KB PN 3680
            'In Broking we auto-generate claim numbers and do not allow overwrite

            'Tomo190402
            If txtClaimNumber.Text.Trim() = "" Then
                DisplayMessage(ACMandatoryFieldMsg, lblClaimNumber.Name.Substring(3))
                result = False
                'PSL 13/05/2003 Only set focus if Enabled
                If txtClaimNumber.Enabled Then txtClaimNumber.Focus()
                Return result
            End If

            Select Case g_nPMMode
                Case g_nADDMODE
                    lBusinessTypeId = ACProvisionalClaim
                Case Else
                    lBusinessTypeId = ACFullClaim
            End Select

            If txtClaimNumber.Enabled Then

                m_lReturn = g_oBusiness.ValidateClaimNumber(sEnteredNumber:=txtClaimNumber.Text, v_lBusinessType:=lBusinessTypeId, v_lProductID:=Conversion.Val(CStr(m_vInsuranceFileDetails(ACInsFileProductId, 0))), sFailureReason:=sFailureReason)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If sFailureReason = "" Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureReason, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    Else
                        MessageBox.Show(sFailureReason, "Claim Number Mask", MessageBoxButtons.OK, MessageBoxIcon.Error) 'Mid$(lblClaimNumber.Name, 4)
                        result = False
                        If txtClaimNumber.Enabled Then txtClaimNumber.Focus()
                        Return result
                    End If
                End If
            End If


            If m_bOptionUnderwritingYear And cboUnderwritingYearID.Text = "" And g_nPMMode <> g_nREADMODE And g_nPMMode <> g_nVIEWMODE Then
                MessageBox.Show("Please select underwriting year", "Underwriting Year", MessageBoxButtons.OK, MessageBoxIcon.Error) 'Mid$(lblClaimNumber.Name, 4)

                If cboUnderwritingYearID.Enabled Then
                    cboUnderwritingYearID.Focus()
                End If

                Return False
            End If


            If cboHandler.Text = "" Then ' Handler Combo box
                DisplayMessage(ACMandatoryFieldMsg, lblHandler.Name.Substring(3))
                result = False
                If cboHandler.Enabled Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    cboHandler.Focus()
                End If
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            If cboProgressStatus.Text = "" Then ' Progress Status Combo box
                DisplayMessage(ACMandatoryFieldMsg, lblProgressStatus.Name.Substring(3))
                result = False
                If cboProgressStatus.Enabled Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    cboProgressStatus.Focus()
                End If
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            If txtOpenClaim(g_nDESCRIPTION).Text.Trim() = "" Then ' Description Text box
                DisplayMessage(ACMandatoryFieldMsg, lblDescription.Name.Substring(3))
                result = False
                If txtOpenClaim(g_nDESCRIPTION).Enabled Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    txtOpenClaim(g_nDESCRIPTION).Focus()
                End If
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            If cboPrimaryCausationCode.Text = "" Then ' Primary Causation Code Combo box
                DisplayMessage(ACMandatoryFieldMsg, "Primary Cause Code")
                result = False
                If cboPrimaryCausationCode.Enabled Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    cboPrimaryCausationCode.Focus()
                End If
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            If txtOpenClaim(g_nLOSS_DATE).Text.Trim() = "" Then ' Loss Date Text box
                DisplayMessage(ACMandatoryFieldMsg, lblLossDate.Name.Substring(3))
                result = False
                If txtOpenClaim(g_nLOSS_DATE).Enabled Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    txtOpenClaim(g_nLOSS_DATE).Focus()
                End If
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            If txtOpenClaim(g_nLOSS_TIME).Text.Trim() = "" Then ' Loss Time Text box
                DisplayMessage(ACMandatoryFieldMsg, lblLossDate.Name.Substring(3))
                result = False
                If txtOpenClaim(g_nLOSS_TIME).Enabled Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    txtOpenClaim(g_nLOSS_TIME).Focus()
                End If
                Return result
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
            Else
                result = True
            End If

            If txtOpenClaim(g_nREPORTED_DATE).Text.Trim() = "" Then ' Reported Date Text box
                DisplayMessage(ACMandatoryFieldMsg, lblReportedDate.Name.Substring(3))
                result = False
                If txtOpenClaim(g_nREPORTED_DATE).Enabled Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    txtOpenClaim(g_nREPORTED_DATE).Focus()
                End If
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            If txtOpenClaim(g_nREPORTED_TIME).Text.Trim() = "" Then ' Reported Time Text box
                DisplayMessage(ACMandatoryFieldMsg, lblReportedDate.Name.Substring(3))
                result = False
                If txtOpenClaim(g_nREPORTED_TIME).Enabled Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    txtOpenClaim(g_nREPORTED_TIME).Focus()
                End If
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            If cboCurrency.Text = "" Then ' Currency Combo box
                DisplayMessage(ACMandatoryFieldMsg, lblCurrency.Name.Substring(3))
                result = False
                If cboCurrency.Enabled Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    cboCurrency.Focus()
                End If
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If


            If cboRiskType.Text = "" Then ' RiskType Combo box
                DisplayMessage(ACMandatoryFieldMsg, lblRiskType.Name.Substring(3))
                result = False
                If cboRiskType.Enabled Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    cboRiskType.Focus()
                End If
                Return result
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                Return True
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for Mandatory Fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name:         CheckProgressStatus
    ' Created:      Peter Finney (12/01/2006)
    ' Description:  Check if we should enter quick close claim mode
    ' ***************************************************************** '
    Private Function CheckProgressStatus() As Boolean

        Dim result As Boolean = False
        Dim lStatus As Integer
        Dim bIsClosed As Boolean
        Dim vResults As Object
        Dim sMessage As String = ""
        Dim sMessageResponse As MsgBoxStyle

        Dim lReturn As Integer
        'Start (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (5.2.1)
        Dim sOptionValue As String = ""
        Dim bRestrictNonZeroClosure As Boolean
        'End (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (5.2.1)
        Const kMethodName As String = "CheckProgressStatus"


        Try

        result = True
        g_bBalanceAndCloseClaim = False

        ' Only do this if the claim was not closed to start with and this is maintain claim
        If (m_sTransactionType = "C_CR") And (Not iPMFunc.IsIn(CStr(m_lClaimStatusonLoad), CLMClosed, CLMReClosed)) Then
            ' Check if the current progress status has the closed_flag set
            lStatus = gPMFunctions.ToSafeLong(CStr(VB6.GetItemData(cboProgressStatus, cboProgressStatus.SelectedIndex)), 0)

            m_lReturn = g_oBusiness.IsProgressStatusClosed(v_lClaimStatusID:=lStatus, r_bIsClosed:=bIsClosed)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("IsProgressStatusClosed", "Failed to check progress status flag")
            End If

            ' Only prompt if the new status is closed
            If bIsClosed Then
                ' Check if reserves are zero

                m_lReturn = g_oBusiness.GetCurrentReserveRecovery(v_lClaimId:=g_lClaimID, r_vDataArray:=vResults)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("g_oBusiness.GetCurrentReserveRecovery", "Failed to check current reserves")
                End If

                'Start (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (5.2.1)
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSystemOptionRestrictNonZeroClosure, r_sOptionValue:=sOptionValue, v_iSourceID:=g_oObjectManager.SourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed for option_number :- " & kSystemOptionRestrictNonZeroClosure, gPMConstants.PMELogLevel.PMLogError)
                End If
                bRestrictNonZeroClosure = sOptionValue.Trim() = "1"

                'End (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (5.2.1)

                If Information.IsArray(vResults) Then
                    ' Check the current reserve and recovery reserve
                        If (gPMFunctions.ToSafeCurrency(vResults(1, 0)) = 0) And (gPMFunctions.ToSafeCurrency(vResults(2, 0)) = 0) Then
                        sMessage = "Close this claim?"
                        sMessageResponse = MsgBoxStyle.YesNoCancel
                    Else
                        'Start (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (5.2.1)
                        If bRestrictNonZeroClosure Then
                            sMessage = "Close this Claim? All reserves must be set to Zero"
                            sMessageResponse = MsgBoxStyle.YesNo
                        Else
                            'sMessage = "Balance and close this claim?"
                            'PN34192
                            sMessage = "All reserves (Including Salvage and Third Party Recovery) will be reduced to Zero"
                            sMessageResponse = MsgBoxStyle.YesNoCancel
                        End If
                        'End (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (5.2.1)
                    End If
                Else
                    ' No array so no transaction?
                    sMessage = "Close this claim?"
                    sMessageResponse = MsgBoxStyle.YesNoCancel
                End If

                Select Case Interaction.MsgBox(sMessage, sMessageResponse, "Progress Status Changed")
                    Case System.Windows.Forms.DialogResult.Yes
                        'Start (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (5.2.1)


                            'moving process continue while if Restrict checkbox in system option is true as well Yogi
                            ' If bRestrictNonZeroClosure Then result = False



                        g_bBalanceAndCloseClaim = Not bRestrictNonZeroClosure
                        'g_bBalanceAndCloseClaim = True
                        'End (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (5.2.1)
                    Case System.Windows.Forms.DialogResult.No
                            'If bRestrictNonZeroClosure Then
                            '    result = False
                            'Else
                            '    g_bBalanceAndCloseClaim = False
                            'End If

                            'Always return false if in case of No
                            Return False
                    Case Else
                        result = False
                End Select
            Else
                g_bBalanceAndCloseClaim = False
            End If
        End If


        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)
        result = False

        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: CheckUpdatedLossDate
    ' Created: JMK (05/04/2001)
    ' Description: If the LossDate has changed during Maintain Claim
    '              give Warning message, and create Event Log entry
    '              bug #464
    ' ***************************************************************** '
    Private Function CheckUpdatedLossDate() As Boolean

        Dim result As Boolean = False
        Try
            Dim sOldDate, sOldTime As String

            sOldDate = StringsHelper.Format(g_sLossFromDate, ACDateConversion)
            Dim TempDate As Date
            sOldTime = IIf(DateTime.TryParse(g_sLossFromDate, TempDate), TempDate.ToString("HH:mm"), g_sLossFromDate)
            result = True
            'Modified(ADD two line) as for comparison of same date with two diff. format 
            Dim Obj_g_dtLossDate As String
            Obj_g_dtLossDate = FormatDateTime(g_dtLossDate, DateFormat.ShortDate)

            'do this if the either date or time has changed
            'If sOldDate <> g_dtLossDate Or sOldTime <> txtOpenClaim(g_nLOSS_TIME).Text Then
            If sOldDate <> Obj_g_dtLossDate Or sOldTime <> txtOpenClaim(g_nLOSS_TIME).Text Then
                'build the warning message, reinsurer/insurer

                'put in the new values
                g_dtLossDate = StringsHelper.Format(txtOpenClaim(g_nLOSS_DATE).Text, ACDateConversion)
                g_dtLossTime = txtOpenClaim(g_nLOSS_TIME).Text

                'JMK 31/05/2001 - Underwriting only: event will be created at end of roadmap

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed the check for Update Loss Date", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUpdatedLossDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: LoadDataInCombo
    '
    ' Description: Fills the data from variant array into combobox
    '               INPUTS : Combo Control to be filled
    '                       2D - Array Containing the Record values
    '                       Index in the Array where the Records of the
    '                           Table Start from
    '                       Number of records to enter
    ' ***************************************************************** '
    Private Function LoadDataInCombo(ByRef cboControl As ComboBox, ByVal vntData(,) As Object, ByVal vnStart As Integer, ByVal vnCount As Integer) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check whether an array has been passed
            If Information.IsArray(vntData) Then

                'clear the combobox
                cboControl.Items.Clear()

                'Load the data from the Array to the combobox
                For lCount As Integer = vnStart To vnStart + vnCount - 1


                    sDescription = Convert.ToString(vntData(1, lCount)).Trim()
                    If sDescription = "" Then
                        ''RWH(12/04/2001) If no risk description then use risk type description.

                        sDescription = Convert.ToString(vntData(3, lCount)).Trim()
                    End If

                    Dim cboControl_NewIndex As Integer = -1
                    cboControl_NewIndex = cboControl.Items.Add(sDescription)

                    VB6.SetItemData(cboControl, cboControl_NewIndex, CInt(vntData(0, lCount)))
                Next lCount

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load data in combobox", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDataInCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Private Sub cboCatastropheCode_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCatastropheCode.SelectionChangeCommitted

        '    If m_bFormload = True Then
        '
        '        m_bDataChanged = True
        '
        '    End If

    End Sub



    Private Sub cboCatastropheCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCatastropheCode.Leave


        Dim nhandler As Integer = 0
        'cboCatastropheCode.ListIndex = 0

        For nCount As Integer = 0 To cboCatastropheCode.Items.Count - 1

            If cboCatastropheCode.Text.Trim() = VB6.GetItemString(cboCatastropheCode, nCount) Then
                cboCatastropheCode.SelectedIndex = nCount
                nhandler = 1
                Exit For
            End If

            '        If (nCount < (cboCatastropheCode.ListCount - 1)) Then
            '
            '            cboCatastropheCode.ListIndex = cboCatastropheCode.ListIndex + 1
            '
            '         End If

        Next

        If nhandler = 0 Then

            'If cboCatastropheCode.ListCount > 0 Then

            cboCatastropheCode.Text = ""

            'End If

        End If

    End Sub


    Private isInitializingComponent As Boolean
    Private Sub cboPrimaryCausationCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPrimaryCausationCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        ' RDT 08/04/03 - start - prompt the user on change of Primary Cause

        Dim strCheck As String = ""

        'DC081103 PN8955 -added EDITADDMODE
        'Modified as per vb code
        'If (g_nPMMode = g_nEDITMODE Or g_nPMMode = g_nEDITADDMODE) And Not m_bFormload Then
        If (g_nPMMode = g_nEDITMODE Or g_nPMMode = g_nEDITADDMODE) And m_bFormload Then

            strCheck = "Changing the Primary Cause will affect the questions asked subsequently."

            MessageBox.Show(strCheck, "Primary Cause", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If

    End Sub

    Private Sub cboProgressStatus_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProgressStatus.SelectedIndexChanged

        '    If m_bFormload = True Then
        '
        '        m_bDataChanged = True
        '
        '    End If

    End Sub

    Private Sub cboProgressStatus_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProgressStatus.Leave

        If m_sTransactionType <> "C_CO" Then
            If cboProgressStatus.Text = "Closed" Then
                If m_lClaimStatusonLoad = CLMReOpened Then
                    g_lClaimStatusID = CLMReClosed
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_RECLOSED
                Else
                    g_lClaimStatusID = CLMClosed
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_sCLOSED
                End If
            Else
                If m_lClaimStatusonLoad = CLMClosed Or m_lClaimStatusonLoad = CLMReClosed Then
                    g_lClaimStatusID = CLMReOpened
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_REOPENED
                Else
                    g_lClaimStatusID = CLMLiveOpenClaim
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_sLIVEOPENCLAIM
                End If
            End If
        End If
    End Sub

    Private Sub cboSecondaryCausationCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSecondaryCausationCode.Leave

        Dim nhandler As Integer = 0

        For nCount As Integer = 0 To cboSecondaryCausationCode.Items.Count - 1

            If cboSecondaryCausationCode.Text.Trim() = VB6.GetItemString(cboSecondaryCausationCode, nCount) Then
                cboSecondaryCausationCode.SelectedIndex = nCount
                nhandler = 1
                Exit For
            End If


        Next

        If nhandler = 0 Then

            'If cboSecondaryCausationCode.ListCount > 0 Then

            cboSecondaryCausationCode.Text = ""

            'End If

        End If
    End Sub

    Private Sub cboTown_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTown.Leave


        Dim nhandler As Integer = 0

        For nCount As Integer = 0 To cboTown.Items.Count - 1

            If cboTown.Text.Trim() = VB6.GetItemString(cboTown, nCount) Then
                cboTown.SelectedIndex = nCount
                nhandler = 1
                Exit For
            End If


        Next

        If nhandler = 0 Then

            'If cboTown.ListCount > 0 Then

            cboTown.Text = ""

            'End If

        End If

    End Sub


    'Private Sub Exit_Click()
    'cmdCancel_Click(cmdCancel, New EventArgs())
    'End Sub

    Private Sub cmdChangeClientPolicy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdChangeClientPolicy.Click
        m_lReturn = GetPolicyInfo()
    End Sub

    'DC251001 -start - allow Broking user to select a another Insurer
    Private Sub cmdInsurerDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsurerDetails.Click

        Dim vCnt, vShortName, vName, vResolvedName, vIResult As Object

        Try





            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="IN", vResolvedName:=vResolvedName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'DJM 07/05/2002 : Changed to use GetPartyDetails

            m_lReturn = g_oBusiness.GetPartyDetails(vShortName, 4, vIResult)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'AR20050404 - PN15664

                g_lInsurer_AddressId = gPMFunctions.ToSafeLong(CStr(vIResult(13)), 0)
                g_lInsurer_AddressUsage = 4


                txtOpenClaim(g_nINSURER_NAME).Text = Convert.ToString(vIResult(ACIInsurerName)).Trim()

                g_sInsurerShortName = Convert.ToString(vIResult(ACIInsurerShortName)).Trim()

                g_sInsurerAddress1 = Convert.ToString(vIResult(ACIAddress1)).Trim()

                g_sInsurerAddress2 = Convert.ToString(vIResult(ACIAddress2)).Trim()

                g_sInsurerAddress3 = Convert.ToString(vIResult(ACIAddress3)).Trim()

                g_sInsurerAddress4 = Convert.ToString(vIResult(ACIAddress4)).Trim()

                g_sInsurerPostCode = Convert.ToString(vIResult(ACIPostCode)).Trim()
                txtOpenClaim(g_nINSURER_ADDRESS).Text = g_sInsurerAddress1 & " " &
                                                        g_sInsurerAddress2 & " " &
                                                        g_sInsurerAddress3 & " " &
                                                        g_sInsurerAddress4 & " " &
                                                        g_sInsurerPostCode


                txtOpenClaim(g_nINSURER_TELNO).Text = Convert.ToString(vIResult(12)).Trim()

                txtOpenClaim(g_nINSURER_FAXNO).Text = Convert.ToString(vIResult(ACCFax)).Trim()


                txtOpenClaim(g_nINSURER_EMAIL).Text = CStr(vIResult(ACCEmail))

                'AR20050404 - PN15644
                m_bInsurerChanged = True

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdInsurerDetails_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub Form_Initialize_Renamed()

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iOpenClaim.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'Done to match the VB functionality.
    Private Sub frmInterface_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Shifted the code to frmInterfaceLoad
    End Sub
    ''' <summary>
    ''' frmInterfaceLoad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub frmInterfaceLoad()

        Dim InsResult(,) As Object
        Dim CResult As Object
        Dim IResult As Object
        Dim vInsurerContact As Object
        Dim vClientContact As Object
        Dim oResultArray As Object
        Dim lRiskTypeID As Long
        Dim oSIRProduct As bSIRProduct.Business
        Dim lAllowCurrencyChange As Integer
        Dim vIsRI2007 As String = ""

        Const ACConTelephoneHomeArea As Integer = 0
        Const ACConTelephoneHomeNumber As Integer = 1
        Const ACConTelephoneHomeExt As Integer = 2

        Const ACConTelephoneOfficeArea As Integer = 3
        'Const ACConTelephoneOfficeNumber As Integer = 4
        'Const ACConTelephoneOfficeExt As Integer = 5

        Const ACConFaxArea As Integer = 6
        Const ACConFaxNumber As Integer = 7
        Const ACConFaxExt As Integer = 8

        Const ACConMobileArea As Integer = 9
        Const ACConMobileNumber As Integer = 10
        Const ACConMobileExt As Integer = 11

        Const ACConEmail As Integer = 12
        Const kClaimTypeBasis As Integer = 0

        Try

            '#  m_lReturn& = SetFieldValidation

            m_bDataChanged = False
            m_bFormload = False

            'S4B Claim Enhancements R&D 2005
            If Not (g_oBusiness Is Nothing) Then

                m_sSiriusProduct = g_oBusiness.SiriusProduct
            End If

            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            m_lReturn = g_oBusiness.GetInsuranceFile(g_lPolicyID, m_vInsuranceFileDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            If Information.IsArray(m_vInsuranceFileDetails) Then
                'store claims productId
                m_lProductId = gPMFunctions.ToSafeLong(CStr(m_vInsuranceFileDetails(ACInsFileProductId, 0)))

                m_lReturn = GetProductDetails(v_lProductID:=m_lProductId, r_lClaimsUserDefTableA:=m_lClaimsUserDefTableA, r_lClaimsUserDefTableB:=m_lClaimsUserDefTableB, r_lClaimsUserDefTableC:=m_lClaimsUserDefTableC, r_lClaimsUserDefTableD:=m_lClaimsUserDefTableD, r_lClaimsUserDefTableE:=m_lClaimsUserDefTableE, r_bDuplicateCheckEnabled:=m_bDuplicateCheckEnabled, r_bDisplayValidVersionEnabled:=m_bDisplayValidVersionEnabled)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If

                'Reset array
                'developer guide no. 146
                Array.Clear(m_vInsuranceFileDetails, 0, m_vInsuranceFileDetails.Length)
            End If


            ' SET 30072002 - Add the toolbar icons
            With Toolbar1

                .ImageList = ImageList1
                .Items.Item("Events").ImageIndex = m_iEvent - 1

                .Items.Item("ClaimVersionsEvents").ImageIndex = m_iEvent - 1
            End With

            If m_sTransactionType = "C_CO" Then
                Toolbar1.Items.Item("ClaimVersionsEvents").Enabled = False
            End If


            ' SET 30072002 - End

            '*****************************************************************
            'PN28240 added History tab to display history
            '*****************************************************************

            uctCLMListPaymentsC1.CountColumn = 15 ''Saurabh LOA010 Claim payment Improvement
            uctCLMListPaymentsC1.ColumnCaption(0) = "Payment_id"
            uctCLMListPaymentsC1.ColumnCaption(1) = "Date"
            uctCLMListPaymentsC1.ColumnCaption(2) = "Party Paid"
            uctCLMListPaymentsC1.ColumnCaption(3) = "Payee"
            uctCLMListPaymentsC1.ColumnCaption(4) = "Amount"
            uctCLMListPaymentsC1.ColumnCaption(5) = "TaxAmount"
            uctCLMListPaymentsC1.ColumnCaption(6) = "Currency"
            uctCLMListPaymentsC1.ColumnCaption(7) = "Loss Amount"
            uctCLMListPaymentsC1.ColumnCaption(8) = "Base Amount"
            'Start(Saurabh Agrawal) LOA010 Tech Spec Claim Payment Improvement
            uctCLMListPaymentsC1.ColumnCaption(9) = "Bank Name"
            uctCLMListPaymentsC1.ColumnCaption(10) = "Bank Account No"
            uctCLMListPaymentsC1.ColumnCaption(11) = "Bank Code"
            uctCLMListPaymentsC1.ColumnCaption(12) = "BIC"
            uctCLMListPaymentsC1.ColumnCaption(13) = "IBAN"
            uctCLMListPaymentsC1.ColumnCaption(14) = "Status"
            'End(Saurabh Agrawal) LOA010 Tech Spec Claim Payment Improvement

            uctCLMListPaymentsC1.ClaimId = g_lClaimID
            uctCLMListPaymentsC1.GetBusiness()

            '****************************************************************
            'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.2.3)
            If m_bShowPaymentView Then
                uctCLMListPaymentsC1.visibleCmdView = True
                uctCLMListPaymentsC1.ShowPaymentView = m_bShowPaymentView
            End If
            'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.2.3)

            'DC281100 do not display "Risk Details" or "Information Checklist" buttons
            '           if not in view mode
            'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.5.1)
            Toolbar1.Items.Item("DocArchive").ImageIndex = m_kTOOLBAR_IMAGE_DOCARCHIVE - 1
            'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.5.1)
            If g_nPMMode <> g_nREADMODE Then

                If g_nPMMode = g_nEDITMODE Then
                    Me.Toolbar1.Items.Item("RiskDetails").Visible = True
                    Toolbar1.Items.Item("RiskDetails").ImageIndex = m_iRisk - 1

                    Toolbar1.Items.Item("Financial").Visible = True
                    Toolbar1.Items.Item("Financial").ImageIndex = m_kTOOLBAR_IMAGE_FINANCIAL - 1

                ElseIf g_nPMMode = g_nADDMODE Then
                    Me.Toolbar1.Items.Item("RiskDetails").Visible = True
                    Toolbar1.Items.Item("RiskDetails").ImageIndex = m_iRisk - 1

                    Toolbar1.Items.Item("Financial").Visible = True
                    Toolbar1.Items.Item("Financial").ImageIndex = m_kTOOLBAR_IMAGE_FINANCIAL - 1

                    Toolbar1.Items.Item("RiskDetails").Enabled = False
                    Toolbar1.Items.Item("Financial").Enabled = False

                Else
                    Me.Toolbar1.Items.Item("RiskDetails").Visible = False
                    Me.Toolbar1.Items.Item("Financial").Visible = False
                End If

                Me.Toolbar1.Items.Item("InformationChecklist").Visible = False
                'Me.Toolbar1.Buttons("Financial").Visible = False

                ' if we are not viewing a claim do not show the events button
                ' as no events will have been created yet for this version of the claim
                Me.Toolbar1.Items.Item("Events").Visible = False
            Else
                Me.Toolbar1.Items.Item("RiskDetails").Visible = True
                'Set the caption
                Me.Toolbar1.Items.Item("RiskDetails").ToolTipText = "Risk Details"
                Me.Toolbar1.Items.Item("InformationChecklist").Visible = True
                ' SET 30072002 - Add the toolbar icons
                Toolbar1.Items.Item("RiskDetails").ImageIndex = m_iRisk - 1
                Toolbar1.Items.Item("InformationChecklist").ImageIndex = m_iInfo - 1
                ' SET 30072002 - End
                Me.Toolbar1.Items.Item("Financial").Visible = True
                Me.Toolbar1.Items.Item("Financial").ImageIndex = m_kTOOLBAR_IMAGE_FINANCIAL - 1
            End If

            m_lReturn = PrepareForm(g_nPMMode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'DD 26/7/2004 - Enable/disable Currency based on Product
            If g_nPMMode = g_nADDMODE Then
                Dim temp_oSIRProduct As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oSIRProduct, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oSIRProduct = temp_oSIRProduct

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If


                m_lReturn = oSIRProduct.GetAllowCurrencyChange(v_lProductId:=CInt(m_vInsuranceFileDetails(ACInsFileProductId, 0)), r_lAllowCurrencyChange:=0, r_lAllowLossCurrencyChange:=lAllowCurrencyChange)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If

                'Enable the currency accordingly.
                cboCurrency.Enabled = (lAllowCurrencyChange = 1)

            Else
                'Currency cannot be changed except on add
                cboCurrency.Enabled = False
            End If


            m_lReturn = g_oBusiness.GetInsurerDetails(g_lPolicyID, g_vClaimDate, IResult, g_nPMMode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                fraCoinsurers.Visible = False
            End If



            'JMK 16/05/2001 - check whether an agent has been found for the policy
            bHasAgent = True
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                bHasAgent = False

                'RKS 15122005 PN25890
                SSTabHelper.SetTabEnabled(tabMainTab, 3, False)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            'TN20010905 - end


            Select Case g_nPMMode
                Case g_nADDMODE

                    'RWH(14/11/2000)
                    m_lClaimStatusonLoad = CLMProvisionalOpenClaim


                    m_lReturn = g_oBusiness.GetClientDetails(g_lPolicyID, g_vClaimDate, CResult)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    End If


                    m_lReturn = g_oBusiness.GetPolicyDetails(InsResult, g_lPolicyID, g_vClaimDate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    End If

                    'Write Policy From and To Date and Currency id


                    g_dtPolicyFromDate = StringsHelper.Format(InsResult(0, 0), ACDateConversion)

                    g_dtPolicyToDate = StringsHelper.Format(InsResult(1, 0), ACDateConversion)

                    g_nCurrencyId = CInt(InsResult(2, 0))

                    'Set Default Currency id equal to policy currency id

                    If cboCurrency.Items.Count > 0 Then

                        For nCount As Integer = 0 To cboCurrency.Items.Count - 1

                            If VB6.GetItemData(cboCurrency, nCount) = g_nCurrencyId Then

                                cboCurrency.SelectedIndex = nCount

                                Exit For

                            End If

                        Next nCount

                    End If

                    'Displaying the Client Details
                    'AR20050303 - PN15644
                    g_lClientAdressCnt = 0

                    g_lClient_AddressId = gPMFunctions.ToSafeLong(CStr(CResult(ACCAddressId)))
                    g_lClient_AddressUsage = 4
                    'Changed the Following Code -Pandu

                    g_sClientName = CStr(CResult(ACCClientName))


                    txtOpenClaim(g_nCLIENT_NAME).Text = CStr(CResult(ACCClientName))

                    g_sClientShortName = CStr(CResult(ACCClientShortName))


                    g_sClientAddress1 = Convert.ToString(CResult(ACCAddress1)).Trim()

                    g_sClientAddress2 = Convert.ToString(CResult(ACCAddress2)).Trim()

                    g_sClientAddress3 = Convert.ToString(CResult(ACCAddress3)).Trim()

                    g_sClientAddress4 = Convert.ToString(CResult(ACCAddress4)).Trim()

                    g_sClientPostCode = Convert.ToString(CResult(ACCPostCode)).Trim()

                    'JMK 01/06/2001


                    g_lClientCountryId = CInt(CResult(ACCCountryId))

                    m_lReturn = g_oBusiness.GetCountryName(g_sClientCountryName, g_lClientCountryId)


                    'JMK 04/06/2001 only display postcode for UK country
                    If g_lClientCountryId <> ACCountryGBR Then
                        txtOpenClaim(g_nCLIENT_ADDRESS).Text = g_sClientAddress1 & " " &
                                                               g_sClientAddress2 & " " & g_sClientAddress3 & " " &
                                                               g_sClientAddress4 & " " & g_sClientCountryName
                    Else
                        txtOpenClaim(g_nCLIENT_ADDRESS).Text = g_sClientAddress1 & " " &
                                                               g_sClientAddress2 & " " & g_sClientAddress3 & " " &
                                                               g_sClientAddress4 & " " & g_sClientPostCode & " " & g_sClientCountryName
                    End If
                  


                    If Strings.Len(Convert.ToString(CResult(ACCTeleHome))) > 0 Then


                        txtOpenClaim(g_nCLIENT_TELNO).Text = CStr(CResult(ACCTeleHome))


                        txtOpenClaim(g_nCLIENT_TELNOOFF).Text = CStr(CResult(ACCTeleOff))


                        txtOpenClaim(g_nCLIENT_FAXNO).Text = CStr(CResult(ACCFax))


                        txtOpenClaim(g_nCLIENT_MOBILENO).Text = CStr(CResult(ACCMobile))


                        txtOpenClaim(g_nCLIENT_EMAIL).Text = CStr(CResult(ACCEmail))
                    Else


                        m_lReturn = g_oBusiness.GetDefaultContacts(v_lPolicyID:=g_lPolicyID, r_vResults:=vClientContact, v_bIsClient:=True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                            Exit Sub
                        End If

                        If Information.IsArray(vClientContact) Then
                            ' Default the client's details through to the screen

                            ' Home Phone Number



                            m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_TELNO), sAreaCode:=CStr(vClientContact(ACConTelephoneHomeArea)), sNumber:=CStr(vClientContact(ACConTelephoneHomeNumber)), sExtension:=CStr(vClientContact(ACConTelephoneHomeExt)))

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                ' Office Phone Number


                                m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_TELNOOFF), sAreaCode:=CStr(vClientContact(ACConTelephoneOfficeArea)), sNumber:=CStr(vClientContact(ACConTelephoneOfficeArea)), sExtension:=CStr(vClientContact(ACConTelephoneOfficeArea)))
                            End If

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                ' Fax Number



                                m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_FAXNO), sAreaCode:=CStr(vClientContact(ACConFaxArea)), sNumber:=CStr(vClientContact(ACConFaxNumber)), sExtension:=CStr(vClientContact(ACConFaxExt)))

                            End If

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                ' Mobile Number



                                m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_MOBILENO), sAreaCode:=CStr(vClientContact(ACConMobileArea)), sNumber:=CStr(vClientContact(ACConMobileNumber)), sExtension:=CStr(vClientContact(ACConMobileExt)))

                            End If

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to format a client phone / fax number", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                Exit Sub
                            End If


                            'developer guide no.(Check for nothing)
                            If Not vClientContact(ACConEmail) Is Nothing Then
                                txtOpenClaim(g_nCLIENT_EMAIL).Text = Convert.ToString(vClientContact(ACConEmail)).Trim()
                            End If
                        End If

                    End If

                    txtClaimNumber.Text = g_sClaimNo


                    m_lPartyCnt = CInt(CResult(ACCPartyCnt))
                    'DC140904 PN14948 allow merge fields to work
                    g_lPartyCnt = m_lPartyCnt

                    'Displaying the Insurer Details
                    'Changed the Following Code -Pandu
                    'JMK 16/05/2001 - check whether an agent has been found for the policy
                    If bHasAgent Then

                        'Start PN48079
                        g_sInsurerContactName = Trim(IResult(ACIInsurerContact))
                        txtOpenClaim(g_nINSURER_CONTACT).Text = g_sInsurerContactName
                        'End PN48079

                        'AR20050303 - PN15644
                        g_lInsurerAdressCnt = 0

                        g_lInsurer_AddressId = gPMFunctions.ToSafeLong(CStr(IResult(ACIAddressId)))
                        g_lInsurer_AddressUsage = 4


                        txtOpenClaim(g_nINSURER_NAME).Text = Convert.ToString(IResult(ACIInsurerName)).Trim()

                        g_sInsurerShortName = Convert.ToString(IResult(ACIInsurerShortName)).Trim()

                        g_sInsurerAddress1 = Convert.ToString(IResult(ACIAddress1)).Trim()

                        g_sInsurerAddress2 = Convert.ToString(IResult(ACIAddress2)).Trim()

                        g_sInsurerAddress3 = Convert.ToString(IResult(ACIAddress3)).Trim()

                        g_sInsurerAddress4 = Convert.ToString(IResult(ACIAddress4)).Trim()

                        g_sInsurerPostCode = Convert.ToString(IResult(ACIPostCode)).Trim()

                        'JMK 01/06/2001


                        g_lInsurerCountryId = CInt(CResult(ACICountryId))


                        'JMK 04/06/2001 only display Postcode if UK
                        If g_lInsurerCountryId <> ACCountryGBR Then
                            txtOpenClaim(g_nINSURER_ADDRESS).Text = g_sInsurerAddress1 & " " &
                                                                    g_sInsurerAddress2 & " " & g_sInsurerAddress3 & " " &
                                                                    g_sInsurerAddress4
                        Else
                            txtOpenClaim(g_nINSURER_ADDRESS).Text = g_sInsurerAddress1 & " " &
                                                                    g_sInsurerAddress2 & " " & g_sInsurerAddress3 & " " &
                                                                    g_sInsurerAddress4 & " " & g_sInsurerPostCode
                        End If


                        If Strings.Len(Convert.ToString(IResult(ACITeleHome))) > 0 Then

                            txtOpenClaim(g_nINSURER_TELNO).Text = Convert.ToString(IResult(ACITeleHome)).Trim()

                            txtOpenClaim(g_nINSURER_FAXNO).Text = Convert.ToString(IResult(ACIFax)).Trim()
                            'txtOpenClaim(g_nINSURER_CONTACT) = IResult(5)


                            txtOpenClaim(g_nINSURER_EMAIL).Text = CStr(IResult(ACIEmail))
                        Else

                            m_lReturn = g_oBusiness.GetDefaultContacts(v_lPolicyID:=g_lPolicyID, r_vResults:=vInsurerContact, v_bIsClient:=False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                                Exit Sub
                            End If

                            If Information.IsArray(vInsurerContact) Then
                                ' Default the client's details through to the screen
                                ' Home Phone Number



                                m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nINSURER_TELNO), sAreaCode:=CStr(vInsurerContact(ACConTelephoneHomeArea)), sNumber:=CStr(vInsurerContact(ACConTelephoneHomeNumber)), sExtension:=CStr(vInsurerContact(ACConTelephoneHomeExt)))

                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    ' Fax Number



                                    m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nINSURER_FAXNO), sAreaCode:=CStr(vInsurerContact(ACConFaxArea)), sNumber:=CStr(vInsurerContact(ACConFaxNumber)), sExtension:=CStr(vInsurerContact(ACConFaxExt)))

                                End If

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to format an insurer phone / fax number", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                    Exit Sub
                                End If


                                txtOpenClaim(g_nINSURER_EMAIL).Text = Convert.ToString(vInsurerContact(ACConEmail)).Trim()
                            End If
                        End If
                    Else
                        'AR20050303 - PN15644
                        g_lInsurerAdressCnt = 0
                        g_lInsurer_AddressId = 0
                        g_lInsurer_AddressUsage = 4

                        txtOpenClaim(g_nINSURER_NAME).Text = ""
                        g_sInsurerShortName = ""
                        g_sInsurerAddress1 = ""
                        g_sInsurerAddress2 = ""
                        g_sInsurerAddress3 = ""
                        g_sInsurerAddress4 = ""
                        g_sInsurerPostCode = ""
                        txtOpenClaim(g_nINSURER_ADDRESS).Text = ""
                        txtOpenClaim(g_nINSURER_TELNO).Text = ""
                        txtOpenClaim(g_nINSURER_FAXNO).Text = ""
                        'txtOpenClaim(g_nINSURER_CONTACT) = IResult(5)
                        txtOpenClaim(g_nINSURER_EMAIL).Text = ""
                    End If

                    'DD 29/03/2004
                    'JMK(28/02/2001) Add Pay mode
                    'DC020403 ISS3153 added view mode
                    'DC081203 PN8955 added EDITADDMODE
                Case g_nEDITMODE, g_nPAYMODE, g_nVIEWMODE, g_nEDITADDMODE

                    m_lReturn = BusinessToInterface()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    End If

                    'Call function to populate policy details in the business object.
                    'These details then get used when the event is created.     PN 17508

                    m_lReturn = g_oBusiness.GetPolicyDetails(InsResult, g_lPolicyID, g_vClaimDate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    End If

                    m_lReturn = g_oBusiness.GetClientDetails(g_lPolicyID, g_vClaimDate, CResult)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    End If


                    m_lPartyCnt = CInt(CResult(ACCPartyCnt))

                    'DC020403 ISS3153 added view mode
                Case g_nREADMODE, g_nVIEWMODE

                    m_lReturn = BusinessToInterface()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    End If

                    'Call BusinessToData(g_sClaimNo)
                    'Call DisplayPolicyDetails

            End Select

            'if underwriting year option is set then make sure claim has underwriting year
            If (m_bOptionUnderwritingYear) And (g_nPMMode <> g_nREADMODE And g_nPMMode <> g_nVIEWMODE And m_sTransactionType <> "C_CP") Then
                If SetUnderWritingYear() <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If
            End If

            'TN20010621 start
            If m_sTransactionType <> "C_CO" And m_sTransactionType <> "C_CR" And g_nInfoOnly = 1 Then

                Toolbar1.Items.Item("RiskDetails").Enabled = False
            End If
            'TN20010621 end

            'S4B Claims Enhancements R&D 2005

            txtCaseNumber.Text = g_sCaseNumber
            If g_nPMMode <> g_nREADMODE And g_nPMMode = g_nADDMODE Then
                txtOpenClaim(g_nLOSS_DATE).Text = ""
                txtOpenClaim(g_nLOSS_TO_DATE).Text = ""

            End If

            If g_nPMMode <> g_nREADMODE Then
                If cboRiskType.Items.Count > 0 Then
                    Dim bIsClaimsMadeExists As Boolean = True
                    For nCount As Integer = 0 To cboRiskType.Items.Count - 1
                        lRiskTypeID = VB6.GetItemData(cboRiskType, nCount)

                        m_lReturn = g_oBusiness.GetClaimTypeAndCover(v_lRiskTypeID:=lRiskTypeID, r_vResults:=oResultArray)

                        If Information.IsArray(oResultArray) Then

                            m_sClaimsTypeBasisCode = gPMFunctions.ToSafeString(oResultArray(kClaimTypeBasis, 0))

                    If g_nPMMode = g_nADDMODE And m_sClaimsTypeBasisCode <> "OCCUR" Then
                                bIsClaimsMadeExists = False
                                Exit For
                    End If
                    End If
                    Next nCount
                    If bIsClaimsMadeExists Then
                        FormatDate(g_vClaimDate, g_nLOSS_DATE)
                        FormatDate(txtOpenClaim(g_nLOSS_DATE).Text, g_nLOSS_TO_DATE)
                End If
            End If
            End If

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=vIsRI2007)


            m_bFormload = True

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayPhoneNumber
    '
    ' Description: Formats the display of a phone number into a single
    '              text field.
    '
    ' ***************************************************************** '
    Private Function DisplayPhoneNumber(ByRef ctlTextField As TextBox, ByVal sAreaCode As String, ByVal sNumber As String, ByVal sExtension As String) As Integer

        Dim result As Integer = 0
        Dim sPhoneNumber As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no.(Put check for nothing)
            'start
            If Not sAreaCode Is Nothing Then
                If sAreaCode.Trim().Length > 0 Then
                    sPhoneNumber = "(" & sAreaCode.Trim() & ") "
                End If
            End If
            If Not sNumber Is Nothing Then
                If sNumber.Trim().Length > 0 Then
                    sPhoneNumber = sPhoneNumber & sNumber.Trim() & " "
                End If
            End If
            If Not sExtension Is Nothing Then
                If sExtension.Trim().Length > 0 Then
                    sPhoneNumber = sPhoneNumber & "ext " & sExtension.Trim()
                End If
            End If
            'end
            ctlTextField.Text = sPhoneNumber

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayPhoneNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayPhoneNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            ' developer guide no. 7
            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()



            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub
    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
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
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
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
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                End Select
            End With

            'developer guide no.293
            'start
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMainTab.SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                tabMainTab.SelectedIndex = 3
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D5 Then
                tabMainTab.SelectedIndex = 4
            End If
            'end
        Catch



            Exit Sub
        End Try


    End Sub

    'To validate email id.
    ''PN_69448 Start
    Private Sub ValidateEmail(ByRef txtEmail As TextBox, ByRef KeyAscii As Integer)
        Dim dbNumericTemp As Double
        If KeyAscii = 8 Then
            Exit Sub
        ElseIf txtEmail.Text.IndexOf("@"c) >= 0 Then
            If txtEmail.Text.Substring(txtEmail.Text.IndexOf("@"c), 1) = Strings.Chr(KeyAscii).ToString() Then
                KeyAscii = 0
            ElseIf Strings.Chr(KeyAscii).ToString() = "." And txtEmail.Text.Trim().Length = (txtEmail.Text.IndexOf("@"c) + 1) Then
                KeyAscii = 0
            End If
        ElseIf txtEmail.Text.Trim().Length = 0 And Not (KeyAscii >= 65 And KeyAscii <= 90 Or KeyAscii >= 97 And KeyAscii <= 122) And Not Double.TryParse(Strings.Chr(KeyAscii).ToString(), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            KeyAscii = 0
        End If
        'developer guide no.(Check for empty string)
        If txtEmail.Text.Trim().Length > 0 Then
            If (StrEmail.IndexOf(Strings.Chr(KeyAscii).ToString()) + 1) = 0 OrElse txtEmail.Text.Trim().Substring(txtEmail.Text.Trim().Length - 1).ToUpper() = Strings.Chr(KeyAscii).ToString().ToUpper() Then
                KeyAscii = 0
            End If
        End If
    End Sub

    Private Function CheckEmailAddress(ByRef txtEmail As TextBox) As Boolean
        Dim result As Boolean = False
        Dim sEmailStr As String = txtEmail.Text.Trim()
        If sEmailStr.Length > 0 And sEmailStr.Trim() <> "NA" Then
            If (sEmailStr.IndexOf("@"c) + 1) = 0 Or (sEmailStr.IndexOf("."c) + 1) = 0 Then
                result = True
            ElseIf sEmailStr.IndexOf("@"c) >= 0 Then
                If (sEmailStr.Substring(sEmailStr.IndexOf("@"c) + 1, sEmailStr.Length - (sEmailStr.IndexOf("@"c) + 1)).IndexOf("."c) + 1) = 0 Then
                    result = True
                ElseIf sEmailStr.EndsWith(".") Then
                    result = True
                ElseIf sEmailStr.IndexOf("@.") >= 0 Then
                    result = True
                End If

            End If
        End If
        If result Then
            MessageBox.Show(" Please Enter a Valid Email Address ", "Email Address Validation", MessageBoxButtons.OK)
            txtEmail.Focus()
            txtEmail.SelectionStart = 0
            txtEmail.SelectionLength = Strings.Len(txtEmail.Text)
        End If
        Return result
    End Function
    ''PN_69448 end

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        'Const ACRenewalPrompt As String = "This policy is in the renewal cycle." & Strings.Chr(13) & Strings.Chr(10) & _
        '                                  "Saving the claim will reset the renewal process to the beginning." & _
        '                                  Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue?"
        'Const ACRenewalTitle As String = "Renewal"

        'Const AC_RENEWAL_PROMPT_2A As String = "Warning - policy is due for renewal in "
        'Const AC_RENEWAL_PROMPT_2B As String = " days. This claim could affect renewal terms." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue?"
        'Const ACEventPrompt As String = "Enter the Event Description"
        'Const ACEventTitle As String = "Event Log"

        Dim sEventDescription As String = ""
        Dim lEventcnt As Integer
        Dim lMsgReturn As DialogResult
        Dim bDeferredRIStatus As Boolean
        Dim lRiskCnt As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sRenewalStatusTypeCode As String = ""
        Dim vArray As Object

        'PN_69448 start
        If CheckEmailAddress(txtOpenClaim(g_nCLIENT_EMAIL)) Then
            Exit Sub
        End If
        If _txtOpenClaim_9.Text.Trim() = String.Empty Then
            MessageBox.Show("Loss To Date is mandatory.", "Invalid LossToDate", MessageBoxButtons.OK, MessageBoxIcon.Information)
            _txtOpenClaim_9.Focus()
            Exit Sub
        End If

        ''PN_69448 end
        Dim bComplete As Boolean = True
        If txtTPA.Tag <> "" Then
            g_iUserOtherPartyID = CInt(txtTPA.Tag)
        Else
            g_iUserOtherPartyID = 0
        End If
        'Start (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (not mentioned in the spec)
        'This part of code is required for setting the claim status (txtOpenClaim).
        If m_sTransactionType <> "C_CO" Then
            If cboProgressStatus.Text = "Closed" Then
                If m_lClaimStatusonLoad = CLMReOpened Then
                    g_lClaimStatusID = CLMReClosed
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_RECLOSED
                Else
                    g_lClaimStatusID = CLMClosed
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_sCLOSED
                End If
            Else
                If m_lClaimStatusonLoad = CLMClosed Or m_lClaimStatusonLoad = CLMReClosed Then
                    g_lClaimStatusID = CLMReOpened
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_REOPENED
                Else
                    g_lClaimStatusID = CLMLiveOpenClaim
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_sLIVEOPENCLAIM
                End If
            End If
        End If
        'End (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (not mentioned in the spec)


        Dim bForceLostFocus As Boolean = iPMFunc.ForceLostFocus(cmdOK)

        If Not bForceLostFocus Then
            Exit Sub
        End If

        ' Click event of the OK button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Check Any Events Have Failed
            Application.DoEvents()

            If g_nPMMode <> g_nREADMODE Then
                bComplete = CheckMandatory()
            End If

            If Not bComplete Then
                Exit Sub
            End If

            If ValidateDates() <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'PN: 73770
            If cboProgressStatus.SelectedIndex <> -1 Then
                m_lReturn = g_oBusiness.GetProgressStatusDetails(iProgressStatusID:=VB6.GetItemData(cboProgressStatus, cboProgressStatus.SelectedIndex), r_vDataArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("cmd_OK Click", "Failed to Get GetProgressStatusDetails")
                End If
                If Information.IsArray(vArray) Then
                    m_iClaimPaymentValid = gPMFunctions.ToSafeInteger(vArray(1, 0), 0)
                End If
            End If

            ' only process claim transaction suppression for underwriting
            ' process claim transaction suppression indicators
            ProcessClaimTransactionSuppression()


            ' only check for duplicate claim when
            ' creating a new claim...
            ' PN22703 - Open up for broking too when in add new claim mode
            If m_sTransactionType = "C_CO" Then

                lReturn = CheckForDuplicateClaims()

                If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Me.Hide()
                    Exit Sub
                ElseIf lReturn = gPMConstants.PMEReturnCode.PMError Then
                    Exit Sub
                End If

            End If

            'DC091003 -PN7298 added underwriting check

            ' AMB 10-Sep-03: 1.8.6 Deferred Reinsurance development - check deferred RI status
            If (g_nPMMode <> g_nREADMODE) Or (g_nPMMode <> g_nVIEWMODE) Then
                'TR - Protect against unselected risk
                If cboRiskType.SelectedIndex <> -1 Then
                    lRiskCnt = VB6.GetItemData(cboRiskType, cboRiskType.SelectedIndex)

                    m_lReturn = g_oBusiness.GetClaimRiskStatus(v_lRiskCnt:=lRiskCnt, r_bIsDeferred:=bDeferredRIStatus)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the risk status for this claim.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If
                    ' Prompt user
                    If bDeferredRIStatus Then
                        If m_sTransactionType = "C_CP" Or m_sTransactionType = "C_SA" Or m_sTransactionType = "C_RV" Then

                            ' If making a claim payment, we should stop the roadmap!

                            lMsgReturn = MessageBox.Show("Payment cannot be processed – reinsurance is deferred.", "Deferred Reinsurance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                            ' Create task
                            m_lReturn = CreateDeferredTask()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create work manager task.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                Exit Sub
                            End If

                            ' Exit
                            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                            Me.Hide()
                            Exit Sub

                        Else
                            ' If NOT making a payment, we simply warn the user

                            lMsgReturn = MessageBox.Show("Reinsurance is deferred on this policy - do you want to continue?", "Deferred Reinsurance", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                            If lMsgReturn = System.Windows.Forms.DialogResult.No Then
                                ' go back to open claim screen
                                Exit Sub
                                ' ElseIf lMsgReturn = vbYes Then
                                '    the user carries on with the roadmap
                            End If
                        End If
                    End If
                End If
            End If
            ' AMB 10-Sep-03: 1.8.6 Deferred Reinsurance development - end



            '     If g_nPMMode = g_nADDMODE Then
            '        If m_bLossDateTime = True Then
            '            MsgBox "Date of Loss amended - has R/I been affected? If Yes, please action via the claim modify option", vbInformation, "Loss Date"
            '        End If
            '     End If

            'JMK (05/04/2001)
            If bComplete And g_nPMMode <> g_nADDMODE Then
                bComplete = CheckUpdatedLossDate()
            End If

            'DC081103 -PN8955 -added EDITADDMODE
            If bComplete And (g_nPMMode = g_nEDITMODE Or g_nPMMode = g_nEDITADDMODE) Then
                bComplete = CheckProgressStatus()
            End If


            If bComplete Then

                If chkVATRegistered.CheckState = CheckState.Checked Then

                    If txtOpenClaim(g_nCLIENT_VAT_REGNO).Text.Trim() = "" Then

                        DisplayMessage(ACEmptyVatMsg, lblVATRegistartionNumber.Name.Substring(3))

                        Exit Sub

                    End If
                    'eck 210803 PN6279
                Else
                    txtOpenClaim(g_nCLIENT_VAT_REGNO).Text = ""
                End If

            End If

            If bComplete Then
                ' Process the next set of actions.
                m_lReturn = m_oGeneral.ProcessCommand()
            Else
                Exit Sub
            End If

            '
            ' JMK 30/05/2001 Claims status should automatically change from provisional to live...
            ' ...if Info Only checkbox is not checked, and claim not Closed

            'DC091003 -PN7298 -reset status according to Information Only checkbox
            'MSS250901 - Added check and section for merge

            '
            '        'DC020403 ISS3153 added check for view mode
            If g_nPMMode <> g_nREADMODE And g_nPMMode <> g_nVIEWMODE Then
                If chkInfoOnly.CheckState = CheckState.Checked Then
                    g_lClaimStatusID = CLMProvisionalOpenClaim
                ElseIf m_lClaimStatusonLoad = CLMProvisionalOpenClaim And g_lClaimStatusID <> CLMClosed Then
                    g_lClaimStatusID = CLMLiveOpenClaim
                End If
            End If
            '     'End of the Changes by Sameer for the Claim Status field on 18-07-00 at 2:45 pm
            '    Else
            '        If g_nPMMode = g_nADDMODE Then
            '            If chkInfoOnly.Value = YesNoCheckyes Then 'And chkLikelyClaim.Value = YesNoCheckYes Then
            '                    g_lClaimStatusID = CLMProvisionalOpenClaim
            '            ElseIf chkInfoOnly.Value = YesNoCheckNo Or chkInfoOnly.Value = YesNoCheckNone Then
            '                    g_lClaimStatusID = CLMLiveOpenClaim
            '            End If
            '        End If
            '    End If
            'MSS250901 - Merge end

            If g_nPMMode = g_nADDMODE Then
                If g_lClientAdressCnt = 0 Then
                    'JMK 04/06/2001 add country id

                    m_lReturn = g_oBusiness.AddAddress(g_sClientAddress1, g_sClientAddress2, g_sClientAddress3, g_sClientAddress4, g_sClientPostCode, g_lClient_AddressUsage, g_lClient_AddressId, g_lClientAdressCnt, g_bClientAddressChanged, g_lClientCountryId)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                End If

                If g_lInsurerAdressCnt = 0 Then
                    'JMK 04/06/2001 add country id

                    'TN20010905 - only save agent address if we have agent on policy
                    If bHasAgent Then

                        m_lReturn = g_oBusiness.AddAddress(g_sInsurerAddress1, g_sInsurerAddress2, g_sInsurerAddress3, g_sInsurerAddress4, g_sInsurerPostCode, g_lInsurer_AddressUsage, g_lInsurer_AddressId, g_lInsurerAdressCnt, g_bInsurerAddressChanged, g_lInsurerCountryId)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                End If

            ElseIf (g_nPMMode = g_nEDITMODE Or g_nPMMode = g_nEDITADDMODE) Then

                'AR20050404 - PN15664
                'Will only need to update address details in Edit mode if user has selected a different Insurer
                If m_bInsurerChanged Then

                    If bHasAgent Then

                        m_lReturn = g_oBusiness.UpdateAddress(g_lInsurerAdressCnt, g_sInsurerAddress1, g_sInsurerAddress2, g_sInsurerAddress3, g_sInsurerAddress4, g_sInsurerPostCode, g_lInsurer_AddressUsage, g_lInsurer_AddressId, g_lInsurerCountryId)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                End If

            End If



            Select Case g_nPMMode
                Case g_nADDMODE


                    'Tomo190402 - start
                    '                InterfaceToBusiness
                    m_lReturn = InterfaceToBusiness()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                    'Tomo190402 - start

                    'RWH(06/03/2001) Claims stores risk_cnt in risk_type_id !!!
                    'DN 27/03/01 - Add SourceID and Language
                    'AB - Added ClaimHandled

                    g_oBusiness.SetProperties(g_nPMMode, g_sClaimNo, g_sPolicyNo, g_lPolicyID, g_sDescription, g_lClaimStatusID, g_lProgressStatusID, g_lPrimaryCauseID, g_lSecondaryCauseID, g_lCatastropheCodeID, g_sLossFromDate, g_sLossToDate, g_sReportedDate, g_sReportedToDate, g_sLastModifiedDate, g_lHandlerID, g_lCurrencyID, g_nInfoOnly, g_nLikelyClaim, g_sLocation, g_lTown, g_lRiskTypeID, g_sClientName, g_sClientAddress, g_sClientTelNo, g_sClientFaxNo, g_sClientMobileNo, g_sClientEMail, g_sClientClaimNo, g_sInsurerName, g_sInsurerAddress, g_sInsurerTelNo, g_sInsurerFaxNo, g_sInsurerEmail, g_sInsurerClaimNo, g_sInsurerContact, g_nVATRegistered, g_sVATRegisteredNo, g_sComments, g_sClaimsStatusDate, g_sClientShortName, g_sInsurerShortName, g_sClientTelNoOff, g_lClaimID, g_lUserDefFldA, g_lUserDefFldB, g_lUserDefFldC, g_lUserDefFldD, g_lUserDefFldE, g_iSourceID, g_iLanguageID, g_vUnderwritingYearID, g_vClaimHandled, g_iUserOtherPartyID, m_lBaseCaseID)
                    'g_oBusiness.SetProperties(g_nPMMode, g_sClaimNo, g_sPolicyNo, g_lPolicyID, g_sDescription, g_lClaimStatusID, g_lProgressStatusID, g_lPrimaryCauseID, g_lSecondaryCauseID, g_lCatastropheCodeID, g_sLossFromDate, g_sLossToDate, g_sReportedDate, g_sReportedToDate, g_sLastModifiedDate, g_lHandlerID, g_lCurrencyID, g_nInfoOnly, g_nLikelyClaim, g_sLocation, g_lTown, g_lRiskTypeID, g_sClientName, g_sClientAddress, g_sClientTelNo, g_sClientFaxNo, g_sClientMobileNo, g_sClientEMail, g_sClientClaimNo, g_sInsurerName, g_sInsurerAddress, g_sInsurerTelNo, g_sInsurerFaxNo, g_sInsurerEmail, g_sInsurerClaimNo, g_sInsurerContact, g_nVATRegistered, g_sVATRegisteredNo, g_sComments, g_sClaimsStatusDate, g_sClientShortName, g_sInsurerShortName, g_sClientTelNoOff, g_lClaimID, g_lUserDefFldA, g_lUserDefFldB, g_lUserDefFldC, g_lUserDefFldD, g_lUserDefFldE, g_iSourceID, g_iLanguageID, g_vUnderwritingYearID, g_vClaimHandled, m_lBaseCaseID, m_iUserOtherPartyID)




                    m_lReturn = g_oBusiness.Add
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed attempting to add claim", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If                   

                    g_lClaimID = g_oBusiness.Claimid
                    'DC290702

                    g_sClaimNo = g_oBusiness.ClaimNo
                    'DC240402

                    'PN32387
                    Select Case (g_sPolicyType)
                        Case ACGeminiIIMotor
                            MessageBox.Show("Please also update the claim details for the policy using Maintain Motor policy details", "Claims", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Case ACGeminiIIHouseHold
                            MessageBox.Show("Please also update the claim details for the policy using Maintain House hold policy details", "Claims", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Select
                    'JMK 30/05/2001 - Underwriting only: event will be created at end of roadmap

                    'JMK(28/02/2001) Add Pay mode

                    'RVH 24/2/2003 - Create peril information if required
                    m_lReturn = AddPerilForClaimRisk(g_lPolicyID, g_lRiskTypeID, g_lClaimID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed attempting to add peril information for claim (AddPerilForClaimRisk)", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If
                    'RVH 24/2/2003 - End

                Case g_nEDITMODE, g_nPAYMODE, g_nEDITADDMODE

                    InterfaceToBusiness()

                    'RWH(06/03/2001) Claims stores risk_cnt in risk_type_id !!!
                    'DN 27/03/01 - Add SourceID and Language
                    'AB - Added ClaimHandled
                    If g_nPMMode = g_nEDITADDMODE Then


                        g_oBusiness.SetProperties(g_nEDITMODE, g_sClaimNo, g_sPolicyNo, g_lPolicyID, g_sDescription, g_lClaimStatusID, g_lProgressStatusID, g_lPrimaryCauseID, g_lSecondaryCauseID, g_lCatastropheCodeID, g_sLossFromDate, g_sLossToDate, g_sReportedDate, g_sReportedToDate, g_sLastModifiedDate, g_lHandlerID, g_lCurrencyID, g_nInfoOnly, g_nLikelyClaim, g_sLocation, g_lTown, g_lRiskTypeID, g_sClientName, g_sClientAddress, g_sClientTelNo, g_sClientFaxNo, g_sClientMobileNo, g_sClientEMail, g_sClientClaimNo, g_sInsurerName, g_sInsurerAddress, g_sInsurerTelNo, g_sInsurerFaxNo, g_sInsurerEmail, g_sInsurerClaimNo, g_sInsurerContact, g_nVATRegistered, g_sVATRegisteredNo, g_sComments, g_sClaimsStatusDate, g_sClientShortName, g_sInsurerShortName, g_sClientTelNoOff, g_lClaimID, g_lUserDefFldA, g_lUserDefFldB, g_lUserDefFldC, g_lUserDefFldD, g_lUserDefFldE, g_iSourceID, g_iLanguageID, g_vUnderwritingYearID, g_vClaimHandled, g_iUserOtherPartyID)
                    Else

                        g_oBusiness.SetProperties(g_nPMMode, g_sClaimNo, g_sPolicyNo, g_lPolicyID, g_sDescription, g_lClaimStatusID, g_lProgressStatusID, g_lPrimaryCauseID, g_lSecondaryCauseID, g_lCatastropheCodeID, g_sLossFromDate, g_sLossToDate, g_sReportedDate, g_sReportedToDate, g_sLastModifiedDate, g_lHandlerID, g_lCurrencyID, g_nInfoOnly, g_nLikelyClaim, g_sLocation, g_lTown, g_lRiskTypeID, g_sClientName, g_sClientAddress, g_sClientTelNo, g_sClientFaxNo, g_sClientMobileNo, g_sClientEMail, g_sClientClaimNo, g_sInsurerName, g_sInsurerAddress, g_sInsurerTelNo, g_sInsurerFaxNo, g_sInsurerEmail, g_sInsurerClaimNo, g_sInsurerContact, g_nVATRegistered, g_sVATRegisteredNo, g_sComments, g_sClaimsStatusDate, g_sClientShortName, g_sInsurerShortName, g_sClientTelNoOff, g_lClaimID, g_lUserDefFldA, g_lUserDefFldB, g_lUserDefFldC, g_lUserDefFldD, g_lUserDefFldE, g_iSourceID, g_iLanguageID, g_vUnderwritingYearID, g_vClaimHandled, g_iUserOtherPartyID)
                    End If


                    If g_lOrPolicyID <> g_lPolicyID Then

                        m_lReturn = g_oBusiness.UpdateClaimPolicyDetails
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                            g_oBusiness.LogMessageToPMMessageTable(v_iType:=gPMConstants.PMELogLevel.PMLogInfo, v_sMsg:="Moved Claim " & g_sClaimNo & " to policy " & g_sPolicyNo, v_sCallingAppName:=m_sCallingAppName, v_vApp:=ACApp, v_vClass:=ACClass, v_vMethod:="cmdOK_Click", v_vErrNo:=Information.Err().Number, v_vErrDesc:=Information.Err().Description)
                        Else
                            gPMFunctions.RaiseError("cmdOk_Click", "Failed to Update Calims", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If


                    m_lReturn = g_oBusiness.Update

                    'PN32387
                    If m_bDataChanged Then
                        Select Case (g_sPolicyType)
                            Case ACGeminiIIMotor
                                MessageBox.Show("Please also update the claim details for the policy using Maintain Motor policy details", "Claims", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Case ACGeminiIIHouseHold
                                MessageBox.Show("Please also update the claim details for the policy using Maintain House hold policy details", "Claims", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End Select
                    End If
                    'JMK 31/05/2001 - Underwriting only: event will be created at end of roadmap

                    'S4B Claim Enhancements R&D 2005

                    'DC020403 ISS3153 added check for view mode
                Case g_nREADMODE, g_nVIEWMODE

                    If m_sTransactionType = "C_SA" Or m_sTransactionType = "C_RV" Then

                        InterfaceToBusiness()


                        g_oBusiness.SetProperties(g_nPMMode, g_sClaimNo, g_sPolicyNo, g_lPolicyID, g_sDescription, g_lClaimStatusID, g_lProgressStatusID, g_lPrimaryCauseID, g_lSecondaryCauseID, g_lCatastropheCodeID, g_sLossFromDate, g_sLossToDate, g_sReportedDate, g_sReportedToDate, g_sLastModifiedDate, g_lHandlerID, g_lCurrencyID, g_nInfoOnly, g_nLikelyClaim, g_sLocation, g_lTown, g_lRiskTypeID, g_sClientName, g_sClientAddress, g_sClientTelNo, g_sClientFaxNo, g_sClientMobileNo, g_sClientEMail, g_sClientClaimNo, g_sInsurerName, g_sInsurerAddress, g_sInsurerTelNo, g_sInsurerFaxNo, g_sInsurerEmail, g_sInsurerClaimNo, g_sInsurerContact, g_nVATRegistered, g_sVATRegisteredNo, g_sComments, g_sClaimsStatusDate, g_sClientShortName, g_sInsurerShortName, g_sClientTelNoOff, g_lClaimID, g_lUserDefFldA, g_lUserDefFldB, g_lUserDefFldC, g_lUserDefFldD, g_lUserDefFldE, g_iSourceID, g_iLanguageID, g_vUnderwritingYearID)



                        m_lReturn = g_oBusiness.Update

                    End If

            End Select

            If m_bCreateDuplicateClaimOverrideEvent Then

                ' create event indicating which user id has been used to override duplicate claim check

                lReturn = g_oBusiness.CreateEvent(r_lEventCnt:=lEventcnt, v_vClaimCnt:=g_lClaimID, v_vDescription:=m_sDuplicateClaimEventDescription, v_lEventTypeId:=kEventTypeNewClaim, v_lPartyid:=m_lPartyCnt)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Duplicate Claim Override Event Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                End If

                ' create claim link

                lReturn = g_oBusiness.AddClaimLink(v_lClaimId:=g_lClaimID, v_lLinkTypeId:=gPMConstants.kWorkClaimLinkTypeEvent, v_lLinkId:=lEventcnt)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClaimLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                End If

            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If g_lClaimStatusID <> CLMClosed Then
                    If m_sTransactionType <> "C_CO" And m_sTransactionType <> "C_CR" And chkInfoOnly.CheckState = CheckState.Checked Then

                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    End If
                End If

                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If



        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            'Start - (Sankar) - (QBENZCR007 Authorise Claim payments  v0 04.doc) - (10)
            If Not ShowPaymentView Then
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
            Else
                Me.Hide()
            End If
            'End - (Sankar) - (QBENZCR007 Authorise Claim payments  v0 04.doc) - (10)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cboPrimaryCausationCode_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPrimaryCausationCode.SelectedIndexChanged

        Dim sSelText As String = ""

        Try

            ' RDT 08/04/03 - start - prompt the user on change of Primary Cause - IAG Spec 215
            'DC081103 -PN8955 -added EDITADDMODE
            If (g_nPMMode = g_nEDITMODE Or g_nPMMode = g_nEDITADDMODE) And (m_bFormload) Then

                MessageBox.Show("Changing the Primary Cause will affect the questions asked subsequently.", "Primary Cause", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If
            ' RDT 08/04/03 - end - prompt the user on change of Primary Cause - IAG Spec 215

            m_lReturn = PopulateSecondaryCause()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Primary Causation Code combobox's click event", vApp:=ACApp, vClass:=ACClass, vMethod:="cboPrimaryCausationCode_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try



    End Sub

    ' ***************************************************************** '
    ' Name:     PopulateSecondaryCause (Private)
    '
    ' Description:  Depending upon the Primary Cause Selected Secondary
    '               causes are filled into the combo
    '
    ' ***************************************************************** '
    Private Function PopulateSecondaryCause() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cboSecondaryCausationCode.Items.Clear()
            cboSecondaryCausationCode.Text = ""

            If Information.IsArray(m_vSecondaryCauseArray) Then

                For nCount As Integer = 0 To m_vSecondaryCauseArray.GetUpperBound(1)

                    If CDbl(m_vSecondaryCauseArray(1, nCount)) = VB6.GetItemData(cboPrimaryCausationCode, cboPrimaryCausationCode.SelectedIndex) Then

                        Dim cboSecondaryCausationCode_NewIndex As Integer = -1
                        cboSecondaryCausationCode_NewIndex = cboSecondaryCausationCode.Items.Add(CStr(m_vSecondaryCauseArray(3, nCount)))
                        VB6.SetItemData(cboSecondaryCausationCode, cboSecondaryCausationCode_NewIndex, CInt(m_vSecondaryCauseArray(0, nCount)))

                    End If
                Next

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process PopulateSecondaryCause", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateSecondaryCause", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:  PopulatePrimaryCause (Private)
    '
    ' Desc:  Load the primary cause combo with valid causes
    '
    ' Auth:  AMB 08/01/03 - Created
    '     :  RDT 08/04/03 - Added to 1.8.6 Branch
    '
    ' ***************************************************************** '
    Private Function PopulatePrimaryCause() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cboPrimaryCausationCode.Items.Clear()

            If Information.IsArray(m_vPrimaryCauseArray) Then

                For nCount As Integer = 0 To m_vPrimaryCauseArray.GetUpperBound(1)
                    Dim cboPrimaryCausationCode_NewIndex As Integer = -1
                    cboPrimaryCausationCode_NewIndex = cboPrimaryCausationCode.Items.Add(CStr(m_vPrimaryCauseArray(1, nCount)))
                    VB6.SetItemData(cboPrimaryCausationCode, cboPrimaryCausationCode_NewIndex, CInt(m_vPrimaryCauseArray(0, nCount)))
                Next

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process PopulatePrimaryCause", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulatePrimaryCause", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:     PrepareForm (Private)
    '
    ' Description:  Depending on the mode of operation enable & disable the
    '               controls
    ' ***************************************************************** '
    Private Function PrepareForm(ByVal vMode As Integer) As Integer

        Dim result As Integer = 0
        Dim sOptionValue As String = ""
        Const kMethodName As String = "PrepareForm"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC241001 -start -do not display option to find insurer for UW

            cmdInsurerDetails.Visible = False


            If Not (m_sTransactionType = "C_CR") Then
                cmdChangeClientPolicy.Visible = False
            End If
            'DC241001 -end

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                vMode = g_nREADMODE
            End If


            Select Case vMode
                Case g_nADDMODE

                    'Tomo190402

                    txtClaimNumber.Enabled = True
                    txtOpenClaim(g_nDESCRIPTION).Enabled = True
                    txtOpenClaim(g_nLOCATION).Enabled = True
                    txtOpenClaim(g_nCLIENT_NAME).Enabled = False
                    txtOpenClaim(g_nCLIENT_ADDRESS).Enabled = False
                    txtOpenClaim(g_nINSURER_NAME).Enabled = False
                    txtOpenClaim(g_nINSURER_ADDRESS).Enabled = False
                    'Added -Pandu

                    txtOpenClaim(g_nCLAIM_STATUS_DATE).Enabled = False
                    txtOpenClaim(g_nCLAIM_STATUS_TIME).Enabled = False

                    'Setting Default Values to Date Field
                    g_dtReportedDate = StringsHelper.Format(DateTime.Today, ACDateConversion)
                    g_dtReportedToDate = StringsHelper.Format(DateTime.Today, ACDateConversion)
                    g_dtLossDate = StringsHelper.Format(DateTime.Today, ACDateConversion)
                    g_dtLossToDate = StringsHelper.Format(DateTime.Today, ACDateConversion)
                    g_dtClaimStatusDate = StringsHelper.Format(DateTime.Today, ACDateConversion)

                    g_dtLossTime = DateTime.Now.ToString("HH:mm")
                    g_dtReportedTime = DateTime.Now.ToString("HH:mm")
                    g_dtReportedToTime = DateTime.Now.ToString("HH:mm")

                    txtOpenClaim(g_nLOSS_TIME).Text = DateTime.Now.ToString("HH:mm")
                    txtOpenClaim(g_nREPORTED_TIME).Text = DateTime.Now.ToString("HH:mm")
                    txtOpenClaim(g_nREPORTED_TO_TIME).Text = DateTime.Now.ToString("HH:mm")

                    '                FormatDate CStr(g_dtLossDate), g_nLOSS_DATE
                    FormatDate(g_vClaimDate, g_nLOSS_DATE)
                    g_dtLossToDate = g_vClaimDate
                    FormatDate(g_dtLossToDate, g_nLOSS_TO_DATE)
                    FormatDate(g_dtReportedDate, g_nREPORTED_DATE)
                    FormatDate(g_dtReportedToDate, g_nREPORTED_TO_DATE)
                    FormatDate(g_dtClaimStatusDate, g_nCLAIM_STATUS_DATE)

                    txtOpenClaim(g_nCLAIM_STATUS_TIME).Text = DateTime.Now.ToString("HH:mm")

                    chkVATRegistered.CheckState = CheckState.Unchecked
                    chkInfoOnly.CheckState = CheckState.Unchecked
                    chkLikelyClaim.CheckState = CheckState.Unchecked
                    cboHandler.Enabled = True
                    cboProgressStatus.Enabled = True
                    'DC280601 default for infoonly flag is disabled
                    chkLikelyClaim.Enabled = False

                    txtOpenClaim(g_nCLAIM_STATUS_DATE).Enabled = False
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_sPROVISIONALOPENCLAIM
                    txtOpenClaim(g_nCLIENT_VAT_REGNO).Enabled = False

                    'KB PN 3680 for Broking we will have an autogenerated claim no
                    ' and we dont want to change it
                    'PSL 02/05/2003 3892 Don't do it if it is underwriting
                Case g_nEDITMODE

                    'Tomo190402
                    txtClaimNumber.Enabled = False

                    'JMK(05/04/2001) bug#464 Claim Details tab field
                    'extra fields enabled in Edit Mode

                    'disable all txtOpenClaim() for Claim Details
                    'START
                    For nCount As Integer = 0 To 13
                        txtOpenClaim(nCount).Enabled = False
                    Next nCount
                    'enable the ones we want
                    'JMK 04/06/2001 bug #746
                    txtOpenClaim(g_nDESCRIPTION).Enabled = True
                    txtOpenClaim(g_nLOCATION).Enabled = True
                    m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSystemOptionEnableLossDateonClaim, r_sOptionValue:=sOptionValue, v_iSourceID:=g_oObjectManager.SourceID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed for option_number :- " & kSystemOptionEnableLossDateonClaim, gPMConstants.PMELogLevel.PMLogError)
                    End If
                    If sOptionValue = "1" Then
                        txtOpenClaim(g_nLOSS_DATE).Enabled = False
                        txtOpenClaim(g_nLOSS_TIME).Enabled = False
                        txtOpenClaim(g_nLOSS_TO_DATE).Enabled = False
                    Else
                    txtOpenClaim(g_nLOSS_DATE).Enabled = True
                    txtOpenClaim(g_nLOSS_TIME).Enabled = True
                    txtOpenClaim(g_nLOSS_TO_DATE).Enabled = True
                    End If
                    'DC070203
                    'IS1910
                    'make reported dates editable
                    txtOpenClaim(g_nREPORTED_DATE).Enabled = True
                    txtOpenClaim(g_nREPORTED_TIME).Enabled = True
                    txtOpenClaim(g_nREPORTED_TO_DATE).Enabled = True
                    txtOpenClaim(g_nREPORTED_TO_TIME).Enabled = True

                    'disable combos

                    'DC270601 make certain fields enabled for Broking

                    ' RDT 08/04/03 - start - do not disable Primary/Secondary Cause combo - IAG 215 spec
                    cboPrimaryCausationCode.Enabled = True
                    cboSecondaryCausationCode.Enabled = True
                    ' RDT 08/04/03 - End
                    chkLikelyClaim.Enabled = False

                    cboRiskType.Enabled = False
                    'enable combos we want
                    cboProgressStatus.Enabled = True
                    cboCatastropheCode.Enabled = True
                    cboHandler.Enabled = True
                    cboTown.Enabled = True

                    'disable checkboxes
                    'JMK 19/05/2001 Info Only enabled during Maintain Claim
                    'chkInfoOnly.Enabled = False
                    chkInfoOnly.Enabled = True
                    'eck 210803 PN6279
                    'chkVATRegistered.Enabled = False
                    chkVATRegistered.Enabled = True


                    'fields on other tabs - left alone
                    'JMK(05/05/2001) END
                    txtOpenClaim(g_nCLIENT_NAME).Enabled = False
                    txtOpenClaim(g_nCLIENT_ADDRESS).Enabled = False
                    txtOpenClaim(g_nCLIENT_CLAIMNO).Enabled = True
                    txtOpenClaim(g_nINSURER_NAME).Enabled = False
                    txtOpenClaim(g_nINSURER_ADDRESS).Enabled = False


                    'JMK(28/02/2001) Add Pay mode
                    'To allow enabling of the following as per spec:
                    '   Reported To Date    ?spec
                    '   Catastrophe Code    spec
                    '   Progress Status     RSA bug 20
                    '   Comments            RSA bug 21
                    '   User defined fields spec
                    'START
                Case g_nPAYMODE

                    'Tomo190402
                    txtClaimNumber.Enabled = False

                    'disable all txtOpenClaim()
                    For nCount As Integer = 0 To 45
                        If nCount <> 14 Then 'txtOpenClaim(14) does not exist
                            txtOpenClaim(nCount).Enabled = False
                        End If
                    Next nCount

                    'Need to clarify this before uncommenting
                    '                'enable the ones we want
                    '                txtOpenClaim(g_nREPORTED_TO_DATE).Enabled = True
                    '                txtOpenClaim(g_nREPORTED_TO_TIME).Enabled = True
                    'JMK 04/06/2001 bug #746
                    txtOpenClaim(g_nDESCRIPTION).Enabled = True

                    'disable combos
                    cboHandler.Enabled = False
                    cboPrimaryCausationCode.Enabled = False
                    cboRiskType.Enabled = False
                    cboSecondaryCausationCode.Enabled = False
                    cboTown.Enabled = False
                    'S4B Claim Enhancements R&D 2005
                    cboAtFault.Enabled = False
                    cboStandardExcess.Enabled = False
                    ddDriverTitle.Enabled = False
                    ddEmployeeTitle.Enabled = False

                    'enable combos we want
                    cboProgressStatus.Enabled = True
                    cboCatastropheCode.Enabled = True

                    'enable user defined fields
                    cboUDFA.Enabled = True
                    cboUDFB.Enabled = True
                    cboUDFC.Enabled = True
                    cboUDFD.Enabled = True
                    cboUDFE.Enabled = True

                    'enable comments
                    'txtComments.Enabled = True

                    'disable checkboxes
                    chkInfoOnly.Enabled = False
                    chkVATRegistered.Enabled = False
                    chkLikelyClaim.Enabled = False
                    'S4B Claim Enhancements R&D 2005
                    chkBonusAffected.Enabled = False
                    chkPreviousClaim.Enabled = False
                    chkSolicitorAppointed.Enabled = False
                    chkULR.Enabled = False

                    cmdTPA.Enabled = False


                    'JMK END

                    'DC020403 ISS3153 added check for view mode
                    cboUnderwritingYearID.Enabled = False
                Case g_nREADMODE, g_nVIEWMODE

                    'Tomo190402
                    txtClaimNumber.Enabled = False

                    ' No value is allowed to be changed so lock all the controls

                    txtOpenClaim(g_nCLAIM_STATUS).Enabled = False

                    For nCount As Integer = 2 To 4

                        txtOpenClaim(nCount).Enabled = False

                    Next nCount

                    txtOpenClaim(6).Enabled = False
                    txtOpenClaim(8).Enabled = False

                    For nCount As Integer = 10 To 13

                        txtOpenClaim(nCount).Enabled = False

                    Next nCount

                    For nCount As Integer = 15 To 45

                        txtOpenClaim(nCount).Enabled = False

                    Next nCount

                    txtOpenClaim(g_nLOSS_DATE).Enabled = False
                    txtOpenClaim(g_nLOSS_TIME).Enabled = False
                    txtOpenClaim(g_nREPORTED_DATE).Enabled = False
                    txtOpenClaim(g_nREPORTED_TIME).Enabled = False
                    txtOpenClaim(g_nLOSS_TO_DATE).Enabled = False
                    txtOpenClaim(g_nCLAIM_STATUS).Enabled = False
                    txtOpenClaim(g_nCLAIM_STATUS_DATE).Enabled = False
                    txtOpenClaim(g_nCLAIM_STATUS_TIME).Enabled = False

                    txtOpenClaim(g_nCLIENT_NAME).Enabled = False
                    txtOpenClaim(g_nCLIENT_ADDRESS).Enabled = False
                    txtOpenClaim(g_nINSURER_NAME).Enabled = False
                    txtOpenClaim(g_nINSURER_ADDRESS).Enabled = False

                    ' Don't disable the comments textbox as we won't be able to scroll then to
                    ' read it in view only mode!  PN16040
                    'txtComments.Locked = True
                    'txtComments.ForeColor = vbGrayText

                    CmdClient.Enabled = False
                    cmdInsurer.Enabled = False
                    'DC241001
                    cmdInsurerDetails.Enabled = False

                    'chkInfoOnly.Value = YesNoCheckNo
                    'chkVATRegistered.Value = YesNoCheckYes
                    'chkLikelyClaim.Value = YesNoCheckYes

                    chkInfoOnly.Enabled = False
                    chkVATRegistered.Enabled = False
                    chkLikelyClaim.Enabled = False
                    'S4B Claim Enhancements R&D 2005
                    chkBonusAffected.Enabled = False
                    chkPreviousClaim.Enabled = False
                    chkSolicitorAppointed.Enabled = False
                    chkULR.Enabled = False

                    cboHandler.Enabled = False
                    '                cboProgressStatus.Enabled = False
                    'DC020403 ISS3153 added check for view mode
                    cboProgressStatus.Enabled = Not (m_iTask = gPMConstants.PMEComponentAction.PMView Or g_nPMMode = g_nVIEWMODE)
                    cboTown.Enabled = False
                    cboPrimaryCausationCode.Enabled = False
                    cboSecondaryCausationCode.Enabled = False
                    cboCatastropheCode.Enabled = False
                    cboRiskType.Enabled = False

                    cboUDFA.Enabled = False
                    cboUDFB.Enabled = False
                    cboUDFC.Enabled = False
                    cboUDFD.Enabled = False
                    cboUDFE.Enabled = False

                    cboUnderwritingYearID.Enabled = False

                    'S4B Claim Enhancements R&D 2005
                    cboAtFault.Enabled = False
                    cboStandardExcess.Enabled = False
                    ddDriverTitle.Enabled = False
                    ddEmployeeTitle.Enabled = False
                    chkClaimHandled.Enabled = False
                    cmdTPA.Enabled = False
            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failure to PrepareForm", vApp:=ACApp, vClass:=ACClass, vMethod:="PrepareForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    'Private Sub mnuFileExit_Click()
    'Me.Close()
    'End Sub

    Private Sub CmdClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdClient.Click
        Dim oAddress As iCLMAddress.Interface_Renamed
        Dim lAddressUsageTypeID As Integer
        Dim sAddressUsageType As String = ""
        Dim vIResult, vClientContact As Object
        Dim bRefresh As Boolean

        Const ACConTelephoneHomeArea As Integer = 0
        Const ACConTelephoneHomeNumber As Integer = 1
        Const ACConTelephoneHomeExt As Integer = 2

        Const ACConTelephoneOfficeArea As Integer = 3
        'Const ACConTelephoneOfficeNumber As Integer = 4
        'Const ACConTelephoneOfficeExt As Integer = 5

        Const ACConFaxArea As Integer = 6
        Const ACConFaxNumber As Integer = 7
        Const ACConFaxExt As Integer = 8

        Const ACConMobileArea As Integer = 9
        Const ACConMobileNumber As Integer = 10
        Const ACConMobileExt As Integer = 11

        Const ACConEmail As Integer = 12

        Try


            Dim temp_oAddress As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAddress, sClassName:="iCLMAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oAddress = temp_oAddress

            'm_lReturn& = oAddress.SetProcessModes(vTask:=PMEdit)
            If g_lClientAdressCnt = 0 Then

                'developer guide no.9
                oAddress.Initialise()

                oAddress.Address1 = g_sClientAddress1

                oAddress.Address2 = g_sClientAddress2

                oAddress.Address3 = g_sClientAddress3

                oAddress.Address4 = g_sClientAddress4

                oAddress.PostalCode = g_sClientPostCode
                'AR20050303 - PN15644

                oAddress.AddressId = g_lClient_AddressId

                'JMK


                oAddress.CountryID = g_lClientCountryId

                oAddress.Task = gPMConstants.PMEComponentAction.PMAdd

                oAddress.Start()

                'AR20050303 - PN15644

                If oAddress.Status = gPMConstants.PMEReturnCode.PMOK Then

                    bRefresh = True


                    g_bClientAddressChanged = oAddress.AddressChanged


                    g_lClientAdressCnt = oAddress.AddressCnt

                    g_sClientAddress1 = oAddress.Address1

                    g_sClientAddress2 = oAddress.Address2

                    g_sClientAddress3 = oAddress.Address3

                    g_sClientAddress4 = oAddress.Address4

                    g_sClientPostCode = oAddress.PostalCode

                    'AR20050303 - PN15644

                    g_lClient_AddressId = oAddress.AddressId

                    g_lClient_AddressUsage = oAddress.AddressUsageTypeID

                    'JMK


                    g_lClientCountryId = oAddress.CountryID

                End If


                oAddress.Dispose()

            Else


                CType(oAddress, SSP.S4I.Interfaces.ILocalInterface).Initialise()

                oAddress.AddressCnt = g_lClientAdressCnt
                'AR20050303 - PN15644

                oAddress.AddressId = g_lClient_AddressId


                oAddress.Task = gPMConstants.PMEComponentAction.PMEdit

                oAddress.Start()

                'AR20050303 - PN15644

                If oAddress.Status = gPMConstants.PMEReturnCode.PMOK Then

                    bRefresh = True


                    g_sClientAddress1 = oAddress.Address1

                    g_sClientAddress2 = oAddress.Address2

                    g_sClientAddress3 = oAddress.Address3

                    g_sClientAddress4 = oAddress.Address4

                    g_sClientPostCode = oAddress.PostalCode

                    'AR20050303 - PN15644

                    g_lClient_AddressId = oAddress.AddressId

                    g_lClient_AddressUsage = oAddress.AddressUsageTypeID

                    'JMK


                    g_lClientCountryId = oAddress.CountryID
                End If


                oAddress.Dispose()

            End If

            'AR20050303 - PN15644
            If bRefresh Then

                'JMK 04/06/2001 Postcode UK only
                If g_lClientCountryId <> ACCountryGBR Then

                    m_lReturn = g_oBusiness.GetCountryName(g_sClientCountryName, g_lClientCountryId)
                    txtOpenClaim(g_nCLIENT_ADDRESS).Text = g_sClientAddress1 & " " &
                                                           g_sClientAddress2 & " " & g_sClientAddress3 & " " &
                                                           g_sClientAddress4 & " " & g_sClientCountryName
                Else
                    txtOpenClaim(g_nCLIENT_ADDRESS).Text = g_sClientAddress1 & " " &
                                                           g_sClientAddress2 & " " & g_sClientAddress3 & " " &
                                                           g_sClientAddress4 & " " & g_sClientPostCode

                    'DJM 30/04/2002 : Check contact details on form against contact
                    '                 details of selected address type on DB.

                    m_lReturn = g_oBusiness.GetPartyDetails(g_sClientShortName, lAddressUsageTypeID, vIResult)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        'If found then overwrite
                        '            txtOpenClaim(g_nCLIENT_TELNO) = Trim(vIResult(ACCTeleHome))
                        '            txtOpenClaim(g_nCLIENT_TELNOOFF) = Trim(vIResult(ACCTeleOff))
                        '            txtOpenClaim(g_nCLIENT_FAXNO) = Trim(vIResult(ACCFax))
                        '            txtOpenClaim(g_nCLIENT_MOBILENO) = Trim(vIResult(ACCMobile))
                        '            txtOpenClaim(g_nCLIENT_EMAIL) = Trim(vIResult(ACCEmail))

                        If Strings.Len(Convert.ToString(vIResult(ACCTeleHome))) > 0 Then


                            txtOpenClaim(g_nCLIENT_TELNO).Text = CStr(vIResult(ACCTeleHome))


                            txtOpenClaim(g_nCLIENT_TELNOOFF).Text = CStr(vIResult(ACCTeleOff))


                            txtOpenClaim(g_nCLIENT_FAXNO).Text = CStr(vIResult(ACCFax))


                            txtOpenClaim(g_nCLIENT_MOBILENO).Text = CStr(vIResult(ACCMobile))


                            txtOpenClaim(g_nCLIENT_EMAIL).Text = CStr(vIResult(ACCEmail))
                        Else


                            m_lReturn = g_oBusiness.GetDefaultContacts(v_lPolicyID:=g_lPolicyID, r_vResults:=vClientContact, v_bIsClient:=True)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                                Exit Sub
                            End If

                            If Information.IsArray(vClientContact) Then
                                ' Default the client's details through to the screen

                                ' Home Phone Number



                                m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_TELNO), sAreaCode:=CStr(vClientContact(ACConTelephoneHomeArea)), sNumber:=CStr(vClientContact(ACConTelephoneHomeNumber)), sExtension:=CStr(vClientContact(ACConTelephoneHomeExt)))

                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    ' Office Phone Number


                                    m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_TELNOOFF), sAreaCode:=CStr(vClientContact(ACConTelephoneOfficeArea)), sNumber:=CStr(vClientContact(ACConTelephoneOfficeArea)), sExtension:=CStr(vClientContact(ACConTelephoneOfficeArea)))
                                End If

                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    ' Fax Number



                                    m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_FAXNO), sAreaCode:=CStr(vClientContact(ACConFaxArea)), sNumber:=CStr(vClientContact(ACConFaxNumber)), sExtension:=CStr(vClientContact(ACConFaxExt)))

                                End If

                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    ' Mobile Number



                                    m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_MOBILENO), sAreaCode:=CStr(vClientContact(ACConMobileArea)), sNumber:=CStr(vClientContact(ACConMobileNumber)), sExtension:=CStr(vClientContact(ACConMobileExt)))

                                End If

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to format a client phone / fax number", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                    Exit Sub
                                End If


                                txtOpenClaim(g_nCLIENT_EMAIL).Text = Convert.ToString(vClientContact(ACConEmail)).Trim()
                            End If
                        End If


                    End If

                End If

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdclient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description:  To write the details from the Business to Local variables
    '               INPUT : Claim Number
    '
    ' ***************************************************************** '
    Public Function BusinessToData() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the Claim ID in the Business Object

            g_oBusiness.SetKeyID(g_lClaimID)

            'Select the Claim from the Database

            g_oBusiness.SelectSingle()

            'Added New Fetch values

            'RWH(06/03/2001) Claims stores risk_cnt in risk_type_id !!!
            'DN 27/03/01 - Added SourceID and LanguageID
            'Get the Values into the Local variables
            'DC081203 PN8955 -treat EDITADDMODE as EDITMODE
            'AB - Added ClaimHandled
            If g_nPMMode = g_nEDITADDMODE Then


                g_oBusiness.GetProperties(g_nEDITMODE, g_sClaimNo, g_sPolicyNo, g_lPolicyID, g_sDescription, g_lClaimStatusID, g_lProgressStatusID, g_lPrimaryCauseID, g_lSecondaryCauseID, g_lCatastropheCodeID, g_sLossFromDate, g_sLossToDate, g_sReportedDate, g_sReportedToDate, g_sLastModifiedDate, g_lHandlerID, g_lCurrencyID, g_nInfoOnly, g_nLikelyClaim, g_sLocation, g_lTown, g_lRiskTypeID, g_sClientName, g_sClientAddress, g_sClientTelNo, g_sClientFaxNo, g_sClientMobileNo, g_sClientEMail, g_sClientClaimNo, g_sInsurerName, g_sInsurerAddress, g_sInsurerTelNo, g_sInsurerFaxNo, g_sInsurerEmail, g_sInsurerClaimNo, g_sInsurerContact, g_nVATRegistered, g_sVATRegisteredNo, g_sComments, g_sClaimsStatusDate, g_sClientShortName, g_sInsurerShortName, g_sClientTelNoOff, g_lUserDefFldA, g_lUserDefFldB, g_lUserDefFldC, g_lUserDefFldD, g_lUserDefFldE, g_iSourceID, g_iLanguageID, g_vUnderwritingYearID, g_lVersionId, g_vClaimHandled, g_sCaseNumber, g_lCaseID, g_iUserOtherPartyID, g_sUserOtherPartyname)
            Else

                g_oBusiness.GetProperties(g_nPMMode, g_sClaimNo, g_sPolicyNo, g_lPolicyID, g_sDescription, g_lClaimStatusID, g_lProgressStatusID, g_lPrimaryCauseID, g_lSecondaryCauseID, g_lCatastropheCodeID, g_sLossFromDate, g_sLossToDate, g_sReportedDate, g_sReportedToDate, g_sLastModifiedDate, g_lHandlerID, g_lCurrencyID, g_nInfoOnly, g_nLikelyClaim, g_sLocation, g_lTown, g_lRiskTypeID, g_sClientName, g_sClientAddress, g_sClientTelNo, g_sClientFaxNo, g_sClientMobileNo, g_sClientEMail, g_sClientClaimNo, g_sInsurerName, g_sInsurerAddress, g_sInsurerTelNo, g_sInsurerFaxNo, g_sInsurerEmail, g_sInsurerClaimNo, g_sInsurerContact, g_nVATRegistered, g_sVATRegisteredNo, g_sComments, g_sClaimsStatusDate, g_sClientShortName, g_sInsurerShortName, g_sClientTelNoOff, g_lUserDefFldA, g_lUserDefFldB, g_lUserDefFldC, g_lUserDefFldD, g_lUserDefFldE, g_iSourceID, g_iLanguageID, g_vUnderwritingYearID, g_lVersionId, g_vClaimHandled, g_sCaseNumber, g_lCaseID, g_iUserOtherPartyID, g_sUserOtherPartyname)

            End If

            'Write Status Id on Load to Local Variable for verification if it is changed during edit
            m_lClaimStatusonLoad = g_lClaimStatusID
            m_bClaimNcdBonusOnLoad = g_bBonusAffected

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdInsurer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsurer.Click

        Dim oInsAddress As iCLMAddress.Interface_Renamed
        Dim lAddressUsageTypeID As Integer
        Dim sAddressUsageType As String = ""
        Dim vIResult As Object
        Dim bRefresh As Boolean

        Try

            Dim temp_oInsAddress As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oInsAddress, sClassName:="iCLMAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oInsAddress = temp_oInsAddress

            'm_lReturn& = oAddress.SetProcessModes(vTask:=PMEdit)

            If g_lInsurerAdressCnt = 0 Then


                CType(oInsAddress, SSP.S4I.Interfaces.ILocalInterface).Initialise()

                oInsAddress.Address1 = g_sInsurerAddress1

                oInsAddress.Address2 = g_sInsurerAddress2

                oInsAddress.Address3 = g_sInsurerAddress3

                oInsAddress.Address4 = g_sInsurerAddress4

                oInsAddress.PostalCode = g_sInsurerPostCode
                'AR20050303 - PN15644

                oInsAddress.AddressId = g_lInsurer_AddressId
                'JMK 04/06/2001 add Country id

                oInsAddress.CountryID = g_lInsurerCountryId


                oInsAddress.Task = gPMConstants.PMEComponentAction.PMAdd

                oInsAddress.Start()

                'AR20050303 - PN15644

                If oInsAddress.Status = gPMConstants.PMEReturnCode.PMOK Then

                    bRefresh = True
                    m_bInsurerChanged = False

                    'PN28530
                    g_bInsurerAddressChanged = False 'oInsAddress.AddressChanged


                    g_lInsurerAdressCnt = oInsAddress.AddressCnt

                    g_sInsurerAddress1 = oInsAddress.Address1

                    g_sInsurerAddress2 = oInsAddress.Address2

                    g_sInsurerAddress3 = oInsAddress.Address3

                    g_sInsurerAddress4 = oInsAddress.Address4

                    g_sInsurerPostCode = oInsAddress.PostalCode

                    'AR20050303 - PN15644

                    g_lInsurer_AddressId = oInsAddress.AddressId

                    g_lInsurer_AddressUsage = oInsAddress.AddressUsageTypeID

                    'JMK 04/06/2001 add Country id

                    g_lInsurerCountryId = oInsAddress.CountryID


                End If


                oInsAddress.Dispose()

            Else


                CType(oInsAddress, SSP.S4I.Interfaces.ILocalInterface).Initialise()

                oInsAddress.AddressCnt = g_lInsurerAdressCnt
                'AR20050303 - PN15644

                oInsAddress.AddressId = g_lInsurer_AddressId


                oInsAddress.Task = gPMConstants.PMEComponentAction.PMEdit

                oInsAddress.Start()


                If oInsAddress.Status = gPMConstants.PMEReturnCode.PMOK Then

                    bRefresh = True


                    g_sInsurerAddress1 = oInsAddress.Address1

                    g_sInsurerAddress2 = oInsAddress.Address2

                    g_sInsurerAddress3 = oInsAddress.Address3

                    g_sInsurerAddress4 = oInsAddress.Address4

                    g_sInsurerPostCode = oInsAddress.PostalCode

                    'AR20050303 - PN15644

                    g_lInsurer_AddressId = oInsAddress.AddressId

                    g_lInsurer_AddressUsage = oInsAddress.AddressUsageTypeID

                    'JMK 04/06/2001 add Country id


                    g_lInsurerCountryId = oInsAddress.CountryID


                End If

                oInsAddress.Dispose()

            End If

            If bRefresh Then

                'JMK 04/06/2001 Postcode UK only
                If g_lInsurerCountryId <> ACCountryGBR Then
                    txtOpenClaim(g_nINSURER_ADDRESS).Text = g_sInsurerAddress1 & " " &
                                                            g_sInsurerAddress2 & " " & g_sInsurerAddress3 & " " &
                                                            g_sInsurerAddress4
                Else
                    txtOpenClaim(g_nINSURER_ADDRESS).Text = g_sInsurerAddress1 & " " &
                                                            g_sInsurerAddress2 & " " & g_sInsurerAddress3 & " " &
                                                            g_sInsurerAddress4 & " " & g_sInsurerPostCode

                    'DJM 07/05/2002 : Check contact details on form against contact
                    '                 details of selected address type on DB.

                    m_lReturn = g_oBusiness.GetPartyDetails(g_sInsurerShortName, lAddressUsageTypeID, vIResult)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        'If found then overwrite

                        txtOpenClaim(g_nINSURER_TELNO).Text = Convert.ToString(vIResult(12)).Trim() 'Business telephone, not home telephone

                        txtOpenClaim(g_nINSURER_FAXNO).Text = Convert.ToString(vIResult(ACCFax)).Trim()

                        txtOpenClaim(g_nINSURER_EMAIL).Text = Convert.ToString(vIResult(ACCEmail)).Trim()
                    End If

                End If

            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdinsurer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ClaimVersionsEvents.Click, _Toolbar1_Button2.Click, Events.Click, _Toolbar1_Button4.Click, RiskDetails.Click, _Toolbar1_Button6.Click, InformationChecklist.Click, _Toolbar1_Button8.Click, Financial.Click, _Toolbar1_Button10.Click, DocArchive.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)
        Dim vKeyArray(,) As String
        ReDim vKeyArray(1, 9)

        Dim oPMUPolicy As bPMUPolicy.Business


        Dim oRiskDetails As iCLMRiskDetails.Interface_Renamed

        Dim oInfoChecklist As iCLMInfoChklst.Interface_Renamed

        Dim oFinancialSummary As iCLMFinSumm.Interface_Renamed
        Dim vClientCode As Object
        Dim sClientCode As String = ""
        Dim sOption, sSPUrl, sDocLIB As String
        Select Case Button.Name
            Case "ClaimVersionsEvents"
                ShowRiskEvents(v_bShowAllClaimVersionsEvents:=True)

            Case "Events"
                ShowRiskEvents()

                'DC281100 processing for Risk Details & Information Checklist buttons
            Case "RiskDetails"

                'disable it - stop user from clicking on it repeatedly
                Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = False

                Dim temp_oRiskDetails As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oRiskDetails, sClassName:="iCLMRiskDetails.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oRiskDetails = temp_oRiskDetails

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iCLMRiskDetails.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar1_ButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                    Exit Sub
                End If

                If oRiskDetails Is Nothing Then Exit Sub



                'RWH(06/03/2001) Claims stores risk_cnt in risk_type_id !!!

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameRiskTypeID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = g_lRiskTypeID


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClaimCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = g_lClaimID


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimDate

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = g_vClaimDate


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameOperateMode
                'DC081203 -PN8955 -change back to ADD mode for rest of roadmap
                If g_nPMMode = g_nEDITADDMODE Then
                    g_nPMMode = g_nADDMODE
                End If


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = g_nPMMode


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNamePolicyID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = g_lPolicyID


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNamePolicyNumber

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = g_sPolicyNo


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClientHolder

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = g_sClientName


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = "DeleteWorkTableFlag"

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = gPMConstants.PMEReturnCode.PMFalse

                'DC140904 PN14948 allow merge fields to work

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameInsFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = g_lPolicyID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNamePartyCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = g_lPartyCnt


                m_lReturn = oRiskDetails.SetKeys(vKeyArray:=vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oRiskDetails = Nothing
                    Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                    Exit Sub
                End If


                m_lReturn = CType(oRiskDetails, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oRiskDetails = Nothing
                    Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                    Exit Sub
                End If


                m_lReturn = oRiskDetails.SetProcessModes(vTransactionType:=m_sTransactionType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oRiskDetails = Nothing
                    Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                    Exit Sub
                End If


                m_lReturn = oRiskDetails.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oRiskDetails = Nothing
                    Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                    Exit Sub
                End If


                oRiskDetails.Dispose()


                oRiskDetails = Nothing
                Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                Me.Focus()
            Case "InformationChecklist"

                'disable it - stop user from clicking on it repeatedly
                Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = False

                Dim temp_oInfoChecklist As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oInfoChecklist, sClassName:="iCLMInfoChklst.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oInfoChecklist = temp_oInfoChecklist

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iCLMInfoChklst.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar1_ButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                    Exit Sub
                End If

                If oInfoChecklist Is Nothing Then Exit Sub

                ReDim vKeyArray(1, 4)


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimReference

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = g_sClaimNo


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClaimCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = g_lClaimID


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameOperateMode

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = gPMConstants.PMEComponentAction.PMEdit

                'RWH(06/03/2001) Claims stores risk_cnt in risk_type_id !!!

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameRiskTypeID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = g_lRiskTypeID


                vKeyArray(0, 4) = "DeleteWorkTableFlag"

                vKeyArray(1, 4) = gPMConstants.PMEReturnCode.PMFalse





                m_lReturn = oInfoChecklist.SetKeys(vKeyArray:=vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oInfoChecklist = Nothing
                    Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                    Exit Sub
                End If




                m_lReturn = CType(oInfoChecklist, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oInfoChecklist = Nothing
                    Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                    Exit Sub
                End If



                m_lReturn = oInfoChecklist.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)


                m_lReturn = oInfoChecklist.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oInfoChecklist = Nothing
                    Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                    Exit Sub
                End If


                oInfoChecklist.Dispose()


                oInfoChecklist = Nothing
                Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                'DC281100

                'S4B Claim Enhancements R&D 2005
            Case "Financial"

                Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = False

                Dim temp_oFinancialSummary As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oFinancialSummary, sClassName:="iCLMFinSumm.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oFinancialSummary = temp_oFinancialSummary

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iCLMFinSumm.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="frmInterface.Toolbar1.ButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                If Not (oFinancialSummary Is Nothing) Then

                    CType(oFinancialSummary, SSP.S4I.Interfaces.ILocalInterface).Initialise()

                    oFinancialSummary.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)

                    oFinancialSummary.Claimid = g_lClaimID

                    oFinancialSummary.Start()

                    oFinancialSummary.Dispose()
                    oFinancialSummary = Nothing
                End If


                Toolbar1.Items.Item(Button.Owner.Items.IndexOf(Button) - 1).Enabled = True
                'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.5.1)
                'This button Doc Archive is newly added for implementing the document archive functionality
            Case "DocArchive"
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOption, v_iSourceID:=g_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return
                End If

                If sOption = "2" Then
                    m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5085, r_sOptionValue:=sSPUrl, v_iSourceID:=g_iSourceID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return
                    End If


                    m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5086, r_sOptionValue:=sDocLIB, v_iSourceID:=g_iSourceID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return
                    End If
                End If

                If Claimid = 0 Then

                    Dim temp_oPMUPolicy As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oPMUPolicy, "bPMUPolicy.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    oPMUPolicy = temp_oPMUPolicy

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bPMUPolicy.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Exit Sub

                    End If


                    m_lReturn = oPMUPolicy.GetClientCode(v_iPartyID:=m_lPartyCnt, r_vClientarray:=vClientCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bPMUPolicy.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Exit Sub

                    End If

                    sClientCode = gPMFunctions.ToSafeString(CStr(vClientCode(0, 0)))

                    oPMUPolicy.Dispose()
                    If sOption = "1" Then
                        m_lReturn = iPMFunc.RunDocumaster(v_sLinkCode:=sClientCode.Trim() & "1")
                    ElseIf sOption = "2" Then
                        System.Diagnostics.Process.Start(sSPUrl & sDocLIB & "\" & sClientCode.Trim())
                    End If

                Else
                    If sOption = "1" Then
                        m_lReturn = iPMFunc.RunDocumaster(v_sLinkCode:=txtClaimNumber.Text.Trim() & "2")
                    ElseIf sOption = "2" Then
                        System.Diagnostics.Process.Start(sSPUrl & sDocLIB & "\" & g_sClientShortName.Trim() & "\Claim\" & txtClaimNumber.Text.Trim())
                    End If

                    End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                    'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.5.1)
        End Select

    End Sub

    Private Sub txtOpenClaim_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtOpenClaim_3.Enter, _txtOpenClaim_11.Enter, _txtOpenClaim_13.Enter, _txtOpenClaim_12.Enter, _txtOpenClaim_8.Enter, _txtOpenClaim_7.Enter, _txtOpenClaim_4.Enter, _txtOpenClaim_9.Enter, _txtOpenClaim_6.Enter, _txtOpenClaim_5.Enter, _txtOpenClaim_2.Enter, _txtOpenClaim_1.Enter, _txtOpenClaim_0.Enter, _txtOpenClaim_10.Enter, _txtOpenClaim_24.Enter, _txtOpenClaim_30.Enter, _txtOpenClaim_29.Enter, _txtOpenClaim_28.Enter, _txtOpenClaim_27.Enter, _txtOpenClaim_26.Enter, _txtOpenClaim_25.Enter, _txtOpenClaim_15.Enter, _txtOpenClaim_16.Enter, _txtOpenClaim_17.Enter, _txtOpenClaim_19.Enter, _txtOpenClaim_20.Enter, _txtOpenClaim_21.Enter, _txtOpenClaim_22.Enter, _txtOpenClaim_23.Enter, _txtOpenClaim_18.Enter, _txtOpenClaim_31.Enter, _txtOpenClaim_32.Enter, _txtOpenClaim_33.Enter, _txtOpenClaim_37.Enter, _txtOpenClaim_34.Enter, _txtOpenClaim_35.Enter, _txtOpenClaim_36.Enter, _txtOpenClaim_38.Enter, _txtOpenClaim_39.Enter, _txtOpenClaim_40.Enter, _txtOpenClaim_41.Enter, _txtOpenClaim_42.Enter, _txtOpenClaim_44.Enter, _txtOpenClaim_45.Enter, _txtOpenClaim_43.Enter
        Dim Index As Integer = Array.IndexOf(txtOpenClaim, eventSender)

        Try

            iPMFunc.SelectText(txtOpenClaim(Index))



            Select Case Index
                Case g_nLOSS_DATE
                    txtOpenClaim(Index).Text = StringsHelper.Format(txtOpenClaim(Index).Text, ACDateConversion)
                    'g_dtLossDate = ""
                    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtOpenClaim(Index))
                Case g_nREPORTED_DATE
                    txtOpenClaim(Index).Text = StringsHelper.Format(txtOpenClaim(Index).Text, ACDateConversion)
                    'g_dtReportedDate = ""
                    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtOpenClaim(Index))
                Case g_nLOSS_TO_DATE
                    txtOpenClaim(Index).Text = StringsHelper.Format(txtOpenClaim(Index).Text, ACDateConversion)
                    'g_dtLossToDate = ""
                    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtOpenClaim(Index))
                Case g_nREPORTED_TO_DATE
                    txtOpenClaim(Index).Text = StringsHelper.Format(txtOpenClaim(Index).Text, ACDateConversion)
                    'g_dtReportedToDate = ""

                Case g_nDRIVER_PASSEDTEST
                    txtOpenClaim(Index).Text = StringsHelper.Format(txtOpenClaim(Index).Text, ACDateConversion)

                Case g_nLOSS_TIME, g_nREPORTED_TIME, g_nREPORTED_TO_TIME
                    If txtOpenClaim(Index).Text.Trim() = "" Then
                        txtOpenClaim(Index).Text = DateTime.Now.ToString("HH:mm")
                    End If

            End Select

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Procedure Failure", vApp:=ACApp, vClass:=ACClass, vMethod:="txtOpenClaim_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: CheckDatesValid
    ' Created: JMK (31/05/2001)
    ' Description: Move Date validation from txtOpenClaim_LostFocus to this function
    ' History: DN 14/05/03  Re-jig the function so that the validation is done after
    '                       the date variable have been populated
    ' ***************************************************************** '
    Private Function CheckDatesValid() As Boolean

        Dim result As Boolean = False
        Dim sMessageTitle As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '*****    DATES    *****
            'g_nLOSS_DATE, g_nREPORTED_DATE, g_nLOSS_TO_DATE, g_nREPORTED_TO_DATE

            '*****LOSS DATE*****
            If gPMFunctions.ToSafeString(txtOpenClaim(g_nLOSS_DATE).Text).Trim() <> "" Then

                sMessageTitle = "Loss Date"

                If Information.IsDate(txtOpenClaim(g_nLOSS_DATE).Text) Then
                    g_dtLossDate = StringsHelper.Format(txtOpenClaim(g_nLOSS_DATE).Text, ACDateConversion)
                    FormatDate(txtOpenClaim(g_nLOSS_DATE).Text, g_nLOSS_DATE)
                Else
                    DisplayMessage(ACInvalidDateMsg, sMessageTitle)
                    txtOpenClaim(g_nLOSS_DATE).Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            '*****REPORTED DATE*****
            If txtOpenClaim(g_nREPORTED_DATE).Text.Trim() <> "" Then

                sMessageTitle = "Reported Date"

                'Changes by Sameer for the Message to come from the Resource File
                If Information.IsDate(txtOpenClaim(g_nREPORTED_DATE).Text) Then
                    g_dtReportedDate = StringsHelper.Format(txtOpenClaim(g_nREPORTED_DATE).Text, ACDateConversion)
                    FormatDate(txtOpenClaim(g_nREPORTED_DATE).Text, g_nREPORTED_DATE)
                Else
                    DisplayMessage(ACInvalidDateMsg, sMessageTitle)
                    txtOpenClaim(g_nREPORTED_DATE).Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            '*****LOSS TO DATE*****
            If txtOpenClaim(g_nLOSS_TO_DATE).Text.Trim() <> "" Then

                sMessageTitle = "Loss To Date"

                If Information.IsDate(txtOpenClaim(g_nLOSS_TO_DATE).Text) Then
                    g_dtLossToDate = StringsHelper.Format(txtOpenClaim(g_nLOSS_TO_DATE).Text, ACDateConversion)
                    FormatDate(txtOpenClaim(g_nLOSS_TO_DATE).Text, g_nLOSS_TO_DATE)
                Else
                    DisplayMessage(ACInvalidDateMsg, sMessageTitle)
                    txtOpenClaim(g_nLOSS_TO_DATE).Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            '*****VALIDATE THE DATES*****
            If gPMFunctions.ToSafeString(txtOpenClaim(g_nLOSS_DATE).Text).Trim() <> "" Then

                sMessageTitle = "Loss Date"

                If DateAndTime.DateDiff("d", CDate(g_dtLossDate), DateTime.Today, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                    DisplayMessage(ACLossDateLaterThanCurrentDate, sMessageTitle)
                    If txtOpenClaim(g_nLOSS_DATE).Enabled = True Then
                    txtOpenClaim(g_nLOSS_DATE).Focus()
                    End If
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If g_dtLossDate <> "" And g_dtReportedDate <> "" Then
                    If DateAndTime.DateDiff("d", CDate(StringsHelper.Format(g_dtLossDate, ACDateConversion)), CDate(StringsHelper.Format(g_dtReportedDate, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                        DisplayMessage(ACLossDateLaterThanReportedDate, sMessageTitle)
                        If txtOpenClaim(g_nLOSS_DATE).Enabled Then
                            txtOpenClaim(g_nLOSS_DATE).Focus()
                        End If
                        Return gPMConstants.PMEReturnCode.PMFalse
                    ElseIf (DateAndTime.DateDiff("d", CDate(StringsHelper.Format(g_dtLossDate, ACDateConversion)), CDate(StringsHelper.Format(g_dtReportedDate, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) = 0) And IsTime(txtOpenClaim(g_nLOSS_TIME).Text) And IsTime(txtOpenClaim(g_nREPORTED_TIME).Text) Then
                        If DateAndTime.DateDiff("s", CDate(StringsHelper.Format(txtOpenClaim(g_nLOSS_TIME).Text, ACTimeConversion)), CDate(StringsHelper.Format(txtOpenClaim(g_nREPORTED_TIME).Text, ACTimeConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                            DisplayMessage(ACLossDateLaterThanReportedDate, sMessageTitle)
                            If txtOpenClaim(g_nLOSS_TIME).Enabled Then
                                txtOpenClaim(g_nLOSS_TIME).Focus()
                            End If
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If

                If g_dtLossDate <> "" And g_dtLossToDate <> "" Then
                    If DateAndTime.DateDiff("d", CDate(StringsHelper.Format(g_dtLossDate, ACDateConversion)), CDate(StringsHelper.Format(g_dtLossToDate, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                        DisplayMessage(ACLossDateLaterThanLossToDate, sMessageTitle)
                        txtOpenClaim(g_nLOSS_DATE).Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If txtOpenClaim(g_nREPORTED_DATE).Text.Trim() <> "" Then

                sMessageTitle = "Reported Date"

                If g_dtLossDate <> "" And g_dtReportedDate <> "" Then
                    If DateAndTime.DateDiff("d", CDate(StringsHelper.Format(g_dtLossDate, ACDateConversion)), CDate(StringsHelper.Format(g_dtReportedDate, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                        DisplayMessage(ACLossDateLaterThanReportedDate, sMessageTitle)
                        txtOpenClaim(g_nREPORTED_DATE).Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    ElseIf (DateAndTime.DateDiff("d", CDate(StringsHelper.Format(g_dtLossDate, ACDateConversion)), CDate(StringsHelper.Format(g_dtReportedDate, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) = 0) And IsTime(txtOpenClaim(g_nLOSS_TIME).Text) And IsTime(txtOpenClaim(g_nREPORTED_TIME).Text) Then
                        If DateAndTime.DateDiff("s", CDate(StringsHelper.Format(gPMFunctions.ToSafeDate(txtOpenClaim(g_nLOSS_TIME).Text), ACTimeConversion)), CDate(StringsHelper.Format(txtOpenClaim(g_nREPORTED_TIME).Text, ACTimeConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                            DisplayMessage(ACLossDateLaterThanReportedDate, sMessageTitle)
                            txtOpenClaim(g_nREPORTED_TIME).Focus()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If

                If DateAndTime.DateDiff("d", CDate(g_dtReportedDate), CDate(StringsHelper.Format(DateTime.Today, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                    DisplayMessage(ACReportedDateLaterThanCurrentDate, sMessageTitle)
                    txtOpenClaim(g_nREPORTED_DATE).Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If txtOpenClaim(g_nLOSS_TO_DATE).Text.Trim() <> "" Then

                sMessageTitle = "Loss To Date"

                If g_dtLossDate <> "" And g_dtLossToDate <> "" Then
                    If DateAndTime.DateDiff("d", CDate(StringsHelper.Format(g_dtLossDate, ACDateConversion)), CDate(StringsHelper.Format(g_dtLossToDate, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                        DisplayMessage(ACLossDateLaterThanLossToDate, sMessageTitle)
                        txtOpenClaim(g_nLOSS_TO_DATE).Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If


            '*****    TIMES    *****
            'g_nLOSS_TIME, g_nREPORTED_TIME, g_nREPORTED_TO_TIME
            If txtOpenClaim(g_nLOSS_TIME).Text.Trim() <> "" Then
                If IsTime(txtOpenClaim(g_nLOSS_TIME).Text) Then
                    g_dtLossTime = StringsHelper.Format(txtOpenClaim(g_nLOSS_TIME).Text, ACTimeConversion)
                Else
                    DisplayMessage(ACInvaildTimeMsg, "Loss Time")
                    txtOpenClaim(g_nLOSS_TIME).Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If txtOpenClaim(g_nREPORTED_TIME).Text.Trim() <> "" Then
                If IsTime(txtOpenClaim(g_nREPORTED_TIME).Text) Then
                    g_dtReportedTime = StringsHelper.Format(txtOpenClaim(g_nREPORTED_TIME).Text, ACTimeConversion)
                Else
                    DisplayMessage(ACInvaildTimeMsg, "Reported Time")
                    txtOpenClaim(g_nREPORTED_TIME).Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Date Validation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDatesValid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub txtOpenClaim_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles _txtOpenClaim_3.KeyPress, _txtOpenClaim_11.KeyPress, _txtOpenClaim_13.KeyPress, _txtOpenClaim_12.KeyPress, _txtOpenClaim_8.KeyPress, _txtOpenClaim_7.KeyPress, _txtOpenClaim_4.KeyPress, _txtOpenClaim_9.KeyPress, _txtOpenClaim_6.KeyPress, _txtOpenClaim_5.KeyPress, _txtOpenClaim_2.KeyPress, _txtOpenClaim_1.KeyPress, _txtOpenClaim_0.KeyPress, _txtOpenClaim_10.KeyPress, _txtOpenClaim_24.KeyPress, _txtOpenClaim_30.KeyPress, _txtOpenClaim_29.KeyPress, _txtOpenClaim_28.KeyPress, _txtOpenClaim_27.KeyPress, _txtOpenClaim_26.KeyPress, _txtOpenClaim_25.KeyPress, _txtOpenClaim_15.KeyPress, _txtOpenClaim_16.KeyPress, _txtOpenClaim_17.KeyPress, _txtOpenClaim_19.KeyPress, _txtOpenClaim_20.KeyPress, _txtOpenClaim_21.KeyPress, _txtOpenClaim_22.KeyPress, _txtOpenClaim_23.KeyPress, _txtOpenClaim_18.KeyPress, _txtOpenClaim_31.KeyPress, _txtOpenClaim_32.KeyPress, _txtOpenClaim_33.KeyPress, _txtOpenClaim_37.KeyPress, _txtOpenClaim_34.KeyPress, _txtOpenClaim_35.KeyPress, _txtOpenClaim_36.KeyPress, _txtOpenClaim_38.KeyPress, _txtOpenClaim_39.KeyPress, _txtOpenClaim_40.KeyPress, _txtOpenClaim_41.KeyPress, _txtOpenClaim_42.KeyPress, _txtOpenClaim_44.KeyPress, _txtOpenClaim_45.KeyPress, _txtOpenClaim_43.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim Index As Integer = Array.IndexOf(txtOpenClaim, eventSender)
        'PN_69448 Start
        Select Case Index
            Case g_nCLIENT_EMAIL
                ValidateEmail(txtOpenClaim(g_nCLIENT_EMAIL), KeyAscii)
        End Select
        'PN_69448 end
        '******** Bugid - 17 Start of Code Change As Vallidation were not required for Tel/Fax No
        '
        '
        '            Select Case Index
        '
        '                Case g_nCLIENT_FAXNO
        '
        '                    If (KeyAscii <> 8) Then
        '                    If (KeyAscii <> 32) Then
        '
        '                    If KeyAscii < 48 Or KeyAscii > 57 Then
        '
        '                        KeyAscii = 0
        '
        '                        Call DisplayMessage(ACInvalidNumberMsg, mid$(lblFaxNumber.Name, 4))
        '
        '                    End If
        '                    End If
        '
        '                    End If
        '
        '                Case g_nCLIENT_MOBILENO
        '
        '                    If KeyAscii <> 8 Then
        '                    If (KeyAscii <> 32) Then
        '
        '                    If KeyAscii < 48 Or KeyAscii > 57 Then
        '
        '                        KeyAscii = 0
        '                        Call DisplayMessage(ACInvalidNumberMsg, mid$(lblMobileNumber.Name, 4))
        '
        '                    End If
        '                    End If
        '                    End If
        '
        '                Case g_nCLIENT_TELNO
        '
        '                    If KeyAscii <> 8 Then
        '                    If (KeyAscii <> 32) Then
        '                    If KeyAscii < 48 Or KeyAscii > 57 Then
        '
        '                        KeyAscii = 0
        '                        Call DisplayMessage(ACInvalidNumberMsg, mid$(lblTelePhoneNumberH.Name, 4))
        '
        '                    End If
        '                    End If
        '                    End If
        '                Case g_nCLIENT_TELNOOFF
        '
        '                    If KeyAscii <> 8 Then
        '                    If (KeyAscii <> 32) Then
        '                    If KeyAscii < 48 Or KeyAscii > 57 Then
        '
        '                        KeyAscii = 0
        '                        Call DisplayMessage(ACInvalidNumberMsg, mid$(lblTelePhoneNumberO.Name, 4))
        '
        '                    End If
        '                    End If
        '                    End If
        '
        '                Case g_nINSURER_FAXNO
        '
        '                    If KeyAscii <> 8 Then
        '                    If (KeyAscii <> 32) Then
        '                    If KeyAscii < 48 Or KeyAscii > 57 Then
        '
        '                        KeyAscii = 0
        '                        Call DisplayMessage(ACInvalidNumberMsg, mid$(lblIFaxNumber.Name, 5))
        '
        '                    End If
        '                    End If
        '                    End If
        '
        '                Case g_nINSURER_TELNO
        '
        '                    If KeyAscii <> 8 Then
        '                    If (KeyAscii <> 32) Then
        '                    If KeyAscii < 48 Or KeyAscii > 57 Then
        '
        '                        KeyAscii = 0
        '                        Call DisplayMessage(ACInvalidNumberMsg, mid$(lblTelephoneNumber.Name, 4))
        '
        '                    End If
        '                    End If
        '                    End If
        '        End Select

        '******** Bugid - 17 end of Code Change As Vallidation were not required for Tel/Fax No


        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub


    ' ***************************************************************** '
    ' Name: FormatDate
    '
    ' Description:  To write the details from the Business to Local variables
    '               INPUT : Input Date as strings
    '                       Control array index
    ' ***************************************************************** '
    Private Function FormatDate(ByRef sInDate As String, ByRef txtIndex As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACLongdate As String = "long date"

            If Not Information.IsDate(sInDate) Then
                'MSS250901 - Added for merge
                'MSS250901 - Merge end
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Changes done for the date format to remain as long date
            sInDate = StringsHelper.Format(sInDate, ACLongdate)
            txtOpenClaim(txtIndex).Text = sInDate

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatDate Method Failure", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FormatDate
    '
    ' Description:  To display the details from the Local values to the
    '               Form controls
    '
    ' ***************************************************************** '
    Private Function DisplayPolicyDetails() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vClientAddress, vInsurerAddress As Object

            'Non Displayed Values g_sClaimNo , g_sPolicyNo, g_lPolicyID

            'Tomo190402
            txtClaimNumber.Text = g_sClaimNo

            txtOpenClaim(g_nDESCRIPTION).Text = g_sDescription
            txtOpenClaim(g_nLOCATION).Text = g_sLocation

            Dim g_sLossFromDateTemp As String = g_sLossFromDate
            FormatDate(g_sLossFromDateTemp, g_nLOSS_DATE)

            g_dtLossDate = StringsHelper.Format(g_sLossFromDate, ACDateConversion)

            Dim TempDate As Date
            txtOpenClaim(g_nLOSS_TIME).Text = IIf(DateTime.TryParse(g_sLossFromDate, TempDate), TempDate.ToString("HH:mm"), g_sLossFromDate)

            Dim g_sReportedDateTemp As String = g_sReportedDate
            FormatDate(g_sReportedDateTemp, g_nREPORTED_DATE)

            g_dtReportedDate = StringsHelper.Format(g_sReportedDate, ACDateConversion)
            g_dtReportedTime = StringsHelper.Format(g_sReportedDate, ACTimeConversion)

            Dim TempDate2 As Date
            txtOpenClaim(g_nREPORTED_TIME).Text = IIf(DateTime.TryParse(g_sReportedDate, TempDate2), TempDate2.ToString("HH:mm"), g_sReportedDate)

            Dim g_sLossToDateTemp As String = g_sLossToDate
            FormatDate(g_sLossToDateTemp, g_nLOSS_TO_DATE)

            g_dtLossToDate = StringsHelper.Format(g_sLossToDate, ACDateConversion)

            Dim g_sReportedToDateTemp As String = g_sReportedToDate
            FormatDate(g_sReportedToDateTemp, g_nREPORTED_TO_DATE)

            g_dtReportedToDate = StringsHelper.Format(g_sReportedToDate, ACDateConversion)

            Dim TempDate3 As Date
            g_dtReportedToTime = IIf(DateTime.TryParse(g_sReportedToDate, TempDate3), TempDate3.ToString("HH:mm"), g_sReportedToDate)

            Dim TempDate4 As Date
            txtOpenClaim(g_nREPORTED_TO_TIME).Text = IIf(DateTime.TryParse(g_sReportedToDate, TempDate4), TempDate4.ToString("HH:mm"), g_sReportedToDate)

            Dim g_sLastModifiedDateTemp As String = g_sLastModifiedDate
            FormatDate(g_sLastModifiedDateTemp, g_nLAST_MODIFIED_DATE)

            g_dtLastModifiedDate = StringsHelper.Format(g_sLastModifiedDate, ACDateConversion)

            Dim TempDate5 As Date
            txtOpenClaim(g_nLAST_MODIFIED_TIME).Text = IIf(DateTime.TryParse(g_sLastModifiedDate, TempDate5), TempDate5.ToString("HH:mm"), g_sLastModifiedDate)

            txtOpenClaim(g_nCLIENT_NAME).Text = g_sClientName

            'Address Need to be got from Claim Address Table-Pandu

            g_lClientAdressCnt = g_sClientAddress


            m_lReturn = g_oBusiness.GetClmAdd(vClientAddress, g_lClientAdressCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            g_sClientAddress1 = Convert.ToString(vClientAddress(0, 0)).Trim()

            g_sClientAddress2 = Convert.ToString(vClientAddress(1, 0)).Trim()

            g_sClientAddress3 = Convert.ToString(vClientAddress(2, 0)).Trim()

            g_sClientAddress4 = Convert.ToString(vClientAddress(3, 0)).Trim()

            g_sClientPostCode = Convert.ToString(vClientAddress(4, 0)).Trim()
            'JMK 31/05/2001


            g_lClientCountryId = CInt(vClientAddress(5, 0))

            m_lReturn = g_oBusiness.GetCountryName(g_sClientCountryName, g_lClientCountryId)
            If g_sCaseNumber.Trim() <> "" Then
                txtCaseNumber.Text = g_sCaseNumber.Trim()
                lblCaseNumber.Visible = True
                txtCaseNumber.Visible = True
            Else
                lblCaseNumber.Visible = False
                txtCaseNumber.Visible = False
            End If

            'JMK 31/05/2001 - Postcode for UK only
            If g_lClientCountryId <> ACCountryGBR Then
                txtOpenClaim(g_nCLIENT_ADDRESS).Text = g_sClientAddress1 & " " &
                                                       g_sClientAddress2 & " " & g_sClientAddress3 & " " &
                                                       g_sClientAddress4 & " " & g_sClientCountryName
            Else
                txtOpenClaim(g_nCLIENT_ADDRESS).Text = g_sClientAddress1 & " " &
                                                       g_sClientAddress2 & " " & g_sClientAddress3 & " " &
                                                       g_sClientAddress4 & " " & g_sClientPostCode
            End If


            m_sClientTemp = txtOpenClaim(g_nCLIENT_ADDRESS).Text

            txtOpenClaim(g_nCLIENT_TELNO).Text = g_sClientTelNo
            txtOpenClaim(g_nCLIENT_TELNOOFF).Text = g_sClientTelNoOff
            txtOpenClaim(g_nCLIENT_FAXNO).Text = g_sClientFaxNo
            txtOpenClaim(g_nCLIENT_MOBILENO).Text = g_sClientMobileNo
            txtOpenClaim(g_nCLIENT_EMAIL).Text = g_sClientEMail
            txtOpenClaim(g_nCLIENT_VAT_REGNO).Text = g_sVATRegisteredNo
            txtOpenClaim(g_nCLIENT_CLAIMNO).Text = g_sClientClaimNo

            txtOpenClaim(g_nINSURER_NAME).Text = g_sInsurerName


            'Address Need to be got from Claim Address Table -Pandu

            g_lInsurerAdressCnt = g_sInsurerAddress



            m_lReturn = g_oBusiness.GetClmAdd(vInsurerAddress, g_lInsurerAdressCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'TN20010905 - only error if its not not_found
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            If Information.IsArray(vInsurerAddress) Then

                g_sInsurerAddress1 = Convert.ToString(vInsurerAddress(0, 0)).Trim()

                g_sInsurerAddress2 = Convert.ToString(vInsurerAddress(1, 0)).Trim()

                g_sInsurerAddress3 = Convert.ToString(vInsurerAddress(2, 0)).Trim()

                g_sInsurerAddress4 = Convert.ToString(vInsurerAddress(3, 0)).Trim()

                g_sInsurerPostCode = Convert.ToString(vInsurerAddress(4, 0)).Trim()
            Else
                g_sInsurerAddress1 = ""
                g_sInsurerAddress2 = ""
                g_sInsurerAddress3 = ""
                g_sInsurerAddress4 = ""
                g_sInsurerPostCode = ""
            End If

            'JMK 31/05/2001

            If Information.IsArray(vInsurerAddress) Then

                g_lInsurerCountryId = CInt(vInsurerAddress(5, 0))
            Else
                g_lInsurerCountryId = 0
            End If

            'JMK 31/05/2001 - Postcode for UK only
            If g_lInsurerCountryId <> ACCountryGBR Then
                txtOpenClaim(g_nINSURER_ADDRESS).Text = g_sInsurerAddress1 & " " &
                                                        g_sInsurerAddress2 & " " & g_sInsurerAddress3 & " " &
                                                        g_sInsurerAddress4
            Else
                txtOpenClaim(g_nINSURER_ADDRESS).Text = g_sInsurerAddress1 & " " &
                                                        g_sInsurerAddress2 & " " & g_sInsurerAddress3 & " " &
                                                        g_sInsurerAddress4 & " " & g_sInsurerPostCode
            End If

            m_sInsurertemp = txtOpenClaim(g_nINSURER_ADDRESS).Text


            txtOpenClaim(g_nINSURER_TELNO).Text = g_sInsurerTelNo
            txtOpenClaim(g_nINSURER_FAXNO).Text = g_sInsurerFaxNo
            txtOpenClaim(g_nINSURER_CONTACT).Text = g_sInsurerContact
            txtOpenClaim(g_nINSURER_EMAIL).Text = g_sInsurerEmail
            txtOpenClaim(g_nINSURER_CLAIMNO).Text = g_sInsurerClaimNo

            'DC240402 -Start

            If g_nInfoOnly = 1 Then
                chkInfoOnly.CheckState = CheckState.Checked
                cmdChangeClientPolicy.Enabled = True
            Else
                chkInfoOnly.CheckState = CheckState.Unchecked
                cmdChangeClientPolicy.Enabled = False

                'DC180202 moved check further down

                chkInfoOnly.Enabled = False


                'Added by Pandu as enhancement
                chkLikelyClaim.Enabled = False


            End If

            If g_nLikelyClaim = 1 Then
                chkLikelyClaim.CheckState = CheckState.Checked
            Else
                chkLikelyClaim.CheckState = CheckState.Unchecked
            End If

            'Changes by Sameer for displaying the Decription of the Claim Status on 18-07-00 at 3:30 pm
            'Condition made to set Claim status as reopen if Progress is Reopen
            If g_lProgressStatusID = 3 Then g_lClaimStatusID = 4

            Select Case g_lClaimStatusID
                Case CLMProvisionalOpenClaim '   Provisional Open Claim
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_sPROVISIONALOPENCLAIM

                Case CLMLiveOpenClaim '   Live Open Claim
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_sLIVEOPENCLAIM

                Case CLMClosed '   Closed Claim
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_sCLOSED

                Case CLMReOpened '   Re Opened Claim
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_REOPENED

                Case CLMReClosed '   Re Closed
                    txtOpenClaim(g_nCLAIM_STATUS).Text = g_RECLOSED

            End Select

            If g_lClaimStatusID = CLMLiveOpenClaim Then


            End If

            'End of the changes by Sameer for displaying the Decription of the Claim Status on 18-07-00 at 3:30 pm

            If g_nVATRegistered = 1 Then
                chkVATRegistered.CheckState = CheckState.Checked
            Else
                chkVATRegistered.CheckState = CheckState.Unchecked
                'Change Made After Enhancements
                txtOpenClaim(g_nCLIENT_VAT_REGNO).Enabled = False
            End If


            txtTPA.Tag = CStr(g_iUserOtherPartyID)
            txtTPA.Text = CStr(g_sUserOtherPartyname)

            For nCount As Integer = 0 To cboHandler.Items.Count - 1

                If g_lHandlerID = VB6.GetItemData(cboHandler, nCount) Then
                    cboHandler.SelectedIndex = nCount
                    Exit For
                End If

            Next

            For nCount As Integer = 0 To cboProgressStatus.Items.Count - 1

                If g_lProgressStatusID = VB6.GetItemData(cboProgressStatus, nCount) Then
                    cboProgressStatus.SelectedIndex = nCount
                    Exit For
                End If

            Next

            For nCount As Integer = 0 To cboPrimaryCausationCode.Items.Count - 1

                If g_lPrimaryCauseID = VB6.GetItemData(cboPrimaryCausationCode, nCount) Then
                    cboPrimaryCausationCode.SelectedIndex = nCount
                    Exit For
                End If

            Next

            For nCount As Integer = 0 To cboSecondaryCausationCode.Items.Count - 1

                If g_lSecondaryCauseID = VB6.GetItemData(cboSecondaryCausationCode, nCount) Then
                    cboSecondaryCausationCode.SelectedIndex = nCount
                    Exit For
                End If

            Next

            For nCount As Integer = 0 To cboTown.Items.Count - 1

                If g_lTown = VB6.GetItemData(cboTown, nCount) Then
                    cboTown.SelectedIndex = nCount
                    Exit For
                End If

            Next

            For nCount As Integer = 0 To cboCurrency.Items.Count - 1

                If g_lCurrencyID = VB6.GetItemData(cboCurrency, nCount) Then
                    cboCurrency.SelectedIndex = nCount
                    Exit For
                End If

            Next

            For nCount As Integer = 0 To cboCatastropheCode.Items.Count - 1

                If g_lCatastropheCodeID = VB6.GetItemData(cboCatastropheCode, nCount) Then
                    cboCatastropheCode.SelectedIndex = nCount
                    Exit For
                End If

            Next

            'Fields Newly Added in Database to be Displayed -Pandu

            'Claims Status Date

            Dim TempDate6 As Date
            txtOpenClaim(g_nCLAIM_STATUS_TIME).Text = IIf(DateTime.TryParse(g_sClaimsStatusDate, TempDate6), TempDate6.ToString("HH:mm"), g_sClaimsStatusDate)

            FormatDate(g_sClaimsStatusDate, g_nCLAIM_STATUS_DATE)

            Dim TempDate7 As Date
            g_dtClaimStatusDate = IIf(DateTime.TryParse(g_sClaimsStatusDate, TempDate7), TempDate7.ToString("dd/MM/yyyy"), g_sClaimsStatusDate)

            'RiskDescription to be Shown

            For nCount As Integer = 0 To cboRiskType.Items.Count - 1

                'RWH(06/03/2001) Claims stores risk_cnt in risk_type_id !!!
                If g_lRiskTypeID = VB6.GetItemData(cboRiskType, nCount) Then
                    cboRiskType.SelectedIndex = nCount
                    Exit For
                End If

            Next

            For nCount As Integer = 0 To cboUDFA.ListCount - 1

                If g_lUserDefFldA = cboUDFA.ItemId Then
                    cboUDFA.ListIndex = nCount
                    Exit For
                End If

                If nCount < (cboUDFA.ListCount - 1) Then

                    cboUDFA.ListIndex += 1

                End If
            Next

            For nCount As Integer = 0 To cboUDFB.ListCount - 1

                If g_lUserDefFldB = cboUDFB.ItemId Then
                    cboUDFB.ListIndex = nCount
                    Exit For
                End If

                If nCount < (cboUDFB.ListCount - 1) Then

                    cboUDFB.ListIndex += 1

                End If
            Next

            For nCount As Integer = 0 To cboUDFC.ListCount - 1

                If g_lUserDefFldC = cboUDFC.ItemId Then
                    cboUDFC.ListIndex = nCount
                    Exit For
                End If

                If nCount < (cboUDFC.ListCount - 1) Then

                    cboUDFC.ListIndex += 1

                End If

            Next

            For nCount As Integer = 0 To cboUDFD.ListCount - 1

                If g_lUserDefFldD = cboUDFD.ItemId Then
                    cboUDFD.ListIndex = nCount
                    Exit For
                End If

                If nCount < (cboUDFD.ListCount - 1) Then

                    cboUDFD.ListIndex += 1

                End If

            Next

            For nCount As Integer = 0 To cboUDFE.ListCount - 1

                If g_lUserDefFldE = cboUDFE.ItemId Then
                    cboUDFE.ListIndex = nCount
                    Exit For
                End If

                If nCount < (cboUDFE.ListCount - 1) Then

                    cboUDFE.ListIndex += 1

                End If

            Next

            For nCount As Integer = 0 To cboUnderwritingYearID.Items.Count - 1
                'Developer Guide no. 248
                If VB6.GetItemData(cboUnderwritingYearID, nCount) = gPMFunctions.ToSafeInteger(g_vUnderwritingYearID) Then
                    cboUnderwritingYearID.SelectedIndex = nCount
                    Exit For
                End If
            Next nCount

            txtClaimVersion.Text = CStr(g_lVersionId)


            'S4B Claim Enhancements R&D 2005

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayPolicyDetails Method Failure", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayPolicyDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'MSS250901 - Added whole of Broking section with switch
    ' We are probably repeating code here but I want it to work first.
    ' It can be stripped out at a later date
    Private Sub txtOpenClaim_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtOpenClaim_3.Leave, _txtOpenClaim_11.Leave, _txtOpenClaim_13.Leave, _txtOpenClaim_12.Leave, _txtOpenClaim_8.Leave, _txtOpenClaim_7.Leave, _txtOpenClaim_4.Leave, _txtOpenClaim_9.Leave, _txtOpenClaim_6.Leave, _txtOpenClaim_5.Leave, _txtOpenClaim_2.Leave, _txtOpenClaim_1.Leave, _txtOpenClaim_0.Leave, _txtOpenClaim_10.Leave, _txtOpenClaim_24.Leave, _txtOpenClaim_30.Leave, _txtOpenClaim_29.Leave, _txtOpenClaim_28.Leave, _txtOpenClaim_27.Leave, _txtOpenClaim_26.Leave, _txtOpenClaim_25.Leave, _txtOpenClaim_15.Leave, _txtOpenClaim_16.Leave, _txtOpenClaim_17.Leave, _txtOpenClaim_19.Leave, _txtOpenClaim_20.Leave, _txtOpenClaim_21.Leave, _txtOpenClaim_22.Leave, _txtOpenClaim_23.Leave, _txtOpenClaim_18.Leave, _txtOpenClaim_31.Leave, _txtOpenClaim_32.Leave, _txtOpenClaim_33.Leave, _txtOpenClaim_37.Leave, _txtOpenClaim_34.Leave, _txtOpenClaim_35.Leave, _txtOpenClaim_36.Leave, _txtOpenClaim_38.Leave, _txtOpenClaim_39.Leave, _txtOpenClaim_40.Leave, _txtOpenClaim_41.Leave, _txtOpenClaim_42.Leave, _txtOpenClaim_44.Leave, _txtOpenClaim_45.Leave, _txtOpenClaim_43.Leave
        Dim Index As Integer = Array.IndexOf(txtOpenClaim, eventSender)
        Dim RResult(,) As Object = Nothing
        Dim lReturnCode, lPolicyID As Integer
        Dim sPolicyNo As String = ""
        Dim dtStartDate, dtEndDate As Date, dtInceptionDate As Date

        If Index = g_nLOSS_DATE Or Index = g_nLOSS_TIME Then
            lPolicyID = g_lPolicyID

            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtOpenClaim(g_nLOSS_DATE))
            If txtOpenClaim(g_nLOSS_DATE).Text <> "" then                                                                                                                    
               m_lReturn = g_oBusiness.GetPolicyForClaimDate(v_dtClaimDate:=CDate(txtOpenClaim(g_nLOSS_DATE).Text), r_lInsuranceFileCnt:=lPolicyID, r_sPolicyNumber:=sPolicyNo, r_dtStartDate:=dtStartDate, r_dtEndDate:=dtEndDate, r_lReturnCode:=lReturnCode, r_dtInceptionDate:=dtInceptionDate)
            End If   
            If lPolicyID = 0 Then
                lPolicyID = g_lPolicyID
            End If
            m_bLossDateTime = m_sLossDate <> txtOpenClaim(g_nLOSS_DATE).Text Or m_sLossTime <> txtOpenClaim(g_nLOSS_TIME).Text
        End If

        If IsDate(m_sLossDate) Then
            If m_sLossDate < m_dtInceptionDate AndAlso m_bDisplayValidVersionEnabled Then
                cboRiskType.Items.Clear()
                m_bClearRiskType = True
            Else
                If IsDate(txtOpenClaim(g_nLOSS_DATE).Text) AndAlso txtOpenClaim(g_nLOSS_TO_DATE).Text <> txtOpenClaim(g_nLOSS_DATE).Text Then
                    m_lReturn = g_oBusiness.GetRiskDetails(lPolicyID, CDate(txtOpenClaim(g_nLOSS_DATE).Text), RResult, g_lClaimID)
                    If Information.IsArray(RResult) Then
                        Call LoadDataInCombo(cboRiskType, RResult, LBound(RResult, 2), UBound(RResult, 2) + 1)
        End If

                End If
            End If
        End If

        Select Case Index
            Case g_nLOSS_DATE, g_nREPORTED_TO_DATE
                'TN20010904 - start
                If g_nLOSS_DATE = Index Then
                    txtOpenClaim(g_nLOSS_TO_DATE).Text = txtOpenClaim(g_nLOSS_DATE).Text
                End If
                'TN20010904 - end
            Case g_nREPORTED_DATE
                m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtOpenClaim(g_nREPORTED_DATE))
            Case g_nLOSS_TO_DATE
                m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtOpenClaim(g_nLOSS_TO_DATE))
        End Select
        Exit Sub
        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtOpenClaim_LostFocus", vApp:=ACApp, vClass:=ACClass, vMethod:="txtOpenClaim_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub
    End Sub
    'MSS250901 - Merge end

    Private Sub chkVATRegistered_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkVATRegistered.CheckStateChanged


        Select Case g_nPMMode
            'DC081103 -PN8955 -added EDITADDMODE
            Case g_nADDMODE, g_nEDITMODE, g_nEDITADDMODE


                txtOpenClaim(g_nCLIENT_VAT_REGNO).Enabled = chkVATRegistered.CheckState = CheckState.Checked

        End Select

    End Sub

    '####       CURRENTLY NOT IN USE - Sravan       ####
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oFormFields = New iPMFormControl.FormFields()

            'Handler
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboHandler, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Progress Status
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboProgressStatus, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Claim Status ID
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLAIM_STATUS), lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Claim Status Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLAIM_STATUS_DATE), lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Claim Status Time
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLAIM_STATUS_TIME), lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Description
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nDESCRIPTION), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Primary Cause
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPrimaryCausationCode, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Secondary Cause
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboSecondaryCausationCode, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Catastrophe Code
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCatastropheCode, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Location
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nLOCATION), lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Town
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTown, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Loss From Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nLOSS_DATE), lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Loss From Time
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nLOSS_TIME), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Loss To Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nLOSS_TO_DATE), lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Reported Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nREPORTED_DATE), lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Reported Time
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nREPORTED_TIME), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Reported To Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nREPORTED_TO_DATE), lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Reported To Time
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nREPORTED_TO_TIME), lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Last Modified Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nLAST_MODIFIED_DATE), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Last Modified Time
            'developer guide no. repalace lMandotory:= by lMandatory:=
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nLAST_MODIFIED_TIME), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Risk Type
            If Not txtOpenClaim(g_nRISK_TYPE) Is Nothing Then
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nRISK_TYPE), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            End If
            'Currency
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Information
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=chkInfoOnly, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Likely Claim
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=chkLikelyClaim, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Client Name
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLIENT_NAME), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            'Client Address
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLIENT_ADDRESS), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Client Tel No
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLIENT_TELNO), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Client Fax No
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLIENT_FAXNO), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Client Mobile No
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLIENT_MOBILENO), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Client Email
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLIENT_EMAIL), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Client VAT Registered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=chkVATRegistered, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Client VAT Regd. No
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLIENT_VAT_REGNO), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Client Claim No
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nCLIENT_CLAIMNO), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Insurer Name
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nINSURER_NAME), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Insurer Address
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nINSURER_ADDRESS), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Insurer Tel No.
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nINSURER_TELNO), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Insurer Fax No.
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nINSURER_FAXNO), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Insurer Contact
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nINSURER_CONTACT), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Insurer EMail
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nINSURER_EMAIL), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Insurer Insurer Claim No.
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOpenClaim(g_nINSURER_CLAIMNO), lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Comments
            '  m_lReturn = m_oFormFields.AddNewFormField( _
            ''          ctlControl:=txtComments, _
            ''          lMandatory:=PMMandatory)

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
    ' Name: InterfaceToBusiness
    '
    ' Description:  To write the values from the controls to the
    '               Local variables
    '
    ' ***************************************************************** '
    Private Function InterfaceToBusiness() As Integer

        'Changes by Sameer : 10-07-00
        'This is the Error Constant defined if the Non Mandatory Combo Boxes are  NOT Selected
        '
        'JMK(05/04/2001)
        '       - ensure m_bDataChanged is set for all fields newly editable in Maintain Claim

        Dim result As Integer = 0
        Const nINVALIDPROPERTYVALUEERROR As Integer = 381

        Dim vTemp As String = ""
        ' Tomo190402 - start
        Dim lClaimId As Integer
        ' Tomo190402 - end
        'DC150402 -Start
        Dim sTemp As String = ""
        Dim sTextLine As String = ""
        'DC150402 -End

        Try

            'Tomo190402 - start
            result = gPMConstants.PMEReturnCode.PMTrue

            g_sClaimNo = txtClaimNumber.Text.Trim()
            'Tomo190402 - end

            vTemp = g_sDescription
            g_sDescription = txtOpenClaim(g_nDESCRIPTION).Text
            If g_sDescription <> vTemp Then
                m_bDataChanged = True
            End If

            'JMK(05/04/2001) - Progress Status
            vTemp = CStr(g_lProgressStatusID)
            g_lProgressStatusID = VB6.GetItemData(cboProgressStatus, cboProgressStatus.SelectedIndex)
            If g_lProgressStatusID <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            vTemp = CStr(g_lPrimaryCauseID)
            If cboPrimaryCausationCode.SelectedIndex <> -1 Then
                g_lPrimaryCauseID = VB6.GetItemData(cboPrimaryCausationCode, cboPrimaryCausationCode.SelectedIndex)
            Else
                g_lPrimaryCauseID = 0
            End If
            If g_lPrimaryCauseID <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If


            vTemp = CStr(g_lSecondaryCauseID)
            If cboSecondaryCausationCode.SelectedIndex <> -1 Then
                g_lSecondaryCauseID = VB6.GetItemData(cboSecondaryCausationCode, cboSecondaryCausationCode.SelectedIndex)
            Else
                g_lSecondaryCauseID = 0
            End If
            If g_lSecondaryCauseID <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If


            'JMK(05/04/2001) - Catastrophe Code
            vTemp = CStr(g_lCatastropheCodeID)
            If cboCatastropheCode.SelectedIndex <> -1 Then
                g_lCatastropheCodeID = VB6.GetItemData(cboCatastropheCode, cboCatastropheCode.SelectedIndex)
            Else
                g_lCatastropheCodeID = 0
            End If
            If g_lCatastropheCodeID <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            'JMK(05/04/2001) - Handler
            vTemp = CStr(g_lHandlerID)
            If cboHandler.SelectedIndex <> -1 Then
                g_lHandlerID = VB6.GetItemData(cboHandler, cboHandler.SelectedIndex)
            Else
                g_lHandlerID = 0
            End If
            If g_lHandlerID <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            'JMK(05/04/2001) - Town
            vTemp = CStr(g_lTown)
            If cboTown.SelectedIndex <> -1 Then
                g_lTown = VB6.GetItemData(cboTown, cboTown.SelectedIndex)
            Else
                g_lTown = 0
            End If
            If g_lTown <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            vTemp = CStr(g_lCurrencyID)
            g_lCurrencyID = VB6.GetItemData(cboCurrency, cboCurrency.SelectedIndex)
            If g_lCurrencyID <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            'JMK(05/04/2001) bug#464 Claim Details tab
            'include edit mode
            Dim TempDate As Date
            vTemp = StringsHelper.Format(g_sLossFromDate, ACDateConversion) & " " & (IIf(DateTime.TryParse(g_sLossFromDate, TempDate), TempDate.ToString("HH:mm"), g_sLossFromDate))

            'DC081203 -PN8955 -added EDITADDMODE
            If (g_nPMMode = g_nADDMODE Or g_nPMMode = g_nEDITMODE) Or g_nPMMode = g_nEDITADDMODE Then
                g_sLossFromDate = g_dtLossDate & " " & txtOpenClaim(g_nLOSS_TIME).Text
                If g_sLossFromDate <> vTemp Then
                    m_bDataChanged = True
                End If
            End If

            'JMK(05/04/2001) include edit mode
            '*JMK(05/04/2001) take date format out
            'DC081203 -PN8955 -added EDITADDMODE
            If (g_nPMMode = g_nADDMODE Or g_nPMMode = g_nEDITMODE) Or g_nPMMode = g_nEDITADDMODE Then
                vTemp = g_sLossToDate
                g_sLossToDate = g_dtLossToDate
                If g_sLossToDate <> vTemp Then
                    m_bDataChanged = True
                End If
            End If
            '            If g_dtLossToDate <> "" Then
            '                'DC120301 format wrong way round
            '                'g_sLossToDate = Format(g_dtLossToDate, "m/d/yyyy")
            '                g_sLossToDate = Format(g_dtLossToDate, "d/m/yyyy")
            '            Else
            '                g_sLossToDate = ""
            '            End If

            'DC070203
            'IS1910
            'allow reported dates to be amendable when maintaining claim
            'If g_nPMMode = g_nADDMODE Then
            g_sReportedDate = g_dtReportedDate & " " & g_dtReportedTime
            'End If

            vTemp = (g_dtReportedToDate & " " & g_dtReportedToTime).Trim()
            g_sReportedToDate = (g_dtReportedToDate & " " & g_dtReportedToTime).Trim() '.txtopenclaim(g_nREPORTED_TO_DATE).Text
            If g_sReportedToDate <> vTemp Then
                m_bDataChanged = True
            End If

            'JMK(17/04/2001)- Start Commented out this lot.
            '        If g_nPMMode = g_nADDMODE Then
            '            If g_dtReportedToDate <> "" Then
            '                'DC120301 format wrong way round
            '                'g_sReportedToDate = Format(g_dtReportedToDate + " " + g_dtReportedToTime, "m/d/yyyy hh:mm")
            '                g_sReportedToDate = Format(g_dtReportedToDate + " " + g_dtReportedToTime, "d/m/yyyy hh:mm")
            '            Else
            '                g_sReportedToDate = ""
            '            End If
            '        End If
            'JMK(17/04/2001)- End.

            vTemp = CStr(g_nInfoOnly)
            If chkInfoOnly.CheckState = CheckState.Unchecked Or chkInfoOnly.CheckState = CheckState.Unchecked Then
                g_nInfoOnly = 0
            Else
                g_nInfoOnly = 1
            End If
            If StringsHelper.ToDoubleSafe(vTemp) <> g_nInfoOnly Then
                m_bDataChanged = True
            End If

            'RWH(14/11/2000) If this is NOT for Info only then generate
            'a Full Claim No.
            'DC280601 only for underwriting though


            If cboUnderwritingYearID.SelectedIndex <> -1 Then
                g_vUnderwritingYearID = VB6.GetItemData(cboUnderwritingYearID, cboUnderwritingYearID.SelectedIndex)
            End If

            If (g_nInfoOnly = 0) And (m_lClaimStatusonLoad = CLMProvisionalOpenClaim) Then
                'Tomo190402 - start
                '                m_lReturn = GenerateClaimNumber(ACFullClaim)

                If (g_sClaimNo = g_sProvisionalClaimNo) Or (g_sClaimNo = "") Then
                    m_lReturn = GenerateClaimNumber(ACFullClaim)

                    g_sFullClaimNo = g_sClaimNo
                End If
                'Tomo190402 - end

            End If

            'Tomo190402 - start
            If g_nPMMode = g_nADDMODE Then
                If g_sClaimNo <> g_sProvisionalClaimNo Then
                    If g_sClaimNo <> g_sFullClaimNo Then

                        m_lReturn = g_oBusiness.CheckClaimNumber(v_sClaimNumber:=g_sClaimNo, r_lClaimID:=lClaimId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If lClaimId <> 0 Then

                            MessageBox.Show("Claim Number already exists", "Claim Details", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            g_sClaimNo = g_sProvisionalClaimNo

                            txtClaimNumber.Text = g_sClaimNo

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            End If
            'Tomo190402 - end
            Dim sOptionValue As String = String.Empty
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOptionValue, v_iSourceID:=g_oObjectManager.SourceID)

            If Not String.IsNullOrEmpty(sOptionValue) AndAlso sOptionValue = "2" Then
            If g_nPMMode = g_nADDMODE Then
                If txtClaimNumber.Text <> "" Then
                    If IsValidString(txtClaimNumber.Text) = False Then
                        MessageBox.Show("Claim Number can't contain any of the following characters. " & vbNewLine & ":~ "" # % & * : < > ? / \ { } |", "Mandatory Field - Trading Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        'sj 18/06/2002 - start
                        If txtClaimNumber.Visible Then
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                            txtClaimNumber.Focus()
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'sj 18/06/2002 - end
                        Return result
                    End If
                End If
            End If
            End If


            vTemp = CStr(g_nLikelyClaim)
            If chkLikelyClaim.CheckState = CheckState.Unchecked Or chkLikelyClaim.CheckState = CheckState.Unchecked Then
                g_nLikelyClaim = 0
            Else
                g_nLikelyClaim = 1
            End If
            If StringsHelper.ToDoubleSafe(vTemp) <> g_nLikelyClaim Then
                m_bDataChanged = True
            End If

            'JMK (05/04/2001) - Location
            vTemp = g_sLocation
            g_sLocation = txtOpenClaim(g_nLOCATION).Text.Trim()
            If g_sLocation <> vTemp Then
                m_bDataChanged = True
            End If

            g_sClientName = txtOpenClaim(g_nCLIENT_NAME).Text

            If g_nADDMODE Then
                g_sClientAddress = g_lClientAdressCnt
            End If

            If txtOpenClaim(g_nCLIENT_ADDRESS).Text.Trim() <> m_sClientTemp.Trim() Then
                m_bDataChanged = True
            End If

            vTemp = g_sClientTelNo.Trim()
            g_sClientTelNo = txtOpenClaim(g_nCLIENT_TELNO).Text.Trim()
            If g_sClientTelNo <> vTemp Then
                m_bDataChanged = True
            End If

            vTemp = g_sClientTelNoOff.Trim()
            'Added Office Telephone No of Client -Pandu
            g_sClientTelNoOff = txtOpenClaim(g_nCLIENT_TELNOOFF).Text.Trim()
            If g_sClientTelNoOff <> vTemp Then
                m_bDataChanged = True
            End If

            vTemp = g_sClientFaxNo.Trim()
            g_sClientFaxNo = txtOpenClaim(g_nCLIENT_FAXNO).Text.Trim()
            If g_sClientFaxNo <> vTemp Then
                m_bDataChanged = True
            End If

            vTemp = g_sClientMobileNo.Trim()
            g_sClientMobileNo = txtOpenClaim(g_nCLIENT_MOBILENO).Text.Trim()
            If g_sClientMobileNo <> vTemp Then
                m_bDataChanged = True
            End If

            vTemp = g_sClientEMail.Trim()
            g_sClientEMail = txtOpenClaim(g_nCLIENT_EMAIL).Text.Trim()
            If g_sClientEMail <> vTemp Then
                m_bDataChanged = True
            End If

            vTemp = g_sClientClaimNo.Trim()
            g_sClientClaimNo = txtOpenClaim(g_nCLIENT_CLAIMNO).Text.Trim()
            If g_sClientClaimNo <> vTemp Then
                m_bDataChanged = True
            End If

            g_sInsurerName = txtOpenClaim(g_nINSURER_NAME).Text.Trim()

            'Changed this line of code as addresscnt should go -Pandu
            'g_sInsurerAddress = .txtOpenClaim(g_nINSURER_ADDRESS).Text
            'vTemp = g_sInsurerAddress

            If g_nADDMODE Then
                g_sInsurerAddress = g_lInsurerAdressCnt
            End If
            If txtOpenClaim(g_nINSURER_ADDRESS).Text.Trim() <> m_sInsurertemp.Trim() Then
                m_bDataChanged = True
            End If

            vTemp = g_sInsurerTelNo.Trim()
            g_sInsurerTelNo = txtOpenClaim(g_nINSURER_TELNO).Text.Trim()
            If g_sInsurerTelNo <> vTemp Then
                m_bDataChanged = True
            End If

            vTemp = g_sInsurerFaxNo.Trim()
            g_sInsurerFaxNo = txtOpenClaim(g_nINSURER_FAXNO).Text.Trim()
            If g_sInsurerFaxNo <> vTemp Then
                m_bDataChanged = True
            End If

            vTemp = g_sInsurerEmail.Trim()
            g_sInsurerEmail = txtOpenClaim(g_nINSURER_EMAIL).Text.Trim()
            If g_sInsurerEmail <> vTemp Then
                m_bDataChanged = True
            End If

            vTemp = g_sInsurerClaimNo.Trim()
            g_sInsurerClaimNo = txtOpenClaim(g_nINSURER_CLAIMNO).Text.Trim()
            If g_sInsurerClaimNo <> vTemp Then
                m_bDataChanged = True
            End If

            vTemp = g_sInsurerContact.Trim()
            g_sInsurerContact = txtOpenClaim(g_nINSURER_CONTACT).Text.Trim()
            If g_sInsurerContact <> vTemp Then
                m_bDataChanged = True
            End If

            vTemp = CStr(g_nVATRegistered)
            If chkVATRegistered.CheckState = CheckState.Unchecked Or chkVATRegistered.CheckState = CheckState.Unchecked Then
                g_nVATRegistered = 0
            Else
                g_nVATRegistered = 1
            End If

            If g_nVATRegistered <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            vTemp = g_sVATRegisteredNo.Trim()
            g_sVATRegisteredNo = txtOpenClaim(g_nCLIENT_VAT_REGNO).Text.Trim()
            If g_sVATRegisteredNo <> vTemp Then
                m_bDataChanged = True
            End If


            If g_nPMMode = gPMConstants.PMEComponentAction.PMAdd Then
                'developer guide no.(Modified as per required)
                g_sClaimsStatusDate = Now
            End If

            'DC081203 -PN8955 -added EDITADDMODE
            If g_nPMMode = gPMConstants.PMEComponentAction.PMEdit Or g_nPMMode = 5 Then
                If m_lClaimStatusonLoad <> g_lClaimStatusID Then
                    'developer guide no.(Modified as per required)
                    g_sClaimsStatusDate = StringsHelper.Format(DateTime.Today, ACDateConversion)
                End If
            End If

            'RWH(06/03/2001) Claims stores risk_cnt in risk_type_id !!!
            If cboRiskType.SelectedIndex <> -1 Then
                g_lRiskTypeID = VB6.GetItemData(cboRiskType, cboRiskType.SelectedIndex)
            Else
                g_lRiskTypeID = 0
            End If

            vTemp = CStr(g_lUserDefFldA)
            If cboUDFA.ListIndex <> -1 Then
                g_lUserDefFldA = cboUDFA.ItemId '(cboUDFA.ListIndex)
            Else
                g_lUserDefFldA = 0
            End If

            If g_lUserDefFldA <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            vTemp = CStr(g_lUserDefFldB)
            If cboUDFB.ListIndex <> -1 Then
                g_lUserDefFldB = cboUDFB.ItemId '(cboUDFB.ListIndex)
            End If

            If g_lUserDefFldB <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            vTemp = CStr(g_lUserDefFldC)
            If cboUDFC.ListIndex <> -1 Then
                g_lUserDefFldC = cboUDFC.ItemId '(cboUDFC.ListIndex)
            End If

            If g_lUserDefFldC <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            vTemp = CStr(g_lUserDefFldD)
            If cboUDFD.ListIndex <> -1 Then
                g_lUserDefFldD = cboUDFD.ItemId '(cboUDFD.ListIndex)
            End If

            If g_lUserDefFldD <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            vTemp = CStr(g_lUserDefFldE)

            If cboUDFE.ListIndex <> -1 Then
                g_lUserDefFldE = cboUDFE.ItemId '(cboUDFE.ListIndex)
            End If

            If g_lUserDefFldE <> StringsHelper.ToDoubleSafe(vTemp) Then
                m_bDataChanged = True
            End If

            'DC240402 -Start

            ' JMK(05/04/2001) This should only be set if the claim was modified
            If m_bDataChanged Then
                'developer guide no.(Modifies as per required)
                g_sLastModifiedDate = StringsHelper.Format(DateTime.Today, ACDateConversion)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.


            '   Start of Changes by Sameer 10-07-00 12:15 pm
            '   Changes by Sameer for the Empty Non Mandatory Combo Boxes
            '   By saying Resume Next we are passing this Error by putting Zero Values in the respective fields
            If Information.Err().Number = nINVALIDPROPERTYVALUEERROR Then ' nINVALIDPROPERTYVALUEERROR = 381


            End If
            '   End of the Changes by Sameer.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to InterfaceToBusiness", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description:  To write the values from the Business Object to
    '               the Form Controls
    '
    ' ***************************************************************** '
    Private Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Call Method to write data from Business to Local Variables
            m_lReturn = BusinessToData()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Call Method to write data from Local Variables to Form Controls
            m_lReturn = DisplayPolicyDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to BusinessToInterface", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: IsTime
    '
    ' Description:  To Check if the entered time value is in the correct
    '               Time Format
    '               INPUT : Time as string
    '               OUTPUT: True if correct format else False
    '
    ' ***************************************************************** '
    Private Function IsTime(ByRef sTime As String) As Boolean

        Dim result As Boolean = False
        Dim dtCheckTime As Date

        Try

            result = True

            sTime = sTime.Trim()

            If sTime <> "" Then

                If sTime.Length <= 5 Then

                    dtCheckTime = CDate(sTime)

                Else

                    result = False

                End If

            End If

            Return result

        Catch



            ' Error Section.




            Return False
        End Try

    End Function


    ' ***************************************************************** '
    ' Name:         DisplayMessage
    '
    ' Description:  This function is used to display he Error Messages for this Form.
    '               We are passing two parameters MessageCount which is the
    '               Constant defined in the Resource file
    '                The Title is the Error Message Text for the same.
    '
    ' ***************************************************************** '

    Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.


            sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the status message.

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub chkInfoOnly_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkInfoOnly.CheckStateChanged

        'DC081203 -PN8955 -added EDITADDMODE
        If (g_nPMMode = g_nADDMODE Or g_nPMMode = g_nEDITMODE) Or g_nPMMode = g_nEDITADDMODE Then

            If chkInfoOnly.CheckState = CheckState.Unchecked Or chkInfoOnly.CheckState = CheckState.Unchecked Then

                chkLikelyClaim.CheckState = CheckState.Unchecked
                chkLikelyClaim.Enabled = False

                'Thinh Nguyen 25/07/2003 (start) - when turn off info only check set flag so that full claim number get generated
                'DC081203 -PN8955 -added EDITADDMODE
                If g_nPMMode = g_nEDITMODE Or g_nPMMode = g_nEDITADDMODE Then
                    g_sProvisionalClaimNo = g_sClaimNo
                End If
                'Thinh Nguyen 25/07/2003 (end) - when turn off info only check set flag so that full claim number get generated
            Else

                If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                    chkLikelyClaim.Enabled = True
                End If

            End If

        End If

    End Sub



    '***************************************************************** '
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

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MSS250901 - Inserted for merge
            'AK 180401 - begin

            m_lReturn = g_oBusiness.GetPolicyType(g_lPolicyID, g_sPolicyType)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'AK 180401 - end
            'PN: 73770
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                m_lReturn = GetClaimHandler()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                m_lReturn = GetLookupDetails(sLookupTable:="Handler", ctlLookup:=cboHandler)

            End If

            m_lReturn = GetProgressStatus()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'MSS250901 - Merge end

            '    m_lReturn& = GetLookupDetails(sLookupTable:="Progress_Status", _
            ''            ctlLookup:=cboProgressStatus)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            ' RDT 08/04/03 - Start - fill primary cause combo with valid causes for the product
            '                      - Underwriting only - IAG 215 Spec



            m_lReturn = g_oBusiness.GetValidPrimaryCauses(g_lPolicyID, m_vPrimaryCauseArray)
            ' Check for errors.
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = PopulatePrimaryCause()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RDT 08/04/03 - End

            m_lReturn = GetLookupDetails(sLookupTable:="Catastrophe_Code", ctlLookup:=cboCatastropheCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RDC 02062004 only get currencies for the branch
            'm_lReturn& = GetLookupDetails(sLookupTable:="Currency", _
            'ctlLookup:=cboCurrency)

            'AR20050309 - PN19140 (on behalf of DD)

            '    m_lReturn = g_oBusiness.RetrieveCurrenciesForBranch( _
            ''                                        iSourceID:=g_iSourceID, _
            ''                                        vReturnArray:=vReturnArray)
            '
            '    If m_lReturn <> PMTrue Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            '    For iLoop = LBound(vReturnArray, 2) To UBound(vReturnArray, 2)
            '        cboCurrency.AddItem vReturnArray(1, iLoop)
            '        cboCurrency.ItemData(cboCurrency.NewIndex) = vReturnArray(0, iLoop)
            '    Next
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = GetLookupDetails(sLookupTable:="Town", ctlLookup:=cboTown)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:="Underwriting_Year", ctlLookup:=cboUnderwritingYearID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'S4B Claims Enhancements R&D 2005
            cboAtFault.Items.Clear()
            Dim cboAtFault_NewIndex As Integer = -1
            cboAtFault_NewIndex = cboAtFault.Items.Add("")
            VB6.SetItemData(cboAtFault, cboAtFault_NewIndex, -1)

            m_lReturn = GetLookupDetails(sLookupTable:="Claim_At_Fault", ctlLookup:=cboAtFault)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboStandardExcess.Items.Clear()
            Dim cboStandardExcess_NewIndex As Integer = -1
            cboStandardExcess_NewIndex = cboStandardExcess.Items.Add("")
            VB6.SetItemData(cboStandardExcess, cboStandardExcess_NewIndex, -1)
            m_lReturn = GetLookupDetails(sLookupTable:="Policy_Deductible", ctlLookup:=cboStandardExcess)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Get all of the lookup values.

            m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)



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
    'Developer Guide no 153
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        'MSS250901 - Added for merge
        'AK 180401
        Dim sCode As String = ""
        Const ACDetailCode As Integer = 2
        'MSS250901 - Merge end

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If Convert.ToString(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
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

            'MSS250901 - Added for merge
            'AK 18042001
            If sLookupTable.Trim() = "Primary_Cause" Then
                Select Case (g_sPolicyType)
                    Case ACGeminiIIMotor
                        sCode = "M-" & ACGIIMPrimaryCause & "-"
                    Case ACGeminiIIHouseHold
                        sCode = "M-" & ACGIIHPrimaryCause & "-"
                    Case ACCommercialVehicle
                        sCode = "M-" & ACCVPrimaryCause & "-"
                End Select
            End If
            'MSS250901 - Merge end

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.

                'MSS250901 - Added for merge
                'AK 180401
                ' if it is a gemini II policy and list-item code contains the relevant code
                Dim newindex As Integer
                If sCode <> "" Then
                    If CStr(m_vLookupDetails(ACDetailCode, lCntr)).Substring(0, Math.Min(CStr(m_vLookupDetails(ACDetailCode, lCntr)).Length, sCode.Length)) = sCode Then

                        'Developer Guide no 153
                        newindex = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))
                    End If
                Else
                    If CStr(m_vLookupDetails(ACDetailCode, lCntr)).Substring(0, Math.Min(CStr(m_vLookupDetails(ACDetailCode, lCntr)).Length, 2)) <> "M-" Then


                        'Developer Guide no 153
                        newindex = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))
                    End If
                End If
                'MSS250901 - Merge end

                'SP150998 - compare long value not string
                ' Check if this is the selected index.
                '        If (m_vLookupValues(ACValueID, lRow&) = _
                ''        CLng(m_vLookupDetails(ACDetailKey, lCntr&))) Then
                '            ctlLookup.ListIndex = ctlLookup.NewIndex
                '        End If

                If Convert.ToString(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'Developer Guide no.28
                        ctlLookup.SelectedIndex = newindex
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            '    If (m_vLookupValues(ACValueID, lRow&) = "") Then
            '        ctlLookup.ListIndex = 0
            '    End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '***************************************************************** '
    ' Name: DisplayUserDefinedFields
    '
    ' Description: Displays all of the User Definedlookup details using the lookup
    '              values/details.
    ' Edit History:Pandu
    '
    ' Date :20/09/2000
    ' ***************************************************************** '
    Public Function DisplayUserDefinedFields() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACUDFAOptionNumber As Integer = 2003
            Const ACUDFBOptionNumber As Integer = 2004
            Const ACUDFCOptionNumber As Integer = 2005
            Const ACUDFDOptionNumber As Integer = 2006
            Const ACUDFEOptionNumber As Integer = 2007
            Const ACFirstValue As Integer = 0
            'Const ACNoValue As Integer = -1
            Const ACColon As String = ":"


            Dim vResultArray(,) As Object
            Dim nOptionValue As Integer

            'Populate User Defined Field A

            m_lReturn = g_oBusiness.getOption(ACUDFAOptionNumber, nOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then


                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            cboUDFA.Table = nOptionValue

            If g_nPMMode = g_nADDMODE Then

                '        cboUDFA.DefaultItemId = ACNoValue

            End If

            cboUDFA.RefreshList()

            '******** Bugid-35 Start of Change for getting the Caption for User Defined Fields
            '*******Dtd 20-10-2000 Pandu

            'DC231100 - Start - Do not bother looking for caption if no table set up
            If nOptionValue < 1 Then

                'Do not display if no table entered
                cboUDFA.Visible = False
                lblUserDefinedFieldA.Visible = False
                cboUDFA.ListIndex = -1

            Else
                'DC231100 - End

                'Get the Caption

                m_lReturn = g_oBusiness.GetUserDefinedCaption(vResultArray, nOptionValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                        'DC231100 do not display if no table entered
                        cboUDFA.Visible = False
                        lblUserDefinedFieldA.Visible = False
                        cboUDFA.ListIndex = -1

                    Else


                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If

                If Information.IsArray(vResultArray) Then


                    lblUserDefinedFieldA.Text = CStr(vResultArray(ACFirstValue, ACFirstValue)) & ACColon

                End If

                'DC231100
            End If

            'Intialise for next fetch

            vResultArray = Nothing

            '***************** Bugid-35 End of Change For User Defined Caption

            'Populate User Defined Field B

            m_lReturn = g_oBusiness.getOption(ACUDFBOptionNumber, nOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then


                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            cboUDFB.Table = nOptionValue

            If g_nPMMode = g_nADDMODE Then

                '       cboUDFB.DefaultItemId = ACNoValue

            End If

            cboUDFB.RefreshList()

            '******** Bugid-35 Start of Change for getting the Caption for User Defined Fields
            '*******Dtd 20-10-2000 Pandu

            'DC231100 - Start - Do not bother looking for caption if no table set up
            If nOptionValue < 1 Then

                'Do not display if no table entered
                cboUDFB.Visible = False
                lblUserDefinedFieldB.Visible = False
                cboUDFB.ListIndex = -1

            Else
                'DC231100 - End

                'Get the Caption

                m_lReturn = g_oBusiness.GetUserDefinedCaption(vResultArray, nOptionValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                        'DC231100 do not display if no table entered
                        cboUDFB.Visible = False
                        lblUserDefinedFieldB.Visible = False
                        cboUDFB.ListIndex = -1

                    Else


                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If

                If Information.IsArray(vResultArray) Then


                    lblUserDefinedFieldB.Text = CStr(vResultArray(ACFirstValue, ACFirstValue)) & ACColon

                End If

                'DC231100
            End If

            'Intialise for next fetch

            vResultArray = Nothing

            '***************** Bugid-35 End of Change For User Defined Caption

            'Populate User Defined Field C

            m_lReturn = g_oBusiness.getOption(ACUDFCOptionNumber, nOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then


                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            cboUDFC.Table = nOptionValue

            If g_nPMMode = g_nADDMODE Then

                '        cboUDFC.DefaultItemId = ACNoValue

            End If

            cboUDFC.RefreshList()

            '******** Bugid-35 Start of Change for getting the Caption for User Defined Fields
            '*******Dtd 20-10-2000 Pandu

            'DC231100 - Start - Do not bother looking for caption if no table set up
            If nOptionValue < 1 Then

                'Do not display if no table entered
                cboUDFC.Visible = False
                lblUserDefinedFieldC.Visible = False
                cboUDFC.ListIndex = -1

            Else
                'DC231100 - End

                'Get the Caption

                m_lReturn = g_oBusiness.GetUserDefinedCaption(vResultArray, nOptionValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                        'DC231100 do not display if no table entered
                        cboUDFC.Visible = False
                        lblUserDefinedFieldC.Visible = False
                        cboUDFC.ListIndex = -1

                    Else


                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If

                If Information.IsArray(vResultArray) Then


                    lblUserDefinedFieldC.Text = CStr(vResultArray(ACFirstValue, ACFirstValue)) & ACColon

                End If

                'DC231100
            End If

            'Intialise for next fetch

            vResultArray = Nothing

            '***************** Bugid-35 End of Change For User Defined Caption

            'Populate User Defined Field D

            m_lReturn = g_oBusiness.getOption(ACUDFDOptionNumber, nOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then


                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            cboUDFD.Table = nOptionValue

            If g_nPMMode = g_nADDMODE Then

                '        cboUDFD.DefaultItemId = ACNoValue

            End If

            cboUDFD.RefreshList()

            '******** Bugid-35 Start of Change for getting the Caption for User Defined Fields
            '*******Dtd 20-10-2000 Pandu

            'DC231100 - Start - Do not bother looking for caption if no table set up
            If nOptionValue < 1 Then

                'Do not display if no table entered
                cboUDFD.Visible = False
                lblUserDefinedFieldD.Visible = False
                cboUDFD.ListIndex = -1

            Else
                'DC231100 - End

                'Get the Caption

                m_lReturn = g_oBusiness.GetUserDefinedCaption(vResultArray, nOptionValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                        'DC231100 do not display if no table entered
                        cboUDFD.Visible = False
                        lblUserDefinedFieldD.Visible = False
                        cboUDFD.ListIndex = -1

                    Else


                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If

                If Information.IsArray(vResultArray) Then


                    lblUserDefinedFieldD.Text = CStr(vResultArray(ACFirstValue, ACFirstValue)) & ACColon

                End If

                'DC231100
            End If

            'Intialise for next fetch

            vResultArray = Nothing

            '***************** Bugid-35 End of Change For User Defined Caption

            'Populate User Defined Field E

            m_lReturn = g_oBusiness.getOption(ACUDFEOptionNumber, nOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then


                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            cboUDFE.Table = nOptionValue

            If g_nPMMode = g_nADDMODE Then

                '        cboUDFE.DefaultItemId = ACNoValue

            End If

            cboUDFE.RefreshList()

            '******** Bugid-35 Start of Change for getting the Caption for User Defined Fields
            '******* Dtd 20-10-2000 Pandu

            'DC231100 - Start - Do not bother looking for caption if no table set up
            If nOptionValue < 1 Then

                'Do not display if no table entered
                cboUDFE.Visible = False
                lblUserDefinedFieldE.Visible = False
                cboUDFE.ListIndex = -1

            Else
                'DC231100 - End

                'Get the Caption

                m_lReturn = g_oBusiness.GetUserDefinedCaption(vResultArray, nOptionValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                        'DC231100 do not display if no table entered
                        cboUDFE.Visible = False
                        lblUserDefinedFieldE.Visible = False
                        cboUDFE.ListIndex = -1

                    Else


                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If

                If Information.IsArray(vResultArray) Then


                    lblUserDefinedFieldE.Text = CStr(vResultArray(ACFirstValue, ACFirstValue)) & ACColon

                End If

                'DC231100
            End If

            'Intialise for next fetch

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayUserDefinedFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name:         DisplayMessage
    '
    ' Description:  This function is used to display he Error Messages for this Form.
    '               We are passing two parameters MessageCount which is the
    '               Constant defined in the Resource file
    '                The Title is the Error Message Text for the same.
    '
    ' ***************************************************************** '


    'Private Sub DisplayMessageYesNo(ByRef MessageConstant As Integer, ByRef sTitle As String)
    '
    'Static sMessage As String = ""
    '
    'Try 
    '
    ' Get message text if not already present.
    '

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '
    ' Display the status message.
    '
    'MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo)
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    '
    ' Name: GenerateClaimNumber
    '
    ' Description: Calls through to business object to generate claim
    '               number.
    '
    ' History:  10/11/2000 RWH  - Created.
    '           15/10/2001 JMK  - add 2 optional claim date parameters
    ' ***************************************************************** '
    Private Function GenerateClaimNumber(ByRef lBusinessTypeId As Integer) As Integer
        Dim result As Integer = 0
        Dim sClaimNumber As String = ""
        Dim dtReported As Date
        Dim sReportedYear As String = ""
        Dim dtLoss As Date
        Dim sLossYear As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            dtReported = CDate(StringsHelper.Format(txtOpenClaim(g_nREPORTED_DATE).Text, ACDateConversion))
            sReportedYear = Conversion.Str(dtReported.Year).Trim()
            dtLoss = CDate(StringsHelper.Format(txtOpenClaim(g_nLOSS_DATE).Text, ACDateConversion))
            sLossYear = Conversion.Str(dtLoss.Year).Trim()

            'Thinh Nguyen 06/06/2003 (start) - get source from policy not user ( replace g_iSourceID with m_vInsuranceFileDetails(ACInsFileSourceId, 0))

            m_lReturn = g_oBusiness.GenerateClaimNumber(lBusinessTypeId, CInt(m_vInsuranceFileDetails(ACInsFileSourceId, 0)), Conversion.Val(CStr(m_vInsuranceFileDetails(ACInsFileProductId, 0))), Conversion.Val(CStr(m_vInsuranceFileDetails(ACInsFileLeadAgentCnt, 0))), sClaimNumber, sLossYear, sReportedYear, PartyCnt)

            'Thinh Nguyen 06/06/2003 (end) - get source from policy not user ( replace g_iSourceID with m_vInsuranceFileDetails(ACInsFileSourceId, 0))

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If sClaimNumber <> "" Then
                    g_sClaimNo = sClaimNumber
                End If
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateClaimNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnlockClaim
    '
    ' Description:
    '
    ' History: 17/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UnlockClaim(ByVal v_lOriginalClaimID As Integer) As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get bPMLock
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If


            m_lReturn = oPMLock.UnLockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)

            ' Alix - 02/02/2004 - PN10129
            ' Only error if return = PMError. If return = PMFalse, it just means
            ' the claim was not locked in the first place.
            'If (m_lReturn <> PMTrue) Then
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then

                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock the screen", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function SelectParty(ByRef vPartyCnt As Object, ByRef vShortName As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vSpecialParty As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing) As Integer 'CT 19/07/00 added vResolvedName parameter

        Dim result As Integer = 0
        'developer guide no. 108
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 108
            oFindParty = New iPMBFindParty.Interface_Renamed()

            'Set appropriate key if agent only


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lErrorNumber = oFindParty.SetKeys(vKeyArray)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'developer guide no.9
            m_lErrorNumber = oFindParty.Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.CallingAppName = "Claims"

            m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            oFindParty.EnableNewParty = False

            oFindParty.EnableNewParty = False

            m_lErrorNumber = oFindParty.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                vPartyCnt = oFindParty.PartyCnt
                vShortName = oFindParty.ShortName
                vName = oFindParty.LongName
                vResolvedName = oFindParty.ResolvedName
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.Dispose()
            oFindParty = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC251001 -end

    ' ***************************************************************** '
    ' Name:         CheckValidLossDate
    ' Description:  Checks if the entered loss date is within the policy
    '               start and end dates
    ' ***************************************************************** '
    Private Function CheckValidLossDate() As Integer

        Dim nResult As Integer = 0
        Dim sMessage As String = ""
        Dim dtStartDate, dtEndDate As Date, dtInceptionDate As Date
        Dim lReturnCode, lPolicyID As Integer
        Dim sPolicyNo As String = ""
        Dim lReturn As DialogResult
        Dim sOldDate As Date, sOldTime As String

        'Const ReturnCode_Error As Integer = 0
        Const ReturnCode_Ok As Integer = 1
        Const ReturnCode_TooEarly As Integer = 2
        Const ReturnCode_TooLate As Integer = 3
        Const ReturnCode_Voided As Integer = 4
        Dim lPolicyStatus As Integer
        Dim dtLossDate As Date, dtCancellationDate As Date
        Dim bSkipMessage As Boolean

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_bPolicyFound = True
            If (Not String.IsNullOrEmpty(g_sLossFromDate)) Then
                sOldDate = CDate(StringsHelper.Format(g_sLossFromDate, ACDateConversion))
            End If
            If (Not String.IsNullOrEmpty(g_dtLossDate)) Then
                dtLossDate = CDate(StringsHelper.Format(g_dtLossDate, ACDateConversion))
            End If
            Dim TempDate As Date
            sOldTime = IIf(DateTime.TryParse(g_sLossFromDate, TempDate), TempDate.ToString("HH:mm"), g_sLossFromDate)


            If Not Information.IsDate(txtOpenClaim(g_nLOSS_DATE).Text) Then
                'CheckValidLossDate = PMFalse
                Return nResult
                    End If
            m_lReturn = g_oBusiness.GetPolicyStatus(v_lInsuranceFileCnt:=g_lPolicyID, r_lPolicyStatus:=lPolicyStatus)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get policy status", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If

            Select Case lPolicyStatus
                Case 1 'cancelled

                    dtLossDate = CDate(txtOpenClaim(5).Text)

                    ' get the cancellation date in order to determine whether or not
                    ' to display the message

                    lReturn = g_oBusiness.GetCancellationDate(v_lInsuranceFileCnt:=g_lPolicyID, r_dtCancellationDate:=dtCancellationDate)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to get GetCancellationDate", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Function
                    End If

            End Select
            'TN20010823 - end

            '*******************Enhancement ends here ****************************************


            'do this if the either date or time has changed
            If sOldDate <> dtLossDate Or sOldTime <> txtOpenClaim(g_nLOSS_TIME).Text Then
                lPolicyID = g_lPolicyID

                nResult = g_oBusiness.GetPolicyForClaimDate(v_dtClaimDate:=CDate(txtOpenClaim(g_nLOSS_DATE).Text), r_lInsuranceFileCnt:=lPolicyID, r_sPolicyNumber:=sPolicyNo, r_dtStartDate:=dtStartDate, r_dtEndDate:=dtEndDate, r_lReturnCode:=lReturnCode, r_dtInceptionDate:=dtInceptionDate)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check Loss Date against policy cover dates.", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckValidLossDate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return nResult
                End If
                bSkipMessage = False
                ' Alix - 16/02/2004 - Process 52/53
                If (lReturnCode = ReturnCode_TooEarly) And lPolicyID <> g_lPolicyID Then
                    ' User shouldn't change the loss date to a value outside of the current
                    ' policy version
                    sMessage = "The loss date is outside the cover dates for this policy."
                    sMessage = sMessage & Strings.Chr(13) & Strings.Chr(13)
                    sMessage = sMessage & "Cover Inception Date:" & Format(dtInceptionDate, "dd/MM/yyyy")
                    sMessage = sMessage & Strings.Chr(13)
                    sMessage = sMessage & "Cover Expiry Date:" & Format(IIf(lPolicyStatus = 1, dtCancellationDate, dtEndDate), "dd/MM/yyyy") & Strings.Chr(13) & Strings.Chr(10)
                    If lPolicyID <> 0 Then
                        g_lPolicyID = lPolicyID
                    End If
                ElseIf lPolicyStatus = 1 And dtLossDate >= dtCancellationDate Then
                    sMessage = "Policy Cancelled with effect from " & dtCancellationDate.ToString("dd\/MM\/yyyy") & ". Do you wish to proceed?"
                    'sMessage = "Policy Cancelled with effect from " & dtCancellationDate.Day.ToString() & "/" & dtCancellationDate.Month.ToString() & "/" & dtCancellationDate.Year.ToString() & ". Do you wish to proceed?"
                    If MessageBox.Show(sMessage, "Loss Date Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If lPolicyID <> 0 Then
                        g_lPolicyID = lPolicyID
                    End If
                    bSkipMessage = True
                ElseIf lPolicyStatus = 2 Then
                    sMessage = "Warning! This policy is lapsed." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue?"
                    If MessageBox.Show(sMessage, "Loss Date Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If lPolicyID <> 0 Then
                        g_lPolicyID = lPolicyID
                    End If
                    bSkipMessage = True
                ElseIf lReturnCode = ReturnCode_TooLate AndAlso lPolicyID <> g_lPolicyID Then
                    If m_sTransactionType = "C_CO" Then
                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoPolicyFound, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    Else
                        sMessage = "The loss date is outside the cover dates for this policy."
                        sMessage = sMessage & Strings.Chr(13) & Strings.Chr(13)
                        sMessage = sMessage & "Cover Inception Date:" & Format(dtInceptionDate, "dd/MM/yyyy")
                        sMessage = sMessage & Strings.Chr(13)
                        sMessage = sMessage & "Cover Expiry Date:" & Format(IIf(lPolicyStatus = 1, dtCancellationDate, dtEndDate), "dd/MM/yyyy") & Strings.Chr(13) & Strings.Chr(10)
                    End If
                    If lPolicyID <> 0 Then
                        g_lPolicyID = lPolicyID
                    End If

                ElseIf (lReturnCode = ReturnCode_Voided Or lPolicyID <> g_lPolicyID) AndAlso lPolicyStatus = 0 Then
                    If m_sTransactionType = "C_CO" Then
                        sMessage = "Date of Loss amended – Reinsurance may be affected."
                    End If
                    If lPolicyID <> 0 Then
                        g_lPolicyID = lPolicyID
                    End If
                ElseIf lReturnCode = ReturnCode_Ok Then
                    If lPolicyID <> 0 Then
                        g_lPolicyID = lPolicyID
                    End If
                    ' Fine!
                ElseIf lReturnCode = ReturnCode_TooEarly AndAlso m_sTransactionType = "C_CR" Then

                    sMessage = "The loss date is outside the cover dates for this policy."
                    sMessage = sMessage & Strings.Chr(13) & Strings.Chr(13)
                    sMessage = sMessage & "Cover Inception Date:" & Format(dtInceptionDate, "dd/MM/yyyy")
                    sMessage = sMessage & Strings.Chr(13)
                    sMessage = sMessage & "Cover Expiry Date:" & Format(IIf(lPolicyStatus = 1, dtCancellationDate, dtEndDate), "dd/MM/yyyy") & Strings.Chr(13) & Strings.Chr(10)
                    If lPolicyID <> 0 Then
                        g_lPolicyID = lPolicyID
                    End If
                ElseIf lReturnCode = ReturnCode_TooLate And m_sTransactionType = "C_CR" Then

                    sMessage = "The loss date is outside the cover dates for this policy."
                    sMessage = sMessage & Strings.Chr(13) & Strings.Chr(13)
                    sMessage = sMessage & "Cover Inception Date:" & Format(dtInceptionDate, "dd/MM/yyyy")
                    sMessage = sMessage & Strings.Chr(13)
                    sMessage = sMessage & "Cover Expiry Date:" & Format(IIf(lPolicyStatus = 1, dtCancellationDate, dtEndDate), "dd/MM/yyyy") & Strings.Chr(13) & Strings.Chr(10)

                    If lPolicyID <> 0 Then
                        g_lPolicyID = lPolicyID
                    End If
                Else
                    ' We couldn't find a valid policy for an unexpected reason
                    If m_sClaimsCoverBasisCode = "STD" Then 'applicable for standard ClaimsCoverBasis policies only

                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoPolicyFound, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    End If
                    m_bPolicyFound = False
                End If



                ' Report to user
                If sMessage <> "" And bSkipMessage = False Then

                    If m_sTransactionType = "C_CR" And m_bDisplayValidVersionEnabled Then
                        lReturn = MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10), "Loss Date Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    Else
                        If m_bAttachClaimOutsideOfPolicyPeriod = True AndAlso m_sClaimsCoverBasisCode = "STD" Then
                        lReturn = MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue?", "Loss Date Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                        If lReturn = System.Windows.Forms.DialogResult.No Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If
                        Else
                            lReturn = MessageBox.Show(sMessage, "Loss Date Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                           
                            If m_sTransactionType = "C_CR" Then
                                sMessage = "Date of Loss amended – Reinsurance may be affected."
                                lReturn = MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue?", "Loss Date Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                                If lReturn = System.Windows.Forms.DialogResult.No Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                Else
                                    If Not m_bPolicyFound Then
                                        m_bLossDateTime = False
                                    End If
                                End If
                            ElseIf Not m_bPolicyFound Then
                                m_bLossDateTime = False
                            End If
                        End If

                ElseIf m_sTransactionType = "C_CR" And bSkipMessage = False Then
                    sMessage = "Date of Loss amended – Reinsurance may be affected."
                    lReturn = MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue?", "Loss Date Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                    If lReturn = System.Windows.Forms.DialogResult.No Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    Else
                        If Not m_bPolicyFound Then
                            m_bLossDateTime = False
                        End If
                    End If
                End If
            End If

            Return nResult

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckValidLossDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckValidLossDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         CreateDeferredTask
    ' Description:  -
    ' ***************************************************************** '
    Private Function CreateDeferredTask() As Integer
        Dim result As Integer = 0

        Try


            Dim oWrkMgrTaskControl As bPMWrkTaskInstance.TaskControl
            Dim lTaskInstanceCnt, lTaskGroupId As Integer
            Dim vInstall As String = ""
            Dim lUserGroupId As Integer

            ' Initialise Required Variables
            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = iPMFunc.getUnderwritingOrAgency(vInstall)

            lTaskGroupId = 10

            ' Get claim user group id and task group ID

            m_lReturn = g_oBusiness.GetClaimTaskUserGroup(r_lTaskGroupID:=lTaskGroupId, r_lUserGroupID:=lUserGroupId)
            'PN28222
            If lUserGroupId = 0 Then
                MessageBox.Show("User group not present. Task can not be created.", Application.ProductName)
                Return result
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get claim user group id and task group ID", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_oWrkMgrTaskControl As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oWrkMgrTaskControl, "bPMWrkTaskInstance.TaskControl", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oWrkMgrTaskControl = temp_oWrkMgrTaskControl

            'Initialise with the Sirius user and password

            m_lReturn = CType(oWrkMgrTaskControl, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iUserID:=g_oObjectManager.UserID, iSourceID:=g_oObjectManager.SourceID, iLanguageID:=g_oObjectManager.LanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise WorkManager Task Business Object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'Create the WorkManager Task

            m_lReturn = oWrkMgrTaskControl.CreateNewByCode(v_lPMWrkTaskGroupID:=lTaskGroupId, v_sPMWrkTaskCode:="PAYCLM", v_sCustomer:=g_sClientShortName, v_dtTaskDueDate:=DateTime.Today, v_lPMUserGroupID:=lUserGroupId, v_sDescription:="Deferred Reinsurance Claim Payment", v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=0, r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create WorkManager Task.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDeferredTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            oWrkMgrTaskControl = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create WorkManager Task", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDeferredTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         AddPerilForClaimRisk (Private)
    '
    ' Description:  Determine if we should add the perils for claim risk
    '               HERE or wait until the iCLMRiskDetails screen...
    '
    ' Created       Russell Hill 24/02/2003
    '
    ' ***************************************************************** '
    Private Function AddPerilForClaimRisk(ByVal lPolicyID As Integer, ByVal lRiskID As Integer, ByVal lClaimId As Integer) As Integer
        Dim result As Integer = 0

        Try

            Dim o_bCLMRiskDetails As bCLMRiskDetails.Business


            result = gPMConstants.PMEReturnCode.PMTrue


            Dim temp_o_bCLMRiskDetails As Object
            'developer guide no.(Changed to match the dll name)
            m_lReturn = g_oObjectManager.GetInstance(temp_o_bCLMRiskDetails, "bCLMRiskDetails.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            o_bCLMRiskDetails = temp_o_bCLMRiskDetails

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the business object
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bCLMRiskDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPerilForClaimRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            m_lReturn = o_bCLMRiskDetails.AddPerilForClaimRisk(lPolicyID, lRiskID, lClaimId)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the business object
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed when calling bCLMRiskDetails.business.AddPerilForClaimRisk", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPerilForClaimRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process AddPerilForClaimRisk", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPerilForClaimRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*****************************************************************************************
    'if underwriting year switch is on and its on the policy then default to it
    'else if  switch is on and its not on policy then ask user to select it
    '*****************************************************************************************
    Private Function SetUnderWritingYear() As Integer

        Dim result As Integer = 0

        Try
        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = g_oBusiness.GetDefaultUnderwritingYear(v_lPolicyID:=g_lPolicyID, r_vUnderwritingYearID:=g_vUnderwritingYearID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
                Return result
        End If

        'developer guide no.(As per VB Code)
        'start
        If IsNumeric(g_vUnderwritingYearID) Then
            For nCount As Integer = 0 To cboUnderwritingYearID.Items.Count - 1
                If VB6.GetItemData(cboUnderwritingYearID, nCount) = g_vUnderwritingYearID Then
                    cboUnderwritingYearID.SelectedIndex = nCount
                    Exit For
                End If
            Next nCount
        End If

        cboUnderwritingYearID.Enabled = Not IsNumeric(g_vUnderwritingYearID)
        lblUnderwritingYearID.Enabled = True
        Dim dbNumericTemp3 As Double
        lblUnderwritingYearID.Font = VB6.FontChangeBold(lblUnderwritingYearID.Font, Not IsNumeric(g_vUnderwritingYearID))
        'end

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set underwriting year", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUnderWritingYear", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetDuplicateClaims
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetDuplicateClaims(ByVal v_sPolicyNumber As String, ByVal v_dtLossDate As Date, ByVal v_lRiskTypeId As Integer, ByRef r_vDuplicateClaimDetails As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDuplicateClaims"

        Dim lReturn As gPMConstants.PMEReturnCode


        Try

        result = gPMConstants.PMEReturnCode.PMTrue


        lReturn = g_oBusiness.GetDuplicateClaims(v_sPolicyNumber:=v_sPolicyNumber, v_dtLossDate:=v_dtLossDate, v_lRiskTypeId:=v_lRiskTypeId, r_vResults:=r_vDuplicateClaimDetails)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetDuplicateClaims Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: GetDuplicateClaimOverrideUsers
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetDuplicateClaimOverrideUsers(ByRef r_vOverrideUserDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDuplicateClaimOverrideUsers"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

        result = gPMConstants.PMEReturnCode.PMTrue


        lReturn = g_oBusiness.GetDuplicateClaimOverrideUsers(r_vResults:=r_vOverrideUserDetails)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetDuplicateClaimOverrideUsers", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: CheckForDuplicateClaims
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function CheckForDuplicateClaims() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckForDuplicateClaims"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vDuplicateClaimDetails, vDuplicateClaimOverrideUserDetails As Object
        Dim sOptionValue As String = ""
        Dim lRiskTypeId As Integer
        Dim sPolicyNumber As String = ""
        Dim dtLossDate As Date

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        'developer guide no.101
            If m_bDuplicateCheckEnabled = True Then
                sOptionValue = "1"
            Else
                sOptionValue = "0"
            End If

        If sOptionValue = "1" Then

            ' get the details required to find any duplicate claims
            lRiskTypeId = VB6.GetItemData(cboRiskType, cboRiskType.SelectedIndex)
            sPolicyNumber = g_sPolicyNo.Trim()
            dtLossDate = CDate(txtOpenClaim(g_nLOSS_DATE).Text)

            lReturn = CType(GetDuplicateClaims(v_sPolicyNumber:=sPolicyNumber, v_dtLossDate:=dtLossDate, v_lRiskTypeId:=lRiskTypeId, r_vDuplicateClaimDetails:=vDuplicateClaimDetails), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetDuplicateClaims Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if there are duplicate claim details
            If Information.IsArray(vDuplicateClaimDetails) Then

                ' get all defined duplicate claim override user details

                lReturn = CType(GetDuplicateClaimOverrideUsers(vDuplicateClaimOverrideUserDetails), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetDuplicateClaimOverrideUsers Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' if there are no override user details then
                ' indicate this problem to the user....
                If Not Information.IsArray(vDuplicateClaimOverrideUserDetails) Then
                    MessageBox.Show("System Option: 'Duplicate Claim Check Enabled' but no users have been defined with override capability. You will be unable to proceed with this claim entry.", Application.ProductName)
                End If

                ' there are duplicates so


                lReturn = CType(ShowDuplicatesScreen(vDuplicateClaimDetails, vDuplicateClaimOverrideUserDetails), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                        result = gPMConstants.PMEReturnCode.PMCancel

                    ElseIf lReturn = gPMConstants.PMEReturnCode.PMError Then
                        gPMFunctions.RaiseError(kMethodName, "ShowDuplicatesScreen Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

            Else
                ' no possible duplicates so continue without showing screen
            End If
        Else
            ' option not set or turned off so dont do check
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
    ' Name: ShowDuplicatesScreen
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ShowDuplicatesScreen(ByVal vDuplicateClaimDetails(,) As Object, ByVal vDuplicateClaimOverrideUserDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ShowDuplicatesScreen"

        Dim lStatus As gPMConstants.PMEReturnCode
        Dim sUsername As String


        Try

        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no.50
        objfrmDuplicateClaimsOverride = New frmDuplicateClaimsOverride
        ' set screen details

        objfrmDuplicateClaimsOverride.DuplicateClaimDetails = vDuplicateClaimDetails

        objfrmDuplicateClaimsOverride.OverrideUserDetails = vDuplicateClaimOverrideUserDetails

        ' load duplicate screen

        Dim tempLoadForm As frmDuplicateClaimsOverride = objfrmDuplicateClaimsOverride

        ' show duplicate screen

        objfrmDuplicateClaimsOverride.ShowDialog()

        ' get status from form

        lStatus = objfrmDuplicateClaimsOverride.Status

        ' if the user cancels or the screen errors
        ' just return the status
        If lStatus = gPMConstants.PMEReturnCode.PMCancel Then
            result = gPMConstants.PMEReturnCode.PMCancel

        ElseIf lStatus = gPMConstants.PMEReturnCode.PMError Then
            result = gPMConstants.PMEReturnCode.PMError

            ' if the user confirms and provides a valid overriding user and password
        ElseIf lStatus = gPMConstants.PMEReturnCode.PMOK Then

            ' get the approving user id

            sUsername = objfrmDuplicateClaimsOverride.OverrideUserName

            ' save details for event creation
            ' NB: this must take place after the claim has been copied to the work tables
                m_sDuplicateClaimEventDescription = "User " & sUsername & " performed a duplicate " &
                                                " claim override on this claim."

            ' indicate event should be created
            m_bCreateDuplicateClaimOverrideEvent = True


        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Sub ShowRiskEvents(Optional ByVal v_bShowAllClaimVersionsEvents As Boolean = False)

        Dim lClaimId As Integer
        Dim vResultArray(,) As Object
        Dim lInsuranceFolderCnt, lBaseClaimId, lCaseID As Integer
        Dim sCaseNumber As String = ""

        Try


            If m_sTransactionType <> "C_CO" Then

                If v_bShowAllClaimVersionsEvents Then
                    'get base claim id to show events across all versions of the claim

                    m_lReturn = g_oBusiness.GetOriginalClaimNo(v_lClaimId:=g_lClaimID, r_lOriginalClaimID:=lBaseClaimId)

                    'Done as part of PLICO 24_28 issues
                    lClaimId = 0
                    lCaseID = 0 'g_lCaseID
                    sCaseNumber = CStr(0) 'g_sCaseNumber
                Else
                    lClaimId = g_lClaimID
                    lBaseClaimId = 0
                End If

                'get InsuranceFolderCnt

                m_lReturn = g_oBusiness.GetInsuranceFolderCnt(g_lPolicyID, vResultArray)
                If Information.IsArray(vResultArray) Then

                    lInsuranceFolderCnt = CInt(vResultArray(0, 0))
                End If
                ''Done as part of PLICO 24_28 issues
                ' m_lPartyCnt lInsuranceFolderCnt , g_lPolicyID, g_sPolicyNo
                'Developer Guide no. 294
                SharedFiles.iPMBListEvents.g_oObjectManager = g_oObjectManager
                m_lReturn = SharedFiles.iPMBListEvents.ShowEvents(v_lPartyCnt:=0, v_lInsuranceFolderCnt:=0, v_lInsuranceFileCnt:=0, v_lClaimCnt:=lClaimId, v_sInsuranceRef:=CStr(0), v_sClaimRef:=g_sClaimNo, v_sTransactionType:=m_sTransactionType, v_lBaseClaimId:=lBaseClaimId, v_lCaseID:=lCaseID, v_sCaseNumber:=sCaseNumber)
            End If



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskEvents")
                Exit Sub
            End If

        Catch

            'Continue as not serious
            Exit Sub
        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: ProcessClaimTransactionSuppression
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-10-2005 : Claims Transaction Suppression
    ' ***************************************************************** '
    Private Function ProcessClaimTransactionSuppression() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessClaimTransactionSuppression"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim lClaimReserveSuppressed, lClaimPaymentSuppressed, lClaimRecoverySuppressed As Integer

        Dim lProductReserveSuppressed, lProductPaymentSuppressed, lProductRecoverySuppressed As Integer

        Dim bReserveTransactionsSuppressed, bPaymentTransactionsSuppressed, bRecoveryTransactionsSuppressed As Boolean

        Dim sSuppressionMessage As String = ""

        Dim vTransactionSuppression As Object

        Try



        result = gPMConstants.PMEReturnCode.PMTrue


        Select Case g_nPMMode
            Case g_nREADMODE, g_nVIEWMODE
                ' do nothing in readonly mode
            Case Else

                ' get the claim transaction suppression indicators

                lReturn = g_oBusiness.GetClaimTransactionSuppressionInd(v_lInsuranceFileCnt:=g_lPolicyID, v_lClaimId:=g_lClaimID, r_vResults:=vTransactionSuppression)

                ' if call to get claim transactions succeeded
                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    ' if data was returned
                    If Information.IsArray(vTransactionSuppression) Then

                        ' get claim transaction suppression details

                            lProductReserveSuppressed = gPMFunctions.ToSafeLong(CStr(vTransactionSuppression(kClaimTransSuppProdReserve, 0)), 0)

                            lProductPaymentSuppressed = gPMFunctions.ToSafeLong(CStr(vTransactionSuppression(kClaimTransSuppProdPayment, 0)), 0)

                            lProductRecoverySuppressed = gPMFunctions.ToSafeLong(CStr(vTransactionSuppression(kClaimTransSuppProdRecovery, 0)), 0)


                            lClaimReserveSuppressed = gPMFunctions.ToSafeLong(CStr(vTransactionSuppression(kClaimTransSuppClaimReserve, 0)), 0)

                            lClaimPaymentSuppressed = gPMFunctions.ToSafeLong(CStr(vTransactionSuppression(kClaimTransSuppClaimPayment, 0)), 0)

                            lClaimRecoverySuppressed = gPMFunctions.ToSafeLong(CStr(vTransactionSuppression(kClaimTransSuppClaimRecovery, 0)), 0)

                    End If
                End If

                ' if this is "OPEN CLAIM"
                If m_sTransactionType = "C_CO" Then

                    ' use suppression indicators from the product
                    bReserveTransactionsSuppressed = gPMFunctions.ToSafeBoolean(CStr(lProductReserveSuppressed), False)
                    bPaymentTransactionsSuppressed = gPMFunctions.ToSafeBoolean(CStr(lProductPaymentSuppressed), False)
                    bRecoveryTransactionsSuppressed = gPMFunctions.ToSafeBoolean(CStr(lProductRecoverySuppressed), False)

                Else
                    ' for all other transaction types

                    ' use reserve suppression indicator only from the claim
                    ' once this has been set it cannot be changed
                    bReserveTransactionsSuppressed = gPMFunctions.ToSafeBoolean(CStr(lClaimReserveSuppressed), False)

                    ' use payment suppression indicator from the claim initially
                    bPaymentTransactionsSuppressed = gPMFunctions.ToSafeBoolean(CStr(lClaimPaymentSuppressed), False)
                    ' if it is not suppressed on the claim then
                    If Not bPaymentTransactionsSuppressed Then
                        ' use the value from the product
                        bPaymentTransactionsSuppressed = gPMFunctions.ToSafeBoolean(CStr(lProductPaymentSuppressed), False)
                    End If

                    ' use recovery suppression indicator from the claim initially
                    bRecoveryTransactionsSuppressed = gPMFunctions.ToSafeBoolean(CStr(lClaimRecoverySuppressed), False)
                    ' if it is not suppressed on the claims then
                    If Not bRecoveryTransactionsSuppressed Then
                        ' use the value from the product
                        bRecoveryTransactionsSuppressed = gPMFunctions.ToSafeBoolean(CStr(lProductRecoverySuppressed), False)
                    End If

                End If

                If bReserveTransactionsSuppressed And bPaymentTransactionsSuppressed And bRecoveryTransactionsSuppressed Then
                    sSuppressionMessage = "No reserves, payments or recoveries are recorded in accounts for this claim"

                ElseIf bPaymentTransactionsSuppressed And bRecoveryTransactionsSuppressed Then
                    sSuppressionMessage = "No payments or recoveries are recorded in accounts for this claim"

                ElseIf bPaymentTransactionsSuppressed Then
                    sSuppressionMessage = "No payments are recorded in accounts for this claim"

                ElseIf bRecoveryTransactionsSuppressed Then
                    sSuppressionMessage = "No recoveries are recorded in accounts for this claim"
                End If

                If sSuppressionMessage <> "" Then
                    MessageBox.Show(sSuppressionMessage, "Claim Transaction Suppression", MessageBoxButtons.OK)
                End If

        End Select



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Public Function GetPolicyInfo() As Integer
        Dim result As Integer = 0
        Dim InsResult, CResult, IResult, RResult(,) As Object
        Dim sClaimTitle As String = ""
        Dim vInsurerContact, vClientContact As Object
        Dim oFindPolicy As iSIRFindInsurance.Interface_Renamed

        Const ACClaim As String = "Claim Details"

        Const ACConTelephoneHomeArea As Integer = 0
        Const ACConTelephoneHomeNumber As Integer = 1
        Const ACConTelephoneHomeExt As Integer = 2

        Const ACConTelephoneOfficeArea As Integer = 3
        'Const ACConTelephoneOfficeNumber As Integer = 4
        'Const ACConTelephoneOfficeExt As Integer = 5

        Const ACConFaxArea As Integer = 6
        Const ACConFaxNumber As Integer = 7
        Const ACConFaxExt As Integer = 8

        Const ACConMobileArea As Integer = 9
        Const ACConMobileNumber As Integer = 10
        Const ACConMobileExt As Integer = 11

        Const ACConEmail As Integer = 12


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Insurance object
            Dim temp_oFindPolicy As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindPolicy, sClassName:="iSIRFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindPolicy = temp_oFindPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iSIRFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface

            oFindPolicy.CallingAppName = ACApp



            m_lReturn = oFindPolicy.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            If oFindPolicy.PolicyNumber.Trim().Length = 0 Then Return result

            ' Retrieve InsuranceRef and set as PolicyRef

            m_sPolicyRef = oFindPolicy.PolicyNumber

            m_lPolicyId = oFindPolicy.InsuranceFilecnt


            If g_sPolicyNo.Trim() <> m_sPolicyRef.Trim() Then

                If MessageBox.Show("Warning! The claim is being moved to new policy. Are you sure you want to continue ?", "Claim Policy Change", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.No Then

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Else

                    ' Set the mouse pointer to busy.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                    'PN47398
                    If m_iTask = gPMConstants.PMEComponentAction.PMEdit And cboRiskType.Items.Count > 0 And m_sPolicyRef.Trim() <> "" Then
                        cboRiskType.Enabled = True
                    End If



                    m_lReturn = g_oBusiness.GetValidPrimaryCauses(m_lPolicyId, m_vPrimaryCauseArray)
                    ' Check for errors.
                    If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Return result
                    End If
                    cboSecondaryCausationCode.Items.Clear()
                    cboPrimaryCausationCode.Items.Clear()
                    m_lReturn = PopulatePrimaryCause()
                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Return result
                    End If

                    'Getting Risk Details from the Business Object

                    m_lReturn = g_oBusiness.GetRiskDetails(m_lPolicyId, g_vClaimDate, RResult, g_lClaimID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'm_lErrorNumber& = PMFalse

                        Return result
                    End If

                    If Information.IsArray(RResult) Then
                        cboRiskType.Items.Clear()
                        'Load Details of Risk Type in the combo
                        'RWH(06/03/2001) This doesn't retrieve risk_type_id at all.


                        LoadDataInCombo(cboRiskType, RResult, RResult.GetLowerBound(1), RResult.GetUpperBound(1) + 1)

                    End If


                    g_lPolicyID = m_lPolicyId
                    g_sPolicyNo = m_sPolicyRef

                    ' Gets the interface details to be displayed.
                    m_lReturn = m_oGeneral.GetInterfaceDetails()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to get the interface details.
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                        ' Set the mouse pointer to normal.
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                        Return result
                    End If


                    m_lReturn = g_oBusiness.GetInsurerDetails(g_lPolicyID, g_vClaimDate, IResult, g_nPMMode)

                    bHasAgent = True
                    SSTabHelper.SetTabEnabled(tabMainTab, 3, True)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        bHasAgent = False
                        SSTabHelper.SetTabEnabled(tabMainTab, 3, False)
                    End If


                    m_lReturn = g_oBusiness.GetClientDetails(m_lPolicyId, g_vClaimDate, CResult)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Return result
                    End If

                    'Call function to populate details in the business object.

                    m_lReturn = g_oBusiness.GetPolicyDetails(InsResult, m_lPolicyId, g_vClaimDate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Return result
                    End If


                    g_dtPolicyFromDate = StringsHelper.Format(InsResult(0, 0), ACDateConversion)

                    g_dtPolicyToDate = StringsHelper.Format(InsResult(1, 0), ACDateConversion)

                    g_nCurrencyId = CInt(InsResult(2, 0))

                    'Set Default Currency id equal to policy currency id
                    If cboCurrency.Items.Count > 0 Then
                        For nCount As Integer = 0 To cboCurrency.Items.Count - 1
                            If VB6.GetItemData(cboCurrency, nCount) = g_nCurrencyId Then
                                cboCurrency.SelectedIndex = nCount
                                Exit For
                            End If
                        Next nCount
                    End If

                    'Displaying the Client Details
                    g_lClientAdressCnt = 0

                    g_lClient_AddressId = gPMFunctions.ToSafeLong(CStr(CResult(ACCAddressId)))
                    g_lClient_AddressUsage = 4


                    g_sClientName = CStr(CResult(ACCClientName))


                    txtOpenClaim(g_nCLIENT_NAME).Text = CStr(CResult(ACCClientName))

                    g_sClientShortName = CStr(CResult(ACCClientShortName))


                    g_sClientAddress1 = Convert.ToString(CResult(ACCAddress1)).Trim()

                    g_sClientAddress2 = Convert.ToString(CResult(ACCAddress2)).Trim()

                    g_sClientAddress3 = Convert.ToString(CResult(ACCAddress3)).Trim()

                    g_sClientAddress4 = Convert.ToString(CResult(ACCAddress4)).Trim()

                    g_sClientPostCode = Convert.ToString(CResult(ACCPostCode)).Trim()



                    g_lClientCountryId = CInt(CResult(ACCCountryId))

                    m_lReturn = g_oBusiness.GetCountryName(g_sClientCountryName, g_lClientCountryId)


                    'Display postcode for UK country only
                    If g_lClientCountryId <> ACCountryGBR Then
                        txtOpenClaim(g_nCLIENT_ADDRESS).Text = g_sClientAddress1 & " " &
                                                               g_sClientAddress2 & " " & g_sClientAddress3 & " " &
                                                               g_sClientAddress4 & " " & g_sClientCountryName
                    End If


                    If Strings.Len(Convert.ToString(CResult(ACCTeleHome))) > 0 Then


                        txtOpenClaim(g_nCLIENT_TELNO).Text = CStr(CResult(ACCTeleHome))


                        txtOpenClaim(g_nCLIENT_TELNOOFF).Text = CStr(CResult(ACCTeleOff))


                        txtOpenClaim(g_nCLIENT_FAXNO).Text = CStr(CResult(ACCFax))


                        txtOpenClaim(g_nCLIENT_MOBILENO).Text = CStr(CResult(ACCMobile))


                        txtOpenClaim(g_nCLIENT_EMAIL).Text = CStr(CResult(ACCEmail))
                    Else


                        m_lReturn = g_oBusiness.GetDefaultContacts(v_lPolicyID:=m_lPolicyId, r_vResults:=vClientContact, v_bIsClient:=True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                            Return result
                        End If

                        If Information.IsArray(vClientContact) Then
                            ' Default the client's details through to the screen

                            ' Home Phone Number



                            m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_TELNO), sAreaCode:=CStr(vClientContact(ACConTelephoneHomeArea)), sNumber:=CStr(vClientContact(ACConTelephoneHomeNumber)), sExtension:=CStr(vClientContact(ACConTelephoneHomeExt)))

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                ' Office Phone Number


                                m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_TELNOOFF), sAreaCode:=CStr(vClientContact(ACConTelephoneOfficeArea)), sNumber:=CStr(vClientContact(ACConTelephoneOfficeArea)), sExtension:=CStr(vClientContact(ACConTelephoneOfficeArea)))
                            End If

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                ' Fax Number



                                m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_FAXNO), sAreaCode:=CStr(vClientContact(ACConFaxArea)), sNumber:=CStr(vClientContact(ACConFaxNumber)), sExtension:=CStr(vClientContact(ACConFaxExt)))

                            End If

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                ' Mobile Number



                                m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nCLIENT_MOBILENO), sAreaCode:=CStr(vClientContact(ACConMobileArea)), sNumber:=CStr(vClientContact(ACConMobileNumber)), sExtension:=CStr(vClientContact(ACConMobileExt)))

                            End If

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to format a client phone / fax number", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                Return result
                            End If


                            txtOpenClaim(g_nCLIENT_EMAIL).Text = Convert.ToString(vClientContact(ACConEmail)).Trim()

                        End If

                    End If


                    m_lPartyCnt = CInt(CResult(ACCPartyCnt))
                    g_lPartyCnt = m_lPartyCnt

                    'Displaying the Insurer Details
                    'Check whether an agent has been found for the policy
                    If bHasAgent Then

                        g_lInsurerAdressCnt = 0

                        g_lInsurer_AddressId = gPMFunctions.ToSafeLong(IResult(ACIAddressId))
                        g_lInsurer_AddressUsage = 4


                        txtOpenClaim(g_nINSURER_NAME).Text = Convert.ToString(IResult(ACIInsurerName)).Trim()

                        g_sInsurerShortName = Convert.ToString(IResult(ACIInsurerShortName)).Trim()

                        g_sInsurerAddress1 = Convert.ToString(IResult(ACIAddress1)).Trim()

                        g_sInsurerAddress2 = Convert.ToString(IResult(ACIAddress2)).Trim()

                        g_sInsurerAddress3 = Convert.ToString(IResult(ACIAddress3)).Trim()

                        g_sInsurerAddress4 = Convert.ToString(IResult(ACIAddress4)).Trim()

                        g_sInsurerPostCode = Convert.ToString(IResult(ACIPostCode)).Trim()



                        g_lInsurerCountryId = CInt(CResult(ACICountryId))


                        If g_lInsurerCountryId <> ACCountryGBR Then
                            txtOpenClaim(g_nINSURER_ADDRESS).Text = g_sInsurerAddress1 & " " &
                                                                    g_sInsurerAddress2 & " " & g_sInsurerAddress3 & " " &
                                                                    g_sInsurerAddress4
                        End If


                        If Strings.Len(Convert.ToString(IResult(ACITeleHome))) > 0 Then

                            txtOpenClaim(g_nINSURER_TELNO).Text = Convert.ToString(IResult(ACITeleHome)).Trim()

                            txtOpenClaim(g_nINSURER_FAXNO).Text = Convert.ToString(IResult(ACIFax)).Trim()
                            'txtOpenClaim(g_nINSURER_CONTACT) = IResult(5)


                            txtOpenClaim(g_nINSURER_EMAIL).Text = CStr(IResult(ACIEmail))
                        Else

                            m_lReturn = g_oBusiness.GetDefaultContacts(v_lPolicyID:=m_lPolicyId, r_vResults:=vInsurerContact, v_bIsClient:=False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                                Return result
                            End If

                            If Information.IsArray(vInsurerContact) Then
                                ' Default the client's details through to the screen
                                ' Home Phone Number



                                m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nINSURER_TELNO), sAreaCode:=CStr(vInsurerContact(ACConTelephoneHomeArea)), sNumber:=CStr(vInsurerContact(ACConTelephoneHomeNumber)), sExtension:=CStr(vInsurerContact(ACConTelephoneHomeExt)))

                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    ' Fax Number



                                    m_lReturn = DisplayPhoneNumber(ctlTextField:=txtOpenClaim(g_nINSURER_FAXNO), sAreaCode:=CStr(vInsurerContact(ACConFaxArea)), sNumber:=CStr(vInsurerContact(ACConFaxNumber)), sExtension:=CStr(vInsurerContact(ACConFaxExt)))

                                End If

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to format an insurer phone / fax number", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                    Return result
                                End If


                                txtOpenClaim(g_nINSURER_EMAIL).Text = Convert.ToString(vInsurerContact(ACConEmail)).Trim()
                            End If
                        End If
                    Else
                        g_lInsurerAdressCnt = 0
                        g_lInsurer_AddressId = 0
                        g_lInsurer_AddressUsage = 4

                        txtOpenClaim(g_nINSURER_NAME).Text = ""
                        g_sInsurerShortName = ""
                        g_sInsurerAddress1 = ""
                        g_sInsurerAddress2 = ""
                        g_sInsurerAddress3 = ""
                        g_sInsurerAddress4 = ""
                        g_sInsurerPostCode = ""
                        txtOpenClaim(g_nINSURER_ADDRESS).Text = ""
                        txtOpenClaim(g_nINSURER_TELNO).Text = ""
                        txtOpenClaim(g_nINSURER_FAXNO).Text = ""
                        txtOpenClaim(g_nINSURER_EMAIL).Text = ""
                    End If


                    If g_lClientAdressCnt = 0 Then


                        m_lReturn = g_oBusiness.AddAddress(g_sClientAddress1, g_sClientAddress2, g_sClientAddress3, g_sClientAddress4, g_sClientPostCode, g_lClient_AddressUsage, g_lClient_AddressId, g_lClientAdressCnt, g_bClientAddressChanged, g_lClientCountryId)


                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return result
                        End If
                    End If

                    If g_lInsurerAdressCnt = 0 Then

                        If bHasAgent Then

                            m_lReturn = g_oBusiness.AddAddress(g_sInsurerAddress1, g_sInsurerAddress2, g_sInsurerAddress3, g_sInsurerAddress4, g_sInsurerPostCode, g_lInsurer_AddressUsage, g_lInsurer_AddressId, g_lInsurerAdressCnt, g_bInsurerAddressChanged, g_lInsurerCountryId)
                        End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return result
                        End If
                    End If

                    g_sClientAddress = g_lClientAdressCnt
                    g_sInsurerAddress = g_lInsurerAdressCnt
                    m_bDataChanged = True

                    m_lReturn = InterfaceToBusiness()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If


                    If g_nInfoOnly = 1 Then
                        sClaimTitle = "Prov. Claim No. "
                    Else
                        sClaimTitle = "Claim No. "
                    End If
                    'RWH(10/11/2000) Updated to display Policy AND claim no.
                    Me.Text = ACClaim & " [ " & g_sClientName & " ] " & m_sPolicyRef & New String(" "c, 2) & sClaimTitle & " " & g_sClaimNo


                    'Call Method to write data from Local Variables to Form Controls
                    m_lReturn = DisplayPolicyDetails()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            ' Destroy Find Insurance object

            oFindPolicy.Dispose()
            oFindPolicy = Nothing

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetProductDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Private Function GetProductDetails(ByVal v_lProductID As Integer, ByRef r_lClaimsUserDefTableA As Integer, ByRef r_lClaimsUserDefTableB As Integer, ByRef r_lClaimsUserDefTableC As Integer, ByRef r_lClaimsUserDefTableD As Integer, ByRef r_lClaimsUserDefTableE As Integer, ByRef r_bDuplicateCheckEnabled As Boolean, ByRef r_bDisplayValidVersionEnabled As Boolean) As Integer
        Dim result As Integer = 0
        Dim bSIRProduct As Object

        Const kMethodName As String = "GetProductDetails"

        Const kIClaimsTypeBasis As Integer = 60
        Const kIClaimsCoverBasis As Integer = 61
        Const kIClaimsUDTA As Integer = 134
        Const kIClaimsUDTB As Integer = 135
        Const kIClaimsUDTC As Integer = 136
        Const kIClaimsUDTD As Integer = 137
        Const kIClaimsUDTE As Integer = 138
        Const kIIsDuplicateClaimCheckEnabled As Integer = 139

        Dim lReturn As Integer

        Dim oBusiness As bSIRProduct.Business
        Dim vResultArray(,) As Object

        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of the business object via
        ' the public object manager.
        Dim temp_oBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oBusiness = temp_oBusiness

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of bSIRProduct.Business", gPMConstants.PMELogLevel.PMLogError)
        End If

        'get product details

        m_lReturn = oBusiness.GetProductDetails(v_lProductID, vResultArray)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Information.IsArray(vResultArray) Then

                r_lClaimsUserDefTableA = gPMFunctions.ToSafeLong(CStr(vResultArray(kIClaimsUDTA, 0)))

                r_lClaimsUserDefTableB = gPMFunctions.ToSafeLong(CStr(vResultArray(kIClaimsUDTB, 0)))

                r_lClaimsUserDefTableC = gPMFunctions.ToSafeLong(CStr(vResultArray(kIClaimsUDTC, 0)))

                r_lClaimsUserDefTableD = gPMFunctions.ToSafeLong(CStr(vResultArray(kIClaimsUDTD, 0)))

                r_lClaimsUserDefTableE = gPMFunctions.ToSafeLong(CStr(vResultArray(kIClaimsUDTE, 0)))

                r_bDuplicateCheckEnabled = gPMFunctions.ToSafeLong(CStr(vResultArray(kIIsDuplicateClaimCheckEnabled, 0)))

                r_bDisplayValidVersionEnabled = IIf(gPMFunctions.ToSafeLong(CStr(vResultArray(132, 0))) = 1, True, False)
        End If


            oBusiness.Dispose()



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: ValidateLossDate
    ' Description:
    ' History:
    ' Created : VB : 02-08-2007
    ' ***************************************************************** '
    Private Function ValidateLossDate() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateLossDate"

        Dim lReturn As DialogResult
        Dim dtRetroactiveDate As Date
        Dim vResultArray(,) As Object
        Dim sMessage As String = ""
        Dim lRiskCnt As Integer

        Dim dtInceptionDate As Date

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        If g_nPMMode <> g_nREADMODE Or g_nPMMode <> g_nVIEWMODE Then
            If m_sClaimsCoverBasisCode <> "" Then

                lRiskCnt = VB6.GetItemData(cboRiskType, cboRiskType.SelectedIndex)

                'Get the Retoactive Date and the Inception Date Specific to Risk - Amit

                m_lReturn = g_oBusiness.GetGISRetroactiveDate(v_lInsurancefileID:=g_lPolicyID, v_lRiskCnt:=lRiskCnt, r_vResults:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If



            If m_sClaimsCoverBasisCode = "CM" Then

                If Information.IsArray(vResultArray) Then

                        If gPMFunctions.ToSafeString(vResultArray(0, 0)) <> "" Then

                            dtRetroactiveDate = gPMFunctions.ToSafeDate(vResultArray(0, 0))

                        If (gPMFunctions.ToSafeDate(txtOpenClaim(g_nLOSS_DATE).Text) < dtRetroactiveDate Or gPMFunctions.ToSafeDate(txtOpenClaim(g_nLOSS_DATE).Text) > gPMFunctions.ToSafeDate(CStr(m_vInsuranceFileDetails(ACInsFileExpiryDate, 0)))) And (gPMFunctions.ToSafeDate(txtOpenClaim(g_nLOSS_DATE).Text) < gPMFunctions.ToSafeDate(CStr(m_vInsuranceFileDetails(ACInsFileExpiryDate, 0))) Or gPMFunctions.ToSafeDate(txtOpenClaim(g_nLOSS_DATE).Text) > dtRetroactiveDate) Then

                            DisplayMessage(AC_Res_MsgLossDate1, "Loss Date")
                            txtOpenClaim(g_nLOSS_DATE).Focus()
                            result = gPMConstants.PMEReturnCode.PMFalse
                                Return result
                        End If

                        If (gPMFunctions.ToSafeDate(txtOpenClaim(g_nREPORTED_DATE).Text) < gPMFunctions.ToSafeDate(CStr(m_vInsuranceFileDetails(ACInsFileCoverStartDate, 0))) Or gPMFunctions.ToSafeDate(txtOpenClaim(g_nREPORTED_DATE).Text) > gPMFunctions.ToSafeDate(CStr(m_vInsuranceFileDetails(ACInsFileExpiryDate, 0)))) And (gPMFunctions.ToSafeDate(txtOpenClaim(g_nREPORTED_DATE).Text) > gPMFunctions.ToSafeDate(CStr(m_vInsuranceFileDetails(ACInsFileCoverStartDate, 0))) Or gPMFunctions.ToSafeDate(txtOpenClaim(g_nREPORTED_DATE).Text) < gPMFunctions.ToSafeDate(CStr(m_vInsuranceFileDetails(ACInsFileExpiryDate, 0)))) Then

                            DisplayMessage(AC_Res_MsgReportedDate, "Reported Date")
                            txtOpenClaim(g_nREPORTED_DATE).Focus()
                            result = gPMConstants.PMEReturnCode.PMFalse
                                Return result
                        End If
                    Else
                        If Not m_bPolicyFound Then

                            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoPolicyFound, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lReturn = MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue?", "Loss Date Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                            If lReturn = System.Windows.Forms.DialogResult.No Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                    Return result
                            Else
                                m_bLossDateTime = False
                            End If
                        Else
                            DisplayMessage(AC_Res_MsgRetroactiveDate, "Retroactive Date")
                        End If
                    End If
                Else

                    DisplayMessage(AC_Res_MsgRetroactiveDate, "Retroactive Date")

                End If

            ElseIf m_sClaimsCoverBasisCode = "TC" Then
                If Information.IsArray(vResultArray) Then

                        If gPMFunctions.ToSafeString(vResultArray(0, 0)) <> "" Then


                            dtRetroactiveDate = gPMFunctions.ToSafeDate(vResultArray(0, 0))

                            dtInceptionDate = gPMFunctions.ToSafeDate(vResultArray(1, 0))

                        'Should be Selected Risk Inception Date - Amit
                        If (gPMFunctions.ToSafeDate(txtOpenClaim(g_nLOSS_DATE).Text) < dtInceptionDate Or gPMFunctions.ToSafeDate(txtOpenClaim(g_nLOSS_DATE).Text) > dtRetroactiveDate) And (gPMFunctions.ToSafeDate(txtOpenClaim(g_nLOSS_DATE).Text) < dtRetroactiveDate Or gPMFunctions.ToSafeDate(txtOpenClaim(g_nLOSS_DATE).Text) > dtInceptionDate) Then

                            DisplayMessage(AC_Res_MsgLossDate2, "Loss Date")
                            txtOpenClaim(g_nLOSS_DATE).Focus()
                            result = gPMConstants.PMEReturnCode.PMFalse
                                Return result

                        End If

                        If (gPMFunctions.ToSafeDate(txtOpenClaim(g_nREPORTED_DATE).Text) < gPMFunctions.ToSafeDate(CStr(m_vInsuranceFileDetails(ACInsFileCoverStartDate, 0))) Or gPMFunctions.ToSafeDate(txtOpenClaim(g_nREPORTED_DATE).Text) > gPMFunctions.ToSafeDate(CStr(m_vInsuranceFileDetails(ACInsFileExpiryDate, 0)))) And (gPMFunctions.ToSafeDate(txtOpenClaim(g_nREPORTED_DATE).Text) < gPMFunctions.ToSafeDate(CStr(m_vInsuranceFileDetails(ACInsFileExpiryDate, 0))) Or gPMFunctions.ToSafeDate(txtOpenClaim(g_nREPORTED_DATE).Text) > gPMFunctions.ToSafeDate(CStr(m_vInsuranceFileDetails(ACInsFileCoverStartDate, 0)))) Then

                            DisplayMessage(AC_Res_MsgReportedDate, "Reported Date")
                            txtOpenClaim(g_nREPORTED_DATE).Focus()
                            result = gPMConstants.PMEReturnCode.PMFalse
                                Return result

                        End If
                    Else
                        If Not m_bPolicyFound Then

                            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoPolicyFound, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            lReturn = MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue?", "Loss Date Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                            If lReturn = System.Windows.Forms.DialogResult.No Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                    Return result
                            Else
                                m_bLossDateTime = False
                            End If
                        Else
                            DisplayMessage(AC_Res_MsgRetroactiveDate, "Retroactive Date")
                        End If
                    End If
                Else
                    DisplayMessage(AC_Res_MsgRetroactiveDate, "Retroactive Date")

                End If
            End If
        End If




        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sUsername:=g_oBusiness.UserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function


    Private Function GetProgressStatus() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetProgressStatus(sTransaction_Type:=m_sTransactionType, r_vDataArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to execute GetProgressStatus", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProgressStatus")

                Return result
            End If

            If Information.IsArray(vArray) Then

                For iVar As Integer = 0 To vArray.GetUpperBound(1)
                    Dim cboProgressStatus_NewIndex As Integer = -1

                    If Not (g_nPMMode = g_nADDMODE And CStr(vArray(1, iVar).ToString().Trim().ToUpper()) = "CLOSED") Then
                        cboProgressStatus_NewIndex = cboProgressStatus.Items.Add(CStr(vArray(1, iVar)))

                        VB6.SetItemData(cboProgressStatus, cboProgressStatus_NewIndex, CInt(vArray(0, iVar)))
                    End If

                Next
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute GetProgressStatus", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProgressStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    'PN: 73770
    Private Function GetClaimHandler() As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oBusiness.GetClaimHandler(r_vDataArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to execute GetClaimHandler", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimHandler")

                Return result
            End If

            If Information.IsArray(vArray) Then
                For iVar As Integer = 0 To vArray.GetUpperBound(1)
                    Dim cboHandler_NewIndex As Integer = -1
                    cboHandler_NewIndex = cboHandler.Items.Add(CStr(vArray(1, iVar)))
                    VB6.SetItemData(cboHandler, cboHandler_NewIndex, CInt(vArray(0, iVar)))
                Next
            End If

            Return result

        Catch excep As System.Exception


            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute GetClaimHandler", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimHandler", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateLossDate
    ' Description:
    ' History:
    ' Created : VB : 02-08-2007
    ' ***************************************************************** '
    Private Function ValidateDates() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateDates"

        Dim lReturn As Integer
        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        If Not CheckDatesValid() Then
            result = gPMConstants.PMEReturnCode.PMFalse
                Return result
        End If

        If CheckValidLossDate() <> gPMConstants.PMEReturnCode.PMTrue Then
            txtOpenClaim(g_nLOSS_DATE).Focus()
            result = gPMConstants.PMEReturnCode.PMFalse
                Return result
        End If


        If ValidateLossDate() <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
                Return result
        End If




        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sUsername:=g_oBusiness.UserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    '***************************************************************** '
    ' Name: DisplayUserDefinedFieldsSFU
    '
    ' Description: Displays all of the User Definedlookup details using the lookup
    '              values/details for Underwriting.
    ' Edit History:
    '
    ' Date :01/04/2008
    ' ***************************************************************** '
    Public Function DisplayUserDefinedFieldsSFU() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisplayUserDefinedFieldsSFU"

        Try

        result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object
            Dim vUDTArray(,) As Object
        Dim nOptionValue As Integer

        Const ACFirstValue As Integer = 0
        Const ACColon As String = ":"

        cboUDFA.Table = m_lClaimsUserDefTableA
        cboUDFA.RefreshList()

        If m_lClaimsUserDefTableA < 1 Then
            'Do not display if no table entered
            cboUDFA.Visible = False
            lblUserDefinedFieldA.Visible = False
            cboUDFA.ListIndex = -1
        Else
            'Get the Caption

            m_lReturn = g_oBusiness.GetUserDefinedCaption(vResultArray, m_lClaimsUserDefTableA)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    cboUDFA.Visible = False
                    lblUserDefinedFieldA.Visible = False
                    cboUDFA.ListIndex = -1
                Else
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If Information.IsArray(vResultArray) Then

                    lblUserDefinedFieldA.Text = CStr(vResultArray(ACFirstValue, ACFirstValue)) & ACColon
            End If
        End If

        'Intialise for next fetch

            vResultArray = Nothing

        'Populate User Defined Field B
        cboUDFB.Table = m_lClaimsUserDefTableB
        cboUDFB.RefreshList()

        If m_lClaimsUserDefTableB < 1 Then
            'Do not display if no table entered
            cboUDFB.Visible = False
            lblUserDefinedFieldB.Visible = False
            cboUDFB.ListIndex = -1
        Else
            'Get the Caption

            m_lReturn = g_oBusiness.GetUserDefinedCaption(vResultArray, m_lClaimsUserDefTableB)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    cboUDFB.Visible = False
                    lblUserDefinedFieldB.Visible = False
                    cboUDFB.ListIndex = -1
                Else
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If Information.IsArray(vResultArray) Then

                    lblUserDefinedFieldB.Text = CStr(vResultArray(ACFirstValue, ACFirstValue)) & ACColon
            End If
        End If

        'Intialise for next fetch

            vResultArray = Nothing

        'Populate User Defined Field C
        cboUDFC.Table = m_lClaimsUserDefTableC
        cboUDFC.RefreshList()

        If m_lClaimsUserDefTableC < 1 Then
            'Do not display if no table entered
            cboUDFC.Visible = False
            lblUserDefinedFieldC.Visible = False
            cboUDFC.ListIndex = -1
        Else
            'Get the Caption

            m_lReturn = g_oBusiness.GetUserDefinedCaption(vResultArray, m_lClaimsUserDefTableC)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    cboUDFC.Visible = False
                    lblUserDefinedFieldC.Visible = False
                    cboUDFC.ListIndex = -1
                Else
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If Information.IsArray(vResultArray) Then

                    lblUserDefinedFieldC.Text = CStr(vResultArray(ACFirstValue, ACFirstValue)) & ACColon
            End If
        End If

        'Intialise for next fetch

            vResultArray = Nothing

        'Populate User Defined Field D
        cboUDFD.Table = m_lClaimsUserDefTableD
        cboUDFD.RefreshList()

        If m_lClaimsUserDefTableD < 1 Then
            'Do not display if no table entered
            cboUDFD.Visible = False
            lblUserDefinedFieldD.Visible = False
            cboUDFD.ListIndex = -1
        Else
            'Get the Caption

            m_lReturn = g_oBusiness.GetUserDefinedCaption(vResultArray, m_lClaimsUserDefTableD)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    cboUDFD.Visible = False
                    lblUserDefinedFieldD.Visible = False
                    cboUDFD.ListIndex = -1
                Else
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If Information.IsArray(vResultArray) Then

                    lblUserDefinedFieldD.Text = CStr(vResultArray(ACFirstValue, ACFirstValue)) & ACColon
            End If
        End If

        'Intialise for next fetch

            vResultArray = Nothing

        'Populate User Defined Field E
        cboUDFE.Table = m_lClaimsUserDefTableE
        cboUDFE.RefreshList()

        If m_lClaimsUserDefTableE < 1 Then
            'Do not display if no table entered
            cboUDFE.Visible = False
            lblUserDefinedFieldE.Visible = False
            cboUDFE.ListIndex = -1
        Else
            'Get the Caption

            m_lReturn = g_oBusiness.GetUserDefinedCaption(vResultArray, m_lClaimsUserDefTableE)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    cboUDFE.Visible = False
                    lblUserDefinedFieldE.Visible = False
                    cboUDFE.ListIndex = -1
                Else
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If Information.IsArray(vResultArray) Then

                    lblUserDefinedFieldE.Text = CStr(vResultArray(ACFirstValue, ACFirstValue)) & ACColon
            End If
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sUsername:=g_oBusiness.UserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Private Sub cmdTPA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTPA.Click
        Dim vCnt, vName, vShortName As Object
        Dim vResolvedName As String = ""

        Try

            m_lReturn = SelectParty(vPartyCnt:=vCnt, vName:=vName, vShortName:=vShortName, vResolvedName:=vResolvedName, vSpecialParty:="OTTPA")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            txtTPA.Tag = CStr(vCnt)

            If vResolvedName = "" Then

                txtTPA.Text = CStr(vName)
            Else
                txtTPA.Text = vResolvedName
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAgentCode_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    Private Function IsValidString(ByRef str As String) As Boolean
        Dim illegalChars As Char() = ":~""#%&*:<>?/\{}|".ToCharArray()

        For Each ch As Char In str
            If Array.IndexOf(illegalChars, ch) > -1 Then
                Return False
            End If
        Next
        Return True
    End Function
    Private Sub txtTPA_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTPA.KeyDown
        If (e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back) And cmdTPA.Enabled = True Then
            txtTPA.Text = ""
            txtTPA.Tag = ""
            m_iUserOtherPartyID = 0
        End If
    End Sub
    

    Private Sub cboRiskType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRiskType.SelectedIndexChanged
        Dim oResultArray(,) As Object = Nothing
        Dim lRiskTypeID As Long
        Const kClaimTypeBasis As Integer = 0
        Const kClaimCoverBasis As Integer = 1
        Const kAttachClaimOutsideOfPolicyPeriod As Integer = 2


        If g_nPMMode <> g_nREADMODE Then

            If cboRiskType.SelectedIndex <> -1 Then
                lRiskTypeID = VB6.GetItemData(cboRiskType, cboRiskType.SelectedIndex)

                m_lReturn = g_oBusiness.GetClaimTypeAndCover(v_lRiskTypeID:=lRiskTypeID, r_vResults:=oResultArray)

                If Information.IsArray(oResultArray) Then

                    m_sClaimsTypeBasisCode = gPMFunctions.ToSafeString(oResultArray(kClaimTypeBasis, 0))

                    m_sClaimsCoverBasisCode = gPMFunctions.ToSafeString(oResultArray(kClaimCoverBasis, 0))
                    m_bAttachClaimOutsideOfPolicyPeriod = gPMFunctions.ToSafeBoolean(oResultArray(kAttachClaimOutsideOfPolicyPeriod, 0))

                    If g_nPMMode = g_nADDMODE And m_sClaimsTypeBasisCode = "OCCUR" And m_bLossDateChanged = False And txtOpenClaim(g_nLOSS_DATE).Text = "" Then
                        FormatDate(g_vClaimDate, g_nLOSS_DATE)
                        FormatDate(txtOpenClaim(g_nLOSS_DATE).Text, g_nLOSS_TO_DATE)
                        m_bLossDateChanged = True

                    ElseIf g_nPMMode = g_nADDMODE And m_sClaimsTypeBasisCode = "CM" And m_bReportedDateChanged = False And gPMFunctions.ToSafeDate(txtOpenClaim(g_nREPORTED_DATE).Text) = Now.Date Then
                        txtOpenClaim(g_nREPORTED_DATE).Text = StringsHelper.Format(g_vClaimDate, ACDateConversion)
                        FormatDate(txtOpenClaim(g_nREPORTED_DATE).Text, g_nREPORTED_DATE)
                        m_bReportedDateChanged = True
                    End If
                End If
            End If
        End If
    End Sub
End Class
