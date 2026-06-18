Option Strict Off
Option Explicit On
Imports SSP.Shared
' developer guide no. 129

'<System.Runtime.InteropServices.ProgId("SchemeGroup_NET.SchemeGroup")> _
Public NotInheritable Class SchemeGroup
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
    Private Const ACClass As String = "SchemeGroup"

    ' Constants to define stored procedure statements for scheme_group table.
    'developer guide no 39. 

    Private Const SQL_SCHEME_GROUP_ADD As String = "spu_GIS_Scheme_Group_add"
    Private Const SQL_SCHEME_GROUP_DELETE As String = "spu_GIS_Scheme_Group_del"
    Private Const SQL_SCHEME_GROUP_UPDATE As String = "spu_GIS_Scheme_Group_upd"
    Private Const SQL_SCHEME_GROUP_SELECT_ALL As String = "spu_GIS_Scheme_Groups_All_sel"
    Private Const SQL_SCHEME_GROUP_SELECT_BUS As String = "spu_GIS_Scheme_Groups_Bus_sel"
    Private Const SQL_SCHEME_GROUP_SELECT_SINGLE As String = "spu_GIS_Scheme_group_sel"

    ' Constants to define names of above stored procedure statements for scheme_group table.
    Private Const SQL_NAME_SCHEME_GROUP_ADD As String = "SchemeGroupAdd"
    Private Const SQL_NAME_SCHEME_GROUP_DELETE As String = "SchemeGroupDelete"
    Private Const SQL_NAME_SCHEME_GROUP_UPDATE As String = "SchemeGroupUpdate"
    Private Const SQL_NAME_SCHEME_GROUP_SELECT_ALL As String = "SchemeGroupSelectAll"
    Private Const SQL_NAME_SCHEME_GROUP_SELECT_BUS As String = "SchemeGroupSelectBus"
    Private Const SQL_NAME_SCHEME_GROUP_SELECT_SINGLE As String = "SchemeGroupSelectSingle"



    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Parameter variables for getting scheme group lists.
    ' Are filled by ExtractParameterValues
    Private m_lBusinessType As Integer
    Private m_lSchemeGroupID As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Properties (Begin)

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property

    ' PUBLIC Properties (End)

    ' PUBLIC Methods (Begin)


    Public Function Add(ByRef r_lSchemeGroupID As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_lBusinessType As Integer) As Integer

        ' Adds a scheme group

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lCaptionID As Integer

        Try

            With m_oDatabase

                .Parameters.Clear()

                ' sjd 26/3/2003 - code moved from after first parameter add
                ' as it was interferring with main SQL parameters
                ' get an caption_id for the description supplied.
                lReturn = CType(GetCaptionID(v_sDescription, lCaptionID, m_oDatabase), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                .Parameters.Clear()

                ' add code parameter
                If .Parameters.Add(GISSB_PARAM_NAME_CODE, v_sCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add caption_id parameter
                If .Parameters.Add(GISSB_PARAM_NAME_CAPTION_ID, CStr(lCaptionID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add description parameter
                If .Parameters.Add(GISSB_PARAM_NAME_DESCRIPTION, v_sDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add business type parameter.
                If .Parameters.Add(GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, CStr(v_lBusinessType), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add scheme group id for output from the procedure.
                If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID, CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add scheme group
                lReturn = .SQLAction(SQL_SCHEME_GROUP_ADD, SQL_NAME_SCHEME_GROUP_ADD, True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' return GIS_Scheme_Group_id returned from procedure
                r_lSchemeGroupID = .Parameters.Item(GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID).Value

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function Delete(ByVal v_lSchemeGroupID As Integer) As Integer

        ' Deletes a scheme group record

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            With m_oDatabase

                .Parameters.Clear()

                If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID, CStr(v_lSchemeGroupID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = .SQLAction(SQL_SCHEME_GROUP_DELETE, SQL_NAME_SCHEME_GROUP_DELETE, True)
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

    Public Function Update(ByVal v_lSchemeGroupID As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_lBusinessType As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lCaptionID As Integer

        Try

            'IJR 2003-08-29 Start
            lReturn = CType(GetCaptionID(v_sDescription, lCaptionID, m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'IJR 2003-08-29 End

            With m_oDatabase

                .Parameters.Clear()

                If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID, CStr(v_lSchemeGroupID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If .Parameters.Add(GISSB_PARAM_NAME_CODE, v_sCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'IJR 2003-08-29 Start
                'Moved to start of procedure
                '        lReturn = GetCaptionID(v_sDescription, lCaptionID, m_oDatabase)
                '        If lReturn <> PMTrue Then
                '            Update = PMFalse
                '            Exit Function
                '        End If
                'IJR 2003-08-29 End

                If .Parameters.Add(GISSB_PARAM_NAME_CAPTION_ID, CStr(lCaptionID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If .Parameters.Add(GISSB_PARAM_NAME_DESCRIPTION, v_sDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If .Parameters.Add(GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, CStr(v_lBusinessType), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                lReturn = .SQLAction(SQL_SCHEME_GROUP_UPDATE, SQL_NAME_SCHEME_GROUP_UPDATE, True)
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

    Public Function GetList(ByVal v_lListType As Integer, ByRef r_vSchemeGroupsArray(,) As Object, Optional ByVal v_vParameterValues As Object = Nothing) As Integer

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
                Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_GROUP_LISTS.GSGL_ALL
                    sSQL = SQL_SCHEME_GROUP_SELECT_ALL
                    sSQLName = SQL_NAME_SCHEME_GROUP_SELECT_ALL

                Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_GROUP_LISTS.GSGL_BUSINESS
                    If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, m_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    sSQL = SQL_SCHEME_GROUP_SELECT_BUS
                    sSQLName = SQL_NAME_SCHEME_GROUP_SELECT_BUS

                Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_GROUP_LISTS.GSGL_SINGLE
                    If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID, m_lSchemeGroupID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    sSQL = SQL_SCHEME_GROUP_SELECT_SINGLE
                    sSQLName = SQL_NAME_SCHEME_GROUP_SELECT_SINGLE

                Case Else


            End Select


            lReturn = m_oDatabase.SQLSelect(sSQL, sSQLName, True, , r_vSchemeGroupsArray, , , , True)
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



        m_lBusinessType = PARAMETER_NOT_PRESENT_NO
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
                Case GISSB_PARAM_NAME_GIS_BUSINESS_TYPE.ToUpper()

                    m_lBusinessType = CInt(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))

                Case GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID.ToUpper()

                    m_lSchemeGroupID = CInt(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))

                Case Else
                    ' Ignore unexpected parameters

            End Select

        Next


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' This is needed to remove the global variables from main module
    Friend Sub SetGlobalData(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String)

        Try

            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLanguageID = iLanguageID
            m_sCallingAppName = sCallingAppName
            m_iLogLevel = iLogLevel

        Catch excep As System.Exception



            Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

            Exit Sub

        End Try

    End Sub

    Public Function GetCaptionID(ByVal v_sDescription As String, ByRef r_lCaptionID As Integer, ByVal v_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oPMCaption As bPMCaption.Business

        Try

            oPMCaption = New bPMCaption.Business()
            If oPMCaption Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = oPMCaption.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = oPMCaption.GetCaptionID(v_sDescription, r_lCaptionID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMCaption.Dispose()

            oPMCaption = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptionID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptionID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function


    ' PRIVATE Methods (End)
End Class

