Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Public Module TempFunc
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ***************************************************************** '
	' Name: LogMessage
	'
	' Description: Wrapper function to the log message method of the
	'              message object.
	'
	' RAW 20/06/2003 : removed the guts from RetrieveSingleSystemOption
	' ***************************************************************** '
	Public Sub LogMessage(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing)
        Try
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)))
        Catch
            Exit Sub
        End Try
	End Sub
	
	
	' ***************************************************************** '
	' Name: RetrieveSingleSystemOption
	'
	' Description:  gets the system option required for the current branch
	'               held in g_iSourceID
	'
	' History: SW 07/04/2003
	'
	' PSL 24/04/2003 Added to Allow compile
	' ***************************************************************** '
	Public Function RetrieveSingleSystemOption(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iMainSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, ByRef v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef v_iSourceID As Integer = 0) As Integer
		
		
		Dim result As Integer = 0
		Dim oSystemOptions, oObjectManager As Object
		Dim lResult As Integer
		
		
		Try
		
		result = gPMConstants.PMEReturnCode.pmtrue
		
		' RAW 20/06/2003 : This code has been removed because it introduced compatibility errors
		' with cGISDataSetControl due to a reference to Object Manager being added - this
		' reference has now been removed to restore compat.
		' This function is used in common components (ie used at client or server) so it needs
		' to be more intelligent as to how to create the bSIROptions object.
		' At the moment it only works for client-side.
		' For now, time is short, so we will simply return False and address a proper fix later
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		'    ' Create an instance of the object manager and Initialise
		'    Set oObjectManager = New bObjectManager.ObjectManager
		'
		'    lResult = oObjectManager.Initialise( _
		''            sCallingAppName:=ACApp)
		'
		'    '   If not initialised then call error handler
		'    If lResult <> pmtrue Then
		'        RetrieveSingleSystemOption = PMFalse
		'        Set oObjectManager = Nothing
		'        Exit Function
		'    End If
		'
		'    '   Find the Business Class
		'    lResult = oObjectManager.GetInstance( _
		''            oobject:=oSystemOptions, _
		''            sclassname:="bSIROptions.Business", _
		''            vInstanceManager:=PMGetViaClientManager)
		'
		'    If lResult <> pmtrue Then
		'        RetrieveSingleSystemOption = PMFalse
		'        Set oObjectManager = Nothing
		'        Exit Function
		'    End If
		'
		'    'get the system option
		'    lResult = oSystemOptions.GetOption(iOptionNumber:=v_iOptionNumber, _
		''                                         sValue:=r_sOptionValue, _
		''                                         v_iSourceID:=v_iSourceID)
		'
		'    If lResult <> pmtrue Then
		'        RetrieveSingleSystemOption = PMFalse
		'        oSystemOptions.Terminate
		'        Set oSystemOptions = Nothing
		'        oObjectManager.Terminate
		'        Set oObjectManager = Nothing
		'        Exit Function
		'    End If
		'
		'    lResult = oSystemOptions.Terminate
		'
		'    If lResult <> pmtrue Then
		'        RetrieveSingleSystemOption = PMFalse
		'        Set oSystemOptions = Nothing
		'        oObjectManager.Terminate
		'        Set oObjectManager = Nothing
		'        Exit Function
		'    End If
		'
		'    Set oSystemOptions = Nothing
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		' sw 07/04/2003: Raise the error to the calling function so it can be logged correctly
		Throw New System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().Source + ", " + Information.Err().Description + ", " + Information.Err().HelpFile + ", " + Information.Err().HelpContext)
		
		Finally
		
		
		End Try
		Return result
    End Function


    ' ***************************************************************** '
    ' Name: LogMessageFile
    '
    ' Description: Wrapper function to the log message method of the
    '              message object.
    '
    ' ***************************************************************** '
    Public Sub LogMessageFile(ByVal iType As Integer, ByVal sMsg As String, ByVal sUsername As String, Optional ByVal vApp As Object = Nothing, Optional ByVal vClass As Object = Nothing, Optional ByVal vMethod As Object = Nothing, Optional ByVal vErrNo As Object = Nothing, Optional ByVal vErrDesc As Object = Nothing)
        Try
            LogMessageToFile(sUsername:=sUsername, iType:=iType%, sMsg:=sMsg$, vApp:=vApp, vClass:=vClass, vMethod:=vMethod, excep:=New Exception(vErrDesc))
            Exit Sub
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

End Module
