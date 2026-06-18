<Serializable()> Public Class ClaimResponse
#Region "Private Fields"
    Private iClaimKey As Integer

    Private iBaseClaimKey As Integer

    Private sClaimNumber As String

    Private iVersion As Integer

    Private bTimeStamp As Byte()
    Private ISPaymentAuthorized As Boolean

    Private sResultingStatus As String

    Private oWarnings As WarningCollection
#End Region

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    ''' 

    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    ''' 

    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Claim Key : " & iClaimKey & "<br />")
        sbPrint.AppendLine("Base Claim Key: " & iBaseClaimKey & "<br />")
        sbPrint.AppendLine("Claim Number: " & sClaimNumber & "<br />")
        sbPrint.AppendLine("Version: " & iVersion & "<br />")
        'sbPrint.AppendLine("Time Stamp: " & bTimeStamp.ToString() & "<br />")
        sbPrint.AppendLine("Resulting Status: " & sResultingStatus & "<br />")
        sbPrint.AppendLine("Warnings ---------------><br />")
        If oWarnings IsNot Nothing Then
            sbPrint.AppendLine(oWarnings.Print())
        End If
        Return sbPrint.ToString

    End Function
#Region "properties"
    '''<remarks/>
    Public Property ClaimKey() As Integer
        Get
            Return Me.iClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseClaimKey() As Integer
        Get
            Return Me.iBaseClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimNumber() As String
        Get
            Return Me.sClaimNumber
        End Get
        Set(ByVal value As String)
            Me.sClaimNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property Version() As Integer
        Get
            Return Me.iVersion
        End Get
        Set(ByVal value As Integer)
            Me.iVersion = value
        End Set
    End Property

    Public Property PaymentAuthorized() As Boolean
        Get
            Return Me.ISPaymentAuthorized
        End Get
        Set(ByVal value As Boolean)
            Me.ISPaymentAuthorized = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")> _
    Public Property TimeStamp() As Byte()
        Get
            Return Me.bTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bTimeStamp = value
        End Set
    End Property

    '''<remarks/>
    Public Property ResultingStatus() As String
        Get
            Return Me.sResultingStatus
        End Get
        Set(ByVal value As String)
            Me.sResultingStatus = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Warnings")> _
    Public Property Warnings() As WarningCollection
        Get
            Return Me.oWarnings
        End Get
        Set(ByVal value As WarningCollection)
            Me.oWarnings = value
        End Set
    End Property
#End Region
End Class
<Serializable()> Public Class PayClaimResponse : Inherits ClaimResponse
#Region "Privatefields"
    Private icreditedAccountKey As Integer

    Private icreditedDocumentKey As Integer

    Private icreditedTransdetailKey As Integer

    Private oCashList As CashList
#End Region
    Sub New()
        CashList = New CashList
    End Sub
#Region "Properties"
    '''<remarks/>
    Public Property creditedAccountKey() As Integer
        Get
            Return Me.icreditedAccountKey
        End Get
        Set(ByVal value As Integer)
            Me.icreditedAccountKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property creditedDocumentKey() As Integer
        Get
            Return Me.icreditedDocumentKey
        End Get
        Set(ByVal value As Integer)
            Me.icreditedDocumentKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property creditedTransdetailKey() As Integer
        Get
            Return Me.icreditedTransdetailKey
        End Get
        Set(ByVal value As Integer)
            Me.icreditedTransdetailKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property CashList() As CashList
        Get
            Return Me.oCashList
        End Get
        Set(ByVal value As CashList)
            Me.oCashList = value
        End Set
    End Property
#End Region
End Class

