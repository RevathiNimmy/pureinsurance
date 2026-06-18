Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Developer Guide no. 129
Imports SharedFiles
Friend NotInheritable Class NavigatorV2
    Implements IDisposable
    Implements aPMNav.NavigatorV2
    ' ***************************************************************** '
    ' Class Name: NavigatorV2
    '
    ' Date: 02/07/1998
    '
    ' Description: NavigatorV2 public class to control the Interface class.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "NavigatorV2"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Stores the component Inrterface object
    'developer guide no.108
    Private m_oInterface As Interface_Renamed

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public WriteOnly Property NavigatorV2_CallingAppName() As String Implements aPMNav.NavigatorV2.CallingAppName
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            'm_sCallingAppName$ = sCallingAppName$
            m_oInterface.CallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property NavigatorV2_Status() As Integer Implements aPMNav.NavigatorV2.Status
        Get

            ' Standard Property.

            ' Return the interface exit status.
            'NavigatorV2_Status = m_lStatus&
            Return m_oInterface.Status

        End Get
    End Property

    Public ReadOnly Property NavigatorV2_Task() As Integer Implements aPMNav.NavigatorV2.Task
        Get

            ' Standard Property.

            ' Return the task.
            'NavigatorV2_Task = m_iTask%
            Return m_oInterface.Task

        End Get
    End Property

    Public ReadOnly Property NavigatorV2_Navigate() As Integer Implements aPMNav.NavigatorV2.Navigate
        Get

            ' Standard Property.

            ' Return the navigate flag.
            'NavigatorV2_Navigate = m_lNavigate&
            Return m_oInterface.Navigate

        End Get
    End Property

    Public ReadOnly Property NavigatorV2_ProcessMode() As Integer Implements aPMNav.NavigatorV2.ProcessMode
        Get

            ' Standard Property.

            ' Return the process mode.
            'NavigatorV2_ProcessMode = m_lProcessMode&
            Return m_oInterface.ProcessMode

        End Get
    End Property

    Public ReadOnly Property NavigatorV2_TransactionType() As String Implements aPMNav.NavigatorV2.TransactionType
        Get

            ' Standard Property.

            ' Return the type of business.
            'NavigatorV2_TransactionType = m_sTransactionType$
            Return m_oInterface.TransactionType

        End Get
    End Property

    Public ReadOnly Property NavigatorV2_EffectiveDate() As Date Implements aPMNav.NavigatorV2.EffectiveDate
        Get

            ' Standard Property.

            ' Return the effective date.
            'NavigatorV2_EffectiveDate = m_dtEffectiveDate
            Return m_oInterface.EffectiveDate

        End Get
    End Property

    Public ReadOnly Property NavigatorV2_StepStatus() As String Implements aPMNav.NavigatorV2.StepStatus
        Get

            ' Standard Property.

            ' Return the Steps Status
            'NavigatorV2_StepStatus = m_sStepStatus$
            Return m_oInterface.StepStatus

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
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
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of the Interface class
            'developer guide no.108
            m_oInterface = New Interface_Renamed()

            m_lReturn = CType(CType(m_oInterface, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the Interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
            Me.disposedValue = True
            If disposing Then
                If m_oInterface IsNot Nothing Then
                    m_oInterface.Dispose()

                End If
                m_oInterface = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: NavigatorV2_SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function NavigatorV2_SetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV2.SetKeys

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the corresponding Interface function

            m_lReturn = CType(m_oInterface.SetKeys(vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetKeys failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV2_GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function NavigatorV2_GetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV2.GetKeys

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = CType(m_oInterface.GetKeys(vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.GetKeys failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV2_GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function NavigatorV2_GetSummary(ByRef vSummaryArray As Object) As Integer Implements aPMNav.NavigatorV2.GetSummary

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function

            m_lReturn = CType(m_oInterface.GetSummary(vSummaryArray:=vSummaryArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.GetSummary failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV2_SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function NavigatorV2_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements aPMNav.NavigatorV2.SetProcessModes

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = CType(m_oInterface.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetProcessModes failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV2_SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function NavigatorV2_SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer Implements aPMNav.NavigatorV2.SetStatus

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = CType(m_oInterface.SetStatus(sProcessStatus:=sProcessStatus, sMapStatus:=sMapStatus, sStepStatus:=sStepStatus), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetStatus failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV2_Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function NavigatorV2_Start() As Integer Implements aPMNav.NavigatorV2.Start

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = CType(m_oInterface.Start(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.Start failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class