<Serializable()> Public Class ClaimRiskLink
    Private iInsuranceFileKey As Integer
    Private iRiskKey As Integer
    Private sCode As String
    Private sDescription As String
    Private dSumInsured As Decimal

    Private oReserveItemType As ReserveTypeCollection
    Private oRecoveryItemType As RecoveryTypeCollection

    Public Sub New()
        oReserveItemType = New ReserveTypeCollection
        oRecoveryItemType = New RecoveryTypeCollection
    End Sub

    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKey = value
        End Set
    End Property

    Public Property RiskKey() As Integer
        Get
            Return Me.iRiskKey
        End Get
        Set(ByVal value As Integer)
            Me.iRiskKey = value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return Me.sCode
        End Get
        Set(ByVal value As String)
            Me.sCode = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    Public Property SumInsured() As Decimal
        Get
            Return Me.dSumInsured
        End Get
        Set(ByVal value As Decimal)
            Me.dSumInsured = value
        End Set
    End Property

    Public Property ReserveItemType() As ReserveTypeCollection
        Get
            Return Me.oReserveItemType
        End Get
        Set(ByVal value As ReserveTypeCollection)
            Me.oReserveItemType = value
        End Set
    End Property

    Public Property RecoveryItemType() As RecoveryTypeCollection
        Get
            Return Me.oRecoveryItemType
        End Get
        Set(ByVal value As RecoveryTypeCollection)
            Me.oRecoveryItemType = value
        End Set
    End Property

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("InsuranceFileKey : " & iInsuranceFileKey.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Riskkey : " & iRiskKey & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Code : " & sCode & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Sum Insured : " & dSumInsured & "<br />")
        sbPrint.AppendLine("<br />")
        'sbPrint.AppendLine("ReserveType : " & oReserveItemType & "<br />")
        'sbPrint.AppendLine("<br />")
        'sbPrint.AppendLine("Recovery Type : " & oRecoveryItemType & "<br />")
        'sbPrint.AppendLine("<br />")
        Return sbPrint.ToString()

    End Function

End Class

<Serializable()> Public Class ClaimRiskLinkCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oClaimRisk As ClaimRiskLink In List
        '    sbPrint.AppendLine(oClaimRiskLink.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a ClaimRiskLink object to the collection
    ''' </summary>
    ''' <param name="v_oClaimRiskLink">The ClaimRiskLink object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oClaimRiskLink As ClaimRiskLink) As Integer
        Return List.Add(v_oClaimRiskLink)
    End Function


    Public Sub Remove(ByVal v_oClaimRiskLink As ClaimRiskLink)
        List.Remove(v_oClaimRiskLink)
    End Sub

    ''' <summary>
    ''' Remove an ClaimRiskLink object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the ClaimRiskLink object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ClaimRiskLink object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the ClaimRiskLink object</param>
    ''' <value>The replacement ClaimRiskLink object</value>
    ''' <returns>The ClaimRiskLink object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As ClaimRiskLink
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimRiskLink)
            List(i) = value
        End Set
    End Property

End Class
