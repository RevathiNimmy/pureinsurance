Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No 129
Imports SharedFiles
Imports Artinsoft.VB6.Utils
Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: 08/09/1998
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "General"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Private instance of the interface form.
    Private m_frmInterface As Form

    ' Private instance of the business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef frmInterface As Form, ByRef oBusiness As Object) As Integer

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

            'NIIT - Replaced with the Migrated code 1144 
            If ReflectionHelper.GetMember(m_frmInterface, "Task") = gPMConstants.PMEComponentAction.PMEdit Or ReflectionHelper.GetMember(m_frmInterface, "Task") = gPMConstants.PMEComponentAction.PMView Then
                'If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMEdit Or m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
                ' Get the interface details from the
                ' business object.

                'NIIT - Replaced with the Migrated code 1144 
                m_lReturn = ReflectionHelper.Invoke(m_frmInterface, "GetBusiness", New Object() {})
                'm_lReturn = m_frmInterface.GetBusiness()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return m_lReturn
                End If

                ' Assign the details from the business object
                ' to the interface.

                'NIIT - Replaced with the Migrated code 1144 
                'm_lReturn = m_frmInterface.BusinessToInterface()
                m_lReturn = ReflectionHelper.Invoke(m_frmInterface, "BusinessToInterface", New Object() {})


                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            ' Check the task.

            'NIIT - Replaced with the Migrated code 1144 
            'If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
            If ReflectionHelper.GetMember(m_frmInterface, "Task") = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(lDisabled:=True)

            Else
                'Enable form
                m_lReturn = DisableForm(lDisabled:=False)
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

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.

            'NIIT - Replaced with the Migrated code 1144 
            'Select Case (m_frmInterface.Task)
            Select Case (ReflectionHelper.GetMember(m_frmInterface, "Task"))
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit

                    'NIIT - Replaced with the Migrated code 1144 
                    'If m_frmInterface.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                    If ReflectionHelper.GetMember(m_frmInterface, "Status") <> gPMConstants.PMEReturnCode.PMCancel Then
                        ' Update the business from the interface.

                        'NIIT - Replaced with the Migrated code 1144 
                        'm_lReturn = m_frmInterface.InterfaceToBusiness()
                        m_lReturn = ReflectionHelper.Invoke(m_frmInterface, "InterfaceToBusiness", New Object() {})

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update business.
                            Return result
                        End If
                    End If
            End Select

            ' Check the task.

            'NIIT - Replaced with the Migrated code 1144 
            'Select Case (m_frmInterface.Task)
            Select Case (ReflectionHelper.GetMember(m_frmInterface, "Task"))
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.

                    'NIIT - Replaced with the Migrated code 1144 
                    'If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    If ReflectionHelper.GetMember(m_frmInterface, "Status") = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Form hasn't been cancelled, so we just go
                        ' ahead and add the details.


                        ' Add the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If

                        ' MSS210601 - Added Events for created notes
                        'late binding to be used as the variable is declared as Form
                        'm_lReturn = frmInterface.CreateEvents()
                        m_lReturn = ReflectionHelper.Invoke(m_frmInterface, "CreateEvents", New Object() {})

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the event", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If

                    End If

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.

                    'NIIT - Replaced with the Migrated code 1144 
                    'If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    If ReflectionHelper.GetMember(m_frmInterface, "Status") = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Check the details havn't changed.

                        m_lReturn = m_oBusiness.Cancel()
                        'MH Request - Allways confirm cancellation
                        '                If (m_lReturn& = PMDataChanged) Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        '               End If
                    Else
                        ' Update the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    'Private Function DisableForm(ByRef lDisabled As Integer) As Integer
    Private Function DisableForm(ByRef lDisabled As Boolean) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set all of the forms controls to the disable state.

        For Each ctlFormControl As Control In ContainerHelper.Controls(m_frmInterface)
            ' Check the type of the control.
            If TypeOf ctlFormControl Is TextBox Then

                'NIIT - Replaced with the Migrated code 1144 
                'developer guide no 154
                ReflectionHelper.SetMember(ctlFormControl, "ReadOnly", lDisabled)
                'ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                '    ctlFormControl.Locked = lDisabled&
                'ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                '    ctlFormControl.Locked = lDisabled&
                'ElseIf (TypeOf ctlFormControl Is SSOption) Then
                '    ctlFormControl.Locked = lDisabled&
            End If
        Next ctlFormControl

        Return result

    End Function
    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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