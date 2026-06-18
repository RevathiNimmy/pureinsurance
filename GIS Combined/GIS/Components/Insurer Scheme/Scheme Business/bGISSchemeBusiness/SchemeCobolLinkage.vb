Option Strict Off
Option Explicit On
Imports SSP.Shared
' developer guide no. 129

'<System.Runtime.InteropServices.ProgId("SchemeCobolLinkage_NET.SchemeCobolLinkage")> _
Public NotInheritable Class SchemeCobolLinkage
    ' ***************************************************************** '
    ' Class Name: SchemeCobolLinkage
    '
    ' Date:  20/7/1999
    '
    ' Description: Class to contain any business rules for accessing
    '              SchemeCobolLinkage table.
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
    Private Const ACClass As String = "SchemeCobolLinkage"

    ' Constants to define stored procedure statements for updating GIS_Scheme table.
    'developer guide no 39. 

    Private Const SQL_SCHEME_COBOL_LINKAGE_ADD_BY_SCHEME As String = "spu_GIS_SCL_Add_By_Scheme"
    Private Const SQL_SCHEME_COBOL_LINKAGE_DEL_BY_SCHEME As String = "spu_GIS_SCL_Del_By_Scheme"
    Private Const SQL_SCHEME_COBOL_LINKAGE_DEL_ALL As String = "spu_GIS_SCL_Del_All"

    ' Constants to define names of above stored procedure statements for scheme table.
    Private Const SQL_NAME_SCHEME_COBOL_LINKAGE_ADD_BY_SCHEME As String = "CobolLinkageAddByScheme"
    Private Const SQL_NAME_SCHEME_COBOL_LINKAGE_DEL_BY_SCHEME As String = "CobolLinkageDeleteByScheme"
    Private Const SQL_NAME_SCHEME_COBOL_LINKAGE_DEL_ALL As String = "CobolLinkageDeleteAll"


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property


    ' PUBLIC Methods (Begin)

    Public Function AddByScheme(ByVal v_sQmInsurerRef As String, ByVal v_lSchemeNo As Integer) As Integer

        Dim result As Integer = 0
        Try

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

                ' add scheme cobol linkage records.
                If .SQLAction(SQL_SCHEME_COBOL_LINKAGE_ADD_BY_SCHEME, SQL_NAME_SCHEME_COBOL_LINKAGE_ADD_BY_SCHEME, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

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

                ' add scheme cobol linkage records.
                If .SQLAction(SQL_SCHEME_COBOL_LINKAGE_DEL_BY_SCHEME, SQL_NAME_SCHEME_COBOL_LINKAGE_DEL_BY_SCHEME, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete By Scheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteByScheme", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function DeleteAll() As Integer

        Dim result As Integer = 0
        Try

            With m_oDatabase

                .Parameters.Clear()

                If .SQLAction(SQL_SCHEME_COBOL_LINKAGE_DEL_ALL, SQL_NAME_SCHEME_COBOL_LINKAGE_DEL_ALL, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete By Scheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' PUBLIC Methods (End)
End Class
