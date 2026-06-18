Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles

Friend NotInheritable Class General
    Implements IDisposable

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "General"

#Region "Private Variables"

    ' Private instance of the business object.
    Private m_oBusiness As Object

    Private m_ofrmInterface As Object

    Private bDisposedValue As Boolean

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Create Object of this class
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Entry point for any initialisation code for this object
    ''' </summary>
    ''' <param name="frmInterface"></param>
    ''' <param name="oBusiness"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise(ByRef frmInterface As Form, ByRef oBusiness As Object) As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Store the instance of the form into the member.
            m_ofrmInterface = frmInterface

            ' Store the instance of the business object
            ' into the member.
            m_oBusiness = oBusiness
        Catch Excep As Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Gets the interface details and sets the appropriate style
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInterfaceDetails() As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try

            If m_ofrmInterface.Task = gPMConstants.PMEComponentAction.PMEdit Or m_ofrmInterface.Task = gPMConstants.PMEComponentAction.PMView Then

                ' Get the interface details from the business object.
                nResult = m_ofrmInterface.GetBusiness
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object to the interface.
                nResult = m_ofrmInterface.BusinessToInterface()
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Check the task.
            If m_ofrmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                nResult = CType(DisableForm(lDisabled:=True), gPMConstants.PMEReturnCode)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        Catch Excep As Exception

            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Determines which action to take on the details
    '''      depending upon the task and interface state.
    ''' </summary>
    ''' <param name="r_bChangesMade"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessCommand(ByRef r_bChangesMade As Boolean) As Integer

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Dim oMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try
            'If we're viewing there's nothing to update or cancel
            If m_ofrmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
                Return nResult
            End If

            ' Check the task.
            If m_ofrmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                If r_bChangesMade Then
                    ' Update the business from the interface.

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_nLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_nLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    oMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                    ' Check message result.
                    If oMsgResult = System.Windows.Forms.DialogResult.No Then
                        ' Set return to false, meaning don't cancel.
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Else
                nResult = m_ofrmInterface.InterfaceToBusiness()
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFail
                    Return nResult
                End If
            End If
        Catch Excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Entry point for any termination code for this object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "Private/ Protected Methods"

    ''' <summary>
    ''' Sets all of the interface details to the disable state passed.
    ''' </summary>
    ''' <param name="lDisabled"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue

        ' Set all of the forms controls to the disable state.
        For Each ctlFormControl As Control In ContainerHelper.Controls(m_ofrmInterface)
            ' Check the type of the control.
            If TypeOf ctlFormControl Is TextBox Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (TypeOf ctlFormControl Is GroupBox) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (TypeOf ctlFormControl Is PMLookupControl.cboPMLookup) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            ElseIf (TypeOf ctlFormControl Is UserControls.BankAccount) Then
                ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
            End If
        Next ctlFormControl

        Return nResult

    End Function

    ''' <summary>
    ''' Call Dispose on Finalise
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ''' <summary>
    ''' Object distruction during Dispose
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Sub Dispose(disposing As Boolean)
        If Not Me.bDisposedValue Then
            If disposing Then
                m_oBusiness = Nothing
                m_ofrmInterface = Nothing
            End If
        End If
        Me.bDisposedValue = True
    End Sub

#End Region

End Class

