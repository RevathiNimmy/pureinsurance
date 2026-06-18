Option Strict Off
Option Explicit On

Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports SharedFiles

Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    Private Const ACClass As String = "Business"

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

#Region "Private Variables"

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_nUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_nSourceID As Integer
    Private m_nLanguageID As Integer
    Private m_nCurrencyID As Integer
    Private m_nLogLevel As Integer

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Process Mode Properties
    Private m_nProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private nPMAuthorityLevel As Integer
    Private bDisposedValue As Boolean

#End Region

#Region "Standard Methods"

    ''' <summary>
    ''' Entry point for any initialisation code for this object
    ''' </summary>
    ''' <param name="sUsername"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="iUserID"></param>
    ''' <param name="iSourceID"></param>
    ''' <param name="iLanguageID"></param>
    ''' <param name="iCurrencyID"></param>
    ''' <param name="iLogLevel"></param>
    ''' <param name="sCallingAppName"></param>
    ''' <param name="bStandAlone"></param>
    ''' <param name="vDatabase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_nUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_nLanguageID = iLanguageID
            m_nSourceID = iSourceID
            m_nCurrencyID = iCurrencyID
            m_nLogLevel = iLogLevel

            nResult = gPMComponentServices.CheckDatabase(m_sUsername, m_nSourceID, m_nLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_nProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

        Catch Excep As Exception
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Entry point for any termination code for this object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Sub Dispose(disposing As Boolean)
        If Not Me.bDisposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.bDisposedValue = True
    End Sub

#End Region

#Region "MID Rules Management"

    ''' <summary>
    ''' Get the MID rule Details from Database
    ''' </summary>
    ''' <param name="nSourceID"></param>
    ''' <param name="r_aoResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMIDRules(ByVal nSourceID As Integer, ByRef r_aoResultArray(,) As Object) As Integer
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.SQLSelect(sSQL:=kGetMIDRuleDetailsSQL, sSQLName:=kGetMIDRuleDetailsName, bStoredProcedure:=True, vResultArray:=r_aoResultArray)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue And nResult <> PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kGetMIDRuleDetailsSQL, kGetMIDRuleDetailsName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        Catch Excep As Exception
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMIDRules Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMIDRules", excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Add or Edit the MID rule Details
    ''' </summary>
    ''' <param name="nSourceID"></param>
    ''' <param name="nMIDRuleID"></param>
    ''' <param name="sCode"></param>
    ''' <param name="sDescription"></param>
    ''' <param name="dtEffectiveDate"></param>
    ''' <param name="dtStartDate"></param>
    ''' <param name="dtExpiryDate"></param>
    ''' <param name="sMIDType"></param>
    ''' <param name="nSupplierTypeId"></param>
    ''' <param name="nSupplierid"></param>
    ''' <param name="nInsurerId"></param>
    ''' <param name="nDelegatedAuthorityID"></param>
    ''' <param name="nSiteNumber"></param>
    ''' <param name="sDABranchId"></param>
    ''' <param name="nPMUserGroupId"></param>
    ''' <param name="nPMwrkTaskGroupid"></param>
    ''' <param name="sFilename"></param>
    ''' <param name="nTestIndicator"></param>
    ''' <param name="sFileSeqNumStart"></param>
    ''' <param name="sCurrentFileSeqNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddorEditMIDRule(ByVal nSourceID As Integer, ByVal nMIDRuleID As Integer, ByVal sCode As String, ByVal sDescription As String, ByVal dtEffectiveDate As DateTime, ByVal dtStartDate As DateTime, _
                                ByVal dtExpiryDate As DateTime, ByVal sMIDType As String, ByVal nSupplierTypeId As Integer, ByVal nSupplierid As Integer, _
                                ByVal nInsurerId As Integer, ByVal nDelegatedAuthorityID As Integer, ByVal nSiteNumber As Integer, _
                                ByVal nPMUserGroupId As Integer, ByVal nPMwrkTaskGroupid As Integer, sFilename As String, _
                                ByVal nTestIndicator As Integer, ByVal sFileSeqNumStart As String, ByVal sCurrentFileSeqNum As String) As Integer

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add(sName:="Code", vValue:=sCode, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            m_oDatabase.Parameters.Add(sName:="Description", vValue:=sDescription, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            m_oDatabase.Parameters.Add(sName:="Effective_Date", vValue:=dtEffectiveDate, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMDate)
            m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=dtStartDate, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMDate)
            m_oDatabase.Parameters.Add(sName:="Expiry_Date", vValue:=dtExpiryDate, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMDate)
            m_oDatabase.Parameters.Add(sName:="MID_Type", vValue:=sMIDType, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            m_oDatabase.Parameters.Add(sName:="Supplier_Type_id", vValue:=nSupplierTypeId, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add(sName:="Supplier_id", vValue:=nSupplierid, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add(sName:="Insurer_id", vValue:=nInsurerId, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add(sName:="Delegated_Authority_id", vValue:=IIf(nDelegatedAuthorityID = 0, DBNull.Value, nDelegatedAuthorityID), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add(sName:="Site_Number", vValue:=IIf(nSiteNumber = 0, DBNull.Value, nSiteNumber), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add(sName:="PMUser_Group_id", vValue:=nPMUserGroupId, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add(sName:="PMwrk_Task_Group_id", vValue:=nPMwrkTaskGroupid, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add(sName:="FileName", vValue:=sFilename, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            m_oDatabase.Parameters.Add(sName:="Test_Indicator", vValue:=nTestIndicator, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMBoolean)
            m_oDatabase.Parameters.Add(sName:="File_Seq_Num_Start", vValue:=sFileSeqNumStart, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

            If (nMIDRuleID > 0) Then
                m_oDatabase.Parameters.Add(sName:="MID_Rule_id", vValue:=nMIDRuleID, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            Else

                m_oDatabase.Parameters.Add(sName:="Current_File_Seq_Num", vValue:=sCurrentFileSeqNum, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
                m_oDatabase.Parameters.Add(sName:="SourceID", vValue:=nSourceID, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            End If

            If (nMIDRuleID > 0) Then
                'Editing the Existing Rule
                nResult = m_oDatabase.SQLAction(sSQL:=kUpdateMIDRuleSQL, sSQLName:=kUpdateMIDRuleName, bStoredProcedure:=True)
            Else
                'Adding New Rule
                nResult = m_oDatabase.SQLAction(sSQL:=kAddMIDRuleSQL, sSQLName:=kAddMIDRuleName, bStoredProcedure:=True)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kAddMIDRuleName, kAddMIDRuleSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

        Catch Excep As Exception
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddEditMIDRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteMIDRule", excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try
        Return nResult

    End Function

    ''' <summary>
    ''' Delete MID rule Details
    ''' </summary>
    ''' <param name="nMIDRuleId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteMIDRule(ByVal nMIDRuleId As Integer) As Integer

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add(sName:="MID_Rule_id", vValue:=nMIDRuleId, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'execute SQL statement
            nResult = m_oDatabase.SQLAction(sSQL:=kDeleteMIDRuleSQL, sSQLName:=kDeleteMIDRuleName, bStoredProcedure:=True)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kDeleteMIDRuleName, kDeleteMIDRuleSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch Excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteMIDRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteMIDRule", excep:=Excep)
        Finally
        End Try
        Return nResult

    End Function

    ''' <summary>
    ''' UnDelete MID rule Details
    ''' </summary>
    ''' <param name="nMIDRuleId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UnDeleteMIDRule(ByVal nMIDRuleId As Integer) As Integer

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add(sName:="MID_Rule_id", vValue:=nMIDRuleId, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'execute SQL statement
            nResult = m_oDatabase.SQLSelect(sSQL:=kUnDeleteMIDRuleSQL, sSQLName:=kUnDeleteMIDRuleName, bStoredProcedure:=True)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kUnDeleteMIDRuleSQL, kUnDeleteMIDRuleName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch Excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnDeleteMIDRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnDeleteMIDRule", excep:=Excep)
        End Try
        Return nResult
    End Function

#End Region

#Region "Lookup Values"

    ''' <summary>
    ''' Get the lookup Values
    ''' </summary>
    ''' <param name="v_vLookupTables"></param>
    ''' <param name="r_vLookupDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLookupValues(ByRef v_vLookupTables(,) As Object, ByRef r_vLookupDetails(,) As Object) As Integer
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim oLookup As bPMLookup.Business = Nothing
        Const kMethodName As String = "GetLookupValues"
        Try
            ' create instance of lookup object
            oLookup = New bPMLookup.Business
            nResult = oLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_nUserID, iSourceID:=m_nSourceID, iLanguageID:=m_nLanguageID, iCurrencyID:=m_nCurrencyID, iLogLevel:=m_nLogLevel, sCallingAppName:=m_sCallingAppName)

            ' get the lookup details for the specified
            nResult = oLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=v_vLookupTables, iLanguageID:=m_nLanguageID, dtEffectiveDate:=DateTime.Today, vResultArray:=r_vLookupDetails)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bPMLookup.Business.GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch Excep As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=Excep)
            nResult = PMEReturnCode.PMError
        Finally
            oLookup.Dispose()
            oLookup = Nothing
        End Try
        Return nResult

    End Function

    ''' <summary>
    ''' Get all work TaskGroup Tasks
    ''' </summary>
    ''' <param name="r_oaResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetALLPMWrkTaskGroupTasks(ByRef r_oaResults(,) As Object) As Integer
        Const kMethodName As String = "GetALLPMWrkTaskGroupTasks"
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()
            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(sSQL:=kGetALLPMWrkTaskGroupTasksSQL, sSQLName:=kGetALLPMWrkTaskGroupTasksName, bStoredProcedure:=True, vResultArray:=r_oaResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kGetALLPMWrkTaskGroupTasksSQL, "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch Excep As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn, excep:=Excep)
            nReturn = PMEReturnCode.PMError
        End Try

        Return nReturn
    End Function

    ''' <summary>
    ''' Get all work task user groups
    ''' </summary>
    ''' <param name="r_aoResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetALLPMWrkTaskGroupPMUserGroups(ByRef r_aoResults(,) As Object) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "GetALLPMWrkTaskGroupPMUserGroups"
        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            nResult = m_oDatabase.SQLSelect(sSQL:=kGetALLPMWrkTaskGroupPMUserGroupsSQL, sSQLName:=kGetALLPMWrkTaskGroupPMUserGroupsName, bStoredProcedure:=True, vResultArray:=r_aoResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kGetALLPMWrkTaskGroupPMUserGroupsSQL, "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        Catch Excep As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=Excep)
            nResult = PMEReturnCode.PMError
        End Try

        Return nResult

    End Function

#End Region

End Class
