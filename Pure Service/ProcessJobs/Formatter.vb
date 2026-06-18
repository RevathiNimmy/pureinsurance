''' <summary>
''' Utility methods to format various classes for storing in the message table error details column.
''' </summary>
Friend NotInheritable Class Formatter

#Region "Private Constants"

    Private Const CannotReadText As String = "(cannot read)"
    Private Const NullText As String = "(null)"

#End Region

#Region "Constructors"

     ''' <summary>
     ''' This class cannot be instantiated.
     ''' </summary>
    Private Sub New()
    End Sub

#End Region

#Region "Public Shared Methods"

     ''' <summary>
     ''' Format an arbitrary exception.
     ''' </summary>
    Public Shared Function Format(ByVal ex As Exception) As String

        Dim text As New StringBuilder

        Dim separator As Boolean = False
        Do
            If separator Then
                text.AppendLine()
                text.AppendLine("InnerException")
                text.AppendLine("--------------")
                text.AppendLine()
            End If
            FormatException(text, ex)
            ex = ex.InnerException
            separator = True
        Loop Until ex Is Nothing

        Return text.ToString().TrimEnd()

    End Function

#End Region

#Region "Private Shared Methods"

     ''' <summary>
     ''' Format all properties of an exception type except <see cref="Exception.InnerException"/>.
     ''' </summary>
    Private Shared Sub FormatException(ByVal text As StringBuilder, ByVal ex As Exception)

        text.AppendLine(ex.Message.TrimEnd())

        FormatString(text, "Type", ex.GetType().FullName)

        Try
            For Each item As PropertyInfo In ex.GetType().FindMembers(MemberTypes.Property, BindingFlags.Instance Or BindingFlags.Public, AddressOf FormatExceptionFilter, Nothing)
                FormatProperty(text, item.Name, item.GetValue(ex, Nothing))
            Next
        Catch
            FormatString(text, "Properties", CannotReadText)
        End Try

        FormatDictionary(text, "Data", ex.Data)

        FormatString(text, "Source", ex.Source)

        text.AppendLine(ex.StackTrace.TrimEnd())

    End Sub

    ''' <summary>
    ''' Format all public readable non-indexable properties of an object.
    ''' </summary>
    Private Shared Sub FormatObject(ByVal text As StringBuilder, ByVal name As String, ByVal value As Object)

        Try
            For Each item As PropertyInfo In value.GetType().FindMembers(MemberTypes.Property, BindingFlags.Instance Or BindingFlags.Public, AddressOf FormatObjectFilter, Nothing)
                FormatProperty(text, name & "." & item.Name, item.GetValue(value, Nothing))
            Next
        Catch
            FormatString(text, name, CannotReadText)
        End Try

    End Sub

    ''' <summary>
    ''' Format all items in an enumerable list.
    ''' </summary>
    Private Shared Sub FormatEnumerable(ByVal text As StringBuilder, ByVal name As String, ByVal value As IEnumerable)

        Try
            Dim itemIndex As Integer = 0
            For Each item As Object In value
                FormatProperty(text, name & "[" & itemIndex & "]", item)
                itemIndex += 1
            Next
        Catch
            FormatString(text, name, CannotReadText)
        End Try

    End Sub

    ''' <summary>
    ''' Format all entries in a dictionary.
    ''' </summary>
    Private Shared Sub FormatDictionary(ByVal text As StringBuilder, ByVal name As String, ByVal value As IDictionary)

        Try
            For Each item As DictionaryEntry In value
                FormatProperty(text, name & "[" & item.Key.ToString() & "]", item.Value)
            Next
        Catch
            FormatString(text, name, CannotReadText)
        End Try

    End Sub

    ''' <summary>
    ''' Format the whole contents of a stream converted to text using the specified encoding.
    ''' </summary>
    Private Shared Sub FormatStream(ByVal text As StringBuilder, ByVal name As String, ByVal value As Stream, ByVal encoding As Encoding)

        Try
            Const bufferSize As Integer = 1024 * 64 ' always buffer the stream for speed
            Using reader As New StreamReader(value, encoding, True, bufferSize)
                FormatProperty(text, name, reader.ReadToEnd())
            End Using
        Catch
            FormatString(text, name, CannotReadText)
        End Try

    End Sub

    ''' <summary>
    ''' Format a name/value pair.
    ''' </summary>
    Private Shared Sub FormatProperty(ByVal text As StringBuilder, ByVal name As String, ByVal value As Object)

        Try
            If value Is Nothing Then
                FormatString(text, name, NullText)
            ElseIf TypeOf value Is HttpWebResponse Then
                FormatObject(text, name, value)
                FormatStream(text, name & ".ResponseStream", _
                    DirectCast(value, HttpWebResponse).GetResponseStream(), _
                    GetEncoding(DirectCast(value, HttpWebResponse).CharacterSet))
            ElseIf TypeOf value Is CookieCollection OrElse TypeOf value Is WebHeaderCollection Then
                FormatEnumerable(text, name, DirectCast(value, IEnumerable))
            Else
                FormatString(text, name, value.ToString())
            End If
        Catch
            FormatString(text, name, CannotReadText)
        End Try

    End Sub

     ''' <summary>
     ''' Format a name/value pair of type string.
     ''' </summary>
    Private Shared Sub FormatString(ByVal text As StringBuilder, ByVal name As String, ByVal value As String)

        text.Append(name)
        text.Append(": ")
        text.Append(value)
        text.AppendLine()

    End Sub

     ''' <summary>
     ''' Member filter function for use in <see cref="Format"/>.
     ''' </summary>
    Private Shared Function FormatExceptionFilter(ByVal memberInfo As MemberInfo, ByVal filterCriteria As Object) As Boolean

         ' Skip over all the basic exception properties because we handle them separately.
        Select Case memberInfo.Name
            Case "Data", "HelpLink", "InnerException", "Message", "Source", "StackTrace", "TargetSite"
                Return False
        End Select

        Return FormatObjectFilter(memberInfo, filterCriteria)

    End Function

     ''' <summary>
     ''' Member filter function for use in <see cref="FormatObject"/>.
     ''' </summary>
    Private Shared Function FormatObjectFilter(ByVal memberInfo As MemberInfo, ByVal filterCriteria As Object) As Boolean

         ' Only read properties that are readable and have no index parameters.
        Dim propertyInfo As PropertyInfo = TryCast(memberInfo, PropertyInfo)
        Return propertyInfo IsNot Nothing AndAlso propertyInfo.CanRead AndAlso propertyInfo.GetIndexParameters().Length = 0

    End Function

     ''' <summary>
     ''' Wrapper round <see cref="Encoding.GetEncoding"/>, but with blank string indicating the default system encoding.
     ''' </summary>
    Private Shared Function GetEncoding(ByVal name As String) As Encoding

        If String.IsNullOrEmpty(name) Then
            Return Encoding.Default
        Else
            Return Encoding.GetEncoding(name)
        End If

    End Function

#End Region

End Class
