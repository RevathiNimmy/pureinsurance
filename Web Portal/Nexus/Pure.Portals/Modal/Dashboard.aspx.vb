Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Linq
Imports System.Xml.Linq
Imports Aspose.Words.Tables
Imports Nexus.Utils
Imports NexusProvider

Namespace Nexus
    Partial Class Modal_Dashboard
        Inherits System.Web.UI.Page

        Private sCurrency As String = ""
        Private sUserName As String = ""
        Private iPartyKey As Integer = 0

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then

                Dim oParty As NexusProvider.BaseParty = Nothing
                Dim dAccountBalance As Decimal = 0
                If TypeOf Session(Nexus.Constants.Session.CNParty) Is NexusProvider.PersonalParty Then
                    Dim oPersonalParty = CType(Session(Nexus.Constants.Session.CNParty), NexusProvider.PersonalParty)
                    dAccountBalance = CType(Session(Nexus.Constants.Session.CNParty), NexusProvider.PersonalParty).ClientSharedData.AccountBalance

                    lblName_view.Text = oPersonalParty.Title & " " & oPersonalParty.Forename & " " & oPersonalParty.Lastname
                    lblAddress.Text = oPersonalParty.Addresses(0).Address1 & " " & oPersonalParty.Addresses(0).Address2 & " " & oPersonalParty.Addresses(0).StateDescription & " " & oPersonalParty.Addresses(0).CountryDescription & " " & oPersonalParty.Addresses(0).PostCode
                    'lblNoofPolicies.Text = oPersonalParty.NoofPolicies.ToString()
                    'lblNoofOpenClaims.Text = oPersonalParty.NoofOpenClaims.ToString()
                    'lblNoofClosedClaims.Text = oPersonalParty.NoofClosedClaims.ToString()
                    sCurrency = oPersonalParty.Currency
                    sUserName = oPersonalParty.UserName
                    iPartyKey = oPersonalParty.Key
                    lblPremiumDue.Text = FormatNumber(New Money(CSng(dAccountBalance), sCurrency).Formatted())
                    BindChartDataFromDataBase(oPersonalParty.Key)

                ElseIf TypeOf Session(Nexus.Constants.Session.CNParty) Is NexusProvider.CorporateParty Then
                    Dim oCorporateParty = CType(Session(Nexus.Constants.Session.CNParty), NexusProvider.CorporateParty)
                    lblName_view.Text = oCorporateParty.MainContact
                    lblAddress.Text = oCorporateParty.CompanyName
                    'lblNoofPolicies.Text = oCorporateParty.NoofPolicies.ToString()
                    'lblNoofOpenClaims.Text = oCorporateParty.NoofOpenClaims.ToString()
                    'lblNoofClosedClaims.Text = oCorporateParty.NoofClosedClaims.ToString()
                    dAccountBalance = CType(Session(Nexus.Constants.Session.CNParty), NexusProvider.CorporateParty).ClientSharedData.AccountBalance
                    sCurrency = oCorporateParty.Currency
                    sUserName = oCorporateParty.UserName
                    iPartyKey = oCorporateParty.Key
                    lblPremiumDue.Text = FormatNumber(New Money(CSng(dAccountBalance), sCurrency).Formatted())
                    BindChartDataFromDataBase(oCorporateParty.Key)
                ElseIf TypeOf Session(Nexus.Constants.Session.CNParty) Is NexusProvider.OtherParty Then
                    oParty = CType(Session(Nexus.Constants.Session.CNParty), NexusProvider.OtherParty)
                End If

                Dim serverRoot As String = HttpContext.Current.Server.MapPath("~/")
                Dim relativePath As String = "dashboard/assets/images/users/" & sUserName & ".jpg"

                ' Combine server root with the relative path
                Dim fullPath As String = Path.Combine(serverRoot, relativePath)

                ' Check if the file exists
                If File.Exists(fullPath) Then
                    imgUserThumnail.Src = Request.ApplicationPath & "/" & "dashboard/assets/images/users/" & sUserName & ".jpg"
                Else
                    imgUserThumnail.Src = Request.ApplicationPath & "/" & "dashboard/assets/images/users/blank-profile-image.png"
                End If

                BindLatestTransactions()
            End If
        End Sub

        Private Sub BindChartDataFromDataBase(party_cnt As Integer)

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim storedProcedureName As String = "spu_SAM_Dashboard"
            Dim agentCnt As Integer = 0

            Try
                If Not IsNothing(Nexus.Constants.Session.CNAgentDetails) Then
                    agentCnt = CType(Session(Nexus.Constants.Session.CNAgentDetails), NexusProvider.UserDetails).Key
                End If

                Dim storedProcedureParameterType As New List(Of StoredProcedureParameterType) From {
                    New StoredProcedureParameterType With {.ParamName = "lparty_cnt", .ParamValue = Convert.ToString(party_cnt)},
                    New StoredProcedureParameterType With {.ParamName = "lagent_cnt", .ParamValue = Convert.ToString(agentCnt)}
                }

                Dim result = oWebservice.CallNamedStoredProcedure(storedProcedureParameterType.ToArray(), storedProcedureName)
                Dim xdoc As XDocument = XDocument.Parse(result.Results)

                Dim records = xdoc.Descendants("Row1").
                    Select(Function(row) New With {
                        Key .Year = CType(row.Element("Year"), Integer),
                        Key .WrittenPremium = CType(row.Element("WrittenPremium"), Decimal),
                        Key .ClaimIncurred = CType(row.Element("ClaimIncurred"), Decimal)
                    }).ToList()

                Dim BarChartjsonData As String = " ["
                For Each record In records
                    BarChartjsonData &= "{" &
                        """Year"": " & record.Year.ToString() & ", " &
                        """WrittenPremium"": " & record.WrittenPremium.ToString().Replace(",", ".") & ", " &
                        """ClaimIncurred"": " & record.ClaimIncurred.ToString().Replace(",", ".") &
                        "},"
                Next

                If records.Count > 0 Then BarChartjsonData = BarChartjsonData.TrimEnd(","c)
                BarChartjsonData &= "];"

                Dim riskData = xdoc.Descendants("Row").
                    Select(Function(row) New With {
                        Key .RiskType = CType(row.Element("RiskType"), String),
                        Key .Premium = CType(row.Element("Premium"), Decimal)
                    }).ToList()

                Dim jsonRiskData As String = " ["

                For Each record In riskData
                    jsonRiskData &= "{" &
                """RiskType"": """ & record.RiskType.Replace("""", "\""") & """, " &
                """Premium"": " & record.Premium.ToString("0.0000").Replace(",", ".") &
                "},"
                Next

                If riskData.Count > 0 Then jsonRiskData = jsonRiskData.TrimEnd(","c)
                jsonRiskData &= "];"

                ClientScript.RegisterStartupScript(Me.GetType(), "barChartData", "var barChartData = " & BarChartjsonData & ";", True)
                ClientScript.RegisterStartupScript(Me.GetType(), "chartData", "var chartData = " & jsonRiskData & ";", True)

                lblTotalPremium.Text = FormatNumber(New Money(CSng(xdoc.Descendants("Row1").Sum(Function(row) CType(row.Element("WrittenPremium"), Decimal))), sCurrency).Formatted())

                lblTotalClaimIncurred.Text = FormatNumber(New Money(CSng(xdoc.Descendants("Row1").Sum(Function(row) CType(row.Element("ClaimIncurred"), Decimal))), sCurrency).Formatted())

                Dim dTotalPremium As Decimal = xdoc.Descendants("Row1").Sum(Function(row) CType(row.Element("WrittenPremium"), Decimal))
                Dim dTotalClaimIncurred As Decimal = xdoc.Descendants("Row1").Sum(Function(row) CType(row.Element("ClaimIncurred"), Decimal))

                Dim dPremiumLastYear As Decimal
                Dim dPremiumLastTwoTear As Decimal
                Dim dPremiumThisYear As Decimal
                Dim dWrittenPremiumPercentage As Decimal

                If xdoc.Descendants("Row3").FirstOrDefault().Element("Premium_Till_Last_Year") IsNot Nothing Then
                    dPremiumLastYear = CType(xdoc.Descendants("Row3").FirstOrDefault().Element("Premium_Till_Last_Year"), Decimal)
                End If

                If xdoc.Descendants("Row5").FirstOrDefault().Element("Premium_Till_Two_Years_Back") IsNot Nothing Then
                    dPremiumLastTwoTear = CType(xdoc.Descendants("Row5").FirstOrDefault().Element("Premium_Till_Two_Years_Back"), Decimal)
                End If

                If xdoc.Descendants("Row3").FirstOrDefault().Element("Premium_Till_Last_Year") IsNot Nothing Then 'CType(xdoc.Descendants("Row3").FirstOrDefault()?.Element("Premium_Till_Last_Year"), Decimal) <> 0.0 Then
                    dPremiumThisYear = dTotalPremium - CType(xdoc.Descendants("Row3").FirstOrDefault().Element("Premium_Till_Last_Year"), Decimal)
                Else
                    dPremiumThisYear = dTotalPremium
                End If

                lblThisYearPremium.Text = FormatNumber(New Money(dPremiumThisYear, sCurrency).Formatted())

                If dPremiumLastYear <> 0 Then
                    dWrittenPremiumPercentage = ((dPremiumThisYear - Math.Abs(dPremiumLastYear - dPremiumLastTwoTear)) / dPremiumLastYear) * 100
                End If

                If Math.Round(dWrittenPremiumPercentage, 3) = 0 Then
                    spanPremium.Attributes("class") = "badge bg-warning me-1"
                    lblPremiumPercent.Text = "N/A"
                Else
                    If Math.Round(dWrittenPremiumPercentage, 3) > 0 Then
                        iPremiumPercent.Attributes("class") = "mdi mdi-arrow-up-bold"
                        spanPremium.Attributes("class") = "badge bg-success me-1"
                    Else
                        iPremiumPercent.Attributes("class") = "mdi mdi-arrow-down-bold"
                        spanPremium.Attributes("class") = "badge bg-danger me-1"
                    End If
                    lblPremiumPercent.Text = String.Format("{0:N2}", Math.Abs(Math.Round(dWrittenPremiumPercentage, 3))) & "%"
                End If

                Dim dClaimIncurredThisYear As Decimal
                Dim dClaimIncurredPercentage As Decimal
                Dim dClaimIncurredLastYear As Decimal
                Dim dClaimIncurredLastTwoYear As Decimal

                If xdoc.Descendants("Row4").FirstOrDefault().Element("Claim_Incurred_Till_Last_Year") IsNot Nothing Then
                    dClaimIncurredLastYear = CType(xdoc.Descendants("Row4").FirstOrDefault().Element("Claim_Incurred_Till_Last_Year"), Decimal)
                End If

                If xdoc.Descendants("Row6").FirstOrDefault().Element("Claim_Incurred_Till_Two_Years_Back") IsNot Nothing Then
                    dClaimIncurredLastTwoYear = CType(xdoc.Descendants("Row6").FirstOrDefault().Element("Claim_Incurred_Till_Two_Years_Back"), Decimal)
                End If

                If xdoc.Descendants("Row4").FirstOrDefault().Element("Claim_Incurred_Till_Last_Year") IsNot Nothing Then 'CType(xdoc.Descendants("Row4").FirstOrDefault().Element("Claim_Incurred_Till_Last_Year"), Decimal) <> 0.0 Then
                    dClaimIncurredThisYear = dTotalClaimIncurred - CType(xdoc.Descendants("Row4").FirstOrDefault().Element("Claim_Incurred_Till_Last_Year"), Decimal)
                Else
                    dClaimIncurredThisYear = dTotalClaimIncurred
                End If

                If xdoc.Descendants("Row8").FirstOrDefault().Element("NoofPolicies") IsNot Nothing Then
                    lblNoofPolicies.Text = CType(xdoc.Descendants("Row8").FirstOrDefault().Element("NoofPolicies"), String)
                    lblNoofOpenClaims.Text = CType(xdoc.Descendants("Row8").FirstOrDefault().Element("NoofOpenClaims"), String)
                    lblNoofClosedClaims.Text = CType(xdoc.Descendants("Row8").FirstOrDefault().Element("NoofCloseClaims"), String)
                End If

                lblClaimIncurred.Text = FormatNumber(New Money(dClaimIncurredThisYear, sCurrency).Formatted())

                If dClaimIncurredLastYear <> 0 Then
                    dClaimIncurredPercentage = ((dClaimIncurredThisYear - Math.Abs(dClaimIncurredLastYear - dClaimIncurredLastTwoYear)) / dClaimIncurredLastYear) * 100
                End If

                If Math.Round(dClaimIncurredPercentage, 3) = 0 Then
                    spanClaimIncurred.Attributes("class") = "badge bg-warning me-1"
                    lblClaimIncurredPercent.Text = "N/A"
                Else
                    If Math.Round(dClaimIncurredPercentage, 3) > 0 Then
                        iClaimIncurred.Attributes("class") = "mdi mdi-arrow-up-bold"
                        spanClaimIncurred.Attributes("class") = "badge bg-success me-1"
                    Else
                        iClaimIncurred.Attributes("class") = "mdi mdi-arrow-down-bold"
                        spanClaimIncurred.Attributes("class") = "badge bg-danger me-1"
                    End If
                    lblClaimIncurredPercent.Text = String.Format("{0:N2}", Math.Abs(Math.Round(dClaimIncurredPercentage, 3))) & "%"
                End If

                Dim dClaimLossRatioPercentage As Decimal
                Dim dClaimLossRatio As Decimal
                Dim dClaimLossRatioTillLastYear As Decimal

                If dTotalPremium <> 0 Then
                    dClaimLossRatio = (dTotalClaimIncurred / dTotalPremium) * 100
                End If

                If dPremiumLastYear <> 0 Then
                    dClaimLossRatioTillLastYear = (dClaimIncurredLastYear / dPremiumLastYear) * 100
                End If

                If CDec(dClaimLossRatio) <> 0 Then
                    lblClaimLossRatio.Text = String.Format("{0:N2}", dClaimLossRatio) & "%"
                    If dClaimLossRatio < 100 Then
                        lblClaimLossRatio.Attributes("style") = "color: #008000;"
                    Else
                        lblClaimLossRatio.Attributes("style") = "color: #FF6347;"
                    End If
                End If

                If dClaimLossRatioTillLastYear <> 0 Then
                    dClaimLossRatioPercentage = ((dClaimLossRatio - dClaimLossRatioTillLastYear) / dClaimLossRatioTillLastYear) * 100
                End If

                If Math.Round(dClaimLossRatioPercentage, 3) = 0 Then
                    spanClaimLossRatio.Attributes("class") = "badge bg-warning me-1"
                    lblClmLossRatioPercent.Text = "N/A"
                Else
                    If Math.Round(dClaimLossRatioPercentage, 3) > 0 Then
                        iClaimLossRatio.Attributes("class") = "mdi mdi-arrow-up-bold"
                        spanClaimLossRatio.Attributes("class") = "badge bg-success me-1"
                    Else
                        iClaimLossRatio.Attributes("class") = "mdi mdi-arrow-down-bold"
                        spanClaimLossRatio.Attributes("class") = "badge bg-danger me-1"
                    End If
                    lblClmLossRatioPercent.Text = String.Format("{0:N2}", Math.Abs(Math.Round(dClaimLossRatioPercentage, 3))) & "%"
                End If

                lblClaimOutstanding.Text = FormatNumber(New Money(CSng(xdoc.Descendants("Row2").Sum(Function(row) CType(row.Element("ClaimOutstanding"), Decimal))), sCurrency).Formatted())
                BindLatestPolicyFromSession(xdoc.Descendants("Row7"))
            Catch ex As Exception
                ' Handle exceptions
            Finally
                oWebservice = Nothing
            End Try
        End Sub

        Private Sub BindLatestPolicyFromSession(rows As IEnumerable(Of XElement))

            Try
                If rows IsNot Nothing AndAlso rows.Any() Then
                    Dim filteredPolicies = (
                        From row In rows
                        Select New With {
                            .PolicyId = row.Element("Policy_Id").Value.Trim(),
                            .RenewalDate = row.Element("Renewal_Date").Value.Trim(),
                            .PolicyStatus = row.Element("Status").Value.Trim(),
                            .ProductCode = row.Element("Code").Value.Trim(),
                            .Premium = row.Element("Premium").Value.Trim()
                        }
                    ).Take(5).ToList()

                    grdvPolicies.DataSource = filteredPolicies
                Else
                    grdvPolicies.DataSource = New List(Of Object)()
                End If

                grdvPolicies.DataBind()

            Finally

            End Try
        End Sub

        Protected Sub grdvPolicies_RowDataBound(sender As Object, e As GridViewRowEventArgs)
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' Get the current data item (e.g., from a DataTable, object, etc.)
                Dim dataItem = e.Row.DataItem

                If dataItem IsNot Nothing Then
                    Dim ltPolicyID = TryCast(e.Row.FindControl("ltPolicyID"), Literal)

                    If ltPolicyID IsNot Nothing Then
                        ' Bind the Literal control with data from the current item
                        ltPolicyID.Text = dataItem.PolicyId.ToString()
                    End If

                    Dim ltProduct = TryCast(e.Row.FindControl("ltProduct"), Literal)

                    If ltProduct IsNot Nothing Then
                        ' Bind the Literal control with data from the current item
                        ltProduct.Text = dataItem.ProductCode.ToString()
                    End If

                    Dim ltRenewalDate = TryCast(e.Row.FindControl("ltRenewalDate"), Literal)

                    If ltRenewalDate IsNot Nothing Then
                        ' Bind the Literal control with data from the current item
                        ltRenewalDate.Text = dataItem.RenewalDate.ToString()
                    End If

                    Dim ltStatus = TryCast(e.Row.FindControl("ltStatus"), Literal)
                    If ltStatus IsNot Nothing Then
                        ' Bind the Literal control with data from the current item
                        ltStatus.Text = dataItem.PolicyStatus.ToString()
                    End If

                    Dim spanControl = TryCast(e.Row.FindControl("spanStatus"), HtmlGenericControl)

                    If spanControl IsNot Nothing Then
                        Select Case dataItem.PolicyStatus.ToString()
                            Case "LIVE"
                                spanControl.Attributes("class") = "badge badge-success-lighten"
                            Case "CANCELLED", "LAPSED"
                                spanControl.Attributes("class") = "badge badge-danger-lighten"
                            Case "QUOTE"
                                spanControl.Attributes("class") = "badge badge-warning-lighten"
                            Case "RENEWED"
                                spanControl.Attributes("class") = "badge bg-info"
                            Case Else
                                spanControl.Attributes("class") = "badge bg-secondary"
                        End Select
                    End If

                    Dim ltPremium = TryCast(e.Row.FindControl("ltPremium"), Literal)
                    If ltPremium IsNot Nothing Then
                        Dim oCurrencies As Nexus.Library.Config.Currencies
                        oCurrencies = CType(System.Configuration.ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Currencies
                        ' Bind the Literal control with data from the current item
                        ltPremium.Text = FormatNumber(New Money(dataItem.Premium, sCurrency).Formatted())
                    End If
                End If
            End If
        End Sub

        Private Sub BindLatestTransactions()

            Dim oWebService As NexusProvider.ProviderBase
            Dim oAccountdetails As New AccountDetails()
            Dim oAccountDetailsCollection As NexusProvider.AccountDetailsDefaults

            Try
                oAccountdetails.PartyCnt = iPartyKey
                oWebService = New NexusProvider.ProviderManager().Provider
                oAccountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails)

                If oAccountDetailsCollection.AccountDetails IsNot Nothing Then
                    grdvTransactions.DataSource = oAccountDetailsCollection.AccountDetails.Cast(Of Object)().Take(5).ToList()
                Else
                    ' Bind an empty list to trigger EmptyDataText
                    grdvTransactions.DataSource = New List(Of Object)()
                End If

                grdvTransactions.DataBind()

            Catch ex As Exception

            Finally
                oWebService = Nothing
            End Try

        End Sub

        Protected Sub grdvTransactions_RowDataBound(sender As Object, e As GridViewRowEventArgs)
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' Get the current data item (e.g., from a DataTable, object, etc.)
                Dim dataItem As NexusProvider.AccountDetails = TryCast(e.Row.DataItem, NexusProvider.AccountDetails)

                If dataItem IsNot Nothing Then
                    Dim ltDocRef As Literal = TryCast(e.Row.FindControl("ltDocRef"), Literal)

                    If ltDocRef IsNot Nothing Then
                        ' Bind the Literal control with data from the current item
                        ltDocRef.Text = dataItem.DocRef.ToString()
                    End If

                    Dim ltDocRefDecs As Literal = TryCast(e.Row.FindControl("ltDocRefDecs"), Literal)

                    If ltDocRefDecs IsNot Nothing Then
                        ' Bind the Literal control with data from the current item
                        ltDocRefDecs.Text = dataItem.DocumentTypeCode.ToString()
                    End If

                    Dim ltMediaRef As Literal = TryCast(e.Row.FindControl("ltMediaRef"), Literal)

                    If ltMediaRef IsNot Nothing Then
                        ' Bind the Literal control with data from the current item
                        ltMediaRef.Text = dataItem.MediaRef.ToString()
                    End If

                    Dim ltRenewalDate As Literal = TryCast(e.Row.FindControl("ltTransactionsDate"), Literal)

                    If ltRenewalDate IsNot Nothing Then
                        ' Bind the Literal control with data from the current item
                        ltRenewalDate.Text = dataItem.TransDate.ToString("D")
                    End If

                    Dim ltAmount As Literal = TryCast(e.Row.FindControl("ltAmount"), Literal)
                    If ltAmount IsNot Nothing Then
                        Dim oCurrencies As Nexus.Library.Config.Currencies
                        oCurrencies = CType(System.Configuration.ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Currencies
                        ' Bind the Literal control with data from the current item
                        ltAmount.Text = oCurrencies.Currency(sCurrency).Symbol & dataItem.AccountAmount.ToString()
                    End If

                    Dim spanControl As HtmlGenericControl = TryCast(e.Row.FindControl("spanAmount"), HtmlGenericControl)

                    If spanControl IsNot Nothing Then
                        If dataItem.AccountAmount < 0 Then
                            spanControl.Attributes("class") = "badge badge-success-lighten"
                        Else
                            spanControl.Attributes("class") = "badge badge-warning-lighten"
                        End If
                    End If
                End If
            End If

        End Sub

        Private Function FormatNumber(value As String) As String

            Dim currencySymbol As String = ""
            Dim DecimalValue As Decimal
            If value.Contains("-") Then
                If (value.Contains("Rs")) Then
                    currencySymbol = value.Substring(0, 3)
                    DecimalValue = CDec(value.Substring(3))
                Else
                    currencySymbol = value.Substring(0, 2)
                    DecimalValue = CDec(value.Substring(2))
                End If
            ElseIf value.Contains(" ") Then
                Dim firstSpaceIndex As Integer = value.IndexOf(" "c)
                Select Case firstSpaceIndex
                    Case 1
                        currencySymbol = value.Substring(0, 1)
                        DecimalValue = CDec(value.Substring(1))
                    Case 2
                        currencySymbol = value.Substring(0, 2)
                        DecimalValue = CDec(value.Substring(2))
                    Case 3
                        currencySymbol = value.Substring(0, 3)
                        DecimalValue = CDec(value.Substring(3))
                End Select
            Else
                currencySymbol = value.Substring(0, 1)
                DecimalValue = CDec(value.Substring(1))
            End If


            If DecimalValue >= 1000000 Then
                Return currencySymbol.Trim() & (DecimalValue / 1000000).ToString("0.##") & "M" ' Millions
            ElseIf DecimalValue >= 1000 Then
                Return currencySymbol.Trim() & (DecimalValue / 1000).ToString("0.##") & "K" ' Thousands
            Else
                Return currencySymbol.Trim() & DecimalValue.ToString("N2") ' Default format for small numbers
            End If
        End Function

    End Class
End Namespace
