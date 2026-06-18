Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'developer guide no.129
Imports SharedFiles
Imports System.Windows
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ***************************************************************** '
    ' Class Name: Business
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 22/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPMAuthorityLevel As Integer

    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this object.
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this object.
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Information.IsNothing(vTask) Then
                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then
                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then
                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then
                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then
                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Adds a ri model and it's lines
    ' ***************************************************************** '
    Public Function AddRIModel(ByRef r_lRIModelID As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpiryDate As Object, ByVal v_iRIModelType As Integer, ByVal v_iFACPremiumType As Integer, ByVal v_iClaimAllocationType As Integer, ByVal v_lCurrencyID As Integer, ByVal v_lXOLClmRIModelID As Integer, ByVal v_cXOLClmLimit As Decimal, ByVal v_lXOLCatRIModelID As Integer, ByVal v_cXOLCatLimit As Decimal, ByVal v_iXOLCatReinstatements As Integer, ByVal v_vRIModelLines(,) As Object, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "", Optional ByVal v_iTreatyPremiumType As Integer = 0, Optional ByVal v_vRIModelLinesVariableQuotaShare(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bTrans As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "AddRIModel"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start a transaction
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = True
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Unable to create SQL transaction")
            End If

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "code", v_sCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "description", v_sDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", v_bIsDeleted, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "effective_date", v_dtEffectiveDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "expiry_date", v_dtExpiryDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_type", v_iRIModelType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "fac_premium_type", v_iFACPremiumType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_allocation_type", v_iClaimAllocationType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", v_lCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If v_lXOLClmRIModelID = 0 Then

                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_clm_ri_model_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_clm_limit", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_clm_ri_model_id", v_lXOLClmRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_clm_limit", v_cXOLClmLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            End If
            If v_lXOLCatRIModelID = 0 Then

                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_ri_model_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_limit", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_reinstatements", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_ri_model_id", v_lXOLCatRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_limit", v_cXOLCatLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_reinstatements", IIf(v_iXOLCatReinstatements = 0, DBNull.Value, v_iXOLCatReinstatements), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If
            bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_premium_type", v_iTreatyPremiumType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            ' Call sql
            lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRIModelSQL, sSQLName:=ACInsertRIModelName, bStoredProcedure:=ACInsertRIModelStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add ri_model details")
            End If

            ' Get new ri model id
            r_lRIModelID = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("ri_model_id").Value)
            If r_lRIModelID = 0 Then
                gPMFunctions.RaiseError("r_lRIModelID = 0", "Invalid ri model id returned from insert")
            End If

            ' Update ri model lines
            lReturn = CType(UpdateRIModelLines(r_lRIModelID, v_vRIModelLines, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy, v_vRIModelLinesVariableQuotaShare:=v_vRIModelLinesVariableQuotaShare), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("UpdateRIModelLines", "Unable to add ri model line details")
            End If

            ' Commit the transaction
            lReturn = m_oDatabase.SQLCommitTrans()
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = False
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Unable to commit SQL transaction")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' If we are in a transaction when we get here roll it back!
            If bTrans Then
                m_oDatabase.SQLRollbackTrans()
            End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Delete an ri model
    ' ***************************************************************** '
    Public Function DeleteRIModel(ByVal v_lRIModelID As Integer, Optional ByVal v_bIsDeleted As Boolean = True, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "DeleteRIModel"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_id", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", v_bIsDeleted, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            ' Call sql
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRIModelSQL, sSQLName:=ACDeleteRIModelName, bStoredProcedure:=ACDeleteRIModelStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to delete ri model")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Retrieve ri_model details
    ' ***************************************************************** '
    Public Function GetRIModels(ByRef r_vRIModel(,) As Object, Optional ByVal v_lRIModelID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetRIModels"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we were given an ri model id add it, else select all
            If v_lRIModelID > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_id", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            Else
                m_oDatabase.Parameters.Clear()
            End If

            ' Call sql
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIModelSQL, sSQLName:=ACSelectRIModelName, bStoredProcedure:=ACSelectRIModelStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vRIModel)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get ri model")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Retrieve all ri model lines for a given ri model
    ' ***************************************************************** '
    Public Function GetRIModelLines(ByVal v_lRIModelID As Integer, ByRef r_vRIModelLines(,) As Object, Optional ByVal v_iFilterType As Integer = 0, Optional ByVal v_sTreatyTypeCode As String = "", Optional ByVal v_lRIArrangementID As Long = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetRIModelLines"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_id", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "FilterType", v_iFilterType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "TreatyTypeCode", v_sTreatyTypeCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", v_lRIArrangementID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Call sql
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIModelLineSQL, sSQLName:=ACSelectRIModelLineName, bStoredProcedure:=ACSelectRIModelLineStored, vResultArray:=r_vRIModelLines)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get ri model line list")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Update an ri model and it's lines
    ' ***************************************************************** '
    Public Function UpdateRIModel(ByVal v_lRIModelID As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpiryDate As Object, ByVal v_iRIModelType As Integer, ByVal v_iFACPremiumType As Integer, ByVal v_iClaimAllocationType As Integer, ByVal v_lCurrencyID As Integer, ByVal v_lXOLClmRIModelID As Integer, ByVal v_cXOLClmLimit As Decimal, ByVal v_lXOLCatRIModelID As Integer, ByVal v_cXOLCatLimit As Decimal, ByVal v_iXOLCatReinstatements As Integer, ByVal v_vRIModelLines(,) As Object, Optional sUniqueId As String = "", Optional sScreenHierarchy As String = "", Optional ByVal v_iTreatyPremiumType As Integer = 0, Optional ByVal v_vRIModelLinesVariableQuotaShare(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bTrans As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim m_iAuditRIModelId As Integer
        Const kMethodName As String = "UpdateRIModel"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start a transaction
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = True
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Unable to create SQL transaction")
            End If

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_id", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "code", v_sCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "description", v_sDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", v_bIsDeleted, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "effective_date", v_dtEffectiveDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "expiry_date", v_dtExpiryDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_type", v_iRIModelType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "fac_premium_type", v_iFACPremiumType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_allocation_type", v_iClaimAllocationType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", v_lCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "pmuser_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "audit_ri_model_id", m_iAuditRIModelId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            If v_lXOLClmRIModelID = 0 Then

                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_clm_ri_model_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_clm_limit", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_clm_ri_model_id", v_lXOLClmRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_clm_limit", v_cXOLClmLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            End If
            If v_lXOLCatRIModelID = 0 Then

                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_ri_model_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_limit", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_reinstatements", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_ri_model_id", v_lXOLCatRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_limit", v_cXOLCatLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_cat_reinstatements", IIf(v_iXOLCatReinstatements = 0, DBNull.Value, v_iXOLCatReinstatements), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If
            bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_premium_type", v_iTreatyPremiumType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            ' Call sql
            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRIModelSQL, sSQLName:=ACUpdateRIModelName, bStoredProcedure:=ACUpdateRIModelStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to update ri model details")
            End If

            m_iAuditRIModelId = gPMFunctions.ToSafeInteger(m_oDatabase.Parameters.Item("audit_ri_model_id").Value)

            ' Update ri model lines
            lReturn = CType(UpdateRIModelLines(v_lRIModelID, v_vRIModelLines, m_iAuditRIModelId, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy, v_vRIModelLinesVariableQuotaShare), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("UpdateRIModelLines", "Unable to update ri model line details")
            End If

            ' Commit the transaction
            lReturn = m_oDatabase.SQLCommitTrans()
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = False
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Unable to commit SQL transaction")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' If we are in a transaction when we get here roll it back!
            If bTrans Then
                m_oDatabase.SQLRollbackTrans()
            End If


        End Try
        Return result
    End Function

    ' *****************************************************************
    ' Update ri models. Called from AddRIModel or UpdateRIModel
    ' ***************************************************************** '
    Private Function UpdateRIModelLines(ByVal v_lRIModelID As Integer, ByVal v_vRIModelLines(,) As Object, Optional ByVal m_iAuditRIModelId As Integer = 0, Optional sUniqueId As String = "", Optional sScreenHierarchy As String = "", Optional ByVal v_vRIModelLinesVariableQuotaShare(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRIModelLineID As Integer
        'Const kMethodName As String = "UpdateRIModelLines"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Note: No transaction, we are covered by the transaction in the calling method!

        ' Add parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_line_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_id", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "audit_ri_model_id", m_iAuditRIModelId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
        bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

        ' Delete the current ri model lines
        lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRIModelLineSQL, sSQLName:=ACDeleteRIModelLineName, bStoredProcedure:=ACDeleteRIModelLineStored)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to delete existing ri model lines")
        End If

        ' Add current ri model lines
        If Information.IsArray(v_vRIModelLines) Then
            ' Loop through lines
            For lCount As Integer = v_vRIModelLines.GetLowerBound(1) To v_vRIModelLines.GetUpperBound(1)
                ' Check for active share percent
                If gPMFunctions.ToSafeLong(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLTreatyID, lCount)) > 0 Then
                    ' Update ri_model_id as it will not be set for new lines

                    v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLRIModelID, lCount) = v_lRIModelID
                    Dim ScreenHierarchy As String = ""
                    If (sScreenHierarchy <> "") Then
                        ScreenHierarchy = sScreenHierarchy & "/" & $"Treaty Line({v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLDescription, lCount).ToString().Trim()})"
                    End If
                    ' Add parameters

                    bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_line_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_id", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "priority", v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLPriority, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "number_of_lines", v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLNumberOfLines, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLLineLimit, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLTreatyID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "share_percent", v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLSharePercent, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Lower_limit", IIf(CStr(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLLowerLimit, lCount)) = "", DBNull.Value, v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLLowerLimit, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ceding_rate", IIf(CStr(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLCedingrate, lCount)) = "", DBNull.Value, v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLCedingrate, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Treaty_Type_id", IIf(CDbl(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLTreatyTypeID, lCount)) = 0 Or CStr(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLTreatyTypeID, lCount)) = "", 1, v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLTreatyTypeID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Is_Obligatory", IIf(gPMFunctions.ToSafeBoolean(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLIsObligatory, lCount)) Or gPMFunctions.ToSafeInteger(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLIsObligatory, lCount)) = 1, 1, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "cede_premium_only", ToSafeInteger(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLCedePremiumOnly, lCount), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", ScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    Dim premiumCalculationBasis As Integer = ToSafeInteger(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLPremiumCalculationBasis, lCount), 0)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "premium_calculation_basis", IIf(premiumCalculationBasis = 0, DBNull.Value, premiumCalculationBasis), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Is_VariableQuotaShare", ToSafeInteger(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLRIIsVariableQuotaShare, lCount), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    ' Insert ri model line
                    lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRIModelLineSQL, sSQLName:=ACInsertRIModelLineName, bStoredProcedure:=ACInsertRIModelLineStored)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add new ri model line")
                    End If

                    ' Get the newly created ri_model_line_id
                    lRIModelLineID = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("ri_model_line_id").Value)

                    ' Save Variable Quota Share configuration if exists
                    If v_vRIModelLinesVariableQuotaShare IsNot Nothing AndAlso Information.IsArray(v_vRIModelLinesVariableQuotaShare) Then
                        If ToSafeInteger(v_vRIModelLines(DBMLMax, lCount), 0) > 0 Then
                            Dim vqsConfig As Object(,) = v_vRIModelLinesVariableQuotaShare
                            lReturn = CType(SaveVariableQuotaShareConfigForLine(v_lRIModelID, lRIModelLineID, gPMFunctions.ToSafeLong(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLTreatyID, lCount)), vqsConfig, gPMFunctions.ToSafeLong(v_vRIModelLines(MainModule.TreatyPartyEnum.DBMLRIModelLineID, lCount))), gPMConstants.PMEReturnCode)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("SaveVariableQuotaShareConfigForLine", "Unable to save Variable Quota Share configuration")
                            End If
                        End If
                    End If
                End If
            Next lCount
        End If


        Return result
    End Function

    ''' <summary>
    ''' Save Variable Quota Share configuration for a specific RI Model Line
    ''' </summary>
    Private Function SaveVariableQuotaShareConfigForLine(ByVal v_lRIModelID As Integer, ByVal v_lRIModelLineID As Integer, ByVal v_lTreatyID As Integer, ByVal v_vConfigData As Object(,), ByVal prev_RIModelLineID As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "SaveVariableQuotaShareConfigForLine"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Save each configuration row
            If Information.IsArray(v_vConfigData) Then
                For lCount As Integer = v_vConfigData.GetLowerBound(1) To v_vConfigData.GetUpperBound(1)
                    If v_lTreatyID = v_vConfigData(MainModule.TreatyQuotaShareEnum.DBMLTreatyId, lCount) Then
                        ' Add parameters
                        bPMAddParameter.AddParameterLite(m_oDatabase, "RIModelID", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "RIModelLineID", v_lRIModelLineID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TreatyID", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "SALowerLimit", gPMFunctions.ToSafeCurrency(v_vConfigData(1, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "SAUpperLimit", gPMFunctions.ToSafeCurrency(v_vConfigData(2, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "SharePercent", gPMFunctions.ToSafeCurrency(v_vConfigData(3, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TreatyLimit", gPMFunctions.ToSafeCurrency(v_vConfigData(4, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "VariableQuotaShareID", IIf(v_vConfigData(0, lCount) Is Nothing, 0, gPMFunctions.ToSafeInteger(v_vConfigData(0, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                        ' Execute the command
                        lReturn = m_oDatabase.SQLAction(sSQL:=ACSaveRIModelVariableQuotaShareSQL, sSQLName:=ACSaveRIModelVariableQuotaShareName, bStoredProcedure:=ACSaveRIModelVariableQuotaShareStored)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = lReturn
                            Exit Try
                        End If
                    End If
                Next
            Else
                Dim loadReturn As Integer = GetVariableQuotaShareConfig(v_lTreatyID, v_vConfigData, prev_RIModelLineID)
                If loadReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = loadReturn
                ElseIf Information.IsArray(v_vConfigData) Then
                    SaveVariableQuotaShareConfigForLine(v_lRIModelID, v_lRIModelLineID, v_lTreatyID, v_vConfigData, prev_RIModelLineID)

                End If
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMError
        End Try

        Return result
    End Function

    ' ***************************************************************** '
    '                           CLASS EVENTS
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()
        Exit Sub
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Public Function CheckRetainedReinsurer(ByVal lTreatyId As Integer, ByRef bIsRetainedReinsurer As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "CheckRetainedReinsurer"

        Try

            bPMAddParameter.AddParameterLite(m_oDatabase, "Treaty_id", lTreatyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckRetainedReinsurerSQL, sSQLName:=ACCheckRetainedReinsurerName, bStoredProcedure:=ACCheckRetainedReinsurerStored, vResultArray:=vResult)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to Check Retained Reinsurer in the selected treaty.")
            End If

            If Information.IsArray(vResult) Then
                bIsRetainedReinsurer = gPMFunctions.ToSafeBoolean(vResult(1, 0))
            End If
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Public Function GetRIModelAuditTrail(ByVal v_lRIModelID As Integer, ByRef v_vRIModelAuditTrailArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetRIModelAuditTrail"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_id", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Call sql
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRiModelAuditInfoSQL, sSQLName:=ACSelectRiModelAuditInfoName, bStoredProcedure:=ACSelectRiModelAuditInfoStored, vResultArray:=v_vRIModelAuditTrailArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get ri model line list")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Public Function GetRITypeForTreaty(ByVal v_lTreatyID As Integer, ByRef r_vRITypeId(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetRITypeForTreaty"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Call sql
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRITypeForTreatySQL, sSQLName:=ACGetRITypeForTreatyName, bStoredProcedure:=ACGetRITypeForTreatyStored, vResultArray:=r_vRITypeId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get ri type")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    ''' <summary>
    ''' GetRIExtendedLimitAmount
    ''' </summary>
    ''' <param name="v_nRIArrangementID"></param>
    ''' <param name="v_nFilterType"></param>
    ''' <param name="r_oRIExtendedLimit"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRIExtendedLimitAmount(ByVal v_nRIArrangementID As Integer,
                                        ByVal v_nFilterType As Integer,
                                    ByRef r_oRIExtendedLimit(,) As Object) As Integer

        Dim nReturn As Long
        Const kMethodName As String = "GetRIExtendedLimitAmount"

        Try

            nReturn = PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", v_nRIArrangementID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "FilterType", v_nFilterType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' Call sql
            nReturn = m_oDatabase.SQLSelect(sSQL:=kGetRIExtendedLimitSQL, sSQLName:=kGetRIExtendedLimitName, bStoredProcedure:=kGetRIExtendedLimitStored, vResultArray:=r_oRIExtendedLimit)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get ri extended limit")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            nReturn = PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn, excep:=ex)
        End Try
        Return nReturn
    End Function

    Private Function GetScreenHierarchyNameForTreatyParties(ByVal sScreenHierarchy As String, ByVal AppendReinsurerNameForTreaty As String) As String

        Dim index As Integer = sScreenHierarchy.LastIndexOf(")")
        sScreenHierarchy = sScreenHierarchy.Remove(index)
        sScreenHierarchy = sScreenHierarchy & "/" & $"Treaty Line({AppendReinsurerNameForTreaty})" & ")"
        Return sScreenHierarchy
    End Function

    ' ***************************************************************** '
    ' Retrieve all ri model currency rates for a given ri model
    ' ***************************************************************** '
    Public Function GetRIModelCurrencyRates(ByVal v_lRIModelID As Integer, ByRef r_vRIModelCurrRates(,) As Object, Optional ByVal v_iFilterType As Integer = 0, Optional ByVal v_sTreatyTypeCode As String = "", Optional ByVal v_lRIArrangementID As Long = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetRIModelCurrencyRates"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_id", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Call sql
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectModelCurrencySQL, sSQLName:=ACSelectModelCurrencyName, bStoredProcedure:=ACSelectModelCurrencyStored, vResultArray:=r_vRIModelCurrRates)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get ri model Cuurency Rates")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Save RI Model Currency Rates
    ' ***************************************************************** '
    Public Function UpdateRIModelCurrencyRates(ByVal v_lRIModelID As Integer, ByVal v_vRIModelCurrRates(,) As Object, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "UpdateRIModelCurrencyRates"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(v_vRIModelCurrRates) Then
                For lCount As Integer = v_vRIModelCurrRates.GetLowerBound(1) To v_vRIModelCurrRates.GetUpperBound(1)
                    Dim rateValue As String = Convert.ToString(v_vRIModelCurrRates(2, lCount)).Trim()
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ri_model_id", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", v_vRIModelCurrRates(0, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "conversion_rate", rateValue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy & $"\ConversionRate({v_vRIModelCurrRates(1, lCount).ToString().Trim()})", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                    lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateModelCurrencySQL, sSQLName:=ACUpdateModelCurrencyName, bStoredProcedure:=ACUpdateModelCurrencyStored)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to save RI model currency rate")
                    End If
                Next lCount
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try
        Return result
    End Function


    ''' <summary>
    ''' Get Variable Quota Share configuration for a treaty
    ''' </summary>
    Public Function GetRIModelVariableQuotaShareConfig(ByVal v_lRIModelID As Integer, ByRef v_vRIModelLinesVariableQuotaShare As Object(,)) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetRIModelVariableQuotaShareConfig"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter - use TreatyID since we don't have RIModelLineID when loading

            bPMAddParameter.AddParameterLite(m_oDatabase, "RIModelID", v_lRIModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            ' Execute and get recordset
            Dim lReturn As Integer = m_oDatabase.SQLSelect(sSQL:=ACGetRIModelVariableQuotaShareSQL, sSQLName:=ACGetRIModelVariableQuotaShareName, bStoredProcedure:=ACGetRIModelVariableQuotaShareStored, vResultArray:=v_vRIModelLinesVariableQuotaShare)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMError
        End Try

        'r_lReturn = result
        Return result
    End Function


    ''' <summary>
    ''' Get Variable Quota Share configuration for a treaty
    ''' </summary>
    Public Function GetVariableQuotaShareConfig(ByVal v_lTreatyID As Integer, ByRef r_vConfigData As Object(,), Optional ByVal v_lRIModelLineID As Integer = 0) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetVariableQuotaShareConfig"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter - use TreatyID since we don't have RIModelLineID when loading
            bPMAddParameter.AddParameterLite(m_oDatabase, "TreatyID", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "RIModelLineID", v_lRIModelLineID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            ' Execute and get recordset
            Dim lReturn As Integer = m_oDatabase.SQLSelect(sSQL:=ACSelectRIModelVariableQuotaShareSQL, sSQLName:=ACSelectRIModelVariableQuotaShareName, bStoredProcedure:=ACSelectRIModelVariableQuotaShareStored, vResultArray:=r_vConfigData)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMError
        End Try

        'r_lReturn = result
        Return result
    End Function



    ''' <summary>
    ''' Delete all Variable Quota Share configurations for a treaty
    ''' </summary>
    Public Function DeleteVariableQuotaShareConfigByTreaty(ByVal v_lVariableQuotaShareID As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DeleteVariableQuotaShareConfigByTreaty"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "VariableQuotaShareID", v_lVariableQuotaShareID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            ' Execute the command
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRIModelVariableQuotaShareSQL, sSQLName:=ACDeleteRIModelVariableQuotaShareName, bStoredProcedure:=ACDeleteRIModelVariableQuotaShareStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMError
        End Try

        Return result
    End Function

End Class
