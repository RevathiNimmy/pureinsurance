''' <summary>
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Reinsurer

    Private sRICode As String
    Private sRIName As String
    Private sBranchCode As String
    Private sSubBranchCode As String
    Private sCurrencyCode As String
    Private sReinsuranceTypeCode As String
    Private bIsRetained As Boolean
    Private bIsBroker As Boolean
    Private sTaxNumber As String
    Private bIsDomiciledForTax As Boolean
    Private bIsDomiciledForTaxSpecified As Boolean
    Private bIsTaxExempt As Boolean
    Private bIsTaxExemptSpecified As Boolean
    Private dTaxPercentage As Decimal
    Private bTaxPercentageSpecified As Boolean
    Private sTaxGroupCode, sReinsurerCode, sReinsurerKey As String
    Private oAddresses As AddressCollection
    Private sAddress1, sAddress2, sPostCode As String
    Private dParticipationPercentage As Double
    Private dTotalParticipationPercentage As Double
    Private dCommissionPercentage As Double
    Private sAccountType As String
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
        oAddresses = New AddressCollection
        bIsDomiciledForTaxSpecified = False
        bIsTaxExemptSpecified = False
        bTaxPercentageSpecified = False
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("RI Code : " & sRICode & "<br />")
        sbPrint.AppendLine("RI Name : " & sRIName & "<br />")
        sbPrint.AppendLine("Branch Code : " & sBranchCode & "<br />")
        sbPrint.AppendLine("Sub Branch Code : " & sSubBranchCode & "<br />")
        sbPrint.AppendLine("Currency Code : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("Reinsurance Type Code : " & sReinsuranceTypeCode & "<br />")
        sbPrint.AppendLine("IsRetained : " & bIsRetained & "<br />")
        sbPrint.AppendLine("IsBroker : " & bIsBroker & "<br />")
        sbPrint.AppendLine("Tax Number : " & sTaxNumber & "<br />")
        sbPrint.AppendLine("Domiciled For Tax : " & IIf(bIsDomiciledForTax, "true", "false") & "<br />")
        sbPrint.AppendLine("Domiciled For Tax Specified : " & IIf(bIsDomiciledForTaxSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Tax Exempt : " & IIf(bIsTaxExempt, "true", "false") & "<br />")
        sbPrint.AppendLine("Tax Exempt Specified : " & IIf(bIsTaxExemptSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Tax Percentage : " & dTaxPercentage.ToString & "<br />")
        sbPrint.AppendLine("Tax Percentage Specified : " & IIf(bTaxPercentageSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("TaxGroupCode : " & sTaxGroupCode & "<br />")

        sbPrint.AppendLine("Addresses ---------------><br />")

        If oAddresses IsNot Nothing Then
            sbPrint.AppendLine(oAddresses.Print())
        End If

        Return sbPrint.ToString

    End Function
    Public Property PostCode() As String
        Get
            Return sPostCode
        End Get
        Set(ByVal value As String)
            sPostCode = value
        End Set
    End Property
    Public Property Address1() As String
        Get
            Return sAddress1
        End Get
        Set(ByVal value As String)
            sAddress1 = value
        End Set
    End Property
    Public Property Address2() As String
        Get
            Return sAddress2
        End Get
        Set(ByVal value As String)
            sAddress2 = value
        End Set
    End Property
    Public Property ReinsurerKey() As String
        Get
            Return sReinsurerKey
        End Get
        Set(ByVal value As String)
            sReinsurerKey = value
        End Set
    End Property
    Public Property ReinsurerCode() As String
        Get
            Return sReinsurerCode
        End Get
        Set(ByVal value As String)
            sReinsurerCode = value
        End Set
    End Property
    Public Property RIName() As String
        Get
            Return sRIName
        End Get
        Set(ByVal value As String)
            sRIName = value
        End Set
    End Property
    Public Property RICode() As String
        Get
            Return sRICode
        End Get
        Set(ByVal value As String)
            sRICode = value
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
    Public Property SubBranchCode() As String
        Get
            Return sSubBranchCode
        End Get
        Set(ByVal value As String)
            sSubBranchCode = value
        End Set
    End Property
    Public Property CurrencyCode() As String
        Get
            Return sCurrencyCode
        End Get
        Set(ByVal value As String)
            sCurrencyCode = value
        End Set
    End Property
    Public Property ReinsuranceTypeCode() As String
        Get
            Return sReinsuranceTypeCode
        End Get
        Set(ByVal value As String)
            sReinsuranceTypeCode = value
        End Set
    End Property
    Public Property IsRetained() As Boolean
        Get
            Return bIsRetained
        End Get
        Set(ByVal value As Boolean)
            bIsRetained = value
        End Set
    End Property
    Public Property IsBroker() As Boolean
        Get
            Return bIsBroker
        End Get
        Set(ByVal value As Boolean)
            bIsBroker = value
        End Set
    End Property

    Public Property TaxNumber() As String
        Get
            Return sTaxNumber
        End Get
        Set(ByVal value As String)
            sTaxNumber = value
        End Set
    End Property

    Public Property IsDomiciledForTax() As Boolean
        Get
            Return bIsDomiciledForTax
        End Get
        Set(ByVal value As Boolean)
            bIsDomiciledForTax = value
            bIsDomiciledForTaxSpecified = True
        End Set
    End Property

    Public ReadOnly Property IsDomiciledForTaxSpecified() As Boolean
        Get
            Return bIsDomiciledForTaxSpecified
        End Get
    End Property

    Public Property IsTaxExempt() As Boolean
        Get
            Return bIsTaxExempt
        End Get
        Set(ByVal value As Boolean)
            bIsTaxExempt = value
            bIsTaxExemptSpecified = True
        End Set
    End Property

    Public ReadOnly Property IsTaxExemptSpecified() As Boolean
        Get
            Return bIsTaxExemptSpecified
        End Get
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

    Public Property TaxGroupCode() As String
        Get
            Return sTaxGroupCode
        End Get
        Set(ByVal value As String)
            sTaxGroupCode = value
        End Set
    End Property

    Public Property AccountType() As String
        Get
            Return sAccountType
        End Get
        Set(ByVal value As String)
            sAccountType = value
        End Set
    End Property
    Public Property Addresses() As AddressCollection
        Get
            Return oAddresses
        End Get
        Set(ByVal value As AddressCollection)
            oAddresses = value
        End Set
    End Property

    Public Property ParticipationPercentage() As Double
        Get
            Return Me.dParticipationPercentage
        End Get
        Set(ByVal value As Double)
            Me.dParticipationPercentage = value
        End Set
    End Property

    Public Property CommissionPercentage() As Double
        Get
            Return Me.dCommissionPercentage
        End Get
        Set(ByVal value As Double)
            Me.dCommissionPercentage = value
        End Set
    End Property

    Public Property TotalParticipationPercentage() As Double
        Get
            Return Me.dTotalParticipationPercentage
        End Get
        Set(ByVal value As Double)
            Me.dTotalParticipationPercentage = value
        End Set
    End Property
    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by RICode
    ''' </summary>
    <Serializable()> Public Class SortByRICode : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Party by their RICode attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As Reinsurer

            If TypeOf x Is Reinsurer Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is Reinsurer Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.RICode) And String.IsNullOrEmpty(oRight.RICode) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.RICode) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.RICode) Then
                Return 1
            ElseIf oLeft.RICode < oRight.RICode Then
                Return -1
            ElseIf oLeft.RICode = oRight.RICode Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by RIName
    ''' </summary>
    <Serializable()> Public Class SortByRIName : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Party by their RIName attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As Reinsurer

            If TypeOf x Is Reinsurer Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is Reinsurer Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.RIName) And String.IsNullOrEmpty(oRight.RIName) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.RIName) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.RIName) Then
                Return 1
            ElseIf oLeft.RIName < oRight.RIName Then
                Return -1
            ElseIf oLeft.RIName = oRight.RIName Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by RIName
    ''' </summary>
    <Serializable()> Public Class SortByBranchCode : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Party by their BranchCode attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As Reinsurer

            If TypeOf x Is Reinsurer Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is Reinsurer Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.BranchCode) And String.IsNullOrEmpty(oRight.BranchCode) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.BranchCode) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.BranchCode) Then
                Return 1
            ElseIf oLeft.BranchCode < oRight.BranchCode Then
                Return -1
            ElseIf oLeft.BranchCode = oRight.BranchCode Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class
    
End Class


''' <summary>
''' Collection of Reinsurer objects
''' </summary>
<Serializable()> Public Class ReinsurerCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oParty As Reinsurer In List
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
    Public Function Add(ByVal v_oParty As Reinsurer) As Integer
        Return List.Add(v_oParty)
    End Function

    ''' <summary>
    ''' Remove an Reinsurer object from the collection
    ''' </summary>
    ''' <param name="v_oParty">The Reinsurer object to be removed</param>
    Public Sub Remove(ByVal v_oParty As Reinsurer)
        List.Remove(v_oParty)
    End Sub

    ''' <summary>
    ''' Sort the collection
    ''' </summary>
    ''' <param name="oItem">Reinsurer attribute to sort by</param>
    ''' <param name="oDirection">Sort order</param>
    Public Sub Sort(ByVal oItem As PartySort, ByVal oDirection As Direction)

        Select Case oItem
            Case ReinsurerSort.RICode
                InnerList.Sort(New Reinsurer.SortByRICode())
            Case ReinsurerSort.RIName
                InnerList.Sort(New Reinsurer.SortByRIName())
            Case ReinsurerSort.BranchCode
                InnerList.Sort(New Reinsurer.SortByBranchCode())
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
    Default Public Property Item(ByVal i As Integer) As Reinsurer
        Get
            Return List(i)
        End Get
        Set(ByVal value As Reinsurer)
            List(i) = value
        End Set
    End Property

End Class

''' <summary>
''' Attribute to sort Party Collection by
''' </summary>
''' <remarks></remarks>
Public Enum ReinsurerSort

    ''' <summary>
    ''' RICode
    ''' </summary>
    RICode = 0

    ''' <summary>
    ''' RIName
    ''' </summary>
    RIName = 1

    ''' <summary>
    ''' BranchCode
    ''' </summary>
    BranchCode = 2



End Enum

''' <summary>
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class CalculateRITax

    Private iRiskKey As Integer
    Private iPartyKey As Integer
    Private dPremium As Double
    Private iInsuranceFileKey As Integer

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
    
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("RiskKey : " & iRiskKey & "<br />")
        sbPrint.AppendLine("PartyKey : " & iPartyKey & "<br />")
        sbPrint.AppendLine("Premium : " & dPremium & "<br />")
        sbPrint.AppendLine("InsuranceFileKey : " & iInsuranceFileKey & "<br />")


        Return sbPrint.ToString

    End Function
    Public Property RiskKey() As Integer
        Get
            Return iRiskKey
        End Get
        Set(ByVal value As Integer)
            iRiskKey = value
        End Set
    End Property
    Public Property PartyKey() As Integer
        Get
            Return iPartyKey
        End Get
        Set(ByVal value As Integer)
            iPartyKey = value
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
    Public Property InsuranceFileKey() As Integer
        Get
            Return iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFileKey = value
        End Set
    End Property
    
End Class


