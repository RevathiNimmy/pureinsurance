Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports System.Web
Imports System.Text
Imports System.Configuration
Imports System.Web.HttpContext
Imports System.IO

Public Module funcEmail

    Function SendEmail(ByVal pSender As String, _
                              ByVal pRecipient As String, _
                              ByVal pSubject As String, _
                              ByVal pMessage As String, _
                              Optional ByVal pEmailDetails As System.Collections.Hashtable = Nothing, _
                              Optional ByVal pTemplateFile As String = Nothing, _
                              Optional ByVal pWebRoot As String = Nothing, _
                              Optional ByVal pAttachment As String = Nothing, _
                              Optional ByVal pCC As String = Nothing, _
                              Optional ByVal pBCC As String = Nothing, _
                              Optional ByVal pEmailServer As String = Nothing) As Boolean

        'DH - 01/09/05
        'Send an email using the passed parameters,
        'TemplateFile the path of a file to be used a template, should contain keywords to be replaced with text

        Dim SMTPObj As SmtpClient

        If pEmailServer Is Nothing Then
            SMTPObj = New SmtpClient("localhost", 25)
        Else
            SMTPObj = New SmtpClient(pEmailServer, 25)
        End If

        Dim MailObj As New MailMessage(pSender, pRecipient, pSubject, "")

        If pCC IsNot Nothing Then
            MailObj.CC.Add(pCC)
        End If
        If pBCC IsNot Nothing Then
            MailObj.Bcc.Add(pBCC)
        End If

        If pTemplateFile Is Nothing Then
            MailObj.IsBodyHtml = False
            MailObj.Body = pMessage
        Else
            MailObj.IsBodyHtml = True
            MailObj.Body = GenericEmailFormat(pTemplateFile, pEmailDetails, pWebRoot)

            If MailObj.Body.Length <= 0 Then
                Return True 'Failed
            End If

        End If

        If pAttachment <> Nothing Then
            Dim atcItem As New Attachment(HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath) + pAttachment)
            MailObj.Attachments.Add(atcItem)
        End If
        Try
            SMTPObj.Send(MailObj)
            Return False 'Success
        Catch ex As Exception
            Return True 'Failed
        End Try

    End Function

    Private Function GenericEmailFormat(ByVal pTemplateFile As String, _
                                        ByVal pEmailDetails As System.Collections.Hashtable, ByVal pWebRoot As String) As String

        'DH - 01/09/05
        'Create a string for the email content using the passed
        'template to have the keywords replace with passed values

        'Open Template
        Dim srTmp As New StreamReader(File.OpenRead(Current.Server.MapPath(pTemplateFile)))

        Dim sbTemplate As New StringBuilder(srTmp.ReadToEnd())
        srTmp.Close()

        sbTemplate.Replace("[!WEBROOT!]", pWebRoot)

        Dim Email As New StringBuilder

        With Email

            For Each sKey As String In pEmailDetails.Keys
                sbTemplate.Replace(sKey, pEmailDetails(sKey))
            Next

        End With

        Return sbTemplate.ToString

    End Function

End Module

