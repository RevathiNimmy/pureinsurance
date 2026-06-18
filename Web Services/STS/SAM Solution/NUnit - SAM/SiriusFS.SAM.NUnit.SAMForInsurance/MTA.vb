Option Strict On

Imports System.Xml.XPath

Public Class MTA
    Inherits BaseTest

    Public Sub TestMTCPayByInstalments()

        Dim insuranceFileKey As Integer = 802
        Dim insuranceFolderKey As Integer
        Dim transactionType As String = "MTC"
        Dim branchCode As String = "HEADOFF"
        Dim quoteExpiryDate As DateTime
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim riskDescription As String = String.Empty
        Dim effectivedate As Date
        Dim amount As Decimal = 0

        Try

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            effectivedate = DateAdd(DateInterval.Day, 1, effectivedate)

            ' add mta quote
            ' mtareason = mta_reason_description table?
            AddMTAQuote(insuranceFileKey, branchCode, quoteExpiryDate, "OTHER", "PERMANENT", quoteTimeStamp, effectivedate)

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            ' get risk
            GetRisk(branchCode, insuranceFileKey, insuranceFolderKey, riskKey, quoteTimeStamp, xmlDataSet)

            'ProcessRiskMTA(xmlDataSet)

            ' update risk
            UpdateRisk(branchCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, riskDescription, riskKey, "HOMEOWNERS", "", transactionType, xmlDataSet, amount)

            ' bind quote 
            BindQuoteByInstalmentsMTA(branchCode, insuranceFileKey, transactionType, amount)

        Catch ex As Exception

            Debug.Print(ex.ToString())
            Throw

        End Try

    End Sub

    Public Sub TestMTCPayByInvoice()

        Dim insuranceFileKey As Integer = 802
        Dim insuranceFolderKey As Integer
        Dim transactionType As String = "MTC"
        Dim branchCode As String = "HEADOFF"
        Dim quoteExpiryDate As DateTime
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim riskDescription As String = String.Empty
        Dim effectivedate As Date
        Dim amount As Decimal = 0
        Dim insuranceRef As String = String.Empty

        Try

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            effectivedate = DateAdd(DateInterval.Day, 1, effectivedate)

            ' add mta quote
            ' mtareason = mta_reason_description table?
            AddMTAQuote(insuranceFileKey, branchCode, quoteExpiryDate, "OTHER", "PERMANENT", quoteTimeStamp, effectivedate)

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            ' get risk
            GetRisk(branchCode, insuranceFileKey, insuranceFolderKey, riskKey, quoteTimeStamp, xmlDataSet)

            'ProcessRiskMTA(xmlDataSet)

            ' update risk
            UpdateRisk(branchCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, riskDescription, riskKey, "HOMEOWNERS", "", transactionType, xmlDataSet, amount)

            ' bind quote 
            BindQuoteAsInvoice(branchCode, insuranceFileKey, transactionType, insuranceRef)

        Catch ex As Exception

            Debug.Print(ex.ToString())
            Throw

        End Try

    End Sub


    Public Sub TestMTAWithInstalments()

        Dim insuranceFileKey As Integer = 802
        Dim insuranceFolderKey As Integer
        Dim transactionType As String = "MTA"
        Dim branchCode As String = "HEADOFF"
        Dim quoteExpiryDate As DateTime
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim riskDescription As String = String.Empty
        Dim effectivedate As Date
        Dim amount As Decimal = 0

        Try

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            effectivedate = DateAdd(DateInterval.Day, 1, effectivedate)

            ' add mta quote
            ' mtareason = mta_reason_description table?
            AddMTAQuote(insuranceFileKey, branchCode, quoteExpiryDate, "OTHER", "PERMANENT", quoteTimeStamp, effectivedate)

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            ' get risk
            GetRisk(branchCode, insuranceFileKey, insuranceFolderKey, riskKey, quoteTimeStamp, xmlDataSet)

            ProcessRiskMTA(xmlDataSet)

            ' update risk
            UpdateRisk(branchCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, riskDescription, riskKey, "HOMEOWNERS", "", transactionType, xmlDataSet, amount)

            ' bind quote 
            BindQuoteByInstalmentsMTA(branchCode, insuranceFileKey, transactionType, amount)

        Catch ex As Exception

            Debug.Print(ex.ToString())
            Throw

        End Try

    End Sub

    Public Sub TestNewBusinessPayByInstalments()

        Dim shortName As String = "SMITHNR"
        Dim transactionType As String = "NB"
        Dim branchCode As String = "HEADOFF"
        Dim subBranchCode As String = "HEADOFF"
        Dim currencyCode As String = "GBP"
        Dim dataModelCode As String = "HOMEOWNERS"
        Dim riskTypeCode As String = "RTHOMEOWN"
        Dim screenCode As String = "HOMEOWNERS"
        Dim productCode As String = "PRHOMEOWN"
        Dim partyKey As Integer = 0
        Dim insuranceFileKey As Integer
        Dim insuranceFolderKey As Integer
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskDescription As String = String.Empty
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim insuranceRef As String = String.Empty
        Dim runDefaultRules As Boolean = True
        Dim riskFolderKey As Integer
        Dim amount As Decimal = 0

        FindParty(branchCode, shortName, partyKey)

        AddNBQuote( _
            branchCode, _
            partyKey, _
            currencyCode, _
            productCode, _
            insuranceFileKey, _
            insuranceFolderKey, _
            quoteTimeStamp, _
            insuranceRef)

        AddRisk( _
            branchCode, _
            dataModelCode, _
             insuranceFileKey, _
            insuranceFolderKey, _
             productCode, _
             quoteTimeStamp, _
             riskDescription, _
             riskTypeCode, _
             runDefaultRules, _
             screenCode, _
             subBranchCode, _
             xmlDataSet, _
             riskFolderKey, _
             riskKey)

        ProcessRiskNB(xmlDataSet)

        UpdateRisk( _
            branchCode, _
            insuranceFileKey, _
            insuranceFolderKey, _
            quoteTimeStamp, _
            riskDescription, _
            riskKey, _
            screenCode, _
            subBranchCode, _
            transactionType, _
            xmlDataSet, _
            amount)

        BindQuoteByInstalments(branchCode, insuranceFileKey, transactionType, amount)

    End Sub

    Public Sub TestNewBusinessTMPPayByInstalments()

        Dim shortName As String = "SMITHNR"
        Dim transactionType As String = "NB"
        Dim branchCode As String = "HEADOFF"
        Dim subBranchCode As String = "HEADOFF"
        Dim currencyCode As String = "GBP"
        Dim dataModelCode As String = "HOMEOWNERS"
        Dim riskTypeCode As String = "RTHOMEOWN"
        Dim screenCode As String = "HOMEOWNERS"
        Dim productCode As String = "NewTMP"
        Dim partyKey As Integer = 0
        Dim insuranceFileKey As Integer
        Dim insuranceFolderKey As Integer
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskDescription As String = String.Empty
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim insuranceRef As String = String.Empty
        Dim runDefaultRules As Boolean = True
        Dim riskFolderKey As Integer
        Dim amount As Decimal = 0

        FindParty(branchCode, shortName, partyKey)

        AddNBQuoteTMP( _
            branchCode, _
            partyKey, _
            currencyCode, _
            productCode, _
            insuranceFileKey, _
            insuranceFolderKey, _
            quoteTimeStamp, _
            insuranceRef)

        AddRisk( _
            branchCode, _
            dataModelCode, _
             insuranceFileKey, _
            insuranceFolderKey, _
             productCode, _
             quoteTimeStamp, _
             riskDescription, _
             riskTypeCode, _
             runDefaultRules, _
             screenCode, _
             subBranchCode, _
             xmlDataSet, _
             riskFolderKey, _
             riskKey)

        ProcessRiskNB(xmlDataSet)

        UpdateRisk( _
            branchCode, _
            insuranceFileKey, _
            insuranceFolderKey, _
            quoteTimeStamp, _
            riskDescription, _
            riskKey, _
            screenCode, _
            subBranchCode, _
            transactionType, _
            xmlDataSet, _
            amount)

        BindQuoteByInstalments(branchCode, insuranceFileKey, transactionType, amount)

    End Sub

    Public Sub TestNewBusinessPayByInvoiceDirect()

        Dim shortName As String = "SMITHNR"
        Dim transactionType As String = "NB"
        Dim branchCode As String = "HEADOFF"
        Dim subBranchCode As String = "HEADOFF"
        Dim currencyCode As String = "GBP"
        Dim dataModelCode As String = "HOMEOWNERS"
        Dim riskTypeCode As String = "RTHOMEOWN"
        Dim screenCode As String = "HOMEOWNERS"
        Dim productCode As String = "PRHOMEOWN"
        Dim partyKey As Integer = 0
        Dim insuranceFileKey As Integer
        Dim insuranceFolderKey As Integer
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskDescription As String = String.Empty
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim insuranceRef As String = String.Empty
        Dim runDefaultRules As Boolean = True
        Dim riskFolderKey As Integer
        Dim amount As Decimal = 0

        FindParty(branchCode, shortName, partyKey)

        AddNBQuote(branchCode, partyKey, currencyCode, productCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, insuranceRef)




        AddRisk( _
            branchCode, _
            dataModelCode, _
             insuranceFileKey, _
            insuranceFolderKey, _
             productCode, _
             quoteTimeStamp, _
             riskDescription, _
             riskTypeCode, _
             runDefaultRules, _
             screenCode, _
             subBranchCode, _
             xmlDataSet, _
             riskFolderKey, _
             riskKey)

        ProcessRiskNB(xmlDataSet)

        UpdateRisk( _
            branchCode, _
            insuranceFileKey, _
            insuranceFolderKey, _
            quoteTimeStamp, _
            riskDescription, _
            riskKey, _
            screenCode, _
            subBranchCode, _
            transactionType, _
            xmlDataSet, _
            amount)

        BindQuoteAsInvoice(branchCode, insuranceFileKey, transactionType, insuranceRef)

    End Sub

    Public Sub TestUpdateQuoteAndGetPolicyDetails()

        Dim shortName As String = "SMITHNR"
        Dim transactionType As String = "NB"
        Dim branchCode As String = "HEADOFF"
        Dim subBranchCode As String = "HEADOFF"
        Dim currencyCode As String = "GBP"
        Dim dataModelCode As String = "HOMEOWNERS"
        Dim riskTypeCode As String = "RTHOMEOWN"
        Dim screenCode As String = "HOMEOWNERS"
        Dim productCode As String = "PRHOMEOWN"
        Dim partyKey As Integer = 0
        Dim insuranceFileKey As Integer
        Dim insuranceFolderKey As Integer
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskDescription As String = String.Empty
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim insuranceRef As String = String.Empty
        Dim runDefaultRules As Boolean = True
        Dim riskFolderKey As Integer
        Dim amount As Decimal = 0

        FindParty(branchCode, shortName, partyKey)

        AddNBQuote(branchCode, partyKey, currencyCode, productCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, insuranceRef)

        UpdateNBQuote(branchCode, insuranceFolderKey, insuranceFileKey, quoteTimeStamp)

        AddRisk( _
            branchCode, _
            dataModelCode, _
             insuranceFileKey, _
            insuranceFolderKey, _
             productCode, _
             quoteTimeStamp, _
             riskDescription, _
             riskTypeCode, _
             runDefaultRules, _
             screenCode, _
             subBranchCode, _
             xmlDataSet, _
             riskFolderKey, _
             riskKey)

        ProcessRiskNB(xmlDataSet)

        UpdateRisk( _
            branchCode, _
            insuranceFileKey, _
            insuranceFolderKey, _
            quoteTimeStamp, _
            riskDescription, _
            riskKey, _
            screenCode, _
            subBranchCode, _
            transactionType, _
            xmlDataSet, _
            amount)

        BindQuoteAsInvoice(branchCode, insuranceFileKey, transactionType, insuranceRef)

        GetPartySummary(branchCode, partyKey)

        GetAllPolicyVersions(branchCode, insuranceFolderKey)

        GetHeaderAndSummariesByKey(branchCode, insuranceFileKey)

        GetHeaderAndSummariesByRef(branchCode, insuranceRef)

    End Sub

    Public Sub GetHeaderAndSummariesByKey( _
              ByVal branchCode As String, _
              ByVal insuranceFileKey As Integer)

        Dim request As New ProxyWS.GetHeaderAndSummariesByKeyRequestType
        Dim response As ProxyWS.GetHeaderAndSummariesByKeyResponseType

        request.BranchCode = branchCode
        request.InsuranceFileKey = insuranceFileKey

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.GetHeaderAndSummariesByKey(request)

        If response.Errors Is Nothing Then

            If Not String.IsNullOrEmpty(response.AlternativeRef) Then
                Console.WriteLine(response.AlternativeRef)
            End If

        Else

            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in GetHeaderAndSummariesByKey")

        End If

    End Sub

    Public Sub GetHeaderAndSummariesByRef( _
          ByVal branchCode As String, _
          ByVal insuranceRef As String)

        Dim request As New ProxyWS.GetHeaderAndSummariesByRefRequestType
        Dim response As ProxyWS.GetHeaderAndSummariesByRefResponseType

        request.BranchCode = branchCode
        request.InsuranceRef = insuranceRef

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.GetHeaderAndSummariesByRef(request)

        If response.Errors Is Nothing Then

            If Not String.IsNullOrEmpty(response.AlternativeRef) Then
                Console.WriteLine(response.AlternativeRef)
            End If

            'Dim alternativeRef As String = response.AlternativeRef

        Else

            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in GetHeaderAndSummariesByKey")

        End If

    End Sub


    Public Sub GetAllPolicyVersions( _
      ByVal branchCode As String, _
      ByVal insuranceFolderKey As Integer)

        Dim request As New ProxyWS.GetAllPolicyVersionsRequestType
        Dim response As ProxyWS.GetAllPolicyVersionsResponseType

        request.BranchCode = branchCode
        request.InsuranceFolderKey = insuranceFolderKey

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.GetAllPolicyVersions(request)

        If response.Errors Is Nothing Then
            For Each policy As ProxyWS.BaseGetAllPolicyVersionsResponseTypeRow In response.Policies
                If policy.AlternativeRef IsNot Nothing Then
                    Console.WriteLine("AlternativeRef: " + policy.AlternativeRef.ToString)
                End If
            Next
            'Dim alternativeRef As String = response.Policies(0).AlternativeRef

        Else

            Debug.WriteLine("ERROR")
            Throw (New ApplicationException("Error occurred GetAllPolicyVersions"))

        End If

    End Sub

    Public Sub GetPartySummary( _
      ByVal branchCode As String, _
      ByVal partyKey As Integer)

        Dim request As New ProxyWS.GetPartySummaryRequestType
        Dim response As ProxyWS.GetPartySummaryResponseType

        request.BranchCode = branchCode
        request.PartyKey = partyKey

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.GetPartySummary(request)

        If response.Errors Is Nothing Then

            For Each policy As ProxyWS.BaseGetPartySummaryResponseTypeRow In response.Policies
                If policy.AlternativeRef IsNot Nothing Then
                    Console.WriteLine("AlternativeRef: " + policy.AlternativeRef.ToString)
                End If
            Next

        Else

            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in GetPartySummary")

        End If

    End Sub

    Public Sub UpdateNBQuote( _
    ByVal branchCode As String, _
    ByVal insuranceFolderKey As Integer, _
    ByVal insuranceFileKey As Integer, _
    ByRef quoteTimeStamp As Byte())

        Dim request As New ProxyWS.UpdateQuoteRequestType
        Dim response As ProxyWS.UpdateQuoteResponseType

        request.BranchCode = branchCode
        request.InsuranceFileKey = insuranceFileKey
        request.InsuranceFolderKey = insuranceFolderKey
        request.CoverStartDate = Date.Today
        request.CoverEndDate = DateAdd(DateInterval.Year, 1, request.CoverStartDate)
        request.AlternativeRef = "Hi Im Alt Ref 2"
        request.Description = "Hi Im A Description"
        request.QuoteTimeStamp = quoteTimeStamp

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.UpdateQuote(request)

        If response.Errors Is Nothing Then

            quoteTimeStamp = response.QuoteTimeStamp

        Else

            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in AddQuote")

        End If

    End Sub


    Public Sub TestNewBusinessPayByInvoiceDirectTMP()

        Dim shortName As String = "SMITHNR"
        Dim transactionType As String = "NB"
        Dim branchCode As String = "HEADOFF"
        Dim subBranchCode As String = "HEADOFF"
        Dim currencyCode As String = "GBP"
        Dim dataModelCode As String = "HOMEOWNERS"
        Dim riskTypeCode As String = "RTHOMEOWN"
        Dim screenCode As String = "HOMEOWNERS"
        Dim productCode As String = "NewTMP"
        Dim partyKey As Integer = 0
        Dim insuranceFileKey As Integer
        Dim insuranceFolderKey As Integer
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskDescription As String = String.Empty
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim insuranceRef As String = String.Empty
        Dim runDefaultRules As Boolean = True
        Dim riskFolderKey As Integer
        Dim amount As Decimal = 0

        FindParty(branchCode, shortName, partyKey)

        AddNBQuoteTMP(branchCode, partyKey, currencyCode, productCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, insuranceRef)

        AddRisk( _
            branchCode, _
            dataModelCode, _
             insuranceFileKey, _
            insuranceFolderKey, _
             productCode, _
             quoteTimeStamp, _
             riskDescription, _
             riskTypeCode, _
             runDefaultRules, _
             screenCode, _
             subBranchCode, _
             xmlDataSet, _
             riskFolderKey, _
             riskKey)

        ProcessRiskNB(xmlDataSet)

        UpdateRisk( _
            branchCode, _
            insuranceFileKey, _
            insuranceFolderKey, _
            quoteTimeStamp, _
            riskDescription, _
            riskKey, _
            screenCode, _
            subBranchCode, _
            transactionType, _
            xmlDataSet, _
            amount)

        BindQuoteAsInvoice(branchCode, insuranceFileKey, transactionType, insuranceRef)

    End Sub

    Public Sub TestNewBusinessPayByInvoiceWithAgent()

        Dim shortName As String = "SMITHNR"
        Dim transactionType As String = "NB"
        Dim branchCode As String = "HEADOFF"
        Dim subBranchCode As String = "HEADOFF"
        Dim currencyCode As String = "GBP"
        Dim dataModelCode As String = "HOMEOWNERS"
        Dim riskTypeCode As String = "RTHOMEOWN"
        Dim screenCode As String = "HOMEOWNERS"
        Dim productCode As String = "PRHOMEOWN"
        Dim partyKey As Integer = 0
        Dim insuranceFileKey As Integer
        Dim insuranceFolderKey As Integer
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskDescription As String = String.Empty
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim insuranceRef As String = String.Empty
        Dim runDefaultRules As Boolean = True
        Dim riskFolderKey As Integer
        Dim amount As Decimal = 0

        FindParty(branchCode, shortName, partyKey)

        AddNBQuoteWithAgent(branchCode, partyKey, currencyCode, productCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, insuranceRef)

        AddRisk( _
             branchCode, _
             dataModelCode, _
             insuranceFileKey, _
             insuranceFolderKey, _
             productCode, _
             quoteTimeStamp, _
             riskDescription, _
             riskTypeCode, _
             runDefaultRules, _
             screenCode, _
             subBranchCode, _
             xmlDataSet, _
             riskFolderKey, _
             riskKey)

        ProcessRiskNB(xmlDataSet)

        UpdateRisk( _
            branchCode, _
            insuranceFileKey, _
            insuranceFolderKey, _
            quoteTimeStamp, _
            riskDescription, _
            riskKey, _
            screenCode, _
            subBranchCode, _
            transactionType, _
            xmlDataSet, _
            amount)

        BindQuoteAsInvoice(branchCode, insuranceFileKey, transactionType, insuranceRef)

    End Sub

    Public Sub TestNewBusinessPayNow()

        Dim shortName As String = "SMITHNR"
        Dim transactionType As String = "NB"
        Dim branchCode As String = "HEADOFF"
        Dim subBranchCode As String = "HEADOFF"
        Dim currencyCode As String = "GBP"
        Dim dataModelCode As String = "HOMEOWNERS"
        Dim riskTypeCode As String = "RTHOMEOWN"
        Dim screenCode As String = "HOMEOWNERS"
        Dim productCode As String = "PRHOMEOWN"
        Dim partyKey As Integer = 0
        Dim insuranceFileKey As Integer
        Dim insuranceFolderKey As Integer
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskDescription As String = String.Empty
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim insuranceRef As String = String.Empty
        Dim runDefaultRules As Boolean = True
        Dim riskFolderKey As Integer
        Dim amount As Decimal = 0

        FindParty(branchCode, shortName, partyKey)

        AddNBQuote(branchCode, partyKey, currencyCode, productCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, insuranceRef)

        AddRisk( _
             branchCode, _
             dataModelCode, _
             insuranceFileKey, _
             insuranceFolderKey, _
             productCode, _
             quoteTimeStamp, _
             riskDescription, _
             riskTypeCode, _
             runDefaultRules, _
             screenCode, _
             subBranchCode, _
             xmlDataSet, _
             riskFolderKey, _
             riskKey)

        ProcessRiskNB(xmlDataSet)

        UpdateRisk( _
            branchCode, _
            insuranceFileKey, _
            insuranceFolderKey, _
            quoteTimeStamp, _
            riskDescription, _
            riskKey, _
            screenCode, _
            subBranchCode, _
            transactionType, _
            xmlDataSet, _
            amount)

        BindQuoteAsPayNow(branchCode, insuranceFileKey, transactionType, amount)

    End Sub


    Public Sub TestMTA()

        Dim insuranceFileKey As Integer = 802
        Dim insuranceFolderKey As Integer
        Dim transactionType As String = "MTA"
        Dim branchCode As String = "HEADOFF"
        Dim quoteExpiryDate As DateTime
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim riskDescription As String = String.Empty
        Dim effectivedate As Date
        Dim amount As Decimal = 0
        Dim insuranceRef As String = String.Empty

        Try

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            effectivedate = DateAdd(DateInterval.Day, 1, effectivedate)

            ' add mta quote
            ' mtareason = mta_reason_description table?
            AddMTAQuote(insuranceFileKey, branchCode, quoteExpiryDate, "OTHER", "PERMANENT", quoteTimeStamp, effectivedate)

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            ' get risk
            GetRisk(branchCode, insuranceFileKey, insuranceFolderKey, riskKey, quoteTimeStamp, xmlDataSet)

            ProcessRiskMTA(xmlDataSet)

            ' update risk
            UpdateRisk(branchCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, riskDescription, riskKey, "HOMEOWNERS", "", transactionType, xmlDataSet, amount)

            ' bind quote 
            BindQuoteAsInvoice(branchCode, insuranceFileKey, transactionType, insuranceref)

        Catch ex As Exception

            Debug.Print(ex.ToString())
            Throw

        End Try

    End Sub


    Public Sub TestMTAWithPolicyExtendedBy1Month()

        Dim insuranceFileKey As Integer = 805

        Dim insuranceFolderKey As Integer
        Dim transactionType As String = "MTA"
        Dim branchCode As String = "HEADOFF"
        Dim quoteExpiryDate As DateTime
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim riskDescription As String = String.Empty
        Dim effectivedate As Date
        Dim amount As Decimal = 0
        Dim insuranceRef As String = String.Empty

        Try

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            effectivedate = DateAdd(DateInterval.Day, 1, effectivedate)
            quoteExpiryDate = DateAdd(DateInterval.Month, 1, quoteExpiryDate)

            ' add mta quote
            ' mtareason = mta_reason_description table?
            AddMTAQuote(insuranceFileKey, branchCode, quoteExpiryDate, "OTHER", "PERMANENT", quoteTimeStamp, effectivedate)

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            ' get risk
            GetRisk(branchCode, insuranceFileKey, insuranceFolderKey, riskKey, quoteTimeStamp, xmlDataSet)

            ProcessRiskMTA(xmlDataSet)

            ' update risk
            UpdateRisk(branchCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, riskDescription, riskKey, "HOMEOWNERS", "", transactionType, xmlDataSet, amount)

            ' bind quote 
            BindQuoteAsInvoice(branchCode, insuranceFileKey, transactionType, insuranceref)

        Catch ex As Exception

            Debug.Print(ex.ToString())
            Throw

        End Try

    End Sub



    Public Sub TestMTATMPWithInvoice()

        Dim insuranceFileKey As Integer = 802
        Dim insuranceFolderKey As Integer
        Dim transactionType As String = "MTA"
        Dim branchCode As String = "HEADOFF"
        Dim quoteExpiryDate As DateTime
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim riskDescription As String = String.Empty
        Dim effectivedate As Date
        Dim amount As Decimal = 0
        Dim insuranceRef As String = String.Empty

        Try

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            effectivedate = DateAdd(DateInterval.Day, 1, effectivedate)

            ' add mta quote
            ' mtareason = mta_reason_description table?
            AddMTAQuote(insuranceFileKey, branchCode, quoteExpiryDate, "OTHER", "PERMANENT", quoteTimeStamp, effectivedate)

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectivedate)

            ' get risk
            GetRisk(branchCode, insuranceFileKey, insuranceFolderKey, riskKey, quoteTimeStamp, xmlDataSet)

            ProcessRiskMTA(xmlDataSet)

            ' update risk
            UpdateRisk(branchCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, riskDescription, riskKey, "HOMEOWNERS", "", transactionType, xmlDataSet, amount)

            ' bind quote 
            BindQuoteAsInvoice(branchCode, insuranceFileKey, transactionType, insuranceRef)


        Catch ex As Exception

            Debug.Print(ex.ToString())
            Throw

        End Try

    End Sub

    Public Sub TestMTAPayNow()

        Dim insuranceFileKey As Integer = 802
        Dim insuranceFolderKey As Integer
        Dim transactionType As String = "MTA"
        Dim branchCode As String = "HEADOFF"
        Dim quoteExpiryDate As DateTime
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim riskDescription As String = String.Empty
        Dim effectiveDate As Date
        Dim amount As Decimal = 0

        Try

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectiveDate)

            effectiveDate = DateAdd(DateInterval.Day, 1, effectiveDate)

            ' add mta quote
            ' mtareason = mta_reason_description table?
            AddMTAQuote(insuranceFileKey, branchCode, quoteExpiryDate, "OTHER", "PERMANENT", quoteTimeStamp, effectiveDate)

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectiveDate)

            ' get risk
            GetRisk(branchCode, insuranceFileKey, insuranceFolderKey, riskKey, quoteTimeStamp, xmlDataSet)

            ProcessRiskMTA(xmlDataSet)

            ' update risk
            UpdateRisk(branchCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, riskDescription, riskKey, "HOMEOWNERS", "", transactionType, xmlDataSet, amount)

            ' bind quote 
            BindQuoteAsPayNow(branchCode, insuranceFileKey, transactionType, amount)

        Catch ex As Exception

            Debug.Print(ex.ToString())
            Throw

        End Try
    End Sub

    Public Sub TestMTCPayNow()

        Dim insuranceFileKey As Integer = 802
        Dim insuranceFolderKey As Integer
        Dim transactionType As String = "MTC"
        Dim branchCode As String = "HEADOFF"
        Dim quoteExpiryDate As DateTime
        Dim quoteTimeStamp As Byte() = Nothing
        Dim riskKey As Integer
        Dim xmlDataSet As String = String.Empty
        Dim riskDescription As String = String.Empty
        Dim effectiveDate As Date
        Dim amount As Decimal = 0

        Try

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectiveDate)

            effectiveDate = Date.Today

            ' add mta quote
            ' mtareason = mta_reason_description table?
            AddMTAQuote(insuranceFileKey, branchCode, quoteExpiryDate, "OTHER", "PERMANENT", quoteTimeStamp, effectiveDate)

            ' get insurance file details
            GetInsuranceFileDetails(branchCode, insuranceFileKey, quoteExpiryDate, riskKey, riskDescription, insuranceFolderKey, effectiveDate)

            ' get risk
            GetRisk(branchCode, insuranceFileKey, insuranceFolderKey, riskKey, quoteTimeStamp, xmlDataSet)

            'ProcessRisk(xmlDataSet)

            ' update risk
            UpdateRisk(branchCode, insuranceFileKey, insuranceFolderKey, quoteTimeStamp, riskDescription, riskKey, "HOMEOWNERS", "", transactionType, xmlDataSet, amount)

            ' bind quote 
            BindQuoteMTCAsPayNow(branchCode, insuranceFileKey, transactionType, amount)

        Catch ex As Exception
            Debug.Print(ex.ToString())
            Throw
        End Try
    End Sub

    Private Sub AddRisk( _
    ByVal branchCode As String, _
    ByVal dataModelCode As String, _
    ByVal insuranceFileKey As Integer, _
    ByVal insuranceFolderKey As Integer, _
    ByVal productCode As String, _
    ByRef quoteTimeStamp As Byte(), _
    ByVal riskDescription As String, _
    ByVal riskTypeCode As String, _
    ByVal runDefaultRules As Boolean, _
    ByVal screenCode As String, _
    ByVal subBranchCode As String, _
    ByRef xmlDataSet As String, _
    ByRef riskFolderKey As Integer, _
    ByRef riskKey As Integer)

        Dim request As New ProxyWS.AddRiskRequestType
        Dim response As ProxyWS.AddRiskResponseType

        request.BranchCode = branchCode
        request.DataModelCode = dataModelCode
        request.InsuranceFileKey = insuranceFileKey
        request.InsuranceFolderKey = insuranceFolderKey
        request.ProductCode = productCode
        request.QuoteTimeStamp = quoteTimeStamp
        request.RiskDescription = riskDescription
        request.RiskTypeCode = riskTypeCode
        request.RunDefaultRules = runDefaultRules
        request.ScreenCode = screenCode
        request.SubBranchCode = subBranchCode
        request.RiskDescription = "Homeowners"

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.AddRisk(request)

        If response.Errors Is Nothing Then

            quoteTimeStamp = response.QuoteTimeStamp
            riskFolderKey = response.RiskFolderKey
            riskKey = response.RiskKey
            xmlDataSet = response.XMLDataSet

        Else

            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in AddRisk")

        End If

    End Sub

    Private Sub FindParty( _
    ByVal branchCode As String, _
    ByVal shortName As String, _
    ByRef partyKey As Integer)

        Dim request As New ProxyWS.FindPartyRequestType
        Dim response As ProxyWS.FindPartyResponseType

        request.BranchCode = branchCode
        request.Shortname = shortName

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.FindParty(request)

        If response.Errors Is Nothing Then

            If response.Parties IsNot Nothing And response.Parties.Length > 0 Then
                partyKey = response.Parties(0).PartyKey
            End If

        Else

            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in FindParty")

        End If

    End Sub

    Private Sub AddNBQuote( _
    ByVal branchCode As String, _
    ByVal partyKey As Integer, _
    ByVal currencyCode As String, _
    ByVal productCode As String, _
    ByRef insuranceFileKey As Integer, _
    ByRef insuranceFolderKey As Integer, _
    ByRef quoteTimeStamp As Byte(), _
    ByRef insuranceFileRef As String)

        Dim request As New ProxyWS.AddQuoteRequestType
        Dim response As ProxyWS.AddQuoteResponseType

        request.BranchCode = branchCode

        request.AgentKeySpecified = False

        request.CoverStartDate = Date.Today
        request.CoverEndDate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Year, 2, request.CoverStartDate))

        'request.CoverEndDate = request.CoverEndDate.AddDays(-1)

        request.CurrencyCode = "GBP"
        request.PartyKey = partyKey
        request.ProductCode = productCode
        request.AlternativeRef = "HI IM ALT REF 1"

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.AddQuote(request)

        If response.Errors Is Nothing Then

            insuranceFileKey = response.InsuranceFileKey
            insuranceFileRef = response.InsuranceFileRef
            insuranceFolderKey = response.InsuranceFolderKey
            quoteTimeStamp = response.QuoteTimeStamp

            'quoteExpiryDate = response.QuoteExpiryDate

        Else

            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in AddQuote")

        End If

    End Sub

    Private Sub AddNBQuoteTMP( _
        ByVal branchCode As String, _
        ByVal partyKey As Integer, _
        ByVal currencyCode As String, _
        ByVal productCode As String, _
        ByRef insuranceFileKey As Integer, _
        ByRef insuranceFolderKey As Integer, _
        ByRef quoteTimeStamp As Byte(), _
        ByRef insuranceFileRef As String)

        Dim request As New ProxyWS.AddQuoteRequestType
        Dim response As ProxyWS.AddQuoteResponseType

        request.BranchCode = branchCode

        request.AgentKeySpecified = False

        request.CoverStartDate = Date.Today
        request.CoverEndDate = DateAdd(DateInterval.Month, 1, request.CoverStartDate) 'DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, request.CoverStartDate))
        request.CurrencyCode = "GBP"
        request.PartyKey = partyKey
        request.ProductCode = productCode
        request.AgentKey = 51
        request.AgentKeySpecified = True

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.AddQuote(request)

        If response.Errors Is Nothing Then

            insuranceFileKey = response.InsuranceFileKey
            insuranceFileRef = response.InsuranceFileRef
            insuranceFolderKey = response.InsuranceFolderKey
            quoteTimeStamp = response.QuoteTimeStamp

        Else

            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in AddQuote")

        End If

    End Sub

    Private Sub AddNBQuoteWithAgent( _
        ByVal branchCode As String, _
        ByVal partyKey As Integer, _
        ByVal currencyCode As String, _
        ByVal productCode As String, _
        ByRef insuranceFileKey As Integer, _
        ByRef insuranceFolderKey As Integer, _
        ByRef quoteTimeStamp As Byte(), _
        ByRef insuranceFileRef As String)

        Dim request As New ProxyWS.AddQuoteRequestType
        Dim response As ProxyWS.AddQuoteResponseType

        request.BranchCode = branchCode

        request.AgentKeySpecified = True
        request.AgentKey = 16

        request.CoverStartDate = Date.Today
        request.CoverEndDate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Year, 1, request.CoverStartDate))
        request.CurrencyCode = "GBP"
        request.PartyKey = partyKey
        request.ProductCode = productCode

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.AddQuote(request)

        If response.Errors Is Nothing Then

            insuranceFileKey = response.InsuranceFileKey
            insuranceFileRef = response.InsuranceFileRef
            insuranceFolderKey = response.InsuranceFolderKey
            quoteTimeStamp = response.QuoteTimeStamp

            'quoteExpiryDate = response.QuoteExpiryDate

        Else

            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in AddQuote")

        End If

    End Sub

    Private Sub ProcessRiskMTA(ByRef xmlDataSet As String)

        ' load the xmldataset into an xml document
        Dim oXMLDoc As XmlDocument = New XmlDocument
        oXMLDoc.LoadXml(xmlDataSet)

        '' get the risk objects node
        Dim buildingsNode As XmlNode
        buildingsNode = oXMLDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER").SelectSingleNode("BUILDINGS")

        buildingsNode.Attributes("BUILDINGS_SUM_INSURED").Value = "65000"
        buildingsNode.Attributes("TOTAL_BUILDINGS_SI").Value = "65000"
        buildingsNode.Attributes("US").Value = "2"

        xmlDataSet = oXMLDoc.OuterXml

    End Sub

    Private Sub ProcessRisk(ByRef xmlDataSet As String)

        ' load the xmldataset into an xml document
        Dim oXMLDoc As XmlDocument = New XmlDocument
        oXMLDoc.LoadXml(xmlDataSet)

        '' get the risk objects node
        Dim buildingsNode As XmlNode
        buildingsNode = oXMLDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER").SelectSingleNode("BUILDINGS")

        buildingsNode.Attributes("BUILDINGS_SUM_INSURED").Value = "60000"
        buildingsNode.Attributes("TOTAL_BUILDINGS_SI").Value = "60000"
        buildingsNode.Attributes("US").Value = "2"

        xmlDataSet = oXMLDoc.OuterXml

    End Sub

    Private Sub ProcessRiskNB(ByRef xmlDataSet As String)

        ' load the xmldataset into an xml document
        Dim document As XmlDocument = New XmlDocument
        document.LoadXml(xmlDataSet)

        ProcessBuildingsNodeNBRisk(document)

        ProcessPropertyNodeNBRisk(document)

        ProcessRiskLocationNodeNBRisk(document)

        xmlDataSet = document.OuterXml

    End Sub

    Private Sub ProcessBuildingsNodeNBRisk(ByRef document As XmlDocument)

        Dim datasetNode As XmlNode
        datasetNode = document.SelectSingleNode("DATA_SET")

        Dim homeownersNode As XmlNode
        homeownersNode = document.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER")

        Dim buildingsNode As XmlNode
        Dim buildingsElement As XmlElement
        buildingsNode = document.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER").SelectSingleNode("BUILDINGS")

        If buildingsNode Is Nothing Then

            Dim nextOINumber As Integer = Integer.Parse(datasetNode.Attributes("NextOINumber").Value)
            nextOINumber = nextOINumber + 1
            datasetNode.Attributes("NextOINumber").Value = nextOINumber.ToString

            buildingsElement = document.CreateElement("BUILDINGS")
            buildingsElement.SetAttribute("OI", "OI" + nextOINumber.ToString())
            buildingsElement.SetAttribute("US", "1")
            homeownersNode.PrependChild(buildingsElement)
            buildingsNode = document.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER").SelectSingleNode("BUILDINGS")

        Else

            buildingsElement = DirectCast(buildingsNode, XmlElement)
            buildingsElement.SetAttribute("US", "2")

        End If

        buildingsElement.SetAttribute("BUILDINGS_SUM_INSURED", "60000")
        buildingsElement.SetAttribute("TOTAL_BUILDINGS_SI", "60000")
        buildingsElement.SetAttribute("BUILDINGS_COVER_REQUIRED", "1")

    End Sub

    Private Sub ProcessPropertyNodeNBRisk(ByRef document As XmlDocument)

        Dim datasetNode As XmlNode
        datasetNode = document.SelectSingleNode("DATA_SET")

        Dim homeownersNode As XmlNode
        homeownersNode = document.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER")

        Dim propertyNode As XmlNode
        Dim propertyElement As XmlElement
        propertyNode = document.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER").SelectSingleNode("PROPERTY")

        If propertyNode Is Nothing Then

            Dim nextOINumber As Integer = Integer.Parse(datasetNode.Attributes("NextOINumber").Value)
            nextOINumber = nextOINumber + 1
            datasetNode.Attributes("NextOINumber").Value = nextOINumber.ToString

            propertyElement = document.CreateElement("PROPERTY")
            propertyElement.SetAttribute("OI", "OI" + nextOINumber.ToString())
            propertyElement.SetAttribute("US", "1")
            homeownersNode.PrependChild(propertyElement)

            propertyNode = document.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER").SelectSingleNode("BUILDINGS")

        Else
            propertyElement = DirectCast(propertyNode, XmlElement)
            propertyElement.SetAttribute("US", "2")
        End If

        propertyElement.SetAttribute("CONSTRUCTION_TYPE", "23")
        propertyElement.SetAttribute("RESISTANCE_FEATURES", "31")
        propertyElement.SetAttribute("HOME_OCCUPIED_DETAILS", "H")
        propertyElement.SetAttribute("WATERS_EDGE", "53")
        propertyElement.SetAttribute("PROPERTY_TYPE", "33")
        propertyElement.SetAttribute("MORTGAGE_INTEREST", "40")
        propertyElement.SetAttribute("BANK_SCHEME", "59")

    End Sub

    Private Sub ProcessRiskLocationNodeNBRisk(ByRef document As XmlDocument)


        Dim datasetNode As XmlNode
        datasetNode = document.SelectSingleNode("DATA_SET")

        Dim homeownersNode As XmlNode
        homeownersNode = document.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER")

        Dim locationNode As XmlNode
        Dim locationElement As XmlElement
        locationNode = document.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER").SelectSingleNode("LOCATION")

        If locationNode Is Nothing Then

            Dim nextOINumber As Integer = Integer.Parse(datasetNode.Attributes("NextOINumber").Value)
            nextOINumber = nextOINumber + 1
            datasetNode.Attributes("NextOINumber").Value = nextOINumber.ToString

            locationElement = document.CreateElement("LOCATION")
            locationElement.SetAttribute("OI", "OI" + nextOINumber.ToString())
            locationElement.SetAttribute("US", "1")
            homeownersNode.PrependChild(locationElement)

            locationNode = document.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("HOMEOWNERS_POLICY_BINDER").SelectSingleNode("LOCATION")

        Else
            locationElement = DirectCast(locationNode, XmlElement)
            locationElement.SetAttribute("US", "2")
        End If

        locationElement.SetAttribute("COUNTRY", "60")
        locationElement.SetAttribute("ISLAND", "74")
        locationElement.SetAttribute("AREA", "121")



    End Sub

    Private Sub GetInsuranceFileDetails( _
    ByVal branchCode As String, _
    ByVal insuranceFileKey As Integer, _
    ByRef quoteExpiryDate As DateTime, _
    ByRef riskKey As Integer, _
    ByRef riskDescription As String, _
    ByRef insuranceFolderKey As Integer, _
    ByRef effectiveDate As DateTime)

        Dim request As New ProxyWS.GetHeaderAndSummariesByKeyRequestType
        Dim response As ProxyWS.GetHeaderAndSummariesByKeyResponseType

        request.BranchCode = branchCode
        request.InsuranceFileKey = insuranceFileKey

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.GetHeaderAndSummariesByKey(request)

        If response.Errors Is Nothing Then

            quoteExpiryDate = response.CoverEndDate
            insuranceFolderKey = response.InsuranceFolderKey

            If response.Risks IsNot Nothing AndAlso response.Risks.Length > 0 Then
                riskKey = response.Risks(0).RiskKey
                riskDescription = response.Risks(0).Description
                effectiveDate = response.CoverStartDate
            End If

        Else
            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in GetHeaderAndSummariesByKey")
        End If

    End Sub



    Private Sub AddMTAQuote( _
    ByRef insuranceFileKey As Integer, _
    ByVal branchCode As String, _
    ByVal expiryDate As DateTime, _
    ByVal mtaReason As String, _
    ByVal typeOfMTA As String, _
    ByRef quoteTimeStamp As Byte(), _
    ByVal effectiveDate As Date)

        Dim request As New ProxyWS.AddMtaQuoteRequestType
        Dim response As ProxyWS.AddMtaQuoteResponseType

        request.BranchCode = branchCode
        request.EffectiveDate = effectiveDate
        request.ExpiryDate = expiryDate
        request.InsuranceFileKey = insuranceFileKey
        request.MtaReason = mtaReason
        request.TypeOfMta = typeOfMTA

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.AddMtaQuote(request)

        If response.Errors Is Nothing Then
            quoteTimeStamp = response.QuoteTimeStamp
            insuranceFileKey = response.InsuranceFileKey
        Else
            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in AddMtaQuote")
        End If

    End Sub

    Private Sub GetRisk( _
    ByVal branchCode As String, _
    ByVal insuranceFileKey As Integer, _
    ByVal insuranceFolderKey As Integer, _
    ByVal riskKey As Integer, _
    ByRef quoteTimeStamp As Byte(), _
    ByRef xmlDataSet As String)

        Dim request As New ProxyWS.GetRiskRequestType
        Dim response As ProxyWS.GetRiskResponseType

        request.BranchCode = branchCode
        request.InsuranceFileKey = insuranceFileKey
        request.InsuranceFolderKey = insuranceFolderKey
        request.QuoteTimeStamp = quoteTimeStamp
        request.RiskKey = riskKey

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.GetRisk(request)

        If response.Errors Is Nothing Then

            quoteTimeStamp = response.QuoteTimeStamp
            xmlDataSet = response.XMLDataSet
        Else
            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in GetRisk")
        End If

    End Sub

    Private Sub UpdateRisk( _
    ByVal branchCode As String, _
    ByVal insuranceFileKey As Integer, _
    ByVal insuranceFolderKey As Integer, _
    ByVal quoteTimeStamp As Byte(), _
    ByVal riskDescription As String, _
    ByVal riskKey As Integer, _
    ByVal screenCode As String, _
    ByVal subBranchCode As String, _
    ByVal transactionType As String, _
    ByVal xmlDataSet As String, _
    ByRef amount As Decimal)

        Dim request As New ProxyWS.UpdateRiskRequestType
        Dim response As ProxyWS.UpdateRiskResponseType

        request.BranchCode = branchCode
        request.InsuranceFileKey = insuranceFileKey
        request.InsuranceFolderKey = insuranceFolderKey
        request.QuoteTimeStamp = quoteTimeStamp
        request.RiskDescription = riskDescription
        request.RiskKey = riskKey
        request.ScreenCode = screenCode
        request.SubBranchCode = subBranchCode
        request.TransactionType = transactionType
        request.XMLDataSet = xmlDataSet
        request.RiskDescription = "Homeowners"

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.UpdateRisk(request)

        If response.Errors Is Nothing Then

            quoteTimeStamp = response.QuoteTimeStamp
            xmlDataSet = response.XMLDataSet

            If response.PolicyLevelFeesSpecified Then
                amount += response.PolicyLevelFees
            End If

            If response.PolicyLevelTaxSpecified Then
                amount += response.PolicyLevelTax
            End If

            amount += response.PremiumDueGross

        Else

            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in UpdateRisk")

        End If

    End Sub

    Private Sub BindQuoteAsInvoice( _
    ByVal branchCode As String, _
    ByVal insuranceFileKey As Integer, _
    ByVal transactionType As String, _
    ByRef insuranceRef As String)

        Dim request As New ProxyWS.BindQuoteRequestType
        Dim response As ProxyWS.BindQuoteResponseType

        request.BranchCode = "HeadOff"
        request.InsuranceFileKey = insuranceFileKey
        request.TransactionType = transactionType
        request.PaymentMethodSpecified = False

        'request.PaymentMethod = ProxyWS.PaymentMethodType.None

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.BindQuote(request)

        If response.Errors IsNot Nothing Then
            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in BindQuote")
        Else
            insuranceRef = response.Policy.PolicyRef
        End If

    End Sub

    Private Sub BindQuoteByInstalmentsMTA( _
        ByVal branchCode As String, _
        ByVal insuranceFileKey As Integer, _
        ByVal transactionType As String, _
        ByVal amount As Decimal)

        Dim coverStartDate As Date = Date.Today
        Dim coverEndDate As Date = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Year, 1, coverStartDate))

        'Dim getInstalmentsRequest As New ProxyWS.GetInstalmentQuotesRequestType
        'Dim getInstalmentsResponse As  ProxyWS.GetInstalmentQuotesResponseType

        'getInstalmentsRequest.AmountToFinance = amount
        'getInstalmentsRequest.BranchCode = branchCode
        'getInstalmentsRequest.EndDate = coverEndDate
        'getInstalmentsRequest.InsuranceFileKey = insuranceFileKey
        'getInstalmentsRequest.MonthDay = 1
        'getInstalmentsRequest.OverrideInterestRate = 0
        'getInstalmentsRequest.OverrideRate = 0
        'getInstalmentsRequest.PaymentProtection = False
        'getInstalmentsRequest.PreferredDate = coverStartDate
        'getInstalmentsRequest.QuoteDate = coverStartDate
        'getInstalmentsRequest.StartDate = coverStartDate
        'getInstalmentsRequest.WeekDay = 1

        ' Set to valid user who has all SAM access rights.
        'SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        ' getInstalmentsResponse = oProxy.GetInstalmentQuotes(getInstalmentsRequest)
        Dim request As New ProxyWS.BindQuoteRequestType
        Dim response As ProxyWS.BindQuoteResponseType

        request.BranchCode = "HeadOff"
        request.InsuranceFileKey = insuranceFileKey
        request.TransactionType = transactionType
        request.PaymentMethodSpecified = True
        request.PaymentMethod = ProxyWS.PaymentMethodType.CreditCard

        request.SelectedInstalmentQuote = New ProxyWS.BaseSelectedInstalmentQuoteType

        request.SelectedInstalmentQuote.AmountPaid = 0
        request.SelectedInstalmentQuote.AmountToFinance = amount

        'request.SelectedInstalmentQuote.BankAccountName = "Bank GBP"
        'request.SelectedInstalmentQuote.BankAccountNo = "123456789"
        'request.SelectedInstalmentQuote.BankAddress = New ProxyWS.BaseAddressType
        'request.SelectedInstalmentQuote.BankAreaCode = "AREA_CODE"
        'request.SelectedInstalmentQuote.BankBranch = "BANK_BRANCH"
        'request.SelectedInstalmentQuote.BankExtn = "BANK_EXTN"
        'request.SelectedInstalmentQuote.BankFax = "BankFax"
        'request.SelectedInstalmentQuote.BankFaxCode = "BankFaxCode"
        'request.SelectedInstalmentQuote.BankName = "BankName"
        'request.SelectedInstalmentQuote.BankPhone = "012199999"
        'request.SelectedInstalmentQuote.BankSortCode = "102030"

        request.SelectedInstalmentQuote.CreditCard = New ProxyWS.BaseCreditCardType

        With request.SelectedInstalmentQuote.CreditCard
            .CardHolder = Nothing
            .ExpiryDate = "04/10"
            .Number = "123456789"
        End With

        'Dim schemeToUse As Integer = 0
        'If getInstalmentsResponse.Quotes.Length > 0 Then
        '    For item As Integer = 0 To getInstalmentsResponse.Quotes.Length
        '        If getInstalmentsResponse.Quotes(item).PFRF_ID = 6 Then
        '            schemeToUse = item
        '            Exit For
        '        End If
        '    Next
        'End If

        request.SelectedInstalmentQuote.EndDate = coverEndDate
        request.SelectedInstalmentQuote.MonthDay = 1
        request.SelectedInstalmentQuote.OverrideInterestRate = -1
        request.SelectedInstalmentQuote.OverrideRate = 0
        request.SelectedInstalmentQuote.PaymentProtection = False
        request.SelectedInstalmentQuote.PFRF_ID = 12 ' getInstalmentsResponse.Quotes(schemeToUse).PFRF_ID
        request.SelectedInstalmentQuote.PreferredDate = coverStartDate
        request.SelectedInstalmentQuote.QuoteDate = coverStartDate
        request.SelectedInstalmentQuote.SelectedSchemeNo = 4 'getInstalmentsResponse.Quotes(schemeToUse).SchemeNo
        request.SelectedInstalmentQuote.SelectedSchemeVersion = 1 'getInstalmentsResponse.Quotes(schemeToUse).SchemeVersion
        request.SelectedInstalmentQuote.StartDate = coverStartDate
        request.SelectedInstalmentQuote.WeekDay = 1

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.BindQuote(request)

        If response.Errors IsNot Nothing Then
            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in BindQuote")
        End If

    End Sub

    Private Sub BindQuoteByInstalments( _
    ByVal branchCode As String, _
    ByVal insuranceFileKey As Integer, _
    ByVal transactionType As String, _
    ByVal amount As Decimal)

        Dim coverStartDate As Date = Date.Today
        Dim coverEndDate As Date = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Year, 1, coverStartDate))

        Dim getInstalmentsRequest As New ProxyWS.GetInstalmentQuotesRequestType
        Dim getInstalmentsResponse As ProxyWS.GetInstalmentQuotesResponseType

        getInstalmentsRequest.AmountToFinance = amount
        getInstalmentsRequest.BranchCode = branchCode
        getInstalmentsRequest.EndDate = coverEndDate
        getInstalmentsRequest.InsuranceFileKey = insuranceFileKey
        getInstalmentsRequest.MonthDay = 1
        getInstalmentsRequest.OverrideInterestRate = 0
        getInstalmentsRequest.OverrideRate = 0
        getInstalmentsRequest.PaymentProtection = False
        'getInstalmentsRequest.PreferredDate = coverStartDate
        getInstalmentsRequest.QuoteDate = coverStartDate
        getInstalmentsRequest.StartDate = coverStartDate
        getInstalmentsRequest.WeekDay = 1

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        getInstalmentsResponse = oProxy.GetInstalmentQuotes(getInstalmentsRequest)

        Dim request As New ProxyWS.BindQuoteRequestType
        Dim response As ProxyWS.BindQuoteResponseType

        request.BranchCode = "HeadOff"
        request.InsuranceFileKey = insuranceFileKey
        request.TransactionType = transactionType
        request.PaymentMethodSpecified = True
        request.PaymentMethod = ProxyWS.PaymentMethodType.DebitCard

        request.SelectedInstalmentQuote = New ProxyWS.BaseSelectedInstalmentQuoteType

        request.SelectedInstalmentQuote.AmountPaid = 0
        request.SelectedInstalmentQuote.AmountToFinance = amount

        request.SelectedInstalmentQuote.BankAccountName = "Bank GBP"
        request.SelectedInstalmentQuote.BankAccountNo = "123456789"

        'request.SelectedInstalmentQuote.BankAddress = New ProxyWS.BaseAddressType
        'request.SelectedInstalmentQuote.BankAreaCode = "AREA_CODE"
        'request.SelectedInstalmentQuote.BankBranch = "BANK_BRANCH"
        'request.SelectedInstalmentQuote.BankExtn = "BANK_EXTN"
        'request.SelectedInstalmentQuote.BankFax = "BankFax"
        'request.SelectedInstalmentQuote.BankFaxCode = "BankFaxCode"
        'request.SelectedInstalmentQuote.BankName = "BankName"
        'request.SelectedInstalmentQuote.BankPhone = "012199999"
        'request.SelectedInstalmentQuote.BankSortCode = "102030"

        'request.SelectedInstalmentQuote.CreditCard = New ProxyWS.BaseCreditCardType

        'With request.SelectedInstalmentQuote.CreditCard
        '    .CardHolder = Nothing
        '    .ExpiryDate = "04/10"
        '    .Number = "123456789"
        'End With

        Dim schemeToUse As Integer = 0
        If getInstalmentsResponse.Quotes.Length > 0 Then
            For item As Integer = 0 To getInstalmentsResponse.Quotes.Length
                If getInstalmentsResponse.Quotes(item).PFRF_ID = 1 Then
                    schemeToUse = item
                    Exit For
                End If
            Next
        End If

        request.SelectedInstalmentQuote.EndDate = coverEndDate
        request.SelectedInstalmentQuote.MonthDay = 1
        request.SelectedInstalmentQuote.OverrideInterestRate = -1
        request.SelectedInstalmentQuote.OverrideRate = 0
        request.SelectedInstalmentQuote.PaymentProtection = False
        request.SelectedInstalmentQuote.PFRF_ID = getInstalmentsResponse.Quotes(schemeToUse).PFRF_ID
        request.SelectedInstalmentQuote.PreferredDate = coverStartDate
        request.SelectedInstalmentQuote.QuoteDate = coverStartDate
        request.SelectedInstalmentQuote.SelectedSchemeNo = getInstalmentsResponse.Quotes(schemeToUse).SchemeNo
        request.SelectedInstalmentQuote.SelectedSchemeVersion = getInstalmentsResponse.Quotes(schemeToUse).SchemeVersion
        request.SelectedInstalmentQuote.StartDate = coverStartDate
        request.SelectedInstalmentQuote.WeekDay = 1

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.BindQuote(request)

        If response.Errors IsNot Nothing Then
            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in BindQuote")
        End If

    End Sub


    Private Sub BindQuoteAsPayNow( _
        ByVal branchCode As String, _
        ByVal insuranceFileKey As Integer, _
        ByVal transactionType As String, _
        ByVal amount As Decimal)

        Dim request As New ProxyWS.BindQuoteRequestType
        Dim response As ProxyWS.BindQuoteResponseType

        request.BranchCode = "HeadOff"
        request.InsuranceFileKey = insuranceFileKey
        request.TransactionType = transactionType
        request.PaymentMethodSpecified = False

        request.PayNowDetails = New ProxyWS.BaseReceiptType

        request.PayNowDetails.CashListRef = "REF02"
        request.PayNowDetails.BankAccountName = "Bank GB"
        request.PayNowDetails.CurrencyCode = "GBP"
        request.PayNowDetails.ReceiptTypeCode = "STD"
        request.PayNowDetails.MediaTypeCode = "CA"
        request.PayNowDetails.TransactionDate = Now
        request.PayNowDetails.Amount = amount

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.BindQuote(request)

        If response.Errors IsNot Nothing Then
            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in BindQuote")
        End If

    End Sub

    Private Sub BindQuoteMTCAsPayNow( _
            ByVal branchCode As String, _
            ByVal insuranceFileKey As Integer, _
            ByVal transactionType As String, _
            ByVal amount As Decimal)

        Dim request As New ProxyWS.BindQuoteRequestType
        Dim response As ProxyWS.BindQuoteResponseType

        request.BranchCode = "HeadOff"
        request.InsuranceFileKey = insuranceFileKey
        request.TransactionType = transactionType
        request.PaymentMethodSpecified = False

        request.PayNowDetails = New ProxyWS.BaseReceiptType

        request.PayNowDetails.CashListRef = "REF03"
        request.PayNowDetails.BankAccountName = "Bank GB"
        request.PayNowDetails.CurrencyCode = "GBP"
        request.PayNowDetails.ReceiptTypeCode = "STD"
        request.PayNowDetails.MediaTypeCode = "CA"
        request.PayNowDetails.TransactionDate = CDate("2007/12/14")
        request.PayNowDetails.Amount = amount

        ' Set to valid user who has all SAM access rights.
        SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")

        response = oProxy.BindQuote(request)

        If response.Errors IsNot Nothing Then
            Debug.WriteLine("ERROR")
            Throw New ApplicationException("Error occurred in BindQuote")
        End If

    End Sub

End Class
