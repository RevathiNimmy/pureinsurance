Option Strict Off
Option Explicit On
Imports System
Module QASFormData
	'data items for the QAS Names lookup
	
	'name data
	Public m_sTitle As String = ""
	Public m_sInitial As String = ""
	Public m_sForename As String = ""
	Public m_sSurname As String = ""
	Public m_sOrgName As String = ""
	
	Public Const ACQASNames As Integer = 3
	
	Public m_bChosenAddress As Boolean
	Public m_sPartyType4QAS As String = ""
	Public m_bQASCancel As Boolean
	
	
	'JDW 20/08/2001
	
	'JDW
	Public Structure QASNamesData
		Dim Title As String
		Dim Forename As String
		Dim Surname As String
		Dim Initial As String
		Dim Postcode As String
		Dim OrgName As String
		Dim Add1 As String
		Dim Add2 As String
		Dim Add3 As String
		Dim Add4 As String
		Dim IsOrg As Boolean
		Public Shared Function CreateInstance() As QASNamesData
			Dim result As New QASNamesData
			result.Title = String.Empty
			result.Forename = String.Empty
			result.Surname = String.Empty
			result.Initial = String.Empty
			result.Postcode = String.Empty
			result.OrgName = String.Empty
			result.Add1 = String.Empty
			result.Add2 = String.Empty
			result.Add3 = String.Empty
			result.Add4 = String.Empty
			Return result
		End Function
	End Structure
	
	Public m_oQASN As QASNamesData = QASNamesData.CreateInstance()
	'
End Module