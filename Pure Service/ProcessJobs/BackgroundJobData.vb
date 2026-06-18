''' <summary>
''' Background job data (any type).
''' </summary>
Public NotInheritable Class BackgroundJobData

#Region "Public Fields"

    Public ID As Integer
    Public Xml As String
    Public Type As String
    Public ErrorDetails As String
    Public PartyCode As String
    Public InsuranceRef As String
    Public ClaimNumber As String
    Public JobUserID As Integer
    Public LoggedUserID As Integer
    Public LoggedUserName As String
    Public LoggedUserPassword As String
#End Region

#Region "Constructors"

    ''' <summary>
    ''' Instantiate this object based on the supplied input param data.
    ''' </summary>
    Friend Sub New(ByVal id As Integer, _
                   ByVal xml As String, _
                   ByVal partyCode As String, _
                   ByVal insuranceRef As String, _
                   ByVal claimNumber As String, _
                   ByVal nLoggedUserId As Integer, _
                   ByVal sLoggedUserName As String, _
                   ByVal sLoggedUserPassword As String, _
                   ByVal nUserID As Integer)

        Me.ID = id
        Me.Xml = xml
        Me.JobUserID = nUserID

        If partyCode Is Nothing Then
            Me.PartyCode = String.Empty
        Else
            Me.PartyCode = partyCode.Trim
        End If

        If insuranceRef Is Nothing Then
            Me.InsuranceRef = String.Empty
        Else
            Me.InsuranceRef = insuranceRef.Trim
        End If

        If claimNumber Is Nothing Then
            Me.ClaimNumber = String.Empty
        Else
            Me.ClaimNumber = claimNumber.Trim
        End If

        Me.LoggedUserID = nLoggedUserId

        If sLoggedUserName Is Nothing Then
            Me.LoggedUserName = String.Empty
        Else
            Me.LoggedUserName = sLoggedUserName.Trim
        End If

        If sLoggedUserPassword Is Nothing Then
            Me.LoggedUserPassword = String.Empty
        Else
            Me.LoggedUserPassword = sLoggedUserPassword.Trim
        End If


        If claimNumber Is Nothing Then
            Me.ClaimNumber = String.Empty
        Else
            Me.ClaimNumber = claimNumber.Trim
        End If
        If xml.Length > 0 Then
            Dim jobType As String = String.Empty = ""
            If GetBackgroundJobType(xml, jobType) Then
                Me.Type = jobType
            End If
        End If

    End Sub

#End Region

#Region "Private Functions"

    Private Function GetBackgroundJobType(ByVal jobXml As String, _
                                          ByRef jobType As String) As Boolean


        Dim settings As New XmlReaderSettings
        settings.ValidationType = ValidationType.None

        Dim jobDoc As New XPathDocument(XmlReader.Create(New StringReader(jobXml), settings))
        Dim jobNav As XPathNavigator = jobDoc.CreateNavigator()

        jobType = jobNav.SelectSingleNode("/BACKGROUND_JOB/JOB").GetAttribute("jobtype", String.Empty)
        Return True

    End Function

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Quick test to see if an error has been recorded.
    ''' </summary>
    Public ReadOnly Property ErrorOccurred() As Boolean
        Get
            Return Not String.IsNullOrEmpty(Me.ErrorDetails)
        End Get
    End Property

#End Region

End Class

