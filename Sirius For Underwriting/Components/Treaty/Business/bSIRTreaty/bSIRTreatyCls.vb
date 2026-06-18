Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
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
    ' Adds a treaty and it's parties
    ' ***************************************************************** '
    Public Function AddTreaty(ByRef r_lTreatyID As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpiryDate As Object, ByVal v_sAgreementCode As String, ByVal v_lReinsuranceTypeID As Integer, ByVal v_lReplacesTreatyID As Object, ByVal v_vTreatyParties(,) As Object, ByVal v_dtReplacedEffectiveDt As Object, ByVal v_lReplacedByTreatyID As Object, ByVal v_dTreatyLimit As Decimal, ByVal v_lCurrencyID As Integer, ByVal v_lReinstatements As Integer, Optional ByVal v_vTreatyPartiesBrokerParticipants(,) As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim bTrans As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "AddTreaty"

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
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", r_lTreatyID, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "code", v_sCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "description", v_sDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", v_bIsDeleted, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "effective_date", v_dtEffectiveDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "expiry_date", v_dtExpiryDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", v_sAgreementCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "reinsurance_type_id", v_lReinsuranceTypeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "replaces_treaty_id", v_lReplacesTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "replaced_by_effective_date", v_dtReplacedEffectiveDt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "replaced_by_treaty_id", v_lReplacedByTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_limit", v_dTreatyLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", v_lCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "reinstatements", v_lReinstatements, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            ' Call sql
            lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertTreatySQL, sSQLName:=ACInsertTreatyName, bStoredProcedure:=ACInsertTreatyStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add treaty details")
            End If

            ' Get new treaty id
            r_lTreatyID = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("treaty_id").Value)
            If r_lTreatyID = 0 Then
                gPMFunctions.RaiseError("r_lTreatyID = 0", "Invalid treaty id returned from insert")
            End If

            ' Update treaty parties
            lReturn = CType(UpdateTreatyParties(r_lTreatyID, v_vTreatyParties, v_vTreatyPartiesBrokerParticipants, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("UpdateTreatyParties", "Unable to add treaty party details")
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
    ' Delete a treaty
    ' ***************************************************************** '
    Public Function DeleteTreaty(ByVal v_lTreatyID As Integer, Optional ByVal v_bIsDeleted As Boolean = True, Optional sUniqueId As String = "", Optional sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "DeleteTreaty"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", v_bIsDeleted, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            ' Call sql
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteTreatySQL, sSQLName:=ACDeleteTreatyName, bStoredProcedure:=ACDeleteTreatyStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to delete treaty")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Retrieve treaty list
    ' ***************************************************************** '
    Public Function GetTreatyList(ByRef r_vTreaties(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetTreatyList"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters
            m_oDatabase.Parameters.Clear()

            ' Get all treaties
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectTreatySQL, sSQLName:=ACSelectTreatyName, bStoredProcedure:=ACSelectTreatyStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vTreaties)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get treaty list")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Retrieve all treaty parties for a given treaty
    ' ***************************************************************** '
    Public Function GetTreatyPartyList(ByVal v_lTreatyID As Integer, ByRef r_vTreatyParties(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetTreatyList"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Call sql
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectTreatyPartySQL, sSQLName:=ACSelectTreatyPartyName, bStoredProcedure:=ACSelectTreatyPartyStored, vResultArray:=r_vTreatyParties)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get treaty party list")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Retrieve tax information for a given party
    ' ***************************************************************** '
    Public Function GetTreatyPartyTaxInfo(ByVal lPartyCnt As Integer, ByRef r_iIsDomiciledForTax As Integer, ByRef r_lTaxGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetTreatyPartyTaxInfo"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Select information
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPartyInsurerSQL, sSQLName:=ACSelectPartyInsurerName, bStoredProcedure:=ACSelectPartyInsurerStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Failed to select party tax details")
            End If

            ' Check for results and return information
            If m_oDatabase.Records.Count() > 0 Then
                With m_oDatabase.Records.Item(0).Fields
                    r_iIsDomiciledForTax = gPMFunctions.ToSafeInteger(m_oDatabase.Records.Item(0).Fields()("domiciled_for_tax"))
                    r_lTaxGroupID = gPMFunctions.ToSafeLong(m_oDatabase.Records.Item(0).Fields("tax_group_id"))
                End With
            Else
                r_iIsDomiciledForTax = 0
                r_lTaxGroupID = 0
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Update a treaty and it's parties
    ' ***************************************************************** '
    Public Function UpdateTreaty(ByVal v_lTreatyID As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpiryDate As Object, ByVal v_sAgreementCode As String, ByVal v_lReinsuranceTypeID As Integer, ByVal v_lReplacesTreatyID As Object, ByVal v_vTreatyParties(,) As Object, ByVal v_dtReplacedEffectiveDate As Date, ByVal v_vlReplacedByTreatyID As Object, ByVal v_dTreatyLimit As Decimal, ByVal v_lCurrencyID As Integer, ByVal v_lReinstatements As Integer, Optional ByVal v_vTreatyPartiesBrokerParticipants(,) As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim bTrans As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "UpdateTreaty"

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
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "code", v_sCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "description", v_sDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", v_bIsDeleted, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            bPMAddParameter.AddParameterLite(m_oDatabase, "effective_date", v_dtEffectiveDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "expiry_date", v_dtExpiryDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", v_sAgreementCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "reinsurance_type_id", v_lReinsuranceTypeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "replaces_treaty_id", v_lReplacesTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "replaced_by_effective_date", v_dtReplacedEffectiveDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "replaced_by_treaty_id", v_vlReplacedByTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_limit", v_dTreatyLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", v_lCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "reinstatements", v_lReinstatements, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            ' Call sql
            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateTreatySQL, sSQLName:=ACUpdateTreatyName, bStoredProcedure:=ACUpdateTreatyStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to update treaty details")
            End If

            ' Update treaty parties
            lReturn = CType(UpdateTreatyParties(v_lTreatyID, v_vTreatyParties, v_vTreatyPartiesBrokerParticipants, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("UpdateTreatyParties", "Unable to update treaty party details")
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
    ' Update treaty parties. Called from AddTreaty or UpdateTreaty
    ' ***************************************************************** '
    Private Function UpdateTreatyParties(ByVal v_lTreatyID As Integer, ByVal v_vTreatyParties(,) As Object, Optional ByVal v_vTreatyPartiesBrokerParticipants(,) As Object = Nothing, Optional sUniqueId As String = "", Optional sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim dShare As Double
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lCountParticipant As Integer = 0
        Dim lTreatyPartyId As Integer = 0
        Dim lCnt As Integer = 0

        Const kMethodName As String = "UpdateTreatyParties"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Note: No transaction, we are covered by the transaction in the calling method!
        ' Add parameters

        bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_party_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If Not String.IsNullOrEmpty(sScreenHierarchy) Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        End If
        ' Delete the current treaty parties
        lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteTreatyPartySQL, sSQLName:=ACDeleteTreatyPartyName, bStoredProcedure:=ACDeleteTreatyPartyStored)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to delete existing treaty parties")
        End If

        'Delete current participants
        If IsArray(v_vTreatyPartiesBrokerParticipants) Then
            For lCnt = 0 To UBound(v_vTreatyPartiesBrokerParticipants)
                If ToSafeLong(v_vTreatyPartiesBrokerParticipants(lCnt, 2)) > 0 Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_party_id", ToSafeLong(v_vTreatyPartiesBrokerParticipants(lCnt, 2)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    lReturn = m_oDatabase.SQLAction(
                                        sSQL:=ACDeleteTreatyPartyBrokerParticipantSQL,
                                        sSQLName:=ACDeleteTreatyPartyBrokerParticipantName,
                                        bStoredProcedure:=ACDeleteTreatyPartyBrokerParticipantStored)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("m_oDatabase.SQLAction", "Unable to delete existing treaty parties broker participant")
                    End If
                End If
            Next
        End If

        ' Add current treaty parties
        If Information.IsArray(v_vTreatyParties) Then
            ' Loop through parties
            For lCount As Integer = v_vTreatyParties.GetLowerBound(1) To v_vTreatyParties.GetUpperBound(1)
                ' Check for valid id (deleted records will have their party_cnt zeroed)
                If gPMFunctions.ToSafeLong(v_vTreatyParties(MainModule.TreatyPartyEnum.DBTPPartyCnt, lCount)) <> 0 Then
                    ' Keep running total of share percent to ensure 100% allocation

                    dShare += CDbl(v_vTreatyParties(MainModule.TreatyPartyEnum.DBTPSharePercent, lCount))

                    ' Update treaty_id as it will not be set for new treaties

                    v_vTreatyParties(MainModule.TreatyPartyEnum.DBTPTreatyID, lCount) = v_lTreatyID
                    Dim ScreenHierarchy As String = ""
                    If (sScreenHierarchy <> "") Then
                        ScreenHierarchy = sScreenHierarchy & "/" & $"Treaty Party({v_vTreatyParties(MainModule.TreatyPartyEnum.DBTPResolvedName, lCount).ToString().Trim()})"
                    End If
                    ' Add parameters
                    bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_party_id", lTreatyPartyId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", v_vTreatyParties(MainModule.TreatyPartyEnum.DBTPPartyCnt, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "share_percent", v_vTreatyParties(MainModule.TreatyPartyEnum.DBTPSharePercent, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "commission_percent", v_vTreatyParties(MainModule.TreatyPartyEnum.DBTPCommissionPercent, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "is_ReInsurer_Approved", v_vTreatyParties(MainModule.TreatyPartyEnum.DBTPIsReinsurerApproved, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", ScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                    ' Insert treaty party
                    lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertTreatyPartySQL, sSQLName:=ACInsertTreatyPartyName, bStoredProcedure:=ACInsertTreatyPartyStored)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add new treaty parties")
                    End If
                    lTreatyPartyId = ToSafeLong(m_oDatabase.Parameters.Item("treaty_party_id").Value)

                    If IsArray(v_vTreatyPartiesBrokerParticipants) Then
                        For lCountParticipant = 0 To UBound(v_vTreatyPartiesBrokerParticipants)
                            If v_vTreatyParties(MainModule.TreatyPartyEnum.DBTPPartyCnt, lCount) <> 0 Then
                                AddParameterLite(m_oDatabase, "participantontreaty_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
                                AddParameterLite(m_oDatabase, "treaty_id", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                AddParameterLite(m_oDatabase, "treaty_party_id", lTreatyPartyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                AddParameterLite(m_oDatabase, "associated_party_cnt", v_vTreatyPartiesBrokerParticipants(lCountParticipant, 3), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                AddParameterLite(m_oDatabase, "party_cnt", v_vTreatyPartiesBrokerParticipants(lCountParticipant, 4), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                AddParameterLite(m_oDatabase, "participant_percent", v_vTreatyPartiesBrokerParticipants(lCountParticipant, 5), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                lReturn = m_oDatabase.SQLAction(
                                                    sSQL:=ACInsertTreatyPartyBrokerParticipantSQL,
                                                    sSQLName:=ACInsertTreatyPartyBrokerParticipantName,
                                                    bStoredProcedure:=ACInsertTreatyPartyBrokerParticipantStored)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    RaiseError("m_oDatabase.SQLAction", "Unable to add new treaty parties broker participants")
                                End If
                            End If
                        Next
                    End If
                End If
            Next lCount
        End If

        ' Check share
        If Math.Round(dShare, 5) <> 100 Then
            gPMFunctions.RaiseError("Round(dShare, 4) <> 100", "Treaty party allocation is not 100%")
        End If

        Return result

    End Function

    Public Function GetTreatyEffectivePeriod(ByVal lTreatyId As Integer, ByRef vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetTreatyEffectivePeriod"

        Try

            bPMAddParameter.AddParameterLite(m_oDatabase, "Treaty_id", lTreatyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTreatyEffectivePeriodSQL, sSQLName:=ACGetTreatyEffectivePeriodName, bStoredProcedure:=ACGetTreatyEffectivePeriodStored, vResultArray:=vResult)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to select the Effective treaty period")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


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
    Public Function GetTreatyPartyBrokerParticipantList(
    ByVal v_lTreatyID As Long,
    ByRef r_vTreatyPartiesBrokerParticipant(,) As Object) As Long

        Dim lReturn As Long
        Const kMethodName As String = "GetTreatyPartyBrokerParticipantList"

        Dim dtResult As New DataTable
        GetTreatyPartyBrokerParticipantList = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()
            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", v_lTreatyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Call sql
            lReturn = m_oDatabase.SQLSelect(
                sSQL:=ACSelectTreatyPartyBrokerParticipantSQL,
                sSQLName:=ACSelectTreatyPartyBrokerParticipantName,
                bStoredProcedure:=ACSelectTreatyPartyBrokerParticipantStored,
                vResultArray:=r_vTreatyPartiesBrokerParticipant)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oDatabase.SQLSelect", "Unable to get treaty party list")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)
            GetTreatyPartyBrokerParticipantList = False
        Finally


        End Try
        Return lReturn
    End Function

End Class
