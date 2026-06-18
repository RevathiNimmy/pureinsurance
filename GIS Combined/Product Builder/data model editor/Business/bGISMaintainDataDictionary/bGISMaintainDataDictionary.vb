Option Strict Off
Option Explicit On
Imports SSP.Shared
'refer Developer Guide No. 129
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
    ' Date:  07/05/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' RAW 02/09/2003 : CQ2158 : : added functions to build "ON DELETE CASCADE" text - but only if DB supports it
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bGISMaintainDataDictionary"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"






    'TF021002
    'Constant to define top level GIS Object
    Public Const ACPolicyBinder As Integer = 0
    'Constants to define additional GIS Objects for Broking
    Public Const ACOutputDetails As Integer = -1
    Public Const ACOutputEndorsements As Integer = -2
    Public Const ACOutputExcess As Integer = -3
    Public Const ACOutputFees As Integer = -4
    Public Const ACPolicy As Integer = -5
    Public Const ACParty As Integer = -6
    Public Const ACClaims As Integer = -7
    Public Const ACPeril As Integer = -8

    '****************
    ' MEvans : 23-07-2003 : 223 Document Production
    Public Const ACClaimsOutput As Integer = -9
    Public Const ACClaimsDocFolder As Integer = -10
    Public Const ACClaimsDocRequest As Integer = -11
    '****************

    'MKW 250606 - Datasure Section Changes
    Public Const ACOutputSections As Integer = -12
    Public Const ACOutputSectionsCoinsurers As Integer = -13
    'Plico24-28
    Public Const ACCaseGeneral As Integer = -14
    Public Const ACRiskS4IDefault As Integer = -15

    Public Const ACOutputPremiumBreakdown As Integer = -16
    Public Const ACOutputCommission As Integer = -17
    Public Const ACOutputReferrals As Integer = -18
    Public Const ACOutputReferralsAudit As Integer = -19

    '*************
    ' MEvans : 08-11-2003 : CQ3049
    Public Const ACSPGenDataTempDataModelName As String = "XXXDATAMODELNAME"
    Public Const ACSPGenDataTempDataModelTableName As String = "XXXDATAMODELTABLENAME"
    Public Const ACSPGenDataTempDataModelTableAlias As String = "XXXDATAMODELTABLEALIAS"

    ' RAW 02/09/2003 : CQ2158 : moved from business class and renamed with 'g' prefix
    Private g_lSQLServerVersion As Integer


    Sub Main_Renamed()


    End Sub

    ' ***************************************************************** '
    ' Name: GetSQLServerVersion
    '
    ' Description:
    '
    ' History:
    ' RAW 02/09/2003 : CQ2158 : Created.
    ' ***************************************************************** '
    Public Function GetSQLServerVersion(ByRef r_oDatabase As dPMDAO.Database) As Integer
        Dim vSQLSvrVersion(,) As Object = Nothing
        Dim nPosition As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            If g_lSQLServerVersion <= 0 Then 'just read once

                g_lSQLServerVersion = -1

                lReturn = r_oDatabase.SQLSelect(sSQL:=ACMSGetVersionSQL, sSQLName:=ACMSGetVersionName, bStoredProcedure:=ACMSGetVersionStored, vResultArray:=vSQLSvrVersion)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Function
                End If


                nPosition = (CStr(vSQLSvrVersion(0, 0)).IndexOf("."c) + 1)


                g_lSQLServerVersion = CInt(CStr(vSQLSvrVersion(0, 0)).Substring(0, nPosition - 1))

            End If

            Return g_lSQLServerVersion

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSQLServerVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSQLServerVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDeleteCascadeText
    '
    ' Description:
    '
    ' History:
    ' RAW 02/09/2003 : CQ2158 : Created.
    ' ***************************************************************** '
    Public Function GetDeleteCascadeText() As String

        'Need to know SQL Server version to see Cascade Delete is supported
        If g_lSQLServerVersion > 7 Then
            Return " ON DELETE CASCADE "
        Else
            Return ""
        End If

    End Function
End Module
