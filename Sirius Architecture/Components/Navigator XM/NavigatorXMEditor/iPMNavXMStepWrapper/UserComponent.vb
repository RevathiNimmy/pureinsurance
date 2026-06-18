Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Runtime.ExceptionServices
<System.Runtime.InteropServices.ProgId("UserComponent_NET.UserComponent")> _
Public NotInheritable Class UserComponent 
	
	  Implements IDisposable
Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_lStatus As Integer
	
	Private m_oMSScriptControl As MSScriptControl.ScriptControl
	
	' Step variables
	Private m_sScriptFilename As String = ""
	Private m_sScriptStartMethod As String = ""
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_sNavigatorTitle As String = ""
	
	Private m_vKeyArray As Array
	
	Private Const PMKeyScriptReturnValue As String = "ScriptReturnValue"
	
Private Const ACClass As String = "UserComponent" 
	
	' Step properties
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public Sub New()
		MyBase.New()
	End Sub
	
	Protected Overrides Sub Finalize()
		Dispose(False)
	End Sub

	
	Public Function Initialise() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			m_lReturn = CType(GetObjectManager(), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to create ObjectManager", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
 Private disposedValue As Boolean
	Public Sub Dispose() Implements IDisposable.Dispose
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub


	Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
		Me.disposedValue = True
	End Sub

	
	Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the process modes to the property members.
			

            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function SetKeys(ByRef vKeyArray As Array) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return result
            End If

            ' Copy the key array so that it can be passed to the script
            m_vKeyArray = vKeyArray

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)



                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameScriptFilename

                        m_sScriptFilename = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameScriptStartMethod

                        m_sScriptStartMethod = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                End Select

            Next


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
	
	Public Function GetSummary(ByRef vSummaryArray As Object) As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' user code here
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
Public Function GetKeys(ByRef vKeyArray(,) As Object ) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	<HandleProcessCorruptedStateExceptions>
	Public Function Start() As Integer
		Dim Err_Script As Boolean = False
		Dim Err_Start As Boolean = False
		
		Dim result As Integer = 0
        Dim lFH As Integer
		Dim sCode As String = ""
		Dim oControl As StatusControl
		
		Try 
			Err_Start = True
			Err_Script = False
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			m_oMSScriptControl = New MSScriptControl.ScriptControl()
			
			lFH = FileSystem.FreeFile()
			
			FileSystem.FileOpen(lFH, m_sScriptFilename, OpenMode.Input)
			
			sCode = FileSystem.InputString(lFH, FileSystem.LOF(lFH)).Trim()
			
			FileSystem.FileClose(lFH)
			
			If sCode = "" Then
				MessageBox.Show("Code file is empty", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				
				Return result
			End If
			
			m_oMSScriptControl.Language = "VBScript"
			
			' error handler that reports script error details
			Err_Script = True
			Err_Start = False
			
			oControl = New StatusControl()
			
			oControl.KeyArray = m_vKeyArray
			
			m_oMSScriptControl.AddObject("ScriptControl", oControl, True)
			
			m_oMSScriptControl.AddCode(sCode)
			
			m_oMSScriptControl.Run(m_sScriptStartMethod)
			
			m_oMSScriptControl = Nothing
			
			m_lStatus = oControl.Status
			m_vKeyArray = oControl.KeyArray
			
			oControl = Nothing
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			If Not Err_Script And Not Err_Start Then
				Throw excep
			End If
			
			If Err_Start Then
				
				
				result = gPMConstants.PMEReturnCode.PMError
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
				
				Return result
				


				
			End If
			If Err_Script Or Err_Start Then
				
				
				result = gPMConstants.PMEReturnCode.PMError
				
				With m_oMSScriptControl.Error
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=.Description & " at line " & CStr(.Line) & " column " & CStr(.Column), excep:=excep)
				End With
				
				Return result
			End If
		End Try
	End Function
End Class
