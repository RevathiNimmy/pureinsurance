''' <summary>
''' Nexus BaseParty object, containing the common elements between the various party types
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class BaseParty

    Private nKey As Integer
    Private sUserName As String
    Private sResolvedName As String
    Private sName As String
    Private sShortName As String
    Private bTimeStamp() As Byte
    Private nAgentKey As Integer = 0
    Private oAddresses As AddressCollection
    Private oBankDetails As BankCollection
    Private oContacts As ContactCollection
    Private oAssociate As AssociateCollection
    Private oConviction As ConvictionCollection
    Private oLifestyle As LifestyleCollection
    Private oLoyalty As LoyaltyCollection
    Private oProspectPolicy As ProspectPolicyCollection
    Private oCreditCardDetails As CreditCardCollection
    Private oAccidents As AccidentCollection
    Private oSupplierBusinesses As SupplierBusinessCollection
    Private sAccountExecutive As String
    Private sAccountExecutiveCode As String
    Private sAgent As String
    Private sAgentType As String
    Private sCurrency As String
    Private sTPINtroducer As String
    Private sTPUserCode As String
    Private bDomiciledForTax As Boolean
    Private bDomiciledForTaxSpecified As Boolean
    Private bTaxExempt As Boolean
    Private bTaxExemptSpecified As Boolean
    Private sTaxNumber, sAddressLine1, sAddressLine2, sPartySourceDescription, sType, sPostCode As String
    Private dTaxPercentage As Decimal
    Private bTaxPercentageSpecified As Boolean
    Private sFileCode As String
    'Newly added property for AddParty
    Private sXMLDataset As String
    Private iNoofPolicies As Integer
    Private iNoofOpenClaims As Integer
    Private iNoofClosedClaims As Integer
    Private sBranchCode As String
    Private sDateCancelled As Date
    Private sDOB As String
    Private partyTypeIdField As Integer


    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
        oAddresses = New AddressCollection
        oContacts = New ContactCollection
        oAssociate = New AssociateCollection
        oConviction = New ConvictionCollection
        oLifestyle = New LifestyleCollection
        oLoyalty = New LoyaltyCollection
        oProspectPolicy = New ProspectPolicyCollection
        oBankDetails = New BankCollection
        oCreditCardDetails = New CreditCardCollection
        oAccidents = New AccidentCollection
        oSupplierBusinesses = New SupplierBusinessCollection
    End Sub

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="v_iKey">Party Key</param>
    ''' <param name="v_sUserName">Username / Client Code</param>
    Public Sub New(ByVal v_iKey As Integer, ByVal v_sUserName As String)

        nKey = v_iKey
        sUserName = v_sUserName

        oAddresses = New AddressCollection
        oContacts = New ContactCollection
        oAssociate = New AssociateCollection
        oConviction = New ConvictionCollection
        oLifestyle = New LifestyleCollection
        oLoyalty = New LoyaltyCollection
        oProspectPolicy = New ProspectPolicyCollection
        oAccidents = New AccidentCollection
        oSupplierBusinesses = New SupplierBusinessCollection
        bDomiciledForTaxSpecified = False
        bTaxExemptSpecified = False
        bTaxPercentageSpecified = False

    End Sub

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Type : " & Me.GetType.Name & "<br />")
        sbPrint.AppendLine("Key : " & nKey & "<br />")
        sbPrint.AppendLine("Username : " & sUserName & "<br />")
        sbPrint.AppendLine("Resolved Name : " & sResolvedName & "<br />")
        sbPrint.Append("TimeStamp : ")

        If bTimeStamp IsNot Nothing Then

            For Each oByte As Byte In bTimeStamp
                sbPrint.Append(oByte.ToString & " | ")
            Next

        End If

        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("AgentKey : " & nAgentKey.ToString & "<br />")

        sbPrint.AppendLine("Addresses ---------------><br />")

        If oAddresses IsNot Nothing Then
            sbPrint.AppendLine(oAddresses.Print())
        End If

        sbPrint.AppendLine("Contacts ---------------><br />")

        If oContacts IsNot Nothing Then
            sbPrint.AppendLine(oContacts.Print())
        End If

        sbPrint.AppendLine("Account Executive : " & sAccountExecutive & "<br />")
        sbPrint.AppendLine("Agent : " & sAgent & "<br />")
        sbPrint.AppendLine("Currency : " & sCurrency & "<br />")
        sbPrint.AppendLine("TP Introducer : " & sTPINtroducer & "<br />")
        sbPrint.AppendLine("TP User Code : " & sTPUserCode & "<br />")

        sbPrint.AppendLine("Domiciled For Tax : " & IIf(bDomiciledForTax, "true", "false") & "<br />")
        sbPrint.AppendLine("Domiciled For Tax Specified : " & IIf(bDomiciledForTaxSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Tax Exempt : " & IIf(bTaxExempt, "true", "false") & "<br />")
        sbPrint.AppendLine("Tax Exempt Specified : " & IIf(bTaxExemptSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Tax Number : " & sTaxNumber & "<br />")
        sbPrint.AppendLine("Tax Percentage : " & dTaxPercentage.ToString & "<br />")
        sbPrint.AppendLine("Tax Percentage Specified : " & IIf(bTaxPercentageSpecified, "true", "false") & "<br />")

        sbPrint.AppendLine("Number of Policies  : " & iNoofPolicies & "<br />")
        sbPrint.AppendLine("Number of Open Claims  : " & iNoofOpenClaims & "<br />")
        sbPrint.AppendLine("Number of Closed Claims  : " & iNoofClosedClaims & "<br />")

        Return sbPrint.ToString

    End Function
    Public Property Name() As String
        Get
            Return sName
        End Get
        Set(ByVal value As String)
            sName = value
        End Set
    End Property
    Public Property ShortName() As String
        Get
            Return sShortName
        End Get
        Set(ByVal value As String)
            sShortName = value
        End Set
    End Property
    Public Property AddressLine1() As String
        Get
            Return sAddressLine1
        End Get
        Set(ByVal value As String)
            sAddressLine1 = value
        End Set
    End Property
    Public Property AddressLine2() As String
        Get
            Return sAddressLine2
        End Get
        Set(ByVal value As String)
            sAddressLine2 = value
        End Set
    End Property
    Public Property Type() As String
        Get
            Return sType
        End Get
        Set(ByVal value As String)
            sType = value
        End Set
    End Property
    Public Property PartyTypeId() As Integer
        Get
            Return partyTypeIdField
        End Get
        Set(ByVal value As Integer)
            partyTypeIdField = value
        End Set
    End Property
    Public Property PartySourceDescription() As String
        Get
            Return sPartySourceDescription
        End Get
        Set(ByVal value As String)
            sPartySourceDescription = value
        End Set
    End Property
    Public Property PostCode() As String
        Get
            Return sPostCode
        End Get
        Set(ByVal value As String)
            sPostCode = value
        End Set
    End Property
    Public Property Key() As Integer
        Get
            Return nKey
        End Get
        Set(ByVal value As Integer)
            nKey = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return sUserName
        End Get
        Set(ByVal value As String)
            sUserName = value
        End Set
    End Property

    Public Property ResolvedName() As String
        Get
            Return sResolvedName
        End Get
        Set(ByVal value As String)
            sResolvedName = value
        End Set
    End Property

    Public Property TimeStamp() As Byte()
        Get
            Return bTimeStamp
        End Get
        Set(ByVal value As Byte())
            bTimeStamp = value
        End Set
    End Property

    Public Property AgentKey() As Integer
        Get
            Return nAgentKey
        End Get
        Set(ByVal value As Integer)
            nAgentKey = value
        End Set
    End Property

    Public ReadOnly Property AgentKeySpecified() As Boolean
        Get
            IIf(nAgentKey = 0, False, True)
        End Get
    End Property

    Public Property Addresses() As AddressCollection
        Get
            Return oAddresses
        End Get
        Set(ByVal value As AddressCollection)
            oAddresses = value
        End Set
    End Property

    Public Property BankDetails() As BankCollection
        Get
            Return oBankDetails
        End Get
        Set(ByVal value As BankCollection)
            oBankDetails = value
        End Set
    End Property

    Public Property Contacts() As ContactCollection
        Get
            Return oContacts
        End Get
        Set(ByVal value As ContactCollection)
            oContacts = value
        End Set
    End Property

    Public Property Associate() As AssociateCollection
        Get
            Return oAssociate
        End Get
        Set(ByVal value As AssociateCollection)
            oAssociate = value
        End Set
    End Property

    Public Property Conviction() As ConvictionCollection
        Get
            Return oConviction
        End Get
        Set(ByVal value As ConvictionCollection)
            oConviction = value
        End Set
    End Property

    Public Property Lifestyle() As LifestyleCollection
        Get
            Return oLifestyle
        End Get
        Set(ByVal value As LifestyleCollection)
            oLifestyle = value
        End Set
    End Property

    Public Property Loyalty() As LoyaltyCollection
        Get
            Return oLoyalty
        End Get
        Set(ByVal value As LoyaltyCollection)
            oLoyalty = value
        End Set
    End Property

    Public Property ProspectPolicy() As ProspectPolicyCollection
        Get
            Return oProspectPolicy
        End Get
        Set(ByVal value As ProspectPolicyCollection)
            oProspectPolicy = value
        End Set
    End Property

    Public Property Accidents As AccidentCollection
        Get
            Return oAccidents
        End Get
        Set(ByVal value As AccidentCollection)
            oAccidents = value
        End Set
    End Property

    Public Property SupplierBusinesses As SupplierBusinessCollection
        Get
            Return oSupplierBusinesses
        End Get
        Set(ByVal value As SupplierBusinessCollection)
            oSupplierBusinesses = value
        End Set
    End Property

    Public Property AccountExecutive() As String
        Get
            Return sAccountExecutive
        End Get
        Set(ByVal value As String)
            sAccountExecutive = value
        End Set
    End Property
    Public Property AccountExecutiveCode() As String
        Get
            Return sAccountExecutiveCode
        End Get
        Set(ByVal value As String)
            sAccountExecutiveCode = value
        End Set
    End Property

    Public Property Agent() As String
        Get
            Return sAgent
        End Get
        Set(ByVal value As String)
            sAgent = value
        End Set
    End Property
    Public Property AgentType() As String
        Get
            Return sAgentType
        End Get
        Set(ByVal value As String)
            sAgentType = value
        End Set
    End Property
    Public Property Currency() As String
        Get
            Return sCurrency
        End Get
        Set(ByVal value As String)
            sCurrency = value
        End Set
    End Property

    Public Property TPIntroducer() As String
        Get
            Return sTPINtroducer
        End Get
        Set(ByVal value As String)
            sTPINtroducer = value
        End Set
    End Property

    Public Property TPUserCode() As String
        Get
            Return sTPUserCode
        End Get
        Set(ByVal value As String)
            sTPUserCode = value
        End Set
    End Property

    Public Property DomiciledForTax() As Boolean
        Get
            Return bDomiciledForTax
        End Get
        Set(ByVal value As Boolean)
            bDomiciledForTax = value
            bDomiciledForTaxSpecified = True
        End Set
    End Property

    Public ReadOnly Property DomiciledForTaxSpecified() As Boolean
        Get
            Return bDomiciledForTaxSpecified
        End Get
    End Property


    Public Property TaxExempt() As Boolean
        Get
            Return bTaxExempt
        End Get
        Set(ByVal value As Boolean)
            bTaxExempt = value
            bTaxExemptSpecified = True
        End Set
    End Property

    Public ReadOnly Property TaxExemptSpecified() As Boolean
        Get
            Return bTaxExemptSpecified
        End Get
    End Property

    Public Property TaxNumber() As String
        Get
            Return sTaxNumber
        End Get
        Set(ByVal value As String)
            sTaxNumber = value
        End Set
    End Property

    Public Property TaxPercentage() As Decimal
        Get
            Return dTaxPercentage
        End Get
        Set(ByVal value As Decimal)
            dTaxPercentage = value
            bTaxPercentageSpecified = True
        End Set
    End Property

    Public ReadOnly Property TaxPercentageSpecified() As Boolean
        Get
            Return bTaxPercentageSpecified
        End Get
    End Property
    Public Property FileCode() As String
        Get
            Return sFileCode
        End Get
        Set(ByVal value As String)
            sFileCode = value
        End Set
    End Property
    'Newly added Property for Add Party
    Public Property XMLDataset() As String
        Get
            Return sXMLDataset
        End Get
        Set(ByVal value As String)
            sXMLDataset = value
        End Set
    End Property

    Public Property NoofPolicies() As Integer
        Get
            Return iNoofPolicies
        End Get
        Set(ByVal value As Integer)
            iNoofPolicies = value
        End Set
    End Property

    Public Property NoofOpenClaims() As Integer
        Get
            Return iNoofOpenClaims
        End Get
        Set(ByVal value As Integer)
            iNoofOpenClaims = value
        End Set
    End Property

    Public Property NoofClosedClaims() As Integer
        Get
            Return iNoofClosedClaims
        End Get
        Set(ByVal value As Integer)
            iNoofClosedClaims = value
        End Set
    End Property
    Public Property BranchCode() As String
        Get
            Return sBranchCode
        End Get
        Set(ByVal value As String)
            sBranchCode = value
        End Set
    End Property
    Public Property DateCancelled() As Date
        Get
            Return sDateCancelled
        End Get
        Set(ByVal value As Date)
            sDateCancelled = value
        End Set
    End Property

    Public Property CountryCode() As String
    Public Property DOB() As String
        Get
            Return sDOB
        End Get
        Set(ByVal value As String)
            sDOB = value
        End Set
    End Property

    Public Property ServiceLevelCode() As String
    Public Property ServiceLevelDescription() As String
    Public Property BlacklistReasonCode() As String
    Public Property RenewalStopCode() As String

    Public Property CreditCardDetails() As CreditCardCollection
        Get
            Return oCreditCardDetails
        End Get
        Set(ByVal value As CreditCardCollection)
            oCreditCardDetails = value
        End Set
    End Property
    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by Key
    ''' </summary>
    <Serializable()> Public Class SortByKey : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Party by their key attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As BaseParty

            If TypeOf x Is BaseParty Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is BaseParty Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If oLeft.Key = 0 And oRight.Key = 0 Then
                Return 0
            ElseIf oLeft.Key = 0 Then
                Return -1
            ElseIf oRight.Key = 0 Then
                Return 1
            ElseIf oLeft.Key < oRight.Key Then
                Return -1
            ElseIf oLeft.Key = oRight.Key Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by Resolved Name
    ''' </summary>
    <Serializable()> Public Class SortByResolvedName : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Party by their Resolved Name attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As BaseParty

            If TypeOf x Is BaseParty Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is BaseParty Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.ResolvedName) And String.IsNullOrEmpty(oRight.ResolvedName) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.ResolvedName) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.ResolvedName) Then
                Return 1
            ElseIf oLeft.ResolvedName < oRight.ResolvedName Then
                Return -1
            ElseIf oLeft.ResolvedName = oRight.ResolvedName Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by UserName
    ''' </summary>
    <Serializable()> Public Class SortByUserName : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Party by their UserName attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As BaseParty

            If TypeOf x Is BaseParty Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is BaseParty Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.UserName) And String.IsNullOrEmpty(oRight.UserName) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.UserName) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.UserName) Then
                Return 1
            ElseIf oLeft.UserName < oRight.UserName Then
                Return -1
            ElseIf oLeft.UserName = oRight.UserName Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

End Class

''' <summary>
''' Nexus Corporate Party object
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class CorporateParty : Inherits BaseParty
    Private oClientSharedData As Client
    Private sBusinessCode, sCompanyName, sMainContact, sNumberOfEmployees, sTradeCode, sCompanyReg, sSICCode, sTurnoverCode, sSalutation, sSource, sAlternativeId As String
    Private iNumberOfOffices As Integer
    Private bNumberOfOfficesSpecified, bTPS, bMPS, beMPS, beMPSSpecified, bMPSSpecified, bTPSSpecified, bFinancialYearSpecified, bWageRollSpecified, bTradingsinceSpecified As Boolean
    Private dtTradingSince, dtFinancialYear As DateTime
    Private dWageRoll As Decimal
    Private sRegistrationNumber, sRegistrationName, sRegistrationOffice, sAnnualSales, sQuarterlySales, sBusinessName, sPaidOfCapital, sShareCapital, sTANNumber, sStateTaxNumber, sCentralTaxNumber As String
    Private bIsActive, bIsPartner As Boolean
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
        MyBase.New()
        oClientSharedData = New Client
    End Sub

    ''' <summary>
    ''' Constructor for CorporateParty
    ''' </summary>
    ''' <param name="v_iKey">Key</param>
    ''' <param name="v_sShortName">Username / Client Code</param>
    Public Sub New(ByVal v_iKey As Integer, ByVal v_sShortName As String)

        MyBase.New(v_iKey, v_sShortName)
        bNumberOfOfficesSpecified = False

    End Sub

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overrides Function Print() As String

        Dim sbPrint As New Text.StringBuilder(MyBase.Print())

        sbPrint.AppendLine("Business Code : " & sBusinessCode & "<br />")
        sbPrint.AppendLine("Company Name : " & sCompanyName & "<br />")
        sbPrint.AppendLine("Main Contact : " & sMainContact & "<br />")
        sbPrint.AppendLine("Number Of Employees : " & sNumberOfEmployees & "<br />")
        sbPrint.AppendLine("Client Shared data -------><br />")
        If oClientSharedData IsNot Nothing Then
            sbPrint.AppendLine(oClientSharedData.Print())
        End If
        sbPrint.AppendLine("Trade Code : " & sTradeCode & "<br />")
        sbPrint.AppendLine("Number Of Offices : " & iNumberOfOffices.ToString & "<br />")
        sbPrint.AppendLine("Company Reg : " & sCompanyReg & "<br />")
        sbPrint.AppendLine("SIC Code : " & sSICCode & "<br />")
        sbPrint.AppendLine("Turnover Code : " & sTurnoverCode & "<br />")
        sbPrint.AppendLine("Salutation : " & sSalutation & "<br />")
        sbPrint.AppendLine("Source : " & sSource & "<br />")
        sbPrint.AppendLine("Alternative Id : " & sAlternativeId & "<br />")
        sbPrint.AppendLine("TPS : " & IIf(bTPS, "true", "false") & "<br />")
        sbPrint.AppendLine("MPS : " & IIf(bMPS, "true", "false") & "<br />")
        sbPrint.AppendLine("eMPS : " & IIf(beMPS, "true", "false") & "<br />")
        sbPrint.AppendLine("Trading Since : " & dtTradingSince & "<br />")
        sbPrint.AppendLine("Financial Year : " & dtFinancialYear & "<br />")
        sbPrint.AppendLine("Wage Roll : " & dWageRoll & "<br />")
        sbPrint.AppendLine("Number Of Offices Specified : " & IIf(bNumberOfOfficesSpecified, "true", "false") & "<br />")

        Return sbPrint.ToString

    End Function

    Public Property RegistrationNumber() As String
        Get
            Return Me.sRegistrationNumber
        End Get
        Set(ByVal value As String)
            Me.sRegistrationNumber = value
        End Set
    End Property
    Public Property RegistrationName() As String
        Get
            Return Me.sRegistrationName
        End Get
        Set(ByVal value As String)
            Me.sRegistrationName = value
        End Set
    End Property

    Public Property RegistrationOffice() As String
        Get
            Return Me.sRegistrationOffice
        End Get
        Set(ByVal value As String)
            Me.sRegistrationOffice = value
        End Set
    End Property

    Public Property AnnualSales() As String
        Get
            Return Me.sAnnualSales
        End Get
        Set(ByVal value As String)
            Me.sAnnualSales = value
        End Set
    End Property

    Public Property QuaterlySales() As String
        Get
            Return Me.sQuarterlySales
        End Get
        Set(ByVal value As String)
            Me.sQuarterlySales = value
        End Set
    End Property

    Public Property BusinessName() As String
        Get
            Return Me.sBusinessName
        End Get
        Set(ByVal value As String)
            Me.sBusinessName = value
        End Set
    End Property

    Public Property PaidOfCapital() As String
        Get
            Return Me.sPaidOfCapital
        End Get
        Set(ByVal value As String)
            Me.sPaidOfCapital = value
        End Set
    End Property

    Public Property ShareCapital() As String
        Get
            Return Me.sShareCapital
        End Get
        Set(ByVal value As String)
            Me.sShareCapital = value
        End Set
    End Property

    Public Property TANNumber() As String
        Get
            Return Me.sTANNumber
        End Get
        Set(ByVal value As String)
            Me.sTANNumber = value
        End Set
    End Property

    Public Property StateTaxNumber() As String
        Get
            Return Me.sStateTaxNumber
        End Get
        Set(ByVal value As String)
            Me.sStateTaxNumber = value
        End Set
    End Property

    Public Property CentralTaxNumber() As String
        Get
            Return Me.sCentralTaxNumber
        End Get
        Set(ByVal value As String)
            Me.sCentralTaxNumber = value
        End Set
    End Property

    Public Property IsActive() As Boolean
        Get
            Return Me.bIsActive
        End Get
        Set(ByVal value As Boolean)
            Me.bIsActive = value
        End Set
    End Property

    Public Property IsPartner() As Boolean
        Get
            Return Me.bIsPartner
        End Get
        Set(ByVal value As Boolean)
            Me.bIsPartner = value
        End Set
    End Property
    Public Property BusinessCode() As String
        Get
            Return sBusinessCode
        End Get
        Set(ByVal value As String)
            sBusinessCode = value
        End Set
    End Property

    Public Property CompanyName() As String
        Get
            Return sCompanyName
        End Get
        Set(ByVal value As String)
            sCompanyName = value
        End Set
    End Property

    Public Property MainContact() As String
        Get
            Return sMainContact
        End Get
        Set(ByVal value As String)
            sMainContact = value
        End Set
    End Property

    Public Property NumberOfEmployees() As String
        Get
            Return sNumberOfEmployees
        End Get
        Set(ByVal value As String)
            sNumberOfEmployees = value
        End Set
    End Property

    Public Property TradeCode() As String
        Get
            Return sTradeCode
        End Get
        Set(ByVal value As String)
            sTradeCode = value
        End Set
    End Property

    Public Property CompanyReg() As String
        Get
            Return sCompanyReg
        End Get
        Set(ByVal value As String)
            sCompanyReg = value
        End Set
    End Property

    Public Property SICCode() As String
        Get
            Return sSICCode
        End Get
        Set(ByVal value As String)
            sSICCode = value
        End Set
    End Property

    Public Property TurnoverCode() As String
        Get
            Return sTurnoverCode
        End Get
        Set(ByVal value As String)
            sTurnoverCode = value
        End Set
    End Property

    Public Property Salutation() As String
        Get
            Return sSalutation
        End Get
        Set(ByVal value As String)
            sSalutation = value
        End Set
    End Property

    Public Property AlternativeId() As String
        Get
            Return sAlternativeId
        End Get
        Set(ByVal value As String)
            sAlternativeId = value
        End Set
    End Property

    Public Property Source() As String
        Get
            Return sSource
        End Get
        Set(ByVal value As String)
            sSource = value
        End Set
    End Property

    Public Property NumberOfOffices() As Integer
        Get
            Return iNumberOfOffices
        End Get
        Set(ByVal value As Integer)
            iNumberOfOffices = value
            bNumberOfOfficesSpecified = True
        End Set
    End Property

    Public ReadOnly Property NumberOfOfficesSpecified() As Boolean
        Get
            Return bNumberOfOfficesSpecified
        End Get
    End Property

    Public Property TPS() As Boolean
        Get
            Return bTPS
        End Get
        Set(ByVal value As Boolean)
            bTPS = value
        End Set
    End Property

    Public Property eMPS() As Boolean
        Get
            Return beMPS
        End Get
        Set(ByVal value As Boolean)
            beMPS = value
        End Set
    End Property

    Public Property MPS() As Boolean
        Get
            Return bMPS
        End Get
        Set(ByVal value As Boolean)
            bMPS = value
        End Set
    End Property

    Public Property TradingSince() As DateTime
        Get
            Return dtTradingSince
        End Get
        Set(ByVal value As DateTime)
            dtTradingSince = value
        End Set
    End Property

    Public Property FinancialYear() As DateTime
        Get
            Return dtFinancialYear
        End Get
        Set(ByVal value As DateTime)
            dtFinancialYear = value
        End Set
    End Property

    Public Property WageRoll() As Decimal
        Get
            Return dWageRoll
        End Get
        Set(ByVal value As Decimal)
            dWageRoll = value
        End Set
    End Property

    Public Property ClientSharedData() As Client
        Get
            Return oClientSharedData
        End Get
        Set(ByVal value As Client)
            oClientSharedData = value
        End Set
    End Property

    Public ReadOnly Property eMPSSpecified() As Boolean
        Get
            Return beMPSSpecified
        End Get
    End Property

    Public ReadOnly Property MPSSpecified() As Boolean
        Get
            Return bMPSSpecified
        End Get
    End Property

    Public ReadOnly Property TPSSpecified() As Boolean
        Get
            Return bTPSSpecified
        End Get
    End Property
    Public ReadOnly Property FinancialYearSpecified() As Boolean
        Get
            Return bFinancialYearSpecified
        End Get
    End Property
    Public ReadOnly Property WageRollSpecified() As Boolean
        Get
            Return bWageRollSpecified
        End Get
    End Property
    Public ReadOnly Property TradingsinceSpecified() As Boolean
        Get
            Return bTradingsinceSpecified
        End Get
    End Property

End Class


''' <summary>
''' Nexus Personal Party object
''' </summary>
<Serializable()> Public Class PersonalParty : Inherits BaseParty

    Private oClientSharedData As Client
    Private dtDateOfBirth As DateTime
    Private sAlternativeID, sEmployersBusinessCode, sForename, sLastName, sGenderCode, sInitials As String
    Private sOccupationCode, sSurname, sTitle, sTradingName, sSecOccupationCode, sSecEmployersBusinessCode, sNationalityCode, sAccommodationCode, sSalutation, sSource As String
    Private oEmploymentStatusCode, oSecEmploymentStatusCode As EmploymentStatusCodeTypes
    Private oMaritalStatusCode As MaritalStatusCodeTypes
    Private bEmploymentStatusCodeSpecified, bMaritalStatusCodeSpecified, bTPS, bMPS, beMPS, beMPSSpecified, bMPSSpecified, bPetOwner, bPetOwnerSpecified, bSecEmploymentStatusCodeSpecified, bTPSSpecified As Boolean
    Private sCustomerNumber As String
    Private sPANNumber As String
    Private sMaidenName, sJobtitle, sEmployeeNumber, sComments As String
    Private bEmployee As Boolean

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bEmploymentStatusCodeSpecified = False
        bMaritalStatusCodeSpecified = False
        bTPS = False
        bMPS = False
        beMPS = False
        bPetOwner = False
        oClientSharedData = New Client


    End Sub

    ''' <summary>
    ''' Constructor for PersonalParty
    ''' </summary>
    ''' <param name="v_iKey">Key</param>
    ''' <param name="v_sShortName">UserName / Client Code</param>
    Public Sub New(ByVal v_iKey As Integer, ByVal v_sShortName As String)
        MyBase.New(v_iKey, v_sShortName)
        bEmploymentStatusCodeSpecified = False
        bMaritalStatusCodeSpecified = False
    End Sub

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overrides Function Print() As String

        Dim sbPrint As New Text.StringBuilder(MyBase.Print())

        sbPrint.AppendLine("Date Of Birth : " & dtDateOfBirth.ToString & "<br />")
        sbPrint.AppendLine("AlternativeID : " & sAlternativeID & "<br />")
        sbPrint.AppendLine("EmployersBusinessCode : " & sEmployersBusinessCode & "<br />")
        sbPrint.AppendLine("Forename : " & sForename & "<br />")
        sbPrint.AppendLine("GenderCode : " & sGenderCode & "<br />")
        sbPrint.AppendLine("Initials : " & sInitials & "<br />")
        sbPrint.AppendLine("OccupationCode : " & sOccupationCode & "<br />")
        sbPrint.AppendLine("Surname : " & sSurname & "<br />")
        sbPrint.AppendLine("Title : " & sTitle & "<br />")
        sbPrint.AppendLine("TradingName : " & sTradingName & "<br />")
        sbPrint.AppendLine("SecOccupationCode : " & sSecOccupationCode & "<br />")
        sbPrint.AppendLine("SecEmployersBusinessCode : " & sSecEmployersBusinessCode & "<br />")
        sbPrint.AppendLine("NationalityCode : " & sNationalityCode & "<br />")
        sbPrint.AppendLine("AccommodationCode : " & sAccommodationCode & "<br />")
        sbPrint.AppendLine("Salutation : " & sSalutation & "<br />")
        sbPrint.AppendLine("Source : " & sSource & "<br />")
        sbPrint.AppendLine("Employment Status Code : " & oEmploymentStatusCode.GetName(GetType(EmploymentStatusCodeTypes), oEmploymentStatusCode) & "<br />")
        sbPrint.AppendLine("Marital Status Code : " & oMaritalStatusCode.GetName(GetType(MaritalStatusCodeTypes), oMaritalStatusCode) & "<br />")
        sbPrint.AppendLine("Employment Status Code : " & oSecEmploymentStatusCode.GetName(GetType(EmploymentStatusCodeTypes), oSecEmploymentStatusCode) & "<br />")
        sbPrint.AppendLine("Employment Status Code Specified : " & IIf(bEmploymentStatusCodeSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Marital Status Code Specified : " & IIf(bMaritalStatusCodeSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("TPS : " & IIf(bTPS, "true", "false") & "<br />")
        sbPrint.AppendLine("MPS : " & IIf(bMPS, "true", "false") & "<br />")
        sbPrint.AppendLine("eMPS : " & IIf(beMPS, "true", "false") & "<br />")
        sbPrint.AppendLine("PetOwner : " & IIf(bPetOwner, "true", "false") & "<br />")
        sbPrint.AppendLine("Client Shared data -------><br />")
        If oClientSharedData IsNot Nothing Then
            sbPrint.AppendLine(oClientSharedData.Print())
        End If

        Return sbPrint.ToString

    End Function

    ''' <summary>
    ''' Create an anonymous Personal Party, this is used for generating anonymous quotes in Nexus
    ''' </summary>
    ''' <returns>A PersonalParty object with all the attributes defaulted to anonymous</returns>
    Public Shared Function CreateAnonymous() As PersonalParty

        Dim oParty As New PersonalParty

        With oParty

            .Title = "Mr"
            .Initials = "WC"
            .Forename = "Web"
            .Surname = "Customer"
            .DateOfBirth = New Date(1899, 12, 29)

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sCountryCode As String

            Try
                'Set country code to GBR, incase we can't find any countries in the country list
                sCountryCode = "GBR"

                'Need to retrieve the country list as we can't assume any country is configured in backoffice
                Dim oItems As NexusProvider.LookupListCollection = oWebService.GetList(NexusProvider.ListType.PMLookup, "COUNTRY", True, False)

                If oItems IsNot Nothing Then

                    Dim i As Integer = 0
                    Dim bFound = False

                    While (i <= oItems.Count And Not bFound)

                        'Grab the first country code from the list that hasn't been deleted.
                        If Not oItems(i).IsDeleted Then
                            sCountryCode = oItems.Item(i).Code
                            bFound = True
                        End If

                        i += 1

                    End While

                End If

            Finally
                oWebService = Nothing
            End Try

            Dim oAddress As New NexusProvider.Address("2500 The Crescent", "B37 7YE", sCountryCode)
            oAddress.Address2 = "Birmingham Business Park"
            oAddress.Address3 = "Solihull"

            .Addresses.Add(oAddress)
            .Contacts.Add(New NexusProvider.Contact(ContactType.HomePhone, "0121", "779 8400"))
            .Contacts.Add(New NexusProvider.Contact(ContactType.Email, "noreply@ssp-uk.com"))

        End With

        Return oParty

    End Function
    Public Property IsEmployee() As Boolean
        Get
            Return bEmployee
        End Get
        Set(ByVal value As Boolean)
            bEmployee = value
        End Set
    End Property
    Public Property PANNumber() As String
        Get
            Return sPANNumber
        End Get
        Set(ByVal value As String)
            sPANNumber = value
        End Set
    End Property
    Public Property CustomerNumber() As String
        Get
            Return sCustomerNumber
        End Get
        Set(ByVal value As String)
            sCustomerNumber = value
        End Set
    End Property

    Public Property DateOfBirth() As DateTime
        Get
            Return dtDateOfBirth
        End Get
        Set(ByVal value As DateTime)
            'Year zero between the web service and vb don't match so check for it and correct to vb year zero
            '  If value = #12/29/1899# Or value = #12/30/1899# Then
            'dtDateOfBirth = value ' System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper() 'DateTime.MinValue
            ' Else
            dtDateOfBirth = value
            'End If
        End Set
    End Property
    Public Property MaidenName() As String
        Get
            Return sMaidenName
        End Get
        Set(ByVal value As String)
            sMaidenName = value
        End Set
    End Property
    Public Property Jobtitle() As String
        Get
            Return sJobtitle
        End Get
        Set(ByVal value As String)
            sJobtitle = value
        End Set
    End Property
    Public Property EmployeeNumber() As String
        Get
            Return sEmployeeNumber
        End Get
        Set(ByVal value As String)
            sEmployeeNumber = value
        End Set
    End Property
    Public Property Comments() As String
        Get
            Return sComments
        End Get
        Set(ByVal value As String)
            sComments = value
        End Set
    End Property

    Public ReadOnly Property DateOfBirthSpecified() As Boolean
        Get
            Return IIf(dtDateOfBirth = DateTime.MinValue, False, True)
        End Get
    End Property

    Public Property AlternativeID() As String
        Get
            Return sAlternativeID
        End Get
        Set(ByVal value As String)
            sAlternativeID = value
        End Set
    End Property

    Public Property EmployersBusinessCode() As String
        Get
            Return sEmployersBusinessCode
        End Get
        Set(ByVal value As String)
            sEmployersBusinessCode = value
        End Set
    End Property

    Public Property Forename() As String
        Get
            Return sForename
        End Get
        Set(ByVal value As String)
            sForename = value
        End Set
    End Property
    Public Property Lastname() As String
        Get
            Return sLastName
        End Get
        Set(ByVal value As String)
            sLastName = value
        End Set
    End Property
    Public Property GenderCode() As String
        Get
            Return sGenderCode
        End Get
        Set(ByVal value As String)
            sGenderCode = value
        End Set
    End Property

    Public Property Initials() As String
        Get
            Return sInitials
        End Get
        Set(ByVal value As String)
            sInitials = value
        End Set
    End Property

    Public Property OccupationCode() As String
        Get
            Return sOccupationCode
        End Get
        Set(ByVal value As String)
            sOccupationCode = value
        End Set
    End Property

    Public Property Surname() As String
        Get
            Return sSurname
        End Get
        Set(ByVal value As String)
            sSurname = value
        End Set
    End Property

    Public Property Title() As String
        Get
            Return sTitle
        End Get
        Set(ByVal value As String)
            sTitle = value
        End Set
    End Property

    Public Property TradingName() As String
        Get
            Return sTradingName
        End Get
        Set(ByVal value As String)
            sTradingName = value
        End Set
    End Property

    Public Property SecOccupationCode() As String
        Get
            Return sSecOccupationCode
        End Get
        Set(ByVal value As String)
            sSecOccupationCode = value
        End Set
    End Property

    Public Property NationalityCode() As String
        Get
            Return sNationalityCode
        End Get
        Set(ByVal value As String)
            sNationalityCode = value
        End Set
    End Property

    Public Property AccommodationCode() As String
        Get
            Return sAccommodationCode
        End Get
        Set(ByVal value As String)
            sAccommodationCode = value
        End Set
    End Property
    Public Property SecEmployersBusinessCode() As String
        Get
            Return sSecEmployersBusinessCode
        End Get
        Set(ByVal value As String)
            sSecEmployersBusinessCode = value
        End Set
    End Property

    Public Property Salutation() As String
        Get
            Return sSalutation
        End Get
        Set(ByVal value As String)
            sSalutation = value
        End Set
    End Property

    Public Property Source() As String
        Get
            Return sSource
        End Get
        Set(ByVal value As String)
            sSource = value
        End Set
    End Property

    Public Property EmploymentStatusCode() As EmploymentStatusCodeTypes
        Get
            Return oEmploymentStatusCode
        End Get
        Set(ByVal value As EmploymentStatusCodeTypes)
            oEmploymentStatusCode = value
        End Set
    End Property

    Public Property SecEmploymentStatusCode() As EmploymentStatusCodeTypes
        Get
            Return oSecEmploymentStatusCode
        End Get
        Set(ByVal value As EmploymentStatusCodeTypes)
            oSecEmploymentStatusCode = value
        End Set
    End Property

    Public ReadOnly Property EmploymentStatusCodeSpecified() As Boolean
        Get
            Return bEmploymentStatusCodeSpecified
        End Get
    End Property

    Public Property TPS() As Boolean
        Get
            Return bTPS
        End Get
        Set(ByVal value As Boolean)
            bTPS = value
        End Set
    End Property

    Public Property eMPS() As Boolean
        Get
            Return beMPS
        End Get
        Set(ByVal value As Boolean)
            beMPS = value
        End Set
    End Property

    Public Property MPS() As Boolean
        Get
            Return bMPS
        End Get
        Set(ByVal value As Boolean)
            bMPS = value
        End Set
    End Property

    Public Property PetOwner() As Boolean
        Get
            Return bPetOwner
        End Get
        Set(ByVal value As Boolean)
            bPetOwner = value
        End Set
    End Property

    Public Property MaritalStatusCode() As MaritalStatusCodeTypes
        Get
            Return oMaritalStatusCode
        End Get
        Set(ByVal value As MaritalStatusCodeTypes)
            oMaritalStatusCode = value
            bMaritalStatusCodeSpecified = True
        End Set
    End Property

    Public Property MaritalStatusCodeSpecified() As Boolean
        Get
            Return bMaritalStatusCodeSpecified
        End Get
        Set(ByVal value As Boolean)
            bMaritalStatusCodeSpecified = value
        End Set
    End Property

    Public Property ClientSharedData() As Client
        Get
            Return oClientSharedData
        End Get
        Set(ByVal value As Client)
            oClientSharedData = value
        End Set
    End Property

    Public ReadOnly Property eMPSSpecified() As Boolean
        Get
            Return beMPSSpecified
        End Get
    End Property
    Public ReadOnly Property MPSSpecified() As Boolean
        Get
            Return bMPSSpecified
        End Get
    End Property
    Public ReadOnly Property PetOwnerSpecified() As Boolean
        Get
            Return bPetOwnerSpecified
        End Get
    End Property
    Public ReadOnly Property SecEmploymentStatusCodeSpecified() As Boolean
        Get
            Return bSecEmploymentStatusCodeSpecified
        End Get
    End Property
    Public ReadOnly Property TPSSpecified() As Boolean
        Get
            Return bTPSSpecified
        End Get
    End Property


End Class

<Serializable()> Public Class OtherParty : Inherits BaseParty
    Private oConviction As Convictions
    Private oAccident As Accident
    Private oBranches As BranchCollection
    Private sCode, sName, sTypeCode, sLicenseTypeCode, sLicenseNumber, sGender, sDriverStatusCode, sRegNumber As String
    Private bActiveIndicator, bAfterHoursIndicator As Boolean
    Private iPriorityIndicator As Integer
    Private dtDateOfBirth As Date
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bActiveIndicator = False
        bAfterHoursIndicator = False
        oConviction = New Convictions
        oAccident = New Accident
        oBranches = New BranchCollection


    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overrides Function Print() As String

        Dim sbPrint As New Text.StringBuilder(MyBase.Print())


        sbPrint.AppendLine("Code : " & sCode & "<br />")
        sbPrint.AppendLine("Name : " & sName & "<br />")
        sbPrint.AppendLine("Type Code : " & sTypeCode & "<br />")
        sbPrint.AppendLine("License Type Code : " & sLicenseTypeCode & "<br />")
        sbPrint.AppendLine("Date Of Birth : " & dtDateOfBirth & "<br />")
        sbPrint.AppendLine("Gender : " & sGender & "<br />")
        sbPrint.AppendLine("Driver Status Code : " & sDriverStatusCode & "<br />")
        sbPrint.AppendLine("Reg Number : " & sRegNumber & "<br />")
        sbPrint.AppendLine("Priority Indicator : " & iPriorityIndicator & "<br />")
        sbPrint.AppendLine("ActiveIndicator : " & IIf(bActiveIndicator, "true", "false") & "<br />")
        sbPrint.AppendLine("AfterHoursIndicator : " & IIf(bAfterHoursIndicator, "true", "false") & "<br />")
        sbPrint.AppendLine("Covictions -------><br />")
        If oConviction IsNot Nothing Then
            sbPrint.AppendLine(oConviction.Print())
        End If
        sbPrint.AppendLine("Accident -------><br />")
        If oAccident IsNot Nothing Then
            sbPrint.AppendLine(oAccident.Print())
        End If
        sbPrint.AppendLine("Branches -------><br />")
        If oBranches IsNot Nothing Then
            sbPrint.AppendLine(oBranches.Print())
        End If

        Return sbPrint.ToString

    End Function

    Public Property Code() As String
        Get
            Return sCode
        End Get
        Set(ByVal value As String)
            sCode = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return sName
        End Get
        Set(ByVal value As String)
            sName = value
        End Set
    End Property

    Public Property TypeCode() As String
        Get
            Return sTypeCode
        End Get
        Set(ByVal value As String)
            sTypeCode = value
        End Set
    End Property

    Public Property LicenseTypeCode() As String
        Get
            Return sLicenseTypeCode
        End Get
        Set(ByVal value As String)
            sLicenseTypeCode = value
        End Set
    End Property

    Public Property LicenseNumber() As String
        Get
            Return sLicenseNumber
        End Get
        Set(ByVal value As String)
            sLicenseNumber = value
        End Set
    End Property

    Public Property DateOfBirth() As Date
        Get
            Return dtDateOfBirth
        End Get
        Set(ByVal value As Date)
            dtDateOfBirth = value
        End Set
    End Property

    Public Property Gender() As String
        Get
            Return sGender
        End Get
        Set(ByVal value As String)
            sGender = value
        End Set
    End Property

    Public Property DriverStatusCode() As String
        Get
            Return sDriverStatusCode
        End Get
        Set(ByVal value As String)
            sDriverStatusCode = value
        End Set
    End Property

    Public Property RegistrationNumber() As String
        Get
            Return sRegNumber
        End Get
        Set(ByVal value As String)
            sRegNumber = value
        End Set
    End Property

    Public Property ActiveIndicator() As Boolean
        Get
            Return bActiveIndicator
        End Get
        Set(ByVal value As Boolean)
            bActiveIndicator = value
        End Set
    End Property

    Public Property AfterHoursIndicator() As Boolean
        Get
            Return bAfterHoursIndicator
        End Get
        Set(ByVal value As Boolean)
            bAfterHoursIndicator = value
        End Set
    End Property

    Public Property PriorityIndicator() As Integer
        Get
            Return iPriorityIndicator
        End Get
        Set(ByVal value As Integer)
            iPriorityIndicator = value
        End Set
    End Property

    Public Property Accident() As Accident
        Get
            Return oAccident
        End Get
        Set(ByVal value As Accident)
            oAccident = value
        End Set
    End Property

    Public Property Conviction() As Convictions
        Get
            Return oConviction
        End Get
        Set(ByVal value As Convictions)
            oConviction = value
        End Set
    End Property

    Public Property Branches() As BranchCollection
        Get
            Return oBranches
        End Get
        Set(ByVal value As BranchCollection)
            oBranches = value
        End Set
    End Property

    Public Property IsTPASettleDirectly() As Integer

    Public Property SubBranchCode() As String

End Class

''' <summary>
''' Type of employment of the Party
''' </summary>
''' <remarks></remarks>
Public Enum EmploymentStatusCodeTypes

    ''' <summary>
    ''' Company
    ''' </summary>
    ''' <remarks>Code C in BackOffice</remarks>
    Company = 0

    ''' <summary>
    ''' Employed
    ''' </summary>
    ''' <remarks>Code E in BackOffice</remarks>
    Employed = 1

    ''' <summary>
    ''' Household Duties
    ''' </summary>
    ''' <remarks>Code H in BackOffice</remarks>
    HouseholdDuties = 2

    ''' <summary>
    ''' Full / Part time Eduction
    ''' </summary>
    ''' <remarks>Code F in BackOffice</remarks>
    FullPartTimeEduction = 3

    ''' <summary>
    ''' Independent
    ''' </summary>
    ''' <remarks>Code I in BackOffice</remarks>
    Independent = 4

    ''' <summary>
    ''' Not Employed Due to Disability
    ''' </summary>
    ''' <remarks>Code N in BackOffice</remarks>
    NotEmployedDueToDisability = 5

    ''' <summary>
    ''' Retired
    ''' </summary>
    ''' <remarks>Code R in BackOffice</remarks>
    Retired = 6

    ''' <summary>
    ''' Self Employed
    ''' </summary>
    ''' <remarks>Code S in BackOffice</remarks>
    SelfEmpolyed = 7

    ''' <summary>
    ''' Unemployed
    ''' </summary>
    ''' <remarks>Code U in BackOffice</remarks>
    Unemployed = 8

    ''' <summary>
    ''' Voluntary Work
    ''' </summary>
    ''' <remarks>Code V in BackOffice</remarks>
    VoluntaryWork = 9

End Enum


''' <summary>
''' Marital status of party
''' </summary>
''' <remarks>Only for Personal Parties</remarks>
Public Enum MaritalStatusCodeTypes

    ''' <summary>
    ''' Divorced
    ''' </summary>
    ''' <remarks>Code D in BackOffice</remarks>
    Divorced = 0

    ''' <summary>
    ''' Common Law
    ''' </summary>
    ''' <remarks>Code C in BackOffice</remarks>
    CommonLaw = 1

    ''' <summary>
    ''' Married
    ''' </summary>
    ''' <remarks>Code M in BackOffice</remarks>
    Married = 2

    ''' <summary>
    ''' Not Applicable
    ''' </summary>
    ''' <remarks>Code N in BackOffice</remarks>
    NotApplicable = 3

    ''' <summary>
    ''' Not Available
    ''' </summary>
    ''' <remarks>Code O in BackOffice</remarks>
    NotAvailable = 4

    ''' <summary>
    ''' Partnered
    ''' </summary>
    ''' <remarks>Code P in BackOffice</remarks>
    Partnered = 5

    ''' <summary>
    ''' Seperated
    ''' </summary>
    ''' <remarks>Code A in BackOffice</remarks>
    Separated = 6

    ''' <summary>
    ''' Single
    ''' </summary>
    ''' <remarks>Code S in BackOffice</remarks>
    Single_ = 7

    ''' <summary>
    ''' Widowed
    ''' </summary>
    ''' <remarks>Code W in BackOffice</remarks>
    Widowed = 8

End Enum

''' <summary>
''' Collection of BaseParty objects
''' </summary>
<Serializable()> Public Class PartyCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oParty As BaseParty In List
            sbPrint.AppendLine(oParty.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a BaseParty object to the collection
    ''' </summary>
    ''' <param name="v_oParty">The BaseParty object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oParty As BaseParty) As Integer
        Return List.Add(v_oParty)
    End Function

    ''' <summary>
    ''' Remove an BaseParty object from the collection
    ''' </summary>
    ''' <param name="v_oParty">The BaseParty object to be removed</param>
    Public Sub Remove(ByVal v_oParty As BaseParty)
        List.Remove(v_oParty)
    End Sub

    ''' <summary>
    ''' Sort the collection
    ''' </summary>
    ''' <param name="oItem">BaseParty attribute to sort by</param>
    ''' <param name="oDirection">Sort order</param>
    Public Sub Sort(ByVal oItem As PartySort, ByVal oDirection As Direction)

        Select Case oItem
            Case PartySort.Key
                InnerList.Sort(New BaseParty.SortByKey())
            Case PartySort.ResolvedName
                InnerList.Sort(New BaseParty.SortByResolvedName())
            Case PartySort.UserName
                InnerList.Sort(New BaseParty.SortByUserName())
        End Select

        If oDirection = Direction.Desc Then
            InnerList.Reverse()
        End If

    End Sub

    ''' <summary>
    ''' Retrieve or replace an BaseParty object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the BaseParty object</param>
    ''' <value>The replacement BaseParty object</value>
    ''' <returns>The BaseParty object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As BaseParty
        Get
            Return List(i)
        End Get
        Set(ByVal value As BaseParty)
            List(i) = value
        End Set
    End Property

End Class

''' <summary>
''' Attribute to sort Party Collection by
''' </summary>
''' <remarks></remarks>
Public Enum PartySort

    ''' <summary>
    ''' Party Key
    ''' </summary>
    Key = 0

    ''' <summary>
    ''' Username
    ''' </summary>
    UserName = 1

    ''' <summary>
    ''' Resolved Name
    ''' </summary>
    ''' <remarks>For Personal parties this will be Title, FirstName and Surname and
    ''' for Corporate parties Company Name</remarks>
    ResolvedName = 2

End Enum

''' <summary>
''' Direction of Sort
''' </summary>
Public Enum Direction

    ''' <summary>
    ''' Ascending
    ''' </summary>
    Asc = 0

    ''' <summary>
    ''' Descending
    ''' </summary>
    Desc = 1
End Enum
