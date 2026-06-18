Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
<System.Runtime.InteropServices.ProgId("ACTFunc_NET.ACTFunc")> _
 Public Module ACTFunc
	' ***************************************************************** '
	' Module Name: ACTFunc
	'
	' Date: 17 September 1997
	'
	' Description: Functions common to business & interface
	'
	' Edit History:
	' ***************************************************************** '
	
	Const ACClass As String = "ACTFunc"
	
	
	
	' ***************************************************************** '
	' Function CompanyBaseCurrency
	'
	' Date: 05 October 1997
	'
	' Description: Return this company's base currency id
	'
	' Edit History:
	' ***************************************************************** '
	
	Public Function CompanyBaseCurrency() As Integer
		Return 26 'GBP
	End Function
	
	' ***************************************************************** '
	' Function CurrentCompany
	'
	' Date: 05 October 1997
	'
	' Description: Return the current CompanyId
	'
	' Edit History:
	' ***************************************************************** '
	
	Public Function CurrentCompany() As Integer
		Return 1
	End Function
	
	' ***************************************************************** '
	' Function GetSLSuspense
	'
	' Date: 05 October 1997
	'
	' Description: Return the Sales Ledger Suspense account
	'
	' Edit History:
	' ***************************************************************** '
	
	Public Function GetSLSuspense() As Integer
		Return 32
	End Function
	
	' ***************************************************************** '
	' Function GetPLSuspense
	'
	' Date: 05 October 1997
	'
	' Description: Return the Purchase Ledger Suspense account
	'
	' Edit History:
	' ***************************************************************** '
	
	Public Function GetPLSuspense() As Integer
		Return 33
	End Function
	
	
	' ***************************************************************** '
	' Name: ParseArray
	'
	' Description: Convert a 1-d array to pipe delimited string and vice
	' versa.
	' String Fromat will be "data1|data2|..dataX|"
	'
	' ***************************************************************** '
	Public Function ParseArray(ByRef vArray() As Object, ByRef sString As String, ByRef bArrayToString As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim sTmp As New StringBuilder
		Dim iMax As Integer
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If bArrayToString Then
				'convert the array to string
				sString = ""
				If Not Information.IsArray(vArray) Then
					Return result
				End If
				
				For	Each vArray_item As Object In vArray

					sString = sString & CStr(vArray_item) & "|"
				Next vArray_item
				
			Else
				'convert the string to an array
				If sString.Trim() = "" Then
					vArray = Nothing
					Return result
				End If
				
				iMax = 0
				sTmp = New StringBuilder("")
				For i As Integer = 1 To sString.Length
					
					If sString.Substring(i - 1, 1) = "|" Then
						'add to the arrays
						If Information.IsArray(vArray) Then
							ReDim Preserve vArray(iMax)
						Else
							ReDim vArray(iMax)
						End If
						

						vArray(iMax) = sTmp.ToString()
						sTmp = New StringBuilder("")
						iMax += 1
					Else
						sTmp.Append(sString.Substring(i - 1, 1))
					End If
					
				Next i
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ParseArrayFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ParseArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module