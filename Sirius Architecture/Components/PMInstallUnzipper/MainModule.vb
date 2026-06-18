Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ************************************************
	' Added to replace global variables 19/09/2003
	' Username.
	Private m_sUsername As String = ""
	
	' Password.
	Private m_sPassword As String = ""
	
	' User ID
	Private m_iUserID As Integer
	
	' Calling Application
	Private m_sCallingAppName As String = ""
	' Source ID
	Private m_iSourceID As Integer
	' Language ID
	Private m_iLanguageID As Integer
	' Currency ID
	Private m_iCurrencyID As Integer
	' LogLevel
	Private m_iLogLevel As Integer
	' ************************************************
	
	
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	''Public constants
	'***********************************************
	
	' Product names passed from install program
	' used in program to set product specific parms
	Public Const PRODUCT_CODE_SA As String = "Sirius"
	Public Const PRODUCT_CODE_DME As String = "DocuMaster"
	Public Const PRODUCT_CODE_SBO As String = "SirSol"
	
	' Product folders (under \\PMSetup)
	Public Const PRODUCT_FOLDER_SA As String = "SA"
	Public Const PRODUCT_FOLDER_DME As String = "DocuMaster"
	Public Const PRODUCT_FOLDER_SBO As String = "Back-office"
	
	' Product setup program
	Public Const PRODUCT_SETUP_PROGRAM_SA As String = "setup.exe"
	Public Const PRODUCT_SETUP_PROGRAM_DME As String = "setup.exe"
	Public Const PRODUCT_SETUP_PROGRAM_SBO As String = "setup.exe"
	
	Public Const ACApp As String = "PMInstallZipper"
	
	
	' program starts here

	Public Sub Main()
		
		
		Dim oInterface As New frmInterface
		

		oInterface.Start()
		
		oInterface.Close()
		
		oInterface = Nothing
		
	End Sub
End Module