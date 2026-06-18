Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("PolicyNumberGen_NET.PolicyNumberGen")> _
 Public Module PolicyNumberGen
	Public Const MMAInsurerNo As Integer = 21
	Public Const NUInsurerNo As Integer = 6
	Public Const CORNHILLInsurerNo As Integer = 13
	Public Const CGUInsurerNo As Integer = 1
	Public Const FORTISInsurerNo As Integer = 9
	Public Const GANInsurerNo As Integer = 15
	Public Const LINKInsurerNo As Integer = 51
	Public Const PEGASUSInsurerNo As Integer = 71
	Public Const NIGInsurerNo As Integer = 20
	
	Public Const I4MPolicyNums As String = "I4MPOLNO"
	
	Public Const MMARangeCode As String = "MMA"
	Public Const NURangeCode As String = "NU"
	Public Const CORNHILLRangeCode As String = "CORNHILL"
	Public Const CGURangeCode As String = "CGU"
	Public Const FORTISRangeCode As String = "FORTIS"
	Public Const GANRangeCode As String = "GAN"
	Public Const LINKRangeCode As String = "LINK"
	Public Const PEGASUSRangeCode As String = "PEGASUS"
	Public Const NIGRangeCode As String = "NIG"
	Public Const I4MRangeCode As String = "I4M"
	
	
	' ***************************************************************** '
	'
	' Name: GetAgencyRef
	'
	' Description:
	'
	' History: 20/07/2000 RFC - Created.
	'
	' ***************************************************************** '
	Public Sub GetAgencyRef(ByVal v_lInsurerNo As Integer, ByVal v_lSchemeNo As Integer, ByRef r_sAgencyRef As String)
        Dim ACClass As Object = Nothing
		
		Try 
			
			
			Select Case v_lInsurerNo
				' MMA
				Case MMAInsurerNo
					r_sAgencyRef = "00470"
					
					' NU
				Case NUInsurerNo
					r_sAgencyRef = "2SN200"
					
					' Cornhill
				Case CORNHILLInsurerNo
					r_sAgencyRef = "6385569"
					
					' NIG
				Case NIGInsurerNo
					r_sAgencyRef = "1234"
					
				Case Else
					r_sAgencyRef = "AGCYREF"
					
			End Select
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message

			GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgencyRef Failed", vApp:=ACApp, vClass:=CStr(ACClass), vMethod:="GetAgencyRef", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: GenPolicyNum
	'
	' Description:
	'
	' History: 20/07/2000 RFC - Created.
	'
	' CJB270701 Add extra 'v_sQMMSuffix' param on for QMM schemes that don't
	'           generate their own pol no in Cobol.
	'
	' ***************************************************************** '
	Public Function GenPolicyNum(ByVal v_lInsurerNo As Integer, ByVal v_lSchemeNo As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_sQMMSuffix As String, ByRef r_sPolicyNum As String) As Integer
		Dim result As Integer = 0
        Dim ACClass As Object = Nothing
		Dim g_iCurrencyID, g_iLanguageID, g_iLogLevel, g_iSourceID, g_iUserID As Integer
        Dim g_sPassword As String = ""
        Dim g_sUsername As String = ""
		
		Dim lReturn As gPMConstants.PMEReturnCode
        'Modified by Vijay Pal on 5/25/2010 3:44:30 PM todolist
        'Dim oAutoNum As bPMAutoNumber.Business
        Dim oAutoNum As Object
		
		Dim sRangeCode As String = ""
        Dim lPolNum As Integer
		Dim sPrefix, sSuffix As String
		
		Dim sAgencyRef As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
            'Modified by Vijay Pal on 5/25/2010 3:44:54 PM todolist
            'oAutoNum = New bPMAutoNumber.Business()
            oAutoNum = New Object
			'Set oAutoNum = New bPMAutoNumber.Business
			
			lReturn = CType(oAutoNum, SSP.S4I.Interfaces.IBusiness).Initialise(g_sUsername, g_sPassword, g_iUserID, g_iSourceID, g_iLanguageID, g_iCurrencyID, g_iLogLevel, ACApp)
			
			'If (GenPolicyNum <> PMTrue) Then
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				' Log Error Message

				GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oAutoNum.Initialise returned " & lReturn, vApp:=ACApp, vClass:=CStr(ACClass), vMethod:="GenPolicyNum")
				
				Return lReturn
				
			End If
			
			' What Range Code do we want
			Select Case v_lInsurerNo
				Case MMAInsurerNo
					sRangeCode = MMARangeCode
				Case CORNHILLInsurerNo
					sRangeCode = CORNHILLRangeCode
				Case NIGInsurerNo
					sRangeCode = NIGRangeCode
				Case PEGASUSInsurerNo
					sRangeCode = PEGASUSRangeCode
				Case LINKInsurerNo
					sRangeCode = LINKRangeCode
				Case NUInsurerNo
					sRangeCode = NURangeCode
				Case Else
					sRangeCode = I4MRangeCode
			End Select
			
			'    ' Get the Policy Number Range
			'    lReturn = oAutoNum.GetNumberRange( _
			''        v_sGroupCode:=I4MPolicyNums, _
			''        v_sRangeCode:=sRangeCode, _
			''        v_sPMProductCode:=PMProductCode(pmePFSiriusSolutions), _
			''        r_lnumberrangeid:=lPolNoNumRange)
			'    If (GenPolicyNum <> pmtrue) Then
			'        GenPolicyNum = lReturn
			'        Exit Function
			'    End If
			
			
			lReturn = oAutoNum.GenerateNewNumber(v_sPMProductCode:=gPMConstants.PMProductCode(gPMConstants.PMEProductFamily.pmePFSiriusSolutions), v_sGroupCode:=I4MPolicyNums, v_sRangeCode:=sRangeCode, v_iUserId:=1, r_lNumber:=lPolNum, r_sPrefix:=sPrefix, r_sSuffix:=sSuffix)
			
			'If (GenPolicyNum <> PMTrue) Then
			
			' RG - AutoNum component returns 0 anyway !
			' for now, just check for the polno being 0
			
			'If (lReturn <> PMTrue) Then
			If lPolNum = 0 Then
				
				' Log Error Message

				GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oAutoNum.GenerateNewNumber returned zero policy number", vApp:=ACApp, vClass:=CStr(ACClass), vMethod:="GenPolicyNum")
				
				Return lReturn
			End If
			
            oAutoNum.Dispose()
            oAutoNum = Nothing
			
			' Get the Agency Ref for this Insurer
			GetAgencyRef(v_lInsurerNo:=v_lInsurerNo, v_lSchemeNo:=v_lSchemeNo, r_sAgencyRef:=sAgencyRef)
			
			
			' Format the Policy Number
			FormatPolicyNum(v_lInsurerNo:=v_lInsurerNo, v_lNumber:=lPolNum, v_sPrefix:=sPrefix, v_sSuffix:=sSuffix, v_sAgencyRef:=sAgencyRef, v_dtEffectiveDate:=v_dtEffectiveDate, v_sQMMSuffix:=v_sQMMSuffix, r_sPolicyNum:=r_sPolicyNum)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message

			GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenPolicyNum Failed", vApp:=ACApp, vClass:=CStr(ACClass), vMethod:="GenPolicyNum", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	
	' ***************************************************************** '
	'
	' Name: FormatPolicyNum
	'
	' Description:
	'
	' History: 20/07/2000 RFC - Created.
	'
	'MMA Policy number       511(00470)000001 to 5110047099999
	'(in brackets is the agency number)
	'its4me only
	'
	'NU  Policy number       YY3(M)MSN70100 to YY3(M)MSN70599
	'where yy = year and (M)M = month either 1 0r 2 digits
	'So, eg policies issues this month =     0037SN70100 to 0037SN70599
	'Agency number   2SN200
	'xelector , iii And its4me
	'
	'Cornhill    Policy number       PH19450001 to PH19450099
	'Agency number   4734676
	'xelector , iii And its4me
	'
	'
	' RAG 22-08-00 NIG and LINK only need six digits, not eight.
	' CJB270701 Add extra 'v_sQMMSuffix' param on for QMM schemes that don't
	'           generate their own pol no in Cobol.
	' ***************************************************************** '
	Public Sub FormatPolicyNum(ByVal v_lInsurerNo As Integer, ByVal v_lNumber As Integer, ByVal v_sPrefix As String, ByVal v_sSuffix As String, ByVal v_sAgencyRef As String, ByVal v_dtEffectiveDate As Date, ByVal v_sQMMSuffix As String, ByRef r_sPolicyNum As String)
        Dim ACClass As Object = Nothing
		
		Try 
			
			
			Select Case v_lInsurerNo
				' MMA
				Case MMAInsurerNo
					r_sPolicyNum = v_sPrefix.Trim() & v_sAgencyRef.Trim() & StringsHelper.Format(v_lNumber, "000000")
					
					' NU
				Case NUInsurerNo
					r_sPolicyNum = CStr(v_dtEffectiveDate.Year).Substring(CStr(v_dtEffectiveDate.Year).Length - 2) & "3" & StringsHelper.Format(DateTime.Now.Month, "#0") & v_sPrefix.Trim() & StringsHelper.Format(v_lNumber, "00000")
					
					' NIG
				Case NIGInsurerNo
					'r_sPolicyNum = Trim$(v_sPrefix) & Format$(v_lNumber, "00000000")
					r_sPolicyNum = v_sPrefix.Trim() & StringsHelper.Format(v_lNumber, "000000")
					
					' PEGASUS
				Case PEGASUSInsurerNo
					r_sPolicyNum = v_sPrefix.Trim() & StringsHelper.Format(v_lNumber, "00000000")
					
					' LINK
				Case LINKInsurerNo
					'r_sPolicyNum = Trim$(v_sPrefix) & Format$(v_lNumber, "00000000")
					r_sPolicyNum = v_sPrefix.Trim() & StringsHelper.Format(v_lNumber, "000000")
					
					' Cornhill
				Case CORNHILLInsurerNo
					r_sPolicyNum = v_sPrefix.Trim() & StringsHelper.Format(v_lNumber, "000000")
					
					' Anyone Else
					'CJB270701 Change format for certain QMM insurers (that don't
					'generate a policy no. in the Cobol as part of the transact) to
					'format the no. as YYYYMMDD + Unique Number + QMM Suffix
				Case Else
					'r_sPolicyNum = "POL" & Format$(v_lNumber, "00000000") 'old format before 270701 change
					r_sPolicyNum = CStr(DateTime.Now.Year) & StringsHelper.Format(DateTime.Now.Month, "00") & StringsHelper.Format(DateAndTime.Day(DateTime.Now), "00") & StringsHelper.Format(v_lNumber, "0000") & v_sQMMSuffix
			End Select
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message

			GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatPolicyNum Failed", vApp:=ACApp, vClass:=CStr(ACClass), vMethod:="FormatPolicyNum", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
		End Try
		
	End Sub
End Module