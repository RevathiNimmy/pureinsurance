Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System

Imports SharedFiles
<System.Runtime.InteropServices.ProgId("NavigatorV3_NET.NavigatorV3")> _
Public NotInheritable Class NavigatorV3
    Implements IDisposable
    Implements aPMNav.NavigatorV3
    ' ***************************************************************** '
    ' Class Name: NavigatorV3
    '
    ' Date: 25/08/1998
    '
    ' Description: NavigatorV3 public class to control the Interface class.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "NavigatorV3"
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

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' XMLFileName
    Private m_sXMLFileName As String = ""

    ' Stores the component Inrterface object

    Private WithEvents m_oInterface As Interface_Renamed

    ' PUBLIC Events (Begin)
    Public Event SetProcessStatus(ByVal v_bProcessComplete As Boolean)
    Public Event NavigatorClose()
    ' PUBLIC Events (End)
    Public Property XMLFileName() As String
        Get

            Try
                Return m_oInterface.XMLFileName

            Catch excep As System.Exception

                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Get XMLFileName property", vApp:=ACApp, vClass:=ACClass, vMethod:="Get XMLFileName", excep:=excep)

            End Try
        End Get
        Set(ByVal Value As String)

            Try

                m_oInterface.XMLFileName = Value

            Catch excep As System.Exception

                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Let XMLFileName property", vApp:=ACApp, vClass:=ACClass, vMethod:="Let XMLFileName", excep:=excep)

            End Try

        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public WriteOnly Property NavigatorV3_CallingAppName() As String Implements aPMNav.NavigatorV3.CallingAppName
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            'm_sCallingAppName$ = sCallingAppName$
            m_oInterface.CallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property NavigatorV3_PMAuthorityLevel() As Integer Implements aPMNav.NavigatorV3.PMAuthorityLevel
        Set(ByVal Value As Integer)

        End Set
    End Property

    Public ReadOnly Property NavigatorV3_Status() As Integer Implements aPMNav.NavigatorV3.Status
        Get

            ' Standard Property.

            ' Return the interface exit status.
            'NavigatorV3_Status = m_lStatus&
            Return m_oInterface.Status

        End Get
    End Property

    Private Sub m_oInterface_NavigatorClose() Handles m_oInterface.NavigatorClose

        ' Pass the event on
        RaiseEvent NavigatorClose()

    End Sub

    Private Sub m_oInterface_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oInterface.SetProcessStatus

        ' Pass the event on
        RaiseEvent SetProcessStatus(v_bProcessComplete)

    End Sub

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

            m_oInterface = New Interface_Renamed

            m_lReturn = m_oInterface.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the Interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description))

                Return result
            End If

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

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
                    m_oInterface = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: NavigatorV3_SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_SetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV3.SetKeys

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

                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetKeys failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_SetKeys", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_( (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV3.GetKeys

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function

            m_lReturn = CType(m_oInterface.GetKeys(vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.GetKeys failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_GetKeys", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer Implements aPMNav.NavigatorV3.GetSummary

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = CType(m_oInterface.GetSummary(vSummaryArray:=vSummaryArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.GetSummary failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_GetSummary", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements aPMNav.NavigatorV3.SetProcessModes

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function
            m_lReturn = CType(m_oInterface.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("vEffectiveDate", vEffectiveDate)
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetProcessModes failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", oDicParms:=oDict)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_SetProcessModes", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NavigatorV3_Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function NavigatorV3_Start() As Integer Implements aPMNav.NavigatorV3.Start

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the corresponding Interface function            
            m_lReturn = CType(m_oInterface.Start(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.Start failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result
        Catch exDisposed As ObjectDisposedException

            result = gPMConstants.PMEReturnCode.PMTrue
            Return result
        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_Start", excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)
    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
