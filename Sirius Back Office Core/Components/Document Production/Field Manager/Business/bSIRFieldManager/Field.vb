Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129 (guide)
Friend NotInheritable Class Field
    Public Key As String = ""

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
    'local variable(s) to hold property value(s)
    Private m_sSQLString As String = "" 'local copy
    Private m_sColumnName As String = ""
    Private m_iColumnType As Integer
    Private m_iProductFamily As Integer
    Private m_vDataModel As Object
    Private m_vPropertyId As Object
    Private m_iSpecialType As Integer
    ' MainGroup
    Private m_sMainGroup As String = ""
    Private m_sSubGroup As String = ""
    'RWH(16/01/2001) Added loop fields to assist extra levels of looping.
    Private m_sLoop1 As String = ""
    Private m_sLoop2 As String = ""
    Private m_sLoop3 As String = ""
    Private m_sLoop4 As String = ""

    Public Property TableName() As String

    Public Property Loop4() As String
        Get
            Return m_sLoop4
        End Get
        Set(ByVal Value As String)
            m_sLoop4 = Value
        End Set
    End Property

    Public Property Loop3() As String
        Get
            Return m_sLoop3
        End Get
        Set(ByVal Value As String)
            m_sLoop3 = Value
        End Set
    End Property

    Public Property Loop2() As String
        Get
            Return m_sLoop2
        End Get
        Set(ByVal Value As String)
            m_sLoop2 = Value
        End Set
    End Property

    Public Property Loop1() As String
        Get
            Return m_sLoop1
        End Get
        Set(ByVal Value As String)
            m_sLoop1 = Value
        End Set
    End Property
    'RWH(03/10/2000) RSAIB Process 28. Added MainGroup to aid risk looping.
    Public Property MainGroup() As String
        Get
            Return m_sMainGroup
        End Get
        Set(ByVal Value As String)
            m_sMainGroup = Value
        End Set
    End Property
    Public Property SQLString() As String
        Get
            Return m_sSQLString
        End Get
        Set(ByVal Value As String)
            m_sSQLString = Value
        End Set
    End Property
    Public Property SubGroup() As String
        Get
            Return m_sSubGroup
        End Get
        Set(ByVal Value As String)
            m_sSubGroup = Value
        End Set
    End Property
    Public Property ColumnName() As String
        Get
            Return m_sColumnName
        End Get
        Set(ByVal Value As String)
            m_sColumnName = Value
        End Set
    End Property
    Public Property ColumnType() As Integer
        Get
            Return m_iColumnType
        End Get
        Set(ByVal Value As Integer)
            m_iColumnType = Value
        End Set
    End Property
    Public Property ProductFamily() As Integer
        Get
            Return m_iProductFamily
        End Get
        Set(ByVal Value As Integer)
            m_iProductFamily = Value
        End Set
    End Property
    Public Property DataModel() As Object
        Get
            Return m_vDataModel
        End Get
        Set(ByVal Value As Object)


            m_vDataModel = Value
        End Set
    End Property
    Public Property PropertyId() As Object
        Get
            Return m_vPropertyId
        End Get
        Set(ByVal Value As Object)


            m_vPropertyId = Value
        End Set
    End Property
    Public Property SpecialType() As Integer
        Get
            Return m_iSpecialType
        End Get
        Set(ByVal Value As Integer)
            m_iSpecialType = Value
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
End Class
