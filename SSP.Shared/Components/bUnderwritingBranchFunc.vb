Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("bUnderwritingBranchFunc_NET.bUnderwritingBranchFunc")>
Public Module bUnderwritingBranchFunc

    Private Const ACClass As String = "bUnderwritingBranchFunc"

    Private Const ACGetPartyCntFromBrokerAbiIdStored As Boolean = True
    Private Const ACGetPartyCntFromBrokerAbiIdName As String = "GetPartyCntFromBrokerAbiId"
    'Modified by Deepak Sharma on 4/14/2010 4:26:36 PM refer developer guide no. 39 (Guide)
    'Private Const ACGetPartyCntFromBrokerAbiIdSQL As String = "{call spu_party_agent_broker_abi_id_sel (?)}"
    Private Const ACGetPartyCntFromBrokerAbiIdSQL As String = "spu_party_agent_broker_abi_id_sel"

    Private Const ACGISSchemeEDILinkSTSStored As Boolean = True
    Private Const ACGISSchemeEDILinkSTSName As String = "GISSchemeEDILinkSTS"
    'Modified by Deepak Sharma on 4/14/2010 4:26:36 PM refer developer guide no. 39 (Guide)
    'Private Const ACGISSchemeEDILinkSTSSQL As String = "{call spu_GIS_Scheme_EDI_Link_STS_sel (?,?)}"
    Private Const ACGISSchemeEDILinkSTSSQL As String = "spu_GIS_Scheme_EDI_Link_STS_sel"

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    ' Name: GetSchemeDetailsFromExternalSchemeNo
    '
    ' Description:
    '
    ' History: 02/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetSchemeDetailsFromExternalSchemeNo(ByVal v_sExternalSchemeNo As String, ByVal v_oDatabase As dPMDAO.Database, ByVal v_sUsername As String, Optional ByRef r_lGisSchemeId As Integer = 0, Optional ByRef r_lGisInsurerId As Integer = 0, Optional ByRef r_sAbi81Insurer As String = "", Optional ByRef r_lRiskCodeId As Integer = 0, Optional ByRef r_lRiskGroupId As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing

            r_lGisSchemeId = 0
            r_lGisInsurerId = 0
            r_sAbi81Insurer = ""

            v_oDatabase.Parameters.Clear()

            ' Add the external scheme no parameter
            m_lReturn = v_oDatabase.Parameters.Add(sName:="sExternalSchemeNo", vValue:=v_sExternalSchemeNo.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the policy start date parameter

            m_lReturn = v_oDatabase.Parameters.Add(sName:="dtPolicyStartDate", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = v_oDatabase.SQLSelect(sSQL:=ACGISSchemeEDILinkSTSSQL, sSQLName:=ACGISSchemeEDILinkSTSName, bStoredProcedure:=ACGISSchemeEDILinkSTSStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (vResultArray Is Nothing) Then

                r_lGisSchemeId = CInt(vResultArray(0, 0))

                r_lGisInsurerId = CInt(vResultArray(1, 0))

                r_sAbi81Insurer = CStr(vResultArray(2, 0)).Trim()

                r_lRiskGroupId = CInt(vResultArray(3, 0))

                r_lRiskCodeId = CInt(vResultArray(4, 0))
            Else
                'not found raise an error
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            If r_lGisSchemeId = 0 Or r_lGisInsurerId = 0 Or r_sAbi81Insurer = "" Or r_lRiskCodeId = 0 Or r_lRiskGroupId = 0 Then
                'Should always return a value
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemeDetailsFromExternalSchemeNo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeDetailsFromExternalSchemeNo", vErrNo:=Nothing, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPartyCntFromBrokerAbiId
    '
    ' Description:
    '
    ' History: 02/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetPartyCntFromBrokerAbiId(ByVal v_sBrokerAbiId As String, ByVal v_oDatabase As dPMDAO.Database, ByVal v_sUsername As String, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_sTradingName As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim vResultArray(,) As Object = Nothing

            r_lPartyCnt = 0
            r_sTradingName = ""

            v_oDatabase.Parameters.Clear()

            ' Add the party count parameter
            m_lReturn = v_oDatabase.Parameters.Add(sName:="broker_abi_id", vValue:=v_sBrokerAbiId.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = v_oDatabase.SQLSelect(sSQL:=ACGetPartyCntFromBrokerAbiIdSQL, sSQLName:=ACGetPartyCntFromBrokerAbiIdName, bStoredProcedure:=ACGetPartyCntFromBrokerAbiIdStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (vResultArray Is Nothing) Then

                r_lPartyCnt = CInt(vResultArray(0, 0))

                r_sTradingName = CStr(vResultArray(1, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyCntFromBrokerAbiId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyCntFromBrokerAbiId", vErrNo:=Nothing, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUnderwritingBranchDetails
    '
    ' Description:
    '
    ' History: 25/02/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetUnderwritingBranchDetails(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_oDatabase As Object, ByVal v_sCallingAppName As String, ByRef r_bUnderwritingBranchEnabled As Boolean, ByRef r_bIsUnderwritingBranch As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vValue As String = ""

            'Are we running the folgate branch acting as insurer solution
            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername, v_sPassword, v_iUserID, v_iSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTUnderwritingBranchEnabled, v_iSourceID, vValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for " & gPMConstants.SIRHiddenOptions.SIROPTUnderwritingBranchEnabled, vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingBranchDetails")
                Return result
            End If
            r_bUnderwritingBranchEnabled = (gPMFunctions.NullToString(vValue) = "1")

            If r_bUnderwritingBranchEnabled Then
                m_lReturn = CType(GetUnderwritingBranchInd(v_oDatabase:=v_oDatabase, v_iSourceID:=v_iSourceID, v_sUsername:=v_sUsername, r_bIsUnderwritingBranch:=r_bIsUnderwritingBranch), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUnderwritingBranchInd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingBranchDetails")
                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUnderwritingBranchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingBranchDetails", vErrNo:=Nothing, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUnderwritingBranchInd
    '
    ' Description:
    '
    ' History: 23/02/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetUnderwritingBranchInd(ByVal v_oDatabase As dPMDAO.Database, ByVal v_iSourceID As Integer, ByVal v_sUsername As String, ByRef r_bIsUnderwritingBranch As Boolean) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        r_bIsUnderwritingBranch = False

        sSQL = "SELECT underwriting_branch_ind FROM source WHERE source_id = " & v_iSourceID
        m_lReturn = v_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetUnderwritingBranchInd", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (vResultArray Is Nothing) Then

            If Val(CStr(vResultArray(0, 0))) = 1 Then
                r_bIsUnderwritingBranch = True
            End If
        End If

        Return result



    End Function

    'Public Function Val(ByVal value As String) As Double
    '    Dim result As String = String.Empty

    '    For Each c As Char In value

    '        If Char.IsNumber(c) OrElse (c.Equals("."c) AndAlso result.Count(Function(x) x.Equals("."c)) = 0) Then
    '            result += c
    '        ElseIf Not c.Equals(" "c) Then
    '            Return If(String.IsNullOrEmpty(result), 0, Convert.ToDouble(result))
    '        End If
    '    Next

    '    Return If(String.IsNullOrEmpty(result), 0, Convert.ToDouble(result))
    'End Function
End Module

