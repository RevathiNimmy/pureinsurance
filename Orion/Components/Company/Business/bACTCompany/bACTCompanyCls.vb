Option Strict Off
Option Explicit On
'developer guide no 129. 
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Company_NET.Company")>
Public NotInheritable Class Company
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Company
    '
    ' Date: 31/07/1997
    '
    ' Description: Describes the Company attributes.
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
    Private Const ACClass As String = "Company"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_iCompanyID As Integer
    Private m_iBaseCurrency As Integer
    Private m_sCode As New StringsHelper.FixedLengthString(10)
    Private m_sDescription As New StringsHelper.FixedLengthString(255)
    Private m_lCaptionID As Integer
    Private m_iParentID As Integer
    Private m_sRegNo1 As New StringsHelper.FixedLengthString(30)
    Private m_sRegNo2 As New StringsHelper.FixedLengthString(30)
    Private m_sAddress1 As New StringsHelper.FixedLengthString(40)
    Private m_sAddress2 As New StringsHelper.FixedLengthString(40)
    Private m_sAddress3 As New StringsHelper.FixedLengthString(40)
    Private m_sAddress4 As New StringsHelper.FixedLengthString(40)
    Private m_sPostalCode As New StringsHelper.FixedLengthString(20)
    Private m_iCountryID As Integer
    Private m_sPhoneAreaCode As New StringsHelper.FixedLengthString(10)
    Private m_sPhoneNumber As New StringsHelper.FixedLengthString(15)
    Private m_sPhoneExtension As New StringsHelper.FixedLengthString(6)
    Private m_sFaxAreaCode As New StringsHelper.FixedLengthString(10)
    Private m_sFaxNumber As New StringsHelper.FixedLengthString(15)
    Private m_sFaxExtension As New StringsHelper.FixedLengthString(6)
    ' DC 31/01/00
    Private m_sEmail As New StringsHelper.FixedLengthString(50)
    Private m_sVatNo As New StringsHelper.FixedLengthString(20)
    Private m_sSenderMailboxId As New StringsHelper.FixedLengthString(14)
    Private m_sBrokerABIId As New StringsHelper.FixedLengthString(6)
    Private m_iUserLicenceId As Integer
    Private m_iPMCompanyNumber As Integer
    Private m_sDefaultIndicator As New StringsHelper.FixedLengthString(1)



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


    Public Property CompanyID() As Integer
        Get

            Return m_iCompanyID

        End Get
        Set(ByVal Value As Integer)

            m_iCompanyID = Value

        End Set
    End Property

    Public Property BaseCurrency() As Integer
        Get

            Return m_iBaseCurrency

        End Get
        Set(ByVal Value As Integer)

            m_iBaseCurrency = Value

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

    Public Property Description() As String
        Get

            Return m_sDescription.Value

        End Get
        Set(ByVal Value As String)

            m_sDescription.Value = Value

        End Set
    End Property

    Public Property CaptionID() As Integer
        Get

            Return m_lCaptionID

        End Get
        Set(ByVal Value As Integer)

            m_lCaptionID = Value

        End Set
    End Property

    Public Property ParentID() As Integer
        Get

            Return m_iParentID

        End Get
        Set(ByVal Value As Integer)

            m_iParentID = Value

        End Set
    End Property

    Public Property RegNo1() As String
        Get

            Return m_sRegNo1.Value

        End Get
        Set(ByVal Value As String)

            m_sRegNo1.Value = Value

        End Set
    End Property

    Public Property RegNo2() As String
        Get

            Return m_sRegNo2.Value

        End Get
        Set(ByVal Value As String)

            m_sRegNo2.Value = Value

        End Set
    End Property

    Public Property Address1() As String
        Get

            Return m_sAddress1.Value

        End Get
        Set(ByVal Value As String)

            m_sAddress1.Value = Value

        End Set
    End Property

    Public Property Address2() As String
        Get

            Return m_sAddress2.Value

        End Get
        Set(ByVal Value As String)

            m_sAddress2.Value = Value

        End Set
    End Property

    Public Property Address3() As String
        Get

            Return m_sAddress3.Value

        End Get
        Set(ByVal Value As String)

            m_sAddress3.Value = Value

        End Set
    End Property

    Public Property Address4() As String
        Get

            Return m_sAddress4.Value

        End Get
        Set(ByVal Value As String)

            m_sAddress4.Value = Value

        End Set
    End Property

    Public Property PostalCode() As String
        Get

            Return m_sPostalCode.Value

        End Get
        Set(ByVal Value As String)

            m_sPostalCode.Value = Value

        End Set
    End Property

    Public Property CountryID() As Integer
        Get

            Return m_iCountryID

        End Get
        Set(ByVal Value As Integer)

            m_iCountryID = Value

        End Set
    End Property

    Public Property PhoneAreaCode() As String
        Get

            Return m_sPhoneAreaCode.Value

        End Get
        Set(ByVal Value As String)

            m_sPhoneAreaCode.Value = Value

        End Set
    End Property

    Public Property PhoneNumber() As String
        Get

            Return m_sPhoneNumber.Value

        End Get
        Set(ByVal Value As String)

            m_sPhoneNumber.Value = Value

        End Set
    End Property

    Public Property PhoneExtension() As String
        Get

            Return m_sPhoneExtension.Value

        End Get
        Set(ByVal Value As String)

            m_sPhoneExtension.Value = Value

        End Set
    End Property

    Public Property FaxAreaCode() As String
        Get

            Return m_sFaxAreaCode.Value

        End Get
        Set(ByVal Value As String)

            m_sFaxAreaCode.Value = Value

        End Set
    End Property

    Public Property FaxNumber() As String
        Get

            Return m_sFaxNumber.Value

        End Get
        Set(ByVal Value As String)

            m_sFaxNumber.Value = Value

        End Set
    End Property

    Public Property FaxExtension() As String
        Get

            Return m_sFaxExtension.Value

        End Get
        Set(ByVal Value As String)

            m_sFaxExtension.Value = Value

        End Set
    End Property
    ' DC 31/01/00
    ' DC 31/01/00
    Public Property Email() As String
        Get

            Return m_sEmail.Value

        End Get
        Set(ByVal Value As String)

            m_sEmail.Value = Value

        End Set
    End Property
    ' DC 31/01/00
    ' DC 31/01/00
    Public Property VatNo() As String
        Get

            Return m_sVatNo.Value

        End Get
        Set(ByVal Value As String)

            m_sVatNo.Value = Value

        End Set
    End Property
    ' DC 31/01/00
    ' DC 31/01/00
    Public Property SenderMailboxId() As String
        Get

            Return m_sSenderMailboxId.Value

        End Get
        Set(ByVal Value As String)

            m_sSenderMailboxId.Value = Value

        End Set
    End Property
    ' DC 31/01/00
    ' DC 31/01/00
    Public Property BrokerABIId() As String
        Get

            Return m_sBrokerABIId.Value

        End Get
        Set(ByVal Value As String)

            m_sBrokerABIId.Value = Value

        End Set
    End Property
    ' DC 31/01/00
    ' DC 31/01/00
    Public Property UserLicenceId() As Integer
        Get

            Return m_iUserLicenceId

        End Get
        Set(ByVal Value As Integer)

            m_iUserLicenceId = Value

        End Set
    End Property
    ' DC 31/01/00
    ' DC 31/01/00
    Public Property PMCompanyNumber() As Integer
        Get

            Return m_iPMCompanyNumber

        End Get
        Set(ByVal Value As Integer)

            m_iPMCompanyNumber = Value

        End Set
    End Property
    ' DC 31/01/00
    ' DC 31/01/00
    Public Property DefaultIndicator() As String
        Get

            Return m_sDefaultIndicator.Value

        End Get
        Set(ByVal Value As String)

            m_sDefaultIndicator.Value = Value

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
