<Serializable()> Public Class UserDetails

    Private iKey As Integer
    Private sResolvedName As String
    Private sPartyKey As Integer
    Private sPartyName As String
    Private sPartyType As String
    Private sEmailAddress As String
    Private dtLastLogin As DateTime
    Private dtPasswordChange As DateTime
    Private oListOfBranches As BranchCollection
    Private bConsolidatedAgentCommission As Boolean
    Private oUserdetailsGroup As UserGroupCollection
    Private oUserProductByBranch As UserProductByBranchCollection
    Private sPartyCode As String
    Private sUserId As String


    Public Sub New(ByVal v_iKey As Integer, ByVal v_sResolvedName As String)
        iKey = v_iKey
        sResolvedName = v_sResolvedName
        oListOfBranches = New BranchCollection
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Key : " & iKey.ToString() & "<br />")
        sbPrint.AppendLine("Resolved Name : " & sResolvedName & "<br />")
        sbPrint.AppendLine("Party Name : " & sPartyName & "<br />")
        sbPrint.AppendLine("Party Type : " & sPartyType & "<br />")
        sbPrint.AppendLine("Email Address : " & sEmailAddress & "<br />")
        sbPrint.AppendLine("Last Login : " & dtLastLogin & "<br />")
        sbPrint.AppendLine("Password Change : " & dtPasswordChange & "<br />")
        sbPrint.AppendLine("Branches ---------------><br />")

        If oListOfBranches IsNot Nothing Then
            sbPrint.AppendLine(oListOfBranches.Print())
            sbPrint.AppendLine("<br />")
        End If

        Return sbPrint.ToString()

    End Function

    Public Property Key() As Integer
        Get
            Return iKey
        End Get
        Set(ByVal value As Integer)
            iKey = value
        End Set
    End Property
    Public Property ConsolidatedAgentCommission() As Boolean
        Get
            Return bConsolidatedAgentCommission
        End Get
        Set(ByVal value As Boolean)
            bConsolidatedAgentCommission = value
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

    ''' <summary>
    ''' WPR08- to check the party attached with the logged in user
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PartyCode() As String
        Get
            Return sPartyCode
        End Get
        Set(ByVal value As String)
            sPartyCode = value
        End Set
    End Property
    Public Property PartyKey() As Integer
        Get
            Return sPartyKey
        End Get
        Set(ByVal value As Integer)
            sPartyKey = value
        End Set
    End Property

    Public Property PartyName() As String
        Get
            Return sPartyName
        End Get
        Set(ByVal value As String)
            sPartyName = value
        End Set
    End Property

    Public Property PartyType() As String
        Get
            Return sPartyType
        End Get
        Set(ByVal value As String)
            sPartyType = value
        End Set
    End Property

    Public Property EmailAddress() As String
        Get
            Return sEmailAddress
        End Get
        Set(ByVal value As String)
            sEmailAddress = value
        End Set
    End Property

    Public Property LastLogin() As DateTime
        Get
            Return dtLastLogin
        End Get
        Set(ByVal value As DateTime)
            dtLastLogin = value
        End Set
    End Property

    Public Property PasswordChange() As DateTime
        Get
            Return dtPasswordChange
        End Get
        Set(ByVal value As DateTime)
            dtPasswordChange = value
        End Set
    End Property

    Public Property ListOfBranches() As BranchCollection
        Get
            Return oListOfBranches
        End Get
        Set(ByVal value As BranchCollection)
            oListOfBranches = value
        End Set
    End Property
    Public Property AvailableUsergroups() As UserGroupCollection
        Get
            Return oUserdetailsGroup
        End Get
        Set(ByVal value As UserGroupCollection)
            oUserdetailsGroup = value
        End Set
    End Property
    Public Property AvailableUserProductsByBranch() As UserProductByBranchCollection
        Get
            Return oUserProductByBranch
        End Get
        Set(ByVal value As UserProductByBranchCollection)
            oUserProductByBranch = value
        End Set
    End Property

    ''' <summary>
    ''' Pure BO user name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PureUsername() As String

    ''' <summary>
    ''' User's password expiry date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PasswordExpiryDate() As Nullable(Of DateTime)

    Public Property IsTempPassword() As Boolean

    Public Property IsLocked() As Boolean

    Public Property IsAuthenticated() As Boolean
       
    Public Property IsWeakPassword() As Boolean

    Public Property UserId() As Integer


End Class

''' <summary>
''' "SortableCollectionBase" class internally inherits "CollectionBase" and gives additionaly "Sortable" feture in class.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class BranchCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(Branch)
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oBranch As Branch In List
            sbPrint.AppendLine(oBranch.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oBranch As Branch) As Integer
        Return List.Add(v_oBranch)
    End Function

    Public Sub Remove(ByVal v_oBranch As Branch)
        List.Remove(v_oBranch)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Branch
        Get
            Return List(i)
        End Get
        Set(ByVal value As Branch)
            List(i) = value
        End Set
    End Property

End Class

<Serializable()> Public Class Branch

    Private sCode As String
    Private sDescription As String
    Private iBranchKey As Integer
    Private sAgentCode As String
    Private sBusinessType As String
    Private iAgentKey As Integer
    Public Sub New()

    End Sub
    Public Sub New(ByVal v_sCode As String, ByVal v_sDescription As String, Optional ByVal v_iBranckkey As Integer = 0, Optional ByVal v_sAgentCode As String = "", Optional ByVal v_sBusinessType As String = "", Optional ByVal v_iAgentKey As Integer = 0)
        sCode = v_sCode
        sDescription = v_sDescription
        iBranchKey = v_iBranckkey
        sAgentCode = v_sAgentCode
        sBusinessType = v_sBusinessType
        iAgentKey = v_iAgentKey
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Code : " & sCode & "<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property Code() As String
        Get
            Return sCode
        End Get
        Set(ByVal value As String)
            sCode = value
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
    Public Property BranchKey() As Integer
        Get
            Return iBranchKey
        End Get
        Set(ByVal value As Integer)
            iBranchKey = value
        End Set
    End Property
    Public Property AgentCode() As String
        Get
            Return sAgentCode
        End Get
        Set(ByVal value As String)
            sAgentCode = value
        End Set
    End Property

    Public Property BusinessType() As String
        Get
            Return sBusinessType
        End Get
        Set(ByVal value As String)
            sBusinessType = value
        End Set
    End Property
    Public Property AgentKey() As Integer
        Get
            Return iAgentKey
        End Get
        Set(ByVal value As Integer)
            iAgentKey = value
        End Set
    End Property
End Class


''' <summary>
''' "SortableCollectionBase" class internally inherits "CollectionBase" and gives additionaly "Sortable" feture in class.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class PickListCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(PickList)
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oPickList As PickList In List
            sbPrint.AppendLine(oPickList.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oPickList As PickList) As Integer
        Return List.Add(v_oPickList)
    End Function

    Public Sub Remove(ByVal v_oPickList As PickList)
        List.Remove(v_oPickList)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PickList
        Get
            Return List(i)
        End Get
        Set(ByVal value As PickList)
            List(i) = value
        End Set
    End Property

End Class

<Serializable()> Public Class PickList

    Private sCode As String
    Private sDescription As String
    Private iKey As Integer
    Private dtEffectiveDate As DateTime
    Public Sub New()

    End Sub
    Public Sub New(ByVal v_sCode As String, ByVal v_sDescription As String, Optional ByVal v_ikey As Integer = 0, Optional ByVal v_dtDateTime As DateTime = Nothing)
        sCode = v_sCode
        sDescription = v_sDescription
        iKey = v_ikey
        dtEffectiveDate = Convert.ToDateTime(IIf(v_dtDateTime = Nothing, DateTime.Now(), v_dtDateTime))
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Code : " & sCode & "<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property Code() As String
        Get
            Return sCode
        End Get
        Set(ByVal value As String)
            sCode = value
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
    Public Property Key() As Integer
        Get
            Return iKey
        End Get
        Set(ByVal value As Integer)
            iKey = value
        End Set
    End Property
    Public Property EffectiveDate() As DateTime
        Get
            Return dtEffectiveDate
        End Get
        Set(ByVal value As DateTime)
            dtEffectiveDate = value
        End Set
    End Property
End Class

Public Class ValidateUserResponse

    Public Property HashedPassword As String

    Public Property PasswordHistory As List(Of String)

End Class

