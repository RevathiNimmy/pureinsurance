<Serializable()> Public Class Convictions
    Private sKey As String
    Private iConvictionKey As Integer
    Private sTypeCode As String
    Private sStatusCode As String
    Private sDescription As String
    Private dtConvictionDate As Date
    Private dFineAmount As Decimal
    Private sSentenceTypeCode As String
    Private sSentenceDescription As String
    Private dSentenceDuration As Decimal
    Private sSentenceDurationQualifier As String
    Private dtSentenceEffectiveDate As DateTime
    Private dAlcoholLevel As Integer
    Private sAlcoholMeasurementMethod As String
    Private dDrivingLicensePenaltyPoints As Integer
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("ConvictionKey : " & iConvictionKey.ToString & "<br />")
        sbPrint.AppendLine("TypeCode : " & sTypeCode & "<br />")
        sbPrint.AppendLine("StatusCode : " & sStatusCode & "<br />")
        sbPrint.AppendLine("Description: " & sDescription & "<br />")
        sbPrint.AppendLine("Date : " & dtConvictionDate.ToString & "<br />")
        sbPrint.AppendLine("FineAmount : " & dFineAmount & "<br />")
        sbPrint.AppendLine("SentenceTypeCode : " & sSentenceTypeCode & "<br />")
        sbPrint.AppendLine("SentenceDescription: " & sSentenceDescription & "<br />")
        sbPrint.AppendLine("SentenceDuration : " & dSentenceDuration & "<br />")
        sbPrint.AppendLine("SentenceDurationQualifier : " & sSentenceDurationQualifier & "<br />")
        sbPrint.AppendLine("SentenceEffectiveDate : " & dtSentenceEffectiveDate & "<br />")
        sbPrint.AppendLine("AlcoholLevel: " & dAlcoholLevel & "<br />")
        sbPrint.AppendLine("AlcoholMeasurementMethod : " & sAlcoholMeasurementMethod & "<br />")
        sbPrint.AppendLine("DrivingLicensePenaltyPoints : " & dDrivingLicensePenaltyPoints & "<br />")
        Return sbPrint.ToString

    End Function

    Public Property Key() As String
        Get
            Return Me.sKey
        End Get
        Set(ByVal value As String)
            Me.sKey = value
        End Set
    End Property

    Public Property ConvictionKey() As Integer
        Get
            Return iConvictionKey
        End Get
        Set(ByVal value As Integer)
            iConvictionKey = value
        End Set
    End Property

    Public Property TypeCode() As String
        Get
            Return sTypeCode
        End Get
        Set(ByVal value As String)
            sTypeCode = value
        End Set
    End Property
    Public Property StatusCode() As String
        Get
            Return sStatusCode
        End Get
        Set(ByVal value As String)
            sStatusCode = value
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
    Public Property ConvictionDate() As Date
        Get
            Return dtConvictionDate
        End Get
        Set(ByVal value As Date)
            dtConvictionDate = value
        End Set
    End Property
    Public Property FineAmount() As Decimal
        Get
            Return dFineAmount
        End Get
        Set(ByVal value As Decimal)
            dFineAmount = value
        End Set
    End Property
    Public Property SentenceTypeCode() As String
        Get
            Return sSentenceTypeCode
        End Get
        Set(ByVal value As String)
            sSentenceTypeCode = value
        End Set
    End Property
    Public Property SentenceDescription() As String
        Get
            Return sSentenceDescription
        End Get
        Set(ByVal value As String)
            sSentenceDescription = value
        End Set
    End Property
    Public Property SentenceDuration() As Decimal
        Get
            Return dSentenceDuration
        End Get
        Set(ByVal value As Decimal)
            dSentenceDuration = value
        End Set
    End Property
    Public Property SentenceDurationQualifier() As String
        Get
            Return sSentenceDurationQualifier
        End Get
        Set(ByVal value As String)
            sSentenceDurationQualifier = value
        End Set
    End Property
    Public Property SentenceEffectiveDate() As Date
        Get
            Return dtSentenceEffectiveDate
        End Get
        Set(ByVal value As Date)
            dtSentenceEffectiveDate = value
        End Set
    End Property
    Public Property AlcoholLevel() As Decimal
        Get
            Return dAlcoholLevel
        End Get
        Set(ByVal value As Decimal)
            dAlcoholLevel = value
        End Set
    End Property
    Public Property AlcoholMeasurementMethod() As String
        Get
            Return sAlcoholMeasurementMethod
        End Get
        Set(ByVal value As String)
            sAlcoholMeasurementMethod = value
        End Set
    End Property
    Public Property DrivingLicensePenaltyPoints() As Decimal
        Get
            Return dDrivingLicensePenaltyPoints
        End Get
        Set(ByVal value As Decimal)
            dDrivingLicensePenaltyPoints = value
        End Set
    End Property
End Class

<Serializable()> Public Class ConvictionCollection : Inherits CollectionBase

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
    ''' Add an Conviction object to the collection
    ''' </summary>
    ''' <param name="v_oConviction">The Conviction object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oConviction As Convictions) As Integer
        v_oConviction.Key = List.Add(v_oConviction)
        Return v_oConviction.Key
    End Function

    ''' <summary>
    ''' Remove an Conviction object from the collection
    ''' </summary>
    ''' <param name="v_oConviction">The Conviction object to be removed</param>
    Public Sub Remove(ByVal v_oConviction As Convictions)
        List.Remove(v_oConviction)
    End Sub

    ''' <summary>
    ''' Remove an Conviction object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Conviction object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Conviction object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Conviction object</param>
    ''' <value>The replacement Conviction object</value>
    ''' <returns>The Conviction object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Convictions
        Get
            Return List(i)
        End Get
        Set(ByVal value As Convictions)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oConviction As Convictions)
        List.Item(v_oConviction.Key) = v_oConviction
    End Sub

    Public Sub Update(ByVal v_oConviction As Convictions, ByVal index As Integer)
        List.Item(index) = v_oConviction
    End Sub

    '''' <summary>
    '''' Return the first Conviction object in the collection with the specified ConvictionType
    '''' </summary>
    '''' <param name="v_oConvictionType">The ConvictionType of the Conviction object to be returned</param>
    '''' <value>The ConvictionType the Conviction is to be retrieved by</value>
    '''' <returns>Matching Conviction object, if any</returns>
    'Default Public ReadOnly Property Item(ByVal v_oConvictionType As ConvictionType) As Convictions
    '    Get
    '        Return FindItemByConvictionType(v_oConvictionType)
    '    End Get
    'End Property

    '''' <summary>
    '''' Find the first Conviction object in the collection with the specified ConvictionType
    '''' </summary>
    '''' <param name="v_oConvictionType">The ConvictionType of the Conviction object to be returned</param>
    '''' <returns>The matching Conviction object, if any</returns>
    'Public Function FindItemByConvictionType(ByVal v_oConvictionType As ConvictionType) As Convictions

    '    For Each oConviction As Convictions In List
    '        If oConviction.ConvictionType = v_oConvictionType Then
    '            Return oConviction
    '        End If
    '    Next
    '    Return Nothing
    'End Function

End Class