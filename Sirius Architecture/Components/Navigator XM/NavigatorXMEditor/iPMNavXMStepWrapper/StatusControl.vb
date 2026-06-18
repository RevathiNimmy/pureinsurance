Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports SharedFiles
Imports System
<System.Runtime.InteropServices.ProgId("StatusControl_NET.StatusControl")> _
Public NotInheritable Class StatusControl 
	' #############################################################################
	' Class: StatusControl
	'
	' Description: This class used by scripts for Navigator control (ok/cancel),
	'              and access to key array.
	'              Keys passed in by Navigator are read-only.
	'              In script, class is accessed as object 'ScriptControl'.
	' #############################################################################
	
	
	' status can be modified by user script
	Private m_lStatus As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMCancel
	
	' lower limit of the key array
	Private m_lNavKeyLimitLow As Integer
	' upper limit of the Navigator keys. User keys will be above this.
	Private m_lNavKeyLimitHigh As Integer
	
	Private m_vKeyArray As Array
	
	Private Const KEY_NAME As Integer = 0
	Private Const KEY_VALUE As Integer = 1
	
	
	
	' checked by UserComponent.Start when script ends
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	
	Public Property KeyArray() As Object
		Get
			Return m_vKeyArray
		End Get
		Set(ByVal Value As Object)
			m_vKeyArray = Value
			' set the upper limit of array passed in by script control
			m_lNavKeyLimitLow = m_vKeyArray.GetLowerBound(1)
			m_lNavKeyLimitHigh = m_vKeyArray.GetUpperBound(1)
		End Set
	End Property
	
	' used to set status to Ok
	' eg. ScriptControl.Status = ScriptControl.Ok
	Public ReadOnly Property Ok() As Integer
		Get
			Return gPMConstants.PMEReturnCode.PMOk
		End Get
	End Property
	
	' used to set status to cancel
	' eg. ScriptControl.Status = ScriptControl.Cancel
	Public ReadOnly Property Cancel() As Integer
		Get
			Return gPMConstants.PMEReturnCode.PMCancel
		End Get
	End Property
	
	' get value of key in key array on key name
	Public Function GetKeyValue(ByVal sKey As String, ByRef vValue As String) As Boolean
		
		Dim result As Boolean = False
		
		
		If sKey.Trim() = "" Then
			' key name is empty
			Return result
		End If
		
		For lLoop As Integer = m_lNavKeyLimitLow To m_vKeyArray.GetUpperBound(1)
            If sKey.ToUpper() = CStr(m_vKeyArray(KEY_NAME, lLoop)).ToUpper() Then
                result = True
                vValue = CStr(m_vKeyArray(KEY_VALUE, lLoop))
                Exit For
            End If
        Next

        Return result
    End Function

    ' set value of key in key array on key name
    ' only keys added by user can be modified
    Public Function SetKeyValue(ByVal sKey As String, ByVal vValue As String) As Boolean

        Dim result As Boolean = False


        If sKey.Trim() = "" Then
            ' key name is empty
            Return result
        End If

        For lLoop As Integer = m_lNavKeyLimitLow To m_vKeyArray.GetUpperBound(1)
            If sKey.ToUpper() = CStr(m_vKeyArray(KEY_NAME, lLoop)).ToUpper() And lLoop > m_lNavKeyLimitHigh Then
                ' key found and is a user key
                result = True
                m_vKeyArray(KEY_VALUE, lLoop) = vValue
                Exit For
            End If
        Next

        Return result
    End Function

    ' add a new key and value to the key array
    Public Function AddKey(ByVal sKey As String, ByVal vValue As String) As Boolean

        Dim result As Boolean = False
        Dim lPos As Integer

        Try


            If sKey.Trim() = "" Then
                ' key name is empty
                Return result
            End If

            For lPos = m_lNavKeyLimitLow To m_vKeyArray.GetUpperBound(1)
                If CStr(m_vKeyArray(KEY_NAME, lPos)).ToUpper() = sKey.ToUpper() Then
                    ' key already exists, duplicates not allowed
                    Return result
                End If
            Next

            lPos = m_vKeyArray.GetUpperBound(1) + 1

            m_vKeyArray = ArraysHelper.RedimPreserve(Of Object(,))(m_vKeyArray, New Integer() {m_vKeyArray.GetUpperBound(0) - m_vKeyArray.GetLowerBound(0) + 1, lPos - m_lNavKeyLimitLow + 1}, New Integer() {m_vKeyArray.GetLowerBound(0), m_lNavKeyLimitLow})

            m_vKeyArray(KEY_NAME, lPos) = sKey
            m_vKeyArray(KEY_VALUE, lPos) = vValue


            Return True

        Catch



            Return False
        End Try

    End Function

    ' delete a key in key array on key name
    ' only keys added by user can be deleted
    Public Function DeleteKey(ByVal sKey As String) As Boolean

        Dim result As Boolean = False
        Dim bFound As Boolean
        Dim lLoop As Integer

        Try


            If sKey.Trim() = "" Then
                ' key name is empty
                Return result
            End If

            For lLoop = m_lNavKeyLimitLow To m_vKeyArray.GetUpperBound(1)

                If CStr(m_vKeyArray(KEY_NAME, lLoop)).ToUpper() = sKey.ToUpper() And lLoop > m_lNavKeyLimitHigh Then
                    ' user key found
                    bFound = True
                End If

                If bFound And lLoop < m_vKeyArray.GetUpperBound(1) Then
                    ' move keys down the array to delete specified key
                    m_vKeyArray(KEY_NAME, lLoop) = m_vKeyArray(KEY_NAME, lLoop + 1)
                    m_vKeyArray(KEY_VALUE, lLoop) = m_vKeyArray(KEY_VALUE, lLoop + 1)
                End If

            Next

            lLoop = m_vKeyArray.GetUpperBound(1) - 1

            If bFound Then
                ' resize the array
                m_vKeyArray = ArraysHelper.RedimPreserve(Of Object(,))(m_vKeyArray, New Integer() {m_vKeyArray.GetUpperBound(0) - m_vKeyArray.GetLowerBound(0) + 1, lLoop - m_lNavKeyLimitLow + 1}, New Integer() {m_vKeyArray.GetLowerBound(0), m_lNavKeyLimitLow})
            End If


            Return bFound

        Catch



            Return False
        End Try

    End Function
	
	' remove all user keys from the key array
	Public Function DeleteAllUserKeys() As Boolean
		
		Dim result As Boolean = False
		Try 
			
			
			m_vKeyArray = ArraysHelper.RedimPreserve(Of Object(, ))(m_vKeyArray, New Integer(){m_vKeyArray.GetUpperBound(0) - m_vKeyArray.GetLowerBound(0) + 1, m_lNavKeyLimitHigh - m_lNavKeyLimitLow + 1}, New Integer(){m_vKeyArray.GetLowerBound(0), m_lNavKeyLimitLow})
			
			
			Return True
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
End Class
