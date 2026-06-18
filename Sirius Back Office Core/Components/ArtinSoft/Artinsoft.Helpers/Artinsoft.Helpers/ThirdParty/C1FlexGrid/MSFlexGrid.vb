Namespace VBUtils

    Public Class MSFlexGrid

        Public Shared Function GetCellAlignment( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As C1.Win.C1FlexGrid.TextAlignEnum

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.TextAlign
            Else
                Return TheCell.TextAlign
            End If

        End Function

        Public Shared Sub SetCellAlignment( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheTextAlign As C1.Win.C1FlexGrid.TextAlignEnum)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString + "_" _
                    + TheFlexGrid.Col.ToString)
                TheCell.TextAlign = TheTextAlign
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            Else
                TheCell.TextAlign = TheTextAlign
            End If

        End Sub

        Public Shared Function GetCellBackColor( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As Color

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.BackColor
            Else
                Return TheCell.BackColor
            End If

        End Function

        Public Shared Sub SetCellBackColor( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheColor As System.Drawing.Color)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString + "_" _
                    + TheFlexGrid.Col.ToString)
                TheCell.BackColor = TheColor
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            Else
                TheCell.BackColor = TheColor
            End If

        End Sub

        Public Shared Function GetCellFontBold( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As Boolean

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.Font.Bold
            Else
                Return TheCell.Font.Bold
            End If

        End Function

        Public Shared Sub SetCellFontBold( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheFontBold As Boolean)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            Dim TheFont As System.Drawing.Font

            Dim Flag As Boolean = False
            If (TheCell Is Nothing) Then
                Flag = True
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString _
                    + "_" + TheFlexGrid.Col.ToString)
            End If

            Dim TheFontStyle As System.Drawing.FontStyle
            If TheFontBold Then TheFontStyle = System.Drawing.FontStyle.Bold
            If TheCell.Font.Italic Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Italic
            If TheCell.Font.Strikeout Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Strikeout
            If TheCell.Font.Underline Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Underline
            TheFont = New System.Drawing.Font( _
                TheCell.Font.FontFamily, _
                TheCell.Font.Size, _
                TheFontStyle)

            TheCell.Font = TheFont
            If Flag Then
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            End If

        End Sub

        Public Shared Function GetCellFontItalic( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As Boolean

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.Font.Italic
            Else
                Return TheCell.Font.Italic
            End If

        End Function

        Public Shared Sub SetCellFontItalic( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheFontItalic As Boolean)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            Dim TheFont As System.Drawing.Font

            Dim Flag As Boolean = False
            If (TheCell Is Nothing) Then
                Flag = True
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString _
                    + "_" + TheFlexGrid.Col.ToString)
            End If

            Dim TheFontStyle As System.Drawing.FontStyle
            If TheFontItalic Then _
                TheFontStyle = System.Drawing.FontStyle.Italic
            If TheCell.Font.Bold Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Bold
            If TheCell.Font.Strikeout Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Strikeout
            If TheCell.Font.Underline Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Underline
            TheFont = New System.Drawing.Font( _
                TheCell.Font.FontFamily, _
                TheCell.Font.Size, _
                TheFontStyle)

            TheCell.Font = TheFont
            If Flag Then
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            End If

        End Sub

        Public Shared Function GetCellFontStrikeThrough( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As Boolean

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.Font.Strikeout
            Else
                Return TheCell.Font.Strikeout
            End If

        End Function

        Public Shared Sub SetCellFontStrikeThrough( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheFontStrikeThrough As Boolean)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            Dim TheFont As System.Drawing.Font

            Dim Flag As Boolean = False
            If (TheCell Is Nothing) Then
                Flag = True
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString _
                    + "_" + TheFlexGrid.Col.ToString)
            End If

            Dim TheFontStyle As System.Drawing.FontStyle
            If TheFontStrikeThrough Then _
                TheFontStyle = System.Drawing.FontStyle.Strikeout
            If TheCell.Font.Bold Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Bold
            If TheCell.Font.Italic Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Italic
            If TheCell.Font.Underline Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Underline
            TheFont = New System.Drawing.Font( _
                TheCell.Font.FontFamily, _
                TheCell.Font.Size, _
                TheFontStyle)

            TheCell.Font = TheFont
            If Flag Then
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            End If

        End Sub

        Public Shared Function GetCellFontUnderline( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As Boolean

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.Font.Underline
            Else
                Return TheCell.Font.Underline
            End If

        End Function

        Public Shared Sub SetCellFontUnderline( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheFontUnderline As Boolean)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            Dim TheFont As System.Drawing.Font

            Dim Flag As Boolean = False
            If (TheCell Is Nothing) Then
                Flag = True
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString _
                    + "_" + TheFlexGrid.Col.ToString)
            End If

            Dim TheFontStyle As System.Drawing.FontStyle
            If TheFontUnderline Then _
                TheFontStyle = System.Drawing.FontStyle.Underline
            If TheCell.Font.Bold Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Bold
            If TheCell.Font.Italic Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Italic
            If TheCell.Font.Strikeout Then TheFontStyle = TheFontStyle Or _
                System.Drawing.FontStyle.Strikeout
            TheFont = New System.Drawing.Font( _
                TheCell.Font.FontFamily, _
                TheCell.Font.Size, _
                TheFontStyle)

            TheCell.Font = TheFont
            If Flag Then
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            End If

        End Sub

        Public Shared Function GetCellFontName( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As Boolean

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.Font.Name
            Else
                Return TheCell.Font.Name
            End If

        End Function

        Public Shared Sub SetCellFontName( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheFontName As String)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            Dim TheFont As System.Drawing.Font

            Dim Flag As Boolean = False
            If (TheCell Is Nothing) Then
                Flag = True
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString _
                    + "_" + TheFlexGrid.Col.ToString)
            End If

            TheFont = New System.Drawing.Font( _
                New System.Drawing.FontFamily(TheFontName), _
                TheCell.Font.Size, _
                TheCell.Font.Style)

            TheCell.Font = TheFont
            If Flag Then
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            End If

        End Sub

        Public Shared Function GetCellFontSize( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As Boolean

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.Font.Size
            Else
                Return TheCell.Font.Size
            End If

        End Function

        Public Shared Sub SetCellFontSize( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheFontSize As Integer)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            Dim TheFont As System.Drawing.Font

            Dim Flag As Boolean = False
            If (TheCell Is Nothing) Then
                Flag = True
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString _
                    + "_" + TheFlexGrid.Col.ToString)
            End If

            TheFont = New System.Drawing.Font( _
                TheCell.Font.FontFamily, _
                TheFontSize, _
                TheCell.Font.Style)

            TheCell.Font = TheFont
            If Flag Then
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            End If

        End Sub

        Public Shared Function GetCellForeColor( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As Color

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.ForeColor
            Else
                Return TheCell.ForeColor
            End If

        End Function

        Public Shared Sub SetCellForeColor( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheColor As System.Drawing.Color)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString + "_" _
                    + TheFlexGrid.Col.ToString)
                TheCell.ForeColor = TheColor
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            Else
                TheCell.ForeColor = TheColor
            End If

        End Sub

        Public Shared Function GetCellPictureAlignment( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As C1.Win.C1FlexGrid.ImageAlignEnum

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.ImageAlign
            Else
                Return TheCell.ImageAlign
            End If

        End Function

        Public Shared Sub SetCellPictureAlignment( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheImageAlign As C1.Win.C1FlexGrid.ImageAlignEnum)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString _
                    + "_" + TheFlexGrid.Col.ToString)
                TheCell.ImageAlign = TheImageAlign
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            Else
                TheCell.ImageAlign = TheImageAlign
            End If

        End Sub

        Public Shared Function GetCellTextStyle( _
                ByVal TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid) _
                As C1.Win.C1FlexGrid.TextEffectEnum

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                Return TheFlexGrid.Styles.Normal.TextEffect
            Else
                Return TheCell.TextEffect
            End If

        End Function

        Public Shared Sub SetCellTextStyle( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheTextEffect As C1.Win.C1FlexGrid.TextEffectEnum)

            Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                TheFlexGrid.GetCellStyle(TheFlexGrid.Row, TheFlexGrid.Col)
            If (TheCell Is Nothing) Then
                TheCell = TheFlexGrid.Styles.Add("style_" _
                    + TheFlexGrid.Row.ToString _
                    + "_" + TheFlexGrid.Col.ToString)
                TheCell.TextEffect = TheTextEffect
                TheFlexGrid.SetCellStyle(TheFlexGrid.Row, _
                    TheFlexGrid.Col, TheCell)
            Else
                TheCell.TextEffect = TheTextEffect
            End If

        End Sub

        Public Shared Sub SetColumnAlignment( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheColumn As Integer, _
                ByVal TheTextAlign As C1.Win.C1FlexGrid.TextAlignEnum)

            For TheRow As Integer = 0 To TheFlexGrid.Rows.Count - 1
                Dim TheCell As C1.Win.C1FlexGrid.CellStyle = _
                    TheFlexGrid.GetCellStyle(TheRow, TheColumn)
                If (TheCell Is Nothing) Then
                    TheCell = TheFlexGrid.Styles.Add("style_" _
                        + TheRow.ToString + "_" _
                        + TheColumn.ToString)
                    TheCell.TextAlign = TheTextAlign
                    TheFlexGrid.SetCellStyle(TheRow, TheColumn, TheCell)
                Else
                    TheCell.TextAlign = TheTextAlign
                End If
            Next

        End Sub

        Public Shared Function GetTextArray( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheCellIndex As Integer) As String

            Dim TheRow As Integer = _
                (TheCellIndex - _
                (TheCellIndex Mod TheFlexGrid.Cols.Count)) / _
                TheFlexGrid.Cols.Count
            Dim TheCol As Integer = _
                TheCellIndex Mod TheFlexGrid.Cols.Count

            Return TheFlexGrid.Item(TheRow, TheCol)

        End Function

        Public Shared Sub SetTextArray( _
                ByRef TheFlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, _
                ByVal TheCellIndex As Integer, _
                ByVal TheValue As String)

            Dim TheRow As Integer = _
                (TheCellIndex - _
                (TheCellIndex Mod TheFlexGrid.Cols.Count)) / _
                TheFlexGrid.Cols.Count
            Dim TheCol As Integer = _
                TheCellIndex Mod TheFlexGrid.Cols.Count
            TheFlexGrid.Item(TheRow, TheCol) = TheValue

        End Sub

    End Class

End Namespace