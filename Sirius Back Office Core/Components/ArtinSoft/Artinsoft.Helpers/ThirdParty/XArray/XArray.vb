Namespace VBUtils
    ''' <summary>
    ''' This class is a support class to simulate XarrayDbObject funcionality based on DataTable class.
    ''' </summary>
    ''' <remarks>
    ''' It supposes that is used only for managing two-dimensions array. Any other case is not supported for this class.
    ''' </remarks>
    ''' 

    Public Class XArray
        Inherits DataTable

        'Variables to save lengths and LowerBounds to handle indexes
        Private DimensionLowerBounds() As Integer
        Private DimensionLengths() As Integer

        'Constructor
        Public Sub New()
            DimensionLowerBounds = Nothing
            DimensionLengths = Nothing
        End Sub



        ''' <summary>
        ''' This function is a Factory to create Xarray instances. 
        ''' </summary>
        ''' <param name="Lengths"> Dimension lengths </param>
        ''' <param name="LowerBounds">Dimension Lower Bounds</param>
        ''' <returns>A new XArray instance</returns>
        ''' <remarks></remarks>
        Public Shared Function CreateInstanceXarray(ByVal Lengths() As Integer, ByVal LowerBounds() As Integer) As XArray
            Dim xarr As New XArray

            xarr.DimensionLengths = Lengths
            xarr.DimensionLowerBounds = LowerBounds
            For col As Integer = 0 To Lengths(1)
                xarr.Columns.Add(New DataColumn())
            Next

            For i As Integer = 0 To Lengths(0)
                Dim row As DataRow = xarr.NewRow()
                xarr.Rows.Add(row)
            Next
            Return xarr
        End Function

        ''' <summary>
        ''' This function redimensions a Xarray instance
        ''' </summary>
        ''' <param name="Lengths">New dimension Lengths</param>
        ''' <param name="LowerBounds">New dimension lower bounds</param>
        ''' <returns>It returns a redimensioned instance of itself</returns>
        ''' <remarks></remarks>
        Public Function RedimXArray(ByVal Lengths() As Integer, ByVal LowerBounds() As Integer) As XArray

            DimensionLengths = Lengths
            DimensionLowerBounds = LowerBounds

            If Me.Columns.Count = 0 Then
                For colIndex As Integer = 0 To Lengths(1)
                    Me.Columns.Add(New DataColumn())
                Next
            ElseIf Me.Columns.Count < Lengths(1) + 1 Then
                For colIndex As Integer = Me.Columns.Count To Lengths(1)
                    Me.Columns.Add(New DataColumn())
                Next
            ElseIf Me.Columns.Count > Lengths(1) + 1 Then
                For colIndex As Integer = Lengths(1) + 1 To Me.Columns.Count - 1
                    Me.Columns.RemoveAt(colIndex)
                Next
            End If

            If Me.Rows.Count = 0 Then
                For rowIndex As Integer = 0 To Lengths(0)
                    Dim row As DataRow = Me.NewRow()
                    Me.Rows.Add(row)
                Next
            ElseIf Me.Rows.Count < Lengths(0) + 1 Then
                For rowIndex As Integer = Me.Rows.Count To Lengths(0)
                    Dim row As DataRow = Me.NewRow()
                    Me.Rows.Add(row)
                Next
            ElseIf Me.Rows.Count > Lengths(0) + 1 Then
                For rowIndex As Integer = Lengths(0) + 1 To Me.Rows.Count - 1
                    Me.Rows.RemoveAt(rowIndex)
                Next
            End If
            Return Me

        End Function

        ''' <summary>
        ''' Gets the upper bound of the specified dimension
        ''' </summary>
        ''' <param name="Dimension">A zero-based dimension whose upper bound needs to be determined.</param>
        ''' <returns></returns>
        Function GetUpperBound(ByVal Dimension As Integer) As Integer
            Return DimensionLengths(Dimension)
        End Function

        ''' <summary>
        ''' Gets the Lower bound of the specified dimension
        ''' </summary>
        ''' <param name="Dimension">A zero-based dimension whose lower bound needs to be determined.</param>
        ''' <returns></returns>
        Function GetLowerBound(ByVal Dimension As Integer) As Integer
            Return DimensionLowerBounds(Dimension)
        End Function

        ''' <summary>
        ''' Gets the number of elements in the specified dimension
        ''' </summary>
        ''' <param name="Dimension">A zero-based dimension whose length needs to be determined.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetLength(ByVal Dimension As Integer) As Integer
            Return DimensionLengths(Dimension)
        End Function

        ''' <summary>
        ''' Returns the element at the specified row and columns
        ''' </summary>
        ''' <param name="row">Row index where the element is located</param>
        ''' <param name="column">Column index where the element is located</param>
        ''' <value>Value for the specified element</value>
        ''' <returns>The element at the specified index</returns>
        ''' <remarks></remarks>
        Default Public Property Item(ByVal row As Integer, ByVal column As Integer) As Object
            Get
                Return Me.Rows(row - Me.DimensionLowerBounds(0)).Item(column - Me.DimensionLowerBounds(1))
            End Get
            Set(ByVal value As Object)
                Me.Rows(row - Me.DimensionLowerBounds(0)).Item(column - Me.DimensionLowerBounds(1)) = value
            End Set
        End Property

        ''' <summary>
        ''' Get the value at the specified position
        ''' </summary>
        ''' <param name="row">Index row where the element is located</param>
        ''' <param name="column">Index column where the element is located</param>
        ''' <returns>The value at the specified position</returns>
        ''' <remarks></remarks>
        Public Function GetValue(ByVal row As Integer, ByVal column As Integer) As Object
            Return Me.Rows(row - Me.DimensionLowerBounds(0)).Item(column - Me.DimensionLowerBounds(1))
        End Function

        ''' <summary>
        ''' Sets a value to the element at the specified position
        ''' </summary>
        ''' <param name="value">The new value for the specified element</param>
        ''' <param name="row">Index row where the element is located</param>
        ''' <param name="column">Index column where the element is located</param>
        ''' <remarks></remarks>
        Public Sub SetValue(ByVal value As Object, ByVal row As Integer, ByVal column As Integer)
            Me.Rows(row - Me.DimensionLowerBounds(0)).Item(column - Me.DimensionLowerBounds(1)) = value
        End Sub

        ''' <summary>
        ''' Clear a range of elements in the XArray
        ''' </summary>
        ''' <param name="arr">XArray whose elements need to be cleared</param>
        ''' <param name="index">The starting index of the range of elements</param>
        ''' <param name="length">The number of elements to be cleared</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub Clear(ByVal arr As XArray, ByVal index As Integer, ByVal length As Integer)

            Dim realIndexi As Integer = arr.GetLowerBound(0)
            Dim realIndexj As Integer = arr.GetLowerBound(1)

            index = index - arr.GetLowerBound(0)

            While index > 0
                If index > arr.GetUpperBound(1) Then
                    realIndexi = realIndexi + 1
                    index = index - arr.GetLength(1)
                Else
                    realIndexj = realIndexj + index
                    index = 0
                End If
            End While

            For j As Integer = realIndexj To arr.GetUpperBound(1)
                If length < 0 Then Exit Sub
                arr.Item(realIndexi, j) = Nothing
                length = length - 1
            Next

            realIndexi = realIndexi + 1

            For i As Integer = realIndexi To arr.GetUpperBound(0)
                For j As Integer = arr.GetLowerBound(1) To arr.GetUpperBound(1)
                    If length < 1 Then Exit Sub
                    arr.Item(i, j) = Nothing
                    length = length - 1
                Next
            Next
        End Sub

        Public Overloads Sub Clear(ByRef arr As XArray)
            Dim length() As Integer = New Integer() {1, 0}
            Dim lowerB() As Integer = New Integer() {arr.DimensionLowerBounds(0), arr.DimensionLowerBounds(1)}
            Me.Clear()
            arr.RedimXArray(length, lowerB)
        End Sub
        Public Sub AppendRows()
            Dim length() As Integer = New Integer() {Me.DimensionLengths(0) + 1, Me.DimensionLengths(1)}
            Dim lowerB() As Integer = New Integer() {Me.DimensionLowerBounds(0), Me.DimensionLowerBounds(1)}
            Me.RedimXArray(length, lowerB)
        End Sub
        Public Sub AppendRows(ByVal value As Object, ByVal row As Integer, ByVal column As Integer)
            Dim length() As Integer = New Integer() {Me.DimensionLengths(0) + 1, Me.DimensionLengths(1)}
            Dim lowerB() As Integer = New Integer() {Me.DimensionLowerBounds(0), Me.DimensionLowerBounds(1)}
            Me.RedimXArray(length, lowerB)

            Me.Rows(row - Me.DimensionLowerBounds(0)).Item(column - Me.DimensionLowerBounds(1)) = value
        End Sub

        Public Sub DeleteRows(ByVal row As Integer)
            Me.Rows(row - Me.DimensionLowerBounds(0)).Delete()
            Dim length() As Integer = New Integer() {Me.DimensionLengths(0) - 1, Me.DimensionLengths(1)}
            Dim lowerB() As Integer = New Integer() {Me.DimensionLowerBounds(0), Me.DimensionLowerBounds(1)}
            Me.RedimXArray(length, lowerB)
        End Sub

        Public Sub LoadRows(ByVal array(,) As Object)
            Me.RedimXArray(New Integer() {array.GetUpperBound(0), array.GetUpperBound(1)}, New Integer() {array.GetLowerBound(0), array.GetLowerBound(1)})
            For row As Long = array.GetLowerBound(0) To array.GetUpperBound(0)
                For col As Integer = array.GetLowerBound(1) To array.GetUpperBound(1)
                    Me.SetValue(array(CInt(row), col), CInt(row), col)
                Next
            Next
        End Sub

        Public Sub LoadRows(ByVal table As XArray)
            Me.RedimXArray(New Integer() {table.GetUpperBound(0), table.GetUpperBound(1)}, New Integer() {table.GetLowerBound(0), table.GetLowerBound(1)})
            For row As Long = table.GetLowerBound(0) To table.GetUpperBound(0)
                For col As Integer = table.GetLowerBound(1) To table.GetUpperBound(1)
                    Me.SetValue(table.GetValue(CInt(row), col), CInt(row), col)
                Next
            Next
        End Sub

        Public Function Find(ByVal value As Object) As Object
            Dim result As Boolean = False
            For row As Long = Me.GetLowerBound(0) To Me.GetUpperBound(0)
                For col As Integer = Me.GetLowerBound(1) To Me.GetUpperBound(1)
                    If (Me.GetValue(CInt(row), CInt(col)) Is value) Then
                        result = True
                        Exit For
                    End If
                Next
            Next
            Return result
        End Function
        Public Function Find(ByVal value As Object, ByVal lowerBound As Long, ByVal upperBound As Long) As Object
            Dim index As Long = -1
            For row As Long = lowerBound To Me.GetUpperBound(0)
                For col As Long = upperBound To Me.GetUpperBound(1)
                    If (Me.GetValue(CInt(row), CInt(col)) Is value) Then
                        index = row
                        Exit For
                    End If
                Next
            Next
            Return index
        End Function
    End Class
End Namespace