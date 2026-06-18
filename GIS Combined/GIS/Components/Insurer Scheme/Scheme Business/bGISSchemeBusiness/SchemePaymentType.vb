Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129

'<System.Runtime.InteropServices.ProgId("SchemePaymentType_NET.SchemePaymentType")> _
Public NotInheritable Class SchemePaymentType
    ' ***************************************************************** '
    ' Class Name: SchemePaymentType
    '
    ' Date:  8/2/00
    '
    ' Description: Class to contain any business rules for accessing
    '              SchemePaymentType table.
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
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private Const ACClass As String = "SchemePaymentType"

    ' Constants to define stored procedure statements for updating GIS_Scheme table.
    Private Const SQL_SCHEME_PAYMENT_TYPE_SELECT_SQL As String = "{ call spu_GIS_Select_Sch_Pay (?)}"
    Private Const SQL_SCHEME_PAYMENT_TYPE_SELECT_NAME As String = "SelectPayTypeByScheme"
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property


    ' PUBLIC Methods (Begin)

    Public Function GetListByScheme(ByVal v_lGISSchemeId As Integer, ByRef r_vPaymentTypeArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumberRecords As Integer

        Try

            With m_oDatabase

                .Parameters.Clear()


                ' add scheme id parameter
                If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_ID, CStr(v_lGISSchemeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=SQL_SCHEME_PAYMENT_TYPE_SELECT_SQL, sSQLName:=SQL_SCHEME_PAYMENT_TYPE_SELECT_NAME, bStoredProcedure:=True, lNumberRecords:=lNumberRecords, vResultArray:=r_vPaymentTypeArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(r_vPaymentTypeArray) Then

                    ' No records for this scheme use default scheme_id 0

                    .Parameters.Clear()

                    ' add scheme id parameter
                    If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_ID, CStr(0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = .SQLSelect(sSQL:=SQL_SCHEME_PAYMENT_TYPE_SELECT_SQL, sSQLName:=SQL_SCHEME_PAYMENT_TYPE_SELECT_NAME, bStoredProcedure:=True, lNumberRecords:=lNumberRecords, vResultArray:=r_vPaymentTypeArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End With



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListByScheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' PUBLIC Methods (End)
End Class
