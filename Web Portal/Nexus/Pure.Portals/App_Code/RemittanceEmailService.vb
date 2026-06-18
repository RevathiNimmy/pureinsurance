Imports System.IO
Imports System.Net.Mail
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml.Linq
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

#Region "Data Models"

''' <summary>
''' Encapsulates all data required to compose and send a Remittance Advice email.
''' </summary>
Public Class RemittanceEmailRequest
    Public Property AccountCode As String
    Public Property RecipientEmail As String
    Public Property SenderEmail As String
    Public Property Subject As String
    Public Property Body As String
    Public Property PdfFilePath As String
    Public Property AttachmentFileName As String
    Public Property PaymentDate As DateTime
    Public Property TotalPaymentAmount As Decimal
    Public Property TransactionCount As Integer
    Public Property PartyKey As Integer
    Public Property PaymentReference As String
End Class

''' <summary>
''' Result of resolving a recipient or sender email address from Party contacts or System Options.
''' </summary>
Public Class EmailResolutionResult
    Public Property Success As Boolean
    Public Property EmailAddress As String
    Public Property ErrorMessage As String
End Class

''' <summary>
''' Result of sending a Remittance Advice email via SMTP.
''' </summary>
Public Class SendEmailResult
    Public Property Success As Boolean
    Public Property Message As String
    Public Property ErrorDetails As String
End Class

''' <summary>
''' Result of archiving a sent email to SharePoint.
''' </summary>
Public Class ArchiveResult
    Public Property Success As Boolean
    Public Property ErrorMessage As String
End Class

''' <summary>
''' Audit actions for Remittance Advice email operations.
''' Note: Audit trail is handled automatically by the SAM background job (DOCUPACK) process.
''' This enum is retained only for reference.
''' </summary>
Public Enum AuditAction
    RemittanceAdviceEmailSent
    RemittanceAdviceEmailFailed
End Enum

#End Region

''' <summary>
''' Service class encapsulating Remittance Advice email business logic including
''' email resolution, SMTP sending, SharePoint archival, Work Manager task creation,
''' and audit trail recording.
''' </summary>
Public Class RemittanceEmailService

    ''' <summary>
    ''' Resolves recipient email from Party contacts.
    ''' Priority: Main_Email_Contact (MEMAIL) > Internet_Electronic_Mail_Address (Email) > error.
    ''' </summary>
    ''' <param name="iPartyKey">The Party key to resolve the email for.</param>
    ''' <returns>EmailResolutionResult with the resolved email or an error message.</returns>
    Public Function ResolveRecipientEmail(ByVal iPartyKey As Integer) As EmailResolutionResult
        Try
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oParty As NexusProvider.BaseParty = oWebService.GetParty(iPartyKey)

            ' Priority: MEMAIL first, then Email fallback
            Dim strPartyMainEmail As String = ""
            Dim oMemail As NexusProvider.Contact = oParty.Contacts.FindItemByContactType(NexusProvider.ContactType.MEMAIL)
            If oMemail IsNot Nothing AndAlso Not String.IsNullOrEmpty(oMemail.Number) Then
                strPartyMainEmail = oMemail.Number
            Else
                For Each oContact As NexusProvider.Contact In oParty.Contacts
                    If oContact.ContactType = NexusProvider.ContactType.Email Then
                        strPartyMainEmail = oContact.Number
                        Exit For
                    End If
                Next
            End If

            If Not String.IsNullOrEmpty(strPartyMainEmail) Then
                Dim oResult As New EmailResolutionResult()
                oResult.Success = True
                oResult.EmailAddress = strPartyMainEmail
                Return oResult
            Else
                Dim sPartyName As String = ""
                If TypeOf oParty Is NexusProvider.PersonalParty Then
                    sPartyName = If(oParty.ResolvedName IsNot Nothing AndAlso oParty.ResolvedName.Trim() <> "", oParty.ResolvedName.Trim(), "")
                ElseIf TypeOf oParty Is NexusProvider.CorporateParty Then
                    sPartyName = If(DirectCast(oParty, NexusProvider.CorporateParty).CompanyName IsNot Nothing AndAlso DirectCast(oParty, NexusProvider.CorporateParty).CompanyName.Trim() <> "", DirectCast(oParty, NexusProvider.CorporateParty).CompanyName.Trim(), "")
                End If
                If String.IsNullOrEmpty(sPartyName) Then
                    sPartyName = If(oParty.ShortName IsNot Nothing AndAlso oParty.ShortName.Trim() <> "", oParty.ShortName.Trim(), If(HttpContext.Current.Session("AccountCode") IsNot Nothing, HttpContext.Current.Session("AccountCode").ToString(), iPartyKey.ToString()))
                End If
                Dim oResult As New EmailResolutionResult()
                oResult.Success = False
                oResult.ErrorMessage = String.Format("No email address found for account {0}. The remittance advice cannot be emailed.", sPartyName)
                Return oResult
            End If
        Catch ex As Exception
            Dim oResult As New EmailResolutionResult()
            oResult.Success = False
            oResult.ErrorMessage = ex.Message
            Return oResult
        End Try
    End Function

    ''' <summary>
    ''' Resolves sender email address.
    ''' Priority: Insurer_Email > SMTP_Email_Return_Address (System Option 5047) > error.
    ''' </summary>
    ''' <param name="iInsurerPartyKey">The insurer Party key to resolve the sender email for.</param>
    ''' <returns>EmailResolutionResult with the resolved sender email or an error message.</returns>
    Public Function ResolveSenderEmail(ByVal iInsurerPartyKey As Integer) As EmailResolutionResult
        Try
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If (iInsurerPartyKey > 0) Then

                Dim oParty As NexusProvider.BaseParty = oWebService.GetParty(iInsurerPartyKey)

                ' Check for insurer email from party contacts (MEMAIL then Email)
                Dim strInsurerEmail As String = ""
                For Each oContact As NexusProvider.Contact In oParty.Contacts
                    If oContact.ContactType = NexusProvider.ContactType.MEMAIL Then
                        strInsurerEmail = oContact.Number
                        Exit For
                    ElseIf oContact.ContactType = NexusProvider.ContactType.Email AndAlso oParty.Contacts.FindItemByContactType(NexusProvider.ContactType.MEMAIL) Is Nothing Then
                        strInsurerEmail = oContact.Number
                    End If
                Next

                If Not String.IsNullOrEmpty(strInsurerEmail) Then
                    Dim oResult As New EmailResolutionResult()
                    oResult.Success = True
                    oResult.EmailAddress = strInsurerEmail
                    Return oResult
                End If
            End If

            ' Fallback to System Option 5047 (SMTP Email Return Address)
            Dim oOptionSettings As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5047)
            If oOptionSettings IsNot Nothing AndAlso Not String.IsNullOrEmpty(oOptionSettings.OptionValue) Then
                Dim oResult As New EmailResolutionResult()
                oResult.Success = True
                oResult.EmailAddress = oOptionSettings.OptionValue
                Return oResult
            End If
            ' Neither configured
            Dim oErrorResult As New EmailResolutionResult()
            oErrorResult.Success = False
            oErrorResult.ErrorMessage = "Email sender address is not configured. Please contact your System Administrator."
            Return oErrorResult
        Catch ex As Exception
            Dim oResult As New EmailResolutionResult()
            oResult.Success = False
            oResult.ErrorMessage = ex.Message
            Return oResult
        End Try
    End Function

    ''' <summary>
    ''' Composes and sends the Remittance Advice email via SMTP with PDF attachment.
    ''' Uses System Options 5045 (server), 5046 (port), 5094 (BCC), 5184 (SSL), 5183 (password).
    ''' </summary>
    ''' <param name="oRequest">The email request containing all composition and delivery details.</param>
    ''' <returns>SendEmailResult indicating success or failure with details.</returns>
    Public Function SendRemittanceAdvice(ByVal oRequest As RemittanceEmailRequest) As SendEmailResult
        Dim oResult As New SendEmailResult()

        Try
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            ' Read System Option 5045 (SMTP Email Server)
            Dim oSmtpServerOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5045)
            If oSmtpServerOption Is Nothing OrElse String.IsNullOrEmpty(oSmtpServerOption.OptionValue) Then
                oResult.Success = False
                oResult.ErrorDetails = "SMTP email server is not configured. Please contact your System Administrator."
                Return oResult
            End If
            Dim sSMTPServer As String = oSmtpServerOption.OptionValue

            ' Read System Option 5046 (SMTP Email Server Port)
            Dim oSmtpPortOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5046)
            Dim iSmtpPort As Integer = 25
            If oSmtpPortOption IsNot Nothing AndAlso Not String.IsNullOrEmpty(oSmtpPortOption.OptionValue) Then
                Integer.TryParse(oSmtpPortOption.OptionValue, iSmtpPort)
            End If

            ' Read System Option 5094 (SMTP Email BCC Address)
            Dim oSmtpBccOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5094)
            Dim sSMTPBcc As String = ""
            If oSmtpBccOption IsNot Nothing AndAlso Not String.IsNullOrEmpty(oSmtpBccOption.OptionValue) Then
                sSMTPBcc = oSmtpBccOption.OptionValue
            End If

            ' Read System Option 5184 (SMTP Enable SSL)
            Dim oSmtpSslOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5184)
            Dim bEnableSsl As Boolean = False
            If oSmtpSslOption IsNot Nothing AndAlso Not String.IsNullOrEmpty(oSmtpSslOption.OptionValue) Then
                bEnableSsl = (oSmtpSslOption.OptionValue = "True" OrElse oSmtpSslOption.OptionValue = "1")
            End If

            ' Read System Option 5183 (SMTP User Password)
            Dim oSmtpPwdOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5183)
            Dim sSMTPPassword As String = ""
            If oSmtpPwdOption IsNot Nothing AndAlso Not String.IsNullOrEmpty(oSmtpPwdOption.OptionValue) Then
                sSMTPPassword = oSmtpPwdOption.OptionValue
            End If

            ' Compose and send using Using blocks for proper resource disposal
            Using oMailMessage As New MailMessage()
                oMailMessage.From = New MailAddress(oRequest.SenderEmail)
                oMailMessage.To.Add(New MailAddress(oRequest.RecipientEmail))
                oMailMessage.Subject = oRequest.Subject
                oMailMessage.Body = oRequest.Body

                ' Add BCC if configured
                If Not String.IsNullOrEmpty(sSMTPBcc) Then
                    oMailMessage.Bcc.Add(New MailAddress(sSMTPBcc))
                End If

                ' Attach PDF file - fail if attachment is missing (Bug #38681 Issue 1)
                If String.IsNullOrEmpty(oRequest.PdfFilePath) OrElse Not File.Exists(oRequest.PdfFilePath) Then
                    oResult.Success = False
                    oResult.ErrorDetails = "The remittance advice PDF attachment could not be found. The email has not been sent."
                    Return oResult
                End If

                Dim oAttachment As New Attachment(oRequest.PdfFilePath)
                If Not String.IsNullOrEmpty(oRequest.AttachmentFileName) Then
                    oAttachment.Name = oRequest.AttachmentFileName
                End If
                oMailMessage.Attachments.Add(oAttachment)

                Using oSmtpClient As New SmtpClient(sSMTPServer, iSmtpPort)
                    oSmtpClient.EnableSsl = bEnableSsl

                    If Not String.IsNullOrEmpty(sSMTPPassword) Then
                        oSmtpClient.Credentials = New System.Net.NetworkCredential(oRequest.SenderEmail, sSMTPPassword)
                    End If

                    oSmtpClient.Send(oMailMessage)
                End Using
            End Using

            oResult.Success = True
            oResult.Message = String.Format("Remittance Advice emailed to {0} for account {1}", oRequest.RecipientEmail, oRequest.AccountCode)
            Return oResult

        Catch ex As Exception
            oResult.Success = False
            oResult.ErrorDetails = String.Format("Failed to send Remittance Advice email. Error: {0}. Please try again or contact your System Administrator.", ex.Message)
            Return oResult

        Finally
            ' Note: Do NOT delete the temp PDF file here.
            ' The file may still be needed for SharePoint archival which runs asynchronously.
            ' Temp files will be cleaned up by the OS or a scheduled cleanup process.
        End Try
    End Function


    ''' <summary>
    ''' Creates a Work Manager task for failed email delivery, allocated to the
    ''' Failed_Email_Workmanager_Task_Group configured in System Option 5068.
    ''' Mirrors the logic in DocManagerWrapper.AddFailedEmailWorkManagerTask().
    ''' </summary>
    Public Sub CreateFailedEmailTask(ByVal sAccountCode As String, ByVal sPartyName As String, ByVal dPaymentDate As DateTime, ByVal dPaymentAmount As Decimal, ByVal sFailureReason As String)
        Try
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            ' Read Failed_Email_Workmanager_Task_Group from System Option 5068
            Dim oFailedEmailTaskGroupOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5068)

            If oFailedEmailTaskGroupOption Is Nothing OrElse String.IsNullOrEmpty(oFailedEmailTaskGroupOption.OptionValue) Then
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(
                    "Failed Email Work Manager Task Group not configured (System Option 5068). Cannot create failed email task.", "Error")
                Return
            End If

            Dim iTaskGroupId As Integer = Convert.ToInt32(oFailedEmailTaskGroupOption.OptionValue)

            ' Fetch task group code from task group key
            Dim sTaskGroupCode As String = ""
            Dim oTaskGroups As NexusProvider.TaskGroupCollection = oWebService.GetTaskGroups()
            If oTaskGroups IsNot Nothing Then
                For Each oTaskGroup As NexusProvider.TaskGroup In oTaskGroups
                    If oTaskGroup.TaskGroupKey = iTaskGroupId Then
                        sTaskGroupCode = oTaskGroup.Code
                        Exit For
                    End If
                Next
            End If

            If String.IsNullOrEmpty(sTaskGroupCode) Then
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(
                    String.Format("Failed Email Work Manager Task Group ID {0} not found in task groups.", iTaskGroupId), "Error")
                Return
            End If

            ' Build task description with all required details (Req 8 AC3)
            Dim sTaskDescription As String = String.Format(
                "Remittance Advice Email Failed" & vbCrLf &
                "Account: {0}" & vbCrLf &
                "Party Name: {1}" & vbCrLf &
                "Payment Date: {2}" & vbCrLf &
                "Total Payment Amount: {3:C}" & vbCrLf &
                "Failure Reason: {4}",
                sAccountCode, sPartyName, dPaymentDate.ToString("dd/MM/yyyy"), dPaymentAmount, sFailureReason)

            ' Create Work Manager task using System Option 5068 task group (same as DocManagerWrapper)
            Dim oWorkManager As New NexusProvider.WorkManager
            oWorkManager.Client = If(Not String.IsNullOrEmpty(sAccountCode), "Code " & sAccountCode & If(Not String.IsNullOrEmpty(sPartyName), " - " & sPartyName, ""), "Email Document Failed")
            oWorkManager.DueDate = DateTime.Now()
            oWorkManager.DateCreated = DateTime.Now()
            oWorkManager.Description = sTaskDescription
            oWorkManager.Task = "MEMO"
            oWorkManager.TaskGroup = sTaskGroupCode
            oWorkManager.AllocationUserGroup = "UA"
            oWorkManager.IsUrgent = True
            oWorkManager.IsUrgentForUpdate = 1
            oWorkManager.IsComplete = False
            oWorkManager.IsTaskReview = True

            ' Add AccountCode as key data
            Dim oKeyData As New NexusProvider.KeyData
            oKeyData.KeyName = "AccountCode"
            oKeyData.KeyValue = sAccountCode
            oWorkManager.KeyData.Add(oKeyData)

            oWorkManager.LockName = NexusProvider.SAMForInsurance.PureService.TaskLockName.InvalidValue
            oWebService.CreateWmTask(oWorkManager)

        Catch ex As Exception
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(
                "Error creating Work Manager task for failed email: " & ex.Message, "Error")
        End Try
    End Sub



End Class
