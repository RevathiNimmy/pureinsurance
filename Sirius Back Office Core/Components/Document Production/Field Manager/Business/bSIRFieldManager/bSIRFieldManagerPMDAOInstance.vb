Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129 (guide)
Friend NotInheritable Class PMDAOInstance

    Private Const ACClass As String = "PMDAOInstance"

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
    Private m_eFamily As gPMConstants.PMEProductFamily
    Private m_oDatabase As dPMDAO.Database

    Public Property Family() As gPMConstants.PMEProductFamily
        Get
            Return m_eFamily
        End Get
        Set(ByVal Value As gPMConstants.PMEProductFamily)
            m_eFamily = Value
        End Set
    End Property
    Public Property Database() As dPMDAO.Database
        Get
            Return m_oDatabase
        End Get
        Set(ByVal Value As dPMDAO.Database)
            m_oDatabase = Value
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
    Private Shared _DefaultInstance As PMDAOInstance = Nothing
    Public Shared ReadOnly Property DefaultInstance() As PMDAOInstance
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New PMDAOInstance
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
