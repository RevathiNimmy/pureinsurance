Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
'Modified by Vijay Pal on 5/19/2010 10:34:07 AM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("PMMAPI_NET.PMMAPI")> _
Public NotInheritable Class PMMAPI 
    ' ***************************************************************** '
    ' Class Name: PMMAPI
    '
    ' Date: 23rd Janaury 98
    '
    ' Description: PM MAPI Routines
    '
    ' Edit History:
    ' ***************************************************************** '
    Dim objfrmDummy As frmDummy
    Private Const ACClass As String = "PMMAPI"

    Private m_mapiSession As AxMSMAPI.AxMAPISession
    Private m_mapiMessageControl As AxMSMAPI.AxMAPIMessages
    Private m_mapiErrors As MSMAPI.MAPIErrors
    Private m_sMailUserName As String
    Private m_sMailPassword As String
    Private m_oMessages As Messages
    Private m_oFunctions As Functions

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PUBLIC PROPERTIES
    Public ReadOnly Property Messages() As Messages
        Get
            Return m_oMessages
        End Get
    End Property


    Public Property Session() As Object
        Get
            Return m_mapiSession
        End Get
        Set(ByVal Value As Object)
            m_mapiSession = Value
        End Set
    End Property


    Public Property MessageControl() As Object
        Get
            Return m_mapiMessageControl
        End Get
        Set(ByVal Value As Object)
            m_mapiMessageControl = Value
        End Set
    End Property


    Public Property MailUserName() As String
        Get
            Return m_sMailUserName
        End Get
        Set(ByVal Value As String)
            m_sMailUserName = Value
        End Set
    End Property


    Public Property MailPassword() As String
        Get
            Return m_sMailPassword
        End Get
        Set(ByVal Value As String)
            m_sMailPassword = Value
        End Set
    End Property

    ' END OF PUBLIC PROPERTIES
    Public Function Initialise( _
        Optional ByVal v_vMailUserName As Object = Nothing, _
        Optional ByVal v_vMailPassword As Object = Nothing, _
        Optional ByRef r_mapiSession As Object = Nothing, _
        Optional ByRef r_mapiMessages As Object = Nothing) As Long


        Initialise = gPMConstants.PMEReturnCode.PMTrue

        m_oFunctions = New Functions
        m_lReturn = m_oFunctions.Initialise(Me)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Initialise = gPMConstants.PMEReturnCode.PMFail
            Exit Function
        End If
        'developer guide no. 50 
        objfrmDummy = New frmDummy
        m_oMessages = New Messages

        m_lReturn = m_oMessages.Initialise(m_oFunctions)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Initialise = gPMConstants.PMEReturnCode.PMFail
            Exit Function
        End If

        If Not r_mapiSession Is Nothing Then
            m_mapiSession = r_mapiSession
        Else
            m_mapiSession = objfrmDummy.MAPISession1
        End If

        If Not r_mapiMessages Is Nothing Then
            m_mapiMessageControl = r_mapiMessages
        Else
            m_mapiMessageControl = objfrmDummy.MAPIMessages1
        End If

        If Not Information.IsNothing(v_vMailUserName) Then
            m_sMailUserName = v_vMailUserName
        End If

        If Not Information.IsNothing(v_vMailPassword) Then
            m_sMailPassword = v_vMailPassword
        End If

        m_lReturn = SessionSignOn()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Initialise = gPMConstants.PMEReturnCode.PMFail
            Exit Function
        End If

        Exit Function

Err_Initialise:

        Initialise = gPMConstants.PMEReturnCode.PMFalse

        iPMFunc.LogMessage( _
            iType:=PMConst.PMLogOnError, _
            sMsg:="Failed to Initialise", _
            vApp:=ACApp, _
            vClass:=ACClass, _
            vMethod:="Initialise", _
            vErrNo:=Err.Number, _
            vErrDesc:=Err.Description)

        Exit Function

    End Function


    Private Function SessionSignOn() As Long

        Try
        SessionSignOn = gPMConstants.PMEReturnCode.PMTrue

        With m_mapiSession
            '    .UserName = m_sMailUserName
            '    .Password = m_sMailPassword
            .SignOn()
            m_mapiMessageControl.SessionID = .SessionID
        End With

        Exit Function
        Catch ex As Exception

        ' Trap the MAPI Errors that we know about

        ' BB "Logon failure. Valid session ID already exists"
        ' Logon Failure caused by Outlook being already loaded
        ' with Word as editor. Can be safely ignored
        If Err.Number = 32050 Then
            SessionSignOn = gPMConstants.PMEReturnCode.PMTrue
            Exit Function
        End If

        SessionSignOn = gPMConstants.PMEReturnCode.PMFalse

        iPMFunc.LogMessage(iType:=PMConst.PMLogOnError,sMsg:="Failed to SessionSignOn",vApp:=ACApp,vClass:=ACClass,vMethod:="SessionSignOn",vErrNo:=Err.Number,vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function
    '*******************************************************
    ' Send a message
    '*******************************************************
    Public Function SendMessage(ByVal v_vSubject As Object, ByVal v_vNoteText As Object, ByVal v_vRecipient As Object, Optional ByVal v_vAttachment As Object = Nothing) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Following block re-written to use the new add methods on the collections
        ' to stop Attachment/Message/Recipient having to be publicly createable

        '  Dim oMsg As New Message
        '  Dim oRcp As New Recipient
        '  Dim oAtt As New Attachment
        '
        '  oMsg.Subject = v_vSubject
        '  oMsg.NoteText = v_vNoteText
        '
        '  m_lReturn = Me.Messages.Add(oMsg)
        '
        '  oRcp.Name = v_vRecipient
        '  oRcp.RecipientType = mapToList
        '  oRcp.AddressBook = True
        '
        '  m_lReturn = Me.Messages.Item(1).Recipients.Add(oRcp)
        '
        '  If Not IsMissing(v_vAttachment) Then
        '    m_lReturn = Me.Messages.LastItem.Attachments.Add(oAtt)
        '    oAtt.Name = v_vAttachment
        '    oAtt.Path = v_vAttachment
        '    oAtt.FileType = mapData
        '  End If



        m_lReturn = CType(Me.Messages.AddMessage(v_vSubject:=CStr(v_vSubject), v_vNoteText:=CStr(v_vNoteText)), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        With Me.Messages.LastItem


            'TODO check at run time
            ' m_lReturn = CType(.Recipients.AddRecipient(v_vName:=CStr(v_vRecipient), v_vAddressType:=CStr(MSMAPI.RecipTypeConstants.mapToList), v_vAddressBook:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Information.IsNothing(v_vAttachment) Then



                m_lReturn = CType(.Attachments.AddAttachment(v_vName:=CStr(v_vAttachment), v_vPath:=CStr(v_vAttachment)), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            m_lReturn = .Send()

        End With

        Return result



        result = gPMConstants.PMEReturnCode.PMFail

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SendMessage", vApp:=ACApp, vClass:=ACClass, vMethod:="SendMessage", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function
    '*******************************************************
    ' Send all the messages in the collection
    '*******************************************************
    Public Function SendAll() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For i As Integer = 1 To m_oMessages.Count()

                m_lReturn = CType(m_oMessages.Item(i).Send(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SendAll", vApp:=ACApp, vClass:=ACClass, vMethod:="SendAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
