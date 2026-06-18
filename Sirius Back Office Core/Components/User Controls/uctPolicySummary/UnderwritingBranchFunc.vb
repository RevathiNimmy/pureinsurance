Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Module UnderwritingBranchFunc

    Private Const ACClass As String = "UnderwritingBranchFunc"
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    ' Name: CheckForUnderwritingBranch
    '
    ' Description:
    '
    ' History: 18/02/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function CheckForUnderwritingBranch(ByVal v_iSourceId As Integer, Optional ByRef r_bUnderwritingBranchEnabled As Boolean = False, Optional ByRef r_bIsUnderwritingBranch As Boolean = False) As Integer
        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vValue As String = ""

            Dim oPMSource As bPMSource.Business
            Dim vUnderwritingBranchInd As String = ""

            'iSourceId = g_oObjectManager.SourceId

            r_bUnderwritingBranchEnabled = False
            r_bIsUnderwritingBranch = False

            m_lReturn = CType(iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTUnderwritingBranchEnabled, v_vBranch:=v_iSourceId, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTUnderwritingBranchEnabled, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForUnderwritingBranch")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Conversion.Val(vValue) = 1 Then
                r_bUnderwritingBranchEnabled = True

                'Get bPMSource
                Dim temp_oPMSource As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oPMSource, "bPMSource.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oPMSource = temp_oPMSource
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of PMSource", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForUnderwritingBranch")
                    oPMSource = Nothing
                    Return result
                End If


                m_lReturn = oPMSource.GetDetails(vSourceId:=v_iSourceId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oSource.GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForUnderwritingBranch")
                    oPMSource = Nothing
                    Return result
                End If


                m_lReturn = oPMSource.GetNext(vUnderwritingBranchInd:=vUnderwritingBranchInd)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oSource.GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForUnderwritingBranch")
                    oPMSource = Nothing
                    Return result
                End If

                If Conversion.Val(vUnderwritingBranchInd) = 1 Then
                    r_bIsUnderwritingBranch = True
                End If


                oPMSource.Dispose()
                oPMSource = Nothing

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForUnderwritingBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForUnderwritingBranch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
End Module