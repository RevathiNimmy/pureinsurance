Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129

<System.Runtime.InteropServices.ProgId("QEMUsage_NET.QEMUsage")>
Public NotInheritable Class QEMUsage
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
    Private Const ACClass As String = "QEMUsage"

    ' Constants to define stored procedure statements for updating GIS_Scheme table.
    'developer guide no 39. 
    Private Const SQL_QEM_Usage_ADD As String = "spu_GIS_QEM_Usage_Add"

    ' Constants to define names of above stored procedure statements for scheme table.
    Private Const SQL_NAME_QEM_Usage_ADD As String = "QEMUsageAdd"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property


    ' PUBLIC Methods (Begin)

    Public Function Add(ByVal v_lGISDataModelId As Integer, ByVal v_lGISBusinessTypeId As Integer, ByVal v_lGISSchemeId As Integer, ByVal v_lGISQEMId As Integer) As Integer

        Dim result As Integer = 0
        Try

            With m_oDatabase

                .Parameters.Clear()

                ' add data model id reference parameter
                If .Parameters.Add(GISSB_PARAM_NAME_GIS_DATA_MODEL, CStr(v_lGISDataModelId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add business type id parameter
                If .Parameters.Add(GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, CStr(v_lGISBusinessTypeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add scheme id parameter
                If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_ID, CStr(v_lGISSchemeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add QEM id parameter
                If .Parameters.Add(GISSB_PARAM_NAME_GIS_QEM, CStr(v_lGISQEMId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add scheme cobol linkage records.
                If .SQLAction(SQL_QEM_Usage_ADD, SQL_NAME_QEM_Usage_ADD, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add By Scheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' PUBLIC Methods (End)
End Class
