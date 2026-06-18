Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("PMMessage_NET.PMMessage")> _
Public NotInheritable Class PMMessage
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMMessage
    '
    ' Date: 04 September 1996
    '
    ' Description: Main class containing all of the business methods.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Constant for the methods to identify
    ' which class this is.
    Private Const ACClass As String = "PMMessage"

    ' System Log Level
    Private m_iLogLevel As Integer

    ' User Defined Log Level
    Private m_iUserLogLevel As Integer

    ' Log Filename
    Private m_sLogName As String = ""

    ' Username
    Private m_sUserName As String = ""

    ' Password
    Private m_sPassword As String = ""

    ' Instance of the client manager
    Private m_oClientManager As Object


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property LogLevel() As Integer
        Get

            ' Return the System Log Level.
            Return m_iLogLevel

        End Get
    End Property

    Public ReadOnly Property UserLogLevel() As Integer
        Get

            ' Return the user defined Log Level.
            Return m_iUserLogLevel

        End Get
    End Property

    Public ReadOnly Property LogName() As String
        Get

            ' Return the Log Filename.
            Return m_sLogName

        End Get
    End Property
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef oClientManager As Object) As Integer

        Dim result As Integer = 0
        Dim lErrorValue As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Username and Password
            m_sUserName = sUsername
            m_sPassword = sPassword

            ' Check to see that we have a valid Client Manager
            If oClientManager Is Nothing Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error to screen.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Client Manager Reference", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the instance of the client manager
            m_oClientManager = oClientManager

            ' Get the system log level
            lErrorValue = GetLogDetails()

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the log details", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                m_oClientManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: AddMessage
    '
    ' Description: Wrapper method to the LogMessage function.
    '
    ' ***************************************************************** '
    Public Sub AddMessage(ByVal iType As Integer, ByVal sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing)

        Try

            If (iType <= m_iLogLevel) Or (iType <= gPMConstants.PMELogLevel.PMLogOnError) Then






                gPMFunctions.LogMessageToFile(sUsername:=m_sUserName, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)))

            End If

        Catch excep As Exception



            ' Error Section.

            ' Failed to log message, so we must
            ' call the function to popup the
            ' message instead.





            gPMFunctions.LogMessagePopup(iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=excep)
        End Try


    End Sub
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetLogDetails
    '
    ' Description: Gets the Log details from the client manager object
    '              and from the registry.
    '
    '
    ' ***************************************************************** '
    Private Function GetLogDetails() As Integer

        Dim result As Integer = 0
        Dim lErrorValue As Integer
        Dim sResult As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set the System Log Level member.

        m_iLogLevel = m_oClientManager.LogLevel

        ' Get the User Log Level from the registry.
        lErrorValue = gPMFunctions.GetRegSettings(sResult, gPMConstants.PMRegAppName, gPMConstants.PMRegSecSystem, gPMConstants.PMRegKeyLogLevel, CStr(m_iLogLevel))

        ' Set the user log level member.
        m_iUserLogLevel = CInt(sResult)

        ' Get the Log Filename from the registry.
        lErrorValue = gPMFunctions.GetRegSettings(sResult, gPMConstants.PMRegAppName, gPMConstants.PMRegSecSystem, gPMConstants.PMRegKeyLogFile, gPMConstants.PMDefaultLogFile)

        ' Set the user log filename.
        m_sLogName = sResult.Trim()

        Return result

    End Function

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

