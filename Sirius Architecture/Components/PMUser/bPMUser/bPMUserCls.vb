Option Strict Off
Option Explicit On
'Developer Guide No. 129
Imports SSP.Shared

Friend NotInheritable Class PMUser
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMUser
    '
    ' Date: 24th October 1996
    '
    ' Description: Describes the PMUser attributes.
    '
    ' Edit History:
    ' RFC290398 - LoggedOnAtClient added, Timestamp removed.
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PMUser"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lPartyID As Integer
    Private m_dtPasswordChangeDate As Date
    Private m_dtDateCreated As Date
    Private m_iDeleted As Integer
    Private m_dtLastlogin As Date
    'Developer Guide No 98
    Private m_vLoggedOnAtClient As String = ""
    Private m_lPartyCnt As Integer
    Private m_iIsPMBLinkRequired As Integer
    'Developer Guide No 98
    Private m_vServerPrinter As Object
    Private m_iIsPrinterChangeable As Integer
    Private m_iIsDeleted As Integer
    Private m_dtEffectiveDate As Date
    'DAK140400
    ' PMUserSources
    Private m_oPMUserSources As Bpmuser.PMUserSources

    ' CTAF 20030721 - EmailAddress
    Private m_sEmailAddress As String = ""
    Private m_sSSOPreferredName As String = ""
    'DC040903 -start
    Private m_sFullName As String = ""
    Private m_sSignatureFile As String = ""
    Private m_sTitle As String = ""
    Private m_sTelephoneNumber As String = ""
    Private m_sExtensionNumber As String = ""
    Private m_sFaxNumber As String = ""
    Private m_lJobTitleId As Integer
    Private m_lClaimHandlerId As Integer
    Private m_lPartyHandlerId As Integer
    Private m_sInitials As String = ""
    Private m_sMobileNumber As String = ""
    Private m_lOtherPartyId As Integer '(RC) WR34
    'DC040903 -end

    'AK 08072003 - new properties (PS#246)
    Private m_sAlternativeIdentifier As String = ""


    ' ************************************************
    ' Added to replace global variables 02/04/2007
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""
    Private m_sOldPassword As String = ""
    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    Private m_lJobBasis As Integer
    Private m_dPercentHoursWorked As Double
    Private m_bIsSiriusUser As Boolean
    'Developer Guide No 98
    Private m_vDateDeleted As Object
    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.2)
    Private m_sUserConfigXMLDataset As String = ""
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.2)
    Private m_iIncorrectAttemptCount As Integer
    Private m_bIsLocked As Boolean

    Private m_bIsTempPassword As Boolean

    Private m_bIsStrongPassword As Boolean

    Private m_bIsSystemUpgradeTempPwd As Boolean
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    'DC040903 -start
    Public Property FullName() As String
        Get

            Return m_sFullName

        End Get
        Set(ByVal Value As String)

            m_sFullName = Value

        End Set
    End Property

    Public Property Initials() As String
        Get

            Return m_sInitials

        End Get
        Set(ByVal Value As String)

            m_sInitials = Value

        End Set
    End Property

    Public Property MobileNumber() As String
        Get

            Return m_sMobileNumber

        End Get
        Set(ByVal Value As String)

            m_sMobileNumber = Value

        End Set
    End Property

    Public Property SignatureFile() As String
        Get

            Return m_sSignatureFile

        End Get
        Set(ByVal Value As String)

            m_sSignatureFile = Value

        End Set
    End Property

    Public Property Title() As String
        Get

            Return m_sTitle

        End Get
        Set(ByVal Value As String)

            m_sTitle = Value

        End Set
    End Property

    Public Property TelephoneNumber() As String
        Get

            Return m_sTelephoneNumber

        End Get
        Set(ByVal Value As String)

            m_sTelephoneNumber = Value

        End Set
    End Property

    Public Property ExtensionNumber() As String
        Get

            Return m_sExtensionNumber

        End Get
        Set(ByVal Value As String)

            m_sExtensionNumber = Value

        End Set
    End Property

    Public Property FaxNumber() As String
        Get

            Return m_sFaxNumber

        End Get
        Set(ByVal Value As String)

            m_sFaxNumber = Value

        End Set
    End Property

    Public Property JobTitleId() As Integer
        Get

            Return m_lJobTitleId

        End Get
        Set(ByVal Value As Integer)

            m_lJobTitleId = Value

        End Set
    End Property

    Public Property ClaimHandlerId() As Integer
        Get

            Return m_lClaimHandlerId

        End Get
        Set(ByVal Value As Integer)

            m_lClaimHandlerId = Value

        End Set
    End Property

    Public Property PartyHandlerId() As Integer
        Get

            Return m_lPartyHandlerId

        End Get
        Set(ByVal Value As Integer)

            m_lPartyHandlerId = Value

        End Set
    End Property

    Public Property OtherPartyId() As Integer
        Get

            Return m_lOtherPartyId

        End Get
        Set(ByVal Value As Integer)

            m_lOtherPartyId = Value

        End Set
    End Property
    'DC040903 -end

    Public Property EmailAddress() As String
        Get

            Return m_sEmailAddress

        End Get
        Set(ByVal Value As String)

            m_sEmailAddress = Value

        End Set
    End Property
    Public Property SSOPreferredName() As String
        Get
            Return m_sSSOPreferredName
        End Get
        Set(ByVal Value As String)
            m_sSSOPreferredName = Value
        End Set
    End Property

    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property

    Public Property UserID() As Integer
        Get

            Return m_iUserID

        End Get
        Set(ByVal Value As Integer)

            m_iUserID = Value

        End Set
    End Property

    Public Property PartyID() As Integer
        Get

            Return m_lPartyID

        End Get
        Set(ByVal Value As Integer)

            m_lPartyID = Value

        End Set
    End Property

    Public Property LanguageID() As Integer
        Get

            Return m_iLanguageID

        End Get
        Set(ByVal Value As Integer)

            m_iLanguageID = Value

        End Set
    End Property

    Public Property Username() As String
        Get

            Return m_sUsername

        End Get
        Set(ByVal Value As String)

            m_sUsername = Value

        End Set
    End Property

    Public Property Password() As String
        Get

            Return m_sPassword

        End Get
        Set(ByVal Value As String)

            m_sPassword = Value

        End Set
    End Property
    Public Property PasswordChanged() As String

    Public Property OldPassword() As String
        Get

            Return m_sOldPassword

        End Get
        Set(ByVal Value As String)

            m_sOldPassword = Value

        End Set
    End Property


    Public Property PasswordChangeDate() As Date
        Get

            Return m_dtPasswordChangeDate

        End Get
        Set(ByVal Value As Date)

            m_dtPasswordChangeDate = Value

        End Set
    End Property

    Public Property DateCreated() As Date
        Get

            Return m_dtDateCreated

        End Get
        Set(ByVal Value As Date)

            m_dtDateCreated = Value

        End Set
    End Property

    Public Property Deleted() As Integer
        Get

            Return m_iDeleted

        End Get
        Set(ByVal Value As Integer)

            m_iDeleted = Value

        End Set
    End Property

    Public Property Lastlogin() As Date
        Get

            Return m_dtLastlogin

        End Get
        Set(ByVal Value As Date)

            m_dtLastlogin = Value

        End Set
    End Property

    Public Property LoggedOnAtClient() As String
        Get

            Return m_vLoggedOnAtClient

        End Get
        Set(ByVal Value As String)

            ' If its a string value
            If TypeOf (Value) Is String Then
                ' Trim and store

                m_vLoggedOnAtClient = CStr(Value).Trim()
            Else
                ' else, store Null

                m_vLoggedOnAtClient = Nothing
            End If

        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property IsPMBLinkRequired() As Integer
        Get

            Return m_iIsPMBLinkRequired

        End Get
        Set(ByVal Value As Integer)

            m_iIsPMBLinkRequired = Value

        End Set
    End Property

    Public Property ServerPrinter() As String
        Get

            Return m_vServerPrinter

        End Get
        Set(ByVal Value As String)

            ' If its a string value
            If TypeOf (Value) Is String Then
                ' Trim and store

                m_vServerPrinter = CStr(Value).Trim()
            Else
                ' else, store Null

                m_vServerPrinter = Nothing
            End If

        End Set
    End Property

    Public Property IsPrinterChangeable() As Integer
        Get

            Return m_iIsPrinterChangeable

        End Get
        Set(ByVal Value As Integer)

            m_iIsPrinterChangeable = Value

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

    'DAK140400
    Public Property PMUserSources() As Bpmuser.PMUserSources
        Get
            Return m_oPMUserSources
        End Get
        Set(ByVal Value As Bpmuser.PMUserSources)
            m_oPMUserSources = Value
        End Set
    End Property

    Public Property JobBasis() As Integer
        Get
            Return m_lJobBasis
        End Get
        Set(ByVal Value As Integer)
            m_lJobBasis = Value
        End Set
    End Property

    Public Property PercentHoursWorked() As Double
        Get
            Return m_dPercentHoursWorked
        End Get
        Set(ByVal Value As Double)
            m_dPercentHoursWorked = Value
        End Set
    End Property

    Public Property IsSiriusUser() As Boolean
        Get
            Return m_bIsSiriusUser
        End Get
        Set(ByVal Value As Boolean)
            m_bIsSiriusUser = Value
        End Set
    End Property

    'Developer Guide No 98
    Public Property DateDeleted() As Object
        Get
            Return m_vDateDeleted
        End Get
        Set(ByVal Value As Object)

            m_vDateDeleted = Value
        End Set
    End Property
    'Ends

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.1.1)
    Public Property UserConfigXMLDataSet() As String
        Get
            Return m_sUserConfigXMLDataset
        End Get
        Set(ByVal Value As String)
            m_sUserConfigXMLDataset = Value
        End Set
    End Property
    'AK 08072003 - new properties to support alternative identifier (PS#246)

    Public Property AlternativeIdentifier() As String
        Get

            Return m_sAlternativeIdentifier

        End Get
        Set(ByVal Value As String)

            m_sAlternativeIdentifier = Value

        End Set
    End Property

    ''' <summary>
    ''' To get and set Is Locked property of user
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsLocked() As Boolean
        Get
            Return m_bIsLocked
        End Get
        Set(ByVal Value As Boolean)
            m_bIsLocked = Value
        End Set
    End Property

    Public Property IncorrectAttemptCount() As Integer
        Get
            Return m_iIncorrectAttemptCount
        End Get
        Set(ByVal Value As Integer)
            m_iIncorrectAttemptCount = Value
        End Set
    End Property


    Public Property IsTempPassword() As Boolean
        Get
            Return m_bIsTempPassword
        End Get
        Set(ByVal value As Boolean)
            m_bIsTempPassword = value
        End Set
    End Property

    Public Property IsStrongPassword() As Boolean
        Get
            Return m_bIsStrongPassword
        End Get
        Set(ByVal value As Boolean)
            m_bIsStrongPassword = value
        End Set
    End Property

    Public Property IsSystemUpgradeTempPwd() As Boolean
        Get
            Return m_bIsSystemUpgradeTempPwd
        End Get
        Set(ByVal value As Boolean)
            m_bIsSystemUpgradeTempPwd = value
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
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.1.1)

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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        Try

            'create empty user sources collection
            m_oPMUserSources = New Bpmuser.PMUserSources()

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Shared _DefaultInstance As PMUser = Nothing
    Public Shared ReadOnly Property DefaultInstance() As PMUser
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New PMUser
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
