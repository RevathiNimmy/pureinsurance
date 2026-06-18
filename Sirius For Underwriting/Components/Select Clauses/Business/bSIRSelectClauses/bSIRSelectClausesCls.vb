Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no .129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    '***********************************************************************************************
    'Created By     :   Arul Stephen
    'Date           :   02-Sep-2008
    'History        :   It is a New File
    '***********************************************************************************************


    Private Const ACClass As String = "bSIRSelectClauses.Business"

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Properities
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property


    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"
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



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function


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


    Public Sub New()
        MyBase.New()

        Const kMethodName As String = "Class_Initialize"
        Try



            Dim vDatabase As Object

            ' Class Initialise
            m_oDatabase = New dPMDAO.Database()


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "gPMComponentServices.CheckDatabase  Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    '************************************************************************************************
    'Created By     :   Arul Stephen
    'Description    :   This method is used to delete the clauses that are not attached to any branches
    '***********************************************************************************************
    Public Function DelSelectedClausesProperties(ByVal v_lClauseType As Integer, ByVal v_lRisk_Type_Id As Integer, ByVal v_lProduct_Type_Id As Integer, ByVal v_vSelectedClauses(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "DelSelectedClauses"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue
            Dim lCount, lDefaultClause, lBranchesCount As Integer

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            BeginTrans()

            For lClauseCount As Integer = v_vSelectedClauses.GetLowerBound(kISelectClauseRowIndex - 1) To v_vSelectedClauses.GetUpperBound(kISelectClauseRowIndex - 1)
                If v_lClauseType = MainModule.EnClauseType.RiskType Then


                    bPMAddParameter.AddParameterLite(m_oDatabase, "document_Template_Id", CStr(v_vSelectedClauses(kIClauseId, lClauseCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Type_Id", CStr(v_lRisk_Type_Id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy & $"/Allowed Clause({CStr(v_vSelectedClauses(kIClauseCode, lClauseCount)).Trim()})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELRisktypeClausesSQL, sSQLName:=ACDELRiskTypeClausesSQLName, bStoredProcedure:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, ACDELRisktypeClausesSQL & " Failed to delete the unattached clauses", gPMConstants.PMELogLevel.PMLogError)
                    End If

                ElseIf (v_lClauseType = MainModule.EnClauseType.ProductType) Then


                    bPMAddParameter.AddParameterLite(m_oDatabase, "document_Template_Id", CStr(v_vSelectedClauses(kIClauseId, lClauseCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Product_Id", CStr(v_lProduct_Type_Id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy & $"/Allowed Clause({CStr(v_vSelectedClauses(kIClauseCode, lClauseCount)).Trim()})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELProductTypeClausesSQL, sSQLName:=ACDELProductTypeClausesSQLName, bStoredProcedure:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, ACDELProductTypeClausesSQL & " Failed to delete the unattached clauses", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If



            Next

            CommitTrans()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()

        Finally



        End Try
        Return result
    End Function

    '************************************************************************************************
    'Created By     :   Arul Stephen
    'Description    :   This method is used to fetch all the clauses that are attahced or not attached
    '                   with branches
    '*************************************************************************************************
    Public Function GetAllClauses(ByVal v_lClauseType As Integer, ByVal v_lRiskType As Integer, ByVal v_lProduct_id As Integer, ByRef r_vReturnValues(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRiskClauses"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            Dim vBranches As Object
            Dim lCount As Integer

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            If v_lClauseType = MainModule.EnClauseType.RiskType Then

                bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Type_Id", CStr(v_lRiskType), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELRiskTypeClausesSQL, sSQLName:=ACSELRiskTypeClausesSQLName, bStoredProcedure:=True, vResultArray:=r_vReturnValues, lNumberRecords:=gPMConstants.PMAllRecords)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACSELRiskTypeClausesSQL & " Failed to get the clauses", gPMConstants.PMELogLevel.PMLogError)
                End If

            ElseIf (v_lClauseType = MainModule.EnClauseType.ProductType) Then

                ' Add Required Stored Procedure Parameters
                bPMAddParameter.AddParameterLite(m_oDatabase, "Product_type_id", CStr(v_lProduct_id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELProductTypeClausesSQL, sSQLName:=ACSELProductTypeClausesSQLName, bStoredProcedure:=True, vResultArray:=r_vReturnValues, lNumberRecords:=gPMConstants.PMAllRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACSELRiskTypeClausesSQL & " Failed to get the clauses", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function
    '************************************************************************************************
    'Created By     :   Arul Stephen
    'Description    :   This method is used to fetch all Branches from Soruce Table
    '************************************************************************************************
    Public Function GetAllBranches(ByRef r_vReturnValues(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAllBranches"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            Dim vBranches As Object
            Dim lCount As Integer

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELAllBranchesSQL, sSQLName:=ACSELAllBranchesSQLSQLName, bStoredProcedure:=True, vResultArray:=r_vReturnValues)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELAllBranchesSQL & " Failed to fetch the branches", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    '************************************************************************************************
    'Created By     :   Arul Stephen
    'Description    :   This method is used to update the selected clauses along with its branches
    '************************************************************************************************
    Public Function UpdateSelectedClausesProperties(ByVal v_lClauseType As Integer, ByVal v_lRisk_Type_Id As Integer, ByVal v_lProduct_Type_Id As Integer, ByVal v_vSelectedClauses(,) As Object, ByVal v_vBranches(,) As Object, ByVal v_bDefaultClause As Boolean, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateSelectedClausesProperties"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lCount, lDefaultClause As Integer

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            '***********************************************************************************
            'Delete  the existing data from DB                                                 *
            '***********************************************************************************
            If v_bDefaultClause Then
                lDefaultClause = kIDefaultClauseExist
            Else
                lDefaultClause = kIDefaultClauseNotExist
            End If

            BeginTrans()

            For lClauseCount As Integer = v_vSelectedClauses.GetLowerBound(kISelectClauseRowIndex - 1) To v_vSelectedClauses.GetUpperBound(kISelectClauseRowIndex - 1)
                If v_lClauseType = MainModule.EnClauseType.RiskType Then


                    bPMAddParameter.AddParameterLite(m_oDatabase, "document_Template_Id", CStr(v_vSelectedClauses(kIClauseId, lClauseCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Type_Id", CStr(v_lRisk_Type_Id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy & $"/Allowed Clause({CStr(v_vSelectedClauses(kIClauseCode, lClauseCount)).Trim()})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELRisktypeClausesSQL, sSQLName:=ACDELRiskTypeClausesSQLName, bStoredProcedure:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, ACDELRisktypeClausesSQL & " Failed to delete clauses", gPMConstants.PMELogLevel.PMLogError)
                    End If

                ElseIf (v_lClauseType = MainModule.EnClauseType.ProductType) Then


                    bPMAddParameter.AddParameterLite(m_oDatabase, "document_Template_Id", CStr(v_vSelectedClauses(kIClauseId, lClauseCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Product_Id", CStr(v_lProduct_Type_Id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy & $"/Allowed Clause({CStr(v_vSelectedClauses(kIClauseCode, lClauseCount)).Trim()})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELProductTypeClausesSQL, sSQLName:=ACDELProductTypeClausesSQLName, bStoredProcedure:=True)

                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACDELRisktypeClausesSQL & " Failed to delete clauses", gPMConstants.PMELogLevel.PMLogError)
                End If
            Next


            '***********************************************************************************
            'SAVE the Data in to BB                                                            *
            '***********************************************************************************

            For lClauseCount As Integer = v_vSelectedClauses.GetLowerBound(kISelectClauseRowIndex - 1) To v_vSelectedClauses.GetUpperBound(kISelectClauseRowIndex - 1)
                For lBranchesCount As Integer = v_vBranches.GetLowerBound(kISelectClauseRowIndex - 1) To v_vBranches.GetUpperBound(kISelectClauseRowIndex - 1)
                    If v_lClauseType = MainModule.EnClauseType.RiskType Then

                        bPMAddParameter.AddParameterLite(m_oDatabase, "document_Template_Id", CStr(v_vSelectedClauses(kIClauseId, lClauseCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Type_Id", CStr(v_lRisk_Type_Id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "Branch_Id", CStr(v_vBranches(kIBranchId, lBranchesCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "default", CStr(lDefaultClause), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy & $"/Allowed Clause({CStr(v_vSelectedClauses(kIClauseCode, lClauseCount)).Trim()})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRisktypeClausesSQL, sSQLName:=ACADDRiskTypeClausesSQLName, bStoredProcedure:=True)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, ACAddRisktypeClausesSQL & " Failed to update ", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    ElseIf (v_lClauseType = MainModule.EnClauseType.ProductType) Then


                        bPMAddParameter.AddParameterLite(m_oDatabase, "document_Template_Id", CStr(v_vSelectedClauses(kIClauseId, lClauseCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "Product_Type_Id", CStr(v_lProduct_Type_Id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "Branch_Id", CStr(v_vBranches(kIBranchId, lBranchesCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "default", CStr(lDefaultClause), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy & $"/Allowed Clause({CStr(v_vSelectedClauses(kIClauseCode, lClauseCount)).Trim()})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddProductTypeClausesSQL, sSQLName:=ACADDProductTypeClausesSQLName, bStoredProcedure:=True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, ACAddProductTypeClausesSQL & " Failed to update", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If

                Next
            Next

            CommitTrans()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()

        Finally



        End Try
        Return result
    End Function
    '****************************************************************************************************
    'Created By     :   Arul Stephen
    'Description    :   This method is used to make sure whether all the selected clauses are attahced
    '                   to same branch or not
    '****************************************************************************************************

    Public Function GetSelectedClausesProperties(ByVal v_lClauseType As Integer, ByVal v_lRisk_Type_Id As Integer, ByVal v_lProduct_Type_Id As Integer, ByVal v_vSelectedClauses(,) As Object, ByRef r_vBranches(,) As Object, ByRef r_bDefaultClause As Boolean, ByRef r_bPropertiesNotSame As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSelectedClausesProperties"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lCount, lBranchesCount As Integer
            Dim vResultArray(,) As Object
            'Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()
            Dim iBranchCount As Integer
            Dim bFlag, bDefaultClauseCheck As Boolean
            bDefaultClauseCheck = True


            For lClauseCount As Integer = v_vSelectedClauses.GetLowerBound(kISelectClauseRowIndex - 1) To v_vSelectedClauses.GetUpperBound(kISelectClauseRowIndex - 1)
                If v_lClauseType = MainModule.EnClauseType.RiskType Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Type_Id", CStr(v_lRisk_Type_Id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "Code", CStr(v_vSelectedClauses(kIClauseCode, lClauseCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    'PM035567 
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Document_Template_Id", CStr(v_vSelectedClauses(kIClauseId, lClauseCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELRiskTypeLinkedClausesSelSQL, sSQLName:=ACSELRiskTypeLinkedClausesSelSQLName, bStoredProcedure:=True, vResultArray:=vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, ACSELRiskTypeLinkedClausesSelSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If



                    If Not Object.Equals(vResultArray, Nothing) And Information.IsArray(vResultArray) Then

                        If lClauseCount = kIClauseCountIndex Then
                            'Store the branches and the defaultClause values

                            ReDim r_vBranches(kIBranchId, kIBranchId)

                            For iBranchCount = vResultArray.GetLowerBound(kISelectClauseRowIndex - 1) To vResultArray.GetUpperBound(kISelectClauseRowIndex - 1)
                                ReDim Preserve r_vBranches(kIBranchId, iBranchCount)


                                r_vBranches(kIBranchId, iBranchCount) = vResultArray(kIBranchArrayCode, iBranchCount)
                            Next

                            r_bDefaultClause = CBool(vResultArray(kIDefaultClauseExistInArray, iBranchCount - 1))
                            m_lReturn = CType(gPMFunctions.ShellSort2DArray(r_vBranches, kIBranchId), gPMConstants.PMEReturnCode)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "ShellSort2DArray Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If


                        Else
                            bFlag = True

                            m_lReturn = CType(gPMFunctions.ShellSort2DArray(vResultArray, kIBranchArrayCode), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "ShellSort2DArray Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            'Check to see if the list of branches or the value of the default

                            If r_vBranches.GetUpperBound(kISelectClauseRowIndex - 1) = vResultArray.GetUpperBound(kISelectClauseRowIndex - 1) Then

                                For iBranchCount = vResultArray.GetLowerBound(kISelectClauseRowIndex - 1) To vResultArray.GetUpperBound(kISelectClauseRowIndex - 1)


                                    If Not vResultArray(kISelectClauseBranchIndex, iBranchCount).Equals(r_vBranches(kIBranchId, iBranchCount)) Then
                                        bFlag = False
                                        Exit For
                                    End If
                                    'Flag returned are different to the one stored
                                    If bDefaultClauseCheck Then

                                        If CBool(vResultArray(kIDefaultClauseExistInArray, iBranchCount)) <> r_bDefaultClause Then
                                            bDefaultClauseCheck = False
                                        End If
                                    End If
                                Next



                                If Not bFlag Then
                                    'Set the default flag to fasle
                                    r_bDefaultClause = bDefaultClauseCheck
                                    'Empty the branches array
                                    r_vBranches = VB6.CopyArray(Nothing)
                                    'Set the PropertiesNotSame flag to true
                                    r_bPropertiesNotSame = True
                                    Return result
                                Else
                                    r_bDefaultClause = bDefaultClauseCheck
                                    r_bPropertiesNotSame = False
                                End If

                            Else
                                r_bDefaultClause = bDefaultClauseCheck
                                r_vBranches = VB6.CopyArray(Nothing)
                                r_bPropertiesNotSame = True
                                Return result
                            End If

                        End If
                    Else
                        r_bDefaultClause = False
                        r_vBranches = VB6.CopyArray(Nothing)
                        r_bPropertiesNotSame = True
                        Return result
                    End If

                Else
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Product_id", CStr(v_lProduct_Type_Id), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "Code", CStr(v_vSelectedClauses(kIClauseCode, lClauseCount)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELProductTypeLinkedClausesSelSQL, sSQLName:=ACSELProductTypeLinkedClausesSelSQLName, bStoredProcedure:=True, vResultArray:=vResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, ACSELProductTypeLinkedClausesSelSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Not Object.Equals(vResultArray, Nothing) And Information.IsArray(vResultArray) Then


                        If lClauseCount = kIClauseCountIndex Then

                            ReDim r_vBranches(kIBranchId, kIBranchId)

                            For iBranchCount = vResultArray.GetLowerBound(kISelectClauseRowIndex - 1) To vResultArray.GetUpperBound(kISelectClauseRowIndex - 1)
                                ReDim Preserve r_vBranches(kIBranchId, iBranchCount)


                                r_vBranches(kIBranchId, iBranchCount) = vResultArray(kIBranchArrayCode, iBranchCount)
                            Next

                            r_bDefaultClause = CBool(vResultArray(kIDefaultClauseExistInArray, iBranchCount - 1))
                            m_lReturn = CType(gPMFunctions.ShellSort2DArray(r_vBranches, kIBranchId), gPMConstants.PMEReturnCode)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "DataToInterface Failed", gPMConstants.PMELogLevel.PMLogError)
                                End If
                            End If

                        Else
                            bFlag = True

                            m_lReturn = CType(gPMFunctions.ShellSort2DArray(vResultArray, kISelectClauseSortIndex), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "DataToInterface Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            'Check to see if the list of branches or the value of the default

                            If r_vBranches.GetUpperBound(kISelectClauseRowIndex - 1) = vResultArray.GetUpperBound(kISelectClauseRowIndex - 1) Then

                                For iBranchCount = vResultArray.GetLowerBound(kISelectClauseRowIndex - 1) To vResultArray.GetUpperBound(kISelectClauseRowIndex - 1)


                                    If Not vResultArray(kISelectClauseBranchIndex, iBranchCount).Equals(r_vBranches(kIBranchId, iBranchCount)) Then
                                        bFlag = False
                                        Exit For
                                    End If
                                    'Flag returned are different to the one stored
                                    If bDefaultClauseCheck Then

                                        If CBool(vResultArray(kIDefaultClauseExistInArray, iBranchCount)) <> r_bDefaultClause Then
                                            bDefaultClauseCheck = False
                                        End If
                                    End If
                                Next
                                If Not bFlag Then
                                    'Set the default flag to fasle
                                    r_bDefaultClause = bDefaultClauseCheck
                                    'Empty the branches array
                                    r_vBranches = VB6.CopyArray(Nothing)
                                    'Set the PropertiesNotSame flag to true
                                    r_bPropertiesNotSame = True
                                    Return result
                                Else
                                    r_bDefaultClause = bDefaultClauseCheck
                                    r_bPropertiesNotSame = False
                                End If

                            Else
                                r_bDefaultClause = bDefaultClauseCheck
                                r_vBranches = VB6.CopyArray(Nothing)
                                r_bPropertiesNotSame = True
                                Return result
                            End If

                        End If
                    Else
                        r_bDefaultClause = False
                        r_vBranches = VB6.CopyArray(Nothing)
                        r_bPropertiesNotSame = True
                        Return result
                    End If
                End If
            Next

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CommitTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oDatabase.SQLCommitTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BeginTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oDatabase.SQLBeginTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLBeginTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RollbackTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oDatabase.SQLRollbackTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLRollbackTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function
End Class
