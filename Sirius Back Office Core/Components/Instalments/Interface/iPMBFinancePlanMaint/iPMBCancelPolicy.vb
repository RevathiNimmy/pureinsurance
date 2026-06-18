Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Partial Friend Class frmCancelPolicy
	Inherits System.Windows.Forms.Form
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmCancelPolicy"
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	Private m_lLapseReasonID As Integer
	Private m_lStatus As Integer
	Private m_bWriteOff As Boolean
	Private m_bSpoolDocument As Boolean
	Private m_lAccountID As Integer
	Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_dPolicyLapseDate As Date
    Private m_vFinancePlanArray As Object
	'Public property declaration
	Public Property LapseReasonID() As Integer
		Get
			Return m_lLapseReasonID
		End Get
		Set(ByVal Value As Integer)
			m_lLapseReasonID = Value
		End Set
	End Property
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	Public Property WriteOff() As Boolean
		Get
			Return m_bWriteOff
		End Get
		Set(ByVal Value As Boolean)
			m_bWriteOff = Value
		End Set
	End Property
	Public Property SpoolDocument() As Boolean
		Get
			Return m_bSpoolDocument
		End Get
		Set(ByVal Value As Boolean)
			m_bSpoolDocument = Value
		End Set
	End Property
	Public Property AccountID() As Integer
		Get
			Return m_lAccountID
		End Get
		Set(ByVal Value As Integer)
			m_lAccountID = Value
		End Set
	End Property
	Public Property PolicyLapseDate() As Date
		Get
			Return m_dPolicyLapseDate
		End Get
		Set(ByVal Value As Date)
			m_dPolicyLapseDate = Value
		End Set
    End Property

    'Public Property Let FinancePlanArray(vArray As Variant)
    'm_vFinancePlanArray = vArray
    'End Property
    Public Property FinancePlanArray() As Object
        Get
            Return m_vFinancePlanArray
        End Get
        Set(ByVal Value As Object)
            m_vFinancePlanArray = Value
        End Set
    End Property
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		LapseReasonID = 0
		WriteOff = False
		SpoolDocument = False
		Status = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        'Richard Clarke 25/01/2010
        Dim vPolicyListArray(,) As Object = Nothing
        Dim lInsuranceFileCnt As Long
        Dim lErrorCode As Long
        Dim bBackdatingRequired As Boolean
        Dim lPolicyVersion As Long
        Dim vAffectedInsuranceFileCnts As Object = Nothing
        Dim bOkToContinue As Boolean
        Dim dOldLapseDate As Date
        'Dim result As Integer

        'result = gPMConstants.PMEReturnCode.PMTrue
        dOldLapseDate = PolicyLapseDate
        LapseReasonID = cboLapseReason.ItemId
        PolicyLapseDate = txtPolicyLapseDate.Text
        WriteOff = chkWriteOff.CheckState = CheckState.Checked
        SpoolDocument = chkSpoolDocument.CheckState = CheckState.Checked

        'we need to check the date they've entered before we can cancel as there may be NO policy
        'versions for this date - in which case prompt them to enter a different date
        'fcancelpolicy.PolicyLapseDate
        Dim temp_g_oBusiness As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRPremiumFinance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        g_oBusiness = temp_g_oBusiness
        m_lReturn = g_oBusiness.GetPolicyList(v_lPlanPFCnt:=m_vFinancePlanArray(k_PFPlanPFCnt, 0), _
                                            r_vPolicyListArray:=vPolicyListArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Err_cmdOK_Click
        End If

        If IsArray(vPolicyListArray) Then
            'now we need to loop round the array to make sure there's something to cancel
            Dim lPolicyList As Long
            For lPolicyList = 0 To UBound(vPolicyListArray, 2)

                m_lReturn = g_oFindInsurance.GetVersionsByDate( _
                    r_lInsuranceFileCnt:=lInsuranceFileCnt, _
                    v_dtStartDate:=PolicyLapseDate, _
                    r_lPolicyVersion:=lPolicyVersion, _
                    r_lErrorCode:=lErrorCode, _
                    v_lInsuranceFolderCnt:=vPolicyListArray(6, lPolicyList), _
                    r_bBackdatingRequired:=bBackdatingRequired, _
                    r_vAffectedInsuranceFileCnts:=vAffectedInsuranceFileCnts, _
                    v_bIsReinstatement:=False, _
                    v_bIsCancellation:=True, _
                    v_lDeletedRiskInsuranceFileCnt:=0, _
                    v_lMTAType:=kMTATypePermanentAndTemporary)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GoTo Err_cmdOK_Click
                End If

                If IsArray(vAffectedInsuranceFileCnts) Then
                    Dim lAffectedInsuranceFileCnts As Long
                    For lAffectedInsuranceFileCnts = 0 To UBound(vAffectedInsuranceFileCnts, 2)

                        If vAffectedInsuranceFileCnts(1, lAffectedInsuranceFileCnts) <= PolicyLapseDate Then
                            bOkToContinue = True
                            Exit For
                        End If
                    Next
                End If
                If bOkToContinue Then
                    Exit For
                End If
            Next

            'we didn't find a single policy that we can use to lapse from this date
            If Not bOkToContinue Then
                MsgBox("The lapse date is not valid for this policy. Please enter a valid lapse date.", vbOKOnly + vbInformation, "Premium Finance")
                PolicyLapseDate = dOldLapseDate
                ' txtPolicyLapseDate = (dOldLapseDate)
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPolicyLapseDate, vControlValue:=dOldLapseDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GoTo Err_cmdOK_Click
                End If
                Exit Sub
            End If

        Else
            'we just need to quit here as something's wrong somewhere else and we can't cancel a policy at all
            MsgBox("This instalment plan is not associated with a policy.", vbOKOnly + vbInformation, "Premium Finance")
            Status = gPMConstants.PMEReturnCode.PMNotFound
            PolicyLapseDate = dOldLapseDate
            'txtPolicyLapseDate = dOldLapseDate
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPolicyLapseDate, vControlValue:=dOldLapseDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GoTo Err_cmdOK_Click
            End If
            Me.Hide()
            Exit Sub
        End If

        Status = gPMConstants.PMEReturnCode.PMOK
        Me.Hide()
        Exit Sub

Err_cmdOK_Click:
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ok button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

    End Sub
	
	Private Sub cmdViewAccount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdViewAccount.Click
		
        Dim oFindTransaction As iACTFindTransaction.Interface_Renamed = Nothing
		
		If oFindTransaction Is Nothing Then
			
            Dim temp_oFindTransaction As Object = Nothing
			m_lReturn = g_oObjectManager.GetInstance(temp_oFindTransaction, sClassName:="iACTFindTransaction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oFindTransaction = temp_oFindTransaction
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iACTFindTransaction.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdViewAccount_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Exit Sub
			End If
			
		End If
		' Set the account id

		oFindTransaction.AccountID = m_lAccountID
		
		'to set the value if its called from client manager
		'oFindTransaction.CalledViaClientManager = m_bCalledViaClientManager
		
		' Show outstandingonly or not

		oFindTransaction.OutstandingOnly = False

		oFindTransaction.CallingAppName = ACApp
		
		' Start it up

		m_lReturn = oFindTransaction.Start()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start iACTFindTransaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdViewAccount_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			Exit Sub
		End If
		
		' Terminate

		oFindTransaction.Dispose()
		
		oFindTransaction = Nothing
		
	End Sub
	

	Private Sub frmCancelPolicy_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		m_oFormFields = New iPMFormControl.FormFields()
		m_lReturn = SetFieldValidation()
		chkWriteOff.CheckState = CheckState.Checked
		chkSpoolDocument.CheckState = CheckState.Checked
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPolicyLapseDate, vControlValue:=PolicyLapseDate)
        'Developer Guide No 220
        cboLapseReason.FirstItem = ""
	End Sub
	
	Private Sub txtPolicyLapseDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicyLapseDate.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPolicyLapseDate)
	End Sub
	
	Private Sub txtPolicyLapseDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicyLapseDate.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPolicyLapseDate)
	End Sub
	Private Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPolicyLapseDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
End Class
