Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129

'<System.Runtime.InteropServices.ProgId("SchemeData_NET.SchemeData")> _
Public NotInheritable Class SchemeData
    ' ***************************************************************** '
    ' Class Name: SchemeGroup
    '
    ' Date:  15/06/1999
    '
    ' Description: Class to contain any business rules for access GIS
    '              scheme related data.
    '
    ' Edit History:
    ' ***************************************************************** '


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


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "SchemeData"

    ' Constants to define stored procedure statements for scheme_group table.
    'developer guide no 39. 
    Private Const SQL_SCHEME_DATA_ADD As String = "spe_GIS_Scheme_Data_add"
    Private Const SQL_SCHEME_DATA_DELETE As String = "spe_GIS_Scheme_Data_del"
    Private Const SQL_SCHEME_DATA_UPDATE As String = "spe_GIS_Scheme_Data_upd"
    Private Const SQL_SCHEME_DATA_SELECT As String = "spe_GIS_Scheme_Data_sel"
    Private Const SQL_SCHEME_DATA_DELETE_ALL As String = "spe_GIS_Scheme_Data_delall"

    ' Constants to define names of above stored procedure statements for scheme_group table.
    Private Const SQL_NAME_SCHEME_DATA_ADD As String = "SchemeDataAdd"
    Private Const SQL_NAME_SCHEME_DATA_DELETE As String = "SchemeDataDelete"
    Private Const SQL_NAME_SCHEME_DATA_UPDATE As String = "SchemeDataUpdate"
    Private Const SQL_NAME_SCHEME_DATA_SELECT As String = "SchemeDataSelect"
    Private Const SQL_NAME_SCHEME_DATA_DELETE_ALL As String = "SchemeDataDelAll"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Parameter variables for getting scheme group lists.
    ' Are filled by ExtractParameterValues
    Private m_lGisSchemeId As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Properties (Begin)

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property

    ' PUBLIC Properties (End)

    ' PUBLIC Methods (Begin)

    Public Function Add(ByVal v_lGISSchemeId As Integer, ByVal v_cAdminCharge As Decimal, ByVal v_lGISBusinessTypeId As Integer, ByVal v_cMinPermCharge As Decimal, ByVal v_cMinReinstPremium As Decimal, ByVal v_cMinTempCharge As Decimal, ByVal v_lMtaCanAdmCharge As Integer, ByVal v_lMtaCanMinValue As Integer, ByVal v_sMtaCanRoundType As String, ByVal v_lMtaReiAdmCharge As Integer, ByVal v_lMtaReiMinValue As Integer, ByVal v_sMtaReiRoundType As String, ByVal v_lMtaCpdAdmCharge As Integer, ByVal v_lMtaCpdMinValue As Integer, ByVal v_sMtaCpdRoundType As String, ByVal v_lMtaTempAdmCharge As Integer, ByVal v_lMtaTempMinValue As Integer, ByVal v_sMtaTempRoundType As String, ByVal v_lMtaPermAdmCharge As Integer, ByVal v_lMtaPermMinValue As Integer, ByVal v_sMtaPermRoundType As String, ByVal v_lOverrideScr As Integer, ByVal v_lReinstDaysWithNoRp As Integer) As Integer

        ' Adds a scheme data record

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            lReturn = CType(AddParameters(v_lGISSchemeId:=v_lGISSchemeId, v_cAdminCharge:=v_cAdminCharge, v_lGISBusinessTypeId:=v_lGISBusinessTypeId, v_cMinPermCharge:=v_cMinPermCharge, v_cMinReinstPremium:=v_cMinReinstPremium, v_cMinTempCharge:=v_cMinTempCharge, v_lMtaCanAdmCharge:=v_lMtaCanAdmCharge, v_lMtaCanMinValue:=v_lMtaCanMinValue, v_sMtaCanRoundType:=v_sMtaCanRoundType, v_lMtaReiAdmCharge:=v_lMtaReiAdmCharge, v_lMtaReiMinValue:=v_lMtaReiMinValue, v_sMtaReiRoundType:=v_sMtaReiRoundType, v_lMtaCpdAdmCharge:=v_lMtaCpdAdmCharge, v_lMtaCpdMinValue:=v_lMtaCpdMinValue, v_sMtaCpdRoundType:=v_sMtaCpdRoundType, v_lMtaTempAdmCharge:=v_lMtaTempAdmCharge, v_lMtaTempMinValue:=v_lMtaTempMinValue, v_sMtaTempRoundType:=v_sMtaTempRoundType, v_lMtaPermAdmCharge:=v_lMtaPermAdmCharge, v_lMtaPermMinValue:=v_lMtaPermMinValue, v_sMtaPermRoundType:=v_sMtaPermRoundType, v_lOverrideScr:=v_lOverrideScr, v_lReinstDaysWithNoRp:=v_lReinstDaysWithNoRp), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With m_oDatabase

                ' add scheme data
                lReturn = .SQLAction(SQL_SCHEME_DATA_ADD, SQL_NAME_SCHEME_DATA_ADD, True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function AddParameters(ByVal v_lGISSchemeId As Integer, ByVal v_cAdminCharge As Decimal, ByVal v_lGISBusinessTypeId As Integer, ByVal v_cMinPermCharge As Decimal, ByVal v_cMinReinstPremium As Decimal, ByVal v_cMinTempCharge As Decimal, ByVal v_lMtaCanAdmCharge As Integer, ByVal v_lMtaCanMinValue As Integer, ByVal v_sMtaCanRoundType As String, ByVal v_lMtaReiAdmCharge As Integer, ByVal v_lMtaReiMinValue As Integer, ByVal v_sMtaReiRoundType As String, ByVal v_lMtaCpdAdmCharge As Integer, ByVal v_lMtaCpdMinValue As Integer, ByVal v_sMtaCpdRoundType As String, ByVal v_lMtaTempAdmCharge As Integer, ByVal v_lMtaTempMinValue As Integer, ByVal v_sMtaTempRoundType As String, ByVal v_lMtaPermAdmCharge As Integer, ByVal v_lMtaPermMinValue As Integer, ByVal v_sMtaPermRoundType As String, ByVal v_lOverrideScr As Integer, ByVal v_lReinstDaysWithNoRp As Integer) As Integer


        Dim result As Integer = 0



        With m_oDatabase

            .Parameters.Clear()

            ' AddParameters gis_scheme_id parameter
            If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_ID, CStr(v_lGISSchemeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters business type parameter
            If .Parameters.Add(GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, CStr(v_lGISBusinessTypeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters admin charge parameter
            If .Parameters.Add(GISSB_PARAM_NAME_ADMIN_CHARGE, CStr(v_cAdminCharge), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters min_reinst_premium parameter
            If .Parameters.Add(GISSB_PARAM_NAME_min_reinst_premium, CStr(v_cMinReinstPremium), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters min_perm_charge parameter
            If .Parameters.Add(GISSB_PARAM_NAME_min_perm_charge, CStr(v_cMinPermCharge), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters min_reinst_premium parameter
            If .Parameters.Add(GISSB_PARAM_NAME_min_temp_charge, CStr(v_cMinTempCharge), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_can_adm_charge parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_can_adm_charge, CStr(v_lMtaCanAdmCharge), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_can_min_value parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_can_min_value, CStr(v_lMtaCanMinValue), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_can_round_type parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_can_round_type, v_sMtaCanRoundType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_rei_adm_charge parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_rei_adm_charge, CStr(v_lMtaReiAdmCharge), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_rei_min_value parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_rei_min_value, CStr(v_lMtaReiMinValue), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_rei_round_type parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_rei_round_type, v_sMtaReiRoundType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_cpd_adm_charge parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_cpd_adm_charge, CStr(v_lMtaCpdAdmCharge), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_cpd_min_value parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_cpd_min_value, CStr(v_lMtaCpdMinValue), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_cpd_round_type parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_cpd_round_type, v_sMtaCpdRoundType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_perm_adm_charge parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_perm_adm_charge, CStr(v_lMtaPermAdmCharge), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_perm_min_value parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_perm_min_value, CStr(v_lMtaPermMinValue), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_perm_round_type parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_perm_round_type, v_sMtaPermRoundType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_temp_adm_charge parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_temp_adm_charge, CStr(v_lMtaTempAdmCharge), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_temp_min_value parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_temp_min_value, CStr(v_lMtaTempMinValue), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters mta_temp_round_type parameter
            If .Parameters.Add(GISSB_PARAM_NAME_mta_temp_round_type, v_sMtaTempRoundType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters override_scr parameter
            If .Parameters.Add(GISSB_PARAM_NAME_override_scr, CStr(v_lOverrideScr), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AddParameters reinst_days_with_no_rp
            If .Parameters.Add(GISSB_PARAM_NAME_reinst_days_with_no_rp, CStr(v_lReinstDaysWithNoRp), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End With


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
    Public Function Delete(ByVal v_lGISSchemeId As Integer) As Integer

        ' Deletes a scheme group record

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            With m_oDatabase

                .Parameters.Clear()

                If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_ID, CStr(v_lGISSchemeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .SQLAction(SQL_SCHEME_DATA_DELETE, SQL_NAME_SCHEME_DATA_DELETE, True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteSchemeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteSchemeGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function DeleteAll(ByVal v_sGisBusinessTypeCode As String) As Integer

        ' Deletes a scheme group record

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            With m_oDatabase

                .Parameters.Clear()

                If .Parameters.Add(GISSB_PARAM_NAME_CODE, v_sGisBusinessTypeCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .SQLAction(SQL_SCHEME_DATA_DELETE_ALL, SQL_NAME_SCHEME_DATA_DELETE_ALL, True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function Update(ByVal v_lGISSchemeId As Integer, ByVal v_cAdminCharge As Decimal, ByVal v_lGISBusinessTypeId As Integer, ByVal v_cMinPermCharge As Decimal, ByVal v_cMinReinstPremium As Decimal, ByVal v_cMinTempCharge As Decimal, ByVal v_lMtaCanAdmCharge As Integer, ByVal v_lMtaCanMinValue As Integer, ByVal v_sMtaCanRoundType As String, ByVal v_lMtaReiAdmCharge As Integer, ByVal v_lMtaReiMinValue As Integer, ByVal v_sMtaReiRoundType As String, ByVal v_lMtaCpdAdmCharge As Integer, ByVal v_lMtaCpdMinValue As Integer, ByVal v_sMtaCpdRoundType As String, ByVal v_lMtaTempAdmCharge As Integer, ByVal v_lMtaTempMinValue As Integer, ByVal v_sMtaTempRoundType As String, ByVal v_lMtaPermAdmCharge As Integer, ByVal v_lMtaPermMinValue As Integer, ByVal v_sMtaPermRoundType As String, ByVal v_lOverrideScr As Integer, ByVal v_lReinstDaysWithNoRp As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            lReturn = CType(AddParameters(v_lGISSchemeId:=v_lGISSchemeId, v_cAdminCharge:=v_cAdminCharge, v_lGISBusinessTypeId:=v_lGISBusinessTypeId, v_cMinPermCharge:=v_cMinPermCharge, v_cMinReinstPremium:=v_cMinReinstPremium, v_cMinTempCharge:=v_cMinTempCharge, v_lMtaCanAdmCharge:=v_lMtaCanAdmCharge, v_lMtaCanMinValue:=v_lMtaCanMinValue, v_sMtaCanRoundType:=v_sMtaCanRoundType, v_lMtaReiAdmCharge:=v_lMtaReiAdmCharge, v_lMtaReiMinValue:=v_lMtaReiMinValue, v_sMtaReiRoundType:=v_sMtaReiRoundType, v_lMtaCpdAdmCharge:=v_lMtaCpdAdmCharge, v_lMtaCpdMinValue:=v_lMtaCpdMinValue, v_sMtaCpdRoundType:=v_sMtaCpdRoundType, v_lMtaTempAdmCharge:=v_lMtaTempAdmCharge, v_lMtaTempMinValue:=v_lMtaTempMinValue, v_sMtaTempRoundType:=v_sMtaTempRoundType, v_lMtaPermAdmCharge:=v_lMtaPermAdmCharge, v_lMtaPermMinValue:=v_lMtaPermMinValue, v_sMtaPermRoundType:=v_sMtaPermRoundType, v_lOverrideScr:=v_lOverrideScr, v_lReinstDaysWithNoRp:=v_lReinstDaysWithNoRp), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With m_oDatabase


                lReturn = .SQLAction(SQL_SCHEME_DATA_UPDATE, SQL_NAME_SCHEME_DATA_UPDATE, True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'developer guide no. 17
    Public Function GetList(ByVal v_lListType As Integer, ByRef r_vSchemeDataArray(,) As Object, Optional ByVal v_vParameterValues As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL As String = ""
        Dim sSQLName As String = ""


        Try

            If ExtractParameterValues(v_vParameterValues) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            Select Case v_lListType
                Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_DATA_LISTS.GSDL_BY_SCHEME
                    If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_SCHEME_ID, m_lGisSchemeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    sSQL = SQL_SCHEME_DATA_SELECT
                    sSQLName = SQL_NAME_SCHEME_DATA_SELECT



                Case Else


            End Select


            lReturn = m_oDatabase.SQLSelect(sSQL, sSQLName, True, , r_vSchemeDataArray, , , , True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    Private Function ExtractParameterValues(ByVal v_vParameterValues(,) As Object) As Integer

        Dim result As Integer = 0



        m_lGisSchemeId = PARAMETER_NOT_PRESENT_NO

        If False Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        If CheckArrayDimension(v_vParameterValues, 2) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If v_vParameterValues.GetUpperBound(0) <> 1 Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="First dimension of ParameterValues parameter must be a value of 1", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractParameterValues")
            Return result
        End If


        For iCnt As Integer = v_vParameterValues.GetLowerBound(1) To v_vParameterValues.GetUpperBound(1)


            Select Case CStr(v_vParameterValues(PARAMETER_POS_NAME, iCnt)).ToUpper()
                Case GISSB_PARAM_NAME_GIS_SCHEME_ID.ToUpper()

                    m_lGisSchemeId = CInt(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))

                Case Else
                    ' Ignore unexpected parameters

            End Select

        Next


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' PRIVATE Methods (End)
End Class

