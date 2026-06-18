Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Text
Module ENCRYPT
	
	Function Encryption(ByRef sPassword As String) As String
		'
		' Encrypts the supplied string returning the encrypted
		' result. Encrypted string will always be 2 characters
		' longer than original (leave space!)
		'
		' Encrypted string contains only ASCII characters in
		' range 32-126
		
		Dim sChar1 As New FixedLengthString(1)
		Dim sChar2 As New FixedLengthString(1)
		
		Dim CodeStr As String = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
		Dim iClen As Integer = CodeStr.Length
		
		Dim sAStr As String = sPassword
		Dim iCnt As Integer = sAStr.Length
		If iCnt < 1 Then
			Return "Error"
		End If
		sChar1.Value = CodeStr.Substring((Strings.Asc(sAStr.Substring(0, 1)(0)) + iCnt) Mod iClen, 1)
		sChar2.Value = CodeStr.Substring(Strings.Asc(sAStr.Substring(sAStr.Length - 1)(0)) Mod iClen, 1)
		Dim iSn As Integer = ((Strings.Asc(sChar1.Value(0)) + Strings.Asc(sChar2.Value(0))) Mod iClen) + 1
		Dim sBStr As New StringBuilder
		sBStr.Append(sChar2.Value)
		For iCnt2 As Integer = 1 To iCnt
			sBStr.Append(CodeStr.Substring((Strings.Asc(sAStr.Substring(iCnt2 - 1, 1)(0)) + iSn + iCnt2) Mod iClen, 1))
		Next iCnt2
		sBStr.Append(sChar1.Value)
		
		Return sBStr.ToString()
		
	End Function
	
	Function LicenceKey(ByRef sProduct As String, ByRef iCusNum As Integer, ByRef iLicence As Integer) As String
		
		Return Encryption(StringsHelper.Format(iLicence, "0#") & StringsHelper.Format(iCusNum, "0#") & sProduct & Strings.Chr(19).ToString() & Strings.Chr(8).ToString() & Strings.Chr(63).ToString() & StringsHelper.Format(iLicence, "0#"))
		
	End Function
	
	Function LicenceOK(ByRef sProduct As String, ByRef iCusNum As Integer, ByRef iLicence As Integer, ByRef sPassKey As String) As Integer
		
		Dim sNewKey As String = LicenceKey(sProduct, iCusNum, iLicence)
		Return sNewKey = sPassKey
		
	End Function
End Module