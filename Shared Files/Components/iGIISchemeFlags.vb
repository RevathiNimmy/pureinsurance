Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Module SchemeFlags
Public Module SchemeFlags
	Public Const ACClassName As String = "SchemeFlags"
	Public Const PARAMETER_NOT_PRESENT_NO As Integer = -32767
	Public Const PARAMETER_NOT_PRESENT_STR As String = "-32727"
	
	' Scheme Flags Constants
	
	Public Const SCHEMEFLAG_FORMS As Integer = 1
	Public Const SCHEMEFLAG_VBS As Integer = 2
	Public Const SCHEMEFLAG_VBS_ONLY As Integer = 4
	Public Const SCHEMEFLAG_FULL_GUARANTEE As Integer = 8
	Public Const SCHEMEFLAG_SECOND_CAR As Integer = 16
	Public Const SCHEMEFLAG_PREMIUM_OVERRIDE As Integer = 32
	Public Const SCHEMEFLAG_COMMISSION_OVERRIDE As Integer = 64
	Public Const SCHEMEFLAG_ADD_ONS As Integer = 128
	Public Const SCHEMEFLAG_QUOTE_GUARANTEE As Integer = 256
	Public Const SCHEMEFLAG_EDI_MTA As Integer = 512
	Public Const SCHEMEFLAG_POLICY_NUMBERS As Integer = 1024
	Public Const SCHEMEFLAG_COVERNOTE_NUMBERS As Integer = 2048
	'sj 21/11/2001 - start
	Public Const SCHEMEFLAG_AUTO_RENEW As Integer = 4096
	'sj 21/11/2001 - end
	'sjd 17/9/2002
	Public Const SCHEMEFLAG_PMD As Integer = 8192
	'sjd 17/9/2002
	
	' ***************************************************************** '
	' Name: DecodeSchemeFlags
	'
	' Description:
	'
	'
	' sjd 16/10/2002 - add New parameter to process PMD
	' ***************************************************************** '
	Public Function DecodeSchemeFlags(ByVal v_lSchemeFlags As Integer, Optional ByRef r_sFormFlag As String = PARAMETER_NOT_PRESENT_STR, Optional ByRef r_sVbsFlag As String = PARAMETER_NOT_PRESENT_STR, Optional ByRef r_sGuarenteedFlag As String = PARAMETER_NOT_PRESENT_STR, Optional ByRef r_sSecondCarFlag As String = PARAMETER_NOT_PRESENT_STR, Optional ByRef r_bAllowPremiumOverride As Boolean = False, Optional ByRef r_bAllowCommissionOverride As Boolean = False, Optional ByRef r_bAllowAddOns As Boolean = False, Optional ByRef r_bAllowEDIMTAs As Boolean = False, Optional ByRef r_bPolicyNumbers As Boolean = False, Optional ByRef r_bCovernoteNumbers As Boolean = False, Optional ByRef r_bAutoRenew As Boolean = False, Optional ByRef r_sPmdFlag As String = PARAMETER_NOT_PRESENT_STR) As Integer
		
		Dim result As Integer = 0
		Dim lResult As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			r_bAllowPremiumOverride = False
			r_bAllowCommissionOverride = False
			r_bAllowAddOns = False
			
			' Form flag
			If r_sFormFlag <> PARAMETER_NOT_PRESENT_STR Then
				r_sFormFlag = ""
				lResult = v_lSchemeFlags And SCHEMEFLAG_FORMS
				If lResult = SCHEMEFLAG_FORMS Then
					r_sFormFlag = "f"
				End If
			End If
			
			' Vbs flag
			If r_sVbsFlag <> PARAMETER_NOT_PRESENT_STR Then
				r_sVbsFlag = ""
				lResult = v_lSchemeFlags And SCHEMEFLAG_VBS
				If lResult = SCHEMEFLAG_VBS Then
					' KBrown - 220402 - Use 'E' for EDI schemes (NU EDI Development)
					'r_sVbsFlag = "E"
					'RWH(09/02/2004) Changed back to "v" as is needed in check
					'in calling function. The "E" setting is not actually used in
					'any project which uses this function.
					r_sVbsFlag = "v"
				End If
				lResult = v_lSchemeFlags And SCHEMEFLAG_VBS_ONLY
				If lResult = SCHEMEFLAG_VBS_ONLY Then
					r_sVbsFlag = "o"
				End If
			End If
			
			' Guarenteed flag Uppercase "G" Or Lowercase "g"
			If r_sGuarenteedFlag <> PARAMETER_NOT_PRESENT_STR Then
				r_sGuarenteedFlag = ""
				lResult = v_lSchemeFlags And SCHEMEFLAG_FULL_GUARANTEE
				If lResult = SCHEMEFLAG_FULL_GUARANTEE Then
					r_sGuarenteedFlag = "G"
				End If
				lResult = v_lSchemeFlags And SCHEMEFLAG_QUOTE_GUARANTEE
				If lResult = SCHEMEFLAG_QUOTE_GUARANTEE Then
					r_sGuarenteedFlag = "g"
				End If
			End If
			
			
			' Second car flag
			If r_sSecondCarFlag <> PARAMETER_NOT_PRESENT_STR Then
				r_sSecondCarFlag = ""
				lResult = v_lSchemeFlags And SCHEMEFLAG_SECOND_CAR
				If lResult = SCHEMEFLAG_SECOND_CAR Then
					r_sSecondCarFlag = "s"
				End If
			End If
			
			' Allow premium override
			lResult = v_lSchemeFlags And SCHEMEFLAG_PREMIUM_OVERRIDE
			If lResult = SCHEMEFLAG_PREMIUM_OVERRIDE Then
				r_bAllowPremiumOverride = True
			End If
			
			' Allow commission override
			lResult = v_lSchemeFlags And SCHEMEFLAG_COMMISSION_OVERRIDE
			If lResult = SCHEMEFLAG_COMMISSION_OVERRIDE Then
				r_bAllowCommissionOverride = True
			End If
			
			' Allow Addons
			lResult = v_lSchemeFlags And SCHEMEFLAG_ADD_ONS
			If lResult = SCHEMEFLAG_ADD_ONS Then
				r_bAllowAddOns = True
			End If
			
			' NB 256 is used for Guaranteed 'g' and 'G'
			
			' EDI MTA's
			lResult = v_lSchemeFlags And SCHEMEFLAG_EDI_MTA
			If lResult = SCHEMEFLAG_EDI_MTA Then
				r_bAllowEDIMTAs = True
			End If
			
			' Policy Number Ranges
			lResult = v_lSchemeFlags And SCHEMEFLAG_POLICY_NUMBERS
			If lResult = SCHEMEFLAG_POLICY_NUMBERS Then
				r_bPolicyNumbers = True
			End If
			
			' CoverNote Number Ranges
			lResult = v_lSchemeFlags And SCHEMEFLAG_COVERNOTE_NUMBERS
			If lResult = SCHEMEFLAG_COVERNOTE_NUMBERS Then
				r_bCovernoteNumbers = True
			End If
			
			'sj 21/11/2001 - start
			' Renewal Auto Renew
			lResult = v_lSchemeFlags And SCHEMEFLAG_AUTO_RENEW
			If lResult = SCHEMEFLAG_AUTO_RENEW Then
				r_bAutoRenew = True
			End If
			'sj 21/11/2001 - end
			
			'sjd 17/9/2002 - start
			' PMD scheme
			If r_sPmdFlag <> PARAMETER_NOT_PRESENT_STR Then
				r_sPmdFlag = ""
				lResult = v_lSchemeFlags And SCHEMEFLAG_PMD
				If lResult = SCHEMEFLAG_PMD Then
					r_sPmdFlag = "p"
				End If
			End If
			'sjd 17/9/2002 - end
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
            ' Log Error Message
            'Modified by Archana Tokas on 4/26/2010 3:55:16 PM changes as per requirement
            'iPMFunc.LogMessage(sUsername:=g_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DecodeSchemeFlags Failed", vApp:=ACApp, vClass:=ACClassName, vMethod:="DecodeSchemeFlags", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DecodeSchemeFlags Failed", vApp:=ACApp, vClass:=ACClassName, vMethod:="DecodeSchemeFlags", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: EncodeSchemeFlags
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Public Function EncodeSchemeFlags(ByVal v_sVbsFlag As String, ByVal v_sFrmFlag As String, ByVal v_sGtdFlag As String, ByVal v_sPmdFlag As String, ByVal v_s2carmFlag As String, ByVal v_sOvrFlag As String, ByVal v_sComFlag As String, ByVal v_sAddFlag As String, ByVal v_bEDIMTAFlag As Boolean, ByVal v_bPolicyNos As Boolean, ByVal v_bCovernoteNos As Boolean, ByVal v_bAutoRenew As Boolean, ByRef r_lSchemeTypeFlags As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			r_lSchemeTypeFlags = 0
			
			If v_sFrmFlag.ToLower() = "f" Then
				r_lSchemeTypeFlags += SCHEMEFLAG_FORMS
			End If
			
			If v_sVbsFlag.ToLower() = "v" Then
				r_lSchemeTypeFlags += SCHEMEFLAG_VBS
			End If
			
			If v_sVbsFlag.ToLower() = "o" Then
				r_lSchemeTypeFlags += SCHEMEFLAG_VBS_ONLY
			End If
			' TB 13/9/00 G & g mean different things, preserve case
			' Keep flag + 8 as uppercase 'G'
			' Next flag is 256 for lowercase 'g'
			If v_sGtdFlag = "G" Then
				r_lSchemeTypeFlags += SCHEMEFLAG_FULL_GUARANTEE
			End If
			If v_sGtdFlag = "g" Then
				r_lSchemeTypeFlags += SCHEMEFLAG_QUOTE_GUARANTEE
			End If
			
			If v_s2carmFlag.ToLower() = "s" Then
				r_lSchemeTypeFlags += SCHEMEFLAG_SECOND_CAR
			End If
			
			If v_sOvrFlag.ToLower() = "y" Then
				r_lSchemeTypeFlags += SCHEMEFLAG_PREMIUM_OVERRIDE
			End If
			
			If v_sComFlag.ToLower() = "y" Then
				r_lSchemeTypeFlags += SCHEMEFLAG_COMMISSION_OVERRIDE
			End If
			
			If v_sAddFlag.ToLower() = "y" Then
				r_lSchemeTypeFlags += SCHEMEFLAG_ADD_ONS
			End If
			
			' NB Flag 256 is used, see "G" and "g"
			
			If v_bEDIMTAFlag Then
				r_lSchemeTypeFlags += SCHEMEFLAG_EDI_MTA
			End If
			
			If v_bPolicyNos Then
				r_lSchemeTypeFlags += SCHEMEFLAG_POLICY_NUMBERS
			End If
			
			If v_bCovernoteNos Then
				r_lSchemeTypeFlags += SCHEMEFLAG_COVERNOTE_NUMBERS
			End If
			
			'sj 21/11/2001 - start
			If v_bAutoRenew Then
				r_lSchemeTypeFlags += SCHEMEFLAG_AUTO_RENEW
			End If
			'sj 21/11/2001 - end
			
			' sjd 17/9/2002 - start
			If v_sPmdFlag.ToLower() = "p" Then
				r_lSchemeTypeFlags += SCHEMEFLAG_PMD
			End If
			' sjd 17/9/2002 - end
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            'Modified by Archana Tokas on 4/26/2010 3:55:16 PM changes as per requirement
            'iPMFunc.LogMessage(sUsername:=g_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncodeSchemeFlags Failed", vApp:=ACApp, vClass:=ACClassName, vMethod:="EncodeSchemeFlags", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncodeSchemeFlags Failed", vApp:=ACApp, vClass:=ACClassName, vMethod:="EncodeSchemeFlags", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module