''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class CoInsurers

#Region "Private Variables"

    Private iCoInsurerKey As Integer
    Private sArrangementRef As String
    Private dSharePerc As Double
    Private dCommissionPerc As Double
    Private sCoInsurer As String

    Private iRecoveryKey, iPartyKey As Integer
    Private sRecoveryType, sRecoveryTypeCode As String
    Private dSharePercent, dRecoveryToDate As Decimal
    Private dTotalThisRecovery As Decimal

    Private dThisRecovery As Decimal
    Private sPerilDescription As String
#End Region

    Public Sub New()
    End Sub
    Public Property SharePercent() As Decimal
        Get
            Return dSharePercent
        End Get
        Set(ByVal value As Decimal)
            dSharePercent = value
        End Set
    End Property
    Public Property RecoveryToDate() As Decimal
        Get
            Return dRecoveryToDate
        End Get
        Set(ByVal value As Decimal)
            dRecoveryToDate = value
        End Set
    End Property
    Public Property RecoveryType() As String
        Get
            Return sRecoveryType
        End Get
        Set(ByVal value As String)
            sRecoveryType = value
        End Set
    End Property
    Public Property RecoveryKey() As Integer
        Get
            Return iRecoveryKey
        End Get
        Set(ByVal value As Integer)
            iRecoveryKey = value
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

    Public Property RecoveryTypeCode() As String
        Get
            Return sRecoveryTypeCode
        End Get
        Set(ByVal value As String)
            sRecoveryTypeCode = value
        End Set
    End Property

    Public Property PerilDescription() As String
        Get
            Return Me.sPerilDescription
        End Get
        Set(ByVal value As String)
            Me.sPerilDescription = value
        End Set
    End Property

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("CoInsurer Key : " & iCoInsurerKey & "<br />")
        sbPrint.AppendLine("Arrangement Ref : " & sArrangementRef & "<br />")
        sbPrint.AppendLine("Share Perc" & dSharePerc.ToString() & "<br />")
        sbPrint.AppendLine("CommissionPerc : " & dCommissionPerc.ToString() & "<br />")
        
        Return sbPrint.ToString()

    End Function

#Region "Public Property"
    
    Public Property CoInsurerKey() As Integer
        Get
            Return iCoInsurerKey
        End Get
        Set(ByVal value As Integer)
            iCoInsurerKey = value
        End Set
    End Property
    Public Property CoInsurer() As String
        Get
            Return sCoInsurer
        End Get
        Set(ByVal value As String)
            sCoInsurer = value
        End Set
    End Property
    Public Property ArrangementRef() As String
        Get
            Return sArrangementRef
        End Get
        Set(ByVal value As String)
            sArrangementRef = value
        End Set
    End Property

    Public Property SharePerc() As Double
        Get
            Return dSharePerc
        End Get
        Set(ByVal value As Double)
            dSharePerc = value
        End Set
    End Property

    Public Property CommissionPerc() As Double
        Get
            Return dCommissionPerc
        End Get
        Set(ByVal value As Double)
            dCommissionPerc = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalThisRecovery() As Decimal
        Get
            Return Me.dTotalThisRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalThisRecovery = value
        End Set
    End Property

    '''<remarks/>
    Public Property ThisRecovery() As Decimal
        Get
            Return Me.dThisRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dThisRecovery = value
        End Set
    End Property
#End Region

End Class

<Serializable()> Public Class CoInsurersCollections : Inherits CollectionBase
    Public Sub New()

    End Sub
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCoInsurers As CoInsurers In List
            sbPrint.AppendLine(oCoInsurers.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oCoInsurers As CoInsurers) As Integer
        Return List.Add(v_oCoInsurers)
    End Function

    Public Sub Remove(ByVal v_oCoInsurers As CoInsurers)
        List.Remove(v_oCoInsurers)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CoInsurers
        Get
            Return List(i)
        End Get
        Set(ByVal value As CoInsurers)
            List(i) = value
        End Set
    End Property
    Public Sub Update(ByVal v_CoInsurers As CoInsurers)
        List.Item(v_CoInsurers.CoInsurerKey) = v_CoInsurers
    End Sub
    Public Sub Update(ByVal v_CoInsurers As CoInsurers, ByVal index As Integer)
        List.Item(index) = v_CoInsurers
    End Sub

End Class