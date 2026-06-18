<Serializable()> Public Class CreditCardTypeCardHolder

    Private iKey As Integer
    Private sName As String
    Private sAddress1 As String
    Private sAddress2 As String
    Private sAddress3 As String
    Private sAddress4 As String
    Private sPostCode As String
    Private sCountryCode As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal v_sAddress1 As String, ByVal v_sPostCode As String, Optional ByVal v_sCountryCode As String = Nothing)
        sAddress1 = v_sAddress1
        sPostCode = v_sPostCode
        sCountryCode = v_sCountryCode
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Key : " & iKey & "<br />")
        sbPrint.AppendLine("Address Line 1 : " & sAddress1 & "<br />")
        sbPrint.AppendLine("Address Line 2 : " & sAddress2 & "<br />")
        sbPrint.AppendLine("Address Line 3 : " & sAddress3 & "<br />")
        sbPrint.AppendLine("Address Line 4 : " & sAddress4 & "<br />")
        sbPrint.AppendLine("Country Code : " & sCountryCode & "<br />")
        sbPrint.AppendLine("Post Code : " & sPostCode & "<br />")

        Return sbPrint.ToString

    End Function

    Public Property Key() As Integer
        Get
            Return iKey
        End Get
        Set(ByVal value As Integer)
            iKey = value
        End Set
    End Property
    Public Property Address1() As String
        Get
            Return sAddress1
        End Get
        Set(ByVal value As String)
            sAddress1 = value
        End Set
    End Property

    Public Property Address2() As String
        Get
            Return sAddress2
        End Get
        Set(ByVal value As String)
            sAddress2 = value
        End Set
    End Property

    Public Property Address3() As String
        Get
            Return sAddress3
        End Get
        Set(ByVal value As String)
            sAddress3 = value
        End Set
    End Property

    Public Property Address4() As String
        Get
            Return sAddress4
        End Get
        Set(ByVal value As String)
            sAddress4 = value
        End Set
    End Property

    Public Property PostCode() As String
        Get
            If sCountryCode IsNot Nothing Then
                If sCountryCode.Equals("GBR") Then

                    'reformat UK only postcodes with seperator, we can't do
                    'this on set as the country code may not have been set
                    If sPostCode.IndexOf(" ") >= 0 Then
                    Else
                        If sPostCode.Length > 4 Then
                            Dim iLen As Int16 = sPostCode.Length
                            sPostCode = Left(sPostCode, iLen - 3) & " " & Right(sPostCode, iLen - (iLen - 3))
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

    Public Property CountryCode() As String
        Get
            Return sCountryCode
        End Get
        Set(ByVal value As String)
            sCountryCode = value
        End Set
    End Property


    Public Property Name() As String
        Get
            Return sName
        End Get
        Set(ByVal value As String)
            sName = value
        End Set
    End Property
End Class

<Serializable()> Public Class CreditCardTypeCardHolderCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCreditCardTypeCardHolder As CreditCardTypeCardHolder In List
            sbPrint.AppendLine(oCreditCardTypeCardHolder.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oCreditCardTypeCardHolder As CreditCardTypeCardHolder) As Integer
        Return List.Add(v_oCreditCardTypeCardHolder)
    End Function

    Public Sub Remove(ByVal v_oCreditCardTypeCardHolder As CreditCardTypeCardHolder)
        List.Remove(v_oCreditCardTypeCardHolder)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CreditCardTypeCardHolder
        Get
            Return List(i)
        End Get
        Set(ByVal value As CreditCardTypeCardHolder)
            List(i) = value
        End Set
    End Property
End Class



