Imports System.Web
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Xml
Imports System.Web.UI
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Session


'   *** CURRENCY CONVERSION ***
'
'   Convert between different currencies and format output, use euros as the base for any conversion,
'   Caches values for 4 hours, remote source is updated everyday at 14:00 CET,
'
'   *** Required web.config entries ***
'
'   <add key="WebRoot" value="/ModernSolidWeb/" /> <!-- don't use http:// -->
'   <add key="CurrencyISOCode" value="ZAR" />
'   <add key="RemoteXMLCurrency" value="http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml" />
'   <add key="LocalXMLCurrency" value="xml/currency.xml" />
'
'   DH - Taken from MODERNSOLID - 01/09/05


Public Class Currency

    Private oCurrencyType As String
    Private sDataModelCode As String
    Private oCurrencies As Config.Currencies

    Public Sub New()
        oCurrencyType = AppSettings("CurrencyISOCode")
    End Sub

    Public Sub New(ByVal v_sISOCode As String)
        oCurrencyType = v_sISOCode
    End Sub

    Public ReadOnly Property Type() As String
        Get
            Return oCurrencyType
        End Get
    End Property

    Public ReadOnly Property Symbol() As String
        Get
            'Need to read all the Currencies avail in the Web.Config at Nexus Framework level
            'then return the Symbol of the requested Currency
            oCurrencies = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Currencies

            Return oCurrencies.Currency(oCurrencyType).Symbol
        End Get
    End Property

    Public ReadOnly Property FormatString() As String
        Get
            'Need to read all the Currencies avail in the Web.Config at Nexus Framework level
            'then return the FormatString of the requested Currency
            oCurrencies = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Currencies

            Return oCurrencies.Currency(oCurrencyType.Trim()).FormatString
        End Get
    End Property

    Public Shared Operator =(ByVal lhs As Currency, ByVal rhs As Currency) As Boolean

        If lhs.Type = rhs.Type Then
            Return True
        Else
            Return False
        End If

    End Operator

    Public Shared Operator <>(ByVal lhs As Currency, ByVal rhs As Currency) As Boolean

        If lhs.Type <> rhs.Type Then
            Return True
        Else
            Return False
        End If

    End Operator

End Class

Public Class Money

    Private dMoney As Double
    Private oCurrency As Currency

    Public Sub New(ByVal Money As Double, Optional ByVal v_oCurrencyType As String = "")
        'Initialize new money object
        dMoney = Money
        If v_oCurrencyType = "" Then
            'if Blank, Need to take the Default Currency Code set in Web.Config
            oCurrency = New Currency(AppSettings("CurrencyISOCode"))
        Else
            oCurrency = New Currency(v_oCurrencyType)
        End If
    End Sub

    Public ReadOnly Property Value(Optional ByVal v_oCurrencyType As String = "") As Double
        'Return numeric value of currency, converted to the passed currency e.g 1.50
        Get
            If v_oCurrencyType = "" Then v_oCurrencyType = oCurrency.Type

            If v_oCurrencyType = oCurrency.Type Then
                'Dont convert use, stored value
                Return dMoney
            Else
                Return Convert(v_oCurrencyType)
            End If
        End Get
    End Property

    Public ReadOnly Property Formatted(Optional ByVal v_oCurrencyType As String = "") As String
        'Return string value of currency, formatted and with suffix or prefix of passed currency e.g £1.50
        Get
            If v_oCurrencyType = "" Then v_oCurrencyType = oCurrency.Type

            If v_oCurrencyType = oCurrency.Type Then
                'Dont convert use, stored value
                Return FormatCurrency(dMoney, oCurrency)
            Else
                Return FormatCurrency(Convert(v_oCurrencyType), New Currency(v_oCurrencyType))
            End If
        End Get
    End Property

    Public Function Rate(ByVal v_oCurrencyType As String) As Double
        'Returns the exchange rate of the passed currency, base rate is euros

        'Data from ECB is mid rate figures using Euros as the base rate
        'http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml

        Dim dRates As New Hashtable()

        If Not HttpContext.Current.Cache("ECBData") Is Nothing Then
            'Use cache data
            dRates = CType(HttpContext.Current.Cache("ECBData"), Hashtable)
        Else
            Dim sRemote As String = AppSettings("RemoteXMLCurrency")
            Dim sLocal As String = AppSettings("WebRoot") & AppSettings("LocalXMLCurrency")

            Try
                'Read from remote source
                Dim xtrSource As New XmlTextReader(sRemote)
                Dim xdData As New XmlDocument
                xdData.Load(xtrSource)
                xtrSource.Close()

                dRates = ReadXMLData(xdData)

                Dim xtwLocalCopy As New XmlTextWriter(HttpContext.Current.Server.MapPath(sLocal), Encoding.UTF8)
                xdData.WriteTo(xtwLocalCopy)
                xtwLocalCopy.Close()

            Catch exSource As Exception
                'Read from local source
                Try
                    Dim xtrSource As New XmlTextReader(sLocal)
                    Dim xdData As New XmlDocument
                    xdData.Load(xtrSource)
                    xtrSource.Close()

                    dRates = ReadXMLData(xdData)

                Catch exLocal As Exception

                End Try

            End Try

            'Cache for 4 hours, as the feed is updated a 14:00 CET
            HttpContext.Current.Cache.Insert("ECBData", dRates, Nothing, Now.AddHours(4), TimeSpan.Zero)
        End If

        Return dRates(v_oCurrencyType)

    End Function

    Private Function ReadXMLData(ByRef pxdData As XmlDocument) As Hashtable
        'Process the xml document into the array of currencies

        Dim tmpRates As New Hashtable()

        Dim xnTmp As XmlNode
        For Each xnTmp In pxdData.ChildNodes(1).ChildNodes(2).ChildNodes(0).ChildNodes
            Dim oCurrencyTmp As New Currency(xnTmp.Attributes(0).InnerText)
            If oCurrencyTmp.Type <> "" Then
                tmpRates.Add(xnTmp.Attributes(0).InnerText.ToString, CDec(xnTmp.Attributes(1).InnerText))
            End If
        Next

        Return tmpRates

    End Function

    Private Function FormatCurrency(ByVal pMoney As Double, ByVal pOutputCurrency As Currency) As String
        Return Format(pMoney, pOutputCurrency.FormatString)
    End Function

    Private Function Convert(ByVal v_oCurrencyType As String) As Double

        'Carries out the actual conversion between currencies, base rate
        'is euros so if the source value isn't euros we need to convert
        'to euros before we can convert to destination currency
        If dMoney > 0 Then
            If v_oCurrencyType = oCurrency.Type Then
                'No Conversion neccessary
                Return dMoney
            Else
                'If v_oCurrencyType = CurrencyType.Euros Then
                If v_oCurrencyType = "EUR" Then
                    'divide by currency rate as the base rate values are euros
                    Return dMoney / Rate(oCurrency.Type)
                Else
                    If oCurrency.Type = "EUR" Then
                        'mulitple by the currency rate as the base rate values are euros
                        Return dMoney * Rate(v_oCurrencyType)
                    Else
                        'Convert to euros and then output currency
                        Return (dMoney / Rate(oCurrency.Type) * Rate(v_oCurrencyType))
                    End If
                End If
            End If
        Else
            Return 0
        End If

    End Function

End Class
Public NotInheritable Class TransactionCurrency


    Public Shared ReadOnly Property Symbol() As String
        Get
            'Need to read all the Currencies avail in the Web.Config at Nexus Framework level
            'then return the Symbol of the requested Currency
            Dim oCurrencyType As String = String.Empty
            Dim oCurrencies As Config.Currencies
            If HttpContext.Current.Session("CURRENYCODE") IsNot Nothing Then
                oCurrencyType = HttpContext.Current.Session("CURRENYCODE")
            Else
                oCurrencyType = AppSettings("CurrencyISOCode")
            End If
            Try
                oCurrencies = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Currencies
                Return oCurrencies.Currency(oCurrencyType).Symbol
            Catch ex As Exception
                Return String.Empty
            End Try
        End Get
    End Property
End Class

