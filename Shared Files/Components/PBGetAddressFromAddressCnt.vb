Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("PBGetAddressFromAddressCnt_NET.PBGetAddressFromAddressCnt")> _
 Public Module PBGetAddressFromAddressCnt
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	Private Const ACClass As String = "PBGetAddressFromAdressCnt"
	
	' Select Address SQL
	Public Const ACGetAddressStored As Boolean = False
	Public Const ACGetAddressName As String = "SelectAddress"
	Public Const ACGetAddressSQL As String = ""
	
	
	' ***************************************************************** '
	'
	' Name: GetAddressFromAddressCnt
	'
	' Description:
	'
	' History: 25/08/2000 Tomo - Created as GetAddress.
	'
	' ***************************************************************** '
    Public Function GetAddressFromAddressCnt(ByRef r_oDatabase As Object, ByVal lAddressCnt As String, ByRef vAddressArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            vAddressArray = Nothing

            sSQL = ""
            sSQL = sSQL & "SELECT a.address_cnt, a.address1, a.address2, a.address3, a.address4, a.postal_code, c.description, a.address_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "FROM address a, country c" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "WHERE a.country_id = c.country_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "AND a.address_cnt = " & lAddressCnt & Strings.Chr(13) & Strings.Chr(10)

            r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAddressName, bStoredProcedure:=ACGetAddressStored, vResultArray:=vAddressArray)

            'If the postal_code has the same in it as the address_cnt then we don't have a postal_code
            If Information.IsArray(vAddressArray) Then


                If CStr(vAddressArray(5, 0)) = CStr(vAddressArray(7, 0)) Then

                    vAddressArray(5, 0) = ""
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAddressFromAddressCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressFromAddressCnt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
