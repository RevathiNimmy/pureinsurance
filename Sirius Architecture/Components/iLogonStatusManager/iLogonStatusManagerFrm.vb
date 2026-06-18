Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Imports Artinsoft.VB6.Gui
Imports System.Collections.Generic
Imports Artinsoft.VB6.VB
'developer guide no.211
'Friend Partial Class frmInterface
Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 09 July 1996
    '
    ' Description: Main View Form.
    '
    ' Edit History:
    ' RFC 23/04/1998 - Added 'Logged On To' and 'PMB Company' fields.
    ' RFC 17/06/1998 Add Confirmation of Password
    ' RFC 17/06/1998 Add Server Printer
    ' RFC 19/09/1998 Display status of Broking Link. Display server name
    '                from Client Manager. Help About SA Added.
    ' RFC170299 - Get the Version from the Registry
    ' RKS 13/12/2004 Unified Login Implementation
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Form cancelled flag.
    Private m_bFormCancelled As Boolean

    ' Form error number.
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    ' Object parameter member.
    Private m_vObjectParam As Object

    ' System log level.
    Private m_iLogLevel As Integer

    ' User log level.
    Private m_iUserLogLevel As Integer

    ' Log filename.
    Private m_sLogFile As String = ""

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iLogonStatusManager.General
    ' PRIVATE Data Members (End)
    Private m_ologonstatusmanager As iLogonStatusManager.LogonStatusManager
    Private m_bPasswordChanged As Boolean

    Private lstProcess As List(Of String) = New List(Of String)
    Const portnumberconstant = 65535
    ' Private Declare Function WTSGetActiveConsoleSessionId Lib "Kernel32.dll" Alias "WTSGetActiveConsoleSessionId" () As Int32



    ' PUBLIC Property Procedures (Begin)


    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    Public Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the form.
            Return m_lErrorNumber

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the current form's error number.
            m_lErrorNumber = Value

        End Set
    End Property


    Public Property FormCancelled() As Boolean
        Get

            ' Standard Property.

            ' Return if the form has been cancelled.
            Return m_bFormCancelled

        End Get
        Set(ByVal Value As Boolean)

            ' Standard Property.

            ' Set the form's cancelled flag.
            m_bFormCancelled = Value

        End Set
    End Property


    Public Property PasswordChanged() As Boolean
        Get

            ' Standard Property.

            ' Return if the password has been changed.
            Return m_bPasswordChanged

        End Get
        Set(ByVal Value As Boolean)

            ' Standard Property.

            ' Set the password changed flag.
            m_bPasswordChanged = Value

        End Set
    End Property



    'Private Function ObjectParam() As Object
    '
    ' Standard Property.
    '
    ' Return the objects parameter value.
    'Return m_vObjectParam
    '
    'End Function
    Public WriteOnly Property ObjectParam() As Object
        Set(ByVal Value As Object)

            ' Standard Property.

            ' Set the objects parameter value.


            m_vObjectParam = Value

        End Set
    End Property
    Public WriteOnly Property UserLogLevel() As Integer
        Set(ByVal Value As Integer)
            m_iUserLogLevel = Value
        End Set
    End Property
    Public WriteOnly Property LogFile() As String
        Set(ByVal Value As String)
            m_sLogFile = Value
        End Set
    End Property

    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DisplayFormDetails
    '
    ' Description: Display all of the form's details using the details
    '              from the public logon manager.
    '
    ' ***************************************************************** '
    Private Function DisplayFormDetails() As Integer

        Dim result As Integer = 0
        Dim lErrorValue As gPMConstants.PMEReturnCode
        Dim sLanguageCaption, sPMBCompany, sSystemName, sCaption As String
        ' RDC 17092002
        Dim sVersion, sRelease, sSiriusType As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update form details from the logon
            ' manager object.
            With g_oLogonManager
                g_sUserName = .UserName
                g_sPassword = .Password
                g_iLanguageID = .LanguageID
                g_iUserID = .Userid
                ' RFC 23/04/1998
                ' RFC 19081998 - Use Client Manager server name property.

                sSystemName = .ClientManager.ServerName
                If .LoggedOnLocally Then
                    sCaption = "My Computer" & " (" & sSystemName & ")"

                    'developer guide no. 26
                    lbl3LoggedOnTo.Text = sCaption
                Else
                    sCaption = "Server" & " (" & sSystemName & ")"

                    'developer guide no. 26
                    lbl3LoggedOnTo.Text = sCaption
                End If

                ' RFC 23/04/1998
                sPMBCompany = .PMBCompanyNumber.Trim()

                If sPMBCompany = "" Then
                    lblPMBCompany.Visible = False
                    pan3PMBCompany.Visible = False
                    lbl3PMBCompany.Visible = False
                Else

                    'developer guide no. 26
                    lbl3PMBCompany.Text = sPMBCompany
                End If

                ' RFC 19081998
                ' Is the PMB Link Required, Available or Not Available.
                If Not .LoggedOnToPMB Then
                    If sPMBCompany.Trim() = "" Then

                        'developer guide no. 26
                        lbl3PMBLink.Text = "Not Required"
                    Else

                        'developer guide no. 26
                        lbl3PMBLink.Text = "Not Available"
                    End If
                Else

                    'developer guide no. 26
                    lbl3PMBLink.Text = "Available"
                End If
                'lblSource.Text = g_sSourceName

                'developer guide no. 26
                lbl3LogonName.Text = g_sUserName
                If g_oLogonManager.UnifiedLogon Then

                    'developer guide no. 26
                    lbl3UnifiedLogin.Text = g_oLogonManager.UnifiedLogonUsername


                    cmdChangePassword.Enabled = False
                    txtOldPassword.Enabled = False
                    txtNewPassword.Enabled = False
                    txtConfirmPassword.Enabled = False
                Else

                    'developer guide no. 26
                    lbl3UnifiedLogin.Text = ""
                End If

                'developer guide no. 26
                lbl3LogonTime.Text = StringsHelper.Format(.LogonTime, "dddddd hh:mm:ss am/pm")
                txtLogFilename.Text = m_sLogFile.Trim()

                For Each vdp As ValueDescriptionPair In cmbUserLogLevel.Items
                    If (vdp.Value = m_iUserLogLevel) Then
                        ' Found a match

                        ' Set the list index to the current item.
                        cmbUserLogLevel.SelectedItem = vdp

                        Exit For
                    End If
                Next

                'set the language_id combo text
                g_iLanguageID = 1
                lErrorValue = CType(m_oGeneral.LanguageIdLookup(sLanguageCaption:=sLanguageCaption, iLanguageId:=g_iLanguageID), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    ErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to select list index", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayFormDetails")
                End If

                'set combobox text

                lErrorValue = CType(SetListIndexFromItemText(sItemText:=sLanguageCaption, oControl:=cmbUserLanguage), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    ErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to select list index", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayFormDetails")
                End If

            End With           

            ' RDC 17092002 version string

            lErrorValue = CType(gPMFunctions.GetSiriusVersion(sVersion, sRelease, sSiriusType), gPMConstants.PMEReturnCode)

            Dim sSR As String = ""
            Dim sBuild As String = ""
            Dim vStringArray() As String
            sBuild = " Build " & sRelease.Substring(sRelease.LastIndexOf("."c) + 1)
            vStringArray = sRelease.Split("."c)
            sSR = " SR" & vStringArray(0)
            'Do Not Show Build Numbers.
            sVersion = sVersion & sSR & sBuild
            
            ' Set the version number and date
            lblVersion.Text = "Pure Insurance Version " & sVersion

            lErrorValue = CType(PopulatePrinterList(), gPMConstants.PMEReturnCode)
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display form details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayFormDetails", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayFormDefaults
    '
    ' Description: Display all of the form's default details.
    '
    ' ***************************************************************** '
    Private Function DisplayFormDefaults() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try
            Dim VDP_Array As New ArrayList
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add details to the message log level.
            'developer guide no. 170
            If m_iLogLevel <= 1 Then
                VDP_Array.Add(New ValueDescriptionPair(1, gPMConstants.PMFatalText))
            End If
            If m_iLogLevel <= 2 Then
                VDP_Array.Add(New ValueDescriptionPair(2, gPMConstants.PMErrorText))
            End If
            If m_iLogLevel <= 3 Then
                VDP_Array.Add(New ValueDescriptionPair(3, gPMConstants.PMWarningText))
            End If
            If m_iLogLevel <= 4 Then
                VDP_Array.Add(New ValueDescriptionPair(4, gPMConstants.PMOnErrorText))
            End If
            If m_iLogLevel <= 5 Then
                VDP_Array.Add(New ValueDescriptionPair(5, gPMConstants.PMInfoText))
            End If
            If m_iLogLevel <= 6 Then
                VDP_Array.Add(New ValueDescriptionPair(6, gPMConstants.PMDebug1Text))
            End If
            If m_iLogLevel <= 7 Then
                VDP_Array.Add(New ValueDescriptionPair(7, gPMConstants.PMDebug2Text))
            End If
            If m_iLogLevel <= 8 Then
                VDP_Array.Add(New ValueDescriptionPair(8, gPMConstants.PMDebug3Text))
            End If
            If m_iLogLevel <= 9 Then
                VDP_Array.Add(New ValueDescriptionPair(9, gPMConstants.PMDebug4Text))
            End If

            With cmbUserLogLevel
                .DisplayMember = "Description"
                .ValueMember = "Value"
                .DataSource = VDP_Array
            End With

            m_lErrorNumber = CType(m_oGeneral.PopulateLanguageList(cmbUserLanguage), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display form defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayFormDefaults", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLogDetails
    '
    ' Description: Gets the log details from the registry.
    '
    ' ***************************************************************** '
    Private Function GetLogDetails() As Integer

        Dim result As Integer = 0
        Dim lErrorValue As Integer
        Dim sResult As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the system log level.
            m_iLogLevel = g_oLogonManager.LogLevel

            ' Get the UserLogLevel setting from
            ' HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Client
            lErrorValue = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogLevel, r_sSettingValue:=sResult)

            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a User Log Level
            Dim dbNumericTemp As Double
            If Double.TryParse(sResult, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                ' Yes, So Set the user log level member.
                m_iUserLogLevel = CInt(sResult)
            Else
                ' No, so use the System default
                m_iUserLogLevel = m_iLogLevel
                ' Save the default for this User
                ' Set the UserLogLevel setting in
                ' HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Client
                lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogLevel, v_sSettingValue:=CStr(m_iLogLevel))
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Is the System Log Level is greater than the User Log level

            If m_iLogLevel > m_iUserLogLevel Then
                'This is invalid - reset the user log level
                m_iUserLogLevel = m_iLogLevel
                ' Save the default for this User
                ' Set the UserLogLevel setting in
                ' HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Client
                lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogLevel, v_sSettingValue:=CStr(m_iLogLevel))
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'Get the User Specific Log File Name setting in
            ' HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Common
            lErrorValue = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogFile, r_sSettingValue:=sResult)
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If there is no User specific Log File,
            ' get the Machine Specific one
            If sResult.Trim() = "" Then
                ' Get the Log File Name setting in
                ' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Common
                lErrorValue = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogFile, r_sSettingValue:=sResult)
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If we got the Machine Log File,
                ' save it for this user.
                If sResult.Trim() <> "" Then
                    'DAK130100
                    ' Insert User Name
                    lErrorValue = gPMFunctions.InsertUserName(sResult, g_oLogonManager.UserName.Trim())
                    If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    lErrorValue = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogFile, v_sSettingValue:=sResult)
                    If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If

            ' If we still do not have a Log File, use the System default
            If sResult.Trim() = "" Then
                sResult = gPMConstants.PMDefaultLogFile
                ' Insert User Name
                lErrorValue = gPMFunctions.InsertUserName(sResult, g_sUserName)
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Set the user log level member.
            m_sLogFile = sResult.Trim()

            g_iSourceID = g_oLogonManager.SourceID
            g_iCountryID = g_oLogonManager.CountryID
            m_ologonstatusmanager.SourceName = g_oLogonManager.SourceName
            lblSource.Text = g_oLogonManager.SourceName
            'MessageBox.Show(g_oLogonManager.SourceName)
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get log details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLogDetails", excep:=excep)

            Return result

        End Try
    End Function

    Private Function SetListIndexFromItemText(ByRef sItemText As String, ByRef oControl As Control) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If TypeOf oControl Is ComboBox Then
                Dim combx As ComboBox
                combx = oControl
                If combx.FindString(sItemText) > -1 Then
                    combx.SelectedItem = sItemText
                    result = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set index in " & oControl.Name, vApp:=ACApp, vClass:=ACClass, vMethod:="SetListIndexFromItemText", excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulatePrinterList
    '
    ' Description: Populate the supplied List box with the List of Valid
    '              Server Printers for this user. If the user is not
    '              allowed to change this, disable the control
    ' ***************************************************************** '
    Private Function PopulatePrinterList() As Integer
        Dim result As Integer = 0
        Dim sDefaultPrinter, sSelectedPrinter As String
        'developer guide no. 17
        
        Dim lReturn As gPMConstants.PMEReturnCode

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            cmbServerPrinter.Items.Clear()

            sSelectedPrinter = g_oLogonManager.ServerPrinter

            sDefaultPrinter = PrinterHelper.Printer.DeviceName.Trim()

            If String.IsNullOrEmpty(sDefaultPrinter) Then
                Return result
            End If

            Dim printersList As New List(Of String)

            ' Collect each unique printer name
            For Each oPrinter As Object In PrinterHelper.Printers
                Dim name As String = oPrinter.DeviceName.Trim()

                ' Avoid duplicates
                If Not printersList.Contains(name) Then
                    cmbServerPrinter.Items.Add(name)
                End If
            Next

            ' If the selected printer is "" this means use the default
            If sSelectedPrinter = "" Then
                sSelectedPrinter = sDefaultPrinter
            End If

            ' Use the Tag so we now if the user changes printer
            cmbServerPrinter.Tag = sSelectedPrinter

            

            ' Is the User allowed to change their printer
            If g_oLogonManager.IsPrinterChangeable = gPMConstants.PMEReturnCode.PMFalse Then
                ' No
                cmbServerPrinter.Enabled = False
            Else
                ' Yes
                cmbServerPrinter.Enabled = True
            End If

            ' Set the selected printer
            lReturn = CType(SetListIndexFromItemText(sItemText:=sSelectedPrinter, oControl:=cmbServerPrinter), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' The User Selected Printer was not in the list
                sSelectedPrinter = sDefaultPrinter
                ' Use the Default
                lReturn = CType(SetListIndexFromItemText(sItemText:=sSelectedPrinter, oControl:=cmbServerPrinter), gPMConstants.PMEReturnCode)
            End If

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: ResetFormDetails
    '
    ' Description: Resets the Data on the form.
    '
    ' ***************************************************************** '
    Private Function ResetFormDetails() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lReturn As Integer
        Dim sLanguageCaption As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Password Fields
            txtOldPassword.Text = ""
            txtNewPassword.Text = ""
            txtConfirmPassword.Text = ""

            'Hide the password controls
            lblOldPassword.Visible = False
            lblNewPassword.Visible = False
            txtOldPassword.Visible = False
            txtNewPassword.Visible = False
            lblConfirmPassword.Visible = False
            txtConfirmPassword.Visible = False


            ' lReturn = m_oGeneral.SetListIndex(cmbUserLogLevel, m_iUserLogLevel)
            For Each vdp As ValueDescriptionPair In cmbUserLogLevel.Items
                If (vdp.Value = m_iUserLogLevel) Then
                    ' Found a match

                    ' Set the list index to the current item.
                    cmbUserLogLevel.SelectedItem = vdp

                    Exit For
                End If
            Next

            'If cmbUserLogLevel.Items.Count >= m_iUserLogLevel Then
            '    cmbUserLogLevel.SelectedValue = m_iUserLogLevel - 1
            'End If

            ' Reset the List Boxes
            lReturn = m_oGeneral.PopulateLanguageList(cmbUserLanguage)

            lReturn = m_oGeneral.LanguageIdLookup(sLanguageCaption:=sLanguageCaption, iLanguageId:=g_iLanguageID)

            lReturn = SetListIndexFromItemText(sItemText:=sLanguageCaption, oControl:=cmbUserLanguage)
            lReturn = PopulatePrinterList()

            txtLogFilename.Text = ""
            txtLogFilename.Text = m_sLogFile

            ' Disable the apply buttons
            cmdApply.Enabled = False
            cmdApplyLogDetails.Enabled = False

            ' Reset the Password Changed Flag
            m_bPasswordChanged = False

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display form defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetFormDetails", excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)

    Private Sub cmbServerPrinter_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbServerPrinter.SelectedIndexChanged

        'enable the apply button
        ' RFC 17/06/1998 Add Server Printer
        cmdApply.Enabled = True

    End Sub

    Private Sub cmbUserLanguage_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbUserLanguage.SelectedIndexChanged

        'enable the apply button
        cmdApply.Enabled = True

    End Sub

    Private Sub cmbUserLogLevel_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbUserLogLevel.SelectedIndexChanged

        ' Enable the Apply Button
        cmdApplyLogDetails.Enabled = True

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        'Apply the changes on the user detail tab

        'set the language_id from a lookup component
        m_oGeneral.LanguageCaption = cmbUserLanguage.Text

        ' Store the Password Details
        m_oGeneral.OldPassword = txtOldPassword.Text.Trim()
        m_oGeneral.NewPassword = txtNewPassword.Text.Trim()
        m_oGeneral.ConfirmPassword = txtConfirmPassword.Text.Trim()
        If txtOldPassword.Text = "" Then
            lblOldPassword.Visible = False
            lblNewPassword.Visible = False
            txtOldPassword.Visible = False
            txtNewPassword.Visible = False
            ' RFC 17/06/1998 Add Confirmation of Password
            lblConfirmPassword.Visible = False
            txtConfirmPassword.Visible = False
            txtOldPassword.Text = ""
            txtNewPassword.Text = ""
            txtConfirmPassword.Text = ""

            'disable the apply button
            cmdApply.Enabled = False

            PasswordChanged = False
            Exit Sub
        End If
        ' Valide and Apply the new details.
        m_lErrorNumber = CType(m_oGeneral.VerifyAndUpdateProperties(bPasswordChanged:=m_bPasswordChanged, frmInterface:=Me), gPMConstants.PMEReturnCode)

        If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If
        ' Store the new password
        g_sPassword = m_oGeneral.ConfirmPassword

        'hide the password controls
        lblOldPassword.Visible = False
        lblNewPassword.Visible = False
        txtOldPassword.Visible = False
        txtNewPassword.Visible = False
        ' RFC 17/06/1998 Add Confirmation of Password
        lblConfirmPassword.Visible = False
        txtConfirmPassword.Visible = False
        txtOldPassword.Text = ""
        txtNewPassword.Text = ""
        txtConfirmPassword.Text = ""

        'disable the apply button
        cmdApply.Enabled = False

        PasswordChanged = False

    End Sub

    Private Sub cmdApplyLogDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApplyLogDetails.Click

        ' Apply the changes
        Dim lReturn As gPMConstants.PMEReturnCode = CType(m_oGeneral.SaveLogDetails(Me), gPMConstants.PMEReturnCode)

        ' Disable the Apply Button
        cmdApplyLogDetails.Enabled = False

    End Sub

    Private Sub cmdChangePassword_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdChangePassword.Click

        'unhide the password controls
        lblOldPassword.Visible = True
        lblNewPassword.Visible = True
        txtOldPassword.Visible = True
        txtNewPassword.Visible = True
        ' RFC 17/06/1998 Add Confirmation of Password
        lblConfirmPassword.Visible = True
        txtConfirmPassword.Visible = True
        m_bPasswordChanged = True 'PN 37247
        'enable the apply button
        cmdApply.Enabled = True

        txtOldPassword.Focus()

    End Sub


    Private Sub Form_Initialize_Renamed()

        Dim lErrorValue As Integer
        Dim iSession As Integer
        ' Forms Initialise Event.

        Const portnumberconstant = 65535
        Try

            ' Get the instance of the running logon manager.
            'logoninst g_oLogonManager = New iLogonManager.LogonManager()
            'g_oLogonManager = iLogonManager.LogonManager.logoninstance()
            g_oLogonManager = CType(Activator.GetObject(GetType(iLogonManager.LogonManager), "tcp://localhost:" & (portnumberconstant - Process.GetCurrentProcess.SessionId) & "/SSP"), iLogonManager.LogonManager)

            ' Check for errors.
            If g_oLogonManager Is Nothing Then
                ErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the instance of the logon manager object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Exit Sub
            End If

            ' Load the instance of the general
            ' interface object into memory.
            m_oGeneral = New iLogonStatusManager.General()
            m_ologonstatusmanager = New iLogonStatusManager.LogonStatusManager()
            ' Call the initialise method passing
            ' this form as the parameter.
            lErrorValue = m_oGeneral.Initialise(Me)

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the general interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Exit Sub
            End If

            ' Get the log details.
            lErrorValue = GetLogDetails()

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the log details", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Exit Sub
            End If

            'hide the password controls
            lblOldPassword.Visible = False
            lblNewPassword.Visible = False
            txtOldPassword.Visible = False
            txtNewPassword.Visible = False
            ' RFC 17/06/1998 Add Confirmation of Password
            lblConfirmPassword.Visible = False
            txtConfirmPassword.Visible = False

            ' Set the cancelled property to true
            FormCancelled = True

            ' Set the password changed property to false
            PasswordChanged = False

            ' Disable the Apply Log Details Button
            cmdApplyLogDetails.Enabled = False

        Catch excep As System.Exception



            ' Error Section

            ErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the form object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Sets up the forms details.

        Dim lErrorValue As Integer

        Try
            Form_Initialize_Renamed()

            ' Display the form's default values.
            lErrorValue = DisplayFormDefaults()

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to display the form's defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Exit Sub
            End If

            ' Display the form's details
            lErrorValue = DisplayFormDetails()

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to display the form's details", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            End If

            'disable the apply button
            cmdApply.Enabled = False
            cmdApplyLogDetails.Enabled = False
            PopulateProcessList()
        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click Event Of The OK Button.

        Dim lErrorValue As Integer

        Try

            ' Set the cancelled property to false
            FormCancelled = False

            ' Do we need to save the Log Details
            If cmdApplyLogDetails.Enabled Then

                ' Yes, so save them.
                lErrorValue = m_oGeneral.SaveLogDetails(Me)

                ' Check for errors.
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                End If

            End If

            ' Do we need to update the User Options
            If cmdApply.Enabled Then
                cmdApply_Click(cmdApply, New EventArgs())
            End If

            ' If either of the two Apply buttons are still enabled
            ' there were errors, therefore do not minimise
            If cmdApply.Enabled Or cmdApplyLogDetails.Enabled Then
                ' Do Nothing
            Else
                ' Minimise the form.
                Me.WindowState = FormWindowState.Minimized
            End If

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Click Event Of The OK Button.

        Try

            ' Set the cancelled property to true
            FormCancelled = True

            lReturn = ResetFormDetails()

            ' Minimise the form.
            Me.WindowState = FormWindowState.Minimized

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdLogOff_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLogOff.Click

        ' Click Event Of The LogOff Button.

        Try

            ' Unloads the form, which then destorys
            ' the public instance of the logon manager,
            ' causing this object to logoff from the
            ' licence manager (If now other objects have
            ' an instance of the logon manager).

            Me.Close()

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Log off command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdLogOff_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        Dim lErrorValue As Integer

        ' If we still have a Reference to Logon Manager
        If Not (g_oLogonManager Is Nothing) Then
            ' Check whether other applications are running.
            If Not CheckProcessBeforeExit() Then
                MessageBox.Show("There are other Pure applications running." & Strings.Chr(13) & Strings.Chr(10) & _
                                "Close them all before logging off." & Strings.Chr(13) & Strings.Chr(10) & _
                                "If the applications are no longer visible, " & Strings.Chr(13) & Strings.Chr(10) & "use Windows Task Manager to close them.", "E0101 - Unable to Logoff", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Cancel = True
            Else
                If (Process.GetProcessesByName("Ilogonserver").Length >= 1) Then
                    'Dim myprocess() As Process = 
                    'Process.GetProcessesByName("Ilogonserver")(0).Kill()
                    Dim myprocess() As Process = Process.GetProcessesByName("Ilogonserver")
                    For index As Integer = 0 To myprocess.Length - 1
                        If myprocess(index).SessionId = Process.GetCurrentProcess.SessionId Then
                            g_oLogonManager.Dispose()
                            Process.GetProcessesByName("Ilogonserver")(index).Kill()
                        End If
                    Next
                    'myprocess(0).Kill()
                End If
            End If
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        Dim lErrorValue As Integer

        ' Forms Terminate Event.

        Try

            ' Call the terminate method
            m_oGeneral.Dispose()
            ' Destroy the instance of the general
            ' interface object from memory.
            m_oGeneral = Nothing

            ' Destorys the public instance of the logon manager,
            ' causing this object to logoff from the licence
            ' manager (If now other objects have an instance of
            ' the logon manager).
            g_oLogonManager = Nothing

        Catch excep As System.Exception



            ' Error Section.

            ErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Unload", excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' RFC 19081998 - Help About SA Added.
    Public Sub mnuAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuAbout.Click


        Dim sVersionNumber, sComponent As String

        ' Set the application title
        Dim sTitle As String = "Sirius Architecture"

        'RFC170299 - Get the Version from the Registry
        Dim lReturn As gPMConstants.PMEReturnCode = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.PMRegKeyVersion, r_sSettingValue:=sVersionNumber), gPMConstants.PMEReturnCode)

        ' Set the version number and date
        Dim sVersionDate As String = ACArchitectureDate
        'sComponent = "User Logon Details"

        ' Create the object
        Dim oPMAbout As New iPMAbout.Interface_Renamed

        ' Initialise it. No parameters
        'lReturn = CType(oPMAbout, SSP.S4I.Interfaces.ILocalInterface).Initialise() 
        lReturn = oPMAbout.Initialise()

        ' Display the about screen modally
        lReturn = oPMAbout.Show(sTitle:=sTitle, sVersionNumber:=sVersionNumber, sVersionDate:=sVersionDate, sComponent:=sComponent)

        ' Terminate it, and...
        oPMAbout.Dispose()

        ' ...remove it from memory
        oPMAbout = Nothing

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtConfirmPassword_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConfirmPassword.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'set the password changed property to true
        PasswordChanged = True

    End Sub

    Private Sub txtLogFilename_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLogFilename.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        ' Enable the Apply Button
        cmdApplyLogDetails.Enabled = True

    End Sub

    Private Sub txtNewPassword_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNewPassword.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'set the password changed property to true
        PasswordChanged = True

    End Sub

    Private Sub txtOldPassword_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOldPassword.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'set the password changed property to true
        PasswordChanged = True

    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.G Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.P Then
            tabMainTab.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.U Then
            tabMainTab.SelectedIndex = 2
        End If
    End Sub
    Private Sub PopulateProcessList()
        Try
            lstProcess.Clear()
            lstProcess.Add("iPMLock")
            lstProcess.Add("iPMUserMaintenance")
            lstProcess.Add("PMCurrencyMaintenance")
            lstProcess.Add("PMGroupMaintenance")
            lstProcess.Add("PMMaintainLookup")
            lstProcess.Add("PMMessageAdmin")
            lstProcess.Add("PMSourceMaintenance")
            lstProcess.Add("PMSystem")
            lstProcess.Add("PMTaskGroupMaintenance")
            lstProcess.Add("PMWorkManager")
            lstProcess.Add("iDOCManager")
            lstProcess.Add("TestLogon")
            lstProcess.Add("iPMClientInstallAdmin")
            lstProcess.Add("PMProductUpdateHist")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function CheckProcessBeforeExit() As Boolean
        Dim bExit As Boolean = True
        Try
            Dim arrRunningProcesses As Process() = Nothing
            For Each proc As String In lstProcess
                arrRunningProcesses = Process.GetProcessesByName(proc)
                If Not arrRunningProcesses Is Nothing Then
                    If arrRunningProcesses.Length > 0 Then
                        For Each pRunningProc As Process In arrRunningProcesses
                            If pRunningProc.SessionId = Process.GetCurrentProcess.SessionId Then
                                bExit = False
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
        Return bExit
    End Function
End Class
Public Class ValueDescriptionPair

    Private m_Value As Object
    Private m_Description As String

    Public ReadOnly Property Value() As Object
        Get
            Return m_Value
        End Get
    End Property

    Public ReadOnly Property Description() As String
        Get
            Return m_Description
        End Get
    End Property

    Public Sub New(ByVal NewValue As Object, ByVal NewDescription As String)
        m_Value = NewValue
        m_Description = NewDescription
    End Sub

    Public Overrides Function ToString() As String
        Return m_Description
    End Function

End Class


