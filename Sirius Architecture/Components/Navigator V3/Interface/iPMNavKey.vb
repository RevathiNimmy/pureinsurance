Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class Key
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Key
    '
    ' Date: 01/09/1998
    '
    ' Description: Describes the attributes for a single Navigator Key.
    '
    ' Edit History:
    ' RFC020299 - Do NOT Call the Terminate Method for Object Keys.
    '             Release Reference only. The Terminate method on Gemini
    '             Component Method was being called by Navigator when a
    '             Sub Process was started.
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Key"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' DataBase Attributes
    Private m_sKeyName As New FixedLengthString(30)
    Private m_vValue As Object = Nothing
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property KeyName() As String
        Get

            Return m_sKeyName.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sKeyName.Value = Value.Trim()

        End Set
    End Property

    Public Property Value() As Object
        Get

            Select Case Information.VarType(m_vValue)
                Case VariantType.String
                    Return m_vValue.Trim()
                Case VariantType.Object
                    Return m_vValue
                Case Else
                    Return m_vValue
            End Select

        End Get
        Set(ByVal Value As Object)
            If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is String) Then

                m_vValue = Value

            Else
                Dim oValue As String = ""

                Select Case Information.VarType(Value)
                    Case VariantType.String

                        m_vValue = CStr(Value).Trim()
                    Case VariantType.Object
                        m_vValue = Value
                    Case Else

                        m_vValue = CStr(Value)
                End Select

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                Value = Nothing
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
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
