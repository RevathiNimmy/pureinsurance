Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private Const ACClass As String = "Business"
    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean
    Private m_lError As gPMConstants.PMEReturnCode
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTypeOfBusiness As New FixedLengthString(10)
    ' Effective
    Private m_dtEffectiveDate As Date
    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' Component Sub Type
    Private m_sSubType As New FixedLengthString(20)
    ' Variable Data Business Component (Private)

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TypeOfBusiness() As String
        Get

            Return m_sTypeOfBusiness.Value

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    ' PUBLIC Property Procedures (End)

    ' PUBLIC Method (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            '
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

            ' Set User ID
            m_iUserID = iUserID


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Initialise", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
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


    Public Function GetRenewalJobs(ByVal v_vBatch_Renewal_Job_Id As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRenewalJobs"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            If v_vBatch_Renewal_Job_Id.Equals(DBNull.Value) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_id", vValue:=CStr(v_vBatch_Renewal_Job_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter batch_renewal_job_id")
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACBatchRenewalJobQuerySQL, sSQLName:=ACBatchRenewalJobQueryName, bStoredProcedure:=ACBatchRenewalJobQueryStored, vResultArray:=r_vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListLoad
    '
    ' Description: Standard method for the PickList control to call
    '
    ' History: 16/05/2008 PK Created.
    '
    ' ***************************************************************** '
    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "PickListLoad"

        Dim iParam, iBatchRenewalJobId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the value of BatchRenewalJobId
            iBatchRenewalJobId = gPMFunctions.ToSafeInteger(vFKArray(1, 0))

            'Clear the parameter list
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRenewalJobId", vValue:=CStr(iBatchRenewalJobId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            If sPickListType = "Batch_Renewal_Job_Products" Then
                'Call sp and populate the linked products in vResultArray
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductsListForPickListSQL, sSQLName:=ACGetProductsListForPickListName, bStoredProcedure:=ACGetProductsListForPickListStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

                'Check for any error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(ACClass, kMethod & " Fails to get products linked with Batch Renewal Jobs ", gPMConstants.PMELogLevel.PMLogError)
                End If
            ElseIf sPickListType = "Batch_Renewal_Job_Branches" Then
                'Call sp and populate the linked sources in vResultArray
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSourcesListForPickListSQL, sSQLName:=ACGetSourcesListForPickListName, bStoredProcedure:=ACGetSourcesListForPickListStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

                'Check for any error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(ACClass, kMethod & " Fails to get products linked with Batch Renewal Jobs ", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListSave
    '
    ' Description: Standard method for the PickList control to call
    '
    ' History: 16/05/2008 PK Created.
    '
    ' ***************************************************************** '
    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys() As Object) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "PickListSave"

        Try

            Dim iBatchRenewalJobId, iProductID, iSourceID As Integer
            Dim lRecordsAffected As Integer



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            iBatchRenewalJobId = CInt(vFKArray(1, 0))

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRenewalJobId", vValue:=CStr(iBatchRenewalJobId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            If sPickListType = "Batch_Renewal_Job_Products" Then
                'Call sp and populate the linked sources in vResultArray
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelProductsLinkedWithBatchRenewalSQL, sSQLName:=ACDelProductsLinkedWithBatchRenewalName, bStoredProcedure:=ACDelProductsLinkedWithBatchRenewalStored, lRecordsAffected:=lRecordsAffected)


                If Not Information.IsArray(vKeys) Then

                    'Clear Parameters
                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRenewalJobId", vValue:=CStr(iBatchRenewalJobId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    'Check for any error
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

                For Each vKeys_item As Object In vKeys

                    iProductID = gPMFunctions.ToSafeInteger(vKeys_item)

                    'Begin the Transaction
                    BeginTrans()

                    'Clear Parameters
                    m_oDatabase.Parameters.Clear()

                    'Adding Param
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRenewalJobId", vValue:=CStr(iBatchRenewalJobId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    'Adding Param
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Product_id", vValue:=CStr(iProductID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


                    'Check for any error
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    'Call sp
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddProductsLinkedWithBatchRenewalSQL, sSQLName:=ACAddProductsLinkedWithBatchRenewalName, bStoredProcedure:=ACAddProductsLinkedWithBatchRenewalStored, lRecordsAffected:=lRecordsAffected)

                    'Check for any error
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add products to the Batch Renewal", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Commit the Transaction
                    CommitTrans()
                Next vKeys_item

            ElseIf sPickListType = "Batch_Renewal_Job_Branches" Then
                'Call sp and populate the linked sources in vResultArray
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelBranchesLinkedWithBatchRenewalSQL, sSQLName:=ACDelBranchesLinkedWithBatchRenewalName, bStoredProcedure:=ACDelBranchesLinkedWithBatchRenewalStored, lRecordsAffected:=lRecordsAffected)


                If Not Information.IsArray(vKeys) Then

                    'Clear Parameters
                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRenewalJobId", vValue:=CStr(iBatchRenewalJobId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    'Check for any error
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

                For Each vKeys_item_2 As Object In vKeys

                    iSourceID = gPMFunctions.ToSafeInteger(vKeys_item_2)

                    'Begin the Transaction
                    BeginTrans()

                    'Clear Parameters
                    m_oDatabase.Parameters.Clear()

                    'Adding Param
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRenewalJobId", vValue:=CStr(iBatchRenewalJobId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    'Adding Param
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_Id", vValue:=CStr(iSourceID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


                    'Check for any error
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    'Call sp
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddBranchesLinkedWithBatchRenewalSQL, sSQLName:=ACAddBranchesLinkedWithBatchRenewalName, bStoredProcedure:=ACAddBranchesLinkedWithBatchRenewalStored, lRecordsAffected:=lRecordsAffected)

                    'Check for any error
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add products to the Batch Renewal", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Commit the Transaction
                    CommitTrans()
                Next vKeys_item_2

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()

        Finally

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function GetJobCode(ByVal v_vBatch_Renewal_Job_Type As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetJobCode"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_type", vValue:=CStr(v_vBatch_Renewal_Job_Type), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter batch_renewal_job_type")
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetJobCodeSQL, sSQLName:=ACGetJobCodeName, bStoredProcedure:=ACGetJobCodeStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function DirectAdd(ByVal v_vCode As Object, ByVal v_vDescription As Object, ByVal v_vSAMServer As Object, ByVal v_vDaysBeforeRenewalDate As Object, ByVal v_vIsActive As Object, ByVal v_vBatchRenewalJobTypeId As Object, ByVal v_vRenewalDocsDestination As Object, ByVal v_vReportSortOrder As Object, ByVal v_vAllAgents As Object, ByVal v_vPMUserId As Object, ByVal v_vIncludeDirectPolicies As Object, ByRef r_vResultArray(,) As Object, Optional ByVal v_bRunExtendedRule As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DirectAdd"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(v_vCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter code")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(v_vDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter description")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="sam_server", vValue:=CStr(v_vSAMServer), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter sam_server")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="days_before_renewal_date", vValue:=CStr(v_vDaysBeforeRenewalDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter days_before_renewal_date")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_active", vValue:=CStr(v_vIsActive), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter is_active")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_type_id", vValue:=CStr(v_vBatchRenewalJobTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter batch_renewal_job_type")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_docs_destination", vValue:=CStr(v_vRenewalDocsDestination), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter renewal_docs_destination")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="report_sort_order", vValue:=CStr(v_vReportSortOrder), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter report_sort_order")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="all_agents", vValue:=CStr(v_vAllAgents), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter all_agents")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=CStr(v_vPMUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter pmuser_id")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="include_direct_policies", vValue:=CStr(v_vIncludeDirectPolicies), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter include_direct_policies")
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="run_extended_rule", vValue:=v_bRunExtendedRule, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter run_extended_rule")
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAddBatchRenewalJobSQL, sSQLName:=ACAddBatchRenewalJobName, bStoredProcedure:=ACAddBatchRenewalJobStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSave failed")
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    Public Function AddAgent(ByRef iBatchRenewalJobId As Integer, ByRef vKeys() As Object) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "AddAgent"
        Dim lRecordsAffected, lPartyCnt As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRenewalJobId", vValue:=CStr(iBatchRenewalJobId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Call sp and populate the linked sources in vResultArray
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelAgentsLinkedWithBatchRenewalSQL, sSQLName:=ACDelAgentsLinkedWithBatchRenewalName, bStoredProcedure:=ACDelAgentsLinkedWithBatchRenewalStored, lRecordsAffected:=lRecordsAffected)

            If Information.IsArray(vKeys) Then
                'Begin the Transaction
                BeginTrans()
                For Each vKeys_item As Object In vKeys

                    lPartyCnt = gPMFunctions.ToSafeLong(vKeys_item)

                    'Clear Parameters
                    m_oDatabase.Parameters.Clear()

                    'Adding Param
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRenewalJobId", vValue:=CStr(iBatchRenewalJobId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    'Adding Param
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Cnt", vValue:=CStr(lPartyCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                    'Check for any error
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    'Call sp
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddAgentsLinkedWithBatchRenewalSQL, sSQLName:=ACAddAgentsLinkedWithBatchRenewalName, bStoredProcedure:=ACAddAgentsLinkedWithBatchRenewalStored, lRecordsAffected:=lRecordsAffected)

                    'Check for any error
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add Agents to the Batch Renewal", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Next vKeys_item
                'Commit the Transaction
                CommitTrans()
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()

        Finally

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function DirectDelete(ByVal iBatchRenewalJobId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "DirectDelete"
        Dim lRecordsAffected As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_id", vValue:=CStr(iBatchRenewalJobId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Call sp and populate the linked sources in vResultArray
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelBatchRenewalSQL, sSQLName:=ACDelBatchRenewalName, bStoredProcedure:=ACDelBatchRenewalStored, lRecordsAffected:=lRecordsAffected)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to Delete Batch Renewal Record", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()

        Finally

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iBatchRenewalJobId"></param>
    ''' <param name="v_iIsActive"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SuspendJobs(ByRef v_iBatchRenewalJobId As Integer, Optional ByVal v_iIsActive As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "SuspendJobs"
        Dim lRecordsAffected As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRenewalJobId", vValue:=(v_iBatchRenewalJobId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_active", vValue:=(v_iIsActive), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Call sp
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSuspendJobSQL, sSQLName:=ACSuspendJobName, bStoredProcedure:=ACSuspendJobStored, lRecordsAffected:=lRecordsAffected)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to Suspend Job of Batch Renewal", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Commit the Transaction
            CommitTrans()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function GetBusiness(ByVal v_vBatch_Renewal_Job_Id As Object, ByRef r_vResultArray(,) As Object, ByRef r_vResultAgentArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_id", vValue:=CStr(v_vBatch_Renewal_Job_Id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter batch_renewal_job_id")
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACBatchRenewalJobQuerySQL, sSQLName:=ACBatchRenewalJobQueryName, bStoredProcedure:=ACBatchRenewalJobQueryStored, vResultArray:=r_vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_id", vValue:=CStr(v_vBatch_Renewal_Job_Id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter batch_renewal_job_id")
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAgentSelSQL, sSQLName:=ACAgentSelName, bStoredProcedure:=ACAgentSelStored, vResultArray:=r_vResultAgentArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function DirectUpdate(ByVal v_vBatchRenewalJobId As Object, ByVal v_vCode As Object, ByVal v_vDescription As Object, ByVal v_vSAMServer As Object, ByVal v_vDaysBeforeRenewalDate As Object, ByVal v_vIsActive As Object, ByVal v_vBatchRenewalJobTypeId As Object, ByVal v_vRenewalDocsDestination As Object, ByVal v_vReportSortOrder As Object, ByVal v_vAllAgents As Object, ByVal v_vPMUserId As Object, ByVal v_vIncludeDirectPolicies As Object, Optional ByVal v_bRunExtendedRule As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DirectUpdate"
        Dim lRecordsAffected As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_id", vValue:=CStr(v_vBatchRenewalJobId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter code")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(v_vCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter code")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(v_vDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter description")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="sam_server", vValue:=CStr(v_vSAMServer), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter sam_server")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="days_before_renewal_date", vValue:=CStr(v_vDaysBeforeRenewalDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter days_before_renewal_date")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_active", vValue:=CStr(v_vIsActive), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter is_active")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_type_id", vValue:=CStr(v_vBatchRenewalJobTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter batch_renewal_job_type")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_docs_destination", vValue:=CStr(v_vRenewalDocsDestination), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter renewal_docs_destination")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="report_sort_order", vValue:=CStr(v_vReportSortOrder), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter report_sort_order")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="all_agents", vValue:=CStr(v_vAllAgents), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter all_agents")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=CStr(v_vPMUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter pmuser_id")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="include_direct_policies", vValue:=CStr(v_vIncludeDirectPolicies), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter include_direct_policies")
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="run_extended_rule", vValue:=v_bRunExtendedRule, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter run_extended_rule")
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateBatchRenewalJobQuerySQL, sSQLName:=ACUpdateBatchRenewalJobQueryName, bStoredProcedure:=ACUpdateBatchRenewalJobQueryStored, lRecordsAffected:=lRecordsAffected)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLUpdate failed")
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' GetRenewalConfigurationResults
    ''' </summary>
    ''' <param name="v_sBatchRenewalJobCode"></param>
    ''' <param name="v_iBatchRenewalJobId"></param>
    ''' <param name="r_vResultArray"></param>
    ''' <param name="v_sUserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRenewalConfigurationResults(ByVal v_sBatchRenewalJobCode As String,
                                                   ByVal v_iBatchRenewalJobId As Integer,
                                                   ByRef r_vResultArray(,) As Object,
                                                   Optional ByVal v_sUserName As String = "") As Integer

        Dim nResult As Integer
        Const kMethodName As String = "GetRenewalConfigurationResults"

        Try


            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Batch_Renewal_Job_Code", vValue:=v_sBatchRenewalJobCode,
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                    iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter batch_renewal_job_id")
            End If
            If v_sUserName <> "" Then
                m_lReturn = m_oDatabase.Parameters.Add(
                            sName:="userName",
                            vValue:=v_sUserName,
                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                            iDataType:=gPMConstants.PMEDataType.PMString)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "Failed to Add parameter username")
                End If
            End If
            Select Case v_iBatchRenewalJobId
                Case kAcceptance
                    'Execute SQL Statement
                    m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetBatchRenewalJobAcceptanceQuerySQL,
                                                        sSQLName:=ACGetBatchRenewalJobAcceptanceQueryName,
                                                        bStoredProcedure:=ACGetBatchRenewalJobAcceptanceQueryStored,
                                                        vResultArray:=r_vResultArray)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                    End If
                Case kInvitation
                    'Execute SQL Statement
                    m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetBatchRenewalJobInvitationQuerySQL,
                                                        sSQLName:=ACGetBatchRenewalJobInvitationQueryName,
                                                        bStoredProcedure:=ACGetBatchRenewalJobInvitationQueryStored,
                                                        vResultArray:=r_vResultArray)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                    End If
                Case kSelection
                    'Execute SQL Statement
                    m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetBatchRenewalJobSelectionQuerySQL,
                                                        sSQLName:=ACGetBatchRenewalJobSelectionQueryName,
                                                        bStoredProcedure:=ACGetBatchRenewalJobSelectionQueryStored,
                                                        vResultArray:=r_vResultArray)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                    End If
            End Select


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        Finally

        End Try
        Return nResult
    End Function

    Public Function CheckTwoActiveConfigurations(ByVal v_sBatchRenewalJobCode As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodNAME As String = "CheckTwoActiveConfigurations"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="batchrenewaljobcode", vValue:=v_sBatchRenewalJobCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodNAME, "Failed to Add parameter batchrenewaljobcode")
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetActiveBatchRenewalJobInvitationQuerySQL, sSQLName:=ACGetActiveBatchRenewalJobInvitationQueryName, bStoredProcedure:=ACGetActiveBatchRenewalJobInvitationQueryStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodNAME, "m_oDatabase.SQLSelect failed")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodNAME, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function
    ' PUBLIC Method (End)

    ' PRIVATE Methods (Start)

    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise


        Catch ex As Exception

            ' Error Section.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally



        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub




    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Const kMethod As String = "BeginTrans"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Const kMethod As String = "CommitTrans"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

        Finally
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Const kMethod As String = "RollbackTrans"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ' PRIVATE Methods (End)
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
