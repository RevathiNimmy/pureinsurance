Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Friend NotInheritable Class CSelectANSI 
	' Class:    Constructs an ANSI SQL select statement
	' Shared:   Private
	' Needs:    Nothing
	'
	' Generic class for constructing a dynamic SQL statement.
	' THIS IS A TEMPORARY COPY OF A CLASS FROM THE SWIFT DATABASE
	' LIBRARY. IT WILL EVENTUALLY BE REPLACED BY A DLL REFERENCE.
	' DO NOT COPY OR MODIFY IN ANY WAY.
	'
	
	Private m_bDistinct As Boolean = False
	Private m_sFields As String = ""
	Private m_sTables As String = ""
	Private m_sFilters As String = ""
	Private m_sGroups As String = ""
	Private m_sSorts As String = ""
	
	
	Public Property Distinct() As Boolean
		Get
			
			Return m_bDistinct
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bDistinct = Value
			
		End Set
	End Property
	
	' Attributes: Default
	Public ReadOnly Property SQL() As String
		Get
			
			
			' NB: This code assumes that all clauses of the SELECT
			' statement are optional. This allows you to use the
			' class to build up an UPDATE statement's FROM and
			' WHERE clauses without including the SELECT field list.
			Dim sSQL As String = ""
			
			If m_sFields <> "" Then
				If sSQL <> "" Then sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
				If m_bDistinct Then
					sSQL = sSQL & "select distinct " & m_sFields
				Else
                    sSQL = sSQL & "select distinct TOP 500 " & m_sFields
				End If
			End If
			
			If m_sTables <> "" Then
				If sSQL <> "" Then sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
				sSQL = sSQL & "from " & m_sTables
			End If
			
			If m_sFilters <> "" Then
				If sSQL <> "" Then sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
				sSQL = sSQL & "where " & m_sFilters
			End If
			
			If m_sGroups <> "" Then
				If sSQL <> "" Then sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
				sSQL = sSQL & "group by " & m_sGroups
			End If
			
			If m_sSorts <> "" Then
				If sSQL <> "" Then sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
				sSQL = sSQL & "order by " & m_sSorts
			End If
			
			Return sSQL
			
		End Get
	End Property
	
	Public Sub AddField(ByVal sField As String)
		
		If m_sFields <> "" Then m_sFields = m_sFields & "," & Strings.ChrW(13) & Strings.ChrW(10)
		m_sFields = m_sFields & sField
		
	End Sub
	
	Public Sub AddTable(ByVal sTable As String, Optional ByVal sJoinKeyword As String = "", Optional ByVal sOnClause As String = "", Optional ByVal bWithNolock As Boolean = False)
		
		If bWithNolock Then
			sTable = sTable & " WITH(NOLOCK)"
		End If
		
		If sJoinKeyword = "" And sOnClause = "" Then
			m_sTables = sTable
		Else
			m_sTables = m_sTables & Strings.ChrW(13) & Strings.ChrW(10) & sJoinKeyword & " join " & sTable & " on " & sOnClause
		End If
		
	End Sub
	
	Public Sub AddFilter(ByVal sFilter As String)
		
		If m_sFilters <> "" Then m_sFilters = m_sFilters & Strings.ChrW(13) & Strings.ChrW(10) & "and "
		m_sFilters = m_sFilters & sFilter
		
	End Sub
	
	Public Sub AddGroup(ByVal sGroup As String)
		
		If m_sGroups <> "" Then m_sGroups = m_sGroups & ", "
		m_sGroups = m_sGroups & sGroup
		
	End Sub
	
	Public Sub AddSort(ByVal sSort As String)
		
		If m_sSorts <> "" Then m_sSorts = m_sSorts & ", "
		m_sSorts = m_sSorts & sSort
		
	End Sub
	
	Public Sub New()
		MyBase.New()
	End Sub
End Class
