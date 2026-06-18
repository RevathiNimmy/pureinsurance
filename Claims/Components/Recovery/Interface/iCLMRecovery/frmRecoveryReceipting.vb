Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Friend Partial Class frmRecoveryReceipting
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmRecoveryREceipt"
	
	
	Private m_lClaimId As Integer
	Private m_lClaimPerilId As Integer
	Private m_sTransactionType As String = ""
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_dtEffectiveDate As Date
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lRecoveryMode As MainModule.RecoveryModeEnum
	Private m_lErrorNumber As gPMConstants.PMEReturnCode
	
	Public WriteOnly Property RecoveryMode() As Integer
		Set(ByVal Value As Integer)
			m_lRecoveryMode = Value
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
	
	Public WriteOnly Property ClaimId() As Integer
		Set(ByVal Value As Integer)
			m_lClaimId = Value
		End Set
	End Property
	
	Public WriteOnly Property ClaimPerilId() As Integer
		Set(ByVal Value As Integer)
			m_lClaimPerilId = Value
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			m_sTransactionType = Value
		End Set
	End Property
	
	Public WriteOnly Property Task() As Integer
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			Return m_lErrorNumber
		End Get
	End Property
	
	Private Sub cmcCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmcCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		ActionOk()
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		' Initialise the error number value.
		m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
	End Sub
	
	' ***************************************************************** '
	' Name: zName
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '

    Private Sub frmRecoveryReceipting_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"


        Dim lReturn As gPMConstants.PMEReturnCode

        Try




        '**********************************************
        'Developer Guide no. 9
        lReturn = uctCLMReceipt.Initialise()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
        End If

        '**********************************************

        uctCLMReceipt.RecoveryMode = m_lRecoveryMode

        lReturn = uctCLMReceipt.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType, vEffectiveDate:=DateTime.Today)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
        End If

        '**********************************************

        uctCLMReceipt.ClaimID = m_lClaimId
        uctCLMReceipt.ClaimPerilId = m_lClaimPerilId

        '**********************************************
        'Developer Guide no. 68
        lReturn = uctCLMReceipt.Load_Renamed()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                MessageBox.Show("No recovery reserves have been setup for this peril.", "Recovery Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                'Modified,form should not be shown if there is no recovery reserves
                'Me.Close()
                Me.Dispose()
            Else
                gPMFunctions.RaiseError(kMethodName, "uctCLMReceipt.Load Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If




        Catch ex As Exception
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub
	
	' ***************************************************************** '
	' Name: ActionOk
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Function ActionOk() As Integer
		
        Dim nResult As Integer = 0
        Const kMethodName As String = "ActionOk"

        Dim nReturnCode As gPMConstants.PMEReturnCode

        Try

        nReturnCode = uctCLMReceipt.ValidateReciept() ' PN 77608, Start
        If nReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
        Else ' PN 77608, End

            nResult = PMEReturnCode.PMTrue

            nReturnCode = uctCLMReceipt.Save()
            If nReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "uctCLMReceipt.Save Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            'S4B Claim Enhancements R&D 2005
            nResult = PMEReturnCode.PMOK


            ' check if no any party selected 
            If uctCLMReceipt.Controls.Find("SSTab1", False)(0).Controls.Find("_SSTab1_TabPage0", False)(0).Controls.Find("fraPayee", False)(0).Controls.Find("txtparty", False)(0) IsNot Nothing Then
                Dim stxt_party As String = uctCLMReceipt.Controls.Find("SSTab1", False)(0).Controls.Find("_SSTab1_TabPage0", False)(0).Controls.Find("fraPayee", False)(0).Controls.Find("txtparty", False)(0).Text
                If String.IsNullOrEmpty(stxt_party) Then
                    MessageBox.Show("No Party is attached with this Claim Recovery", "Third party payer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    Me.Hide()
                End If
            End If

        End If

        Return nResult


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        
        End Try
	Return nResult
	End Function

    Private Sub uctCLMReceipt_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles uctCLMReceipt.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctCLMReceipt.Controls("SSTab1"), TabControl).SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            DirectCast(uctCLMReceipt.Controls("SSTab1"), TabControl).SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            DirectCast(uctCLMReceipt.Controls("SSTab1"), TabControl).SelectedIndex = 2
        End If
    End Sub
End Class
