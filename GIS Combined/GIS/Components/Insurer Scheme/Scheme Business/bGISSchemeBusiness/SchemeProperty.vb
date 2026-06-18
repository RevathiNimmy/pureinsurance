Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129

'<System.Runtime.InteropServices.ProgId("SchemeProperty_NET.SchemeProperty")> _
Public NotInheritable Class SchemeProperty
    ' ***************************************************************** '
    ' Class Name: SchemeProperty
    '
    ' Date:  20/7/1999
    '
    ' Description: Class to contain any business rules for accessing
    '              SchemeProperty table.
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
    Private Const ACClass As String = "SchemeProperty"

    ' Constants to define stored procedure statements for updating GIS_Scheme table.
    'developer guide no 39. 
    Private Const SQL_SCHEME_PROPERTY_ADD_BY_SCHEME As String = "spu_GIS_SP_Add_By_Scheme"
    'sj 24/05/2001 - start
    'developer guide no 39.
    Private Const SQL_SCHEME_PROPERTY_DEL_BY_SCHEME As String = "spu_GIS_SP_Del_By_Scheme"
    'sj 24/05/2001 - end

    'sj 02/08/2001 - start
    'developer guide no 39.
    Private Const SQL_SCHEME_PROPERTY_ADD_BY_SCHEME_V2 As String = "spu_GIS_SP_Add_By_Scheme_V2"
    Private Const SQL_SCHEME_PROPERTY_DEL_BY_SCHEME_V2 As String = "spu_GIS_SP_Del_By_Scheme_V2"

    'sj 02/08/2001 - end


    ' Constants to define names of above stored procedure statements for scheme table.
    Private Const SQL_NAME_SCHEME_PROPERTY_ADD_BY_SCHEME As String = "SchemePropertyAddByScheme"
    Private Const SQL_NAME_SCHEME_PROPERTY_DEL_BY_SCHEME As String = "SchemePropertyDeleteByScheme"


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    Private m_lGisBusinessTypeId As Integer
    'sj 13/7/2000 - start
    Private m_lLinkageMapMin As Integer
    Private m_lLinkageMapMax As Integer
    'sj 13/7/2000 - end

    ''CLG 20040927
    'moved from being public in main module
    Private m_sClassOfBusiness As String = ""



    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property
    'sj 13/7/2000 - start
    Public WriteOnly Property GisBusinessTypeId() As Integer
        Set(ByVal Value As Integer)
            m_lGisBusinessTypeId = Value
        End Set
    End Property
    Public WriteOnly Property LinkageMapMin() As Integer
        Set(ByVal Value As Integer)
            m_lLinkageMapMin = Value
        End Set
    End Property
    Public WriteOnly Property LinkageMapMax() As Integer
        Set(ByVal Value As Integer)
            m_lLinkageMapMax = Value
        End Set
    End Property
    'sj 13/7/2000 - end

    'CLG 20040927
    Public WriteOnly Property ClassOfBusiness() As String
        Set(ByVal Value As String)
            m_sClassOfBusiness = Value
        End Set
    End Property




    ' PUBLIC Methods (Begin)

    Public Function AddByScheme(ByVal v_sQmInsurerRef As String, ByVal v_lSchemeNo As Integer) As Integer

        Dim result As Integer = 0
        Try

            If m_lLinkageMapMax = 0 Then
                m_lLinkageMapMax = 5000
            End If

            With m_oDatabase

                .Parameters.Clear()

                ' add qm insurer reference parameter
                If .Parameters.Add(GISSB_PARAM_NAME_QM_INSURER_REF, v_sQmInsurerRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add scheme no parameter
                If .Parameters.Add(GISSB_PARAM_NAME_SCHEME_NO, CStr(v_lSchemeNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' add linkage map min parameter
                If .Parameters.Add(GISSB_PARAM_NAME_LINKAGE_MAP_MIN, CStr(m_lLinkageMapMin), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' add linkage map max parameter
                If .Parameters.Add(GISSB_PARAM_NAME_LINKAGE_MAP_MAX, CStr(m_lLinkageMapMax), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'sj 02/08/2001 - start
                If m_sClassOfBusiness = "" Then
                    ' add business type id parameter
                    If .Parameters.Add(GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, CStr(m_lGisBusinessTypeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' add scheme cobol linkage records.
                    If .SQLAction(SQL_SCHEME_PROPERTY_ADD_BY_SCHEME, SQL_NAME_SCHEME_PROPERTY_ADD_BY_SCHEME, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else
                    ' add class of business parameter
                    If .Parameters.Add(GISSB_PARAM_NAME_CLASS_OF_BUSINESS, m_sClassOfBusiness, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' add scheme cobol linkage records.
                    If .SQLAction(SQL_SCHEME_PROPERTY_ADD_BY_SCHEME_V2, SQL_NAME_SCHEME_PROPERTY_ADD_BY_SCHEME, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                'sj 02/08/2001 - end

                '        ' add business type id parameter
                '        If .Parameters.Add(GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, _
                ''                           m_lGisBusinessTypeId, _
                ''                           PMParamInput, _
                ''                           PMLong) <> PMTrue Then
                '            AddByScheme = PMFalse
                '            Exit Function
                '        End If
                '
                '        ' add scheme cobol linkage records.
                '        If .SQLAction(SQL_SCHEME_PROPERTY_ADD_BY_SCHEME, _
                ''                      SQL_NAME_SCHEME_PROPERTY_ADD_BY_SCHEME, _
                ''                      True) <> PMTrue Then
                '            AddByScheme = PMFalse
                '            Exit Function
                '        End If

            End With



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add By Scheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddByScheme", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function DeleteByScheme(ByVal v_sQmInsurerRef As String, ByVal v_lSchemeNo As Integer) As Integer

        Dim result As Integer = 0
        Try

            If m_lGisBusinessTypeId = 0 Then
                m_lGisBusinessTypeId = 1
            End If

            With m_oDatabase

                .Parameters.Clear()

                ' add qm insurer reference parameter
                If .Parameters.Add(GISSB_PARAM_NAME_QM_INSURER_REF, v_sQmInsurerRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add scheme no parameter
                If .Parameters.Add(GISSB_PARAM_NAME_SCHEME_NO, CStr(v_lSchemeNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'sj 02/08/2001 - start
                If m_sClassOfBusiness = "" Then
                    ' add business type id parameter
                    If .Parameters.Add(GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, CStr(m_lGisBusinessTypeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' add scheme cobol linkage records.
                    If .SQLAction(SQL_SCHEME_PROPERTY_DEL_BY_SCHEME, SQL_NAME_SCHEME_PROPERTY_DEL_BY_SCHEME, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else
                    ' add class of business parameter
                    If .Parameters.Add(GISSB_PARAM_NAME_CLASS_OF_BUSINESS, m_sClassOfBusiness, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' add scheme cobol linkage records.
                    If .SQLAction(SQL_SCHEME_PROPERTY_DEL_BY_SCHEME_V2, SQL_NAME_SCHEME_PROPERTY_DEL_BY_SCHEME, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                'sj 02/08/2001 - end

            End With



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete By Scheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteByScheme", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' PUBLIC Methods (End)
End Class
