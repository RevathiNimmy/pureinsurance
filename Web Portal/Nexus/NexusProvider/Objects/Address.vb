''' <summary>
''' Nexus address object
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Address

    Private sKey As String
    Private oAddressType As AddressType = NexusProvider.AddressType.CorrespondenceAddress
    Private sAddress1 As String
    Private sAddress2 As String
    Private sAddress3 As String
    Private sAddress4 As String
    Private sAddress5 As String
    Private sAddress6 As String
    Private sAddress7 As String
    Private sAddress8 As String
    Private sAddress9 As String
    Private sAddress10 As String
    Private sPostCode As String
    Private sCountryCode As String
    Private sCountryDesc As String
    Private sStateCode As String
    Private iCountryKey As Integer
    Private sStateKey As String
    Private sStateDescription As String
    Private sMonikar As String
    Private sQASGNAFPID As String

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Constructor with the minimum requirements for an address in SAM
    ''' </summary>
    ''' <param name="v_sAddress1">First line of the address</param>
    ''' <param name="v_sPostCode">PostCode</param>
    ''' <param name="v_sCountryCode">Country Code, codes are from BackOffice</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal v_sAddress1 As String, ByVal v_sPostCode As String, Optional ByVal v_sCountryCode As String = Nothing)
        sAddress1 = v_sAddress1
        sPostCode = v_sPostCode
        sCountryCode = v_sCountryCode
    End Sub

    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>html string of the contents of the object</returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Key : " & sKey & "<br />")
        sbPrint.AppendLine("Address Type : " & [Enum].GetName(GetType(AddressType), oAddressType) & "<br />")
        sbPrint.AppendLine("Address Line 1 : " & sAddress1 & "<br />")
        sbPrint.AppendLine("Address Line 2 : " & sAddress2 & "<br />")
        sbPrint.AppendLine("Address Line 3 : " & sAddress3 & "<br />")
        sbPrint.AppendLine("Address Line 4 : " & sAddress4 & "<br />")
        sbPrint.AppendLine("Country Code : " & sCountryCode & "<br />")
        sbPrint.AppendLine("Post Code : " & sPostCode & "<br />")
        sbPrint.AppendLine("Country Code : " & sCountryCode & "<br />")
        sbPrint.AppendLine("Monikar : " & sMonikar & "<br />")
        sbPrint.AppendLine("Monikar : " & sQASGNAFPID & "<br />")
        sbPrint.AppendLine("Address Line 5 : " & sAddress5 & "<br />")
        sbPrint.AppendLine("Address Line 6 : " & sAddress6 & "<br />")
        sbPrint.AppendLine("Address Line 7 : " & sAddress7 & "<br />")
        sbPrint.AppendLine("Address Line 8 : " & sAddress8 & "<br />")
        sbPrint.AppendLine("Address Line 9 : " & sAddress9 & "<br />")
        sbPrint.AppendLine("Address Line 10 : " & sAddress10 & "<br />")

        Return sbPrint.ToString

    End Function
    Public Property CountryKey() As Integer
        Get
            Return iCountryKey
        End Get
        Set(ByVal value As Integer)
            iCountryKey = value
        End Set
    End Property
    ''' <summary>
    ''' Identifying key of the address
    ''' </summary>
    ''' <value>Identifying key of the address</value>
    ''' <returns>An integer representing the identifying key of the address</returns>
    ''' <remarks></remarks>
    Public Property Key() As String
        Get
            Return sKey
        End Get
        Set(ByVal value As String)
            sKey = value
        End Set
    End Property


    ''' <summary>
    ''' Address type
    ''' </summary>
    ''' <value>Type of address contained within the object</value>
    ''' <returns>returns the type of address of the object</returns>
    ''' <remarks></remarks>
    Public Property AddressType() As AddressType
        Get
            Return oAddressType
        End Get
        Set(ByVal value As AddressType)
            oAddressType = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 1
    ''' </summary>
    ''' <value>String representation of address line 1</value>
    ''' <returns>String reprensentation of address line 1</returns>
    ''' <remarks></remarks>
    Public Property Address1() As String
        Get
            Return sAddress1
        End Get
        Set(ByVal value As String)
            sAddress1 = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 2
    ''' </summary>
    ''' <value>String representation of address line 2</value>
    ''' <returns>String reprensentation of address line 2</returns>
    ''' <remarks></remarks>
    Public Property Address2() As String
        Get
            Return sAddress2
        End Get
        Set(ByVal value As String)
            sAddress2 = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 3
    ''' </summary>
    ''' <value>String representation of address line 3</value>
    ''' <returns>String reprensentation of address line 3</returns>
    ''' <remarks></remarks>
    Public Property Address3() As String
        Get
            Return sAddress3
        End Get
        Set(ByVal value As String)
            sAddress3 = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 4
    ''' </summary>
    ''' <value>String representation of address line 4</value>
    ''' <returns>String reprensentation of address line 4</returns>
    ''' <remarks></remarks>
    Public Property Address4() As String
        Get
            Return sAddress4
        End Get
        Set(ByVal value As String)
            sAddress4 = value
        End Set
    End Property

    ''' <summary>
    ''' PostCode
    ''' </summary>
    ''' <value>A string containing the postcode</value>
    ''' <returns>Returns a string containing the postcode, formatted correctly if the country code of the address is set to GBR</returns>
    ''' <remarks></remarks>
    Public Property PostCode() As String
        Get
            If sCountryCode IsNot Nothing Then
                If sCountryCode.Equals("GBR") Then

                    'reformat UK only postcodes with seperator, we can't do
                    'this on set as the country code may not have been set
                    If sPostCode IsNot Nothing Then
                        If sPostCode.Length > 4 Then
                            Dim iLen As Int16 = sPostCode.Length
                            'sPostCode = Left(sPostCode, iLen - 3) & " " & Right(sPostCode, iLen - (iLen - 3))
                        End If
                    End If

                End If
            End If

            Return sPostCode

        End Get
        Set(ByVal value As String)
            sPostCode = UCase(value)
        End Set
    End Property

    ''' <summary>
    ''' Country Code
    ''' </summary>
    ''' <value>A string containing the Country Code</value>
    ''' <returns>A string containing the Country Code</returns>
    Public Property CountryCode() As String
        Get
            Return sCountryCode
        End Get
        Set(ByVal value As String)
            sCountryCode = value
        End Set
    End Property
    ''' <summary>
    ''' Country Description
    ''' </summary>
    ''' <value>A string containing the Country Code</value>
    ''' <returns>A string containing the Country Code</returns>
    Public Property CountryDescription() As String
        Get
            Return sCountryDesc
        End Get
        Set(ByVal value As String)
            sCountryDesc = value
        End Set
    End Property

    ''' <summary>
    ''' State Code of Address
    ''' </summary>
    ''' <value>A string containing the State Code</value>
    ''' <returns>A string containing the State Code</returns>
    Public Property StateCode() As String
        Get
            Return sStateCode
        End Get
        Set(ByVal value As String)
            sStateCode = value
        End Set
    End Property

    ''' <summary>
    ''' State Key of Address
    ''' </summary>
    ''' <value>A string containing the State Key</value>
    ''' <returns>A string containing the State Key</returns>
    Public Property StateKey() As String
        Get
            Return sStateKey
        End Get
        Set(ByVal value As String)
            sStateKey = value
        End Set
    End Property

    ''' <summary>
    ''' State Description/Name of Address
    ''' </summary>
    ''' <value>A string containing the State Name</value>
    ''' <returns>A string containing the State Name</returns>
    Public Property StateDescription() As String
        Get
            Return sStateDescription
        End Get
        Set(ByVal value As String)
            sStateDescription = value
        End Set
    End Property

    ''' <summary>
    ''' Identifying key for QAS address
    ''' </summary>
    ''' <value>Identifying key for QAS address</value>
    ''' <returns>A string representing the identifying key of the QAS address</returns>
    ''' <remarks></remarks>
    Public Property Monikar() As String
        Get
            Return sMonikar
        End Get
        Set(ByVal value As String)
            sMonikar = value
        End Set
    End Property

    ''' <summary>
    ''' QAS Location key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property QASGNAFPID() As String
        Get
            Return sQASGNAFPID
        End Get
        Set(ByVal value As String)
            sQASGNAFPID = value
        End Set
    End Property
    ''' <summary>
    ''' Address line 5
    ''' </summary>
    ''' <value>String representation of address line 5</value>
    ''' <returns>String reprensentation of address line 5</returns>
    ''' <remarks></remarks>
    Public Property Address5() As String
        Get
            Return sAddress5
        End Get
        Set(ByVal value As String)
            sAddress5 = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 6
    ''' </summary>
    ''' <value>String representation of address line 6</value>
    ''' <returns>String reprensentation of address line 6</returns>
    ''' <remarks></remarks>
    Public Property Address6() As String
        Get
            Return sAddress6
        End Get
        Set(ByVal value As String)
            sAddress6 = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 7
    ''' </summary>
    ''' <value>String representation of address line 7</value>
    ''' <returns>String reprensentation of address line 7</returns>
    ''' <remarks></remarks>
    Public Property Address7() As String
        Get
            Return sAddress7
        End Get
        Set(ByVal value As String)
            sAddress7 = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 8
    ''' </summary>
    ''' <value>String representation of address line 8</value>
    ''' <returns>String reprensentation of address line 8</returns>
    ''' <remarks></remarks>
    Public Property Address8() As String
        Get
            Return sAddress8
        End Get
        Set(ByVal value As String)
            sAddress8 = value
        End Set
    End Property


    ''' <summary>
    ''' Address line 9
    ''' </summary>
    ''' <value>String representation of address line 9</value>
    ''' <returns>String reprensentation of address line 9</returns>
    ''' <remarks></remarks>
    Public Property Address9() As String
        Get
            Return sAddress9
        End Get
        Set(ByVal value As String)
            sAddress9 = value
        End Set
    End Property

    ''' <summary>
    ''' Address line 10
    ''' </summary>
    ''' <value>String representation of address line 10</value>
    ''' <returns>String reprensentation of address line 10</returns>
    ''' <remarks></remarks>
    Public Property Address10() As String
        Get
            Return sAddress10
        End Get
        Set(ByVal value As String)
            sAddress10 = value
        End Set
    End Property

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by AddressLine 1
    ''' </summary>
    <Serializable()> Public Class SortByAddressLine1 : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Address by their Address1 attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As Address

            If TypeOf x Is Address Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is Address Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.Address1) And String.IsNullOrEmpty(oRight.Address1) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.Address1) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.Address1) Then
                Return 1
            ElseIf oLeft.Address1 < oRight.Address1 Then
                Return -1
            ElseIf oLeft.Address1 = oRight.Address1 Then
                Return 0
            Else
                Return 1
            End If

        End Function
    End Class

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by AddressLine 2
    ''' </summary>
    <Serializable()> Public Class SortByAddressLine2 : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Address by their Address2 attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As Address

            If TypeOf x Is Address Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is Address Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.Address2) And String.IsNullOrEmpty(oRight.Address2) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.Address2) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.Address2) Then
                Return 1
            ElseIf oLeft.Address1 < oRight.Address2 Then
                Return -1
            ElseIf oLeft.Address1 = oRight.Address2 Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by AddressLine 3
    ''' </summary>
    <Serializable()> Public Class SortByAddressLine3 : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Address by their Address3 attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As Address

            If TypeOf x Is Address Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is Address Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.Address3) And String.IsNullOrEmpty(oRight.Address3) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.Address3) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.Address3) Then
                Return 1
            ElseIf oLeft.Address1 < oRight.Address3 Then
                Return -1
            ElseIf oLeft.Address1 = oRight.Address3 Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by AddressLine 4
    ''' </summary>
    <Serializable()> Public Class SortByAddressLine4 : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Address by their Address4 attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As Address

            If TypeOf x Is Address Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is Address Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.Address4) And String.IsNullOrEmpty(oRight.Address4) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.Address4) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.Address4) Then
                Return 1
            ElseIf oLeft.Address1 < oRight.Address4 Then
                Return -1
            ElseIf oLeft.Address1 = oRight.Address4 Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by PostCode
    ''' </summary>
    <Serializable()> Public Class SortByPostcode : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Address by their PostCode attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As Address

            If TypeOf x Is Address Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is Address Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.PostCode) And String.IsNullOrEmpty(oRight.PostCode) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.PostCode) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.PostCode) Then
                Return 1
            ElseIf oLeft.Address1 < oRight.PostCode Then
                Return -1
            ElseIf oLeft.Address1 = oRight.PostCode Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class


    ''' <summary>
    ''' A class implementing IComparer to sort a collection of Party objects by PostCode
    ''' </summary>
    <Serializable()> Public Class SortByCountry : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type Address by their Country attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As Address

            If TypeOf x Is Address Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is Address Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.CountryDescription) And String.IsNullOrEmpty(oRight.CountryDescription) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.CountryDescription) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.CountryDescription) Then
                Return 1
            ElseIf oLeft.Address1 < oRight.CountryDescription Then
                Return -1
            ElseIf oLeft.Address1 = oRight.CountryDescription Then
                Return 0
            Else
                Return 1
            End If

        End Function
    End Class
End Class

''' <summary>
''' Collection of Address objects
''' </summary>
<Serializable()> Public Class AddressCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(Address)
    End Sub
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
    ''' Add an Address object to the collection
    ''' </summary>
    ''' <param name="v_oAddress">The Address object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oAddress As Address) As Integer
        v_oAddress.Key = List.Add(v_oAddress)
        Return v_oAddress.Key
    End Function

    ''' <summary>
    ''' Remove an Address object from the collection
    ''' </summary>
    ''' <param name="v_oAddress">The Address object to be removed</param>
    Public Sub Remove(ByVal v_oAddress As Address)
        List.Remove(v_oAddress)
    End Sub

    ''' <summary>
    ''' Remove an Address object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Address object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Address object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Address object</param>
    ''' <value>The replacement Address object</value>
    ''' <returns>The Address object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Address
        Get
            Return List(i)
        End Get
        Set(ByVal value As Address)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oAddress As Address)
        List.Item(v_oAddress.Key) = v_oAddress
    End Sub

    Public Sub Update(ByVal v_oAddress As Address, ByVal index As Integer)
        List.Item(index) = v_oAddress
    End Sub

    ''' <summary>
    ''' Return the first Address object in the collection with the specified AddressType
    ''' </summary>
    ''' <param name="v_oAddressType">The AddressType of the Address object to be returned</param>
    ''' <value>The AddressType the Address is to be retrieved by</value>
    ''' <returns>Matching Address object, if any</returns>
    Default Public ReadOnly Property Item(ByVal v_oAddressType As AddressType) As Address
        Get
            Return FindItemByAddressType(v_oAddressType)
        End Get
    End Property

    ''' <summary>
    ''' Find the first Address object in the collection with the specified AddressType
    ''' </summary>
    ''' <param name="v_oAddressType">The AddressType of the Address object to be returned</param>
    ''' <returns>The matching Address object, if any</returns>
    Public Function FindItemByAddressType(ByVal v_oAddressType As AddressType) As Address

        For Each oAddress As Address In List
            If oAddress.AddressType = v_oAddressType Then
                Return oAddress
            End If
        Next

        Return Nothing

    End Function

End Class

''' <summary>
''' Address Types
''' </summary>
''' <remarks></remarks>
Public Enum AddressType
    HomeAddress
    BusinessAddress
    OtherAddress
    SubAgent
    BranchAddress
    BillingAddress
    CorrespondenceAddress
    PreviousAddress
    RegisteredAddress
    SiteAddress
    EmailAddress
End Enum
''' <summary>
''' Attribute to sort Address Collection by
''' </summary>
''' <remarks></remarks>
Public Enum AddressSort

    ''' <summary>
    ''' AddressLine1
    ''' </summary>
    AddressLine1 = 0

    ''' <summary>
    ''' AddressLine2
    ''' </summary>
    AddressLine2 = 1

    ''' <summary>
    ''' AddressLine3
    ''' </summary>
    AddressLine3 = 2

    ''' <summary>
    ''' AddressLine43
    ''' </summary>
    AddressLine4 = 4

    ''' <summary>
    ''' PostCode
    ''' </summary>
    PostCode = 5

    ''' <summary>
    ''' Country
    ''' </summary>
    Country = 6

End Enum
