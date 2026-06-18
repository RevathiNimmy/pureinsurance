Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Developer Guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Bank_NET.Bank")> _
Public NotInheritable Class Bank
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Bank
    '
    ' Date: 08/09/1997
    '
    ' Description: Describes the Bank attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Bank"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_iBankID As Integer
    Private m_sCode As New FixedLengthString(10)
    Private m_sBranchCode As New FixedLengthString(30)
    Private m_sBankName As New FixedLengthString(60)
    Private m_iHeadOffice As Integer
    Private m_sBankAddress1 As New FixedLengthString(40)
    Private m_sBankAddress2 As New FixedLengthString(40)
    Private m_sBankAddress3 As New FixedLengthString(40)
    Private m_sBankAddress4 As New FixedLengthString(40)
    Private m_sBankPostalCode As New FixedLengthString(20)
    Private m_iBankCountry As Integer
    Private m_sBankPhoneAreaCode As New FixedLengthString(10)
    Private m_sBankPhoneNumber As New FixedLengthString(15)
    Private m_sBankPhoneExtension As New FixedLengthString(6)
    Private m_sBankFaxAreaCode As New FixedLengthString(10)
    Private m_sBankFaxNumber As New FixedLengthString(15)
    Private m_sBankFaxExtension As New FixedLengthString(6)
    Private m_sComments As New FixedLengthString(255)
    Private m_iBankAccountType As Integer
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property


    Public Property BankID() As Integer
        Get

            Return m_iBankID

        End Get
        Set(ByVal Value As Integer)

            m_iBankID = Value

        End Set
    End Property

    Public Property Code() As String
        Get

            Return m_sCode.Value

        End Get
        Set(ByVal Value As String)

            m_sCode.Value = Value

        End Set
    End Property

    Public Property BranchCode() As String
        Get

            Return m_sBranchCode.Value

        End Get
        Set(ByVal Value As String)

            m_sBranchCode.Value = Value

        End Set
    End Property

    Public Property BankName() As String
        Get

            Return m_sBankName.Value

        End Get
        Set(ByVal Value As String)

            m_sBankName.Value = Value

        End Set
    End Property

    Public Property HeadOffice() As Integer
        Get

            Return m_iHeadOffice

        End Get
        Set(ByVal Value As Integer)

            m_iHeadOffice = Value

        End Set
    End Property

    Public Property BankAddress1() As String
        Get

            Return m_sBankAddress1.Value

        End Get
        Set(ByVal Value As String)

            m_sBankAddress1.Value = Value

        End Set
    End Property

    Public Property BankAddress2() As String
        Get

            Return m_sBankAddress2.Value

        End Get
        Set(ByVal Value As String)

            m_sBankAddress2.Value = Value

        End Set
    End Property

    Public Property BankAddress3() As String
        Get

            Return m_sBankAddress3.Value

        End Get
        Set(ByVal Value As String)

            m_sBankAddress3.Value = Value

        End Set
    End Property

    Public Property BankAddress4() As String
        Get

            Return m_sBankAddress4.Value

        End Get
        Set(ByVal Value As String)

            m_sBankAddress4.Value = Value

        End Set
    End Property

    Public Property BankPostalCode() As String
        Get

            Return m_sBankPostalCode.Value

        End Get
        Set(ByVal Value As String)

            m_sBankPostalCode.Value = Value

        End Set
    End Property

    Public Property BankCountry() As Integer
        Get

            Return m_iBankCountry

        End Get
        Set(ByVal Value As Integer)

            m_iBankCountry = Value

        End Set
    End Property

    Public Property BankPhoneAreaCode() As String
        Get

            Return m_sBankPhoneAreaCode.Value

        End Get
        Set(ByVal Value As String)

            m_sBankPhoneAreaCode.Value = Value

        End Set
    End Property

    Public Property BankPhoneNumber() As String
        Get

            Return m_sBankPhoneNumber.Value

        End Get
        Set(ByVal Value As String)

            m_sBankPhoneNumber.Value = Value

        End Set
    End Property

    Public Property BankPhoneExtension() As String
        Get

            Return m_sBankPhoneExtension.Value

        End Get
        Set(ByVal Value As String)

            m_sBankPhoneExtension.Value = Value

        End Set
    End Property

    Public Property BankFaxAreaCode() As String
        Get

            Return m_sBankFaxAreaCode.Value

        End Get
        Set(ByVal Value As String)

            m_sBankFaxAreaCode.Value = Value

        End Set
    End Property

    Public Property BankFaxNumber() As String
        Get

            Return m_sBankFaxNumber.Value

        End Get
        Set(ByVal Value As String)

            m_sBankFaxNumber.Value = Value

        End Set
    End Property

    Public Property BankFaxExtension() As String
        Get

            Return m_sBankFaxExtension.Value

        End Get
        Set(ByVal Value As String)

            m_sBankFaxExtension.Value = Value

        End Set
    End Property

    Public Property Comments() As String
        Get

            Return m_sComments.Value

        End Get
        Set(ByVal Value As String)

            m_sComments.Value = Value

        End Set
    End Property


    Public Property BankAccountType() As Integer
        Get
            Return m_iBankAccountType
        End Get
        Set(ByVal Value As Integer)
            m_iBankAccountType = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
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

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


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