Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance

    ' ***************************************************************** '
    ' Form Name: frmInterface

    ' Description: Main interface.
    ' ***************************************************************** '
    'developer guide no.7
    Public Const vbFormCode As Integer = 0

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)


    Private m_lClaimId As Integer
    Private m_lRecoveryID As Integer
    Private m_sRecoveryText As String = ""
    Private m_vReceiptList As Object
    Private m_vRecoveryType As Integer
    Private m_lClaimPerilId As Integer

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMListReceipts.General

    ' Declare an instance of the Business object.

    Private m_oBusiness As bCLMPeril.Business

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast() As Control
    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PRIVATE Property Procedures (Begin)


    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
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
    Public WriteOnly Property RecoveryID() As Integer
        Set(ByVal Value As Integer)
            m_lRecoveryID = Value
        End Set
    End Property

    Public WriteOnly Property RecoveryText() As String
        Set(ByVal Value As String)
            m_sRecoveryText = Value
        End Set
    End Property
    Public WriteOnly Property RecoveryType() As Integer
        Set(ByVal Value As Integer)
            m_vRecoveryType = Value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetFieldValidation"
        Try
            result = gPMConstants.PMEReturnCode.PMTrue



        Catch ex As Exception

            ' LogError.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(kMethodName), excep:=ex)

        Finally


        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            ' Get the details from the business object.


            uctCLMListReceiptsC1.CountColumn = 10
            uctCLMListReceiptsC1.ColumnCaption(0) = "Receipt_id"
            uctCLMListReceiptsC1.ColumnCaption(1) = "Date"
            uctCLMListReceiptsC1.ColumnCaption(2) = "Party Received"
            uctCLMListReceiptsC1.ColumnCaption(3) = "Payer"
            uctCLMListReceiptsC1.ColumnCaption(4) = "Amount"
            uctCLMListReceiptsC1.ColumnCaption(5) = "TaxAmount"
            uctCLMListReceiptsC1.ColumnCaption(6) = "Currency"
            uctCLMListReceiptsC1.ColumnCaption(7) = "Loss Amount"
            uctCLMListReceiptsC1.ColumnCaption(8) = "Base Amount"
            uctCLMListReceiptsC1.ColumnCaption(9) = "Media Ref."

            uctCLMListReceiptsC1.ClaimId = m_lClaimId
            uctCLMListReceiptsC1.RecoveryID = m_lRecoveryID
            uctCLMListReceiptsC1.RecoveryType = m_vRecoveryType


            m_lReturn = uctCLMListReceiptsC1.GetBusiness()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Call To GetBusiness of uctCLMListReceipts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' LogError.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetStatus"
        Try
            result = gPMConstants.PMEReturnCode.PMTrue



            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

        Catch ex As Exception

            ' LogError.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(kMethodName), excep:=ex)

        Finally



        End Try
        Return result
    End Function
    ' PUBLIC Methods (End)

    'Private Sub cmdViewPayment_Click()
    'ActionViewReceipt()
    'End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        Const kMethodName As String = "Form_Initialize"
        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMPeril.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialize bCLMPeril.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrencyConvert = temp_m_oCurrencyConvert

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialize bACTCurrencyConvert.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMListReceipts.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialize iCLMListReceipts.frmInterface", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' LogError.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"

        Try


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

                gPMFunctions.RaiseError(kMethodName, "SetProcessmodes method failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "GetInterfaceDetails Method Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "SetFieldValidation Method Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' LogError.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)
        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
        Exit Sub
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Const kMethodName As String = "Form_QueryUnload"
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

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

            ' Terminate the general object.
            m_oGeneral.Dispose()

            

            ' Destroy the instance of the general object
            ' from memory.


            ' Terminate the business object

            m_oBusiness.Dispose()

           


            ' Terminate the business object

            m_oCurrencyConvert.Dispose()

           



            ' Terminate the form control object.
            m_oFormFields.Dispose()

           


            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)



        Catch ex As Exception

            ' LogError.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)
        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



            eventArgs.Cancel = Cancel <> 0
        End Try
        Exit Sub
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Process the next set of actions depending
        ' upon the interface task etc.
        Const kMethodName As String = "cmdOk_Click"
        m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

        ' Check the return value.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Everything OK, so we can hide the interface.
            gPMFunctions.RaiseError(kMethodName, "Method ProcessCommand Failed", gPMConstants.PMELogLevel.PMLogError)

        End If
        Me.Hide()

    End Sub

    ' PRIVATE Events (End)

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        Dim lWidth, lHeight As Integer
        If Me.WindowState <> FormWindowState.Minimized Then
            If VB6.PixelsToTwipsX(Me.Width) < 11295 Then
                Me.Width = VB6.TwipsToPixelsX(11295)
            End If

            If VB6.PixelsToTwipsY(Me.Height) < 5505 Then
                Me.Height = VB6.TwipsToPixelsY(5505)
            End If

            lWidth = CInt(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - (VB6.PixelsToTwipsX(tabMainTab.Left) * 2))
            lHeight = CInt(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - (VB6.PixelsToTwipsY(tabMainTab.Top) * 3) - VB6.PixelsToTwipsY(cmdOK.Height))

            tabMainTab.Width = VB6.TwipsToPixelsX(lWidth)
            tabMainTab.Height = VB6.TwipsToPixelsY(lHeight)

            'resize usercontrol according to form. ClaimsSummary
            '------------------------------------------Claim Summary
            uctCLMListReceiptsC1.Width = tabMainTab.Width - VB6.TwipsToPixelsX(250)
            uctCLMListReceiptsC1.Height = VB6.TwipsToPixelsY(lHeight - 450)
            '------------------------------------------Claim Summary
            cmdOK.Left = Me.ClientRectangle.Width - cmdOK.Width - tabMainTab.Left
            cmdOK.Top = Me.ClientRectangle.Height - cmdOK.Height - tabMainTab.Top
        End If

    End Sub


    ' ***************************************************************** '
    ' Name: ActionViewPayment
    '
    ' ***************************************************************** '
    Public Function ActionViewReceipt() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionViewReceipt"

        Dim lReturn As Integer
        Dim oReceiptItem As frmReceiptItem
        Dim A As Integer
        'developer guide no.108
        Dim oInterface As New Interface_Renamed

        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            If uctCLMListReceiptsC1.selectedItem = 0 Then
                MessageBox.Show("A payment item must be selected", "View Payment Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else

                oReceiptItem = New frmReceiptItem()

                oReceiptItem.ClaimId = m_lClaimId
                oReceiptItem.ClaimPerilId = m_lClaimPerilId
                oReceiptItem.ClaimReceiptId = uctCLMListReceiptsC1.selectedItem
                oReceiptItem.Task = m_iTask
                oReceiptItem.EffectiveDate = m_dtEffectiveDate
                oReceiptItem.TransactionType = m_sTransactionType


                oInterface.WindowHWND = oReceiptItem.Handle.ToInt32()
                oInterface.KeepWindowOnTop(True)
                oInterface = Nothing


                oReceiptItem.ShowDialog()
                oReceiptItem = Nothing
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        'developer guide no.293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
