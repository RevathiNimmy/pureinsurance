Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Vijay Pal on 5/19/2010 10:34:22 AM refer developer guide no. 129
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Message_NET.Message")> _
Public NotInheritable Class Message
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Message
    '
    ' Date: 23rd Janaury 98
    '
    ' Description: MAPI Message
    '
    ' Edit History:
    ' ***************************************************************** '

    Private Const ACClass As String = "Message"

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_sId As String = ""
    Private m_sNoteText As String = ""
    Private m_sSubject As String = ""
    Private m_oRecipients As Recipients
    Private m_oAttachments As Attachments
    Private m_oFunctions As Functions


    Public Property Id() As String
        Get
            Return m_sId
        End Get
        Set(ByVal Value As String)
            m_sId = Value
        End Set
    End Property


    Public Property NoteText() As String
        Get
            Return m_sNoteText
        End Get
        Set(ByVal Value As String)
            m_sNoteText = Value
        End Set
    End Property


    Public Property Subject() As String
        Get
            Return m_sSubject
        End Get
        Set(ByVal Value As String)
            m_sSubject = Value
        End Set
    End Property

    Public ReadOnly Property Attachments() As Attachments
        Get
            Return m_oAttachments
        End Get
    End Property

    Public ReadOnly Property Recipients() As Recipients
        Get
            Return m_oRecipients
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef oFunctions As Object) As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            m_oFunctions = oFunctions

            m_oRecipients = New Recipients()
            m_lReturn = CType(m_oRecipients.Initialise(m_oFunctions), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            m_oAttachments = New Attachments()
            m_lReturn = CType(m_oAttachments.Initialise(m_oFunctions), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    ' ********************************************************************
    ' Send this message
    ' ********************************************************************
    Public Function Send() As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oFunctions.Send(Me), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Send", vApp:=ACApp, vClass:=ACClass, vMethod:="Send", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
End Class
