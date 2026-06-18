Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  13-Aug-2001
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRPFExport"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	Public Enum enumStrip
		stripNonNumeric = 0
		stripNonAlpha = 1
		stripNonAlphaNumeric = 2
		stripNonASCII = 3
		stripQuote = 4
		stripSpecial = 5
	End Enum
	
	'DD 18/06/2003: Constants from hub for Recurring Interface
	'Export Header Data Array Elements
	Public Const ehdBatchTypeCode As Integer = 9
	Public Const ehdBatchCode As Integer = 10
	Public Const ehdAccountsFilter As Integer = 11
	Public Const ehdTotalNoOfTransactions As Integer = 12
	Public Const ehdBatchAmount As Integer = 13
	Public Const ehdSourceList As Integer = 14
	Public Const ehdLeadDays As Integer = 15
	Public Const ehdMediaTypeCode As Integer = 16
	Public Const ehdAutoClose As Integer = 17
	Public Const ehdBatchName As Integer = 18
	Public Const ehdArrayOnly As Integer = 19
	
	Sub Main_Renamed()
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: ReplaceTags
	'
	' Description:  Replaces the tags in a string with the collection
	'               contents.
	'
	' History: 14/08/2001 DD - Created.
	'
	' ***************************************************************** '
	Public Function ReplaceTags(ByRef colTags As Collection, ByRef sSource As String) As String
		Dim result As String = String.Empty
		Dim sResult As String = ""
		
		Try 
			
			sResult = sSource
			For	Each Tag As TagEntry In colTags
				sResult = sResult.Replace(Tag.Tag, Tag.Replacement)
			Next Tag
			
			Return sResult
		
		Catch excep As System.Exception
			
			
			
			If Information.Err().Number = 91 Then
				'ignore collection not found errors


			Else
				result = ""
				
				' Log Error Message
				bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReplaceTags Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplaceTags", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
				
				Return result
			End If
			Return result
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: Strip
	'
	' Description:  Strips certain elements from a string
	'
	' History: 14/08/2001 DD - Created.
	'
	' ***************************************************************** '
	Public Function Strip(ByRef vSource As String, ByRef StripType As enumStrip, Optional ByRef sCharacterList As String = "") As String
		Dim result As String = String.Empty
		Dim sResult As New StringBuilder
		
		Try 
			
			For iPos As Integer = 1 To vSource.Length
				Select Case StripType
					Case enumStrip.stripNonNumeric
						If Mid(vSource, iPos, 1) >= "0" And Mid(vSource, iPos, 1) <= "9" Then
							sResult.Append(Mid(vSource, iPos, 1))
						End If
					Case enumStrip.stripNonAlpha
						If Mid(vSource, iPos, 1).ToLower() >= "a" And Mid(vSource, iPos, 1).ToLower() <= "z" Then
							sResult.Append(Mid(vSource, iPos, 1))
						End If
					Case enumStrip.stripNonAlphaNumeric
						If (Mid(vSource, iPos, 1) >= "0" And Mid(vSource, iPos, 1) <= "9") And (Mid(vSource, iPos, 1).ToLower() >= "a" And Mid(vSource, iPos, 1).ToLower() <= "z") Then
							sResult.Append(Mid(vSource, iPos, 1))
						End If
					Case enumStrip.stripNonASCII
						If Strings.Asc(Mid(vSource, iPos, 1)(0)) >= 0 And Strings.Asc(Mid(vSource, iPos, 1)(0)) <= 127 Then
							sResult.Append(Mid(vSource, iPos, 1))
						End If
					Case enumStrip.stripQuote
						If (sCharacterList.IndexOf(Mid(vSource, iPos, 1)) + 1) = 0 Then
							sResult.Append(Mid(vSource, iPos, 1))
						End If
					Case enumStrip.stripSpecial
						If sCharacterList.IndexOf(Mid(vSource, iPos, 1)) >= 0 Then
							sResult.Append(Mid(vSource, iPos, 1))
						End If
				End Select
			Next iPos
			
			
			Return sResult.ToString()
		
		Catch excep As System.Exception
			
			
			
			result = ""
			
			' Log Error Message
			bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Strip Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Strip", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: nz
	'
	' Description:  Null conversion function, as per Office VBA.
	'
	' History: 13/08/2001 DD - Created.
	'
	' ***************************************************************** '
	Function nz(ByRef vValue As String, Optional ByRef vValueIfNull As String = "") As String

		If Convert.IsDBNull(vValue) Or IsNothing(vValue) Then
			Return vValueIfNull
		Else
			Return vValue
		End If
	End Function
End Module
