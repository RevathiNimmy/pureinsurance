''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Versions

#Region "Private Variables"

    Private iClaimKey As Integer
    Private iVersion As Integer
    Private dtTransactionDate As DateTime
    Private sTransactionType As String
    Private sVersionDescription As String
    Private dTotalIncurred As Decimal
    Private dTotalPaid As Decimal
    Private dThisRevision As Decimal
    Private dThisPayment As Decimal
    Private dThisSalvageRecovery As Decimal
    Private dThisThirdPartyRecovery As Decimal
    Private dCurrentReserve As Decimal
    Private sPolicyCurrency As String
    Private sLossCurrency As String
    Private sUser As String
    Private sClaimDescription As String
    Private sInsuranceRef As String
    Private iInsuranceFileKey As Integer
    Private sClaimNumber As String
    Private iRiskKey As Integer
    Private sClientShortName As String
    Private dtLossFromDate As DateTime
    Private sInsuranceHolderShortName As String
    Private iInsuranceFolderKey As Integer
    Private iSPreviouslyLockedField As Boolean
#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Claim Key : " & iClaimKey.ToString() & "<br />")
        sbPrint.AppendLine("Version : " & iVersion.ToString() & "<br />")
        sbPrint.AppendLine("Transaction Date : " & dtTransactionDate.ToString & "<br />")
        sbPrint.AppendLine("Transaction Type : " & sTransactionType & "<br />")
        sbPrint.AppendLine("Version Description : " & sVersionDescription & "<br />")
        sbPrint.AppendLine("Total Incurred : " & dTotalIncurred.ToString() & "<br />")
        sbPrint.AppendLine("Total Paid : " & dTotalPaid.ToString() & "<br />")
        sbPrint.AppendLine("This Revision : " & dThisRevision.ToString() & "<br />")
        sbPrint.AppendLine("This Payment : " & dThisPayment.ToString() & "<br />")
        sbPrint.AppendLine("This Salvage Recovery : " & dThisSalvageRecovery.ToString() & "<br />")
        sbPrint.AppendLine("This Third Party Recovery : " & dThisThirdPartyRecovery.ToString() & "<br />")
        sbPrint.AppendLine("Current Reserve : " & dCurrentReserve.ToString() & "<br />")
        sbPrint.AppendLine("Policy Currency : " & sPolicyCurrency.ToString() & "<br />")
        sbPrint.AppendLine("Loss Currency : " & sLossCurrency.ToString() & "<br />")
        sbPrint.AppendLine("User : " & sUser.ToString() & "<br />")
        sbPrint.AppendLine("Claim Description : " & sClaimDescription.ToString() & "<br />")
        sbPrint.AppendLine("Insurance Ref : " & sInsuranceRef.ToString() & "<br />")
        sbPrint.AppendLine("Insurance File Key : " & iInsuranceFileKey.ToString() & "<br />")
        sbPrint.AppendLine("Claim Number : " & sClaimNumber.ToString() & "<br />")
        sbPrint.AppendLine("Risk Key : " & iRiskKey.ToString() & "<br />")
        sbPrint.AppendLine("Client Short Name : " & sClientShortName.ToString() & "<br />")
        sbPrint.AppendLine("Loss From Date : " & dtLossFromDate.ToString() & "<br />")
        sbPrint.AppendLine("Insurance Holder Short Name : " & sInsuranceHolderShortName.ToString() & "<br />")
        sbPrint.AppendLine("Insurance Folder Key : " & iInsuranceFolderKey.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

#Region "Public Property"

    Public Property ClaimKey() As Integer
        Get
            Return Me.iClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimKey = value
        End Set
    End Property

    Public Property Version() As Integer
        Get
            Return Me.iVersion
        End Get
        Set(ByVal value As Integer)
            Me.iVersion = value
        End Set
    End Property

    Public Property TransactionDate() As DateTime
        Get
            Return Me.dtTransactionDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtTransactionDate = value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            Return Me.sTransactionType
        End Get
        Set(ByVal value As String)
            Me.sTransactionType = value
        End Set
    End Property

    Public Property VersionDescription() As String
        Get
            Return Me.sVersionDescription
        End Get
        Set(ByVal value As String)
            Me.sVersionDescription = value
        End Set
    End Property

    Public Property TotalIncurred() As Decimal
        Get
            Return Me.dTotalIncurred
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalIncurred = value
        End Set
    End Property

    Public Property TotalPaid() As Decimal
        Get
            Return Me.dTotalPaid
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalPaid = value
        End Set
    End Property

    Public Property ThisRevision() As Decimal
        Get
            Return Me.dThisRevision
        End Get
        Set(ByVal value As Decimal)
            Me.dThisRevision = value
        End Set
    End Property

    Public Property ThisPayment() As Decimal
        Get
            Return Me.dThisPayment
        End Get
        Set(ByVal value As Decimal)
            Me.dThisPayment = value
        End Set
    End Property

    Public Property ThisSalvageRecovery() As Decimal
        Get
            Return Me.dThisSalvageRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dThisSalvageRecovery = value
        End Set
    End Property

    Public Property ThisThirdPartyRecovery() As Decimal
        Get
            Return Me.dThisThirdPartyRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dThisThirdPartyRecovery = value
        End Set
    End Property

    Public Property CurrentReserve() As Decimal
        Get
            Return Me.dCurrentReserve
        End Get
        Set(ByVal value As Decimal)
            Me.dCurrentReserve = value
        End Set
    End Property

    Public Property PolicyCurrency() As String
        Get
            Return Me.sPolicyCurrency
        End Get
        Set(ByVal value As String)
            Me.sPolicyCurrency = value
        End Set
    End Property

    Public Property LossCurrency() As String
        Get
            Return Me.sLossCurrency
        End Get
        Set(ByVal value As String)
            Me.sLossCurrency = value
        End Set
    End Property

    Public Property User() As String
        Get
            Return Me.sUser
        End Get
        Set(ByVal value As String)
            Me.sUser = value
        End Set
    End Property

    Public Property ClaimDescription() As String
        Get
            Return Me.sClaimDescription
        End Get
        Set(ByVal value As String)
            Me.sClaimDescription = value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return Me.sInsuranceRef
        End Get
        Set(ByVal value As String)
            Me.sInsuranceRef = value
        End Set
    End Property

    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKey = value
        End Set
    End Property

    Public Property ClaimNumber() As String
        Get
            Return Me.sClaimNumber
        End Get
        Set(ByVal value As String)
            Me.sClaimNumber = value
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

    Public Property ClientShortName() As String
        Get
            Return Me.sClientShortName
        End Get
        Set(ByVal value As String)
            Me.sClientShortName = value
        End Set
    End Property

    Public Property LossFromDate() As DateTime
        Get
            Return Me.dtLossFromDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtLossFromDate = value
        End Set
    End Property

    Public Property InsuranceHolderShortName() As String
        Get
            Return Me.sInsuranceHolderShortName
        End Get
        Set(ByVal value As String)
            Me.sInsuranceHolderShortName = value
        End Set
    End Property

    Public Property InsuranceFolderKey() As Integer
        Get
            Return Me.iInsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFolderKey = value
        End Set
    End Property

    Public Property IsPreviouslyLocked() As Boolean
        Get
            Return Me.iSPreviouslyLockedField
        End Get
        Set(ByVal value As Boolean)
            Me.iSPreviouslyLockedField = value
        End Set
    End Property
#End Region

End Class

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class VersionsCollections : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oVersions As Versions In List
            sbPrint.AppendLine(oVersions.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oVersions As Versions) As Integer
        Return List.Add(v_oVersions)
    End Function

    Public Sub Remove(ByVal v_oVersions As Versions)
        List.Remove(v_oVersions)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Versions
        Get
            Return List(i)
        End Get
        Set(ByVal value As Versions)
            List(i) = value
        End Set
    End Property

End Class