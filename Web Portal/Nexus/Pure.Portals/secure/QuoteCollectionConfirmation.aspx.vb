
Imports System.Data
Imports CMS.Library
Imports Nexus.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils

Partial Class secure_QuoteCollectionConfirmation
    Inherits Frontend.clsCMSPage
   

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'bind all of the quotes passed in
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        BindGrid()
    End Sub

    Protected Sub BindGrid()
        Dim oQuote As NexusProvider.Quote
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        'Bind all the Quote from Collection Array we have the InsurancefileKey
        If Session(CNQuoteCollectionFiles) IsNot Nothing Then
            Dim arrQuoteCollectionFiles As New ArrayList
            Dim iInsuranceFileKey As Integer
            arrQuoteCollectionFiles = Session(CNQuoteCollectionFiles)
            Dim oPolicySummaryCollection As New NexusProvider.PolicySummaryCollection
            Dim oPolicySummary As NexusProvider.PolicySummary
            Dim iCount As Integer

            'only run this when there is no postback
            If Session(CNPolicySummaryCollection) Is Nothing Then
                For iCount = 0 To arrQuoteCollectionFiles.Count - 1
                    iInsuranceFileKey = arrQuoteCollectionFiles(iCount)
                    oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey)
                    Session(CNQuote) = oQuote
                    'if there is no processing of the Quotes during Quote Collection
                    'means that there is no policy Summary collection then runbind quote
                    'Depending on the insurancefiletype code which is if a quote is NB/MTA/Renewal bind the quote accordingly

                    If oQuote.InsuranceFileTypeCode = "QUOTE" Then
                        oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, Session(CNPayment), oQuote.TimeStamp, False, _
                                        oQuote.BranchCode, "NB")
                        oPolicySummaryCollection.Add(oPolicySummary)
                        'Depending on the insurancefiletype code which is if a quote is MTA Quote
                    ElseIf oQuote.InsuranceFileTypeCode = "MTAQUOTE" Then
                        oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, Session(CNPayment), oQuote.TimeStamp, False, _
                                        oQuote.BranchCode, "MTA")
                        oPolicySummaryCollection.Add(oPolicySummary)
                        'Depending on the insurancefiletype code which is if a quote is Renewal Quote
                    ElseIf oQuote.InsuranceFileTypeCode = "RENEWAL" Then
                        oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, Session(CNPayment), oQuote.TimeStamp, False, _
                                        oQuote.BranchCode, "REN")
                        oPolicySummaryCollection.Add(oPolicySummary)
                    End If
                Next
            Else
                oPolicySummaryCollection = Session(CNPolicySummaryCollection)
            End If

            Session(CNPolicySummaryCollection) = oPolicySummaryCollection
            grdPolicies.DataSource = oPolicySummaryCollection
            grdPolicies.DataBind()
            grdPolicies.Visible = True
        End If

    End Sub

    Protected Sub grdPolicies_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPolicies.PageIndexChanging
        grdPolicies.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Protected Sub grdPolicies_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPolicies.RowDataBound
        'This will disable Recept link when Produce document is not selected in pAy now page.
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session(CNProduceDocument) Is Nothing Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    If e.Row.FindControl("liReceiptdocument") IsNot Nothing Then
                        e.Row.FindControl("liReceiptdocument").Visible = False
                    End If
                End If
            Else
                Session(CNPolicyNumber) = CType(e.Row.DataItem, NexusProvider.PolicySummary).Reference
            End If
        End If
    End Sub

    
End Class
