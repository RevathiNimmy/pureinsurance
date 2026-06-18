Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("bACTFunc_NET.bACTFunc")> _
 Public Module bACTFunc
 
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ************************************************************************* '
	'
	' Orion Business general functions module. Contains all of the global
	' functions that might be useful when writing the business layer.
	'
	' History:
	'       DD 08/08/2002: Rebuilt from ACTFunc.
	'
	' ************************************************************************* '
	
	Dim m_lReturn As Integer
	
	Private Const ACClass As String = "bACTFunc"
	
	' ---------------------------------------------------------------------------
	' PROCEDURE NAME: GetLedgerIDFromShortName
	' PURPOSE: Returns the LedgerID for a ShortName within a Sub Branch
	' AUTHOR: Raj Chanian
	' DATE: 22/08/2002, 10:30
	' RETURNS: PMTrue for success
	' CHANGES:
	' ---------------------------------------------------------------------------
    Public Function GetLedgerIDFromShortName(ByVal v_oDatabase As Object, ByRef r_lLedgerID As Integer, ByVal v_sShortName As String, ByVal v_lSubBranchID As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

        With v_oDatabase
            .Parameters.Clear()
            .Parameters.Add("ShortName", v_sShortName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            .Parameters.Add("sub_branch_id", CStr(v_lSubBranchID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Modified by Deepak Sharma on 4/20/2010  4:43:18 PM refer developer guide no. 85(Guide)
            '.Parameters.Add("ledgerid", CStr(DBNull.Value), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("ledgerid", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'result = .SQLAction("{ call spu_ACT_Get_LedgerID_From_ShortName (?,?,?) }", "Get Ledger ID", True)
            result = .SQLAction("spu_ACT_Get_LedgerID_From_ShortName", "Get Ledger ID", True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            Else
                r_lLedgerID = .Parameters.Item("ledgerid").Value
            End If
        End With


        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
         

        Catch ex As Exception
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                ' PSL 07/10/2003 Added Username Parameter (blank) - for globals change
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetLedgerIDFromShortName"), vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMFalse

        End Select

        Finally
       


        End Try
	Return result
    End Function

    '
    Public Function GetLedgerID(ByVal v_oDatabase As Object, ByRef r_lLedgerID As Integer, ByVal v_sLedgerCode As String, ByVal v_lSubBranchID As Integer) As gPMConstants.PMEReturnCode
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetLedgerID
        ' PURPOSE: Returns the LedgerID for a Ledger Code within a Sub Branch
        ' AUTHOR: Danny Davis
        ' DATE: 08/08/2002, 12:32
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

        With v_oDatabase
            .Parameters.Clear()
            .Parameters.Add("ledger_code", v_sLedgerCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            .Parameters.Add("sub_branch_id", CStr(v_lSubBranchID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Modified by Deepak Sharma on 4/20/2010 4:43:44 PM refer developer guide no. 85(Guide)
            '.Parameters.Add("ledger_id", CStr(DBNull.Value), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("ledger_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'result = .SQLAction("{call spu_ACT_Get_LedgerID_From_ShortName (?,?,?)", "Get Ledger ID", True)
            result = .SQLAction("spu_ACT_Get_LedgerID_From_ShortName", "Get Ledger ID", True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            Else
                r_lLedgerID = .Parameters.Item("ledger_id").Value
            End If
        End With
 


	Catch ex As Exception
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                ' PSL 07/10/2003 Added Username Parameter (blank) - for globals change
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetLedgerID"), vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMFalse


        End Select

	Finally 
	End Try
	
        Return result


    End Function







    'Modified by Deepak Sharma on 4/20/2010 5:20:52 PM refer developer guide no. 101(Guide)
    'Public Function GetSubBranchID(ByVal v_oDatabase As Object, ByRef r_lSubBranchID As Integer, Optional ByVal v_vAccountID As String = DBNull.Value, Optional ByVal v_vTransDetailID As String = DBNull.Value, Optional ByVal v_vPeriodID As String = DBNull.Value, Optional ByVal v_vBankAccountID As String = DBNull.Value, Optional ByVal v_vPartyCnt As String = DBNull.Value, Optional ByVal v_vSourceID As String = DBNull.Value) As Integer
    Public Function GetSubBranchID(ByVal v_oDatabase As Object, ByRef r_lSubBranchID As Integer, Optional ByVal v_vAccountID As Object = Nothing, Optional ByVal v_vTransDetailID As Object = Nothing, Optional ByVal v_vPeriodID As Object = Nothing, Optional ByVal v_vBankAccountID As Object = Nothing, Optional ByVal v_vPartyCnt As Object = Nothing, Optional ByVal v_vSourceID As Object = Nothing) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSubBranchID
        ' PURPOSE: Returns the SubBranchID for different areas of Orion. Used by
        ' multi-branch accounting to determine the correct period in which a
        ' record will fall.
        ' AUTHOR: Danny Davis
        ' DATE: 05/08/2002, 11:27
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

        With v_oDatabase
            .Parameters.Clear()

            'Modified by Deepak Sharma on 4/20/2010 4:44:22 PM refer developer guide no. 85(Guide)
            '.Parameters.Add("sub_branch_id", CStr(DBNull.Value), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("sub_branch_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("account_id", v_vAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("transdetail_id", v_vTransDetailID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("period_id", v_vPeriodID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("bankaccount_id", v_vBankAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("party_cnt", v_vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("Source_id", v_vSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'result = .SQLAction("{call spu_ACT_Get_Sub_Branch_id (?,?,?,?,?,?,?)}", "Get Sub Branch ID", True)
            result = .SQLAction("spu_ACT_Get_Sub_Branch_id", "Get Sub Branch ID", True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            Else
                r_lSubBranchID = gPMFunctions.NullToLong(.Parameters.Item("sub_branch_id").Value)
            End If
        End With


        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
         

        Catch ex As Exception
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                ' PSL 07/10/2003 Added Username Parameter (blank) - for globals change
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetSubBranchID"), vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMFalse

        End Select

        Finally
       


        End Try
	 Return result
    End Function
End Module
