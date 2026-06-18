Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
<System.Runtime.InteropServices.ProgId("PBQuoteTypeEncode_NET.PBQuoteTypeEncode")> _
 Public Module PBQuoteTypeEncode
	
	
	' History:
	' RAW 08/09/2003 : CQ277 : added GetQuoteTypeDesc
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "PBQuoteTypeEncode"
	
	'public constants
	Public Const PBCQemQuoteTypeQuote As Integer = 1
	Public Const PBCQemQuoteTypeValidate As Integer = 2
	Public Const PBCQemQuoteTypeUal As Integer = 3
	Public Const PBCQemQuoteTypeDefault As Integer = 4
	Public Const PBCQemQuoteTypeRenewal As Integer = 5
	Public Const PBCQemQuoteTypePreScreen As Integer = 6
	Public Const PBCQemQuoteTypeCopyRisk As Integer = 7
    Public Const PBCQemQuoteTypeRenewalLapse As Integer = 8
	
	
	' ***************************************************************** '
	'
	' Name: EncodeTransactionScreenAndType
	'
	' Description: Encodes Transaction, Screen id and tYpe from encoded value
	'              Originally TTTSSYY
	'              Now        1TTTSSSSYY
	'
	' History: 19/12/2001 CLG - Created.
	'
	' ***************************************************************** '
	Public Sub EncodeTransactionScreenAndType(ByRef r_lEncoded As Integer, ByRef r_lTransactionType As Double, ByRef r_lGISScreenId As Double, ByRef r_lQuoteType As Double)
		
		' Debug message
		Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".EncodeTransactionScreenAndType")
		
		Try 
			
			'new format 1TTTSSSSYY
			r_lEncoded = CInt(1000000000 + (r_lTransactionType * 1000000) + (r_lGISScreenId * 100) + r_lQuoteType)
			
			' Debug message
			Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".EncodeTransactionScreenAndType")
		
		Catch excep As System.Exception
			
			
			
			' Debug message
			Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".EncodeTransactionScreenAndType")
			
			
			' Log Error Message
			bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncodeTransactionScreenAndType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncodeTransactionScreenAndType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: decodeTransactionScreenAndType
	'
	' Description: decodes Transaction, Screen id and tYpe from encoded value
	'              Originally TTTSSYY
	'              Now also   1TTTSSSSYY
	'
	' History: 19/12/2001 CLG - Created.
	'
	' ***************************************************************** '
	Public Sub decodeTransactionScreenAndType(ByRef r_lEncoded As Integer, ByRef r_lTransactionType As Integer, ByRef r_lGISScreenId As Integer, ByRef r_lQuoteType As Integer)
		
		' Debug message
		Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".decodeTransactionScreenAndType")
		
		Try 
			
			If r_lEncoded < 1000000000 Then
				'old format TTTSSYY
				r_lQuoteType = r_lEncoded Mod 100
				r_lEncoded = r_lEncoded \ 100
				r_lGISScreenId = r_lEncoded Mod 100
				r_lEncoded = r_lEncoded \ 100
				r_lTransactionType = r_lEncoded
			Else
				'new format 1TTTSSSSYY
				r_lEncoded -= 1000000000
				r_lQuoteType = r_lEncoded Mod 100
				r_lEncoded = r_lEncoded \ 100
				r_lGISScreenId = r_lEncoded Mod 10000
				r_lEncoded = r_lEncoded \ 10000
				r_lTransactionType = r_lEncoded
			End If
			
			' Debug message
			Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".decodeTransactionScreenAndType")
		
		Catch excep As System.Exception
			
			
			
			' Debug message
			Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".decodeTransactionScreenAndType")
			
			
			' Log Error Message
			bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="decodeTransactionScreenAndType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="decodeTransactionScreenAndType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	
	' ***************************************************************** '
	' Name: GetQuoteTypeDesc
	' Description:
	' History:
	' RAW 08/09/2003 : CQ277 : created
	' ***************************************************************** '
	Public Function GetQuoteTypeDesc(ByVal v_lQuoteType As Integer) As String
		
		Dim result As String = String.Empty
		Try 
			
			result = "Unknown"
			
			
			Select Case v_lQuoteType
				Case PBCQemQuoteTypeQuote
					Return "Rating"
				Case PBCQemQuoteTypeValidate
					Return "Validation"
				Case PBCQemQuoteTypeUal
					Return "User Authority Limits"
				Case PBCQemQuoteTypeDefault, PBCQemQuoteTypePreScreen
					Return "Default"
				Case PBCQemQuoteTypeRenewal
					Return "Renewal"
				Case Else
					Return "Rating"
			End Select
		
		Catch 
		End Try
		
		
		
        bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteTypeDesc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteTypeDesc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
End Module