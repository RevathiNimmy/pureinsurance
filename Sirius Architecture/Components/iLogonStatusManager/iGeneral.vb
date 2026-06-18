Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class General

    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: iGeneral
    '
    ' Date: 09 July 1996
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    'Private g_frmInterface As frmInterface
    'Private objfrmInterface As frmInterface

    Private Const ACClass As String = "General"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter member.
    Private m_sOldPassword As String = ""
    Private m_sNewPassword As String = ""
    Private m_sConfirmPassword As String = ""
    Private m_sLanguageCaption As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
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
    Public Function Initialise(ByRef frmInterface As frmInterface) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Store the instance of the form into the member.
            g_frmInterface = frmInterface

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    Private Function LanguageLookup(ByRef sLanguageCaption As String, ByRef iLanguageId As Integer) As Integer

        'test function remove once lookup component has been developed
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Select Case sLanguageCaption
            Case "English(British)"
                iLanguageId = 1
            Case "American English"
                iLanguageId = 2
            Case "German"
                iLanguageId = 3
            Case "French"
                iLanguageId = 4
        End Select

        Return result

    End Function

    Public Function LanguageIdLookup(ByRef iLanguageId As Byte, ByRef sLanguageCaption As String) As Integer

        'test function remove once lookup component has been developed
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case iLanguageId
                Case 1
                    sLanguageCaption = "English(British)"
                Case 2
                    sLanguageCaption = "American English"
                Case 3
                    sLanguageCaption = "German"
                Case 4
                    sLanguageCaption = "French"
                Case Else

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Language Lookup Error", vApp:=ACApp, vClass:=ACClass, vMethod:="LanguageLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function PopulateLanguageList(ByRef lstBox As ComboBox) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            'test function until the lookup component has been developed
            lstBox.Items.Clear()

            lstBox.Items.Add("English(British)")
            lstBox.Items.Add("American English")
            lstBox.Items.Add("German")
            lstBox.Items.Add("French")

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMFalse
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
                g_frmInterface = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SaveLogDetails
    '
    ' Description: Saves the Log
    '
    '
    ' ***************************************************************** '
    Public Function SaveLogDetails(ByRef frmInterface As frmInterface) As Integer

        Dim result As Integer = 0
        Dim lErrorValue As Integer
        Dim iUserLogLevel As Integer
        Dim sLogFilename As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Save the user log level to the registry.

            iUserLogLevel = frmInterface.cmbUserLogLevel.SelectedValue
            ' Set the UserLogLevel setting in
            ' HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Client
            lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogLevel, v_sSettingValue:=CStr(iUserLogLevel))

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save user log level to the registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveLogDetails")
            End If

            ' Save the log filename to the registry.

            sLogFilename = frmInterface.txtLogFilename.Text
            lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogFile, v_sSettingValue:=sLogFilename)

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save log filename to the registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveLogDetails")
            End If

            ' Disable the Apply Log Details Button
            'developers guide no 69
            frmInterface.cmdApplyLogDetails.Enabled = False

            ' Set the relevant Form Properties
            frmInterface.LogFile = sLogFilename
            frmInterface.UserLogLevel = iUserLogLevel

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveLogDetails", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetListIndex
    '
    ' Description: Sets the ID passed to the correct list item.
    '
    ' ***************************************************************** '
    Public Function SetListIndex(ByRef ctlListDetails As ComboBox, ByRef lID As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Step through all of the list details, checking
            ' the ID for a match.
            If ctlListDetails.Items.Count >= lID Then
                ctlListDetails.SelectedIndex = lID - 1
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("lID", lID)
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the list index", vApp:=ACApp, vClass:=ACClass, vMethod:="SetListIndex", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    Public Function VerifyAndUpdateProperties(ByRef bPasswordChanged As Boolean, Optional ByRef frmInterface As frmInterface = Nothing) As Integer

        Dim result As Integer = 0
        Dim lError As gPMConstants.PMEReturnCode
        Dim sEncryptedPassword As String = ""
        Dim iLanguageId As Integer
        Dim sSelectedPrinter As String = ""
        Dim bIsStrongPassword As Boolean = False
        Dim bIsvalid As Boolean = False
        Dim sOldencryPassword As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'if the password has changed
            If bPasswordChanged Then

                
                'Encrypt Old Password
                lError = CType(iPMFunc.Encrypt(sPassword:=OldPassword, sEncryptedPassword:=sEncryptedPassword), gPMConstants.PMEReturnCode)
                OldPassword = sEncryptedPassword

                'check old password matches database
                'change this to call a wrapper function in logon manager, which calls a function in client manager

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
                lError = gPMFunctions.IsStrongPassword(v_sUsername:=g_sUserName, v_iUserID:=g_iUserID, v_iMainSourceID:=g_iSourceID, v_iLanguageID:=iLanguageId, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=ACApp, sPasswordString:=ConfirmPassword.Trim(), bIsstrongPassword:=bIsStrongPassword, v_iSourceID:=g_iSourceID)
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    frmInterface.txtNewPassword.Text = ""
                    frmInterface.txtConfirmPassword.Text = ""
                    frmInterface.txtNewPassword.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If Not bIsStrongPassword Then
                    ' Retrieve Strong Password Message from System options and display it as a message
                    MessageBox.Show("Password must be between eight and fifteen characters in length, be a mix of upper and lowercase letters, and contain at least one number. Special characters are not permitted" & Strings.Chr(10).ToString() & _
                                "", "Logged-In with Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    frmInterface.txtNewPassword.Text = ""
                    frmInterface.txtConfirmPassword.Text = ""
                    frmInterface.txtNewPassword.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check for ReUse password. Make a call to IsReusedPassword 
                lError = g_oLogonManager.ClientManager.IsReusedPassword(iUser_Id:=g_iUserID, sNewPassword:=ConfirmPassword.Trim(), bIsValid:=bIsvalid)
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


            'get the language id from the language caption using a lookup function
            lError = CType(LanguageLookup(LanguageCaption, iLanguageId), gPMConstants.PMEReturnCode)

            If lError <> gPMConstants.PMEReturnCode.PMTrue Then


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the selected printer
            sSelectedPrinter = frmInterface.cmbServerPrinter.Items(0).ToString()
            ' Has the Printer been changed

            g_oLogonManager.ClientManager.Password = OldPassword
            'objfrmInterface is used instead of g_frmInterface as cmbServerPrinter is not a part of g_frmInterface
            If frmInterface.cmbServerPrinter.Tag.Trim() = sSelectedPrinter.Trim() Then

                'No(change)
                If bPasswordChanged Then

                    lError = g_oLogonManager.ClientManager.UpdateUser(vLanguageID:=iLanguageId, vpassword:=NewPassword, vUsername:=g_sUserName, vPasswordChangeDate:=DateTime.Now, vUserID:=g_iUserID, sOldPassword:=sOldencryPassword)
                Else
                    lError = g_oLogonManager.ClientManager.UpdateUser(vLanguageID:=iLanguageId, vUsername:=g_sUserName)
                End If

            Else
                'Yes, it has changed so update it.
                If bPasswordChanged Then

                    lError = g_oLogonManager.ClientManager.UpdateUser(vLanguageID:=iLanguageId, vpassword:=NewPassword, vUsername:=g_sUserName, vServerPrinter:=sSelectedPrinter, vPasswordChangeDate:=DateTime.Now, vUserID:=g_iUserID, sOldPassword:=sOldencryPassword)
                Else
                    lError = g_oLogonManager.ClientManager.UpdateUser(vLanguageID:=iLanguageId, vUsername:=g_sUserName, vServerPrinter:=sSelectedPrinter)
                End If
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



    'PUBLIC Methods (End)

    'PRIVATE Methods (Begin)
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
End Class