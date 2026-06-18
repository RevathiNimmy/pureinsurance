Option Strict On
Option Explicit On
Public NotInheritable Class SiriusUserDefaults

#Region "Constructors"
    Private Sub New()
        ' This class cannot be instantiated.
    End Sub
#End Region

    Public Const Username As String = "sirius"
    Public Const Password As String = "XctqMUbg"
    Public Const LanguageID As Int16 = 1
    Public Const CurrencyID As Int16 = 26
    Public Const SourceID As Int16 = 1
    Public Const UserID As Int32 = 1
    Public Const LogLevel As Int16 = 9
    Public Const AppName As String = "SSPPureWindowsService"

End Class

#Region " SiriusUser"

Public NotInheritable Class SIRIUSUSER

    '<remarks/>
    Public UserID As Int16 = SiriusUserDefaults.UserID

    '<remarks/>
    Public PartyCnt As Int32 = 1

    '<remarks/>
    Public LanguageID As Int16 = SiriusUserDefaults.LanguageID

    '<remarks/>
    Public Username As String = SiriusUserDefaults.Username

    '<remarks/>
    Public Password As String = SiriusUserDefaults.Password

    '<remarks/>
    Public EmailAddress As String = ""

    '<remarks/>
    Public SourceID As Int16 = SiriusUserDefaults.SourceID

    '<remarks/>
    Public CurrencyID As Int16 = SiriusUserDefaults.CurrencyID

    '<remarks/>
    Public LogLevel As Int16 = SiriusUserDefaults.LogLevel
End Class

' ***************************************************************** '
' * Return Codes                                                  * '
' ***************************************************************** '
Public Enum PMEReturnCode
    ' General
    '********
    PMFalse = 0
    PMTrue = 1
    PMFail = 10
    PMError = 11
    PMSucceed = 12
    PMOK = 20
    PMCancel = 21
    PMNavigate = 30
    ' Broking Link Set 1
    '*************
    PMMNoAuthority = 51
    PMMAlreadyInUse = 52
    PMMInvalidPassword = 53
    PMMNoAccess = 54
    ' System
    '*******
    PMIncorrectUsername = 200
    PMIncorrectPassword = 201
    PMLoggedOnElsewhere = 202
    PMLogError1 = 210
    PMLogError2 = 211
    ' Interface
    '**********
    PMMoveStatusBack = 400
    PMMoveStatusNext = 401
    PMMoveStatusCancel = 402
    PMMoveStatusFinish = 403
    ' Broking Link Set 2
    '*************
    PMError_argcount = 500
    PMError_protocol = 501
    PMError_notconnected = 502
    PMError_timeout = 503
    PMError_usage = 504
    ' Business
    '*********
    PMLogonExceeded = 600
    PMLicenceExceeded = 601
    PMInvalidLicenceKey = 602
    PMDataChanged = 610
    PMMandatoryMissing = 611
    PMDataNotChanged = 612
    PMInvalidRequest = 620
    PMIncorrectDateFormat = 621
    PMIncorrectSystemDate = 622
    PMEarlier = 623
    PMLater = 624
    PMInstallStarted = 625
    PMBlockLicenceExceeded = 626
    PMWarnLicenceExceeded = 627
    PMInvalidRiskStatus = 628
    ' Navigator Return Codes
    '***********************
    PMNavStartNewProcess = 700
    PMNavCallComponent = 701
    PMNavBuildMap = 702
    PMNavRepeatMap = 703
    PMNavEndMap = 704
    PMNavNavigate = 705
    PMNavEndProcess = 706
    ' Database
    '*********
    PMRecordChanged = 800
    PMRecordDeleted = 801
    PMRecordInUse = 810
    PMNotFound = 811
    PMBOF = 820
    PMEOF = 821
    ' Broking Link Set 3
    '*************
    PMNoHostRegistry = 1002
    PMNoPortRegistry = 1003
    PMNoConnection = 1004
    PMNoPMLink = 1005
    PMNoCompanies = 1006

    ' Agents/Customers Online
    ' ***********************
    PMNoEmailAddress = 1100
    PMFailedEmail = 1101
    PMUpdateUserFailed = 1102
    PMUserNotExist = 1103
    PMUserNotLinkedAgent = 1104 ' When a user isn't linked to an agent
    PMGISOutDated = 1105 ' When the GIS doesn't support a method that the STS requires
    PMBusinessRuleError = 3001
    PMRenewalAlreadyAccepted = 60132
    PMNBQuoteReferred = 9999901
    PMNBQuoteDeclined = 9999902

End Enum

#End Region
