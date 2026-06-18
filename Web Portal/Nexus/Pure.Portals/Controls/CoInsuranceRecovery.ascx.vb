Imports System.Configuration.ConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports Nexus.Utils
Imports NexusProvider

Namespace Nexus
    ''' <summary>
    ''' Co-Insurance Recovery user control — displays co-insurer share breakdown
    ''' and per-peril co-insurance recovery details.
    ''' - Normal modes (NewClaim, EditClaim, ViewClaim, PayClaim): shows co-insurer breakdown grid
    ''' - Salvage/TP Recovery modes: shows salvage and third party recovery grids
    ''' Reusable control embedded on Perils.aspx and as a tab in PerilDetails.aspx.
    ''' All data is read-only.
    ''' </summary>
    Partial Class Controls_CoInsuranceRecovery
        Inherits System.Web.UI.UserControl

        Private Const CNCoInsuranceCache As String = "CoInsuranceCache"

        ''' <summary>
        ''' Self-contained Page_Load — checks if coinsurance is applicable and loads data.
        ''' Parent pages don't need any coinsurance logic.
        ''' </summary>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                If IsCoinsuranceApplicable() Then

                    Select Case Session(CNMode)
                        Case Mode.SalvageClaim
                            pnlRecoveryGrids.Visible = True
                            LoadAndCacheAllPerilData(True)
                        Case Mode.TPRecovery
                            pnlRecoveryGrids.Visible = True
                            LoadAndCacheAllPerilData(False)
                        Case Mode.ViewClaim
                            Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                            If Not String.IsNullOrEmpty(oClaim.ClaimVersionDescription) AndAlso oClaim.ClaimVersionDescription.Trim().IndexOf("Salvage") <> -1 Then
                                pnlRecoveryGrids.Visible = True
                                LoadAndCacheAllPerilData(True)
                            ElseIf Not String.IsNullOrEmpty(oClaim.ClaimVersionDescription) AndAlso oClaim.ClaimVersionDescription.Trim().IndexOf("Third Party") <> -1 Then
                                pnlRecoveryGrids.Visible = True
                                LoadAndCacheAllPerilData(False)
                            Else
                                LoadCoinsuranceBreakDownData()
                            End If

                        Case Else
                            LoadCoinsuranceBreakDownData()

                    End Select

                Else
                    Me.Visible = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Checks whether coinsurance is applicable — business type is Co-Insurance Lead
        ''' and product option "Inclusion of Co-insurers on claims" is enabled.
        ''' </summary>
        Private Function IsCoinsuranceApplicable() As Boolean
            Try
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                If oClaim Is Nothing Then
                    Return False
                End If

                Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                If oQuote Is Nothing OrElse String.IsNullOrEmpty(oQuote.BusinessTypeCode) Then Return False
                If oQuote.BusinessTypeCode.Trim().ToUpper() <> "COIN LEAD" Then
                    Return False
                End If

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sOptionValue As String = oWebService.GetProductRiskOptionValue(
                    NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.InclusionOfCoInsurersOnClaims, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                If String.IsNullOrEmpty(sOptionValue) OrElse sOptionValue = "0" Then
                    Return False
                End If

            Catch ex As Exception
                Return False
            End Try
            Return True
        End Function

        Private Sub LoadCoinsuranceBreakDownData()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            If oClaim Is Nothing Then
                Exit Sub
            End If

            Dim oCoInsurerClaim As NexusProvider.Claim = oWebService.GetClaimCoinsurer(oClaim.ClaimKey.ToString())
            If oCoInsurerClaim IsNot Nothing AndAlso oCoInsurerClaim.ClaimCoInsurer IsNot Nothing AndAlso oCoInsurerClaim.ClaimCoInsurer.Count > 0 Then
                oClaim.ClaimCoInsurer = oCoInsurerClaim.ClaimCoInsurer
                oClaim.TotalShare = oCoInsurerClaim.TotalShare
                oClaim.TotalCurrentShareValue = oCoInsurerClaim.TotalCurrentShareValue
            End If

             Dim bShowGrid As Boolean = False
            If Session(CNMode) = Mode.NewClaim Then
                bShowGrid = (oClaim.TotalCurrentShareValue > 0)
            Else
                bShowGrid = (oClaim.ClaimCoInsurer IsNot Nothing AndAlso oClaim.ClaimCoInsurer.Count > 0)
            End If

            If bShowGrid Then
                pnlCoInsuranceGrid.Visible = True

                txtClaimNumber.Text = oClaim.ClaimNumber
                If Session(CNMode) = Mode.NewClaim Then
                    GISLookup_Type.Enabled = True
                End If
                If Not String.IsNullOrEmpty(oClaim.CoinsuranceTreatmentCode) Then
                    GISLookup_Type.Value = oClaim.CoinsuranceTreatmentCode.Trim
                End If
                txtTotalShare.Text = String.Format("{0:0.00}%", oClaim.TotalShare)
                txtTotalCurrentShareValue.Text = FormatNumber(oClaim.TotalCurrentShareValue, 2)
                If oClaim.ClaimCoInsurer IsNot Nothing AndAlso oClaim.ClaimCoInsurer.Count > 0 Then
                    gvCoInsurerBreakdown.DataSource = oClaim.ClaimCoInsurer
                    gvCoInsurerBreakdown.DataBind()
                End If
                If Session(CNMode) = Mode.NewClaim Then
                    GISLookup_Type.Enabled = True
                    If String.IsNullOrEmpty(oClaim.CoinsuranceTreatmentCode) Then
                        If GISLookup_Type.Items IsNot Nothing AndAlso GISLookup_Type.Items.Count > 0 Then
                            GISLookup_Type.Value = GISLookup_Type.Items(0).Code
                            oClaim.CoinsuranceTreatmentCode = GISLookup_Type.Value
                            Session(CNClaim) = oClaim
                        End If
                    End If
                End If

            End If
        End Sub


        ''' <summary>
        ''' Loads coinsurance data for ALL perils, caches in ViewState,
        ''' and displays combined data in the grid with peril description column.
        ''' </summary>
        Private Sub LoadAndCacheAllPerilData(ByVal v_IsSalvage As Boolean)
            Try
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                If oClaim Is Nothing OrElse oClaim.ClaimPeril Is Nothing OrElse oClaim.ClaimPeril.Count = 0 Then
                    Exit Sub
                End If

                If v_IsSalvage Then
                    lblSalvageRecovery.Text = GetLocalResourceObject("lbl_SalvageRecovery").ToString()
                Else
                    lblSalvageRecovery.Text = GetLocalResourceObject("lbl_TPRecovery").ToString()
                End If

                ' Load coinsurance data for all perils and cache in ViewState
                Dim coInsuranceCache As New System.Collections.Generic.Dictionary(Of Integer, NexusProvider.CoInsurersCollections)
                For Each oPeril As NexusProvider.PerilSummary In oClaim.ClaimPeril
                    Dim oCoInsurers As NexusProvider.CoInsurersCollections = oWebService.GetRecoveryCoinsurance(oPeril.ClaimPerilKey, v_IsSalvage)
                    If oCoInsurers IsNot Nothing Then
                        ' Set peril description on each co-insurer row
                        Dim sPerilDesc As String = If(String.IsNullOrEmpty(oPeril.Description), "Peril", oPeril.Description.Trim())
                        For Each oCoIns As NexusProvider.CoInsurers In oCoInsurers
                            oCoIns.PerilDescription = sPerilDesc
                        Next
                        coInsuranceCache(oPeril.ClaimPerilKey) = oCoInsurers
                    End If
                Next
                ViewState(CNCoInsuranceCache) = coInsuranceCache

                ' Populate recovery type filter from all perils
                PopulateRecoveryTypeFilterAllPerils(oClaim, v_IsSalvage)

                ' Display combined data for all perils
                DisplayAllPerilsData(oClaim, v_IsSalvage)

            Catch ex As Exception
            End Try
        End Sub

        ''' <summary>
        ''' Displays coinsurance recovery data for all perils combined in one grid.
        ''' </summary>
        Private Sub DisplayAllPerilsData(ByVal oClaim As NexusProvider.ClaimOpen, ByVal v_IsSalvage As Boolean)
            If oClaim.ClaimPeril Is Nothing OrElse oClaim.ClaimPeril.Count = 0 Then Exit Sub

            Dim sSelectedType As String = ddlRecoveryTypeFilter.SelectedValue
            Dim oCombined As New NexusProvider.CoInsurersCollections
            Dim dReceiptTotal As Double = 0

            For i As Integer = 0 To oClaim.ClaimPeril.Count - 1
                Dim oPeril As NexusProvider.PerilSummary = oClaim.ClaimPeril(i)
                Dim oCoInsurersCollections As NexusProvider.CoInsurersCollections = GetCachedCoInsuranceData(oPeril.ClaimPerilKey)
                If oCoInsurersCollections Is Nothing OrElse oCoInsurersCollections.Count = 0 Then Continue For

                ' Get recoveries for this peril
                Dim oRecoveries As NexusProvider.PerilRecoveryCollection = Nothing
                If v_IsSalvage Then
                    oRecoveries = oClaim.ClaimPeril(i).SalvageRecovery
                Else
                    oRecoveries = oClaim.ClaimPeril(i).TPRecovery
                End If

                ' Build per-type receipt totals for this peril
                Dim receiptTotalsByType As New System.Collections.Generic.Dictionary(Of String, Double)
                If oRecoveries IsNot Nothing Then
                    For Each oRecovery As NexusProvider.PerilRecovery In oRecoveries
                        If Not oRecovery.IsDeleted AndAlso Not String.IsNullOrEmpty(oRecovery.TypeCode) Then
                            Dim sTypeCode As String = oRecovery.TypeCode.Trim()
                            If sSelectedType = "0" OrElse sTypeCode = sSelectedType Then
                                If Not receiptTotalsByType.ContainsKey(sTypeCode) Then
                                    receiptTotalsByType(sTypeCode) = 0
                                End If
                                receiptTotalsByType(sTypeCode) += oRecovery.LossThisNet
                                dReceiptTotal += oRecovery.LossThisNet
                            End If
                        End If
                    Next
                End If

                ' Add co-insurer rows to combined collection
                For Each oCoInsurer As NexusProvider.CoInsurers In oCoInsurersCollections
                    Dim sCoInsTypeCode As String = If(String.IsNullOrEmpty(oCoInsurer.RecoveryTypeCode), "", oCoInsurer.RecoveryTypeCode.Trim())
                    If sSelectedType <> "0" AndAlso sCoInsTypeCode <> sSelectedType Then Continue For

                    Dim typeTotal As Double = 0
                    If receiptTotalsByType.ContainsKey(sCoInsTypeCode) Then
                        typeTotal = receiptTotalsByType(sCoInsTypeCode)
                    End If
                    oCoInsurer.ThisRecovery = CDec(Math.Truncate(typeTotal * (oCoInsurer.SharePercent / 100) * 100) / 100)
                    oCoInsurer.TotalThisRecovery = typeTotal
                    oCombined.Add(oCoInsurer)
                Next
            Next

            txtRecoveryAmount.Text = FormatNumber(dReceiptTotal, 2)
            gvSalvageRecovery.DataSource = oCombined
            gvSalvageRecovery.DataBind()
        End Sub

        ''' <summary>
        ''' Populates the recovery type filter from ALL perils' recovery data.
        ''' </summary>
        Private Sub PopulateRecoveryTypeFilterAllPerils(ByVal oClaim As NexusProvider.ClaimOpen, ByVal v_IsSalvage As Boolean)
            Try
                ddlRecoveryTypeFilter.Items.Clear()
                ddlRecoveryTypeFilter.Items.Add(New ListItem(GetLocalResourceObject("lbl_All").ToString(), "0"))

                Dim addedTypes As New System.Collections.Generic.HashSet(Of String)

                If oClaim.ClaimPeril IsNot Nothing Then
                    For i As Integer = 0 To oClaim.ClaimPeril.Count - 1
                        Dim oRecoveries As NexusProvider.PerilRecoveryCollection = Nothing
                        If v_IsSalvage Then
                            oRecoveries = oClaim.ClaimPeril(i).SalvageRecovery
                        Else
                            oRecoveries = oClaim.ClaimPeril(i).TPRecovery
                        End If

                        If oRecoveries IsNot Nothing Then
                            For Each oRecovery As NexusProvider.PerilRecovery In oRecoveries
                                If Not oRecovery.IsDeleted AndAlso Not String.IsNullOrWhiteSpace(oRecovery.TypeCode) Then
                                    Dim sTypeCode As String = oRecovery.TypeCode.Trim()
                                    If Not addedTypes.Contains(sTypeCode) Then
                                        ddlRecoveryTypeFilter.Items.Add(New ListItem(sTypeCode, sTypeCode))
                                        addedTypes.Add(sTypeCode)
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Catch ex As Exception
            End Try
        End Sub

        ''' <summary>
        ''' Gets cached coinsurance data for a peril key from ViewState.
        ''' Falls back to a DB call if ViewState cache is empty.
        ''' </summary>
        Private Function GetCachedCoInsuranceData(ByVal iClaimPerilKey As Integer) As NexusProvider.CoInsurersCollections
            Dim cache As System.Collections.Generic.Dictionary(Of Integer, NexusProvider.CoInsurersCollections) = Nothing
            If ViewState(CNCoInsuranceCache) IsNot Nothing Then
                cache = CType(ViewState(CNCoInsuranceCache), System.Collections.Generic.Dictionary(Of Integer, NexusProvider.CoInsurersCollections))
                If cache.ContainsKey(iClaimPerilKey) Then
                    Return cache(iClaimPerilKey)
                End If
            End If

            ' Fallback: load from DB if ViewState cache is empty
            Try
                Dim bIsSalvage As Boolean = IsSalvageMode()
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oCoInsurers As NexusProvider.CoInsurersCollections = oWebService.GetRecoveryCoinsurance(iClaimPerilKey, bIsSalvage)
                Return oCoInsurers
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Determines if current mode is salvage.
        ''' </summary>
        Private Function IsSalvageMode() As Boolean
            If Session(CNMode) = Mode.SalvageClaim Then Return True
            If Session(CNMode) = Mode.ViewClaim Then
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                If oClaim IsNot Nothing AndAlso Not String.IsNullOrEmpty(oClaim.ClaimVersionDescription) AndAlso oClaim.ClaimVersionDescription.Trim().IndexOf("Salvage") <> -1 Then
                    Return True
                End If
            End If
            Return False
        End Function

        ''' <summary>
        ''' Handles recovery type filter change — rebuilds the combined grid
        ''' filtered by recovery type across all perils. Uses cached data.
        ''' </summary>
        Protected Sub ddlRecoveryTypeFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRecoveryTypeFilter.SelectedIndexChanged
            Try
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                If oClaim Is Nothing Then Exit Sub

                Dim bIsSalvage As Boolean = IsSalvageMode()
                DisplayAllPerilsData(oClaim, bIsSalvage)
            Catch ex As Exception
            End Try
        End Sub


        Private Sub GISLookup_Type_SelectedIndexChange(sender As Object, e As EventArgs) Handles GISLookup_Type.SelectedIndexChange
            If Session(CNMode) = Mode.NewClaim Then
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                If oClaim IsNot Nothing AndAlso Not String.IsNullOrEmpty(GISLookup_Type.Value) Then
                    oClaim.CoinsuranceTreatmentCode = GISLookup_Type.Value
                    Session(CNClaim) = oClaim
                End If
            End If
        End Sub
    End Class
End Namespace
