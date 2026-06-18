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
    ' Date: 04/03/1997
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "General"


    ' Private instance of the interface form.
    'Modified as make it latebinding
    'Private m_frmInterface As Form
    Private m_frmInterface As Object
    ' Private instance of the business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer


    ' ***************************************************************** '
    ' Entry point for any initialisation code for this object.
    ' ***************************************************************** '
    Public Function Initialise(ByRef ofrmInterface As Form, ByRef oBusiness As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the form into the member.
            m_frmInterface = ofrmInterface
            'Set m_frmInterface = frmInterfaceRI2007
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
    ' Entry point for any termination code for this object.
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
                m_frmInterface = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Gets the interface details and sets the appropriate style.
    ' ***************************************************************** '
    Public Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            ' Edit mode also needs to call process auto ri in case this claim
            ' was originally made against a risk with deferred reinsurance

            If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMAdd Or m_frmInterface.Task = gPMConstants.PMEComponentAction.PMEdit Then

                m_lReturn = m_frmInterface.ProcessAutoRI()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If


                m_frmInterface.Task = gPMConstants.PMEComponentAction.PMEdit
            End If


            If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMEdit Or m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
                ' Get the interface details from the business object.

                m_lReturn = m_frmInterface.GetBusiness()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                ' Assign the details from the business object to the interface.

                m_lReturn = m_frmInterface.BusinessToInterface()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Check the task.

            If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(lDisabled:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMError
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
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.


            Select Case (m_frmInterface.Task)
                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so, check if the details
                    ' have changed and if so, prompt if they wish to cancel.

                    If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Check the details havn't changed.

                        m_lReturn = m_oBusiness.Cancel()
                        If m_lReturn = gPMConstants.PMEReturnCode.PMDataChanged Then
                            ' Get string messages

                            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            ' Check message result.
                            If MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                                ' Set return to false, meaning don't cancel.
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If



                        m_lReturn = m_oBusiness.TidyUpAfterCancel(v_lClaimId:=m_frmInterface.ClaimID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to tidy up...
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to tidy up work data following user cancel", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    Else
                        ' Update the details using the business object.

                        m_lReturn = m_oBusiness.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If
            End Select

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMTrue

            'Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Sets all of the interface details to the disable state passed.
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set all of the forms controls to the disable state.

        For Each ctlFormControl As Control In ContainerHelper.Controls(m_frmInterface)
            ' Check the type of the control.
            If TypeOf ctlFormControl Is TextBox Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                '        ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                '            ctlFormControl.Enabled = Not lDisabled
            ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            End If
        Next ctlFormControl

        Return result

    End Function


    ' ***************************************************************** '
    '                           CLASS EVENTS
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()
        Exit Sub
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

