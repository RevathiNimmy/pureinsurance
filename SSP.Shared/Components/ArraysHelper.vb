Imports System.Reflection

Public Class ArraysHelper
    Private Class InitialValueProvider
        Private Enum InitialValueMethod
            [String]
            Constructor
            ValueType
            CreateInstanceValueType
        End Enum

        Private ReadOnly elementType As Type

        Private constructorParams As Object()

        Private initialized As Boolean

        Private initializeMethod As InitialValueMethod = InitialValueMethod.String

        Private constructor As ConstructorInfo

        Private method As MethodInfo

        Public Sub New(elementType As Type, constructorParams As Object())
            Me.elementType = elementType
            Me.constructorParams = constructorParams
            initialized = False
        End Sub

        Public Function GetInitialValue() As Object
            Initialize()
            Return initializeMethod
        End Function

        Private Sub Initialize()
            If initialized Then
                Return
            End If

            initialized = True
            If elementType.Equals(GetType(String)) Then
                Return
            End If

            If constructorParams Is Nothing Then
                constructorParams = New Object(-1) {}
            End If

            If CObj(elementType.GetConstructor(Type.GetTypeArray(constructorParams))) Is Nothing Then
                If elementType.IsValueType AndAlso (constructorParams Is Nothing OrElse constructorParams.Length = 0) Then
                    initializeMethod = If(CObj(elementType.GetMethod("CreateInstance")) Is Nothing, InitialValueMethod.ValueType, InitialValueMethod.CreateInstanceValueType)
                End If
            Else
                initializeMethod = InitialValueMethod.Constructor
            End If
        End Sub

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class

    Public Shared Function InitializeArray(Of E)(length As Integer) As E()
        Return InitializeArray(Of E())(New Integer(0) {length})
    End Function

    Public Shared Function InitializeArray(Of E)(length As Integer, lowerBound As Integer) As E()
        Return InitializeArray(Of E())(New Integer(0) {length}, New Integer(0) {lowerBound})
    End Function

    Public Shared Function InitializeArray(Of E)(length As Integer, constructorParams As Object()) As E()
        Return InitializeArray(Of E())(New Integer(0) {length}, constructorParams)
    End Function

    Public Shared Function InitializeArray(Of E)(length As Integer, lowerBound As Integer, constructorParams As Object()) As E()
        Return InitializeArray(Of E())(New Integer(0) {length}, New Integer(0) {lowerBound}, constructorParams)
    End Function

    Public Shared Function InitializeArray(Of E)(length As Integer, initValue As Object) As E()
        Return InitializeArray(Of E())(New Integer(0) {length}, initValue)
    End Function

    Public Shared Function InitializeArray(Of E)(length As Integer, lowerBound As Integer, initValue As Object) As E()
        Return InitializeArray(Of E())(New Integer(0) {length}, New Integer(0) {lowerBound}, initValue)
    End Function

    Public Shared Function InitializeArray(Of A As Class)(lengths As Integer()) As A
        Return InitializeArray(Of A)(lengths, New Object(-1) {})
    End Function

    Public Shared Function InitializeArray(Of A As Class)(lengths As Integer(), lowerBounds As Integer()) As A
        Return InitializeArray(Of A)(lengths, lowerBounds, New Object(-1) {})
    End Function

    Public Shared Function InitializeArray(Of A As Class)(lengths As Integer(), constructorParams As Object()) As A
        If lengths Is Nothing OrElse constructorParams Is Nothing Then
            Throw New NullReferenceException("AIS-Exception. Either 'lengths' or 'constructorParams' parameter is null")
        End If

        Return InitializeArray(Of A)(lengths, New Integer(lengths.Length - 1) {}, constructorParams)
    End Function

    Public Shared Function InitializeArray(Of A As Class)(lengths As Integer(), lowerBounds As Integer(), constructorParams As Object()) As A
        If lengths Is Nothing OrElse lowerBounds Is Nothing OrElse constructorParams Is Nothing Then
            Throw New NullReferenceException("AIS-Exception. Either 'lengths', 'lowerBounds' or 'constructorParams' parameter is null")
        End If

        Dim typeFromHandle = GetType(A)
        If Not typeFromHandle.IsArray Then
            Throw New Exception("AIS-Exception. Array type is expected as parameter")
        End If

        Dim elementType As Type = typeFromHandle.GetElementType()
        If elementType Is Nothing Then
            Throw New NullReferenceException("AIS-Exception. itemType for the array couldn't be resolved")
        End If

        Dim initialValueProvider As InitialValueProvider = New InitialValueProvider(elementType, constructorParams)
        If elementType.IsPrimitive OrElse elementType.Equals(GetType(String)) Then
            Return TryCast(InternalInitializeArray(lengths, lowerBounds, elementType, initialValueProvider.GetInitialValue()), A)
        End If

        Return TryCast(InternalInitializeArray(lengths, lowerBounds, elementType, initialValueProvider), A)
    End Function

    Public Shared Function InitializeArray(Of A As Class)(lengths As Integer(), initValue As Object) As A
        If lengths Is Nothing Then
            Throw New NullReferenceException("AIS-Exception. 'lengths' parameter is null")
        End If

        Return InitializeArray(Of A)(lengths, New Integer(lengths.Length - 1) {}, initValue)
    End Function

    Public Shared Function InitializeArray(Of A As Class)(lengths As Integer(), lowerBounds As Integer(), initValue As Object) As A
        If lengths Is Nothing OrElse lowerBounds Is Nothing Then
            Throw New NullReferenceException("AIS-Exception. Either 'lengths', 'lowerBounds' parameter is null")
        End If

        If lengths.Length <> lowerBounds.Length Then
            Throw New Exception("AIS-Exception. The length of 'lengths' and 'lowerBounds' parameters is different")
        End If

        Dim typeFromHandle = GetType(A)
        If Not typeFromHandle.IsArray Then
            Throw New Exception("AIS-Exception. Array type is expected as parameter")
        End If

        Dim elementType As Type = typeFromHandle.GetElementType()
        If elementType Is Nothing Then
            Throw New NullReferenceException("AIS-Exception. itemType for the array couldn't be resolved")
        End If

        Return TryCast(InternalInitializeArray(lengths, lowerBounds, elementType, initValue), A)
    End Function

    Private Shared Function InternalInitializeArray(lengths As Integer(), lowerBounds As Integer(), itemType As Type, value As Object) As Array
        Dim array1 = Array.CreateInstance(itemType, lengths, lowerBounds)
        Dim array2 = New Integer(lowerBounds.Length - 1) {}
        For i = 0 To array1.Rank - 1
            array2(i) = array1.GetUpperBound(i)
        Next

        Dim indexes = New Integer(lengths.Length - 1) {}
        Array.Copy(lowerBounds, indexes, lowerBounds.Length)
        Dim num = array1.Rank - 1
        indexes(num) -= 1
        num = CalculateIndexes(indexes, num, lowerBounds, array2)
        If num >= 0 Then
            Dim value2 = array1.GetValue(indexes)
            Dim obj As Object = (If((TypeOf value Is InitialValueProvider), CType(value, InitialValueProvider).GetInitialValue(), value))
            If (value2 IsNot Nothing OrElse obj IsNot Nothing) AndAlso Not If(value2?.Equals(obj), False) Then
                While num >= 0
                    array1.SetValue(If((TypeOf value Is InitialValueProvider), CType(value, InitialValueProvider).GetInitialValue(), value), indexes)
                    num = CalculateIndexes(indexes, num, lowerBounds, array2)
                End While
            End If
        End If

        Return array1
    End Function

    Public Shared Function RedimPreserve(Of A As Class)(arraySource As A, lengths As Integer()) As A
        If lengths Is Nothing Then
            Throw New NullReferenceException("AIS-Exception. 'lengths' parameter is null")
        End If

        Return RedimPreserve(arraySource, lengths, New Integer(lengths.Length - 1) {})
    End Function

    Public Shared Function RedimPreserve(Of A As Class)(arraySource As A, lengths As Integer(), lowerBounds As Integer()) As A
        Dim typeFromHandle = GetType(A)
        If arraySource Is Nothing Then
            Return InitializeArray(Of A)(lengths, lowerBounds)
        End If

        RunRedimPreserveVerifications(arraySource, typeFromHandle, lengths, lowerBounds)
        Dim array As Array = TryCast(arraySource, Array)
        Dim array2 As Array
        If array IsNot Nothing Then
            array2 = Array.CreateInstance(typeFromHandle.GetElementType(), lengths, lowerBounds)
            Dim valueProvider As InitialValueProvider = New InitialValueProvider(typeFromHandle.GetElementType(), Nothing)
            If array.Rank > 1 Then
                FillsMultiDimensionalArray(array, array2, valueProvider)
            Else
                FillsOneDimensionArray(array, array2, valueProvider)
            End If
        Else
            array2 = TryCast(InitializeArray(Of A)(lengths, lowerBounds), Array)
        End If

        Return TryCast(array2, A)
    End Function

    Private Shared Sub FillsOneDimensionArray(sourceArray As Array, targetArray As Array, valueProvider As InitialValueProvider)
        Array.Copy(sourceArray, sourceArray.GetLowerBound(0), targetArray, sourceArray.GetLowerBound(0), Math.Min(targetArray.GetLength(0), sourceArray.GetLength(0)))
        If targetArray.Length > sourceArray.Length Then
            For i = sourceArray.Length To targetArray.Length - 1
                targetArray.SetValue(valueProvider.GetInitialValue(), i + targetArray.GetLowerBound(0))
            Next
        End If
    End Sub

    Private Shared Sub FillsMultiDimensionalArray(sourceArray As Array, targetArray As Array, valueProvider As InitialValueProvider)
        Dim firstDimensionsSize = GetFirstDimensionsSize(sourceArray)
        Dim lastDimensionSize = GetLastDimensionSize(sourceArray)
        Dim lastDimensionSize2 = GetLastDimensionSize(targetArray)
        Dim num = lastDimensionSize2 - lastDimensionSize
        Dim lowerBound = sourceArray.GetLowerBound(0)
        Dim array1 = Array.CreateInstance(GetType(Integer), targetArray.Rank)
        For i = 0 To targetArray.Rank - 1 - 1
            array1.SetValue(1, i)
        Next

        array1.SetValue(targetArray.GetLength(targetArray.Rank - 1), targetArray.Rank - 1)
        Dim array2 As Array = Array.CreateInstance(targetArray.GetType().GetElementType(), CType(array1, Integer()))
        For i = 0 To firstDimensionsSize - 1
            array.Copy(sourceArray, i * lastDimensionSize + lowerBound, targetArray, i * lastDimensionSize2 + lowerBound, Math.Min(lastDimensionSize, lastDimensionSize2))
            If num > 0 Then
                For j = 0 To array1.Length - 1
                    array1.SetValue(0, j)
                Next

                For k = lastDimensionSize To lastDimensionSize2 - 1
                    array1.SetValue(k - lastDimensionSize, array1.Length - 1)
                    array2.SetValue(valueProvider.GetInitialValue(), CType(array1, Integer()))
                Next

                array.Copy(array2, 0, targetArray, i * lastDimensionSize2 + lastDimensionSize + lowerBound, num)
            End If
        Next
    End Sub

    Public Shared Function CastArray(Of A As Class)(srcArray As Array) As A
        Dim array4 As Array
        Try
            If srcArray Is Nothing Then
                Return Nothing
            End If

            Dim typeFromHandle = GetType(A)
            If Not typeFromHandle.IsArray Then
                Throw New Exception("AIS-Exception. Array type is expected as parameter")
            End If

            Dim elementType As Type = typeFromHandle.GetElementType()
            If elementType Is Nothing Then
                Throw New NullReferenceException("AIS-Exception. itemType for the array couldn't be resolved")
            End If

            Dim array = New Integer(srcArray.Rank - 1) {}
            Dim array2 = New Integer(srcArray.Rank - 1) {}
            Dim array3 = New Integer(srcArray.Rank - 1) {}
            For i = 0 To srcArray.Rank - 1
                array(i) = srcArray.GetLength(i)
                array2(i) = srcArray.GetLowerBound(i)
                array3(i) = srcArray.GetUpperBound(i)
            Next

            array4 = System.Array.CreateInstance(elementType, array, array2)
            Dim indexes = New Integer(array.Length - 1) {}
            System.Array.Copy(array2, indexes, array2.Length)
            Dim num = array4.Rank - 1
            indexes(num) -= 1
            num = CalculateIndexes(indexes, num, array2, array3)

            While num >= 0
                array4.SetValue(Convert.ChangeType(srcArray.GetValue(indexes), elementType), indexes)
                num = CalculateIndexes(indexes, num, array2, array3)
            End While
        Catch __unusedException1__ As Exception
            Throw New Exception("AIS-Exception. Array casting is generating an exception")
        End Try

        Return TryCast(array4, A)
    End Function

    Private Shared Function CalculateIndexes(ByRef indexes As Integer(), pos As Integer, lBounds As Integer(), UBounds As Integer()) As Integer
        indexes(pos) += 1
        If indexes(pos) > UBounds(pos) Then
            indexes(pos) = lBounds(pos)
            pos -= 1
            If pos >= 0 Then
                pos = CalculateIndexes(indexes, pos, lBounds, UBounds)
                If pos >= 0 Then
                    pos += 1
                End If
            End If
        End If

        Return pos
    End Function

    Private Shared Sub RunRedimPreserveVerifications(arrayPrototype As Object, arrayType As Type, lengths As Integer(), lowerBounds As Integer())
        If Not arrayType.IsArray Then
            Throw New Exception("AIS-Exception. Array type is expected as parameter")
        End If

        Dim elementType As Type = arrayType.GetElementType()
        If elementType Is Nothing Then
            Throw New NullReferenceException("AIS-Exception. itemType for the array couldn't be resolved")
        End If

        If lengths Is Nothing OrElse lowerBounds Is Nothing Then
            Throw New NullReferenceException("AIS-Exception. Either 'lengths' or 'lowerBounds' parameter is null")
        End If

        If lengths.Length <> lowerBounds.Length Then
            Throw New Exception("AIS-Exception. The length of 'lengths' and 'lowerBounds' parameters is different")
        End If

        If arrayPrototype IsNot Nothing AndAlso arrayType.GetArrayRank() <> lengths.Length Then
            Throw New Exception("AIS-Exception. Can't change the number of dimensions of the current array")
        End If

        Dim array = CType(arrayPrototype, Array)
        For i = 0 To lengths.Length - 1 - 1
            If array.GetLength(i) <> lengths(i) Then
                Throw New Exception("AIS-Exception.  Only last dimension can be modified.")
            End If

            If array.GetLowerBound(i) <> lowerBounds(i) Then
                Throw New Exception("AIS-Exception.  Only last dimension can be modified.")
            End If
        Next
    End Sub

    Private Shared Function GetFirstDimensionsSize(array As Array) As Integer
        Dim num = 1
        For i = 0 To array.Rank - 1 - 1
            num *= array.GetLength(i)
        Next

        Return num
    End Function

    Private Shared Function GetLastDimensionSize(array As Array) As Integer
        Return array.GetLength(array.Rank - 1)
    End Function
End Class
