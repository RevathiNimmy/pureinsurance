Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmMainSFU
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name     : frmMainSFU
	' File Name     : frmMainSFU.frm
	' Date          : 17-10-2002
	' Author        : Ram Chandrabose
	' Description   : Interface used to display the Policy Details for a
	'                   given party
	' Note          : This interface is Underwriting specific
	'
	' Edit History  :
	' RAM20021017   : Created
	' ***************************************************************** '
	
	Private Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Integer) As Integer
	
	Private Const ACClass As String = "frmMainSFU"
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_lError As Integer
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As Integer
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	Private m_sPartyShortName As String = ""
	Private m_lPartyCnt As Integer
	Private m_lInsuranceFolderCnt As Integer
	Private m_lInsuranceFileCnt As Integer
	Private m_sInsuranceRef As String = ""
	
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
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			' Set the interface exit status.
			m_lStatus = Value
		End Set
	End Property
	
	Public WriteOnly Property PartyCnt() As Integer
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property PartyShortName() As String
		Set(ByVal Value As String)
			m_sPartyShortName = Value
		End Set
	End Property
	
	Public WriteOnly Property InsuranceFolderCnt() As Integer
		Set(ByVal Value As Integer)
			m_lInsuranceFolderCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property InsuranceFileCnt() As Integer
		Set(ByVal Value As Integer)
			m_lInsuranceFileCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property InsuranceRef() As String
		Set(ByVal Value As String)
			m_sInsuranceRef = Value
		End Set
	End Property
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
		
	End Sub
	

	Private Sub frmMainSFU_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Try 
			
			m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
			End If
		
		Catch excep As System.Exception
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name          : SetInterfaceDefaults
	'
	' Description   : Sets any defaults to the Interface form
	'
	' Edit History  :
	' RAM20021018   : Created
	' ***************************************************************** '
	Public Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_sInsuranceRef.Length > 0 Then
				Me.Text = "Policy Summary - " & m_sPartyShortName
			Else
				Me.Text = "Policy Summary Screen"
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name          : DataToInterface
	'
	' Description   : This function sets the properties of the Policy
	'                   Summary Control, so that it can display the
	'                   Summary details of all the policy for a party
	'
	' Edit History  :
	' RAM20021018   : Created
	' ***************************************************************** '
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			With uctPMUPolicyExplorer1
				.CallingAppName = m_sCallingAppName
				.Task = gPMConstants.PMEComponentAction.PMView
				.Status = gPMConstants.PMEReturnCode.PMTrue
				.TransactionType = m_sTransactionType
				.EffectiveDate = DateTime.Today
				.ProcessMode = m_lProcessMode
			End With
            'developer guide no.9
            m_lReturn = uctPMUPolicyExplorer1.Initialise()
			If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
				
				result = m_lReturn
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
			End If
			
			uctPMUPolicyExplorer1.ShortName = m_sPartyShortName
			uctPMUPolicyExplorer1.InsHolderCnt = m_lPartyCnt
			
			m_lReturn = uctPMUPolicyExplorer1.LoadControl()
			If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
				result = m_lReturn
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
			End If
			
			m_lReturn = uctPMUPolicyExplorer1.GetPolicies()
			If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
				result = m_lReturn
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Policy Details.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
    Private Const vbFormCode As Integer = 0
	Private Sub frmMainSFU_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		' Check if the interface has been terminated by means
		' other than pressing the command buttons (OK or Cancel).


        If UnloadMode <> vbFormCode Then

            Cancel = True

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub
	
	' ***************************************************************** '
	' Name          : uctPMUPolicyExplorer1_lvwVersionsDblClick
	'
	' Description   : This event procedure will be triggered when the
	'                   user click the "View Details" Button, after selecting
	'                   a policy. So we need to show the details of the
	'                   selected policy.
	'
	' Edit History  :
	' RAM20021018   : Created
	' ***************************************************************** '
    Private Sub uctPMUPolicyExplorer1_lvwVersionsDblClick(ByVal Sender As Object, ByVal e As uctPMUPolicyExpCtl.uctPMUPolicyExplorer.lvwVersionsDblClickEventArgs) Handles uctPMUPolicyExplorer1.lvwVersionsDblClick

        Try

            Dim m_lInsHolderCnt As Long = e.m_lInsHolderCnt
            Dim m_lInsFileCnt As Long = e.m_lInsFileCnt
            Dim m_sShortName As String = e.m_sShortName
            Dim m_sInsReference As String = e.m_sInsReference
            m_lInsuranceFolderCnt = e.m_lInsuranceFolderCnt

            m_lReturn = ShowPolicySummary(v_lPartyCnt:=m_lInsHolderCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsFileCnt:=m_lInsFileCnt, v_sShortName:=m_sShortName, v_sInsReference:=m_sInsReference)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try

    End Sub
	
	' ***************************************************************** '
	' Name          : ShowPolicySummary
	'
	' Description   : Displays policy summary information (individual)
	'
	' Edit History  :
	' RAM20021018   : Created
	' ***************************************************************** '
	Public Function ShowPolicySummary(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsFileCnt As Integer, ByVal v_sShortName As String, ByVal v_sInsReference As String) As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim oForm As frmPolicySummaryUW
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			oForm = New frmPolicySummaryUW()
			
			With oForm
				
				.PartyCnt = v_lPartyCnt
				.ShortName = v_sShortName
				.InsuranceFolderCnt = v_lInsuranceFolderCnt
				.InsuranceFileCnt = v_lInsFileCnt
				.InsReference = v_sInsReference
				
				.ShowDialog()
				
			End With
			
			oForm.Close()
			oForm = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPolicySummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name          : switchTo
	'
	' Description   : Switches focus to this form.
	'
	' Edit History  :
	' RAM20021021   : Created
	' ***************************************************************** '
	Public Function switchTo() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the focus
			Me.Activate()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="switchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="switchTo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Class
