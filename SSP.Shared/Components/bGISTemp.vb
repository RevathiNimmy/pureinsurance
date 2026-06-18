Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("bGISTemp_NET.bGISTemp")> _
 Public Module bGISTemp
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	Private Const ACClass As String = "bGISTemp"
	
	Private Const ACUpdatePolLnkTransactStored As Boolean = True
	Private Const ACUpdatePolLnkTransactName As String = "UpdatePolLnkTransact"
    Private Const ACUpdatePolLnkTransactSQL As String = "spu_gis_policy_link_transact_upd" ' RAG130600
	
	Public Const GISNBTransTypeBankAccountValidation As String = "0"
	Public Const GISNBTransTypePayment As String = "1"
	Public Const GISNBTransTypeGenPolNo As String = "2"
	Public Const GISNBTransTypePremiumFinanceTransact As String = "3"
	Public Const GISNBTransTypeEDIFile As String = "4"
	Public Const GISNBTransTypeForms As String = "5"
	Public Const GISNBTransTypeSaveInDB As String = "6"
	Public Const GISNBTransTypeSOF As String = "7"
	Public Const GISNBTransTypeQuote2Pol As String = "8"
	Public Const GISNBTransTypeComplete As String = "NB"
	
	Public Const GISMTATransTypePayment As String = "101"
	Public Const GISMTATransTypePremiumFinanceTransact As String = "102"
	Public Const GISMTATransTypeEDIFile As String = "103"
	Public Const GISMTATransTypeForms As String = "104"
	Public Const GISMTATransTypeSaveInDB As String = "105"
	Public Const GISMTATransTypeSOF As String = "106"
	Public Const GISMTATransTypeBorderaux As String = "107"
	Public Const GISMTATransTypeMakeLive As String = "108"
	Public Const GISMTATransTypeComplete As String = "MTA"


    ' ***************************************************************** '
    ' Name: UpdatePolicyLinkTransact
    '
    ' Description: Update the transact_date and transact_type in the
    '              gis_policy_link table. This status is updated as
    '              we progress through each of the steps required for a
    '              Transact. The mothod tells us if we need to process
    '              this step.
    ' RAG 13/06/2000
    '
    ' ***************************************************************** '
    Public Function UpdatePolicyLinkTransact(ByVal v_lPolicyLinkID As Integer, ByVal v_dTransactDate As Date, ByVal v_sTransactType As String, ByRef r_oDatabase As dPMDAO.Database, Optional ByRef r_bTransStepRequired As Boolean = False, Optional ByVal v_lGISSchemeID As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim sDate As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vNewTransactType As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Convert date to string a in format "YYYY/MM/DD HH:MM:SS"
            sDate = v_dTransactDate.ToString("yyyy/MM/dd HH:NN:SS")

            r_oDatabase.Parameters.Clear()

            ' GIS policy link id
            lReturn = r_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter gis_policy_link_id", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkTransact")
                Return result
            End If

            ' transact_type
            lReturn = r_oDatabase.Parameters.Add(sName:="transact_date", vValue:=sDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter transact_date", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkTransact")
                Return result
            End If

            ' transact_date
            lReturn = r_oDatabase.Parameters.Add(sName:="transact_type", vValue:=v_sTransactType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter transact_type", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkTransact")
                Return result
            End If

            If v_lGISSchemeID < 0 Then


                'Modified by Deepak Sharma on 4/20/2010 4:44:44 PM refer developer guide no. 85(Guide)
                'lReturn = r_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)
                lReturn = r_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            Else

                lReturn = r_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(v_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter gis scheme id", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkTransact")
                Return result
            End If

            ' new transactaction type
            lReturn = r_oDatabase.Parameters.Add(sName:="new_transact_type", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter new_transact_type", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkTransact")
                Return result
            End If

            ' Call the SQL
            lReturn = r_oDatabase.SQLAction(sSQL:=ACUpdatePolLnkTransactSQL, sSQLName:=ACUpdatePolLnkTransactName, bStoredProcedure:=ACUpdatePolLnkTransactStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on SQLAction", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkTransact")
                Return result
            End If

            ' Get the New Transaction Type
            vNewTransactType = r_oDatabase.Parameters.Item("new_transact_type").Value

            ' What is the New Transaction Type


            Select Case vNewTransactType
                ' NULL
                'Modified by Deepak Sharma on 4/20/2010 4:45:12 PM refer developer guide no. 85(Guide)
                'Case CStr(DBNull.Value)
                Case DBNull.Value.ToString
                    ' ERROR
                    r_bTransStepRequired = False
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' New Status is NB
                Case "NB"
                    ' If we wanted to set it to NB then all is ok
                    If v_sTransactType = "NB" Then
                        r_bTransStepRequired = True
                    Else
                        ' Error
                        r_bTransStepRequired = False
                        ' UpdatePolicyLinkTransact = PMFalse 'To enable retransact fix
                    End If

                    ' New Status is MTA
                Case "MTA"
                    ' If we wanted to set it to MTA then all is ok
                    If v_sTransactType = "MTA" Then
                        r_bTransStepRequired = True
                    Else
                        ' Error
                        r_bTransStepRequired = False
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' New Transact Type is Set to what we Requested
                Case v_sTransactType
                    ' Step is Required
                    r_bTransStepRequired = True

                    ' New Transaction Type is not what we requested
                Case Else
                    ' Step Not Required
                    r_bTransStepRequired = False

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run-time Error", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkTransact", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
End Module