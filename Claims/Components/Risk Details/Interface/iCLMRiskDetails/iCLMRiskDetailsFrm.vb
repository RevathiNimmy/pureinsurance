Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No: 129
Imports SharedFiles


Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name   : frmInterface
	'
	' Date        : August 14 2000
	'
	' Description : Main interface.
	'
	' Edit History: 18th Oct DG : Bugs fixed according to "036-Bug Report.xls" 21, 22,23,25
	'               21st Oct DG : Added a functionality. On pressing OK, in Risk Details screen,
	'                             when the Sum(CurrentReserve) = 0 then a message will come up
	'                             asking the User wether the Claim can be closed.
	'                             If the reply is YES the Claim status is set to closed.
	'               24th Oct DG : Bug ID -28 Internal Bug
	'                             Display a colon after very label.
	'               01st Nov DG : Bug ID -04 PM
	'                             Display a colon after very label.
	'               RAM20021018 : NRMA Changes (Sirius Process No 126)
	'                             Added 2 tool bar buttons to view Party Summary Details &
	'                             Policy Summary Details
	'               VB20050703  : PN18898 Removed max length(255) from TxtComment
	' ***************************************************************** '
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	Private collRiskDataDefnForDates As Collection
	
	' Object parameter members.
	Private m_vLookupDataArray( ,  ) As Object
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As Integer
	Private m_sSiriusProduct As String = ""
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_lRiskType As Integer
	Private m_lclaimid As Integer
	Private m_sClaimRiskDesciption As String = ""
	Private m_lRisk As Integer
	Private m_lPolicyId As Integer
	Private m_bViewRiskFlag As Boolean
	Private m_sRiskDescription As String = ""
	Private m_sClaimNumber As String = ""
	Private m_iLanguageID As Integer
	Private m_lOriginalClaimId As Integer
	Private m_lClaimMode As Integer
	Private m_sGeneralTabName As String = ""
	Private m_cThisPayment As Decimal
	Private m_lThisPaymentCurrencyID As Integer
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iCLMRiskDetails.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	Private g_bFirst As Boolean
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	Private g_vFieldData( ,  ) As Object
	Private m_vDrivers As Object
	Private m_sUnderwritingOrAgency As String = ""
	Private m_bInfoOnlyStatus As Boolean
	Private m_lCloseClaim As gPMConstants.PMEReturnCode 'set to pmtrue to close down claim
	Private m_bLossSchedule As Boolean
	
	Const ACDataTypeGEMLookup As Integer = 99
	
	Const ACDataTypeText As Integer = 1
	Const ACDataTypeInteger As Integer = 2
	Const ACDataTypeDate As Integer = 3
	Const ACDataTypeBoolean As Integer = 4
	Const ACDataTypeLookup As Integer = 5
	Const ACDataTypeParty As Integer = 6
	
	'col
	Const ACCOL_risk_data_defn_id As Integer = 0
	Const ACCOL_Risk_type_id As Integer = 1
	Const ACCOL_Description As Integer = 2
	Const ACCOL_Claim_party_type_id As Integer = 3
	Const ACCOL_Caption As Integer = 4
	Const ACCOL_type As Integer = 5
	Const ACCOL_code As Integer = 6
	Const ACCOL_display_order As Integer = 7
	Const ACCOL_Mandatory As Integer = 8
	Const ACCOL_read_only As Integer = 9
	Const ACCOL_Claim_Lookup_id As Integer = 10
	Const ACCOL_claim_user_defined_risk_data_id As Integer = 11
	Const ACCOL_claim_id As Integer = 12
	Const ACCOL_Value As Integer = 14
	Const ACCOL_Claim_Tab_ID As Integer = 15
	Const ACCOL_Tab_Caption As Integer = 16
	Const ACCOL_Tab_Display_Order As Integer = 17
	
	' Lookup value contants.
	Const ACValueTableName As Integer = 0
	Const ACValueID As Integer = 1
	Const ACValueStartPos As Integer = 2
	Const ACValueNumber As Integer = 3
	
	' Lookup detail contants.
	Const ACDetailKey As Integer = 0
	Const ACDetailDesc As Integer = 1
	
	' Constant for length of Value as per the size of VALUE in Claim_User_Defined_Risk_Data
	Const ACLengthOfValue As Integer = 255 'TN 20010828 changed from 20 to 255
	Const ACLengthOfComment As Integer = 255
	
	Private m_bAlreadyEdited As Boolean
	'GSD 150702 Claims builder decleration
	Private m_bClaimsBuilder As Boolean
	
	'sj 03/10/2002 - start
	Private m_lPartyCnt As Integer
	Private m_sPartyShortName As String = "" ' RAM20021021 : Added this variable
	Private m_lInsuranceFolderCnt As Integer
	Private m_sInsuranceRef As String = ""
	Private m_bClientPolicyDetailsLoaded As Boolean
	'sj 03/10/2002 - end
	
	' RAM20021021 - NRMA Changes - Sirius Process No 126 - Start
	Private m_oPartySummary As iSIRPartySummary.Interface_Renamed
	Private m_oPolicySummary As iSIRPolicySummary.Interface_Renamed
	' RAM20021021 - NRMA Changes - Sirius Process No 126 - End
	
	'KR 13/02/2003 - set to pmtrue to delete work table when cancelled
	Private m_lDeleteWorkTableFlag As gPMConstants.PMEReturnCode
	Private m_oRisk As iPMURiskWrapper.Interface_Renamed
	
	Private m_sClient As String = ""
	Private m_sClaimNo As String = ""
	Private m_cReserveold As Decimal
	Private m_bAddtask As Boolean
	Private m_bOpenClaimNoTrans As Boolean
	'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
	Private m_sScreenCaption As String = ""
    'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)

    Private m_bReserveLimitExceeded As Boolean
    Private m_dExceededReserve As Decimal

    ' PUBLIC Property Procedures (Begin)

    'TN20010618 start
    Public ReadOnly Property UnderwritingOrAgency() As String
		Get
			If m_sUnderwritingOrAgency = "" Then

				m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency
			End If
			Return m_sUnderwritingOrAgency
		End Get
	End Property
	Public ReadOnly Property InfoOnlyStatus(ByVal lClaimId As Integer) As Boolean
		Get

			m_lReturn = m_oBusiness.GetInfoOnlyStatus(v_lClaim_Id:=lClaimId, r_bInfoStatus:=m_bInfoOnlyStatus)
			Return m_bInfoOnlyStatus
		End Get
	End Property
	'TN20010618 end
	
	Public Property ClaimMode() As Integer
		Get
			Return m_lClaimMode
		End Get
		Set(ByVal Value As Integer)
			m_lClaimMode = Value
		End Set
	End Property
	
	Public Property ClaimNumber() As String
		Get
			Return m_sClaimNumber
		End Get
		Set(ByVal Value As String)
			m_sClaimNumber = Value
		End Set
	End Property
	Public Property ViewRiskFlag() As Boolean
		Get
			Return m_bViewRiskFlag
		End Get
		Set(ByVal Value As Boolean)
			m_bViewRiskFlag = Value
		End Set
	End Property
	Public Property Policy() As Integer
		Get
			Return m_lPolicyId
		End Get
		Set(ByVal Value As Integer)
			m_lPolicyId = Value
		End Set
	End Property
	Public Property Risk() As Integer
		Get
			Return m_lRisk
		End Get
		Set(ByVal Value As Integer)
			m_lRisk = Value
		End Set
	End Property
	Public Property SiriusProduct() As String
		Get
			Return m_sSiriusProduct
		End Get
		Set(ByVal Value As String)
			m_sSiriusProduct = Value
		End Set
	End Property
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
		End Get
	End Property
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			' Set the calling application name.
			m_sCallingAppName = Value
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
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			m_lNavigate = Value
		End Set
	End Property
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			m_lProcessMode = Value
		End Set
	End Property
	Public Property TransactionType() As String
		Get
			Return m_sTransactionType
		End Get
		Set(ByVal Value As String)
			m_sTransactionType = Value
		End Set
	End Property
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	Public WriteOnly Property LanguageID() As Integer
		Set(ByVal Value As Integer)
			m_iLanguageID = Value
		End Set
	End Property

	'Private Sub Status(ByVal Value As Integer)
		' Set the interface exit status.
		'm_lStatus = Value
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			' Return the interface exit status.
			Return m_lStatus
		End Get
	End Property
	'KR 13/02/2003 start
	Public Property DeleteWorkTableFlag() As Integer
		Get
			Return m_lDeleteWorkTableFlag
		End Get
		Set(ByVal Value As Integer)
			m_lDeleteWorkTableFlag = Value
		End Set
	End Property
	'KR 13/02/2003 end
	Public Property RiskDescription() As String
		Get
			Return m_sRiskDescription
		End Get
		Set(ByVal Value As String)
			m_sRiskDescription = Value
		End Set
	End Property
	Public Property ClaimRiskDesciption() As String
		Get
			Return m_sClaimRiskDesciption
		End Get
		Set(ByVal Value As String)
			m_sClaimRiskDesciption = Value
		End Set
	End Property
	Public Property Claimid() As Integer
		Get
			Return m_lclaimid
		End Get
		Set(ByVal Value As Integer)
			m_lclaimid = Value
		End Set
	End Property
	Public Property RiskType() As Integer
		Get
			Return m_lRiskType
		End Get
		Set(ByVal Value As Integer)
			m_lRiskType = Value
		End Set
	End Property
	Public ReadOnly Property ThisPayment() As Decimal
		Get
			Return m_cThisPayment
		End Get
	End Property
	Public ReadOnly Property ThisPaymentCurrencyID() As Integer
		Get
			Return m_lThisPaymentCurrencyID
		End Get
	End Property
	'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
	
	Public WriteOnly Property ScreenCaption() As String
		Set(ByVal Value As String)
			m_sScreenCaption = Value
		End Set
	End Property
	
	
	Public Property IsOpenClaimNoTrans() As Boolean
		Get
			Return m_bOpenClaimNoTrans
		End Get
		Set(ByVal Value As Boolean)
			m_bOpenClaimNoTrans = Value
		End Set
	End Property
    'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4 )

    Public Property ReserveLimitExceeded() As Boolean
        Get
            Return m_bReserveLimitExceeded
        End Get
        Set(ByVal Value As Boolean)
            m_bReserveLimitExceeded = Value
        End Set
    End Property

    Public Property ExceededReserve() As Decimal
        Get
            Return m_dExceededReserve
        End Get
        Set(ByVal Value As Decimal)
            m_dExceededReserve = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer
		Dim result As Integer = 0
		Try 
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
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
			

			m_lReturn = m_oBusiness.GetDetails()
			
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
	' ***************************************************************** '
	Public Function BusinessToInterface() As Integer
		
		Dim result As Integer = 0
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
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
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
				Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
					
			End Select
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
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
        Dim vResultArray(,) As Object
		Const ACFirstItem As Integer = 0
		Const ACFirstRow As Integer = 0
		Const ACSecondRow As Integer = 1
		Const ACThirdRow As Integer = 2
		Const ACNumberOfColumns As Integer = 4 ' Zero based
		Const ACNumberOfRows As Integer = 2 ' Zero based
		Try 
			result = gPMConstants.PMEReturnCode.PMTrue
			' Get the lookup values.

			m_vLookupValues = Nothing

			m_vLookupDetails = Nothing
			
			ReDim m_vLookupValues(ACNumberOfColumns, ACNumberOfRows) ' Zero based
			' Progress status
			m_vLookupValues(ACFirstItem, ACFirstRow) = "Progress_Status"
			
			' Secondary cause
			m_vLookupValues(ACFirstItem, ACSecondRow) = "Secondary_Cause"
			
			'Primary cause
			m_vLookupValues(ACFirstItem, ACThirdRow) = "Primary_Cause"
			

			m_lReturn = m_oBusiness.GetLookupValues(gPMConstants.PMELookupType.PMLookupAll, m_vLookupValues, m_iLanguageID, m_vLookupDetails)
			
			m_lReturn = GetLookupValues()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Populate Progress status combo box
			m_lReturn = GetLookupDetails("Progress_Status", cmbProgressStatus)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			' Populate Secondary cause combo box
			m_lReturn = GetLookupDetails("Secondary_Cause", cmbSecondaryCause)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = GetLookupDetails("Primary_Cause", cmbPrimaryCause)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the Lookup values
			If Not ViewRiskFlag Then

				m_lReturn = m_oBusiness.GetBasicClaimDetails(m_lclaimid, vResultArray)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				' Do we have any records ?
				If Not Information.IsArray(vResultArray) Then
					' No Records, return PMFalse
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				
				'RWH(10/11/2000) Display Policy AND Claim no.




                Me.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & " [" & CStr(vResultArray(ACClient_name, ACFirstRow)) & " " & CStr(vResultArray(ACPolicy_Number, ACFirstRow)) & "]" & New String(" "c, 2) & "Claim no. " & CStr(vResultArray(ACClaim_Number, ACFirstRow))


                m_sClaimNo = CStr(vResultArray(ACClaim_Number, ACFirstRow))

                'fraGeneralDetails.Caption = iPMFunc.GetResData(iLangID:=m_iLanguageID, lID:=ACGenralDetailsFrame, iDataType:=PMResString)


                txtDescription.Text = CStr(vResultArray(ACDescription, ACFirstRow))

                ClaimNumber = CStr(vResultArray(ACClaim_Number, ACFirstRow))

                Dim auxVar As Object = vResultArray(ACClient_short_name, ACFirstRow)


                If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then

                    m_sClient = gPMFunctions.ToSafeString(CStr(vResultArray(ACClient_short_name, ACFirstRow)))
                Else

                    m_sClient = gPMFunctions.ToSafeString(CStr(vResultArray(ACClient_name, ACFirstRow)))
                End If

                ' Tool tips


                Toolbar1.Items.Item(ACFinancialdetailsButton - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACToolTipFinancialdetailsButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                Toolbar1.Items.Item(ACEventLogButton - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACToolTipEventLogButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                Toolbar1.Items.Item(ACRiskDetailsButton - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACToolTipRiskDetailsButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'RAM20021021 :  NRMA Changes - Sirius Process Number 126 - Start


                Toolbar1.Items.Item(ACPartySummaryButton - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACToolTipPartySummaryButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                Toolbar1.Items.Item(ACPolicySummaryButton - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACToolTipPolicySummaryButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                'RAM20021021 :  NRMA Changes - Sirius Process Number 126 - End

                ' get Claim status
                Select Case (vResultArray(ACclaim_status_id, ACFirstRow))
                    Case CLMProvisionalOpenClaim

                        txtStatus.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACCLMProvisionalOpenClaim, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    Case CLMLiveOpenClaim

                        txtStatus.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACCLMLiveOpenClaim, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    Case CLMClosed

                        txtStatus.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACCLMClosed, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    Case CLMReOpened

                        txtStatus.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACCLMReOpened, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    Case CLMReClosed

                        txtStatus.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACCLMReClosed, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                End Select
                ' Get Progress status

                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(vResultArray(ACProgress_Status_Id, ACFirstRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    For iItem As Integer = ACFirstItem To cmbProgressStatus.Items.Count - 1

                        If CInt(vResultArray(ACProgress_Status_Id, ACFirstRow)) = VB6.GetItemData(cmbProgressStatus, iItem) Then
                            cmbProgressStatus.SelectedIndex = iItem
                            Exit For
                        End If
                    Next iItem
                End If
                ' Get Primary Cause

                Dim dbNumericTemp2 As Double
                If Double.TryParse(CStr(vResultArray(ACPrimary_Cause_Id, ACFirstRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    For iItem As Integer = ACFirstItem To cmbPrimaryCause.Items.Count - 1

                        If CInt(vResultArray(ACPrimary_Cause_Id, ACFirstRow)) = VB6.GetItemData(cmbPrimaryCause, iItem) Then
                            cmbPrimaryCause.SelectedIndex = iItem
                            Exit For
                        End If
                    Next iItem
                End If

                ' Get secondary cause

                Dim dbNumericTemp3 As Double
                If Double.TryParse(CStr(vResultArray(ACSecondary_Cause_Id, ACFirstRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    For iItem As Integer = ACFirstItem To cmbSecondaryCause.Items.Count - 1

                        If CInt(vResultArray(ACSecondary_Cause_Id, ACFirstRow)) = VB6.GetItemData(cmbSecondaryCause, iItem) Then
                            cmbSecondaryCause.SelectedIndex = iItem
                            Exit For
                        End If
                    Next iItem
                End If
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer
        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


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
        Try


            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




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
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(12/03/2001)
            'Made full row select on list views
            'm_lReturn& = SetExtraListViewProperties(v_hWndList:=lvwPerils.hwnd, _
            'v_vShowRowSelect:=True)
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        SetInterfaceDefaults = PMFalse
            '        Exit Function
            '    End If

            '    If (m_sTransactionType = "C_CP") Then
            '        cmdPerilAdd.Enabled = False
            '        cmdPerilDelete.Enabled = False
            '    End If

            SSTabHelper.SetTabVisible(tabMainTab, ACDriver, False)
            SSTabHelper.SetTabVisible(tabMainTab, ACThirdParty, False)
            SSTabHelper.SetTabVisible(tabMainTab, ACRepairer, False)
            SSTabHelper.SetTabVisible(tabMainTab, ACWitness, False)


            If m_sTransactionType = "C_CR" Then
                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    txtComment(0).Enabled = False
                    txtDate(0).Enabled = False
                    txtDescription.Enabled = False
                    txtInteger(0).Enabled = False
                    txtStatus.Enabled = False
                    txtText(0).Enabled = False
                    cmbLookup(0).Enabled = False
                    cmbPrimaryCause.Enabled = False
                    cmbProgressStatus.Enabled = False
                    cmbSecondaryCause.Enabled = False
                    'cmdPerilAdd.Enabled = False
                    'cmdPerilDelete.Enabled = False
                    'cmdPerilEdit.Enabled = False
                    chkCheck(0).Enabled = False
                End If
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


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            'ReDim m_ctlTabFirstLast(1, )
            '
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

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

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


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACOK, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACCancel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACHelp, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    cmdNavigate.Caption = iPMFunc.GetResData( _
            'iLangID:=m_iLanguageID, _
            'lID:=ACNavigateButton , _
            'iDataType:=PMResString)


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACRiskTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACRiskTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    tabMainTab.TabCaption(2) = iPMFunc.GetResData( _
            'iLangID:=m_iLanguageID, _
            'lID:=ACGeneralDetails, _
            'iDataType:=PMResString)


            lblProgressStatus.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACProgressStaus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACClaimDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPrimaryCause.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACPrimaryCause, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSecondaryCause.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACSecondaryCause, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraPeril.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACCoverPerils, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    cmdPerilAdd.Caption = iPMFunc.GetResData( _
            ''        iLangID:=m_iLanguageID, _
            ''        lId:=ACAdd, _
            ''        iDataType:=PMResString)
            '
            '    cmdPerilDelete.Caption = iPMFunc.GetResData( _
            ''        iLangID:=m_iLanguageID, _
            ''        lId:=ACDelete, _
            ''        iDataType:=PMResString)
            '
            '    cmdPerilEdit.Caption = iPMFunc.GetResData( _
            ''        iLangID:=m_iLanguageID, _
            ''        lId:=ACEdit, _
            ''        iDataType:=PMResString)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=m_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=m_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.
                    '            m_lReturn& = m_oBusiness.GetLookupValues( _
                    ''                iLookupType:=PMLookupSingle, _
                    ''                vTableArray:=m_vLookupValues, _
                    ''                iLanguageID:=m_iLanguageID, _
                    ''                vResultArray:=m_vLookupDetails)

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=m_iLanguageID, vResultArray:=m_vLookupDetails)

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
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
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

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.
                Dim ctlLookup_NewIndex As Integer = -1
                ctlLookup_NewIndex = ctlLookup.Items.Add(CStr(m_vLookupDetails(ACDetailDesc, lCntr)))
                VB6.SetItemData(ctlLookup, ctlLookup_NewIndex, CInt(m_vLookupDetails(ACDetailKey, lCntr)))

                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then
                        ctlLookup.SelectedIndex = ctlLookup_NewIndex
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then
                If ctlLookup.Items.Count > 0 Then
                    'RWH(12/04/2001) Set box to blank unless value is specifically set.
                    ctlLookup.SelectedIndex = -1
                    '            ctlLookup.ListIndex = 0
                End If
            End If
            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: cmdPerilAdd_Click
    '
    ' Description: On command button click event this procedure will be
    '               executed. This will display the Add peril screen.
    '               If OK was pressed on the Add screen peril will be added
    '               in the Claim_Peril table.
    '
    ' ***************************************************************** '

    'Private Sub cmdPerilAdd_Click()
    '
    'Dim oFrmAddPeril As frmAddPeril
    'Dim vDataArray As Variant
    'Dim sMessage As String
    'Dim sTitle As String
    'Dim iPeril As Integer
    ''JMK 23/05/2001 'Dim iPerilID As Integer: change to Long
    'Dim lPerilID As Long
    '
    'Const ACPerilType = 0
    'Const ACDescription = 1
    '
    '    m_lReturn = GetPerilTypes(vDataArray)
    '
    '    If m_lReturn <> PMTrue Then
    '        SetPerilButtons
    '        Exit Sub
    '    End If
    '
    '    If IsArray(vDataArray) = False Then
    '        sTitle$ = iPMFunc.GetResData( _
    ''            iLangID:=m_iLanguageID, _
    ''            lId:=ACAddPerilTitle, _
    ''            iDataType:=PMResString)
    '        sMessage$ = iPMFunc.GetResData( _
    ''            iLangID:=m_iLanguageID, _
    ''            lId:=ACAddPerilDetails, _
    ''            iDataType:=PMResString)
    '
    '        MsgBox sMessage$, vbOKOnly, sTitle$
    '        SetPerilButtons
    '        Exit Sub
    '    End If
    '
    '    Set oFrmAddPeril = New frmAddPeril
    '
    '    With oFrmAddPeril
    '        .tabAddPeril.Top = 120
    '        .tabAddPeril.Left = 120
    '
    '        .tabAddPeril.Height = .cmdCancel.Top - 120 - .tabAddPeril.Top
    '        .tabAddPeril.Width = .Width - 240 - .tabAddPeril.Left
    '
    '        ' Get Peril types and populate the FRMPeril's  cmbPeril combobox
    '        If IsArray(vDataArray) = True Then
    '            For iPeril = LBound(vDataArray, 2) To UBound(vDataArray, 2)
    '                .cmbPeril.AddItem vDataArray(ACDescription, iPeril)
    '                .cmbPeril.ItemData(.cmbPeril.NewIndex) = vDataArray(ACPerilType, iPeril)
    '            Next iPeril
    '        End If
    '
    '        If .cmbPeril.ListCount > 0 Then
    '            .cmbPeril.ListIndex = 0
    '        End If
    '    End With
    '
    '    oFrmAddPeril.Show vbModal
    '
    '    '**********************************
    '    ' MEvans : 03-12-2002 : 202
    '    ' Rerun the manage salvage set up as on return from peril
    '    ' as salvage items may now exist
    '    m_lReturn = SetupManageSalvage
    '    If m_lReturn <> PMTrue Then
    '        LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to reset manage salvage", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="cmdPerilAdd_Click", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '    End If
    '    '**********************************
    '
    '
    '    If oFrmAddPeril.Status <> PMOk Then
    '        Set oFrmAddPeril = Nothing
    '        SetPerilButtons
    '        Exit Sub
    '    End If
    '
    '    'Public Function AddClaimPeril(ByVal v_iClaimId As Integer, ByVal v_iPerilTypeId As Integer, r_iClaimPerilId As Integer) As Long
    '    m_lReturn = AddClaimPeril(v_iPerilTypeId:=oFrmAddPeril.cmbPeril.ItemData(oFrmAddPeril.cmbPeril.ListIndex), _
    ''                              r_lPerilID:=lPerilID, _
    ''                              v_sDescription:=oFrmAddPeril.txtDescription)
    '
    '    If m_lReturn <> PMTrue Then
    '        SetPerilButtons
    '        Exit Sub
    '        Set oFrmAddPeril = Nothing
    '    End If
    '
    '    'JMK 23/05/2001 - call Sub LoadPerilData with new optional flag
    '    '                   (to indicate where it was called from)
    '
    '    LoadPerilData (1)
    '
    ''    Dim lstPeril As ListItem
    ''    Set lstPeril = lvwPerils.ListItems.Add(Text:=RiskDescription)
    ''    lstPeril.Tag = lPerilID
    ''    lstPeril.SubItems(1) = oFrmAddPeril.cmbPeril.Text
    ''    lstPeril.SubItems(2) = ACFormatforNumber
    ''    lstPeril.SubItems(3) = ACFormatforNumber
    ''    lstPeril.SubItems(4) = "1"
    ''    cmdPerilDelete.Enabled = False
    ''    cmdPerilEdit.Enabled = False
    '
    '    Set oFrmAddPeril = Nothing
    '
    'End Sub

    ' ***************************************************************** '
    ' Name:        AddClaimPeril
    '
    ' Description: AddClaimPeril is called from cmdPerilAdd_ClickOn event.
    '               If OK was pressed on the Add screen peril will be added
    '               in the Claim_Peril table. To add to Claim_Peril the business object is called
    '
    ' JMK 23/05/3001: v_iPerilTypeId As Int - change to Long
    '               'add Risk as Long
    ' ***************************************************************** '

    'Private Function AddClaimPeril(ByVal v_iPerilTypeId As Integer, ByRef r_lPerilID As Integer, Optional ByRef v_sDescription As String = "") As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'ByVal v_iClaimId As Integer, ByVal v_iPerilTypeId As Integer, r_iClaimPerilId As Integer) As Long

    'm_lReturn = m_oBusiness.AddClaimPeril(Claimid, v_iPerilTypeId, r_lPerilID, Risk, v_sDescription)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add ClaimPeril", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimPeril", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name:        GetPerilTypes
    '
    ' Description: GetPerilTypes gets all the peril type defined for a Policy for
    '               Underwriting or all the peril types for broking. but the Peril types
    '               should not be defined, already, in the Claim_Peril table
    '
    ' ***************************************************************** '

    'Private Function GetPerilTypes(ByRef r_vDataArray As Object) As Integer
    'Dim result As Integer = 0
    'Try 
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'MSS260901 - Added for merge
    'TN20010423 Start

    'm_lReturn = m_oBusiness.GetPerilTypeForRisk(Claimid, Risk, Policy, r_vDataArray, m_bLossSchedule)
    '
    'TN20010423 End
    'MSS260901 - Merge end
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'Return result
    '
    'Catch excep As System.Exception
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get peril types", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    'Return result
    'End Try
    'End Function




    Private Sub cmdAddTask_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTask.Click

        m_bAddtask = True
        m_lReturn = CreateWorkManagerTask()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        Else
            cmdCancel_Click(cmdCancel, New EventArgs())
        End If

        '    If m_sTransactionType = "C_CP" Then
        '        m_lReturn = m_oBusiness.CreateWorkTask("PAYCLM", txtDescription.Text, m_vKeyArray)
        '    ElseIf m_sTransactionType = "C_CO" Then
        '        m_lReturn = m_oBusiness.CreateWorkTask("MAINCLM", txtDescription.Text, m_vKeyArray)
        '    End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Create Work Task Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTask")
            Exit Sub
        End If


    End Sub

    Private Function CreateWorkManagerTask() As Integer
        Dim result As Integer = 0

        Dim oTaskInstance As iPMWrkTaskInstance.Interface_Renamed
        Dim lReturn As Integer
        Dim v_lAction As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_lAction = 1

            ' Create the Component
            Dim temp_oTaskInstance As Object
            lReturn = g_oObjectManager.GetInstance(temp_oTaskInstance, sClassName:="iPMWrkTaskInstance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oTaskInstance = temp_oTaskInstance
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Process Modes

            lReturn = oTaskInstance.SetProcessModes(vTask:=v_lAction, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bAddtask Then
                ShowPartySummaryDetails()
            End If
            ' Set Task Group Id and Task Id

            oTaskInstance.PMWrkTaskGroupId = 5

            oTaskInstance.PMWrkTaskId = 18


            oTaskInstance.duedate = DateTime.Now

            oTaskInstance.customer = m_sPartyShortName.Trim() & " " & m_sClaimNo

            oTaskInstance.Description = m_sClaimNo

            oTaskInstance.disablecustomer = gPMConstants.PMEReturnCode.PMTrue

            oTaskInstance.TaskStatus = 2



            oTaskInstance.TaskInstKeyArray = m_vKeyArray

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Start the Form

            lReturn = oTaskInstance.Start
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start Task Instance Display Form:-      iPMWrkTaskInstanceDisplay.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="StartStep", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' If the User Cancelled then exit as we do not need
            ' to Refresh the Form details.

            If oTaskInstance.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
                'r_vPMWrkTaskInstanceCntArray = ""

                oTaskInstance.Dispose()
                oTaskInstance = Nothing
                Return result
            End If

            oTaskInstance.Dispose()
            oTaskInstance = Nothing

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkManagerTask", vApp:=ACApp, vClass:=ACClass, vMethod:=" CreateWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result


            Return result
        End Try
    End Function


    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String
        Dim vValue As String = ""
        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMRiskDetails.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMRiskDetails.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = m_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_lCloseClaim = gPMConstants.PMEReturnCode.PMFalse

            'CMG/PB See if LossSchedule is enabled and set a private boolean
            With g_oObjectManager
                iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTLossSchedule, .SourceID, vValue)
            End With
            m_bLossSchedule = (gPMFunctions.NullToString(vValue) = "1")
            'End CMG

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'GSD 150702
            '    With g_oObjectManager
            '        'DC131003 -PN7429 -had wrong parameters
            '        Call getProductOptionValue(SIROPTClaimsBuilder, SIRBCHHeadOffice, vreturn)
            '    End With
            '    If m_lReturn <> PMTrue Then
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="getProductOptionValue failed for Claims Builder", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Initialise"
            '    End If
            '
            '    If vreturn = "1" Then
            m_bClaimsBuilder = False
            '    End If

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    '*******************************************************************
    'Name        : GetRiskDetails
    'Description : GetRiskDetails procedure is used for getting basic details
    '              of a risk
    '********************************************************************
    Private Sub GetRiskDetails()
        Dim vResultArray(,) As Object
        Dim sSiriusProduct As String = ""
        Const ACFirstColumn As Integer = 0
        Const ACFirstRow As Integer = 0
        Const ACSecondColumn As Integer = 1
        ' Get the Product definition

        m_lReturn = m_oBusiness.GetSiriusProduct(sSiriusProduct)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            Exit Sub
        End If
        SiriusProduct = sSiriusProduct
        ' Get the Policy Id , then Policy contains the Event_insurance
        ' update Policy with File_insurance_cnt

        'AK 30032001 - As we already have the policy number, probably we do not need to fetch it again
        '              the function being called in the following line, actually intrprets policynumber as eventID
        '              so commented the following 5 lines (beginning with ***

        '***m_lReturn& = m_oBusiness.GetPolicynumber(Policy, vResultArray)
        '***If m_lReturn <> PMTrue Then Exit Sub
        '***If IsArray(vResultArray) = False Then Exit Sub

        '***Policy = vResultArray(0, 0)
        '***vResultArray = Null

        'AK 30032001 - end
        ' Get the risk details for the risk pertaining to a policy

        m_lReturn = m_oBusiness.GetRiskDetails(Risk, Policy, vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then Exit Sub
        If Not Information.IsArray(vResultArray) Then Exit Sub


        RiskType = CInt(vResultArray(ACFirstColumn, ACFirstRow))

        RiskDescription = CStr(vResultArray(ACSecondColumn, ACFirstRow))

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        g_bFirst = True
        ' Forms load event.

        Try
            Dim vdataArray(,) As Object
            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the RiskTypeId & Description
            If Not ViewRiskFlag Then
                GetRiskDetails()
            Else
                Claimid = 0
                RiskType = Risk
                Task = gPMConstants.PMEComponentAction.PMView
            End If

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            collRiskDataDefnForDates = New Collection()
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

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            '-------------------------------------------------------------------------------------
            '   19/07/2002  RVH BEGIN
            '                   If this is not an Open claim and it is Underwriting then
            '                   get the original claim id into a member variable
            '-------------------------------------------------------------------------------------

            'Only do this if we are not viewing the screen from risk type maintainance
            'because the m_lclaimid is not set
            If Not ViewRiskFlag Then

                If m_sTransactionType <> "C_CO" Then
                    'get original claim id

                    m_lReturn = m_oBusiness.GetOriginalClaimID(v_lClaimId:=m_lclaimid, r_lOriginalClaimID:=m_lOriginalClaimId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        ' Set the mouse pointer to normal.
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub

                    End If
                End If
            End If
            '-------------------------------------------------------------------------------------
            '   19/07/2002  RVH END
            '-------------------------------------------------------------------------------------


            SetInterface()


            m_lReturn = m_oBusiness.GetCurrentReserveRecovery(m_lclaimid, vdataArray)
            If Information.IsArray(vdataArray) Then
                m_cReserveold = gPMFunctions.ToSafeCurrency(vdataArray(1, 0))
            End If
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'developer guide no.7
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub

                End If
            End If

            If m_bClaimsBuilder Then

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20021023 : Close the Party  Policy Summary Screen, if they
                '               are loaded
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                m_lReturn = ClosePartyPolicySummary()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                End If
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            collRiskDataDefnForDates = Nothing

            

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

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

            ' BSJ 28/09/2004 - PN 15191, Risk details is displayed via iPMURiskWrapper which in turn
            ' is currently hard coded to display the iPMURisk screen in a modeless state, therefore we cannot
            ' check if it is being killed correctly so we must kill here
            If m_oRisk Is Nothing Then
                ' Do nothing
            Else
                m_oRisk.Dispose()
                'Clear it
                m_oRisk = Nothing
            End If

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


            'end

        Catch

            Exit Sub
        End Try
    End Sub

    '*******************************************************************
    'Name        : Form_Resize
    'Description : Form_Resize procedure is used for setting the controls
    '              position and sizing them with respect to the form size.
    '********************************************************************
    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        Dim NewLargeChange, iOldTab As Integer

        Try

            If WindowState = FormWindowState.Minimized Then
                Exit Sub
            End If

            tabMainTab.Visible = False 'pkh 06/09/2002 - stops 'flickering'

            iOldTab = SSTabHelper.GetSelectedIndex(tabMainTab)
            ' Setting the lefts of the controls
            tabMainTab.Left = VB6.TwipsToPixelsX(ACLeftOfTabInform)

            ' Set the label width
            If WindowState <> FormWindowState.Maximized Then
                If VB6.PixelsToTwipsY(Height) < ACMinimumFormHeight Then
                    Height = VB6.TwipsToPixelsY(ACMinimumFormHeight)
                End If

                If VB6.PixelsToTwipsX(Width) < ACMinimumFormWidth Then
                    Width = VB6.TwipsToPixelsX(ACMinimumFormWidth)
                End If
            End If

            'MSS260901 - Added for merge
            If SSTabHelper.GetTabVisible(tabMainTab, 2) Then
                SSTabHelper.SetSelectedIndex(tabMainTab, 2)
            End If

            'MS260901 - Merge end

            ' Button width
            cmdHelp.Width = VB6.TwipsToPixelsX(ACButtonWidth)
            cmdCancel.Width = VB6.TwipsToPixelsX(ACButtonWidth)
            cmdOK.Width = VB6.TwipsToPixelsX(ACButtonWidth)
            cmdAddTask.Width = VB6.TwipsToPixelsX(ACButtonWidth)



            '    cmdPerilAdd.Width = ACButtonWidth
            '    cmdPerilDelete.Width = ACButtonWidth
            '    cmdPerilEdit.Width = ACButtonWidth

            ' Button height
            cmdCancel.Height = VB6.TwipsToPixelsY(ACButtonHeight)
            cmdHelp.Height = VB6.TwipsToPixelsY(ACButtonHeight)
            cmdOK.Height = VB6.TwipsToPixelsY(ACButtonHeight)
            cmdAddTask.Height = VB6.TwipsToPixelsY(ACButtonHeight)
            '    cmdPerilAdd.Height = ACButtonHeight
            '    cmdPerilDelete.Height = ACButtonHeight
            '    cmdPerilEdit.Height = ACButtonHeight

            ' Button left
            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(ACButtonWidth + (ACFormBorder * ACTwice))
            cmdCancel.Left = cmdHelp.Left - VB6.TwipsToPixelsX(ACButtonWidth + ACFormBorder)
            cmdOK.Left = cmdCancel.Left - VB6.TwipsToPixelsX(ACButtonWidth + ACFormBorder)
            cmdAddTask.Left = cmdOK.Left - VB6.TwipsToPixelsX(ACButtonWidth + ACFormBorder)
            cmdNavigate.Left = tabMainTab.Left

            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(ACButtonHeight * 3 + ACFormBorder)
            cmdCancel.Top = cmdHelp.Top
            cmdOK.Top = cmdHelp.Top
            cmdNavigate.Top = cmdHelp.Top
            cmdAddTask.Top = cmdHelp.Top

            ' Setting the Tops of all the controls.
            tabMainTab.Top = VB6.TwipsToPixelsY((ACTopOfTabInform * ACThrice) + ACTopOfTabInform)
            tabMainTab.Left = VB6.TwipsToPixelsX(ACTopOfTabInform)
            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(ACTopOfTabInform * ACThrice)
            tabMainTab.Height = cmdHelp.Top - VB6.TwipsToPixelsY((ACTopOfTabInform * ACThrice) + ACTopOfTabInform + ACFormBorder)

            'TN20010807 - start - don't see why we need to do this - plus its does not work
            '    If tabMainTab.TabVisible(ACDriver) Then
            '        fraDriver.Width = tabMainTab.Width - 240 - fraDriver.Left
            '        fraDriver.Height = tabMainTab.Height - 240 - fraDriver.Top
            '        uctDriver.Width = fraDriver.Width - 480
            '        uctDriver.Height = fraDriver.Height - 480
            '    End If
            '
            '    If tabMainTab.TabVisible(ACThirdParty) Then
            '        fraThirdParty.Width = tabMainTab.Width - 240 - fraThirdParty.Left
            '        fraThirdParty.Height = tabMainTab.Height - 240 - fraThirdParty.Top
            '        uctThirdParty.Width = fraThirdParty.Width - 480
            '        uctThirdParty.Height = fraThirdParty.Height - 480
            '    End If
            '
            '    If tabMainTab.TabVisible(ACRepairer) Then
            '        fraRepairer.Width = tabMainTab.Width - 240 - fraRepairer.Left
            '        fraRepairer.Height = tabMainTab.Height - 240 - fraRepairer.Top
            '        uctRepairer.Width = fraRepairer.Width - 480
            '        uctRepairer.Height = fraRepairer.Height - 480
            '    End If
            '
            '    If tabMainTab.TabVisible(ACWitness) Then
            '        fraWitness.Width = tabMainTab.Width - 240 - fraWitness.Left
            '        fraWitness.Height = tabMainTab.Height - 240 - fraWitness.Top
            '        uctWitness.Width = fraWitness.Width - 480
            '        uctWitness.Height = fraWitness.Height - 480
            '    End If
            'TN20010807 - end

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            cmbProgressStatus.Top = VB6.TwipsToPixelsY(ACTopOfFirstTextBoxInTab)
            txtStatus.Top = VB6.TwipsToPixelsY(ACTopOfFirstTextBoxInTab)
            txtDescription.Top = txtStatus.Top + VB6.TwipsToPixelsY(ACNormalGapBetweenTopsOfTwoTextBoxes)
            cmbPrimaryCause.Top = txtDescription.Top + VB6.TwipsToPixelsY(ACNormalGapBetweenTopsOfTwoTextBoxes)
            cmbSecondaryCause.Top = cmbPrimaryCause.Top

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            lblProgressStatus.Top = cmbProgressStatus.Top + VB6.TwipsToPixelsY(ACDiffBetweenTopsOfLabelAndText)
            lblDescription.Top = txtDescription.Top + VB6.TwipsToPixelsY(ACDiffBetweenTopsOfLabelAndText)
            lblPrimaryCause.Top = cmbPrimaryCause.Top + VB6.TwipsToPixelsY(ACDiffBetweenTopsOfLabelAndText)
            lblSecondaryCause.Top = cmbSecondaryCause.Top + VB6.TwipsToPixelsY(ACDiffBetweenTopsOfLabelAndText)
            lblStatus.Top = txtStatus.Top + VB6.TwipsToPixelsY(ACDiffBetweenTopsOfLabelAndText)

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            ' Setting the frame
            fraPeril.Left = VB6.TwipsToPixelsX(ACLeftOfFrameInTab)
            fraPeril.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmbPrimaryCause.Top) + VB6.PixelsToTwipsY(cmbPrimaryCause.Height) + ACFormBorder)
            fraPeril.Width = tabMainTab.Width - VB6.TwipsToPixelsX(ACTwice * ACLeftOfFrameInTab)
            fraPeril.Height = tabMainTab.Height - (fraPeril.Top + VB6.TwipsToPixelsY(ACLeftOfFrameInTab))

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            ' Set the label height
            lblDescription.Height = VB6.TwipsToPixelsY(ACLabelHeight)
            lblPrimaryCause.Height = VB6.TwipsToPixelsY(ACLabelHeight)
            lblProgressStatus.Height = VB6.TwipsToPixelsY(ACLabelHeight)
            lblSecondaryCause.Height = VB6.TwipsToPixelsY(ACLabelHeight)
            lblStatus.Height = VB6.TwipsToPixelsY(ACLabelHeight)

            ' Set textbox height
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            txtStatus.Height = VB6.TwipsToPixelsY(ACTextBoxHeight)
            txtDescription.Height = VB6.TwipsToPixelsY(ACTextBoxHeight)

            lblDescription.Width = VB6.TwipsToPixelsX(ACLabelWidthMedium)
            lblPrimaryCause.Width = VB6.TwipsToPixelsX(ACLabelWidthMedium)
            lblProgressStatus.Width = VB6.TwipsToPixelsX(ACLabelWidthMedium)
            lblSecondaryCause.Width = VB6.TwipsToPixelsX(ACLabelWidthMedium)
            lblStatus.Width = VB6.TwipsToPixelsX(ACLabelWidthMedium)

            lblProgressStatus.Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn)
            lblDescription.Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn)
            lblPrimaryCause.Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn)

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            cmbProgressStatus.Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn + ACFormBorder + ACLabelWidthMedium)
            txtDescription.Left = cmbProgressStatus.Left
            cmbPrimaryCause.Left = cmbProgressStatus.Left

            lblStatus.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmbProgressStatus.Left) + VB6.PixelsToTwipsX(cmbProgressStatus.Width) + ACFormBorder)
            lblSecondaryCause.Left = lblStatus.Left
            txtStatus.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lblStatus.Left) + ACFormBorder + ACLabelWidthMedium)
            cmbSecondaryCause.Left = txtStatus.Left

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            '    cmdPerilAdd.Left = fraPeril.Width - (ACButtonWidth + ACFormBorder)
            '    cmdPerilDelete.Left = fraPeril.Width - (ACButtonWidth + ACFormBorder)
            '    cmdPerilEdit.Left = fraPeril.Width - (ACButtonWidth + ACFormBorder)
            '
            '    cmdPerilAdd.Top = ACFormBorder * ACTwice
            '    cmdPerilEdit.Top = cmdPerilAdd.Top + ACButtonHeight + ACFormBorder
            '    cmdPerilDelete.Top = cmdPerilEdit.Top + ACButtonHeight + ACFormBorder

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            If SSTabHelper.GetTabVisible(tabMainTab, ACGeneral) Then
                SSTabHelper.SetSelectedIndex(tabMainTab, ACGeneral)

                For i As Integer = 0 To 4
                    If SSTabHelper.GetTabVisible(tabMainTab, 6 + i) Then
                        SSTabHelper.SetSelectedIndex(tabMainTab, 6 + i)

                        fraGeneralDetails(i).Top = VB6.TwipsToPixelsY(480)
                        fraGeneralDetails(i).Left = VB6.TwipsToPixelsX(240)
                        fraGeneralDetails(i).Width = tabMainTab.Width - VB6.TwipsToPixelsX(240) - fraGeneralDetails(i).Left
                        fraGeneralDetails(i).Height = tabMainTab.Height - VB6.TwipsToPixelsY(240) - fraGeneralDetails(i).Top

                        Picture1(i).Top = VB6.TwipsToPixelsY(160)
                        Picture1(i).Left = VB6.TwipsToPixelsX(30)
                        Picture1(i).Width = fraGeneralDetails(i).Width - VB6.TwipsToPixelsX(53)
                        Picture1(i).Height = fraGeneralDetails(i).Height - VB6.TwipsToPixelsY(200)

                        SSTabHelper.SetSelectedIndex(tabMainTab, ACGeneral)
                        VScroll1(i).Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Picture1(i).Width) - VB6.PixelsToTwipsX(VScroll1(i).Width) - 20)
                        VScroll1(i).Top = 0
                        VScroll1(i).Height = Picture1(i).Height
                        'VScroll1.Max = CInt((Picture2.Height - 195) * 0.25)
                        NewLargeChange = (VScroll1(i).Maximum - (VScroll1(i).LargeChange + 1))
                        VScroll1(i).Maximum = VScroll1(i).Maximum + NewLargeChange - VScroll1(i).LargeChange
                        VScroll1(i).LargeChange = NewLargeChange
                        'End - Bug ID - 04, Date 01st Nov 2000, Author: DG
                        SSTabHelper.SetSelectedIndex(tabMainTab, ACGeneral)
                        Picture2(i).Width = VScroll1(i).Left - VB6.TwipsToPixelsX(195)
                        If VB6.PixelsToTwipsY(Picture2(i).Height) < VB6.PixelsToTwipsY(Picture1(i).Height) Then
                            VScroll1(i).Value = 0
                            VScroll1(i).Visible = False
                        Else
                            VScroll1(i).Visible = True
                        End If

                    End If

                Next i

            End If

            'Comment Tab
            '    tabMainTab.Tab = tabMainTab.Tabs - 1
            'S4B Claim Enhancements R&D 2005
            If SSTabHelper.GetTabVisible(tabMainTab, ACCommentsTab) Then
                SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                txtComment(0).Top = VB6.TwipsToPixelsY(480)
                txtComment(0).Left = VB6.TwipsToPixelsX(ACFormBorder)
                txtComment(0).Width = tabMainTab.Width - VB6.TwipsToPixelsX(ACFormBorder) - txtComment(0).Left
                txtComment(0).Height = tabMainTab.Height - VB6.TwipsToPixelsY(ACFormBorder) - txtComment(0).Top
            End If

            If Not SSTabHelper.GetTabVisible(tabMainTab, iOldTab) Then
                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            Else
                SSTabHelper.SetSelectedIndex(tabMainTab, iOldTab)
            End If


            tabMainTab.Visible = True 'pkh 06/09/2002 - stops 'flickering'

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the form resize", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Resize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    'Private Sub SetPerilButtons()
    '
    '    If ViewRiskFlag = True Or Task = PMView Or (m_sTransactionType = "C_CP") Then
    '        cmdPerilAdd.Enabled = False
    '    Else
    '        cmdPerilAdd.Enabled = True
    '    End If
    '    If Not lvwPerils.SelectedItem Is Nothing Then
    ''DC070601
    ''        If ViewRiskFlag <> True And Task <> PMView Then
    '        If ViewRiskFlag <> True And (m_sTransactionType <> "C_CP") And (m_iTask <> PMView) Then
    '            cmdPerilDelete.Enabled = True
    '        Else
    '            cmdPerilDelete.Enabled = False
    '        End If
    '        If ((ViewRiskFlag <> True) And (Not m_bAlreadyEdited)) Then
    '            cmdPerilEdit.Enabled = True
    '        Else
    '            cmdPerilEdit.Enabled = False
    '        End If
    '    Else
    '        cmdPerilDelete.Enabled = False
    '        cmdPerilEdit.Enabled = False
    '    End If
    'End Sub


    '--------------------------------------------------------------------------
    'Start - Bug ID - 22& 23, Date 18th Oct 2000, Author: DG
    'Description :- Bug Id - 22,Peril list : Edit button is always enabled. Should only enable when peril highlighted
    '               Bug Id - 23, Peril list : Delete button always enabled :If trying to delete, gives message which is ambiguous.Does not allow deletion of peril just added.
    '
    ' To rectify the bug the following code has been commented.
    'Private Sub lvwPerils_Click()
    '    SetPerilButtons
    'End Sub
    'END - Bug ID - 22& 23, Date 18th Oct 2000, Author: DG
    '--------------------------------------------------------------------------------
    'Private Sub lvwPerils_DblClick()
    '    'Start - Bug ID - 22& 23, Date 18th Oct 2000, Author: DG
    '    'Description :- Bug Id - 22,Peril list : Edit button is always enabled. Should only enable when peril highlighted
    '    '               Bug Id - 23, Peril list : Delete button always enabled :If trying to delete, gives message which is ambiguous.Does not allow deletion of peril just added.
    '    '
    '    ' To rectify the bug the following code has been added.
    '    'END - Bug ID - 22& 23, Date 18th Oct 2000, Author: DG
    '     If lvwPerils.SelectedItem Is Nothing Then Exit Sub
    '     If cmdPerilEdit.Enabled = False Then Exit Sub
    '     cmdPerilEdit_Click
    'End Sub

    'Private Sub lvwPerils_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
    '    'Start - Bug ID - 22& 23, Date 18th Oct 2000, Author: DG
    '    'Description :- Bug Id - 22,Peril list : Edit button is always enabled. Should only enable when peril highlighted
    '    '               Bug Id - 23, Peril list : Delete button always enabled :If trying to delete, gives message which is ambiguous.Does not allow deletion of peril just added.
    '    '
    '    ' To rectify the bug the following code has been added.
    '    'END - Bug ID - 22& 23, Date 18th Oct 2000, Author: DG
    '
    '    If lvwPerils.HitTest(x, y) Is Nothing Then
    '        cmdPerilEdit.Enabled = False
    '        cmdPerilDelete.Enabled = False
    '    Else
    '        SetPerilButtons
    '    End If
    'End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
        With tabMainTab
            ' Set the default button.
            Application.DoEvents()

            '        ' Set focus to the first control on the tab.
            '        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
            '            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
            '        End If
        End With
        Exit Sub
        tabMainTabPreviousTab = tabMainTab.SelectedIndex
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'DC070601
            '   If Task <> PMView And ViewRiskFlag <> True Then
            If Not ViewRiskFlag Then
                ' Check mandatory controls have been entered into.
                m_lReturn = m_oFormFields.CheckMandatoryControls()
                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                m_lReturn = CheckForMandatoryFields()
                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                ' Process the next set of actions depending
                ' upon the interface task etc.
                SaveAllGeneralDetails()
                SaveComment()

                'DC070601 moved further down
                'End If

                'Start : Internal Bug Id - 21, Date- 21st Oct Author - Dipika Gadekar
                '       Added a functionality. On pressing OK, in Risk Details screen,
                '                             when the Sum(CurrentReserve) = 0 then a message will come up
                '                             asking the User wether the Claim can be closed.
                '                             If the reply is YES the Claim status is set to closed.

                CheckCurrentReserve()

                m_lReturn = SaveOtherParty()

                'End : Internal Bug Id - 21, Date- 21st Oct Author - Dipika Gadekar
                m_lReturn = m_oGeneral.ProcessCommand()

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If

                'DC070601 -start -hide interface if exit view only
            Else
                Me.Hide()
            End If
            'DC070601 -end

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'Start : Internal Bug Id - 21, Date- 21st Oct Author - Dipika Gadekar
    '       Added a functionality. On pressing OK, in Risk Details screen,
    '                             when the Sum(CurrentReserve) = 0 then a message will come up
    '                             asking the User wether the Claim can be closed.
    '                             If the reply is YES the Claim status is set to closed.
    Private Sub CheckCurrentReserve()

        Dim sTitle, sMessage As String
        Dim vdataArray(,) As Object
        'Const AC_COL_CLAIMPERIL_DESC As Integer = 3
        'Const AC_EVENT_TYPE_UPDATECLAIM As Integer = 6
        Dim sEventDescription As String = ""

        If Task <> gPMConstants.PMEComponentAction.PMEdit Then Exit Sub


        m_lReturn = m_oBusiness.GetCurrentReserveRecovery(Claimid, vdataArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vdataArray) Then

            sTitle = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACInvalidAction, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sMessage = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACFailedToGetCurrentReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

    End Sub
    'End : Internal Bug Id - 21, Date- 21st Oct Author - Dipika Gadekar

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim sTitle As String = ""
        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20021023 : Close the Party  Policy Summary Screen, if they
            '               are loaded
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            m_lReturn = ClosePartyPolicySummary()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(15/06/01) Shifted DeleteWorkClaim stuff from here to ProcessCommand.

                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If Not CheckForMandatoryFields() Then Exit Sub
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            SaveAllGeneralDetails()
            '    SaveParties m_lclaimid
            SaveComment()

            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_5.Click, _cmdNext_4.Click, _cmdNext_3.Click, _cmdNext_2.Click, _cmdNext_0.Click, _cmdNext_1.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)
        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch

            Exit Sub
        End Try

    End Sub



    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _Financial.Click, _Event.Click, _Party.Click, _Policy.Click, _Risk.Click, _DocArchive.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)
        Dim sOption, sSPUrl, sDocLIB As String
        Dim oFinancialSummary As iCLMFinSumm.Interface_Renamed
        If ViewRiskFlag Then Exit Sub

        Select Case (Button.Name)
            Case "_Financial"
                Dim temp_oFinancialSummary As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oFinancialSummary, sClassName:="iCLMFinSumm.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oFinancialSummary = temp_oFinancialSummary

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object '.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGenericRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                If oFinancialSummary Is Nothing Then Exit Sub

                'developer guide no.9
                oFinancialSummary.Initialise()

                oFinancialSummary.SetProcessModes(Task, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)

                oFinancialSummary.ClaimId = Claimid

                oFinancialSummary.Start()

                oFinancialSummary.Dispose()
                oFinancialSummary = Nothing
            Case "_Risk"
                ShowRiskDetails()
            Case "_Event"
                ShowRiskEvents()
            Case "_Party"
                ' RAM20021021 : NRMA Changes (Sirius Process No 126)
                m_lReturn = ShowPartySummaryDetails()
            Case "_Policy"
                ' RAM20021021 : NRMA Changes (Sirius Process No 126)
                m_lReturn = ShowPolicySummaryDetails()
                'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.5.1)
                'This button Doc Archive is newly added for implementing the document archive functionality
            Case "_DocArchive"
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

                If m_sTransactionType = ACTransactionType Then
                    If sOption = "1" Then
                        m_lReturn = iPMFunc.RunDocumaster(v_sLinkCode:=m_sClient.Trim() & "1")
                    ElseIf sOption = "2" Then
                        System.Diagnostics.Process.Start(sSPUrl & sDocLIB & "\" & m_sClient.Trim())
                    End If
                Else
                    If sOption = "1" Then
                        m_lReturn = iPMFunc.RunDocumaster(v_sLinkCode:=m_sClaimNumber.Trim() & "2")
                    ElseIf sOption = "2" Then
                        System.Diagnostics.Process.Start(sSPUrl & sDocLIB & "\" & m_sClient.Trim() & "\Claim\" & m_sClaimNumber.Trim())
                    End If
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
                'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.5.1)
        End Select
    End Sub
    Private Sub ShowRiskEvents()

        Dim lClaimId As Integer

        'On Error GoTo Err_ShowRiskDetails
        Try  ' RAM20021021 : Variable Fix

            'sj 02/10/2002 - start
            If Not m_bClientPolicyDetailsLoaded Then

                m_lReturn = m_oBusiness.GetClientPolicyDetails(v_lInsuranceFileCnt:=m_lPolicyId, r_lPartyCnt:=m_lPartyCnt, r_sPartyShortName:=m_sPartyShortName, r_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_sInsuranceRef:=m_sInsuranceRef)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskEvents")
                    Exit Sub
                End If

                m_bClientPolicyDetailsLoaded = True
            End If
            'sj 02/10/2002 - end

            'MSS260901 - Added switch for merge

            'TN20010824 - start

            If m_sTransactionType = "C_CO" Then
                lClaimId = 0 'we don't have any event for this claim yet
            Else
                'get original claim id

                m_lReturn = m_oBusiness.GetOriginalClaimID(v_lClaimId:=m_lclaimid, r_lOriginalClaimID:=lClaimId)
            End If
            'Developer Guide no.294
            SharedFiles.iPMBListEvents.g_oObjectManager = g_oObjectManager
            m_lReturn = SharedFiles.iPMBListEvents.ShowEvents(v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lPolicyId, v_lClaimCnt:=lClaimId, v_sInsuranceRef:=m_sInsuranceRef, v_sClaimRef:=m_sClaimNumber, v_sTransactionType:=m_sTransactionType, v_sSource_App:="")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskEvents")
                Exit Sub
            End If

        Catch

            'Err_ShowRiskDetails:
            ' RAM20021021 : Variable Fix
            'Continue as not serious
            Exit Sub
        End Try

    End Sub



    '*********************************************************************
    ' Created By: Jes 270303
    ' Description: To choose between two methods of risk screen launching
    '              to cater for post quote risk screens
    Private Sub ShowRiskDetails()

        Try
            'if UW then ShowRiskDetailsUW else ShowRiskDetailsBRO


            ShowRiskDetailsUW()

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRiskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try


    End Sub


    'Private Sub ShowRiskDetailsBRO()
    'Dim vResultArray As Object
    'Dim oObject As Object
    'Dim vKeyArray As Object
    '
    'Try 
    '
    ' BSJ 28/09/2004 - PN 15191, Risk details is displayed via iPMURiskWrapper which in turn
    ' is currently hard coded to display the iPMURisk screen in a modeless state, therefore we cannot
    ' rely on the 'switch to' method as we were previously and are better off killing and recreating the object as required
    'If m_oRisk Is Nothing Then
    ' Do nothing
    'Else
    'm_lReturn = m_oRisk.Terminate()
    'Clear it
    'm_oRisk = Nothing
    'End If
    '

    'm_lReturn = m_oBusiness.GetRiskDetails_U(v_lClaimId:=m_lclaimid, r_vResultArray:=vResultArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
    'MessageBox.Show("No risk screen exists for this policy record.", "Risk Details Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    'End If
    'Exit Sub
    'End If
    '
    'm_oRisk = New iPMURiskWrapper.Interface_Renamed()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Sub
    'End If
    '
    'm_lReturn = CType(m_oRisk, SSP.S4I.Interfaces.ILocalInterface).Initialise()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Sub
    'End If
    '
    'With m_oRisk
    '

    '.InsuranceFolderCnt = CInt(vResultArray(0, 0))

    '.InsuranceFileCnt = CInt(vResultArray(1, 0))

    '.ProductId = CInt(vResultArray(2, 0))

    '.RiskId = CInt(vResultArray(3, 0))

    '.RiskTypeId = CInt(vResultArray(4, 0))

    '.ScreenId = CInt(vResultArray(5, 0))
    '
    'm_lReturn = .Start()
    '
    'End With
    '
    'Catch excep As System.Exception
    '
    '
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRiskDetailsUW Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskDetailsBRO", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'End Try
    '
    'End Sub


    Private Sub ShowRiskDetailsUW()
        Dim vResultArray(,) As Object

        Dim oObject As iPMURisk.Interface_Renamed

        Try


            m_lReturn = m_oBusiness.GetRiskDetails_U(v_lClaimId:=m_lclaimid, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            'developer guide no.9
            m_lReturn = oObject.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oObject.Dispose()
                oObject = Nothing
                Exit Sub
            End If



            oObject.InsuranceFolderCnt = vResultArray(0, 0)


            oObject.InsuranceFileCnt = vResultArray(1, 0)


            oObject.ProductID = vResultArray(2, 0)


            oObject.RiskId = vResultArray(3, 0)


            oObject.RiskTypeId = vResultArray(4, 0)


            oObject.ScreenId = vResultArray(5, 0)


            m_lReturn = oObject.Start()


            oObject.Dispose()
            oObject = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRiskDetailsUW Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub


    ' PRIVATE Events (End)
    '=============================================================================
    Private Sub txtDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtDate_0.Enter
        Dim Index As Integer = Array.IndexOf(txtDate, eventSender)
        If Strings.Len(Convert.ToString(txtDate(Index).Tag)) > 0 Then

            If Not Information.IsDate(Convert.ToString(txtDate(Index).Tag)) Then
                txtDate(Index).Tag = DateTimeHelper.ToString(DateTime.Today)
                txtDate(Index).Text = DateTimeHelper.ToString(DateTime.Today)
            Else
                txtDate(Index).Text = DateTime.Parse(Convert.ToString(txtDate(Index).Tag)).ToString("d")
            End If
        Else
            txtDate(Index).Tag = DateTimeHelper.ToString(DateTime.Today)
            txtDate(Index).Text = DateTimeHelper.ToString(DateTime.Today)
        End If

        txtDate(Index).Text = DateTime.Parse(txtDate(Index).Text).ToString("d")
        txtDate(Index).Tag = DateTime.Parse(txtDate(Index).Text).ToString("d")

        If Strings.Len(txtDate(Index).Text) > 0 Then
            txtDate(Index).SelectionStart = 0
            txtDate(Index).SelectionLength = Strings.Len(txtDate(Index).Text)
        End If
    End Sub

    Private Sub txtDate_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles _txtDate_0.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If (KeyAscii < Strings.Asc("0"c) Or KeyAscii > Strings.Asc("9"c)) And (KeyAscii <> Strings.Asc("/"c)) And (KeyAscii <> CInt(Keys.Back)) Then
            KeyAscii = 0 ' Cancel the character.
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtDate_0.Leave
        Dim Index As Integer = Array.IndexOf(txtDate, eventSender)

        Dim sTitle As String = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
        ' Start: Bug ID - Code review, Date - 18th Oct Author- DG
        ' The following lines are added for resource strings

        Dim sMessage As String = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACInvalidDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
        ' End: Bug ID - Code review, Date - 19th Oct Author- DG
        If Strings.Len(txtDate(Index).Text) > 0 Then
            If Not Information.IsDate(txtDate(Index).Text) Then
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
                txtDate(Index).Text = DateTimeHelper.ToString(DateTime.Today)
            End If
        Else
            txtDate(Index).Text = DateTimeHelper.ToString(DateTime.Today)
        End If

        txtDate(Index).Tag = DateTime.Parse(txtDate(Index).Text).ToString("d")
        txtDate(Index).Text = DateTime.Parse(txtDate(Index).Text).ToString("D")
    End Sub

    Private Sub txtInteger_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtInteger_0.Enter
        Dim Index As Integer = Array.IndexOf(txtInteger, eventSender)
        If Strings.Len(txtInteger(Index).Text) > 0 Then
            txtInteger(Index).SelectionStart = 0
            txtInteger(Index).SelectionLength = Strings.Len(txtInteger(Index).Text)
        End If
    End Sub

    Private Sub txtInteger_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles _txtInteger_0.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim Index As Integer = Array.IndexOf(txtInteger, eventSender)
        If Strings.Len(txtInteger(Index).Text) >= ACLengthOfValue And (KeyAscii <> CInt(Keys.Back)) Then
            KeyAscii = 0 ' Cancel the character.
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtInteger_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtInteger_0.Leave
        Dim Index As Integer = Array.IndexOf(txtInteger, eventSender)
        Dim dbNumericTemp As Double
        If Not Double.TryParse(txtInteger(Index).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            txtInteger(Index).Text = ""
        Else
            txtInteger(Index).Text = txtInteger(Index).Text.Trim()
        End If

    End Sub

    Private Sub txtText_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtText_0.Enter
        Dim Index As Integer = Array.IndexOf(txtText, eventSender)
        If Strings.Len(txtText(Index).Text) > 0 Then
            txtText(Index).SelectionStart = 0
            txtText(Index).SelectionLength = Strings.Len(txtText(Index).Text)
        End If
    End Sub

    Private Sub txtText_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles _txtText_0.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim Index As Integer = Array.IndexOf(txtText, eventSender)
        If (KeyAscii < Strings.Asc("0"c) Or KeyAscii > Strings.Asc("9"c)) And (KeyAscii < Strings.Asc("a"c) Or KeyAscii > Strings.Asc("z"c)) And (KeyAscii < Strings.Asc("A"c) Or KeyAscii > Strings.Asc("Z"c)) And (KeyAscii <> CInt(Keys.Space)) And (KeyAscii <> Strings.Asc("."c)) And (KeyAscii <> CInt(Keys.Back)) Then
            KeyAscii = 0 ' Cancel the character.
        End If
        If Strings.Len(txtText(Index).Text) >= ACLengthOfValue And (KeyAscii <> CInt(Keys.Back)) Then
            KeyAscii = 0 ' Cancel the character.
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ' ***************************************************************** '
    ' Name: CheckForMandatoryFields
    '
    ' Description: Check if data in all the mandataory controls on General details
    '              tab are set. Check if the party is entered for
    '              mandatory party type
    '
    ' ***************************************************************** '
    Private Function CheckForMandatoryFields() As Integer
        Dim result As Integer = 0
        Dim bError As Boolean
        Dim sPartyType As String = ""

        Dim sTitle As String = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        ' Start: Bug ID - Code review, Date - 18th Oct Author- DG
        ' The following lines are added for resource strings

        Dim sMessage As String = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACPleaseAddPerils, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
        ' End: Bug ID - Code review, Date - 19th Oct Author- DG
        result = gPMConstants.PMEReturnCode.PMTrue
        Dim iText As Integer = 0
        Dim iDate As Integer = 0
        Dim iBool As Integer = 0
        Dim iInteger As Integer = 0
        Dim iLookup As Integer = 0

        If uctCLMPerilRT1.PerilCount <= 0 Then
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(g_vFieldData) Then Return result
        For iRow As Integer = g_vFieldData.GetLowerBound(1) To g_vFieldData.GetUpperBound(1)
            If CBool(g_vFieldData(ACCOL_Mandatory, iRow)) Or (CBool(g_vFieldData(ACCOL_read_only, iRow)) And m_sTransactionType = "C_CO") Then
                bError = False
                Select Case (g_vFieldData(ACCOL_type, iRow))
                    Case ACDataTypeText
                        If Strings.Len(txtText(iText).Text) = 0 Then
                            bError = True
                            '                    tabMainTab.Tab = 1
                            SSTabHelper.SetSelectedIndex(tabMainTab, ACGeneral)
                            If txtText(iText).Enabled Then
                                txtText(iText).Focus()
                            End If
                        End If
                        iText += 1
                    Case ACDataTypeInteger
                        If Strings.Len(txtInteger(iInteger).Text) = 0 Then
                            bError = True
                            '                    tabMainTab.Tab = 1
                            SSTabHelper.SetSelectedIndex(tabMainTab, ACGeneral)
                            If txtInteger(iInteger).Enabled Then
                                txtInteger(iInteger).Focus()
                            End If
                        End If
                        iInteger += 1
                    Case ACDataTypeDate
                        If Strings.Len(txtDate(iDate).Text) = 0 Then
                            bError = True
                            '                    tabMainTab.Tab = 1
                            SSTabHelper.SetSelectedIndex(tabMainTab, ACGeneral)
                        End If
                        If txtDate(iDate).Enabled Then
                            txtDate_Enter(txtDate(iDate), New EventArgs())
                            txtDate(iDate).Focus()
                        End If
                        iDate += 1
                    Case ACDataTypeBoolean
                        If chkCheck(iBool).CheckState = CheckState.Unchecked Then
                            bError = True
                            '                    tabMainTab.Tab = 1
                            SSTabHelper.SetSelectedIndex(tabMainTab, ACGeneral)
                        End If
                        If chkCheck(iBool).Enabled Then
                            chkCheck(iBool).Focus()
                        End If
                        iBool += 1
                    Case ACDataTypeLookup
                        If cmbLookup(iLookup).SelectedIndex = -1 Then
                            bError = True
                            '                    tabMainTab.Tab = 1
                            SSTabHelper.SetSelectedIndex(tabMainTab, ACGeneral)
                        End If
                        If cmbLookup(iLookup).Enabled Then
                            cmbLookup(iLookup).Focus()
                        End If
                        iLookup += 1
                End Select
                If bError Then
                    ' Start: Bug ID - Code review, Date - 18th Oct Author- DG
                    ' The following lines are added for resource strings

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACEnterValues, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    ' End: Bug ID - Code review, Date - 18th Oct Author- DG
                    MessageBox.Show(sMessage & New String(" "c, 1) & lblLabel(iRow).Text.ToLower() & ".", sTitle, MessageBoxButtons.OK)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                Select Case (g_vFieldData(ACCOL_type, iRow))
                    Case ACDataTypeText
                        iText += 1
                    Case ACDataTypeInteger
                        iInteger += 1
                    Case ACDataTypeDate
                        iDate += 1
                    Case ACDataTypeBoolean
                        iBool += 1
                    Case ACDataTypeLookup
                        iLookup += 1
                End Select
            End If
        Next iRow

        If SSTabHelper.GetTabVisible(tabMainTab, ACDriver) Then
            If CBool(Convert.ToString(uctDriver.Tag)) Then
                If uctDriver.PartyCount = 0 Then

                    sPartyType = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACDriverDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    SSTabHelper.SetSelectedIndex(tabMainTab, ACDriver)


                    MessageBox.Show(CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACEnterParties, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & " " & sPartyType & ".", sTitle, MessageBoxButtons.OK)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If SSTabHelper.GetTabVisible(tabMainTab, ACThirdParty) Then
            If CBool(Convert.ToString(uctThirdParty.Tag)) Then
                If uctThirdParty.PartyCount = 0 Then

                    sPartyType = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACThirdPartyDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    SSTabHelper.SetSelectedIndex(tabMainTab, ACThirdParty)


                    MessageBox.Show(CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACEnterParties, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & " " & sPartyType & ".", sTitle, MessageBoxButtons.OK)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If SSTabHelper.GetTabVisible(tabMainTab, ACRepairer) Then
            If CBool(Convert.ToString(uctRepairer.Tag)) Then
                If uctRepairer.PartyCount = 0 Then

                    sPartyType = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACRepairerDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    SSTabHelper.SetSelectedIndex(tabMainTab, ACRepairer)


                    MessageBox.Show(CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACEnterParties, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & " " & sPartyType & ".", sTitle, MessageBoxButtons.OK)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If SSTabHelper.GetTabVisible(tabMainTab, ACWitness) Then
            If CBool(Convert.ToString(uctWitness.Tag)) Then
                If uctWitness.PartyCount = 0 Then

                    sPartyType = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACWitnessDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    SSTabHelper.SetSelectedIndex(tabMainTab, ACWitness)


                    MessageBox.Show(CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACEnterParties, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & " " & sPartyType & ".", sTitle, MessageBoxButtons.OK)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: SetInterface
    '
    ' Description: Set the interface with all the controls on General details
    '              tab required for the Risk types. Sets the party type
    '              and comment tab.
    '
    ' 21 June 2001 - change the load (text,integer...) function to load
    '                new control if we have data.
    '                for some reason control.visibility is still false
    '                after we set it to true so next control will not
    '                be loaded
    ' ***************************************************************** '

    Private Sub SetInterface()
        Dim NewLargeChange As Integer
        Dim lResults As gPMConstants.PMEReturnCode
        Dim sTemp As String = ""
        Dim vResultArray(,) As Object
        Dim iCurrentTab As Integer
        Dim lTop As Integer
        Dim TabCountDisplay As Integer
        Dim collLookupID As Collection
        Dim vdataArray(,) As Object
        Dim lClaimId As Integer
        Dim k, iTabIndex As Integer
        Const ACTabIndex As Integer = 22
        Dim lTab As Integer
        Try
            'DC150302
            Dim sGeneralTabName As String = ""
            Dim vDescription As Object

            'DC150302
            Dim lngLastTabID, lngCurrentTabID As Integer
            Dim intCurrentTab, intTextCount, intIntCount, intDateCount, intBoolCount, intLookupCount As Integer

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Debug.WriteLine(uctWitness.Visible)
            iTabIndex = ACTabIndex
            collLookupID = New Collection()
            m_vLookupDataArray = Nothing

            'DC150302
            sGeneralTabName = ""


            ' Set General Details
            lClaimId = m_lclaimid


            lResults = m_oBusiness.GetFieldsForRiskDataDefn(RiskType, Claimid, vResultArray)

            If lResults <> gPMConstants.PMEReturnCode.PMTrue Then
                '        tabMainTab.TabVisible(1) = False
                SSTabHelper.SetTabVisible(tabMainTab, 2, False)
                TabCountDisplay = 2

                g_vFieldData = Nothing
            End If

            For i As Integer = 6 To 10
                SSTabHelper.SetTabVisible(tabMainTab, i, False)
            Next i

            If lResults = gPMConstants.PMEReturnCode.PMTrue Then
                '        tabMainTab.Tab = 1
                'tabMainTab.Tab = ACGeneral
                g_vFieldData = VB6.CopyArray(vResultArray)
                lTop = 120

                For i As Integer = 0 To 4
                    Picture1(i).Top = VB6.TwipsToPixelsY(480)
                    Picture1(i).Left = VB6.TwipsToPixelsX(ACFormBorder)
                    Picture1(i).Width = tabMainTab.Width - VB6.TwipsToPixelsX(ACFormBorder) - Picture1(i).Left
                    Picture1(i).Height = tabMainTab.Height - VB6.TwipsToPixelsY(ACFormBorder) - Picture1(i).Top
                    VScroll1(i).Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Picture1(i).Width) - VB6.PixelsToTwipsX(VScroll1(i).Width) - 60)
                    VScroll1(i).Top = 0
                    VScroll1(i).Height = Picture1(i).Height - VB6.TwipsToPixelsY(60)
                Next

                lngLastTabID = -1
                ' Load General details
                intCurrentTab = 5



                For l As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    'DC150302
                    If CInt(vResultArray(ACCOL_type, l)) <> 7 Then
                        lngCurrentTabID = CInt(vResultArray(ACCOL_Claim_Tab_ID, l))
                        If lngCurrentTabID <> lngLastTabID Then
                            intCurrentTab += 1
                            SSTabHelper.SetTabVisible(tabMainTab, intCurrentTab, True)
                            SSTabHelper.SetSelectedIndex(tabMainTab, intCurrentTab)
                            'DC030703 -ISS4415 -removed as dealt with later on
                            'tabMainTab.TabCaption(intCurrentTab) = vResultArray(ACCOL_Tab_Caption, l)
                            lTop = 120

                        End If
                    End If
                    If CInt(vResultArray(ACCOL_type, l)) <> 7 Then

                        If lblLabel(lblLabel.GetUpperBound(0)).Visible Then
                            ContainerHelper.LoadControl(Me, "lblLabel", lblLabel.GetUpperBound(0) + 1)
                        End If
                        lblLabel(lblLabel.GetUpperBound(0)).Parent = Picture2(intCurrentTab - 6)

                        If CBool(vResultArray(ACCOL_Mandatory, l)) Or (CBool(vResultArray(ACCOL_read_only, l)) And m_sTransactionType = "C_CO") Then
                            lblLabel(lblLabel.GetUpperBound(0)).Font = VB6.FontChangeBold(lblLabel(lblLabel.GetUpperBound(0)).Font, True)
                        Else
                            lblLabel(lblLabel.GetUpperBound(0)).Font = VB6.FontChangeBold(lblLabel(lblLabel.GetUpperBound(0)).Font, False)
                        End If
                        lblLabel(lblLabel.GetUpperBound(0)).Visible = True
                        lblLabel(lblLabel.GetUpperBound(0)).Text = CStr(vResultArray(ACCOL_Caption, l))


                        'START       : Bug ID -28 Internal Bug, Date- 24th Oct., Author-DG
                        ' Colon at the end of the label
                        If Mid(CStr(vResultArray(ACCOL_Caption, l)).Trim(), Strings.Len(CStr(vResultArray(ACCOL_Caption, l))), 1) <> ":" Then
                            lblLabel(lblLabel.GetUpperBound(0)).Text = lblLabel(lblLabel.GetUpperBound(0)).Text & ":"
                        End If
                        'End      : Bug ID -2528 Internal Bug, Date- 24th Oct., Author-DG

                        iTabIndex += 1

                    End If
                    'DC150302
                    Select Case CInt(vResultArray(ACCOL_type, l))
                        Case ACDataTypeText
                            SSTabHelper.SetSelectedIndex(tabMainTab, intCurrentTab)
                            LoadText(lTop, intCurrentTab, intTextCount)
                            intTextCount += 1
                            SSTabHelper.SetSelectedIndex(tabMainTab, intCurrentTab)
                            txtText(txtText.GetUpperBound(0)).Parent = Picture2(intCurrentTab - 6)


                            m_lReturn = m_oBusiness.GetDataForRiskDataDefn(vResultArray(ACCOL_risk_data_defn_id, l), lClaimId, vdataArray)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                                vResultArray(ACCOL_claim_user_defined_risk_data_id, l) = vdataArray(0, 0)

                                vResultArray(ACCOL_Value, l) = vdataArray(1, 0)


                                txtText(txtText.GetUpperBound(0)).Text = CStr(vdataArray(1, 0))
                            End If

                            txtText(txtText.GetUpperBound(0)).Tag = CStr(vResultArray(ACCOL_risk_data_defn_id, l))
                            If CBool(vResultArray(ACCOL_read_only, l)) Then
                                If m_sTransactionType <> "C_CO" Then
                                    txtText(txtText.GetUpperBound(0)).Enabled = False
                                End If
                            Else

                                txtText(txtText.GetUpperBound(0)).Enabled = Not ViewRiskFlag And Task <> gPMConstants.PMEComponentAction.PMView

                                If m_sTransactionType = "C_CR" Then
                                    If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                        txtText(txtText.GetUpperBound(0)).Enabled = False
                                    End If
                                End If

                            End If
                            txtText(txtText.GetUpperBound(0)).TabIndex = iTabIndex
                        Case ACDataTypeInteger
                            SSTabHelper.SetSelectedIndex(tabMainTab, intCurrentTab)
                            LoadInteger(lTop, intCurrentTab, intIntCount)
                            intIntCount += 1
                            SSTabHelper.SetSelectedIndex(tabMainTab, intCurrentTab)
                            txtInteger(txtInteger.GetUpperBound(0)).Parent = Picture2(intCurrentTab - 6)

                            m_lReturn = m_oBusiness.GetDataForRiskDataDefn(vResultArray(ACCOL_risk_data_defn_id, l), lClaimId, vdataArray)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                                vResultArray(ACCOL_claim_user_defined_risk_data_id, l) = vdataArray(0, 0)

                                vResultArray(ACCOL_Value, l) = vdataArray(1, 0)

                                Dim dbNumericTemp As Double
                                If Double.TryParse(CStr(vdataArray(1, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then


                                    txtInteger(txtInteger.GetUpperBound(0)).Text = CStr(vdataArray(1, 0))
                                Else
                                    txtInteger(txtInteger.GetUpperBound(0)).Text = ""
                                End If
                            End If
                            txtInteger(txtInteger.GetUpperBound(0)).Tag = CStr(vResultArray(ACCOL_risk_data_defn_id, l))

                            If CBool(vResultArray(ACCOL_read_only, l)) Then
                                If m_sTransactionType <> "C_CO" Then
                                    txtInteger(txtInteger.GetUpperBound(0)).Enabled = False
                                End If
                            Else

                                txtInteger(txtInteger.GetUpperBound(0)).Enabled = Not ViewRiskFlag And Task <> gPMConstants.PMEComponentAction.PMView


                                If m_sTransactionType = "C_CR" Then
                                    If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                        txtInteger(txtInteger.GetUpperBound(0)).Enabled = False
                                    End If
                                End If

                            End If
                            txtInteger(txtInteger.GetUpperBound(0)).TabIndex = iTabIndex
                        Case ACDataTypeDate
                            SSTabHelper.SetSelectedIndex(tabMainTab, intCurrentTab)
                            LoadDate(lTop, intCurrentTab, intDateCount)
                            intDateCount += 1
                            SSTabHelper.SetSelectedIndex(tabMainTab, intCurrentTab)
                            txtDate(txtDate.GetUpperBound(0)).Parent = Picture2(intCurrentTab - 6)

                            m_lReturn = m_oBusiness.GetDataForRiskDataDefn(vResultArray(ACCOL_risk_data_defn_id, l), lClaimId, vdataArray)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                                vResultArray(ACCOL_claim_user_defined_risk_data_id, l) = vdataArray(0, 0)

                                vResultArray(ACCOL_Value, l) = vdataArray(1, 0)
                                If Information.IsDate(vdataArray(1, 0)) Then


                                    txtDate(txtDate.GetUpperBound(0)).Text = CStr(vdataArray(1, 0))
                                    txtDate_Leave(txtDate((txtDate.GetUpperBound(0))), New EventArgs())
                                Else
                                    txtDate(txtDate.GetUpperBound(0)).Text = ""
                                End If
                            End If
                            collRiskDataDefnForDates.Add(vResultArray(ACCOL_risk_data_defn_id, l))

                            If CBool(vResultArray(ACCOL_read_only, l)) Then
                                If m_sTransactionType <> "C_CO" Then
                                    txtDate(txtDate.GetUpperBound(0)).Enabled = False
                                End If
                            Else

                                txtDate(txtDate.GetUpperBound(0)).Enabled = Not ViewRiskFlag And Task <> gPMConstants.PMEComponentAction.PMView


                                If m_sTransactionType = "C_CR" Then
                                    If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                        txtDate(txtDate.GetUpperBound(0)).Enabled = False
                                    End If
                                End If


                            End If
                            txtDate(txtDate.GetUpperBound(0)).TabIndex = iTabIndex
                        Case ACDataTypeBoolean
                            SSTabHelper.SetSelectedIndex(tabMainTab, intCurrentTab)
                            LoadBoolean(lTop, intCurrentTab, intBoolCount)
                            intBoolCount += 1
                            chkCheck(chkCheck.GetUpperBound(0)).Parent = Picture2(intCurrentTab - 6)

                            m_lReturn = m_oBusiness.GetDataForRiskDataDefn(vResultArray(ACCOL_risk_data_defn_id, l), lClaimId, vdataArray)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                                vResultArray(ACCOL_claim_user_defined_risk_data_id, l) = vdataArray(0, 0)

                                vResultArray(ACCOL_Value, l) = vdataArray(1, 0)


                                chkCheck(chkCheck.GetUpperBound(0)).CheckState = vdataArray(1, 0)
                            End If
                            chkCheck(chkCheck.GetUpperBound(0)).Tag = CStr(vResultArray(ACCOL_risk_data_defn_id, l))
                            If CBool(vResultArray(ACCOL_read_only, l)) Then
                                If m_sTransactionType <> "C_CO" Then
                                    chkCheck(chkCheck.GetUpperBound(0)).Enabled = False
                                End If
                            Else

                                If Not ViewRiskFlag And Task <> gPMConstants.PMEComponentAction.PMView Then
                                    chkCheck(chkCheck.GetUpperBound(0)).Enabled = True
                                End If


                                If m_sTransactionType = "C_CR" Then
                                    If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                        chkCheck(chkCheck.GetUpperBound(0)).Enabled = False
                                    End If
                                End If


                            End If
                            chkCheck(chkCheck.GetUpperBound(0)).TabIndex = iTabIndex
                        Case ACDataTypeLookup
                            SSTabHelper.SetSelectedIndex(tabMainTab, intCurrentTab)
                            LoadLookup(lTop, intCurrentTab, intLookupCount)
                            intLookupCount += 1
                            cmbLookup(cmbLookup.GetUpperBound(0)).Parent = Picture2(intCurrentTab - 6)
                            cmbLookup(cmbLookup.GetUpperBound(0)).Tag = CStr(vResultArray(ACCOL_Claim_Lookup_id, l))

                            If Not Information.IsArray(m_vLookupDataArray) Then
                                ReDim m_vLookupDataArray(2, 0)
                                k = 0
                            Else
                                k = m_vLookupDataArray.GetUpperBound(1) + 1
                                ReDim Preserve m_vLookupDataArray(2, k)
                            End If
                            m_vLookupDataArray(0, k) = vResultArray(ACCOL_Claim_Lookup_id, l)
                            m_vLookupDataArray(1, k) = vResultArray(ACCOL_risk_data_defn_id, l)
                            collLookupID.Add(vResultArray(ACCOL_Claim_Lookup_id, l))

                            m_lReturn = m_oBusiness.GetDataForRiskDataDefn(vResultArray(ACCOL_risk_data_defn_id, l), lClaimId, vdataArray)

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                                vResultArray(ACCOL_claim_user_defined_risk_data_id, l) = vdataArray(0, 0)

                                vResultArray(ACCOL_Value, l) = vdataArray(1, 0)

                                m_vLookupDataArray(2, k) = vdataArray(1, 0) 'Value
                            End If

                            If CBool(vResultArray(ACCOL_read_only, l)) Then
                                If m_sTransactionType <> "C_CO" Then
                                    cmbLookup(cmbLookup.GetUpperBound(0)).Enabled = False
                                End If
                            Else

                                cmbLookup(cmbLookup.GetUpperBound(0)).Enabled = Not ViewRiskFlag And Task <> gPMConstants.PMEComponentAction.PMView


                                If m_sTransactionType = "C_CR" Then
                                    If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                        cmbLookup(cmbLookup.GetUpperBound(0)).Enabled = False
                                    End If
                                End If


                            End If
                            cmbLookup(cmbLookup.GetUpperBound(0)).TabIndex = iTabIndex
                            'MSS260901 - Added for merge
                            'AK 080801 - for getting list from frontoffice database
                        Case ACDataTypeGEMLookup

                            'MSS260901 - Merge end
                            'DC150302 rename to one set by user
                        Case ACDataTypeTabName

                            'DC030703 -ISS4415 -was sGeneralTabName
                            m_sGeneralTabName = CStr(vResultArray(ACCOL_Description, l))

                            lTop -= ACNormalGapBetweenTopsOfTwoTextBoxes
                            'DC150302

                    End Select
                    lTop += ACNormalGapBetweenTopsOfTwoTextBoxes

                    'Keep this to see if the tab has changed
                    lngLastTabID = lngCurrentTabID

                    'DC030703 -ISS4415 -do not readjust if no more than 5 tabs
                    If intCurrentTab > 5 Then

                        'Set the correct picture height for how many controls in the current tab
                        Picture2(intCurrentTab - 6).Height = VB6.TwipsToPixelsY(lTop + 315)

                        VScroll1(intCurrentTab - 6).Maximum = (100 + VScroll1(intCurrentTab - 6).LargeChange - 1)
                        VScroll1(intCurrentTab - 6).Minimum = 0
                        VScroll1(intCurrentTab - 6).SmallChange = 1
                        NewLargeChange = (VScroll1(intCurrentTab - 6).Maximum - (VScroll1(intCurrentTab - 6).LargeChange + 1))
                        VScroll1(intCurrentTab - 6).Maximum = VScroll1(intCurrentTab - 6).Maximum + NewLargeChange - VScroll1(intCurrentTab - 6).LargeChange
                        VScroll1(intCurrentTab - 6).LargeChange = NewLargeChange

                        VScroll1(intCurrentTab - 6).Visible = Not (VB6.PixelsToTwipsY(Picture2(intCurrentTab - 6).Height) < VB6.PixelsToTwipsY(Picture1(intCurrentTab - 6).Height))

                    End If

                Next l

                If collLookupID.Count > 0 Then
                    PopulateComboBoxes(collLookupID)
                End If
                ' Party Screen
                '        Picture2.Height = lTop + 315
                '
                '        VScroll1.Max = 100
                '        VScroll1.Min = 0
                '        VScroll1.SmallChange = 1
                '        VScroll1.LargeChange = VScroll1.Max
                '
                '        If Picture2.Height < Picture1.Height Then
                '            VScroll1.Visible = False
                '        Else
                '            VScroll1.Visible = True
                '        End If
                TabCountDisplay = 3
            End If

            'Not convinced about this, but leave it in for now...
            iCurrentTab = 2
            ' Get the Parties.

            lResults = m_oBusiness.GetPartyTypesforRiskType(RiskType, vResultArray)

            If lResults = gPMConstants.PMEReturnCode.PMTrue And Information.IsArray(vResultArray) Then
                For l As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                    Select Case CStr(vResultArray(3, l)).Trim()
                        Case "OTDRIVER"
                            SSTabHelper.SetTabVisible(tabMainTab, ACDriver, True)
                            uctDriver.ClaimId = m_lclaimid
                            uctDriver.RiskTypeId = m_lRisk
                            uctDriver.PerilTypeId = 0
                            uctDriver.PartyType = CInt(vResultArray(0, l))
                            uctDriver.PartyTypeCode = CStr(vResultArray(3, l)).Trim()

                            'Mandatory
                            If CBool(vResultArray(1, l)) Then
                                uctDriver.Tag = CStr(1)
                            ElseIf Not CBool(vResultArray(1, l)) Then
                                uctDriver.Tag = CStr(0)
                            Else
                                uctDriver.Tag = CStr(vResultArray(1, l))
                            End If

                            'ReadOnly
                            If Not CBool(vResultArray(2, l)) Then
                                If Not ViewRiskFlag Then
                                    uctDriver.Task = gPMConstants.PMEComponentAction.PMEdit
                                Else
                                    uctDriver.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            Else
                                If m_sTransactionType = "C_CO" Then
                                    uctDriver.Tag = CStr(1)
                                    uctDriver.Task = gPMConstants.PMEComponentAction.PMEdit
                                Else
                                    uctDriver.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            End If


                            If m_sTransactionType = "C_CR" Then
                                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                    uctDriver.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            End If

                            'developer guide no.9
                            m_lReturn = uctDriver.Initialise()

                            m_lReturn = uctDriver.LoadControl()

                            m_lReturn = uctDriver.GetParties()

                        Case "OTTHIRD"
                            SSTabHelper.SetTabVisible(tabMainTab, ACThirdParty, True)
                            uctThirdParty.ClaimId = m_lclaimid
                            uctThirdParty.RiskTypeId = m_lRisk
                            uctThirdParty.PerilTypeId = 0
                            uctThirdParty.PartyType = CInt(vResultArray(0, l))
                            uctThirdParty.PartyTypeCode = CStr(vResultArray(3, l)).Trim()

                            'Mandatory
                            If CBool(vResultArray(1, l)) Then
                                uctThirdParty.Tag = CStr(1)
                            ElseIf Not CBool(vResultArray(1, l)) Then
                                uctThirdParty.Tag = CStr(0)
                            Else
                                uctThirdParty.Tag = CStr(vResultArray(1, l))
                            End If

                            'ReadOnly
                            If Not CBool(vResultArray(2, l)) Then
                                If Not ViewRiskFlag Then
                                    uctThirdParty.Task = gPMConstants.PMEComponentAction.PMEdit
                                Else
                                    uctThirdParty.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            Else
                                If m_sTransactionType = "C_CO" Then
                                    uctThirdParty.Tag = CStr(1)
                                    uctThirdParty.Task = gPMConstants.PMEComponentAction.PMEdit
                                Else
                                    uctThirdParty.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            End If


                            If m_sTransactionType = "C_CR" Then
                                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                    uctThirdParty.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            End If

                            'developer guide no.9
                            m_lReturn = uctThirdParty.Initialise()

                            m_lReturn = uctThirdParty.LoadControl()

                            m_lReturn = uctThirdParty.GetParties()

                        Case "OTREPAIRER"
                            SSTabHelper.SetTabVisible(tabMainTab, ACRepairer, True)
                            uctRepairer.ClaimId = m_lclaimid
                            uctRepairer.RiskTypeId = m_lRisk
                            uctRepairer.PerilTypeId = 0
                            uctRepairer.PartyType = CInt(vResultArray(0, l))
                            uctRepairer.PartyTypeCode = CStr(vResultArray(3, l)).Trim()

                            'Mandatory
                            If CBool(vResultArray(1, l)) Then
                                uctRepairer.Tag = CStr(1)
                            ElseIf Not CBool(vResultArray(1, l)) Then
                                uctRepairer.Tag = CStr(0)
                            Else
                                uctRepairer.Tag = CStr(vResultArray(1, l))
                            End If

                            'ReadOnly
                            If Not CBool(vResultArray(2, l)) Then
                                If Not ViewRiskFlag Then
                                    uctRepairer.Task = gPMConstants.PMEComponentAction.PMEdit
                                Else
                                    uctRepairer.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            Else
                                If m_sTransactionType = "C_CO" Then
                                    uctRepairer.Tag = CStr(1)
                                    uctRepairer.Task = gPMConstants.PMEComponentAction.PMEdit
                                Else
                                    uctRepairer.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            End If


                            If m_sTransactionType = "C_CR" Then
                                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                    uctRepairer.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            End If

                            'developer guide no.9
                            m_lReturn = uctRepairer.Initialise()

                            m_lReturn = uctRepairer.LoadControl()

                            m_lReturn = uctRepairer.GetParties()

                        Case "OTWITNESS"
                            SSTabHelper.SetTabVisible(tabMainTab, ACWitness, True)
                            uctWitness.ClaimId = m_lclaimid
                            uctWitness.RiskTypeId = m_lRisk
                            uctWitness.PerilTypeId = 0
                            uctWitness.PartyType = CInt(vResultArray(0, l))
                            uctWitness.PartyTypeCode = CStr(vResultArray(3, l)).Trim()

                            'Mandatory
                            If CBool(vResultArray(1, l)) Then
                                uctWitness.Tag = CStr(1)
                            ElseIf Not CBool(vResultArray(1, l)) Then
                                uctWitness.Tag = CStr(0)
                            Else
                                uctWitness.Tag = CStr(vResultArray(1, l))
                            End If

                            'ReadOnly
                            If Not CBool(vResultArray(2, l)) Then
                                If Not ViewRiskFlag Then
                                    uctWitness.Task = gPMConstants.PMEComponentAction.PMEdit
                                Else
                                    uctWitness.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            Else
                                If m_sTransactionType = "C_CO" Then
                                    uctWitness.Tag = CStr(1)
                                    uctWitness.Task = gPMConstants.PMEComponentAction.PMEdit
                                Else
                                    uctWitness.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            End If


                            If m_sTransactionType = "C_CR" Then
                                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                    uctWitness.Task = gPMConstants.PMEComponentAction.PMView
                                End If
                            End If

                            'developer guide no.9
                            m_lReturn = uctWitness.Initialise()

                            m_lReturn = uctWitness.LoadControl()

                            m_lReturn = uctWitness.GetParties()

                    End Select

                Next l
            End If

            '    tabMainTab.Tabs = tabMainTab.Tabs + 1
            '    tabMainTab.Tab = tabMainTab.Tabs - 1

            vResultArray = Nothing
            '    Set txtComment(0).Container = tabMainTab

            'S4B Claim Enhancements R&D 2005

            txtComment(0).Visible = True


            If Not ViewRiskFlag Then

                'DC240402 -Start -added ClaimRiskDescription and set up multi line comments

                m_lReturn = m_oBusiness.GetCommentsForClaim(Claimid, RiskType, vDescription, vResultArray)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    txtComment(0).Text = CStr(vResultArray(0, 0))
                    ClaimRiskDesciption = CStr(vResultArray(1, 0))
                End If


            End If



            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            ' load peril data
            'developer guide no.9
            uctCLMPerilRT1.Initialise()
            uctCLMPerilRT1.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            uctCLMPerilRT1.Claimid = m_lclaimid
            uctCLMPerilRT1.Risk = m_lRisk
            uctCLMPerilRT1.ClaimMode = m_lClaimMode
            uctCLMPerilRT1.Policy = m_lPolicyId
            uctCLMPerilRT1.IsOpenClaimNoTrans = m_bOpenClaimNoTrans
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
            uctCLMPerilRT1.ScreenCaption = m_sScreenCaption
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
            uctCLMPerilRT1.LoadControl()


            'LoadPerilData
            collLookupID = Nothing
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            lTab = 2
            For l As Integer = 2 To 6
                If SSTabHelper.GetTabVisible(tabMainTab, l) Then
                    lTab += 1
                    Select Case l
                        Case ACDriver
                            SSTabHelper.SetTabCaption(tabMainTab, l, "&" & lTab & " - Driver")
                        Case ACThirdParty
                            SSTabHelper.SetTabCaption(tabMainTab, l, "&" & lTab & " - Third Party")
                        Case ACRepairer
                            SSTabHelper.SetTabCaption(tabMainTab, l, "&" & lTab & " - Repairer")
                        Case ACWitness
                            SSTabHelper.SetTabCaption(tabMainTab, l, "&" & lTab & " - Witness")
                        Case ACGeneral
                            'DC150302
                            If m_sGeneralTabName = "" Then
                                SSTabHelper.SetTabCaption(tabMainTab, l, "&" & lTab & " - General Details")
                            Else
                                'DC030703 -ISS4415 -insert hyphen to keep in line with default general details
                                SSTabHelper.SetTabCaption(tabMainTab, l, "&" & lTab & " - " & m_sGeneralTabName)
                            End If
                    End Select
                End If
            Next l

        Catch excep As System.Exception


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub


        End Try


    End Sub



    ' ***************************************************************** '
    ' Name: LoadBoolean
    '
    ' Description: Add a new check box control to the general details tab
    '               at a given height. Set its label.
    '
    ' ***************************************************************** '

    Private Sub LoadBoolean(ByRef lTop As Integer, ByRef iCurrentTab As Integer, ByVal v_lCount As Integer)

        SSTabHelper.SetSelectedIndex(tabMainTab, iCurrentTab)

        'If chkCheck(chkCheck.UBound).Visible = True Then
        If v_lCount > 0 Then
            ContainerHelper.LoadControl(Me, "chkCheck", chkCheck.GetUpperBound(0) + 1)
        End If
        'End If

        chkCheck(chkCheck.GetUpperBound(0)).Visible = True
        lblLabel(lblLabel.GetUpperBound(0)).Visible = True

        chkCheck(chkCheck.GetUpperBound(0)).Top = VB6.TwipsToPixelsY(lTop)
        lblLabel(lblLabel.GetUpperBound(0)).Top = VB6.TwipsToPixelsY(ACDiffBetweenTopsOfLabelAndText + lTop)

        lblLabel(lblLabel.GetUpperBound(0)).Width = VB6.TwipsToPixelsX(ACLabelWidthLong)

        chkCheck(chkCheck.GetUpperBound(0)).Height = VB6.TwipsToPixelsY(ACTextBoxHeight)
        lblLabel(lblLabel.GetUpperBound(0)).Height = VB6.TwipsToPixelsY(ACLabelHeight)

        lblLabel(lblLabel.GetUpperBound(0)).Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn)
        chkCheck(chkCheck.GetUpperBound(0)).Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn + ACLabelWidthLong + ACFormBorder)
    End Sub

    ' ***************************************************************** '
    ' Name: LoadText
    '
    ' Description: Add a new textbox control to the general details tab
    '               at a given height. Set its label.
    '
    ' ***************************************************************** '

    Private Sub LoadText(ByRef lTop As Integer, ByRef iCurrentTab As Integer, ByVal v_lCount As Integer)

        SSTabHelper.SetSelectedIndex(tabMainTab, iCurrentTab)

        'If txtText(txtText.UBound).Visible = True Then
        If v_lCount > 0 Then
            ContainerHelper.LoadControl(Me, "txtText", txtText.GetUpperBound(0) + 1)
        End If
        'End If
        txtText(txtText.GetUpperBound(0)).Text = ""
        txtText(txtText.GetUpperBound(0)).Visible = True
        lblLabel(lblLabel.GetUpperBound(0)).Visible = True

        txtText(txtText.GetUpperBound(0)).Top = VB6.TwipsToPixelsY(lTop)
        lblLabel(lblLabel.GetUpperBound(0)).Top = VB6.TwipsToPixelsY(ACDiffBetweenTopsOfLabelAndText + lTop)

        'txtText(txtText.UBound).Width = ACTextBoxHeight
        lblLabel(lblLabel.GetUpperBound(0)).Width = VB6.TwipsToPixelsX(ACLabelWidthLong)

        txtText(txtText.GetUpperBound(0)).Height = VB6.TwipsToPixelsY(ACTextBoxHeight)
        lblLabel(lblLabel.GetUpperBound(0)).Height = VB6.TwipsToPixelsY(ACLabelHeight)

        lblLabel(lblLabel.GetUpperBound(0)).Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn)
        txtText(txtText.GetUpperBound(0)).Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn + ACLabelWidthLong + ACFormBorder)
    End Sub

    ' ***************************************************************** '
    ' Name: LoadInteger
    '
    ' Description: Add a new integer textbox control to the general details tab
    '               at a given height. Set its label.
    '
    ' ***************************************************************** '
    Private Sub LoadInteger(ByRef lTop As Integer, ByRef iCurrentTab As Integer, ByVal v_lCount As Integer)

        SSTabHelper.SetSelectedIndex(tabMainTab, iCurrentTab)

        'If txtInteger(txtInteger.UBound).Visible = True Then
        If v_lCount > 0 Then
            ContainerHelper.LoadControl(Me, "txtInteger", txtInteger.GetUpperBound(0) + 1)
        End If
        'End If

        txtInteger(txtInteger.GetUpperBound(0)).Visible = True
        lblLabel(lblLabel.GetUpperBound(0)).Visible = True
        txtInteger(txtInteger.GetUpperBound(0)).Text = ""
        txtInteger(txtInteger.GetUpperBound(0)).Top = VB6.TwipsToPixelsY(lTop)
        lblLabel(lblLabel.GetUpperBound(0)).Top = VB6.TwipsToPixelsY(ACDiffBetweenTopsOfLabelAndText + lTop)

        'txtInteger(txtInteger.UBound).Width = ACTextBoxHeight
        lblLabel(lblLabel.GetUpperBound(0)).Width = VB6.TwipsToPixelsX(ACLabelWidthLong)

        txtInteger(txtInteger.GetUpperBound(0)).Height = VB6.TwipsToPixelsY(ACTextBoxHeight)
        lblLabel(lblLabel.GetUpperBound(0)).Height = VB6.TwipsToPixelsY(ACLabelHeight)

        lblLabel(lblLabel.GetUpperBound(0)).Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn)
        txtInteger(txtInteger.GetUpperBound(0)).Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn + ACLabelWidthLong + ACFormBorder)
    End Sub
    ' ***************************************************************** '
    ' Name: LoadDate
    '
    ' Description: Add a new date textbox control to the general details tab
    '               at a given height. Set its label.
    '
    ' ***************************************************************** '
    Private Sub LoadDate(ByRef lTop As Integer, ByRef iCurrentTab As Integer, ByVal v_lCount As Integer)

        SSTabHelper.SetSelectedIndex(tabMainTab, iCurrentTab)

        'If txtDate(txtDate.UBound).Visible = True Then
        If v_lCount > 0 Then
            ContainerHelper.LoadControl(Me, "txtDate", txtDate.GetUpperBound(0) + 1)
        End If
        'End If

        txtDate(txtDate.GetUpperBound(0)).Text = ""
        txtDate(txtDate.GetUpperBound(0)).Visible = True
        lblLabel(lblLabel.GetUpperBound(0)).Visible = True

        txtDate(txtDate.GetUpperBound(0)).Top = VB6.TwipsToPixelsY(lTop)
        lblLabel(lblLabel.GetUpperBound(0)).Top = VB6.TwipsToPixelsY(ACDiffBetweenTopsOfLabelAndText + lTop)


        lblLabel(lblLabel.GetUpperBound(0)).Width = VB6.TwipsToPixelsX(ACLabelWidthLong)

        txtDate(txtDate.GetUpperBound(0)).Height = VB6.TwipsToPixelsY(ACTextBoxHeight)
        lblLabel(lblLabel.GetUpperBound(0)).Height = VB6.TwipsToPixelsY(ACLabelHeight)

        lblLabel(lblLabel.GetUpperBound(0)).Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn)
        txtDate(txtDate.GetUpperBound(0)).Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn + ACLabelWidthLong + ACFormBorder)

    End Sub

    ' ***************************************************************** '
    ' Name: LoadLookup
    '
    ' Description: Add a new combo control to the general details tab
    '               at a given height. Set its label.
    '
    ' ***************************************************************** '
    Private Sub LoadLookup(ByRef lTop As Integer, ByRef iCurrentTab As Integer, ByVal v_lCount As Integer)

        SSTabHelper.SetSelectedIndex(tabMainTab, iCurrentTab)

        'If cmbLookup(cmbLookup.UBound).Visible = True Then
        If v_lCount > 0 Then
            ContainerHelper.LoadControl(Me, "cmbLookup", cmbLookup.GetUpperBound(0) + 1)
        End If
        'End If

        cmbLookup(cmbLookup.GetUpperBound(0)).Visible = True
        lblLabel(lblLabel.GetUpperBound(0)).Visible = True

        cmbLookup(cmbLookup.GetUpperBound(0)).Top = VB6.TwipsToPixelsY(lTop)
        lblLabel(lblLabel.GetUpperBound(0)).Top = VB6.TwipsToPixelsY(ACDiffBetweenTopsOfLabelAndText + lTop)

        lblLabel(lblLabel.GetUpperBound(0)).Width = VB6.TwipsToPixelsX(ACLabelWidthLong)

        lblLabel(lblLabel.GetUpperBound(0)).Height = VB6.TwipsToPixelsY(ACLabelHeight)

        lblLabel(lblLabel.GetUpperBound(0)).Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn)
        cmbLookup(cmbLookup.GetUpperBound(0)).Left = VB6.TwipsToPixelsX(ACLeftOFLabelInFirstColumn + ACLabelWidthLong + ACFormBorder)
    End Sub

    ' ***************************************************************** '
    ' Name: SetTabCaptionForPartytype
    '
    ' Description: Set the tab captions from the Resource file
    '              depending upon the party types defined for the Risk_type
    '
    ' ***************************************************************** '


    'Private Sub SetTabCaptionForPartytype(ByVal iPartyType As Integer, ByRef iCurrentTab As Integer, ByRef TabCountDisplay As Integer)
    'SSTabHelper.SetSelectedIndex(tabMainTab, iCurrentTab)
    'Select Case iPartyType
    'Case 1

    'SSTabHelper.SetTabCaption(tabMainTab, iCurrentTab, "&" & TabCountDisplay & " - " &  _
    '                          CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACDriverDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
    'Case 2

    'SSTabHelper.SetTabCaption(tabMainTab, iCurrentTab, "&" & TabCountDisplay & " - " &  _
    '                          CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACThirdPartyDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
    'Case 3

    'SSTabHelper.SetTabCaption(tabMainTab, iCurrentTab, "&" & TabCountDisplay & " - " &  _
    '                          CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACRepairerDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
    '
    'Case 4

    'SSTabHelper.SetTabCaption(tabMainTab, iCurrentTab, "&" & TabCountDisplay & " - " &  _
    '                          CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=ACWitnessDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
    'End Select
    '
    'End Sub


    ' ***************************************************************** '
    ' Name: LoadDataInCombo
    '
    ' Description: Fills the data from variant array into combobox
    ' INPUTS     : Combo Control to be filled
    '              2D - Array Containing the Record values
    '              Index in the Array where the Records of the
    '              Table Start from
    '              Number of records to enter
    ' ***************************************************************** '
    Private Function LoadDataInCombo(ByRef cboControl As ComboBox, ByVal vntData(,) As Object, ByVal vnStart As Integer, ByVal vnCount As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check whether an array has been passed
            If Information.IsArray(vntData) Then

                'clear the combobox
                cboControl.Items.Clear()

                'Load the data from the Array to the combobox
                For lCount As Integer = vnStart To vnStart + vnCount - 1

                    Dim cboControl_NewIndex As Integer = -1

                    cboControl_NewIndex = cboControl.Items.Add(CStr(vntData(1, lCount)))

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

    ' ***************************************************************** '
    ' Name: PopulateComboBoxes
    '
    ' Description: Populate the combo boxes with data from the lookup tabels
    '             specified for the risk type.
    '
    ' ***************************************************************** '

    Private Sub PopulateComboBoxes(ByRef collLookupID As Collection)
        Dim vResultArrray(,) As Object
        Dim bFound As Boolean
        Dim iComboNumber As Integer
        Dim vNameArrray As Object
        Const ACFirstItem As Integer = 0
        Const ACFirstColumn As Integer = 0
        Const ACSecondColumn As Integer = 1
        Const ACNoItems As Integer = 0

        Dim sLookupIDs As String = ""
        If collLookupID.Count <= ACNoItems Then Exit Sub
        ReDim vResultArrray(1, collLookupID.Count - 1)
        For iCount As Integer = 0 To collLookupID.Count - 1

            sLookupIDs = CStr(collLookupID(iCount + 1))
            ' Get then names of the lookup tables

            m_lReturn = m_oBusiness.GetLookupTables(sLookupIDs, vNameArrray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Exit Sub
            If Not Information.IsArray(vNameArrray) Then Exit Sub

            vResultArrray(ACFirstColumn, iCount) = sLookupIDs


            vResultArrray(ACSecondColumn, iCount) = vNameArrray(ACSecondColumn, ACFirstColumn)
        Next iCount

        If Not Information.IsArray(vResultArrray) Then Exit Sub


        m_vLookupValues = Nothing

        m_vLookupDetails = Nothing


        ReDim m_vLookupValues(4, vResultArrray.GetUpperBound(1))


        For iCount As Integer = vResultArrray.GetLowerBound(1) To vResultArrray.GetUpperBound(1)

            m_vLookupValues(ACFirstColumn, iCount) = vResultArrray(ACSecondColumn, iCount)
        Next iCount
        ' Get the lookup values

        m_lReturn = m_oBusiness.GetLookupValues(gPMConstants.PMELookupType.PMLookupAll, m_vLookupValues, m_iLanguageID, m_vLookupDetails)

        m_lReturn = GetLookupValues()


        For iCount As Integer = vResultArrray.GetLowerBound(1) To vResultArrray.GetUpperBound(1)
            bFound = False
            iComboNumber = iCount
            LoadDataInCombo(cmbLookup(iComboNumber), m_vLookupDetails, CInt(m_vLookupValues(2, iCount)), CInt(m_vLookupValues(3, iCount)))

            If Information.IsArray(m_vLookupDataArray) Then
                cmbLookup(iComboNumber).Tag = CStr(m_vLookupDataArray(1, iComboNumber))
                For iVar As Integer = ACFirstItem To cmbLookup(iComboNumber).Items.Count - 1
                    If VB6.GetItemData(cmbLookup(iComboNumber), iVar) = CInt(m_vLookupDataArray(2, iComboNumber)) Then
                        cmbLookup(iComboNumber).SelectedIndex = iVar
                    End If
                Next iVar
            End If
        Next iCount
    End Sub

    ' ***************************************************************** '
    ' Name: SaveAllGeneralDetails
    '
    ' Description: Save data, of all data types,defined for the Risk type in claim_user_defined_risk data
    '               pertaining to a Claim_Id stored in their respective control array.
    '
    ' ***************************************************************** '

    Public Sub SaveAllGeneralDetails()
        If txtText(txtText.GetLowerBound(0)).Visible Then
            SaveDataForText()
        End If

        If txtInteger(txtInteger.GetLowerBound(0)).Visible Then
            SaveDataForInteger()
        End If

        If txtDate(txtDate.GetLowerBound(0)).Visible Then
            SaveDataForDate()
        End If

        If cmbLookup(cmbLookup.GetLowerBound(0)).Visible Then
            SaveDataForLookup()
        End If

        If chkCheck(chkCheck.GetLowerBound(0)).Visible Then
            SaveDataForBoolean()
        End If

        'Checkbox
    End Sub

    ' ***************************************************************** '
    ' Name: SaveDataForBoolean
    '
    ' Description: Save boolean data defined for the Risk type in claim_user_defined_risk data
    '               pertaining to a Claim_Id stored in the control array of check boxes.
    '
    ' ***************************************************************** '

    Public Sub SaveDataForBoolean()
        For iControl As Integer = chkCheck.GetLowerBound(0) To chkCheck.GetUpperBound(0)
            SaveGeneralDetails(CInt(Convert.ToString(chkCheck(iControl).Tag)), m_lclaimid, CStr(chkCheck(iControl).CheckState))
        Next iControl
    End Sub

    ' ***************************************************************** '
    ' Name: SaveDataForText
    '
    ' Description: Save text data defined for the Risk type in claim_user_defined_risk data
    '               pertaining to a Claim_Id stored in the control array of text boxes.
    '
    ' ***************************************************************** '
    Public Sub SaveDataForText()
        For iControl As Integer = txtText.GetLowerBound(0) To txtText.GetUpperBound(0)
            SaveGeneralDetails(CInt(Convert.ToString(txtText(iControl).Tag)), m_lclaimid, txtText(iControl).Text)
        Next iControl
    End Sub
    ' ***************************************************************** '
    ' Name: SaveDataForInteger
    '
    ' Description: Save integer data defined for the Risk type in claim_user_defined_risk data
    '               pertaining to a Claim_Id stored in the control array of integer text boxes.
    '
    ' ***************************************************************** '
    Public Sub SaveDataForInteger()
        For iControl As Integer = txtInteger.GetLowerBound(0) To txtInteger.GetUpperBound(0)
            SaveGeneralDetails(CInt(Convert.ToString(txtInteger(iControl).Tag)), m_lclaimid, txtInteger(iControl).Text)
        Next iControl
    End Sub

    ' ***************************************************************** '
    ' Name: SaveDataForDate
    '
    ' Description: Save date data defined for the Risk type in claim_user_defined_risk data
    '               pertaining to a Claim_Id stored in the control array of date text boxes.
    '
    ' ***************************************************************** '
    Private Sub SaveDataForDate()
        For iControl As Integer = txtDate.GetLowerBound(0) To txtDate.GetUpperBound(0)

            SaveGeneralDetails(CInt(collRiskDataDefnForDates(iControl + 1)), m_lclaimid, Convert.ToString(txtDate(iControl).Tag))
        Next iControl
    End Sub

    ' ***************************************************************** '
    ' Name: SaveDataForLookup
    '
    ' Description: Save lookup data defined for the Risk type in claim_user_defined_risk data
    '               pertaining to a Claim_Id stored in the control array of combo boxes.
    '
    ' ***************************************************************** '

    Private Sub SaveDataForLookup()
        Const ACNoItems As Integer = 0
        For iControl As Integer = cmbLookup.GetLowerBound(0) To cmbLookup.GetUpperBound(0)
            If cmbLookup(iControl).SelectedIndex >= ACNoItems Then
                SaveGeneralDetails(CInt(Convert.ToString(cmbLookup(iControl).Tag)), m_lclaimid, CStr(VB6.GetItemData(cmbLookup(iControl), cmbLookup(iControl).SelectedIndex)))
            End If
        Next iControl
    End Sub

    ' ***************************************************************** '
    ' Name: SaveGeneralDetails
    '
    ' Description: Save the data stored in control arrays in to the
    '              Claim_user_defined_risk_data
    ' ***************************************************************** '

    Private Sub SaveGeneralDetails(ByVal lRiskDataDefnID As Integer, ByVal lClaimId As Integer, ByRef sValue As String)


        m_lReturn = m_oBusiness.AddGeneralDetail(lClaimId, lRiskDataDefnID, sValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save general details for claim", vApp:=ACApp, vClass:=ACClass, vMethod:="Save general details")
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: SaveComment
    '
    ' Description: Save comment in claim_risk for a Claim
    '
    ' ***************************************************************** '

    Private Sub SaveComment()
        Const ACFirstCommentTextBox As Integer = 0

        m_lReturn = m_oBusiness.AddClaimRisk(Claimid, RiskType, ClaimRiskDesciption, txtComment(ACFirstCommentTextBox).Text)
    End Sub

    ' ***************************************************************** '
    ' Name: CopyWorkToClaim
    ' Created: JMK 21/05/2001
    ' Description: Info Only Claims do not proceed to the 'Copy work to claim'
    '               part of the roadmap, so it is done here
    '
    ' ***************************************************************** '

    'Private Function CopyWorkToClaim(ByVal v_lWorkClaimID As Integer) As Integer
    'Dim result As Integer = 0
    'Dim iCLMChangeClaimStatus As Object

    'Dim oChangeClaimStatus As iCLMChangeClaimStatus.Interface_Renamed
    'Dim vKeyArray As Object
    '
    'Try 
    '
    ''ReDim vKeyArray(1, 0)
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'vKeyArray(0, 0) = "claim_cnt"

    'vKeyArray(1, 0) = v_lWorkClaimID
    '
    'Dim temp_oChangeClaimStatus As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oChangeClaimStatus, sClassName:="iCLMChangeClaimStatus.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
    'oChangeClaimStatus = temp_oChangeClaimStatus
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iCLMChangeClaimStatus.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = oChangeClaimStatus.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetProcessModes.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = CType(oChangeClaimStatus, SSP.S4I.Interfaces.ILocalInterface).Initialise()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = oChangeClaimStatus.SetKeys(vKeyArray)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set Keys.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = oChangeClaimStatus.Start()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = oChangeClaimStatus.Terminate()
    '
    'oChangeClaimStatus = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed Copy Work To Claim", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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


            'PSL 11/07/2003
            'If it's open claim then there is no lock to unlock making this the same as third party recovery eand others
            If m_sTransactionType <> "C_CO" Then

                'PSL 08/07/2003 don't unlock if it is in View mode
                'PSL 15/07/2003 the view stuff was wrong.
                'We DO lock when viewing (otherwise we get duplicates in work tables
                'Changed find claim, so that pressing OK from the view roadmap, uses mode
                'of DUMMYDELETE, like it used to before the view roadmap
                'If m_iTask <> PMView And m_iTask <> PMDummyDelete Then

                m_lReturn = oPMLock.UnlockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)

                ' DD 26/7/2004 - PN13122
                ' Only error if return = PMError. If return = PMFalse, it just means
                ' the claim was not locked in the first place.
                'If (m_lReturn <> PMTrue) Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock claim", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If
                'End If
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
    '
    ' Name: SaveOtherParty
    '
    ' Description: save driver, repairer etc
    '
    ' History: 07/08/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function SaveOtherParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If SSTabHelper.GetTabVisible(tabMainTab, ACDriver) Then
                m_lReturn = uctDriver.OKClick()
            End If

            If SSTabHelper.GetTabVisible(tabMainTab, ACThirdParty) Then
                m_lReturn = uctThirdParty.OKClick()
            End If

            If SSTabHelper.GetTabVisible(tabMainTab, ACRepairer) Then
                m_lReturn = uctRepairer.OKClick()
            End If

            If SSTabHelper.GetTabVisible(tabMainTab, ACWitness) Then
                m_lReturn = uctWitness.OKClick()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveOtherParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveOtherParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *************************************************************************** '
    '
    ' Name: RefreshOtherParty
    ' Description: refresh other party details
    ' History: 08/02/2006 - A.Robinson. Created (S4B Claim Enhancments R&D 2005)
    '
    ' *************************************************************************** '

    'Private Function RefreshOtherParty() As Integer
    '
    'Dim result As Integer = 0
    'Const kMETHOD_NAME As String = "RefreshOtherParty"
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If SSTabHelper.GetTabVisible(tabMainTab, ACDriver) Then
    'uctDriver.Refresh()
    'End If
    '
    'If SSTabHelper.GetTabVisible(tabMainTab, ACThirdParty) Then
    'uctThirdParty.Refresh()
    'End If
    '
    'If SSTabHelper.GetTabVisible(tabMainTab, ACRepairer) Then
    'uctRepairer.Refresh()
    'End If
    '
    'If SSTabHelper.GetTabVisible(tabMainTab, ACWitness) Then
    'uctWitness.Refresh()
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshOtherParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMETHOD_NAME, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: GetReserveDetails
    '
    ' Description: Get the reserve details...for non-ClaimsBuilder screens
    '              this is returned from the Peril screen in a key array.
    '              For ClaimsBuilder, we have to re-get the data...
    '
    ' History: 22/07/2002   RVH Created
    '
    ' ***************************************************************** '

    'Private Function GetReserveDetails(ByVal lPerilId As Integer, ByRef r_nSumInsured As Single, ByRef r_nCurrentReserve As Single) As Integer
    'Dim result As Integer = 0
    'Dim bCLMPeril As Object

    'Dim oBusiness As bCLMPeril.Business
    'Dim r_vReserveDetailsArray, r_vRecoveryDetailsArray, r_vArray As Object
    'Dim nSalvage, nRecovery As Single
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    'r_nSumInsured = 0
    'r_nCurrentReserve = 0
    ''
    '   Get instance of Peril business object
    ''
    'Dim temp_oBusiness As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bCLMPeril.Business", vInstanceManager:="ClientManager")
    'oBusiness = temp_oBusiness
    '
    '   Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create business object to re-get reserve details for screen display", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    ''
    '   Have to "trick" the business object into re-setting the main
    '   key details into the data layer...
    ''

    'oBusiness.PerilID = lPerilId

    'oBusiness.Claimid = m_lclaimid
    '

    'm_lReturn = oBusiness.GetControls(r_vArray)
    '
    '   Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed calling the GetControls method on the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    ''
    '   Get the reserve details back into an array
    ''

    'm_lReturn = oBusiness.GetReserveDetails(m_lPolicyId, m_lRisk, r_vReserveDetailsArray)
    '
    '   Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to re-get reserve details for screen display", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    ''
    '   Check if we got some data back...
    ''

    'If Not Information.IsArray(r_vReserveDetailsArray) Or Object.Equals(r_vReserveDetailsArray, Nothing) Then
    '
    '******************************
    ' MEvans : 11-02-2003 : Issue - 2144
    ' Should have been logging info silently - not to an error popup
    ' The Empty Array is handled by calling procedure being returned false from
    ' this routine
    'gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Reserve details not found for passed policy/risk", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' call the function to collect the details for the Recovery Type Third Party

    'm_lReturn = oBusiness.GetRecoveryDetails(0, r_vRecoveryDetailsArray)
    '
    '   Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to re-get recovery details for screen display", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    ''
    '   Check for empty strings and replace with zero
    ''
    'For 'lCount As Integer = 0 To 2

    'If CStr(r_vRecoveryDetailsArray(lCount, 0)) = "" Then

    'r_vRecoveryDetailsArray(lCount, 0) = 0
    'End If
    'Next lCount
    '
    '   (0,0)=Initial, (1,0)=Revised, (2,0)=Received



    'nRecovery = (CDec(r_vRecoveryDetailsArray(0, 0)) + CDec(r_vRecoveryDetailsArray(1, 0))) - CDec(r_vRecoveryDetailsArray(2, 0))
    '
    '   call the function to collect the details for the Salvage Type Third Party

    'm_lReturn = oBusiness.GetRecoveryDetails(1, r_vRecoveryDetailsArray)
    '
    '   Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to re-get salvage details for screen display", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    ''
    '   Check for empty strings and replace with zero
    ''
    'For 'lCount As Integer = 0 To 2

    'If CStr(r_vRecoveryDetailsArray(lCount, 0)) = "" Then

    'r_vRecoveryDetailsArray(lCount, 0) = 0
    'End If
    'Next lCount
    '
    '   (0,0)=Initial, (1,0)=Revised, (2,0)=Received



    'nSalvage = (CDec(r_vRecoveryDetailsArray(0, 0)) + CDec(r_vRecoveryDetailsArray(1, 0))) - CDec(r_vRecoveryDetailsArray(2, 0))
    ''
    '   Run through the reserve details and compute out the sum insured and
    '   current reserve
    ''

    'For 'lCount As Integer = r_vReserveDetailsArray.GetLowerBound(1) To r_vReserveDetailsArray.GetUpperBound(1)
    'For 'lColumn As Integer = 1 To 6

    'If CStr(r_vReserveDetailsArray(lColumn, lCount)) = "" Then

    'r_vReserveDetailsArray(lColumn, lCount) = 0
    'End If
    'Next lColumn

    'r_nSumInsured += CDbl(r_vReserveDetailsArray(4, lCount))



    'r_nCurrentReserve = r_nCurrentReserve + ((CDec(r_vReserveDetailsArray(1, lCount)) + CDec(r_vReserveDetailsArray(3, lCount))) - CDec(r_vReserveDetailsArray(2, lCount))) - (nSalvage - nRecovery)
    'Next lCount
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReserveDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result


			'
			'Return result
		'End Try
	'End Function
	
	


	Private Sub VScroll1_Change(ByRef Index As Integer, ByVal newScrollValue As Integer)

		Picture2(Index).Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Picture2(Index).Height) * ((-VScroll1(Index).Value) * 0.01))
	End Sub
	

	Private Sub VScroll1_Scroll_Renamed(ByRef Index As Integer, ByVal newScrollValue As Integer)

		Picture2(Index).Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Picture2(Index).Height) * ((-VScroll1(Index).Value) * 0.01))
	End Sub
	
	' ***************************************************************** '
	'
	' Name          : ShowPartySummaryDetails
	'
	' Description   : This function will create the Party Summary Interface,
	'                   set it keys and show the interface
	'
	' Edit History  :
	' RAM20021021   : Created  - NRMA Changes (Sirius Process No 126)
	' ***************************************************************** '
	Public Function ShowPartySummaryDetails() As Integer
		

		Dim result As Integer = 0
		Dim ACShowFormModal As FormShowConstants = FormShowConstants.Modal ' Show the Form vbModal

		Dim ACShowFormModeLess As FormShowConstants = FormShowConstants.Modeless ' Show the Form vbModeless
		
        Dim vKeyArray(,) As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' First fetch the basic details for Party / Policy
			If Not m_bClientPolicyDetailsLoaded Then

				m_lReturn = m_oBusiness.GetClientPolicyDetails(v_lInsuranceFileCnt:=m_lPolicyId, r_lPartyCnt:=m_lPartyCnt, r_sPartyShortName:=m_sPartyShortName, r_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_sInsuranceRef:=m_sInsuranceRef)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
					Return result
				End If
				
				m_bClientPolicyDetailsLoaded = True
			End If
			
			If m_bAddtask Then
				Return result
			End If
			
			ReDim vKeyArray(1, 2) ' Set the Navigator Keys
			

			vKeyArray(0, 0) = PMNavKeyConst.PMKeyNamePartyCnt ' Sent in the Party Cnt

			vKeyArray(1, 0) = m_lPartyCnt
			

			vKeyArray(0, 1) = PMNavKeyConst.PMKeyNameShortName

			vKeyArray(1, 1) = m_sPartyShortName.Trim() ' Sent in the Party Short Name
			

			vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameDisplayMode
			' SET 13/08/2004 ISS14119 - display modally

			vKeyArray(1, 2) = ACShowFormModal
			
			If m_oPartySummary Is Nothing Then
				
				' Create the Interface if not available
				m_oPartySummary = New iSIRPartySummary.Interface_Renamed()
                'developer guide no.9
                m_lReturn = m_oPartySummary.Initialise()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				m_oPartySummary.CallingAppName = ACApp
				

				m_lReturn = m_oPartySummary.SetKeys(vKeyArray)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				m_lReturn = m_oPartySummary.Start()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			Else
				' Swith to the Party Summary Interface (i.e show it on top of all interface)
				m_lReturn = m_oPartySummary.switchTo()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.switchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			' SET 13/08/2004 ISS14119 - clean up the interface
			m_lReturn = ClosePartyPolicySummary()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPartySummaryDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
			


			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name          : ShowPolicySummaryDetails
	'
	' Description   : This function will create the Policy Summary Interface,
	'                   set it keys and show the interface
	'
	' Edit History  :
	' RAM20021021   : Created  - NRMA Changes (Sirius Process No 126)
	' ***************************************************************** '
	Public Function ShowPolicySummaryDetails() As Integer
		

		Dim result As Integer = 0
		Dim ACShowFormModal As FormShowConstants = FormShowConstants.Modal ' Show the Form vbModal

		Dim ACShowFormModeLess As FormShowConstants = FormShowConstants.Modeless ' Show the Form vbModeless
		
        Dim vKeyArray(,) As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' First fetch the basic details for Party / Policy
			If Not m_bClientPolicyDetailsLoaded Then

				m_lReturn = m_oBusiness.GetClientPolicyDetails(v_lInsuranceFileCnt:=m_lPolicyId, r_lPartyCnt:=m_lPartyCnt, r_sPartyShortName:=m_sPartyShortName, r_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_sInsuranceRef:=m_sInsuranceRef)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
					Return result
				End If
				
				m_bClientPolicyDetailsLoaded = True
			End If
			
			ReDim vKeyArray(1, 5) ' Set the Navigator Keys
			

			vKeyArray(0, 0) = PMNavKeyConst.PMKeyNamePartyCnt ' Sent in the Party Cnt

			vKeyArray(1, 0) = m_lPartyCnt
			

			vKeyArray(0, 1) = PMNavKeyConst.PMKeyNameShortName

			vKeyArray(1, 1) = m_sPartyShortName.Trim()
			

			vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameInsuranceFolderCnt

			vKeyArray(1, 2) = m_lInsuranceFolderCnt
			

			vKeyArray(0, 3) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

			vKeyArray(1, 3) = m_lPolicyId ' Note : This is insurance_file_cnt
			

			vKeyArray(0, 4) = PMNavKeyConst.PMKeyNameInsReference

			vKeyArray(1, 4) = m_sInsuranceRef.Trim()
			

			vKeyArray(0, 5) = PMNavKeyConst.PMKeyNameDisplayMode
			' SET 13/08/2004 ISS14119 - display modally

			vKeyArray(1, 5) = ACShowFormModal
			
			If m_oPolicySummary Is Nothing Then
				
				' Create the Interface if not available
				m_oPolicySummary = New iSIRPolicySummary.Interface_Renamed()
				
                'developer guide no.9
                m_lReturn = m_oPolicySummary.Initialise()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				m_oPolicySummary.CallingAppName = ACApp
				
				

				m_lReturn = m_oPolicySummary.SetKeys(vKeyArray)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				m_lReturn = m_oPolicySummary.Start()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			Else
				
				' Swith to the Policy Summary Interface  (i.e show it on top of all interface)
				
				m_lReturn = m_oPolicySummary.switchTo()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.switchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			' SET 13/08/2004 ISS14119 - clean up the interface
			m_lReturn = ClosePartyPolicySummary()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPolicySummaryDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
			


			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name          : ClosePartyPolicySummary
	'
	' Description   : This function will Close the Party & Policy Summary
	'                   Interfaces, if they are loaded
	'
	' Edit History  :
	' RAM20021023   : Created  - NRMA Changes (Sirius Process No 126)
	' ***************************************************************** '
	Private Function ClosePartyPolicySummary() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Terminate the objects if necessary and available
			If m_oPartySummary Is Nothing Then
				' Do nothing
			Else
				' Terminate
                m_oPartySummary.Dispose()
                'Clear it
                m_oPartySummary = Nothing
			End If
			
			If m_oPolicySummary Is Nothing Then
				' Do nothing
			Else
				' Terminate
                m_oPolicySummary.Dispose()
                'Clear it
                m_oPolicySummary = Nothing
			End If
			
			If m_oRisk Is Nothing Then
				' Do nothing
			Else
                m_oRisk.Dispose()
                'Clear it
                m_oRisk = Nothing
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClosePartyPolicySummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClosePartyPolicySummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
			


			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name          : OptionOn
	'
	' Description   : Determines whether the specified product option
	'                   is switched on and enabled
	'
	' ***************************************************************** '

	'Private Function OptionOn(ByVal v_lOption As Integer, ByRef r_bOptionOn As Boolean) As Integer
		'
		'Dim result As Integer = 0
		'Dim sValue As String = ""
		'
		'Try 
			'
			' initialise the function return status
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' assume selected option isnt on
			'r_bOptionOn = False
			'
			' get the product option values
			'With g_oObjectManager
				'DC131003 -PN7429 -had wrong parameters for Pre-Globalization
				'm_lReturn = iPMFunc.getProductOptionValue(v_lOption, gPMConstants.SIRBCHHeadOffice, sValue)
			'End With
			'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' check if the option is present or is enabled for the specified branch
				'If sValue <> "" And sValue <> "0" Then
					'r_bOptionOn = True
				'End If
			'Else
				'result = gPMConstants.PMEReturnCode.PMFalse
			'End If
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error Message
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OptionOn Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OptionOn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'Return result
			'


			'
			'Return result
		'End Try
	'End Function
	Private Sub VScroll1_Scroll(ByVal eventSender As Object, ByVal eventArgs As ScrollEventArgs) Handles _VScroll1_4.Scroll, _VScroll1_3.Scroll, _VScroll1_2.Scroll, _VScroll1_1.Scroll, _VScroll1_0.Scroll
		Dim Index As Integer = Array.IndexOf(VScroll1, eventSender)
		Select Case eventArgs.Type
			Case ScrollEventType.ThumbTrack
				VScroll1_Scroll_Renamed(Index, eventArgs.NewValue)
			Case ScrollEventType.EndScroll
				VScroll1_Change(Index, eventArgs.NewValue)
		End Select
	End Sub

    
End Class
