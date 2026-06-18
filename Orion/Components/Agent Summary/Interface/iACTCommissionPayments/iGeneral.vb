Option Strict Off
Option Explicit On
Imports SharedFiles
Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: 02/09/1999
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "General"


    ' Private instance of the interface form.
    Private m_frmInterface As iACTCommissionPayments.frmInterface

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
    Public Function Initialise(ByRef frmInterface As System.Windows.Forms.Form) As Integer
        ', _
        'oBusiness As Object

        Try

            Initialise = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the form into the member.
            m_frmInterface = frmInterface

            ' Store the instance of the business object
            ' into the member.
            'Set m_oBusiness = oBusiness

            Exit Function

        Catch ex As Exception

            ' Error Section.

            Initialise = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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

        Try

            GetInterfaceDetails = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the business object.
            m_lReturn = m_frmInterface.PerformSearch()

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get the details.
                GetInterfaceDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Assign the details from the search data storage
            ' to the interface.
            m_lReturn = m_frmInterface.DataToInterface()

            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to assign the details.
                GetInterfaceDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception

            ' Error Section.

            GetInterfaceDetails = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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

        Dim iMsgResult As Short
        Dim sMessage As String
        Dim sTitle As String

        Try

            ProcessCommand = gPMConstants.PMEReturnCode.PMTrue

            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.
            If (m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel) Then
                ' Get string messages

                'UPGRADE_WARNING: Couldn't resolve default property of object GetResData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sTitle = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)

                'UPGRADE_WARNING: Couldn't resolve default property of object GetResData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sMessage = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)

                iMsgResult = MsgBox(sMessage, MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2 + MsgBoxStyle.Question, sTitle)

                ' Check message result.
                If (iMsgResult = MsgBoxResult.No) Then
                    ' Set return to false, meaning
                    ' don't cancel.
                    ProcessCommand = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Update the property member from the interface.
                m_lReturn = m_frmInterface.DataToProperties()

                ' Check for errors.
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ' Failed to update business.
                    Exit Function
                End If
            End If

            Exit Function

        Catch ex As Exception

            ' Error Section.

            ProcessCommand = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function
    'PUBLIC Methods (End)
    '
    '
    ''PRIVATE Methods (Begin)
    ''PRIVATE Methods (End)
    '
    '
    'Private Sub Class_Initialize()
    '
    '    ' Class Initialise Event.
    '
    '    On Error GoTo Err_ClassInitialise
    '
    '    Exit Sub
    '
    'Err_ClassInitialise:
    '
    '    ' Error Section.
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to initialise the interface general class", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="Class_Initialise", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub
End Class