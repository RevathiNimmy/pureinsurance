Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'Modified by Sudhanshu Behera on 5/28/2010 4:30:28 PM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	'Public Const ACApp = "bSIRLaunchAutoRenewalsAccept"
	Public Const ACApp As String = "bSIRRenewalAcceptAgentEmail"
	
	
	Private m_sUsername As String = ""
	Private m_sPassword As String = ""
	Private m_iUserID As Integer
	Private m_sCallingAppName As String = ""
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_iCurrencyID As Integer
	Private m_iLogLevel As Integer
	
	Private m_oBatch As bSIRRenewal.Business
	Private m_lReturn As Integer
	
	Public Sub Main()
		
		Try 
			
			m_sUsername = "sirius"
			m_sPassword = "sirius"
			m_iUserID = 1
			m_sCallingAppName = ACApp
			m_iSourceID = 1
			m_iLanguageID = 1
			m_iCurrencyID = 26
			m_iLogLevel = 6
			
			m_oBatch = New bSIRRenewal.Business()
			
			m_lReturn = CType(m_oBatch, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
			
			
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBatch.Initialise failed.", vApp:=ACApp, vClass:="mainModule", vMethod:="Main", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Exit Sub
			End If
			
			
			m_lReturn = m_oBatch.GenerateAgentRenewalEmail("acceptance")
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBatch.Start failed.", vApp:=ACApp, vClass:="mainModule", vMethod:="Main", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Exit Sub
			End If
			
		m_oBatch.Dispose()
		 
			
			
			m_oBatch = Nothing
		
		Catch excep As System.Exception
			
			
			bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Sub Main failed.", vApp:=ACApp, vClass:="mainModule", vMethod:="Main", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
End Module
