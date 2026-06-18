Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Public Module PBReadScriptColumn
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PBReadScriptColumns
	'
	' Date: 19/02/2001
	'
	' Description: Contains the function that reads "text" risk script columns
	'
	' Edit History:
	' ***************************************************************** '
	Private Const ACClass As String = "PBReadScriptColumn"
	
	'Column name constants for the Screen Header script columns
	Public Const ACNScriptDefaults As String = "script_defaults"
	Public Const ACNScriptDynamicLogic As String = "script_dynamic_logic"
	Public Const ACNScriptQuote As String = "script_quote"
	Public Const ACNScriptUAL As String = "script_UAL"
	
	' Select Screen Header Script Column
	Public Const ACScreenScriptColumnStored As Boolean = True
	Public Const ACScreenScriptColumnName As String = "SelectScreenScriptColumn"
    Public Const ACScreenScriptColumnSQL As String = "spu_PB_script_sel"
	
	
	' ***************************************************************** '
	'
	' Name: ReadScriptColumn
	'
	' Description:
	'
	' History: 19/02/2002 CLG - Created.
	'
	' ***************************************************************** '
    Public Sub ReadScriptColumn(ByRef r_oDatabase As Object, ByVal v_lScreenId As Integer, ByRef r_vScreenHeader As Object, ByVal iHeaderColumnOffset As Integer, ByVal v_sColumnName As String)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".ReadScriptColumn")

        Try

            Dim sText As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode

            ' Clear the Database Parameters Collection
            r_oDatabase.Parameters.Clear()

            'Add the screen
            lReturn = r_oDatabase.Parameters.Add(sName:="key", vValue:=CStr(v_lScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            lReturn = r_oDatabase.Parameters.Add(sName:="required_column", vValue:=v_sColumnName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If



            lReturn = r_oDatabase.SQLSelectTextField(sSQL:=ACScreenScriptColumnSQL, sSQLName:=ACScreenScriptColumnName, bStoredProcedure:=ACScreenScriptColumnStored, sTextData:=sText)

            'set value (to null string) even if not read!

            r_vScreenHeader(iHeaderColumnOffset, 0) = sText

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".ReadScriptColumn")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".ReadScriptColumn")


            ' Log Error Message
            bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReadScriptColumn Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReadScriptColumn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
End Module
