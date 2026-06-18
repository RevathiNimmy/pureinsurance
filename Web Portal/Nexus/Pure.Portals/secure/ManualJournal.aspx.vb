Imports CMS.Library
Imports Nexus.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml
Imports Nexus
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports NexusProvider
Imports System.Security.Policy
Imports System.Web.UI.WebControls
Imports System.Collections.Generic

Partial Class secure_ManualJournal
    Inherits Frontend.clsCMSPage
    Dim oManualJournal As New NexusProvider.ManualJournal
    Dim dDocumentBalance, dTempAmount As Decimal
    Dim iManualJournalId As Integer
    Dim oWebservice As NexusProvider.ProviderBase
    Dim sMode As String = ""
    Dim sUrl As String = ""
    Dim dApproverLimitValue As Decimal = 0.0

    Private Sub Secure_ManualJournal_Init(sender As Object, e As EventArgs) Handles Me.Init
        If (Request.QueryString.HasKeys() = True) Then
            sMode = Request.QueryString("mode").Trim.ToUpper()
        End If
        sUrl = "ApproversComment.aspx?mode=" + sMode + "&ManualJournalKey=" & Request.QueryString("ManualJournalKey")

        If sMode <> "JOURNAL" Then
            btnAdd.Visible = False
            btnFinish.Visible = False
            liCommentTab.Visible = True
            hdnUrl.Value = sUrl
        Else
            liCommentTab.Visible = False
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ApprovalAlert", "<script language=""JavaScript"" type=""text/javascript"">function ApprovalAlert(){alert('" & GetLocalResourceObject("msg_AuthAlert").ToString() & "')}</script>")
        End If

        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim olist As NexusProvider.LookupListCollection
        Dim v_sWhereClause As New List(Of ListFilterOptions)

        Dim v_sWherelist As ListFilterOptions = New ListFilterOptions()

        v_sWherelist.ColumnName = "Step_Number"
        v_sWherelist.FilterOperator = "="
        v_sWherelist.FilterValue = "1"
        v_sWhereClause.Add(v_sWherelist)
        v_sWherelist = New ListFilterOptions()

        v_sWherelist.ColumnName = "source_id"
        v_sWherelist.FilterOperator = "="
        Dim oLookUP As New NexusProvider.LookupListCollection
        oLookUP = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Source", False, False, "Source_ID")
        Dim iBranchKey As Integer = 0
        For iBranchCount As Integer = 0 To oLookUP.Count - 1
            If oLookUP(iBranchCount).Code = Session(CNBranchCode) Then
                iBranchKey = oLookUP(iBranchCount).Key
                Exit For
            End If
        Next
        v_sWherelist.FilterValue = iBranchKey
        v_sWhereClause.Add(v_sWherelist)
        olist = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Debtor_User_Groups", True, False, "debtor_user_groups_type_id", 8,,,, v_sWhereClause)
        If olist.Count > 0 Then
            hDebtorGroup.Value = 1
        End If
        Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)
        hMultistep.Value = oMultiStepApproval.OptionValue

        Dim oUserAuthority As New NexusProvider.UserAuthority With {
             .UserCode = CType(Session(CNLoginName), String),
             .UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasManualJournalAuthority
         }
        oWebservice.GetUserAuthorityValue(oUserAuthority)
        hdnJournalAuthoriser.Value = oUserAuthority.UserAuthorityValue
        hdnApprovalAuthorityLimit.Value = Convert.ToDecimal(oUserAuthority.UserAuthorityOptionalValue2)

        dApproverLimitValue = hdnApprovalAuthorityLimit.Value
    End Sub

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Cleaning of the session values
            ClearQuote()
            ClearClaims()
            ClearHeader()
            'setting branch and document type values in BindControls()
            BindControls()
            'binding main transaction grid
            BindGrid()

            If Request.QueryString.HasKeys() Then
                If sMode = "V" Then
                    btnApprove.Visible = False
                    btnBack.Visible = True
                ElseIf sMode = "A" Then
                    btnApprove.Visible = True
                    btnBack.Visible = True
                ElseIf sMode = "D" Then
                    btnDecline.Visible = True
                    btnBack.Visible = True
                ElseIf sMode = "WM" Then
                    btnApprove.Visible = True
                    btnDecline.Visible = True
                    btnBack.Text = "Cancel"
                    btnBack.Visible = True
                End If
            Else
                btnApprove.Visible = False
                btnBack.Visible = False
                ' GetAuthorizationComments()

            End If

            If Session(CNBranchCode) IsNot Nothing AndAlso (Not String.IsNullOrEmpty(Convert.ToString(Session(CNBranchCode)))) AndAlso (Session(CNManualJournal) Is Nothing OrElse String.IsNullOrEmpty(CType(Session(CNManualJournal), NexusProvider.ManualJournal).JournalBranchCode) = True) Then
                drpBranch.SelectedValue = Convert.ToString(Session(CNBranchCode))
            End If

            Dim bShowSubBranchForPosting As Boolean = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).ShowSubBranchForPosting
            Dim oSessionManualJournal As NexusProvider.ManualJournal = Session(CNManualJournal)
            If bShowSubBranchForPosting Then
                liSubBranchCode.Visible = True
                If drpBranch IsNot Nothing Then
                    Dim sBranchCode As String = drpBranch.SelectedValue.ToString
                    FillSubBranches(sBranchCode)
                End If
            Else
                liSubBranchCode.Visible = False
            End If
        End If
    End Sub

    Public Sub FillSubBranches(Optional ByVal oBranchCode As String = Nothing)

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oSessionManualJournal As NexusProvider.ManualJournal = Session(CNManualJournal)
        Dim oLookUP As NexusProvider.LookupListCollection

        'sam call to retreive the list of branch from table source
        oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Source", False, False, "Source_ID")

        'Retreival of the Branch Key, which will latet identify the sub-branch
        'sam need barnch key to find the respective sub-branches of the selected branches
        Dim iBranchKey As Integer = 0
        For iBranchCount As Integer = 0 To oLookUP.Count - 1
            If oLookUP(iBranchCount).Code = oBranchCode Then
                iBranchKey = oLookUP(iBranchCount).Key
                Exit For
            End If
        Next

        'sam call to retreive the list of sub-branch from table source
        oLookUP = Nothing
        oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Sub_Branch", False, False, "Source_ID", iBranchKey, oBranchCode)

        'Populating the sub-branch control with the retreived values
        If ddlSubBranchCode IsNot Nothing Then
            'existing items cleared
            ddlSubBranchCode.Items.Clear()
            ddlSubBranchCode.Items.Add(New ListItem("(Please Select)", ""))
            For iSubBranchCount As Integer = 0 To oLookUP.Count - 1
                Dim lstSubBranch As New ListItem With {
                    .Text = oLookUP(iSubBranchCount).Description,
                    .Value = Trim(oLookUP(iSubBranchCount).Code)
                }
                ddlSubBranchCode.Items.Add(lstSubBranch)
                ddlSubBranchCode.DataBind()
            Next
            If oSessionManualJournal IsNot Nothing AndAlso String.IsNullOrEmpty(oSessionManualJournal.JournalSubBranchCode) = False Then
                ddlSubBranchCode.SelectedValue = oSessionManualJournal.JournalSubBranchCode.Trim
            End If
        End If
    End Sub

    Private Function ManualJournalParameters(oManualJournal As NexusProvider.ManualJournal, oManualJournalItemCollection As NexusProvider.ManualJournalItemCollection) As NexusProvider.ManualJournal
        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        oManualJournal.JournalTypeCode = PMLookup_DocumentType.Value
        oManualJournal.JournalComment = txtComments.Text
        If Not String.IsNullOrEmpty(drpBranch.SelectedValue) Then
            oManualJournal.JournalBranchCode = drpBranch.SelectedValue
        End If

        If txtDate.Text.Length = 0 Then
            oManualJournal.JournalDate = CType(Now.Date.ToShortDateString(), Date)
            oManualJournal.JournalDateSpecified = False
        Else
            oManualJournal.JournalDate = Convert.ToDateTime(txtDate.Text)
            oManualJournal.JournalDateSpecified = True
        End If

        If Not String.IsNullOrEmpty(ddlSubBranchCode.SelectedValue) Then
            oManualJournal.JournalSubBranchCode = ddlSubBranchCode.SelectedValue
        End If
        oManualJournal.JournalReverseDate = Convert.ToDateTime(txtReversesDate.Text)
        If PMLookup_DocumentType.Value = "ACC" Or PMLookup_DocumentType.Value = "RVJ" Or PMLookup_DocumentType.Value = "PPT" Then
            If txtReversesDate.Text.Length = 0 Then
                txtReversesDate.Text = Now.ToShortDateString
            End If
            oManualJournal.JournalReverseDate = Convert.ToDateTime(txtReversesDate.Text)

        ElseIf PMLookup_DocumentType.Value = "RCJ" Or PMLookup_DocumentType.Value = "DPJ" Then
            oManualJournal.RecurringOccurs = Convert.ToInt32(txtOccurs.Text)
            oManualJournal.RecurringPerPeriodOnDay = Convert.ToInt32(txtPeriod.Text)
            oManualJournal.RecurringPerPeriodOnDayBool = rbPeriod.Checked
            oManualJournal.RecurringPerMonthOnDay = Convert.ToInt32(txtMonth.Text)
            oManualJournal.RecurringPerMonthOnDayBool = rbMonth.Checked
            oManualJournal.RecurringPerQuarterOnDay = Convert.ToInt32(txtQuarter.Text)
            oManualJournal.RecurringPerQuarterOnDayBool = rbQuarter.Checked

        End If
        Dim damount As Decimal = 0.0
        'Dim oManualJournalItemCollection As NexusProvider.ManualJournalItemCollection
        If (sMode <> "JOURNAL") Then
            If Session(CNManualJournalAuthItemCollection) IsNot Nothing Then
                oManualJournalItemCollection = CType(Session(CNManualJournalAuthItemCollection), NexusProvider.ManualJournalItemCollection)
                oManualJournal.ManualJournalItemCollection = oManualJournalItemCollection
            End If
        Else
            If Session(CNManualJournalItemCollection) IsNot Nothing Then
                oManualJournalItemCollection = CType(Session(CNManualJournalItemCollection), NexusProvider.ManualJournalItemCollection)
                For icount As Integer = 0 To oManualJournalItemCollection.Count - 1
                    If (oManualJournalItemCollection.Item(icount).Amount > 0) Then
                        damount = damount + oManualJournalItemCollection.Item(icount).Amount
                    End If

                Next
                oManualJournal.ManualJournalItemCollection = oManualJournalItemCollection
            End If

        End If


        Dim sTaskdesc As String = ""
        Dim sTaskName As String = "MEMO"
        Dim sTaskGroup As String = "Common"
        Dim sPMUser_Group As String

        Dim oManualJournalCollection As New ManualJournalCollection
        oManualJournalCollection = oWebservice.ValidateAuthorizationSteps(oManualJournal)
        sPMUser_Group = oManualJournalCollection.Item(0).PMUserGroup

        Dim bstep As Boolean = oManualJournalCollection.Item(0).IsLastStep

        GetParty(oManualJournal.ManualJournalItemCollection(0).AccountKey)

        Try
            'call webservice method AddJournal to insert the journal entry
            If (sMode <> "JOURNAL") Then
                If oManualJournal.Approved = 0 Then
                    oManualJournal.Is_Reffered = 2
                    sTaskdesc = "Declined Manual Journal, Type: " + PMLookup_DocumentType.Value + ", Amount:" + oManualJournal.ManualJournalItemCollection(0).CurrencyTypeCode.ToString().Trim() + " " + (oManualJournalCollection.Item(0).JournalAmount).ToString()
                ElseIf oManualJournal.Approved = 1 AndAlso (Not bstep) Then
                    sTaskdesc = " Your Manual Journal of amount " + oManualJournal.ManualJournalItemCollection(0).CurrencyTypeCode.ToString().Trim() + " " + (oManualJournalCollection.Item(0).JournalAmount).ToString() + " has been approved and further assigned on " + DateTime.Now.ToString()
                    oManualJournal.Is_Reffered = 1
                    sTaskName = "SAMADJNL"
                    sTaskGroup = "SYSADMIN"
                ElseIf oManualJournal.Approved = 1 AndAlso bstep Then
                    sTaskdesc = " Your Manual Journal Of amount " + oManualJournal.ManualJournalItemCollection(0).CurrencyTypeCode.ToString().Trim() + " " + (oManualJournalCollection.Item(0).JournalAmount).ToString() + " has been approved On " + DateTime.Now.ToString()
                    oManualJournal.Is_Reffered = 0
                End If
            Else
                If hMultistep.Value = "1" AndAlso hDebtorGroup.Value = "1" Then
                    oManualJournal.Is_Reffered = 1
                End If
                If (oManualJournal.Is_Reffered = 1) Then
                    sTaskdesc = "Manual Journal, Type: " + PMLookup_DocumentType.Value + ", Amount:" + oManualJournal.ManualJournalItemCollection(0).CurrencyTypeCode.ToString().Trim() + " " + damount.ToString()
                    sTaskName = "SAMADJNL"
                    sTaskGroup = "SYSADMIN"
                    oManualJournal.Approved = 0
                Else
                    sTaskdesc = "Manual Journal"
                    sPMUser_Group = "SYSADMIN"
                End If
            End If
            If (sPMUser_Group <> "") Then
                oWebservice.AddJournal(oManualJournal, drpBranch.SelectedValue.Trim())
                'Sent for Authorization
                CreateWorkManagerTask(Convert.ToInt32(oManualJournal.ManualJournalId), Convert.ToString(Me.hPartyName.Value), sTaskdesc, sTaskName, sTaskGroup, oManualJournal.Is_Reffered, sPMUser_Group)
            End If
        Catch ex As Exception
            Throw

        End Try

        Return oManualJournal
    End Function

    Protected Sub BindControls()

        'populate branches in dropdown and setting default value
        If Session(CNAgentDetails) IsNot Nothing Then

            drpBranch.DataSource = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
            drpBranch.DataTextField = "Description"
            drpBranch.DataValueField = "Code"
            drpBranch.DataBind()
        End If

        If (sMode <> "JOURNAL") Then
            oWebservice = New NexusProvider.ProviderManager().Provider

            Dim oCashListItems As NexusProvider.ManualJournalTransactionApprovalMasterCollection = oWebservice.GetListOfManualJournalTransactionApprovalMaster(Request.QueryString("ManualJournalKey"), Session(CNBranchCode))
            If oCashListItems.Count > 0 Then
                If oCashListItems.Item(0).Branch IsNot Nothing Then
                    drpBranch.SelectedValue = oCashListItems.Item(0).Branch
                Else
                    If drpBranch.Items.Count = 1 Then

                    Else
                        drpBranch.Items.Insert(0, New ListItem("", ""))
                        drpBranch.SelectedValue = ""
                    End If
                End If
                If oCashListItems.Item(0).DocumentType.Trim IsNot Nothing Then
                    PMLookup_DocumentType.Value = oCashListItems.Item(0).DocumentType.Trim
                Else
                    PMLookup_DocumentType.Value = "JN"
                End If

                If oCashListItems.Item(0) IsNot Nothing Then
                    txtOccurs.Text = oCashListItems.Item(0).RecurringOccurs
                    txtPeriod.Text = oCashListItems.Item(0).PerPeriodOnDay
                    rbPeriod.Checked = If(oCashListItems.Item(0).PerPeriodOnDay = 1, True, False)
                    txtMonth.Text = oCashListItems.Item(0).PerMonthOnDay
                    rbMonth.Checked = If(oCashListItems.Item(0).PerMonthOnDay = 1, True, False)
                    txtQuarter.Text = oCashListItems.Item(0).PerQuarterOnDay
                    rbQuarter.Checked = If(oCashListItems.Item(0).PerQuarterOnDay = 1, True, False)
                    txtComments.Text = oCashListItems.Item(0).Comment

                    txtOccurs.Enabled = False
                    txtPeriod.Enabled = False
                    txtMonth.Enabled = False
                    txtQuarter.Enabled = False
                    txtComments.Enabled = False
                    drpBranch.Enabled = False
                    PMLookup_DocumentType.Enabled = False
                    SubBranchCode.Enabled = False

                End If

                'setting date's default value as current date
                txtApproversComments.Text = oCashListItems.Item(0).AuthorisationComment
                txtDate.Text = CDate(oCashListItems.Item(0).CreatedDate).ToShortDateString
                txtReversesDate.Text = CDate(oCashListItems.Item(0).ReversesOn).ToShortDateString
                If sMode <> "JOURNAL" Then
                    txtApproversComments.Enabled = False
                    txtDate.Enabled = False
                    txtReversesDate.Enabled = False
                    txtComments.Enabled = False
                    drpBranch.Enabled = False
                    PMLookup_DocumentType.Enabled = False
                End If
            End If


        Else
            Dim oSessionManualJournal As NexusProvider.ManualJournal = Session(CNManualJournal)
            If oSessionManualJournal IsNot Nothing AndAlso String.IsNullOrEmpty(oSessionManualJournal.JournalBranchCode) = False Then
                drpBranch.SelectedValue = oSessionManualJournal.JournalBranchCode.Trim
            Else
                If drpBranch.Items.Count = 1 Then

                Else
                    drpBranch.Items.Insert(0, New ListItem("", ""))
                    drpBranch.SelectedValue = ""
                End If
            End If

            'setting document type default value in document type lookup
            If oSessionManualJournal IsNot Nothing AndAlso String.IsNullOrEmpty(oSessionManualJournal.JournalTypeCode) = False Then
                PMLookup_DocumentType.Value = oSessionManualJournal.JournalTypeCode.Trim
            Else
                PMLookup_DocumentType.Value = "JN"
            End If

            If oSessionManualJournal IsNot Nothing Then
                txtOccurs.Text = oSessionManualJournal.RecurringOccurs
                txtPeriod.Text = oSessionManualJournal.RecurringPerPeriodOnDay
                rbPeriod.Checked = oSessionManualJournal.RecurringPerPeriodOnDayBool
                txtMonth.Text = oSessionManualJournal.RecurringPerMonthOnDay
                rbMonth.Checked = oSessionManualJournal.RecurringPerMonthOnDayBool
                txtQuarter.Text = oSessionManualJournal.RecurringPerQuarterOnDay
                rbQuarter.Checked = oSessionManualJournal.RecurringPerQuarterOnDayBool
                txtComments.Text = oSessionManualJournal.JournalComment
            End If

            'setting date's default value as current date
            If oSessionManualJournal IsNot Nothing AndAlso oSessionManualJournal.JournalDateSpecified = True Then
                txtDate.Text = CDate(oSessionManualJournal.JournalDate).ToShortDateString
            Else
                txtDate.Text = Now.ToShortDateString
            End If
            If oSessionManualJournal IsNot Nothing AndAlso oSessionManualJournal.JournalReverseDateSpecified = True Then
                txtReversesDate.Text = CDate(oSessionManualJournal.JournalReverseDate).ToShortDateString
            Else
                txtReversesDate.Text = Now.ToShortDateString
            End If
        End If



        'setting date control's min/max vaues
        rangevldReverseDate.MinimumValue = Now.Date.ToShortDateString
        rangevldReverseDate.MaximumValue = Date.MaxValue.ToShortDateString
        rangevldDate.MinimumValue = Date.MinValue.ToShortDateString
        rangevldDate.MaximumValue = Date.MaxValue.ToShortDateString



        rbPeriod.Checked = True
    End Sub

    Protected Sub BindGrid()
        'binding grid from manual journal items from the item collection 
        'stored in Session(CNManualJournalItemCollection) which is provided by the manualjournalitem.aspx page
        Dim oManualJournalItemCollection As NexusProvider.ManualJournalItemCollection = Nothing

        If sMode <> "JOURNAL" Then
            oWebservice = New NexusProvider.ProviderManager().Provider
            drgManualJournal.Columns(13).Visible = False
            oManualJournalItemCollection = oWebservice.GetListOfManualJournalTransactionApprovalDetails(Request.QueryString("ManualJournalKey"), Session(CNBranchCode))
            Session(CNManualJournalAuthItemCollection) = oManualJournalItemCollection
            drgManualJournal.DataSource = oManualJournalItemCollection
        Else
            If Session(CNManualJournalItemCollection) IsNot Nothing Then


                oManualJournalItemCollection = CType(Session(CNManualJournalItemCollection), NexusProvider.ManualJournalItemCollection)
                drgManualJournal.DataSource = oManualJournalItemCollection
            End If
        End If


        drgManualJournal.DataBind()
        hiddentGridRowCount.Value = drgManualJournal.Rows.Count.ToString

    End Sub
    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'focus redirects to the manual journal item page for adding new entry into the transaction gird
        Dim oSessionManualJournal As New NexusProvider.ManualJournal
        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        If String.IsNullOrEmpty(drpBranch.SelectedValue) Then
            vldrqdBranch.IsValid = False
            vldrqdBranch.ErrorMessage = GetLocalResourceObject("vldrqdBranch")
        ElseIf liSubBranchCode.Visible = True AndAlso String.IsNullOrEmpty(ddlSubBranchCode.SelectedValue) Then
            vldSubBranchCodeRequired.IsValid = False
            vldSubBranchCodeRequired.ErrorMessage = GetLocalResourceObject("vldSubBranchCode")
        Else
            oSessionManualJournal.JournalBranchCode = drpBranch.SelectedValue
            oSessionManualJournal.JournalTypeCode = PMLookup_DocumentType.Value
            oSessionManualJournal.JournalDate = txtDate.Text
            oSessionManualJournal.JournalDateSpecified = True
            oSessionManualJournal.JournalComment = txtComments.Text
            oSessionManualJournal.JournalSubBranchCode = ddlSubBranchCode.SelectedValue


            Dim sCurrencyCollections As NexusProvider.CurrencyCollection = oWebservice.GetCurrenciesByBranch(drpBranch.SelectedValue)
            If Not sCurrencyCollections Is Nothing Then
                'add base currency in session
                Session("CurrencyCollections") = sCurrencyCollections(0).BaseCurrencyCode
            End If

            If PMLookup_DocumentType.Value = "ACC" Or PMLookup_DocumentType.Value = "RVJ" Or PMLookup_DocumentType.Value = "PPT" Then
                oSessionManualJournal.JournalReverseDate = txtReversesDate.Text
                oSessionManualJournal.JournalReverseDateSpecified = True
            ElseIf PMLookup_DocumentType.Value = "RCJ" Or PMLookup_DocumentType.Value = "DPJ" Then
                oSessionManualJournal.RecurringOccurs = Convert.ToInt32(txtOccurs.Text)
                oSessionManualJournal.RecurringPerPeriodOnDay = If(IsNumeric(txtPeriod.Text), Convert.ToInt32(txtPeriod.Text), 0)
                oSessionManualJournal.RecurringPerPeriodOnDayBool = rbPeriod.Checked
                oSessionManualJournal.RecurringPerMonthOnDay = If(IsNumeric(txtMonth.Text), Convert.ToInt32(txtMonth.Text), 0)
                oSessionManualJournal.RecurringPerMonthOnDayBool = rbMonth.Checked
                oSessionManualJournal.RecurringPerQuarterOnDay = If(IsNumeric(txtQuarter.Text), Convert.ToInt32(txtQuarter.Text), 0)
                oSessionManualJournal.RecurringPerQuarterOnDayBool = rbQuarter.Checked
            End If

            If PMLookup_DocumentType.Value = "ACC" Or PMLookup_DocumentType.Value = "RVJ" Or PMLookup_DocumentType.Value = "PPT" Then
                oSessionManualJournal.JournalReverseDate = txtReversesDate.Text
                oSessionManualJournal.JournalReverseDateSpecified = True
            ElseIf PMLookup_DocumentType.Value = "RCJ" Or PMLookup_DocumentType.Value = "DPJ" Then
                oSessionManualJournal.RecurringOccurs = Convert.ToInt32(txtOccurs.Text)
                oSessionManualJournal.RecurringPerPeriodOnDay = If(IsNumeric(txtPeriod.Text), Convert.ToInt32(txtPeriod.Text), 0)
                oSessionManualJournal.RecurringPerPeriodOnDayBool = rbPeriod.Checked
                oSessionManualJournal.RecurringPerMonthOnDay = If(IsNumeric(txtMonth.Text), Convert.ToInt32(txtMonth.Text), 0)
                oSessionManualJournal.RecurringPerMonthOnDayBool = rbMonth.Checked
                oSessionManualJournal.RecurringPerQuarterOnDay = If(IsNumeric(txtQuarter.Text), Convert.ToInt32(txtQuarter.Text), 0)
                oSessionManualJournal.RecurringPerQuarterOnDayBool = rbQuarter.Checked
            End If
            Session(CNManualJournal) = oSessionManualJournal
            Response.Redirect("~/secure/manualjournalitem.aspx", False)




        End If
    End Sub

    Protected Sub BtnFinish_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinish.Click
        If Page.IsValid Then
            'If ValidDocumentBalance() Then

            Dim oManualJournalItemCollection As NexusProvider.ManualJournalItemCollection = Nothing
            oManualJournal.Approved = 0
            Try
                oManualJournal = ManualJournalParameters(oManualJournal, oManualJournalItemCollection)

                If oManualJournal.ManualJournalId > 0 OrElse Not oManualJournal.ManualJournalDocumentRef = "" Then
                    oManualJournalItemCollection = Nothing
                    Session(CNManualJournalItemCollection) = Nothing
                    Session(CNManualJournal) = Nothing
                    Response.Redirect("~/secure/ManualJournal.aspx?mode=journal", False)

                End If
            Catch ex As NexusProvider.NexusException
                If ex.Errors.Count > 0 Then 'Code : 336 :: Description: Unconfirmed/Unhandled Exceptions
                    Dim cstUnconfirmeds As New CustomValidator
                    cstUnconfirmeds.IsValid = False
                    cstUnconfirmeds.ErrorMessage = ex.Errors(0).Detail
                    cstUnconfirmeds.Display = ValidatorDisplay.None
                    Page.Validators.Add(cstUnconfirmeds)
                    Exit Sub
                End If
                Throw New NexusException(ex.Message)
            End Try
        End If
    End Sub

    Private Sub BtnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click

        Try
            Dim iManualJournalKey As Integer = Request.QueryString("ManualJournalKey")
            Dim oManualJournalItemCollection As NexusProvider.ManualJournalItemCollection = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oManualJournal.ManualJournalId = iManualJournalKey
            oManualJournal.Approved = 1
            Dim oManualJournalCollection As New ManualJournalCollection
            oManualJournalCollection = oWebservice.ValidateAuthorizationSteps(oManualJournal)

            If (oManualJournalCollection.Item(0).ValidationMessage <> "") Then
                Dim sErrorMessage = GetLocalResourceObject(oManualJournalCollection.Item(0).ValidationMessage.ToString)
                sErrorMessage = sErrorMessage.ToString.Replace("{amount}", oManualJournalCollection.Item(0).JournalAmount)
                sErrorMessage = sErrorMessage.ToString.Replace("{Authlimit}", dApproverLimitValue.ToString("N2"))
                'look for a validation message in the page resources, but if there is not one defined add a default message
                Dim cstUserAuthorisation As New CustomValidator With {
                        .IsValid = False,
                        .ErrorMessage = sErrorMessage,
                        .Display = ValidatorDisplay.None
                    }
                'add the validator to the page, this will have the effect of making the page invalid
                Page.Validators.Add(cstUserAuthorisation)

                Exit Sub
            Else
                oManualJournal = ManualJournalParameters(oManualJournal, oManualJournalItemCollection)
                If (oManualJournal.ManualJournalDocumentRef <> "") Then
                    Dim message As String = GetLocalResourceObject("Message_Approval").ToString().Replace("{}", oManualJournal.ManualJournalDocumentRef)
                    Session("aaprovalmsg") = message
                Else
                    Dim message As String = GetLocalResourceObject("NotLastStep").ToString()
                    Session("furtheraaprovalmsg") = message
                End If

                Response.Redirect(sUrl, False)
            End If

            BindGrid()

        Catch ex As NexusProvider.NexusException
            Throw New NexusException(ex.Message)
        Catch exx As Exception
            Throw New Exception(exx.Message)
        Finally
            'cleaning up
            oWebservice = Nothing


        End Try
    End Sub

    Private Sub BtnDecline_Click(sender As Object, e As EventArgs) Handles btnDecline.Click

        Try
            Dim iManualJournalKey As Integer = Request.QueryString("ManualJournalKey")
            Dim oManualJournalItemCollection As NexusProvider.ManualJournalItemCollection = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oManualJournal.ManualJournalId = iManualJournalKey
            oManualJournal.Approved = 0
            Dim oManualJournalCollection As New ManualJournalCollection
            oManualJournalCollection = oWebservice.ValidateAuthorizationSteps(oManualJournal)

            If (oManualJournalCollection.Item(0).ValidationMessage <> "") Then
                Dim sErrorMessage = GetLocalResourceObject(oManualJournalCollection.Item(0).ValidationMessage.ToString)
                sErrorMessage = sErrorMessage.ToString.Replace("{amount}", oManualJournalCollection.Item(0).JournalAmount)
                sErrorMessage = sErrorMessage.ToString.Replace("{Authlimit}", dApproverLimitValue.ToString("N2"))
                'look for a validation message in the page resources, but if there is not one defined add a default message
                Dim cstUserAuthorisation As New CustomValidator With {
                        .IsValid = False,
                        .ErrorMessage = sErrorMessage,
                        .Display = ValidatorDisplay.None
                }
                'add the validator to the page, this will have the effect of making the page invalid
                Page.Validators.Add(cstUserAuthorisation)

                Exit Sub
            Else
                'Dim message As String = GetLocalResourceObject("DeclineMsg").ToString()
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DeclineMsg", "confirm('" & message & "');window.location.href = 'ApproversComment.aspx?mode=" + sMode + "&ManualJournalKey=" & Request.QueryString("ManualJournalKey") + "'", True)
                oManualJournal = ManualJournalParameters(oManualJournal, oManualJournalItemCollection)
                Response.Redirect(sUrl, False)

            End If
            BindGrid()



        Catch ex As NexusProvider.NexusException
            Throw New NexusException(ex.Message)

        Catch exx As Exception
            Throw New Exception(exx.Message)
        Finally
            'cleaning up
            oWebservice = Nothing



        End Try
    End Sub
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click

        If sMode = "WM" Then
            Response.Redirect("~/secure/workmanager.aspx")
        Else
            Response.Redirect("~/secure/AuthorizeManualJournal.aspx")
        End If

    End Sub



    Protected Sub DrgManualJournal_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles drgManualJournal.RowCommand
        'code for row deleting - session's item collection is being changed and 
        'again it is being assigned to the same item collection

        If e.CommandName = "Delete" Then
            Dim iCount As Integer
            Dim oColl As NexusProvider.ManualJournalItemCollection
            oColl = CType(Session(CNManualJournalItemCollection), NexusProvider.ManualJournalItemCollection)

            For iCount = 0 To oColl.Count - 1
                If oColl(iCount).ManualJournalKey = Convert.ToInt32(e.CommandArgument.ToString.Trim) Then
                    oColl.Remove(iCount)
                    Exit For
                End If
            Next

            If oColl.Count = 0 Then
                lblDocumentBalance.Text = "0.00"
                hiddenDocumentBalance.Value = "0"
            End If


            'bind grid again to show the updated rows
            BindGrid()

            'cleaning up of resources
            oColl = Nothing
            iCount = Nothing
        End If
    End Sub

    Protected Sub DrgManualJournal_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgManualJournal.RowDataBound
        'populating edit link's postback url in row databound event
        If e.Row.RowType = DataControlRowType.DataRow Then
            'NOTE - this will need to be changed to give each row a unique id
            'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ManualJournalKey") %>")
            e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.ManualJournalItem).ManualJournalKey)

            If e.Row.Cells(13).FindControl("hypManualJournalEdit") IsNot Nothing Then

                Dim oHyperLink As LinkButton = CType(e.Row.Cells(13).FindControl("hypManualJournalEdit"), LinkButton)
                oHyperLink.PostBackUrl = "ManualJournalItem.aspx?Mode=Edit&MJItem=" & e.Row.RowIndex
            End If

            'calculating document balance on row databound
            If e.Row.FindControl("lblAmount") IsNot Nothing Then

                dTempAmount = Convert.ToDecimal(CType(e.Row.FindControl("lblAmount"), Label).Text)
                dDocumentBalance = dTempAmount + dDocumentBalance
                lblDocumentBalance.Text = dDocumentBalance.ToString()
                hiddenDocumentBalance.Value = dDocumentBalance.ToString()
            End If

            'Etana Nexus 3.1 Grid Formatting
            Dim oManualJournalItem As New NexusProvider.ManualJournalItem
            oManualJournalItem = CType(e.Row.DataItem, NexusProvider.ManualJournalItem)

            e.Row.Cells(3).Text = New Money(oManualJournalItem.Amount, oManualJournalItem.CurrencyTypeCode).Formatted 'Amount
            e.Row.Cells(4).Text = New Money(oManualJournalItem.CurrencyRate, oManualJournalItem.CurrencyTypeCode).Formatted 'Currency Rate
            e.Row.Cells(5).Text = New Money(oManualJournalItem.BaseAmount, oManualJournalItem.CurrencyTypeCode).Formatted 'Base Amount

        End If
        e.Row.Cells(0).Visible = False
    End Sub

    Protected Sub DocumentType_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles PMLookup_DocumentType.SelectedIndexChange
        If PMLookup_DocumentType.Value = "ACC" Or PMLookup_DocumentType.Value = "RVJ" Or PMLookup_DocumentType.Value = "PPT" Then
            calRevDate.Enabled = True
            txtReversesDate.Enabled = True
            calRevDate.Visible = True
            txtReversesDate.Visible = True
            lblReverseDate.Visible = True
            txtOccurs.Visible = False
            btnOccursUp.Visible = False
            btnOccursDown.Visible = False
            lblOccurs.Visible = False
            lblTimes.Visible = False
            rbPeriod.Visible = False
            txtPeriod.Visible = False
            btnPeriodUp.Visible = False
            btnPeriodDown.Visible = False
            lblPeriod.Visible = False
            rbMonth.Visible = False
            txtMonth.Visible = False
            btnMonthUp.Visible = False
            btnMonthDown.Visible = False
            lblMonth.Visible = False
            rbQuarter.Visible = False
            txtQuarter.Visible = False
            btnQuarterUp.Visible = False
            btnQuarterDown.Visible = False
            lblQuarter.Visible = False
            divRecurring.Visible = True

        ElseIf PMLookup_DocumentType.Value = "RCJ" Or PMLookup_DocumentType.Value = "DPJ" Then

            calRevDate.Visible = False
            txtReversesDate.Visible = False
            lblReverseDate.Visible = False
            txtOccurs.Visible = True
            btnOccursUp.Visible = True
            btnOccursDown.Visible = True
            lblOccurs.Visible = True
            lblTimes.Visible = True
            rbPeriod.Visible = True
            txtPeriod.Visible = True
            btnPeriodUp.Visible = True
            btnPeriodDown.Visible = True
            lblPeriod.Visible = True
            rbMonth.Visible = True
            txtMonth.Visible = True
            btnMonthUp.Visible = True
            btnMonthDown.Visible = True
            lblMonth.Visible = True
            rbQuarter.Visible = True
            txtQuarter.Visible = True
            btnQuarterUp.Visible = True
            btnQuarterDown.Visible = True
            lblQuarter.Visible = True
            divRecurring.Visible = True

            Dim iOccursValue As Integer = 0
            If (txtOccurs.Text.Trim().Length > 0) Then
                iOccursValue = Convert.ToInt32(txtOccurs.Text)
            End If
            If (iOccursValue > 0 And iOccursValue < 13) Then
            Else
                txtOccurs.Text = 1
            End If


        Else
            divRecurring.Visible = False
        End If
    End Sub

    Protected Function ValidDocumentBalance() As Boolean
        'if document balance calculated on the basis of the rows present in the grid
        'is not zero, this funtion returns false and prevents further page processing
        Dim dLocalBalance As Decimal
        Dim bValid As Boolean = False

        For i As Integer = 0 To drgManualJournal.Rows.Count - 1
            If drgManualJournal.Rows(i).FindControl("lblAmount") IsNot Nothing Then
                dTempAmount = Convert.ToDecimal(CType(drgManualJournal.Rows(i).FindControl("lblAmount"), Label).Text)
                dLocalBalance = dTempAmount + dLocalBalance
            End If
        Next

        If dLocalBalance = 0 Then
            bValid = True
        End If

        Return bValid

    End Function

    Protected Sub custvldTransaction_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        If drgManualJournal.Rows.Count = 0 Then
            args.IsValid = False
        Else
            args.IsValid = True
        End If
    End Sub

    Protected Sub custvldDocumentBalance_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        If hiddenDocumentBalance.Value = "0" Or hiddenDocumentBalance.Value = "0.00" Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If
    End Sub

    Protected Sub CustvldReverseDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        If PMLookup_DocumentType.Value = "ACC" Or PMLookup_DocumentType.Value = "RVJ" Or PMLookup_DocumentType.Value = "PPT" Then
            If txtReversesDate.Text.Length = 0 Then
                args.IsValid = False
            Else
                args.IsValid = True
            End If
        End If
    End Sub

    Protected Sub custvldOccurs_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        If PMLookup_DocumentType.Value = "RCJ" Or PMLookup_DocumentType.Value = "DPJ" Then
            If txtOccurs.Text.Length = 0 Then
                args.IsValid = False
            Else
                args.IsValid = True
            End If
        End If
    End Sub

    Protected Sub custvldCurrencyType_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim oldCurrencyType As String = String.Empty
        For iRowCount As Integer = 0 To drgManualJournal.Rows.Count - 1

            If drgManualJournal.Rows(iRowCount).FindControl("lblCurrencyType") IsNot Nothing Then
                If oldCurrencyType IsNot String.Empty Then
                    If Not oldCurrencyType = CType(drgManualJournal.Rows(iRowCount).FindControl("lblCurrencyType"), Label).Text Then
                        args.IsValid = False
                    End If
                End If
                oldCurrencyType = CType(drgManualJournal.Rows(iRowCount).FindControl("lblCurrencyType"), Label).Text
            End If

        Next
    End Sub
    'don't delete it's blank body is required
    Protected Sub drgManualJournal_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles drgManualJournal.RowDeleting

    End Sub

    Protected Sub DrpBranch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpBranch.SelectedIndexChanged
        FillSubBranches(drpBranch.SelectedValue)
    End Sub

    Sub CreateWorkManagerTask(ByVal nManualJournalId As Integer, ByVal nPartyName As String, ByVal v_sTaskDescription As String, ByVal v_sTask As String, ByVal v_sTaskGroup As String, ByVal Is_Reffered As String, Optional ByVal v_sAllocationUserGroup As String = "UA")
        'Create the work manager task by passing following details
        Dim oWorkManager As New NexusProvider.WorkManager
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        oWorkManager.Client = nPartyName
        oWorkManager.DueDate = DateTime.Now()
        oWorkManager.DateCreated = DateTime.Now()
        oWorkManager.Description = v_sTaskDescription
        'oWorkManager.AllocationUser = oParty.Name
        oWorkManager.AllocationUserGroup = v_sAllocationUserGroup
        oWorkManager.IsUrgent = True
        oWorkManager.IsUrgentForUpdate = 1
        If (Is_Reffered = 1) Then
            oWorkManager.IsComplete = False
        Else
            oWorkManager.IsComplete = True
            oWorkManager.TaskStatus = TaskStatus.Complete
        End If

        oWorkManager.IsTaskReview = True
        oWorkManager.Task = v_sTask
        oWorkManager.TaskGroup = v_sTaskGroup

        Dim oWmrk As New NexusProvider.KeyData
        oWmrk.KeyName = "ManualJournalId"
        oWmrk.KeyValue = nManualJournalId
        oWorkManager.KeyData.Add(oWmrk)

        If oWorkManager.TaskGroup IsNot Nothing Then
            oWorkManager.LockName = NexusProvider.SAMForInsurance.PureService.TaskLockName.InvalidValue
            oWebService.CreateWmTask(oWorkManager)
        End If
    End Sub
    Protected Sub GetParty(ByVal shortcode As String)
        Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
        Dim oAccountSearchResultCollection As New NexusProvider.AccountSearchResultCollection

        oAccountSearchCriteria.ShortCode = shortcode
        oAccountSearchCriteria.ShowBalanceSpecified = False
        oAccountSearchCriteria.ShowDeletedSpecified = False
        oAccountSearchCriteria.ExcludeInsurerAgents = False
        oAccountSearchCriteria.IncludeInsurerAgents = False

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        oAccountSearchResultCollection = oWebService.FindAccounts(oAccountSearchCriteria)

        If oAccountSearchResultCollection IsNot Nothing Then
            If oAccountSearchResultCollection.Count > 0 Then

                Me.hPartyKey.Value = oAccountSearchResultCollection(0).PartyKey
                Me.hPartyName.Value = oAccountSearchResultCollection(0).AccountName
            End If
        Else
            Me.hPartyKey.Value = ""
        End If
        oAccountSearchCriteria = Nothing
        oAccountSearchResultCollection = Nothing
        oWebService = Nothing
    End Sub


End Class
