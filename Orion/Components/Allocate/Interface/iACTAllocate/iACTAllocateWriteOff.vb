Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmWriteOff
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmWriteOff"
	
	
	Public m_oWriteOffReason As Object
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_vDebitAmount As String = ""
	Private m_vCreditAmount As String = ""
	
	Public Property DebitAmount() As String
		Get
			Return m_vDebitAmount
		End Get
		Set(ByVal Value As String)

			m_vDebitAmount = CStr(Value)
		End Set
	End Property
	
	Public Property CreditAmount() As String
		Get
			Return m_vCreditAmount
		End Get
		Set(ByVal Value As String)

			m_vCreditAmount = CStr(Value)
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public ReadOnly Property WriteOffReason() As String
		Get
			
			Dim sReason As String = ""
			
			m_lReturn = CType(GetReasonFromWriteOff(sWriteOffReason:=sReason), gPMConstants.PMEReturnCode)
			
			Return sReason
			
		End Get
	End Property
	
	Public ReadOnly Property WriteOffReasonCode() As String
		Get
			
			Dim sCode As String = ""
			
			m_lReturn = CType(GetCodeFromWriteOff(sWriteOffCode:=sCode), gPMConstants.PMEReturnCode)
			
			Return sCode
			
		End Get
	End Property
	
	Public ReadOnly Property WriteOffReasonID() As Integer
		Get
			
			Dim lWriteOffReasonID As Integer
			
			' Get the ID to match the code

			m_lReturn = m_oWriteOffReason.MatchIDWithCode(vCode:=WriteOffReasonCode, vID:=lWriteOffReasonID)
			
			Return lWriteOffReasonID
			
		End Get
	End Property
	
	' ***************************************************************** '
	'
	' Name: Initialise
	'
	' Description:
	'
	' History: 03/03/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function Initialise() As Integer



		
		Dim result As Integer = 0
		Dim sReason As String = ""
		Dim vWriteOffReasons(,) As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set status
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			Dim temp_m_oWriteOffReason As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oWriteOffReason, "bACTWriteOffReason.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oWriteOffReason = temp_m_oWriteOffReason
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return m_lReturn
			End If
			
			' Add the write off reasons

			m_lReturn = m_oWriteOffReason.GetReasons(vReasonList:=vWriteOffReasons)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Dont exit, we'll set the status next
				' Exit Function
			Else
				
				If Information.IsArray(vWriteOffReasons) Then
					cboWriteoffReason.Items.Clear()

					For iLoop1 As Integer = vWriteOffReasons.GetLowerBound(1) To vWriteOffReasons.GetUpperBound(1)


                        sReason = "[" & CStr(vWriteOffReasons(0, iLoop1)) & "] " & CStr(vWriteOffReasons(1, iLoop1))
						cboWriteoffReason.Items.Add(sReason)
					Next iLoop1
					cboWriteoffReason.SelectedIndex = 0
				End If
				
			End If
			
			' Set the captions

            panDebitAmount.Name = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, DebitAmount)

            panCreditAmount.Name = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CreditAmount)

            panWriteOffAmount.Name = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(DebitAmount) + CDec(CreditAmount)))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetCodeFromWriteOff
	'
	' Description: Gets the code from the combo box that has the
	'              write off reason and code in.
	'              Should be in the format of "[code] reason"
	'
	' CF-130898
	'
	' ***************************************************************** '
	Private Function GetCodeFromWriteOff(ByRef sWriteOffCode As String) As Integer
		
		Dim result As Integer = 0
		Dim iLen As Integer
		Dim sReasonTxt, sByte_Renamed As String
		Dim sCode As New StringBuilder
		Dim iStart As Integer
		
		Const LEFTBRACKET As String = "["
		Const RIGHTBRACKET As String = "]"
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Error trapping
		Try 
			
			' Grab the string
			sReasonTxt = cboWriteoffReason.Text
			
			' Get the length of the string
			iLen = sReasonTxt.Length
			
			' Skip to the start of the "[". SHOULD be the first character.
			
			iStart = (sReasonTxt.IndexOf(LEFTBRACKET) + 1)
			
			iStart += 1
			
			If (iStart = 0) Or (iStart > iLen) Then
				
				result = gPMConstants.PMEReturnCode.PMError
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteOffReason combo box does not have a valid code.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeFromWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return result
				
			End If
			
			' Zero out the code
			sCode = New StringBuilder("")
			
			' Loop through the string
			For iLoop1 As Integer = iStart To iLen
				
				' Grab the next character
				sByte_Renamed = sReasonTxt.Substring(iLoop1 - 1, 1)
				
				' If its the end bracket, then exit the loop
				If sByte_Renamed = RIGHTBRACKET Then
					Exit For
				End If
				
				' Store the next character
				sCode.Append(sByte_Renamed)
				
			Next iLoop1
			
			' Return the value
			sWriteOffCode = sCode.ToString()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get code from write off reason.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeFromWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetReasonFromWriteOff
	'
	' Description: Gets the reason from the combo box that has the
	'              write off reason and code in.
	'              Should be in the format of "[code] reason"
	'
	'
	'
	' ***************************************************************** '
	Private Function GetReasonFromWriteOff(ByRef sWriteOffReason As String) As Integer
		
		Dim result As Integer = 0
		Dim iLen As Integer
		Dim sReasonTxt, sByte_Renamed As String
		Dim sReason As New StringBuilder
		Dim iStart As Integer
		
        'Const LEFTBRACKET As String = "["
		Const RIGHTBRACKET As String = "]"
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Error trapping
		Try 
			
			' Grab the string
			sReasonTxt = cboWriteoffReason.Text
			
			' Get the length of the string
			iLen = sReasonTxt.Length
			
			' Skip to the start of the "]"
			
			iStart = (sReasonTxt.IndexOf(RIGHTBRACKET) + 1)
			
			iStart += 1
			
			If (iStart = 0) Or (iStart > iLen) Then
				
				result = gPMConstants.PMEReturnCode.PMError
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteOffReason combo box does not have a valid code.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeFromWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return result
				
			End If
			
			' Zero out the reason
			sReason = New StringBuilder("")
			
			' Loop through the string
			For iLoop1 As Integer = iStart To iLen
				
				' Grab the next character
				sByte_Renamed = sReasonTxt.Substring(iLoop1 - 1, 1)
				
				' Store the next character
				sReason.Append(sByte_Renamed)
				
			Next iLoop1
			
			' Return the value
			sWriteOffReason = sReason.ToString()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get write off reason.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReasonFromWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		Me.Hide()
		
	End Sub
	
	Private Sub frmWriteOff_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		If Not (m_oWriteOffReason Is Nothing) Then

            m_oWriteOffReason.Dispose()
            m_oWriteOffReason = Nothing
		End If
		eventArgs.Cancel = Cancel <> 0
	End Sub
End Class
