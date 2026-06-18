Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "LicenceKeyGen"
	Private Const ACClass As String = "MainModule"
	
	Public Const ACSystemPrefix As String = "SYSTEM_"
	Public Const ACProductPrefix As String = "PRODUCT_"
	Public Const ACInstalledProducts As String = "INSTALLED_PRODUCTS"
	Public Const ACProductDescription As String = "ProductDescription"
	Public Const ACLicenceLimit As String = "LicenceLimit"
	Public Const ACLicenceKey As String = "LicenceKey"
	Public Const ACExceedLimitAction As String = "ExceedLimitAction"
	Public Const ACExceedLimitBlock As String = "Block"
	Public Const ACExceedLimitWarn As String = "Warn"
	Public Const ACExceedLimitNone As String = "None"
	
	Declare Function WritePrivateProfileSection Lib "kernel32"  Alias "WritePrivateProfileSectionA"(ByVal lpAppName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
	
	Declare Function WritePrivateProfileString Lib "kernel32"  Alias "WritePrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As Integer, ByVal lpString As Integer, ByVal lpFileName As String) As Integer
	
	Declare Function GetPrivateProfileString Lib "kernel32"  Alias "GetPrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As Integer, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
End Module