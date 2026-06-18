Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
'developer guide no . 129
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  10/06/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bGISSchemeBusiness"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Con
    Public Const PARAMETER_NOT_PRESENT_NO As Integer = -32767
    Public Const PARAMETER_NOT_PRESENT_STR As String = "-32727"

    Public Const PARAMETER_POS_NAME As Integer = 0
    Public Const PARAMETER_POS_VALUE As Integer = 1



    ' UserID

    'sj 08/08/2001 - start
    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    '***********************************************
    'sj 08/08/2001 - end

    Sub Main_Renamed()

        ' Main entry point for the component

    End Sub

    Public Function AddParameter(ByVal v_oDatabase As dPMDAO.Database, ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_lParamDirection As gPMConstants.PMEParameterDirection, ByVal v_lParamDataType As gPMConstants.PMEDataType, Optional ByVal v_bAddNullIfMissing As Boolean = False, Optional ByVal v_vDataMissing As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sIncorrectType As String = ""

        Try


            If Not Informations.IsNothing(v_vDataMissing) Then


                If v_vValue.Equals(v_vDataMissing) Then

                    If Not v_bAddNullIfMissing Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Value for parameter " & v_sName & " is missing!", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParameter")

                        Return result

                    Else



                        v_vValue = DBNull.Value

                    End If

                End If

            End If

            If v_lParamDataType <> gPMConstants.PMEDataType.PMString Then
                If TypeOf v_vValue Is String Then

                    If CStr(v_vValue) = "" Then


                        v_vValue = DBNull.Value
                    End If
                End If
            End If


            If Not (Convert.IsDBNull(v_vValue) Or Informations.IsNothing(v_vValue)) Then

                Select Case v_lParamDataType
                    Case gPMConstants.PMEDataType.PMCurrency, gPMConstants.PMEDataType.PMDecimal, gPMConstants.PMEDataType.PMDouble, gPMConstants.PMEDataType.PMInteger, gPMConstants.PMEDataType.PMLong

                        Dim dbNumericTemp As Double
                        If Not Double.TryParse(CStr(v_vValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Parameter for " & v_sName & " is not numeric", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParameter")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Case gPMConstants.PMEDataType.PMDate
                        If Not Informations.IsDate(v_vValue) Then
                            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Parameter for " & v_sName & " is not a date", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParameter")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                End Select
            End If


            If v_oDatabase.Parameters.Add(v_sName, CStr(v_vValue), v_lParamDirection, v_lParamDataType) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParameter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParameter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function CheckArrayDimension(ByVal v_vArray(,) As Object, ByVal v_iCheckDimension As Integer) As Integer

        Dim result As Integer = 0
        Try

            If Not Informations.IsArray(v_vArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Parameter must be an array", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckArrayDimension")
                Return result
            End If

            If CheckDimension(v_vArray, v_iCheckDimension) <> gPMConstants.PMEReturnCode.PMTrue Or CheckDimension(v_vArray, v_iCheckDimension + 1) <> gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Array not the check number of dimensions", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractParameterValues")
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckArrayDimension Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckArrayDimension", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function CheckDimension(ByVal v_vArray(,) As Object, ByVal v_iCheckDimension As Integer) As Integer

        Dim iLBound As Integer

        Try

            iLBound = v_vArray.GetLowerBound(v_iCheckDimension - 1)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function
End Module
