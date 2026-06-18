Imports System.Data
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Collections
Imports System.Collections.Generic
Imports System.Xml
Imports System.Linq
Imports Nexus.Reinsurance
Imports Nexus.Library
Imports CMS.Library
Imports NexusProvider.SAMForInsurance
Imports Nexus
Imports Nexus.Utils
Imports System.Configuration.ConfigurationManager
Imports CMS.Library.Portal
Imports System.Xml.Linq
Imports System.Xml.Serialization
Imports System.Xml.Linq.XElement

Namespace Nexus
    Partial Class Controls_Reinsurance2007
        Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase
        Dim oQuote As NexusProvider.Quote
        Dim oFormatStringPercentage As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(GetPortalID()).FormatStrings.FormatString("Percentage").DataFormatString
        Dim oFormatStringCurrency As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
        Dim nRIVersionId As Integer = 0
        Dim dAllocatedPerAmount As Double
        Dim dAllocatedSIAmount As Double
        Dim dOriginalPerAmount As Double
        Dim dOriginalSIAmount As Double
        Dim bIsFACPremiumNonProportional As Boolean = False
        Dim bIsManualPremiumAdjustmentEnabled As Boolean = False
#Region "Page Event"

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "FACXOLError",
                                                       "<script language=""JavaScript"" type=""text/javascript"">function FACXOLError(){ alert('" & GetLocalResourceObject("msg_FACXOL_err").ToString() & "'); return false;}</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "RIBroker",
                                                       "<script language=""JavaScript"" type=""text/javascript"">function RIBroker(){ alert('" & GetLocalResourceObject("msg_RIBroker_err").ToString() & "'); return false;}</script>")

            ScriptManager.RegisterStartupScript(Me.UpdatePanelReinsurance, Me.GetType, "validatePercentage", "configPercentage('input:text[class*=""PercTextBox""]', '#.0000%%');", True)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Me.Visible Then

                btnCancel.OnClientClick = "return ConfirmRIMsg('" & GetLocalResourceObject("msg_ConfirmRI").ToString() & "');"
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim sMode As String
                If Request.QueryString("Mode") IsNot Nothing Then
                    sMode = Request.QueryString("Mode")
                End If
                'WPR 100(a)
                Dim nRiskKey As Integer
                Dim oOptionType As New NexusProvider.OptionTypeSetting
                Dim oGetRIArrangementTypeCol As New NexusProvider.ArrangementsTypeCollection
                'Get Value for RI Regeneration hidden option
                oOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 105)
                'If RI Regeneration hidden option is ON then we need to validate RI Model exists for current Accounting year
                If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) AndAlso oOptionType.OptionValue = "1" Then
                    If Session(CNCurrentRiskKey) IsNot Nothing Then
                        nRiskKey = CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key
                    End If
                    'get RI Model code
                    oGetRIArrangementTypeCol = oWebService.GetRiskReinsuranceArrangements(nRiskKey)
                    For Each oArrangementType As NexusProvider.ArrangementsType In oGetRIArrangementTypeCol
                        If oArrangementType.BandId <> 0 Then
                            If Convert.ToInt32(oArrangementType.XOLRIModelID) = 0 And Convert.ToInt32(oArrangementType.ModelId = 0) Then
                                'If XOL RI Modal Id and RI Model Id does'nt exist then we need to redirect the user to Premium Display page
                                Dim sMessage As String
                                sMessage = Convert.ToString(GetLocalResourceObject("RIModelNotAvailable_Err"))
                                If Session(CNMode) = Mode.PortFolioTransferAmendment OrElse Session(CNMode) = Mode.ClonedTransferAmendment Then
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateXOLRiModelAndRIModelExistance", "ValidateXOLRiModelAndRIModelExistance('" & sMessage & "');", True)
                                Else
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ValidateXOLRiModelAndRIModelExistance", "ValidateXOLRiModelAndRIModelExistance('" & sMessage & "',false);", True)
                                End If
                                Exit Sub
                            End If
                        End If
                    Next
                End If

                'if mode is view or review AddFAcprop and XOl button should not visible
                If Session(CNMode) = Mode.View OrElse CType(Session(CNMode), Mode) = Mode.Review OrElse Session(CNRiskMode) = RiskMode.View Then
                    btnAddFacProp.Visible = False
                    btnAddFacXOL.Visible = False
                    btnAddPropTreaty.Visible = False
                    btnAddXOLTreaty.Visible = False
                    btnCancel.Visible = False
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    btnAddFacProp.Visible = False
                    btnAddFacXOL.Visible = False
                    btnAddPropTreaty.Visible = False
                    btnAddXOLTreaty.Visible = False
                    pnlOverrideReason.Visible = False
                Else
                    SetTreatyButtonVisibility()
                End If

                ' Obtaining value of Quote from session
                oQuote = Session(CNQuote)
                If oQuote.Risks(Session(CNCurrentRiskKey)).ReturnPremiumMoreThanBilled = True Then
                    btnOk.OnClientClick = "alert('" & GetLocalResourceObject("msgReturnPremiumMoreThanBilled").ToString() & "'); return false;"
                End If

                ' Check FAC Premium Non-Proportional flag on every page load
                CheckFACPremiumNonProportional()
                bIsManualPremiumAdjustmentEnabled = CheckReinsuranceManualPremiumAdjustment()


                If Not IsPostBack Then
                    LoadOverrideReasonDropdown()
                    If Session("SelectedTreatyCode") IsNot Nothing Then
                        Try
                            Dim treatyId As Integer = 0
                            Dim treatyCode As String = Convert.ToString(Session("SelectedTreatyCode"))
                            Dim treatyName As String = Convert.ToString(Session("SelectedTreatyName"))
                            Dim treatyType As String = Convert.ToString(Session("SelectedTreatyType"))
                            Dim reinsuranceCode As String = Convert.ToString(Session("SelectedReinsuranceCode"))

                            If Session("SelectedTreatyId") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Session("SelectedTreatyId").ToString()) Then
                                Integer.TryParse(Session("SelectedTreatyId").ToString(), treatyId)
                            End If

                            Session.Remove("SelectedTreatyCode")
                            Session.Remove("SelectedTreatyId")
                            Session.Remove("SelectedTreatyName")
                            Session.Remove("SelectedTreatyType")
                            Session.Remove("SelectedTreatyIds")
                            Session.Remove("SelectedReinsuranceCode")

                            Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
                            Dim sRIBANDID As String = Convert.ToString(Session(CNRIBandKey))
                            Dim sRIArrangementKey As String = Convert.ToString(Session(CNRIArrangementkey))

                            If String.IsNullOrEmpty(sXML) OrElse String.IsNullOrEmpty(sRIBANDID) OrElse String.IsNullOrEmpty(sRIArrangementKey) Then
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SessionError", "alert('Session data is missing. Please refresh the page.');", True)
                                Exit Try
                            End If

                            If ValidateDuplicateTreaty(sXML, sRIBANDID, treatyCode) Then
                                Dim sDupMsg As String = String.Format(GetLocalResourceObject("msg_DuplicateTreaty").ToString(), treatyName).Replace("'", "\'")
                                Dim sTreatyType As String = If(String.IsNullOrEmpty(treatyType), "T", treatyType)
                                Dim sTreatyUrl As String = AppSettings("WebRoot") & "/Modal/SelectTreaty.aspx?Type=" & sTreatyType & "&ClientID=" & updSubmitArea.ClientID & "&RIArrangementKey=" & sRIArrangementKey & "&RI2007=ON&modal=true&KeepThis=true&TB_iframe=true&height=100&width=950"
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DuplicateTreaty", "setTimeout(function(){ alert('" & sDupMsg & "'); tb_show(null, '" & sTreatyUrl & "', null); }, 300);", True)
                                Exit Try
                            End If

                            Dim nextPriority As Integer = GetManualTreatyPriority(sXML, sRIBANDID)
                            Dim riPlacement As String
                            If treatyType = "T" Then
                                riPlacement = If(reinsuranceCode = "001" OrElse reinsuranceCode = "002" OrElse reinsuranceCode = "003", "Treaty TFS", "Treaty QSH")
                            Else
                                riPlacement = "Treaty XOL"
                            End If

                            Dim iRIArrangementKey As Integer = 0
                            Integer.TryParse(sRIArrangementKey, iRIArrangementKey)

                            Dim oArrangementLine As New ArrangementLinesType With {
                                .TreatyCode = treatyCode,
                                .RIName = treatyName,
                                .TreatyID = treatyId,
                                .Priority = nextPriority,
                                .RIPlacement = riPlacement,
                                .Type = treatyType,
                                .ManuallyAdded = True,
                                .RIarrangementKey = iRIArrangementKey,
                                .CommissionPerc = 0,
                                .SumInsured = 0,
                                .PremiumValue = 0,
                                .ThisPerc = 0,
                                .DefaultPerc = 0,
                                .LineLimit = 0,
                                .LowerLimit = 0
                            }
                            Dim iNewTreatyId As Integer = 0
                            GenerateUniqueRILineId(sXML, sRIBANDID, iNewTreatyId)
                            AddRIArrangementLines(sXML, oArrangementLine, sRIBANDID, iNewTreatyId.ToString(), sRIArrangementKey, treatyType, Session(CNRITransactionType), bIsManuallyAdded:=True)
                            ' Bug 39248 / Gap 1: mark newly added treaty as edited so Override Reason panel is visible
                            MarkRowAsEdited(sXML, sRIBANDID, iNewTreatyId.ToString(), treatyCode)
                            ' Manually added treaties do not trigger the Override Reason panel
                            Session(CNRIXMLData) = sXML
                        Catch ex As Exception
                            Dim errorMsg As String = "Error adding treaty: " & ex.Message
                            If ex.InnerException IsNot Nothing Then
                                errorMsg &= " - " & ex.InnerException.Message
                            End If
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "addTreatyError", "alert('" & errorMsg.Replace("'", "\'") & "');", True)
                        End Try
                    End If

                    If Session(CNMode) = Mode.View Then
                        ddlRIVersion.Visible = True
                        lblVersionEffectiveDate.Visible = True
                        lblEffectiveDate.Visible = True
                        Dim oRiVersions As New NexusProvider.LookupListCollection
                        oRiVersions = oWebService.GetRIVersion(oQuote.Risks(Session(CNCurrentRiskKey)).Key, oQuote.BranchCode)
                        ViewState.Add("RIVersions", oRiVersions)
                        If oRiVersions IsNot Nothing AndAlso oRiVersions.Count > 0 Then
                            ddlRIVersion.DataSource = oRiVersions
                            ddlRIVersion.DataValueField = "Key"
                            ddlRIVersion.DataTextField = "Description"
                            ddlRIVersion.DataBind()
                            lblVersionEffectiveDate.Text = oRiVersions(0).EffectiveDate
                        End If
                    End If
                    RIModelSummaryCntrl.Visible = True
                    'ReInitialize the Model Summary
                    Dim oTreeView As TreeView
                    oTreeView = RIModelSummaryCntrl.FindControl("treeRIModel")
                    If oTreeView IsNot Nothing Then
                        oTreeView.Nodes.Clear()
                    End If

                    'Make the "submit" button invisile at parent page
                    If Me.Parent.FindControl("divButton") IsNot Nothing Then
                        CType(Me.Parent.FindControl("divButton"), HtmlGenericControl).Visible = False
                    End If

                    Dim sRICode As String = String.Empty
                    'To set the Focus
                    Page.SetFocus(ddlReinsurance)
                    If Request.QueryString("RICode") IsNot Nothing Then
                        sRICode = Request.QueryString("RICode")
                    End If

                    If String.IsNullOrEmpty(sRICode) = False AndAlso CheckCollectionOfPartcipantsInXML(sRICode, Session(CNRIXMLData), bAlertOnly:=True) = True Then
                        ScriptManager.RegisterStartupScript(Page, GetType(Page), "script", "alert('" & sRICode.ToString & " " & GetLocalResourceObject("msg_RICodeAlreadyPresent").ToString() & "');", True)
                    End If

                    If oQuote.InsuranceFileTypeCode IsNot Nothing AndAlso (oQuote.InsuranceFileTypeCode.ToUpper.Trim = "MTA PERM" Or oQuote.InsuranceFileTypeCode.ToUpper.Trim = "MTA TEMP") Then
                        Session(CNRITransactionType) = "MTA"
                    ElseIf oQuote.InsuranceFileTypeCode IsNot Nothing AndAlso oQuote.InsuranceFileTypeCode.ToUpper.Trim = "MTACAN" Then
                        Session(CNRITransactionType) = "MTC"
                    ElseIf oQuote.InsuranceFileTypeCode IsNot Nothing AndAlso oQuote.InsuranceFileTypeCode.ToUpper.Trim = "MTAREINS" Then
                        Session(CNRITransactionType) = "MTR"
                    ElseIf oQuote.InsuranceFileTypeCode IsNot Nothing AndAlso oQuote.InsuranceFileTypeCode.ToUpper.Trim = "RENEWAL" Then
                        Session(CNRITransactionType) = "REN"
                    Else
                        Session(CNRITransactionType) = "NB"
                    End If
                    ' Obtaining the value of Reinsurance bands from SAM
                    Dim oReinsurarerBandCollection As NexusProvider.ReinsuranceBandsCollection

                    ' Pass the RiskKey to get the Reinsurance band values for the Current risk key else 
                    If Session(CNCurrentRiskKey) Is Nothing Then
                        oReinsurarerBandCollection = oWebService.GetRiskReinsuranceBands(CType(Session(CNQuote), NexusProvider.Quote).Risks(0).Key)
                    Else
                        oReinsurarerBandCollection = oWebService.GetRiskReinsuranceBands(CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key)
                    End If

                    ' Adding value from the collection to the Dropdown
                    For Each oReinsurarerBand As NexusProvider.ReinsuranceBands In oReinsurarerBandCollection
                        ddlReinsurance.Items.Add(New ListItem(oReinsurarerBand.Band, oReinsurarerBand.BandKey))
                    Next
                    'Select the first band
                    If ddlReinsurance.Items.Count > 0 Then
                        If Session(CNRIBandKey) IsNot Nothing Then
                            ddlReinsurance.SelectedValue = Convert.ToString(Session(CNRIBandKey)).Trim
                        Else
                            ddlReinsurance.SelectedIndex = 0
                        End If
                    End If

                    Session.Remove(CNRIArrangementkey)
                    Session.Remove(CNRIFACProp)
                    Session.Remove(CNRIFACXol)
                    ' Populate the Grid  
                    If Session(CNMode) <> Mode.View OrElse ddlRIVersion.Items.Count = 0 Then
                        PopulateGrid(0, oReinsurarerBandCollection) ' Do not pass version Id
                    Else
                        PopulateGrid(ddlRIVersion.SelectedValue) 'Populate grid for first version
                    End If

                    'Setup treaty button URLs after PopulateGrid sets CNRIArrangementkey
                    SetupTreatyButtonUrls()
                ElseIf Session(CNRefreshRI) = True Then
                    ' Populate the Grid  
                    Session.Remove(CNRIXMLData)
                    PopulateGrid()
                    Session(CNRefreshRI) = False
                    'Setup treaty button URLs after PopulateGrid
                    SetupTreatyButtonUrls()
                Else
                    'Setup treaty button URLs for postback scenarios
                    SetupTreatyButtonUrls()
                End If
            End If
        End Sub
#End Region

#Region "Grid And DropDownlist Event"
        Protected Sub gvReinsurance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReinsurance.Load
            If gvReinsurance.PageCount = 1 Then
                gvReinsurance.AllowPaging = False
            End If
        End Sub

        Protected Sub ddlReinsurance_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReinsurance.SelectedIndexChanged
            ' GAP 5: Persist current band override reason to XML before switching bands
            Dim sPrevBandKey As String = Convert.ToString(Session(CNRIBandKey)).Trim()
            If Not String.IsNullOrEmpty(sPrevBandKey) AndAlso pnlOverrideReason.Visible Then
                Dim sXMLTemp As String = Convert.ToString(Session(CNRIXMLData))
                If Not String.IsNullOrEmpty(sXMLTemp) Then
                    SetBandOverrideReasonId(sXMLTemp, sPrevBandKey, ddlOverrideReason.SelectedValue)
                    Session(CNRIXMLData) = sXMLTemp
                End If
            End If
            
            'ReInitialize the Model Summary
            Dim oTreeView As TreeView
            oTreeView = RIModelSummaryCntrl.FindControl("treeRIModel")
            If oTreeView IsNot Nothing Then
                oTreeView.Nodes.Clear()
            End If
            RIModelSummaryCntrl.PopulateModelSummary()

            ' call made for populating the grid 
            If Session(CNMode) = Mode.View Then
                PopulateGrid(Convert.ToInt16(ddlRIVersion.SelectedValue))
            Else
                PopulateGrid()
            End If
            RestoreOverrideReasonState()

        End Sub

        Protected Sub gvOrgReinsurance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOrgReinsurance.Load
            If gvOrgReinsurance.PageCount = 1 Then
                gvOrgReinsurance.AllowPaging = False
            End If
        End Sub

        Protected Sub gvReinsurance_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReinsurance.DataBound
            gvReinsurance.Columns(0).Visible = False
            gvReinsurance.Columns(1).Visible = False
            gvReinsurance.Columns(7).Visible = bIsFACPremiumNonProportional
            gvReinsurance.Columns(18).Visible = False
            gvReinsurance.Columns(19).Visible = False
            gvReinsurance.Columns(20).Visible = False
            gvReinsurance.Columns(21).Visible = False
        End Sub

        Protected Sub gvReinsurance_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReinsurance.RowCommand
            Dim oXMLSource As New XmlDataSource
            Dim oReinsurance2007 As New Reinsurance.Reinsurance
            oXMLSource.EnableCaching = False
            Dim oXMLData As String = String.Empty
            Dim oParentNode As XmlNode = Nothing
            Dim iIndex As Integer
            Dim sElement As String = "RIBAND"
            Dim oNode As XmlNode
            Dim oXMLDoc As New XmlDocument
            Dim oNodeList As XmlNodeList
            Dim oArrangementLine As ArrangementLinesType
            Dim owebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Integer.TryParse(e.CommandArgument, iIndex)
            Dim row As GridViewRow = gvReinsurance.Rows(iIndex)
            Dim sRIArrLineKey As String = "0"
            Dim btnDelete As LinkButton = gvReinsurance.Rows(iIndex).FindControl("lnkDelete")
            Dim btnView As LinkButton = gvReinsurance.Rows(iIndex).FindControl("lnkView")
            Dim btnEdit As LinkButton = gvReinsurance.Rows(iIndex).FindControl("lnkEdit")
            Select Case e.CommandName
                Case "Delete"
                    If Session(CNRIXMLData) IsNot Nothing Then
                        oXMLData = Session(CNRIXMLData)
                    End If
                    'Populate the XML into the Memory
                    Dim srDataSet As New System.IO.StringReader(oXMLData)
                    Dim xmlTR As New XmlTextReader(srDataSet)
                    oXMLDoc.Load(xmlTR)
                    xmlTR.Close()
                    oXMLDoc.LoadXml(oXMLData)
                    Dim sanitizedSelectedValue As String = ddlReinsurance.SelectedValue.Replace("'", "''")
                    Dim sanitizedBandKey As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
                    Dim sanitizedPlacement As String = Trim(row.Cells(2).Text).Replace("'", "''")
                    Dim oRootNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedSelectedValue & "']")
                    oNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Placement='" & sanitizedPlacement & "']")

                    If btnDelete.ToolTip <> "" Then
                        sRIArrLineKey = Convert.ToString(btnDelete.ToolTip)
                    End If

                    Dim sTreatyCode As String = ""
                    If sRIArrLineKey = "0" Then
                        sTreatyCode = GetTreatyCodeFromGridRow(oXMLData, sanitizedBandKey, btnDelete)
                    End If

                    If oNodeList IsNot Nothing Then
                        If oNodeList.Count > 0 Then
                            For Each oNode In oNodeList
                                Dim bNameMatch As Boolean = Server.HtmlEncode(oNode.Attributes("Name").Value.ToUpper.Trim) IsNot Nothing AndAlso Server.HtmlEncode(oNode.Attributes("Name").Value.ToUpper.Trim).ToUpper = Server.HtmlEncode(Trim(row.Cells(3).Text.ToUpper)).ToUpper
                                Dim bKeyMatch As Boolean = False
                                If sRIArrLineKey <> "0" Then
                                    bKeyMatch = oNode.Attributes("RIArrangementLineKey").Value.ToUpper.Trim = sRIArrLineKey.Trim
                                ElseIf Not String.IsNullOrEmpty(sTreatyCode) AndAlso oNode.Attributes("TreatyCode") IsNot Nothing Then
                                    bKeyMatch = oNode.Attributes("TreatyCode").Value.ToUpper.Trim = sTreatyCode.ToUpper.Trim
                                End If
                                If bNameMatch Or bKeyMatch Then
                                    Dim bIsManuallyAdded As Boolean = False
                                    If oNode.Attributes("ManuallyAdded") IsNot Nothing Then
                                        Boolean.TryParse(oNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
                                    End If
                                    ' GAP 3 fix: only tombstone rows that are genuinely edited (IsEdited=True); empty string was incorrectly treated as edited
                                    If oNode.Attributes("IsNew").Value.ToUpper.Trim = "FALSE" AndAlso oNode.Attributes("IsEditedDB").Value.ToUpper.Trim = "TRUE" Then
                                        oNode.Attributes("IsDeleted").Value = "True"
                                        If oNode.Attributes("ActionType") IsNot Nothing Then
                                            oNode.Attributes("ActionType").Value = "2"
                                        Else
                                            Dim attrAction As XmlAttribute = oXMLDoc.CreateAttribute("ActionType")
                                            attrAction.Value = "2"
                                            oNode.Attributes.Append(attrAction)
                                        End If
                                        btnDelete.Enabled = False
                                        btnView.Enabled = False
                                        btnEdit.Enabled = False
                                    ElseIf bIsManuallyAdded Then
                                        ' Manually added treaties must be tombstoned so SAM can delete them
                                        oNode.Attributes("IsDeleted").Value = "True"
                                        If oNode.Attributes("ActionType") IsNot Nothing Then
                                            oNode.Attributes("ActionType").Value = "2"
                                        Else
                                            Dim attrAction As XmlAttribute = oXMLDoc.CreateAttribute("ActionType")
                                            attrAction.Value = "2"
                                            oNode.Attributes.Append(attrAction)
                                        End If
                                        btnDelete.Enabled = False
                                        btnView.Enabled = False
                                        btnEdit.Enabled = False
                                    ElseIf sRIArrLineKey <> "0" Then
                                        oNode.Attributes("IsDeleted").Value = "True"
                                        If oNode.Attributes("ActionType") IsNot Nothing Then
                                            oNode.Attributes("ActionType").Value = "2"
                                        Else
                                            Dim attrAction As XmlAttribute = oXMLDoc.CreateAttribute("ActionType")
                                            attrAction.Value = "2"
                                            oNode.Attributes.Append(attrAction)
                                        End If
                                        btnDelete.Enabled = False
                                        btnView.Enabled = False
                                        btnEdit.Enabled = False
                                    Else
                                        ' FAC / FAC XOL lines must be tombstoned, not removed
                                        Dim sPlacementVal As String = ""
                                        If oNode.Attributes("Placement") IsNot Nothing Then sPlacementVal = oNode.Attributes("Placement").Value.Trim.ToUpper()
                                        If sPlacementVal = "FAC PROP" OrElse sPlacementVal = "FAC XOL" Then
                                            oNode.Attributes("IsDeleted").Value = "True"
                                            If oNode.Attributes("ActionType") IsNot Nothing Then
                                                oNode.Attributes("ActionType").Value = "2"
                                            Else
                                                Dim attrAction As XmlAttribute = oXMLDoc.CreateAttribute("ActionType")
                                                attrAction.Value = "2"
                                                oNode.Attributes.Append(attrAction)
                                            End If
                                            btnDelete.Enabled = False
                                            btnView.Enabled = False
                                            btnEdit.Enabled = False
                                        Else
                                            oRootNode.RemoveChild(oNode)
                                        End If
                                    End If

                                    ' Mark FAXParticipentRow children as deleted for FAC XOL
                                    If oNode.Attributes("IsDeleted") IsNot Nothing AndAlso oNode.Attributes("IsDeleted").Value.ToUpper = "TRUE" Then
                                        For Each oFAXChild As XmlNode In oNode.SelectNodes("FAXParticipentRow")
                                            If oFAXChild.Attributes("IsDeleted") IsNot Nothing Then
                                                oFAXChild.Attributes("IsDeleted").Value = "True"
                                            Else
                                                Dim attrDel As XmlAttribute = oXMLDoc.CreateAttribute("IsDeleted")
                                                attrDel.Value = "True"
                                                oFAXChild.Attributes.Append(attrDel)
                                            End If
                                            If oFAXChild.Attributes("ActionType") IsNot Nothing Then
                                                oFAXChild.Attributes("ActionType").Value = "2"
                                            Else
                                                Dim attrAct As XmlAttribute = oXMLDoc.CreateAttribute("ActionType")
                                                attrAct.Value = "2"
                                                oFAXChild.Attributes.Append(attrAct)
                                            End If
                                        Next
                                    End If

                                End If
                            Next
                        End If
                    End If
                    'Update the XML before returning
                    Dim swContent As New System.IO.StringWriter
                    Dim xmlwContent As New XmlTextWriter(swContent)

                    oXMLDoc.WriteTo(xmlwContent)
                    oXMLData = swContent.ToString()
                    xmlwContent.Close()
                    swContent.Close()
                    Session(CNRIXMLData) = oXMLData
                    nRIVersionId = ViewState("RIVersionId")
                    Dim sRecalcXML As String = Convert.ToString(Session(CNRIXMLData))
                    If Session(CNMode) = Mode.PortFolioTransferAmendment Then
                        'Do not chnage XOL and Treaty layers
                        oReinsurance2007.Recalculate(sRecalcXML, Session(CNRIBandKey), Session(CNRIArrangementkey), 0, Session(CNRITransactionType), bIsPortfolioRIAmendment:=True)
                    ElseIf Session(CNMode) = Mode.ClonedTransferAmendment Then
                        If nRIVersionId > 1 Then
                            oReinsurance2007.Recalculate(sRecalcXML, Session(CNRIBandKey), Session(CNRIArrangementkey), 0, Session(CNRITransactionType), bIsPortfolioRIAmendment:=True)
                        Else
                            oReinsurance2007.Recalculate(sRecalcXML, Session(CNRIBandKey), Session(CNRIArrangementkey), 0, Session(CNRITransactionType), bIsPortfolioRIAmendment:=False)
                        End If
                    Else
                        oReinsurance2007.Recalculate(sRecalcXML, Session(CNRIBandKey), Session(CNRIArrangementkey), 0, Session(CNRITransactionType))
                    End If
                    Session(CNRIXMLData) = sRecalcXML
                    oXMLSource.Data = Session(CNRIXMLData)
                    oXMLSource.XPath = ".//RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow[@IsDeleted='False']"
                    gvReinsurance.DataSource = oXMLSource
                    gvReinsurance.DataBind()

                    'Prevent reload from database on next postback
                    Session.Remove(CNNEXTBUTTON)

                    ApplyEditedRowHighlighting()

            End Select
        End Sub

        Protected Sub gvReinsurance_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReinsurance.RowDataBound
            If e.Row.RowType = DataControlRowType.Header Then
                gvReinsurance.Columns(0).Visible = True
                gvReinsurance.Columns(1).Visible = True
                gvReinsurance.Columns(7).Visible = bIsFACPremiumNonProportional
                gvReinsurance.Columns(18).Visible = True
                gvReinsurance.Columns(19).Visible = True
                gvReinsurance.Columns(20).Visible = True
                gvReinsurance.Columns(21).Visible = True
            ElseIf e.Row.RowType = DataControlRowType.DataRow Then
                Dim sPlacement As String = e.Row.Cells(2).Text
                Dim sName As String = e.Row.Cells(3).Text
                Dim lblThisPerc As Label = e.Row.FindControl("lblThisPerc")
                Dim txtThisPerc As TextBox = e.Row.FindControl("txtThisPerc")
                Dim txtFACPropPremiumPerc As TextBox = CType(e.Row.FindControl("txtFACPropPremiumPerc"), TextBox)
                Dim txtSumInsured As TextBox = e.Row.FindControl("txtSumInsured")
                Dim lblSumInsured As Label = e.Row.FindControl("lblSumInsured")
                Dim txtPremium As TextBox = e.Row.FindControl("txtPremium")
                Dim lblPremium As Label = e.Row.FindControl("lblPremium")
                Dim txtCommissionPerc As TextBox = e.Row.FindControl("txtCommissionPerc")
                Dim lblCommissionPerc As Label = e.Row.FindControl("lblCommissionPerc")
                Dim txtAgreement As TextBox = e.Row.FindControl("txtAgreement")
                Dim lblAgreement As Label = e.Row.FindControl("lblAgreement")
                Dim lnkDelete As LinkButton = e.Row.FindControl("lnkDelete")
                Dim lnkView As LinkButton = e.Row.FindControl("lnkView")
                Dim lnkEdit As LinkButton = e.Row.FindControl("lnkEdit")
                Dim iRIArrangementLineKey, iRIarrangementKey As Integer
                Dim bIsBroker As Boolean
                Dim sType As String = e.Row.Cells(21).Text
                Dim txtLowerLimit As TextBox = Nothing
                Dim txtUpperLimit As TextBox = Nothing
                Dim lblLowerLimit As Label = Nothing
                Dim lblUpperLimit As Label = Nothing

                Integer.TryParse(e.Row.Cells(18).Text, iRIarrangementKey)
                Integer.TryParse(e.Row.Cells(19).Text, iRIArrangementLineKey)
                Boolean.TryParse(e.Row.Cells(20).Text, bIsBroker)

                lnkDelete.CommandArgument = e.Row.RowIndex
                lnkView.CommandArgument = e.Row.RowIndex
                lnkEdit.CommandArgument = e.Row.RowIndex
                lnkDelete.ToolTip = iRIArrangementLineKey.ToString()
                lnkDelete.Attributes.Add("onclick", "if(!confirm('" & GetLocalResourceObject("msg_ConfirmDeletePlacement").ToString() & "')) return false;")

                ' Check if manually added
                Dim bIsManuallyAdded As Boolean = False
                Try
                    If Session(CNRIXMLData) IsNot Nothing AndAlso Not String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLData))) Then
                        Dim oXMLDoc As New XmlDocument
                        oXMLDoc.LoadXml(Session(CNRIXMLData))
                        Dim oNode As XmlNode = Nothing
                        If iRIArrangementLineKey > 0 AndAlso iRIarrangementKey > 0 Then
                            oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow[@RIArrangementLineKey='" & iRIArrangementLineKey & "']")
                        Else
                            ' For manually added treaties without RIArrangementLineKey, find by name and placement
                            oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow[@Name='" & sName.Replace("'", "''") & "' and @Placement='" & sPlacement.Replace("'", "''") & "' and @ManuallyAdded='True']")
                        End If
                        If oNode IsNot Nothing AndAlso oNode.Attributes("ManuallyAdded") IsNot Nothing Then
                            Boolean.TryParse(oNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
                        End If
                    End If
                Catch ex As Exception
                    bIsManuallyAdded = False
                End Try

                'Setting the link - skip for manually added treaties and when keys are not valid
                If Not bIsManuallyAdded Then
                    If String.IsNullOrEmpty(sName) = False AndAlso sName.Trim.ToUpper = "MULTIPLE ACTS" Then
                        lnkView.PostBackUrl = "~\Modal\FACPlacement.aspx?RIArrangementLineKey=" & iRIArrangementLineKey & "&Type=FACXOL&Mode=VIEW"
                        lnkEdit.PostBackUrl = "~\Modal\FACPlacement.aspx?RIArrangementLineKey=" & iRIArrangementLineKey & "&Type=FACXOL&Mode=EDIT"
                    ElseIf bIsBroker = False AndAlso (String.IsNullOrEmpty(sName) = False And sName.Trim.ToUpper <> "MULTIPLE ACTS") _
                    AndAlso (String.IsNullOrEmpty(sType) = False And sType.Trim.ToUpper = "F") Then

                        lnkView.OnClientClick = "javascript:RIBroker()"
                        lnkEdit.OnClientClick = "javascript:RIBroker()"
                    Else
                        If String.IsNullOrEmpty(sPlacement) = False AndAlso (sPlacement.Trim.ToUpper = "FAC PROP") Then
                            lnkView.PostBackUrl = "~\Modal\RIBrokerParticipant.aspx?RIArrangementLineKey=" & iRIArrangementLineKey & "&Type=FACPROP&Mode=VIEW"
                            lnkEdit.PostBackUrl = "~\Modal\RIBrokerParticipant.aspx?RIArrangementLineKey=" & iRIArrangementLineKey & "&Type=FACPROP&Mode=EDIT"
                        ElseIf String.IsNullOrEmpty(sPlacement) = False AndAlso (sPlacement.Trim.ToUpper = "FAC XOL") Then
                            lnkView.PostBackUrl = "~\Modal\FACPlacement.aspx?RIArrangementLineKey=" & iRIArrangementLineKey & "&Type=FACXOL&Mode=VIEW"
                            lnkEdit.PostBackUrl = "~\Modal\FACPlacement.aspx?RIArrangementLineKey=" & iRIArrangementLineKey & "&Type=FACXOL&Mode=EDIT"

                        End If
                    End If
                End If

                'Formatting oFormatStringPercentage,oFormatStringCurrency
                Dim dThisPerc As Decimal
                If InStr(1, lblThisPerc.Text, "%") <> 0 Then
                    lblThisPerc.Text = lblThisPerc.Text.Substring(0, lblThisPerc.Text.Trim.Length - 1)
                End If
                Decimal.TryParse(lblThisPerc.Text, dThisPerc)
                lblThisPerc.Text = String.Format("{0:N2}%", dThisPerc)

                If txtThisPerc IsNot Nothing Then
                    Decimal.TryParse(txtThisPerc.Text, dThisPerc)
                    txtThisPerc.Text = String.Format("{0:N2}%", dThisPerc)
                End If

                ' Format DefaultPerc to 2dp for PX/TC (Prop XOL/Treaty Commission) - stored as 4dp for calculation accuracy
                If sType.Trim.ToUpper = "PX" OrElse sType.Trim.ToUpper = "TC" Then
                    Dim dDefaultPerc As Decimal
                    Dim sDefaultPercCell As String = e.Row.Cells(5).Text
                    If InStr(1, sDefaultPercCell, "%") <> 0 Then sDefaultPercCell = sDefaultPercCell.Substring(0, sDefaultPercCell.Trim.Length - 1)
                    Decimal.TryParse(sDefaultPercCell, dDefaultPerc)
                    e.Row.Cells(5).Text = String.Format("{0:N2}%", dDefaultPerc)
                End If


                ' Format FAC Prop Premium %
                Dim dFACPropPremiumPerc As Decimal

                If txtFACPropPremiumPerc IsNot Nothing Then
                    Decimal.TryParse(txtFACPropPremiumPerc.Text, dFACPropPremiumPerc)
                    txtFACPropPremiumPerc.Text = String.Format("{0:N2}%", dFACPropPremiumPerc)
                End If

                Dim dCommPerc As Decimal
                If InStr(1, lblCommissionPerc.Text, "%") <> 0 Then
                    lblCommissionPerc.Text = lblCommissionPerc.Text.Substring(0, lblCommissionPerc.Text.Trim.Length - 1)
                End If
                Decimal.TryParse(lblCommissionPerc.Text, dCommPerc)
                lblCommissionPerc.Text = String.Format("{0:N2}%", dCommPerc)

                If txtCommissionPerc IsNot Nothing Then
                    Decimal.TryParse(Replace(txtCommissionPerc.Text, "%", ""), dCommPerc)
                    txtCommissionPerc.Text = String.Format("{0:N2}%", dCommPerc)
                End If

                txtLowerLimit = e.Row.FindControl("txtLowerLimit")
                lblLowerLimit = e.Row.FindControl("lblLowerLimit")
                txtUpperLimit = e.Row.FindControl("txtUpperLimit")
                lblUpperLimit = e.Row.FindControl("lblUpperLimit")

                Dim dLowerLimit As Decimal
                If lblLowerLimit IsNot Nothing Then
                    Decimal.TryParse(lblLowerLimit.Text, dLowerLimit)
                    lblLowerLimit.Text = String.Format(oFormatStringCurrency, dLowerLimit)
                End If
                If txtLowerLimit IsNot Nothing Then
                    Decimal.TryParse(txtLowerLimit.Text, dLowerLimit)
                    txtLowerLimit.Text = String.Format(oFormatStringCurrency, dLowerLimit)
                End If

                Dim dLineLimit As Decimal
                If lblUpperLimit IsNot Nothing Then
                    Decimal.TryParse(lblUpperLimit.Text, dLineLimit)
                    lblUpperLimit.Text = String.Format(oFormatStringCurrency, dLineLimit)
                End If
                If txtUpperLimit IsNot Nothing Then
                    Decimal.TryParse(txtUpperLimit.Text, dLineLimit)
                    txtUpperLimit.Text = String.Format(oFormatStringCurrency, dLineLimit)
                End If

                Dim dSumInsured As Decimal
                Decimal.TryParse(lblSumInsured.Text, dSumInsured)
                dSumInsured = Math.Round(Convert.ToDouble(dSumInsured), 2)
                lblSumInsured.Text = String.Format(oFormatStringCurrency, dSumInsured)
                txtSumInsured.Text = String.Format(oFormatStringCurrency, dSumInsured)

                Dim dPremium As Decimal
                If lblPremium IsNot Nothing Then
                    Decimal.TryParse(lblPremium.Text, dPremium)
                    lblPremium.Text = String.Format(oFormatStringCurrency, dPremium)
                End If
                If txtPremium IsNot Nothing Then
                    Decimal.TryParse(txtPremium.Text, dPremium)
                    txtPremium.Text = String.Format(oFormatStringCurrency, dPremium)
                End If

                Dim dTax As Decimal
                Decimal.TryParse(e.Row.Cells(12).Text, dTax)
                e.Row.Cells(12).Text = String.Format(oFormatStringCurrency, dTax)

                Dim dCommission As Decimal
                Decimal.TryParse(e.Row.Cells(14).Text, dCommission)
                e.Row.Cells(14).Text = String.Format(oFormatStringCurrency, dCommission)

                Dim dCommissionTax As Decimal
                Decimal.TryParse(e.Row.Cells(15).Text, dCommissionTax)
                e.Row.Cells(15).Text = String.Format(oFormatStringCurrency, dCommissionTax)


                If (String.IsNullOrEmpty(sPlacement) = False AndAlso (sPlacement.Trim.ToUpper = "FAC PROP")) OrElse bIsManuallyAdded Then
                    If Session(CNMode) <> Mode.View AndAlso Session(CNMTAType) <> MTAType.CANCELLATION Then
                        If Not bIsManuallyAdded AndAlso sPlacement.Trim.ToUpper = "FAC PROP" Then
                            txtThisPerc.Visible = True
                            txtThisPerc.Enabled = True
                            lblThisPerc.Visible = False
                        Else
                            txtThisPerc.Visible = False
                            lblThisPerc.Visible = If(sType = "TX" OrElse sType = "PX", False, True)
                        End If
                        txtSumInsured.Visible = (sType <> "TX" AndAlso sType <> "PX")
                        txtSumInsured.Enabled = (sType <> "TX" AndAlso sType <> "PX")
                        lblSumInsured.Visible = (sType = "TX" OrElse sType = "PX")
                        txtCommissionPerc.Visible = True
                        txtCommissionPerc.Enabled = True
                        txtAgreement.Visible = True
                        txtAgreement.Enabled = True
                        If sType = "F" AndAlso bIsFACPremiumNonProportional Then
                            txtPremium.Visible = True
                            txtPremium.Enabled = True
                            lblPremium.Visible = False
                        Else
                            txtPremium.Visible = False
                            lblPremium.Visible = True
                        End If

                        If bIsManuallyAdded Then
                            e.Row.CssClass &= " ri-edited-row"
                            lnkDelete.Visible = True
                            lnkView.Visible = False
                            lnkEdit.Visible = False
                            txtFACPropPremiumPerc.Visible = False
                            txtThisPerc.Visible = False
                            lblThisPerc.Visible = (sType <> "TX" AndAlso sType <> "PX")
                            txtPremium.Visible = True
                            txtPremium.Enabled = True
                            lblPremium.Visible = False
                            If sPlacement.Trim.ToUpper = "TREATY XOL" Then
                                If txtLowerLimit IsNot Nothing Then
                                    txtLowerLimit.Visible = True
                                    txtLowerLimit.Enabled = True
                                    txtLowerLimit.Style("width") = "100%"
                                    txtLowerLimit.Style("box-sizing") = "border-box"
                                End If
                                If txtUpperLimit IsNot Nothing Then
                                    txtUpperLimit.Visible = True
                                    txtUpperLimit.Enabled = True
                                    txtUpperLimit.Style("width") = "100%"
                                    txtUpperLimit.Style("box-sizing") = "border-box"
                                End If
                                If lblLowerLimit IsNot Nothing Then lblLowerLimit.Visible = False
                                If lblUpperLimit IsNot Nothing Then lblUpperLimit.Visible = False
                            Else
                                If txtLowerLimit IsNot Nothing Then txtLowerLimit.Visible = False
                                If txtUpperLimit IsNot Nothing Then txtUpperLimit.Visible = False
                                If lblLowerLimit IsNot Nothing Then lblLowerLimit.Visible = False
                                If lblUpperLimit IsNot Nothing Then lblUpperLimit.Visible = False
                            End If
                        Else
                            lnkDelete.Visible = True
                            lnkView.Visible = True
                            lnkEdit.Visible = True
                            If bIsFACPremiumNonProportional Then
                                txtFACPropPremiumPerc.Visible = True
                                txtFACPropPremiumPerc.Enabled = True
                            Else
                                txtFACPropPremiumPerc.Visible = False
                            End If
                            If txtLowerLimit IsNot Nothing Then
                                txtLowerLimit.Visible = False
                            End If
                            If txtUpperLimit IsNot Nothing Then
                                txtUpperLimit.Visible = False
                            End If
                        End If
                    Else
                        txtThisPerc.Visible = False
                        lblThisPerc.Visible = If(sType = "TX" OrElse sType = "PX", False, True)
                        txtSumInsured.Visible = False
                        lblSumInsured.Visible = True
                        txtPremium.Visible = False
                        lblPremium.Visible = True
                        txtCommissionPerc.Visible = False
                        lblCommissionPerc.Visible = True
                        txtAgreement.Visible = False
                        lblAgreement.Visible = True
                        txtFACPropPremiumPerc.Visible = False
                        Dim lblFACPropPremiumPerc As Label = e.Row.FindControl("lblFACPropPremiumPerc")
                        If lblFACPropPremiumPerc IsNot Nothing AndAlso bIsFACPremiumNonProportional Then
                            lblFACPropPremiumPerc.Visible = True
                            Decimal.TryParse(Replace(lblFACPropPremiumPerc.Text, "%", ""), dFACPropPremiumPerc)
                            lblFACPropPremiumPerc.Text = String.Format("{0:N2}%", dFACPropPremiumPerc)
                        End If
                        lnkDelete.Visible = False
                        lnkView.Visible = False
                        lnkEdit.Visible = False
                        If txtLowerLimit IsNot Nothing Then txtLowerLimit.Visible = False
                        If txtUpperLimit IsNot Nothing Then txtUpperLimit.Visible = False
                        If bIsManuallyAdded AndAlso sPlacement.Trim.ToUpper = "TREATY XOL" Then
                            If lblLowerLimit IsNot Nothing Then lblLowerLimit.Visible = True
                            If lblUpperLimit IsNot Nothing Then lblUpperLimit.Visible = True
                        Else
                            If lblLowerLimit IsNot Nothing Then lblLowerLimit.Visible = False
                            If lblUpperLimit IsNot Nothing Then lblUpperLimit.Visible = False
                        End If
                    End If
                Else
                    txtThisPerc.Visible = False
                    lblThisPerc.Visible = If(sType = "TX" OrElse sType = "PX", False, True)
                    txtSumInsured.Visible = False
                    lblSumInsured.Visible = True
                    txtPremium.Visible = False
                    lblPremium.Visible = True
                    lblCommissionPerc.Visible = True
                    lblAgreement.Visible = True
                    txtCommissionPerc.Visible = False
                    txtAgreement.Visible = False
                    txtFACPropPremiumPerc.Visible = False

                    Dim bIsSummaryRow As Boolean = (Trim(sPlacement).ToUpper = "GROSS NET" OrElse
                                                     Trim(sPlacement).ToUpper = "GROSS" OrElse
                                                     Trim(sPlacement).ToUpper = "NET" OrElse
                                                     Trim(sName).ToUpper = "BAND TOTAL" OrElse
                                                     Trim(sName).ToUpper = "ALLOCATED" OrElse
                                                     Trim(sName).ToUpper = "UNALLOCATED" OrElse
                                                     Trim(sName).ToUpper = "ORIGINAL RI TOTALS" OrElse
                                                     Trim(sName).ToUpper = "NET" OrElse
                                                     Trim(sName).ToUpper = "NET OF FAC")

                    Dim bIsXOL As Boolean = (sType.Trim.ToUpper = "FX" OrElse sType.Trim.ToUpper = "PX" OrElse
                                             (Not String.IsNullOrEmpty(sPlacement) AndAlso
                                              (sPlacement.Trim.ToUpper = "TREATY XOL" OrElse sPlacement.Trim.ToUpper = "FAC XOL")))

                    Dim bEditable As Boolean = (Session(CNMode) <> Mode.View AndAlso
                                                Session(CNRiskMode) <> RiskMode.View AndAlso
                                                Session(CNMTAType) <> MTAType.CANCELLATION AndAlso
                                                Not bIsSummaryRow AndAlso
                                                Not bIsXOL)

                    ' TX, TC are XOL types — SI is not editable, but Commission % and Agreement are
                    Dim bIsXOLCommEditable As Boolean = (sType.Trim.ToUpper = "TX" OrElse sType.Trim.ToUpper = "PX" OrElse sType.Trim.ToUpper = "TC")
                    Dim bCommEditable As Boolean = (bEditable OrElse bIsXOLCommEditable) AndAlso
                                                   Session(CNMode) <> Mode.View AndAlso
                                                   Session(CNRiskMode) <> RiskMode.View AndAlso
                                                   Session(CNMTAType) <> MTAType.CANCELLATION AndAlso
                                                   Not bIsSummaryRow

                    txtSumInsured.Visible = bEditable
                    lblSumInsured.Visible = Not bEditable

                    Dim bRetained As Boolean = (sPlacement.Trim.ToUpper = "RETAINED" OrElse sType.Trim.ToUpper = "R")
                    txtCommissionPerc.Visible = bCommEditable AndAlso Not bRetained
                    lblCommissionPerc.Visible = Not (bCommEditable AndAlso Not bRetained)
                    txtAgreement.Visible = bCommEditable AndAlso Not bRetained
                    lblAgreement.Visible = Not (bCommEditable AndAlso Not bRetained)

                    Dim bPremiumEditable As Boolean = bIsManualPremiumAdjustmentEnabled
                    'txtPremium.Visible = bEditable AndAlso bPremiumEditable
                    'lblPremium.Visible = Not (bEditable AndAlso bPremiumEditable)
                    ' PBI 35359: When Reinsurance Manual Premium Adjustment is enabled, premium is editable
                    ' for ALL treaty lines including XOL (TX/FX). bEditable excludes XOL for SI/Comm/Agreement
                    ' but premium must use a separate check that allows XOL when the config is on.
                    Dim bPremiumRowEditable As Boolean = bPremiumEditable AndAlso
                                                        Session(CNMode) <> Mode.View AndAlso
                                                        Session(CNRiskMode) <> RiskMode.View AndAlso
                                                        Session(CNMTAType) <> MTAType.CANCELLATION AndAlso
                                                        Not bIsSummaryRow
                    txtPremium.Visible = bPremiumRowEditable
                    lblPremium.Visible = Not bPremiumRowEditable


                    Dim bIsEdited As Boolean = False
                    Dim bIsDBEdited As Boolean = False
                    Try
                        If Session(CNRIXMLData) IsNot Nothing Then
                            Dim oXMLDocRow As New XmlDocument()
                            oXMLDocRow.LoadXml(Convert.ToString(Session(CNRIXMLData)))
                            Dim sBandKeyRow As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
                            Dim oVisibleNodes As XmlNodeList = oXMLDocRow.SelectNodes("/rows/RIBAND[@Name='Current_" & sBandKeyRow & "']/ArrangementRow[@IsDeleted='False']")
                            If oVisibleNodes IsNot Nothing AndAlso e.Row.RowIndex < oVisibleNodes.Count Then
                                Dim oRowXmlNode As XmlNode = oVisibleNodes(e.Row.RowIndex)
                                'If oRowXmlNode.Attributes("IsUserEdited") IsNot Nothing Then
                                '    Boolean.TryParse(oRowXmlNode.Attributes("IsUserEdited").Value, bIsEdited)
                                'End If
                                If Not bIsEdited AndAlso oRowXmlNode.Attributes("IsEditedDB") IsNot Nothing Then
                                    Boolean.TryParse(oRowXmlNode.Attributes("IsEditedDB").Value, bIsEdited)
                                End If
                                If oRowXmlNode.Attributes("IsEditedDB") IsNot Nothing Then
                                    Boolean.TryParse(oRowXmlNode.Attributes("IsEditedDB").Value, bIsDBEdited)
                                End If
                            End If
                        End If
                    Catch : End Try
                    ' Bug 38298: FAC Prop (F) and FAC XOL (FX) lines must never receive the edited-row highlight
                    Dim bIsFACLine As Boolean = (sType.Trim.ToUpper = "F" OrElse sType.Trim.ToUpper = "FX")
                    If (bIsEdited OrElse bIsDBEdited) AndAlso Not bIsFACLine AndAlso Not e.Row.CssClass.Contains("ri-edited-row") Then
                        e.Row.CssClass = If(String.IsNullOrEmpty(e.Row.CssClass), "ri-edited-row", e.Row.CssClass & " ri-edited-row")
                    End If
                    If bIsManuallyAdded AndAlso Not bIsFACLine AndAlso Not e.Row.CssClass.Contains("ri-edited-row") Then
                        e.Row.CssClass = If(String.IsNullOrEmpty(e.Row.CssClass), "ri-edited-row", e.Row.CssClass & " ri-edited-row")

                    End If
                    Dim lblFACPropPremiumPerc As Label = e.Row.FindControl("lblFACPropPremiumPerc")
                    If lblFACPropPremiumPerc IsNot Nothing AndAlso String.IsNullOrEmpty(sPlacement) = False AndAlso sPlacement.Trim.ToUpper = "FAC PROP" AndAlso bIsFACPremiumNonProportional Then
                        lblFACPropPremiumPerc.Visible = True
                        If lblFACPropPremiumPerc IsNot Nothing Then
                            Decimal.TryParse(Replace(lblFACPropPremiumPerc.Text, "%", ""), dFACPropPremiumPerc)
                            lblFACPropPremiumPerc.Text = String.Format("{0:N2}%", dFACPropPremiumPerc)
                        End If
                    End If
                    If sType.Trim.ToUpper = "TX" OrElse sType.Trim.ToUpper = "PX" OrElse sType.Trim.ToUpper = "FX" OrElse (String.IsNullOrEmpty(sPlacement) = False AndAlso (sPlacement.Trim.ToUpper = "TREATY XOL" OrElse sPlacement.Trim.ToUpper = "FAC XOL")) Then
                        If txtLowerLimit IsNot Nothing Then txtLowerLimit.Visible = False
                        If txtUpperLimit IsNot Nothing Then txtUpperLimit.Visible = False
                        If lblLowerLimit IsNot Nothing Then lblLowerLimit.Visible = True
                        If lblUpperLimit IsNot Nothing Then lblUpperLimit.Visible = True
                    Else
                        If txtLowerLimit IsNot Nothing Then txtLowerLimit.Visible = False
                        If txtUpperLimit IsNot Nothing Then txtUpperLimit.Visible = False
                        If lblLowerLimit IsNot Nothing Then lblLowerLimit.Visible = False
                        If lblUpperLimit IsNot Nothing Then lblUpperLimit.Visible = False
                    End If
                    If bIsManuallyAdded Then
                        lnkDelete.Visible = True
                        lnkView.Visible = False
                        lnkEdit.Visible = False
                    Else
                        lnkDelete.Visible = False
                        If bIsBroker Then
                            lnkView.Visible = True
                        Else
                            lnkView.Visible = False
                        End If
                        lnkEdit.Visible = False
                    End If
                    If sPlacement.Trim.ToUpper = "FAC XOL" Then
                        lnkDelete.Visible = True
                        lnkView.Visible = True
                        lnkEdit.Visible = True
                        If Session(CNMode) = Mode.View OrElse Session(CNMTAType) = MTAType.CANCELLATION Then
                            lnkDelete.Visible = False
                            lnkView.Visible = True
                            lnkEdit.Visible = False
                        End If
                    End If
                End If

                ' Retained row — swap to edited highlight when edited, restore when reverted
                If sType.Trim.ToUpper = "R" Then
                    Dim bRIsEdited As Boolean = False
                    Dim bRIsDBEdited As Boolean = False
                    Try
                        If Session(CNRIXMLData) IsNot Nothing Then
                            Dim oXMLDocR As New XmlDocument()
                            oXMLDocR.LoadXml(Convert.ToString(Session(CNRIXMLData)))
                            Dim sBandKeyR As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
                            Dim oVisibleR As XmlNodeList = oXMLDocR.SelectNodes("/rows/RIBAND[@Name='Current_" & sBandKeyR & "']/ArrangementRow[@IsDeleted='False']")
                            If oVisibleR IsNot Nothing AndAlso e.Row.RowIndex < oVisibleR.Count Then
                                Dim oRNode As XmlNode = oVisibleR(e.Row.RowIndex)
                                'If oRNode.Attributes("IsUserEdited") IsNot Nothing Then Boolean.TryParse(oRNode.Attributes("IsUserEdited").Value, bRIsEdited)
                                If oRNode.Attributes("IsEditedDB") IsNot Nothing Then Boolean.TryParse(oRNode.Attributes("IsEditedDB").Value, bRIsDBEdited)
                            End If
                        End If
                    Catch : End Try
                    If (bRIsEdited OrElse bRIsDBEdited) AndAlso Not e.Row.CssClass.Contains("ri-edited-row") Then
                        e.Row.CssClass = e.Row.CssClass.Replace("ri-retained-row", "").Trim()
                        e.Row.CssClass = If(String.IsNullOrEmpty(e.Row.CssClass), "ri-edited-row", e.Row.CssClass & " ri-edited-row")
                    Else
                        If Not e.Row.CssClass.Contains("ri-retained-row") Then
                            e.Row.CssClass = e.Row.CssClass.Replace("ri-edited-row", "").Trim()
                            e.Row.CssClass = If(String.IsNullOrEmpty(e.Row.CssClass), "ri-retained-row", e.Row.CssClass & " ri-retained-row")
                        End If
                    End If
                End If

                If Trim(e.Row.Cells(2).Text) = "Treaty QSH" Then
                    e.Row.Cells(9).Text = ""
                    e.Row.Cells(8).Text = ""
                    e.Row.Cells(4).Text = ""
                    If Not (String.IsNullOrEmpty(e.Row.Cells(5).Text)) AndAlso InStr(1, e.Row.Cells(5).Text, "%") <> 0 Then
                        e.Row.Cells(5).Text = e.Row.Cells(5).Text.Substring(0, e.Row.Cells(5).Text.Trim.Length - 1)
                        e.Row.Cells(5).Text = String.Format("{0:N2}%", Convert.ToDecimal(e.Row.Cells(5).Text))
                    End If
                End If

                If Trim(e.Row.Cells(2).Text) = "FAC XOL" Or Trim(e.Row.Cells(2).Text) = "FAC Prop" Then
                    e.Row.Cells(5).Text = ""
                    If e.Row.Cells(14).Text = "NaN" Then
                        e.Row.Cells(14).Text = "0"
                    End If
                    If e.Row.Cells(17).Text = "NaN" Then
                        e.Row.Cells(17).Text = "0"
                    End If
                    If Trim(e.Row.Cells(2).Text) = "FAC Prop" Then
                        e.Row.Cells(9).Text = ""
                        e.Row.Cells(8).Text = ""
                        e.Row.Cells(4).Text = ""
                    Else
                        e.Row.Cells(7).Text = ""
                    End If
                End If

                If Trim(e.Row.Cells(2).Text) = "Treaty Surplus" Or Trim(e.Row.Cells(2).Text) = "Treaty XOL" Then
                    e.Row.Cells(4).Text = ""
                    If Trim(e.Row.Cells(2).Text) = "Treaty XOL" Then
                        e.Row.Cells(7).Text = ""
                    Else
                        e.Row.Cells(8).Text = ""
                        e.Row.Cells(9).Text = ""
                    End If
                    If Not (String.IsNullOrEmpty(e.Row.Cells(5).Text)) Then
                        If InStr(1, e.Row.Cells(5).Text, "%") <> 0 Then
                            e.Row.Cells(5).Text = e.Row.Cells(5).Text.Substring(0, e.Row.Cells(5).Text.Trim.Length - 1)
                            e.Row.Cells(5).Text = Format(Convert.ToDecimal(e.Row.Cells(5).Text), "0.00") + "%"
                        End If
                    End If
                End If

                If Trim(e.Row.Cells(2).Text) = "GROSS NET" Or Trim(e.Row.Cells(3).Text) = "Allocated" Or Trim(e.Row.Cells(3).Text) = "Unallocated" Or Trim(e.Row.Cells(2).Text) = "GROSS" Or Trim(e.Row.Cells(3).Text) = "Original RI Totals" Or Trim(e.Row.Cells(3).Text) = "Net" Or Trim(e.Row.Cells(2).Text) = "Net" Then
                    e.Row.CssClass = "summary"
                    e.Row.Cells(5).Text = ""
                    e.Row.Cells(7).Text = ""
                    e.Row.Cells(8).Text = ""
                    e.Row.Cells(9).Text = ""
                    If Trim(e.Row.Cells(2).Text) <> "Net" Then
                        e.Row.Cells(4).Text = ""
                        e.Row.Cells(14).Text = ""
                        e.Row.Cells(15).Text = ""
                        e.Row.Cells(16).Text = ""
                        e.Row.Cells(17).Text = ""
                    End If
                    '----End of Formatting---------
                    If Trim(e.Row.Cells(3).Text) = "Net" OrElse Trim(e.Row.Cells(2).Text) = "Net" Then
                        Dim dBandSI As Double = 0
                        Dim dBandPremium As Double = 0
                        Dim dOrgSI As Double = 0
                        Dim dOrgPremium As Double = 0
                        If Session(CNRIXMLData) IsNot Nothing Then
                            Dim oNetXMLDoc As New XmlDocument()
                            oNetXMLDoc.LoadXml(Convert.ToString(Session(CNRIXMLData)))
                            Dim sBandKey As String = ddlReinsurance.SelectedValue.Replace("'", "''")
                            Dim oBandNode As XmlNode = oNetXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKey & "']/ArrangementRow[@Name='Band Total']")
                            If oBandNode IsNot Nothing Then
                                Double.TryParse(oBandNode.Attributes("SumInsured").Value, dBandSI)
                                Double.TryParse(oBandNode.Attributes("Premium").Value, dBandPremium)
                            End If
                            Dim oOrgBandNode As XmlNode = oNetXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Original_" & sBandKey & "']/ArrangementRow[@Name='Band Total']")
                            If oOrgBandNode IsNot Nothing Then
                                Double.TryParse(oOrgBandNode.Attributes("SumInsured").Value, dOrgSI)
                                Double.TryParse(oOrgBandNode.Attributes("Premium").Value, dOrgPremium)
                            End If
                        End If
                        Dim lblNetSI As Label = e.Row.FindControl("lblSumInsured")
                        If lblNetSI IsNot Nothing Then lblNetSI.Text = String.Format(oFormatStringCurrency, dBandSI + dOrgSI)
                        Dim lblNetPremium As Label = e.Row.FindControl("lblPremium")
                        If lblNetPremium IsNot Nothing Then lblNetPremium.Text = String.Format(oFormatStringCurrency, dBandPremium + dOrgPremium)
                    End If
                End If
                'If lnkDelete.Visible = False AndAlso lnkDelete.Visible = False AndAlso lnkEdit.Visible = False Then
                '    e.Row.Cells(16).Attributes.Add("style", "display:none;")
                'Else
                '    e.Row.Cells(16).Attributes.Add("style", "")
                'End If
            ElseIf e.Row.RowType = DataControlRowType.Footer Then
                If CheckFACXOLLimit(Session(CNRIXMLData)) = False Then
                    btnOk.Attributes.Add("onclick", "javascript:return FACXOLError();")
                Else
                    btnOk.Attributes.Remove("onclick")
                    btnOk.Attributes.Add("onclick", "javascript:return true;")
                End If
            End If
        End Sub

        Protected Sub gvReinsurance_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvReinsurance.RowDeleting

        End Sub

        Protected Sub gvReinsurance_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvReinsurance.RowEditing
            Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
            Dim sRIBANDID As String = Convert.ToString(Session(CNRIBandKey))
            Dim row As GridViewRow = gvReinsurance.Rows(e.NewEditIndex)
            Dim iRIArrangementLineKey As Integer
            Integer.TryParse(row.Cells(19).Text, iRIArrangementLineKey)
            If IsManuallyAdded(sXML, sRIBANDID, iRIArrangementLineKey.ToString()) Then
                gvReinsurance.EditIndex = e.NewEditIndex
                PopulateGrid()
            End If
        End Sub

        Protected Sub gvReinsurance_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvReinsurance.RowUpdating
            Try
                Dim row As GridViewRow = gvReinsurance.Rows(e.RowIndex)
                Dim txtSumInsured As TextBox = row.FindControl("txtSumInsured")
                Dim txtCommissionPerc As TextBox = row.FindControl("txtCommissionPerc")
                Dim txtAgreement As TextBox = row.FindControl("txtAgreement")
                Dim txtPremium As TextBox = row.FindControl("txtPremium")
                Dim iRIArrangementLineKey As Integer
                Integer.TryParse(row.Cells(19).Text, iRIArrangementLineKey)

                Dim updates As New Dictionary(Of String, Object)
                If txtSumInsured IsNot Nothing Then updates.Add("SumInsured", txtSumInsured.Text)
                If txtCommissionPerc IsNot Nothing Then
                    Dim commPerc As String = txtCommissionPerc.Text.Replace("%", "")
                    If Not ValidateCommissionPercentage(Convert.ToDecimal(commPerc)) Then
                        HandleBusinessRuleViolation(GetLocalResourceObject("msg_CommissionPercRange").ToString())
                        Return
                    End If
                    updates.Add("CommissionPerc", commPerc)
                End If
                If txtAgreement IsNot Nothing Then updates.Add("Agreement", txtAgreement.Text)

                If UpdateManualTreatyLine(Convert.ToInt32(Session(CNRIArrangementkey)), iRIArrangementLineKey, updates) Then
                    gvReinsurance.EditIndex = -1
                    PopulateGrid()
                Else
                    HandleBusinessRuleViolation(GetLocalResourceObject("msg_UpdateTreatyLineFailed").ToString())
                End If
            Catch ex As Exception
                HandleAPIError(ex)
            End Try
        End Sub

        Protected Sub gvOrgReinsurance_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOrgReinsurance.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then

                ''Formatting


                Dim dThisPerc As Decimal
                If InStr(1, e.Row.Cells(4).Text, "%") <> 0 Then
                    e.Row.Cells(4).Text = e.Row.Cells(4).Text.Substring(0, e.Row.Cells(4).Text.Trim.Length - 1)
                End If
                Decimal.TryParse(e.Row.Cells(4).Text, dThisPerc)
                e.Row.Cells(4).Text = String.Format("{0:N2}%", dThisPerc)

                Dim dLowerLimit As Decimal
                Decimal.TryParse(e.Row.Cells(5).Text, dLowerLimit)
                e.Row.Cells(5).Text = String.Format(oFormatStringCurrency, dLowerLimit)

                Dim dLineLimit As Decimal
                Decimal.TryParse(e.Row.Cells(6).Text, dLineLimit)
                e.Row.Cells(6).Text = String.Format(oFormatStringCurrency, dLineLimit)

                Dim dSumInsured As Decimal
                Decimal.TryParse(e.Row.Cells(7).Text, dSumInsured)
                e.Row.Cells(7).Text = String.Format(oFormatStringCurrency, dSumInsured)

                Dim dPremium As Decimal
                Decimal.TryParse(e.Row.Cells(8).Text, dPremium)
                e.Row.Cells(8).Text = String.Format(oFormatStringCurrency, dPremium)

                Dim dTax As Decimal
                Decimal.TryParse(e.Row.Cells(9).Text, dTax)
                e.Row.Cells(9).Text = String.Format(oFormatStringCurrency, dTax)

                Dim dCommission As Decimal
                Decimal.TryParse(e.Row.Cells(11).Text, dCommission)
                e.Row.Cells(11).Text = String.Format(oFormatStringCurrency, dCommission)

                Dim dCommissionTax As Decimal
                Decimal.TryParse(e.Row.Cells(12).Text, dCommissionTax)
                e.Row.Cells(12).Text = String.Format(oFormatStringCurrency, dCommissionTax)

                Dim dCommPerc As Decimal
                If InStr(1, e.Row.Cells(10).Text, "%") <> 0 Then
                    e.Row.Cells(10).Text = e.Row.Cells(10).Text.Substring(0, e.Row.Cells(10).Text.Trim.Length - 1)
                End If
                Decimal.TryParse(e.Row.Cells(10).Text.Replace("%", ""), dCommPerc)
                e.Row.Cells(10).Text = String.Format("{0:N2}%", dCommPerc)

                ' Retained row bold
                If Trim(e.Row.Cells(0).Text) = "Treaty RET" OrElse Trim(e.Row.Cells(1).Text) = "Retained" Then
                    If Not e.Row.CssClass.Contains("ri-retained-row") AndAlso Not e.Row.CssClass.Contains("ri-edited-row") Then
                        e.Row.CssClass &= " ri-retained-row"
                    End If
                End If

                If Trim(e.Row.Cells(0).Text) = "Treaty QSH" Then
                    e.Row.Cells(6).Text = ""
                    e.Row.Cells(5).Text = ""
                    e.Row.Cells(2).Text = ""
                    If Not (String.IsNullOrEmpty(e.Row.Cells(3).Text)) AndAlso InStr(1, e.Row.Cells(3).Text, "%") <> 0 Then
                        e.Row.Cells(3).Text = e.Row.Cells(3).Text.Substring(0, e.Row.Cells(3).Text.Trim.Length - 1)
                        e.Row.Cells(3).Text = String.Format("{0:N2}%", Convert.ToDecimal(e.Row.Cells(3).Text))
                    End If
                End If

                If Trim(e.Row.Cells(0).Text) = "FAC XOL" Or Trim(e.Row.Cells(0).Text) = "FAC Prop" Then
                    e.Row.Cells(3).Text = ""
                    If e.Row.Cells(9).Text = "NaN" Then
                        e.Row.Cells(9).Text = "0"
                    End If
                    If e.Row.Cells(12).Text = "NaN" Then
                        e.Row.Cells(12).Text = "0"
                    End If
                    If Trim(e.Row.Cells(0).Text) = "FAC Prop" Then
                        e.Row.Cells(6).Text = ""
                        e.Row.Cells(5).Text = ""
                        e.Row.Cells(2).Text = ""
                    Else
                        e.Row.Cells(4).Text = ""
                    End If
                End If

                If Trim(e.Row.Cells(0).Text) = "Treaty Surplus" Or Trim(e.Row.Cells(0).Text) = "Treaty XOL" Then
                    e.Row.Cells(2).Text = ""
                    If Trim(e.Row.Cells(0).Text) = "Treaty XOL" Then
                        e.Row.Cells(4).Text = ""
                    Else
                        e.Row.Cells(5).Text = ""
                        e.Row.Cells(6).Text = ""
                    End If
                    If Not (String.IsNullOrEmpty(e.Row.Cells(3).Text)) Then
                        If InStr(1, e.Row.Cells(3).Text, "%") <> 0 Then
                            e.Row.Cells(3).Text = e.Row.Cells(3).Text.Substring(0, e.Row.Cells(3).Text.Trim.Length - 1)
                            e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "0.0000") + "%"
                        End If
                    End If
                End If

                If Trim(e.Row.Cells(0).Text) = "GROSS NET" Or Trim(e.Row.Cells(1).Text) = "Allocated" Or Trim(e.Row.Cells(1).Text) = "Unallocated" Or Trim(e.Row.Cells(0).Text) = "GROSS" Or Trim(e.Row.Cells(0).Text) = "Net" Then
                    e.Row.CssClass = "summary"
                    e.Row.Cells(3).Text = ""
                    e.Row.Cells(4).Text = ""
                    e.Row.Cells(5).Text = ""
                    e.Row.Cells(6).Text = ""
                    If Trim(e.Row.Cells(1).Text) = "Band Total" Then
                        Dim sOrgPrem As String = e.Row.Cells(8).Text.Replace(",", "")
                        dOriginalPerAmount = If(IsNumeric(sOrgPrem), Convert.ToDouble(sOrgPrem), 0)
                        Dim sOrgSI As String = e.Row.Cells(7).Text.Replace(",", "")
                        dOriginalSIAmount = If(IsNumeric(sOrgSI), Convert.ToDouble(sOrgSI), 0)
                    End If
                    If Trim(e.Row.Cells(0).Text) <> "Net" Then
                        e.Row.Cells(2).Text = ""
                        e.Row.Cells(9).Text = ""
                        e.Row.Cells(10).Text = ""
                        e.Row.Cells(11).Text = ""
                        e.Row.Cells(12).Text = ""
                    End If
                End If
                '----End of Formatting---------
            End If
        End Sub
#End Region

#Region "Button Event"
        ''' <summary>
        ''' This method Updates the RIArrangement lines
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If Session(CNRiskViewStartPoint) = "ClientManager" Then
                Session.Remove(CNRiskViewStartPoint)
                Dim oParty As NexusProvider.BaseParty
                Dim sUrl As String = String.Empty
                oParty = Session(CNParty)
                Select Case True
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        sUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & oParty.UserName & ""
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        sUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & oParty.UserName & ""
                End Select
                Response.Redirect(sUrl, True)
            End If
            If Session(CNRiskMode) <> RiskMode.View Or Session(CNMode) <> Mode.View Then

                If Session(CNMTAType) = MTAType.CANCELLATION OrElse ValidateXML() = True Then
                    Dim sErrorMessage As String = ""
                    Dim oReinsurance As New Reinsurance.Reinsurance()
                    Dim iTotalBandKey As Integer = ddlReinsurance.Items.Count
                    Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
                    For iBandCnt As Integer = 0 To iTotalBandKey - 1
                        Dim sBandKey As String = ddlReinsurance.Items(iBandCnt).Value.Trim
                        If Session(CNMTAType) <> MTAType.CANCELLATION Then
                            If Not oReinsurance.ValidateManualXOLUpperLimits(Session(CNRIXMLData), sBandKey, sErrorMessage) Then
                                ScriptManager.RegisterStartupScript(Page, GetType(Page), "xolValidation", "alert('" & sErrorMessage.Replace("'", "\'") & "');", True)
                                Exit Sub
                            End If
                        End If
                        If BandHasEditedRows(sXML, sBandKey) AndAlso Session(CNMTAType) <> MTAType.CANCELLATION Then
                            Dim sOverrideReasonId As String = GetBandOverrideReasonId(sXML, sBandKey)
                            If sBandKey = Convert.ToString(Session(CNRIBandKey)).Trim Then
                                sOverrideReasonId = ddlOverrideReason.SelectedValue
                                If sOverrideReasonId = "0" OrElse String.IsNullOrEmpty(sOverrideReasonId) Then
                                    ScriptManager.RegisterStartupScript(Page, GetType(Page), "overrideReasonMandatory", "alert('" & GetLocalResourceObject("msg_OverrideReasonMandatory").ToString() & "');", True)
                                    Return
                                End If
                                SetBandOverrideReasonId(sXML, sBandKey, sOverrideReasonId)
                                Session(CNRIXMLData) = sXML
                            Else
                                If String.IsNullOrEmpty(sOverrideReasonId) OrElse sOverrideReasonId = "0" Then
                                    ScriptManager.RegisterStartupScript(Page, GetType(Page), "overrideReasonMandatory", "alert('" & GetLocalResourceObject("msg_OverrideReasonMandatory").ToString() & "');", True)
                                    Return
                                End If
                            End If
                        End If
                    Next

                    ' PBI 39760 Scenario 3: Warn if retained premium is zero or negative
                    If hdnRetainedPremiumConfirmed.Value <> "1" AndAlso Session(CNMTAType) <> MTAType.CANCELLATION Then
                        Dim bRetainedPremiumInvalid As Boolean = False
                        Dim oXMLDocRetained As New XmlDocument()
                        oXMLDocRetained.LoadXml(sXML)
                        For iBandCnt As Integer = 0 To iTotalBandKey - 1
                            Dim sBandKey As String = ddlReinsurance.Items(iBandCnt).Value.Trim
                            Dim oRetainedNodes As XmlNodeList = oXMLDocRetained.SelectNodes("/rows/RIBAND[@Name='Current_" & sBandKey.Replace("'", "''") & "']/ArrangementRow[@Type='R' and @IsDeleted='False']")
                            For Each oRetainedNode As XmlNode In oRetainedNodes
                                Dim dRetainedPremium As Double = 0
                                If oRetainedNode.Attributes("Premium") IsNot Nothing Then
                                    Double.TryParse(oRetainedNode.Attributes("Premium").Value, dRetainedPremium)
                                End If
                                If dRetainedPremium <= 0 Then
                                    bRetainedPremiumInvalid = True
                                    Exit For
                                End If
                            Next
                            If bRetainedPremiumInvalid Then Exit For
                        Next
                        If bRetainedPremiumInvalid Then
                            Dim sConfirmMsg As String = GetLocalResourceObject("msg_RetainedPremiumZeroOrNegative").ToString().Replace("'", "\'")
                            ScriptManager.RegisterStartupScript(Page, GetType(Page), "retainedPremiumConfirm",
                                "if(confirm('" & sConfirmMsg & "')){document.getElementById('" & hdnRetainedPremiumConfirmed.ClientID & "').value='1';" & Page.ClientScript.GetPostBackEventReference(btnOk, "") & ";}", True)
                            Return
                        End If
                    End If
                    hdnRetainedPremiumConfirmed.Value = "0"

                    UpdateRIArrangement()

                    Session.Remove(CNRIArrangementkey)
                    Session.Remove(CNRIBandKey)
                    Session.Remove(CNRIFACProp)
                    Session.Remove(CNRIFACXolTemp)
                    Session.Remove(CNRIXMLData)

                    If Session(CNMode) = Mode.PortFolioTransferAmendment Or Session(CNMode) = Mode.ClonedTransferAmendment Then
                        Response.Redirect("~/secure/RIAmendRiskList.aspx", False)
                    Else
                        Response.Redirect("~/secure/PremiumDisplay.aspx", False)
                    End If
                End If
            Else
                'Remove existing session before redirecting an user
                Session.Remove(CNRIArrangementkey)
                Session.Remove(CNRIBandKey)
                Session.Remove(CNRIFACProp)
                Session.Remove(CNRIFACXolTemp)
                Session.Remove(CNRIXMLData)
                Response.Redirect("~/secure/PremiumDisplay.aspx", False)
            End If
        End Sub
#End Region
#Region "Private Method"
        ''' <summary>
        ''' validate participation in xml
        ''' </summary>
        ''' <param name="sRICODE"></param>
        ''' <param name="sxml"></param>
        ''' <param name="iCount"></param>
        ''' <param name="bAlertOnly"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckCollectionOfPartcipantsInXML(ByVal sRICODE As String, ByVal sxml As String, Optional ByRef iCount As Integer = 0, Optional ByVal bAlertOnly As Boolean = False) As Boolean
            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(sxml)
            Dim dSumInsured As Double = 0

            Dim doc As XDocument = XDocument.Parse(oXMLDoc.OuterXml)
            Dim sanitizedBandKey As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()

            Dim oRIBands =
                From RIBands In doc.Elements("rows").Elements("RIBAND") Where RIBands.Attribute("Name").Value() = "Current_" & sanitizedBandKey
                Select RIBands

            Dim oArrangementRows =
                    From ArrangementRows In oRIBands.Elements("ArrangementRow") Where ArrangementRows.Attribute("IsDeleted").Value = "False" _
                    And ArrangementRows.Attribute("Name").Value.Trim = sRICODE.Trim
                    Select Name = ArrangementRows.Attribute("Name").Value, SumInsured = ArrangementRows.Attribute("SumInsured").Value

            If bAlertOnly = True Then
                Dim gpArrangementRows =
                    From gpArrangementRow In oArrangementRows Group gpArrangementRow By
                    gpArrangementRow.Name Into oGroupArrangementRows = ToList()
                    Select oGroupArrangementRows

                For Each oRow In gpArrangementRows
                    If oRow.Item(0).Name.Trim.ToUpper = sRICODE.ToUpper.Trim And oRow.Count() > 1 Then
                        iCount = oRow.Count
                        Return True
                        Exit For
                        Exit Function
                    End If
                Next
            Else
                Dim gpArrangementRows =
                    From gpArrangementRow In oArrangementRows Group gpArrangementRow By
                    gpArrangementRow.Name, gpArrangementRow.SumInsured Into oGroupArrangementRows = ToList()
                    Select oGroupArrangementRows

                For Each oRow In gpArrangementRows
                    If oRow.Item(0).Name.Trim.ToUpper = sRICODE.ToUpper.Trim And oRow.Count() > 1 Then
                        iCount = oRow.Count
                        Return True
                        Exit For
                        Exit Function
                    End If
                Next
            End If

        End Function
        ''' <summary>
        ''' PopulateGrid obtains the value from Webmethods 
        ''' "GetRiskReinsuranceArrangements" - Obtain value in respective of riskkey
        ''' "GetRiskReinsuranceArrangementLines" - Obtain value in respective of ArrangementId
        ''' and populates the grid "gvReinsurance".
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overloads Sub PopulateGrid()
            PopulateGrid(iVersionId:=0, oReinsuranceBandsCollection:=Nothing)
        End Sub
        Protected Overloads Sub PopulateGrid(ByVal iVersionId As Integer)
            PopulateGrid(iVersionId, oReinsuranceBandsCollection:=Nothing)
        End Sub
        ''' <summary>
        ''' PopulateGrid obtains the value from Webmethods 
        ''' "GetRiskReinsuranceArrangements" - Obtain value in respective of riskkey
        ''' "GetRiskReinsuranceArrangementLines" - Obtain value in respective of ArrangementId
        ''' and populates the grid "gvReinsurance".
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overloads Sub PopulateGrid(ByVal iVersionId As Integer, ByVal oReinsuranceBandsCollection As NexusProvider.ReinsuranceBandsCollection)

            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oXMLSource As New XmlDataSource
            Dim oReinsurance2007 As New Reinsurance.Reinsurance
            oXMLSource.EnableCaching = False
            Dim oXMLData As String = String.Empty
            Dim xdocXMLDoc As New XmlDocument
            oQuote = Session(CNQuote)
            If Session(CNRIXMLData) IsNot Nothing And Session(CNMode) <> Mode.View Then
                oXMLData = Session(CNRIXMLData)
            End If

            If String.IsNullOrEmpty(oXMLData) OrElse (Not IsNothing(Session(CNNEXTBUTTON)) AndAlso Session(CNNEXTBUTTON) = "TRUE") Then
                Dim sPreviousXML As String = Convert.ToString(Session(CNRIXMLData))
                oXMLData = oWebService.GetReinsurance2007(oQuote.Risks(Session(CNCurrentRiskKey)).Key, iVersionId, oReinsuranceBandsCollection)
                ' GAP 7: For portfolio transfer, clear override data
                If Session(CNMode) = Mode.PortFolioTransferAmendment Then
                    Dim oXMLDocPT As New XmlDocument()
                    oXMLDocPT.LoadXml(oXMLData)
                    For Each oBandNode As XmlNode In oXMLDocPT.SelectNodes("/rows/RIBAND[contains(@Name,'Current_')]")
                        If oBandNode.Attributes("OverrideReasonId") IsNot Nothing Then oBandNode.Attributes("OverrideReasonId").Value = "0"
                        For Each oRow As XmlNode In oBandNode.SelectNodes("ArrangementRow")
                            If oRow.Attributes("IsEdited") IsNot Nothing Then oRow.Attributes("IsEdited").Value = "False"
                            'If oRow.Attributes("IsUserEdited") IsNot Nothing Then oRow.Attributes("IsUserEdited").Value = "False"
                            If oRow.Attributes("IsEditedDB") IsNot Nothing Then oRow.Attributes("IsEditedDB").Value = "False"
                        Next
                    Next
                    oXMLData = oXMLDocPT.OuterXml
                End If

                ' Re-apply manually added treaty data from previous session XML � SAM does not persist these attributes
                If Not String.IsNullOrEmpty(sPreviousXML) AndAlso Session(CNMode) <> Mode.View Then
                    Try
                        Dim oPrevDoc As New XmlDocument()
                        oPrevDoc.LoadXml(sPreviousXML)
                        Dim oNewDoc As New XmlDocument()
                        oNewDoc.LoadXml(oXMLData)
                        Dim oManualNodes As XmlNodeList = oPrevDoc.SelectNodes("//ArrangementRow[@ManuallyAdded='True' and @IsDeleted!='True']")
                        For Each oManualNode As XmlNode In oManualNodes
                            Dim sTreatyCode As String = If(oManualNode.Attributes("TreatyCode") IsNot Nothing, oManualNode.Attributes("TreatyCode").Value, "")
                            If Not String.IsNullOrEmpty(sTreatyCode) Then
                                ' Only restore onto nodes that SAM also returned as ManuallyAdded=True,
                                ' preventing stale SI/Premium from overwriting auto-treaty lines with the same TreatyCode
                                ' Also scope to the same RIBAND (same band name) to prevent cross-band contamination
                                Dim sPrevBandName As String = ""
                                If oManualNode.ParentNode IsNot Nothing AndAlso oManualNode.ParentNode.Attributes("Name") IsNot Nothing Then
                                    sPrevBandName = oManualNode.ParentNode.Attributes("Name").Value
                                End If
                                Dim oMatchNode As XmlNode = Nothing
                                If Not String.IsNullOrEmpty(sPrevBandName) Then
                                    oMatchNode = oNewDoc.SelectSingleNode("/rows/RIBAND[@Name=" & XPathLiteral(sPrevBandName) & "]/ArrangementRow[@TreatyCode=" & XPathLiteral(sTreatyCode) & " and @ManuallyAdded='True']")
                                Else
                                    oMatchNode = oNewDoc.SelectSingleNode("//ArrangementRow[@TreatyCode=" & XPathLiteral(sTreatyCode) & " and @ManuallyAdded='True']")
                                End If
                                If oMatchNode IsNot Nothing Then
                                    ' Restore all attributes that SAM does not persist for manually added treaties
                                    For Each sAttr As String In New String() {"ManuallyAdded", "Priority", "TreatyTypeID", "LowerLimit", "LineLimit", "SumInsured", "Premium", "Tax", "CommissionPerc", "Commission", "CommissionTax", "TreatyId", "IsObligatory", "IsPortfolioTransferred", "NumberOfLines"}
                                        If oManualNode.Attributes(sAttr) IsNot Nothing Then
                                            If oMatchNode.Attributes(sAttr) Is Nothing Then
                                                Dim attr As XmlAttribute = oNewDoc.CreateAttribute(sAttr)
                                                attr.Value = oManualNode.Attributes(sAttr).Value
                                                oMatchNode.Attributes.Append(attr)
                                            Else
                                                oMatchNode.Attributes(sAttr).Value = oManualNode.Attributes(sAttr).Value
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Next
                        oXMLData = oNewDoc.OuterXml
                    Catch
                    End Try
                End If


                ' PBI 35359: Restore edited treaty values (Agreement, CommissionPerc, SumInsured, Premium)
                ' from previous session XML for IsEditedDB=True non-manually-added rows.
                ' When the user navigates away and back within the same MTA session, a fresh SAM call
                ' may return stale/blank values for fields edited in-session but not yet saved to DB.
                If Not String.IsNullOrEmpty(sPreviousXML) AndAlso Session(CNMode) <> Mode.View Then
                    Try
                        Dim oPrevDocEdited As New XmlDocument()
                        oPrevDocEdited.LoadXml(sPreviousXML)
                        Dim oNewDocEdited As New XmlDocument()
                        oNewDocEdited.LoadXml(oXMLData)
                        Dim oEditedNodes As XmlNodeList = oPrevDocEdited.SelectNodes("//ArrangementRow[@IsEditedDB='True' and (not(@ManuallyAdded) or @ManuallyAdded='False') and @IsDeleted!='True']")
                        For Each oEditedNode As XmlNode In oEditedNodes
                            Dim iLineKey As String = If(oEditedNode.Attributes("RIArrangementLineKey") IsNot Nothing, oEditedNode.Attributes("RIArrangementLineKey").Value, "")
                            If String.IsNullOrEmpty(iLineKey) OrElse iLineKey = "0" Then Continue For
                            Dim sPrevBand As String = ""
                            If oEditedNode.ParentNode IsNot Nothing AndAlso oEditedNode.ParentNode.Attributes("Name") IsNot Nothing Then
                                sPrevBand = oEditedNode.ParentNode.Attributes("Name").Value
                            End If
                            If String.IsNullOrEmpty(sPrevBand) Then Continue For
                            Dim oMatchEdited As XmlNode = oNewDocEdited.SelectSingleNode("/rows/RIBAND[@Name='" & sPrevBand.Replace("'", "''") & "']/ArrangementRow[@RIArrangementLineKey='" & iLineKey.Replace("'", "''") & "']")
                            If oMatchEdited Is Nothing Then Continue For
                            For Each sAttr As String In New String() {"Agreement", "CommissionPerc", "SumInsured", "Premium", "IsEditedDB", "ActionType"}
                                If oEditedNode.Attributes(sAttr) IsNot Nothing AndAlso Not String.IsNullOrEmpty(oEditedNode.Attributes(sAttr).Value) Then
                                    If oMatchEdited.Attributes(sAttr) Is Nothing Then
                                        Dim attr As XmlAttribute = oNewDocEdited.CreateAttribute(sAttr)
                                        attr.Value = oEditedNode.Attributes(sAttr).Value
                                        oMatchEdited.Attributes.Append(attr)
                                    Else
                                        oMatchEdited.Attributes(sAttr).Value = oEditedNode.Attributes(sAttr).Value
                                    End If
                                End If
                            Next
                        Next
                        oXMLData = oNewDocEdited.OuterXml
                    Catch
                    End Try
                End If
            End If
            Session.Add(CNRIBandKey, ddlReinsurance.SelectedValue)
            If Session(CNRIBandKey) Is Nothing Or String.IsNullOrEmpty(Session(CNRIBandKey)) Or Convert.ToString(Session(CNRIBandKey)).Trim = "0" Then
                Session(CNRIBandKey) = "0"
                btnAddFacProp.Enabled = False
                btnAddFacXOL.Enabled = False
            End If
            Session.Add(CNRIXMLData, oXMLData)
            ' Store the original non-mutated XML from DB for IsEditedDB comparison
            If Session(CNRIXMLDataOriginal) Is Nothing Then
                Session(CNRIXMLDataOriginal) = oXMLData
            End If
            xdocXMLDoc.LoadXml(Session(CNRIXMLData))
            Dim oALLXMLNodes As XmlNodeList = xdocXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow")
            For Each oALLXMLNode As XmlNode In oALLXMLNodes
                Dim iRIarrangementKey As Integer
                Integer.TryParse(oALLXMLNode.Attributes("RIarrangementKey").Value, iRIarrangementKey)
                If iRIarrangementKey <> 0 Then
                    Session(CNRIArrangementkey) = iRIarrangementKey
                    Exit For
                Else
                    Session(CNRIArrangementkey) = 0
                End If
            Next

            'Recalculate the data
            Dim oReinsurance As New Reinsurance.Reinsurance
            Dim nRIBandKey As Integer
            Dim sRecalcXML As String
            Integer.TryParse(Session(CNRIBandKey), nRIBandKey)
            If Session(CNMode) = Mode.View Then
                sRecalcXML = Convert.ToString(Session(CNRIXMLData))
                oReinsurance.Recalculate(sRecalcXML, Convert.ToString(nRIBandKey), Convert.ToString(Session(CNRIArrangementkey)))
                Session(CNRIXMLData) = sRecalcXML
            ElseIf Session(CNMode) = Mode.PortFolioTransferAmendment Then
                sRecalcXML = Convert.ToString(Session(CNRIXMLData))
                If oQuote.InsuranceFileTypeCode = "MTACAN" Then
                    oReinsurance.Recalculate(sRecalcXML, Convert.ToString(nRIBandKey), Convert.ToString(Session(CNRIArrangementkey)), bIsPortfolioRIAmendment:=True, bIsCancellation:=True)
                Else
                    oReinsurance.Recalculate(sRecalcXML, Convert.ToString(nRIBandKey), Convert.ToString(Session(CNRIArrangementkey)), bIsPortfolioRIAmendment:=True)
                End If
                Session(CNRIXMLData) = sRecalcXML
            ElseIf Session(CNMode) = Mode.ClonedTransferAmendment Then
                Dim r_oList As New NexusProvider.LookupListCollection
                r_oList = oWebService.GetRIVersion(oQuote.Risks(Session(CNCurrentRiskKey)).Key, oQuote.BranchCode)
                If Not r_oList Is Nothing Then
                    For iVersionCount As Integer = 0 To r_oList.Count - 1
                        nRIVersionId = r_oList(iVersionCount).Key
                    Next
                End If
                ViewState.Add("RIVersionId", nRIVersionId)
                sRecalcXML = Convert.ToString(Session(CNRIXMLData))
                If nRIVersionId > 1 Then
                    If oQuote.InsuranceFileTypeCode = "MTACAN" Or oQuote.InsuranceFileStatusCode = "CAN" Or oQuote.InsuranceFileStatusCode = "REPBDMTA" Then
                        oReinsurance.Recalculate(sRecalcXML, nRIBandKey, Session(CNRIArrangementkey), bIsPortfolioRIAmendment:=True, bIsCancellation:=True)
                    Else
                        oReinsurance.Recalculate(sRecalcXML, nRIBandKey, Session(CNRIArrangementkey), bIsPortfolioRIAmendment:=True)
                    End If
                Else
                    If oQuote.InsuranceFileTypeCode = "MTACAN" Or oQuote.InsuranceFileStatusCode = "CAN" Or oQuote.InsuranceFileStatusCode = "REPBDMTA" Then
                        oReinsurance.Recalculate(sRecalcXML, nRIBandKey, Session(CNRIArrangementkey), bIsPortfolioRIAmendment:=False, bIsCancellation:=True)
                    Else
                        oReinsurance.Recalculate(sRecalcXML, nRIBandKey, Session(CNRIArrangementkey), bIsPortfolioRIAmendment:=False)
                    End If
                End If
                Session(CNRIXMLData) = sRecalcXML
            ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                sRecalcXML = Convert.ToString(Session(CNRIXMLData))
                oReinsurance.Recalculate(sRecalcXML, nRIBandKey, Session(CNRIArrangementkey), bIsCancellation:=True)
                Session(CNRIXMLData) = sRecalcXML
            Else
                ' For MTA (PERMANENT/TEMPORARY/REINSTATEMENT/RENEWAL): mark all editable lines as IsEdited
                ' so Recalculate preserves their saved DB values instead of recalculating from DefaultPerc.
                ' PBI 35359: Before ProcessMTA overwrites IsEdited on all T/R rows, snapshot the DB-persisted
                ' IsEdited flag into IsUserEdited so ApplyEditedRowHighlighting can highlight only truly
                ' user-edited rows (not all T/R rows).
                ' Gap 8: Reinstatement behaves like NB (fresh defaults) not MTA (carry forward overrides)
                If Session(CNMTAType) = MTAType.PERMANENT OrElse Session(CNMTAType) = MTAType.TEMPORARY OrElse
                   Session(CNRenewal) = True Then
                    Dim oXMLDocMTA As New System.Xml.XmlDocument
                    oXMLDocMTA.LoadXml(Convert.ToString(Session(CNRIXMLData)))
                    ' PBI 35359: Snapshot IsEditedDB (DB-persisted flag) into IsUserEdited
                    ' BEFORE ProcessMTA runs. IsUserEdited drives row highlighting so only
                    ' genuinely user-edited rows are highlighted on MTA/Renewal load.
                    Dim oAllRows As System.Xml.XmlNodeList = oXMLDocMTA.SelectNodes("//ArrangementRow")
                    For Each oSnapNode As System.Xml.XmlNode In oAllRows
                        Dim bDbEdited As Boolean = False
                        If oSnapNode.Attributes("IsEditedDB") IsNot Nothing Then
                            Boolean.TryParse(oSnapNode.Attributes("IsEditedDB").Value, bDbEdited)
                        End If
                        'IsUserEdited snapshot commented out
                    Next
                    ' PBI 35359 + PBI 31753/35602: ProcessMTA updates Band Total SI
                    ' and updates Band Total SI to the new MTA risk SI so Recalculate uses the
                    ' correct base while preserving ManuallyAdded SI/Premium and IsEdited SI/Premium.
                    Dim dNewSI As Double = 0
                    If oQuote IsNot Nothing AndAlso oQuote.Risks IsNot Nothing AndAlso oQuote.Risks.Count > 0 Then
                        Dim iRiskIdx As Integer = If(Session(CNCurrentRiskKey) IsNot Nothing, CInt(Session(CNCurrentRiskKey)), 0)
                        dNewSI = oQuote.Risks(iRiskIdx).TotalSumInsured
                    End If
                    '    If oSnapNode.Attributes("IsUserEdited") Is Nothing Then
                    '        Dim attrUE As System.Xml.XmlAttribute = oXMLDocMTA.CreateAttribute("IsUserEdited")
                    '        attrUE.Value = bDbEdited.ToString()
                    '        oSnapNode.Attributes.Append(attrUE)
                    '    Else
                    '        oSnapNode.Attributes("IsUserEdited").Value = bDbEdited.ToString()
                    '    End If
                    'Next
                    Session(CNRIXMLData) = oXMLDocMTA.OuterXml
                    'oReinsurance.ProcessMTA(oXMLDocMTA, 0)
                    oReinsurance.ProcessMTA(oXMLDocMTA, dNewSI)
                    Session(CNRIXMLData) = oXMLDocMTA.OuterXml

                End If
                ' Gap 8: Reinstatement — clear any DB-persisted edit flags so the screen loads
                ' with fresh default RI values (same as New Business), not carried-forward overrides.
                If Session(CNMTAType) = MTAType.REINSTATEMENT Then
                    Dim oXMLDocRein As New System.Xml.XmlDocument
                    oXMLDocRein.LoadXml(Convert.ToString(Session(CNRIXMLData)))
                    For Each oReinNode As System.Xml.XmlNode In oXMLDocRein.SelectNodes("//ArrangementRow")
                        'If oReinNode.Attributes("IsUserEdited") IsNot Nothing Then oReinNode.Attributes("IsUserEdited").Value = "False"
                        If oReinNode.Attributes("IsEditedDB") IsNot Nothing Then oReinNode.Attributes("IsEditedDB").Value = "False"
                    Next
                    For Each oBandNode As System.Xml.XmlNode In oXMLDocRein.SelectNodes("/rows/RIBAND[contains(@Name,'Current_')]")
                        If oBandNode.Attributes("OverrideReasonId") IsNot Nothing Then oBandNode.Attributes("OverrideReasonId").Value = "0"
                    Next
                    Session(CNRIXMLData) = oXMLDocRein.OuterXml
                End If
                sRecalcXML = Convert.ToString(Session(CNRIXMLData))
                oReinsurance.Recalculate(sRecalcXML, nRIBandKey, Session(CNRIArrangementkey))
                Session(CNRIXMLData) = sRecalcXML
            End If

            ' Bug #39513: Calculate tax for obligatory nodes that have Tax=0 and a valid PartyKey
            CalculateObligatoryTax()
            oXMLData = Convert.ToString(Session(CNRIXMLData))

            oXMLSource.Data = oXMLData
            oXMLSource.XPath = ".//RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow[@IsDeleted='False']"
            gvReinsurance.DataSource = oXMLSource
            gvReinsurance.DataBind()

            Session.Remove(CNNEXTBUTTON)
            ApplyEditedRowHighlighting()
            RestoreOverrideReasonState()

            Dim oOriginalXMLNodes As XmlNodeList = xdocXMLDoc.SelectNodes("/rows/RIBAND[@Name='Original_" & ddlReinsurance.SelectedValue & "']/ArrangementRow")

            If oOriginalXMLNodes IsNot Nothing Then
                If oOriginalXMLNodes.Count > 0 Then
                    lblOrgReinsurance.Visible = True
                    gvOrgReinsurance.Visible = True
                    oXMLSource.XPath = ".//*[@Name='Original_" & ddlReinsurance.SelectedValue & "']/ArrangementRow"
                    gvOrgReinsurance.DataSource = oXMLSource
                    gvOrgReinsurance.DataBind()
                    ApplyOrgEditedRowHighlighting()
                Else
                    lblOrgReinsurance.Visible = False
                    gvOrgReinsurance.Visible = False
                End If
            Else
                lblOrgReinsurance.Visible = False
            End If
            Dim iTotalRow As Integer = gvReinsurance.Rows.Count - 1
            For iCount As Integer = 0 To iTotalRow
                If Trim(gvReinsurance.Rows(iCount).Cells(2).Text) = "Treaty QSH" Then
                    gvReinsurance.Rows(iCount).Cells(8).Text = ""
                End If
                If Trim(gvReinsurance.Rows(iCount).Cells(3).Text) = "Band Total" Then
                    Dim lblSumInsured As Label = DirectCast(gvReinsurance.Rows(iCount).FindControl("lblSumInsured"), Label)
                    If lblSumInsured IsNot Nothing AndAlso Not String.IsNullOrEmpty(lblSumInsured.Text) Then
                        Dim sSIText As String = lblSumInsured.Text.Replace(",", "")
                        If IsNumeric(sSIText) Then dAllocatedSIAmount = Convert.ToDouble(sSIText)
                    End If
                    Dim lblPremium As Label = DirectCast(gvReinsurance.Rows(iCount).FindControl("lblPremium"), Label)
                    If lblPremium IsNot Nothing AndAlso Not String.IsNullOrEmpty(lblPremium.Text) Then
                        Dim sPremText As String = lblPremium.Text.Replace(",", "")
                        If IsNumeric(sPremText) Then dAllocatedPerAmount = Convert.ToDouble(sPremText)
                    End If
                End If
                If Trim(gvReinsurance.Rows(iCount).Cells(3).Text) = "Net" Then
                    gvReinsurance.Rows(iCount).Cells(10).Text = String.Format("{0:N2}", (dAllocatedSIAmount + dOriginalSIAmount))
                    gvReinsurance.Rows(iCount).Cells(11).Text = String.Format("{0:N2}", (dAllocatedPerAmount + dOriginalPerAmount))
                End If
            Next
        End Sub

        ''' <summary>
        ''' Fetches CommissionPercent from GetRITreatyPartyDetailsWithTax for manually added treaties where CommissionPerc is 0
        ''' </summary>
        Function GetCommissionPercentage(ByVal sXML As String, ByVal RIBANDID As String, ByVal iRIArrangementLineKey As Integer, ByVal sTreatyCode As String) As Double
            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(sXML)
            Dim sanitizedBandId As String = RIBANDID.ToString().Trim().Replace("'", "''")
            Dim oNode As XmlNode = Nothing
            If iRIArrangementLineKey > 0 Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@RIArrangementLineKey='" & iRIArrangementLineKey & "']")
            ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "' and @IsDeleted='False']")
            End If
            If oNode Is Nothing Then Return 0
            Dim dCommPerc As Double = 0
            If oNode.Attributes("CommissionPerc") IsNot Nothing Then
                Double.TryParse(oNode.Attributes("CommissionPerc").Value, dCommPerc)
            End If
            If dCommPerc > 0 Then Return dCommPerc
            ' CommissionPerc is 0 - fetch from SAM via GetRITreatyPartyDetailsWithTax
            Dim sTreatyCodeNode As String = If(oNode.Attributes("TreatyCode") IsNot Nothing, oNode.Attributes("TreatyCode").Value, sTreatyCode)
            Dim iTreatyID As Integer = 0
            If oNode.Attributes("TreatyId") IsNot Nothing Then Integer.TryParse(oNode.Attributes("TreatyId").Value, iTreatyID)
            Dim sLineType As String = If(oNode.Attributes("Type") IsNot Nothing, oNode.Attributes("Type").Value, "T")
            oQuote = CType(Session(CNQuote), NexusProvider.Quote)
            Dim oRITreatyDetailsWithTax As New NexusProvider.RITreatyPartyWithTax
            oRITreatyDetailsWithTax.InsuranceFileID = oQuote.InsuranceFileKey
            oRITreatyDetailsWithTax.Premium = 0
            oRITreatyDetailsWithTax.PremiumTransType = IIf(sLineType = "T" OrElse sLineType = "TX", "TTRITP", "TTRIFP")
            oRITreatyDetailsWithTax.RIArrangementLineID = iRIArrangementLineKey
            oRITreatyDetailsWithTax.RiskID = oQuote.Risks(Session(CNCurrentRiskKey)).Key
            oRITreatyDetailsWithTax.Commission = 0
            oRITreatyDetailsWithTax.CommissionTransType = IIf(sLineType = "T" OrElse sLineType = "TX", "TTRITC", "TTRIFC")
            oRITreatyDetailsWithTax.TreatyCode = sTreatyCodeNode
            oRITreatyDetailsWithTax.TreatyID = iTreatyID
            oRITreatyDetailsWithTax.IgnoreDetails = False
            oRITreatyDetailsWithTax.IgnoreTax = True
            oWebService = New NexusProvider.ProviderManager().Provider
            oWebService.GetRITreatyPartyDetailsWithTax(oRITreatyDetailsWithTax, "")
            Return Convert.ToDouble(oRITreatyDetailsWithTax.CommissionPercent)
        End Function

        Function GetTaxPercentage(ByVal dPremium As Double, ByVal iPartyKey As Integer, ByVal sXML As String, ByVal RIBANDID As Integer, ByVal iRIArrangementLineKey As Integer, Optional ByVal sTreatyCode As String = "") As Double
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim iRiskKey As Integer = 0
            Dim iInsuranceFileKey As Integer = 0
            Dim dTaxPercentage As Double = 0
            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(sXML)

            ' Sanitize inputs to prevent XPath injection
            Dim sanitizedBandId As String = RIBANDID.ToString().Trim().Replace("'", "''")
            If iRIArrangementLineKey < 0 Then iRIArrangementLineKey = 0
            Dim sanitizedLineKey As String = iRIArrangementLineKey.ToString().Replace("'", "''")

            Dim oDNode As XmlNode = Nothing
            If iRIArrangementLineKey > 0 Then
                oDNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@RIArrangementLineKey='" & sanitizedLineKey & "']")
            ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                Dim sanitizedTreatyCode As String = sTreatyCode.Replace("'", "''")
                oDNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@TreatyCode='" & sanitizedTreatyCode & "' and @IsDeleted='False']")
            End If
            If oDNode IsNot Nothing AndAlso oDNode.Attributes("PartyKey") IsNot Nothing Then
                Integer.TryParse(oDNode.Attributes("PartyKey").Value, iPartyKey)
                If iPartyKey > 0 Then
                    iRiskKey = CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key
                    iInsuranceFileKey = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey
                    If iRiskKey > 0 AndAlso iInsuranceFileKey > 0 Then
                        oWebService.CalculateRITax(iRiskKey, iPartyKey, dPremium, iInsuranceFileKey, dTaxPercentage)
                    End If
                ElseIf oDNode.Attributes("ManuallyAdded") IsNot Nothing AndAlso oDNode.Attributes("ManuallyAdded").Value = "True" Then
                    ' For manually added treaties (PartyKey=0), use GetRITreatyPartyDetailsWithTax like RecalculateTaxes in Reinsurance.ascx.vb
                    Dim sTreatyCodeNode As String = If(oDNode.Attributes("TreatyCode") IsNot Nothing, oDNode.Attributes("TreatyCode").Value, sTreatyCode)
                    Dim iTreatyID As Integer = 0
                    If oDNode.Attributes("TreatyId") IsNot Nothing Then Integer.TryParse(oDNode.Attributes("TreatyId").Value, iTreatyID)
                    Dim sLineType As String = If(oDNode.Attributes("Type") IsNot Nothing, oDNode.Attributes("Type").Value, "T")
                    oQuote = CType(Session(CNQuote), NexusProvider.Quote)
                    Dim oRITreatyDetailsWithTax As New NexusProvider.RITreatyPartyWithTax
                    oRITreatyDetailsWithTax.InsuranceFileID = oQuote.InsuranceFileKey
                    oRITreatyDetailsWithTax.Premium = Convert.ToDecimal(dPremium)
                    oRITreatyDetailsWithTax.PremiumTransType = IIf(sLineType = "T" OrElse sLineType = "TX", "TTRITP", "TTRIFP")
                    oRITreatyDetailsWithTax.RIArrangementLineID = iRIArrangementLineKey
                    oRITreatyDetailsWithTax.RiskID = oQuote.Risks(Session(CNCurrentRiskKey)).Key
                    oRITreatyDetailsWithTax.Commission = 0
                    oRITreatyDetailsWithTax.CommissionTransType = IIf(sLineType = "T" OrElse sLineType = "TX", "TTRITC", "TTRIFC")
                    oRITreatyDetailsWithTax.TreatyCode = sTreatyCodeNode
                    oRITreatyDetailsWithTax.TreatyID = iTreatyID
                    oRITreatyDetailsWithTax.IgnoreDetails = True
                    oRITreatyDetailsWithTax.IgnoreTax = False
                    oWebService.GetRITreatyPartyDetailsWithTax(oRITreatyDetailsWithTax, "")
                    dTaxPercentage = If(dPremium <> 0, Convert.ToDouble(oRITreatyDetailsWithTax.PremiumTax) / dPremium * 100, 0)
                    ' Store CommissionTax rate on node � use premium as base since commission may not be set yet
                    If oDNode.Attributes("CommissionTaxPerc") Is Nothing Then
                        Dim attrCTP As XmlAttribute = oXMLDoc.CreateAttribute("CommissionTaxPerc")
                        attrCTP.Value = "0"
                        oDNode.Attributes.Append(attrCTP)
                    End If
                    Dim dCommTaxAmt As Double = Convert.ToDouble(oRITreatyDetailsWithTax.CommissionTax)
                    Dim dCommAmt As Double = Convert.ToDouble(oRITreatyDetailsWithTax.Commission)
                    If dCommAmt <> 0 Then
                        oDNode.Attributes("CommissionTaxPerc").Value = Format(dCommTaxAmt / dCommAmt * 100, "0.0000")
                    ElseIf dPremium <> 0 AndAlso Convert.ToDouble(oDNode.Attributes("CommissionPerc").Value) > 0 Then
                        ' Derive commission amount from premium and CommissionPerc to get the rate
                        Dim dDerivedComm As Double = dPremium * Convert.ToDouble(oDNode.Attributes("CommissionPerc").Value) / 100
                        If dDerivedComm <> 0 Then
                            oDNode.Attributes("CommissionTaxPerc").Value = Format(dCommTaxAmt / dDerivedComm * 100, "0.0000")
                        End If
                    End If
                End If
            End If

            Return dTaxPercentage
        End Function

        ''' <summary>
        ''' Bug #39513: Calculate tax for treaty nodes that have Tax=0 but a valid TreatyCode.
        ''' Called after Recalculate to ensure all treaty lines get their tax from SAM.
        ''' </summary>
        Private Sub CalculateObligatoryTax()
            If Session(CNRIXMLData) Is Nothing OrElse String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLData))) Then Exit Sub
            Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim sBandKey As String = Convert.ToString(Session(CNRIBandKey))
            If String.IsNullOrEmpty(sBandKey) OrElse sBandKey = "0" Then Exit Sub

            ' Select all treaty nodes with Tax=0 (or empty) that have a TreatyCode - excludes Band Total, Net of FAC, Allocated, Unallocated
            Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sBandKey & "']/ArrangementRow[@IsDeleted!='True' and @TreatyCode!='' and @Type!='' and @Premium!='0' and @Premium!='0.00']")
            If oNodes Is Nothing OrElse oNodes.Count = 0 Then Exit Sub

            Dim bChanged As Boolean = False
            oWebService = New NexusProvider.ProviderManager().Provider
            oQuote = CType(Session(CNQuote), NexusProvider.Quote)

            For Each oNode As XmlNode In oNodes
                Dim dPremium As Double = 0
                If oNode.Attributes("Premium") IsNot Nothing Then Double.TryParse(oNode.Attributes("Premium").Value, dPremium)
                If dPremium = 0 Then Continue For

                Dim sTreatyCode As String = If(oNode.Attributes("TreatyCode") IsNot Nothing, oNode.Attributes("TreatyCode").Value, "")
                Dim iTreatyID As Integer = 0
                If oNode.Attributes("TreatyId") IsNot Nothing Then Integer.TryParse(oNode.Attributes("TreatyId").Value, iTreatyID)
                If String.IsNullOrEmpty(sTreatyCode) AndAlso iTreatyID = 0 Then Continue For

                Dim sLineType As String = If(oNode.Attributes("Type") IsNot Nothing, oNode.Attributes("Type").Value, "T")
                Dim iRIArrangementLineKey As Integer = 0
                If oNode.Attributes("RIArrangementLineKey") IsNot Nothing Then Integer.TryParse(oNode.Attributes("RIArrangementLineKey").Value, iRIArrangementLineKey)

                Dim dCommission As Double = 0
                If oNode.Attributes("Commission") IsNot Nothing Then Double.TryParse(oNode.Attributes("Commission").Value, dCommission)

                Dim oRITreatyDetailsWithTax As New NexusProvider.RITreatyPartyWithTax
                oRITreatyDetailsWithTax.InsuranceFileID = oQuote.InsuranceFileKey
                oRITreatyDetailsWithTax.Premium = Convert.ToDecimal(dPremium)
                oRITreatyDetailsWithTax.PremiumTransType = IIf(sLineType = "T" OrElse sLineType = "TX", "TTRITP", "TTRIFP")
                oRITreatyDetailsWithTax.RIArrangementLineID = iRIArrangementLineKey
                oRITreatyDetailsWithTax.RiskID = oQuote.Risks(Session(CNCurrentRiskKey)).Key
                oRITreatyDetailsWithTax.Commission = Convert.ToDecimal(dCommission)
                oRITreatyDetailsWithTax.CommissionTransType = IIf(sLineType = "T" OrElse sLineType = "TX", "TTRITC", "TTRIFC")
                oRITreatyDetailsWithTax.TreatyCode = sTreatyCode
                oRITreatyDetailsWithTax.TreatyID = iTreatyID
                oRITreatyDetailsWithTax.IgnoreDetails = True
                oRITreatyDetailsWithTax.IgnoreTax = False

                oWebService.GetRITreatyPartyDetailsWithTax(oRITreatyDetailsWithTax, "")

                Dim dPremTax As Double = Convert.ToDouble(oRITreatyDetailsWithTax.PremiumTax)
                Dim dCommTax As Double = Convert.ToDouble(oRITreatyDetailsWithTax.CommissionTax)

                ' Always write SAM result back (SAM is source of truth for tax amounts)
                oNode.Attributes("Tax").Value = Format(dPremTax, "0.00")
                If oNode.Attributes("CommissionTax") IsNot Nothing Then
                    oNode.Attributes("CommissionTax").Value = Format(dCommTax, "0.00")
                End If
                bChanged = True
            Next

            If bChanged Then
                Session(CNRIXMLData) = oXMLDoc.OuterXml
            End If
        End Sub

        Protected Sub txtThisPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim value As String = CType(sender, TextBox).Text

            If InStr(1, value, "%") <> 0 Then
                value = value.Substring(0, value.Trim.Length - 1)
            End If

            Dim oReinsurance As New Reinsurance.Reinsurance
            Dim RIBANDID As String = ddlReinsurance.SelectedValue.ToString.Trim
            Dim oXMLData As String = Convert.ToString(Session(CNRIXMLData))
            Dim oTempXMLData As String = Convert.ToString(Session(CNRIXMLData))
            Dim sMinNumber As String = Double.MinValue.ToString
            Dim sMaxNumber As String = Double.MaxValue.ToString
            Dim dTaxPercentage As Double = 0
            Dim dTotPercentage As Double = 0
            Dim bIsPortfolioRIAmendment As Boolean = False
            dTaxPercentage = GetTaxPercentage(100, 0, oXMLData, RIBANDID, CType(sender, TextBox).ToolTip)
            nRIVersionId = ViewState("RIVersionId")
            If Session(CNMode) = Mode.PortFolioTransferAmendment Then
                bIsPortfolioRIAmendment = True
            ElseIf Session(CNMode) = Mode.ClonedTransferAmendment And nRIVersionId > 1 Then
                bIsPortfolioRIAmendment = True
            End If

            If String.IsNullOrEmpty(value) Or (Not IsNumeric(value)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_InvalidThisRate").ToString() & "');", True)
            Else
                oReinsurance.UpdateThisPercentage(oTempXMLData, Convert.ToDouble(value), RIBANDID, CType(sender, TextBox).ToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
                dTotPercentage = GetTotalPercentage(oTempXMLData, "F")
                If Convert.ToDouble(value) > 100 Or Convert.ToDouble(value) <= 0 Or Convert.ToDouble(dTotPercentage) > 100 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_InvalidThisRate").ToString() & "');", True)
                Else
                    value = Math.Round(Convert.ToDouble(value), 4)
                    oReinsurance.UpdateThisPercentage(oXMLData, Convert.ToDouble(value), RIBANDID, CType(sender, TextBox).ToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
                    ' GAP 2 fix: mark row as edited only if value differs from original
                    Dim bThisPercDiffers As Boolean = True
                    If Not String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLDataOriginal))) Then
                        Dim oOrigDocTP As New XmlDocument()
                        oOrigDocTP.LoadXml(Convert.ToString(Session(CNRIXMLDataOriginal)))
                        Dim sBandKeyTP As String = RIBANDID.Replace("'", "''")
                        Dim oOrigNodeTP As XmlNode = oOrigDocTP.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKeyTP & "']/ArrangementRow[@RIArrangementLineKey='" & CType(sender, TextBox).ToolTip & "']")
                        If oOrigNodeTP IsNot Nothing AndAlso oOrigNodeTP.Attributes("ThisPerc") IsNot Nothing Then
                            Dim dOrigTP As Double = 0
                            Double.TryParse(oOrigNodeTP.Attributes("ThisPerc").Value, dOrigTP)
                            If Math.Round(Convert.ToDouble(value), 4) = Math.Round(dOrigTP, 4) Then
                                bThisPercDiffers = False
                            End If
                        End If
                    End If
                    If bThisPercDiffers Then
                        MarkRowAsEdited(oXMLData, RIBANDID, CType(sender, TextBox).ToolTip)
                        If Not IsFACLine(oXMLData, RIBANDID, CType(sender, TextBox).ToolTip) Then
                            pnlOverrideReason.Visible = True
                        End If
                    Else
                        ClearRowEditedFlags(oXMLData, RIBANDID, CType(sender, TextBox).ToolTip)
                        If Not BandHasEditedRows(oXMLData, RIBANDID) Then pnlOverrideReason.Visible = False
                    End If

                    ' Copy This % to FAC Prop Premium % if Non-Proportional and FAC Prop Premium % is empty
                    If bIsFACPremiumNonProportional Then
                        Dim oXMLDoc As New XmlDocument
                        oXMLDoc.LoadXml(oXMLData)
                        Dim sanitizedBandId As String = RIBANDID.Replace("'", "''")
                        Dim sanitizedToolTip As String = CType(sender, TextBox).ToolTip.Replace("'", "''")
                        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@RIArrangementLineKey='" & sanitizedToolTip & "']")
                        If oNode IsNot Nothing AndAlso oNode.Attributes("Placement") IsNot Nothing AndAlso oNode.Attributes("Placement").Value = "FAC Prop" Then
                            Dim sFACPropPremiumPerc As String = ""
                            If oNode.Attributes("FACPropPremiumPerc") IsNot Nothing Then
                                sFACPropPremiumPerc = oNode.Attributes("FACPropPremiumPerc").Value
                            End If
                            If String.IsNullOrEmpty(sFACPropPremiumPerc) OrElse Convert.ToDouble(sFACPropPremiumPerc) = 0 Then
                                oReinsurance.UpdateFACPropPremiumPercentage(oXMLData, Convert.ToDouble(value), RIBANDID, CType(sender, TextBox).ToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
                            End If
                        End If
                    End If
                End If

            End If
            Session(CNRIXMLData) = oXMLData
            ' Bug #39513: Recalculate tax for treaty nodes reset to 0
            CalculateObligatoryTax()
            oXMLData = Convert.ToString(Session(CNRIXMLData))
            'Update the Grid with latest calculation
            Dim oXMLSource As New XmlDataSource

            oXMLSource.EnableCaching = False

            oXMLSource.Data = oXMLData
            oXMLSource.XPath = ".//RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow[@IsDeleted='False']"

            gvReinsurance.DataSource = oXMLSource
            gvReinsurance.DataBind()
            ApplyEditedRowHighlighting()
        End Sub

        Protected Sub txtSumInsured_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                HandleConcurrentModification()
                If Session(CNRIXMLData) Is Nothing Then Return

                Dim txtSumInsured As TextBox = CType(sender, TextBox)
                Dim iRIArrangementLineKey As Integer = 0
                Integer.TryParse(txtSumInsured.ToolTip, iRIArrangementLineKey)
                Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
                Dim sRIBANDID As String = Convert.ToString(Session(CNRIBandKey))
                Dim sTreatyCode As String = ""
                sTreatyCode = GetTreatyCodeFromGridRow(sXML, sRIBANDID, txtSumInsured)

                Dim value As String = txtSumInsured.Text.Trim().Replace(",", "").Replace("$", "").Replace(" ", "")

                If String.IsNullOrEmpty(value) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_InvalidSumInsured").ToString() & "');", True)
                    Exit Sub
                End If

                If Not IsNumeric(value) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_InvalidSumInsured").ToString() & "');", True)
                    Exit Sub
                End If

                Dim oReinsurance As New Reinsurance.Reinsurance
                Dim RIBANDID As String = ddlReinsurance.SelectedValue.ToString.Trim
                Dim oXMLData As String = Convert.ToString(Session(CNRIXMLData))
                Dim sMinNumber As String = Double.MinValue.ToString
                Dim sMaxNumber As String = Double.MaxValue.ToString
                Dim dTaxPercentage As Double = 0
                Dim oTempXMLData As String = Convert.ToString(Session(CNRIXMLData))
                Dim dTotalRISum As Double = 0
                Dim dBandTotal As Double = 0
                Dim oXMLDoc As New XmlDocument
                Dim oBXMLNode As XmlNode
                Dim bIsPortfolioRIAmendment As Boolean = False
                nRIVersionId = ViewState("RIVersionId")
                If Session(CNMode) = Mode.PortFolioTransferAmendment Then
                    bIsPortfolioRIAmendment = True
                ElseIf Session(CNMode) = Mode.ClonedTransferAmendment And nRIVersionId > 1 Then
                    bIsPortfolioRIAmendment = True
                End If

                oXMLDoc.LoadXml(oXMLData)
                Dim sanitizedBandKey As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
                dTaxPercentage = GetTaxPercentage(100, 0, oXMLData, RIBANDID, iRIArrangementLineKey)
                If Convert.ToDecimal(value) < 0 Then
                    ScriptManager.RegisterStartupScript(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_NegativeSumInsuredAmount").ToString() & "');", True)
                    Exit Sub
                End If

                Dim dValueRounded As Double = Math.Round(Convert.ToDouble(value), 2)
                value = dValueRounded.ToString()
                oXMLDoc.LoadXml(oTempXMLData)
                sanitizedBandKey = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
                oBXMLNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Name='Band Total']")
                If oBXMLNode IsNot Nothing Then
                    If oBXMLNode.Attributes("SumInsured").Value <> "" Then
                        dBandTotal = Convert.ToDouble(oBXMLNode.Attributes("SumInsured").Value)
                    End If
                End If

                Dim bIsManuallyAdded As Boolean = IsManuallyAdded(oTempXMLData, sanitizedBandKey, iRIArrangementLineKey.ToString(), sTreatyCode)
                If bIsManuallyAdded Then
                    Dim dthis_premium As Double
                    Dim sanitizedTreatyCode As String = sTreatyCode.Replace("'", "''")
                    Dim oTreatyNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@TreatyCode='" & sanitizedTreatyCode & "' and @IsDeleted='False']")
                    'Dim oTreatyNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@TreatyCode='" & sTreatyCode & "' and @IsDeleted='False']")
                    If oTreatyNode IsNot Nothing Then
                        Dim sTreatyType As String = If(oTreatyNode.Attributes("Type") IsNot Nothing, oTreatyNode.Attributes("Type").Value, "")
                        If sTreatyType = "TX" Then
                            Dim dLowerLimit As Double = 0
                            Dim dUpperLimit As Double = 0
                            If oTreatyNode.Attributes("LowerLimit") IsNot Nothing Then Double.TryParse(oTreatyNode.Attributes("LowerLimit").Value, dLowerLimit)
                            If oTreatyNode.Attributes("LineLimit") IsNot Nothing Then Double.TryParse(oTreatyNode.Attributes("LineLimit").Value, dUpperLimit)
                            If dUpperLimit = 0 Then
                                Dim sMessage As String = String.Format(GetLocalResourceObject("msg_XOLLimitsNotSet").ToString(), oTreatyNode.Attributes("Name").Value)
                                ScriptManager.RegisterStartupScript(Page, GetType(Page), "siNoLimits", "alert('" & sMessage.Replace("'", "\'") & "');", True)
                                Exit Sub
                            End If
                            If Convert.ToDouble(value) < dLowerLimit OrElse Convert.ToDouble(value) > dUpperLimit Then
                                Dim sMessage As String = String.Format("Sum Insured for {0} must be between {1:N2} and {2:N2}.", oTreatyNode.Attributes("Name").Value, dLowerLimit, dUpperLimit)
                                ScriptManager.RegisterStartupScript(Page, GetType(Page), "siOutOfRange", "alert('" & sMessage.Replace("'", "\'") & "');", True)
                                Exit Sub
                            End If
                            Exit Sub
                        End If
                        oTreatyNode.Attributes("SumInsured").Value = value.ToString()
                        Dim netValues As Tuple(Of Double, Double) = GetNetOfFACValues(oTempXMLData, sanitizedBandKey)
                        Dim dThisPerc As Double = RecalculateThisPercentage(Convert.ToDouble(value), netValues.Item1)
                        oTreatyNode.Attributes("ThisPerc").Value = Format(dThisPerc, "0.0000")
                        Dim dCalculatedPremium As Double = (dThisPerc / 100) * netValues.Item2
                        oTreatyNode.Attributes("Premium").Value = Math.Round(dCalculatedPremium, 2).ToString()
                        oTempXMLData = oXMLDoc.OuterXml
                        dTaxPercentage = GetTaxPercentage(dCalculatedPremium, 0, oTempXMLData, RIBANDID, iRIArrangementLineKey, sTreatyCode)
                        Dim dCommPerc As Double = 0
                        If oTreatyNode.Attributes("CommissionPerc") IsNot Nothing Then Double.TryParse(oTreatyNode.Attributes("CommissionPerc").Value, dCommPerc)
                        If dCommPerc = 0 Then dCommPerc = GetCommissionPercentage(oTempXMLData, RIBANDID, iRIArrangementLineKey, sTreatyCode)
                        oReinsurance.ApplyManualTreatyFinancials(oTreatyNode, dCalculatedPremium, dTaxPercentage, dCommPerc)
                        oTempXMLData = oXMLDoc.OuterXml
                        If dCommPerc > 0 Then
                            oReinsurance.UpdateComissionPercentage(oTempXMLData, dCommPerc, RIBANDID, CType(sender, TextBox).ToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment, sTreatyCode:=sTreatyCode)
                        End If
                    End If
                    oTempXMLData = oXMLDoc.OuterXml
                End If
                ' Only mark as edited if the new value differs from the original DB value
                Dim bValueDiffersFromOriginal As Boolean = True
                If Not String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLDataOriginal))) Then
                    Dim oOrigDoc As New XmlDocument()
                    oOrigDoc.LoadXml(Convert.ToString(Session(CNRIXMLDataOriginal)))
                    Dim oOrigNode As XmlNode = Nothing
                    If iRIArrangementLineKey > 0 Then
                        oOrigNode = oOrigDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@RIArrangementLineKey='" & iRIArrangementLineKey & "']")
                    ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                        oOrigNode = oOrigDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
                    End If
                    If oOrigNode IsNot Nothing AndAlso oOrigNode.Attributes("SumInsured") IsNot Nothing Then


                        Dim dOrigSI As Double = 0
                        Double.TryParse(oOrigNode.Attributes("SumInsured").Value, dOrigSI)
                        ' Compare user-entered value against the pre-edit session value (not post-Recalculate)
                        Dim dPreEditSI As Double = 0
                        Dim oPreEditDoc As New XmlDocument()
                        oPreEditDoc.LoadXml(oXMLData)
                        Dim oPreEditNode As XmlNode = Nothing
                        If iRIArrangementLineKey > 0 Then
                            oPreEditNode = oPreEditDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@RIArrangementLineKey='" & iRIArrangementLineKey & "']")
                        ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                            oPreEditNode = oPreEditDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
                        End If
                        If oPreEditNode IsNot Nothing AndAlso oPreEditNode.Attributes("SumInsured") IsNot Nothing Then
                            Double.TryParse(oPreEditNode.Attributes("SumInsured").Value, dPreEditSI)
                        End If
                        ' Value differs if it differs from EITHER the original DB value OR the pre-edit session value
                        If Math.Round(dValueRounded, 2) = Math.Round(dOrigSI, 2) Then
                            bValueDiffersFromOriginal = False
                        End If
                    End If
                End If
                oReinsurance.UpdateRISumInsured(oTempXMLData, Convert.ToDouble(value), RIBANDID, CType(sender, TextBox).ToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment, sTreatyCode:=sTreatyCode, sOriginalXML:=Convert.ToString(Session(CNRIXMLDataOriginal)))
                ' Mark/clear edited flags AFTER UpdateRISumInsured so Recalculate inside it cannot reset IsUserEdited
                If bValueDiffersFromOriginal Then
                    MarkRowAsEdited(oTempXMLData, RIBANDID, iRIArrangementLineKey.ToString(), sTreatyCode)
                    If Not IsFACLine(oTempXMLData, RIBANDID, iRIArrangementLineKey.ToString(), sTreatyCode) Then
                        If BandHasEditedRows(oTempXMLData, RIBANDID) Then pnlOverrideReason.Visible = True
                    End If
                Else
                    ClearRowEditedFlags(oTempXMLData, RIBANDID, iRIArrangementLineKey.ToString(), sTreatyCode)
                    If Not BandHasEditedRows(oTempXMLData, RIBANDID) Then pnlOverrideReason.Visible = False
                End If
                dTotalRISum = GetTotalRISum(oTempXMLData, "F")
                If HasManuallyAddedTreaties(oTempXMLData, sanitizedBandKey) Then
                    dTotalRISum += GetTotalRISum(oTempXMLData, "T") + GetTotalRISum(oTempXMLData, "TX")
                End If
                If Math.Round(Convert.ToDouble(dTotalRISum), 0) > Math.Round(Convert.ToDouble(dBandTotal), 0) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_InvalidSumInsuredExceedsBandTotal").ToString() & "');", True)
                Else
                    'IF THE INITAIL ALLOC VALIDATION PASSED, THEN COPY THE TEMP DATA
                    oXMLData = oTempXMLData
                    oXMLDoc.LoadXml(oXMLData)
                    'oReinsurance.UpdateRISumInsured(oXMLData, Convert.ToDouble(value), RIBANDID, CType(sender, TextBox).ToolTip, Session(CNRIArrangementkey), "NB", dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment, sTreatyCode:=sTreatyCode)
                    If bIsFACPremiumNonProportional Then
                        'oXMLDoc.LoadXml(oXMLData)
                        Dim sanitizedBandId As String = RIBANDID.Replace("'", "''")
                        Dim sanitizedToolTip As String = txtSumInsured.ToolTip.Replace("'", "''")
                        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@RIArrangementLineKey='" & sanitizedToolTip & "']")
                        If oNode IsNot Nothing AndAlso oNode.Attributes("Placement") IsNot Nothing AndAlso oNode.Attributes("Placement").Value = "FAC Prop" Then
                            Dim sFACPropPremiumPerc As String = ""
                            If oNode.Attributes("FACPropPremiumPerc") IsNot Nothing Then
                                sFACPropPremiumPerc = oNode.Attributes("FACPropPremiumPerc").Value
                            End If
                            If Not String.IsNullOrEmpty(sFACPropPremiumPerc) AndAlso Convert.ToDouble(sFACPropPremiumPerc) > 0 Then
                                oReinsurance.UpdateFACPropPremiumPercentage(oXMLData, Convert.ToDouble(sFACPropPremiumPerc), RIBANDID, txtSumInsured.ToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
                            End If
                        End If
                    End If
                End If
                oTempXMLData = Nothing
                Session(CNRIXMLData) = oXMLData
                ' Bug #39513: Recalculate tax for treaty nodes reset to 0
                CalculateObligatoryTax()
                oXMLData = Convert.ToString(Session(CNRIXMLData))
                Dim oXMLSource As New XmlDataSource

                oXMLSource.EnableCaching = False

                oXMLSource.Data = oXMLData
                oXMLSource.XPath = ".//RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow[@IsDeleted='False']"

                gvReinsurance.DataSource = oXMLSource
                gvReinsurance.DataBind()
                ApplyEditedRowHighlighting()
            Catch ex As Exception
                HandleAPIError(ex)
            End Try
        End Sub
        Protected Sub txtCommissionPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim value As String = CType(sender, TextBox).Text
            If InStr(1, value, "%") <> 0 Then
                value = value.Substring(0, value.Trim.Length - 1)
            End If
            If IsNumeric(value) Then
                Dim sNumber() As String = value.Split(New String() {"."}, StringSplitOptions.RemoveEmptyEntries)
                If sNumber.Length > 1 AndAlso sNumber(1).Length > 4 Then
                    value = sNumber(0) + "." + sNumber(1).Substring(0, 4)
                End If
            End If
            Dim oReinsurance As New Reinsurance.Reinsurance
            Dim RIBANDID As String = ddlReinsurance.SelectedValue.ToString.Trim
            Dim oXMLData As String = Convert.ToString(Session(CNRIXMLData))

            Dim dTaxPercentage As Double = 0
            Dim bIsPortfolioRIAmendment As Boolean = False
            Dim iRIArrangementLineKey As Integer = 0
            Integer.TryParse(CType(sender, TextBox).ToolTip, iRIArrangementLineKey)
            Dim sTreatyCode As String = ""
            If iRIArrangementLineKey = 0 Then
                sTreatyCode = GetTreatyCodeFromGridRow(oXMLData, Convert.ToString(Session(CNRIBandKey)).Trim(), CType(sender, TextBox))
            End If
            nRIVersionId = ViewState("RIVersionId")
            If Session(CNMode) = Mode.PortFolioTransferAmendment Then
                bIsPortfolioRIAmendment = True
            ElseIf Session(CNMode) = Mode.ClonedTransferAmendment And nRIVersionId > 1 Then
                bIsPortfolioRIAmendment = True
            End If

            Dim sToolTip As String = CType(sender, TextBox).ToolTip
            If String.IsNullOrEmpty(sToolTip) Then sToolTip = "0"

            'Dim iRIArrangementLineKey As Integer = 0
            'Integer.TryParse(sToolTip, iRIArrangementLineKey)
            'Dim sTreatyCode As String = ""
            'If iRIArrangementLineKey = 0 Then
            '    sTreatyCode = GetTreatyCodeFromGridRow(oXMLData, Convert.ToString(Session(CNRIBandKey)), CType(sender, TextBox))
            'End If

            'dTaxPercentage = GetTaxPercentage(100, 0, oXMLData, RIBANDID, sToolTip, sTreatyCode)
            Dim iTaxLineKey As Integer = 0 : Integer.TryParse(sToolTip, iTaxLineKey)
            dTaxPercentage = GetTaxPercentage(100, 0, oXMLData, RIBANDID, iTaxLineKey, sTreatyCode)

            If String.IsNullOrEmpty(value) Or (Not IsNumeric(value)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_InvalidCommissionPerc").ToString() & "');", True)
            Else
                If Convert.ToDouble(value) > 100 Or Convert.ToDouble(value) < 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_CommissionPercRange").ToString() & "');", True)
                Else
                    ' For manually added treaties, fetch and store CommissionTaxPerc before Recalculate runs
                    Dim bIsManuallyAdded As Boolean = IsManuallyAdded(oXMLData, Convert.ToString(Session(CNRIBandKey)).Trim(), sToolTip, sTreatyCode)
                    If bIsManuallyAdded Then
                        Dim oXMLDoc As New XmlDocument()
                        oXMLDoc.LoadXml(oXMLData)
                        Dim sanitizedBandKey As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
                        Dim oTreatyNode As XmlNode = Nothing
                        If iRIArrangementLineKey > 0 Then
                            oTreatyNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@RIArrangementLineKey='" & sToolTip & "']")
                        ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                            oTreatyNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "' and @IsDeleted='False']")
                        End If
                        If oTreatyNode IsNot Nothing Then
                            Dim dPremium As Double = 0
                            Double.TryParse(oTreatyNode.Attributes("Premium").Value, dPremium)
                            Dim dCommPerc As Double = Convert.ToDouble(value)
                            Dim dDerivedComm As Double = dPremium * dCommPerc / 100
                            If dDerivedComm > 0 Then
                                Dim sTreatyCodeNode As String = If(oTreatyNode.Attributes("TreatyCode") IsNot Nothing, oTreatyNode.Attributes("TreatyCode").Value, sTreatyCode)
                                Dim iTreatyID As Integer = 0
                                If oTreatyNode.Attributes("TreatyId") IsNot Nothing Then Integer.TryParse(oTreatyNode.Attributes("TreatyId").Value, iTreatyID)
                                Dim sLineType As String = If(oTreatyNode.Attributes("Type") IsNot Nothing, oTreatyNode.Attributes("Type").Value, "T")
                                oQuote = CType(Session(CNQuote), NexusProvider.Quote)
                                Dim oRITreatyDetailsWithTax As New NexusProvider.RITreatyPartyWithTax
                                oRITreatyDetailsWithTax.InsuranceFileID = oQuote.InsuranceFileKey
                                oRITreatyDetailsWithTax.Premium = Convert.ToDecimal(dPremium)
                                oRITreatyDetailsWithTax.PremiumTransType = IIf(sLineType = "T" OrElse sLineType = "TX", "TTRITP", "TTRIFP")
                                oRITreatyDetailsWithTax.RIArrangementLineID = iRIArrangementLineKey
                                oRITreatyDetailsWithTax.RiskID = oQuote.Risks(Session(CNCurrentRiskKey)).Key
                                oRITreatyDetailsWithTax.Commission = Convert.ToDecimal(dDerivedComm)
                                oRITreatyDetailsWithTax.CommissionTransType = IIf(sLineType = "T" OrElse sLineType = "TX", "TTRITC", "TTRIFC")
                                oRITreatyDetailsWithTax.TreatyCode = sTreatyCodeNode
                                oRITreatyDetailsWithTax.TreatyID = iTreatyID
                                oRITreatyDetailsWithTax.IgnoreDetails = True
                                oRITreatyDetailsWithTax.IgnoreTax = False
                                oWebService = New NexusProvider.ProviderManager().Provider
                                oWebService.GetRITreatyPartyDetailsWithTax(oRITreatyDetailsWithTax, "")
                                Dim dCommTaxAmt As Double = Convert.ToDouble(oRITreatyDetailsWithTax.CommissionTax)
                                If dDerivedComm <> 0 Then
                                    If oTreatyNode.Attributes("CommissionTaxPerc") Is Nothing Then
                                        Dim attr As XmlAttribute = oXMLDoc.CreateAttribute("CommissionTaxPerc")
                                        attr.Value = Format(dCommTaxAmt / dDerivedComm * 100, "0.0000")
                                        oTreatyNode.Attributes.Append(attr)
                                    Else
                                        oTreatyNode.Attributes("CommissionTaxPerc").Value = Format(dCommTaxAmt / dDerivedComm * 100, "0.0000")
                                    End If
                                    oXMLData = oXMLDoc.OuterXml
                                    Session(CNRIXMLData) = oXMLData
                                End If
                            End If
                        End If
                    End If
                    oReinsurance.UpdateComissionPercentage(oXMLData, Convert.ToDouble(value), RIBANDID, sToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment, sTreatyCode:=sTreatyCode, sOriginalXML:=Convert.ToString(Session(CNRIXMLDataOriginal)))
                    ' GAP 2: Mark row as edited only if value differs from original
                    Dim dCommValue As Double = 0
                    Double.TryParse(value, dCommValue)
                    Dim bCommDiffers As Boolean = True
                    If Not String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLDataOriginal))) Then
                        Dim oOrigDocComm As New XmlDocument()
                        oOrigDocComm.LoadXml(Convert.ToString(Session(CNRIXMLDataOriginal)))
                        Dim sBandKeyComm As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
                        Dim oOrigNodeComm As XmlNode = Nothing
                        If iRIArrangementLineKey > 0 Then
                            oOrigNodeComm = oOrigDocComm.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKeyComm & "']/ArrangementRow[@RIArrangementLineKey='" & sToolTip & "']")
                        ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                            oOrigNodeComm = oOrigDocComm.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKeyComm & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
                        End If
                        If oOrigNodeComm IsNot Nothing AndAlso oOrigNodeComm.Attributes("CommissionPerc") IsNot Nothing Then
                            Dim dOrigComm As Double = 0
                            Double.TryParse(oOrigNodeComm.Attributes("CommissionPerc").Value, dOrigComm)
                            If Math.Round(dCommValue, 4) = Math.Round(dOrigComm, 4) Then
                                bCommDiffers = False
                            End If
                        End If
                    End If
                    If bCommDiffers Then
                        MarkRowAsEdited(oXMLData, RIBANDID, sToolTip, sTreatyCode)
                        If Not IsFACLine(oXMLData, RIBANDID, sToolTip, sTreatyCode) Then
                            If BandHasEditedRows(oXMLData, RIBANDID) Then pnlOverrideReason.Visible = True
                        End If
                    Else
                        ClearRowEditedFlags(oXMLData, RIBANDID, sToolTip, sTreatyCode)
                        If Not BandHasEditedRows(oXMLData, RIBANDID) Then pnlOverrideReason.Visible = False
                    End If
                End If
            End If

            Session(CNRIXMLData) = oXMLData
            ' Bug #39513: Recalculate tax for treaty nodes reset to 0
            CalculateObligatoryTax()
            oXMLData = Convert.ToString(Session(CNRIXMLData))
            'Update the Grid with latest calculation
            Dim oXMLSource As New XmlDataSource

            oXMLSource.EnableCaching = False

            oXMLSource.Data = oXMLData
            oXMLSource.XPath = ".//RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow[@IsDeleted='False']"

            gvReinsurance.DataSource = oXMLSource
            gvReinsurance.DataBind()
            ApplyEditedRowHighlighting()
        End Sub

        Protected Sub txtUpperLimit_TextChanged(sender As Object, e As EventArgs)
            UpdateXOLLimit(sender, "LineLimit", "msg_InvalidUpperLimit", "msg_NegativeUpperLimit", True)
        End Sub

        Protected Sub txtLowerLimit_TextChanged(sender As Object, e As EventArgs)
            UpdateXOLLimit(sender, "LowerLimit", "msg_InvalidLowerLimit", "msg_NegativeLowerLimit", False)
        End Sub

        Private Sub UpdateXOLLimit(sender As Object, attributeName As String, invalidMsgKey As String, negativeMsgKey As String, validateUpperLimit As Boolean)
            Try
                Dim txtLimit As TextBox = CType(sender, TextBox)
                Dim value As String = txtLimit.Text.Trim().Replace(",", "").Replace("$", "").Replace(" ", "")

                If String.IsNullOrEmpty(value) OrElse Not IsNumeric(value) Then
                    ScriptManager.RegisterStartupScript(Page, GetType(Page), "invalidLimit", "alert('" & GetLocalResourceObject(invalidMsgKey).ToString() & "');", True)
                    Exit Sub
                End If

                Dim dLimit As Double = Convert.ToDouble(value)
                If dLimit < 0 Then
                    ScriptManager.RegisterStartupScript(Page, GetType(Page), "negativeLimit", "alert('" & GetLocalResourceObject(negativeMsgKey).ToString() & "');", True)
                    Exit Sub
                End If

                Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
                Dim sRIBANDID As String = Convert.ToString(Session(CNRIBandKey))
                Dim oXMLDoc As New XmlDocument()
                oXMLDoc.LoadXml(sXML)

                Dim gridRow As GridViewRow = CType(txtLimit.NamingContainer, GridViewRow)
                If gridRow Is Nothing Then Exit Sub

                Dim iRIArrangementLineKey As Integer = 0
                Dim sTreatyCode As String = ""
                Integer.TryParse(txtLimit.ToolTip, iRIArrangementLineKey)
                If iRIArrangementLineKey = 0 Then
                    sTreatyCode = GetTreatyCodeFromGridRow(sXML, sRIBANDID, txtLimit)
                End If

                Dim oNode As XmlNode = Nothing
                If iRIArrangementLineKey > 0 Then
                    oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iRIArrangementLineKey & "']")
                ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                    oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
                End If

                If oNode Is Nothing Then Exit Sub

                'Dim bIsManuallyAdded As Boolean = False
                'If oNode.Attributes("ManuallyAdded") IsNot Nothing Then
                '    Boolean.TryParse(oNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
                'End If

                'If Not bIsManuallyAdded Then Exit Sub

                Dim sTreatyName As String = If(oNode.Attributes("Name") IsNot Nothing, oNode.Attributes("Name").Value, "XOL Treaty")
                Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
                Dim dGrossSI As Double = 0
                If oBandNode IsNot Nothing Then
                    Double.TryParse(oBandNode.Attributes("SumInsured").Value, dGrossSI)
                End If

                Dim dFACSI As Double = 0
                Dim oFACNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Type='F' and @IsDeleted='False']")
                For Each oFACNode As XmlNode In oFACNodes
                    Dim dSI As Double = 0
                    Double.TryParse(oFACNode.Attributes("SumInsured").Value, dSI)
                    dFACSI += dSI
                Next

                Dim dMaxAllowedUpperLimit As Double = dGrossSI - dFACSI

                Dim dCurrentLower As Double = 0
                Dim dCurrentUpper As Double = 0
                Dim txtLowerLimit As TextBox = gridRow.FindControl("txtLowerLimit")
                Dim txtUpperLimit As TextBox = gridRow.FindControl("txtUpperLimit")
                If txtLowerLimit IsNot Nothing AndAlso Not String.IsNullOrEmpty(txtLowerLimit.Text) Then
                    Dim lowerValue As String = txtLowerLimit.Text.Trim().Replace(",", "").Replace("$", "").Replace(" ", "")
                    Double.TryParse(lowerValue, dCurrentLower)
                End If
                If txtUpperLimit IsNot Nothing AndAlso Not String.IsNullOrEmpty(txtUpperLimit.Text) Then
                    Dim upperValue As String = txtUpperLimit.Text.Trim().Replace(",", "").Replace("$", "").Replace(" ", "")
                    Double.TryParse(upperValue, dCurrentUpper)
                End If

                If validateUpperLimit Then
                    If dLimit <= 0 Then
                        ScriptManager.RegisterStartupScript(Page, GetType(Page), "upperLimitZero", "alert('Upper Limit must be greater than 0.');", True)
                        Exit Sub
                    End If
                    If dLimit > dMaxAllowedUpperLimit Then
                        Dim sMessage As String = String.Format(GetLocalResourceObject("msg_UpperLimitExceedsGrossLessFAC").ToString(), sTreatyName, dMaxAllowedUpperLimit.ToString("N2"))
                        ScriptManager.RegisterStartupScript(Page, GetType(Page), "upperLimitExceeds", "alert('" & sMessage.Replace("'", "\'") & "');", True)
                        Exit Sub
                    End If
                    If dCurrentLower > 0 AndAlso dLimit <= dCurrentLower Then
                        ScriptManager.RegisterStartupScript(Page, GetType(Page), "upperLowerMismatch", "alert('Upper Limit must be greater than Lower Limit.');", True)
                        Exit Sub
                    End If
                    If dCurrentLower < 0 Then
                        ScriptManager.RegisterStartupScript(Page, GetType(Page), "lowerLimitMin", "alert('" & GetLocalResourceObject("msg_LowerLimitMinValue").ToString() & "');", True)
                        Exit Sub
                    End If
                Else
                    If dLimit < 0 Then
                        ScriptManager.RegisterStartupScript(Page, GetType(Page), "lowerLimitMin", "alert('" & GetLocalResourceObject("msg_LowerLimitMinValue").ToString() & "');", True)
                        Exit Sub
                    End If
                    If dCurrentUpper > 0 AndAlso dLimit >= dCurrentUpper Then
                        ScriptManager.RegisterStartupScript(Page, GetType(Page), "lowerUpperMismatch", "alert('Lower Limit must be less than Upper Limit.');", True)
                        Exit Sub
                    End If
                End If

                If oNode.Attributes("LowerLimit") Is Nothing Then
                    Dim attrLower As XmlAttribute = oXMLDoc.CreateAttribute("LowerLimit")
                    attrLower.Value = dCurrentLower.ToString()
                    oNode.Attributes.Append(attrLower)
                Else
                    oNode.Attributes("LowerLimit").Value = dCurrentLower.ToString()
                End If
                If oNode.Attributes("LineLimit") Is Nothing Then
                    Dim attrUpper As XmlAttribute = oXMLDoc.CreateAttribute("LineLimit")
                    attrUpper.Value = dCurrentUpper.ToString()
                    oNode.Attributes.Append(attrUpper)
                Else
                    oNode.Attributes("LineLimit").Value = dCurrentUpper.ToString()
                End If
                oNode.Attributes(attributeName).Value = dLimit.ToString()

                Session(CNRIXMLData) = oXMLDoc.OuterXml

                ' For manually added TX treaties, SumInsured is auto-calculated from limits — trigger recalculate
                If validateUpperLimit Then
                    Dim bIsManuallyAdded As Boolean = False
                    If oNode.Attributes("ManuallyAdded") IsNot Nothing Then
                        Boolean.TryParse(oNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
                    End If
                    Dim sTreatyType As String = If(oNode.Attributes("Type") IsNot Nothing, oNode.Attributes("Type").Value, "")
                    If bIsManuallyAdded AndAlso sTreatyType = "TX" Then
                        PopulateGrid()
                    End If
                End If
                PopulateGrid()
            Catch ex As Exception
                HandleAPIError(ex)
            End Try
        End Sub


        Protected Sub txtAgreement_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim value As String = CType(sender, TextBox).Text
            Dim oReinsurance As New Reinsurance.Reinsurance
            Dim RIBANDID As String = ddlReinsurance.SelectedValue.ToString.Trim
            Dim oXMLData As String = Convert.ToString(Session(CNRIXMLData))
            Dim iRIArrangementLineKey As Integer = 0
            Integer.TryParse(CType(sender, TextBox).ToolTip, iRIArrangementLineKey)
            Dim sTreatyCode As String = ""
            If iRIArrangementLineKey = 0 Then
                sTreatyCode = GetTreatyCodeFromGridRow(oXMLData, Convert.ToString(Session(CNRIBandKey)).Trim(), CType(sender, TextBox))
            End If

            oReinsurance.UpdateAgreement(oXMLData, value, RIBANDID, CType(sender, TextBox).ToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), sTreatyCode, Convert.ToString(Session(CNRIXMLDataOriginal)))
            ' GAP 3: Mark row as edited only if value differs from original
            Dim bAgreementDiffers As Boolean = True
            If Not String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLDataOriginal))) Then
                Dim oOrigDocAgr As New XmlDocument()
                oOrigDocAgr.LoadXml(Convert.ToString(Session(CNRIXMLDataOriginal)))
                Dim sBandKeyAgr As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
                Dim oOrigNodeAgr As XmlNode = Nothing
                If iRIArrangementLineKey > 0 Then
                    oOrigNodeAgr = oOrigDocAgr.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKeyAgr & "']/ArrangementRow[@RIArrangementLineKey='" & CType(sender, TextBox).ToolTip & "']")
                ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                    oOrigNodeAgr = oOrigDocAgr.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKeyAgr & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
                End If
                If oOrigNodeAgr IsNot Nothing AndAlso oOrigNodeAgr.Attributes("Agreement") IsNot Nothing Then
                    If String.Equals(value.Trim(), oOrigNodeAgr.Attributes("Agreement").Value.Trim(), StringComparison.OrdinalIgnoreCase) Then
                        bAgreementDiffers = False
                    End If
                End If
            End If
            If bAgreementDiffers Then
                MarkRowAsEdited(oXMLData, RIBANDID, CType(sender, TextBox).ToolTip, sTreatyCode)
                If Not IsFACLine(oXMLData, RIBANDID, CType(sender, TextBox).ToolTip, sTreatyCode) Then
                    If BandHasEditedRows(oXMLData, RIBANDID) Then pnlOverrideReason.Visible = True
                End If
            Else
                ClearRowEditedFlags(oXMLData, RIBANDID, CType(sender, TextBox).ToolTip, sTreatyCode)
                If Not BandHasEditedRows(oXMLData, RIBANDID) Then pnlOverrideReason.Visible = False
            End If
            Session(CNRIXMLData) = oXMLData
            Dim oXMLSource As New XmlDataSource
            oXMLSource.EnableCaching = False
            oXMLSource.Data = oXMLData
            oXMLSource.XPath = ".//RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow[@IsDeleted='False']"
            gvReinsurance.DataSource = oXMLSource
            gvReinsurance.DataBind()
            ApplyEditedRowHighlighting()
        End Sub

        Function GetRIArrangementLineFromXML(ByVal v_iRIArrangementLineKey As Integer, ByVal v_sXML As String, Optional ByVal sMode As String = "", Optional ByVal sTreatyCode As String = "") As ArrangementLinesType
            'Variable to Hold the returned data
            Dim oArrangementLineType As New ArrangementLinesType
            'Load the XML Data 
            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(v_sXML)

            Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']")

            If oNode IsNot Nothing Then
                'Populate Arrangement Line Details
                With oArrangementLineType

                    .ActionType = oNode.Attributes("ActionType").Value
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    .CedePremiumOnly = oNode.Attributes("CedePremiumOnly").Value
                    Decimal.TryParse(oNode.Attributes("CommissionPerc").Value, .CommissionPerc)
                    Integer.TryParse(oNode.Attributes("Grouping").Value, .Grouping)
                    Boolean.TryParse(oNode.Attributes("IsCommissionModified").Value, .IsCommissionModified)
                    Boolean.TryParse(oNode.Attributes("IsDomiciledForTax").Value, .IsDomiciledForTax)
                    Boolean.TryParse(oNode.Attributes("IsRIBroker").Value, .IsRIBroker)
                    Decimal.TryParse(oNode.Attributes("LineLimit").Value, .LineLimit)
                    Decimal.TryParse(oNode.Attributes("LowerLimit").Value, .LowerLimit)
                    Integer.TryParse(oNode.Attributes("NumberOfLines").Value, .NumberOfLines)
                    Integer.TryParse(oNode.Attributes("PartyKey").Value, .PartyKey)
                    Decimal.TryParse(oNode.Attributes("PremiumPercent").Value, .PremiumPercent)
                    Integer.TryParse(oNode.Attributes("Priority").Value, .Priority)
                    .ReinsuranceTypeCode = oNode.Attributes("ReinsuranceTypeCode").Value
                    Decimal.TryParse(oNode.Attributes("Retained").Value, .Retained)
                    Integer.TryParse(oNode.Attributes("RIarrangementKey").Value, .RIarrangementKey)
                    Integer.TryParse(oNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)
                    .RIPlacement = oNode.Attributes("Placement").Value
                    .TreatyCode = oNode.Attributes("TreatyCode").Value
                    .Type = oNode.Attributes("Type").Value
                    .RIName = oNode.Attributes("Name").Value
                    Decimal.TryParse(oNode.Attributes("DefaultPerc").Value, .DefaultPerc)
                    Decimal.TryParse(oNode.Attributes("ThisPerc").Value, .ThisPerc)
                    Decimal.TryParse(oNode.Attributes("SumInsured").Value, .SumInsured)
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    Decimal.TryParse(oNode.Attributes("ParticipationPercent").Value, .ParticipationPercent)
                    Decimal.TryParse(oNode.Attributes("Premium").Value, .PremiumValue)
                    Decimal.TryParse(oNode.Attributes("Tax").Value, .Tax)
                    Decimal.TryParse(oNode.Attributes("Commission").Value, .CommissionValue)
                    Decimal.TryParse(oNode.Attributes("CommissionTax").Value, .CommissionTax)
                    If oNode.Attributes("FACPropPremiumPerc") IsNot Nothing Then
                        Decimal.TryParse(oNode.Attributes("FACPropPremiumPerc").Value, .FACPropPremiumPerc)
                    End If

                    'Populate Broker Details
                    If sMode.Trim <> "FACXOL" Then
                        Dim oBrokerPart As BrokerParticipants
                        Dim oBrokerNodeList As XmlNodeList = oNode.SelectNodes("//BrokerParticipentRow")
                        If oBrokerNodeList IsNot Nothing AndAlso oBrokerNodeList.Count > 0 Then
                            For Each oBrokerPartNode As XmlNode In oBrokerNodeList
                                oBrokerPart = New BrokerParticipants
                                With oBrokerPart
                                    .PartyCode = oBrokerPartNode.Attributes("PartyCode").Value
                                    .PartyName = oBrokerPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                End With
                                oArrangementLineType.BrokerParticipants.Add(oBrokerPart)
                            Next
                        End If
                    Else
                        'Populate FAX Participents
                        Dim oFAXPart As FAXParticipants
                        Dim oFAXNodeList As XmlNodeList = oNode.SelectNodes("//FAXParticipentRow")
                        If oFAXNodeList IsNot Nothing AndAlso oFAXNodeList.Count > 0 Then
                            For Each oFAXPartNode As XmlNode In oFAXNodeList
                                oFAXPart = New FAXParticipants
                                With oFAXPart

                                    .PartyCode = oFAXPartNode.Attributes("PartyCode").Value
                                    .PartyName = oFAXPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oFAXPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    If oFAXPartNode.Attributes("ParticipationPercentage") IsNot Nothing Then
                                        Decimal.TryParse(oFAXPartNode.Attributes("ParticipationPercentage").Value, .ParticipationPercentage)
                                    Else
                                        Decimal.TryParse(oFAXPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                    End If
                                    .AccountType = oFAXPartNode.Attributes("AccountType").Value
                                    .AgreementCode = oFAXPartNode.Attributes("AgreementCode").Value
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionPercent").Value, .CommissionPercent)
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionTax").Value, .CommissionTax)
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionValue").Value, .CommissionValue)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PremiumTax").Value, .PremiumTax)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PremiumValue").Value, .PremiumValue)
                                    Decimal.TryParse(oFAXPartNode.Attributes("SumInsured").Value, .SumInsured)
                                    Integer.TryParse(oFAXPartNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)

                                    'Populate FAX Broker Part
                                    Dim oFAXBrokerPart As BrokerParticipants
                                    Dim oFAXBrokerNodeList As XmlNodeList = oNode.SelectNodes("//FAXBrokerParticipentRow")
                                    If oFAXBrokerNodeList IsNot Nothing AndAlso oFAXBrokerNodeList.Count > 0 Then
                                        For Each oFAXBrokerPartNode As XmlNode In oFAXBrokerNodeList
                                            oFAXBrokerPart = New BrokerParticipants

                                            With oFAXBrokerPart
                                                .PartyCode = oFAXBrokerPartNode.Attributes("PartyCode").Value
                                                .PartyName = oFAXBrokerPartNode.Attributes("PartyName").Value
                                                Integer.TryParse(oFAXBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                                Decimal.TryParse(oFAXBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                            End With
                                            oFAXPart.BrokerParticipants.Add(oFAXBrokerPart)
                                        Next
                                    End If
                                End With
                                oArrangementLineType.FAXParticipants.Add(oFAXPart)
                            Next
                        End If
                    End If
                End With
            End If

            'Return the Data
            Return oArrangementLineType
        End Function

        ''' <summary>
        ''' This method validate the RIArrangement lines present in XML
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function ValidateXML() As Boolean

            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(Session(CNRIXMLData))
            Dim iCount, iBandCnt As Integer
            Dim oXMLNode As XmlNode
            Dim iBandKey As Integer = 0

            Dim oTotalBandNodes As XmlNodeList
            oTotalBandNodes = oXMLDoc.SelectNodes("/rows/RIBAND")
            If Session(CNRIBandKey) Is Nothing Or String.IsNullOrEmpty(Session(CNRIBandKey)) Or Convert.ToString(Session(CNRIBandKey)).Trim = "0" Then
                Session(CNRIBandKey) = "0"
            End If
            Integer.TryParse(Session(CNRIBandKey), iBandKey)
            Dim iTotalBandKey As Integer = ddlReinsurance.Items.Count

            For iBandCnt = 0 To iTotalBandKey - 1
                Dim dFACTotal As Double = 0
                Dim dFXTotal As Double = 0
                Dim dBandTotal As Double = 0
                Session(CNRIBandKey) = ddlReinsurance.Items(iBandCnt).Value.Trim
                'Check for multiple existance of one reinsurer 
                iCount = 0
                Dim sanitizedBandKey As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
                Dim oFNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Type='F']")
                For Each oFNode As XmlNode In oFNodes
                    If oFNode.Attributes("IsDeleted").Value = "False" Then
                        If Convert.ToDouble(oFNode.Attributes("ThisPerc").Value) = 0 Then
                            Dim sMessage As String
                            sMessage = GetLocalResourceObject("RIInVldcedrate_Err")
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowMsg",
                                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>", False)
                            Session(CNRIBandKey) = iBandKey
                            Return False
                            Exit Function

                        End If
                        If CheckCollectionOfPartcipantsInXML(oFNode.Attributes("Name").Value, Session(CNRIXMLData), iCount) = True Then
                            If iCount > 1 Then
                                Dim sMessage As String
                                sMessage = GetLocalResourceObject("RICodeExist_Err")
                                sMessage = sMessage.Replace("#Code", oFNode.Attributes("Name").Value)
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowMsg",
                                                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>", False)
                                Session(CNRIBandKey) = iBandKey
                                Return False
                                Exit Function

                            End If
                        End If
                        If oFNode.Attributes("SumInsured").Value <> "" Then
                            dFACTotal += Convert.ToDouble(oFNode.Attributes("SumInsured").Value)
                        End If
                    End If
                Next
                iCount = 0
                Dim oFXNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Type='FX']")
                For Each oFXNode As XmlNode In oFXNodes
                    If oFXNode.Attributes("IsDeleted").Value = "False" Then
                        If CheckCollectionOfPartcipantsInXML(oFXNode.Attributes("Name").Value, Session(CNRIXMLData), iCount) = True Then
                            If iCount > 1 Then
                                Dim sMessage As String
                                sMessage = GetLocalResourceObject("RICodeExist_Err")
                                sMessage = sMessage.Replace("#Code", oFXNode.Attributes("Name").Value)
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowMsg",
                                                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>", False)
                                Session(CNRIBandKey) = iBandKey
                                Return False
                                Exit Function
                            End If
                        End If
                        If oFXNode.Attributes("SumInsured").Value <> "" Then
                            dFXTotal += Convert.ToDouble(oFXNode.Attributes("SumInsured").Value)
                        End If
                    End If
                Next
                Dim oBXMLNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Name='Band Total']")
                If oBXMLNode IsNot Nothing Then
                    If oBXMLNode.Attributes("SumInsured").Value <> "" Then
                        dBandTotal = Convert.ToDouble(oBXMLNode.Attributes("SumInsured").Value)
                    End If
                End If
                Dim oFXXMLNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Type='FX']")
                For Each oXMLNode In oFXXMLNodeList
                    If oXMLNode.Attributes("IsDeleted").Value = "False" Then
                        If oXMLNode.Attributes("LineLimit").Value <> "" Then
                            If Convert.ToDouble(oXMLNode.Attributes("LineLimit").Value) > (dBandTotal - dFACTotal) Then
                                Dim sMessage As String
                                sMessage = GetLocalResourceObject("RIFACXOLUL_Err")
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowMsg",
                                                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>", False)
                                Session(CNRIBandKey) = iBandKey
                                Return False
                                Exit Function
                            End If
                        End If
                    End If
                Next
                'Check manually added treaties - type-specific validation (PBI 39760)
                Dim oManualNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@ManuallyAdded='True' and @IsDeleted='False']")
                If Session(CNMTAType) <> MTAType.CANCELLATION Then
                    For Each oManualNode As XmlNode In oManualNodes
                        Dim dManualSI As Double = 0
                        Dim dManualPremium As Double = 0
                        Dim dManualLowerLimit As Double = 0
                        Dim dManualUpperLimit As Double = 0
                        Double.TryParse(oManualNode.Attributes("SumInsured").Value, dManualSI)
                        Double.TryParse(oManualNode.Attributes("Premium").Value, dManualPremium)
                        If oManualNode.Attributes("LowerLimit") IsNot Nothing Then Double.TryParse(oManualNode.Attributes("LowerLimit").Value, dManualLowerLimit)
                        If oManualNode.Attributes("LineLimit") IsNot Nothing Then Double.TryParse(oManualNode.Attributes("LineLimit").Value, dManualUpperLimit)
                        Dim sManualType As String = If(oManualNode.Attributes("Type") IsNot Nothing, oManualNode.Attributes("Type").Value, "T")
                        If sManualType = "TX" Then
                            ' XOL treaty: Upper Limit and Premium must be > 0 (Lower Limit can be 0 for ground-up layers)
                            If dManualLowerLimit < 0 OrElse dManualUpperLimit <= 0 OrElse dManualPremium <= 0 Then
                                Dim sMessage As String = GetLocalResourceObject("msg_ManualXOLTreatyInvalidFields_Err").ToString()
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowMsgManualXOLTreaty",
                                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>", False)
                                Session(CNRIBandKey) = iBandKey
                                Return False
                                Exit Function
                            End If
                        Else
                            ' Proportional treaty (T, TFS): Sum Insured and Premium must be > 0
                            If dManualSI <= 0 OrElse dManualPremium <= 0 Then
                                Dim sMessage As String = GetLocalResourceObject("RIManualTreatyInvalidSIPremium_Err").ToString()
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowMsgManualPropTreaty",
                                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>", False)
                                Session(CNRIBandKey) = iBandKey
                                Return False
                                Exit Function
                            End If
                        End If
                    Next
                End If ' CNMTAType <> CANCELLATION

                'Check Unallocated amoumt
                Dim oUXMLNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Name='Unallocated']")
                Dim dUAAmount As Double
                If oUXMLNode IsNot Nothing Then
                    Double.TryParse(oUXMLNode.Attributes("SumInsured").Value, dUAAmount)
                    If dUAAmount <> 0 Then
                        Dim sMessage As String
                        sMessage = GetLocalResourceObject("RISISHPerc_Err")
                        sMessage = sMessage.Replace("#Code", ddlReinsurance.Items(iBandCnt).Text)
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowMsg",
                                                           "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>", False)
                        Session(CNRIBandKey) = iBandKey
                        Return False
                        Exit Function
                    End If
                    Double.TryParse(oUXMLNode.Attributes("Premium").Value, dUAAmount)
                    If dUAAmount <> 0 Then
                        Dim sMessage As String
                        sMessage = GetLocalResourceObject("RIPSHPerc_Err")
                        sMessage = sMessage.Replace("#Code", ddlReinsurance.Items(iBandCnt).Text)
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowMsg",
                                                           "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>", False)
                        Session(CNRIBandKey) = iBandKey
                        Return False
                        Exit Function
                    End If
                End If
            Next
            Session(CNRIBandKey) = iBandKey
            Return True
        End Function

        ''' <summary>
        ''' This method Updates the RIArrangement lines
        ''' </summary>
        ''' <remarks></remarks>
        Sub UpdateRIArrangement()
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oXMLData As String = Convert.ToString(Session(CNRIXMLData))
            Dim RIBANDID As String = "Current_" & ddlReinsurance.SelectedValue
            Dim oArrangementLine As NexusProvider.ArrangementLinesType
            Dim iRIArrangementLineKey As Integer

            If String.IsNullOrEmpty(oXMLData) = False Then
                Dim oXMLDoc As New XmlDocument
                oXMLDoc.LoadXml(oXMLData)
                Dim iTotalBandKey As Integer = ddlReinsurance.Items.Count
                For iBandCnt As Integer = 0 To iTotalBandKey - 1
                    Dim dTotalSI As Double = 0
                    RIBANDID = "Current_" & ddlReinsurance.Items(iBandCnt).Value.Trim
                    Dim xmlNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & RIBANDID & "']/ArrangementRow")
                    Dim oArrangementLineColl As New NexusProvider.ArrangementLinesTypeCollection
                    If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then
                        For Each oArrangeLineNode As XmlNode In xmlNodes
                            oArrangementLine = New NexusProvider.ArrangementLinesType
                            If oArrangeLineNode.Attributes("Name").Value = "Band Total" Then
                                Double.TryParse(oArrangeLineNode.Attributes("SumInsured").Value, dTotalSI)
                            End If

                            Integer.TryParse(oArrangeLineNode.Attributes("RIArrangementLineKey").Value, iRIArrangementLineKey)
                            Dim bIsManuallyAdded As Boolean = False
                            Dim streatyNode As String = ""
                            If oArrangeLineNode.Attributes("ManuallyAdded") IsNot Nothing Then
                                Boolean.TryParse(oArrangeLineNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
                            End If
                            If oArrangeLineNode.Attributes("TreatyCode") IsNot Nothing Then
                                streatyNode = oArrangeLineNode.Attributes("TreatyCode").Value
                            End If

                            'Only send lines that have been modified (ActionType 1=update, 2=delete) or are manually added
                            'Dim sActionType As String = If(oArrangeLineNode.Attributes("ActionType") IsNot Nothing, oArrangeLineNode.Attributes("ActionType").Value, "0")
                            'Dim bHasAction As Boolean = (sActionType = "1" OrElse sActionType = "2")
                            Dim bIsNew As Boolean = False
                            Dim bIsDeleted As Boolean = False
                            If oArrangeLineNode.Attributes("IsNew") IsNot Nothing Then Boolean.TryParse(oArrangeLineNode.Attributes("IsNew").Value, bIsNew)
                            If oArrangeLineNode.Attributes("IsDeleted") IsNot Nothing Then Boolean.TryParse(oArrangeLineNode.Attributes("IsDeleted").Value, bIsDeleted)

                            If ((iRIArrangementLineKey <> 0) Or (bIsManuallyAdded)) AndAlso Not (bIsNew AndAlso bIsDeleted) Then 'AndAlso (bHasAction OrElse bIsManuallyAdded) Then
                                oArrangementLine = oWebService.GetRIArrangementLineFromXML(iRIArrangementLineKey, oXMLData, False, RIBANDID, streatyNode)
                                oArrangementLineColl.Add(oArrangementLine)
                            End If

                        Next
                    End If
                    If oArrangementLineColl.Count > 0 Then
                        oWebService.UpdateArrangementLinesRI2007(oArrangementLineColl, v_dTotalSI:=dTotalSI)
                    End If

                    Dim sOverrideReasonId As String = GetBandOverrideReasonId(oXMLData, ddlReinsurance.Items(iBandCnt).Value.Trim)
                    If Not String.IsNullOrEmpty(sOverrideReasonId) AndAlso sOverrideReasonId <> "0" Then
                        Dim iArrangementId As Integer = GetBandArrangementId(oXMLData, ddlReinsurance.Items(iBandCnt).Value.Trim)
                        If iArrangementId > 0 Then
                            oWebService.UpdateRIArrangementOverrideReason(iArrangementId, Convert.ToInt32(sOverrideReasonId))
                        End If
                    End If
                Next
            End If
        End Sub
        ''' <summary>
        ''' This method checks FAC XOL Validation
        ''' </summary>
        ''' <param name="v_sXML"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckFACXOLLimit(ByVal v_sXML As String) As Boolean
            Dim bValidation As Boolean = True
            'Load the XML Data 
            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(v_sXML)

            Dim oRIBandList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND")

            If oRIBandList IsNot Nothing Then
                For Each oBand As XmlNode In oRIBandList

                    If oBand.Attributes("Name") IsNot Nothing AndAlso oBand.Attributes("Name").Value.Contains("Current") = True Then

                        'Find the Band Total
                        'Calculate/Retreive Band Total
                        Dim dBANDPremium As Decimal = 0
                        Dim oBandNode As XmlNode = oBand.SelectSingleNode("/rows/RIBAND[@Name='" & oBand.Attributes("Name").Value & "']/ArrangementRow[@Name='Band Total']")
                        If oBandNode IsNot Nothing Then
                            Decimal.TryParse(oBandNode.Attributes("Premium").Value, dBANDPremium)
                        End If

                        'Find the FAC XOL
                        Dim dTotFACXOLPremium As Decimal = 0
                        Dim FACXOLNodes As XmlNodeList = oBand.SelectNodes("/rows/RIBAND[@Name='" & oBand.Attributes("Name").Value & "']/ArrangementRow[@Placement='FAC XOL' and @IsDeleted='False']")

                        For Each oNode As XmlNode In FACXOLNodes
                            Dim dFACXOLPremium As Decimal
                            Decimal.TryParse(oNode.Attributes("Premium").Value, dFACXOLPremium)
                            dTotFACXOLPremium = dTotFACXOLPremium + dFACXOLPremium
                        Next

                        'if Total FAC XOL premium is greater than BAND premium
                        If dTotFACXOLPremium > dBANDPremium AndAlso dTotFACXOLPremium <> 0 Then
                            bValidation = False
                            Exit For
                        End If
                    End If
                Next
            End If

            Return bValidation
        End Function

#End Region

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Session.Remove(CNRIArrangementkey)
            Session.Remove(CNRIBandKey)
            Session.Remove(CNRIFACProp)
            Session.Remove(CNRIFACXolTemp)
            Session.Remove(CNRIXMLData)
            If Session(CNMode) = Mode.PortFolioTransferAmendment Or Session(CNMode) = Mode.ClonedTransferAmendment Then
                Response.Redirect("~/secure/RIAmendRiskList.aspx", False)
            Else
                'Update the Status as PENDINGRI
                UpdateRiskStatus()
                Response.Redirect("~/secure/PremiumDisplay.aspx", False)
            End If
        End Sub

        Sub UpdateRiskStatus()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            If oQuote IsNot Nothing Then
                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.UpdateRiskStatus(oQuote.InsuranceFileKey, oQuote.Risks(Session(CNCurrentRiskKey)).Key, NexusProvider.RiskStatusType.PENDINGRI, oQuote.BranchCode)
            End If
        End Sub
        Function GetTotalPercentage(ByVal sXML As String, ByVal RIBANDTYPE As String) As Double
            Dim oXMLDoc As New XmlDocument
            Dim oNode As XmlNode
            Dim dPerc As Decimal
            Dim dTotPerc As Decimal
            oXMLDoc.LoadXml(sXML)
            Dim sanitizedBandKey As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
            Dim sanitizedBandType As String = RIBANDTYPE.Replace("'", "''")
            Dim oRIBandList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Type='" & sanitizedBandType & "']")
            If oRIBandList IsNot Nothing Then
                For Each oNode In oRIBandList
                    If oNode.Attributes("IsNew").Value.ToUpper.Trim = "TRUE" Or (oNode.Attributes("IsDeleted").Value.ToUpper.Trim = "FALSE") Then
                        'Find FAC Percentage
                        'Calculate/Retreive FACPerc
                        Decimal.TryParse(oNode.Attributes("ThisPerc").Value, dPerc)
                        dTotPerc = dTotPerc + dPerc
                    End If
                Next
            End If
            Return dTotPerc
        End Function
        Function GetTotalRISum(ByVal sXML As String, ByVal RIBANDTYPE As String) As Double
            Dim oXMLDoc As New XmlDocument
            Dim dTotRISum As Decimal = 0
            oXMLDoc.LoadXml(sXML)
            Dim sanitizedBandKey As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
            Dim sanitizedBandType As String = RIBANDTYPE.Replace("'", "''")

            Dim xpath As String = "/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Type='" & sanitizedBandType & "'"
            If RIBANDTYPE = "T" OrElse RIBANDTYPE = "TX" Then
                xpath &= " and @ManuallyAdded='True' and @IsDeleted='False']"
            Else
                xpath &= "]"
            End If

            Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes(xpath)
            If oNodes IsNot Nothing Then
                For Each oNode As XmlNode In oNodes
                    If RIBANDTYPE = "T" OrElse RIBANDTYPE = "TX" Then
                        Dim dValue As Decimal = 0
                        Decimal.TryParse(oNode.Attributes("SumInsured").Value, dValue)
                        dTotRISum += dValue
                    ElseIf oNode.Attributes("IsNew").Value.ToUpper.Trim = "TRUE" And oNode.Attributes("IsDeleted").Value.ToUpper.Trim = "FALSE" Then
                        Dim dValue As Decimal = 0
                        Decimal.TryParse(oNode.Attributes("SumInsured").Value, dValue)
                        dTotRISum += dValue
                    End If
                Next
            End If

            Return dTotRISum
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlRIVersion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRIVersion.SelectedIndexChanged
            ' Obtaining the value of Reinsurance bands from SAM
            Dim oReinsurarerBandCollection As NexusProvider.ReinsuranceBandsCollection
            Dim oRIVersions As NexusProvider.LookupListCollection = ViewState("RIVersions")
            lblVersionEffectiveDate.Text = oRIVersions(ddlRIVersion.SelectedIndex).EffectiveDate
            'Clear existing bands
            ddlReinsurance.Items.Clear()
            ' Pass the RiskKey to get the Reinsurance band values for the Current risk key else 
            If Session(CNCurrentRiskKey) Is Nothing Then
                oReinsurarerBandCollection = oWebService.GetRiskReinsuranceBands(CType(Session(CNQuote), NexusProvider.Quote).Risks(0).Key, ddlRIVersion.SelectedValue)
            Else
                oReinsurarerBandCollection = oWebService.GetRiskReinsuranceBands(CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key, ddlRIVersion.SelectedValue)
            End If

            ' Adding value from the collection to the Dropdown
            For Each oReinsurarerBand As NexusProvider.ReinsuranceBands In oReinsurarerBandCollection
                ddlReinsurance.Items.Add(New ListItem(oReinsurarerBand.Band, oReinsurarerBand.BandKey))
            Next
            'Select the first band
            If ddlReinsurance.Items.Count > 0 Then
                If Session(CNRIBandKey) IsNot Nothing Then
                    ddlReinsurance.SelectedValue = Convert.ToString(Session(CNRIBandKey)).Trim
                Else
                    ddlReinsurance.SelectedIndex = 0
                End If
            End If

            Session.Remove(CNRIArrangementkey)
            Session.Remove(CNRIFACProp)
            Session.Remove(CNRIFACXol)

            RIModelSummaryCntrl.PopulateModelSummary()

            ' Populate the Grid    
            PopulateGrid(ddlRIVersion.SelectedValue)

        End Sub

        ''' <summary>
        ''' Check if FAC Premium is Non-Proportional from RI Model Maintenance
        ''' </summary>
        Private Sub CheckFACPremiumNonProportional()
            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                oQuote = Session(CNQuote)
                Dim nRiskKey As Integer = oQuote.Risks(Session(CNCurrentRiskKey)).Key
                Dim oArrangementTypeCol As NexusProvider.ArrangementsTypeCollection = oWebService.GetRiskReinsuranceArrangements(nRiskKey)

                ' Check if any arrangement has FAC Premium as Non-Proportional
                For Each oArrangementType As NexusProvider.ArrangementsType In oArrangementTypeCol
                    If Convert.ToBoolean(oArrangementType.FACPremiumType) Then
                        bIsFACPremiumNonProportional = True
                        Exit For
                    End If
                Next
            Catch ex As Exception
                ' Default to proportional if error occurs
                bIsFACPremiumNonProportional = False
            End Try
        End Sub

        ''' <summary>
        ''' Handle FAC Prop Premium Percentage change
        ''' </summary>
        Protected Sub txtFACPropPremiumPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            If Not bIsFACPremiumNonProportional Then
                Exit Sub
            End If

            Dim value As String = CType(sender, TextBox).Text
            If InStr(1, value, "%") <> 0 Then
                value = value.Substring(0, value.Trim.Length - 1)
            End If

            Dim oReinsurance As New Reinsurance.Reinsurance
            Dim RIBANDID As String = ddlReinsurance.SelectedValue.ToString.Trim
            Dim oXMLData As String = Convert.ToString(Session(CNRIXMLData))
            Dim oTempXMLData As String = Convert.ToString(Session(CNRIXMLData))
            Dim dTaxPercentage As Double = 0
            Dim dTotPercentage As Double = 0
            Dim bIsPortfolioRIAmendment As Boolean = False
            Dim txtFACPropPremiumPerc As TextBox = CType(sender, TextBox)

            nRIVersionId = ViewState("RIVersionId")
            If Session(CNMode) = Mode.PortFolioTransferAmendment Then
                bIsPortfolioRIAmendment = True
            ElseIf Session(CNMode) = Mode.ClonedTransferAmendment And nRIVersionId > 1 Then
                bIsPortfolioRIAmendment = True
            End If

            If String.IsNullOrEmpty(value) Or (Not IsNumeric(value)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_InvalidFACPropPremiumPerc").ToString() & "');", True)
                Exit Sub
            End If

            Dim dFACPropPremiumPerc As Double = Math.Round(Convert.ToDouble(value), 4)

            ' Validate total FAC Prop Premium % does not exceed 100%
            dTotPercentage = GetTotalFACPropPremiumPercentage(oTempXMLData, txtFACPropPremiumPerc.ToolTip, dFACPropPremiumPerc)

            If dFACPropPremiumPerc > 100 Or dFACPropPremiumPerc < 0 Or dTotPercentage > 100 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_FACPropPremiumPercExceeds100").ToString() & "');", True)
                Exit Sub
            End If

            ' Update FAC Prop Premium based on percentage
            dTaxPercentage = GetTaxPercentage(100, 0, oXMLData, RIBANDID, txtFACPropPremiumPerc.ToolTip)
            oReinsurance.UpdateFACPropPremiumPercentage(oXMLData, dFACPropPremiumPerc, RIBANDID, txtFACPropPremiumPerc.ToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)

            Session(CNRIXMLData) = oXMLData

            ' Update the Grid
            Dim oXMLSource As New XmlDataSource
            oXMLSource.EnableCaching = False
            oXMLSource.Data = oXMLData
            oXMLSource.XPath = ".//RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow[@IsDeleted='False']"
            gvReinsurance.DataSource = oXMLSource
            gvReinsurance.DataBind()
            ApplyEditedRowHighlighting()
        End Sub

        ''' <summary>
        ''' Handle FAC Prop Premium direct value change
        ''' </summary>
        Protected Sub txtPremium_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim txtPremium As TextBox = CType(sender, TextBox)
            Dim value As String = txtPremium.Text.Trim().Replace(",", "").Replace(" ", "")
            Dim oReinsurance As New Reinsurance.Reinsurance
            Dim RIBANDID As String = ddlReinsurance.SelectedValue.ToString.Trim
            Dim oXMLData As String = Convert.ToString(Session(CNRIXMLData))
            Dim oTempXMLData As String = Convert.ToString(Session(CNRIXMLData))
            Dim dTaxPercentage As Double = 0
            Dim bIsPortfolioRIAmendment As Boolean = False

            nRIVersionId = ViewState("RIVersionId")
            If Session(CNMode) = Mode.PortFolioTransferAmendment Then
                bIsPortfolioRIAmendment = True
            ElseIf Session(CNMode) = Mode.ClonedTransferAmendment And nRIVersionId > 1 Then
                bIsPortfolioRIAmendment = True
            End If

            If String.IsNullOrEmpty(value) Or (Not IsNumeric(value)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_InvalidPremiumValue").ToString() & "');", True)
                Exit Sub
            End If

            Dim dPremium As Double = Math.Round(Convert.ToDouble(value), 2)

            Dim iRIArrangementLineKey As Integer = 0
            Integer.TryParse(txtPremium.ToolTip, iRIArrangementLineKey)
            Dim sPremiumToolTip As String = If(String.IsNullOrEmpty(txtPremium.ToolTip), "0", txtPremium.ToolTip)
            Dim sTreatyCode As String = ""
            If iRIArrangementLineKey = 0 Then
                sTreatyCode = GetTreatyCodeFromGridRow(oXMLData, Convert.ToString(Session(CNRIBandKey)).Trim(), txtPremium)
            End If

            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(oXMLData)
            Dim sanitizedBandId As String = RIBANDID.Replace("'", "''")
            Dim sanitizedToolTip As String = txtPremium.ToolTip.Replace("'", "''")
            Dim oNode As XmlNode = Nothing
            If iRIArrangementLineKey > 0 Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@RIArrangementLineKey='" & sanitizedToolTip & "']")
            ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "' and @IsDeleted='False']")
            End If

            If oNode IsNot Nothing Then
                Dim bIsManuallyAdded As Boolean = False
                If oNode.Attributes("ManuallyAdded") IsNot Nothing Then
                    Boolean.TryParse(oNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
                End If

                If bIsManuallyAdded Then
                    ' Manually added: call GetTaxPercentage (SAM) for tax
                    dTaxPercentage = GetTaxPercentage(dPremium, 0, oXMLDoc.OuterXml, RIBANDID, sPremiumToolTip, sTreatyCode)
                    oNode.Attributes("Tax").Value = Format((dTaxPercentage * dPremium) / 100, "0.00")
                Else
                    ' Non-manual: preserve tax proportionally from existing Tax/Premium ratio
                    Dim dOldPremium As Double = 0
                    Dim dOldTax As Double = 0
                    Double.TryParse(oNode.Attributes("Premium").Value, dOldPremium)
                    If oNode.Attributes("Tax") IsNot Nothing Then Double.TryParse(oNode.Attributes("Tax").Value, dOldTax)
                    dTaxPercentage = If(dOldPremium <> 0, dOldTax * 100 / dOldPremium, 0)
                    oNode.Attributes("Tax").Value = Format((dTaxPercentage * dPremium) / 100, "0.00")
                    ' Preserve CommissionTax proportionally
                    Dim dOldComm As Double = 0
                    Dim dOldCommTax As Double = 0
                    If oNode.Attributes("Commission") IsNot Nothing Then Double.TryParse(oNode.Attributes("Commission").Value, dOldComm)
                    If oNode.Attributes("CommissionTax") IsNot Nothing Then Double.TryParse(oNode.Attributes("CommissionTax").Value, dOldCommTax)
                    Dim dCommTaxPerc As Double = If(dOldComm <> 0, dOldCommTax * 100 / dOldComm, 0)
                    Dim dCommPercVal As Double = 0
                    If oNode.Attributes("CommissionPerc") IsNot Nothing Then Double.TryParse(oNode.Attributes("CommissionPerc").Value, dCommPercVal)
                    Dim dNewComm As Double = dPremium * dCommPercVal / 100
                    oNode.Attributes("Commission").Value = Format(dNewComm, "0.00")
                    If oNode.Attributes("CommissionTax") IsNot Nothing Then
                        oNode.Attributes("CommissionTax").Value = Format(dNewComm * dCommTaxPerc / 100, "0.00")
                    End If
                End If

                If bIsManuallyAdded Then
                    Dim sTreatyType As String = If(oNode.Attributes("Type") IsNot Nothing, oNode.Attributes("Type").Value, "")
                    Dim sTreatyName As String = If(oNode.Attributes("Name") IsNot Nothing, oNode.Attributes("Name").Value, "Treaty")
                    If sTreatyType = "TX" Then
                        Dim dBandPremium As Double = GetBandTotalPremium(oXMLData, RIBANDID)
                        Dim dFACPremium As Double = 0
                        Dim oFACNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@Type='F' and @IsDeleted='False']")
                        For Each oFACNode As XmlNode In oFACNodes
                            Dim dPrem As Double = 0
                            Double.TryParse(oFACNode.Attributes("Premium").Value, dPrem)
                            dFACPremium += dPrem
                        Next
                        If dPremium > (dBandPremium - dFACPremium) Then
                            Dim sMessage As String = String.Format(GetLocalResourceObject("msg_XOLPremiumExceedsLimit").ToString(), sTreatyName, String.Format("{0:N2}", dBandPremium - dFACPremium))
                            ScriptManager.RegisterStartupScript(Page, GetType(Page), "xolPremiumExceeds", "alert('" & sMessage.Replace("'", "\'") & "');", True)
                            Exit Sub
                        End If
                    End If
                    oNode.Attributes("Premium").Value = dPremium.ToString()
                    If oNode.Attributes("IsPremiumEdited") Is Nothing Then
                        Dim attrPE As XmlAttribute = oXMLDoc.CreateAttribute("IsPremiumEdited")
                        attrPE.Value = "True"
                        oNode.Attributes.Append(attrPE)
                    Else
                        oNode.Attributes("IsPremiumEdited").Value = "True"
                    End If
                    oXMLData = oXMLDoc.OuterXml
                    dTaxPercentage = GetTaxPercentage(dPremium, 0, oXMLData, RIBANDID, sPremiumToolTip, sTreatyCode)
                    Dim dCommPerc As Double = 0
                    If oNode.Attributes("CommissionPerc") IsNot Nothing Then Double.TryParse(oNode.Attributes("CommissionPerc").Value, dCommPerc)
                    If dCommPerc = 0 Then dCommPerc = GetCommissionPercentage(oXMLData, RIBANDID, iRIArrangementLineKey, sTreatyCode)
                    oReinsurance.ApplyManualTreatyFinancials(oNode, dPremium, dTaxPercentage, dCommPerc)
                    oXMLData = oXMLDoc.OuterXml
                    Dim oReinsuranceCalc As New Reinsurance.Reinsurance
                    oReinsuranceCalc.Recalculate(oXMLData, RIBANDID, Session(CNRIArrangementkey), iRIArrangementLineKey.ToString(), Session(CNRITransactionType), False, dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
                    If dCommPerc > 0 Then
                        oReinsurance.UpdateComissionPercentage(oXMLData, dCommPerc, RIBANDID, sPremiumToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment, sTreatyCode:=sTreatyCode)
                    End If
                    ' PBI 35359 / Bug 39248: Mark manually added treaty as edited only if premium differs from original
                    Dim bPremDiffersManual As Boolean = True
                    If Not String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLDataOriginal))) Then
                        Dim oOrigDocPrem As New XmlDocument()
                        oOrigDocPrem.LoadXml(Convert.ToString(Session(CNRIXMLDataOriginal)))
                        Dim sBandKeyPrem As String = RIBANDID.Replace("'", "''")
                        Dim oOrigNodePrem As XmlNode = Nothing
                        If iRIArrangementLineKey > 0 Then
                            oOrigNodePrem = oOrigDocPrem.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKeyPrem & "']/ArrangementRow[@RIArrangementLineKey='" & sPremiumToolTip & "']")
                        ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                            oOrigNodePrem = oOrigDocPrem.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKeyPrem & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
                        End If
                        If oOrigNodePrem IsNot Nothing AndAlso oOrigNodePrem.Attributes("Premium") IsNot Nothing Then
                            Dim dOrigPrem As Double = 0
                            Double.TryParse(oOrigNodePrem.Attributes("Premium").Value, dOrigPrem)
                            If Math.Round(dPremium, 2) = Math.Round(dOrigPrem, 2) Then
                                bPremDiffersManual = False
                            End If
                        End If
                        If oOrigNodePrem IsNot Nothing AndAlso oOrigNodePrem.Attributes("IsEditedDB") IsNot Nothing AndAlso oOrigNodePrem.Attributes("IsEditedDB").Value.ToUpper() = "TRUE" Then
                            bPremDiffersManual = True
                        End If
                    End If
                    If bPremDiffersManual Then
                        MarkRowAsEdited(oXMLData, RIBANDID, sPremiumToolTip, sTreatyCode)
                        If BandHasEditedRows(oXMLData, RIBANDID) Then pnlOverrideReason.Visible = True
                    Else
                        ClearRowEditedFlags(oXMLData, RIBANDID, sPremiumToolTip, sTreatyCode)
                        If Not BandHasEditedRows(oXMLData, RIBANDID) Then pnlOverrideReason.Visible = False
                    End If
                ElseIf Not bIsManuallyAdded Then
                    ' Regular model-driven treaty with manual premium adjustment enabled
                    oNode.Attributes("Premium").Value = dPremium.ToString()
                    If oNode.Attributes("IsPremiumEdited") Is Nothing Then
                        Dim attrPE As XmlAttribute = oXMLDoc.CreateAttribute("IsPremiumEdited")
                        attrPE.Value = "True"
                        oNode.Attributes.Append(attrPE)
                    Else
                        oNode.Attributes("IsPremiumEdited").Value = "True"
                    End If
                    oXMLData = oXMLDoc.OuterXml
                    ' dTaxPercentage already derived proportionally in the Else branch above - do NOT call GetTaxPercentage again
                    Dim dCommPerc As Double = 0
                    If oNode.Attributes("CommissionPerc") IsNot Nothing Then Double.TryParse(oNode.Attributes("CommissionPerc").Value, dCommPerc)
                    If dCommPerc = 0 Then dCommPerc = GetCommissionPercentage(oXMLData, RIBANDID, iRIArrangementLineKey, sTreatyCode)
                    oReinsurance.ApplyManualTreatyFinancials(oNode, dPremium, dTaxPercentage, dCommPerc)
                    oXMLData = oXMLDoc.OuterXml
                    Dim oReinsuranceCalc As New Reinsurance.Reinsurance
                    oReinsuranceCalc.Recalculate(oXMLData, RIBANDID, Session(CNRIArrangementkey), iRIArrangementLineKey.ToString(), Session(CNRITransactionType), False, dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
                    ' Re-assert IsPremiumEdited and the user-entered premium after Recalculate
                    ' Recalculate may reset IsPremiumEdited=False (same path as UpdateRISumInsured)
                    Dim oReassertDoc As New XmlDocument()
                    oReassertDoc.LoadXml(oXMLData)
                    Dim oReassertNode As XmlNode = Nothing
                    If iRIArrangementLineKey > 0 Then
                        oReassertNode = oReassertDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@RIArrangementLineKey='" & sanitizedToolTip & "']")
                    ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                        oReassertNode = oReassertDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "' and @IsDeleted='False']")
                    End If
                    If oReassertNode IsNot Nothing Then
                        oReassertNode.Attributes("Premium").Value = dPremium.ToString()
                        If oReassertNode.Attributes("IsPremiumEdited") Is Nothing Then
                            Dim attrRE As XmlAttribute = oReassertDoc.CreateAttribute("IsPremiumEdited")
                            attrRE.Value = "True"
                            oReassertNode.Attributes.Append(attrRE)
                        Else
                            oReassertNode.Attributes("IsPremiumEdited").Value = "True"
                        End If
                    End If
                    oXMLData = oReassertDoc.OuterXml
                    ' Mark regular treaty as edited only if premium differs from original
                    Dim bPremDiffersRegular As Boolean = True
                    If Not String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLDataOriginal))) Then
                        Dim oOrigDocPrem2 As New XmlDocument()
                        oOrigDocPrem2.LoadXml(Convert.ToString(Session(CNRIXMLDataOriginal)))
                        Dim sBandKeyPrem2 As String = RIBANDID.Replace("'", "''")
                        Dim oOrigNodePrem2 As XmlNode = Nothing
                        If iRIArrangementLineKey > 0 Then
                            oOrigNodePrem2 = oOrigDocPrem2.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKeyPrem2 & "']/ArrangementRow[@RIArrangementLineKey='" & sPremiumToolTip & "']")
                        ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                            oOrigNodePrem2 = oOrigDocPrem2.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKeyPrem2 & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
                        End If
                        If oOrigNodePrem2 IsNot Nothing AndAlso oOrigNodePrem2.Attributes("Premium") IsNot Nothing Then
                            Dim dOrigPrem2 As Double = 0
                            Double.TryParse(oOrigNodePrem2.Attributes("Premium").Value, dOrigPrem2)
                            If Math.Round(dPremium, 2) = Math.Round(dOrigPrem2, 2) Then
                                bPremDiffersRegular = False
                            End If
                        End If
                        If oOrigNodePrem2 IsNot Nothing AndAlso oOrigNodePrem2.Attributes("IsEditedDB") IsNot Nothing AndAlso oOrigNodePrem2.Attributes("IsEditedDB").Value.ToUpper() = "TRUE" Then
                            bPremDiffersRegular = True
                        End If
                    End If
                    If bPremDiffersRegular Then
                        MarkRowAsEdited(oXMLData, RIBANDID, sPremiumToolTip, sTreatyCode)
                        If BandHasEditedRows(oXMLData, RIBANDID) Then pnlOverrideReason.Visible = True
                    Else
                        ClearRowEditedFlags(oXMLData, RIBANDID, sPremiumToolTip, sTreatyCode)
                        If Not BandHasEditedRows(oXMLData, RIBANDID) Then pnlOverrideReason.Visible = False
                    End If
                ElseIf oNode.Attributes("Placement") IsNot Nothing AndAlso oNode.Attributes("Placement").Value = "FAC Prop" AndAlso bIsFACPremiumNonProportional Then
                    MarkRowAsEdited(oXMLData, RIBANDID, txtPremium.ToolTip, sTreatyCode)
                    ' FAC Prop premium edit — do not show override reason panel
                    ' rfvOverrideReason.Enabled = True
                    Dim dBandPremium As Double = GetBandTotalPremium(oTempXMLData, RIBANDID)
                    Dim dTotalFACPropPremium As Double = GetTotalFACPropPremium(oTempXMLData, txtPremium.ToolTip, dPremium)

                    If dTotalFACPropPremium > dBandPremium Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_FACPropPremiumExceedsBand").ToString() & "');", True)
                        Exit Sub
                    End If

                    dTaxPercentage = GetTaxPercentage(dPremium, 0, oXMLData, RIBANDID, sPremiumToolTip, sTreatyCode)
                    oReinsurance.UpdateFACPropPremium(oXMLData, dPremium, RIBANDID, txtPremium.ToolTip, Session(CNRIArrangementkey), Session(CNRITransactionType), dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
                Else
                    ' PBI 35359: Standard treaty lines (T, R, TFS) — editable when Reinsurance Manual Premium Adjustment is enabled
                    If bIsManualPremiumAdjustmentEnabled Then
                        oNode.Attributes("Premium").Value = dPremium.ToString()
                        dTaxPercentage = GetTaxPercentage(dPremium, 0, oXMLDoc.OuterXml, RIBANDID, sPremiumToolTip, sTreatyCode)
                        Dim dCommPerc As Double = 0
                        If oNode.Attributes("CommissionPerc") IsNot Nothing Then Double.TryParse(oNode.Attributes("CommissionPerc").Value, dCommPerc)
                        oReinsurance.ApplyManualTreatyFinancials(oNode, dPremium, dTaxPercentage, dCommPerc)
                        ' Mark IsEditedDB=True and IsPremiumEdited=True BEFORE Recalculate.
                        ' IsEditedDB must always be set so Recalculate preserves the user-entered
                        ' premium regardless of whether it matches the original DB value.
                        If oNode.Attributes("IsEditedDB") Is Nothing Then
                            Dim attrEdited As XmlAttribute = oXMLDoc.CreateAttribute("IsEditedDB")
                            attrEdited.Value = "True"
                            oNode.Attributes.Append(attrEdited)
                        Else
                            oNode.Attributes("IsEditedDB").Value = "True"
                        End If
                        If oNode.Attributes("IsPremiumEdited") Is Nothing Then
                            Dim attrPE As XmlAttribute = oXMLDoc.CreateAttribute("IsPremiumEdited")
                            attrPE.Value = "True"
                            oNode.Attributes.Append(attrPE)
                        Else
                            oNode.Attributes("IsPremiumEdited").Value = "True"
                        End If
                        oXMLData = oXMLDoc.OuterXml
                        Dim oReinsuranceCalc As New Reinsurance.Reinsurance
                        oReinsuranceCalc.Recalculate(oXMLData, RIBANDID, Session(CNRIArrangementkey), iRIArrangementLineKey.ToString(), Session(CNRITransactionType), False, dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
                        MarkRowAsEdited(oXMLData, RIBANDID, sPremiumToolTip, sTreatyCode)
                        If BandHasEditedRows(oXMLData, RIBANDID) Then pnlOverrideReason.Visible = True
                    End If
                End If
            End If

            Session(CNRIXMLData) = oXMLData
            ' Bug #39513: Recalculate tax for treaty nodes reset to 0 during Recalculate
            CalculateObligatoryTax()
            oXMLData = Convert.ToString(Session(CNRIXMLData))
            Dim oXMLSource As New XmlDataSource
            oXMLSource.EnableCaching = False
            oXMLSource.Data = oXMLData
            oXMLSource.XPath = ".//RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow[@IsDeleted='False']"
            gvReinsurance.DataSource = oXMLSource
            gvReinsurance.DataBind()
            ApplyEditedRowHighlighting()
        End Sub

        ''' <summary>
        ''' Get total FAC Prop Premium Percentage excluding current item
        ''' </summary>
        Function GetTotalFACPropPremiumPercentage(ByVal sXML As String, ByVal sCurrentRIArrangementLineKey As String, ByVal dCurrentPerc As Double) As Double
            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(sXML)
            Dim dTotPerc As Double = 0
            Dim sanitizedBandKey As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()

            Dim oRIBandList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Placement='FAC Prop' and @IsDeleted='False']")
            If oRIBandList IsNot Nothing Then
                For Each oNode As XmlNode In oRIBandList
                    If oNode.Attributes("RIArrangementLineKey").Value <> sCurrentRIArrangementLineKey Then
                        Dim dPerc As Double = 0
                        If oNode.Attributes("FACPropPremiumPerc") IsNot Nothing Then
                            Double.TryParse(oNode.Attributes("FACPropPremiumPerc").Value, dPerc)
                        End If
                        dTotPerc += dPerc
                    End If
                Next
            End If

            Return dTotPerc + dCurrentPerc
        End Function

        ''' <summary>
        ''' Get Band Total Premium
        ''' </summary>
        Function GetBandTotalPremium(ByVal sXML As String, ByVal RIBANDID As String) As Double
            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(sXML)
            Dim dBandPremium As Double = 0
            Dim sanitizedBandId As String = RIBANDID.Replace("'", "''")

            Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@Name='Band Total']")
            If oBandNode IsNot Nothing AndAlso oBandNode.Attributes("Premium") IsNot Nothing Then
                Double.TryParse(oBandNode.Attributes("Premium").Value, dBandPremium)
            End If

            Return dBandPremium
        End Function

        ''' <summary>
        ''' Get total FAC Prop Premium excluding current item
        ''' </summary>
        Function GetTotalFACPropPremium(ByVal sXML As String, ByVal sCurrentRIArrangementLineKey As String, ByVal dCurrentPremium As Double) As Double
            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(sXML)
            Dim dTotPremium As Double = 0
            Dim sanitizedBandKey As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()

            Dim oRIBandList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@Placement='FAC Prop' and @IsDeleted='False']")
            If oRIBandList IsNot Nothing Then
                For Each oNode As XmlNode In oRIBandList
                    If oNode.Attributes("RIArrangementLineKey").Value <> sCurrentRIArrangementLineKey Then
                        Dim dPremium As Double = 0
                        If oNode.Attributes("Premium") IsNot Nothing Then
                            Double.TryParse(oNode.Attributes("Premium").Value, dPremium)
                        End If
                        dTotPremium += dPremium
                    End If
                Next
            End If

            Return dTotPremium + dCurrentPremium
        End Function
        Public Function GetExistingTreatyIds(ByVal sXML As String, ByVal sRIBANDID As String) As List(Of String)
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim existingTreatyIds As New List(Of String)()
            Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@TreatyCode!='']")
            For Each oNode As XmlNode In oNodes
                If oNode.Attributes("IsDeleted") Is Nothing OrElse oNode.Attributes("IsDeleted").Value <> "True" Then
                    Dim treatyCode As String = oNode.Attributes("TreatyCode").Value
                    If Not String.IsNullOrEmpty(treatyCode) AndAlso Not existingTreatyIds.Contains(treatyCode) Then
                        existingTreatyIds.Add(treatyCode)
                    End If
                End If
            Next
            Return existingTreatyIds
        End Function

        ''' <summary>
        ''' Returns the shared priority for manually added treaties.
        ''' If any manually added treaty already exists, reuse its priority so all share the same value.
        ''' Otherwise falls back to GetNextPriority.
        ''' </summary>
        Public Function GetManualTreatyPriority(ByVal sXML As String, ByVal sRIBANDID As String) As Integer
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            ' Check non-deleted manual treaties first
            Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@ManuallyAdded='True' and @IsDeleted='False']")
            If oNode IsNot Nothing AndAlso oNode.Attributes("Priority") IsNot Nothing Then
                Dim iPriority As Integer = 0
                If Integer.TryParse(oNode.Attributes("Priority").Value, iPriority) AndAlso iPriority > 0 Then
                    Return iPriority
                End If
            End If
            ' Also check deleted manual treaties to preserve the same priority slot
            Dim oDeletedNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@ManuallyAdded='True' and @IsDeleted='True']")
            If oDeletedNode IsNot Nothing AndAlso oDeletedNode.Attributes("Priority") IsNot Nothing Then
                Dim iPriority As Integer = 0
                If Integer.TryParse(oDeletedNode.Attributes("Priority").Value, iPriority) AndAlso iPriority > 0 Then
                    Return iPriority
                End If
            End If
            Return GetNextPriority(sXML, sRIBANDID)
        End Function

        Public Function GetNextPriority(ByVal sXML As String, ByVal sRIBANDID As String) As Integer
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim maxPriority As Integer = 0
            ' Only consider system-generated (non-manual) treaties for max priority
            Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Priority and @Type!='F' and @Type!='FX']")
            For Each oNode As XmlNode In oNodes
                If oNode.Attributes("IsDeleted") Is Nothing OrElse oNode.Attributes("IsDeleted").Value <> "True" Then
                    Dim bIsManual As Boolean = False
                    If oNode.Attributes("ManuallyAdded") IsNot Nothing Then
                        Boolean.TryParse(oNode.Attributes("ManuallyAdded").Value, bIsManual)
                    End If
                    If Not bIsManual Then
                        Dim priority As Integer = 0
                        If Integer.TryParse(oNode.Attributes("Priority").Value, priority) Then
                            If priority > maxPriority Then maxPriority = priority
                        End If
                    End If
                End If
            Next
            Return maxPriority + 1
        End Function

        ''' <summary>
        ''' Generates a unique RIArrangementLineKey by finding the max existing key across all
        ''' arrangement types (F, FX, T, TX, TC, R, TFS) in the band and incrementing by 1.
        ''' </summary>
        Public Sub GenerateUniqueRILineId(ByVal sXML As String, ByVal sRIBANDID As String, ByRef iNewId As Integer)
            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(sXML)
            Dim sBandName As String = "Current_" & sRIBANDID
            iNewId = 0
            For Each sType As String In New String() {"F", "FX", "TC", "R", "TFS"}
                Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sBandName & "']/ArrangementRow[@Type='" & sType & "']")
                For Each oNode As XmlNode In oNodes
                    Dim iKey As Integer = 0
                    If Integer.TryParse(oNode.Attributes("RIArrangementLineKey").Value.Trim(), iKey) AndAlso iKey > iNewId Then
                        iNewId = iKey
                    End If
                Next
            Next
            For Each sType As String In New String() {"T", "TX", "PX"}
                Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sBandName & "']/ArrangementRow[@Type='" & sType & "']")
                For Each oNode As XmlNode In oNodes
                    Dim bIsManuallyAdded As Boolean = False
                    If oNode.Attributes("ManuallyAdded") IsNot Nothing Then
                        Boolean.TryParse(oNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
                    End If
                    Dim sLineKey As String = If(oNode.Attributes("RIArrangementLineKey") IsNot Nothing, oNode.Attributes("RIArrangementLineKey").Value.Trim(), "")
                    If bIsManuallyAdded AndAlso String.IsNullOrEmpty(sLineKey) Then sLineKey = "0"
                    Dim iKey As Integer = 0
                    If Integer.TryParse(sLineKey, iKey) AndAlso iKey > iNewId Then
                        iNewId = iKey
                    End If
                Next
            Next
            iNewId += 1
        End Sub

        Public Sub AddRIArrangementLines(ByRef sXML As String, ByVal oArrangementLinesType As ArrangementLinesType, ByVal sRIBANDID As String, Optional ByVal sRIArrangementLineKey As String = "", Optional ByVal sRIarrangementKey As String = "", Optional ByVal sRIType As String = "", Optional ByVal sTransType As String = "NB", Optional ByVal bEditCall As Boolean = False, Optional ByRef sEditiedRIArrangementLineKey As String = "0", Optional ByVal bIsPortfolioRIAmendment As Boolean = False, Optional ByVal bIsManuallyAdded As Boolean = False)
            Dim oReinsurance As New Reinsurance.Reinsurance()
            oReinsurance.AddRIArrangementLines(sXML, oArrangementLinesType, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, sRIType, sTransType, bEditCall, sEditiedRIArrangementLineKey, bIsPortfolioRIAmendment, bIsManuallyAdded)
        End Sub

        Public Sub EditRIArrangementLines(ByRef sXML As String, ByVal oArrangementLinesType As ArrangementLinesType, ByVal sRIBANDID As String, Optional ByVal sRIArrangementLineKey As String = "", Optional ByVal sRIarrangementKey As String = "", Optional ByVal sRIType As String = "", Optional ByVal sTransType As String = "NB")
            Dim oReinsurance As New Reinsurance.Reinsurance()
            oReinsurance.EditRIArrangementLines(sXML, oArrangementLinesType, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, sRIType, sTransType)
        End Sub

        Public Sub RemoveRIArrangementLine(ByRef sXML As String, ByVal sRIBANDID As String, ByVal sRIArrangementLineKey As String, ByVal sRIarrangementKey As String, Optional ByVal sTransType As String = "NB")
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey & "']")
            If oNode IsNot Nothing Then
                If oNode.Attributes("IsNew") IsNot Nothing AndAlso oNode.Attributes("IsNew").Value = "True" Then
                    Dim oRootNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']")
                    oRootNode.RemoveChild(oNode)
                Else
                    oNode.Attributes("IsDeleted").Value = "True"
                End If
            End If
            sXML = oXMLDoc.OuterXml
            Dim oReinsurance As New Reinsurance.Reinsurance()
            oReinsurance.Recalculate(sXML, sRIBANDID, sRIarrangementKey, sRIArrangementLineKey, sTransType, True)
        End Sub

        Public Function IsManuallyAdded(ByVal sXML As String, ByVal sRIBANDID As String, ByVal sRIArrangementLineKey As String, Optional ByVal sTreatyCode As String = "") As Boolean
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim oNode As XmlNode = Nothing

            If String.IsNullOrEmpty(sRIArrangementLineKey) OrElse sRIArrangementLineKey = "0" Then
                If Not String.IsNullOrEmpty(sTreatyCode) Then
                    oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
                Else
                    Return False
                End If
            Else
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey & "']")
            End If

            If oNode IsNot Nothing AndAlso oNode.Attributes("ManuallyAdded") IsNot Nothing Then
                Return oNode.Attributes("ManuallyAdded").Value = "True" OrElse oNode.Attributes("ManuallyAdded").Value = "1"
            End If
            Return False
        End Function

        Public Function GetNetOfFACValues(ByVal sXML As String, ByVal sRIBANDID As String) As Tuple(Of Double, Double)
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim dNetSI As Double = 0
            Dim dNetPremium As Double = 0
            Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")
            If oNetFACNode IsNot Nothing Then
                ' Net of FAC node already has obligatory QSH deducted by Recalculate — use as-is
                Double.TryParse(oNetFACNode.Attributes("SumInsured").Value, dNetSI)
                Double.TryParse(oNetFACNode.Attributes("Premium").Value, dNetPremium)
            Else
                ' Fallback to Band Total — subtract obligatory QSH manually
                Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
                If oBandNode IsNot Nothing Then
                    Double.TryParse(oBandNode.Attributes("SumInsured").Value, dNetSI)
                    Double.TryParse(oBandNode.Attributes("Premium").Value, dNetPremium)
                End If
                Dim oOblTNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Type='T' and (@ManuallyAdded='False' or not(@ManuallyAdded)) and @IsDeleted!='True']")
                For Each oOblNode As XmlNode In oOblTNodes
                    Dim bIsObligatory As Boolean = False
                    If oOblNode.Attributes("IsObligatory") IsNot Nothing Then Boolean.TryParse(oOblNode.Attributes("IsObligatory").Value, bIsObligatory)
                    If bIsObligatory Then
                        Dim dOblSI As Double = 0, dOblPremium As Double = 0
                        Double.TryParse(oOblNode.Attributes("SumInsured").Value, dOblSI)
                        Double.TryParse(oOblNode.Attributes("Premium").Value, dOblPremium)
                        dNetSI -= dOblSI
                        dNetPremium -= dOblPremium
                    End If
                Next
            End If
            Return New Tuple(Of Double, Double)(dNetSI, dNetPremium)
        End Function

        Public Function RecalculateThisPercentage(ByVal dSumInsured As Double, ByVal dNetOfFACSI As Double) As Double
            Try
                If dNetOfFACSI = 0 Then
                    HandleDivisionByZero()
                    Return 0
                End If
                Return (dSumInsured / dNetOfFACSI) * 100
            Catch ex As DivideByZeroException
                HandleDivisionByZero()
                Return 0
            Catch ex As Exception
                HandleAPIError(ex)
                Return 0
            End Try
        End Function

        Public Function GetTotalThisPercentage(ByVal sXML As String, ByVal sRIBANDID As String, Optional ByVal sExcludeLineKey As String = "") As Double
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim dTotalPercentage As Double = 0
            Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@ThisPerc]")
            For Each oNode As XmlNode In oNodes
                If oNode.Attributes("IsDeleted") Is Nothing OrElse oNode.Attributes("IsDeleted").Value <> "True" Then
                    Dim lineKey As String = If(oNode.Attributes("RIArrangementLineKey") IsNot Nothing, oNode.Attributes("RIArrangementLineKey").Value, "")
                    If lineKey <> sExcludeLineKey Then
                        Dim dPerc As Double = 0
                        If Double.TryParse(oNode.Attributes("ThisPerc").Value, dPerc) Then dTotalPercentage += dPerc
                    End If
                End If
            Next
            Return dTotalPercentage
        End Function

        'Public Function GetAvailableTreatiesForAddition(policyId As Integer, arrangementId As Integer, treatyType As String) As DataTable
        '    Dim dtTreaties As New DataTable()
        '    Dim apiUrl As String = If(treatyType = "Prop", "GetRIPropTreaties", "GetRIXOLTreaties")
        '    Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
        '    Dim sRIBANDID As String = Convert.ToString(Session(CNRIBandKey))
        '    Dim existingTreatyIds As List(Of String) = GetExistingTreatyIds(sXML, sRIBANDID)

        '    Using client As New System.Net.WebClient()
        '        client.Headers("Content-Type") = "application/json"
        '        Dim response As String = client.DownloadString(ConfigurationManager.AppSettings("APIBaseUrl") & apiUrl)
        '        Dim ds As DataSet = Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataSet)(response)
        '        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
        '            dtTreaties = ds.Tables(0).Clone()
        '            For Each row As DataRow In ds.Tables(0).Rows
        '                Dim treatyCode As String = row("TreatyCode").ToString()
        '                If Not existingTreatyIds.Contains(treatyCode) Then dtTreaties.ImportRow(row)
        '            Next
        '        End If
        '    End Using
        '    Return dtTreaties
        'End Function

        'Public Function AddManualTreaty(arrangementId As Integer, treatyId As Integer) As Boolean
        '    Try
        '        Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
        '        Dim sRIBANDID As String = Convert.ToString(Session(CNRIBandKey))
        '        Dim sRIArrangementKey As String = Convert.ToString(Session(CNRIArrangementkey))

        '        Using client As New System.Net.WebClient()
        '            client.Headers("Content-Type") = "application/json"
        '            Dim response As String = client.DownloadString(ConfigurationManager.AppSettings("APIBaseUrl") & "GetRIPropTreaties")
        '            Dim ds As DataSet = Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataSet)(response)
        '            If ds Is Nothing OrElse ds.Tables.Count = 0 Then Return False

        '            Dim treatyRow As DataRow = ds.Tables(0).Select("TreatyId = " & treatyId).FirstOrDefault()
        '            If treatyRow Is Nothing Then Return False

        '            Dim nextPriority As Integer = GetNextPriority(sXML, sRIBANDID)
        '            Dim oArrangementLine As New ArrangementLinesType With {
        '                .TreatyCode = treatyRow("TreatyCode").ToString(),
        '                .TreatyID = Convert.ToInt32(treatyRow("TreatyID")),
        '                .RIName = treatyRow("TreatyDescription").ToString(),
        '                .Priority = nextPriority,
        '                .RIPlacement = "Treaty QSH",
        '                .Type = "T",
        '                .ManuallyAdded = True,
        '                .RIarrangementKey = arrangementId
        '            }

        '            AddRIArrangementLines(sXML, oArrangementLine, sRIBANDID, "", sRIArrangementKey, "T", Session(CNRITransactionType), bIsManuallyAdded:=True)
        '            Dim oReinsurance As New Reinsurance.Reinsurance()
        '            Dim riXmlDoc As New XmlDocument()
        '            riXmlDoc.LoadXml(sXML)
        '            oReinsurance.Recalculate(riXmlDoc)
        '            sXML = riXmlDoc.OuterXml
        '            Session(CNRIXMLData) = sXML
        '            Return True
        '        End Using
        '    Catch
        '        Return False
        '    End Try
        'End Function

        Public Function UpdateManualTreatyLine(arrangementId As Integer, lineId As Integer, updates As Dictionary(Of String, Object)) As Boolean
            Try
                Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
                Dim sRIBANDID As String = Convert.ToString(Session(CNRIBandKey))
                Dim oXMLDoc As New XmlDocument()
                oXMLDoc.LoadXml(sXML)

                Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & lineId & "']")
                If oNode Is Nothing Then Return False

                Dim bSumInsuredChanged As Boolean = False
                For Each kvp In updates
                    Select Case kvp.Key
                        Case "SumInsured"
                            oNode.Attributes("SumInsured").Value = kvp.Value.ToString()
                            bSumInsuredChanged = True
                        Case "CommissionPerc"
                            Dim dCommPerc As Double = Convert.ToDouble(kvp.Value)
                            If dCommPerc < 0 OrElse dCommPerc > 100 Then Return False
                            oNode.Attributes("CommissionPerc").Value = dCommPerc.ToString()
                        Case "Agreement"
                            oNode.Attributes("Agreement").Value = kvp.Value.ToString()
                    End Select
                Next

                If bSumInsuredChanged Then
                    Dim netValues As Tuple(Of Double, Double) = GetNetOfFACValues(sXML, sRIBANDID)
                    Dim dThisPerc As Double = RecalculateThisPercentage(Convert.ToDouble(oNode.Attributes("SumInsured").Value), netValues.Item1)
                    oNode.Attributes("ThisPerc").Value = Format(dThisPerc, "0.0000")

                    Dim dTotalPerc As Double = GetTotalThisPercentage(oXMLDoc.OuterXml, sRIBANDID)
                    If dTotalPerc > 100 Then Return False
                End If

                sXML = oXMLDoc.OuterXml
                Dim oReinsurance As New Reinsurance.Reinsurance()
                Dim riXmlDoc As New XmlDocument()
                riXmlDoc.LoadXml(sXML)
                oReinsurance.Recalculate(riXmlDoc)
                sXML = riXmlDoc.OuterXml
                Session(CNRIXMLData) = sXML
                Return True
            Catch
                Return False
            End Try
        End Function

        Public Function DeleteManualTreaty(lineId As Integer) As Boolean
            Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
            Dim sRIBANDID As String = Convert.ToString(Session(CNRIBandKey))
            Dim sRIArrangementKey As String = Convert.ToString(Session(CNRIArrangementkey))
            RemoveRIArrangementLine(sXML, sRIBANDID, lineId.ToString(), sRIArrangementKey, Session(CNRITransactionType))
            Session(CNRIXMLData) = sXML
            Return True
        End Function

        Public Sub SaveRIArrangement()
            UpdateRIArrangement()
        End Sub

        Public Function GetRIArrangementFromSession() As String
            Return Convert.ToString(Session(CNRIXMLData))
        End Function

        Public Sub SaveRIArrangementToSession(riXml As String)
            Session(CNRIXMLData) = riXml
        End Sub

        Public Sub ClearRIArrangementFromSession()
            Session.Remove(CNRIXMLData)
            Session.Remove(CNRIXMLDataOriginal)
            Session.Remove(CNRIArrangementkey)
            Session.Remove(CNRIBandKey)
            Session.Remove(CNRIFACProp)
            Session.Remove(CNRIFACXolTemp)
        End Sub

        Private Function LoadRIArrangementFromDatabase(arrangementId As Integer) As String
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oReinsuranceBandsCollection As NexusProvider.ReinsuranceBandsCollection
            If Session(CNCurrentRiskKey) Is Nothing Then
                oReinsuranceBandsCollection = oWebService.GetRiskReinsuranceBands(CType(Session(CNQuote), NexusProvider.Quote).Risks(0).Key)
            Else
                oReinsuranceBandsCollection = oWebService.GetRiskReinsuranceBands(CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key)
            End If
            Return oWebService.GetReinsurance2007(CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key, 0, oReinsuranceBandsCollection)
        End Function

        Private Function ValidateCommissionPercentage(commission As Decimal) As Boolean
            Return commission >= 0 AndAlso commission <= 100
        End Function

        Private Function ValidateDuplicateTreaty(sXML As String, sRIBANDID As String, treatyCode As String) As Boolean
            Dim existingTreatyIds As List(Of String) = GetExistingTreatyIds(sXML, sRIBANDID)
            Return existingTreatyIds.Contains(treatyCode)
        End Function

        Private Function CheckOverAllocation(sXML As String, sRIBANDID As String) As Boolean
            Dim dTotalPercentage As Double = GetTotalThisPercentage(sXML, sRIBANDID)
            Return dTotalPercentage > 100
        End Function

        Private Function ValidateTransactionState() As Boolean
            If Session(CNMode) = Mode.View OrElse Session(CNRiskMode) = RiskMode.View Then
                Return False
            End If
            ' Gap 4: Add Treaty buttons must be hidden during Cancellation (RI screen is non-editable per spec)
            If Session(CNMTAType) = MTAType.CANCELLATION Then
                Return False
            End If
            Return True
        End Function

        Private Sub HandleConcurrentModification()
            Try
                If Session(CNRIXMLData) Is Nothing Then
                    ScriptManager.RegisterStartupScript(Page, GetType(Page), "concurrency", "alert('" & GetLocalResourceObject("msg_SessionExpired").ToString() & "');", True)
                    Session.Remove(CNRIArrangementkey)
                    Session.Remove(CNRIBandKey)
                End If
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Page, GetType(Page), "error", "alert('" & GetLocalResourceObject("msg_ConcurrentModification").ToString() & "');", True)
            End Try
        End Sub

        Private Sub HandleBusinessRuleViolation(message As String)
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "businessRule", "alert('" & message.Replace("'", "\'") & "');", True)
        End Sub

        Private Sub HandleAPIError(ex As Exception)
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "apiError", "alert('" & String.Format(GetLocalResourceObject("msg_APIError").ToString(), ex.Message.Replace("'", "\'")) & "');", True)
        End Sub

        Private Sub HandleInvalidTreatyID()
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "invalidTreaty", "alert('" & GetLocalResourceObject("msg_InvalidTreatySelected").ToString() & "');", True)
        End Sub

        Private Sub HandleDivisionByZero()
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "divZero", "alert('" & GetLocalResourceObject("msg_DivisionByZero").ToString() & "');", True)
        End Sub

        ''' <summary>
        ''' Returns the TreatyCode for a manually added treaty row identified by the TextBox ToolTip (RIArrangementLineKey).
        ''' Used when RIArrangementLineKey is 0 (manually added treaties not yet persisted to DB).
        ''' </summary>
        Private Function GetTreatyCodeFromGridRow(ByVal sXML As String, ByVal sRIBANDID As String, ByVal oControl As System.Web.UI.WebControls.WebControl) As String
            If String.IsNullOrEmpty(sXML) OrElse String.IsNullOrEmpty(sRIBANDID) OrElse oControl Is Nothing Then Return ""
            Dim sToolTip As String = oControl.ToolTip.Trim()
            If String.IsNullOrEmpty(sToolTip) OrElse sToolTip = "0" Then Return ""
            Try
                Dim oXMLDoc As New XmlDocument()
                oXMLDoc.LoadXml(sXML)
                Dim sanitizedBandId As String = sRIBANDID.Replace("'", "''")
                Dim sanitizedToolTip As String = sToolTip.Replace("'", "''")
                Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sanitizedBandId & "']/ArrangementRow[@RIArrangementLineKey='" & sanitizedToolTip & "' and @ManuallyAdded='True']")
                If oNode IsNot Nothing AndAlso oNode.Attributes("TreatyCode") IsNot Nothing Then
                    Return oNode.Attributes("TreatyCode").Value
                End If
            Catch
            End Try
            Return ""
        End Function

        Private Sub SetTreatyButtonVisibility()
            Dim bIsEditable As Boolean = ValidateTransactionState()
            btnAddPropTreaty.Visible = bIsEditable
            btnAddXOLTreaty.Visible = bIsEditable
        End Sub

        Private Function HasManuallyAddedTreaties(ByVal sXML As String, ByVal sBandKey As String) As Boolean
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sBandKey & "']/ArrangementRow[@ManuallyAdded='True' and @IsDeleted='False']")
            Return oNodes IsNot Nothing AndAlso oNodes.Count > 0
        End Function
        Private Sub SetupTreatyButtonUrls()
            Dim sRIArrangementKeyParam As String = If(Session(CNRIArrangementkey) IsNot Nothing, Session(CNRIArrangementkey).ToString(), "0")
            Dim sPropTreatyUrl As String = AppSettings("WebRoot") & "/Modal/SelectTreaty.aspx?Type=T&ClientID=" + updSubmitArea.ClientID + "&RIArrangementKey=" & sRIArrangementKeyParam & "&RI2007=ON&modal=true&KeepThis=true&TB_iframe=true&height=100&width=950"
            Dim sXOLTreatyUrl As String = AppSettings("WebRoot") & "/Modal/SelectTreaty.aspx?Type=TX&ClientID=" + updSubmitArea.ClientID + "&RIArrangementKey=" & sRIArrangementKeyParam & "&RI2007=ON&modal=true&KeepThis=true&TB_iframe=true&height=100&width=950"
            btnAddPropTreaty.OnClientClick = "tb_show(null , '" & sPropTreatyUrl & "' , null);return false;"
            btnAddXOLTreaty.OnClientClick = "tb_show(null , '" & sXOLTreatyUrl & "' , null);return false;"
        End Sub
        Private Function CheckReinsuranceManualPremiumAdjustment() As Boolean
            Try
                Dim oWS As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sValue As String = oWS.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RiManualPremiumAdjustment, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()
                Return Not String.IsNullOrEmpty(sValue) AndAlso sValue = "1"
            Catch
                Return False
            End Try
        End Function

        Private Sub LoadOverrideReasonDropdown()
            Try
                Dim oWS As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oList As NexusProvider.LookupListCollection = oWS.GetList(NexusProvider.ListType.PMLookup, "RI_Override_Reason", True, False)
                ddlOverrideReason.Items.Clear()
                ddlOverrideReason.Items.Add(New ListItem("", "0"))
                ddlOrgOverrideReason.Items.Clear()
                ddlOrgOverrideReason.Items.Add(New ListItem("", "0"))
                If oList IsNot Nothing Then
                    For Each oItem As NexusProvider.LookupListItem In oList
                        ddlOverrideReason.Items.Add(New ListItem(oItem.Description, oItem.Key.ToString()))
                        ddlOrgOverrideReason.Items.Add(New ListItem(oItem.Description, oItem.Key.ToString()))
                    Next
                End If
            Catch
            End Try
        End Sub

        Private Sub RestoreOverrideReasonState()
            Dim sXML As String = Convert.ToString(Session(CNRIXMLData))
            Dim sBandKey As String = Convert.ToString(Session(CNRIBandKey)).Trim()
            If String.IsNullOrEmpty(sXML) OrElse String.IsNullOrEmpty(sBandKey) Then Exit Sub

            Dim bIsCancellation As Boolean = (Session(CNMTAType) = MTAType.CANCELLATION)
            Dim bIsMTAMode As Boolean = (Session(CNMTAType) = MTAType.PERMANENT OrElse
                                         Session(CNMTAType) = MTAType.TEMPORARY OrElse
                                         Session(CNRenewal) = True)

            ' Current Override Reason: show for MTA always, or when edited rows detected for NB/Renewal.
            ' At Renewal, only show if the user has actually made changes (bHasEdited).
            ' Cancellation is non-editable so the panel must not be shown as an interactive control.
            Dim bHasEdited As Boolean = BandHasEditedRows(sXML, sBandKey)
            pnlOverrideReason.Visible = bHasEdited AndAlso Not bIsCancellation

            If pnlOverrideReason.Visible Then
                Dim sReasonId As String = GetBandOverrideReasonId(sXML, sBandKey)
                If Not String.IsNullOrEmpty(sReasonId) AndAlso sReasonId <> "0" AndAlso ddlOverrideReason.Items.FindByValue(sReasonId) IsNot Nothing Then
                    ddlOverrideReason.SelectedValue = sReasonId
                Else
                    ddlOverrideReason.SelectedIndex = 0
                End If
            End If

            ' Original Override Reason: show for PERMANENT/TEMPORARY MTA and Renewal only when a reason was previously saved
            Dim bIsOrgReasonMode As Boolean = (Session(CNMTAType) = MTAType.PERMANENT OrElse
                                               Session(CNMTAType) = MTAType.TEMPORARY OrElse
                                               Session(CNRenewal) = True OrElse
                                               Session(CNMTAType) = MTAType.CANCELLATION OrElse
                                               Session(CNRITransactionType) = "PT" OrElse
                                               Session(CNRITransactionType) = "REN")
            If bIsOrgReasonMode Then
                If ddlOrgOverrideReason.Items.Count <= 1 Then LoadOverrideReasonDropdown()
                Dim sOrgReasonId As String = GetBandOriginalOverrideReasonId(sXML, sBandKey)
                If Not String.IsNullOrEmpty(sOrgReasonId) AndAlso sOrgReasonId <> "0" Then
                    If ddlOrgOverrideReason.Items.FindByValue(sOrgReasonId) IsNot Nothing Then
                        ddlOrgOverrideReason.SelectedValue = sOrgReasonId
                    End If
                    ddlOrgOverrideReason.Enabled = False
                    pnlOrgOverrideReason.Visible = True
                Else
                    pnlOrgOverrideReason.Visible = False
                End If
            Else
                pnlOrgOverrideReason.Visible = False
            End If
        End Sub

        ''' <summary>
        ''' Returns True if the RI arrangement line identified by sLineKey (or sTreatyCode) is
        ''' a FAC Prop (Type='F') or FAC XOL (Type='FX') line. Used to suppress the override
        ''' reason panel for FAC lines — adding/editing a FAC is a placement action, not a
        ''' manual override of RI model values.
        ''' </summary>
        Private Function IsFACLine(ByVal sXML As String, ByVal sBandId As String, ByVal sLineKey As String, Optional ByVal sTreatyCode As String = "") As Boolean
            If String.IsNullOrEmpty(sXML) OrElse String.IsNullOrEmpty(sBandId) Then Return False
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim oNode As XmlNode = Nothing
            Dim sBandIdXPath As String = XPathLiteral("Current_" & sBandId)
            If Not String.IsNullOrEmpty(sLineKey) AndAlso sLineKey <> "0" Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name=" & sBandIdXPath & "]/ArrangementRow[@RIArrangementLineKey='" & sLineKey & "']")
            End If
            If oNode Is Nothing AndAlso Not String.IsNullOrEmpty(sTreatyCode) Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name=" & sBandIdXPath & "]/ArrangementRow[@TreatyCode=" & XPathLiteral(sTreatyCode) & "]")
            End If
            If oNode Is Nothing Then Return False
            Dim sType As String = If(oNode.Attributes("Type") IsNot Nothing, oNode.Attributes("Type").Value, "")
            Return sType = "F" OrElse sType = "FX"
        End Function

        Private Sub MarkRowAsEdited(ByRef sXML As String, ByVal sBandId As String, ByVal sLineKey As String, Optional ByVal sTreatyCode As String = "")
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim oNode As XmlNode = Nothing
            If sLineKey <> "0" AndAlso Not String.IsNullOrEmpty(sLineKey) Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandId & "']/ArrangementRow[@RIArrangementLineKey='" & sLineKey & "']")
            ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandId & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
            End If
            If oNode IsNot Nothing Then
                ' IsEdited drives the tombstone logic in gvReinsurance_RowCommand
                'If oNode.Attributes("IsEdited") Is Nothing Then
                '    Dim attr As XmlAttribute = oXMLDoc.CreateAttribute("IsEdited")
                '    attr.Value = "True"
                '    oNode.Attributes.Append(attr)
                'Else
                '    oNode.Attributes("IsEdited").Value = "True"
                'End If
                ' GAP 1 fix: set IsUserEdited so ApplyEditedRowHighlighting highlights this row
                'If oNode.Attributes("IsUserEdited") Is Nothing Then
                'Dim attrUE2 As XmlAttribute = oXMLDoc.CreateAttribute("IsUserEdited")
                'attrUE2.Value = "True"
                'oNode.Attributes.Append(attrUE2)
                'Else
                'oNode.Attributes("IsUserEdited").Value = "True"
                'End If
                ' PBI 35359 refactor: set IsEditedDB=True so DB is_edited flag is persisted on save
                If oNode.Attributes("IsEditedDB") Is Nothing Then
                    Dim attrDB As XmlAttribute = oXMLDoc.CreateAttribute("IsEditedDB")
                    attrDB.Value = "True"
                    oNode.Attributes.Append(attrDB)
                Else
                    oNode.Attributes("IsEditedDB").Value = "True"
                End If
                Dim bIsNewNode As Boolean = False
                If oNode.Attributes("IsNew") IsNot Nothing Then Boolean.TryParse(oNode.Attributes("IsNew").Value, bIsNewNode)
                If Not bIsNewNode Then
                    If oNode.Attributes("ActionType") Is Nothing Then
                        Dim attrAction As XmlAttribute = oXMLDoc.CreateAttribute("ActionType")
                        attrAction.Value = "1"
                        oNode.Attributes.Append(attrAction)
                    Else
                        oNode.Attributes("ActionType").Value = "1"
                    End If
                End If
                If oNode.Attributes("TreatyTypeID") Is Nothing Then
                    Dim sType As String = If(oNode.Attributes("Type") IsNot Nothing, oNode.Attributes("Type").Value, "")
                    Dim attrTTID As XmlAttribute = oXMLDoc.CreateAttribute("TreatyTypeID")
                    If sType = "TX" OrElse sType = "FX" Then
                        attrTTID.Value = "2"
                    ElseIf sType = "R" Then
                        attrTTID.Value = "1"
                    ElseIf sType = "T" OrElse sType = "TFS" Then
                        attrTTID.Value = "1"
                    Else
                        attrTTID.Value = "1"
                    End If
                    oNode.Attributes.Append(attrTTID)
                End If
            End If
            sXML = oXMLDoc.OuterXml
        End Sub

        ''' <summary>
        ''' Clears IsEditedDB and IsUserEdited flags when user reverts a value back to its original.
        ''' </summary>
        Private Sub ClearRowEditedFlags(ByRef sXML As String, ByVal sBandId As String, ByVal sLineKey As String, Optional ByVal sTreatyCode As String = "")
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim oNode As XmlNode = Nothing
            If Not String.IsNullOrEmpty(sLineKey) AndAlso sLineKey <> "0" Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandId & "']/ArrangementRow[@RIArrangementLineKey='" & sLineKey & "']")
            ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandId & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
            End If
            If oNode IsNot Nothing Then
                ' Only clear IsEditedDB if it was not already True in the original XML
                Dim bOriginallyEdited As Boolean = False
                If Not String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLDataOriginal))) Then
                    Dim oOrigDoc As New XmlDocument()
                    oOrigDoc.LoadXml(Convert.ToString(Session(CNRIXMLDataOriginal)))
                    Dim oOrigNode As XmlNode = Nothing
                    If Not String.IsNullOrEmpty(sLineKey) AndAlso sLineKey <> "0" Then
                        oOrigNode = oOrigDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandId & "']/ArrangementRow[@RIArrangementLineKey='" & sLineKey & "']")
                    ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                        oOrigNode = oOrigDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandId & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
                    End If
                    If oOrigNode IsNot Nothing AndAlso oOrigNode.Attributes("IsEditedDB") IsNot Nothing Then
                        Boolean.TryParse(oOrigNode.Attributes("IsEditedDB").Value, bOriginallyEdited)
                    End If
                End If
                If bOriginallyEdited Then
                    ' Preserve IsEditedDB=True from DB — the row was edited in a prior transaction
                    If oNode.Attributes("IsEditedDB") IsNot Nothing Then oNode.Attributes("IsEditedDB").Value = "True"
                    'If oNode.Attributes("IsUserEdited") IsNot Nothing Then oNode.Attributes("IsUserEdited").Value = "True"
                Else
                    If oNode.Attributes("IsEditedDB") IsNot Nothing Then oNode.Attributes("IsEditedDB").Value = "False"
                    'If oNode.Attributes("IsUserEdited") IsNot Nothing Then oNode.Attributes("IsUserEdited").Value = "False"
                End If
            End If
            sXML = oXMLDoc.OuterXml
        End Sub

        Private Sub ClearPremiumEditedFlag(ByRef sXML As String, ByVal sBandId As String, ByVal sLineKey As String, Optional ByVal sTreatyCode As String = "")
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim oNode As XmlNode = Nothing
            If Not String.IsNullOrEmpty(sLineKey) AndAlso sLineKey <> "0" Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandId & "']/ArrangementRow[@RIArrangementLineKey='" & sLineKey & "']")
            ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandId & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
            End If
            If oNode IsNot Nothing AndAlso oNode.Attributes("IsPremiumEdited") IsNot Nothing Then
                oNode.Attributes("IsPremiumEdited").Value = "False"
            End If
            sXML = oXMLDoc.OuterXml
        End Sub

        Private Function BandHasEditedRows(ByVal sXML As String, ByVal sBandKey As String) As Boolean
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            ' PBI 35359: Check IsUserEdited first (set from DB on MTA load), then fall back to IsEditedDB.
            ' Bug 38298: Exclude FAC Prop (Type='F') and FAC XOL (Type='FX') lines — adding a FAC is a
            ' placement action, not a manual override of RI model values, so it must not trigger the
            ' override reason mandatory check or show the override reason panel.
            Dim sBandKeyXPath As String = XPathLiteral("Current_" & sBandKey)
            Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name=" & sBandKeyXPath & "]/ArrangementRow[@IsEditedDB='True' and @Type!='F' and @Type!='FX' and @ManuallyAdded!='True' and @IsDeleted!='True']")
            If oNodes IsNot Nothing AndAlso oNodes.Count > 0 Then Return True
            oNodes = oXMLDoc.SelectNodes("/rows/RIBAND[@Name=" & sBandKeyXPath & "]/ArrangementRow[@IsEditedDB='True' and @Type!='F' and @Type!='FX' and @ManuallyAdded!='True' and @IsDeleted!='True']")
            Return oNodes IsNot Nothing AndAlso oNodes.Count > 0
        End Function

        Private Function GetBandOverrideReasonId(ByVal sXML As String, ByVal sBandKey As String) As String
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKey & "']")
            If oBandNode IsNot Nothing AndAlso oBandNode.Attributes("OverrideReasonId") IsNot Nothing Then
                Return oBandNode.Attributes("OverrideReasonId").Value
            End If
            Return ""
        End Function

        Private Sub SetBandOverrideReasonId(ByRef sXML As String, ByVal sBandKey As String, ByVal sId As String)
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKey & "']")
            If oBandNode IsNot Nothing Then
                If oBandNode.Attributes("OverrideReasonId") Is Nothing Then
                    Dim attr As XmlAttribute = oXMLDoc.CreateAttribute("OverrideReasonId")
                    attr.Value = sId
                    oBandNode.Attributes.Append(attr)
                Else
                    oBandNode.Attributes("OverrideReasonId").Value = sId
                End If
            End If
            sXML = oXMLDoc.OuterXml
        End Sub

        Private Function GetBandOriginalOverrideReasonId(ByVal sXML As String, ByVal sBandKey As String) As String
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            ' PBI 35359: Read from Original_ band first — holds the DB-persisted override reason
            ' from the previous policy version and is never overwritten by current session edits.
            ' Fall back to Current_ band in case Original_ band is absent or has no reason stamped.
            Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Original_" & sBandKey & "']")
            If oBandNode IsNot Nothing AndAlso oBandNode.Attributes("OverrideReasonId") IsNot Nothing Then
                Dim sVal As String = oBandNode.Attributes("OverrideReasonId").Value
                If Not String.IsNullOrEmpty(sVal) AndAlso sVal <> "0" Then Return sVal
            End If
            ' Fallback: read from Current_ band (covers cases where Original_ band is absent)
            Dim oCurrentNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKey & "']")
            If oCurrentNode IsNot Nothing AndAlso oCurrentNode.Attributes("OverrideReasonId") IsNot Nothing Then
                Return oCurrentNode.Attributes("OverrideReasonId").Value
            End If
            Return ""
        End Function


        Private Function GetOverrideReasonDescription(ByVal sId As String) As String
            Dim oItem As ListItem = ddlOverrideReason.Items.FindByValue(sId)
            If oItem IsNot Nothing Then Return oItem.Text
            Return ""
        End Function

        Private Function GetBandArrangementId(ByVal sXML As String, ByVal sBandKey As String) As Integer
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(sXML)
            Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKey & "']/ArrangementRow[@RIarrangementKey!='0']")
            If oNode IsNot Nothing AndAlso oNode.Attributes("RIarrangementKey") IsNot Nothing Then
                Dim iKey As Integer = 0
                Integer.TryParse(oNode.Attributes("RIarrangementKey").Value, iKey)
                Return iKey
            End If
            Return 0
        End Function

        Private Sub ApplyOrgEditedRowHighlighting()
            If Session(CNRIXMLData) Is Nothing OrElse String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLData))) Then Exit Sub
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(Convert.ToString(Session(CNRIXMLData)))
            Dim sBandKey As String = Convert.ToString(Session(CNRIBandKey)).Replace("'", "''").Trim()
            ' Original_ rows never have IsEditedDB stamped (column absent from dtOrgArrangements).
            ' Match by TreatyCode to the corresponding Current_ node which does carry IsEditedDB/IsUserEdited.
            Dim oOrgNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Original_" & sBandKey & "']/ArrangementRow")
            For iRow As Integer = 0 To gvOrgReinsurance.Rows.Count - 1
                If oOrgNodes Is Nothing OrElse iRow >= oOrgNodes.Count Then Continue For
                Dim oOrgNode As XmlNode = oOrgNodes(iRow)
                ' Get TreatyCode from the Original_ row to find the matching Current_ row
                Dim sTreatyCode As String = ""
                If oOrgNode.Attributes("TreatyCode") IsNot Nothing Then
                    sTreatyCode = oOrgNode.Attributes("TreatyCode").Value
                End If
                Dim bIsEdited As Boolean = False
                If Not String.IsNullOrEmpty(sTreatyCode) Then
                    Dim oCurrentNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sBandKey & "']/ArrangementRow[@TreatyCode=" & XPathLiteral(sTreatyCode) & " and @IsDeleted='False']")
                    If oCurrentNode IsNot Nothing Then
                        'IsUserEdited commented out - using IsEditedDB
                        If oCurrentNode.Attributes("IsEditedDB") IsNot Nothing Then Boolean.TryParse(oCurrentNode.Attributes("IsEditedDB").Value, bIsEdited)
                        If Not bIsEdited AndAlso oCurrentNode.Attributes("IsEditedDB") IsNot Nothing Then
                            Boolean.TryParse(oCurrentNode.Attributes("IsEditedDB").Value, bIsEdited)
                        End If
                    End If
                End If
                If bIsEdited Then
                    Dim sCss As String = gvOrgReinsurance.Rows(iRow).CssClass
                    If Not sCss.Contains("ri-edited-row") Then
                        sCss &= " ri-edited-row" 'If(String.IsNullOrEmpty(sCss), "ri-edited-row", sCss & " ri-edited-row")
                    End If

                    gvOrgReinsurance.Rows(iRow).CssClass = sCss

                End If
            Next
        End Sub

        Private Sub ApplyEditedRowHighlighting()
            If Session(CNRIXMLData) Is Nothing OrElse String.IsNullOrEmpty(Convert.ToString(Session(CNRIXMLData))) Then Exit Sub
            Dim oXMLDoc As New XmlDocument()
            oXMLDoc.LoadXml(Convert.ToString(Session(CNRIXMLData)))
            Dim sBandKey As String = Convert.ToString(Session(CNRIBandKey)).Trim()
            Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name=" & XPathLiteral("Current_" & sBandKey) & "]/ArrangementRow[@IsDeleted='False']")
            For iRow As Integer = 0 To gvReinsurance.Rows.Count - 1
                ' PBI 35359: Use IsUserEdited (DB-persisted user edit flag, snapshotted before ProcessMTA)
                ' when available (MTA/Renewal load). Fall back to IsEdited for NB/inline edits.
                Dim bIsEdited As Boolean = False, bIsDBEdited As Boolean = False
                If oNodes IsNot Nothing AndAlso iRow < oNodes.Count Then
                    Dim oRowNode As XmlNode = oNodes(iRow)
                    'If oRowNode.Attributes("IsUserEdited") IsNot Nothing Then
                    'Boolean.TryParse(oRowNode.Attributes("IsUserEdited").Value, bIsEdited)
                    'Else
                    'Boolean.TryParse(gvReinsurance.Rows(iRow).Cells(1).Text, bIsEdited)
                    'End If
                    If oRowNode.Attributes("IsEditedDB") IsNot Nothing Then
                        Boolean.TryParse(oRowNode.Attributes("IsEditedDB").Value, bIsDBEdited)
                    Else
                        Boolean.TryParse(gvReinsurance.Rows(iRow).Cells(1).Text, bIsDBEdited)
                    End If
                Else
                    Boolean.TryParse(gvReinsurance.Rows(iRow).Cells(1).Text, bIsDBEdited)
                End If
                Dim bIsManuallyAdded As Boolean = False
                Dim sRowType As String = ""
                If oNodes IsNot Nothing AndAlso iRow < oNodes.Count Then
                    If oNodes(iRow).Attributes("ManuallyAdded") IsNot Nothing Then
                        Boolean.TryParse(oNodes(iRow).Attributes("ManuallyAdded").Value, bIsManuallyAdded)
                    End If
                    If oNodes(iRow).Attributes("Type") IsNot Nothing Then
                        sRowType = oNodes(iRow).Attributes("Type").Value
                    End If
                End If
                ' Bug 38298: FAC Prop (F) and FAC XOL (FX) lines must never receive the edited-row highlight
                Dim bIsFACLine As Boolean = (sRowType = "F" OrElse sRowType = "FX" OrElse sRowType = "PX")
                Dim bIsRetained As Boolean = (sRowType = "R")
                Dim sCss As String = gvReinsurance.Rows(iRow).CssClass
                If bIsManuallyAdded AndAlso Not bIsFACLine AndAlso Not sCss.Contains("ri-edited-row") Then
                    sCss &= " ri-edited-row"
                End If
                If (bIsEdited OrElse bIsDBEdited) AndAlso Not bIsFACLine AndAlso Not sCss.Contains("ri-edited-row") Then
                    ' For R rows: remove ri-retained-row and apply ri-edited-row instead
                    If bIsRetained Then sCss = sCss.Replace("ri-retained-row", "").Trim()
                    sCss = If(String.IsNullOrEmpty(sCss), "ri-edited-row", sCss & " ri-edited-row")
                ElseIf Not (bIsEdited OrElse bIsDBEdited) AndAlso Not bIsManuallyAdded Then
                    ' R row reverted — remove ri-edited-row, restore ri-retained-row
                    sCss = sCss.Replace("ri-edited-row", "").Trim()
                    If bIsRetained AndAlso Not sCss.Contains("ri-retained-row") Then sCss &= " ri-retained-row"
                End If
                gvReinsurance.Rows(iRow).CssClass = sCss.Trim()
            Next
            ScriptManager.RegisterStartupScript(Me.UpdatePanelReinsurance, Me.GetType, "reapplyPercentage", "configPercentage('input:text[class*=""PercTextBox""]', '#.0000%%');", True)
        End Sub
        Private Function GetTreatyCodeFromGridRow(ByVal sXML As String, ByVal sBandKey As String, ByVal gridRow As GridViewRow) As String
            Dim oXMLDoc As New XmlDocument
            oXMLDoc.LoadXml(sXML)
            Dim sanitizedBandKey As String = sBandKey.Replace("'", "''").Trim()
            Dim nodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sanitizedBandKey & "']/ArrangementRow[@IsDeleted='False']")
            If nodes IsNot Nothing AndAlso gridRow.RowIndex < nodes.Count Then
                Dim oNode As XmlNode = nodes(gridRow.RowIndex)
                If oNode IsNot Nothing AndAlso oNode.Attributes("TreatyCode") IsNot Nothing Then
                    Return oNode.Attributes("TreatyCode").Value
                End If
            End If
            Return ""
        End Function
        ''' <summary>
        ''' Builds a safe XPath string literal for a value that may contain single quotes.
        ''' XPath 1.0 has no escape sequence for apostrophes inside single-quoted literals;
        ''' SQL-style doubling ('') is NOT valid XPath and causes XPathException at runtime.
        ''' Uses concat() when the value contains a single quote, plain quotes otherwise.
        ''' </summary>
        Private Shared Function XPathLiteral(ByVal value As String) As String
            If Not value.Contains("'") Then
                Return "'" & value & "'"
            End If
            ' Split on apostrophes and reassemble with concat()
            Dim parts As String() = value.Split("'"c)
            Dim quotedParts As New System.Text.StringBuilder("concat(")
            For i As Integer = 0 To parts.Length - 1
                If i > 0 Then
                    quotedParts.Append(", ""'"", ")
                End If
                quotedParts.Append("'")
                quotedParts.Append(parts(i))
                quotedParts.Append("'")
            Next
            quotedParts.Append(")")
            Return quotedParts.ToString()
        End Function

    End Class
End Namespace
