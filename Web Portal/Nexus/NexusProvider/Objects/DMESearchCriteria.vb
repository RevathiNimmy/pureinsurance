<Serializable()> Public Class DMESearchCriteria

    Private sFolderName As String
    Private iParentNum As Integer
    Private sPartyCode As String
    Private sPartyName As String
    Private sPolicyNumber As String
    Private sClaimNumber As String
    Private sRiskIndex As String
    Private sPostCode As String
    Private sDocumentDescription As String
    Private bIncludeFiles As Boolean
    Private iMaxRowsToFetch As Integer
    Public Sub New()

    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("FolderName : " & sFolderName & "<br />")
        sbPrint.AppendLine("ParentNum : " & iParentNum & "<br />")

        sbPrint.AppendLine("Party Code : " & sPartyCode & "<br />")
        sbPrint.AppendLine("Party Name : " & sPartyName & "<br />")
        sbPrint.AppendLine("Policy Number : " & sPolicyNumber & "<br />")
        sbPrint.AppendLine("Claim Number : " & sClaimNumber & "<br />")
        sbPrint.AppendLine("Risk Index : " & sRiskIndex & "<br />")
        sbPrint.AppendLine("Post Code : " & sPostCode & "<br />")
        sbPrint.AppendLine("Document Description : " & sDocumentDescription & "<br />")
        sbPrint.AppendLine("Include File : " & bIncludeFiles.ToString & "<br />")
        sbPrint.AppendLine("Max Rows ToFetch: " & iMaxRowsToFetch.ToString & "<br />")

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' FolderName
    ''' </summary>
    ''' <value>Set the FolderName</value>
    ''' <returns>Get the FolderName</returns>
    Public Property FolderName() As String
        Get
            Return sFolderName
        End Get
        Set(ByVal value As String)
            sFolderName = value
        End Set
    End Property

    ''' <summary>
    ''' ParentNum
    ''' </summary>
    ''' <value>Set the ParentNum</value>
    ''' <returns>Get the ParentNum</returns>
    Public Property ParentNum() As Integer
        Get
            Return iParentNum
        End Get
        Set(ByVal value As Integer)
            iParentNum = value
        End Set
    End Property

    ''' <summary>
    ''' PartyCode
    ''' </summary>
    ''' <value>Set the PartyCode</value>
    ''' <returns>Get the PartyCode</returns>
    Public Property PartyCode() As String
        Get
            Return sPartyCode
        End Get
        Set(ByVal value As String)
            sPartyCode = value
        End Set
    End Property

    ''' <summary>
    ''' PartyName
    ''' </summary>
    ''' <value>Set the PartyName</value>
    ''' <returns>Get the PartyName</returns>
    Public Property PartyName() As String
        Get
            Return sPartyName
        End Get
        Set(ByVal value As String)
            sPartyName = value
        End Set
    End Property

    ''' <summary>
    ''' PolicyNumber
    ''' </summary>
    ''' <value>Set the PolicyNumber</value>
    ''' <returns>Get the PolicyNumber</returns>
    Public Property PolicyNumber() As String
        Get
            Return sPolicyNumber
        End Get
        Set(ByVal value As String)
            sPolicyNumber = value
        End Set
    End Property

    ''' <summary>
    ''' ClaimNumber
    ''' </summary>
    ''' <value>Set the ClaimNumber</value>
    ''' <returns>Get the ClaimNumber</returns>
    Public Property ClaimNumber() As String
        Get
            Return sClaimNumber
        End Get
        Set(ByVal value As String)
            sClaimNumber = value
        End Set
    End Property

    ''' <summary>
    ''' RiskIndex
    ''' </summary>
    ''' <value>Set the RiskIndex</value>
    ''' <returns>Get the RiskIndex</returns>
    Public Property RiskIndex() As String
        Get
            Return sRiskIndex
        End Get
        Set(ByVal value As String)
            sRiskIndex = value
        End Set
    End Property

    ''' <summary>
    ''' PostCode
    ''' </summary>
    ''' <value>Set the PostCode</value>
    ''' <returns>Get the PostCode</returns>
    Public Property PostCode() As String
        Get
            Return sPostCode
        End Get
        Set(ByVal value As String)
            sPostCode = value
        End Set
    End Property

    ''' <summary>
    ''' DocumentDescription
    ''' </summary>
    ''' <value>Set the DocumentDescription</value>
    ''' <returns>Get the DocumentDescription</returns>
    Public Property DocumentDescription() As String
        Get
            Return sDocumentDescription
        End Get
        Set(ByVal value As String)
            sDocumentDescription = value
        End Set
    End Property

    ''' <summary>
    ''' IncludeFiles
    ''' </summary>
    ''' <value>Set the IncludeFiles</value>
    ''' <returns>Get the IncludeFiles</returns>
    Public Property IncludeFiles() As Boolean
        Get
            Return bIncludeFiles
        End Get
        Set(ByVal value As Boolean)
            bIncludeFiles = value
        End Set
    End Property

    ''' <summary>
    ''' MaxRowsToFetch
    ''' </summary>
    ''' <value>Set the MaxRowsToFetch</value>
    ''' <returns>Get the MaxRowsToFetch</returns>
    Public Property MaxRowsToFetch() As Integer
        Get
            Return iMaxRowsToFetch
        End Get
        Set(ByVal value As Integer)
            iMaxRowsToFetch = value
        End Set
    End Property

End Class

