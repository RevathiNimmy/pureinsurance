Option Strict Off
Option Explicit On
Imports System
Module MainModule
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCountryId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUserName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPassword As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Private _g_oObjectManager As bObjectManager.ObjectManager = Nothing
	Public Property g_oObjectManager() As bObjectManager.ObjectManager
		Get
			If _g_oObjectManager Is Nothing Then
				_g_oObjectManager = New bObjectManager.ObjectManager()
			End If
			Return _g_oObjectManager
		End Get
		Set(ByVal Value As bObjectManager.ObjectManager)
			_g_oObjectManager = value
		End Set
	End Property
	
	Public Const ACApp As String = "iSIRRSTEarningPattern"
	
	Public Const ACEarningPatternDetails As Integer = 101
	Public Const ACEarningPattern As Integer = 102
	Public Const ACEffectiveDate As Integer = 103
	
	Public Const ACEPDaily As Integer = 1
	Public Const ACEPFully As Integer = 2
	
	Public Const ACEarningPatternValue As Integer = 0
	Public Const ACEffectiveDateValue As Integer = 1
End Module