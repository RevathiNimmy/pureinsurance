Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Module modFilterEvents
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	Public Const ALL_ITEMS As String = "[All]"
	
	Public Function FilterRecord(ByRef xi_typRecord As cEventLogs.EventRecord, ByRef xi_typUserFilterData As cEventLogs.FilterData) As Boolean
		
        Dim p_lngEventID As Integer
		Dim p_blnInclude As Boolean
		Dim p_strThisEventSource, p_strThisEventCategory As String
		Dim p_dtRecordDate As Date
		
		' Assume we are going to show this record
		Dim p_blnShowRec As Boolean = True
		
		' Check for Event Types
		Dim p_strEventType As String = xi_typRecord.EventType
		p_blnShowRec = CheckEventTypes(xi_strEventType:=p_strEventType, xi_typUserFilterData:=xi_typUserFilterData)
		
		' Check for EventID
		If p_blnShowRec Then
			p_lngEventID = xi_typRecord.EventID
			p_blnShowRec = CheckEventID(xi_lngEventID:=p_lngEventID, xi_typUserFilterData:=xi_typUserFilterData)
		End If
		
		' Check for Event Dates
		If p_blnShowRec Then
			p_dtRecordDate = CDate(DateTime.Parse(xi_typRecord.EventTimeCreated).ToString("d"))
			
			' Only check if the 'Filter By Date' checkbox is checked!
			If xi_typUserFilterData.CategoryInclude Then
				p_blnShowRec = CheckEventDate(xi_dtRecordDate:=p_dtRecordDate, xi_typUserFilterData:=xi_typUserFilterData)
			End If
		End If
		
		' Check for Event Source
		If p_blnShowRec Then
			p_strThisEventSource = xi_typRecord.EventSourceName
			p_blnInclude = xi_typUserFilterData.SourceInclude
			
			' Get the name, and whether to include or exclude
			p_blnShowRec = GetSourcesMatch(xi_strThisEventSource:=p_strThisEventSource, xi_blnInclude:=p_blnInclude, xi_typUserFilterData:=xi_typUserFilterData)
		End If
		
		' Check for Event Category
		If p_blnShowRec Then
			p_strThisEventCategory = xi_typRecord.EventCategoryString
			p_blnInclude = xi_typUserFilterData.CategoryInclude
			
			' Get the name, and whether to include or exclude
			p_blnShowRec = GetCategoriesMatch(xi_strThisEventCategory:=p_strThisEventCategory, xi_blnInclude:=p_blnInclude, xi_typUserFilterData:=xi_typUserFilterData)
			
		End If
		
		' Set the return value
		Return p_blnShowRec
		
	End Function
	
	Private Function CheckEventDate(ByVal xi_dtRecordDate As Date, ByRef xi_typUserFilterData As cEventLogs.FilterData) As Boolean
		
		
		' Default to showing record
		Dim p_blnShowRec As Boolean = True
		
		If xi_dtRecordDate < xi_typUserFilterData.EventDateFrom Then
			p_blnShowRec = False
		ElseIf (xi_dtRecordDate > xi_typUserFilterData.EventDateTo) Then 
			p_blnShowRec = False
		Else
			p_blnShowRec = True
		End If
		
		Return p_blnShowRec
		
	End Function
	
	Private Function CheckEventID(ByVal xi_lngEventID As Integer, ByRef xi_typUserFilterData As cEventLogs.FilterData) As Boolean
		
		
		' Default to showing record
		Dim p_blnShowRec As Boolean = True
		
		If xi_typUserFilterData.EventID <> -1 Then
			
			If xi_typUserFilterData.EventIdInclude Then
				
				If xi_lngEventID <> xi_typUserFilterData.EventID Then
					p_blnShowRec = False
				End If
				
			ElseIf Not xi_typUserFilterData.EventIdInclude Then 
				
				If xi_lngEventID = xi_typUserFilterData.EventID Then
					p_blnShowRec = False
				End If
				
			End If
			
		End If
		
		Return p_blnShowRec
		
	End Function
	
	Private Function CheckEventTypes(ByVal xi_strEventType As String, ByRef xi_typUserFilterData As cEventLogs.FilterData) As Boolean
		
		
		' Default to showing record
		Dim p_blnShowRec As Boolean = True
		
		Select Case xi_strEventType
			Case Event_Type_Info
				If Not xi_typUserFilterData.EvtInfo Then
					p_blnShowRec = False
				End If
				
			Case Event_Type_Warning
				If Not xi_typUserFilterData.EvtWarning Then
					p_blnShowRec = False
				End If
				
			Case Event_Type_Error
				If Not xi_typUserFilterData.EvtError Then
					p_blnShowRec = False
				End If
				
			Case Event_Type_Success_Audit
				If Not xi_typUserFilterData.EvtSuccessAudit Then
					p_blnShowRec = False
				End If
				
			Case Event_Type_Failure_Audit
				If Not xi_typUserFilterData.EvtFailureAudit Then
					p_blnShowRec = False
				End If
				
			Case Else
				p_blnShowRec = False
				
		End Select
		
		Return p_blnShowRec
		
	End Function
	
	Private Function GetCategoriesMatch(ByVal xi_strThisEventCategory As String, ByVal xi_blnInclude As Boolean, ByRef xi_typUserFilterData As cEventLogs.FilterData) As Boolean
		
		Dim result As Boolean = False
		Dim p_strAllSources, p_strCategory1, p_strCategory2, p_strCategory3, p_strThisCategory As String
		
		Dim p_lngPos As Integer = (xi_strThisEventCategory.IndexOf(":"c) + 1)
		If p_lngPos > 0 Then
			p_strThisCategory = xi_strThisEventCategory.Substring(p_lngPos).Trim()
		Else
			p_strThisCategory = xi_strThisEventCategory.Trim()
		End If
		
		If (xi_typUserFilterData.Category1 = ALL_ITEMS) And (xi_typUserFilterData.Category2 = ALL_ITEMS) And (xi_typUserFilterData.Category3 = ALL_ITEMS) Then
			
			' No filtering to do
			Return True
		End If
		
		If xi_typUserFilterData.Category1 <> ALL_ITEMS Then
			p_strAllSources = xi_typUserFilterData.Category1
			p_strCategory1 = xi_typUserFilterData.Category1
		Else
			p_strCategory1 = Nothing
		End If
		
		If xi_typUserFilterData.Category2 <> ALL_ITEMS Then
			p_strAllSources = p_strAllSources & " " & xi_typUserFilterData.Category2
			p_strCategory2 = xi_typUserFilterData.Category2
		Else
			p_strCategory2 = Nothing
		End If
		
		If xi_typUserFilterData.Category3 <> ALL_ITEMS Then
			p_strAllSources = p_strAllSources & " " & xi_typUserFilterData.Category3
			p_strCategory3 = xi_typUserFilterData.Category3
		Else
			p_strCategory3 = Nothing
		End If
		
		' Include one or more sources
		If xi_blnInclude Then
			If (p_strAllSources.IndexOf(p_strThisCategory) + 1) <= 0 Then
				result = False
				
				' Handle things like 'DNS' vs 'DNS Server'
			Else
				result = (p_strThisCategory = p_strCategory1) Or (p_strThisCategory = p_strCategory2) Or (p_strThisCategory = p_strCategory3)
			End If
			
			' Exclude one or more sources
		ElseIf Not xi_blnInclude Then 
			If p_strAllSources.IndexOf(p_strThisCategory) >= 0 Then
				
				result = Not ((p_strThisCategory = p_strCategory1) Or (p_strThisCategory = p_strCategory2) Or (p_strThisCategory = p_strCategory3))
				
				' Handle things like 'DNS' vs 'DNS Server'
			Else
				result = True
				
			End If
			
		End If
		
		Return result
	End Function
	
	Private Function GetSourcesMatch(ByVal xi_strThisEventSource As String, ByVal xi_blnInclude As Boolean, ByRef xi_typUserFilterData As cEventLogs.FilterData) As Boolean
		
		Dim result As Boolean = False
		Dim p_strAllSources, p_strSource1, p_strSource2, p_strSource3 As String
		
		If (xi_typUserFilterData.Source1 = ALL_ITEMS) And (xi_typUserFilterData.Source2 = ALL_ITEMS) And (xi_typUserFilterData.Source3 = ALL_ITEMS) Then
			
			' No filtering to do
			Return True
		End If
		
		If xi_typUserFilterData.Source1 <> ALL_ITEMS Then
			p_strAllSources = "||" & xi_typUserFilterData.Source1
			p_strSource1 = xi_typUserFilterData.Source1
		Else
			p_strSource1 = Nothing
		End If
		
		If xi_typUserFilterData.Source2 <> ALL_ITEMS Then
			p_strAllSources = p_strAllSources & "||" & xi_typUserFilterData.Source2
			p_strSource2 = xi_typUserFilterData.Source2
		Else
			p_strSource2 = Nothing
		End If
		
		If xi_typUserFilterData.Source3 <> ALL_ITEMS Then
			p_strAllSources = p_strAllSources & "||" & xi_typUserFilterData.Source3
			p_strSource3 = xi_typUserFilterData.Source3
		Else
			p_strSource3 = Nothing
		End If
		
		' Include one or more sources
		If xi_blnInclude Then
			If (p_strAllSources.IndexOf("||" & xi_strThisEventSource) + 1) <= 0 Then
				result = False
				
				' Handle things like 'DNS' vs 'DNS Server'
			Else
				result = (xi_strThisEventSource = p_strSource1) Or (xi_strThisEventSource = p_strSource2) Or (xi_strThisEventSource = p_strSource3)
			End If
			
			' Exclude one or more sources
		ElseIf Not xi_blnInclude Then 
			If p_strAllSources.IndexOf("||" & xi_strThisEventSource) >= 0 Then
				
				result = Not ((xi_strThisEventSource = p_strSource1) Or (xi_strThisEventSource = p_strSource2) Or (xi_strThisEventSource = p_strSource3))
				'GetSourcesMatch = False
				
				' Handle things like 'DNS' vs 'DNS Server'
			Else
				result = True
				
			End If
			
		End If
		
		Return result
	End Function
End Module