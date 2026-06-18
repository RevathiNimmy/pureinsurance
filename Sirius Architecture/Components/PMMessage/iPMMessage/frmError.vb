Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
' developer guide no. 129
Imports SharedFiles
Partial Friend Class frmError
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmError"

    Private Const ACDateTime As Integer = 0
    Private Const ACApplication As Integer = 1
    Private Const ACModule As Integer = 2
    Private Const ACMethod As Integer = 3
    Private Const ACMessage As Integer = 4
    Private Const ACVBErrNo As Integer = 5
    Private Const ACVBErrMessage As Integer = 6
    Private Const ACUser As Integer = 7
    Private Const ACErrUniqueId As Integer = 8

    Private Const ACQuestion As String = "question"
    Private Const ACExclamation As String = "exclamation"
    Private Const ACInformation As String = "information"
    Private Const ACCritical As String = "stop"

    ' Zip file
    Private m_sZippedFile As String = ""

    ' Application
    Private m_sApplication As String = ""
    ' Module
    Private m_sModule As String = ""
    ' Method
    Private m_sMethod As String = ""
    ' Message
    Private m_sMessage As String = ""
    ' UniqueErrId
    Private m_sErrUniqueId As String = ""
    ' VBErrNo
    Private m_lVBErrNo As Integer
    ' VBErrMessage
    Private m_sVBErrMessage As String = ""
    ' User
    Private m_sUser As String = ""
    ' ErrType
    Private m_iErrType As gPMConstants.PMELogLevel

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' RDC 30072002 for event log support
    Dim m_atypEventRecords() As cPMEventLogViewer.cEventLogs.EventRecord = Nothing
    Dim m_typUserFilterData As New cPMEventLogViewer.cEventLogs.FilterData
    ' end of decs

    ' PWF 01/09/2002 Simple message?
    Private m_bSimpleError As Boolean


    Public Property Application() As String
        Get
            Return m_sApplication
        End Get
        Set(ByVal Value As String)
            m_sApplication = Value
        End Set
    End Property

    Public Property Module_Renamed() As String
        Get
            Return m_sModule
        End Get
        Set(ByVal Value As String)
            m_sModule = Value
        End Set
    End Property

    Public Property Method() As String
        Get
            Return m_sMethod
        End Get
        Set(ByVal Value As String)
            m_sMethod = Value
        End Set
    End Property

    Public Property Message() As String
        Get
            Return m_sMessage
        End Get
        Set(ByVal Value As String)
            m_sMessage = Value
        End Set
    End Property

    Public Property ErrUniqueId() As String
        Get
            Return m_sErrUniqueId
        End Get
        Set(ByVal Value As String)
            m_sErrUniqueId = Value
        End Set
    End Property

    Public Property VBErrNo() As Integer
        Get
            Return m_lVBErrNo
        End Get
        Set(ByVal Value As Integer)
            m_lVBErrNo = Value
        End Set
    End Property

    Public Property VBErrMessage() As String
        Get
            Return m_sVBErrMessage
        End Get
        Set(ByVal Value As String)
            m_sVBErrMessage = Value
        End Set
    End Property

    Public Property User() As String
        Get
            Return m_sUser
        End Get
        Set(ByVal Value As String)
            m_sUser = Value
        End Set
    End Property

    Public Property ErrType() As Integer
        Get
            Return m_iErrType
        End Get
        Set(ByVal Value As Integer)
            m_iErrType = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: PropertiesToInterface
    '
    ' Description:
    '
    ' History: 17/07/2001 CTAF - Created.
    ' JAS121104 : ISS16690 : changed Date$ to a Format field to remove US format date
    '
    ' ***************************************************************** '
    Public Function PropertiesToInterface() As Integer

        Dim result As Integer = 0
        Dim oObject As Object
        Dim lReturn As Integer
        Dim sSetting As String = ""

        Const ACApp As String = "iPMMessage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' PWF 01/09/2002 - Get error display status (unless errtype is feedback)
            m_bSimpleError = True

            If m_iErrType <> gPMConstants.PMELogLevel.PMLogFeedback Then
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="Error Expansion", r_sSettingValue:=sSetting), gPMConstants.PMEReturnCode)

                ' Set default (expandable) and check for 1 (expandable) or 2 (full)
                Dim dbNumericTemp As Double
                If Double.TryParse(sSetting, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    If StringsHelper.ToDoubleSafe(sSetting) = 1 Then
                        m_bSimpleError = False
                    End If
                End If
            End If

            ' We have a user?
            If m_sUser = "" Then
                ' Obtain it

                ' Get object manager
                oObject = CreateLateBoundObject("bObjectManager.ObjectManager")

                ' Initialise object manager
                lReturn = oObject.Initialise(sCallingAppName:=ACApp)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the user name
                m_sUser = oObject.UserName

                ' Clear up
                oObject.Dispose()
                oObject = Nothing

            End If

            ' Application
            txtError(ACApplication).Text = m_sApplication
            ' Module
            txtError(ACModule).Text = m_sModule
            ' Method
            txtError(ACMethod).Text = m_sMethod
            ' Message
            txtError(ACMessage).Text = m_sMessage
            ' Unique Error Number
            'txtError(ACErrUniqueId).Text = m_sErrUniqueId
            ' Get the Error Id Caption from SharedFiles
            _lblCapion_5.Text = gPMConstants.ERROR_LABEL
            ' VB Error Number
            'txtError(ACVBErrNo).Text = CStr(m_lVBErrNo)
            txtError(ACVBErrNo).Text = m_sErrUniqueId.Substring(gPMConstants.ERROR_LABEL.Length)
            ' VB Error Message
            txtError(ACVBErrMessage).Text = m_sVBErrMessage
            ' Date/Time
            'JAS121104 : ISS16690 : changed Date$ to a Format field to remove US format date
            txtError(ACDateTime).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, DateTime.Now) & " " & DateTime.Now.ToString("HH:mm:ss")
            ' Username
            txtError(ACUser).Text = m_sUser

            ' PWF 01/09/2002 - Set form caption (feedback is not an error!)
            If m_iErrType = gPMConstants.PMELogLevel.PMLogFeedback Then
                Me.Text = "Information"
            Else
                Me.Text = "There has been an error"
            End If


            ' PWF 01/09/2002 - Set simple error message

            Select Case True
                Case m_sMessage.Length
                    lblSimpleError.Text = m_sMessage
                Case Marshal.SizeOf(m_lVBErrNo)
                    lblSimpleError.Text = "[" & m_lVBErrNo & "] " & m_sVBErrMessage
                Case Else
                    lblSimpleError.Text = "Unhandled error in " & m_sModule & "." & m_sMethod
            End Select

            ' CTAF 240701 - Default to Information for friendliness
            ' PWF 01/09/2002 - Distinguish between error types again.
            'm_iErrType = PMLogInfo

            ' PWF 01/09/2002 - Load icons from imagelist
            Select Case m_iErrType
                Case gPMConstants.PMELogLevel.PMLogFatal, gPMConstants.PMELogLevel.PMLogError
                    sSetting = ACCritical

                Case gPMConstants.PMELogLevel.PMLogWarning, gPMConstants.PMELogLevel.PMLogOnError
                    sSetting = ACExclamation

                Case gPMConstants.PMELogLevel.PMLogInfo, gPMConstants.PMELogLevel.PMLogDebug1, gPMConstants.PMELogLevel.PMLogDebug2, gPMConstants.PMELogLevel.PMLogDebug3, gPMConstants.PMELogLevel.PMLogFeedback
                    sSetting = ACInformation

                Case Else
                    sSetting = ACInformation
            End Select

            ' PWF 01/09/2002 - Must use ExtractIcon to get correct picture format

            'developer guide no.171
            imgMain.Image = imlIcons.Images.Item(sSetting)

            imgSimple.Image = imlIcons.Images.Item(sSetting)

            ' PWF 01/09/2002 - Set appropriate display state
            SetInterfaceStatus()

            Return result

        Catch




            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    Private Function SetInterfaceStatus() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SetInterfaceStatus
        ' PURPOSE: Set appropriate display state for message
        ' AUTHOR: Peter Finney
        ' DATE: 02-Oct-02, 10:30 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set visible states
            cmdLog.Visible = (m_iErrType <> gPMConstants.PMELogLevel.PMLogFeedback)
            cmdMore.Visible = (m_iErrType <> gPMConstants.PMELogLevel.PMLogFeedback) And m_bSimpleError

            fraMain.Visible = Not m_bSimpleError
            fraSimple.Visible = m_bSimpleError

            ' Set sizes & positions
            If m_bSimpleError Then
                cmdLog.Top = fraSimple.Height + VB6.TwipsToPixelsY(135)
                cmdMore.Top = fraSimple.Height + VB6.TwipsToPixelsY(135)
                cmdOK.Top = fraSimple.Height + VB6.TwipsToPixelsY(135)
                Me.Height = fraSimple.Height + VB6.TwipsToPixelsY(915)
            Else
                cmdLog.Top = fraMain.Height + VB6.TwipsToPixelsY(135)
                cmdMore.Top = fraMain.Height + VB6.TwipsToPixelsY(135)
                cmdOK.Top = fraMain.Height + VB6.TwipsToPixelsY(135)
                Me.Height = fraMain.Height + VB6.TwipsToPixelsY(915)
            End If


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
            End Select

        Finally


        End Try
        Return result

    End Function


    Private Function ClearUp() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clears up all of the ~~~.txt and .zip files
            If FileSystem.Dir(ACClientLog, FileAttribute.Normal) <> "" Then
                File.Delete(ACClientLog)
            End If

            If FileSystem.Dir(ACServerLog, FileAttribute.Normal) <> "" Then
                File.Delete(ACServerLog)
            End If

            If FileSystem.Dir(ACCobolLog, FileAttribute.Normal) <> "" Then
                File.Delete(ACCobolLog)
            End If

            If FileSystem.Dir(ACRegistry, FileAttribute.Normal) <> "" Then
                File.Delete(ACRegistry)
            End If

            If FileSystem.Dir(m_sZippedFile, FileAttribute.Normal) <> "" Then
                File.Delete(m_sZippedFile)
            End If

            Return result

        Catch




            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub cmdLog_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLog.Click

        m_lReturn = CType(EmailLogs(), gPMConstants.PMEReturnCode)

        m_lReturn = CType(ClearUp(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdMore_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMore.Click

        ' Alter error expansion flag and refresh dialog
        m_bSimpleError = False
        SetInterfaceStatus()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Close the form
        Me.Hide()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: SaveToFile
    '
    ' Description:
    '
    ' History: 18/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SaveToFile(ByVal v_sFileName As String, ByVal v_sText As String) As Integer

        Dim result As Integer = 0
        Dim lFile As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lFile = FileSystem.FreeFile()

            FileSystem.FileOpen(lFile, v_sFileName, OpenMode.Output)

            FileSystem.PrintLine(lFile, v_sText)

            FileSystem.FileClose(lFile)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveToFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveToFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetClientLogFile
    '
    ' Description:
    '
    ' History: 19/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetClientLogFile(ByVal v_sUsername As String, ByRef r_sClientLog As String) As Integer

        Dim result As Integer = 0
        Dim sLogFile As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Read registry location
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="LogFileName", r_sSettingValue:=sLogFile), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the file exists
            If FileSystem.Dir(sLogFile, FileAttribute.Normal) = "" Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Encrypt and return it
            m_lReturn = CType(EncryptLogFile(v_sLogFile:=sLogFile, r_sEncryptedLogFile:=r_sClientLog), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientLogFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientLogFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SendEmail
    '
    ' Description:
    '
    ' History: 19/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SendEmail(ByVal v_bHasClient As Boolean, ByVal v_bHasServer As Boolean, ByVal v_bHasCobol As Boolean) As Integer

        'Dim result As Integer = 0
        'Dim oMAPI As iPMMapi.PMMAPI
        'Dim sSubject, sMessage As String
        'Dim lStatus As gPMConstants.PMEReturnCode
        'Dim sRcpt As String = ""
        'Dim iLoop1 As Integer
        'Dim oForm As frmEmail
        'Dim oZipper As bPMZipper.Business
        'Dim sSystem As String = ""
        '' RDC 27092001
        'Dim sError As String = ""

        'Try 

        '	result = gPMConstants.PMEReturnCode.PMTrue

        '	oForm = New frmEmail()

        '	With oForm
        '		.HasClientLog = v_bHasClient
        '		.HasServerLog = v_bHasServer
        '		.HasCobolLog = v_bHasCobol
        '	End With

        '	' Load the form


        '	' Show the form
        '	oForm.ShowDialog()

        '	' Get properties
        '	With oForm
        '		lStatus = .Status
        '		sMessage = .Message
        '		sRcpt = .Rcpt
        '		sSubject = .Subject
        '	End With

        '	If txtError(4).Text.Trim() = "" Then
        '		txtError(4).Text = "Failure"
        '	End If

        '	' RDC 27092001 Add properties from the pop-up form  START
        '	sError = txtError(4).Text & ", in " & txtError(1).Text & "." & txtError(2).Text & "." & txtError(3).Text & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

        '	sError = sError & "User: " & txtError(7).Text & Strings.Chr(13) & Strings.Chr(10)
        '	sError = sError & "Date/Time: " & CDate(txtError(0).Text).ToString("dd-MMM-yyyy HH:mm:ss") & Strings.Chr(13) & Strings.Chr(10)
        '	sError = sError & "Application: " & txtError(1).Text & Strings.Chr(13) & Strings.Chr(10)
        '	sError = sError & "Class: " & txtError(2).Text & Strings.Chr(13) & Strings.Chr(10)
        '	sError = sError & "Method: " & txtError(3).Text & Strings.Chr(13) & Strings.Chr(10)
        '	sError = sError & "Error message: " & txtError(4).Text & Strings.Chr(13) & Strings.Chr(10)
        '	sError = sError & "VB error number: " & txtError(5).Text & Strings.Chr(13) & Strings.Chr(10)
        '	sError = sError & "VB error message: " & txtError(6).Text & Strings.Chr(13) & Strings.Chr(10)

        '	' provide 'something' if user specified nothing
        '	If sMessage.Trim() = "" Then
        '		sMessage = "(User has not provided any additional information)"
        '	End If

        '	' format complete message
        '	sMessage = sError & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "User message:" & Strings.Chr(13) & Strings.Chr(10) & sMessage
        '	' RDC 27092001  END

        '	' Unload the form
        '	oForm.Close()

        '	oForm = Nothing

        '	' Cancelled or what?
        '	If lStatus = gPMConstants.PMEReturnCode.PMCancel Then
        '		Return result
        '	End If

        '	' Create an instance of PMMAPI
        '	oMAPI = New iPMMapi.PMMAPI()

        '	If oMAPI.Session Is Nothing Then

        '		m_lReturn = CType(oMAPI, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '			Return gPMConstants.PMEReturnCode.PMFalse
        '		End If

        '	End If

        '	' Add a message to the messages collection
        '	m_lReturn = oMAPI.Messages.AddMessage(v_vSubject:=sSubject, v_vNoteText:=sMessage)
        '	If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '		Return gPMConstants.PMEReturnCode.PMFalse
        '	End If

        '	' Add the recipient
        '	m_lReturn = oMAPI.Messages.LastItem().Recipients.AddRecipient(v_vName:=sRcpt, v_vRecipientType:=gPMConstants.PMEMapiRecipientTypes.pmeMapiToList, v_vAddressBook:=True)
        '	If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '		Return gPMConstants.PMEReturnCode.PMFalse
        '	End If

        '	' Get an instance of the zipped object
        '	oZipper = New bPMZipper.Business()

        '	' Get the name of the machine
        '	m_lReturn = CType(gPMFunctions.GetSystemName(sSystemName:=sSystem), gPMConstants.PMEReturnCode)
        '	If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '		' This machine's possessed I tell ya!
        '		sSystem = "(unknown)"
        '	End If

        '	' Create a "unique" filename for the zip file
        '	m_sZippedFile = "C:\Log" & sSystem & CDate(DateTime.Today.ToString("MM-dd-yyyy")).ToString("ddMMyyyy") & CDate(DateTime.Now.ToString("HH:mm:ss")).ToString("HHmmss") & ".zip"

        '	' Registry
        '	m_lReturn = CType(oZipper.addFileToZIP(szZIP:=m_sZippedFile, szFile:=ACRegistry), gPMConstants.PMEReturnCode)

        '	' Check for attachments
        '	If v_bHasClient Then

        '		' Zip it
        '		m_lReturn = CType(oZipper.addFileToZIP(szZIP:=m_sZippedFile, szFile:=ACClientLog), gPMConstants.PMEReturnCode)

        '	End If

        '	If v_bHasServer Then

        '		' Zip it
        '		m_lReturn = CType(oZipper.addFileToZIP(szZIP:=m_sZippedFile, szFile:=ACServerLog), gPMConstants.PMEReturnCode)

        '	End If

        '	If v_bHasCobol Then

        '		' Zip it
        '		m_lReturn = CType(oZipper.addFileToZIP(szZIP:=m_sZippedFile, szFile:=ACCobolLog), gPMConstants.PMEReturnCode)

        '	End If

        '	' Attach it
        '	m_lReturn = oMAPI.Messages.LastItem().Attachments.AddAttachment(v_vName:=m_sZippedFile, v_vPath:=m_sZippedFile)

        '	oZipper = Nothing

        '	' Send the email
        '	m_lReturn = oMAPI.Messages.LastItem().Send()

        '	' Clear up
        '	oMAPI = Nothing

        '	' Tell the user its been sent
        '	MessageBox.Show("Your email has been sent to Sirius Support.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Information)

        '	Return result

        'Catch excep As System.Exception



        '	result = gPMConstants.PMEReturnCode.PMError

        '	' Log Error Message
        '	LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendEmail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendEmail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        '	Return result




        '	Return result
        'End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EmailLogs
    '
    ' Description: Emails the logs to sirius support
    '
    ' History:  18/07/2001 CTAF - Created.
    '           30/07/2002 RDC - modify to get event logs
    ' ***************************************************************** '
    Public Function EmailLogs() As Integer

        Dim result As Integer = 0
        Dim bCanGetServerDetails, bHasClient, bHasServer, bHasCobol As Boolean

        Dim iUserID, iSourceID As Integer

        Dim sUsername, sServerName, sServerLog, sClientLog, sCobolLog, sMsgLogging As String

        Dim oBusiness As Object
        Dim oObject As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        ' RDC 30082002 messages to event log or text files?
        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyUseEventLog, r_sSettingValue:=sMsgLogging), gPMConstants.PMEReturnCode)

        If sMsgLogging <> "1" Then
            sMsgLogging = "0"
        End If

        ' try to get ObjMgr and bPMMessage.Business
        m_lReturn = CType(GetBusiness(oObject, oBusiness), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' we can only get client logs and registry
            bCanGetServerDetails = False
        Else
            ' we can get client & server logs and registries
            bCanGetServerDetails = True
        End If

        ' initialise flags
        bHasServer = False
        bHasClient = False
        bHasCobol = False

        ' initialse file strings
        sClientLog = ""
        sServerLog = ""
        sCobolLog = ""

        ' **************************** SERVER ***********************************

        If bCanGetServerDetails Then

            sUsername = oObject.UserName
            iSourceID = oObject.SourceID
            iUserID = oObject.UserID

            ' choose between text file logs and o/s application event log
            If sMsgLogging = "0" Then
                ' get text logs

                m_lReturn = oBusiness.GetLogFile(r_sLogFile:=sServerLog)
            Else
                ' need server name to get its event log

                m_lReturn = oBusiness.GetSystemName(sServerName)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' we've got the server system name
                    m_lReturn = CType(GetEventLog(sServerName, sServerLog), gPMConstants.PMEReturnCode)
                End If
            End If

            ' server-side cobol log

            m_lReturn = oBusiness.GetCobolLogFile(v_iUserID:=iUserID, v_iSourceID:=iSourceID, r_sLogFile:=sCobolLog)

        End If

        If sServerLog.Length > 0 Then

            ' Save it to disk
            m_lReturn = CType(SaveToFile(v_sFileName:=ACServerLog, v_sText:=sServerLog), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bHasServer = True
            End If

        End If

        If sCobolLog.Length > 0 Then

            ' Save it to disk
            m_lReturn = CType(SaveToFile(v_sFileName:=ACCobolLog, v_sText:=sCobolLog), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bHasCobol = True
            End If

        End If

        If bCanGetServerDetails Then

            ' Clear up server connection

            oBusiness.Dispose()
            oBusiness = Nothing

            oObject.Dispose()
            oObject = Nothing

        End If

        ' **************************** CLIENT ***********************************

        ' choose between text file logs and o/s application event log
        If sMsgLogging = "1" Then
            m_lReturn = CType(GetEventLog("", sClientLog), gPMConstants.PMEReturnCode)
        Else
            ' This is late bound for a reason
            Try
                oObject = CreateLateBoundObject("bObjectManager.ObjectManager")

            Catch
            End Try



            ' Check it was created
            If oObject Is Nothing Then
                MessageBox.Show("Failed to get an instance of ClientManager.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Initialise it
            m_lReturn = oObject.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the username and sourceid
            sUsername = oObject.UserName
            iSourceID = oObject.SourceID
            iUserID = oObject.UserID

            oObject.Dispose()
            oObject = Nothing

            ' Get the client log file
            m_lReturn = CType(GetClientLogFile(v_sUsername:=sUsername, r_sClientLog:=sClientLog), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

        End If

        If sClientLog.Length > 0 Then

            ' Save it to disk
            m_lReturn = CType(SaveToFile(v_sFileName:=ACClientLog, v_sText:=sClientLog), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bHasClient = True
            End If

        End If

        ' **************************** REGISTRY ********************************

        ' local registry
        m_lReturn = CType(DumpReg(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' **************************** EMAIL ***********************************

        ' Create the email
        m_lReturn = CType(SendEmail(v_bHasClient:=bHasClient, v_bHasServer:=bHasServer, v_bHasCobol:=bHasCobol), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

Err_EmailLogs:


        Return gPMConstants.PMEReturnCode.PMError




        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetBusiness
    '
    ' Description: used by EmailLogs to get ObjMgr and bPMMessage
    '
    ' History: RDC 30072002 Created
    '
    ' ***************************************************************** '
    Private Function GetBusiness(ByRef oObject As Object, ByRef oBusiness As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oObject = CreateLateBoundObject("bObjectManager.ObjectManager")

            If oObject Is Nothing Then
                MessageBox.Show("Failed to get instance of Object Manager", "iPMMessage.frmError.GetBusiness", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Return result
            End If

            m_lReturn = oObject.Initialise(sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = Nothing

                MessageBox.Show("Failed to initialise Object Manager", "iPMMessage.frmError.GetBusiness", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            Dim temp_oBusiness As Object
            m_lReturn = oObject.GetInstance(temp_oBusiness, "bPMMessage.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject.Dispose()

                oObject = Nothing

                MessageBox.Show("Failed to create instance of bPMMessage.Business", "iPMMessage.frmError.GetBusiness", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            result = gPMConstants.PMEReturnCode.PMError


            If Not (oObject Is Nothing) Then
                oObject.Dispose()
                oObject = Nothing
            End If

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetEventLog
    '
    ' Description:
    '
    ' History: RDC 30072002 created
    '
    ' ***************************************************************** '
    Private Function GetEventLog(ByVal sMachineName As String, ByRef sLogStore As String) As Integer

        Dim result As Integer = 0
        Dim blnFilterInDll As Boolean

        Dim lEventCount, lNumActualRecs, lNumRecs As Integer

        Dim sTemp, sEncrypted As String

        Dim oEventLog As cPMEventLogViewer.cEventLogs




        result = gPMConstants.PMEReturnCode.PMFalse

        oEventLog = New cPMEventLogViewer.cEventLogs()

        If oEventLog Is Nothing Then
            MessageBox.Show("Failed to create instance of cPMEventLogViewer.cEventLogs", "iPMMessage.frmError.GetEventLog", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check the number of records to read
        lNumRecs = -1

        ' Set data over 256 bytes *NOT* to be converted to Hex
        oEventLog.EventDataReturnHex = False
        oEventLog.EventTypeLog = "Application"

        ' Get the settings from the form
        Try

            oEventLog.OpenAnyEventLog(sMachineName)

        Catch
            oEventLog = Nothing

            If sMachineName = "" Then
                sMachineName = "local"
            End If

            MessageBox.Show("Failed to open event log on '" & sMachineName & "'", "iPMMessage.frmError.GetEventLog", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try





        ' Set the filter
        If blnFilterInDll Then
            oEventLog.EventFilter = m_typUserFilterData
        End If

        ' Read the event log
        Information.Err().Clear()

        Try

            lNumActualRecs = oEventLog.ReadEventEntries(xi_blnFilterEvents:=blnFilterInDll, xi_lngNumRecsToRead:=lNumRecs)

        Catch
            oEventLog = Nothing

            If sMachineName = "" Then
                sMachineName = "local"
            End If

            MessageBox.Show("Failed to read event log on machine '" & sMachineName & "'", "iPMMessage.frmError.GetEventLog", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try





        ' Get the record count
        lEventCount = oEventLog.CountEventRecords

        If lEventCount = 0 Then
            oEventLog = Nothing
            Return result
        End If

        sLogStore = ""

        ' Cycle thru the properties in the collection in the OLE server
        If lNumActualRecs > 0 Then

            For lLoop As Integer = 1 To lNumActualRecs

                If oEventLog.EventSourceName(lLoop) = "Sirius" Then

                    ' get an entry from the event log
                    sTemp = ""

                    With oEventLog
                        sTemp = sTemp & DateTimeHelper.ToString(.EventTimeWritten(lLoop)) & "   "
                        sTemp = sTemp & DateTimeHelper.ToString(.EventTimeCreated(lLoop)) & "   "
                        sTemp = sTemp & .EventSourceName(lLoop) & "   "
                        sTemp = sTemp & .EventUserSID(lLoop) & "   "
                        sTemp = sTemp & .EventComputerName(lLoop) & "   "
                        sTemp = sTemp & .EventType(lLoop) & "   "
                        sTemp = sTemp & .EventDescription(lLoop) & "   "
                        sTemp = sTemp & .EventData(lLoop) & "   "
                        sTemp = sTemp & .EventDataText(lLoop) & "   "
                        sTemp = sTemp & CStr(.EventCategory(lLoop)) & "   "
                        sTemp = sTemp & .EventCategoryString(lLoop) & "   "
                        sTemp = sTemp & CStr(.EventRecordNum(lLoop)) & "   "
                        sTemp = sTemp & CStr(.EventID(lLoop)) & Strings.Chr(13) & Strings.Chr(10)
                    End With

                    ' encrypt it
                    m_lReturn = CType(EncryptString(sTemp, sEncrypted), gPMConstants.PMEReturnCode)

                    ' add it to the log store
                    sLogStore = sLogStore & sEncrypted

                End If
            Next
        End If

        oEventLog = Nothing


        Return gPMConstants.PMEReturnCode.PMTrue

Err_GetEventLog:

        Return gPMConstants.PMEReturnCode.PMError


    End Function

    ' ***************************************************************** '
    '
    ' Name: EncryptString
    '
    ' Description:
    '
    ' History: 18/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function EncryptString(ByVal v_sInString As String, ByRef r_sOutString As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sOutString = ""

            For Each sByte_Renamed As Char In v_sInString
                ' Get the next byte
                ' Encrypt it with the shaolin magic key
                sByte_Renamed = Strings.Chr(Strings.Asc(sByte_Renamed) Xor &HCFS).ToString()
                ' Add it
                r_sOutString = r_sOutString & sByte_Renamed
            Next sByte_Renamed

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".EncryptString")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncryptString Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncryptString", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EncryptLogFile
    '
    ' Description: Update this in the interface and business
    '
    ' History: 18/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function EncryptLogFile(ByVal v_sLogFile As String, ByRef r_sEncryptedLogFile As String, Optional ByVal v_bSiriusLog As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim lFile As Integer
        Dim sTemp, sLine As String
        Dim vTempArray As Object
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lFile = FileSystem.FreeFile()

            ' Load the last few lines
            FileSystem.FileOpen(lFile, v_sLogFile, OpenMode.Input)

            sTemp = ""
            sLine = ""

            vTempArray = Nothing

            While Not FileSystem.EOF(lFile)

                sLine = FileSystem.LineInput(lFile)

                If v_bSiriusLog Then

                    ' RDC 01112001 search for date dd/mm/yy or dd/mm/yyyy
                    If (sLine.Substring(11, 1) = ":") And (sLine.Substring(14, 1) = ":") Or (sLine.Substring(13, 1) = ":") And (sLine.Substring(16, 1) = ":") Or sLine.IndexOf("Date / Time") >= 0 Then
                        ' New line
                        If Information.IsArray(vTempArray) Then

                            ReDim Preserve vTempArray(vTempArray.GetUpperBound(0) + 1)
                        Else
                            ReDim vTempArray(0)
                        End If


                        vTempArray(vTempArray.GetUpperBound(0)) = sLine
                    Else



                        vTempArray(vTempArray.GetUpperBound(0)) = CStr(vTempArray(vTempArray.GetUpperBound(0))) & Environment.NewLine & sLine
                    End If

                Else
                    ' New line
                    If Information.IsArray(vTempArray) Then



                        vTempArray(vTempArray.GetUpperBound(0)) = CStr(vTempArray(vTempArray.GetUpperBound(0))) & Environment.NewLine & sLine
                    Else
                        ReDim vTempArray(0)

                        vTempArray(0) = sLine
                    End If

                End If

            End While

            FileSystem.FileClose(lFile)

            r_sEncryptedLogFile = ""

            ' Encrypt them

            For iLoop1 As Integer = vTempArray.GetUpperBound(0) To (vTempArray.GetUpperBound(0) - ACReturnLines) Step -1

                If iLoop1 < 0 Then
                    Exit For
                End If


                sTemp = CStr(vTempArray(iLoop1)) & Environment.NewLine

                lReturn = EncryptString(v_sInString:=sTemp, r_sOutString:=sLine)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Return the value
                r_sEncryptedLogFile = r_sEncryptedLogFile & sLine

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            ' RDC 01112001 add file name to message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncryptLogFile Failed on file '" & v_sLogFile & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="EncryptLogFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Private Function DumpSection(ByVal Group As Integer, ByRef sSection As String, ByRef lHandle As Integer) As Integer

        Dim sKey As String = ""

        Dim lLoop1 As Integer = 0

        Dim vRes As Object = ADVReg.ReadRegistryGetAll(gPMConstants.HKEY_LOCAL_MACHINE, sSection, lLoop1)


        Do Until CStr(vRes(2)) = "Not Found"

            Select Case Group
                Case gPMConstants.HKEY_LOCAL_MACHINE
                    sKey = "HKEY_LOCAL_MACHINE\"
                Case gPMConstants.HKEY_CURRENT_USER
                    sKey = "HKEY_CURRENT_USER\"
                Case gPMConstants.HKEY_USERS
                    sKey = "HKEY_USERS\"
                Case gPMConstants.HKEY_CLASSES_ROOT
                    sKey = "HKEY_CLASSES_ROOT\"
            End Select



            FileSystem.PrintLine(lHandle, sKey & sSection & "\" & CStr(vRes(1)) & " = " & CStr(vRes(2)))

            lLoop1 += 1

            vRes = ADVReg.ReadRegistryGetAll(gPMConstants.HKEY_LOCAL_MACHINE, sSection, lLoop1)

        Loop

        If lLoop1 > 0 Then
            FileSystem.PrintLine(lHandle, "")
        End If

    End Function

    Private Function DumpRegistry(ByVal Group As Integer, ByRef Section As String, ByRef lHandle As Integer) As Integer

        Dim lLoop1 As Integer
        Dim sNewSection As String = ""

        Dim l As Integer = DumpSection(Group, Section, lHandle)

        Dim vRes As String = ADVReg.ReadRegistryGetSubkey(Group, Section, lLoop1)

        Do Until vRes = "Not Found"

            sNewSection = Section & "\" & vRes

            l = DumpRegistry(Group, sNewSection, lHandle)

            lLoop1 += 1
            vRes = ADVReg.ReadRegistryGetSubkey(Group, Section, lLoop1)

        Loop

    End Function

    ' ***************************************************************** '
    '
    ' Name: DumpRegistry
    '
    ' Description:
    '
    ' History: 19/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function DumpReg() As Integer

        Dim result As Integer = 0
        Dim lHandle As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lHandle = FileSystem.FreeFile()

            FileSystem.FileOpen(lHandle, ACRegistry, OpenMode.Output)

            FileSystem.PrintLine(lHandle, "PM Registry Settings - " & DateTimeHelper.ToString(DateTime.Now))
            FileSystem.PrintLine(lHandle, "")

            m_lReturn = CType(DumpRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\PM", lHandle), gPMConstants.PMEReturnCode)
            m_lReturn = CType(DumpRegistry(gPMConstants.HKEY_CURRENT_USER, "SOFTWARE\PM", lHandle), gPMConstants.PMEReturnCode)

            FileSystem.FileClose(lHandle)

            Return result

        Catch




            Return gPMConstants.PMEReturnCode.PMError




            Return result
        End Try
    End Function
End Class
