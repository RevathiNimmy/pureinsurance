Option Strict Off
Option Explicit On
'developer guide no. 129    
Imports SSP.Shared

Friend NotInheritable Class Group
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Group
    '
    ' Date: 18/06/1998
    '
    ' Description: Contains information for a single Group
    '              Name/Value pair.
    '
    ' Edit History:
    '
    ' 17/06/1998 RFC Original Created.
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Group"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Attributes
    Private m_sGroupName As String = ""
    Private m_oGroupProperties As bPMPropertyManager.Properties
    ' PRIVATE Data Members (End)

    ' PUBLIC Group Procedures (Begin)
    Public Property GroupName() As String
        Get

            Return m_sGroupName

        End Get
        Set(ByVal Value As String)

            m_sGroupName = Value.Trim()

        End Set
    End Property

    Public Property GroupProperties() As bPMPropertyManager.Properties
        Get

            Return m_oGroupProperties

        End Get
        Set(ByVal Value As bPMPropertyManager.Properties)

            ''developer guide no.  unrequired code is commented
            'If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is string) Then

            '	m_oGroupProperties = Value

            'Else

            '	m_oGroupProperties = Value

            'End If
            m_oGroupProperties = Value
        End Set
    End Property
    ' PUBLIC Group Procedures (End)

    ' PRIVATE Group Procedures (Begin)
    ' PRIVATE Group Procedures (End)


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

            ' Initialisation Code.
            m_oGroupProperties = New bPMPropertyManager.Properties()

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                If m_oGroupProperties IsNot Nothing Then
                    m_oGroupProperties.Dispose()
                    m_oGroupProperties = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
