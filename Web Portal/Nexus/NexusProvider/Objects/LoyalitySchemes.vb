<Serializable()> Public Class LoyalitySchemes

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
