Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no . 129

<System.Runtime.InteropServices.ProgId("SchemeGroupMember_NET.SchemeGroupMember")>
Public NotInheritable Class SchemeGroupMember
    ' ***************************************************************** '
    ' Class Name: SchemeGroupMember
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
    Private Const ACClass As String = "SchemeGroupMember"

    ' Constants to define stored procedure statements for scheme_group_members table.
    'developer guide no 39. 

    Private Const SQL_GROUP_MEMBERS_ADD As String = "spu_GIS_SchemeGrpMember_add"
    Private Const SQL_GROUP_MEMBERS_DEL_ALL As String = "spu_GIS_SchemeGrpMember_All_del"
    Private Const SQL_GROUP_MEMBERS_SEL As String = "spu_GIS_SchemeGrpMember_sel"
    Private Const SQL_GROUP_MEMBERS_All_SEL As String = "spu_GIS_SchemeGrpMember_All_sel"
    'sj 21/06/2001 - start
    'Private Const SQL_GROUP_MEMBERS_SEL_BY_CODE As String = "{ call spu_Gis_SchemeGrpMemByCode_sel (?)}"
    Private Const SQL_GROUP_MEMBERS_SEL_BY_CODE As String = "spu_Gis_SchemeGrpMemByCode_sel"
    'sj 21/06/2001 - end

    ' Constants to define names of above stored procedure statements for scheme_group table.
    Private Const SQL_NAME_GROUP_MEMBERS_ADD As String = "SchemeGroupMemberAdd"
    Private Const SQL_NAME_GROUP_MEMBERS_DEL_ALL As String = "SchemeGroupMemberDelAll"
    Private Const SQL_NAME_GROUP_MEMBERS_SEL As String = "SchemeGroupMemberSel"
    Private Const SQL_NAME_GROUP_MEMBERS_All_SEL As String = "SchemeGroupMemberAllSel"
    Private Const SQL_NAME_GROUP_MEMBERS_SEL_BY_CODE As String = "SchemeGroupMemberSelByCode"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    Private m_lSchemeGroupID As Integer
    'sj 21/06/2001 - start
    Private m_sCode As String = ""
    'sj 21/06/2001 - end

    ' PRIVATE Data Members (End)

    ' PUBLIC Properties (Begin)

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property

    ' PUBLIC Properties (End)

    ' PUBLIC Methods (Begin)

    Public Function Add(ByVal v_lSchemeGroupID As Integer, ByVal v_lSchemeID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            With m_oDatabase

                .Parameters.Clear()

                If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID, CStr(v_lSchemeGroupID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_ID, CStr(v_lSchemeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .SQLAction(SQL_GROUP_MEMBERS_ADD, SQL_NAME_GROUP_MEMBERS_ADD, True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    Public Function DeleteAll(ByVal v_lSchemeGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            With m_oDatabase

                .Parameters.Clear()

                If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID, CStr(v_lSchemeGroupID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .SQLAction(SQL_GROUP_MEMBERS_DEL_ALL, SQL_NAME_GROUP_MEMBERS_DEL_ALL, True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetList(ByVal v_lListType As Integer, ByRef r_vMembersArray(,) As Object, Optional ByVal v_vParameterValues(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL, sSQLName As String

        Try

            If ExtractParameterValues(v_vParameterValues) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With m_oDatabase

                .Parameters.Clear()

                Select Case v_lListType
                    Case bGISSchemeBusinessConst.GISSB_GET_LIST_SCHEME_GROUP_MEMBER.GLSGM_BY_GROUP_ID

                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID, m_lSchemeGroupID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        sSQL = SQL_GROUP_MEMBERS_SEL
                        sSQLName = SQL_NAME_GROUP_MEMBERS_SEL

                    Case bGISSchemeBusinessConst.GISSB_GET_LIST_SCHEME_GROUP_MEMBER.GLSGM_ALL
                        sSQL = SQL_GROUP_MEMBERS_All_SEL
                        sSQLName = SQL_NAME_GROUP_MEMBERS_All_SEL

                        'sj 21/06/2001 - start
                    Case bGISSchemeBusinessConst.GISSB_GET_LIST_SCHEME_GROUP_MEMBER.GLSGM_BY_GROUP_CODE

                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_CODE, m_sCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        sSQL = SQL_GROUP_MEMBERS_SEL_BY_CODE
                        sSQLName = SQL_NAME_GROUP_MEMBERS_SEL_BY_CODE
                        'sj 21/06/2001 - end

                    Case Else

                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="List Type unknown!", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList")

                        Return result

                End Select

                lReturn = .SQLSelect(sSQL, sSQLName, True, , r_vMembersArray, , , , True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


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



        m_lSchemeGroupID = PARAMETER_NOT_PRESENT_NO

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
                Case GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID.ToUpper()

                    m_lSchemeGroupID = CInt(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))

                        'sj 21/06/2001 - start
                Case GISSB_PARAM_NAME_CODE.ToUpper()

                    m_sCode = CStr(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))
                    'sj 21/06/2001 - end
                Case Else
                    ' Ignore unexpected parameters.

            End Select

        Next


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' PRIVATE Methods (End)
End Class

