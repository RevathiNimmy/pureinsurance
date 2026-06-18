''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class AgentDetailsForPolicy

#Region "Private Variables"

    Private sName As String
    Private sShortname As String
    Private sAddress1 As String
    Private sAddress2 As String
    Private sAddress3 As String
    Private sAddress4 As String
    Private sPostalCode As String
    Private sAreaCode As String
    Private sNumber As String
    Private iExtension As Integer
    Private iContactTypeKey As Integer
    Private sCode As String
    Private iCountryKey As Integer
    Private sDescription As String
    Private iAddressUsageTypeKey As Integer
    Private iAddressKey As Integer
    Private oContacts As ContactCollection
#End Region

    Public Sub New()
        oContacts = New ContactCollection
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'sbPrint.AppendLine("Name : " & sName.ToString() & "<br />")
        'sbPrint.AppendLine("Short Name : " & sShortname.ToString() & "<br />")
        'sbPrint.AppendLine("Address1 : " & sAddress1.ToString() & "<br />")
        'sbPrint.AppendLine("Address2 : " & sAddress2.ToString() & "<br />")
        'sbPrint.AppendLine("Address3 : " & sAddress3.ToString() & "<br />")
        'sbPrint.AppendLine("Address4 : " & sAddress4.ToString() & "<br />")
        'sbPrint.AppendLine("Postal Code : " & sPostalCode.ToString() & "<br />")
        'sbPrint.AppendLine("Number : " & sNumber.ToString() & "<br />")
        'sbPrint.AppendLine("Extension : " & iExtension.ToString() & "<br />")
        'sbPrint.AppendLine("Contact Type Key : " & iContactTypeKey.ToString() & "<br />")
        'sbPrint.AppendLine("Code : " & sCode.ToString() & "<br />")
        'sbPrint.AppendLine("Country Key : " & iCountryKey.ToString() & "<br />")
        'sbPrint.AppendLine("Description : " & sDescription.ToString() & "<br />")
        'sbPrint.AppendLine("Address Usage Type Key : " & iAddressUsageTypeKey.ToString() & "<br />")
        'sbPrint.AppendLine("AddressKey : " & iAddressKey.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

#Region "Public Properties"
    Public Property Contacts() As ContactCollection
        Get
            Return oContacts
        End Get
        Set(ByVal value As ContactCollection)
            oContacts = value
        End Set
    End Property
    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property

    Public Property Shortname() As String
        Get
            Return Me.sShortname
        End Get
        Set(ByVal value As String)
            Me.sShortname = value
        End Set
    End Property

    Public Property Address1() As String
        Get
            Return Me.sAddress1
        End Get
        Set(ByVal value As String)
            Me.sAddress1 = value
        End Set
    End Property

    Public Property Address2() As String
        Get
            Return Me.sAddress2
        End Get
        Set(ByVal value As String)
            Me.sAddress2 = value
        End Set
    End Property

    Public Property Address3() As String
        Get
            Return Me.sAddress3
        End Get
        Set(ByVal value As String)
            Me.sAddress3 = value
        End Set
    End Property

    Public Property Address4() As String
        Get
            Return Me.sAddress4
        End Get
        Set(ByVal value As String)
            Me.sAddress4 = value
        End Set
    End Property

    Public Property PostalCode() As String
        Get
            Return Me.sPostalCode
        End Get
        Set(ByVal value As String)
            Me.sPostalCode = value
        End Set
    End Property

    Public Property AreaCode() As String
        Get
            Return Me.sAreaCode
        End Get
        Set(ByVal value As String)
            Me.sAreaCode = value
        End Set
    End Property

    Public Property Number() As String
        Get
            Return Me.sNumber
        End Get
        Set(ByVal value As String)
            Me.sNumber = value
        End Set
    End Property

    Public Property Extension() As Integer
        Get
            Return Me.iExtension
        End Get
        Set(ByVal value As Integer)
            Me.iExtension = value
        End Set
    End Property

    Public Property ContactTypeKey() As Integer
        Get
            Return Me.iContactTypeKey
        End Get
        Set(ByVal value As Integer)
            Me.iContactTypeKey = value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return Me.sCode
        End Get
        Set(ByVal value As String)
            Me.sCode = value
        End Set
    End Property

    Public Property CountryKey() As Integer
        Get
            Return Me.iCountryKey
        End Get
        Set(ByVal value As Integer)
            Me.iCountryKey = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    Public Property AddressUsageTypeKey() As Integer
        Get
            Return Me.iAddressUsageTypeKey
        End Get
        Set(ByVal value As Integer)
            Me.iAddressUsageTypeKey = value
        End Set
    End Property

    Public Property AddressKey() As Integer
        Get
            Return Me.iAddressKey
        End Get
        Set(ByVal value As Integer)
            Me.iAddressKey = value
        End Set
    End Property

#End Region



End Class
