Imports System.Web.HttpContext

<Serializable()> Public Class Risk

    Private iKey, iFolderKey, iRiskKey, iRiskNumber, iInsuranceFileKey, iInsuranceFolderKey, iScreenKey As Integer
    Private sDescription, sScreenCode, sRiskCode, sDataModelCode, sXMLDataset, sRiskTypeCode, sStatusCode, sDatasetSchema As String
    Private dTotalSumInsured, dCommissionAmount, dPremiumDueGross As Decimal
    Private dPremiumDueNet, dPremiumDueTax, dTotalAnnualTax As Decimal
    Private bQuoteTimeStamp() As Byte
    Private sCopyType As Enum_CopyRiskType
    Private dPolicyLevelTax As Decimal
    Private dPolicyLevelFees As Decimal

    Private iProRata As Integer
    Private dProRataRate As Decimal
    Private sProRataMessage As String

    Private dPremium, dFeeTax, dFeePremium, dRiskFee As Double
    Private bIsRisk, bDiscounted As Boolean
    Private iVariation As Integer
    Private sStatusDescription, sCoverage, sInsuredItem, sExtensions As String
    Private dtStartDate, dtEndDate As Date

    Dim oRiskTaxes As TaxCollection
    Dim oPolicyFees As FeeCollection
    Dim oPolicyTaxes As TaxCollection
    Dim oRiskFees As FeeCollection
    'For PolicyLevelTaxes
    ' Private oPolicyTaxesAndFees As TaxesAndFeesType
    'For RiskLevelTaxes And Fees
    Private oRiskTaxesAndFees As TaxesAndFeesType
    Private bIsRiskSelected, bIsMandatoryRisk As Boolean
    Private iOriginalRiskKey As Integer
    Private GISRetroactiveDateField As Date

    Private RiskInceptionDateField As Date
    Private bHasClaimLink As Boolean
    Private sRiskLinkStatusFlag As String
    Private dtRiskLinkChangeDate As DateTime

    
    Public Sub New()
        MyBase.New()
        oRiskTaxes = New TaxCollection
        oPolicyFees = New FeeCollection
        oPolicyTaxes = New TaxCollection
        oRiskFees = New FeeCollection
    End Sub

    Public Sub New(ByVal v_sScreenCode As String, ByVal v_sRiskDescription As String)

        oRiskTaxes = New TaxCollection
        oPolicyFees = New FeeCollection
        oPolicyTaxes = New TaxCollection
        oRiskFees = New FeeCollection

        sScreenCode = v_sScreenCode
        sDescription = v_sRiskDescription

    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Key : " & iKey.ToString() & "<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")
        sbPrint.AppendLine("Screen Code : " & sScreenCode & "<br />")
        sbPrint.AppendLine("Risk Code : " & sRiskCode & "<br />")
        sbPrint.AppendLine("Data Model Code : " & sDataModelCode & "<br />")
        If sXMLDataset IsNot Nothing Then
            sbPrint.AppendLine("XML Dataset : " & ProviderHelper.PrettyFormatXMLToHTML(sXMLDataset))
        End If
        sbPrint.AppendLine("RiskType Code : " & sRiskTypeCode & "<br />")
        sbPrint.AppendLine("Status Code : " & sStatusCode & "<br />")
        sbPrint.AppendLine("Total Sum Insured : " & dTotalSumInsured.ToString() & "<br />")
        sbPrint.AppendLine("Commission Amount : " & dCommissionAmount.ToString() & "<br />")
        sbPrint.AppendLine("Premium Due Gross : " & dPremiumDueGross & "<br />")
        sbPrint.AppendLine("Premium Due Net : " & dPremiumDueNet.ToString() & "<br />")
        sbPrint.AppendLine("Premium Due Tax : " & dPremiumDueTax.ToString() & "<br />")
        sbPrint.AppendLine("Total Annual Tax : " & dTotalAnnualTax.ToString() & "<br />")
        sbPrint.AppendLine("Original Risk Key : " & iOriginalRiskKey.ToString & "<br />")
        sbPrint.AppendLine("Risk Link Status Flag : " & sRiskLinkStatusFlag & "<br />")
        sbPrint.AppendLine("Risk Link Change Date : " & dtRiskLinkChangeDate & "<br />")

        Return sbPrint.ToString()

    End Function
    ''' <summary>
    ''' Mandatory Risk for Policy
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsMandatoryRisk() As Boolean
        Get
            Return bIsMandatoryRisk
        End Get
        Set(ByVal value As Boolean)
            bIsMandatoryRisk = value
        End Set
    End Property

    Public Property IsRiskSelected() As Boolean
        Get
            Return bIsRiskSelected
        End Get
        Set(ByVal value As Boolean)
            bIsRiskSelected = value
        End Set
    End Property
    Public Property EndDate() As Date
        Get
            Return dtEndDate
        End Get
        Set(ByVal value As Date)
            dtEndDate = value
        End Set
    End Property
    Public Property StartDate() As Date
        Get
            Return dtStartDate
        End Get
        Set(ByVal value As Date)
            dtStartDate = value
        End Set
    End Property
    Public Property Extensions() As String
        Get
            Return sExtensions
        End Get
        Set(ByVal value As String)
            sExtensions = value
        End Set
    End Property
    Public Property InsuredItem() As String
        Get
            Return sInsuredItem
        End Get
        Set(ByVal value As String)
            sInsuredItem = value
        End Set
    End Property
    Public Property Coverage() As String
        Get
            Return sCoverage
        End Get
        Set(ByVal value As String)
            sCoverage = value
        End Set
    End Property
    Public Property StatusDescription() As String
        Get
            Return sStatusDescription
        End Get
        Set(ByVal value As String)
            sStatusDescription = value
        End Set
    End Property
    Public Property Variation() As Integer
        Get
            Return iVariation
        End Get
        Set(ByVal value As Integer)
            iVariation = value
        End Set
    End Property
    Public Property Discounted() As Boolean
        Get
            Return bDiscounted
        End Get
        Set(ByVal value As Boolean)
            bDiscounted = value
        End Set
    End Property
    Public Property IsRisk() As Boolean
        Get
            Return bIsRisk
        End Get
        Set(ByVal value As Boolean)
            bIsRisk = value
        End Set
    End Property
    Public Property RiskFee() As Double
        Get
            Return dRiskFee
        End Get
        Set(ByVal value As Double)
            dRiskFee = value
        End Set
    End Property
    Public Property FeePremium() As Double
        Get
            Return dFeePremium
        End Get
        Set(ByVal value As Double)
            dFeePremium = value
        End Set
    End Property
    Public Property FeeTax() As Double
        Get
            Return dFeeTax
        End Get
        Set(ByVal value As Double)
            dFeeTax = value
        End Set
    End Property
    Public Property Premium() As Double
        Get
            Return dPremium
        End Get
        Set(ByVal value As Double)
            dPremium = value
        End Set
    End Property

    Public Property Key() As Integer
        Get
            Return iKey
        End Get
        Set(ByVal value As Integer)
            iKey = value
        End Set
    End Property

    Public Property ScreenKey() As Integer
        Get
            Return iScreenKey
        End Get
        Set(ByVal value As Integer)
            iScreenKey = value
        End Set
    End Property
    Public Property FolderKey() As Integer
        Get
            Return iFolderKey
        End Get
        Set(ByVal value As Integer)
            iFolderKey = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return sDescription
        End Get
        Set(ByVal value As String)
            sDescription = value
        End Set
    End Property

    Public Property ScreenCode() As String
        Get
            Return sScreenCode
        End Get
        Set(ByVal value As String)
            sScreenCode = value
        End Set
    End Property

    Public Property RiskCode() As String
        Get
            Return sRiskCode
        End Get
        Set(ByVal value As String)
            sRiskCode = value
        End Set
    End Property

    Public Property DataModelCode() As String
        Get
            Return sDataModelCode
        End Get
        Set(ByVal value As String)
            sDataModelCode = value
        End Set
    End Property

    Public Property XMLDataset() As String
        Get
            Return sXMLDataset
        End Get
        Set(ByVal value As String)
            sXMLDataset = value
        End Set
    End Property

    Public Property RiskTypeCode() As String
        Get
            Return sRiskTypeCode
        End Get
        Set(ByVal value As String)
            sRiskTypeCode = value
        End Set
    End Property

    Public Property StatusCode() As String
        Get
            Return sStatusCode
        End Get
        Set(ByVal value As String)
            sStatusCode = value
        End Set
    End Property

    Public Property TotalSumInsured() As Decimal
        Get
            Return dTotalSumInsured
        End Get
        Set(ByVal value As Decimal)
            dTotalSumInsured = value
        End Set
    End Property

    Public Property CommissionAmount() As Decimal
        Get
            Return dCommissionAmount
        End Get
        Set(ByVal value As Decimal)
            dCommissionAmount = value
        End Set
    End Property

    Public Property PremiumDueGross() As Decimal
        Get
            Return dPremiumDueGross
        End Get
        Set(ByVal value As Decimal)
            dPremiumDueGross = value
        End Set
    End Property

    Public Property PremiumDueNet() As Decimal
        Get
            Return dPremiumDueNet
        End Get
        Set(ByVal value As Decimal)
            dPremiumDueNet = value
        End Set
    End Property

    Public Property PremiumDueTax() As Decimal
        Get
            Return dPremiumDueTax
        End Get
        Set(ByVal value As Decimal)
            dPremiumDueTax = value
        End Set
    End Property

    Public Property TotalAnnualTax() As Decimal
        Get
            Return dTotalAnnualTax
        End Get
        Set(ByVal value As Decimal)
            dTotalAnnualTax = value
        End Set
    End Property
    Public Property CopyType() As Enum_CopyRiskType
        Get
            Return sCopyType
        End Get
        Set(ByVal value As Enum_CopyRiskType)
            sCopyType = value
        End Set
    End Property
    Public Property InsuranceFileKey() As Integer
        Get
            Return iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFileKey = value
        End Set
    End Property
    Public Property InsuranceFolderKey() As Integer
        Get
            Return iInsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFolderKey = value
        End Set
    End Property

    Public Property RiskKey() As Integer
        Get
            Return iRiskKey
        End Get
        Set(ByVal value As Integer)
            iRiskKey = value
        End Set
    End Property
    Public Property RiskNumber() As Integer
        Get
            Return iRiskNumber
        End Get
        Set(ByVal value As Integer)
            iRiskNumber = value
        End Set
    End Property
    Public Property QuoteTimeStamp() As Byte()
        Get
            Return bQuoteTimeStamp
        End Get
        Set(ByVal value As Byte())
            bQuoteTimeStamp = value
        End Set
    End Property

    Public Property DatasetSchema() As String
        Get
            Return sDatasetSchema
        End Get
        Set(ByVal value As String)
            sDatasetSchema = value
        End Set
    End Property
    Public Property ProRataRate() As Decimal
        Get
            Return dProRataRate
        End Get
        Set(ByVal value As Decimal)
            dProRataRate = value
        End Set
    End Property
    Public Property ProRata() As Integer
        Get
            Return iProRata
        End Get
        Set(ByVal value As Integer)
            iProRata = value
        End Set
    End Property
    Public Property PolicyLevelFees() As Decimal
        Get
            Return dPolicyLevelFees
        End Get
        Set(ByVal value As Decimal)
            dPolicyLevelFees = value
        End Set
    End Property
    Public Property PolicyLevelTax() As Decimal
        Get
            Return dPolicyLevelTax
        End Get
        Set(ByVal value As Decimal)
            dPolicyLevelTax = value
        End Set
    End Property

    Public Property ProRataMessage() As String
        Get
            Return sProRataMessage
        End Get
        Set(ByVal value As String)
            sProRataMessage = value
        End Set
    End Property
    Public Property RiskLevelTaxesAndFee() As TaxesAndFeesType
        Get
            Return oRiskTaxesAndFees
        End Get
        Set(ByVal value As TaxesAndFeesType)
            oRiskTaxesAndFees = value
        End Set
    End Property
    Public Property PolicyFees() As FeeCollection
        Get
            Return oPolicyFees
        End Get
        Set(ByVal value As FeeCollection)
            oPolicyFees = value
        End Set
    End Property
    Public Property RiskTaxes() As TaxCollection
        Get
            Return oRiskTaxes
        End Get
        Set(ByVal value As TaxCollection)
            oRiskTaxes = value
        End Set
    End Property
    Public Property PolicyTaxes() As TaxCollection
        Get
            Return oPolicyTaxes
        End Get
        Set(ByVal value As TaxCollection)
            oPolicyTaxes = value
        End Set
    End Property
    Public Property RiskFees() As FeeCollection
        Get
            Return oRiskFees
        End Get
        Set(ByVal value As FeeCollection)
            oRiskFees = value
        End Set
    End Property

    Public Property OriginalRiskKey() As Integer
        Get
            Return iOriginalRiskKey
        End Get
        Set(ByVal value As Integer)
            iOriginalRiskKey = value
        End Set
    End Property

    Public Property GISRetroactiveDate() As Date
        Get
            Return Me.GISRetroactiveDateField
        End Get
        Set(value As Date)
            Me.GISRetroactiveDateField = value
        End Set
    End Property


    Public Property RiskInceptionDate() As Date
        Get
            Return Me.RiskInceptionDateField
        End Get
        Set(value As Date)
            Me.RiskInceptionDateField = value
        End Set
    End Property

    Public Property HasClaimLink() As Boolean
        Get
            Return bHasClaimLink
        End Get
        Set(ByVal value As Boolean)
            bHasClaimLink = value
        End Set
    End Property

    Public Property RiskLinkStatusFlag() As String
        Get
            Return sRiskLinkStatusFlag
        End Get
        Set(ByVal value As String)
            sRiskLinkStatusFlag = value
        End Set
    End Property
    ''' <summary>
    ''' Risk changed date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RiskLinkChangeDate() As DateTime
        Get
            Return dtRiskLinkChangeDate
        End Get
        Set(ByVal value As DateTime)
            dtRiskLinkChangeDate = value
        End Set
    End Property


    ''' <remarks />
    Public Property HasFacProp() As Boolean

    ''' <remarks />
    Public Property IsAutoRated() As Boolean

    ''' <summary>
    ''' A flag to idetify that risk is editable during backdated MTA
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsRiskEditableForBackDatedMTA As Boolean

    ''' <summary>
    ''' ReturnPremiumMoreThanBilled
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReturnPremiumMoreThanBilled() As Boolean

    ''' <summary>
    '''CopyRisk Type
    ''' </summary>
    Enum Enum_CopyRiskType
        ''' <summary>
        ''' Comparative
        ''' </summary>
        Comparative

        ''' <summary>
        '''Duplicate
        ''' </summary>
        Duplicate

    End Enum

End Class

<Serializable()> Public Class RiskCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(Risk)
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oRisk As Risk In List
            sbPrint.AppendLine(oRisk.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oRisk As Risk) As Integer
        Return List.Add(v_oRisk)
    End Function

    Public Sub Remove(ByVal v_oRisk As Risk)
        List.Remove(v_oRisk)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Risk
        Get
            Return List(i)
        End Get
        Set(ByVal value As Risk)
            List(i) = value
        End Set
    End Property

    Public Function FindItemByRiskKey(ByVal v_iRiskKey As Integer) As Risk

        For Each oRisk As Risk In List
            If oRisk.Key = v_iRiskKey Then
                Return oRisk
            End If
        Next

        Return Nothing

    End Function

End Class
<Serializable()> Partial Public Class TaxesAndFeesType

    Private oFees() As FeeCollection

    Private oTaxes() As TaxCollection

    '''<remarks/>
    Public Property Fees() As FeeCollection()
        Get
            Return Me.oFees
        End Get
        Set(ByVal value As FeeCollection())
            Me.oFees = value
        End Set
    End Property

    '''<remarks/>
    Public Property Taxes() As TaxCollection()
        Get
            Return Me.oTaxes
        End Get
        Set(ByVal value As TaxCollection())
            Me.oTaxes = value
        End Set
    End Property


End Class


