''' <summary>
''' Nexus ValidationDetails object
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class ValidationDetails

    Private sBankName As String
    Private bIsValid As Boolean
    Private sAddressLine1 As String
    Private sAddressLine2 As String
    Private sAddressLine3 As String
    Private sAddressLine4 As String
    Private sPostalCode As String
    Private sValidationMessageDataset As String
    Private bIsValidationOverridable As Boolean

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>html string of the contents of the object</returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        'Dim sbPrint As New Text.StringBuilder()

        'sbPrint.AppendLine("Key : " & iClaimKey & "<br />")
        'sbPrint.AppendLine("Address Line 1 : " & iBaseClaimKey & "<br />")
        'sbPrint.AppendLine("Address Line 2 : " & sAddress2 & "<br />")
        'sbPrint.AppendLine("Address Line 3 : " & sAddress3 & "<br />")
        'sbPrint.AppendLine("Address Line 4 : " & sAddress4 & "<br />")
        'sbPrint.AppendLine("Country Code : " & sCountryCode & "<br />")
        'sbPrint.AppendLine("Post Code : " & sPostCode & "<br />")

        'Return sbPrint.ToString

    End Function


    '''<remarks/>
    Public Property BankName() As String
        Get
            Return Me.sBankName
        End Get
        Set(ByVal value As String)
            Me.sBankName = value
        End Set
    End Property


    Public Property IsValid() As Boolean
        Get
            Return Me.bIsValid
        End Get
        Set(ByVal value As Boolean)
            Me.bIsValid = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 1
    ''' </summary>
    ''' <value>String representation of address line 1</value>
    ''' <returns>String reprensentation of address line 1</returns>
    ''' <remarks></remarks>
    Public Property AddressLine1() As String
        Get
            Return sAddressLine1
        End Get
        Set(ByVal value As String)
            sAddressLine1 = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 2
    ''' </summary>
    ''' <value>String representation of address line 2</value>
    ''' <returns>String reprensentation of address line 2</returns>
    ''' <remarks></remarks>
    Public Property AddressLine2() As String
        Get
            Return sAddressLine2
        End Get
        Set(ByVal value As String)
            sAddressLine2 = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 3
    ''' </summary>
    ''' <value>String representation of address line 3</value>
    ''' <returns>String reprensentation of address line 3</returns>
    ''' <remarks></remarks>
    Public Property AddressLine3() As String
        Get
            Return sAddressLine3
        End Get
        Set(ByVal value As String)
            sAddressLine3 = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 4
    ''' </summary>
    ''' <value>String representation of address line 4</value>
    ''' <returns>String reprensentation of address line 4</returns>
    ''' <remarks></remarks>
    Public Property AddressLine4() As String
        Get
            Return sAddressLine4
        End Get
        Set(ByVal value As String)
            sAddressLine4 = value
        End Set
    End Property

    ''' <summary>
    ''' PostCode
    ''' </summary>
    ''' <value>A string containing the postcode</value>
    ''' <returns>Returns a string containing the postcode, formatted correctly if the country code of the address is set to GBR</returns>
    ''' <remarks></remarks>
    Public Property PostalCode() As String
        Get
            Return sPostalCode
        End Get
        Set(ByVal value As String)
            sPostalCode = UCase(value)
        End Set
    End Property

    Public Property ValidationMessageDataset() As String
        Get
            Return sValidationMessageDataset
        End Get
        Set(ByVal value As String)
            sValidationMessageDataset = value
        End Set
    End Property

    Public Property IsValidationOverridable() As String
        Get
            Return bIsValidationOverridable
        End Get
        Set(ByVal value As String)
            bIsValidationOverridable = value
        End Set
    End Property

End Class

''' <summary>
''' Collection of Address objects
''' </summary>
<Serializable()> Public Class ValidationDetailsCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>An HTML string containining data held within the object</returns>
    Public Function Print() As String

        'Dim sbPrint As New Text.StringBuilder()

        'For Each oAddress As Address In List
        '    sbPrint.AppendLine(oAddress.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        'Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oValidationDetails As ValidationDetails) As Integer
        Return List.Add(v_oValidationDetails)
    End Function


    Public Sub Remove(ByVal v_oValidationDetails As ValidationDetails)
        List.Remove(v_oValidationDetails)
    End Sub

    ''' <summary>
    ''' Remove an ValidationDetails object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the ValidationDetails object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ValidationDetails
        Get
            Return List(i)
        End Get
        Set(ByVal value As ValidationDetails)
            List(i) = value
        End Set
    End Property


End Class
