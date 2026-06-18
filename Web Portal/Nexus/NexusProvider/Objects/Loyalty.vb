<Serializable()> Public Class Loyalty

    Private sKey As String
    Private iLoyaltySchemeKey As Integer
    Private sLoyaltySchemeCode As String
    Private sMembershipNumber As String
    Private sOtherReference As String
    Private dtStartDate As DateTime
    Private dtEndDate As DateTime
    Private sMainMember As String
    Private bActive As Boolean
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bActive = False

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("LoyaltySchemeKey : " & iLoyaltySchemeKey & "<br />")
        sbPrint.AppendLine("LoyaltySchemeCode : " & sLoyaltySchemeCode & "<br />")
        sbPrint.AppendLine("MembershipNumber : " & sMembershipNumber & "<br />")
        sbPrint.AppendLine("OtherReference : " & sOtherReference & "<br />")
        sbPrint.AppendLine("StartDate : " & dtStartDate & "<br />")
        sbPrint.AppendLine("EndDate : " & dtEndDate & "<br />")
        sbPrint.AppendLine("MainMember : " & sMainMember & "<br />")
        sbPrint.AppendLine("Active: " & IIf(bActive, "true", "false") & "<br />")
        Return sbPrint.ToString

    End Function

    Public Property Key() As String
        Get
            Return Me.sKey
        End Get
        Set(ByVal value As String)
            Me.sKey = value
        End Set
    End Property
    Public Property LoyaltySchemeKey() As Integer
        Get
            Return iLoyaltySchemeKey
        End Get
        Set(ByVal value As Integer)
            iLoyaltySchemeKey = value
        End Set
    End Property
    Public Property LoyaltySchemeCode() As String
        Get
            Return sLoyaltySchemeCode
        End Get
        Set(ByVal value As String)
            sLoyaltySchemeCode = value
        End Set
    End Property
    Public Property MembershipNumber() As String
        Get
            Return sMembershipNumber
        End Get
        Set(ByVal value As String)
            sMembershipNumber = value
        End Set
    End Property
    Public Property OtherReference() As String
        Get
            Return sOtherReference
        End Get
        Set(ByVal value As String)
            sOtherReference = value
        End Set
    End Property
    Public Property StartDate() As DateTime
        Get
            Return dtStartDate
        End Get
        Set(ByVal value As DateTime)
            dtStartDate = value
        End Set
    End Property
    Public Property EndDate() As DateTime
        Get
            Return dtEndDate
        End Get
        Set(ByVal value As DateTime)
            dtEndDate = value
        End Set
    End Property
    Public Property MainMember() As String
        Get
            Return sMainMember
        End Get
        Set(ByVal value As String)
            sMainMember = value
        End Set
    End Property
    Public Property Active() As Boolean
        Get
            Return bActive
        End Get
        Set(ByVal value As Boolean)
            bActive = value
        End Set
    End Property
End Class

<Serializable()> Public Class LoyaltyCollection : Inherits CollectionBase

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
    ''' Add an Loyalty object to the collection
    ''' </summary>
    ''' <param name="v_oLoyalty">The Loyalty object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oLoyalty As Loyalty) As Integer
        v_oLoyalty.Key = List.Add(v_oLoyalty)
        Return v_oLoyalty.Key
    End Function

    ''' <summary>
    ''' Remove an Loyalty object from the collection
    ''' </summary>
    ''' <param name="v_oLoyalty">The Loyalty object to be removed</param>
    Public Sub Remove(ByVal v_oLoyalty As Loyalty)
        List.Remove(v_oLoyalty)
    End Sub

    ''' <summary>
    ''' Remove an Loyalty object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Loyalty object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Loyalty object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Loyalty object</param>
    ''' <value>The replacement Loyalty object</value>
    ''' <returns>The Loyalty object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Loyalty
        Get
            Return List(i)
        End Get
        Set(ByVal value As Loyalty)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oLoyalty As Loyalty)
        List.Item(v_oLoyalty.Key) = v_oLoyalty
    End Sub

    Public Sub Update(ByVal v_oLoyalty As Loyalty, ByVal index As Integer)
        List.Item(index) = v_oLoyalty
    End Sub

    '''' <summary>
    '''' Return the first Loyalty object in the collection with the specified AddressType
    '''' </summary>
    '''' <param name="v_oLoyaltyType">The LoyaltyType of the Loyalty object to be returned</param>
    '''' <value>The LoyaltyType the Loyalty is to be retrieved by</value>
    '''' <returns>Matching Address object, if any</returns>
    'Default Public ReadOnly Property Item(ByVal v_oLoyaltyType As LoyaltyType) As LoyalitySchemes
    '    Get
    '        Return FindItemByLoyaltyType(v_oLoyaltyType)
    '    End Get
    'End Property

    '''' <summary>
    '''' Find the first Loyalty object in the collection with the specified LoyaltyType
    '''' </summary>
    '''' <param name="v_oLoyaltyType">The LoyaltyType of the Loyalty object to be returned</param>
    '''' <returns>The matching Loyalty object, if any</returns>
    'Public Function FindItemByLoyaltyType(ByVal v_oLoyaltyType As LoyaltyType) As Loyalty

    '    For Each oLoyalty As LoyalitySchemes In List
    '        If oLoyalty.LoyaltyType = v_oLoyaltyType Then
    '            Return oLoyalty
    '        End If
    '    Next

    '    Return Nothing

    'End Function

End Class
