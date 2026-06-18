Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Modified by Vijay Pal on 5/20/2010 11:52:07 AM refer developer guide no. 129
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("ListCustom_NET.ListCustom")> _
Public NotInheritable Class ListCustom
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: ListCustom
    '
    ' Date: 10/02/1999
    '
    ' Description: Describes the ListCustom attributes.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ListCustom"

    ' ************************************************
    ' Added to replace global variables 12/01/2004
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    ' Update Status
    Private m_iDatabaseStatus As gPMConstants.PMEComponentAction
    ' DataBase Attributes
    Private m_lListCustomID As Integer
    Private m_lPositionID As Integer
    Private m_lValueID As Integer
    Private m_sText As New FixedLengthString(70)
    Private m_sAbiCode As New FixedLengthString(10)
    Private m_sCommand As New FixedLengthString(1)
    Private m_sPropertyID As New FixedLengthString(10)

    Public Property DatabaseStatus() As Integer
        Get
            Return m_iDatabaseStatus
        End Get
        Set(ByVal Value As Integer)
            m_iDatabaseStatus = Value
        End Set
    End Property
    Public Property ListCustomID() As Integer
        Get
            Return m_lListCustomID
        End Get
        Set(ByVal Value As Integer)
            m_lListCustomID = Value
        End Set
    End Property
    Public Property PositionID() As Integer
        Get
            Return m_lPositionID
        End Get
        Set(ByVal Value As Integer)
            m_lPositionID = Value
        End Set
    End Property
    Public Property ValueID() As Integer
        Get
            Return m_lValueID
        End Get
        Set(ByVal Value As Integer)
            m_lValueID = Value
        End Set
    End Property
    Public Property Text() As String
        Get
            Return m_sText.Value
        End Get
        Set(ByVal Value As String)
            m_sText.Value = Value
        End Set
    End Property
    Public Property AbiCode() As String
        Get
            Return m_sAbiCode.Value
        End Get
        Set(ByVal Value As String)
            m_sAbiCode.Value = Value
        End Set
    End Property
    Public Property Command() As String
        Get
            Return m_sCommand.Value
        End Get
        Set(ByVal Value As String)
            m_sCommand.Value = Value
        End Set
    End Property
    Public Property PropertyID() As String
        Get
            Return m_sPropertyID.Value
        End Get
        Set(ByVal Value As String)
            m_sPropertyID.Value = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            End If
        End If
        Me.disposedValue = True
    End Sub


    Friend Sub New()
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
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
