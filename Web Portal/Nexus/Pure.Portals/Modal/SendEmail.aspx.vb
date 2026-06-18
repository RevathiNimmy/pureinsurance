Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Mail
Imports System.Xml.Linq
Imports CMS.Library
Imports Nexus
Imports Nexus.Constants
Imports Nexus.Library
Imports Nexus.Utils

Partial Class Modal_SendEmail
    Inherits System.Web.UI.Page

    Private _InsuranceFileKey As Integer
    Private _ClaimKey As Integer
    Private _PartyKey As Integer
    Private _ShowEmailTemplateList As Integer = 0
    Private Const ACEmailDocType As Integer = 8
    Private sFileReportName As String
    Private _DocumentRef As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim oParty As NexusProvider.BaseParty
        Dim oEmailDefaults As New EmailDefaults
        Dim oOptionSettings As NexusProvider.OptionTypeSetting

        _ShowEmailTemplateList = Request.QueryString("ShowEmailTemplateList")
        If _ShowEmailTemplateList = 1 Then
            ClientScript.RegisterStartupScript(Me.GetType(), "Javascript", "javascript:AddRichTextControl(); ", True)
        End If
        If Not IsPostBack Then
            ' Check for Remittance Advice email mode
            If Request.QueryString("EmailMode") = "RemittanceAdvice" Then
                InitialiseRemittanceAdviceMode()
                Return
            End If

            Dim idocID As Integer 'holds the document id
            Dim oFileTypes As Config.FileTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
            .Portals.Portal(Portal.GetPortalID()).FileTypes() 'so that we can fetch the file type config
            'we must always pass the location of the documents to be attached. If nothing is passed then we will end up with no attachements

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.EmailRegex)
            If (oOptionSettings.OptionValue <> "") Then
                regEmailTo.ValidationExpression = oOptionSettings.OptionValue
                regEmailCC.ValidationExpression = oOptionSettings.OptionValue
            Else
                regEmailTo.ValidationExpression = txtEmailRegex.Value
                regEmailCC.ValidationExpression = txtEmailRegex.Value
            End If

            'determine where the email request has come from and populate the attachements appropriately
            Select Case Request.QueryString("loc")
                Case "docm"
                    'files from document manager, we should look in Session(CNCurrentDocumentCollection) for the details
                    'Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Session(CNCurrentDocumentCollection), NexusProvider.DocumentDefaultsCollection)
                    Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Cache.Item(Request.QueryString("key")), NexusProvider.DocumentDefaultsCollection)
                    'we will have ids passed in query string in format docID1docID2 etc
                    'extract the ids by splitting on the string "docID"
                    If oDocumentCollection IsNot Nothing Then

                        For Each docID As String In Regex.Split(Request.QueryString("Docs"), "asp-check docID")

                            If Integer.TryParse(docID, idocID) Then
                                'we have an id passed so add an item to the checkboxlist by matching with the document collection in session
                                'set the attachement container to visisble
                                lblAttachments.Visible = True
                                With oDocumentCollection(Integer.Parse(docID))
                                    Dim iValue As Integer = .Key
                                    Dim sText As String = .DocumentName
                                    chklstAttachments.Items.Add(New ListItem With {.Value = docID, .Text = sText, .Selected = True})
                                    'Get the CSS class for this file type out of config

                                    If oFileTypes.FileType(oDocumentCollection(Integer.Parse(docID)).FileType) IsNot Nothing Then
                                        chklstAttachments.Items(chklstAttachments.Items.Count - 1).Attributes.Add("class", oFileTypes.FileType(oDocumentCollection(Integer.Parse(docID)).FileType).CssClass)
                                    End If

                                End With
                            End If
                        Next
                    End If


                Case ("sharep")
                    'files are located in sharepoint, we should find the file list in cache using the key from the query string
                    Dim oSPFileList As NexusProvider.SharepointFileList = CType(Cache.Item(Request.QueryString("key")), NexusProvider.SharepointFileList)
                    If oSPFileList IsNot Nothing Then
                        For Each spID As String In Regex.Split(Request.QueryString("Docs"), "spID")
                            If Integer.TryParse(spID, idocID) Then
                                'set the attachement container to visisble
                                lblAttachments.Visible = True
                                With oSPFileList.ItemList.Item(Integer.Parse(idocID))
                                    Dim sValue As String = idocID.ToString
                                    Dim sText As String = .Filename
                                    chklstAttachments.Items.Add(New ListItem With {.Value = sValue, .Text = sText, .Selected = True})
                                    'Get the CSS class for this file type out of config
                                    If oFileTypes.FileType(oSPFileList.ItemList.Item(Integer.Parse(idocID)).ItemType) IsNot Nothing Then
                                        chklstAttachments.Items(chklstAttachments.Items.Count - 1).Attributes.Add("class", oFileTypes.FileType(oSPFileList.ItemList.Item(Integer.Parse(idocID)).ItemType).CssClass)
                                    End If
                                End With
                            End If
                        Next
                    End If

                Case ("report")
                    _PartyKey = Request.QueryString("PartyKey")
                    oEmailDefaults = GetEmailDefaults("RemittanceReport")
                    sFileReportName = GenerateReport()
                    Session("Report") = sFileReportName
                    Dim oMsg As MailMessage = New MailMessage()
                    Dim oAttch As MailAttachment = New MailAttachment(sFileReportName)
                    oMsg.Attachments.Add(oAttch)

                    lblAttachments.Visible = True
                    chklstAttachments.Items.Add(New ListItem With {.Value = 1, .Text = "Remittance Advice Report", .Selected = True})
                    'Get the CSS class for this file type out of config

                    chklstAttachments.Items(chklstAttachments.Items.Count - 1).Attributes.Add("class", "upload-pdf")
            End Select

            'populate the message body with template according to where we are in the process
            'note - below logic will need to be redone once we need to handle claims and so on but should work to determine between quote and policy
            If Not Page.IsPostBack Then
                If _ShowEmailTemplateList = 1 Then
                    PopulateEmailTemplates()
                    'dvEmailTemplate.Visible = True
                    dvEmailTemplate.Style.Add("display", "block")
                End If
            End If
            If Request.QueryString("loc") <> "report" Then
                If ddlTemplate.Items.Count > 0 AndAlso ddlTemplate.SelectedValue <> "" Then
                    oEmailDefaults = GetEmailDefaults(ddlTemplate.SelectedItem.Text)
                Else
                    If Request.QueryString("CalledFrom") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.QueryString("CalledFrom")) AndAlso Request.QueryString("CalledFrom").ToUpper.Trim = "CASHLIST" Then
                        If Request.QueryString("ModeType") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.QueryString("CalledFrom")) AndAlso Request.QueryString("ModeType").ToUpper.Trim = "RECEIPT" Then
                            oEmailDefaults = GetEmailDefaults("CashListReceipt")
                        Else
                            oEmailDefaults = GetEmailDefaults("CashListPayment")
                        End If
                    ElseIf CType(Session(CNClaim), NexusProvider.ClaimOpen) IsNot Nothing Then
                        'bound policy so use the NewPolicy template
                        oEmailDefaults = GetEmailDefaults("NewClaim")

                    ElseIf Session(CNIsTransactionConfirmationVisited) = True Then
                        'bound policy so use the NewPolicy template
                        oEmailDefaults = GetEmailDefaults("NewPolicy")
                    Else
                        'this is a new quote, so use the NewQuote template
                        oEmailDefaults = GetEmailDefaults("NewQuote")
                    End If
                End If
            End If
            txtMessageBody.Text = oEmailDefaults.MessageBody
            txtEmailTo.Text = oEmailDefaults.EmailTo
            'For duplicate email id  
            Dim sEmailStr As String = txtEmailTo.Text
            Dim sSplitEmail As String() = sEmailStr.Split(","c)
            Dim list As List(Of String) = New List(Of String)()
            For Each SplitEmail As String In sSplitEmail
                If Not list.Contains(SplitEmail) Then
                    list.Add(SplitEmail)
                End If
            Next
            Dim sCombinedEmail As String = String.Join(",", list)
            ' end
            txtEmailTo.Text = sCombinedEmail
            txtEmailSubject.Text = oEmailDefaults.Subject

        End If


        If Request.QueryString("loc") = "docm" OrElse Request.QueryString("loc") = "manual" Then
            'Get the context (quote / claim / party) from query string values, regardless of whether it's a postback or not
            If Not String.IsNullOrEmpty(Request.QueryString("PartyKey")) Then
                Integer.TryParse(Request.QueryString("PartyKey"), _PartyKey)
            End If

            If Not String.IsNullOrEmpty(Request.QueryString("ClaimKey")) Then
                Integer.TryParse(Request.QueryString("ClaimKey"), _ClaimKey)
            End If

            If Not String.IsNullOrEmpty(Request.QueryString("InsuranceFileKey")) Then
                Integer.TryParse(Request.QueryString("InsuranceFileKey"), _InsuranceFileKey)
            End If

            If Not String.IsNullOrEmpty(Request.QueryString("Document_ref")) Then
                _DocumentRef = Request.QueryString("Document_ref")
            End If
        End If

    End Sub

    Private Sub PopulateEmailTemplates()
        Dim oQuote As NexusProvider.Quote = Session(Nexus.Constants.Session.CNQuote)
        Dim sProductCode As String = String.Empty
        Dim sInsuranceFileTypeCode As String = "POLICY"
        If Not oQuote Is Nothing Then
            sProductCode = oQuote.ProductCode
            sInsuranceFileTypeCode = oQuote.InsuranceFileTypeCode.Trim()
            If Session(CNIsTransactionConfirmationVisited) = True Then
                '<!--TransactionType="QUOTE,POLICY,RENEWAL,MTAQUOTE,MTA PERM,MTA TEMP,MTAQTETEMP,MTACAN,MTAREINS,MTAQREINS,WRITTEN,MTAQCAN,OPENCLAIM,EDITCLAIM,PAYCLAIM,VIEWCLAIM"-->
                If Session(CNMTAType) IsNot Nothing Then
                    If sInsuranceFileTypeCode = "MTAQUOTE" Or sInsuranceFileTypeCode = "MTA PERM" Then
                        sInsuranceFileTypeCode = "MTA PERM"
                    ElseIf sInsuranceFileTypeCode = "MTA TEMP" Or sInsuranceFileTypeCode = "MTAQTETEMP" Then
                        sInsuranceFileTypeCode = "MTA TEMP"
                    ElseIf sInsuranceFileTypeCode = "MTAQCAN" Or sInsuranceFileTypeCode = "MTACAN" Then
                        sInsuranceFileTypeCode = "MTACAN"
                    ElseIf sInsuranceFileTypeCode = "MTAQREINS" Or sInsuranceFileTypeCode = "MTAREINS" Then
                        sInsuranceFileTypeCode = "MTAREINS"
                    Else
                        sInsuranceFileTypeCode = "MTA PERM"
                    End If
                ElseIf Session(CNRenewal) IsNot Nothing Then
                    sInsuranceFileTypeCode = "RENEWAL"
                Else
                    sInsuranceFileTypeCode = "POLICY"
                End If
            End If
        Else
            oQuote = Session(Nexus.Constants.Session.CNClaimQuote)
            sProductCode = IIf(Session(Nexus.Constants.Session.CNProductCode) Is Nothing, "", Session(Nexus.Constants.Session.CNProductCode))
        End If
        Dim EmailTemplates As New Nexus.Library.Config.EmailTemplates
        EmailTemplates = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).EmailTemplates




        ddlTemplate.Items.Clear()
        Dim iTemplateCounter As Integer = 0
        For Each oEmailTemplate As Nexus.Library.Config.EmailTemplate In EmailTemplates
            If sProductCode <> "" Then
                Dim arrProductCodes() As String = IIf(Not IsNothing(oEmailTemplate.ProductCode), oEmailTemplate.ProductCode.ToString().ToUpper().Split(","), New String() {})

                For j As Integer = 0 To arrProductCodes.Length - 1
                    If arrProductCodes(j) = sProductCode.ToUpper Then
                        If Session(Nexus.Constants.Session.CNQuote) IsNot Nothing Then
                            If oEmailTemplate.TransactionType IsNot Nothing Then
                                If oEmailTemplate.TransactionType.Contains(sInsuranceFileTypeCode) Then
                                    ddlTemplate.Items.Insert(iTemplateCounter, oEmailTemplate.EmailTemplateCode)
                                    ddlTemplate.Items(iTemplateCounter).Text = oEmailTemplate.ID
                                    ddlTemplate.Items(iTemplateCounter).Value = oEmailTemplate.EmailTemplateCode
                                    iTemplateCounter += 1
                                End If
                                If oEmailTemplate.TransactionType = "*" Then
                                    ddlTemplate.Items.Insert(iTemplateCounter, oEmailTemplate.EmailTemplateCode)
                                    ddlTemplate.Items(iTemplateCounter).Text = oEmailTemplate.ID
                                    ddlTemplate.Items(iTemplateCounter).Value = oEmailTemplate.EmailTemplateCode
                                    iTemplateCounter += 1
                                End If
                            End If
                        ElseIf Session(Nexus.Constants.Session.CNClaimQuote) IsNot Nothing Then
                            If oEmailTemplate.TransactionType IsNot Nothing Then
                                If Session(CNMode) = Mode.NewClaim Then
                                    If oEmailTemplate.TransactionType.Contains("OPENCLAIM") Then
                                        ddlTemplate.Items.Insert(iTemplateCounter, oEmailTemplate.EmailTemplateCode)
                                        ddlTemplate.Items(iTemplateCounter).Text = oEmailTemplate.ID
                                        ddlTemplate.Items(iTemplateCounter).Value = oEmailTemplate.EmailTemplateCode
                                        iTemplateCounter += 1
                                    End If
                                End If
                                If Session(CNMode) = Mode.EditClaim Then
                                    If oEmailTemplate.TransactionType.Contains("EDITCLAIM") Then
                                        ddlTemplate.Items.Insert(iTemplateCounter, oEmailTemplate.EmailTemplateCode)
                                        ddlTemplate.Items(iTemplateCounter).Text = oEmailTemplate.ID
                                        ddlTemplate.Items(iTemplateCounter).Value = oEmailTemplate.EmailTemplateCode
                                        iTemplateCounter += 1
                                    End If
                                End If
                                If Session(CNMode) = Mode.PayClaim Then
                                    If oEmailTemplate.TransactionType.Contains("PAYCLAIM") Then
                                        ddlTemplate.Items.Insert(iTemplateCounter, oEmailTemplate.EmailTemplateCode)
                                        ddlTemplate.Items(iTemplateCounter).Text = oEmailTemplate.ID
                                        ddlTemplate.Items(iTemplateCounter).Value = oEmailTemplate.EmailTemplateCode
                                        iTemplateCounter += 1
                                    End If
                                End If
                                If Session(CNMode) = Mode.ViewClaim Then
                                    If oEmailTemplate.TransactionType.Contains("VIEWCLAIM") Then
                                        ddlTemplate.Items.Insert(iTemplateCounter, oEmailTemplate.EmailTemplateCode)
                                        ddlTemplate.Items(iTemplateCounter).Text = oEmailTemplate.ID
                                        ddlTemplate.Items(iTemplateCounter).Value = oEmailTemplate.EmailTemplateCode
                                        iTemplateCounter += 1
                                    End If
                                End If
                                If oEmailTemplate.TransactionType = "*" Then
                                    ddlTemplate.Items.Insert(iTemplateCounter, oEmailTemplate.EmailTemplateCode)
                                    ddlTemplate.Items(iTemplateCounter).Text = oEmailTemplate.ID
                                    ddlTemplate.Items(iTemplateCounter).Value = oEmailTemplate.EmailTemplateCode
                                    iTemplateCounter += 1
                                End If
                            End If
                        ElseIf Session(Nexus.Constants.Session.CNParty) IsNot Nothing Then
                            If oEmailTemplate.TransactionType = "*" Then
                                ddlTemplate.Items.Insert(iTemplateCounter, oEmailTemplate.EmailTemplateCode)
                                ddlTemplate.Items(iTemplateCounter).Text = oEmailTemplate.ID
                                ddlTemplate.Items(iTemplateCounter).Value = oEmailTemplate.EmailTemplateCode
                                iTemplateCounter += 1
                            End If
                        End If

                    End If

                Next
            Else
                If oEmailTemplate.TransactionType = "*" Then
                    ddlTemplate.Items.Insert(iTemplateCounter, oEmailTemplate.EmailTemplateCode)
                    ddlTemplate.Items(iTemplateCounter).Text = oEmailTemplate.ID
                    ddlTemplate.Items(iTemplateCounter).Value = oEmailTemplate.EmailTemplateCode
                    iTemplateCounter += 1
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Initialises the email form for Remittance Advice mode.
    ''' Resolves recipient and sender emails, populates subject, body, and attachment info.
    ''' </summary>
    Private Sub InitialiseRemittanceAdviceMode()
        Dim sAccountCode As String = Request.QueryString("AccountCode")
        Dim iPartyKey As Integer = 0
        Integer.TryParse(Request.QueryString("PartyKey"), iPartyKey)
        Dim sPaymentDate As String = Request.QueryString("PaymentDate")
        Dim sPaymentAmount As String = Request.QueryString("PaymentAmount")
        Dim iTransactionCount As Integer = 0
        Integer.TryParse(Request.QueryString("TransactionCount"), iTransactionCount)

        ' Recipient and sender email already validated in ReportViewer before opening this modal
        Dim oService As New RemittanceEmailService()
        Dim oRecipientResult As EmailResolutionResult = oService.ResolveRecipientEmail(iPartyKey)
        txtEmailTo.Text = If(oRecipientResult.Success, oRecipientResult.EmailAddress, "")

        Dim iInsurerPartyKey As Integer = 0
        If Session("InsurerPartyKey") IsNot Nothing Then
            Integer.TryParse(Session("InsurerPartyKey").ToString(), iInsurerPartyKey)
        End If
        Dim oSenderResult As EmailResolutionResult = oService.ResolveSenderEmail(iInsurerPartyKey)

        txtEmailCC.Text = ""
        sPaymentDate = sPaymentDate.Replace("/", "")
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oParty As NexusProvider.BaseParty = oWebService.GetParty(iPartyKey)
        If TypeOf oParty Is NexusProvider.PersonalParty Then
            sAccountCode = If(oParty.ResolvedName IsNot Nothing AndAlso oParty.ResolvedName.Trim() <> "", oParty.ResolvedName.Trim(), sAccountCode)
        ElseIf TypeOf oParty Is NexusProvider.CorporateParty Then
            sAccountCode = If(DirectCast(oParty, NexusProvider.CorporateParty).CompanyName IsNot Nothing AndAlso DirectCast(oParty, NexusProvider.CorporateParty).CompanyName.Trim() <> "", DirectCast(oParty, NexusProvider.CorporateParty).CompanyName.Trim(), sAccountCode)
        End If
        ' Store sender email and mode in session for use during send
        Session("RemittanceAdvice_SenderEmail") = oSenderResult.EmailAddress
        Session("RemittanceAdvice_Mode") = True
        Session("RemittanceAdvice_AccountCode") = sAccountCode
        Session("RemittanceAdvice_PartyKey") = iPartyKey
        Session("RemittanceAdvice_PaymentDate") = sPaymentDate
        Session("RemittanceAdvice_PaymentAmount") = sPaymentAmount
        Session("RemittanceAdvice_TransactionCount") = iTransactionCount

        ' Set subject
        txtEmailSubject.Text = String.Format("Remittance_Advice_{0}_{1}", sAccountCode, sPaymentDate)

        ' Set body from template or default
        txtMessageBody.Text = GetRemittanceEmailBody(sAccountCode, sPaymentDate, sPaymentAmount, iTransactionCount)

        ' Set attachment info — show in attachments list
        Dim sAttachmentName As String = String.Format("Remittance_Advice_{0}_{1}.pdf", sAccountCode, sPaymentDate)
        lblAttachments.Visible = True
        chklstAttachments.Items.Add(New ListItem With {.Value = "1", .Text = sAttachmentName, .Selected = True})
        chklstAttachments.Items(chklstAttachments.Items.Count - 1).Attributes.Add("class", "upload-pdf")

        ' Set up email validation using existing regex validator
        Dim oWebServiceVal As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oOptionSettings As NexusProvider.OptionTypeSetting = oWebServiceVal.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.EmailRegex)
        If oOptionSettings IsNot Nothing AndAlso oOptionSettings.OptionValue <> "" Then
            regEmailTo.ValidationExpression = oOptionSettings.OptionValue
        Else
            regEmailTo.ValidationExpression = txtEmailRegex.Value
        End If

        ' Show Cancel button only in Remittance Advice mode (opened from ReportViewer)
        btnCancelEmail.Visible = True
    End Sub

    ''' <summary>
    ''' Returns the default email body for Remittance Advice emails.
    ''' Attempts to read from RemittanceAdviceEmail.txt template, falls back to inline default.
    ''' </summary>
    Private Function GetRemittanceEmailBody(ByVal sAccountCode As String, ByVal sPaymentDate As String, ByVal sPaymentAmount As String, ByVal iTransactionCount As Integer) As String
        Dim sBody As String = ""
        Try
            Dim sTemplatePath As String = Server.MapPath("~/emailtemplates/Sample/RemittanceAdviceEmail.txt")
            If IO.File.Exists(sTemplatePath) Then
                Dim srTemplate As New IO.StreamReader(IO.File.OpenRead(sTemplatePath))
                sBody = srTemplate.ReadToEnd()
                srTemplate.Close()

                ' Replace placeholders
                sBody = sBody.Replace("[AccountName]", sAccountCode)
                sBody = sBody.Replace("[PaymentDate]", sPaymentDate)
                sBody = sBody.Replace("[PaymentAmount]", sPaymentAmount)
                sBody = sBody.Replace("[TransactionCount]", iTransactionCount.ToString())

                ' Replace logged-in user placeholder
                Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                If oUserDetails IsNot Nothing Then
                    sBody = sBody.Replace("[!LoggedInUserFullName!]", oUserDetails.ResolvedName)
                End If
            End If
        Catch ex As Exception
            ' Fall back to default body
        End Try

        ' Fallback if template not found or error
        If String.IsNullOrEmpty(sBody) Then
            sBody = String.Format(
                "Dear Sir/Madam," & vbCrLf & vbCrLf &
                "Please find attached the Remittance Advice for account {0}." & vbCrLf & vbCrLf &
                "Payment Date: {1}" & vbCrLf &
                "Total Payment Amount: {2}" & vbCrLf &
                "Number of Transactions Settled: {3}" & vbCrLf & vbCrLf &
                "Regards",
                sAccountCode, sPaymentDate, sPaymentAmount, iTransactionCount)
        End If

        Return sBody
    End Function

    Protected Sub DownloadAttachments(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Placeholder for attachment download functionality
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
    End Sub

    ''' <summary>
    ''' Handles click event to send an email. Creates the XML to pass to the CreateBackgroundJob method
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSendEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendEmail.Click
        If Page.IsValid Then
            ' Handle Remittance Advice email mode
            If Session("RemittanceAdvice_Mode") IsNot Nothing AndAlso CBool(Session("RemittanceAdvice_Mode")) = True Then
                HandleRemittanceAdviceSend()
                Return
            End If

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'set up the job to send emails to the requested addresses
            'create an html file on the disk in the temp docs directory, with contents taken from the body textbox
            Dim sFileName As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation & "\" & Guid.NewGuid.ToString & ".html"
            Dim emailBodyFile As New StreamWriter(sFileName)
            Dim sHtml As String
            'form html formatted string by replacing line breaks with "<br />" and adding basic html tags
            If Not txtMessageBody.Text.Contains("<html>") Then
                sHtml = "<html><body>" & Replace(txtMessageBody.Text.Trim, Chr(13) & Chr(10), "<br />") & "</body></html>"
            Else
                sHtml = txtMessageBody.Text.Trim
            End If
            emailBodyFile.Write(sHtml)
            emailBodyFile.Close()


            If Request.QueryString("loc") = "report" Then
                Dim EmailDetails As New Hashtable
                Dim oEmailTemplate As NexusProvider.EmailTemplateConfiguration = GetEmailTemplateDetails("InsurerPayment", "CCANNUAL")
                Dim sTemplatePath As String = oEmailTemplate.Path
                Dim sRecipient As String = txtEmailTo.Text
                'SEND EMAIL
                Dim dtEmailDetails As New DataTable
                Dim EmailTemplates As New Nexus.Library.Config.EmailTemplates
                EmailTemplates = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).EmailTemplates

                'Call SendEmail Method with parameter FileName
                SendEmail(sFileName)

            End If

            If Request.QueryString("loc") <> "report" Then
                Dim xlJob As XElement =
                   <BACKGROUND_JOB>
                       <JOB jobtype="DOCUPACK">
                           <PARAMETERS>
                               <PARAMETER name="emailTo" value=<%= txtEmailTo.Text %>/>
                               <PARAMETER name="emailCc" value=<%= txtEmailCC.Text %>/>
                               <PARAMETER name="emailSubject" value=<%= txtEmailSubject.Text %>/>
                               <PARAMETER name="Destination" value="email"/>
                               <PARAMETER name="path" value=<%= sFileName %>/>
                               <PARAMETER name="PartyCnt" value=<%= _PartyKey %>/>
                               <PARAMETER name="ClaimID" value=<%= _ClaimKey %>/>
                               <PARAMETER name="InsuranceFileCnt" value=<%= _InsuranceFileKey %>/>
                               <PARAMETER name="archive" value="true"/>
                               <PARAMETER name="DocumentRef" value=<%= _DocumentRef %>/>
                           </PARAMETERS>
                           <ITEMS>

                           </ITEMS>
                       </JOB>
                   </BACKGROUND_JOB>

                Select Case Request.QueryString("loc")
                    Case "docm"
                        'documents from the document manager control, so we need to look in session for the details
                        'Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Session(CNCurrentDocumentCollection), NexusProvider.DocumentDefaultsCollection)
                        Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Cache.Item(Request.QueryString("key")), NexusProvider.DocumentDefaultsCollection)

                        For Each chkAttachment As ListItem In chklstAttachments.Items
                            If chkAttachment.Selected Then
                                Dim xlItem As XElement
                                Dim iDocID As Integer
                                Integer.TryParse(chkAttachment.Value, iDocID)
                                If oDocumentCollection(iDocID).FileLocation IsNot Nothing Then
                                    'we've got an actual file so add the location as an item
                                    xlItem = <ITEM path=<%= CreateTempExternalFile(oDocumentCollection(iDocID).FileLocation) %>/>
                                Else
                                    'add the document code as an item, it will get generated
                                    xlItem = <ITEM code=<%= oDocumentCollection(iDocID).documentTemplateCode %>/>
                                End If
                                'if we are specifying a document to generate, or a document which has been generated and editted
                                'we then need to specify a format in which to archive it, either docx or pdf depending on the setting in web.cofig
                                If Not oDocumentCollection(iDocID).IsUpload Then
                                    Dim xlParameters As XElement = New XElement("PARAMETERS")
                                    'get the file type from config
                                    Dim sFileType As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()
                                    'we need to pass in the file name, which may change according to file type (e.g. quote.docx may archive as quote.pdf)
                                    Dim sOutputFileName As String = oDocumentCollection(iDocID).DocumentName
                                    sOutputFileName = Left(sOutputFileName, sOutputFileName.LastIndexOf(".")) & "." & sFileType.ToLower
                                    'add output format and file name params
                                    Dim xlDocumentFormat As XElement = <PARAMETER name="OutputFormat" value=<%= sFileType.ToUpper %>/>
                                    xlParameters.Add(xlDocumentFormat)
                                    Dim xlDestinationFilename As XElement = <PARAMETER name="DestinationFilename" value=<%= sOutputFileName %>/>
                                    xlParameters.Add(xlDestinationFilename)

                                    xlItem.Add(xlParameters)
                                End If

                                xlJob.Element("JOB").Element("ITEMS").Add(xlItem)
                            End If
                        Next
                    Case "sharep"
                        Dim oSPFileList As NexusProvider.SharepointFileList = CType(Cache.Item(Request.QueryString("key")), NexusProvider.SharepointFileList)

                        For Each chkAttachment As ListItem In chklstAttachments.Items
                            If chkAttachment.Selected Then
                                'get the document ID from the checkbox, and use this to get the path frmo the item list
                                Dim iDocID As Integer
                                Integer.TryParse(chkAttachment.Value, iDocID)
                                'set up the xml element to add to the job
                                Dim xlItem As XElement

                                xlItem = <ITEM path=<%= CreateTempExternalFile(oSPFileList.ItemList(iDocID).URL) %>/>
                                'if we are specifying a document to generate, or a document which has been generated and editted
                                'we then need to specify a format in which to archive it, either docx or pdf depending on the setting in web.cofig
                                Dim xlParameters As XElement = New XElement("PARAMETERS")
                                Dim xlDocumentFormat As XElement = <PARAMETER name="OutputFormat" value=<%= CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToUpper() %>/>
                                xlParameters.Add(xlDocumentFormat)

                                xlItem.Add(xlParameters)

                                xlJob.Element("JOB").Element("ITEMS").Add(xlItem)
                            End If
                        Next
                    Case "manual"

                        For Each chkAttachment As ListItem In chklstAttachments.Items
                            If chkAttachment.Selected Then
                                Dim xlItem As XElement
                                Dim iDocID As Integer
                                Integer.TryParse(chkAttachment.Value, iDocID)

                                Dim strFileName As String = Path.GetFileNameWithoutExtension(chkAttachment.Text)

                                'add the document code as an item, it will get generated
                                xlItem = <ITEM code=<%= strFileName %>/>

                                'if we are specifying a document to generate, or a document which has been generated and editted
                                'we then need to specify a format in which to archive it, either docx or pdf depending on the setting in web.cofig

                                Dim xlParameters As XElement = New XElement("PARAMETERS")
                                'get the file type from config
                                Dim sFileType As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()
                                'we need to pass in the file name, which may change according to file type (e.g. quote.docx may archive as quote.pdf)
                                'add output format and file name params
                                Dim xlDocumentFormat As XElement = <PARAMETER name="OutputFormat" value=<%= sFileType.ToUpper %>/>
                                xlParameters.Add(xlDocumentFormat)
                                Dim xlDestinationFilename As XElement = <PARAMETER name="DestinationFilename" value=<%= chkAttachment.Text %>/>
                                xlParameters.Add(xlDestinationFilename)
                                xlItem.Add(xlParameters)
                                xlJob.Element("JOB").Element("ITEMS").Add(xlItem)
                            End If
                        Next

                End Select

                Dim strJob As String = xlJob.ToString 'this will be used as input to the SAM call
                Dim sDescription As String = "Email documents"

                'call SAM to queue the docs for Archiving
                Dim iBackgroundJobID As Integer = oWebService.CreateBackgroundJob(sDescription, strJob, Now.Date)
                If Request.QueryString("PostBack") IsNot Nothing Then
                    If Request.QueryString("PostBack").ToUpper = "TRUE" Then
                        Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                        'refresh the parent page on postback with event argument RefreshGrid  
                        Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                    End If
                End If
                'close the modal page
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            End If
            ''Close the modal when the page reloads
            'ClientScript.RegisterStartupScript(GetType(String), "closeModal", "self.parent.hide_modal();", True)
        End If
    End Sub
    ''' <summary>
    ''' Handles the Send button click for Remittance Advice mode.
    ''' Uses the existing background job pattern (same as loc=report SendEmail) to send via SAM.
    ''' </summary>
    Private Sub HandleRemittanceAdviceSend()
        Dim oService As New RemittanceEmailService()
        Dim oRequest As New RemittanceEmailRequest()

        Try
            ' Build request from form fields and session
            oRequest.AccountCode = If(Session("RemittanceAdvice_AccountCode") IsNot Nothing, Session("RemittanceAdvice_AccountCode").ToString(), "")
            oRequest.RecipientEmail = txtEmailTo.Text.Trim()
            oRequest.SenderEmail = If(Session("RemittanceAdvice_SenderEmail") IsNot Nothing, Session("RemittanceAdvice_SenderEmail").ToString(), "")
            oRequest.Subject = txtEmailSubject.Text.Trim()
            oRequest.Body = txtMessageBody.Text.Trim()

            Dim iPartyKey As Integer = 0
            If Session("RemittanceAdvice_PartyKey") IsNot Nothing Then
                Integer.TryParse(Session("RemittanceAdvice_PartyKey").ToString(), iPartyKey)
            End If
            oRequest.PartyKey = iPartyKey

            Dim sPaymentDate As String = If(Session("RemittanceAdvice_PaymentDate") IsNot Nothing, Session("RemittanceAdvice_PaymentDate").ToString(), "")
            DateTime.TryParse(sPaymentDate, oRequest.PaymentDate)

            Dim sPaymentAmount As String = If(Session("RemittanceAdvice_PaymentAmount") IsNot Nothing, Session("RemittanceAdvice_PaymentAmount").ToString(), "")
            Decimal.TryParse(sPaymentAmount, oRequest.TotalPaymentAmount)

            If Session("RemittanceAdvice_TransactionCount") IsNot Nothing Then
                Integer.TryParse(Session("RemittanceAdvice_TransactionCount").ToString(), oRequest.TransactionCount)
            End If

            oRequest.AttachmentFileName = String.Format("Remittance_Advice_{0}_{1}.pdf", oRequest.AccountCode, If(sPaymentDate IsNot Nothing, sPaymentDate.Replace("/", ""), ""))
            oRequest.PaymentReference = oRequest.AccountCode & "-" & sPaymentDate

            ' Get PDF path from session (exported by ReportViewer)
            Dim sPdfPath As String = ""
            If Session("Report") IsNot Nothing Then
                sPdfPath = Session("Report").ToString()
            End If

            ' Create email body HTML file (same pattern as existing SendEmail)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sBodyFileName As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation & "\" & Guid.NewGuid.ToString & ".html"
            Dim emailBodyFile As New StreamWriter(sBodyFileName)
            Dim sHtml As String
            If Not txtMessageBody.Text.Contains("<html>") Then
                sHtml = "<html><body>" & Replace(txtMessageBody.Text.Trim, Chr(13) & Chr(10), "<br />") & "</body></html>"
            Else
                sHtml = txtMessageBody.Text.Trim
            End If
            emailBodyFile.Write(sHtml)
            emailBodyFile.Close()

            ' Build background job XML (same pattern as existing SendEmail/loc=report)
            Dim xlJob As XElement =
               <BACKGROUND_JOB>
                   <JOB jobtype="DOCUPACK">
                       <PARAMETERS>
                           <PARAMETER name="emailTo" value=<%= oRequest.RecipientEmail %>/>
                           <PARAMETER name="emailCc" value=""/>
                           <PARAMETER name="emailSubject" value=<%= oRequest.Subject %>/>
                           <PARAMETER name="Destination" value="email"/>
                           <PARAMETER name="path" value=<%= sBodyFileName %>/>
                           <PARAMETER name="PartyCnt" value=<%= iPartyKey %>/>
                           <PARAMETER name="archive" value="true"/>
                           <PARAMETER name="type" value="report"/>
                       </PARAMETERS>
                       <ITEMS>
                       </ITEMS>
                   </JOB>
               </BACKGROUND_JOB>
            ' Add PDF attachment only if user has selected the attachment checkbox
            Dim bAttachPdf As Boolean = (chklstAttachments.Items.Count > 0 AndAlso chklstAttachments.Items(0).Selected)
            If bAttachPdf Then
                If String.IsNullOrEmpty(sPdfPath) OrElse Not IO.File.Exists(sPdfPath) Then
                    Page.ClientScript.RegisterStartupScript(GetType(String), "RemittanceError",
                        "alert('The remittance advice PDF attachment could not be found. The email has not been sent.');", True)
                    Exit Sub
                End If

                Dim xlItem As XElement = <ITEM path=<%= sPdfPath %>/>
                Dim xlParameters As XElement = New XElement("PARAMETERS")
                Dim xlDocumentFormat As XElement = <PARAMETER name="OutputFormat" value="PDF"/>
                xlParameters.Add(xlDocumentFormat)
                Dim xlDestinationFilename As XElement = <PARAMETER name="DestinationFilename" value=<%= oRequest.AttachmentFileName %>/>
                xlParameters.Add(xlDestinationFilename)
                xlItem.Add(xlParameters)
                xlJob.Element("JOB").Element("ITEMS").Add(xlItem)
            End If

            ' Submit background job via SAM (handles email send, archival, and audit trail)
            Dim iBackgroundJobID As Integer = oWebService.CreateBackgroundJob("Email Remittance Advice for " & oRequest.AccountCode, xlJob.ToString(), Now.Date)

            ' Display success and close
            Dim sSuccessMsg As String = String.Format("Remittance Advice emailed to {0} for account {1}", oRequest.RecipientEmail, oRequest.AccountCode)
            Page.ClientScript.RegisterStartupScript(GetType(String), "RemittanceSuccess",
                String.Format("alert('{0}'); window.close();", sSuccessMsg.Replace("'", "\'")), True)

        Catch ex As Exception
            ' Create Work Manager task for failed send
            Try
                Dim oWebSvc As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oParty As NexusProvider.BaseParty = oWebSvc.GetParty(oRequest.PartyKey)
                Dim sPartyName As String = ""
                If oParty IsNot Nothing Then
                    If TypeOf oParty Is NexusProvider.PersonalParty Then
                        sPartyName = If(oParty.ResolvedName IsNot Nothing AndAlso oParty.ResolvedName.Trim() <> "", oParty.ResolvedName.Trim(), "")
                    ElseIf TypeOf oParty Is NexusProvider.CorporateParty Then
                        sPartyName = If(DirectCast(oParty, NexusProvider.CorporateParty).CompanyName IsNot Nothing AndAlso DirectCast(oParty, NexusProvider.CorporateParty).CompanyName.Trim() <> "", DirectCast(oParty, NexusProvider.CorporateParty).CompanyName.Trim(), "")
                    End If
                End If
                If String.IsNullOrEmpty(sPartyName) Then sPartyName = oRequest.AccountCode
                oService.CreateFailedEmailTask(oRequest.AccountCode, sPartyName, oRequest.PaymentDate, oRequest.TotalPaymentAmount, ex.Message)
            Catch
            End Try

            Page.ClientScript.RegisterStartupScript(GetType(String), "RemittanceError",
                String.Format("alert('Failed to send Remittance Advice email. Error: {0}');", ex.Message.Replace("'", "\'")), True)
        Finally
            Session.Remove("RemittanceAdvice_Mode")
            Session.Remove("RemittanceAdvice_SenderEmail")
            Session.Remove("RemittanceAdvice_AccountCode")
            Session.Remove("RemittanceAdvice_PartyKey")
            Session.Remove("RemittanceAdvice_PaymentDate")
            Session.Remove("RemittanceAdvice_PaymentAmount")
            Session.Remove("RemittanceAdvice_TransactionCount")
        End Try
    End Sub

    ''' <summary>
    ''' Email and archive document is both process where the same file is used for archiving the document so required to make two versions.
    ''' </summary>
    ''' <param name="sOriginalFilepath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateTempExternalFile(ByVal sOriginalFilepath As String) As String
        Dim sEmailTemplocation As String = String.Empty

        If sOriginalFilepath <> "" Then
            'set the location using an guid as the folder inside the temp file location from config
            sEmailTemplocation = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation & "\" & Guid.NewGuid.ToString
            If IO.File.Exists(sOriginalFilepath) Then
                System.IO.Directory.CreateDirectory(sEmailTemplocation)
                sEmailTemplocation = sEmailTemplocation & "\" & Path.GetFileName(sOriginalFilepath)
                IO.File.Copy(sOriginalFilepath, sEmailTemplocation, True)
            End If
        End If
        Return sEmailTemplocation
    End Function
    ''' <summary>
    ''' Gets the defaults for the email message and returns them as an email defaults object
    ''' </summary>
    ''' <param name="sTemplateCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetEmailDefaults(ByVal sTemplateCode As String) As EmailDefaults
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim sProductCode As String = String.Empty
        Dim oAsposeLicense As New Aspose.Words.License
        oAsposeLicense.SetLicense("Aspose.Totalfor.NET.lic")

        If Not oQuote Is Nothing Then
            sProductCode = oQuote.ProductCode
        Else
            sProductCode = IIf(Session(CNProductCode) Is Nothing, "", Session(CNProductCode))
        End If

        Dim oEmailDefaults As New EmailDefaults 'object to store properties which must be returned
        With oEmailDefaults
            Dim oEmailTemplate As NexusProvider.EmailTemplateConfiguration
            If Request.QueryString("loc") <> "report" Then
                oEmailTemplate = GetEmailTemplateDetails(sTemplateCode.ToUpper(), sProductCode.ToUpper())
            Else
                oEmailTemplate = GetEmailTemplateDetails(sTemplateCode.ToUpper(), sProductCode)
            End If
            Dim lstDocTemplate As New List(Of String)
            Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)

            Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sFileLocation As String = String.Empty
            Dim sDocumentTextContents As String = String.Empty
            Dim oDocument As NexusProvider.DocumentDefaults
            Dim sFile As String = ""
            Dim sbTemplate As New StringBuilder
            Dim sMailContact As String = String.Empty
            Dim sEmailAttachmentBODocument As String = String.Empty
            Dim sEmailSubjectBODocument As String = String.Empty

            If Request.QueryString("loc") = "report" Then
                oParty = oWebService.GetParty(_PartyKey)
            End If

            If Convert.ToInt64(Request.QueryString("PartyKey")) > 0 And oParty Is Nothing Then
                _PartyKey = Convert.ToInt64(Request.QueryString("PartyKey"))
                oParty = oWebService.GetParty(_PartyKey)
            End If

            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                oQuote = Session(CNClaimQuote)
                _ClaimKey = Session(CNClaimKey)
            Else
                oQuote = Session(CNQuote)
            End If

            If Request.QueryString("loc") <> "report" Then
                If Not oEmailTemplate.EmailTemplateCode Is Nothing AndAlso Not String.IsNullOrEmpty(oEmailTemplate.EmailTemplateCode) Then
                    lstDocTemplate.Add(oEmailTemplate.EmailTemplateCode.Trim)
                    oDocumentCollection = oWebService.GetDocumentDefaults(lstDocTemplate)
                    If oDocumentCollection IsNot Nothing And oDocumentCollection.Count > 0 Then
                        sEmailAttachmentBODocument = oDocumentCollection(0).EmailDocumentAttachmentCode
                        sEmailSubjectBODocument = oDocumentCollection(0).EmailDocumentSubjectCode
                        If sEmailAttachmentBODocument IsNot Nothing AndAlso Not String.IsNullOrEmpty(sEmailAttachmentBODocument) Then
                            AddBOEmailAttachments(sEmailAttachmentBODocument)
                        End If
                    End If
                End If

                If sEmailSubjectBODocument Is Nothing AndAlso String.IsNullOrEmpty(sEmailSubjectBODocument) Then
                    sEmailSubjectBODocument = oEmailTemplate.SubjectTemplateCode
                End If

                If Not IsNothing(oQuote) AndAlso Not IsNothing(oQuote.MailContacts) AndAlso oQuote.MailContacts <> String.Empty Then
                    sMailContact = oQuote.MailContacts
                End If
            End If

            'replace the merge values with real ones

            'do the straight forward replacements
            Dim sInsuredName As String = String.Empty
            Dim sPolicyHeader_CoverStartDate As String = String.Empty
            Dim sPolicyHeader_CoverEndDate As String = String.Empty

            If Session(CNQuote) IsNot Nothing Then
                'get the insured name from the quote object
                'sInsuredName = CType(Session(CNQuote), NexusProvider.Quote).InsuredName
                sPolicyHeader_CoverStartDate = CType(Session(CNQuote), NexusProvider.Quote).CoverStartDate
                sPolicyHeader_CoverEndDate = CType(Session(CNQuote), NexusProvider.Quote).CoverEndDate
            Else
                sInsuredName = Session(CNInsurer_Header)
            End If


            Select Case True
                Case TypeOf oParty Is NexusProvider.CorporateParty
                    With CType(oParty, NexusProvider.CorporateParty)
                        sInsuredName = .CompanyName 'to make the company name as hyper link

                    End With
                Case TypeOf oParty Is NexusProvider.PersonalParty
                    With CType(oParty, NexusProvider.PersonalParty)
                        sInsuredName = .Title & " " & .Forename & " " & .Lastname

                    End With
            End Select

            ' Get Email Subject
            If Not oEmailTemplate.Subject Is Nothing AndAlso Not String.IsNullOrEmpty(oEmailTemplate.Subject) Then
                .Subject = Replace(oEmailTemplate.Subject, "[!InsuredName!]", sInsuredName)
            Else
                If Not String.IsNullOrEmpty(sEmailSubjectBODocument) Then
                    oEmailTemplate.SubjectTemplateCode = sEmailSubjectBODocument
                Else
                    sEmailSubjectBODocument = oEmailTemplate.SubjectTemplateCode
                End If

                If Not oEmailTemplate.SubjectTemplateCode Is Nothing AndAlso Not String.IsNullOrEmpty(oEmailTemplate.SubjectTemplateCode) Then
                    sFile = ""

                    sFileLocation = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation _
                                & "\" & Guid.NewGuid.ToString & "\" & sEmailSubjectBODocument & ".TXT"

                    If Not oQuote Is Nothing Then
                        sFile = oWebService.GenerateDocument(v_iPartyKey:=oQuote.PartyKey, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
                            v_iInsuranceFolderKey:=oQuote.InsuranceFolderKey, v_sDocumentCode:=sEmailSubjectBODocument, v_oDocumentType:=NexusProvider.DocumentType.TXT,
                                                        v_sDocumentExtractionDirectory:=sFileLocation, v_iClaimKey:=_ClaimKey, v_bSkipArchiveonEdit:=True, v_iMode:=ACEmailDocType)
                    Else
                        sFile = oWebService.GenerateDocument(v_iPartyKey:=oParty.Key, v_iInsuranceFileKey:=0,
                            v_iInsuranceFolderKey:=0, v_sDocumentCode:=sEmailSubjectBODocument, v_oDocumentType:=NexusProvider.DocumentType.TXT,
                            v_sDocumentExtractionDirectory:=sFileLocation, v_iClaimKey:=_ClaimKey, v_bSkipArchiveonEdit:=True, v_iMode:=ACEmailDocType)
                    End If

                    If Not String.IsNullOrEmpty(sFile) Then
                        Dim bodyDoc As New Aspose.Words.Document(sFile)
                        Dim strSubject As String
                        strSubject = bodyDoc.GetText()
                        .Subject = strSubject
                    End If
                End If
            End If
            If Not String.IsNullOrEmpty(oEmailTemplate.ID) Then
                'create an array by splitting on comma
                Dim sTemp() As String = oEmailTemplate.Recipient.Split(",")

                'loop through the array, do the merge on each value to replace with the final value
                For iCounter As Integer = 0 To sTemp.Length - 1
                    If sTemp(iCounter).Contains("RiskData") Then
                        'we will have a path specified in the risk, so we need to extract the data from there

                        'strip out everything other than the path
                        sTemp(iCounter) = sTemp(iCounter).Replace("[", "")
                        sTemp(iCounter) = sTemp(iCounter).Replace("]", "")
                        sTemp(iCounter) = sTemp(iCounter).Replace("!", "")
                        sTemp(iCounter) = sTemp(iCounter).Replace("(", "")
                        sTemp(iCounter) = sTemp(iCounter).Replace(")", "")
                        sTemp(iCounter) = sTemp(iCounter).Replace("RiskData", "")
                        sTemp(iCounter) = sTemp(iCounter).Replace("'", "")

                        If sMailContact = String.Empty Then
                            sMailContact = FindInRiskData(sTemp(iCounter))
                        Else
                            sMailContact &= "," & FindInRiskData(sTemp(iCounter))
                        End If
                    Else
                        Select Case sTemp(iCounter)
                            Case "[!LoggedInUserEmail!]"
                                'we need to replace the value with the logged in users email address
                                If Not String.IsNullOrEmpty(CType(Session(CNAgentDetails), NexusProvider.UserDetails).EmailAddress) Then
                                    If sMailContact = String.Empty Then
                                        sMailContact = CType(Session(CNAgentDetails), NexusProvider.UserDetails).EmailAddress
                                    Else
                                        sMailContact &= "," & CType(Session(CNAgentDetails), NexusProvider.UserDetails).EmailAddress
                                    End If
                                End If
                        End Select
                    End If
                Next
                .EmailTo = sMailContact
                If Right(.EmailTo, 1) = "," Then
                    'remove the trailing comma
                    .EmailTo = Left(.EmailTo, Len(.EmailTo) - 1)
                End If


                If oParty IsNot Nothing Then
                    Dim strPartyMainEmail As String = ""
                    For Each oContact As NexusProvider.Contact In oParty.Contacts
                        If oContact.ContactType = NexusProvider.ContactType.MEMAIL Then
                            If Not String.IsNullOrEmpty(strPartyMainEmail) Then
                                strPartyMainEmail = strPartyMainEmail + "," + oContact.Number
                            Else
                                strPartyMainEmail = oContact.Number
                            End If
                            Exit For
                        ElseIf oContact.ContactType = NexusProvider.ContactType.Email AndAlso oParty.Contacts.FindItemByContactType(NexusProvider.ContactType.MEMAIL) Is Nothing Then
                            If Not String.IsNullOrEmpty(strPartyMainEmail) Then
                                strPartyMainEmail = strPartyMainEmail + "," + oContact.Number
                            Else
                                strPartyMainEmail = oContact.Number
                            End If
                        End If
                    Next
                    .EmailTo = IIf(.EmailTo.Trim.Length > 0, .EmailTo & "," & strPartyMainEmail, strPartyMainEmail)
                End If
                'Get the message body
                'Email Body 
                .MessageBody = String.Empty
                If Not oEmailTemplate.Path Is Nothing AndAlso Not String.IsNullOrEmpty(oEmailTemplate.Path) Then

                    'open the template from the given path
                    Dim srTmp As New StreamReader(File.OpenRead(Server.MapPath(oEmailTemplate.Path)))
                    sbTemplate = New StringBuilder(srTmp.ReadToEnd())
                    srTmp.Close()
                    If oEmailTemplate.Path.ToLower().EndsWith(".html") Then
                        .MessageBody = ConvertHTMLToText(sbTemplate.ToString)
                    Else
                        .MessageBody = sbTemplate.ToString
                    End If
                Else
                    If Not oEmailTemplate.EmailTemplateCode Is Nothing AndAlso Not String.IsNullOrEmpty(oEmailTemplate.EmailTemplateCode) Then
                        sFileLocation = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation _
                                & "\" & Guid.NewGuid.ToString & "\" & oEmailTemplate.EmailTemplateCode & ".TXT"

                        If Not oQuote Is Nothing Then
                            sFile = oWebService.GenerateDocument(v_iPartyKey:=oQuote.PartyKey, v_iInsuranceFileKey:=oQuote.InsuranceFileKey,
                            v_iInsuranceFolderKey:=oQuote.InsuranceFolderKey, v_sDocumentCode:=oEmailTemplate.EmailTemplateCode, v_oDocumentType:=NexusProvider.DocumentType.TXT,
                                                         v_sDocumentExtractionDirectory:=sFileLocation, v_iClaimKey:=_ClaimKey, v_bSkipArchiveonEdit:=True, v_iMode:=ACEmailDocType)
                        Else
                            sFile = oWebService.GenerateDocument(v_iPartyKey:=oParty.Key, v_iInsuranceFileKey:=0,
                                v_iInsuranceFolderKey:=0, v_sDocumentCode:=oEmailTemplate.EmailTemplateCode, v_oDocumentType:=NexusProvider.DocumentType.TXT,
                                v_sDocumentExtractionDirectory:=sFileLocation, v_iClaimKey:=_ClaimKey, v_bSkipArchiveonEdit:=True, v_iMode:=ACEmailDocType)
                        End If

                        'If Not String.IsNullOrEmpty(sFile) Then
                        '    Dim srBodyTmp As New StreamReader(File.OpenRead(sFile))
                        '    sbTemplate = New StringBuilder(srBodyTmp.ReadToEnd())
                        '    srBodyTmp.Close()                            '    
                        '    '.MessageBody = sbTemplate.ToString

                        '    Dim strDoc As String = sbTemplate.ToString
                        '    Dim iStartTitle, iEndTitle As Integer
                        '    'remove the title from document
                        '    iStartTitle = InStr(1, strDoc, "<o:DocumentProperties>")
                        '    iEndTitle = InStr(1, strDoc, "</o:DocumentProperties>")
                        '    If iStartTitle > 0 Then
                        '        Dim sSubstring As String = strDoc.Substring(iStartTitle - 1, (iEndTitle - iStartTitle) + 23)
                        '        strDoc = strDoc.Replace(sSubstring, "")
                        '    End If
                        '    .MessageBody = strDoc
                        'End If

                        'new code here
                        If Not String.IsNullOrEmpty(sFile) Then

                            Dim workingDirectory As String = Left(sFile, InStrRev(sFile, "\"))
                            Dim htmlsaveOption As Aspose.Words.Saving.HtmlSaveOptions = New Aspose.Words.Saving.HtmlSaveOptions()
                            Dim bodyDoc As New Aspose.Words.Document(sFile)
                            htmlsaveOption.ImagesFolder = workingDirectory
                            bodyDoc.Save(String.Concat(workingDirectory, "EmailBody.html"), htmlsaveOption)

                            Dim emailBody As String = File.ReadAllText(String.Concat(workingDirectory, "EmailBody.html"))

                            'Get a list of images saved from the document
                            Dim extractedImages As String() = Directory.GetFiles(workingDirectory, "*.jpeg")

                            'Convert saved images into emmbedded images
                            Dim sImagePath As String
                            For Each str As String In extractedImages
                                'Create the Base64 encoded image
                                sImagePath = str
                                Dim fileName As String = System.IO.Path.GetFileName(sImagePath)
                                Dim ms As New MemoryStream()
                                Dim imageFile As Bitmap = Bitmap.FromFile(sImagePath)
                                Dim image As Image = imageFile
                                image.Save(ms, Imaging.ImageFormat.Jpeg)
                                Dim imageBytes As Byte() = ms.ToArray() ' Convert byte[] to Base64 String
                                Dim base64String As String = Convert.ToBase64String(imageBytes)
                                Dim encodedImage As String = String.Concat("data:image/jpeg;base64,", base64String)

                                emailBody = emailBody.Replace(String.Concat(workingDirectory, fileName), encodedImage)
                            Next
                            .MessageBody = emailBody
                        End If

                        Return oEmailDefaults
                        Exit Function
                    End If
                End If


                'If Session(CNParty) IsNot Nothing Then
                .MessageBody = .MessageBody.Replace("[!InsuredName!]", sInsuredName)
                .MessageBody = .MessageBody.Replace("[!PolicyHeader_CoverStartDate!]", sPolicyHeader_CoverStartDate)
                .MessageBody = .MessageBody.Replace("[!PolicyHeader_CoverEndDate!]", sPolicyHeader_CoverEndDate)

                Dim sLoggedInUserFullName As String = String.Empty
                If Session(CNAgentDetails) IsNot Nothing Then
                    'get the logged in user name from session
                    sLoggedInUserFullName = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ResolvedName
                End If
                .MessageBody = .MessageBody.Replace("[!LoggedInUserFullName!]", sLoggedInUserFullName)

                'find instances of riskdata in the string builder and replace by searching the risk using specified xpath query
                'split the string to get each risk data part
                sTemp = .MessageBody.Split("[")
                'Clear the messagebody property then we rebuild it
                .MessageBody = String.Empty
                'loop through the array
                For Each sPart As String In sTemp
                    'take off the start of the merge string ("[!RiskData('")
                    'don't need this? sPart = Right(sPart, Len(sPart) - 12)
                    'extract the query. This will be the bit before the first " ')!]"
                    If Left(sPart, 11) = "!RiskData('" Then
                        'we need to do a query against the risk
                        Dim sQuery As String = Left(sPart, sPart.IndexOf("')!]"))
                        sQuery = Right(sQuery, Len(sQuery) - 11)
                        'put it all back together by adding the merged text, then the rest of the string after "')!]"
                        .MessageBody += FindInRiskData(sQuery) & Right(sPart, Len(sPart) - sPart.IndexOf("')!]") - 4)
                    Else
                        'just put the string back together, there shouldn't be any merge fields left in it
                        .MessageBody += sPart
                    End If
                Next



            End If

        End With

        Return oEmailDefaults
    End Function

    Private Function FindInRiskData(ByVal sQuery As String) As String
        Dim sReturn As String = String.Empty
        'loop through the risks and try to find a value for the broker email
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        If Session(CNMode) = Mode.NewClaim OrElse Session(CNMode) = Mode.EditClaim OrElse Session(CNMode) = Mode.PayClaim OrElse Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then
            oQuote = Session(CNClaimQuote)
            If Session(CNDataSet) IsNot Nothing Then
                Dim strDataset As New System.IO.StringReader(Session(CNDataSet))
                Dim sNavString As String = "DATA_SET/RISK_OBJECTS/" & Session(CNDataModelCode) & "_POLICY_BINDER/" & sQuery
                Dim Navigator As System.Xml.XPath.XPathNavigator
                Dim trDataset As New System.Xml.XmlTextReader(strDataset)
                Dim Doc As System.Xml.XPath.XPathDocument = New System.Xml.XPath.XPathDocument(trDataset)
                Navigator = Doc.CreateNavigator()
                Dim NodeI As System.Xml.XPath.XPathNodeIterator = Navigator.Select(sNavString)
                While NodeI.MoveNext
                    If Not String.IsNullOrEmpty(NodeI.Current.Value) Then
                        sReturn += NodeI.Current.Value
                    End If
                End While
            End If
        Else
            oQuote = Session(CNQuote)
            If oQuote IsNot Nothing Then
                If (oQuote.ContactUserKey = 0) Then
                    For iCount As Integer = 0 To oQuote.Risks.Count - 1
                        If oQuote.Risks(iCount).XMLDataset IsNot Nothing Then
                            Dim strDataset As New System.IO.StringReader(oQuote.Risks(iCount).XMLDataset)
                            Dim NavString As String = "DATA_SET/RISK_OBJECTS/" & Session(CNDataModelCode) & "_POLICY_BINDER/" & sQuery
                            Dim Navigator As System.Xml.XPath.XPathNavigator
                            Dim trDataset As New System.Xml.XmlTextReader(strDataset)
                            Dim Doc As System.Xml.XPath.XPathDocument = New System.Xml.XPath.XPathDocument(trDataset)
                            Navigator = Doc.CreateNavigator()
                            Dim NodeI As System.Xml.XPath.XPathNodeIterator = Navigator.Select(NavString)
                            While NodeI.MoveNext
                                'if we've got a value then add this to the email string and get out
                                If Not String.IsNullOrEmpty(NodeI.Current.Value) Then
                                    sReturn += NodeI.Current.Value
                                    Exit For
                                End If
                            End While
                        End If
                    Next
                Else
                    sReturn = oQuote.EmailAddress
                End If
            End If
        End If
        Return sReturn
    End Function


    ''' <summary>
    ''' This function converts HTML code to plain text
    ''' </summary>
    ''' <param name="sHTMLCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertHTMLToText(ByVal sHTMLCode As String) As String
        ' Remove new lines since they are not visible in HTML
        sHTMLCode = sHTMLCode.Replace("\n", " ")

        ' Remove tab spaces
        sHTMLCode = sHTMLCode.Replace("\t", " ")

        ' Remove multiple white spaces from HTML
        sHTMLCode = Regex.Replace(sHTMLCode, "\\s+", "  ")

        ' Remove HEAD tag
        sHTMLCode = Regex.Replace(sHTMLCode, "<head.*?</head>", "" _
          , RegexOptions.IgnoreCase Or RegexOptions.Singleline)

        ' Remove any JavaScript
        sHTMLCode = Regex.Replace(sHTMLCode, "<script.*?</script>", "" _
          , RegexOptions.IgnoreCase Or RegexOptions.Singleline)

        ' Replace special characters like &, <, >, " etc.
        Dim sbHTMLString As StringBuilder = New StringBuilder(sHTMLCode)
        ' Note: There are many more special characters, these are just
        ' most common. You can add new characters in this arrays if needed
        Dim OldWords() As String = {"&nbsp;", "&amp;", "&quot;", "&lt;", "&gt;", "&reg;", "&copy;", "&bull;", "&trade;"}
        Dim NewWords() As String = {" ", "&", """", "<", ">", "Â®", "Â©", "â€¢", "â„¢"}
        For i As Integer = 0 To i < OldWords.Length
            sbHTMLString.Replace(OldWords(i), NewWords(i))
        Next i

        ' Finally, remove all HTML tags and return plain text
        Return System.Text.RegularExpressions.Regex.Replace(sbHTMLString.ToString(), "<[^>]*>", "")
    End Function

    ''' <summary>
    ''' Holds defaults for the email to be sent
    ''' </summary>
    ''' <remarks></remarks>
    Private Class EmailDefaults
        Private _Subject As String
        Private _MessageBody As String
        Private _EmailTo As String

        Public Property Subject() As String
            Get
                Return _Subject
            End Get
            Set(ByVal value As String)
                _Subject = value
            End Set
        End Property

        Public Property MessageBody() As String
            Get
                Return _MessageBody
            End Get
            Set(ByVal value As String)
                _MessageBody = value
            End Set
        End Property

        Public Property EmailTo() As String
            Get
                Return _EmailTo
            End Get
            Set(ByVal value As String)
                _EmailTo = value
            End Set
        End Property
    End Class

    Public Function GenerateReport() As String
        Dim oParametersCollection As New NexusProvider.ParametersCollection
        Dim sPlaceHolderControlID As String = "plcReportForm"
        Dim sUrl As String = String.Empty
        Dim sReportsTypeControlID As String = Nothing
        Dim sSelectedReportsType As String = Nothing
        Dim sCustomValidator As String = "cusReportForm"
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim sFileName As String

        sSelectedReportsType = GetLocalResourceObject("lblReport")

        'Executed Function from Dataset function
        Try
            Dim oParameters As NexusProvider.Parameters
            oParameters = New NexusProvider.Parameters
            oParameters.ParamNameField = "user_id"
            oParameters.ParamValueField = Nothing

            'add the param into the collection
            oParametersCollection.Add(oParameters)
            sFileName = GetReportUrl(sSelectedReportsType, oParametersCollection)

        Catch ex As NexusProvider.NexusException
            'Checking  (bSIRReportPrint.Business.SendToPrint Failed : Failed : Return Value = PMNotFound) Error code , then display a message saying no record found 
            If ex.Errors(0).Code = "1000019" Then
                ex.Errors(0).Code = "88"
            End If
            Throw
        End Try
        Return sFileName
    End Function

    ''' <summary>
    ''' This method retreive the report and returns the Url to open the report
    ''' </summary>
    ''' <param name="sReportName"></param>
    ''' <param name="oParametersCollection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReportUrl(ByVal sReportName As String, ByVal oParametersCollection As NexusProvider.ParametersCollection) As String
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim sDocumentExtractionDirectory As String = Nothing
        Dim sUniqueDirectory As String = Guid.NewGuid.ToString
        Dim url As String = String.Empty
        Dim sFileName As String = String.Empty
        Dim sLocation As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation

        Dim sReportDirName As String = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork) _
              .Portals.Portal(CMS.Library.Portal.GetPortalID()).Reports.Location

        'set the extraction directory using a guid to ensure it is unique
        sDocumentExtractionDirectory = sLocation & "/" & sUniqueDirectory

        'make SAM call with request parameters, sFileName will contain the name of the file we need to display
        sFileName = oWebService.GetReport(sReportName, NexusProvider.DocumentFormatType.PDF,
            oParametersCollection, sDocumentExtractionDirectory)

        Return sFileName
    End Function

    Public Sub SendEmail(Optional ByVal sFileName As String = Nothing)

        Dim sPartyKey = Request.QueryString("PartyKey")

        Dim xlJob As XElement =
           <BACKGROUND_JOB>
               <JOB jobtype="DOCUPACK">
                   <PARAMETERS>
                       <PARAMETER name="emailTo" value=<%= txtEmailTo.Text %>/>
                       <PARAMETER name="emailCc" value=<%= txtEmailCC.Text %>/>
                       <PARAMETER name="emailSubject" value=<%= txtEmailSubject.Text %>/>
                       <PARAMETER name="Destination" value="email"/>
                       <PARAMETER name="path" value=<%= sFileName %>/>
                       <PARAMETER name="PartyCnt" value=<%= sPartyKey %>/>
                       <PARAMETER name="archive" value="true"/>
                       <PARAMETER name="type" value="report"/>
                   </PARAMETERS>
                   <ITEMS>
                   </ITEMS>
               </JOB>
           </BACKGROUND_JOB>

        For Each chkAttachment As ListItem In chklstAttachments.Items
            If chkAttachment.Selected Then
                Dim xlItem As XElement
                Dim iDocID As Integer
                Integer.TryParse(chkAttachment.Value, iDocID)
                If sFileName IsNot Nothing Then
                    'we've got an actual file so add the location as an item
                    xlItem = <ITEM path=<%= Session("Report") %>/>
                End If
                'if we are specifying a document to generate, or a document which has been generated and editted
                'we then need to specify a format in which to archive it, either docx or pdf depending on the setting in web.cofig
                Dim xlParameters As XElement = New XElement("PARAMETERS")
                'get the file type from config
                Dim sFileType As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()
                'we need to pass in the file name, which may change according to file type (e.g. quote.docx may archive as quote.pdf)
                Dim sOutputFileName As String = Right(Session("Report").ToString(), Len(Session("Report").ToString()) - InStrRev(Session("Report").ToString(), "\"))
                sOutputFileName = Left(sOutputFileName, sOutputFileName.LastIndexOf(".")) & "." & sFileType.ToLower
                'add output format and file name params
                Dim xlDocumentFormat As XElement = <PARAMETER name="OutputFormat" value=<%= sFileType.ToUpper %>/>
                xlParameters.Add(xlDocumentFormat)
                Dim xlDestinationFilename As XElement = <PARAMETER name="DestinationFilename" value=<%= sOutputFileName %>/>
                xlParameters.Add(xlDestinationFilename)

                xlItem.Add(xlParameters)

                xlJob.Element("JOB").Element("ITEMS").Add(xlItem)
            End If
        Next

        Dim strJob As String = xlJob.ToString 'this will be used as input to the SAM call
        Dim sDescription As String = "Email report"
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        'call SAM to queue the docs for Archiving
        Dim iBackgroundJobID As Integer = oWebService.CreateBackgroundJob(sDescription, strJob, Now.Date)
        If Request.QueryString("PostBack") IsNot Nothing Then
            If Request.QueryString("PostBack").ToUpper = "TRUE" Then
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                'refresh the parent page on postback with event argument RefreshGrid  
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            End If
        End If
        'close the modal page
        Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
    End Sub

    Protected Shadows Sub ddlTemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTemplate.SelectedIndexChanged
        Dim oEmailDefault As EmailDefaults
        oEmailDefault = GetEmailDefaults(ddlTemplate.SelectedItem.Text)
        If oEmailDefault IsNot Nothing Then
            txtMessageBody.Text = oEmailDefault.MessageBody
            txtEmailTo.Text = oEmailDefault.EmailTo
            txtEmailSubject.Text = oEmailDefault.Subject
        End If

    End Sub

    Private Sub AddBOEmailAttachments(ByVal sAttachmentDocumentCodes As String)
        Dim optionType As New NexusProvider.OptionTypeSetting
        Dim oWebService As NexusProvider.ProviderBase
        Dim DocId As Integer
        Dim sText, documentType As String
        Dim oFileTypes As Config.FileTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
            .Portals.Portal(Portal.GetPortalID()).FileTypes()

        oWebService = New NexusProvider.ProviderManager().Provider

        optionType = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5009)
        documentType = "PDF"
        'If optionType.OptionValue = "1" Then
        '    documentType = "PDF"
        '    'Else
        '    '    documentType = "docx"
        'End If

        Dim sAttachmentSTR As String() = sAttachmentDocumentCodes.Split(",")
        chklstAttachments.Items.Clear()

        If sAttachmentSTR IsNot Nothing AndAlso sAttachmentSTR.Length > 0 Then
            lblAttachments.Visible = True
            For DocId = 0 To sAttachmentSTR.Length - 1
                sText = sAttachmentSTR(DocId) + "." + documentType
                chklstAttachments.Items.Add(New ListItem With {.Value = chklstAttachments.Items.Count, .Text = sText, .Selected = True})
                If oFileTypes.FileType(documentType) IsNot Nothing Then
                    chklstAttachments.Items(chklstAttachments.Items.Count - 1).Attributes.Add("class", oFileTypes.FileType(documentType).CssClass)
                End If
            Next
        End If
    End Sub
End Class




