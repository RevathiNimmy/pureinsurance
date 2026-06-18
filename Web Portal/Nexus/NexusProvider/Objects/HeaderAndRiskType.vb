<Serializable()> Public Class HeaderAndRiskType
    Private iRiskKey As Integer

    Private iRiskFolderKey As Integer

    Private sRiskTypeCode As String

    Private sDescription As String

    Private dTotalSumInsured As Double

    Private dPremium As Double

    Private sStatusCode As String

    Private bIsRisk As Boolean

    Private iRiskNumber As Integer

    Private bDiscounted As Boolean

    Private iVariation As Integer

    Private sStatusDescription As String

    Private sCoverage As String

    Private sInsuredItem As String

    Private sExtensions As String

    Private dFeeTax As Double

    Private dFeePremium As Double

    Private dStartDate As Date

    Private dEndDate As Date

    Private dRiskTax As Double

    Private sRiskTypeDescription As String

    '''<remarks/>
    Public Property RiskKey() As Integer
        Get
            Return Me.iRiskKey
        End Get
        Set(ByVal value As Integer)
            Me.iRiskKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property RiskFolderKey() As Integer
        Get
            Return Me.iRiskFolderKey
        End Get
        Set(ByVal value As Integer)
            Me.iRiskFolderKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property RiskTypeCode() As String
        Get
            Return Me.sRiskTypeCode
        End Get
        Set(ByVal value As String)
            Me.sRiskTypeCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalSumInsured() As Double
        Get
            Return Me.dTotalSumInsured
        End Get
        Set(ByVal value As Double)
            Me.dTotalSumInsured = value
        End Set
    End Property

    '''<remarks/>
    Public Property Premium() As Double
        Get
            Return Me.dPremium
        End Get
        Set(ByVal value As Double)
            Me.dPremium = value
        End Set
    End Property

    '''<remarks/>
    Public Property StatusCode() As String
        Get
            Return Me.sStatusCode
        End Get
        Set(ByVal value As String)
            Me.sStatusCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsRisk() As Boolean
        Get
            Return Me.bIsRisk
        End Get
        Set(ByVal value As Boolean)
            Me.bIsRisk = value
        End Set
    End Property

    '''<remarks/>
    Public Property RiskNumber() As Integer
        Get
            Return Me.iRiskNumber
        End Get
        Set(ByVal value As Integer)
            Me.iRiskNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property Discounted() As Boolean
        Get
            Return Me.bDiscounted
        End Get
        Set(ByVal value As Boolean)
            Me.bDiscounted = value
        End Set
    End Property

    '''<remarks/>
    Public Property Variation() As Integer
        Get
            Return Me.iVariation
        End Get
        Set(ByVal value As Integer)
            Me.iVariation = value
        End Set
    End Property

    '''<remarks/>
    Public Property StatusDescription() As String
        Get
            Return Me.sStatusDescription
        End Get
        Set(ByVal value As String)
            Me.sStatusDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property Coverage() As String
        Get
            Return Me.sCoverage
        End Get
        Set(ByVal value As String)
            Me.sCoverage = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuredItem() As String
        Get
            Return Me.sInsuredItem
        End Get
        Set(ByVal value As String)
            Me.sInsuredItem = value
        End Set
    End Property

    '''<remarks/>
    Public Property Extensions() As String
        Get
            Return Me.sExtensions
        End Get
        Set(ByVal value As String)
            Me.sExtensions = value
        End Set
    End Property

    '''<remarks/>
    Public Property FeeTax() As Double
        Get
            Return Me.dFeeTax
        End Get
        Set(ByVal value As Double)
            Me.dFeeTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property FeePremium() As Double
        Get
            Return Me.dFeePremium
        End Get
        Set(ByVal value As Double)
            Me.dFeePremium = value
        End Set
    End Property

    '''<remarks/>
    Public Property StartDate() As Date
        Get
            Return Me.dStartDate
        End Get
        Set(ByVal value As Date)
            Me.dStartDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property EndDate() As Date
        Get
            Return Me.dEndDate
        End Get
        Set(ByVal value As Date)
            Me.dEndDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property RiskTax() As Double
        Get
            Return Me.dRiskTax
        End Get
        Set(ByVal value As Double)
            Me.dRiskTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property RiskTypeDescription() As String
        Get
            Return Me.sRiskTypeDescription
        End Get
        Set(ByVal value As String)
            Me.sRiskTypeDescription = value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("RiskKey : " & iRiskKey & "<br />")
        sbPrint.AppendLine("RiskFolderKey: " & iRiskFolderKey & "<br />")
        sbPrint.AppendLine("TotalSumInsured : " & dTotalSumInsured.ToString & "<br />")
        sbPrint.AppendLine("IsRisk : " & bIsRisk.ToString & "<br />")
        sbPrint.AppendLine("RiskNumber : " & iRiskNumber & "<br />")
        sbPrint.AppendLine("Discounted : " & bDiscounted.ToString & "<br />")
        sbPrint.AppendLine("Variation : " & iVariation & "<br />")
        sbPrint.AppendLine("FeeTax : " & dFeeTax.ToString & "<br />")
        sbPrint.AppendLine("FeePremium : " & dFeePremium.ToString & "<br />")
        sbPrint.AppendLine("StartDate : " & dStartDate.ToString & "<br />")
        sbPrint.AppendLine("EndDate : " & dEndDate.ToString & "<br />")
        sbPrint.AppendLine("RiskTax : " & dRiskTax.ToString & "<br />")


        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class
<Serializable()> Public Class HeaderAndRiskTypeCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oHeaderAndRiskType As HeaderAndRiskType In List
            sbPrint.AppendLine(oHeaderAndRiskType.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oHeaderAndRiskType As HeaderAndRiskType) As Integer
        Return List.Add(v_oHeaderAndRiskType)
    End Function

    Public Sub Remove(ByVal v_oHeaderAndRiskType As HeaderAndRiskType)
        List.Remove(v_oHeaderAndRiskType)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As HeaderAndRiskType
        Get
            Return List(i)
        End Get
        Set(ByVal value As HeaderAndRiskType)
            List(i) = value
        End Set
    End Property

End Class