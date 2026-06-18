<Serializable()> Public Class ClaimSearchCriteria
    Private sClaimNumber As String
    Private iInsuranceFileRef As String
    Private sClientShortName As String
    Private dtLossDateFrom As Date
    Private dtLossDateTo As Date
    Private sRiskIndex As String
    Private bIncludeClosedClaim As Boolean
    Private iRiskKey As Integer
    Private iMaxRowsToFetch As Integer
    Private sTPACode As String
    Private sCaseNumber As String
    Private sDescription As String

    Public Sub New()

    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Claim Number : " & sClaimNumber & "<br />")
        sbPrint.AppendLine("Insurance File Ref : " & iInsuranceFileRef & "<br />")
        sbPrint.AppendLine("Client Short Name : " & sClientShortName & "<br />")
        sbPrint.AppendLine("Loss Date From : " & dtLossDateFrom.ToString & "<br />")
        sbPrint.AppendLine("Loss Date To : " & dtLossDateTo.ToString & "<br />")
        sbPrint.AppendLine("Risk Index : " & sRiskIndex & "<br />")
        sbPrint.AppendLine("Include Closed Claim : " & bIncludeClosedClaim & "<br />")
        sbPrint.AppendLine("Risk Key : " & iRiskKey.ToString & "<br />")
        sbPrint.AppendLine("Max Rows ToFetch: " & iMaxRowsToFetch.ToString & "<br />")
        sbPrint.AppendLine("Case Number: " & sCaseNumber & "<br />")

        Return sbPrint.ToString()

    End Function

    '''<remarks/>
    Public Property ClaimNumber() As String
        Get
            Return sClaimNumber
        End Get
        Set(ByVal value As String)
            sClaimNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuranceFileRef() As String
        Get
            Return iInsuranceFileRef
        End Get
        Set(ByVal value As String)
            iInsuranceFileRef = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientShortName() As String
        Get
            Return sClientShortName
        End Get
        Set(ByVal value As String)
            sClientShortName = value
        End Set
    End Property

    '''<remarks/>
    Public Property LossDateFrom() As Date
        Get
            Return dtLossDateFrom
        End Get
        Set(ByVal value As Date)
            dtLossDateFrom = value
        End Set
    End Property

    '''<remarks/>
    Public Property LossDateTo() As Date
        Get
            Return dtLossDateTo
        End Get
        Set(ByVal value As Date)
            dtLossDateTo = value
        End Set
    End Property

    '''<remarks/>
    Public Property RiskIndex() As String
        Get
            Return sRiskIndex
        End Get
        Set(ByVal value As String)
            sRiskIndex = value
        End Set
    End Property

    '''<remarks/>
    Public Property IncludeClosedClaim() As Boolean
        Get
            Return bIncludeClosedClaim
        End Get
        Set(ByVal value As Boolean)
            bIncludeClosedClaim = value
        End Set
    End Property

    '''<remarks/>
    Public Property RiskKey() As Integer
        Get
            Return iRiskKey
        End Get
        Set(ByVal value As Integer)
            iRiskKey = value
        End Set
    End Property


    Public Property MaxRowsToFetch() As Integer
        Get
            Return iMaxRowsToFetch
        End Get
        Set(ByVal value As Integer)
            iMaxRowsToFetch = value
        End Set
    End Property

    ''' <summary>
    ''' WPR08- To enable search based on TPA
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TPACode() As String
        Get
            Return sTPACode
        End Get
        Set(ByVal value As String)
            sTPACode = value
        End Set
    End Property

    ''' <summary>
    ''' Will be used in searching and displaying in grid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaseNumber() As String
        Get
            Return sCaseNumber
        End Get
        Set(ByVal value As String)
            sCaseNumber = value
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

End Class
