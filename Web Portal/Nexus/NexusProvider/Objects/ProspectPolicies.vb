<Serializable()> Public Class ProspectPolicies

    Private sKey As String
    Private iProspectPolicyKey As Integer
    Private sProspectTypeCode As String
    Private dtRenewalDate As DateTime
    Private dTimesQuoted As Decimal
    Private dTargetPremium As Decimal
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("ProspectPolicyKey : " & iProspectPolicyKey & "<br />")
        sbPrint.AppendLine("ProspectTypeCode : " & sProspectTypeCode & "<br />")
        sbPrint.AppendLine("RenewalDate : " & dtRenewalDate & "<br />")
        sbPrint.AppendLine("TimesQuoted : " & dTimesQuoted & "<br />")
        sbPrint.AppendLine("TargetPremium : " & dTargetPremium & "<br />")
        Return sbPrint.ToString

    End Function

    Public Property Key() As String
        Get
            Return sKey
        End Get
        Set(ByVal value As String)
            sKey = value
        End Set
    End Property
    Public Property ProspectPolicyKey() As Integer
        Get
            Return iProspectPolicyKey
        End Get
        Set(ByVal value As Integer)
            iProspectPolicyKey = value
        End Set
    End Property
    Public Property ProspectTypeCode() As String
        Get
            Return sProspectTypeCode
        End Get
        Set(ByVal value As String)
            sProspectTypeCode = value
        End Set
    End Property
    Public Property RenewalDate() As DateTime
        Get
            Return dtRenewalDate
        End Get
        Set(ByVal value As DateTime)
            dtRenewalDate = value
        End Set
    End Property
    Public Property TimesQuoted() As Decimal
        Get
            Return dTimesQuoted
        End Get
        Set(ByVal value As Decimal)
            dTimesQuoted = value
        End Set
    End Property
    Public Property TargetPremium() As Decimal
        Get
            Return dTargetPremium
        End Get
        Set(ByVal value As Decimal)
            dTargetPremium = value
        End Set
    End Property
End Class

<Serializable()> Public Class ProspectPolicyCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>An HTML string containining data held within the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAddress As Address In List
            sbPrint.AppendLine(oAddress.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add an ProspectPolicy object to the collection
    ''' </summary>
    ''' <param name="v_oProspectPolicy">The ProspectPolicy object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oProspectPolicy As ProspectPolicies) As Integer
        v_oProspectPolicy.Key = List.Add(v_oProspectPolicy)
        Return v_oProspectPolicy.Key
    End Function

    ''' <summary>
    ''' Remove an ProspectPolicy object from the collection
    ''' </summary>
    ''' <param name="v_oProspectPolicy">The ProspectPolicy object to be removed</param>
    Public Sub Remove(ByVal v_oProspectPolicy As ProspectPolicies)
        List.Remove(v_oProspectPolicy)
    End Sub

    ''' <summary>
    ''' Remove an ProspectPolicy object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the ProspectPolicy object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ProspectPolicy object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the ProspectPolicy object</param>
    ''' <value>The replacement ProspectPolicy object</value>
    ''' <returns>The ProspectPolicy object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As ProspectPolicies
        Get
            Return List(i)
        End Get
        Set(ByVal value As ProspectPolicies)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oProspectPolicy As ProspectPolicies)
        List.Item(v_oProspectPolicy.Key) = v_oProspectPolicy
    End Sub

    Public Sub Update(ByVal v_oProspectPolicy As ProspectPolicies, ByVal index As Integer)
        List.Item(index) = v_oProspectPolicy
    End Sub

    '''' <summary>
    '''' Return the first ProspectPolicy object in the collection with the specified ProspectPolicyType
    '''' </summary>
    '''' <param name="v_oProspectPolicyType">The ProspectPolicyType of the ProspectPolicy object to be returned</param>
    '''' <value>The ProspectPolicyType the ProspectPolicy is to be retrieved by</value>
    '''' <returns>Matching ProspectPolicy object, if any</returns>
    'Default Public ReadOnly Property Item(ByVal v_oProspectPolicyType As ProspectPolicyType) As ProspectPolicies
    '    Get
    '        Return FindItemByProspectPolicyType(v_oProspectPolicyType)
    '    End Get
    'End Property

    '''' <summary>
    '''' Find the first ProspectPolicy object in the collection with the specified ProspectPolicyType
    '''' </summary>
    '''' <param name="v_oProspectPolicyType">The ProspectPolicyType of the ProspectPolicy object to be returned</param>
    '''' <returns>The matching ProspectPolicy object, if any</returns>
    'Public Function FindItemByProspectPolicyType(ByVal v_oProspectPolicyType As ProspectPolicyType) As ProspectPolicies

    '    For Each oProspectPolicy As ProspectPolicy In List
    '        If oProspectPolicy.ProspectPolicyType = v_oProspectPolicyType Then
    '            Return oProspectPolicy
    '        End If
    '    Next

    '    Return Nothing

    'End Function

End Class
