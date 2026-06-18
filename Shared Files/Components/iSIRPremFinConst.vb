Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("iSIRPremFinConst_NET.iSIRPremFinConst")> _
 Public Module iSIRPremFinConst
	'INTERFACE CONSTANTS
	'Used to receive available schemes
	Public m_vResultArray As Object
	
	Public m_sProductName As String = ""
	Public m_sTransactionType As String = ""
	'array with product class codes in
	Public m_vProductClassCodes As Object
	
	'Used for printing of quote(s) / accept a quote process
	Public m_vBusinessArray As Object
	'Used for selecting schemes to be quoted against
	Public m_vSchemeArray As Object
	
	'Used for the selection of one plan
	'When client has more than one plan on PFPremiumFinance
	Public g_vPlanSelectionArray As Object
	
	'Used for keeping a track of what plans/quotes have been selected
	Public m_vRowSelectArray As Object
	Public m_iRowNumber As Integer
	
	'AK 310800 - to flag incomplete policies not to be marked as complete
	Public g_bComplete As Boolean
	'AK 030900
	Public g_bCancel As Boolean
	
	Public m_bDisplayResult As Boolean
	Public m_bCheckPrint As Boolean
	Public oPMPremFinance As Object
    Public oObjectManager As Object
    'Modified by Deepak Sharma on 4/20/2010 3:10:00 PM refer developer guide no. 101 (Guide)
    'Public g_oParent As ClassInterface
    Public g_oParent As Object
	Public g_bDisplayClientDetails As Boolean
	
	
	'declarations
	Public m_sTransType As String = ""
	
	Public g_iClientId As Integer
	Public g_sClientCode As String = ""
	Public g_sInsuranceRef As String = ""
	Public g_sSystemTag As String = ""
	Public g_vInsuranceFileCnt As Object
	
	Public Const g_cInsurerType As Integer = 7
	Public g_bEFINPRO As Boolean
	Public m_bCalculate As Boolean
	
	
	
	
	' ***************************************************************** '
	' Name: GetProductClass
	'
	' Description:  Converts the Product Class code into a useful(!) description
	'
	' History:
	'   PF210901 - Created.
	' ***************************************************************** '
	Public Function GetProductClass(ByVal sProductClass As String) As String
		Dim result As String = String.Empty
        Dim ACClass As Object = Nothing
		
		Try 
			
			Select Case sProductClass
				Case "N"
					result = "All New Business"
				Case "C"
					result = "Commercial New Business"
				Case "P"
					result = "Personal New Business"
				Case "R"
					result = "All Renewal"
				Case "M"
					result = "Commercial Renewal"
				Case "L"
					result = "Personal Renewal"
				Case "A"
					result = "All New Business/Renewal"
				Case "I"
					result = "Commercial New Business/Renewal"
				Case "W"
					result = "Personal New Business/Renewal"
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = ""
			
			' Log Error Message

			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductClass Failed", vApp:=ACApp, vClass:=CStr(ACClass), vMethod:="GetProductClass", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetStatusText
	'
	' Description:  Converts the Status Indicator into a useful(!) description
	'
	' History:
	'   DD170801 - Created.
	'   PF210901 - Replaced codes with constant references
	' ***************************************************************** '
	Public Function GetStatusText(ByVal sStatusInd As String) As String
		Dim result As String = String.Empty
        Dim ACClass As Object = Nothing
		
		Try 
			
			
			Select Case sStatusInd
				Case bSIRPremFinConst.PFStatusIndSaved
					Return "Saved"
				Case bSIRPremFinConst.PFStatusIndUpdated
					Return "Updated"
				Case bSIRPremFinConst.PFStatusIndQuotePrinted
					Return "Quoted Printed"
				Case bSIRPremFinConst.PFStatusIndLive
					Return "Live"
				Case bSIRPremFinConst.PFStatusIndOnHold
					Return "On Hold"
				Case bSIRPremFinConst.PFStatusIndCompleted
					Return "Completed"
				Case bSIRPremFinConst.PFStatusIndSuperseded
					Return "Superceded"
				Case bSIRPremFinConst.PFStatusIndCancelled
					Return "Cancelled"
				Case Else
					Return "#Unknown#"
			End Select
		
		Catch 
		End Try
		
		
		
		result = ""
		
		' Log Error Message

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStatusText Failed", vApp:=ACApp, vClass:=CStr(ACClass), vMethod:="GetStatusText", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

		Return result
		
	End Function
	
	
	' ***************************************************************** '
	' Name: GetInstalmentStatus
	'
	' Description:  Converts the instalment status into useful description
	'
	' History:
	'   PF240901 - Created
	' ***************************************************************** '
	Public Function GetInstalmentStatus(ByVal sStatus As String) As String
		Dim result As String = String.Empty
        Dim ACClass As Object = Nothing
		
		Try 
			
			
			Select Case sStatus
				Case CStr(bSIRPremFinConst.PFStatusNew)
					Return "New"
				Case CStr(bSIRPremFinConst.PFStatusPending)
					Return "Pending"
				Case CStr(bSIRPremFinConst.PFStatusCollected)
					Return "Collected"
				Case CStr(bSIRPremFinConst.PFStatusManual)
					Return "Manual"
				Case CStr(bSIRPremFinConst.PFStatusRetrying)
					Return "Retrying"
				Case CStr(bSIRPremFinConst.PFStatusFailed)
					Return "Failed"
				Case CStr(bSIRPremFinConst.PFStatusHold)
					Return "Hold"
				Case CStr(bSIRPremFinConst.PFStatusWriteOff)
					Return "Write Off"
				Case CStr(bSIRPremFinConst.PFStatusTransferred)
					Return "Transferred"
				Case Else
					Return "#Unknown#"
			End Select
		
		Catch 
		End Try
		
		
		
		result = ""
		
		' Log Error Message

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstalmentStatus Failed", vApp:=ACApp, vClass:=CStr(ACClass), vMethod:="GetInstalmentStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
End Module