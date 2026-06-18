Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared

Friend NotInheritable Class Property_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Property
    '
    ' Date: 18/06/1998
    '
    ' Description: Contains information for a single Property
    '              Name/Value pair.
    '
    ' Edit History:
    '
    ' 17/06/1998 RFC Original Created.
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Property"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Attributes
    Private m_sPropertyName As String = ""
    Private m_vPropertyValue As Object
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property PropertyName() As String
        Get

            Return m_sPropertyName

        End Get
        Set(ByVal Value As String)

            m_sPropertyName = Value.Trim()

        End Set
    End Property

    Public Property PropertyValue() As Object
        Get

            If Informations.IsReference(m_vPropertyValue) Then
                Return m_vPropertyValue
            Else
                'Select Case Information.VarType(m_vPropertyValue)
                '    Case VariantType.String

                '        Return CStr(m_vPropertyValue).Trim()
                '    Case Else
                '        Return m_vPropertyValue
                'End Select
                If TypeOf m_vPropertyValue Is String Then
                    Return CStr(m_vPropertyValue).Trim()
                Else
                    Return m_vPropertyValue
                End If
            End If

        End Get
        Set(ByVal Value As Object)
            If Informations.IsReference(Value) And Not (TypeOf Value Is String) Then

                m_vPropertyValue = Value

            Else

                If Informations.IsReference(Value) Then
                    m_vPropertyValue = Value
                Else
                    'Select Case Information.VarType(Value)
                    '    Case VariantType.String


                    '        m_vPropertyValue = CStr(Value).Trim()
                    '    Case Else


                    '        m_vPropertyValue = Value
                    'End Select
                    If TypeOf Value Is String Then
                        m_vPropertyValue = CStr(Value).Trim()
                    Else
                        m_vPropertyValue = Value
                    End If
                End If

            End If
        End Set
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
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

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
                If PropertyValue IsNot Nothing Then
                    PropertyValue.Dispose()
                    PropertyValue = Nothing
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

    Private Shared _DefaultInstance As Property_Renamed = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Property_Renamed
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Property_Renamed
            End If
            Return _DefaultInstance
        End Get
    End Property
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
