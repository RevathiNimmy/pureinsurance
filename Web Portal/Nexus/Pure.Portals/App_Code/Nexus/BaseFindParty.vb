Imports CMS.library
Imports Nexus.Library
Imports SiriusFS.SAM.Client
Imports System.Xml.XPath
Imports System.Web.HttpContext
'Imports Nexus.Library.Config
'Imports CMS.Library.Portal
Imports System.Xml
Imports System.Globalization.CultureInfo
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Constants.Constant

Namespace Nexus

    Public Class BaseFindParty : Inherits CMS.Library.Frontend.clsCMSPage

        Private oMaster As ContentPlaceHolder
        Dim oPersonalClientControl As Object

        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Protected Sub FindParty(Optional ByVal sSearchType As String = "", Optional ByVal v_bIncludeAgent As Boolean = Nothing, Optional ByVal bIsAnySelected As Boolean = False)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPartySearchCriteria As NexusProvider.PartySearchCriteria
            Dim oPartyCollection As NexusProvider.PartyCollection

            oPartySearchCriteria = New NexusProvider.PartySearchCriteria()
            oPartySearchCriteria.PartyType = Session(CNSearchType)

            If Session(CNSearchAgentType) IsNot Nothing Then
                If Session(CNSearchAgentType) = PartyAgentType.All Then
                    oPartySearchCriteria.AgentType = Nothing
                Else
                    oPartySearchCriteria.AgentType = Session(CNSearchAgentType)
                End If
            End If
            If Session(CNSearchType) = PartyType.PC Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.PC)
            ElseIf Session(CNSearchType) = PartyType.CC Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.CC)
            ElseIf Session(CNSearchType) = PartyType.GC Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.GC)
                oPartySearchCriteria.IsAnySelected = bIsAnySelected

            ElseIf Session(CNSearchType) = PartyType.AG Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)
            ElseIf Session(CNSearchType) = PartyType.AH Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AH)
            ElseIf Session(CNSearchType) = PartyType.INS Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.IN)
            ElseIf Session(CNSearchType) = PartyType.OTSOL Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTSOL)
            ElseIf Session(CNSearchType) = PartyType.OTDOCTOR Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTDOCTOR)
            ElseIf Session(CNSearchType) = PartyType.OTDRIVER Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTDRIVER)
            ElseIf Session(CNSearchType) = PartyType.OTLOSS Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTLOSS)
            ElseIf Session(CNSearchType) = PartyType.OTREPAIRER Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTREPAIRER)
            ElseIf Session(CNSearchType) = PartyType.OTSUPPLIER Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTSUPPLIER)
            ElseIf Session(CNSearchType) = PartyType.OTSURVEYOR Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTSURVEYOR)
            ElseIf Session(CNSearchType) = PartyType.OTTHIRD Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTTHIRD)
            ElseIf Session(CNSearchType) = PartyType.OTWITNESS Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTWITNESS)
            ElseIf Session(CNSearchType) = PartyType.OTOTHERPARTY Then
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTOTHERPARTY)
                oPartySearchCriteria.OtherPartyTypeCode = Session(CNSearchOtherPartyTypeCode)
            ElseIf Session(CNSearchType) = PartyType.AllClient Then
                If oMaster.FindControl("ddlPartyType") IsNot Nothing AndAlso CType(oMaster.FindControl("ddlPartyType"), DropDownList).Visible = True Then
                    Dim ddlPartyType As DropDownList = oMaster.FindControl("ddlPartyType")

                    For iCount As Integer = 0 To ddlPartyType.Items.Count - 1
                        If ddlPartyType.Items(iCount).Value.ToUpper = NexusProvider.PartyTypeType.OTSOL.ToString Then
                            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTSOL)
                        ElseIf ddlPartyType.Items(iCount).Value.ToUpper = NexusProvider.PartyTypeType.OTDOCTOR.ToString Then
                            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTDOCTOR)
                        ElseIf ddlPartyType.Items(iCount).Value.ToUpper = NexusProvider.PartyTypeType.OTDRIVER.ToString Then
                            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTDRIVER)
                        ElseIf ddlPartyType.Items(iCount).Value.ToUpper = NexusProvider.PartyTypeType.OTLOSS.ToString Then
                            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTLOSS)
                        ElseIf ddlPartyType.Items(iCount).Value.ToUpper = NexusProvider.PartyTypeType.OTREPAIRER.ToString Then
                            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTREPAIRER)
                        ElseIf ddlPartyType.Items(iCount).Value.ToUpper = NexusProvider.PartyTypeType.OTSUPPLIER.ToString Then
                            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTSUPPLIER)
                        ElseIf ddlPartyType.Items(iCount).Value.ToUpper = NexusProvider.PartyTypeType.OTSUPPLIER.ToString Then
                            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTSUPPLIER)
                        ElseIf ddlPartyType.Items(iCount).Value.ToUpper = NexusProvider.PartyTypeType.OTSURVEYOR.ToString Then
                            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTSURVEYOR)
                        ElseIf ddlPartyType.Items(iCount).Value.ToUpper = NexusProvider.PartyTypeType.OTTHIRD.ToString Then
                            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTTHIRD)
                        ElseIf ddlPartyType.Items(iCount).Value.ToUpper = NexusProvider.PartyTypeType.OTWITNESS.ToString Then
                            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTWITNESS)
                        End If
                    Next
                Else
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTSOL)
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTDOCTOR)
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTDRIVER)
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTLOSS)
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTREPAIRER)
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTSUPPLIER)
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTSURVEYOR)
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTTHIRD)
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTWITNESS)
                End If

            End If
            If v_bIncludeAgent Then
                oPartySearchCriteria.IncludeAgent = v_bIncludeAgent
            End If
            Try
                Dim oAddress As New NexusProvider.Address
                For Each oControl As Control In oMaster.Controls
                    Select Case oControl.GetType.Name.Trim.ToUpper
                        Case "PANEL"
                            For Each oPanelControl As Control In oControl.Controls
                                Select Case oPanelControl.ID

                                    Case "txtAccounthandler_code", "txtAgent_code", "txtReinsurerCode", "txtClientCode", "txtPartyCode"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.ShortName = CType(oPanelControl, TextBox).Text.Trim()
                                        End If

                                    Case "txtFileCode"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.FileCode = CType(oPanelControl, TextBox).Text.Trim()
                                        End If

                                    Case "txtPhone"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.TelephoneNumber = CType(oPanelControl, TextBox).Text.Trim()
                                        End If

                                    Case "txtDOB"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.DateOfBirth = CType(CType(oPanelControl, TextBox).Text, Date)
                                        End If

                                    Case "txtPostcode"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oAddress.PostCode = CType(oPanelControl, TextBox).Text.Trim()
                                            oPartySearchCriteria.Address = oAddress
                                            oAddress = Nothing
                                        End If



                                    Case "txtName", "txtClientName", "txtPartyName"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.Name = CType(oPanelControl, TextBox).Text.Trim()
                                        End If

                                    Case "txtRiskIndex", "txtPolicyRiskIndex"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.RiskIndex = CType(oPanelControl, TextBox).Text.Trim()
                                        End If

                                    Case "txtPartyIndex"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.PartyIndex = CType(oPanelControl, TextBox).Text.Trim()
                                        End If

                                    Case "txtClaimRiskIndex"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.ClaimsRiskIndex = CType(oPanelControl, TextBox).Text.Trim()
                                        End If

                                    Case "chkIncludeClosedBranches"
                                        oPartySearchCriteria.IncludeClosedBranches = CType(oPanelControl, CheckBox).Checked

                                    Case "txtStatus"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.Status = CType(oPanelControl, TextBox).Text.Trim()
                                        End If

                                    Case "txtAddress"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oAddress.Address1 = CType(oPanelControl, TextBox).Text.Trim()
                                            oPartySearchCriteria.Address = oAddress
                                            oAddress = Nothing
                                        End If

                                    Case "txtPolicyNumber"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.PolicyRef = CType(oPanelControl, TextBox).Text.Trim()
                                        End If

                                    Case "txtClaimNumber"
                                        If Not CType(oPanelControl, TextBox).Text.Trim().Length = 0 Then
                                            oPartySearchCriteria.ClaimNumber = CType(oPanelControl, TextBox).Text.Trim()
                                        End If
                                    Case "ddlStatus"
                                        If Not CType(oPanelControl, DropDownList).SelectedValue.Trim().Length = 0 Then
                                            oPartySearchCriteria.Status = CType(oPanelControl, DropDownList).SelectedValue.Trim()
                                        End If
                                    Case "ddlAgentGroup"
                                        If Not CType(oPanelControl, DropDownList).SelectedValue.Trim().Length = 0 Then
                                            oPartySearchCriteria.AgentGroupCode = CType(oPanelControl, DropDownList).SelectedValue.Trim()
                                        End If
                                    Case "liCaseNumber"
                                        For Each oPanelChildControl As Control In oPanelControl.Controls
                                            Select Case oPanelChildControl.ID
                                                Case "txtCaseNumber"
                                                    If Not CType(oPanelChildControl, TextBox).Text.Trim().Length = 0 Then
                                                        oPartySearchCriteria.CaseNumber = CType(oPanelChildControl, TextBox).Text.Trim()
                                                    End If
                                            End Select
                                        Next
                                End Select
                            Next
                        Case Else
                            Select Case oControl.ID

                                Case "txtAccounthandler_code", "txtAgent_code", "txtReinsurerCode", "txtClientCode", "txtPartyCode"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.ShortName = CType(oControl, TextBox).Text.Trim()
                                    End If

                                Case "txtFileCode"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.FileCode = CType(oControl, TextBox).Text.Trim()
                                    End If

                                Case "txtPhone"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.TelephoneNumber = CType(oControl, TextBox).Text.Trim()
                                    End If

                                Case "txtDOB"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.DateOfBirth = CType(CType(oControl, TextBox).Text, Date)
                                    End If

                                Case "txtPostcode"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oAddress.PostCode = CType(oControl, TextBox).Text.Trim()
                                    End If

                                Case "txtName", "txtClientName", "txtPartyName"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.Name = CType(oControl, TextBox).Text.Trim()
                                    End If
                                Case "txtRiskIndex", "txtPolicyRiskIndex"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.RiskIndex = CType(oControl, TextBox).Text.Trim()
                                    End If
                                Case "txtPartyIndex"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.PartyIndex = CType(oControl, TextBox).Text.Trim()
                                    End If
                                Case "txtClaimRiskIndex"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.ClaimsRiskIndex = CType(oControl, TextBox).Text.Trim()
                                    End If

                                Case "chkIncludeClosedBranches"
                                    oPartySearchCriteria.IncludeClosedBranches = CType(oControl, CheckBox).Checked

                                Case "txtStatus"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.Status = CType(oControl, TextBox).Text.Trim()
                                    End If

                                Case "txtAddress"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then

                                        oAddress.Address1 = CType(oControl, TextBox).Text.Trim()
                                    End If

                                Case "txtPolicyNumber"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.PolicyRef = CType(oControl, TextBox).Text.Trim()
                                    End If

                                Case "txtClaimNumber"
                                    If Not CType(oControl, TextBox).Text.Trim().Length = 0 Then
                                        oPartySearchCriteria.ClaimNumber = CType(oControl, TextBox).Text.Trim()
                                    End If
                                Case "ddlStatus"
                                    If Not CType(oControl, DropDownList).SelectedValue.Trim().Length = 0 Then
                                        oPartySearchCriteria.Status = CType(oControl, DropDownList).SelectedValue.Trim()
                                    End If
                                Case "ddlAgentGroup"
                                    If Not CType(oControl, DropDownList).SelectedValue.Trim().Length = 0 Then
                                        oPartySearchCriteria.AgentGroupCode = CType(oControl, DropDownList).SelectedValue.Trim()
                                    End If
                                Case "liCaseNumber"
                                    For Each oChildControl As Control In oControl.Controls
                                        Select Case oChildControl.ID
                                            Case "txtCaseNumber"
                                                If Not CType(oChildControl, TextBox).Text.Trim().Length = 0 Then
                                                    oPartySearchCriteria.CaseNumber = CType(oChildControl, TextBox).Text.Trim()
                                                End If
                                        End Select
                                    Next

                            End Select
                    End Select
                Next

                'to limit the search return from SAM
                oPartySearchCriteria.MaxRowsToFetch = oPortal.MaxSearchResults

                ' ADO-39411: Cross-Branch Client Search
                ' When system option 5246 is enabled, bypass branch filter for client search
                Dim sBranchCode As String = Session(CNTransBranchCode)
                Dim oCrossBranchOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ClientSearchIndependentToBranchAccess)
                If oCrossBranchOption IsNot Nothing AndAlso Not String.IsNullOrEmpty(oCrossBranchOption.OptionValue) AndAlso oCrossBranchOption.OptionValue <> "0" Then
                    sBranchCode = Nothing
                End If

                oPartyCollection = oWebService.FindParty(oPartySearchCriteria, sBranchCode, sSearchType:=sSearchType)

                ' storing search result in session
                If Session(CNSearchData) IsNot Nothing Then
                    Session(CNSearchData) = Nothing
                End If

                If Request IsNot Nothing AndAlso Request.Path.ToString.ToLower.Contains("findclient.aspx") Then
                    If Not (Request.Path.ToString.ToLower.Contains("modal/findclient.aspx")) Then
                        If Session(CNParty) IsNot Nothing Then
                            Session(CNParty) = Nothing
                        End If
                    End If
                End If

                Dim oTempPartyColl As New NexusProvider.PartyCollection
                If oPartyCollection.Count > 0 Then
                    Dim iAnonPartyID As Integer = CType(oPortal.AnnPartyID, Integer)
                    For iCount As Integer = 0 To oPartyCollection.Count - 1
                        If oPartyCollection(iCount).Key <> iAnonPartyID Then
                            oTempPartyColl.Add(oPartyCollection(iCount))
                        End If
                    Next

                    Session(CNSearchData) = oTempPartyColl
                End If

                ' populating the data in datagrid 
                PopulateGrid(oTempPartyColl)

                'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
                If oPartyCollection.Count >= oPortal.MaxSearchResults Then
                    'create a custom validator
                    Dim cstMaxResults As New CustomValidator
                    cstMaxResults.IsValid = False
                    'look for a validation message in the page resources, but if there is not one defined add a default message
                    cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                    cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    'add the validator to the page, this will have the effect of making the page invalid
                    Page.Validators.Add(cstMaxResults)
                End If

            Catch ex As Exception

            Finally

            End Try

        End Sub


        ''' <summary>
        ''' Populating the datagrid with respective of the ID in the MainPage
        ''' </summary>
        ''' <param name="oPartyCollection"></param>
        ''' <remarks></remarks>
        Protected Sub PopulateGrid(ByVal oPartyCollection As NexusProvider.PartyCollection)

            Try

                ' Obtaining the main container from the page
                oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                ' Temporary DataGrid
                Dim TempDataGrid As GridView
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

                Dim iAnonPartyID As Integer = CType(oPortal.AnnPartyID, Integer)
                Dim oTempPartyColl As New NexusProvider.PartyCollection


                If oPartyCollection IsNot Nothing Then
                    If Convert.ToString(Request.UrlReferrer).Contains("FindAgent") Then
                        If Request.QueryString("AgentType") = "SubAgent" Then
                            For iCount As Integer = 0 To oPartyCollection.Count - 1
                                If oPartyCollection(iCount).AgentType.Trim.ToUpper = "SUB-AGENT" And oPartyCollection(iCount).Key <> iAnonPartyID Then
                                    oTempPartyColl.Add(oPartyCollection(iCount))
                                End If
                            Next
                            Session(CNSearchData) = oTempPartyColl
                        ElseIf Request.QueryString("FromPage") = "MainDetails" Then
                            For iCount As Integer = 0 To oPartyCollection.Count - 1
                                If oPartyCollection(iCount).AgentType.Trim.ToUpper <> "SUB-AGENT" And oPartyCollection(iCount).Key <> iAnonPartyID Then
                                    oTempPartyColl.Add(oPartyCollection(iCount))
                                End If
                            Next
                            Session(CNSearchData) = oTempPartyColl
                        Else
                            For iCount As Integer = 0 To oPartyCollection.Count - 1
                                If oPartyCollection(iCount).Key <> iAnonPartyID Then
                                    oTempPartyColl.Add(oPartyCollection(iCount))
                                End If
                            Next
                            Session(CNSearchData) = oTempPartyColl
                        End If
                        For Each oControl As Control In oMaster.Controls
                            Select Case oControl.GetType.Name.ToUpper
                                Case "UPDATEPANEL"
                                    Dim oUpdPanel As UpdatePanel = CType(oControl, UpdatePanel)
                                    If oUpdPanel.HasControls Then
                                        For Each oChildControl As Control In oUpdPanel.Controls(0).Controls
                                            Select Case oChildControl.ID
                                                Case "grdvSearchResults"
                                                    ' converting selected control to datagrid
                                                    TempDataGrid = CType(oChildControl, GridView)
                                                    TempDataGrid.Visible = True
                                                    TempDataGrid.AllowPaging = True
                                                    ' assigning datasource to grid
                                                    TempDataGrid.DataSource = oTempPartyColl
                                                    ' binding data to grid
                                                    TempDataGrid.DataBind()
                                            End Select
                                        Next
                                    End If
                            End Select
                            ' Selecting functionalities related to control ID
                            Select Case oControl.ID
                                Case "grdvSearchResults"
                                    ' converting selected control to datagrid
                                    TempDataGrid = CType(oControl, GridView)
                                    TempDataGrid.Visible = True
                                    TempDataGrid.AllowPaging = True
                                    ' assigning datasource to grid
                                    TempDataGrid.DataSource = oTempPartyColl
                                    ' binding data to grid
                                    TempDataGrid.DataBind()
                            End Select
                        Next
                    Else
                        For Each oControl As Control In oMaster.Controls
                            Select Case oControl.GetType.Name.ToUpper
                                Case "UPDATEPANEL"
                                    Dim oUpdPanel As UpdatePanel = CType(oControl, UpdatePanel)
                                    If oUpdPanel.HasControls Then
                                        For Each oChildControl As Control In oUpdPanel.Controls(0).Controls
                                            Select Case oChildControl.ID
                                                Case "grdvSearchResults"
                                                    ' converting selected control to datagrid
                                                    TempDataGrid = CType(oChildControl, GridView)
                                                    For iCount As Integer = 0 To oPartyCollection.Count - 1
                                                        If oPartyCollection(iCount).Key <> iAnonPartyID Then
                                                            oTempPartyColl.Add(oPartyCollection(iCount))
                                                        End If
                                                    Next

                                                    TempDataGrid.AllowPaging = True
                                                    TempDataGrid.Visible = True
                                                    ' assigning datasource to grid
                                                    TempDataGrid.DataSource = oTempPartyColl
                                                    ' binding data to grid
                                                    TempDataGrid.DataBind()
                                            End Select
                                        Next
                                    End If
                            End Select
                            ' Selecting functionalities related to control ID
                            Select Case oControl.ID
                                Case "grdvSearchResults"
                                    ' converting selected control to datagrid
                                    TempDataGrid = CType(oControl, GridView)
                                    For iCount As Integer = 0 To oPartyCollection.Count - 1
                                        If oPartyCollection(iCount).Key <> iAnonPartyID Then
                                            oTempPartyColl.Add(oPartyCollection(iCount))
                                        End If
                                    Next

                                    TempDataGrid.AllowPaging = True
                                    TempDataGrid.Visible = True
                                    ' assigning datasource to grid
                                    TempDataGrid.DataSource = oTempPartyColl
                                    ' binding data to grid
                                    TempDataGrid.DataBind()
                            End Select
                        Next
                    End If
                End If

            Catch ex As Exception

            Finally

            End Try

        End Sub
        ''' <summary>
        ''' To Find Agent Groups
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function GetAgentGroups(Optional ByVal sBranchCode As String = "") As NexusProvider.PartyCollection

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPartySearchCriteria As NexusProvider.PartySearchCriteria
            Dim oAgentGroups As NexusProvider.PartyCollection

            oPartySearchCriteria = New NexusProvider.PartySearchCriteria()

            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AGG)
            oPartySearchCriteria.PartyType = PartyType.AGG

            If sBranchCode <> "" Then
                oAgentGroups = oWebService.FindParty(oPartySearchCriteria, sBranchCode)
            Else
                oAgentGroups = oWebService.FindParty(oPartySearchCriteria)
            End If

            Return oAgentGroups

        End Function
        Protected Sub grdvSearchResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

            CType(sender, GridView).PageIndex = e.NewPageIndex
            CType(sender, GridView).DataSource = Session(CNSearchData)
            CType(sender, GridView).DataBind()

        End Sub
        Protected Sub grdvSearchResults_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
            If CType(sender, GridView).Rows.Count = 0 Or CType(sender, GridView).PageCount = 1 Then
                CType(sender, GridView).AllowPaging = False
            End If
        End Sub
        Protected Sub grdvSearchResults_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
            If e.CommandName = "Claim" Then
                If Request("ClaimFlag") = "1" Then
                    Dim sClaimType As String = Request("ClientType")
                    Session(CNClaimShortName) = CStr(e.CommandArgument)
                    Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.refreshPanel('ShortName','" + sClaimType + "');", True)
                End If
            End If
        End Sub

        ''' <summary>
        '''  To find out the party type which need to be searched.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Enum PartyType
            ''' <summary>
            ''' PersonalClient
            ''' </summary>
            ''' <remarks></remarks>

            PC

            ''' <summary>
            ''' GroupClient
            ''' </summary>
            ''' <remarks></remarks>

            GC

            ''' <summary>
            ''' Agent
            ''' </summary>
            ''' <remarks></remarks>

            AG

            ''' <summary>
            ''' CorporateClient
            ''' </summary>
            ''' <remarks></remarks>
            CC

            ''' <summary>
            ''' Consultant
            ''' </summary>
            ''' <remarks></remarks>

            CO

            ''' <summary>
            ''' AccountHandler
            ''' </summary>
            ''' <remarks></remarks>
            AH

            ''' <summary>
            ''' Insurer
            ''' </summary>
            ''' <remarks></remarks>
            INS

            ''' <summary>
            ''' Broker
            ''' </summary>
            ''' <remarks></remarks>
            BR

            ''' <summary>
            ''' FeeAccount
            ''' </summary>
            ''' <remarks></remarks>
            FE

            ''' <summary>
            ''' ExtrasAccount
            ''' </summary>
            ''' <remarks></remarks>
            EX

            ''' <summary>
            ''' DiscountAccount
            ''' </summary>
            ''' <remarks></remarks>
            DI

            ''' <summary>
            ''' CommissionAccount
            ''' </summary>
            ''' <remarks></remarks>
            CM

            ''' <summary>
            ''' NetClient
            ''' </summary>
            ''' <remarks></remarks>
            NC

            ''' <summary>
            ''' Driver
            ''' </summary>
            ''' <remarks></remarks>
            OTDRIVER

            ''' <summary>
            ''' Witness
            ''' </summary>
            ''' <remarks></remarks>
            OTWITNESS

            ''' <summary>
            ''' Repairer
            ''' </summary>
            ''' <remarks></remarks>
            OTREPAIRER

            ''' <summary>
            ''' ThirdParty
            ''' </summary>
            ''' <remarks></remarks>
            OTTHIRD

            ''' <summary>
            ''' FinanceProvider
            ''' </summary>
            ''' <remarks></remarks>
            FP

            ''' <summary>
            ''' AgentGroup
            ''' </summary>
            ''' <remarks></remarks>
            AGG

            ''' <summary>
            ''' Supplier
            ''' </summary>
            ''' <remarks></remarks>
            OTSUPPLIER

            ''' <summary>
            ''' LossAdjuster
            ''' </summary>
            ''' <remarks></remarks>
            OTLOSS

            ''' <summary>
            ''' Solicitor
            ''' </summary>
            ''' <remarks></remarks>
            OTSOL

            ''' <summary>
            ''' Doctor
            ''' </summary>
            ''' <remarks></remarks>
            OTDOCTOR

            ''' <summary>
            ''' BuildingSurveyor
            ''' </summary>
            ''' <remarks></remarks>
            OTSURVEYOR

            ''' <summary>
            ''' ExecutiveHandler
            ''' </summary>
            ''' <remarks></remarks>
            HC

            ''' <summary>
            ''' OTOTHERPARTY
            ''' </summary>
            ''' <remarks></remarks>
            OTOTHERPARTY

            ''' <summary>
            ''' AnyClient
            ''' </summary>
            ''' <remarks></remarks>
            AllClient

            ''' <summary>
            ''' Reassured
            ''' </summary>
            ''' <remarks></remarks>
            OTREASSUR

        End Enum

        Protected Enum PartyAgentType

            ''' <summary>
            ''' Broker
            ''' </summary>
            ''' <remarks></remarks>
            Broker

            ''' <summary>
            ''' SubAgent
            ''' </summary>
            ''' <remarks></remarks>
            SubAgent

            ''' <summary>
            ''' CommissionAccount
            ''' </summary>
            ''' <remarks></remarks>
            CommissionAccount

            ''' <summary>
            ''' Intermediary
            ''' </summary>
            ''' <remarks></remarks>
            Intermediary

            ''' <summary>
            ''' Intermediary
            ''' </summary>
            ''' <remarks></remarks>
            All


        End Enum

    End Class

End Namespace
