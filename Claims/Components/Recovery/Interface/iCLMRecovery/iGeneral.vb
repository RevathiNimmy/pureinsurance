Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: 02/07/1998
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "General"

    ' Private instance of the interface form.
    Private m_frmRecovery As frmRecovery

    ' Private instance of the business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef frmRecovery As Form, ByRef oBusiness As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the form into the member.
            m_frmRecovery = frmRecovery

            ' Store the instance of the business object
            ' into the member.
            m_oBusiness = oBusiness

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
                m_oBusiness = Nothing
                m_frmRecovery = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Public Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the business object.
            m_lReturn = m_frmRecovery.GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details from the business object to the interface.
            m_lReturn = m_frmRecovery.BusinessToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Const kMethodName As String = "ProcessCommand"
        Dim lReturn As Integer

        Dim sTitle, sMessage As String

        Dim lReceiptPartyCnt As Integer
        Dim sAccountCode, sMappingCode, sReceiptComments As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if form has been cancelled, if so, prompt if they wish to cancel.
            If m_frmRecovery.Status = gPMConstants.PMEReturnCode.PMCancel Then
                ' Get message captions

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Check message result.
                If MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                    ' Set return to false, meaning
                    ' don't cancel.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Check the task.
                If m_frmRecovery.Task <> gPMConstants.PMEComponentAction.PMView Then
                    ' If we are in receipt mode there are a few more things to do
                    If g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReceipt Or g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReceipt Then
                        ' Add receipt and payment records as they are required by payment methods
                        ' screen for currency rate updates

                        lReturn = g_oBusiness.AddReceiptAndPayments()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("g_oBusiness.AddReceiptAndPayments()", "Failed to add receipts and payments")
                        End If

                        ' Get posting details for the receipt
                        lReturn = m_frmRecovery.GetReceiptPostingDetails(lReceiptPartyCnt, sAccountCode, sMappingCode, sReceiptComments)
                        If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                            ' If user cancelled delete the temporary receipts and payments

                            lReturn = g_oBusiness.DeleteReceiptAndPayments()
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("g_oBusiness.DeleteReceiptAndPayments()", "Unable to delete receipts and payments")
                            End If
                            result = gPMConstants.PMEReturnCode.PMCancel
                            Return result
                        ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("GetReceiptPostingDetails()", "Failed to get receipt posting information")
                        End If
                    End If

                    ' Add the details using the business object.

                    m_lReturn = g_oBusiness.Update()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("g_oBusiness.Update()", "Failed to update business details")
                    End If

                    ' If we are in receipt mode there are a few more things to do
                    If g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReceipt Or g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReceipt Then
                        ' Post the receipt

                        lReturn = g_oBusiness.PostReceipt(bIsSalvage:=m_frmRecovery.IsSalvage, lInsuranceFileCnt:=m_frmRecovery.InsuranceFileCnt, lClaimId:=m_frmRecovery.ClaimId, lPerilId:=m_frmRecovery.PerilID, lReceiptPartyCnt:=lReceiptPartyCnt, sAccountCode:=sAccountCode, sMappingCode:=sMappingCode, sReceiptComments:=sReceiptComments, lCOBId:=m_frmRecovery.ClassOfBusinessID, sCOBCode:=m_frmRecovery.ClassOfBusiness)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("g_oBusiness.PostReceipt()", "Failed to post receipts")
                        End If

                        ' Check if we can close the claim
                        lReturn = m_frmRecovery.CheckCurrentReserve()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("CheckCurrentReserve()", "Failed to check current reserve")
                        End If
                    End If
                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMError

        Finally

            '		Return result


            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '

    'Private Function DisableForm(ByRef lDisabled As Integer) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Set all of the forms controls to the disable state.

    'For	Each ctlFormControl As Control In ContainerHelper.Controls(m_frmRecovery)
    ' Check the type of the control.
    'If TypeOf ctlFormControl Is TextBox Then
    'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
    'ElseIf (TypeOf ctlFormControl Is ComboBox) Then 
    'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
    'ElseIf (TypeOf ctlFormControl Is CheckBox) Then 
    'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
    'End If
    'Next ctlFormControl
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface general class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
