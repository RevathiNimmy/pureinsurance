Option Strict Off
Option Explicit On
Imports System.Text
Imports Aspose.Email
Imports Aspose.Email.Clients.Smtp
Imports Microsoft.VisualBasic.Compatibility.VB6
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    'Standard Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Email Address
    Private m_lPartyCnt As Integer
    Private m_sEmailAddress As String = ""
    Private m_sEmailSubject As String = ""
    Private m_sEmailText As String = ""
    Private m_sEmailAttachment As String = ""

    Private m_oSharePoint As Object
    Private m_lTemplateGroupID As Integer
    Private m_lTemplateSubGroupID As Integer
    Private m_bIsInternalOnly As Boolean

    'Business Component

    Private m_oBusiness As bSIREmail.Business
    'Email Component
    Private m_oPMMAPI As Object

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property
    Public WriteOnly Property Business() As bSIREmail.Business
        Set(ByVal Value As bSIREmail.Business)

            m_oBusiness = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property
    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)

            ' Set the objects parameter value.
            m_lPartyCnt = Value

        End Set
    End Property
    Public Property EmailAddress() As String
        Get

            ' Return the Email Address
            Return m_sEmailAddress
        End Get
        Set(ByVal Value As String)

            ' Set the objects parameter value.
            m_sEmailAddress = Value

        End Set
    End Property
    Public WriteOnly Property EmailSubject() As String
        Set(ByVal Value As String)

            ' Set the objects parameter value.
            m_sEmailSubject = Value

        End Set
    End Property
    Public WriteOnly Property EmailText() As String
        Set(ByVal Value As String)

            ' Set the objects parameter value.
            m_sEmailText = Value

        End Set
    End Property
    Public WriteOnly Property EmailAttachment() As String
        Set(ByVal Value As String)

            ' Set the objects parameter value.
            m_sEmailAttachment = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (END) *}

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' {* USER DEFINED CODE (Begin) *}
    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Private Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Get the interface details from the
            ' business object.
            If m_sEmailAddress = "" Then
                m_lReturn = CType(GetEmailAddress(), gPMConstants.PMEReturnCode)
            End If
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details from the business object
            ' to the interface.
            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetEmailAddress() As Integer

        Dim result As Integer = 0
        Dim vContacts As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetAddressContactDetails(vPartyCnt:=m_lPartyCnt, vContacts:=vContacts)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Information.IsArray(vContacts) Then

                m_lReturn = CType(GetEmail(vContacts:=vContacts), gPMConstants.PMEReturnCode)
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEmailAddress")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEmailAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Takes Data from Business & Set KEys and populates Form
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        txtTo.Text = m_sEmailAddress
        txtSubject.Text = m_sEmailSubject
        txtNote.Text = m_sEmailText
        txtFile.Text = m_sEmailAttachment

        Return result
    End Function
    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: GetMail
    '
    ' Description: Fills the grid control with contact details
    '
    ' ***************************************************************** '
    Private Function GetEmail(ByRef vContacts(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sTemp As New StringBuilder

        result = gPMConstants.PMEReturnCode.PMTrue

        Try


            ' Assign the details to the interface.
            For i As Integer = vContacts.GetLowerBound(1) To vContacts.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}


                sTemp = New StringBuilder(CStr(vContacts(1, i)).Trim() & " " & CStr(vContacts(2, i)).Trim())


                If CStr(vContacts(3, i)).Trim() <> "" Then

                    sTemp.Append(" ext: " & CStr(vContacts(3, i)).Trim())
                End If
                sTemp = New StringBuilder(sTemp.ToString().Trim())


                Select Case CStr(vContacts(5, i)).Trim()
                    'Correspondence Address
                    Case gSIRLibrary.SIRMainAddressABICode

                        Select Case CStr(vContacts(4, i)).Trim()
                            Case "E-MAIL"

                                m_sEmailAddress = CStr(vContacts(2, i))
                        End Select

                End Select

                ' {* USER DEFINED CODE (End) *}

            Next i

            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetEmail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Private Sub SendMessage()

        Dim result As Integer = 0
        Dim sSMTPEmailServer As String, sSMTPEmailPort As String, sSMTPEmailFrom As String, sSMTPEmailBCC As String, sClientDocumentFolder As String
        Dim sFailMsg As String = ""

        Const SMTP_Email_Server As Integer = 5045
        Const SMTP_Email_Port As Integer = 5046
        Const SMTP_Email_From As Integer = 5047
        Const SMTP_Email_BCC As Integer = 5094
        Const SHAREPOINT_PATH As Integer = 5085

        Dim sSMTP_Email_To As String = txtTo.Text
        Dim sSMTP_Email_Subject As String = txtSubject.Text
        Dim sSMTP_Email_Body As String = txtNote.Text
        Dim sSMTP_Email_Attachment As String = txtFile.Text.Trim()
        Dim sOption As String = ""
        Dim sSharePointServer_Path As String = ""
        Dim sEMLFile As String = ""

        Const kMethodName As String = "SendEMail"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get System Option (SMTP Email From)
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=MainModule.g_oObjectManager.UserName, v_sPassword:=MainModule.g_oObjectManager.Password, v_iUserID:=g_iUserID, v_iMainSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=MainModule.g_oObjectManager.CurrencyID, v_iLogLevel:=MainModule.g_oObjectManager.LogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_From, r_sOptionValue:=sSMTPEmailFrom, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get the System option SMTP_Email_From"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            If sSMTPEmailFrom = "" Then
                ' option not populated, no action
                sFailMsg = "Invalid SMTP Email From"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            ' Get System Option (SMTP Email BCC)
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=MainModule.g_oObjectManager.UserName, v_sPassword:=MainModule.g_oObjectManager.Password, v_iUserID:=g_iUserID, v_iMainSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=MainModule.g_oObjectManager.CurrencyID, v_iLogLevel:=MainModule.g_oObjectManager.LogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_BCC, r_sOptionValue:=sSMTPEmailBCC, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get the System option SMTP_Email_BCC"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            ' Get System Option (SMTP Email Server)
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=MainModule.g_oObjectManager.UserName, v_sPassword:=MainModule.g_oObjectManager.Password, v_iUserID:=g_iUserID, v_iMainSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=MainModule.g_oObjectManager.CurrencyID, v_iLogLevel:=MainModule.g_oObjectManager.LogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_Server, r_sOptionValue:=sSMTPEmailServer, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get the System option SMTP_Email_Server"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            If sSMTPEmailServer = "" Then
                ' option not populated, no action
                sFailMsg = "Invalid SMTP Email server"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            ' Get System Option (SMTP Email Port)
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=MainModule.g_oObjectManager.UserName, v_sPassword:=MainModule.g_oObjectManager.Password, v_iUserID:=g_iUserID, v_iMainSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=MainModule.g_oObjectManager.CurrencyID, v_iLogLevel:=MainModule.g_oObjectManager.LogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_Port, r_sOptionValue:=sSMTPEmailPort, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get the System option SMTP_Email_Port"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            If sSMTPEmailPort = "" Then
                ' option not populated, no action
                sFailMsg = "Invalid SMTP Port"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            'Instantiate the License class
            Dim license As Aspose.Email.License = New Aspose.Email.License()
            license.SetLicense("Aspose.Totalfor.NET.lic")

            Dim MailClient As SmtpClient = New SmtpClient(sSMTPEmailServer, CInt(sSMTPEmailPort))
            Dim SMTPMessage As MailMessage = New MailMessage(sSMTPEmailFrom, sSMTP_Email_To)

            If sSMTPEmailBCC <> "" Then
                Dim SMTPBCC As MailAddress = New MailAddress(sSMTPEmailBCC)
                SMTPMessage.Bcc.Add(SMTPBCC)
            End If

            SMTPMessage.Subject = sSMTP_Email_Subject

            ' Check if the path exists
            If Not String.IsNullOrEmpty(sSMTP_Email_Attachment) Then
                If Not FileExists(sSMTP_Email_Attachment) Then
                    sFailMsg = "Missing file / folder for e-mail body : " & sSMTP_Email_Attachment
                    Throw New Exception(sFailMsg)
                Else
                    Dim MailAttachment As Attachment = New Attachment(sSMTP_Email_Attachment)
                    SMTPMessage.Attachments.Add(MailAttachment)
                End If
            End If

            SMTPMessage.Body = sSMTP_Email_Body
            MailClient.Send(SMTPMessage)

            'Save a copy of the EML message file (for archiving)
            If sSMTP_Email_Attachment <> "" Then
                sEMLFile = IO.Path.GetPathRoot(sSMTP_Email_Attachment) & "\" & IO.Path.GetFileNameWithoutExtension(sSMTP_Email_Attachment) & ".EML"
            Else
                sClientDocumentFolder = CStr(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Pure\PureInstallation\Client", "DocClient"))
                sEMLFile = IO.Path.GetPathRoot(sClientDocumentFolder) & "\" & "ArchiveMAIL" + Date.Now.ToString("ddMMyyyyhhmmss") + ".EML"
            End If

            SMTPMessage.Save(fileName:=sEMLFile)


            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOption, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            'Check for sharepoint
            If sOption = "2" Then

                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=SHAREPOINT_PATH, r_sOptionValue:=sSharePointServer_Path, v_iSourceID:=g_iSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sFailMsg = "SharePoint is not configured"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception(sFailMsg)
                End If

                'Sharepoint Integration
                If m_oSharePoint Is Nothing Then
                    m_oSharePoint = New bSIRSharepoint.Business()

                    m_lReturn = CType(m_oSharePoint, SSP.S4I.Interfaces.IBusiness).Initialise("", "", g_iUserID, g_iSourceID, g_iLanguageID, 0, 0, m_sCallingAppName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the m_oSharePoint object", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToSharePoint", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_oSharePoint.ArchiveDocument(PartyCnt:=m_lPartyCnt, InsuranceFileCnt:=0,
                                                  ClaimID:=0, CaseID:=0, DocumentTemplateId:=0,
                                                  TemplateGroupID:=m_lTemplateGroupID,
                                                  TemplateSubGroupID:=m_lTemplateSubGroupID,
                                                  SourceFile:=sEMLFile, InternalOnly:=m_bIsInternalOnly, SharepointPath:=sSharePointServer_Path,
                                                  DestinationFilename:="",
                                                  PartyCode:="", PolicyNumber:="",
                                                  ClaimNumber:="")
                End If
            End If


            SMTPMessage.Attachments.Dispose()
            SMTPMessage.Dispose()

        Catch ex As Exception

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ex.InnerException.InnerException.Message, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFailedEmail

        End Try

    End Sub

    ' MS200601 - Added to allow user to browse for an attachment

    Private Sub cmdBrowse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrowse.Click

        Try

            ' Open the dialog
            cdlBrowseOpen.ShowDialog()

            ' Populate attachment field with selected file
            txtFile.Text = cdlBrowseOpen.FileName

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process browse click", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdBrowse_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    ' {* USER DEFINED CODE (End) *}
    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click
        Me.Hide()
    End Sub

    Private Sub cmdSend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSend.Click
        SendMessage()
        Me.Hide()
    End Sub

    Private Sub Form_Initialize_Renamed()
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If


            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (Text1_Change) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Text1_Change()
    '
    'End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'developer guide no.293
        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
    End Sub
End Class
