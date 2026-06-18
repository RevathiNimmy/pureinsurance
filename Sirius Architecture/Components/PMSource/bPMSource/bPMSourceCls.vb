Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Source_NET.Source")>
Public NotInheritable Class Source
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Source
    '
    ' Date: 31/07/1997
    '
    ' Description: Describes the Source attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Source"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_iSourceIDX As Integer
    Private m_lCaptionID As Integer
    Private m_sCode As New StringsHelper.FixedLengthString(10)
    Private m_sDescription As New StringsHelper.FixedLengthString(255)
    Private m_iParentID As Integer
    Private m_vBaseCurrency As Object
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
    Private m_sEmail As New StringsHelper.FixedLengthString(50)
    Private m_sVatNo As New StringsHelper.FixedLengthString(20)
    Private m_sSenderMailboxId As New StringsHelper.FixedLengthString(14)
    Private m_sBrokerABIId As New StringsHelper.FixedLengthString(6)
    Private m_iUserLicenceId As Integer
    Private m_iPMSourceNumber As Integer
    Private m_sDefaultIndicator As New StringsHelper.FixedLengthString(1)
    Private m_iIsDeleted As Integer
    Private m_dtEffectiveDate As Date

    'MKW010903 PS255 - FSA Compliance START
    Private m_iFSA_CompanyCategoryID As Integer
    Private m_sFSA_StaffWording As String = ""
    'MKW010903 PS255 - FSA Compliance END
    Private m_iFSA_BankTypeID As Integer 'FSA Phase III
    'SJ 17/02/2004 - start
    Private m_iUnderwritingBranchInd As Integer
    'SJ 17/02/2004 - end

    Private m_iAllowTempMTA As Integer
    Private m_iAllowPermMTA As Integer
    Private m_iAllowReports As Integer
    Private m_iAllowClaims As Integer
    Private m_iAllowAccounts As Integer
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)


    Public Property AllowTempMTA() As Integer
        Get
            Return m_iAllowTempMTA
        End Get
        Set(ByVal Value As Integer)
            m_iAllowTempMTA = Value
        End Set
    End Property
    Public Property AllowPermMTA() As Integer
        Get
            Return m_iAllowPermMTA
        End Get
        Set(ByVal Value As Integer)
            m_iAllowPermMTA = Value
        End Set
    End Property
    Public Property AllowReports() As Integer
        Get
            Return m_iAllowReports
        End Get
        Set(ByVal Value As Integer)
            m_iAllowReports = Value
        End Set
    End Property
    Public Property AllowClaims() As Integer
        Get
            Return m_iAllowClaims
        End Get
        Set(ByVal Value As Integer)
            m_iAllowClaims = Value
        End Set
    End Property
    Public Property AllowAccounts() As Integer
        Get
            Return m_iAllowAccounts
        End Get
        Set(ByVal Value As Integer)
            m_iAllowAccounts = Value
        End Set
    End Property

    Public Property UnderwritingBranchInd() As Integer
        Get
            Return m_iUnderwritingBranchInd
        End Get
        Set(ByVal Value As Integer)
            m_iUnderwritingBranchInd = Value
        End Set
    End Property

    'MKW010903 PS255 - FSA Compliance START
    Public Property FSA_CompanyCategory_ID() As Integer
        Get
            Return m_iFSA_CompanyCategoryID
        End Get
        Set(ByVal Value As Integer)
            m_iFSA_CompanyCategoryID = Value
        End Set
    End Property

    Public Property FSA_StaffWording() As String
        Get
            Return m_sFSA_StaffWording
        End Get
        Set(ByVal Value As String)
            m_sFSA_StaffWording = Value
        End Set
    End Property
    'MKW010903 PS255 - FSA Compliance END
    'FSA Phase III
    Public Property FSA_BankType_ID() As Integer
        Get
            Return m_iFSA_BankTypeID
        End Get
        Set(ByVal Value As Integer)
            m_iFSA_BankTypeID = Value
        End Set
    End Property
    'FSA Phase IIIEnd
    Public Property DatabaseStatus() As Integer
        Get
            Return m_iDatabaseStatus
        End Get
        Set(ByVal Value As Integer)
            m_iDatabaseStatus = Value
        End Set
    End Property

    Public Property SourceID() As Integer
        Get
            Return m_iSourceIDX
        End Get
        Set(ByVal Value As Integer)
            m_iSourceIDX = Value
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

    Public Property ParentID() As Integer
        Get
            Return m_iParentID
        End Get
        Set(ByVal Value As Integer)
            m_iParentID = Value
        End Set
    End Property

    Public Property BaseCurrency() As Object
        Get
            Return m_vBaseCurrency
        End Get
        Set(ByVal Value As Object)


            m_vBaseCurrency = Value
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


    Public Property Email() As String
        Get
            Return m_sEmail.Value
        End Get
        Set(ByVal Value As String)
            m_sEmail.Value = Value
        End Set
    End Property


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
    Public Property PMSourceNumber() As Integer
        Get

            Return m_iPMSourceNumber

        End Get
        Set(ByVal Value As Integer)

            m_iPMSourceNumber = Value

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

    Public Property IsDeleted() As Integer
        Get
            Return m_iIsDeleted
        End Get
        Set(ByVal Value As Integer)
            m_iIsDeleted = Value
        End Set
    End Property

    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

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
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
