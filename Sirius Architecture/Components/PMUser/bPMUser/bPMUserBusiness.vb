Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Xml
Imports SSP.Shared
'Developer Guide No. 129
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 24th October 1996
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMUser.
    '
    ' Edit History:
    ' RFC160698 - CurrentRecord property Let/Get added.
    ' RFC290398 - LoggedOnAtClient added, Timestamp removed.
    ' RFC270398 - Logon and Logoff methods added.
    ' RFC250398 - Changed to use Sirius Architecture DSN
    ' RFC250398 - Product Family Property Get Added.
    ' DAK231199 - Check PMProduct lookup for permissions
    ' DAK221299 - PMProductLookup.GetDetails - PMProductID changed to
    '             variant.
    ' DAK140400 - Add PMUserSources
    ' DAK190500 - Get user details using user id parameter
    ' DAK190600 - Get is_deleted and effective_date for each source
    ' RKS101204 - Implementation of Unified Logon Process
    ' AG161204  - Add GetUsersWithNoPassword method
    ' AG161204  - Add DeleteUserMappings method
    ' AG161204  - Add UpdateUserMapping method
    ' AG161204  - Add SetProductOptionValue method
    ' AG161204  - Add GetAdminUserCount method
    ' AG161204  - Add GetUserAdminStatus method
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 02/04/2007
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_bIsLocked As Boolean
    Private m_bIsTempPassword As Boolean
    Private m_sPasswordLifecycleDays As String
    Private m_sTempPasswordValDur As String
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Collection of PMUsers (Private)
    Private m_oPMUsers As Bpmuser.PMUsers

    'DAK140400
    ' Collection of PMUserSources (Private)
    Private m_oPMUserSources As Bpmuser.PMUserSources

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer (Private)
    Private m_lCurrentRecord As gPMConstants.PMEReturnCode

    ' Error Code (Private)
    Private m_lError As Integer

    Private m_oLookup As BPMLOOKUP.Business

    Private m_sUnderwritingOrAgency As String = ""

    ' replace with version in gSIRLibrary
    Private Const SIRLookupJobTitle As String = "job_title"

    Private m_bLockingEnabled As Boolean

    Private m_lIncorrectAttemptCount As Integer
    ' AMB 26-Nov-03: - end

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' RFC250398 - Product Family Property Get Added.
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    ' RFC160698 - CurrentRecord property Let/Get added.
    Public Property CurrentRecord() As Integer
        Get
            Return m_lCurrentRecord
        End Get
        Set(ByVal Value As Integer)
            If Value < 0 Then
                m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
            ElseIf (Value > m_oPMUsers.Count()) Then
                m_lCurrentRecord = m_oPMUsers.Count()
            Else
                m_lCurrentRecord = Value
            End If
        End Set
    End Property

    'DC180903
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    Public Property m_oPMUserSync() As Object
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Have we a valid Database Object Reference?

            If (Not Informations.IsNothing(vDatabase)) And (Informations.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
                m_oDatabase = New dPMDAO.Database()

                ' Open the Database
                ' RFC250398 - Changed to use Sirius Architecture DSN
                ' RDC 27062002 changed to use Comp Serv
                m_lError = gPMComponentServices.NewDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If

            'create empty users collection
            m_oPMUsers = New Bpmuser.PMUsers()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            'DC180903
            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            m_bLockingEnabled = False

            Dim m_sUserAttemptCount As String = ""

            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=sUsername, v_sPassword:=sPassword, v_iUserID:=iUserID, v_iMainSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_iCurrencyID:=iCurrencyID, v_iLogLevel:=iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5107, r_sOptionValue:=m_sUserAttemptCount, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=sUsername, v_sPassword:=sPassword, v_iUserID:=iUserID, v_iMainSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_iCurrencyID:=iCurrencyID, v_iLogLevel:=iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5103, r_sOptionValue:=m_sPasswordLifecycleDays, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=sUsername, v_sPassword:=sPassword, v_iUserID:=iUserID, v_iMainSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_iCurrencyID:=iCurrencyID, v_iLogLevel:=iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5109, r_sOptionValue:=m_sTempPasswordValDur, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Double.TryParse(m_sUserAttemptCount, 0) AndAlso ToSafeInteger(m_sUserAttemptCount) > 0 Then
                m_lIncorrectAttemptCount = ToSafeInteger(m_sUserAttemptCount)
                m_bLockingEnabled = True
            Else
                m_lIncorrectAttemptCount = 0
                m_bLockingEnabled = False
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: CheckLogon
    '
    ' Description: Validates the Logon details supplied.
    '
    ' ***************************************************************** '
    Public Function CheckLogon(ByRef sCheckUsername As String, ByRef sCheckPassword As String, ByRef dtEffectiveFrom As Date, ByRef iLanguageID As Integer, ByRef lPartyCnt As Integer) As Integer
        Return CheckLogon(sCheckUsername:=sCheckUsername, sCheckPassword:=sCheckPassword, dtEffectiveFrom:=dtEffectiveFrom, iLanguageID:=iLanguageID, lPartyCnt:=lPartyCnt, vUserId:=0, sLoggedOnAtClient:="", iIsPMBLinkRequired:=0, sServerPrinter:="", iIsPrinterChangeable:=0, sUserConfigXMLDataSet:="", sIsTempPassword:="", oPasswordChangeDate:=Nothing, bIsLocked:=False)
    End Function

    Public Function CheckLogon(ByRef sCheckUsername As String, ByRef sCheckPassword As String, ByRef dtEffectiveFrom As Date, ByRef iLanguageID As Integer, ByRef lPartyCnt As Integer, ByRef vUserId As Integer) As Integer
        Return CheckLogon(sCheckUsername:=sCheckUsername, sCheckPassword:=sCheckPassword, dtEffectiveFrom:=dtEffectiveFrom, iLanguageID:=iLanguageID, lPartyCnt:=lPartyCnt, vUserId:=vUserId, sLoggedOnAtClient:="", iIsPMBLinkRequired:=0, sServerPrinter:="", iIsPrinterChangeable:=0, sUserConfigXMLDataSet:="", sIsTempPassword:="", oPasswordChangeDate:=Nothing, bIsLocked:=False)
    End Function

    Public Function CheckLogon(ByRef sCheckUsername As String, ByRef sCheckPassword As String, ByRef dtEffectiveFrom As Date, ByRef iLanguageID As Integer, ByRef lPartyCnt As Integer, ByRef vUserId As Integer, ByRef sLoggedOnAtClient As String) As Integer
        Return CheckLogon(sCheckUsername:=sCheckUsername, sCheckPassword:=sCheckPassword, dtEffectiveFrom:=dtEffectiveFrom, iLanguageID:=iLanguageID, lPartyCnt:=lPartyCnt, vUserId:=vUserId, sLoggedOnAtClient:=sLoggedOnAtClient, iIsPMBLinkRequired:=0, sServerPrinter:="", iIsPrinterChangeable:=0, sUserConfigXMLDataSet:="", sIsTempPassword:="", oPasswordChangeDate:=Nothing, bIsLocked:=False)
    End Function

    Public Function CheckLogon(ByRef sCheckUsername As String, ByRef sCheckPassword As String, ByRef dtEffectiveFrom As Date, ByRef iLanguageID As Integer, ByRef lPartyCnt As Integer, ByRef vUserId As Integer, ByRef sLoggedOnAtClient As String, ByRef iIsPMBLinkRequired As Integer, ByRef sServerPrinter As String, ByRef iIsPrinterChangeable As Integer, ByRef sUserConfigXMLDataSet As String, ByRef sIsTempPassword As String, ByRef oPasswordChangeDate As Object, ByRef bIsLocked As Boolean) As Integer

        Dim result As Integer = 0
        Dim sNewLicenceKey As String = ""
        Dim lRecordCount As Integer

        Dim sSPCheckLogon As String = ""
        Dim vRetval As String = ""

        Dim bUnifiedLogon, bMixedModeLogon As Boolean
        Dim sEncrCheckPassword As String = ""

        Dim oPMUser As Bpmuser.PMUser
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' This validation has been added because Alternative_Identifier field can
            ' have empty string for standard user's records, if something goes wrong and
            ' GetNTUsernameEX will return an empty value then it is likely to be possible
            ' that one can grant logon (in Unified Mode) with blank username.
            ' Access will be denied if username is empty
            If sCheckUsername.Length = 0 Then
                Return gPMConstants.PMEReturnCode.PMIncorrectUsername
            End If



            oPMUser = New Bpmuser.PMUser()

            'Getting the "System Security Model" i.e. (AlternativeLogon option)
            m_lReturn = CType(bPMFunc.GetSystemSecurityModel(vRetval), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case vRetval
                Case "0", "" 'need to include "" - if alternativelogon productoption is disabled
                    'Standard Sirius Login
                    'Access will be denied if password is empty in case of Standard Sirius Login
                    If sCheckPassword.Length = 0 Then
                        Return gPMConstants.PMEReturnCode.PMIncorrectPassword
                    End If
                    sSPCheckLogon = ACCheckLogonSQL
                Case "1"
                    'Mixed Mode
                    'It is assumed that if the password is blank then a unified logon
                    'is being used
                    If sCheckPassword.Length = 0 Then
                        bUnifiedLogon = True
                        sSPCheckLogon = kCheckAlternativeLogonSQL
                    Else
                        sSPCheckLogon = ACCheckLogonSQL
                    End If
                    bMixedModeLogon = True
                Case "2"
                    'Unified Only (Windows)
                    bUnifiedLogon = True
                    sSPCheckLogon = kCheckAlternativeLogonSQL
            End Select


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Username parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="username", vValue:=CStr(sCheckUsername), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Effective Date parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=CStr(dtEffectiveFrom), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=sSPCheckLogon, sSQLName:=ACCheckLogonName, bStoredProcedure:=ACCheckLogonStored, lNumberRecords:=0, bKeepNulls:=True)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' How many records do we have ?
            If lRecordCount <> 1 Then
                ' If more than one record is found, then send back "Incorrect User Name"
                If bUnifiedLogon Then
                    If bMixedModeLogon Then
                        Return gPMConstants.PMEReturnCode.PMMixedModeIncorrectUserName
                    Else
                        Return gPMConstants.PMEReturnCode.PMUnifiedModeIncorrectUserName
                    End If
                Else
                    Return gPMConstants.PMEReturnCode.PMIncorrectUsername
                End If
            Else
                m_lError = SetPropertiesFromDB(oPMUser:=oPMUser, lRecordNumber:=1)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'in case of Mixed Mode, standard user can only login if user
                'record has not any alternative_identifier specified
                If bMixedModeLogon And Not bUnifiedLogon Then
                    'standard user login (in Mixed Mode)
                    'if alternative_identifier exists, raise an error - User can only
                    'login using Unified login
                    If (oPMUser.AlternativeIdentifier).Length > 0 Then
                        Return gPMConstants.PMEReturnCode.PMIncorrectUsername
                    End If
                End If


                ' HG151103 - Only check passwords when not in the unified process mode
                ' This stops the need for blank passwords being required in PMUser when
                ' using a unified logon

                If Not bUnifiedLogon Then

                    m_lError = bPMFunc.LicenceEncrypt(sCheckPassword, sEncrCheckPassword)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' Compare supplied Password with one from User record.
                    If oPMUser.Password = String.Empty Then
                        If sEncrCheckPassword.Trim() <> oPMUser.OldPassword.Trim() Then
                            Return gPMConstants.PMEReturnCode.PMIncorrectPassword
                        End If
                    Else
                        m_lError = bPMFunc.CheckPassword(sCheckPassword, oPMUser.Password)
                    End If

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Incorrect Password
                        If m_bLockingEnabled Then
                            UpdateIncorrectAttemptsAndLockUnlock(oPMUser.UserID, 1, m_lIncorrectAttemptCount, oPMUser.IsLocked)
                            If oPMUser.IsLocked Then
                                Return gPMConstants.PMEReturnCode.PMLogonExceeded
                            End If
                        End If
                        Return gPMConstants.PMEReturnCode.PMIncorrectPassword

                    ElseIf oPMUser.IsLocked And m_bLockingEnabled Then
                        Return gPMConstants.PMEReturnCode.PMUserAccountLocked
                        'Lock Password is Temporary and Expired
                    ElseIf oPMUser.IsTempPassword And oPMUser.PasswordChangeDate.AddDays(ToSafeInteger(m_sTempPasswordValDur)) < Date.Today And m_bLockingEnabled And m_sTempPasswordValDur <> 0 Then
                        UpdateIncorrectAttemptsAndLockUnlock(oPMUser.UserID, 1, m_lIncorrectAttemptCount, 1)
                        Return gPMConstants.PMEReturnCode.PMUserAccountLocked
                    ElseIf oPMUser.Password = String.Empty Then
                        result = gPMConstants.PMEReturnCode.PMNewBuildUpgrade
                    ElseIf oPMUser.PasswordChangeDate.AddDays(ToSafeInteger(m_sPasswordLifecycleDays)) < Date.Today And m_sPasswordLifecycleDays <> 0 And oPMUser.IsTempPassword <> True Then
                        result = gPMConstants.PMEReturnCode.PMUserPasswordExpired
                    ElseIf oPMUser.IsTempPassword Then
                        result = gPMConstants.PMEReturnCode.PMUserTemporaryPassword
                    ElseIf gPMFunctions.IsStrongPassword(v_sUsername:=sCheckUsername, v_iUserID:=oPMUser.UserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, sPasswordString:=sCheckPassword, bIsstrongPassword:=oPMUser.IsStrongPassword, v_iSourceID:=m_iSourceID) = gPMConstants.PMEReturnCode.PMTrue AndAlso oPMUser.IsStrongPassword = False Then
                        result = gPMConstants.PMEReturnCode.PMUserWeakPassword
                        ' Reset attempt count to 0 in case password is correct
                    ElseIf m_bLockingEnabled Then
                        UpdateIncorrectAttemptsAndLockUnlock(oPMUser.UserID, 2, m_lIncorrectAttemptCount, oPMUser.IsLocked)
                    End If

                End If


                ' If we have been asked to check where User is logged on
                If sLoggedOnAtClient.Trim() <> "" Then
                    ' If the user is already logged on

                    If Not (Convert.IsDBNull(oPMUser.LoggedOnAtClient) Or Informations.IsNothing(oPMUser.LoggedOnAtClient) OrElse oPMUser.LoggedOnAtClient = "") Then
                        ' Is the User already logged on at another client
                        If sLoggedOnAtClient.Trim() <> oPMUser.LoggedOnAtClient Then
                            'Yes, so return where they are already logged on.
                            If bUnifiedLogon Then
                                If bMixedModeLogon Then
                                    sLoggedOnAtClient = oPMUser.LoggedOnAtClient
                                    Return gPMConstants.PMEReturnCode.PMMixedModeUserLoggedOnElsewhere
                                Else
                                    sLoggedOnAtClient = oPMUser.LoggedOnAtClient
                                    Return gPMConstants.PMEReturnCode.PMUnifiedModeUserLoggedOnElsewhere
                                End If
                            Else
                                sLoggedOnAtClient = oPMUser.LoggedOnAtClient
                                Return gPMConstants.PMEReturnCode.PMLoggedOnElsewhere
                            End If
                        End If
                    End If
                End If

                With oPMUser
                    'set the return parameters from the populated user object
                    vUserId = .UserID
                    iLanguageID = .LanguageID
                    lPartyCnt = .PartyCnt
                    ' Return is PMLink required
                    iIsPMBLinkRequired = .IsPMBLinkRequired
                    ' Return Server Printer & IsPrinterChangeable

                    If Convert.IsDBNull(.ServerPrinter) Or Informations.IsNothing(.ServerPrinter) Then
                        sServerPrinter = ""
                    Else
                        sServerPrinter = .ServerPrinter
                    End If
                    iIsPrinterChangeable = .IsPrinterChangeable
                    'Unified logon: return user name
                    If bUnifiedLogon Then
                        sCheckUsername = .Username
                    End If
                    sUserConfigXMLDataSet = .UserConfigXMLDataSet
                    sIsTempPassword = .IsTempPassword
                    oPasswordChangeDate = .PasswordChangeDate
                    bIsLocked = .IsLocked
                End With
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get a valid system record", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckLogon", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Logon
    '
    ' Description: Logs On the User supplied.
    '
    ' ***************************************************************** '
    Public Function Logon(ByVal v_sUsername As String, ByVal v_sLoggedOnAtClient As String) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Username parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="username", vValue:=CStr(v_sUsername), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the LastLogin parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="lastlogin", vValue:=CStr(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the LastLogin parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="logged_on_at_client", vValue:=CStr(v_sLoggedOnAtClient), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLAction(sSQL:=ACLogonSQL, sSQLName:=ACLogonName, bStoredProcedure:=ACLogonStored, lRecordsAffected:=lRecordsAffected)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lRecordsAffected < 1 Then
                ' Something went wrong
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Log the fact that the user has logged on.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="User " & v_sUsername & " logged on", vApp:=ACApp, vClass:=ACClass, vMethod:="Logon")

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to logon user " & v_sUsername, vApp:=ACApp, vClass:=ACClass, vMethod:="Logon", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Logoff
    '
    ' Description: Logs off the User supplied.
    '
    ' ***************************************************************** '
    Public Function Logoff(ByVal v_sUsername As String) As Integer
        Return Logoff(v_sUsername:=v_sUsername, sUserConfigXMLDataSet:="")
    End Function

    Public Function Logoff(ByVal v_sUsername As String, ByVal sUserConfigXMLDataSet As String) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            If Not m_oDatabase.Parameters Is Nothing Then
                m_oDatabase.Parameters.Clear()

                ' Add the Username parameter (INPUT)
                m_lError = m_oDatabase.Parameters.Add(sName:="username", vValue:=CStr(v_sUsername), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
                m_lError = m_oDatabase.Parameters.Add(sName:="user_config_xml_dataset", vValue:=sUserConfigXMLDataSet, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLAction(sSQL:=ACLogoffSQL, sSQLName:=ACLogoffName, bStoredProcedure:=ACLogoffStored, lRecordsAffected:=lRecordsAffected)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lRecordsAffected < 1 Then
                    ' Something went wrong
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Log the fact that the user has logged on.
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="User " & v_sUsername & " logged off", vApp:=ACApp, vClass:=ACClass, vMethod:="Logoff")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Logoff user " & v_sUsername, vApp:=ACApp, vClass:=ACClass, vMethod:="Logoff", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds a single PMUser directly into the database.
    '              Note: The PMUser will NOT be added to the collection.
    '
    'AK 08072003 - new parameter for alternativeidentifier ps#246
    ' AMB 24-Nov-03: 1.8.6 Unified login code from 1.9
    ' ***************************************************************** '
    'DC040903
    Public Function Add(ByRef iUserID As Integer, ByRef iLanguageID As Integer, ByRef sUsername As String,
                        ByRef sPassword As String, ByRef dtPasswordChangeDate As Date, ByRef dtDateCreated As Date,
                        ByRef dtLastlogin As Date, ByRef lPartyCnt As Integer, ByRef iIsDeleted As Integer,
                        ByRef dtEffectiveDate As Date, Optional ByRef vIsPMBLinkRequired As Object = Nothing,
                        Optional ByRef vServerPrinter As Object = Nothing,
                        Optional ByRef vIsPrinterChangeable As Object = Nothing,
                        Optional ByRef vEmailAddress As Object = Nothing, Optional ByRef vFullName As Object = Nothing,
                        Optional ByRef vSignatureFile As Object = Nothing, Optional ByRef vTitle As Object = Nothing,
                        Optional ByRef vTelephoneNumber As Object = Nothing,
                        Optional ByRef vExtensionNumber As Object = Nothing,
                        Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vJobTitleID As Object = Nothing,
                        Optional ByRef vClaimHandlerId As Object = Nothing,
                        Optional ByRef vPartyHandlerId As Object = Nothing, Optional ByRef vInitials As Object = Nothing,
                        Optional ByRef vMobileNumber As Object = Nothing,
                        Optional ByRef vOtherPartyId As Object = Nothing,
                        Optional ByRef vAlternativeIdentifier As Object = Nothing,
                        Optional ByRef vJobBasis As Object = Nothing,
                        Optional ByRef vPercentHoursWorked As Object = Nothing,
                        Optional ByRef vSiriusUser As Object = Nothing, Optional ByRef vDateDeleted As Object = Nothing,
                        Optional vIsTempPassword As Object = Nothing,
                        Optional sOldPassword As String = "",
                        Optional ByVal sPasswordChanged As String = "", Optional ByVal sUniqueId As String = "",
                        Optional ByVal sScreenHierarchy As String = "", Optional ByRef vSSOPreferredName As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMUser As bPMUser.PMUser

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new PMUser
            oPMUser = New bPMUser.PMUser()

            'Developer Guide No 98
            m_lError = SetProperties(oPMUser:=oPMUser, iStatus:=gPMConstants.PMEComponentAction.PMAdd,
                                     vUserId:=iUserID, vLanguageID:=iLanguageID, vUsername:=sUsername,
                                     vPassword:=sPassword, vPasswordChangeDate:=dtPasswordChangeDate,
                                     vDateCreated:=dtDateCreated, vLastLogin:=dtLastlogin, vPartyCnt:=lPartyCnt,
                                     vIsDeleted:=iIsDeleted, vEffectiveDate:=dtEffectiveDate,
                                     vIsPMBLinkRequired:=vIsPMBLinkRequired, vServerPrinter:=vServerPrinter,
                                     vIsPrinterChangeable:=vIsPrinterChangeable, vEmailAddress:=vEmailAddress,
                                     vFullName:=vFullName, vSignatureFile:=vSignatureFile, vTitle:=vTitle,
                                     vTelephoneNumber:=vTelephoneNumber, vExtensionNumber:=vExtensionNumber,
                                     vFaxNumber:=vFaxNumber, vJobTitleID:=vJobTitleID,
                                     vClaimHandlerId:=vClaimHandlerId, vPartyHandlerId:=vPartyHandlerId,
                                     vInitials:=vInitials, vMobileNumber:=vMobileNumber,
                                     vOtherPartyId:=vOtherPartyId, vAlternativeIdentifier:=vAlternativeIdentifier,
                                     vJobBasis:=vJobBasis, vPercentHoursWorked:=vPercentHoursWorked,
                                     vSiriusUser:=vSiriusUser, vDateDeleted:=vDateDeleted,
                                     vIsTempPassword:=vIsTempPassword, sOldPassword:=sOldPassword,
                                     sPasswordChanged:=sPasswordChanged, sUniqueId:=sUniqueId,
                                     sScreenHierarhcy:=sScreenHierarchy, vSSOPreferredName:=vSSOPreferredName)


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the PMUser to the Database
            m_lError = AddItem(oPMUser)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the ID of the PMUser Added
            iUserID = oPMUser.UserID
            m_oPMUserSync = oPMUser
            oPMUser = Nothing
            If m_oPMUserSync IsNot Nothing Then
                SyncUser(1)
            End If
            Return result

        Catch excep As System.Exception
            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function
    Public Sub SyncUser(ByVal AddOrEdit As Integer)
        Dim keyCloakConfigSettings As New KeyCloakConfiguration()
        Dim sEnableAuthentication As String = String.Empty
        m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5249, r_sOptionValue:=sEnableAuthentication, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to get system options for Authentication Integration", vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
        End If
        If sEnableAuthentication = "1" Then
            Dim sRealm As String = String.Empty
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5250, r_sOptionValue:=sRealm, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to get system options for Realm", vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
            End If
            keyCloakConfigSettings.Realm = sRealm

            Dim sClientID As String = String.Empty
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5251, r_sOptionValue:=sClientID, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to get system options for Client Id", vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
            End If
            keyCloakConfigSettings.client_id = sClientID

            Dim sClientSecret As String = String.Empty
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5252, r_sOptionValue:=sClientSecret, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to get system options for Client Secret", vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
            End If
            keyCloakConfigSettings.client_secret = sClientSecret

            Dim sUserName As String = String.Empty
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5253, r_sOptionValue:=sUserName, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to get system options for username", vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
            End If
            keyCloakConfigSettings.username = sUserName

            Dim sPassword As String = String.Empty
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5254, r_sOptionValue:=sPassword, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to get system options for password", vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
            End If
            keyCloakConfigSettings.Password = bPMFunc.GetOVal(sPassword)

            Dim sGroup As String = String.Empty
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5255, r_sOptionValue:=sGroup, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to get system options for User Group", vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
            End If
            keyCloakConfigSettings.AdminGroupName = sGroup

            Dim sTokenEndPoint As String = String.Empty
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5256, r_sOptionValue:=sTokenEndPoint, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Unable to get system options for TokenUrl", vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
            End If
            keyCloakConfigSettings.TokenEndpoint = sTokenEndPoint
            keyCloakConfigSettings.grant_type = "password"
            Dim oPMUser As Bpmuser.PMUser
            oPMUser = CType(m_oPMUserSync, Bpmuser.PMUser)
            If Not IsNothing(keyCloakConfigSettings) Then
                Dim adminGroup As String = ""
                If oPMUser.IsSiriusUser Then
                    adminGroup = keyCloakConfigSettings.AdminGroupName
                End If
                Dim usersSync As New SSP.Pure.UsersSync.Services.AuthenticationService(keyCloakConfigSettings)
                Dim firstName As String
                Dim lastName As String
                If String.IsNullOrEmpty(oPMUser.FullName) Then
                    firstName = oPMUser.Username
                    lastName = oPMUser.Username
                Else
                    Dim str() = oPMUser.FullName.Split(" ")

                    If str.Length > 0 Then

                        If str.Length = 1 Then
                            firstName = str(0)
                            lastName = oPMUser.Username
                        Else
                            firstName = str(0)
                            lastName = str(str.Length - 1)
                        End If

                    Else
                        firstName = oPMUser.FullName
                        lastName = oPMUser.Username
                    End If
                End If

                Dim user = New SSP.Pure.UsersSync.Contracts.UserRegisterRequestDTO(oPMUser.Username, oPMUser.EmailAddress, oPMUser.PasswordChanged, adminGroup, "0", firstName, lastName, oPMUser.IsDeleted)
                If AddOrEdit = 1 Then
                    Try
                        Dim response = usersSync.RegisterUserAsync(user)
                        If (response IsNot Nothing AndAlso response.Result IsNot Nothing) Then
                            If (response.Result.User IsNot Nothing AndAlso response.Result.User.Error IsNot Nothing AndAlso String.IsNullOrEmpty(response.Result.User.Error.ErrorResponseCode) = False) Then
                                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Cannot Add User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails, vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
                            ElseIf (response.Result.Error IsNot Nothing) Then
                                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Cannot Add User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails, vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
                            End If
                        End If
                        If String.IsNullOrEmpty(usersSync.GetUserAsync(oPMUser.Username).Result) = True Then
                            ' Log Error Message
                            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Cannot Add User Details To KeyCloak Server", vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
                        End If
                    Catch excep As System.Exception
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot Add User Details To KeyCloak Server", vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
                    End Try
                Else
                    user = New SSP.Pure.UsersSync.Contracts.UserRegisterRequestDTO(oPMUser.Username, oPMUser.EmailAddress, oPMUser.PasswordChanged, adminGroup, "0", firstName, lastName, oPMUser.IsDeleted)
                    Try
                        Dim id = usersSync.GetUserAsync(oPMUser.Username).Result
                        If String.IsNullOrEmpty(id) = True Then
                            Dim response = usersSync.RegisterUserAsync(user)
                            If (response IsNot Nothing AndAlso response.Result IsNot Nothing) Then
                                If (response.Result.User IsNot Nothing AndAlso response.Result.User.Error IsNot Nothing AndAlso String.IsNullOrEmpty(response.Result.User.Error.ErrorResponseCode) = False) Then
                                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Cannot Add User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails, vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
                                ElseIf (response.Result.Error IsNot Nothing) Then
                                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Cannot Add User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails, vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
                                End If
                            End If
                        Else
                            user = New SSP.Pure.UsersSync.Contracts.UserRegisterRequestDTO(oPMUser.Username, oPMUser.EmailAddress, oPMUser.PasswordChanged, adminGroup, id, firstName, lastName, oPMUser.IsDeleted)
                            Dim response = usersSync.UpdateUserAsync(user)
                            If (response IsNot Nothing AndAlso response.Result IsNot Nothing) Then
                                If (response.Result.User IsNot Nothing AndAlso response.Result.User.Error IsNot Nothing AndAlso String.IsNullOrEmpty(response.Result.User.Error.ErrorResponseCode) = False) Then
                                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Cannot Update User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails, vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
                                ElseIf (response.Result.Error IsNot Nothing) Then
                                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Cannot Update User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails, vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
                                End If
                            End If
                        End If
                    Catch
                        Dim response = usersSync.RegisterUserAsync(user)
                        If (response IsNot Nothing AndAlso response.Result IsNot Nothing) Then
                            If (response.Result.User IsNot Nothing AndAlso response.Result.User.Error IsNot Nothing AndAlso String.IsNullOrEmpty(response.Result.User.Error.ErrorResponseCode) = False) Then
                                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Cannot Add User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails, vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
                            ElseIf (response.Result.Error IsNot Nothing) Then
                                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Cannot Add User Details To KeyCloak Server." & response.Result.User.Error.ErrorResponseCode & ":" & response.Result.User.Error.ErrorDetails, vApp:=ACApp, vClass:=ACClass, vMethod:="SyncUser")
                            End If
                        End If
                    End Try
                End If
            End If
        End If
    End Sub
    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required PMUsers and populate the Collection
    ' for the UserId if supplied, else returns all of the users
    '
    ' ***************************************************************** '
    Public Function GetDetails() As Integer
        Return GetDetails(vUserId:=Nothing, v_bIncludeDeletedSources:=False, v_lProductID:=0)
    End Function

    Public Function GetDetails(ByRef vUserId As Object) As Integer
        Return GetDetails(vUserId:=vUserId, v_bIncludeDeletedSources:=False, v_lProductID:=0)
    End Function

    Public Function GetDetails(ByRef vUserId As Object, ByVal v_bIncludeDeletedSources As Boolean) As Integer
        Return GetDetails(vUserId:=vUserId, v_bIncludeDeletedSources:=v_bIncludeDeletedSources, v_lProductID:=0)
    End Function

    Public Function GetDetails(ByRef vUserId As Object, ByVal v_bIncludeDeletedSources As Boolean, ByVal v_lProductID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPMUser As Bpmuser.PMUser

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.
            m_lError = Cancel()

            ' If changes are outstanding, exit.
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lError
            End If

            ' Clear the Collection
            m_oPMUsers.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            'if the userid was supplied

            If Not Informations.IsNothing(vUserId) Then

                ' If the supplied keys are not valid, exit

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vUserId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : MessageID=" & CStr(vUserId), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add the UserId parameter (INPUT)

                m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(vUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' select all of the pmuser records

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' Yes, load them into the collection

                For lSub As Integer = 1 To lRecordCount

                    ' Create New PMUser
                    oPMUser = New Bpmuser.PMUser()

                    m_lError = SetPropertiesFromDB(oPMUser:=oPMUser, lRecordNumber:=lSub)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add PMUser to collection
                    If (m_oPMUsers.Count = 0) Then
                        m_oPMUsers.Add(Nothing)
                    End If
                    m_lError = m_oPMUsers.Add(oNewPMUser:=oPMUser)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPMUser = Nothing

                Next lSub

                'DAK030500
                m_lError = GetPMUserSources(v_bIncludeDeletedSources, v_lProductID)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' GetNext (Public)- Gets the required PMUsers and populate the Collection
    ''' </summary>
    ''' <param name="vUserId"></param>
    ''' <param name="vLanguageID"></param>
    ''' <param name="vUsername"></param>
    ''' <param name="vPassword"></param>
    ''' <param name="vPasswordChangeDate"></param>
    ''' <param name="vDateCreated"></param>
    ''' <param name="vLastLogin"></param>
    ''' <param name="vPartyCnt"></param>
    ''' <param name="vIsDeleted"></param>
    ''' <param name="vEffectiveDate"></param>
    ''' <param name="vLoggedOnAtClient"></param>
    ''' <param name="vIsPMBLinkRequired"></param>
    ''' <param name="vServerPrinter"></param>
    ''' <param name="vIsPrinterChangeable"></param>
    ''' <param name="vEmailAddress"></param>
    ''' <param name="vFullName"></param>
    ''' <param name="vSignatureFile"></param>
    ''' <param name="vTitle"></param>
    ''' <param name="vTelephoneNumber"></param>
    ''' <param name="vExtensionNumber"></param>
    ''' <param name="vFaxNumber"></param>
    ''' <param name="vJobTitleID"></param>
    ''' <param name="vClaimHandlerId"></param>
    ''' <param name="vPartyHandlerId"></param>
    ''' <param name="vInitials"></param>
    ''' <param name="vMobileNumber"></param>
    ''' <param name="vOtherPartyId"></param>
    ''' <param name="vAlternativeIdentifier"></param>
    ''' <param name="vJobBasis"></param>
    ''' <param name="vPercentHoursWorked"></param>
    ''' <param name="vSiriusUser"></param>
    ''' <param name="vDateDeleted"></param>
    ''' <param name="o_bIsUserTempPassword"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetNext(Optional ByRef vUserId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing,
                            Optional ByRef vUsername As Object = Nothing, Optional ByRef vPassword As Object = Nothing,
                            Optional ByRef vPasswordChangeDate As Object = Nothing,
                            Optional ByRef vDateCreated As Object = Nothing,
                            Optional ByRef vLastLogin As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing,
                            Optional ByRef vIsDeleted As Object = Nothing,
                            Optional ByRef vEffectiveDate As Object = Nothing,
                            Optional ByRef vLoggedOnAtClient As Object = Nothing,
                            Optional ByRef vIsPMBLinkRequired As Object = Nothing,
                            Optional ByRef vServerPrinter As Object = Nothing,
                            Optional ByRef vIsPrinterChangeable As Object = Nothing,
                            Optional ByRef vEmailAddress As Object = Nothing,
                            Optional ByRef vFullName As Object = Nothing,
                            Optional ByRef vSignatureFile As Object = Nothing, Optional ByRef vTitle As Object = Nothing,
                            Optional ByRef vTelephoneNumber As Object = Nothing,
                            Optional ByRef vExtensionNumber As Object = Nothing,
                            Optional ByRef vFaxNumber As Object = Nothing,
                            Optional ByRef vJobTitleID As Object = Nothing,
                            Optional ByRef vClaimHandlerId As Object = Nothing,
                            Optional ByRef vPartyHandlerId As Object = Nothing,
                            Optional ByRef vInitials As Object = Nothing,
                            Optional ByRef vMobileNumber As Object = Nothing,
                            Optional ByRef vOtherPartyId As Object = Nothing,
                            Optional ByRef vAlternativeIdentifier As Object = Nothing,
                            Optional ByRef vJobBasis As Object = Nothing,
                            Optional ByRef vPercentHoursWorked As Object = Nothing,
                            Optional ByRef vSiriusUser As Object = Nothing,
                            Optional ByRef vDateDeleted As Object = Nothing,
                            Optional ByRef o_bIsUserTempPassword As Boolean = Nothing,
                            Optional ByRef vOldPassword As String = "",
                            Optional ByRef vSSOPreferredName As Object = Nothing) As Integer

        Dim nResult As Integer = 0
        Dim oPMUser As bPMUser.PMUser
        Dim nStatus As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oPMUsers.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord = CType(m_lCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                nResult = gPMConstants.PMEReturnCode.PMEOF
            End If

            oPMUser = m_oPMUsers.Item(m_lCurrentRecord)

            ' Get the PMUser Property Values

            m_lError = GetProperties(oPMUser:=oPMUser, vStatus:=nStatus, vUserId:=vUserId,
                                     vLanguageID:=vLanguageID, vUsername:=vUsername, vPassword:=vPassword,
                                     vPasswordChangeDate:=vPasswordChangeDate, vDateCreated:=vDateCreated,
                                     vLastLogin:=vLastLogin, vPartyCnt:=vPartyCnt, vIsDeleted:=vIsDeleted,
                                     vEffectiveDate:=vEffectiveDate, vLoggedOnAtClient:=vLoggedOnAtClient,
                                     vIsPMBLinkRequired:=vIsPMBLinkRequired, vServerPrinter:=vServerPrinter,
                                     vIsPrinterChangeable:=vIsPrinterChangeable, vEmailAddress:=vEmailAddress,
                                     vFullName:=vFullName, vSignatureFile:=vSignatureFile, vTitle:=vTitle,
                                     vTelephoneNumber:=vTelephoneNumber, vExtensionNumber:=vExtensionNumber,
                                     vFaxNumber:=vFaxNumber, vJobTitleID:=vJobTitleID,
                                     vClaimHandlerId:=vClaimHandlerId, vPartyHandlerId:=vPartyHandlerId,
                                     vInitials:=vInitials, vMobileNumber:=vMobileNumber,
                                     vOtherPartyId:=vOtherPartyId, vAlternativeIdentifier:=vAlternativeIdentifier,
                                     vJobBasis:=vJobBasis, vPercentHoursWorked:=vPercentHoursWorked,
                                     vSiriusUser:=vSiriusUser, vDateDeleted:=vDateDeleted,
                                     o_bIsUserTempPassword:=o_bIsUserTempPassword, vOldPassword:=vOldPassword,
                                     vSSOPreferredName:=vSSOPreferredName)


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMUser = Nothing

            Return nResult

        Catch excep As System.Exception


            ' Error.

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetUserSources
    '
    ' Description:
    '
    ' History: 19/04/2000 DAK - Created.
    '
    ' DAK190500 - Get user details using user id parameter
    '
    ' ***************************************************************** '
    Public Function GetUserSources(ByRef r_vSourceArray(,) As Object) As Integer
        Return GetUserSources(r_vSourceArray:=r_vSourceArray, v_vUserID:=Nothing, v_bIncludeDeletedSources:=False, lProductID:=0)
    End Function

    Public Function GetUserSources(ByRef r_vSourceArray(,) As Object, ByVal v_vUserID As Object) As Integer
        Return GetUserSources(r_vSourceArray:=r_vSourceArray, v_vUserID:=v_vUserID, v_bIncludeDeletedSources:=False, lProductID:=0)
    End Function

    Public Function GetUserSources(ByRef r_vSourceArray(,) As Object, ByVal v_vUserID As Object, ByVal v_bIncludeDeletedSources As Boolean) As Integer
        Return GetUserSources(r_vSourceArray:=r_vSourceArray, v_vUserID:=v_vUserID, v_bIncludeDeletedSources:=v_bIncludeDeletedSources, lProductID:=0)
    End Function

    Public Function GetUserSources(ByRef r_vSourceArray(,) As Object, ByVal v_vUserID As Object, ByVal lProductID As Integer) As Integer
        Return GetUserSources(r_vSourceArray:=r_vSourceArray, v_vUserID:=v_vUserID, v_bIncludeDeletedSources:=False, lProductID:=lProductID)
    End Function

    Public Function GetUserSources(ByRef r_vSourceArray(,) As Object, ByVal v_vUserID As Object, ByVal v_bIncludeDeletedSources As Boolean, ByVal lProductID As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMUser As Bpmuser.PMUser
        'DAK300500
        Dim oPMUserSource As Bpmuser.PMUserSource
        Dim vSourceArray(,) As Object
        'DAK300500
        Dim lArraySub As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            vSourceArray = Nothing

            ' Get user
            'DAK190500

            If Not Informations.IsNothing(v_vUserID) Then
                m_lError = GetDetails(v_vUserID, v_bIncludeDeletedSources, lProductID)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = GetNext()
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            oPMUser = m_oPMUsers.Item(CurrentRecord)

            If oPMUser Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' add sources to array
            'DAK300500
            lArraySub = 0
            If oPMUser.PMUserSources.Count() > 0 Then
                For lSub As Integer = 1 To oPMUser.PMUserSources.Count()

                    oPMUserSource = oPMUser.PMUserSources.Item(lSub)

                    'DAK300500 - check database status before adding sources to array
                    If (oPMUserSource IsNot Nothing) Then
                        Select Case oPMUserSource.DatabaseStatus
                            Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete

                                'DAK300500
                                lArraySub += 1

                                If lArraySub = 1 Then
                                    ReDim vSourceArray(3, lArraySub - 1)
                                Else
                                    ReDim Preserve vSourceArray(3, lArraySub - 1)
                                End If
                                'Developer Guide No. 162

                                vSourceArray(0, lArraySub - 1) = oPMUser.PMUserSources.Item(lSub).SourceID

                                vSourceArray(1, lArraySub - 1) = oPMUser.PMUserSources.Item(lSub).SourceCode

                                vSourceArray(2, lArraySub - 1) = oPMUser.PMUserSources.Item(lSub).Description

                                vSourceArray(3, lArraySub - 1) = oPMUser.PMUserSources.Item(lSub).CountryID

                            Case Else
                                '
                        End Select
                    End If
                Next lSub

                r_vSourceArray = vSourceArray

            End If

            ' If no PMUser_Sources tables then all Source tables are available to the user
            'DAK300500
            If Not Informations.IsArray(r_vSourceArray) Then

                m_lError = GetAllSources(r_vSourceArray:=r_vSourceArray)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserSources Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserSources", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetUserIDForParty
    '
    ' Description: Returns the UserID the supplied PartyCnt
    '
    ' History: 19/07/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function GetUserIDForParty(ByVal v_lPartyCnt As Integer, ByRef r_lUserId As Integer) As Integer
        Return GetUserIDForParty(v_lPartyCnt:=v_lPartyCnt, r_lUserId:=r_lUserId, r_sUsername:="")
    End Function
    Public Function GetUserIDForParty(ByVal v_lPartyCnt As Integer, ByRef r_lUserId As Integer, ByRef r_sUsername As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lUserId = -1

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the UserId parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=v_lPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACSelectUserByPartySQL, sSQLName:=ACSelectUserByPartyName, bStoredProcedure:=ACSelectUserByPartyStored, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records did we find?
            Select Case m_oDatabase.Records.Count()
                Case Is < 1
                    ' None, so return not found
                    result = gPMConstants.PMEReturnCode.PMNotFound
                Case Is > 1
                    ' More than one, so return an error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Just one, so return the UserID & Username
                    r_lUserId = m_oDatabase.Records.Item(1).Fields()("user_id")
                    r_sUsername = m_oDatabase.Records.Item(1).Fields()("username")
            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserIDForParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserIDForParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetUserStatus
    '
    ' Description: Returns the available User Status' from DB.
    '
    ' History: 11/11/03 MKW Created
    '
    ' ***************************************************************** '
    Public Function GetUserStatus(ByRef r_vUserStatus(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetUserStatusSQL, sSQLName:=ACGetUserStatusName, bStoredProcedure:=ACGetUserStatusStored, vResultArray:=r_vUserStatus, lNumberRecords:=lRecordCount)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserStatusFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied PMUser into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    'AK 08072003 - add new parameter for alternative identifier ps#246
    ' AMB 24-Nov-03: 1.8.6 Unified login code from 1.9
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vUserId As Object = Nothing,
                            Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vUsername As Object = Nothing,
                            Optional ByRef vPassword As Object = Nothing,
                            Optional ByRef vPasswordChangeDate As Object = Nothing,
                            Optional ByRef vDateCreated As Object = Nothing,
                            Optional ByRef vLastLogin As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing,
                            Optional ByRef vIsDeleted As Object = Nothing,
                            Optional ByRef vEffectiveDate As Object = Nothing,
                            Optional ByRef vIsPMBLinkRequired As Object = Nothing,
                            Optional ByRef vServerPrinter As Object = Nothing,
                            Optional ByRef vIsPrinterChangeable As Object = Nothing,
                            Optional ByRef vEmailAddress As Object = Nothing,
                            Optional ByRef vFullName As Object = Nothing,
                            Optional ByRef vSignatureFile As Object = Nothing, Optional ByRef vTitle As Object = Nothing,
                            Optional ByRef vTelephoneNumber As Object = Nothing,
                            Optional ByRef vExtensionNumber As Object = Nothing,
                            Optional ByRef vFaxNumber As Object = Nothing,
                            Optional ByRef vJobTitleID As Object = Nothing,
                            Optional ByRef vClaimHandlerId As Object = Nothing,
                            Optional ByRef vPartyHandlerId As Object = Nothing,
                            Optional ByRef vInitials As Object = Nothing,
                            Optional ByRef vMobileNumber As Object = Nothing,
                            Optional ByRef vOtherPartyId As Object = Nothing,
                            Optional ByRef vAlternativeIdentifier As Object = Nothing,
                            Optional ByRef vJobBasis As Object = Nothing,
                            Optional ByRef vPercentHoursWorked As Object = Nothing,
                            Optional ByRef vSiriusUser As Object = Nothing,
                            Optional ByRef vDateDeleted As Object = Nothing,
                            Optional ByVal sOldPassword As String = "",
                            Optional ByRef vSSOPreferredName As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMUser As bPMUser.PMUser

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oPMUsers.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new PMUser
            oPMUser = New bPMUser.PMUser()

            ' Populate PMUser Attributes
            ' AMB 24-Nov-03: 1.8.6 Unified login code from 1.9

            'Developer Guide No 98
            m_lError = SetProperties(oPMUser:=oPMUser, iStatus:=gPMConstants.PMEComponentAction.PMAdd,
                                     vUserId:=vUserId, vLanguageID:=vLanguageID, vUsername:=vUsername,
                                     vPassword:=vPassword, vPasswordChangeDate:=vPasswordChangeDate,
                                     vDateCreated:=vDateCreated, vLastLogin:=vLastLogin, vPartyCnt:=vPartyCnt,
                                     vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate,
                                     vIsPMBLinkRequired:=vIsPMBLinkRequired, vServerPrinter:=vServerPrinter,
                                     vIsPrinterChangeable:=vIsPrinterChangeable, vEmailAddress:=vEmailAddress,
                                     vFullName:=vFullName, vSignatureFile:=vSignatureFile, vTitle:=vTitle,
                                     vTelephoneNumber:=vTelephoneNumber, vExtensionNumber:=vExtensionNumber,
                                     vFaxNumber:=vFaxNumber, vJobTitleID:=vJobTitleID,
                                     vClaimHandlerId:=vClaimHandlerId, vPartyHandlerId:=vPartyHandlerId,
                                     vInitials:=vInitials, vMobileNumber:=vMobileNumber,
                                     vOtherPartyId:=vOtherPartyId, vAlternativeIdentifier:=vAlternativeIdentifier,
                                     vJobBasis:=vJobBasis, vPercentHoursWorked:=vPercentHoursWorked,
                                     vSiriusUser:=vSiriusUser, vDateDeleted:=vDateDeleted, sOldPassword:=sOldPassword,
                                     vSSOPreferredName:=vSSOPreferredName)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PMUser to collection
            If (m_oPMUsers.Count = 0) Then
                m_oPMUsers.Add(Nothing)
            End If
            m_lError = m_oPMUsers.Add(oNewPMUser:=oPMUser)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMUser = Nothing

            CurrentRecord = m_oPMUsers.Count()

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the PMUser
    '              specified and updates the PMUser with the new values.
    '
    'AK 08072003 - add new parameter for alternative identifier
    ' AMB 24-Nov-03: 1.8.6 Unified login code from 1.9
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vUserId As Object = Nothing,
                               Optional ByRef vLanguageID As Object = Nothing,
                               Optional ByRef vUsername As Object = Nothing,
                               Optional ByRef vPassword As Object = Nothing,
                               Optional ByRef vPasswordChangeDate As Object = Nothing,
                               Optional ByRef vDateCreated As Object = Nothing,
                               Optional ByRef vLastLogin As Object = Nothing,
                               Optional ByRef vPartyCnt As Object = Nothing,
                               Optional ByRef vIsDeleted As Object = Nothing,
                               Optional ByRef vEffectiveDate As Object = Nothing,
                               Optional ByRef vIsPMBLinkRequired As Object = Nothing,
                               Optional ByRef vServerPrinter As Object = Nothing,
                               Optional ByRef vIsPrinterChangeable As Object = Nothing,
                               Optional ByRef vEmailAddress As Object = Nothing,
                               Optional ByRef vFullName As Object = Nothing,
                               Optional ByRef vSignatureFile As Object = Nothing,
                               Optional ByRef vTitle As Object = Nothing,
                               Optional ByRef vTelephoneNumber As Object = Nothing,
                               Optional ByRef vExtensionNumber As Object = Nothing,
                               Optional ByRef vFaxNumber As Object = Nothing,
                               Optional ByRef vJobTitleID As Object = Nothing,
                               Optional ByRef vClaimHandlerId As Object = Nothing,
                               Optional ByRef vPartyHandlerId As Object = Nothing,
                               Optional ByRef vInitials As Object = Nothing,
                               Optional ByRef vMobileNumber As Object = Nothing,
                               Optional ByRef vOtherPartyId As Object = Nothing,
                               Optional ByRef vAlternativeIdentifier As Object = Nothing,
                               Optional ByRef vJobBasis As Object = Nothing,
                               Optional ByRef vPercentHoursWorked As Object = Nothing,
                               Optional ByRef vSiriusUser As Object = Nothing,
                               Optional ByRef vDateDeleted As Object = Nothing,
                               Optional vIsTempPassword As Object = Nothing,
                               Optional ByVal bSystemUpgradeTempPwd As Boolean = False,
                               Optional ByVal sOldPassword As String = Nothing,
                               Optional ByVal sPasswordChanged As String = "",
                               Optional ByVal sUniqueId As String = "",
                               Optional ByVal sScreenHierarchy As String = "",
                               Optional ByRef vSSOPreferredName As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMUser As bPMUser.PMUser
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMUsers.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oPMUser = m_oPMUsers.Item(lRow)

            ' Check the Status of the PMUser

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oPMUser.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update PMUser Attributes
            ' AMB 24-Nov-03: 1.8.6 Unified login code from 1.9

            'Developer Guide No 98
            m_lError = SetProperties(oPMUser:=oPMUser, iStatus:=iStatus, vUserId:=vUserId,
                                     vLanguageID:=vLanguageID, vUsername:=vUsername, vPassword:=vPassword,
                                     vPasswordChangeDate:=vPasswordChangeDate, vDateCreated:=vDateCreated,
                                     vLastLogin:=vLastLogin, vPartyCnt:=vPartyCnt, vIsDeleted:=vIsDeleted,
                                     vEffectiveDate:=vEffectiveDate, vIsPMBLinkRequired:=vIsPMBLinkRequired,
                                     vServerPrinter:=vServerPrinter, vIsPrinterChangeable:=vIsPrinterChangeable,
                                     vEmailAddress:=vEmailAddress, vFullName:=vFullName,
                                     vSignatureFile:=vSignatureFile, vTitle:=vTitle,
                                     vTelephoneNumber:=vTelephoneNumber, vExtensionNumber:=vExtensionNumber,
                                     vFaxNumber:=vFaxNumber, vJobTitleID:=vJobTitleID,
                                     vClaimHandlerId:=vClaimHandlerId, vPartyHandlerId:=vPartyHandlerId,
                                     vInitials:=vInitials, vMobileNumber:=vMobileNumber,
                                     vOtherPartyId:=vOtherPartyId, vAlternativeIdentifier:=vAlternativeIdentifier,
                                     vJobBasis:=vJobBasis, vPercentHoursWorked:=vPercentHoursWorked,
                                     vSiriusUser:=vSiriusUser, vDateDeleted:=vDateDeleted,
                                     vIsTempPassword:=vIsTempPassword, bSystemUpgradeTempPwd:=bSystemUpgradeTempPwd,
                                     sOldPassword:=sOldPassword, sPasswordChanged:=sPasswordChanged, sUniqueId:=sUniqueId,
                                     sScreenHierarhcy:=sScreenHierarchy, vSSOPreferredName:=vSSOPreferredName)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release reference to PMUser
            oPMUser = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EditSources
    '
    ' Description:
    '
    ' History: 19/04/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function EditSources(ByRef r_vSourceArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim oPMUser As Bpmuser.PMUser
        Dim oPMUserSource As Bpmuser.PMUserSource
        Dim lSub2 As Integer
        Dim sKey As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get current user
            oPMUser = m_oPMUsers.Item(m_lCurrentRecord)

            If oPMUser Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for default: all sources = no PMUser_Source tables
            ' PWF: 10/10/2002 - The list is of all valid branches so check all valid
            '                 - branches, not an empty array
            '    If IsArray(r_vSourceArray) = False Then
            '        If oPMUser.PMUserSources.Count > 0 Then
            '            For lSub = 1 To oPMUser.PMUserSources.Count
            '                Set oPMUserSource = oPMUser.PMUserSources.Item(lSub)
            '                Select Case oPMUserSource.DatabaseStatus
            '                    Case PMAdd
            '                        oPMUserSource.DatabaseStatus = PMDummyDelete
            '                    Case PMView
            '                        oPMUserSource.DatabaseStatus = PMDelete
            '                    Case Else
            '                        ' Do nothing
            '                End Select
            '                Set oPMUserSource = Nothing
            '            Next lSub
            '        End If
            '        Exit Function
            '    End If

            ' The users access is limited to a subset of the Companies/Sources
            ' If the source is in the array it should be deleted from the database
            For lSub As Integer = r_vSourceArray.GetLowerBound(1) To r_vSourceArray.GetUpperBound(1)

                'Developer Guide No. 162
                sKey = "K" & CStr(r_vSourceArray(0, lSub))
                oPMUserSource = oPMUser.PMUserSources.Item(CInt(sKey))
                'DAK300500
                If Not (oPMUserSource Is Nothing) Then
                    If oPMUserSource.IsSaved Then
                        oPMUserSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                    Else
                        Select Case oPMUserSource.DatabaseStatus
                            Case gPMConstants.PMEComponentAction.PMAdd
                                oPMUserSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
                            Case gPMConstants.PMEComponentAction.PMView
                                oPMUserSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
                            Case Else
                                ' Do nothing
                        End Select
                    End If
                Else
                    oPMUserSource = New Bpmuser.PMUserSource()
                    'Developer Guide No. 162

                    oPMUserSource.SourceID = CInt(r_vSourceArray(0, lSub))

                    oPMUserSource.SourceCode = CStr(r_vSourceArray(1, lSub))
                    'DAK300500

                    oPMUserSource.Description = CStr(r_vSourceArray(2, lSub))
                    oPMUserSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
                    If (oPMUser.PMUserSources.Count = 0) Then
                        oPMUser.PMUserSources.Add(Nothing)
                    End If
                    m_lError = oPMUser.PMUserSources.Add(oPMUserSource)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                oPMUserSource = Nothing
            Next lSub

            ' Delete sources
            For lSub As Integer = 1 To oPMUser.PMUserSources.Count()
                oPMUserSource = oPMUser.PMUserSources.Item(lSub)
                For lSub2 = r_vSourceArray.GetLowerBound(1) To r_vSourceArray.GetUpperBound(1)

                    'Developer Guide No. 162
                    If r_vSourceArray(0, lSub2) = oPMUserSource.SourceID Then
                        Exit For
                    End If
                Next lSub2
                ' If source is not found in the array delete it
                If lSub2 > r_vSourceArray.GetUpperBound(1) Then
                    If oPMUserSource.IsSaved Then
                        oPMUserSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd
                    Else
                        ' If we are adding the user we must add this source....
                        ' Note: When adding a NEW user's sources this function must
                        ' first be called with the FULL source list to populate it.
                        ' It should then be called with the updated user list.
                        oPMUserSource.DatabaseStatus = If(oPMUser.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit)
                    End If
                End If
                oPMUserSource = Nothing
            Next lSub

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditSources Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditSources", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified PMUser can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMUser As Bpmuser.PMUser

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMUsers.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMUser = m_oPMUsers.Item(lRow)

            ' Check the Status of the PMUser

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oPMUser.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oPMUser.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                'set deleted status to true
                oPMUser.IsDeleted = gPMConstants.PMEVarTrueFalse.PMVarTrue

                'set database status to update
                oPMUser.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
            End If

            ' Release reference to PMUser
            oPMUser = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oPMUsers.Count()
                Select Case m_oPMUsers.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim oPMUser As Bpmuser.PMUser
        Dim bTransStarted As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub As Integer = 1 To m_oPMUsers.Count()
                oPMUser = m_oPMUsers.Item(lSub)
                m_oPMUserSync = oPMUser
                Select Case oPMUser.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lError = BeginTrans()
                            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lError = AddItem(oPMUser)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lError = BeginTrans()
                            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lError = UpdateItem(oPMUser)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        'do nothing because in this case the isdeleted flag is set to true
                        'and the row is simply updated

                End Select

                'DAK040500 - check if Company/Source list has been changed
                m_lError = UpdateSources(oPMUser, bTransStarted)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Exit For
                End If

            Next lSub
            oPMUser = Nothing
            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = CommitTrans()
                    ' Release last reference

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If m_oPMUserSync IsNot Nothing Then
                        SyncUser(m_oPMUserSync.DatabaseStatus)
                    End If
                    ' Set all items in the collection to PMView
                    For lSub As Integer = 1 To m_oPMUsers.Count()
                        m_oPMUsers.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                    Next lSub
                Else

                    m_lError = RollbackTrans()
                    ' Release last reference
                    oPMUser = Nothing
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If
            ' Release last reference
            oPMUser = Nothing
            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAllSources
    '
    ' Description:
    '
    ' History: 19/04/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetAllSources(ByRef r_vSourceArray(,) As Object) As Integer
        Return GetAllSources(r_vSourceArray:=r_vSourceArray, v_bIncludeDeletedSources:=False)
    End Function

    Public Function GetAllSources(ByRef r_vSourceArray(,) As Object, ByVal v_bIncludeDeletedSources As Boolean) As Integer
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim result As Integer = 0
        Dim vSourceArray(,) As Object = Nothing
        Dim oSource As bPMSource.Business
        Dim iSourceID As Integer
        Dim sCode As String = ""
        Dim sDescription As String = ""
        Dim iIsDeleted As gPMConstants.PMEReturnCode
        Dim dtEffectiveDate As Date
        Dim lSub As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lError = oCS.CreateBusinessObject( _
            'r_oObject:=oSource, _
            'v_sClassName:="bPMSource.Business", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUsername:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oSource = New bPMSource.Business
            m_lError = oSource.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lError
            End If


            m_lError = oSource.GetDetails()
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                oSource = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lError = oSource.GetNext(vSourceID:=iSourceID, vCode:=sCode, vDescription:=sDescription, vIsDeleted:=iIsDeleted, vEffectiveDate:=dtEffectiveDate)
            Do While m_lError = gPMConstants.PMEReturnCode.PMTrue

                If False Then
                    If dtEffectiveDate <= DateTime.Now And iIsDeleted = gPMConstants.PMEReturnCode.PMFalse Then
                        lSub += 1
                        If lSub = 1 Then
                            ReDim vSourceArray(2, lSub - 1)
                        Else
                            ReDim Preserve vSourceArray(2, lSub - 1)
                        End If
                        'Developer Guide No. 162

                        vSourceArray(0, lSub - 1) = iSourceID

                        vSourceArray(1, lSub - 1) = sCode

                        vSourceArray(2, lSub - 1) = sDescription
                    End If
                Else
                    If v_bIncludeDeletedSources Then
                        If dtEffectiveDate <= DateTime.Now Then
                            lSub += 1
                            If lSub = 1 Then
                                ReDim vSourceArray(2, lSub - 1)
                            Else
                                ReDim Preserve vSourceArray(2, lSub - 1)
                            End If
                            'Developer Guide No. 162

                            vSourceArray(0, lSub - 1) = iSourceID

                            vSourceArray(1, lSub - 1) = sCode

                            vSourceArray(2, lSub - 1) = sDescription
                        End If
                    Else
                        If dtEffectiveDate <= DateTime.Now And iIsDeleted = gPMConstants.PMEReturnCode.PMFalse Then
                            lSub += 1
                            If lSub = 1 Then
                                ReDim vSourceArray(2, lSub - 1)
                            Else
                                ReDim Preserve vSourceArray(2, lSub - 1)
                            End If
                            'Developer Guide No. 162


                            vSourceArray(0, lSub - 1) = iSourceID

                            vSourceArray(1, lSub - 1) = sCode

                            vSourceArray(2, lSub - 1) = sDescription
                        End If
                    End If
                End If

                'DAK190600

                m_lError = oSource.GetNext(vSourceID:=iSourceID, vCode:=sCode, vDescription:=sDescription, vIsDeleted:=iIsDeleted, vEffectiveDate:=dtEffectiveDate)

            Loop

            oSource = Nothing

            If m_lError <> gPMConstants.PMEReturnCode.PMEOF Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_vSourceArray = vSourceArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllSources Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllSources", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPrivilegeLevel
    '
    ' Description:
    '
    ' History: 10/11/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetPrivilegeLevel(ByRef r_iPrivilegeLevel As Integer) As Integer
        Dim result As Integer = 0
        Dim lPMProductID As Integer
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim oPMProductLookup As bPMProductLookup.Business


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lError = GetProductID(lPMProductID)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lError = oCS.CreateBusinessObject( _
            'r_oObject:=oPMProductLookup, _
            'v_sClassName:="bPMProductLookup.Business", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUsername:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oPMProductLookup = New bPMProductLookup.Business
            m_lError = oPMProductLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK221299

            m_lError = oPMProductLookup.GetDetails(vPMProductId:=lPMProductID, vTableName:="PMUser")
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                oPMProductLookup.Dispose()
                oPMProductLookup = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lError = oPMProductLookup.GetNext(vPrivilegeLevel:=r_iPrivilegeLevel)

            oPMProductLookup.Dispose()
            oPMProductLookup = Nothing
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrivilegeLevel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivilegeLevel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetProductID
    '
    ' Description:
    '
    ' History: 11/11/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetProductID(ByRef r_lPMProductID As Integer) As Integer
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim result As Integer = 0
        Dim oPMProduct As bPMProduct.Business


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lError = oCS.CreateBusinessObject( _
            'r_oObject:=oPMProduct, _
            'v_sClassName:="bPMProduct.Business", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUsername:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oPMProduct = New bPMProduct.Business
            m_lError = oPMProduct.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lError = oPMProduct.GetProductID(v_sProductCode:="Sirius", r_lPMProductID:=r_lPMProductID)

            oPMProduct.Dispose()
            oPMProduct = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Gets the User Groups that the current user is a
    '              Supervisor of.
    '
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_bIsAdministrator As Boolean, ByRef r_vSupervisedGroups As Object) As Integer

        Dim result As Integer = 0
        Dim oUserGroup As bPMUserGroup.Utilities
        ' RDC 13062002 CompServ Replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lError = oCS.CreateBusinessObject( _
            'r_oObject:=oUserGroup, _
            'v_sClassName:="bPMUserGroup.Utilities", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUsername:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oUserGroup = New bPMUserGroup.Utilities
            m_lError = oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Is the User an Administrator

            m_lError = oUserGroup.IsUserAdministrator(v_iUserID:=m_iUserID, r_bIsAdministrator:=r_bIsAdministrator)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Get the Groups they Supervise

            m_lError = oUserGroup.GetGroupsSupervisedByUser(v_iUserID:=m_iUserID, r_vSupervisedGroups:=r_vSupervisedGroups)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Terminate

            oUserGroup.Dispose()
            oUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC090903
    ' ***************************************************************** '
    ' Name: GetAllUserGroups
    '
    ' Description: Gets all the User Groups
    '
    ' ***************************************************************** '
    Public Function GetUserGroupInfo(ByRef r_lUserId As Integer, ByRef r_vUserGroupInfo As Object) As Integer

        Dim result As Integer = 0
        Dim oUserGroup As bPMUserGroup.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oUserGroup = New bPMUserGroup.Business
            m_lError = oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Groups

            m_lError = oUserGroup.GetUserGroupInfo(r_lUserId:=r_lUserId, r_vUserGroupInfo:=r_vUserGroupInfo)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Terminate

            oUserGroup.Dispose()
            oUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserGroupInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroupInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC090903
    ' ***************************************************************** '
    ' Name: UpdateAllUserGroups
    '
    ' Description: Update User Group Info
    '
    ' ***************************************************************** '
    Public Function UpdateUserGroupInfo(ByRef r_lUserId As Integer, ByRef r_lPMUserGroupId As Integer, ByRef r_iMode As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer
        Return UpdateUserGroupInfo(r_lUserId:=r_lUserId, r_lPMUserGroupId:=r_lPMUserGroupId, r_iMode:=r_iMode, r_iIsSupervisor:=0, v_sUniqueId:=v_sUniqueId, v_sScreenHierarchy:=v_sScreenHierarchy)
    End Function

    Public Function UpdateUserGroupInfo(ByRef r_lUserId As Integer, ByRef r_lPMUserGroupId As Integer, ByRef r_iMode As Integer, ByRef r_iIsSupervisor As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oUserGroup As bPMUserGroup.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oUserGroup = New bPMUserGroup.Business
            m_lError = oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Groups

            m_lError = oUserGroup.UpdateUserGroupInfo(r_lUserId:=r_lUserId, r_lPMUserGroupId:=r_lPMUserGroupId, r_iIsSupervisor:=r_iIsSupervisor, r_iMode:=r_iMode, v_sUniqueId:=v_sUniqueId, v_sScreenHierarchy:=v_sScreenHierarchy)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Terminate

            oUserGroup.Dispose()
            oUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserGroupInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserGroupInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC090903
    ' ***************************************************************** '
    ' Name: UpdateUserSourceInfo
    '
    ' Description: Update User Source Info
    '
    ' ***************************************************************** '
    Public Function UpdateUserSourceInfo(ByRef r_lUserId As Integer, ByRef r_lSourceId As Integer, ByRef r_iMode As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the user_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(r_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the source_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(r_lSourceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=CStr(v_sUniqueId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=CStr(v_sScreenHierarchy), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If r_iMode = 0 Then

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACDeleteUserSourceInfoSQL, sSQLName:=ACDeleteUserSourceInfoName, bStoredProcedure:=ACDeleteUserSourceInfoStored)

            Else

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACAddUserSourceInfoSQL, sSQLName:=ACAddUserSourceInfoName, bStoredProcedure:=ACAddUserSourceInfoStored)

            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserSourceInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserSourceInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC090903
    ' ***************************************************************** '
    ' Name: GetUserSourceInfo
    '
    ' Description: Gets All Info On Sources And What User Can Use
    '
    ' ***************************************************************** '
    Public Function GetUserSourceInfo(ByRef r_lUserId As Integer, ByRef r_vUserSourceInfo(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the UserId parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(r_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetUserSourceInfoSQL, sSQLName:=ACGetUserSourceInfoName, bStoredProcedure:=ACGetUserSourceInfoStored, vResultArray:=r_vUserSourceInfo, lNumberRecords:=lRecordCount)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserSourceInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserSourceInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC090903
    ' ***************************************************************** '
    ' Name: GetUserPartyInfo
    '
    ' Description: Gets All Info On Parties For User
    '
    ' ***************************************************************** '
    Public Function GetUserPartyInfo(ByRef r_lUserId As Integer, ByRef r_vUserPartyInfo(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the UserId parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(r_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetUserPartyInfoSQL, sSQLName:=ACGetUserPartyInfoName, bStoredProcedure:=ACGetUserPartyInfoStored, vResultArray:=r_vUserPartyInfo, lNumberRecords:=lRecordCount)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserPartyInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserPartyInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC090903
    ' ***************************************************************** '
    ' Name: GetClaimHandlers
    '
    ' Description: Gets Claim Handlers
    '
    ' ***************************************************************** '
    Public Function GetClaimHandlers(ByRef r_vParty(,) As Object) As Integer

        Dim nResult As Integer = 0
        Dim nRecordCount As Integer

        Try
            nResult = PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetClaimHandlersSQL, sSQLName:=ACGetClaimHandlersName, bStoredProcedure:=ACGetClaimHandlersStored, vResultArray:=r_vParty, lNumberRecords:=nRecordCount)

            If m_lError <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If nRecordCount < 1 Then
                ' No Records, return PMFalse
                Return PMEReturnCode.PMFalse
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimHandlersFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimHandlers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    'DC090903
    ' ***************************************************************** '
    ' Name: GetParty
    '
    ' Description: Gets Party
    '
    ' ***************************************************************** '
    Public Function GetParty(ByRef r_sPartyType As String, ByRef r_vParty(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the party type parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="party_type", vValue:=r_sPartyType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetPartySQL, sSQLName:=ACGetPartyName, bStoredProcedure:=ACGetPartyStored, vResultArray:=r_vParty, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?

            If Not Informations.IsArray(r_vParty) Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC090903
    ' ***************************************************************** '
    ' Name: GetUserRiskGroupInfo
    '
    ' Description: Gets All Info On Risk Groups And What User Status
    '
    ' ***************************************************************** '
    Public Function GetUserRiskGroupInfo(ByRef r_lUserId As Integer, ByRef r_vUserRiskGroupInfo(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the UserId parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(r_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'date time helper not required
            'm_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now.ToShortDateString(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetUserRiskGroupInfoSQL, sSQLName:=ACGetUserRiskGroupInfoName, bStoredProcedure:=ACGetUserRiskGroupInfoStored, vResultArray:=r_vUserRiskGroupInfo, lNumberRecords:=lRecordCount)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserRiskGroupInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserRiskGroupInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC090903
    ' ***************************************************************** '
    ' Name: UpdateUserRiskGroupInfo
    '
    ' Description: Update All User Risk Groups
    '
    ' ***************************************************************** '
    Public Function UpdateUserRiskGroupInfo(ByVal v_lUserId As Integer, ByVal v_lRiskGroupId As Integer, ByVal v_lFSAUserStatusId As Integer, ByVal v_bPassedExam As Boolean, ByVal v_vDatePassedExam As Object, ByRef r_lFSAUserCompetencyId As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            If r_lFSAUserCompetencyId = 0 Then

                m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(v_lRiskGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = m_oDatabase.Parameters.Add(sName:="fsa_user_status_id", vValue:=CStr(v_lFSAUserStatusId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = m_oDatabase.Parameters.Add(sName:="passed_exam", vValue:=CStr(If(v_bPassedExam, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lError = m_oDatabase.Parameters.Add(sName:="date_passed_exam", vValue:=CStr(v_vDatePassedExam), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'Developer Guide No. 85
                m_lError = m_oDatabase.Parameters.Add(sName:="fsa_user_competency_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACAddUserRiskGroupInfoSQL, sSQLName:=ACAddUserRiskGroupInfoName, bStoredProcedure:=ACAddUserRiskGroupInfoStored)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_lFSAUserCompetencyId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("fsa_user_competency_id").Value)

            Else

                m_lError = m_oDatabase.Parameters.Add(sName:="fsa_user_competency_id", vValue:=CStr(r_lFSAUserCompetencyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = m_oDatabase.Parameters.Add(sName:="fsa_user_status_id", vValue:=CStr(v_lFSAUserStatusId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = m_oDatabase.Parameters.Add(sName:="passed_exam", vValue:=CStr(If(v_bPassedExam, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lError = m_oDatabase.Parameters.Add(sName:="date_passed_exam", vValue:=CStr(v_vDatePassedExam), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACUpdateUserRiskGroupInfoSQL, sSQLName:=ACUpdateUserRiskGroupInfoName, bStoredProcedure:=ACUpdateUserRiskGroupInfoStored)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserRiskGroupInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserRiskGroupInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Private)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oPMUser As Bpmuser.PMUser) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lError = AddInputParam(oPMUser:=oPMUser)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    ' Add PMUserID as an OUTPUT param for an insert
        '    m_lError& = m_oDatabase.Parameters.Add( _
        ''          sName:="user_id", _
        ''          vValue:=oPMUser.UserID, _
        ''          iDirection:=PMParamOutput, _
        ''          iDataType:=PMInteger)
        '
        '    If (m_lError& <> PMTrue) Then
        '        AddItem = PMFalse
        '        Exit Function
        '    End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    ' Get the ID of the record inserted
        '     oPMUser.UserID = m_oDatabase.Parameters.Item("user_id").Value
        '
        '    If (m_lError& <> PMTrue) Then
        '        AddItem = PMFalse
        '        Exit Function
        '    End If

        ' Get this record using the username
        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add Username as an Input param
        m_lError = m_oDatabase.Parameters.Add(sName:="username", vValue:=oPMUser.Username, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.SQLSelect(sSQL:=ACSelectUserByNameSQL, sSQLName:=ACSelectUserByNameName, bStoredProcedure:=ACSelectUserByNameStored, vResultArray:=vResultArray)

        If Not Informations.IsArray(vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to select recently stored PMUser", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem")

            Return result

        End If



        oPMUser.UserID = CInt(vResultArray(vResultArray.GetLowerBound(0), vResultArray.GetLowerBound(1)))

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Private)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oPMUser As Bpmuser.PMUser) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        With m_oDatabase

            m_lError = .Parameters.Add(sName:="language_id", vValue:=oPMUser.LanguageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="username", vValue:=oPMUser.Username, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="secure_password", vValue:=oPMUser.Password,
                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                       iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lError = .Parameters.Add(sName:="password", vValue:=oPMUser.OldPassword,
                           iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                           iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oPMUser.PasswordChangeDate = #12:00:00 AM# OrElse oPMUser.IsSystemUpgradeTempPwd Then

                m_lError = .Parameters.Add(sName:="password_change_date", vValue:=ToSafeDate("29/12/1899", #12/29/1899#), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lError = .Parameters.Add(sName:="password_change_date", vValue:=oPMUser.PasswordChangeDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If oPMUser.DateCreated = #12:00:00 AM# Then

                m_lError = .Parameters.Add(sName:="date_created", vValue:="12/29/1899", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lError = .Parameters.Add(sName:="date_created", vValue:=oPMUser.DateCreated, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If oPMUser.Lastlogin = #12:00:00 AM# Then

                m_lError = .Parameters.Add(sName:="lastlogin", vValue:="12/29/1899", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lError = .Parameters.Add(sName:="lastlogin", vValue:=oPMUser.Lastlogin, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            If oPMUser.PartyCnt = 0 Then

                m_lError = .Parameters.Add(sName:="party_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lError = .Parameters.Add(sName:="party_cnt", vValue:=CStr(oPMUser.PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="is_pmb_link_required", vValue:=oPMUser.IsPMBLinkRequired, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Server Printer

            If Convert.IsDBNull(oPMUser.ServerPrinter) Or Informations.IsNothing(oPMUser.ServerPrinter) Then
                m_lError = .Parameters.Add(sName:="server_printer", vValue:="Null", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMTableName)
            ElseIf (oPMUser.ServerPrinter = "") Then
                m_lError = .Parameters.Add(sName:="server_printer", vValue:=CStr(-1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lError = .Parameters.Add(sName:="server_printer", vValue:=oPMUser.ServerPrinter, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="is_printer_changeable", vValue:=oPMUser.IsPrinterChangeable, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="is_deleted", vValue:=oPMUser.IsDeleted, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="effective_date", vValue:=oPMUser.EffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oPMUser.EmailAddress = "" Then
                m_lError = .Parameters.Add(sName:="email_address", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="email_address", vValue:=oPMUser.EmailAddress, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.SSOPreferredName = "" Then
                m_lError = .Parameters.Add(sName:="SSO_Preferred_Username", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="SSO_Preferred_Username", vValue:=oPMUser.SSOPreferredName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If


            If oPMUser.FullName = "" Then
                m_lError = .Parameters.Add(sName:="full_name", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="full_name", vValue:=oPMUser.FullName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            m_lError = .Parameters.Add(sName:="alternative_identifier", vValue:=oPMUser.AlternativeIdentifier, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oPMUser.MobileNumber = "" Then

                m_lError = .Parameters.Add(sName:="mobile_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="mobile_number", vValue:=oPMUser.MobileNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.Initials = "" Then

                m_lError = .Parameters.Add(sName:="initials", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="initials", vValue:=oPMUser.Initials, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.SignatureFile = "" Then


                m_lError = .Parameters.Add(sName:="signature_file", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="signature_file", vValue:=oPMUser.SignatureFile, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.Title = "" Then

                m_lError = .Parameters.Add(sName:="title", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="title", vValue:=oPMUser.Title, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.TelephoneNumber = "" Then

                m_lError = .Parameters.Add(sName:="telephone_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="telephone_number", vValue:=oPMUser.TelephoneNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.ExtensionNumber = "" Then

                m_lError = .Parameters.Add(sName:="extension_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="extension_number", vValue:=oPMUser.ExtensionNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.FaxNumber = "" Then

                m_lError = .Parameters.Add(sName:="fax_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="fax_number", vValue:=oPMUser.FaxNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.JobTitleId = -1 Or oPMUser.JobTitleId = 0 Then

                m_lError = .Parameters.Add(sName:="job_title_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                m_lError = .Parameters.Add(sName:="job_title_id", vValue:=oPMUser.JobTitleId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If oPMUser.ClaimHandlerId = 0 Then

                m_lError = .Parameters.Add(sName:="claim_handler_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lError = .Parameters.Add(sName:="claim_handler_id", vValue:=oPMUser.ClaimHandlerId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If oPMUser.PartyHandlerId = 0 Then

                m_lError = .Parameters.Add(sName:="party_handler_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            Else
                m_lError = .Parameters.Add(sName:="party_handler_id", vValue:=oPMUser.PartyHandlerId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If oPMUser.OtherPartyId = 0 Then

                m_lError = .Parameters.Add(sName:="other_party_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lError = .Parameters.Add(sName:="other_party_id", vValue:=oPMUser.OtherPartyId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            m_lError = .Parameters.Add(sName:="job_basis", vValue:=oPMUser.JobBasis, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lError = .Parameters.Add(sName:="percent_hours_worked", vValue:=oPMUser.PercentHoursWorked, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            m_lError = .Parameters.Add(sName:="sirius_user", vValue:=CInt(oPMUser.IsSiriusUser), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If Informations.IsNothing(oPMUser.DateDeleted) Or Convert.IsDBNull(oPMUser.DateDeleted) Then
                m_lError = .Parameters.Add(sName:="date_deleted", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lError = .Parameters.Add(sName:="date_deleted", vValue:=oPMUser.DateDeleted, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If Informations.IsNothing(oPMUser.IsTempPassword) Or Convert.IsDBNull(oPMUser.IsTempPassword) Then
                m_lError = .Parameters.Add(sName:="is_temp_password", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            Else
                m_lError = .Parameters.Add(sName:="is_temp_password", vValue:=CInt(oPMUser.IsTempPassword), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            End If

            m_lError = .Parameters.Add(sName:="modified_by", vValue:=CInt(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If String.IsNullOrEmpty(oPMUser.UniqueId) Then
                m_lError = .Parameters.Add(sName:="unique_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lError = .Parameters.Add(sName:="screen_hierarchy", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="unique_id", vValue:=oPMUser.UniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lError = .Parameters.Add(sName:="screen_hierarchy", vValue:=oPMUser.ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PMUserID as an INPUT param for an update
            m_lError = .Parameters.Add(sName:="user_id", vValue:=oPMUser.UserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With
        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If the record was NOT UpdateItemd reselect it to see if the data
        ' has been changed or the record deleted

        If lRecordsAffected > 0 Then

            ' UpdatedItem, No action required

        Else

            result = gPMConstants.PMEReturnCode.PMFalse

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Private)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DeleteItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DeleteItem(ByRef oPMUser As bPMUser.PMUser) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRecordsAffected As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Clear the Database Parameters Collection
    'm_oDatabase.Parameters.Clear()
    '
    ' Add the InsuranceFileID INPUT parameter
    'm_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(oPMUser.UserID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Execute SQL Statement
    'm_lError = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' If record wasn't deleted, error
    'If lRecordsAffected > 0 Then
    ' Deleted, No action required
    'Else
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied PMUser properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oPMUser As Bpmuser.PMUser, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No. 112
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        'Developer Guide No. 111(Guide)
        oFields = m_oDatabase.Records.Item(lRecordNumber - 1).Fields()

        ' Populate Base Details

        With oPMUser

            .UserID = oFields("user_id")
            .LanguageID = oFields("language_id")
            .Username = oFields("username")
            .Password = gPMFunctions.ToSafeString(oFields("secure_password"))
            .OldPassword = gPMFunctions.ToSafeString(oFields("password"))
            .PasswordChangeDate = oFields("password_change_date")
            If Informations.IsDBNull(oFields.Item("date_created")) Then
                .DateCreated = System.DateTime.FromOADate(0)
            Else
                .DateCreated = oFields("date_created")
            End If
            If Informations.IsDBNull(oFields.Item("lastlogin")) Then
                .Lastlogin = System.DateTime.FromOADate(0)
            Else
                .DateCreated = oFields("date_created")
            End If


            .Lastlogin = oFields("lastlogin")
            'Developer Guide No.85
            If Informations.IsDBNull(oFields.Item("logged_on_at_client")) Then
                .LoggedOnAtClient = ""
            Else
                .LoggedOnAtClient = oFields("logged_on_at_client")
            End If



            If Convert.IsDBNull(oFields("party_cnt")) Or Informations.IsNothing(oFields("party_cnt")) Then
                .PartyCnt = 0
            Else
                .PartyCnt = oFields("party_cnt")
            End If

            If Convert.IsDBNull(oFields("is_deleted")) Or Informations.IsNothing(oFields("is_deleted")) Then
                .IsDeleted = 0
            Else
                .IsDeleted = oFields("is_deleted")
            End If

            If Convert.IsDBNull(oFields("effective_date")) Or Informations.IsNothing(oFields("effective_date")) Then
                .EffectiveDate = #12/30/1899#
            Else
                .EffectiveDate = oFields("effective_date")
            End If

            If Convert.IsDBNull(oFields("is_pmb_link_required")) Or Informations.IsNothing(oFields("is_pmb_link_required")) Then
                .IsPMBLinkRequired = 0
            Else
                .IsPMBLinkRequired = oFields("is_pmb_link_required")
            End If

            If Convert.IsDBNull(oFields("server_printer")) Or Informations.IsNothing(oFields("server_printer")) Then
                .ServerPrinter = ""
            Else
                .ServerPrinter = oFields("server_printer")
            End If

            If Convert.IsDBNull(oFields("is_printer_changeable")) Or Informations.IsNothing(oFields("is_printer_changeable")) Then
                .IsPrinterChangeable = 0
            Else
                .IsPrinterChangeable = oFields("is_printer_changeable")
            End If

            If Convert.IsDBNull(oFields("email_address")) Or Informations.IsNothing(oFields("email_address")) Then
                .EmailAddress = ""
            Else
                .EmailAddress = oFields("email_address")
            End If
            'DC040903 -start

            If Convert.IsDBNull(oFields("full_name")) Or Informations.IsNothing(oFields("full_name")) Then
                .FullName = ""
            Else
                .FullName = oFields("full_name").Replace("'", "''")
            End If

            If Convert.IsDBNull(oFields("mobile_number")) Or Informations.IsNothing(oFields("mobile_number")) Then
                .MobileNumber = ""
            Else
                .MobileNumber = oFields("mobile_number")
            End If

            If Convert.IsDBNull(oFields("initials")) Or Informations.IsNothing(oFields("initials")) Then
                .Initials = ""
            Else
                .Initials = oFields("initials")
            End If

            If Convert.IsDBNull(oFields("signature_file")) Or Informations.IsNothing(oFields("signature_file")) Then
                .SignatureFile = ""
            Else
                .SignatureFile = oFields("signature_file")
            End If

            If Convert.IsDBNull(oFields("title")) Or Informations.IsNothing(oFields("title")) Then
                .Title = ""
            Else
                .Title = oFields("title")
            End If

            If Convert.IsDBNull(oFields("telephone_number")) Or Informations.IsNothing(oFields("telephone_number")) Then
                .TelephoneNumber = ""
            Else
                .TelephoneNumber = oFields("telephone_number")
            End If

            If Convert.IsDBNull(oFields("extension_number")) Or Informations.IsNothing(oFields("extension_number")) Then
                .ExtensionNumber = ""
            Else
                .ExtensionNumber = oFields("extension_number")
            End If

            If Convert.IsDBNull(oFields("fax_number")) Or Informations.IsNothing(oFields("fax_number")) Then
                .FaxNumber = ""
            Else
                .FaxNumber = oFields("fax_number")
            End If

            If Convert.IsDBNull(oFields("job_title_id")) Or Informations.IsNothing(oFields("job_title_id")) Then
                .JobTitleId = -1
            Else
                .JobTitleId = CInt(oFields("job_title_id"))
            End If

            If Convert.IsDBNull(oFields("claim_handler_id")) Or Informations.IsNothing(oFields("claim_handler_id")) Then
                .ClaimHandlerId = 0
            Else
                .ClaimHandlerId = CInt(oFields("claim_handler_id"))
            End If

            If Convert.IsDBNull(oFields("party_handler_id")) Or Informations.IsNothing(oFields("party_handler_id")) Then
                .PartyHandlerId = 0
            Else
                .PartyHandlerId = CInt(oFields("party_handler_id"))
            End If

            '(RC) WR34

            If Convert.IsDBNull(oFields("other_party_id")) Or Informations.IsNothing(oFields("other_party_id")) Then
                .OtherPartyId = 0
            Else
                .OtherPartyId = CInt(oFields("other_party_id"))
            End If

            'DC040903 -end

            'AK 08072003 alternative identifier - ps#246

            If Convert.IsDBNull(oFields("alternative_identifier")) Or Informations.IsNothing(oFields("alternative_identifier")) Then
                .AlternativeIdentifier = ""
            Else
                .AlternativeIdentifier = oFields("alternative_identifier")
            End If

            .JobBasis = gPMFunctions.ToSafeLong(oFields("job_basis"))
            .PercentHoursWorked = gPMFunctions.ToSafeDouble(oFields("percent_hours_worked"))
            .IsSiriusUser = gPMFunctions.ToSafeBoolean(oFields("sirius_user"))


            If Convert.IsDBNull(oFields("date_deleted")) Or Informations.IsNothing(oFields("date_deleted")) Then

                .DateDeleted = Nothing
            Else
                .DateDeleted = gPMFunctions.ToSafeDate(oFields("date_deleted"))
            End If

            'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.2)
            .UserConfigXMLDataSet = gPMFunctions.ToSafeString(oFields("user_config_xml_dataset"), "")
            'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.2)

            .IsLocked = gPMFunctions.ToSafeBoolean(oFields("is_locked"))
            .IncorrectAttemptCount = gPMFunctions.ToSafeInteger(oFields("incorrect_attempt_count"))

            .IsTempPassword = gPMFunctions.ToSafeBoolean(oFields("Is_Temp_Password"))
            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied PMUser property values.
    '
    'AK 08072003 Add alternative identifier (ps#246)
    ' AMB 24-Nov-03: 1.8.6 Unified login code from 1.9
    ' ***************************************************************** '
    'Developer Guide No 101
    Private Function SetProperties(ByRef oPMUser As bPMUser.PMUser, ByRef iStatus As Integer,
                                   Optional ByRef vUserId As Object = Nothing,
                                   Optional ByRef vLanguageID As Object = Nothing,
                                   Optional ByRef vUsername As Object = Nothing,
                                   Optional ByRef vPassword As Object = Nothing,
                                   Optional ByRef vPasswordChangeDate As Object = Nothing,
                                   Optional ByRef vDateCreated As Object = Nothing,
                                   Optional ByRef vLastLogin As Object = Nothing,
                                   Optional ByRef vPartyCnt As Object = Nothing,
                                   Optional ByRef vIsDeleted As gPMConstants.PMEVarTrueFalse = 0,
                                   Optional ByRef vEffectiveDate As Object = Nothing,
                                   Optional ByRef vLoggedOnAtClient As Object = Nothing,
                                   Optional ByRef vIsPMBLinkRequired As Object = Nothing,
                                   Optional ByRef vServerPrinter As Object = Nothing,
                                   Optional ByRef vIsPrinterChangeable As Object = Nothing,
                                   Optional ByRef vEmailAddress As Object = Nothing,
                                   Optional ByRef vFullName As Object = Nothing,
                                   Optional ByRef vSignatureFile As Object = Nothing,
                                   Optional ByRef vTitle As Object = Nothing,
                                   Optional ByRef vTelephoneNumber As Object = Nothing,
                                   Optional ByRef vExtensionNumber As Object = Nothing,
                                   Optional ByRef vFaxNumber As Object = Nothing,
                                   Optional ByRef vJobTitleID As Object = Nothing,
                                   Optional ByRef vClaimHandlerId As Object = Nothing,
                                   Optional ByRef vPartyHandlerId As Object = Nothing,
                                   Optional ByRef vInitials As Object = Nothing,
                                   Optional ByRef vMobileNumber As Object = Nothing,
                                   Optional ByRef vOtherPartyId As Object = Nothing,
                                   Optional ByRef vAlternativeIdentifier As Object = Nothing,
                                   Optional ByRef vJobBasis As Object = Nothing,
                                   Optional ByRef vPercentHoursWorked As Object = Nothing,
                                   Optional ByRef vSiriusUser As Object = Nothing,
                                   Optional ByRef vDateDeleted As Object = Nothing,
                                   Optional vIsTempPassword As Object = Nothing,
                                   Optional ByVal bSystemUpgradeTempPwd As Boolean = False,
                                   Optional ByVal sOldPassword As String = "",
                                   Optional ByVal sPasswordChanged As String = "",
                                   Optional ByVal sUniqueId As String = "",
                                   Optional ByVal sScreenHierarhcy As String = "", Optional vSSOPreferredName As Object = Nothing) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'decide whether call is an add or an edit
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            'decide whether all of the mandatory values have been supplied
            m_lError = MandatoryParameterCheck(vUserId:=vUserId, vLanguageID:=vLanguageID,
                                               vUsername:=vUsername, vPassword:=vPassword,
                                               vPasswordChangeDate:=vPasswordChangeDate,
                                               vDateCreated:=vDateCreated, vLastLogin:=vLastLogin,
                                               vPartyCnt:=vPartyCnt, vIsDeleted:=vIsDeleted,
                                               vEffectiveDate:=vEffectiveDate, vEmailAddress:=vEmailAddress,
                                               vFullName:=vFullName, vSignatureFile:=vSignatureFile,
                                               vTitle:=vTitle, vTelephoneNumber:=vTelephoneNumber,
                                               vExtensionNumber:=vExtensionNumber, vFaxNumber:=vFaxNumber,
                                               vJobTitleID:=vJobTitleID, vClaimHandlerId:=vClaimHandlerId,
                                               vPartyHandlerId:=vPartyHandlerId, vInitials:=vInitials,
                                               vOtherPartyId:=vOtherPartyId, vMobileNumber:=vMobileNumber, vSSOPreferredName:=vSSOPreferredName)


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'supply defaults for any missing parameters
            ' AMB 24-Nov-03: 1.8.6 Unified login code from 1.9

            'Developer Guide No 98
            m_lError = DefaultMissingParameters(vUserId:=vUserId, vLanguageID:=vLanguageID,
                                               vUsername:=vUsername, vPassword:=vPassword,
                                               vPasswordChangeDate:=vPasswordChangeDate,
                                               vDateCreated:=vDateCreated, vLastLogin:=vLastLogin,
                                               vPartyCnt:=vPartyCnt, vIsDeleted:=vIsDeleted,
                                               vEffectiveDate:=vEffectiveDate, vServerPrinter:=vServerPrinter,
                                               vIsPrinterChangeable:=vIsPrinterChangeable,
                                               vEmailAddress:=vEmailAddress, vFullName:=vFullName,
                                               vSignatureFile:=vSignatureFile, vTitle:=vTitle,
                                               vTelephoneNumber:=vTelephoneNumber,
                                               vExtensionNumber:=vExtensionNumber, vFaxNumber:=vFaxNumber,
                                               vJobTitleID:=vJobTitleID, vClaimHandlerId:=vClaimHandlerId,
                                               vPartyHandlerId:=vPartyHandlerId, vInitials:=vInitials,
                                               vMobileNumber:=vMobileNumber, vOtherPartyId:=vOtherPartyId,
                                               vAlternativeIdentifier:=vAlternativeIdentifier,
                                               vJobBasis:=vJobBasis, vPercentHoursWorked:=vPercentHoursWorked,
                                               vSiriusUser:=vSiriusUser, vDateDeleted:=vDateDeleted,
                                               vIsTempPassword:=vIsTempPassword, vSSOPreferredName:=vSSOPreferredName)

            If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'check whether the values in the parameters are valid
        m_lError = ValidateParameters(vUserId:=vUserId, vLanguageID:=vLanguageID, vUsername:=vUsername,
                                      vPassword:=vPassword, vPasswordChangeDate:=vPasswordChangeDate,
                                      vDateCreated:=vDateCreated, vLastLogin:=vLastLogin, vPartyCnt:=vPartyCnt,
                                      vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate,
                                      vEmailAddress:=vEmailAddress, vFullName:=vFullName,
                                      vSignatureFile:=vSignatureFile, vTitle:=vTitle,
                                      vTelephoneNumber:=vTelephoneNumber, vExtensionNumber:=vExtensionNumber,
                                      vFaxNumber:=vFaxNumber, vJobTitleID:=vJobTitleID,
                                      vClaimHandlerId:=vClaimHandlerId, vPartyHandlerId:=vPartyHandlerId,
                                      vInitials:=vInitials, vOtherPartyId:=vOtherPartyId,
                                      vMobileNumber:=vMobileNumber, vIsTempPassword:=vIsTempPassword,
                                      vSSOPreferredName:=vSSOPreferredName)


        If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Set Property values.
        With oPMUser

            If Not Informations.IsNothing(vUserId) Then
                .UserID = vUserId
            End If

            If Not Informations.IsNothing(vLanguageID) Then
                .LanguageID = vLanguageID
            End If
            'PN27568 07/04/2006

            If Not Informations.IsNothing(vUsername) Then
                .Username = vUsername.Replace("'", "''")
            End If

            If Not Informations.IsNothing(vPassword) Then
                .Password = vPassword
            End If
            .PasswordChanged = sPasswordChanged
            If Not Informations.IsNothing(sOldPassword) Then
                .OldPassword = sOldPassword
            End If

            If Not Informations.IsNothing(vPasswordChangeDate) Then
                .PasswordChangeDate = vPasswordChangeDate
            End If

            If Not Informations.IsNothing(vDateCreated) Then
                .DateCreated = vDateCreated
            End If

            If Not Informations.IsNothing(vLastLogin) Then
                .Lastlogin = vLastLogin
            End If

            If Not Informations.IsNothing(vPartyCnt) Then
                .PartyCnt = vPartyCnt
            End If

            If Not Informations.IsNothing(vIsDeleted) Then
                .IsDeleted = vIsDeleted
            End If

            If Not Informations.IsNothing(vEffectiveDate) Then

                .EffectiveDate = CDate(vEffectiveDate)
            End If

            If Not Informations.IsNothing(vLoggedOnAtClient) Then
                .LoggedOnAtClient = vLoggedOnAtClient
            End If

            If Not Informations.IsNothing(vIsPMBLinkRequired) Then
                .IsPMBLinkRequired = vIsPMBLinkRequired
            End If

            If Not Informations.IsNothing(vServerPrinter) AndAlso vServerPrinter <> "" Then
                .ServerPrinter = vServerPrinter
            End If

            If Not Informations.IsNothing(vIsPrinterChangeable) Then
                .IsPrinterChangeable = vIsPrinterChangeable
            End If
            ' CTAF 20030721 - Email Address - Start

            If Not Informations.IsNothing(vEmailAddress) Then
                .EmailAddress = gPMFunctions.NullToString(vEmailAddress)
            End If
            ' CTAF 20030721 - Email Address - End

            If Not Informations.IsNothing(vSSOPreferredName) Then
                .SSOPreferredName = gPMFunctions.NullToString(vSSOPreferredName)
            End If

            'DC040903 -start
            'PN:27568

            If Not Informations.IsNothing(vFullName) Then
                .FullName = gPMFunctions.NullToString(vFullName).Replace("'", "''")
            End If

            If Not Informations.IsNothing(vMobileNumber) Then
                .MobileNumber = gPMFunctions.NullToString(vMobileNumber)
            End If

            If Not Informations.IsNothing(vInitials) Then
                .Initials = gPMFunctions.NullToString(vInitials)
            End If

            If Not Informations.IsNothing(vSignatureFile) Then
                .SignatureFile = gPMFunctions.NullToString(vSignatureFile)
            End If

            If Not Informations.IsNothing(vTitle) Then
                .Title = gPMFunctions.NullToString(vTitle)
            End If

            If Not Informations.IsNothing(vTelephoneNumber) Then
                .TelephoneNumber = gPMFunctions.NullToString(vTelephoneNumber)
            End If

            If Not Informations.IsNothing(vExtensionNumber) Then
                .ExtensionNumber = gPMFunctions.NullToString(vExtensionNumber)
            End If

            If Not Informations.IsNothing(vFaxNumber) Then
                .FaxNumber = gPMFunctions.NullToString(vFaxNumber)
            End If

            If Not Informations.IsNothing(vJobTitleID) Then
                .JobTitleId = gPMFunctions.NullToLong(vJobTitleID)
            End If

            If Not Informations.IsNothing(vClaimHandlerId) Then
                .ClaimHandlerId = gPMFunctions.NullToLong(vClaimHandlerId)
            End If

            If Not Informations.IsNothing(vPartyHandlerId) Then
                .PartyHandlerId = gPMFunctions.NullToLong(vPartyHandlerId)
            End If


            If Not Informations.IsNothing(vOtherPartyId) Then
                .OtherPartyId = gPMFunctions.NullToLong(vOtherPartyId)
            End If

            'DC040903 -end
            'AK 08072003 - add AlternativeIdentifier (ps#246)

            If Not Informations.IsNothing(vAlternativeIdentifier) Then
                .AlternativeIdentifier = gPMFunctions.NullToString(vAlternativeIdentifier)
            End If


            If Not Informations.IsNothing(vJobBasis) Then
                .JobBasis = gPMFunctions.ToSafeLong(vJobBasis)
            End If


            If Not Informations.IsNothing(vPercentHoursWorked) Then
                .PercentHoursWorked = gPMFunctions.ToSafeDouble(vPercentHoursWorked)
            End If


            If Not Informations.IsNothing(vSiriusUser) Then
                .IsSiriusUser = gPMFunctions.ToSafeBoolean(vSiriusUser)
            End If


            If Not Informations.IsNothing(vDateDeleted) Then
                .DateDeleted = vDateDeleted
            End If

            'If user is not deleted or Undelete the Date Delete reset to Null 
            If .IsDeleted = 0 Then
                .DateDeleted = Nothing
            End If

            If Not Informations.IsNothing(vIsTempPassword) Then
                .IsTempPassword = vIsTempPassword
            End If
            If Not Informations.IsNothing(bSystemUpgradeTempPwd) Then
                .IsSystemUpgradeTempPwd = bSystemUpgradeTempPwd
            End If

            If Not String.IsNullOrEmpty(sUniqueId) Then
                .UniqueId = sUniqueId
                .ScreenHierarchy = sScreenHierarhcy
            End If

            .DatabaseStatus = iStatus

        End With

        Return result

    End Function


    ''' <summary>
    ''' GetProperties (Private)-Returns the supplied PMUser property values.
    ''' </summary>
    ''' <param name="oPMUser"></param>
    ''' <param name="vStatus"></param>
    ''' <param name="vUserId"></param>
    ''' <param name="vLanguageID"></param>
    ''' <param name="vUsername"></param>
    ''' <param name="vPassword"></param>
    ''' <param name="vPasswordChangeDate"></param>
    ''' <param name="vDateCreated"></param>
    ''' <param name="vLastLogin"></param>
    ''' <param name="vPartyCnt"></param>
    ''' <param name="vIsDeleted"></param>
    ''' <param name="vEffectiveDate"></param>
    ''' <param name="vLoggedOnAtClient"></param>
    ''' <param name="vIsPMBLinkRequired"></param>
    ''' <param name="vServerPrinter"></param>
    ''' <param name="vIsPrinterChangeable"></param>
    ''' <param name="vEmailAddress"></param>
    ''' <param name="vFullName"></param>
    ''' <param name="vSignatureFile"></param>
    ''' <param name="vTitle"></param>
    ''' <param name="vTelephoneNumber"></param>
    ''' <param name="vExtensionNumber"></param>
    ''' <param name="vFaxNumber"></param>
    ''' <param name="vJobTitleID"></param>
    ''' <param name="vClaimHandlerId"></param>
    ''' <param name="vPartyHandlerId"></param>
    ''' <param name="vInitials"></param>
    ''' <param name="vMobileNumber"></param>
    ''' <param name="vOtherPartyId"></param>
    ''' <param name="vAlternativeIdentifier"></param>
    ''' <param name="vJobBasis"></param>
    ''' <param name="vPercentHoursWorked"></param>
    ''' <param name="vSiriusUser"></param>
    ''' <param name="vDateDeleted"></param>
    ''' <param name="o_bIsUserTempPassword"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function GetProperties(ByRef oPMUser As bPMUser.PMUser, ByRef vStatus As gPMConstants.PMEComponentAction,
                                   ByRef vUserId As Integer, ByRef vLanguageID As Integer, ByRef vUsername As String,
                                   ByRef vPassword As String, ByRef vPasswordChangeDate As Date,
                                   ByRef vDateCreated As Date, ByRef vLastLogin As Date, ByRef vPartyCnt As Integer,
                                   ByRef vIsDeleted As gPMConstants.PMEVarTrueFalse, ByRef vEffectiveDate As Object,
                                   Optional ByRef vLoggedOnAtClient As Object = Nothing,
                                   Optional ByRef vIsPMBLinkRequired As Object = Nothing,
                                   Optional ByRef vServerPrinter As Object = Nothing,
                                   Optional ByRef vIsPrinterChangeable As Object = Nothing,
                                   Optional ByRef vEmailAddress As Object = Nothing,
                                   Optional ByRef vFullName As Object = Nothing,
                                   Optional ByRef vSignatureFile As Object = Nothing,
                                   Optional ByRef vTitle As Object = Nothing,
                                   Optional ByRef vTelephoneNumber As Object = Nothing,
                                   Optional ByRef vExtensionNumber As Object = Nothing,
                                   Optional ByRef vFaxNumber As Object = Nothing,
                                   Optional ByRef vJobTitleID As Object = Nothing,
                                   Optional ByRef vClaimHandlerId As Object = Nothing,
                                   Optional ByRef vPartyHandlerId As Object = Nothing,
                                   Optional ByRef vInitials As Object = Nothing,
                                   Optional ByRef vMobileNumber As Object = Nothing,
                                   Optional ByRef vOtherPartyId As Object = Nothing,
                                   Optional ByRef vAlternativeIdentifier As Object = Nothing,
                                   Optional ByRef vJobBasis As Object = Nothing,
                                   Optional ByRef vPercentHoursWorked As Object = Nothing,
                                   Optional ByRef vSiriusUser As Object = Nothing,
                                   Optional ByRef vDateDeleted As Object = Nothing,
                                   Optional ByRef o_bIsUserTempPassword As Boolean = Nothing,
                                   Optional ByRef vOldPassword As String = "", Optional vSSOPreferredName As Object = Nothing) As Integer


        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPMUser
            'Developer Guide No. 118
            vUserId = .UserID

            'Developer Guide No. 118
            vLanguageID = .LanguageID

            'Developer Guide No. 118
            vUsername = .Username

            'Developer Guide No. 118
            vPassword = .Password
            vOldPassword = .OldPassword
            'Developer Guide No. 118
            vPasswordChangeDate = .PasswordChangeDate

            'Developer Guide No. 118
            vDateCreated = .DateCreated

            'Developer Guide No. 118
            vLastLogin = .Lastlogin

            'Developer Guide No. 118
            vPartyCnt = .PartyCnt

            'Developer Guide No. 118
            vIsDeleted = .IsDeleted

            'Developer Guide No. 118

            vEffectiveDate = .EffectiveDate


            'Developer Guide No. 118
            vLoggedOnAtClient = .LoggedOnAtClient


            'Developer Guide No. 118
            vIsPMBLinkRequired = .IsPMBLinkRequired


            'Developer Guide No. 118

            vServerPrinter = If(Convert.IsDBNull(.ServerPrinter) Or Informations.IsNothing(.ServerPrinter), "", .ServerPrinter)


            'Developer Guide No. 118
            vIsPrinterChangeable = .IsPrinterChangeable

            'Developer Guide No. 118
            vStatus = .DatabaseStatus


            'Developer Guide No. 118
            vEmailAddress = .EmailAddress
            vSSOPreferredName = .SSOPreferredName

            'DC040903 -start

            'Developer Guide No. 118
            vFullName = .FullName


            'Developer Guide No. 118
            vMobileNumber = .MobileNumber


            'Developer Guide No. 118
            vInitials = .Initials


            'Developer Guide No. 118
            vSignatureFile = .SignatureFile


            'Developer Guide No. 118
            vTitle = .Title


            'Developer Guide No. 118
            vTelephoneNumber = .TelephoneNumber


            'Developer Guide No. 118
            vExtensionNumber = .ExtensionNumber


            'Developer Guide No. 118
            vFaxNumber = .FaxNumber


            'Developer Guide No. 118
            vJobTitleID = .JobTitleId


            'Developer Guide No. 118
            vClaimHandlerId = .ClaimHandlerId


            'Developer Guide No. 118
            vPartyHandlerId = .PartyHandlerId


            'Developer Guide No. 118
            vOtherPartyId = .OtherPartyId

            'DC040903

            'AK 08072003 - add Alternative identifier ps#246
            ' AMB 24-Nov-03: 1.8.6 Unified login code from 1.9

            'Developer Guide No. 118
            vAlternativeIdentifier = .AlternativeIdentifier


            'Developer Guide No. 118
            vJobBasis = .JobBasis


            'Developer Guide No. 118
            vPercentHoursWorked = .PercentHoursWorked


            'Developer Guide No. 118
            vSiriusUser = .IsSiriusUser


            'Developer Guide No. 118
            vDateDeleted = .DateDeleted
            o_bIsUserTempPassword = .IsTempPassword

        End With

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oPMUser As bPMUser.PMUser) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lError = .Parameters.Add(sName:="language_id", vValue:=CStr(oPMUser.LanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="username", vValue:=oPMUser.Username, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="secure_password", vValue:=oPMUser.Password,
                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                       iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lError = .Parameters.Add(sName:="password", vValue:=oPMUser.OldPassword,
                           iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                           iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No. 113
            If oPMUser.PasswordChangeDate = #12:00:00 AM# OrElse oPMUser.IsSystemUpgradeTempPwd Then
                'set to nul
                m_lError = .Parameters.Add(sName:="password_change_date", vValue:=ToSafeDate("29/12/1899", #12/29/1899#), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lError = .Parameters.Add(sName:="password_change_date", vValue:=oPMUser.PasswordChangeDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If oPMUser.DateCreated = #12:00:00 AM# Then
                'set to nul
                m_lError = .Parameters.Add(sName:="date_created", vValue:="12/29/1899", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lError = .Parameters.Add(sName:="date_created", vValue:=oPMUser.DateCreated, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If oPMUser.Lastlogin = #12:00:00 AM# Then
                'set to nul
                m_lError = .Parameters.Add(sName:="lastlogin", vValue:="12/29/1899", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lError = .Parameters.Add(sName:="lastlogin", vValue:=oPMUser.Lastlogin, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="logged_on_at_client", vValue:=oPMUser.LoggedOnAtClient, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'if party_cnt is 0 then set parameter value to null
            If oPMUser.PartyCnt = 0 Then
                'set to null
                'm_lError = .Parameters.Add(sName:="party_cnt", vValue:="Null", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMTableName)
                m_lError = .Parameters.Add(sName:="party_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMTableName)
            Else
                m_lError = .Parameters.Add(sName:="party_cnt", vValue:=CStr(oPMUser.PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Is PMB Link Required
            m_lError = .Parameters.Add(sName:="is_pmb_link_required", vValue:=CStr(oPMUser.IsPMBLinkRequired), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Server Printer

            If Convert.IsDBNull(oPMUser.ServerPrinter) Or Informations.IsNothing(oPMUser.ServerPrinter) Then
                m_lError = .Parameters.Add(sName:="server_printer", vValue:="Null", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMTableName)
            ElseIf (oPMUser.ServerPrinter = "") Then
                m_lError = .Parameters.Add(sName:="server_printer", vValue:=CStr(-1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lError = .Parameters.Add(sName:="server_printer", vValue:=oPMUser.ServerPrinter, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Server Printer
            '        m_lError& = .Parameters.Add( _
            'sName:="server_printer", _
            'vValue:=oPMUser.ServerPrinter, _
            'iDirection:=PMParamInput, _
            'iDataType:=PMString)

            '        If (m_lError& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If

            ' Is Printer Changeable
            m_lError = .Parameters.Add(sName:="is_printer_changeable", vValue:=CStr(oPMUser.IsPrinterChangeable), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="is_deleted", vValue:=CStr(oPMUser.IsDeleted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'datetime helper not required
            'm_lError = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(oPMUser.EffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lError = .Parameters.Add(sName:="effective_date", vValue:=oPMUser.EffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 20030721 - start
            If oPMUser.EmailAddress = "" Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="email_address", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="email_address", vValue:=oPMUser.EmailAddress, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            ' CTAF 20030721 - end

            If oPMUser.SSOPreferredName = "" Then
                m_lError = .Parameters.Add(sName:="SSO_Preferred_Username", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="SSO_Preferred_Username", vValue:=oPMUser.SSOPreferredName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If


            'DC040903 -start
            If oPMUser.FullName = "" Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="full_name", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="full_name", vValue:=oPMUser.FullName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            'AK 08072003 - alternative identifier PS#246
            m_lError = .Parameters.Add(sName:="alternative_identifier", vValue:=oPMUser.AlternativeIdentifier, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oPMUser.MobileNumber = "" Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="mobile_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="mobile_number", vValue:=oPMUser.MobileNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.Initials = "" Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="initials", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="initials", vValue:=oPMUser.Initials, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.SignatureFile = "" Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="signature_file", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="signature_file", vValue:=oPMUser.SignatureFile, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.Title = "" Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="title", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="title", vValue:=oPMUser.Title, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.TelephoneNumber = "" Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="telephone_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="telephone_number", vValue:=oPMUser.TelephoneNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.ExtensionNumber = "" Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="extension_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="extension_number", vValue:=oPMUser.ExtensionNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If oPMUser.FaxNumber = "" Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="fax_number", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="fax_number", vValue:=oPMUser.FaxNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            'DC041103 -PN8111 -check
            If oPMUser.JobTitleId = -1 Or oPMUser.JobTitleId = 0 Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="job_title_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="job_title_id", vValue:=oPMUser.JobTitleId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If oPMUser.ClaimHandlerId = 0 Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="claim_handler_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lError = .Parameters.Add(sName:="claim_handler_id", vValue:=CStr(oPMUser.ClaimHandlerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If oPMUser.PartyHandlerId = 0 Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="party_handler_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            Else
                m_lError = .Parameters.Add(sName:="party_handler_id", vValue:=CStr(oPMUser.PartyHandlerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            '(RC) WR34
            If oPMUser.OtherPartyId = 0 Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="other_party_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lError = .Parameters.Add(sName:="other_party_id", vValue:=CStr(oPMUser.OtherPartyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            m_lError = .Parameters.Add(sName:="job_basis", vValue:=CStr(oPMUser.JobBasis), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lError = .Parameters.Add(sName:="percent_hours_worked", vValue:=CStr(oPMUser.PercentHoursWorked), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            'm_lError = .Parameters.Add(sName:="sirius_user", vValue:=CStr(oPMUser.IsSiriusUser), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lError = .Parameters.Add(sName:="sirius_user", vValue:=CInt(oPMUser.IsSiriusUser), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If Informations.IsNothing(oPMUser.DateDeleted) Or Convert.IsDBNull(oPMUser.DateDeleted) Then
                m_lError = .Parameters.Add(sName:="date_deleted", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lError = .Parameters.Add(sName:="date_deleted", vValue:=oPMUser.DateDeleted, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If Informations.IsNothing(oPMUser.IsTempPassword) Or Convert.IsDBNull(oPMUser.IsTempPassword) Then
                m_lError = .Parameters.Add(sName:="is_temp_password", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            Else
                m_lError = .Parameters.Add(sName:="is_temp_password", vValue:=CInt(oPMUser.IsTempPassword), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            End If

            m_lError = .Parameters.Add(sName:="modified_by", vValue:=CInt(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If String.IsNullOrEmpty(oPMUser.UniqueId) Then
                m_lError = .Parameters.Add(sName:="unique_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lError = .Parameters.Add(sName:="screen_hierarchy", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lError = .Parameters.Add(sName:="UniqueId", vValue:=oPMUser.UniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lError = .Parameters.Add(sName:="ScreenHierarchy", vValue:=oPMUser.ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If



            'DC040903 -end

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ValidateParameters (Private)
    '
    ' Description: Checks that all paramaters are valid for the datatype
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Private Function ValidateParameters(Optional ByRef vUserId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vPasswordChangeDate As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastLogin As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vLoggedOnAtClient As Object = Nothing, Optional ByRef vIsPMBLinkRequired As Object = Nothing, Optional ByRef vServerPrinter As Object = Nothing, Optional ByRef vIsPrinterChangeable As Object = Nothing, Optional ByRef vEmailAddress As Object = Nothing, Optional ByRef vFullName As Object = Nothing, Optional ByRef vSignatureFile As Object = Nothing, Optional ByRef vTitle As Object = Nothing, Optional ByRef vTelephoneNumber As Object = Nothing, Optional ByRef vExtensionNumber As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vJobTitleID As Object = Nothing, Optional ByRef vClaimHandlerId As Object = Nothing, Optional ByRef vPartyHandlerId As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vOtherPartyId As Object = Nothing, Optional ByRef vMobileNumber As Object = Nothing, Optional vIsTempPassword As Object = Nothing, Optional vSSOPreferredName As Object = Nothing) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Informations.IsNothing(vUserId) Then
            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vUserId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                ValidateParameters(gPMConstants.PMEReturnCode.PMFalse)

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vUserID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vLanguageID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vLanguageID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vLanguageID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vUsername) Then

        End If


        If Not Informations.IsNothing(vPassword) Then

        End If


        If Not Informations.IsNothing(vPasswordChangeDate) Then
            If Not Informations.IsDate(vPasswordChangeDate) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vPasswordChangeDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vDateCreated) Then
            If Not Informations.IsDate(vDateCreated) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vDateCreated Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vLastLogin) Then
            If Not Informations.IsDate(vLastLogin) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vLastLogin Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vPartyCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vIsDeleted) Then
            If vIsDeleted <> 0 And vIsDeleted <> 1 Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vIsDeleted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vEffectiveDate) Then
            If Not Informations.IsDate(vEffectiveDate) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vEffectiveDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vEmailAddress) Then

        End If

        'DC080903 -start

        If Not Informations.IsNothing(vFullName) Then

        End If

        If Not Informations.IsNothing(vMobileNumber) Then

        End If

        If Not Informations.IsNothing(vInitials) Then

        End If

        If Not Informations.IsNothing(vSignatureFile) Then

        End If

        If Not Informations.IsNothing(vTitle) Then

        End If

        If Not Informations.IsNothing(vTelephoneNumber) Then

        End If

        If Not Informations.IsNothing(vExtensionNumber) Then

        End If

        If Not Informations.IsNothing(vFaxNumber) Then

        End If

        If Not Informations.IsNothing(vJobTitleID) Then

        End If

        If Not Informations.IsNothing(vClaimHandlerId) Then

        End If

        If Not Informations.IsNothing(vPartyHandlerId) Then

        End If

        '(RC) WR34



        If Not Informations.IsNothing(vOtherPartyId) And Not Object.Equals(vOtherPartyId, Nothing) And Not (Convert.IsDBNull(vOtherPartyId) Or Informations.IsNothing(vOtherPartyId)) Then


            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vOtherPartyId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vOtherPartyId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If

        If Not Informations.IsNothing(vIsTempPassword) Then
            If vIsTempPassword <> False And vIsTempPassword <> True Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vIsTempPassword  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If

        'DC080903 -end

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultMissingProperties (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied and sets defaults for the non mandatory ones
    '
    'AK 08072003 - new paramaetr for defaulting alternative identifier PS#246
    ' AMB 24-Nov-03: 1.8.6 Unified login code from 1.9
    ' ***************************************************************** '
    'Developer Guide No 101
    Private Function DefaultMissingParameters(Optional ByRef vUserId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vPasswordChangeDate As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastLogin As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vLoggedOnAtClient As Object = Nothing, Optional ByRef vIsPMBLinkRequired As Object = Nothing, Optional ByRef vServerPrinter As Object = Nothing, Optional ByRef vIsPrinterChangeable As Object = Nothing, Optional ByRef vEmailAddress As Object = Nothing, Optional ByRef vFullName As Object = Nothing, Optional ByRef vSignatureFile As Object = Nothing, Optional ByRef vTitle As Object = Nothing, Optional ByRef vTelephoneNumber As Object = Nothing, Optional ByRef vExtensionNumber As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vJobTitleID As Object = Nothing, Optional ByRef vClaimHandlerId As Object = Nothing, Optional ByRef vPartyHandlerId As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vMobileNumber As Object = Nothing, Optional ByRef vOtherPartyId As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vJobBasis As Object = Nothing, Optional ByRef vPercentHoursWorked As Object = Nothing, Optional ByRef vSiriusUser As Object = Nothing, Optional ByRef vDateDeleted As Object = Nothing, Optional vIsTempPassword As Object = Nothing, Optional vSSOPreferredName As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'set defaults for any properties which have not been supplied

        If Informations.IsNothing(vUserId) Then
            vUserId = 1
        End If


        If Informations.IsNothing(vPasswordChangeDate) Then
            vPasswordChangeDate = DateTime.Now
        End If


        If Informations.IsNothing(vDateCreated) Then
            vDateCreated = DateTime.Now
        End If


        If Informations.IsNothing(vLastLogin) Then
            vLastLogin = DateTime.Now
        End If


        If Informations.IsNothing(vPartyCnt) Then
            vPartyCnt = 0
        End If


        If Informations.IsNothing(vIsDeleted) Then
            vIsDeleted = 0
        End If


        If Informations.IsNothing(vEffectiveDate) Then
            vEffectiveDate = DateTime.Now
        End If


        If Informations.IsNothing(vLoggedOnAtClient) Then


            vLoggedOnAtClient = DBNull.Value
        End If


        If Informations.IsNothing(vIsPMBLinkRequired) Then
            vIsPMBLinkRequired = 0
        End If


        If Informations.IsNothing(vServerPrinter) Then


            vServerPrinter = DBNull.Value
        End If


        If Informations.IsNothing(vIsPrinterChangeable) Then
            vIsPrinterChangeable = 0
        End If


        If Informations.IsNothing(vEmailAddress) Then


            vEmailAddress = DBNull.Value
        End If

        'DC040903 -start

        If Informations.IsNothing(vFullName) Then


            vFullName = DBNull.Value
        End If

        If Informations.IsNothing(vSSOPreferredName) Then
            vSSOPreferredName = DBNull.Value
        End If

        If Informations.IsNothing(vMobileNumber) Then


            vMobileNumber = DBNull.Value
        End If


        If Informations.IsNothing(vInitials) Then


            vInitials = DBNull.Value
        End If


        If Informations.IsNothing(vSignatureFile) Then


            vSignatureFile = DBNull.Value
        End If


        If Informations.IsNothing(vTitle) Then


            vTitle = DBNull.Value
        End If


        If Informations.IsNothing(vTelephoneNumber) Then


            vTelephoneNumber = DBNull.Value
        End If


        If Informations.IsNothing(vExtensionNumber) Then


            vExtensionNumber = DBNull.Value
        End If


        If Informations.IsNothing(vFaxNumber) Then


            vFaxNumber = DBNull.Value
        End If


        If Informations.IsNothing(vJobTitleID) Then


            vJobTitleID = DBNull.Value
        End If


        If Informations.IsNothing(vClaimHandlerId) Then


            vClaimHandlerId = DBNull.Value
        End If


        If Informations.IsNothing(vPartyHandlerId) Then


            vPartyHandlerId = DBNull.Value
        End If


        If Informations.IsNothing(vOtherPartyId) Then


            vOtherPartyId = DBNull.Value
        End If
        'DC040903 -end

        'AK 08072003
        ' AMB 24-Nov-03: 1.8.6 Unified login code from 1.9

        If Informations.IsNothing(vAlternativeIdentifier) Then


            vAlternativeIdentifier = DBNull.Value
        End If


        If Informations.IsNothing(vJobBasis) Then
            vJobBasis = 0
        End If


        If Informations.IsNothing(vPercentHoursWorked) Then
            vPercentHoursWorked = 0
        End If


        If Informations.IsNothing(vSiriusUser) Then
            vSiriusUser = 0
        End If


        If Informations.IsNothing(vDateDeleted) Then


            vDateDeleted = DBNull.Value
        End If

        If Informations.IsNothing(vIsTempPassword) Then
            vIsTempPassword = 0
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lError = m_oDatabase.SQLBeginTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MandatoryParameterCheck (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied
    '
    ' ***************************************************************** '

    Private Function MandatoryParameterCheck(Optional ByRef vUserId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vPasswordChangeDate As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastLogin As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vLoggedOnAtClient As Object = Nothing, Optional ByRef vIsPMBLinkRequired As Object = Nothing, Optional ByRef vServerPrinter As Object = Nothing, Optional ByRef vIsPrinterChangeable As Object = Nothing, Optional ByRef vEmailAddress As Object = Nothing, Optional ByRef vFullName As Object = Nothing, Optional ByRef vSignatureFile As Object = Nothing, Optional ByRef vTitle As Object = Nothing, Optional ByRef vTelephoneNumber As Object = Nothing, Optional ByRef vExtensionNumber As Object = Nothing, Optional ByRef vFaxNumber As Object = Nothing, Optional ByRef vJobTitleID As Object = Nothing, Optional ByRef vClaimHandlerId As Object = Nothing, Optional ByRef vPartyHandlerId As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vOtherPartyId As Object = Nothing, Optional ByRef vMobileNumber As Object = Nothing, Optional vSSOPreferredName As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'check that all mandatory parameters have been supplied

        If Informations.IsNothing(vLanguageID) Or Informations.IsNothing(vUsername) Or Informations.IsNothing(vPassword) Then

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Mandatory Property WasNot Supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAndDefaultProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lError = m_oDatabase.SQLCommitTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lError = m_oDatabase.SQLRollbackTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSources
    '
    ' Description: Gets the source_ids from the PMUser_Source table
    '
    ' History: 14/04/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function GetSources(ByRef oPMUser As Bpmuser.PMUser, Optional ByVal v_bIncludeDeletedSources As Boolean = False, Optional ByVal v_lProductID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'Developer Guide No. 112
        Dim oFields As DataRow
        Dim oPMUserSource As Bpmuser.PMUserSource



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set the SQL parameters
        m_oDatabase.Parameters.Clear()

        m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(oPMUser.UserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the data from the database
        If v_bIncludeDeletedSources Then
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACSelectPMUserSourceIncDeletedSQL, sSQLName:=ACSelectPMUserSourceName, bStoredProcedure:=ACSelectPMUserSourceStored, lNumberRecords:=0)
        Else
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACSelectPMUserSourceSQL, sSQLName:=ACSelectPMUserSourceName, bStoredProcedure:=ACSelectPMUserSourceStored, lNumberRecords:=0)
        End If

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Populate the PMUserSources collection for the PMUser
        lRecordCount = m_oDatabase.Records.Count()

        ' Do we have any records ?

        If lRecordCount < 1 Then
            Return result
        End If

        ' Yes, load them into the collection
        For lSub As Integer = 1 To lRecordCount
            ' Create New PMUserSource
            oPMUserSource = New Bpmuser.PMUserSource()
            'Developer Guide No. 111
            oFields = m_oDatabase.Records.Item(lSub - 1).Fields()
            oPMUserSource.IsSaved = True
            oPMUserSource.SourceID = oFields("source_id")
            oPMUserSource.SourceCode = oFields("code")
            'DAK220500
            oPMUserSource.Description = oFields("description")
            oPMUserSource.CountryID = oFields("Country_id")
            oPMUserSource.DatabaseStatus = gPMConstants.PMEComponentAction.PMView
            ' Add it to the collection
            If (oPMUser.PMUserSources.Count = 0) Then
                oPMUser.PMUserSources.Add(Nothing)
            End If
            m_lError = oPMUser.PMUserSources.Add(oNewPMUserSource:=oPMUserSource)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFields = Nothing
            oPMUserSource = Nothing

        Next lSub

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPMUserSources
    '
    ' Description:
    '
    ' History: 03/05/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function GetPMUserSources(Optional ByVal v_bIncludeDeletedSources As Boolean = False, Optional ByVal v_lProductID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oPMUser As Bpmuser.PMUser




        result = gPMConstants.PMEReturnCode.PMTrue

        For lSub As Integer = 1 To m_oPMUsers.Count()

            oPMUser = m_oPMUsers.Item(lSub)
            If (oPMUser IsNot Nothing) Then
                ' Populate user sources collection
                m_lError = GetSources(oPMUser:=oPMUser, v_bIncludeDeletedSources:=v_bIncludeDeletedSources, v_lProductID:=v_lProductID)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            oPMUser = Nothing

        Next lSub

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateSources
    '
    ' Description:
    '
    ' History: 04/05/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateSources(ByVal v_oPMUser As Bpmuser.PMUser, ByRef r_bTransStarted As Boolean) As Integer
        Dim result As Integer = 0
        Dim oPMUserSource As Bpmuser.PMUserSource




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Loop round Collection

        For lSub As Integer = 1 To v_oPMUser.PMUserSources.Count()
            oPMUserSource = v_oPMUser.PMUserSources.Item(lSub)


            Select Case oPMUserSource.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete, gPMConstants.PMEComponentAction.PMEdit
                    ' Do nothing

                Case gPMConstants.PMEComponentAction.PMAdd

                    ' If we haven't already started a transaction start one.
                    If Not r_bTransStarted Then
                        m_lError = BeginTrans()
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        r_bTransStarted = True
                    End If

                    ' Add Source Item
                    '               m_lError& = AddSourceItem(v_oPMUser, oPMUserSource)
                    m_lError = AddSourceItem(v_oPMUser, oPMUserSource)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case gPMConstants.PMEComponentAction.PMDelete

                    ' If we haven't already started a transaction start one.
                    If Not r_bTransStarted Then
                        m_lError = BeginTrans()
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        r_bTransStarted = True
                    End If

                    ' Delete Source Item
                    '                m_lError& = DeleteSourceItem(v_oPMUser, oPMUserSource)
                    m_lError = DeleteSourceItem(v_oPMUser, oPMUserSource)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

            End Select

            oPMUserSource = Nothing

        Next lSub

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: AddSourceItem
    '
    ' Description:
    '
    ' History: 04/05/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function AddSourceItem(ByVal v_oPMUser As Bpmuser.PMUser, ByVal v_oPMUserSource As Bpmuser.PMUserSource) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lError = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(v_oPMUserSource.SourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_oPMUser.UserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACAddPMUserSourceSQL, sSQLName:=ACAddPMUserSourceName, bStoredProcedure:=ACAddPMUserSourceStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteSourceItem
    '
    ' Description:
    '
    ' History: 04/05/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteSourceItem(ByVal v_oPMUser As Bpmuser.PMUser, ByVal v_oPMUserSource As Bpmuser.PMUserSource) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lError = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(v_oPMUserSource.SourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_oPMUser.UserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACDeletePMUserSourceSQL, sSQLName:=ACDeletePMUserSourceName, bStoredProcedure:=ACDeletePMUserSourceStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetSystemOption
    '
    ' Description: get a value from the hidden options table
    '
    ' ***************************************************************** '
    Public Function GetSystemOption(ByVal lOptionID As Integer, ByVal iSourceID As Integer, ByRef vOptionValue As String) As Integer

        Dim result As Integer = 0
        Dim oDatabase As dPMDAO.Database

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oDatabase = New dPMDAO.Database()

            m_lReturn = CType(gPMComponentServices.NewDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            oDatabase.Parameters.Clear()

            m_lReturn = oDatabase.Parameters.Add(sName:="option_number", vValue:=CStr(lOptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = oDatabase.Parameters.Add(sName:="branch_id", vValue:=CStr(iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = oDatabase.Parameters.Add(sName:="option_value", vValue:=vOptionValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = oDatabase.SQLSelect(sSQL:=ACGetSystemOptionSQL, sSQLName:=ACGetSystemOptionName, bStoredProcedure:=ACGetSystemOptionStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            If Convert.IsDBNull(oDatabase.Parameters.Item("option_value").Value) Or Informations.IsNothing(oDatabase.Parameters.Item("option_value").Value) Then
                vOptionValue = ""
            Else
                vOptionValue = oDatabase.Parameters.Item("option_value").Value
            End If

            oDatabase.CloseDatabase()

            oDatabase = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 17102002 created
    ' ***************************************************************** '
    Public Function GetSysAdminStatus(ByRef lStatus As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(gPMComponentServices.GetSysAdminAccessStatus(m_sUsername, m_iUserID, m_iSourceID, m_iLanguageID, lStatus, m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSysAdminStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a FindParty
    '
    '
    ' ***************************************************************** '
    'Developer Guide No. 17
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = SIRLookupJobTitle
            iLookupType = gPMConstants.PMELookupType.PMLookupAll

            ' Do not supply a key

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC190903
    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'sj 19/06/2002 - start
        m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingOrAgency")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateUserMapping
    '
    ' Description: Update Sirius User Mapping with domain user
    '
    ' ***************************************************************** '
    Public Function UpdateUserMapping(ByVal r_lUserId As Integer, ByVal r_sAlternativeIdentifier As Object, Optional ByVal vResultArray(,) As Object = Nothing, Optional ByVal sUniqueId As String = "") As Integer


        Dim result As Integer = 0
        Dim nUserId As Integer
        Dim sAlternativeIdentifier As String = ""
        Dim bTransApplied As Boolean = False
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            If Not Informations.IsArray(vResultArray) Then

                m_oDatabase.Parameters.Clear()

                ' Add the user_id parameter (INPUT)
                m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(r_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the Alternative_Identifier parameter (INPUT)

                m_lError = m_oDatabase.Parameters.Add(sName:="Alternative_Identifier", vValue:=CStr(r_sAlternativeIdentifier), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = m_oDatabase.Parameters.Add(sName:="modified_by", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=CStr(sUniqueId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACUpdateUserMappingSQL, sSQLName:=ACUpdateUserMappingName, bStoredProcedure:=ACUpdateUserMappingStored)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



            Else
                Dim nRow As Integer

                m_lError = BeginTrans()
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                bTransApplied = True


                m_lError = DeleteUserMappings(sUniqueId:=sUniqueId)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception
                End If

                For nRow = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    nUserId = CInt(vResultArray(0, nRow))
                    sAlternativeIdentifier = CStr(vResultArray(1, nRow))

                    m_oDatabase.Parameters.Clear()

                    ' Add the user_id parameter (INPUT)
                    m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(nUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception
                    End If

                    ' Add the Alternative_Identifier parameter (INPUT)

                    m_lError = m_oDatabase.Parameters.Add(sName:="Alternative_Identifier", vValue:=CStr(sAlternativeIdentifier), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception
                    End If

                    m_lError = m_oDatabase.Parameters.Add(sName:="modified_by", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lError = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=CStr(sUniqueId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement
                    m_lError = m_oDatabase.SQLSelect(sSQL:=ACUpdateUserMappingSQL, sSQLName:=ACUpdateUserMappingName, bStoredProcedure:=ACUpdateUserMappingStored)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception
                    End If

                Next

                m_lError = CommitTrans()
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If
            Return result

        Catch excep As System.Exception

            If bTransApplied Then
                m_lError = RollbackTrans()
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserMapping failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserMapping", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteUserMappings
    '
    ' Description: Remove mapping among the Sirius and domain users
    '
    ' ***************************************************************** '
    Public Function DeleteUserMappings(Optional ByVal sUniqueId As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.Parameters.Add(sName:="modified_by", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=CStr(sUniqueId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACDeleteUserMappingSQL, sSQLName:=ACDeleteUserMappingName, bStoredProcedure:=ACDeleteUserMappingStored)


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteUserMappings failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUserMappings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUsersWithNoPassword
    '
    ' Description: Get Users With No Password
    '
    ' ***************************************************************** '
    Public Function GetUsersWithNoPassword(ByRef r_vUsers(,) As Object, ByVal r_iSecurityModel As Integer, ByVal r_sUserName As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.Parameters.Add(sName:="security_model", vValue:=CStr(r_iSecurityModel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="username", vValue:=CStr(r_sUserName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetUsersWithNoPasswordSQL, sSQLName:=ACGetUsersWithNoPasswordName, bStoredProcedure:=ACGetUsersWithNoPasswordStored, vResultArray:=r_vUsers)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUsersWithNoPassword failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUsersWithNoPassword", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProductOptionValue
    '
    ' Description: Get Users With No Password
    '
    ' ***************************************************************** '
    Public Function SetProductOptionValue(ByVal r_vOption As Object, ByVal r_vBranch As Object, ByVal r_vValue As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the option_number parameter (INPUT)

            m_lError = m_oDatabase.Parameters.Add(sName:="option_number", vValue:=CStr(r_vOption), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the branch_id parameter (INPUT)

            m_lError = m_oDatabase.Parameters.Add(sName:="branch_id", vValue:=CStr(r_vBranch), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the value parameter (INPUT)

            m_lError = m_oDatabase.Parameters.Add(sName:="value", vValue:=CStr(r_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACUpdateProductOptionSQL, sSQLName:=ACUpdateProductOptionName, bStoredProcedure:=ACUpdateProductOptionStored)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProductOptionValue failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProductOptionValue", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' ***************************************************************** '
    Public Function GetUserAdminStatus(ByVal r_iUserId As Integer, ByRef r_lStatus As Integer, ByVal r_iSecurityModel As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(r_iUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="security_model", vValue:=CStr(r_iSecurityModel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            'date time helper not required
            'm_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now.ToShortDateString(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetUserAdminStatusSQL, sSQLName:=ACGetUserAdminStatusName, bStoredProcedure:=ACGetUserAdminStatusStored)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            r_lStatus = m_oDatabase.Records.Fields("sys_admin_count")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAdminStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAdminStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAdminUserCount
    '
    ' Description: Returns system valid administrator count according
    '              to system security model
    '
    ' ***************************************************************** '
    Public Function GetAdminUserCount(ByVal r_iSecurityModel As Integer, ByRef r_lAdminUserCount As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.Parameters.Add(sName:="SecurityMode", vValue:=CStr(r_iSecurityModel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetAdminUserCountSQL, sSQLName:=ACGetAdminUserCountName, bStoredProcedure:=ACGetAdminUserCountStored)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            r_lAdminUserCount = m_oDatabase.Records.Fields("NumberOfSystemAdministrator")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAdminUserCount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAdminUserCount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOtherParty
    '
    ' Description: Gets OtherParty
    '
    ' Created: Rajesh Choudhary 02 Nov 2006 '(RC) WR34
    '
    ' ***************************************************************** '
    Public Function GetOtherParty(ByRef r_vOtherParty(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetOtherPartySQL, sSQLName:=ACGetOtherPartyName, bStoredProcedure:=ACGetOtherPartyStored, vResultArray:=r_vOtherParty, lNumberRecords:=lRecordCount)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOtherPartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAllUsers(ByRef r_vResultArray(,) As Object) As Integer
        Return GetAllUsers(r_vResultArray:=r_vResultArray, v_bIncludeDeletedSources:=False)
    End Function

    Public Function GetAllUsers(ByRef r_vResultArray(,) As Object, ByVal v_bIncludeDeletedSources As Boolean) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.
            m_lError = Cancel()

            ' If changes are outstanding, exit.
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lError
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Informations.IsArray(r_vResultArray) Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllUsers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllUsers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateIncorrectAttemptsAndLockUnlock(ByVal v_iUserId As Integer, ByVal v_iMode As Integer, ByVal v_iIncorrectAttemptAllowed As Integer, ByRef r_iIsLocked As Boolean) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="mode", vValue:=CStr(v_iMode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="incorrect_attempts_allowed", vValue:=CStr(v_iIncorrectAttemptAllowed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If r_iIsLocked = True Then
                m_lError = m_oDatabase.Parameters.Add(sName:="is_locked", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lError = m_oDatabase.Parameters.Add(sName:="is_locked", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLAction(sSQL:=ACUpdateIncorrectAttemptsAndLockUnlockSQL, sSQLName:=ACUpdateIncorrectAttemptsAndLockUnlockName, bStoredProcedure:=ACUpdateIncorrectAttemptsAndLockUnlockStored, lRecordsAffected:=lRecordsAffected)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If r_iIsLocked = True Then
                m_lError = m_oDatabase.Parameters.Add(sName:="is_locked", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lError = m_oDatabase.Parameters.Add(sName:="is_locked", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If


            Return result


        Catch ex As Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateIncorrectAttemptsAndLockUnlock Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllUsers", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return result
        End Try
    End Function


    Public Function GetPartySources(ByVal v_lPartyCnt As Integer, ByVal v_sPartyType As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.Parameters.Add(sName:="Party_Cnt", vValue:=v_lPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            If v_sPartyType = "OT" Then
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetOtherPartySourceSQL, sSQLName:=ACGetOtherPartySourceName, bStoredProcedure:=ACGetOtherPartySourceStored, vResultArray:=r_vResultArray, lNumberRecords:=lRecordCount)
            ElseIf v_sPartyType = "AG" Then
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetAgentSourceSQL, sSQLName:=ACGetAgentSourceName, bStoredProcedure:=ACGetAgentSourceStored, vResultArray:=r_vResultArray, lNumberRecords:=lRecordCount)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartySourcesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartySources", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' This function will Update history of past used password
    ''' </summary>
    ''' <param name="iUser_Id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdatePasswordHistory(ByVal iUser_Id As Integer) As Integer
        Dim result As Integer = 0
        Dim iRecordsAffected As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(iUser_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLAction(sSQL:=ACPasswordHistoryAddSQL, sSQLName:=ACPasswordHistoryAddName, bStoredProcedure:=ACPasswordHistoryAddStored, lRecordsAffected:=iRecordsAffected)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch ex As Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateIncorrectAttemptsAndLockUnlock Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateIncorrectAttemptsAndLockUnlock", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return result
        End Try
    End Function




End Class
