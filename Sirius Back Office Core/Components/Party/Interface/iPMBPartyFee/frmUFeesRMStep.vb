Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Partial Friend Class frmUFeesRMStepInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:
    '
    ' Description: Main interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    '********************************
    ' General Property variables
    Private m_sCallingAppName As String = ""
    Private m_iTask As Integer
    Private m_lProcessMode As Integer
    Private m_lNavigate As Integer
    Private m_dtEffectiveDate As Date
    Private m_lTransactionType As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lPMAuthorityLevel As Integer
    Private m_bError As Integer
    Private m_oBusiness As Object
    Private m_lReturn As Integer
    Private m_bInterfaceError As Boolean
    '********************************

    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskCnt As Integer
    Private m_sTransactionType As String = ""

    '********************************
    ' General Interface Properties
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property
    Public ReadOnly Property Error_Renamed() As Integer
        Get
            Return m_bError
        End Get
    End Property
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public WriteOnly Property RiskCnt() As Integer
        Set(ByVal Value As Integer)
            m_lRiskCnt = Value
        End Set
    End Property
    Public WriteOnly Property Task() As Integer
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
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        Me.Hide()
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Initialize
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process Id
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        Const sFunction As String = "Form_Initialize"

        Try

            ' initialise form error indicator
            m_bError = False

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=excep)

            ' reset mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub

        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: Form_QueryUnload
    '
    ' Description: Determines whether any actions need to take place
    '               before unload.
    ' History:
    '           Created : MEvans : Date : Process Id
    ' ***************************************************************** '
    Private Sub frmUFeesRMStepInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)


        eventArgs.Cancel = Cancel <> 0
    End Sub
    ' ***************************************************************** '
    ' Name: Form_Unload
    '
    ' Description: Destroys all object references
    '
    ' History:
    '           Created : MEvans : Date : Process Id
    ' ***************************************************************** '
    Private Sub frmUFeesRMStepInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        Const sFunctionName As String = "Form_Unload"


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        'm_bInterfaceError = True
        '
        '******************************
        ' Log Error.
        'gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '*******************************
        '
        'Exit Sub
        '
        'End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Form_Load
    '
    ' Parameters: N/A
    '
    ' Description: Sets up the form, populates controls, etc
    '
    ' History:
    '           Created : MEvans : Date : Process Id
    ' ***************************************************************** '

    Private Sub frmUFeesRMStepInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const sFunctionName As String = "Form_Load"

        Try

            ' set up interface
            m_lReturn = SetUpForm()

        Catch excep As System.Exception



            m_bInterfaceError = True

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SetUpForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetUpForm() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SetUpForm"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            uctPMUFees1.Initialise()
            uctPMUFees1.SetProcessModes(m_iTask, , , m_sTransactionType, DateTime.Now)
            uctPMUFees1.ReadOnly_Renamed = False
            uctPMUFees1.InsuranceFileCnt = m_lInsuranceFileCnt
            uctPMUFees1.RiskCnt = m_lRiskCnt
            uctPMUFees1.Recalculate()

            txtProduct.Text = uctPMUFees1.AppliesTo

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    Private Sub frmUFeesRMStepInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class