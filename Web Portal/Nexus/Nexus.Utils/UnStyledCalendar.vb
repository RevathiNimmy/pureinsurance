Imports System.Web.UI

Public Class UnStyledCalendar
    Inherits WebControls.Calendar

    'yet another fudge to correct crappy styling in .net

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(New NonStyleWriter(writer))
    End Sub

    Protected Overrides Sub OnDayRender(ByVal cell As System.Web.UI.WebControls.TableCell, ByVal day As System.Web.UI.WebControls.CalendarDay)
        MyBase.OnDayRender(cell, day)
    End Sub

    Private Class NonStyleWriter
        Inherits HtmlTextWriter

        Private _writer As HtmlTextWriter

        Public Sub New(ByVal innerWriter As HtmlTextWriter)
            MyBase.New(innerWriter.InnerWriter)
            _writer = innerWriter
        End Sub

        Public Overrides Sub AddAttribute(ByVal key As System.Web.UI.HtmlTextWriterAttribute, ByVal value As String)
            Select Case key
                Case HtmlTextWriterAttribute.Align
                Case HtmlTextWriterAttribute.Cellpadding
                Case HtmlTextWriterAttribute.Cellpadding
                Case HtmlTextWriterAttribute.Border
                Case Else
                    _writer.AddAttribute(key, value)
            End Select
        End Sub

        Public Overrides Sub AddAttribute(ByVal name As String, ByVal value As String)
            _writer.AddAttribute(name, value)
        End Sub

        Public Overrides Sub AddAttribute(ByVal name As String, ByVal value As String, ByVal fEndode As Boolean)
            _writer.AddAttribute(name, value, fEndode)
        End Sub

        Public Overrides Sub AddAttribute(ByVal key As System.Web.UI.HtmlTextWriterAttribute, ByVal value As String, ByVal fEncode As Boolean)
            _writer.AddAttribute(key, value, fEncode)
        End Sub

        Public Overrides Sub AddStyleAttribute(ByVal name As String, ByVal value As String)
            'Do Nothing, this is intentional
        End Sub

        Public Overrides Sub AddStyleAttribute(ByVal key As System.Web.UI.HtmlTextWriterStyle, ByVal value As String)
            'Do Nothing, this is intentional
        End Sub

        Public Overrides Sub WriteStyleAttribute(ByVal name As String, ByVal value As String)
            'Do Nothing, this is intentional
        End Sub

        Public Overrides Sub WriteStyleAttribute(ByVal name As String, ByVal value As String, ByVal fEncode As Boolean)
            'Do Nothing, this is intentional
        End Sub

        Public Overrides Sub WriteAttribute(ByVal name As String, ByVal value As String)
            _writer.WriteAttribute(name, value)
        End Sub

        Public Overrides Sub WriteAttribute(ByVal name As String, ByVal value As String, ByVal fEncode As Boolean)
            _writer.WriteAttribute(name, value, fEncode)
        End Sub

        Public Overrides Sub Close()
            _writer.Close()
        End Sub

        Public Overrides Sub Flush()
            _writer.Flush()
        End Sub

        Public Overrides Sub RenderBeginTag(ByVal tagName As String)
            _writer.RenderBeginTag(tagName)
        End Sub

        Public Overrides Sub RenderBeginTag(ByVal tagKey As System.Web.UI.HtmlTextWriterTag)
            _writer.RenderBeginTag(tagKey)
        End Sub

        Public Overrides Sub RenderEndTag()
            _writer.RenderEndTag()
        End Sub

        Public Overrides Sub Write(ByVal value As Boolean)
            _writer.Write(value)
        End Sub

        Public Overrides Sub Write(ByVal value As Char)
            _writer.Write(value)
        End Sub

        Public Overrides Sub Write(ByVal buffer() As Char)
            _writer.Write(buffer)
        End Sub

        Public Overrides Sub Write(ByVal buffer() As Char, ByVal index As Integer, ByVal count As Integer)
            _writer.Write(buffer, index, count)
        End Sub

        Public Overrides Sub Write(ByVal value As Decimal)
            _writer.Write(value)
        End Sub

        Public Overrides Sub Write(ByVal value As Double)
            _writer.Write(value)
        End Sub

        Public Overrides Sub Write(ByVal value As Integer)
            _writer.Write(value)
        End Sub

        Public Overrides Sub Write(ByVal value As Long)
            _writer.Write(value)
        End Sub

        Public Overrides Sub Write(ByVal value As Object)
            _writer.Write(value)
        End Sub

        Public Overrides Sub Write(ByVal value As Single)
            _writer.Write(value)
        End Sub

        Public Overrides Sub Write(ByVal s As String)
            _writer.Write(s)
        End Sub

        Public Overrides Sub Write(ByVal format As String, ByVal arg0 As Object)
            _writer.Write(format, arg0)
        End Sub

        Public Overrides Sub Write(ByVal format As String, ByVal arg0 As Object, ByVal arg1 As Object)
            _writer.Write(format, arg0, arg1)
        End Sub

        Public Overrides Sub Write(ByVal format As String, ByVal arg0 As Object, ByVal arg1 As Object, ByVal arg2 As Object)
            _writer.Write(format, arg0, arg1, arg2)
        End Sub

        Public Overrides Sub Write(ByVal format As String, ByVal ParamArray arg() As Object)
            _writer.Write(format, arg)
        End Sub

        Public Overrides Sub Write(ByVal value As UInteger)
            _writer.Write(value)
        End Sub

        Public Overrides Sub Write(ByVal value As ULong)
            _writer.Write(value)
        End Sub

        Public Overrides Sub WriteBeginTag(ByVal tagName As String)
            _writer.WriteBeginTag(tagName)
        End Sub

        Public Overrides Sub WriteEndTag(ByVal tagName As String)
            _writer.WriteEndTag(tagName)
        End Sub

        Public Overrides Sub WriteFullBeginTag(ByVal tagName As String)
            _writer.WriteFullBeginTag(tagName)
        End Sub

        Public Overrides Sub WriteLine()
            _writer.WriteLine()
        End Sub

        Public Overrides Sub WriteLine(ByVal value As Boolean)
            MyBase.WriteLine(value)
        End Sub

        Public Overrides Sub WriteLine(ByVal value As Char)
            _writer.WriteLine(value)
        End Sub

        Public Overrides Sub WriteLine(ByVal buffer() As Char)
            _writer.WriteLine(buffer)
        End Sub

        Public Overrides Sub WriteLine(ByVal buffer() As Char, ByVal index As Integer, ByVal count As Integer)
            _writer.WriteLine(buffer, index, count)
        End Sub

        Public Overrides Sub WriteLine(ByVal value As Decimal)
            _writer.WriteLine(value)
        End Sub

        Public Overrides Sub WriteLine(ByVal value As Double)
            _writer.WriteLine(value)
        End Sub

        Public Overrides Sub WriteLine(ByVal value As Integer)
            _writer.WriteLine(value)
        End Sub

        Public Overrides Sub WriteLine(ByVal value As Long)
            _writer.WriteLine(value)
        End Sub

        Public Overrides Sub WriteLine(ByVal value As Object)
            _writer.WriteLine(value)
        End Sub

        Public Overrides Sub WriteLine(ByVal value As Single)
            _writer.WriteLine(value)
        End Sub

        Public Overrides Sub WriteLine(ByVal s As String)
            _writer.WriteLine(s)
        End Sub

        Public Overrides Sub WriteLine(ByVal format As String, ByVal arg0 As Object)
            _writer.WriteLine(format, arg0)
        End Sub

        Public Overrides Sub WriteLine(ByVal format As String, ByVal arg0 As Object, ByVal arg1 As Object)
            _writer.WriteLine(format, arg0, arg1)
        End Sub

        Public Overrides Sub WriteLine(ByVal format As String, ByVal arg0 As Object, ByVal arg1 As Object, ByVal arg2 As Object)
            _writer.WriteLine(format, arg0, arg1, arg2)
        End Sub

        Public Overrides Sub WriteLine(ByVal format As String, ByVal ParamArray arg() As Object)
            _writer.WriteLine(format, arg)
        End Sub

        Public Overrides Sub WriteLine(ByVal value As UInteger)
            _writer.WriteLine(value)
        End Sub

        Public Overrides Sub WriteLine(ByVal value As ULong)
            _writer.WriteLine(value)
        End Sub

        Public Overrides ReadOnly Property Encoding() As System.Text.Encoding
            Get
                Return _writer.Encoding
            End Get
        End Property

        Public Overrides Property NewLine() As String
            Get
                Return _writer.NewLine
            End Get
            Set(ByVal value As String)
                _writer.NewLine = value
            End Set
        End Property

    End Class

End Class

