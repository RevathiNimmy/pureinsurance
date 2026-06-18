Public Class BaseAddressType
    Public Property AddressKey As String
    Public Property AddressLine1 As String
    Public Property AddressLine2 As String
    Public Property AddressLine3 As String
    Public Property AddressLine4 As String
    Public Property AddressLine5 As String
    Public Property AddressLine6 As String
    Public Property AddressLine7 As String
    Public Property AddressLine8 As String
    Public Property AddressLine9 As String
    Public Property AddressLine10 As String
    Public Property AddressTypeCode As AddressTypeType = AddressTypeType.Correspondence
    Public Property CountryCode As String
    Public Property PostCode As String
    Public Property CountryId As Integer?
End Class
