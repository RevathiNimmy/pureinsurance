Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Xml
Imports System.Xml.XPath
Imports System.Globalization.CultureInfo
Imports System.Data
Imports SiriusFS.SAM.Client
Imports System.Globalization


Partial Class Products_TestMOTOR_TestMOTOR_SummaryOfCover
    Inherits System.Web.UI.UserControl
    Public Const CNStandardWording As String = "StandardWording"
    Public Const CNPremiumTransactions As String = "PremiumTransactions"
    Public olbWritten As Button
    Dim sWrittenStatus As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim olblbuynow As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnBuy"), LinkButton)
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        ''PM020609 - jai Prakash 08/06/2012
        ''olblbuynow.OnClientClick = "this.disabled=true; "
        ''olblbuynow.UseSubmitBehavior = False

	    If oQuote IsNot Nothing AndAlso oQuote.BusinessTypeCode IsNot Nothing AndAlso
            oQuote.BusinessTypeCode.Trim().ToUpper() = "DIRECT" Then
            lbl_errmessage.Visible = False
        End If
        'CommonProperties(oQuote, Session(CNCurrentRiskKey))
        'lkList_UNDERWRITER.Value = PCATLIN__UNDERWRITER.Text
        'txtUnderwriter.Text = lkList_UNDERWRITER.Text
        ''added by sukh sagar to get frequeny in hidden field
        'Frequency.Value = Session("Frequency")

        'If GetDatafromXML("_POLICY_BINDER/VEHDET", "LASTMODIFYDATE") IsNot Nothing Then
        '    PCATLIN__LASTMODIFYDATE.Text = GetDatafromXML("_POLICY_BINDER/VEHDET", "LASTMODIFYDATE")
        'End If

        'If GetDatafromXML("_POLICY_BINDER/PCATLIN", "MIDCLIENTMANAGED") IsNot Nothing Then
        '    If GetDatafromXML("_POLICY_BINDER/PCATLIN", "MIDCLIENTMANAGED") = "0" Then
        '        MIDCLIENTMANAGED.Checked = False
        '    End If
        '    If GetDatafromXML("_POLICY_BINDER/PCATLIN", "MIDCLIENTMANAGED") = "1" Then
        '        MIDCLIENTMANAGED.Checked = True
        '    End If
        'End If

        'If GetDatafromXML("_POLICY_BINDER/PCATLIN", "PREMIUMCREDIT") IsNot Nothing Then
        '    If GetDatafromXML("_POLICY_BINDER/PCATLIN", "PREMIUMCREDIT") = "0" Then
        '        PREMIUMCREDIT.Checked = False
        '    End If
        '    If GetDatafromXML("_POLICY_BINDER/PCATLIN", "PREMIUMCREDIT") = "1" Then
        '        PREMIUMCREDIT.Checked = True
        '    End If
        'End If

        'If oQuote.Risks.Count > 0 Then
        '    UpdateXML("POLICYSTATUS", oQuote.InsuranceFileTypeCode.ToString())
        'End If
        ''code commented for motor product
        ''Page.ClientScript.RegisterStartupScript(GetType(String), "StartupScripts", "VehicleBreakdownGrid();", True)

        ''If GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR1PREMIUM") IsNot Nothing Then
        ''    txtQtr1.Text = GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR1PREMIUM")
        ''    txtHalf1.Text = GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR1PREMIUM")
        ''    txtAnnual.Text = GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR1PREMIUM")
        ''End If
        ''If GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR2PREMIUM") IsNot Nothing Then
        ''    txtQtr2.Text = GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR2PREMIUM")
        ''    txtHalf2.Text = GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR2PREMIUM")
        ''End If
        ''If GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR3PREMIUM") IsNot Nothing Then
        ''    txtQtr3.Text = GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR3PREMIUM")
        ''End If

        ''If GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR4PREMIUM") IsNot Nothing Then
        ''    txtQtr4.Text = GetDatafromXML("_POLICY_BINDER/PCATLIN", "QTR4PREMIUM")
        ''End If


        'If Not IsPostBack Then

        '    Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
        '    Dim sPolicyRef As String = CType(Session.Item(CNQuote), NexusProvider.Quote).InsuranceFileRef

        '    Dim sShortName As String = String.Empty
        '    Dim PreviousYear_Status As Boolean = False
        '    Dim Filetypecode = oQuote.InsuranceFileKey
        '    Dim InsuranceFileType = oQuote.InsuranceFileTypeCode
        '    Dim oTempQuote As New NexusProvider.Quote

        '    'Finding Client ShortName
        '    If Session(CNParty) IsNot Nothing Then
        '        Select Case True
        '            Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
        '                With CType(Session(CNParty), NexusProvider.CorporateParty)
        '                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
        '                        sShortName = .ClientSharedData.ShortName.Trim()
        '                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
        '                        sShortName = .UserName
        '                    End If
        '                End With
        '            Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
        '                With CType(Session(CNParty), NexusProvider.PersonalParty)
        '                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
        '                        sShortName = .ClientSharedData.ShortName.Trim()
        '                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
        '                        sShortName = .UserName
        '                    End If

        '                End With
        '        End Select
        '    End If

        '    'On Page load PreviousYear Button Visible Check
        '    '      If (sPolicyRef <> String.Empty) Then
        '    '          'If Current Version is  QUOTE or POLICY  then we are not allow to Search any Previous Detail
        '    '          If InsuranceFileType <> "QUOTE" Or InsuranceFileType <> "POLICY" Or InsuranceFileType <> "" Then
        '    '              'Finding All Policy version Details
        '    '              oInsuranceFileDetailsCollection = oWebService.FindPolicy(UCase(Trim(sPolicyRef)), "", sShortName, NexusProvider.InsuranceFileTypes.ALL, False, Nothing)
        '    '              If oInsuranceFileDetailsCollection IsNot Nothing Then
        '    '                  For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
        '    ''Response.Write("Filetypecode:" & Filetypecode)					
        '    ''Response.Write("InsuranceFileKey:" & oInsuranceFileDetails.InsuranceFileKey)					
        '    '                      If Filetypecode > oInsuranceFileDetails.InsuranceFileKey Then
        '    '                          oTempQuote = oWebService.GetHeaderAndSummariesByKey(oInsuranceFileDetails.InsuranceFileKey)
        '    '		'Response.Write("yes")		
        '    '                          If (DateDiff(DateInterval.Day, oTempQuote.CoverStartDate, oQuote.CoverStartDate) > 364 And oTempQuote.InsuranceFileTypeCode = oQuote.InsuranceFileTypeCode) Then
        '    '		'Response.Write("yes1")
        '    '                              If oTempQuote.InsuranceFileTypeCode = oQuote.InsuranceFileTypeCode Then
        '    '                                  PreviousYear_Status = True
        '    '                                  Btn_PreviousYear.Visible = True
        '    '                                  Exit For
        '    '                              End If
        '    '                          End If
        '    '                      End If
        '    '                  Next
        '    '              End If
        '    '          End If
        '    '      End If
        '    '      If PreviousYear_Status = False Then
        '    '          Btn_PreviousYear.Visible = False
        '    '      End If





        '    'On Page load PreviousYear Button Visible Check
        '    If (sPolicyRef <> String.Empty) Then
        '        'If Current Version is  QUOTE or POLICY  then we are not allow to Search any Previous Detail
        '        If InsuranceFileType <> "QUOTE" Or InsuranceFileType <> "POLICY" Or InsuranceFileType <> "" Then
        '            'Finding All Policy version Details
        '            oInsuranceFileDetailsCollection = oWebService.FindPolicy(UCase(Trim(sPolicyRef)), "", sShortName, NexusProvider.InsuranceFileTypes.ALL, False, Nothing)
        '            If (oInsuranceFileDetailsCollection IsNot Nothing) Then
        '                For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
        '                    If Filetypecode > oInsuranceFileDetails.InsuranceFileKey Then
        '                        oTempQuote = oWebService.GetHeaderAndSummariesByKey(oInsuranceFileDetails.InsuranceFileKey)

        '                        If (oTempQuote.InsuranceFileTypeCode = oQuote.InsuranceFileTypeCode) Then
        '                            If oQuote.Regarding = "Renewals" Then
        '                                If Not IsPostBack Then
        '                                    If (Session("CHECKRENEWAL") Is Nothing) Then
        '                                        PreviousYear_Status = True
        '                                        Btn_PreviousYear.Visible = True
        '                                        Btn_CurrentYear.Visible = False
        '                                        Session("CHECKRENEWAL") = True
        '                                    End If
        '                                End If

        '                            End If
        '                        End If
        '                        Exit For
        '                    End If
        '                Next
        '            End If
        '        End If
        '    End If



        '    Dim oPolicy As NexusProvider.PolicyCollection
        '    oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)
        '    Dim count As Integer
        '    Dim PolicyVersionCount As Integer = 0
        '    Dim countPolVersion As Integer = oPolicy.Count
        '    For countPolver As Integer = countPolVersion To 1 Step -1
        '        If Trim(oPolicy(countPolver - 1).InsuranceFileTypeCode) = "POLICY" Then
        '            PolicyVersionCount = PolicyVersionCount + 1
        '            count = countPolver - 1
        '        End If

        '    Next



        '    If PreviousYear_Status = False And (PolicyVersionCount > 1 And Session(CNMode) = Mode.View) And oQuote.InsuranceFileKey <> oPolicy(count).InsuranceFileKey Then
        '        Btn_PreviousYear.Visible = True
        '        Session("CHECKRENEWAL") = True
        '    End If

        '    'Call the RunDefaultRuleEdit after addition/updation of the record
        '    Dim v_sScreenCode As String
        '    'v_sScreenCode = "fleet2"
        '    v_sScreenCode = "MOTOR"

        '    If oQuote.Risks.Count > 0 Then
        '        If Session(CNMTAType) IsNot Nothing And Session(CNRenewal) Is Nothing Then
        '            If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
        '                oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(v_sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "MTA")
        '            ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
        '                oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(v_sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "MTC")
        '            ElseIf (Session(CNMTAType) = MTAType.REINSTATEMENT) Then
        '                oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(v_sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "MTR")
        '            End If
        '        ElseIf Session(CNMTAType) Is Nothing And Session(CNRenewal) IsNot Nothing Then
        '            oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(v_sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "REN")
        '        Else
        '            oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(v_sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
        '        End If
        '    End If
        '    'Code for  WRITTEN


        '    If oQuote IsNot Nothing Then
        '        If oQuote.InsuranceFileTypeCode IsNot Nothing AndAlso oQuote.InsuranceFileTypeCode = "WRITTEN" Then
        '            Dim oCtrMultiRisk As Control = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("MultiRisk1"), Control)
        '            If (Not oCtrMultiRisk Is Nothing) Then
        '                oCtrMultiRisk.Visible = False
        '            End If
        '            Dim returndate As Date
        '            If PCATLIN__LASTMODIFYDATE.Text <> "" Then
        '                ConverDateFormat(PCATLIN__LASTMODIFYDATE.Text, returndate)
        '            End If
        '            If IsDate(returndate) Then
        '                ' Dim returndate As Date
        '                'code added for error message not displayed while Written policy status of same day.
        '                ' ConverDateFormat(PCATLIN__LASTMODIFYDATE.Text, returndate)
        '                If Date.Compare(returndate.Date, Date.Now.Date) = 0 Then
        '                    Dim obtnRequote As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnRequote"), Button)
        '                    Dim obtnSaveQuote As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnSaveQuote"), Button)
        '                    lbl_Writtenerrmessage.Visible = True
        '                    oCtrMultiRisk.Visible = False
        '                    If (Not obtnRequote Is Nothing) Then
        '                        obtnRequote.Visible = False
        '                    End If

        '                    If (Not obtnSaveQuote Is Nothing) Then
        '                        obtnSaveQuote.Visible = False
        '                    End If
        '                Else
        '                    oCtrMultiRisk.Visible = True
        '                End If

        '                If Date.Compare(returndate, FormatDateTime(Now, DateFormat.ShortDate)) Then
        '                    Dim obtnRequote As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnRequote"), Button)
        '                    If (Not obtnRequote Is Nothing) Then
        '                        'obtnRequote.Visible = False
        '                        'lbl_Writtenerrmessage.Visible = True
        '                    End If
        '                    ' START - DONE AGAINST PN 38532
        '                    Dim oSAMClient As New SiriusFS.SAM.Client.DataSetControl.Application
        '                    oSAMClient.LoadFromXML(GetDataSetDefinition(Session.Item(CNDataModelCode)), oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)

        '                    If oSAMClient.Risk.Item(Session.Item(CNDataModelCode) & "_POLICY_BINDER").Item(Session.Item(CNDataModelCode) _
        '                        & "_OUTPUT").Item(Session.Item(CNDataModelCode) & "_OUTPUT_ID").Value IsNot Nothing Then

        '                        oSAMClient.Risk.Item(Session.Item(CNDataModelCode) & "_POLICY_BINDER").Item(Session.Item(CNDataModelCode) _
        '                            & "_OUTPUT").DeleteObject()

        '                    End If

        '                    oSAMClient.ReturnAsXML(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
        '                    oSAMClient.Terminate()
        '                    ' End - DONE AGAINST PN 38532

        '                    oQuote.Risks(0).ScreenCode = "MOTOR"
        '                    If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
        '                        'oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTA")
        '                        'Session(CNQuote) = oQuote
        '                    ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
        '                        'oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTC")
        '                        'Session(CNQuote) = oQuote
        '                    ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
        '                        'oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTR")
        '                        'Session(CNQuote) = oQuote
        '                    ElseIf Session(CNRenewal) Then
        '                        oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, Nothing, "REN")
        '                        Session(CNQuote) = oQuote
        '                    Else
        '                        oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
        '                        Session(CNQuote) = oQuote
        '                    End If

        '                End If
        '            End If
        '        End If
        '    End If
        '    Dim olblRenewalInvite As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnPrint"), Button)
        '    sWrittenStatus = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.AllowWrittenStatus, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
        '    If (oQuote.InsuranceFileTypeCode = "QUOTE" Or oQuote.InsuranceFileTypeCode = "" Or oQuote.InsuranceFileTypeCode Is Nothing) AndAlso oQuote.InsuranceFileTypeCode <> "WRITTEN" AndAlso sWrittenStatus = "0" Then
        '        ' Dim olblbuynow As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnBuy"), Button)
        '        If (Not olblbuynow Is Nothing) Then
        '            olblbuynow.Visible = True
        '        End If
        '    ElseIf oQuote.InsuranceFileTypeCode = "WRITTEN" AndAlso sWrittenStatus = "1" Then
        '        If (Not olbWritten Is Nothing) Then
        '            olblbuynow.Visible = False
        '        End If
        '    ElseIf oQuote.InsuranceFileTypeCode = "QUOTE" AndAlso sWrittenStatus = "1" Then
        '        olblbuynow.Visible = False
        '    Else
        '        If (Not olblbuynow Is Nothing) Then
        '            olblbuynow.Visible = True
        '        End If
        '    End If
        '    'End of WRITTEN 

        '    'Code for Cancellation Date
        '    If oQuote IsNot Nothing Then
        '        If oQuote.InsuranceFileTypeCode = "MTAREINS" Or oQuote.InsuranceFileTypeCode = "MTAQREINS" Then
        '            Dim Current_PolicyStartDate, Current_PolicyExpiryDate, MTACAN_PolicyStartDate, MTACAN_PolicyExpiryDate As Date
        '            'Dim oPolicy As NexusProvider.PolicyCollection
        '            oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)
        '            Current_PolicyStartDate = oQuote.CoverStartDate 'oPolicy.Item(0).CoverStartDate ' 'need in validating custvldMTAEffectiveDate


        '            Dim TempVar As Integer
        '            For TempVar = 0 To oPolicy.Count - 1
        '                If oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTACAN" Then
        '                    MTACAN_PolicyStartDate = oPolicy.Item(TempVar).CoverStartDate
        '                    MTACAN_PolicyExpiryDate = oPolicy.Item(TempVar).ExpiryDate
        '                End If
        '            Next
        '            If Not (Current_PolicyStartDate <= MTACAN_PolicyExpiryDate And Current_PolicyStartDate >= MTACAN_PolicyStartDate) Then
        '                'Dim olblbuynow As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnBuy"), Button)
        '                If (Not olblbuynow Is Nothing) Then
        '                    olblbuynow.Visible = False
        '                    lbl_mtareinserrmessage.Visible = True
        '                End If
        '            End If
        '        End If
        '    End If
        '    'End of Cancellation Date 
        'End If

        'olbWritten = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnWrite"), Button)
        'Dim olhdnfRenewalStatus As HiddenField = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("hdnfRenewalStatus"), HiddenField)
        'Dim strRenewalStatus As String = String.Empty
        'If (Not olhdnfRenewalStatus Is Nothing) Then
        '    strRenewalStatus = olhdnfRenewalStatus.Value
        'End If

        'Dim olblbuynow1 As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnBuy"), Button)
        'If Session(CNRenewal) IsNot Nothing Then
        '    Dim olblRenewalInvite As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnPrint"), Button)

        '    olblbuynow1 = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnBuy"), Button)

        '    If oQuote.InsuranceFileTypeCode IsNot Nothing AndAlso oQuote.InsuranceFileTypeCode <> "WRITTEN" AndAlso sWrittenStatus = "0" AndAlso (olblRenewalInvite Is Nothing Or (strRenewalStatus = "Awaiting_Update")) Then
        '        If (Not olblbuynow1 Is Nothing) Then
        '            olblbuynow1.Visible = True
        '        End If
        '    ElseIf oQuote.InsuranceFileTypeCode = "WRITTEN" AndAlso sWrittenStatus = "1" AndAlso (Not olblRenewalInvite Is Nothing) Then
        '        If (Not olbWritten Is Nothing) Then
        '            olblbuynow.Visible = False
        '        End If
        '    ElseIf (olblRenewalInvite.Visible = False AndAlso oQuote.InsuranceFileTypeCode = "RENEWAL" AndAlso sWrittenStatus Is Nothing) Then
        '        olblbuynow.Visible = True
        '    Else
        '        If (Not olblbuynow1 Is Nothing) Then
        '            olblbuynow1.Visible = False
        '        End If
        '    End If
        '    CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("tblDocs").Visible = False
        'End If
        'If GetDatafromXML("_POLICY_BINDER/PCATLIN", "POLICYSTATUS") IsNot Nothing Then
        '    If GetDatafromXML("_POLICY_BINDER/PCATLIN", "POLICYSTATUS") = "6" Or GetDatafromXML("_POLICY_BINDER/PCATLIN", "POLICYSTATUS") = "7" Then

        '        Dim olbl_buynow As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnBuy"), Button)
        '        Dim olbl_Write As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnWrite"), Button)
        '        If (Not olbl_buynow Is Nothing) Then
        '            olbl_buynow.Enabled = False
        '        End If
        '        If (Not olbl_Write Is Nothing) Then
        '            olbl_Write.Enabled = False
        '        End If
        '    End If
        'End If

        'Find Branch of selected quote/policy       
        Dim sBranchName As String = ""
        If Session(CNAgentDetails) IsNot Nothing Then
            Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
            If oBranchs IsNot Nothing Then
                For Each oBranch As NexusProvider.Branch In oBranchs
                    If oBranch.Code = Session(CNBranchCode) Then
                        sBranchName = oBranch.Description
                        Exit For
                    End If
                Next
            End If
        End If
        'Find client code of selected quote/policy         
        Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)

        Dim sClientCode As String
        Select Case True
            Case TypeOf oParty Is NexusProvider.CorporateParty
                With CType(oParty, NexusProvider.CorporateParty)
                    If Session(CNLoginType) = LoginType.Customer Then
                    Else
                        If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                            sClientCode = .ClientSharedData.ShortName.Trim()
                        ElseIf String.IsNullOrEmpty(.UserName) = False Then
                            sClientCode = .UserName.Trim()
                        End If
                    End If
                End With
            Case TypeOf oParty Is NexusProvider.PersonalParty
                With CType(oParty, NexusProvider.PersonalParty)
                    If Session(CNLoginType) = LoginType.Customer Then
                        If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                            sClientCode = .ClientSharedData.ShortName.Trim()
                        ElseIf String.IsNullOrEmpty(.UserName) = False Then
                            sClientCode = .UserName.Trim()
                        End If
                    Else
                        If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                            sClientCode = .ClientSharedData.ShortName.Trim()
                        ElseIf String.IsNullOrEmpty(.UserName) = False Then
                            sClientCode = .UserName.Trim()
                        End If
                    End If
                End With
        End Select
        If Session(CNMode) = Mode.View Then
            'DocLstCtrl.Visible = True
        Else
            ' DocLstCtrl.Visible = False
        End If
        'Set hyperlink with  Branch/client code/ Quote number     
        cnt_FileView.CurrentFolder = Trim(sBranchName) & "|" & Trim(sClientCode) & "|" & Trim(oQuote.InsuranceFileRef)
        HyperLink1.NavigateUrl = "~/secure/DocumentManager.aspx?path=" & Trim(sBranchName) & "|" & Trim(sClientCode) & "|" & Trim(oQuote.InsuranceFileRef)

        Dim oOptionType As New NexusProvider.OptionTypeSetting
        oOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 10)
        Dim myUC As UserControl = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("docMgr"), UserControl) 'LoadControl("ucHeading.ascx")
        If myUC IsNot Nothing Then
            If String.IsNullOrEmpty(oOptionType.OptionValue) = False AndAlso oOptionType.OptionValue = "1" Then
                myUC.Visible = False
                myUC.Attributes.Add("style", "display: none")
                divDME.Attributes.Add("style", "display:block;")
            ElseIf String.IsNullOrEmpty(oOptionType.OptionValue) = False AndAlso oOptionType.OptionValue = "2" Then
                divDME.Attributes.Add("style", "display:none;")
                myUC.Visible = True
                myUC.Attributes.Add("style", "display:block;")
            End If
        End If

        If Request("__EVENTARGUMENT") = "RefreshSOC" Then
            Response.Redirect(Request.RawUrl)
        End If
    End Sub

    Public Function GetDataSetDefinition(Optional ByVal v_sDataModelCode As String = Nothing) As String

        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim sDataSetDefinition As String = String.Empty
        Dim sBranchCode As String = oQuote.BranchCode
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Try
            sDataSetDefinition = oWebService.GetDatasetDefinition(v_sDataModelCode, sBranchCode)
        Finally
            oWebService = Nothing
        End Try

        Return sDataSetDefinition

    End Function

    Sub CommonProperties(ByVal Quote, ByVal CNCurrentRiskKey)

        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim CurrentVesrionInsuranceFilekey As Integer

        CurrentVesrionInsuranceFilekey = oQuote.InsuranceFileKey

        Dim oParty As NexusProvider.BaseParty = Session(CNParty)
        Dim AgentComm As Integer

        txtPolicyNo.Text = oQuote.InsuranceFileRef
        Select Case True
            Case TypeOf oParty Is NexusProvider.CorporateParty
                With CType(oParty, NexusProvider.CorporateParty)
                    txtPolicyHolder.Text = .CompanyName
                End With
            Case TypeOf oParty Is NexusProvider.PersonalParty
                With CType(oParty, NexusProvider.PersonalParty)
                    txtPolicyHolder.Text = .Title & " " & .Forename & " " & .Lastname
                End With
        End Select
        ' commented  on 17/7/2012
        'txtInceptionDate.Text = Quote.InceptionDate.ToShortDateString
        ' commented on 17/7/2012

        txtInceptionDate.Text = Quote.inceptionTPI.ToShortDateString
        txtBrokerCode.Text = Quote.AgentCode

        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim obtnRequote As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnRequote"), LinkButton)
        If Session(CNMTAType) IsNot Nothing And Session(CNRenewal) Is Nothing Then
            If (Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Or Session(CNMTAType) = MTAType.CANCELLATION) Then
                If (Not obtnRequote Is Nothing) Then
                    obtnRequote.Text = "Requote"
                End If
            End If
        End If

        For Each oAddress As NexusProvider.Address In oParty.Addresses
            If oAddress.AddressType = NexusProvider.AddressType.CorrespondenceAddress Then
                txtAddressLine1.Text = oAddress.Address1
                txtAddressLine2.Text = oAddress.Address2
                txtAddressLine3.Text = oAddress.Address3
                txtAddressLine4.Text = oAddress.Address4
                txtPostCode.Text = oAddress.PostCode
                Exit For
            End If
        Next


        'Read Values from XML
        If oQuote.Risks.Count > 0 Then
            ReadFromXML(Quote, CNCurrentRiskKey)
        End If

        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

        'Find Premium Transaction
        Dim oAccountdetails As New NexusProvider.AccountDetails
        Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults = Nothing
        Dim oManupulatedAccountDetailsCollection As New NexusProvider.AccountDetailsDefaults

        Dim oAccountSearchCriteria = New NexusProvider.AccountSearchCriteria
        Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection

        oAccountSearchCriteria.ShortCode = Quote.AgentCode
        ' calls the web method for obtaining data

        oAccountSearchResultCollection = oWebservice.FindAccounts(oAccountSearchCriteria)

        ' checks if the collections returns any value and assigns the values to Grid


        oAccountdetails.InsuranceRef = Quote.InsuranceFileRef
        oAccountdetails.BranchCode = Quote.BranchCode

        'Following line will add Account key to search transaction only if Agency business
        If oQuote.AgentCode <> "" Then
            oAccountdetails.AccountKey = oAccountSearchResultCollection(0).AccountKey
        End If
        oAccountDetailsCollection = oWebservice.GetAccountDetails(oAccountdetails, Quote.BranchCode)

        'Calculation of the Gross
        For iCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
            If Quote.CoverStartDate >= oAccountDetailsCollection.AccountDetails(iCount).EffectiveDate Then
                If oAccountDetailsCollection.AccountDetails(iCount).MediaRef.Trim.ToUpper = "GROSS" Then
                    oManupulatedAccountDetailsCollection.AccountDetails.Add(oAccountDetailsCollection.AccountDetails(iCount))
                End If
            End If
        Next
        'Calculation for the Tax and Comm

        For iCount As Integer = 0 To oManupulatedAccountDetailsCollection.AccountDetails.Count - 1
            For jCount As Integer = 0 To oAccountDetailsCollection.AccountDetails.Count - 1
                'If Quote.CoverStartDate >= oAccountDetailsCollection.AccountDetails(iCount).EffectiveDate Then
                If oManupulatedAccountDetailsCollection.AccountDetails(iCount).DocRef.Trim.ToUpper = oAccountDetailsCollection.AccountDetails(jCount).DocRef.Trim.ToUpper Then
                    If oAccountDetailsCollection.AccountDetails(jCount).MediaRef.Trim.ToUpper = "TAX" Then
                        oManupulatedAccountDetailsCollection.AccountDetails(iCount).Tax += oAccountDetailsCollection.AccountDetails(jCount).Amount
                        oManupulatedAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount += oAccountDetailsCollection.AccountDetails(jCount).OutstandingAmount
                    End If
                    If oAccountDetailsCollection.AccountDetails(jCount).MediaRef.Trim.ToUpper = "COMM" Then
                        oManupulatedAccountDetailsCollection.AccountDetails(iCount).Comm += oAccountDetailsCollection.AccountDetails(jCount).Amount

                        oManupulatedAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount += oAccountDetailsCollection.AccountDetails(jCount).OutstandingAmount
                        AgentComm = oManupulatedAccountDetailsCollection.AccountDetails(iCount).Comm
                    End If
                End If
                'End If
            Next
        Next

        'SB CHANGE
        Dim iInsuranceFilekey As Integer
        Dim oHeaderAndAgentCommission As NexusProvider.HeaderAndAgentCommission
        Dim oAgentCommissionCollection As NexusProvider.AgentCommissionCollection

        Dim cTotalAmount As Double = 0.0

        iInsuranceFilekey = Quote.InsuranceFileKey
        'oEditAgentCommission = DirectCast(Session(CNAgentCommission), NexusProvider.AgentCommission)
        'On load of the page first time
        oHeaderAndAgentCommission = oWebservice.GetHeaderAndAgentCommissionByKey(iInsuranceFilekey)
        oAgentCommissionCollection = oHeaderAndAgentCommission.AgentCommission

        Dim count As Double
        For count = 0 To oAgentCommissionCollection.Count - 1
            cTotalAmount = oAgentCommissionCollection(count).CommissionValue + cTotalAmount
        Next

        'Calculation for the Settled Amount
        For iCount As Integer = 0 To oManupulatedAccountDetailsCollection.AccountDetails.Count - 1
            'If Quote.CoverStartDate >= oAccountDetailsCollection.AccountDetails(iCount).EffectiveDate Then
            oManupulatedAccountDetailsCollection.AccountDetails(iCount).Settled = (oManupulatedAccountDetailsCollection.AccountDetails(iCount).Amount + oManupulatedAccountDetailsCollection.AccountDetails(iCount).Tax + oManupulatedAccountDetailsCollection.AccountDetails(iCount).Comm) - oManupulatedAccountDetailsCollection.AccountDetails(iCount).OutstandingAmount
            'End If
        Next

        gvPremiumTransactions.DataSource = oManupulatedAccountDetailsCollection.AccountDetails
        'PM017241 Jai Prakash 03/02/2012
        ViewState(CNPremiumTransactions) = oManupulatedAccountDetailsCollection.AccountDetails
        gvPremiumTransactions.DataBind()


        If Session(CNMode) = Mode.View Then
            'Display cover details
            CvrDtls.Visible = True
            TransDetails.Visible = True
            PremDetails.Visible = True
            QuoteDocument.Visible = False
        End If


        If Session(CNMode) = Mode.View Then
            'View mode Should display POlicy Status 'Live' 
            txtStatus.Text = "Live"
            'Btn_PreviousYear.Visible = False
            For Each Ctrl As Control In Me.Parent.Parent.Controls
                If Ctrl.ClientID = "ctl00_cntMainBody_lblPageheader" Then
                    DirectCast(Ctrl, Label).Text = "Policy Reference " & oQuote.InsuranceFileRef
                End If
            Next
        End If


        If Quote.InsuranceFileTypeCode = "WRITTEN" Then
            txtStatus.Text = "WRITTEN"
        Else
            lkList_POLICYSTATUS.Value = PCATLIN__POLICYSTATUS.Text
            txtStatus.Text = lkList_POLICYSTATUS.Text
        End If

        If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Or Session(CNMTAType) = MTAType.CANCELLATION Or (Session(CNMTAType) = MTAType.REINSTATEMENT) Then
            txtStatus.Text = "Live"
        End If
        If Session(CNRenewal) Then
            txtStatus.Text = "Live"
        End If
        If Quote.InsuranceFileTypeCode = "QUOTE" Or Quote.InsuranceFileTypeCode = "" Then
            ltHeading.Text = "Quote Summary"
        End If


        GetStandardWording()
        lblPremiumValue.Text = New Money(Quote.NetTotal, Session(CNCurrenyCode)).Formatted
        lblTotalTaxValue.Text = New Money(Quote.TaxTotal, Session(CNCurrenyCode)).Formatted



        If Quote.AgentCode <> "" Then
            Dim iIndex As Integer
            iIndex = 0
            Dim Premium, CommissionValue, commissionRate
            Dim oAgentCommission
            oAgentCommission = oWebservice.GetAgentCommission(Quote.InsuranceFileKey)
            If oAgentCommission IsNot Nothing Then
                With oAgentCommission
                    If oAgentCommission.AgentCommission.count > 0 Then
                        Dim oSelectAgentCommission As NexusProvider.AgentCommission = .AgentCommission(iIndex)
                        .InsuranceFileKey = Quote.InsuranceFileKey
                        Premium = oSelectAgentCommission.Premium
                        If .AgentCommission(iIndex).Agent IsNot Nothing And IsNumeric(Replace(PCATLIN__STRCOMMISSION.Text, "%", "")) Then

                            .AgentCommission(iIndex).CalculatedCommissionValue = Math.Round(((Premium * Replace(PCATLIN__STRCOMMISSION.Text, "%", "")) / 100) * 100) / 100
                            .AgentCommission(iIndex).CalculatedCommissionValueSpecified = True
                            If .AgentCommission(iIndex).IsValue = False Then
                                .AgentCommission(iIndex).CommissionRate = Replace(PCATLIN__STRCOMMISSION.Text, "%", "") 'hdCommissionRate.Value
                            Else
                                .AgentCommission(iIndex).CommissionRate = Math.Round(((Premium * Replace(PCATLIN__STRCOMMISSION.Text, "%", "")) / 100) * 100) / 100
                            End If
                            .AgentCommission(iIndex).CommissionValue = Math.Round(((Premium * Replace(PCATLIN__STRCOMMISSION.Text, "%", "")) / 100) * 100) / 100
                            .AgentCommission(iIndex).OverRideReason = "" 'txtOverride.Text
                            .AgentCommission(iIndex).IsAmended = True
                        Else
                            .AgentCommission(iIndex).CalculatedCommissionValue = .AgentCommission(iIndex).CommissionValue
                            If .AgentCommission(iIndex).IsValue Then
                                .AgentCommission(iIndex).CommissionRate = .AgentCommission(iIndex).CommissionValue
                            End If
                            .AgentCommission(iIndex).CalculatedCommissionValueSpecified = True
                        End If

                        If .AgentCommission(0).MaximumRate() IsNot Nothing AndAlso .AgentCommission(0).MaximumRate() > 0 Then
                            If .AgentCommission(0).MaximumRate() <= .AgentCommission(iIndex).CommissionRate Then
                                ' lbl_errmessage.Text = GetLocalResourceObject("lbl_errormessage") & " " & .AgentCommission(0).MaximumRate()
                                Dim olblbuynow As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnBuy"), LinkButton)
                                olblbuynow.Visible = False
                            Else
                                '  lbl_errmessage.Visible = False
                                ' lbl_errmessage.Text = ""
                                If Session(CNMode) <> Mode.View And Quote.InsuranceFileKey = CurrentVesrionInsuranceFilekey Then
                                    oAgentCommission = oWebservice.UpdateAgentCommission(oAgentCommission)
                                    Session(CNAgentCommission) = oAgentCommission
                                End If
                            End If
                        Else
                            ' lbl_errmessage.Visible = False
                            ' lbl_errmessage.Text = ""
                        End If

                    End If
                End With


                If oAgentCommission IsNot Nothing Then
                    With oAgentCommission
                        If oAgentCommission.AgentCommission.count > 0 Then
                            If Quote.NetTotal > 0 Then
                                'The commission field should always be a negative value if the Total_Premium value is a positive value.
                                lblTotalCommissionValue.Text = "-" & New Money(oAgentCommission.LeadAgentTotalCommission, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                                'HidTotalCommission hold only numeric value 
                                HidTotalCommissionValue.Value = "-" & oAgentCommission.LeadAgentTotalCommission
                            Else
                                'The Commission field  should always be a Positive number if the Total_Premium value is a negative value.
                                lblTotalCommissionValue.Text = Replace(New Money(oAgentCommission.LeadAgentTotalCommission, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString, "-", "")
                                'HidTotalCommission hold only numeric value 
                                HidTotalCommissionValue.Value = Replace(oAgentCommission.LeadAgentTotalCommission, "-", "")
                            End If
                        Else
                            ' lbl_errmessage.Visible = False
                            ' lbl_errmessage.Text = ""
                        End If
                    End With
                Else

                    lblTotalCommissionValue.Text = New Money(0, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                    HidTotalCommissionValue.Value = "0"
                End If

            Else
                ' lbl_errmessage.Visible = False
                lblTotalCommissionValue.Text = New Money(0, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                HidTotalCommissionValue.Value = "0"
            End If
        Else
           
        End If

        'Calculatation of the Earned premium
        'If Session(CNMode) = Mode.View Then
        Dim dEarnedPremium As Double
        Dim Old_dCurrentDate As Date = DirectCast(Quote, NexusProvider.Quote).CoverStartDate '
        Dim Old_tsDaysElapsed As TimeSpan
        Dim Old_tsCoverDuration As Long
        Dim Old_tsNoofdays_Expired As Long
        Dim Old_dEarnedPremium As Double
        Dim Old_iDaysElapsed As Integer
        Dim Old_iDaysCover As Integer
        Dim Str_PolicyHistory As String
        Dim NetTotal As Decimal
        Dim tsNoofdays_Expired


        If Quote.Risks.Count > 0 Then
            Dim xmlTR1 As New XmlTextReader(New System.IO.StringReader(Quote.Risks(0).XMLDataset))
            Dim Doc1 As New XmlDocument
            Dim Insurance_file_cnt As Integer
            Dim CoverStartDate As Date
            Dim CoverEndDate As Date
            Doc1.Load(xmlTR1)
            xmlTR1.Close()

            Dim xmlNodes As XmlNodeList = Doc1.SelectNodes("//" & Session.Item(CNDataModelCode) & "_POLICY_BINDER/GENERAL/POLICYMEMOS")
            For Each xmlNode As XmlNode In xmlNodes
                Try
                    Str_PolicyHistory = xmlNode.Attributes("TEXT").Value
                    Dim intX As Integer
                    Dim intY As Integer
                    Dim astrSplitItems1 As String()
                    ' Loop through words in string
                    Dim checkPrevYear As Date

                    Dim astrSplitItems As String() = Split(Str_PolicyHistory, "]")
                    ' Display each word. Note that punctuation is handled correctly.
                    For intX = 0 To UBound(astrSplitItems) - 1

                        'added by  to get the cover enddate
                        Try
                            astrSplitItems1 = Split(astrSplitItems(intX + 1), ",")
                            If (Not astrSplitItems1(0) = "") Then
                                If Not checkPrevYear = astrSplitItems1(1) Then
                                    Old_dCurrentDate = astrSplitItems1(1)
                                    checkPrevYear = CDate(astrSplitItems1(1))

                                End If
                            End If
                        Catch ex As System.Exception
                        End Try
                        'added by to get the cover enddate
                        astrSplitItems1 = Split(astrSplitItems(intX), ",")
                        Insurance_file_cnt = astrSplitItems1(0)
                        CoverStartDate = astrSplitItems1(1)
                        CoverEndDate = astrSplitItems1(2)
                        Dim astrSplitItems1_3 As Integer = Replace(astrSplitItems1(3), "]", "")

                        If astrSplitItems1_3 > 0 Then
                            NetTotal = astrSplitItems1_3
                        End If


                        Old_tsDaysElapsed = Old_dCurrentDate.Subtract(CoverStartDate)
                        Old_tsCoverDuration = DateDiff(DateInterval.Day, CoverStartDate, CoverEndDate)
                        Old_tsNoofdays_Expired = DateDiff(DateInterval.Day, CoverStartDate, Old_dCurrentDate)
                        Old_iDaysElapsed = Old_tsDaysElapsed.Days
                        Old_iDaysCover = Old_tsCoverDuration + 1
                        If Insurance_file_cnt < oQuote.InsuranceFileKey Then
                            If Old_iDaysCover > 0 Then

                                Old_dEarnedPremium = Old_dEarnedPremium + ((NetTotal / Old_iDaysCover) * Old_tsNoofdays_Expired)
                            Else
                                Old_dEarnedPremium = Old_dEarnedPremium + 0
                            End If
                        End If
                    Next
                Catch ex As System.Exception

                End Try
            Next
        End If

        'Calculatation of the Earned premium
        Dim Curr_dCurrentDate As Date = Date.Today
        Dim Curr_tsDaysElapsed As TimeSpan = Curr_dCurrentDate.Subtract(Quote.CoverStartDate)
        Dim Curr_tsCoverDuration As Long = DateDiff(DateInterval.Day, Quote.CoverStartDate, Quote.CoverEndDate)
        Dim Curr_tsNoofdays_Expired As Long = DateDiff(DateInterval.Day, Quote.CoverStartDate, Curr_dCurrentDate)
        Dim Curr_dEarnedPremium As Decimal
        Dim Curr_iDaysElapsed As Integer = Curr_tsDaysElapsed.Days
        Dim Curr_iDaysCover As Integer = Curr_tsCoverDuration + 1
        If lblTotalCommissionValue.Text = "" Then
            lblTotalCommissionValue.Text = "0"
            HidTotalCommissionValue.Value = "0"
        End If
        'if year is completed
        If Curr_iDaysCover <= Curr_tsNoofdays_Expired Then

            Curr_tsNoofdays_Expired = Curr_iDaysCover

        End If



        If UCase(oQuote.InsuranceFileTypeCode) = "POLICY" Then
            If Curr_iDaysCover > 0 Then
                dEarnedPremium = dEarnedPremium + (((Quote.GrossTotal - (Math.Abs(CDbl(HidTotalCommissionValue.Value)) + Quote.TaxTotal)) / Curr_iDaysCover) * Curr_tsNoofdays_Expired)
            Else
                dEarnedPremium = dEarnedPremium + 0
            End If

        Else
            If Curr_iDaysCover > 0 Then
                dEarnedPremium = (CDbl(dEarnedPremium) + CDbl(Old_dEarnedPremium) + (((Quote.GrossTotal - (Math.Abs(CDbl(HidTotalCommissionValue.Value)) + Quote.TaxTotal)) / Curr_iDaysCover) * Curr_tsNoofdays_Expired))
            Else
                dEarnedPremium = (dEarnedPremium + Old_dEarnedPremium + 0)
            End If
        End If

        If dEarnedPremium < 0 Then

            dEarnedPremium = 0

        End If

        txtEarnedPremium.Text = New Money(dEarnedPremium, Session(CNCurrenyCode)).Formatted
        'Calculate Incurred Claims
        Dim oClaimColl As NexusProvider.ClaimCollection
        Dim oClaimSearchCriteria As New NexusProvider.ClaimSearchCriteria
        Dim oClaimVersions As NexusProvider.VersionsCollections = Nothing
        Dim iHighest As Integer = 0
        Dim dIncurredClaim As Decimal
        oClaimSearchCriteria.InsuranceFileRef = Quote.InsuranceFileRef

        'commented by ss 
        'oClaimSearchCriteria.LossDateFrom = oQuote.CoverStartDate
        'oClaimSearchCriteria.LossDateTo = oQuote.CoverEndDate
        'commented by ss

        oClaimSearchCriteria.LossDateFrom = DirectCast(Quote, NexusProvider.Quote).InceptionTPI
        oClaimSearchCriteria.LossDateTo = DirectCast(Quote, NexusProvider.Quote).CoverEndDate
        oClaimSearchCriteria.IncludeClosedClaim = True
        oClaimColl = oWebservice.FindClaim(oClaimSearchCriteria)

        'Set Net Loss Ratio , Incurred To date & No Of Claim to 0 
        txtNetLossRatio.Text = "0.00%" 'New Money(0, Session(CNCurrenyCode)).Formatted
        txtIncurredTodate.Text = New Money(0, Session(CNCurrenyCode)).Formatted
        hypNoOfClaimty.Text = 0
        hypNoOfClaimty.NavigateUrl = "~/Claims/FindClaim.aspx?Policyno=" & txtPolicyNo.Text & "&LossStartDate=" & oQuote.CoverStartDate & "&LossEndDate=" & Quote.CoverEndDate

        If oClaimColl IsNot Nothing AndAlso oClaimColl.Count > 0 Then
            For iCount As Integer = 0 To oClaimColl.Count - 1
                iHighest = 0
                oClaimVersions = oWebservice.GetVersionsForClaim(oClaimColl(iCount).ClaimNumber.Trim)
                Dim thisSalvageValueAllVer As Decimal = 0
                Dim thisThirdPartyRecoveryValueAllver As Decimal = 0
                If oClaimVersions IsNot Nothing AndAlso oClaimVersions.Count > 0 Then
                    For jCount As Integer = 0 To oClaimVersions.Count - 1

                        thisSalvageValueAllVer = thisSalvageValueAllVer + oClaimVersions(jCount).ThisSalvageRecovery
                        thisThirdPartyRecoveryValueAllver = thisThirdPartyRecoveryValueAllver + oClaimVersions(jCount).ThisThirdPartyRecovery
                        If oClaimVersions(jCount).Version > iHighest Then
                            iHighest = oClaimVersions(jCount).Version
                        End If
                    Next

                    'Pull the values from the highest version
                    For iVersionCount As Integer = 0 To oClaimVersions.Count - 1
                        If oClaimVersions(iVersionCount).Version = iHighest Then

                            dIncurredClaim = dIncurredClaim + oClaimVersions(iVersionCount).TotalIncurred - (thisSalvageValueAllVer + thisThirdPartyRecoveryValueAllver)

                            Exit For
                        End If
                    Next
                End If
            Next
            dEarnedPremium = Math.Round(dEarnedPremium, 2)
            txtIncurredTodate.Text = New Money(dIncurredClaim, Session(CNCurrenyCode)).Formatted 'CStr(dIncurredClaim)
            If dEarnedPremium > 0 Then
                txtNetLossRatio.Text = CStr(FormatNumber((dIncurredClaim / dEarnedPremium) * 100)) + "%" 'New Money( CStr((dIncurredClaim / dEarnedPremium) * 100), Session(CNCurrenyCode)).Formatted
            Else
                'txtNetLossRatio.Text = New Money(0, Session(CNCurrenyCode)).Formatted
                txtNetLossRatio.Text = "0.00%"
            End If

            hypNoOfClaimty.NavigateUrl = "~/Claims/FindClaim.aspx?Policyno=" & txtPolicyNo.Text & "&LossStartDate=" & Quote.CoverstartDate & "&LossEndDate=" & Quote.CoverEndDate
            hypNoOfClaimty.Text = oClaimColl.Count.ToString
        End If
    End Sub

    Protected Sub gvPremiumTransactions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPremiumTransactions.Load

    End Sub
    Protected Sub gvPremiumTransactions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPremiumTransactions.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            If CDate(e.Row.Cells(1).Text) >= CDate(txtInceptionDate.Text) And CDate(e.Row.Cells(1).Text) < CDate(txtInceptionDate.Text).AddYears(1) Then
                e.Row.Cells(3).Text = New Money(CType(e.Row.DataItem, NexusProvider.AccountDetails).Amount, CType(e.Row.DataItem, NexusProvider.AccountDetails).CurrencyCode).Formatted
                e.Row.Cells(4).Text = New Money(CType(e.Row.DataItem, NexusProvider.AccountDetails).Tax, CType(e.Row.DataItem, NexusProvider.AccountDetails).CurrencyCode).Formatted
                e.Row.Cells(5).Text = New Money(CType(e.Row.DataItem, NexusProvider.AccountDetails).Comm, CType(e.Row.DataItem, NexusProvider.AccountDetails).CurrencyCode).Formatted
                e.Row.Cells(6).Text = New Money(CType(e.Row.DataItem, NexusProvider.AccountDetails).OutstandingAmount, CType(e.Row.DataItem, NexusProvider.AccountDetails).CurrencyCode).Formatted
                e.Row.Cells(7).Text = New Money(CType(e.Row.DataItem, NexusProvider.AccountDetails).Settled, CType(e.Row.DataItem, NexusProvider.AccountDetails).CurrencyCode).Formatted

            Else
                e.Row.Visible = False

            End If
        End If
    End Sub

    Protected Sub gvPremiumTransactions_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPremiumTransactions.PageIndexChanging
        gvPremiumTransactions.DataSource = ViewState(CNPremiumTransactions)
        gvPremiumTransactions.PageIndex = e.NewPageIndex
        gvPremiumTransactions.DataBind()
    End Sub

    Sub ReadFromXML(ByVal Quote, ByVal CNCurrentRiskKey)
        Dim oControl As Object
        Dim sControlName(), sChildControlName() As String
        Dim sControlValue, sChildControlValue As String

        Dim xmlTR As New XmlTextReader(New System.IO.StringReader(Quote.Risks(0).XMLDataset))
        Dim Doc As New XmlDocument

        Doc.Load(xmlTR)
        xmlTR.Close()

        For Each oControl In Me.Controls
            If oControl.id IsNot Nothing Then
                sControlName = Regex.Split(oControl.ID.ToUpper(), "__")
                sControlValue = String.Empty

                Select Case oControl.GetType.Name.ToUpper
                    Case "HTMLGENERICCONTROL"
                        Dim oChildControl As Object
                        For Each oChildControl In oControl.Controls
                            If oChildControl.id IsNot Nothing Then
                                sChildControlName = Regex.Split(oChildControl.ID.ToUpper(), "__")
                                sChildControlValue = String.Empty

                                Select Case oChildControl.GetType.Name.ToUpper
                                    Case "LABEL"
                                        If ReadAttributeFromXML(Doc, sChildControlName, sChildControlValue, Nothing) Then
                                            Dim dtAttribute As DateTime
                                            If DateTime.TryParseExact(sChildControlValue, "yyyy-MM-dd HH:mm:ss", _
                                                InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                                sChildControlValue = dtAttribute.ToShortDateString

                                            End If

                                            CType(oChildControl, Label).Text = sChildControlValue
                                        End If

                                    Case "TEXTBOX"
                                        If ReadAttributeFromXML(Doc, sChildControlName, sChildControlValue, Nothing) Then

                                            Dim dtAttribute As DateTime
                                            If DateTime.TryParseExact(sChildControlValue, "yyyy-MM-dd HH:mm:ss", _
                                                InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                                sChildControlValue = dtAttribute.ToShortDateString

                                            End If

                                            CType(oChildControl, TextBox).Text = sChildControlValue
                                            CType(oChildControl, TextBox).ReadOnly = True
                                        End If
                                End Select
                            End If
                        Next

                    Case "LABEL"
                        If ReadAttributeFromXML(Doc, sControlName, sControlValue, Nothing) Then


                            Dim dtAttribute As DateTime
                            If DateTime.TryParseExact(sControlValue, "yyyy-MM-dd HH:mm:ss", _
                                InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                sControlValue = dtAttribute.ToShortDateString

                            End If

                            CType(oControl, Label).Text = sControlValue
                        End If

                    Case "TEXTBOX"
                        If ReadAttributeFromXML(Doc, sControlName, sControlValue, Nothing) Then

                            Dim dtAttribute As DateTime
                            If DateTime.TryParseExact(sControlValue, "yyyy-MM-dd HH:mm:ss", _
                                InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                sControlValue = dtAttribute.ToShortDateString

                            End If

                            CType(oControl, TextBox).Text = sControlValue
                            CType(oControl, TextBox).ReadOnly = True
                        Else
                            CType(oControl, TextBox).Text = String.Empty
                        End If

                    Case "CHECKBOX"
                        If ReadAttributeFromXML(Doc, sControlName, sControlValue, Nothing) Then
                            CType(oControl, CheckBox).Checked = IIf(sControlValue = 1, True, False)
                        End If

                    Case "LOOKUPLIST"
                        sControlValue = Session.Item(oControl.ID)
                        If ReadAttributeFromXML(Doc, sControlName, sControlValue, Nothing) Then
                            oControl.Value = sControlValue
                        End If
                End Select
            End If
        Next
    End Sub
    Function ReadAttributeFromXML(ByRef r_oDoc As XmlDocument, _
                                   ByVal v_sControlName() As String, _
                                   ByRef r_sControlValue As String, _
                                   Optional ByRef r_sOI As String = "", _
                                   Optional ByVal v_sDataModelCode As String = Nothing) As Boolean

        Try
            r_sControlValue = String.Empty
            Dim oNode As XmlNode

            If String.IsNullOrEmpty(r_sOI) Then
                If v_sDataModelCode Is Nothing Then
                    oNode = r_oDoc.SelectSingleNode("//" & Session.Item(CNDataModelCode).ToString() & "_POLICY_BINDER")
                Else
                    oNode = r_oDoc.SelectSingleNode("//" & v_sDataModelCode.Trim & "_POLICY_BINDER")
                End If

                If oNode IsNot Nothing Then

                    Dim oOI As New Collections.Stack()
                    r_sOI = oNode.Attributes("OI").Value
                    oOI.Push(r_sOI)

                    oNode = oNode.SelectSingleNode("//" & v_sControlName(0))
                    If oNode IsNot Nothing Then
                        r_sOI = oNode.Attributes("OI").Value
                        oOI.Push(r_sOI)
                    End If
                    Session.Item(CNOI) = oOI
                End If
            End If

            oNode = r_oDoc.SelectSingleNode("//*[@OI='" & r_sOI & "']")
            If oNode IsNot Nothing Then

                If oNode.Name.Equals(v_sControlName(0)) Then
                    'element name retrieved with the OI matches that passed in, so we are in the right element to read attributes
                    r_sControlValue = oNode.Attributes(v_sControlName(1)).Value
                Else
                    'Not matching, so we go back up the tree one level are look for the element amongst the siblings
                    Dim oOI As Collections.Stack = Session.Item(CNOI)
                    r_sOI = oOI.Pop()

                    oNode = r_oDoc.SelectSingleNode("//*[@OI='" & oOI.Peek() & "']/" & v_sControlName(0))

                    'DH - Copying a stack from session does it byref, so the previous OI popped needs to be pushed back in
                    oOI.Push(r_sOI)
                    Session.Item(CNOI) = oOI

                    If oNode IsNot Nothing Then
                        r_sControlValue = oNode.Attributes(v_sControlName(1)).Value

                    End If
                End If
            End If
        Catch ex As System.Exception
            ReadAttributeFromXML = False
        End Try
        If String.IsNullOrEmpty(r_sControlValue) Then
            ReadAttributeFromXML = False
        Else
            ReadAttributeFromXML = True
        End If

    End Function
    Sub GetStandardWording()
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim dtc As NexusProvider.DocumentTemplateCollection
        If oQuote IsNot Nothing Then
            dtc = oWebservice.GetStandardPolicyWordings(oQuote.InsuranceFileKey)

            grdStdWording.DataSource = dtc
            ViewState(CNStandardWording) = dtc
            grdStdWording.DataBind()
            grdStdWording.Visible = True
            'If grdStdWording.Rows.Count > 0 Then

            'End If
        End If
    End Sub
    Protected Sub grdStdWording_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdStdWording.PageIndexChanging
        grdStdWording.DataSource = ViewState(CNStandardWording)
        grdStdWording.PageIndex = e.NewPageIndex
        grdStdWording.DataBind()
    End Sub
    Protected Sub grdStdWording_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdStdWording.Load
        If grdStdWording.PageCount = 1 Then
            grdStdWording.AllowPaging = False
        End If
    End Sub
    Protected Sub Btn_PreviousYear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_PreviousYear.Click

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
        Dim sPolicyRef As String = String.Empty
        Dim sShortName As String = String.Empty
        Dim PreviousYear_Status As Boolean = False

        'Finding Client ShortName
        If Session(CNParty) IsNot Nothing Then
            Select Case True
                Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                    With CType(Session(CNParty), NexusProvider.CorporateParty)
                        If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                            sShortName = .ClientSharedData.ShortName.Trim()
                        ElseIf String.IsNullOrEmpty(.UserName) = False Then
                            sShortName = .UserName
                        End If
                    End With
                Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                    With CType(Session(CNParty), NexusProvider.PersonalParty)
                        If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                            sShortName = .ClientSharedData.ShortName.Trim()
                        ElseIf String.IsNullOrEmpty(.UserName) = False Then
                            sShortName = .UserName
                        End If
                    End With
            End Select
        End If
        If Session.Item(CNQuote) IsNot Nothing Then
            If Session.Item(CNPolicy_Summary) IsNot Nothing Then
                Dim oPolicySummary As NexusProvider.PolicySummary
                oPolicySummary = Session.Item(CNPolicy_Summary)
                If oPolicySummary IsNot Nothing AndAlso oPolicySummary.Reference IsNot Nothing Then
                    sPolicyRef = oPolicySummary.Reference
                End If
                If sPolicyRef = "" Then
                    sPolicyRef = CType(Session.Item(CNQuote), NexusProvider.Quote).InsuranceFileRef
                End If
            Else
                sPolicyRef = CType(Session.Item(CNQuote), NexusProvider.Quote).InsuranceFileRef
            End If
        End If






        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim Filetypecode = oQuote.InsuranceFileKey
        Dim InsuranceFileType = oQuote.InsuranceFileTypeCode
        Dim CurrentPolVersion As Integer
        'to get all policy versions and the previous year policy

        Dim oPolicy As NexusProvider.PolicyCollection
        Dim PreviouPolicyVersion As Integer
        If (sPolicyRef <> String.Empty) Then
            'If Current Version is  QUOTE or POLICY  then we are not allow to Search any Previous Detail
            If InsuranceFileType <> "QUOTE" Or InsuranceFileType <> "POLICY" Then
                'Finding All Policy version Details
                oInsuranceFileDetailsCollection = oWebService.FindPolicy(UCase(Trim(sPolicyRef)), "", sShortName, NexusProvider.InsuranceFileTypes.ALL, False, Nothing)
                'oInsuranceFileDetailsCollection = oWebService.FindPolicy(UCase(Trim(sPolicyRef)), "", sShortName, NexusProvider.InsuranceFileTypes.RENEWAL, False, Nothing)
                'added to get all policy versions
                oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)


                Dim countPolVersion As Integer = oPolicy.Count
                For countPOlver As Integer = countPolVersion To 0 Step -1
                    If Trim(oPolicy(countPOlver - 1).InsuranceFileTypeCode) = "POLICY" And oQuote.InsuranceFileKey = oPolicy(countPOlver - 1).InsuranceFileKey Then
                        PreviouPolicyVersion = countPOlver - 1
                        Exit For
                    End If

                Next


                For countPOlver As Integer = PreviouPolicyVersion To 0 Step -1
                    If Trim(oPolicy(countPOlver - 1).InsuranceFileTypeCode) = "POLICY" Then
                        PreviouPolicyVersion = PreviouPolicyVersion - 1
                        Exit For
                    End If

                Next




                For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection

                    oInsuranceFileDetails.InsuranceFileKey = oPolicy(PreviouPolicyVersion).InsuranceFileKey
                    If Filetypecode > (oInsuranceFileDetails.InsuranceFileKey) Then
                        Dim oTempQuote As New NexusProvider.Quote
                        oTempQuote = oWebService.GetHeaderAndSummariesByKey(oInsuranceFileDetails.InsuranceFileKey)
                        'If oQuote.CoverStartDate.AddYears(-1).ToShortDateString() <= oTempQuote.CoverStartDate And oQuote.CoverStartDate.AddYears(1).ToShortDateString() >= oTempQuote.CoverStartDate Then
                        'If (DateDiff(DateInterval.Day, oTempQuote.CoverStartDate, oQuote.CoverStartDate) > 364 And oTempQuote.InsuranceFileTypeCode = oQuote.InsuranceFileTypeCode) Then
                        'If oTempQuote.InsuranceFileTypeCode = (oQuote.InsuranceFileTypeCode) Then
                        PreviousYear_Status = True
                        'Catlin have only single Risk 
                        oWebService.GetRisk(oTempQuote.Risks(0).Key, 0, oTempQuote)
                        oWebService.GetHeaderAndRisksByKey(oTempQuote)

                        CommonProperties(oTempQuote, oTempQuote.Risks(0).Key)
                        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        Dim oCtrMultiRisk As Control = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("MultiRisk1"), Control)

                        If (Not oCtrMultiRisk Is Nothing) Then
                            oCtrMultiRisk.Visible = False
                        End If

                        Dim oUpdatePanel1 As UpdatePanel = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("UpdatePanel1"), UpdatePanel)

                        If (Not oUpdatePanel1 Is Nothing) Then
                            oUpdatePanel1.Visible = False
                        End If

                        CvrDtls.Visible = False
                        '  Btn_CurrentYear.Visible = True
                        ' Btn_PreviousYear.Visible = False
                        Exit For
                    End If
                    'End If
                    'End If

                Next
            End If
        End If
        'If Btn_PreviousYear.Visible = False Then
        '    Btn_CurrentYear.Visible = True
        'End If
        If PreviousYear_Status = False Then
            'lbl_errmessage.Visible = False
            'lbl_errmessage.Text = ""
            'Btn_PreviousYear.Enabled = False
            'Btn_CurrentYear.Enabled = True
        End If
    End Sub

    Public Function GetDatafromXML(ByVal Xpath As String, ByVal field As String) As String
        Dim strVal As String = ""
        If System.Web.HttpContext.Current.Session(CNQuote) IsNot Nothing Then
            Dim oQuote As NexusProvider.Quote = System.Web.HttpContext.Current.Session(CNQuote)

            Dim Maxcount As Integer
            If oQuote.Risks.Count > 0 Then
             
                Maxcount = oQuote.Risks.Count - 1
                Dim srDataset As New System.IO.StringReader(oQuote.Risks(Maxcount).XMLDataset)
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument
                Doc.Load(xmlTR)
                xmlTR.Close()

                Dim oNode As XmlNode = Doc.SelectSingleNode("//" & Session.Item(CNDataModelCode) & Xpath)
                If oNode IsNot Nothing Then
                    If oNode.Attributes(field) IsNot Nothing Then
                        strVal = oNode.Attributes(field).InnerText
                    Else
                        strVal = ""
                    End If
                Else
                    strVal = ""
                End If
            End If
            
        End If
        GetDatafromXML = strVal
    End Function

    Protected Sub Btn_CurrentYear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_CurrentYear.Click
        Session("CHECKRENEWAL") = Nothing

        Response.Redirect("~/secure/PremiumDisplay.aspx")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        If oQuote Is Nothing Then
            Exit Sub 'Session(CNQuote) has been cleared on save quote
        End If
        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim olblRenewalInvite As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnPrint"), LinkButton)
        Dim olblbuynow1 As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnBuy"), LinkButton)
        If Session(CNRenewal) IsNot Nothing Then
            'If oQuote.InsuranceFileTypeCode IsNot Nothing AndAlso oQuote.InsuranceFileTypeCode <> "WRITTEN" And Session(CNRenewal) Is Nothing Then
            '    If (Not olblbuynow1 Is Nothing) Then
            '        olblbuynow1.Visible = False
            '    End If
            'Else
            '    If (Not olblbuynow1 Is Nothing) Then
            '        olblbuynow1.Visible = True
            '    End If
            'End If
            'olbWritten = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnWrite"), LinkButton)
            sWrittenStatus = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.AllowWrittenStatus, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            If (olblRenewalInvite.Visible = False AndAlso oQuote.InsuranceFileTypeCode = "RENEWAL" AndAlso sWrittenStatus = "1") Then
                If (Not olbWritten Is Nothing) Then
                    olbWritten.Visible = True
                    olblbuynow1.Visible = False
                Else
                    olbWritten.Visible = False
                    olblbuynow1.Visible = True
                End If
            ElseIf (olblRenewalInvite.Visible = False AndAlso oQuote.InsuranceFileTypeCode = "WRITTEN" AndAlso sWrittenStatus = "1") Then
                olblbuynow1.Visible = True
            End If
        End If
        If Session(CNMode) = Mode.View Then
            olblbuynow1.Visible = False
        End If
        If oQuote.InsuranceFileTypeCode = "WRITTEN" Then
            'btnSaveQuote
            Dim obtnRequote As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnRequote"), LinkButton)
            Dim obtnSaveQuote As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnSaveQuote"), LinkButton)
            If (Not obtnRequote Is Nothing) Then
                obtnRequote.Text = "Edit Written"
            End If
            If (Not obtnSaveQuote Is Nothing) Then
                obtnSaveQuote.Text = "Save Written"
            End If
        End If

    End Sub

    Sub UpdateXML(ByVal sFieldName As String, ByVal sFieldValue As String)
        Dim oDataSet As New DataSetControl.Application

        'get the dataset definition
        Dim oQuote As NexusProvider.Quote = System.Web.HttpContext.Current.Session(CNQuote)

        Dim sDataSetDefinition As String = Nexus.DataSetFunctions.GetDataSetDefinition(Session(CNDataModelCode))
        'load dataset into SAM client
        oDataSet.LoadFromXML(sDataSetDefinition, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)

        Dim srXMLDataset As String = oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset
        Dim oDoc As New XmlDocument
        oDoc.LoadXml(srXMLDataset)

        Dim oxmlnode As XmlNode = oDoc.SelectSingleNode("//" & Session.Item(CNDataModelCode) & "_POLICY_BINDER/VEHDET")

        If oxmlnode IsNot Nothing Then
            oDataSet.SetPropertyValue("GENERAL__", sFieldName, oxmlnode.Attributes("OI").Value, sFieldValue, True)
            oDataSet.SetPropertyValue("GENERAL__", "US", oxmlnode.Attributes("OI").Value, "2", True)
        End If

        Dim swContent As New System.IO.StringWriter
        Dim xmlwContent As New XmlTextWriter(swContent)

        oDoc.WriteTo(xmlwContent)
        oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()

        oDataSet.ReturnAsXML(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)

        Session(CNQuote) = oQuote
        xmlwContent.Close()
        swContent.Close()

    End Sub

    Sub ConverDateFormat(ByVal DateString As String, ByRef returndate As Date)
        Dim formats() As String = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt", _
                           "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss", _
                           "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt", _
                           "M/d/yyyy h:mm", "M/d/yyyy h:mm", _
                           "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"}


        Dim dateValue As DateTime


        If Date.TryParseExact(DateString, formats, New CultureInfo("en-US"), DateTimeStyles.None, dateValue) Then
            returndate = dateValue

        Else
            If IsDate(CDate(DateString)) Then
                'If IsDate(Date.ParseExact(PCATLIN__LASTMODIFYDATE.Text, "MM/dd/yy HH:mm:ss tt", New CultureInfo("en-us", True))) Then
                'returndate = Date.ParseExact(PCATLIN__LASTMODIFYDATE.Text, "MM/dd/yy HH:mm:ss tt", New CultureInfo("en-us", True))

                returndate = DateString
            End If


        End If


    End Sub

    Private Function IsValidDate(ByVal idate As String) As Boolean
        Dim res As Boolean = False
        Try
            ' Dim culture As New CultureInfo("en-us", True)
            Dim realDate As Date = Date.ParseExact(idate, "MM/dd/yy HH:mm:ss tt", New CultureInfo("en-us", True))
            ' DateTime.TryParseExact(idate, "MM/dd/YY HH:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture)
            Return True
        Catch ex As System.Exception
            Return False
        End Try

    End Function


End Class
