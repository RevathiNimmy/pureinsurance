Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: 10/05/1999
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' DJM 01/05/2002 : Only merge text file if printing check now done in GetInterfaceDetails instead of frmInterface.
    ' ***************************************************************** '



    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "General"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Private instance of the interface form.
    Private m_frmInterface As iPMBTextFile.frmInterface

    ' Private instance of the business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRTextFile.Business

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
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
    Public Function Initialise(ByRef frmInterface As iPMBTextFile.frmInterface, ByRef oBusiness As Object) As Integer

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
            If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMEdit Or m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView Or m_frmInterface.Task = gPMConstants.PMEComponentAction.PMDelete Then
                ' Get the interface details from the
                ' business object.
                m_lReturn = CType(m_frmInterface.GetBusiness(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object
                ' to the interface.
                m_lReturn = CType(m_frmInterface.BusinessToInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Display all of the lookup details.
            m_lReturn = CType(m_frmInterface.DisplayLookupDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Copy the document.
            m_lReturn = m_frmInterface.CopyServerToClient()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then 'MKW281003 PN7287 1.8.5 to 1.8.6 Catchup
                Return m_lReturn
            End If

            'DJM 24/04/2002 : Only merge if printing
            If m_frmInterface.Task = 20 Then
                ' Merge the document.
                m_lReturn = m_frmInterface.MergeDocument()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display the document.
            m_lReturn = m_frmInterface.SetBrowser()

            ' Check the task.
            If (m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView) Or (m_frmInterface.Task = gPMConstants.PMEComponentAction.PMDelete) Then
                ' Disable the interface to only allow viewing.
                m_lReturn = CType(DisableForm(lDisabled:=True), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
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
            Select Case (m_frmInterface.Task)
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                    If m_frmInterface.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                        ' Update the business from the interface.
                        m_lReturn = CType(m_frmInterface.InterfaceToBusiness(), gPMConstants.PMEReturnCode)

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update business.
                            Return gPMConstants.PMEReturnCode.PMFalse
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

                        '                m_lReturn = m_oBusiness.CheckCode(vCode:=m_frmInterface.FileNumber)
                        '
                        '                If (m_lReturn <> PMNotFound) Then
                        '                    sTitle$ = GetResData( _
                        ''                        iLangID:=g_iLanguageID%, _
                        ''                        lID:=ACAddDetailsTitle, _
                        ''                        iDataType:=PMResString)
                        '
                        '                    sMessage$ = GetResData( _
                        ''                        iLangID:=g_iLanguageID%, _
                        ''                        lID:=ACAddDetails, _
                        ''                        iDataType:=PMResString)
                        '
                        '                    MsgBox sMessage$, vbOKOnly, sTitle$
                        '
                        '                    ProcessCommand = PMFalse
                        '
                        '                    Exit Function
                        '                End If

                        ' Add the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If


                        m_frmInterface.FileNumber = m_oBusiness.FileNumber

                    End If

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Check the details havn't changed.

                        m_lReturn = m_oBusiness.Cancel()

                        If m_lReturn = gPMConstants.PMEReturnCode.PMDataChanged Then
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
                        End If
                    Else

                        ' Update the details using the business object.
                        '                m_lReturn& = m_oBusiness.Update()
                        '
                        '                ' Check for errors.
                        '                If (m_lReturn& <> PMTrue) Then
                        '                   ' Failed to update the details
                        '                   ProcessCommand = PMFalse
                        '
                        '                   ' Log Error.
                        '                   LogMessage _
                        ''                       iType:=PMLogError, _
                        ''                       sMsg:="Failed to update the details", _
                        ''                       vApp:=ACApp, _
                        ''                       vClass:=ACClass, _
                        ''                       vMethod:="ProcessCommand"
                        '                End If
                        '
                    End If

                Case gPMConstants.PMEComponentAction.PMDelete
                    If m_frmInterface.Status <> gPMConstants.PMEReturnCode.PMCancel Then
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
            End If
        Next ctlFormControl

        '    m_frmInterface.OLE1.Enabled = Not lDisabled
        m_frmInterface.cmdEdit.Enabled = Not lDisabled

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

