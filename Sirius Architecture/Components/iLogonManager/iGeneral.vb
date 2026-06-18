Option Strict Off
Option Explicit On
Imports System.Text
Imports Artinsoft.VB6.Utils
'developer guide no.129
Imports SharedFiles

<Serializable()> _
Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: 03 July 1996
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' RFC 23/04/1998 - Amended to allow local or remote logon.
    ' RFC 16/10/1998 - Check Client & Server date formats.
    ' RFC 24/11/1998 - Added refresh of Status Bar after every set
    '                  of the Simple Text.
    ' RKS 13/12/2004 - Implementation of Unified Logon Process
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "General"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' RDC 17102000 for timebomb checking
    Private m_sTBTodayDate As String = ""
    Private m_sTBRunDate As String = ""
    Private m_sTBStartDate As String = ""
    Private m_sTBWarnDate As String = ""
    Private m_sTBExpiryDate As String = ""

    ' RDC 11072002
    Private m_bUnifiedLogon As Boolean
    Private m_sUnifiedLogonUsername As String = ""

    ' Local instance of the interface form.
    Private m_frmInterface As Form

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.4.1.1)
    Private m_sUserConfigXMLDataset As String = ""
    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.4.1.1)

    ' Licence Manager
    Private m_oLicenceManager As bPMLocalLicenceManager.LicenceManager

    Private m_oPasswordChangeDate As Object
    Private m_sOldPassword As String = ""
    Private m_sNewPassword As String = ""
    Private m_sConfirmPassword As String = ""
    Private m_sLanguageCaption As String = ""
    Private sFailureMessage As String = ""
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property ShowWarningessage As Boolean
    Public WriteOnly Property UnifiedLogon() As Boolean
        Set(ByVal Value As Boolean)
            m_bUnifiedLogon = Value
        End Set
    End Property

    Public WriteOnly Property UnifiedLogonUsername() As String
        Set(ByVal Value As String)
            m_sUnifiedLogonUsername = Value
        End Set
    End Property
    ' PUBLIC Property Procedures (End)
    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.4.1.1)
    Public Property UserConfigXMLDataSet() As String
        Get
            Return m_sUserConfigXMLDataset
        End Get
        Set(ByVal Value As String)
            m_sUserConfigXMLDataset = Value
        End Set
    End Property

    Public Property OldPassword() As String
        Get

            ' Set the OldPassword.
            Return m_sOldPassword

        End Get
        Set(ByVal Value As String)

            ' Set the OldPassword.
            m_sOldPassword = Value

        End Set
    End Property


    Public Property NewPassword() As String
        Get

            ' Set the NewPassword.
            Return m_sNewPassword

        End Get
        Set(ByVal Value As String)

            ' Set the NewPassword.
            m_sNewPassword = Value

        End Set
    End Property


    Public Property ConfirmPassword() As String
        Get
            ' Set the ConfirmPassword.
            Return m_sConfirmPassword
        End Get
        Set(ByVal Value As String)
            ' Set the ConfirmPassword.
            m_sConfirmPassword = Value
        End Set
    End Property


    Public Property LanguageCaption() As String
        Get

            ' Set the LanguageCaption
            Return m_sLanguageCaption

        End Get
        Set(ByVal Value As String)

            ' Set the LanguageCaption.
            m_sLanguageCaption = Value

        End Set
    End Property
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.4.1.1)


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
    '
    Public Function Initialise(ByRef frmInterface As frmInterface) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Store the instance of the form into the member.
            m_frmInterface = frmInterface

            ' Initialise LicenceManager
            m_oLicenceManager = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateUser(Optional ByRef vUserId As gPMConstants.PMEReturnCode = 0, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vPasswordChangeDate As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastLogin As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturnValue As Integer

        Try

            lReturnValue = g_oClientManager.UpdateUser(vUserId:=vUserId, vLanguageID:=vLanguageID, vUsername:=vUsername, vPassword:=vPassword, vPasswordChangeDate:=vPasswordChangeDate, vDateCreated:=vDateCreated, vLastLogin:=vLastLogin, vPartyCnt:=vPartyCnt, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate)

            result = lReturnValue

        Catch
        End Try



        ' Error Section.

        UpdateUser(gPMConstants.PMEReturnCode.PMError)

        ' Log Error.
        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
        oDict.Add("vUserId", vUserId)
        oDict.Add("vLanguageID", vLanguageID)
        oDict.Add("vPasswordChangeDate", vPasswordChangeDate)
        oDict.Add("vPartyCnt", vPartyCnt)
        oDict.Add("vEffectiveDate", vEffectiveDate)
        gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check logon via the client manager", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckLogon", oDicParms:=oDict)

        Return result

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
                m_frmInterface = Nothing
                If Not (m_oLicenceManager Is Nothing) Then
                    ' Terminate LicenceManager

                    m_oLicenceManager.Dispose()
                    m_oLicenceManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Logon
    '
    ' Description: Attempts to logon via the client manager using the
    '              username and password values.
    '
    ' ***************************************************************** '
    Public Function Logon() As Integer

        Dim result As Integer = 0
        Dim lErrorValue As gPMConstants.PMEReturnCode
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim vDummy As Object
        Dim sClientSystemName As String = ""
        Static iLogonAttempts As Integer
        Dim lRet As Integer
        ' RDC 18102000 for temporary licensing
        Dim lInstConfig As Integer
        Dim sMsg, sEncryptedStatus As String
        Dim msgResult As DialogResult
        Dim sPasswordLifecycleDays As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            'developer guide no. 69 (Guide)

            LogonManager.objFrmInterface._staStatus_Panel1.Text = "Establishing a connection..."
            LogonManager.objFrmInterface.staStatus.Refresh()

            ' Initialise the Logon Properties
            g_bPMBLinkRequired = False
            g_bLoggedOnToPMB = False
            g_sPMBCompanyNumber = ""

            ' Assign the form details to the
            ' public members.
            'modified as per  VB code
            'lErrorValue = m_frmInterface.FormToPublic()
            lErrorValue = ReflectionHelper.Invoke(m_frmInterface, "FormToPublic", New Object() {})
            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign form details to public", vApp:=ACApp, vClass:=ACClass, vMethod:="Logon")
            End If

            ' Check if we have an instance of Licence Manager
            If m_oLicenceManager Is Nothing Then
                ' Get the object reference for the
                ' client manager.
                lReturnValue = CType(GetLicenceManager(), gPMConstants.PMEReturnCode)
                If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMCancel
                End If
                ShowWarningessage = False
            End If

            ' Get the Client PC Name - RFC300398
            lErrorValue = CType(gPMFunctions.GetSystemName(sClientSystemName), gPMConstants.PMEReturnCode)

            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lErrorValue
            End If

            'developer guide no. 69 (Guide)
            LogonManager.objFrmInterface._staStatus_Panel1.Text = "Logging on to Sirius Architecture..."
            LogonManager.objFrmInterface.staStatus.Refresh()

            ' Call the logon method of the LicenceManager

            lReturnValue = m_oLicenceManager.LicenceManager_Logon(v_sUsername:=g_sUserName, v_sPassword:=g_sPassword, r_sClientSystemName:=sClientSystemName, r_bPMBLinkRequired:=g_bPMBLinkRequired, r_oClientManager:=g_oClientManager)

            ' RDC 19102000 get temporary licensing details
            ' Details used after other checks performed

            lInstConfig = m_oLicenceManager.GetTemporaryLicenceDetails(sEncryptedStatus)

            If lInstConfig <> gPMConstants.PMEReturnCode.PMTrue Then
                sEncryptedStatus = ""
            End If

            ' Terminate LicenceManager

            m_oLicenceManager.Dispose()
            m_oLicenceManager = Nothing

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'developer guide no. 69 (Guide)
            LogonManager.objFrmInterface._staStatus_Panel1.Text = ""
            LogonManager.objFrmInterface.staStatus.Refresh()

            If Not (g_oClientManager Is Nothing) Then
                ' Check logon return values.
                lErrorValue = CType(g_oClientManager.GetPropertyValues(sUsername:=g_sUserName, sPassword:=CStr(vDummy), iUserID:=g_iUserID, iSourceID:=g_iSourceID, iCountryID:=g_iCountryID, iLanguageId:=g_iLanguageID, iLogLevel:=g_iLogLevel, iCurrencyID:=g_iCurrencyID, lPartyCnt:=g_lPartyCnt, sCallingAppName:=CStr(vDummy), sServerPrinter:=g_sServerPrinter, iIsPrinterChangeable:=g_iIsPrinterChangeable, sUserConfigXMLDataSet:=m_sUserConfigXMLDataset, oPasswordChangeDate:=m_oPasswordChangeDate), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the property values from Client Manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Logon")
                End If

                lErrorValue = CType(bPMFunc.GetSystemOption(v_sUsername:=g_sUserName, v_sPassword:=g_sPassword, v_iUserID:=g_iUserID, v_iMainSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5113, r_sOptionValue:=sFailureMessage, v_iSourceID:=g_iSourceID), gPMConstants.PMEReturnCode)

                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            Select Case (lReturnValue)
                Case gPMConstants.PMEReturnCode.PMTrue

                    'RFC161098
                    ' Check the Client & Server are Compatible.

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                    'developer guide no. 69 (Guide)
                    LogonManager.objFrmInterface._staStatus_Panel1.Text = "Checking Client/Server Consistency..."
                    LogonManager.objFrmInterface.staStatus.Refresh()

                    lErrorValue = CType(ClientServerCompatible(), gPMConstants.PMEReturnCode)

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    'developer guide no. 69 (Guide)
                    LogonManager.objFrmInterface._staStatus_Panel1.Text = ""
                    LogonManager.objFrmInterface.staStatus.Refresh()


                    If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        Select Case lErrorValue
                            Case gPMConstants.PMEReturnCode.PMIncorrectDateFormat
                                MessageBox.Show("Unable to logon." & Strings.Chr(10).ToString() & _
                                                "The local machine date format does NOT match that of the server." & Strings.Chr(10).ToString() & _
                                                "Contact your System Administrator to resolve the problem." & Strings.Chr(10).ToString() & _
                                                "Logon will be Cancelled", "E0004 - Inconsistent Date Formats", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            Case gPMConstants.PMEReturnCode.PMIncorrectSystemDate
                                MessageBox.Show("Unable to logon." & Strings.Chr(10).ToString() & _
                                                "The local machine system date does NOT match that of the server." & Strings.Chr(10).ToString() & _
                                                "Contact your System Administrator to resolve the problem." & Strings.Chr(10).ToString() & _
                                                "Logon will be Cancelled", "E0005 - Inconsistent System Dates", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            Case Else
                                MessageBox.Show("Unable to logon." & Strings.Chr(10).ToString() & _
                                                "There is an inconsistency between the local machine and the server." & Strings.Chr(10).ToString() & _
                                                "Contact you system Administrator to resolve the problem." & Strings.Chr(10).ToString() & _
                                                "Logon will be Cancelled", "E0006 - Inconsistentency between Local Machine and Server", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        End Select
                        g_oClientManager = Nothing
                        Return gPMConstants.PMEReturnCode.PMCancel
                    End If

                    ' RFC100299 - Check the Sirius Architecture Installation
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                    'developer guide no. 69 (Guide)
                    LogonManager.objFrmInterface._staStatus_Panel1.Text = "Checking Client Version..."
                    LogonManager.objFrmInterface.staStatus.Refresh()

                    lErrorValue = CType(CheckClientInstallation(gPMConstants.PMEProductFamily.pmePFSiriusArchitecture), gPMConstants.PMEReturnCode)

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    'developer guide no. 69 (Guide)
                    LogonManager.objFrmInterface._staStatus_Panel1.Text = ""
                    LogonManager.objFrmInterface.staStatus.Refresh()

                    If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        g_oClientManager = Nothing
                        Return gPMConstants.PMEReturnCode.PMCancel
                    End If

                    ' RDC 17102000 - temporary licensing - START
                    lInstConfig = 0

                    lReturnValue = CType(CheckInstallationConfig(lInstConfig, sEncryptedStatus), gPMConstants.PMEReturnCode)

                    Select Case lInstConfig
                        Case TBStatusUnrestricted, TBStatusNoWarning
                            ' no worries, come on in!
                        Case TBStatusRestricted
                            ' registry setting does not exist, so only allow sirius/sirius login
                            If g_sUserName.ToUpper() <> "SIRIUS" Then
                                ' yer name's not down, etc
                                sMsg = "Warning: Access restricted to 'sirius' ID as installation config is missing."
                                lReturnValue = CType(LogTimebombMessage(sMsg, MsgBoxStyle.Critical), gPMConstants.PMEReturnCode)

                                g_oClientManager = Nothing
                                Return gPMConstants.PMEReturnCode.PMCancel
                            End If
                        Case TBStatusWarning
                            ' into grace period. Message everything
                            sMsg = "Warning: Temporary licence expires on " & m_sTBExpiryDate & "."
                            lReturnValue = CType(LogTimebombMessage(sMsg, MsgBoxStyle.Exclamation), gPMConstants.PMEReturnCode)

                        Case Else
                            ' expiry date must be set to something
                            If m_sTBExpiryDate = "" Then
                                m_sTBExpiryDate = DateTimeHelper.ToString(DateTime.Now.AddDays(1))
                            End If

                            If m_sTBStartDate = "" Then
                                m_sTBStartDate = DateTime.Now.ToString("dd-MMM-yyyy")
                            End If

                            If DateTime.Now < CDate(m_sTBStartDate) Then
                                'if today date before start date, UAT has not started
                                sMsg = "Warning: Temporary licensing does not start until " & m_sTBStartDate & "."
                                lReturnValue = CType(LogTimebombMessage(sMsg, MsgBoxStyle.Critical), gPMConstants.PMEReturnCode)

                            ElseIf CDate(m_sTBExpiryDate) < DateTime.Now Then
                                ' current date beyond expiry date
                                sMsg = "Warning: Temporary licensing period expired on " & m_sTBExpiryDate & "."
                                lReturnValue = CType(LogTimebombMessage(sMsg, MsgBoxStyle.Critical), gPMConstants.PMEReturnCode)

                            Else
                                ' someone's been screwing with the registry setting
                                sMsg = "Warning: Invalid installation configuration settings. System will terminate."
                                lReturnValue = CType(LogTimebombMessage(sMsg, MsgBoxStyle.Critical), gPMConstants.PMEReturnCode)
                            End If

                            g_oClientManager = Nothing
                            Return gPMConstants.PMEReturnCode.PMCancel
                    End Select
                    ' RDC 17102000 Timebomb code - END

                    ' Get the language ID & Log Level from the Client Manager

                    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.4.1.1)

                    lErrorValue = CType(g_oClientManager.GetPropertyValues(sUsername:=g_sUserName, sPassword:=CStr(vDummy), iUserID:=g_iUserID, iSourceID:=g_iSourceID, iCountryID:=g_iCountryID, iLanguageId:=g_iLanguageID, iLogLevel:=g_iLogLevel, iCurrencyID:=g_iCurrencyID, lPartyCnt:=g_lPartyCnt, sCallingAppName:=CStr(vDummy), sServerPrinter:=g_sServerPrinter, iIsPrinterChangeable:=g_iIsPrinterChangeable, sUserConfigXMLDataSet:=m_sUserConfigXMLDataset, oPasswordChangeDate:=m_oPasswordChangeDate), gPMConstants.PMEReturnCode)
                    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.4.1.1)

                    ' Check for errors.
                    If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error.
                        gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the property values from Client Manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Logon")
                    End If



                    ' RFC270398
                Case gPMConstants.PMEReturnCode.PMInvalidLicenceKey
                    ' No Licence At All
                    MessageBox.Show("Unable to logon." & Strings.Chr(10).ToString() & _
                                    "Product is not correctly Licenced. Contact Policy Master Support." & Strings.Chr(10).ToString() & _
                                    "Logon will be Cancelled", "E0001 - No Product Licence Found", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Case gPMConstants.PMEReturnCode.PMLicenceExceeded
                    ' Licence limit has been exceeded.
                    MessageBox.Show("Unable to logon." & Strings.Chr(10).ToString() & _
                                    "The licence limit has been exceeded. Please try later.", "E0007 - Licence Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Case gPMConstants.PMEReturnCode.PMLoggedOnElsewhere
                    ' User is already logged on at another client
                    MessageBox.Show("You are already logged on at " & sClientSystemName & Strings.Chr(10).ToString() & _
                                    "You must logoff from that computer before you can logon here." & Strings.Chr(13) & Strings.Chr(10) & _
                                    "If due to an error, you are not logged on at another machine, " & Strings.Chr(13) & Strings.Chr(10) & _
                                    "use the server application 'Licence Administrator' to reset your user account.", "E0008 - Already Logged On", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Case gPMConstants.PMEReturnCode.PMIncorrectUsername
                    ' RDC 11072002
                    ' Logon has failed with current username.
                    MessageBox.Show("Username not found." & Strings.Chr(10).ToString() & _
                                    "Please check your username and try again", "E0009 - Username not found.", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Clear the form details.


                    'developer guide no. 37 (Guide)
                    lErrorValue = LogonManager.objFrmInterface.ClearForm(gPMConstants.PMEReturnCode.PMIncorrectUsername)

                    ' Increment the number of logon attempts
                    iLogonAttempts += 1

                Case gPMConstants.PMEReturnCode.PMIncorrectPassword
                    ' Logon has failed with current password.
                    If Not m_bUnifiedLogon Then
                        MessageBox.Show("Password is incorrect." & Strings.Chr(10).ToString() & _
                                        "Please check your password and try again", "E0010 - Incorrect Password", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' Clear the form details.


                        ' developer guide no. 37
                        ' lErrorValue = LogonManager.objFrmInterface.ClearForm(gPMConstants.PMEReturnCode.PMIncorrectUsername)
                        lErrorValue = LogonManager.objFrmInterface.ClearForm(gPMConstants.PMEReturnCode.PMIncorrectPassword)
                        ' Increment the number of logon attempts
                        iLogonAttempts += 1
                    End If

                Case gPMConstants.PMEReturnCode.PMMixedModeIncorrectUserName
                    '            MsgBox "Username '" & m_sUnifiedLogonUsername & "' not found." & Chr(10) & _
                    ''            "Please contact your system administrator", _
                    ''            vbInformation, "E0009 - Username not found."

                Case gPMConstants.PMEReturnCode.PMMixedModeUserLoggedOnElsewhere
                    MessageBox.Show("'" & m_sUnifiedLogonUsername & "' is already logged on at " & sClientSystemName & Strings.Chr(10).ToString() & _
                                    "You must logoff from that computer before you can logon here." & Strings.Chr(13) & Strings.Chr(10) & _
                                    "If due to an error, you are not logged on at another machine, " & Strings.Chr(13) & Strings.Chr(10) & _
                                    "use the server application 'Licence Administrator' to reset your user account.", "E0008 - Already Logged On", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Case gPMConstants.PMEReturnCode.PMUnifiedModeIncorrectUserName
                    MessageBox.Show("Username '" & m_sUnifiedLogonUsername & "' not found." & Strings.Chr(10).ToString() & _
                                    "Please contact your system administrator", "E0009 - Username not found.", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Case gPMConstants.PMEReturnCode.PMUnifiedModeUserLoggedOnElsewhere
                    MessageBox.Show("'" & m_sUnifiedLogonUsername & "' is already logged on at " & sClientSystemName & Strings.Chr(10).ToString() & _
                                    "You must logoff from that computer before you can logon here." & Strings.Chr(13) & Strings.Chr(10) & _
                                    "If due to an error, you are not logged on at another machine, " & Strings.Chr(13) & Strings.Chr(10) & _
                                    "use the server application 'Licence Administrator' to reset your user account.", "E0008 - Already Logged On", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Case gPMConstants.PMEReturnCode.PMUserAccountLocked
                    MessageBox.Show("Your Account has been locked. Please contact your System Administrator.", "User Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    lErrorValue = LogonManager.objFrmInterface.ClearForm(gPMConstants.PMEReturnCode.PMIncorrectUsername)
                    lReturnValue = gPMConstants.PMEReturnCode.PMUserAccountLocked
                Case gPMConstants.PMEReturnCode.PMLogonExceeded
                    MessageBox.Show("Logon attempts exceeded." & Strings.Chr(10).ToString() & "Invalid Password. Your Account has been locked. Please contact your system administrator for your current logon", "E0011 - Logon Attempts Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    lErrorValue = LogonManager.objFrmInterface.ClearForm(gPMConstants.PMEReturnCode.PMIncorrectUsername)
                    lReturnValue = gPMConstants.PMEReturnCode.PMUserAccountLocked
                Case gPMConstants.PMEReturnCode.PMUserPasswordExpired
                    MessageBox.Show("Your password has expired - please change your password now. If you have any questions regarding changing passwords, please contact your System Administrator.", "User Password Expired", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    lErrorValue = LogonManager.objFrmInterface.ClearForm(gPMConstants.PMEReturnCode.PMIncorrectUsername)
                    lReturnValue = gPMConstants.PMEReturnCode.PMUserForceChangePassword
                Case gPMConstants.PMEReturnCode.PMUserTemporaryPassword
                    MessageBox.Show("Currently you are logged-in with temporary password - please change your password now. If you have any questions regarding changing passwords, please contact your System Administrator.", "Logged-In with Temporary Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    lErrorValue = LogonManager.objFrmInterface.ClearForm(gPMConstants.PMEReturnCode.PMIncorrectUsername)
                    lReturnValue = gPMConstants.PMEReturnCode.PMUserForceChangePassword
                Case gPMConstants.PMEReturnCode.PMUserWeakPassword
                    MessageBox.Show(sFailureMessage, "Logged-In with Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    lErrorValue = LogonManager.objFrmInterface.ClearForm(gPMConstants.PMEReturnCode.PMIncorrectUsername)
                    lReturnValue = gPMConstants.PMEReturnCode.PMUserForceChangePassword
                Case gPMConstants.PMEReturnCode.PMNewBuildUpgrade
                    MessageBox.Show("Due to new password functionality need to update password - please change your password now. If you have any questions regarding changing passwords, please contact your System Administrator.", "User Password Change Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    lErrorValue = LogonManager.objFrmInterface.ClearForm(gPMConstants.PMEReturnCode.PMIncorrectUsername)
                    lReturnValue = gPMConstants.PMEReturnCode.PMUserForceChangePassword
                Case Else
                    ' An Unknown error has occurred.

                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to logon.", vApp:=ACApp, vClass:=ACClass, vMethod:="Logon")

                    ' Destroy the instance of the client manager
                    ' from memory.
                    g_oClientManager = Nothing
                    Return gPMConstants.PMEReturnCode.PMCancel

            End Select
            'Getting the "System Security Model" i.e. (AlternativeLogon option)
            Dim sRetval As String = String.Empty
            lErrorValue = CType(bPMFunc.GetSystemSecurityModel(sRetval), gPMConstants.PMEReturnCode)
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Retrieve System option for Password Expiry Warning Message
            If g_iUserID <> 0 And lReturnValue <> gPMConstants.PMEReturnCode.PMUserForceChangePassword And sRetval <> "1" Then
                Dim sPasswordExpiryWarningDays As String
                lErrorValue = CType(bPMFunc.GetSystemOption(v_sUsername:=g_sUserName, v_sPassword:=g_sPassword, v_iUserID:=g_iUserID, v_iMainSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5111, r_sOptionValue:=sPasswordExpiryWarningDays, v_iSourceID:=g_iSourceID), gPMConstants.PMEReturnCode)
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                lErrorValue = CType(bPMFunc.GetSystemOption(v_sUsername:=g_sUserName, v_sPassword:=g_sPassword, v_iUserID:=g_iUserID, v_iMainSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=5103, r_sOptionValue:=sPasswordLifecycleDays, v_iSourceID:=g_iSourceID), gPMConstants.PMEReturnCode)
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If sPasswordExpiryWarningDays <> 0 AndAlso DateAdd("d", ToSafeInteger(sPasswordExpiryWarningDays), Date.Today) >= DateAdd("d", ToSafeInteger(sPasswordLifecycleDays), DateTime.Parse(m_oPasswordChangeDate).Date) Then
                    msgResult = MessageBox.Show(String.Format("Your password is due to expire in {0}  days. Please consider changing your password", DateDiff(DateInterval.Day, Date.Today, DateAdd("d", ToSafeInteger(sPasswordLifecycleDays), m_oPasswordChangeDate))), "Password Expiry Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                End If
                If msgResult = System.Windows.Forms.DialogResult.Yes Then
                    lReturnValue = gPMConstants.PMEReturnCode.PMUserForceChangePassword
                End If
            End If

            Select Case lReturnValue
                ' OK
                Case gPMConstants.PMEReturnCode.PMTrue

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Known Fatal Errors
                Case gPMConstants.PMEReturnCode.PMInvalidLicenceKey, gPMConstants.PMEReturnCode.PMLicenceExceeded, gPMConstants.PMEReturnCode.PMLoggedOnElsewhere, gPMConstants.PMEReturnCode.PMLogonExceeded
                    g_oClientManager = Nothing
                    ' RFC18081998 Return Cancel for all of the above
                    lReturnValue = gPMConstants.PMEReturnCode.PMCancel

                    ' Try Again Errors
                Case gPMConstants.PMEReturnCode.PMIncorrectUsername, gPMConstants.PMEReturnCode.PMIncorrectPassword, gPMConstants.PMEReturnCode.PMUserAccountLocked
                    g_oClientManager = Nothing

                    ' Other Fatal Errors
                Case Else

            End Select

            ' Return logon value.

            Return lReturnValue

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Destroy the instance of the client manager
            ' from memory.
            g_oClientManager = Nothing

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to logon via the client manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Logon", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckLogon
    '
    ' Description: Checks logon and returns the result.
    '
    ' ***************************************************************** '
    Public Function CheckLogon(ByRef sUsername As String, ByRef sPassword As String) As Integer

        Dim result As Integer = 0
        Dim lReturnValue As Integer

        Try

            If g_oClientManager Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Call the logon method of the client manager.
                lReturnValue = g_oClientManager.Logon(sUsername, sPassword)
            End If

            ' Return the result.

            Return lReturnValue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check logon via the client manager", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckLogon", excep:=excep)

            Return result

        End Try
    End Function


    'PUBLIC Methods (End)

    'PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: CreateStatusManager
    '
    ' Description: Creates a reference of the logon status manager.
    '
    ' ***************************************************************** '
    Private Function CreateStatusManager() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create an instance of the public logon status manager.
        '    g_oStatusManager = New iLogonStatusManager.LogonStatusManager()

        '    ' Call the initialise method.
        '    'developer Guide no.97
        '    lErrorValue = g_oStatusManager.Initialise()
        '    ' Check for errors.
        '    If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
        '        ' Failed to create an instance of the client
        '        ' status manager.
        '        result = gPMConstants.PMEReturnCode.PMFalse

        '        ' Set the public instance to nothing.
        '        g_oStatusManager = Nothing

        '        Return result
        '    End If

        '    '    With g_oStatusManager
        '    ' Assign me to the status manager parent
        '    ' property, inorder for the status manager
        '    ' to stay active with the logon manager.
        '    '        .Parent = Me
        '    '    End With

        '    ' Call the start method.
        '    lErrorValue = g_oStatusManager.Start()
        '    ' Check for errors.
        '    If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
        '        ' Failed to start the logon status manager.
        '        result = gPMConstants.PMEReturnCode.PMFalse

        '        ' Set the public instance to nothing.
        '        g_oStatusManager = Nothing

        '        Return result
        '    End If

        Return result




        '    ' Error Section.

        '    result = gPMConstants.PMEReturnCode.PMError

        '    ' Set the public instance to nothing.
        '    g_oStatusManager = Nothing

        '    ' Log Error.
        '    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of the logon status manager", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStatusManager", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)



    End Function

    ' ***************************************************************** '
    ' Name: GetLicenceManager
    '
    ' Description: Gets a reference to licence manager.
    '
    ' ***************************************************************** '
    Private Function GetLicenceManager() As Integer
        Dim result As Integer = 0
        Dim lVBErrNo As Integer
        Dim sVBErrDesc, sErrTitle As String
        Dim sTrusted As String = String.Empty
        Dim sbErrMesg As StringBuilder
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Format the Error Message incase we need it.
            sbErrMesg = New StringBuilder("")
            sbErrMesg.Append("Unable to logon." & Strings.Chr(10).ToString() & _
                       "Failed to establish a Connection to the")

            sbErrMesg.Append(" LOCAL ")
            sErrTitle = "E0002 - LOCAL Logon Connection Error"
            sbErrMesg.Append("Licence Manager")

            ' Create/Get a instance of the licence manager.
            m_oLicenceManager = New bPMLocalLicenceManager.LicenceManager()

            ' Initialise
            result = m_oLicenceManager.LicenceManager_Initialise()
            If ShowWarningessage = True AndAlso result = PMEReturnCode.PMTrue AndAlso Not String.IsNullOrEmpty(m_oLicenceManager.WarningMessage) Then
                MessageBox.Show(m_oLicenceManager.WarningMessage.ToString, "License Expiration Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            'result = m_oLicenceManager.
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                If result = gPMConstants.PMEReturnCode.PMInvalidLicenceKey Then
                    MessageBox.Show("Unable to logon." & Strings.Chr(10).ToString() & _
                                    "Product is not correctly Licenced. Contact SSP Pure Support." & Strings.Chr(10).ToString() & _
                                    "Logon will be Cancelled", "E0001 - No Product Licence Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMCancel
                ElseIf (result = gPMConstants.PMEReturnCode.PMLicenceExceeded) Then  '32125
                    ' Licence limit has been exceeded.
                    MessageBox.Show("Unable to logon." & Strings.Chr(10).ToString() & _
                                    "The licence limit has been exceeded. Please try later.", "E0007 - Licence Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    result = gPMConstants.PMEReturnCode.PMCancel
                Else
                    'Get the Trusted Registry settings
                    result = GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, _
                                      v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, _
                                      v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, _
                                      v_sSettingName:="Trusted", _
                                      r_sSettingValue:=sTrusted, _
                                      v_sSubKey:="Databases\Pure")
                    If sTrusted = "1" Then
                        MessageBox.Show(sbErrMesg.ToString, sErrTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        result = gPMConstants.PMEReturnCode.PMCancel
                    Else
                        sbErrMesg.Append(Strings.Chr(10).ToString())
                        sbErrMesg.Append("Would you like to configure the SQL Login Id and Password?")

                        If MessageBox.Show(sbErrMesg.ToString, sErrTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Error) = DialogResult.Yes Then
                            'Allow the User to To Change the User ID and Password
                            Dim ofrmSQLCredentials As frmSQLCredentials = New frmSQLCredentials
                            ofrmSQLCredentials.ShowDialog()
                            result = ofrmSQLCredentials.ReturnStatus

                            'Initalize the LicenceManager Again
                            m_oLicenceManager.Dispose()
                            m_oLicenceManager = New bPMLocalLicenceManager.LicenceManager()
                            result = m_oLicenceManager.LicenceManager_Initialise()

                        End If
                    End If
                End If
                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMCancel

            ' Set the return object to nothing.
            m_oLicenceManager = Nothing

            lVBErrNo = Information.Err().Number
            sVBErrDesc = excep.Message

            sbErrMesg.Append(Strings.Chr(10).ToString() & _
                       "Logon will be Cancelled" & Strings.Chr(10).ToString() & _
                       "Error Number: " & CStr(lVBErrNo) & Strings.Chr(10).ToString() & _
                       "Errror Description: " & sVBErrDesc)

            ' Display the Message to the User
            MessageBox.Show(sbErrMesg.ToString, sErrTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

            ' Log the Message
            gPMFunctions.LogMessageToFile(sUsername:=g_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sbErrMesg.ToString, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLicenceManager", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ArchitectureLocalEnabled
    '
    ' Description: Returns True if the Architecture can run locally,
    '              False otherwise.
    '
    ' ***************************************************************** '
    Public Function ArchitectureLocalEnabled() As Boolean

        Dim result As Boolean = False
        Dim sLocalEnabled As String = ""
        Dim lErrorValue As Integer

        Try

            '    ' Get the log file name from the registry
            '    lErrorValue& = GetRegSettings( _
            ''        sResult:=sLocalEnabled, _
            ''        sAppName:=PMRegAppName, _
            ''        sSection:=PMRegSecSystem, _
            ''        sKey:=PMRegKeyArchitectureLocalEnabled)

            ' Get the ArchitectureLocal Enabled setting in
            ' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Client
            lErrorValue = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyArchitectureLocalEnabled, r_sSettingValue:=sLocalEnabled)

            ' Check for errors.
            If (lErrorValue <> gPMConstants.PMEReturnCode.PMTrue) Or (sLocalEnabled.Trim() = "") Then
                ' Return False if Errors
                Return False
            End If


            Select Case sLocalEnabled.Trim().ToUpper()
                Case CStr(gPMConstants.PMEReturnCode.PMTrue)
                    Return True
                Case Else
                    Return False
            End Select

        Catch
        End Try



        Return False

    End Function

    ' ***************************************************************** '
    ' Name: ArchitectureServerEnabled
    '
    ' Description: Returns True if the Architecture can run client/server,
    '              False otherwise.
    '
    ' ***************************************************************** '
    Public Function ArchitectureServerEnabled() As Boolean

        Dim result As Boolean = False
        Dim sServerEnabled As String = ""
        Dim lErrorValue As Integer

        Try

            result = True

            ' Get the ArchitectureLocal Enabled setting in
            ' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Client
            lErrorValue = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyArchitectureServerEnabled, r_sSettingValue:=sServerEnabled)

            ' Check for errors.
            If (lErrorValue <> gPMConstants.PMEReturnCode.PMTrue) Or (sServerEnabled.Trim() = "") Then
                ' If the setting is not there we assume it is enabled
                Return result
            End If

            ' Return result PMTrue = True

            Select Case sServerEnabled.Trim().ToUpper()
                Case CStr(gPMConstants.PMEReturnCode.PMTrue)
                    Return True
                Case Else
                    Return False
            End Select

        Catch
        End Try



        Return False

    End Function

    ' ***************************************************************** '
    ' Policy Master Broking Methods
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: PMBLogon
    '
    ' Description: Logs on to PM Broking via Client Manager
    '
    ' ***************************************************************** '

    'Private Function PMBLogon() As Integer
    '
    'Dim result As Integer = 0
    'Dim lReturnCode As gPMConstants.PMEReturnCode
    'Dim iNoOfCompanies As Integer
    'Dim lReturnValue As gPMConstants.PMEReturnCode
    'Dim sError As String = ""
    'Dim msgResult As DialogResult
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
    '
    'frmInterface.staStatus.Text = "Logging on to PM Broking..."
    'frmInterface.staStatus.Refresh()
    '
    ' Attempt to logon to PMB
    'lReturnCode = CType(g_oClientManager.PMBLogon(r_lReturnValue:=lReturnValue, r_iNoOfCompanies:=CShort(iNoOfCompanies)), gPMConstants.PMEReturnCode)
    '
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    '
    'frmInterface.staStatus.Text = ""
    'frmInterface.staStatus.Refresh()
    '
    'Select Case lReturnCode
    'Case gPMConstants.PMEReturnCode.PMTrue
    'lReturnValue = gPMConstants.PMEReturnCode.PMTrue
    ' We are successfully logged on to PM Broking
    'g_bLoggedOnToPMB = True
    'Case gPMConstants.PMEReturnCode.PMCancel
    'lReturnValue = gPMConstants.PMEReturnCode.PMCancel
    'Case Else
    'Select Case lReturnValue
    'Case gPMConstants.PMEReturnCode.PMMNoAuthority
    'sError = "User has no authority to use PM Broking."
    'Case gPMConstants.PMEReturnCode.PMMAlreadyInUse
    'sError = "PM user id already in use."
    'Case gPMConstants.PMEReturnCode.PMMInvalidPassword
    'sError = "Invalid Password."
    'Case gPMConstants.PMEReturnCode.PMMNoAccess
    'sError = "No access to any PM company."
    'Case 56
    'sError = "No access to PM company requested."
    'Case 57
    'sError = "Company control file missing or corrupt."
    'Case 58
    'sError = "Options 49 or 50 not set and not system user."
    'Case 59
    'sError = "Invalid company number."
    'Case 60
    'sError = "LCS activity file busy."
    'Case 61
    'sError = "Too many users."
    'Case 62
    'sError = "No product licence."
    'Case 63
    'sError = "Unrecognised program name."
    'Case 64
    'sError = "Evaluation licence expired."
    'Case 65
    'sError = "Unable to update licence details."
    'Case 66
    'sError = "LCS activity file read failure."
    'Case 67
    'sError = "Mismatch in product index."
    'Case 68
    'sError = "Attempt to set company when already logged on."
    'Case gPMConstants.PMEReturnCode.PMNoHostRegistry, gPMConstants.PMEReturnCode.PMNoPortRegistry
    'sError = "Host and Port settings are Not correctly setup on the Server."
    'Case gPMConstants.PMEReturnCode.PMNoConnection
    'sError = "Could not create a connection to PM Broking."
    'Case gPMConstants.PMEReturnCode.PMNoPMLink
    'sError = "Could not create PMLink."
    'Case gPMConstants.PMEReturnCode.PMNoCompanies
    'sError = "There are no valid companies."
    'Case Else
    'sError = "Unknown error :" & lReturnValue & "."
    'End Select
    '
    'msgResult = MessageBox.Show("Error logging into Broking." & Strings.Chr(13) & Strings.Chr(10) &  _
    '            sError & Strings.Chr(13) & Strings.Chr(10) &  _
    '            "Do you wish to continue without the link?", "E0014 - Broking logon error", MessageBoxButtons.YesNo)
    '
    'Select Case msgResult
    'Case System.Windows.Forms.DialogResult.Yes
    'lReturnValue = gPMConstants.PMEReturnCode.PMTrue
    'Case System.Windows.Forms.DialogResult.No
    'lReturnValue = gPMConstants.PMEReturnCode.PMCancel
    'End Select
    'End Select
    '
    ' If all is ok, Select the Broking Company
    'If lReturnValue = gPMConstants.PMEReturnCode.PMTrue Then
    ' Note: If we are logging in locally we can still get
    ' the companies as they will be
    ' available from the local Sirius architecture d/b.
    'lReturnCode = CType(PMBSelectCompany(), gPMConstants.PMEReturnCode)
    '
    'If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
    'If lReturnCode = gPMConstants.PMEReturnCode.PMCancel Then
    'Return gPMConstants.PMEReturnCode.PMCancel
    'Else
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '
    'End If
    '
    ' Return the results
    '
    'Return lReturnValue
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to logon to PMB", vApp:=ACApp, vClass:=ACClass, vMethod:="PMBLogon", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: PMBSelectCompany
    '
    ' Description: Lets the user select the PMB Company
    '
    ' ***************************************************************** '
    Private Function PMBSelectCompany() As Integer

        Dim result As Integer = 0
        'Modified by Archana Tokas on 5/11/2010 10:55:11 AM it was for test which is not required now
        'Dim oGenericCompanyScreen As iPMBrokingLogonScreens.SelectCompany
        Dim oGenericCompanyScreen As Object
        Dim lReturnCode As gPMConstants.PMEReturnCode
        Dim vCompanyID As Object
        Dim vCompanyDesc As Object
        Dim sCompanyNumber As String = ""
        Dim lReturn As Integer
        Dim sErrorMessage, sErrorTitle As String



        result = gPMConstants.PMEReturnCode.PMTrue

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Format Error Message in case we need it below
        sErrorMessage = "Unable to get list of valid Companies from "
        If g_bLoggedOnToPMB Then
            sErrorMessage = sErrorMessage & "Broking."
            sErrorTitle = "E0012 - Broking Company Selection Error"
        Else
            sErrorMessage = sErrorMessage & "Sirius Architecture database."
            sErrorTitle = "E0013 - Broking Company Selection Error"
        End If
        sErrorMessage = sErrorMessage & Strings.Chr(13) & Strings.Chr(10) & "Logon will be cancelled."

        ' Do we already have a selected PMB Company
        lReturnCode = CType(gPMFunctions.GetRegSettings(sResult:=sCompanyNumber, sAppName:=gPMConstants.PMProduct, sSection:="Command", sKey:="Company"), gPMConstants.PMEReturnCode)

        ' Yes, so exit
        If sCompanyNumber.Trim() <> "" Then
            'There's a possible conflict here if the company ain't on the
            'list.  For now, assume that it is...
            Return result
        End If

        'developer guide no. 69 (Guide)
        LogonManager.objFrmInterface._staStatus_Panel1.Text = "Getting list of valid companies..."
        LogonManager.objFrmInterface.staStatus.Refresh()

        ' Get the list of valid companies
        lReturnCode = CType(g_oClientManager.PMBGetValidCompanies(r_vValidCompanies:=vCompanyID, r_vValidDescriptions:=vCompanyDesc), gPMConstants.PMEReturnCode)

        If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
            vCompanyID = Nothing
            vCompanyDesc = Nothing
            MessageBox.Show(sErrorMessage, sErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return gPMConstants.PMEReturnCode.PMCancel
        End If

        If Not Information.IsArray(vCompanyID) Then
            vCompanyID = Nothing
            vCompanyDesc = Nothing
            MessageBox.Show(sErrorMessage, sErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return gPMConstants.PMEReturnCode.PMCancel
        End If

        ' RFC18091998 Auto select company if there is only one.
        ' Is there more than one company.
        ' Note: This array is one (1) based.

        If vCompanyID.GetUpperBound(0) > 1 Then

            'developer guide no. 69 (Guide)
            LogonManager.objFrmInterface._staStatus_Panel1.Text = "Select required company..."
            LogonManager.objFrmInterface.staStatus.Refresh()

            ' Create the Selection Component
            'Modified by Archana Tokas on 5/11/2010 10:55:11 AM it was for test which is not required now
            'oGenericCompanyScreen = New iPMBrokingLogonScreens.SelectCompany()
            oGenericCompanyScreen = New Object()
            ' Initialise
            lReturnCode = CType(oGenericCompanyScreen.Initialise(vCompanyID:=vCompanyID, vCompanyDesc:=vCompanyDesc), gPMConstants.PMEReturnCode)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Get the selected company
            lReturnCode = CType(oGenericCompanyScreen.GetCompany(sCompany:=sCompanyNumber), gPMConstants.PMEReturnCode)

            ' Terminate
            oGenericCompanyScreen.Dispose()
            oGenericCompanyScreen = Nothing

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Else
            ' There is only one so get it directly from the array.

            sCompanyNumber = CStr(vCompanyID(1))
        End If
        'developer guide no. 69 (Guide)
        LogonManager.objFrmInterface._staStatus_Panel1.Text = "Initialising company (" & sCompanyNumber.Trim() & ")..."
        LogonManager.objFrmInterface.staStatus.Refresh()

        vCompanyID = Nothing
        vCompanyDesc = Nothing

        ' Set the PMB Company
        lReturnCode = CType(g_oClientManager.PMBSetCompany(v_sCompany:=sCompanyNumber), gPMConstants.PMEReturnCode)

        If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Set the global PMB Company
        g_sPMBCompanyNumber = sCompanyNumber

        'developer guide no. 69 (Guide)
        LogonManager.objFrmInterface._staStatus_Panel1.Text = ""
        LogonManager.objFrmInterface.staStatus.Refresh()
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Return result

    End Function

    'RFC161098
    ' ***************************************************************** '
    ' Name: ClientServerCompatible
    '
    ' Description: Checks to see if the Client and Server are
    '              are compatible. .e.g. Date formats are the same etc.
    '
    ' ***************************************************************** '
    Private Function ClientServerCompatible() As Integer
        Dim result As Integer = 0
        Dim sTestDateString As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If we do not have Client Manager then cannot do the test.
        If g_oClientManager Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Note: Deliberately use a 2 digit year to check that
        ' both Client & Server will resolve the 4 digit year in the
        ' same way.
        sTestDateString = "01/12/29"

        ' Check that the date formats and system dates are the same.

        Return g_oClientManager.CheckClientDateFormat(v_sDateString:=sTestDateString, v_lClientDay:=DateAndTime.Day(CDate(sTestDateString)), v_lClientMonth:=CDate(sTestDateString).Month, v_lClientYear:=CDate(sTestDateString).Year, v_dtClientSystemDate:=DateTime.Now)

    End Function

    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface general class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' RDC 19102000 Temporary licensing START
    Private Function CheckInstallationConfig(ByRef lStatus As Integer, ByRef sEncryptedStatus As String) As Integer

        Dim result As Integer = 0
        Dim iPos1, iPos2, iPos3 As Integer
        Dim sStatus As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lStatus = 0

            If sEncryptedStatus = "" Then
                ' registry setting is missing
                lStatus = TBStatusRestricted
                Return result
            Else
                ' should contain four dates
                sStatus = TBDecrypt(sEncryptedStatus)
            End If

            ' get separators
            iPos1 = (sStatus.IndexOf(","c) + 1)
            iPos2 = Strings.InStr(iPos1 + 1, sStatus, ",")
            iPos3 = Strings.InStr(iPos2 + 1, sStatus, ",")

            ' check separators
            If iPos1 = 0 Or iPos2 = 0 Or iPos3 = 0 Then
                lStatus = TBStatusNoAccess
                Return result
            End If

            ' get dates
            m_sTBTodayDate = DateTime.Now.ToString("dd-MMM-yyyy")
            m_sTBRunDate = sStatus.Substring(0, iPos1 - 1)
            m_sTBStartDate = Mid(sStatus, iPos1 + 1, iPos2 - iPos1 - 1)
            m_sTBWarnDate = Mid(sStatus, iPos2 + 1, iPos3 - iPos2 - 1)
            m_sTBExpiryDate = Mid(sStatus, iPos3 + 1)

            ' someone has tampered with the registry setting
            If Not Information.IsDate(m_sTBRunDate) Or Not Information.IsDate(m_sTBStartDate) Or Not Information.IsDate(m_sTBWarnDate) Or Not Information.IsDate(m_sTBExpiryDate) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lStatus = TBStatusNoAccess
                Return result
            End If

            ' dates are the same - no timebomb
            If CDate(m_sTBRunDate) = CDate(m_sTBExpiryDate) Then
                lStatus = TBStatusUnrestricted
                Return result
            End If

            ' current date before start date - no action
            If CDate(m_sTBTodayDate) < CDate(m_sTBStartDate) Then
                lStatus = TBStatusNotStarted
                Return result
            End If
            ' today is greater than expiry date - no access
            If CDate(m_sTBTodayDate) >= CDate(m_sTBExpiryDate) Then
                lStatus = TBStatusNoAccess
                Return result
            End If

            ' today is greater than date when warnings start
            If CDate(m_sTBTodayDate) >= CDate(m_sTBWarnDate) Then
                lStatus = TBStatusWarning
                Return result
            End If

            ' today is after date when countdown starts
            If CDate(m_sTBTodayDate) >= CDate(m_sTBStartDate) Then
                lStatus = TBStatusNoWarning
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMFalse

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' RDC 19102000 used by temporary licensing
    Public Function TBDecrypt(ByVal sInput As String) As String


        Dim iRnd As Integer = CInt("&H" & Mid(sInput, 1, 2))
        Dim iMin As Integer = CInt("&H" & Mid(sInput, 3, 2))
        Dim iLen As Integer = CInt("&H" & Mid(sInput, 5, 2))

        Dim sTemp As String = Upgrade(Mid(sInput, 7), iMin)

        Return RollLeft(ConvertFromHex(RollLeft(sTemp, iRnd)), iRnd).Substring(0, iLen)

    End Function

    ' RDC 19102000 Temporary licensing
    Private Function ShortHex(ByVal iInput As Integer, ByVal iWidth As Integer) As String

        Return (New String("0", iWidth) & iInput.ToString("X")).Substring((New String("0", iWidth) & iInput.ToString("X")).Length - iWidth)

    End Function

    ' RDC 19102000 Temporary licensing

    'Private Function Degrade(ByVal sInput As String, ByRef iMin As Integer) As String
    '
    'Dim iValue As Integer
    '
    'iMin = 255
    '
    'For 'iLoop As Integer = 1 To sInput.Length - 1 Step 2
    'iValue = CInt("&H" & Mid(sInput, iLoop, 2))
    'If iValue < iMin Then
    'iMin = iValue
    'End If
    'Next 
    '
    'Dim sTemp As New StringBuilder
    '
    'For 'iLoop As Integer = 1 To sInput.Length - 1 Step 2
    'iValue = CInt("&H" & Mid(sInput, iLoop, 2)) - iMin
    'sTemp.Append(ShortHex(iValue, 2))
    'Next 
    '
    'Return sTemp.ToString()
    '
    'End Function

    ' RDC 19102000 Temporary licensing
    Private Function Upgrade(ByVal sInput As String, ByRef iMin As Integer) As String

        Dim iValue As Integer

        Dim sTemp As New StringBuilder

        For iLoop As Integer = 1 To sInput.Length - 1 Step 2
            iValue = CInt("&H" & Mid(sInput, iLoop, 2)) + iMin
            sTemp.Append(ShortHex(iValue, 2))
        Next

        Return sTemp.ToString()

    End Function

    ' RDC 19102000 Temporary licensing

    'Private Function ConvertToHex(ByVal sInput As String) As String
    '
    '
    'Dim sTemp As New StringBuilder
    'Dim iOff As Integer = 0
    '
    'For 'iLoop As Integer = 1 To sInput.Length
    'sTemp.Append((Strings.Asc(Mid(sInput, iLoop, 1)(0)) + iOff).ToString("X"))
    '
    'iOff += 1
    '
    'If iOff = 16 Then
    'iOff = 0
    'End If
    'Next 
    '
    'Return sTemp.ToString()
    '
    'End Function

    ' RDC 19102000 Temporary licensing
    Private Function ConvertFromHex(ByVal sInput As String) As String


        Dim sTemp As New StringBuilder
        Dim iOff As Integer = 0

        For iLoop As Integer = 1 To sInput.Length - 1 Step 2
            sTemp.Append(Strings.Chr(CInt("&H" & Mid(sInput, iLoop, 2)) - iOff).ToString())

            iOff += 1

            If iOff = 16 Then
                iOff = 0
            End If
        Next

        Return sTemp.ToString()

    End Function

    ' RDC 19102000 Temporary licensing
    Private Function RollLeft(ByVal sInput As String, ByVal iNumber As Integer) As String


        Dim sTemp As String = sInput

        For iLoop As Integer = 1 To iNumber
            sTemp = Mid(sTemp, 2) & sTemp.Substring(0, 1)
        Next

        Return sTemp

    End Function

    ' RDC 19102000 Temporary licensing

    'Private Function RollRight(ByVal sInput As String, ByVal iNumber As Integer) As String
    '
    '
    'Dim sTemp As String = sInput
    '
    'For 'iLoop As Integer = 1 To iNumber
    'sTemp = sTemp.Substring(sTemp.Length - 1) & sTemp.Substring(0, sTemp.Length - 1)
    'Next 
    '
    'Return sTemp
    '
    'End Function

    ' RDC 19102000 Temporary licensing
    Private Function LogTimebombMessage(ByVal sMsg As String, ByVal vLevel As MsgBoxStyle) As Integer

        Dim result As Integer = 0
        Dim sLogFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            Dim sErrUniqueId As String = gPMFunctions.GenerateUniqueSSPExceptionRef(gPMConstants.ERROR_NO_LENGTH)
            ' write to screen
            Interaction.MsgBox(sErrUniqueId & Strings.Chr(13) & Strings.Chr(10) & _
                               sMsg & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                               "Contact your local system administrator." & Strings.Chr(13) & Strings.Chr(10) & _
                               "Contact Policy Master Support for further details.", vLevel, "iLogonManager")

            ' write to local log file
            gPMFunctions.LogMessageToFile(sUsername:=g_sUserName, iType:=1, sMsg:=sMsg, sErrUniqueId:=sErrUniqueId, vApp:=ACApp, vClass:=ACClass, vMethod:="Logon")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    ' RDC 19102000 Temporary licensing END

    ' RFC11/08/03 - Load Balance LicenceManager
    Private Function SiriusArchitectureCOMPlusExists() As Boolean

        ' COM+ objects
        Dim result As Boolean = False
        Dim oCatalog As COMAdmin.COMAdminCatalog ' primary connection to COM+ catalog (RegDB)
        Dim cApplications As COMAdmin.COMAdminCatalogCollection ' catalog collection
        Dim oApplication As COMAdmin.COMAdminCatalogObject ' single item in catalog collection

        'Dim cComponents As COMAdmin.COMAdminCatalogCollection ' application roles collection
        'Dim oComponent As COMAdmin.COMAdminCatalogObject ' single item in role collection
        'Dim cRoles As COMAdmin.COMAdminCatalogCollection ' application roles collection
        'Dim oRole As COMAdmin.COMAdminCatalogObject ' single item in role collection




        ' open catalog
        oCatalog = New COMAdmin.COMAdminCatalog()

        ' retrieve catalog
        cApplications = oCatalog.GetCollection("Applications")

        ' important! catalog will be empty otherwise
        cApplications.Populate()

        ' Check each app name
        For Each oApplication2 As COMAdmin.COMAdminCatalogObject In cApplications
            oApplication = oApplication2

            If oApplication.Name = "Sirius Architecture COM+" Then
                result = True
                Exit For
            End If
        Next oApplication2

        oApplication = Nothing
        cApplications = Nothing
        oCatalog = Nothing

        Return result

    End Function

    Public Function VerifyAndUpdateProperties(ByRef bPasswordChanged As Boolean, Optional ByRef frmInterface As frmInterface = Nothing) As Integer

        Dim result As Integer = 0
        Dim lError As gPMConstants.PMEReturnCode
        Dim sEncryptedPassword As String = ""
        Dim sSelectedPrinter As String = ""
        Dim sRegEx As String = ""
        Dim bIsStrongPassword As Boolean = False
        Dim bIsReusedPassword As Boolean = True

        Dim bIsvalid As Boolean = False
        Dim sOldencryPassword As String = ""
        Dim sPasswordChanged As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'if the password has changed
            If bPasswordChanged Then
                lError = CType(iPMFunc.Encrypt(sPassword:=OldPassword, sEncryptedPassword:=sEncryptedPassword), gPMConstants.PMEReturnCode)
                OldPassword = sEncryptedPassword
                If bPMFunc.CheckPassword(g_sPassword, OldPassword) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Old Password is incorrect.", "E0102 - Incorrect Old Password", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    frmInterface.txtOldPassword.Text = ""
                    frmInterface.txtOldPassword.Focus()
                    Return result
                End If

                'check the new and old passwords are different
                lError = bPMFunc.CheckPassword(NewPassword, OldPassword)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    'do nothing
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("New Password must not be the same as the old password.", "E0103 - Incorrect Old Password", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    frmInterface.txtNewPassword.Text = ""
                    frmInterface.txtConfirmPassword.Text = ""
                    frmInterface.txtNewPassword.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'call encrypt string with hashing password
                lError = CType(iPMFunc.Encrypt(sPassword:=NewPassword, sEncryptedPassword:=sEncryptedPassword), gPMConstants.PMEReturnCode)
                sPasswordChanged = NewPassword
                NewPassword = sEncryptedPassword

                'call encrypt string with old encryption password
                lError = CType(bPMFunc.LicenceEncrypt(sLicence:=ConfirmPassword, sLicenceKey:=sEncryptedPassword), gPMConstants.PMEReturnCode)
                sOldencryPassword = sEncryptedPassword

                lError = bPMFunc.CheckPassword(ConfirmPassword, NewPassword)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("The new and confirmed passwords do not match." & Strings.Chr(10).ToString() & _
                                    "Please Type them again.", "E0104 - Incorrect Password Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    frmInterface.txtNewPassword.Text = ""
                    frmInterface.txtConfirmPassword.Text = ""
                    frmInterface.txtNewPassword.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Make a call to IsStrongPassword Function with New Password to check whether new password is strong enough or not
                lError = gPMFunctions.IsStrongPassword(v_sUsername:=g_sUserName, v_iUserID:=g_iUserID, v_iMainSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=ACApp, sPasswordString:=ConfirmPassword.Trim(), bIsstrongPassword:=bIsStrongPassword, v_iSourceID:=g_iSourceID)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    frmInterface.txtNewPassword.Text = ""
                    frmInterface.txtConfirmPassword.Text = ""
                    frmInterface.txtNewPassword.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If Not bIsStrongPassword Then
                    ' Retrieve Strong Password Message from System options and display it as a message
                    MessageBox.Show(sFailureMessage, "Logged-In with Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    frmInterface.txtNewPassword.Text = ""
                    frmInterface.txtConfirmPassword.Text = ""
                    frmInterface.txtNewPassword.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check for ReUse password. Make a call to IsReusedPassword 
                lError = g_oClientManager.IsReusedPassword(iUser_Id:=g_iUserID, sNewPassword:=ConfirmPassword.Trim(), bIsValid:=bIsvalid)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If bIsvalid Then

                Else
                    MessageBox.Show("Password Entered is already in use. Please choose another Password.", "Password Already in use", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    frmInterface.txtNewPassword.Text = ""
                    frmInterface.txtConfirmPassword.Text = ""
                    frmInterface.txtNewPassword.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            g_oClientManager.Password = OldPassword



            If bPasswordChanged Then
                lError = g_oClientManager.UpdateUser(vLanguageID:=g_iLanguageID, vpassword:=NewPassword, vUsername:=g_sUserName, vPasswordChangeDate:=DateTime.Now, vUserID:=g_iUserID, sOldPassword:=sOldencryPassword, sPasswordChanged:=sPasswordChanged)
            Else
                lError = g_oClientManager.UpdateUser(vLanguageID:=g_iLanguageID, vUsername:=g_sUserName)
            End If

            If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Verify and Update Properties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="VerifyAndUpdateProperties", excep:=excep)

            Return result

        End Try
    End Function
End Class