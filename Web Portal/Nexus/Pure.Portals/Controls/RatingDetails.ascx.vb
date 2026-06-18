Imports System.Xml
Imports nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Data
Imports System.Configuration.ConfigurationManager

Imports Nexus.Library
Imports CMS.Library

Namespace Nexus

    Partial Class Controls_RatingDetails : Inherits System.Web.UI.UserControl

        Dim NavString, AttribString, DataTypeString, ReturnString, ReturnString1, ReturnString2, TotalAttribString, sCurrencySymbolAttributeString As String
        Dim TableRowHeadersString, SeparatorString As String
        Dim AttribNames, HeaderRowNames, ColumnHeaderCSSNames, TotalAttribNames, CurrencySymbolAttributesNames, ColumnCSSNames, LastRowColumnCSSNames As String()
        Dim bShowOriginalPremium As Boolean = False
        Dim dtRatingSection As DataTable
        Dim dSum() As Decimal
        Dim dRatingTax As Decimal

        Public WriteOnly Property AttributeName() As String
            Set(ByVal value As String)
                AttribString = value
            End Set
        End Property

        Public WriteOnly Property DataType() As String
            Set(ByVal value As String)
                DataTypeString = value
            End Set
        End Property

        Public WriteOnly Property TableRowHeaders() As String
            Set(ByVal value As String)
                TableRowHeadersString = value
            End Set
        End Property

        Public WriteOnly Property Separator() As String
            Set(ByVal value As String)
                SeparatorString = value
            End Set
        End Property

        Public WriteOnly Property TotalAttributeName() As String
            Set(ByVal value As String)
                TotalAttribString = value
            End Set
        End Property

        Public WriteOnly Property CurrencySymbolAttributeName() As String
            Set(ByVal value As String)
                sCurrencySymbolAttributeString = value
            End Set
        End Property

        Public Property ShowOriginalPremium() As Boolean
            Get
                Return bShowOriginalPremium
            End Get
            Set(ByVal value As Boolean)
                bShowOriginalPremium = value
            End Set
        End Property

        ''' <summary>
        ''' Get the Rating Details in a Collection
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim InsuranceFileKey As String = ""
            Dim RiskKey As String = ""
            Dim oRatingCollection As NexusProvider.RatingCollection
            Dim oWebService As NexusProvider.ProviderBase

            If Session(CNMode) = Mode.View Or CType(Session(CNMode), Mode) = Mode.Review Then
                btnAdd.Visible = False
            End If

            If Me.Visible = True Then

                If Session(CNQuote) IsNot Nothing And CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).XMLDataset IsNot Nothing Then

                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)

                    oWebService = New NexusProvider.ProviderManager().Provider
                    InsuranceFileKey = oQuote.InsuranceFileKey
                    RiskKey = oQuote.Risks(Session(CNCurrentRiskKey)).Key

                    Dim RiskTypeCode As String = oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim
                    Try
                        If Not IsPostBack Then ' Or oRatingCollection Is Nothing Then
                            If Session(CNRatingSections) IsNot Nothing Then
                                oRatingCollection = CType(Session(CNRatingSections), NexusProvider.RatingCollection)
                            Else
                                oRatingCollection = oWebService.GetRatingDetails(RiskKey, InsuranceFileKey)
                                Session(CNRatingSections) = oRatingCollection
                            End If
                            PopulateData(oRatingCollection:=oRatingCollection)
                        Else
                            If Request("__EVENTARGUMENT") = "RefreshRatingGrid" Then
                                Page.ClientScript.GetPostBackEventReference(Me, "")
                                oRatingCollection = oWebService.GetRatingDetails(RiskKey, InsuranceFileKey)
                                Session(CNRatingSections) = oRatingCollection
                                dSum = Nothing
                            Else
                                oRatingCollection = CType(Session(CNRatingSections), NexusProvider.RatingCollection)
                                If oRatingCollection Is Nothing Then
                                    oRatingCollection = oWebService.GetRatingDetails(RiskKey, InsuranceFileKey)
                                    Session(CNRatingSections) = oRatingCollection
                                End If
                            End If
                            PopulateData(oRatingCollection:=oRatingCollection)
                        End If

                    Finally
                        oWebService = Nothing
                        oRatingCollection = Nothing
                    End Try
                End If
            End If
        End Sub
        ''' <summary>
        ''' Get the User Authotity 
        ''' </summary>
        ''' <param name="oRatingCollection"></param>
        ''' <remarks></remarks>
        Private Sub PopulateData(ByVal oRatingCollection As NexusProvider.RatingCollection)


            Dim oWebService As NexusProvider.ProviderBase
            Dim oUserAuthority As New NexusProvider.UserAuthority
            Dim bUserAuthorityAllowAddRemoveRatingSections As Boolean, bUserAuthorityAllowEditRatingSections As Boolean
            Dim bProductRiskAllowAddRatingSection, bProductRiskAllowEditRatingSection, bProductRiskAllowDeleteRatingSection As Boolean
            Dim bEditRatingsectionAllowed As Boolean = False, bAddRatingSectionAllowed As Boolean = False, bDeleteRatingSectionAllowed As Boolean = False
            Dim sProductRiskOption As String
            Dim gvTemplateField As TemplateField = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim nTempVar1 As Integer = 0

            Dim InsuranceFileKey As String = oQuote.InsuranceFileKey
            Dim RiskKey As String = oQuote.Risks(Session(CNCurrentRiskKey)).Key
            Dim RiskTypeCode As String = oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim

            Dim NoOfAttributes As Integer = 0, TempVar As Integer = 0
            Dim TotalNoOfRecords As Integer = 0, TotalPremium As Double = 0.0
            Dim i As Integer, iFlag As Integer = 0
            Dim oOptionType As New NexusProvider.OptionTypeSetting

            If Session(CNMode) <> Mode.View Then ' In View Mode Donot show the Add/Edit/Delete buttons


                'Get the User Authority for Rating Sections
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AllowAddRemoveRatingSections

                Try
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oWebService.GetUserAuthorityValue(oUserAuthority)
                    If oUserAuthority.UserAuthorityValue = "1" Then
                        bUserAuthorityAllowAddRemoveRatingSections = True
                    End If

                    oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.AllowEditRatingSections
                    oWebService.GetUserAuthorityValue(oUserAuthority)
                    If oUserAuthority.UserAuthorityValue = "1" Then
                        bUserAuthorityAllowEditRatingSections = True
                    End If

                    'Get the Product Risk Option For Rating Sections
                    'To Check whether the AddRatingSection is allowed at Product Risk Option
                    sProductRiskOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.AllowRatingSectionAdd, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, RiskTypeCode)
                    If sProductRiskOption = "1" Then
                        bProductRiskAllowAddRatingSection = True
                    End If
                    sProductRiskOption = ""
                    'To Check whether the EditRatingSection is allowed at Product Risk Option
                    sProductRiskOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.AllowRatingSectionEdit, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, RiskTypeCode)
                    If sProductRiskOption = "1" Then
                        bProductRiskAllowEditRatingSection = True
                    End If
                    sProductRiskOption = ""
                    'To Check whether the deleteRatingSection is allowed at Product Risk Option
                    sProductRiskOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.AllowRatingSectionDelete, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, RiskTypeCode)
                    If sProductRiskOption = "1" Then
                        bProductRiskAllowDeleteRatingSection = True
                    End If
                    'To Get the value for 6dp is on
                    oOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 106)
                Catch
                Finally
                    oWebService = Nothing
                End Try
                'Decide whether the Add/Delete/Edit button are allowed or not( based on User Authority and Risk Type Options)
                If bProductRiskAllowDeleteRatingSection = True And bUserAuthorityAllowAddRemoveRatingSections = True Then
                    bDeleteRatingSectionAllowed = True
                End If
                If bProductRiskAllowAddRatingSection = True And bUserAuthorityAllowAddRemoveRatingSections = True Then
                    bAddRatingSectionAllowed = True
                End If
                If bProductRiskAllowEditRatingSection = True And bUserAuthorityAllowEditRatingSections = True Then
                    bEditRatingsectionAllowed = True
                End If
                If btnAdd.Enabled = True Then
                    If HttpContext.Current.Session.IsCookieless Then
                        btnAdd.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/RatingSectionDetails.aspx?Mode=Add" & "',null);return false;"
                    Else
                        btnAdd.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "/Modal/RatingSectionDetails.aspx?Mode=Add&modal=true" & "',null);return false;"
                    End If
                End If
                If bAddRatingSectionAllowed = False Then
                    btnAdd.Visible = False
                End If
                If (Session(CNMTAType) IsNot Nothing And ShowOriginalPremium = False) Then
                    btnAdd.Visible = False
                Else
                    'Show the Edit/Delete Fields if Allowed
                    If bEditRatingsectionAllowed Or bDeleteRatingSectionAllowed Then
                        gvTemplateField = New TemplateField()
                        If bDeleteRatingSectionAllowed And bEditRatingsectionAllowed Then
                            gvTemplateField.ItemTemplate = New GridViewTemplate(ListItemType.Item, GetLocalResourceObject("lnk_Edit"), GetLocalResourceObject("lnk_Delete"))
                        ElseIf bEditRatingsectionAllowed = True Then
                            gvTemplateField.ItemTemplate = New GridViewTemplate(ListItemType.Item, GetLocalResourceObject("lnk_Edit"), "")
                        ElseIf bDeleteRatingSectionAllowed = True Then
                            gvTemplateField.ItemTemplate = New GridViewTemplate(ListItemType.Item, "", GetLocalResourceObject("lnk_Delete"))
                        End If
                    End If

                End If
            End If
            'Create the Data table
            CreateDataTable(dtRatingSection)
            grdViewRatingSection.Columns.Clear()

            'split the collection of AttribString and HeaderString
            HeaderRowNames = TableRowHeadersString.Split(",")
            AttribNames = AttribString.Split(",")
            NoOfAttributes = UBound(AttribNames)

            If ShowOriginalPremium Then
                If Session(CNMTAType) IsNot Nothing Then
                    lblRatingDetails.Text = GetLocalResourceObject("lblChangedRatingDetails")
                ElseIf Session(CNViewType) IsNot Nothing Then
                    'During View mode of MTA display ShowOriginal Rating
                    If Session(CNViewType) <> ViewType.POLICY Then
                        lblRatingDetails.Text = GetLocalResourceObject("lblChangedRatingDetails")
                    End If
                End If
            Else
                If Session(CNMTAType) IsNot Nothing Then
                    lblRatingDetails.Text = GetLocalResourceObject("lblOriginalRatingDetails")
                ElseIf Session(CNViewType) IsNot Nothing Then
                    'During View mode of MTA display ShowOriginal Rating
                    If Session(CNViewType) <> ViewType.POLICY Then
                        lblRatingDetails.Text = GetLocalResourceObject("lblOriginalRatingDetails")
                    End If
                End If
            End If

            Dim dr As DataRow

            'This code will check if During MTA then display both the Ratings i.e Original And Changed
            'If Session(CNMTAType) IsNot Nothing Then
            'Fill DataTable with Data 

            If Session(CNMTAType) Is Nothing Then 'NB
                If Session(CNViewType) IsNot Nothing Then
                    'During View mode of MTA display ShowOriginal Rating
                    If Session(CNViewType) <> ViewType.POLICY And ShowOriginalPremium = False Then
                        iFlag = 1
                    ElseIf Session(CNViewType) <> ViewType.POLICY And ShowOriginalPremium = True Then
                        iFlag = 0
                    End If
                ElseIf ShowOriginalPremium = False Then
                    iFlag = 0
                End If
            ElseIf Session(CNMTAType) IsNot Nothing And ShowOriginalPremium = False Then
                iFlag = 1
            ElseIf Session(CNMTAType) IsNot Nothing And ShowOriginalPremium = True Then
                iFlag = 0
            End If
            Dim iCountNewRows As Integer = 0

            'Build the Data Grid from rating Collection
            If oRatingCollection.Count > 0 Then
                For i = 0 To oRatingCollection.Count - 1 Step 1
                    If oRatingCollection.Item(i).OriginalFlag = iFlag Then
                        dr = dtRatingSection.NewRow
                        dr.Item(0) = oRatingCollection.GetValue(i, "RateSectionID")
                        dr.Item(1) = oRatingCollection.GetValue(i, "RatingSectionType")
                        dr.Item(2) = oRatingCollection.GetValue(i, "RATETYPE")
                        dr.Item(3) = oRatingCollection.GetValue(i, "AnnualRate")
                        dr.Item(4) = oRatingCollection.GetValue(i, "SumInsured")
                        dr.Item(5) = oRatingCollection.GetValue(i, "ThisPremium")
                        dr.Item(6) = oRatingCollection.GetValue(i, "AnnualPremium")
                        dtRatingSection.Rows.Add(dr)
                        If Session(CNMTAType) IsNot Nothing And ShowOriginalPremium = True Then
                            iCountNewRows += 1
                        End If
                    End If
                Next

                'BoundField to be created from Nexus.Web.UI.WebControls.BoundField 
                Dim boundFld As Nexus.Web.UI.WebControls.BoundField
                boundFld = New Nexus.Web.UI.WebControls.BoundField
                boundFld.DataField = "RateSectionID"
                grdViewRatingSection.Columns.Add(boundFld)
                Dim oFormatString As Config.FormatString
                'This code will check if During NB then display only Original 
                For TempVar = LBound(AttribNames) To UBound(AttribNames)
                    boundFld = New Nexus.Web.UI.WebControls.BoundField

                    boundFld.DataField = AttribNames(TempVar)
                    boundFld.HeaderText = HeaderRowNames(TempVar)
                    If AttribNames(TempVar) = "SumInsured" Or AttribNames(TempVar) = "ThisPremium" Or AttribNames(TempVar) = "AnnualPremium" Then
                        boundFld.DataType = "Decimal2"
                        oFormatString = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Decimal2")
                        boundFld.ControlStyle.CssClass = oFormatString.ItemStyleCssClass
                    ElseIf AttribNames(TempVar) = "AnnualRate" Then
                        If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
                          AndAlso oOptionType.OptionValue = "1" Then
                            boundFld.DataType = "Decimal6"
                            oFormatString = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Decimal6")
                            boundFld.ControlStyle.CssClass = oFormatString.ItemStyleCssClass
                        Else
                            boundFld.DataType = "Decimal4"
                            oFormatString = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Decimal4")
                            boundFld.ControlStyle.CssClass = oFormatString.ItemStyleCssClass
                        End If

                    End If
                    grdViewRatingSection.Columns.Add(boundFld)
                Next
            End If

            If dtRatingSection.Rows.Count = 0 Then
                grdViewRatingSection.DataSource = Nothing
                grdViewRatingSection.ShowFooter = False
                'Me.Visible = False
            Else
                grdViewRatingSection.DataSource = dtRatingSection
            End If

            'For MTA Donot show the Add/Edit/Delete links for the Original Grid
            If (Session(CNMTAType) IsNot Nothing And ShowOriginalPremium = False) Then
            Else
                If Not gvTemplateField Is Nothing Then
                    grdViewRatingSection.Columns.Add(gvTemplateField)
                End If
            End If
            If oQuote.Risks(Session(CNCurrentRiskKey)).StatusCode <> "DELETED" Then
                For iCount = 0 To oRatingCollection.Count - 1

                    If oRatingCollection.Item(iCount).OriginalFlag = iFlag Then
                        nTempVar1 += 1
                    End If
                Next
                If Session(CNMTAType) IsNot Nothing AndAlso nTempVar1 < 1 AndAlso (Session(CNOnlyOriginalRating) Is Nothing Or Page.IsPostBack = True) Then
                    Me.Visible = False
                    Session(CNOnlyOriginalRating) = "True"
                End If
                If Session(CNOnlyOriginalRating) IsNot Nothing AndAlso Convert.ToString(Session(CNOnlyOriginalRating)) = "True" Then
                    lblRatingDetails.Text = GetLocalResourceObject("lblRatingDetails")
                End If
            End If

            grdViewRatingSection.DataBind()
            If grdViewRatingSection.Columns.Count > 0 Then
                grdViewRatingSection.Columns(0).Visible = False
            End If
        End Sub
        ''' <summary>
        ''' Create the Data Table 
        ''' </summary>
        ''' <param name="DT"></param>
        ''' <remarks></remarks>
        Protected Sub CreateDataTable(ByRef DT As DataTable)

            DT = New DataTable
            DT.Columns.Add("RateSectionID", GetType(Integer))
            DT.Columns.Add("RatingSectionType", GetType(String))
            DT.Columns.Add("RATETYPE", GetType(String))
            DT.Columns.Add("AnnualRate", GetType(Decimal))
            DT.Columns.Add("SumInsured", GetType(Decimal))
            DT.Columns.Add("ThisPremium", GetType(Decimal))
            DT.Columns.Add("AnnualPremium", GetType(Decimal))
        End Sub
        ''' <summary>
        ''' RowCommand Event. 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdViewRatingSection_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdViewRatingSection.RowCommand
            Dim sRatingSectionID As String = ""
            Dim oRating As NexusProvider.Rating = Nothing
            Dim oRatingCollection As NexusProvider.RatingCollection = Nothing

            If e.CommandName = "deleterating" Then
                sRatingSectionID = e.CommandArgument.ToString
                oRatingCollection = CType(Session(CNRatingSections), NexusProvider.RatingCollection)
                oRating = oRatingCollection.GetRatingItem(sRatingSectionID)
                oRatingCollection.Remove(oRating)
                DeleteRatingSection(oRatingCollection:=oRatingCollection)
            End If
            oRatingCollection = Nothing

        End Sub
        ''' <summary>
        ''' Delete the Rating Section. 
        ''' </summary>
        ''' <param name="oRatingCollection"></param>
        ''' <remarks></remarks>
        Private Sub DeleteRatingSection(ByVal oRatingCollection As NexusProvider.RatingCollection)

            Dim oWebService As NexusProvider.ProviderBase
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim InsuranceFileKey As String = oQuote.InsuranceFileKey
            Dim RiskKey As String = oQuote.Risks(Session(CNCurrentRiskKey)).Key
            Dim RiskTypeCode As String = oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim
            Dim bTimeStamp() As Byte
            Dim dTotalFeeTax As Decimal = 0
            Dim dTotalRiskTax As Decimal = 0
            Dim dTotalRiskLevelTax As Decimal = 0

            bTimeStamp = oQuote.TimeStamp

            If Not oRatingCollection Is Nothing Then
                Try
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oWebService.UpdateRatingSections(v_iriskKeyField:=RiskKey, i_InsuranceFileKey:=InsuranceFileKey, r_bTimeStamp:=bTimeStamp, oRatingCollection:=oRatingCollection)
                    oQuote.TimeStamp = bTimeStamp
                    Session(CNQuote) = oQuote
                    'Dim iRiskKey As Integer = Session(CNCurrentRiskKey)
                    '                    oQuote = oWebService.UpdateRiskSelection(r_oQuote:=oQuote, v_iQuoteRiskIndex:=iRiskKey, v_sIsSelected:="1")

                    oRatingCollection = oWebService.GetRatingDetails(RiskKey, InsuranceFileKey)


                    'Get the Fee Total, Net Total, Gross Total to be displayed
                    oWebService.GetHeaderAndRisksByKey(oQuote)

                    Dim oHeaderandRisk As NexusProvider.HeaderAndRisk
                    oHeaderandRisk = oWebService.GetHeaderAndRiskFeesByKey(oQuote.InsuranceFileKey, oQuote.Risks(Session(CNCurrentRiskKey)).Key)
                    For Each oRiskFee As NexusProvider.Fee In oHeaderandRisk.RiskFees
                        dTotalFeeTax = dTotalFeeTax + oRiskFee.TaxAmount
                    Next
                    oHeaderandRisk = Nothing

                    'to get risk taxes
                    Dim oQuoteForRiskTax As NexusProvider.Quote
                    oQuoteForRiskTax = oWebService.GetHeaderAndRiskTaxByKey(oQuote.InsuranceFileKey, oQuote.Risks(Session(CNCurrentRiskKey)).Key)

                    For Each oRiskTax As NexusProvider.Tax In oQuoteForRiskTax.RiskTaxes
                        dTotalRiskTax = dTotalRiskTax + oRiskTax.TaxAmount
                    Next

                    dTotalRiskLevelTax = dTotalRiskTax + dTotalFeeTax

                    Session(CNQuote) = oQuote
                    Dim otxtNetTotal As Label = CType(Me.Parent.FindControl("txtNetTotal"), Label)
                    If otxtNetTotal IsNot Nothing Then
                        otxtNetTotal.Text = New Money(oQuote.Risks(Session(CNCurrentRiskKey)).Premium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                    End If

                    Dim otxtFeeTotal As Label = CType(Me.Parent.FindControl("txtFeeTotal"), Label)
                    If otxtFeeTotal IsNot Nothing Then
                        otxtFeeTotal.Text = New Money(oQuote.Risks(Session(CNCurrentRiskKey)).FeePremium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                    End If

                    Dim otxtTaxTotal As Label = CType(Me.Parent.FindControl("txtTaxTotal"), Label)
                    If otxtTaxTotal IsNot Nothing Then
                        otxtTaxTotal.Text = New Money(dTotalRiskLevelTax, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                    End If

                    Dim otxtGrossTotal As Label = CType(Me.Parent.FindControl("txtGrossTotal"), Label)
                    If otxtGrossTotal IsNot Nothing Then
                        otxtGrossTotal.Text = New Money((oQuote.Risks(Session(CNCurrentRiskKey)).Premium + oQuote.Risks(Session(CNCurrentRiskKey)).FeePremium + dTotalRiskLevelTax), New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                    End If

                Finally
                    oWebService = Nothing
                End Try
            End If
            If oRatingCollection IsNot Nothing Then
                Session(CNRatingSections) = oRatingCollection
            End If
            dSum = Nothing
            PopulateData(oRatingCollection:=oRatingCollection)

            oRatingCollection = Nothing
        End Sub
        ''' <summary>
        ''' Row Data Bound - Calculate the Total if the column exists in TotalAttribNames
        ''' Show the Currency Symbol based if the Column exists in CurrencySymbolAttributesNames
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdViewRatingSection_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdViewRatingSection.RowDataBound
            Dim row As DataRowView


            TotalAttribNames = TotalAttribString.Split(",")

            ReDim Preserve dSum(UBound(TotalAttribNames))

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lnkbtnEdit As LinkButton = e.Row.FindControl("editrating")
                Dim lnkbtndelete As LinkButton = e.Row.FindControl("deleterating")
                If lnkbtnEdit IsNot Nothing Then
                    lnkbtnEdit.CommandArgument = e.Row.Cells(0).Text
                    If HttpContext.Current.Session.IsCookieless Then
                        lnkbtnEdit.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/RatingSectionDetails.aspx?RatingSectionID=" + e.Row.Cells(0).Text + "&Mode=Edit" & "',null);return false;"
                    Else
                        lnkbtnEdit.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/RatingSectionDetails.aspx?RatingSectionID=" + e.Row.Cells(0).Text + "&Mode=Edit&modal=true&KeepThis=true" & "',null);return false;"
                    End If

                End If
                If lnkbtndelete IsNot Nothing Then
                    lnkbtndelete.CommandArgument = e.Row.Cells(0).Text
                End If

                'Calculate the Total
                row = e.Row.DataItem()
                For iCount = 0 To UBound(TotalAttribNames)
                    If row(TotalAttribNames(iCount)) IsNot Nothing Then
                        dSum(iCount) += row(TotalAttribNames(iCount))
                    End If
                Next
                'Footer Row
            ElseIf e.Row.RowType = DataControlRowType.Footer Then    'Show the Totals in Grid View Footer
                Dim oFormatString As Config.FormatString
                Try
                    oFormatString = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Decimal2")
                Catch
                End Try

                e.Row.Cells(1).Text = "Total:"
                For iCount = 0 To UBound(TotalAttribNames)
                    Dim index As Integer
                    index = Array.IndexOf(AttribNames, TotalAttribNames(iCount))
                    If oFormatString IsNot Nothing Then
                        e.Row.Cells(index + 1).Attributes.Add("class", oFormatString.ItemStyleCssClass)
                        e.Row.Cells(index + 1).Text = String.Format(oFormatString.DataFormatString, dSum(iCount))
                    Else
                        e.Row.Cells(index + 1).Text = String.Format("{0:N2}", dSum(iCount))
                    End If

                Next
            End If
        End Sub
        Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        End Sub
    End Class
    ''' <summary>
    ''' Class to implement the Dynamic Addition of Link Buttons
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GridViewTemplate
        Implements System.Web.UI.ITemplate

        Dim templateType As ListItemType
        Dim editLinkField, deleteLinkField As String

        Sub New(ByVal type As ListItemType, ByVal field1 As String, ByVal field2 As String)
            templateType = type
            editLinkField = field1
            deleteLinkField = field2
        End Sub
        ''' <summary>
        ''' Add the Link Buttons to the Grid.   
        ''' </summary>
        ''' <param name="container"></param>
        ''' <remarks></remarks>
        Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Select Case (templateType)
                Case ListItemType.Item
                    Dim EditLink As New LinkButton, DeleteLink As New LinkButton
                    Dim oDiv As New System.Web.UI.HtmlControls.HtmlGenericControl("div")
                    Dim oOL As New System.Web.UI.HtmlControls.HtmlGenericControl("ol")
                    Dim oLIEdit As New System.Web.UI.HtmlControls.HtmlGenericControl("li")
                    Dim oLIDelete As New System.Web.UI.HtmlControls.HtmlGenericControl("li")
                    oDiv.Attributes.Add("class", "rowMenu")
                    oOL.Attributes.Add("class", "list-inline no-margin")
                    oDiv.Controls.Add(oOL)


                    With EditLink
                        .ID = "editrating"
                        .CommandName = "editrating"
                        .Text = editLinkField
                        .SkinID = "btnGrid"

                    End With

                    With DeleteLink
                        .ID = "deleterating"
                        .Text = deleteLinkField
                        .CommandName = "deleterating"
                        .SkinID = "btnGrid"
                    End With

                    If editLinkField.Length > 0 Then
                        oOL.Controls.Add(oLIEdit)
                        oLIEdit.Controls.Add(EditLink)
                        container.Controls.Add(oDiv)
                    End If
                    If deleteLinkField.Length > 0 Then
                        oOL.Controls.Add(oLIDelete)
                        oLIDelete.Controls.Add(DeleteLink)
                        container.Controls.Add(oDiv)
                    End If

            End Select
        End Sub
    End Class

End Namespace
