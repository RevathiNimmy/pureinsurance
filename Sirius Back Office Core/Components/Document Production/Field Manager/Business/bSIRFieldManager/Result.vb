Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129 (guide)
<System.Runtime.InteropServices.ProgId("Result_NET.Result")>
Public NotInheritable Class Result
    Public Key As String = ""

    'local variable(s) to hold record
    'SD 01/08/2002 scalability changes
    'sj 13/08/2002 - start
    'Private m_oRecord  As adodb.Record
    Private m_oRecord As dPMDAO.Records
    'sj 13/08/2002 - end

    Private m_lReturn As Integer

    ' ************************************************
    ' Added to replace global variables 27/11/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    'SD 01/08/2002 scalability changes
    'sj 13/08/2002 - start
    'Public Property Set Record(oData As adodb.Record)

    'SD 01/08/2002 scalability changes
    'sj 13/08/2002 - start
    'Public Property Get Record() As adodb.Record
    Public Property Record() As dPMDAO.Records
        Get
            'sj 13/08/2002 - end
            Return m_oRecord

        End Get
        Set(ByVal Value As dPMDAO.Records)
            'sj 13/08/2002 - end
            m_oRecord = Value

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
        Dim ACClass As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
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



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Protected Overrides Sub Finalize()

        If Not (m_oRecord Is Nothing) Then
            'sj 13/08/2002 - start
            'm_lReturn& = m_oRecord.Terminate()
            'sj 13/08/2002 - end
        End If

        m_oRecord = Nothing

    End Sub
End Class
