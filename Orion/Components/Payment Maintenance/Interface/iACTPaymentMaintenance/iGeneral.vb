Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class General

    Implements IDisposable
    Private Const ACClass As String = "General"
    ' Private instance of the interface form.
    Private m_frmInterface As iACTPaymentMaintenance.frmInterface
    ' Private instance of the business object.
    Private m_oBusiness As Object
    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef frmInterface As Form, ByRef oBusiness As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the form into the member.
            m_frmInterface = frmInterface

            ' Store the instance of the business object
            ' into the member.
            m_oBusiness = oBusiness


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
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
        Const kMethodName As String = "GetInterfaceDetails"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the business object.
            'if object is nothing 
            If IsNothing(m_frmInterface) Then
                m_frmInterface = New iACTPaymentMaintenance.frmInterface
            End If
            m_lReturn = m_frmInterface.PerformSearch()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to Process PerformSearch")
            End If

            ' Assign the details from the search data storage
            ' to the interface.
            m_lReturn = m_frmInterface.DataToInterface()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to Process DataToInterface")
            End If


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
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

        Const kMethodName As String = "ProcessCommand"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.
            If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                ' Get string messages

                iMsgResult = MessageBox.Show("Cancelling will not return any search details " & Strings.Chr(13) & Strings.Chr(10) & _
                             "Do you really wish to cancel?", "Cancel Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Set return to false, meaning
                    ' don't cancel.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Update the property member from the interface.
                m_lReturn = m_frmInterface.DataToProperties()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Raise Error.
                    gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to Process DataToProperties")
                End If
            End If


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        Try


        Catch ex As Exception
            ' Error Section.
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface general class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
