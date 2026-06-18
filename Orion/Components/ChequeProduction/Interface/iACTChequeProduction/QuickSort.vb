Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("QuickSort_NET.QuickSort")> _
Public NotInheritable Class QuickSort 
	
	Private Const PMTrue As Integer = 1
	Private Const PMFalse As Integer = 0
	
	Private m_lReturn As Integer
    Private m_vSortArray As Object
	Private m_bIsDecending As Boolean
	Private m_vSortColumn() As Object
	Private m_lSortColumn As Integer
	Private m_lNoOfColumns As Integer
	Private m_lNoOfRows As Integer
	Private m_lColumnsMinusOne As Integer
	
	
	Public Property SortArray() As Object
		Get
			Return m_vSortArray
		End Get
		Set(ByVal Value As Object)


			m_vSortArray = Value
		End Set
	End Property
	
	Public Property IsDecending() As Boolean
		Get
			Return m_bIsDecending
		End Get
		Set(ByVal Value As Boolean)
			m_bIsDecending = Value
		End Set
	End Property
	Public WriteOnly Property SortColumn() As Object()
		Set(ByVal Value() As Object)
			m_vSortColumn = Value
		End Set
	End Property
	Public Sub QuickSort()
		
		Dim lLeft, lRight As Integer
		
		If Not Information.IsArray(m_vSortArray) Then
			' Nothing to sort
			Exit Sub
		End If
		
		m_lReturn = CheckArrayBounds()
		If m_lReturn = PMTrue Then
			
			' Two dimentional array

			lLeft = m_vSortArray.GetLowerBound(1)

			lRight = m_vSortArray.GetUpperBound(1)
			
			' Make sure we have at least one sort column
			If Not Information.IsArray(m_vSortColumn) Then
				ReDim m_vSortColumn(0)
				m_vSortColumn(0) = 0
			End If
			

			m_lNoOfColumns = m_vSortArray.GetUpperBound(0) + 1
			m_lColumnsMinusOne = m_lNoOfColumns - 1
			
			' Number of rows

			m_lNoOfRows = m_vSortArray.GetUpperBound(1)
			
			'Do the sort
			QuickSortMulti(lLeft, lRight)
		Else
			' Single dimention array
			

			lLeft = m_vSortArray.GetLowerBound(0)

			lRight = m_vSortArray.GetUpperBound(0)
			
			QuickSortSingle(lLeft, lRight)
		End If
		
	End Sub
	Private Function CheckArrayBounds() As Integer
		
		Dim result As Integer = 0
		 
			
			result = PMTrue
			
			Dim lDummy As Integer
			

			lDummy = m_vSortArray.GetUpperBound(1)
			
			Return result
		
	End Function
	
	Private Sub QuickSortSingle(ByVal left As Integer, ByVal right As Integer)
		
		Dim pivot As String = ""
		Dim p As Integer
		
		If (FindPivot(left, right, pivot)) = System.Windows.Forms.DialogResult.Yes Then
			p = Partition(left, right, pivot)
			QuickSortSingle(left, p - 1)
			QuickSortSingle(p, right)
		End If
		
	End Sub
	Private Function Partition(ByVal left As Integer, ByVal right As Integer, ByRef pivot As String) As Integer
		
		While left <= right
			
			If m_bIsDecending Then
				'Sorting in decending order

                While CStr(m_vSortArray(left)) >= pivot
                    left += 1
                End While


                While CStr(m_vSortArray(right)) < pivot
                    right -= 1
                End While
            Else
                'Sorting in acending order

                While CStr(m_vSortArray(left)) < pivot
                    left += 1
                End While


                While CStr(m_vSortArray(right)) >= pivot
                    right -= 1
                End While
            End If

            If left < right Then
                ' Swap the two values


                Swap(CStr(m_vSortArray(left)), CStr(m_vSortArray(right)))
                left += 1
                right -= 1
            End If
        End While

        Return left

    End Function
    Private Function FindPivot(ByVal left As Integer, ByVal right As Integer, ByRef pivot As String) As Integer



        Dim Middle As Integer = IIf(left + ((right - left) / 2) > 0, Math.Floor(left + ((right - left) / 2)), Math.Ceiling(left + ((right - left) / 2)))


        Dim a As String = CStr(m_vSortArray(left)) ' Left value

        Dim b As String = CStr(m_vSortArray(Middle)) ' middle value

        Dim c As String = CStr(m_vSortArray(right)) ' right value

        o3(a, b, c) ' order these three values

        If a < b Then ' pivot will be higher of two values
            pivot = b
            Return System.Windows.Forms.DialogResult.Yes
        End If

        If b < c Then
            pivot = c
            Return System.Windows.Forms.DialogResult.Yes
        End If

        Dim p As Integer = left + 1
        While p <= right



            If Not m_vSortArray(p).Equals(m_vSortArray(left)) Then



                If m_vSortArray(p) < m_vSortArray(left) Then

                    pivot = CStr(m_vSortArray(left))
                Else

                    pivot = CStr(m_vSortArray(p))
                End If
                Return System.Windows.Forms.DialogResult.Yes
            End If

            p += 1
        End While

        Return System.Windows.Forms.DialogResult.No ' all elements have the same value

    End Function

    Private Sub QuickSortMulti(ByVal left As Integer, ByVal right As Integer)



        Dim vStoreValue As String = ""
        Dim lSubLeft, lCnt, lPreviousSortColumn As Integer

        For lSortIndex As Integer = 0 To m_vSortColumn.GetUpperBound(0)

            ' Set the module level sort column
            m_lSortColumn = CInt(m_vSortColumn(lSortIndex))

            If lSortIndex > 0 Then

                ' Get the previous sort level
                lPreviousSortColumn = CInt(m_vSortColumn(lSortIndex - 1))


                vStoreValue = CStr(m_vSortArray(lPreviousSortColumn, 0))
                lSubLeft = 0

                For lCnt = 0 To m_lNoOfRows

                    If CStr(m_vSortArray(lPreviousSortColumn, lCnt)) <> vStoreValue Then
                        If lSubLeft <> lCnt - 1 Then
                            QuickSortMultiSub(lSubLeft, lCnt - 1)
                        End If
                        lSubLeft = lCnt

                        vStoreValue = CStr(m_vSortArray(lPreviousSortColumn, lCnt))
                    End If
                Next lCnt

                If lSubLeft <> lCnt - 1 Then
                    QuickSortMultiSub(lSubLeft, lCnt - 1)
                End If

            Else

                QuickSortMultiSub(left, right)
            End If

        Next lSortIndex

    End Sub
    Private Sub QuickSortMultiSub(ByVal left As Integer, ByVal right As Integer)

        Dim pivot As String = ""
        Dim p As Integer

        If (FindPivotMulti(left, right, pivot)) = System.Windows.Forms.DialogResult.Yes Then
            p = PartitionMulti(left, right, pivot)
            QuickSortMultiSub(left, p - 1)
            QuickSortMultiSub(p, right)
        End If

    End Sub
    Private Function FindPivotMulti(ByVal left As Integer, ByVal right As Integer, ByRef pivot As String) As Integer



        Dim Middle As Integer = IIf(left + ((right - left) / 2) > 0, Math.Floor(left + ((right - left) / 2)), Math.Ceiling(left + ((right - left) / 2)))


        Dim a As String = CStr(m_vSortArray(m_lSortColumn, left)) ' Left value

        Dim b As String = CStr(m_vSortArray(m_lSortColumn, Middle)) ' middle value

        Dim c As String = CStr(m_vSortArray(m_lSortColumn, right)) ' right value

        o3(a, b, c) ' order these three values

        If a < b Then ' pivot will be higher of two values
            pivot = b
            Return System.Windows.Forms.DialogResult.Yes
        End If

        If b < c Then
            pivot = c
            Return System.Windows.Forms.DialogResult.Yes
        End If

        Dim p As Integer = left + 1
        While p <= right



            If Not m_vSortArray(m_lSortColumn, p).Equals(m_vSortArray(m_lSortColumn, left)) Then



                If m_vSortArray(m_lSortColumn, p) < m_vSortArray(m_lSortColumn, left) Then

                    pivot = CStr(m_vSortArray(m_lSortColumn, left))
                Else

                    pivot = CStr(m_vSortArray(m_lSortColumn, p))
                End If
                Return System.Windows.Forms.DialogResult.Yes
            End If

            p += 1
        End While

        Return System.Windows.Forms.DialogResult.No ' all elements have the same value

    End Function
    Private Function PartitionMulti(ByVal left As Integer, ByVal right As Integer, ByRef pivot As String) As Integer

        While left <= right

            If m_bIsDecending Then
                'Sorting in decending order

                While CStr(m_vSortArray(m_lSortColumn, left)) >= pivot
                    left += 1
                End While


                While CStr(m_vSortArray(m_lSortColumn, right)) < pivot
                    right -= 1
                End While
            Else
                'Sorting in acending order

                While CStr(m_vSortArray(m_lSortColumn, left)) < pivot
                    left += 1
                End While


                While CStr(m_vSortArray(m_lSortColumn, right)) >= pivot
                    right -= 1
                End While
            End If

            If left < right Then
                ' Swap the two values
                For lCnt As Integer = 0 To m_lColumnsMinusOne


                    Swap(CStr(m_vSortArray(lCnt, left)), CStr(m_vSortArray(lCnt, right)))
                Next lCnt

                left += 1
                right -= 1
            End If
        End While

        Return left

    End Function
	
	Private Sub o3(ByRef x As String, ByRef y As String, ByRef z As String)
		
		Order(x, y)
		Order(x, z)
		Order(y, z)
		
	End Sub
	
	Private Sub Order(ByRef x As String, ByRef y As String)
		
		If x > y Then
			Swap(x, y)
		End If
		
	End Sub
	
	Private Sub Swap(ByRef x As String, ByRef y As String)
		
		
		Dim Temp As String = x
		x = y
		y = Temp
		
	End Sub
End Class

