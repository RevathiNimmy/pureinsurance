Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide No: 129
Imports SharedFiles


Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    ' Date: {TodaysDate}
    ' Description: General class to accompany the interface form.
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "General"
    ' Private instance of the interface form.

    'developer guide no.291
    Private m_frmInterface As Object
    ' Private instance of the business object.
    Private m_oBusiness As Object
    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef frmInterface As Object, ByRef oBusiness As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the form into the member.
            m_frmInterface = frmInterface

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
                m_frmInterface = Nothing
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

            ' Check the task.

            If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMEdit Or m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
                ' Get the interface details from the
                ' business object.
                'm_lReturn& = m_frmInterface.GetBusiness()

                ' Check for errors.
                '        If (m_lReturn& <> PMTrue) Then
                '            ' Failed to get the details.
                '            GetInterfaceDetails = PMFalse
                '            Exit Function
                '        End If

                ' Assign the details from the business object
                ' to the interface.

                m_lReturn = m_frmInterface.BusinessToInterface()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Display all of the lookup details.

            m_lReturn = m_frmInterface.DisplayLookupDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the task.


            If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = CType(DisableForm(lDisabled:=True), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If


                m_frmInterface.txtComment(0).Enabled = True

                'Developer Guide no. 154
                m_frmInterface.txtComment(0).ReadOnly = True

                m_frmInterface.txtComment(0).ForeColor = System.Drawing.Color.Gray



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
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        'Developer Guide No. 50
        Dim frmInterface As frmInterface = New frmInterface

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.

            Select Case (m_frmInterface.Task)
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit

                    If m_frmInterface.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                        ' Update the business from the interface.

                        m_lReturn = m_frmInterface.InterfaceToBusiness()
                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update business.
                            Return result
                        End If
                    End If
            End Select

            ' Check the task.

            Select Case (m_frmInterface.Task)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.

                    If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_oObjectManager.LanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_oObjectManager.LanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse

                        End If
                    End If

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.

                    If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Check the details havn't changed.

                        m_lReturn = m_oBusiness.Cancel()
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_oObjectManager.LanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_oObjectManager.LanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
            End Select

            If frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then

                If frmInterface.DeleteWorkTableFlag = gPMConstants.PMEReturnCode.PMTrue Then

                    If Not frmInterface.ViewRiskFlag Then

                        If frmInterface.Task <> gPMConstants.PMEComponentAction.PMView Then

                            ' Tidy up - delete work table entries, unlock claim...now moved to business component so can
                            ' be called from other components...

                            m_lReturn = m_oBusiness.TidyUpAfterCancel(v_lClaimId:=frmInterface.Claimid, v_lclaimmode:=frmInterface.ClaimMode)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed when calling TidyUpAfterCancel", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function
    'PUBLIC Methods (End)


    'PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set all of the forms controls to the disable state.

        For Each ctlFormControl As Control In ContainerHelper.Controls(m_frmInterface)
            ' Check the type of the control.
            If TypeOf ctlFormControl Is TextBox Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (TypeOf ctlFormControl Is RadioButton) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (ctlFormControl.Name = "uctCLMParty") Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (ctlFormControl.Name = "cmdPerilDelete") Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (ctlFormControl.Name = "txtText") Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (ctlFormControl.Name = "cmbLookup") Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (ctlFormControl.Name = "txtInteger") Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (ctlFormControl.Name = "txtDate") Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (ctlFormControl.Name = "cmdPerilAdd") Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (ctlFormControl.Name = "chkCheck") Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                '        ElseIf (ctlFormControl.Name = "Toolbar1") Then
                '            ctlFormControl.Enabled = Not lDisabled&
            ElseIf (ctlFormControl.Name = "uctCLMParty") Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            End If
        Next ctlFormControl
        Return result

    End Function
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

