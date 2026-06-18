Imports System.Web.HttpContext
<Serializable()> Public Class AddOn
    Private sPartyShortCode, sResolvedName As String
    Private dExtraAmount, dExtraPercentage As Decimal
    Private bSelected As Boolean

    Public Sub New()
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        'sbPrint.AppendLine("Key : " & iKey.ToString() & "<br />")
        'sbPrint.AppendLine("Description : " & sDescription & "<br />")
        'sbPrint.AppendLine("Screen Code : " & sScreenCode & "<br />")
        'sbPrint.AppendLine("Risk Code : " & sRiskCode & "<br />")
        'sbPrint.AppendLine("Data Model Code : " & sDataModelCode & "<br />")
        'If sXMLDataset IsNot Nothing Then
        '    sbPrint.AppendLine("XML Dataset : " & ProviderHelper.PrettyFormatXMLToHTML(sXMLDataset))
        'End If
        'sbPrint.AppendLine("RiskType Code : " & sRiskTypeCode & "<br />")
        'sbPrint.AppendLine("Status Code : " & sStatusCode & "<br />")
        'sbPrint.AppendLine("Total Sum Insured : " & dTotalSumInsured.ToString() & "<br />")
        'sbPrint.AppendLine("Commission Amount : " & dCommissionAmount.ToString() & "<br />")
        'sbPrint.AppendLine("Premium Due Gross : " & dPremiumDueGross & "<br />")
        'sbPrint.AppendLine("Premium Due Net : " & dPremiumDueNet.ToString() & "<br />")
        'sbPrint.AppendLine("Premium Due Tax : " & dPremiumDueTax.ToString() & "<br />")
        'sbPrint.AppendLine("Total Annual Tax : " & dTotalAnnualTax.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function
    Public Property Selected() As Boolean
        Get
            Return bSelected
        End Get
        Set(ByVal value As Boolean)
            bSelected = value
        End Set
    End Property
    Public Property PartyShortCode() As String
        Get
            Return sPartyShortCode
        End Get
        Set(ByVal value As String)
            sPartyShortCode = value
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

    Public Property ExtraAmount() As Decimal
        Get
            Return dExtraAmount
        End Get
        Set(ByVal value As Decimal)
            dExtraAmount = value
        End Set
    End Property

    Public Property ExtraPercentage() As Decimal
        Get
            Return dExtraPercentage
        End Get
        Set(ByVal value As Decimal)
            dExtraPercentage = value
        End Set
    End Property
End Class

<Serializable()> Public Class AddOnCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oRisk As Risk In List
            sbPrint.AppendLine(oRisk.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oAddOn As AddOn) As Integer
        Return List.Add(v_oAddOn)
    End Function

    Public Sub Remove(ByVal v_oAddOn As AddOn)
        List.Remove(v_oAddOn)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As AddOn
        Get
            Return List(i)
        End Get
        Set(ByVal value As AddOn)
            List(i) = value
        End Set
    End Property

    Public Function FindItemByPartyShortCode(ByVal v_oAddOn As String) As AddOn

        For Each oAddon As AddOn In List
            If oAddon.PartyShortCode = v_oAddOn Then
                Return oAddon
            End If
        Next

        Return Nothing

    End Function


End Class
